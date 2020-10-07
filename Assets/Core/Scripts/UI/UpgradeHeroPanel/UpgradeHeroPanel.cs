using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Common;
using Utilities.Components;
using DG.Tweening;
using System;

namespace FoodZombie.UI
{
    public class UpgradeHeroPanel : MyGamesBasePanel
    {
        public TextMeshProUGUI txtName;
        public TextMeshProUGUI txtInfo;
        public TextMeshProUGUI txtFood;
        public Image imgRank;
        public Image imgRank2;
        public TextMeshProUGUI txtRank;
        public TextMeshProUGUI txtRank2;
        public Image imgLevelBarBG;
        public Image imgLevelBar;
        public TextMeshProUGUI txtLevelPercent; //viết ngược chính tả cho dễ nhìn
        public TextMeshProUGUI txtLevelCurrent;
        public TextMeshProUGUI txtLevelNext;
        public TextMeshProUGUI txtHPCurrent;
        public TextMeshProUGUI txtHPNext;
        public TextMeshProUGUI txtAtkCurrent;
        public TextMeshProUGUI txtAtkNext;
        public TextMeshProUGUI txtAtkSpeedCurrent;
        public TextMeshProUGUI txtAtkSpeedNext;

        public SkeletonGraphic fxModel;
        public SkeletonGraphic model;
        public SkeletonGraphic modelUpgrade;
        public SimpleTMPButton btnEvolve;
        public SimpleTMPButton btnUpgrade;
        public TextMeshProUGUI txtCostUpgrade;
        public AnimationCurve highlightFXAnim;

        public SimpleTMPButton btnBackTutorial;

        public bool initialized;

        private HeroData heroData;

        private Tweener tweener = null;

        private void Start()
        {
            btnEvolve.onClick.AddListener(BtnEvolve_Pressed);
            btnUpgrade.onClick.AddListener(BtnUpgrade_Pressed);

            btnBackTutorial.onClick.AddListener(BtnBack_Pressed);
        }

        public void Init(HeroData _heroData)
        {
            initialized = false;

            heroData = _heroData;

            Refresh();
        }

        private void Refresh()
        {
            if (tweener != null) tweener.Kill();
            model.material = AssetsCollection.instance.matDefaultSekelton;
            fxModel.AnimationState.Update(10f);

            var isMaxLevel = heroData.IsMaxLevel();
            btnEvolve.SetEnable(isMaxLevel);
            btnUpgrade.SetEnable(!isMaxLevel);

            var skeletonData = heroData.GetSkeletonData();
            if (skeletonData != null)
            {
                model.SetActive(true);
                model.skeletonDataAsset = skeletonData;
                model.Initialize(true);
                model.Skeleton.SetSkin(heroData.GetSkinName());
                model.Skeleton.SetToSetupPose();
                model.AnimationState.SetAnimation(0, "idle1", true);
            }
            else
            {
                model.SetActive(false);
            }

            // UnityEngine.Debug.Log(model.rectTransform.localPosition);
            model.rectTransform.localPosition = new Vector3(417.5f, -365, 0);
            modelUpgrade.gameObject.SetActive(false);
            if (isMaxLevel)
            {
                Config.LogEvent(TrackingConstants.EVENT_EVOLVE_SHOW);
                model.rectTransform.localPosition = new Vector3(217.5f, -365, 0);
                modelUpgrade.gameObject.SetActive(true);
                modelUpgrade.skeletonDataAsset = skeletonData;
                modelUpgrade.Initialize(true);
                modelUpgrade.Skeleton.SetSkin(heroData.Rank > 3 ? "skin4" : "skin" + (heroData.Rank + 1));
                modelUpgrade.Skeleton.SetToSetupPose();
                modelUpgrade.AnimationState.SetAnimation(0, "idle1", true);
            }

            txtName.text = heroData.GetName();
            txtInfo.text = "Rarity: " + heroData.GetRarityName();
            imgRank.sprite = heroData.GetRankIcon();
            imgRank2.sprite = heroData.GetRankIcon();
            txtRank.text = heroData.Rank + "";
            txtRank2.text = heroData.Rank + "";
            txtFood.text = "" + heroData.FoodRequired;

            var percentLevel = (heroData.Level - 1) * 1f / (heroData.GetLevelMax() - 1);
            var sizeBG = imgLevelBarBG.rectTransform.sizeDelta;
            imgLevelBar.rectTransform.sizeDelta = new Vector2(sizeBG.x * percentLevel, sizeBG.y);
            txtLevelPercent.text = (percentLevel * 100f).ToString("0") + "%";

            txtLevelCurrent.text = "Level " + heroData.Level;
            var HP = heroData.Hp;
            txtHPCurrent.text = HP.ToString("0");
            if (isMaxLevel)
            {
                txtHPNext.text = HP.ToString("0");
                txtLevelNext.text = "Max level";
            }
            else
            {
                txtHPNext.text = heroData.GetNextLevelHP().ToString("0");
                txtLevelNext.text = "Level " + (heroData.Level + 1);
            }
            
            var atk = heroData.Atk;
            float nextLevelAtk = atk;
            if (!isMaxLevel) nextLevelAtk = heroData.GetNextLevelAtk();
            txtAtkCurrent.text = atk.ToString("0");
            txtAtkNext.text = nextLevelAtk.ToString("0");
            txtAtkSpeedCurrent.text = heroData.AtkSpeed.ToString();
            txtAtkSpeedNext.text = heroData.AtkSpeed.ToString();

            if (isMaxLevel) txtCostUpgrade.text = "Max level";
            else txtCostUpgrade.text = heroData.CostUpgrade > 0 ? heroData.CostUpgrade + "" : "Free";

            initialized = true;
        }

