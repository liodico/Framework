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
    public class UpgradeFoodPanel : MyGamesBasePanel
    {
        #region Members

        [SerializeField] private CurrencyView mGoldView;

        [SerializeField] private TextMeshProUGUI mTxtLevelFoodGuy1;
        [SerializeField] private TextMeshProUGUI mTxtLevelFoodGuy2;
        [SerializeField] private TextMeshProUGUI mTxtCurrentStartingFood;
        [SerializeField] private TextMeshProUGUI mTxtCurrentFoodSpeed;
        [SerializeField] private TextMeshProUGUI mTxtNextStartingFood;
        [SerializeField] private TextMeshProUGUI mTxtNextFoodSpeed;
        [SerializeField] private TextMeshProUGUI mTxtAttributes;
        [SerializeField] private PriceTMPButton mBtnUpgrade;
        [SerializeField] private SkeletonGraphicMultiObject mPlaneSkeleton;
        [SerializeField] private SkeletonGraphic mSkeGrapModel;
        [SerializeField] private SkeletonGraphic fxModel;
        [SerializeField] private AnimationCurve mHighlightFXAnim;

        private Tweener tweener = null;

        private bool mInitialized;

        private FoodGuyData foodGuyData => GameData.Instance.FoodGuyData;

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
                ShowFoodGuyStats();

                Config.LogEvent(TrackingConstants.EVENT_SAUSAGE_SHOW);
            }
        }

        internal override void Init()
        {
            // mGoldView.Init(IDs.CURRENCY_COIN);

            mInitialized = true;
            ShowFoodGuyStats();
        }

        #endregion

        //=============================================

        #region Public

        #endregion

        //==============================================

        #region Private

        private void ShowFoodGuyStats(string skinName = null)
        {
            if (skinName == null)
            {
                skinName = GameData.Instance.FoodGuyData.GetSkillName();
            }

            if (tweener != null) tweener.Kill();
            mSkeGrapModel.Initialize(true);
            mSkeGrapModel.Skeleton.SetSkin(skinName);
            mSkeGrapModel.Skeleton.SetToSetupPose();
            mSkeGrapModel.material = AssetsCollection.instance.matDefaultSekelton;
            fxModel.AnimationState.Update(10f);

            var info = foodGuyData.GetCurrentLevelInfo();
            mTxtLevelFoodGuy1.text = $"Level {foodGuyData.Level}";
            mTxtLevelFoodGuy2.text = $"Level {foodGuyData.Level + 1}";
            mTxtCurrentFoodSpeed.text = $"+{info.foodSpeed} %";
            mTxtCurrentStartingFood.text = $"+{info.startingFood}";
            mBtnUpgrade.labelTMP.text = info.cost > 0 ? $"{info.cost}" : "Free";

            info = foodGuyData.GetNextLevelInfo();
            if (info == null)
            {
                mTxtLevelFoodGuy1.text = $"Level MAX";
                mTxtLevelFoodGuy2.text = $"";
                mTxtNextFoodSpeed.text = $"";
                mTxtNextStartingFood.text = $"";

                mBtnUpgrade.enabled = false;
                mBtnUpgrade.labelTMP.text = "Maxed";
            }
            else
            {
                mTxtNextFoodSpeed.text = $"+{info.foodSpeed} %";
                mTxtNextStartingFood.text = $"+{info.startingFood}";
            }
        }

        private void OnBtnUpgrade_Pressed()
        {
            var info = foodGuyData.GetCurrentLevelInfo();
            if (GameData.Instance.CurrenciesGroup.Pay(IDs.CURRENCY_COIN, info.cost))
            {
                foodGuyData.LevelUp();
                UpgradeEffect();

                Config.LogEvent(TrackingConstants.EVENT_SAUSAGE_UPGRADE);
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
            ShowFoodGuyStats(skinNameTemp);

            fxModel.AnimationState.SetAnimation(0, "evolveUnit1", false);
            var trackEntry = mSkeGrapModel.AnimationState.SetAnimation(0, "run1", false);
            //mSkeGrapModel.AnimationState.AddAnimation(0, "idle_cook1", true, 0f);
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
                    mSkeGrapModel.AnimationState.SetAnimation(0, "idle_cook1", true);
                    ShowFoodGuyStats();
                });
        }

        #endregion
    }
}
