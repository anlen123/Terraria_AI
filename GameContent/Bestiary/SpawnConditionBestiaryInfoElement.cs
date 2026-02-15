using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x02000353 RID: 851
	public class SpawnConditionBestiaryInfoElement : FilterProviderInfoElement, IBestiaryBackgroundImagePathAndColorProvider, IBestiaryPrioritizedElement
	{
		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06002883 RID: 10371 RVA: 0x00572B6B File Offset: 0x00570D6B
		// (set) Token: 0x06002884 RID: 10372 RVA: 0x00572B73 File Offset: 0x00570D73
		public float OrderPriority { get; set; }

		// Token: 0x06002885 RID: 10373 RVA: 0x00572B7C File Offset: 0x00570D7C
		public SpawnConditionBestiaryInfoElement(string nameLanguageKey, int filterIconFrame, string backgroundImagePath = null, Color? backgroundColor = null) : base(nameLanguageKey, filterIconFrame)
		{
			this._backgroundImagePath = backgroundImagePath;
			this._backgroundColor = backgroundColor;
		}

		// Token: 0x06002886 RID: 10374 RVA: 0x00572B95 File Offset: 0x00570D95
		public Asset<Texture2D> GetBackgroundImage()
		{
			if (this._backgroundImagePath == null)
			{
				return null;
			}
			return Main.Assets.Request<Texture2D>(this._backgroundImagePath, 1);
		}

		// Token: 0x06002887 RID: 10375 RVA: 0x00572BB2 File Offset: 0x00570DB2
		public Color? GetBackgroundColor()
		{
			return this._backgroundColor;
		}

		// Token: 0x0400512C RID: 20780
		private string _backgroundImagePath;

		// Token: 0x0400512D RID: 20781
		private Color? _backgroundColor;
	}
}
