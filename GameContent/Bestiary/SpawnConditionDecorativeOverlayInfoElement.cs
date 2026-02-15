using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x02000355 RID: 853
	public class SpawnConditionDecorativeOverlayInfoElement : IBestiaryInfoElement, IBestiaryBackgroundOverlayAndColorProvider
	{
		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x0600288F RID: 10383 RVA: 0x00572C1A File Offset: 0x00570E1A
		// (set) Token: 0x06002890 RID: 10384 RVA: 0x00572C22 File Offset: 0x00570E22
		public float DisplayPriority { get; set; }

		// Token: 0x06002891 RID: 10385 RVA: 0x00572C2B File Offset: 0x00570E2B
		public SpawnConditionDecorativeOverlayInfoElement(string overlayImagePath = null, Color? overlayColor = null)
		{
			this._overlayImagePath = overlayImagePath;
			this._overlayColor = overlayColor;
		}

		// Token: 0x06002892 RID: 10386 RVA: 0x00572C41 File Offset: 0x00570E41
		public Asset<Texture2D> GetBackgroundOverlayImage()
		{
			if (this._overlayImagePath == null)
			{
				return null;
			}
			return Main.Assets.Request<Texture2D>(this._overlayImagePath, 1);
		}

		// Token: 0x06002893 RID: 10387 RVA: 0x00572C5E File Offset: 0x00570E5E
		public Color? GetBackgroundOverlayColor()
		{
			return this._overlayColor;
		}

		// Token: 0x06002894 RID: 10388 RVA: 0x000762F3 File Offset: 0x000744F3
		public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			return null;
		}

		// Token: 0x04005133 RID: 20787
		private string _overlayImagePath;

		// Token: 0x04005134 RID: 20788
		private Color? _overlayColor;
	}
}
