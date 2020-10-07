
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
    public class FoodGuyLevelDefinition : IComparable<FoodGuyLevelDefinition>
    {
        public int level;
        public int cost;
        public int startingFood;
        public int foodSpeed;
        public string skin;

        public int CompareTo(FoodGuyLevelDefinition other)
        {
            return level.CompareTo(other.level);
        }
    }

    //==================================

    public class FoodGuyData : DataGroup
    {
        #region Members
        // private Dictionary<int, FoodGuyLevelDefinition> mLevels;
        private IntegerData mLevel;
        private FoodGuyLevelDefinition mInfo;

        public int Level => mLevel.Value;

        #endregion

        //=============================================

        #region Public

        public FoodGuyData(int pId) : base(pId)
        {
            mLevel = AddData(new IntegerData(0, 1));
            //InitLevels();
        }

        public override void PostLoad()
        {
            base.PostLoad();

            mInfo = LogicAPI.GetFoodGuyLevelDefinition(mLevel.Value);
        }

        public int GetStartingFood()
        {
            return mInfo.startingFood;
        }

        public int GetFoodSpeed()
        {
            return mInfo.foodSpeed;
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
            return mLevel.Value >= LogicAPI.FOOD_LEVEL_MAX; // finalLevel <= mLevel.Value;
        }

        public FoodGuyLevelDefinition GetCurrentLevelInfo()
        {
            return mInfo;
        }

        public FoodGuyLevelDefinition GetNextLevelInfo()
        {
            if (IsMaxLevel())
                return null;
            return LogicAPI.GetFoodGuyLevelDefinition(mLevel.Value + 1);
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
                mInfo = LogicAPI.GetFoodGuyLevelDefinition(mLevel.Value);
                EventDispatcher.Raise(new FoodGuyUpgradeEvent());
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
        //    var dataContent = GameData.GetTextContent("Data/FoodGuy");
        //    var levels = JsonHelper.GetJsonList<FoodGuyLevelDefinition>(dataContent);
        //    levels.Sort();
        //    mLevels = new Dictionary<int, FoodGuyLevelDefinition>();
        //    foreach (var item in levels)
        //    {
        //        mLevels.Add(item.level, item);
        //    }

        //    Debug.LogNewtonJson(levels);
        //}

        #endregion
    }
}