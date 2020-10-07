using UnityEngine;
using Utilities.Service.RFirebase;
using Utilities.Services;
using System;

public class Config
{
    public static int MIN_LEVEL_INTERSTITIAL = 5;
    public static double TIME_INTERSTITIAL_OPENING = 90;
    public static double TIME_INTERSTITIAL_AFTER_RV = 90;
    public static double TIME_INTERSTITIAL_AFTER_INTERSTITIAL = 90;

    public static int typeMenuInGameMode = 0;
    public static byte isJustWin = 0;
    //public static BlittableBool isJustWin = false;
    public const int TYPE_CONDITION_CHECK_GET_PLAN = 1;
    public const int TYPE_CONDITION_CHECK_GET_NEXT_ACTION = 2;
    public const int TYPE_CONDITION_CHECK_END_ACTION = 3;

    public const int TYPE_CONDITION_CHECK_END_PLAN = 4;
    public const int TYPE_CONDITION_CHECK_FAST = 5;

    public static int typeModeInGame = 0;

    public const int TYPE_MODE_NORMAL = 0;
    public const int TYPE_MODE_BOSS = 1;
    //public const int TYPE_MODE_HARD = 2;
    public const int TYPE_MODE_BONUS = 3;
    //public static BlittableBool isHardMode = false;
    public static byte isHardMode = 0;

    //------------Gameplay----------------------
    public const string TAG_HERO_BULLET = "HeroBullet";
    public const string TAG_HERO_ATTACK = "HeroAttack";
    public const string TAG_ENEMY_BULLET = "EnemyBullet";
    public const string TAG_ENEMY_ATTACK = "EnemyAttack";
    public const string TAG_HERO = "Hero";
    public const string TAG_ENEMY = "Enemy";
    public const string TAG_FISH = "Fish";
    public const string TAG_COIN = "Coin";
    public const string TAG_NADE = "Nade";
    public const string TAG_EXPLOSION = "Explosion";

    public const int NADE_DAMAGE = 10;

    public const float xMaxMap = 2.72f;

