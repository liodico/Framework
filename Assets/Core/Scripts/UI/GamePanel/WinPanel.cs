using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Inspector;
using Utilities.Components;
using System;
using Spine.Unity;
using Utilities.Common;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace FoodZombie.UI
{
    public class WinPanel : MyGamesBasePanel
    {
        [SerializeField, Tooltip("Buildin Pool")] public List<RewardView> rewardViewsPool;
        public Transform rewardViewsGrid;

        public TextMeshProUGUI txtGoldRW;
        public TextMeshProUGUI txtBrainRW;

        public SimpleTMPButton btnHome;
        public SimpleTMPButton btnVideo;
        public GameObject imgAds;

        //public GameObject efectStar1;
        //public GameObject efectStar2;
        //public GameObject efectStar3;
        public SkeletonGraphic fxModel;
        public SkeletonGraphic fxModelSafe;
        public SkeletonGraphic fxModelGold;
        public SkeletonGraphic fxModelBrain;

        private int totalGold;

        private void Start()
        {
            btnHome.onClick.AddListener(BtnHome_Pressed);
            btnVideo.onClick.AddListener(BtnWatchAds_Pressed);
        }

        public void Init(List<RewardInfo> rewards, int indexBonusCount)
        {
            Lock(true);

            rewardViewsPool.Free();

            fxModel.AnimationState.SetAnimation(0, "Victory_start", false);
            fxModel.AnimationState.AddAnimation(0, "Victory_idle", true, 0f);
            fxModelGold.AnimationState.SetAnimation(0, "Coins_fly", false);
            fxModelGold.AnimationState.AddAnimation(0, "Coins_idle", true, 0f);
            fxModelBrain.Initialize(false);
            fxModelBrain.AnimationState.SetAnimation(0, "Brain_fly", false);
            fxModelBrain.AnimationState.AddAnimation(0, "Brain_idle", true, 0f);

            if ((GameData.Instance.MissionsGroup.CountMissionWin + 1) == 2)
            {
                imgAds.SetActive(false);
            }
            else
            {
                Config.LogEvent(TrackingConstants.EVENT_WIN_X3_SHOW);
                imgAds.SetActive(true);
            }

            totalGold = 0;
            foreach (RewardInfo item in rewards)
            {
                if (item.Type == IDs.REWARD_TYPE_CURRENCY && item.Id == IDs.CURRENCY_COIN)
                {
                    totalGold += item.Value;//tính tổng gold thưởng thôi, tạm vậy
                }
                else if (item.Type == IDs.REWARD_TYPE_UNLOCK_CHARACTER)
                {
                    GameData.Instance.HeroesGroup.RemainShowClaimHero(item.Id);
                }
                else
                {
                    LogicAPI.ClaimReward(item);
                    var rewardView = rewardViewsPool.Obtain(rewardViewsGrid);
                    rewardView.Init(item);
                    rewardView.SetActive(true);
                }
            }
            GameData.Instance.CurrenciesGroup.Add(IDs.CURRENCY_COIN, totalGold);

            if (indexBonusCount > 0)
            {
                txtBrainRW.text = indexBonusCount + "";
                txtBrainRW.transform.parent.SetActive(true);
            }
            float goldFX = 0;
            DOTween.To(tweenVal => goldFX = tweenVal, 0, totalGold, 1f)
                    .OnUpdate(() =>
                    {
                        txtGoldRW.text = "" + goldFX.ToString("0");

                    })
                    .OnComplete(() =>
                    {
                        txtGoldRW.text = "" + totalGold;
                        if (indexBonusCount > 0) CombieBrain(indexBonusCount);
                    })
                    .SetUpdate(true);

            //efectStar1.SetActive(true);
        }

        private void CombieBrain(int indexBonusCount)
        {
            txtGoldRW.transform.parent.DOMoveX(0f, 0.3f).SetUpdate(true).SetDelay(0.5f);
            txtBrainRW.transform.parent.DOMoveX(0f, 0.3f).OnComplete(() =>
            {
                txtBrainRW.transform.parent.SetActive(false);
                //efectStar2.SetActive(true);
            })
                    .SetUpdate(true).SetDelay(0.5f);

            float goldFX = totalGold;
            int totalGoldNew = totalGold + LogicAPI.GoldBonus(indexBonusCount);
            DOTween.To(tweenVal => goldFX = tweenVal, totalGold, totalGoldNew, 1f)
                    .OnUpdate(() =>
                    {
                        txtGoldRW.text = "" + goldFX.ToString("0");

                    })
                    .OnComplete(() =>
                    {
                        txtGoldRW.text = "" + totalGoldNew;
                    })
                    .SetUpdate(true).SetDelay(1.5f);
        }

        private void OnEnable()
        {
            btnHome.SetActive(false);
            if ((GameData.Instance.MissionsGroup.CountMissionWin + 1) != 2) StartCoroutine(IEShowBtnMenu());
        }

        private IEnumerator IEShowBtnMenu()
        {
            yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(2f));
            btnHome.SetActive(true);
        }

        private void BtnHome_Pressed()
        {
            //nếu không show chest ở main menu thì show ads popup
            //nếu show chest ở main menu thì ấn back ở show chest sẽ gọi ads popup
            MainMenuPanel.canShowChest = LogicAPI.CanShowNormalChestOnMainPanelAfterWinMission();
            if (!MainMenuPanel.canShowChest)
            {
                if (AdsHelper.Instance.CanShowInterstitial())
                {
                    AdsHelper.__ShowInterstitialAd();
                }
            }

            Lock(false);
            GameplayController.Instance.ResumeGame();
            SceneManager.LoadScene("Home");
        }

        private void BtnWatchAds_Pressed()
        {
            if ((GameData.Instance.MissionsGroup.CountMissionWin + 1) == 2)
            {
                ClaimReward();
            }
            else
            {
                Config.LogEvent(TrackingConstants.EVENT_WIN_X3_REQUEST);
                if (!AdsHelper.__IsVideoRewardedAdReady("win_x3"))
                {
                    //MainGamePanel.Instance.ShowText("Ads not available", new Vector2(420f, 100f), btnVideo.rectTransform());
                }
                else
                {
                    AdsHelper.__ShowVideoRewardedAd(OnRewardedAdCompleted, "win_x3");
                }
            }
        }

        private void OnRewardedAdCompleted(bool isCompleted)
        {
            if (isCompleted)
            {
                ClaimReward();
                Config.LogEvent(TrackingConstants.EVENT_WIN_X3_REWARDED);
            }
        }

        private void ClaimReward()
        {
            fxModelGold.SetActive(false);
            fxModelSafe.SetActive(true);
            fxModelSafe.Initialize(false);
            fxModelSafe.AnimationState.SetAnimation(0, "safes_idleStart", false);
            fxModelSafe.AnimationState.AddAnimation(0, "safes_idleOpened", true, 0f);

            float goldFX = totalGold;
            int totalGoldNew = totalGold * 3;
            DOTween.To(tweenVal => goldFX = tweenVal, totalGold, totalGoldNew, 1f)
                    .OnUpdate(() =>
                    {
                        txtGoldRW.text = "" + goldFX.ToString("0");

                    })
                    .OnComplete(() =>
                    {
                        txtGoldRW.text = "" + totalGoldNew;
                    })
                    .SetUpdate(true);

            btnVideo.SetActive(false);
            btnHome.SetActive(true);
            GameData.Instance.CurrenciesGroup.Add(IDs.CURRENCY_COIN, totalGold * 2);
            //efectStar3.SetActive(true);
        }
    }
}
