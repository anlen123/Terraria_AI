using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Localization;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x0200033B RID: 827
	public class CustomEntryIcon : IEntryIcon
	{
		// Token: 0x06002838 RID: 10296 RVA: 0x00572246 File Offset: 0x00570446
		public CustomEntryIcon(string nameLanguageKey, string texturePath, Func<bool> unlockCondition)
		{
			this._text = Language.GetText(nameLanguageKey);
			this._textureAsset = Main.Assets.Request<Texture2D>(texturePath, 1);
			this._unlockCondition = unlockCondition;
			this.UpdateUnlockState(false);
		}

		// Token: 0x06002839 RID: 10297 RVA: 0x0057227A File Offset: 0x0057047A
		public IEntryIcon CreateClone()
		{
			return new CustomEntryIcon(this._text.Key, this._textureAsset.Name, this._unlockCondition);
		}

		// Token: 0x0600283A RID: 10298 RVA: 0x0057229D File Offset: 0x0057049D
		public void Update(BestiaryUICollectionInfo providedInfo, Rectangle hitbox, EntryIconDrawSettings settings)
		{
			this.UpdateUnlockState(this.GetUnlockState(providedInfo));
		}

		// Token: 0x0600283B RID: 10299 RVA: 0x005722AC File Offset: 0x005704AC
		public void Draw(BestiaryUICollectionInfo providedInfo, SpriteBatch spriteBatch, EntryIconDrawSettings settings)
		{
			Rectangle iconbox = settings.iconbox;
			spriteBatch.Draw(this._textureAsset.Value, iconbox.Center.ToVector2() + Vector2.One, new Rectangle?(this._sourceRectangle), Color.White, 0f, this._sourceRectangle.Size() / 2f, 1f, SpriteEffects.None, 0f);
		}

		// Token: 0x0600283C RID: 10300 RVA: 0x0057231C File Offset: 0x0057051C
		public string GetHoverText(BestiaryUICollectionInfo providedInfo)
		{
			if (this.GetUnlockState(providedInfo))
			{
				return this._text.Value;
			}
			return "???";
		}

		// Token: 0x0600283D RID: 10301 RVA: 0x00572338 File Offset: 0x00570538
		private void UpdateUnlockState(bool state)
		{
			this._sourceRectangle = this._textureAsset.Frame(2, 1, state.ToInt(), 0, 0, 0);
			this._sourceRectangle.Inflate(-2, -2);
		}

		// Token: 0x0600283E RID: 10302 RVA: 0x0057223B File Offset: 0x0057043B
		public bool GetUnlockState(BestiaryUICollectionInfo providedInfo)
		{
			return providedInfo.UnlockState > BestiaryEntryUnlockState.NotKnownAtAll_0;
		}

		// Token: 0x0400510B RID: 20747
		private LocalizedText _text;

		// Token: 0x0400510C RID: 20748
		private Asset<Texture2D> _textureAsset;

		// Token: 0x0400510D RID: 20749
		private Rectangle _sourceRectangle;

		// Token: 0x0400510E RID: 20750
		private Func<bool> _unlockCondition;
	}
}
