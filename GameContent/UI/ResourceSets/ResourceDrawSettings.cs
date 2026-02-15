using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.GameContent.UI.ResourceSets
{
	// Token: 0x020003BF RID: 959
	public struct ResourceDrawSettings
	{
		// Token: 0x06002D09 RID: 11529 RVA: 0x005A1554 File Offset: 0x0059F754
		public void Draw(SpriteBatch spriteBatch, ref bool isHovered)
		{
			int elementCount = this.ElementCount;
			Vector2 value = this.TopLeftAnchor;
			Point value2 = Main.MouseScreen.ToPoint();
			for (int i = 0; i < elementCount; i++)
			{
				int elementIndex = i + this.ElementIndexOffset;
				Asset<Texture2D> asset;
				Vector2 value3;
				float scale;
				Rectangle? rectangle;
				this.GetTextureMethod(elementIndex, this.ElementIndexOffset, this.ElementIndexOffset + elementCount - 1, out asset, out value3, out scale, out rectangle);
				Rectangle rectangle2 = asset.Frame(1, 1, 0, 0, 0, 0);
				if (rectangle != null)
				{
					rectangle2 = rectangle.Value;
				}
				Vector2 vector = value + value3;
				Vector2 vector2 = this.OffsetSpriteAnchor + rectangle2.Size() * this.OffsetSpriteAnchorByTexturePercentile;
				Rectangle rectangle3 = rectangle2;
				rectangle3.X += (int)(vector.X - vector2.X);
				rectangle3.Y += (int)(vector.Y - vector2.Y);
				if (rectangle3.Contains(value2))
				{
					isHovered = true;
				}
				spriteBatch.Draw(asset.Value, vector, new Rectangle?(rectangle2), Color.White, 0f, vector2, scale, SpriteEffects.None, 0f);
				value += this.OffsetPerDraw + rectangle2.Size() * this.OffsetPerDrawByTexturePercentile;
			}
		}

		// Token: 0x04005469 RID: 21609
		public Vector2 TopLeftAnchor;

		// Token: 0x0400546A RID: 21610
		public int ElementCount;

		// Token: 0x0400546B RID: 21611
		public int ElementIndexOffset;

		// Token: 0x0400546C RID: 21612
		public ResourceDrawSettings.TextureGetter GetTextureMethod;

		// Token: 0x0400546D RID: 21613
		public Vector2 OffsetPerDraw;

		// Token: 0x0400546E RID: 21614
		public Vector2 OffsetPerDrawByTexturePercentile;

		// Token: 0x0400546F RID: 21615
		public Vector2 OffsetSpriteAnchor;

		// Token: 0x04005470 RID: 21616
		public Vector2 OffsetSpriteAnchorByTexturePercentile;

		// Token: 0x02000920 RID: 2336
		// (Invoke) Token: 0x060047DC RID: 18396
		public delegate void TextureGetter(int elementIndex, int firstElementIndex, int lastElementIndex, out Asset<Texture2D> texture, out Vector2 drawOffset, out float drawScale, out Rectangle? sourceRect);
	}
}
