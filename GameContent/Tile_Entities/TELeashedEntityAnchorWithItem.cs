using System;
using System.IO;
using Terraria.DataStructures;

namespace Terraria.GameContent.Tile_Entities
{
	// Token: 0x0200040F RID: 1039
	public abstract class TELeashedEntityAnchorWithItem : TELeashedEntityAnchor
	{
		// Token: 0x06002F9D RID: 12189 RVA: 0x005B3F92 File Offset: 0x005B2192
		public override void WriteExtraData(BinaryWriter writer, bool networkSend)
		{
			writer.Write((short)this.itemType);
		}

		// Token: 0x06002F9E RID: 12190 RVA: 0x005B3FA1 File Offset: 0x005B21A1
		public override void ReadExtraData(BinaryReader reader, int gameVersion, bool networkSend)
		{
			this.itemType = (int)reader.ReadInt16();
		}

		// Token: 0x06002F9F RID: 12191 RVA: 0x005B3FB0 File Offset: 0x005B21B0
		public void DropItemForTileBreak()
		{
			if (this.itemType <= 0)
			{
				return;
			}
			if (Main.netMode != 1)
			{
				Item.NewItem(new EntitySource_TileBreak((int)this.Position.X, (int)this.Position.Y), (int)(this.Position.X * 16), (int)(this.Position.Y * 16), 16, 16, this.itemType, 1, false, 0, false);
			}
			this.itemType = 0;
		}

		// Token: 0x06002FA0 RID: 12192 RVA: 0x005B4021 File Offset: 0x005B2221
		public void InsertItem(int itemType)
		{
			this.itemType = itemType;
			base.RespawnLeashedEntity();
		}

		// Token: 0x06002FA1 RID: 12193 RVA: 0x005B4030 File Offset: 0x005B2230
		public override void OnWorldLoaded()
		{
			if (!this.FitsItem(this.itemType))
			{
				this.itemType = 0;
			}
			base.OnWorldLoaded();
		}

		// Token: 0x06002FA2 RID: 12194
		public abstract bool FitsItem(int itemType);

		// Token: 0x06002FA3 RID: 12195 RVA: 0x005B4050 File Offset: 0x005B2250
		protected new static int PlaceFromPlayerPlacementHook(int x, int y, int type)
		{
			int num = TELeashedEntityAnchor.PlaceFromPlayerPlacementHook(x, y, type);
			Item heldItem = Main.LocalPlayer.HeldItem;
			int type2 = heldItem.type;
			if (!heldItem.consumable)
			{
				Item item = heldItem;
				int num2 = item.stack - 1;
				item.stack = num2;
				if (num2 <= 0)
				{
					heldItem.TurnToAir(false);
				}
			}
			if (Main.netMode == 1)
			{
				NetMessage.SendData(156, -1, -1, null, x, (float)y, (float)type2, 0f, 0, 0, 0);
			}
			else
			{
				((TELeashedEntityAnchorWithItem)TileEntity.ByID[num]).InsertItem(type2);
			}
			return num;
		}

		// Token: 0x0400566B RID: 22123
		protected int itemType;
	}
}
