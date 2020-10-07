using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Common;
using Utilities.Pattern.Data;

namespace FoodZombie
{
    [System.Serializable]
    public class HeroDefinition : IComparable<HeroDefinition>
    {
        /// <summary>
        /// This is a part of old prototype, which hardly to be removed because of old terible data management.
        /// </summary>
        public int id;
        public string name;
        public int rarity;
        public float hp;
        public float meleeAtk;
        public float rangeAtk;
        public int mainAttackType;
        public float crit;
        public float speed;
        public float meleeAtkSpeed;
        public float rangeAtkSpeed;
        public int foodRequired;
        public int rageRequired;
        public float cooldown;
        public float prepare;
        public int clipSize;
        public int rageGain;
        public int missionToUnlock;

        public int CompareTo(HeroDefinition other)
        {
            return id.CompareTo(other.id);
        }
    }

    public class HeroData : DataGroup
    {
        private HeroDefinition baseData;
        private BoolData mUnlocked;
        private BoolData mViewed;
        private IntegerData mLevel;
        private IntegerData mRank;

        public bool Unlocked => mUnlocked.Value;
        public bool Viewed => mViewed.Value;
        public int Level => mLevel.Value;
        public int Rank => mRank.Value;

        public int Rarity => baseData.rarity;
        public float Hp => LogicAPI.GetHeroHP(baseData.hp, mLevel.Value, mRank.Value);
        public float Atk
        {
            get
            {
                float atk;
                if (baseData.mainAttackType == IDs.MELEE)
                    atk = baseData.meleeAtk;
                else
                    atk = baseData.rangeAtk;

                return LogicAPI.GetHeroAtk(atk, mLevel.Value, mRank.Value);
            }
        }
        public float MeleeAtk => LogicAPI.GetHeroAtk(baseData.meleeAtk, mLevel.Value, mRank.Value);
        public float RangeAtk => LogicAPI.GetHeroAtk(baseData.rangeAtk, mLevel.Value, mRank.Value);
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
        public int FoodRequired => baseData.foodRequired;
        public int RageRequired => baseData.rageRequired;
        public float Cooldown => baseData.cooldown;
        public float Prepare => baseData.prepare;
        public int ClipSize => baseData.clipSize;
        public int RageGain => baseData.rageGain;
        public int MissionToUnlock => baseData.missionToUnlock;
        public int CostUpgrade => LogicAPI.GetHeroCostUpgrade(mLevel.Value, mRank.Value, baseData.rarity);

        public HeroData(int pId, HeroDefinition pBaseData) : base(pId)
        {
            baseData = pBaseData;
            mUnlocked = AddData(new BoolData(0, false));
            mViewed = AddData(new BoolData(1, false));
            mLevel = AddData(new IntegerData(2, 1));
            mRank = AddData(new IntegerData(3, 1));
        }

        public void Unlock()
        {
            if (mUnlocked.Value) return;
            mUnlocked.Value = true;

            EventDispatcher.Raise(new HeroUnlockedEvent(Id));
        }

        public void View()
        {
            mViewed.Value = true;
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
                EventDispatcher.Raise(new HeroUpgradeEvent(Id));
                return true;
            }
        }

        public bool RankUp(bool pForce = false)
        {
            if (!IsMaxLevel() && !pForce)
            {
                return false;
            }
            else
            {
                mLevel.Value = 1;
                mRank.Value += 1;
                EventDispatcher.Raise(new HeroUpgradeEvent(Id));
                return true;
            }
        }

        public int GetLevelMax()
        {
            return LogicAPI.GetHeroLevelMax(mRank.Value);
        }

        internal bool IsMaxLevel()
        {
            return mLevel.Value >= GetLevelMax();
        }

        public Spine.Unity.SkeletonDataAsset GetSkeletonData()
        {
            return AssetsCollection.instance.heroAnimations.GetAsset(Id - 1);
        }

        public string GetName()
        {
            return Localization.Get("HERO_NAME_" + baseData.id);
        }

        public string GetDescription()
        {
            return Localization.Get("HERO_DESCRIPTION_" + baseData.id);
        }

