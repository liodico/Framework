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
    public class MainGamePanel : MyGamesBasePanel
    {
        #region Members

        public static MainGamePanel mInstance;
        public static MainGamePanel instance
        {
            get
            {
                if (mInstance == null)
                    mInstance = FindObjectOfType<MainGamePanel>();
                return mInstance;
            }
        }

        public Action<MyGamesBasePanel> onAnyChildShow;
        public Action<MyGamesBasePanel> onAnyChildHide;

        [Separator("Common")]
        [SerializeField] private Button mBtnBackLayer;

        [Separator("Build-in Panels")]
        [SerializeField] private HubPanel mHubPanel;

        [Separator("Panel Prefabs")]
        [SerializeField] private PausePanel mPausePanel;
        [SerializeField] private WinPanel mWinPanel;
        [SerializeField] private LosePanel mLosePanel;

        [SerializeField] private ZombiaryPanel mZombiaryPanel;
        [SerializeField] private MessagesPopup mMessagePopup;
        [SerializeField] private RewardsPopup mRewardsPopup;
        [SerializeField] private EvolveRewardsPopup mEvolveRewardsPopup;

        [Separator("UI Widgets")]
        [SerializeField] private UITooltips mUIWidgets;
        [SerializeField] private UIEffects mEffects;

        public HubPanel HubPanel => GetCachedPanel(mHubPanel);
        public ZombiaryPanel ZombiaryPanel => GetCachedPanel(mZombiaryPanel);
        public UITooltips UITooltips => mUIWidgets;

        /// <summary>
        /// Cached for once use panel
        /// </summary>
        private Dictionary<int, MyGamesBasePanel> mCachedPanels;
        private Queue<MessagesPopup.Message> mMessageQueue = new Queue<MessagesPopup.Message>();
        private bool mInitialized;

        private GameData GameData => GameData.Instance;
        private GameplayController GameplayController => GameplayController.Instance;

        public bool Initialized => mInitialized;

        #endregion

        //=============================================

        #region MonoBehaviour

        private void Start()
        {
            //mBtnBackLayer.onClick.AddListener(BtnBackLayer_Pressed);
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

        #endregion

        //=============================================

        #region Public

        internal override void Init()
        {
            InitPanels();

            GameplayController.Init(GetCachedPanel(mHubPanel));
            //EventDispatcher.AddListener<ShowRewardsEvent>(OnShowRewardsEvent);

            mInitialized = true;
        }
        
        public void ShowHubPanel()
        {
            PushPanelToTop(ref mHubPanel);
        }

        public void ShowBestiaryPanel()
        {
            PushPanelToTop(ref mZombiaryPanel);
        }

        public void ShowPausePanel()
        {
            PushPanelToTop(ref mPausePanel);
        }

        public void ShowWinPanel(List<RewardInfo> rewards, int indexBonusCount)
        {
            PushPanelToTop(ref mWinPanel);
            mWinPanel.Init(rewards, indexBonusCount);
        }

        public void ShowLosePanel(List<RewardInfo> rewards)
        {
            PushPanelToTop(ref mLosePanel);
            mLosePanel.Init(rewards);
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

            ShowHubPanel();
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
            if (TopPanel == mHubPanel)
                return;

            if (TopPanel != null)
                TopPanel.Back();
        }

        protected override void OnAnyChildHide(PanelController pLastTop)
        {
            base.OnAnyChildHide(pLastTop);

            if (TopPanel == null || TopPanel is HubPanel)
                mBtnBackLayer.SetActive(false);
            else
            {
                mBtnBackLayer.SetActive(true);
                mBtnBackLayer.transform.SetSiblingIndex(0);//reset về 0 để tránh tình trạng lùi BackLayer quá HubPanel
                mBtnBackLayer.transform.SetSiblingIndex(TopPanel.transform.GetSiblingIndex() - 1);
            }

            onAnyChildHide.Raise(pLastTop as MyGamesBasePanel);
        }

        protected override void OnAnyChildShow(PanelController pPanel)
        {
            base.OnAnyChildShow(pPanel);

            if (TopPanel == null || TopPanel is HubPanel)
                mBtnBackLayer.SetActive(false);
            else
            {
                mBtnBackLayer.SetActive(true);
                mBtnBackLayer.transform.SetSiblingIndex(0);//reset về 0 để tránh tình trạng lùi BackLayer quá HubPanel
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