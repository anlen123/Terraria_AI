using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Threading;
using Terraria.DataStructures;
using Terraria.Graphics.Capture;
using Terraria.Utilities;

namespace Terraria.Graphics.Light
{
	// Token: 0x020001FC RID: 508
	public class LegacyLighting : ILightingEngine
	{
		// Token: 0x1700032E RID: 814
		// (get) Token: 0x060020D2 RID: 8402 RVA: 0x0052828D File Offset: 0x0052648D
		// (set) Token: 0x060020D3 RID: 8403 RVA: 0x00528295 File Offset: 0x00526495
		public int Mode { get; set; }

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x060020D4 RID: 8404 RVA: 0x0052829E File Offset: 0x0052649E
		public bool IsColorOrWhiteMode
		{
			get
			{
				return this.Mode < 2;
			}
		}

		// Token: 0x060020D5 RID: 8405 RVA: 0x005282AC File Offset: 0x005264AC
		public LegacyLighting(Camera camera)
		{
			this._camera = camera;
		}

		// Token: 0x060020D6 RID: 8406 RVA: 0x00528360 File Offset: 0x00526560
		public Vector3 GetColor(int x, int y)
		{
			if (x < this._expandedRectLeft || x >= this._expandedRectRight || y < this._expandedRectTop || y >= this._expandedRectBottom)
			{
				return Vector3.Zero;
			}
			LegacyLighting.LightingState lightingState = this._states[x - this._expandedRectLeft][y - this._expandedRectTop];
			return new Vector3(lightingState.R, lightingState.G, lightingState.B);
		}

		// Token: 0x060020D7 RID: 8407 RVA: 0x005283C8 File Offset: 0x005265C8
		public void Rebuild()
		{
			if (!CaptureManager.Instance.IsCapturing)
			{
				this._tempLights = new Dictionary<Point16, LegacyLighting.ColorTriplet>();
			}
			this._swipe = new LegacyLighting.LightingSwipeData();
			this._threadSwipes = new LegacyLighting.LightingSwipeData[Environment.ProcessorCount];
			for (int i = 0; i < this._threadSwipes.Length; i++)
			{
				this._threadSwipes[i] = new LegacyLighting.LightingSwipeData();
			}
			int num = (int)this._camera.UnscaledSize.X / 16 + 90 + 10;
			int num2 = (int)this._camera.UnscaledSize.Y / 16 + 90 + 10;
			this._lightMap.SetSize(num, num2);
			if (this._states == null || this._states.Length < num || this._states[0].Length < num2)
			{
				this._states = new LegacyLighting.LightingState[num][];
				this._axisFlipStates = new LegacyLighting.LightingState[num2][];
				for (int j = 0; j < num2; j++)
				{
					this._axisFlipStates[j] = new LegacyLighting.LightingState[num];
				}
				for (int k = 0; k < num; k++)
				{
					LegacyLighting.LightingState[] array = new LegacyLighting.LightingState[num2];
					for (int l = 0; l < num2; l++)
					{
						LegacyLighting.LightingState lightingState = new LegacyLighting.LightingState();
						array[l] = lightingState;
						this._axisFlipStates[l][k] = lightingState;
					}
					this._states[k] = array;
				}
			}
		}

		// Token: 0x060020D8 RID: 8408 RVA: 0x00528510 File Offset: 0x00526710
		public void AddLight(int x, int y, Vector3 color)
		{
			float x2 = color.X;
			float y2 = color.Y;
			float z = color.Z;
			if (x - this._requestedRectLeft + Lighting.OffScreenTiles >= 0 && (float)(x - this._requestedRectLeft + Lighting.OffScreenTiles) < this._camera.UnscaledSize.X / 16f + (float)(Lighting.OffScreenTiles * 2) + 10f && y - this._requestedRectTop + Lighting.OffScreenTiles >= 0 && (float)(y - this._requestedRectTop + Lighting.OffScreenTiles) < this._camera.UnscaledSize.Y / 16f + (float)(Lighting.OffScreenTiles * 2) + 10f)
			{
				if (this._tempLights.Count == 2000)
				{
					return;
				}
				Point16 key = new Point16(x, y);
				LegacyLighting.ColorTriplet colorTriplet;
				if (this._tempLights.TryGetValue(key, out colorTriplet))
				{
					if (this._rgb)
					{
						if (colorTriplet.R < x2)
						{
							colorTriplet.R = x2;
						}
						if (colorTriplet.G < y2)
						{
							colorTriplet.G = y2;
						}
						if (colorTriplet.B < z)
						{
							colorTriplet.B = z;
						}
						this._tempLights[key] = colorTriplet;
						return;
					}
					float num = (x2 + y2 + z) / 3f;
					if (colorTriplet.R < num)
					{
						this._tempLights[key] = new LegacyLighting.ColorTriplet(num);
						return;
					}
				}
				else
				{
					if (this._rgb)
					{
						colorTriplet = new LegacyLighting.ColorTriplet(x2, y2, z);
					}
					else
					{
						colorTriplet = new LegacyLighting.ColorTriplet((x2 + y2 + z) / 3f);
					}
					this._tempLights.Add(key, colorTriplet);
				}
			}
		}

		// Token: 0x060020D9 RID: 8409 RVA: 0x005286A8 File Offset: 0x005268A8
		public void ProcessArea(Rectangle area)
		{
			this._oldSkyColor = this._skyColor;
			float num = (float)Main.tileColor.R / 255f;
			float num2 = (float)Main.tileColor.G / 255f;
			float num3 = (float)Main.tileColor.B / 255f;
			this._skyColor = (num + num2 + num3) / 3f;
			if (this.IsColorOrWhiteMode)
			{
				this._offScreenTiles2 = 34;
				Lighting.OffScreenTiles = 40;
			}
			else
			{
				this._offScreenTiles2 = 18;
				Lighting.OffScreenTiles = 23;
			}
			this._requestedRectLeft = area.Left;
			this._requestedRectRight = area.Right;
			this._requestedRectTop = area.Top;
			this._requestedRectBottom = area.Bottom;
			this._expandedRectLeft = this._requestedRectLeft - Lighting.OffScreenTiles;
			this._expandedRectTop = this._requestedRectTop - Lighting.OffScreenTiles;
			this._expandedRectRight = this._requestedRectRight + Lighting.OffScreenTiles;
			this._expandedRectBottom = this._requestedRectBottom + Lighting.OffScreenTiles;
			Main.renderCount++;
			int maxLightArrayX = (int)this._camera.UnscaledSize.X / 16 + Lighting.OffScreenTiles * 2;
			int maxLightArrayY = (int)this._camera.UnscaledSize.Y / 16 + Lighting.OffScreenTiles * 2;
			if (Main.renderCount < 3)
			{
				this.DoColors();
			}
			if (Main.renderCount == 2)
			{
				this.CopyFullyProcessedDataOver(maxLightArrayX, maxLightArrayY);
			}
			else if (this._lastCameraPosition != null)
			{
				this.ShiftUnProcessedDataOver(maxLightArrayX, maxLightArrayY);
				if (Netplay.Connection.StatusMax > 0)
				{
					Main.mapTime = 1;
				}
				if (Main.mapDelay > 0)
				{
					Main.mapDelay--;
				}
				else if (Main.mapTime == 0 && Main.mapEnabled && Main.renderCount == 3)
				{
					try
					{
						this.TryUpdatingMapWithLight();
					}
					catch
					{
					}
				}
				if (this._oldSkyColor != this._skyColor)
				{
					this.UpdateLightToSkyColor(num, num2, num3);
				}
			}
			if (Main.renderCount > LegacyLighting.RenderPhases)
			{
				this.PreRenderPhase();
			}
			this._lastCameraPosition = new Vector2?(Main.Camera.UnscaledPosition);
		}

