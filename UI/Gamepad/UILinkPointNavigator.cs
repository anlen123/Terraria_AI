using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Tile_Entities;
using Terraria.GameContent.UI;
using Terraria.GameInput;
using Terraria.Testing;

namespace Terraria.UI.Gamepad
{
	// Token: 0x02000107 RID: 263
	public class UILinkPointNavigator
	{
		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06001A59 RID: 6745 RVA: 0x004F5084 File Offset: 0x004F3284
		public static int CurrentPoint
		{
			get
			{
				return UILinkPointNavigator.Pages[UILinkPointNavigator.CurrentPage].CurrentPoint;
			}
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06001A5A RID: 6746 RVA: 0x004F509C File Offset: 0x004F329C
		public static bool Available
		{
			get
			{
				return Main.playerInventory || Main.ingameOptionsWindow || Main.player[Main.myPlayer].talkNPC != -1 || Main.player[Main.myPlayer].sign != -1 || Main.mapFullscreen || Main.clothesWindow || Main.MenuUI.IsVisible || Main.InGameUI.IsVisible;
			}
		}

		// Token: 0x06001A5B RID: 6747 RVA: 0x004F5103 File Offset: 0x004F3303
		public static void SuggestUsage(int PointID)
		{
			if (!UILinkPointNavigator.Points.ContainsKey(PointID))
			{
				return;
			}
			UILinkPointNavigator._suggestedPointID = new int?(PointID);
		}

		// Token: 0x06001A5C RID: 6748 RVA: 0x004F5120 File Offset: 0x004F3320
		public static void ConsumeSuggestion()
		{
			if (UILinkPointNavigator._suggestedPointID == null)
			{
				return;
			}
			int value = UILinkPointNavigator._suggestedPointID.Value;
			UILinkPointNavigator.ClearSuggestion();
			UILinkPointNavigator.CurrentPage = UILinkPointNavigator.Points[value].Page;
			UILinkPointNavigator.OverridePoint = value;
			UILinkPointNavigator.ProcessChanges();
			PlayerInput.Triggers.Current.UsedMovementKey = true;
		}

		// Token: 0x06001A5D RID: 6749 RVA: 0x004F517A File Offset: 0x004F337A
		public static void ClearSuggestion()
		{
			UILinkPointNavigator._suggestedPointID = null;
		}

		// Token: 0x06001A5E RID: 6750 RVA: 0x004F5188 File Offset: 0x004F3388
		public static void GoToDefaultPage(int specialFlag = 0)
		{
			TileEntity tileEntity = Main.LocalPlayer.tileEntityAnchor.GetTileEntity();
			if (Main.MenuUI.IsVisible)
			{
				UILinkPointNavigator.CurrentPage = 1004;
				return;
			}
			if (Main.InGameUI.IsVisible || specialFlag == 1)
			{
				UILinkPointNavigator.CurrentPage = 1004;
				return;
			}
			if (Main.gameMenu)
			{
				UILinkPointNavigator.CurrentPage = 1000;
				return;
			}
			if (Main.ingameOptionsWindow)
			{
				UILinkPointNavigator.CurrentPage = 1001;
				return;
			}
			if (Main.CreativeMenu.Enabled)
			{
				UILinkPointNavigator.CurrentPage = 1005;
				return;
			}
			if (NewCraftingUI.Visible)
			{
				UILinkPointNavigator.CurrentPage = 24;
				return;
			}
			if (Main.hairWindow)
			{
				UILinkPointNavigator.CurrentPage = 12;
				return;
			}
			if (Main.clothesWindow)
			{
				UILinkPointNavigator.CurrentPage = 15;
				return;
			}
			if (Main.npcShop != 0)
			{
				UILinkPointNavigator.CurrentPage = 13;
				return;
			}
			if (Main.InGuideCraftMenu)
			{
				UILinkPointNavigator.CurrentPage = 0;
				return;
			}
			if (Main.InReforgeMenu)
			{
				UILinkPointNavigator.CurrentPage = 0;
				return;
			}
			if (Main.player[Main.myPlayer].chest != -1)
			{
				UILinkPointNavigator.CurrentPage = 4;
				return;
			}
			if (tileEntity is TEDisplayDoll)
			{
				UILinkPointNavigator.CurrentPage = 20;
				return;
			}
			if (tileEntity is TEHatRack)
			{
				UILinkPointNavigator.CurrentPage = 21;
				return;
			}
			if (Main.player[Main.myPlayer].talkNPC != -1 || Main.player[Main.myPlayer].sign != -1)
			{
				UILinkPointNavigator.CurrentPage = 1003;
				return;
			}
			UILinkPointNavigator.CurrentPage = 0;
		}

		// Token: 0x06001A5F RID: 6751 RVA: 0x004F52DC File Offset: 0x004F34DC
		public static void Update()
		{
			bool inUse = UILinkPointNavigator.InUse;
			UILinkPointNavigator.InUse = false;
			bool flag = true;
			if (flag)
			{
				InputMode currentInputMode = PlayerInput.CurrentInputMode;
				if (currentInputMode <= InputMode.Mouse && !Main.gameMenu)
				{
					flag = false;
				}
			}
			if (flag && PlayerInput.NavigatorRebindingLock > 0)
			{
				flag = false;
			}
			if (flag && !Main.gameMenu && !PlayerInput.UsingGamepadUI)
			{
				flag = false;
			}
			if (flag && !Main.gameMenu && PlayerInput.InBuildingMode)
			{
				flag = false;
			}
			if (flag && !Main.gameMenu && !UILinkPointNavigator.Available)
			{
				flag = false;
			}
			if (flag && Main.gameMenu && Main.MenuUI.IsVisible && Main.MenuUI.CurrentState != null && Main.MenuUI.CurrentState.NoGamepadSupport)
			{
				flag = false;
			}
			bool flag2 = false;
			UILinkPage uilinkPage;
			if (!UILinkPointNavigator.Pages.TryGetValue(UILinkPointNavigator.CurrentPage, out uilinkPage))
			{
				flag2 = true;
			}
			else if (!uilinkPage.IsValid())
			{
				flag2 = true;
			}
			if (flag2)
			{
				UILinkPointNavigator.GoToDefaultPage(0);
				UILinkPointNavigator.ProcessChanges();
				flag = false;
			}
			if (inUse != flag)
			{
				if (!flag)
				{
					uilinkPage.Leave();
					UILinkPointNavigator.GoToDefaultPage(0);
					UILinkPointNavigator.ProcessChanges();
				}
				else
				{
					UILinkPointNavigator.GoToDefaultPage(0);
					UILinkPointNavigator.ProcessChanges();
					UILinkPointNavigator.ConsumeSuggestion();
					uilinkPage.Enter();
				}
				if (flag)
				{
					if (!PlayerInput.SteamDeckIsUsed || PlayerInput.PreventCursorModeSwappingToGamepad)
					{
						Main.player[Main.myPlayer].releaseInventory = false;
					}
					Main.player[Main.myPlayer].releaseUseTile = false;
					PlayerInput.LockGamepadTileUseButton = true;
				}
				if (!Main.gameMenu)
				{
					if (flag)
					{
						PlayerInput.NavigatorCachePosition();
					}
					else
					{
						PlayerInput.NavigatorUnCachePosition();
					}
				}
			}
			UILinkPointNavigator.ClearSuggestion();
			if (!flag)
			{
				return;
			}
			UILinkPointNavigator.InUse = true;
			UILinkPointNavigator.OverridePoint = -1;
			if (UILinkPointNavigator.PageLeftCD > 0)
			{
				UILinkPointNavigator.PageLeftCD--;
			}
			if (UILinkPointNavigator.PageRightCD > 0)
			{
				UILinkPointNavigator.PageRightCD--;
			}
			Vector2 navigatorDirections = PlayerInput.Triggers.Current.GetNavigatorDirections();
			object obj = PlayerInput.Triggers.Current.HotbarMinus && !PlayerInput.Triggers.Current.HotbarPlus;
			bool flag3 = PlayerInput.Triggers.Current.HotbarPlus && !PlayerInput.Triggers.Current.HotbarMinus;
			object obj2 = obj;
			if (obj2 == null)
			{
				UILinkPointNavigator.PageLeftCD = 0;
			}
			if (!flag3)
			{
				UILinkPointNavigator.PageRightCD = 0;
			}
			object obj3 = obj2 != null && UILinkPointNavigator.PageLeftCD == 0;
			flag3 = (flag3 && UILinkPointNavigator.PageRightCD == 0);
			if (UILinkPointNavigator.LastInput.X != navigatorDirections.X)
			{
				UILinkPointNavigator.XCooldown = 0;
			}
			if (UILinkPointNavigator.LastInput.Y != navigatorDirections.Y)
			{
				UILinkPointNavigator.YCooldown = 0;
			}
			if (UILinkPointNavigator.XCooldown > 0)
			{
				UILinkPointNavigator.XCooldown--;
			}
			if (UILinkPointNavigator.YCooldown > 0)
			{
				UILinkPointNavigator.YCooldown--;
			}
			UILinkPointNavigator.LastInput = navigatorDirections;
			object obj4 = obj3;
			if (obj4 != null)
			{
				UILinkPointNavigator.PageLeftCD = 16;
			}
			if (flag3)
			{
				UILinkPointNavigator.PageRightCD = 16;
			}
			UILinkPointNavigator.Pages[UILinkPointNavigator.CurrentPage].Update();
			int num = 10;
			if (!Main.gameMenu && Main.playerInventory && !Main.ingameOptionsWindow && !Main.inFancyUI && (UILinkPointNavigator.CurrentPage == 0 || UILinkPointNavigator.CurrentPage == 4 || UILinkPointNavigator.CurrentPage == 2 || UILinkPointNavigator.CurrentPage == 1 || UILinkPointNavigator.CurrentPage == 20 || UILinkPointNavigator.CurrentPage == 21))
			{
				num = PlayerInput.CurrentProfile.InventoryMoveCD;
			}
			if (navigatorDirections.X == -1f && UILinkPointNavigator.XCooldown == 0)
			{
				UILinkPointNavigator.XCooldown = num;
				UILinkPointNavigator.Pages[UILinkPointNavigator.CurrentPage].TravelLeft();
			}
			if (navigatorDirections.X == 1f && UILinkPointNavigator.XCooldown == 0)
			{
				UILinkPointNavigator.XCooldown = num;
				UILinkPointNavigator.Pages[UILinkPointNavigator.CurrentPage].TravelRight();
			}
			if (navigatorDirections.Y == -1f && UILinkPointNavigator.YCooldown == 0)
			{
				UILinkPointNavigator.YCooldown = num;
				UILinkPointNavigator.Pages[UILinkPointNavigator.CurrentPage].TravelUp();
			}
			if (navigatorDirections.Y == 1f && UILinkPointNavigator.YCooldown == 0)
			{
				UILinkPointNavigator.YCooldown = num;
				UILinkPointNavigator.Pages[UILinkPointNavigator.CurrentPage].TravelDown();
			}
			UILinkPointNavigator.XCooldown = (UILinkPointNavigator.YCooldown = Math.Max(UILinkPointNavigator.XCooldown, UILinkPointNavigator.YCooldown));
			if (obj4 != null)
			{
				UILinkPointNavigator.Pages[UILinkPointNavigator.CurrentPage].SwapPageLeft();
			}
			if (flag3)
			{
				UILinkPointNavigator.Pages[UILinkPointNavigator.CurrentPage].SwapPageRight();
			}
			if (PlayerInput.Triggers.Current.UsedMovementKey)
			{
				Vector2 position = UILinkPointNavigator.Points[UILinkPointNavigator.CurrentPoint].Position;
				Vector2 value = new Vector2((float)PlayerInput.MouseX, (float)PlayerInput.MouseY);
				float amount = 0.3f;
				if (PlayerInput.InvisibleGamepadInMenus)
				{
					amount = 1f;
				}
				Vector2 vector = Vector2.Lerp(value, position, amount);
				if (Main.gameMenu)
				{
					if (Math.Abs(vector.X - position.X) <= 5f)
					{
						vector.X = position.X;
					}
					if (Math.Abs(vector.Y - position.Y) <= 5f)
					{
						vector.Y = position.Y;
					}
				}
				PlayerInput.MouseX = (int)vector.X;
				PlayerInput.MouseY = (int)vector.Y;
			}
			UILinkPointNavigator.ResetFlagsEnd();
			if (DebugOptions.DrawLinkPoints)
			{
				UILinkPointNavigator.DrawLinks();
			}
		}

		// Token: 0x06001A60 RID: 6752 RVA: 0x004F57D7 File Offset: 0x004F39D7
		public static void ResetFlagsEnd()
		{
			UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 0;
			UILinkPointNavigator.Shortcuts.BackButtonLock = false;
			UILinkPointNavigator.Shortcuts.BackButtonCommand = 0;
		}

		// Token: 0x06001A61 RID: 6753 RVA: 0x004F57EC File Offset: 0x004F39EC
		public static string GetInstructions()
		{
			UILinkPage uilinkPage = UILinkPointNavigator.Pages[UILinkPointNavigator.CurrentPage];
			UILinkPoint uilinkPoint = UILinkPointNavigator.Points[UILinkPointNavigator.CurrentPoint];
			if (UILinkPointNavigator._suggestedPointID != null)
			{
				UILinkPointNavigator.SwapToSuggestion();
				uilinkPoint = UILinkPointNavigator.Points[UILinkPointNavigator._suggestedPointID.Value];
				uilinkPage = UILinkPointNavigator.Pages[uilinkPoint.Page];
				UILinkPointNavigator.CurrentPage = uilinkPage.ID;
				uilinkPage.CurrentPoint = UILinkPointNavigator._suggestedPointID.Value;
			}
			string text = uilinkPage.SpecialInteractions();
			if ((PlayerInput.SettingsForUI.CurrentCursorMode == CursorMode.Gamepad && PlayerInput.Triggers.Current.UsedMovementKey && UILinkPointNavigator.InUse) || UILinkPointNavigator._suggestedPointID != null)
			{
				text += uilinkPoint.SpecialInteractions();
			}
			text += uilinkPage.SpecialInteractionsLate();
			UILinkPointNavigator.ConsumeSuggestionSwap();
			return text;
		}

		// Token: 0x06001A62 RID: 6754 RVA: 0x004F58C1 File Offset: 0x004F3AC1
		public static void SwapToSuggestion()
		{
			UILinkPointNavigator._preSuggestionPoint = new int?(UILinkPointNavigator.CurrentPoint);
		}

		// Token: 0x06001A63 RID: 6755 RVA: 0x004F58D4 File Offset: 0x004F3AD4
		public static void ConsumeSuggestionSwap()
		{
			if (UILinkPointNavigator._preSuggestionPoint != null)
			{
				int value = UILinkPointNavigator._preSuggestionPoint.Value;
				UILinkPointNavigator.CurrentPage = UILinkPointNavigator.Points[value].Page;
				UILinkPointNavigator.Pages[UILinkPointNavigator.CurrentPage].CurrentPoint = value;
			}
			UILinkPointNavigator._preSuggestionPoint = null;
		}

		// Token: 0x06001A64 RID: 6756 RVA: 0x004F592D File Offset: 0x004F3B2D
		public static void ForceMovementCooldown(int time)
		{
			UILinkPointNavigator.LastInput = PlayerInput.Triggers.Current.GetNavigatorDirections();
			UILinkPointNavigator.XCooldown = time;
			UILinkPointNavigator.YCooldown = time;
		}

		// Token: 0x06001A65 RID: 6757 RVA: 0x004F594F File Offset: 0x004F3B4F
		public static void SetPosition(int ID, Vector2 Position)
		{
			UILinkPointNavigator.Points[ID].Position = Position * Main.UIScale;
		}

		// Token: 0x06001A66 RID: 6758 RVA: 0x004F596C File Offset: 0x004F3B6C
		public static Vector2 GetPosition(int ID)
		{
			UILinkPoint uilinkPoint;
			Vector2 value;
			if (!UILinkPointNavigator.Points.TryGetValue(ID, out uilinkPoint))
			{
				value = Vector2.Zero;
			}
			else
			{
				value = uilinkPoint.Position;
			}
			if (value == Vector2.Zero)
			{
				if (ID >= 180 && ID <= 184)
				{
					value = UILinkPointNavigator.GetPosition(ID - 180 + 100);
				}
				else if (ID >= 185 && ID <= 189)
				{
					value = UILinkPointNavigator.GetPosition(ID - 185 + 110);
				}
			}
			return value / Main.UIScale;
		}

		// Token: 0x06001A67 RID: 6759 RVA: 0x004F59F4 File Offset: 0x004F3BF4
		public static void RegisterPage(UILinkPage page, int ID, bool automatedDefault = true)
		{
			if (automatedDefault)
			{
				page.DefaultPoint = page.LinkMap.Keys.First<int>();
			}
			page.CurrentPoint = page.DefaultPoint;
			page.ID = ID;
			UILinkPointNavigator.Pages.Add(page.ID, page);
			foreach (KeyValuePair<int, UILinkPoint> keyValuePair in page.LinkMap)
			{
				keyValuePair.Value.SetPage(ID);
				UILinkPointNavigator.Points.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x06001A68 RID: 6760 RVA: 0x004F5AA4 File Offset: 0x004F3CA4
		public static void ChangePage(int PageID)
		{
			if (UILinkPointNavigator.Pages.ContainsKey(PageID) && UILinkPointNavigator.Pages[PageID].CanEnter())
			{
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
				UILinkPointNavigator.CurrentPage = PageID;
				UILinkPointNavigator.ProcessChanges();
			}
		}

		// Token: 0x06001A69 RID: 6761 RVA: 0x004F5AF0 File Offset: 0x004F3CF0
		public static void ChangePoint(int PointID)
		{
			if (UILinkPointNavigator.Points.ContainsKey(PointID))
			{
				UILinkPointNavigator.CurrentPage = UILinkPointNavigator.Points[PointID].Page;
				UILinkPointNavigator.OverridePoint = PointID;
				UILinkPointNavigator.ProcessChanges();
			}
		}

		// Token: 0x06001A6A RID: 6762 RVA: 0x004F5B20 File Offset: 0x004F3D20
		public static void ProcessChanges()
		{
			UILinkPage uilinkPage = UILinkPointNavigator.Pages[UILinkPointNavigator.OldPage];
			if (UILinkPointNavigator.OldPage != UILinkPointNavigator.CurrentPage)
			{
				uilinkPage.Leave();
				if (!UILinkPointNavigator.Pages.TryGetValue(UILinkPointNavigator.CurrentPage, out uilinkPage))
				{
					UILinkPointNavigator.GoToDefaultPage(0);
					UILinkPointNavigator.ProcessChanges();
					UILinkPointNavigator.OverridePoint = -1;
				}
				uilinkPage.CurrentPoint = uilinkPage.DefaultPoint;
				uilinkPage.Enter();
				uilinkPage.Update();
				UILinkPointNavigator.OldPage = UILinkPointNavigator.CurrentPage;
			}
			if (UILinkPointNavigator.OverridePoint != -1 && uilinkPage.LinkMap.ContainsKey(UILinkPointNavigator.OverridePoint))
			{
				uilinkPage.CurrentPoint = UILinkPointNavigator.OverridePoint;
			}
		}

		// Token: 0x06001A6B RID: 6763 RVA: 0x004F5BBC File Offset: 0x004F3DBC
		private static void DrawLinks()
		{
			UILinkPoint item;
			if (!UILinkPointNavigator.Points.TryGetValue(UILinkPointNavigator.CurrentPoint, out item))
			{
				return;
			}
			UILinkPointNavigator._visited.Clear();
			UILinkPointNavigator._visited.Add(item);
			UILinkPointNavigator._queue.Clear();
			UILinkPointNavigator._queue.Enqueue(item);
			while (UILinkPointNavigator._queue.Any<UILinkPoint>())
			{
				UILinkPoint uilinkPoint = UILinkPointNavigator._queue.Dequeue();
				UILinkPointNavigator.DrawLink(uilinkPoint, uilinkPoint.Up, new Vector2(0f, -1f), new Color(120, 0, 20), new Color(255, 0, 255));
				UILinkPointNavigator.DrawLink(uilinkPoint, uilinkPoint.Down, new Vector2(0f, 1f), new Color(0, 0, 255), new Color(0, 255, 255));
				UILinkPointNavigator.DrawLink(uilinkPoint, uilinkPoint.Left, new Vector2(-1f, 0f), new Color(0, 100, 0), new Color(50, 205, 50));
				UILinkPointNavigator.DrawLink(uilinkPoint, uilinkPoint.Right, new Vector2(1f, 0f), new Color(100, 100, 0), new Color(255, 215, 0));
			}
		}

		// Token: 0x06001A6C RID: 6764 RVA: 0x004F5CF8 File Offset: 0x004F3EF8
		private static void DrawLink(UILinkPoint src, int targetId, Vector2 dir, Color colorStart, Color colorEnd)
		{
			UILinkPoint uilinkPoint;
			if (!UILinkPointNavigator.Points.TryGetValue(targetId, out uilinkPoint) || uilinkPoint.Position == Vector2.Zero)
			{
				return;
			}
			if (UILinkPointNavigator._visited.Add(uilinkPoint))
			{
				UILinkPointNavigator._queue.Enqueue(uilinkPoint);
			}
			Vector2 vector = dir.RotatedBy(1.5707963267948966, default(Vector2));
			Vector2 vector2 = src.Position / Main.UIScale + vector * 2f;
			Vector2 vector3 = uilinkPoint.Position / Main.UIScale + vector * 2f;
			if (Vector2.Dot(vector3 - vector2, dir) < 0f)
			{
				DebugLineDraw.UI.AddLine(vector2, vector2 += (vector + dir * 2f) * 2f * 2f, colorStart, default(Color), 1, 1f);
				DebugLineDraw.UI.AddLine(vector3, vector3 += (vector - dir * 2f) * 2f * 2f, colorEnd, default(Color), 1, 1f);
			}
			DebugLineDraw.UI.AddLine(vector2, vector3, colorStart, colorEnd, 1, 1f);
		}

		// Token: 0x040014CA RID: 5322
		public static Dictionary<int, UILinkPage> Pages = new Dictionary<int, UILinkPage>();

		// Token: 0x040014CB RID: 5323
		public static Dictionary<int, UILinkPoint> Points = new Dictionary<int, UILinkPoint>();

		// Token: 0x040014CC RID: 5324
		public static int CurrentPage = 1000;

		// Token: 0x040014CD RID: 5325
		public static int OldPage = 1000;

		// Token: 0x040014CE RID: 5326
		private static int XCooldown;

		// Token: 0x040014CF RID: 5327
		private static int YCooldown;

		// Token: 0x040014D0 RID: 5328
		private static Vector2 LastInput;

		// Token: 0x040014D1 RID: 5329
		private static int PageLeftCD;

		// Token: 0x040014D2 RID: 5330
		private static int PageRightCD;

		// Token: 0x040014D3 RID: 5331
		public static bool InUse;

		// Token: 0x040014D4 RID: 5332
		public static int OverridePoint = -1;

		// Token: 0x040014D5 RID: 5333
		private static int? _suggestedPointID;

		// Token: 0x040014D6 RID: 5334
		private static int? _preSuggestionPoint;

		// Token: 0x040014D7 RID: 5335
		private static HashSet<UILinkPoint> _visited = new HashSet<UILinkPoint>();

		// Token: 0x040014D8 RID: 5336
		private static Queue<UILinkPoint> _queue = new Queue<UILinkPoint>();

		// Token: 0x02000719 RID: 1817
		public static class Shortcuts
		{
			// Token: 0x040068DC RID: 26844
			public static int NPCS_IconsPerColumn = 100;

			// Token: 0x040068DD RID: 26845
			public static int NPCS_IconsTotal = 0;

			// Token: 0x040068DE RID: 26846
			public static int NPCS_HoveredBanner = -2;

			// Token: 0x040068DF RID: 26847
			public static int NPCS_SelectedNPC = -2;

			// Token: 0x040068E0 RID: 26848
			public static bool NPCS_IconsDisplay = false;

			// Token: 0x040068E1 RID: 26849
			public static int CRAFT_IconsPerRow = 100;

			// Token: 0x040068E2 RID: 26850
			public static int CRAFT_IconsPerColumn = 100;

			// Token: 0x040068E3 RID: 26851
			public static int CRAFT_CurrentIngredientsCount = 0;

			// Token: 0x040068E4 RID: 26852
			public static int CRAFT_CurrentRecipeBig = 0;

			// Token: 0x040068E5 RID: 26853
			public static int CRAFT_CurrentRecipeSmall = 0;

			// Token: 0x040068E6 RID: 26854
			public static int NewCraftingUI_MaterialIndex = 0;

			// Token: 0x040068E7 RID: 26855
			public static bool NPCCHAT_ButtonsNew = false;

			// Token: 0x040068E8 RID: 26856
			public static int NPCCHAT_ButtonsCount = 1;

			// Token: 0x040068E9 RID: 26857
			public static bool NPCCHAT_ButtonsLeft = false;

			// Token: 0x040068EA RID: 26858
			public static bool NPCCHAT_ButtonsMiddle = false;

			// Token: 0x040068EB RID: 26859
			public static bool NPCCHAT_ButtonsRight = false;

			// Token: 0x040068EC RID: 26860
			public static bool NPCCHAT_ButtonsRight2 = false;

			// Token: 0x040068ED RID: 26861
			public static int INGAMEOPTIONS_BUTTONS_LEFT = 0;

			// Token: 0x040068EE RID: 26862
			public static int INGAMEOPTIONS_BUTTONS_RIGHT = 0;

			// Token: 0x040068EF RID: 26863
			public static bool ItemSlotShouldHighlightAsSelected = false;

			// Token: 0x040068F0 RID: 26864
			public static bool ItemSlotShouldHighlightAsPreviouslySelected = false;

			// Token: 0x040068F1 RID: 26865
			public static int OPTIONS_BUTTON_SPECIALFEATURE;

			// Token: 0x040068F2 RID: 26866
			public static int BackButtonCommand;

			// Token: 0x040068F3 RID: 26867
			public static bool BackButtonInUse = false;

			// Token: 0x040068F4 RID: 26868
			public static bool BackButtonLock;

			// Token: 0x040068F5 RID: 26869
			public static int FANCYUI_HIGHEST_INDEX = 1;

			// Token: 0x040068F6 RID: 26870
			public static int FANCYUI_SPECIAL_INSTRUCTIONS = 0;

			// Token: 0x040068F7 RID: 26871
			public static int INFOACCCOUNT = 0;

			// Token: 0x040068F8 RID: 26872
			public static int BUILDERACCCOUNT = 0;

			// Token: 0x040068F9 RID: 26873
			public static int BUFFS_PER_COLUMN = 0;

			// Token: 0x040068FA RID: 26874
			public static int BUFFS_DRAWN = 0;

			// Token: 0x040068FB RID: 26875
			public static int INV_MOVE_OPTION_CD = 0;
		}
	}
}
