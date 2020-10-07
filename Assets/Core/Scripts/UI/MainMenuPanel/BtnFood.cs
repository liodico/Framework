using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FoodZombie.UI;
using Utilities.Common;
using FoodZombie;
using Spine.Unity;

public class BtnFood : MonoBehaviour
{
    public SkeletonAnimation model;
    public GameObject imgCanUpgrade;

    // Start is called before the first frame update
    void Start()
    {
        var cost = GameData.Instance.FoodGuyData.GetCurrentCostUpgrade();
        if(imgCanUpgrade != null) imgCanUpgrade.SetActive(LogicAPI.CanShowUpgrade()
                                && GameData.Instance.CurrenciesGroup.CanPay(IDs.CURRENCY_COIN, cost)
                                && TutorialsManager.Instance.IsCompleted(TutorialsGroup.UPGRADE_UNIT));

        EventDispatcher.AddListener<CurrencyChangedEvent>(OnCurrencyChanged);
        EventDispatcher.AddListener<TutorialFinishedEvent>(OnTutorialFinished);
        EventDispatcher.AddListener<FoodGuyUpgradeEvent>(OnFoodGuyUpgrade);

        model.Initialize(true);
        model.Skeleton.SetSkin(GameData.Instance.FoodGuyData.GetSkillName());
        model.Skeleton.SetToSetupPose();
    }

    private void OnDestroy()
    {
        EventDispatcher.RemoveListener<CurrencyChangedEvent>(OnCurrencyChanged);
        EventDispatcher.RemoveListener<TutorialFinishedEvent>(OnTutorialFinished);
        EventDispatcher.RemoveListener<FoodGuyUpgradeEvent>(OnFoodGuyUpgrade);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowFoodPanel()
    {
        MainPanel.instance.ShowFoodPanel();
    }

    private void OnCurrencyChanged(CurrencyChangedEvent e)
    {
        var cost = GameData.Instance.FoodGuyData.GetCurrentCostUpgrade();
        if (imgCanUpgrade != null) imgCanUpgrade.SetActive(LogicAPI.CanShowUpgrade()
                                && GameData.Instance.CurrenciesGroup.CanPay(IDs.CURRENCY_COIN, cost)
                                && TutorialsManager.Instance.IsCompleted(TutorialsGroup.UPGRADE_UNIT));
    }

    private void OnTutorialFinished(TutorialFinishedEvent e)
    {
        var cost = GameData.Instance.FoodGuyData.GetCurrentCostUpgrade();
        if (imgCanUpgrade != null) imgCanUpgrade.SetActive(LogicAPI.CanShowUpgrade()
                                && GameData.Instance.CurrenciesGroup.CanPay(IDs.CURRENCY_COIN, cost)
                                && TutorialsManager.Instance.IsCompleted(TutorialsGroup.UPGRADE_UNIT));
    }
    
    private void OnFoodGuyUpgrade(FoodGuyUpgradeEvent e)
    {
        StartCoroutine(IEOnFoodGuyUpgrade());
    }

    IEnumerator IEOnFoodGuyUpgrade()
    {
        yield return new WaitForSeconds(1.7f);

        model.Initialize(true);
        model.Skeleton.SetSkin(GameData.Instance.FoodGuyData.GetSkillName());
        model.Skeleton.SetToSetupPose();
    }
}
