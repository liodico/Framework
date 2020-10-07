using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Components;
using Spine.Unity;
using TMPro;
using Utilities.Common;
using DG.Tweening;

namespace FoodZombie.UI
{
    public class EditTeamPanel : MyGamesBasePanel
    {
        public TextMeshProUGUI txtName;
        public TextMeshProUGUI txtInfo;
        public Image imgRank;
        public TextMeshProUGUI txtRank;
        public TextMeshProUGUI txtFood;
        //public Image imgLevelBarBG;
        //public Image imgLevelBar;
        public SkeletonGraphic model;
        public Image imgPerk;
        //public TextMeshProUGUI txtPerkName;
        //public TextMeshProUGUI txtPerkDescription;
        [SerializeField, Tooltip("Buildin Pool")] List<EditTeamHeroSlotView> heroSlotViewsPool;
        public Transform heroSlotViewsGrid;

        public SimpleTMPButton btnJoin;
        public ToggleGroup toggleGroup;

        public ScrollRect scroll;

        private HeroData heroChoice;


        void Start()
        {
            btnJoin.onClick.AddListener(OnBtnJoin_Pressed);
        }

        private void OnEnable()
        {
            Refresh();
        }

        private void Refresh()
        {
            heroSlotViewsPool.Free();

            var heroDatas = GameData.Instance.HeroesGroup.GetAllHeroDatas();
            var length = heroDatas.Count;
            //hiện những hero đã unlock lên trước
            for (int i = 0; i < length - 1; i++)
            {
                for (int j = i + 1; j < length; j++)
                {
                    if (!heroDatas[i].Unlocked && heroDatas[j].Unlocked)
                    {
                        HeroData temp = heroDatas[i];
                        heroDatas[i] = heroDatas[j];
                        heroDatas[j] = temp;
                    }
                }
            }
            //hiện những hero equip lên trước
            var newHeroDatas = new List<HeroData>();
            var equipIds = GameData.Instance.HeroesGroup.CharactersEquipped;
            for (int i = 0; i < equipIds.Count; i++)
            {
                var id = equipIds[i];
                for (int j = 0; j < length; j++)
                {
                    var hero = heroDatas[j];
                    if (id == hero.Id)
                    {
                        newHeroDatas.Add(hero);
                    }
                }
            }
            for (int i = 0; i < length; i++)
            {
                var hero = heroDatas[i];
                if (!newHeroDatas.Contains(hero))
                {
                    newHeroDatas.Add(hero);
                }
            }

            //show lên
            int indexChoice = 0;
            for (int i = 0; i < length; i++)
            {
                var hero = newHeroDatas[i] as HeroData;
                var heroSlotView = heroSlotViewsPool.Obtain(heroSlotViewsGrid);
                heroSlotView.Reset();
                heroSlotView.Init(hero, ShowInfoHero);
                heroSlotView.SetActive(true);
                if ((hero == null && i == 0) || hero == heroChoice)
                {
                    indexChoice = i;
                    heroSlotView.togViewDetail.isOn = true;
                    ShowInfoHero(hero);
                }
            }

            //focus scroll
            scroll.DOVerticalNormalizedPos(1f - (float)(indexChoice / 4) / (float)(length / 4), 0f);
        }

        private void ShowInfoHero(HeroData _heroChoice)
        {
            heroChoice = _heroChoice;
            btnJoin.SetActive(!heroChoice.IsEquipped());

            txtName.text = heroChoice.GetName();
            txtInfo.text = "Rarity: " + heroChoice.GetRarityName();
            imgRank.sprite = heroChoice.GetRankIcon();
            txtRank.text = "" + heroChoice.Rank;
            txtFood.text = "" + heroChoice.FoodRequired;

            var skeletonData = heroChoice.GetSkeletonData();
            if (skeletonData != null)
            {
                model.SetActive(true);
                model.skeletonDataAsset = skeletonData;
                model.Initialize(true);
                model.Skeleton.SetSkin(heroChoice.GetSkinName());
                model.Skeleton.SetToSetupPose();
                if (heroChoice.MainAttackType == IDs.MELEE)
                {
                    Spine.Animation anim = null;
                    anim = model.Skeleton.Data.FindAnimation("attackMelee");
                    if (anim == null) anim = model.Skeleton.Data.FindAnimation("attackMelee1");
                    if (anim == null) anim = model.Skeleton.Data.FindAnimation("attackMelee2");
                    model.AnimationState.SetAnimation(0, anim, true);
                }
                else
                {
                    Spine.Animation anim = null;
                    anim = model.Skeleton.Data.FindAnimation("attackRange");
                    if (anim == null) anim = model.Skeleton.Data.FindAnimation("attackRange1");
                    if (anim == null) anim = model.Skeleton.Data.FindAnimation("attackRange2");
                    if (anim == null) anim = model.Skeleton.Data.FindAnimation("attackGun");
                    model.AnimationState.SetAnimation(0, anim, true);
                }
            }
            else
            {
                model.SetActive(false);
            }

            //var perks = heroChoice.GetPerks();
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
            //    //txtPerkName.text = perk.GetName();
            //    //txtPerkDescription.text = perk.GetDescription();
            //}
            //else
            //{
            //    imgPerk.transform.parent.SetActive(false);
            //}
        }

        void OnBtnJoin_Pressed()
        {
            Back();
            MainPanel.instance.ShowEditTeamInMainMenu(heroChoice);
        }
    }
}