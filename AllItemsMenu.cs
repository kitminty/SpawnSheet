using SpawnSheet.Menus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;

namespace SpawnSheet
{
	internal class AllItemsMenu : GlobalItem
	{
		internal static Item[] singleSlotArray;

		public AllItemsMenu() {
			singleSlotArray = new Item[1];
		}

		//internal void UpdateInput()
		//{
		//	try
		//	{
		//		UIView.UpdateUpdateInput();
		//		SpawnSheet.instance.npcBrowser.Update();
		//		SpawnSheet.instance.itemBrowser.Update();
		//		SpawnSheet.instance.recipeBrowser.Update();
		//		SpawnSheet.instance.extendedCheatMenu.Update();

		//		SpawnSheet.instance.hotbar.Update();
		//		SpawnSheet.instance.paintToolsHotbar.Update();
		//		SpawnSheet.instance.quickTeleportHotbar.Update();
		//		SpawnSheet.instance.quickClearHotbar.Update();
		//		SpawnSheet.instance.npcButchererHotbar.Update();
		//		ConfigurationTool.configurationWindow.Update();
		//	}
		//	catch (Exception e)
		//	{
		//		ErrorLogger.Log(e.Message + " " + e.StackTrace);
		//	}
		//}

		public void DrawUpdateAll(SpriteBatch spriteBatch) {
			SpawnSheet.instance.itemBrowser.Draw(spriteBatch);
			SpawnSheet.instance.npcBrowser.Draw(spriteBatch);
			SpawnSheet.instance.recipeBrowser.Draw(spriteBatch);
			SpawnSheet.instance.extendedCheatMenu.Draw(spriteBatch);
			SpawnSheet.instance.paintToolsUI.Draw(spriteBatch);

			//			SpawnSheet.instance.itemBrowser.Update();
			//	spriteBatch.End();
			//	spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

			SpawnSheet.instance.npcBrowser.Update();
			SpawnSheet.instance.itemBrowser.Update();
			SpawnSheet.instance.recipeBrowser.Update();
			SpawnSheet.instance.extendedCheatMenu.Update();

			SpawnSheet.instance.hotbar.Update();
			SpawnSheet.instance.paintToolsHotbar.Update();
			SpawnSheet.instance.paintToolsUI.Update();
			SpawnSheet.instance.quickTeleportHotbar.Update();
			SpawnSheet.instance.quickClearHotbar.Update();
			SpawnSheet.instance.npcButchererHotbar.Update();
			ConfigurationTool.configurationWindow.Update();
			//BossDowner.bossDownerWindow.Update();
			//SpawnSheet.instance.eventManagerHotbar.Update();

			SpawnSheet.instance.hotbar.Draw(spriteBatch);
			SpawnSheet.instance.paintToolsHotbar.Draw(spriteBatch);
			SpawnSheet.instance.quickTeleportHotbar.Draw(spriteBatch);
			SpawnSheet.instance.quickClearHotbar.Draw(spriteBatch);
			SpawnSheet.instance.npcButchererHotbar.Draw(spriteBatch);
			ConfigurationTool.configurationWindow.Draw(spriteBatch);
			//BossDowner.bossDownerWindow.Draw(spriteBatch);
			//SpawnSheet.instance.eventManagerHotbar.Draw(spriteBatch);

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Main.UIScaleMatrix);

			//	DrawUpdateExtraAccessories(spriteBatch);
		}

		public void DrawUpdatePaintTools(SpriteBatch spriteBatch) {
			SpawnSheet.instance.paintToolsHotbar.UpdateGameScale();
			SpawnSheet.instance.paintToolsHotbar.DrawGameScale(spriteBatch);
		}

		internal void DrawUpdateExtraAccessories(SpriteBatch spriteBatch) {
			if (Main.playerInventory && Main.EquipPage == 0) {
				Point value = new Point(Main.mouseX, Main.mouseY);
				Rectangle r = new Rectangle(0, 0, (int)((float)TextureAssets.InventoryBack.Value.Width * Main.inventoryScale), (int)((float)TextureAssets.InventoryBack.Value.Height * Main.inventoryScale));

				SpawnSheetPlayer csp = Main.LocalPlayer.GetModPlayer<SpawnSheetPlayer>();
				for (int i = 0; i < csp.numberExtraAccessoriesEnabled; i++) {
					Main.inventoryScale = 0.85f;
					Item accItem = csp.ExtraAccessories[i];
					//if (accItem.type > 0)
					//{
					//	ErrorLogger.Log("aaa " + i + " " + accItem.type);
					//}

					int mH = 0;
					if (Main.mapEnabled) {
						if (!Main.mapFullscreen && Main.mapStyle == 1) {
							mH = 256;
						}
						if (mH + 600 > Main.screenHeight) {
							mH = Main.screenHeight - 600;
						}
					}

					int num17 = Main.screenWidth - 92 - (47 * 3);
					int num18 = /*Main.mH +*/mH + 174;
					if (Main.netMode == 1)
						num17 -= 47;
					r.X = num17/* + l * -47*/;
					r.Y = num18 + (0 + i) * 47;

					if (r.Contains(value)/* && !flag2*/) {
						Main.LocalPlayer.mouseInterface = true;
						Main.armorHide = true;
						singleSlotArray[0] = accItem;
						ItemSlot.Handle(singleSlotArray, ItemSlot.Context.EquipAccessory, 0);
						accItem = singleSlotArray[0];
						//ItemSlot.Handle(ref accItem, ItemSlot.Context.EquipAccessory);
					}
					singleSlotArray[0] = accItem;
					ItemSlot.Draw(spriteBatch, singleSlotArray, 10, 0, new Vector2(r.X, r.Y));
					accItem = singleSlotArray[0];

					//ItemSlot.Draw(spriteBatch, ref accItem, 10, new Vector2(r.X, r.Y));

					csp.ExtraAccessories[i] = accItem;
					//	ErrorLogger.Log("pd");
					//player.VanillaUpdateAccessory(csp.ExtraAccessories[i], false, ref wallSpeedBuff, ref tileSpeedBuff, ref tileRangeBuff);
				}
			}
		}
	}
}