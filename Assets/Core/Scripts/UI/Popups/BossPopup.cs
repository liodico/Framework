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
    public class BossPopup : MyGamesBasePanel
    {
        [SerializeField] private TextMeshProUGUI mInfo;
        [SerializeField] private SimpleTMPButton mBtnPlay;
        //[SerializeField] private ManagerSpine mSpineModel;
        //[SerializeField] private Image mImgTopPrize;
        //public SimpleTMPButton BtnOpen => mBtnOpen;
        //NormalChestData chestData => GameData.Instance.NormalChestData;

        //bool mDisable;

        void Start()
        {
            mBtnPlay.onClick.AddListener(OnBtnPlay_Pressed);
        }

        internal override void Init()
        {
            base.Init();

            var currentBossMissions = GameData.Instance.MissionsGroup.CurrentBossMissions;
            if (currentBossMissions.Count > 0)
            {
                mInfo.text = "<color=#591900>ASSEMBLE YOUR ARMY\nDefeat the <color=#E73720>ELITE BOSS <color=#591900>to get <color=#5B8E12>HUGE <color=#591900>rewards!";
                mBtnPlay.SetEnable(true);
            }
            else
            {
                mInfo.text = "<color=#591900>ASSEMBLE YOUR ARMY\nUnlock at Mission <color=#E73720>" + GameData.Instance.MissionsGroup.GetNextMissionHasBoss();
                mBtnPlay.SetEnable(false);
            }
        }

        private void OnBtnPlay_Pressed()
        {
            Config.typeMenuInGameMode = Config.TYPE_MODE_BOSS;
            MainPanel.instance.LoadGamePlayScreen();
        }
    }
}