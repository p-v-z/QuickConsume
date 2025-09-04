using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Objects;
using StardewValley.Menus;
using StardewValley.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using SObject = StardewValley.Object;

namespace QuickConsume
{
	public class ModEntry : Mod
	{
		private ModConfig Config = new();
		private ConfigMenuManager? configMenuManager;

		public override void Entry(IModHelper helper)
		{
			try
			{
				Config = helper.ReadConfig<ModConfig>();

				// Initialize configuration menu manager
				configMenuManager = new ConfigMenuManager(
					helper,
					ModManifest,
					Monitor,
					() => Config,
					config => Config = config
				);

				helper.Events.Input.ButtonPressed += OnButtonPressed;
				helper.Events.GameLoop.GameLaunched += OnGameLaunched;
			}
			catch (Exception ex)
			{
				Monitor.Log($"Error loading Quick Consume mod: {ex.Message}", LogLevel.Error);
			}
		}

		private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
		{
			configMenuManager?.SetupConfigMenu();
		}

		private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
		{
			// Only in-game, not in menus like title screen
			if (!Context.IsPlayerFree || Game1.player is null)
				return;

			// Require a modifier key if configured (useful to avoid accidental eats)
			if (Config.RequireModifier && !Helper.Input.IsDown(Config.ModifierKey))
				return;

			// Only trigger on the "use tool" / action buttons (right-click/controller A)
			if (!e.Button.IsActionButton())
				return;

			var who = Game1.player;
			Item? active = who.CurrentItem;

			// Must be a normal Object that's edible
			if (active is not SObject obj || obj.Edibility <= 0)
				return;

			// Optional: don't eat if HP/energy full unless override
			if (!Config.AllowWhenFull && who.Stamina >= who.MaxStamina && who.health >= who.maxHealth)
				return;

			// Calculate energy/health gains using the game's own methods
			int staminaGain = obj.staminaRecoveredOnConsumption();
			int healthGain = obj.healthRecoveredOnConsumption();

			// Check if this food has buffs - if so, don't allow quick consumption
			if (HasBuffs(obj))
			{
				// Show message that buffed consumables can't be consumed quickly
				var hudMessage = new HUDMessage(null)
				{
					message = "Buffed consumables can't be consumed quickly",
					timeLeft = HUDMessage.defaultTime,
					messageSubject = obj
				};
				Game1.addHUDMessage(hudMessage);

				// Don't suppress the button - let the game handle it normally
				return;
			}

			// Apply instant eating effects for non-buff foods

			// Store current values to calculate actual restoration for display
			int previousStamina = (int)who.Stamina;
			int previousHealth = who.health;

			// Apply energy/health restoration
			who.Stamina = System.Math.Min(who.MaxStamina, who.Stamina + staminaGain);
			who.health = System.Math.Min(who.maxHealth, who.health + healthGain);

			// Calculate actual restoration amounts for display
			int actualStaminaGain = (int)who.Stamina - previousStamina;
			int actualHealthGain = who.health - previousHealth;

			// Play eat sfx (no animation)
			if (Config.PlayEatSound)
				Game1.playSound("eat");

			// Show "Quickly consumed" message with consumable icon (optional)
			if (Config.ShowQuickConsumeDialog)
			{
				var consumedMessage = new HUDMessage(null)
				{
					message = $"Quickly consumed {obj.DisplayName}",
					timeLeft = HUDMessage.defaultTime,
					messageSubject = obj
				};
				Game1.addHUDMessage(consumedMessage);
			}

			// Show health/energy gain as separate messages like the game does (optional)
			if (Config.ShowHealthGain)
			{
				if (actualHealthGain > 0)
				{
					var healthMessage = new HUDMessage($"+{actualHealthGain} Health", 5);
					Game1.addHUDMessage(healthMessage);
				}
				if (actualStaminaGain > 0)
				{
					var energyMessage = new HUDMessage($"+{actualStaminaGain} Energy", 4);
					Game1.addHUDMessage(energyMessage);
				}
			}

			// Consume one from the stack
			obj.Stack--;
			if (obj.Stack <= 0)
				who.removeItemFromInventory(obj);

			// Hard-cancel any pending animations/stances the game might try to start this tick
			who.completelyStopAnimatingOrDoingAction();
			who.CanMove = true;

			// Swallow the click so vanilla doesn't open the confirm dialog afterward
			Helper.Input.Suppress(e.Button);
		}

		private bool HasBuffs(SObject obj)
		{
			try
			{
				// Primary method: Check the object's buff data
				if (Game1.objectData.TryGetValue(obj.ItemId, out var objectData))
				{
					// If the item has any buff definitions, it has buffs
					if (objectData.Buffs != null && objectData.Buffs.Count > 0)
					{
						// Simply check if any buff entries exist - they wouldn't be there unless they do something
						return true;
					}
				}

				// If we reach here, the item has no buff data in the game's native system
				// This likely means it's safe for quick consumption

				// Check if the item's category or type suggests it might have buffs
				// Most buff foods are in the "Cooking" category (-7) or specific artisan goods
				if (obj.Category == SObject.CookingCategory)
				{
					// Cooked foods might have buffs but we couldn't detect them through the normal system
					// This could be a vanilla item we missed or a modded item with custom buff logic
					Monitor.Log($"Cooked food '{obj.Name}' has no detectable buff data - using normal consumption for safety", LogLevel.Warn);
					return true;
				}

				// For all other categories (fruits, vegetables, foraged items, etc.)
				// these are usually safe for quick consumption
				Monitor.Log($"Food '{obj.Name}' appears safe for quick consumption (no buffs detected)", LogLevel.Trace);
				return false;
			}
			catch (Exception ex)
			{
				Monitor.Log($"Error checking buffs for {obj.Name}: {ex.Message}", LogLevel.Warn);

				// If we can't determine buff status, err on the side of caution
				// and use normal consumption to avoid breaking buff mechanics
				return true;
			}
		}
	}

}