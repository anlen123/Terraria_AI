using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003CA RID: 970
	public class UIIconTextButton : UIElement
	{
		// Token: 0x06002D5C RID: 11612 RVA: 0x005A2BD8 File Offset: 0x005A0DD8
		public UIIconTextButton(LocalizedText title, Color textColor, string iconTexturePath, float textSize = 1f, float titleAlignmentX = 0.5f, float titleWidthReduction = 10f)
		{
			this.Width = StyleDimension.FromPixels(44f);
			this.Height = StyleDimension.FromPixels(34f);
			this._hoverColor = Color.White;
			this._BasePanelTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/PanelGrayscale", 1);
			this._hoveredTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight", 1);
			if (iconTexturePath != null)
			{
				this._iconTexture = Main.Assets.Request<Texture2D>(iconTexturePath, 1);
			}
			this.SetColor(Color.Lerp(Color.Black, Colors.InventoryDefaultColor, this.FadeFromBlack), 1f);
			if (title != null)
			{
				this.SetText(title, textSize, textColor);
			}
		}

		// Token: 0x06002D5D RID: 11613 RVA: 0x005A2CA8 File Offset: 0x005A0EA8
		public void SetText(LocalizedText text, float textSize, Color color)
		{
			if (this._title != null)
			{
				this._title.Remove();
			}
			UIText uitext = new UIText(text, textSize, false)
			{
				HAlign = 0f,
				VAlign = 0.5f,
				Top = StyleDimension.FromPixels(0f),
				Left = StyleDimension.FromPixelsAndPercent(10f, 0f),
				IgnoresMouseInteraction = true
			};
			uitext.TextColor = color;
			base.Append(uitext);
			this._title = uitext;
			if (this._iconTexture != null)
			{
				this.Width.Set(this._title.GetDimensions().Width + (float)this._iconTexture.Width() + 26f, 0f);
				this.Height.Set(Math.Max(this._title.GetDimensions().Height, (float)this._iconTexture.Height()) + 16f, 0f);
			}
		}

		// Token: 0x06002D5E RID: 11614 RVA: 0x005A2D9C File Offset: 0x005A0F9C
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (this._hovered)
			{
				if (!this._soundedHover)
				{
					SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
				}
				this._soundedHover = true;
			}
			else
			{
				this._soundedHover = false;
			}
			CalculatedStyle dimensions = base.GetDimensions();
			Color color = this._color;
			float opacity = this._opacity;
			Utils.DrawSplicedPanel(spriteBatch, this._BasePanelTexture.Value, (int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height, 10, 10, 10, 10, Color.Lerp(Color.Black, color, this.FadeFromBlack) * opacity);
			if (this._iconTexture != null)
			{
				Color color2 = Color.Lerp(color, Color.White, this._whiteLerp) * opacity;
				spriteBatch.Draw(this._iconTexture.Value, new Vector2(dimensions.X + dimensions.Width - (float)this._iconTexture.Width() - 5f, dimensions.Center().Y - (float)(this._iconTexture.Height() / 2)), color2);
			}
		}

		// Token: 0x06002D5F RID: 11615 RVA: 0x005A2EB1 File Offset: 0x005A10B1
		public override void LeftMouseDown(UIMouseEvent evt)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			base.LeftMouseDown(evt);
		}

		// Token: 0x06002D60 RID: 11616 RVA: 0x005A2ECF File Offset: 0x005A10CF
		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			this.SetColor(Color.Lerp(Colors.InventoryDefaultColor, Color.White, this._whiteLerp), 0.7f);
			this._hovered = true;
		}

		// Token: 0x06002D61 RID: 11617 RVA: 0x005A2EFF File Offset: 0x005A10FF
		public override void MouseOut(UIMouseEvent evt)
		{
			base.MouseOut(evt);
			this.SetColor(Color.Lerp(Color.Black, Colors.InventoryDefaultColor, this.FadeFromBlack), 1f);
			this._hovered = false;
		}

		// Token: 0x06002D62 RID: 11618 RVA: 0x005A2F2F File Offset: 0x005A112F
		public void SetColor(Color color, float opacity)
		{
			this._color = color;
			this._opacity = opacity;
		}

		// Token: 0x06002D63 RID: 11619 RVA: 0x005A2F3F File Offset: 0x005A113F
		public void SetHoverColor(Color color)
		{
			this._hoverColor = color;
		}

		// Token: 0x04005499 RID: 21657
		private readonly Asset<Texture2D> _BasePanelTexture;

		// Token: 0x0400549A RID: 21658
		private readonly Asset<Texture2D> _hoveredTexture;

		// Token: 0x0400549B RID: 21659
		private readonly Asset<Texture2D> _iconTexture;

		// Token: 0x0400549C RID: 21660
		private Color _color;

		// Token: 0x0400549D RID: 21661
		private Color _hoverColor;

		// Token: 0x0400549E RID: 21662
		public float FadeFromBlack = 1f;

		// Token: 0x0400549F RID: 21663
		private float _whiteLerp = 0.7f;

		// Token: 0x040054A0 RID: 21664
		private float _opacity = 0.7f;

		// Token: 0x040054A1 RID: 21665
		private bool _hovered;

		// Token: 0x040054A2 RID: 21666
		private bool _soundedHover;

		// Token: 0x040054A3 RID: 21667
		private UIText _title;
	}
}
