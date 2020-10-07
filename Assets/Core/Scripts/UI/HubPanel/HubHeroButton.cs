using FoodZombie;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FoodZombie.UI;
using Utilities.Components;

public class HubHeroButton : MonoBehaviour
{
    public Image imgIcon;
    public Image imgRank;
    public Image imgCountDown;
    public TextMeshProUGUI txtRank;
    public TextMeshProUGUI txtFoodRequired;
    public SimpleTMPButton btnSpawn;

    private HubPanel hubPanel;
    private HeroData heroData;

    private float timeCountDown = 0f;
    private float timeCountDownMax = 1f;

    // Start is called before the first frame update
    void Start()
    {
        btnSpawn.onClick.AddListener(BtnSpawn_Pressed);
    }

    void FixedUpdate()
    {
        if(GameplayController.Instance.currentFood >= heroData.FoodRequired && timeCountDown <= 0f)
        {
            imgIcon.rectTransform.anchoredPosition = new Vector2(0f, 20f);
        }
        else
        {
            imgIcon.rectTransform.anchoredPosition = new Vector2(0f, 0f);
        }

        timeCountDown -= Time.fixedDeltaTime;
        if (timeCountDown < 0f) timeCountDown = 0f;
        imgCountDown.fillAmount = timeCountDown / timeCountDownMax;
    }

    public void Init(HubPanel _hubPanel, HeroData _heroData)
    {
        hubPanel = _hubPanel;
        heroData = _heroData;

        imgIcon.sprite = heroData.GetIcon();
        imgRank.sprite = heroData.GetRankIcon();
        imgRank.SetNativeSize();
        txtRank.text = heroData.Rank + "";
        txtFoodRequired.text = heroData.FoodRequired + "";

        timeCountDownMax = heroData.Prepare;
        timeCountDown = timeCountDownMax;
    }

    private void BtnSpawn_Pressed()
    {
        if (timeCountDown <= 0f)
        {
            var spawned = GameplayController.Instance.SpawnHero(heroData);
            if (spawned)
            {
                timeCountDownMax = heroData.Cooldown;
                timeCountDown = timeCountDownMax;
            }
        }
    }
}
