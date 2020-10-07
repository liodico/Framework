using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Components;

namespace FoodZombie.UI
{
    public class TutorialClaimUnit : TutorialController
    {
        private JustButton mBtnRescue;

        private bool mPressedBtn;


        public TutorialClaimUnit(int pId, bool pIsToolTips) : base(pId, pIsToolTips)
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
            //TutorialsManager.LockPanel(MainPanel.RescueHeroPanel);

            //=== step 2: Click to chest button
            yield return new WaitUntil(() => MainPanel.RescueHeroPanel.initialized);
            //Focus to upgrade button
            mBtnRescue = MainPanel.RescueHeroPanel.btnRescue;
            TutorialsManager.InputMasker.FocusToTargetImmediately(mBtnRescue.rectTransform);
            //board = MainPanel.ShowNotificationBoard(mBtnGet.rectTransform, "Tap To Get A Booster Or Pre Unit", Alignment.Top, new Vector2(440, 180));
            //board.transform.SetParent(TutorialsManager.transform);
            mBtnRescue.onClick.RemoveListener(OnBtnRescue_Pressed);
            mBtnRescue.onClick.AddListener(OnBtnRescue_Pressed);

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
            //TutorialsManager.UnlockPanel(MainPanel.RescueHeroPanel);
        }

        private void OnBtnRescue_Pressed()
        {
            mPressedBtn = true;
            mBtnRescue.onClick.RemoveListener(OnBtnRescue_Pressed);
        }
    }
}
