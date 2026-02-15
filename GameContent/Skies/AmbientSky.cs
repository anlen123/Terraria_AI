using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Utilities;
using Terraria.DataStructures;
using Terraria.GameContent.Ambience;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.Utilities;

namespace Terraria.GameContent.Skies
{
	// Token: 0x02000448 RID: 1096
	public class AmbientSky : CustomSky
	{
		// Token: 0x060031C4 RID: 12740 RVA: 0x005E3298 File Offset: 0x005E1498
		public override void Activate(Vector2 position, params object[] args)
		{
			this._isActive = true;
		}

		// Token: 0x060031C5 RID: 12741 RVA: 0x005E32A1 File Offset: 0x005E14A1
		public override void Deactivate(params object[] args)
		{
			this._isActive = false;
		}

		// Token: 0x060031C6 RID: 12742 RVA: 0x005E32AA File Offset: 0x005E14AA
		private bool AnActiveSkyConflictsWithAmbience()
		{
			return SkyManager.Instance["MonolithMoonLord"].IsActive() || SkyManager.Instance["MoonLord"].IsActive();
		}

		// Token: 0x060031C7 RID: 12743 RVA: 0x005E32D8 File Offset: 0x005E14D8
		public override void Update(GameTime gameTime)
		{
			if (Main.gamePaused)
			{
				return;
			}
			this._frameCounter++;
			if (Main.netMode != 2 && this.AnActiveSkyConflictsWithAmbience() && SkyManager.Instance["Ambience"].IsActive())
			{
				SkyManager.Instance.Deactivate("Ambience", new object[0]);
			}
			foreach (SlotVector<AmbientSky.SkyEntity>.ItemPair itemPair in this._entities)
			{
				AmbientSky.SkyEntity value = itemPair.Value;
				value.Update(this._frameCounter);
				if (!value.IsActive)
				{
					this._entities.Remove(itemPair.Id);
					if (Main.netMode != 2 && this._entities.Count == 0 && SkyManager.Instance["Ambience"].IsActive())
					{
						SkyManager.Instance.Deactivate("Ambience", new object[0]);
					}
				}
			}
		}

		// Token: 0x060031C8 RID: 12744 RVA: 0x005E33DC File Offset: 0x005E15DC
		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			if (Main.gameMenu && Main.netMode == 0 && SkyManager.Instance["Ambience"].IsActive())
			{
				this._entities.Clear();
				SkyManager.Instance.Deactivate("Ambience", new object[0]);
			}
			foreach (SlotVector<AmbientSky.SkyEntity>.ItemPair itemPair in this._entities)
			{
				itemPair.Value.Draw(spriteBatch, 3f, minDepth, maxDepth);
			}
		}

		// Token: 0x060031C9 RID: 12745 RVA: 0x005E3478 File Offset: 0x005E1678
		public override bool IsActive()
		{
			return this._isActive;
		}

		// Token: 0x060031CA RID: 12746 RVA: 0x00009E06 File Offset: 0x00008006
		public override void Reset()
		{
		}

		// Token: 0x060031CB RID: 12747 RVA: 0x005E3480 File Offset: 0x005E1680
		public void Spawn(Player player, SkyEntityType type, int seed)
		{
			FastRandom random = new FastRandom(seed);
			switch (type)
			{
			case SkyEntityType.BirdsV:
				this._entities.Add(new AmbientSky.BirdsPackSkyEntity(player, random));
				break;
			case SkyEntityType.Wyvern:
				this._entities.Add(new AmbientSky.WyvernSkyEntity(player, random));
				break;
			case SkyEntityType.Airship:
				this._entities.Add(new AmbientSky.AirshipSkyEntity(player, random));
				break;
			case SkyEntityType.AirBalloon:
				this._entities.Add(new AmbientSky.AirBalloonSkyEntity(player, random));
				break;
			case SkyEntityType.Eyeball:
				this._entities.Add(new AmbientSky.EOCSkyEntity(player, random));
				break;
			case SkyEntityType.Meteor:
				this._entities.Add(new AmbientSky.MeteorSkyEntity(player, random));
				break;
			case SkyEntityType.Bats:
			{
				List<AmbientSky.BatsGroupSkyEntity> list = AmbientSky.BatsGroupSkyEntity.CreateGroup(player, random);
				for (int i = 0; i < list.Count; i++)
				{
					this._entities.Add(list[i]);
				}
				break;
			}
			case SkyEntityType.Butterflies:
				this._entities.Add(new AmbientSky.ButterfliesSkyEntity(player, random));
				break;
			case SkyEntityType.LostKite:
				this._entities.Add(new AmbientSky.LostKiteSkyEntity(player, random));
				break;
			case SkyEntityType.Vulture:
				this._entities.Add(new AmbientSky.VultureSkyEntity(player, random));
				break;
			case SkyEntityType.PixiePosse:
				this._entities.Add(new AmbientSky.PixiePosseSkyEntity(player, random));
				break;
			case SkyEntityType.Seagulls:
			{
				List<AmbientSky.SeagullsGroupSkyEntity> list2 = AmbientSky.SeagullsGroupSkyEntity.CreateGroup(player, random);
				for (int j = 0; j < list2.Count; j++)
				{
					this._entities.Add(list2[j]);
				}
				break;
			}
			case SkyEntityType.SlimeBalloons:
			{
				List<AmbientSky.SlimeBalloonGroupSkyEntity> list3 = AmbientSky.SlimeBalloonGroupSkyEntity.CreateGroup(player, random);
				for (int k = 0; k < list3.Count; k++)
				{
					this._entities.Add(list3[k]);
				}
				break;
			}
			case SkyEntityType.Gastropods:
			{
				List<AmbientSky.GastropodGroupSkyEntity> list4 = AmbientSky.GastropodGroupSkyEntity.CreateGroup(player, random);
				for (int l = 0; l < list4.Count; l++)
				{
					this._entities.Add(list4[l]);
				}
				break;
			}
			case SkyEntityType.Pegasus:
				this._entities.Add(new AmbientSky.PegasusSkyEntity(player, random));
				break;
			case SkyEntityType.EaterOfSouls:
				this._entities.Add(new AmbientSky.EOSSkyEntity(player, random));
				break;
			case SkyEntityType.Crimera:
				this._entities.Add(new AmbientSky.CrimeraSkyEntity(player, random));
				break;
			case SkyEntityType.Hellbats:
			{
				List<AmbientSky.HellBatsGoupSkyEntity> list5 = AmbientSky.HellBatsGoupSkyEntity.CreateGroup(player, random);
				for (int m = 0; m < list5.Count; m++)
				{
					this._entities.Add(list5[m]);
				}
				break;
			}
			}
			if (Main.netMode != 2 && !this.AnActiveSkyConflictsWithAmbience() && !SkyManager.Instance["Ambience"].IsActive())
			{
				SkyManager.Instance.Activate("Ambience", default(Vector2), new object[0]);
			}
		}

		// Token: 0x04005795 RID: 22421
		private bool _isActive;

		// Token: 0x04005796 RID: 22422
		private readonly SlotVector<AmbientSky.SkyEntity> _entities = new SlotVector<AmbientSky.SkyEntity>(500);

		// Token: 0x04005797 RID: 22423
		private int _frameCounter;

		// Token: 0x02000948 RID: 2376
		private abstract class SkyEntity
		{
			// Token: 0x17000581 RID: 1409
			// (get) Token: 0x06004854 RID: 18516 RVA: 0x006CB8CD File Offset: 0x006C9ACD
			public Rectangle SourceRectangle
			{
				get
				{
					return this.Frame.GetSourceRectangle(this.Texture.Value);
				}
			}

			// Token: 0x06004855 RID: 18517 RVA: 0x006CB8E5 File Offset: 0x006C9AE5
			protected void NextFrame()
			{
				this.Frame.CurrentRow = (this.Frame.CurrentRow + 1) % this.Frame.RowCount;
			}

			// Token: 0x06004856 RID: 18518
			public abstract Color GetColor(Color backgroundColor);

			// Token: 0x06004857 RID: 18519
			public abstract void Update(int frameCount);

			// Token: 0x06004858 RID: 18520 RVA: 0x006CB90C File Offset: 0x006C9B0C
			protected void SetPositionInWorldBasedOnScreenSpace(Vector2 actualWorldSpace)
			{
				Vector2 value = actualWorldSpace - Main.Camera.Center;
				Vector2 position = Main.Camera.Center + value * (this.Depth / 3f);
				this.Position = position;
			}

			// Token: 0x06004859 RID: 18521
			public abstract Vector2 GetDrawPosition();

			// Token: 0x0600485A RID: 18522 RVA: 0x006CB953 File Offset: 0x006C9B53
			public virtual void Draw(SpriteBatch spriteBatch, float depthScale, float minDepth, float maxDepth)
			{
				this.CommonDraw(spriteBatch, depthScale, minDepth, maxDepth);
			}

			// Token: 0x0600485B RID: 18523 RVA: 0x006CB960 File Offset: 0x006C9B60
			public void CommonDraw(SpriteBatch spriteBatch, float depthScale, float minDepth, float maxDepth)
			{
				if (this.Depth <= minDepth || this.Depth > maxDepth)
				{
					return;
				}
				Vector2 drawPositionByDepth = this.GetDrawPositionByDepth();
				Color color = this.GetColor(Main.ColorOfTheSkies) * Main.atmo;
				Vector2 origin = this.SourceRectangle.Size() / 2f;
				float scale = depthScale / this.Depth;
				spriteBatch.Draw(this.Texture.Value, drawPositionByDepth - Main.Camera.UnscaledPosition, new Rectangle?(this.SourceRectangle), color, this.Rotation, origin, scale, this.Effects, 0f);
			}

