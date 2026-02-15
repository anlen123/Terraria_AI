using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameInput;

namespace Terraria.GameContent.UI
{
	// Token: 0x0200037A RID: 890
	public class WiresUI
	{
		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x0600295D RID: 10589 RVA: 0x0057AF1C File Offset: 0x0057911C
		public static bool Open
		{
			get
			{
				return WiresUI.radial.active;
			}
		}

		// Token: 0x0600295E RID: 10590 RVA: 0x0057AF28 File Offset: 0x00579128
		public static void HandleWiresUI(SpriteBatch spriteBatch)
		{
			WiresUI.radial.Update();
			WiresUI.radial.Draw(spriteBatch);
		}

		// Token: 0x040051CC RID: 20940
		private static WiresUI.WiresRadial radial = new WiresUI.WiresRadial();

		// Token: 0x020008D5 RID: 2261
		public static class Settings
		{
			// Token: 0x1700056A RID: 1386
			// (get) Token: 0x06004671 RID: 18033 RVA: 0x006C5AE4 File Offset: 0x006C3CE4
			public static bool DrawWires
			{
				get
				{
					return (!Main.noTrapsWorld || NPC.downedBoss3) && (Main.player[Main.myPlayer].inventory[Main.player[Main.myPlayer].selectedItem].mech || (Main.player[Main.myPlayer].InfoAccMechShowWires && Main.player[Main.myPlayer].builderAccStatus[8] == 0));
				}
			}

			// Token: 0x1700056B RID: 1387
			// (get) Token: 0x06004672 RID: 18034 RVA: 0x006C5B54 File Offset: 0x006C3D54
			public static bool HideWires
			{
				get
				{
					return Main.player[Main.myPlayer].inventory[Main.player[Main.myPlayer].selectedItem].type == 3620;
				}
			}

			// Token: 0x1700056C RID: 1388
			// (get) Token: 0x06004673 RID: 18035 RVA: 0x006C5B84 File Offset: 0x006C3D84
			public static bool DrawToolModeUI
			{
				get
				{
					int type = Main.player[Main.myPlayer].inventory[Main.player[Main.myPlayer].selectedItem].type;
					return type == 3611 || type == 3625;
				}
			}

			// Token: 0x1700056D RID: 1389
			// (get) Token: 0x06004674 RID: 18036 RVA: 0x006C5BCC File Offset: 0x006C3DCC
			public static bool DrawToolAllowActuators
			{
				get
				{
					int type = Main.player[Main.myPlayer].inventory[Main.player[Main.myPlayer].selectedItem].type;
					if (type == 3611)
					{
						WiresUI.Settings._lastActuatorEnabled = 2;
					}
					if (type == 3625)
					{
						WiresUI.Settings._lastActuatorEnabled = 1;
					}
					return WiresUI.Settings._lastActuatorEnabled == 2;
				}
			}

			// Token: 0x04007349 RID: 29513
			public static WiresUI.Settings.MultiToolMode ToolMode = WiresUI.Settings.MultiToolMode.Red;

			// Token: 0x0400734A RID: 29514
			private static int _lastActuatorEnabled;

			// Token: 0x02000ADE RID: 2782
			[Flags]
			public enum MultiToolMode
			{
				// Token: 0x0400785E RID: 30814
				Red = 1,
				// Token: 0x0400785F RID: 30815
				Green = 2,
				// Token: 0x04007860 RID: 30816
				Blue = 4,
				// Token: 0x04007861 RID: 30817
				Yellow = 8,
				// Token: 0x04007862 RID: 30818
				Actuator = 16,
				// Token: 0x04007863 RID: 30819
				Cutter = 32
			}
		}

		// Token: 0x020008D6 RID: 2262
		public class WiresRadial
		{
			// Token: 0x06004676 RID: 18038 RVA: 0x006C5C2A File Offset: 0x006C3E2A
			public void Update()
			{
				this.FlowerUpdate();
				this.LineUpdate();
			}

