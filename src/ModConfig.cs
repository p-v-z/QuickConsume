using StardewModdingAPI;

namespace QuickConsume
{
	public class ModConfig
	{
		public bool RequireModifier { get; set; } = false;      // set true if you want to hold a key to instant-eat
		public SButton ModifierKey { get; set; } = SButton.LeftControl;
		public bool AllowWhenFull { get; set; } = true;          // allow eating even when full
		public bool PlayEatSound { get; set; } = true;           // play the eating sound effect
		public bool ShowHealthGain { get; set; } = true;         // show floating text for health/energy gained
		public bool ShowQuickConsumeDialog { get; set; } = true; // show "Quickly consumed X" message
	}

}