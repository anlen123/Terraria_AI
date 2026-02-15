using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.Events;
using Terraria.Graphics.Effects;
using Terraria.Utilities;

namespace Terraria.GameContent.Skies
{
	// Token: 0x0200044A RID: 1098
	public class LanternSky : CustomSky
	{
		// Token: 0x060031D6 RID: 12758 RVA: 0x005E39F1 File Offset: 0x005E1BF1
		public override void OnLoad()
		{
			this._texture = TextureAssets.Extra[134];
			this.GenerateLanterns(false);
		}

		// Token: 0x060031D7 RID: 12759 RVA: 0x005E3A0C File Offset: 0x005E1C0C
		private void GenerateLanterns(bool onlyMissing)
		{
			if (!onlyMissing)
			{
				this._lanterns = new LanternSky.Lantern[Main.maxTilesY / 4];
			}
			for (int i = 0; i < this._lanterns.Length; i++)
			{
				if (!onlyMissing || !this._lanterns[i].Active)
				{
					int num = (int)((double)Main.screenPosition.Y * 0.7 - (double)Main.screenHeight);
					int minValue = (int)((double)num - Main.worldSurface * 16.0);
					this._lanterns[i].Position = new Vector2((float)(this._random.Next(0, Main.maxTilesX) * 16), (float)this._random.Next(minValue, num));
					this.ResetLantern(i);
					this._lanterns[i].Active = true;
				}
			}
			this._lanternsDrawing = this._lanterns.Length;
		}

		// Token: 0x060031D8 RID: 12760 RVA: 0x005E3AF4 File Offset: 0x005E1CF4
		public void ResetLantern(int i)
		{
			this._lanterns[i].Depth = (1f - (float)i / (float)this._lanterns.Length) * 4.4f + 1.6f;
			this._lanterns[i].Speed = -1.5f - 2.5f * (float)this._random.NextDouble();
			this._lanterns[i].Texture = this._texture.Value;
			this._lanterns[i].Variant = this._random.Next(3);
			this._lanterns[i].TimeUntilFloat = (int)((float)(2000 + this._random.Next(1200)) * 2f);
			this._lanterns[i].TimeUntilFloatMax = this._lanterns[i].TimeUntilFloat;
		}

		// Token: 0x060031D9 RID: 12761 RVA: 0x005E3BE4 File Offset: 0x005E1DE4
		public override void Update(GameTime gameTime)
		{
			if (FocusHelper.PauseSkies)
			{
				return;
			}
			this._opacity = Utils.Clamp<float>(this._opacity + (float)LanternNight.LanternsUp.ToDirectionInt() * 0.01f, 0f, 1f);
			for (int i = 0; i < this._lanterns.Length; i++)
			{
				if (this._lanterns[i].Active)
				{
					float num = Main.windSpeedCurrent;
					if (num == 0f)
					{
						num = 0.1f;
					}
					float num2 = (float)Math.Sin((double)(this._lanterns[i].Position.X / 120f)) * 0.5f;
					LanternSky.Lantern[] lanterns = this._lanterns;
					int num3 = i;
					lanterns[num3].Position.Y = lanterns[num3].Position.Y + num2 * 0.5f;
					LanternSky.Lantern[] lanterns2 = this._lanterns;
					int num4 = i;
					lanterns2[num4].Position.Y = lanterns2[num4].Position.Y + this._lanterns[i].FloatAdjustedSpeed * 0.5f;
					LanternSky.Lantern[] lanterns3 = this._lanterns;
					int num5 = i;
					lanterns3[num5].Position.X = lanterns3[num5].Position.X + (0.1f + num) * (3f - this._lanterns[i].Speed) * 0.5f * ((float)i / (float)this._lanterns.Length + 1.5f) / 2.5f;
					this._lanterns[i].Rotation = num2 * (float)((num < 0f) ? -1 : 1) * 0.5f;
					this._lanterns[i].TimeUntilFloat = Math.Max(0, this._lanterns[i].TimeUntilFloat - 1);
					if (this._lanterns[i].Position.Y < 300f)
					{
						if (!this._leaving)
						{
							this.ResetLantern(i);
							this._lanterns[i].Position = new Vector2((float)(this._random.Next(0, Main.maxTilesX) * 16), (float)Main.worldSurface * 16f + 1600f);
						}
						else
						{
							this._lanterns[i].Active = false;
							this._lanternsDrawing--;
						}
					}
				}
			}
			this._active = true;
		}

