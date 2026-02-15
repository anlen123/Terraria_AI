using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.Bestiary;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	// Token: 0x020003D9 RID: 985
	public class UIBestiaryEntryIcon : UIElement
	{
		// Token: 0x06002DD6 RID: 11734 RVA: 0x005A6598 File Offset: 0x005A4798
		public UIBestiaryEntryIcon(BestiaryEntry entry, bool isPortrait)
		{
			this._entry = entry;
			this.IgnoresMouseInteraction = true;
			this.OverrideSamplerState = Main.DefaultSamplerState;
			this.UseImmediateMode = true;
			this.Width.Set(0f, 1f);
			this.Height.Set(0f, 1f);
			this._notUnlockedTexture = Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Icon_Locked", 1);
			this._isPortrait = isPortrait;
			this._collectionInfo = this._entry.UIInfoProvider.GetEntryUICollectionInfo();
		}

		// Token: 0x06002DD7 RID: 11735 RVA: 0x005A6628 File Offset: 0x005A4828
		public override void Update(GameTime gameTime)
		{
			this._collectionInfo = this._entry.UIInfoProvider.GetEntryUICollectionInfo();
			CalculatedStyle dimensions = base.GetDimensions();
			bool isHovered = base.IsMouseHovering || this.ForceHover;
			this._entry.Icon.Update(this._collectionInfo, dimensions.ToRectangle(), new EntryIconDrawSettings
			{
				iconbox = dimensions.ToRectangle(),
				IsPortrait = this._isPortrait,
				IsHovered = isHovered
			});
			base.Update(gameTime);
		}

		// Token: 0x06002DD8 RID: 11736 RVA: 0x005A66B4 File Offset: 0x005A48B4
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = base.GetDimensions();
			bool unlockState = this._entry.Icon.GetUnlockState(this._collectionInfo);
			bool isHovered = base.IsMouseHovering || this.ForceHover;
			if (unlockState)
			{
				this._entry.Icon.Draw(this._collectionInfo, spriteBatch, new EntryIconDrawSettings
				{
					iconbox = dimensions.ToRectangle(),
					IsPortrait = this._isPortrait,
					IsHovered = isHovered
				});
				return;
			}
			Texture2D value = this._notUnlockedTexture.Value;
			spriteBatch.Draw(value, dimensions.Center(), null, Color.White * 0.15f, 0f, value.Size() / 2f, 1f, SpriteEffects.None, 0f);
		}

		// Token: 0x06002DD9 RID: 11737 RVA: 0x005A6789 File Offset: 0x005A4989
		public string GetHoverText()
		{
			return this._entry.Icon.GetHoverText(this._collectionInfo);
		}

		// Token: 0x040054E9 RID: 21737
		private BestiaryEntry _entry;

		// Token: 0x040054EA RID: 21738
		private Asset<Texture2D> _notUnlockedTexture;

		// Token: 0x040054EB RID: 21739
		private bool _isPortrait;

		// Token: 0x040054EC RID: 21740
		public bool ForceHover;

		// Token: 0x040054ED RID: 21741
		private BestiaryUICollectionInfo _collectionInfo;
	}
}
