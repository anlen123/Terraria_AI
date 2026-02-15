using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003EE RID: 1006
	public class UIColoredImageButton : UIElement
	{
		// Token: 0x06002E7C RID: 11900 RVA: 0x005AAAD8 File Offset: 0x005A8CD8
		public UIColoredImageButton(Asset<Texture2D> texture, bool isSmall = false)
		{
			this._color = Color.White;
			this._texture = texture;
			if (isSmall)
			{
				this._backPanelTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/SmallPanel", 1);
			}
			else
			{
				this._backPanelTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanel", 1);
			}
			this.Width.Set((float)this._backPanelTexture.Width(), 0f);
			this.Height.Set((float)this._backPanelTexture.Height(), 0f);
			this._backPanelHighlightTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight", 1);
			if (isSmall)
			{
				this._backPanelBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/SmallPanelBorder", 1);
				return;
			}
			this._backPanelBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelBorder", 1);
		}

		// Token: 0x06002E7D RID: 11901 RVA: 0x005AABC4 File Offset: 0x005A8DC4
		public void SetImage(Asset<Texture2D> texture)
		{
			this._texture = texture;
			this.Width.Set((float)this._texture.Width(), 0f);
			this.Height.Set((float)this._texture.Height(), 0f);
		}

		// Token: 0x06002E7E RID: 11902 RVA: 0x005AAC10 File Offset: 0x005A8E10
		public void SetImageWithoutSettingSize(Asset<Texture2D> texture)
		{
			this._texture = texture;
		}

		// Token: 0x06002E7F RID: 11903 RVA: 0x005AAC1C File Offset: 0x005A8E1C
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = base.GetDimensions();
			Vector2 position = dimensions.Position() + new Vector2(dimensions.Width, dimensions.Height) / 2f;
			spriteBatch.Draw(this._backPanelTexture.Value, position, null, Color.White * (base.IsMouseHovering ? this._visibilityActive : this._visibilityInactive), 0f, this._backPanelTexture.Size() / 2f, 1f, SpriteEffects.None, 0f);
			Color white = Color.White;
			if (this._hovered)
			{
				spriteBatch.Draw(this._backPanelBorderTexture.Value, position, null, Color.White, 0f, this._backPanelBorderTexture.Size() / 2f, 1f, SpriteEffects.None, 0f);
			}
			if (this._selected)
			{
				spriteBatch.Draw(this._backPanelHighlightTexture.Value, position, null, Color.White, 0f, this._backPanelHighlightTexture.Size() / 2f, 1f, SpriteEffects.None, 0f);
			}
			if (this._middleTexture != null)
			{
				spriteBatch.Draw(this._middleTexture.Value, position, null, Color.White, 0f, this._middleTexture.Size() / 2f, 1f, SpriteEffects.None, 0f);
			}
			if (this._texture != null)
			{
				spriteBatch.Draw(this._texture.Value, position, null, this._color, 0f, this._texture.Size() / 2f, 1f, SpriteEffects.None, 0f);
			}
		}

		// Token: 0x06002E80 RID: 11904 RVA: 0x005AADF3 File Offset: 0x005A8FF3
		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
			this._hovered = true;
		}

		// Token: 0x06002E81 RID: 11905 RVA: 0x005AAE18 File Offset: 0x005A9018
		public void SetVisibility(float whenActive, float whenInactive)
		{
			this._visibilityActive = MathHelper.Clamp(whenActive, 0f, 1f);
			this._visibilityInactive = MathHelper.Clamp(whenInactive, 0f, 1f);
		}

		// Token: 0x06002E82 RID: 11906 RVA: 0x005AAE46 File Offset: 0x005A9046
		public void SetColor(Color color)
		{
			this._color = color;
		}

		// Token: 0x06002E83 RID: 11907 RVA: 0x005AAE4F File Offset: 0x005A904F
		public void SetMiddleTexture(Asset<Texture2D> texAsset)
		{
			this._middleTexture = texAsset;
		}

		// Token: 0x06002E84 RID: 11908 RVA: 0x005AAE58 File Offset: 0x005A9058
		public void SetSelected(bool selected)
		{
			this._selected = selected;
		}

		// Token: 0x06002E85 RID: 11909 RVA: 0x005AAE61 File Offset: 0x005A9061
		public override void MouseOut(UIMouseEvent evt)
		{
			base.MouseOut(evt);
			this._hovered = false;
		}

		// Token: 0x04005586 RID: 21894
		private Asset<Texture2D> _backPanelTexture;

		// Token: 0x04005587 RID: 21895
		private Asset<Texture2D> _texture;

		// Token: 0x04005588 RID: 21896
		private Asset<Texture2D> _middleTexture;

		// Token: 0x04005589 RID: 21897
		private Asset<Texture2D> _backPanelHighlightTexture;

		// Token: 0x0400558A RID: 21898
		private Asset<Texture2D> _backPanelBorderTexture;

		// Token: 0x0400558B RID: 21899
		private Color _color;

		// Token: 0x0400558C RID: 21900
		private float _visibilityActive = 1f;

		// Token: 0x0400558D RID: 21901
		private float _visibilityInactive = 0.4f;

		// Token: 0x0400558E RID: 21902
		private bool _selected;

		// Token: 0x0400558F RID: 21903
		private bool _hovered;
	}
}
