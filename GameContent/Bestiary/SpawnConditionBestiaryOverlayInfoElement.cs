using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x02000354 RID: 852
	public class SpawnConditionBestiaryOverlayInfoElement : FilterProviderInfoElement, IBestiaryBackgroundOverlayAndColorProvider, IBestiaryPrioritizedElement
	{
		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06002888 RID: 10376 RVA: 0x00572BBA File Offset: 0x00570DBA
		// (set) Token: 0x06002889 RID: 10377 RVA: 0x00572BC2 File Offset: 0x00570DC2
		public float DisplayPriority { get; set; }

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x0600288A RID: 10378 RVA: 0x00572BCB File Offset: 0x00570DCB
		// (set) Token: 0x0600288B RID: 10379 RVA: 0x00572BD3 File Offset: 0x00570DD3
		public float OrderPriority { get; set; }

		// Token: 0x0600288C RID: 10380 RVA: 0x00572BDC File Offset: 0x00570DDC
		public SpawnConditionBestiaryOverlayInfoElement(string nameLanguageKey, int filterIconFrame, string overlayImagePath = null, Color? overlayColor = null) : base(nameLanguageKey, filterIconFrame)
		{
			this._overlayImagePath = overlayImagePath;
			this._overlayColor = overlayColor;
		}

		// Token: 0x0600288D RID: 10381 RVA: 0x00572BF5 File Offset: 0x00570DF5
		public Asset<Texture2D> GetBackgroundOverlayImage()
		{
			if (this._overlayImagePath == null)
			{
				return null;
			}
			return Main.Assets.Request<Texture2D>(this._overlayImagePath, 1);
		}

		// Token: 0x0600288E RID: 10382 RVA: 0x00572C12 File Offset: 0x00570E12
		public Color? GetBackgroundOverlayColor()
		{
			return this._overlayColor;
		}

		// Token: 0x0400512F RID: 20783
		private string _overlayImagePath;

		// Token: 0x04005130 RID: 20784
		private Color? _overlayColor;
	}
}
