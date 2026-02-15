using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003F1 RID: 1009
	public class UIColoredSliderSimple : UIElement
	{
		// Token: 0x06002E8D RID: 11917 RVA: 0x005AB3E4 File Offset: 0x005A95E4
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			this.DrawValueBarDynamicWidth(spriteBatch);
		}

		// Token: 0x06002E8E RID: 11918 RVA: 0x005AB3F0 File Offset: 0x005A95F0
		private void DrawValueBarDynamicWidth(SpriteBatch sb)
		{
			Texture2D value = TextureAssets.ColorBar.Value;
			Rectangle rectangle = base.GetDimensions().ToRectangle();
			Rectangle rectangle2 = new Rectangle(5, 4, 4, 4);
			Utils.DrawSplicedPanel(sb, value, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, rectangle2.X, rectangle2.Width, rectangle2.Y, rectangle2.Height, Color.White);
			Rectangle rectangle3 = rectangle;
			rectangle3.X += rectangle2.Left;
			rectangle3.Width -= rectangle2.Right;
			rectangle3.Y += rectangle2.Top;
			rectangle3.Height -= rectangle2.Bottom;
			Texture2D value2 = TextureAssets.MagicPixel.Value;
			Rectangle value3 = new Rectangle(0, 0, 1, 1);
			sb.Draw(value2, rectangle3, new Rectangle?(value3), this.EmptyColor);
			Rectangle rectangle4 = rectangle3;
			rectangle4.Width = (int)((float)rectangle4.Width * this.FillPercent);
			sb.Draw(value2, rectangle4, new Rectangle?(value3), this.FilledColor);
		}

		// Token: 0x0400559F RID: 21919
		public float FillPercent;

		// Token: 0x040055A0 RID: 21920
		public Color FilledColor = Main.OurFavoriteColor;

		// Token: 0x040055A1 RID: 21921
		public Color EmptyColor = Color.Black;
	}
}
