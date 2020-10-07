
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
    public class NormalChestDefinition : IComparable<NormalChestDefinition>
    {
        public int id;
        public int rewardType;
        public int rewardId;
        public int rewardValue;
        public int ratio;

        public int CompareTo(NormalChestDefinition other)
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

    [Serializable]
    internal class OrderHero
    {
        public int order;
        public int id;
    }

    //==================================

    public class NormalChestData : DataGroup
    {
        #region Members
        private List<NormalChestDefinition> mNormalChests;
        private List<HeroRarityRatioDefinition> mHeroRarityRatioDefinitions;
        private IntegerData mSpinCount;
        private IntegerData mOrderGot;
        private IntegerData mCountShowedChest;

        public List<NormalChestDefinition> NormalChests => mNormalChests;
        public int SpinCount => mSpinCount.Value;
        public int CountShowedChest => mCountShowedChest.Value;
        #endregion

        //=============================================

        #region Public

        public NormalChestData(int pId) : base(pId)
        {
            mSpinCount = AddData(new IntegerData(0, 0));
            mOrderGot = AddData(new IntegerData(1, 2));
            mCountShowedChest = AddData(new IntegerData(2, 1));

            InitNormalChests();
            InitHeroRarityRatios();
        }

        public RewardInfo Open()
        {
            //HideChest();
            RewardInfo rewardInfo;
            if (mSpinCount.Value == 0)
            {
                rewardInfo = new RewardInfo(IDs.REWARD_TYPE_UNLOCK_CHARACTER, 3, 1);
            }
            else
            {
                float[] chances = new float[mNormalChests.Count];
                for (int i = 0; i < mNormalChests.Count; i++)
                {
                    chances[i] = mNormalChests[i].ratio;
                }

                int rewardChoice = LogicAPI.CalcRandomWithChances(chances);
                rewardInfo = mNormalChests[rewardChoice].GetReward();
            }
            mSpinCount.Value += 1;
            return rewardInfo;
        }

        public int GetOrderedIdHero()
        {
            var str = GameData.GetTextContent("Data/NormalOrderUnlockCharacter");
            var dataContent = JsonHelper.GetJsonList<OrderHero>(str);
            for (int i = 0; i < dataContent.Count; ++i)
            {
                if (dataContent[i].order == mOrderGot.Value)
                {
                    mOrderGot.Value += 1;
                    return dataContent[i].id;
                }
            }
            return dataContent[UnityEngine.Random.Range(0, dataContent.Count)].id;
        }

        public int GetBestHero()
        {
            //var str = GameData.GetTextContent("Data/NormalOrderUnlockCharacter");
            //var dataContent = JsonHelper.GetJsonList<OrderHero>(str);
            //for (int i = 0; i < dataContent.Count; ++i)
            //    if (dataContent[i].order == mOrderGot.Value) return dataContent[i].id;
            //return dataContent[dataContent.Count - 1].id;

            //lấy ra danh sách hero có thể unlock sort theo rarity
            var heroes = GameData.Instance.HeroesGroup.GetHerosCanUnlock();
            int heroId = heroes[heroes.Count - 1].Id;
            var length = heroes.Count;
            var maxRarityCanRandom = mHeroRarityRatioDefinitions[mHeroRarityRatioDefinitions.Count - 1].heroRarity;
            for (int i = length - 1; i >= 0; i--)
            {
                var hero = heroes[i] as HeroData;
                if (hero.Unlocked || hero.Rarity != maxRarityCanRandom) heroes.RemoveAt(i);
            }
            //ưu tiên show các hero chưa unlock và có rarity max, nếu unlock hết rồi thì lấy con xịn nhất
            if(heroes.Count > 0)
            {
                heroId = heroes[heroes.Count - 1].Id;
            }
            return heroId;
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

        public void ShowChest()
        {
            mCountShowedChest.Value = 0;
        }

        public void CountShowChest()
        {
            mCountShowedChest.Value++;
        }

        private void HideChest()
        {
            mCountShowedChest.Value = -1;
        }

        #endregion

        //==============================================

        #region Private

        private void InitNormalChests()
        {
            var dataContent = GameData.GetTextContent("Data/NormalChest");
            mNormalChests = JsonHelper.GetJsonList<NormalChestDefinition>(dataContent);
            mNormalChests.Sort();

            Debug.LogNewtonJson(mNormalChests);
        }

        private void InitHeroRarityRatios()
        {
            var dataContent = GameData.GetTextContent("Data/NormalRatioUnlockCharacters");
            mHeroRarityRatioDefinitions = JsonHelper.GetJsonList<HeroRarityRatioDefinition>(dataContent);
            mHeroRarityRatioDefinitions.Sort();

            Debug.LogNewtonJson(mHeroRarityRatioDefinitions);
        }

        #endregion
    }
}