using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace SpawnSheet
{
	class SpawnSheetClientConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;

		[DefaultValue(false)]
		[Label("Hotbar Shown by Default")]
		[Tooltip("Allows the hotbar to default to being shown rather than hidden.")]
		public bool HotbarShownByDefault { get; set; }
	}
}
