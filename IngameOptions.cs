using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.BigProgressBar;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.Social;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria
{
	// Token: 0x02000028 RID: 40
	public static class IngameOptions
	{
		// Token: 0x060001C1 RID: 449 RVA: 0x0001648C File Offset: 0x0001468C
		public static void Open()
		{
			Main.ClosePlayerChat();
			Main.chatText = "";
			Main.playerInventory = false;
			Main.editChest = false;
			Main.npcChatText = "";
			SoundEngine.PlaySound(10, -1, -1, 1, 1f, 0f);
			Main.ingameOptionsWindow = true;
			IngameOptions.category = 0;
			for (int i = 0; i < IngameOptions.leftScale.Length; i++)
			{
				IngameOptions.leftScale[i] = 0f;
			}
			for (int j = 0; j < IngameOptions.rightScale.Length; j++)
			{
				IngameOptions.rightScale[j] = 0f;
			}
			IngameOptions.leftHover = -1;
			IngameOptions.rightHover = -1;
			IngameOptions.oldLeftHover = -1;
			IngameOptions.oldRightHover = -1;
			IngameOptions.rightLock = -1;
			IngameOptions.inBar = false;
			IngameOptions.notBar = false;
			IngameOptions.noSound = false;
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0001654B File Offset: 0x0001474B
		public static void Close()
		{
			IngameOptions.Close(false);
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00016553 File Offset: 0x00014753
		public static void Close(bool quiet = false)
		{
			if (Main.setKey != -1)
			{
				return;
			}
			Main.ingameOptionsWindow = false;
			if (!quiet)
			{
				SoundEngine.PlaySound(11, -1, -1, 1, 1f, 0f);
			}
			Main.playerInventory = true;
			Main.SaveSettings();
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00016588 File Offset: 0x00014788
		public static void Draw(Main mainInstance, SpriteBatch sb)
		{
			IngameOptions._canConsumeHover = true;
			for (int i = 0; i < IngameOptions.skipRightSlot.Length; i++)
			{
				IngameOptions.skipRightSlot[i] = false;
			}
			bool flag = GameCulture.FromCultureName(GameCulture.CultureName.Russian).IsActive || GameCulture.FromCultureName(GameCulture.CultureName.Portuguese).IsActive || GameCulture.FromCultureName(GameCulture.CultureName.Polish).IsActive || GameCulture.FromCultureName(GameCulture.CultureName.French).IsActive;
			bool isActive = GameCulture.FromCultureName(GameCulture.CultureName.Polish).IsActive;
			bool isActive2 = GameCulture.FromCultureName(GameCulture.CultureName.German).IsActive;
			bool flag2 = GameCulture.FromCultureName(GameCulture.CultureName.Italian).IsActive || GameCulture.FromCultureName(GameCulture.CultureName.Spanish).IsActive;
			bool flag3 = false;
			int num = 70;
			float scale = 0.75f;
			float num2 = 60f;
			float num3 = 300f;
			if (flag)
			{
				flag3 = true;
			}
			if (isActive)
			{
				num3 = 200f;
			}
			new Vector2((float)Main.mouseX, (float)Main.mouseY);
			bool flag4 = Main.mouseLeft && Main.mouseLeftRelease;
			Vector2 value = new Vector2((float)Main.screenWidth, (float)Main.screenHeight);
			Vector2 vector = new Vector2(670f, 480f);
			Vector2 vector2 = value / 2f - vector / 2f;
			int num4 = 20;
			IngameOptions._GUIHover = new Rectangle((int)(vector2.X - (float)num4), (int)(vector2.Y - (float)num4), (int)(vector.X + (float)(num4 * 2)), (int)(vector.Y + (float)(num4 * 2)));
			Utils.DrawInvBG(sb, vector2.X - (float)num4, vector2.Y - (float)num4, vector.X + (float)(num4 * 2), vector.Y + (float)(num4 * 2), new Color(33, 15, 91, 255) * 0.685f);
			if (new Rectangle((int)vector2.X - num4, (int)vector2.Y - num4, (int)vector.X + num4 * 2, (int)vector.Y + num4 * 2).Contains(new Point(Main.mouseX, Main.mouseY)))
			{
				Main.player[Main.myPlayer].mouseInterface = true;
			}
			Utils.DrawBorderString(sb, Language.GetTextValue("GameUI.SettingsMenu"), vector2 + vector * new Vector2(0.5f, 0f), Color.White, 1f, 0.5f, 0f, -1);
			if (flag)
			{
				Utils.DrawInvBG(sb, vector2.X + (float)(num4 / 2), vector2.Y + (float)(num4 * 5 / 2), vector.X / 3f - (float)num4, vector.Y - (float)(num4 * 3), default(Color));
				Utils.DrawInvBG(sb, vector2.X + vector.X / 3f + (float)num4, vector2.Y + (float)(num4 * 5 / 2), vector.X * 2f / 3f - (float)(num4 * 3 / 2), vector.Y - (float)(num4 * 3), default(Color));
			}
			else
			{
				Utils.DrawInvBG(sb, vector2.X + (float)(num4 / 2), vector2.Y + (float)(num4 * 5 / 2), vector.X / 2f - (float)num4, vector.Y - (float)(num4 * 3), default(Color));
				Utils.DrawInvBG(sb, vector2.X + vector.X / 2f + (float)num4, vector2.Y + (float)(num4 * 5 / 2), vector.X / 2f - (float)(num4 * 3 / 2), vector.Y - (float)(num4 * 3), default(Color));
			}
			float num5 = 0.7f;
			float num6 = 0.8f;
			float num7 = 0.01f;
			if (flag)
			{
				num5 = 0.4f;
				num6 = 0.44f;
			}
			if (isActive2)
			{
				num5 = 0.55f;
				num6 = 0.6f;
			}
			if (IngameOptions.oldLeftHover != IngameOptions.leftHover && IngameOptions.leftHover != -1)
			{
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			}
			if (IngameOptions.oldRightHover != IngameOptions.rightHover && IngameOptions.rightHover != -1)
			{
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			}
			if (flag4 && IngameOptions.rightHover != -1 && !IngameOptions.noSound)
			{
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			}
			IngameOptions.oldLeftHover = IngameOptions.leftHover;
			IngameOptions.oldRightHover = IngameOptions.rightHover;
			IngameOptions.noSound = false;
			bool flag5 = SocialAPI.Network != null && SocialAPI.Network.CanInvite();
			int num8 = flag5 ? 1 : 0;
			int num9 = 5 + num8 + 2;
			Vector2 vector3 = new Vector2(vector2.X + vector.X / 4f, vector2.Y + (float)(num4 * 5 / 2));
			Vector2 vector4 = new Vector2(0f, vector.Y - (float)(num4 * 5)) / (float)(num9 + 1);
			if (flag)
			{
				vector3.X -= 55f;
			}
			UILinkPointNavigator.Shortcuts.INGAMEOPTIONS_BUTTONS_LEFT = num9 + 1;
			for (int j = 0; j <= num9; j++)
			{
				bool flag6 = false;
				int num10;
				if (IngameOptions._leftSideCategoryMapping.TryGetValue(j, out num10))
				{
					flag6 = (IngameOptions.category == num10);
				}
				if (IngameOptions.leftHover == j || flag6)
				{
					IngameOptions.leftScale[j] += num7;
				}
				else
				{
					IngameOptions.leftScale[j] -= num7;
				}
				if (IngameOptions.leftScale[j] < num5)
				{
					IngameOptions.leftScale[j] = num5;
				}
				if (IngameOptions.leftScale[j] > num6)
				{
					IngameOptions.leftScale[j] = num6;
				}
			}
			IngameOptions.leftHover = -1;
			int num11 = IngameOptions.category;
			int num12 = 0;
			if (IngameOptions.DrawLeftSide(sb, Lang.menu[114].Value, num12, vector3, vector4, IngameOptions.leftScale, 0.7f, 0.8f, 0.01f))
			{
				IngameOptions.leftHover = num12;
				if (flag4)
				{
					IngameOptions.category = 0;
					SoundEngine.PlaySound(10, -1, -1, 1, 1f, 0f);
				}
			}
			num12++;
			if (IngameOptions.DrawLeftSide(sb, Lang.menu[210].Value, num12, vector3, vector4, IngameOptions.leftScale, 0.7f, 0.8f, 0.01f))
			{
				IngameOptions.leftHover = num12;
				if (flag4)
				{
					IngameOptions.category = 1;
					SoundEngine.PlaySound(10, -1, -1, 1, 1f, 0f);
				}
			}
			num12++;
			if (IngameOptions.DrawLeftSide(sb, Lang.menu[63].Value, num12, vector3, vector4, IngameOptions.leftScale, 0.7f, 0.8f, 0.01f))
			{
				IngameOptions.leftHover = num12;
				if (flag4)
				{
					IngameOptions.category = 2;
					SoundEngine.PlaySound(10, -1, -1, 1, 1f, 0f);
				}
			}
			num12++;
			if (IngameOptions.DrawLeftSide(sb, Lang.menu[218].Value, num12, vector3, vector4, IngameOptions.leftScale, 0.7f, 0.8f, 0.01f))
			{
				IngameOptions.leftHover = num12;
				if (flag4)
				{
					IngameOptions.category = 3;
					SoundEngine.PlaySound(10, -1, -1, 1, 1f, 0f);
				}
			}
			num12++;
			if (IngameOptions.DrawLeftSide(sb, Lang.menu[66].Value, num12, vector3, vector4, IngameOptions.leftScale, 0.7f, 0.8f, 0.01f))
			{
				IngameOptions.leftHover = num12;
				if (flag4)
				{
					IngameOptions.Close();
					IngameFancyUI.OpenKeybinds();
				}
			}
			num12++;
			if (flag5 && IngameOptions.DrawLeftSide(sb, Lang.menu[147].Value, num12, vector3, vector4, IngameOptions.leftScale, 0.7f, 0.8f, 0.01f))
			{
				IngameOptions.leftHover = num12;
				if (flag4)
				{
					IngameOptions.Close();
					SocialAPI.Network.OpenInviteInterface();
				}
			}
			if (flag5)
			{
				num12++;
			}
			if (IngameOptions.DrawLeftSide(sb, Lang.menu[131].Value, num12, vector3, vector4, IngameOptions.leftScale, 0.7f, 0.8f, 0.01f))
			{
				IngameOptions.leftHover = num12;
				if (flag4)
				{
					IngameOptions.Close();
					IngameFancyUI.OpenAchievements();
				}
			}
			num12++;
			if (IngameOptions.DrawLeftSide(sb, Lang.menu[118].Value, num12, vector3, vector4, IngameOptions.leftScale, 0.7f, 0.8f, 0.01f))
			{
				IngameOptions.leftHover = num12;
				if (flag4)
				{
					IngameOptions.Close();
				}
			}
			num12++;
			if (IngameOptions.DrawLeftSide(sb, Lang.inter[35].Value, num12, vector3, vector4, IngameOptions.leftScale, 0.7f, 0.8f, 0.01f))
			{
				IngameOptions.leftHover = num12;
				if (flag4)
				{
					IngameOptions.Close();
					Main.menuMode = 10;
					Main.gameMenu = true;
					WorldGen.SaveAndQuit(null);
				}
			}
			num12++;
			if (num11 != IngameOptions.category)
			{
				for (int k = 0; k < IngameOptions.rightScale.Length; k++)
				{
					IngameOptions.rightScale[k] = 0f;
				}
			}
			int num13 = 0;
			int num14 = 0;
			switch (IngameOptions.category)
			{
			case 0:
				num14 = 17;
				num5 = 1f;
				num6 = 1.001f;
				num7 = 0.001f;
				break;
			case 1:
				num14 = 14;
				num5 = 1f;
				num6 = 1.001f;
				num7 = 0.001f;
				break;
			case 2:
				num14 = 14;
				num5 = 1f;
				num6 = 1.001f;
				num7 = 0.001f;
				break;
			case 3:
				num14 = 15;
				num5 = 1f;
				num6 = 1.001f;
				num7 = 0.001f;
				break;
			}
			if (flag)
			{
				num5 -= 0.1f;
				num6 -= 0.1f;
			}
			if (isActive2 && IngameOptions.category == 3)
			{
				num5 -= 0.15f;
				num6 -= 0.15f;
			}
			if (flag2 && (IngameOptions.category == 0 || IngameOptions.category == 3))
			{
				num5 -= 0.2f;
				num6 -= 0.2f;
			}
			UILinkPointNavigator.Shortcuts.INGAMEOPTIONS_BUTTONS_RIGHT = num14;
			Vector2 vector5 = new Vector2(vector2.X + vector.X * 3f / 4f, vector2.Y + (float)(num4 * 5 / 2));
			Vector2 vector6 = new Vector2(0f, vector.Y - (float)(num4 * 3)) / (float)(num14 + 1);
			if (IngameOptions.category == 2)
			{
				vector6.Y -= 2f;
			}
			new Vector2(8f, 0f);
			if (flag)
			{
				vector5.X = vector2.X + vector.X * 2f / 3f;
			}
			for (int l = 0; l < IngameOptions.rightScale.Length; l++)
			{
				if (IngameOptions.rightLock == l || (IngameOptions.rightHover == l && IngameOptions.rightLock == -1))
				{
					IngameOptions.rightScale[l] += num7;
				}
				else
				{
					IngameOptions.rightScale[l] -= num7;
				}
				if (IngameOptions.rightScale[l] < num5)
				{
					IngameOptions.rightScale[l] = num5;
				}
				if (IngameOptions.rightScale[l] > num6)
				{
					IngameOptions.rightScale[l] = num6;
				}
			}
			IngameOptions.inBar = false;
			IngameOptions.rightHover = -1;
			if (!Main.mouseLeft)
			{
				IngameOptions.rightLock = -1;
			}
			if (IngameOptions.rightLock == -1)
			{
				IngameOptions.notBar = false;
			}
			if (IngameOptions.category == 0)
			{
				int num15 = 0;
				IngameOptions.DrawRightSide(sb, Lang.menu[65].Value, num15, vector5, vector6, IngameOptions.rightScale[num15], 1f, default(Color));
				IngameOptions.skipRightSlot[num15] = true;
				num15++;
				vector5.X -= (float)num;
				if (IngameOptions.DrawRightSide(sb, string.Concat(new object[]
				{
					Lang.menu[99].Value,
					" ",
					Math.Round((double)(Main.musicVolume * 100f)),
					"%"
				}), num15, vector5, vector6, IngameOptions.rightScale[num15], (IngameOptions.rightScale[num15] - num5) / (num6 - num5), default(Color)))
				{
					if (IngameOptions.rightLock == -1)
					{
						IngameOptions.notBar = true;
					}
					IngameOptions.noSound = true;
					IngameOptions.rightHover = num15;
				}
				IngameOptions.valuePosition.X = vector2.X + vector.X - (float)(num4 / 2) - 20f;
				IngameOptions.valuePosition.Y = IngameOptions.valuePosition.Y - 3f;
				float musicVolume = IngameOptions.DrawValueBar(sb, scale, Main.musicVolume, 0, null);
				if ((IngameOptions.inBar || IngameOptions.rightLock == num15) && !IngameOptions.notBar)
				{
					IngameOptions.rightHover = num15;
					if (Main.mouseLeft && IngameOptions.rightLock == num15)
					{
						Main.musicVolume = musicVolume;
					}
				}
				if ((float)Main.mouseX > vector2.X + vector.X * 2f / 3f + (float)num4 && (float)Main.mouseX < IngameOptions.valuePosition.X + 3.75f && (float)Main.mouseY > IngameOptions.valuePosition.Y - 10f && (float)Main.mouseY <= IngameOptions.valuePosition.Y + 10f)
				{
					if (IngameOptions.rightLock == -1)
					{
						IngameOptions.notBar = true;
					}
					IngameOptions.rightHover = num15;
				}
				if (IngameOptions.rightHover == num15)
				{
					UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 2;
				}
				num15++;
				if (IngameOptions.DrawRightSide(sb, string.Concat(new object[]
				{
					Lang.menu[98].Value,
					" ",
					Math.Round((double)(Main.soundVolume * 100f)),
					"%"
				}), num15, vector5, vector6, IngameOptions.rightScale[num15], (IngameOptions.rightScale[num15] - num5) / (num6 - num5), default(Color)))
				{
					if (IngameOptions.rightLock == -1)
					{
						IngameOptions.notBar = true;
					}
					IngameOptions.rightHover = num15;
				}
				IngameOptions.valuePosition.X = vector2.X + vector.X - (float)(num4 / 2) - 20f;
				IngameOptions.valuePosition.Y = IngameOptions.valuePosition.Y - 3f;
				float soundVolume = IngameOptions.DrawValueBar(sb, scale, Main.soundVolume, 0, null);
				if ((IngameOptions.inBar || IngameOptions.rightLock == num15) && !IngameOptions.notBar)
				{
					IngameOptions.rightHover = num15;
					if (Main.mouseLeft && IngameOptions.rightLock == num15)
					{
						Main.soundVolume = soundVolume;
						IngameOptions.noSound = true;
					}
				}
				if ((float)Main.mouseX > vector2.X + vector.X * 2f / 3f + (float)num4 && (float)Main.mouseX < IngameOptions.valuePosition.X + 3.75f && (float)Main.mouseY > IngameOptions.valuePosition.Y - 10f && (float)Main.mouseY <= IngameOptions.valuePosition.Y + 10f)
				{
					if (IngameOptions.rightLock == -1)
					{
						IngameOptions.notBar = true;
					}
					IngameOptions.rightHover = num15;
				}
				if (IngameOptions.rightHover == num15)
				{
					UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 3;
				}
				num15++;
				if (IngameOptions.DrawRightSide(sb, string.Concat(new object[]
				{
					Lang.menu[119].Value,
					" ",
					Math.Round((double)(Main.ambientVolume * 100f)),
					"%"
				}), num15, vector5, vector6, IngameOptions.rightScale[num15], (IngameOptions.rightScale[num15] - num5) / (num6 - num5), default(Color)))
				{
					if (IngameOptions.rightLock == -1)
					{
						IngameOptions.notBar = true;
					}
					IngameOptions.rightHover = num15;
				}
				IngameOptions.valuePosition.X = vector2.X + vector.X - (float)(num4 / 2) - 20f;
				IngameOptions.valuePosition.Y = IngameOptions.valuePosition.Y - 3f;
				float ambientVolume = IngameOptions.DrawValueBar(sb, scale, Main.ambientVolume, 0, null);
				if ((IngameOptions.inBar || IngameOptions.rightLock == num15) && !IngameOptions.notBar)
				{
					IngameOptions.rightHover = num15;
					if (Main.mouseLeft && IngameOptions.rightLock == num15)
					{
						Main.ambientVolume = ambientVolume;
						IngameOptions.noSound = true;
					}
				}
				if ((float)Main.mouseX > vector2.X + vector.X * 2f / 3f + (float)num4 && (float)Main.mouseX < IngameOptions.valuePosition.X + 3.75f && (float)Main.mouseY > IngameOptions.valuePosition.Y - 10f && (float)Main.mouseY <= IngameOptions.valuePosition.Y + 10f)
				{
					if (IngameOptions.rightLock == -1)
					{
						IngameOptions.notBar = true;
					}
					IngameOptions.rightHover = num15;
				}
				if (IngameOptions.rightHover == num15)
				{
					UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 4;
				}
				num15++;
				vector5.X += (float)num;
				IngameOptions.DrawRightSide(sb, "", num15, vector5, vector6, IngameOptions.rightScale[num15], 1f, default(Color));
				IngameOptions.skipRightSlot[num15] = true;
				num15++;
				IngameOptions.DrawRightSide(sb, Language.GetTextValue("GameUI.ZoomCategory"), num15, vector5, vector6, IngameOptions.rightScale[num15], 1f, default(Color));
				IngameOptions.skipRightSlot[num15] = true;
				num15++;
				vector5.X -= (float)num;
				string text = Language.GetTextValue("GameUI.GameZoom", Math.Round((double)(Main.GameZoomTarget * 100f)), Math.Round((double)(Main.GameViewMatrix.Zoom.X * 100f)));
				if (flag3)
				{
					text = FontAssets.ItemStack.Value.CreateWrappedText(text, num3, Language.ActiveCulture.CultureInfo);
				}
				if (IngameOptions.DrawRightSide(sb, text, num15, vector5, vector6, IngameOptions.rightScale[num15] * 0.85f, (IngameOptions.rightScale[num15] - num5) / (num6 - num5), default(Color)))
				{
					if (IngameOptions.rightLock == -1)
					{
						IngameOptions.notBar = true;
					}
					IngameOptions.rightHover = num15;
				}
				IngameOptions.valuePosition.X = vector2.X + vector.X - (float)(num4 / 2) - 20f;
				IngameOptions.valuePosition.Y = IngameOptions.valuePosition.Y - 3f;
				float num16 = IngameOptions.DrawValueBar(sb, scale, Main.GameZoomTarget - 1f, 0, null);
				if ((IngameOptions.inBar || IngameOptions.rightLock == num15) && !IngameOptions.notBar)
				{
					IngameOptions.rightHover = num15;
					if (Main.mouseLeft && IngameOptions.rightLock == num15)
					{
						Main.GameZoomTarget = num16 + 1f;
					}
				}
				if ((float)Main.mouseX > vector2.X + vector.X * 2f / 3f + (float)num4 && (float)Main.mouseX < IngameOptions.valuePosition.X + 3.75f && (float)Main.mouseY > IngameOptions.valuePosition.Y - 10f && (float)Main.mouseY <= IngameOptions.valuePosition.Y + 10f)
				{
					if (IngameOptions.rightLock == -1)
					{
						IngameOptions.notBar = true;
					}
					IngameOptions.rightHover = num15;
				}
				if (IngameOptions.rightHover == num15)
				{
					UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 10;
				}
				num15++;
				bool flag7 = false;
				if (Main.temporaryGUIScaleSlider == -1f)
				{
					Main.temporaryGUIScaleSlider = Main.UIScaleWanted;
				}
				string text2 = Language.GetTextValue("GameUI.UIScale", Math.Round((double)(Main.temporaryGUIScaleSlider * 100f)), Math.Round((double)(Main.UIScale * 100f)));
				if (flag3)
				{
					text2 = FontAssets.ItemStack.Value.CreateWrappedText(text2, num3, Language.ActiveCulture.CultureInfo);
				}
				if (IngameOptions.DrawRightSide(sb, text2, num15, vector5, vector6, IngameOptions.rightScale[num15] * 0.75f, (IngameOptions.rightScale[num15] - num5) / (num6 - num5), default(Color)))
				{
					if (IngameOptions.rightLock == -1)
					{
						IngameOptions.notBar = true;
					}
					IngameOptions.rightHover = num15;
				}
				IngameOptions.valuePosition.X = vector2.X + vector.X - (float)(num4 / 2) - 20f;
				IngameOptions.valuePosition.Y = IngameOptions.valuePosition.Y - 3f;
				float num17 = IngameOptions.DrawValueBar(sb, scale, MathHelper.Clamp((Main.temporaryGUIScaleSlider - 0.5f) / 1.5f, 0f, 1f), 0, null);
				if ((IngameOptions.inBar || IngameOptions.rightLock == num15) && !IngameOptions.notBar)
				{
					IngameOptions.rightHover = num15;
					if (Main.mouseLeft && IngameOptions.rightLock == num15)
					{
						Main.temporaryGUIScaleSlider = num17 * 1.5f + 0.5f;
						Main.temporaryGUIScaleSlider = (float)((int)(Main.temporaryGUIScaleSlider * 100f)) / 100f;
						Main.temporaryGUIScaleSliderUpdate = true;
						flag7 = true;
					}
				}
				if (!flag7 && Main.temporaryGUIScaleSliderUpdate && Main.temporaryGUIScaleSlider != -1f)
				{
					Main.UIScale = Main.temporaryGUIScaleSlider;
					Main.temporaryGUIScaleSliderUpdate = false;
				}
				if ((float)Main.mouseX > vector2.X + vector.X * 2f / 3f + (float)num4 && (float)Main.mouseX < IngameOptions.valuePosition.X + 3.75f && (float)Main.mouseY > IngameOptions.valuePosition.Y - 10f && (float)Main.mouseY <= IngameOptions.valuePosition.Y + 10f)
				{
					if (IngameOptions.rightLock == -1)
					{
						IngameOptions.notBar = true;
					}
					IngameOptions.rightHover = num15;
				}
				if (IngameOptions.rightHover == num15)
				{
					UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 11;
				}
				num15++;
				vector5.X += (float)num;
				IngameOptions.DrawRightSide(sb, "", num15, vector5, vector6, IngameOptions.rightScale[num15], 1f, default(Color));
				IngameOptions.skipRightSlot[num15] = true;
				num15++;
				IngameOptions.DrawRightSide(sb, Language.GetTextValue("GameUI.Gameplay"), num15, vector5, vector6, IngameOptions.rightScale[num15], 1f, default(Color));
				IngameOptions.skipRightSlot[num15] = true;
				num15++;
				if (IngameOptions.DrawRightSide(sb, Main.autoSave ? Lang.menu[67].Value : Lang.menu[68].Value, num15, vector5, vector6, IngameOptions.rightScale[num15], (IngameOptions.rightScale[num15] - num5) / (num6 - num5), default(Color)))
				{
					IngameOptions.rightHover = num15;
					if (flag4)
					{
						Main.autoSave = !Main.autoSave;
					}
				}
				num15++;
				if (IngameOptions.DrawRightSide(sb, Main.autoPause ? Lang.menu[69].Value : Lang.menu[70].Value, num15, vector5, vector6, IngameOptions.rightScale[num15], (IngameOptions.rightScale[num15] - num5) / (num6 - num5), default(Color)))
				{
					IngameOptions.rightHover = num15;
					if (flag4)
					{
						Main.autoPause = !Main.autoPause;
					}
				}
				num15++;
				string textValue = Language.GetTextValue(Main.SettingPlayWhenUnfocused ? "UI.PlayWhenUnfocusedOn" : "UI.PlayWhenUnfocusedOff");
				if (IngameOptions.DrawRightSide(sb, textValue, num15, vector5, vector6, IngameOptions.rightScale[num15], (IngameOptions.rightScale[num15] - num5) / (num6 - num5), default(Color)))
				{
					IngameOptions.rightHover = num15;
					if (flag4)
					{
						Main.SettingPlayWhenUnfocused = !Main.SettingPlayWhenUnfocused;
					}
				}
				num15++;
				if (IngameOptions.DrawRightSide(sb, Main.ReversedUpDownArmorSetBonuses ? Lang.menu[220].Value : Lang.menu[221].Value, num15, vector5, vector6, IngameOptions.rightScale[num15], (IngameOptions.rightScale[num15] - num5) / (num6 - num5), default(Color)))
				{
					IngameOptions.rightHover = num15;
					if (flag4)
					{
						Main.ReversedUpDownArmorSetBonuses = !Main.ReversedUpDownArmorSetBonuses;
					}
				}
				num15++;
				string textValue2;
				switch (DoorOpeningHelper.PreferenceSettings)
				{
				default:
					textValue2 = Language.GetTextValue("UI.SmartDoorsDisabled");
					break;
				case DoorOpeningHelper.DoorAutoOpeningPreference.EnabledForGamepadOnly:
					textValue2 = Language.GetTextValue("UI.SmartDoorsGamepad");
					break;
				case DoorOpeningHelper.DoorAutoOpeningPreference.EnabledForEverything:
					textValue2 = Language.GetTextValue("UI.SmartDoorsEnabled");
					break;
				}
				if (IngameOptions.DrawRightSide(sb, textValue2, num15, vector5, vector6, IngameOptions.rightScale[num15], (IngameOptions.rightScale[num15] - num5) / (num6 - num5), default(Color)))
				{
					IngameOptions.rightHover = num15;
					if (flag4)
					{
						DoorOpeningHelper.CyclePreferences();
					}
				}
				num15++;
				Player.Settings.HoverControlMode hoverControl = Player.Settings.HoverControl;
				string textValue3;
				if (hoverControl != Player.Settings.HoverControlMode.Hold)
				{
					textValue3 = Language.GetTextValue("UI.HoverControlSettingIsClick");
				}
				else
				{
					textValue3 = Language.GetTextValue("UI.HoverControlSettingIsHold");
				}
				if (IngameOptions.DrawRightSide(sb, textValue3, num15, vector5, vector6, IngameOptions.rightScale[num15], (IngameOptions.rightScale[num15] - num5) / (num6 - num5), default(Color)))
				{
					IngameOptions.rightHover = num15;
					if (flag4)
					{
						Player.Settings.CycleHoverControl();
					}
				}
				num15++;
				if (IngameOptions.DrawRightSide(sb, Language.GetTextValue(Main.SettingsEnabled_AutoReuseAllItems ? "UI.AutoReuseAllOn" : "UI.AutoReuseAllOff"), num15, vector5, vector6, IngameOptions.rightScale[num15], (IngameOptions.rightScale[num15] - num5) / (num6 - num5), default(Color)))
				{
					IngameOptions.rightHover = num15;
					if (flag4)
					{
						Main.SettingsEnabled_AutoReuseAllItems = !Main.SettingsEnabled_AutoReuseAllItems;
					}
				}
				num15++;
				IngameOptions.DrawRightSide(sb, "", num15, vector5, vector6, IngameOptions.rightScale[num15], 1f, default(Color));
				IngameOptions.skipRightSlot[num15] = true;
				num15++;
			}
			if (IngameOptions.category == 1)
			{
				int num18 = 0;
				if (IngameOptions.DrawRightSide(sb, Main.showItemText ? Lang.menu[71].Value : Lang.menu[72].Value, num18, vector5, vector6, IngameOptions.rightScale[num18], (IngameOptions.rightScale[num18] - num5) / (num6 - num5), default(Color)))
				{
					IngameOptions.rightHover = num18;
					if (flag4)
					{
						Main.showItemText = !Main.showItemText;
					}
				}
				num18++;
				if (IngameOptions.DrawRightSide(sb, Lang.menu[123].Value + " " + Lang.menu[124 + Main.invasionProgressMode], num18, vector5, vector6, IngameOptions.rightScale[num18], (IngameOptions.rightScale[num18] - num5) / (num6 - num5), default(Color)))
				{
					IngameOptions.rightHover = num18;
					if (flag4)
					{
						Main.invasionProgressMode++;
						if (Main.invasionProgressMode >= 3)
						{
							Main.invasionProgressMode = 0;
						}
					}
				}
				num18++;
				if (IngameOptions.DrawRightSide(sb, Main.placementPreview ? Lang.menu[128].Value : Lang.menu[129].Value, num18, vector5, vector6, IngameOptions.rightScale[num18], (IngameOptions.rightScale[num18] - num5) / (num6 - num5), default(Color)))
				{
					IngameOptions.rightHover = num18;
					if (flag4)
					{
						Main.placementPreview = !Main.placementPreview;
					}
				}
				num18++;
				if (IngameOptions.DrawRightSide(sb, ItemSlot.Options.HighlightNewItems ? Lang.inter[117].Value : Lang.inter[116].Value, num18, vector5, vector6, IngameOptions.rightScale[num18], (IngameOptions.rightScale[num18] - num5) / (num6 - num5), default(Color)))
				{
					IngameOptions.rightHover = num18;
					if (flag4)
					{
						ItemSlot.Options.HighlightNewItems = !ItemSlot.Options.HighlightNewItems;
					}
				}
				num18++;
				if (IngameOptions.DrawRightSide(sb, Main.MouseShowBuildingGrid ? Lang.menu[229].Value : Lang.menu[230].Value, num18, vector5, vector6, IngameOptions.rightScale[num18], (IngameOptions.rightScale[num18] - num5) / (num6 - num5), default(Color)))
				{
					IngameOptions.rightHover = num18;
					if (flag4)
					{
						Main.MouseShowBuildingGrid = !Main.MouseShowBuildingGrid;
					}
				}
				num18++;
				if (IngameOptions.DrawRightSide(sb, Main.GamepadDisableInstructionsDisplay ? Lang.menu[241].Value : Lang.menu[242].Value, num18, vector5, vector6, IngameOptions.rightScale[num18], (IngameOptions.rightScale[num18] - num5) / (num6 - num5), default(Color)))
				{
					IngameOptions.rightHover = num18;
					if (flag4)
					{
						Main.GamepadDisableInstructionsDisplay = !Main.GamepadDisableInstructionsDisplay;
					}
				}
				num18++;
				string textValue4 = Language.GetTextValue("UI.MinimapFrame_" + Main.MinimapFrameManagerInstance.ActiveSelectionKeyName);
				if (IngameOptions.DrawRightSide(sb, Language.GetTextValue("UI.SelectMapBorder", textValue4), num18, vector5, vector6, IngameOptions.rightScale[num18], (IngameOptions.rightScale[num18] - num5) / (num6 - num5), default(Color)))
				{
					IngameOptions.rightHover = num18;
					if (flag4)
					{
						Main.MinimapFrameManagerInstance.CycleSelection();
					}
				}
				num18++;
				vector5.X -= (float)num;
				string text3 = Language.GetTextValue("GameUI.MapScale", Math.Round((double)(Main.MapScale * 100f)));
				if (flag3)
				{
					text3 = FontAssets.ItemStack.Value.CreateWrappedText(text3, num3, Language.ActiveCulture.CultureInfo);
				}
				if (IngameOptions.DrawRightSide(sb, text3, num18, vector5, vector6, IngameOptions.rightScale[num18] * 0.85f, (IngameOptions.rightScale[num18] - num5) / (num6 - num5), default(Color)))
				{
					if (IngameOptions.rightLock == -1)
					{
						IngameOptions.notBar = true;
					}
					IngameOptions.rightHover = num18;
				}
				IngameOptions.valuePosition.X = vector2.X + vector.X - (float)(num4 / 2) - 20f;
				IngameOptions.valuePosition.Y = IngameOptions.valuePosition.Y - 3f;
				float num19 = IngameOptions.DrawValueBar(sb, scale, (Main.MapScale - 0.5f) / 0.5f, 0, null);
				if ((IngameOptions.inBar || IngameOptions.rightLock == num18) && !IngameOptions.notBar)
				{
					IngameOptions.rightHover = num18;
					if (Main.mouseLeft && IngameOptions.rightLock == num18)
					{
						Main.MapScale = num19 * 0.5f + 0.5f;
					}
				}
				if ((float)Main.mouseX > vector2.X + vector.X * 2f / 3f + (float)num4 && (float)Main.mouseX < IngameOptions.valuePosition.X + 3.75f && (float)Main.mouseY > IngameOptions.valuePosition.Y - 10f && (float)Main.mouseY <= IngameOptions.valuePosition.Y + 10f)
				{
					if (IngameOptions.rightLock == -1)
					{
						IngameOptions.notBar = true;
					}
					IngameOptions.rightHover = num18;
				}
				if (IngameOptions.rightHover == num18)
				{
					UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 12;
				}
				num18++;
				vector5.X += (float)num;
				string activeSetKeyName = Main.ResourceSetsManager.ActiveSetKeyName;
				string textValue5 = Language.GetTextValue("UI.HealthManaStyle_" + activeSetKeyName);
				if (IngameOptions.DrawRightSide(sb, Language.GetTextValue("UI.SelectHealthStyle", textValue5), num18, vector5, vector6, IngameOptions.rightScale[num18], (IngameOptions.rightScale[num18] - num5) / (num6 - num5), default(Color)))
				{
					IngameOptions.rightHover = num18;
					if (flag4)
					{
						Main.ResourceSetsManager.CycleResourceSet();
					}
				}
				num18++;
				string textValue6;
				switch (Main.DialoguePortraitPreference)
				{
				default:
					textValue6 = Language.GetTextValue("UI.PortraitsDetailed");
					break;
				case Main.DialoguePortraitDrawOption.CloseUp:
					textValue6 = Language.GetTextValue("UI.PortraitsCloseUp");
					break;
				case Main.DialoguePortraitDrawOption.FullBodyRetro:
					textValue6 = Language.GetTextValue("UI.PortraitsFullBody");
					break;
				case Main.DialoguePortraitDrawOption.Disabled:
					textValue6 = Language.GetTextValue("UI.PortraitsDisabled");
					break;
				}
				if (IngameOptions.DrawRightSide(sb, textValue6, num18, vector5, vector6, IngameOptions.rightScale[num18], (IngameOptions.rightScale[num18] - num5) / (num6 - num5), default(Color)))
				{
					IngameOptions.rightHover = num18;
					if (flag4)
					{
						Main.CycleNPCPortraitMode();
					}
				}
				num18++;
				string textValue7 = Language.GetTextValue(BigProgressBarSystem.ShowText ? "UI.ShowBossLifeTextOn" : "UI.ShowBossLifeTextOff");
				if (IngameOptions.DrawRightSide(sb, textValue7, num18, vector5, vector6, IngameOptions.rightScale[num18], (IngameOptions.rightScale[num18] - num5) / (num6 - num5), default(Color)))
				{
					IngameOptions.rightHover = num18;
					if (flag4)
					{
						BigProgressBarSystem.ToggleShowText();
					}
				}
				num18++;
				if (IngameOptions.DrawRightSide(sb, Main.SettingsEnabled_OpaqueBoxBehindTooltips ? Language.GetTextValue("GameUI.HoverTextBoxesOn") : Language.GetTextValue("GameUI.HoverTextBoxesOff"), num18, vector5, vector6, IngameOptions.rightScale[num18], (IngameOptions.rightScale[num18] - num5) / (num6 - num5), default(Color)))
				{
					IngameOptions.rightHover = num18;
					if (flag4)
					{
						Main.SettingsEnabled_OpaqueBoxBehindTooltips = !Main.SettingsEnabled_OpaqueBoxBehindTooltips;
					}
				}
				num18++;
				string txt = ItemSlot.Options.DisableQuickTrash ? Lang.menu[253].Value : (ItemSlot.Options.DisableLeftShiftTrashCan ? Lang.menu[224].Value : Lang.menu[223].Value);
				if (IngameOptions.DrawRightSide(sb, txt, num18, vector5, vector6, IngameOptions.rightScale[num18], (IngameOptions.rightScale[num18] - num5) / (num6 - num5), default(Color)))
				{
					IngameOptions.rightHover = num18;
					if (flag4)
					{
						if (ItemSlot.Options.DisableQuickTrash)
						{
							ItemSlot.Options.DisableQuickTrash = false;
							ItemSlot.Options.DisableLeftShiftTrashCan = true;
						}
						else if (ItemSlot.Options.DisableLeftShiftTrashCan)
						{
							ItemSlot.Options.DisableLeftShiftTrashCan = false;
						}
						else
						{
							ItemSlot.Options.DisableQuickTrash = true;
							ItemSlot.Options.DisableLeftShiftTrashCan = false;
						}
					}
				}
				num18++;
				string textValue8 = Language.GetTextValue(Main.FlashyEffectsInterface ? "UI.FlashyEffectsInterfaceOn" : "UI.FlashyEffectsInterfaceOff");
				if (IngameOptions.DrawRightSide(sb, textValue8, num18, vector5, vector6, IngameOptions.rightScale[num18], (IngameOptions.rightScale[num18] - num5) / (num6 - num5), default(Color)))
				{
					IngameOptions.rightHover = num18;
					if (flag4)
					{
						Main.FlashyEffectsInterface = !Main.FlashyEffectsInterface;
					}
				}
				num18++;
			}
			if (IngameOptions.category == 2)
			{
				int num20 = 0;
				if (IngameOptions.DrawRightSide(sb, Main.graphics.IsFullScreen ? Lang.menu[49].Value : Lang.menu[50].Value, num20, vector5, vector6, IngameOptions.rightScale[num20], (IngameOptions.rightScale[num20] - num5) / (num6 - num5), default(Color)))
				{
					IngameOptions.rightHover = num20;
					if (flag4)
					{
						Main.ToggleFullScreen();
					}
				}
				num20++;
				if (IngameOptions.DrawRightSide(sb, string.Concat(new object[]
				{
					Lang.menu[51].Value,
					": ",
					Main.PendingResolutionWidth,
					"x",
					Main.PendingResolutionHeight
				}), num20, vector5, vector6, IngameOptions.rightScale[num20], (IngameOptions.rightScale[num20] - num5) / (num6 - num5), default(Color)))
				{
					IngameOptions.rightHover = num20;
					if (flag4)
					{
						int num21 = 0;
						for (int m = 0; m < Main.numDisplayModes; m++)
						{
							if (Main.displayWidth[m] == Main.PendingResolutionWidth && Main.displayHeight[m] == Main.PendingResolutionHeight)
							{
								num21 = m;
								break;
							}
						}
						num21++;
						if (num21 >= Main.numDisplayModes)
						{
							num21 = 0;
						}
						Main.PendingResolutionWidth = Main.displayWidth[num21];
						Main.PendingResolutionHeight = Main.displayHeight[num21];
						Main.SetResolution(Main.PendingResolutionWidth, Main.PendingResolutionHeight);
					}
				}
				num20++;
				vector5.X -= (float)num;
				if (IngameOptions.DrawRightSide(sb, string.Concat(new object[]
				{
					Lang.menu[52].Value,
					": ",
					Main.bgScroll,
					"%"
				}), num20, vector5, vector6, IngameOptions.rightScale[num20], (IngameOptions.rightScale[num20] - num5) / (num6 - num5), default(Color)))
				{
					if (IngameOptions.rightLock == -1)
					{
						IngameOptions.notBar = true;
					}
					IngameOptions.noSound = true;
					IngameOptions.rightHover = num20;
				}
				IngameOptions.valuePosition.X = vector2.X + vector.X - (float)(num4 / 2) - 20f;
				IngameOptions.valuePosition.Y = IngameOptions.valuePosition.Y - 3f;
				float num22 = IngameOptions.DrawValueBar(sb, scale, (float)Main.bgScroll / 100f, 0, null);
				if ((IngameOptions.inBar || IngameOptions.rightLock == num20) && !IngameOptions.notBar)
				{
					IngameOptions.rightHover = num20;
					if (Main.mouseLeft && IngameOptions.rightLock == num20)
					{
						Main.bgScroll = (int)(num22 * 100f);
						Main.caveParallax = 1f - (float)Main.bgScroll / 500f;
					}
				}
				if ((float)Main.mouseX > vector2.X + vector.X * 2f / 3f + (float)num4 && (float)Main.mouseX < IngameOptions.valuePosition.X + 3.75f && (float)Main.mouseY > IngameOptions.valuePosition.Y - 10f && (float)Main.mouseY <= IngameOptions.valuePosition.Y + 10f)
				{
					if (IngameOptions.rightLock == -1)
					{
						IngameOptions.notBar = true;
					}
					IngameOptions.rightHover = num20;
				}
				if (IngameOptions.rightHover == num20)
				{
					UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 1;
				}
				num20++;
				vector5.X += (float)num;
				if (IngameOptions.DrawRightSide(sb, Lang.menu[(int)(247 + Main.FrameSkipMode)].Value, num20, vector5, vector6, IngameOptions.rightScale[num20], (IngameOptions.rightScale[num20] - num5) / (num6 - num5), default(Color)))
				{
					IngameOptions.rightHover = num20;
					if (flag4)
					{
						Main.CycleFrameSkipMode();
					}
				}
				num20++;
				if (IngameOptions.DrawRightSide(sb, Language.GetTextValue("UI.LightMode_" + Lighting.Mode), num20, vector5, vector6, IngameOptions.rightScale[num20], (IngameOptions.rightScale[num20] - num5) / (num6 - num5), default(Color)))
				{
					IngameOptions.rightHover = num20;
					if (flag4)
					{
						Lighting.NextLightMode();
					}
				}
				num20++;
				if (IngameOptions.DrawRightSide(sb, Lang.menu[59 + Main.qaStyle].Value, num20, vector5, vector6, IngameOptions.rightScale[num20], (IngameOptions.rightScale[num20] - num5) / (num6 - num5), default(Color)))
				{
					IngameOptions.rightHover = num20;
					if (flag4)
					{
						Main.qaStyle++;
						if (Main.qaStyle > 3)
						{
							Main.qaStyle = 0;
						}
					}
				}
				num20++;
				if (IngameOptions.DrawRightSide(sb, Main.BackgroundEnabled ? Lang.menu[100].Value : Lang.menu[101].Value, num20, vector5, vector6, IngameOptions.rightScale[num20], (IngameOptions.rightScale[num20] - num5) / (num6 - num5), default(Color)))
				{
					IngameOptions.rightHover = num20;
					if (flag4)
					{
						Main.BackgroundEnabled = !Main.BackgroundEnabled;
					}
				}
				num20++;
				if (IngameOptions.DrawRightSide(sb, ChildSafety.Disabled ? Lang.menu[132].Value : Lang.menu[133].Value, num20, vector5, vector6, IngameOptions.rightScale[num20], (IngameOptions.rightScale[num20] - num5) / (num6 - num5), default(Color)))
				{
					IngameOptions.rightHover = num20;
					if (flag4)
					{
						ChildSafety.Disabled = !ChildSafety.Disabled;
					}
				}
				num20++;
				if (IngameOptions.DrawRightSide(sb, Language.GetTextValue("GameUI.ForegroundSunlightEffects", Main.ForegroundSunlightEffects ? Language.GetTextValue("GameUI.Enabled") : Language.GetTextValue("GameUI.Disabled")), num20, vector5, vector6, IngameOptions.rightScale[num20], (IngameOptions.rightScale[num20] - num5) / (num6 - num5), default(Color)))
				{
					IngameOptions.rightHover = num20;
					if (flag4)
					{
						Main.ForegroundSunlightEffects = !Main.ForegroundSunlightEffects;
					}
				}
				num20++;
				if (IngameOptions.DrawRightSide(sb, Language.GetTextValue("GameUI.HeatDistortion", Main.UseHeatDistortion ? Language.GetTextValue("GameUI.Enabled") : Language.GetTextValue("GameUI.Disabled")), num20, vector5, vector6, IngameOptions.rightScale[num20], (IngameOptions.rightScale[num20] - num5) / (num6 - num5), default(Color)))
				{
					IngameOptions.rightHover = num20;
					if (flag4)
					{
						Main.UseHeatDistortion = !Main.UseHeatDistortion;
					}
				}
				num20++;
				if (IngameOptions.DrawRightSide(sb, Language.GetTextValue("GameUI.StormEffects", Main.UseStormEffects ? Language.GetTextValue("GameUI.Enabled") : Language.GetTextValue("GameUI.Disabled")), num20, vector5, vector6, IngameOptions.rightScale[num20], (IngameOptions.rightScale[num20] - num5) / (num6 - num5), default(Color)))
				{
					IngameOptions.rightHover = num20;
					if (flag4)
					{
						Main.UseStormEffects = !Main.UseStormEffects;
					}
				}
				num20++;
				string textValue9;
				switch (Main.WaveQuality)
				{
				case 1:
					textValue9 = Language.GetTextValue("GameUI.QualityLow");
					break;
				case 2:
					textValue9 = Language.GetTextValue("GameUI.QualityMedium");
					break;
				case 3:
					textValue9 = Language.GetTextValue("GameUI.QualityHigh");
					break;
				default:
					textValue9 = Language.GetTextValue("GameUI.QualityOff");
					break;
				}
				if (IngameOptions.DrawRightSide(sb, Language.GetTextValue("GameUI.WaveQuality", textValue9), num20, vector5, vector6, IngameOptions.rightScale[num20], (IngameOptions.rightScale[num20] - num5) / (num6 - num5), default(Color)))
				{
					IngameOptions.rightHover = num20;
					if (flag4)
					{
						Main.WaveQuality = (Main.WaveQuality + 1) % 4;
					}
				}
				num20++;
				if (IngameOptions.DrawRightSide(sb, Language.GetTextValue("UI.TilesSwayInWind" + (Main.SettingsEnabled_TilesSwayInWind ? "On" : "Off")), num20, vector5, vector6, IngameOptions.rightScale[num20], (IngameOptions.rightScale[num20] - num5) / (num6 - num5), default(Color)))
				{
					IngameOptions.rightHover = num20;
					if (flag4)
					{
						Main.SettingsEnabled_TilesSwayInWind = !Main.SettingsEnabled_TilesSwayInWind;
					}
				}
				num20++;
				if (IngameOptions.DrawRightSide(sb, Language.GetTextValue("GameUI.ScreenShake", Main.UseScreenShake ? Language.GetTextValue("GameUI.Enabled") : Language.GetTextValue("GameUI.Disabled")), num20, vector5, vector6, IngameOptions.rightScale[num20], (IngameOptions.rightScale[num20] - num5) / (num6 - num5), default(Color)))
				{
					IngameOptions.rightHover = num20;
					if (flag4)
					{
						Main.UseScreenShake = !Main.UseScreenShake;
					}
				}
				num20++;
				string textValue10 = Language.GetTextValue(Main.FlashyEffectsWorld ? "UI.FlashyEffectsWorldOn" : "UI.FlashyEffectsWorldOff");
				if (IngameOptions.DrawRightSide(sb, textValue10, num20, vector5, vector6, IngameOptions.rightScale[num20], (IngameOptions.rightScale[num20] - num5) / (num6 - num5), default(Color)))
				{
					IngameOptions.rightHover = num20;
					if (flag4)
					{
						Main.FlashyEffectsWorld = !Main.FlashyEffectsWorld;
					}
				}
				num20++;
			}
			if (IngameOptions.category == 3)
			{
				int num23 = 0;
				float num24 = (float)num;
				if (flag)
				{
					num2 = 126f;
				}
				Vector3 hslvector = Main.mouseColorSlider.GetHSLVector();
				Main.mouseColorSlider.ApplyToMainLegacyBars();
				IngameOptions.DrawRightSide(sb, Lang.menu[64].Value, num23, vector5, vector6, IngameOptions.rightScale[num23], 1f, default(Color));
				IngameOptions.skipRightSlot[num23] = true;
				num23++;
				vector5.X -= num24;
				if (IngameOptions.DrawRightSide(sb, "", num23, vector5, vector6, IngameOptions.rightScale[num23], (IngameOptions.rightScale[num23] - num5) / (num6 - num5), default(Color)))
				{
					if (IngameOptions.rightLock == -1)
					{
						IngameOptions.notBar = true;
					}
					IngameOptions.rightHover = num23;
				}
				IngameOptions.valuePosition.X = vector2.X + vector.X - (float)(num4 / 2) - 20f;
				IngameOptions.valuePosition.Y = IngameOptions.valuePosition.Y - 3f;
				IngameOptions.valuePosition.X = IngameOptions.valuePosition.X - num2;
				DelegateMethods.v3_1 = hslvector;
				float num25 = IngameOptions.DrawValueBar(sb, scale, hslvector.X, 0, new Utils.ColorLerpMethod(DelegateMethods.ColorLerp_HSL_H));
				if ((IngameOptions.inBar || IngameOptions.rightLock == num23) && !IngameOptions.notBar)
				{
					IngameOptions.rightHover = num23;
					if (Main.mouseLeft && IngameOptions.rightLock == num23)
					{
						hslvector.X = num25;
						IngameOptions.noSound = true;
					}
				}
				if ((float)Main.mouseX > vector2.X + vector.X * 2f / 3f + (float)num4 && (float)Main.mouseX < IngameOptions.valuePosition.X + 3.75f && (float)Main.mouseY > IngameOptions.valuePosition.Y - 10f && (float)Main.mouseY <= IngameOptions.valuePosition.Y + 10f)
				{
					if (IngameOptions.rightLock == -1)
					{
						IngameOptions.notBar = true;
					}
					IngameOptions.rightHover = num23;
				}
				if (IngameOptions.rightHover == num23)
				{
					UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 5;
					Main.menuMode = 25;
				}
				num23++;
				if (IngameOptions.DrawRightSide(sb, "", num23, vector5, vector6, IngameOptions.rightScale[num23], (IngameOptions.rightScale[num23] - num5) / (num6 - num5), default(Color)))
				{
					if (IngameOptions.rightLock == -1)
					{
						IngameOptions.notBar = true;
					}
					IngameOptions.rightHover = num23;
				}
				IngameOptions.valuePosition.X = vector2.X + vector.X - (float)(num4 / 2) - 20f;
				IngameOptions.valuePosition.Y = IngameOptions.valuePosition.Y - 3f;
				IngameOptions.valuePosition.X = IngameOptions.valuePosition.X - num2;
				DelegateMethods.v3_1 = hslvector;
				num25 = IngameOptions.DrawValueBar(sb, scale, hslvector.Y, 0, new Utils.ColorLerpMethod(DelegateMethods.ColorLerp_HSL_S));
				if ((IngameOptions.inBar || IngameOptions.rightLock == num23) && !IngameOptions.notBar)
				{
					IngameOptions.rightHover = num23;
					if (Main.mouseLeft && IngameOptions.rightLock == num23)
					{
						hslvector.Y = num25;
						IngameOptions.noSound = true;
					}
				}
				if ((float)Main.mouseX > vector2.X + vector.X * 2f / 3f + (float)num4 && (float)Main.mouseX < IngameOptions.valuePosition.X + 3.75f && (float)Main.mouseY > IngameOptions.valuePosition.Y - 10f && (float)Main.mouseY <= IngameOptions.valuePosition.Y + 10f)
				{
					if (IngameOptions.rightLock == -1)
					{
						IngameOptions.notBar = true;
					}
					IngameOptions.rightHover = num23;
				}
				if (IngameOptions.rightHover == num23)
				{
					UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 6;
					Main.menuMode = 25;
				}
				num23++;
				if (IngameOptions.DrawRightSide(sb, "", num23, vector5, vector6, IngameOptions.rightScale[num23], (IngameOptions.rightScale[num23] - num5) / (num6 - num5), default(Color)))
				{
					if (IngameOptions.rightLock == -1)
					{
						IngameOptions.notBar = true;
					}
					IngameOptions.rightHover = num23;
				}
				IngameOptions.valuePosition.X = vector2.X + vector.X - (float)(num4 / 2) - 20f;
				IngameOptions.valuePosition.Y = IngameOptions.valuePosition.Y - 3f;
				IngameOptions.valuePosition.X = IngameOptions.valuePosition.X - num2;
				DelegateMethods.v3_1 = hslvector;
				DelegateMethods.v3_1.Z = Utils.GetLerpValue(0.15f, 1f, DelegateMethods.v3_1.Z, true);
				num25 = IngameOptions.DrawValueBar(sb, scale, DelegateMethods.v3_1.Z, 0, new Utils.ColorLerpMethod(DelegateMethods.ColorLerp_HSL_L));
				if ((IngameOptions.inBar || IngameOptions.rightLock == num23) && !IngameOptions.notBar)
				{
					IngameOptions.rightHover = num23;
					if (Main.mouseLeft && IngameOptions.rightLock == num23)
					{
						hslvector.Z = num25 * 0.85f + 0.15f;
						IngameOptions.noSound = true;
					}
				}
				if ((float)Main.mouseX > vector2.X + vector.X * 2f / 3f + (float)num4 && (float)Main.mouseX < IngameOptions.valuePosition.X + 3.75f && (float)Main.mouseY > IngameOptions.valuePosition.Y - 10f && (float)Main.mouseY <= IngameOptions.valuePosition.Y + 10f)
				{
					if (IngameOptions.rightLock == -1)
					{
						IngameOptions.notBar = true;
					}
					IngameOptions.rightHover = num23;
				}
				if (IngameOptions.rightHover == num23)
				{
					UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 7;
					Main.menuMode = 25;
				}
				num23++;
				if (hslvector.Z < 0.15f)
				{
					hslvector.Z = 0.15f;
				}
				Main.mouseColorSlider.SetHSL(hslvector);
				Main.mouseColor = Main.mouseColorSlider.GetColor();
				vector5.X += num24;
				IngameOptions.DrawRightSide(sb, "", num23, vector5, vector6, IngameOptions.rightScale[num23], 1f, default(Color));
				IngameOptions.skipRightSlot[num23] = true;
				num23++;
				hslvector = Main.mouseBorderColorSlider.GetHSLVector();
				if (PlayerInput.UsingGamepad && IngameOptions.rightHover == -1)
				{
					Main.mouseBorderColorSlider.ApplyToMainLegacyBars();
				}
				IngameOptions.DrawRightSide(sb, Lang.menu[217].Value, num23, vector5, vector6, IngameOptions.rightScale[num23], 1f, default(Color));
				IngameOptions.skipRightSlot[num23] = true;
				num23++;
				vector5.X -= num24;
				if (IngameOptions.DrawRightSide(sb, "", num23, vector5, vector6, IngameOptions.rightScale[num23], (IngameOptions.rightScale[num23] - num5) / (num6 - num5), default(Color)))
				{
					if (IngameOptions.rightLock == -1)
					{
						IngameOptions.notBar = true;
					}
					IngameOptions.rightHover = num23;
				}
				IngameOptions.valuePosition.X = vector2.X + vector.X - (float)(num4 / 2) - 20f;
				IngameOptions.valuePosition.Y = IngameOptions.valuePosition.Y - 3f;
				IngameOptions.valuePosition.X = IngameOptions.valuePosition.X - num2;
				DelegateMethods.v3_1 = hslvector;
				num25 = IngameOptions.DrawValueBar(sb, scale, hslvector.X, 0, new Utils.ColorLerpMethod(DelegateMethods.ColorLerp_HSL_H));
				if ((IngameOptions.inBar || IngameOptions.rightLock == num23) && !IngameOptions.notBar)
				{
					IngameOptions.rightHover = num23;
					if (Main.mouseLeft && IngameOptions.rightLock == num23)
					{
						hslvector.X = num25;
						IngameOptions.noSound = true;
					}
				}
				if ((float)Main.mouseX > vector2.X + vector.X * 2f / 3f + (float)num4 && (float)Main.mouseX < IngameOptions.valuePosition.X + 3.75f && (float)Main.mouseY > IngameOptions.valuePosition.Y - 10f && (float)Main.mouseY <= IngameOptions.valuePosition.Y + 10f)
				{
					if (IngameOptions.rightLock == -1)
					{
						IngameOptions.notBar = true;
					}
					IngameOptions.rightHover = num23;
				}
				if (IngameOptions.rightHover == num23)
				{
					UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 5;
					Main.menuMode = 252;
				}
				num23++;
				if (IngameOptions.DrawRightSide(sb, "", num23, vector5, vector6, IngameOptions.rightScale[num23], (IngameOptions.rightScale[num23] - num5) / (num6 - num5), default(Color)))
				{
					if (IngameOptions.rightLock == -1)
					{
						IngameOptions.notBar = true;
					}
					IngameOptions.rightHover = num23;
				}
				IngameOptions.valuePosition.X = vector2.X + vector.X - (float)(num4 / 2) - 20f;
				IngameOptions.valuePosition.Y = IngameOptions.valuePosition.Y - 3f;
				IngameOptions.valuePosition.X = IngameOptions.valuePosition.X - num2;
				DelegateMethods.v3_1 = hslvector;
				num25 = IngameOptions.DrawValueBar(sb, scale, hslvector.Y, 0, new Utils.ColorLerpMethod(DelegateMethods.ColorLerp_HSL_S));
				if ((IngameOptions.inBar || IngameOptions.rightLock == num23) && !IngameOptions.notBar)
				{
					IngameOptions.rightHover = num23;
					if (Main.mouseLeft && IngameOptions.rightLock == num23)
					{
						hslvector.Y = num25;
						IngameOptions.noSound = true;
					}
				}
				if ((float)Main.mouseX > vector2.X + vector.X * 2f / 3f + (float)num4 && (float)Main.mouseX < IngameOptions.valuePosition.X + 3.75f && (float)Main.mouseY > IngameOptions.valuePosition.Y - 10f && (float)Main.mouseY <= IngameOptions.valuePosition.Y + 10f)
				{
					if (IngameOptions.rightLock == -1)
					{
						IngameOptions.notBar = true;
					}
					IngameOptions.rightHover = num23;
				}
				if (IngameOptions.rightHover == num23)
				{
					UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 6;
					Main.menuMode = 252;
				}
				num23++;
				if (IngameOptions.DrawRightSide(sb, "", num23, vector5, vector6, IngameOptions.rightScale[num23], (IngameOptions.rightScale[num23] - num5) / (num6 - num5), default(Color)))
				{
					if (IngameOptions.rightLock == -1)
					{
						IngameOptions.notBar = true;
					}
					IngameOptions.rightHover = num23;
				}
				IngameOptions.valuePosition.X = vector2.X + vector.X - (float)(num4 / 2) - 20f;
				IngameOptions.valuePosition.Y = IngameOptions.valuePosition.Y - 3f;
				IngameOptions.valuePosition.X = IngameOptions.valuePosition.X - num2;
				DelegateMethods.v3_1 = hslvector;
				num25 = IngameOptions.DrawValueBar(sb, scale, hslvector.Z, 0, new Utils.ColorLerpMethod(DelegateMethods.ColorLerp_HSL_L));
				if ((IngameOptions.inBar || IngameOptions.rightLock == num23) && !IngameOptions.notBar)
				{
					IngameOptions.rightHover = num23;
					if (Main.mouseLeft && IngameOptions.rightLock == num23)
					{
						hslvector.Z = num25;
						IngameOptions.noSound = true;
					}
				}
				if ((float)Main.mouseX > vector2.X + vector.X * 2f / 3f + (float)num4 && (float)Main.mouseX < IngameOptions.valuePosition.X + 3.75f && (float)Main.mouseY > IngameOptions.valuePosition.Y - 10f && (float)Main.mouseY <= IngameOptions.valuePosition.Y + 10f)
				{
					if (IngameOptions.rightLock == -1)
					{
						IngameOptions.notBar = true;
					}
					IngameOptions.rightHover = num23;
				}
				if (IngameOptions.rightHover == num23)
				{
					UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 7;
					Main.menuMode = 252;
				}
				num23++;
				if (IngameOptions.DrawRightSide(sb, "", num23, vector5, vector6, IngameOptions.rightScale[num23], (IngameOptions.rightScale[num23] - num5) / (num6 - num5), default(Color)))
				{
					if (IngameOptions.rightLock == -1)
					{
						IngameOptions.notBar = true;
					}
					IngameOptions.rightHover = num23;
				}
				IngameOptions.valuePosition.X = vector2.X + vector.X - (float)(num4 / 2) - 20f;
				IngameOptions.valuePosition.Y = IngameOptions.valuePosition.Y - 3f;
				IngameOptions.valuePosition.X = IngameOptions.valuePosition.X - num2;
				DelegateMethods.v3_1 = hslvector;
				float num26 = Main.mouseBorderColorSlider.Alpha;
				num25 = IngameOptions.DrawValueBar(sb, scale, num26, 0, new Utils.ColorLerpMethod(DelegateMethods.ColorLerp_HSL_O));
				if ((IngameOptions.inBar || IngameOptions.rightLock == num23) && !IngameOptions.notBar)
				{
					IngameOptions.rightHover = num23;
					if (Main.mouseLeft && IngameOptions.rightLock == num23)
					{
						num26 = num25;
						IngameOptions.noSound = true;
					}
				}
				if ((float)Main.mouseX > vector2.X + vector.X * 2f / 3f + (float)num4 && (float)Main.mouseX < IngameOptions.valuePosition.X + 3.75f && (float)Main.mouseY > IngameOptions.valuePosition.Y - 10f && (float)Main.mouseY <= IngameOptions.valuePosition.Y + 10f)
				{
					if (IngameOptions.rightLock == -1)
					{
						IngameOptions.notBar = true;
					}
					IngameOptions.rightHover = num23;
				}
				if (IngameOptions.rightHover == num23)
				{
					UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 8;
					Main.menuMode = 252;
				}
				num23++;
				Main.mouseBorderColorSlider.SetHSL(hslvector);
				Main.mouseBorderColorSlider.Alpha = num26;
				Main.MouseBorderColor = Main.mouseBorderColorSlider.GetColor();
				vector5.X += num24;
				IngameOptions.DrawRightSide(sb, "", num23, vector5, vector6, IngameOptions.rightScale[num23], 1f, default(Color));
				IngameOptions.skipRightSlot[num23] = true;
				num23++;
				string txt2 = "";
				switch (LockOnHelper.UseMode)
				{
				case LockOnHelper.LockOnMode.FocusTarget:
					txt2 = Lang.menu[232].Value;
					break;
				case LockOnHelper.LockOnMode.TargetClosest:
					txt2 = Lang.menu[233].Value;
					break;
				case LockOnHelper.LockOnMode.ThreeDS:
					txt2 = Lang.menu[234].Value;
					break;
				}
				if (IngameOptions.DrawRightSide(sb, txt2, num23, vector5, vector6, IngameOptions.rightScale[num23] * 0.9f, (IngameOptions.rightScale[num23] - num5) / (num6 - num5), default(Color)))
				{
					IngameOptions.rightHover = num23;
					if (flag4)
					{
						LockOnHelper.CycleUseModes();
					}
				}
				num23++;
				if (IngameOptions.DrawRightSide(sb, Player.SmartCursorSettings.SmartBlocksEnabled ? Lang.menu[215].Value : Lang.menu[216].Value, num23, vector5, vector6, IngameOptions.rightScale[num23] * 0.9f, (IngameOptions.rightScale[num23] - num5) / (num6 - num5), default(Color)))
				{
					IngameOptions.rightHover = num23;
					if (flag4)
					{
						Player.SmartCursorSettings.SmartBlocksEnabled = !Player.SmartCursorSettings.SmartBlocksEnabled;
					}
				}
				num23++;
				if (IngameOptions.DrawRightSide(sb, Main.cSmartCursorModeIsToggleAndNotHold ? Lang.menu[121].Value : Lang.menu[122].Value, num23, vector5, vector6, IngameOptions.rightScale[num23], (IngameOptions.rightScale[num23] - num5) / (num6 - num5), default(Color)))
				{
					IngameOptions.rightHover = num23;
					if (flag4)
					{
						Main.cSmartCursorModeIsToggleAndNotHold = !Main.cSmartCursorModeIsToggleAndNotHold;
					}
				}
				num23++;
				if (IngameOptions.DrawRightSide(sb, Player.SmartCursorSettings.SmartAxeAfterPickaxe ? Lang.menu[214].Value : Lang.menu[213].Value, num23, vector5, vector6, IngameOptions.rightScale[num23] * 0.9f, (IngameOptions.rightScale[num23] - num5) / (num6 - num5), default(Color)))
				{
					IngameOptions.rightHover = num23;
					if (flag4)
					{
						Player.SmartCursorSettings.SmartAxeAfterPickaxe = !Player.SmartCursorSettings.SmartAxeAfterPickaxe;
					}
				}
				num23++;
			}
			if (IngameOptions.rightHover != -1 && IngameOptions.rightLock == -1)
			{
				IngameOptions.rightLock = IngameOptions.rightHover;
			}
			for (int n = 0; n < num9 + 1; n++)
			{
				UILinkPointNavigator.SetPosition(2900 + n, vector3 + vector4 * (float)(n + 1));
			}
			Vector2 zero = Vector2.Zero;
			if (flag)
			{
				zero.X = -40f;
			}
			for (int num27 = 0; num27 < num14; num27++)
			{
				if (!IngameOptions.skipRightSlot[num27])
				{
					UILinkPointNavigator.SetPosition(2930 + num13, vector5 + zero + vector6 * (float)(num27 + 1));
					num13++;
				}
			}
			UILinkPointNavigator.Shortcuts.INGAMEOPTIONS_BUTTONS_RIGHT = num13;
			Main.DrawInterface_29_SettingsButton();
			Main.DrawGamepadInstructions();
			Main.mouseText = false;
			Main.instance.GUIBarsDraw();
			Main.instance.DrawMouseOver();
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.SamplerStateForCursor, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);
			Main.DrawCursor(Main.DrawThickCursor(false), false);
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00019FF4 File Offset: 0x000181F4
		public static void MouseOver()
		{
			if (!Main.ingameOptionsWindow)
			{
				return;
			}
			if (IngameOptions._GUIHover.Contains(Main.MouseScreen.ToPoint()))
			{
				Main.mouseText = true;
			}
			if (IngameOptions._mouseOverText != null)
			{
				Main.instance.MouseText(IngameOptions._mouseOverText, 0, 0, -1, -1, -1, -1, 0);
			}
			IngameOptions._mouseOverText = null;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0001A048 File Offset: 0x00018248
		public static bool DrawLeftSide(SpriteBatch sb, string txt, int i, Vector2 anchor, Vector2 offset, float[] scales, float minscale = 0.7f, float maxscale = 0.8f, float scalespeed = 0.01f)
		{
			bool flag = false;
			int num;
			if (IngameOptions._leftSideCategoryMapping.TryGetValue(i, out num))
			{
				flag = (IngameOptions.category == num);
			}
			Color color = Color.Lerp(Color.Gray, Color.White, (scales[i] - minscale) / (maxscale - minscale));
			if (flag)
			{
				color = Color.Gold;
			}
			Vector2 vector = Utils.DrawBorderStringBig(sb, txt, anchor + offset * (float)(1 + i), color, scales[i], 0.5f, 0.5f, -1);
			bool flag2 = new Rectangle((int)anchor.X - (int)vector.X / 2, (int)anchor.Y + (int)(offset.Y * (float)(1 + i)) - (int)vector.Y / 2, (int)vector.X, (int)vector.Y).Contains(new Point(Main.mouseX, Main.mouseY));
			if (!IngameOptions._canConsumeHover)
			{
				return false;
			}
			if (flag2)
			{
				IngameOptions._canConsumeHover = false;
				return true;
			}
			return false;
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0001A134 File Offset: 0x00018334
		public static bool DrawRightSide(SpriteBatch sb, string txt, int i, Vector2 anchor, Vector2 offset, float scale, float colorScale, Color over = default(Color))
		{
			Color color = Color.Lerp(Color.Gray, Color.White, colorScale);
			if (over != default(Color))
			{
				color = over;
			}
			Vector2 vector = Utils.DrawBorderString(sb, txt, anchor + offset * (float)(1 + i), color, scale, 0.5f, 0.5f, -1);
			IngameOptions.valuePosition = anchor + offset * (float)(1 + i) + vector * new Vector2(0.5f, 0f);
			bool flag = new Rectangle((int)anchor.X - (int)vector.X / 2, (int)anchor.Y + (int)(offset.Y * (float)(1 + i)) - (int)vector.Y / 2, (int)vector.X, (int)vector.Y).Contains(new Point(Main.mouseX, Main.mouseY));
			if (!IngameOptions._canConsumeHover)
			{
				return false;
			}
			if (flag)
			{
				IngameOptions._canConsumeHover = false;
				return true;
			}
			return false;
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0001A230 File Offset: 0x00018430
		public static Rectangle GetExpectedRectangleForNotification(int itemIndex, Vector2 anchor, Vector2 offset, int areaWidth)
		{
			return Utils.CenteredRectangle(anchor + offset * (float)(1 + itemIndex), new Vector2((float)areaWidth, offset.Y - 4f));
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0001A25C File Offset: 0x0001845C
		public static bool DrawValue(SpriteBatch sb, string txt, int i, float scale, Color over = default(Color))
		{
			Color color = Color.Gray;
			Vector2 vector = FontAssets.MouseText.Value.MeasureString(txt) * scale;
			bool flag = new Rectangle((int)IngameOptions.valuePosition.X, (int)IngameOptions.valuePosition.Y - (int)vector.Y / 2, (int)vector.X, (int)vector.Y).Contains(new Point(Main.mouseX, Main.mouseY));
			if (flag)
			{
				color = Color.White;
			}
			if (over != default(Color))
			{
				color = over;
			}
			Utils.DrawBorderString(sb, txt, IngameOptions.valuePosition, color, scale, 0f, 0.5f, -1);
			IngameOptions.valuePosition.X = IngameOptions.valuePosition.X + vector.X;
			if (!IngameOptions._canConsumeHover)
			{
				return false;
			}
			if (flag)
			{
				IngameOptions._canConsumeHover = false;
				return true;
			}
			return false;
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0001A334 File Offset: 0x00018534
		public static float DrawValueBar(SpriteBatch sb, float scale, float perc, int lockState = 0, Utils.ColorLerpMethod colorMethod = null)
		{
			if (colorMethod == null)
			{
				colorMethod = new Utils.ColorLerpMethod(Utils.ColorLerp_BlackToWhite);
			}
			Texture2D value = TextureAssets.ColorBar.Value;
			Vector2 vector = new Vector2((float)value.Width, (float)value.Height) * scale;
			IngameOptions.valuePosition.X = IngameOptions.valuePosition.X - (float)((int)vector.X);
			Rectangle rectangle = new Rectangle((int)IngameOptions.valuePosition.X, (int)IngameOptions.valuePosition.Y - (int)vector.Y / 2, (int)vector.X, (int)vector.Y);
			Rectangle destinationRectangle = rectangle;
			sb.Draw(value, rectangle, Color.White);
			int num = 167;
			float num2 = (float)rectangle.X + 5f * scale;
			float num3 = (float)rectangle.Y + 4f * scale;
			for (float num4 = 0f; num4 < (float)num; num4 += 1f)
			{
				float percent = num4 / (float)num;
				sb.Draw(TextureAssets.ColorBlip.Value, new Vector2(num2 + num4 * scale, num3), null, colorMethod(percent), 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
			}
			rectangle.Inflate((int)(-5f * scale), 0);
			bool flag = rectangle.Contains(new Point(Main.mouseX, Main.mouseY));
			if (lockState == 2)
			{
				flag = false;
			}
			if (flag || lockState == 1)
			{
				sb.Draw(TextureAssets.ColorHighlight.Value, destinationRectangle, Main.OurFavoriteColor);
			}
			sb.Draw(TextureAssets.ColorSlider.Value, new Vector2(num2 + 167f * scale * perc, num3 + 4f * scale), null, Color.White, 0f, new Vector2(0.5f * (float)TextureAssets.ColorSlider.Width(), 0.5f * (float)TextureAssets.ColorSlider.Height()), scale, SpriteEffects.None, 0f);
			if (Main.mouseX >= rectangle.X && Main.mouseX <= rectangle.X + rectangle.Width)
			{
				IngameOptions.inBar = flag;
				return (float)(Main.mouseX - rectangle.X) / (float)rectangle.Width;
			}
			IngameOptions.inBar = false;
			if (rectangle.X >= Main.mouseX)
			{
				return 0f;
			}
			return 1f;
		}

		// Token: 0x04000147 RID: 327
		public const int width = 670;

		// Token: 0x04000148 RID: 328
		public const int height = 480;

		// Token: 0x04000149 RID: 329
		public static float[] leftScale = new float[]
		{
			0.7f,
			0.7f,
			0.7f,
			0.7f,
			0.7f,
			0.7f,
			0.7f,
			0.7f,
			0.7f,
			0.7f
		};

		// Token: 0x0400014A RID: 330
		public static float[] rightScale = new float[]
		{
			0.7f,
			0.7f,
			0.7f,
			0.7f,
			0.7f,
			0.7f,
			0.7f,
			0.7f,
			0.7f,
			0.7f,
			0.7f,
			0.7f,
			0.7f,
			0.7f,
			0.7f,
			0.7f,
			0.7f,
			0.7f,
			0.7f,
			0.7f
		};

		// Token: 0x0400014B RID: 331
		private static Dictionary<int, int> _leftSideCategoryMapping = new Dictionary<int, int>
		{
			{
				0,
				0
			},
			{
				1,
				1
			},
			{
				2,
				2
			},
			{
				3,
				3
			}
		};

		// Token: 0x0400014C RID: 332
		public static bool[] skipRightSlot = new bool[20];

		// Token: 0x0400014D RID: 333
		public static int leftHover = -1;

		// Token: 0x0400014E RID: 334
		public static int rightHover = -1;

		// Token: 0x0400014F RID: 335
		public static int oldLeftHover = -1;

		// Token: 0x04000150 RID: 336
		public static int oldRightHover = -1;

		// Token: 0x04000151 RID: 337
		public static int rightLock = -1;

		// Token: 0x04000152 RID: 338
		public static bool inBar;

		// Token: 0x04000153 RID: 339
		public static bool notBar;

		// Token: 0x04000154 RID: 340
		public static bool noSound;

		// Token: 0x04000155 RID: 341
		private static Rectangle _GUIHover;

		// Token: 0x04000156 RID: 342
		public static int category;

		// Token: 0x04000157 RID: 343
		public static Vector2 valuePosition = Vector2.Zero;

		// Token: 0x04000158 RID: 344
		private static string _mouseOverText;

		// Token: 0x04000159 RID: 345
		private static bool _canConsumeHover;
	}
}
