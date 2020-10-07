using FoodZombie;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utilities.Common;
using FoodZombie.UI;

public class EditTeamHeroSlotView : MonoBehaviour
{
    public Image imgHero;
    public GameObject imgCheck;
    public GameObject imgNoti;
    //public GameObject imgEmpty;
    public GameObject imgLock;
    private HeroData heroData;
    private UnityAction<HeroData> showInfoHero;
    public Toggle togViewDetail;

    // Start is called before the first frame update
    void Start()
    {
        togViewDetail.onValueChanged.AddListener(OnTogViewDetail_Pressed);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(HeroData _heroData, UnityAction<HeroData> _showInfoHero)
    {
        heroData = _heroData;
        showInfoHero = _showInfoHero;

        imgHero.sprite = heroData.GetIcon();
        if (heroData.Unlocked)
        {
            imgHero.SetActive(true);
            if (heroData.IsEquipped())
            {
                imgCheck.SetActive(true);
            }
            else
            {
                imgCheck.SetActive(false);
            }
            imgLock.SetActive(false);
            togViewDetail.interactable = true;
            imgNoti.SetActive(!heroData.Viewed);
        }
        else
        {
            imgHero.SetActive(false);
            imgLock.SetActive(true);
            togViewDetail.interactable = false;
            imgNoti.SetActive(!heroData.Viewed);
        }
    }

    public void Reset()
    {
        imgHero.SetActive(false);
        //imgEmpty.SetActive(true);
        imgCheck.SetActive(false);
        imgLock.SetActive(true);

        togViewDetail.isOn = false;
        togViewDetail.interactable = false;
        showInfoHero = null;
        heroData = null;
    }

    private void OnTogViewDetail_Pressed(bool value)
    {
        if (value && showInfoHero != null)
        {
            //characterData.Buzz(false);
            imgNoti.SetActive(false);
            showInfoHero(heroData);
            MainPanel.instance.MainMenuPanel.CheckNewHeroBuzz();
        }
    }
}