		// Token: 0x060020DA RID: 8410 RVA: 0x005288C4 File Offset: 0x00526AC4
		private void TryUpdatingMapWithLight()
		{
			Main.mapTime = Main.mapTimeMax;
			int num = 40;
			Vector2 unscaledPosition = this._camera.UnscaledPosition;
			int num2 = (int)this._camera.ScaledSize.X;
			int num3 = (int)this._camera.ScaledSize.Y;
			Vector2 vector = unscaledPosition + Main.GameViewMatrix.Translation;
			int num4 = (int)Math.Floor((double)(vector.X / 16f));
			int num5 = (int)Math.Floor((double)((vector.X + (float)num2) / 16f)) + 1;
			int num6 = (int)Math.Floor((double)(vector.Y / 16f));
			int num7 = (int)Math.Floor((double)((vector.Y + (float)num3) / 16f)) + 1;
			num4 = Utils.Clamp<int>(num4, Lighting.OffScreenTiles, Main.maxTilesX - Lighting.OffScreenTiles);
			num5 = Utils.Clamp<int>(num5, Lighting.OffScreenTiles, Main.maxTilesX - Lighting.OffScreenTiles);
			num6 = Utils.Clamp<int>(num6, Lighting.OffScreenTiles, Main.maxTilesY - Lighting.OffScreenTiles);
			num7 = Utils.Clamp<int>(num7, Lighting.OffScreenTiles, Main.maxTilesY - Lighting.OffScreenTiles);
			int num8 = Utils.Clamp<int>(this._requestedRectLeft, num, Main.maxTilesX - num);
			int num9 = Utils.Clamp<int>(this._requestedRectRight, num, Main.maxTilesX - num);
			int num10 = Utils.Clamp<int>(this._requestedRectTop, num, Main.maxTilesY - num);
			int num11 = Utils.Clamp<int>(this._requestedRectBottom, num, Main.maxTilesY - num);
			num8 = Utils.Clamp<int>(num8, num4, num5);
			num9 = Utils.Clamp<int>(num9, num4, num5);
			num10 = Utils.Clamp<int>(num10, num6, num7);
			num11 = Utils.Clamp<int>(num11, num6, num7);
			int offScreenTiles = Lighting.OffScreenTiles;
			for (int i = num8; i < num9; i++)
			{
				LegacyLighting.LightingState[] array = this._states[i - this._requestedRectLeft + offScreenTiles];
				for (int j = num10; j < num11; j++)
				{
					LegacyLighting.LightingState lightingState = array[j - this._requestedRectTop + offScreenTiles];
					Tile tile = Main.tile[i, j];
					float num12 = 0f;
					if (lightingState.R > num12)
					{
						num12 = lightingState.R;
					}
					if (lightingState.G > num12)
					{
						num12 = lightingState.G;
					}
					if (lightingState.B > num12)
					{
						num12 = lightingState.B;
					}
					if (this.IsColorOrWhiteMode)
					{
						num12 *= 1.5f;
					}
					byte b = (byte)Math.Min(255f, num12 * 255f);
					if ((double)j < Main.worldSurface && !tile.active() && tile.wall == 0 && tile.liquid == 0)
					{
						b = 22;
					}
					if ((b > 18 || Main.Map[i, j].Light > 0) && b < 22)
					{
						b = 22;
					}
					Main.Map.UpdateLighting(i, j, b);
				}
			}
			Main.updateMap = new Rectangle?(new Rectangle(num8, num10, num9 - num8, num11 - num10));
		}

		// Token: 0x060020DB RID: 8411 RVA: 0x00528BB4 File Offset: 0x00526DB4
		private void UpdateLightToSkyColor(float tileR, float tileG, float tileB)
		{
			int num = Utils.Clamp<int>(this._expandedRectLeft, 0, Main.maxTilesX - 1);
			int num2 = Utils.Clamp<int>(this._expandedRectRight, 0, Main.maxTilesX - 1);
			int num3 = Utils.Clamp<int>(this._expandedRectTop, 0, Main.maxTilesY - 1);
			int num4 = Utils.Clamp<int>(this._expandedRectBottom, 0, (int)Main.worldSurface - 1);
			if ((double)num3 < Main.worldSurface)
			{
				for (int i = num; i < num2; i++)
				{
					LegacyLighting.LightingState[] array = this._states[i - this._expandedRectLeft];
					for (int j = num3; j < num4; j++)
					{
						LegacyLighting.LightingState lightingState = array[j - this._expandedRectTop];
						Tile tile = Main.tile[i, j];
						if (tile == null)
						{
							tile = new Tile();
							Main.tile[i, j] = tile;
						}
						if ((!tile.active() || !Main.tileNoSunLight[(int)tile.type]) && lightingState.R < this._skyColor && tile.liquid < 200 && (Main.wallLight[(int)tile.wall] || tile.wall == 73))
						{
							lightingState.R = tileR;
							if (lightingState.G < this._skyColor)
							{
								lightingState.G = tileG;
							}
							if (lightingState.B < this._skyColor)
							{
								lightingState.B = tileB;
							}
						}
					}
				}
			}
		}

		// Token: 0x060020DC RID: 8412 RVA: 0x00528D1C File Offset: 0x00526F1C
		private void ShiftUnProcessedDataOver(int maxLightArrayX, int maxLightArrayY)
		{
			Vector2 value = this._lastCameraPosition.Value;
			Vector2 unscaledPosition = this._camera.UnscaledPosition;
			int num = (int)Math.Floor((double)(unscaledPosition.X / 16f)) - (int)Math.Floor((double)(value.X / 16f));
			if (num > 5 || num < -5)
			{
				num = 0;
			}
			int num2;
			int num3;
			int num4;
			if (num < 0)
			{
				num2 = -1;
				num *= -1;
				num3 = maxLightArrayX;
				num4 = num;
			}
			else
			{
				num2 = 1;
				num3 = 0;
				num4 = maxLightArrayX - num;
			}
			int num5 = (int)Math.Floor((double)(unscaledPosition.Y / 16f)) - (int)Math.Floor((double)(value.Y / 16f));
			if (num5 > 5 || num5 < -5)
			{
				num5 = 0;
			}
			int num6;
			int num7;
			int num8;
			if (num5 < 0)
			{
				num6 = -1;
				num5 *= -1;
				num7 = maxLightArrayY;
				num8 = num5;
			}
			else
			{
				num6 = 1;
				num7 = 0;
				num8 = maxLightArrayY - num5;
			}
			if (num != 0 || num5 != 0)
			{
				for (int num9 = num3; num9 != num4; num9 += num2)
				{
					LegacyLighting.LightingState[] array = this._states[num9];
					LegacyLighting.LightingState[] array2 = this._states[num9 + num * num2];
					for (int num10 = num7; num10 != num8; num10 += num6)
					{
						LegacyLighting.LightingState lightingState = array[num10];
						LegacyLighting.LightingState lightingState2 = array2[num10 + num5 * num6];
						lightingState.R = lightingState2.R;
						lightingState.G = lightingState2.G;
						lightingState.B = lightingState2.B;
					}
				}
			}
		}

