using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x02000338 RID: 824
	public interface IEntryIcon
	{
		// Token: 0x0600282A RID: 10282
		void Update(BestiaryUICollectionInfo providedInfo, Rectangle hitbox, EntryIconDrawSettings settings);

		// Token: 0x0600282B RID: 10283
		void Draw(BestiaryUICollectionInfo providedInfo, SpriteBatch spriteBatch, EntryIconDrawSettings settings);

		// Token: 0x0600282C RID: 10284
		bool GetUnlockState(BestiaryUICollectionInfo providedInfo);

		// Token: 0x0600282D RID: 10285
		string GetHoverText(BestiaryUICollectionInfo providedInfo);

		// Token: 0x0600282E RID: 10286
		IEntryIcon CreateClone();
	}
}
