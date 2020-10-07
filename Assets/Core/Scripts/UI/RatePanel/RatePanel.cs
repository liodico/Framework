using UnityEngine;
using Utilities.Common;
using Utilities.Components;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

namespace FoodZombie.UI
{
    public class RatePanel : MyGamesBasePanel
    {
        public static bool isShowed = false;
        [SerializeField] private SimpleTMPButton btnRate;

        private void Start()
        {
            btnRate.onClick.AddListener(BtnRate_Pressed);

            isShowed = true;
        }

        protected override void BeforeHiding()
        {
            base.BeforeHiding();
        }

        private void BtnRate_Pressed()
        {
            GameData.Instance.GameConfigGroup.Rate();

#if UNITY_ANDROID
            Application.OpenURL("market://details?id=" + Application.identifier);
#elif UNITY_IOS
            Device.RequestStoreReview();
#endif

            Back();
        }
    }
}