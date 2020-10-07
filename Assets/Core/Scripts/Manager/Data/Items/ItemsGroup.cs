
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Common;
using Utilities.Pattern.Data;
using Debug = Utilities.Common.Debug;
using Random = UnityEngine.Random;

namespace FoodZombie
{
    public class ItemsGroup : DataGroup
    {
        #region Members

        private DataGroup mPowerUpItemsGroup;

        #endregion

        //=============================================

        #region Public

        public ItemsGroup(int pId) : base(pId)
        {
            mPowerUpItemsGroup = AddData(new DataGroup(0));

            InitPowerUpItems();
        }
        
        //=== POWERUP

        public PowerUpItemData GetPowerUpItem(int pId)
        {
            return mPowerUpItemsGroup.GetData<PowerUpItemData>(pId);
        }

        public List<PowerUpItemData> GetPowerUpItems(bool pSort = true)
        {
            var items = new List<PowerUpItemData>();
            foreach (PowerUpItemData item in mPowerUpItemsGroup.Children)
                if (item.baseData.visible)
                    items.Add(item);
            if (pSort)
                items.Sort();
            return items;
        }

        public PowerUpItemData GetRandomPowerUpItem()
        {
            var items = new List<PowerUpItemData>();
            foreach (PowerUpItemData item in mPowerUpItemsGroup.Children)
                if (item.baseData.visible)
                    items.Add(item);
            return items[Random.Range(0, items.Count)];
        }

        public PowerUpItemData AddPowerUpItem(int pId, int pValue)
        {
            var powerUp = GetPowerUpItem(pId);
            if (powerUp != null)
                powerUp.AddToStock(pValue);
            return powerUp;
        }

        public PowerUpItemData AddPowerUpItemRandomly(int pValue)
        {
            var powerUp = GetRandomPowerUpItem();
            if (powerUp != null)
            {
                powerUp.AddToStock(pValue);
                return powerUp;
            }
            return powerUp;
        }
        
        #endregion

        //==============================================

        #region Private
        
        private void InitPowerUpItems()
        {
            var dataContent = GameData.GetTextContent("Data/PowerUpItems");
            var collection = JsonHelper.GetJsonList<PowerUpDefinition>(dataContent);
            if (collection != null)
                foreach (var item in collection)
                {
                    var data = new PowerUpItemData(item.id, item);
                    mPowerUpItemsGroup.AddData(data);
                }

            Debug.LogNewtonJson(collection);
        }

        #endregion
    }
}