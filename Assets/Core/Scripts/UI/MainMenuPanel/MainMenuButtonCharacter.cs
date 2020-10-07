using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Utilities.Components;
using System;
using FoodZombie;
using UnityEngine.Events;
using UnityEngine.UI;
using Utilities.Common;
using HedgehogTeam.EasyTouch;
using UnityEngine.EventSystems;
using FoodZombie.UI;

public class MainMenuButtonCharacter : MonoBehaviour
{
    public SkeletonAnimation model;
    public GameObject imgReplace;
    public HeroData heroData;
    public Transform objectTap;
    public SpriteRenderer objectTapRenderer;
    public GameObject imgCanUpgrade;
    private UnityAction<HeroData> showHero;

    private bool inEditTeam = false;

    // Start is called before the first frame update
    void Start()
    {
        EventDispatcher.AddListener<CurrencyChangedEvent>(OnCurrencyChanged);
        EventDispatcher.AddListener<TutorialFinishedEvent>(OnTutorialFinished);
        EventDispatcher.AddListener<HeroUpgradeEvent>(OnHeroUpgradeEvent);
    }

    private void OnDestroy()
    {
        EventDispatcher.RemoveListener<CurrencyChangedEvent>(OnCurrencyChanged);
        EventDispatcher.RemoveListener<TutorialFinishedEvent>(OnTutorialFinished);
        EventDispatcher.RemoveListener<HeroUpgradeEvent>(OnHeroUpgradeEvent);
    }

    private void OnCurrencyChanged(CurrencyChangedEvent e)
    {
        if (heroData != null)
        {
            var cost = heroData.CostUpgrade;
            imgCanUpgrade.SetActive(LogicAPI.CanShowUpgrade()
                                     && GameData.Instance.CurrenciesGroup.CanPay(IDs.CURRENCY_COIN, cost)
                                    /*&& TutorialsManager.Instance.IsCompleted(TutorialsGroup.UPGRADE_UNIT)*/);
        }
        else
        {
            imgCanUpgrade.SetActive(false);
        }
    }

    public void Init(HeroData _heroData, UnityAction<HeroData> _showHero)
    {
        heroData = _heroData;
        showHero = _showHero;
        model.skeletonDataAsset = heroData.GetSkeletonData();
        model.Initialize(true);
        model.Skeleton.SetSkin(heroData.GetSkinName());
        model.Skeleton.SetToSetupPose();
        var anim = model.Skeleton.Data.FindAnimation("victory");
        if (anim == null) anim = model.Skeleton.Data.FindAnimation("victory1");
        if (anim == null) anim = model.Skeleton.Data.FindAnimation("idle1");
        model.AnimationState.SetAnimation(0, anim, true);

        var cost = heroData.CostUpgrade;
        imgCanUpgrade.SetActive(LogicAPI.CanShowUpgrade()
                                && GameData.Instance.CurrenciesGroup.CanPay(IDs.CURRENCY_COIN, cost)
                                /*&& TutorialsManager.Instance.IsCompleted(TutorialsGroup.UPGRADE_UNIT)*/);
    }

    // Update is called once per frame
    void Update()
    {
        #if UNITY_EDITOR
        if (!inEditTeam)
        {
            if (EventSystem.current == null || EventSystem.current.IsPointerOverGameObject(-1))    // is the touch on the GUI
            {
                // GUI Action
                return;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == objectTap)
                {
                    showHero(heroData);
                }
            }
        }

        #else
        if (!inEditTeam)
        {
            if (EventSystem.current == null || EventSystem.current.IsPointerOverGameObject(0))    // is the touch on the GUI
            {
                // GUI Action
                return;
            }
        }

        if (Input.touchCount > 0)
        {
            foreach (var item in Input.touches)
            {
                if (item.phase == TouchPhase.Began)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.transform == objectTap)
                        {
                            showHero(heroData);
                        }
                    }
                }
            }
        }
        #endif
    }

    public void ToNormal()
    {
        inEditTeam = false;
        model.GetComponent<MeshRenderer>().sortingOrder = 0;
        imgReplace.SetActive(false);
        //btnHero.SetEnable(true);
        //model.skeleton.SetColor(Color.white);
    }

    public void ToEditTeam()
    {
        inEditTeam = true;
        model.GetComponent<MeshRenderer>().sortingOrder = 1;
        imgReplace.SetActive(true);
        //btnHero.SetEnable(true);
        //model.skeleton.SetColor(Color.white);
    }

    private void OnTutorialFinished(TutorialFinishedEvent e)
    {
        //if (characterData != null)
        //{
        //    var cost = characterData.GetCostUpgrade();
        //    imgCanUpgrade.SetActive(LogicAPI.CanShowUpgrade()
        //                            && GameData.Instance.CurrenciesGroup.CanPay(IDs.CURRENCY_COIN, cost)
        //                            && TutorialsManager.Instance.IsCompleted(TutorialsGroup.UPGRADE_UNIT));
        //}
        //else
        //{
        //    imgCanUpgrade.SetActive(false);
        //}
    }

    private void OnHeroUpgradeEvent(HeroUpgradeEvent e)
    {
        model.Skeleton.SetSkin(heroData.GetSkinName());
        model.Skeleton.SetToSetupPose();
    }
}
