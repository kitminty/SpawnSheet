using SpawnSheet.CustomUI;
using SpawnSheet.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpawnSheet.Menus
{
	internal class Hotbar : UIWindow
	{
		internal static string CSText(string key, string category = "Hotbar") => SpawnSheet.CSText(category, key);

		private static float moveSpeed = 8f;

		public static float xPosition = 78f;

		public bool hidden;
		public static bool pVac = false;
		public static int pVacID = 0;

		private float lerpAmount;

		private float spacing = 8f;

		internal UIHotbar currentHotbar;

		//public static Texture2D mapTexture;
		public static Asset<Texture2D> loginTexture;
		public static Asset<Texture2D> logoutTexture;

		public UIView buttonView;

		private UIImage arrow;
		internal UIImage button;

		//		public UIImage bToggleEnemies;
		//		public UIImage bToggleBlockReach;
		//		public UIImage bFlyCamera;

		//		public UIImage bRevealMap;
		//		public UIImage bWaypoints;
		//		public UIImage bGroupManager;
		//		public UIImage bOnlinePlayers;
		//		public UIImage bTime;

		//		public UIImage bWeatherWindow;
		//		public UIImage bBackupWorld;
		//		public UIImage bCTFSettings;
		//		public UIImage bLogin;

		public UIImage bToggleRecipeBrowser;
		// public UIImage bSpawnRateMultiplier;
		//public UIImage bToggleEventManager;

		//private static Color buttonUnselectedColor = Color.White;
		internal static Color buttonUnselectedColor = Color.LightSkyBlue;

		internal static Color buttonSelectedColor = Color.White;

		//private static Color buttonSelectedColor = Color.LightSkyBlue;
		internal static Color buttonSelectedHiddenColor = Color.Blue;

		public static float disabledOpacity = 0.5f;
		private SpawnSheet mod;

		public Vector2 chatOffset {
			get {
				if (base.Visible) {
					return new Vector2(0f, base.Position.Y - (float)Main.screenHeight - this.arrow.Height);
				}
				return Vector2.Zero;
			}
		}

		public float shownPosition {
			get {
				return (float)Main.screenHeight - base.Height - 12f;
			}
		}

		public float hiddenPosition {
			get {
				return (float)Main.screenHeight;
			}
		}

		public Hotbar(SpawnSheet mod) {
			this.mod = mod;
			this.buttonView = new UIView();
			//	this.timeWindow = new TimeControlWindow();
			//	this.npcSpawnWindow = new NPCSpawnerWindow();
			//	this.weatherWindow = new WeatherControlWindow();
			//	this.timeWindow.Visible = false;
			//	this.npcSpawnWindow.Visible = false;
			//	this.weatherWindow.Visible = false;
			//	this.AddChild(this.timeWindow);
			//	this.AddChild(this.npcSpawnWindow);
			//	this.AddChild(this.weatherWindow);
			Hotbar.loginTexture = mod.Assets.Request<Texture2D>("UI/Images.login", ReLogic.Content.AssetRequestMode.ImmediateLoad);// UIView.GetEmbeddedTexture("Images.login.png");
			Hotbar.logoutTexture = mod.Assets.Request<Texture2D>("UI/Images.logout", ReLogic.Content.AssetRequestMode.ImmediateLoad); //UIView.GetEmbeddedTexture("Images.logout.png");
																																	  //	this.bLogin = new UIImage(Hotbar.loginTexture);
																																	  //		bLogin = new UIImage(mod.Assets.Request<Texture2D>("UI/Images.login"));
			base.Visible = false;
			base.UpdateWhenOutOfBounds = true;
			//	Hotbar.groupWindow = new GroupManagementWindow();
			this.button = new UIImage(mod.Assets.Request<Texture2D>("UI/Images.CollapseBar.CollapseButtonHorizontal", ReLogic.Content.AssetRequestMode.ImmediateLoad));//new UIImage(UIView.GetEmbeddedTexture("Images.CollapseBar.CollapseButtonHorizontal.png"));

			this.button.UpdateWhenOutOfBounds = true;
			this.arrow = new UIImage(mod.Assets.Request<Texture2D>("UI/Images.CollapseBar.CollapseArrowHorizontal", ReLogic.Content.AssetRequestMode.ImmediateLoad));  //new UIImage(UIView.GetEmbeddedTexture("Images.CollapseBar.CollapseArrowHorizontal.png"));


			//		bToggleEnemies = new UIImage(mod.Assets.Request<Texture2D>("UI/Images.npcIcon"));
			//		bToggleBlockReach = new UIImage(ModUtils.GetItemTexture(407));

			//		bFlyCamera = new UIImage(ModUtils.GetItemTexture(493));

			//		bRevealMap = new UIImage(mod.Assets.Request<Texture2D>("UI/Images.canIcon"));// Hotbar.mapTexture);
			//		bWaypoints = new UIImage(mod.Assets.Request<Texture2D>("UI/Images.waypointIcon"));
			//		bGroupManager = new UIImage(mod.Assets.Request<Texture2D>("UI/Images.manageGroups"));
			//		bOnlinePlayers = new UIImage(mod.Assets.Request<Texture2D>("UI/Images.connectedPlayers"));
			//		bTime = new UIImage(mod.Assets.Request<Texture2D>("UI/Images.sunIcon"));
			//		bWeatherWindow = new UIImage(Main.npcHeadTexture[2]);// WeatherControlWindow.rainTexture);
			//		bBackupWorld = new UIImage(mod.Assets.Request<Texture2D>("UI/Images.UIKit.saveIcon"));
			//		bCTFSettings = new UIImage(mod.Assets.Request<Texture2D>("UI/Images.CTF.redFlag"));

			//	Main.instance.LoadNPC(NPCID.KingSlime);

			bToggleRecipeBrowser = new UIImage(ModUtils.GetItemTexture(ItemID.CookingPot));

			//bToggleEventManager = new UIImage(ModUtils.GetItemTexture(ItemID.PirateMap));


			this.arrow.UpdateWhenOutOfBounds = true;
			this.button.Anchor = AnchorPosition.Top;
			this.arrow.Anchor = AnchorPosition.Top;
			this.arrow.SpriteEffect = SpriteEffects.FlipVertically;
			this.AddChild(this.button);
			this.AddChild(this.arrow);
			this.button.Position = new Vector2(0f, -this.button.Height);
			this.button.CenterXAxisToParentCenter();
			//Do i need this?		this.button.X -= 40;
			this.arrow.Position = this.button.Position;
			this.arrow.onLeftClick += new EventHandler(this.button_onLeftClick);
			//		this.bBackupWorld.onLeftClick += new EventHandler(this.bBackupWorld_onLeftClick);
			//		this.bToggleBlockReach.Tooltip = "Toggle Block Reach";
			//		this.bToggleEnemies.Tooltip = "Toggle Enemy Spawns";
			//		this.bFlyCamera.Tooltip = "Toggle Fly Cam";
			//		this.bRevealMap.Tooltip = "Reveal Map";
			//		this.bWaypoints.Tooltip = "Open Waypoints Window";
			//		this.bGroupManager.Tooltip = "Open Group Management";
			//		this.bOnlinePlayers.Tooltip = "View Connected Players";
			//		this.bTime.Tooltip = "Set Time";
			//		this.bWeatherWindow.Tooltip = "Control Rain";
			//		this.bLogin.Tooltip = "Login";
			//		this.bCTFSettings.Tooltip = "Capture the Flag Settings";
			//		this.bBackupWorld.Tooltip = "Backup World";
			bToggleRecipeBrowser.Tooltip = CSText("ShowRecipeBrowser");
			//		bToggleEventManager.Tooltip = "Show Event Manager";

			//		this.bToggleBlockReach.Opacity = Hotbar.disabledOpacity;
			//		this.bFlyCamera.Opacity = Hotbar.disabledOpacity;
			//		this.bToggleEnemies.Opacity = Hotbar.disabledOpacity;
			//		this.bToggleBlockReach.onLeftClick += new EventHandler(this.bToggleBlockReach_onLeftClick);
			//		this.bFlyCamera.onLeftClick += new EventHandler(this.bFlyCamera_onLeftClick);
			//		this.bToggleEnemies.onLeftClick += new EventHandler(this.bToggleEnemies_onLeftClick);
			//			this.bRevealMap.onLeftClick += new EventHandler(this.bRevealMap_onLeftClick);
			//			this.bWaypoints.onLeftClick += new EventHandler(this.bWaypoints_onLeftClick);
			//		this.bGroupManager.onLeftClick += new EventHandler(this.bGroupManager_onLeftClick);
			//		this.bOnlinePlayers.onLeftClick += new EventHandler(this.bOnlinePlayers_onLeftClick);
			//		this.bCTFSettings.onLeftClick += new EventHandler(this.bCTFSettings_onLeftClick);
			//		this.bLogin.onLeftClick += new EventHandler(this.bLogin_onLeftClick);
			//		this.bTime.onLeftClick += new EventHandler(this.bTime_onLeftClick);
			//		this.bWeatherWindow.onLeftClick += new EventHandler(this.bWeatherWindow_onLeftClick);
			this.bToggleRecipeBrowser.onLeftClick += new EventHandler(this.bToggleRecipeBrowser_onLeftClick);
			//		this.bToggleEventManager.onLeftClick += new EventHandler(this.bToggleEventManager_onLeftClick);

			//		this.buttonView.AddChild(this.bToggleBlockReach);
			//		this.buttonView.AddChild(this.bFlyCamera);
			//		this.buttonView.AddChild(this.bToggleEnemies);
			//		this.buttonView.AddChild(this.bRevealMap);
			//		this.buttonView.AddChild(this.bWaypoints);
			//		this.buttonView.AddChild(this.bTime);
			//			this.buttonView.AddChild(this.bWeatherWindow);
			//			this.buttonView.AddChild(this.bGroupManager);
			//			this.buttonView.AddChild(this.bOnlinePlayers);
			//			this.buttonView.AddChild(this.bCTFSettings);
			//			this.buttonView.AddChild(this.bLogin);
			//			this.buttonView.AddChild(this.bBackupWorld);

			buttonView.AddChild(bToggleRecipeBrowser);
			//			buttonView.AddChild(bToggleEventManager);
			buttonView.AddChild(SpawnRateMultiplier.GetButton(mod));
			//	buttonView.AddChild(FullBright.GetButton(mod));
			//			buttonView.AddChild(BossDowner.GetButton(mod));

			base.Width = 200f;
			base.Height = 55f;
			this.buttonView.Height = base.Height;
			base.Anchor = AnchorPosition.Top;
			this.AddChild(this.buttonView);
			base.Position = new Vector2(Hotbar.xPosition, this.hiddenPosition);
			base.CenterXAxisToParentCenter();
			float num = this.spacing;
			for (int i = 0; i < this.buttonView.children.Count; i++) {
				this.buttonView.children[i].Anchor = AnchorPosition.Left;
				this.buttonView.children[i].Position = new Vector2(num, 0f);
				this.buttonView.children[i].CenterYAxisToParentCenter();
				this.buttonView.children[i].Visible = true;
				this.buttonView.children[i].ForegroundColor = buttonUnselectedColor;
				num += this.buttonView.children[i].Width + this.spacing;
			}
			//	Hotbar.groupWindow.Visible = false;
			//	MasterView.gameScreen.AddChild(Hotbar.groupWindow);
			ChangedConfiguration();
			//this.Resize();
			return;
		}

		public override void Draw(SpriteBatch spriteBatch) {
			base.Draw(spriteBatch);

			if (Visible && (IsMouseInside() || button.MouseInside)) {
				Main.LocalPlayer.mouseInterface = true;
				Main.LocalPlayer.cursorItemIconEnabled = false;
			}

			float x = FontAssets.MouseText.Value.MeasureString(UIView.HoverText).X;
			Vector2 vector = new Vector2((float)Main.mouseX, (float)Main.mouseY) + new Vector2(16f);
			if (vector.Y > (float)(Main.screenHeight - 30)) {
				vector.Y = (float)(Main.screenHeight - 30);
			}
			if (vector.X > (float)Main.screenWidth - x) {
				vector.X = (float)(Main.screenWidth - 460);
			}
			Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, UIView.HoverText, vector.X, vector.Y, new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor), Color.Black, Vector2.Zero, 1f);
		}

		private void bToggleRecipeBrowser_onLeftClick(object sender, EventArgs e) {
			UIImage uIImage = (UIImage)sender;
			if (mod.recipeBrowser.selected) {
				mod.recipeBrowser.selected = false;
				//	uIImage.selected = false;
				uIImage.ForegroundColor = buttonUnselectedColor;
			}
			else {
				DisableAllWindows();
				mod.recipeBrowser.selected = true;
				//	uIImage.selected = true;
				uIImage.ForegroundColor = buttonSelectedColor;
			}
			//	DisableAllWindows();
			//	mod.recipeBrowser.Visible = true;
		}

		private void bWeatherWindow_onLeftClick(object sender, EventArgs e) {
			//this.weatherWindow.Visible = !this.weatherWindow.Visible;
			//if (this.bWeatherWindow.Visible)
			//{
			//	this.weatherWindow.X = this.bWeatherWindow.X + this.bWeatherWindow.Width / 2f - this.weatherWindow.Width / 2f;
			//	this.weatherWindow.Y = -this.weatherWindow.Height;
			//}
		}

		private void bToggleNPCBrowser_onLeftClick(object sender, EventArgs e) {
			UIImage uIImage = (UIImage)sender;

			if (mod.npcBrowser.selected) {
				mod.npcBrowser.selected = false;
				//		uIImage.selected = false;
				uIImage.ForegroundColor = buttonUnselectedColor;
			}
			else {
				DisableAllWindows();
				mod.npcBrowser.selected = true;
				//		uIImage.selected = true;
				uIImage.ForegroundColor = buttonSelectedColor;
			}
			//this.npcSpawnWindow.Visible = !this.npcSpawnWindow.Visible;
			//if (this.npcSpawnWindow.Visible)
			//{
			//	this.npcSpawnWindow.X = base.Width / 2f - this.npcSpawnWindow.Width / 2f;
			//	this.npcSpawnWindow.Y = -this.npcSpawnWindow.Height;
			//}
		}

		private void bToggleEventManager_onLeftClick(object sender, EventArgs e) {
			UIImage uIImage = (UIImage)sender;

			if (mod.eventManagerHotbar.selected) {
				mod.eventManagerHotbar.selected = false;
				mod.eventManagerHotbar.Hide();
				uIImage.ForegroundColor = buttonUnselectedColor;
			}
			else {
				DisableAllWindows();
				mod.eventManagerHotbar.selected = true;
				mod.eventManagerHotbar.Show();
				uIImage.ForegroundColor = buttonSelectedColor;
			}
		}

		internal void DisableAllWindows() {
			mod.itemBrowser.selected = false;
			mod.npcBrowser.selected = false;
			mod.recipeBrowser.selected = false;
			//BossDowner.bossDownerWindow.selected = false;
			//mod.eventManagerHotbar.selected = false;

			//bToggleNPCBrowser.selected = false;
			//bToggleRecipeBrowser.selected = false;
			//bToggleExtendedCheat.selected = false;
			//bToggleItemBrowser.selected = false;

			bToggleRecipeBrowser.ForegroundColor = buttonUnselectedColor;
			//BossDowner.button.ForegroundColor = buttonUnselectedColor;
			//bToggleEventManager.ForegroundColor = buttonUnselectedColor;
		}

		private void bCTFSettings_onLeftClick(object sender, EventArgs e) {
			//if (Mod.ctf.GameInProgress || Mod.ctf.inLobby)
			//{
			//	Mod.ctf.ToggleTeamListVisible();
			//	return;
			//}
			//Mod.ctf.ToggleSettingsWindow();
		}

		private void bBackupWorld_onLeftClick(object sender, EventArgs e) {
			//ServerTools.SendTextToServer("¶606", default(Color));
		}

		private void bTime_onLeftClick(object sender, EventArgs e) {
			//this.timeWindow.Visible = !this.timeWindow.Visible;
			//if (this.timeWindow.Visible)
			//{
			//	this.timeWindow.X = this.bTime.X + this.bTime.Width / 2f - this.timeWindow.Width / 2f;
			//	this.timeWindow.Y = -this.timeWindow.Height;
			//}
		}

		private void bOnlinePlayers_onLeftClick(object sender, EventArgs e) {
			//ServerTools.playersWindow.Visible = !ServerTools.playersWindow.Visible;
		}

		private void bGroupManager_onLeftClick(object sender, EventArgs e) {
			//Hotbar.groupWindow.Visible = !Hotbar.groupWindow.Visible;
		}

		private void bLogin_onLeftClick(object sender, EventArgs e) {
			//if (this.bLogin.Tooltip == "Login")
			//{
			//	MasterView.gameScreen.AddChild(new LoginWindow());
			//	return;
			//}
			//ServerTools.SendTextToServer("¶urz", default(Color));
		}

		private void bWaypoints_onLeftClick(object sender, EventArgs e) {
			//Waypoints.ToggleWindow();
		}

		private void bRevealMap_onLeftClick(object sender, EventArgs e) {
			//Creative.RevealMap();
		}

		private void button_onLeftClick(object sender, EventArgs e) {
			if (this.hidden) {
				this.Show();
			}
			else {
				this.Hide();
			}
			//this.hidden = !this.hidden;
			//if (this.hidden)
			//{
			//	//this.timeWindow.Visible = false;
			//	this.arrow.SpriteEffect = SpriteEffects.None;
			//	return;
			//}
			//this.arrow.SpriteEffect = SpriteEffects.FlipVertically;
		}

		private void bToggleEnemies_onLeftClick(object sender, EventArgs e) {
			//Creative.ToggleNPCs(0);
		}

		private void bFlyCamera_onLeftClick(object sender, EventArgs e) {
			//Creative.ToggleFlyCam();
		}

		private void bToggleBlockReach_onLeftClick(object sender, EventArgs e) {
			//Creative.ToggleBlockReach();
		}

		private void bToggleItemBrowser_onLeftClick(object sender, EventArgs e) {
			UIImage uIImage = (UIImage)sender;
			if (mod.itemBrowser.selected) {
				mod.itemBrowser.selected = false;
				uIImage.ForegroundColor = buttonUnselectedColor;
			}
			else {
				DisableAllWindows();
				mod.itemBrowser.selected = true;
				uIImage.ForegroundColor = buttonSelectedColor;
			}
			//	DisableAllWindows();
			//	mod.itemBrowser.Visible = true;
			//Creative.ToggleItemBrowser();
		}

		public override void Update() {
			try {
				if (this.hidden) {
					this.lerpAmount -= /*Mod.deltaTime*/ .01f * Hotbar.moveSpeed;
					if (this.lerpAmount < 0f) {
						this.lerpAmount = 0f;
					}
					float y = MathHelper.SmoothStep(this.hiddenPosition, this.shownPosition, this.lerpAmount);
					base.Position = new Vector2(Hotbar.xPosition, y);
				}
				else {
					this.lerpAmount += .01f/*Mod.deltaTime */* Hotbar.moveSpeed;
					if (this.lerpAmount > 1f) {
						this.lerpAmount = 1f;
					}
					float y2 = MathHelper.SmoothStep(this.hiddenPosition, this.shownPosition, this.lerpAmount);
					base.Position = new Vector2(Hotbar.xPosition, y2);
				}
				base.CenterXAxisToParentCenter();
				base.Update();
			}
			catch (Exception e) {
				SpawnSheet.instance.Logger.Error(e.ToString());
			}
		}

		//public void EnableAllControls(bool login)
		//{
		//	//if (Mod.ctf.GameInProgress)
		//	//{
		//	//	this.SetCTFControls(ServerTools.group);
		//	//	return;
		//	//}
		//	for (int i = 0; i < this.buttonView.children.Count; i++)
		//	{
		//		this.buttonView.children[i].Visible = true;
		//	}
		//	//this.bLogin.Visible = login;
		//	//if (Main.netMode != 1)
		//	//{
		//	//	this.bGroupManager.Visible = false;
		//	//	this.bOnlinePlayers.Visible = false;
		//	//	this.bBackupWorld.Visible = false;
		//	//	this.bCTFSettings.Visible = false;
		//	//}
		//	this.Resize();
		//}

		//public void EnableControls(Group group)
		//{
		//	//if (Mod.ctf.GameInProgress)
		//	//{
		//	//	this.SetCTFControls(group);
		//	//	return;
		//	//}
		//	//if (group.admin)
		//	//{
		//	//	this.EnableAllControls(true);
		//	//	return;
		//	//}
		//	//this.DisableAllControls(true);
		//	//this.bToggleItemBrowser.Visible = group.itemBrowser;
		//	//this.bWaypoints.Visible = group.accessWaypoints;
		//	//this.bToggleBlockReach.Visible = group.blockReach;
		//	//this.bRevealMap.Visible = group.mapReveal;
		//	//this.bTime.Visible = group.timeControl;
		//	//this.bWeatherWindow.Visible = group.timeControl;
		//	//this.bToggleEnemies.Visible = group.disableEnemies;
		//	//this.bClearItems.Visible = group.clearItems;
		//	//this.bSpawnWindow.Visible = group.spawnNPCs;
		//	//this.bCTFSettings.Visible = group.startCTF;
		//	//if (!group.itemBrowser)
		//	//{
		//	//	Creative.itemBrowser.Visible = false;
		//	//}
		//	//if (!group.accessWaypoints)
		//	//{
		//	//	Waypoints.HideWindow();
		//	//}
		//	//if (!group.blockReach)
		//	//{
		//	//	Creative.fastItems = false;
		//	//}
		//	//Hotbar.groupWindow.Visible = false;
		//	//ServerTools.playersWindow.ClosePlayerInfo();
		//	//ServerTools.playersWindow.Visible = false;
		//	//if (group.kick || group.ban || group.teleportTo || group.snoop)
		//	//{
		//	//	this.bOnlinePlayers.Visible = true;
		//	//}
		//	this.Resize();
		//}

		//public void SetCTFControls(Group group)
		//{
		//	this.DisableAllControls(true);
		//	this.AddCTFControl(group);
		//}

		//public void AddCTFControl(Group group)
		//{
		//	//if (group.admin)
		//	//{
		//	//	this.bOnlinePlayers.Visible = true;
		//	//}
		//	//this.bCTFSettings.Visible = true;
		//	this.Resize();
		//}

		private bool ControlExists(UIView view) {
			foreach (UIView current in this.children) {
				if (current == view) {
					return true;
				}
			}
			return false;
		}

		//public void DisableAllControls(bool login)
		//{
		//	for (int i = 0; i < this.buttonView.children.Count; i++)
		//	{
		//		this.buttonView.children[i].Visible = false;
		//	}
		//	//this.bLogin.Visible = login;
		//	this.Resize();
		//}

		public void ChangedConfiguration() {
			DisableAllWindows();

			bool heros = ModLoader.TryGetMod("HEROsMod", out Mod herosMod);
			bool recentHeros = heros && herosMod.Version >= new Version(0, 2, 2);
			bool itemBrowserPermissions = true;
			if (Main.netMode == 1 && recentHeros && herosMod.Call("HasPermission", Main.myPlayer, "ItemBrowser") is bool resultA)
				itemBrowserPermissions = resultA;
			bool SpawnNPCsPermissions = true;
			if (Main.netMode == 1 && recentHeros && herosMod.Call("HasPermission", Main.myPlayer, "SpawnNPCs") is bool resultB)
				SpawnNPCsPermissions = resultB;

			bToggleRecipeBrowser.Visible = ConfigurationLoader.personalConfiguration.RecipeBrowser && SpawnSheet.instance.herosPermissions[SpawnSheet.RecipeBrowser_Permission];
			SpawnRateMultiplier.button.Visible = ConfigurationLoader.personalConfiguration.SpawnRate && SpawnRateMultiplier.HasPermission;
			//BossDowner.button.Visible = ConfigurationLoader.configuration.BossDowner;
			//bToggleEventManager.Visible = ConfigurationLoader.configuration.EventManager;
			//Main.NewText("bToggleItemBrowser " + bToggleItemBrowser.Visible);
			Resize();
		}

		//public void ChangedBossDowner()
		//{
		//	DisableAllWindows();
		//	//todo: implement All Towers / All mech bosses
		//	Resize();
		//}

		public void Resize() {
			float num = this.spacing;
			for (int i = 0; i < this.buttonView.children.Count; i++) {
				if (this.buttonView.children[i].Visible) {
					this.buttonView.children[i].X = num;
					num += this.buttonView.children[i].Width + this.spacing;
				}
			}
			base.Width = num;
			this.buttonView.Width = base.Width;
			this.button.CenterXAxisToParentCenter();
			this.arrow.Position = this.button.Position;
		}

		public void Hide() {
			this.hidden = true;
			this.arrow.SpriteEffect = SpriteEffects.None;
			if (mod.itemBrowser.selected && !mod.itemBrowser.hidden) {
				mod.itemBrowser.Hide();
			}
			if (mod.recipeBrowser.selected && !mod.recipeBrowser.hidden) {
				mod.recipeBrowser.Hide();
			}
			if (mod.npcBrowser.selected && !mod.npcBrowser.hidden) {
				mod.npcBrowser.Hide();
			}
			//if (BossDowner.bossDownerWindow.selected && !BossDowner.bossDownerWindow.hidden)
			//{
			//	BossDowner.bossDownerWindow.Hide();
			//}
			//if (mod.eventManagerHotbar.selected && !mod.eventManagerHotbar.hidden)
			//{
			//	mod.eventManagerHotbar.Hide();
			//}
		}

		public void Show() {
			this.hidden = false;
			this.arrow.SpriteEffect = SpriteEffects.FlipVertically;
			if (mod.itemBrowser.selected) {
				mod.itemBrowser.Show();
			}
			if (mod.recipeBrowser.selected) {
				mod.recipeBrowser.Show();
			}
			if (mod.npcBrowser.selected) {
				mod.npcBrowser.Show();
			}
			//if (BossDowner.bossDownerWindow.selected)
			//{
			//	BossDowner.bossDownerWindow.Show();
			//}
			//if (mod.eventManagerHotbar.selected)
			//{
			//	mod.eventManagerHotbar.Show();
			//}

			if (ModLoader.TryGetMod("HEROsMod", out Mod herosMod)) {
				herosMod.Call("HideHotbar");
			}
		}
	}
}

