# Quick Consume 🍎

A Stardew Valley mod that allows you to instantly consume regular edible items without the confirmation dialog or eating animation. Buffed consumables use normal game consumption to preserve their buff effects.

## Features

- **Instant consumption** - Right-click any non-buffed edible item to consume it immediately
- **No confirmation dialog** - Skip the "Do you want to eat this?" prompt for regular consumables
- **Energy and health messages** - See floating text showing how much you gained
- **Sound effects** - Optional eating sound when consuming items
- **Smart buff detection** - Buffed consumables show a message and use normal consumption to preserve buffs
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
  "ModifierKey": "LeftShift",
  "AllowWhenFull": true,
  "PlayEatSound": true,
  "ShowHealthGain": true
}
```

### Configuration Options

| Setting           | Description                                 | Default     |
| ----------------- | ------------------------------------------- | ----------- |
| `RequireModifier` | Must hold modifier key to quick-consume     | `false`     |
| `ModifierKey`     | Key to hold when RequireModifier is true    | `LeftShift` |
| `AllowWhenFull`   | Allow consuming when health/energy are full | `true`      |
| `PlayEatSound`    | Play eating sound effect                    | `true`      |
| `ShowHealthGain`  | Show "+X Energy +Y Health" messages         | `true`      |

## Usage

1. **Hold any edible item** in your hands
2. **Right-click** to instantly consume it
   - If `RequireModifier` is enabled, hold the modifier key while right-clicking
   - **Important**: Buffed consumables (like Coffee, Lucky Lunch, Spicy Eel, etc.) will show "Buffed consumables can't be consumed quickly" and require normal consumption to preserve their buff effects
3. **See the results** - Energy/health restored instantly, floating messages show gains

### Compatible Items

- **Regular consumables**: Fruits, vegetables, foraged items, basic food
- **Any edible item** with positive edibility value that doesn't provide stat buffs
- **Examples**: Parsnips, Berries, Fish, Milk, Basic Meals

### Non-Compatible Items (Use Normal Consumption)

- **Buffed consumables**: Items that provide temporary stat bonuses
- **Examples**: Coffee (+Speed), Lucky Lunch (+Luck), Spicy Eel (+Luck +Speed), Pumpkin Soup (+Defense +Luck)
- **Reason**: These items use normal game consumption to ensure buffs are properly applied without conflicts

## Technical Details

### Requirements

- **SMAPI 4.0.0+**
- **Stardew Valley 1.5+**
- **.NET 6.0** (included with game)

### Optional Dependencies

- **Generic Mod Config Menu** - For in-game configuration

### Compatibility

- ✅ **Multiplayer compatible** - Works for all players who have the mod installed
- ✅ **Save-safe** - Can be added/removed without affecting save files
- ✅ **Cross-platform** - Windows, Mac, Linux

## Development

### Building from Source

```bash
git clone https://github.com/yourusername/QuickConsume.git
cd QuickConsume
dotnet build
```

### Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly
5. Submit a pull request

## Changelog

### Version 1.0.0

- Initial release
- Instant consumption for regular (non-buffed) consumables
- Energy/health gain messages
- Generic Mod Config Menu integration
- Configurable modifier key support
- Sound effects and visual feedback
- Smart buff detection prevents conflicts with buffed consumables

## Support

- **Report bugs** - Use the [GitHub Issues](https://github.com/yourusername/QuickConsume/issues) page
- **Get help** - Ask questions in the [Stardew Valley Discord](https://discord.gg/StardewValley)
- **Suggest features** - Open a feature request on GitHub

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Credits

- **Author**: StyGGz
- **Framework**: [SMAPI](https://smapi.io/) by Pathoschild
- **Build Config**: [ModBuildConfig](https://github.com/Pathoschild/Stardew.ModBuildConfig) by Pathoschild
- **Config Menu**: [Generic Mod Config Menu](https://www.nexusmods.com/stardewvalley/mods/5098) by spacechase0

## Acknowledgments

Thanks to the Stardew Valley modding community for their excellent documentation and helpful tools that made this mod possible.
