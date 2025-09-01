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
				Monitor.Log("Quick Consume mod loaded successfully!", LogLevel.Info);
			}
			catch (Exception ex)
			{
				Monitor.Log($"Error loading Quick Consume mod: {ex.Message}", LogLevel.Error);
			}
		}

		private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
		{
			Monitor.Log("OnGameLaunched event fired - looking for Generic Mod Config Menu...", LogLevel.Debug);

			// Get Generic Mod Config Menu's API (if it's installed)
			var configMenu = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");

			// Try alternative UniqueID if the first one doesn't work
			if (configMenu is null)
			{
				configMenu = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("GenericModConfigMenu");
			}

			if (configMenu is null)
			{
				Monitor.Log("Generic Mod Config Menu not found. Make sure it's installed for in-game configuration.", LogLevel.Info);
				// List all loaded mods for debugging
				Monitor.Log("Loaded mods:", LogLevel.Debug);
				foreach (var mod in Helper.ModRegistry.GetAll())
				{
					Monitor.Log($"  - {mod.Manifest.UniqueID} ({mod.Manifest.Name})", LogLevel.Debug);
				}
				return;
			}

			Monitor.Log("Found Generic Mod Config Menu! Setting up configuration options...", LogLevel.Debug);          // Register mod
			configMenu.Register(
				mod: ModManifest,
				reset: () => Config = new ModConfig(),
				save: () => Helper.WriteConfig(Config)
			);

			Monitor.Log("Mod registered with GMCM successfully.", LogLevel.Debug);

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

			Monitor.Log("All GMCM configuration options added successfully!", LogLevel.Debug);
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

			// Calculate energy/health gains (used for both buff and non-buff foods)
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

			// Apply energy/health
			who.Stamina = System.Math.Min(who.MaxStamina, who.Stamina + staminaGain);
			who.health = System.Math.Min(who.maxHealth, who.health + healthGain);

			// Play eat sfx (no animation)
			if (Config.PlayEatSound)
				Game1.playSound("eat");

			// Show health/energy gain as floating text (optional)
			if (Config.ShowHealthGain && (staminaGain > 0 || healthGain > 0))
			{
				string gainText = "";
				if (healthGain > 0) gainText += $"+{healthGain} Health ";
				if (staminaGain > 0) gainText += $"+{staminaGain} Energy";

				var hudMessage = new HUDMessage(null)
				{
					message = gainText.Trim(),
					timeLeft = HUDMessage.defaultTime,
					messageSubject = obj
				};
				Game1.addHUDMessage(hudMessage);
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

	// Extension methods that work across versions
	internal static class EatHelpers
	{
		public static int staminaRecoveredOnConsumption(this SObject o)
			=> o.Edibility > 0 ? o.Edibility * 2 : 0; // vanilla rule of thumb

		public static int healthRecoveredOnConsumption(this SObject o)
			=> o.Edibility > 0 ? (int)System.Math.Round(o.Edibility * 0.4) : 0;

		public static void performEatEffects(this SObject o, Farmer who)
		{
			// This method is kept for compatibility but simplified
			// We now only allow quick consumption of non-buffed foods
			try
			{
				if (o.Edibility > 0)
				{
					// If this food has buffs, we don't allow quick consumption
					if (BuffData.HasBuffs(o.Name))
					{
						return;
					}
				}
			}
			catch
			{
				// Silent error handling - just don't process if something goes wrong
			}
		}
	}

	/// <summary>The API interface for Generic Mod Config Menu.</summary>
	public interface IGenericModConfigMenuApi
	{
		void Register(IManifest mod, Action reset, Action save, bool titleScreenOnly = false);
		void AddBoolOption(IManifest mod, Func<bool> getValue, Action<bool> setValue, Func<string> name, Func<string>? tooltip = null, string? fieldId = null);
		void AddKeybind(IManifest mod, Func<SButton> getValue, Action<SButton> setValue, Func<string> name, Func<string>? tooltip = null, string? fieldId = null);
	}
}