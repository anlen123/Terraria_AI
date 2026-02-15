using System;

namespace Terraria.DataStructures
{
	// Token: 0x02000546 RID: 1350
	public static class GameDifficultyData
	{
		// Token: 0x04005B78 RID: 23416
		public static readonly GameDifficultyData.LinearCurve EnemyMaxLifeMultiplier = new GameDifficultyData.LinearCurve(new GameDifficultyData.LinearCurve.Key[]
		{
			new GameDifficultyData.LinearCurve.Key(GameDifficultyLevel.Journey, 0.5f),
			new GameDifficultyData.LinearCurve.Key(GameDifficultyLevel.Legendary, 4f)
		});

		// Token: 0x04005B79 RID: 23417
		public static readonly GameDifficultyData.LinearCurve EnemyDamageMultiplier = new GameDifficultyData.LinearCurve(new GameDifficultyData.LinearCurve.Key[]
		{
			new GameDifficultyData.LinearCurve.Key(GameDifficultyLevel.Journey, 0.5f),
			new GameDifficultyData.LinearCurve.Key(GameDifficultyLevel.Master, 3f),
			new GameDifficultyData.LinearCurve.Key(GameDifficultyLevel.Legendary, 5.3333335f)
		});

		// Token: 0x04005B7A RID: 23418
		public static readonly GameDifficultyData.LinearCurve HostileProjectileDamageMultiplier = new GameDifficultyData.LinearCurve(new GameDifficultyData.LinearCurve.Key[]
		{
			new GameDifficultyData.LinearCurve.Key(GameDifficultyLevel.Journey, 0.5f),
			new GameDifficultyData.LinearCurve.Key(GameDifficultyLevel.Master, 3f)
		});

		// Token: 0x04005B7B RID: 23419
		public static readonly GameDifficultyData.LinearCurve KnockbackToEnemiesMultiplier = new GameDifficultyData.LinearCurve(new GameDifficultyData.LinearCurve.Key[]
		{
			new GameDifficultyData.LinearCurve.Key(GameDifficultyLevel.Classic, 1f),
			new GameDifficultyData.LinearCurve.Key(GameDifficultyLevel.Master, 0.8f)
		});

		// Token: 0x04005B7C RID: 23420
		public static readonly GameDifficultyData.LinearCurve EnemyMoneyDropMultiplier = new GameDifficultyData.LinearCurve(new GameDifficultyData.LinearCurve.Key[]
		{
			new GameDifficultyData.LinearCurve.Key(GameDifficultyLevel.Classic, 1f),
			new GameDifficultyData.LinearCurve.Key(GameDifficultyLevel.Expert, 2.5f),
			new GameDifficultyData.LinearCurve.Key(GameDifficultyLevel.Master, 2.5f),
			new GameDifficultyData.LinearCurve.Key(GameDifficultyLevel.Legendary, 3.5f)
		});

		// Token: 0x04005B7D RID: 23421
		public static readonly GameDifficultyData.LinearCurve TownNPCDamageMultiplier = new GameDifficultyData.LinearCurve(new GameDifficultyData.LinearCurve.Key[]
		{
			new GameDifficultyData.LinearCurve.Key(GameDifficultyLevel.Journey, 2f),
			new GameDifficultyData.LinearCurve.Key(GameDifficultyLevel.Classic, 1f),
			new GameDifficultyData.LinearCurve.Key(GameDifficultyLevel.Expert, 1.5f),
			new GameDifficultyData.LinearCurve.Key(GameDifficultyLevel.Legendary, 2f)
		});

		// Token: 0x04005B7E RID: 23422
		public static readonly GameDifficultyData.LinearCurve DebuffTimeMultiplier = new GameDifficultyData.LinearCurve(new GameDifficultyData.LinearCurve.Key[]
		{
			new GameDifficultyData.LinearCurve.Key(GameDifficultyLevel.Classic, 1f),
			new GameDifficultyData.LinearCurve.Key(GameDifficultyLevel.Expert, 2f),
			new GameDifficultyData.LinearCurve.Key(GameDifficultyLevel.Master, 2.5f)
		});

		// Token: 0x04005B7F RID: 23423
		public static readonly GameDifficultyData.LinearCurve LightningPlayerDamageScaling = new GameDifficultyData.LinearCurve(new GameDifficultyData.LinearCurve.Key[]
		{
			new GameDifficultyData.LinearCurve.Key(GameDifficultyLevel.Journey, 0.04f),
			new GameDifficultyData.LinearCurve.Key(GameDifficultyLevel.Classic, 0.08f),
			new GameDifficultyData.LinearCurve.Key(GameDifficultyLevel.Master, 0.24f),
			new GameDifficultyData.LinearCurve.Key(GameDifficultyLevel.Legendary, 0.4f)
		});

		// Token: 0x020009B6 RID: 2486
		public struct LinearCurve
		{
			// Token: 0x06004A22 RID: 18978 RVA: 0x006D270C File Offset: 0x006D090C
			public LinearCurve(params GameDifficultyData.LinearCurve.Key[] keys)
			{
				this.keys = keys;
				for (int i = 1; i < keys.Length; i++)
				{
					float input = keys[i].input;
				}
			}

			// Token: 0x06004A23 RID: 18979 RVA: 0x006D2744 File Offset: 0x006D0944
			public float Sample(float value)
			{
				GameDifficultyData.LinearCurve.Key key = this.keys[0];
				GameDifficultyData.LinearCurve.Key key2 = key;
				for (int i = 0; i < this.keys.Length; i++)
				{
					key2 = this.keys[i];
					if (value <= key2.input)
					{
						break;
					}
					key = key2;
				}
				float num = key2.input - key.input;
				float num2 = key2.output - key.output;
				if (num == 0f)
				{
					return key.output;
				}
				return (value - key.input) * num2 / num + key.output;
			}

			// Token: 0x06004A24 RID: 18980 RVA: 0x006D27CE File Offset: 0x006D09CE
			public override string ToString()
			{
				return string.Join<GameDifficultyData.LinearCurve.Key>(", ", this.keys);
			}

			// Token: 0x0400768C RID: 30348
			public readonly GameDifficultyData.LinearCurve.Key[] keys;

			// Token: 0x02000B0F RID: 2831
			public struct Key
			{
				// Token: 0x06004DA1 RID: 19873 RVA: 0x006DB181 File Offset: 0x006D9381
				public Key(float input, float output)
				{
					this.input = input;
					this.output = output;
				}

				// Token: 0x06004DA2 RID: 19874 RVA: 0x006DB191 File Offset: 0x006D9391
				public override string ToString()
				{
					return this.input + " -> " + this.output;
				}

				// Token: 0x040078DA RID: 30938
				public readonly float input;

				// Token: 0x040078DB RID: 30939
				public readonly float output;
			}
		}
	}
}
