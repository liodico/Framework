
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities.Common;
using Utilities.Components;
using Utilities.Inspector;
using Utilities.Pattern.UI;

namespace FoodZombie.UI
{
    public class LoadingPanel : MyGamesBasePanel
    {
        [Separator("Panel Prefabs")]
        [SerializeField] private EarlyAccessWarningPopup mEarlyAccessWarningPopup;

        [Separator("Element")]
        [SerializeField] private Image mImgProcessBg;
        [SerializeField] private Image mImgProcess;
        [SerializeField] private TextMeshProUGUI mTxtVersion;
        [SerializeField] private TextMeshProUGUI mTxtHints;

        public void Start()
        {
            mTxtVersion.text = "Ver: " + Application.version;

            //if (mTxtHints != null)
            //{
            //    var localizedHint = new LocalizationGetter("HINTS_" + Random.Range(1, 6), "You can throw grenades on oil to burn enemies.");
            //    mTxtHints.text = Localization.Get(Localization.HINTS_TITLE) + localizedHint.Get();
            //}
        }

        public void LoadHomeScene()
        {
            float curProgress = 0;
            float w = mImgProcessBg.rectTransform.sizeDelta.x;
            float h = mImgProcess.rectTransform.sizeDelta.y;

            SceneLoader.LoadScene("Home", false, true, (progress) =>
            {
                if (progress > curProgress)
                    curProgress = progress;
                if (mImgProcess.rectTransform != null)
                {
                    mImgProcess.rectTransform.sizeDelta = new Vector2(w * curProgress, h);
                }
            }, null);
        }

        public void ShowEarlyAccessWarningPopup()
        {
            PushPanelToTop(ref mEarlyAccessWarningPopup);
            mEarlyAccessWarningPopup.Init(LoadGamePlayScreen);
        }

        public void LoadGamePlayScreen()
        {
            float curProgress = 0;
            float w = mImgProcessBg.rectTransform.sizeDelta.x;
            float h = mImgProcess.rectTransform.sizeDelta.y;

            SceneLoader.LoadScene("GamePlay", false, true, (progress) =>
            {
                if (progress > curProgress)
                    curProgress = progress;

                mImgProcess.rectTransform.sizeDelta = new Vector2(w * curProgress, h);
            }, () =>
            {
                EventDispatcher.Raise(new BattleLoadEvent());
            }, 2f);
        }
    }
}