using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Events;

namespace Terraria.GameContent.UI
{
	// Token: 0x02000368 RID: 872
	public class UIDust
	{
		// Token: 0x0600290B RID: 10507 RVA: 0x00577080 File Offset: 0x00575280
		public void Clear()
		{
			for (int i = 0; i < 201; i++)
			{
				this.dust[i] = new Dust();
			}
		}

		// Token: 0x0600290C RID: 10508 RVA: 0x005770AC File Offset: 0x005752AC
		public Dust NewDustPerfect(Vector2 Position, int Type, Vector2? Velocity = null, int Alpha = 0, Color newColor = default(Color), float Scale = 1f)
		{
			Dust dust = this.NewDust(Position, 0, 0, Type, 0f, 0f, Alpha, newColor, Scale);
			dust.position = Position;
			if (Velocity != null)
			{
				dust.velocity = Velocity.Value;
			}
			return dust;
		}

		// Token: 0x0600290D RID: 10509 RVA: 0x005770F4 File Offset: 0x005752F4
		public Dust NewDustDirect(Vector2 Position, int Width, int Height, int Type, float SpeedX = 0f, float SpeedY = 0f, int Alpha = 0, Color newColor = default(Color), float Scale = 1f)
		{
			Dust dust = this.NewDust(Position, Width, Height, Type, SpeedX, SpeedY, Alpha, newColor, Scale);
			if (dust.velocity.HasNaNs())
			{
				dust.velocity = Vector2.Zero;
			}
			return dust;
		}

