using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Common;
using Utilities.Components;

namespace FoodZombie.UI
{
    public class EvolveRewardsPopup : MyGamesBasePanel
    {
        [Header("Build-in prefab")]
        [SerializeField] private List<EvolveView> mEvolveViews;
        [SerializeField] private HorizontalAlignmentUI mAlignment;

        private List<RewardInfo> mRewards;
        private bool mShowing;
        private RectTransform mPreferedSpawnPos;
        private Color mCoverColor = Color.white;

        internal override void Init()
        {
            mEvolveViews.Free();
            mRewards = new List<RewardInfo>();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            mEvolveViews.Free();

            mShowing = false;
            mRewards = new List<RewardInfo>();
        }

        private void OnEnable()
        {
            if (!mShowing && mRewards != null)
                StartCoroutine(IEShowRewards(mRewards));
        }

        public void SetRewards(List<RewardInfo> pRewards, RectTransform pSpawnPosition)
        {
            mCoverColor = Color.clear;

            mRewards.AddRange(pRewards);
            mPreferedSpawnPos = pSpawnPosition;

            //if (!mShowing && gameObject.activeSelf)
            //    StartCoroutine(IEShowRewards(mRewards));
        }

        public void SetRewards(List<RewardInfo> pRewards)
        {
            mCoverColor = Color.clear;

            mRewards.AddRange(pRewards);
            mPreferedSpawnPos = null;
            
            //if (!mShowing && gameObject.activeSelf)
            //    StartCoroutine(IEShowRewards(mRewards));
        }

        private IEnumerator IEShowRewards(List<RewardInfo> pRewards)
        {
            Lock(true);

            yield return null;
            yield return null;

            mShowing = true;

            for (int i = 0; i < pRewards.Count; i++)
            {
                var reward = pRewards[i];

                Vector2 pSpawnPosition = mAlignment.transform.position;
                if (mPreferedSpawnPos != null)
                    pSpawnPosition = mPreferedSpawnPos.position;

                EvolveView obj = null;

                obj = mEvolveViews.Obtain(mAlignment.transform);
                obj.Init(reward);
                obj.SetActive(true);
                obj.transform.Reset();
                obj.transform.position = pSpawnPosition;

                float tweenTime = 0.1f * i;
                if (pSpawnPosition != (Vector2)mAlignment.transform.position)
                    tweenTime = 0.75f + 0.1f * i;

                if (tweenTime == 0)
                {
                    PlayFX(reward, obj.transform);
                }
                else
                {
                    obj.transform.position = pSpawnPosition;
                    mAlignment.tweenTime = tweenTime;
                    mAlignment.AlignByTweener(() =>
                    {
                        PlayFX(reward, obj.transform);
                    });
                }
                yield return new WaitForSeconds(tweenTime);
            }

            mShowing = false;

            yield return new WaitForSeconds(1.25f);

            Lock(false);
        }

        private void PlayFX(RewardInfo r, Transform pTarget)
        {
            UIFXManager.instance.PlayStarFX(pTarget);
            SimpleLeanFX.instance.Bubble(pTarget, 0.5f);
        }
    }
}