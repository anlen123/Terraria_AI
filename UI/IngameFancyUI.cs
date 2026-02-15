using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Achievements;
using Terraria.Audio;
using Terraria.GameContent.UI.States;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.UI.Gamepad;

namespace Terraria.UI
{
	// Token: 0x020000F8 RID: 248
	public class IngameFancyUI
	{
		// Token: 0x06001941 RID: 6465 RVA: 0x004E711A File Offset: 0x004E531A
		public static void CoverNextFrame()
		{
			IngameFancyUI.CoverForOneUIFrame = true;
		}

		// Token: 0x06001942 RID: 6466 RVA: 0x004E7122 File Offset: 0x004E5322
		public static bool CanCover()
		{
			if (IngameFancyUI.CoverForOneUIFrame)
			{
				IngameFancyUI.CoverForOneUIFrame = false;
				return true;
			}
			return Main.inFancyUI;
		}

		// Token: 0x06001943 RID: 6467 RVA: 0x004E713D File Offset: 0x004E533D
		public static void OpenAchievements()
		{
			if (Main.gameMenu)
			{
				Main.MenuUI.SetState(Main.AchievementsMenu);
				return;
			}
			IngameFancyUI.OpenUIState(Main.AchievementsMenu);
		}

		// Token: 0x06001944 RID: 6468 RVA: 0x004E7160 File Offset: 0x004E5360
		public static void OpenAchievementsAndGoto(Achievement achievement)
		{
			IngameFancyUI.OpenAchievements();
			Main.AchievementsMenu.GotoAchievement(achievement);
		}

		// Token: 0x06001945 RID: 6469 RVA: 0x004E7172 File Offset: 0x004E5372
		private static void ClearChat()
		{
			Main.ClosePlayerChat();
			Main.chatText = "";
		}

		// Token: 0x06001946 RID: 6470 RVA: 0x004E7183 File Offset: 0x004E5383
		public static void OpenKeybinds()
		{
			IngameFancyUI.OpenUIState(Main.ManageControlsMenu);
		}

		// Token: 0x06001947 RID: 6471 RVA: 0x004E718F File Offset: 0x004E538F
		public static void OpenUIState(UIState uiState)
		{
			IngameFancyUI.OpenUIState(uiState, true);
		}

		// Token: 0x06001948 RID: 6472 RVA: 0x004E7198 File Offset: 0x004E5398
		public static void OpenUIState(UIState uiState, bool closeIngameWindows = true)
		{
			IngameFancyUI.CoverNextFrame();
			IngameFancyUI.ClearChat();
			if (!Main.inFancyUI && closeIngameWindows)
			{
				IngameUIWindows.CloseAll(true);
			}
			Main.inFancyUI = true;
			Main.InGameUI.SetState(uiState);
		}

		// Token: 0x06001949 RID: 6473 RVA: 0x004E71C7 File Offset: 0x004E53C7
		public static bool CanShowVirtualKeyboard(int context)
		{
			return UIVirtualKeyboard.CanDisplay(context);
		}

