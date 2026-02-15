using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.UI;

namespace Terraria.Map
{
	// Token: 0x0200017C RID: 380
	public struct MapOverlayDrawContext
	{
		// Token: 0x06001E25 RID: 7717 RVA: 0x0050298B File Offset: 0x00500B8B
		public MapOverlayDrawContext(Vector2 mapPosition, Vector2 mapOffset, Rectangle? clippingRect, float mapScale, float drawScale, float opacity)
		{
			this._mapPosition = mapPosition;
			this._mapOffset = mapOffset;
			this._clippingRect = clippingRect;
			this._mapScale = mapScale;
			this._drawScale = drawScale;
			this._opacity = opacity;
		}

		// Token: 0x06001E26 RID: 7718 RVA: 0x005029BA File Offset: 0x00500BBA
		public MapOverlayDrawContext.DrawResult Draw(Texture2D texture, Vector2 position, Alignment alignment)
		{
			return this.Draw(texture, position, new SpriteFrame(1, 1), alignment);
		}

		// Token: 0x06001E27 RID: 7719 RVA: 0x005029CC File Offset: 0x00500BCC
		public MapOverlayDrawContext.DrawResult Draw(Texture2D texture, Vector2 position, SpriteFrame frame, Alignment alignment)
		{
			if (this._opacity == 0f)
			{
				return new MapOverlayDrawContext.DrawResult(false);
			}
			position = (position - this._mapPosition) * this._mapScale + this._mapOffset;
			if (this._clippingRect != null && !this._clippingRect.Value.Contains(position.ToPoint()))
			{
				return MapOverlayDrawContext.DrawResult.Culled;
			}
			Rectangle sourceRectangle = frame.GetSourceRectangle(texture);
			Vector2 vector = sourceRectangle.Size() * alignment.OffsetMultiplier;
			Main.spriteBatch.Draw(texture, position, new Rectangle?(sourceRectangle), Color.White * this._opacity, 0f, vector, this._drawScale, SpriteEffects.None, 0f);
			position -= vector * this._drawScale;
			Rectangle rectangle = new Rectangle((int)position.X, (int)position.Y, (int)((float)sourceRectangle.Width * this._drawScale), (int)((float)sourceRectangle.Height * this._drawScale));
			return new MapOverlayDrawContext.DrawResult(rectangle.Contains(Main.MouseScreen.ToPoint()));
		}

		// Token: 0x06001E28 RID: 7720 RVA: 0x00502AF4 File Offset: 0x00500CF4
		public Rectangle GetUnclampedDrawRegion(Texture2D texture, Vector2 position, SpriteFrame frame, float scale, Alignment alignment)
		{
			position = (position - this._mapPosition) * this._mapScale + this._mapOffset;
			Rectangle sourceRectangle = frame.GetSourceRectangle(texture);
			Vector2 value = sourceRectangle.Size() * alignment.OffsetMultiplier;
			float num = this._drawScale * scale;
			Vector2 vector = position - value * num;
			return new Rectangle((int)vector.X, (int)vector.Y, (int)((float)sourceRectangle.Width * num), (int)((float)sourceRectangle.Height * num));
		}

