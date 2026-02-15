using System;

namespace Terraria.Audio
{
	// Token: 0x020005C8 RID: 1480
	public class ProjectileAudioTracker
	{
		// Token: 0x06003A1C RID: 14876 RVA: 0x00653B8D File Offset: 0x00651D8D
		public ProjectileAudioTracker(Projectile proj)
		{
			this._expectedIndex = proj.whoAmI;
			this._expectedType = proj.type;
		}

		// Token: 0x06003A1D RID: 14877 RVA: 0x00653BB0 File Offset: 0x00651DB0
		public bool IsActiveAndInGame()
		{
			if (Main.gameMenu)
			{
				return false;
			}
			Projectile projectile = Main.projectile[this._expectedIndex];
			return projectile.active && projectile.type == this._expectedType;
		}

		// Token: 0x04005DA0 RID: 23968
		private int _expectedType;

		// Token: 0x04005DA1 RID: 23969
		private int _expectedIndex;
	}
}