			// Token: 0x0600485C RID: 18524 RVA: 0x006CBA00 File Offset: 0x006C9C00
			internal Vector2 GetDrawPositionByDepth()
			{
				return (this.GetDrawPosition() - Main.Camera.Center) * new Vector2(1f / this.Depth, 0.9f / this.Depth) + Main.Camera.Center;
			}

			// Token: 0x0600485D RID: 18525 RVA: 0x006CBA54 File Offset: 0x006C9C54
			internal float Helper_GetOpacityWithAccountingForOceanWaterLine()
			{
				ref Vector2 ptr = this.GetDrawPositionByDepth() - Main.Camera.UnscaledPosition;
				int num = this.SourceRectangle.Height / 2;
				float t = ptr.Y + (float)num;
				float yscreenPosition = AmbientSkyDrawCache.Instance.OceanLineInfo.YScreenPosition;
				float num2 = Utils.GetLerpValue(yscreenPosition - 10f, yscreenPosition - 2f, t, true);
				num2 *= AmbientSkyDrawCache.Instance.OceanLineInfo.OceanOpacity;
				return 1f - num2;
			}

			// Token: 0x04007531 RID: 30001
			public Vector2 Position;

			// Token: 0x04007532 RID: 30002
			public Asset<Texture2D> Texture;

			// Token: 0x04007533 RID: 30003
			public SpriteFrame Frame;

			// Token: 0x04007534 RID: 30004
			public float Depth;

			// Token: 0x04007535 RID: 30005
			public SpriteEffects Effects;

			// Token: 0x04007536 RID: 30006
			public bool IsActive = true;

			// Token: 0x04007537 RID: 30007
			public float Rotation;
		}

		// Token: 0x02000949 RID: 2377
		private class FadingSkyEntity : AmbientSky.SkyEntity
		{
			// Token: 0x0600485F RID: 18527 RVA: 0x006CBADC File Offset: 0x006C9CDC
			public FadingSkyEntity()
			{
				this.Opacity = 0f;
				this.TimeEntitySpawnedIn = -1;
				this.BrightnessLerper = 0f;
				this.FinalOpacityMultiplier = 1f;
				this.OpacityNormalizedTimeToFadeIn = 0.1f;
				this.OpacityNormalizedTimeToFadeOut = 0.9f;
			}

			// Token: 0x06004860 RID: 18528 RVA: 0x006CBB30 File Offset: 0x006C9D30
			public override void Update(int frameCount)
			{
				if (this.IsMovementDone(frameCount))
				{
					return;
				}
				this.UpdateOpacity(frameCount);
				if ((frameCount + this.FrameOffset) % this.FramingSpeed == 0)
				{
					base.NextFrame();
				}
				this.UpdateVelocity(frameCount);
				this.Position += this.Velocity;
			}

			// Token: 0x06004861 RID: 18529 RVA: 0x00009E06 File Offset: 0x00008006
			public virtual void UpdateVelocity(int frameCount)
			{
			}

			// Token: 0x06004862 RID: 18530 RVA: 0x006CBB84 File Offset: 0x006C9D84
			private void UpdateOpacity(int frameCount)
			{
				int num = frameCount - this.TimeEntitySpawnedIn;
				if ((float)num >= (float)this.LifeTime * this.OpacityNormalizedTimeToFadeOut)
				{
					this.Opacity = Utils.GetLerpValue((float)this.LifeTime, (float)this.LifeTime * this.OpacityNormalizedTimeToFadeOut, (float)num, true);
					return;
				}
				this.Opacity = Utils.GetLerpValue(0f, (float)this.LifeTime * this.OpacityNormalizedTimeToFadeIn, (float)num, true);
			}

			// Token: 0x06004863 RID: 18531 RVA: 0x006CBBF1 File Offset: 0x006C9DF1
			private bool IsMovementDone(int frameCount)
			{
				if (this.TimeEntitySpawnedIn == -1)
				{
					this.TimeEntitySpawnedIn = frameCount;
				}
				if (frameCount - this.TimeEntitySpawnedIn >= this.LifeTime)
				{
					this.IsActive = false;
					return true;
				}
				return false;
			}

			// Token: 0x06004864 RID: 18532 RVA: 0x006CBC1D File Offset: 0x006C9E1D
			public override Color GetColor(Color backgroundColor)
			{
				return Color.Lerp(backgroundColor, Color.White, this.BrightnessLerper) * this.Opacity * this.FinalOpacityMultiplier * base.Helper_GetOpacityWithAccountingForOceanWaterLine();
			}

			// Token: 0x06004865 RID: 18533 RVA: 0x006CBC54 File Offset: 0x006C9E54
			public void StartFadingOut(int currentFrameCount)
			{
				int num = (int)((float)this.LifeTime * this.OpacityNormalizedTimeToFadeOut);
				int num2 = currentFrameCount - num;
				if (num2 < this.TimeEntitySpawnedIn)
				{
					this.TimeEntitySpawnedIn = num2;
				}
			}

			// Token: 0x06004866 RID: 18534 RVA: 0x006CBC85 File Offset: 0x006C9E85
			public override Vector2 GetDrawPosition()
			{
				return this.Position;
			}

			// Token: 0x04007538 RID: 30008
			protected int LifeTime;

			// Token: 0x04007539 RID: 30009
			protected Vector2 Velocity;

			// Token: 0x0400753A RID: 30010
			protected int FramingSpeed;

			// Token: 0x0400753B RID: 30011
			protected int TimeEntitySpawnedIn;

			// Token: 0x0400753C RID: 30012
			protected float Opacity;

			// Token: 0x0400753D RID: 30013
			protected float BrightnessLerper;

			// Token: 0x0400753E RID: 30014
			protected float FinalOpacityMultiplier;

			// Token: 0x0400753F RID: 30015
			protected float OpacityNormalizedTimeToFadeIn;

			// Token: 0x04007540 RID: 30016
			protected float OpacityNormalizedTimeToFadeOut;

			// Token: 0x04007541 RID: 30017
			protected int FrameOffset;
		}

		// Token: 0x0200094A RID: 2378
		private class ButterfliesSkyEntity : AmbientSky.FadingSkyEntity
		{
			// Token: 0x06004867 RID: 18535 RVA: 0x006CBC90 File Offset: 0x006C9E90
			public ButterfliesSkyEntity(Player player, FastRandom random)
			{
				VirtualCamera virtualCamera = new VirtualCamera(player);
				this.Effects = ((Main.WindForVisuals > 0f) ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
				int num = 100;
				if (this.Effects == SpriteEffects.FlipHorizontally)
				{
					this.Position.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
				}
				else
				{
					this.Position.X = virtualCamera.Position.X - (float)num;
				}
				this.Position.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 4000f) + 4000f;
				this.Depth = random.NextFloat() * 3f + 3f;
				base.SetPositionInWorldBasedOnScreenSpace(this.Position);
				int num2 = random.Next(2) + 1;
				this.Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/ButterflySwarm" + num2, 1);
				this.Frame = new SpriteFrame(1, (num2 == 2) ? 19 : 17);
				this.LifeTime = random.Next(60, 121) * 60;
				this.OpacityNormalizedTimeToFadeIn = 0.15f;
				this.OpacityNormalizedTimeToFadeOut = 0.85f;
				this.BrightnessLerper = 0.2f;
				this.FinalOpacityMultiplier = 1f;
				this.FramingSpeed = 5;
			}

			// Token: 0x06004868 RID: 18536 RVA: 0x006CBDEC File Offset: 0x006C9FEC
			public override void UpdateVelocity(int frameCount)
			{
				float num = 0.1f + Math.Abs(Main.WindForVisuals) * 0.05f;
				this.Velocity = new Vector2(num * (float)((this.Effects == SpriteEffects.FlipHorizontally) ? -1 : 1), 0f);
			}

			// Token: 0x06004869 RID: 18537 RVA: 0x006CBE30 File Offset: 0x006CA030
			public override void Update(int frameCount)
			{
				base.Update(frameCount);
				if (Main.IsItRaining || !Main.dayTime || Main.eclipse)
				{
					base.StartFadingOut(frameCount);
				}
			}
		}

		// Token: 0x0200094B RID: 2379
		private class LostKiteSkyEntity : AmbientSky.FadingSkyEntity
		{
			// Token: 0x0600486A RID: 18538 RVA: 0x006CBE58 File Offset: 0x006CA058
			public LostKiteSkyEntity(Player player, FastRandom random)
			{
				VirtualCamera virtualCamera = new VirtualCamera(player);
				this.Effects = ((Main.WindForVisuals > 0f) ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
				int num = 100;
				if (this.Effects == SpriteEffects.FlipHorizontally)
				{
					this.Position.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
				}
				else
				{
					this.Position.X = virtualCamera.Position.X - (float)num;
				}
				this.Position.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 2400f) + 2400f;
				this.Depth = random.NextFloat() * 3f + 3f;
				base.SetPositionInWorldBasedOnScreenSpace(this.Position);
				this.Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/LostKite", 1);
				this.Frame = new SpriteFrame(1, 42);
				this.LifeTime = random.Next(60, 121) * 60;
				this.OpacityNormalizedTimeToFadeIn = 0.15f;
				this.OpacityNormalizedTimeToFadeOut = 0.85f;
				this.BrightnessLerper = 0.2f;
				this.FinalOpacityMultiplier = 1f;
				this.FramingSpeed = 6;
				int num2 = random.Next((int)this.Frame.RowCount);
				for (int i = 0; i < num2; i++)
				{
					base.NextFrame();
				}
			}

