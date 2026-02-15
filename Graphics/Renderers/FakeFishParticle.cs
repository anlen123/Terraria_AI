using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Shaders;
using Terraria.Graphics.Effects;
using Terraria.ID;

namespace Terraria.Graphics.Renderers
{
	// Token: 0x02000208 RID: 520
	public class FakeFishParticle : IPooledParticle, IParticle, IParticleRepel
	{
		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06002138 RID: 8504 RVA: 0x0052CC32 File Offset: 0x0052AE32
		// (set) Token: 0x06002139 RID: 8505 RVA: 0x0052CC3A File Offset: 0x0052AE3A
		public bool ShouldBeRemovedFromRenderer { get; private set; }

		// Token: 0x0600213A RID: 8506 RVA: 0x0052CC43 File Offset: 0x0052AE43
		public FakeFishParticle()
		{
			this._itemInstance = new Item();
		}

		// Token: 0x0600213B RID: 8507 RVA: 0x0052CC58 File Offset: 0x0052AE58
		public bool TryToMagnetizeTo(Projectile projectile, int requiredItemType = -1)
		{
			if (!this.CanHit(projectile, requiredItemType, 80f))
			{
				return false;
			}
			this._latchedProjectile = projectile;
			this._latchedProjectileType = projectile.type;
			this._state = FakeFishParticle.State.Latched;
			Vector2 vector;
			Vector2 vector2;
			this.SetAndGetLatchDetails(out vector, out vector2);
			this.Rotate(true);
			return true;
		}

		// Token: 0x0600213C RID: 8508 RVA: 0x0052CCA4 File Offset: 0x0052AEA4
		public bool TryToPushAway(Projectile projectile, int requiredItemType = -1)
		{
			if (this._state == FakeFishParticle.State.Jumping)
			{
				return false;
			}
			if (requiredItemType == this._itemInstance.type)
			{
				return false;
			}
			if (!this.CanHit(projectile, -1, 160f))
			{
				return false;
			}
			this.Velocity += projectile.DirectionTo(this.Position).SafeNormalize(Vector2.UnitY) * (1f + 1f * Main.rand.NextFloat()) * 1f;
			return true;
		}

		// Token: 0x0600213D RID: 8509 RVA: 0x0052CD2C File Offset: 0x0052AF2C
		public bool TryToBePinged(Projectile projectile, int requiredItemType = -1)
		{
			if (!this.CanHit(projectile, -1, 1000f))
			{
				return false;
			}
			int num = (int)projectile.Center.Distance(this.Position) / 10;
			int num2 = 60;
			this._sonarStartTime = num2;
			this._sonarTimeleft = num2 + num;
			this._isSelectedSonarType = (requiredItemType == this._itemInstance.type);
			this._sonarColor = (this._isSelectedSonarType ? new Color(255, 220, 30, 0) : (new Color(30, 200, 255, 0) * 0.05f));
			return true;
		}

		// Token: 0x0600213E RID: 8510 RVA: 0x0052CDC8 File Offset: 0x0052AFC8
		public bool CanHit(Projectile projectile, int requiredItemType = -1, float allowedRange = 80f)
		{
			if (requiredItemType != -1 && this._itemInstance.type != requiredItemType)
			{
				return false;
			}
			Vector2 center = projectile.Center;
			int num = 80;
			return center.Distance(this.Position) <= (float)num && Collision.CanHitLine(center, 0, 0, this.Position, 0, 0);
		}

