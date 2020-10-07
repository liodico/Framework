using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Inspector;


namespace FoodZombie
{
    public class AssetGetter<T> where T : Object
    {
        public List<T> source;

        public AssetGetter(List<T> pSource)
        {
            source = pSource;
        }

        public AssetGetter<T> Init(List<T> pSource)
        {
            source = pSource;
            return this;
        }

        public T GetAsset(string pName)
        {
            foreach (var s in source)
                if (s != null && pName != null && s.name.ToLower() == pName.ToLower())
                    return s;

            Debug.LogError(string.Format("Not found {0} with name {1}", typeof(T).Name, pName));
            return null;
        }

        public T GetAsset(int pIndex)
        {
            if (pIndex < 0 || pIndex >= source.Count)
            {
                Debug.LogError(string.Format("Index {0} {1} is invalid!", pIndex, typeof(T).Name));
                return default(T);
            }
            return source[pIndex];
        }

        public int GetAssetIndex(string pName)
        {
            for (int i = 0; i < source.Count; i++)
            {
                if (source[i].name == pName)
                    return i;
            }
            Debug.LogError(string.Format("Not found {0} with name {1}", typeof(T).Name, pName));
            return -1;
        }
    }

    //============================================================

    public static class FixedNames
    {
        public const string ICON_GOLD = "iconCoin";
        public const string ICON_GOLD_REWARD_1 = "iconCoin1";
        public const string ICON_GOLD_REWARD_2 = "iconCoin2";
        public const string ICON_GOLD_REWARD_3 = "iconCoin3";
        public const string ICON_CASH = "iconDiamond";
        public const string ICON_CASH_REWARD_1 = "PrizeDiamond";
        public const string ICON_RAGE = "iconRage";
        public const string ICON_EMPTY_PERK = "perkIconEmpty";
        public const string ICON_GIFT_BOX = "iconRewardBoxSmall";
        public const string ICON_RANDOM_HERO = "unitRandom";
        public const string ICON_RANDOM_POWER_UP = "iconRandomPowerUp";


        public const string TEXT_KNIFE = "\u0001";
        public const string TEXT_GUN = "\u0002";
        public const string TEXT_LUCK = "\u0003";
        public const string TEXT_HP = "\u0004";
        public const string TEXT_ATK = "\u0005";
    }

    //============================================================

    [CreateAssetMenu(fileName = "AssetsCollection", menuName = "Assets/Scriptable Objects/Create Assets Collection")]
    public class AssetsCollection : ScriptableObject
    {
        #region Constants

        #endregion

        //========================================================

        #region Members 

        private static AssetsCollection mInstance;
        public static AssetsCollection instance
        {
            get
            {
                if (mInstance == null)
                    mInstance = Resources.Load<AssetsCollection>("Collection/AssetsCollection");
                return mInstance;
            }
        }

        //private Material mMatGrey;
        //public Material matGrey
        //{
        //    get
        //    {
        //        if (mMatGrey == null)
        //            mMatGrey = new Material(Shader.Find("NBCustom/Sprites/Greyscale"));
        //        return mMatGrey;
        //    }
        //}

        [Separator("Common")]
        public Material matDefaultSekelton;
        public Material matSkeletonFill;
        public Material matImageFill;

        [Separator("Icons Managed by Name")]
        public List<Sprite> commonSprites;
        public List<Sprite> unitPerkSprites;

        [Separator("Icons Managed by Id")]
        public List<Sprite> unitSprites;
        public List<Sprite> heroIconSprites;
        public List<Sprite> enemyIconSprites;
        public List<Sprite> powerUpItemSprites;
        public List<Sprite> skillSprites;
        public List<Sprite> rankSprites;

        [Separator("Animation")]
        public List<SkeletonDataAsset> heroAnimationDataAssets;
        public List<SkeletonDataAsset> enemyAnimationDataAssets;

