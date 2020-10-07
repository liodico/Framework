using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Components;

namespace FoodZombie.UI
{
    public class TutorialUpgradeUnit : TutorialController
    {
        private JustButton mBtnUpgrade;
        private JustButton mBtnBack;
        private JustButton mBtnFight;

        private MainMenuButtonCharacter mBtnCharacter;

        private bool mPressedBtn;


        public TutorialUpgradeUnit(int pId, bool pIsToolTips) : base(pId, pIsToolTips)
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
            MainPanel.MainMenuPanel.lockTutorial.SetActive(true);
            var dialog = GameObject.FindObjectOfType<DialogTutorialUpgrade>().dialog;
            dialog.SetActive(true);
            yield return new WaitForSeconds(1.0f);
            MainPanel.MainMenuPanel.lockTutorial.SetActive(false);

            mBtnCharacter = MainPanel.MainMenuPanel.btnCharacters[0];
            TutorialsManager.InputMasker.FocusToTargetImmediately(mBtnCharacter.objectTapRenderer, MainPanel.transform.parent.rectTransform(), Camera.main, Vector2.zero);

            yield return new WaitUntil(() => MainPanel.UpgradeHeroPanel.IsActiveAndEnabled());
            //Lock panel, we dont want player accidently back this panel during tutorial
            TutorialsManager.LockPanel(MainPanel.UpgradeHeroPanel);
            dialog.SetActive(false);

            //=== step 2: Click to upgrade button
            yield return new WaitUntil(() => MainPanel.UpgradeHeroPanel.initialized);
            //Focus to upgrade button
            mBtnUpgrade = MainPanel.UpgradeHeroPanel.btnUpgrade;
            TutorialsManager.InputMasker.FocusToTargetImmediately(mBtnUpgrade.rectTransform);
            //var board = MainPanel.ShowNotificationBoard(mBtnUpgrade.rectTransform, "Tap To Upgrade Unit", Alignment.Top, new Vector2(440, 180));
            //board.transform.SetParent(TutorialsManager.transform);
            mBtnUpgrade.onClick.RemoveListener(OnBtnUpgrade_Pressed);
            mBtnUpgrade.onClick.AddListener(OnBtnUpgrade_Pressed);

            yield return new WaitUntil(() => mPressedBtn);
            yield return null;

            //=== step 4: Click to upgrade button
            mPressedBtn = false;
            //Focus to upgrade button
            TutorialsManager.InputMasker.FocusToTargetImmediately(mBtnUpgrade.rectTransform);
            //MainPanel.ShowNotificationBoard(mBtnUpgrade.rectTransform, "Again", Alignment.Top, new Vector2(440, 180));
            //board.transform.SetParent(TutorialsManager.transform);
            mBtnUpgrade.onClick.RemoveListener(OnBtnUpgrade_Pressed);
            mBtnUpgrade.onClick.AddListener(OnBtnUpgrade_Pressed);

            yield return new WaitUntil(() => mPressedBtn);
            yield return null;

            //=== step 5: Click to close button
            mPressedBtn = false;
            //Focus to close button
            mBtnBack = MainPanel.UpgradeHeroPanel.btnBackTutorial;
            //TutorialsManager.InputMasker.FocusToTargetImmediately(mBtnBack.rectTransform);
            //MainPanel.ShowNotificationBoard(mBtnUpgrade.rectTransform, "Again", Alignment.Top, new Vector2(440, 180));
            //board.transform.SetParent(TutorialsManager.transform);
            TutorialsManager.InputMasker.Active(false);
            TutorialsManager.UnlockPanel(MainPanel.UpgradeHeroPanel);
            mBtnBack.onClick.RemoveListener(OnBtnBack_Pressed);
            mBtnBack.onClick.AddListener(OnBtnBack_Pressed);

            yield return new WaitUntil(() => mPressedBtn);
            yield return null;

            //Focus to fight button
            mPressedBtn = false;
            mBtnFight = MainPanel.MainMenuPanel.btnFight;
            TutorialsManager.InputMasker.FocusToTargetImmediately(mBtnFight.rectTransform, true);
            //var board = MainPanel.ShowNotificationBoard(mBtnFight.rectTransform, "Tap To Play", Alignment.Top, new Vector2(440, 180));
            //board.transform.SetParent(TutorialsManager.transform);
            mBtnFight.onClick.RemoveListener(OnBtnFight_Pressed);
            mBtnFight.onClick.AddListener(OnBtnFight_Pressed);

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
            TutorialsManager.UnlockPanel(MainPanel.UpgradeHeroPanel);
        }

        private void OnBtnUpgrade_Pressed()
        {
            mPressedBtn = true;
            mBtnUpgrade.onClick.RemoveListener(OnBtnUpgrade_Pressed);
        }

        private void OnBtnBack_Pressed()
        {
            mPressedBtn = true;
            mBtnUpgrade.onClick.RemoveListener(OnBtnBack_Pressed);
        }
        private void OnBtnFight_Pressed()
        {
            mPressedBtn = true;
            mBtnUpgrade.onClick.RemoveListener(OnBtnFight_Pressed);
        }

    }
}
