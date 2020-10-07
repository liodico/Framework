
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;
using Utilities.Common;
using Utilities.Components;
using Utilities.Inspector;
using Utilities.Service.RFirebase;

using DG.Tweening;
using Spine.Unity;
using Utilities.Service;

namespace FoodZombie.UI
{
    public class MainMenuPanel : MyGamesBasePanel
    {
        [Separator("Center")]
        public List<MainMenuButtonCharacter> btnCharacters;
        public SimpleTMPButton btnEditTeamLock;
        public GameObject txtEditTeamLabel;
        public GameObject[] listBGs;
        public GameObject lockTutorial;

        [Separator("Top-Center")]
        public GameObject missionBanner;
        public TextMeshProUGUI txtArea1;
        public TextMeshProUGUI txtArea2;
        [SerializeField, Tooltip("Buildin Pool")] public List<Image> iconMissionsPool;
        public Transform iconMissionGrid;
        public Sprite iconMissionOn;
        public Sprite iconMissionOff;
        public Sprite iconMissionBossOn;
        public Sprite iconMissionBossOff;

        public List<LayoutElement> listMission;
        public List<RectTransform> listBoss;
        public List<RectTransform> listBrain;
        public Transform progressTransform;
        public GameObject[] imgAreas;

        [Separator("Top-Left")]
        public SimpleTMPButton btnSetting;
        public SimpleTMPButton btnShop;
        public Image imgShopBuzz;

        [Separator("Top-Right")]
        public CurrencyView goldView;
        public CurrencyView cashView;
        public SimpleTMPButton btnCollection;
        public Image imgCollectionBuzz;
        public SimpleTMPButton btnWheel;
        public Image imgWheelBuzz;
        public TextMeshProUGUI txtWheel;
        public SimpleTMPButton btnSafe;
        public SimpleTMPButton btnBoss;
        public TextMeshProUGUI txtBoss;
        public Image imgSafeBuzz;

        [Separator("Bot-Left")]
        public SimpleTMPButton btnEditTeam;
        public Image imgEditTeamBuzz;

        //[Separator("Bot-Center")]

        [Separator("Bot-Right")]
        public SimpleTMPButton btnFight;
        public TextMeshProUGUI txtFight;
        public GameObject imgBoss;
        public GameObject imgBrain;
        public GameObject imgBossE;

        private bool initialized;

        private GameData GameData => GameData.Instance;

        private HeroData mHero = null;

        public static int showCount = 0;
        public static bool canShowChest = false;

