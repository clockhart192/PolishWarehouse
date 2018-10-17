using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolishWarehouse.Models;
using PolishWarehouseData;

namespace PolishWarehouseTesting
{
    [TestClass]
    public class PolishTests
    {
        [TestMethod]
        public void SavePolishModel()
        {
            var p = new PolishModel()
            {
                BrandID = 1,
                ColorID = 1,
                BrandName = "test",
                Coats = 1,
                ColorName = "test",
                ColorNumber = 1,
                Description = "SavePolishModel Test Polish",
                GiftFromName = "",
                GlitterColors = null,
                GlitterColorsIDs = null,
                HasBeenTried = false,
                Images = null,
                PolishName = "SavePolishModel Test Polish",
                Quantity = 1,
                SecondaryColors = null,
                SecondaryColorsIDs = null,
                WasGift = false
            };

            var resp = p.Save();

            Assert.AreEqual(true, resp);
        }
    }
}
