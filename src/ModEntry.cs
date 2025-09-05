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

			// Store current values to calculate actual restoration for display
			int previousStamina = (int)who.Stamina;
			int previousHealth = who.health;

			// Apply energy/health restoration
			who.Stamina = System.Math.Min(who.MaxStamina, who.Stamina + staminaGain);
			who.health = System.Math.Min(who.maxHealth, who.health + healthGain);

			// Apply buffs using the game's native buff system (if any)
			ApplyBuffsFromItem(obj, who);

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

		/// <summary>
		/// Apply buffs from an item using the game's native buff system.
		/// This ensures buffs are applied exactly as the game would apply them,
		/// preventing stacking issues and preserving all buff mechanics.
		/// </summary>
		/// <param name="obj">The consumable object</param>
		/// <param name="who">The player consuming the item</param>
		private void ApplyBuffsFromItem(SObject obj, Farmer who)
		{
			try
			{
				// Use the game's native GetFoodOrDrinkBuffs method to get exactly the same buffs
				// that would be applied if the item was consumed normally
				var buffs = obj.GetFoodOrDrinkBuffs();

				foreach (var buff in buffs)
				{
					if (buff != null)
					{
						// Apply the buff using the same method the game uses
						// This ensures proper buff ID handling, stacking rules, and duration management
						who.applyBuff(buff);
					}
				}
			}
			catch (Exception ex)
			{
				Monitor.Log($"Error applying buffs from {obj.DisplayName}: {ex.Message}", LogLevel.Warn);
			}
		}
	}
}