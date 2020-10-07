using FoodZombie.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Common;
using Random = UnityEngine.Random;

namespace FoodZombie
{
    public static class LogicAPI
    {
        private static GameData GameData => GameData.Instance;

        public static List<RewardInfo> ClaimRewards(List<RewardInfo> pRewards, bool pFromGiftCode = false)
        {
            var list = new List<RewardInfo>();
            foreach (var reward in pRewards)
            {
                var r = ClaimReward(reward, pFromGiftCode);
                list.Add(r);
            }
            return list;
        }

        public static RewardInfo ClaimReward(RewardInfo pReward, bool pFromGiftCode = false)
        {
            var gameData = GameData.Instance;
            var currenciesGroup = gameData.CurrenciesGroup;
            var missionsGroup = gameData.MissionsGroup;
            var vehicleData = gameData.VehicleData;
            var heroesGroup = gameData.HeroesGroup;
            var itemsGroup = gameData.ItemsGroup;
            var wheelData = gameData.WheelData;
            var normalChestData = gameData.NormalChestData;
            var premiumChestData = gameData.PremiumChestData;
            bool randomize = pReward.Id <= -1;

            bool isRewardUnlockedCharacter = false;
            switch (pReward.Type)
            {
                case IDs.REWARD_TYPE_CURRENCY:
                    currenciesGroup.Add(pReward.Id, pReward.Value);
                    break;
                case IDs.REWARD_TYPE_POWER_UP:
                    if (!randomize)
                    {
                        itemsGroup.AddPowerUpItem(pReward.Id, pReward.Value);
                    }
                    else
                    {
                        var powerUpItem = itemsGroup.AddPowerUpItemRandomly(pReward.Value);
                        pReward.SetId(powerUpItem.Id);
                    }
                    break;
                case IDs.REWARD_TYPE_UNLOCK_CHARACTER:
                    HeroData heroData;
                    if (!randomize)
                    {
                        heroData = heroesGroup.GetHeroData(pReward.Id);
                    }
                    else
                    {
                        //if (pReward.Id == IDs.RANDOM_FROM_NORMAL_CHEST) characterData = unitsGroup.GetCharacterData(normalChestData.GetOrderedIdHero());
                        //else
                        {
                            int rarity = -1;
                            if (pReward.Id == IDs.RANDOM_FROM_NORMAL_CHEST)
                            {
                                rarity = normalChestData.RandomRarity();
                            }
                            else if (pReward.Id == IDs.RANDOM_FROM_PREMIUM_CHEST)
                            {
                                rarity = premiumChestData.RandomRarity();
                            }
                            else
                            {
                                rarity = wheelData.RandomRarity();
                            }
                            heroData = heroesGroup.GetRandomHero(rarity);
                        }
                    }

                    if (heroData.Unlocked)
                    {
                        heroData.RankUp(true);
                        isRewardUnlockedCharacter = true;
                    }
                    else
                    {
                        heroesGroup.UnlockHero(heroData.Id);
                        isRewardUnlockedCharacter = false;
                    }
                    pReward.SetId(heroData.Id);
                    break;
                case IDs.REWARD_TYPE_COIN_BY_MISSION:
                    var value = GetRewardGoldByMission(pReward.Id, pReward.Value);
                    currenciesGroup.Add(IDs.CURRENCY_COIN, value);
                    break;
                case IDs.REWARD_TYPE_PRE_UNIT:
                    HeroData characterPreUnit = null;
                    if (!randomize)
                    {
                        characterPreUnit = heroesGroup.GetHeroData(pReward.Id);
                    }
                    else
                    {
                        do
                        {
                            characterPreUnit = heroesGroup.GetRandomHero();
                        } while (characterPreUnit.MissionToUnlock >= 999);
                    }

                    missionsGroup.SetPreUnit(characterPreUnit.Id);
                    pReward.SetId(characterPreUnit.Id);
                    break;
            }

            if (MainPanel.instance != null)
            {
                if (isRewardUnlockedCharacter) MainPanel.instance.ShowEvolveRewardsPopup(pReward);
                else MainPanel.instance.ShowRewardsPopup(pReward);
                MainPanel.instance.MainMenuPanel.CheckNewHeroBuzz();
            }

            return pReward;
        }

