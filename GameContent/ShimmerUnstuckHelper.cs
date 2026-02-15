using System;
using Terraria.GameContent.Drawing;

namespace Terraria.GameContent
{
	// Token: 0x02000247 RID: 583
	public struct ShimmerUnstuckHelper
	{
		// Token: 0x17000373 RID: 883
		// (get) Token: 0x060022E2 RID: 8930 RVA: 0x0053AE2D File Offset: 0x0053902D
		public bool ShouldUnstuck
		{
			get
			{
				return this.IndefiniteProtectionActive || this.TimeLeftUnstuck > 0;
			}
		}

		// Token: 0x060022E3 RID: 8931 RVA: 0x0053AE44 File Offset: 0x00539044
		public void Update(Player player)
		{
			bool flag = !player.shimmering && !player.shimmerWet;
			if (flag)
			{
				this.IndefiniteProtectionActive = false;
			}
			if (this.TimeLeftUnstuck > 0 && !flag)
			{
				this.StartUnstuck();
			}
			if (this.IndefiniteProtectionActive)
			{
				return;
			}
			if (this.TimeLeftUnstuck > 0)
			{
				this.TimeLeftUnstuck--;
				if (this.TimeLeftUnstuck == 0)
				{
					ParticleOrchestrator.BroadcastOrRequestParticleSpawn(ParticleOrchestraType.ShimmerTownNPC, new ParticleOrchestraSettings
					{
						PositionInWorld = player.Bottom
					});
				}
			}
		}

		// Token: 0x060022E4 RID: 8932 RVA: 0x0053AEC7 File Offset: 0x005390C7
		public void StartUnstuck()
		{
			this.IndefiniteProtectionActive = true;
			this.TimeLeftUnstuck = 120;
		}

		// Token: 0x060022E5 RID: 8933 RVA: 0x0053AED8 File Offset: 0x005390D8
		public void Clear()
		{
			this.IndefiniteProtectionActive = false;
			this.TimeLeftUnstuck = 0;
		}

		// Token: 0x04004D18 RID: 19736
		public int TimeLeftUnstuck;

		// Token: 0x04004D19 RID: 19737
		public bool IndefiniteProtectionActive;
	}
}