		// Token: 0x0600213F RID: 8511 RVA: 0x0052CE1C File Offset: 0x0052B01C
		private void CheckLatch()
		{
			if (this._state != FakeFishParticle.State.Latched)
			{
				return;
			}
			int num = 80;
			if (!this._latchedProjectile.active || this._latchedProjectile.type != this._latchedProjectileType || this.Position.Distance(this._latchedProjectile.Center) > (float)num)
			{
				this.RemoveLatch();
				return;
			}
			if (this._latchedProjectile.ai[0] == 0f && this._latchedProjectile.ai[1] >= 0f)
			{
				this.RemoveLatch();
				return;
			}
			if (this._latchedProjectile.ai[0] == 1f)
			{
				this.RemoveLatch();
				if (this._latchedProjectile.ai[1] == (float)this._itemInstance.type)
				{
					this.ShouldBeRemovedFromRenderer = true;
				}
			}
		}

		// Token: 0x06002140 RID: 8512 RVA: 0x0052CEE2 File Offset: 0x0052B0E2
		private void RemoveLatch()
		{
			this._state = FakeFishParticle.State.FreeRoaming;
			this.PickNewVelocity();
		}

		// Token: 0x06002141 RID: 8513 RVA: 0x0052CEF4 File Offset: 0x0052B0F4
		private void EmitWaterPulse()
		{
			if (Main.netMode == 2)
			{
				return;
			}
			Vector2 position = this.Position - new Vector2(4f, 4f);
			int width = 8;
			int height = 8;
			bool lavaWet = Collision.LavaCollision(position, width, height);
			Collision.WetCollision(position, width, height);
			this.DoStandardWaterSplash(this.Position, Collision.shimmer, Collision.honey, lavaWet);
			WaterShaderData waterShaderData = (WaterShaderData)Filters.Scene["WaterDistortion"].GetShader();
			float value = 1.4f;
			waterShaderData.QueueRipple(this.Position, new Color(0.5f, 0.1f * (float)Math.Sign(value) + 0.5f, 0f, 1f) * Math.Abs(value), new Vector2(4f, 4f), RippleShape.Circle, 0f);
		}

		// Token: 0x06002142 RID: 8514 RVA: 0x0052CFC4 File Offset: 0x0052B1C4
		private void DoStandardWaterSplash(Vector2 castPosition, bool shimmerWet, bool honeyWet, bool lavaWet)
		{
			Vector2 vector = castPosition - new Vector2(4f, 4f);
			int num = 8;
			int num2 = 8;
			if (shimmerWet)
			{
				for (int i = 0; i < 10; i++)
				{
					int num3 = Dust.NewDust(new Vector2(vector.X - 6f, vector.Y + (float)(num2 / 2) - 8f), num + 12, 24, 308, 0f, 0f, 0, default(Color), 1f);
					Dust dust = Main.dust[num3];
					dust.velocity.Y = dust.velocity.Y - 4f;
					Dust dust2 = Main.dust[num3];
					dust2.velocity.X = dust2.velocity.X * 2.5f;
					Main.dust[num3].scale = 1.3f;
					Main.dust[num3].noGravity = true;
					int num4 = Main.rand.Next(6);
					if (num4 == 0)
					{
						Main.dust[num3].color = new Color(255, 255, 210);
					}
					else if (num4 == 1)
					{
						Main.dust[num3].color = new Color(190, 245, 255);
					}
					else if (num4 == 2)
					{
						Main.dust[num3].color = new Color(255, 150, 255);
					}
					else
					{
						Main.dust[num3].color = new Color(190, 175, 255);
					}
					SoundEngine.PlaySound(SoundID.FishSplash, (int)vector.X, (int)vector.Y, 5f, 1f);
				}
				return;
			}
			if (honeyWet)
			{
				for (int j = 0; j < 10; j++)
				{
					int num5 = Dust.NewDust(new Vector2(vector.X - 6f, vector.Y + (float)(num2 / 2) - 8f), num + 12, 24, 152, 0f, 0f, 0, default(Color), 1f);
					Dust dust3 = Main.dust[num5];
					dust3.velocity.Y = dust3.velocity.Y - 1f;
					Dust dust4 = Main.dust[num5];
					dust4.velocity.X = dust4.velocity.X * 2.5f;
					Main.dust[num5].scale = 1.3f;
					Main.dust[num5].alpha = 100;
					Main.dust[num5].noGravity = true;
				}
				SoundEngine.PlaySound(SoundID.FishSplash, (int)vector.X, (int)vector.Y, 1f, 1f);
				return;
			}
			if (lavaWet)
			{
				for (int k = 0; k < 10; k++)
				{
					int num6 = Dust.NewDust(new Vector2(vector.X - 6f, vector.Y + (float)(num2 / 2) - 8f), num + 12, 24, 35, 0f, 0f, 0, default(Color), 1f);
					Dust dust5 = Main.dust[num6];
					dust5.velocity.Y = dust5.velocity.Y - 1.5f;
					Dust dust6 = Main.dust[num6];
					dust6.velocity.X = dust6.velocity.X * 2.5f;
					Main.dust[num6].scale = 1.3f;
					Main.dust[num6].alpha = 100;
					Main.dust[num6].noGravity = true;
				}
				SoundEngine.PlaySound(SoundID.FishSplash, (int)vector.X, (int)vector.Y, 1f, 1f);
				return;
			}
			for (int l = 0; l < 10; l++)
			{
				int num7 = Dust.NewDust(new Vector2(vector.X - 6f, vector.Y + (float)(num2 / 2)), num + 12, 24, Dust.dustWater(), 0f, 0f, 0, default(Color), 1f);
				Dust dust7 = Main.dust[num7];
				dust7.velocity.Y = dust7.velocity.Y - 4f;
				Dust dust8 = Main.dust[num7];
				dust8.velocity.X = dust8.velocity.X * 2.5f;
				Main.dust[num7].scale = 1.3f;
				Main.dust[num7].alpha = 100;
				Main.dust[num7].noGravity = true;
			}
			SoundEngine.PlaySound(SoundID.FishSplash, (int)vector.X, (int)vector.Y, 1f, 1f);
		}

