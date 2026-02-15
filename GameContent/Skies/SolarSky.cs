using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Graphics.Effects;
using Terraria.Utilities;

namespace Terraria.GameContent.Skies
{
	// Token: 0x02000450 RID: 1104
	public class SolarSky : CustomSky
	{
		// Token: 0x06003210 RID: 12816 RVA: 0x005E5620 File Offset: 0x005E3820
		public override void OnLoad()
		{
			this._planetTexture = Main.Assets.Request<Texture2D>("Images/Misc/SolarSky/Planet", 1);
			this._bgTexture = Main.Assets.Request<Texture2D>("Images/Misc/SolarSky/Background", 1);
			this._meteorTexture = Main.Assets.Request<Texture2D>("Images/Misc/SolarSky/Meteor", 1);
		}

		// Token: 0x06003211 RID: 12817 RVA: 0x005E5670 File Offset: 0x005E3870
		public override void Update(GameTime gameTime)
		{
			if (this._isActive)
			{
				this._fadeOpacity = Math.Min(1f, 0.01f + this._fadeOpacity);
			}
			else
			{
				this._fadeOpacity = Math.Max(0f, this._fadeOpacity - 0.01f);
			}
			float num = 1200f;
			for (int i = 0; i < this._meteors.Length; i++)
			{
				SolarSky.Meteor[] meteors = this._meteors;
				int num2 = i;
				meteors[num2].Position.X = meteors[num2].Position.X - num * (float)gameTime.ElapsedGameTime.TotalSeconds;
				SolarSky.Meteor[] meteors2 = this._meteors;
				int num3 = i;
				meteors2[num3].Position.Y = meteors2[num3].Position.Y + num * (float)gameTime.ElapsedGameTime.TotalSeconds;
				if ((double)this._meteors[i].Position.Y > Main.worldSurface * 16.0)
				{
					this._meteors[i].Position.X = this._meteors[i].StartX;
					this._meteors[i].Position.Y = -10000f;
				}
			}
		}

		// Token: 0x06003212 RID: 12818 RVA: 0x005E579E File Offset: 0x005E399E
		public override Color OnTileColor(Color inColor)
		{
			return new Color(Vector4.Lerp(inColor.ToVector4(), Vector4.One, this._fadeOpacity * 0.5f));
		}