		// Token: 0x06001E29 RID: 7721 RVA: 0x00502B80 File Offset: 0x00500D80
		public Rectangle GetClampedDrawRegion(Texture2D texture, Vector2 position, SpriteFrame frame, float scale, Alignment alignment, int screenBorderRegion)
		{
			position = (position - this._mapPosition) * this._mapScale + this._mapOffset;
			Rectangle sourceRectangle = frame.GetSourceRectangle(texture);
			Vector2 value = sourceRectangle.Size() * alignment.OffsetMultiplier;
			float num = this._drawScale * scale;
			Vector2 vector = position - value * num;
			Rectangle rectangle = new Rectangle((int)vector.X, (int)vector.Y, (int)((float)sourceRectangle.Width * num), (int)((float)sourceRectangle.Height * num));
			int num2 = Main.screenWidth - screenBorderRegion;
			int num3 = Main.screenHeight - screenBorderRegion;
			int num4 = (screenBorderRegion + num2) / 2;
			int num5 = (screenBorderRegion + num3) / 2;
			if (rectangle.X < screenBorderRegion)
			{
				float num6 = (float)(rectangle.X - num4);
				float num7 = (float)(rectangle.Y - num5);
				int num8 = rectangle.X - screenBorderRegion;
				int num9 = (int)((float)num8 / num6 * num7);
				rectangle.X -= num8;
				rectangle.Y -= num9;
			}
			else if (rectangle.X + rectangle.Width > num2)
			{
				float num10 = (float)(rectangle.X - num4);
				float num11 = (float)(rectangle.Y - num5);
				int num12 = rectangle.X + rectangle.Width - num2;
				int num13 = (int)((float)num12 / num10 * num11);
				rectangle.X -= num12;
				rectangle.Y -= num13;
			}
			if (rectangle.Y < screenBorderRegion)
			{
				float num14 = (float)(rectangle.X - num4);
				float num15 = (float)(rectangle.Y - num5);
				int num16 = rectangle.Y - screenBorderRegion;
				int num17 = (int)((float)num16 / num15 * num14);
				rectangle.X -= num17;
				rectangle.Y -= num16;
			}
			else if (rectangle.Y + rectangle.Height > num3)
			{
				float num18 = (float)(rectangle.X - num4);
				float num19 = (float)(rectangle.Y - num5);
				int num20 = rectangle.Y + rectangle.Height - num3;
				int num21 = (int)((float)num20 / num19 * num18);
				rectangle.X -= num21;
				rectangle.Y -= num20;
			}
			return rectangle;
		}

		// Token: 0x06001E2A RID: 7722 RVA: 0x00502DC0 File Offset: 0x00500FC0
		public MapOverlayDrawContext.DrawResult DrawClamped(Texture2D texture, Texture2D offscreenTexture, Vector2 position, Color color, SpriteFrame frame, float scaleIfNotSelected, float scaleIfSelected, float scaleIfOffscreen, Alignment alignment, int screenBorderRegion, out bool onScreen)
		{
			onScreen = true;
			if (this._opacity == 0f)
			{
				return new MapOverlayDrawContext.DrawResult(false);
			}
			position = (position - this._mapPosition) * this._mapScale + this._mapOffset;
			Rectangle sourceRectangle = frame.GetSourceRectangle(texture);
			Vector2 vector = sourceRectangle.Size() * alignment.OffsetMultiplier;
			Vector2 vector2 = position;
			float num = this._drawScale * scaleIfNotSelected;
			float num2 = this._drawScale * scaleIfOffscreen;
			Vector2 vector3 = position - vector * num;
			Rectangle rectangle = new Rectangle((int)vector3.X, (int)vector3.Y, (int)((float)sourceRectangle.Width * num), (int)((float)sourceRectangle.Height * num));
			int num3 = Main.screenWidth - screenBorderRegion;
			int num4 = Main.screenHeight - screenBorderRegion;
			int num5 = (screenBorderRegion + num3) / 2;
			int num6 = (screenBorderRegion + num4) / 2;
			if (rectangle.X < screenBorderRegion)
			{
				float num7 = (float)(rectangle.X - num5);
				float num8 = (float)(rectangle.Y - num6);
				int num9 = rectangle.X - screenBorderRegion;
				int num10 = (int)((float)num9 / num7 * num8);
				vector2.X -= (float)num9;
				rectangle.X -= num9;
				vector2.Y -= (float)num10;
				rectangle.Y -= num10;
				onScreen = false;
			}
			else if (rectangle.X + rectangle.Width > num3)
			{
				onScreen = false;
				float num11 = (float)(rectangle.X - num5);
				float num12 = (float)(rectangle.Y - num6);
				int num13 = rectangle.X + rectangle.Width - num3;
				int num14 = (int)((float)num13 / num11 * num12);
				vector2.X -= (float)num13;
				rectangle.X -= num13;
				vector2.Y -= (float)num14;
				rectangle.Y -= num14;
			}
			if (rectangle.Y < screenBorderRegion)
			{
				onScreen = false;
				float num15 = (float)(rectangle.X - num5);
				float num16 = (float)(rectangle.Y - num6);
				int num17 = rectangle.Y - screenBorderRegion;
				int num18 = (int)((float)num17 / num16 * num15);
				vector2.X -= (float)num18;
				rectangle.X -= num18;
				vector2.Y -= (float)num17;
				rectangle.Y -= num17;
			}
			else if (rectangle.Y + rectangle.Height > num4)
			{
				onScreen = false;
				float num19 = (float)(rectangle.X - num5);
				float num20 = (float)(rectangle.Y - num6);
				int num21 = rectangle.Y + rectangle.Height - num4;
				int num22 = (int)((float)num21 / num20 * num19);
				vector2.X -= (float)num22;
				rectangle.X -= num22;
				vector2.Y -= (float)num21;
				rectangle.Y -= num21;
			}
			bool flag = rectangle.Contains(Main.MouseScreen.ToPoint());
			float num23 = num;
			if (!onScreen)
			{
				num23 = num2;
				if (flag)
				{
					num23 *= scaleIfSelected;
				}
			}
			else if (flag)
			{
				num23 = this._drawScale * scaleIfSelected;
			}
			if (!onScreen && !flag)
			{
				int frameX = 2;
				int frameY = 1;
				int num24 = offscreenTexture.Width / 3;
				int num25 = offscreenTexture.Height / 3;
				Vector2 vector4 = position - vector2;
				float rotation = vector4.ToRotation();
				vector4.Normalize();
				Vector2 vector5 = rectangle.Center.ToVector2();
				vector5 += vector4 * ((float)(sourceRectangle.Height / 4) * num2);
				Rectangle rectangle2 = offscreenTexture.Frame(3, 3, frameX, frameY, 0, 0);
				Vector2 origin = new Vector2(0f, (float)rectangle2.Height * 0.5f);
				Main.spriteBatch.Draw(offscreenTexture, vector5, new Rectangle?(rectangle2), Color.White * this._opacity, rotation, origin, num2, SpriteEffects.None, 0f);
			}
			Main.spriteBatch.Draw(texture, vector2, new Rectangle?(sourceRectangle), color * this._opacity, 0f, vector, num23, SpriteEffects.None, 0f);
			return new MapOverlayDrawContext.DrawResult(flag);
		}