		// Token: 0x06002143 RID: 8515 RVA: 0x0052D440 File Offset: 0x0052B640
		public void Update(ref ParticleRendererSettings settings)
		{
			int num = this._delayTime;
			this._delayTime = num - 1;
			if (num >= 0)
			{
				return;
			}
			num = this._lifeTimeCounted + 1;
			this._lifeTimeCounted = num;
			if (num >= this._lifeTimeTotal)
			{
				this.ShouldBeRemovedFromRenderer = true;
			}
			num = this._sonarTimeleft - 1;
			this._sonarTimeleft = num;
			if (num < 0)
			{
				this._sonarTimeleft = 0;
			}
			bool flag = Collision.WetCollision(this.Position, 2, 2);
			if (flag != this._wasWet)
			{
				this.EmitWaterPulse();
			}
			this._wasWet = flag;
			if (this._state == FakeFishParticle.State.Jumping)
			{
				this.Velocity.Y = this.Velocity.Y + 0.2f;
				this.Rotation = this.Velocity.ToRotation();
				this.Position += this.Velocity;
				num = this._jumpTimeLeft - 1;
				this._jumpTimeLeft = num;
				if (num > 0)
				{
					return;
				}
				this._state = FakeFishParticle.State.FreeRoaming;
				this.PickNewVelocity();
				this.Velocity *= 0.15f;
			}
			if ((float)this._lifeTimeCounted / (float)this._lifeTimeTotal >= 0.15f && this.Velocity.Length() < 0.1f)
			{
				num = this._waitTime - 1;
				this._waitTime = num;
				if (num <= 0)
				{
					this._waitTime = Main.rand.Next(30, 121);
					this.PickNewVelocity();
					this._steerTime = Main.rand.Next(30, 121);
				}
			}
			num = this._steerTime - 1;
			this._steerTime = num;
			if (num > 0)
			{
				this.Velocity = Vector2.Lerp(this.Velocity, this.Rotation.ToRotationVector2() * 1f, 0.033333335f);
				this.Velocity = Vector2.Lerp(this.Velocity, this._targetVelocity, 0.05f);
			}
			else if (this.Velocity.Length() > 0.3f)
			{
				this.Velocity *= 0.975f;
			}
			if (this._state == FakeFishParticle.State.InterestedInBobber && this.Position.Distance(this._bobberLocation) < 16f)
			{
				this.Velocity *= 0.9f;
				if (this.Velocity.Length() < 0.02f)
				{
					float num2 = 0.3f + 1.4f * Main.rand.NextFloat();
					this.Velocity = (this._bobberLocation - this.Position).SafeNormalize(Vector2.Zero) * -num2;
				}
			}
			this.CheckLatch();
			if (this._state == FakeFishParticle.State.Latched)
			{
				Vector2 value;
				Vector2 value2;
				this.SetAndGetLatchDetails(out value, out value2);
				this.Position = Vector2.Lerp(this.Position, value - value2, 0.05f);
			}
			this.Position += this.Velocity;
			this.Rotate(false);
			int num3 = 20;
			if (this._lifeTimeTotal - this._lifeTimeCounted > num3 && !flag)
			{
				this._lifeTimeCounted = this._lifeTimeTotal - num3;
			}
			if (this.Velocity.Y < 0f && !Collision.WetCollision(this.Position + new Vector2(0f, -20f), 0, 0))
			{
				this._steerTime = 0;
				this.Velocity.Y = this.Velocity.Y * 0.92f;
			}
			if (this.Velocity != Vector2.Zero && !Collision.WetCollision(this.Position + this.Velocity.SafeNormalize(Vector2.Zero) * 32f, 0, 0))
			{
				this._steerTime = 0;
				this.PickNewVelocity();
				this.Velocity *= 0.92f;
			}
			if (this.Velocity != Vector2.Zero)
			{
				for (float num4 = 0f; num4 < 1f; num4 += 0.25f)
				{
					Vector2 vector = (6.2831855f * num4).ToRotationVector2() * 16f;
					if (Vector2.Dot(vector, this.Velocity.SafeNormalize(Vector2.UnitY)) >= 0f && !Collision.WetCollision(this.Position + vector, 0, 0))
					{
						this._steerTime = 0;
						this.PickNewVelocity();
						this.Velocity *= 0.92f;
						return;
					}
				}
			}
		}

