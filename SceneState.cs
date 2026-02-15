using System;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.Audio;
using Terraria.GameContent.Events;
using Terraria.GameContent.RGB;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Map;

namespace Terraria
{
	// Token: 0x02000016 RID: 22
	public class SceneState
	{
		// Token: 0x060000AA RID: 170 RVA: 0x0000AFD9 File Offset: 0x000091D9
		public SceneState()
		{
			this.Reset();
		}

		// Token: 0x060000AB RID: 171 RVA: 0x0000B000 File Offset: 0x00009200
		public void Reset()
		{
			this.airLightDecay = 1f;
			this.solidLightDecay = 1f;
			this.outsideWeatherEffectIntensity = 1f;
			this._outsideWeatherEffectIntensityBackingValue = 1f;
			this._deerclopsBlizzardSmoothedEffect = 0f;
			this._blizzardSoundVolume = 0f;
			this._shimmerBrightenDelay = 0f;
			this.skipTransitions = true;
		}

		// Token: 0x060000AC RID: 172 RVA: 0x0000B061 File Offset: 0x00009261
		public void Update(SceneMetrics metrics)
		{
			this.ApplyVisuals(metrics);
			MapHelper.CaptureSceneState(metrics);
			this.skipTransitions = false;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x0000B078 File Offset: 0x00009278
		private void ApplyVisuals(SceneMetrics metrics)
		{
			if (Main.dedServ)
			{
				return;
			}
			Player perspectivePlayer = metrics.PerspectivePlayer;
			this.UpdateRGBPeriheralProbe(metrics);
			this.UpdateGraveyard(metrics);
			this.UpdateShimmer(metrics);
			this.UpdateLightDecay(metrics);
			ScreenObstruction.Update(this, metrics);
			ScreenDarkness.Update(this, metrics);
			MoonlordDeathDrama.Update(this, metrics);
			bool flag = metrics.ZoneRain && metrics.ZoneSnow;
			bool flag2 = metrics.TileCenter.Y > Main.maxTilesY - 320;
			bool flag3 = (double)metrics.TileCenter.Y < Main.worldSurface && metrics.ZoneDesert && !metrics.ZoneRain && !metrics.ZoneSandstorm;
			this.ManageSpecialBiomeVisuals("Stardust", metrics.CloseEnoughToStardustTower, metrics.ClosestNPCPosition[493] - new Vector2(0f, 10f), false);
			this.ManageSpecialBiomeVisuals("Nebula", metrics.CloseEnoughToNebulaTower, metrics.ClosestNPCPosition[507] - new Vector2(0f, 10f), false);
			this.ManageSpecialBiomeVisuals("Vortex", metrics.CloseEnoughToVortexTower, metrics.ClosestNPCPosition[422] - new Vector2(0f, 10f), false);
			this.ManageSpecialBiomeVisuals("Solar", metrics.CloseEnoughToSolarTower, metrics.ClosestNPCPosition[517] - new Vector2(0f, 10f), false);
			this.ManageSpecialBiomeVisuals("MoonLord", metrics.ClosestNPCPosition[398] != Vector2.Zero, default(Vector2), false);
			bool flag4 = metrics.CloseEnoughToSolarTower || metrics.CloseEnoughToVortexTower || metrics.CloseEnoughToNebulaTower || metrics.CloseEnoughToStardustTower;
			this.ManageSpecialBiomeVisuals("MonolithVortex", (!flag4 && metrics.ActiveMonolithType == 0) || perspectivePlayer.vortexMonolithShader, default(Vector2), false);
			this.ManageSpecialBiomeVisuals("MonolithNebula", (!flag4 && metrics.ActiveMonolithType == 1) || perspectivePlayer.nebulaMonolithShader, default(Vector2), false);
			this.ManageSpecialBiomeVisuals("MonolithStardust", (!flag4 && metrics.ActiveMonolithType == 2) || perspectivePlayer.stardustMonolithShader, default(Vector2), false);
			this.ManageSpecialBiomeVisuals("MonolithSolar", (!flag4 && metrics.ActiveMonolithType == 3) || perspectivePlayer.solarMonolithShader, default(Vector2), false);
			this.ManageSpecialBiomeVisuals("MonolithMoonLord", (!flag4 && metrics.ActiveMonolithType == 4) || perspectivePlayer.moonLordMonolithShader, default(Vector2), false);
			this.ManageSpecialBiomeVisuals("BloodMoon", Main.bloodMoon || metrics.BloodMoonMonolith || perspectivePlayer.bloodMoonMonolithShader, default(Vector2), false);
			bool flag5 = Main.UseStormEffects && flag;
			bool flag6 = !Main.dayTime && !flag5 && Main.GraveyardVisualIntensity < 0.5f;
			this.ManageSpecialBiomeVisuals("Aurora", metrics.ZoneSnow && flag6, default(Vector2), false);
			this.ManageSpecialBiomeVisuals("Blizzard", Main.UseStormEffects && flag, default(Vector2), false);
			this.ManageSpecialBiomeVisuals("Sandstorm", Main.UseStormEffects && Sandstorm.ShowSandstormVisuals(), default(Vector2), false);
			bool flag7 = flag2 || flag3 || perspectivePlayer.sunScorchCounter > 0;
			this.ManageSpecialBiomeVisuals("HeatDistortion", Main.UseHeatDistortion && flag7, default(Vector2), false);
			this.ManageSpecialBiomeVisuals("Graveyard", Main.GraveyardVisualIntensity > 0f, default(Vector2), false);
			this.ManageSpecialBiomeVisuals("Sepia", Main.onlyDontStarveWorld ^ (perspectivePlayer.dontStarveShader || metrics.RadioThingMonolith), default(Vector2), false);
			this.ManageSpecialBiomeVisuals("Noir", metrics.NoirMonolith || perspectivePlayer.noirShader, default(Vector2), false);
			this.ManageSpecialBiomeVisuals("CRT", metrics.CRTMonolith || perspectivePlayer.CRTMonolithShader, default(Vector2), false);
			this.ManageSpecialBiomeVisuals("Test2", metrics.RetroMonolith || perspectivePlayer.retroMonolithShader, default(Vector2), false);
			this.ManageSpecialBiomeVisuals("WaterDistortion", Main.WaveQuality > 0, default(Vector2), false);
			bool flag8 = metrics.TownNPCCount > 0 || metrics.PartyMonolithCount > 0;
			this.MoveTowards(ref SkyManager.Instance["Party"].Opacity, (float)(flag8 ? 1 : 0), 0.01f);
			if (Filters.Scene["Graveyard"].IsActive())
			{
				float progress = MathHelper.Lerp(0f, 0.75f, Main.GraveyardVisualIntensity);
				ScreenShaderData shader = Filters.Scene["Graveyard"].GetShader();
				shader.UseTargetPosition(metrics.Center);
				shader.UseProgress(progress);
				shader.UseIntensity(1.2f);
			}
			if (Filters.Scene["Noir"].IsActive())
			{
				float num = 0.1f;
				float value = Utils.Remap(Vector3.Dot(Main.tileColor.ToVector3(), new Vector3(0.33333334f)), 0.5f, 0.1f, 0f, 0.2f, true);
				float amount = Utils.Remap((float)((int)Main.worldSurface - metrics.TileCenter.Y), -40f, 40f, 0f, 1f, true);
				num = MathHelper.Lerp(num, value, amount);
				float value2 = 0.15f;
				float amount2 = Utils.Remap((float)(metrics.TileCenter.Y - Main.UnderworldLayer), -40f, 40f, 0f, 1f, true);
				num = MathHelper.Lerp(num, value2, amount2);
				Random random = new Random((int)(Main.GlobalTimeWrappedHourly * 10f));
				float x = (float)random.NextDouble();
				float y = (float)random.NextDouble();
				ScreenShaderData shader2 = Filters.Scene["Noir"].GetShader();
				shader2.UseTargetPosition(metrics.Center);
				shader2.UseIntensity(num);
				shader2.UseImageOffset(new Vector2(x, y));
			}
			if (Filters.Scene["WaterDistortion"].IsActive())
			{
				float num2 = (float)Main.maxTilesX * 0.5f - Math.Abs((float)metrics.TileCenter.X - (float)Main.maxTilesX * 0.5f);
				float num3 = 1f;
				float num4 = Math.Abs(Main.windSpeedCurrent);
				num3 += num4 * 1.25f;
				float num5 = MathHelper.Clamp(Main.maxRaining, 0f, 1f);
				num3 += num5 * 1.25f;
				float num6 = -(MathHelper.Clamp((num2 - 380f) / 100f, 0f, 1f) * 0.5f - 0.25f);
				num3 += num6;
				float num7 = 1f - MathHelper.Clamp(3f * ((float)((double)metrics.TileCenter.Y - Main.worldSurface) / (float)(Main.rockLayer - Main.worldSurface)), 0f, 1f);
				num3 *= num7;
				float num8 = 0.9f - MathHelper.Clamp((float)(Main.maxTilesY - metrics.TileCenter.Y - 200) / 300f, 0f, 1f) * 0.9f;
				num3 += num8;
				num3 += (1f - num7) * 0.75f;
				num3 = MathHelper.Clamp(num3, 0f, 2.5f);
				Filters.Scene["WaterDistortion"].GetShader().UseIntensity(num3);
			}
			this.MoveTowards(ref this._outsideWeatherEffectIntensityBackingValue, metrics.BehindBackwall ? -0.1f : 1.1f, 0.005f);
			this.outsideWeatherEffectIntensity = Utils.Clamp<float>(this._outsideWeatherEffectIntensityBackingValue, 0f, 1f);
			if (Filters.Scene["Sandstorm"].IsActive())
			{
				Filters.Scene["Sandstorm"].GetShader().UseIntensity(this.outsideWeatherEffectIntensity * 0.4f * Math.Min(1f, Sandstorm.Severity));
				Filters.Scene["Sandstorm"].GetShader().UseOpacity(Math.Min(1f, Sandstorm.Severity * 1.5f) * this.outsideWeatherEffectIntensity);
				((SimpleOverlay)Overlays.Scene["Sandstorm"]).GetShader().UseOpacity(Math.Min(1f, Sandstorm.Severity * 1.5f) * (1f - this.outsideWeatherEffectIntensity));
			}
			Filter filter = Filters.Scene["HeatDistortion"];
			if (filter.IsActive())
			{
				float num9 = 0f;
				if (perspectivePlayer.sunScorchCounter > 0)
				{
					float val = Utils.GetLerpValue(0f, 300f, (float)perspectivePlayer.sunScorchCounter, true) * 4f;
					num9 = Math.Max(num9, val);
				}
				if (flag2)
				{
					float val2 = (float)(metrics.TileCenter.Y - (Main.maxTilesY - 320)) / 120f;
					val2 = Math.Min(1f, val2) * 2f;
					num9 = Math.Max(num9, val2);
				}
				else if (flag3)
				{
					Vector3 vector = Main.tileColor.ToVector3();
					float num10 = (vector.X + vector.Y + vector.Z) / 3f;
					float val3 = this.outsideWeatherEffectIntensity * 4f * Math.Max(0f, 0.5f - Main.cloudAlpha) * num10;
					num9 = Math.Max(num9, val3);
				}
				filter.GetShader().UseIntensity(num9);
				filter.IsHidden = (num9 <= 0f);
			}
			if (!this._disabledBlizzardGraphic)
			{
				try
				{
					if (flag)
					{
						float num11 = Main.cloudAlpha;
						if (Main.remixWorld)
						{
							num11 = 0.4f;
						}
						this.MoveTowards(ref this._deerclopsBlizzardSmoothedEffect, (float)(NPC.IsADeerclopsNearScreen() ? 1 : 0), 0.0033333334f);
						float num12 = Math.Min(1f, num11 * 2f) * this.outsideWeatherEffectIntensity;
						float num13 = this.outsideWeatherEffectIntensity * 0.4f * Math.Min(1f, num11 * 2f) * 0.9f + 0.1f;
						num13 = MathHelper.Lerp(num13, num13 * 0.5f, this._deerclopsBlizzardSmoothedEffect);
						num12 = MathHelper.Lerp(num12, num12 * 0.5f, this._deerclopsBlizzardSmoothedEffect);
						Filters.Scene["Blizzard"].GetShader().UseIntensity(num13);
						Filters.Scene["Blizzard"].GetShader().UseOpacity(num12);
						((SimpleOverlay)Overlays.Scene["Blizzard"]).GetShader().UseOpacity(1f - num12);
					}
				}
				catch
				{
					this._disabledBlizzardGraphic = true;
				}
			}
			if (!this._disabledBlizzardSound)
			{
				try
				{
					if (flag)
					{
						bool activeSound = SoundEngine.GetActiveSound(this._strongBlizzardSound) != null;
						ActiveSound activeSound2 = SoundEngine.GetActiveSound(this._insideBlizzardSound);
						if (!activeSound)
						{
							this._strongBlizzardSound = SoundEngine.PlayTrackedSound(SoundID.BlizzardStrongLoop);
						}
						if (activeSound2 == null)
						{
							this._insideBlizzardSound = SoundEngine.PlayTrackedSound(SoundID.BlizzardInsideBuildingLoop);
						}
						SoundEngine.GetActiveSound(this._strongBlizzardSound);
						activeSound2 = SoundEngine.GetActiveSound(this._insideBlizzardSound);
					}
					this.MoveTowards(ref this._blizzardSoundVolume, (float)(flag ? 1 : 0), 0.01f);
					float num14 = Math.Min(1f, Main.cloudAlpha * 2f) * this.outsideWeatherEffectIntensity;
					ActiveSound activeSound3 = SoundEngine.GetActiveSound(this._strongBlizzardSound);
					ActiveSound activeSound4 = SoundEngine.GetActiveSound(this._insideBlizzardSound);
					if (this._blizzardSoundVolume > 0f)
					{
						if (activeSound3 == null)
						{
							this._strongBlizzardSound = SoundEngine.PlayTrackedSound(SoundID.BlizzardStrongLoop);
							activeSound3 = SoundEngine.GetActiveSound(this._strongBlizzardSound);
						}
						activeSound3.Volume = num14 * this._blizzardSoundVolume;
						if (activeSound4 == null)
						{
							this._insideBlizzardSound = SoundEngine.PlayTrackedSound(SoundID.BlizzardInsideBuildingLoop);
							activeSound4 = SoundEngine.GetActiveSound(this._insideBlizzardSound);
						}
						activeSound4.Volume = (1f - num14) * this._blizzardSoundVolume;
					}
					else
					{
						if (activeSound3 != null)
						{
							activeSound3.Volume = 0f;
						}
						else
						{
							this._strongBlizzardSound = SlotId.Invalid;
						}
						if (activeSound4 != null)
						{
							activeSound4.Volume = 0f;
						}
						else
						{
							this._insideBlizzardSound = SlotId.Invalid;
						}
					}
				}
				catch
				{
					this._disabledBlizzardSound = true;
				}
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x0000BD30 File Offset: 0x00009F30
		private void UpdateLightDecay(SceneMetrics metrics)
		{
			float num = 1f;
			float num2 = 1f;
			num *= 1f - Main.shimmerAlpha * 0f;
			num2 *= 1f - Main.shimmerAlpha * 0.3f;
			if (Main.getGoodWorld)
			{
				if (metrics.WithinRangeOfNPC(245, 2000.0))
				{
					num *= 0.6f;
					num2 *= 0.6f;
				}
				else if (metrics.ZoneLihzhardTemple)
				{
					num *= 0.88f;
					num2 *= 0.88f;
				}
				else if (metrics.ZoneDungeon)
				{
					num *= 0.94f;
					num2 *= 0.94f;
				}
			}
			this.MoveTowards(ref this.airLightDecay, num, 0.005f);
			this.MoveTowards(ref this.solidLightDecay, num2, 0.005f);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x0000BDF8 File Offset: 0x00009FF8
		private void UpdateShimmer(SceneMetrics metrics)
		{
			bool flag = metrics.ShimmerMonolithState == 1 || metrics.ZoneShimmer || metrics.PerspectivePlayer.shimmerMonolithShader || (metrics.PerspectivePlayer.shimmering && metrics.UndergroundForShimmering);
			if (metrics.ShimmerMonolithState == 2)
			{
				flag = false;
			}
			if (flag)
			{
				this.MoveTowards(ref Main.shimmerAlpha, 1f, 0.025f);
				if (Main.shimmerAlpha >= 0.5f)
				{
					this.MoveTowards(ref Main.shimmerDarken, 1f, 0.025f);
					this._shimmerBrightenDelay = 4f;
					return;
				}
			}
			else
			{
				this.MoveTowards(ref Main.shimmerDarken, 0f, 0.05f);
				if (Main.shimmerDarken == 0f)
				{
					this.MoveTowards(ref this._shimmerBrightenDelay, 0f, 1f);
				}
				if (this._shimmerBrightenDelay == 0f)
				{
					this.MoveTowards(ref Main.shimmerAlpha, 0f, 0.05f);
				}
			}
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x0000BEE8 File Offset: 0x0000A0E8
		private void ManageSpecialBiomeVisuals(string biomeName, bool inZone, Vector2 activationSource = default(Vector2), bool alwaysInstant = false)
		{
			if (SkyManager.Instance[biomeName] != null && inZone != SkyManager.Instance[biomeName].IsActive())
			{
				if (inZone)
				{
					SkyManager.Instance.Activate(biomeName, activationSource, new object[0]);
				}
				else
				{
					SkyManager.Instance.Deactivate(biomeName, new object[0]);
				}
			}
			Filter filter = Filters.Scene[biomeName];
			Overlay overlay = Overlays.Scene[biomeName];
			if (filter != null)
			{
				if (inZone != Filters.Scene[biomeName].IsActive())
				{
					if (inZone)
					{
						Filters.Scene.Activate(biomeName, activationSource, new object[0]);
					}
					else
					{
						filter.Deactivate(new object[0]);
					}
				}
				else if (inZone)
				{
					filter.GetShader().UseTargetPosition(activationSource);
				}
			}
			if (overlay != null && inZone != (Overlays.Scene[biomeName].Mode != OverlayMode.Inactive))
			{
				if (inZone)
				{
					Overlays.Scene.Activate(biomeName, activationSource, new object[0]);
				}
				else
				{
					overlay.Deactivate(new object[0]);
				}
			}
			if (alwaysInstant || this.skipTransitions)
			{
				if (filter != null)
				{
					filter.Opacity = (inZone ? 1f : 0f);
				}
				if (overlay != null)
				{
					overlay.Opacity = (inZone ? 1f : 0f);
				}
			}
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x0000C024 File Offset: 0x0000A224
		private void UpdateGraveyard(SceneMetrics metrics)
		{
			float lerpValue = Utils.GetLerpValue((float)SceneMetrics.GraveyardTileMin, (float)SceneMetrics.GraveyardTileMax, (float)metrics.GraveyardTileCount, true);
			this.MoveTowards(ref Main.GraveyardVisualIntensity, lerpValue, 0.02f, 0.1f);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x0000C064 File Offset: 0x0000A264
		private void UpdateRGBPeriheralProbe(SceneMetrics metrics)
		{
			int highestTierBossOrEvent = 0;
			bool zoneOverworldHeight = metrics.ZoneOverworldHeight;
			if (metrics.AnyNPCs(4))
			{
				highestTierBossOrEvent = 4;
			}
			if (metrics.AnyNPCs(50))
			{
				highestTierBossOrEvent = 50;
			}
			if (zoneOverworldHeight && Main.invasionType == 1)
			{
				highestTierBossOrEvent = -1;
			}
			if (metrics.AnyNPCs(13))
			{
				highestTierBossOrEvent = 13;
			}
			if (metrics.AnyNPCs(266))
			{
				highestTierBossOrEvent = 266;
			}
			if (metrics.AnyNPCs(222))
			{
				highestTierBossOrEvent = 222;
			}
			if (metrics.AnyNPCs(35))
			{
				highestTierBossOrEvent = 35;
			}
			if (metrics.AnyNPCs(113))
			{
				highestTierBossOrEvent = 113;
			}
			if (zoneOverworldHeight && Main.invasionType == 2)
			{
				highestTierBossOrEvent = -2;
			}
			if (metrics.AnyNPCs(657))
			{
				highestTierBossOrEvent = 657;
			}
			if (metrics.AnyNPCs(126) || metrics.AnyNPCs(125))
			{
				highestTierBossOrEvent = 126;
			}
			if (metrics.AnyNPCs(134))
			{
				highestTierBossOrEvent = 134;
			}
			if (metrics.AnyNPCs(127))
			{
				highestTierBossOrEvent = 127;
			}
			if (zoneOverworldHeight && Main.invasionType == 3)
			{
				highestTierBossOrEvent = -3;
			}
			if (metrics.AnyNPCs(262))
			{
				highestTierBossOrEvent = 262;
			}
			if (metrics.AnyNPCs(245))
			{
				highestTierBossOrEvent = 245;
			}
			if (metrics.AnyNPCs(636))
			{
				highestTierBossOrEvent = 636;
			}
			if (metrics.AnyNPCs(668) && NPC.IsDeerclopsHostile())
			{
				highestTierBossOrEvent = 668;
			}
			if (DD2Event.Ongoing)
			{
				highestTierBossOrEvent = -6;
			}
			if (zoneOverworldHeight && Main.invasionType == 4)
			{
				highestTierBossOrEvent = -4;
			}
			if (metrics.AnyNPCs(439))
			{
				highestTierBossOrEvent = 439;
			}
			if (metrics.AnyNPCs(370))
			{
				highestTierBossOrEvent = 370;
			}
			if (metrics.AnyNPCs(398))
			{
				highestTierBossOrEvent = 398;
			}
			CommonConditions.Boss.HighestTierBossOrEvent = highestTierBossOrEvent;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x0000C1FA File Offset: 0x0000A3FA
		public void MoveTowards(ref float value, float target, float amount)
		{
			this.MoveTowards(ref value, target, amount, amount);
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x0000C206 File Offset: 0x0000A406
		public void MoveTowards(ref float value, float target, float inc, float dec)
		{
			if (this.skipTransitions)
			{
				value = target;
				return;
			}
			if (value < target)
			{
				value = Math.Min(value + inc, target);
				return;
			}
			if (value > target)
			{
				value = Math.Max(value - dec, target);
			}
		}

		// Token: 0x0400004F RID: 79
		public float airLightDecay;

		// Token: 0x04000050 RID: 80
		public float solidLightDecay;

		// Token: 0x04000051 RID: 81
		public float outsideWeatherEffectIntensity;

		// Token: 0x04000052 RID: 82
		private float _outsideWeatherEffectIntensityBackingValue;

		// Token: 0x04000053 RID: 83
		private float _deerclopsBlizzardSmoothedEffect;

		// Token: 0x04000054 RID: 84
		private bool _disabledBlizzardGraphic;

		// Token: 0x04000055 RID: 85
		private bool _disabledBlizzardSound;

		// Token: 0x04000056 RID: 86
		private float _blizzardSoundVolume;

		// Token: 0x04000057 RID: 87
		private SlotId _strongBlizzardSound = SlotId.Invalid;

		// Token: 0x04000058 RID: 88
		private SlotId _insideBlizzardSound = SlotId.Invalid;

		// Token: 0x04000059 RID: 89
		private float _shimmerBrightenDelay;

		// Token: 0x0400005A RID: 90
		public bool skipTransitions;
	}
}
