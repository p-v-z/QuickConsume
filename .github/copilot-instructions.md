# GitHub Copilot Instructions

## Project Overview

QuickConsume is a **Stardew Valley mod** built with C# .NET 6.0 and SMAPI framework. It allows players to instantly consume regular edible items without confirmation dialogs or eating animations, while preserving buff effects for special consumables.

## Core Functionality

- **Instant consumption** - Right-click non-buffed edible items for immediate consumption
- **Buff preservation** - Automatically detects buffed consumables and uses normal game consumption
- **Configuration system** - In-game config menu support via Generic Mod Config Menu
- **Visual feedback** - Energy/health gain messages and optional sound effects
- **Smart item detection** - Distinguishes between regular and buffed consumables

## Code Style Guidelines

- Follow C# naming conventions (PascalCase for classes and methods, camelCase for variables)
- Use SMAPI coding patterns and best practices
- Add XML documentation comments for public methods and classes
- Handle edge cases gracefully (null checks, invalid game states)
- Use SMAPI's logging system for debug information

## Commit Message Guidelines

- **Write commit messages as a single sentence**
- Start with a verb in imperative mood (Add, Fix, Update, Remove, etc.)
- Keep the message concise and descriptive
- Examples:
  - "Add buff detection logic for consumable items"
  - "Fix null reference exception when player inventory is empty"
  - "Update manifest version to support SMAPI 4.0+"
  - "Remove deprecated eating animation override"

## Project Structure

- `src/ModEntry.cs` - Main SMAPI mod entry point and event handlers
- `src/BuffData.cs` - Buff detection and consumable categorization logic
- `manifest.json` - SMAPI mod manifest with dependencies and metadata
- `config.json` - User configuration file (auto-generated)
- `Readme.md` - Comprehensive documentation and usage guide

## Stardew Valley Modding Context

- **SMAPI framework** - Use SMAPI APIs for game interaction and event handling
- **Game compatibility** - Target Stardew Valley 1.5+ with SMAPI 4.0+
- **Save-safe design** - Ensure mod can be added/removed without corrupting saves
- **Multiplayer considerations** - Handle potential desyncs and per-player mod requirements
- **Cross-platform support** - Write platform-agnostic code for Windows/Mac/Linux

## Key Technical Considerations

- **Item edibility validation** - Check item.Edibility > 0 for consumable detection
- **Buff detection** - Identify items with temporary stat bonuses or special effects
- **Input handling** - Right-click detection with optional modifier key support
- **Game state checks** - Validate player can consume (not full health/energy if configured)
- **SMAPI events** - Use appropriate event handlers for input and game state changes

## Dependencies and Integration

- **Required**: SMAPI 4.0.0+, Stardew Valley 1.5+, .NET 6.0
- **Optional**: Generic Mod Config Menu for in-game configuration
- **Build tools**: ModBuildConfig for automated SMAPI mod packaging