        public string GetRarityName()
        {
            return Localization.Get("HERO_RARITY_" + baseData.rarity);
        }

        public string GetSkinName()
        {
            if (Rank > 4) return "skin4";
            return "skin" + Rank;
        }

        public Sprite GetRankIcon()
        {
            return AssetsCollection.instance.GetRankIcon(mRank.Value);
        }

        public Sprite GetIcon()
        {
            return AssetsCollection.instance.heroIcon.GetAsset(Id - 1);
        }
        
        //next level for UpgradeHeroPanel
        public float GetNextLevelHP()
        {
            return LogicAPI.GetHeroHP(baseData.hp, mLevel.Value + 1, mRank.Value);
        }

        public float GetNextLevelAtk()
        {
            float atk;
            if (baseData.mainAttackType == IDs.MELEE)
                atk = baseData.meleeAtk;
            else
                atk = baseData.rangeAtk;

            return LogicAPI.GetHeroAtk(atk, mLevel.Value + 1, mRank.Value);
        }

        public bool IsEquipped()
        {
            return GameData.Instance.HeroesGroup.HasEquippedUnit(Id);
        }
    }

    public class HeroesGroup : DataGroup
    {
        #region Members

        private DataGroup mHeroesGroup;
        private ListData<int> mHeroesEquipped;
        private IntegerData mCountClaimHero;
        private IntegerData mClaimHeroId;

        public List<int> CharactersEquipped { get { return mHeroesEquipped.GetValues(); } }
        public int CountClaimHero { get { return mCountClaimHero.Value; } }
        public int ClaimHeroId { get { return mClaimHeroId.Value; } }

        #endregion

        //=============================================

        #region Public

        public override void PostLoad()
        {
            base.PostLoad();

            int count = mHeroesEquipped.Count;
            foreach (HeroData unit in mHeroesGroup.Children)
            {
                if (unit.Id == Constants.DEFAULT_CHARACTER_ID)
                {
                    unit.Unlock();
                    if (count == 0)
                    {
                        mHeroesEquipped.Add(unit.Id);
                        count++;
                    }
                    break;
                }
            }
        }

        public HeroesGroup(int pId) : base(pId)
        {
            //Declare sub groups which contain units data
            mHeroesGroup = AddData(new DataGroup(0));
            mHeroesEquipped = AddData(new ListData<int>(1, new List<int>()));
            mCountClaimHero = AddData(new IntegerData(2, 0));
            mClaimHeroId = AddData(new IntegerData(3, -1));

            InitHeroesGroup();
        }

        public HeroData GetHeroData(int pId)
        {
            return mHeroesGroup.GetData<HeroData>(pId);
        }

        public HeroData GetRandomHero(int pRarity = -1)
        {
            var list = GetAllHeroDatas();

            // Neu it hero qua thi ra hero moi
            if (mHeroesEquipped.Count < 4)
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    var hero = list[i];
                    foreach (var item in mHeroesEquipped.GetValues())
                    {
                        if (item == hero.Id)
                        {
                            list.RemoveAt(i);
                            break;
                        }
                    }
                }
            }

            if (pRarity != -1)
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    var hero = list[i] as HeroData;
                    if (hero.Rarity != pRarity
                        || (GameData.Instance.MissionsGroup.CountMissionWin + 1) <= hero.MissionToUnlock)
                    {
                        list.RemoveAt(i);
                    }
                }

                //nếu list theo rarity rỗng thì chỉ random common / rare
                if (list.Count <= 0)
                {
                    list = GetAllHeroDatas();
                    for (int i = list.Count - 1; i >= 0; i--)
                    {
                        var hero = list[i] as HeroData;
                        if (hero.Rarity != IDs.HERO_RARITY_COMMON && hero.Rarity != IDs.HERO_RARITY_RARE
                            || (GameData.Instance.MissionsGroup.CountMissionWin + 1) <= hero.MissionToUnlock)
                        {
                            list.RemoveAt(i);
                        }
                    }
                }
            }

            return list[UnityEngine.Random.Range(0, list.Count)] as HeroData;
        }

        public List<HeroData> GetAllHeroDatas()
        {
            var list = new List<HeroData>();
            foreach (HeroData item in mHeroesGroup.Children)
                    list.Add(item);
            return list;
        }

        public void UnlockHero(int pId)
        {
            var unit = GetHeroData(pId);
            if (unit != null)
                unit.Unlock();

            if (!IsMaxEquippedUnits())
            {
                EquipHero(pId);
            }
        }

        public void UnlockAllHeroes()
        {
            foreach (HeroData unit in mHeroesGroup.Children)
            {
                UnlockHero(unit.Id);
            }
        }

        public bool HasEquippedUnit(int pId)
        {
            return mHeroesEquipped.Contain(pId);
        }

        public List<HeroData> GetEquippedHeroes()
        {
            var list = new List<HeroData>();
            int count = mHeroesEquipped.Count;
            for (int i = 0; i < count; i++)
            {
                int id = mHeroesEquipped[i];
                list.Add(GetHeroData(id));
            }
            return list;
        }

        public void RemainShowClaimHero(int pHeroId)
        {
            mClaimHeroId.Value = pHeroId;
        }

        public void ClaimHero()
        {
            mCountClaimHero.Value++;
            mClaimHeroId.Value = -1;
        }

        public void ResetClaimHero()
        {
            mClaimHeroId.Value = -1;
        }

        public bool IsMaxEquippedUnits()
        {
            return mHeroesEquipped.Count >= Constants.MAX_UNITS_CAN_EQUIP;
        }

        public bool EquipHero(int pId)
        {
            if (mHeroesEquipped.Contain(pId) && !IsMaxEquippedUnits())
                return false;
            mHeroesEquipped.Add(pId);

            EventDispatcher.Raise(new HeroEquipEvent(pId));
            return true;
        }

        public bool ReplaceUnit(int pOldId, int pNewId)
        {
            if (!mHeroesEquipped.Contain(pOldId))
                return false;
            for (int i = 0; i < mHeroesEquipped.Count; i++)
            {
                if (mHeroesEquipped[i] == pOldId) mHeroesEquipped[i] = pNewId;
            }

            EventDispatcher.Raise(new HeroEquipEvent(pOldId));
            return true;
        }

        public List<HeroData> GetHerosCanUnlock()
        {
            var heroes = GetAllHeroDatas();
            var length = heroes.Count;
            var missionId = (GameData.Instance.MissionsGroup.CountMissionWin + 1);
            for (int i = length - 1; i >= 0; i--)
            {
                var hero = heroes[i] as HeroData;
                if (missionId <= hero.MissionToUnlock)
                {
                    heroes.RemoveAt(i);
                }
            }

            //sort theo rarity
            length = heroes.Count;
            if (length >= 2)
            {
                for (int i = 0; i < length - 1; i++)
                {
                    var hero1 = heroes[i] as HeroData;
                    for (int j = i + 1; j < length; j++)
                    {
                        var hero2 = heroes[j] as HeroData;
                        if (hero1.Rarity > hero2.Rarity)
                        {
                            var temp = heroes[i];
                            heroes[i] = heroes[j];
                            heroes[j] = temp;
                        }
                    }
                }
            }
            return heroes;
        }

        public bool HasUpgradableCharacter()
        {
            foreach (HeroData data in mHeroesGroup.Children)
                if (data.Unlocked)
                {
                    return true;
                }
            return false;
        }

        #endregion

        //==============================================

        #region Private

        private void InitHeroesGroup()
        {
            //Read json data from resouce file
            var dataContent = GameData.GetTextContent("Data/Heroes");
            //Parse json data to list objects
            var collection = JsonHelper.GetJsonList<HeroDefinition>(dataContent);
            if (collection != null)
            {
                collection.Sort();
                foreach (var item in collection)
                {
                    //Declare unit data, then push it into a data group
                    var data = new HeroData(item.id, item);
                    mHeroesGroup.AddData(data);
                }
            }
        }

        #endregion
    }
}
