using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Utilities;

namespace Terraria.GameContent.Skies
{
	// Token: 0x02000453 RID: 1107
	public class MartianSky : CustomSky
	{
		// Token: 0x0600322F RID: 12847 RVA: 0x005E6CE0 File Offset: 0x005E4EE0
		public override void Update(GameTime gameTime)
		{
			if (FocusHelper.PauseSkies)
			{
				return;
			}
			int num = this._activeUfos;
			for (int i = 0; i < this._ufos.Length; i++)
			{
				MartianSky.Ufo ufo = this._ufos[i];
				if (ufo.IsActive)
				{
					int frame = ufo.Frame;
					ufo.Frame = frame + 1;
					if (!ufo.Update())
					{
						if (!this._leaving)
						{
							ufo.AssignNewBehavior();
						}
						else
						{
							ufo.IsActive = false;
							num--;
						}
					}
				}
				this._ufos[i] = ufo;
			}
			if (!this._leaving && num != this._maxUfos)
			{
				this._ufos[num].IsActive = true;
				this._ufos[num++].AssignNewBehavior();
			}
			this._active = (!this._leaving || num != 0);
			this._activeUfos = num;
		}

		// Token: 0x06003230 RID: 12848 RVA: 0x005E6DBC File Offset: 0x005E4FBC
		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			if (Main.screenPosition.Y > 10000f)
			{
				return;
			}
			int num = -1;
			int num2 = 0;
			for (int i = 0; i < this._ufos.Length; i++)
			{
				float depth = this._ufos[i].Depth;
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
			Color value = new Color(Main.ColorOfTheSkies.ToVector4() * 0.9f + new Vector4(0.1f));
			Vector2 value2 = Main.screenPosition + new Vector2((float)(Main.screenWidth >> 1), (float)(Main.screenHeight >> 1));
			Rectangle rectangle = new Rectangle(-1000, -1000, Main.screenWidth + 1000, Main.screenHeight + 1000);
			for (int j = num; j < num2; j++)
			{
				Vector2 vector = new Vector2(1f / this._ufos[j].Depth, 0.9f / this._ufos[j].Depth);
				Vector2 vector2 = this._ufos[j].Position;
				vector2 = (vector2 - value2) * vector + value2 - Main.screenPosition;
				if (this._ufos[j].IsActive && rectangle.Contains((int)vector2.X, (int)vector2.Y))
				{
					spriteBatch.Draw(this._ufos[j].Texture, vector2, new Rectangle?(this._ufos[j].GetSourceRectangle()), value * this._ufos[j].Opacity, this._ufos[j].Rotation, Vector2.Zero, vector.X * 5f * this._ufos[j].Scale, SpriteEffects.None, 0f);
					if (this._ufos[j].GlowTexture != null)
					{
						spriteBatch.Draw(this._ufos[j].GlowTexture, vector2, new Rectangle?(this._ufos[j].GetSourceRectangle()), Color.White * this._ufos[j].Opacity, this._ufos[j].Rotation, Vector2.Zero, vector.X * 5f * this._ufos[j].Scale, SpriteEffects.None, 0f);
					}
				}
			}
		}

		// Token: 0x06003231 RID: 12849 RVA: 0x005E7070 File Offset: 0x005E5270
		private void GenerateUfos()
		{
			float num = (float)Main.maxTilesX / 4200f;
			this._maxUfos = (int)(256f * num);
			this._ufos = new MartianSky.Ufo[this._maxUfos];
			int num2 = this._maxUfos >> 4;
			for (int i = 0; i < num2; i++)
			{
				float num3 = (float)i / (float)num2;
				this._ufos[i] = new MartianSky.Ufo(TextureAssets.Extra[5].Value, (float)Main.rand.NextDouble() * 4f + 6.6f);
				this._ufos[i].GlowTexture = TextureAssets.GlowMask[90].Value;
			}
			for (int j = num2; j < this._ufos.Length; j++)
			{
				float num4 = (float)(j - num2) / (float)(this._ufos.Length - num2);
				this._ufos[j] = new MartianSky.Ufo(TextureAssets.Extra[6].Value, (float)Main.rand.NextDouble() * 5f + 1.6f);
				this._ufos[j].Scale = 0.5f;
				this._ufos[j].GlowTexture = TextureAssets.GlowMask[91].Value;
			}
		}