        private void Start()
        {
            ShowInfoMap();
            var area = GameData.MissionsGroup.GetCurrentArea();
            int areaId = area.id;
            if(areaId > listBGs.Length)
            {
                areaId = listBGs.Length;
            }
            listBGs[areaId - 1].SetActive(true);

            btnSetting.onClick.AddListener(BtnSetting_Pressed);
            btnShop.onClick.AddListener(BtnShop_Pressed);
            btnCollection.onClick.AddListener(BtnCollection_Pressed);
            btnEditTeam.onClick.AddListener(BtnEditTeam_Pressed);
            //btnEditTeamLock.onClick.AddListener(BtnEditTeamLock_Pressed);
            btnWheel.onClick.AddListener(BtnSpinningWheel_Pressed);
            btnFight.onClick.AddListener(BtnFight_Pressed);
            btnSafe.onClick.AddListener(BtnSafe_Pressed);
            btnSafe.SetActive(LogicAPI.CanShowSafe1Panel());
            btnBoss.onClick.AddListener(BtnBoss_Pressed);
            btnBoss.SetActive(LogicAPI.CanShowBossPanel());
            var currentBossMissions = GameData.Instance.MissionsGroup.CurrentBossMissions;
            if (currentBossMissions.Count > 0)
            {
                txtBoss.text = "Fight now!";
            }
            else
            {
                txtBoss.text = "Unlock at Mission " + GameData.Instance.MissionsGroup.GetNextMissionHasBoss();
            }
            btnWheel.SetActive(LogicAPI.CanShowWheelPanel());

            var heroDatas = GameData.HeroesGroup.GetEquippedHeroes();
            for (int i = 0; i < heroDatas.Count; i++)
            {
                var heroData = heroDatas[i] as HeroData;
                btnCharacters[i].Init(heroData, ShowHero);
                btnCharacters[i].SetActive(true);
            }

            var countUnlocked = 0;
            heroDatas = GameData.HeroesGroup.GetAllHeroDatas();
            foreach (var item in heroDatas)
            {
                if (item.Unlocked)
                {
                    countUnlocked++;
                }
            }
            btnEditTeam.SetActive(countUnlocked > btnCharacters.Count);

            MainPanel.instance.onAnyChildHide += OnMainPanelChildHide;
            MainPanel.instance.onAnyChildShow += OnMainPanelChildShow;
            //EventDispatcher.AddListener<FeatureUnlockedEvent>(OnFeatureUnlocked);
            EventDispatcher.AddListener<HeroUnlockedEvent>(OnHeroUnlocked);
            EventDispatcher.AddListener<HeroEquipEvent>(OnHeroEquip);
            EventDispatcher.AddListener<SafeChangeValueEvent>(OnSafeChangeValue);
            EventDispatcher.AddListener<GamePaymentInitializedEvent>(OnGamePaymentInitialized);

            if(showCount == 0)
            {
                canShowChest = LogicAPI.CanShowNormalChestOnMainPanelAfterWinMission();
            }

            //show Claim Hero Panel
            if (GameData.HeroesGroup.ClaimHeroId != -1)
            {
                MainPanel.instance.ShowRescueHeroPanel(GameData.HeroesGroup.ClaimHeroId);
                LogicAPI.countLockChuaXuatHien++;
            }
            else if (canShowChest)
            {
                MainPanel.instance.ShowOpenChestPanel();
            }
            else if (LogicAPI.CanShowRatePanel() && showCount > 0)
            {
                MainPanel.instance.ShowRatePanelIfAvailable();
            }

            SoundManager.Instance.PlayMusic(IDs.MUSIC_MAINMENU);

            //if(showCount > 0)
            //{
            //    if (AdsHelper.Instance.CanShowInterstitial())
            //    {
            //        AdsHelper.__ShowInterstitialAd();
            //    }
            //}

            btnShop.SetActive(GamePayment.Instance.Initialized);

            showCount++;
        }

        private void OnDestroy()
        {
            MainPanel.instance.onAnyChildHide -= OnMainPanelChildHide;
            MainPanel.instance.onAnyChildShow -= OnMainPanelChildShow;
            //EventDispatcher.RemoveListener<FeatureUnlockedEvent>(OnFeatureUnlocked);
            EventDispatcher.RemoveListener<HeroUnlockedEvent>(OnHeroUnlocked);
            EventDispatcher.RemoveListener<HeroEquipEvent>(OnHeroEquip);
            EventDispatcher.RemoveListener<SafeChangeValueEvent>(OnSafeChangeValue);
            EventDispatcher.RemoveListener<GamePaymentInitializedEvent>(OnGamePaymentInitialized);
        }

        private void OnEnable()
        {
            //StartCoroutine(IEUpdate());
            ShowCountWheel(1);
        }

        internal override void Init()
        {
            goldView.Init(IDs.CURRENCY_COIN);
            cashView.Init(IDs.CURRENCY_CASH);

            imgCollectionBuzz.SetActive(false);
            imgShopBuzz.SetActive(false);
            imgEditTeamBuzz.SetActive(false);

            txtFight.text = "Mission " + (GameData.MissionsGroup.CountMissionWin + 1) + "";
            CheckNewZombieBuzz();

            if (GameData.Instance.SafeData.CanClaimSafe1
                || GameData.Instance.SafeData.CanClaimSafe2
                || GameData.Instance.SafeData.CanClaimSafe3
                || (GameData.Instance.MissionsGroup.CountMissionWin + 1) == 7)
                imgSafeBuzz.SetActive(true);

            initialized = true;
        }

