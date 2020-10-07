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
    public class LosePanel : MyGamesBasePanel
    {
        public GameObject groupLose, groupSecondChance;
        public SimpleTMPButton btnHome, btnPlayOn, btnNoThank;
        float xModeBossMap = 0;
        public SkeletonGraphic fxModel;

        private int showCount = 0;

        public TextMeshProUGUI txtGoldRW;
        //public GameObject efectStar1;
        private int totalGold;

        private void Start()
        {
            btnHome.onClick.AddListener(BtnHome_Pressed);
            btnPlayOn.onClick.AddListener(BtnPlayOn_Pressed);
            btnNoThank.onClick.AddListener(BtnNoThank_Pressed);
        }

        private void OnEnable()
        {
            btnNoThank.SetActive(false);
            StartCoroutine(IEShowBtnMenu());
        }

        private IEnumerator IEShowBtnMenu()
        {
            yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(2f));
            btnNoThank.SetActive(true);
        }

        public void Init(List<RewardInfo> rwComp)
        {
            Lock(true);

            if (showCount == 0)
            {
                Config.LogEvent(TrackingConstants.EVENT_PLAY_ON_SHOW);
                groupSecondChance.SetActive(true);
                groupLose.SetActive(false);
            }
            else
            {
                GameData.Instance.CurrenciesGroup.Add(IDs.CURRENCY_COIN, totalGold);
                groupSecondChance.SetActive(false);
                groupLose.SetActive(true);
            }
            showCount++;

            fxModel.AnimationState.SetAnimation(0, "Fail_start", false);
            fxModel.AnimationState.AddAnimation(0, "Fail_idle", true, 0f);

            if (Config.typeModeInGame == Config.TYPE_MODE_BOSS
                || Config.isHardMode == 1
                )
            {
                //btnRetry.gameObject.SetActive(false);
                btnHome.rectTransform().anchoredPosition = new Vector3(xModeBossMap, btnHome.rectTransform().localPosition.y, btnHome.rectTransform().localPosition.z);
            }

            //set gold reward
            totalGold = 0;
            foreach (RewardInfo item in rwComp)
            {
                if (item.Type == IDs.REWARD_TYPE_CURRENCY && item.Id == IDs.CURRENCY_COIN)
                {
                    totalGold += item.Value;
                }
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
                    })
                    .SetUpdate(true);

            //efectStar1.SetActive(true);
        }

        private void BtnHome_Pressed()
        {
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

        private void BtnPlayOn_Pressed()
        {
            Config.LogEvent(TrackingConstants.EVENT_PLAY_ON_REQUEST);
            if (!AdsHelper.__IsVideoRewardedAdReady("play_on"))
            {
                //MainGamePlay.Instance.controlUI.ShowText("Ads not available", new Vector2(420f, 100f), btnPlayOn.rectTransform());
            }
            else
            {
                AdsHelper.__ShowVideoRewardedAd(OnRewardedAdCompleted, "play_on");
            }
        }

        private void OnRewardedAdCompleted(bool isCompleted)
        {
            if (isCompleted)
            {
                Config.LogEvent(TrackingConstants.EVENT_PLAY_ON_REWARDED);
                //MainGamePlay.Instance.ReviceBaseAlly();
                Lock(false);
                Back();
            }
        }

        private void BtnNoThank_Pressed()
        {
            groupSecondChance.SetActive(false);
            groupLose.SetActive(true);

            GameData.Instance.CurrenciesGroup.Add(IDs.CURRENCY_COIN, totalGold);
            float goldFX = 0;
            DOTween.To(tweenVal => goldFX = tweenVal, 0, totalGold, 1f)
                    .OnUpdate(() =>
                    {
                        txtGoldRW.text = "" + goldFX.ToString("0");

                    })
                    .OnComplete(() =>
                    {
                        txtGoldRW.text = "" + totalGold;
                    })
                    .SetUpdate(true);

            //efectStar1.SetActive(true);
        }
    }
}