			// Token: 0x0600486B RID: 18539 RVA: 0x006CBFC0 File Offset: 0x006CA1C0
			public override void UpdateVelocity(int frameCount)
			{
				float num = 1.2f + Math.Abs(Main.WindForVisuals) * 3f;
				if (Main.IsItStorming)
				{
					num *= 1.5f;
				}
				this.Velocity = new Vector2(num * (float)((this.Effects == SpriteEffects.FlipHorizontally) ? -1 : 1), 0f);
			}

			// Token: 0x0600486C RID: 18540 RVA: 0x006CC014 File Offset: 0x006CA214
			public override void Update(int frameCount)
			{
				if (Main.IsItStorming)
				{
					this.FramingSpeed = 4;
				}
				this.Effects = ((Main.WindForVisuals > 0f) ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
				base.Update(frameCount);
				if (!Main.dayTime || Main.eclipse)
				{
					base.StartFadingOut(frameCount);
				}
			}
		}

		// Token: 0x0200094C RID: 2380
		private class PegasusSkyEntity : AmbientSky.FadingSkyEntity
		{
			// Token: 0x0600486D RID: 18541 RVA: 0x006CC064 File Offset: 0x006CA264
			public PegasusSkyEntity(Player player, FastRandom random)
			{
				VirtualCamera virtualCamera = new VirtualCamera(player);
				this.Effects = ((Main.WindForVisuals > 0f) ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
				int num = 100;
				if (this.Effects == SpriteEffects.FlipHorizontally)
				{
					this.Position.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
				}
				else
				{
					this.Position.X = virtualCamera.Position.X - (float)num;
				}
				this.Position.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 2400f) + 2400f;
				this.Depth = random.NextFloat() * 3f + 3f;
				base.SetPositionInWorldBasedOnScreenSpace(this.Position);
				this.Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/Pegasus", 1);
				this.Frame = new SpriteFrame(1, 11);
				this.LifeTime = random.Next(60, 121) * 60;
				this.OpacityNormalizedTimeToFadeIn = 0.15f;
				this.OpacityNormalizedTimeToFadeOut = 0.85f;
				this.BrightnessLerper = 0.2f;
				this.FinalOpacityMultiplier = 1f;
				this.FramingSpeed = 5;
			}

			// Token: 0x0600486E RID: 18542 RVA: 0x006CC1A8 File Offset: 0x006CA3A8
			public override void UpdateVelocity(int frameCount)
			{
				float num = 1.5f + Math.Abs(Main.WindForVisuals) * 0.6f;
				this.Velocity = new Vector2(num * (float)((this.Effects == SpriteEffects.FlipHorizontally) ? -1 : 1), 0f);
			}

			// Token: 0x0600486F RID: 18543 RVA: 0x006CBE30 File Offset: 0x006CA030
			public override void Update(int frameCount)
			{
				base.Update(frameCount);
				if (Main.IsItRaining || !Main.dayTime || Main.eclipse)
				{
					base.StartFadingOut(frameCount);
				}
			}

			// Token: 0x06004870 RID: 18544 RVA: 0x006CC1EC File Offset: 0x006CA3EC
			public override Color GetColor(Color backgroundColor)
			{
				return base.GetColor(backgroundColor) * Main.bgAlphaFrontLayer[6];
			}
		}

		// Token: 0x0200094D RID: 2381
		private class VultureSkyEntity : AmbientSky.FadingSkyEntity
		{
			// Token: 0x06004871 RID: 18545 RVA: 0x006CC204 File Offset: 0x006CA404
			public VultureSkyEntity(Player player, FastRandom random)
			{
				VirtualCamera virtualCamera = new VirtualCamera(player);
				this.Effects = ((Main.WindForVisuals > 0f) ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
				int num = 100;
				if (this.Effects == SpriteEffects.FlipHorizontally)
				{
					this.Position.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
				}
				else
				{
					this.Position.X = virtualCamera.Position.X - (float)num;
				}
				this.Position.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 2400f) + 2400f;
				this.Depth = random.NextFloat() * 3f + 3f;
				base.SetPositionInWorldBasedOnScreenSpace(this.Position);
				this.Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/Vulture", 1);
				this.Frame = new SpriteFrame(1, 10);
				this.LifeTime = random.Next(60, 121) * 60;
				this.OpacityNormalizedTimeToFadeIn = 0.15f;
				this.OpacityNormalizedTimeToFadeOut = 0.85f;
				this.BrightnessLerper = 0.2f;
				this.FinalOpacityMultiplier = 1f;
				this.FramingSpeed = 5;
			}

			// Token: 0x06004872 RID: 18546 RVA: 0x006CC348 File Offset: 0x006CA548
			public override void UpdateVelocity(int frameCount)
			{
				float num = 3f + Math.Abs(Main.WindForVisuals) * 0.8f;
				this.Velocity = new Vector2(num * (float)((this.Effects == SpriteEffects.FlipHorizontally) ? -1 : 1), 0f);
			}

			// Token: 0x06004873 RID: 18547 RVA: 0x006CBE30 File Offset: 0x006CA030
			public override void Update(int frameCount)
			{
				base.Update(frameCount);
				if (Main.IsItRaining || !Main.dayTime || Main.eclipse)
				{
					base.StartFadingOut(frameCount);
				}
			}

			// Token: 0x06004874 RID: 18548 RVA: 0x006CC38C File Offset: 0x006CA58C
			public override Color GetColor(Color backgroundColor)
			{
				float num = Math.Max(Main.bgAlphaFrontLayer[5], Main.bgAlphaFrontLayer[14]);
				num = Math.Max(num, Main.bgAlphaFrontLayer[13]);
				return base.GetColor(backgroundColor) * Math.Max(Main.bgAlphaFrontLayer[2], num);
			}
		}

		// Token: 0x0200094E RID: 2382
		private class PixiePosseSkyEntity : AmbientSky.FadingSkyEntity
		{
			// Token: 0x06004875 RID: 18549 RVA: 0x006CC3D8 File Offset: 0x006CA5D8
			public PixiePosseSkyEntity(Player player, FastRandom random)
			{
				VirtualCamera virtualCamera = new VirtualCamera(player);
				this.Effects = ((Main.WindForVisuals > 0f) ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
				int num = 100;
				if (this.Effects == SpriteEffects.FlipHorizontally)
				{
					this.Position.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
				}
				else
				{
					this.Position.X = virtualCamera.Position.X - (float)num;
				}
				this.Position.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 4000f) + 4000f;
				this.Depth = random.NextFloat() * 3f + 2f;
				base.SetPositionInWorldBasedOnScreenSpace(this.Position);
				if (!Main.dayTime)
				{
					this.pixieType = 2;
				}
				this.Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/PixiePosse" + this.pixieType, 1);
				this.Frame = new SpriteFrame(1, 25);
				this.LifeTime = random.Next(60, 121) * 60;
				this.OpacityNormalizedTimeToFadeIn = 0.15f;
				this.OpacityNormalizedTimeToFadeOut = 0.85f;
				this.BrightnessLerper = 0.6f;
				this.FinalOpacityMultiplier = 1f;
				this.FramingSpeed = 5;
			}

			// Token: 0x06004876 RID: 18550 RVA: 0x006CC538 File Offset: 0x006CA738
			public override void UpdateVelocity(int frameCount)
			{
				float num = 0.12f + Math.Abs(Main.WindForVisuals) * 0.08f;
				this.Velocity = new Vector2(num * (float)((this.Effects == SpriteEffects.FlipHorizontally) ? -1 : 1), 0f);
			}

			// Token: 0x06004877 RID: 18551 RVA: 0x006CC57C File Offset: 0x006CA77C
			public override void Update(int frameCount)
			{
				base.Update(frameCount);
				if ((this.pixieType == 1 && !Main.dayTime) || (this.pixieType == 2 && Main.dayTime) || Main.IsItRaining || Main.eclipse || Main.bloodMoon || Main.pumpkinMoon || Main.snowMoon)
				{
					base.StartFadingOut(frameCount);
				}
			}

			// Token: 0x06004878 RID: 18552 RVA: 0x006CC5DA File Offset: 0x006CA7DA
			public override void Draw(SpriteBatch spriteBatch, float depthScale, float minDepth, float maxDepth)
			{
				base.CommonDraw(spriteBatch, depthScale - 0.1f, minDepth, maxDepth);
			}

			// Token: 0x04007542 RID: 30018
			private int pixieType = 1;
		}

