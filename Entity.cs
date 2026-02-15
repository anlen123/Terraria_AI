using System;
using Microsoft.Xna.Framework;

namespace Terraria
{
	// Token: 0x02000023 RID: 35
	public abstract class Entity : IEntitySourceTarget
	{
		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000179 RID: 377 RVA: 0x00011930 File Offset: 0x0000FB30
		public bool AnyWet
		{
			get
			{
				return this.wet || this.lavaWet || this.honeyWet || this.shimmerWet;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600017A RID: 378 RVA: 0x00011952 File Offset: 0x0000FB52
		public virtual Vector2 VisualPosition
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0001195A File Offset: 0x0000FB5A
		public float AngleTo(Vector2 Destination)
		{
			return (float)Math.Atan2((double)(Destination.Y - this.Center.Y), (double)(Destination.X - this.Center.X));
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00011988 File Offset: 0x0000FB88
		public float AngleFrom(Vector2 Source)
		{
			return (float)Math.Atan2((double)(this.Center.Y - Source.Y), (double)(this.Center.X - Source.X));
		}

		// Token: 0x0600017D RID: 381 RVA: 0x000119B6 File Offset: 0x0000FBB6
		public float Distance(Vector2 Other)
		{
			return Vector2.Distance(this.Center, Other);
		}

		// Token: 0x0600017E RID: 382 RVA: 0x000119C4 File Offset: 0x0000FBC4
		public float DistanceSQ(Vector2 Other)
		{
			return Vector2.DistanceSquared(this.Center, Other);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x000119D2 File Offset: 0x0000FBD2
		public Vector2 DirectionTo(Vector2 Destination)
		{
			return Vector2.Normalize(Destination - this.Center);
		}

		// Token: 0x06000180 RID: 384 RVA: 0x000119E5 File Offset: 0x0000FBE5
		public Vector2 DirectionFrom(Vector2 Source)
		{
			return Vector2.Normalize(this.Center - Source);
		}

		// Token: 0x06000181 RID: 385 RVA: 0x000119F8 File Offset: 0x0000FBF8
		public bool WithinRange(Vector2 Target, float MaxRange)
		{
			return Vector2.DistanceSquared(this.Center, Target) <= MaxRange * MaxRange;
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000182 RID: 386 RVA: 0x00011A0E File Offset: 0x0000FC0E
		// (set) Token: 0x06000183 RID: 387 RVA: 0x00011A47 File Offset: 0x0000FC47
		public Vector2 Center
		{
			get
			{
				return new Vector2(this.position.X + (float)this.width / 2f, this.position.Y + (float)this.height / 2f);
			}
			set
			{
				this.position = new Vector2(value.X - (float)this.width / 2f, value.Y - (float)this.height / 2f);
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000184 RID: 388 RVA: 0x00011A7C File Offset: 0x0000FC7C
		// (set) Token: 0x06000185 RID: 389 RVA: 0x00011AA7 File Offset: 0x0000FCA7
		public Vector2 Left
		{
			get
			{
				return new Vector2(this.position.X, this.position.Y + (float)this.height / 2f);
			}
			set
			{
				this.position = new Vector2(value.X, value.Y - (float)this.height / 2f);
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000186 RID: 390 RVA: 0x00011ACE File Offset: 0x0000FCCE
		// (set) Token: 0x06000187 RID: 391 RVA: 0x00011B01 File Offset: 0x0000FD01
		public Vector2 Right
		{
			get
			{
				return new Vector2(this.position.X + (float)this.width, this.position.Y + (float)this.height / 2f);
			}
			set
			{
				this.position = new Vector2(value.X - (float)this.width, value.Y - (float)this.height / 2f);
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000188 RID: 392 RVA: 0x00011B30 File Offset: 0x0000FD30
		// (set) Token: 0x06000189 RID: 393 RVA: 0x00011B5B File Offset: 0x0000FD5B
		public Vector2 Top
		{
			get
			{
				return new Vector2(this.position.X + (float)this.width / 2f, this.position.Y);
			}
			set
			{
				this.position = new Vector2(value.X - (float)this.width / 2f, value.Y);
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600018A RID: 394 RVA: 0x00011952 File Offset: 0x0000FB52
		// (set) Token: 0x0600018B RID: 395 RVA: 0x00011B82 File Offset: 0x0000FD82
		public Vector2 TopLeft
		{
			get
			{
				return this.position;
			}
			set
			{
				this.position = value;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600018C RID: 396 RVA: 0x00011B8B File Offset: 0x0000FD8B
		// (set) Token: 0x0600018D RID: 397 RVA: 0x00011BB0 File Offset: 0x0000FDB0
		public Vector2 TopRight
		{
			get
			{
				return new Vector2(this.position.X + (float)this.width, this.position.Y);
			}
			set
			{
				this.position = new Vector2(value.X - (float)this.width, value.Y);
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600018E RID: 398 RVA: 0x00011BD1 File Offset: 0x0000FDD1
		// (set) Token: 0x0600018F RID: 399 RVA: 0x00011C04 File Offset: 0x0000FE04
		public Vector2 Bottom
		{
			get
			{
				return new Vector2(this.position.X + (float)this.width / 2f, this.position.Y + (float)this.height);
			}
			set
			{
				this.position = new Vector2(value.X - (float)this.width / 2f, value.Y - (float)this.height);
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000190 RID: 400 RVA: 0x00011C33 File Offset: 0x0000FE33
		// (set) Token: 0x06000191 RID: 401 RVA: 0x00011C58 File Offset: 0x0000FE58
		public Vector2 BottomLeft
		{
			get
			{
				return new Vector2(this.position.X, this.position.Y + (float)this.height);
			}
			set
			{
				this.position = new Vector2(value.X, value.Y - (float)this.height);
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000192 RID: 402 RVA: 0x00011C79 File Offset: 0x0000FE79
		// (set) Token: 0x06000193 RID: 403 RVA: 0x00011CA6 File Offset: 0x0000FEA6
		public Vector2 BottomRight
		{
			get
			{
				return new Vector2(this.position.X + (float)this.width, this.position.Y + (float)this.height);
			}
			set
			{
				this.position = new Vector2(value.X - (float)this.width, value.Y - (float)this.height);
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000194 RID: 404 RVA: 0x00011CCF File Offset: 0x0000FECF
		// (set) Token: 0x06000195 RID: 405 RVA: 0x00011CE4 File Offset: 0x0000FEE4
		public Vector2 Size
		{
			get
			{
				return new Vector2((float)this.width, (float)this.height);
			}
			set
			{
				this.width = (int)value.X;
				this.height = (int)value.Y;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000196 RID: 406 RVA: 0x00011D00 File Offset: 0x0000FF00
		// (set) Token: 0x06000197 RID: 407 RVA: 0x00011D2B File Offset: 0x0000FF2B
		public Rectangle Hitbox
		{
			get
			{
				return new Rectangle((int)this.position.X, (int)this.position.Y, this.width, this.height);
			}
			set
			{
				this.position = new Vector2((float)value.X, (float)value.Y);
				this.width = value.Width;
				this.height = value.Height;
			}
		}

		// Token: 0x0400011F RID: 287
		public int whoAmI;

		// Token: 0x04000120 RID: 288
		public Vector2 position;

		// Token: 0x04000121 RID: 289
		public Vector2 velocity;

		// Token: 0x04000122 RID: 290
		public Vector2 oldPosition;

		// Token: 0x04000123 RID: 291
		public Vector2 oldVelocity;

		// Token: 0x04000124 RID: 292
		public int oldDirection;

		// Token: 0x04000125 RID: 293
		public int direction = 1;

		// Token: 0x04000126 RID: 294
		public int width;

		// Token: 0x04000127 RID: 295
		public int height;

		// Token: 0x04000128 RID: 296
		public bool wet;

		// Token: 0x04000129 RID: 297
		public bool shimmerWet;

		// Token: 0x0400012A RID: 298
		public bool honeyWet;

		// Token: 0x0400012B RID: 299
		public byte wetCount;

		// Token: 0x0400012C RID: 300
		public bool lavaWet;
	}
}
