using System;

namespace Terraria.Graphics.CameraModifiers
{
	// Token: 0x0200021B RID: 539
	public interface ICameraModifier
	{
		// Token: 0x17000349 RID: 841
		// (get) Token: 0x060021B7 RID: 8631
		string UniqueIdentity { get; }

		// Token: 0x060021B8 RID: 8632
		void Update(ref CameraInfo cameraPosition);

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x060021B9 RID: 8633
		bool IsAScreenShake { get; }

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x060021BA RID: 8634
		bool Finished { get; }
	}
}
