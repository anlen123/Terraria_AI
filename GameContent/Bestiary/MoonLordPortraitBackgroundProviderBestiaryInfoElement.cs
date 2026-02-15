using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x02000352 RID: 850
	public class MoonLordPortraitBackgroundProviderBestiaryInfoElement : IBestiaryInfoElement, IBestiaryBackgroundImagePathAndColorProvider
	{
		// Token: 0x06002880 RID: 10368 RVA: 0x00572B4D File Offset: 0x00570D4D
		public Asset<Texture2D> GetBackgroundImage()
		{
			return Main.Assets.Request<Texture2D>("Images/MapBG1", 1);
		}

		// Token: 0x06002881 RID: 10369 RVA: 0x00572B5F File Offset: 0x00570D5F
		public Color? GetBackgroundColor()
		{
			return new Color?(Color.Black);
		}

		// Token: 0x06002882 RID: 10370 RVA: 0x000762F3 File Offset: 0x000744F3
		public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			return null;
		}
	}
}
