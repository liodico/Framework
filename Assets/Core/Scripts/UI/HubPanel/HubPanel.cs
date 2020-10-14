
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
        public CurrencyView coinView;
        public SimpleTMPButton btnPause;
        public HubInfoHero[] hubInfoHeroes;
        
        private bool initialized;

        private GameData GameData => GameData.Instance;

        private void Start()
        {
            btnPause.onClick.AddListener(BtnPause_Pressed);
        }

        internal override void Init()
        {
            coinView.Init(IDs.CURRENCY_COIN);

            initialized = true;
        }

        private void BtnPause_Pressed()
        {
            MainGamePanel.instance.ShowPausePanel();
        }

        public void LinkHubInfoHero()
        {
            var heroes = GameplayController.Instance.GetHeroes();
            var lenght = heroes.Count;
            for (int i = 0; i < lenght; i++)
            {
                var hero = heroes[i];
                var hubInfoHero = hubInfoHeroes[i];
                hero.LinkHubInfoHero(hubInfoHero);
                hubInfoHero.SetActive(true);
            }
        }
    }
}