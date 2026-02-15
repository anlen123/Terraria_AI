using System;
using Terraria.DataStructures;
using Terraria.GameContent.LeashedEntities;
using Terraria.ID;

namespace Terraria.GameContent.Tile_Entities
{
	// Token: 0x02000410 RID: 1040
	public class TEKiteAnchor : TELeashedEntityAnchorWithItem
	{
		// Token: 0x06002FA5 RID: 12197 RVA: 0x005B40DE File Offset: 0x005B22DE
		public TEKiteAnchor()
		{
			this.type = TEKiteAnchor._myEntityID;
		}

		// Token: 0x06002FA6 RID: 12198 RVA: 0x005B40F1 File Offset: 0x005B22F1
		public override void RegisterTileEntityID(int assignedID)
		{
			this.type = (TEKiteAnchor._myEntityID = (byte)assignedID);
		}

		// Token: 0x06002FA7 RID: 12199 RVA: 0x005B4104 File Offset: 0x005B2304
		public override bool IsTileValidForEntity(int x, int y)
		{
			Tile tile = Main.tile[x, y];
			return tile.active() && tile.type == 723;
		}

		// Token: 0x06002FA8 RID: 12200 RVA: 0x005B4135 File Offset: 0x005B2335
		public override TileEntity GenerateInstance()
		{
			return new TEKiteAnchor();
		}

		// Token: 0x06002FA9 RID: 12201 RVA: 0x005B413C File Offset: 0x005B233C
		public static void Kill(int x, int y)
		{
			TileEntity.Kill(x, y, (int)TEKiteAnchor._myEntityID);
		}

		// Token: 0x06002FAA RID: 12202 RVA: 0x005B414A File Offset: 0x005B234A
		public static int Hook_AfterPlacement(int x, int y, int type, int style, int direction, int alternate)
		{
			return TELeashedEntityAnchorWithItem.PlaceFromPlayerPlacementHook(x, y, (int)TEKiteAnchor._myEntityID);
		}

		// Token: 0x06002FAB RID: 12203 RVA: 0x005B4158 File Offset: 0x005B2358
		public override bool FitsItem(int itemType)
		{
			return ItemID.Sets.IsAKite[itemType];
		}

		// Token: 0x06002FAC RID: 12204 RVA: 0x005B4161 File Offset: 0x005B2361
		public override LeashedEntity CreateLeashedEntity()
		{
			if (this.itemType <= 0)
			{
				return null;
			}
			LeashedKite leashedKite = (LeashedKite)LeashedKite.Prototype.NewInstance();
			leashedKite.SetDefaults(ContentSamples.ItemsByType[this.itemType].shoot);
			return leashedKite;
		}

		// Token: 0x06002FAD RID: 12205 RVA: 0x005B4198 File Offset: 0x005B2398
		public static void DebugPlace(int x, int y, int itemType)
		{
			int key = TileEntity.Place(x, y, (int)TEKiteAnchor._myEntityID);
			((TEKiteAnchor)TileEntity.ByID[key]).InsertItem(itemType);
			NetMessage.SendData(156, -1, -1, null, x, (float)y, (float)itemType, 0f, 0, 0, 0);
		}

		// Token: 0x0400566C RID: 22124
		private static byte _myEntityID;
	}
}
