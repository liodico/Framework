using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utilities.Components;
using FoodZombie;


namespace FoodZombie.UI
{
    public class ZombieSlotView : MonoBehaviour
    {
        [SerializeField] private Image mImgAvatar;
        [SerializeField] private GameObject mImgNotUnlock;
        [SerializeField] private Toggle mTogViewDetail;

        private EnemyData mCharacterData;
        private UnityAction<EnemyData> mShowInfoZombie;

        // Start is called before the first frame update
        void Start()
        {
            mTogViewDetail.onValueChanged.AddListener(OnTogViewDetail_Pressed);
        }

        // Update is called once per frame
        void Update()
        {

        }

        internal void Init(EnemyData pCharacterData, ToggleGroup pToggleGroup, UnityAction<EnemyData> pShowInfoZombie)
        {
            mCharacterData = pCharacterData;
            mTogViewDetail.group = pToggleGroup;

            if (mCharacterData.Viewed)
            {
                mShowInfoZombie = pShowInfoZombie;

                if (mCharacterData.baseData.id == GameData.Instance.EnemiesGroup.LastZombieViewedId)
                {
                    mTogViewDetail.isOn = true;
                    mShowInfoZombie(mCharacterData);
                }

                mImgAvatar.gameObject.SetActive(true);
                mImgAvatar.sprite = mCharacterData.GetIcon();
                mImgNotUnlock.SetActive(false);

                mTogViewDetail.interactable = true;
            }
            else
            {
                mImgAvatar.gameObject.SetActive(false);
                mImgNotUnlock.SetActive(true);

                mTogViewDetail.interactable = false;
            }
        }

        private void OnTogViewDetail_Pressed(bool value)
        {
            if (value && mShowInfoZombie != null)
            {
                mShowInfoZombie(mCharacterData);
            }
        }
    }
}
