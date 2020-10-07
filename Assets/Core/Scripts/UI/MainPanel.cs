using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Common;
using Utilities.Pattern.UI;
using Utilities.Inspector;

using TMPro;
using Utilities.Components;
using UnityEngine.SceneManagement;

namespace FoodZombie.UI
{
    public class MyGamesBasePanel : PanelController
    {
        public bool playSound = true;

        protected override void BeforeShowing()
        {
            base.BeforeShowing();

            if (playSound)
                SoundManager.Instance.PlaySFX(IDs.SOUND_UI_POPUP_OPEN, false);
        }

        protected override void BeforeHiding()
        {
            base.BeforeHiding();

            if (playSound)
                SoundManager.Instance.PlaySFX(IDs.SOUND_UI_POPUP_CLOSE, false);
        }

        public bool IsActiveOrEnable()
        {
            return !gameObject.IsPrefab() && gameObject.activeSelf && this.isActiveAndEnabled;
        }
    }

    //=================================================

    public class MainPanel : MyGamesBasePanel
    {
        #region Members

        public static MainPanel mInstance;
        public static MainPanel instance
        {
            get
            {
                if (mInstance == null)
                    mInstance = FindObjectOfType<MainPanel>();
                return mInstance;
            }
        }

        public Action<MyGamesBasePanel> onAnyChildShow;
        public Action<MyGamesBasePanel> onAnyChildHide;

        [Separator("Common")]
        [SerializeField] private Button mBtnBackLayer;

        [Separator("Build-in Panels")]
        [SerializeField] private MainMenuPanel mMainMenuPanel;

        [Separator("Panel Prefabs")]
        [SerializeField] private LoadingPanel mLoadingPanel;
        [SerializeField] private ZombiaryPanel mZombiaryPanel;
        [SerializeField] private SettingPanel mSettingPanel;
        [SerializeField] private ShopPanel mShopPanel;
        [SerializeField] private OpenChestPanel mOpenChestPanel;
        [SerializeField] private UpgradeVehiclePanel mVehiclePanel;
        [SerializeField] private UpgradeFoodPanel mFoodPanel;
        [SerializeField] private UpgradeHeroPanel mUpgradeHeroPanel;
        [SerializeField] private SpinningWheel mSpinningWheelPanel;
        [SerializeField] private MessagesPopup mMessagePopup;
        [SerializeField] private RewardsPopup mRewardsPopup;
        [SerializeField] private EvolveRewardsPopup mEvolveRewardsPopup;
        [SerializeField] private RatePanel mRatePanel;
        [SerializeField] private EditTeamPanel mEditTeamPanel;
        [SerializeField] private MissionDetailPanel mMissionDetailPanel;
        [SerializeField] private RescueHeroPanel mRescueHeroPanel;
        [SerializeField] private SafePopup mSafePopup;
        [SerializeField] private BossPopup mBossPopup;

        [Separator("UI Widgets")]
        [SerializeField] private UITooltips mUIWidgets;
        [SerializeField] private UIEffects mEffects;

        public MainMenuPanel MainMenuPanel => GetCachedPanel(mMainMenuPanel);
        public LoadingPanel LoadingPanel => GetCachedPanel(mLoadingPanel);
        public ZombiaryPanel ZombiaryPanel => GetCachedPanel(mZombiaryPanel);
        public SettingPanel SettingPanel => GetCachedPanel(mSettingPanel);
        public ShopPanel ShopPanel => GetCachedPanel(mShopPanel);
        public OpenChestPanel OpenChestPanel => GetCachedPanel(mOpenChestPanel);
        public UpgradeVehiclePanel VehiclePanel => GetCachedPanel(mVehiclePanel);
        public UpgradeFoodPanel FoodPanel => GetCachedPanel(mFoodPanel);
        public UpgradeHeroPanel UpgradeHeroPanel => GetCachedPanel(mUpgradeHeroPanel);
        public SpinningWheel SpinningWheelPanel => GetCachedPanel(mSpinningWheelPanel);
        public EditTeamPanel EditTeamPanel => GetCachedPanel(mEditTeamPanel);
        public MissionDetailPanel MissionDetailPanel => GetCachedPanel(mMissionDetailPanel);
        public RescueHeroPanel RescueHeroPanel => GetCachedPanel(mRescueHeroPanel);
        public SafePopup SafePopup => GetCachedPanel(mSafePopup);
        public UITooltips UITooltips => mUIWidgets;