    //
    // get market link of app
    public static string GetMarketLink()
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
        return "market://details?id=" + Application.identifier;
        #elif UNITY_IOS && !UNITY_EDITOR
        return "itms-apps://itunes.apple.com/app/id" + Application.identifier;
        #else
        return "https://www.google.com.vn/search?q=" + Application.identifier;
        #endif
    }
        
    public static int GetVersionCode()
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass contextCls = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject context = contextCls.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageMngr = context.Call<AndroidJavaObject>("getPackageManager");
        string packageName = context.Call<string>("getPackageName");
        AndroidJavaObject packageInfo = packageMngr.Call<AndroidJavaObject>("getPackageInfo", packageName, 0);
        return packageInfo.Get<int>("versionCode");
        #elif UNITY_IOS && !UNITY_EDITOR
        return 0;
        #else
        return 0;
        #endif
    }
    
    public static DateTime UnixTimeStampToDateTime( double unixTimeStamp )
    {
        // Unix timestamp is seconds past epoch
        DateTime dtDateTime = new DateTime(1970,1,1,0,0,0,0,System.DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddSeconds( unixTimeStamp );
        return dtDateTime;
    }
    
    //------Tracking------

    public static bool LogEvent(string pEventName, bool needLogin = false)
    {
        RFirebaseManager.LogEvent(pEventName);
        //FBServices.LogEvent(pEventName);

        //AppsFlyer.trackRichEvent(pEventName, null);

        return false;
    }

    public static bool LogEvent(string pEventName, string pParamName, string pParamValue, bool needLogin = false)
    {
        RFirebaseManager.LogEvent(pEventName, pParamName, pParamValue, needLogin);
        //FBServices.LogEvent(pEventName, pParamName, pParamValue);

        //Dictionary<string, string> paramEvent = new Dictionary<string, string>();
        //paramEvent.Add(pParamName, pParamValue);
        //AppsFlyer.trackRichEvent(pEventName, paramEvent);

        return false;
    }

    public static bool LogEvent(string pEventName, string pParamName, long pParamValue, bool needLogin = false)
    {
        RFirebaseManager.LogEvent(pEventName, pParamName, pParamValue, needLogin);
        //FBServices.LogEvent(pEventName, pParamName, pParamValue);

        //Dictionary<string, string> paramEvent = new Dictionary<string, string>();
        //paramEvent.Add(pParamName, pParamValue + "");
        //AppsFlyer.trackRichEvent(pEventName, paramEvent);

        return false;
    }

    public static bool LogEvent(string pEventName, string pParamName, int pParamValue, bool needLogin = false)
    {
        RFirebaseManager.LogEvent(pEventName, pParamName, pParamValue, needLogin);
        //FBServices.LogEvent(pEventName, pParamName, pParamValue);

        //Dictionary<string, string> paramEvent = new Dictionary<string, string>();
        //paramEvent.Add(pParamName, pParamValue + "");
        //AppsFlyer.trackRichEvent(pEventName, paramEvent);

        return false;
    }

    public static bool LogEvent(string pEventName, string pParamName, float pParamValue, bool needLogin = false)
    {
        RFirebaseManager.LogEvent(pEventName, pParamName, pParamValue, needLogin);
        //FBServices.LogEvent(pEventName, pParamName, pParamValue);

        //Dictionary<string, string> paramEvent = new Dictionary<string, string>();
        //paramEvent.Add(pParamName, pParamValue + "");
        //AppsFlyer.trackRichEvent(pEventName, paramEvent);

        return false;
    }

    public static bool LogEvent(string pEventName, string[] pParamNames, int[] pParamValues, bool needLogin = false)
    {
        RFirebaseManager.LogEvent(pEventName, pParamNames, pParamValues, needLogin);
        //FBServices.LogEvent(pEventName, pParamNames, pParamValues);

        //Dictionary<string, string> paramEvent = new Dictionary<string, string>();
        //for (int i = 0; i < pParamNames.Length; i++)
        //{
        //    paramEvent.Add(pParamNames[i], pParamValues[i] + "");
        //}
        //AppsFlyer.trackRichEvent(pEventName, paramEvent);

        return false;
    }

    //public static bool LogEvent(string pEventName, string[] pParamNames, float[] pParamValues, bool needLogin = false)
    //{
    //    RFirebaseManager.LogEvent(pEventName, pParamNames, pParamValues, needLogin);
    //    FBServices.LogEvent(pEventName, pParamNames, pParamValues);
    //    return false;
    //}

    public static bool LogEvent(string pEventName, string[] pParamNames, string[] pParamValues, bool needLogin = false)
    {
        RFirebaseManager.LogEvent(pEventName, pParamNames, pParamValues, needLogin);
        //FBServices.LogEvent(pEventName, pParamNames, pParamValues);

        //Dictionary<string, string> paramEvent = new Dictionary<string, string>();
        //for (int i = 0; i < pParamNames.Length; i++)
        //{
        //    paramEvent.Add(pParamNames[i], pParamValues[i]);
        //}
        //AppsFlyer.trackRichEvent(pEventName, paramEvent);

        return false;
    }

    private static System.Random mRandom = new System.Random();
    public static int EasyRandom(int range)
    {
        return mRandom.Next(range);
    }

    public static int EasyRandom(int min, int max)
    {
        return mRandom.Next(min, max);
    }

    public static float EasyRandom(float min, float max)
    {
        return UnityEngine.Random.RandomRange(min, max);
    }

    //code trên mạng
    public static float GetZangleFromTwoPosition(Vector3 fromPos, Vector3 toPos)
    {
        var xDistance = toPos.x - fromPos.x;
        var yDistance = toPos.y - fromPos.y;
        var angle = Mathf.Atan2(xDistance, yDistance) * Mathf.Rad2Deg;
        angle = -Get360Angle(angle);

        return angle + 90;
    }

    public static float Get360Angle(float angle)
    {
        while (angle < 0f)
        {
            angle += 360f;
        }
        while (360f < angle)
        {
            angle -= 360f;
        }
        return angle;
    }
}

public enum DIRECTION
{
    Left = -1,
    Right = 1,
    Up = 2,
    Down = -2,
};