		// Token: 0x06003213 RID: 12819 RVA: 0x005E57C4 File Offset: 0x005E39C4
		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			if (maxDepth >= 3.4028235E+38f && minDepth < 3.4028235E+38f)
			{
				spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black * this._fadeOpacity);
				spriteBatch.Draw(this._bgTexture.Value, new Rectangle(0, Math.Max(0, (int)((Main.worldSurface * 16.0 - (double)Main.screenPosition.Y - 2400.0) * 0.10000000149011612)), Main.screenWidth, Main.screenHeight), Color.White * Math.Min(1f, (Main.screenPosition.Y - 800f) / 1000f * this._fadeOpacity));
				Vector2 value = new Vector2((float)(Main.screenWidth >> 1), (float)(Main.screenHeight >> 1));
				Vector2 value2 = 0.01f * (new Vector2((float)Main.maxTilesX * 8f, (float)Main.worldSurface / 2f) - Main.screenPosition);
				spriteBatch.Draw(this._planetTexture.Value, value + new Vector2(-200f, -200f) + value2, null, Color.White * 0.9f * this._fadeOpacity, 0f, new Vector2((float)(this._planetTexture.Width() >> 1), (float)(this._planetTexture.Height() >> 1)), 1f, SpriteEffects.None, 0f);
			}
			int num = -1;
			int num2 = 0;
			for (int i = 0; i < this._meteors.Length; i++)
			{
				float depth = this._meteors[i].Depth;
				if (num == -1 && depth < maxDepth)
				{
					num = i;
				}
				if (depth <= minDepth)
				{
					break;
				}
				num2 = i;
			}
			if (num == -1)
			{
				return;
			}
			float scale = Math.Min(1f, (Main.screenPosition.Y - 1000f) / 1000f);
			Vector2 value3 = Main.screenPosition + new Vector2((float)(Main.screenWidth >> 1), (float)(Main.screenHeight >> 1));
			Rectangle rectangle = new Rectangle(-1000, -1000, Main.screenWidth + 1000, Main.screenHeight + 1000);
			for (int j = num; j < num2; j++)
			{
				Vector2 vector = new Vector2(1f / this._meteors[j].Depth, 0.9f / this._meteors[j].Depth);
				Vector2 vector2 = (this._meteors[j].Position - value3) * vector + value3 - Main.screenPosition;
				int num3 = this._meteors[j].FrameCounter / 3;
				this._meteors[j].FrameCounter = (this._meteors[j].FrameCounter + 1) % 12;
				if (rectangle.Contains((int)vector2.X, (int)vector2.Y))
				{
					spriteBatch.Draw(this._meteorTexture.Value, vector2, new Rectangle?(new Rectangle(0, num3 * (this._meteorTexture.Height() / 4), this._meteorTexture.Width(), this._meteorTexture.Height() / 4)), Color.White * scale * this._fadeOpacity, 0f, Vector2.Zero, vector.X * 5f * this._meteors[j].Scale, SpriteEffects.None, 0f);
				}
			}
		}

		// Token: 0x06003214 RID: 12820 RVA: 0x005E5B82 File Offset: 0x005E3D82
		public override float GetCloudAlpha()
		{
			return (1f - this._fadeOpacity) * 0.3f + 0.7f;
		}

		// Token: 0x06003215 RID: 12821 RVA: 0x005E5B9C File Offset: 0x005E3D9C
		public override void Activate(Vector2 position, params object[] args)
		{
			this._fadeOpacity = 0.002f;
			this._isActive = true;
			this._meteors = new SolarSky.Meteor[150];
			for (int i = 0; i < this._meteors.Length; i++)
			{
				float num = (float)i / (float)this._meteors.Length;
				this._meteors[i].Position.X = num * ((float)Main.maxTilesX * 16f) + this._random.NextFloat() * 40f - 20f;
				this._meteors[i].Position.Y = this._random.NextFloat() * -((float)Main.worldSurface * 16f + 10000f) - 10000f;
				if (this._random.Next(3) != 0)
				{
					this._meteors[i].Depth = this._random.NextFloat() * 3f + 1.8f;
				}
				else
				{
					this._meteors[i].Depth = this._random.NextFloat() * 5f + 4.8f;
				}
				this._meteors[i].FrameCounter = this._random.Next(12);
				this._meteors[i].Scale = this._random.NextFloat() * 0.5f + 1f;
				this._meteors[i].StartX = this._meteors[i].Position.X;
			}
			Array.Sort<SolarSky.Meteor>(this._meteors, new Comparison<SolarSky.Meteor>(this.SortMethod));
		}

		// Token: 0x06003216 RID: 12822 RVA: 0x005E5D4C File Offset: 0x005E3F4C
		private int SortMethod(SolarSky.Meteor meteor1, SolarSky.Meteor meteor2)
		{
			return meteor2.Depth.CompareTo(meteor1.Depth);
		}

		// Token: 0x06003217 RID: 12823 RVA: 0x005E5D60 File Offset: 0x005E3F60
		public override void Deactivate(params object[] args)
		{
			this._isActive = false;
		}

		// Token: 0x06003218 RID: 12824 RVA: 0x005E5D60 File Offset: 0x005E3F60
		public override void Reset()
		{
			this._isActive = false;
		}

		// Token: 0x06003219 RID: 12825 RVA: 0x005E5D69 File Offset: 0x005E3F69
		public override bool IsActive()
		{
			return this._isActive || this._fadeOpacity > 0.001f;
		}

		// Token: 0x040057C4 RID: 22468
		private UnifiedRandom _random = new UnifiedRandom();

		// Token: 0x040057C5 RID: 22469
		private Asset<Texture2D> _planetTexture;

		// Token: 0x040057C6 RID: 22470
		private Asset<Texture2D> _bgTexture;

		// Token: 0x040057C7 RID: 22471
		private Asset<Texture2D> _meteorTexture;

		// Token: 0x040057C8 RID: 22472
		private bool _isActive;

		// Token: 0x040057C9 RID: 22473
		private SolarSky.Meteor[] _meteors;

		// Token: 0x040057CA RID: 22474
		private float _fadeOpacity;

		// Token: 0x02000962 RID: 2402
		private struct Meteor
		{
			// Token: 0x04007579 RID: 30073
			public Vector2 Position;

			// Token: 0x0400757A RID: 30074
			public float Depth;

			// Token: 0x0400757B RID: 30075
			public int FrameCounter;

			// Token: 0x0400757C RID: 30076
			public float Scale;

			// Token: 0x0400757D RID: 30077
			public float StartX;
		}
	}
}
