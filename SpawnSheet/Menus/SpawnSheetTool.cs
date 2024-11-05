using SpawnSheet.UI;
using Terraria.ModLoader;

namespace SpawnSheet.Menus
{
	abstract class SpawnSheetTool
	{
		public SpawnSheetTool(Mod mod) {

		}

		public static UIImage hotbarButton;

		public abstract UIImage GetButton(Mod mod);
	}
}