        private void BtnEvolve_Pressed()
        {
            Config.LogEvent(TrackingConstants.EVENT_EVOLVE_REQUEST);

            if (!AdsHelper.__IsVideoRewardedAdReady("unit_evolve"))
            {

                MainPanel.instance.ShowWarningTooltip(
                    btnEvolve.rectTransform,
                    new UITooltips.Message("Ads is not available yet!", new Vector2(600, 160))
                );

                return;
            }

            AdsHelper.__ShowVideoRewardedAd(OnRewardedAdCompleted, "unit_evolve");
        }

        private void OnRewardedAdCompleted(bool isCompleted)
        {
            if (isCompleted)
            {
                Config.LogEvent(TrackingConstants.EVENT_EVOLVE_REWARDED);

                var isSuccess = heroData.RankUp();
                if (isSuccess) UpgradeEffect();
            }
        }

        private void BtnUpgrade_Pressed()
        {
            if (GameData.Instance.CurrenciesGroup.CanPay(IDs.CURRENCY_COIN, heroData.CostUpgrade))
            {
                GameData.Instance.CurrenciesGroup.Pay(IDs.CURRENCY_COIN, heroData.CostUpgrade);
                var isSuccess = heroData.LevelUp();
                if (isSuccess) UpgradeEffect();
            }
            else
            {
                MainPanel.instance.ShowWarningTooltip(
                    btnUpgrade.rectTransform,
                    new UITooltips.Message("Not enough gold", new Vector2(600, 160))
                );
            }
        }

        private void UpgradeEffect()
        {
            Refresh();

            if (heroData.Level == 1) fxModel.AnimationState.SetAnimation(0, "evolveUnit1", false);
            else fxModel.AnimationState.SetAnimation(0, "upgradeUnit1", false);
            var anim = model.SkeletonData.FindAnimation("victory");
            if (anim == null) anim = model.SkeletonData.FindAnimation("victory1");
            if (anim == null) anim = model.SkeletonData.FindAnimation("idle1");
            model.AnimationState.SetAnimation(0, anim, false);
            model.AnimationState.AddAnimation(0, "idle1", true, 0f);

            //model.material = AssetsCollection.instance.matSkeletonFill;
            //float lerp = 0;
            //tweener = DOTween.To(tweenVal => lerp = tweenVal, 0f, 1f, anim.Duration)
            //    .OnUpdate(() =>
            //    {
            //        var val = highlightFXAnim.Evaluate(lerp);
            //        model.material.SetFloat("_FillPhase", val);
            //    })
            //    .OnComplete(() =>
            //    {
            //        model.material = AssetsCollection.instance.matDefaultSekelton;
            //    });
        }
    }
}