		// Token: 0x06002144 RID: 8516 RVA: 0x0052D888 File Offset: 0x0052BA88
		private void SetAndGetLatchDetails(out Vector2 idealBobberPosition, out Vector2 idealOffset)
		{
			idealBobberPosition = this._latchedProjectile.Center + new Vector2(0f, 8f) + new Vector2(8f, 5f);
			Vector2 vector = idealBobberPosition - this.Position;
			idealOffset = vector.SafeNormalize(-Vector2.UnitY) * 6f;
			if (idealOffset.Y > 0f)
			{
				idealOffset.Y *= -1f;
			}
			idealOffset.Y -= 3f;
			idealOffset += new Vector2((float)(Math.Sign(vector.X) * -4), 0f);
			this._targetVelocity = idealOffset;
			this._bobberLocation = idealBobberPosition;
			int num = (int)((float)this._lifeTimeTotal * 0.85f);
			if (this._lifeTimeCounted > num)
			{
				this._lifeTimeCounted = num;
			}
		}

		// Token: 0x06002145 RID: 8517 RVA: 0x0052D98C File Offset: 0x0052BB8C
		private void Rotate(bool instant = false)
		{
			float num = 0.1f;
			if (this._isInanimate)
			{
				num *= 0.05f;
			}
			if (instant)
			{
				num = 6.2831855f;
			}
			if (this._state == FakeFishParticle.State.InterestedInBobber || this._state == FakeFishParticle.State.Latched)
			{
				this.Rotation = Utils.rotateTowards(this.Position, this.Rotation.ToRotationVector2(), this._bobberLocation, num).ToRotation();
				return;
			}
			if (this.Velocity != Vector2.Zero)
			{
				this.Rotation = Utils.rotateTowards(Vector2.Zero, this.Rotation.ToRotationVector2(), this.Velocity, num).ToRotation();
			}
		}

