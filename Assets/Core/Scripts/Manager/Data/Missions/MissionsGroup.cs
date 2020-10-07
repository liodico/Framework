using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Common;
using Utilities.Pattern.Data;
using Debug = Utilities.Common.Debug;

namespace FoodZombie
{
    [System.Serializable]
    public class MissionDefinition : IComparable<MissionDefinition>
    {
        public int id;
        public string name;
        public string battleJsonFile;

        public int[] rewardTypes;
        public int[] rewardIds;
        public int[] rewardValues;

        public bool hasBoss;
        public int area;
        public int bossMissionId;

        public int CompareTo(MissionDefinition other)
        {
            return id.CompareTo(other.id);
        }
    }

    [System.Serializable]
    public class AreaDefinition : IComparable<AreaDefinition>
    {
        public int id;
        public string name;
        public int[] rewardTypes;
        public int[] rewardIds;
        public int[] rewardValues;

        public int CompareTo(AreaDefinition other)
        {
            return id.CompareTo(other.id);
        }
    }

    [System.Serializable]
    public class WinStreakRewardDefinition : IComparable<WinStreakRewardDefinition>
    {
        public int id;
        public int rewardType;
        public int rewardId;
        public int rewardValue;
        public int ratio;

        public int CompareTo(WinStreakRewardDefinition other)
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


    public class MissionsGroup : DataGroup
    {
        #region Members

        private List<MissionDefinition> mMissions;
        private List<MissionDefinition> mBossMissions;
        private Dictionary<int, AreaDefinition> mAreas;
        private IntegerData mCurrentMissionId;
        private IntegerData mCountWinStreak;
        private IntegerData mPreUnitId;
        private IntegerData mCountRequestWinStreak;
        private List<WinStreakRewardDefinition> mWinStreakRewards;
        private IntegerData mCountMissionWin;
        private ListData<int> mCurrentBossMissions;

        public int CurrentMissionId => mCurrentMissionId.Value;
        public int CountWinStreak => mCountWinStreak.Value;
        public int CountRequestWinStreak => mCountRequestWinStreak.Value;
        public int PreUnitId => mPreUnitId.Value;
        public int CountMissionWin => mCountMissionWin.Value;
        public List<int> CurrentBossMissions => mCurrentBossMissions.GetValues();

        #endregion

        //=============================================

        #region Public

        public MissionsGroup(int pId) : base(pId)
        {
            mCurrentMissionId = AddData(new IntegerData(0, 0));
            mCountWinStreak = AddData(new IntegerData(1, 0));
            mPreUnitId = AddData(new IntegerData(2, -1));
            mCountRequestWinStreak = AddData(new IntegerData(3, 0));
            mCountMissionWin = AddData(new IntegerData(4, 0));
            mCurrentBossMissions = AddData(new ListData<int>(5, new List<int>()));

            InitMissions();
            InitBossMissions();
            InitAreas();
            InitWinStreak();
        }

        public override void PostLoad()
        {
            base.PostLoad();

            if (mCurrentMissionId.Value == 0)
            {
                mCurrentMissionId.Value = mMissions[0].id;
            }
        }

        public List<RewardInfo> Cleared(int type, Dictionary<int, int> pTotalKills)
        {
            //count enemies
            int enemyId;
            int killsCount = 0;
            foreach (var kill in pTotalKills)
            {
                enemyId = kill.Key;
                killsCount = kill.Value;
                var enemyData = GameData.Instance.EnemiesGroup.GetEnemyData(enemyId);
                enemyData.AddKillsCount(killsCount);
            }

            if (type == Config.TYPE_MODE_NORMAL)
            {
                //reset pre unit
                mPreUnitId.Value = -1;

                //mission reward
                var mission = GetCurrentMission();
                var index = mMissions.IndexOf(mission);

                //set show normal chest
                LogicAPI.SetShowNormalChestOnMainPanelAfterWinMission();

                //gold reward
                var gold = LogicAPI.GetRewardGoldByMission(1, 1);

                //win streak and next mission
                mCountWinStreak.Value++;
                if (mCountMissionWin.Value < mMissions.Count - 1)
                {
                    mCurrentMissionId.Value = mMissions[index + 1].id;
                    var missionNext = GetCurrentMission();
                    if (missionNext.bossMissionId != 0) mCurrentBossMissions.Add(missionNext.bossMissionId);
                }
                else
                {
                    mCurrentMissionId.Value = mMissions[UnityEngine.Random.Range(Constants.MIN_STAGE_RD, Constants.MAX_STAGE_RD + 1)].id;
                }
                mCountMissionWin.Value++;

                //Safe
                GameData.Instance.SafeData.CheckStartTimeMore();

                List<RewardInfo> rewards = new List<RewardInfo>();
                if (mission.rewardIds != null && mission.rewardIds.Length > 0)
                {
                    int length = mission.rewardIds.Length;
                    for (int i = 0; i < length; i++)
                    {
                        rewards.Add(new RewardInfo(mission.rewardTypes[i], mission.rewardIds[i], mission.rewardValues[i]));
                    }
                }
                rewards.Add(new RewardInfo(IDs.REWARD_TYPE_CURRENCY, IDs.CURRENCY_COIN, gold));
                return rewards;
            }
            else
            {
                var bossMission = GetCurrentBossMission();
                mCurrentBossMissions.Remove(bossMission.id);

                List<RewardInfo> rewards = new List<RewardInfo>();
                var gold = LogicAPI.GetRewardGoldByMission(1, 1) * 3;
                rewards.Add(new RewardInfo(IDs.REWARD_TYPE_CURRENCY, IDs.CURRENCY_COIN, gold));
                return rewards;
            }
        }