        public void ShowCountWheel(int w = 0)
        {
            StartCoroutine(CountWheel(w));
        }

        IEnumerator CountWheel(int w)
        {
            if (w > 0) yield return new WaitForSeconds(w);
            var wait = new WaitForSeconds(1);
            while (!GameData.WheelData.FreeSpin)
            {
                txtWheel.gameObject.SetActive(true);
                imgWheelBuzz.gameObject.SetActive(false);
                txtWheel.text = GameData.WheelData.GetTimeFree();
                //btnSafe.rectTransform().anchoredPosition = new Vector2(-92f, -665f);
                yield return wait;
            }
            txtWheel.gameObject.SetActive(false);
            imgWheelBuzz.gameObject.SetActive(true);
            //btnSafe.rectTransform().anchoredPosition = new Vector2(-92f, -614f);
        }

        protected override void AfterShowing()
        {
            base.AfterShowing();

            //Showing shop notification

            //Showing character notification
            if (btnEditTeam.Enabled())
            {
                CheckNewHeroBuzz();
            }
        }

        public void CheckNewHeroBuzz()
        {
            imgEditTeamBuzz.SetActive(false);
            var heroes = GameData.HeroesGroup.GetAllHeroDatas();
            for (int i = 0; i < heroes.Count; i++)
            {
                if (heroes[i].Unlocked && !heroes[i].Viewed)
                {
                    imgEditTeamBuzz.SetActive(true);
                    break;
                }
            }
        }

        public void CheckNewZombieBuzz()
        {
            var enemies = GameData.EnemiesGroup.GetAllEnemyDatas();
            for (int i = 0; i < enemies.Count; i++)
            {
                if (!enemies[i].Viewed)
                {
                    imgCollectionBuzz.SetActive(true);
                    return;
                }
            }
        }