        public static int GetRandomRewardId(int mType)
        {
            switch ((IDs.Reward_Types)mType)
            {
                case IDs.Reward_Types.REWARD_TYPE_POWER_UP:
                    return GameData.ItemsGroup.GetRandomPowerUpItem().Id;
                case IDs.Reward_Types.REWARD_TYPE_CURRENCY:
                    return IDs.CURRENCY_COIN;
            }
            return -1;
        }

        public static string GetRewardName(int pType, int pId)
        {
            bool randomize = pId <= -1;

            switch (pType)
            {
                case IDs.REWARD_TYPE_CURRENCY:
                    switch (pId)
                    {
                        case IDs.CURRENCY_CASH:
                            return Localization.Get(Localization.CASH);
                        case IDs.CURRENCY_COIN:
                            return Localization.Get(Localization.GOLD);
                    }
                    break;
                case IDs.REWARD_TYPE_POWER_UP:
                    if (!randomize)
                    {
                        var data = GameData.Instance.ItemsGroup.GetPowerUpItem(pId);
                        if (data != null)
                            return data.GetName();
                    }
                    else
                    {
                        return "Power Up";
                    }
                    break;
                case IDs.REWARD_TYPE_UNLOCK_CHARACTER:
                    if (!randomize)
                    {
                        var data = GameData.Instance.HeroesGroup.GetHeroData(pId);
                        if (data != null)
                            return string.Format(Localization.Get(Localization.UNLOCK_UNIT), data.GetName());
                    }
                    else
                    {
                        return string.Format(Localization.Get(Localization.UNLOCK_RANDOM_UNIT));
                    }
                    break;
                case IDs.REWARD_TYPE_COIN_BY_MISSION:
                    return Localization.Get(Localization.GOLD);
                    break;
                case IDs.REWARD_TYPE_PRE_UNIT:
                    if (!randomize)
                    {
                        var data = GameData.Instance.HeroesGroup.GetHeroData(pId);
                        if (data != null)
                            return string.Format(Localization.Get(Localization.PRE_UNIT), data.GetName());
                    }
                    else
                    {
                        return string.Format(Localization.Get(Localization.PRE_RANDOM_UNIT));
                    }
                    break;
            }
            return "";
        }

        public static string GetShortDescription(int pType, int pId, int pValue)
        {
            bool randomize = pId <= -1;

            switch (pType)
            {
                case IDs.REWARD_TYPE_CURRENCY:
                    switch (pId)
                    {
                        case IDs.CURRENCY_CASH:
                            return "" + pValue;
                        case IDs.CURRENCY_COIN:
                            return "" + pValue;
                    }
                    break;
                case IDs.REWARD_TYPE_COIN_BY_MISSION:
                    {
                        var value = GetRewardGoldByMission(pId, pValue);
                        return "" + value;
                    }
            }
            return "";
        }