//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text.RegularExpressions;
//using Terraria;
//using Terraria.ID;
//using Terraria.ModLoader;

//namespace SpawnSheet.UI
//{
//	class CheatMenu : UIWindow
//	{
//		public Mod mod;
//		private static UIImage[] buttons = new UIImage[SpawnSheet.ButtonTexture.Count];
//		private float spacing = 16f;

//		public CheatMenu(Mod mod)
//		{
//			this.mod = mod;
//			this.CanMove = true;
//			base.Width = 40 + spacing * 2;
//			base.Height = 400f;
//			Texture2D texture = mod.Assets.Request<Texture2D>("UI/closeButton");
//			UIImage uIImage = new UIImage(texture);
//			uIImage.Anchor = AnchorPosition.TopRight;
//			uIImage.Position = new Vector2(base.Width - this.spacing / 2, this.spacing / 2);
//			uIImage.onLeftClick += new EventHandler(this.bClose_onLeftClick);
//			this.AddChild(uIImage);

//			for (int j = 0; j < SpawnSheet.ButtonTexture.Count; j++)
//			{
//				UIImage button = new UIImage(SpawnSheet.ButtonTexture[j]);
//				Vector2 position = new Vector2(this.spacing, this.spacing);
//				button.Scale = 32f / Math.Max(SpawnSheet.ButtonTexture[j].Width, SpawnSheet.ButtonTexture[j].Height);

