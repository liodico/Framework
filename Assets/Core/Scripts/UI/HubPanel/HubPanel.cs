
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;
using Utilities.Common;
using Utilities.Components;
using Utilities.Inspector;
using Utilities.Service.RFirebase;

using DG.Tweening;
using Spine.Unity;
using Utilities.Service;

namespace FoodZombie.UI
{
    public class HubPanel : MyGamesBasePanel
    {
        public TextMeshProUGUI txtFood;
        public TextMeshProUGUI txtRage;
        public CurrencyView coinView;
        public SimpleTMPButton btnPause;

        [SerializeField, Tooltip("Buildin Pool")] public List<HubHeroButton> hubHeroButtonsPool;
        public Transform hubHeroButtonsGrid;

        private bool initialized;

        private GameData GameData => GameData.Instance;

        private void Start()
        {
            btnPause.onClick.AddListener(BtnPause_Pressed);
        }

        internal override void Init()
        {
            hubHeroButtonsPool.Free();

            coinView.Init(IDs.CURRENCY_COIN);

            initialized = true;
        }

        private void BtnPause_Pressed()
        {
            MainGamePanel.instance.ShowPausePanel();
        }

        public void ShowFood(float currentFood, float MAX_FOOD)
        {
            txtFood.text = currentFood.ToString("0") + "/" + MAX_FOOD.ToString("0");
        }

        public void ShowRage(float currentRage, float MAX_RAGE)
        {
            txtRage.text = currentRage.ToString("0") + "/" + MAX_RAGE.ToString("0");
        }

        public void ShowHubHeroButtons(List<HeroData> heroDatas)
        {
            foreach (var item in heroDatas)
            {
                var hubHeroButton = hubHeroButtonsPool.Obtain(hubHeroButtonsGrid);
                hubHeroButton.Init(this, item);
                hubHeroButton.SetActive(true);
            }
        }
    }
}