using System;

namespace Terraria.ID
{
	// Token: 0x02000192 RID: 402
	public static class ImmunityCooldownID
	{
		// Token: 0x040017EA RID: 6122
		public static readonly int General = -1;

		// Token: 0x040017EB RID: 6123
		public static readonly int TileContactDamage = 0;

		// Token: 0x040017EC RID: 6124
		public static readonly int BossNoCheese = 1;

		// Token: 0x040017ED RID: 6125
		public static readonly int LegacyUnused2 = 2;

		// Token: 0x040017EE RID: 6126
		public static readonly int WrongBugNet = 3;

		// Token: 0x040017EF RID: 6127
		public static readonly int Lava = 4;

		// Token: 0x040017F0 RID: 6128
		public static readonly int PaladinsShield = 5;

		// Token: 0x040017F1 RID: 6129
		public static readonly int Count = 6;

		// Token: 0x0200075B RID: 1883
		public static class Sets
		{
			// Token: 0x060040F5 RID: 16629 RVA: 0x0069DA0C File Offset: 0x0069BC0C
			public static ImmunityCooldownID.Sets.BoolSet CreateBoolSet(params int[] types)
			{
				ImmunityCooldownID.Sets.BoolSet result = new ImmunityCooldownID.Sets.BoolSet(ImmunityCooldownID.Count);
				foreach (int idx in types)
				{
					result[idx] = true;
				}
				return result;
			}

			// Token: 0x040069C9 RID: 27081
			public static ImmunityCooldownID.Sets.BoolSet Retaliate = ImmunityCooldownID.Sets.CreateBoolSet(new int[]
			{
				ImmunityCooldownID.General,
				ImmunityCooldownID.BossNoCheese,
				ImmunityCooldownID.PaladinsShield
			});

			// Token: 0x040069CA RID: 27082
			public static ImmunityCooldownID.Sets.BoolSet Counter = ImmunityCooldownID.Sets.CreateBoolSet(new int[]
			{
				ImmunityCooldownID.General,
				ImmunityCooldownID.BossNoCheese
			});

			// Token: 0x040069CB RID: 27083
			public static ImmunityCooldownID.Sets.BoolSet TeamDamageShare = ImmunityCooldownID.Sets.CreateBoolSet(new int[]
			{
				ImmunityCooldownID.General,
				ImmunityCooldownID.BossNoCheese
			});

			// Token: 0x040069CC RID: 27084
			public static ImmunityCooldownID.Sets.BoolSet ImmuneTimerOnlyLimitsEffects = ImmunityCooldownID.Sets.CreateBoolSet(new int[]
			{
				ImmunityCooldownID.PaladinsShield
			});

			// Token: 0x02000A8C RID: 2700
			public struct BoolSet
			{
				// Token: 0x170005C0 RID: 1472
				public bool this[int idx]
				{
					get
					{
						return this._arr[idx + 1];
					}
					set
					{
						this._arr[idx + 1] = value;
					}
				}

				// Token: 0x06004BC0 RID: 19392 RVA: 0x006D7EA6 File Offset: 0x006D60A6
				public BoolSet(int count)
				{
					this._arr = new bool[count + 1];
				}

				// Token: 0x04007744 RID: 30532
				private readonly bool[] _arr;
			}
		}
	}
}
