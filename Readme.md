# Quick Consume 🍎

A **Stardew Valley** mod that allows you to instantly consume edible items without the confirmation dialog or eating animation.

## Features

- **Instant consumption** - Right-click any edible item to consume it immediately
- **No confirmation dialog** - Skip the "Do you want to eat this?" prompt for all consumables
- **Full compatibility** - Works with all foods including buffed items (preserves all effects)
- **Energy and health messages** - See floating text showing how much you gained
- **Sound effects** - Optional eating sound when consuming items
- **Configurable** - Customize all settings through the in-game mod config menu

## Installation

1. **Install SMAPI** - Download from [smapi.io](https://smapi.io/)
2. **Download this mod** - Get the latest release from [Nexus Mods](#) or GitHub
3. **Extract to Mods folder** - Unzip the mod into `Stardew Valley/Mods/QuickConsume/`
4. **Run the game** - Launch Stardew Valley through SMAPI

## Configuration

### In-Game Configuration (Recommended)

1. Install [Generic Mod Config Menu](https://www.nexusmods.com/stardewvalley/mods/5098)
2. Go to the main menu and click the gear icon
3. Find "Quick Consume" in the mod list
4. Configure your preferences with the user-friendly interface

### Manual Configuration

Edit `config.json` in the mod folder:

```json
{
  "RequireModifier": false,
  "ModifierKey": "LeftControl",
  "AllowWhenFull": true,
  "PlayEatSound": true,
  "ShowHealthGain": true
}
```

### Configuration Options

| Setting           | Description                                 | Default       |
| ----------------- | ------------------------------------------- | ------------- |
| `RequireModifier` | Must hold modifier key to quick-consume     | `false`       |
| `ModifierKey`     | Key to hold when RequireModifier is true    | `LeftControl` |
| `AllowWhenFull`   | Allow consuming when health/energy are full | `true`        |
| `PlayEatSound`    | Play eating sound effect                    | `true`        |
| `ShowHealthGain`  | Show "+X Energy +Y Health" messages         | `true`        |

## Usage

1. **Hold any edible item** in your hands
2. **Right-click** to instantly consume it
   - If `RequireModifier` is enabled, hold the modifier key while right-clicking
3. **See the results** - Energy/health restored instantly, floating messages show gains, and any buffs are applied

## Technical Details

### Requirements

- **[SMAPI 4.0.0+](https://smapi.io/)** - Stardew Valley modding framework
- **[Stardew Valley 1.5+](https://store.steampowered.com/app/413150/Stardew_Valley/)** - Base game
- **[.NET 6.0](https://dotnet.microsoft.com/download/dotnet/6.0)** (included with game)

### Optional Dependencies

- **[Generic Mod Config Menu](https://www.nexusmods.com/stardewvalley/mods/5098)** - For in-game configuration

### Compatibility

- ✅ **Save-safe** - Can be added/removed without affecting save files
- ⚠️ **Cross-platform** - Windows, Mac, Linux (built with cross-platform code, but primarily tested on Windows)
- ❓ **Multiplayer** - Not extensively tested in multiplayer. Each player needs the mod installed. May work but could cause desyncs.

## Development

### Building from Source

```bash
dotnet build
```

### Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly
5. Submit a pull request

## Support

- **Report bugs** - Use the [GitHub Issues](https://github.com/p-v-z/QuickConsume/issues) page
- **Suggest features** - Open a feature request on GitHub

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Credits

- **Author**: StyGGz
- **Framework**: [SMAPI](https://smapi.io/) by [Pathoschild](https://github.com/Pathoschild)
- **Build Config**: [ModBuildConfig](https://github.com/Pathoschild/Stardew.ModBuildConfig) by [Pathoschild](https://github.com/Pathoschild)
- **Config Menu**: [Generic Mod Config Menu](https://www.nexusmods.com/stardewvalley/mods/5098) by [spacechase0](https://github.com/spacechase0)

## Acknowledgments

Thanks to [ConcernedApe](https://twitter.com/ConcernedApe) for creating Stardew Valley and providing an excellent modding framework. Special thanks to the active modding community for maintaining documentation and development tools.