        public static string GetDescription(int pType, int pId, int pValue)
        {
            bool randomize = pId <= -1;

            switch (pType)
            {
                case IDs.REWARD_TYPE_CURRENCY:
                    switch (pId)
                    {
                        case IDs.CURRENCY_CASH:
                            return "x" + pValue;
                        case IDs.CURRENCY_COIN:
                            return "x" + pValue;
                    }
                    break;
                case IDs.REWARD_TYPE_POWER_UP:
                    {
                        if (!randomize)
                        {
                            var data = GameData.Instance.ItemsGroup.GetPowerUpItem(pId);
                            if (data != null)
                                return data.GetDescription();
                        }
                        else
                        {
                            return "x" + pValue + " Power Up";
                        }
                    }
                    break;
                case IDs.REWARD_TYPE_UNLOCK_CHARACTER:
                    {
                        if (!randomize)
                        {
                            var data = GameData.Instance.HeroesGroup.GetHeroData(pId);
                            if (data != null)
                                return string.Format(Localization.Get(Localization.UNLOCK_UNIT), data.GetName());
                        }
                        else
                        {
                            return string.Format(Localization.Get(Localization.UNLOCK_RANDOM_UNIT));
                        }
                    }
                    break;
                case IDs.REWARD_TYPE_COIN_BY_MISSION:
                    {
                        var value = GetRewardGoldByMission(pId, pValue);
                        return "x" + value;
                    }
                    break;
                case IDs.REWARD_TYPE_PRE_UNIT:
                    if (!randomize)
                    {
                        var data = GameData.Instance.HeroesGroup.GetHeroData(pId);
                        if (data != null)
                            return string.Format(Localization.Get(Localization.PRE_UNIT), data.GetName());
                    }
                    else
                    {
                        return string.Format(Localization.Get(Localization.PRE_RANDOM_UNIT));
                    }
                    break;
            }
            return "";
        }

        public static Sprite GetRewardIcon(int pType, int pId, int pValue = -1)
        {
            bool randomize = pId <= -1;

            var gameData = GameData.Instance;
            switch ((IDs.Reward_Types)pType)
            {
                case IDs.Reward_Types.REWARD_TYPE_CURRENCY:
                    return AssetsCollection.instance.GetCurrencyIcon(pId, pValue);
                case IDs.Reward_Types.REWARD_TYPE_POWER_UP:
                    {
                        if (!randomize)
                        {
                            var data = gameData.ItemsGroup.GetPowerUpItem(pId);
                            if (data != null)
                                return data.GetIcon();
                        }
                        else
                        {
                            return AssetsCollection.instance.GetRandomPowerUpIcon();
                        }
                    }
                    break;
                case IDs.Reward_Types.REWARD_TYPE_UNLOCK_CHARACTER:
                    {
                        if (!randomize)
                        {
                            var data = gameData.HeroesGroup.GetHeroData(pId);
                            if (data != null)
                                return data.GetIcon();
                        }
                        else
                        {
                            return AssetsCollection.instance.GetRandomHeroIcon();
                        }
                    }
                    break;
                case IDs.Reward_Types.REWARD_TYPE_COIN_BY_MISSION:
                    var value = GetRewardGoldByMission(pId, pValue);
                    return AssetsCollection.instance.GetGoldIcon(value);
                    break;
                case IDs.Reward_Types.REWARD_TYPE_PRE_UNIT:
                    {
                        if (!randomize)
                        {
                            var data = gameData.HeroesGroup.GetHeroData(pId);
                            if (data != null)
                                return data.GetIcon();
                        }
                        else
                        {
                            return AssetsCollection.instance.GetRandomHeroIcon();
                        }
                    }
                    break;
            }
            return AssetsCollection.instance.common.GetAsset(FixedNames.ICON_GIFT_BOX);
        }

