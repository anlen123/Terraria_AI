using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003E9 RID: 1001
	public class UICharacterNameButton : UIElement
	{
		// Token: 0x06002E5C RID: 11868 RVA: 0x005A9D9C File Offset: 0x005A7F9C
		public UICharacterNameButton(LocalizedText titleText, LocalizedText emptyContentText, LocalizedText description = null)
		{
			this.Width = StyleDimension.FromPixels(400f);
			this.Height = StyleDimension.FromPixels(40f);
			this.Description = description;
			this._BasePanelTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanel", 1);
			this._selectedBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight", 1);
			this._hoveredBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelBorder", 1);
			this._textToShowWhenEmpty = emptyContentText;
			float textScale = 1f;
			UIText uitext = new UIText(titleText, textScale, false)
			{
				HAlign = 0f,
				VAlign = 0.5f,
				Left = StyleDimension.FromPixels(10f)
			};
			base.Append(uitext);
			this._title = uitext;
			UIText uitext2 = new UIText(Language.GetText("UI.PlayerNameSlot"), textScale, false)
			{
				HAlign = 0f,
				VAlign = 0.5f,
				Left = StyleDimension.FromPixels(150f)
			};
			base.Append(uitext2);
			this._text = uitext2;
			this.SetContents(null);
		}

		// Token: 0x06002E5D RID: 11869 RVA: 0x005A9EBC File Offset: 0x005A80BC
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
			Utils.DrawSplicedPanel(spriteBatch, this._BasePanelTexture.Value, (int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height, 10, 10, 10, 10, Color.White * 0.5f);
			if (this._hovered)
			{
				Utils.DrawSplicedPanel(spriteBatch, this._hoveredBorderTexture.Value, (int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height, 10, 10, 10, 10, Color.White);
			}
		}

		// Token: 0x06002E5E RID: 11870 RVA: 0x005A9F8C File Offset: 0x005A818C
		public void SetContents(string name)
		{
			this.actualContents = name;
			if (string.IsNullOrEmpty(this.actualContents))
			{
				this._text.TextColor = Color.Gray;
				this._text.SetText(this._textToShowWhenEmpty);
			}
			else
			{
				this._text.TextColor = Color.White;
				this._text.SetText(this.actualContents);
			}
			this._text.Left = StyleDimension.FromPixels(this._title.GetInnerDimensions().Width + this.DistanceFromTitleToOption);
		}

		// Token: 0x06002E5F RID: 11871 RVA: 0x005AA018 File Offset: 0x005A8218
		public CalculatedStyle GetTextDimensions()
		{
			return this._text.GetDimensions();
		}

		// Token: 0x06002E60 RID: 11872 RVA: 0x005AA028 File Offset: 0x005A8228
		public void TrimDisplayIfOverElementDimensions(int padding)
		{
			CalculatedStyle dimensions = base.GetDimensions();
			Point point = new Point((int)dimensions.X, (int)dimensions.Y);
			Point point2 = new Point(point.X + (int)dimensions.Width, point.Y + (int)dimensions.Height);
			Rectangle rectangle = new Rectangle(point.X, point.Y, point2.X - point.X, point2.Y - point.Y);
			CalculatedStyle dimensions2 = this._text.GetDimensions();
			Point point3 = new Point((int)dimensions2.X, (int)dimensions2.Y);
			Point point4 = new Point(point3.X + (int)dimensions2.Width, point3.Y + (int)dimensions2.Height);
			Rectangle rectangle2 = new Rectangle(point3.X, point3.Y, point4.X - point3.X, point4.Y - point3.Y);
			bool flag = false;
			while (rectangle2.Right > rectangle.Right - padding)
			{
				this._text.SetText(Utils.TrimLastCharacter(this._text.Text));
				flag = true;
				this.RecalculateChildren();
				dimensions2 = this._text.GetDimensions();
				point3 = new Point((int)dimensions2.X, (int)dimensions2.Y);
				point4 = new Point(point3.X + (int)dimensions2.Width, point3.Y + (int)dimensions2.Height);
				rectangle2 = new Rectangle(point3.X, point3.Y, point4.X - point3.X, point4.Y - point3.Y);
			}
			if (flag)
			{
				this._text.SetText(Utils.TrimLastCharacter(this._text.Text) + "…");
			}
		}

		// Token: 0x06002E61 RID: 11873 RVA: 0x005AA207 File Offset: 0x005A8407
		public override void LeftMouseDown(UIMouseEvent evt)
		{
			base.LeftMouseDown(evt);
		}

		// Token: 0x06002E62 RID: 11874 RVA: 0x005AA210 File Offset: 0x005A8410
		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			this._hovered = true;
		}

		// Token: 0x06002E63 RID: 11875 RVA: 0x005AA220 File Offset: 0x005A8420
		public override void MouseOut(UIMouseEvent evt)
		{
			base.MouseOut(evt);
			this._hovered = false;
		}

		// Token: 0x0400555C RID: 21852
		private readonly Asset<Texture2D> _BasePanelTexture;

		// Token: 0x0400555D RID: 21853
		private readonly Asset<Texture2D> _selectedBorderTexture;

		// Token: 0x0400555E RID: 21854
		private readonly Asset<Texture2D> _hoveredBorderTexture;

		// Token: 0x0400555F RID: 21855
		private bool _hovered;

		// Token: 0x04005560 RID: 21856
		private bool _soundedHover;

		// Token: 0x04005561 RID: 21857
		private readonly LocalizedText _textToShowWhenEmpty;

		// Token: 0x04005562 RID: 21858
		private string actualContents;

		// Token: 0x04005563 RID: 21859
		private UIText _text;

		// Token: 0x04005564 RID: 21860
		private UIText _title;

		// Token: 0x04005565 RID: 21861
		public readonly LocalizedText Description;

		// Token: 0x04005566 RID: 21862
		public float DistanceFromTitleToOption = 20f;
	}
}
