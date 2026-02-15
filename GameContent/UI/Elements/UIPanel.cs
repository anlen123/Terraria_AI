using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x02000403 RID: 1027
	public class UIPanel : UIElement
	{
		// Token: 0x06002F14 RID: 12052 RVA: 0x005B0554 File Offset: 0x005AE754
		public UIPanel()
		{
			if (this._borderTexture == null)
			{
				this._borderTexture = Main.Assets.Request<Texture2D>("Images/UI/PanelBorder", 1);
			}
			if (this._backgroundTexture == null)
			{
				this._backgroundTexture = Main.Assets.Request<Texture2D>("Images/UI/PanelBackground", 1);
			}
			base.SetPadding((float)this._cornerSize);
		}

		// Token: 0x06002F15 RID: 12053 RVA: 0x005B05E8 File Offset: 0x005AE7E8
		public UIPanel(Asset<Texture2D> customBackground, Asset<Texture2D> customborder, int customCornerSize = 12, int customBarSize = 4)
		{
			if (this._borderTexture == null)
			{
				this._borderTexture = customborder;
			}
			if (this._backgroundTexture == null)
			{
				this._backgroundTexture = customBackground;
			}
			this._cornerSize = customCornerSize;
			this._barSize = customBarSize;
			base.SetPadding((float)this._cornerSize);
		}

		// Token: 0x06002F16 RID: 12054 RVA: 0x005B0670 File Offset: 0x005AE870
		private void DrawPanel(SpriteBatch spriteBatch, Texture2D texture, Color color)
		{
			CalculatedStyle dimensions = base.GetDimensions();
			Point point = new Point((int)dimensions.X, (int)dimensions.Y);
			Point point2 = new Point(point.X + (int)dimensions.Width - this._cornerSize, point.Y + (int)dimensions.Height - this._cornerSize);
			int width = point2.X - point.X - this._cornerSize;
			int height = point2.Y - point.Y - this._cornerSize;
			spriteBatch.Draw(texture, new Rectangle(point.X, point.Y, this._cornerSize, this._cornerSize), new Rectangle?(new Rectangle(0, 0, this._cornerSize, this._cornerSize)), color);
			spriteBatch.Draw(texture, new Rectangle(point2.X, point.Y, this._cornerSize, this._cornerSize), new Rectangle?(new Rectangle(this._cornerSize + this._barSize, 0, this._cornerSize, this._cornerSize)), color);
			spriteBatch.Draw(texture, new Rectangle(point.X, point2.Y, this._cornerSize, this._cornerSize), new Rectangle?(new Rectangle(0, this._cornerSize + this._barSize, this._cornerSize, this._cornerSize)), color);
			spriteBatch.Draw(texture, new Rectangle(point2.X, point2.Y, this._cornerSize, this._cornerSize), new Rectangle?(new Rectangle(this._cornerSize + this._barSize, this._cornerSize + this._barSize, this._cornerSize, this._cornerSize)), color);
			spriteBatch.Draw(texture, new Rectangle(point.X + this._cornerSize, point.Y, width, this._cornerSize), new Rectangle?(new Rectangle(this._cornerSize, 0, this._barSize, this._cornerSize)), color);
			spriteBatch.Draw(texture, new Rectangle(point.X + this._cornerSize, point2.Y, width, this._cornerSize), new Rectangle?(new Rectangle(this._cornerSize, this._cornerSize + this._barSize, this._barSize, this._cornerSize)), color);
			spriteBatch.Draw(texture, new Rectangle(point.X, point.Y + this._cornerSize, this._cornerSize, height), new Rectangle?(new Rectangle(0, this._cornerSize, this._cornerSize, this._barSize)), color);
			spriteBatch.Draw(texture, new Rectangle(point2.X, point.Y + this._cornerSize, this._cornerSize, height), new Rectangle?(new Rectangle(this._cornerSize + this._barSize, this._cornerSize, this._cornerSize, this._barSize)), color);
			spriteBatch.Draw(texture, new Rectangle(point.X + this._cornerSize, point.Y + this._cornerSize, width, height), new Rectangle?(new Rectangle(this._cornerSize, this._cornerSize, this._barSize, this._barSize)), color);
		}

		// Token: 0x06002F17 RID: 12055 RVA: 0x005B0990 File Offset: 0x005AEB90
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (this._backgroundTexture != null)
			{
				this.DrawPanel(spriteBatch, this._backgroundTexture.Value, this.BackgroundColor);
			}
			if (this._borderTexture != null)
			{
				this.DrawPanel(spriteBatch, this._borderTexture.Value, this.BorderColor);
			}
		}

		// Token: 0x04005615 RID: 22037
		private int _cornerSize = 12;

		// Token: 0x04005616 RID: 22038
		private int _barSize = 4;

		// Token: 0x04005617 RID: 22039
		private Asset<Texture2D> _borderTexture;

		// Token: 0x04005618 RID: 22040
		private Asset<Texture2D> _backgroundTexture;

		// Token: 0x04005619 RID: 22041
		public Color BorderColor = Color.Black;

		// Token: 0x0400561A RID: 22042
		public Color BackgroundColor = new Color(63, 82, 151) * 0.7f;
	}
}
