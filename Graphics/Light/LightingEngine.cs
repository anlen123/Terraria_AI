using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Threading;
using Terraria.Graphics.Capture;
using Terraria.Map;

namespace Terraria.Graphics.Light
{
	// Token: 0x020001FD RID: 509
	public class LightingEngine : ILightingEngine
	{
		// Token: 0x060020E6 RID: 8422 RVA: 0x0052AD0A File Offset: 0x00528F0A
		public void AddLight(int x, int y, Vector3 color)
		{
			this._perFrameLights.Add(new LightingEngine.PerFrameLight(new Point(x, y), color));
		}

		// Token: 0x060020E7 RID: 8423 RVA: 0x0052AD24 File Offset: 0x00528F24
		public void Clear()
		{
			this._activeLightMap.Clear();
			this._workingLightMap.Clear();
			this._perFrameLights.Clear();
			this._oldPerFrameLights.Clear();
		}

		// Token: 0x060020E8 RID: 8424 RVA: 0x0052AD54 File Offset: 0x00528F54
		public Vector3 GetColor(int x, int y)
		{
			if (!this._activeProcessedArea.Contains(x, y))
			{
				return Vector3.Zero;
			}
			x -= this._activeProcessedArea.X;
			y -= this._activeProcessedArea.Y;
			return this._activeLightMap[x, y];
		}

		// Token: 0x060020E9 RID: 8425 RVA: 0x0052ADA4 File Offset: 0x00528FA4
		public void ProcessArea(Rectangle area)
		{
			TimeLogger.StartTimestamp fromTimestamp = TimeLogger.Start();
			switch (this._state)
			{
			case LightingEngine.EngineState.MinimapUpdate:
				if (Main.mapDelay > 0)
				{
					Main.mapDelay--;
				}
				else
				{
					this.ExportToMiniMap();
				}
				Main.renderCount = 3;
				break;
			case LightingEngine.EngineState.ExportMetrics:
				Main.UpdateSceneMetrics();
				Main.renderCount = 0;
				break;
			case LightingEngine.EngineState.Scan:
				this.ProcessScan(area);
				Main.renderCount = 1;
				break;
			case LightingEngine.EngineState.Blur:
				this.ProcessBlur();
				this.Present();
				Main.renderCount = 2;
				break;
			}
			TimeLogger.LightingByPass[(int)this._state].AddTime(fromTimestamp);
			this.IncrementState();
		}

		// Token: 0x060020EA RID: 8426 RVA: 0x0052AE40 File Offset: 0x00529040
		private void IncrementState()
		{
			this._state = (this._state + 1) % LightingEngine.EngineState.Max;
		}

		// Token: 0x060020EB RID: 8427 RVA: 0x0052AE54 File Offset: 0x00529054
		private void ProcessScan(Rectangle area)
		{
			area.Inflate(28, 28);
			this._workingProcessedArea = area;
			this._workingLightMap.SetSize(area.Width, area.Height);
			this._workingLightMap.NonVisiblePadding = 18;
			this._tileScanner.Update();
			this._tileScanner.ExportTo(area, this._workingLightMap, new TileLightScannerOptions
			{
				DrawInvisibleWalls = Main.ShouldShowInvisibleBlocksAndWalls()
			});
		}

		// Token: 0x060020EC RID: 8428 RVA: 0x0052AEC9 File Offset: 0x005290C9
		private void ProcessBlur()
		{
			this.UpdateLightDecay();
			this.ApplyPerFrameLights();
			this._workingLightMap.Blur();
		}

		// Token: 0x060020ED RID: 8429 RVA: 0x0052AEE2 File Offset: 0x005290E2
		private void Present()
		{
			Utils.Swap<LightMap>(ref this._activeLightMap, ref this._workingLightMap);
			Utils.Swap<Rectangle>(ref this._activeProcessedArea, ref this._workingProcessedArea);
		}

