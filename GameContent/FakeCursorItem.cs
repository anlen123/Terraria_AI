using System;

namespace Terraria.GameContent
{
	// Token: 0x02000233 RID: 563
	public static class FakeCursorItem
	{
		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06002223 RID: 8739 RVA: 0x0053482C File Offset: 0x00532A2C
		public static Item Item
		{
			get
			{
				int num = Main.mouseItem.IsAir ? 0 : Main.mouseItem.stack;
				if (FakeCursorItem._type != FakeCursorItem._item.type)
				{
					FakeCursorItem._item.SetDefaults(FakeCursorItem._type, null);
				}
				else
				{
					FakeCursorItem._item.Refresh(true);
				}
				if (FakeCursorItem._prefix != (int)FakeCursorItem._item.prefix)
				{
					FakeCursorItem._item.Prefix(FakeCursorItem._prefix);
				}
				FakeCursorItem._item.stack = FakeCursorItem._stack + num;
				return FakeCursorItem._item;
			}
		}

		// Token: 0x06002224 RID: 8740 RVA: 0x005348B8 File Offset: 0x00532AB8
		public static void Reset()
		{
			FakeCursorItem._type = 0;
			FakeCursorItem._stack = 0;
		}

		// Token: 0x06002225 RID: 8741 RVA: 0x005348C6 File Offset: 0x00532AC6
		public static void Add(int itemType, int itemStack, int itemPrefix = 0)
		{
			if (itemStack == 0)
			{
				return;
			}
			if (FakeCursorItem._type == itemType)
			{
				FakeCursorItem._stack += itemStack;
			}
			else
			{
				FakeCursorItem._stack = itemStack;
			}
			FakeCursorItem._type = itemType;
			FakeCursorItem._prefix = itemPrefix;
		}

		// Token: 0x06002226 RID: 8742 RVA: 0x005348F4 File Offset: 0x00532AF4
		public static void Add(Item item)
		{
			FakeCursorItem.Add(item.type, item.stack, (int)item.prefix);
		}

		// Token: 0x06002227 RID: 8743 RVA: 0x0053490D File Offset: 0x00532B0D
		public static void Remove(int itemType, int itemStack)
		{
			if (itemStack == 0)
			{
				return;
			}
			if (FakeCursorItem._type != itemType)
			{
				return;
			}
			FakeCursorItem._stack -= itemStack;
			if (FakeCursorItem._stack <= 0)
			{
				FakeCursorItem._type = 0;
			}
		}

		// Token: 0x04004CB0 RID: 19632
		private static int _type;

		// Token: 0x04004CB1 RID: 19633
		private static int _stack;

		// Token: 0x04004CB2 RID: 19634
		private static int _prefix;

		// Token: 0x04004CB3 RID: 19635
		private static Item _item = new Item();
	}
}
