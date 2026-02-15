using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Graphics.Effects;
using Terraria.Utilities;

namespace Terraria.GameContent.Skies
{
	// Token: 0x02000451 RID: 1105
	public class SlimeSky : CustomSky
	{
		// Token: 0x0600321B RID: 12827 RVA: 0x005E5D98 File Offset: 0x005E3F98
		public override void OnLoad()
		{
			this._textures = new Asset<Texture2D>[4];
			for (int i = 0; i < 4; i++)
			{
				this._textures[i] = Main.Assets.Request<Texture2D>("Images/Misc/Sky_Slime_" + (i + 1), 1);
			}
			this.GenerateSlimes();
		}

		// Token: 0x0600321C RID: 12828 RVA: 0x005E5DE8 File Offset: 0x005E3FE8
		private void GenerateSlimes()
		{
			this._slimes = new SlimeSky.Slime[Main.maxTilesY / 6];
			for (int i = 0; i < this._slimes.Length; i++)
			{
				int num = (int)((double)Main.screenPosition.Y * 0.7 - (double)Main.screenHeight);
				int minValue = (int)((double)num - Main.worldSurface * 16.0);
				this._slimes[i].Position = new Vector2((float)(this._random.Next(0, Main.maxTilesX) * 16), (float)this._random.Next(minValue, num));
				this._slimes[i].Speed = 5f + 3f * (float)this._random.NextDouble();
				this._slimes[i].Depth = (float)i / (float)this._slimes.Length * 1.75f + 1.6f;
				this._slimes[i].Texture = this._textures[this._random.Next(2)].Value;
				if (this._random.Next(60) == 0)
				{
					this._slimes[i].Texture = this._textures[3].Value;
					this._slimes[i].Speed = 6f + 3f * (float)this._random.NextDouble();
					SlimeSky.Slime[] slimes = this._slimes;
					int num2 = i;
					slimes[num2].Depth = slimes[num2].Depth + 0.5f;
				}
				else if (this._random.Next(30) == 0)
				{
					this._slimes[i].Texture = this._textures[2].Value;
					this._slimes[i].Speed = 6f + 2f * (float)this._random.NextDouble();
				}
				this._slimes[i].Active = true;
			}
			this._slimesRemaining = this._slimes.Length;
		}

		// Token: 0x0600321D RID: 12829 RVA: 0x005E5FF0 File Offset: 0x005E41F0
		public override void Update(GameTime gameTime)
		{
			if (FocusHelper.PauseSkies)
			{
				return;
			}
			for (int i = 0; i < this._slimes.Length; i++)
			{
				if (this._slimes[i].Active)
				{
					SlimeSky.Slime[] slimes = this._slimes;
					int num = i;
					int frame = slimes[num].Frame;
					slimes[num].Frame = frame + 1;
					SlimeSky.Slime[] slimes2 = this._slimes;
					int num2 = i;
					slimes2[num2].Position.Y = slimes2[num2].Position.Y + this._slimes[i].Speed;
					if ((double)this._slimes[i].Position.Y > Main.worldSurface * 16.0)
					{
						if (!this._isLeaving)
						{
							this._slimes[i].Depth = (float)i / (float)this._slimes.Length * 1.75f + 1.6f;
							this._slimes[i].Position = new Vector2((float)(this._random.Next(0, Main.maxTilesX) * 16), -100f);
							this._slimes[i].Texture = this._textures[this._random.Next(2)].Value;
							this._slimes[i].Speed = 5f + 3f * (float)this._random.NextDouble();
							if (this._random.Next(60) == 0)
							{
								this._slimes[i].Texture = this._textures[3].Value;
								this._slimes[i].Speed = 6f + 3f * (float)this._random.NextDouble();
								SlimeSky.Slime[] slimes3 = this._slimes;
								int num3 = i;
								slimes3[num3].Depth = slimes3[num3].Depth + 0.5f;
							}
							else if (this._random.Next(30) == 0)
							{
								this._slimes[i].Texture = this._textures[2].Value;
								this._slimes[i].Speed = 6f + 2f * (float)this._random.NextDouble();
							}
						}
						else
						{
							this._slimes[i].Active = false;
							this._slimesRemaining--;
						}
					}
				}
			}
			if (this._slimesRemaining == 0)
			{
				this._isActive = false;
			}
		}