		// Token: 0x060020EE RID: 8430 RVA: 0x0052AF08 File Offset: 0x00529108
		private void UpdateLightDecay()
		{
			LightMap workingLightMap = this._workingLightMap;
			workingLightMap.LightDecayThroughAir = 0.91f;
			workingLightMap.LightDecayThroughSolid = 0.56f;
			workingLightMap.LightDecayThroughHoney = new Vector3(0.75f, 0.7f, 0.6f) * 0.91f;
			switch (Main.waterStyle)
			{
			case 0:
			case 1:
			case 7:
			case 8:
				workingLightMap.LightDecayThroughWater = new Vector3(0.88f, 0.96f, 1.015f) * 0.91f;
				break;
			case 2:
				workingLightMap.LightDecayThroughWater = new Vector3(0.94f, 0.85f, 1.01f) * 0.91f;
				break;
			case 3:
				workingLightMap.LightDecayThroughWater = new Vector3(0.84f, 0.95f, 1.015f) * 0.91f;
				break;
			case 4:
				workingLightMap.LightDecayThroughWater = new Vector3(0.9f, 0.86f, 1.01f) * 0.91f;
				break;
			case 5:
				workingLightMap.LightDecayThroughWater = new Vector3(0.84f, 0.99f, 1.01f) * 0.91f;
				break;
			case 6:
				workingLightMap.LightDecayThroughWater = new Vector3(0.83f, 0.93f, 0.98f) * 0.91f;
				break;
			case 9:
				workingLightMap.LightDecayThroughWater = new Vector3(1f, 0.88f, 0.84f) * 0.91f;
				break;
			case 10:
				workingLightMap.LightDecayThroughWater = new Vector3(0.83f, 1f, 1f) * 0.91f;
				break;
			case 12:
				workingLightMap.LightDecayThroughWater = new Vector3(0.95f, 0.98f, 0.85f) * 0.91f;
				break;
			case 13:
				workingLightMap.LightDecayThroughWater = new Vector3(0.9f, 1f, 1.02f) * 0.91f;
				break;
			}
			Player perspectivePlayer = Main.SceneMetrics.PerspectivePlayer;
			if (perspectivePlayer.nightVision)
			{
				workingLightMap.LightDecayThroughAir *= 1.03f;
				workingLightMap.LightDecayThroughSolid *= 1.03f;
			}
			if (perspectivePlayer.blind)
			{
				workingLightMap.LightDecayThroughAir *= 0.95f;
				workingLightMap.LightDecayThroughSolid *= 0.95f;
			}
			if (perspectivePlayer.blackout)
			{
				workingLightMap.LightDecayThroughAir *= 0.85f;
				workingLightMap.LightDecayThroughSolid *= 0.85f;
			}
			if (perspectivePlayer.headcovered)
			{
				workingLightMap.LightDecayThroughAir *= 0.85f;
				workingLightMap.LightDecayThroughSolid *= 0.85f;
			}
			workingLightMap.LightDecayThroughAir *= Main.SceneState.airLightDecay;
			workingLightMap.LightDecayThroughSolid *= Main.SceneState.solidLightDecay;
		}

		// Token: 0x060020EF RID: 8431 RVA: 0x0052B214 File Offset: 0x00529414
		private void ApplyPerFrameLights()
		{
			List<LightingEngine.PerFrameLight> list = this._perFrameLights;
			if (Main.gamePaused)
			{
				list = this._oldPerFrameLights;
			}
			for (int i = 0; i < list.Count; i++)
			{
				Point position = list[i].Position;
				if (this._workingProcessedArea.Contains(position))
				{
					Vector3 color = list[i].Color;
					Vector3 vector = this._workingLightMap[position.X - this._workingProcessedArea.X, position.Y - this._workingProcessedArea.Y];
					Vector3.Max(ref vector, ref color, out color);
					this._workingLightMap[position.X - this._workingProcessedArea.X, position.Y - this._workingProcessedArea.Y] = color;
				}
			}
			if (!CaptureManager.Instance.IsCapturing && !Main.gamePaused)
			{
				Utils.Swap<List<LightingEngine.PerFrameLight>>(ref this._perFrameLights, ref this._oldPerFrameLights);
				this._perFrameLights.Clear();
			}
		}