        private AssetGetter<Sprite> mSpritesGetter = new AssetGetter<Sprite>(null);
        public AssetGetter<Sprite> common => mSpritesGetter.Init(commonSprites);
        public AssetGetter<Sprite> unitPerk => mSpritesGetter.Init(unitPerkSprites);
        public AssetGetter<Sprite> unit => mSpritesGetter.Init(unitSprites);
        public AssetGetter<Sprite> heroIcon => mSpritesGetter.Init(heroIconSprites);
        public AssetGetter<Sprite> enemyIcon => mSpritesGetter.Init(enemyIconSprites);
        public AssetGetter<Sprite> powerUpItem => mSpritesGetter.Init(powerUpItemSprites);
        public AssetGetter<Sprite> skils => mSpritesGetter.Init(skillSprites);
        public AssetGetter<Sprite> ranks => mSpritesGetter.Init(rankSprites);
        private AssetGetter<SkeletonDataAsset> mAnimationsGetter = new AssetGetter<SkeletonDataAsset>(null);
        public AssetGetter<SkeletonDataAsset> heroAnimations => mAnimationsGetter.Init(heroAnimationDataAssets);
        public AssetGetter<SkeletonDataAsset> enemyAnimations => mAnimationsGetter.Init(enemyAnimationDataAssets);


        #endregion

        //=======================================================

        #region Private

        private T GetAsset<T>(List<T> pSource, string pName) where T : UnityEngine.Object
        {
            foreach (var s in pSource)
                if (s != null && pName != null && s.name.ToLower() == pName.ToLower())
                    return s;

            Debug.LogError(string.Format("Not found {0} with name {1}", typeof(T).Name, pName));
            return null;
        }

        private T GetAsset<T>(List<T> pSource, int pIndex)
        {
            if (pIndex < 0 || pIndex >= pSource.Count)
            {
                Debug.LogError(string.Format("Index {0} {1} is invalid!", pIndex, typeof(T).Name));
                return default(T);
            }
            return pSource[pIndex];
        }

        private int GetAssetIndex<T>(List<T> pSource, string pName) where T : UnityEngine.Object
        {
            for (int i = 0; i < pSource.Count; i++)
            {
                if (pSource[i].name == pName)
                    return i;
            }
            Debug.LogError(string.Format("Not found {0} with name {1}", typeof(T).Name, pName));
            return -1;
        }

        #endregion

        //=======================================================

        #region Public

        public Sprite GetCurrencyIcon(int pType, int pValue = -1)
        {
            switch (pType)
            {
                case IDs.CURRENCY_CASH:
                    if (pValue == -1) return common.GetAsset(FixedNames.ICON_CASH);
                    else return GetCashIcon(pValue);
                case IDs.CURRENCY_COIN:
                    if (pValue == -1) return common.GetAsset(FixedNames.ICON_GOLD);
                    else return GetGoldIcon(pValue);
                default:
                    return null;
            }
        }

        public Sprite GetCashIcon(int pValue)
        {
            return common.GetAsset(FixedNames.ICON_CASH_REWARD_1);
        }

        public Sprite GetGoldIcon(int pValue)
        {
            if (pValue <= -1) return common.GetAsset(FixedNames.ICON_GOLD);
            else if (pValue <= LogicAPI.GetRewardGoldByMission(1, 1)) return common.GetAsset(FixedNames.ICON_GOLD_REWARD_1);
            else if (pValue <= LogicAPI.GetRewardGoldByMission(1, 2)) return common.GetAsset(FixedNames.ICON_GOLD_REWARD_2);
            else return common.GetAsset(FixedNames.ICON_GOLD_REWARD_3);
        }

        public Sprite GetRandomHeroIcon()
        {
            return common.GetAsset(FixedNames.ICON_RANDOM_HERO);
        }

        public Sprite GetRandomPowerUpIcon()
        {
            return common.GetAsset(FixedNames.ICON_RANDOM_POWER_UP);
        }

        public Sprite GetRankIcon(int pRank)
        {
            if (pRank > ranks.source.Count) pRank = ranks.source.Count;
            return ranks.GetAsset(pRank - 1);
        }

        public void Init()
        {
            if (heroAnimationDataAssets != null)
            {
                int length = heroAnimationDataAssets.Count;
                for (int i = 0; i < length; i++)
                {
                    heroAnimationDataAssets[i].GetSkeletonData(false);
                }
            }
            if (enemyAnimationDataAssets != null)
            {
                int length = enemyAnimationDataAssets.Count;
                for (int i = 0; i < length; i++)
                {
                    enemyAnimationDataAssets[i].GetSkeletonData(false);
                }
            }
        }
        #endregion
    }
}