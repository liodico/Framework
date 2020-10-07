using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Components;
using Utilities.Service.RFirebase;
using Spine.Unity;
using TMPro;
using Utilities.Common;
using System;

namespace FoodZombie.UI
{
    public class SafePopup : MyGamesBasePanel
    {
        //[SerializeField] private PriceTMPButton mBtnOpen;
        //[SerializeField] private ManagerSpine mSpineModel;
        //[SerializeField] private Image mImgTopPrize;
        public TextMeshProUGUI[] txtTotalGolds;
        public TextMeshProUGUI[] txtTimes;
        public SimpleTMPButton[] btnClaims;
        //public SimpleTMPButton BtnOpen => mBtnOpen;
        //NormalChestData chestData => GameData.Instance.NormalChestData;

        //bool mDisable;

        void Start()
        {
            btnClaims[0].onClick.AddListener(BtnClaim1_Pressed);
            btnClaims[1].onClick.AddListener(BtnClaim2_Pressed);
            btnClaims[2].onClick.AddListener(BtnClaim3_Pressed);
            EventDispatcher.AddListener<SafeChangeValueEvent>(OnSafeChangeValue);
        }

        private void OnDestroy()
        {
            EventDispatcher.RemoveListener<SafeChangeValueEvent>(OnSafeChangeValue);
        }

        private void Update()
        {
            var safeData = GameData.Instance.SafeData;
            if (!safeData.CanClaimSafe1) txtTimes[0].text = safeData.GetTimeSafe1();
            if (!safeData.CanClaimSafe2) txtTimes[1].text = safeData.GetTimeSafe2();
            if (!safeData.CanClaimSafe3) txtTimes[2].text = safeData.GetTimeSafe3();
        }

        private void BtnClaim1_Pressed()
        {
            var gold = GameData.Instance.SafeData.ClaimSafe1();
            GameData.Instance.CurrenciesGroup.Add(IDs.CURRENCY_COIN, gold);
        }

        private void BtnClaim2_Pressed()
        {
            var gold = GameData.Instance.SafeData.ClaimSafe2();
            GameData.Instance.CurrenciesGroup.Add(IDs.CURRENCY_COIN, gold);
        }

        private void BtnClaim3_Pressed()
        {
            var gold = GameData.Instance.SafeData.ClaimSafe3();
            GameData.Instance.CurrenciesGroup.Add(IDs.CURRENCY_COIN, gold);
        }

        internal override void Init()
        {
            base.Init();

            var safeData = GameData.Instance.SafeData;
            safeData.CheckStartTime();
            txtTotalGolds[0].text = safeData.GetSafe1() + "";
            txtTotalGolds[1].text = safeData.GetSafe2() + "";
            txtTotalGolds[2].text = safeData.GetSafe3() + "";

            if(safeData.CanClaimSafe1)
            {
                txtTimes[0].SetActive(false);
                btnClaims[0].SetActive(true);
            }
            else
            {
                txtTimes[0].SetActive(true);
                btnClaims[0].SetActive(false);
            }

            if (safeData.CanClaimSafe2)
            {
                txtTimes[1].SetActive(false);
                btnClaims[1].SetActive(true);
            }
            else
            {
                txtTimes[1].SetActive(true);
                btnClaims[1].SetActive(false);
            }

            if (safeData.CanClaimSafe3)
            {
                txtTimes[2].SetActive(false);
                btnClaims[2].SetActive(true);
            }
            else
            {
                txtTimes[2].SetActive(true);
                btnClaims[2].SetActive(false);
            }
        }

        private void OnSafeChangeValue(SafeChangeValueEvent e)
        {
            if(e.value)
            {
                txtTimes[e.id].SetActive(false);
                btnClaims[e.id].SetActive(true);
            }
            else
            {
                txtTimes[e.id].SetActive(true);
                btnClaims[e.id].SetActive(false);
            }
        }
    }
}