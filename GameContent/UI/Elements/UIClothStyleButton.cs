using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003EA RID: 1002
	public class UIClothStyleButton : UIElement
	{
		// Token: 0x06002E64 RID: 11876 RVA: 0x005AA230 File Offset: 0x005A8430
		public UIClothStyleButton(Player player, int clothStyleId, Action prepareAction = null)
		{
			this._player = player;
			this.ClothStyleId = clothStyleId;
			this.PrepareAction = prepareAction;
			this.Width = StyleDimension.FromPixels(44f);
			this.Height = StyleDimension.FromPixels(80f);
			this._BasePanelTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanel", 1);
			this._selectedBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight", 1);
			this._hoveredBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelBorder", 1);
			this._char = new UICharacter(this._player, false, false, 1f, false)
			{
				HAlign = 0.5f,
				VAlign = 0.5f
			};
			base.Append(this._char);
		}

		// Token: 0x06002E65 RID: 11877 RVA: 0x005AA2F8 File Offset: 0x005A84F8
		public override void Draw(SpriteBatch spriteBatch)
		{
			this._realSkinVariant = this._player.skinVariant;
			this._player.skinVariant = this.ClothStyleId;
			int hair = this._player.hair;
			if (this.PrepareAction != null)
			{
				this.PrepareAction();
			}
			this._player.PlayerFrame();
			base.Draw(spriteBatch);
			this._player.skinVariant = this._realSkinVariant;
			this._player.hair = hair;
		}

		// Token: 0x06002E66 RID: 11878 RVA: 0x005AA378 File Offset: 0x005A8578
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
			if (this._realSkinVariant == this.ClothStyleId)
			{
				Utils.DrawSplicedPanel(spriteBatch, this._selectedBorderTexture.Value, (int)dimensions.X + 3, (int)dimensions.Y + 3, (int)dimensions.Width - 6, (int)dimensions.Height - 6, 10, 10, 10, 10, Color.White);
			}
			if (this._hovered)
			{
				Utils.DrawSplicedPanel(spriteBatch, this._hoveredBorderTexture.Value, (int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height, 10, 10, 10, 10, Color.White);
			}
		}

		// Token: 0x06002E67 RID: 11879 RVA: 0x005AA207 File Offset: 0x005A8407
		public override void LeftMouseDown(UIMouseEvent evt)
		{
			base.LeftMouseDown(evt);
		}

		// Token: 0x06002E68 RID: 11880 RVA: 0x005AA497 File Offset: 0x005A8697
		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			this._hovered = true;
			this._char.SetAnimated(true);
		}

		// Token: 0x06002E69 RID: 11881 RVA: 0x005AA4B3 File Offset: 0x005A86B3
		public override void MouseOut(UIMouseEvent evt)
		{
			base.MouseOut(evt);
			this._hovered = false;
			this._char.SetAnimated(false);
		}

		// Token: 0x04005567 RID: 21863
		private readonly Player _player;

		// Token: 0x04005568 RID: 21864
		public readonly int ClothStyleId;

		// Token: 0x04005569 RID: 21865
		private readonly Asset<Texture2D> _BasePanelTexture;

		// Token: 0x0400556A RID: 21866
		private readonly Asset<Texture2D> _selectedBorderTexture;

		// Token: 0x0400556B RID: 21867
		private readonly Asset<Texture2D> _hoveredBorderTexture;

		// Token: 0x0400556C RID: 21868
		private readonly UICharacter _char;

		// Token: 0x0400556D RID: 21869
		private bool _hovered;

		// Token: 0x0400556E RID: 21870
		private bool _soundedHover;

		// Token: 0x0400556F RID: 21871
		private int _realSkinVariant;

		// Token: 0x04005570 RID: 21872
		private Action PrepareAction;
	}
}
