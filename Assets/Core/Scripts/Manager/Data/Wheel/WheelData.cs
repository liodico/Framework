
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
    public class WheelRewardDefinition : IComparable<WheelRewardDefinition>
    {
        public int id;
        public int rewardType;
        public int rewardId;
        public int rewardValue;
        public int ratio;

        public int CompareTo(WheelRewardDefinition other)
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

    [System.Serializable]
    public class HeroRarityRatioDefinition : IComparable<HeroRarityRatioDefinition>
    {
        public int heroRarity;
        public int ratio;

        public int CompareTo(HeroRarityRatioDefinition other)
        {
            return heroRarity.CompareTo(other.heroRarity);
        }
    }

    //==================================

    public class WheelData : DataGroup
    {
        #region Members
        private List<WheelRewardDefinition> mWheelRewards;
        private List<HeroRarityRatioDefinition> mHeroRarityRatioDefinitions;
        private IntegerData mSpinCount;
        private IntegerData mSpinCountPerDay;
        private TimerTask mTimer;
        private BoolData mFreeSpin;

        public List<WheelRewardDefinition> WheelRewards => mWheelRewards;
        public int SpinCount => mSpinCount.Value;
        public bool FreeSpin => mFreeSpin.Value;

        public bool IsRunning => mTimer.IsRunning;

        #endregion

        //=============================================

        #region Public

        public WheelData(int pId) : base(pId)
        {
            mSpinCount = AddData(new IntegerData(0, 0));
            mSpinCountPerDay = AddData(new IntegerData(1, 0));
            mTimer = AddData(new TimerTask(2, false));
            mTimer.SetOnComplete(GetFreeSpin);
            mFreeSpin = AddData(new BoolData(3, true));
            InitWheelRewards();
            InitHeroRarityRatios();
        }

        public void SpinFree(ref int rewardChoice, ref RewardInfo rewardInfo)
        {
            mFreeSpin.Value = false;
            mSpinCountPerDay.Value += 1;

            if (mSpinCountPerDay.Value == 1) mTimer.Start(15 * 60);
            else if (mSpinCountPerDay.Value == 2) mTimer.Start(30 * 60);
            else if (mSpinCountPerDay.Value > 2) mTimer.Start(120 * 60);

            Spin(ref rewardChoice, ref rewardInfo);
        }

        public string GetTimeFree()
        {
            var s = mTimer.RemainSeconds;
            return TimeHelper.FormatHHMMss(s, true);
        }

        public void NewDay()
        {
            mFreeSpin.Value = true;
            mSpinCountPerDay.Value = 0;
        }

        public void Spin(ref int rewardChoice, ref RewardInfo rewardInfo)
        {
            //if (mSpinCount.Value == 0)
            //{
            //    for (int i = 0; i < mWheelRewards.Count; i++)
            //    {
            //        if(mWheelRewards[i].rewardType == IDs.REWARD_TYPE_UNLOCK_CHARACTER) rewardChoice = i;
            //        rewardInfo = new RewardInfo(IDs.REWARD_TYPE_UNLOCK_CHARACTER, 1, 1);
            //        //Tut spin (free) -> spin ra unit 1 -> evolve 1 lần (tương đương với lần đầu tiên spin chắc chắn ra unit 1)
            //    }
            //}
            //else
            //{
            float[] chances = new float[mWheelRewards.Count];
            for (int i = 0; i < mWheelRewards.Count; i++)
            {
                chances[i] = mWheelRewards[i].ratio;
            }

            rewardChoice = LogicAPI.CalcRandomWithChances(chances);
            rewardInfo = mWheelRewards[rewardChoice].GetReward();
            //}

            mSpinCount.Value += 1;
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

        private void GetFreeSpin(TimerTask timer, long remain)
        {
            if (remain <= 0) mFreeSpin.Value = true;
        }

        private void InitWheelRewards()
        {
            var dataContent = GameData.GetTextContent("Data/WheelRewards");
            mWheelRewards = JsonHelper.GetJsonList<WheelRewardDefinition>(dataContent);
            mWheelRewards.Sort();

            Debug.LogNewtonJson(mWheelRewards);
        }

        private void InitHeroRarityRatios()
        {
            var dataContent = GameData.GetTextContent("Data/RatioUnlockCharacters");
            mHeroRarityRatioDefinitions = JsonHelper.GetJsonList<HeroRarityRatioDefinition>(dataContent);
            mHeroRarityRatioDefinitions.Sort();

            Debug.LogNewtonJson(mHeroRarityRatioDefinitions);
        }

        #endregion
    }
}