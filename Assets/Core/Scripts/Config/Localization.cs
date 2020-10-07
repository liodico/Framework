using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Localization
{
	public enum ID 
	{
		NONE = -1,
		HP = 0, MELEE_ATK, LUCK, RANGE_ATK, SPEED, COURAGE, MELEE_ATK_SPEED, RANGE_ATK_SPEED, COOLDOWN, PREPARE, ATK_RANGE, CLIP_SIZE, UNLOCK_COST, RAGE_GAIN, CASH, GOLD, EXP, UNLOCK_RANDOM_UNIT, UNLOCK_UNIT, PRE_RANDOM_UNIT, PRE_UNIT, TEAM_EDIT, BATTLE_TEAM, COLLECTION, TROOP_DETAIL, ATTACK, HIRE, UPGRADE, USE_UNIT, DAILY_GIFTS, SPEED_UP, CLAIM, LAB, TRANSMUTE, CRAFT, SELECT, DESELECT, QUESTS, NOTIFICATION, LOAD, IGNORE, CLOUD_CHANGE_ACCOUNT, CLOUD_DATA_IS_READY, CLOUD_DATA_IS_NEWER, CLOUD_DATA_NEED_LOGIN, HERO_NAME_1, HERO_NAME_2, HERO_NAME_3, HERO_NAME_4, HERO_NAME_5, HERO_NAME_6, HERO_NAME_7, HERO_NAME_8, HERO_NAME_9, HERO_NAME_10, HERO_DESCRIPTION_1, HERO_DESCRIPTION_2, HERO_DESCRIPTION_3, HERO_DESCRIPTION_4, HERO_DESCRIPTION_5, HERO_DESCRIPTION_6, HERO_DESCRIPTION_7, HERO_DESCRIPTION_8, HERO_DESCRIPTION_9, HERO_DESCRIPTION_10, HERO_RARITY_1, HERO_RARITY_2, HERO_RARITY_3, HERO_RARITY_4, ENEMY_NAME_1, ENEMY_NAME_2, ENEMY_NAME_3, ENEMY_NAME_4, ENEMY_NAME_5, ENEMY_DESCRIPTION_1, ENEMY_DESCRIPTION_2, ENEMY_DESCRIPTION_3, ENEMY_DESCRIPTION_4, ENEMY_DESCRIPTION_5, ENEMY_RARITY_1, ENEMY_RARITY_2, ENEMY_RARITY_3, ENEMY_RARITY_4, POWER_UP_ITEM_DESCRIPTION_1, POWER_UP_ITEM_DESCRIPTION_2, POWER_UP_ITEM_DESCRIPTION_3, POWER_UP_ITEM_DESCRIPTION_4, POWER_UP_ITEM_DESCRIPTION_5, POWERUP_NAME_1, POWERUP_NAME_2, POWERUP_NAME_3, POWERUP_NAME_4, POWERUP_NAME_5,
	}
	public const int
		HP = 0, MELEE_ATK = 1, LUCK = 2, RANGE_ATK = 3, SPEED = 4, COURAGE = 5, MELEE_ATK_SPEED = 6, RANGE_ATK_SPEED = 7, COOLDOWN = 8, PREPARE = 9, ATK_RANGE = 10, CLIP_SIZE = 11, UNLOCK_COST = 12, RAGE_GAIN = 13, CASH = 14, GOLD = 15, EXP = 16, UNLOCK_RANDOM_UNIT = 17, UNLOCK_UNIT = 18, PRE_RANDOM_UNIT = 19, PRE_UNIT = 20, TEAM_EDIT = 21, BATTLE_TEAM = 22, COLLECTION = 23, TROOP_DETAIL = 24, ATTACK = 25, HIRE = 26, UPGRADE = 27, USE_UNIT = 28, DAILY_GIFTS = 29, SPEED_UP = 30, CLAIM = 31, LAB = 32, TRANSMUTE = 33, CRAFT = 34, SELECT = 35, DESELECT = 36, QUESTS = 37, NOTIFICATION = 38, LOAD = 39, IGNORE = 40, CLOUD_CHANGE_ACCOUNT = 41, CLOUD_DATA_IS_READY = 42, CLOUD_DATA_IS_NEWER = 43, CLOUD_DATA_NEED_LOGIN = 44, HERO_NAME_1 = 45, HERO_NAME_2 = 46, HERO_NAME_3 = 47, HERO_NAME_4 = 48, HERO_NAME_5 = 49, HERO_NAME_6 = 50, HERO_NAME_7 = 51, HERO_NAME_8 = 52, HERO_NAME_9 = 53, HERO_NAME_10 = 54, HERO_DESCRIPTION_1 = 55, HERO_DESCRIPTION_2 = 56, HERO_DESCRIPTION_3 = 57, HERO_DESCRIPTION_4 = 58, HERO_DESCRIPTION_5 = 59, HERO_DESCRIPTION_6 = 60, HERO_DESCRIPTION_7 = 61, HERO_DESCRIPTION_8 = 62, HERO_DESCRIPTION_9 = 63, HERO_DESCRIPTION_10 = 64, HERO_RARITY_1 = 65, HERO_RARITY_2 = 66, HERO_RARITY_3 = 67, HERO_RARITY_4 = 68, ENEMY_NAME_1 = 69, ENEMY_NAME_2 = 70, ENEMY_NAME_3 = 71, ENEMY_NAME_4 = 72, ENEMY_NAME_5 = 73, ENEMY_DESCRIPTION_1 = 74, ENEMY_DESCRIPTION_2 = 75, ENEMY_DESCRIPTION_3 = 76, ENEMY_DESCRIPTION_4 = 77, ENEMY_DESCRIPTION_5 = 78, ENEMY_RARITY_1 = 79, ENEMY_RARITY_2 = 80, ENEMY_RARITY_3 = 81, ENEMY_RARITY_4 = 82, POWER_UP_ITEM_DESCRIPTION_1 = 83, POWER_UP_ITEM_DESCRIPTION_2 = 84, POWER_UP_ITEM_DESCRIPTION_3 = 85, POWER_UP_ITEM_DESCRIPTION_4 = 86, POWER_UP_ITEM_DESCRIPTION_5 = 87, POWERUP_NAME_1 = 88, POWERUP_NAME_2 = 89, POWERUP_NAME_3 = 90, POWERUP_NAME_4 = 91, POWERUP_NAME_5 = 92;
	public static readonly string[] idString = new string[]
	{
		"HP", "MELEE_ATK", "LUCK", "RANGE_ATK", "SPEED", "COURAGE", "MELEE_ATK_SPEED", "RANGE_ATK_SPEED", "COOLDOWN", "PREPARE", "ATK_RANGE", "CLIP_SIZE", "UNLOCK_COST", "RAGE_GAIN", "CASH", "GOLD", "EXP", "UNLOCK_RANDOM_UNIT", "UNLOCK_UNIT", "PRE_RANDOM_UNIT", "PRE_UNIT", "TEAM_EDIT", "BATTLE_TEAM", "COLLECTION", "TROOP_DETAIL", "ATTACK", "HIRE", "UPGRADE", "USE_UNIT", "DAILY_GIFTS", "SPEED_UP", "CLAIM", "LAB", "TRANSMUTE", "CRAFT", "SELECT", "DESELECT", "QUESTS", "NOTIFICATION", "LOAD", "IGNORE", "CLOUD_CHANGE_ACCOUNT", "CLOUD_DATA_IS_READY", "CLOUD_DATA_IS_NEWER", "CLOUD_DATA_NEED_LOGIN", "HERO_NAME_1", "HERO_NAME_2", "HERO_NAME_3", "HERO_NAME_4", "HERO_NAME_5", "HERO_NAME_6", "HERO_NAME_7", "HERO_NAME_8", "HERO_NAME_9", "HERO_NAME_10", "HERO_DESCRIPTION_1", "HERO_DESCRIPTION_2", "HERO_DESCRIPTION_3", "HERO_DESCRIPTION_4", "HERO_DESCRIPTION_5", "HERO_DESCRIPTION_6", "HERO_DESCRIPTION_7", "HERO_DESCRIPTION_8", "HERO_DESCRIPTION_9", "HERO_DESCRIPTION_10", "HERO_RARITY_1", "HERO_RARITY_2", "HERO_RARITY_3", "HERO_RARITY_4", "ENEMY_NAME_1", "ENEMY_NAME_2", "ENEMY_NAME_3", "ENEMY_NAME_4", "ENEMY_NAME_5", "ENEMY_DESCRIPTION_1", "ENEMY_DESCRIPTION_2", "ENEMY_DESCRIPTION_3", "ENEMY_DESCRIPTION_4", "ENEMY_DESCRIPTION_5", "ENEMY_RARITY_1", "ENEMY_RARITY_2", "ENEMY_RARITY_3", "ENEMY_RARITY_4", "POWER_UP_ITEM_DESCRIPTION_1", "POWER_UP_ITEM_DESCRIPTION_2", "POWER_UP_ITEM_DESCRIPTION_3", "POWER_UP_ITEM_DESCRIPTION_4", "POWER_UP_ITEM_DESCRIPTION_5", "POWERUP_NAME_1", "POWERUP_NAME_2", "POWERUP_NAME_3", "POWERUP_NAME_4", "POWERUP_NAME_5",
	};
	public static readonly Dictionary<string, string> languageDict = new Dictionary<string, string>() {  { "english", "Localization_english" }, { "vietnamease", "Localization_vietnamease" }, };
	public static readonly string defaultLanguage = "english";

    private static string[] mTexts;
    private static string mFolder = "Data";
	private static string mLanguageTemp;
    public static string currentLanguage
    {
        get { return PlayerPrefs.GetString("currentLanguage", defaultLanguage); }
        set
        {
            if (currentLanguage != value)
            {
                PlayerPrefs.SetString("currentLanguage", value);
                Init();
            }
        }
    }

    public Localization()
    {
        Init();
    }

    public static void Init()
    {
        if (mLanguageTemp != currentLanguage)
        {
            string file = languageDict[currentLanguage];
            string json = Resources.Load<TextAsset>(mFolder + "/" + file).text;
            mTexts = GetJsonList(json);
            mLanguageTemp = currentLanguage;
        }
    }

    private static string[] GetTexts()
    {
        if (mTexts == null)
            Init();
        return mTexts;
    }

    public static string Get(ID pId)
    {
        return GetTexts()[(int)pId];
    }

    public static string Get(int pId)
    {
        if (pId >= 0 && pId < GetTexts().Length)
            return GetTexts()[pId];
        Debug.LogError("Not found id " + pId);
        return "";
    }

    public static string Get(string pIdString)
    {
        int index = 0;
        for (int i = 0; i < idString.Length; i++)
        {
            if (pIdString == idString[i])
            {
                index = i;
                return Get(index);
            }
        }
        Debug.LogError("Not found idString " + pIdString);
        return "";
    }

    public static string Get(string pIdString, ref int pIndex)
    {
        pIndex = -1;
        for (int i = 0; i < idString.Length; i++)
        {
            if (pIdString == idString[i])
            {
                pIndex = i;
                return Get(pIndex);
            }
        }
        Debug.LogError("Not found idString " + pIdString);
        return "";
    }

    public static string Get(string pIdString, ref ID pId)
    {
        int index = -1;
        for (int i = 0; i < idString.Length; i++)
        {
            if (pIdString == idString[i])
            {
                index = i;
                pId = (ID)index;
                return Get(pId);
            }
        }
        Debug.LogError("Not found idString " + pIdString);
        return "";
    }

    public static string[] GetJsonList(string json)
    {
        var sb = new StringBuilder();
        string newJson = sb.Append("{").Append("\"array\":").Append(json).Append("}").ToString();
        StringArray wrapper = JsonUtility.FromJson<StringArray>(newJson);
        return wrapper.array;
    }

    [System.Serializable]
    private class StringArray
    {
        public string[] array;
    }
}

