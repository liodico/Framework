using System;

public class IDs
{
	#region SFXs
	public const int SOUND_UI_POPUP_OPEN = 1;
	public const int SOUND_UI_POPUP_CLOSE = 2;
	public const int SOUND_UI_BUTTON_CLICK = 3;
	public const int SOUND_CB_ENEMYBASE_BEATTACK = 4;
	public const int SOUND_CB_ALLYBASE_BEATTACK = 5;
	public const int SOUND_CB_ENEMYBASE_DESTROYED = 6;
	public const int SOUND_CB_ALLYBASE_DESTROYED = 7;
	public const int SOUND_CB_AREA_FIRE = 8;
	public const int SOUND_CB_LIGHTING = 9;
	public const int SOUND_CB_ENEMY_EXPLOSION = 10;
	public const int SOUND_CB_GENERADIER_SHOOT = 11;
	public const int SOUND_CB_SNIPER_SHOOT = 12;
	public const int SOUND_CB_CILVILIAN_SHOOT = 13;
	public const int SOUND_CB_COMMANDO_SHOOT = 14;
	public const int SOUND_CB_SAPPER_ATTACK = 15;
	public const int SOUND_CB_RELOAD = 16;
	public const int SOUND_CB_FIREFIGHTER_ATTACK = 17;
	public const int SOUND_CB_GUNNER_SHOOT = 18;
	public const int SOUND_CB_BOX_BREAK = 19;
	public const int SOUND_CB_BOSS_DEMON_SHOOT = 20;
	public const int SOUND_CB_BOSS_SUMMON_SHOOT = 21;
	public const int SOUND_CB_BOSS_NEMESIS_ROCKET = 22;
	public const int SOUND_CB_ENEMY_SQUEEL = 23;
	public const int SOUND_CB_ENEMY_BREATH = 24;
	public const int SOUND_CB_BOOM_EXPLOSION = 25;
	public const int SOUND_CB_STAR_SMALL = 26;
	public const int SOUND_UI_CRAFT = 27;
	public const int SOUND_UI_GET_REWARD_ITEM = 28;
	public const int SOUND_UI_OPEN_BOX = 29;
	public const int SOUND_UI_GET_COIN = 30;
	public const int SOUND_UI_EQUIP_VEHICLE = 31;
	public const int SOUND_UI_UNEQUIP_VEHICLE = 32;
	public const int SOUND_UI_EQUIP_ITEM_UNIT = 33;
	public const int SOUND_CB_PICKUP = 34;
	public const int SOUND_CB_FINAL_WAVE = 35;
	public const int SOUND_CB_GAMEOVER = 36;
	public const int SOUND_CB_COUNT_COIN = 37;
	public const int SOUND_CB_AIR_START = 38;
	public const int SOUND_CB_AIR_JOIN = 39;
	public const int SOUND_CB_USE_POWERUP = 40;
	public const int SOUND_UI_BUY_ITEM = 41;
	public const int SOUND_CB_DROP_OIL = 42;
	public const int SOUND_UI_STROKE = 43;
	public const int SOUND_CB_CLICK_BUY_UNIT = 44;
	public const int SOUND_UI_BUY_FAIL = 45;
	public const int SOUND_CB_WALK_IN_WATER = 46;
	public const int SOUND_CB_WALK_IN_OIL = 47;
	public const int SOUND_CB_BOSSDIED = 48;
	public const int SOUND_CB_STEVEDORE_ATTACK = 49;
	public const int SOUND_CB_ZOMBIE_SUMMONORREVIVE = 50;
	public const int SOUND_CB_FEMALE_DIE = 51;
	public const int SOUND_CB_MALE_DIE = 52;
	public const int SOUND_CB_SKILL_100RAGE = 53;
	public const int SOUND_UI_CLAIMACHIEVEMENT = 54;
	public const int SOUND_UI_VICTORY = 55;
	public const int SOUND_CB_UNIT_DOGSTUN = 56;
	public const int SOUND_CB_UNIT_LIGHTINGDOG = 57;
	public const int SOUND_CB_UNIT_WARRIOR_ATTACK = 58;
	public const int SOUND_UI_UNLOCK_UNITSKILL = 59;
	#endregion
	#region Musics
	public const int MUSIC_CB_MAP1 = 1;
	public const int MUSIC_MAINMENU = 2;
	public const int MUSIC_INTRO = 3;
	#endregion
	#region Hero
	public const int A1 = 1;
	public const int A2 = 2;
	public const int A3 = 3;
	public const int A4 = 4;
	public const int A5 = 5;
	public const int A6 = 6;
	public const int A7 = 7;
	public const int A8 = 8;
	public const int A9 = 9;
	public const int A10 = 10;
	#endregion
	#region Enemy
	public const int E1 = 1;
	public const int E2 = 2;
	public const int E3 = 3;
	public const int E4 = 4;
	public const int E5 = 5;
	#endregion
	#region PowerUps Items
	public const int PW_SPEED = 1;
	public const int PW_POWER = 2;
	public const int PW_QUICKEN = 3;
	public const int PW_REGEN = 4;
	public const int PW_PROTECT = 5;
	#endregion
	#region Reward Types [enum]
	public const int REWARD_TYPE_CURRENCY = 1;
	public const int REWARD_TYPE_UNLOCK_CHARACTER = 2;
	public const int REWARD_TYPE_POWER_UP = 3;
	public const int REWARD_TYPE_COIN_BY_MISSION = 4;
	public const int REWARD_TYPE_PRE_UNIT = 5;
	public enum Reward_Types { REWARD_TYPE_CURRENCY = 1, REWARD_TYPE_UNLOCK_CHARACTER = 2, REWARD_TYPE_POWER_UP = 3, REWARD_TYPE_COIN_BY_MISSION = 4, REWARD_TYPE_PRE_UNIT = 5,  }
	#endregion
	#region Currencies [enum]
	public const int CURRENCY_COIN = 1;
	public const int CURRENCY_CASH = 2;
	public enum Currencies { CURRENCY_COIN = 1, CURRENCY_CASH = 2,  }
	#endregion
	#region Areas
	public const int AREA_1 = 1;
	public const int AREA_2 = 2;
	public const int AREA_3 = 3;
	public const int AREA_4 = 4;
	#endregion
	#region Events
	public const int NONE = 0;
	public const int EVENT_NEW_YEAR = 1;
	public const int EVENT_VALENTINE = 2;
	public const int EVENT_EASTER = 3;
	public const int EVENT_HALLOWEEN = 4;
	public const int EVENT_CHRISTMAS = 5;
	#endregion
	#region Main attack type
	public const int MELEE = 1;
	public const int RANGE = 2;
	#endregion
	#region HeroRarity
	public const int HERO_RARITY_COMMON = 1;
	public const int HERO_RARITY_RARE = 2;
	public const int HERO_RARITY_EPIC = 3;
	public const int HERO_RARITY_LEGENDARY = 4;
	#endregion
	#region EnemyDifficult
	public const int ENEMY_DIFFICULT_COMMON = 1;
	public const int ENEMY_DIFFICULT_RARE = 2;
	public const int ENEMY_DIFFICULT_EPIC = 3;
	public const int ENEMY_DIFFICULT_LEGENDARY = 4;
	#endregion
	#region RandomFrom
	public const int RANDOM_FROM_WHEEL = -1;
	public const int RANDOM_FROM_NORMAL_CHEST = -2;
	public const int RANDOM_FROM_PREMIUM_CHEST = -3;
	#endregion

}
