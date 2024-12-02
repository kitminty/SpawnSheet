using SpawnSheet.UI;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpawnSheet.Menus
{
	internal class SpawnRateMultiplierGlobalNPC : GlobalNPC
	{
		public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns) {
			spawnRate = (int)(spawnRate / SpawnRateMultiplier.currentMultiplier);
			maxSpawns = (int)(maxSpawns * SpawnRateMultiplier.currentMultiplier);
		}
	}

	internal class SpawnRateMultiplier
	{
		internal static string CSText(string key, string category = "SpawnRate") => SpawnSheet.CSText(category, key);
		private static float[] multipliers = new float[] { 1f, 5f, 10f, 50f, 100f, 500f, 1000f };
		private static string[] multiplierStrings = new string[] { "1x", "5x", "10x", "50x", "100x", "500x", "1000x" };
		public static float currentMultiplier = 1f;
		private static int currentMultiplierIndex = 2;
		public static UIImage button;
		public static bool HasPermission = true;

		public static UIImage GetButton(Mod mod) {
			button = new UIImage(ModUtils.GetItemTexture(ItemID.WaterCandle));

			button.Tooltip = CSText("SpawnRateMultiplier");
			button.onRightClick += (s, e) => {
				buttonLogic(false);
			};
			button.onLeftClick += (s, e) => {
				buttonLogic(true);
			};
			button.ForegroundColor = Color.White;
			return button;
		}

		public static void buttonLogic(bool leftMouse) {
			int newIndex = leftMouse ? (currentMultiplierIndex + 1) % multiplierStrings.Length : (multiplierStrings.Length + currentMultiplierIndex - 1) % multiplierStrings.Length;

			if (Main.netMode == 1) {
				RequestSetSpawnRate(newIndex);
			}
			else {
				ChangeSettingLogic(newIndex);
			}
		}

		public static void ChangeSettingLogic(int newSetting) {
			currentMultiplierIndex = newSetting;
			currentMultiplier = multipliers[currentMultiplierIndex];
			if (!Main.dedServ) {
				button.Tooltip = CSText("SpawnRateMultiplierNew") + " " + multiplierStrings[currentMultiplierIndex];
				Main.NewText(CSText("SpawnRateMultiplierText") + " " + multiplierStrings[currentMultiplierIndex] + " " + CSText("SpawnRateNormalValue"));
			}
		}

		// Action Taken by Client Button
		internal static void RequestSetSpawnRate(int index) {
			var netMessage = SpawnSheet.instance.GetPacket();
			netMessage.Write((byte)SpawnSheetMessageType.SetSpawnRate);
			netMessage.Write(index);
			netMessage.Send();
		}

		// Action taken by sever receiving button
		internal static void HandleSetSpawnRate(BinaryReader reader, int whoAmI) {
			// TODO: Don't just trust UI? Lazy.

			int newSetting = reader.ReadInt32();
			ChangeSettingLogic(newSetting);

			var netMessage = SpawnSheet.instance.GetPacket();
			netMessage.Write((byte)SpawnSheetMessageType.SpawnRateSet);
			netMessage.Write(newSetting);
			netMessage.Send();
		}

		// Action taken by client receiving button
		internal static void HandleSpawnRateSet(BinaryReader reader, int whoAmI) {
			int newSetting = reader.ReadInt32();
			ChangeSettingLogic(newSetting);
		}

		// HEROS MOD INTEGRATION
		// This method is called when the cursor is hovering over the button in Heros mod or Cheat Sheet
		public static string HEROsTooltip() {
			return "Spawn Rate Multiplier: " + multiplierStrings[currentMultiplierIndex];
		}

		// This method is called when the button is pressed using Heros mod or Cheat Sheet
		public static void HEROsButtonPressed() {
			buttonLogic(true);
		}

		// This method is called when Permissions change while using HERO's Mod.
		// We need to make sure to disable instantRespawn when we are no longer allowed to use the tool.
		public static void HEROsPermissionChanged(bool hasPermission) {
			HasPermission = hasPermission;

			SpawnSheet.instance.hotbar.ChangedConfiguration();
		}
	}
}