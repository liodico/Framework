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
    public class VehiclePanel : MyGamesBasePanel
    {
        #region Members

        [SerializeField] private CurrencyView mGoldView;

        [SerializeField] private TextMeshProUGUI mTxtLevelVehicle1;
        [SerializeField] private TextMeshProUGUI mTxtLevelVehicle2;
        [SerializeField] private TextMeshProUGUI mTxtCurrentHp;
        [SerializeField] private TextMeshProUGUI mTxtCurrentDamage;
        [SerializeField] private TextMeshProUGUI mTxtNextHp;
        [SerializeField] private TextMeshProUGUI mTxtNextDamage;
        [SerializeField] private TextMeshProUGUI mTxtAttributes;
        [SerializeField] private PriceTMPButton mBtnUpgrade;
        [SerializeField] private SkeletonGraphicMultiObject mPlaneSkeleton;
        [SerializeField] private AnimationCurve mHighlightFXAnim;

        private List<PowerUpItemData> mPowerUpItemDatasWithExcept;

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
                //ShowVehicleStats();
            }
        }

        internal override void Init()
        {
            mGoldView.Init(IDs.CURRENCY_COIN);

            mInitialized = true;
            //ShowVehicleStats();
        }

        #endregion

        //=============================================

        #region Public
            
        #endregion

        //==============================================

        #region Private
        
        private void OnBtnUpgrade_Pressed()
        {
            
        }

        #endregion
    }
}
