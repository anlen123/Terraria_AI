using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003D1 RID: 977
	public class UISelectableTextPanel<T> : UITextPanel<T>
	{
		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06002D96 RID: 11670 RVA: 0x005A439E File Offset: 0x005A259E
		// (set) Token: 0x06002D97 RID: 11671 RVA: 0x005A43A6 File Offset: 0x005A25A6
		public Func<UISelectableTextPanel<T>, bool> IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				this._isSelected = value;
			}
		}

		// Token: 0x06002D98 RID: 11672 RVA: 0x005A43AF File Offset: 0x005A25AF
		public UISelectableTextPanel(T text, float textScale = 1f, bool large = false) : base(text, textScale, large)
		{
			this._BasePanelTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/PanelGrayscale", 1);
			this._hoveredBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelBorder", 1);
		}

		// Token: 0x06002D99 RID: 11673 RVA: 0x005A43E8 File Offset: 0x005A25E8
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (this._drawPanel)
			{
				CalculatedStyle dimensions = base.GetDimensions();
				int num = 4;
				int num2 = 10;
				int num3 = 10;
				Utils.DrawSplicedPanel(spriteBatch, this._BasePanelTexture.Value, (int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height, num2, num2, num3, num3, Color.Lerp(Color.Black, this._color, 0.8f) * 0.5f);
				if (this.IsSelected != null && this.IsSelected(this))
				{
					Utils.DrawSplicedPanel(spriteBatch, this._BasePanelTexture.Value, (int)dimensions.X + num, (int)dimensions.Y + num, (int)dimensions.Width - num * 2, (int)dimensions.Height - num * 2, num2, num2, num3, num3, Color.Lerp(this._color, Color.White, 0.7f) * 0.5f);
				}
				if (base.IsMouseHovering)
				{
					Utils.DrawSplicedPanel(spriteBatch, this._hoveredBorderTexture.Value, (int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height, num2, num2, num3, num3, Color.White);
				}
			}
			base.DrawText(spriteBatch);
		}

		// Token: 0x040054C6 RID: 21702
		private readonly Asset<Texture2D> _BasePanelTexture;

		// Token: 0x040054C7 RID: 21703
		private readonly Asset<Texture2D> _hoveredBorderTexture;

		// Token: 0x040054C8 RID: 21704
		private Func<UISelectableTextPanel<T>, bool> _isSelected;
	}
}