		// Token: 0x060020DD RID: 8413 RVA: 0x00528E6C File Offset: 0x0052706C
		private void CopyFullyProcessedDataOver(int maxLightArrayX, int maxLightArrayY)
		{
			Vector2 unscaledPosition = this._camera.UnscaledPosition;
			int num = (int)Math.Floor((double)(unscaledPosition.X / 16f)) - this._scrX;
			int num2 = (int)Math.Floor((double)(unscaledPosition.Y / 16f)) - this._scrY;
			if (num > 16)
			{
				num = 0;
			}
			if (num2 > 16)
			{
				num2 = 0;
			}
			int num3 = 0;
			int num4 = maxLightArrayX;
			int num5 = 0;
			int num6 = maxLightArrayY;
			if (num < 0)
			{
				num3 -= num;
			}
			else
			{
				num4 -= num;
			}
			if (num2 < 0)
			{
				num5 -= num2;
			}
			else
			{
				num6 -= num2;
			}
			if (this._rgb)
			{
				int num7 = maxLightArrayX;
				if (this._states.Length <= num7 + num)
				{
					num7 = this._states.Length - num - 1;
				}
				for (int i = num3; i < num7; i++)
				{
					LegacyLighting.LightingState[] array = this._states[i];
					LegacyLighting.LightingState[] array2 = this._states[i + num];
					int num8 = num6;
					if (array2.Length <= num8 + num)
					{
						num8 = array2.Length - num2 - 1;
					}
					for (int j = num5; j < num8; j++)
					{
						LegacyLighting.LightingState lightingState = array[j];
						LegacyLighting.LightingState lightingState2 = array2[j + num2];
						lightingState.R = lightingState2.R2;
						lightingState.G = lightingState2.G2;
						lightingState.B = lightingState2.B2;
					}
				}
				return;
			}
			int num9 = num4;
			if (this._states.Length <= num9 + num)
			{
				num9 = this._states.Length - num - 1;
			}
			for (int k = num3; k < num9; k++)
			{
				LegacyLighting.LightingState[] array3 = this._states[k];
				LegacyLighting.LightingState[] array4 = this._states[k + num];
				int num10 = num6;
				if (array4.Length <= num10 + num)
				{
					num10 = array4.Length - num2 - 1;
				}
				for (int l = num5; l < num10; l++)
				{
					LegacyLighting.LightingState lightingState3 = array3[l];
					LegacyLighting.LightingState lightingState4 = array4[l + num2];
					lightingState3.R = lightingState4.R2;
					lightingState3.G = lightingState4.R2;
					lightingState3.B = lightingState4.R2;
				}
			}
		}

		// Token: 0x060020DE RID: 8414 RVA: 0x00529050 File Offset: 0x00527250
		public void Clear()
		{
			this._lastCameraPosition = null;
			int num = (int)this._camera.UnscaledSize.X / 16 + Lighting.OffScreenTiles * 2;
			int num2 = (int)this._camera.UnscaledSize.Y / 16 + Lighting.OffScreenTiles * 2;
			for (int i = 0; i < num; i++)
			{
				if (i < this._states.Length)
				{
					LegacyLighting.LightingState[] array = this._states[i];
					for (int j = 0; j < num2; j++)
					{
						if (j < array.Length)
						{
							LegacyLighting.LightingState lightingState = array[j];
							lightingState.R = 0f;
							lightingState.G = 0f;
							lightingState.B = 0f;
						}
					}
				}
			}
		}

