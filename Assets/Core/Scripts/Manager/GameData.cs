using System;
//using MobileLocalNotifications;
/**
 *  Created by hnim.
 *  Copyright (c) 2017  All rights reserved.
 */
using UnityEngine;
using Utilities.AntiCheat;
using Utilities.Pattern.Data;

namespace FoodZombie
{
    public class GameData : DataManager
    {
        #region Members

        private const bool encrypted = false;

        private static GameData mInstance;
        public static GameData Instance => mInstance;

        //public static GameEncryption mEncryption;
        public static Encryption encryption;

        //private string mRootKey;
        private DataSaver mDataSaver;

        private HeroesGroup mHeroesGroup;
        private EnemiesGroup mEnemiesGroup;
        private VehicleData mVehicleData;
        private WheelData mWheelData;
        private FoodGuyData mFoodGuyData;
        private NormalChestData mNormalChestData;
        private PremiumChestData mPremiumChestData;
        private MissionsGroup mMissionsGroup;
        private CurrenciesGroup mCurrenciesGroup;
        private GameConfigGroup mGameConfigGroup;
        private TutorialsGroup mTutorialsGroup;
        private ItemsGroup mItemsGroup;
        //private FloatData mPlayingTime;
        //private TimeCounter mTimeCounter;
        //private BoolData mWaitingForBackup;
        private bool mWaitingForBackup;
        private SafeData mSafeData;

        public HeroesGroup HeroesGroup => mHeroesGroup;
        public EnemiesGroup EnemiesGroup => mEnemiesGroup;
        public VehicleData VehicleData => mVehicleData;
        public WheelData WheelData => mWheelData;
        public FoodGuyData FoodGuyData => mFoodGuyData;
        public NormalChestData NormalChestData => mNormalChestData;
        public PremiumChestData PremiumChestData => mPremiumChestData;
        public MissionsGroup MissionsGroup => mMissionsGroup;
        public CurrenciesGroup CurrenciesGroup => mCurrenciesGroup;
        public GameConfigGroup GameConfigGroup => mGameConfigGroup;
        public TutorialsGroup TutorialsGroup => mTutorialsGroup;
        public ItemsGroup ItemsGroup => mItemsGroup;
        public SafeData SafeData => mSafeData;
        public bool WaitingForAutoBackup => mWaitingForBackup;

        #endregion

        //===================================

        #region Public

        private void Awake()
        {
            if (mInstance == null)
            {
                mInstance = this;
                Init();
            }
            else if (mInstance != this)
                Destroy(gameObject);
        }

        //public GameData() : base()
        //{
        //    //mRootKey = pRootKey;
        //    //mDataSaver = pDataSaver;
        //}

