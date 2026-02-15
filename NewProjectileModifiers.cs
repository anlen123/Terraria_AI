using System;

namespace Terraria
{
	// Token: 0x0200003B RID: 59
	public static class NewProjectileModifiers
	{
		// Token: 0x060004AF RID: 1199 RVA: 0x0012B803 File Offset: 0x00129A03
		public static void RainHazard(Projectile projectile)
		{
			projectile.netImportant = true;
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x0012B80C File Offset: 0x00129A0C
		public static void IchorDartUpdatePenetrate(Projectile projectile)
		{
			if (Main.myPlayer != projectile.owner)
			{
				return;
			}
			if (projectile.ai[1] >= 0f)
			{
				projectile.maxPenetrate = (projectile.penetrate = -1);
				return;
			}
			if (projectile.penetrate < 0)
			{
				projectile.maxPenetrate = (projectile.penetrate = 1);
			}
		}
	}
}
