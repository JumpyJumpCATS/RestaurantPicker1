using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Restaurant.Test
{
    [TestClass]
    public class RestaurantPickTests
    {
        public RestaurantPick RestaurantPicker;

        [TestInitialize]
        public void InitializeTests()
        {
            RestaurantPicker = new RestaurantPick();
            RestaurantPicker.ReadRestaurantData(
                Path.GetFullPath(
                    Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, @"../../../../restaurant_data.csv")
                    )
                );
        }

        [TestMethod]
        public void PickBestRestaurantTest_OneItem()
        {
            var result = RestaurantPicker.PickBestRestaurant("tofu_log");
            Assert.AreEqual((2, (decimal)6.5), result);
        }

        [TestMethod]
        public void PickBestRestaurantTest_TwoItems()
        {
            var result = RestaurantPicker.PickBestRestaurant("burger,tofu_log");
            Assert.AreEqual((2, (decimal)11.50), result);
        }

        [TestMethod]
        public void PickBestRestaurantTest_CombinationItems_1()
        {
            var result = RestaurantPicker.PickBestRestaurant("almond_biscuit,joe_frogger");
            Assert.AreEqual((12, (decimal)1.80), result);
        }

        [TestMethod]
        public void PickBestRestaurantTest_CombinationItems_2()
        {
            var result = RestaurantPicker.PickBestRestaurant("fancy_european_water,extreme_fajita");
            Assert.AreEqual((6, (decimal)11.0), result);
        }

        [TestMethod]
        public void PickBestRestaurantTest_NotFound()
        {
            var result = RestaurantPicker.PickBestRestaurant("chef_salad,wine_spritzer");
            Assert.IsNull(result);
        }
    }
}