			// Token: 0x06004677 RID: 18039 RVA: 0x006C5C38 File Offset: 0x006C3E38
			private void LineUpdate()
			{
				bool value = true;
				float min = 0.75f;
				Player player = Main.player[Main.myPlayer];
				if (!WiresUI.Settings.DrawToolModeUI || Main.drawingPlayerChat)
				{
					value = false;
					min = 0f;
				}
				if (player.dead || Main.mouseItem.type > 0)
				{
					this._lineOpacity = 0f;
					return;
				}
				if (player.cursorItemIconEnabled && player.cursorItemIconID != 0 && player.cursorItemIconID != 3625)
				{
					this._lineOpacity = 0f;
					return;
				}
				if ((!player.cursorItemIconEnabled && ((!PlayerInput.UsingGamepad && !WiresUI.Settings.DrawToolAllowActuators) || player.mouseInterface || player.lastMouseInterface)) || Main.ingameOptionsWindow || Main.InGameUI.IsVisible)
				{
					this._lineOpacity = 0f;
					return;
				}
				float num = Utils.Clamp<float>(this._lineOpacity + 0.05f * (float)value.ToDirectionInt(), min, 1f);
				this._lineOpacity += 0.05f * (float)Math.Sign(num - this._lineOpacity);
				if (Math.Abs(this._lineOpacity - num) < 0.05f)
				{
					this._lineOpacity = num;
				}
			}

			// Token: 0x06004678 RID: 18040 RVA: 0x006C5D60 File Offset: 0x006C3F60
			private void FlowerUpdate()
			{
				Player player = Main.player[Main.myPlayer];
				if (!WiresUI.Settings.DrawToolModeUI)
				{
					this.active = false;
					return;
				}
				if ((player.mouseInterface || player.lastMouseInterface) && !this.OnWiresMenu)
				{
					this.active = false;
					return;
				}
				if (player.dead || Main.mouseItem.type > 0)
				{
					this.active = false;
					this.OnWiresMenu = false;
					return;
				}
				this.OnWiresMenu = false;
				if (Main.mouseRight && Main.mouseRightRelease && !PlayerInput.LockGamepadTileUseButton && player.noThrow == 0 && !Main.HoveringOverAnNPC && player.talkNPC == -1)
				{
					if (this.active)
					{
						this.active = false;
						return;
					}
					if (!Main.SmartInteractShowingGenuine)
					{
						this.active = true;
						this.position = Main.MouseScreen;
						if (PlayerInput.UsingGamepad && Main.SmartCursorWanted)
						{
							this.position = new Vector2((float)Main.screenWidth, (float)Main.screenHeight) / 2f;
						}
					}
				}
			}

			// Token: 0x06004679 RID: 18041 RVA: 0x006C5E58 File Offset: 0x006C4058
			public void Draw(SpriteBatch spriteBatch)
			{
				this.DrawFlower(spriteBatch);
				this.DrawCursorArea(spriteBatch);
			}

