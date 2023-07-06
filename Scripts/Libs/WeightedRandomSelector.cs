﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Libs
{
	/// <summary>
	/// Represents a weighted random picker that can randomly select stored items based on their weight.
	/// </summary>
	/// <typeparam name="T">The type of the stored items.</typeparam>
	public class WeightedRandomPicker<T>
	{
		private List<WeightedItem<T>> items;
		private Random random;

		public WeightedRandomPicker()
		{
			items = new List<WeightedItem<T>>();
			random = new Random();
		}

		/// <summary>
		/// Adds an item with the specified weight to the picker. If the item already exists, its weight is increased.
		/// </summary>
		/// <param name="item">The item to add.</param>
		/// <param name="weight">The weight of the item.</param>
		public void Add(T item, int weight)
		{
			// Check if the item already exists in the list
			WeightedItem<T> existingItem = items.Find(i => i.Item.Equals(item));

			if (existingItem != null)
			{
				// Increase the weight of the existing item
				existingItem.Weight += weight;
			}
			else
			{
				// Add a new item to the list
				items.Add(new WeightedItem<T>(item, weight));
			}
		}

		/// <summary>
		/// Picks a random item from the picker based on their weights.
		/// </summary>
		/// <returns>The randomly selected item.</returns>
		public T PickRandom()
		{
			int totalWeight = 0;

			// Calculate the total weight of all items
			foreach (var item in items)
			{
				totalWeight += item.Weight;
			}

			// Generate a random number within the total weight range
			int randomNumber = random.Next(0, totalWeight);

			// Find the item corresponding to the random number
			foreach (var item in items)
			{
				if (randomNumber < item.Weight)
				{
					return item.Item;
				}

				randomNumber -= item.Weight;
			}

			// This should never happen, but to avoid compilation errors, return the default value
			return default(T);
		}

		/// <summary>
		/// Adjusts the weight of an existing item in the picker.
		/// </summary>
		/// <param name="item">The item whose weight needs to be adjusted.</param>
		/// <param name="weight">The new weight of the item.</param>
		public void AdjustWeight(T item, int weight)
		{
			WeightedItem<T> existingItem = items.Find(i => i.Item.Equals(item));

			if (existingItem != null)
			{
				existingItem.Weight = weight;
			}
		}

		/// <summary>
		/// Removes an item from the picker.
		/// </summary>
		/// <param name="item">The item to remove.</param>
		public void Remove(T item)
		{
			WeightedItem<T> existingItem = items.Find(i => i.Item.Equals(item));

			if (existingItem != null)
			{
				items.Remove(existingItem);
			}
		}

		private class WeightedItem<TItem>
		{
			public TItem Item { get; set; }
			public int Weight { get; set; }

			public WeightedItem(TItem item, int weight)
			{
				Item = item;
				Weight = weight;
			}
		}
	}
}
