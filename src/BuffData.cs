using System.Collections.Generic;

namespace QuickConsume
{
	/// <summary>
	/// Comprehensive buff data extracted from the Stardew Valley wiki cooking page.
	/// This provides detailed buff information for display purposes and documentation.
	/// 
	/// Note: The mod now uses the game's native objectData.Buffs system for buff detection,
	/// which automatically works with modded items. This class is kept for reference
	/// and potential future features that need detailed buff information.
	/// </summary>
	public static class BuffData
	{
		public class FoodBuffInfo
		{
			public string Name { get; set; } = "";
			public int Farming { get; set; } = 0;
			public int Mining { get; set; } = 0;
			public int Foraging { get; set; } = 0;
			public int Fishing { get; set; } = 0;
			public int Luck { get; set; } = 0;
			public int Attack { get; set; } = 0;
			public int Defense { get; set; } = 0;
			public int Magnetism { get; set; } = 0;
			public int Speed { get; set; } = 0;
			public int MaxEnergy { get; set; } = 0;
			public bool HasSpecialBuff { get; set; } = false; // For Squid Ink Ravioli's special debuff protection
			public int DurationMinutes { get; set; } = 7; // Default duration in minutes
			public int DurationSeconds { get; set; } = 0; // Additional seconds for precise timing
		}

		/// <summary>
		/// Complete buff data for all foods that provide buffs, extracted from the wiki.
		/// </summary>
		public static readonly Dictionary<string, FoodBuffInfo> FoodBuffs = new(System.StringComparer.OrdinalIgnoreCase)
		{
			// Drinks
			["Coffee"] = new() { Speed = 1, DurationMinutes = 1, DurationSeconds = 0 },
			["Triple Shot Espresso"] = new() { Speed = 1, DurationMinutes = 4, DurationSeconds = 12 },
			["Ginger Ale"] = new() { Luck = 1, DurationMinutes = 5, DurationSeconds = 0 },

			// Breakfast & Basic Foods
			["Complete Breakfast"] = new() { Farming = 2, MaxEnergy = 50, DurationMinutes = 7, DurationSeconds = 0 },
			["Hashbrowns"] = new() { Farming = 1, DurationMinutes = 5, DurationSeconds = 35 },
			["Pancakes"] = new() { Foraging = 2, DurationMinutes = 11, DurationSeconds = 11 },

			// Luck Foods
			["Lucky Lunch"] = new() { Luck = 3, DurationMinutes = 11, DurationSeconds = 11 },
			["Spicy Eel"] = new() { Luck = 1, Speed = 1, DurationMinutes = 7, DurationSeconds = 0 },
			["Fried Eel"] = new() { Luck = 1, DurationMinutes = 7, DurationSeconds = 0 },
			["Shrimp Cocktail"] = new() { Fishing = 1, Luck = 1, DurationMinutes = 10, DurationSeconds = 2 },

			// Attack/Combat Foods
			["Fried Mushroom"] = new() { Attack = 2, DurationMinutes = 7, DurationSeconds = 0 },
			["Roots Platter"] = new() { Attack = 3, DurationMinutes = 5, DurationSeconds = 35 },

			// Defense Foods
			["Pumpkin Soup"] = new() { Defense = 2, Luck = 2, DurationMinutes = 7, DurationSeconds = 41 },
			["Autumn's Bounty"] = new() { Foraging = 2, Defense = 2, DurationMinutes = 7, DurationSeconds = 41 },
			["Eggplant Parmesan"] = new() { Mining = 1, Defense = 3, DurationMinutes = 4, DurationSeconds = 39 },
			["Stuffing"] = new() { Defense = 2, DurationMinutes = 5, DurationSeconds = 35 },
			["Crab Cakes"] = new() { Speed = 1, Defense = 1, DurationMinutes = 16, DurationSeconds = 47 },
			["Banana Pudding"] = new() { Mining = 1, Luck = 1, Defense = 1, DurationMinutes = 5, DurationSeconds = 1 },
			["Mango Sticky Rice"] = new() { Defense = 3, DurationMinutes = 5, DurationSeconds = 1 },

			// Skill-Based Foods
			["Farmer's Lunch"] = new() { Farming = 3, DurationMinutes = 5, DurationSeconds = 35 },
			["Survival Burger"] = new() { Foraging = 3, DurationMinutes = 5, DurationSeconds = 35 },
			["Dish O' The Sea"] = new() { Fishing = 3, DurationMinutes = 5, DurationSeconds = 35 },
			["Miner's Treat"] = new() { Mining = 3, Magnetism = 32, DurationMinutes = 5, DurationSeconds = 35 },

			// Fishing Foods
			["Fish Taco"] = new() { Fishing = 2, DurationMinutes = 7, DurationSeconds = 0 },
			["Seafoam Pudding"] = new() { Fishing = 4, DurationMinutes = 3, DurationSeconds = 30 },
			["Chowder"] = new() { Fishing = 1, DurationMinutes = 16, DurationSeconds = 47 },
			["Fish Stew"] = new() { Fishing = 3, DurationMinutes = 16, DurationSeconds = 47 },
			["Escargot"] = new() { Fishing = 2, DurationMinutes = 16, DurationSeconds = 47 },
			["Lobster Bisque"] = new() { Fishing = 3, MaxEnergy = 50, DurationMinutes = 16, DurationSeconds = 47 },
			["Trout Soup"] = new() { Fishing = 1, DurationMinutes = 4, DurationSeconds = 39 },

			// Energy/Magnetism Foods
			["Bean Hotpot"] = new() { MaxEnergy = 30, Magnetism = 32, DurationMinutes = 7, DurationSeconds = 0 },
			["Crispy Bass"] = new() { Magnetism = 64, DurationMinutes = 7, DurationSeconds = 0 },
			["Red Plate"] = new() { MaxEnergy = 50, DurationMinutes = 3, DurationSeconds = 30 },
			["Super Meal"] = new() { MaxEnergy = 40, Speed = 1, DurationMinutes = 3, DurationSeconds = 30 },

			// Multi-Skill Foods
			["Tom Kha Soup"] = new() { Farming = 2, MaxEnergy = 30, DurationMinutes = 7, DurationSeconds = 0 },
			["Pepper Poppers"] = new() { Farming = 2, Speed = 1, DurationMinutes = 7, DurationSeconds = 0 },
			["Maple Bar"] = new() { Farming = 1, Fishing = 1, Mining = 1, DurationMinutes = 16, DurationSeconds = 47 },
			["Tropical Curry"] = new() { Foraging = 4, DurationMinutes = 5, DurationSeconds = 1 },

			// Mining Foods
			["Cranberry Sauce"] = new() { Mining = 2, DurationMinutes = 3, DurationSeconds = 30 },

			// Special Foods
			["Squid Ink Ravioli"] = new() { Mining = 1, HasSpecialBuff = true, DurationMinutes = 4, DurationSeconds = 39 } // Special debuff protection (Mining buff duration), 2m 59s for protection
		};