		// Token: 0x06003232 RID: 12850 RVA: 0x005E71A8 File Offset: 0x005E53A8
		public override void Activate(Vector2 position, params object[] args)
		{
			this._activeUfos = 0;
			this.GenerateUfos();
			Array.Sort<MartianSky.Ufo>(this._ufos, (MartianSky.Ufo ufo1, MartianSky.Ufo ufo2) => ufo2.Depth.CompareTo(ufo1.Depth));
			this._active = true;
			this._leaving = false;
		}

		// Token: 0x06003233 RID: 12851 RVA: 0x005E71FA File Offset: 0x005E53FA
		public override void Deactivate(params object[] args)
		{
			this._leaving = true;
		}

		// Token: 0x06003234 RID: 12852 RVA: 0x005E7203 File Offset: 0x005E5403
		public override bool IsActive()
		{
			return this._active;
		}

		// Token: 0x06003235 RID: 12853 RVA: 0x005E720B File Offset: 0x005E540B
		public override void Reset()
		{
			this._active = false;
		}

		// Token: 0x040057D8 RID: 22488
		private MartianSky.Ufo[] _ufos;

		// Token: 0x040057D9 RID: 22489
		private UnifiedRandom _random = new UnifiedRandom();

		// Token: 0x040057DA RID: 22490
		private int _maxUfos;

		// Token: 0x040057DB RID: 22491
		private bool _active;

		// Token: 0x040057DC RID: 22492
		private bool _leaving;

		// Token: 0x040057DD RID: 22493
		private int _activeUfos;

		// Token: 0x02000965 RID: 2405
		private abstract class IUfoController
		{
			// Token: 0x060048C6 RID: 18630
			public abstract void InitializeUfo(ref MartianSky.Ufo ufo);

			// Token: 0x060048C7 RID: 18631
			public abstract bool Update(ref MartianSky.Ufo ufo);
		}

		// Token: 0x02000966 RID: 2406
		private class ZipBehavior : MartianSky.IUfoController
		{
			// Token: 0x060048C9 RID: 18633 RVA: 0x006CEA58 File Offset: 0x006CCC58
			public override void InitializeUfo(ref MartianSky.Ufo ufo)
			{
				ufo.Position.X = (float)(MartianSky.Ufo.Random.NextDouble() * (double)(Main.maxTilesX << 4));
				ufo.Position.Y = (float)(MartianSky.Ufo.Random.NextDouble() * 5000.0);
				ufo.Opacity = 0f;
				float num = (float)MartianSky.Ufo.Random.NextDouble() * 5f + 10f;
				double num2 = MartianSky.Ufo.Random.NextDouble() * 0.6000000238418579 - 0.30000001192092896;
				ufo.Rotation = (float)num2;
				if (MartianSky.Ufo.Random.Next(2) == 0)
				{
					num2 += 3.1415927410125732;
				}
				this._speed = new Vector2((float)Math.Cos(num2) * num, (float)Math.Sin(num2) * num);
				this._ticks = 0;
				this._maxTicks = MartianSky.Ufo.Random.Next(400, 500);
			}

			// Token: 0x060048CA RID: 18634 RVA: 0x006CEB48 File Offset: 0x006CCD48
			public override bool Update(ref MartianSky.Ufo ufo)
			{
				if (this._ticks < 10)
				{
					ufo.Opacity += 0.1f;
				}
				else if (this._ticks > this._maxTicks - 10)
				{
					ufo.Opacity -= 0.1f;
				}
				ufo.Position += this._speed;
				if (this._ticks == this._maxTicks)
				{
					return false;
				}
				this._ticks++;
				return true;
			}

			// Token: 0x0400758E RID: 30094
			private Vector2 _speed;

			// Token: 0x0400758F RID: 30095
			private int _ticks;

			// Token: 0x04007590 RID: 30096
			private int _maxTicks;
		}

		// Token: 0x02000967 RID: 2407
		private class HoverBehavior : MartianSky.IUfoController
		{
			// Token: 0x060048CC RID: 18636 RVA: 0x006CEBD4 File Offset: 0x006CCDD4
			public override void InitializeUfo(ref MartianSky.Ufo ufo)
			{
				ufo.Position.X = (float)(MartianSky.Ufo.Random.NextDouble() * (double)(Main.maxTilesX << 4));
				ufo.Position.Y = (float)(MartianSky.Ufo.Random.NextDouble() * 5000.0);
				ufo.Opacity = 0f;
				ufo.Rotation = 0f;
				this._ticks = 0;
				this._maxTicks = MartianSky.Ufo.Random.Next(120, 240);
			}