        public List<RewardInfo> Lose(int type)
        {
            if (type == Config.TYPE_MODE_NORMAL)
            {
                //reset pre unit
                mPreUnitId.Value = -1;

                //gold reward
                var gold = LogicAPI.GetRewardGoldLoseByMission(1, 1);

                //win streak and next mission
                mCountWinStreak.Value = 0;

                List<RewardInfo> rewards = new List<RewardInfo>();
                rewards.Add(new RewardInfo(IDs.REWARD_TYPE_CURRENCY, IDs.CURRENCY_COIN, gold));
                return rewards;
            }
            else
            {
                //gold reward
                var gold = LogicAPI.GetRewardGoldLoseByMission(1, 1);

                List<RewardInfo> rewards = new List<RewardInfo>();
                rewards.Add(new RewardInfo(IDs.REWARD_TYPE_CURRENCY, IDs.CURRENCY_COIN, gold));
                return rewards;
            }
        }
        public void SetCountMissionWin(int mValue) {
            mCountMissionWin.Value = mValue;
        }
        public void SetCurrentMission(int value)
        {
            mCurrentMissionId.Value = value;
            if(mCurrentMissionId.Value > mCountMissionWin.Value + 1)
            {
                mCountMissionWin.Value = mCurrentMissionId.Value - 1;
            }
        }

        public MissionDefinition GetCurrentMission()
        {
            int currentMissionId = mCurrentMissionId.Value;
            foreach (MissionDefinition mission in mMissions)
            {
                if (mission.id == currentMissionId)
                    return mission;
            }

            return mMissions[0];
        }

        public MissionDefinition GetCurrentBossMission()
        {
            if(mCurrentBossMissions.Count > 0)
            {
                foreach (MissionDefinition mission in mBossMissions)
                {
                    if (mission.id == mCurrentBossMissions[0])
                        return mission;
                }
            }

            return null;
        }

        public int GetNextMissionHasBoss()
        {
            if (mCountMissionWin.Value < mMissions.Count - 1)
            {
                foreach (var item in mMissions)
                {
                    if (item.id > mCurrentMissionId.Value && item.bossMissionId != 0) return item.id;
                }
            }
            return (((mCountMissionWin.Value / 10) + 1) * 10) + 1;
        }

        public MissionDefinition GetMissionById(int pId)
        {
            foreach (MissionDefinition mission in mMissions)
            {
                if (mission.id == pId)
                    return mission;
            }

            return null;
        }

        public AreaDefinition GetCurrentArea()
        {
            var missionDefinition = GetCurrentMission();
            var areaId = missionDefinition.area;
            return mAreas[areaId];
        }

        public AreaDefinition GetNextArea()
        {
            var missionDefinition = GetCurrentMission();
            var areaId = missionDefinition.area;
            var nextAreaId = areaId + 1;
            if (mAreas.ContainsKey(nextAreaId)) return mAreas[nextAreaId];
            else return null;
        }

        public List<MissionDefinition> GetListMissionInCurrentArea()
        {
            int areaId = GetCurrentMission().area;
            List<MissionDefinition> missions = new List<MissionDefinition>();
            foreach (var item in mMissions)
            {
                if (item.area == areaId) missions.Add(item);
            }

            return missions;
        }

        public bool IsFirstMission()
        {
            return mCurrentMissionId.Value == 0 || mCurrentMissionId.Value == mMissions[0].id;
        }

        public RewardInfo ClaimRandomWinStreak()
        {
            RewardInfo rewardInfo;
            if (mCountRequestWinStreak.Value == 0)
            {
                rewardInfo = LogicAPI.ClaimReward(new RewardInfo(IDs.REWARD_TYPE_PRE_UNIT, -1, 1));
            }
            else
            {
                float[] chances = new float[mWinStreakRewards.Count];
                for (int i = 0; i < mWinStreakRewards.Count; i++)
                {
                    chances[i] = mWinStreakRewards[i].ratio;
                }

                int rewardChoice = LogicAPI.CalcRandomWithChances(chances);
                rewardInfo = mWinStreakRewards[rewardChoice].ClaimReward();
            }

            mCountRequestWinStreak.Value++;
            return rewardInfo;
        }

        public void SetPreUnit(int pId)
        {
            mPreUnitId.Value = pId;
        }

        public void RemovePreUnit()
        {
            mPreUnitId.Value = -1;
        }

        #endregion

        //==============================================

        #region Private

        private void InitMissions()
        {
            //init mission
            string dataContent;
            dataContent = GameData.GetTextContent("Data/Missions");
            mMissions = JsonHelper.GetJsonList<MissionDefinition>(dataContent);
            mMissions.Sort();

            Debug.LogNewtonJson(mMissions);
        }

        private void InitBossMissions()
        {
            //init mission
            string dataContent;
            dataContent = GameData.GetTextContent("Data/BossMissions");
            mBossMissions = JsonHelper.GetJsonList<MissionDefinition>(dataContent);
            mBossMissions.Sort();

            Debug.LogNewtonJson(mBossMissions);
        }

        private void InitAreas()
        {
            //init area
            string dataContent;
            dataContent = GameData.GetTextContent("Data/Areas");
            var areas = JsonHelper.GetJsonList<AreaDefinition>(dataContent);
            areas.Sort();
            mAreas = new Dictionary<int, AreaDefinition>();
            foreach (var item in areas)
            {
                mAreas.Add(item.id, item);
            }

            Debug.LogNewtonJson(mAreas);
        }

        private void InitWinStreak()
        {
            //init mission
            string dataContent;
            dataContent = GameData.GetTextContent("Data/OfferBoosterChest");
            mWinStreakRewards = JsonHelper.GetJsonList<WinStreakRewardDefinition>(dataContent);
            mWinStreakRewards.Sort();

            Debug.LogNewtonJson(mMissions);
        }

        #endregion
    }
}
