using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Common;
using Utilities.Pattern.Data;

namespace FoodZombie
{
    [System.Serializable]
    public class EnemyDefinition : IComparable<EnemyDefinition>
    {
        /// <summary>
        /// This is a part of old prototype, which hardly to be removed because of old terible data management.
        /// </summary>
        public int id;
        public string name;
        public int rarity;
        public string skin;
        public float hp;
        public float meleeAtk;
        public float rangeAtk;
        public int mainAttackType;
        public float crit;
        public float speed;
        public float meleeAtkSpeed;
        public float rangeAtkSpeed;
        public float atkRange;
        public int clipSize;
        public int rageGain;

        public int CompareTo(EnemyDefinition other)
        {
            return id.CompareTo(other.id);
        }
    }

    public class EnemyData : DataGroup
    {
        public EnemyDefinition baseData { get; private set; }
        private BoolData mUnlocked;
        private BoolData mViewed;
        private IntegerData mKillsCount;

        public bool Unlocked => mUnlocked.Value;
        public bool Viewed => mViewed.Value;
        public int KillsCount => mKillsCount.Value;

        public int Rarity => baseData.rarity;
        public float Hp => LogicAPI.GetEnemyHP(baseData.hp);
        public float Atk
        {
            get
            {
                float atk;
                if (baseData.mainAttackType == IDs.MELEE)
                    atk = baseData.meleeAtk;
                else
                    atk = baseData.rangeAtk;

                return LogicAPI.GetEnemyAtk(atk);
            }
        }
        public float MeleeAtk => LogicAPI.GetEnemyAtk(baseData.meleeAtk);
        public float RangeAtk => LogicAPI.GetEnemyAtk(baseData.rangeAtk);
        public int MainAttackType => baseData.mainAttackType;
        public float Crit => baseData.crit;
        public float Speed => baseData.speed;
        public float AtkSpeed
        {
            get
            {
                float atkSpeed;
                if (baseData.mainAttackType == IDs.MELEE)
                    atkSpeed = baseData.meleeAtkSpeed;
                else
                    atkSpeed = baseData.rangeAtkSpeed;

                return atkSpeed;
            }
        }
        public float MeleeAtkSpeed => baseData.meleeAtkSpeed;
        public float RangeAtkSpeed => baseData.rangeAtkSpeed;
        public int ClipSize => baseData.clipSize;
        public int RageGain => baseData.rageGain;

        public EnemyData(int pId, EnemyDefinition pBaseData) : base(pId)
        {
            baseData = pBaseData;
            mUnlocked = AddData(new BoolData(0, false));
            mViewed = AddData(new BoolData(1, false));
            mKillsCount = AddData(new IntegerData(2));
        }

        public void Unlock()
        {
            mUnlocked.Value = true;
        }

        public void View()
        {
            mViewed.Value = true;
        }

        public Spine.Unity.SkeletonDataAsset GetSkeletonData()
        {
            return AssetsCollection.instance.enemyAnimations.GetAsset(Id - 1);
        }

        public string GetName()
        {
            return Localization.Get("ENEMY_NAME_" + baseData.id);
        }

        public string GetDescription()
        {
            return Localization.Get("ENEMY_DESCRIPTION_" + baseData.id);
        }

        public string GetRarityName()
        {
            return Localization.Get("ENEMY_RARITY_" + baseData.rarity);
        }

        public string GetSkinName()
        {
            //return baseData.skin;
            return "skin1";
        }

        public Sprite GetIcon()
        {
            return AssetsCollection.instance.enemyIcon.GetAsset(Id - 1);
        }

        public void AddKillsCount(int pValue)
        {
            mKillsCount.Value += pValue;
        }
    }

    public class EnemiesGroup : DataGroup
    {
        #region Members

        private DataGroup mEnemiesGroup;
        private IntegerData mLastZombieViewedId;

        public int LastZombieViewedId { get { return mLastZombieViewedId.Value; } }
        #endregion

        //=============================================

        #region Public

        public EnemiesGroup(int pId) : base(pId)
        {
            //Declare sub groups which contain units data
            mEnemiesGroup = AddData(new DataGroup(0));
            mLastZombieViewedId = AddData(new IntegerData(1, 0));

            InitEnemiesGroup();
        }

        public EnemyData GetEnemyData(int pId)
        {
            return mEnemiesGroup.GetData<EnemyData>(pId);
        }

        public EnemyData GetRandomEnemy()
        {
            return mEnemiesGroup.GetRandomData<EnemyData>();
        }

        public List<EnemyData> GetAllEnemyDatas()
        {
            var list = new List<EnemyData>();
            foreach (EnemyData item in mEnemiesGroup.Children)
                    list.Add(item);
            return list;
        }

        public void UnlockEnemy(int pId)
        {
            var unit = GetEnemyData(pId);
            if (unit != null)
                unit.Unlock();
        }

        public void UnlockAllEnemies()
        {
            foreach (EnemyData unit in mEnemiesGroup.Children)
            {
                unit.Unlock();
            }
        }

        public void SetLastZombieViewed(int pLastZombieViewedId)
        {
            mLastZombieViewedId.Value = (pLastZombieViewedId);
        }

        #endregion

        //==============================================

        #region Private

        private void InitEnemiesGroup()
        {
            //Read json data from resouce file
            var dataContent = GameData.GetTextContent("Data/Enemies");
            //Parse json data to list objects
            var collection = JsonHelper.GetJsonList<EnemyDefinition>(dataContent);
            if (collection != null)
            {
                collection.Sort();
                foreach (var item in collection)
                {
                    //Declare unit data, then push it into a data group
                    var data = new EnemyData(item.id, item);
                    mEnemiesGroup.AddData(data);
                }
            }
        }

        #endregion
    }
}
