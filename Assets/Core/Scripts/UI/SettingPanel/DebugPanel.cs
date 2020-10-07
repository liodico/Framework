
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utilities.Common;
using Utilities.Components;
using Utilities.Pattern.UI;
using Utilities.Service.Ads;
using Utilities.Service.GPGS;
using Utilities.Service.RFirebase;
using Debug = Utilities.Common.Debug;

namespace FoodZombie.UI
{
    public class DebugPanel : MyGamesBasePanel
    {
        #region Members

        [SerializeField] private RectTransform mButtonContainer;
        [SerializeField] private RectTransform mToggleContainer;
        [SerializeField] private Toggle mToggle;
        [SerializeField] private SimpleTMPButton mButton;
        [SerializeField] private TMP_InputField mTxtIdHeroToUnlock;
        [SerializeField] private SimpleTMPButton mBtnUnlockHero;

        private SimpleTMPButton mBtnGetTime;

        #endregion 

        //=============================================

        #region MonoBehaviour

        private void Start()
        {
            mToggle.gameObject.SetActive(false);
            mButton.gameObject.SetActive(false);
#if DEVELOPMENT
            var gameData = GameData.Instance;
            mBtnUnlockHero.onClick.AddListener(OnBtnUnlockHero_Pressed);

            CreateButton("Save Game", Color.green, () =>
            {
                gameData.SaveGame();
            });
            //============================================================
            // Cheat currencies
            //============================================================
            CreateButton("Add 1000 Cash", Color.blue, () =>
            {
                gameData.CurrenciesGroup.Add(IDs.CURRENCY_CASH, 1000);
            });
            CreateButton("Add 1000 Gold", Color.blue, () =>
            {
                gameData.CurrenciesGroup.Add(IDs.CURRENCY_COIN, 1000);
            });
            CreateButton("Add 10000 Gold", Color.blue, () =>
            {
                gameData.CurrenciesGroup.Add(IDs.CURRENCY_COIN, 10000);
            });
            //============================================================
            // Cheat data
            //============================================================
            CreateButton("Give Random PowerUp Item", Color.cyan, () =>
            {
                var rewardInfo = new RewardInfo(IDs.REWARD_TYPE_POWER_UP, -1, 1);
                rewardInfo = LogicAPI.ClaimReward(rewardInfo);
                var data = gameData.ItemsGroup.GetPowerUpItem(rewardInfo.Id);
                Debug.Log(string.Format("Give PowerUp Item id {0}, name {1}", data.Id, data.GetName()));
            });
            CreateButton("Random Zombie View", Color.cyan, () =>
            {
                List<EnemyData> enemyDatas = GameData.Instance.EnemiesGroup.GetAllEnemyDatas();
                int r = UnityEngine.Random.Range(0, enemyDatas.Count);
                enemyDatas[r].View();
            });
            CreateButton("Unlock all heroes", Color.yellow, () =>
            {
                gameData.HeroesGroup.UnlockAllHeroes();
            });
            //CreateButton("Recruit all units", Color.cyan, () =>
            //{
            //    gameData.UnitsGroup.RecruitAllUnits();
            //});
            CreateButton("Level Up Vehicle", Color.cyan, () =>
            {
                gameData.VehicleData.LevelUp();
            });
            CreateButton("Test Crash Popup", Color.black, () =>
            {
                int[] testArray = new int[10];
                testArray[-1] = 10;
            });
            mBtnGetTime = CreateButton("Get Current Time", Color.cyan, () =>
            {
                var time = ServerManager.GetCurrentTime();
                if (time == null)
                    mBtnGetTime.labelTMP.text = "Counld not connect";
                else
                    mBtnGetTime.labelTMP.text = time.ToString();
            });
#if ACTIVE_FACEBOOK
            CreateButton("Login FB", Color.white, () =>
            {
                Facebook.Unity.FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email" }, null);
            });
#endif
            //CreateButton("Log Missing Events", Color.white, () =>
            //{
            //    StartCoroutine(IEPrepareFirebaseEvents());
            //});
            CreateButton("Signout GGPlay", Color.red, () =>
            {
                GameServices.SignOut();
            });
            //============================================================
            // Ads
            //============================================================
#if UNITY_ADS
            CreateButton("Show Unity Ads", Color.white, () => { AdsHelper.__ShowVideoRewardedAd(null, "", RewardedAdNetwork.UnityAds); });
#endif
#if ACTIVE_ADMOB
            CreateButton("Show Admob Ads", Color.white, () => { AdsHelper.__ShowVideoRewardedAd(null, "", RewardedAdNetwork.AdMob); });
#endif
            //============================================================
            // Toggle
            //============================================================
            CreateToggle("Enable Cheat", (isOn) =>
            {
                DevSetting.Instance.enableCheat = isOn;
            }, DevSetting.Instance.enableCheat);
            CreateToggle("Enable Cheat Time", (isOn) =>
            {
                DevSetting.Instance.enableCheatTime = isOn;
            }, DevSetting.Instance.enableCheatTime);
            CreateToggle("Show FPS", (isOn) =>
            {
                DevSetting.Instance.showFPS = isOn;
            }, DevSetting.Instance.showFPS);
            CreateToggle("Enable Log", (isOn) =>
            {
                DevSetting.Instance.enableLog = isOn;
            }, DevSetting.Instance.enableLog);
#endif
        }

