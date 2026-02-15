using System;
using Microsoft.Xna.Framework;

namespace Terraria.ID
{
	// Token: 0x020001A4 RID: 420
	public static class TorchID
	{
		// Token: 0x06001F08 RID: 7944 RVA: 0x00512C04 File Offset: 0x00510E04
		public static void Initialize()
		{
			TorchID.ITorchLightProvider[] array = new TorchID.ITorchLightProvider[(int)TorchID.Count];
			array[0] = new TorchID.ConstantTorchLight(1f, 0.95f, 0.8f);
			array[1] = new TorchID.ConstantTorchLight(0f, 0.1f, 1.3f);
			array[2] = new TorchID.ConstantTorchLight(1f, 0.1f, 0.1f);
			array[3] = new TorchID.ConstantTorchLight(0f, 1f, 0.1f);
			array[4] = new TorchID.ConstantTorchLight(0.9f, 0f, 0.9f);
			array[5] = new TorchID.ConstantTorchLight(1.4f, 1.4f, 1.4f);
			array[6] = new TorchID.ConstantTorchLight(0.9f, 0.9f, 0f);
			array[7] = default(TorchID.DemonTorchLight);
			array[8] = new TorchID.ConstantTorchLight(1f, 1.6f, 0.5f);
			array[9] = new TorchID.ConstantTorchLight(0.75f, 0.85f, 1.4f);
			array[10] = new TorchID.ConstantTorchLight(1f, 0.5f, 0f);
			array[11] = new TorchID.ConstantTorchLight(1.4f, 1.4f, 0.7f);
			array[12] = new TorchID.ConstantTorchLight(0.75f, 1.3499999f, 1.5f);
			array[13] = new TorchID.ConstantTorchLight(0.95f, 0.75f, 1.3f);
			array[14] = default(TorchID.DiscoTorchLight);
			array[15] = new TorchID.ConstantTorchLight(1f, 0f, 1f);
			array[16] = new TorchID.ConstantTorchLight(1.4f, 0.85f, 0.55f);
			array[17] = new TorchID.ConstantTorchLight(0.25f, 1.3f, 0.8f);
			array[18] = new TorchID.ConstantTorchLight(0.95f, 0.4f, 1.4f);
			array[19] = new TorchID.ConstantTorchLight(1.4f, 0.7f, 0.5f);
			array[20] = new TorchID.ConstantTorchLight(1.25f, 0.6f, 1.2f);
			array[21] = new TorchID.ConstantTorchLight(0.75f, 1.45f, 0.9f);
			array[22] = new TorchID.ConstantTorchLight(0.3f, 0.78f, 1.2f);
			array[23] = default(TorchID.ShimmerTorchLight);
			TorchID._lights = array;
		}

		// Token: 0x06001F09 RID: 7945 RVA: 0x00512EB0 File Offset: 0x005110B0
		public static void TorchColor(int torchID, out float R, out float G, out float B)
		{
			if (torchID < 0 || torchID >= TorchID._lights.Length)
			{
				R = (G = (B = 0f));
				return;
			}
			TorchID._lights[torchID].GetRGB(out R, out G, out B);
		}

		// Token: 0x04001950 RID: 6480
		public static int[] Dust = new int[]
		{
			6,
			59,
			60,
			61,
			62,
			63,
			64,
			65,
			75,
			135,
			158,
			169,
			156,
			234,
			66,
			242,
			293,
			294,
			295,
			296,
			297,
			298,
			307,
			310
		};

		// Token: 0x04001951 RID: 6481
		private static TorchID.ITorchLightProvider[] _lights;

		// Token: 0x04001952 RID: 6482
		public const short Torch = 0;

		// Token: 0x04001953 RID: 6483
		public const short Blue = 1;

		// Token: 0x04001954 RID: 6484
		public const short Red = 2;

		// Token: 0x04001955 RID: 6485
		public const short Green = 3;

		// Token: 0x04001956 RID: 6486
		public const short Purple = 4;

		// Token: 0x04001957 RID: 6487
		public const short White = 5;

		// Token: 0x04001958 RID: 6488
		public const short Yellow = 6;

		// Token: 0x04001959 RID: 6489
		public const short Demon = 7;

		// Token: 0x0400195A RID: 6490
		public const short Cursed = 8;

		// Token: 0x0400195B RID: 6491
		public const short Ice = 9;

		// Token: 0x0400195C RID: 6492
		public const short Orange = 10;

