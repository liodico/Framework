using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Common;
using Utilities.Service.Ads;
using Utilities.Service.RFirebase;
using Utilities.Services;
using Random = UnityEngine.Random;

namespace FoodZombie
{
    public class AdsHelper : MonoBehaviour
    {
        #region Members

        private static AdsHelper mInstance;
        public static AdsHelper Instance
        {
            get
            {
                if (mInstance == null)
                    mInstance = FindObjectOfType<AdsHelper>();
                return mInstance;
            }
        }

        //private Advertising Advertising { get { return Advertising.Instance; } }

        private Action<bool> mOnRewardedAdCompleted;
        private Dictionary<string, bool> mTrackedFails = new Dictionary<string, bool>();

        #endregion
        public DateTime m_TimeShowInterstitial = DateTime.Now;

        //=============================================

        #region MonoBehaviour

        private void Awake()
        {
            if (mInstance == null)
                mInstance = this;
            else if (mInstance != this)
                Destroy(gameObject);
        }

        //-----show ads itner------
        private void Start()
        {
            m_TimeShowInterstitial = DateTime.Now;
            m_TimeShowInterstitial = m_TimeShowInterstitial.AddSeconds(Config.TIME_INTERSTITIAL_OPENING);
        }

        private void OnApplicationPause(bool pause)
        {
            if (!pause)
            {
                if (!AdsManager.Instance.TapjoyClient.IsOfferwallReady())
                    AdsManager.Instance.TapjoyClient.LoadOfferwall();
            }
        }

        #endregion

        //=============================================

        #region Methods

        //---------------------------------------------
        // TapJoy
        //---------------------------------------------

        public static void __ShowTapJoyOfferwall()
        {
            Instance.ShowTapjoyOfferWall();
        }

        public void ShowTapjoyOfferWall()
        {
            AdsManager.Instance.TapjoyClient.ShowOfferWall();
        }

        public static int __GetTapJoyBallance()
        {
            return Instance.GetTapJoyBallance();
        }

        public int GetTapJoyBallance()
        {
            return AdsManager.Instance.TapjoyClient.currentBallance;
        }

        public static bool __IsTapjoyOfferwallReady()
        {
            return Instance.IsTapjoyOfferwallReady();
        }

        public bool IsTapjoyOfferwallReady()
        {
            return AdsManager.Instance.TapjoyClient.IsOfferwallReady();
        }

        public static void __UpdateTapJoyBallance(Action<bool> pOnResponse)
        {
            Instance.UpdateTapJoyBallance(pOnResponse);
        }

        public void UpdateTapJoyBallance(Action<bool> onSuccess)
        {
            AdsManager.Instance.TapjoyClient.UpdateCurrencyBallance(onSuccess);
        }

        public static void __SpentCurrencyBallance(Action<bool> pOnResponse)
        {
            Instance.SpentCurrencyBallance(pOnResponse);
        }

        public void SpentCurrencyBallance(Action<bool> pOnResponse)
        {
            int all = AdsManager.Instance.TapjoyClient.currentBallance;
            AdsManager.Instance.TapjoyClient.SpendCurrencyBallance(all, pOnResponse);
        }

        public static void __AwardTapJoyCurrency(int pAmount, Action<bool> pOnResponse)
        {
            Instance.AwardTapJoyCurrency(pAmount, pOnResponse);
        }

        public void AwardTapJoyCurrency(int pAmount, Action<bool> pOnResponse)
        {
            AdsManager.Instance.TapjoyClient.AwardCurrency(pAmount, pOnResponse);
        }

        //---------------------------------------------
        // Rewarded Ads
        //---------------------------------------------

        public static bool __IsVideoRewardedAdReady(string pWhere, RewardedAdNetwork pNetwork = RewardedAdNetwork.None, bool pTrackLog = true)
        {
            if (pTrackLog) Config.LogEvent(TrackingConstants.EVENT_REQUEST_REWARDED_VIDEO, TrackingConstants.PARAM_SOURCE, pWhere);
            return Instance.IsVideoRewardedAdReady(pWhere, pNetwork);
        }

