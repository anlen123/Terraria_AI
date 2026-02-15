using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.DataStructures
{
	// Token: 0x02000593 RID: 1427
	public struct DrawData
	{
		// Token: 0x06003842 RID: 14402 RVA: 0x00630E98 File Offset: 0x0062F098
		public DrawData(Texture2D texture, Vector2 position, Color color)
		{
			this.texture = texture;
			this.position = position;
			this.color = color;
			this.destinationRectangle = default(Rectangle);
			this.sourceRect = DrawData.nullRectangle;
			this.rotation = 0f;
			this.origin = Vector2.Zero;
			this.scale = Vector2.One;
			this.effect = SpriteEffects.None;
			this.shader = 0;
			this.ignorePlayerRotation = false;
			this.useDestinationRectangle = false;
		}

		// Token: 0x06003843 RID: 14403 RVA: 0x00630F10 File Offset: 0x0062F110
		public DrawData(Texture2D texture, Vector2 position, Rectangle? sourceRect, Color color)
		{
			this.texture = texture;
			this.position = position;
			this.color = color;
			this.destinationRectangle = default(Rectangle);
			this.sourceRect = sourceRect;
			this.rotation = 0f;
			this.origin = Vector2.Zero;
			this.scale = Vector2.One;
			this.effect = SpriteEffects.None;
			this.shader = 0;
			this.ignorePlayerRotation = false;
			this.useDestinationRectangle = false;
		}

		// Token: 0x06003844 RID: 14404 RVA: 0x00630F84 File Offset: 0x0062F184
		public DrawData(Texture2D texture, Vector2 position, Rectangle? sourceRect, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effect, float inactiveLayerDepth = 0f)
		{
			this.texture = texture;
			this.position = position;
			this.sourceRect = sourceRect;
			this.color = color;
			this.rotation = rotation;
			this.origin = origin;
			this.scale = new Vector2(scale, scale);
			this.effect = effect;
			this.destinationRectangle = default(Rectangle);
			this.shader = 0;
			this.ignorePlayerRotation = false;
			this.useDestinationRectangle = false;
		}

		// Token: 0x06003845 RID: 14405 RVA: 0x00630FF8 File Offset: 0x0062F1F8
		public DrawData(Texture2D texture, Vector2 position, Rectangle? sourceRect, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effect, float inactiveLayerDepth = 0f)
		{
			this.texture = texture;
			this.position = position;
			this.sourceRect = sourceRect;
			this.color = color;
			this.rotation = rotation;
			this.origin = origin;
			this.scale = scale;
			this.effect = effect;
			this.destinationRectangle = default(Rectangle);
			this.shader = 0;
			this.ignorePlayerRotation = false;
			this.useDestinationRectangle = false;
		}

		// Token: 0x06003846 RID: 14406 RVA: 0x00631064 File Offset: 0x0062F264
		public DrawData(Texture2D texture, Rectangle destinationRectangle, Color color)
		{
			this.texture = texture;
			this.destinationRectangle = destinationRectangle;
			this.color = color;
			this.position = Vector2.Zero;
			this.sourceRect = DrawData.nullRectangle;
			this.rotation = 0f;
			this.origin = Vector2.Zero;
			this.scale = Vector2.One;
			this.effect = SpriteEffects.None;
			this.shader = 0;
			this.ignorePlayerRotation = false;
			this.useDestinationRectangle = false;
		}

		// Token: 0x06003847 RID: 14407 RVA: 0x006310DC File Offset: 0x0062F2DC
		public DrawData(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRect, Color color)
		{
			this.texture = texture;
			this.destinationRectangle = destinationRectangle;
			this.color = color;
			this.position = Vector2.Zero;
			this.sourceRect = sourceRect;
			this.rotation = 0f;
			this.origin = Vector2.Zero;
			this.scale = Vector2.One;
			this.effect = SpriteEffects.None;
			this.shader = 0;
			this.ignorePlayerRotation = false;
			this.useDestinationRectangle = false;
		}

		// Token: 0x06003848 RID: 14408 RVA: 0x00631150 File Offset: 0x0062F350
		public DrawData(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRect, Color color, float rotation, Vector2 origin, SpriteEffects effect, float inactiveLayerDepth = 0f)
		{
			this.texture = texture;
			this.destinationRectangle = destinationRectangle;
			this.sourceRect = sourceRect;
			this.color = color;
			this.rotation = rotation;
			this.origin = origin;
			this.effect = effect;
			this.position = Vector2.Zero;
			this.scale = Vector2.One;
			this.shader = 0;
			this.ignorePlayerRotation = false;
			this.useDestinationRectangle = false;
		}

		// Token: 0x06003849 RID: 14409 RVA: 0x006311C0 File Offset: 0x0062F3C0
		public void Draw(SpriteBatch sb)
		{
			if (this.useDestinationRectangle)
			{
				sb.Draw(this.texture, this.destinationRectangle, this.sourceRect, this.color, this.rotation, this.origin, this.effect, 0f);
				return;
			}
			sb.Draw(this.texture, this.position, this.sourceRect, this.color, this.rotation, this.origin, this.scale, this.effect, 0f);
		}

		// Token: 0x0600384A RID: 14410 RVA: 0x00631248 File Offset: 0x0062F448
		public void Draw(SpriteDrawBuffer sb)
		{
			if (this.useDestinationRectangle)
			{
				sb.Draw(this.texture, this.destinationRectangle, this.sourceRect, this.color, this.rotation, this.origin, this.effect);
				return;
			}
			sb.Draw(this.texture, this.position, this.sourceRect, this.color, this.rotation, this.origin, this.scale, this.effect);
		}

		// Token: 0x04005C4E RID: 23630
		public Texture2D texture;

		// Token: 0x04005C4F RID: 23631
		public Vector2 position;

		// Token: 0x04005C50 RID: 23632
		public Rectangle destinationRectangle;

		// Token: 0x04005C51 RID: 23633
		public Rectangle? sourceRect;

		// Token: 0x04005C52 RID: 23634
		public Color color;

		// Token: 0x04005C53 RID: 23635
		public float rotation;

		// Token: 0x04005C54 RID: 23636
		public Vector2 origin;

		// Token: 0x04005C55 RID: 23637
		public Vector2 scale;

		// Token: 0x04005C56 RID: 23638
		public SpriteEffects effect;

		// Token: 0x04005C57 RID: 23639
		public int shader;

		// Token: 0x04005C58 RID: 23640
		public bool ignorePlayerRotation;

		// Token: 0x04005C59 RID: 23641
		public readonly bool useDestinationRectangle;

		// Token: 0x04005C5A RID: 23642
		public static Rectangle? nullRectangle;
	}
}
