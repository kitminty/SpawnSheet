using SpawnSheet.Menus;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;

namespace SpawnSheet
{
	internal class SpawnSheetWorld : ModSystem
	{
		internal static string CSText(string key, string category = "ExtraAccessorySlots") => SpawnSheet.CSText(category, key);
		public override void OnWorldLoad() {
			if (!Main.dedServ && Main.LocalPlayer.name != "") {
				try {
					SpawnSheet.instance.hotbar.bCycleExtraAccessorySlots.Tooltip = CSText("ExtraAccessorySlots") + " " + Main.LocalPlayer.GetModPlayer<SpawnSheetPlayer>().numberExtraAccessoriesEnabled;
					SpawnSheet.instance.paintToolsHotbar.UndoHistory.Clear();
					SpawnSheet.instance.paintToolsHotbar.UpdateUndoTooltip();
				}
				catch (Exception e) {
					SpawnSheetUtilities.ReportException(e);
				}
			}

			//    SpawnSheet.instance.hotbar.ChangedBossDowner();
		}

		public override void OnWorldUnload() {
			if (!Main.dedServ && Main.LocalPlayer.name != "") {
				try {
					SpawnSheet.instance.hotbar.bCycleExtraAccessorySlots.Tooltip = CSText("ExtraAccessorySlots") + " " + Main.LocalPlayer.GetModPlayer<SpawnSheetPlayer>().numberExtraAccessoriesEnabled;
					SpawnSheet.instance.paintToolsHotbar.UndoHistory.Clear();
					SpawnSheet.instance.paintToolsHotbar.UpdateUndoTooltip();
				}
				catch (Exception e) {
					SpawnSheetUtilities.ReportException(e);
				}
			}

			//    SpawnSheet.instance.hotbar.ChangedBossDowner();
		}

		public override void NetSend(BinaryWriter writer) {
			writer.Write7BitEncodedInt((int)NPCBrowser.filteredNPCSlots.Count);
			foreach (var item in NPCBrowser.filteredNPCSlots) {
				writer.Write7BitEncodedInt((int)item);
			}
		}

		public override void NetReceive(BinaryReader reader) {
			NPCBrowser.filteredNPCSlots.Clear();
			int numFiltered = reader.Read7BitEncodedInt();
			for (int i = 0; i < numFiltered; i++) {
				NPCBrowser.filteredNPCSlots.Add(reader.Read7BitEncodedInt());
			}
			NPCBrowser.needsUpdate = true;
		}

		public override void UpdateUI(GameTime gameTime) {
			base.UpdateUI(gameTime);

			if (Main.netMode == 1 && ModContent.GetInstance<SpawnSheetServerConfig>().DisableCheatsForNonHostUsers && !SpawnSheet.IsPlayerLocalServerOwner(Main.LocalPlayer))
				return;

			if (PaintToolsEx.schematicsToLoad != null && SpawnSheet.instance.numberOnlineToLoad > 0 && SpawnSheet.instance.paintToolsUI.view.childrenToRemove.Count == 0) {
				PaintToolsEx.LoadSingleSchematic();
				//SpawnSheet.instance.paintToolsUI.view.ReorderSlots();
			}

			if (PaintToolsSlot.updateNeeded) {
				bool oneUpdated = false;
				foreach (var item in SpawnSheet.instance.paintToolsUI.view.slotList) {
					if (item.texture == TextureAssets.MagicPixel.Value) {
						item.texture = item.MakeThumbnail(item.stampInfo);
						oneUpdated = true;
						break;
					}
				}
				if (!oneUpdated)
					PaintToolsSlot.updateNeeded = false;
			}
		}

		//public override void PostDrawFullscreenMap(ref string mouseText)
		//{
		//	Main.spriteBatch.DrawString(FontAssets.MouseText.Value, "Testing Testing", new Vector2(Main.screenWidth / 2, Main.screenHeight / 2), Color.Pink, 0.0f, new Vector2(), 0.8f, SpriteEffects.None, 0.0f);
		//}

		private int lastmode = -1;

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
			if (Main.netMode == 1 && ModContent.GetInstance<SpawnSheetServerConfig>().DisableCheatsForNonHostUsers && !SpawnSheet.IsPlayerLocalServerOwner(Main.LocalPlayer))
				return;

			if (Main.netMode != lastmode) {
				lastmode = Main.netMode;
				if (Main.netMode == 0) {
					SpawnRateMultiplier.HasPermission = true;
					foreach (var key in SpawnSheet.instance.herosPermissions.Keys.ToList()) {
						SpawnSheet.instance.herosPermissions[key] = true;
					}
				}
				SpawnSheet.instance.hotbar.ChangedConfiguration();
			}
			int MouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if (MouseTextIndex != -1) {
				layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
					"SpawnSheet: All Cheat Sheet",
					delegate {
						ModContent.GetInstance<AllItemsMenu>().DrawUpdateAll(Main.spriteBatch);
						return true;
					},
					InterfaceScaleType.UI)
				);

				layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
					"SpawnSheet: Paint Tools",
					delegate {
						ModContent.GetInstance<AllItemsMenu>().DrawUpdatePaintTools(Main.spriteBatch);
						return true;
					},
					InterfaceScaleType.Game)
				);
			}

			MouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
			if (MouseTextIndex != -1) {
				layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
					"SpawnSheet: Extra Accessories",
					delegate {
						ModContent.GetInstance<AllItemsMenu>().DrawUpdateExtraAccessories(Main.spriteBatch);
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}
	}
}