			// Token: 0x0600467A RID: 18042 RVA: 0x006C5E68 File Offset: 0x006C4068
			private void DrawLine(SpriteBatch spriteBatch)
			{
				if (this.active || this._lineOpacity == 0f)
				{
					return;
				}
				Vector2 vector = Main.MouseScreen;
				Vector2 vector2 = new Vector2((float)(Main.screenWidth / 2), (float)(Main.screenHeight - 70));
				if (PlayerInput.UsingGamepad)
				{
					vector = Vector2.Zero;
				}
				Vector2 vector3 = vector - vector2;
				Vector2.Dot(Vector2.Normalize(vector3), Vector2.UnitX);
				Vector2.Dot(Vector2.Normalize(vector3), Vector2.UnitY);
				vector3.ToRotation();
				vector3.Length();
				bool flag = false;
				bool drawToolAllowActuators = WiresUI.Settings.DrawToolAllowActuators;
				for (int i = 0; i < 6; i++)
				{
					if (drawToolAllowActuators || i != 5)
					{
						bool flag2 = (WiresUI.Settings.ToolMode & (WiresUI.Settings.MultiToolMode)(1 << i)) > (WiresUI.Settings.MultiToolMode)0;
						if (i == 5)
						{
							flag2 = ((WiresUI.Settings.ToolMode & WiresUI.Settings.MultiToolMode.Actuator) > (WiresUI.Settings.MultiToolMode)0);
						}
						Vector2 vector4 = vector2 + Vector2.UnitX * (45f * ((float)i - 1.5f));
						int num = i ?? 3;
						if (i == 3)
						{
							num = 0;
						}
						switch (num)
						{
						case 0:
						case 1:
							vector4 = vector2 + new Vector2((45f + (float)(drawToolAllowActuators ? 15 : 0)) * (float)(2 - num), 0f) * this._lineOpacity;
							break;
						case 2:
						case 3:
							vector4 = vector2 + new Vector2(-(45f + (float)(drawToolAllowActuators ? 15 : 0)) * (float)(num - 1), 0f) * this._lineOpacity;
							break;
						case 4:
							flag2 = false;
							vector4 = vector2 - new Vector2(0f, drawToolAllowActuators ? 22f : 0f) * this._lineOpacity;
							break;
						case 5:
							vector4 = vector2 + new Vector2(0f, 22f) * this._lineOpacity;
							break;
						}
						bool flag3 = false;
						if (!PlayerInput.UsingGamepad)
						{
							flag3 = (Vector2.Distance(vector4, vector) < 19f * this._lineOpacity);
						}
						if (flag)
						{
							flag3 = false;
						}
						if (flag3)
						{
							flag = true;
						}
						Texture2D value = TextureAssets.WireUi[(((WiresUI.Settings.ToolMode & WiresUI.Settings.MultiToolMode.Cutter) != (WiresUI.Settings.MultiToolMode)0) ? 8 : 0) + (flag3 ? 1 : 0)].Value;
						Texture2D texture2D = null;
						switch (i)
						{
						case 0:
						case 1:
						case 2:
						case 3:
							texture2D = TextureAssets.WireUi[2 + i].Value;
							break;
						case 4:
							texture2D = TextureAssets.WireUi[((WiresUI.Settings.ToolMode & WiresUI.Settings.MultiToolMode.Cutter) != (WiresUI.Settings.MultiToolMode)0) ? 7 : 6].Value;
							break;
						case 5:
							texture2D = TextureAssets.WireUi[10].Value;
							break;
						}
						Color white = Color.White;
						Color white2 = Color.White;
						if (!flag2 && i != 4)
						{
							if (flag3)
							{
								white2 = new Color(100, 100, 100);
								white2 = new Color(120, 120, 120);
								white = new Color(200, 200, 200);
							}
							else
							{
								white2 = new Color(150, 150, 150);
								white2 = new Color(80, 80, 80);
								white = new Color(100, 100, 100);
							}
						}
						Utils.CenteredRectangle(vector4, new Vector2(40f));
						if (flag3)
						{
							if (Main.mouseLeft && Main.mouseLeftRelease)
							{
								switch (i)
								{
								case 0:
									WiresUI.Settings.ToolMode ^= WiresUI.Settings.MultiToolMode.Red;
									break;
								case 1:
									WiresUI.Settings.ToolMode ^= WiresUI.Settings.MultiToolMode.Green;
									break;
								case 2:
									WiresUI.Settings.ToolMode ^= WiresUI.Settings.MultiToolMode.Blue;
									break;
								case 3:
									WiresUI.Settings.ToolMode ^= WiresUI.Settings.MultiToolMode.Yellow;
									break;
								case 4:
									WiresUI.Settings.ToolMode ^= WiresUI.Settings.MultiToolMode.Cutter;
									break;
								case 5:
									WiresUI.Settings.ToolMode ^= WiresUI.Settings.MultiToolMode.Actuator;
									break;
								}
							}
							if (!Main.mouseLeft || Main.player[Main.myPlayer].mouseInterface)
							{
								Main.player[Main.myPlayer].mouseInterface = true;
							}
							this.OnWiresMenu = true;
						}
						spriteBatch.Draw(value, vector4, null, white * this._lineOpacity, 0f, value.Size() / 2f, this._lineOpacity, SpriteEffects.None, 0f);
						spriteBatch.Draw(texture2D, vector4, null, white2 * this._lineOpacity, 0f, texture2D.Size() / 2f, this._lineOpacity, SpriteEffects.None, 0f);
					}
				}
				if (Main.mouseLeft && Main.mouseLeftRelease && !flag)
				{
					this.active = false;
				}
			}