        /// <summary>
        /// Cached for once use panel
        /// </summary>
        private Dictionary<int, MyGamesBasePanel> mCachedPanels;
        private Queue<MessagesPopup.Message> mMessageQueue = new Queue<MessagesPopup.Message>();
        private bool mInitialized;

        private GameData GameData => GameData.Instance;

        public bool Initialized => mInitialized;

        #endregion

        //=============================================

        #region MonoBehaviour

        private void Start()
        {
            //mBtnBackLayer.onClick.AddListener(BtnBackLayer_Pressed);

            GameInitializer.Instance.SendLoadingTime();
        }

        private void OnEnable()
        {
            StartCoroutine(IECustomUpdate());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                BtnBackLayer_Pressed();
        }

        private void OnDestroy()
        {
            //EventDispatcher.RemoveListener<ShowRewardsEvent>(OnShowRewardsEvent);
        }

        private IEnumerator IECustomUpdate()
        {
            var interval = new WaitForSeconds(5f);
            bool processing = false;
            while (true)
            {
                yield return interval;

                if (!processing && GameData.Instance.WaitingForAutoBackup)
                {
                    processing = true;

                    GameInitializer.Instance.BackupGameData((success) =>
                    {
                        processing = false;
                        GameData.Instance.WaitForAutoBackup(false);
                    });
                }
            }
        }

        #endregion

        //=============================================

        #region Public

        internal override void Init()
        {
            InitPanels();

            //EventDispatcher.AddListener<ShowRewardsEvent>(OnShowRewardsEvent);

            mInitialized = true;
        }

        public void ShowMainMenuPanel()
        {
            PushPanelToTop(ref mMainMenuPanel);
        }

        public void ShowLoadingPanel()
        {
            PushPanelToTop(ref mLoadingPanel);
        }

        public void LoadGamePlayScreen()
        {
            PushPanelToTop(ref mLoadingPanel);
            mLoadingPanel.LoadGamePlayScreen();
        }

        public void ShowBestiaryPanel()
        {
            PushPanelToTop(ref mZombiaryPanel);
        }

        public void ShowSettingPanel()
        {
            PushPanelToTop(ref mSettingPanel);
        }

        public void ShowShopPanel()
        {
            PushPanelToTop(ref mShopPanel);
        }

        public void ShowOpenChestPanel()
        {
            PushPanelToTop(ref mOpenChestPanel);
        }

        public void ShowVehiclePanel()
        {
            PushPanelToTop(ref mVehiclePanel);
        }

        public void ShowFoodPanel()
        {
            PushPanelToTop(ref mFoodPanel);
        }

        public void ShowSpinningWheelPanel()
        {
            PushPanelToTop(ref mSpinningWheelPanel);

            // PushPanelToTop(ref mOpenChestPanel);
        }

        public void ShowUpgradeHeroPanel(HeroData pHeroData)
        {
            PushPanelToTop(ref mUpgradeHeroPanel);
            mUpgradeHeroPanel.Init(pHeroData);
        }

        public void ShowEditTeamPanel()
        {
            PushPanelToTop(ref mEditTeamPanel);
        }

        public void ShowEditTeamInMainMenu(HeroData hero)
        {
            mMainMenuPanel.ShowEditTeam(hero);
        }

        public void ShowMissionDetailPanel()
        {
            PushPanelToTop(ref mMissionDetailPanel);
        }

        public void ShowMessageTooltip(RectTransform pTarget, UITooltips.Message pMessage)
        {
            if (mUIWidgets.gameObject.IsPrefab())
                mUIWidgets = Instantiate(mUIWidgets, transform);

            mUIWidgets.SetActive(true);
            mUIWidgets.ShowSimpleMessage(pTarget, pMessage);
            mUIWidgets.transform.SetAsLastSibling();
        }

        internal bool IsBusy()
        {
            foreach (var p in panelStack)
                if (!p.CanPop())
                    return true;
            return false;
        }

        public void ShowWarningTooltip(RectTransform pTarget, UITooltips.Message pMessage)
        {
            if (mUIWidgets.gameObject.IsPrefab())
                mUIWidgets = Instantiate(mUIWidgets, transform);

            mUIWidgets.SetActive(true);
            mUIWidgets.ShowWarning(pTarget, pMessage);
            mUIWidgets.transform.SetAsLastSibling();
        }

