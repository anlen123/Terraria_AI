using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Tile_Entities;
using Terraria.GameContent.UI;
using Terraria.GameContent.UI.States;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.Social;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.Initializers
{
	// Token: 0x02000088 RID: 136
	public class UILinksInitializer
	{
		// Token: 0x0600159B RID: 5531 RVA: 0x004CC225 File Offset: 0x004CA425
		public static bool NothingMoreImportantThanNPCChat()
		{
			return !Main.hairWindow && Main.npcShop == 0 && Main.player[Main.myPlayer].chest == -1;
		}

		// Token: 0x0600159C RID: 5532 RVA: 0x004CC24C File Offset: 0x004CA44C
		public static float HandleSliderHorizontalInput(float currentValue, float min, float max, float deadZone = 0.2f, float sensitivity = 0.5f)
		{
			float num = PlayerInput.GamepadThumbstickLeft.X;
			if (num < -deadZone || num > deadZone)
			{
				num = MathHelper.Lerp(0f, sensitivity / 60f, (Math.Abs(num) - deadZone) / (1f - deadZone)) * (float)Math.Sign(num);
			}
			else
			{
				num = 0f;
			}
			return MathHelper.Clamp((currentValue - min) / (max - min) + num, 0f, 1f) * (max - min) + min;
		}

		// Token: 0x0600159D RID: 5533 RVA: 0x004CC2C0 File Offset: 0x004CA4C0
		public static float HandleSliderVerticalInput(float currentValue, float min, float max, float deadZone = 0.2f, float sensitivity = 0.5f)
		{
			float num = -PlayerInput.GamepadThumbstickLeft.Y;
			if (num < -deadZone || num > deadZone)
			{
				num = MathHelper.Lerp(0f, sensitivity / 60f, (Math.Abs(num) - deadZone) / (1f - deadZone)) * (float)Math.Sign(num);
			}
			else
			{
				num = 0f;
			}
			return MathHelper.Clamp((currentValue - min) / (max - min) + num, 0f, 1f) * (max - min) + min;
		}

		// Token: 0x0600159E RID: 5534 RVA: 0x004CC333 File Offset: 0x004CA533
		public static bool CanExecuteInputCommand()
		{
			return PlayerInput.AllowExecutionOfGamepadInstructions;
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x0600159F RID: 5535 RVA: 0x004CC33A File Offset: 0x004CA53A
		// (set) Token: 0x060015A0 RID: 5536 RVA: 0x004CC341 File Offset: 0x004CA541
		public static int MainfocusRecipe
		{
			get
			{
				return Main.focusRecipe;
			}
			set
			{
				Main.focusRecipe = value;
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x060015A1 RID: 5537 RVA: 0x004CC33A File Offset: 0x004CA53A
		// (set) Token: 0x060015A2 RID: 5538 RVA: 0x004CC341 File Offset: 0x004CA541
		public static int MainFocusBanner
		{
			get
			{
				return Main.focusRecipe;
			}
			set
			{
				Main.focusRecipe = value;
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x060015A3 RID: 5539 RVA: 0x004CC349 File Offset: 0x004CA549
		// (set) Token: 0x060015A4 RID: 5540 RVA: 0x004CC350 File Offset: 0x004CA550
		public static int MainnumAvailableRecipes
		{
			get
			{
				return Main.numAvailableRecipes;
			}
			set
			{
				Main.numAvailableRecipes = value;
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x060015A5 RID: 5541 RVA: 0x004CC349 File Offset: 0x004CA549
		// (set) Token: 0x060015A6 RID: 5542 RVA: 0x004CC350 File Offset: 0x004CA550
		public static int MainnumAvailableRecipes2
		{
			get
			{
				return Main.numAvailableRecipes;
			}
			set
			{
				Main.numAvailableRecipes = value;
			}
		}

		// Token: 0x060015A7 RID: 5543 RVA: 0x004CC358 File Offset: 0x004CA558
		public static void Load()
		{
			Func<string> value = () => PlayerInput.BuildCommand(Lang.misc[53].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]
			});
			UILinkPage uilinkPage = new UILinkPage();
			uilinkPage.UpdateEvent += delegate()
			{
				PlayerInput.GamepadAllowScrolling = true;
			};
			for (int i = 0; i < 20; i++)
			{
				uilinkPage.LinkMap.Add(2000 + i, new UILinkPoint(2000 + i, true, -3, -4, -1, -2));
			}
			uilinkPage.OnSpecialInteracts += (() => PlayerInput.BuildCommand(Lang.misc[53].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]
			}) + PlayerInput.BuildCommand(Lang.misc[82].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]
			}));
			uilinkPage.UpdateEvent += delegate()
			{
				bool flag = PlayerInput.Triggers.JustPressed.Inventory;
				if (Main.inputTextEscape)
				{
					Main.inputTextEscape = false;
					flag = true;
				}
				if (UILinksInitializer.CanExecuteInputCommand() && flag)
				{
					UILinksInitializer.FancyExit();
				}
				UILinkPointNavigator.Shortcuts.BackButtonInUse = flag;
				UILinksInitializer.HandleOptionsSpecials();
			};
			uilinkPage.IsValidEvent += (() => Main.gameMenu && !Main.MenuUI.IsVisible);
			uilinkPage.CanEnterEvent += (() => Main.gameMenu && !Main.MenuUI.IsVisible);
			UILinkPointNavigator.RegisterPage(uilinkPage, 1000, true);
			UILinkPage cp2 = new UILinkPage();
			cp2.LinkMap.Add(2500, new UILinkPoint(2500, true, -3, -4, -1, -2));
			cp2.LinkMap.Add(2501, new UILinkPoint(2501, true, -3, -4, -1, -2));
			cp2.LinkMap.Add(2502, new UILinkPoint(2502, true, -3, -4, -1, -2));
			cp2.LinkMap.Add(2503, new UILinkPoint(2503, true, -3, -4, -1, -2));
			cp2.LinkMap.Add(2504, new UILinkPoint(2504, true, -3, -4, -1, -2));
			cp2.LinkMap.Add(2505, new UILinkPoint(2505, true, -3, -4, -1, -2));
			cp2.LinkMap.Add(2506, new UILinkPoint(2506, true, -3, -4, -1, -2));
			cp2.LinkMap.Add(2507, new UILinkPoint(2507, true, -3, -4, -1, -2));
			cp2.LinkMap.Add(2508, new UILinkPoint(2508, true, -3, -4, -1, -2));
			cp2.LinkMap.Add(2509, new UILinkPoint(2509, true, -3, -4, -1, -2));
			cp2.LinkMap.Add(2510, new UILinkPoint(2510, true, -3, -4, -1, -2));
			cp2.LinkMap.Add(2511, new UILinkPoint(2511, true, -3, -4, -1, -2));
			cp2.UpdateEvent += delegate()
			{
				if (UILinkPointNavigator.Shortcuts.NPCCHAT_ButtonsNew)
				{
					for (int num33 = 0; num33 < UILinkPointNavigator.Shortcuts.NPCCHAT_ButtonsCount; num33++)
					{
						if (num33 - 4 >= 0)
						{
							cp2.LinkMap[2500 + num33].Up = 2500 + num33 - 4;
						}
						else
						{
							cp2.LinkMap[2500 + num33].Up = -1;
						}
						if (num33 + 4 < UILinkPointNavigator.Shortcuts.NPCCHAT_ButtonsCount)
						{
							cp2.LinkMap[2500 + num33].Down = 2500 + num33 + 4;
						}
						else
						{
							cp2.LinkMap[2500 + num33].Down = -2;
						}
						cp2.LinkMap[2500 + num33].Left = ((num33 > 0) ? (2500 + num33 - 1) : -3);
						cp2.LinkMap[2500 + num33].Right = ((num33 < UILinkPointNavigator.Shortcuts.NPCCHAT_ButtonsCount - 1) ? (2500 + num33 + 1) : -4);
					}
					return;
				}
				cp2.LinkMap[2501].Right = (UILinkPointNavigator.Shortcuts.NPCCHAT_ButtonsRight ? 2502 : -4);
				if (cp2.LinkMap[2501].Right == -4 && UILinkPointNavigator.Shortcuts.NPCCHAT_ButtonsRight2)
				{
					cp2.LinkMap[2501].Right = 2503;
				}
				cp2.LinkMap[2502].Right = (UILinkPointNavigator.Shortcuts.NPCCHAT_ButtonsRight2 ? 2503 : -4);
				cp2.LinkMap[2503].Left = (UILinkPointNavigator.Shortcuts.NPCCHAT_ButtonsRight ? 2502 : 2501);
			};
			cp2.OnSpecialInteracts += (() => PlayerInput.BuildCommand(Lang.misc[53].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]
			}) + PlayerInput.BuildCommand(Lang.misc[56].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]
			}));
			cp2.IsValidEvent += (() => (Main.player[Main.myPlayer].talkNPC != -1 || Main.player[Main.myPlayer].sign != -1) && UILinksInitializer.NothingMoreImportantThanNPCChat());
			cp2.CanEnterEvent += (() => (Main.player[Main.myPlayer].talkNPC != -1 || Main.player[Main.myPlayer].sign != -1) && UILinksInitializer.NothingMoreImportantThanNPCChat());
			cp2.EnterEvent += delegate()
			{
				Main.player[Main.myPlayer].releaseInventory = false;
			};
			cp2.LeaveEvent += delegate()
			{
				Main.npcChatRelease = false;
				Main.player[Main.myPlayer].LockGamepadTileInteractions();
			};
			UILinkPointNavigator.RegisterPage(cp2, 1003, true);
			UILinkPage cp3 = new UILinkPage();
			cp3.OnSpecialInteracts += (() => PlayerInput.BuildCommand(Lang.misc[56].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]
			}) + PlayerInput.BuildCommand(Lang.misc[64].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"],
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]
			}));
			Func<string> value2 = delegate()
			{
				int currentPoint = UILinkPointNavigator.CurrentPoint;
				return ItemSlot.GetGamepadInstructions(Main.player[Main.myPlayer].inventory, 0, currentPoint);
			};
			Func<string> value3 = () => ItemSlot.GetGamepadInstructions(ref Main.player[Main.myPlayer].trashItem, 6);
			for (int j = 0; j <= 49; j++)
			{
				UILinkPoint uilinkPoint = new UILinkPoint(j, true, j - 1, j + 1, j - 10, j + 10);
				uilinkPoint.OnSpecialInteracts += value2;
				int num = j;
				if (num < 10)
				{
					uilinkPoint.Up = -1;
				}
				if (num >= 40)
				{
					uilinkPoint.Down = -2;
				}
				if (num % 10 == 9)
				{
					uilinkPoint.Right = -4;
				}
				if (num % 10 == 0)
				{
					uilinkPoint.Left = -3;
				}
				cp3.LinkMap.Add(j, uilinkPoint);
			}
			cp3.LinkMap[9].Right = 0;
			cp3.LinkMap[19].Right = 50;
			cp3.LinkMap[29].Right = 51;
			cp3.LinkMap[39].Right = 52;
			cp3.LinkMap[49].Right = 53;
			cp3.LinkMap[0].Left = 9;
			cp3.LinkMap[10].Left = 54;
			cp3.LinkMap[20].Left = 55;
			cp3.LinkMap[30].Left = 56;
			cp3.LinkMap[40].Left = 57;
			cp3.LinkMap.Add(300, new UILinkPoint(300, true, 309, 310, 49, -2));
			cp3.LinkMap.Add(309, new UILinkPoint(309, true, 310, 300, 302, 54));
			cp3.LinkMap.Add(310, new UILinkPoint(310, true, 300, 309, 301, 50));
			cp3.LinkMap.Add(301, new UILinkPoint(301, true, 300, 302, 53, 310));
			cp3.LinkMap.Add(302, new UILinkPoint(302, true, 301, 300, 57, 309));
			cp3.LinkMap.Add(311, new UILinkPoint(311, true, -3, -4, 40, -2));
			cp3.LinkMap[301].OnSpecialInteracts += value;
			cp3.LinkMap[302].OnSpecialInteracts += value;
			cp3.LinkMap[309].OnSpecialInteracts += value;
			cp3.LinkMap[310].OnSpecialInteracts += value;
			cp3.LinkMap[300].OnSpecialInteracts += value3;
			cp3.UpdateEvent += delegate()
			{
				bool inReforgeMenu = Main.InReforgeMenu;
				bool flag = Main.LocalPlayer.chest != -1;
				bool flag2 = Main.npcShop != 0;
				TileEntity tileEntity = Main.LocalPlayer.tileEntityAnchor.GetTileEntity();
				bool flag3 = tileEntity is TEHatRack;
				bool flag4 = tileEntity is TEDisplayDoll;
				if (NewCraftingUI.Visible)
				{
					flag = false;
				}
				for (int num33 = 40; num33 <= 49; num33++)
				{
					if (inReforgeMenu)
					{
						cp3.LinkMap[num33].Down = ((num33 < 45) ? 303 : 304);
					}
					else if (flag)
					{
						cp3.LinkMap[num33].Down = 400 + num33 - 40;
					}
					else if (flag2)
					{
						cp3.LinkMap[num33].Down = 2700 + num33 - 40;
					}
					else if (num33 == 40 && Main.IsJourneyMode && !Main.CreativeMenu.Blocked)
					{
						cp3.LinkMap[num33].Down = 311;
					}
					else if (!NewCraftingUI.Visible)
					{
						cp3.LinkMap[num33].Down = -2;
					}
				}
				if (flag4)
				{
					for (int num34 = 41; num34 <= 48; num34++)
					{
						cp3.LinkMap[num34].Down = 5100 + (int)Math.Round((double)((num34 - 40) * 10) / 9.0) - 1;
					}
					cp3.LinkMap[40].Down = 5118;
				}
				if (flag3)
				{
					for (int num35 = 44; num35 <= 45; num35++)
					{
						cp3.LinkMap[num35].Down = 5000 + num35 - 44;
					}
				}
				if (NewCraftingUI.Visible && Main.LocalPlayer.chest != -1)
				{
					cp3.LinkMap[49].Down = 300;
					cp3.LinkMap[300].Up = 49;
					cp3.LinkMap[300].Right = 310;
					cp3.LinkMap[310].Up = 53;
					cp3.LinkMap[309].Up = 57;
				}
				else if (flag)
				{
					cp3.LinkMap[300].Up = 439;
					cp3.LinkMap[300].Right = 310;
					cp3.LinkMap[300].Left = 309;
					cp3.LinkMap[310].Up = ((Main.LocalPlayer.chest < -1) ? 505 : 504);
					cp3.LinkMap[309].Up = ((Main.LocalPlayer.chest < -1) ? 505 : 504);
				}
				else if (flag2)
				{
					cp3.LinkMap[300].Up = 2739;
					cp3.LinkMap[300].Right = 310;
					cp3.LinkMap[300].Left = 309;
					cp3.LinkMap[310].Up = 53;
					cp3.LinkMap[309].Up = 57;
				}
				else
				{
					cp3.LinkMap[49].Down = 300;
					cp3.LinkMap[300].Up = 49;
					cp3.LinkMap[300].Right = 301;
					if (!NewCraftingUI.Visible)
					{
						cp3.LinkMap[300].Left = 302;
					}
					cp3.LinkMap[309].Up = 302;
					cp3.LinkMap[310].Up = 301;
				}
				if (!NewCraftingUI.Visible)
				{
					cp3.LinkMap[311].Right = -1;
					cp3.LinkMap[311].Down = -1;
					cp3.LinkMap[300].Down = -1;
				}
				cp3.LinkMap[0].Left = 9;
				cp3.LinkMap[10].Left = 54;
				cp3.LinkMap[20].Left = 55;
				cp3.LinkMap[30].Left = 56;
				cp3.LinkMap[40].Left = 57;
				if (UILinkPointNavigator.Shortcuts.BUILDERACCCOUNT > 0)
				{
					cp3.LinkMap[0].Left = 6000;
				}
				if (UILinkPointNavigator.Shortcuts.BUILDERACCCOUNT > 2)
				{
					cp3.LinkMap[10].Left = 6002;
				}
				if (UILinkPointNavigator.Shortcuts.BUILDERACCCOUNT > 4)
				{
					cp3.LinkMap[20].Left = 6004;
				}
				if (UILinkPointNavigator.Shortcuts.BUILDERACCCOUNT > 6)
				{
					cp3.LinkMap[30].Left = 6006;
				}
				if (UILinkPointNavigator.Shortcuts.BUILDERACCCOUNT > 8)
				{
					cp3.LinkMap[40].Left = 6008;
				}
				cp3.PageOnLeft = 9;
				if (Main.InPipBanner)
				{
					cp3.PageOnLeft = 22;
				}
				if (Main.CreativeMenu.Enabled)
				{
					cp3.PageOnLeft = 1005;
				}
				if (NewCraftingUI.Visible)
				{
					cp3.PageOnLeft = 24;
				}
				if (Main.InReforgeMenu)
				{
					cp3.PageOnLeft = 5;
				}
				if (flag4)
				{
					cp3.PageOnLeft = 20;
				}
				if (flag3)
				{
					cp3.PageOnLeft = 21;
				}
			};
			cp3.IsValidEvent += (() => Main.playerInventory);
			cp3.PageOnLeft = 9;
			cp3.PageOnRight = 2;
			UILinkPointNavigator.RegisterPage(cp3, 0, true);
			UILinkPage cp4 = new UILinkPage();
			cp4.OnSpecialInteracts += (() => PlayerInput.BuildCommand(Lang.misc[56].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]
			}) + PlayerInput.BuildCommand(Lang.misc[64].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"],
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]
			}));
			Func<string> value4 = delegate()
			{
				int currentPoint = UILinkPointNavigator.CurrentPoint;
				return ItemSlot.GetGamepadInstructions(Main.player[Main.myPlayer].inventory, 1, currentPoint);
			};
			for (int k = 50; k <= 53; k++)
			{
				UILinkPoint uilinkPoint2 = new UILinkPoint(k, true, -3, -4, k - 1, k + 1);
				uilinkPoint2.OnSpecialInteracts += value4;
				cp4.LinkMap.Add(k, uilinkPoint2);
			}
			cp4.LinkMap[50].Left = 19;
			cp4.LinkMap[51].Left = 29;
			cp4.LinkMap[52].Left = 39;
			cp4.LinkMap[53].Left = 49;
			cp4.LinkMap[50].Right = 54;
			cp4.LinkMap[51].Right = 55;
			cp4.LinkMap[52].Right = 56;
			cp4.LinkMap[53].Right = 57;
			cp4.LinkMap[50].Up = 310;
			cp4.UpdateEvent += delegate()
			{
				if (Main.npcShop != 0)
				{
					cp4.LinkMap[53].Down = 310;
					return;
				}
				if (Main.player[Main.myPlayer].chest != -1)
				{
					cp4.LinkMap[53].Down = (NewCraftingUI.Visible ? 310 : 500);
					return;
				}
				cp4.LinkMap[53].Down = 301;
			};
			cp4.IsValidEvent += (() => Main.playerInventory);
			cp4.PageOnLeft = 0;
			cp4.PageOnRight = 2;
			UILinkPointNavigator.RegisterPage(cp4, 1, true);
			UILinkPage cp5 = new UILinkPage();
			cp5.OnSpecialInteracts += (() => PlayerInput.BuildCommand(Lang.misc[56].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]
			}) + PlayerInput.BuildCommand(Lang.misc[64].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"],
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]
			}));
			Func<string> value5 = delegate()
			{
				int currentPoint = UILinkPointNavigator.CurrentPoint;
				return ItemSlot.GetGamepadInstructions(Main.player[Main.myPlayer].inventory, 2, currentPoint);
			};
			for (int l = 54; l <= 57; l++)
			{
				UILinkPoint uilinkPoint3 = new UILinkPoint(l, true, -3, -4, l - 1, l + 1);
				uilinkPoint3.OnSpecialInteracts += value5;
				cp5.LinkMap.Add(l, uilinkPoint3);
			}
			cp5.LinkMap[54].Left = 50;
			cp5.LinkMap[55].Left = 51;
			cp5.LinkMap[56].Left = 52;
			cp5.LinkMap[57].Left = 53;
			cp5.LinkMap[54].Right = 10;
			cp5.LinkMap[55].Right = 20;
			cp5.LinkMap[56].Right = 30;
			cp5.LinkMap[57].Right = 40;
			cp5.LinkMap[54].Up = 309;
			cp5.UpdateEvent += delegate()
			{
				if (Main.npcShop != 0)
				{
					cp5.LinkMap[57].Down = 309;
					return;
				}
				if (Main.player[Main.myPlayer].chest != -1)
				{
					cp5.LinkMap[57].Down = (NewCraftingUI.Visible ? 310 : 500);
					return;
				}
				cp5.LinkMap[57].Down = 302;
			};
			cp5.PageOnLeft = 0;
			cp5.PageOnRight = 8;
			UILinkPointNavigator.RegisterPage(cp5, 2, true);
			UILinkPage cp6 = new UILinkPage();
			cp6.OnSpecialInteracts += (() => PlayerInput.BuildCommand(Lang.misc[56].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]
			}) + PlayerInput.BuildCommand(Lang.misc[64].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"],
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]
			}));
			Func<string> value6 = delegate()
			{
				int num33 = UILinkPointNavigator.CurrentPoint - 100;
				if (num33 % 10 == 8 && !Main.LocalPlayer.CanDemonHeartAccessoryBeShown())
				{
					num33++;
				}
				bool flag = num33 >= 10;
				int context = (num33 % 10 < 3) ? (flag ? 9 : 8) : (flag ? 11 : 10);
				return ItemSlot.GetGamepadInstructions(Main.LocalPlayer.armor, context, num33);
			};
			Func<string> value7 = delegate()
			{
				int num33 = UILinkPointNavigator.CurrentPoint - 120;
				if (num33 % 10 == 8 && !Main.LocalPlayer.CanDemonHeartAccessoryBeShown())
				{
					num33++;
				}
				return ItemSlot.GetGamepadInstructions(Main.LocalPlayer.dye, 12, num33);
			};
			for (int m = 100; m <= 119; m++)
			{
				UILinkPoint uilinkPoint4 = new UILinkPoint(m, true, m + 10, m - 10, m - 1, m + 1);
				uilinkPoint4.OnSpecialInteracts += value6;
				int num2 = m - 100;
				if (num2 == 0)
				{
					uilinkPoint4.Up = 305;
				}
				if (num2 == 10)
				{
					uilinkPoint4.Up = 306;
				}
				if (num2 == 9 || num2 == 19)
				{
					uilinkPoint4.Down = -2;
				}
				if (num2 >= 10)
				{
					uilinkPoint4.Left = 120 + num2 % 10;
				}
				else if (num2 >= 3)
				{
					uilinkPoint4.Right = -4;
				}
				else
				{
					uilinkPoint4.Right = 312 + num2;
				}
				cp6.LinkMap.Add(m, uilinkPoint4);
			}
			for (int n = 120; n <= 129; n++)
			{
				UILinkPoint uilinkPoint4 = new UILinkPoint(n, true, -3, -10 + n, n - 1, n + 1);
				uilinkPoint4.OnSpecialInteracts += value7;
				int num3 = n - 120;
				if (num3 == 0)
				{
					uilinkPoint4.Up = 307;
				}
				if (num3 == 9)
				{
					uilinkPoint4.Down = 308;
					uilinkPoint4.Left = 1557;
				}
				if (num3 == 8)
				{
					uilinkPoint4.Left = 1570;
				}
				cp6.LinkMap.Add(n, uilinkPoint4);
			}
			for (int num4 = 312; num4 <= 314; num4++)
			{
				int num5 = num4 - 312;
				UILinkPoint uilinkPoint4 = new UILinkPoint(num4, true, 100 + num5, -4, num4 - 1, num4 + 1);
				if (num5 == 0)
				{
					uilinkPoint4.Up = -1;
				}
				if (num5 == 2)
				{
					uilinkPoint4.Down = -2;
				}
				uilinkPoint4.OnSpecialInteracts += value;
				cp6.LinkMap.Add(num4, uilinkPoint4);
			}
			cp6.IsValidEvent += (() => Main.playerInventory && Main.EquipPage == 0);
			cp6.UpdateEvent += delegate()
			{
				int num33 = 107;
				int amountOfExtraAccessorySlotsToShow = Main.player[Main.myPlayer].GetAmountOfExtraAccessorySlotsToShow();
				for (int num34 = 0; num34 < amountOfExtraAccessorySlotsToShow; num34++)
				{
					cp6.LinkMap[num33 + num34].Down = num33 + num34 + 1;
					cp6.LinkMap[num33 - 100 + 120 + num34].Down = num33 - 100 + 120 + num34 + 1;
					cp6.LinkMap[num33 + 10 + num34].Down = num33 + 10 + num34 + 1;
				}
				cp6.LinkMap[num33 + amountOfExtraAccessorySlotsToShow].Down = 308;
				cp6.LinkMap[num33 + 10 + amountOfExtraAccessorySlotsToShow].Down = 308;
				cp6.LinkMap[num33 - 100 + 120 + amountOfExtraAccessorySlotsToShow].Down = 308;
				for (int num35 = 120; num35 <= 129; num35++)
				{
					UILinkPoint uilinkPoint16 = cp6.LinkMap[num35];
					int num36 = num35 - 120;
					uilinkPoint16.Left = -3;
					if (num36 == 0)
					{
						uilinkPoint16.Left = (Main.ShouldPVPDraw ? 1550 : -3);
					}
					if (num36 == 1)
					{
						uilinkPoint16.Left = (Main.ShouldTeamSelectDraw ? 1552 : -3);
					}
					if (num36 == 2)
					{
						uilinkPoint16.Left = (Main.ShouldTeamSelectDraw ? 1556 : -3);
					}
					if (num36 == 3)
					{
						uilinkPoint16.Left = ((UILinkPointNavigator.Shortcuts.INFOACCCOUNT >= 1) ? 1558 : -3);
					}
					if (num36 == 4)
					{
						uilinkPoint16.Left = ((UILinkPointNavigator.Shortcuts.INFOACCCOUNT >= 5) ? 1562 : -3);
					}
					if (num36 == 5)
					{
						uilinkPoint16.Left = ((UILinkPointNavigator.Shortcuts.INFOACCCOUNT >= 9) ? 1566 : -3);
					}
				}
				cp6.LinkMap[num33 - 100 + 120 + amountOfExtraAccessorySlotsToShow].Left = 1557;
				cp6.LinkMap[num33 - 100 + 120 + amountOfExtraAccessorySlotsToShow - 1].Left = 1570;
			};
			cp6.PageOnLeft = 8;
			cp6.PageOnRight = 8;
			UILinkPointNavigator.RegisterPage(cp6, 3, true);
			UILinkPage cp7 = new UILinkPage();
			cp7.OnSpecialInteracts += (() => PlayerInput.BuildCommand(Lang.misc[56].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]
			}) + PlayerInput.BuildCommand(Lang.misc[64].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"],
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]
			}));
			Func<string> value8 = delegate()
			{
				int slot = UILinkPointNavigator.CurrentPoint - 400;
				int context = 4;
				Item[] item = Main.player[Main.myPlayer].bank.item;
				switch (Main.player[Main.myPlayer].chest)
				{
				case -5:
					item = Main.player[Main.myPlayer].bank4.item;
					context = 32;
					break;
				case -4:
					item = Main.player[Main.myPlayer].bank3.item;
					break;
				case -3:
					item = Main.player[Main.myPlayer].bank2.item;
					break;
				case -2:
					break;
				case -1:
					return "";
				default:
					item = Main.chest[Main.player[Main.myPlayer].chest].item;
					context = 3;
					break;
				}
				return ItemSlot.GetGamepadInstructions(item, context, slot);
			};
			for (int num6 = 400; num6 <= 439; num6++)
			{
				UILinkPoint uilinkPoint5 = new UILinkPoint(num6, true, num6 - 1, num6 + 1, num6 - 10, num6 + 10);
				uilinkPoint5.OnSpecialInteracts += value8;
				int num7 = num6 - 400;
				if (num7 < 10)
				{
					uilinkPoint5.Up = 40 + num7;
				}
				if (num7 >= 30)
				{
					uilinkPoint5.Down = -2;
				}
				if (num7 % 10 == 9)
				{
					uilinkPoint5.Right = -4;
				}
				if (num7 % 10 == 0)
				{
					uilinkPoint5.Left = -3;
				}
				cp7.LinkMap.Add(num6, uilinkPoint5);
			}
			cp7.LinkMap.Add(500, new UILinkPoint(500, true, 409, -4, 53, 501));
			cp7.LinkMap.Add(501, new UILinkPoint(501, true, 419, -4, 500, 502));
			cp7.LinkMap.Add(502, new UILinkPoint(502, true, 429, -4, 501, 503));
			cp7.LinkMap.Add(503, new UILinkPoint(503, true, 439, -4, 502, 505));
			cp7.LinkMap.Add(505, new UILinkPoint(505, true, 439, -4, 503, 504));
			cp7.LinkMap.Add(504, new UILinkPoint(504, true, 439, -4, 505, 310));
			cp7.LinkMap[500].OnSpecialInteracts += value;
			cp7.LinkMap[501].OnSpecialInteracts += value;
			cp7.LinkMap[502].OnSpecialInteracts += value;
			cp7.LinkMap[503].OnSpecialInteracts += value;
			cp7.LinkMap[504].OnSpecialInteracts += value;
			cp7.LinkMap[505].OnSpecialInteracts += value;
			cp7.LinkMap[409].Right = 500;
			cp7.LinkMap[419].Right = 501;
			cp7.LinkMap[429].Right = 502;
			cp7.LinkMap[439].Right = 503;
			cp7.LinkMap[439].Down = 300;
			cp7.PageOnLeft = 0;
			cp7.PageOnRight = 0;
			cp7.DefaultPoint = 400;
			cp7.UpdateEvent += delegate()
			{
				if (Main.LocalPlayer.chest < -1)
				{
					cp7.LinkMap[505].Down = 310;
					return;
				}
				cp7.LinkMap[505].Down = 504;
			};
			UILinkPointNavigator.RegisterPage(cp7, 4, false);
			cp7.IsValidEvent += (() => Main.playerInventory && Main.player[Main.myPlayer].chest != -1 && !NewCraftingUI.Visible);
			UILinkPage uilinkPage2 = new UILinkPage();
			uilinkPage2.OnSpecialInteracts += (() => PlayerInput.BuildCommand(Lang.misc[56].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]
			}) + PlayerInput.BuildCommand(Lang.misc[64].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"],
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]
			}));
			Func<string> value9 = delegate()
			{
				int slot = UILinkPointNavigator.CurrentPoint - 5100;
				TEDisplayDoll tedisplayDoll = Main.LocalPlayer.tileEntityAnchor.GetTileEntity() as TEDisplayDoll;
				if (tedisplayDoll == null)
				{
					return "";
				}
				return tedisplayDoll.GetItemGamepadInstructions(slot);
			};
			int num8;
			UILinkPoint uilinkPoint6;
			for (num8 = 5100; num8 < 5118; num8++)
			{
				uilinkPoint6 = new UILinkPoint(num8, true, num8 - 1, num8 + 1, num8 - 9, num8 + 9);
				uilinkPoint6.OnSpecialInteracts += value9;
				int num9 = num8 - 5100;
				if (num9 < 9)
				{
					uilinkPoint6.Up = 40 + (int)Math.Round((double)(num9 + 1) * 0.9);
				}
				if (num9 >= 9)
				{
					uilinkPoint6.Down = -2;
				}
				if (num9 % 9 == 8)
				{
					uilinkPoint6.Right = -4;
				}
				if (num9 % 9 == 0)
				{
					uilinkPoint6.Left = -3;
				}
				uilinkPage2.LinkMap.Add(num8, uilinkPoint6);
			}
			uilinkPoint6 = new UILinkPoint(num8, true, -3, 5100, 40, -2);
			uilinkPoint6.OnSpecialInteracts += value9;
			uilinkPage2.LinkMap.Add(num8, uilinkPoint6);
			uilinkPage2.LinkMap[5100].Left = num8;
			uilinkPage2.PageOnLeft = 0;
			uilinkPage2.PageOnRight = 0;
			uilinkPage2.DefaultPoint = 5100;
			UILinkPointNavigator.RegisterPage(uilinkPage2, 20, false);
			uilinkPage2.IsValidEvent += (() => Main.playerInventory && Main.LocalPlayer.tileEntityAnchor.GetTileEntity() is TEDisplayDoll);
			UILinkPage uilinkPage3 = new UILinkPage();
			uilinkPage3.OnSpecialInteracts += (() => PlayerInput.BuildCommand(Lang.misc[56].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]
			}) + PlayerInput.BuildCommand(Lang.misc[64].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"],
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]
			}));
			Func<string> value10 = delegate()
			{
				int slot = UILinkPointNavigator.CurrentPoint - 5000;
				TEHatRack tehatRack = Main.LocalPlayer.tileEntityAnchor.GetTileEntity() as TEHatRack;
				if (tehatRack == null)
				{
					return "";
				}
				return tehatRack.GetItemGamepadInstructions(slot);
			};
			for (int num10 = 5000; num10 <= 5003; num10++)
			{
				UILinkPoint uilinkPoint7 = new UILinkPoint(num10, true, num10 - 1, num10 + 1, num10 - 2, num10 + 2);
				uilinkPoint7.OnSpecialInteracts += value10;
				int num11 = num10 - 5000;
				if (num11 < 2)
				{
					uilinkPoint7.Up = 44 + num11;
				}
				if (num11 >= 2)
				{
					uilinkPoint7.Down = -2;
				}
				if (num11 % 2 == 1)
				{
					uilinkPoint7.Right = -4;
				}
				if (num11 % 2 == 0)
				{
					uilinkPoint7.Left = -3;
				}
				uilinkPage3.LinkMap.Add(num10, uilinkPoint7);
			}
			uilinkPage3.PageOnLeft = 0;
			uilinkPage3.PageOnRight = 0;
			uilinkPage3.DefaultPoint = 5000;
			UILinkPointNavigator.RegisterPage(uilinkPage3, 21, false);
			uilinkPage3.IsValidEvent += (() => Main.playerInventory && Main.LocalPlayer.tileEntityAnchor.GetTileEntity() is TEHatRack);
			UILinkPage uilinkPage4 = new UILinkPage();
			uilinkPage4.OnSpecialInteracts += (() => PlayerInput.BuildCommand(Lang.misc[56].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]
			}) + PlayerInput.BuildCommand(Lang.misc[64].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"],
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]
			}));
			Func<string> value11 = delegate()
			{
				if (Main.npcShop == 0)
				{
					return "";
				}
				int slot = UILinkPointNavigator.CurrentPoint - 2700;
				return ItemSlot.GetGamepadInstructions(Main.instance.shop[Main.npcShop].item, 15, slot);
			};
			for (int num12 = 2700; num12 <= 2739; num12++)
			{
				UILinkPoint uilinkPoint8 = new UILinkPoint(num12, true, num12 - 1, num12 + 1, num12 - 10, num12 + 10);
				uilinkPoint8.OnSpecialInteracts += value11;
				int num13 = num12 - 2700;
				if (num13 < 10)
				{
					uilinkPoint8.Up = 40 + num13;
				}
				if (num13 >= 30)
				{
					uilinkPoint8.Down = -2;
				}
				if (num13 % 10 == 9)
				{
					uilinkPoint8.Right = -4;
				}
				if (num13 % 10 == 0)
				{
					uilinkPoint8.Left = -3;
				}
				uilinkPage4.LinkMap.Add(num12, uilinkPoint8);
			}
			uilinkPage4.LinkMap[2739].Down = 300;
			uilinkPage4.PageOnLeft = 0;
			uilinkPage4.PageOnRight = 0;
			UILinkPointNavigator.RegisterPage(uilinkPage4, 13, true);
			uilinkPage4.IsValidEvent += (() => Main.playerInventory && Main.npcShop != 0);
			UILinkPage cp8 = new UILinkPage();
			cp8.LinkMap.Add(303, new UILinkPoint(303, true, 304, 304, 40, -2));
			cp8.LinkMap.Add(304, new UILinkPoint(304, true, 303, 303, 40, -2));
			cp8.OnSpecialInteracts += (() => PlayerInput.BuildCommand(Lang.misc[56].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]
			}) + PlayerInput.BuildCommand(Lang.misc[64].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"],
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]
			}));
			Func<string> value12 = () => ItemSlot.GetGamepadInstructions(ref Main.reforgeItem, 5);
			cp8.LinkMap[303].OnSpecialInteracts += value12;
			cp8.LinkMap[304].OnSpecialInteracts += (() => Lang.misc[53].Value);
			cp8.UpdateEvent += delegate()
			{
				if (Main.reforgeItem.type > 0)
				{
					cp8.LinkMap[303].Left = (cp8.LinkMap[303].Right = 304);
					return;
				}
				if (UILinkPointNavigator.OverridePoint == -1 && cp8.CurrentPoint == 304)
				{
					UILinkPointNavigator.ChangePoint(303);
				}
				cp8.LinkMap[303].Left = -3;
				cp8.LinkMap[303].Right = -4;
			};
			cp8.IsValidEvent += (() => Main.playerInventory && Main.InReforgeMenu);
			cp8.PageOnLeft = 0;
			cp8.PageOnRight = 0;
			cp8.EnterEvent += delegate()
			{
				PlayerInput.LockGamepadButtons("MouseLeft");
			};
			UILinkPointNavigator.RegisterPage(cp8, 5, true);
			UILinkPage cp9 = new UILinkPage();
			cp9.OnSpecialInteracts += delegate()
			{
				string text = PlayerInput.BuildCommand(Lang.misc[56].Value, new List<string>[]
				{
					PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]
				}) + PlayerInput.BuildCommand(Lang.misc[64].Value, new List<string>[]
				{
					PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"],
					PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]
				});
				if (PlayerInput.ControllerHousingCursorActive)
				{
					bool flag = UILinkPointNavigator.CurrentPoint == 600;
					bool flag2 = UILinkPointNavigator.Shortcuts.NPCS_HoveredBanner >= 0;
					if (flag2)
					{
						string fullName = Main.npc[UILinkPointNavigator.Shortcuts.NPCS_HoveredBanner].FullName;
						text += PlayerInput.BuildCommand(Language.GetTextValue("UI.HousingEvict", fullName), new List<string>[]
						{
							PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]
						});
					}
					else if (flag)
					{
						text += PlayerInput.BuildCommand(Lang.misc[70].Value, new List<string>[]
						{
							PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]
						});
					}
					else if (UILinkPointNavigator.Shortcuts.NPCS_SelectedNPC >= 0)
					{
						string fullName2 = Main.npc[UILinkPointNavigator.Shortcuts.NPCS_SelectedNPC].FullName;
						text += PlayerInput.BuildCommand(Language.GetTextValue("UI.HousingAssign", fullName2), new List<string>[]
						{
							PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]
						});
					}
					if (UILinksInitializer.CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.Grapple)
					{
						Point point = PlayerInput.HousingWorldPosition.ToTileCoordinates();
						if (flag2)
						{
							WorldGen.kickOut(UILinkPointNavigator.Shortcuts.NPCS_HoveredBanner);
							SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
						}
						else if (flag)
						{
							Main.instance.PerformHousingCheck(point.X, point.Y);
						}
						else if (UILinkPointNavigator.Shortcuts.NPCS_SelectedNPC >= 0)
						{
							Main.instance.TryMovingNPC(point.X, point.Y, UILinkPointNavigator.Shortcuts.NPCS_SelectedNPC);
						}
						PlayerInput.LockGamepadButtons("Grapple");
						PlayerInput.SettingsForUI.TryRevertingToMouseMode();
					}
					text += PlayerInput.BuildCommand(Language.GetTextValue("UI.HousingAim"), new List<string>[]
					{
						UILinksInitializer.RightStickGlyphBinding
					});
				}
				return text;
			};
			for (int num14 = 600; num14 <= 650; num14++)
			{
				UILinkPoint value13 = new UILinkPoint(num14, true, num14 + 10, num14 - 10, num14 - 1, num14 + 1);
				cp9.LinkMap.Add(num14, value13);
			}
			cp9.UpdateEvent += delegate()
			{
				int num33 = UILinkPointNavigator.Shortcuts.NPCS_IconsPerColumn;
				if (num33 == 0)
				{
					num33 = 100;
				}
				for (int num34 = 0; num34 < 50; num34++)
				{
					cp9.LinkMap[600 + num34].Up = ((num34 % num33 == 0) ? -1 : (600 + num34 - 1));
					if (cp9.LinkMap[600 + num34].Up == -1)
					{
						if (num34 >= num33 * 2)
						{
							cp9.LinkMap[600 + num34].Up = 307;
						}
						else if (num34 >= num33)
						{
							cp9.LinkMap[600 + num34].Up = 306;
						}
						else
						{
							cp9.LinkMap[600 + num34].Up = 305;
						}
					}
					cp9.LinkMap[600 + num34].Down = (((num34 + 1) % num33 == 0 || num34 == UILinkPointNavigator.Shortcuts.NPCS_IconsTotal - 1) ? 308 : (600 + num34 + 1));
					cp9.LinkMap[600 + num34].Left = ((num34 < UILinkPointNavigator.Shortcuts.NPCS_IconsTotal - num33) ? (600 + num34 + num33) : -3);
					cp9.LinkMap[600 + num34].Right = ((num34 < num33) ? -4 : (600 + num34 - num33));
				}
			};
			cp9.IsValidEvent += (() => Main.playerInventory && Main.EquipPage == 1);
			cp9.PageOnLeft = 8;
			cp9.PageOnRight = 8;
			UILinkPointNavigator.RegisterPage(cp9, 6, true);
			UILinkPage cp10 = new UILinkPage();
			cp10.OnSpecialInteracts += (() => PlayerInput.BuildCommand(Lang.misc[56].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]
			}) + PlayerInput.BuildCommand(Lang.misc[64].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"],
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]
			}));
			Func<string> value14 = delegate()
			{
				int slot = UILinkPointNavigator.CurrentPoint - 180;
				return ItemSlot.GetGamepadInstructions(Main.player[Main.myPlayer].miscEquips, 20, slot);
			};
			Func<string> value15 = delegate()
			{
				int slot = UILinkPointNavigator.CurrentPoint - 180;
				return ItemSlot.GetGamepadInstructions(Main.player[Main.myPlayer].miscEquips, 19, slot);
			};
			Func<string> value16 = delegate()
			{
				int slot = UILinkPointNavigator.CurrentPoint - 180;
				return ItemSlot.GetGamepadInstructions(Main.player[Main.myPlayer].miscEquips, 18, slot);
			};
			Func<string> value17 = delegate()
			{
				int slot = UILinkPointNavigator.CurrentPoint - 180;
				return ItemSlot.GetGamepadInstructions(Main.player[Main.myPlayer].miscEquips, 17, slot);
			};
			Func<string> value18 = delegate()
			{
				int slot = UILinkPointNavigator.CurrentPoint - 180;
				return ItemSlot.GetGamepadInstructions(Main.player[Main.myPlayer].miscEquips, 16, slot);
			};
			Func<string> value19 = delegate()
			{
				int slot = UILinkPointNavigator.CurrentPoint - 185;
				return ItemSlot.GetGamepadInstructions(Main.player[Main.myPlayer].miscDyes, 33, slot);
			};
			for (int num15 = 180; num15 <= 184; num15++)
			{
				UILinkPoint uilinkPoint9 = new UILinkPoint(num15, true, 185 + num15 - 180, -4, num15 - 1, num15 + 1);
				int num16 = num15 - 180;
				if (num16 == 0)
				{
					uilinkPoint9.Up = 305;
				}
				if (num16 == 4)
				{
					uilinkPoint9.Down = 308;
				}
				cp10.LinkMap.Add(num15, uilinkPoint9);
				switch (num15)
				{
				case 180:
					uilinkPoint9.OnSpecialInteracts += value15;
					break;
				case 181:
					uilinkPoint9.OnSpecialInteracts += value14;
					break;
				case 182:
					uilinkPoint9.OnSpecialInteracts += value16;
					break;
				case 183:
					uilinkPoint9.OnSpecialInteracts += value17;
					break;
				case 184:
					uilinkPoint9.OnSpecialInteracts += value18;
					break;
				}
			}
			for (int num17 = 185; num17 <= 189; num17++)
			{
				UILinkPoint uilinkPoint9 = new UILinkPoint(num17, true, -3, -5 + num17, num17 - 1, num17 + 1);
				uilinkPoint9.OnSpecialInteracts += value19;
				int num18 = num17 - 185;
				if (num18 == 0)
				{
					uilinkPoint9.Up = 306;
				}
				if (num18 == 4)
				{
					uilinkPoint9.Down = 308;
				}
				cp10.LinkMap.Add(num17, uilinkPoint9);
			}
			cp10.UpdateEvent += delegate()
			{
				cp10.LinkMap[184].Down = ((UILinkPointNavigator.Shortcuts.BUFFS_DRAWN > 0) ? 9000 : 308);
				cp10.LinkMap[189].Down = ((UILinkPointNavigator.Shortcuts.BUFFS_DRAWN > 0) ? 9000 : 308);
			};
			cp10.IsValidEvent += (() => Main.playerInventory && Main.EquipPage == 2);
			cp10.PageOnLeft = 8;
			cp10.PageOnRight = 8;
			UILinkPointNavigator.RegisterPage(cp10, 7, true);
			UILinkPage cp11 = new UILinkPage();
			cp11.OnSpecialInteracts += (() => PlayerInput.BuildCommand(Lang.misc[56].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]
			}) + PlayerInput.BuildCommand(Lang.misc[64].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"],
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]
			}));
			cp11.LinkMap.Add(305, new UILinkPoint(305, true, 306, -4, 308, -2));
			cp11.LinkMap.Add(306, new UILinkPoint(306, true, 307, 305, 308, -2));
			cp11.LinkMap.Add(307, new UILinkPoint(307, true, -3, 306, 308, -2));
			cp11.LinkMap.Add(308, new UILinkPoint(308, true, -3, -4, -1, 305));
			cp11.LinkMap[305].OnSpecialInteracts += value;
			cp11.LinkMap[306].OnSpecialInteracts += value;
			cp11.LinkMap[307].OnSpecialInteracts += value;
			cp11.LinkMap[308].OnSpecialInteracts += value;
			cp11.UpdateEvent += delegate()
			{
				switch (Main.EquipPage)
				{
				case 0:
					cp11.LinkMap[305].Down = 100;
					cp11.LinkMap[306].Down = 110;
					cp11.LinkMap[307].Down = 120;
					cp11.LinkMap[308].Up = 108 + Main.player[Main.myPlayer].GetAmountOfExtraAccessorySlotsToShow() - 1;
					break;
				case 1:
				{
					cp11.LinkMap[305].Down = 600;
					cp11.LinkMap[306].Down = ((UILinkPointNavigator.Shortcuts.NPCS_IconsTotal > UILinkPointNavigator.Shortcuts.NPCS_IconsPerColumn) ? (600 + UILinkPointNavigator.Shortcuts.NPCS_IconsPerColumn) : 600);
					cp11.LinkMap[307].Down = ((UILinkPointNavigator.Shortcuts.NPCS_IconsTotal > UILinkPointNavigator.Shortcuts.NPCS_IconsPerColumn * 2) ? (600 + UILinkPointNavigator.Shortcuts.NPCS_IconsPerColumn * 2) : cp11.LinkMap[306].Down);
					int num33 = UILinkPointNavigator.Shortcuts.NPCS_IconsPerColumn;
					if (num33 == 0)
					{
						num33 = 100;
					}
					if (num33 == 100)
					{
						num33 = UILinkPointNavigator.Shortcuts.NPCS_IconsTotal;
					}
					cp11.LinkMap[308].Up = 600 + num33 - 1;
					break;
				}
				case 2:
					cp11.LinkMap[305].Down = 180;
					cp11.LinkMap[306].Down = 185;
					cp11.LinkMap[307].Down = -2;
					cp11.LinkMap[308].Up = ((UILinkPointNavigator.Shortcuts.BUFFS_DRAWN > 0) ? 9000 : 184);
					break;
				}
				cp11.PageOnRight = UILinksInitializer.GetCornerWrapPageIdFromRightToLeft();
			};
			cp11.IsValidEvent += (() => Main.playerInventory);
			cp11.PageOnLeft = 0;
			cp11.PageOnRight = 0;
			UILinkPointNavigator.RegisterPage(cp11, 8, true);
			UILinkPage cp12 = new UILinkPage();
			cp12.OnSpecialInteracts += (() => PlayerInput.BuildCommand(Lang.misc[56].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]
			}) + PlayerInput.BuildCommand(Lang.misc[64].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"],
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]
			}));
			cp12.OnSpecialInteractsLate += (() => ItemSlot.GetGamepadInstructions(Main.InPipBanner ? 35 : 22));
			for (int num19 = 1500; num19 < 1550; num19++)
			{
				UILinkPoint value20 = new UILinkPoint(num19, true, num19, num19, -1, -2);
				cp12.LinkMap.Add(num19, value20);
			}
			cp12.LinkMap.Add(11001, new UILinkPoint(11001, true, 1501, 11002, -1, 11003));
			cp12.LinkMap.Add(11002, new UILinkPoint(11002, true, 11001, -4, -1, 11003));
			cp12.LinkMap.Add(11003, new UILinkPoint(11003, true, 1501, -4, 11001, 1502));
			cp12.LinkMap[1500].OnSpecialInteracts += (() => ItemSlot.GetGamepadInstructions(ref Main.guideItem, 7));
			cp12.LinkMap[11001].OnSpecialInteracts += (() => PlayerInput.BuildCommand(Language.GetTextValue("UI.ToggleClassicGrid"), new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"]
			}));
			cp12.UpdateEvent += delegate()
			{
				cp12.PageOnLeft = ((Player.Settings.CraftingGridControl == Player.Settings.CraftingGridMode.Classic) ? 10 : 8);
				int num33 = UILinkPointNavigator.Shortcuts.CRAFT_CurrentIngredientsCount;
				int num34 = num33;
				if (UILinksInitializer.MainnumAvailableRecipes > 0)
				{
					num34 += 2;
				}
				if (num33 < num34)
				{
					num33 = num34;
				}
				if (UILinkPointNavigator.OverridePoint == -1)
				{
					if (cp12.CurrentPoint == 11003)
					{
						if (Main.InGuideCraftMenu)
						{
							UILinkPointNavigator.ChangePoint(1501);
						}
					}
					else if (cp12.CurrentPoint != 11001)
					{
						if (cp12.CurrentPoint == 11002)
						{
							if (!Main.bannerUI.AnyAvailableBanners || Main.InGuideCraftMenu)
							{
								UILinkPointNavigator.ChangePoint(11001);
							}
						}
						else if (cp12.CurrentPoint == 1500)
						{
							if (!Main.InGuideCraftMenu)
							{
								UILinkPointNavigator.ChangePoint(1501);
							}
						}
						else if (cp12.CurrentPoint > 1500 + num33)
						{
							UILinkPointNavigator.ChangePoint(1500);
						}
					}
				}
				bool flag = Main.LocalPlayer.chest < 0;
				for (int num35 = 1; num35 < num33; num35++)
				{
					cp12.LinkMap[1500 + num35].Left = 1500 + num35 - 1;
					cp12.LinkMap[1500 + num35].Right = ((num35 == num33 - 2) ? -4 : (1500 + num35 + 1));
					if (num35 >= 2)
					{
						cp12.LinkMap[1500 + num35].Up = (Main.InGuideCraftMenu ? 1500 : (flag ? 11003 : -1));
						cp12.LinkMap[1500 + num35].Down = (flag ? -1 : ((num35 >= 3 && Main.bannerUI.AnyAvailableBanners) ? 11002 : 11001));
					}
				}
				cp12.LinkMap[1501].Left = -3;
				if (num33 > 0)
				{
					cp12.LinkMap[1500 + num33 - 1].Right = -4;
				}
				cp12.LinkMap[1500].Down = ((num33 >= 2) ? 1502 : -2);
				cp12.LinkMap[1500].Left = ((num33 >= 1) ? 1501 : -3);
				cp12.LinkMap[1500].Up = 11001;
				cp12.LinkMap[11001].Left = (Main.InPipCrafting ? 1501 : 12000);
				cp12.LinkMap[11001].Down = ((!Main.InPipCrafting) ? -1 : (Main.InGuideCraftMenu ? 1500 : 11003));
				cp12.LinkMap[11001].Right = ((!Main.bannerUI.AnyAvailableBanners || Main.InGuideCraftMenu) ? -1 : 11002);
				cp12.LinkMap[11001].Up = (flag ? -1 : 1502);
				cp12.LinkMap[11002].Down = ((!Main.InPipCrafting) ? -1 : 11003);
				cp12.LinkMap[11002].Up = (flag ? -1 : ((num33 >= 5) ? 1503 : 1502));
				cp12.LinkMap[11003].Down = (flag ? 1502 : -1);
			};
			cp12.LinkMap[1501].OnSpecialInteracts += (() => ItemSlot.GetCraftSlotGamepadInstructions());
			cp12.ReachEndEvent += delegate(int current, int next)
			{
				if (current != 1500)
				{
					if (current == 1501)
					{
						if (next == -1)
						{
							if (UILinksInitializer.MainfocusRecipe > 0)
							{
								UILinksInitializer.MainfocusRecipe--;
								return;
							}
						}
						else if (next == -2 && UILinksInitializer.MainfocusRecipe < UILinksInitializer.MainnumAvailableRecipes - 1)
						{
							UILinksInitializer.MainfocusRecipe++;
							return;
						}
					}
					else if (next == -1)
					{
						if (UILinksInitializer.MainfocusRecipe > 0)
						{
							UILinkPointNavigator.ChangePoint(1501);
							UILinksInitializer.MainfocusRecipe--;
							return;
						}
					}
					else if (next == -2 && UILinksInitializer.MainfocusRecipe < UILinksInitializer.MainnumAvailableRecipes - 1)
					{
						UILinkPointNavigator.ChangePoint(1501);
						UILinksInitializer.MainfocusRecipe++;
					}
				}
			};
			cp12.EnterEvent += delegate()
			{
				Main.PipsUseGrid = false;
				PlayerInput.LockGamepadButtons("MouseLeft");
			};
			cp12.CanEnterEvent += (() => Main.playerInventory && (UILinksInitializer.MainnumAvailableRecipes > 0 || Main.InGuideCraftMenu));
			cp12.IsValidEvent += (() => Main.playerInventory && (UILinksInitializer.MainnumAvailableRecipes > 0 || Main.InGuideCraftMenu));
			cp12.PageOnLeft = 8;
			cp12.PageOnRight = 0;
			UILinkPointNavigator.RegisterPage(cp12, 9, true);
			UILinkPage cp13 = new UILinkPage();
			cp13.OnSpecialInteracts += (() => PlayerInput.BuildCommand(Lang.misc[56].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]
			}) + PlayerInput.BuildCommand(Lang.misc[64].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"],
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]
			}));
			cp13.OnSpecialInteractsLate += (() => ItemSlot.GetGamepadInstructions(Main.InPipBanner ? 35 : 22));
			for (int num20 = 22000; num20 < 30000; num20++)
			{
				UILinkPoint uilinkPoint10 = new UILinkPoint(num20, true, num20, num20, num20, num20);
				int IHateLambda = num20;
				uilinkPoint10.OnSpecialInteracts += delegate()
				{
					string text = PlayerInput.BuildCommand(Lang.misc[73].Value, new List<string>[]
					{
						PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]
					});
					if (UILinksInitializer.TryQuickCrafting(22000, IHateLambda))
					{
						text += PlayerInput.BuildCommand(Lang.misc[71].Value, new List<string>[]
						{
							PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]
						});
					}
					return text;
				};
				cp13.LinkMap.Add(num20, uilinkPoint10);
			}
			cp13.UpdateEvent += delegate()
			{
				int num33 = UILinkPointNavigator.Shortcuts.CRAFT_IconsPerRow;
				int craft_IconsPerColumn = UILinkPointNavigator.Shortcuts.CRAFT_IconsPerColumn;
				if (num33 == 0)
				{
					num33 = 100;
				}
				int num34 = num33 * craft_IconsPerColumn;
				if (num34 > 8000)
				{
					num34 = 8000;
				}
				if (num34 > UILinksInitializer.MainnumAvailableRecipes)
				{
					num34 = UILinksInitializer.MainnumAvailableRecipes;
				}
				for (int num35 = 0; num35 < num34; num35++)
				{
					cp13.LinkMap[22000 + num35].Left = ((num35 % num33 == 0) ? -3 : (22000 + num35 - 1));
					cp13.LinkMap[22000 + num35].Right = (((num35 + 1) % num33 == 0 || num35 == UILinksInitializer.MainnumAvailableRecipes - 1) ? -4 : (22000 + num35 + 1));
					cp13.LinkMap[22000 + num35].Down = ((num35 < num34 - num33) ? (22000 + num35 + num33) : -2);
					cp13.LinkMap[22000 + num35].Up = ((num35 < num33) ? -1 : (22000 + num35 - num33));
				}
				cp13.PageOnLeft = UILinksInitializer.GetCornerWrapPageIdFromLeftToRight();
			};
			cp13.ReachEndEvent += delegate(int current, int next)
			{
				int craft_IconsPerRow = UILinkPointNavigator.Shortcuts.CRAFT_IconsPerRow;
				if (next == -1)
				{
					Main.recStart -= craft_IconsPerRow;
					if (Main.recStart < 0)
					{
						Main.recStart = 0;
						return;
					}
				}
				else if (next == -2)
				{
					Main.recStart += craft_IconsPerRow;
					SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
					if (Main.recStart > UILinksInitializer.MainnumAvailableRecipes - craft_IconsPerRow)
					{
						Main.recStart = UILinksInitializer.MainnumAvailableRecipes - craft_IconsPerRow;
					}
				}
			};
			cp13.EnterEvent += delegate()
			{
				Main.PipsUseGrid = true;
			};
			cp13.LeaveEvent += delegate()
			{
				Main.PipsUseGrid = false;
			};
			cp13.CanEnterEvent += (() => Main.playerInventory && UILinksInitializer.MainnumAvailableRecipes > 0);
			cp13.IsValidEvent += (() => Main.playerInventory && Main.PipsUseGrid && UILinksInitializer.MainnumAvailableRecipes > 0);
			cp13.PageOnLeft = 0;
			cp13.PageOnRight = 9;
			UILinkPointNavigator.RegisterPage(cp13, 10, true);
			UILinkPage cp14 = new UILinkPage();
			cp14.OnSpecialInteracts += (() => PlayerInput.BuildCommand(Lang.misc[56].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]
			}) + PlayerInput.BuildCommand(Lang.misc[64].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"],
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]
			}));
			for (int num21 = 2605; num21 < 2620; num21++)
			{
				UILinkPoint uilinkPoint11 = new UILinkPoint(num21, true, num21, num21, num21, num21);
				uilinkPoint11.OnSpecialInteracts += (() => PlayerInput.BuildCommand(Lang.misc[73].Value, new List<string>[]
				{
					PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]
				}));
				cp14.LinkMap.Add(num21, uilinkPoint11);
			}
			cp14.UpdateEvent += delegate()
			{
				int num33 = 5;
				int num34 = 3;
				int num35 = num33 * num34;
				int count = Main.Hairstyles.AvailableHairstyles.Count;
				for (int num36 = 0; num36 < num35; num36++)
				{
					cp14.LinkMap[2605 + num36].Left = ((num36 % num33 == 0) ? -3 : (2605 + num36 - 1));
					cp14.LinkMap[2605 + num36].Right = (((num36 + 1) % num33 == 0 || num36 == count - 1) ? -4 : (2605 + num36 + 1));
					cp14.LinkMap[2605 + num36].Down = ((num36 < num35 - num33) ? (2605 + num36 + num33) : -2);
					cp14.LinkMap[2605 + num36].Up = ((num36 < num33) ? -1 : (2605 + num36 - num33));
				}
			};
			cp14.ReachEndEvent += delegate(int current, int next)
			{
				int num33 = 5;
				if (next == -1)
				{
					Main.hairStart -= num33;
					SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
					return;
				}
				if (next == -2)
				{
					Main.hairStart += num33;
					SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
				}
			};
			cp14.CanEnterEvent += (() => Main.hairWindow);
			cp14.IsValidEvent += (() => Main.hairWindow);
			cp14.PageOnLeft = 12;
			cp14.PageOnRight = 12;
			UILinkPointNavigator.RegisterPage(cp14, 11, true);
			UILinkPage uilinkPage5 = new UILinkPage();
			uilinkPage5.OnSpecialInteracts += (() => PlayerInput.BuildCommand(Lang.misc[56].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]
			}) + PlayerInput.BuildCommand(Lang.misc[64].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"],
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]
			}));
			uilinkPage5.LinkMap.Add(2600, new UILinkPoint(2600, true, -3, -4, -1, 2601));
			uilinkPage5.LinkMap.Add(2601, new UILinkPoint(2601, true, -3, -4, 2600, 2602));
			uilinkPage5.LinkMap.Add(2602, new UILinkPoint(2602, true, -3, -4, 2601, 2603));
			uilinkPage5.LinkMap.Add(2603, new UILinkPoint(2603, true, -3, 2604, 2602, -2));
			uilinkPage5.LinkMap.Add(2604, new UILinkPoint(2604, true, 2603, -4, 2602, -2));
			uilinkPage5.UpdateEvent += delegate()
			{
				Vector3 value24 = Main.rgbToHsl(Main.selColor);
				float interfaceDeadzoneX = PlayerInput.CurrentProfile.InterfaceDeadzoneX;
				float num33 = PlayerInput.GamepadThumbstickLeft.X;
				if (num33 < -interfaceDeadzoneX || num33 > interfaceDeadzoneX)
				{
					num33 = MathHelper.Lerp(0f, 0.008333334f, (Math.Abs(num33) - interfaceDeadzoneX) / (1f - interfaceDeadzoneX)) * (float)Math.Sign(num33);
				}
				else
				{
					num33 = 0f;
				}
				int currentPoint = UILinkPointNavigator.CurrentPoint;
				if (currentPoint == 2600)
				{
					Main.hBar = MathHelper.Clamp(Main.hBar + num33, 0f, 1f);
				}
				if (currentPoint == 2601)
				{
					Main.sBar = MathHelper.Clamp(Main.sBar + num33, 0f, 1f);
				}
				if (currentPoint == 2602)
				{
					Main.lBar = MathHelper.Clamp(Main.lBar + num33, 0.15f, 1f);
				}
				Vector3.Clamp(value24, Vector3.Zero, Vector3.One);
				if (num33 != 0f)
				{
					if (Main.hairWindow)
					{
						Main.player[Main.myPlayer].hairColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar, byte.MaxValue));
					}
					SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
				}
			};
			uilinkPage5.CanEnterEvent += (() => Main.hairWindow);
			uilinkPage5.IsValidEvent += (() => Main.hairWindow);
			uilinkPage5.PageOnLeft = 11;
			uilinkPage5.PageOnRight = 11;
			UILinkPointNavigator.RegisterPage(uilinkPage5, 12, true);
			UILinkPage cp15 = new UILinkPage();
			for (int num22 = 0; num22 < 30; num22++)
			{
				cp15.LinkMap.Add(2900 + num22, new UILinkPoint(2900 + num22, true, -3, -4, -1, -2));
				cp15.LinkMap[2900 + num22].OnSpecialInteracts += value;
			}
			cp15.OnSpecialInteracts += (() => PlayerInput.BuildCommand(Lang.misc[56].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]
			}) + PlayerInput.BuildCommand(Lang.misc[64].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"],
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]
			}));
			cp15.TravelEvent += delegate()
			{
				if (UILinkPointNavigator.CurrentPage == cp15.ID)
				{
					int num33 = cp15.CurrentPoint - 2900;
					if (num33 < 5)
					{
						IngameOptions.category = num33;
					}
				}
			};
			cp15.UpdateEvent += delegate()
			{
				int num33 = UILinkPointNavigator.Shortcuts.INGAMEOPTIONS_BUTTONS_LEFT;
				if (num33 == 0)
				{
					num33 = 5;
				}
				if (UILinkPointNavigator.OverridePoint == -1 && cp15.CurrentPoint < 2930 && cp15.CurrentPoint > 2900 + num33 - 1)
				{
					UILinkPointNavigator.ChangePoint(2900);
				}
				for (int num34 = 2900; num34 < 2900 + num33; num34++)
				{
					cp15.LinkMap[num34].Up = num34 - 1;
					cp15.LinkMap[num34].Down = num34 + 1;
				}
				cp15.LinkMap[2900].Up = 2900 + num33 - 1;
				cp15.LinkMap[2900 + num33 - 1].Down = 2900;
				int num35 = cp15.CurrentPoint - 2900;
				if (num35 < 4 && UILinksInitializer.CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.MouseLeft)
				{
					IngameOptions.category = num35;
					UILinkPointNavigator.ChangePage(1002);
				}
				int num36 = (SocialAPI.Network != null && SocialAPI.Network.CanInvite()) ? 1 : 0;
				if (num35 == 4 + num36 && UILinksInitializer.CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.MouseLeft)
				{
					UILinkPointNavigator.ChangePage(1004);
				}
			};
			cp15.EnterEvent += delegate()
			{
				cp15.CurrentPoint = 2900 + IngameOptions.category;
			};
			cp15.PageOnLeft = (cp15.PageOnRight = 1002);
			cp15.IsValidEvent += (() => Main.ingameOptionsWindow && !Main.InGameUI.IsVisible);
			cp15.CanEnterEvent += (() => Main.ingameOptionsWindow && !Main.InGameUI.IsVisible);
			UILinkPointNavigator.RegisterPage(cp15, 1001, true);
			UILinkPage cp16 = new UILinkPage();
			for (int num23 = 0; num23 < 30; num23++)
			{
				cp16.LinkMap.Add(2930 + num23, new UILinkPoint(2930 + num23, true, -3, -4, -1, -2));
				cp16.LinkMap[2930 + num23].OnSpecialInteracts += value;
			}
			cp16.EnterEvent += delegate()
			{
				Main.mouseLeftRelease = false;
			};
			cp16.OnSpecialInteracts += (() => PlayerInput.BuildCommand(Lang.misc[56].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]
			}) + PlayerInput.BuildCommand(Lang.misc[64].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"],
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]
			}));
			cp16.UpdateEvent += delegate()
			{
				int num33 = UILinkPointNavigator.Shortcuts.INGAMEOPTIONS_BUTTONS_RIGHT;
				if (num33 == 0)
				{
					num33 = 5;
				}
				if (UILinkPointNavigator.OverridePoint == -1 && cp16.CurrentPoint >= 2930 && cp16.CurrentPoint > 2930 + num33 - 1)
				{
					UILinkPointNavigator.ChangePoint(2930);
				}
				for (int num34 = 2930; num34 < 2930 + num33; num34++)
				{
					cp16.LinkMap[num34].Up = num34 - 1;
					cp16.LinkMap[num34].Down = num34 + 1;
				}
				cp16.LinkMap[2930].Up = -1;
				cp16.LinkMap[2930 + num33 - 1].Down = -2;
				UILinksInitializer.HandleOptionsSpecials();
			};
			cp16.PageOnLeft = (cp16.PageOnRight = 1001);
			cp16.IsValidEvent += (() => Main.ingameOptionsWindow);
			cp16.CanEnterEvent += (() => Main.ingameOptionsWindow);
			UILinkPointNavigator.RegisterPage(cp16, 1002, true);
			UILinkPage cp17 = new UILinkPage();
			cp17.OnSpecialInteracts += (() => PlayerInput.BuildCommand(Lang.misc[56].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]
			}) + PlayerInput.BuildCommand(Lang.misc[64].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"],
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]
			}));
			for (int num24 = 1550; num24 < 1558; num24++)
			{
				UILinkPoint uilinkPoint12 = new UILinkPoint(num24, true, -3, -4, -1, -2);
				switch (num24 - 1550)
				{
				case 1:
				case 3:
				case 5:
					uilinkPoint12.Up = uilinkPoint12.ID - 2;
					uilinkPoint12.Down = uilinkPoint12.ID + 2;
					uilinkPoint12.Right = uilinkPoint12.ID + 1;
					break;
				case 2:
				case 4:
				case 6:
					uilinkPoint12.Up = uilinkPoint12.ID - 2;
					uilinkPoint12.Down = uilinkPoint12.ID + 2;
					uilinkPoint12.Left = uilinkPoint12.ID - 1;
					break;
				}
				cp17.LinkMap.Add(num24, uilinkPoint12);
			}
			cp17.LinkMap[1550].Down = 1551;
			cp17.LinkMap[1550].Right = 120;
			cp17.LinkMap[1550].Up = 307;
			cp17.LinkMap[1552].Right = 121;
			cp17.LinkMap[1554].Right = 121;
			cp17.LinkMap[1555].Down = 1570;
			cp17.LinkMap[1556].Down = 1570;
			cp17.LinkMap[1556].Right = 122;
			cp17.LinkMap[1557].Up = 1570;
			cp17.LinkMap[1557].Down = 308;
			cp17.LinkMap[1557].Right = 127;
			cp17.LinkMap.Add(1570, new UILinkPoint(1570, true, -3, -4, -1, -2));
			cp17.LinkMap[1570].Up = 1555;
			cp17.LinkMap[1570].Down = 1557;
			cp17.LinkMap[1570].Right = 126;
			for (int num25 = 0; num25 < 7; num25++)
			{
				cp17.LinkMap[1550 + num25].OnSpecialInteracts += value;
			}
			cp17.UpdateEvent += delegate()
			{
				cp17.LinkMap[1551].Up = (Main.ShouldPVPDraw ? 1550 : -1);
				cp17.LinkMap[1552].Up = (Main.ShouldPVPDraw ? 1550 : -1);
				cp17.LinkMap[1570].Up = (Main.ShouldTeamSelectDraw ? 1555 : -1);
				int infoacccount = UILinkPointNavigator.Shortcuts.INFOACCCOUNT;
				if (infoacccount > 0)
				{
					cp17.LinkMap[1570].Up = 1558 + (infoacccount - 1) / 2 * 2;
				}
				if (Main.ShouldTeamSelectDraw)
				{
					if (infoacccount >= 1)
					{
						cp17.LinkMap[1555].Down = 1558;
						cp17.LinkMap[1556].Down = 1558;
					}
					else
					{
						cp17.LinkMap[1555].Down = 1570;
						cp17.LinkMap[1556].Down = 1570;
					}
					if (infoacccount >= 2)
					{
						cp17.LinkMap[1556].Down = 1559;
						return;
					}
					cp17.LinkMap[1556].Down = 1570;
				}
			};
			cp17.IsValidEvent += (() => Main.playerInventory);
			cp17.PageOnLeft = 8;
			cp17.PageOnRight = 8;
			UILinkPointNavigator.RegisterPage(cp17, 16, true);
			UILinkPage cp18 = new UILinkPage();
			cp18.OnSpecialInteracts += (() => PlayerInput.BuildCommand(Lang.misc[56].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]
			}) + PlayerInput.BuildCommand(Lang.misc[64].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"],
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]
			}));
			for (int num26 = 1558; num26 < 1570; num26++)
			{
				UILinkPoint uilinkPoint13 = new UILinkPoint(num26, true, -3, -4, -1, -2);
				uilinkPoint13.OnSpecialInteracts += value;
				switch (num26 - 1558)
				{
				case 1:
				case 3:
				case 5:
					uilinkPoint13.Up = uilinkPoint13.ID - 2;
					uilinkPoint13.Down = uilinkPoint13.ID + 2;
					uilinkPoint13.Right = uilinkPoint13.ID + 1;
					break;
				case 2:
				case 4:
				case 6:
					uilinkPoint13.Up = uilinkPoint13.ID - 2;
					uilinkPoint13.Down = uilinkPoint13.ID + 2;
					uilinkPoint13.Left = uilinkPoint13.ID - 1;
					break;
				}
				cp18.LinkMap.Add(num26, uilinkPoint13);
			}
			cp18.UpdateEvent += delegate()
			{
				int infoacccount = UILinkPointNavigator.Shortcuts.INFOACCCOUNT;
				if (UILinkPointNavigator.OverridePoint == -1 && cp18.CurrentPoint - 1558 >= infoacccount)
				{
					UILinkPointNavigator.ChangePoint(1558 + infoacccount - 1);
				}
				for (int num33 = 0; num33 < infoacccount; num33++)
				{
					bool flag = num33 % 2 == 0;
					int num34 = num33 + 1558;
					cp18.LinkMap[num34].Down = ((num33 < infoacccount - 2) ? (num34 + 2) : 1570);
					cp18.LinkMap[num34].Up = ((num33 > 1) ? (num34 - 2) : (Main.ShouldTeamSelectDraw ? (flag ? 1555 : 1556) : -1));
					cp18.LinkMap[num34].Right = ((flag && num33 + 1 < infoacccount) ? (num34 + 1) : (123 + num33 / 4));
					cp18.LinkMap[num34].Left = (flag ? -3 : (num34 - 1));
				}
			};
			cp18.IsValidEvent += (() => Main.playerInventory && UILinkPointNavigator.Shortcuts.INFOACCCOUNT > 0);
			cp18.PageOnLeft = 8;
			cp18.PageOnRight = 8;
			UILinkPointNavigator.RegisterPage(cp18, 17, true);
			UILinkPage cp19 = new UILinkPage();
			cp19.OnSpecialInteracts += (() => PlayerInput.BuildCommand(Lang.misc[56].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]
			}) + PlayerInput.BuildCommand(Lang.misc[64].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"],
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]
			}));
			int num27 = 6000;
			while (num27 < 6012)
			{
				UILinkPoint uilinkPoint14 = new UILinkPoint(num27, true, -3, -4, -1, -2);
				switch (num27)
				{
				case 6000:
					uilinkPoint14.Right = 0;
					break;
				case 6001:
				case 6002:
					uilinkPoint14.Right = 10;
					break;
				case 6003:
				case 6004:
					uilinkPoint14.Right = 20;
					break;
				case 6005:
				case 6006:
					uilinkPoint14.Right = 30;
					break;
				case 6007:
				case 6008:
				case 6009:
					goto IL_2E17;
				default:
					goto IL_2E17;
				}
				IL_2E20:
				cp19.LinkMap.Add(num27, uilinkPoint14);
				num27++;
				continue;
				IL_2E17:
				uilinkPoint14.Right = 40;
				goto IL_2E20;
			}
			cp19.UpdateEvent += delegate()
			{
				int builderacccount = UILinkPointNavigator.Shortcuts.BUILDERACCCOUNT;
				if (UILinkPointNavigator.OverridePoint == -1 && cp19.CurrentPoint - 6000 >= builderacccount)
				{
					UILinkPointNavigator.ChangePoint(6000 + builderacccount - 1);
				}
				for (int num33 = 0; num33 < builderacccount; num33++)
				{
					int num34 = num33 % 2;
					int num35 = num33 + 6000;
					cp19.LinkMap[num35].Down = ((num33 < builderacccount - 1) ? (num35 + 1) : -2);
					cp19.LinkMap[num35].Up = ((num33 > 0) ? (num35 - 1) : -1);
				}
			};
			cp19.IsValidEvent += (() => Main.playerInventory && UILinkPointNavigator.Shortcuts.BUILDERACCCOUNT > 0);
			cp19.PageOnLeft = 8;
			cp19.PageOnRight = 8;
			UILinkPointNavigator.RegisterPage(cp19, 18, true);
			UILinkPage uilinkPage6 = new UILinkPage();
			uilinkPage6.OnSpecialInteracts += (() => PlayerInput.BuildCommand(Lang.misc[56].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]
			}) + PlayerInput.BuildCommand(Lang.misc[64].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"],
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]
			}));
			uilinkPage6.LinkMap.Add(2806, new UILinkPoint(2806, true, 2805, 2807, -1, 2808));
			uilinkPage6.LinkMap.Add(2807, new UILinkPoint(2807, true, 2806, 2810, -1, 2809));
			uilinkPage6.LinkMap.Add(2808, new UILinkPoint(2808, true, 2813, 2809, 2806, -2));
			uilinkPage6.LinkMap.Add(2809, new UILinkPoint(2809, true, 2808, 2811, 2807, -2));
			uilinkPage6.LinkMap.Add(2810, new UILinkPoint(2810, true, 2807, -4, -1, 2811));
			uilinkPage6.LinkMap.Add(2811, new UILinkPoint(2811, true, 2809, -4, 2810, -2));
			uilinkPage6.LinkMap.Add(2805, new UILinkPoint(2805, true, -3, 2806, -1, 2813));
			uilinkPage6.LinkMap.Add(2813, new UILinkPoint(2813, true, -3, 2808, 2805, -2));
			uilinkPage6.LinkMap[2806].OnSpecialInteracts += value;
			uilinkPage6.LinkMap[2807].OnSpecialInteracts += value;
			uilinkPage6.LinkMap[2808].OnSpecialInteracts += value;
			uilinkPage6.LinkMap[2809].OnSpecialInteracts += value;
			uilinkPage6.LinkMap[2805].OnSpecialInteracts += value;
			uilinkPage6.LinkMap[2813].OnSpecialInteracts += value;
			uilinkPage6.CanEnterEvent += (() => Main.clothesWindow);
			uilinkPage6.IsValidEvent += (() => Main.clothesWindow);
			uilinkPage6.EnterEvent += delegate()
			{
				Main.player[Main.myPlayer].releaseInventory = false;
			};
			uilinkPage6.LeaveEvent += delegate()
			{
				Main.player[Main.myPlayer].LockGamepadTileInteractions();
			};
			uilinkPage6.PageOnLeft = 15;
			uilinkPage6.PageOnRight = 15;
			UILinkPointNavigator.RegisterPage(uilinkPage6, 14, true);
			UILinkPage uilinkPage7 = new UILinkPage();
			uilinkPage7.OnSpecialInteracts += (() => PlayerInput.BuildCommand(Lang.misc[56].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]
			}) + PlayerInput.BuildCommand(Lang.misc[64].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"],
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]
			}));
			uilinkPage7.LinkMap.Add(2800, new UILinkPoint(2800, true, -3, -4, -1, 2801));
			uilinkPage7.LinkMap.Add(2801, new UILinkPoint(2801, true, -3, -4, 2800, 2802));
			uilinkPage7.LinkMap.Add(2802, new UILinkPoint(2802, true, -3, -4, 2801, 2812));
			uilinkPage7.LinkMap.Add(2812, new UILinkPoint(2812, true, -3, -4, 2802, 2803));
			uilinkPage7.LinkMap.Add(2803, new UILinkPoint(2803, true, -3, 2804, 2812, -2));
			uilinkPage7.LinkMap.Add(2804, new UILinkPoint(2804, true, 2803, -4, 2812, -2));
			uilinkPage7.LinkMap[2800].OnSpecialInteracts += value;
			uilinkPage7.LinkMap[2801].OnSpecialInteracts += value;
			uilinkPage7.LinkMap[2802].OnSpecialInteracts += value;
			uilinkPage7.LinkMap[2812].OnSpecialInteracts += value;
			uilinkPage7.LinkMap[2803].OnSpecialInteracts += value;
			uilinkPage7.LinkMap[2804].OnSpecialInteracts += value;
			uilinkPage7.UpdateEvent += delegate()
			{
				Vector3 value24 = Main.rgbToHsl(Main.selColor);
				float interfaceDeadzoneX = PlayerInput.CurrentProfile.InterfaceDeadzoneX;
				float num33 = PlayerInput.GamepadThumbstickLeft.X;
				if (num33 < -interfaceDeadzoneX || num33 > interfaceDeadzoneX)
				{
					num33 = MathHelper.Lerp(0f, 0.008333334f, (Math.Abs(num33) - interfaceDeadzoneX) / (1f - interfaceDeadzoneX)) * (float)Math.Sign(num33);
				}
				else
				{
					num33 = 0f;
				}
				int currentPoint = UILinkPointNavigator.CurrentPoint;
				if (currentPoint == 2800)
				{
					Main.hBar = MathHelper.Clamp(Main.hBar + num33, 0f, 1f);
				}
				if (currentPoint == 2801)
				{
					Main.sBar = MathHelper.Clamp(Main.sBar + num33, 0f, 1f);
				}
				if (currentPoint == 2802)
				{
					Main.lBar = MathHelper.Clamp(Main.lBar + num33, 0.15f, 1f);
				}
				if (currentPoint == 2812)
				{
					Main.player[Main.myPlayer].voicePitchOffset = MathHelper.Clamp(Main.player[Main.myPlayer].voicePitchOffset + num33, -1f, 1f);
				}
				Vector3.Clamp(value24, Vector3.Zero, Vector3.One);
				if (num33 != 0f)
				{
					if (Main.clothesWindow)
					{
						Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar, byte.MaxValue);
						switch (Main.selClothes)
						{
						case 0:
							Main.player[Main.myPlayer].shirtColor = Main.selColor;
							break;
						case 1:
							Main.player[Main.myPlayer].underShirtColor = Main.selColor;
							break;
						case 2:
							Main.player[Main.myPlayer].pantsColor = Main.selColor;
							break;
						case 3:
							Main.player[Main.myPlayer].shoeColor = Main.selColor;
							break;
						}
					}
					if (currentPoint != 2812)
					{
						SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
					}
				}
				if (currentPoint == 2812)
				{
					bool flag = num33 != 0f;
					if (Main.WasDraggingPlayerAudio && !flag)
					{
						Main.player[Main.myPlayer].PlayHurtSound();
					}
					Main.WasDraggingPlayerAudio = flag;
				}
			};
			uilinkPage7.CanEnterEvent += (() => Main.clothesWindow);
			uilinkPage7.IsValidEvent += (() => Main.clothesWindow);
			uilinkPage7.EnterEvent += delegate()
			{
				Main.player[Main.myPlayer].releaseInventory = false;
				Main.WasDraggingPlayerAudio = false;
			};
			uilinkPage7.LeaveEvent += delegate()
			{
				Main.player[Main.myPlayer].LockGamepadTileInteractions();
			};
			uilinkPage7.PageOnLeft = 14;
			uilinkPage7.PageOnRight = 14;
			UILinkPointNavigator.RegisterPage(uilinkPage7, 15, true);
			UILinkPage cp20 = new UILinkPage();
			cp20.UpdateEvent += delegate()
			{
				PlayerInput.GamepadAllowScrolling = true;
			};
			for (int num28 = 3000; num28 <= 4999; num28++)
			{
				cp20.LinkMap.Add(num28, new UILinkPoint(num28, true, -3, -4, -1, -2));
			}
			cp20.OnSpecialInteracts += delegate()
			{
				if (Main.InGameUI.CurrentState is UIBestiaryTest)
				{
					return PlayerInput.BuildCommand(Lang.misc[82].Value, new List<string>[]
					{
						PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]
					}) + PlayerInput.BuildCommand(Language.GetText("UI.SwitchPage").Value, new List<string>[]
					{
						PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"],
						PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]
					}) + PlayerInput.BuildCommand(Lang.misc[53].Value, new List<string>[]
					{
						PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]
					}) + UILinksInitializer.FancyUISpecialInstructions();
				}
				return PlayerInput.BuildCommand(Lang.misc[53].Value, new List<string>[]
				{
					PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]
				}) + PlayerInput.BuildCommand(Lang.misc[82].Value, new List<string>[]
				{
					PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]
				}) + UILinksInitializer.FancyUISpecialInstructions();
			};
			cp20.UpdateEvent += delegate()
			{
				if (UILinksInitializer.CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.Inventory)
				{
					UILinksInitializer.FancyExit();
				}
			};
			cp20.EnterEvent += delegate()
			{
				cp20.CurrentPoint = 3002;
			};
			cp20.CanEnterEvent += (() => Main.MenuUI.IsVisible || Main.InGameUI.IsVisible);
			cp20.IsValidEvent += (() => Main.MenuUI.IsVisible || Main.InGameUI.IsVisible);
			cp20.OnPageMoveAttempt += UILinksInitializer.OnFancyUIPageMoveAttempt;
			UILinkPointNavigator.RegisterPage(cp20, 1004, true);
			UILinkPage cp21 = new UILinkPage();
			cp21.UpdateEvent += delegate()
			{
				PlayerInput.GamepadAllowScrolling = true;
			};
			for (int num29 = 10000; num29 <= 11000; num29++)
			{
				cp21.LinkMap.Add(num29, new UILinkPoint(num29, true, -3, -4, -1, -2));
			}
			for (int num30 = 15000; num30 <= 15000; num30++)
			{
				cp21.LinkMap.Add(num30, new UILinkPoint(num30, true, -3, -4, -1, -2));
			}
			cp21.OnSpecialInteracts += (() => PlayerInput.BuildCommand(Lang.misc[56].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]
			}) + PlayerInput.BuildCommand(Lang.misc[64].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"],
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]
			}) + PlayerInput.BuildCommand(Lang.misc[53].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]
			}) + UILinksInitializer.FancyUISpecialInstructions());
			cp21.UpdateEvent += delegate()
			{
				if (UILinksInitializer.CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.Inventory)
				{
					UILinksInitializer.FancyExit();
				}
			};
			cp21.EnterEvent += delegate()
			{
				cp21.CurrentPoint = 10000;
			};
			cp21.CanEnterEvent += UILinksInitializer.CanEnterCreativeMenu;
			cp21.IsValidEvent += UILinksInitializer.CanEnterCreativeMenu;
			cp21.OnPageMoveAttempt += UILinksInitializer.OnFancyUIPageMoveAttempt;
			cp21.PageOnLeft = 8;
			cp21.PageOnRight = 0;
			UILinkPointNavigator.RegisterPage(cp21, 1005, true);
			UILinkPage uilinkPage8 = new UILinkPage();
			for (int num31 = 20000; num31 < 21000; num31++)
			{
				uilinkPage8.LinkMap.Add(num31, new UILinkPoint(num31, true, -3, -4, -1, -2));
			}
			uilinkPage8.CanEnterEvent += (() => NewCraftingUI.Visible);
			uilinkPage8.IsValidEvent += (() => NewCraftingUI.Visible);
			uilinkPage8.OnPageMoveAttempt += UILinksInitializer.OnFancyUIPageMoveAttempt;
			uilinkPage8.PageOnLeft = 8;
			uilinkPage8.PageOnRight = 0;
			UILinkPointNavigator.RegisterPage(uilinkPage8, 24, true);
			UILinkPage cp22 = new UILinkPage();
			cp22.OnSpecialInteracts += (() => PlayerInput.BuildCommand(Lang.misc[56].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]
			}) + PlayerInput.BuildCommand(Lang.misc[64].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"],
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]
			}));
			Func<string> value21 = () => PlayerInput.BuildCommand(Lang.misc[94].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]
			});
			for (int num32 = 9000; num32 <= 9050; num32++)
			{
				UILinkPoint uilinkPoint15 = new UILinkPoint(num32, true, num32 + 10, num32 - 10, num32 - 1, num32 + 1);
				cp22.LinkMap.Add(num32, uilinkPoint15);
				uilinkPoint15.OnSpecialInteracts += value21;
			}
			cp22.UpdateEvent += delegate()
			{
				int num33 = UILinkPointNavigator.Shortcuts.BUFFS_PER_COLUMN;
				if (num33 == 0)
				{
					num33 = 100;
				}
				for (int num34 = 0; num34 < 50; num34++)
				{
					cp22.LinkMap[9000 + num34].Up = ((num34 % num33 == 0) ? -1 : (9000 + num34 - 1));
					if (cp22.LinkMap[9000 + num34].Up == -1)
					{
						if (num34 >= num33)
						{
							cp22.LinkMap[9000 + num34].Up = 184;
						}
						else
						{
							cp22.LinkMap[9000 + num34].Up = 189;
						}
					}
					cp22.LinkMap[9000 + num34].Down = (((num34 + 1) % num33 == 0 || num34 == UILinkPointNavigator.Shortcuts.BUFFS_DRAWN - 1) ? 308 : (9000 + num34 + 1));
					cp22.LinkMap[9000 + num34].Left = ((num34 < UILinkPointNavigator.Shortcuts.BUFFS_DRAWN - num33) ? (9000 + num34 + num33) : -3);
					cp22.LinkMap[9000 + num34].Right = ((num34 < num33) ? -4 : (9000 + num34 - num33));
				}
			};
			cp22.IsValidEvent += (() => Main.playerInventory && Main.EquipPage == 2 && UILinkPointNavigator.Shortcuts.BUFFS_DRAWN > 0);
			cp22.PageOnLeft = 8;
			cp22.PageOnRight = 8;
			UILinkPointNavigator.RegisterPage(cp22, 19, true);
			UILinkPage uilinkPage9 = new UILinkPage();
			uilinkPage9.OnSpecialInteracts += (() => PlayerInput.BuildCommand(Lang.misc[56].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]
			}) + PlayerInput.BuildCommand(Lang.misc[64].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"],
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]
			}));
			uilinkPage9.OnSpecialInteractsLate += (() => ItemSlot.GetGamepadInstructions(35));
			UILinkPoint value22 = new UILinkPoint(12000, true, -3, 11001, -1, -2);
			uilinkPage9.LinkMap.Add(12000, value22);
			uilinkPage9.LinkMap[12000].OnSpecialInteracts += delegate()
			{
				string text = "";
				if (Main.mouseItem.stack <= 0 || (Main.mouseItem.type == Main.bannerUI.FocusedItemType && Main.mouseItem.stack < Main.mouseItem.maxStack))
				{
					text += PlayerInput.BuildCommand(Language.GetTextValue("UI.GamepadClaimBanner"), new List<string>[]
					{
						PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"],
						PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"]
					});
				}
				return text;
			};
			uilinkPage9.ReachEndEvent += delegate(int current, int next)
			{
				bool value24 = next == -1;
				int yOffset = (next == -2).ToInt() - value24.ToInt();
				Main.bannerUI.NavigatePipsList(yOffset);
			};
			uilinkPage9.EnterEvent += delegate()
			{
				Main.PipsUseGrid = false;
				PlayerInput.LockGamepadButtons("MouseLeft");
			};
			uilinkPage9.CanEnterEvent += (() => Main.playerInventory && Main.bannerUI.AnyAvailableBanners);
			uilinkPage9.IsValidEvent += (() => Main.playerInventory && Main.bannerUI.AnyAvailableBanners);
			uilinkPage9.PageOnLeft = 23;
			uilinkPage9.PageOnRight = 0;
			UILinkPointNavigator.RegisterPage(uilinkPage9, 22, true);
			UILinkPage cp = new UILinkPage();
			cp.OnSpecialInteracts += (() => PlayerInput.BuildCommand(Lang.misc[56].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]
			}) + PlayerInput.BuildCommand(Lang.misc[64].Value, new List<string>[]
			{
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"],
				PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]
			}));
			cp.OnSpecialInteractsLate += (() => ItemSlot.GetGamepadInstructions(35));
			UILinkPoint value23 = new UILinkPoint(11100, true, -3, -4, -1, -2);
			cp.LinkMap.Add(11100, value23);
			cp.UpdateEvent += delegate()
			{
				int craft_IconsPerRow = UILinkPointNavigator.Shortcuts.CRAFT_IconsPerRow;
				int craft_IconsPerColumn = UILinkPointNavigator.Shortcuts.CRAFT_IconsPerColumn;
				cp.PageOnLeft = UILinksInitializer.GetCornerWrapPageIdFromLeftToRight();
			};
			cp.ReachEndEvent += delegate(int current, int next)
			{
				bool value24 = next == -3;
				int xOffset = (next == -4).ToInt() - value24.ToInt();
				bool value25 = next == -1;
				int yOffset = (next == -2).ToInt() - value25.ToInt();
				Main.bannerUI.NavigatePipsGrid(xOffset, yOffset);
			};
			cp.EnterEvent += delegate()
			{
				Main.PipsUseGrid = true;
				Main.bannerUI.ResetGridSelection();
			};
			cp.LeaveEvent += delegate()
			{
				Main.PipsUseGrid = false;
			};
			cp.CanEnterEvent += (() => Main.playerInventory && Main.bannerUI.AnyAvailableBanners);
			cp.IsValidEvent += (() => Main.playerInventory && Main.PipsUseGrid && Main.bannerUI.AnyAvailableBanners);
			cp.PageOnLeft = 0;
			cp.PageOnRight = 22;
			UILinkPointNavigator.RegisterPage(cp, 23, true);
			UILinkPage uilinkPage10 = UILinkPointNavigator.Pages[UILinkPointNavigator.CurrentPage];
			uilinkPage10.CurrentPoint = uilinkPage10.DefaultPoint;
			uilinkPage10.Enter();
		}

		// Token: 0x060015A8 RID: 5544 RVA: 0x004CFF2C File Offset: 0x004CE12C
		private static bool TryQuickCrafting(int startPoint, int pointOffset)
		{
			Player player = Main.player[Main.myPlayer];
			int num = Main.recStart + pointOffset;
			if (num >= UILinksInitializer.MainnumAvailableRecipes)
			{
				return false;
			}
			bool result = false;
			int num2 = num - startPoint;
			Recipe recipe = Main.recipe[Main.availableRecipe[num2]];
			if (Main.mouseItem.type == 0 && recipe.createItem.maxStack > 1 && player.ItemSpace(recipe.createItem).CanTakeItemToPersonalInventory && !player.HasLockedInventory())
			{
				result = true;
				if (UILinksInitializer.CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.Grapple)
				{
					UILinksInitializer.SomeVarsForUILinkers.SequencedCraftingCurrent = recipe;
				}
				if (UILinksInitializer.CanExecuteInputCommand() && PlayerInput.Triggers.Current.Grapple && Main.stackSplit <= 1)
				{
					ItemSlot.RefreshStackSplitCooldown();
					if (UILinksInitializer.SomeVarsForUILinkers.SequencedCraftingCurrent == recipe)
					{
						CraftingRequests.CraftItem(recipe, 1, true);
					}
				}
			}
			return result;
		}

		// Token: 0x060015A9 RID: 5545 RVA: 0x004D0003 File Offset: 0x004CE203
		private static bool CanEnterCreativeMenu()
		{
			return Main.LocalPlayer.chest == -1 && Main.LocalPlayer.talkNPC == -1 && Main.playerInventory && Main.CreativeMenu.Enabled;
		}

		// Token: 0x060015AA RID: 5546 RVA: 0x004D0036 File Offset: 0x004CE236
		private static int GetCornerWrapPageIdFromLeftToRight()
		{
			return 8;
		}

		// Token: 0x060015AB RID: 5547 RVA: 0x004D003C File Offset: 0x004CE23C
		private static int GetCornerWrapPageIdFromRightToLeft()
		{
			if (Main.CreativeMenu.Enabled)
			{
				return 1005;
			}
			if (NewCraftingUI.Visible)
			{
				return 24;
			}
			if (Main.InPipBanner)
			{
				return 23;
			}
			TileEntity tileEntity = Main.LocalPlayer.tileEntityAnchor.GetTileEntity();
			if (tileEntity is TEDisplayDoll)
			{
				return 20;
			}
			if (tileEntity is TEHatRack)
			{
				return 21;
			}
			return 9;
		}

		// Token: 0x060015AC RID: 5548 RVA: 0x004D00A0 File Offset: 0x004CE2A0
		private static void OnFancyUIPageMoveAttempt(int direction)
		{
			UICharacterCreation uicharacterCreation = Main.MenuUI.CurrentState as UICharacterCreation;
			if (uicharacterCreation != null)
			{
				uicharacterCreation.TryMovingCategory(direction);
			}
			UIBestiaryTest uibestiaryTest = UserInterface.ActiveInstance.CurrentState as UIBestiaryTest;
			if (uibestiaryTest != null)
			{
				uibestiaryTest.TryMovingPages(direction);
			}
		}

		// Token: 0x060015AD RID: 5549 RVA: 0x004D00E4 File Offset: 0x004CE2E4
		public static void FancyExit()
		{
			switch (UILinkPointNavigator.Shortcuts.BackButtonCommand)
			{
			case 1:
				SoundEngine.PlaySound(11, -1, -1, 1, 1f, 0f);
				Main.menuMode = 0;
				return;
			case 2:
				SoundEngine.PlaySound(11, -1, -1, 1, 1f, 0f);
				Main.menuMode = (Main.menuMultiplayer ? 12 : 1);
				return;
			case 3:
				Main.menuMode = 0;
				IngameFancyUI.Close(false);
				return;
			case 4:
				SoundEngine.PlaySound(11, -1, -1, 1, 1f, 0f);
				Main.menuMode = 11;
				return;
			case 5:
				SoundEngine.PlaySound(11, -1, -1, 1, 1f, 0f);
				Main.menuMode = 11;
				return;
			case 6:
				Main.LocalPlayer.releaseInventory = false;
				UIVirtualKeyboard.Cancel();
				return;
			case 7:
			{
				IHaveBackButtonCommand haveBackButtonCommand = Main.MenuUI.CurrentState as IHaveBackButtonCommand;
				if (haveBackButtonCommand != null)
				{
					haveBackButtonCommand.HandleBackButtonUsage();
				}
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x060015AE RID: 5550 RVA: 0x004D01D0 File Offset: 0x004CE3D0
		public static string FancyUISpecialInstructions()
		{
			string text = "";
			int fancyui_SPECIAL_INSTRUCTIONS = UILinkPointNavigator.Shortcuts.FANCYUI_SPECIAL_INSTRUCTIONS;
			if (fancyui_SPECIAL_INSTRUCTIONS == 1)
			{
				if (UILinksInitializer.CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.HotbarMinus)
				{
					UIVirtualKeyboard.CycleSymbols();
					PlayerInput.LockGamepadButtons("HotbarMinus");
					PlayerInput.SettingsForUI.TryRevertingToMouseMode();
				}
				text += PlayerInput.BuildCommand(Lang.menu[235].Value, new List<string>[]
				{
					PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"]
				});
				if (UILinksInitializer.CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.MouseRight)
				{
					UIVirtualKeyboard.BackSpace();
					PlayerInput.LockGamepadButtons("MouseRight");
					PlayerInput.SettingsForUI.TryRevertingToMouseMode();
				}
				text += PlayerInput.BuildCommand(Lang.menu[236].Value, new List<string>[]
				{
					PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"]
				});
				if (UILinksInitializer.CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.SmartCursor)
				{
					UIVirtualKeyboard.Write(" ");
					PlayerInput.LockGamepadButtons("SmartCursor");
					PlayerInput.SettingsForUI.TryRevertingToMouseMode();
				}
				text += PlayerInput.BuildCommand(Lang.menu[238].Value, new List<string>[]
				{
					PlayerInput.ProfileGamepadUI.KeyStatus["SmartCursor"]
				});
				if (UIVirtualKeyboard.CanSubmit)
				{
					if (UILinksInitializer.CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.HotbarPlus)
					{
						UIVirtualKeyboard.Submit();
						PlayerInput.LockGamepadButtons("HotbarPlus");
						PlayerInput.SettingsForUI.TryRevertingToMouseMode();
					}
					text += PlayerInput.BuildCommand(Lang.menu[237].Value, new List<string>[]
					{
						PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]
					});
				}
			}
			return text;
		}

		// Token: 0x060015AF RID: 5551 RVA: 0x004D0394 File Offset: 0x004CE594
		public static void HandleOptionsSpecials()
		{
			switch (UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE)
			{
			case 1:
				Main.bgScroll = (int)UILinksInitializer.HandleSliderHorizontalInput((float)Main.bgScroll, 0f, 100f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 1f);
				Main.caveParallax = 1f - (float)Main.bgScroll / 500f;
				return;
			case 2:
				Main.musicVolume = UILinksInitializer.HandleSliderHorizontalInput(Main.musicVolume, 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
				return;
			case 3:
				Main.soundVolume = UILinksInitializer.HandleSliderHorizontalInput(Main.soundVolume, 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
				return;
			case 4:
				Main.ambientVolume = UILinksInitializer.HandleSliderHorizontalInput(Main.ambientVolume, 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
				return;
			case 5:
			{
				float hBar = Main.hBar;
				float num = Main.hBar = UILinksInitializer.HandleSliderHorizontalInput(hBar, 0f, 1f, 0.2f, 0.5f);
				if (hBar != num)
				{
					int menuMode = Main.menuMode;
					switch (menuMode)
					{
					case 17:
						Main.player[Main.myPlayer].hairColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar, byte.MaxValue));
						break;
					case 18:
						Main.player[Main.myPlayer].eyeColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar, byte.MaxValue));
						break;
					case 19:
						Main.player[Main.myPlayer].skinColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar, byte.MaxValue));
						break;
					case 20:
						break;
					case 21:
						Main.player[Main.myPlayer].shirtColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar, byte.MaxValue));
						break;
					case 22:
						Main.player[Main.myPlayer].underShirtColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar, byte.MaxValue));
						break;
					case 23:
						Main.player[Main.myPlayer].pantsColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar, byte.MaxValue));
						break;
					case 24:
						Main.player[Main.myPlayer].shoeColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar, byte.MaxValue));
						break;
					case 25:
						Main.mouseColorSlider.Hue = num;
						break;
					default:
						if (menuMode == 252)
						{
							Main.mouseBorderColorSlider.Hue = num;
						}
						break;
					}
					SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
					return;
				}
				break;
			}
			case 6:
			{
				float sBar = Main.sBar;
				float num2 = Main.sBar = UILinksInitializer.HandleSliderHorizontalInput(sBar, 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.5f);
				if (sBar != num2)
				{
					int menuMode = Main.menuMode;
					switch (menuMode)
					{
					case 17:
						Main.player[Main.myPlayer].hairColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar, byte.MaxValue));
						break;
					case 18:
						Main.player[Main.myPlayer].eyeColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar, byte.MaxValue));
						break;
					case 19:
						Main.player[Main.myPlayer].skinColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar, byte.MaxValue));
						break;
					case 20:
						break;
					case 21:
						Main.player[Main.myPlayer].shirtColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar, byte.MaxValue));
						break;
					case 22:
						Main.player[Main.myPlayer].underShirtColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar, byte.MaxValue));
						break;
					case 23:
						Main.player[Main.myPlayer].pantsColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar, byte.MaxValue));
						break;
					case 24:
						Main.player[Main.myPlayer].shoeColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar, byte.MaxValue));
						break;
					case 25:
						Main.mouseColorSlider.Saturation = num2;
						break;
					default:
						if (menuMode == 252)
						{
							Main.mouseBorderColorSlider.Saturation = num2;
						}
						break;
					}
					SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
					return;
				}
				break;
			}
			case 7:
			{
				float lBar = Main.lBar;
				float min = 0.15f;
				if (Main.menuMode == 252)
				{
					min = 0f;
				}
				Main.lBar = UILinksInitializer.HandleSliderHorizontalInput(lBar, min, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.5f);
				float lBar2 = Main.lBar;
				if (lBar != lBar2)
				{
					int menuMode = Main.menuMode;
					switch (menuMode)
					{
					case 17:
						Main.player[Main.myPlayer].hairColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar, byte.MaxValue));
						break;
					case 18:
						Main.player[Main.myPlayer].eyeColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar, byte.MaxValue));
						break;
					case 19:
						Main.player[Main.myPlayer].skinColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar, byte.MaxValue));
						break;
					case 20:
						break;
					case 21:
						Main.player[Main.myPlayer].shirtColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar, byte.MaxValue));
						break;
					case 22:
						Main.player[Main.myPlayer].underShirtColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar, byte.MaxValue));
						break;
					case 23:
						Main.player[Main.myPlayer].pantsColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar, byte.MaxValue));
						break;
					case 24:
						Main.player[Main.myPlayer].shoeColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar, byte.MaxValue));
						break;
					case 25:
						Main.mouseColorSlider.Luminance = lBar2;
						break;
					default:
						if (menuMode == 252)
						{
							Main.mouseBorderColorSlider.Luminance = lBar2;
						}
						break;
					}
					SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
					return;
				}
				break;
			}
			case 8:
			{
				float aBar = Main.aBar;
				float num3 = Main.aBar = UILinksInitializer.HandleSliderHorizontalInput(aBar, 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.5f);
				if (aBar != num3)
				{
					int menuMode = Main.menuMode;
					if (menuMode == 252)
					{
						Main.mouseBorderColorSlider.Alpha = num3;
					}
					SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
					return;
				}
				break;
			}
			case 9:
			{
				bool left = PlayerInput.Triggers.Current.Left;
				bool right = PlayerInput.Triggers.Current.Right;
				if (PlayerInput.Triggers.JustPressed.Left || PlayerInput.Triggers.JustPressed.Right)
				{
					UILinksInitializer.SomeVarsForUILinkers.HairMoveCD = 0;
				}
				else if (UILinksInitializer.SomeVarsForUILinkers.HairMoveCD > 0)
				{
					UILinksInitializer.SomeVarsForUILinkers.HairMoveCD--;
				}
				if (UILinksInitializer.SomeVarsForUILinkers.HairMoveCD == 0 && (left || right))
				{
					if (left)
					{
						Main.PendingPlayer.hair--;
					}
					if (right)
					{
						Main.PendingPlayer.hair++;
					}
					UILinksInitializer.SomeVarsForUILinkers.HairMoveCD = 12;
				}
				int num4 = 51;
				if (Main.PendingPlayer.hair >= num4)
				{
					Main.PendingPlayer.hair = 0;
				}
				if (Main.PendingPlayer.hair < 0)
				{
					Main.PendingPlayer.hair = num4 - 1;
					return;
				}
				break;
			}
			case 10:
				Main.GameZoomTarget = UILinksInitializer.HandleSliderHorizontalInput(Main.GameZoomTarget, 1f, 2f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
				return;
			case 11:
				Main.UIScale = UILinksInitializer.HandleSliderHorizontalInput(Main.UIScaleWanted, 0.5f, 2f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
				Main.temporaryGUIScaleSlider = Main.UIScaleWanted;
				return;
			case 12:
				Main.MapScale = UILinksInitializer.HandleSliderHorizontalInput(Main.MapScale, 0.5f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.7f);
				break;
			default:
				return;
			}
		}

		// Token: 0x040010F8 RID: 4344
		private static List<string> RightStickGlyphBinding = new List<string>
		{
			"RightStickAxis"
		};

		// Token: 0x0200066D RID: 1645
		public class SomeVarsForUILinkers
		{
			// Token: 0x0400667A RID: 26234
			public static Recipe SequencedCraftingCurrent;

			// Token: 0x0400667B RID: 26235
			public static int HairMoveCD;
		}
	}
}
