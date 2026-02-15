using System;
using ReLogic.Content;
using Terraria.Localization;

namespace Terraria.GameContent
{
	// Token: 0x02000271 RID: 625
	public class ContentRejectionFromSize : IRejectionReason
	{
		// Token: 0x0600240D RID: 9229 RVA: 0x00549B21 File Offset: 0x00547D21
		public ContentRejectionFromSize(int neededWidth, int neededHeight, int actualWidth, int actualHeight)
		{
			this._neededWidth = neededWidth;
			this._neededHeight = neededHeight;
			this._actualWidth = actualWidth;
			this._actualHeight = actualHeight;
		}

		// Token: 0x0600240E RID: 9230 RVA: 0x00549B46 File Offset: 0x00547D46
		public string GetReason()
		{
			return Language.GetTextValueWith("AssetRejections.BadSize", new
			{
				NeededWidth = this._neededWidth,
				NeededHeight = this._neededHeight,
				ActualWidth = this._actualWidth,
				ActualHeight = this._actualHeight
			});
		}

		// Token: 0x04004DAF RID: 19887
		private int _neededWidth;

		// Token: 0x04004DB0 RID: 19888
		private int _neededHeight;

		// Token: 0x04004DB1 RID: 19889
		private int _actualWidth;

		// Token: 0x04004DB2 RID: 19890
		private int _actualHeight;
	}
}
