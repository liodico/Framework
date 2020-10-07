
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Pattern.Data;

namespace FoodZombie
{
    public class ConsumableItemData : DataGroup, IComparable<ConsumableItemData>
    {
        public Action<int> onStockNumberChanged;
        public Action<int> onUsageNumberChanged;

        private IntegerData mStockNumber;
        private IntegerData mUsageNumber;

        public int StockNumber => mStockNumber.Value;
        public int UsageNumber => mUsageNumber.Value;

        public ConsumableItemData(int pId) : base(pId)
        {
            mStockNumber = AddData(new IntegerData(0));
            mUsageNumber = AddData(new IntegerData(1));
        }

        public virtual bool CanUse(int pQuantity = 1)
        {
            return mStockNumber.Value >= 1;
        }

        public virtual void CountUsage(int pValue)
        {
            mUsageNumber.Value+=(pValue);
            if (onUsageNumberChanged != null)
                onUsageNumberChanged(pValue);
        }

        public virtual void SetStock(int pValue)
        {
            mStockNumber.Value=(pValue);
            if (onStockNumberChanged != null)
                onStockNumberChanged(pValue);
        }

        public virtual void AddToStock(int pValue)
        {
            mStockNumber.Value+=(pValue);
            if (onStockNumberChanged != null)
                onStockNumberChanged(pValue);
        }

        public bool Use(int pValue = 1)
        {
            if (CanUse(pValue))
            {
                AddToStock(-pValue);
                CountUsage(pValue);
                return true;
            }
            return false;
        }

        public bool CanSell()
        {
            return mStockNumber.Value > 0;
        }

        public int CompareTo(ConsumableItemData other)
        {
            return mStockNumber.Value.CompareTo(other.mStockNumber.Value);
        }

        public virtual Sprite GetIcon() => null;
    }
}