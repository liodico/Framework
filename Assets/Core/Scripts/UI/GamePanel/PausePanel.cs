using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Inspector;
using Utilities.Components;
using System;
using Spine.Unity;
using Utilities.Common;
using UnityEngine.SceneManagement;

namespace FoodZombie.UI
{
    public class PausePanel : MyGamesBasePanel
    {
        #region Members

        public SimpleTMPButton btnHome;
        public SimpleTMPButton btnResume;
        public SimpleTMPButton btnRetry;

        public GameObject confirmPanel;
        public SimpleTMPButton btnOk;
        public SimpleTMPButton btnCancel;

        #endregion

        //=============================================

        #region MonoBehaviour

        private void Start()
        {
            btnHome.onClick.AddListener(BtnHome_Pressed);
            btnResume.onClick.AddListener(BtnResume_Pressed);
            btnRetry.onClick.AddListener(BtnRetry_Pressed);

            btnOk.onClick.AddListener(BtnOk_Pressed);
            btnCancel.onClick.AddListener(BtnCancel_Pressed);
        }

        private void OnEnable()
        {
            GameplayController.Instance.PauseGame();
        }

        #endregion

        //=============================================

        #region Public

        #endregion

        //==============================================

        #region Private

        internal override void Back()
        {
            GameplayController.Instance.ResumeGame();
            base.Back();
        }

        private void BtnHome_Pressed()
        {
            confirmPanel.SetActive(true);
        }

        private void BtnOk_Pressed()
        {
            GameplayController.Instance.ResumeGame();
            SceneManager.LoadScene("Home");
        }

        private void BtnCancel_Pressed()
        {
            confirmPanel.SetActive(false);
        }

        private void BtnResume_Pressed()
        {
            Back();
        }

        private void BtnRetry_Pressed()
        {
            GameplayController.Instance.ResumeGame();
            SceneManager.LoadScene("GamePlay");
        }
        #endregion
    }
}