using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003E8 RID: 1000
	public class GroupOptionButton<T> : UIElement, IGroupOptionButton
	{
		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06002E46 RID: 11846 RVA: 0x005A9800 File Offset: 0x005A7A00
		public T OptionValue
		{
			get
			{
				return this._myOption;
			}
		}

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06002E47 RID: 11847 RVA: 0x005A9808 File Offset: 0x005A7A08
		public bool IsSelected
		{
			get
			{
				return EqualityComparer<T>.Default.Equals(this._currentOption, this._myOption);
			}
		}

		// Token: 0x06002E48 RID: 11848 RVA: 0x005A9820 File Offset: 0x005A7A20
		public GroupOptionButton(T option, LocalizedText title, LocalizedText description, Color textColor, string iconTexturePath, float textSize = 1f, float titleAlignmentX = 0.5f, float titleWidthReduction = 10f)
		{
			this._borderColor = Color.White;
			this._currentOption = option;
			this._myOption = option;
			this.Description = description;
			this.Width = StyleDimension.FromPixels(44f);
			this.Height = StyleDimension.FromPixels(34f);
			this._BasePanelTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/PanelGrayscale", 1);
			this._selectedBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight", 1);
			this._hoveredBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelBorder", 1);
			if (iconTexturePath != null)
			{
				this._iconTexture = Main.Assets.Request<Texture2D>(iconTexturePath, 1);
			}
			this._color = Colors.InventoryDefaultColor;
			if (title != null)
			{
				UIText uitext = new UIText(title, textSize, false)
				{
					HAlign = titleAlignmentX,
					VAlign = 0.5f,
					Width = StyleDimension.FromPixelsAndPercent(-titleWidthReduction, 1f),
					Top = StyleDimension.FromPixels(0f)
				};
				uitext.TextColor = textColor;
				base.Append(uitext);
				this._title = uitext;
			}
		}

		// Token: 0x06002E49 RID: 11849 RVA: 0x005A998C File Offset: 0x005A7B8C
		public void SetText(LocalizedText text, float textSize, Color color)
		{
			if (this._title != null)
			{
				this._title.Remove();
			}
			UIText uitext = new UIText(text, textSize, false)
			{
				HAlign = 0.5f,
				VAlign = 0.5f,
				Width = StyleDimension.FromPixelsAndPercent(-10f, 1f),
				Top = StyleDimension.FromPixels(0f)
			};
			uitext.TextColor = color;
			base.Append(uitext);
			this._title = uitext;
		}

		// Token: 0x06002E4A RID: 11850 RVA: 0x005A9A08 File Offset: 0x005A7C08
		public void SetTextWithoutLocalization(string text, float textSize, Color color, float hAlign, float left)
		{
			if (this._title != null)
			{
				this._title.Remove();
			}
			UIText uitext = new UIText(text, textSize, false)
			{
				HAlign = 0.5f,
				VAlign = 0.5f,
				Width = StyleDimension.FromPixelsAndPercent(-10f, 1f),
				Top = StyleDimension.FromPixels(0f),
				IgnoresMouseInteraction = true
			};
			uitext.TextOriginX = hAlign;
			uitext.Left.Pixels = left;
			uitext.TextColor = color;
			base.Append(uitext);
			this._title = uitext;
		}

		// Token: 0x06002E4B RID: 11851 RVA: 0x005A9A9D File Offset: 0x005A7C9D
		public void SetCurrentOption(T option)
		{
			this._currentOption = option;
		}

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06002E4C RID: 11852 RVA: 0x005A9AA6 File Offset: 0x005A7CA6
		public Texture2D Icon
		{
			get
			{
				if (this._iconTexture == null)
				{
					return null;
				}
				return this._iconTexture.Value;
			}
		}

		// Token: 0x06002E4D RID: 11853 RVA: 0x005A9ABD File Offset: 0x005A7CBD
		public void SetIcon(string iconTexturePath)
		{
			if (iconTexturePath != null)
			{
				this._iconTexture = Main.Assets.Request<Texture2D>(iconTexturePath, 1);
				return;
			}
			this._iconTexture = null;
		}

		// Token: 0x06002E4E RID: 11854 RVA: 0x005A9ADC File Offset: 0x005A7CDC
		public void SetIconFrame(Rectangle region)
		{
			this._iconFrame = new Rectangle?(region);
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06002E4F RID: 11855 RVA: 0x005A9AEA File Offset: 0x005A7CEA
		// (set) Token: 0x06002E50 RID: 11856 RVA: 0x005A9AF2 File Offset: 0x005A7CF2
		public float IconScale
		{
			get
			{
				return this._iconScale;
			}
			set
			{
				this._iconScale = value;
			}
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06002E51 RID: 11857 RVA: 0x005A9AFB File Offset: 0x005A7CFB
		// (set) Token: 0x06002E52 RID: 11858 RVA: 0x005A9B03 File Offset: 0x005A7D03
		public Vector2 IconOffset
		{
			get
			{
				return this._iconOffset;
			}
			set
			{
				this._iconOffset = value;
			}
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06002E53 RID: 11859 RVA: 0x005A9B0C File Offset: 0x005A7D0C
		// (set) Token: 0x06002E54 RID: 11860 RVA: 0x005A9B14 File Offset: 0x005A7D14
		public Color IconColor
		{
			get
			{
				return this._iconColor;
			}
			set
			{
				this._iconColor = value;
			}
		}

		// Token: 0x06002E55 RID: 11861 RVA: 0x005A9B20 File Offset: 0x005A7D20
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
			float scale = this._opacity;
			bool isSelected = this.IsSelected;
			if (this._UseOverrideColors)
			{
				color = (isSelected ? this._overridePickedColor : this._overrideUnpickedColor);
				scale = (isSelected ? this._overrideOpacityPicked : this._overrideOpacityUnpicked);
			}
			Utils.DrawSplicedPanel(spriteBatch, this._BasePanelTexture.Value, (int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height, 10, 10, 10, 10, Color.Lerp(Color.Black, color, this.FadeFromBlack) * scale);
			if (isSelected && this.ShowHighlightWhenSelected)
			{
				Utils.DrawSplicedPanel(spriteBatch, this._selectedBorderTexture.Value, (int)dimensions.X + this.InnerHighlightRim, (int)dimensions.Y + this.InnerHighlightRim, (int)dimensions.Width - this.InnerHighlightRim * 2, (int)dimensions.Height - this.InnerHighlightRim * 2, 10, 10, 10, 10, Color.Lerp(color, Color.White, this._whiteLerp) * scale);
			}
			if (this._hovered)
			{
				Utils.DrawSplicedPanel(spriteBatch, this._hoveredBorderTexture.Value, (int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height, 10, 10, 10, 10, this._borderColor);
			}
			if (this._iconTexture != null)
			{
				Color color2 = this.IconColor;
				if (!this._hovered && !isSelected)
				{
					color2 = Color.Lerp(color, color2, this._whiteLerp) * scale;
				}
				spriteBatch.Draw(this._iconTexture.Value, new Vector2(dimensions.X + 1f, dimensions.Y + 1f) + this._iconOffset, this._iconFrame, color2, 0f, Vector2.Zero, this._iconScale, SpriteEffects.None, 0f);
			}
		}

		// Token: 0x06002E56 RID: 11862 RVA: 0x005A2EB1 File Offset: 0x005A10B1
		public override void LeftMouseDown(UIMouseEvent evt)
		{
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			base.LeftMouseDown(evt);
		}

		// Token: 0x06002E57 RID: 11863 RVA: 0x005A9D3B File Offset: 0x005A7F3B
		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			this._hovered = true;
		}

		// Token: 0x06002E58 RID: 11864 RVA: 0x005A9D4B File Offset: 0x005A7F4B
		public override void MouseOut(UIMouseEvent evt)
		{
			base.MouseOut(evt);
			this._hovered = false;
		}

		// Token: 0x06002E59 RID: 11865 RVA: 0x005A9D5B File Offset: 0x005A7F5B
		public void SetColor(Color color, float opacity)
		{
			this._color = color;
			this._opacity = opacity;
		}

		// Token: 0x06002E5A RID: 11866 RVA: 0x005A9D6B File Offset: 0x005A7F6B
		public void SetColorsBasedOnSelectionState(Color pickedColor, Color unpickedColor, float opacityPicked, float opacityNotPicked)
		{
			this._UseOverrideColors = true;
			this._overridePickedColor = pickedColor;
			this._overrideUnpickedColor = unpickedColor;
			this._overrideOpacityPicked = opacityPicked;
			this._overrideOpacityUnpicked = opacityNotPicked;
		}

		// Token: 0x06002E5B RID: 11867 RVA: 0x005A9D91 File Offset: 0x005A7F91
		public void SetBorderColor(Color color)
		{
			this._borderColor = color;
		}

		// Token: 0x04005542 RID: 21826
		private T _currentOption;

		// Token: 0x04005543 RID: 21827
		private readonly Asset<Texture2D> _BasePanelTexture;

		// Token: 0x04005544 RID: 21828
		private readonly Asset<Texture2D> _selectedBorderTexture;

		// Token: 0x04005545 RID: 21829
		private readonly Asset<Texture2D> _hoveredBorderTexture;

		// Token: 0x04005546 RID: 21830
		private Asset<Texture2D> _iconTexture;

		// Token: 0x04005547 RID: 21831
		private readonly T _myOption;

		// Token: 0x04005548 RID: 21832
		private Color _color;

		// Token: 0x04005549 RID: 21833
		private Color _borderColor;

		// Token: 0x0400554A RID: 21834
		public float FadeFromBlack = 1f;

		// Token: 0x0400554B RID: 21835
		public int InnerHighlightRim = 7;

		// Token: 0x0400554C RID: 21836
		private float _whiteLerp = 0.7f;

		// Token: 0x0400554D RID: 21837
		private float _opacity = 0.7f;

		// Token: 0x0400554E RID: 21838
		private bool _hovered;

		// Token: 0x0400554F RID: 21839
		private bool _soundedHover;

		// Token: 0x04005550 RID: 21840
		public bool ShowHighlightWhenSelected = true;

		// Token: 0x04005551 RID: 21841
		private bool _UseOverrideColors;

		// Token: 0x04005552 RID: 21842
		private Color _overrideUnpickedColor = Color.White;

		// Token: 0x04005553 RID: 21843
		private Color _overridePickedColor = Color.White;

		// Token: 0x04005554 RID: 21844
		private float _overrideOpacityPicked;

		// Token: 0x04005555 RID: 21845
		private float _overrideOpacityUnpicked;

		// Token: 0x04005556 RID: 21846
		public readonly LocalizedText Description;

		// Token: 0x04005557 RID: 21847
		private UIText _title;

		// Token: 0x04005558 RID: 21848
		private float _iconScale = 1f;

		// Token: 0x04005559 RID: 21849
		private Vector2 _iconOffset;

		// Token: 0x0400555A RID: 21850
		private Rectangle? _iconFrame;

		// Token: 0x0400555B RID: 21851
		private Color _iconColor = Color.White;
	}
}
