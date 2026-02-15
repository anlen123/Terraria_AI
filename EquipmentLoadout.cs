using System;
using System.IO;
using Terraria.DataStructures;

namespace Terraria
{
	// Token: 0x02000046 RID: 70
	public class EquipmentLoadout : IFixLoadedData
	{
		// Token: 0x06000781 RID: 1921 RVA: 0x002D6791 File Offset: 0x002D4991
		public EquipmentLoadout()
		{
			this.Armor = this.CreateItemArray(20);
			this.Dye = this.CreateItemArray(10);
			this.Hide = new bool[10];
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x002D67C4 File Offset: 0x002D49C4
		private Item[] CreateItemArray(int length)
		{
			Item[] array = new Item[length];
			for (int i = 0; i < length; i++)
			{
				array[i] = new Item();
			}
			return array;
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x002D67F0 File Offset: 0x002D49F0
		public void Serialize(BinaryWriter writer)
		{
			ItemSerializationContext context = ItemSerializationContext.SavingAndLoading;
			for (int i = 0; i < this.Armor.Length; i++)
			{
				this.Armor[i].Serialize(writer, context);
			}
			for (int j = 0; j < this.Dye.Length; j++)
			{
				this.Dye[j].Serialize(writer, context);
			}
			for (int k = 0; k < this.Hide.Length; k++)
			{
				writer.Write(this.Hide[k]);
			}
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x002D6864 File Offset: 0x002D4A64
		public void Deserialize(BinaryReader reader, int gameVersion)
		{
			ItemSerializationContext context = ItemSerializationContext.SavingAndLoading;
			for (int i = 0; i < this.Armor.Length; i++)
			{
				this.Armor[i].DeserializeFrom(reader, context);
			}
			for (int j = 0; j < this.Dye.Length; j++)
			{
				this.Dye[j].DeserializeFrom(reader, context);
			}
			for (int k = 0; k < this.Hide.Length; k++)
			{
				this.Hide[k] = reader.ReadBoolean();
			}
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x002D68D8 File Offset: 0x002D4AD8
		public void Swap(Player player)
		{
			Item[] armor = player.armor;
			for (int i = 0; i < armor.Length; i++)
			{
				Utils.Swap<Item>(ref armor[i], ref this.Armor[i]);
			}
			Item[] dye = player.dye;
			for (int j = 0; j < dye.Length; j++)
			{
				Utils.Swap<Item>(ref dye[j], ref this.Dye[j]);
			}
			bool[] hideVisibleAccessory = player.hideVisibleAccessory;
			for (int k = 0; k < hideVisibleAccessory.Length; k++)
			{
				Utils.Swap<bool>(ref hideVisibleAccessory[k], ref this.Hide[k]);
			}
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x002D6978 File Offset: 0x002D4B78
		public void TryDroppingItems(Player player, IEntitySource source)
		{
			for (int i = 0; i < this.Armor.Length; i++)
			{
				player.TryDroppingSingleItem(source, this.Armor[i]);
			}
			for (int j = 0; j < this.Dye.Length; j++)
			{
				player.TryDroppingSingleItem(source, this.Dye[j]);
			}
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x002D69CC File Offset: 0x002D4BCC
		public void FixLoadedData()
		{
			for (int i = 0; i < this.Armor.Length; i++)
			{
				this.Armor[i].FixAgainstExploit();
			}
			for (int j = 0; j < this.Dye.Length; j++)
			{
				this.Dye[j].FixAgainstExploit();
			}
			Player.FixLoadedData_EliminiateDuplicateAccessories(this.Armor);
		}

		// Token: 0x040004FD RID: 1277
		public Item[] Armor;

		// Token: 0x040004FE RID: 1278
		public Item[] Dye;

		// Token: 0x040004FF RID: 1279
		public bool[] Hide;
	}
}
