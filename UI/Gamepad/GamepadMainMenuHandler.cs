using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.GameInput;

namespace Terraria.UI.Gamepad
{
	// Token: 0x02000104 RID: 260
	public class GamepadMainMenuHandler
	{
		// Token: 0x06001A2A RID: 6698 RVA: 0x004F4650 File Offset: 0x004F2850
		public static void Update()
		{
			if (!GamepadMainMenuHandler.CanRun)
			{
				UILinkPage uilinkPage = UILinkPointNavigator.Pages[1000];
				uilinkPage.CurrentPoint = uilinkPage.DefaultPoint;
				Vector2 value = new Vector2((float)Math.Cos((double)(Main.GlobalTimeWrappedHourly * 6.2831855f)), (float)Math.Sin((double)(Main.GlobalTimeWrappedHourly * 6.2831855f * 2f))) * new Vector2(30f, 15f) + Vector2.UnitY * 20f;
				UILinkPointNavigator.SetPosition(2000, new Vector2((float)Main.screenWidth, (float)Main.screenHeight) / 2f + value);
				return;
			}
			if (!Main.gameMenu)
			{
				return;
			}
			if (Main.MenuUI.IsVisible)
			{
				return;
			}
			if (GamepadMainMenuHandler.LastDrew != Main.menuMode)
			{
				return;
			}
			int lastMainMenu = GamepadMainMenuHandler.LastMainMenu;
			GamepadMainMenuHandler.LastMainMenu = Main.menuMode;
			switch (Main.menuMode)
			{
			case 17:
			case 18:
			case 19:
			case 21:
			case 22:
			case 23:
			case 24:
			case 26:
				if (GamepadMainMenuHandler.MenuItemPositions.Count >= 4)
				{
					Vector2 item = GamepadMainMenuHandler.MenuItemPositions[3];
					GamepadMainMenuHandler.MenuItemPositions.RemoveAt(3);
					if (Main.menuMode == 17)
					{
						GamepadMainMenuHandler.MenuItemPositions.Insert(0, item);
					}
				}
				break;
			case 28:
				if (GamepadMainMenuHandler.MenuItemPositions.Count >= 3)
				{
					GamepadMainMenuHandler.MenuItemPositions.RemoveAt(1);
				}
				break;
			}
			UILinkPage uilinkPage2 = UILinkPointNavigator.Pages[1000];
			if (lastMainMenu != Main.menuMode)
			{
				uilinkPage2.CurrentPoint = uilinkPage2.DefaultPoint;
			}
			for (int i = 0; i < GamepadMainMenuHandler.MenuItemPositions.Count; i++)
			{
				Vector2 vector = GamepadMainMenuHandler.MenuItemPositions[i] * Main.UIScale;
				if (i == 0 && lastMainMenu != GamepadMainMenuHandler.LastMainMenu && PlayerInput.UsingGamepad && Main.InvisibleCursorForGamepad)
				{
					Main.mouseX = (PlayerInput.MouseX = (int)vector.X);
					Main.mouseY = (PlayerInput.MouseY = (int)vector.Y);
					Main.menuFocus = -1;
				}
				UILinkPoint uilinkPoint = uilinkPage2.LinkMap[2000 + i];
				uilinkPoint.Position = vector;
				if (i == 0)
				{
					uilinkPoint.Up = -1;
				}
				else
				{
					uilinkPoint.Up = 2000 + i - 1;
				}
				uilinkPoint.Left = -3;
				uilinkPoint.Right = -4;
				if (i == GamepadMainMenuHandler.MenuItemPositions.Count - 1)
				{
					uilinkPoint.Down = -2;
				}
				else
				{
					uilinkPoint.Down = 2000 + i + 1;
				}
				if (GamepadMainMenuHandler.MoveCursorOnNextRun)
				{
					GamepadMainMenuHandler.MoveCursorOnNextRun = false;
					UILinkPointNavigator.ChangePoint(uilinkPoint.ID);
				}
			}
			GamepadMainMenuHandler.MenuItemPositions.Clear();
		}

		// Token: 0x040014AC RID: 5292
		public static int LastMainMenu = -1;

		// Token: 0x040014AD RID: 5293
		public static List<Vector2> MenuItemPositions = new List<Vector2>(20);

		// Token: 0x040014AE RID: 5294
		public static int LastDrew = -1;

		// Token: 0x040014AF RID: 5295
		public static bool CanRun = false;

		// Token: 0x040014B0 RID: 5296
		public static bool MoveCursorOnNextRun = false;
	}
}
