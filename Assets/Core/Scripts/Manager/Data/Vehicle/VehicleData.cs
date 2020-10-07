
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
    public class VehicleLevelDefinition : IComparable<VehicleLevelDefinition>
    {
        public int level;
        public int cost;
        public int startingFood;
        public float hp;
        public float atk;
        public float atkSpeed;
        public float atkSkill;
        public string skin;

        public int CompareTo(VehicleLevelDefinition other)
        {
            return level.CompareTo(other.level);
        }
    }

    //==================================

    public class VehicleData : DataGroup
    {
        #region Members
        // private Dictionary<int, VehicleLevelDefinition> mLevels;
        private IntegerData mLevel;
        private VehicleLevelDefinition mInfo;

        public int Level => mLevel.Value;

        #endregion

        //=============================================

        #region Public

        public VehicleData(int pId) : base(pId)
        {
            mLevel = AddData(new IntegerData(0, 1));
            //InitLevels();
        }

        public override void PostLoad()
        {
            base.PostLoad();

            mInfo = LogicAPI.GetVehicleLevelDefinition(mLevel.Value);
        }

        public float GetHp()
        {
            return mInfo.hp;
        }

        public float GetAtk()
        {
            return mInfo.atk;
        }

        public float GetAtkSpeed()
        {
            return mInfo.atkSpeed;
        }

        public float GetAtkSkill()
        {
            return mInfo.atkSkill;
        }

        public int GetCurrentCostUpgrade()
        {
            if (IsMaxLevel())
                return 0;
            return LogicAPI.GetVehicleLevelDefinition(mLevel.Value).cost;
        }

        public bool IsMaxLevel()
        {
            // int finalLevel = mLevels[mLevels.Count - 1].level;
            return mLevel.Value >= LogicAPI.VEHICLE_LEVEL_MAX;  // finalLevel <= mLevel.Value;
        }

        public VehicleLevelDefinition GetCurrentLevelInfo()
        {
            return mInfo;
        }

        public VehicleLevelDefinition GetNextLevelInfo()
        {
            if (IsMaxLevel())
                return null;
            return LogicAPI.GetVehicleLevelDefinition(mLevel.Value + 1);
        }

        public bool LevelUp()
        {
            if (IsMaxLevel())
            {
                return false;
            }
            else
            {
                mLevel.Value += 1;
                mInfo = LogicAPI.GetVehicleLevelDefinition(mLevel.Value);
                EventDispatcher.Raise(new VehicleUpgradeEvent());
                return true;
            }
        }

        public string GetSkillName()
        {
            if (Level > 4) return "skin4";
            return "skin" + Level;
        }

        #endregion

        //==============================================

        #region Private

        //private void InitLevels()
        //{
        //    var dataContent = GameData.GetTextContent("Data/Vehicle");
        //    var levels = JsonHelper.GetJsonList<VehicleLevelDefinition>(dataContent);
        //    levels.Sort();
        //    mLevels = new Dictionary<int, VehicleLevelDefinition>();
        //    foreach (var item in levels)
        //    {
        //        mLevels.Add(item.level, item);
        //    }

        //    Debug.LogNewtonJson(levels);
        //}

        #endregion
    }
}