using System;

namespace Terraria.ID
{
	// Token: 0x0200019F RID: 415
	public static class PlayerItemSlotID
	{
		// Token: 0x06001F03 RID: 7939 RVA: 0x00512A98 File Offset: 0x00510C98
		static PlayerItemSlotID()
		{
			PlayerItemSlotID.Inventory0 = PlayerItemSlotID.AllocateSlots(58, true);
			PlayerItemSlotID.InventoryMouseItem = PlayerItemSlotID.AllocateSlots(1, true);
			PlayerItemSlotID.Armor0 = PlayerItemSlotID.AllocateSlots(20, true);
			PlayerItemSlotID.Dye0 = PlayerItemSlotID.AllocateSlots(10, true);
			PlayerItemSlotID.Misc0 = PlayerItemSlotID.AllocateSlots(5, true);
			PlayerItemSlotID.MiscDye0 = PlayerItemSlotID.AllocateSlots(5, true);
			PlayerItemSlotID.Bank1_0 = PlayerItemSlotID.AllocateSlots(200, false);
			PlayerItemSlotID.Bank2_0 = PlayerItemSlotID.AllocateSlots(200, false);
			PlayerItemSlotID.TrashItem = PlayerItemSlotID.AllocateSlots(1, false);
			PlayerItemSlotID.Bank3_0 = PlayerItemSlotID.AllocateSlots(200, false);
			PlayerItemSlotID.Bank4_0 = PlayerItemSlotID.AllocateSlots(200, true);
			PlayerItemSlotID.Loadout1_Armor_0 = PlayerItemSlotID.AllocateSlots(20, true);
			PlayerItemSlotID.Loadout1_Dye_0 = PlayerItemSlotID.AllocateSlots(10, true);
			PlayerItemSlotID.Loadout2_Armor_0 = PlayerItemSlotID.AllocateSlots(20, true);
			PlayerItemSlotID.Loadout2_Dye_0 = PlayerItemSlotID.AllocateSlots(10, true);
			PlayerItemSlotID.Loadout3_Armor_0 = PlayerItemSlotID.AllocateSlots(20, true);
			PlayerItemSlotID.Loadout3_Dye_0 = PlayerItemSlotID.AllocateSlots(10, true);
			PlayerItemSlotID.Count = PlayerItemSlotID._nextSlotId;
		}

		// Token: 0x06001F04 RID: 7940 RVA: 0x00512BA0 File Offset: 0x00510DA0
		private static int AllocateSlots(int amount, bool canNetRelay)
		{
			int nextSlotId = PlayerItemSlotID._nextSlotId;
			PlayerItemSlotID._nextSlotId += amount;
			int num = PlayerItemSlotID.CanRelay.Length;
			Array.Resize<bool>(ref PlayerItemSlotID.CanRelay, num + amount);
			for (int i = num; i < PlayerItemSlotID._nextSlotId; i++)
			{
				PlayerItemSlotID.CanRelay[i] = canNetRelay;
			}
			return nextSlotId;
		}

		// Token: 0x04001903 RID: 6403
		public static readonly int Inventory0;

		// Token: 0x04001904 RID: 6404
		public static readonly int InventoryMouseItem;

		// Token: 0x04001905 RID: 6405
		public static readonly int Armor0;

		// Token: 0x04001906 RID: 6406
		public static readonly int Dye0;

		// Token: 0x04001907 RID: 6407
		public static readonly int Misc0;

		// Token: 0x04001908 RID: 6408
		public static readonly int MiscDye0;

		// Token: 0x04001909 RID: 6409
		public static readonly int Bank1_0;

		// Token: 0x0400190A RID: 6410
		public static readonly int Bank2_0;

		// Token: 0x0400190B RID: 6411
		public static readonly int TrashItem;

		// Token: 0x0400190C RID: 6412
		public static readonly int Bank3_0;

		// Token: 0x0400190D RID: 6413
		public static readonly int Bank4_0;

		// Token: 0x0400190E RID: 6414
		public static readonly int Loadout1_Armor_0;

		// Token: 0x0400190F RID: 6415
		public static readonly int Loadout1_Dye_0;

		// Token: 0x04001910 RID: 6416
		public static readonly int Loadout2_Armor_0;

		// Token: 0x04001911 RID: 6417
		public static readonly int Loadout2_Dye_0;

		// Token: 0x04001912 RID: 6418
		public static readonly int Loadout3_Armor_0;

		// Token: 0x04001913 RID: 6419
		public static readonly int Loadout3_Dye_0;

		// Token: 0x04001914 RID: 6420
		public static readonly int Count;

		// Token: 0x04001915 RID: 6421
		public static bool[] CanRelay = new bool[0];

		// Token: 0x04001916 RID: 6422
		private static int _nextSlotId;

		// Token: 0x02000761 RID: 1889
		public struct SlotReference
		{
			// Token: 0x06004107 RID: 16647 RVA: 0x0069F352 File Offset: 0x0069D552
			public SlotReference(Player player, int slot)
			{
				this.Player = player;
				this.SlotId = slot;
			}