        public override void Init()
        {
            encryption = new Encryption();

            //mEncryption = GameEncryption.create(new byte[] { 168, 220, 184, 133, 78, 149, 8, 249, 171, 138, 98, 170, 95, 15, 211, 200, 51, 242, 4, 193, 219, 181, 232, 99, 16, 240, 142, 128, 29, 163, 245, 24, 204, 73, 173, 32, 214, 76, 31, 99, 91, 239, 232, 53, 138, 195, 93, 195, 185, 210, 155, 184, 243, 216, 204, 42, 138, 101, 100, 241, 46, 145, 198, 66, 11, 17, 19, 86, 157, 27, 132, 201, 246, 112, 121, 7, 195, 148, 143, 125, 158, 29, 184, 67, 187, 100, 31, 129, 64, 130, 26, 67, 240, 128, 233, 129, 63, 169, 5, 211, 248, 200, 199, 96, 54, 128, 111, 147, 100, 6, 185, 0, 188, 143, 25, 103, 211, 18, 17, 249, 106, 54, 162, 188, 25, 34, 147, 3, 222, 61, 218, 49, 164, 165, 133, 12, 65, 92, 48, 40, 129, 76, 194, 229, 109, 76, 150, 203, 251, 62, 54, 251, 70, 224, 162, 167, 183, 78, 103, 28, 67, 183, 23, 80, 156, 97, 83, 164, 24, 183, 81, 56, 103, 77, 112, 248, 4, 168, 5, 72, 109, 18, 75, 219, 99, 181, 160, 76, 65, 16, 41, 175, 87, 195, 181, 19, 165, 172, 138, 172, 84, 40, 167, 97, 214, 90, 26, 124, 0, 166, 217, 97, 246, 117, 237, 99, 46, 15, 141, 69, 4, 245, 98, 73, 3, 8, 161, 98, 79, 161, 127, 19, 55, 158, 139, 247, 39, 59, 72, 161, 82, 158, 25, 65, 107, 173, 5, 255, 53, 28, 179, 182, 65, 162, 17 });
            //var dataEnc = GameEncryption.create(new byte[] { 243, 111, 81, 108, 182, 228, 115, 27, 77, 218, 111, 251, 137, 250, 196, 26, 238, 109, 210, 212, 30, 6, 248, 153, 60, 174, 202, 182, 203, 91, 120, 139, 9, 140, 217, 135, 3, 92, 236, 163, 146, 220, 78, 113, 190, 19, 45, 99, 189, 33, 127, 106, 140, 53, 190, 175, 206, 120, 22, 218, 117, 49, 36, 117, 223, 197, 48, 55, 62, 39, 70, 168, 19, 114, 186, 184, 104, 204, 163, 33, 63, 58, 191, 217, 174, 104, 87, 240, 212, 140, 94, 222, 244, 172, 104, 88, 5, 254, 164, 28, 109, 226, 241, 106, 189, 177, 20, 86, 194, 18, 54, 152, 234, 251, 164, 218, 253, 19, 248, 88, 11, 8, 164, 247, 124, 232, 150, 155, 239, 164, 175, 247, 104, 133, 16, 47, 122, 250, 169, 220, 199, 188, 207, 166, 111, 72, 21, 112, 20, 198, 135, 151, 0, 201, 125, 150, 244, 149, 99, 187, 48, 6, 106, 144, 86, 207, 249, 33, 90, 61, 25, 164, 15, 45, 140, 51, 70, 188, 215, 182, 168, 164, 26, 33, 39, 126, 204, 103, 224, 148, 194, 125, 89, 223, 230, 242, 111, 38, 204, 24, 75, 223, 177, 21, 148, 128, 13, 186, 118, 168, 98, 210, 61, 174, 220, 102, 8, 143, 6, 104, 77, 78, 225, 233, 17, 175, 170, 62, 38, 151, 83, 29, 62, 190, 89, 15, 48, 69, 170, 236, 236, 179, 172, 93, 229, 66, 20, 133, 251, 230, 144, 127, 9, 161, 117 });
            mDataSaver = DataSaverContainer.CreateSaver("foodZombie", encrypted ? encryption : null);

            //Load sub groups here
            mHeroesGroup = AddMainDataGroup(new HeroesGroup(0), mDataSaver);
            mEnemiesGroup = AddMainDataGroup(new EnemiesGroup(1), mDataSaver);
            mVehicleData = AddMainDataGroup(new VehicleData(2), mDataSaver);
            mFoodGuyData = AddMainDataGroup(new FoodGuyData(3), mDataSaver);
            mMissionsGroup = AddMainDataGroup(new MissionsGroup(4), mDataSaver);
            mCurrenciesGroup = AddMainDataGroup(new CurrenciesGroup(5), mDataSaver);
            mGameConfigGroup = AddMainDataGroup(new GameConfigGroup(6), mDataSaver);
            mTutorialsGroup = AddMainDataGroup(new TutorialsGroup(7), mDataSaver);
            mItemsGroup = AddMainDataGroup(new ItemsGroup(8), mDataSaver);
            mWheelData = AddMainDataGroup(new WheelData(9), mDataSaver);
            mNormalChestData = AddMainDataGroup(new NormalChestData(10), mDataSaver);
            mPremiumChestData = AddMainDataGroup(new PremiumChestData(11), mDataSaver);
            mSafeData = AddMainDataGroup(new SafeData(12), mDataSaver);
            //mPlayingTime = AddMainDataGroup(new FloatData(8), mDataSaver);
            //mWaitingForBackup = AddMainDataGroup(new BoolData(9), mDataSaver);

            //Config save-load
            //setKey(mRootKey);
            //setDataSaver(mDataSaver);
            //mDataSaver.setDataEncryption(dataEnc);
            //setCommitEnable(false);

            //Load Data
            //load();
            //PostLoad();
            base.Init();

            //Utilities
            //mTimeCounter = new TimeCounter();
            //RGame.Instance.registerUpdateHandler(mTimeCounter);
            SaveGame();
        }

        public void SaveGame()
        {
            //setCommitEnable(true);
            Save();
            //setCommitEnable(false);
        }