		// Token: 0x0400195D RID: 6493
		public const short Ichor = 11;

		// Token: 0x0400195E RID: 6494
		public const short UltraBright = 12;

		// Token: 0x0400195F RID: 6495
		public const short Bone = 13;

		// Token: 0x04001960 RID: 6496
		public const short Rainbow = 14;

		// Token: 0x04001961 RID: 6497
		public const short Pink = 15;

		// Token: 0x04001962 RID: 6498
		public const short Desert = 16;

		// Token: 0x04001963 RID: 6499
		public const short Coral = 17;

		// Token: 0x04001964 RID: 6500
		public const short Corrupt = 18;

		// Token: 0x04001965 RID: 6501
		public const short Crimson = 19;

		// Token: 0x04001966 RID: 6502
		public const short Hallowed = 20;

		// Token: 0x04001967 RID: 6503
		public const short Jungle = 21;

		// Token: 0x04001968 RID: 6504
		public const short Mushroom = 22;

		// Token: 0x04001969 RID: 6505
		public const short Shimmer = 23;

		// Token: 0x0400196A RID: 6506
		public static readonly short Count = 24;

		// Token: 0x02000763 RID: 1891
		public class Sets
		{
			// Token: 0x040069DD RID: 27101
			public static SetFactory Factory = new SetFactory((int)TorchID.Count);

			// Token: 0x040069DE RID: 27102
			public static bool[] IsABiomeTorch = TorchID.Sets.Factory.CreateBoolSet(false, new int[]
			{
				0,
				18,
				19,
				20,
				21,
				23,
				13,
				7,
				9,
				22,
				16
			});
		}

		// Token: 0x02000764 RID: 1892
		private interface ITorchLightProvider
		{
			// Token: 0x0600410E RID: 16654
			void GetRGB(out float r, out float g, out float b);
		}

		// Token: 0x02000765 RID: 1893
		private struct ConstantTorchLight : TorchID.ITorchLightProvider
		{
			// Token: 0x0600410F RID: 16655 RVA: 0x0069F782 File Offset: 0x0069D982
			public ConstantTorchLight(float Red, float Green, float Blue)
			{
				this.R = Red;
				this.G = Green;
				this.B = Blue;
			}

			// Token: 0x06004110 RID: 16656 RVA: 0x0069F799 File Offset: 0x0069D999
			public void GetRGB(out float r, out float g, out float b)
			{
				r = this.R;
				g = this.G;
				b = this.B;
			}

			// Token: 0x040069DF RID: 27103
			public float R;

			// Token: 0x040069E0 RID: 27104
			public float G;

			// Token: 0x040069E1 RID: 27105
			public float B;
		}

		// Token: 0x02000766 RID: 1894
		private struct DemonTorchLight : TorchID.ITorchLightProvider
		{
			// Token: 0x06004111 RID: 16657 RVA: 0x0069F7B3 File Offset: 0x0069D9B3
			public void GetRGB(out float r, out float g, out float b)
			{
				r = 0.5f * Main.demonTorch + (1f - Main.demonTorch);
				g = 0.3f;
				b = Main.demonTorch + 0.5f * (1f - Main.demonTorch);
			}
		}

		// Token: 0x02000767 RID: 1895
		private struct ShimmerTorchLight : TorchID.ITorchLightProvider
		{
			// Token: 0x06004112 RID: 16658 RVA: 0x0069F7F0 File Offset: 0x0069D9F0
			public void GetRGB(out float r, out float g, out float b)
			{
				float num = 0.9f;
				float num2 = 0.9f;
				num += (float)(270 - (int)Main.mouseTextColor) / 900f;
				num2 += (float)(270 - (int)Main.mouseTextColor) / 125f;
				num = MathHelper.Clamp(num, 0f, 1f);
				num2 = MathHelper.Clamp(num2, 0f, 1f);
				r = num * 0.9f;
				g = num2 * 0.55f;
				b = num * 1.2f;
			}
		}

		// Token: 0x02000768 RID: 1896
		private struct DiscoTorchLight : TorchID.ITorchLightProvider
		{
			// Token: 0x06004113 RID: 16659 RVA: 0x0069F870 File Offset: 0x0069DA70
			public void GetRGB(out float r, out float g, out float b)
			{
				r = (float)Main.DiscoR / 255f;
				g = (float)Main.DiscoG / 255f;
				b = (float)Main.DiscoB / 255f;
			}
		}
	}
}
