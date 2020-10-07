using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utilities.Components;

using Spine.Unity;
using DG.Tweening;

using Spine.Unity.Modules;
using Spine;
using System.Collections;
using Utilities.Common;

namespace FoodZombie.UI
{
    public class UpgradeVehiclePanel : MyGamesBasePanel
    {
        #region Members

        [SerializeField] private CurrencyView mGoldView;

        [SerializeField] private TextMeshProUGUI mTxtLevelVehicle1;
        [SerializeField] private TextMeshProUGUI mTxtLevelVehicle2;
        [SerializeField] private TextMeshProUGUI mTxtCurrentHp;
        [SerializeField] private TextMeshProUGUI mTxtCurrentDamage;
        //[SerializeField] private TextMeshProUGUI mTxtCurrentStartingFood;
        [SerializeField] private TextMeshProUGUI mTxtNextHp;
        [SerializeField] private TextMeshProUGUI mTxtNextDamage;
        //[SerializeField] private TextMeshProUGUI mTxtNextStartingFood;
        [SerializeField] private TextMeshProUGUI mTxtAttributes;
        [SerializeField] private PriceTMPButton mBtnUpgrade;
        [SerializeField] private SkeletonGraphicMultiObject mPlaneSkeleton;
        [SerializeField] private SkeletonGraphic fxModel;
        [SerializeField] private SkeletonGraphic mSkeGrapModel;
        [SerializeField] private AnimationCurve mHighlightFXAnim;

        private Tweener tweener = null;

        private bool mInitialized;

        private VehicleData vehicleData => GameData.Instance.VehicleData;

        public SimpleTMPButton BtnUpgrade => mBtnUpgrade;

        #endregion

        //=============================================

        #region MonoBehaviour

        private void Start()
        {
            mBtnUpgrade.onClick.AddListener(OnBtnUpgrade_Pressed);
        }

        private void OnEnable()
        {
            if (mInitialized)
            {
                ShowVehicleStats();

                Config.LogEvent(TrackingConstants.EVENT_VEHICLE_SHOW);
            }
        }

        internal override void Init()
        {
            // mGoldView.Init(IDs.CURRENCY_COIN);

            mInitialized = true;
            ShowVehicleStats();
        }

        #endregion

        //=============================================

        #region Public

        #endregion

        //==============================================

        #region Private

        private void ShowVehicleStats(string skinName = null)
        {
            if(skinName == null)
            {
                skinName = GameData.Instance.VehicleData.GetSkillName();
            }
            if (tweener != null) tweener.Kill();
            mSkeGrapModel.Initialize(true);
            mSkeGrapModel.Skeleton.SetSkin(skinName);
            mSkeGrapModel.Skeleton.SetToSetupPose();
            mSkeGrapModel.material = AssetsCollection.instance.matDefaultSekelton;
            fxModel.AnimationState.Update(10f);

            var info = vehicleData.GetCurrentLevelInfo();
            mTxtLevelVehicle1.text = $"Level {vehicleData.Level}";
            mTxtLevelVehicle2.text = $"Level {vehicleData.Level + 1}";
            mTxtCurrentHp.text = $"{info.hp}";
            mTxtCurrentDamage.text = $"{info.atk}";
            //mTxtCurrentStartingFood.text = $"{info.startingFood}";
            mBtnUpgrade.labelTMP.text = info.cost > 0 ? $"{info.cost}" : "Free";

            info = vehicleData.GetNextLevelInfo();
            if (info == null)
            {
                mTxtLevelVehicle1.text = $"Level MAX";
                mTxtLevelVehicle2.text = $"";
                mTxtNextHp.text = $"";
                mTxtNextDamage.text = $"";
                //mTxtNextStartingFood.text = $"";
            }
            else
            {
                mTxtNextHp.text = $"{info.hp}";
                mTxtNextDamage.text = $"{info.atk}";
                //mTxtNextStartingFood.text = $"{info.startingFood}";
            }
        }

        private void OnBtnUpgrade_Pressed()
        {
            if (vehicleData.IsMaxLevel()) return;
            var info = vehicleData.GetCurrentLevelInfo();
            if (GameData.Instance.CurrenciesGroup.Pay(IDs.CURRENCY_COIN, info.cost))
            {
                vehicleData.LevelUp();
                UpgradeEffect();

                Config.LogEvent(TrackingConstants.EVENT_VEHICLE_UPGRADE);
            }
            else
            {
                MainPanel.instance.ShowWarningTooltip(
                    mBtnUpgrade.rectTransform,
                    new UITooltips.Message("Not enough gold", new Vector2(600, 160))
                );
            }
        }

        private void UpgradeEffect()
        {
            string skinNameTemp = mSkeGrapModel.Skeleton.Skin.Name;
            ShowVehicleStats(skinNameTemp);

            fxModel.AnimationState.SetAnimation(0, "evolveVan", false);

            var trackEntry = mSkeGrapModel.AnimationState.SetAnimation(0, "run", false);
            //mSkeGrapModel.AnimationState.AddAnimation(0, "idle1", true, 0f);

            //mSkeGrapModel.material = AssetsCollection.instance.matSkeletonFill;
            float lerp = 0;
            DOTween.To(tweenVal => lerp = tweenVal, 0f, 1f, trackEntry.Animation.Duration)
                .OnUpdate(() =>
                {
                    var val = mHighlightFXAnim.Evaluate(lerp);
                    //mSkeGrapModel.material.SetFloat("_FillPhase", val);
                })
                .OnComplete(() =>
                {
                    //mSkeGrapModel.material = AssetsCollection.instance.matDefaultSekelton;
                    mSkeGrapModel.AnimationState.SetAnimation(0, "idle1", true);
                    ShowVehicleStats();
                });
        }

        #endregion
    }
}