		// Token: 0x0600321E RID: 12830 RVA: 0x005E6254 File Offset: 0x005E4454
		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			if (Main.screenPosition.Y > 10000f || Main.gameMenu)
			{
				return;
			}
			int num = -1;
			int num2 = 0;
			for (int i = 0; i < this._slimes.Length; i++)
			{
				float depth = this._slimes[i].Depth;
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
			Vector2 value = Main.screenPosition + new Vector2((float)(Main.screenWidth >> 1), (float)(Main.screenHeight >> 1));
			Rectangle rectangle = new Rectangle(-1000, -1000, Main.screenWidth + 1000, Main.screenHeight + 1000);
			for (int j = num; j < num2; j++)
			{
				if (this._slimes[j].Active)
				{
					Color color = new Color(Main.ColorOfTheSkies.ToVector4() * 0.9f + new Vector4(0.1f)) * 0.8f;
					float num3 = 1f;
					if (this._slimes[j].Depth > 3f)
					{
						num3 = 0.6f;
					}
					else if ((double)this._slimes[j].Depth > 2.5)
					{
						num3 = 0.7f;
					}
					else if (this._slimes[j].Depth > 2f)
					{
						num3 = 0.8f;
					}
					else if ((double)this._slimes[j].Depth > 1.5)
					{
						num3 = 0.9f;
					}
					num3 *= 0.8f;
					color = new Color((int)((float)color.R * num3), (int)((float)color.G * num3), (int)((float)color.B * num3), (int)((float)color.A * num3));
					Vector2 vector = new Vector2(1f / this._slimes[j].Depth, 0.9f / this._slimes[j].Depth);
					Vector2 vector2 = this._slimes[j].Position;
					vector2 = (vector2 - value) * vector + value - Main.screenPosition;
					vector2.X = (vector2.X + 500f) % 4000f;
					if (vector2.X < 0f)
					{
						vector2.X += 4000f;
					}
					vector2.X -= 500f;
					if (rectangle.Contains((int)vector2.X, (int)vector2.Y))
					{
						spriteBatch.Draw(this._slimes[j].Texture, vector2, new Rectangle?(this._slimes[j].GetSourceRectangle()), color, 0f, Vector2.Zero, vector.X * 2f, SpriteEffects.None, 0f);
					}
				}
			}
		}

		// Token: 0x0600321F RID: 12831 RVA: 0x005E655F File Offset: 0x005E475F
		public override void Activate(Vector2 position, params object[] args)
		{
			this.GenerateSlimes();
			this._isActive = true;
			this._isLeaving = false;
		}

		// Token: 0x06003220 RID: 12832 RVA: 0x005E6575 File Offset: 0x005E4775
		public override void Deactivate(params object[] args)
		{
			this._isLeaving = true;
		}

		// Token: 0x06003221 RID: 12833 RVA: 0x005E657E File Offset: 0x005E477E
		public override void Reset()
		{
			this._isActive = false;
		}

		// Token: 0x06003222 RID: 12834 RVA: 0x005E6587 File Offset: 0x005E4787
		public override bool IsActive()
		{
			return this._isActive;
		}

		// Token: 0x040057CB RID: 22475
		private Asset<Texture2D>[] _textures;

		// Token: 0x040057CC RID: 22476
		private SlimeSky.Slime[] _slimes;

		// Token: 0x040057CD RID: 22477
		private UnifiedRandom _random = new UnifiedRandom();

		// Token: 0x040057CE RID: 22478
		private int _slimesRemaining;

		// Token: 0x040057CF RID: 22479
		private bool _isActive;

		// Token: 0x040057D0 RID: 22480
		private bool _isLeaving;

		// Token: 0x02000963 RID: 2403
		private struct Slime
		{
			// Token: 0x17000586 RID: 1414
			// (get) Token: 0x060048C1 RID: 18625 RVA: 0x006CE9F6 File Offset: 0x006CCBF6
			// (set) Token: 0x060048C2 RID: 18626 RVA: 0x006CE9FE File Offset: 0x006CCBFE
			public Texture2D Texture
			{
				get
				{
					return this._texture;
				}
				set
				{
					this._texture = value;
					this.FrameWidth = value.Width;
					this.FrameHeight = value.Height / 4;
				}
			}

			// Token: 0x17000587 RID: 1415
			// (get) Token: 0x060048C3 RID: 18627 RVA: 0x006CEA21 File Offset: 0x006CCC21
			// (set) Token: 0x060048C4 RID: 18628 RVA: 0x006CEA29 File Offset: 0x006CCC29
			public int Frame
			{
				get
				{
					return this._frame;
				}
				set
				{
					this._frame = value % 24;
				}
			}

			// Token: 0x060048C5 RID: 18629 RVA: 0x006CEA35 File Offset: 0x006CCC35
			public Rectangle GetSourceRectangle()
			{
				return new Rectangle(0, this._frame / 6 * this.FrameHeight, this.FrameWidth, this.FrameHeight);
			}

			// Token: 0x0400757E RID: 30078
			private const int MAX_FRAMES = 4;

			// Token: 0x0400757F RID: 30079
			private const int FRAME_RATE = 6;

			// Token: 0x04007580 RID: 30080
			private Texture2D _texture;

			// Token: 0x04007581 RID: 30081
			public Vector2 Position;

			// Token: 0x04007582 RID: 30082
			public float Depth;

			// Token: 0x04007583 RID: 30083
			public int FrameHeight;

			// Token: 0x04007584 RID: 30084
			public int FrameWidth;

			// Token: 0x04007585 RID: 30085
			public float Speed;

			// Token: 0x04007586 RID: 30086
			public bool Active;

			// Token: 0x04007587 RID: 30087
			private int _frame;
		}
	}
}
