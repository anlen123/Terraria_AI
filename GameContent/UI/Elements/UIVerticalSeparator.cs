using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003FA RID: 1018
	public class UIVerticalSeparator : UIElement
	{
		// Token: 0x06002EBD RID: 11965 RVA: 0x005AD824 File Offset: 0x005ABA24
		public UIVerticalSeparator()
		{
			this.Color = Color.White;
			this._texture = Main.Assets.Request<Texture2D>("Images/UI/OnePixel", 1);
			this.Width.Set((float)this._texture.Width(), 0f);
			this.Height.Set((float)this._texture.Height(), 0f);
		}

		// Token: 0x06002EBE RID: 11966 RVA: 0x005AD890 File Offset: 0x005ABA90
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = base.GetDimensions();
			spriteBatch.Draw(this._texture.Value, dimensions.ToRectangle(), this.Color);
		}

		// Token: 0x06002EBF RID: 11967 RVA: 0x001DA9FB File Offset: 0x001D8BFB
		public override bool ContainsPoint(Vector2 point)
		{
			return false;
		}

		// Token: 0x040055CE RID: 21966
		private Asset<Texture2D> _texture;

		// Token: 0x040055CF RID: 21967
		public Color Color;

		// Token: 0x040055D0 RID: 21968
		public int EdgeWidth;
	}
}
