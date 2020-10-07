using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Common;
using Utilities.Pattern.Data;
using Utilities.Service.RFirebase;

namespace FoodZombie
{
    public class CurrencyData : DataGroup
    {
        private IntegerData mValue;
        private bool mOnlineOnly;
        private DateTime? mStartDate;
        private DateTime? mExpiredDate;
        private int mMax = -1;

        public CurrencyData(int pId, int pDefault, int pMax = -1, bool pOnlineOnly = false, DateTime? pStartDate = null, DateTime? pEndDate = null) : base(pId)
        {
            mValue = AddData(new IntegerData(0, pDefault));
            mMax = pMax;
            mOnlineOnly = pOnlineOnly;
            mStartDate = pStartDate;
            mExpiredDate = pEndDate;
        }

        public CurrencyData(int pId, int pDefault, int pMax = -1) : base(pId)
        {
            mValue = AddData(new IntegerData(0, pDefault));
            mMax = pMax;
            mOnlineOnly = false;
            mStartDate = null;
            mExpiredDate = null;
        }

        public void Add(int pValue, bool limit)
        {
            if (pValue == 0)
                return;

            if (limit && mMax > 0 && mValue.Value + pValue >= mMax)
                mValue.Value = mMax;
            else
                mValue.Value += pValue;
            EventDispatcher.Raise(new CurrencyChangedEvent(Id, pValue));
        }

        public bool Pay(int pValue)
        {
            if (mValue.Value < pValue)
                return false;

            Add(-pValue, false);
            return true;
        }

        public bool CanPay(int pValue)
        {
            return mValue.Value >= pValue;
        }

        public int GetValue()
        {
            if (mOnlineOnly)
            {
                var now = GetNow();
                if (now != null && ((mStartDate == null && mExpiredDate == null) || (now.Value > mStartDate.Value && now < mExpiredDate.Value)))
                    return mValue.Value;
                else
                    return 0;
            }
            else
            {
                return mValue.Value;
            }
        }

        public DateTime? GetNow()
        {
            //TODO: Call get server time here
            var utcNow = DateTime.UtcNow;
            var vnNow = utcNow.AddHours(7);
            return vnNow;
        }

        /// <summary>
        /// Get Config from server and set to this
        /// </summary>
        public void SetDate(DateTime pStartDate, DateTime pEndDate)
        {
            mStartDate = pStartDate;
            mExpiredDate = pEndDate;
        }
    }

    public class CurrenciesGroup : DataGroup
    {
        public Dictionary<int, CurrencyData> currencies;

        public CurrenciesGroup(int pId) : base(pId)
        {
            currencies = new Dictionary<int, CurrencyData>();
            currencies.Add(IDs.CURRENCY_COIN, AddData(new CurrencyData(IDs.CURRENCY_COIN, 0, -1))); //-1 là không có giới hạn
            currencies.Add(IDs.CURRENCY_CASH, AddData(new CurrencyData(IDs.CURRENCY_CASH, 0, -1)));
        }

        public override void PostLoad()
        {
            base.PostLoad();
        }

        public bool CanPay(int pType, int pValue)
        {
            if (currencies.ContainsKey(pType))
                return currencies[pType].CanPay(pValue);
            else
                return false;
        }

        public bool Pay(int pType, int pValue)
        {
            if (currencies.ContainsKey(pType))
            {
                bool paid = currencies[pType].Pay(pValue);
                return paid;
            }
            else
                return false;
        }

        public void Add(int pType, int pValue, bool limit = false)
        {
            if (currencies.ContainsKey(pType))
                currencies[pType].Add(pValue, limit);
        }

        public void SetDate(int pType, DateTime pStartDate, DateTime pEndDate)
        {
            if (currencies.ContainsKey(pType))
                currencies[pType].SetDate(pStartDate, pEndDate);
        }

        public int GetValue(int pType)
        {
            if (currencies.ContainsKey(pType))
                return currencies[pType].GetValue();
            else
                return 0;
        }

        public int GetGold()
        {
            return currencies[IDs.CURRENCY_COIN].GetValue();
        }

        public int GetCash()
        {
            return currencies[IDs.CURRENCY_CASH].GetValue();
        }
    }
}