        private void ShowInfoMap()
        {
            var areaId = GameData.MissionsGroup.GetCurrentArea().id;
            progressTransform.SetSiblingIndex(areaId);
            if (areaId == 1) imgAreas[2].SetActive(false);
            else if (areaId == 2) imgAreas[0].SetActive(false);
            else if (areaId == 3)
            {
                imgAreas[0].SetActive(false);
                imgAreas[1].SetActive(false);
            }

            listBoss.Free();
            listBrain.Free();
            listMission.Free();
            var missions = GameData.MissionsGroup.GetListMissionInCurrentArea();
            var currentMissionId = GameData.MissionsGroup.CurrentMissionId;
            int done = 0;
            for (int i = 0; i < missions.Count - 1; ++i)
            {
                if (missions[i].id < currentMissionId)
                {
                    done++;
                    var mission = listMission.Obtain(listMission[0].transform.parent);
                    mission.gameObject.SetActive(true);
                    mission.preferredWidth = (600f - 3f * (missions.Count - 1)) / missions.Count;
                }
                else
                {
                    if (missions[i].hasBoss)
                    {
                        var boss = listBoss.Obtain(listBoss[0].transform.parent);
                        boss.transform.localPosition = new Vector3(-300f + 600f * (i - 0.2f) / (missions.Count - 1), -8f, 0f);
                        boss.gameObject.SetActive(true);

                        if(missions[i].id == currentMissionId)
                        {
                            imgBoss.SetActive(true);
                            imgBrain.SetActive(false);
                            imgBossE.SetActive(false);
                        }
                    }
                    else
                    {
                        //UnityEngine.Debug.Log(missions[i].battleJsonFile);
                        //var infoStage = InfoStage.CreateFromJSON("Datas/Map/" + missions[i].battleJsonFile);
                        //if (infoStage.typeModeGame == Config.TYPE_MODE_BONUS)
                        //{
                        //    var brain = listBrain.Obtain(listBrain[0].transform.parent);
                        //    brain.transform.localPosition = new Vector3(-300f + 600f * (i - 0.2f) / (missions.Count - 1), -8f, 0f);
                        //    brain.gameObject.SetActive(true);

                        //    if (missions[i].id == currentMissionId)
                        //    {
                        //        imgBoss.SetActive(false);
                        //        imgBrain.SetActive(true);
                        //        imgBossE.SetActive(false);
                        //    }
                        //}
                    }
                }
            }
            if (missions[missions.Count - 1].id == currentMissionId)
            {
                imgBoss.SetActive(false);
                imgBrain.SetActive(false);
                imgBossE.SetActive(true);
            }
            //mapProgress.sizeDelta = new Vector2(600 * done / (missions.Count - 1), mapProgress.sizeDelta.y);

            return;
            // iconMissionsPool.Free();
            // Image iconMission;
            // var missions = GameData.MissionsGroup.GetListMissionInCurrentArea();
            // var currentMissionId = GameData.MissionsGroup.CurrentMissionId;
            // foreach (var item in missions)
            // {
            //     iconMission = iconMissionsPool.Obtain(iconMissionGrid);
            //     if (item.hasBoss)
            //     {
            //         if (item.id < currentMissionId) iconMission.sprite = iconMissionBossOn;
            //         else iconMission.sprite = iconMissionBossOff;
            //     }
            //     else
            //     {
            //         if (item.id < currentMissionId) iconMission.sprite = iconMissionOn;
            //         else iconMission.sprite = iconMissionOff;
            //     }
            //     iconMission.SetNativeSize();
            //     iconMission.SetActive(true);
            // }
            // txtArea1.text = GameData.MissionsGroup.GetCurrentArea().name;
            // var nextArea = GameData.MissionsGroup.GetNextArea();
            // if (nextArea != null) txtArea2.text = nextArea.name;
            // else txtArea2.text = "Nowhere";

            // missionBanner.SetActive(true);
        }

        private void BtnShop_Pressed()
        {
            MainPanel.instance.ShowShopPanel();
        }

        private void BtnEditTeam_Pressed()
        {
            MainPanel.instance.ShowEditTeamPanel();
        }

        private void BtnEditTeamLock_Pressed()
        {
            //reset btnHeroes
            btnEditTeamLock.SetActive(false);
            txtEditTeamLabel.SetActive(false);
            for (int i = 0; i < btnCharacters.Count; i++)
            {
                btnCharacters[i].ToNormal();
            }

            MainPanel.instance.ShowEditTeamPanel();
        }

        private void BtnSpinningWheel_Pressed()
        {
            MainPanel.instance.ShowSpinningWheelPanel();
        }

        private void BtnFight_Pressed()
        {
            MainPanel.instance.ShowMissionDetailPanel();
        }

        private void BtnSafe_Pressed()
        {
            MainPanel.instance.ShowSafePopup();
        }

        private void BtnBoss_Pressed()
        {
            MainPanel.instance.ShowBossPopup();
        }

        private void BtnCollection_Pressed()
        {
            MainPanel.instance.ShowBestiaryPanel();
        }

        private void BtnSetting_Pressed()
        {
            MainPanel.instance.ShowSettingPanel();
        }

        private void ShowHero(HeroData heroData)
        {
            if (!btnEditTeamLock.gameObject.activeSelf)
            {
                MainPanel.instance.ShowUpgradeHeroPanel(heroData);
            }
            else
            {
                GameData.HeroesGroup.ReplaceUnit(heroData.Id, mHero.Id);
                BtnEditTeamLock_Pressed();
            }
        }