        //public void ShowLockReasonMessage(RectTransform pTarget, LockReason pReason)
        //{
        //    if (mUIWidgets.gameObject.IsPrefab())
        //        mUIWidgets = Instantiate(mUIWidgets, transform);

        //    var message = new UITooltips.Message();
        //    message.size = new Vector2(500, 100);
        //    switch (pReason.lockReasonType)
        //    {
        //        case LockReasonType.NotEnoughItems:
        //            message.message = $"Not enough items"; break;
        //        case LockReasonType.NotEnoughVehicleLevel:
        //            message.message = $"Need Plane Level {pReason.requiredValue}"; break;
        //        case LockReasonType.NotEnoughPlayerLevel:
        //            message.message = $"Need Player Level {pReason.requiredValue}"; break;
        //        case LockReasonType.NotEnoughMoney:
        //            message.message = $"Not enough resources"; break;
        //        case LockReasonType.NotHired:
        //            message.message = $"Unit is not recruited yet"; break;
        //        case LockReasonType.NotUnlocked:
        //            message.message = $"Unit is not unlocked yet"; break;
        //        case LockReasonType.FullSlot:
        //            message.message = "Full Slot"; break;
        //        case LockReasonType.Existed:
        //            message.message = "Existed"; break;
        //    }

        //    mUIWidgets.SetActive(true);
        //    mUIWidgets.ShowWarning(pTarget, message);
        //    mUIWidgets.transform.SetAsLastSibling();
        //}

        public MessageWithPointer ShowNotificationBoard(RectTransform pTarget, string pMessage, Alignment pAlign, Vector2 pSize)
        {
            return ShowNotificationBoard(new UITooltips.Notification(0)
            {
                target = pTarget,
                alignment = pAlign,
                message = pMessage,
                size = pSize
            });
        }

        public MessageWithPointer ShowNotificationBoard(UITooltips.Notification pNotification)
        {
            if (mUIWidgets.gameObject.IsPrefab())
                mUIWidgets = Instantiate(mUIWidgets, transform);
            mUIWidgets.SetActive(true);
            mUIWidgets.transform.SetAsLastSibling();
            var notiBoard = mUIWidgets.ShowNotificationBoard(pNotification);
            return notiBoard;
        }

        public void HideNotificationBoard(int pId)
        {
            mUIWidgets.HideNotificationBoard(pId);
        }

        public void ShowOptionsToolTip(RectTransform pTarget, UITooltips.Option[] pOptions, float pWidth, Action pCancelAction = null)
        {
            if (mUIWidgets.gameObject.IsPrefab())
                mUIWidgets = Instantiate(mUIWidgets, transform);

            mUIWidgets.SetActive(true);
            mUIWidgets.ShowOptions(pTarget, pOptions, pWidth, pCancelAction);
            mUIWidgets.transform.SetAsLastSibling();
        }

        public void ShowTradeToolTip(RectTransform pTarget, UITooltips.QuickTrade pOption)
        {
            if (mUIWidgets.gameObject.IsPrefab())
                mUIWidgets = Instantiate(mUIWidgets, transform);

            mUIWidgets.SetActive(true);
            mUIWidgets.ShowTradeOption(pTarget, pOption);
            mUIWidgets.transform.SetAsLastSibling();
        }

        public void SpawnEffect(RectTransform pFrom, UIEffects.Info pInfo)
        {
            if (mEffects.gameObject.IsPrefab())
                mEffects = Instantiate(mEffects, transform);

            mEffects.SetActive(true);
            mEffects.SpawnEffect(pFrom, pInfo);
            mEffects.transform.SetAsLastSibling();
        }

        public void SpawnEffect(RectTransform pFrom, RewardInfo pReward)
        {
            if (mEffects.gameObject.IsPrefab())
                mEffects = Instantiate(mEffects, transform);

            mEffects.SetActive(true);
            mEffects.SpawnEffectRandomNearByFrom(pFrom, pReward);
            mEffects.transform.SetAsLastSibling();
        }

        public void ShowMessagePopup(MessagesPopup.Message pMessage)
        {
            if (mMessageQueue.Contains(pMessage))
                return;

            mMessageQueue.Enqueue(pMessage);
            if (mMessagePopup.IsShowing)
                return;

            ShowMessagePopup();
        }

        public void ShowRewardsPopup(RewardInfo pReward, RectTransform pSpawnPosition = null)
        {
            ShowRewardsPopup(new List<RewardInfo>() { pReward }, pSpawnPosition);
        }

