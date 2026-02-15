using System;
using System.Collections.Generic;
using Terraria.GameContent.Tile_Entities;

namespace Terraria.DataStructures
{
	// Token: 0x020005A5 RID: 1445
	public class TileEntitiesManager
	{
		// Token: 0x060038F8 RID: 14584 RVA: 0x0064FC2C File Offset: 0x0064DE2C
		private int AssignNewID()
		{
			int nextEntityID = this._nextEntityID;
			this._nextEntityID = nextEntityID + 1;
			return nextEntityID;
		}

		// Token: 0x060038F9 RID: 14585 RVA: 0x0064FC4A File Offset: 0x0064DE4A
		private bool InvalidEntityID(int id)
		{
			return id < 0 || id >= this._nextEntityID;
		}

		// Token: 0x060038FA RID: 14586 RVA: 0x0064FC60 File Offset: 0x0064DE60
		public void RegisterAll()
		{
			this.Register(new TETrainingDummy());
			this.Register(new TEItemFrame());
			this.Register(new TELogicSensor());
			this.Register(new TEDisplayDoll());
			this.Register(new TEWeaponsRack());
			this.Register(new TEHatRack());
			this.Register(new TEFoodPlatter());
			this.Register(new TETeleportationPylon());
			this.Register(new TEDeadCellsDisplayJar());
			this.Register(new TEKiteAnchor());
			this.Register(new TECritterAnchor());
		}

		// Token: 0x060038FB RID: 14587 RVA: 0x0064FCE8 File Offset: 0x0064DEE8
		public void Register(TileEntity entity)
		{
			int num = this.AssignNewID();
			this._types[num] = entity;
			entity.RegisterTileEntityID(num);
		}

		// Token: 0x060038FC RID: 14588 RVA: 0x0064FD10 File Offset: 0x0064DF10
		public bool CheckValidTile(int id, int x, int y)
		{
			return !this.InvalidEntityID(id) && this._types[id].IsTileValidForEntity(x, y);
		}

		// Token: 0x060038FD RID: 14589 RVA: 0x0064FD30 File Offset: 0x0064DF30
		public void NetPlaceEntity(int id, int x, int y)
		{
			if (this.InvalidEntityID(id))
			{
				return;
			}
			if (!this._types[id].IsTileValidForEntity(x, y))
			{
				return;
			}
			this._types[id].NetPlaceEntityAttempt(x, y);
		}

		// Token: 0x060038FE RID: 14590 RVA: 0x0064FD65 File Offset: 0x0064DF65
		public TileEntity GenerateInstance(int id)
		{
			if (this.InvalidEntityID(id))
			{
				return null;
			}
			return this._types[id].GenerateInstance();
		}

		// Token: 0x04005D44 RID: 23876
		private int _nextEntityID;

		// Token: 0x04005D45 RID: 23877
		private Dictionary<int, TileEntity> _types = new Dictionary<int, TileEntity>();
	}
}
