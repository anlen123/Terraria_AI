using System;

namespace Terraria.DataStructures
{
	// Token: 0x020005A6 RID: 1446
	public abstract class TileEntityType<T> : TileEntity where T : TileEntity, new()
	{
		// Token: 0x06003900 RID: 14592 RVA: 0x0064FD96 File Offset: 0x0064DF96
		public override void RegisterTileEntityID(int assignedID)
		{
			TileEntityType<T>.EntityTypeID = (byte)assignedID;
		}

		// Token: 0x06003901 RID: 14593 RVA: 0x0064FD9F File Offset: 0x0064DF9F
		public override TileEntity GenerateInstance()
		{
			return Activator.CreateInstance<T>();
		}

		// Token: 0x06003902 RID: 14594 RVA: 0x0064FDAC File Offset: 0x0064DFAC
		public override void NetPlaceEntityAttempt(int x, int y)
		{
			int number = TileEntityType<T>.Place(x, y);
			NetMessage.SendData(86, -1, -1, null, number, (float)x, (float)y, 0f, 0, 0, 0);
		}

		// Token: 0x06003903 RID: 14595 RVA: 0x0064FDD8 File Offset: 0x0064DFD8
		public static int Place(int x, int y)
		{
			return TileEntity.Place(x, y, (int)TileEntityType<T>.EntityTypeID);
		}

		// Token: 0x06003904 RID: 14596 RVA: 0x0064FDE6 File Offset: 0x0064DFE6
		public static void Kill(int x, int y)
		{
			TileEntity.Kill(x, y, (int)TileEntityType<T>.EntityTypeID);
		}

		// Token: 0x06003905 RID: 14597 RVA: 0x0064FDF4 File Offset: 0x0064DFF4
		public static int Find(int x, int y)
		{
			T t;
			if (!TileEntity.TryGetAt<T>(x, y, out t))
			{
				return -1;
			}
			return t.ID;
		}

		// Token: 0x04005D46 RID: 23878
		protected static byte EntityTypeID;
	}
}
