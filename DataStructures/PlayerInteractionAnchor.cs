using System;

namespace Terraria.DataStructures
{
	// Token: 0x020005A0 RID: 1440
	public struct PlayerInteractionAnchor
	{
		// Token: 0x060038E5 RID: 14565 RVA: 0x0064FA3A File Offset: 0x0064DC3A
		public PlayerInteractionAnchor(int entityID, int x = -1, int y = -1)
		{
			this.interactEntityID = entityID;
			this.X = x;
			this.Y = y;
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x060038E6 RID: 14566 RVA: 0x0064FA51 File Offset: 0x0064DC51
		public bool InUse
		{
			get
			{
				return this.interactEntityID != -1;
			}
		}

		// Token: 0x060038E7 RID: 14567 RVA: 0x0064FA5F File Offset: 0x0064DC5F
		public void Clear()
		{
			this.interactEntityID = -1;
			this.X = -1;
			this.Y = -1;
		}

		// Token: 0x060038E8 RID: 14568 RVA: 0x0064FA3A File Offset: 0x0064DC3A
		public void Set(int entityID, int x, int y)
		{
			this.interactEntityID = entityID;
			this.X = x;
			this.Y = y;
		}

		// Token: 0x060038E9 RID: 14569 RVA: 0x0064FA76 File Offset: 0x0064DC76
		public bool IsInValidUseTileEntity()
		{
			return this.GetTileEntity() != null;
		}

		// Token: 0x060038EA RID: 14570 RVA: 0x0064FA84 File Offset: 0x0064DC84
		public TileEntity GetTileEntity()
		{
			TileEntity result = null;
			if (this.InUse)
			{
				TileEntity.TryGet<TileEntity>(this.interactEntityID, out result);
			}
			return result;
		}

		// Token: 0x04005D1E RID: 23838
		public int interactEntityID;

		// Token: 0x04005D1F RID: 23839
		public int X;

		// Token: 0x04005D20 RID: 23840
		public int Y;
	}
}