		// Token: 0x060020DF RID: 8415 RVA: 0x00529100 File Offset: 0x00527300
		private void PreRenderPhase()
		{
			float num = (float)Main.tileColor.R / 255f;
			float num2 = (float)Main.tileColor.G / 255f;
			float num3 = (float)Main.tileColor.B / 255f;
			TimeLogger.StartTimestamp fromTimestamp = TimeLogger.Start();
			int num4 = 0;
			int num5 = (int)this._camera.UnscaledSize.X / 16 + Lighting.OffScreenTiles * 2 + 10;
			int num6 = 0;
			int num7 = (int)this._camera.UnscaledSize.Y / 16 + Lighting.OffScreenTiles * 2 + 10;
			this._minX = num5;
			this._maxX = num4;
			this._minY = num7;
			this._maxY = num6;
			this._rgb = (this.Mode == 0 || this.Mode == 3);
			for (int i = num4; i < num5; i++)
			{
				LegacyLighting.LightingState[] array = this._states[i];
				for (int j = num6; j < num7; j++)
				{
					LegacyLighting.LightingState lightingState = array[j];
					lightingState.R2 = 0f;
					lightingState.G2 = 0f;
					lightingState.B2 = 0f;
					lightingState.CrackedLight = false;
					lightingState.StopLight = false;
					lightingState.WetLight = false;
					lightingState.HoneyLight = false;
				}
			}
			if (Main.wofNPCIndex >= 0 && Main.player[Main.myPlayer].gross)
			{
				try
				{
					int num8 = (int)this._camera.UnscaledPosition.Y / 16 - 10;
					int num9 = (int)(this._camera.UnscaledPosition.Y + this._camera.UnscaledSize.Y) / 16 + 10;
					int num10 = (int)Main.npc[Main.wofNPCIndex].position.X / 16;
					if (Main.npc[Main.wofNPCIndex].direction > 0)
					{
						num10 -= 3;
					}
					else
					{
						num10 += 2;
					}
					int num11 = num10 + 8;
					float num12 = 0.5f * Main.demonTorch + 1f * (1f - Main.demonTorch);
					float num13 = 0.3f;
					float num14 = 1f * Main.demonTorch + 0.5f * (1f - Main.demonTorch);
					num12 *= 0.2f;
					num13 *= 0.1f;
					num14 *= 0.3f;
					for (int k = num10; k <= num11; k++)
					{
						LegacyLighting.LightingState[] array2 = this._states[k - num10];
						for (int l = num8; l <= num9; l++)
						{
							LegacyLighting.LightingState lightingState2 = array2[l - this._expandedRectTop];
							if (lightingState2.R2 < num12)
							{
								lightingState2.R2 = num12;
							}
							if (lightingState2.G2 < num13)
							{
								lightingState2.G2 = num13;
							}
							if (lightingState2.B2 < num14)
							{
								lightingState2.B2 = num14;
							}
						}
					}
				}
				catch
				{
				}
			}
			num4 = Utils.Clamp<int>(this._expandedRectLeft, 5, Main.maxTilesX - 1);
			num5 = Utils.Clamp<int>(this._expandedRectRight, 5, Main.maxTilesX - 1);
			num6 = Utils.Clamp<int>(this._expandedRectTop, 5, Main.maxTilesY - 1);
			num7 = Utils.Clamp<int>(this._expandedRectBottom, 5, Main.maxTilesY - 1);
			Main.UpdateSceneMetrics();
			this._tileScanner.Update();
			this._tileScanner.ExportTo(new Rectangle(num4, num6, num5 - num4, num7 - num6), this._lightMap, new TileLightScannerOptions
			{
				DrawInvisibleWalls = Main.ShouldShowInvisibleBlocksAndWalls()
			});
			for (int m = num4; m < num5; m++)
			{
				LegacyLighting.LightingState[] array3 = this._states[m - this._expandedRectLeft];
				for (int n = num6; n < num7; n++)
				{
					LegacyLighting.LightingState lightingState3 = array3[n - this._expandedRectTop];
					if (Main.tile[m, n] == null)
					{
						Tile tile = new Tile();
						Main.tile[m, n] = tile;
					}
					Vector3 vector;
					this._lightMap.GetLight(m - num4, n - num6, out vector);
					if (this._rgb)
					{
						lightingState3.R2 = vector.X;
						lightingState3.G2 = vector.Y;
						lightingState3.B2 = vector.Z;
					}
					else
					{
						float num15 = (vector.X + vector.Y + vector.Z) / 3f;
						lightingState3.R2 = num15;
						lightingState3.G2 = num15;
						lightingState3.B2 = num15;
					}
					switch (this._lightMap.GetMask(m - num4, n - num6))
					{
					case LightMaskMode.Solid:
						lightingState3.StopLight = true;
						break;
					case LightMaskMode.Water:
						lightingState3.WetLight = true;
						break;
					case LightMaskMode.Honey:
						lightingState3.WetLight = true;
						lightingState3.HoneyLight = true;
						break;
					case LightMaskMode.CrackedBricks:
						lightingState3.CrackedLight = true;
						break;
					}
					if (lightingState3.R2 > 0f || (this._rgb && (lightingState3.G2 > 0f || lightingState3.B2 > 0f)))
					{
						int num16 = m - this._expandedRectLeft;
						int num17 = n - this._expandedRectTop;
						if (this._minX > num16)
						{
							this._minX = num16;
						}
						if (this._maxX < num16 + 1)
						{
							this._maxX = num16 + 1;
						}
						if (this._minY > num17)
						{
							this._minY = num17;
						}
						if (this._maxY < num17 + 1)
						{
							this._maxY = num17 + 1;
						}
					}
				}
			}
			foreach (KeyValuePair<Point16, LegacyLighting.ColorTriplet> keyValuePair in this._tempLights)
			{
				int num18 = (int)keyValuePair.Key.X - this._requestedRectLeft + Lighting.OffScreenTiles;
				int num19 = (int)keyValuePair.Key.Y - this._requestedRectTop + Lighting.OffScreenTiles;
				if (num18 >= 0 && (float)num18 < this._camera.UnscaledSize.X / 16f + (float)(Lighting.OffScreenTiles * 2) + 10f && num19 >= 0 && (float)num19 < this._camera.UnscaledSize.Y / 16f + (float)(Lighting.OffScreenTiles * 2) + 10f)
				{
					LegacyLighting.LightingState lightingState4 = this._states[num18][num19];
					if (lightingState4.R2 < keyValuePair.Value.R)
					{
						lightingState4.R2 = keyValuePair.Value.R;
					}
					if (lightingState4.G2 < keyValuePair.Value.G)
					{
						lightingState4.G2 = keyValuePair.Value.G;
					}
					if (lightingState4.B2 < keyValuePair.Value.B)
					{
						lightingState4.B2 = keyValuePair.Value.B;
					}
					if (this._minX > num18)
					{
						this._minX = num18;
					}
					if (this._maxX < num18 + 1)
					{
						this._maxX = num18 + 1;
					}
					if (this._minY > num19)
					{
						this._minY = num19;
					}
					if (this._maxY < num19 + 1)
					{
						this._maxY = num19 + 1;
					}
				}
			}
			if (!Main.gamePaused && !CaptureManager.Instance.IsCapturing)
			{
				this._tempLights.Clear();
			}
			this._minX += this._expandedRectLeft;
			this._maxX += this._expandedRectLeft;
			this._minY += this._expandedRectTop;
			this._maxY += this._expandedRectTop;
			this._minBoundArea.Set(this._minX, this._maxX, this._minY, this._maxY);
			this._requestedArea.Set(this._requestedRectLeft, this._requestedRectRight, this._requestedRectTop, this._requestedRectBottom);
			this._expandedArea.Set(this._expandedRectLeft, this._expandedRectRight, this._expandedRectTop, this._expandedRectBottom);
			this._offScreenTiles2ExpandedArea.Set(this._requestedRectLeft - this._offScreenTiles2, this._requestedRectRight + this._offScreenTiles2, this._requestedRectTop - this._offScreenTiles2, this._requestedRectBottom + this._offScreenTiles2);
			this._scrX = (int)Math.Floor((double)(this._camera.UnscaledPosition.X / 16f));
			this._scrY = (int)Math.Floor((double)(this._camera.UnscaledPosition.Y / 16f));
			Main.renderCount = 0;
			TimeLogger.LightingInit.AddTime(fromTimestamp);
			this.DoColors();
		}

