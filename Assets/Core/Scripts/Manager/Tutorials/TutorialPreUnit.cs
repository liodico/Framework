using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Components;

namespace FoodZombie.UI
{
    public class TutorialPreUnit : TutorialController
    {
        private JustButton mBtnFight;
        private JustButton mBtnGet;
        private JustButton mBtnPlay;

        private bool mPressedBtn;


        public TutorialPreUnit(int pId, bool pIsToolTips) : base(pId, pIsToolTips)
        {
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Pause()
        {
        }

        public override void Resume()
        {
        }

        protected override void OnAnyChildHide(MyGamesBasePanel pPanel)
        {
        }

        protected override void OnAnyChildShow(MyGamesBasePanel pPanel)
        {
        }

        public override IEnumerator IEProcess()
        {
            //=== step 1:
            //Focus to fight button
            //mBtnFight = MainPanel.MainMenuPanel.btnFight;
            //TutorialsManager.InputMasker.FocusToTargetImmediately(mBtnFight.rectTransform, true);
            //var board = MainPanel.ShowNotificationBoard(mBtnFight.rectTransform, "Tap To Play", Alignment.Top, new Vector2(440, 180));
            //board.transform.SetParent(TutorialsManager.transform);

            yield return new WaitUntil(() => MainPanel.MissionDetailPanel.IsActiveAndEnabled());
            //Lock panel, we dont want player accidently back this panel during tutorial
            TutorialsManager.LockPanel(MainPanel.MissionDetailPanel);

            //=== step 2: Click to chest button
            yield return new WaitUntil(() => MainPanel.MissionDetailPanel.initialized);
            //Focus to upgrade button
            mBtnGet = MainPanel.MissionDetailPanel.btnGet;
            TutorialsManager.InputMasker.FocusToTargetImmediately(mBtnGet.rectTransform);
            //board = MainPanel.ShowNotificationBoard(mBtnGet.rectTransform, "Tap To Get A Booster Or Pre Unit", Alignment.Top, new Vector2(440, 180));
            //board.transform.SetParent(TutorialsManager.transform);
            mBtnGet.onClick.RemoveListener(OnBtnGet_Pressed);
            mBtnGet.onClick.AddListener(OnBtnGet_Pressed);

            yield return new WaitUntil(() => mPressedBtn);
            yield return null;

            //=== step 4: Click to upgrade button
            End(false);
            yield return new WaitForSeconds(2f);
            yield return new WaitUntil(() => MainPanel.TopPanel is MissionDetailPanel);

            mPressedBtn = false;
            mBtnPlay = MainPanel.MissionDetailPanel.btnPlay;
            //Focus to upgrade button
            TutorialsManager.InputMasker.FocusToTargetImmediately(mBtnPlay.rectTransform);
            //MainPanel.ShowNotificationBoard(mBtnPlay.rectTransform, "Play", Alignment.Top, new Vector2(440, 180));
            //board.transform.SetParent(TutorialsManager.transform);
            mBtnPlay.onClick.RemoveListener(OnBtnPlay_Pressed);
            mBtnPlay.onClick.AddListener(OnBtnPlay_Pressed);

            yield return new WaitUntil(() => mPressedBtn);
            yield return null;

            End(true);
        }

        public override void End(bool pFinished)
        {
            base.End(pFinished);

            MainPanel.UITooltips.LockOptionsGrid(false);

            //Unlock input
            TutorialsManager.InputMasker.Active(false);

            //Hide notification board
            MainPanel.HideNotificationBoard(0);

            //Unlock panel
            TutorialsManager.UnlockPanel(MainPanel.MissionDetailPanel);
        }

        private void OnBtnGet_Pressed()
        {
            mPressedBtn = true;
            mBtnGet.onClick.RemoveListener(OnBtnGet_Pressed);
        }

        private void OnBtnPlay_Pressed()
        {
            mPressedBtn = true;
            mBtnPlay.onClick.RemoveListener(OnBtnPlay_Pressed);
        }
    }
}
