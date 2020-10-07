using Spine;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Common;
using Utilities.Components;
using Utilities.Service.RFirebase;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

namespace FoodZombie.UI
{
    public class RescueHeroPanel : MyGamesBasePanel
    {
        public SkeletonGraphic model;
        public TextMeshProUGUI txtRarity;
        public Image imgPerk;
        public SimpleTMPButton btnRescue;
        public GameObject imgAds;

        public bool initialized;

        private HeroData hero;

        private void Start()
        {
            btnRescue.onClick.AddListener(OnBtnRescue_Pressed);
        }

        public void Init(int heroId)
        {
            imgAds.SetActive(GameData.Instance.HeroesGroup.CountClaimHero != 0);
            btnBack.SetActive(GameData.Instance.HeroesGroup.CountClaimHero != 0);

            hero = GameData.Instance.HeroesGroup.GetHeroData(heroId);
            txtRarity.text = "Rarity: " + hero.GetRarityName();

            var skeletonData = hero.GetSkeletonData();
            if (skeletonData != null)
            {
                model.SetActive(true);
                model.skeletonDataAsset = skeletonData;
                model.Initialize(true);
                var skin = model.Skeleton.Data.FindSkin("skin1");
                if (skin == null) skin = model.Skeleton.Data.FindSkin("1");
                model.Skeleton.SetSkin(skin);

                var anim = model.SkeletonData.FindAnimation("victory");
                if (anim == null) anim = model.SkeletonData.FindAnimation("victory1");
                if (anim == null) anim = model.SkeletonData.FindAnimation("idle1");
                model.AnimationState.SetAnimation(0, anim, true);
            }
            else
            {
                model.SetActive(false);
            }

            //var perks = hero.GetPerks();
            //UnitPerkData perk = null;
            //if (perks != null && perks.Count > 0)
            //{
            //    foreach (var item in perks)
            //    {
            //        var iconName = item.baseData.iconName;
            //        if (iconName != null && !iconName.Equals(""))
            //        {
            //            perk = item;
            //        }
            //    }
            //}

            //if (perk != null)
            //{
            //    imgPerk.transform.parent.SetActive(true);
            //    var icon = perk.GetIcon();
            //    if (icon is null) imgPerk.transform.parent.SetActive(false);
            //    else
            //    {
            //        imgPerk.transform.parent.gameObject.SetActive(true);
            //        imgPerk.sprite = icon;
            //    }
            //}
            //else
            //{
            //    imgPerk.transform.parent.SetActive(false);
            //}

            //free lần đầu
            if (GameData.Instance.HeroesGroup.CountClaimHero != 0)
            {
                Config.LogEvent(TrackingConstants.EVENT_CLAIM_HERO_SHOW);
            }

            initialized = true;
        }

        private void OnBtnRescue_Pressed()
        {
            //free lần đầu
            if (GameData.Instance.HeroesGroup.CountClaimHero == 0)
            {
                Back();
                ClaimHero();
            }
            else
            {
                Config.LogEvent(TrackingConstants.EVENT_CLAIM_HERO_REQUEST);
                //xem ads
                if (!AdsHelper.__IsVideoRewardedAdReady("claim_hero"))
                {
                    MainPanel.instance.ShowWarningTooltip(
                        btnRescue.rectTransform,
                        new UITooltips.Message("Ads is not available yet!", new Vector2(600, 160))
                    );

                    return;
                }

                AdsHelper.__ShowVideoRewardedAd(OnRewardedAdCompleted, "claim_hero");
            }
        }

        private void OnRewardedAdCompleted(bool isCompleted)
        {
            if (isCompleted)
            {
                Config.LogEvent(TrackingConstants.EVENT_CLAIM_HERO_REWARDED);
                Back();
                ClaimHero();
            }
            else
            {
                Config.LogEvent(TrackingConstants.EVENT_CLAIM_HERO_FAIL);
            }
        }

        private void ClaimHero()
        {
            RewardInfo rewardInfo = new RewardInfo(IDs.REWARD_TYPE_UNLOCK_CHARACTER, hero.Id, 1);
            LogicAPI.ClaimReward(rewardInfo);
            GameData.Instance.HeroesGroup.ClaimHero();
        }

        internal override void Back()
        {
            base.Back();
            GameData.Instance.HeroesGroup.ResetClaimHero();
        }
    }
}