		// Token: 0x060020E0 RID: 8416 RVA: 0x00529998 File Offset: 0x00527B98
		private void DoColors()
		{
			if (this.IsColorOrWhiteMode)
			{
				this._blueWave += (float)this._blueDir * 0.0001f;
				if (this._blueWave > 1f)
				{
					this._blueWave = 1f;
					this._blueDir = -1;
				}
				else if (this._blueWave < 0.97f)
				{
					this._blueWave = 0.97f;
					this._blueDir = 1;
				}
				if (this._rgb)
				{
					this._negLight = 0.91f;
					this._negLight2 = 0.56f;
					this._negLight3 = 0.8f;
					this._honeyLightG = 0.7f * this._negLight * this._blueWave;
					this._honeyLightR = 0.75f * this._negLight * this._blueWave;
					this._honeyLightB = 0.6f * this._negLight * this._blueWave;
					switch (Main.waterStyle)
					{
					case 0:
					case 1:
					case 7:
					case 8:
						this._wetLightG = 0.96f * this._negLight * this._blueWave;
						this._wetLightR = 0.88f * this._negLight * this._blueWave;
						this._wetLightB = 1.015f * this._negLight * this._blueWave;
						goto IL_44D;
					case 2:
						this._wetLightG = 0.85f * this._negLight * this._blueWave;
						this._wetLightR = 0.94f * this._negLight * this._blueWave;
						this._wetLightB = 1.01f * this._negLight * this._blueWave;
						goto IL_44D;
					case 3:
						this._wetLightG = 0.95f * this._negLight * this._blueWave;
						this._wetLightR = 0.84f * this._negLight * this._blueWave;
						this._wetLightB = 1.015f * this._negLight * this._blueWave;
						goto IL_44D;
					case 4:
						this._wetLightG = 0.86f * this._negLight * this._blueWave;
						this._wetLightR = 0.9f * this._negLight * this._blueWave;
						this._wetLightB = 1.01f * this._negLight * this._blueWave;
						goto IL_44D;
					case 5:
						this._wetLightG = 0.99f * this._negLight * this._blueWave;
						this._wetLightR = 0.84f * this._negLight * this._blueWave;
						this._wetLightB = 1.01f * this._negLight * this._blueWave;
						goto IL_44D;
					case 6:
						this._wetLightR = 0.83f * this._negLight * this._blueWave;
						this._wetLightG = 0.93f * this._negLight * this._blueWave;
						this._wetLightB = 0.98f * this._negLight * this._blueWave;
						goto IL_44D;
					case 9:
						this._wetLightG = 0.88f * this._negLight * this._blueWave;
						this._wetLightR = 1f * this._negLight * this._blueWave;
						this._wetLightB = 0.84f * this._negLight * this._blueWave;
						goto IL_44D;
					case 10:
						this._wetLightG = 1f * this._negLight * this._blueWave;
						this._wetLightR = 0.83f * this._negLight * this._blueWave;
						this._wetLightB = 1f * this._negLight * this._blueWave;
						goto IL_44D;
					case 12:
						this._wetLightG = 0.98f * this._negLight * this._blueWave;
						this._wetLightR = 0.95f * this._negLight * this._blueWave;
						this._wetLightB = 0.85f * this._negLight * this._blueWave;
						goto IL_44D;
					}
					this._wetLightG = 0f;
					this._wetLightR = 0f;
					this._wetLightB = 0f;
				}
				else
				{
					this._negLight = 0.9f;
					this._negLight2 = 0.54f;
					this._negLight3 = 0.8f;
					this._wetLightR = 0.95f * this._negLight * this._blueWave;
				}
				IL_44D:
				if (Main.player[Main.myPlayer].nightVision)
				{
					this._negLight *= 1.03f;
					this._negLight2 *= 1.03f;
					this._negLight3 *= 1.03f;
				}
				if (Main.player[Main.myPlayer].blind)
				{
					this._negLight *= 0.95f;
					this._negLight2 *= 0.95f;
					this._negLight3 *= 0.95f;
				}
				if (Main.player[Main.myPlayer].blackout)
				{
					this._negLight *= 0.85f;
					this._negLight2 *= 0.85f;
					this._negLight3 *= 0.85f;
				}
				if (Main.player[Main.myPlayer].headcovered)
				{
					this._negLight *= 0.85f;
					this._negLight2 *= 0.85f;
					this._negLight3 *= 0.85f;
				}
			}
			else
			{
				this._negLight = 0.04f;
				this._negLight2 = 0.16f;
				this._negLight3 = 0.08f;
				if (Main.player[Main.myPlayer].nightVision)
				{
					this._negLight -= 0.013f;
					this._negLight2 -= 0.04f;
					this._negLight3 -= 0.04f;
				}
				if (Main.player[Main.myPlayer].blind)
				{
					this._negLight += 0.03f;
					this._negLight2 += 0.06f;
					this._negLight3 += 0.06f;
				}
				if (Main.player[Main.myPlayer].blackout)
				{
					this._negLight += 0.09f;
					this._negLight2 += 0.18f;
					this._negLight3 += 0.18f;
				}
				if (Main.player[Main.myPlayer].headcovered)
				{
					this._negLight += 0.09f;
					this._negLight2 += 0.18f;
					this._negLight3 += 0.18f;
				}
				this._wetLightR = this._negLight * 1.2f;
				this._wetLightG = this._negLight * 1.1f;
			}
			int num;
			int num2;
			switch (Main.renderCount)
			{
			case 0:
				num = 0;
				num2 = 1;
				break;
			case 1:
				num = 1;
				num2 = 3;
				break;
			case 2:
				num = 3;
				num2 = 4;
				break;
			default:
				num = 0;
				num2 = 0;
				break;
			}
			int left = this._expandedArea.Left;
			int top = this._expandedArea.Top;
			for (int i = num; i < num2; i++)
			{
				TimeLogger.StartTimestamp fromTimestamp = TimeLogger.Start();
				int num3 = 0;
				int num4 = 0;
				switch (i)
				{
				case 0:
					this._swipe.InnerLoop1Start = this._minBoundArea.Top - top;
					this._swipe.InnerLoop2Start = this._minBoundArea.Bottom - top;
					this._swipe.InnerLoop1End = this._requestedArea.Bottom + LegacyLighting.RenderPhases - top;
					this._swipe.InnerLoop2End = this._requestedArea.Top - LegacyLighting.RenderPhases - top;
					num3 = this._minBoundArea.Left - left;
					num4 = this._minBoundArea.Right - left;
					this._swipe.JaggedArray = this._states;
					break;
				case 1:
					this._swipe.InnerLoop1Start = this._expandedArea.Left - left;
					this._swipe.InnerLoop2Start = this._expandedArea.Right - left;
					this._swipe.InnerLoop1End = this._requestedArea.Right + LegacyLighting.RenderPhases - left;
					this._swipe.InnerLoop2End = this._requestedArea.Left - LegacyLighting.RenderPhases - left;
					num3 = this._expandedArea.Top - top;
					num4 = this._expandedArea.Bottom - top;
					this._swipe.JaggedArray = this._axisFlipStates;
					break;
				case 2:
					this._swipe.InnerLoop1Start = this._offScreenTiles2ExpandedArea.Top - top;
					this._swipe.InnerLoop2Start = this._offScreenTiles2ExpandedArea.Bottom - top;
					this._swipe.InnerLoop1End = this._requestedArea.Bottom + LegacyLighting.RenderPhases - top;
					this._swipe.InnerLoop2End = this._requestedArea.Top - LegacyLighting.RenderPhases - top;
					num3 = this._offScreenTiles2ExpandedArea.Left - left;
					num4 = this._offScreenTiles2ExpandedArea.Right - left;
					this._swipe.JaggedArray = this._states;
					break;
				case 3:
					this._swipe.InnerLoop1Start = this._offScreenTiles2ExpandedArea.Left - left;
					this._swipe.InnerLoop2Start = this._offScreenTiles2ExpandedArea.Right - left;
					this._swipe.InnerLoop1End = this._requestedArea.Right + LegacyLighting.RenderPhases - left;
					this._swipe.InnerLoop2End = this._requestedArea.Left - LegacyLighting.RenderPhases - left;
					num3 = this._offScreenTiles2ExpandedArea.Top - top;
					num4 = this._offScreenTiles2ExpandedArea.Bottom - top;
					this._swipe.JaggedArray = this._axisFlipStates;
					break;
				}
				if (this._swipe.InnerLoop1Start > this._swipe.InnerLoop1End)
				{
					this._swipe.InnerLoop1Start = this._swipe.InnerLoop1End;
				}
				if (this._swipe.InnerLoop2Start < this._swipe.InnerLoop2End)
				{
					this._swipe.InnerLoop2Start = this._swipe.InnerLoop2End;
				}
				if (num3 > num4)
				{
					num3 = num4;
				}
				ParallelForAction parallelForAction;
				switch (this.Mode)
				{
				case 0:
					parallelForAction = new ParallelForAction(this.doColors_Mode0_Swipe);
					break;
				case 1:
					parallelForAction = new ParallelForAction(this.doColors_Mode1_Swipe);
					break;
				case 2:
					parallelForAction = new ParallelForAction(this.doColors_Mode2_Swipe);
					break;
				case 3:
					parallelForAction = new ParallelForAction(this.doColors_Mode3_Swipe);
					break;
				default:
					parallelForAction = new ParallelForAction(this.doColors_Mode0_Swipe);
					break;
				}
				FastParallel.For(num3, num4, parallelForAction, this._swipe);
				LegacyLighting._swipeRandom.NextSeed();
				TimeLogger.LightingByPass[i].AddTime(fromTimestamp);
			}
		}

