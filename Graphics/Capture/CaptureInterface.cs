using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.UI.Chat;

namespace Terraria.Graphics.Capture
{
	// Token: 0x020001DB RID: 475
	public class CaptureInterface
	{
		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06001FD7 RID: 8151 RVA: 0x0051DED8 File Offset: 0x0051C0D8
		private static Matrix SelectionZoomMatrix
		{
			get
			{
				CaptureInterface.SelectionContext selectionContext = CaptureInterface._selectionContext;
				if (selectionContext != CaptureInterface.SelectionContext.World)
				{
					return Matrix.Identity;
				}
				return Main.GameViewMatrix.ZoomMatrix;
			}
		}

		// Token: 0x06001FD8 RID: 8152 RVA: 0x0051DF04 File Offset: 0x0051C104
		private static void SetZoom_Context()
		{
			CaptureInterface.SelectionContext selectionContext = CaptureInterface._selectionContext;
			if (selectionContext != CaptureInterface.SelectionContext.World)
			{
				if (selectionContext == CaptureInterface.SelectionContext.Map)
				{
					PlayerInput.SetZoom_Unscaled();
					return;
				}
			}
			else
			{
				PlayerInput.SetZoom_World();
			}
		}

		// Token: 0x06001FD9 RID: 8153 RVA: 0x0051DF29 File Offset: 0x0051C129
		private static Dictionary<int, CaptureInterface.CaptureInterfaceMode> FillModes()
		{
			return new Dictionary<int, CaptureInterface.CaptureInterfaceMode>
			{
				{
					0,
					new CaptureInterface.ModeEdgeSelection()
				},
				{
					1,
					new CaptureInterface.ModeDragBounds()
				},
				{
					2,
					new CaptureInterface.ModeChangeSettings()
				}
			};
		}

		// Token: 0x06001FDA RID: 8154 RVA: 0x0051DF54 File Offset: 0x0051C154
		public static Rectangle GetArea()
		{
			int x = Math.Min(CaptureInterface.EdgeA.X, CaptureInterface.EdgeB.X);
			int y = Math.Min(CaptureInterface.EdgeA.Y, CaptureInterface.EdgeB.Y);
			int num = Math.Abs(CaptureInterface.EdgeA.X - CaptureInterface.EdgeB.X);
			int num2 = Math.Abs(CaptureInterface.EdgeA.Y - CaptureInterface.EdgeB.Y);
			return new Rectangle(x, y, num + 1, num2 + 1);
		}

		// Token: 0x06001FDB RID: 8155 RVA: 0x0051DFD8 File Offset: 0x0051C1D8
		public void Update(CaptureInterface.SelectionContext context)
		{
			CaptureInterface._selectionContext = context;
			if (CaptureInterface.CameraLock)
			{
				return;
			}
			PlayerInput.SetZoom_UI();
			bool toggleCameraMode = PlayerInput.Triggers.Current.ToggleCameraMode;
			if (toggleCameraMode && !this.KeyToggleActiveHeld && (Main.mouseItem.type == 0 || this.Active) && !Main.CaptureModeDisabled && !Main.player[Main.myPlayer].dead && !Main.player[Main.myPlayer].ghost)
			{
				this.ToggleCamera(!this.Active);
			}
			this.KeyToggleActiveHeld = toggleCameraMode;
			if (!this.Active)
			{
				return;
			}
			Main.blockMouse = true;
			if (CaptureInterface.JustActivated && Main.mouseLeftRelease && !Main.mouseLeft)
			{
				CaptureInterface.JustActivated = false;
			}
			Vector2 mouse = new Vector2((float)Main.mouseX, (float)Main.mouseY);
			if (this.UpdateButtons(mouse) && Main.mouseLeft)
			{
				return;
			}
			foreach (KeyValuePair<int, CaptureInterface.CaptureInterfaceMode> keyValuePair in CaptureInterface.Modes)
			{
				keyValuePair.Value.Selected = (keyValuePair.Key == this.SelectedMode);
				keyValuePair.Value.Update();
			}
			PlayerInput.SetZoom_Unscaled();
		}