		// Token: 0x06002146 RID: 8518 RVA: 0x0052DA2C File Offset: 0x0052BC2C
		private void PickNewVelocity()
		{
			this._targetVelocity = Main.rand.NextVector2Circular(1.5f, 0.6f);
			if (this._state == FakeFishParticle.State.InterestedInBobber)
			{
				this._targetVelocity = (this._bobberLocation - this.Position).SafeNormalize(-Vector2.UnitY) * (0.7f + 0.5f * Main.rand.NextFloat());
			}
			if (this._state == FakeFishParticle.State.Latched)
			{
				this._targetVelocity = (this._latchedProjectile.Center - this.Position).SafeNormalize(-Vector2.UnitY) * 0.7f;
			}
		}

		// Token: 0x06002147 RID: 8519 RVA: 0x0052DADC File Offset: 0x0052BCDC
		public void Prepare(int itemType, int lifeTimeTotal, Vector2 bobberLocation)
		{
			this._itemInstance.SetDefaults(itemType, null);
			this._totalScale = 0.6f + 0.15f * Main.rand.NextFloat();
			this._bobberLocation = bobberLocation + new Vector2(0f, 8f);
			this._isInanimate = false;
			this._lifeTimeTotal = lifeTimeTotal;
			this._state = ((Main.rand.Next(3) != 0) ? FakeFishParticle.State.InterestedInBobber : FakeFishParticle.State.FreeRoaming);
			this._itemLooksDiagonal = ItemID.Sets.ReceivesDiagonalCorrectionAsFakeFish[itemType];
			if (ItemID.Sets.IsFishingCrate[itemType] || ItemID.Sets.FakeFishInanimate[itemType])
			{
				this._state = FakeFishParticle.State.FreeRoaming;
				this._isInanimate = true;
				this._itemLooksDiagonal = false;
			}
			this.PickNewVelocity();
			this._steerTime = Main.rand.Next(30, 121);
			this._wasWet = true;
			this._delayTime = 0;
		}

		// Token: 0x06002148 RID: 8520 RVA: 0x0052DBB0 File Offset: 0x0052BDB0
		public void TryJumping()
		{
			if (this._isInanimate)
			{
				return;
			}
			int num = (int)(this.Velocity.Y / -0.2f);
			this._jumpTimeLeft = num * 2;
			this._state = FakeFishParticle.State.Jumping;
			this._delayTime = Main.rand.Next(0, 11);
		}

