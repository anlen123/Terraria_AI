using System;

namespace Terraria
{
	// Token: 0x0200002B RID: 43
	public class LiquidBuffer
	{
		// Token: 0x0600020E RID: 526 RVA: 0x00021E38 File Offset: 0x00020038
		public static void AddBuffer(int x, int y)
		{
			if (LiquidBuffer.numLiquidBuffer >= 49998)
			{
				return;
			}
			if (Main.tile[x, y].checkingLiquid())
			{
				return;
			}
			Main.tile[x, y].checkingLiquid(true);
			Main.liquidBuffer[LiquidBuffer.numLiquidBuffer].x = x;
			Main.liquidBuffer[LiquidBuffer.numLiquidBuffer].y = y;
			LiquidBuffer.numLiquidBuffer++;
		}

		// Token: 0x0600020F RID: 527 RVA: 0x00021EA8 File Offset: 0x000200A8
		public static void DelBuffer(int l)
		{
			LiquidBuffer.numLiquidBuffer--;
			Main.liquidBuffer[l].x = Main.liquidBuffer[LiquidBuffer.numLiquidBuffer].x;
			Main.liquidBuffer[l].y = Main.liquidBuffer[LiquidBuffer.numLiquidBuffer].y;
		}

		// Token: 0x04000184 RID: 388
		public static int numLiquidBuffer;

		// Token: 0x04000185 RID: 389
		public int x;

		// Token: 0x04000186 RID: 390
		public int y;
	}
}
