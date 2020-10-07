using System;
using Spine;
using Spine.Unity;
using TMPro;
using UnityEngine;
using Utilities.Common;
using Utilities.Components;
using Utilities.Service.RFirebase;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

namespace FoodZombie.UI
{
    public class MissionDetailPanel : MyGamesBasePanel
    {
        public JustButton btnGet;
        public SimpleTMPButton btnPlay;
        public GameObject btnPlayDisable;
        public TextMeshProUGUI txtMission;
        public TextMeshProUGUI txtInfo;

        public SkeletonGraphic modelChest;
        public GameObject imgChestDisable;
        public TMP_InputField txtIdMission;
        private string eventNameShowOff = "open";

        public bool initialized;

        private EventData mEventDataShowOff;

        private void Start()
        {
            btnGet.onClick.AddListener(BtnGet_Pressed);
            btnPlay.onClick.AddListener(BtnPlay_Pressed);

            mEventDataShowOff = modelChest.Skeleton.Data.FindEvent(eventNameShowOff);
            modelChest.AnimationState.Event += HandleAnimationStateEvent;
        }

#if DEVELOPMENT
        private void OnEnable()
        {
            txtIdMission.text = GameData.Instance.MissionsGroup.CurrentMissionId + "";
            txtIdMission.SetActive(true);
        }
#endif

        internal override void Init()
        {
            txtMission.text = "Mission " + (GameData.Instance.MissionsGroup.CountMissionWin + 1) + "";
            if (LogicAPI.CanShowPreUnit())
            {
                txtInfo.text = "Tap to open a random booster or pre-unit";
                btnGet.enabled = true;

                modelChest.SetActive(true);
                modelChest.Initialize(false);
                modelChest.AnimationState.SetAnimation(0, "idleClose", true);
                imgChestDisable.SetActive(false);

                if (GameData.Instance.MissionsGroup.CountRequestWinStreak == 0)
                {
                    btnPlay.SetActive(false);
                    btnPlayDisable.SetActive(true);
                }
                else
                {
                    btnPlay.SetActive(true);
                    btnPlayDisable.SetActive(false);
                }
            }
            else
            {
                txtInfo.text = "Available at Mission 3";
                btnGet.enabled = false;

                modelChest.SetActive(false);
                imgChestDisable.SetActive(true);
            }

            btnPlay.enabled = true;
            Config.LogEvent(TrackingConstants.EVENT_PREBOOST_SHOW);

            initialized = true;
        }

        //private void OnComplete(TrackEntry trackEntry)
        //{
        //    GameData.Instance.MissionsGroup.ClaimRandomWinStreak();
        //    btnPlay.enabled = true;
        //}

        private void BtnGet_Pressed()
        {
            //free lần đầu
            if (GameData.Instance.MissionsGroup.CountRequestWinStreak == 0)
            {
                var trackEntry = modelChest.AnimationState.SetAnimation(0, "openStart", false);
                modelChest.AnimationState.AddAnimation(0, "idleOpened", true, 0f);
                //trackEntry.Complete += OnComplete;
                btnGet.enabled = false;
                btnPlay.enabled = false;
            }
            else
            {
                Config.LogEvent(TrackingConstants.EVENT_PREBOOST_REQUEST);
                //xem ads
                if (!AdsHelper.__IsVideoRewardedAdReady("preboost"))
                {
                    MainPanel.instance.ShowWarningTooltip(
                        btnGet.rectTransform,
                        new UITooltips.Message("Ads is not available yet!", new Vector2(600, 160))
                    );

                    return;
                }

                AdsHelper.__ShowVideoRewardedAd(OnRewardedAdCompleted, "preboost");
                // networkAds = AdsHelper.ShowVideoRewardedAndGetNetwork(OnRewardedAdCompleted, "preboost");
            }
        }
        string networkAds = "";
        private void OnRewardedAdCompleted(bool isCompleted)
        {
            if (isCompleted)
            {
                Config.LogEvent(TrackingConstants.EVENT_PREBOOST_REWARDED);
                var trackEntry = modelChest.AnimationState.SetAnimation(0, "openStart", false);
                modelChest.AnimationState.AddAnimation(0, "idleOpened", true, 0f);
                //trackEntry.Complete += OnComplete;
                btnGet.enabled = false;
                btnPlay.enabled = false;
            }
        }

        private void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
        {
            bool eventMatch = (mEventDataShowOff == e.Data); // Performance recommendation: Match cached reference instead of string.
            if (eventMatch)
            {
                GameData.Instance.MissionsGroup.ClaimRandomWinStreak();
                btnPlay.enabled = true;

                btnPlay.SetActive(true);
                btnPlayDisable.SetActive(false);
            }
        }

        private void BtnPlay_Pressed()
        {
#if DEVELOPMENT
            int indexMission = int.Parse(txtIdMission.text);
            GameData.Instance.MissionsGroup.SetCurrentMission(indexMission);
            GameData.Instance.MissionsGroup.SetCountMissionWin(indexMission - 1);
#endif
            Config.typeMenuInGameMode = Config.TYPE_MODE_NORMAL;
            MainPanel.instance.LoadGamePlayScreen();
        }
    }
}