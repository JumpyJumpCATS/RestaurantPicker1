///Edited by Cristopher Torres
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Restaurant
{
    public class RestaurantPick
    {
        /// <summary>
        /// Represents a single restaurant and its menu.
        /// Each menu entry is stored as a dictionary:
        ///   key   = item label (string)
        ///   value = price (decimal)
        private class Restaurant
        {
            public int RestaurantId { get; set; }
            public Dictionary<string, decimal> Menu { get; set; } = new Dictionary<string, decimal>();
        }

        private readonly List<Restaurant> _restaurants = new List<Restaurant>();

        /// <summary>
        /// Reads restaurant menu data from the CSV file.
        /// File format:
        ///   restaurantId, price, item1, item2, ...
        /// </summary>
        /// <param name="filePath">Path to the comma separated restuarant menu data</param>
        public void ReadRestaurantData(string filePath)
        {
            try
            {
                var records = File.ReadLines(filePath); // read file line by line

                foreach (var record in records)
                {
                    var data = record.Split(',');

                    if (data.Length < 3)
                        continue; // skip invalid lines

                    int restaurantId = int.Parse(data[0].Trim());
                    decimal price = decimal.Parse(data[1].Trim());

                    // Find restaurant entry or create a new one
                    var restaurant = _restaurants.Find(r => r.RestaurantId == restaurantId);
                    if (restaurant == null)
                    {
                        restaurant = new Restaurant { RestaurantId = restaurantId };
                        _restaurants.Add(restaurant);
                    }

                    // Extract menu items (mzybe one or several if it's a value meal)
                    var menuItems = data.Skip(2)
                                        .Select(s => s.Trim().ToLower())
                                        .Where(s => !string.IsNullOrWhiteSpace(s));

                    foreach (var item in menuItems)
                    {
                        // Item already exists? then keep  cheaper price
                        if (!restaurant.Menu.ContainsKey(item) || price < restaurant.Menu[item])
                        {
                            restaurant.Menu[item] = price;
                        }
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("File not found: " + ex.Message);
            }
        }

        /// <summary>
        /// Takes in items you would like to eat and returns the best restaurant that serves them.
        /// </summary>
        /// <param name="items">Items you would like to eat (seperated by ',')</param>
        /// <returns>Restaurant Id and price tuple</returns>
        public (int RestaurantId, decimal Price)? PickBestRestaurant(string items)
        {
            // make req items cohessive 
            var requestedItems = new HashSet<string>(
                items.Split(',', StringSplitOptions.RemoveEmptyEntries)
                     .Select(i => i.Trim().ToLower())
            );

            (int RestaurantId, decimal Price)? bestChoice = null;

            foreach (var restaurant in _restaurants)
            {
                // Check if restaurant offers ALL requested items
                if (requestedItems.All(item => restaurant.Menu.ContainsKey(item)))
                {
                    // Compute total price by summing individual item prices
                    decimal totalPrice = requestedItems.Sum(item => restaurant.Menu[item]);

                    // Keep track of cheapest option so far
                    if (bestChoice == null || totalPrice < bestChoice.Value.Price)
                    {
                        bestChoice = (restaurant.RestaurantId, totalPrice);
                    }
                }
            }

            return bestChoice;
        }

        static void Main(string[] args)
        {
            var restaurantPicker = new RestaurantPick();

            restaurantPicker.ReadRestaurantData(
                Path.GetFullPath(
                    Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory, @"../../../../restaurant_data.csv"
                    )
                )
            );

            // Item is found in restaurant 12 at price 3.36
            var bestRestaurant = restaurantPicker.PickBestRestaurant("gac") ?? (0, 0);
            Console.WriteLine(bestRestaurant.RestaurantId + ", " + bestRestaurant.Price);
            Console.WriteLine("Done!");
        }
    }
}
