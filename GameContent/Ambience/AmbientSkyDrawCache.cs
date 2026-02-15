using System;

namespace Terraria.GameContent.Ambience
{
	// Token: 0x02000363 RID: 867
	public class AmbientSkyDrawCache
	{
		// Token: 0x060028D0 RID: 10448 RVA: 0x00574B94 File Offset: 0x00572D94
		public void SetUnderworldInfo(int drawIndex, float scale)
		{
			this.Underworld[drawIndex] = new AmbientSkyDrawCache.UnderworldCache
			{
				Scale = scale
			};
		}

		// Token: 0x060028D1 RID: 10449 RVA: 0x00574BC0 File Offset: 0x00572DC0
		public void SetOceanLineInfo(float yScreenPosition, float oceanOpacity)
		{
			this.OceanLineInfo = new AmbientSkyDrawCache.OceanLineCache
			{
				YScreenPosition = yScreenPosition,
				OceanOpacity = oceanOpacity
			};
		}

		// Token: 0x04005151 RID: 20817
		public static AmbientSkyDrawCache Instance = new AmbientSkyDrawCache();

		// Token: 0x04005152 RID: 20818
		public AmbientSkyDrawCache.UnderworldCache[] Underworld = new AmbientSkyDrawCache.UnderworldCache[5];

		// Token: 0x04005153 RID: 20819
		public AmbientSkyDrawCache.OceanLineCache OceanLineInfo;

		// Token: 0x020008C7 RID: 2247
		public struct UnderworldCache
		{
			// Token: 0x04007323 RID: 29475
			public float Scale;
		}

		// Token: 0x020008C8 RID: 2248
		public struct OceanLineCache
		{
			// Token: 0x04007324 RID: 29476
			public float YScreenPosition;

			// Token: 0x04007325 RID: 29477
			public float OceanOpacity;
		}
	}
}