		// Token: 0x060020E1 RID: 8417 RVA: 0x0052A484 File Offset: 0x00528684
		private void doColors_Mode0_Swipe(int outerLoopStart, int outerLoopEnd, object context)
		{
			LegacyLighting.LightingSwipeData lightingSwipeData = context as LegacyLighting.LightingSwipeData;
			FastRandom fastRandom = default(FastRandom);
			try
			{
				bool flag = true;
				for (;;)
				{
					int num;
					int val;
					int val2;
					if (flag)
					{
						num = 1;
						val = lightingSwipeData.InnerLoop1Start;
						val2 = lightingSwipeData.InnerLoop1End;
					}
					else
					{
						num = -1;
						val = lightingSwipeData.InnerLoop2Start;
						val2 = lightingSwipeData.InnerLoop2End;
					}
					for (int i = outerLoopStart; i < outerLoopEnd; i++)
					{
						LegacyLighting.LightingState[] array = lightingSwipeData.JaggedArray[i];
						float num2 = 0f;
						float num3 = 0f;
						float num4 = 0f;
						int num5 = Math.Min(array.Length - 1, Math.Max(0, val));
						int num6 = Math.Min(array.Length - 1, Math.Max(0, val2));
						int num7 = num5;
						while (num7 != num6)
						{
							LegacyLighting.LightingState lightingState = array[num7];
							LegacyLighting.LightingState lightingState2 = array[num7 + num];
							bool flag3;
							bool flag2 = flag3 = false;
							if (lightingState.R2 > num2)
							{
								num2 = lightingState.R2;
							}
							else if ((double)num2 <= 0.0185)
							{
								flag3 = true;
							}
							else if (lightingState.R2 < num2)
							{
								lightingState.R2 = num2;
							}
							if (lightingState.WetLight)
							{
								fastRandom = LegacyLighting._swipeRandom.WithModifier((ulong)((long)(i * 1000 + num7)));
							}
							if (!flag3 && lightingState2.R2 <= num2)
							{
								if (lightingState.StopLight)
								{
									num2 *= this._negLight2;
								}
								else if (lightingState.CrackedLight)
								{
									num2 *= this._negLight3;
								}
								else if (lightingState.WetLight)
								{
									if (lightingState.HoneyLight)
									{
										num2 *= this._honeyLightR * (float)fastRandom.Next(98, 100) * 0.01f;
									}
									else
									{
										num2 *= this._wetLightR * (float)fastRandom.Next(98, 100) * 0.01f;
									}
								}
								else
								{
									num2 *= this._negLight;
								}
							}
							if (lightingState.G2 > num3)
							{
								num3 = lightingState.G2;
							}
							else if ((double)num3 <= 0.0185)
							{
								flag2 = true;
							}
							else
							{
								lightingState.G2 = num3;
							}
							if (!flag2 && lightingState2.G2 <= num3)
							{
								if (lightingState.StopLight)
								{
									num3 *= this._negLight2;
								}
								else if (lightingState.CrackedLight)
								{
									num3 *= this._negLight3;
								}
								else if (lightingState.WetLight)
								{
									if (lightingState.HoneyLight)
									{
										num3 *= this._honeyLightG * (float)fastRandom.Next(97, 100) * 0.01f;
									}
									else
									{
										num3 *= this._wetLightG * (float)fastRandom.Next(97, 100) * 0.01f;
									}
								}
								else
								{
									num3 *= this._negLight;
								}
							}
							if (lightingState.B2 > num4)
							{
								num4 = lightingState.B2;
								goto IL_2A8;
							}
							if ((double)num4 > 0.0185)
							{
								lightingState.B2 = num4;
								goto IL_2A8;
							}
							IL_33F:
							num7 += num;
							continue;
							IL_2A8:
							if (lightingState2.B2 >= num4)
							{
								goto IL_33F;
							}
							if (lightingState.StopLight)
							{
								num4 *= this._negLight2;
								goto IL_33F;
							}
							if (lightingState.CrackedLight)
							{
								num4 *= this._negLight3;
								goto IL_33F;
							}
							if (!lightingState.WetLight)
							{
								num4 *= this._negLight;
								goto IL_33F;
							}
							if (lightingState.HoneyLight)
							{
								num4 *= this._honeyLightB * (float)fastRandom.Next(97, 100) * 0.01f;
								goto IL_33F;
							}
							num4 *= this._wetLightB * (float)fastRandom.Next(97, 100) * 0.01f;
							goto IL_33F;
						}
					}
					if (!flag)
					{
						break;
					}
					flag = false;
				}
			}
			catch
			{
			}
		}

		// Token: 0x060020E2 RID: 8418 RVA: 0x0052A81C File Offset: 0x00528A1C
		private void doColors_Mode1_Swipe(int outerLoopStart, int outerLoopEnd, object context)
		{
			LegacyLighting.LightingSwipeData lightingSwipeData = context as LegacyLighting.LightingSwipeData;
			FastRandom fastRandom = default(FastRandom);
			try
			{
				bool flag = true;
				for (;;)
				{
					int num;
					int num2;
					int num3;
					if (flag)
					{
						num = 1;
						num2 = lightingSwipeData.InnerLoop1Start;
						num3 = lightingSwipeData.InnerLoop1End;
					}
					else
					{
						num = -1;
						num2 = lightingSwipeData.InnerLoop2Start;
						num3 = lightingSwipeData.InnerLoop2End;
					}
					for (int i = outerLoopStart; i < outerLoopEnd; i++)
					{
						LegacyLighting.LightingState[] array = lightingSwipeData.JaggedArray[i];
						float num4 = 0f;
						int num5 = num2;
						while (num5 != num3)
						{
							LegacyLighting.LightingState lightingState = array[num5];
							if (lightingState.R2 > num4)
							{
								num4 = lightingState.R2;
								goto IL_9E;
							}
							if ((double)num4 > 0.0185)
							{
								if (lightingState.R2 < num4)
								{
									lightingState.R2 = num4;
									goto IL_9E;
								}
								goto IL_9E;
							}
							IL_155:
							num5 += num;
							continue;
							IL_9E:
							if (array[num5 + num].R2 > num4)
							{
								goto IL_155;
							}
							if (lightingState.StopLight)
							{
								num4 *= this._negLight2;
								goto IL_155;
							}
							if (lightingState.CrackedLight)
							{
								num4 *= this._negLight3;
								goto IL_155;
							}
							if (!lightingState.WetLight)
							{
								num4 *= this._negLight;
								goto IL_155;
							}
							fastRandom = LegacyLighting._swipeRandom.WithModifier((ulong)((long)(i * 1000 + num5)));
							if (lightingState.HoneyLight)
							{
								num4 *= this._honeyLightR * (float)fastRandom.Next(98, 100) * 0.01f;
								goto IL_155;
							}
							num4 *= this._wetLightR * (float)fastRandom.Next(98, 100) * 0.01f;
							goto IL_155;
						}
					}
					if (!flag)
					{
						break;
					}
					flag = false;
				}
			}
			catch
			{
			}
		}

