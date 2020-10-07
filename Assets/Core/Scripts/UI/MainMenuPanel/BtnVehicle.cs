using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FoodZombie.UI;
using Utilities.Common;
using FoodZombie;
using Spine.Unity;
using System;

public class BtnVehicle : MonoBehaviour
{
    public SkeletonAnimation model;
    public GameObject imgCanUpgrade;

    // Start is called before the first frame update
    void Start()
    {
        var cost = GameData.Instance.VehicleData.GetCurrentCostUpgrade();
        imgCanUpgrade.SetActive(LogicAPI.CanShowUpgrade() 
                                && GameData.Instance.CurrenciesGroup.CanPay(IDs.CURRENCY_COIN, cost)
                                && TutorialsManager.Instance.IsCompleted(TutorialsGroup.UPGRADE_UNIT));

        EventDispatcher.AddListener<CurrencyChangedEvent>(OnCurrencyChanged);
        EventDispatcher.AddListener<TutorialFinishedEvent>(OnTutorialFinished);
        EventDispatcher.AddListener<VehicleUpgradeEvent>(OnVehicleUpgrade);

        model.Initialize(true);
        model.Skeleton.SetSkin(GameData.Instance.VehicleData.GetSkillName());
        model.Skeleton.SetToSetupPose();
    }

    private void OnDestroy()
    {
        EventDispatcher.RemoveListener<CurrencyChangedEvent>(OnCurrencyChanged);
        EventDispatcher.RemoveListener<TutorialFinishedEvent>(OnTutorialFinished);
        EventDispatcher.RemoveListener<VehicleUpgradeEvent>(OnVehicleUpgrade);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowVehiclePanel()
    {
        MainPanel.instance.ShowVehiclePanel();
    }

    private void OnCurrencyChanged(CurrencyChangedEvent e)
    {
        var cost = GameData.Instance.VehicleData.GetCurrentCostUpgrade();
        imgCanUpgrade.SetActive(LogicAPI.CanShowUpgrade()
                                && GameData.Instance.CurrenciesGroup.CanPay(IDs.CURRENCY_COIN, cost)
                                && TutorialsManager.Instance.IsCompleted(TutorialsGroup.UPGRADE_UNIT));
    }

    private void OnTutorialFinished(TutorialFinishedEvent e)
    {
        var cost = GameData.Instance.VehicleData.GetCurrentCostUpgrade();
        imgCanUpgrade.SetActive(LogicAPI.CanShowUpgrade()
                                && GameData.Instance.CurrenciesGroup.CanPay(IDs.CURRENCY_COIN, cost)
                                && TutorialsManager.Instance.IsCompleted(TutorialsGroup.UPGRADE_UNIT));
    }

    private void OnVehicleUpgrade(VehicleUpgradeEvent e)
    {
        StartCoroutine(IEOnVehicleUpgrade());
    }

    IEnumerator IEOnVehicleUpgrade() {
        yield return new WaitForSeconds(1.7f);

        model.Initialize(true);
        model.Skeleton.SetSkin(GameData.Instance.VehicleData.GetSkillName());
        model.Skeleton.SetToSetupPose();
    }
}