			// Token: 0x0600467B RID: 18043 RVA: 0x006C62F8 File Offset: 0x006C44F8
			private void DrawFlower(SpriteBatch spriteBatch)
			{
				if (!this.active)
				{
					return;
				}
				Vector2 vector = Main.MouseScreen;
				Vector2 vector2 = this.position;
				if (PlayerInput.UsingGamepad && Main.SmartCursorWanted)
				{
					if (PlayerInput.GamepadThumbstickRight != Vector2.Zero)
					{
						vector = this.position + PlayerInput.GamepadThumbstickRight * 40f;
					}
					else if (PlayerInput.GamepadThumbstickLeft != Vector2.Zero)
					{
						vector = this.position + PlayerInput.GamepadThumbstickLeft * 40f;
					}
					else
					{
						vector = this.position;
					}
				}
				Vector2 vector3 = vector - vector2;
				Vector2.Dot(Vector2.Normalize(vector3), Vector2.UnitX);
				Vector2.Dot(Vector2.Normalize(vector3), Vector2.UnitY);
				float num = vector3.ToRotation();
				float num2 = vector3.Length();
				bool flag = false;
				bool drawToolAllowActuators = WiresUI.Settings.DrawToolAllowActuators;
				float num3 = (float)(4 + drawToolAllowActuators.ToInt());
				float num4 = drawToolAllowActuators ? 11f : -0.5f;
				for (int i = 0; i < 6; i++)
				{
					if (drawToolAllowActuators || i != 5)
					{
						bool flag2 = (WiresUI.Settings.ToolMode & (WiresUI.Settings.MultiToolMode)(1 << i)) > (WiresUI.Settings.MultiToolMode)0;
						if (i == 5)
						{
							flag2 = ((WiresUI.Settings.ToolMode & WiresUI.Settings.MultiToolMode.Actuator) > (WiresUI.Settings.MultiToolMode)0);
						}
						Vector2 vector4 = vector2 + Vector2.UnitX * (45f * ((float)i - 1.5f));
						switch (i)
						{
						case 0:
						case 1:
						case 2:
						case 3:
						{
							float num5 = (float)i;
							if (i == 0)
							{
								num5 = 3f;
							}
							if (i == 3)
							{
								num5 = 0f;
							}
							vector4 = vector2 + Vector2.UnitX.RotatedBy((double)(num5 * 6.2831855f / num3 - 3.1415927f / num4), default(Vector2)) * 45f;
							break;
						}
						case 4:
							flag2 = false;
							vector4 = vector2;
							break;
						case 5:
							vector4 = vector2 + Vector2.UnitX.RotatedBy((double)((float)(i - 1) * 6.2831855f / num3 - 3.1415927f / num4), default(Vector2)) * 45f;
							break;
						}
						bool flag3 = false;
						if (i == 4)
						{
							flag3 = (num2 < 20f);
						}
						switch (i)
						{
						case 0:
						case 1:
						case 2:
						case 3:
						case 5:
						{
							float value = (vector4 - vector2).ToRotation().AngleTowards(num, 6.2831855f / (num3 * 2f)) - num;
							if (num2 >= 20f && Math.Abs(value) < 0.01f)
							{
								flag3 = true;
							}
							break;
						}
						case 4:
							flag3 = (num2 < 20f);
							break;
						}
						if (!PlayerInput.UsingGamepad)
						{
							flag3 = (Vector2.Distance(vector4, vector) < 19f);
						}
						if (flag)
						{
							flag3 = false;
						}
						if (flag3)
						{
							flag = true;
						}
						Texture2D value2 = TextureAssets.WireUi[(((WiresUI.Settings.ToolMode & WiresUI.Settings.MultiToolMode.Cutter) != (WiresUI.Settings.MultiToolMode)0) ? 8 : 0) + (flag3 ? 1 : 0)].Value;
						Texture2D texture2D = null;
						switch (i)
						{
						case 0:
						case 1:
						case 2:
						case 3:
							texture2D = TextureAssets.WireUi[2 + i].Value;
							break;
						case 4:
							texture2D = TextureAssets.WireUi[((WiresUI.Settings.ToolMode & WiresUI.Settings.MultiToolMode.Cutter) != (WiresUI.Settings.MultiToolMode)0) ? 7 : 6].Value;
							break;
						case 5:
							texture2D = TextureAssets.WireUi[10].Value;
							break;
						}
						Color white = Color.White;
						Color white2 = Color.White;
						if (!flag2 && i != 4)
						{
							if (flag3)
							{
								white2 = new Color(100, 100, 100);
								white2 = new Color(120, 120, 120);
								white = new Color(200, 200, 200);
							}
							else
							{
								white2 = new Color(150, 150, 150);
								white2 = new Color(80, 80, 80);
								white = new Color(100, 100, 100);
							}
						}
						Utils.CenteredRectangle(vector4, new Vector2(40f));
						if (flag3)
						{
							if (Main.mouseLeft && Main.mouseLeftRelease)
							{
								switch (i)
								{
								case 0:
									WiresUI.Settings.ToolMode ^= WiresUI.Settings.MultiToolMode.Red;
									break;
								case 1:
									WiresUI.Settings.ToolMode ^= WiresUI.Settings.MultiToolMode.Green;
									break;
								case 2:
									WiresUI.Settings.ToolMode ^= WiresUI.Settings.MultiToolMode.Blue;
									break;
								case 3:
									WiresUI.Settings.ToolMode ^= WiresUI.Settings.MultiToolMode.Yellow;
									break;
								case 4:
									WiresUI.Settings.ToolMode ^= WiresUI.Settings.MultiToolMode.Cutter;
									break;
								case 5:
									WiresUI.Settings.ToolMode ^= WiresUI.Settings.MultiToolMode.Actuator;
									break;
								}
							}
							Main.player[Main.myPlayer].mouseInterface = true;
							this.OnWiresMenu = true;
						}
						spriteBatch.Draw(value2, vector4, null, white, 0f, value2.Size() / 2f, 1f, SpriteEffects.None, 0f);
						spriteBatch.Draw(texture2D, vector4, null, white2, 0f, texture2D.Size() / 2f, 1f, SpriteEffects.None, 0f);
					}
				}
				if (Main.mouseLeft && Main.mouseLeftRelease && !flag)
				{
					this.active = false;
				}
			}

