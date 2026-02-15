using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.IO;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003D0 RID: 976
	public class UIResourcePackInfoButton<T> : UITextPanel<T>
	{
		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06002D92 RID: 11666 RVA: 0x005A429A File Offset: 0x005A249A
		// (set) Token: 0x06002D93 RID: 11667 RVA: 0x005A42A2 File Offset: 0x005A24A2
		public ResourcePack ResourcePack
		{
			get
			{
				return this._resourcePack;
			}
			set
			{
				this._resourcePack = value;
			}
		}

		// Token: 0x06002D94 RID: 11668 RVA: 0x005A42AB File Offset: 0x005A24AB
		public UIResourcePackInfoButton(T text, float textScale = 1f, bool large = false) : base(text, textScale, large)
		{
			this._BasePanelTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/PanelGrayscale", 1);
			this._hoveredBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelBorder", 1);
		}

		// Token: 0x06002D95 RID: 11669 RVA: 0x005A42E4 File Offset: 0x005A24E4
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (this._drawPanel)
			{
				CalculatedStyle dimensions = base.GetDimensions();
				int num = 10;
				int num2 = 10;
				Utils.DrawSplicedPanel(spriteBatch, this._BasePanelTexture.Value, (int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height, num, num, num2, num2, Color.Lerp(Color.Black, this._color, 0.8f) * 0.5f);
				if (base.IsMouseHovering)
				{
					Utils.DrawSplicedPanel(spriteBatch, this._hoveredBorderTexture.Value, (int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height, num, num, num2, num2, Color.White);
				}
			}
			base.DrawText(spriteBatch);
		}

		// Token: 0x040054C3 RID: 21699
		private readonly Asset<Texture2D> _BasePanelTexture;

		// Token: 0x040054C4 RID: 21700
		private readonly Asset<Texture2D> _hoveredBorderTexture;

		// Token: 0x040054C5 RID: 21701
		private ResourcePack _resourcePack;
	}
}
