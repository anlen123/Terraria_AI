using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Graphics.Effects;
using Terraria.Utilities;

namespace Terraria.GameContent.Skies
{
	// Token: 0x0200044C RID: 1100
	public class PartySky : CustomSky
	{
		// Token: 0x060031E9 RID: 12777 RVA: 0x005E44E4 File Offset: 0x005E26E4
		public override void OnLoad()
		{
			this._textures = new Asset<Texture2D>[3];
			for (int i = 0; i < this._textures.Length; i++)
			{
				this._textures[i] = TextureAssets.Extra[69 + i];
			}
			this.GenerateBalloons(false);
		}

		// Token: 0x060031EA RID: 12778 RVA: 0x005E452C File Offset: 0x005E272C
		private void GenerateBalloons(bool onlyMissing)
		{
			if (!onlyMissing)
			{
				this._balloons = new PartySky.Balloon[Main.maxTilesY / 4];
			}
			for (int i = 0; i < this._balloons.Length; i++)
			{
				if (!onlyMissing || !this._balloons[i].Active)
				{
					int num = (int)((double)Main.screenPosition.Y * 0.7 - (double)Main.screenHeight);
					int minValue = (int)((double)num - Main.worldSurface * 16.0);
					this._balloons[i].Position = new Vector2((float)(this._random.Next(0, Main.maxTilesX) * 16), (float)this._random.Next(minValue, num));
					this.ResetBalloon(i);
					this._balloons[i].Active = true;
				}
			}
			this._balloonsDrawing = this._balloons.Length;
		}

		// Token: 0x060031EB RID: 12779 RVA: 0x005E4614 File Offset: 0x005E2814
		public void ResetBalloon(int i)
		{
			this._balloons[i].Depth = (float)i / (float)this._balloons.Length * 1.75f + 1.6f;
			this._balloons[i].Speed = -1.5f - 2.5f * (float)this._random.NextDouble();
			this._balloons[i].Texture = this._textures[this._random.Next(2)].Value;
			this._balloons[i].Variant = this._random.Next(3);
			if (this._random.Next(30) == 0)
			{
				this._balloons[i].Texture = this._textures[2].Value;
			}
		}

		// Token: 0x060031EC RID: 12780 RVA: 0x005E46E8 File Offset: 0x005E28E8
		public override void Update(GameTime gameTime)
		{
			if (!PartySky.MultipleSkyWorkaroundFix && Main.dayRate == 0)
			{
				return;
			}
			PartySky.MultipleSkyWorkaroundFix = false;
			if (FocusHelper.PauseSkies)
			{
				return;
			}
			for (int i = 0; i < this._balloons.Length; i++)
			{
				if (this._balloons[i].Active)
				{
					PartySky.Balloon[] balloons = this._balloons;
					int num = i;
					int frame = balloons[num].Frame;
					balloons[num].Frame = frame + 1;
					PartySky.Balloon[] balloons2 = this._balloons;
					int num2 = i;
					balloons2[num2].Position.Y = balloons2[num2].Position.Y + this._balloons[i].Speed;
					PartySky.Balloon[] balloons3 = this._balloons;
					int num3 = i;
					balloons3[num3].Position.X = balloons3[num3].Position.X + Main.windSpeedCurrent * (3f - this._balloons[i].Speed);
					if (this._balloons[i].Position.Y < 300f)
					{
						if (!this._leaving)
						{
							this.ResetBalloon(i);
							this._balloons[i].Position = new Vector2((float)(this._random.Next(0, Main.maxTilesX) * 16), (float)Main.worldSurface * 16f + 1600f);
							if (this._random.Next(30) == 0)
							{
								this._balloons[i].Texture = this._textures[2].Value;
							}
						}
						else
						{
							this._balloons[i].Active = false;
							this._balloonsDrawing--;
						}
					}
				}
			}
			if (this._balloonsDrawing == 0)
			{
				this._active = false;
			}
			this._active = true;
		}

