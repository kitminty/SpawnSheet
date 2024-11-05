using Terraria.ModLoader;

namespace SpawnSheet
{
	internal class SpawnSheetSystem : ModSystem
	{
		public override void PostSetupRecipes() {
			SpawnSheet.instance.SetupUI();
		}
	}
}
