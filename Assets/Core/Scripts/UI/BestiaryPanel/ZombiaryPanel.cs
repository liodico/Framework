using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Inspector;
using Utilities.Components;
using System;
using Spine.Unity;
using Utilities.Common;


namespace FoodZombie.UI
{
    public class ZombiaryPanel : MyGamesBasePanel
    {
        #region Members

        [Separator("Left Area")]
        [SerializeField] List<ZombieSlotView> mListZombieSlotView;
        [SerializeField] ToggleGroup mToggleGroupZombie;
        [SerializeField] ScrollRect mScrollView;

        [Separator("Right Area")]
        [SerializeField] private TextMeshProUGUI mTxtFound;
        [SerializeField] private TextMeshProUGUI mTxtZombieName;
        [SerializeField] private TextMeshProUGUI mTxtDifficult;
        //[SerializeField] private TextMeshProUGUI mTxtKillsCount;
        //[SerializeField] private TextMeshProUGUI mTxtHP;
        //[SerializeField] private TextMeshProUGUI mTxtMeleeAtk;
        //[SerializeField] private TextMeshProUGUI mTxtMeleeAtkSpeed;
        //[SerializeField] private TextMeshProUGUI mTxtRangeAtk;
        //[SerializeField] private TextMeshProUGUI mTxtRangeAtkSpeed;
        //[SerializeField] private TextMeshProUGUI mTxtSpeed;
        [SerializeField] private TextMeshProUGUI mTxtZombieDescription;
        [SerializeField] private TextMeshProUGUI mTxtZombieIndex;
        [SerializeField] private SkeletonGraphic mModel;

        private List<EnemyData> listEnemyDatas;

        protected EnemiesGroup EnemiesGroup => GameData.Instance.EnemiesGroup;

        #endregion

        //=============================================

        #region MonoBehaviour

        private void Start()
        {

        }

        internal override void Init()
        {
            listEnemyDatas = EnemiesGroup.GetAllEnemyDatas();
            for (int i = listEnemyDatas.Count - 1; i >= 0; i--)
            {
                var item = listEnemyDatas[i];
                if (item.Id >= 240 && item.Id <= 245)
                {
                    listEnemyDatas.RemoveAt(i);
                }
            }

            LoadListEnemies();
        }

        private void OnEnable()
        {
            if (listEnemyDatas != null)
            {
                LoadListEnemies();

                if (EnemiesGroup.LastZombieViewedId != -1)
                {
                    EnemyData characterData = EnemiesGroup.GetEnemyData(EnemiesGroup.LastZombieViewedId);
                    int indexOf = listEnemyDatas.IndexOf(characterData);
                    int COLUMN = 3;
                    float value = 0f;
                    if (indexOf >= COLUMN)
                    {
                        value = ((float)(indexOf / COLUMN) + 1f) / ((float)(listEnemyDatas.Count / COLUMN));
                    }
                    StartCoroutine(FocusScrollView(1f - value));
                }
            }
        }

        #endregion

        //=============================================

        #region Public

        #endregion

        //==============================================

        #region Private

        private void OnBtnNextPage_Pressed()
        {
        }

        private void OnBtnPrevPage_Pressed()
        {
        }

        private void LoadListEnemies()
        {
            mListZombieSlotView.Free();

            EnemyData temp;
            ZombieSlotView zombieSlotView;
            int hasViewed = 0;
            int length = listEnemyDatas.Count;
            for (int i = 0; i < length; i++)
            {
                temp = listEnemyDatas[i];
                if (temp.Viewed) hasViewed++;

                zombieSlotView = mListZombieSlotView.Obtain(mListZombieSlotView[0].transform.parent);
                zombieSlotView.Init(temp, null, ShowInfoZombie);
                zombieSlotView.SetActive(true);
            }
            if (hasViewed > 0) mToggleGroupZombie.allowSwitchOff = false;
            else mToggleGroupZombie.allowSwitchOff = true;

            mTxtFound.text = "Found: " + hasViewed + "/" + length;
        }

        private void ShowInfoZombie(EnemyData enemyData)
        {
            EnemiesGroup.SetLastZombieViewed(enemyData.Id);

            mTxtZombieName.text = enemyData.GetName();
            mTxtDifficult.text = "Level: " + enemyData.GetRarityName();
            //mTxtKillsCount.text = "Kills: " + characterData.KillsCount;
            //mTxtHP.text = characterData.baseData.hp + "";
            //if (characterData.baseData.meleeAtk > 0)
            //{
            //    mTxtMeleeAtk.text = characterData.baseData.meleeAtk + "";
            //    mTxtMeleeAtkSpeed.text = characterData.baseData.meleeAtkSpeed + "";
            //}
            //else
            //{
            //    mTxtMeleeAtk.text = "-";
            //    mTxtMeleeAtkSpeed.text = "-";
            //}
            //if (characterData.baseData.rangeAtk > 0)
            //{
            //    mTxtRangeAtk.text = characterData.baseData.rangeAtk + "";
            //    mTxtRangeAtkSpeed.text = characterData.baseData.rangerAtkSpeed + "";
            //}
            //else
            //{
            //    mTxtRangeAtk.text = "-";
            //    mTxtRangeAtkSpeed.text = "-";
            //}
            //mTxtSpeed.text = characterData.baseData.speed + "";
            mTxtZombieDescription.text = enemyData.GetDescription();
            mTxtZombieIndex.text = (listEnemyDatas.IndexOf(enemyData) + 1) + "/" + listEnemyDatas.Count;

            var skeletonData = enemyData.GetSkeletonData();
            if (skeletonData != null)
            {
                mModel.SetActive(true);
                mModel.skeletonDataAsset = skeletonData;
                mModel.Initialize(true);
                mModel.Skeleton.SetSkin(enemyData.GetSkinName());
                mModel.Play("idle1", true);
            }
            else
            {
                mModel.SetActive(false);
            }
        }

        IEnumerator FocusScrollView(float value)
        {
            yield return null;
            //mScrollView.verticalNormalizedPosition = value;
        }

        #endregion
    }
}