		// Token: 0x0200094F RID: 2383
		private class BirdsPackSkyEntity : AmbientSky.FadingSkyEntity
		{
			// Token: 0x06004879 RID: 18553 RVA: 0x006CC5F0 File Offset: 0x006CA7F0
			public BirdsPackSkyEntity(Player player, FastRandom random)
			{
				VirtualCamera virtualCamera = new VirtualCamera(player);
				this.Effects = ((Main.WindForVisuals > 0f) ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
				int num = 100;
				if (this.Effects == SpriteEffects.FlipHorizontally)
				{
					this.Position.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
				}
				else
				{
					this.Position.X = virtualCamera.Position.X - (float)num;
				}
				this.Position.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 2400f) + 2400f;
				this.Depth = random.NextFloat() * 3f + 3f;
				base.SetPositionInWorldBasedOnScreenSpace(this.Position);
				this.Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/BirdsVShape", 1);
				this.Frame = new SpriteFrame(1, 4);
				this.LifeTime = random.Next(60, 121) * 60;
				this.OpacityNormalizedTimeToFadeIn = 0.15f;
				this.OpacityNormalizedTimeToFadeOut = 0.85f;
				this.BrightnessLerper = 0.2f;
				this.FinalOpacityMultiplier = 1f;
				this.FramingSpeed = 5;
			}

			// Token: 0x0600487A RID: 18554 RVA: 0x006CC730 File Offset: 0x006CA930
			public override void UpdateVelocity(int frameCount)
			{
				float num = 3f + Math.Abs(Main.WindForVisuals) * 0.8f;
				this.Velocity = new Vector2(num * (float)((this.Effects == SpriteEffects.FlipHorizontally) ? -1 : 1), 0f);
			}

			// Token: 0x0600487B RID: 18555 RVA: 0x006CBE30 File Offset: 0x006CA030
			public override void Update(int frameCount)
			{
				base.Update(frameCount);
				if (Main.IsItRaining || !Main.dayTime || Main.eclipse)
				{
					base.StartFadingOut(frameCount);
				}
			}
		}

		// Token: 0x02000950 RID: 2384
		private class SeagullsGroupSkyEntity : AmbientSky.FadingSkyEntity
		{
			// Token: 0x0600487C RID: 18556 RVA: 0x006CC774 File Offset: 0x006CA974
			public SeagullsGroupSkyEntity(Player player, FastRandom random)
			{
				VirtualCamera virtualCamera = new VirtualCamera(player);
				this.Effects = ((Main.WindForVisuals > 0f) ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
				int num = 100;
				if (this.Effects == SpriteEffects.FlipHorizontally)
				{
					this.Position.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
				}
				else
				{
					this.Position.X = virtualCamera.Position.X - (float)num;
				}
				this.Position.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 2400f) + 2400f;
				this.Depth = random.NextFloat() * 3f + 3f;
				base.SetPositionInWorldBasedOnScreenSpace(this.Position);
				this.Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/Seagull", 1);
				this.Frame = new SpriteFrame(1, 9);
				this.LifeTime = random.Next(60, 121) * 60;
				this.OpacityNormalizedTimeToFadeIn = 0.1f;
				this.OpacityNormalizedTimeToFadeOut = 0.9f;
				this.BrightnessLerper = 0.2f;
				this.FinalOpacityMultiplier = 1f;
				this.FramingSpeed = 4;
				this.FrameOffset = random.Next(0, (int)this.Frame.RowCount);
				int num2 = random.Next((int)this.Frame.RowCount);
				for (int i = 0; i < num2; i++)
				{
					base.NextFrame();
				}
			}

			// Token: 0x0600487D RID: 18557 RVA: 0x006CC8F4 File Offset: 0x006CAAF4
			public override void UpdateVelocity(int frameCount)
			{
				Vector2 value = this._magnetAccelerations * new Vector2((float)Math.Sign(this._magnetPointTarget.X - this._positionVsMagnet.X), (float)Math.Sign(this._magnetPointTarget.Y - this._positionVsMagnet.Y));
				this._velocityVsMagnet += value;
				this._positionVsMagnet += this._velocityVsMagnet;
				float x = 4f * (float)((this.Effects == SpriteEffects.FlipHorizontally) ? -1 : 1);
				this.Velocity = new Vector2(x, 0f) + this._velocityVsMagnet;
			}

			// Token: 0x0600487E RID: 18558 RVA: 0x006CBE30 File Offset: 0x006CA030
			public override void Update(int frameCount)
			{
				base.Update(frameCount);
				if (Main.IsItRaining || !Main.dayTime || Main.eclipse)
				{
					base.StartFadingOut(frameCount);
				}
			}

			// Token: 0x0600487F RID: 18559 RVA: 0x006CC9A6 File Offset: 0x006CABA6
			public void SetMagnetization(Vector2 accelerations, Vector2 targetOffset)
			{
				this._magnetAccelerations = accelerations;
				this._magnetPointTarget = targetOffset;
			}

			// Token: 0x06004880 RID: 18560 RVA: 0x006CC9B6 File Offset: 0x006CABB6
			public override Color GetColor(Color backgroundColor)
			{
				return base.GetColor(backgroundColor) * Main.bgAlphaFrontLayer[4];
			}

			// Token: 0x06004881 RID: 18561 RVA: 0x006CC9CB File Offset: 0x006CABCB
			public override void Draw(SpriteBatch spriteBatch, float depthScale, float minDepth, float maxDepth)
			{
				base.CommonDraw(spriteBatch, depthScale - 1.5f, minDepth, maxDepth);
			}

			// Token: 0x06004882 RID: 18562 RVA: 0x006CC9E0 File Offset: 0x006CABE0
			public static List<AmbientSky.SeagullsGroupSkyEntity> CreateGroup(Player player, FastRandom random)
			{
				List<AmbientSky.SeagullsGroupSkyEntity> list = new List<AmbientSky.SeagullsGroupSkyEntity>();
				int num = 100;
				int num2 = random.Next(5, 9);
				float scaleFactor = 100f;
				VirtualCamera virtualCamera = new VirtualCamera(player);
				SpriteEffects spriteEffects = (Main.WindForVisuals > 0f) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
				Vector2 value = default(Vector2);
				if (spriteEffects == SpriteEffects.FlipHorizontally)
				{
					value.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
				}
				else
				{
					value.X = virtualCamera.Position.X - (float)num;
				}
				value.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 2400f) + 2400f;
				float num3 = random.NextFloat() * 2f + 1f;
				int num4 = random.Next(30, 61) * 60;
				Vector2 value2 = new Vector2(random.NextFloat() * 0.5f + 0.5f, random.NextFloat() * 0.5f + 0.5f);
				Vector2 targetOffset = new Vector2(random.NextFloat() * 2f - 1f, random.NextFloat() * 2f - 1f) * scaleFactor;
				for (int i = 0; i < num2; i++)
				{
					AmbientSky.SeagullsGroupSkyEntity seagullsGroupSkyEntity = new AmbientSky.SeagullsGroupSkyEntity(player, random);
					seagullsGroupSkyEntity.Depth = num3 + random.NextFloat() * 0.5f;
					seagullsGroupSkyEntity.Position = value + new Vector2(random.NextFloat() * 20f - 10f, random.NextFloat() * 3f) * 50f;
					seagullsGroupSkyEntity.Effects = spriteEffects;
					seagullsGroupSkyEntity.SetPositionInWorldBasedOnScreenSpace(seagullsGroupSkyEntity.Position);
					seagullsGroupSkyEntity.LifeTime = num4 + random.Next(301);
					seagullsGroupSkyEntity.SetMagnetization(value2 * (random.NextFloat() * 0.3f + 0.85f) * 0.05f, targetOffset);
					list.Add(seagullsGroupSkyEntity);
				}
				return list;
			}

			// Token: 0x04007543 RID: 30019
			private Vector2 _magnetAccelerations;

			// Token: 0x04007544 RID: 30020
			private Vector2 _magnetPointTarget;

			// Token: 0x04007545 RID: 30021
			private Vector2 _positionVsMagnet;

			// Token: 0x04007546 RID: 30022
			private Vector2 _velocityVsMagnet;
		}

		// Token: 0x02000951 RID: 2385
		private class GastropodGroupSkyEntity : AmbientSky.FadingSkyEntity
		{
			// Token: 0x06004883 RID: 18563 RVA: 0x006CCBF4 File Offset: 0x006CADF4
			public GastropodGroupSkyEntity(Player player, FastRandom random)
			{
				VirtualCamera virtualCamera = new VirtualCamera(player);
				this.Effects = ((Main.WindForVisuals > 0f) ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
				int num = 100;
				if (this.Effects == SpriteEffects.FlipHorizontally)
				{
					this.Position.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
				}
				else
				{
					this.Position.X = virtualCamera.Position.X - (float)num;
				}
				this.Position.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 3200f) + 3200f;
				this.Depth = random.NextFloat() * 3f + 2f;
				base.SetPositionInWorldBasedOnScreenSpace(this.Position);
				this.Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/Gastropod", 1);
				this.Frame = new SpriteFrame(1, 1);
				this.LifeTime = random.Next(60, 121) * 60;
				this.OpacityNormalizedTimeToFadeIn = 0.1f;
				this.OpacityNormalizedTimeToFadeOut = 0.9f;
				this.BrightnessLerper = 0.75f;
				this.FinalOpacityMultiplier = 1f;
				this.FramingSpeed = int.MaxValue;
			}

