using System;

namespace Terraria.DataStructures
{
	// Token: 0x0200053D RID: 1341
	public class CachedProjectileCounterBuffTextHandler : IBuffTextHandler
	{
		// Token: 0x06003749 RID: 14153 RVA: 0x0062DCB8 File Offset: 0x0062BEB8
		public CachedProjectileCounterBuffTextHandler(params int[] projectileTypesToLookFor)
		{
			this.projectilesToLookFor = projectileTypesToLookFor;
		}

		// Token: 0x0600374A RID: 14154 RVA: 0x0062DCC8 File Offset: 0x0062BEC8
		public string HandleBuffText()
		{
			if (this.projectilesToLookFor == null)
			{
				return null;
			}
			int[] ownedProjectileCounts = Main.LocalPlayer.ownedProjectileCounts;
			float num = 0f;
			foreach (int num2 in this.projectilesToLookFor)
			{
				num += (float)ownedProjectileCounts[num2];
			}
			if (num > 0f)
			{
				return "x" + num;
			}
			return null;
		}

		// Token: 0x04005B64 RID: 23396
		private int[] projectilesToLookFor;
	}
}
