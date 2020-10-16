
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
using HedgehogTeam.EasyTouch;
using Spine.Unity;
using Utilities.Service;

namespace FoodZombie.UI
{
    public class HubPanel : MyGamesBasePanel
    {
        public CurrencyView coinView;
        public SimpleTMPButton btnPause;
        public SimpleTMPButton btnAutoPlay;
        public ButtonItemGameplay btnItemBarrier;
        public TextMeshProUGUI txtAutoPlay;
        public HubInfoHero[] hubInfoHeroes;
        
        private bool initialized;

        private GameData GameData => GameData.Instance;

        private void Start()
        {
            btnPause.onClick.AddListener(BtnPause_Pressed);
            btnAutoPlay.onClick.AddListener(BtnAutoPlay_Pressed);
            btnItemBarrier.onDragStart.AddListener(BtnItemBarrier_OnDragStart);
            btnItemBarrier.onDrag.AddListener(BtnItemBarrier_OnDrag);
            btnItemBarrier.onDragEnd.AddListener(BtnItemBarrier_OnDragEnd);
        }

        internal override void Init()
        {
            coinView.Init(IDs.CURRENCY_COIN);
            ShowTextAutoPlay();
            
            initialized = true;
        }

        private void BtnPause_Pressed()
        {
            MainGamePanel.instance.ShowPausePanel();
        }
        
        private void BtnAutoPlay_Pressed()
        {
            GameplayController.Instance.ChangeAutoPlay();
            ShowTextAutoPlay();
        }

        private void BtnItemBarrier_OnDragStart(Gesture gesture)
        {
            // GameplayController.Instance.AddBarrier();
        }
        
        private void BtnItemBarrier_OnDrag(Gesture gesture)
        {
            // GameplayController.Instance.AddBarrier();
        }
        
        private void BtnItemBarrier_OnDragEnd(Gesture gesture)
        {
            GameplayController.Instance.AddBarrier(gesture.swipeVector / 100f);
        }

        private void ShowTextAutoPlay()
        {
            if (GameplayController.Instance.autoPlay) txtAutoPlay.text = "Control";
            else txtAutoPlay.text = "Auto Play";
        }

        public void LinkHubInfoHero(HeroExControl heroExControl)
        {
            for (int i = 0; i < hubInfoHeroes.Length; i++)
            {
                var hubInfoHero = hubInfoHeroes[i];
                if(hubInfoHero.gameObject.activeSelf) continue;
                else
                {
                    heroExControl.LinkHubInfoHero(hubInfoHero);
                    hubInfoHero.SetActive(true);
                    break;
                }
            }
        }
    }
}