        //public void RunLocalNotifications()
        //{
        ////FUEL
        //if (CurrenciesGroup.GetFuel() < Constants.FUEL_MAX)
        //{
        //    LocalNotificationsManager.InitNotifications();
        //    LocalNotificationsManager.SendNotification(1,
        //                                                (Constants.FUEL_MAX - CurrenciesGroup.GetFuel() - 1) * Constants.FUEL_COOLDOWN + CurrenciesGroup.FuelCountdownSeconds,
        //                                                "Gas tank is full " + EmojiList.VictoryHand,
        //                                                "Soldier, let's get back to work" + EmojiList.MilitaryMedal + EmojiList.MilitaryMedal + EmojiList.MilitaryMedal,
        //                                                "Soldier, let's get back to work", "icon_fuel", "", true, true, false, default(Color32), "");
        //}
        //}

        //public static GameData CreateInstance()
        //{
        //    if (mInstance == null)
        //    {
        //        //var saver = OneKeyDataSaver.create("gD", PrefFileData.create());
        //        mInstance = new GameData();
        //    }
        //    return mInstance;
        //}

        public void ImportData(string pGameData)
        {
            if (!string.IsNullOrEmpty(pGameData))
                DataSaverContainer.RestoreData(pGameData);
            //var saver = getDataSaver() as OneKeyDataSaver;
            //resetData();
            //saver.saveTextContent(pGameData);
            //saver.loadTextContent();
            //load();
            //PostLoad();
        }
        public static string GetTextContent(string path)
        {
            return DataManager.LoadFile(path, encrypted ? encryption : null);
        }
        //public static string GetTextContent(string pFilePath, bool pEncrypted = encrypted)
        //{
        //    var textAsset = Resources.Load<TextAsset>(pFilePath);
        //    if (textAsset != null)
        //    {
        //        string textContent = textAsset.text;
        //        Resources.UnloadAsset(textAsset);
        //        if (pEncrypted)
        //            return mEncryption.decryptValue(textContent);
        //        else
        //            return textContent;
        //    }
        //    else
        //    {
        //        Debug.LogWarning("Not Found file " + pFilePath);
        //        return "";
        //    }
        //}

        public void OnApplicationFocus(bool pFocus)
        {
            if (!pFocus)
            {
                //GetPlayingTime();
                SaveGame();
            }
        }

        //public override void OnApplicationPause(bool pPaused)
        //{
        //    base.OnApplicationPause(pPaused);
        //    if (pPaused)
        //    {
        //        //GetPlayingTime();
        //        SaveGame();
        //        RunLocalNotifications();
        //    }
        //}

        //public void OnApplicationQuit()
        //{
        //    //GetPlayingTime();
        //    SaveGame();
        //    RunLocalNotifications();
        //}

        //public float GetPlayingTime()
        //{
        //    mPlayingTime.Value+=(mTimeCounter.elapsedTime);
        //    mTimeCounter.Reset();
        //    return mPlayingTime.Value;
        //}

        public void WaitForAutoBackup(bool pValue)
        {
            //mWaitingForBackup.Value=(pValue);
            mWaitingForBackup = (pValue);
        }

        #endregion

        //===================================

        #region Private

        public StorageGameData BuildStorageData()
        {
            //var saver = mDataSaver as OneKeyDataSaver;
            var storedData = new StorageGameData();
            //storedData.gameData = saver.getFullDataContent();
            //storedData.userId = GameConfigGroup.StorageUserId;
            //storedData.userName = GameConfigGroup.StorageUserName;
            //storedData.updatedDate = ServerManager.GetCurrentTime() == null
            //    ? DateTime.UtcNow.AddHours(7).ToString()
            //    : ServerManager.GetCurrentTime().Value.ToString();
            //storedData.comparisonData = BuildComparisonData();
            return storedData;
        }

        private ComparisonData BuildComparisonData()
        {
            return new ComparisonData()
            {
                missionId = (GameData.Instance.MissionsGroup.CountMissionWin + 1),
                vehicleLevel = VehicleData.Level,
                gold = CurrenciesGroup.GetGold(),
            };
        }

        #endregion

        //====================================
    }

    [System.Serializable]
    public class StorageGameData
    {
        public string gameData;
        public string userId = "Unknown";
        public string userName = "Unknown";
        public string updatedDate;
        public ComparisonData comparisonData = new ComparisonData();

        public bool IsNewerThan(StorageGameData pOther)
        {
            if (pOther == null)
                return true;

            if (comparisonData.missionId > pOther.comparisonData.missionId)
                return true;
            else if (comparisonData.missionId == pOther.comparisonData.missionId)
            {
                if (comparisonData.vehicleLevel > pOther.comparisonData.vehicleLevel)
                    return true;
            }

            return false;
        }
    }

    /// <summary>
    /// Data to compare
    /// </summary>
    [System.Serializable]
    public class ComparisonData
    {
        public int missionId;
        public int vehicleLevel;
        public int gold;
    }
}