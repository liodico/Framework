
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Common;
using Utilities.Pattern.Data;

namespace FoodZombie
{
    public class GameConfigGroup : DataGroup
    {
        #region Members

        public Action<bool> onEnableMuisic;
        public Action<bool> onEnableSFX;
        public Action<bool> onEnableVibration;

        private BoolData mNoAds;
        private StringData mDisplayName;
        private BoolData mEnableSFX;
        private BoolData mEnableMusic;
        private BoolData mEnableVibration;
        private BoolData mEnableNotification;
        private BoolData mEnableHint;
        private ListData<string> mSentEvents;
        /// <summary>
        /// This is GPGS user id
        /// </summary>
        private StringData mStorageUserId;
        private StringData mStorageUserName;
        private BoolData mRated;
        private StringData mLastTimeRestore;
        private IntegerData mCountShowRate;
        private IntegerData mLastDay;
        private BoolData mShowedDailyRate;

        public bool EnableSFX => mEnableSFX.Value;
        public bool EnableMusic => mEnableMusic.Value;
        public bool EnableVibration => mEnableVibration.Value;
        public bool EnableNotification => mEnableNotification.Value;
        public bool EnableHint => mEnableHint.Value;
        public string DisplayName => mDisplayName.Value;
        public List<string> SentEvents => mSentEvents.GetValues();
        public string StorageUserId => mStorageUserId.Value;
        public string StorageUserName => mStorageUserName.Value;
        public bool Rated => mRated.Value;
        public int CountShowRate => mCountShowRate.Value;
        public bool ShowedDailyRate => mShowedDailyRate.Value;
        public bool NoAds => mNoAds.Value;

        public DateTime LastTimeRestore
        {
            get
            {
                if (string.IsNullOrEmpty(mLastTimeRestore.Value))
                    return DateTime.MinValue;
                else
                {
                    DateTime time = DateTime.Now;
                    if (DateTime.TryParse(mLastTimeRestore.Value, out time))
                        return time;
                    else
                        return DateTime.MinValue;
                }
            }
            set
            {
                mLastTimeRestore.Value = (value.ToString());
            }
        }

        #endregion

        //==============================================

        #region Public

        public GameConfigGroup(int pId) : base(pId)
        {
            mEnableSFX = AddData(new BoolData(1, true));
            mEnableMusic = AddData(new BoolData(2, true));
            mEnableNotification = AddData(new BoolData(3, true));
            mEnableHint = AddData(new BoolData(4, true));
            mDisplayName = AddData(new StringData(5));
            mSentEvents = AddData(new ListData<string>(6, new List<string>()));
            mStorageUserId = AddData(new StringData(7));
            mStorageUserName = AddData(new StringData(8));
            mRated = AddData(new BoolData(9));
            mLastTimeRestore = AddData(new StringData(10));
            mCountShowRate = AddData(new IntegerData(11));
            mLastDay = AddData(new IntegerData(12));
            mShowedDailyRate = AddData(new BoolData(13));
            mEnableVibration = AddData(new BoolData(14, true));
            mNoAds = AddData(new BoolData(15, false));
        }

        public override void PostLoad()
        {
            base.PostLoad();
            CheckNewDay();
        }

        public void SetEnableHint(bool pValue)
        {
            mEnableHint.Value = (pValue);
        }

        public void SetEnableMusic(bool pValue)
        {
            mEnableMusic.Value = (pValue);
            onEnableMuisic.Raise(pValue);
        }

        public void SetEnableNotification(bool pValue)
        {
            mEnableNotification.Value = (pValue);
        }

        public void SetEnableSFX(bool pValue)
        {
            mEnableSFX.Value = (pValue);
            onEnableSFX.Raise(pValue);
        }

        public void SetEnableVibration(bool pValue)
        {
            mEnableVibration.Value = pValue;
            onEnableVibration.Raise(pValue);
        }

        public void SetDisplayName(string pValue)
        {
            mDisplayName.Value = (pValue);
        }

        public void AddSentEvent(string pEventName)
        {
            SentEvents.Add(pEventName);
        }

        public void SetNoAds(bool pValue)
        {
            mNoAds.Value = pValue;
        }

        /// <summary>
        /// NOTE: only call this method when
        /// Upload or backup storage
        /// DO NOT CALL IT WHEN LOGIN GPGS
        /// </summary>
        public void SetStorageAccount(string puserId, string pUserName)
        {
            mStorageUserId.Value = (puserId);
            mStorageUserName.Value = (pUserName);
        }

        public void Rate()
        {
            mRated.Value = (true);
        }

        public void AddCountShowRate()
        {
            mShowedDailyRate.Value = (true);
            mCountShowRate.Value += (1);
        }
        public bool CheckNewDayForSpin()
        {
            return mLastDay.Value != DateTime.Now.DayOfYear;
        }


        #endregion

        //=============================================

        #region Private

        private void CheckNewDay()
        {
            int day = DateTime.Now.DayOfYear;
            if (mLastDay.Value != day)
            {
                GameData.Instance.WheelData.NewDay();
                mLastDay.Value = (day);
                mShowedDailyRate.Value = (false);
            }
        }

        #endregion
    }
}