		// Token: 0x06002149 RID: 8521 RVA: 0x0052DBFC File Offset: 0x0052BDFC
		public void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
		{
			Main.instance.LoadItem(this._itemInstance.type);
			float num = (float)this._lifeTimeCounted / (float)this._lifeTimeTotal;
			float toMin = Utils.Remap(num, 0.1f, 0.5f, 0f, 0.85f, true);
			toMin = Utils.Remap(num, 0.5f, 0.9f, toMin, 1f, true);
			Vector2 vector = settings.AnchorPosition + this.Position;
			float scale = Utils.Remap(num, 0f, 0.25f, 0f, 1f, true) * Utils.Remap(num, 0.85f, 1f, 1f, 0f, true);
			Vector2 vector2 = new Vector2(this._itemInstance.scale * this._totalScale);
			Vector2 vector3 = (6.2831855f * num * 6f).ToRotationVector2();
			vector2.X += vector3.Y * 0.014f;
			vector2.Y += vector3.X * 0.012f;
			float num2 = this.Rotation;
			if (this._itemLooksDiagonal)
			{
				num2 += 0.7853982f;
			}
			SpriteEffects effects = SpriteEffects.None;
			Vector2 vector4 = this.Velocity;
			if (this._itemLooksDiagonal)
			{
				if (this._state == FakeFishParticle.State.InterestedInBobber)
				{
					vector4 = this._targetVelocity;
				}
				else if (this._state == FakeFishParticle.State.Latched)
				{
					vector4 = this._targetVelocity;
				}
			}
			if (vector4.X < 0f)
			{
				effects = SpriteEffects.FlipVertically;
				if (this._itemLooksDiagonal)
				{
					num2 -= 1.5707964f;
				}
			}
			Texture2D texture;
			Rectangle rectangle;
			Rectangle rectangle2;
			Main.instance.DrawItem_GetBasics(this._itemInstance, 0, out texture, out rectangle, out rectangle2);
			Color color = Color.Lerp(Color.White, Color.Black, 0.8f) * 0.7f;
			if (this._sonarStartTime > 0 && this._sonarTimeleft <= this._sonarStartTime && this._sonarTimeleft > 0)
			{
				float fromValue = 1f - (float)this._sonarTimeleft / (float)this._sonarStartTime;
				float num3 = Utils.Remap(fromValue, 0f, 0.2f, 0f, 1f, true) * Utils.Remap(fromValue, 0.2f, 1f, 1f, 0f, true);
				color = Color.Lerp(color, Color.White, this._isSelectedSonarType ? num3 : (num3 * 0.4f));
				Color color2 = this._sonarColor * num3 * scale;
				for (float num4 = 0f; num4 < 6.2831855f; num4 += 1.5707964f)
				{
					spritebatch.Draw(texture, vector + (num4 + num2).ToRotationVector2() * 2f * vector2, new Rectangle?(rectangle), color2, num2, rectangle.Size() / 2f, vector2, effects, 0f);
				}
			}
			spritebatch.Draw(texture, vector, new Rectangle?(rectangle), color * scale, num2, rectangle.Size() / 2f, vector2, effects, 0f);
			if (this._itemInstance.glowMask != -1)
			{
				spritebatch.Draw(TextureAssets.GlowMask[(int)this._itemInstance.glowMask].Value, vector, new Rectangle?(rectangle2), Color.Lerp(Color.White, Color.Black, 0.8f) * scale * 0.7f, num2, rectangle2.Size() / 2f, vector2, effects, 0f);
			}
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x0600214A RID: 8522 RVA: 0x0052DF76 File Offset: 0x0052C176
		// (set) Token: 0x0600214B RID: 8523 RVA: 0x0052DF7E File Offset: 0x0052C17E
		public bool IsRestingInPool { get; private set; }

		// Token: 0x0600214C RID: 8524 RVA: 0x0052DF87 File Offset: 0x0052C187
		public void RestInPool()
		{
			this.IsRestingInPool = true;
		}

		// Token: 0x0600214D RID: 8525 RVA: 0x0052DF90 File Offset: 0x0052C190
		public virtual void FetchFromPool()
		{
			this._lifeTimeCounted = 0;
			this._lifeTimeTotal = 0;
			this.IsRestingInPool = false;
			this.ShouldBeRemovedFromRenderer = false;
			this.Position = (this.Velocity = Vector2.Zero);
			this.Rotation = 0f;
			this._gotRepelled = false;
		}

		// Token: 0x0600214E RID: 8526 RVA: 0x0052DFE0 File Offset: 0x0052C1E0
		public void BeRepelled(ref ParticleRepelDetails details)
		{
			if (this._isInanimate || this._gotRepelled)
			{
				return;
			}
			if (this._state == FakeFishParticle.State.Jumping || this._state == FakeFishParticle.State.Latched)
			{
				return;
			}
			float num = this.Position.Distance(details.SourcePosition) - details.Radius;
			if (num >= 100f)
			{
				return;
			}
			float toMin = 2f;
			if (!this._gotRepelled || Main.rand.Next(10) == 0)
			{
				float scaleFactor = Utils.Remap(num, 100f, 0f, toMin, 5f, true);
				this.Velocity = this.Position.DirectionFrom(details.SourcePosition).SafeNormalize(this.Velocity.ToRotation().ToRotationVector2()).RotatedByRandom(0.7853981852531433) * scaleFactor;
				this.Rotation = this.Velocity.ToRotation();
				this.Position -= this.Velocity;
				this._targetVelocity = this.Velocity;
				if (this.Velocity != Vector2.Zero && !Collision.WetCollision(this.Position + this.Velocity.SafeNormalize(Vector2.Zero) * 32f, 0, 0))
				{
					this._steerTime = 0;
					this.PickNewVelocity();
					this.Velocity = this._targetVelocity * 0.1f;
				}
			}
			this._delayTime = -1;
			this._steerTime = Main.rand.Next(30, 121);
			this._state = FakeFishParticle.State.FreeRoaming;
			this._gotRepelled = true;
			if ((float)this._lifeTimeCounted < (float)this._lifeTimeTotal * 0.7f)
			{
				float num2 = Utils.Remap((float)this._lifeTimeCounted / (float)this._lifeTimeTotal, 0f, 0.25f, 0f, 1f, true);
				float num3 = MathHelper.Lerp(1f, 0.85f, num2);
				if (num2 == 1f)
				{
					num3 = 0.7f;
				}
				this._lifeTimeCounted = (int)((float)this._lifeTimeTotal * num3);
			}
		}

		// Token: 0x04004B8B RID: 19339
		public Vector2 Position;

		// Token: 0x04004B8C RID: 19340
		public Vector2 Velocity;

		// Token: 0x04004B8D RID: 19341
		public float Rotation;

		// Token: 0x04004B8E RID: 19342
		private int _latchedProjectileType;

		// Token: 0x04004B8F RID: 19343
		private Projectile _latchedProjectile;

		// Token: 0x04004B90 RID: 19344
		private Item _itemInstance;

		// Token: 0x04004B91 RID: 19345
		private int _lifeTimeCounted;

		// Token: 0x04004B92 RID: 19346
		private int _lifeTimeTotal;

		// Token: 0x04004B93 RID: 19347
		private float _totalScale;

		// Token: 0x04004B94 RID: 19348
		private int _waitTime;

		// Token: 0x04004B95 RID: 19349
		private FakeFishParticle.State _state;

		// Token: 0x04004B96 RID: 19350
		private int _jumpTimeLeft;

		// Token: 0x04004B97 RID: 19351
		private bool _itemLooksDiagonal;

		// Token: 0x04004B98 RID: 19352
		private Vector2 _targetVelocity;

		// Token: 0x04004B99 RID: 19353
		private Vector2 _bobberLocation;

		// Token: 0x04004B9A RID: 19354
		private int _steerTime;

		// Token: 0x04004B9B RID: 19355
		private bool _isInanimate;

		// Token: 0x04004B9C RID: 19356
		private Color _sonarColor;

		// Token: 0x04004B9D RID: 19357
		private int _sonarTimeleft;

		// Token: 0x04004B9E RID: 19358
		private int _sonarStartTime;

		// Token: 0x04004B9F RID: 19359
		private bool _isSelectedSonarType;

		// Token: 0x04004BA0 RID: 19360
		private bool _wasWet;

		// Token: 0x04004BA1 RID: 19361
		private int _delayTime;

		// Token: 0x04004BA2 RID: 19362
		private bool _gotRepelled;

		// Token: 0x020007AB RID: 1963
		private enum State
		{
			// Token: 0x0400706F RID: 28783
			FreeRoaming,
			// Token: 0x04007070 RID: 28784
			InterestedInBobber,
			// Token: 0x04007071 RID: 28785
			Latched,
			// Token: 0x04007072 RID: 28786
			Jumping
		}
	}
}
