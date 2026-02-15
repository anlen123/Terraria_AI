using System;

namespace Terraria.GameContent.UI.BigProgressBar
{
	// Token: 0x02000393 RID: 915
	public struct BigProgressBarCache
	{
		// Token: 0x060029E1 RID: 10721 RVA: 0x0057F3F3 File Offset: 0x0057D5F3
		public void SetLife(float current, float max)
		{
			this.LifeCurrent = current;
			this.LifeMax = max;
		}

		// Token: 0x060029E2 RID: 10722 RVA: 0x0057F403 File Offset: 0x0057D603
		public void SetShield(float current, float max)
		{
			this.ShieldCurrent = current;
			this.ShieldMax = max;
		}

		// Token: 0x040052B2 RID: 21170
		public float LifeCurrent;

		// Token: 0x040052B3 RID: 21171
		public float LifeMax;

		// Token: 0x040052B4 RID: 21172
		public float ShieldCurrent;

		// Token: 0x040052B5 RID: 21173
		public float ShieldMax;
	}
}
