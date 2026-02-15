using System;

namespace Terraria.DataStructures
{
	// Token: 0x020005A4 RID: 1444
	public struct WingStats
	{
		// Token: 0x060038F5 RID: 14581 RVA: 0x0064FBCB File Offset: 0x0064DDCB
		public WingStats(int flyTime = 100, float flySpeedOverride = -1f, float accelerationMultiplier = 1f, bool hasHoldDownHoverFeatures = false, float hoverFlySpeedOverride = -1f, float hoverAccelerationMultiplier = 1f)
		{
			this.FlyTime = flyTime;
			this.AccRunSpeedOverride = flySpeedOverride;
			this.AccRunAccelerationMult = accelerationMultiplier;
			this.HasDownHoverStats = hasHoldDownHoverFeatures;
			this.DownHoverSpeedOverride = hoverFlySpeedOverride;
			this.DownHoverAccelerationMult = hoverAccelerationMultiplier;
		}

		// Token: 0x060038F6 RID: 14582 RVA: 0x0064FBFA File Offset: 0x0064DDFA
		public WingStats WithSpeedBoost(float multiplier)
		{
			return new WingStats(this.FlyTime, this.AccRunSpeedOverride * multiplier, this.AccRunAccelerationMult, this.HasDownHoverStats, this.DownHoverSpeedOverride * multiplier, this.DownHoverAccelerationMult);
		}

		// Token: 0x04005D3D RID: 23869
		public static readonly WingStats Default;

		// Token: 0x04005D3E RID: 23870
		public int FlyTime;

		// Token: 0x04005D3F RID: 23871
		public float AccRunSpeedOverride;

		// Token: 0x04005D40 RID: 23872
		public float AccRunAccelerationMult;

		// Token: 0x04005D41 RID: 23873
		public bool HasDownHoverStats;

		// Token: 0x04005D42 RID: 23874
		public float DownHoverSpeedOverride;

		// Token: 0x04005D43 RID: 23875
		public float DownHoverAccelerationMult;
	}
}