			// Token: 0x17000529 RID: 1321
			// (get) Token: 0x06004108 RID: 16648 RVA: 0x0069F364 File Offset: 0x0069D564
			// (set) Token: 0x06004109 RID: 16649 RVA: 0x0069F3B4 File Offset: 0x0069D5B4
			public Item Item
			{
				get
				{
					if (this.SlotId == PlayerItemSlotID.TrashItem)
					{
						return this.Player.trashItem;
					}
					Item[] array;
					int num;
					if (!this.TryGetArraySlot(out array, out num))
					{
						throw new IndexOutOfRangeException("SlotId: " + this.SlotId);
					}
					return array[num];
				}
				set
				{
					if (this.SlotId == PlayerItemSlotID.TrashItem)
					{
						this.Player.trashItem = value;
						return;
					}
					Item[] array;
					int num;
					if (!this.TryGetArraySlot(out array, out num))
					{
						throw new IndexOutOfRangeException("SlotId: " + this.SlotId);
					}
					array[num] = value;
				}
			}

			// Token: 0x0600410A RID: 16650 RVA: 0x0069F408 File Offset: 0x0069D608
			private bool TryGetArraySlot(out Item[] arr, out int slot)
			{
				if (this.SlotId >= PlayerItemSlotID.Loadout3_Dye_0)
				{
					slot = this.SlotId - PlayerItemSlotID.Loadout3_Dye_0;
					arr = this.Player.Loadouts[2].Dye;
				}
				else if (this.SlotId >= PlayerItemSlotID.Loadout3_Armor_0)
				{
					slot = this.SlotId - PlayerItemSlotID.Loadout3_Armor_0;
					arr = this.Player.Loadouts[2].Armor;
				}
				else if (this.SlotId >= PlayerItemSlotID.Loadout2_Dye_0)
				{
					slot = this.SlotId - PlayerItemSlotID.Loadout2_Dye_0;
					arr = this.Player.Loadouts[1].Dye;
				}
				else if (this.SlotId >= PlayerItemSlotID.Loadout2_Armor_0)
				{
					slot = this.SlotId - PlayerItemSlotID.Loadout2_Armor_0;
					arr = this.Player.Loadouts[1].Armor;
				}
				else if (this.SlotId >= PlayerItemSlotID.Loadout1_Dye_0)
				{
					slot = this.SlotId - PlayerItemSlotID.Loadout1_Dye_0;
					arr = this.Player.Loadouts[0].Dye;
				}
				else if (this.SlotId >= PlayerItemSlotID.Loadout1_Armor_0)
				{
					slot = this.SlotId - PlayerItemSlotID.Loadout1_Armor_0;
					arr = this.Player.Loadouts[0].Armor;
				}
				else if (this.SlotId >= PlayerItemSlotID.Bank4_0)
				{
					slot = this.SlotId - PlayerItemSlotID.Bank4_0;
					arr = this.Player.bank4.item;
				}
				else if (this.SlotId >= PlayerItemSlotID.Bank3_0)
				{
					slot = this.SlotId - PlayerItemSlotID.Bank3_0;
					arr = this.Player.bank3.item;
				}
				else
				{
					if (this.SlotId >= PlayerItemSlotID.TrashItem)
					{
						slot = 0;
						arr = null;
						return false;
					}
					if (this.SlotId >= PlayerItemSlotID.Bank2_0)
					{
						slot = this.SlotId - PlayerItemSlotID.Bank2_0;
						arr = this.Player.bank2.item;
					}
					else if (this.SlotId >= PlayerItemSlotID.Bank1_0)
					{
						slot = this.SlotId - PlayerItemSlotID.Bank1_0;
						arr = this.Player.bank.item;
					}
					else if (this.SlotId >= PlayerItemSlotID.MiscDye0)
					{
						slot = this.SlotId - PlayerItemSlotID.MiscDye0;
						arr = this.Player.miscDyes;
					}
					else if (this.SlotId >= PlayerItemSlotID.Misc0)
					{
						slot = this.SlotId - PlayerItemSlotID.Misc0;
						arr = this.Player.miscEquips;
					}
					else if (this.SlotId >= PlayerItemSlotID.Dye0)
					{
						slot = this.SlotId - PlayerItemSlotID.Dye0;
						arr = this.Player.dye;
					}
					else if (this.SlotId >= PlayerItemSlotID.Armor0)
					{
						slot = this.SlotId - PlayerItemSlotID.Armor0;
						arr = this.Player.armor;
					}
					else
					{
						slot = this.SlotId - PlayerItemSlotID.Inventory0;
						arr = this.Player.inventory;
					}
				}
				return true;
			}

			// Token: 0x040069D8 RID: 27096
			public readonly Player Player;

			// Token: 0x040069D9 RID: 27097
			public readonly int SlotId;
		}
	}
}
