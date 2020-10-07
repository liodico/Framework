
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Common;
using Utilities.Pattern.Data;
using Utilities.Service.RFirebase;
using Debug = Utilities.Common.Debug;

namespace FoodZombie
{
    public class SafeData : DataGroup
    {
        private const int TIME_1 = 3 * 60 * 60;
        private const int TIME_2 = 6 * 60 * 60;
        private const int TIME_3 = 9 * 60 * 60;

        #region Members
        private TimerTask mTimer1;
        private TimerTask mTimer2;
        private TimerTask mTimer3;
        private BoolData mGetSafe1;
        private BoolData mGetSafe2;
        private BoolData mGetSafe3;

        public bool CanClaimSafe1 => mGetSafe1.Value;
        public bool CanClaimSafe2 => mGetSafe2.Value;
        public bool CanClaimSafe3 => mGetSafe3.Value;

        public bool IsRunning => mTimer1.IsRunning;

        #endregion

        #region Public

        public SafeData(int pId) : base(pId)
        {
            mTimer1 = AddData(new TimerTask(0, false));
            mTimer2 = AddData(new TimerTask(1, false));
            mTimer3 = AddData(new TimerTask(2, false));

            mGetSafe1 = AddData(new BoolData(3, false));
            mGetSafe2 = AddData(new BoolData(4, false));
            mGetSafe3 = AddData(new BoolData(5, false));

            mTimer1.SetOnComplete(GetFreeSafe1);
            mTimer2.SetOnComplete(GetFreeSafe2);
            mTimer3.SetOnComplete(GetFreeSafe3);
        }

        public void CheckStartTime()
        {
            if (!mGetSafe1.Value && !mTimer1.IsRunning)
            {
                mTimer1.Start(TIME_1);
            }
        }

        public void CheckStartTimeMore()
        {
            if (!mGetSafe2.Value && !mTimer2.IsRunning && LogicAPI.CanShowSafe2Panel())
            {
                mTimer2.Start(TIME_2);
            }
            if (!mGetSafe3.Value && !mTimer3.IsRunning && LogicAPI.CanShowSafe3Panel())
            {
                mTimer3.Start(TIME_3);
            }
        }

        public int GetSafe1()
        {
            return TIME_1 / 2;
        }

        public int GetSafe2()
        {
            return TIME_2 / 2;
        }

        public int GetSafe3()
        {
            return TIME_3 / 2;
        }

        public int ClaimSafe1()
        {
            mGetSafe1.Value = false;
            EventDispatcher.Raise(new SafeChangeValueEvent(0, false));
            mTimer1.Start(TIME_1);
            return TIME_1 / 2;
        }

        public int ClaimSafe2()
        {
            mGetSafe2.Value = false;
            EventDispatcher.Raise(new SafeChangeValueEvent(1, false));
            mTimer2.Start(TIME_2);
            return TIME_2 / 2;
        }

        public int ClaimSafe3()
        {
            mGetSafe3.Value = false;
            EventDispatcher.Raise(new SafeChangeValueEvent(2, false));
            mTimer3.Start(TIME_3);
            return TIME_3 / 2;
        }

        public string GetTimeSafe1()
        {
            var s = mTimer1.RemainSeconds;
            return TimeHelper.FormatHHMMss(s, true);
        }

        public string GetTimeSafe2()
        {
            if (LogicAPI.CanShowSafe2Panel())
            {
                var s = mTimer2.RemainSeconds;
                return TimeHelper.FormatHHMMss(s, true);
            }
            else
            {
                return "Unlock at Mission 16";
            }
        }

        public string GetTimeSafe3()
        {
            if (LogicAPI.CanShowSafe3Panel())
            {
                var s = mTimer3.RemainSeconds;
                return TimeHelper.FormatHHMMss(s, true);
            }
            else
            {
                return "Unlock at Mission 31";
            }
        }

        #endregion

        //==============================================

        #region Private

        private void GetFreeSafe1(TimerTask timer, long remain)
        {
            if (remain <= 0)
            {
                mGetSafe1.Value = true;
                EventDispatcher.Raise(new SafeChangeValueEvent(0, true));
            }
        }

        private void GetFreeSafe2(TimerTask timer, long remain)
        {
            if (remain <= 0)
            {
                mGetSafe2.Value = true;
                EventDispatcher.Raise(new SafeChangeValueEvent(1, true));
            }
        }

        private void GetFreeSafe3(TimerTask timer, long remain)
        {
            if (remain <= 0)
            {
                mGetSafe3.Value = true;
                EventDispatcher.Raise(new SafeChangeValueEvent(2, true));
            }
        }

        #endregion
    }
}