		// Token: 0x06001FDC RID: 8156 RVA: 0x0051E120 File Offset: 0x0051C320
		public void Draw(SpriteBatch sb)
		{
			if (!this.Active)
			{
				return;
			}
			sb.End();
			sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);
			PlayerInput.SetZoom_UI();
			foreach (CaptureInterface.CaptureInterfaceMode captureInterfaceMode in CaptureInterface.Modes.Values)
			{
				captureInterfaceMode.Draw(sb);
			}
			sb.End();
			sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);
			PlayerInput.SetZoom_UI();
			Main.mouseText = false;
			Main.instance.GUIBarsDraw();
			this.DrawButtons(sb);
			Main.instance.DrawMouseOver();
			sb.End();
			sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);
			Utils.DrawBorderStringBig(sb, Lang.inter[81].Value, new Vector2((float)Main.screenWidth * 0.5f, 100f), Color.White, 1f, 0.5f, 0.5f, -1);
			Utils.DrawCursorSingle(sb, Main.cursorColor, float.NaN, Main.cursorScale, default(Vector2), 0, 0);
			this.DrawCameraLock(sb);
			sb.End();
			sb.Begin();
		}

		// Token: 0x06001FDD RID: 8157 RVA: 0x0051E290 File Offset: 0x0051C490
		public void ToggleCamera(bool On = true)
		{
			if (CaptureInterface.CameraLock)
			{
				return;
			}
			bool active = this.Active;
			this.Active = (CaptureInterface.Modes.ContainsKey(this.SelectedMode) && On);
			if (active != this.Active)
			{
				SoundEngine.PlaySound(On ? 10 : 11, -1, -1, 1, 1f, 0f);
			}
			foreach (KeyValuePair<int, CaptureInterface.CaptureInterfaceMode> keyValuePair in CaptureInterface.Modes)
			{
				keyValuePair.Value.ToggleActive(this.Active && keyValuePair.Key == this.SelectedMode);
			}
			if (On && !active)
			{
				CaptureInterface.JustActivated = true;
			}
		}

		// Token: 0x06001FDE RID: 8158 RVA: 0x0051E35C File Offset: 0x0051C55C
		private bool UpdateButtons(Vector2 mouse)
		{
			this.HoveredMode = -1;
			bool flag = !Main.graphics.IsFullScreen;
			int num = 9;
			for (int i = 0; i < num; i++)
			{
				Rectangle rectangle = new Rectangle(24 + 46 * i, 24, 42, 42);
				if (rectangle.Contains(mouse.ToPoint()))
				{
					this.HoveredMode = i;
					bool flag2 = Main.mouseLeft && Main.mouseLeftRelease;
					int num2 = 0;
					if (i == num2++ && flag2)
					{
						CaptureInterface.QuickScreenshot();
					}
					if (i == num2++ && flag2 && CaptureInterface.EdgeAPinned && CaptureInterface.EdgeBPinned)
					{
						CaptureSettings captureSettings = new CaptureSettings();
						captureSettings.Area = CaptureInterface.GetArea();
						captureSettings.Biome = CaptureBiome.GetCaptureBiome(CaptureInterface.Settings.BiomeChoiceIndex);
						captureSettings.CaptureBackground = !CaptureInterface.Settings.TransparentBackground;
						captureSettings.CaptureEntities = CaptureInterface.Settings.IncludeEntities;
						captureSettings.UseScaling = CaptureInterface.Settings.PackImage;
						captureSettings.CaptureMech = WiresUI.Settings.DrawWires;
						if (captureSettings.Biome.WaterStyle != 13)
						{
							Main.liquidAlpha[13] = 0f;
						}
						CaptureInterface.StartCamera(captureSettings);
					}
					if (i == num2++ && flag2 && this.SelectedMode != 0)
					{
						SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
						this.SelectedMode = 0;
						this.ToggleCamera(true);
					}
					if (i == num2++ && flag2 && this.SelectedMode != 1)
					{
						SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
						this.SelectedMode = 1;
						this.ToggleCamera(true);
					}
					if (i == num2++ && flag2)
					{
						SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
						CaptureInterface.ResetFocus();
					}
					if (i == num2++ && flag2 && Main.mapEnabled)
					{
						SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
						Main.mapFullscreen = !Main.mapFullscreen;
					}
					if (i == num2++ && flag2 && this.SelectedMode != 2)
					{
						SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
						this.SelectedMode = 2;
						this.ToggleCamera(true);
					}
					if (i == num2++ && (flag2 && flag))
					{
						SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
						Utils.OpenFolder(Path.Combine(Main.SavePath, "Captures"));
					}
					if (i == num2++ && flag2)
					{
						this.ToggleCamera(false);
						Main.blockMouse = true;
						Main.mouseLeftRelease = false;
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001FDF RID: 8159 RVA: 0x0051E5E0 File Offset: 0x0051C7E0
		public static Rectangle FullScreenArea()
		{
			int screenWidth = Main.screenWidth;
			int screenHeight = Main.screenHeight;
			Main.screenWidth = PlayerInput.RealScreenWidth;
			Main.screenHeight = PlayerInput.RealScreenHeight;
			Rectangle result;
			try
			{
				Vector2 vector = Main.Camera.ScaledPosition / 16f;
				Vector2 vector2 = (Main.Camera.ScaledPosition + Main.Camera.ScaledSize) / 16f;
				Point point = new Point((int)Math.Ceiling((double)vector.X), (int)Math.Ceiling((double)vector.Y));
				Point point2 = new Point((int)Math.Floor((double)vector2.X), (int)Math.Floor((double)vector2.Y));
				result = new Rectangle(point.X, point.Y, point2.X - point.X, point2.Y - point.Y);
			}
			finally
			{
				Main.screenWidth = screenWidth;
				Main.screenHeight = screenHeight;
			}
			return result;
		}

		// Token: 0x06001FE0 RID: 8160 RVA: 0x0051E6DC File Offset: 0x0051C8DC
		public static void QuickScreenshot()
		{
			CaptureInterface.StartCamera(new CaptureSettings
			{
				Area = CaptureInterface.FullScreenArea(),
				Biome = CaptureBiome.GetCaptureBiome(CaptureInterface.Settings.BiomeChoiceIndex),
				CaptureBackground = !CaptureInterface.Settings.TransparentBackground,
				CaptureEntities = CaptureInterface.Settings.IncludeEntities,
				UseScaling = CaptureInterface.Settings.PackImage,
				CaptureMech = WiresUI.Settings.DrawWires
			});
		}

		// Token: 0x06001FE1 RID: 8161 RVA: 0x0051E740 File Offset: 0x0051C940
		private void DrawButtons(SpriteBatch sb)
		{
			new Vector2((float)Main.mouseX, (float)Main.mouseY);
			int num = 9;
			for (int i = 0; i < num; i++)
			{
				Texture2D texture2D = TextureAssets.InventoryBack.Value;
				float num2 = 0.8f;
				Vector2 vector = new Vector2((float)(24 + 46 * i), 24f);
				Color color = Main.inventoryBack * 0.8f;
				if (this.SelectedMode == 0 && i == 2)
				{
					texture2D = TextureAssets.InventoryBack14.Value;
				}
				else if (this.SelectedMode == 1 && i == 3)
				{
					texture2D = TextureAssets.InventoryBack14.Value;
				}
				else if (this.SelectedMode == 2 && i == 6)
				{
					texture2D = TextureAssets.InventoryBack14.Value;
				}
				else if (i >= 2 && i <= 3)
				{
					texture2D = TextureAssets.InventoryBack2.Value;
				}
				sb.Draw(texture2D, vector, null, color, 0f, default(Vector2), num2, SpriteEffects.None, 0f);
				switch (i)
				{
				case 0:
					texture2D = TextureAssets.Camera[7].Value;
					break;
				case 1:
					texture2D = TextureAssets.Camera[0].Value;
					break;
				case 2:
				case 3:
				case 4:
					texture2D = TextureAssets.Camera[i].Value;
					break;
				case 5:
					texture2D = (Main.mapFullscreen ? TextureAssets.MapIcon[0].Value : TextureAssets.MapIcon[4].Value);
					break;
				case 6:
					texture2D = TextureAssets.Camera[1].Value;
					break;
				case 7:
					texture2D = TextureAssets.Camera[6].Value;
					break;
				case 8:
					texture2D = TextureAssets.Camera[5].Value;
					break;
				}
				sb.Draw(texture2D, vector + new Vector2(26f) * num2, null, Color.White, 0f, texture2D.Size() / 2f, 1f, SpriteEffects.None, 0f);
				bool flag = false;
				if (i != 1)
				{
					if (i != 5)
					{
						if (i == 7)
						{
							if (Main.graphics.IsFullScreen)
							{
								flag = true;
							}
						}
					}
					else if (!Main.mapEnabled)
					{
						flag = true;
					}
				}
				else if (!CaptureInterface.EdgeAPinned || !CaptureInterface.EdgeBPinned)
				{
					flag = true;
				}
				if (flag)
				{
					sb.Draw(TextureAssets.Cd.Value, vector + new Vector2(26f) * num2, null, Color.White * 0.65f, 0f, TextureAssets.Cd.Value.Size() / 2f, 1f, SpriteEffects.None, 0f);
				}
			}
			string text = "";
			switch (this.HoveredMode)
			{
			case -1:
				break;
			case 0:
				text = Lang.inter[111].Value;
				break;
			case 1:
				text = Lang.inter[67].Value;
				break;
			case 2:
				text = Lang.inter[69].Value;
				break;
			case 3:
				text = Lang.inter[70].Value;
				break;
			case 4:
				text = Lang.inter[78].Value;
				break;
			case 5:
				text = (Main.mapFullscreen ? Lang.inter[109].Value : Lang.inter[108].Value);
				break;
			case 6:
				text = Lang.inter[68].Value;
				break;
			case 7:
				text = Lang.inter[110].Value;
				break;
			case 8:
				text = Lang.inter[71].Value;
				break;
			default:
				text = "???";
				break;
			}
			int hoveredMode = this.HoveredMode;
			if (hoveredMode != 1)
			{
				if (hoveredMode != 5)
				{
					if (hoveredMode == 7)
					{
						if (Main.graphics.IsFullScreen)
						{
							text = text + "\n" + Lang.inter[113].Value;
						}
					}
				}
				else if (!Main.mapEnabled)
				{
					text = text + "\n" + Lang.inter[114].Value;
				}
			}
			else if (!CaptureInterface.EdgeAPinned || !CaptureInterface.EdgeBPinned)
			{
				text = text + "\n" + Lang.inter[112].Value;
			}
			if (text != "")
			{
				Main.instance.MouseText(text, 0, 0, -1, -1, -1, -1, 0);
			}
		}

		// Token: 0x06001FE2 RID: 8162 RVA: 0x0051EB78 File Offset: 0x0051CD78
		private static bool GetMapCoords(int PinX, int PinY, int Goal, out Point result)
		{
			if (!Main.mapFullscreen)
			{
				result = new Point(-1, -1);
				return false;
			}
			int num = Main.maxTilesX / MapRenderer.textureMaxWidth;
			int num2 = Main.maxTilesY / MapRenderer.textureMaxHeight;
			float num3 = 10f;
			float num4 = 10f;
			float num5 = (float)(Main.maxTilesX - 10);
			float num6 = (float)(Main.maxTilesY - 10);
			float mapFullscreenScale = Main.mapFullscreenScale;
			float num7 = (float)Main.screenWidth / (float)Main.maxTilesX * 0.8f;
			if (Main.mapFullscreenScale < num7)
			{
				Main.mapFullscreenScale = num7;
			}
			if (Main.mapFullscreenScale > 16f)
			{
				Main.mapFullscreenScale = 16f;
			}
			mapFullscreenScale = Main.mapFullscreenScale;
			if (Main.mapFullscreenPos.X < num3)
			{
				Main.mapFullscreenPos.X = num3;
			}
			if (Main.mapFullscreenPos.X > num5)
			{
				Main.mapFullscreenPos.X = num5;
			}
			if (Main.mapFullscreenPos.Y < num4)
			{
				Main.mapFullscreenPos.Y = num4;
			}
			if (Main.mapFullscreenPos.Y > num6)
			{
				Main.mapFullscreenPos.Y = num6;
			}
			float x = Main.mapFullscreenPos.X;
			float num8 = Main.mapFullscreenPos.Y;
			float num9 = x * mapFullscreenScale;
			num8 *= mapFullscreenScale;
			float num10 = -num9 + (float)(Main.screenWidth / 2);
			float num11 = -num8 + (float)(Main.screenHeight / 2);
			num10 += num3 * mapFullscreenScale;
			num11 += num4 * mapFullscreenScale;
			float num12 = (float)(Main.maxTilesX / 840);
			num12 *= Main.mapFullscreenScale;
			float num13 = num10;
			float num14 = num11;
			float num15 = (float)TextureAssets.Map.Width();
			float num16 = (float)TextureAssets.Map.Height();
			if (Main.maxTilesX == 8400)
			{
				num12 *= 0.999f;
				num13 -= 40.6f * num12;
				num14 = num11 - 5f * num12;
				num15 -= 8.045f;
				num15 *= num12;
				num16 += 0.12f;
				num16 *= num12;
				if ((double)num12 < 1.2)
				{
					num16 += 1f;
				}
			}
			else if (Main.maxTilesX == 6400)
			{
				num12 *= 1.09f;
				num13 -= 38.8f * num12;
				num14 = num11 - 3.85f * num12;
				num15 -= 13.6f;
				num15 *= num12;
				num16 -= 6.92f;
				num16 *= num12;
				if ((double)num12 < 1.2)
				{
					num16 += 2f;
				}
			}
			else if (Main.maxTilesX == 6300)
			{
				num12 *= 1.09f;
				num13 -= 39.8f * num12;
				num14 = num11 - 4.08f * num12;
				num15 -= 26.69f;
				num15 *= num12;
				num16 -= 6.92f;
				num16 *= num12;
				if ((double)num12 < 1.2)
				{
					num16 += 2f;
				}
			}
			else if (Main.maxTilesX == 4200)
			{
				num12 *= 0.998f;
				num13 -= 37.3f * num12;
				num14 -= 1.7f * num12;
				num15 -= 16f;
				num15 *= num12;
				num16 -= 8.31f;
				num16 *= num12;
			}
			if (Goal == 0)
			{
				int num17 = (int)((-num10 + (float)PinX) / mapFullscreenScale + num3);
				int num18 = (int)((-num11 + (float)PinY) / mapFullscreenScale + num4);
				bool flag = false;
				if ((float)num17 < num3)
				{
					flag = true;
				}
				if ((float)num17 >= num5)
				{
					flag = true;
				}
				if ((float)num18 < num4)
				{
					flag = true;
				}
				if ((float)num18 >= num6)
				{
					flag = true;
				}
				if (!flag)
				{
					result = new Point(num17, num18);
					return true;
				}
				result = new Point(-1, -1);
				return false;
			}
			else
			{
				if (Goal == 1)
				{
					Vector2 value = new Vector2(num10, num11);
					Vector2 value2 = new Vector2((float)PinX, (float)PinY) * mapFullscreenScale - new Vector2(10f * mapFullscreenScale);
					result = (value + value2).ToPoint();
					return true;
				}
				result = new Point(-1, -1);
				return false;
			}
		}

		// Token: 0x06001FE3 RID: 8163 RVA: 0x0051EF78 File Offset: 0x0051D178
		private static void ConstraintPoints()
		{
			int fluff = 40;
			if (CaptureInterface.EdgeAPinned)
			{
				CaptureInterface.PointWorldClamp(ref CaptureInterface.EdgeA, fluff);
			}
			if (CaptureInterface.EdgeBPinned)
			{
				CaptureInterface.PointWorldClamp(ref CaptureInterface.EdgeB, fluff);
			}
		}

		// Token: 0x06001FE4 RID: 8164 RVA: 0x0051EFAC File Offset: 0x0051D1AC
		private static void PointWorldClamp(ref Point point, int fluff)
		{
			if (point.X < fluff)
			{
				point.X = fluff;
			}
			if (point.X > Main.maxTilesX - 1 - fluff)
			{
				point.X = Main.maxTilesX - 1 - fluff;
			}
			if (point.Y < fluff)
			{
				point.Y = fluff;
			}
			if (point.Y > Main.maxTilesY - 1 - fluff)
			{
				point.Y = Main.maxTilesY - 1 - fluff;
			}
		}

		// Token: 0x06001FE5 RID: 8165 RVA: 0x0051F019 File Offset: 0x0051D219
		public bool UsingMap()
		{
			return CaptureInterface.CameraLock || CaptureInterface.Modes[this.SelectedMode].UsingMap();
		}

		// Token: 0x06001FE6 RID: 8166 RVA: 0x0051F039 File Offset: 0x0051D239
		public static void ResetFocus()
		{
			CaptureInterface.EdgeAPinned = false;
			CaptureInterface.EdgeBPinned = false;
			CaptureInterface.EdgeA = new Point(-1, -1);
			CaptureInterface.EdgeB = new Point(-1, -1);
		}

		// Token: 0x06001FE7 RID: 8167 RVA: 0x0051F060 File Offset: 0x0051D260
		public void Scrolling()
		{
			int num = PlayerInput.ScrollWheelDelta / 120;
			num %= 30;
			if (num < 0)
			{
				num += 30;
			}
			int selectedMode = this.SelectedMode;
			this.SelectedMode -= num;
			while (this.SelectedMode < 0)
			{
				this.SelectedMode += 2;
			}
			while (this.SelectedMode > 2)
			{
				this.SelectedMode -= 2;
			}
			if (this.SelectedMode != selectedMode)
			{
				SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			}
		}

		// Token: 0x06001FE8 RID: 8168 RVA: 0x0051F0EC File Offset: 0x0051D2EC
		public void UpdateCameraCountdown()
		{
			if (CaptureInterface.CameraLock && CaptureInterface.CameraFrame == 4f)
			{
				CaptureManager.Instance.Capture(CaptureInterface.CameraSettings);
			}
			CaptureInterface.CameraFrame += (float)CaptureInterface.CameraLock.ToDirectionInt();
			if (CaptureInterface.CameraFrame < 0f)
			{
				CaptureInterface.CameraFrame = 0f;
			}
			if (CaptureInterface.CameraFrame > 5f)
			{
				CaptureInterface.CameraFrame = 5f;
			}
			if (CaptureInterface.CameraFrame == 5f)
			{
				CaptureInterface.CameraWaiting += 1f;
			}
			if (CaptureInterface.CameraWaiting > 60f)
			{
				CaptureInterface.CameraWaiting = 60f;
			}
		}

		// Token: 0x06001FE9 RID: 8169 RVA: 0x0051F190 File Offset: 0x0051D390
		private void DrawCameraLock(SpriteBatch sb)
		{
			if (CaptureInterface.CameraFrame == 0f)
			{
				return;
			}
			sb.Draw(TextureAssets.MagicPixel.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Rectangle?(new Rectangle(0, 0, 1, 1)), Color.Black * (CaptureInterface.CameraFrame / 5f));
			if (CaptureInterface.CameraFrame != 5f)
			{
				return;
			}
			float num = CaptureInterface.CameraWaiting - 60f + 5f;
			if (num <= 0f)
			{
				return;
			}
			num /= 5f;
			float num2 = CaptureManager.Instance.GetProgress() * 100f;
			if (num2 > 100f)
			{
				num2 = 100f;
			}
			string text = num2.ToString("##") + " ";
			string text2 = "/ 100%";
			Vector2 vector = FontAssets.DeathText.Value.MeasureString(text);
			Vector2 vector2 = FontAssets.DeathText.Value.MeasureString(text2);
			Vector2 value = new Vector2(-vector.X, -vector.Y / 2f);
			Vector2 value2 = new Vector2(0f, -vector2.Y / 2f);
			ChatManager.DrawColorCodedStringWithShadow(sb, FontAssets.DeathText.Value, text, new Vector2((float)Main.screenWidth, (float)Main.screenHeight) / 2f + value, Color.White * num, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
			ChatManager.DrawColorCodedStringWithShadow(sb, FontAssets.DeathText.Value, text2, new Vector2((float)Main.screenWidth, (float)Main.screenHeight) / 2f + value2, Color.White * num, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
		}

		// Token: 0x06001FEA RID: 8170 RVA: 0x0051F364 File Offset: 0x0051D564
		public static void StartCamera(CaptureSettings settings)
		{
			settings.CameraSpaceEffects = CaptureInterface.FullScreenArea().Contains(settings.Area);
			SoundEngine.PlaySound(40, -1, -1, 1, 1f, 0f);
			CaptureInterface.CameraSettings = settings;
			CaptureInterface.CameraLock = true;
			CaptureInterface.CameraWaiting = 0f;
		}

		// Token: 0x06001FEB RID: 8171 RVA: 0x0051F3B5 File Offset: 0x0051D5B5
		public static void EndCamera()
		{
			CaptureInterface.CameraLock = false;
		}

		// Token: 0x04004A41 RID: 19009
		private static CaptureInterface.SelectionContext _selectionContext;

		// Token: 0x04004A42 RID: 19010
		private static Dictionary<int, CaptureInterface.CaptureInterfaceMode> Modes = CaptureInterface.FillModes();

		// Token: 0x04004A43 RID: 19011
		public bool Active;

		// Token: 0x04004A44 RID: 19012
		public static bool JustActivated;

		// Token: 0x04004A45 RID: 19013
		private bool KeyToggleActiveHeld;

		// Token: 0x04004A46 RID: 19014
		public int SelectedMode;

		// Token: 0x04004A47 RID: 19015
		public int HoveredMode;

		// Token: 0x04004A48 RID: 19016
		public static bool EdgeAPinned;

		// Token: 0x04004A49 RID: 19017
		public static bool EdgeBPinned;

		// Token: 0x04004A4A RID: 19018
		public static Point EdgeA;

		// Token: 0x04004A4B RID: 19019
		public static Point EdgeB;

		// Token: 0x04004A4C RID: 19020
		public static bool CameraLock;

		// Token: 0x04004A4D RID: 19021
		private static float CameraFrame;

		// Token: 0x04004A4E RID: 19022
		private static float CameraWaiting;

		// Token: 0x04004A4F RID: 19023
		private const float CameraMaxFrame = 5f;

		// Token: 0x04004A50 RID: 19024
		private const float CameraMaxWait = 60f;

		// Token: 0x04004A51 RID: 19025
		private static CaptureSettings CameraSettings;

		// Token: 0x02000798 RID: 1944
		public static class Settings
		{
			// Token: 0x0400702F RID: 28719
			public static bool PackImage = true;

			// Token: 0x04007030 RID: 28720
			public static bool IncludeEntities = true;

			// Token: 0x04007031 RID: 28721
			public static bool TransparentBackground;

			// Token: 0x04007032 RID: 28722
			public static int BiomeChoiceIndex = -1;

			// Token: 0x04007033 RID: 28723
			public static int ScreenAnchor = 0;

			// Token: 0x04007034 RID: 28724
			public static Color MarkedAreaColor = new Color(0.8f, 0.8f, 0.8f, 0f) * 0.3f;
		}

		// Token: 0x02000799 RID: 1945
		private abstract class CaptureInterfaceMode
		{
			// Token: 0x06004176 RID: 16758
			public abstract void Update();

			// Token: 0x06004177 RID: 16759
			public abstract void Draw(SpriteBatch sb);

			// Token: 0x06004178 RID: 16760
			public abstract void ToggleActive(bool tickedOn);

			// Token: 0x06004179 RID: 16761
			public abstract bool UsingMap();

			// Token: 0x0600417A RID: 16762 RVA: 0x006B9179 File Offset: 0x006B7379
			protected void StartDrawingSelection(SpriteBatch sb)
			{
				sb.End();
				sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, CaptureInterface.SelectionZoomMatrix);
				CaptureInterface.SetZoom_Context();
			}

			// Token: 0x0600417B RID: 16763 RVA: 0x006B91A7 File Offset: 0x006B73A7
			protected void EndDrawingSelection(SpriteBatch sb)
			{
				sb.End();
				sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);
				PlayerInput.SetZoom_UI();
			}

			// Token: 0x04007035 RID: 28725
			public bool Selected;
		}

		// Token: 0x0200079A RID: 1946
		public enum SelectionContext
		{
			// Token: 0x04007037 RID: 28727
			World,
			// Token: 0x04007038 RID: 28728
			Map
		}

		// Token: 0x0200079B RID: 1947
		private class ModeEdgeSelection : CaptureInterface.CaptureInterfaceMode
		{
			// Token: 0x0600417D RID: 16765 RVA: 0x006B91D8 File Offset: 0x006B73D8
			public override void Update()
			{
				if (!this.Selected)
				{
					return;
				}
				CaptureInterface.SetZoom_Context();
				Vector2 mouse = new Vector2((float)Main.mouseX, (float)Main.mouseY);
				this.EdgePlacement(mouse);
			}

			// Token: 0x0600417E RID: 16766 RVA: 0x006B920D File Offset: 0x006B740D
			public override void Draw(SpriteBatch sb)
			{
				if (!this.Selected)
				{
					return;
				}
				base.StartDrawingSelection(sb);
				this.DrawMarkedArea(sb);
				this.DrawCursors(sb);
				base.EndDrawingSelection(sb);
			}

			// Token: 0x0600417F RID: 16767 RVA: 0x00009E06 File Offset: 0x00008006
			public override void ToggleActive(bool tickedOn)
			{
			}

			// Token: 0x06004180 RID: 16768 RVA: 0x000379F1 File Offset: 0x00035BF1
			public override bool UsingMap()
			{
				return true;
			}

			// Token: 0x06004181 RID: 16769 RVA: 0x006B9234 File Offset: 0x006B7434
			private void EdgePlacement(Vector2 mouse)
			{
				if (CaptureInterface.JustActivated)
				{
					return;
				}
				Point point;
				if (!Main.mapFullscreen)
				{
					if (Main.mouseLeft)
					{
						CaptureInterface.EdgeAPinned = true;
						CaptureInterface.EdgeA = Main.MouseWorld.ToTileCoordinates();
					}
					if (Main.mouseRight)
					{
						CaptureInterface.EdgeBPinned = true;
						CaptureInterface.EdgeB = Main.MouseWorld.ToTileCoordinates();
					}
				}
				else if (CaptureInterface.GetMapCoords((int)mouse.X, (int)mouse.Y, 0, out point))
				{
					if (Main.mouseLeft)
					{
						CaptureInterface.EdgeAPinned = true;
						CaptureInterface.EdgeA = point;
					}
					if (Main.mouseRight)
					{
						CaptureInterface.EdgeBPinned = true;
						CaptureInterface.EdgeB = point;
					}
				}
				CaptureInterface.ConstraintPoints();
			}

			// Token: 0x06004182 RID: 16770 RVA: 0x006B92D0 File Offset: 0x006B74D0
			private void DrawMarkedArea(SpriteBatch sb)
			{
				if (!CaptureInterface.EdgeAPinned || !CaptureInterface.EdgeBPinned)
				{
					return;
				}
				int num = Math.Min(CaptureInterface.EdgeA.X, CaptureInterface.EdgeB.X);
				int num2 = Math.Min(CaptureInterface.EdgeA.Y, CaptureInterface.EdgeB.Y);
				int num3 = Math.Abs(CaptureInterface.EdgeA.X - CaptureInterface.EdgeB.X);
				int num4 = Math.Abs(CaptureInterface.EdgeA.Y - CaptureInterface.EdgeB.Y);
				if (!Main.mapFullscreen)
				{
					Rectangle rectangle = Main.ReverseGravitySupport(new Rectangle(num * 16, num2 * 16, (num3 + 1) * 16, (num4 + 1) * 16));
					Rectangle rectangle2 = Main.ReverseGravitySupport(new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth + 1, Main.screenHeight + 1));
					Rectangle rectangle3;
					Rectangle.Intersect(ref rectangle2, ref rectangle, out rectangle3);
					if (rectangle3.Width == 0 || rectangle3.Height == 0)
					{
						return;
					}
					rectangle3.Offset(-rectangle2.X, -rectangle2.Y);
					sb.Draw(TextureAssets.MagicPixel.Value, rectangle3, CaptureInterface.Settings.MarkedAreaColor);
					for (int i = 0; i < 2; i++)
					{
						sb.Draw(TextureAssets.MagicPixel.Value, new Rectangle(rectangle3.X, rectangle3.Y + ((i == 1) ? rectangle3.Height : -2), rectangle3.Width, 2), Color.White);
						sb.Draw(TextureAssets.MagicPixel.Value, new Rectangle(rectangle3.X + ((i == 1) ? rectangle3.Width : -2), rectangle3.Y, 2, rectangle3.Height), Color.White);
					}
					return;
				}
				else
				{
					Point point;
					CaptureInterface.GetMapCoords(num, num2, 1, out point);
					Point point2;
					CaptureInterface.GetMapCoords(num + num3 + 1, num2 + num4 + 1, 1, out point2);
					Rectangle rectangle4 = new Rectangle(point.X, point.Y, point2.X - point.X, point2.Y - point.Y);
					Rectangle rectangle5 = new Rectangle(0, 0, Main.screenWidth + 1, Main.screenHeight + 1);
					Rectangle rectangle6;
					Rectangle.Intersect(ref rectangle5, ref rectangle4, out rectangle6);
					if (rectangle6.Width == 0 || rectangle6.Height == 0)
					{
						return;
					}
					rectangle6.Offset(-rectangle5.X, -rectangle5.Y);
					sb.Draw(TextureAssets.MagicPixel.Value, rectangle6, CaptureInterface.Settings.MarkedAreaColor);
					for (int j = 0; j < 2; j++)
					{
						sb.Draw(TextureAssets.MagicPixel.Value, new Rectangle(rectangle6.X, rectangle6.Y + ((j == 1) ? rectangle6.Height : -2), rectangle6.Width, 2), Color.White);
						sb.Draw(TextureAssets.MagicPixel.Value, new Rectangle(rectangle6.X + ((j == 1) ? rectangle6.Width : -2), rectangle6.Y, 2, rectangle6.Height), Color.White);
					}
					return;
				}
			}

			// Token: 0x06004183 RID: 16771 RVA: 0x006B95E4 File Offset: 0x006B77E4
			private void DrawCursors(SpriteBatch sb)
			{
				float num = 1f / Main.cursorScale;
				float num2 = 0.8f / num;
				Vector2 vector = Main.screenPosition + new Vector2(30f);
				Vector2 vector2 = vector + new Vector2((float)Main.screenWidth, (float)Main.screenHeight) - new Vector2(60f);
				if (Main.mapFullscreen)
				{
					vector -= Main.screenPosition;
					vector2 -= Main.screenPosition;
				}
				Vector3 vector3 = Main.rgbToHsl(Main.cursorColor);
				Color color = Main.hslToRgb((vector3.X + 0.33f) % 1f, vector3.Y, vector3.Z, byte.MaxValue);
				Color color2 = Main.hslToRgb((vector3.X - 0.33f) % 1f, vector3.Y, vector3.Z, byte.MaxValue);
				color2 = (color = Color.White);
				bool flag = Main.player[Main.myPlayer].gravDir == -1f;
				if (!CaptureInterface.EdgeAPinned)
				{
					Utils.DrawCursorSingle(sb, color, 3.926991f, Main.cursorScale * num * num2, new Vector2((float)Main.mouseX - 5f + 12f, (float)Main.mouseY + 2.5f + 12f), 4, 0);
				}
				else
				{
					int specialMode = 0;
					float num3 = 0f;
					Vector2 vector4 = Vector2.Zero;
					if (!Main.mapFullscreen)
					{
						Vector2 vector5 = CaptureInterface.EdgeA.ToVector2() * 16f;
						if (!CaptureInterface.EdgeBPinned)
						{
							specialMode = 1;
							vector5 += Vector2.One * 8f;
							num3 = (-vector5 + Main.ReverseGravitySupport(new Vector2((float)Main.mouseX, (float)Main.mouseY), 0f) + Main.screenPosition).ToRotation();
							if (flag)
							{
								num3 = -num3;
							}
							vector4 = Vector2.Clamp(vector5, vector, vector2);
							if (vector4 != vector5)
							{
								num3 = (vector5 - vector4).ToRotation();
							}
						}
						else
						{
							Vector2 value = new Vector2((float)((CaptureInterface.EdgeA.X > CaptureInterface.EdgeB.X).ToInt() * 16), (float)((CaptureInterface.EdgeA.Y > CaptureInterface.EdgeB.Y).ToInt() * 16));
							vector5 += value;
							vector4 = Vector2.Clamp(vector5, vector, vector2);
							num3 = (CaptureInterface.EdgeB.ToVector2() * 16f + new Vector2(16f) - value - vector4).ToRotation();
							if (vector4 != vector5)
							{
								num3 = (vector5 - vector4).ToRotation();
								specialMode = 1;
							}
							if (flag)
							{
								num3 *= -1f;
							}
						}
						Utils.DrawCursorSingle(sb, color, num3 - 1.5707964f, Main.cursorScale * num, Main.ReverseGravitySupport(vector4 - Main.screenPosition, 0f), 4, specialMode);
					}
					else
					{
						Point edgeA = CaptureInterface.EdgeA;
						if (CaptureInterface.EdgeBPinned)
						{
							int num4 = (CaptureInterface.EdgeA.X > CaptureInterface.EdgeB.X).ToInt();
							int num5 = (CaptureInterface.EdgeA.Y > CaptureInterface.EdgeB.Y).ToInt();
							edgeA.X += num4;
							edgeA.Y += num5;
							CaptureInterface.GetMapCoords(edgeA.X, edgeA.Y, 1, out edgeA);
							Point edgeB = CaptureInterface.EdgeB;
							edgeB.X += 1 - num4;
							edgeB.Y += 1 - num5;
							CaptureInterface.GetMapCoords(edgeB.X, edgeB.Y, 1, out edgeB);
							vector4 = edgeA.ToVector2();
							vector4 = Vector2.Clamp(vector4, vector, vector2);
							num3 = (edgeB.ToVector2() - vector4).ToRotation();
						}
						else
						{
							CaptureInterface.GetMapCoords(edgeA.X, edgeA.Y, 1, out edgeA);
						}
						Utils.DrawCursorSingle(sb, color, num3 - 1.5707964f, Main.cursorScale * num, edgeA.ToVector2(), 4, 0);
					}
				}
				if (!CaptureInterface.EdgeBPinned)
				{
					Utils.DrawCursorSingle(sb, color2, 0.7853981f, Main.cursorScale * num * num2, new Vector2((float)Main.mouseX + 2.5f + 12f, (float)Main.mouseY - 5f + 12f), 5, 0);
					return;
				}
				int specialMode2 = 0;
				float num6 = 0f;
				Vector2 vector6 = Vector2.Zero;
				if (!Main.mapFullscreen)
				{
					Vector2 vector7 = CaptureInterface.EdgeB.ToVector2() * 16f;
					if (!CaptureInterface.EdgeAPinned)
					{
						specialMode2 = 1;
						vector7 += Vector2.One * 8f;
						num6 = (-vector7 + Main.ReverseGravitySupport(new Vector2((float)Main.mouseX, (float)Main.mouseY), 0f) + Main.screenPosition).ToRotation();
						if (flag)
						{
							num6 = -num6;
						}
						vector6 = Vector2.Clamp(vector7, vector, vector2);
						if (vector6 != vector7)
						{
							num6 = (vector7 - vector6).ToRotation();
						}
					}
					else
					{
						Vector2 value2 = new Vector2((float)((CaptureInterface.EdgeB.X >= CaptureInterface.EdgeA.X).ToInt() * 16), (float)((CaptureInterface.EdgeB.Y >= CaptureInterface.EdgeA.Y).ToInt() * 16));
						vector7 += value2;
						vector6 = Vector2.Clamp(vector7, vector, vector2);
						num6 = (CaptureInterface.EdgeA.ToVector2() * 16f + new Vector2(16f) - value2 - vector6).ToRotation();
						if (vector6 != vector7)
						{
							num6 = (vector7 - vector6).ToRotation();
							specialMode2 = 1;
						}
						if (flag)
						{
							num6 *= -1f;
						}
					}
					Utils.DrawCursorSingle(sb, color2, num6 - 1.5707964f, Main.cursorScale * num, Main.ReverseGravitySupport(vector6 - Main.screenPosition, 0f), 5, specialMode2);
					return;
				}
				Point edgeB2 = CaptureInterface.EdgeB;
				if (CaptureInterface.EdgeAPinned)
				{
					int num7 = (CaptureInterface.EdgeB.X >= CaptureInterface.EdgeA.X).ToInt();
					int num8 = (CaptureInterface.EdgeB.Y >= CaptureInterface.EdgeA.Y).ToInt();
					edgeB2.X += num7;
					edgeB2.Y += num8;
					CaptureInterface.GetMapCoords(edgeB2.X, edgeB2.Y, 1, out edgeB2);
					Point edgeA2 = CaptureInterface.EdgeA;
					edgeA2.X += 1 - num7;
					edgeA2.Y += 1 - num8;
					CaptureInterface.GetMapCoords(edgeA2.X, edgeA2.Y, 1, out edgeA2);
					vector6 = edgeB2.ToVector2();
					vector6 = Vector2.Clamp(vector6, vector, vector2);
					num6 = (edgeA2.ToVector2() - vector6).ToRotation();
				}
				else
				{
					CaptureInterface.GetMapCoords(edgeB2.X, edgeB2.Y, 1, out edgeB2);
				}
				Utils.DrawCursorSingle(sb, color2, num6 - 1.5707964f, Main.cursorScale * num, edgeB2.ToVector2(), 5, 0);
			}
		}

		// Token: 0x0200079C RID: 1948
		private class ModeDragBounds : CaptureInterface.CaptureInterfaceMode
		{
			// Token: 0x06004185 RID: 16773 RVA: 0x006B9D40 File Offset: 0x006B7F40
			public override void Update()
			{
				if (!this.Selected)
				{
					return;
				}
				if (CaptureInterface.JustActivated)
				{
					return;
				}
				CaptureInterface.SetZoom_Context();
				Vector2 mouse = new Vector2((float)Main.mouseX, (float)Main.mouseY);
				this.DragBounds(mouse);
			}

			// Token: 0x06004186 RID: 16774 RVA: 0x006B9D7D File Offset: 0x006B7F7D
			public override void Draw(SpriteBatch sb)
			{
				if (!this.Selected)
				{
					return;
				}
				base.StartDrawingSelection(sb);
				this.DrawMarkedArea(sb);
				base.EndDrawingSelection(sb);
			}

			// Token: 0x06004187 RID: 16775 RVA: 0x006B9D9D File Offset: 0x006B7F9D
			public override void ToggleActive(bool tickedOn)
			{
				if (!tickedOn)
				{
					this.currentAim = -1;
				}
			}

			// Token: 0x06004188 RID: 16776 RVA: 0x006B9DA9 File Offset: 0x006B7FA9
			public override bool UsingMap()
			{
				return this.caughtEdge != -1;
			}

			// Token: 0x06004189 RID: 16777 RVA: 0x006B9DB8 File Offset: 0x006B7FB8
			private void DragBounds(Vector2 mouse)
			{
				if (!CaptureInterface.EdgeAPinned || !CaptureInterface.EdgeBPinned)
				{
					bool flag = false;
					if (Main.mouseLeft)
					{
						flag = true;
					}
					if (flag)
					{
						bool flag2 = true;
						Point point;
						if (!Main.mapFullscreen)
						{
							point = (Main.screenPosition + mouse).ToTileCoordinates();
						}
						else
						{
							flag2 = CaptureInterface.GetMapCoords((int)mouse.X, (int)mouse.Y, 0, out point);
						}
						if (flag2)
						{
							if (!CaptureInterface.EdgeAPinned)
							{
								CaptureInterface.EdgeAPinned = true;
								CaptureInterface.EdgeA = point;
							}
							if (!CaptureInterface.EdgeBPinned)
							{
								CaptureInterface.EdgeBPinned = true;
								CaptureInterface.EdgeB = point;
							}
						}
						this.currentAim = 3;
						this.caughtEdge = 1;
					}
				}
				int num = Math.Min(CaptureInterface.EdgeA.X, CaptureInterface.EdgeB.X);
				int num2 = Math.Min(CaptureInterface.EdgeA.Y, CaptureInterface.EdgeB.Y);
				int num3 = Math.Abs(CaptureInterface.EdgeA.X - CaptureInterface.EdgeB.X);
				int num4 = Math.Abs(CaptureInterface.EdgeA.Y - CaptureInterface.EdgeB.Y);
				bool value = Main.player[Main.myPlayer].gravDir == -1f;
				int num5 = 1 - value.ToInt();
				int num6 = value.ToInt();
				Rectangle rectangle;
				Rectangle rectangle2;
				if (!Main.mapFullscreen)
				{
					rectangle = Main.ReverseGravitySupport(new Rectangle(num * 16, num2 * 16, (num3 + 1) * 16, (num4 + 1) * 16));
					rectangle2 = Main.ReverseGravitySupport(new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth + 1, Main.screenHeight + 1));
					Rectangle rectangle3;
					Rectangle.Intersect(ref rectangle2, ref rectangle, out rectangle3);
					if (rectangle3.Width == 0 || rectangle3.Height == 0)
					{
						return;
					}
					rectangle3.Offset(-rectangle2.X, -rectangle2.Y);
				}
				else
				{
					Point point2;
					CaptureInterface.GetMapCoords(num, num2, 1, out point2);
					Point point3;
					CaptureInterface.GetMapCoords(num + num3 + 1, num2 + num4 + 1, 1, out point3);
					rectangle = new Rectangle(point2.X, point2.Y, point3.X - point2.X, point3.Y - point2.Y);
					rectangle2 = new Rectangle(0, 0, Main.screenWidth + 1, Main.screenHeight + 1);
					Rectangle rectangle3;
					Rectangle.Intersect(ref rectangle2, ref rectangle, out rectangle3);
					if (rectangle3.Width == 0 || rectangle3.Height == 0)
					{
						return;
					}
					rectangle3.Offset(-rectangle2.X, -rectangle2.Y);
				}
				this.dragging = false;
				if (!Main.mouseLeft)
				{
					this.currentAim = -1;
				}
				if (this.currentAim != -1)
				{
					this.dragging = true;
					Point point4 = default(Point);
					if (!Main.mapFullscreen)
					{
						point4 = Main.MouseWorld.ToTileCoordinates();
					}
					else
					{
						Point point5;
						if (!CaptureInterface.GetMapCoords((int)mouse.X, (int)mouse.Y, 0, out point5))
						{
							return;
						}
						point4 = point5;
					}
					int num7 = this.currentAim;
					if (num7 > 1)
					{
						if (num7 - 2 <= 1)
						{
							if (this.caughtEdge == 0)
							{
								CaptureInterface.EdgeA.X = point4.X;
							}
							if (this.caughtEdge == 1)
							{
								CaptureInterface.EdgeB.X = point4.X;
							}
						}
					}
					else
					{
						if (this.caughtEdge == 0)
						{
							CaptureInterface.EdgeA.Y = point4.Y;
						}
						if (this.caughtEdge == 1)
						{
							CaptureInterface.EdgeB.Y = point4.Y;
						}
					}
				}
				else
				{
					this.caughtEdge = -1;
					Rectangle drawbox = rectangle;
					drawbox.Offset(-rectangle2.X, -rectangle2.Y);
					this.inMap = drawbox.Contains(mouse.ToPoint());
					int i = 0;
					while (i < 4)
					{
						Rectangle bound = this.GetBound(drawbox, i);
						bound.Inflate(8, 8);
						if (bound.Contains(mouse.ToPoint()))
						{
							this.currentAim = i;
							if (i == 0)
							{
								if (CaptureInterface.EdgeA.Y < CaptureInterface.EdgeB.Y)
								{
									this.caughtEdge = num6;
									break;
								}
								this.caughtEdge = num5;
								break;
							}
							else if (i == 1)
							{
								if (CaptureInterface.EdgeA.Y >= CaptureInterface.EdgeB.Y)
								{
									this.caughtEdge = num6;
									break;
								}
								this.caughtEdge = num5;
								break;
							}
							else if (i == 2)
							{
								if (CaptureInterface.EdgeA.X < CaptureInterface.EdgeB.X)
								{
									this.caughtEdge = 0;
									break;
								}
								this.caughtEdge = 1;
								break;
							}
							else
							{
								if (i != 3)
								{
									break;
								}
								if (CaptureInterface.EdgeA.X >= CaptureInterface.EdgeB.X)
								{
									this.caughtEdge = 0;
									break;
								}
								this.caughtEdge = 1;
								break;
							}
						}
						else
						{
							i++;
						}
					}
				}
				CaptureInterface.ConstraintPoints();
			}

			// Token: 0x0600418A RID: 16778 RVA: 0x006BA250 File Offset: 0x006B8450
			private Rectangle GetBound(Rectangle drawbox, int boundIndex)
			{
				if (boundIndex == 0)
				{
					return new Rectangle(drawbox.X, drawbox.Y - 2, drawbox.Width, 2);
				}
				if (boundIndex == 1)
				{
					return new Rectangle(drawbox.X, drawbox.Y + drawbox.Height, drawbox.Width, 2);
				}
				if (boundIndex == 2)
				{
					return new Rectangle(drawbox.X - 2, drawbox.Y, 2, drawbox.Height);
				}
				if (boundIndex == 3)
				{
					return new Rectangle(drawbox.X + drawbox.Width, drawbox.Y, 2, drawbox.Height);
				}
				return Rectangle.Empty;
			}

			// Token: 0x0600418B RID: 16779 RVA: 0x006BA2E8 File Offset: 0x006B84E8
			public void DrawMarkedArea(SpriteBatch sb)
			{
				if (!CaptureInterface.EdgeAPinned || !CaptureInterface.EdgeBPinned)
				{
					return;
				}
				int num = Math.Min(CaptureInterface.EdgeA.X, CaptureInterface.EdgeB.X);
				int num2 = Math.Min(CaptureInterface.EdgeA.Y, CaptureInterface.EdgeB.Y);
				int num3 = Math.Abs(CaptureInterface.EdgeA.X - CaptureInterface.EdgeB.X);
				int num4 = Math.Abs(CaptureInterface.EdgeA.Y - CaptureInterface.EdgeB.Y);
				Rectangle rectangle3;
				if (!Main.mapFullscreen)
				{
					Rectangle rectangle = Main.ReverseGravitySupport(new Rectangle(num * 16, num2 * 16, (num3 + 1) * 16, (num4 + 1) * 16));
					Rectangle rectangle2 = Main.ReverseGravitySupport(new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth + 1, Main.screenHeight + 1));
					Rectangle.Intersect(ref rectangle2, ref rectangle, out rectangle3);
					if (rectangle3.Width == 0 || rectangle3.Height == 0)
					{
						return;
					}
					rectangle3.Offset(-rectangle2.X, -rectangle2.Y);
				}
				else
				{
					Point point;
					CaptureInterface.GetMapCoords(num, num2, 1, out point);
					Point point2;
					CaptureInterface.GetMapCoords(num + num3 + 1, num2 + num4 + 1, 1, out point2);
					Rectangle rectangle = new Rectangle(point.X, point.Y, point2.X - point.X, point2.Y - point.Y);
					Rectangle rectangle2 = new Rectangle(0, 0, Main.screenWidth + 1, Main.screenHeight + 1);
					Rectangle.Intersect(ref rectangle2, ref rectangle, out rectangle3);
					if (rectangle3.Width == 0 || rectangle3.Height == 0)
					{
						return;
					}
					rectangle3.Offset(-rectangle2.X, -rectangle2.Y);
				}
				sb.Draw(TextureAssets.MagicPixel.Value, rectangle3, CaptureInterface.Settings.MarkedAreaColor);
				Rectangle empty = Rectangle.Empty;
				for (int i = 0; i < 2; i++)
				{
					if (this.currentAim != i)
					{
						this.DrawBound(sb, new Rectangle(rectangle3.X, rectangle3.Y + ((i == 1) ? rectangle3.Height : -2), rectangle3.Width, 2), 0);
					}
					else
					{
						empty = new Rectangle(rectangle3.X, rectangle3.Y + ((i == 1) ? rectangle3.Height : -2), rectangle3.Width, 2);
					}
					if (this.currentAim != i + 2)
					{
						this.DrawBound(sb, new Rectangle(rectangle3.X + ((i == 1) ? rectangle3.Width : -2), rectangle3.Y, 2, rectangle3.Height), 0);
					}
					else
					{
						empty = new Rectangle(rectangle3.X + ((i == 1) ? rectangle3.Width : -2), rectangle3.Y, 2, rectangle3.Height);
					}
				}
				if (empty != Rectangle.Empty)
				{
					this.DrawBound(sb, empty, 1 + this.dragging.ToInt());
				}
			}

			// Token: 0x0600418C RID: 16780 RVA: 0x006BA5D8 File Offset: 0x006B87D8
			private void DrawBound(SpriteBatch sb, Rectangle r, int mode)
			{
				if (mode == 0)
				{
					sb.Draw(TextureAssets.MagicPixel.Value, r, Color.Silver);
					return;
				}
				if (mode == 1)
				{
					Rectangle destinationRectangle = new Rectangle(r.X - 2, r.Y, r.Width + 4, r.Height);
					sb.Draw(TextureAssets.MagicPixel.Value, destinationRectangle, Color.White);
					destinationRectangle = new Rectangle(r.X, r.Y - 2, r.Width, r.Height + 4);
					sb.Draw(TextureAssets.MagicPixel.Value, destinationRectangle, Color.White);
					sb.Draw(TextureAssets.MagicPixel.Value, r, Color.White);
					return;
				}
				if (mode == 2)
				{
					Rectangle destinationRectangle2 = new Rectangle(r.X - 2, r.Y, r.Width + 4, r.Height);
					sb.Draw(TextureAssets.MagicPixel.Value, destinationRectangle2, Color.Gold);
					destinationRectangle2 = new Rectangle(r.X, r.Y - 2, r.Width, r.Height + 4);
					sb.Draw(TextureAssets.MagicPixel.Value, destinationRectangle2, Color.Gold);
					sb.Draw(TextureAssets.MagicPixel.Value, r, Color.Gold);
				}
			}

			// Token: 0x04007039 RID: 28729
			public int currentAim = -1;

			// Token: 0x0400703A RID: 28730
			private bool dragging;

			// Token: 0x0400703B RID: 28731
			private int caughtEdge = -1;

			// Token: 0x0400703C RID: 28732
			private bool inMap;
		}

		// Token: 0x0200079D RID: 1949
		private class ModeChangeSettings : CaptureInterface.CaptureInterfaceMode
		{
			// Token: 0x0600418E RID: 16782 RVA: 0x006BA734 File Offset: 0x006B8934
			private Rectangle GetRect()
			{
				Rectangle rectangle = new Rectangle(0, 0, 224, 170);
				if (CaptureInterface.Settings.ScreenAnchor == 0)
				{
					rectangle.X = 227 - rectangle.Width / 2;
					rectangle.Y = 80;
				}
				return rectangle;
			}

			// Token: 0x0600418F RID: 16783 RVA: 0x006BA77C File Offset: 0x006B897C
			private void ButtonDraw(int button, ref string key, ref string value)
			{
				switch (button)
				{
				case 0:
					key = Lang.inter[74].Value;
					value = Lang.inter[73 - CaptureInterface.Settings.PackImage.ToInt()].Value;
					return;
				case 1:
					key = Lang.inter[75].Value;
					value = Lang.inter[73 - CaptureInterface.Settings.IncludeEntities.ToInt()].Value;
					return;
				case 2:
					key = Lang.inter[76].Value;
					value = Lang.inter[73 - (!CaptureInterface.Settings.TransparentBackground).ToInt()].Value;
					return;
				case 3:
				case 4:
				case 5:
					break;
				case 6:
					key = "      " + Lang.menu[86].Value;
					value = "";
					break;
				default:
					return;
				}
			}

			// Token: 0x06004190 RID: 16784 RVA: 0x006BA850 File Offset: 0x006B8A50
			private void PressButton(int button)
			{
				bool flag = false;
				switch (button)
				{
				case 0:
					CaptureInterface.Settings.PackImage = !CaptureInterface.Settings.PackImage;
					flag = true;
					break;
				case 1:
					CaptureInterface.Settings.IncludeEntities = !CaptureInterface.Settings.IncludeEntities;
					flag = true;
					break;
				case 2:
					CaptureInterface.Settings.TransparentBackground = !CaptureInterface.Settings.TransparentBackground;
					flag = true;
					break;
				case 6:
					CaptureInterface.Settings.PackImage = true;
					CaptureInterface.Settings.IncludeEntities = true;
					CaptureInterface.Settings.TransparentBackground = false;
					CaptureInterface.Settings.BiomeChoiceIndex = -1;
					flag = true;
					break;
				}
				if (flag)
				{
					SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
				}
			}

			// Token: 0x06004191 RID: 16785 RVA: 0x006BA8E8 File Offset: 0x006B8AE8
			private void DrawWaterChoices(SpriteBatch spritebatch, Point start, Point mouse)
			{
				Rectangle r = new Rectangle(0, 0, 20, 20);
				for (int i = 0; i < 2; i++)
				{
					for (int j = 0; j < 7; j++)
					{
						if (i != 1 || j != 6)
						{
							int num = j + i * 7;
							r.X = start.X + 24 * j + 12 * i;
							r.Y = start.Y + 24 * i;
							int num2 = num;
							int num3 = 0;
							if (r.Contains(mouse))
							{
								if (Main.mouseLeft && Main.mouseLeftRelease)
								{
									SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
									CaptureInterface.Settings.BiomeChoiceIndex = num2;
								}
								Main.instance.MouseText(Language.GetTextValue("CaptureBiomeChoice." + num2), 0, 0, -1, -1, -1, -1, 0);
								num3++;
							}
							if (CaptureInterface.Settings.BiomeChoiceIndex == num2)
							{
								num3 += 2;
							}
							Texture2D value = TextureAssets.Extra[130].Value;
							int x = num * 18;
							Color white = Color.White;
							float num4 = 1f;
							if (num3 < 2)
							{
								num4 *= 0.5f;
							}
							if (num3 % 2 == 1)
							{
								spritebatch.Draw(TextureAssets.MagicPixel.Value, r.TopLeft(), new Rectangle?(new Rectangle(0, 0, 1, 1)), Color.Gold, 0f, Vector2.Zero, new Vector2(20f), SpriteEffects.None, 0f);
							}
							else
							{
								spritebatch.Draw(TextureAssets.MagicPixel.Value, r.TopLeft(), new Rectangle?(new Rectangle(0, 0, 1, 1)), Color.White * num4, 0f, Vector2.Zero, new Vector2(20f), SpriteEffects.None, 0f);
							}
							spritebatch.Draw(value, r.TopLeft() + new Vector2(2f), new Rectangle?(new Rectangle(x, 0, 16, 16)), Color.White * num4);
						}
					}
				}
			}

			// Token: 0x06004192 RID: 16786 RVA: 0x006BAAD4 File Offset: 0x006B8CD4
			private int UnnecessaryBiomeSelectionTypeConversion(int index)
			{
				switch (index)
				{
				case 0:
					return -1;
				case 1:
					return 0;
				case 2:
					return 2;
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
					return index;
				case 9:
					return 10;
				case 10:
					return 12;
				case 11:
					return 13;
				case 12:
					return 14;
				default:
					return 0;
				}
			}

			// Token: 0x06004193 RID: 16787 RVA: 0x006BAB34 File Offset: 0x006B8D34
			public override void Update()
			{
				if (!this.Selected)
				{
					return;
				}
				if (CaptureInterface.JustActivated)
				{
					return;
				}
				PlayerInput.SetZoom_UI();
				Point value = new Point(Main.mouseX, Main.mouseY);
				this.hoveredButton = -1;
				Rectangle rect = this.GetRect();
				this.inUI = rect.Contains(value);
				rect.Inflate(-20, -20);
				rect.Height = 16;
				int y = rect.Y;
				for (int i = 0; i < 7; i++)
				{
					rect.Y = y + i * 20;
					if (rect.Contains(value))
					{
						this.hoveredButton = i;
						break;
					}
				}
				if (Main.mouseLeft && Main.mouseLeftRelease && this.hoveredButton != -1)
				{
					this.PressButton(this.hoveredButton);
				}
			}

			// Token: 0x06004194 RID: 16788 RVA: 0x006BABF0 File Offset: 0x006B8DF0
			public override void Draw(SpriteBatch sb)
			{
				if (!this.Selected)
				{
					return;
				}
				base.StartDrawingSelection(sb);
				((CaptureInterface.ModeDragBounds)CaptureInterface.Modes[1]).currentAim = -1;
				((CaptureInterface.ModeDragBounds)CaptureInterface.Modes[1]).DrawMarkedArea(sb);
				base.EndDrawingSelection(sb);
				Rectangle rect = this.GetRect();
				Utils.DrawInvBG(sb, rect, new Color(63, 65, 151, 255) * 0.485f);
				for (int i = 0; i < 7; i++)
				{
					string text = "";
					string text2 = "";
					this.ButtonDraw(i, ref text, ref text2);
					Color baseColor = Color.White;
					if (i == this.hoveredButton)
					{
						baseColor = Color.Gold;
					}
					ChatManager.DrawColorCodedStringWithShadow(sb, FontAssets.ItemStack.Value, text, rect.TopLeft() + new Vector2(20f, (float)(20 + 20 * i)), baseColor, 0f, Vector2.Zero, Vector2.One, -1f, 2f);
					ChatManager.DrawColorCodedStringWithShadow(sb, FontAssets.ItemStack.Value, text2, rect.TopRight() + new Vector2(-20f, (float)(20 + 20 * i)), baseColor, 0f, FontAssets.ItemStack.Value.MeasureString(text2) * Vector2.UnitX, Vector2.One, -1f, 2f);
				}
				this.DrawWaterChoices(sb, (rect.TopLeft() + new Vector2((float)(rect.Width / 2 - 84), 90f)).ToPoint(), Main.MouseScreen.ToPoint());
			}

			// Token: 0x06004195 RID: 16789 RVA: 0x006BAD89 File Offset: 0x006B8F89
			public override void ToggleActive(bool tickedOn)
			{
				if (tickedOn)
				{
					this.hoveredButton = -1;
				}
			}

			// Token: 0x06004196 RID: 16790 RVA: 0x006BAD95 File Offset: 0x006B8F95
			public override bool UsingMap()
			{
				return this.inUI;
			}

			// Token: 0x0400703D RID: 28733
			private const int ButtonsCount = 7;

			// Token: 0x0400703E RID: 28734
			private int hoveredButton = -1;

			// Token: 0x0400703F RID: 28735
			private bool inUI;
		}
	}
}