		// Token: 0x060020F0 RID: 8432 RVA: 0x0052B310 File Offset: 0x00529510
		public void Rebuild()
		{
			this._activeProcessedArea = Rectangle.Empty;
			this._workingProcessedArea = Rectangle.Empty;
			this._state = LightingEngine.EngineState.MinimapUpdate;
			this._activeLightMap = new LightMap();
			this._workingLightMap = new LightMap();
		}

		// Token: 0x060020F1 RID: 8433 RVA: 0x0052B348 File Offset: 0x00529548
		private void ExportToMiniMap()
		{
			if (!Main.mapEnabled)
			{
				return;
			}
			if (this._activeProcessedArea.Width <= 0 || this._activeProcessedArea.Height <= 0)
			{
				return;
			}
			Rectangle area = new Rectangle(this._activeProcessedArea.X + 28, this._activeProcessedArea.Y + 28, this._activeProcessedArea.Width - 56, this._activeProcessedArea.Height - 56);
			Rectangle value = new Rectangle(0, 0, Main.maxTilesX, Main.maxTilesY);
			value.Inflate(-40, -40);
			area = Rectangle.Intersect(area, value);
			area = Rectangle.Intersect(area, MapHelper.sceneArea);
			FastParallel.For(area.Left, area.Right, delegate(int start, int end, object context)
			{
				for (int i = start; i < end; i++)
				{
					for (int j = area.Top; j < area.Bottom; j++)
					{
						Vector3 vector = this._activeLightMap[i - this._activeProcessedArea.X, j - this._activeProcessedArea.Y];
						float num = Math.Max(Math.Max(vector.X, vector.Y), vector.Z);
						byte light = (byte)Math.Min(255, (int)(num * 255f));
						Main.Map.UpdateLighting(i, j, light);
					}
				}
			}, null);
			Main.updateMap = new Rectangle?(area);
		}

		// Token: 0x04004B55 RID: 19285
		public const int AREA_PADDING = 28;

		// Token: 0x04004B56 RID: 19286
		private const int NON_VISIBLE_PADDING = 18;

		// Token: 0x04004B57 RID: 19287
		private List<LightingEngine.PerFrameLight> _perFrameLights = new List<LightingEngine.PerFrameLight>();

		// Token: 0x04004B58 RID: 19288
		private List<LightingEngine.PerFrameLight> _oldPerFrameLights = new List<LightingEngine.PerFrameLight>();

		// Token: 0x04004B59 RID: 19289
		private TileLightScanner _tileScanner = new TileLightScanner();

		// Token: 0x04004B5A RID: 19290
		private LightMap _activeLightMap = new LightMap();

		// Token: 0x04004B5B RID: 19291
		private Rectangle _activeProcessedArea;

		// Token: 0x04004B5C RID: 19292
		private LightMap _workingLightMap = new LightMap();

		// Token: 0x04004B5D RID: 19293
		private Rectangle _workingProcessedArea;

		// Token: 0x04004B5E RID: 19294
		private LightingEngine.EngineState _state;

		// Token: 0x020007A5 RID: 1957
		private enum EngineState
		{
			// Token: 0x04007060 RID: 28768
			MinimapUpdate,
			// Token: 0x04007061 RID: 28769
			ExportMetrics,
			// Token: 0x04007062 RID: 28770
			Scan,
			// Token: 0x04007063 RID: 28771
			Blur,
			// Token: 0x04007064 RID: 28772
			Max
		}

		// Token: 0x020007A6 RID: 1958
		private struct PerFrameLight
		{
			// Token: 0x060041A7 RID: 16807 RVA: 0x006BB242 File Offset: 0x006B9442
			public PerFrameLight(Point position, Vector3 color)
			{
				this.Position = position;
				this.Color = color;
			}

			// Token: 0x04007065 RID: 28773
			public readonly Point Position;

			// Token: 0x04007066 RID: 28774
			public readonly Vector3 Color;
		}
	}
}
