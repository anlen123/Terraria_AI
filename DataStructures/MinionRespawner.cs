using System;
using System.Collections.Generic;

namespace Terraria.DataStructures
{
	// Token: 0x02000554 RID: 1364
	public class MinionRespawner
	{
		// Token: 0x0600377A RID: 14202 RVA: 0x0062E601 File Offset: 0x0062C801
		public void Clear()
		{
			this._minions.Clear();
		}

		// Token: 0x0600377B RID: 14203 RVA: 0x0062E610 File Offset: 0x0062C810
		public void CollectMinionsFor(Player player)
		{
			int whoAmI = player.whoAmI;
			this.Clear();
			for (int i = 0; i < 1000; i++)
			{
				Projectile projectile = Main.projectile[i];
				if (projectile.active && projectile.owner == whoAmI && projectile.MinionSpawnInfo != null)
				{
					this._minions.Add(projectile.MinionSpawnInfo);
				}
			}
		}

		// Token: 0x0600377C RID: 14204 RVA: 0x0062E66C File Offset: 0x0062C86C
		public void RestoreMinionsFor(Player player)
		{
			int mouseX = Main.mouseX;
			int mouseY = Main.mouseY;
			Main.mouseX = Main.screenWidth / 2;
			Main.mouseY = Main.screenHeight / 2;
			foreach (MinionSpawnInfo minionSpawnInfo in this._minions)
			{
				minionSpawnInfo.TryRespawn(player);
			}
			Main.mouseX = mouseX;
			Main.mouseY = mouseY;
			this.Clear();
		}

		// Token: 0x04005B93 RID: 23443
		private List<MinionSpawnInfo> _minions = new List<MinionSpawnInfo>();
	}
}