//				position.X += (float)(j % 1 * 40);
//				position.Y += (float)(j / 1 * 40);

//				if (SpawnSheet.ButtonTexture[j].Height > SpawnSheet.ButtonTexture[j].Width)
//				{
//					position.X += (32 - SpawnSheet.ButtonTexture[j].Width) / 2;
//				}
//				else if (SpawnSheet.ButtonTexture[j].Height < SpawnSheet.ButtonTexture[j].Width)
//				{
//					position.Y += (32 - SpawnSheet.ButtonTexture[j].Height) / 2;
//				}

//				button.Position = position;
//				button.Tag = j;
//				button.onLeftClick += new EventHandler(this.button_onLeftClick);
//				button.onHover += new EventHandler(this.button_onHover);
//				//	button.ForegroundColor = RecipeBrowser.buttonColor;
//				//	uIImage2.Tooltip = RecipeBrowser.categNames[j];
//				ExtendedCheatMenu.buttons[j] = button;
//				this.AddChild(button);
//			}
//		}

//		public override void Draw(SpriteBatch spriteBatch)
//		{
//			base.Draw(spriteBatch);

//			if (Visible && IsMouseInside())
//			{
//				Main.LocalPlayer.mouseInterface = true;
//				Main.LocalPlayer.showItemIcon = false;
//			}

