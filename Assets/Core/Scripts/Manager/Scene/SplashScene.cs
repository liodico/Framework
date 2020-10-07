using UnityEngine;
using FoodZombie.UI;
using System.Collections;

namespace FoodZombie
{
    public class SplashScene : MonoBehaviour
    {
        [SerializeField] private IntroPanel introPanel;

        private IEnumerator Start()
        {
            yield return null;
            GameInitializer.Instance.Init();

            yield return null;
            introPanel.Init();
        }
    }
}