			// Token: 0x06004884 RID: 18564 RVA: 0x006CCD38 File Offset: 0x006CAF38
			public override void UpdateVelocity(int frameCount)
			{
				Vector2 value = this._magnetAccelerations * new Vector2((float)Math.Sign(this._magnetPointTarget.X - this._positionVsMagnet.X), (float)Math.Sign(this._magnetPointTarget.Y - this._positionVsMagnet.Y));
				this._velocityVsMagnet += value;
				this._positionVsMagnet += this._velocityVsMagnet;
				float x = (1.5f + Math.Abs(Main.WindForVisuals) * 0.2f) * (float)((this.Effects == SpriteEffects.FlipHorizontally) ? -1 : 1);
				this.Velocity = new Vector2(x, 0f) + this._velocityVsMagnet;
				this.Rotation = this.Velocity.X * 0.1f;
			}

			// Token: 0x06004885 RID: 18565 RVA: 0x006CCE12 File Offset: 0x006CB012
			public override void Update(int frameCount)
			{
				base.Update(frameCount);
				if (Main.IsItRaining || Main.dayTime || Main.bloodMoon || Main.pumpkinMoon || Main.snowMoon)
				{
					base.StartFadingOut(frameCount);
				}
			}

			// Token: 0x06004886 RID: 18566 RVA: 0x006CCE45 File Offset: 0x006CB045
			public override Color GetColor(Color backgroundColor)
			{
				return Color.Lerp(backgroundColor, Colors.AmbientNPCGastropodLight, this.BrightnessLerper) * this.Opacity * this.FinalOpacityMultiplier;
			}

			// Token: 0x06004887 RID: 18567 RVA: 0x006CC5DA File Offset: 0x006CA7DA
			public override void Draw(SpriteBatch spriteBatch, float depthScale, float minDepth, float maxDepth)
			{
				base.CommonDraw(spriteBatch, depthScale - 0.1f, minDepth, maxDepth);
			}

			// Token: 0x06004888 RID: 18568 RVA: 0x006CCE6E File Offset: 0x006CB06E
			public void SetMagnetization(Vector2 accelerations, Vector2 targetOffset)
			{
				this._magnetAccelerations = accelerations;
				this._magnetPointTarget = targetOffset;
			}

			// Token: 0x06004889 RID: 18569 RVA: 0x006CCE80 File Offset: 0x006CB080
			public static List<AmbientSky.GastropodGroupSkyEntity> CreateGroup(Player player, FastRandom random)
			{
				List<AmbientSky.GastropodGroupSkyEntity> list = new List<AmbientSky.GastropodGroupSkyEntity>();
				int num = 100;
				int num2 = random.Next(3, 8);
				VirtualCamera virtualCamera = new VirtualCamera(player);
				SpriteEffects spriteEffects = (Main.WindForVisuals > 0f) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
				Vector2 value = default(Vector2);
				if (spriteEffects == SpriteEffects.FlipHorizontally)
				{
					value.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
				}
				else
				{
					value.X = virtualCamera.Position.X - (float)num;
				}
				value.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 3200f) + 3200f;
				float num3 = random.NextFloat() * 3f + 2f;
				int num4 = random.Next(30, 61) * 60;
				Vector2 value2 = new Vector2(random.NextFloat() * 0.1f + 0.1f, random.NextFloat() * 0.3f + 0.3f);
				Vector2 targetOffset = new Vector2(random.NextFloat() * 2f - 1f, random.NextFloat() * 2f - 1f) * 120f;
				for (int i = 0; i < num2; i++)
				{
					AmbientSky.GastropodGroupSkyEntity gastropodGroupSkyEntity = new AmbientSky.GastropodGroupSkyEntity(player, random);
					gastropodGroupSkyEntity.Depth = num3 + random.NextFloat() * 0.5f;
					gastropodGroupSkyEntity.Position = value + new Vector2(random.NextFloat() * 20f - 10f, random.NextFloat() * 3f) * 60f;
					gastropodGroupSkyEntity.Effects = spriteEffects;
					gastropodGroupSkyEntity.SetPositionInWorldBasedOnScreenSpace(gastropodGroupSkyEntity.Position);
					gastropodGroupSkyEntity.LifeTime = num4 + random.Next(301);
					gastropodGroupSkyEntity.SetMagnetization(value2 * (random.NextFloat() * 0.5f) * 0.05f, targetOffset);
					list.Add(gastropodGroupSkyEntity);
				}
				return list;
			}

			// Token: 0x04007547 RID: 30023
			private Vector2 _magnetAccelerations;

			// Token: 0x04007548 RID: 30024
			private Vector2 _magnetPointTarget;

			// Token: 0x04007549 RID: 30025
			private Vector2 _positionVsMagnet;