		// Token: 0x06001E2B RID: 7723 RVA: 0x005031E0 File Offset: 0x005013E0
		public MapOverlayDrawContext.DrawResult Draw(Texture2D texture, Vector2 position, Color color, SpriteFrame frame, float scaleIfNotSelected, float scaleIfSelected, Alignment alignment)
		{
			if (this._opacity == 0f)
			{
				return new MapOverlayDrawContext.DrawResult(false);
			}
			position = (position - this._mapPosition) * this._mapScale + this._mapOffset;
			if (this._clippingRect != null && !this._clippingRect.Value.Contains(position.ToPoint()))
			{
				return MapOverlayDrawContext.DrawResult.Culled;
			}
			Rectangle sourceRectangle = frame.GetSourceRectangle(texture);
			Vector2 vector = sourceRectangle.Size() * alignment.OffsetMultiplier;
			Vector2 position2 = position;
			float num = this._drawScale * scaleIfNotSelected;
			Vector2 vector2 = position - vector * num;
			Rectangle rectangle = new Rectangle((int)vector2.X, (int)vector2.Y, (int)((float)sourceRectangle.Width * num), (int)((float)sourceRectangle.Height * num));
			bool flag = rectangle.Contains(Main.MouseScreen.ToPoint());
			float scale = num;
			if (flag)
			{
				scale = this._drawScale * scaleIfSelected;
			}
			Main.spriteBatch.Draw(texture, position2, new Rectangle?(sourceRectangle), color * this._opacity, 0f, vector, scale, SpriteEffects.None, 0f);
			return new MapOverlayDrawContext.DrawResult(flag);
		}

		// Token: 0x0400167D RID: 5757
		private readonly Vector2 _mapPosition;

		// Token: 0x0400167E RID: 5758
		private readonly Vector2 _mapOffset;

		// Token: 0x0400167F RID: 5759
		private readonly Rectangle? _clippingRect;

		// Token: 0x04001680 RID: 5760
		private readonly float _mapScale;

		// Token: 0x04001681 RID: 5761
		private readonly float _drawScale;

		// Token: 0x04001682 RID: 5762
		private readonly float _opacity;

		// Token: 0x0200074F RID: 1871
		public struct DrawResult
		{
			// Token: 0x060040DA RID: 16602 RVA: 0x0069D754 File Offset: 0x0069B954
			public DrawResult(bool isMouseOver)
			{
				this.IsMouseOver = isMouseOver;
			}

			// Token: 0x040069A1 RID: 27041
			public static readonly MapOverlayDrawContext.DrawResult Culled = new MapOverlayDrawContext.DrawResult(false);

			// Token: 0x040069A2 RID: 27042
			public readonly bool IsMouseOver;
		}
	}
}