        /// <summary>
        /// Return a random index from an array of chances
        /// NOTE: total of chances value does not need to match 100
        /// </summary>
        public static int CalcRandomWithChances(float[] chances)
        {
            int index = 0;
            float totalRatios = 0;
            for (int i = 0; i < chances.Length; i++)
                totalRatios += chances[i];

            float random = Random.Range(0, totalRatios);
            float temp2 = 0;
            for (int i = 0; i < chances.Length; i++)
            {
                temp2 += chances[i];
                if (temp2 > random)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        public static T CalcRandomWithDictionary<T>(Dictionary<T, float> pChoices)
        {
            float[] chances = new float[pChoices.Count];
            int index = 0;
            foreach (var choice in pChoices)
            {
                chances[index] = choice.Value;
                index++;
            }

            T firstChoice = default(T);
            int randomIndex = CalcRandomWithChances(chances);
            index = 0;
            foreach (var choice in pChoices)
            {
                if (index == 0)
                    firstChoice = choice.Key;
                if (index == randomIndex)
                    return choice.Key;
                index++;
            }
            return firstChoice;
        }

        //gold theo current mission
        public static int GetRewardGoldByMission(int pFactor, int pValue)
        {
            var missionId = (GameData.Instance.MissionsGroup.CountMissionWin + 1);
            return pFactor * (520 + (missionId - 1) * 52) * pValue;
        }

        //gold lose theo current mission
        public static int GetRewardGoldLoseByMission(int pFactor, int pValue)
        {
            var missionId = (GameData.Instance.MissionsGroup.CountMissionWin + 1);
            return pFactor * (260 + (missionId - 1) * 26) * pValue;
        }

        //vehicle
        public const int VEHICLE_LEVEL_MAX = 999999999;
        public static VehicleLevelDefinition GetVehicleLevelDefinition(int level)
        {
            var result = new VehicleLevelDefinition();

            result.level = level;
            result.hp = 2000 + (level - 1) * 1000;
            result.atk = 60 + (level - 1) * 4;
            // result.startingFood = 20 + level * 4;
            // if (result.startingFood > 80) result.startingFood = 80;
            result.cost = 2000 + (level - 1) * 2000;
            result.skin = "default";

            return result;
        }

        //foodguy
        public const int FOOD_LEVEL_MAX = 25;
        public static FoodGuyLevelDefinition GetFoodGuyLevelDefinition(int level)
        {
            var result = new FoodGuyLevelDefinition();

            result.level = level;
            result.foodSpeed = 20 + (level - 1) * 5;
            result.startingFood = 5 + (level - 1) * 3;
            if (result.startingFood > 75) result.startingFood = 75;
            result.cost = 2000 + (level - 1) * 2000;
            result.skin = "1";

            return result;
        }

        //hero
        public const int HERO_LEVEL_MAX = 25;
        public static int GetHeroLevelMax(int rank) //đề phòng sau này thay đổi limit level theo rank
        {
            return HERO_LEVEL_MAX;
        }

        public static float GetHeroHP(float baseHP, int level, int rank)
        {
            return baseHP + (level - 1) * (0.05f * baseHP) + (rank - 1) * (GetHeroLevelMax(rank) - 1) * (0.05f * baseHP);
        }

        public static float GetHeroAtk(float baseAtk, int level, int rank)
        {
            return baseAtk + (level - 1) * (0.05f * baseAtk) + (rank - 1) * (GetHeroLevelMax(rank) - 1) * (0.05f * baseAtk);
        }

        public static int GetHeroCostUpgrade(int level, int rank, int rarity)
        {
            // return 100 + (level - 1) * (20 * rank) + (rank - 1) * (GetHeroLevelMax(rank)) * 20 * (rank - 1);
            int v = 150;
            int t = 40;
            if (rarity == IDs.HERO_RARITY_COMMON)
            {
                v = 100;
                t = 20;
            }
            else if (rarity == IDs.HERO_RARITY_RARE)
            {
                v = 110;
                t = 25;
            }
            else if (rarity == IDs.HERO_RARITY_EPIC)
            {
                v = 125;
                t = 30;
            }
            return v + t * rank * (level - 1);
        }

        //enemy
        public static float GetEnemyHP(float baseHP)
        {
            //return baseHP += (int)(baseHP * (((GameData.Instance.MissionsGroup.CountMissionWin + 1) - 1) * 0.05f));

            var level = GameData.Instance.MissionsGroup.CountMissionWin + 1;
            if (level % 5 == 1) return Mathf.Max(baseHP + Mathf.FloorToInt((((float)level) - 5f) / 5f) * baseHP, baseHP / 2);
            else return baseHP + Mathf.FloorToInt(((float)level) / 5f) * baseHP;
        }

        public static float GetEnemyAtk(float baseAtk)
        {
            //return baseAtk += (int)(baseAtk * (((GameData.Instance.MissionsGroup.CountMissionWin + 1) - 1) * 0.05f));

            var level = GameData.Instance.MissionsGroup.CountMissionWin + 1;
            return baseAtk + Mathf.FloorToInt(((float)level) / 5f) * baseAtk;
        }

        //Dummy
        public static float GetDummyHP()
        {
            var level = GameData.Instance.MissionsGroup.CountMissionWin + 1;
            return level * 1000f;
        }

        //normal chest
        public static void SetShowNormalChestOnMainPanelAfterWinMission()
        {
            //var missionId = (GameData.Instance.MissionsGroup.CountMissionWin + 1); //cứ mỗi 5 mission thì hiện chest 1 lần
            //if (missionId % 5 == 0)
            //{
            //    GameData.NormalChestData.ShowChest();
            //}
        }

        public static int countLockDaXuatHien = 0;
        public static int countLockChuaXuatHien = 5;
        //Mystery chest: tỉ lệ xuất hiện sau mỗi Mission là 50/50 (sau Mission 5 bắt đầu xuất hiện, trước đó sẽ không có),
        //sau khi xuất hiện thì trong 2 lần Play battle tiếp theo sẽ không push ra nữa,
        //và sau tối đa 5 lần Play mission mà ko có thì bắt buộc phải push Mystery chest
        public static bool CanShowNormalChestOnMainPanelAfterWinMission()
        {
            var missionId = (GameData.Instance.MissionsGroup.CountMissionWin + 1);
            //5 mission đầu tiên và mới vào sảnh lần đầu tiên thì không hiện
            if (missionId < 6) return false;
            if ((missionId == 7 && MainMenuPanel.showCount >= 1)
                || MainMenuPanel.showCount == 0)
            {
                countLockDaXuatHien = 2;
                countLockChuaXuatHien = 0;
                return true;
            }

            bool r = false;
            if (countLockDaXuatHien > 0)
            {
                r = false;
            }
            else if (countLockChuaXuatHien >= 5)
            {
                r = true;
            }
            else
            {
                r = Random.Range(0, 2) == 0;
            }
            if (!r)
            {
                countLockDaXuatHien--;
                countLockChuaXuatHien++;
            }
            else
            {
                countLockDaXuatHien = 2;
                countLockChuaXuatHien = 0;
            }
            return r;

            //if (GameData.NormalChestData.CountShowedChest == 0)
            //{
            //    //lần đầu hiện chest sau các màn % 5 thì hiển thị 100%
            //    GameData.NormalChestData.CountShowChest();
            //    return true;
            //}
            //else
            //{
            //    //các lần sau random 50%
            //    GameData.NormalChestData.CountShowChest();
            //    return Random.Range(0, 2) == 0;
            //}

            //return false;
        }

        //feature
        public static bool CanShowUpgrade()
        {
            return (GameData.Instance.MissionsGroup.CountMissionWin + 1) >= 2;
        }

        //feature
        public static bool CanShowPreUnit()
        {
            return (GameData.Instance.MissionsGroup.CountMissionWin + 1) >= 3;
        }

        //feature
        public static bool CanShowWheelPanel()
        {
            return (GameData.Instance.MissionsGroup.CountMissionWin + 1) >= 3;
        }

        //feature
        public static bool CanShowSafe1Panel()
        {
            return (GameData.Instance.MissionsGroup.CountMissionWin + 1) >= 0;
        }

        //feature
        public static bool CanShowSafe2Panel()
        {
            return (GameData.Instance.MissionsGroup.CountMissionWin + 1) >= 16;
        }

        //feature
        public static bool CanShowSafe3Panel()
        {
            return (GameData.Instance.MissionsGroup.CountMissionWin + 1) >= 31;
        }

        //feature
        public static bool CanShowRatePanel()
        {
            return (GameData.Instance.MissionsGroup.CountMissionWin + 1) >= 7;
        }

        //feature
        public static bool CanShowBossPanel()
        {
            return (GameData.Instance.MissionsGroup.CountMissionWin + 1) >= 6;
        }

        //bonus mission
        public static int GoldBonus(int indexBonusCount)
        {
            return 260 * indexBonusCount;
        }
    }
}