			// Token: 0x0400754A RID: 30026
			private Vector2 _velocityVsMagnet;
		}

		// Token: 0x02000952 RID: 2386
		private class SlimeBalloonGroupSkyEntity : AmbientSky.FadingSkyEntity
		{
			// Token: 0x0600488A RID: 18570 RVA: 0x006CD088 File Offset: 0x006CB288
			public SlimeBalloonGroupSkyEntity(Player player, FastRandom random)
			{
				VirtualCamera virtualCamera = new VirtualCamera(player);
				this.Effects = ((Main.WindForVisuals > 0f) ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
				int num = 100;
				if (this.Effects == SpriteEffects.FlipHorizontally)
				{
					this.Position.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
				}
				else
				{
					this.Position.X = virtualCamera.Position.X - (float)num;
				}
				this.Position.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 4000f) + 4000f;
				this.Depth = random.NextFloat() * 3f + 3f;
				base.SetPositionInWorldBasedOnScreenSpace(this.Position);
				this.Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/SlimeBalloons", 1);
				this.Frame = new SpriteFrame(1, 7);
				this.Frame.CurrentRow = (byte)random.Next(7);
				this.LifeTime = random.Next(60, 121) * 60;
				this.OpacityNormalizedTimeToFadeIn = 0.025f;
				this.OpacityNormalizedTimeToFadeOut = 0.975f;
				this.BrightnessLerper = 0.2f;
				this.FinalOpacityMultiplier = 1f;
				this.FramingSpeed = int.MaxValue;
			}

			// Token: 0x0600488B RID: 18571 RVA: 0x006CD1E0 File Offset: 0x006CB3E0
			public override void UpdateVelocity(int frameCount)
			{
				Vector2 value = this._magnetAccelerations * new Vector2((float)Math.Sign(this._magnetPointTarget.X - this._positionVsMagnet.X), (float)Math.Sign(this._magnetPointTarget.Y - this._positionVsMagnet.Y));
				this._velocityVsMagnet += value;
				this._positionVsMagnet += this._velocityVsMagnet;
				float x = (1f + Math.Abs(Main.WindForVisuals) * 1f) * (float)((this.Effects == SpriteEffects.FlipHorizontally) ? -1 : 1);
				this.Velocity = new Vector2(x, -0.01f) + this._velocityVsMagnet;
				this.Rotation = this.Velocity.X * 0.1f;
			}

			// Token: 0x0600488C RID: 18572 RVA: 0x006CD2BC File Offset: 0x006CB4BC
			public override void Update(int frameCount)
			{
				base.Update(frameCount);
				this.Effects = ((Main.WindForVisuals > 0f) ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
				if (!Main.IsItAHappyWindyDay || Main.IsItRaining || !Main.dayTime || Main.eclipse)
				{
					base.StartFadingOut(frameCount);
				}
			}

			// Token: 0x0600488D RID: 18573 RVA: 0x006CD309 File Offset: 0x006CB509
			public void SetMagnetization(Vector2 accelerations, Vector2 targetOffset)
			{
				this._magnetAccelerations = accelerations;
				this._magnetPointTarget = targetOffset;
			}

			// Token: 0x0600488E RID: 18574 RVA: 0x006CD31C File Offset: 0x006CB51C
			public static List<AmbientSky.SlimeBalloonGroupSkyEntity> CreateGroup(Player player, FastRandom random)
			{
				List<AmbientSky.SlimeBalloonGroupSkyEntity> list = new List<AmbientSky.SlimeBalloonGroupSkyEntity>();
				int num = 100;
				int num2 = random.Next(5, 10);
				VirtualCamera virtualCamera = new VirtualCamera(player);
				SpriteEffects spriteEffects = (Main.WindForVisuals > 0f) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
				Vector2 value = default(Vector2);
				if (spriteEffects == SpriteEffects.FlipHorizontally)
				{
					value.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
				}
				else
				{
					value.X = virtualCamera.Position.X - (float)num;
				}
				value.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 2400f) + 2400f;
				float num3 = random.NextFloat() * 3f + 3f;
				int num4 = random.Next(80, 121) * 60;
				Vector2 value2 = new Vector2(random.NextFloat() * 0.1f + 0.1f, random.NextFloat() * 0.1f + 0.1f);
				Vector2 targetOffset = new Vector2(random.NextFloat() * 2f - 1f, random.NextFloat() * 2f - 1f) * 150f;
				for (int i = 0; i < num2; i++)
				{
					AmbientSky.SlimeBalloonGroupSkyEntity slimeBalloonGroupSkyEntity = new AmbientSky.SlimeBalloonGroupSkyEntity(player, random);
					slimeBalloonGroupSkyEntity.Depth = num3 + random.NextFloat() * 0.5f;
					slimeBalloonGroupSkyEntity.Position = value + new Vector2(random.NextFloat() * 20f - 10f, random.NextFloat() * 3f) * 80f;
					slimeBalloonGroupSkyEntity.Effects = spriteEffects;
					slimeBalloonGroupSkyEntity.SetPositionInWorldBasedOnScreenSpace(slimeBalloonGroupSkyEntity.Position);
					slimeBalloonGroupSkyEntity.LifeTime = num4 + random.Next(301);
					slimeBalloonGroupSkyEntity.SetMagnetization(value2 * (random.NextFloat() * 0.2f) * 0.05f, targetOffset);
					list.Add(slimeBalloonGroupSkyEntity);
				}
				return list;
			}

			// Token: 0x0400754B RID: 30027
			private Vector2 _magnetAccelerations;

			// Token: 0x0400754C RID: 30028
			private Vector2 _magnetPointTarget;

			// Token: 0x0400754D RID: 30029
			private Vector2 _positionVsMagnet;

			// Token: 0x0400754E RID: 30030
			private Vector2 _velocityVsMagnet;
		}

		// Token: 0x02000953 RID: 2387
		private class HellBatsGoupSkyEntity : AmbientSky.FadingSkyEntity
		{
			// Token: 0x0600488F RID: 18575 RVA: 0x006CD528 File Offset: 0x006CB728
			public HellBatsGoupSkyEntity(Player player, FastRandom random)
			{
				VirtualCamera virtualCamera = new VirtualCamera(player);
				this.Effects = ((Main.WindForVisuals > 0f) ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
				int num = 100;
				if (this.Effects == SpriteEffects.FlipHorizontally)
				{
					this.Position.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
				}
				else
				{
					this.Position.X = virtualCamera.Position.X - (float)num;
				}
				this.Position.Y = random.NextFloat() * 400f + (float)(Main.UnderworldLayer * 16);
				this.Depth = random.NextFloat() * 5f + 3f;
				base.SetPositionInWorldBasedOnScreenSpace(this.Position);
				this.Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/HellBat" + random.Next(1, 3), 1);
				this.Frame = new SpriteFrame(1, 10);
				this.LifeTime = random.Next(60, 121) * 60;
				this.OpacityNormalizedTimeToFadeIn = 0.1f;
				this.OpacityNormalizedTimeToFadeOut = 0.9f;
				this.BrightnessLerper = 0.2f;
				this.FinalOpacityMultiplier = 1f;
				this.FramingSpeed = 4;
				this.FrameOffset = random.Next(0, (int)this.Frame.RowCount);
				int num2 = random.Next((int)this.Frame.RowCount);
				for (int i = 0; i < num2; i++)
				{
					base.NextFrame();
				}
			}

			// Token: 0x06004890 RID: 18576 RVA: 0x006CD6AC File Offset: 0x006CB8AC
			public override void UpdateVelocity(int frameCount)
			{
				Vector2 value = this._magnetAccelerations * new Vector2((float)Math.Sign(this._magnetPointTarget.X - this._positionVsMagnet.X), (float)Math.Sign(this._magnetPointTarget.Y - this._positionVsMagnet.Y));
				this._velocityVsMagnet += value;
				this._positionVsMagnet += this._velocityVsMagnet;
				float x = (3f + Math.Abs(Main.WindForVisuals) * 0.8f) * (float)((this.Effects == SpriteEffects.FlipHorizontally) ? -1 : 1);
				this.Velocity = new Vector2(x, 0f) + this._velocityVsMagnet;
			}

			// Token: 0x06004891 RID: 18577 RVA: 0x006CD76F File Offset: 0x006CB96F
			public override void Update(int frameCount)
			{
				base.Update(frameCount);
			}

			// Token: 0x06004892 RID: 18578 RVA: 0x006CD778 File Offset: 0x006CB978
			public void SetMagnetization(Vector2 accelerations, Vector2 targetOffset)
			{
				this._magnetAccelerations = accelerations;
				this._magnetPointTarget = targetOffset;
			}

			// Token: 0x06004893 RID: 18579 RVA: 0x006CD788 File Offset: 0x006CB988
			public override Color GetColor(Color backgroundColor)
			{
				return Color.Lerp(Color.White, Color.Gray, this.Depth / 15f) * this.Opacity * this.FinalOpacityMultiplier * this.Helper_GetOpacityWithAccountingForBackgroundsOff();
			}

			// Token: 0x06004894 RID: 18580 RVA: 0x006CD7C8 File Offset: 0x006CB9C8
			public static List<AmbientSky.HellBatsGoupSkyEntity> CreateGroup(Player player, FastRandom random)
			{
				List<AmbientSky.HellBatsGoupSkyEntity> list = new List<AmbientSky.HellBatsGoupSkyEntity>();
				int num = 100;
				int num2 = random.Next(20, 40);
				VirtualCamera virtualCamera = new VirtualCamera(player);
				SpriteEffects spriteEffects = (Main.WindForVisuals > 0f) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
				Vector2 value = default(Vector2);
				if (spriteEffects == SpriteEffects.FlipHorizontally)
				{
					value.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
				}
				else
				{
					value.X = virtualCamera.Position.X - (float)num;
				}
				value.Y = random.NextFloat() * 800f + (float)(Main.UnderworldLayer * 16);
				float num3 = random.NextFloat() * 5f + 3f;
				int num4 = random.Next(30, 61) * 60;
				Vector2 value2 = new Vector2(random.NextFloat() * 0.5f + 0.5f, random.NextFloat() * 0.5f + 0.5f);
				Vector2 targetOffset = new Vector2(random.NextFloat() * 2f - 1f, random.NextFloat() * 2f - 1f) * 100f;
				for (int i = 0; i < num2; i++)
				{
					AmbientSky.HellBatsGoupSkyEntity hellBatsGoupSkyEntity = new AmbientSky.HellBatsGoupSkyEntity(player, random);
					hellBatsGoupSkyEntity.Depth = num3 + random.NextFloat() * 0.5f;
					hellBatsGoupSkyEntity.Position = value + new Vector2(random.NextFloat() * 20f - 10f, random.NextFloat() * 3f) * 50f;
					hellBatsGoupSkyEntity.Effects = spriteEffects;
					hellBatsGoupSkyEntity.SetPositionInWorldBasedOnScreenSpace(hellBatsGoupSkyEntity.Position);
					hellBatsGoupSkyEntity.LifeTime = num4 + random.Next(301);
					hellBatsGoupSkyEntity.SetMagnetization(value2 * (random.NextFloat() * 0.3f + 0.85f) * 0.05f, targetOffset);
					list.Add(hellBatsGoupSkyEntity);
				}
				return list;
			}

			// Token: 0x06004895 RID: 18581 RVA: 0x006CD9C9 File Offset: 0x006CBBC9
			internal float Helper_GetOpacityWithAccountingForBackgroundsOff()
			{
				if (Main.netMode == 2 || Main.BackgroundEnabled)
				{
					return 1f;
				}
				return 0f;
			}

			// Token: 0x0400754F RID: 30031
			private Vector2 _magnetAccelerations;

			// Token: 0x04007550 RID: 30032
			private Vector2 _magnetPointTarget;

			// Token: 0x04007551 RID: 30033
			private Vector2 _positionVsMagnet;

			// Token: 0x04007552 RID: 30034
			private Vector2 _velocityVsMagnet;
		}

		// Token: 0x02000954 RID: 2388
		private class BatsGroupSkyEntity : AmbientSky.FadingSkyEntity
		{
			// Token: 0x06004896 RID: 18582 RVA: 0x006CD9E8 File Offset: 0x006CBBE8
			public BatsGroupSkyEntity(Player player, FastRandom random)
			{
				VirtualCamera virtualCamera = new VirtualCamera(player);
				this.Effects = ((Main.WindForVisuals > 0f) ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
				int num = 100;
				if (this.Effects == SpriteEffects.FlipHorizontally)
				{
					this.Position.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
				}
				else
				{
					this.Position.X = virtualCamera.Position.X - (float)num;
				}
				this.Position.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 2400f) + 2400f;
				this.Depth = random.NextFloat() * 3f + 3f;
				base.SetPositionInWorldBasedOnScreenSpace(this.Position);
				this.Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/Bat" + random.Next(1, 4), 1);
				this.Frame = new SpriteFrame(1, 10);
				this.LifeTime = random.Next(60, 121) * 60;
				this.OpacityNormalizedTimeToFadeIn = 0.1f;
				this.OpacityNormalizedTimeToFadeOut = 0.9f;
				this.BrightnessLerper = 0.2f;
				this.FinalOpacityMultiplier = 1f;
				this.FramingSpeed = 4;
				this.FrameOffset = random.Next(0, (int)this.Frame.RowCount);
				int num2 = random.Next((int)this.Frame.RowCount);
				for (int i = 0; i < num2; i++)
				{
					base.NextFrame();
				}
			}

			// Token: 0x06004897 RID: 18583 RVA: 0x006CDB7C File Offset: 0x006CBD7C
			public override void UpdateVelocity(int frameCount)
			{
				Vector2 value = this._magnetAccelerations * new Vector2((float)Math.Sign(this._magnetPointTarget.X - this._positionVsMagnet.X), (float)Math.Sign(this._magnetPointTarget.Y - this._positionVsMagnet.Y));
				this._velocityVsMagnet += value;
				this._positionVsMagnet += this._velocityVsMagnet;
				float x = (3f + Math.Abs(Main.WindForVisuals) * 0.8f) * (float)((this.Effects == SpriteEffects.FlipHorizontally) ? -1 : 1);
				this.Velocity = new Vector2(x, 0f) + this._velocityVsMagnet;
			}

			// Token: 0x06004898 RID: 18584 RVA: 0x006CBE30 File Offset: 0x006CA030
			public override void Update(int frameCount)
			{
				base.Update(frameCount);
				if (Main.IsItRaining || !Main.dayTime || Main.eclipse)
				{
					base.StartFadingOut(frameCount);
				}
			}

			// Token: 0x06004899 RID: 18585 RVA: 0x006CDC3F File Offset: 0x006CBE3F
			public void SetMagnetization(Vector2 accelerations, Vector2 targetOffset)
			{
				this._magnetAccelerations = accelerations;
				this._magnetPointTarget = targetOffset;
			}

			// Token: 0x0600489A RID: 18586 RVA: 0x006CDC50 File Offset: 0x006CBE50
			public override Color GetColor(Color backgroundColor)
			{
				return base.GetColor(backgroundColor) * Utils.Max<float>(new float[]
				{
					Main.bgAlphaFrontLayer[3],
					Main.bgAlphaFrontLayer[0],
					Main.bgAlphaFrontLayer[10],
					Main.bgAlphaFrontLayer[11],
					Main.bgAlphaFrontLayer[12]
				});
			}

			// Token: 0x0600489B RID: 18587 RVA: 0x006CDCAC File Offset: 0x006CBEAC
			public static List<AmbientSky.BatsGroupSkyEntity> CreateGroup(Player player, FastRandom random)
			{
				List<AmbientSky.BatsGroupSkyEntity> list = new List<AmbientSky.BatsGroupSkyEntity>();
				int num = 100;
				int num2 = random.Next(20, 40);
				VirtualCamera virtualCamera = new VirtualCamera(player);
				SpriteEffects spriteEffects = (Main.WindForVisuals > 0f) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
				Vector2 value = default(Vector2);
				if (spriteEffects == SpriteEffects.FlipHorizontally)
				{
					value.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
				}
				else
				{
					value.X = virtualCamera.Position.X - (float)num;
				}
				value.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 2400f) + 2400f;
				float num3 = random.NextFloat() * 3f + 3f;
				int num4 = random.Next(30, 61) * 60;
				Vector2 value2 = new Vector2(random.NextFloat() * 0.5f + 0.5f, random.NextFloat() * 0.5f + 0.5f);
				Vector2 targetOffset = new Vector2(random.NextFloat() * 2f - 1f, random.NextFloat() * 2f - 1f) * 100f;
				for (int i = 0; i < num2; i++)
				{
					AmbientSky.BatsGroupSkyEntity batsGroupSkyEntity = new AmbientSky.BatsGroupSkyEntity(player, random);
					batsGroupSkyEntity.Depth = num3 + random.NextFloat() * 0.5f;
					batsGroupSkyEntity.Position = value + new Vector2(random.NextFloat() * 20f - 10f, random.NextFloat() * 3f) * 50f;
					batsGroupSkyEntity.Effects = spriteEffects;
					batsGroupSkyEntity.SetPositionInWorldBasedOnScreenSpace(batsGroupSkyEntity.Position);
					batsGroupSkyEntity.LifeTime = num4 + random.Next(301);
					batsGroupSkyEntity.SetMagnetization(value2 * (random.NextFloat() * 0.3f + 0.85f) * 0.05f, targetOffset);
					list.Add(batsGroupSkyEntity);
				}
				return list;
			}

			// Token: 0x04007553 RID: 30035
			private Vector2 _magnetAccelerations;

			// Token: 0x04007554 RID: 30036
			private Vector2 _magnetPointTarget;

			// Token: 0x04007555 RID: 30037
			private Vector2 _positionVsMagnet;

			// Token: 0x04007556 RID: 30038
			private Vector2 _velocityVsMagnet;
		}

		// Token: 0x02000955 RID: 2389
		private class WyvernSkyEntity : AmbientSky.FadingSkyEntity
		{
			// Token: 0x0600489C RID: 18588 RVA: 0x006CDEBC File Offset: 0x006CC0BC
			public WyvernSkyEntity(Player player, FastRandom random)
			{
				VirtualCamera virtualCamera = new VirtualCamera(player);
				this.Effects = ((Main.WindForVisuals > 0f) ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
				int num = 100;
				if (this.Effects == SpriteEffects.FlipHorizontally)
				{
					this.Position.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
				}
				else
				{
					this.Position.X = virtualCamera.Position.X - (float)num;
				}
				this.Position.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 2400f) + 2400f;
				this.Depth = random.NextFloat() * 3f + 3f;
				base.SetPositionInWorldBasedOnScreenSpace(this.Position);
				this.Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/Wyvern", 1);
				this.Frame = new SpriteFrame(1, 5);
				this.LifeTime = random.Next(40, 71) * 60;
				this.OpacityNormalizedTimeToFadeIn = 0.15f;
				this.OpacityNormalizedTimeToFadeOut = 0.85f;
				this.BrightnessLerper = 0.2f;
				this.FinalOpacityMultiplier = 1f;
				this.FramingSpeed = 4;
			}

			// Token: 0x0600489D RID: 18589 RVA: 0x006CDFFC File Offset: 0x006CC1FC
			public override void UpdateVelocity(int frameCount)
			{
				float num = 3f + Math.Abs(Main.WindForVisuals) * 0.8f;
				this.Velocity = new Vector2(num * (float)((this.Effects == SpriteEffects.FlipHorizontally) ? -1 : 1), 0f);
			}
		}

		// Token: 0x02000956 RID: 2390
		private class NormalizedBackgroundLayerSpaceSkyEntity : AmbientSky.SkyEntity
		{
			// Token: 0x0600489E RID: 18590 RVA: 0x006CE040 File Offset: 0x006CC240
			public override Color GetColor(Color backgroundColor)
			{
				return Color.Lerp(backgroundColor, Color.White, 0.3f);
			}

			// Token: 0x0600489F RID: 18591 RVA: 0x006CBC85 File Offset: 0x006C9E85
			public override Vector2 GetDrawPosition()
			{
				return this.Position;
			}

			// Token: 0x060048A0 RID: 18592 RVA: 0x00009E06 File Offset: 0x00008006
			public override void Update(int frameCount)
			{
			}
		}

		// Token: 0x02000957 RID: 2391
		private class BoneSerpentSkyEntity : AmbientSky.NormalizedBackgroundLayerSpaceSkyEntity
		{
		}

		// Token: 0x02000958 RID: 2392
		private class AirshipSkyEntity : AmbientSky.FadingSkyEntity
		{
			// Token: 0x060048A3 RID: 18595 RVA: 0x006CE064 File Offset: 0x006CC264
			public AirshipSkyEntity(Player player, FastRandom random)
			{
				VirtualCamera virtualCamera = new VirtualCamera(player);
				this.Effects = ((random.Next(2) == 0) ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
				int num = 100;
				if (this.Effects == SpriteEffects.FlipHorizontally)
				{
					this.Position.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
				}
				else
				{
					this.Position.X = virtualCamera.Position.X - (float)num;
				}
				this.Position.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 2400f) + 2400f;
				this.Depth = random.NextFloat() * 3f + 3f;
				base.SetPositionInWorldBasedOnScreenSpace(this.Position);
				this.Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/FlyingShip", 1);
				this.Frame = new SpriteFrame(1, 4);
				this.LifeTime = random.Next(40, 71) * 60;
				this.OpacityNormalizedTimeToFadeIn = 0.05f;
				this.OpacityNormalizedTimeToFadeOut = 0.95f;
				this.BrightnessLerper = 0.2f;
				this.FinalOpacityMultiplier = 1f;
				this.FramingSpeed = 4;
			}

			// Token: 0x060048A4 RID: 18596 RVA: 0x006CE1A4 File Offset: 0x006CC3A4
			public override void UpdateVelocity(int frameCount)
			{
				float num = 6f + Math.Abs(Main.WindForVisuals) * 1.6f;
				this.Velocity = new Vector2(num * (float)((this.Effects == SpriteEffects.FlipHorizontally) ? -1 : 1), 0f);
			}

			// Token: 0x060048A5 RID: 18597 RVA: 0x006CBE30 File Offset: 0x006CA030
			public override void Update(int frameCount)
			{
				base.Update(frameCount);
				if (Main.IsItRaining || !Main.dayTime || Main.eclipse)
				{
					base.StartFadingOut(frameCount);
				}
			}
		}

		// Token: 0x02000959 RID: 2393
		private class AirBalloonSkyEntity : AmbientSky.FadingSkyEntity
		{
			// Token: 0x060048A6 RID: 18598 RVA: 0x006CE1E8 File Offset: 0x006CC3E8
			public AirBalloonSkyEntity(Player player, FastRandom random)
			{
				new VirtualCamera(player);
				int x = player.Center.ToTileCoordinates().X;
				this.Effects = ((random.Next(2) == 0) ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
				this.Position.X = ((float)x + 100f * (random.NextFloat() * 2f - 1f)) * 16f;
				this.Position.Y = (float)Main.worldSurface * 16f - (float)random.Next(50, 81) * 16f;
				this.Depth = random.NextFloat() * 3f + 3f;
				this.Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/AirBalloons_" + ((random.Next(2) == 0) ? "Large" : "Small"), 1);
				this.Frame = new SpriteFrame(1, 5);
				this.Frame.CurrentRow = (byte)random.Next(5);
				this.LifeTime = random.Next(20, 51) * 60;
				this.OpacityNormalizedTimeToFadeIn = 0.05f;
				this.OpacityNormalizedTimeToFadeOut = 0.95f;
				this.BrightnessLerper = 0.2f;
				this.FinalOpacityMultiplier = 1f;
				this.FramingSpeed = int.MaxValue;
			}

			// Token: 0x060048A7 RID: 18599 RVA: 0x006CE334 File Offset: 0x006CC534
			public override void UpdateVelocity(int frameCount)
			{
				float x = Main.WindForVisuals * 4f;
				float num = 3f + Math.Abs(Main.WindForVisuals) * 1f;
				if ((double)this.Position.Y < Main.worldSurface * 12.0)
				{
					num *= 0.5f;
				}
				if ((double)this.Position.Y < Main.worldSurface * 8.0)
				{
					num *= 0.5f;
				}
				if ((double)this.Position.Y < Main.worldSurface * 4.0)
				{
					num *= 0.5f;
				}
				this.Velocity = new Vector2(x, -num);
			}

			// Token: 0x060048A8 RID: 18600 RVA: 0x006CBE30 File Offset: 0x006CA030
			public override void Update(int frameCount)
			{
				base.Update(frameCount);
				if (Main.IsItRaining || !Main.dayTime || Main.eclipse)
				{
					base.StartFadingOut(frameCount);
				}
			}

			// Token: 0x04007557 RID: 30039
			private const int RANDOM_TILE_SPAWN_RANGE = 100;
		}

		// Token: 0x0200095A RID: 2394
		private class CrimeraSkyEntity : AmbientSky.EOCSkyEntity
		{
			// Token: 0x060048A9 RID: 18601 RVA: 0x006CE3E4 File Offset: 0x006CC5E4
			public CrimeraSkyEntity(Player player, FastRandom random) : base(player, random)
			{
				int num = 3;
				if (this.Depth <= 6f)
				{
					num = 2;
				}
				if (this.Depth <= 5f)
				{
					num = 1;
				}
				this.Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/Crimera" + num, 1);
				this.Frame = new SpriteFrame(1, 3);
			}

			// Token: 0x060048AA RID: 18602 RVA: 0x006CE447 File Offset: 0x006CC647
			public override Color GetColor(Color backgroundColor)
			{
				return base.GetColor(backgroundColor) * Main.bgAlphaFrontLayer[8];
			}
		}

		// Token: 0x0200095B RID: 2395
		private class EOSSkyEntity : AmbientSky.EOCSkyEntity
		{
			// Token: 0x060048AB RID: 18603 RVA: 0x006CE45C File Offset: 0x006CC65C
			public EOSSkyEntity(Player player, FastRandom random) : base(player, random)
			{
				int num = 3;
				if (this.Depth <= 6f)
				{
					num = 2;
				}
				if (this.Depth <= 5f)
				{
					num = 1;
				}
				this.Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/EOS" + num, 1);
				this.Frame = new SpriteFrame(1, 4);
			}

			// Token: 0x060048AC RID: 18604 RVA: 0x006CE4BF File Offset: 0x006CC6BF
			public override Color GetColor(Color backgroundColor)
			{
				return base.GetColor(backgroundColor) * Main.bgAlphaFrontLayer[1];
			}
		}

		// Token: 0x0200095C RID: 2396
		private class EOCSkyEntity : AmbientSky.FadingSkyEntity
		{
			// Token: 0x060048AD RID: 18605 RVA: 0x006CE4D4 File Offset: 0x006CC6D4
			public EOCSkyEntity(Player player, FastRandom random)
			{
				VirtualCamera camera = new VirtualCamera(player);
				this.Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/EOC", 1);
				this.Frame = new SpriteFrame(1, 3);
				this.Depth = random.NextFloat() * 3f + 4.5f;
				if (random.Next(4) != 0)
				{
					this.BeginZigZag(ref random, camera, (random.Next(2) == 1) ? 1 : -1);
				}
				else
				{
					this.BeginChasingPlayer(ref random, camera);
				}
				base.SetPositionInWorldBasedOnScreenSpace(this.Position);
				this.OpacityNormalizedTimeToFadeIn = 0.1f;
				this.OpacityNormalizedTimeToFadeOut = 0.9f;
				this.BrightnessLerper = 0.2f;
				this.FinalOpacityMultiplier = 1f;
				this.FramingSpeed = 5;
			}

			// Token: 0x060048AE RID: 18606 RVA: 0x006CE598 File Offset: 0x006CC798
			private void BeginZigZag(ref FastRandom random, VirtualCamera camera, int direction)
			{
				this._state = 1;
				this.LifeTime = random.Next(18, 31) * 60;
				this._direction = direction;
				this._waviness = random.NextFloat() * 1f + 1f;
				this.Position.Y = camera.Position.Y;
				int num = 100;
				if (this._direction == 1)
				{
					this.Position.X = camera.Position.X - (float)num;
					return;
				}
				this.Position.X = camera.Position.X + camera.Size.X + (float)num;
			}

			// Token: 0x060048AF RID: 18607 RVA: 0x006CE644 File Offset: 0x006CC844
			private void BeginChasingPlayer(ref FastRandom random, VirtualCamera camera)
			{
				this._state = 2;
				this.LifeTime = random.Next(18, 31) * 60;
				this.Position = camera.Position + camera.Size * new Vector2(random.NextFloat(), random.NextFloat());
			}

			// Token: 0x060048B0 RID: 18608 RVA: 0x006CE69C File Offset: 0x006CC89C
			public override void UpdateVelocity(int frameCount)
			{
				int state = this._state;
				if (state != 1)
				{
					if (state == 2)
					{
						this.ChasePlayerTop(frameCount);
					}
				}
				else
				{
					this.ZigzagMove(frameCount);
				}
				this.Rotation = this.Velocity.ToRotation();
			}

			// Token: 0x060048B1 RID: 18609 RVA: 0x006CE6DB File Offset: 0x006CC8DB
			private void ZigzagMove(int frameCount)
			{
				this.Velocity = new Vector2((float)(this._direction * 3), (float)Math.Cos((double)((float)frameCount / 1200f * 6.2831855f)) * this._waviness);
			}

			// Token: 0x060048B2 RID: 18610 RVA: 0x006CE710 File Offset: 0x006CC910
			private void ChasePlayerTop(int frameCount)
			{
				Vector2 vector = Main.LocalPlayer.Center + new Vector2(0f, -500f) - this.Position;
				if (vector.Length() >= 100f)
				{
					this.Velocity.X = this.Velocity.X + 0.1f * (float)Math.Sign(vector.X);
					this.Velocity.Y = this.Velocity.Y + 0.1f * (float)Math.Sign(vector.Y);
					this.Velocity = Vector2.Clamp(this.Velocity, new Vector2(-18f), new Vector2(18f));
				}
			}

			// Token: 0x04007558 RID: 30040
			private const int STATE_ZIGZAG = 1;

			// Token: 0x04007559 RID: 30041
			private const int STATE_GOOVERPLAYER = 2;

			// Token: 0x0400755A RID: 30042
			private int _state;

			// Token: 0x0400755B RID: 30043
			private int _direction;

			// Token: 0x0400755C RID: 30044
			private float _waviness;
		}

		// Token: 0x0200095D RID: 2397
		private class MeteorSkyEntity : AmbientSky.FadingSkyEntity
		{
			// Token: 0x060048B3 RID: 18611 RVA: 0x006CE7BC File Offset: 0x006CC9BC
			public MeteorSkyEntity(Player player, FastRandom random)
			{
				new VirtualCamera(player);
				this.Effects = ((random.Next(2) == 0) ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
				this.Depth = random.NextFloat() * 3f + 3f;
				this.Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/Meteor", 1);
				this.Frame = new SpriteFrame(1, 4);
				Vector2 vector = (0.7853982f + random.NextFloat() * 1.5707964f).ToRotationVector2();
				float num = (float)(Main.worldSurface * 16.0 - 0.0) / vector.Y;
				float num2 = 1200f;
				float scaleFactor = num / num2;
				Vector2 velocity = vector * scaleFactor;
				this.Velocity = velocity;
				int num3 = 100;
				Vector2 position = player.Center + new Vector2((float)random.Next(-num3, num3 + 1), (float)random.Next(-num3, num3 + 1)) - this.Velocity * num2 * 0.5f;
				this.Position = position;
				this.LifeTime = (int)num2;
				this.OpacityNormalizedTimeToFadeIn = 0.05f;
				this.OpacityNormalizedTimeToFadeOut = 0.95f;
				this.BrightnessLerper = 0.5f;
				this.FinalOpacityMultiplier = 1f;
				this.FramingSpeed = 5;
				this.Rotation = this.Velocity.ToRotation() + 1.5707964f;
			}
		}

		// Token: 0x0200095E RID: 2398
		// (Invoke) Token: 0x060048B5 RID: 18613
		private delegate AmbientSky.SkyEntity EntityFactoryMethod(Player player, int seed);
	}
}