public class LocalizationGetter
{
    private static Dictionary<string, int> cachedTexts = new Dictionary<string, int>();

    public string key;
    public string defaultStr;

    private int mIndex = -1;
    private bool mCheckKey;

    public LocalizationGetter(string pKey, string pDefault)
    {
        key = pKey;
        defaultStr = pDefault;

#if UNITY_EDITOR
        //In Editor we check it soon to find missing localization
        Localization.Get(key, ref mIndex);
        mCheckKey = true;
#endif
    }

    public string Get()
    {
        if (!mCheckKey)
        {
            Localization.Get(key, ref mIndex);
            mCheckKey = true;
        }

        if (mIndex == -1)
            return defaultStr;
        var text = Localization.Get(mIndex);
        if (string.IsNullOrEmpty(text))
            return defaultStr;
        else
            return Localization.Get(mIndex).Replace("\\n", "\u000a");
    }

    public static string GetCached(string pKey)
    {
        if (cachedTexts.ContainsKey(pKey))
        {
            int id = cachedTexts[pKey];
            if (id != -1)
            {
                string text = Localization.Get(cachedTexts[pKey]);
                return !string.IsNullOrEmpty(text) ? text : pKey;
            }
            return pKey;
        }
        else
        {
            int id = -1;
            string text = Localization.Get(pKey, ref id);
            cachedTexts.Add(pKey, id);
            return !string.IsNullOrEmpty(text) ? text : pKey;
        }
    }
}