        public void ShowRewardsPopup(List<RewardInfo> pRewards, RectTransform pSpawnPosition = null)
        {
            PushPanelToTop(ref mRewardsPopup);
            mRewardsPopup.SetRewards(pRewards, pSpawnPosition);
        }

        public void ShowEvolveRewardsPopup(RewardInfo pReward, RectTransform pSpawnPosition = null)
        {
            ShowEvolveRewardsPopup(new List<RewardInfo>() { pReward }, pSpawnPosition);
        }

        public void ShowEvolveRewardsPopup(List<RewardInfo> pRewards, RectTransform pSpawnPosition = null)
        {
            PushPanelToTop(ref mEvolveRewardsPopup);
            mEvolveRewardsPopup.SetRewards(pRewards, pSpawnPosition);
        }

        public void ShowRatePanelIfAvailable()
        {
            if (GameData.GameConfigGroup.Rated /*|| GameData.GameConfigGroup.ShowedDailyRate*/
                || RatePanel.isShowed) return;

            PushPanelToTop(ref mRatePanel);
            GameData.GameConfigGroup.AddCountShowRate();
        }

        public void ShowRescueHeroPanel(int pHeroId)
        {
            PushPanelToTop(ref mRescueHeroPanel);
            mRescueHeroPanel.Init(pHeroId);
        }

        public void ShowSafePopup()
        {
            PushPanelToTop(ref mSafePopup);
            mSafePopup.Init();
        }

        public void ShowBossPopup()
        {
            PushPanelToTop(ref mBossPopup);
            mBossPopup.Init();
        }

        #endregion

        //==============================================

        #region Private

        private void InitPanels()
        {
            mCachedPanels = new Dictionary<int, MyGamesBasePanel>();

            var panels = new List<MyGamesBasePanel>();
            foreach (Transform t in transform)
            {
                var panel = t.GetComponent<MyGamesBasePanel>();
                if (panel != null)
                    panels.Add(panel);
            }

            foreach (var panel in panels)
                if (panel != this)
                    InitPanel(panel);

            ShowMainMenuPanel();
        }

        private void InitPanel<T>(T pPanel) where T : MyGamesBasePanel
        {
            if (!pPanel.gameObject.IsPrefab())
            {
                pPanel.SetActive(false);
                pPanel.Init();
            }
        }

        private void BtnBackLayer_Pressed()
        {
            if (TopPanel == mMainMenuPanel)
                return;

            if (TopPanel != null)
                TopPanel.Back();
        }

        protected override void OnAnyChildHide(PanelController pLastTop)
        {
            base.OnAnyChildHide(pLastTop);

            if (TopPanel == null || TopPanel is MainMenuPanel)
                mBtnBackLayer.SetActive(false);
            else
            {
                mBtnBackLayer.SetActive(true);
                mBtnBackLayer.transform.SetSiblingIndex(0);//reset về 0 để tránh tình trạng lùi BackLayer quá MainMenuPanel
                mBtnBackLayer.transform.SetSiblingIndex(TopPanel.transform.GetSiblingIndex() - 1);
            }

            onAnyChildHide.Raise(pLastTop as MyGamesBasePanel);
        }

        protected override void OnAnyChildShow(PanelController pPanel)
        {
            base.OnAnyChildShow(pPanel);

            if (TopPanel == null || TopPanel is MainMenuPanel)
                mBtnBackLayer.SetActive(false);
            else
            {
                mBtnBackLayer.SetActive(true);
                mBtnBackLayer.transform.SetSiblingIndex(0);//reset về 0 để tránh tình trạng lùi BackLayer quá MainMenuPanel
                mBtnBackLayer.transform.SetSiblingIndex(TopPanel.transform.GetSiblingIndex() - 1);
            }

            onAnyChildShow.Raise(TopPanel as MyGamesBasePanel);
        }

        private void ShowMessagePopup()
        {
            PushPanelToTop(ref mMessagePopup);
            mMessagePopup.InitMessage(mMessageQueue.Peek());
            mMessagePopup.onDidHide += OnMessagePopupHidden;
        }

        private void OnMessagePopupHidden()
        {
            mMessagePopup.onDidHide -= OnMessagePopupHidden;
            mMessageQueue.Dequeue();

            if (mMessageQueue.Count > 0)
                ShowMessagePopup();
        }

        //private void OnShowRewardsEvent(ShowRewardsEvent e)
        //{
        //    ShowLevelUpPanel(e.rewards, null);
        //}

        #endregion
    }
}