		// Token: 0x0600290E RID: 10510 RVA: 0x00577130 File Offset: 0x00575330
		public Dust NewDust(Vector2 Position, int Width, int Height, int Type, float SpeedX = 0f, float SpeedY = 0f, int Alpha = 0, Color newColor = default(Color), float Scale = 1f)
		{
			if (Type != 6 && Type != 267)
			{
				throw new Exception();
			}
			int num = 200;
			int i = 0;
			while (i < 200)
			{
				Dust dust = this.dust[i];
				if (!dust.active)
				{
					int num2 = Width;
					int num3 = Height;
					if (num2 < 5)
					{
						num2 = 5;
					}
					if (num3 < 5)
					{
						num3 = 5;
					}
					num = i;
					dust.fadeIn = 0f;
					dust.active = true;
					dust.type = Type;
					dust.noGravity = false;
					dust.color = newColor;
					dust.alpha = Alpha;
					dust.position.X = Position.X + (float)Main.rand.Next(num2 - 4) + 4f;
					dust.position.Y = Position.Y + (float)Main.rand.Next(num3 - 4) + 4f;
					dust.velocity.X = (float)Main.rand.Next(-20, 21) * 0.1f + SpeedX;
					dust.velocity.Y = (float)Main.rand.Next(-20, 21) * 0.1f + SpeedY;
					dust.frame.X = 10 * Type;
					dust.frame.Y = 10 * Main.rand.Next(3);
					dust.shader = null;
					dust.customData = null;
					dust.noLightEmittence = false;
					dust.fullBright = false;
					int j = Type;
					while (j >= 100)
					{
						j -= 100;
						Dust dust2 = dust;
						dust2.frame.X = dust2.frame.X - 1000;
						Dust dust3 = dust;
						dust3.frame.Y = dust3.frame.Y + 30;
					}
					dust.frame.Width = 8;
					dust.frame.Height = 8;
					dust.rotation = 0f;
					dust.scale = 1f + (float)Main.rand.Next(-20, 21) * 0.01f;
					dust.scale *= Scale;
					dust.noLight = false;
					dust.firstFrame = true;
					if (dust.type == 6)
					{
						dust.velocity.Y = (float)Main.rand.Next(-10, 6) * 0.1f;
						Dust dust4 = dust;
						dust4.velocity.X = dust4.velocity.X * 0.3f;
						dust.scale *= 0.7f;
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			return this.dust[num];
		}

		// Token: 0x0600290F RID: 10511 RVA: 0x00577390 File Offset: 0x00575590
		public Dust CloneDust(int dustIndex)
		{
			Dust rf = this.dust[dustIndex];
			return this.CloneDust(rf);
		}

		// Token: 0x06002910 RID: 10512 RVA: 0x005773B0 File Offset: 0x005755B0
		public Dust CloneDust(Dust rf)
		{
			Dust dust = this.NewDust(rf.position, 0, 0, rf.type, 0f, 0f, 0, default(Color), 1f);
			dust.position = rf.position;
			dust.velocity = rf.velocity;
			dust.fadeIn = rf.fadeIn;
			dust.noGravity = rf.noGravity;
			dust.scale = rf.scale;
			dust.rotation = rf.rotation;
			dust.noLight = rf.noLight;
			dust.active = rf.active;
			dust.type = rf.type;
			dust.color = rf.color;
			dust.alpha = rf.alpha;
			dust.frame = rf.frame;
			dust.shader = rf.shader;
			dust.customData = rf.customData;
			return dust;
		}

		// Token: 0x06002911 RID: 10513 RVA: 0x00577494 File Offset: 0x00575694
		public void QuickBox(Vector2 topLeft, Vector2 bottomRight, int divisions, Color color, Action<Dust> manipulator)
		{
			float num = (float)(divisions + 2);
			for (float num2 = 0f; num2 <= (float)(divisions + 2); num2 += 1f)
			{
				Dust obj = this.QuickDust(new Vector2(MathHelper.Lerp(topLeft.X, bottomRight.X, num2 / num), topLeft.Y), color);
				if (manipulator != null)
				{
					manipulator(obj);
				}
				obj = this.QuickDust(new Vector2(MathHelper.Lerp(topLeft.X, bottomRight.X, num2 / num), bottomRight.Y), color);
				if (manipulator != null)
				{
					manipulator(obj);
				}
				obj = this.QuickDust(new Vector2(topLeft.X, MathHelper.Lerp(topLeft.Y, bottomRight.Y, num2 / num)), color);
				if (manipulator != null)
				{
					manipulator(obj);
				}
				obj = this.QuickDust(new Vector2(bottomRight.X, MathHelper.Lerp(topLeft.Y, bottomRight.Y, num2 / num)), color);
				if (manipulator != null)
				{
					manipulator(obj);
				}
			}
		}

		// Token: 0x06002912 RID: 10514 RVA: 0x00577594 File Offset: 0x00575794
		public void QuickCircle(Vector2 center, float radius, int divisions, Color color, Action<Dust> manipulator)
		{
			float num = 1f / Math.Max(1f, (float)divisions);
			for (float num2 = 0f; num2 < 1f; num2 += num)
			{
				float num3 = num2 * 6.2831855f;
				Vector2 pos = center + new Vector2(radius, 0f).RotatedBy((double)num3, Vector2.Zero);
				Dust obj = this.QuickDust(pos, color);
				if (manipulator != null)
				{
					manipulator(obj);
				}
			}
		}

		// Token: 0x06002913 RID: 10515 RVA: 0x0057760C File Offset: 0x0057580C
		public Dust QuickDust(Vector2 pos, Color color)
		{
			Dust dust = this.NewDust(pos, 0, 0, 267, 0f, 0f, 0, default(Color), 1f);
			dust.position = pos;
			dust.velocity = Vector2.Zero;
			dust.fadeIn = 1f;
			dust.noLight = true;
			dust.noGravity = true;
			dust.color = color;
			return dust;
		}

		// Token: 0x06002914 RID: 10516 RVA: 0x00577674 File Offset: 0x00575874
		public Dust QuickDustSmall(Vector2 pos, Color color, bool floorPositionValues = false)
		{
			Dust dust = this.QuickDust(pos, color);
			dust.fadeIn = 0f;
			dust.scale = 0.35f;
			if (floorPositionValues)
			{
				dust.position = dust.position.Floor();
			}
			return dust;
		}

		// Token: 0x06002915 RID: 10517 RVA: 0x005776B8 File Offset: 0x005758B8
		public void QuickDustLine(Vector2 start, Vector2 end, float splits, Color color)
		{
			this.QuickDust(start, color).scale = 0.3f;
			this.QuickDust(end, color).scale = 0.3f;
			float num = 1f / splits;
			for (float num2 = 0f; num2 < 1f; num2 += num)
			{
				this.QuickDust(Vector2.Lerp(start, end, num2), color).scale = 0.3f;
			}
		}

		// Token: 0x06002916 RID: 10518 RVA: 0x00577724 File Offset: 0x00575924
		public void UpdateDust()
		{
			Sandstorm.ShowSandstormVisuals();
			for (int i = 0; i < 200; i++)
			{
				Dust dust = this.dust[i];
				if (dust.active)
				{
					if (dust.scale > 10f)
					{
						dust.active = false;
					}
					dust.position += dust.velocity;
					if (dust.type == 6)
					{
						if (!dust.noGravity)
						{
							Dust dust2 = dust;
							dust2.velocity.Y = dust2.velocity.Y + 0.05f;
						}
					}
					else if (dust.type == 267)
					{
						if (dust.velocity.X < 0f)
						{
							dust.rotation -= 1f;
						}
						else
						{
							dust.rotation += 1f;
						}
						Dust dust3 = dust;
						dust3.velocity.Y = dust3.velocity.Y * 0.98f;
						Dust dust4 = dust;
						dust4.velocity.X = dust4.velocity.X * 0.98f;
						dust.scale += 0.02f;
					}
					Dust dust5 = dust;
					dust5.velocity.X = dust5.velocity.X * 0.99f;
					dust.rotation += dust.velocity.X * 0.5f;
					if (dust.fadeIn > 0f && dust.fadeIn < 100f)
					{
						dust.scale += 0.03f;
						if (dust.scale > dust.fadeIn)
						{
							dust.fadeIn = 0f;
						}
					}
					else
					{
						dust.scale -= 0.01f;
					}
					if (dust.noGravity)
					{
						dust.velocity *= 0.92f;
						if (dust.fadeIn == 0f)
						{
							dust.scale -= 0.04f;
						}
					}
					float num = 0.1f;
					if (dust.scale < num)
					{
						dust.active = false;
					}
				}
			}
		}

		// Token: 0x06002917 RID: 10519 RVA: 0x00577914 File Offset: 0x00575B14
		internal void DrawDust()
		{
			SpriteBatch spriteBatch = Main.spriteBatch;
			for (int i = 0; i < 200; i++)
			{
				Dust dust = this.dust[i];
				if (dust.active)
				{
					float visualScale = dust.GetVisualScale();
					Color color = Lighting.GetColor((int)((double)dust.position.X + 4.0) / 16, (int)((double)dust.position.Y + 4.0) / 16);
					if (dust.type == 6)
					{
						color = Color.White;
					}
					color = dust.GetAlpha(color);
					spriteBatch.Draw(TextureAssets.Dust.Value, dust.position, new Rectangle?(dust.frame), color, dust.GetVisualRotation(), new Vector2(4f, 4f), visualScale, SpriteEffects.None, 0f);
					if (dust.color.PackedValue != 0U)
					{
						Color color2 = dust.GetColor(color);
						if (color2.PackedValue != 0U)
						{
							spriteBatch.Draw(TextureAssets.Dust.Value, dust.position, new Rectangle?(dust.frame), color2, dust.GetVisualRotation(), new Vector2(4f, 4f), visualScale, SpriteEffects.None, 0f);
						}
					}
					if (color == Color.Black)
					{
						dust.active = false;
					}
				}
			}
		}

		// Token: 0x0400518D RID: 20877
		public const int maxDust = 200;

		// Token: 0x0400518E RID: 20878
		public Dust[] dust = new Dust[201];
	}
}