        private bool IsVideoRewardedAdReady(string pWhere = "", RewardedAdNetwork pNetwork = RewardedAdNetwork.None)
        {
            if (pNetwork == RewardedAdNetwork.None)
            {
                if (AdsManager.Instance.IsRewardedAdReady(RewardedAdNetwork.AdMob))
                    return true;
                else if (AdsManager.Instance.IsRewardedAdReady(RewardedAdNetwork.UnityAds))
                    return true;
                else if (AdsManager.Instance.IsRewardedAdReady(RewardedAdNetwork.AudienceNetwork))
                    return true;
                else if (AdsManager.Instance.IsRewardedAdReady(RewardedAdNetwork.IronSource))
                    return true;
            }
            else if (pNetwork == RewardedAdNetwork.UnityAds)
            {
                if (AdsManager.Instance.IsRewardedAdReady(RewardedAdNetwork.UnityAds))
                    return true;
            }
            else if (pNetwork == RewardedAdNetwork.AdMob)
            {
                if (AdsManager.Instance.IsRewardedAdReady(RewardedAdNetwork.AdMob))
                    return true;
            }
            else if (pNetwork == RewardedAdNetwork.AudienceNetwork)
            {
                if (AdsManager.Instance.IsRewardedAdReady(RewardedAdNetwork.AudienceNetwork))
                    return true;
            }
            else if (pNetwork == RewardedAdNetwork.IronSource)
            {
                if (AdsManager.Instance.IsRewardedAdReady(RewardedAdNetwork.IronSource))
                    return true;
            }
            if (!string.IsNullOrEmpty(pWhere))
            {
                if (!mTrackedFails.ContainsKey(pWhere))
                    mTrackedFails.Add(pWhere, false);

                if (!mTrackedFails[pWhere])
                {
                    mTrackedFails[pWhere] = true;
                }
            }
            return false;
        }

        public static bool __ShowVideoRewardedAd(Action<bool> pOnCompleted, string pWhere, RewardedAdNetwork pNetwork = RewardedAdNetwork.None)
        {
            bool showed = Instance.ShowVideoRewardedAd(pOnCompleted, pWhere, pNetwork);

            if (showed)
            {
                var sentEvents = GameData.Instance.GameConfigGroup.SentEvents;
                //Only send this event once
                //if (!sentEvents.Contains(TrackingConstants.EVENT_FIRST_ADS))
                //{
                //    var playingTime = GameData.Instance.GetPlayingTime() / 60f;
                //    RFirebaseManager.LogEvent(TrackingConstants.EVENT_FIRST_ADS, TrackingConstants.PARAM_MINUTES, playingTime);
                //    GameData.Instance.GameConfigGroup.AddSentEvent(TrackingConstants.EVENT_FIRST_ADS);
                //}
            }
            return showed;
        }

        private bool ShowVideoRewardedAd(Action<bool> pOnCompleted, string pWhere, RewardedAdNetwork pNetwork = RewardedAdNetwork.None)
        {
            AdsManager.Instance.onRewardedAdCompleted -= OnRewardedAdCompleted;
            AdsManager.Instance.onRewardedAdSkipped -= OnRewardedAdSkipped;
            AdsManager.Instance.onRewardedAdCompleted += OnRewardedAdCompleted;
            AdsManager.Instance.onRewardedAdSkipped += OnRewardedAdSkipped;

            mOnRewardedAdCompleted = pOnCompleted;
            var network = RewardedAdNetwork.None;
            if (pNetwork == RewardedAdNetwork.None)
            {
#if UNITY_EDITOR
                network = RewardedAdNetwork.UnityAds;
                AdsManager.Instance.ShowRewardedAd(network);
#else
                network = AdsManager.Instance.ShowRewardedAdRandomly();
#endif
            }
            else
            {
                network = pNetwork;
                AdsManager.Instance.ShowRewardedAd(network);
            }

            if (network != RewardedAdNetwork.None)
            {
                Config.LogEvent(TrackingConstants.EVENT_WATCH_REWARDED_VIDEO,
                                new string[] { TrackingConstants.PARAM_SOURCE, TrackingConstants.PARAM_NETWORK }, new string[] { pWhere, network.ToString() });
                Config.LogEvent(TrackingConstants.EVENT_COUNT_AD_IMPRESSION);

                return true;
            }
            return false;
        }

        private void OnRewardedAdSkipped(RewardedAdNetwork pNetwork)
        {
            AdsManager.Instance.onRewardedAdSkipped -= OnRewardedAdSkipped;
            mOnRewardedAdCompleted.Raise(false);
            mOnRewardedAdCompleted = null;

            StartCoroutine(ForceLoadAllAds());
        }