			// Token: 0x0600467C RID: 18044 RVA: 0x006C6800 File Offset: 0x006C4A00
			private void DrawCursorArea(SpriteBatch spriteBatch)
			{
				if (this.active || this._lineOpacity == 0f)
				{
					return;
				}
				Vector2 value = Main.MouseScreen + new Vector2((float)(10 - 9 * PlayerInput.UsingGamepad.ToInt()), 25f);
				Color value2 = new Color(50, 50, 50);
				bool drawToolAllowActuators = WiresUI.Settings.DrawToolAllowActuators;
				if (!drawToolAllowActuators)
				{
					if (!PlayerInput.UsingGamepad)
					{
						value += new Vector2(-20f, 10f);
					}
					else
					{
						value += new Vector2(0f, 10f);
					}
				}
				Texture2D value3 = TextureAssets.BuilderAcc.Value;
				Texture2D texture = value3;
				Rectangle rectangle = new Rectangle(140, 2, 6, 6);
				Rectangle rectangle2 = new Rectangle(148, 2, 6, 6);
				Rectangle rectangle3 = new Rectangle(128, 0, 10, 10);
				float num = 1f;
				float scale = 1f;
				bool flag = false;
				if (flag && !drawToolAllowActuators)
				{
					num *= Main.cursorScale;
				}
				float num2 = this._lineOpacity;
				if (PlayerInput.UsingGamepad)
				{
					num2 *= Main.GamepadCursorAlpha;
				}
				for (int i = 0; i < 5; i++)
				{
					if (drawToolAllowActuators || i != 4)
					{
						float scale2 = num2;
						Vector2 vec = value + Vector2.UnitX * (45f * ((float)i - 1.5f));
						int num3 = i ?? 3;
						if (i == 1)
						{
							num3 = 2;
						}
						if (i == 2)
						{
							num3 = 1;
						}
						if (i == 3)
						{
							num3 = 0;
						}
						if (i == 4)
						{
							num3 = 5;
						}
						int num4 = num3;
						if (num4 == 2)
						{
							num4 = 1;
						}
						else if (num4 == 1)
						{
							num4 = 2;
						}
						bool flag2 = (WiresUI.Settings.ToolMode & (WiresUI.Settings.MultiToolMode)(1 << num4)) > (WiresUI.Settings.MultiToolMode)0;
						if (num4 == 5)
						{
							flag2 = ((WiresUI.Settings.ToolMode & WiresUI.Settings.MultiToolMode.Actuator) > (WiresUI.Settings.MultiToolMode)0);
						}
						Color color = Color.HotPink;
						switch (num3)
						{
						case 0:
							color = new Color(253, 58, 61);
							break;
						case 1:
							color = new Color(83, 180, 253);
							break;
						case 2:
							color = new Color(83, 253, 153);
							break;
						case 3:
							color = new Color(253, 254, 83);
							break;
						case 5:
							color = Color.WhiteSmoke;
							break;
						}
						if (!flag2)
						{
							color = Color.Lerp(color, Color.Black, 0.65f);
						}
						if (flag)
						{
							if (drawToolAllowActuators)
							{
								switch (num3)
								{
								case 0:
									vec = value + new Vector2(-12f, 0f) * num;
									break;
								case 1:
									vec = value + new Vector2(-6f, 12f) * num;
									break;
								case 2:
									vec = value + new Vector2(6f, 12f) * num;
									break;
								case 3:
									vec = value + new Vector2(12f, 0f) * num;
									break;
								case 5:
									vec = value + new Vector2(0f, 0f) * num;
									break;
								}
							}
							else
							{
								vec = value + new Vector2((float)(12 * (num3 + 1)), (float)(12 * (3 - num3))) * num;
							}
						}
						else if (drawToolAllowActuators)
						{
							switch (num3)
							{
							case 0:
								vec = value + new Vector2(-12f, 0f) * num;
								break;
							case 1:
								vec = value + new Vector2(-6f, 12f) * num;
								break;
							case 2:
								vec = value + new Vector2(6f, 12f) * num;
								break;
							case 3:
								vec = value + new Vector2(12f, 0f) * num;
								break;
							case 5:
								vec = value + new Vector2(0f, 0f) * num;
								break;
							}
						}
						else
						{
							float scaleFactor = 0.7f;
							switch (num3)
							{
							case 0:
								vec = value + new Vector2(0f, -12f) * num * scaleFactor;
								break;
							case 1:
								vec = value + new Vector2(-12f, 0f) * num * scaleFactor;
								break;
							case 2:
								vec = value + new Vector2(0f, 12f) * num * scaleFactor;
								break;
							case 3:
								vec = value + new Vector2(12f, 0f) * num * scaleFactor;
								break;
							}
						}
						vec = vec.Floor();
						spriteBatch.Draw(texture, vec, new Rectangle?(rectangle3), value2 * scale2, 0f, rectangle3.Size() / 2f, scale, SpriteEffects.None, 0f);
						spriteBatch.Draw(value3, vec, new Rectangle?(rectangle), color * scale2, 0f, rectangle.Size() / 2f, scale, SpriteEffects.None, 0f);
						if ((WiresUI.Settings.ToolMode & WiresUI.Settings.MultiToolMode.Cutter) != (WiresUI.Settings.MultiToolMode)0)
						{
							spriteBatch.Draw(value3, vec, new Rectangle?(rectangle2), value2 * scale2, 0f, rectangle2.Size() / 2f, scale, SpriteEffects.None, 0f);
						}
					}
				}
			}

			// Token: 0x0400734B RID: 29515
			public Vector2 position;

			// Token: 0x0400734C RID: 29516
			public bool active;

			// Token: 0x0400734D RID: 29517
			public bool OnWiresMenu;

			// Token: 0x0400734E RID: 29518
			private float _lineOpacity;
		}
	}
}