//			float x = FontAssets.MouseText.Value.MeasureString(UIView.HoverText).X;
//			Vector2 vector = new Vector2((float)Main.mouseX, (float)Main.mouseY) + new Vector2(16f);
//			if (vector.Y > (float)(Main.screenHeight - 30))
//			{
//				vector.Y = (float)(Main.screenHeight - 30);
//			}
//			if (vector.X > (float)Main.screenWidth - x)
//			{
//				vector.X = (float)(Main.screenWidth - 460);
//			}
//			Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, UIView.HoverText, vector.X, vector.Y, new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor), Color.Black, Vector2.Zero, 1f);
//		}

//		public override void Update()
//		{
//			base.Update();
//		}

//		private void bClose_onLeftClick(object sender, EventArgs e)
//		{
//			base.Visible = false;
//		}

//		private void button_onLeftClick(object sender, EventArgs e)
//		{
//			UIImage uIImage = (UIImage)sender;
//			int num = (int)uIImage.Tag;

//			SpawnSheet.ButtonClicked[num](0);
//		}

//		private void button_onHover(object sender, EventArgs e)
//		{
//			UIImage uIImage = (UIImage)sender;
//			int num = (int)uIImage.Tag;

//			uIImage.Tooltip = SpawnSheet.ButtonTooltip[num]();
//		}
//	}
//}