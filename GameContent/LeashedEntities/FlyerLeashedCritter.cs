using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Terraria.GameContent.LeashedEntities
{
	// Token: 0x02000456 RID: 1110
	public class FlyerLeashedCritter : LeashedCritter
	{
		// Token: 0x0600325A RID: 12890 RVA: 0x005EEDF4 File Offset: 0x005ECFF4
		public FlyerLeashedCritter()
		{
			this.anchorStyle = 4;
			this.strayingRangeInBlocks = 7;
			this.minWaitTime = 60;
			this.maxWaitTime = 300;
			this.maxFlySpeed = 1f;
			this.acceleration = 0.2f;
			this.brakeDuration = 10;
		}

		// Token: 0x0600325B RID: 12891 RVA: 0x005EEE46 File Offset: 0x005ED046
		public override void Spawn(bool newlyAdded)
		{
			base.Spawn(newlyAdded);
			if (!WorldGen.SolidTile2((int)base.AnchorPosition.X, (int)(base.AnchorPosition.Y + 1)))
			{
				this.velocity.Y = 0.0001f;
			}
			this.PickNewTarget();
		}

		// Token: 0x0600325C RID: 12892 RVA: 0x005EEE84 File Offset: 0x005ED084
		protected void PickNewTarget()
		{
			bool flag = this.hasGroundBias && base.AnchorPosition.Y == this.TargetPosition.Y && this.rand.Next(4) != 0;
			this.TargetPosition = new Point16((int)base.AnchorPosition.X + this.rand.Next(-this.strayingRangeInBlocks, this.strayingRangeInBlocks + 1), (int)base.AnchorPosition.Y + this.rand.Next(-this.strayingRangeInBlocks, 1));
			if (flag)
			{
				this.TargetPosition.Y = base.AnchorPosition.Y;
			}
		}

		// Token: 0x0600325D RID: 12893 RVA: 0x005EEF2B File Offset: 0x005ED12B
		protected override void CopyToDummy()
		{
			base.CopyToDummy();
			if (this.velocity.Y != 0f)
			{
				LeashedCritter._dummy.rotation = this.velocity.X * this.rotationScalar;
			}
		}

		// Token: 0x0600325E RID: 12894 RVA: 0x005EEF64 File Offset: 0x005ED164
		public override void Update()
		{
			base.Update();
			this.WaitTime -= 1;
			if (this.WaitTime <= 0)
			{
				this.WaitTime = (short)this.rand.Next(this.minWaitTime, this.maxWaitTime + 1);
				this.PickNewTarget();
			}
			Vector2 vector = this.TargetPosition.ToWorldCoordinates(8f, 8f);
			Vector2 value = vector - base.Center;
			float num = value.Length();
			Vector2 vector2 = value / num;
			if (vector2.HasNaNs())
			{
				vector2 = Vector2.Zero;
			}
			this.velocity += vector2 * this.acceleration;
			float num2 = this.velocity.Length();
			float val = Math.Min(1f, num / ((float)this.brakeDuration * this.maxFlySpeed));
			float num3 = this.maxFlySpeed * Math.Max(val, 0.25f);
			if (num2 > num3)
			{
				this.velocity *= num3 / num2;
			}
			bool flag = num < this.maxFlySpeed;
			bool flag2 = flag;
			if (!flag2)
			{
				flag2 = WorldGen.SolidTile2((base.Center + base.Size * 0.5f * vector2 + this.velocity).ToTileCoordinates());
			}
			if (flag2)
			{
				if (flag)
				{
					base.Center = vector;
				}
				Point point = base.Center.ToTileCoordinates();
				this.velocity.X = 0f;
				this.velocity.Y = (WorldGen.SolidTile2(point.X, point.Y + 1) ? 0f : 0.0001f);
			}
			else
			{
				base.Center += this.velocity;
				Point point2 = base.Center.ToTileCoordinates();
				if (this.velocity.Y == 0f && !WorldGen.SolidTile2(point2.X, point2.Y + 1))
				{
					this.velocity.Y = 0.0001f;
				}
			}
			int num4 = Math.Sign(this.velocity.X);
			if (num4 != 0 && num4 != this.direction)
			{
				this.direction = num4;
				this.spriteDirection = -this.direction;
			}
			if (Main.netMode != 2)
			{
				this.VisualEffects();
			}
			this.CopyToDummy();
			LeashedCritter._dummy.FindFrame();
			base.CopyFromDummy();
		}

		// Token: 0x0600325F RID: 12895 RVA: 0x005EF1D0 File Offset: 0x005ED3D0
		public override Vector2 GetDrawOffset()
		{
			if (this.velocity.Y == 0f)
			{
				Point16 point = base.Center.ToTileCoordinates16();
				if (Framing.GetTileSafely((int)point.X, (int)(point.Y + 1)).halfBrick())
				{
					return new Vector2(0f, 8f);
				}
				return Vector2.Zero;
			}
			else
			{
				if (this.hoverPeriod == 0f || this.hoverAmplitude == 0f)
				{
					return Vector2.Zero;
				}
				return this.GetBobbingOffset();
			}
		}

		// Token: 0x06003260 RID: 12896 RVA: 0x005EF254 File Offset: 0x005ED454
		protected Vector2 GetBobbingOffset()
		{
			double num = Main.timeForVisualEffects + (double)(this.whoAmI * this.npcType);
			num *= (double)(this.hoverPeriod * 6.2831855f);
			return new Vector2(0f, (float)Math.Sin(num) * this.hoverAmplitude);
		}

		// Token: 0x040057EC RID: 22508
		public static FlyerLeashedCritter Prototype = new FlyerLeashedCritter();

		// Token: 0x040057ED RID: 22509
		protected int minWaitTime;

		// Token: 0x040057EE RID: 22510
		protected int maxWaitTime;

		// Token: 0x040057EF RID: 22511
		protected float maxFlySpeed;

		// Token: 0x040057F0 RID: 22512
		protected float acceleration;

		// Token: 0x040057F1 RID: 22513
		protected int brakeDuration;

		// Token: 0x040057F2 RID: 22514
		protected float rotationScalar;

		// Token: 0x040057F3 RID: 22515
		protected float hoverAmplitude;

		// Token: 0x040057F4 RID: 22516
		protected float hoverPeriod;

		// Token: 0x040057F5 RID: 22517
		protected bool hasGroundBias;

		// Token: 0x040057F6 RID: 22518
		private const float HoverYVelocity = 0.0001f;
	}
}
