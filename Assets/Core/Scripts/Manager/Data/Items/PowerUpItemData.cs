using System;
using System.Collections.Generic;

using UnityEngine;
using Utilities.Common;
using Utilities.Pattern.Data;

namespace FoodZombie
{
    [System.Serializable]
    public class PowerUpDefinition : AttributesCollection<AttributeParse>
    {
        public int id;
        public string name;
        public int[] craftMaterialIds;
        public int[] craftMaterialQuantities;
        public bool visible;
        public int buyCost;
        public int sellCost;
    }

    //======================================

    public class PowerUpItemData : ConsumableItemData
    {
        #region Members

        public PowerUpDefinition baseData { get; private set; }
        public AttributesContainer<BaseAttribute> attsContainer { get; private set; }

        private LocalizationGetter mLocalizedName;
        private LocalizationGetter mLocalizedDescription;

        #endregion

        //=============================================

        #region Public

        public PowerUpItemData(int pId, PowerUpDefinition pBase) : base(pId)
        {
            baseData = pBase;

            attsContainer = new AttributesContainer<BaseAttribute>();
            for (int i = 0; i < baseData.Attributes.Count; i++)
            {
                var att = baseData.Attributes[i];
                attsContainer.Add(new Attribute(att.id, att.value, att.unlock, att.increase, att.max));
            }

            mLocalizedName = new LocalizationGetter("POWERUP_NAME_" + Id, baseData.name);
            mLocalizedDescription = new LocalizationGetter("POWER_UP_ITEM_DESCRIPTION_" + Id, baseData.name);
        }

        public override Sprite GetIcon()
        {
            return AssetsCollection.instance.powerUpItem.GetAsset(baseData.id - 1);
        }

        public string GetName()
        {
            return mLocalizedName.Get();
        }

        internal string GetDescription()
        {
            return mLocalizedDescription.Get();
        }

        public string GetAttributeDescription()
        {
            string description = "";
            for (int i = 0; i < baseData.Attributes.Count; i++)
            {
                var att = new Attribute(baseData.Attributes[i].id, baseData.Attributes[i].value);
                //description += $"{att.GetDisplayBonus()}";
                description += $"{att.GetDisplayBonus(1, true)}";
                if (i <= baseData.Attributes.Count)
                    description += "\n";
            }
            return description;
        }

        public void Sell()
        {
            if (!CanSell())
                return;
            GameData.Instance.CurrenciesGroup.Add(IDs.CURRENCY_COIN, baseData.sellCost);
            AddToStock(-1);
        }

        public override void AddToStock(int pValue)
        {
            base.AddToStock(pValue);

            EventDispatcher.Raise(new StockPowerUpItemChangedEvent(Id));
        }

        #endregion

        //==============================================

        #region Private

        #endregion
    }
}
