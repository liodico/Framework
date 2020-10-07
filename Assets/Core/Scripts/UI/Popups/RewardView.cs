using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Common;


namespace FoodZombie
{
    public class RewardView : MonoBehaviour
    {
        //public Action onAppear;

        [SerializeField] protected Image mImgIcon;
        [SerializeField] protected TextMeshProUGUI mTxtQuantity;
        [SerializeField] protected TextMeshProUGUI mTxtInfo1;
        [SerializeField] protected TextMeshProUGUI mTxtInfo2;
        //[SerializeField] protected Animator mAnimator;
        //[SerializeField] protected Image[] mImgCovers;

        //public bool HasAnimation()
        //{
        //    return mAnimator != null;
        //}

        public void Init(RewardInfo pReward)
        {
            mImgIcon.sprite = pReward.GetIcon();
            //mImgIcon.SketchByFixedHeight(150);

            mTxtQuantity.text = pReward.GetShortDescription();
            mTxtInfo1.text = "";
            mTxtInfo2.text = "";

            if (pReward.Type == IDs.REWARD_TYPE_UNLOCK_CHARACTER)
            {
                var heroData = GameData.Instance.HeroesGroup.GetHeroData(pReward.Id);
                if (heroData != null)
                {
                    mTxtInfo1.text = heroData.GetName();
                    mTxtInfo2.text = heroData.GetRarityName();
                }
            }
            else if (pReward.Type == IDs.REWARD_TYPE_POWER_UP)
            {
                var powerUp = GameData.Instance.ItemsGroup.GetPowerUpItem(pReward.Id);
                if (powerUp != null)
                {
                    mTxtInfo1.text = powerUp.GetName();
                    mTxtInfo2.text = powerUp.GetAttributeDescription();
                }
            }
            else if (pReward.Type == IDs.REWARD_TYPE_PRE_UNIT)
            {
                mTxtInfo1.text = pReward.GetName();
            }

            //if (mAnimator != null)
            //    mAnimator.Rebind();
            //foreach (var img in mImgCovers)
            //    img.SetActive(false);
        }

        //public void SetCoverColor(Color pColor)
        //{
        //    foreach (var img in mImgCovers)
        //        img.color = pColor;
        //}

        ////Trigger by animation clip
        //public void OnAppear()
        //{
        //    if (onAppear != null)
        //        onAppear();
        //}

        //internal void PlayAnimation()
        //{
        //    mAnimator.SetTrigger("Appear");
        //}
    }
}