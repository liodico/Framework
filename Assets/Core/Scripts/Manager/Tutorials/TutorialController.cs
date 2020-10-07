using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FoodZombie.UI
{
    public abstract class TutorialController
    {
        public int id { get; protected set; }
        public bool isToolTips { get; protected set; }
        public bool ended { get; protected set; }
        public MainPanel MainPanel => MainPanel.instance;
        public GameData GameData => GameData.Instance;
        public TutorialsManager TutorialsManager => TutorialsManager.Instance;
        public TutorialController(int pId, bool pIsToolTips)
        {
            id = pId;
            isToolTips = pIsToolTips;
        }
        public virtual void Start()
        {
            ended = false;
            MainPanel.instance.onAnyChildHide += OnAnyChildHide;
            MainPanel.instance.onAnyChildShow += OnAnyChildShow;
        }
        public virtual void End(bool pFinished)
        {
            if (pFinished)
                GameData.TutorialsGroup.Finish(id);
            ended = true;
            MainPanel.instance.onAnyChildHide -= OnAnyChildHide;
            MainPanel.instance.onAnyChildShow -= OnAnyChildShow;
        }
        public abstract IEnumerator IEProcess();
        /// <summary>
        /// Used only for ToolTip Tutorial, we can not Pause Fixed Tutorial
        /// </summary>
        public abstract void Pause();
        /// <summary>
        /// Used only for ToolTip Tutorial, we can not Pause Fixed Tutorial
        /// </summary>
        public abstract void Resume();
        protected abstract void OnAnyChildHide(MyGamesBasePanel obj);
        protected abstract void OnAnyChildShow(MyGamesBasePanel obj);
    }
}