		/// <summary>
		/// Check if a food has any buffs.
		/// 
		/// NOTE: This method is kept for reference but is no longer used for primary buff detection.
		/// The mod now uses the game's native buff detection system which works with all items including mods.
		/// </summary>
		/// <param name="foodName">The display name of the food item</param>
		/// <returns>True if the food is known to provide buffs in our reference data</returns>
		public static bool HasBuffs(string foodName)
		{
			return FoodBuffs.ContainsKey(foodName);
		}

		/// <summary>
		/// Get buff information for a food
		/// </summary>
		public static FoodBuffInfo? GetBuffInfo(string foodName)
		{
			return FoodBuffs.TryGetValue(foodName, out var info) ? info : null;
		}

		/// <summary>
		/// Get the total duration in seconds for a food
		/// </summary>
		public static int GetTotalDurationSeconds(string foodName)
		{
			if (FoodBuffs.TryGetValue(foodName, out var info))
				return info.DurationMinutes * 60 + info.DurationSeconds;
			return 0;
		}

		/// <summary>
		/// Get a formatted duration string (e.g., "7m 41s" or "11m 11s")
		/// </summary>
		public static string GetDurationString(string foodName)
		{
			if (FoodBuffs.TryGetValue(foodName, out var info))
			{
				if (info.DurationSeconds == 0)
					return $"{info.DurationMinutes}m";
				else
					return $"{info.DurationMinutes}m {info.DurationSeconds}s";
			}
			return "0m";
		}

		/// <summary>
		/// Get a human-readable description of the buffs a food provides
		/// </summary>
		/// <param name="foodName">The display name of the food item</param>
		/// <returns>A formatted string describing the buffs, or "No buffs" if none found</returns>
		public static string GetBuffDescription(string foodName)
		{
			if (!FoodBuffs.TryGetValue(foodName, out var info))
				return "No buffs";

			var parts = new List<string>();

			if (info.Farming > 0) parts.Add($"+{info.Farming} Farming");
			if (info.Mining > 0) parts.Add($"+{info.Mining} Mining");
			if (info.Foraging > 0) parts.Add($"+{info.Foraging} Foraging");
			if (info.Fishing > 0) parts.Add($"+{info.Fishing} Fishing");
			if (info.Luck > 0) parts.Add($"+{info.Luck} Luck");
			if (info.Attack > 0) parts.Add($"+{info.Attack} Attack");
			if (info.Defense > 0) parts.Add($"+{info.Defense} Defense");
			if (info.Speed > 0) parts.Add($"+{info.Speed} Speed");
			if (info.Magnetism > 0) parts.Add($"+{info.Magnetism} Magnetism");
			if (info.MaxEnergy > 0) parts.Add($"+{info.MaxEnergy} Max Energy");
			if (info.HasSpecialBuff) parts.Add("Debuff Protection");

			return parts.Count > 0 ? string.Join(", ", parts) : "No buffs";
		}
	}
}