		// Token: 0x060020E3 RID: 8419 RVA: 0x0052A9C8 File Offset: 0x00528BC8
		private void doColors_Mode2_Swipe(int outerLoopStart, int outerLoopEnd, object context)
		{
			LegacyLighting.LightingSwipeData lightingSwipeData = context as LegacyLighting.LightingSwipeData;
			try
			{
				bool flag = true;
				for (;;)
				{
					int num;
					int num2;
					int num3;
					if (flag)
					{
						num = 1;
						num2 = lightingSwipeData.InnerLoop1Start;
						num3 = lightingSwipeData.InnerLoop1End;
					}
					else
					{
						num = -1;
						num2 = lightingSwipeData.InnerLoop2Start;
						num3 = lightingSwipeData.InnerLoop2End;
					}
					for (int i = outerLoopStart; i < outerLoopEnd; i++)
					{
						LegacyLighting.LightingState[] array = lightingSwipeData.JaggedArray[i];
						float num4 = 0f;
						int num5 = num2;
						while (num5 != num3)
						{
							LegacyLighting.LightingState lightingState = array[num5];
							if (lightingState.R2 > num4)
							{
								num4 = lightingState.R2;
								goto IL_81;
							}
							if (num4 > 0f)
							{
								lightingState.R2 = num4;
								goto IL_81;
							}
							IL_CE:
							num5 += num;
							continue;
							IL_81:
							if (lightingState.StopLight)
							{
								num4 -= this._negLight2;
								goto IL_CE;
							}
							if (lightingState.CrackedLight)
							{
								num4 -= this._negLight3;
								goto IL_CE;
							}
							if (lightingState.WetLight)
							{
								num4 -= this._wetLightR;
								goto IL_CE;
							}
							num4 -= this._negLight;
							goto IL_CE;
						}
					}
					if (!flag)
					{
						break;
					}
					flag = false;
				}
			}
			catch
			{
			}
		}

		// Token: 0x060020E4 RID: 8420 RVA: 0x0052AAE0 File Offset: 0x00528CE0
		private void doColors_Mode3_Swipe(int outerLoopStart, int outerLoopEnd, object context)
		{
			LegacyLighting.LightingSwipeData lightingSwipeData = context as LegacyLighting.LightingSwipeData;
			try
			{
				bool flag = true;
				for (;;)
				{
					int num;
					int num2;
					int num3;
					if (flag)
					{
						num = 1;
						num2 = lightingSwipeData.InnerLoop1Start;
						num3 = lightingSwipeData.InnerLoop1End;
					}
					else
					{
						num = -1;
						num2 = lightingSwipeData.InnerLoop2Start;
						num3 = lightingSwipeData.InnerLoop2End;
					}
					for (int i = outerLoopStart; i < outerLoopEnd; i++)
					{
						LegacyLighting.LightingState[] array = lightingSwipeData.JaggedArray[i];
						float num4 = 0f;
						float num5 = 0f;
						float num6 = 0f;
						int num7 = num2;
						while (num7 != num3)
						{
							LegacyLighting.LightingState lightingState = array[num7];
							bool flag3;
							bool flag2 = flag3 = false;
							if (lightingState.R2 > num4)
							{
								num4 = lightingState.R2;
							}
							else if (num4 <= 0f)
							{
								flag3 = true;
							}
							else
							{
								lightingState.R2 = num4;
							}
							if (!flag3)
							{
								if (lightingState.StopLight)
								{
									num4 -= this._negLight2;
								}
								if (lightingState.CrackedLight)
								{
									num4 -= this._negLight3;
								}
								else if (lightingState.WetLight)
								{
									num4 -= this._wetLightR;
								}
								else
								{
									num4 -= this._negLight;
								}
							}
							if (lightingState.G2 > num5)
							{
								num5 = lightingState.G2;
							}
							else if (num5 <= 0f)
							{
								flag2 = true;
							}
							else
							{
								lightingState.G2 = num5;
							}
							if (!flag2)
							{
								if (lightingState.StopLight)
								{
									num5 -= this._negLight2;
								}
								if (lightingState.CrackedLight)
								{
									num5 -= this._negLight3;
								}
								else if (lightingState.WetLight)
								{
									num5 -= this._wetLightG;
								}
								else
								{
									num5 -= this._negLight;
								}
							}
							if (lightingState.B2 > num6)
							{
								num6 = lightingState.B2;
								goto IL_18D;
							}
							if (num6 > 0f)
							{
								lightingState.B2 = num6;
								goto IL_18D;
							}
							IL_1C2:
							num7 += num;
							continue;
							IL_18D:
							if (lightingState.StopLight)
							{
								num6 -= this._negLight2;
							}
							if (lightingState.CrackedLight)
							{
								num6 -= this._negLight3;
								goto IL_1C2;
							}
							num6 -= this._negLight;
							goto IL_1C2;
						}
					}
					if (!flag)
					{
						break;
					}
					flag = false;
				}
			}
			catch
			{
			}
		}

		// Token: 0x04004B27 RID: 19239
		public static int RenderPhases = 4;

		// Token: 0x04004B28 RID: 19240
		private bool _rgb = true;

		// Token: 0x04004B29 RID: 19241
		private int _offScreenTiles2 = 35;

		// Token: 0x04004B2A RID: 19242
		private float _oldSkyColor;

		// Token: 0x04004B2B RID: 19243
		private float _skyColor;

		// Token: 0x04004B2C RID: 19244
		private int _requestedRectLeft;

		// Token: 0x04004B2D RID: 19245
		private int _requestedRectRight;

		// Token: 0x04004B2E RID: 19246
		private int _requestedRectTop;

		// Token: 0x04004B2F RID: 19247
		private int _requestedRectBottom;

		// Token: 0x04004B30 RID: 19248
		private LegacyLighting.LightingState[][] _states;

