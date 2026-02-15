using System;
using System.Collections.Generic;

namespace Terraria.ID
{
	// Token: 0x020001A7 RID: 423
	public static class AmmoID
	{
		// Token: 0x0400197A RID: 6522
		public static int None = 0;

		// Token: 0x0400197B RID: 6523
		public static int Gel = 23;

		// Token: 0x0400197C RID: 6524
		public static int Arrow = 40;

		// Token: 0x0400197D RID: 6525
		public static int Coin = 71;

		// Token: 0x0400197E RID: 6526
		public static int FallenStar = 75;

		// Token: 0x0400197F RID: 6527
		public static int Bullet = 97;

		// Token: 0x04001980 RID: 6528
		public static int Sand = 169;

		// Token: 0x04001981 RID: 6529
		public static int Dart = 283;

		// Token: 0x04001982 RID: 6530
		public static int Rocket = 771;

		// Token: 0x04001983 RID: 6531
		public static int Solution = 780;

		// Token: 0x04001984 RID: 6532
		public static int Flare = 931;

		// Token: 0x04001985 RID: 6533
		public static int Snowball = 949;

		// Token: 0x04001986 RID: 6534
		public static int StyngerBolt = 1261;

		// Token: 0x04001987 RID: 6535
		public static int CandyCorn = 1783;

		// Token: 0x04001988 RID: 6536
		public static int JackOLantern = 1785;

		// Token: 0x04001989 RID: 6537
		public static int Stake = 1836;

		// Token: 0x0400198A RID: 6538
		public static int NailFriendly = 3108;

		// Token: 0x0400198B RID: 6539
		public static int Acorn = 27;

		// Token: 0x02000769 RID: 1897
		public class Sets
		{
			// Token: 0x040069E2 RID: 27106
			public static SetFactory Factory = new SetFactory((int)ItemID.Count);

			// Token: 0x040069E3 RID: 27107
			public static Dictionary<int, Dictionary<int, int>> SpecificLauncherAmmoProjectileMatches = new Dictionary<int, Dictionary<int, int>>
			{
				{
					759,
					new Dictionary<int, int>
					{
						{
							771,
							134
						},
						{
							772,
							137
						},
						{
							773,
							140
						},
						{
							774,
							143
						},
						{
							4445,
							776
						},
						{
							4446,
							780
						},
						{
							4457,
							793
						},
						{
							4458,
							796
						},
						{
							4459,
							799
						},
						{
							4447,
							784
						},
						{
							4448,
							787
						},
						{
							4449,
							790
						}
					}
				},
				{
					758,
					new Dictionary<int, int>
					{
						{
							771,
							133
						},
						{
							772,
							136
						},
						{
							773,
							139
						},
						{
							774,
							142
						},
						{
							4445,
							777
						},
						{
							4446,
							781
						},
						{
							4457,
							794
						},
						{
							4458,
							797
						},
						{
							4459,
							800
						},
						{
							4447,
							785
						},
						{
							4448,
							788
						},
						{
							4449,
							791
						}
					}
				},
				{
					760,
					new Dictionary<int, int>
					{
						{
							771,
							135
						},
						{
							772,
							138
						},
						{
							773,
							141
						},
						{
							774,
							144
						},
						{
							4445,
							778
						},
						{
							4446,
							782
						},
						{
							4457,
							795
						},
						{
							4458,
							798
						},
						{
							4459,
							801
						},
						{
							4447,
							786
						},
						{
							4448,
							789
						},
						{
							4449,
							792
						}
					}
				},
				{
					1946,
					new Dictionary<int, int>
					{
						{
							771,
							338
						},
						{
							772,
							339
						},
						{
							773,
							340
						},
						{
							774,
							341
						},
						{
							4445,
							803
						},
						{
							4446,
							804
						},
						{
							4457,
							808
						},
						{
							4458,
							809
						},
						{
							4459,
							810
						},
						{
							4447,
							805
						},
						{
							4448,
							806
						},
						{
							4449,
							807
						}
					}
				},
				{
					3930,
					new Dictionary<int, int>
					{
						{
							771,
							715
						},
						{
							772,
							716
						},
						{
							773,
							717
						},
						{
							774,
							718
						},
						{
							4445,
							717
						},
						{
							4446,
							718
						},
						{
							4457,
							717
						},
						{
							4458,
							718
						},
						{
							4459,
							717
						},
						{
							4447,
							717
						},
						{
							4448,
							717
						},
						{
							4449,
							717
						}
					}
				}
			};

			// Token: 0x040069E4 RID: 27108
			public static bool[] IsArrow = AmmoID.Sets.Factory.CreateBoolSet(new int[]
			{
				AmmoID.Arrow,
				AmmoID.Stake
			});

			// Token: 0x040069E5 RID: 27109
			public static bool[] IsBullet = AmmoID.Sets.Factory.CreateBoolSet(new int[]
			{
				AmmoID.Bullet,
				AmmoID.CandyCorn
			});

			// Token: 0x040069E6 RID: 27110
			public static bool[] IsSpecialist = AmmoID.Sets.Factory.CreateBoolSet(new int[]
			{
				AmmoID.Rocket,
				AmmoID.StyngerBolt,
				AmmoID.JackOLantern,
				AmmoID.NailFriendly,
				AmmoID.Coin,
				AmmoID.Flare,
				AmmoID.Dart,
				AmmoID.Snowball,
				AmmoID.FallenStar,
				AmmoID.Gel
			});
		}
	}
}
