using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003EB RID: 1003
	public class UIDifficultyButton : UIElement
	{
		// Token: 0x06002E6A RID: 11882 RVA: 0x005AA4D0 File Offset: 0x005A86D0
		public UIDifficultyButton(Player player, LocalizedText title, LocalizedText description, byte difficulty, Color color)
		{
			this._player = player;
			this._difficulty = difficulty;
			this.Width = StyleDimension.FromPixels(44f);
			this.Height = StyleDimension.FromPixels(110f);
			this._BasePanelTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/PanelGrayscale", 1);
			this._selectedBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight", 1);
			this._hoveredBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelBorder", 1);
			this._color = color;
			UIText element = new UIText(title, 0.9f, false)
			{
				HAlign = 0.5f,
				VAlign = 0f,
				Width = StyleDimension.FromPixelsAndPercent(-10f, 1f),
				Top = StyleDimension.FromPixels(5f)
			};
			base.Append(element);
		}

		// Token: 0x06002E6B RID: 11883 RVA: 0x005AA5AC File Offset: 0x005A87AC
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
			int num = 7;
			if (dimensions.Height < 30f)
			{
				num = 5;
			}
			int num2 = 10;
			int num3 = 10;
			bool flag = this._difficulty == this._player.difficulty;
			Utils.DrawSplicedPanel(spriteBatch, this._BasePanelTexture.Value, (int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height, num2, num2, num3, num3, Color.Lerp(Color.Black, this._color, 0.8f) * 0.5f);
			if (flag)
			{
				Utils.DrawSplicedPanel(spriteBatch, this._BasePanelTexture.Value, (int)dimensions.X + num, (int)dimensions.Y + num - 2, (int)dimensions.Width - num * 2, (int)dimensions.Height - num * 2, num2, num2, num3, num3, Color.Lerp(this._color, Color.White, 0.7f) * 0.5f);
			}
			if (this._hovered)
			{
				Utils.DrawSplicedPanel(spriteBatch, this._hoveredBorderTexture.Value, (int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height, num2, num2, num3, num3, Color.White);
			}
		}

		// Token: 0x06002E6C RID: 11884 RVA: 0x005AA70D File Offset: 0x005A890D
		public override void LeftMouseDown(UIMouseEvent evt)
		{
			this._player.difficulty = this._difficulty;
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			base.LeftMouseDown(evt);
		}

		// Token: 0x06002E6D RID: 11885 RVA: 0x005AA73C File Offset: 0x005A893C
		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			this._hovered = true;
		}

		// Token: 0x06002E6E RID: 11886 RVA: 0x005AA74C File Offset: 0x005A894C
		public override void MouseOut(UIMouseEvent evt)
		{
			base.MouseOut(evt);
			this._hovered = false;
		}

		// Token: 0x04005571 RID: 21873
		private readonly Player _player;

		// Token: 0x04005572 RID: 21874
		private readonly Asset<Texture2D> _BasePanelTexture;

		// Token: 0x04005573 RID: 21875
		private readonly Asset<Texture2D> _selectedBorderTexture;

		// Token: 0x04005574 RID: 21876
		private readonly Asset<Texture2D> _hoveredBorderTexture;

		// Token: 0x04005575 RID: 21877
		private readonly byte _difficulty;

		// Token: 0x04005576 RID: 21878
		private readonly Color _color;

		// Token: 0x04005577 RID: 21879
		private bool _hovered;

		// Token: 0x04005578 RID: 21880
		private bool _soundedHover;
	}
}
