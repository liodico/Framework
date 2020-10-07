using Spine;
using Spine.Unity.Modules;
using System.Collections;
using UnityEngine;
using Utilities.Common;
using Utilities.Components;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

namespace FoodZombie.UI
{
    public class IntroPanel : MyGamesBasePanel
    {
        [SerializeField] private LoadingPanel loadingPanel;
        [SerializeField] private SkeletonGraphicMultiObject mSkeleton;

        private bool isFirstMission;

        internal override void Init()
        {
            var trackEntry = mSkeleton.AnimationState.SetAnimation(0, "action", false);
            trackEntry.Complete += delegate (Spine.TrackEntry entry)
            {
                ShowLoadingPanel();
            };

            isFirstMission = GameData.Instance.MissionsGroup.IsFirstMission();
            if (isFirstMission)
            {
                SoundManager.Instance.PlayMusic(IDs.MUSIC_INTRO);
            }
            else
            {
                ShowLoadingPanel();
            }
        }

        private void Start()
        {
            
        }

        private void BtnSkip_Pressed()
        {
            ShowLoadingPanel();
        }

        private void ShowLoadingPanel()
        {
            SoundManager.Instance.StopMusic(true);
            PushPanelToTop(ref loadingPanel);

            if (isFirstMission)
            {
                //Khi mới mở game lên lần đầu, chạy thẳng map 1 luôn chứ ko vào qua world map nữa
                //loadingPanel.LoadGamePlayScreen();
                loadingPanel.LoadHomeScene();
            }
            else
            {
                loadingPanel.LoadHomeScene();
            }
        }
    }
}