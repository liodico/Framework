using FoodZombie.UI;
using DG.Tweening;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Common;
using Utilities.Components;

namespace FoodZombie
{
    public class EvolveView : MonoBehaviour
    {
        public TextMeshProUGUI txtLevelPercent;
        public Image imgLevelBarBG;
        public Image imgLevelBar;
        public TextMeshProUGUI txtRank;
        public Image imgRank;
        public SkeletonGraphic model;

        private HeroData characterData;

        public void Init(RewardInfo pReward)
        {
            if (pReward.Type == IDs.REWARD_TYPE_UNLOCK_CHARACTER)
            {
                characterData = GameData.Instance.HeroesGroup.GetHeroData(pReward.Id);
                var oldRank = characterData.Rank - 1;
                txtRank.text = oldRank + "";
                imgRank.sprite = AssetsCollection.instance.GetRankIcon(oldRank);
                //imgRank.SetNativeSize();
                var skeletonData = characterData.GetSkeletonData();
                if (skeletonData != null)
                {
                    model.SetActive(true);
                    model.skeletonDataAsset = skeletonData;
                    model.Initialize(true);
                    //model.Skeleton.SetSkin(characterData.baseData.skin + "");
                    var anim = model.SkeletonData.FindAnimation("victory");
                    if (anim == null) anim = model.SkeletonData.FindAnimation("victory1");
                    if (anim == null) anim = model.SkeletonData.FindAnimation("idle1");
                    model.AnimationState.SetAnimation(0, anim, true);
                }
                else
                {
                    model.SetActive(false);
                }

                var sizeBG = imgLevelBarBG.rectTransform.sizeDelta;
                imgLevelBar.rectTransform.sizeDelta = sizeBG;
                txtLevelPercent.text = "100%";
            }
        }

        private void OnEnable()
        {
            float lerp = 0;
            DOTween.To(tweenVal => lerp = tweenVal, 100f, 0f, 0.75f)
            .OnUpdate(() =>
            {
                txtLevelPercent.text = lerp.ToString("0") + "%";
                var sizeBG = imgLevelBarBG.rectTransform.sizeDelta;
                imgLevelBar.rectTransform.sizeDelta = new Vector2(sizeBG.x * lerp / 100f, sizeBG.y);
            })
            .OnComplete(() =>
            {
                txtLevelPercent.text = "0%";
                txtRank.text = characterData.Rank + "";
                imgRank.sprite = characterData.GetRankIcon();
                //imgRank.SetNativeSize();
                PlayFX(imgRank.transform);
                var sizeBG = imgLevelBarBG.rectTransform.sizeDelta;
                imgLevelBar.rectTransform.sizeDelta = new Vector2(0f, sizeBG.y);
            })
            .SetDelay(0.5f);
        }

        private void PlayFX(Transform pTarget)
        {
            //UIFXManager.instance.PlayStarFX(pTarget);
            SimpleLeanFX.instance.Bubble(pTarget, 0.35f);
        }
    }
}