using System;
using Terraria.DataStructures;
using Terraria.ID;

namespace Terraria.GameContent.Tile_Entities
{
	// Token: 0x0200040E RID: 1038
	public abstract class TELeashedEntityAnchor : TileEntity
	{
		// Token: 0x06002F95 RID: 12181 RVA: 0x005B3EC8 File Offset: 0x005B20C8
		public override void NetPlaceEntityAttempt(int x, int y)
		{
			int number = TileEntity.Place(x, y, (int)this.type);
			NetMessage.SendData(86, -1, -1, null, number, (float)x, (float)y, 0f, 0, 0, 0);
		}

		// Token: 0x06002F96 RID: 12182 RVA: 0x005B3EFA File Offset: 0x005B20FA
		public override void OnRemoved()
		{
			this.DespawnLeashedEntity();
		}

		// Token: 0x06002F97 RID: 12183 RVA: 0x005B3F04 File Offset: 0x005B2104
		protected static int PlaceFromPlayerPlacementHook(int x, int y, int type)
		{
			if (Main.netMode == 1)
			{
				NetMessage.SendTileSquare(Main.myPlayer, x, y, TileChangeType.None);
				NetMessage.SendData(87, -1, -1, null, x, (float)y, (float)type, 0f, 0, 0, 0);
				return -1;
			}
			return TileEntity.Place(x, y, type);
		}

		// Token: 0x06002F98 RID: 12184 RVA: 0x005B3F47 File Offset: 0x005B2147
		public override void OnWorldLoaded()
		{
			this.RespawnLeashedEntity();
		}

		// Token: 0x06002F99 RID: 12185 RVA: 0x005B3F4F File Offset: 0x005B214F
		protected void DespawnLeashedEntity()
		{
			if (this.leashedEntity != null)
			{
				this.leashedEntity.active = false;
			}
		}

		// Token: 0x06002F9A RID: 12186 RVA: 0x005B3F65 File Offset: 0x005B2165
		protected void RespawnLeashedEntity()
		{
			this.DespawnLeashedEntity();
			this.leashedEntity = this.CreateLeashedEntity();
			LeashedEntity.AddNewEntity(this.leashedEntity, this.Position);
		}

		// Token: 0x06002F9B RID: 12187
		public abstract LeashedEntity CreateLeashedEntity();

		// Token: 0x0400566A RID: 22122
		private LeashedEntity leashedEntity;
	}
}