			// Token: 0x060048CD RID: 18637 RVA: 0x006CEC54 File Offset: 0x006CCE54
			public override bool Update(ref MartianSky.Ufo ufo)
			{
				if (this._ticks < 10)
				{
					ufo.Opacity += 0.1f;
				}
				else if (this._ticks > this._maxTicks - 10)
				{
					ufo.Opacity -= 0.1f;
				}
				if (this._ticks == this._maxTicks)
				{
					return false;
				}
				this._ticks++;
				return true;
			}

			// Token: 0x04007591 RID: 30097
			private int _ticks;

			// Token: 0x04007592 RID: 30098
			private int _maxTicks;
		}

		// Token: 0x02000968 RID: 2408
		private struct Ufo
		{
			// Token: 0x17000588 RID: 1416
			// (get) Token: 0x060048CF RID: 18639 RVA: 0x006CECBB File Offset: 0x006CCEBB
			// (set) Token: 0x060048D0 RID: 18640 RVA: 0x006CECC3 File Offset: 0x006CCEC3
			public int Frame
			{
				get
				{
					return this._frame;
				}
				set
				{
					this._frame = value % 12;
				}
			}

			// Token: 0x17000589 RID: 1417
			// (get) Token: 0x060048D1 RID: 18641 RVA: 0x006CECCF File Offset: 0x006CCECF
			// (set) Token: 0x060048D2 RID: 18642 RVA: 0x006CECD7 File Offset: 0x006CCED7
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
					this.FrameHeight = value.Height / 3;
				}
			}

			// Token: 0x1700058A RID: 1418
			// (get) Token: 0x060048D3 RID: 18643 RVA: 0x006CECFA File Offset: 0x006CCEFA
			// (set) Token: 0x060048D4 RID: 18644 RVA: 0x006CED02 File Offset: 0x006CCF02
			public MartianSky.IUfoController Controller
			{
				get
				{
					return this._controller;
				}
				set
				{
					this._controller = value;
					value.InitializeUfo(ref this);
				}
			}

			// Token: 0x060048D5 RID: 18645 RVA: 0x006CED14 File Offset: 0x006CCF14
			public Ufo(Texture2D texture, float depth = 1f)
			{
				this._frame = 0;
				this.Position = Vector2.Zero;
				this._texture = texture;
				this.Depth = depth;
				this.Scale = 1f;
				this.FrameWidth = texture.Width;
				this.FrameHeight = texture.Height / 3;
				this.GlowTexture = null;
				this.Opacity = 0f;
				this.Rotation = 0f;
				this.IsActive = false;
				this._controller = null;
			}

			// Token: 0x060048D6 RID: 18646 RVA: 0x006CED91 File Offset: 0x006CCF91
			public Rectangle GetSourceRectangle()
			{
				return new Rectangle(0, this._frame / 4 * this.FrameHeight, this.FrameWidth, this.FrameHeight);
			}

			// Token: 0x060048D7 RID: 18647 RVA: 0x006CEDB4 File Offset: 0x006CCFB4
			public bool Update()
			{
				return this.Controller.Update(ref this);
			}

			// Token: 0x060048D8 RID: 18648 RVA: 0x006CEDC4 File Offset: 0x006CCFC4
			public void AssignNewBehavior()
			{
				int num = MartianSky.Ufo.Random.Next(2);
				if (num == 0)
				{
					this.Controller = new MartianSky.ZipBehavior();
					return;
				}
				if (num != 1)
				{
					return;
				}
				this.Controller = new MartianSky.HoverBehavior();
			}

			// Token: 0x04007593 RID: 30099
			private const int MAX_FRAMES = 3;

			// Token: 0x04007594 RID: 30100
			private const int FRAME_RATE = 4;

			// Token: 0x04007595 RID: 30101
			public static UnifiedRandom Random = new UnifiedRandom();

			// Token: 0x04007596 RID: 30102
			private int _frame;

			// Token: 0x04007597 RID: 30103
			private Texture2D _texture;

			// Token: 0x04007598 RID: 30104
			private MartianSky.IUfoController _controller;

			// Token: 0x04007599 RID: 30105
			public Texture2D GlowTexture;

			// Token: 0x0400759A RID: 30106
			public Vector2 Position;

			// Token: 0x0400759B RID: 30107
			public int FrameHeight;

			// Token: 0x0400759C RID: 30108
			public int FrameWidth;

			// Token: 0x0400759D RID: 30109
			public float Depth;

			// Token: 0x0400759E RID: 30110
			public float Scale;

			// Token: 0x0400759F RID: 30111
			public float Opacity;

			// Token: 0x040075A0 RID: 30112
			public bool IsActive;

			// Token: 0x040075A1 RID: 30113
			public float Rotation;
		}
	}
}
