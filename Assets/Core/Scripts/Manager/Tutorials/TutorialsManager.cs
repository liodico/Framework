
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Common;
using Utilities.Components;
using FoodZombie.UI;
using Debug = Utilities.Common.Debug;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FoodZombie.UI
{
    public class TutorialsManager : MonoBehaviour
    {
        #region Members

        private static TutorialsManager mInstance;
        public static TutorialsManager Instance
        {
            get
            {
                if (mInstance == null)
                    mInstance = FindObjectOfType<TutorialsManager>();
                return mInstance;
            }
        }

        [SerializeField] private Image mImgBlocker;
        [SerializeField] private HoledLayerMask mInputMasker;

        private List<TutorialController> mAllTutorials;
        private List<TutorialController> mCurrentTooltipTutorials;
        private Queue<TutorialController> mCurrentFixedTutorials;
        private bool mInitialized;
        private bool mIsShowingFixedTut;

        private GameData GameData => GameData.Instance;
        public HoledLayerMask InputMasker => mInputMasker;
        public bool IsShowingFixedTut => mIsShowingFixedTut;

        private List<MyGamesBasePanel> mLockedPanels = new List<MyGamesBasePanel>();

        #endregion

        //=============================================

        #region MonoBehaviour

        private void Awake()
        {
            if (mInstance == null)
                mInstance = this;
            else if (mInstance != this)
                Destroy(gameObject);
        }

        private void OnDestroy()
        {
            EventDispatcher.RemoveListener<TutorialTriggeredEvent>(OnTutorialTriggered);
            EventDispatcher.RemoveListener<TutorialFinishedEvent>(OnTutorialFinished);
        }

        #endregion

        //=============================================

        #region Public

        public void Init()
        {
            EventDispatcher.AddListener<TutorialTriggeredEvent>(OnTutorialTriggered);
            EventDispatcher.AddListener<TutorialFinishedEvent>(OnTutorialFinished);

            mInitialized = true;
            mAllTutorials = new List<TutorialController>();
            mAllTutorials.Add(new TutorialPreUnit(TutorialsGroup.PRE_UNIT, false));
            mAllTutorials.Add(new TutorialUpgradeUnit(TutorialsGroup.UPGRADE_UNIT, false));
            mAllTutorials.Add(new TutorialWheel(TutorialsGroup.WHEEL, false));
            mAllTutorials.Add(new TutorialClaimUnit(TutorialsGroup.CLAIM_UNIT, false));
            //mAllTutorials.Add(new TutorialShop(TutorialsGroup.SHOP, true));
            //mAllTutorials.Add(new TutorialUpgradeVehicle(TutorialsGroup.UPGRADE_VEHICLE, false));

            mCurrentTooltipTutorials = new List<TutorialController>();
            mCurrentFixedTutorials = new Queue<TutorialController>();
        }

        public void Ready()
        {
            StartCoroutine(IECheckConditions());
        }

        public IEnumerator IEBlockInputWhileWaiting(float pTime, bool pClearMasker)
        {
            if (pClearMasker)
                mInputMasker.Active(false);

            mImgBlocker.SetActive(true);
            yield return new WaitForSeconds(pTime);
            mImgBlocker.SetActive(false);
        }

        public void LockPanel(MyGamesBasePanel pPanel)
        {
            pPanel.Lock(true);
            mLockedPanels.Add(pPanel);
        }

        public void UnlockPanel(MyGamesBasePanel pPanel)
        {
            pPanel.Lock(false);
            mLockedPanels.Remove(pPanel);
        }

        public void UnlockAllPanels()
        {
            for (int i = 0; i < mLockedPanels.Count; i++)
                mLockedPanels[i].Lock(false);
            mLockedPanels.Clear();
        }

        public void SkipFixedTutotiral()
        {
            var tut = mCurrentFixedTutorials.Peek();
            tut.End(false);

            StopCoroutine(tut.IEProcess());
            InputMasker.SetActive(false);
            UnlockAllPanels();
            MainPanel.instance.HideNotificationBoard(0);
        }

        #endregion

        //==============================================

        #region Private

        private IEnumerator IECheckConditions()
        {
            yield return new WaitForSeconds(0.1f);
            var currentMissionId = (GameData.Instance.MissionsGroup.CountMissionWin + 1);

            if (IsCompleted(TutorialsGroup.CLAIM_UNIT) && !IsCompleted(TutorialsGroup.PRE_UNIT))
            {
                if (currentMissionId >= 3)
                {
                    TriggerTutorial(TutorialsGroup.PRE_UNIT);
                }
            }

            if (!IsCompleted(TutorialsGroup.CLAIM_UNIT))
            {
                if (currentMissionId > 2 && MainPanel.instance.RescueHeroPanel.IsActiveAndEnabled())
                {
                    TriggerTutorial(TutorialsGroup.CLAIM_UNIT);
                }
            }

            if (!IsCompleted(TutorialsGroup.UPGRADE_UNIT))
            {
                if (currentMissionId >= 2)
                {
                    bool hasUpgradableUnit = GameData.HeroesGroup.HasUpgradableCharacter();
                    if (hasUpgradableUnit)
                        TriggerTutorial(TutorialsGroup.UPGRADE_UNIT);
                }
            }

            //if (!IsCompleted(TutorialsGroup.WHEEL))
            //{
            //    if (LogicAPI.CanShowWheelPanel())
            //        TriggerTutorial(TutorialsGroup.WHEEL);
            //}
        }

        private IEnumerator IEProcessFixedTutorial()
        {
            mIsShowingFixedTut = true;

            PauseToolTipTutorials();

            yield return new WaitUntil(() => MainPanel.instance.Initialized);

            while (mCurrentFixedTutorials.Count > 0)
            {
                yield return new WaitForSeconds(0.25f);
                // có 1 tut ở ngay lúc hiện RescueHeroPanel
                yield return new WaitUntil(() => !MainPanel.instance.IsBusy()
                                                 && (MainPanel.instance.TopPanel is MainMenuPanel
                                                     || MainPanel.instance.TopPanel is RescueHeroPanel)
                                           );

                var tut = mCurrentFixedTutorials.Peek();
                tut.Start();
                StartCoroutine(tut.IEProcess());

                yield return new WaitUntil(() => tut.ended);

                mCurrentFixedTutorials.Dequeue();
            }
            mIsShowingFixedTut = false;

            ResumeToolTipTutorials();
        }

        /// <summary>
        /// NOTE: we have two type of tutorials, ToolTips and Fixed
        /// ToolTips are basically notifications which player can ignore, it can be done anytime
        /// Fixed ones are unignorable, and more important than ToolTips
        /// When Fixed run, we must pause all ToolTips if there are some available and resume when Fixed ones all done
        /// </summary>
        private void TriggerTutorial(int pId)
        {
            if (IsCompleted(pId))
                return;

            TutorialController tut = null;
            for (int i = 0; i < mAllTutorials.Count; i++)
                if (mAllTutorials[i].id == pId)
                {
                    tut = mAllTutorials[i];
                    break;
                }

            if (mCurrentFixedTutorials.Contains(tut) || mCurrentTooltipTutorials.Contains(tut))
                return;

            if (tut != null)
            {
                Debug.Log("Trigger Tutorial " + tut.id);

                if (tut.isToolTips)
                {
                    //Tooltip Tutorials should not interupt Fixed Tutorial
                    if (mIsShowingFixedTut)
                    {
                        //Wait until there is no Fixed tutorial
                        WaitUtil.Start(new WaitUtil.ConditionEvent()
                        {
                            id = tut.id,
                            onTrigger = () => tut.Start(),
                            triggerCondition = () => !mIsShowingFixedTut
                        });
                    }
                    else
                        tut.Start();

                    mCurrentTooltipTutorials.Add(tut);
                }
                else
                {
                    mCurrentFixedTutorials.Enqueue(tut);

                    if (!mIsShowingFixedTut)
                        StartCoroutine(IEProcessFixedTutorial());
                }
            }
        }

        public bool IsCompleted(int pId)
        {
            var data = GameData.Instance.TutorialsGroup.GetTutorial(pId);
            return data != null ? data.Completed : true;
        }

        private void Complete(int pId)
        {
            var data = GameData.Instance.TutorialsGroup.GetTutorial(pId);
            if (data != null)
            {
                data.SetComplete(true);

                for (int i = 0; i < mCurrentTooltipTutorials.Count; i++)
                    if (mCurrentTooltipTutorials[i].id == pId)
                    {
                        mCurrentTooltipTutorials.RemoveAt(i);
                        break;
                    }
            }
        }

        private void OnTutorialFinished(TutorialFinishedEvent e)
        {
            for (int i = 0; i < mCurrentTooltipTutorials.Count; i++)
                if (mCurrentTooltipTutorials[i].id == e.tutorial)
                {
                    mCurrentTooltipTutorials.RemoveAt(i);
                    break;
                }

            var currentMissionId = (GameData.Instance.MissionsGroup.CountMissionWin + 1);
            if (IsCompleted(TutorialsGroup.CLAIM_UNIT) && !IsCompleted(TutorialsGroup.PRE_UNIT))
            {
                if (currentMissionId > 2)
                {
                    TriggerTutorial(TutorialsGroup.PRE_UNIT);
                }
            }
        }

        private void OnTutorialTriggered(TutorialTriggeredEvent e)
        {
            TriggerTutorial(e.tutorial);
        }

        /// <summary>
        /// When fixed tutorial run, we must not let anything interupt it
        /// </summary>
        private void PauseToolTipTutorials()
        {
            for (int i = 0; i < mCurrentTooltipTutorials.Count; i++)
                mCurrentTooltipTutorials[i].Pause();
        }

        private void ResumeToolTipTutorials()
        {
            for (int i = 0; i < mCurrentTooltipTutorials.Count; i++)
                mCurrentTooltipTutorials[i].Resume();
        }

        #endregion
    }
}