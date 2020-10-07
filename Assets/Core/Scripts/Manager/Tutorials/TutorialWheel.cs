using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Components;

namespace FoodZombie.UI
{
    public class TutorialWheel : TutorialController
    {
        private JustButton mBtnWheel;
        private JustButton mBtnSpin;

        private bool mPressedBtn;


        public TutorialWheel(int pId, bool pIsToolTips) : base(pId, pIsToolTips)
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
            mBtnWheel = MainPanel.MainMenuPanel.btnWheel;
            TutorialsManager.InputMasker.FocusToTargetImmediately(mBtnWheel.rectTransform);
            //var board = MainPanel.ShowNotificationBoard(mBtnFight.rectTransform, "Tap To Play", Alignment.Top, new Vector2(440, 180));
            //board.transform.SetParent(TutorialsManager.transform);

            yield return new WaitUntil(() => MainPanel.SpinningWheelPanel.IsActiveAndEnabled());
            //Lock panel, we dont want player accidently back this panel during tutorial
            TutorialsManager.LockPanel(MainPanel.SpinningWheelPanel);

            //=== step 2: Click to chest button
            yield return new WaitUntil(() => MainPanel.SpinningWheelPanel.initialized);
            //Focus to upgrade button
            mBtnSpin = MainPanel.SpinningWheelPanel.BtnSpin;
            TutorialsManager.InputMasker.FocusToTargetImmediately(mBtnSpin.rectTransform);
            //board = MainPanel.ShowNotificationBoard(mBtnGet.rectTransform, "Tap To Get A Booster Or Pre Unit", Alignment.Top, new Vector2(440, 180));
            //board.transform.SetParent(TutorialsManager.transform);
            mBtnSpin.onClick.RemoveListener(OnBtnSpin_Pressed);
            mBtnSpin.onClick.AddListener(OnBtnSpin_Pressed);

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
            TutorialsManager.UnlockPanel(MainPanel.SpinningWheelPanel);
        }

        private void OnBtnSpin_Pressed()
        {
            mPressedBtn = true;
            mBtnSpin.onClick.RemoveListener(OnBtnSpin_Pressed);
        }
    }
}
