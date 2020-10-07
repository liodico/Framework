using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using TMPro;
using Utilities.Components;
using Utilities.Service.RFirebase;

namespace FoodZombie.UI
{
    public class SpinningWheel : MyGamesBasePanel
    {
        [SerializeField] JustButton mBtnSpin;
        [SerializeField] SkeletonGraphic mAnimWheel;
        [SerializeField] Transform mContent;
        [SerializeField] Transform mPointer;
        [SerializeField] Transform mSpiningWheel;
        [SerializeField] AnimationCurve mAnimWheelCurve;
        [SerializeField] List<TextMeshProUGUI> mListTxtValue;
        [SerializeField] List<Image> mListImgIcon;
        [SerializeField] List<Image> mListImgLight;
        [SerializeField] GameObject imgAds;

        WheelData wheelData => GameData.Instance.WheelData;

        float mTimer = -1;
        float lastRotate = 0;
        float rotateTo = 0;
        public float TOTAL_ROLL = 3;
        public float TOTAL_TIME = 10;
        public float DISTANCE_VIBRATION = 3;

        int rewardChoice;
        RewardInfo rewardInfo;

        public JustButton BtnSpin => mBtnSpin;
        public bool initialized;

        internal override void Init()
        {
            base.Init();
            for (int i = 0; i < mListTxtValue.Count; ++i)
            {
                mListTxtValue[i].text = $"{wheelData.WheelRewards[i].GetReward().GetShortDescription()}";
                mListImgIcon[i].sprite = wheelData.WheelRewards[i].GetReward().GetIcon();
            }
            mContent.transform.eulerAngles = Vector3.zero;
            //mAnimWheel.AnimationState.SetAnimation(0, "idle", true);
            mBtnSpin.onClick.AddListener(BtnSpin_Pressed);

            initialized = true;
        }

        private void OnEnable()
        {
            Config.LogEvent(TrackingConstants.EVENT_SPIN_SHOW);
        }

        private void BtnSpin_Pressed()
        {
            if (mTimer >= 0) return;

            if (wheelData.FreeSpin)
            {
                Spin();
                MainPanel.instance.MainMenuPanel.ShowCountWheel();
                return;
            }

            Config.LogEvent(TrackingConstants.EVENT_SPIN_REQUEST);
            if (!AdsHelper.__IsVideoRewardedAdReady("spin"))
            {

                MainPanel.instance.ShowWarningTooltip(
                    mBtnSpin.rectTransform,
                    new UITooltips.Message("Ads is not available yet!", new Vector2(600, 160))
                );

                return;
            }

            // AdsHelper.__ShowVideoRewardedAd(OnRewardedAdCompleted, "spin");
            AdsHelper.__ShowVideoRewardedAd(OnRewardedAdCompleted, "spin");
        }

        string networkAds = "";

        private void OnRewardedAdCompleted(bool isCompleted)
        {
            if (isCompleted)
            {
                Config.LogEvent(TrackingConstants.EVENT_SPIN_REWARDED);
                Spin();
            }
        }

        void Spin()
        {
            if (wheelData.FreeSpin) wheelData.SpinFree(ref rewardChoice, ref rewardInfo);
            else wheelData.Spin(ref rewardChoice, ref rewardInfo);

            lastRotate = mContent.transform.eulerAngles.z;
            rotateTo = -(TOTAL_ROLL * 360 + rewardChoice * 60 + lastRotate);
            mTimer = 0;
            //mAnimWheel.AnimationState.SetAnimation(0, "roll", true);
            btnBack.gameObject.SetActive(false);
            mBtnSpin.gameObject.SetActive(false);
        }

        void Update()
        {
            imgAds.SetActive(!wheelData.FreeSpin);
            if (mTimer >= 0)
            {
                mTimer += Time.deltaTime;

                if (mTimer >= TOTAL_TIME)
                {
                    mTimer = -1;
                    mContent.eulerAngles = new Vector3(0, 0, lastRotate + rotateTo);
                    mPointer.eulerAngles = Vector3.zero;
                    mSpiningWheel.localPosition = Vector3.zero;
                    CheckLight();
                    LogicAPI.ClaimReward(rewardInfo);
                    //mAnimWheel.AnimationState.SetAnimation(0, "stop", true);

                    btnBack.gameObject.SetActive(true);
                    mBtnSpin.gameObject.SetActive(true);
                }
                else
                {
                    float currentValue = mAnimWheelCurve.Evaluate(mTimer / TOTAL_TIME);
                    mContent.eulerAngles = new Vector3(0, 0, lastRotate + currentValue * rotateTo);

                    var a = mContent.eulerAngles.z % 60;
                    var b = a < 30 ? a : 30 - (a - 30) * 1.5f;
                    if (b < 0 || currentValue > 1) b = 0;
                    mPointer.eulerAngles = new Vector3(0, 0, -b);

                    if (currentValue > 1) currentValue = 1;
                    var c = UnityEngine.Random.Range(0, Mathf.PI * 2);
                    var d = DISTANCE_VIBRATION * Mathf.Sin(currentValue * Mathf.PI);
                    var e = new Vector3(Mathf.Sin(c) * d, Mathf.Cos(c) * d, 0);
                    mSpiningWheel.localPosition = e;
                    CheckLight();
                }
            }
        }

        void CheckLight()
        {
            var currentAngles = (mContent.eulerAngles.z - 30) % 360;
            if (currentAngles < 0) currentAngles += 360;
            currentAngles = 5 - (int)currentAngles / 60;
            for (int i = 0; i < mListImgLight.Count; ++i)
            {
                if (i == currentAngles) mListImgLight[i].gameObject.SetActive(true);
                else mListImgLight[i].gameObject.SetActive(false);
            }
        }
    }
}