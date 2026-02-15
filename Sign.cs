using System;

namespace Terraria
{
	// Token: 0x0200004B RID: 75
	public class Sign
	{
		// Token: 0x06000B5A RID: 2906 RVA: 0x00354C18 File Offset: 0x00352E18
		public static void KillSign(int x, int y)
		{
			for (int i = 0; i < 32000; i++)
			{
				if (Main.sign[i] != null && Main.sign[i].x == x && Main.sign[i].y == y)
				{
					Main.sign[i] = null;
				}
			}
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x00354C64 File Offset: 0x00352E64
		public static int ReadSign(int i, int j, bool CreateIfMissing = true)
		{
			int num = (int)(Main.tile[i, j].frameX / 18);
			int num2 = (int)(Main.tile[i, j].frameY / 18);
			num %= 2;
			int num3 = i - num;
			int num4 = j - num2;
			if (!Main.tileSign[(int)Main.tile[num3, num4].type])
			{
				Sign.KillSign(num3, num4);
				return -1;
			}
			int num5 = -1;
			for (int k = 0; k < 32000; k++)
			{
				if (Main.sign[k] != null && Main.sign[k].x == num3 && Main.sign[k].y == num4)
				{
					num5 = k;
					break;
				}
			}
			if (num5 < 0 && CreateIfMissing)
			{
				for (int l = 0; l < 32000; l++)
				{
					if (Main.sign[l] == null)
					{
						num5 = l;
						Main.sign[l] = new Sign();
						Main.sign[l].x = num3;
						Main.sign[l].y = num4;
						Main.sign[l].text = "";
						break;
					}
				}
			}
			return num5;
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x00354D7C File Offset: 0x00352F7C
		public static void TextSign(int i, string text)
		{
			if (Main.tile[Main.sign[i].x, Main.sign[i].y] == null || !Main.tile[Main.sign[i].x, Main.sign[i].y].active() || !Main.tileSign[(int)Main.tile[Main.sign[i].x, Main.sign[i].y].type])
			{
				Main.sign[i] = null;
				return;
			}
			Main.sign[i].text = text;
		}

		// Token: 0x06000B5D RID: 2909 RVA: 0x00354E1C File Offset: 0x0035301C
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"x",
				this.x,
				"\ty",
				this.y,
				"\t",
				this.text
			});
		}

		// Token: 0x040009AE RID: 2478
		public const int maxSigns = 32000;

		// Token: 0x040009AF RID: 2479
		public int x;

		// Token: 0x040009B0 RID: 2480
		public int y;

		// Token: 0x040009B1 RID: 2481
		public string text;
	}
}