		// Token: 0x060031DA RID: 12762 RVA: 0x005E3E20 File Offset: 0x005E2020
		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			if (Main.gameMenu && this._active)
			{
				this._active = false;
				this._leaving = false;
				for (int i = 0; i < this._lanterns.Length; i++)
				{
					this._lanterns[i].Active = false;
				}
			}
			if ((double)Main.screenPosition.Y > Main.worldSurface * 16.0 || Main.gameMenu)
			{
				return;
			}
			if (this._opacity <= 0f)
			{
				return;
			}
			int num = -1;
			int num2 = 0;
			for (int j = 0; j < this._lanterns.Length; j++)
			{
				float depth = this._lanterns[j].Depth;
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
				if (this._lanterns[k].Active)
				{
					Color opacity = new Color(250, 120, 60, 120);
					float num3 = 1f;
					if (this._lanterns[k].Depth > 5f)
					{
						num3 = 0.3f;
					}
					else if ((double)this._lanterns[k].Depth > 4.5)
					{
						num3 = 0.4f;
					}
					else if (this._lanterns[k].Depth > 4f)
					{
						num3 = 0.5f;
					}
					else if ((double)this._lanterns[k].Depth > 3.5)
					{
						num3 = 0.6f;
					}
					else if (this._lanterns[k].Depth > 3f)
					{
						num3 = 0.7f;
					}
					else if ((double)this._lanterns[k].Depth > 2.5)
					{
						num3 = 0.8f;
					}
					else if (this._lanterns[k].Depth > 2f)
					{
						num3 = 0.9f;
					}
					opacity = new Color((int)((float)opacity.R * num3), (int)((float)opacity.G * num3), (int)((float)opacity.B * num3), (int)((float)opacity.A * num3));
					Vector2 vector = new Vector2(1f / this._lanterns[k].Depth, 0.9f / this._lanterns[k].Depth);
					vector *= 1.2f;
					Vector2 vector2 = this._lanterns[k].Position;
					vector2 = (vector2 - value) * vector + value - Main.screenPosition;
					vector2.X = (vector2.X + 500f) % 4000f;
					if (vector2.X < 0f)
					{
						vector2.X += 4000f;
					}
					vector2.X -= 500f;
					if (rectangle.Contains((int)vector2.X, (int)vector2.Y))
					{
						this.DrawLantern(spriteBatch, this._lanterns[k], opacity, vector, vector2, num3);
					}
				}
			}
		}

		// Token: 0x060031DB RID: 12763 RVA: 0x005E41B0 File Offset: 0x005E23B0
		private void DrawLantern(SpriteBatch spriteBatch, LanternSky.Lantern lantern, Color opacity, Vector2 depthScale, Vector2 position, float alpha)
		{
			float y = (Main.GlobalTimeWrappedHourly % 6f / 6f * 6.2831855f).ToRotationVector2().Y;
			float scale = y * 0.2f + 0.8f;
			Color color = new Color(255, 255, 255, 0) * this._opacity * alpha * scale * 0.4f;
			for (float num = 0f; num < 1f; num += 0.33333334f)
			{
				Vector2 value = new Vector2(0f, 2f).RotatedBy((double)(6.2831855f * num + lantern.Rotation), default(Vector2)) * y;
				spriteBatch.Draw(lantern.Texture, position + value, new Rectangle?(lantern.GetSourceRectangle()), color, lantern.Rotation, lantern.GetSourceRectangle().Size() / 2f, depthScale.X * 2f, SpriteEffects.None, 0f);
			}
			spriteBatch.Draw(lantern.Texture, position, new Rectangle?(lantern.GetSourceRectangle()), opacity * this._opacity, lantern.Rotation, lantern.GetSourceRectangle().Size() / 2f, depthScale.X * 2f, SpriteEffects.None, 0f);
		}

