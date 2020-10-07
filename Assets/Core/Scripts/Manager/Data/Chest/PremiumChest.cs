
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Common;
using Utilities.Pattern.Data;
using Utilities.Service.RFirebase;
using Debug = Utilities.Common.Debug;

namespace FoodZombie
{
    [System.Serializable]
    public class PremiumChestDefinition : IComparable<PremiumChestDefinition>
    {
        public int id;
        public int rewardType;
        public int rewardId;
        public int rewardValue;
        public int ratio;

        public int CompareTo(PremiumChestDefinition other)
        {
            return id.CompareTo(other.id);
        }

        public RewardInfo GetReward()
        {
            return new RewardInfo(rewardType, rewardId, rewardValue);
        }

        public RewardInfo ClaimReward(RewardInfo pPrePickedReward = null)
        {
            if (pPrePickedReward == null)
                pPrePickedReward = GetReward();

            GameData.Instance.WaitForAutoBackup(true);
            return LogicAPI.ClaimReward(pPrePickedReward);
        }
    }

    //==================================

    public class PremiumChestData : DataGroup
    {
        #region Members
        private List<PremiumChestDefinition> mPremiumChests;
        private List<HeroRarityRatioDefinition> mHeroRarityRatioDefinitions;
        private IntegerData mSpinCount;

        public List<PremiumChestDefinition> PremiumChests => mPremiumChests;
        public int SpinCount => mSpinCount.Value;

        #endregion

        //=============================================

        #region Public

        public PremiumChestData(int pId) : base(pId)
        {
            mSpinCount = AddData(new IntegerData(0, 0));
            InitPremiumChests();
            InitHeroRarityRatios();
        }

        public int Open()
        {
            mSpinCount.Value += 1;
            if (mSpinCount.Value == 1)
            {
                for (int i = 0; i < mPremiumChests.Count; i++)
                {
                    if (mPremiumChests[i].rewardType == IDs.REWARD_TYPE_UNLOCK_CHARACTER)
                        return i;
                }
            }

            float[] chances = new float[mPremiumChests.Count];
            for (int i = 0; i < mPremiumChests.Count; i++)
            {
                chances[i] = mPremiumChests[i].ratio;
            }

            int rewardChoice = LogicAPI.CalcRandomWithChances(chances);
            return rewardChoice;
        }

        public int RandomRarity()
        {
            float[] chances = new float[mHeroRarityRatioDefinitions.Count];
            for (int i = 0; i < mHeroRarityRatioDefinitions.Count; i++)
            {
                chances[i] = mHeroRarityRatioDefinitions[i].ratio;
            }

            int rarityChoice = mHeroRarityRatioDefinitions[LogicAPI.CalcRandomWithChances(chances)].heroRarity;
            return rarityChoice;
        }

        #endregion

        //==============================================

        #region Private

        private void InitPremiumChests()
        {
            var dataContent = GameData.GetTextContent("Data/PremiumChest");
            mPremiumChests = JsonHelper.GetJsonList<PremiumChestDefinition>(dataContent);
            mPremiumChests.Sort();

            Debug.LogNewtonJson(mPremiumChests);
        }

        private void InitHeroRarityRatios()
        {
            var dataContent = GameData.GetTextContent("Data/PremiumRatioUnlockCharacters");
            mHeroRarityRatioDefinitions = JsonHelper.GetJsonList<HeroRarityRatioDefinition>(dataContent);
            mHeroRarityRatioDefinitions.Sort();

            Debug.LogNewtonJson(mHeroRarityRatioDefinitions);
        }

        #endregion
    }
}