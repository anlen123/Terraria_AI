using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Graphics.Effects;
using Terraria.Utilities;

namespace Terraria.GameContent.Skies
{
	// Token: 0x02000454 RID: 1108
	public class NebulaSky : CustomSky
	{
		// Token: 0x06003237 RID: 12855 RVA: 0x005E7228 File Offset: 0x005E5428
		public override void OnLoad()
		{
			this._planetTexture = Main.Assets.Request<Texture2D>("Images/Misc/NebulaSky/Planet", 1);
			this._bgTexture = Main.Assets.Request<Texture2D>("Images/Misc/NebulaSky/Background", 1);
			this._beamTexture = Main.Assets.Request<Texture2D>("Images/Misc/NebulaSky/Beam", 1);
			this._rockTextures = new Asset<Texture2D>[3];
			for (int i = 0; i < this._rockTextures.Length; i++)
			{
				this._rockTextures[i] = Main.Assets.Request<Texture2D>("Images/Misc/NebulaSky/Rock_" + i, 1);
			}
		}

		// Token: 0x06003238 RID: 12856 RVA: 0x005E72BC File Offset: 0x005E54BC
		public override void Update(GameTime gameTime)
		{
			if (this._isActive)
			{
				this._fadeOpacity = Math.Min(1f, 0.01f + this._fadeOpacity);
				return;
			}
			this._fadeOpacity = Math.Max(0f, this._fadeOpacity - 0.01f);
		}

		// Token: 0x06003239 RID: 12857 RVA: 0x005E730A File Offset: 0x005E550A
		public override Color OnTileColor(Color inColor)
		{
			return new Color(Vector4.Lerp(inColor.ToVector4(), Vector4.One, this._fadeOpacity * 0.5f));
		}

		// Token: 0x0600323A RID: 12858 RVA: 0x005E7330 File Offset: 0x005E5530
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
			for (int i = 0; i < this._pillars.Length; i++)
			{
				float depth = this._pillars[i].Depth;
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
			Vector2 value3 = Main.screenPosition + new Vector2((float)(Main.screenWidth >> 1), (float)(Main.screenHeight >> 1));
			Rectangle rectangle = new Rectangle(-1000, -1000, Main.screenWidth + 1000, Main.screenHeight + 1000);
			float scale = Math.Min(1f, (Main.screenPosition.Y - 1000f) / 1000f);
			for (int j = num; j < num2; j++)
			{
				Vector2 vector = new Vector2(1f / this._pillars[j].Depth, 0.9f / this._pillars[j].Depth);
				Vector2 vector2 = this._pillars[j].Position;
				vector2 = (vector2 - value3) * vector + value3 - Main.screenPosition;
				if (rectangle.Contains((int)vector2.X, (int)vector2.Y))
				{
					float num3 = vector.X * 450f;
					spriteBatch.Draw(this._beamTexture.Value, vector2, null, Color.White * 0.2f * scale * this._fadeOpacity, 0f, Vector2.Zero, new Vector2(num3 / 70f, num3 / 45f), SpriteEffects.None, 0f);
					int num4 = 0;
					for (float num5 = 0f; num5 <= 1f; num5 += 0.03f)
					{
						float num6 = 1f - (num5 + Main.GlobalTimeWrappedHourly * 0.02f + (float)Math.Sin((double)j)) % 1f;
						spriteBatch.Draw(this._rockTextures[num4].Value, vector2 + new Vector2((float)Math.Sin((double)(num5 * 1582f)) * (num3 * 0.5f) + num3 * 0.5f, num6 * 2000f), null, Color.White * num6 * scale * this._fadeOpacity, num6 * 20f, new Vector2((float)(this._rockTextures[num4].Width() >> 1), (float)(this._rockTextures[num4].Height() >> 1)), 0.9f, SpriteEffects.None, 0f);
						num4 = (num4 + 1) % this._rockTextures.Length;
					}
				}
			}
		}

		// Token: 0x0600323B RID: 12859 RVA: 0x005E779F File Offset: 0x005E599F
		public override float GetCloudAlpha()
		{
			return (1f - this._fadeOpacity) * 0.3f + 0.7f;
		}

		// Token: 0x0600323C RID: 12860 RVA: 0x005E77BC File Offset: 0x005E59BC
		public override void Activate(Vector2 position, params object[] args)
		{
			this._fadeOpacity = 0.002f;
			this._isActive = true;
			this._pillars = new NebulaSky.LightPillar[40];
			for (int i = 0; i < this._pillars.Length; i++)
			{
				this._pillars[i].Position.X = (float)i / (float)this._pillars.Length * ((float)Main.maxTilesX * 16f + 20000f) + this._random.NextFloat() * 40f - 20f - 20000f;
				this._pillars[i].Position.Y = this._random.NextFloat() * 200f - 2000f;
				this._pillars[i].Depth = this._random.NextFloat() * 8f + 7f;
			}
			Array.Sort<NebulaSky.LightPillar>(this._pillars, new Comparison<NebulaSky.LightPillar>(this.SortMethod));
		}

		// Token: 0x0600323D RID: 12861 RVA: 0x005E78C0 File Offset: 0x005E5AC0
		private int SortMethod(NebulaSky.LightPillar pillar1, NebulaSky.LightPillar pillar2)
		{
			return pillar2.Depth.CompareTo(pillar1.Depth);
		}

		// Token: 0x0600323E RID: 12862 RVA: 0x005E78D4 File Offset: 0x005E5AD4
		public override void Deactivate(params object[] args)
		{
			this._isActive = false;
		}

		// Token: 0x0600323F RID: 12863 RVA: 0x005E78D4 File Offset: 0x005E5AD4
		public override void Reset()
		{
			this._isActive = false;
		}

		// Token: 0x06003240 RID: 12864 RVA: 0x005E78DD File Offset: 0x005E5ADD
		public override bool IsActive()
		{
			return this._isActive || this._fadeOpacity > 0.001f;
		}

		// Token: 0x040057DE RID: 22494
		private NebulaSky.LightPillar[] _pillars;

		// Token: 0x040057DF RID: 22495
		private UnifiedRandom _random = new UnifiedRandom();

		// Token: 0x040057E0 RID: 22496
		private Asset<Texture2D> _planetTexture;

		// Token: 0x040057E1 RID: 22497
		private Asset<Texture2D> _bgTexture;

		// Token: 0x040057E2 RID: 22498
		private Asset<Texture2D> _beamTexture;

		// Token: 0x040057E3 RID: 22499
		private Asset<Texture2D>[] _rockTextures;

		// Token: 0x040057E4 RID: 22500
		private bool _isActive;

		// Token: 0x040057E5 RID: 22501
		private float _fadeOpacity;

		// Token: 0x0200096A RID: 2410
		private struct LightPillar
		{
			// Token: 0x040075A4 RID: 30116
			public Vector2 Position;

			// Token: 0x040075A5 RID: 30117
			public float Depth;
		}
	}
}
