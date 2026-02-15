using System;

namespace Terraria.GameContent
{
	// Token: 0x0200023A RID: 570
	public static class Luck
	{
		// Token: 0x0600226F RID: 8815 RVA: 0x005380B0 File Offset: 0x005362B0
		public static int RollLuck(float luck, int range)
		{
			if (luck > 0f && Main.rand.NextFloat() < luck)
			{
				return Main.rand.Next(Main.rand.Next(range / 2, range));
			}
			if (luck < 0f && Main.rand.NextFloat() < -luck)
			{
				return Main.rand.Next(Main.rand.Next(range, range * 2));
			}
			return Main.rand.Next(range);
		}

		// Token: 0x06002270 RID: 8816 RVA: 0x00538128 File Offset: 0x00536328
		public static int RollBadLuck(float luck, int range)
		{
			if (luck > 0f && Main.rand.NextFloat() < luck)
			{
				return Main.rand.Next(Main.rand.Next(range, range * 2));
			}
			if (luck < 0f && Main.rand.NextFloat() < -luck)
			{
				return Main.rand.Next(Main.rand.Next(range / 2, range));
			}
			return Main.rand.Next(range);
		}

		// Token: 0x06002271 RID: 8817 RVA: 0x0053819D File Offset: 0x0053639D
		public static int RollOnlyBadLuck(float luck, int range)
		{
			if (luck < 0f && Main.rand.NextFloat() < -luck)
			{
				return Main.rand.Next(Main.rand.Next(range / 2, range));
			}
			return Main.rand.Next(range);
		}

		// Token: 0x06002272 RID: 8818 RVA: 0x005381DC File Offset: 0x005363DC
		public static int RollBadLuckExtreme(float luck, int range)
		{
			if (luck > 0f && Main.rand.NextFloat() < luck)
			{
				return Main.rand.Next(range * 10);
			}
			if (luck < 0f && Main.rand.NextFloat() < -luck)
			{
				return Main.rand.Next(range / 10);
			}
			return Main.rand.Next(range);
		}

		// Token: 0x06002273 RID: 8819 RVA: 0x0053823D File Offset: 0x0053643D
		public static int RollOnlyBadLuckExtreme(float luck, int range)
		{
			if (luck < 0f && Main.rand.NextFloat() < -luck)
			{
				return Main.rand.Next(range / 10);
			}
			return -1;
		}
	}
}
