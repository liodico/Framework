using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Common;

namespace FoodZombie
{
    public class CurrencyChangedEvent : BaseEvent
    {
        public int id;
        public int additionalValue;
        public CurrencyChangedEvent(int pId, int pAdditionalValue)
        {
            id = pId;
            additionalValue = pAdditionalValue;
        }
    }

    public class TeamChangedEvent : BaseEvent
    {
        public int id;
        public TeamChangedEvent(int pid) { id = pid; }
    }

    public class HeroUnlockedEvent : BaseEvent
    {
        public int id;
        public HeroUnlockedEvent(int pId) { id = pId; }
    }

    public class HeroEquipEvent : BaseEvent
    {
        public int id;
        public HeroEquipEvent(int pId) { id = pId; }
    }

    public class HeroUpgradeEvent : BaseEvent
    {
        public int id;
        public HeroUpgradeEvent(int pId) { id = pId; }
    }

    public class BattleLoadEvent : BaseEvent { }

    public class MissionCompletedEvent : BaseEvent
    {
        public MissionDefinition missionData;
        public Dictionary<int, int> powerupIds;
        public Dictionary<int, int> totalKills;
        public bool completeSpecialWave;
        public bool usedFood;
        public bool usedRage;
        public bool isBossMission;
        public MissionCompletedEvent(MissionDefinition pMissionData, bool pIsBossMission, Dictionary<int, int> pPowerupIds, Dictionary<int, int> pTotalKills, bool pCompletedSpecialWave, bool pUsedFood, bool pUsedRage)
        {
            missionData = pMissionData;
            powerupIds = pPowerupIds;
            totalKills = pTotalKills;
            completeSpecialWave = pCompletedSpecialWave;
            usedFood = pUsedFood;
            usedRage = pUsedRage;
            isBossMission = pIsBossMission;
        }
    }

    public class EnemyKilledEvent : BaseEvent
    {
        public int id;
        public bool isBoss;
        public EnemyKilledEvent(int pId, bool pIsBoss)
        {
            id = pId;
            isBoss = pIsBoss;
        }
    }

    public class TutorialFinishedEvent : BaseEvent
    {
        public int tutorial;
        public TutorialFinishedEvent(int pId)
        {
            tutorial = pId;
        }
    }

    public class TutorialTriggeredEvent : BaseEvent
    {
        public int tutorial;
        public TutorialTriggeredEvent(int pId)
        {
            tutorial = pId;
        }
    }

    public class ShowRewardsEvent : BaseEvent
    {
        public List<RewardInfo> rewards;
        public ShowRewardsEvent(List<RewardInfo> pRewards)
        {
            rewards = pRewards;
        }
    }

    public class StockPowerUpItemChangedEvent : BaseEvent
    {
        public int id;
        public StockPowerUpItemChangedEvent(int pId)
        {
            id = pId;
        }
    }

    public class ResetTimeBossEvent : BaseEvent { }

    public class VehicleUpgradeEvent : BaseEvent { }

    public class FoodGuyUpgradeEvent : BaseEvent { }

    public class SafeChangeValueEvent : BaseEvent
    {
        public int id;
        public bool value;
        public SafeChangeValueEvent(int pId, bool pValue)
        {
            id = pId;
            value = pValue;
        }
    }

    public class GamePaymentInitializedEvent : BaseEvent { }
}