		// Token: 0x060031ED RID: 12781 RVA: 0x005E4890 File Offset: 0x005E2A90
		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			if (Main.gameMenu && this._active)
			{
				this._active = false;
				this._leaving = false;
				for (int i = 0; i < this._balloons.Length; i++)
				{
					this._balloons[i].Active = false;
				}
			}
			if ((double)Main.screenPosition.Y > Main.worldSurface * 16.0 || Main.gameMenu)
			{
				return;
			}
			if (this.Opacity <= 0f)
			{
				return;
			}
			int num = -1;
			int num2 = 0;
			for (int j = 0; j < this._balloons.Length; j++)
			{
				float depth = this._balloons[j].Depth;
				if (num == -1 && depth < maxDepth)
				{
					num = j;
				}
				if (depth <= minDepth)
				{
					break;
				}
				num2 = j;
			}
			if (num == -1)
			{
				return;
			}
			Vector2 value = Main.screenPosition + new Vector2((float)(Main.screenWidth >> 1), (float)(Main.screenHeight >> 1));
			Rectangle rectangle = new Rectangle(-1000, -1000, Main.screenWidth + 1000, Main.screenHeight + 1000);
			for (int k = num; k < num2; k++)
			{
				if (this._balloons[k].Active)
				{
					Color value2 = new Color(Main.ColorOfTheSkies.ToVector4() * 0.9f + new Vector4(0.1f)) * 0.8f;
					float num3 = 1f;
					if (this._balloons[k].Depth > 3f)
					{
						num3 = 0.6f;
					}
					else if ((double)this._balloons[k].Depth > 2.5)
					{
						num3 = 0.7f;
					}
					else if (this._balloons[k].Depth > 2f)
					{
						num3 = 0.8f;
					}
					else if ((double)this._balloons[k].Depth > 1.5)
					{
						num3 = 0.9f;
					}
					num3 *= 0.9f;
					value2 = new Color((int)((float)value2.R * num3), (int)((float)value2.G * num3), (int)((float)value2.B * num3), (int)((float)value2.A * num3));
					Vector2 vector = new Vector2(1f / this._balloons[k].Depth, 0.9f / this._balloons[k].Depth);
					Vector2 vector2 = this._balloons[k].Position;
					vector2 = (vector2 - value) * vector + value - Main.screenPosition;
					vector2.X = (vector2.X + 500f) % 4000f;
					if (vector2.X < 0f)
					{
						vector2.X += 4000f;
					}
					vector2.X -= 500f;
					if (rectangle.Contains((int)vector2.X, (int)vector2.Y))
					{
						spriteBatch.Draw(this._balloons[k].Texture, vector2, new Rectangle?(this._balloons[k].GetSourceRectangle()), value2 * this.Opacity, 0f, Vector2.Zero, vector.X * 2f, SpriteEffects.None, 0f);
					}
				}
			}
		}

		// Token: 0x060031EE RID: 12782 RVA: 0x005E4C06 File Offset: 0x005E2E06
		public override void Activate(Vector2 position, params object[] args)
		{
			if (this._active)
			{
				this._leaving = false;
				this.GenerateBalloons(true);
				return;
			}
			this.GenerateBalloons(false);
			this._active = true;
			this._leaving = false;
		}

		// Token: 0x060031EF RID: 12783 RVA: 0x005E4C34 File Offset: 0x005E2E34
		public override void Deactivate(params object[] args)
		{
			this._leaving = true;
		}

		// Token: 0x060031F0 RID: 12784 RVA: 0x005E4C3D File Offset: 0x005E2E3D
		public override bool IsActive()
		{
			return this._active;
		}

		// Token: 0x060031F1 RID: 12785 RVA: 0x005E4C45 File Offset: 0x005E2E45
		public override void Reset()
		{
			this._active = false;
		}

		// Token: 0x040057AC RID: 22444
		public static bool MultipleSkyWorkaroundFix;

		// Token: 0x040057AD RID: 22445
		private bool _active;

		// Token: 0x040057AE RID: 22446
		private bool _leaving;

		// Token: 0x040057AF RID: 22447
		private Asset<Texture2D>[] _textures;

		// Token: 0x040057B0 RID: 22448
		private PartySky.Balloon[] _balloons;

		// Token: 0x040057B1 RID: 22449
		private UnifiedRandom _random = new UnifiedRandom();

		// Token: 0x040057B2 RID: 22450
		private int _balloonsDrawing;

		// Token: 0x02000960 RID: 2400
		private struct Balloon
		{
			// Token: 0x17000584 RID: 1412
			// (get) Token: 0x060048BC RID: 18620 RVA: 0x006CE985 File Offset: 0x006CCB85
			// (set) Token: 0x060048BD RID: 18621 RVA: 0x006CE98D File Offset: 0x006CCB8D
			public Texture2D Texture
			{
				get
				{
					return this._texture;
				}
				set
				{
					this._texture = value;
					this.FrameWidth = value.Width / 3;
					this.FrameHeight = value.Height / 3;
				}
			}

			// Token: 0x17000585 RID: 1413
			// (get) Token: 0x060048BE RID: 18622 RVA: 0x006CE9B2 File Offset: 0x006CCBB2
			// (set) Token: 0x060048BF RID: 18623 RVA: 0x006CE9BA File Offset: 0x006CCBBA
			public int Frame
			{
				get
				{
					return this._frameCounter;
				}
				set
				{
					this._frameCounter = value % 42;
				}
			}

			// Token: 0x060048C0 RID: 18624 RVA: 0x006CE9C6 File Offset: 0x006CCBC6
			public Rectangle GetSourceRectangle()
			{
				return new Rectangle(this.FrameWidth * this.Variant, this._frameCounter / 14 * this.FrameHeight, this.FrameWidth, this.FrameHeight);
			}

			// Token: 0x04007569 RID: 30057
			private const int MAX_FRAMES_X = 3;

			// Token: 0x0400756A RID: 30058
			private const int MAX_FRAMES_Y = 3;

			// Token: 0x0400756B RID: 30059
			private const int FRAME_RATE = 14;

			// Token: 0x0400756C RID: 30060
			public int Variant;

			// Token: 0x0400756D RID: 30061
			private Texture2D _texture;

			// Token: 0x0400756E RID: 30062
			public Vector2 Position;

			// Token: 0x0400756F RID: 30063
			public float Depth;

			// Token: 0x04007570 RID: 30064
			public int FrameHeight;

			// Token: 0x04007571 RID: 30065
			public int FrameWidth;

			// Token: 0x04007572 RID: 30066
			public float Speed;

			// Token: 0x04007573 RID: 30067
			public bool Active;

			// Token: 0x04007574 RID: 30068
			private int _frameCounter;
		}
	}
}