		// Token: 0x04004B31 RID: 19249
		private LegacyLighting.LightingState[][] _axisFlipStates;

		// Token: 0x04004B32 RID: 19250
		private LegacyLighting.LightingSwipeData _swipe;

		// Token: 0x04004B33 RID: 19251
		private LegacyLighting.LightingSwipeData[] _threadSwipes;

		// Token: 0x04004B34 RID: 19252
		private int _scrX;

		// Token: 0x04004B35 RID: 19253
		private int _scrY;

		// Token: 0x04004B36 RID: 19254
		private int _minX;

		// Token: 0x04004B37 RID: 19255
		private int _maxX;

		// Token: 0x04004B38 RID: 19256
		private int _minY;

		// Token: 0x04004B39 RID: 19257
		private int _maxY;

		// Token: 0x04004B3A RID: 19258
		private const int MAX_TEMP_LIGHTS = 2000;

		// Token: 0x04004B3B RID: 19259
		private Dictionary<Point16, LegacyLighting.ColorTriplet> _tempLights;

		// Token: 0x04004B3C RID: 19260
		private int _expandedRectLeft;

		// Token: 0x04004B3D RID: 19261
		private int _expandedRectTop;

		// Token: 0x04004B3E RID: 19262
		private int _expandedRectRight;

		// Token: 0x04004B3F RID: 19263
		private int _expandedRectBottom;

		// Token: 0x04004B40 RID: 19264
		private float _negLight = 0.04f;

		// Token: 0x04004B41 RID: 19265
		private float _negLight2 = 0.16f;

		// Token: 0x04004B42 RID: 19266
		private float _negLight3 = 0.08f;

		// Token: 0x04004B43 RID: 19267
		private float _wetLightR = 0.16f;

		// Token: 0x04004B44 RID: 19268
		private float _wetLightG = 0.16f;

		// Token: 0x04004B45 RID: 19269
		private float _wetLightB = 0.16f;

		// Token: 0x04004B46 RID: 19270
		private float _honeyLightR = 0.16f;

		// Token: 0x04004B47 RID: 19271
		private float _honeyLightG = 0.16f;

		// Token: 0x04004B48 RID: 19272
		private float _honeyLightB = 0.16f;

		// Token: 0x04004B49 RID: 19273
		private float _blueWave = 1f;

		// Token: 0x04004B4A RID: 19274
		private int _blueDir = 1;

		// Token: 0x04004B4B RID: 19275
		private LegacyLighting.RectArea _minBoundArea;

		// Token: 0x04004B4C RID: 19276
		private LegacyLighting.RectArea _requestedArea;

		// Token: 0x04004B4D RID: 19277
		private LegacyLighting.RectArea _expandedArea;

		// Token: 0x04004B4E RID: 19278
		private LegacyLighting.RectArea _offScreenTiles2ExpandedArea;

		// Token: 0x04004B4F RID: 19279
		private TileLightScanner _tileScanner = new TileLightScanner();

		// Token: 0x04004B50 RID: 19280
		private readonly Camera _camera;

		// Token: 0x04004B51 RID: 19281
		private Vector2? _lastCameraPosition;

		// Token: 0x04004B52 RID: 19282
		private static FastRandom _swipeRandom = FastRandom.CreateWithRandomSeed();

		// Token: 0x04004B53 RID: 19283
		private LightMap _lightMap = new LightMap();

		// Token: 0x020007A1 RID: 1953
		public struct RectArea
		{
			// Token: 0x060041A0 RID: 16800 RVA: 0x006BB169 File Offset: 0x006B9369
			public void Set(int left, int right, int top, int bottom)
			{
				this.Left = left;
				this.Right = right;
				this.Top = top;
				this.Bottom = bottom;
			}

			// Token: 0x04007049 RID: 28745
			public int Left;

			// Token: 0x0400704A RID: 28746
			public int Right;

			// Token: 0x0400704B RID: 28747
			public int Top;

			// Token: 0x0400704C RID: 28748
			public int Bottom;
		}

		// Token: 0x020007A2 RID: 1954
		private class LightingSwipeData
		{
			// Token: 0x060041A1 RID: 16801 RVA: 0x006BB188 File Offset: 0x006B9388
			public LightingSwipeData()
			{
				this.InnerLoop1Start = 0;
				this.InnerLoop1End = 0;
				this.InnerLoop2Start = 0;
				this.InnerLoop2End = 0;
			}

			// Token: 0x060041A2 RID: 16802 RVA: 0x006BB1AC File Offset: 0x006B93AC
			public void CopyFrom(LegacyLighting.LightingSwipeData from)
			{
				this.InnerLoop1Start = from.InnerLoop1Start;
				this.InnerLoop1End = from.InnerLoop1End;
				this.InnerLoop2Start = from.InnerLoop2Start;
				this.InnerLoop2End = from.InnerLoop2End;
				this.JaggedArray = from.JaggedArray;
			}

			// Token: 0x0400704D RID: 28749
			public int InnerLoop1Start;

			// Token: 0x0400704E RID: 28750
			public int InnerLoop1End;

			// Token: 0x0400704F RID: 28751
			public int InnerLoop2Start;

			// Token: 0x04007050 RID: 28752
			public int InnerLoop2End;

			// Token: 0x04007051 RID: 28753
			public LegacyLighting.LightingState[][] JaggedArray;
		}

		// Token: 0x020007A3 RID: 1955
		private class LightingState
		{
			// Token: 0x060041A3 RID: 16803 RVA: 0x006BB1EA File Offset: 0x006B93EA
			public Vector3 ToVector3()
			{
				return new Vector3(this.R, this.G, this.B);
			}

			// Token: 0x04007052 RID: 28754
			public float R;

			// Token: 0x04007053 RID: 28755
			public float R2;

			// Token: 0x04007054 RID: 28756
			public float G;

			// Token: 0x04007055 RID: 28757
			public float G2;

			// Token: 0x04007056 RID: 28758
			public float B;

			// Token: 0x04007057 RID: 28759
			public float B2;

			// Token: 0x04007058 RID: 28760
			public bool CrackedLight;

			// Token: 0x04007059 RID: 28761
			public bool StopLight;

			// Token: 0x0400705A RID: 28762
			public bool WetLight;

			// Token: 0x0400705B RID: 28763
			public bool HoneyLight;
		}

		// Token: 0x020007A4 RID: 1956
		private struct ColorTriplet
		{
			// Token: 0x060041A5 RID: 16805 RVA: 0x006BB203 File Offset: 0x006B9403
			public ColorTriplet(float R, float G, float B)
			{
				this.R = R;
				this.G = G;
				this.B = B;
			}

			// Token: 0x060041A6 RID: 16806 RVA: 0x006BB21C File Offset: 0x006B941C
			public ColorTriplet(float averageColor)
			{
				this.B = averageColor;
				this.G = averageColor;
				this.R = averageColor;
			}

			// Token: 0x0400705C RID: 28764
			public float R;

			// Token: 0x0400705D RID: 28765
			public float G;

			// Token: 0x0400705E RID: 28766
			public float B;
		}
	}
}