		// Token: 0x060031DC RID: 12764 RVA: 0x005E4320 File Offset: 0x005E2520
		public override void Activate(Vector2 position, params object[] args)
		{
			if (this._active)
			{
				this._leaving = false;
				this.GenerateLanterns(true);
				return;
			}
			this.GenerateLanterns(false);
			this._active = true;
			this._leaving = false;
		}

		// Token: 0x060031DD RID: 12765 RVA: 0x005E434E File Offset: 0x005E254E
		public override void Deactivate(params object[] args)
		{
			this._leaving = true;
		}

		// Token: 0x060031DE RID: 12766 RVA: 0x005E4357 File Offset: 0x005E2557
		public override bool IsActive()
		{
			return this._active;
		}

		// Token: 0x060031DF RID: 12767 RVA: 0x005E435F File Offset: 0x005E255F
		public override void Reset()
		{
			this._active = false;
		}

		// Token: 0x040057A0 RID: 22432
		private bool _active;

		// Token: 0x040057A1 RID: 22433
		private bool _leaving;

		// Token: 0x040057A2 RID: 22434
		private float _opacity;

		// Token: 0x040057A3 RID: 22435
		private Asset<Texture2D> _texture;

		// Token: 0x040057A4 RID: 22436
		private LanternSky.Lantern[] _lanterns;

		// Token: 0x040057A5 RID: 22437
		private UnifiedRandom _random = new UnifiedRandom();

		// Token: 0x040057A6 RID: 22438
		private int _lanternsDrawing;

		// Token: 0x040057A7 RID: 22439
		private const float slowDown = 0.5f;

		// Token: 0x0200095F RID: 2399
		private struct Lantern
		{
			// Token: 0x17000582 RID: 1410
			// (get) Token: 0x060048B8 RID: 18616 RVA: 0x006CE921 File Offset: 0x006CCB21
			// (set) Token: 0x060048B9 RID: 18617 RVA: 0x006CE929 File Offset: 0x006CCB29
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
					this.FrameHeight = value.Height;
				}
			}

			// Token: 0x17000583 RID: 1411
			// (get) Token: 0x060048BA RID: 18618 RVA: 0x006CE94C File Offset: 0x006CCB4C
			public float FloatAdjustedSpeed
			{
				get
				{
					return this.Speed * ((float)this.TimeUntilFloat / (float)this.TimeUntilFloatMax);
				}
			}

			// Token: 0x060048BB RID: 18619 RVA: 0x006CE964 File Offset: 0x006CCB64
			public Rectangle GetSourceRectangle()
			{
				return new Rectangle(this.FrameWidth * this.Variant, 0, this.FrameWidth, this.FrameHeight);
			}

			// Token: 0x0400755D RID: 30045
			private const int MAX_FRAMES_X = 3;

			// Token: 0x0400755E RID: 30046
			public int Variant;

			// Token: 0x0400755F RID: 30047
			public int TimeUntilFloat;

			// Token: 0x04007560 RID: 30048
			public int TimeUntilFloatMax;

			// Token: 0x04007561 RID: 30049
			private Texture2D _texture;

			// Token: 0x04007562 RID: 30050
			public Vector2 Position;

			// Token: 0x04007563 RID: 30051
			public float Depth;

			// Token: 0x04007564 RID: 30052
			public float Rotation;

			// Token: 0x04007565 RID: 30053
			public int FrameHeight;

			// Token: 0x04007566 RID: 30054
			public int FrameWidth;

			// Token: 0x04007567 RID: 30055
			public float Speed;

			// Token: 0x04007568 RID: 30056
			public bool Active;
		}
	}
}
