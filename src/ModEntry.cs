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

		public override void Entry(IModHelper helper)
		{
			try
			{
				Config = helper.ReadConfig<ModConfig>();
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
			// Get Generic Mod Config Menu's API (if it's installed)
			var configMenu = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");

			// Try alternative UniqueID if the first one doesn't work
			configMenu ??= Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("GenericModConfigMenu");
			if (configMenu is null)
			{
				Monitor.Log("Generic Mod Config Menu not found. Make sure it's installed for in-game configuration.", LogLevel.Info);
				return;
			}

			// Register mod
			configMenu.Register(
				mod: ModManifest,
				reset: () => Config = new ModConfig(),
				save: () => Helper.WriteConfig(Config)
			);

			// Add mod configuration options
			configMenu.AddBoolOption(
				mod: ModManifest,
				name: () => "Require Modifier Key",
				tooltip: () => "If enabled, you must hold the modifier key while right-clicking to instantly eat food.",
				getValue: () => Config.RequireModifier,
				setValue: value => Config.RequireModifier = value
			);

			configMenu.AddKeybind(
				mod: ModManifest,
				name: () => "Modifier Key",
				tooltip: () => "The key to hold when 'Require Modifier Key' is enabled.",
				getValue: () => Config.ModifierKey,
				setValue: value => Config.ModifierKey = value
			);

			configMenu.AddBoolOption(
				mod: ModManifest,
				name: () => "Allow When Full",
				tooltip: () => "If enabled, you can eat food even when your health and energy are already full.",
				getValue: () => Config.AllowWhenFull,
				setValue: value => Config.AllowWhenFull = value
			);

			configMenu.AddBoolOption(
				mod: ModManifest,
				name: () => "Play Eat Sound",
				tooltip: () => "If enabled, plays the eating sound effect when instantly consuming food.",
				getValue: () => Config.PlayEatSound,
				setValue: value => Config.PlayEatSound = value
			);

			configMenu.AddBoolOption(
				mod: ModManifest,
				name: () => "Show Health/Energy Gain",
				tooltip: () => "If enabled, shows floating text indicating how much health and energy you gained.",
				getValue: () => Config.ShowHealthGain,
				setValue: value => Config.ShowHealthGain = value
			);
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

			// Calculate energy/health gains using correct Stardew Valley formula
			int staminaGain = obj.Edibility > 0 ? (int)System.Math.Round(obj.Edibility * 2.5) : 0;
			int healthGain = obj.Edibility > 0 ? (int)System.Math.Round(obj.Edibility * 0.5) : 0;

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

			// Store current values to measure actual restoration
			int previousStamina = (int)who.Stamina;
			int previousHealth = who.health;

			// Apply energy/health
			who.Stamina = System.Math.Min(who.MaxStamina, who.Stamina + staminaGain);
			who.health = System.Math.Min(who.maxHealth, who.health + healthGain);

			// Calculate actual restoration amounts
			int actualStaminaGain = (int)who.Stamina - previousStamina;
			int actualHealthGain = who.health - previousHealth;

			// Play eat sfx (no animation)
			if (Config.PlayEatSound)
				Game1.playSound("eat");

			// Show "Quickly consumed" message with consumable icon
			var consumedMessage = new HUDMessage(null)
			{
				message = $"Quickly consumed {obj.DisplayName}",
				timeLeft = HUDMessage.defaultTime,
				messageSubject = obj
			};
			Game1.addHUDMessage(consumedMessage);

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
			// Use our comprehensive buff data system
			return BuffData.HasBuffs(obj.Name);
		}
	}

	public class ModConfig
	{
		public bool RequireModifier { get; set; } = false;      // set true if you want to hold a key to instant-eat
		public SButton ModifierKey { get; set; } = SButton.LeftShift;
		public bool AllowWhenFull { get; set; } = true;          // allow eating even when full
		public bool PlayEatSound { get; set; } = true;           // play the eating sound effect
		public bool ShowHealthGain { get; set; } = true;         // show floating text for health/energy gained
	}

	/// <summary>The API interface for Generic Mod Config Menu.</summary>
	public interface IGenericModConfigMenuApi
	{
		void Register(IManifest mod, Action reset, Action save, bool titleScreenOnly = false);
		void AddBoolOption(IManifest mod, Func<bool> getValue, Action<bool> setValue, Func<string> name, Func<string>? tooltip = null, string? fieldId = null);
		void AddKeybind(IManifest mod, Func<SButton> getValue, Action<SButton> setValue, Func<string> name, Func<string>? tooltip = null, string? fieldId = null);
	}
}