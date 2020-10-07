using System;

using TMPro;
using UnityEngine;
using UnityEngine.UI;
using FoodZombie.UI;
using Utilities.Common;

namespace FoodZombie
{
    public class CurrencyView : MonoBehaviour
    {
        #region Members

        [SerializeField] private Image mImgIcon;
        [SerializeField] private TextMeshProUGUI mTxtValue;
        [SerializeField] private Button mBtnShortcut;

        private int mCurrencyId = -1;
        private bool mRegisteredEvents;

        public Button BtnShortcut => mBtnShortcut;
        #endregion

        //=============================================

        #region MonoBehaviour

        private void Start()
        {
            if (mBtnShortcut != null)
                mBtnShortcut.onClick.AddListener(OnBtnShortcut_Pressed);
        }

        private void OnEnable()
        {
            RegisterEvents();
            Refresh();
        }

        private void OnDisable()
        {
            UnregisterEvents();
        }

        #endregion

        //=============================================

        #region Public

        public void Init(int pId)
        {
            mCurrencyId = pId;
            mImgIcon.sprite = AssetsCollection.instance.GetCurrencyIcon(pId);

            Refresh();
            RegisterEvents();
        }

        #endregion

        //==============================================

        #region Private

        private void Refresh()
        {
            mTxtValue.text = GameData.Instance.CurrenciesGroup.GetValue(mCurrencyId).ToString();
        }

        private void RegisterEvents()
        {
            if (mRegisteredEvents || mCurrencyId == -1)
                return;

            mRegisteredEvents = true;

            EventDispatcher.AddListener<CurrencyChangedEvent>(OnCurrencyChanged);
        }

        private void UnregisterEvents()
        {
            if (!mRegisteredEvents)
                return;

            mRegisteredEvents = false;

            EventDispatcher.RemoveListener<CurrencyChangedEvent>(OnCurrencyChanged);
        }

        private void OnCurrencyChanged(CurrencyChangedEvent e)
        {
            if (e.id == mCurrencyId)
            {
                Refresh();
            }
        }

        private void OnBtnShortcut_Pressed()
        {
            MainPanel.instance.ShowShopPanel();
        }

        #endregion
    }
}