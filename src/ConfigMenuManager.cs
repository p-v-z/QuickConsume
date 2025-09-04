using StardewModdingAPI;

namespace QuickConsume
{
	/// <summary>
	/// Manages Generic Mod Config Menu integration for in-game configuration
	/// </summary>
	public class ConfigMenuManager
	{
		private readonly IModHelper helper;
		private readonly IManifest modManifest;
		private readonly IMonitor monitor;
		private readonly System.Func<ModConfig> getConfig;
		private readonly System.Action<ModConfig> setConfig;

		/// <summary>
		/// Initializes a new instance of the ConfigMenuManager
		/// </summary>
		/// <param name="helper">The mod helper instance</param>
		/// <param name="modManifest">The mod manifest</param>
		/// <param name="monitor">The mod monitor for logging</param>
		/// <param name="getConfig">Function to get the current config</param>
		/// <param name="setConfig">Function to set the config</param>
		public ConfigMenuManager(IModHelper helper, IManifest modManifest, IMonitor monitor,
			System.Func<ModConfig> getConfig, System.Action<ModConfig> setConfig)
		{
			this.helper = helper;
			this.modManifest = modManifest;
			this.monitor = monitor;
			this.getConfig = getConfig;
			this.setConfig = setConfig;
		}

		/// <summary>
		/// Sets up the Generic Mod Config Menu integration
		/// </summary>
		public void SetupConfigMenu()
		{
			// Get Generic Mod Config Menu's API (if it's installed)
			var configMenu = helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");

			// Try alternative UniqueID if the first one doesn't work
			configMenu ??= helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("GenericModConfigMenu");
			if (configMenu is null)
			{
				monitor.Log("Generic Mod Config Menu not found. Make sure it's installed for in-game configuration.", LogLevel.Info);
				return;
			}

			// Register mod
			configMenu.Register(
				mod: modManifest,
				reset: () => setConfig(new ModConfig()),
				save: () => helper.WriteConfig(getConfig())
			);

			// Add configuration options
			AddConfigOptions(configMenu);
		}

		/// <summary>
		/// Adds all configuration options to the config menu
		/// </summary>
		/// <param name="configMenu">The Generic Mod Config Menu API instance</param>
		private void AddConfigOptions(IGenericModConfigMenuApi configMenu)
		{
			configMenu.AddBoolOption(
				mod: modManifest,
				name: () => "Require Modifier Key",
				tooltip: () => "If enabled, you must hold the modifier key while right-clicking to instantly eat food.",
				getValue: () => getConfig().RequireModifier,
				setValue: value => { var config = getConfig(); config.RequireModifier = value; setConfig(config); }
			);

			configMenu.AddKeybind(
				mod: modManifest,
				name: () => "Modifier Key",
				tooltip: () => "The key to hold when 'Require Modifier Key' is enabled.",
				getValue: () => getConfig().ModifierKey,
				setValue: value => { var config = getConfig(); config.ModifierKey = value; setConfig(config); }
			);

			configMenu.AddBoolOption(
				mod: modManifest,
				name: () => "Allow When Full",
				tooltip: () => "If enabled, you can eat food even when your health and energy are already full.",
				getValue: () => getConfig().AllowWhenFull,
				setValue: value => { var config = getConfig(); config.AllowWhenFull = value; setConfig(config); }
			);

			configMenu.AddBoolOption(
				mod: modManifest,
				name: () => "Play Eat Sound",
				tooltip: () => "If enabled, plays the eating sound effect when instantly consuming food.",
				getValue: () => getConfig().PlayEatSound,
				setValue: value => { var config = getConfig(); config.PlayEatSound = value; setConfig(config); }
			);

			configMenu.AddBoolOption(
				mod: modManifest,
				name: () => "Show Health/Energy Gain",
				tooltip: () => "If enabled, shows floating text indicating how much health and energy you gained.",
				getValue: () => getConfig().ShowHealthGain,
				setValue: value => { var config = getConfig(); config.ShowHealthGain = value; setConfig(config); }
			);

			configMenu.AddBoolOption(
				mod: modManifest,
				name: () => "Show Quick Consume Dialog",
				tooltip: () => "If enabled, shows a message indicating which item was quickly consumed.",
				getValue: () => getConfig().ShowQuickConsumeDialog,
				setValue: value => { var config = getConfig(); config.ShowQuickConsumeDialog = value; setConfig(config); }
			);
		}
	}
}
