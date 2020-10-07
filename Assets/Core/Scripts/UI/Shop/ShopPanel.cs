using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Components;
using Utilities.Service.RFirebase;
using TMPro;
using Utilities.Service;

namespace FoodZombie.UI
{
    public class ShopPanel : MyGamesBasePanel
    {
        const string NORMAL_CHEST = "NORMAL_CHEST";
        [SerializeField] SimpleButton mBtnNormalChest;
        const string EVENT_CHEST = "EVENT_CHEST";
        [SerializeField] SimpleButton mBtnEventChest;
        public const string NO_ADS = "com.sohcso.game.zdefense2.noads";
        [SerializeField] SimpleButton mBtnAdsFree;
        public const string CASH_500 = "com.sohcso.game.zdefense2.gem1";
        [SerializeField] SimpleButton mBtnCash1;
        public const string CASH_7500 = "com.sohcso.game.zdefense2.gem2";
        [SerializeField] SimpleButton mBtnCash2;

        [SerializeField] TextMeshProUGUI mTxtAdsFree;
        [SerializeField] TextMeshProUGUI mTxtCash1;
        [SerializeField] TextMeshProUGUI mTxtCash2;

        void Start()
        {
            mBtnNormalChest.onClick.AddListener(BtnNormalChest_Pressed);
            mBtnEventChest.onClick.AddListener(BtnEventChest_Pressed);
            mBtnAdsFree.onClick.AddListener(BtnAdsFree_Pressed);
            mBtnCash1.onClick.AddListener(BtnCash500_Pressed);
            mBtnCash2.onClick.AddListener(BtnCash7500_Pressed);

#if UNITY_IAP
            if (GameData.Instance.GameConfigGroup.NoAds)
            {
                mTxtAdsFree.text = "BOUGHT";
                mBtnAdsFree.SetEnable(false);
            }
            else
            {
                try
                {
                    if (PaymentHelper.Instance != null && mTxtAdsFree != null)
                    {
                        var s = PaymentHelper.Instance.GetLocalizedPriceString(NO_ADS);
                        if(s != null) mTxtAdsFree.text = s;
                    }
                }
                catch (System.Exception)
                {

                }
                mBtnAdsFree.SetEnable(true);
            }
            try
            {
                if (PaymentHelper.Instance != null && mTxtCash1 != null)
                {
                    var s = PaymentHelper.Instance.GetLocalizedPriceString(CASH_500);
                    if (s != null) mTxtCash1.text = s;
                }
            }
            catch (System.Exception)
            {
                
            }
            try
            {
                if (PaymentHelper.Instance != null && mTxtCash2 != null)
                {
                    var s = PaymentHelper.Instance.GetLocalizedPriceString(CASH_7500);
                    if (s != null) mTxtCash2.text = s;
                }
            }
            catch (System.Exception)
            {

            }
#else
            mBtnAdsFree.SetEnable(false);
            mBtnCash1.SetEnable(false);
            mBtnCash2.SetEnable(false);
#endif
        }

        void BtnNormalChest_Pressed()
        {
            if (GameData.Instance.CurrenciesGroup.Pay(IDs.CURRENCY_CASH, 50))
            {
                int reward = GameData.Instance.PremiumChestData.Open();
                LogicAPI.ClaimReward(GameData.Instance.PremiumChestData.PremiumChests[reward].GetReward());
                Config.LogEvent(TrackingConstants.EVENT_CHEST_OPEN, TrackingConstants.PARAM_CHEST_NAME, "premium_chest");
            }
            else
            {
                MainPanel.instance.ShowWarningTooltip(
                    mBtnNormalChest.rectTransform,
                    new UITooltips.Message("Not enough cash", new Vector2(600, 160))
                );
            }
        }

        void BtnEventChest_Pressed()
        {
            //Config.LogEvent(TrackingConstants.EVENT_CHEST_OPEN, TrackingConstants.PARAM_CHEST_NAME, "event_chest");
        }

        void BtnAdsFree_Pressed()
        {
            PaymentHelper.Instance.Purchase(NO_ADS, AdsFree_Purchased);
        }

        void AdsFree_Purchased(bool success)
        {
            if (success)
            {
                GameData.Instance.GameConfigGroup.SetNoAds(true);
                mTxtAdsFree.text = "BOUGHT";
                mBtnAdsFree.SetEnable(false);
                Config.LogEvent(TrackingConstants.EVENT_PURCHASE_IAP, TrackingConstants.PARAM_PACK_NAME, NO_ADS);
            }
        }

        void BtnCash500_Pressed()
        {
            PaymentHelper.Instance.Purchase(CASH_500, Cash500_Purchased);
        }

        void Cash500_Purchased(bool success)
        {
            if (success)
            {
                var reward = new RewardInfo(IDs.REWARD_TYPE_CURRENCY, IDs.CURRENCY_CASH, 500);
                LogicAPI.ClaimReward(reward);
                Config.LogEvent(TrackingConstants.EVENT_PURCHASE_IAP, TrackingConstants.PARAM_PACK_NAME, CASH_500);
            }
        }

        void BtnCash7500_Pressed()
        {
            PaymentHelper.Instance.Purchase(CASH_7500, Cash7500_Purchased);
        }

        void Cash7500_Purchased(bool success)
        {
            if (success)
            {
                var reward = new RewardInfo(IDs.REWARD_TYPE_CURRENCY, IDs.CURRENCY_CASH, 7500);
                LogicAPI.ClaimReward(reward);
                Config.LogEvent(TrackingConstants.EVENT_PURCHASE_IAP, TrackingConstants.PARAM_PACK_NAME, CASH_7500);
            }
        }
    }
}