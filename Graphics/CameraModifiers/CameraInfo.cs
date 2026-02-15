using System;
using Microsoft.Xna.Framework;

namespace Terraria.Graphics.CameraModifiers
{
	// Token: 0x0200021C RID: 540
	public struct CameraInfo
	{
		// Token: 0x060021BB RID: 8635 RVA: 0x00531AF3 File Offset: 0x0052FCF3
		public CameraInfo(Vector2 position)
		{
			this.OriginalCameraPosition = position;
			this.OriginalCameraCenter = position + Main.ScreenSize.ToVector2() / 2f;
			this.CameraPosition = this.OriginalCameraPosition;
		}

		// Token: 0x04004C1A RID: 19482
		public Vector2 CameraPosition;

		// Token: 0x04004C1B RID: 19483
		public Vector2 OriginalCameraCenter;

		// Token: 0x04004C1C RID: 19484
		public Vector2 OriginalCameraPosition;
	}
}
