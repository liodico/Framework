using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Components;
using Utilities.Service.RFirebase;
using Spine.Unity;
using TMPro;

namespace FoodZombie.UI
{
    public class OpenChestPanel : MyGamesBasePanel
    {
        [SerializeField] private PriceTMPButton mBtnOpen;
        [SerializeField] private ManagerSpine mSpineModel;
        [SerializeField] private Image mImgTopPrize;
        [SerializeField] private TextMeshProUGUI mTxtRarityTopPrize;
        public SimpleTMPButton BtnOpen => mBtnOpen;
        NormalChestData chestData => GameData.Instance.NormalChestData;

        bool mDisable;

        void Start()
        {
            mBtnOpen.onClick.AddListener(BtnOpen_Pressed);
        }

        void OnEnable()
        {
            mDisable = false;
            mBtnOpen.enabled = true;
            btnBack.enabled = true;

            var topPrize = GameData.Instance.HeroesGroup.GetHeroData(chestData.GetBestHero());
            mImgTopPrize.sprite = topPrize.GetIcon();
            mImgTopPrize.SetNativeSize();
            mTxtRarityTopPrize.text = topPrize.GetRarityName();

            Config.LogEvent(TrackingConstants.EVENT_MYSTERY_BOX_SHOW);

            timer = 0;
            btnBack.gameObject.SetActive(false);
        }

        float timer = -1;

        void Update()
        {
            if (timer >= 0)
            {
                timer += Time.unscaledDeltaTime;
                if (timer > 2)
                {
                    btnBack.gameObject.SetActive(true);
                    timer = -1;
                }
            }
        }

        private void BtnOpen_Pressed()
        {
            if (mDisable) return;

            Config.LogEvent(TrackingConstants.EVENT_MYSTERY_BOX_REQUEST);
            if (!AdsHelper.__IsVideoRewardedAdReady("mystery_box"))
            {

                MainPanel.instance.ShowWarningTooltip(
                    mBtnOpen.rectTransform,
                    new UITooltips.Message("Ads is not available yet!", new Vector2(600, 160))
                );

                return;
            }

            // AdsHelper.__ShowVideoRewardedAd(OnRewardedAdCompleted, "mystery_box");
            AdsHelper.__ShowVideoRewardedAd(OnRewardedAdCompleted, "mystery_box");
        }
        string networkAds = "";
        private void OnRewardedAdCompleted(bool isCompleted)
        {
            if (isCompleted)
            {
                Config.LogEvent(TrackingConstants.EVENT_MYSTERY_BOX_REWARDED);
                OpenChest();
            }
            else
            {
                Config.LogEvent(TrackingConstants.EVENT_MYSTERY_BOX_FAIL);
            }
        }

        void OpenChest()
        {
            mDisable = true;
            mBtnOpen.enabled = false;
            btnBack.enabled = false;
            mSpineModel.PlayAnim("openStart");
            mSpineModel.SetDoneEvent(GetReward);
        }

        void GetReward()
        {
            mSpineModel.PlayAnim("idleOpened");
            Back();
            LogicAPI.ClaimReward(chestData.Open());
        }

        internal override void Back()
        {
            base.Back();

            //var missionId = (GameData.Instance.MissionsGroup.CountMissionWin + 1);
            //if (missionId == 7 && MainMenuPanel.showCount >= 1)
            //{
            //    MainPanel.instance.ShowRatePanelIfAvailable();
            //}
            //else
            //{
                //nếu show chest ở main menu thì ấn back ở show chest sẽ gọi ads popup
                if (AdsHelper.Instance.CanShowInterstitial())
                {
                    AdsHelper.__ShowInterstitialAd();
                }
            //}
        }
    }
}