using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x0200040C RID: 1036
	public class UIHeader : UIElement
	{
		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x06002F83 RID: 12163 RVA: 0x005B3765 File Offset: 0x005B1965
		// (set) Token: 0x06002F84 RID: 12164 RVA: 0x005B3770 File Offset: 0x005B1970
		public string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				if (this._text != value)
				{
					this._text = value;
					if (!Main.dedServ)
					{
						Vector2 vector = FontAssets.DeathText.Value.MeasureString(this.Text);
						this.Width.Pixels = vector.X;
						this.Height.Pixels = vector.Y;
					}
					this.Width.Precent = 0f;
					this.Height.Precent = 0f;
					this.Recalculate();
				}
			}
		}

		// Token: 0x06002F85 RID: 12165 RVA: 0x005B37F7 File Offset: 0x005B19F7
		public UIHeader()
		{
			this.Text = "";
		}

		// Token: 0x06002F86 RID: 12166 RVA: 0x005B380A File Offset: 0x005B1A0A
		public UIHeader(string text)
		{
			this.Text = text;
		}

		// Token: 0x06002F87 RID: 12167 RVA: 0x005B381C File Offset: 0x005B1A1C
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = base.GetDimensions();
			float num = 1.2f;
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.DeathText.Value, this.Text, new Vector2(dimensions.X - num, dimensions.Y - num), Color.Black);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.DeathText.Value, this.Text, new Vector2(dimensions.X + num, dimensions.Y - num), Color.Black);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.DeathText.Value, this.Text, new Vector2(dimensions.X - num, dimensions.Y + num), Color.Black);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.DeathText.Value, this.Text, new Vector2(dimensions.X + num, dimensions.Y + num), Color.Black);
			if (WorldGen.tenthAnniversaryWorldGen && !Main.zenithWorld)
			{
				DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.DeathText.Value, this.Text, new Vector2(dimensions.X, dimensions.Y), Color.HotPink);
				return;
			}
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.DeathText.Value, this.Text, new Vector2(dimensions.X, dimensions.Y), Color.White);
		}

		// Token: 0x04005668 RID: 22120
		private string _text;
	}
}