        private void OnRewardedAdCompleted(RewardedAdNetwork pNetwork)
        {
            m_TimeShowInterstitial = DateTime.Now;
            m_TimeShowInterstitial = m_TimeShowInterstitial.AddSeconds(Config.TIME_INTERSTITIAL_AFTER_RV);

            AdsManager.Instance.onRewardedAdCompleted -= OnRewardedAdCompleted;
            mOnRewardedAdCompleted.Raise(true);
            mOnRewardedAdCompleted = null;

            StartCoroutine(ForceLoadAllAds());
        }

        //Interstitial
        private bool IsInterstitialAdReady(InterstitialAdNetwork pNetwork = InterstitialAdNetwork.None)
        {
            Config.LogEvent(TrackingConstants.EVENT_REQUEST_INTERSTITIAL_AD);

            if (pNetwork == InterstitialAdNetwork.None)
            {
                if (AdsManager.Instance.IsRewardedAdReady(RewardedAdNetwork.AdMob))
                    return true;
                else if (AdsManager.Instance.IsRewardedAdReady(RewardedAdNetwork.UnityAds))
                    return true;
                else if (AdsManager.Instance.IsRewardedAdReady(RewardedAdNetwork.AudienceNetwork))
                    return true;
                else if (AdsManager.Instance.IsRewardedAdReady(RewardedAdNetwork.IronSource))
                    return true;
            }
            else if (pNetwork == InterstitialAdNetwork.UnityAds)
            {
                if (AdsManager.Instance.IsRewardedAdReady(RewardedAdNetwork.UnityAds))
                    return true;
            }
            else if (pNetwork == InterstitialAdNetwork.AdMob)
            {
                if (AdsManager.Instance.IsRewardedAdReady(RewardedAdNetwork.AdMob))
                    return true;
            }
            else if (pNetwork == InterstitialAdNetwork.AudienceNetwork)
            {
                if (AdsManager.Instance.IsRewardedAdReady(RewardedAdNetwork.AudienceNetwork))
                    return true;
            }
            else if (pNetwork == InterstitialAdNetwork.IronSource)
            {
                if (AdsManager.Instance.IsRewardedAdReady(RewardedAdNetwork.IronSource))
                    return true;
            }
            return false;
        }

        public static bool __ShowInterstitialAd(InterstitialAdNetwork pNetwork = InterstitialAdNetwork.None)
        {
            return Instance.ShowInterstitialAd(pNetwork);
        }

        private bool ShowInterstitialAd(InterstitialAdNetwork pNetwork = InterstitialAdNetwork.None)
        {
            if (IsInterstitialAdReady())
            {
                var network = InterstitialAdNetwork.None;
                if (pNetwork == InterstitialAdNetwork.None)
                {
#if UNITY_EDITOR
                    network = InterstitialAdNetwork.UnityAds;
                    AdsManager.Instance.ShowInterstitialAd(network);
#else
                    network =  AdsManager.Instance.ShowInterstitialAdRandomly();
#endif
                }
                else
                {
                    network = pNetwork;
                    AdsManager.Instance.ShowInterstitialAd(network);
                }

                Config.LogEvent(TrackingConstants.EVENT_SHOW_INTERSTITIAL_AD, TrackingConstants.PARAM_NETWORK, network.ToString());
                Config.LogEvent(TrackingConstants.EVENT_COUNT_AD_IMPRESSION);

                m_TimeShowInterstitial = DateTime.Now;
                m_TimeShowInterstitial = m_TimeShowInterstitial.AddSeconds(Config.TIME_INTERSTITIAL_AFTER_INTERSTITIAL);

                StartCoroutine(ForceLoadAllAds());
                return true;
            }

            //RFirebaseManager.LogEvent(TrackingConstants.EVENT_FALL_ADS_INTERSTITIAL);
            return false;
        }

        public bool CanShowInterstitial()
        {
            if (GameData.Instance.GameConfigGroup.NoAds) return false;

            if ((GameData.Instance.MissionsGroup.CountMissionWin + 1) > Config.MIN_LEVEL_INTERSTITIAL)
            {
                DateTime now = DateTime.Now;
                if (DateTime.Compare(now, m_TimeShowInterstitial) > 0)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        private IEnumerator ForceLoadAllAds()
        {
            yield return new WaitForSeconds(1f);
            UnityEngine.Debug.Log("ForceLoadAllAds");
            AdsManager.Instance.ForceLoadAllAds();
        }
    }
}