        private void OnBtnUnlockHero_Pressed()
        {
            if (!mTxtIdHeroToUnlock.text.Equals(""))
            {
                var reward = new RewardInfo(IDs.REWARD_TYPE_UNLOCK_CHARACTER, int.Parse(mTxtIdHeroToUnlock.text), 1);
                LogicAPI.ClaimReward(reward);
            }
        }

        #endregion

        //=============================================

        #region Public

        #endregion

        //==============================================

        #region Private

        private string EventsSent
        {
            set { PlayerPrefs.SetString("Events_Sent", value); }
            get { return PlayerPrefs.GetString("Events_Sent"); }
        }

        //private IEnumerator IEPrepareFirebaseEvents()
        //{
        //    var shopItemsGroup = GameData.Instance.ShopItemsGroup;
        //    var goldPackDefinitions = shopItemsGroup.GoldPackDefinitions;
        //    for (int i = 0; i < goldPackDefinitions.Count; i++)
        //    {
        //        if (!EventsSent.Contains(goldPackDefinitions[i].sku))
        //        {
        //            RFirebaseManager.LogEvent(TrackingConstants.EVENT_BUY_IAP, goldPackDefinitions[i].sku.Replace(".", "_"), 0);
        //            Utilities.Services.FBServices.LogEvent(TrackingConstants.EVENT_BUY_IAP, goldPackDefinitions[i].sku.Replace(".", "_"), 0);
        //            EventsSent = $"{EventsSent},{goldPackDefinitions[i].sku}";
        //            yield return null;
        //        }
        //    }

        //    var specialPacks = shopItemsGroup.GetSpecialPacks();
        //    for (int i = 0; i < specialPacks.Count; i++)
        //    {
        //        if (!EventsSent.Contains(specialPacks[i].baseData.sku))
        //        {
        //            RFirebaseManager.LogEvent(TrackingConstants.EVENT_BUY_IAP, specialPacks[i].baseData.sku.Replace(".", "_"), 0);
        //            Utilities.Services.FBServices.LogEvent(TrackingConstants.EVENT_BUY_IAP, specialPacks[i].baseData.sku.Replace(".", "_"), 0);
        //            EventsSent = $"{EventsSent},{specialPacks[i].baseData.sku}";
        //            yield return null;
        //        }
        //    }

        //    var lootBoxes = shopItemsGroup.LootBoxPackDefinitions;
        //    for (int i = 0; i < lootBoxes.Count; i++)
        //    {
        //        if (!EventsSent.Contains(lootBoxes[i].sku))
        //        {
        //            RFirebaseManager.LogEvent(TrackingConstants.EVENT_BUY_IAP, lootBoxes[i].sku.Replace(".", "_"), 0);
        //            Utilities.Services.FBServices.LogEvent(TrackingConstants.EVENT_BUY_IAP, lootBoxes[i].sku.Replace(".", "_"), 0);
        //            EventsSent = $"{EventsSent},{lootBoxes[i].sku}";
        //            yield return null;
        //        }
        //    }
        //}

        private SimpleTMPButton CreateButton(string name, Color pButtonColor, UnityAction pDoSomething)
        {
            SimpleTMPButton btn = Instantiate(mButton, mButtonContainer);
            btn.gameObject.SetActive(true);
            btn.onClick.AddListener(pDoSomething);
            btn.name = name;
            btn.image.color = pButtonColor;
            btn.labelTMP.text = name;
            btn.labelTMP.color = pButtonColor.Invert();
            return btn;
        }

        private Toggle CreateToggle(string name, UnityAction<bool> pDoSomething, bool pDefault = false)
        {
            Toggle tgl = Instantiate(mToggle, mToggleContainer);
            tgl.gameObject.SetActive(true);
            tgl.onValueChanged.AddListener(pDoSomething);
            tgl.name = name;
            tgl.GetComponentInChildren<TextMeshProUGUI>().text = name;
            tgl.isOn = pDefault;
            return tgl;
        }

        #endregion

    }
}