        private void OnHeroEquip(HeroEquipEvent e)
        {
            for (int i = 0; i < btnCharacters.Count; i++)
            {
                btnCharacters[i].SetActive(false);
            }

            var heroDatas = GameData.HeroesGroup.GetEquippedHeroes();
            var count = heroDatas.Count;
            for (int i = 0; i < count; i++)
            {
                var characterData = heroDatas[i] as HeroData;
                btnCharacters[i].Init(characterData, ShowHero);
                btnCharacters[i].SetActive(true);
            }
        }

        public void ShowEditTeam(HeroData pHero)
        {
            mHero = pHero;
            btnEditTeamLock.SetActive(true);
            txtEditTeamLabel.SetActive(true);
            for (int i = 0; i < btnCharacters.Count; i++)
            {
                btnCharacters[i].ToEditTeam();
            }
        }

        private void OnMainPanelChildShow(MyGamesBasePanel pPanel)
        {
            //if (pPanel == MainPanel.instance.ShopPanel)
            //{
            //    imgShopBuzz.SetActive(false);
            //}
            //else 
            if (pPanel == MainPanel.instance.ZombiaryPanel)
            {
                imgCollectionBuzz.SetActive(false);
            }
            else if (pPanel == MainPanel.instance.SafePopup)
            {
                imgSafeBuzz.SetActive(false);
            }
            //else if (pPanel == MainPanel.instance.TeamPanel)
            //{
            //    imgTroopBuzz.SetActive(false);
            //}
        }

        private void OnMainPanelChildHide(MyGamesBasePanel pPanel)
        {
            //if (pPanel == MainPanel.instance.TeamPanel)
            //{
            //    imgTroopBuzz.SetActive(false);
            //    //Showing character notification
            //    var units = GameData.UnitsGroup.GetUnits(true, true);
            //    for (int i = 0; i < units.Count; i++)
            //        if (units[i].Unlocked && units[i].Buzzed)
            //        {
            //            imgTroopBuzz.SetActive(true);
            //            break;
            //        }
            //}
            if (pPanel == MainPanel.instance.SafePopup)
            {
                imgSafeBuzz.SetActive(false);
            }
        }

        private void OnHeroUnlocked(HeroUnlockedEvent e)
        {
            imgEditTeamBuzz.SetActive(true);
            var countUnlocked = 0;
            var heroDatas = GameData.HeroesGroup.GetAllHeroDatas();
            foreach (var item in heroDatas)
            {
                if (item.Unlocked)
                {
                    countUnlocked++;
                }
            }
            btnEditTeam.SetActive(countUnlocked > btnCharacters.Count);
        }

        private void OnSafeChangeValue(SafeChangeValueEvent e)
        {
            if (e.value)
            {
                imgSafeBuzz.SetActive(true);
            }
        }

        private void OnGamePaymentInitialized(GamePaymentInitializedEvent e)
        {
            btnShop.SetActive(GamePayment.Instance.Initialized);
        }

        //private IEnumerator IEUpdate()
        //{
        //    var interval = new WaitForSeconds(0.5f);
        //    while (true)
        //    {
        //        if (GameData.CurrenciesGroup.FuelCountdownSeconds == 0) mTxtCountDownFuel.text = "";
        //        else mTxtCountDownFuel.text = TimeHelper.FormatHHMMss(GameData.CurrenciesGroup.FuelCountdownSeconds, true);

        //        if (GameData.DailyLoginGroup.RefreshBossSeconds == 0) mTxtCountDownBoss.text = "";
        //        else mTxtCountDownBoss.text = TimeHelper.FormatHHMMss(GameData.DailyLoginGroup.RefreshBossSeconds, true);

        //        yield return interval;
        //    }
        //}

        ////===Stack Effect Coroutines
        //private IEnumerator IEWait()
        //{
        //    yield return new WaitForSeconds(0.5f);
        //    yield return new WaitUntil(() => !MainPanel.instance.IsBusy()
        //                                  && !TutorialsManager.Instance.IsShowingFixedTut
        //                                  && MainPanel.instance.TopPanel != null && MainPanel.instance.TopPanel == this);
        //}
    }
}