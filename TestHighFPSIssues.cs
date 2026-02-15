using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.GameInput;

namespace Terraria
{
	// Token: 0x0200001A RID: 26
	public class TestHighFPSIssues
	{
		// Token: 0x060000D4 RID: 212 RVA: 0x0000C4FC File Offset: 0x0000A6FC
		public static void TapUpdate(GameTime gt)
		{
			TestHighFPSIssues._tapUpdates.Add(gt.TotalGameTime.TotalMilliseconds);
			TestHighFPSIssues.conD = 0;
			TestHighFPSIssues.race--;
			if (++TestHighFPSIssues.conU > TestHighFPSIssues.conUH)
			{
				TestHighFPSIssues.conUH = TestHighFPSIssues.conU;
			}
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x0000C554 File Offset: 0x0000A754
		public static void TapUpdateEnd(GameTime gt)
		{
			TestHighFPSIssues._tapUpdateEnds.Add(gt.TotalGameTime.TotalMilliseconds);
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x0000C57C File Offset: 0x0000A77C
		public static void TapDraw(GameTime gt)
		{
			TestHighFPSIssues._tapDraws.Add(gt.TotalGameTime.TotalMilliseconds);
			TestHighFPSIssues.conU = 0;
			TestHighFPSIssues.race++;
			if (++TestHighFPSIssues.conD > TestHighFPSIssues.conDH)
			{
				TestHighFPSIssues.conDH = TestHighFPSIssues.conD;
			}
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x0000C5D4 File Offset: 0x0000A7D4
		public static void Update(GameTime gt)
		{
			if (PlayerInput.Triggers.Current.Down)
			{
				TestHighFPSIssues.race = (TestHighFPSIssues.conUH = (TestHighFPSIssues.conDH = 0));
			}
			double num = gt.TotalGameTime.TotalMilliseconds - 5000.0;
			while (TestHighFPSIssues._tapUpdates.Count > 0)
			{
				if (TestHighFPSIssues._tapUpdates[0] >= num)
				{
					IL_7F:
					while (TestHighFPSIssues._tapDraws.Count > 0)
					{
						if (TestHighFPSIssues._tapDraws[0] >= num)
						{
							IL_A7:
							while (TestHighFPSIssues._tapUpdateEnds.Count > 0 && TestHighFPSIssues._tapUpdateEnds[0] < num)
							{
								TestHighFPSIssues._tapUpdateEnds.RemoveAt(0);
							}
							Main.versionNumber = string.Concat(new object[]
							{
								"total (u/d)   ",
								TestHighFPSIssues._tapUpdates.Count,
								" ",
								TestHighFPSIssues._tapUpdateEnds.Count,
								"  ",
								TestHighFPSIssues.race,
								" ",
								TestHighFPSIssues.conUH,
								" ",
								TestHighFPSIssues.conDH
							});
							Main.NewText(Main.versionNumber, byte.MaxValue, byte.MaxValue, byte.MaxValue);
							return;
						}
						TestHighFPSIssues._tapDraws.RemoveAt(0);
					}
					goto IL_A7;
				}
				TestHighFPSIssues._tapUpdates.RemoveAt(0);
			}
			goto IL_7F;
		}

		// Token: 0x0400006A RID: 106
		private static List<double> _tapUpdates = new List<double>();

		// Token: 0x0400006B RID: 107
		private static List<double> _tapUpdateEnds = new List<double>();

		// Token: 0x0400006C RID: 108
		private static List<double> _tapDraws = new List<double>();

		// Token: 0x0400006D RID: 109
		private static int conU;

		// Token: 0x0400006E RID: 110
		private static int conUH;

		// Token: 0x0400006F RID: 111
		private static int conD;

		// Token: 0x04000070 RID: 112
		private static int conDH;

		// Token: 0x04000071 RID: 113
		private static int race;
	}
}