		// Token: 0x0600194A RID: 6474 RVA: 0x004E71D0 File Offset: 0x004E53D0
		public static void OpenVirtualKeyboard(int keyboardContext)
		{
			IngameFancyUI.CoverNextFrame();
			IngameFancyUI.ClearChat();
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			string labelText = "";
			if (keyboardContext != 1)
			{
				if (keyboardContext == 2)
				{
					labelText = Language.GetTextValue("UI.EnterNewName");
					Player player = Main.player[Main.myPlayer];
					Main.npcChatText = Main.chest[player.chest].name;
					Tile tile = Main.tile[player.chestX, player.chestY];
					if (tile.type == 21)
					{
						Main.defaultChestName = Lang.chestType[(int)(tile.frameX / 36)].Value;
					}
					else if (tile.type == 467 && tile.frameX / 36 == 4)
					{
						Main.defaultChestName = Lang.GetItemNameValue(3988);
					}
					else if (tile.type == 467)
					{
						Main.defaultChestName = Lang.chestType2[(int)(tile.frameX / 36)].Value;
					}
					else if (tile.type == 88)
					{
						Main.defaultChestName = Lang.dresserType[(int)(tile.frameX / 54)].Value;
					}
					if (Main.npcChatText == "")
					{
						Main.npcChatText = Main.defaultChestName;
					}
					Main.editChest = true;
				}
			}
			else
			{
				Main.editSign = true;
				labelText = Language.GetTextValue("UI.EnterMessage");
			}
			Main.clrInput();
			if (!IngameFancyUI.CanShowVirtualKeyboard(keyboardContext))
			{
				return;
			}
			Main.inFancyUI = true;
			if (keyboardContext != 1)
			{
				if (keyboardContext == 2)
				{
					Main.InGameUI.SetState(new UIVirtualKeyboard(labelText, Main.npcChatText, delegate(string s)
					{
						ChestUI.RenameChestSubmit(Main.player[Main.myPlayer]);
						IngameFancyUI.Close(true);
					}, delegate()
					{
						ChestUI.RenameChestCancel();
						IngameFancyUI.Close(true);
					}, keyboardContext, false, 20));
				}
			}
			else
			{
				Main.InGameUI.SetState(new UIVirtualKeyboard(labelText, Main.npcChatText, delegate(string s)
				{
					Main.SubmitSignText();
					IngameFancyUI.Close(true);
				}, delegate()
				{
					Main.InputTextSignCancel();
					IngameFancyUI.Close(true);
				}, keyboardContext, false, 1200));
			}
			UILinkPointNavigator.GoToDefaultPage(1);
		}

		// Token: 0x0600194B RID: 6475 RVA: 0x004E7400 File Offset: 0x004E5600
		public static void Close(bool quiet = false)
		{
			Main.inFancyUI = false;
			if (!quiet)
			{
				SoundEngine.PlaySound(11, -1, -1, 1, 1f, 0f);
			}
			bool flag = false;
			if (!Main.gameMenu)
			{
				if (Main.InGameUI.CurrentState is UIVirtualKeyboard)
				{
					flag = (UIVirtualKeyboard.KeyboardContext == 2);
				}
				else if (!(Main.InGameUI.CurrentState is UIEmotesMenu))
				{
					flag = true;
				}
			}
			if (flag)
			{
				Main.playerInventory = true;
			}
			Main.LocalPlayer.releaseInventory = false;
			Main.InGameUI.SetState(null);
			UILinkPointNavigator.Shortcuts.FANCYUI_SPECIAL_INSTRUCTIONS = 0;
		}

		// Token: 0x0600194C RID: 6476 RVA: 0x004E7488 File Offset: 0x004E5688
		public static bool Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
			bool result = false;
			if (Main.InGameUI.CurrentState is UIVirtualKeyboard && UIVirtualKeyboard.KeyboardContext > 0)
			{
				if (!Main.inFancyUI)
				{
					Main.InGameUI.SetState(null);
				}
				if (Main.screenWidth >= 1705 || !PlayerInput.UsingGamepad)
				{
					result = true;
				}
			}
			if (!Main.gameMenu)
			{
				Main.mouseText = false;
				if (Main.InGameUI != null && Main.InGameUI.IsElementUnderMouse())
				{
					Main.player[Main.myPlayer].mouseInterface = true;
				}
				Main.instance.GUIBarsDraw();
				if (Main.InGameUI.CurrentState is UIVirtualKeyboard && UIVirtualKeyboard.KeyboardContext > 0)
				{
					Main.instance.GUIChatDraw();
				}
				if (!Main.inFancyUI)
				{
					Main.InGameUI.SetState(null);
				}
				Main.instance.DrawMouseOver();
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.SamplerStateForCursor, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);
				Main.DrawCursor(Main.DrawThickCursor(false), false);
			}
			return result;
		}

		// Token: 0x0600194D RID: 6477 RVA: 0x004E7590 File Offset: 0x004E5790
		public static void MouseOver()
		{
			if (!Main.inFancyUI)
			{
				return;
			}
			if (Main.InGameUI.IsElementUnderMouse())
			{
				Main.mouseText = true;
			}
		}

		// Token: 0x0400132E RID: 4910
		private static bool CoverForOneUIFrame;
	}
}
