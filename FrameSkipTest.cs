using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;

namespace Terraria
{
	// Token: 0x02000019 RID: 25
	public class FrameSkipTest
	{
		// Token: 0x060000CF RID: 207 RVA: 0x0000C3F8 File Offset: 0x0000A5F8
		public static void Update(GameTime gameTime)
		{
			float num = 60f;
			float num2 = 1f / num;
			float num3 = (float)gameTime.ElapsedGameTime.TotalSeconds;
			Thread.Sleep((int)MathHelper.Clamp((num2 - num3) * 1000f + 1f, 0f, 1000f));
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x0000C448 File Offset: 0x0000A648
		public static void CheckReset(GameTime gameTime)
		{
			if (FrameSkipTest.LastRecordedSecondNumber != gameTime.TotalGameTime.Seconds)
			{
				FrameSkipTest.DeltaSamples.Add(FrameSkipTest.DeltasThisSecond / FrameSkipTest.CallsThisSecond);
				if (FrameSkipTest.DeltaSamples.Count > 5)
				{
					FrameSkipTest.DeltaSamples.RemoveAt(0);
				}
				FrameSkipTest.CallsThisSecond = 0f;
				FrameSkipTest.DeltasThisSecond = 0f;
				FrameSkipTest.LastRecordedSecondNumber = gameTime.TotalGameTime.Seconds;
			}
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x0000C4BE File Offset: 0x0000A6BE
		public static void UpdateServerTest()
		{
			FrameSkipTest.serverFramerateTest.Record("frame time");
			FrameSkipTest.serverFramerateTest.StopAndPrint();
			FrameSkipTest.serverFramerateTest.Start();
		}

		// Token: 0x04000064 RID: 100
		private static int LastRecordedSecondNumber;

		// Token: 0x04000065 RID: 101
		private static float CallsThisSecond;

		// Token: 0x04000066 RID: 102
		private static float DeltasThisSecond;

		// Token: 0x04000067 RID: 103
		private static List<float> DeltaSamples = new List<float>();

		// Token: 0x04000068 RID: 104
		private const int SamplesCount = 5;

		// Token: 0x04000069 RID: 105
		private static MultiTimer serverFramerateTest = new MultiTimer(60);
	}
}
