using System;
using System.Collections.Generic;
using Terraria.Utilities;

namespace Terraria.GameContent
{
	// Token: 0x02000245 RID: 581
	public class FlexibleTileWand
	{
		// Token: 0x060022CA RID: 8906 RVA: 0x0053983D File Offset: 0x00537A3D
		public FlexibleTileWand WithoutAmmoIcon()
		{
			this.ShowsHoverAmmoIcon = false;
			return this;
		}

		// Token: 0x060022CB RID: 8907 RVA: 0x00539847 File Offset: 0x00537A47
		public FlexibleTileWand WithoutAmmoConsumption()
		{
			this.ConsumesAmmoItem = false;
			return this;
		}

		// Token: 0x060022CC RID: 8908 RVA: 0x00539851 File Offset: 0x00537A51
		public FlexibleTileWand WithConsumingFavorites()
		{
			this.CanConsumeFavorites = true;
			return this;
		}

		// Token: 0x060022CD RID: 8909 RVA: 0x0053985C File Offset: 0x00537A5C
		public void AddVariation(int itemType, int tileIdToPlace, int tileStyleToPlace)
		{
			FlexibleTileWand.OptionBucket optionBucket;
			if (!this._options.TryGetValue(itemType, out optionBucket))
			{
				optionBucket = (this._options[itemType] = new FlexibleTileWand.OptionBucket(itemType));
			}
			optionBucket.Options.Add(new FlexibleTileWand.PlacementOption
			{
				TileIdToPlace = tileIdToPlace,
				TileStyleToPlace = tileStyleToPlace
			});
		}

		// Token: 0x060022CE RID: 8910 RVA: 0x005398B0 File Offset: 0x00537AB0
		public void AddVariations(int itemType, int tileIdToPlace, params int[] stylesToPlace)
		{
			foreach (int tileStyleToPlace in stylesToPlace)
			{
				this.AddVariation(itemType, tileIdToPlace, tileStyleToPlace);
			}
		}

		// Token: 0x060022CF RID: 8911 RVA: 0x005398D8 File Offset: 0x00537AD8
		public void AddVariationsWithOffset(int itemType, int tileIdToPlace, int offset, params int[] stylesToPlace)
		{
			for (int i = 0; i < stylesToPlace.Length; i++)
			{
				int tileStyleToPlace = offset + stylesToPlace[i];
				this.AddVariation(itemType, tileIdToPlace, tileStyleToPlace);
			}
		}

		// Token: 0x060022D0 RID: 8912 RVA: 0x00539904 File Offset: 0x00537B04
		public void AddVariations_ByRow(int itemType, int tileIdToPlace, int variationsPerRow, params int[] rows)
		{
			for (int i = 0; i < rows.Length; i++)
			{
				for (int j = 0; j < variationsPerRow; j++)
				{
					int tileStyleToPlace = rows[i] * variationsPerRow + j;
					this.AddVariation(itemType, tileIdToPlace, tileStyleToPlace);
				}
			}
		}

		// Token: 0x060022D1 RID: 8913 RVA: 0x00539940 File Offset: 0x00537B40
		public bool TryGetPlacementOption(Player player, int randomSeed, int selectCycleOffset, out FlexibleTileWand.PlacementOption option, out Item itemToConsume)
		{
			option = null;
			itemToConsume = null;
			Item[] inventory = player.inventory;
			int num = 1;
			for (int i = 0; i < 58 + num; i++)
			{
				if (i < 50 || i >= 54)
				{
					Item item = inventory[i];
					FlexibleTileWand.OptionBucket optionBucket;
					if (!item.IsAir && (this.CanConsumeFavorites || !item.favorited) && this._options.TryGetValue(item.type, out optionBucket))
					{
						this._random.SetSeed(randomSeed);
						option = optionBucket.GetOptionWithCycling(selectCycleOffset);
						itemToConsume = item;
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060022D2 RID: 8914 RVA: 0x005399C8 File Offset: 0x00537BC8
		public static FlexibleTileWand CreateRubblePlacerLarge()
		{
			FlexibleTileWand flexibleTileWand = new FlexibleTileWand();
			int tileIdToPlace = 647;
			flexibleTileWand.AddVariations(154, tileIdToPlace, new int[]
			{
				0,
				1,
				2,
				3,
				4,
				5,
				6
			});
			flexibleTileWand.AddVariations(3, tileIdToPlace, new int[]
			{
				7,
				8,
				9,
				10,
				11,
				12,
				13,
				14,
				15
			});
			flexibleTileWand.AddVariations(71, tileIdToPlace, new int[]
			{
				16,
				17
			});
			flexibleTileWand.AddVariations(72, tileIdToPlace, new int[]
			{
				18,
				19
			});
			flexibleTileWand.AddVariations(73, tileIdToPlace, new int[]
			{
				20,
				21
			});
			flexibleTileWand.AddVariations(9, tileIdToPlace, new int[]
			{
				22,
				23,
				24,
				25
			});
			flexibleTileWand.AddVariations(593, tileIdToPlace, new int[]
			{
				26,
				27,
				28,
				29,
				30,
				31
			});
			flexibleTileWand.AddVariations(183, tileIdToPlace, new int[]
			{
				32,
				33,
				34
			});
			tileIdToPlace = 648;
			flexibleTileWand.AddVariations(195, tileIdToPlace, new int[]
			{
				0,
				1,
				2
			});
			flexibleTileWand.AddVariations(195, tileIdToPlace, new int[]
			{
				3,
				4,
				5
			});
			flexibleTileWand.AddVariations(174, tileIdToPlace, new int[]
			{
				6,
				7,
				8
			});
			flexibleTileWand.AddVariation(4144, 706, 0);
			flexibleTileWand.AddVariations(150, tileIdToPlace, new int[]
			{
				9,
				10,
				11,
				12,
				13
			});
			flexibleTileWand.AddVariations(3, tileIdToPlace, new int[]
			{
				14,
				15,
				16
			});
			flexibleTileWand.AddVariations(989, tileIdToPlace, new int[]
			{
				17
			});
			flexibleTileWand.AddVariations(1101, tileIdToPlace, new int[]
			{
				18,
				19,
				20
			});
			flexibleTileWand.AddVariations(9, tileIdToPlace, new int[]
			{
				21,
				22
			});
			flexibleTileWand.AddVariations(9, tileIdToPlace, new int[]
			{
				23,
				24,
				25,
				26,
				27,
				28
			});
			flexibleTileWand.AddVariations(3271, tileIdToPlace, new int[]
			{
				29,
				30,
				31,
				32,
				33,
				34
			});
			flexibleTileWand.AddVariations(3086, tileIdToPlace, new int[]
			{
				35,
				36,
				37,
				38,
				39,
				40
			});
			flexibleTileWand.AddVariations(3081, tileIdToPlace, new int[]
			{
				41,
				42,
				43,
				44,
				45,
				46
			});
			flexibleTileWand.AddVariations(62, tileIdToPlace, new int[]
			{
				47,
				48,
				49
			});
			flexibleTileWand.AddVariations(62, tileIdToPlace, new int[]
			{
				50,
				51
			});
			flexibleTileWand.AddVariations(154, tileIdToPlace, new int[]
			{
				52,
				53,
				54
			});
			tileIdToPlace = 651;
			flexibleTileWand.AddVariations(195, tileIdToPlace, new int[]
			{
				0,
				1,
				2
			});
			flexibleTileWand.AddVariations(62, tileIdToPlace, new int[]
			{
				3,
				4,
				5
			});
			flexibleTileWand.AddVariations(331, tileIdToPlace, new int[]
			{
				6,
				7,
				8
			});
			flexibleTileWand.AddVariation(501, 704, 0);
			tileIdToPlace = 705;
			flexibleTileWand.AddVariations(276, tileIdToPlace, new int[]
			{
				0,
				1,
				2,
				3,
				4,
				5,
				6,
				7,
				8
			});
			flexibleTileWand.AddVariations(369, tileIdToPlace, new int[]
			{
				9,
				10,
				11,
				12,
				13,
				14,
				15,
				16,
				17
			});
			flexibleTileWand.AddVariations(2171, tileIdToPlace, new int[]
			{
				18,
				19,
				20,
				21,
				22,
				23,
				24,
				25,
				26
			});
			flexibleTileWand.AddVariations(59, tileIdToPlace, new int[]
			{
				27,
				28,
				29,
				30,
				31,
				32,
				33,
				34,
				35
			});
			return flexibleTileWand;
		}

		// Token: 0x060022D3 RID: 8915 RVA: 0x00539D4C File Offset: 0x00537F4C
		public static FlexibleTileWand CreateRubblePlacerMedium()
		{
			FlexibleTileWand flexibleTileWand = new FlexibleTileWand();
			ushort tileIdToPlace = 652;
			flexibleTileWand.AddVariations(195, (int)tileIdToPlace, new int[]
			{
				0,
				1,
				2
			});
			flexibleTileWand.AddVariations(62, (int)tileIdToPlace, new int[]
			{
				3,
				4,
				5
			});
			flexibleTileWand.AddVariations(331, (int)tileIdToPlace, new int[]
			{
				6,
				7,
				8,
				9,
				10,
				11
			});
			tileIdToPlace = 649;
			flexibleTileWand.AddVariations(3, (int)tileIdToPlace, new int[]
			{
				0,
				1,
				2,
				3,
				4,
				5
			});
			flexibleTileWand.AddVariations(154, (int)tileIdToPlace, new int[]
			{
				6,
				7,
				8,
				9,
				10
			});
			flexibleTileWand.AddVariations(154, (int)tileIdToPlace, new int[]
			{
				11,
				12,
				13,
				14,
				15
			});
			flexibleTileWand.AddVariations(71, (int)tileIdToPlace, new int[]
			{
				16
			});
			flexibleTileWand.AddVariations(72, (int)tileIdToPlace, new int[]
			{
				17
			});
			flexibleTileWand.AddVariations(73, (int)tileIdToPlace, new int[]
			{
				18
			});
			flexibleTileWand.AddVariations(181, (int)tileIdToPlace, new int[]
			{
				19
			});
			flexibleTileWand.AddVariations(180, (int)tileIdToPlace, new int[]
			{
				20
			});
			flexibleTileWand.AddVariations(177, (int)tileIdToPlace, new int[]
			{
				21
			});
			flexibleTileWand.AddVariations(179, (int)tileIdToPlace, new int[]
			{
				22
			});
			flexibleTileWand.AddVariations(178, (int)tileIdToPlace, new int[]
			{
				23
			});
			flexibleTileWand.AddVariations(182, (int)tileIdToPlace, new int[]
			{
				24
			});
			flexibleTileWand.AddVariations(593, (int)tileIdToPlace, new int[]
			{
				25,
				26,
				27,
				28,
				29,
				30
			});
			flexibleTileWand.AddVariations(9, (int)tileIdToPlace, new int[]
			{
				31,
				32,
				33
			});
			flexibleTileWand.AddVariations(150, (int)tileIdToPlace, new int[]
			{
				34,
				35,
				36,
				37
			});
			flexibleTileWand.AddVariations(3, (int)tileIdToPlace, new int[]
			{
				38,
				39,
				40
			});
			flexibleTileWand.AddVariations(3271, (int)tileIdToPlace, new int[]
			{
				41,
				42,
				43,
				44,
				45,
				46
			});
			flexibleTileWand.AddVariations(3086, (int)tileIdToPlace, new int[]
			{
				47,
				48,
				49,
				50,
				51,
				52
			});
			flexibleTileWand.AddVariations(3081, (int)tileIdToPlace, new int[]
			{
				53,
				54,
				55,
				56,
				57,
				58
			});
			flexibleTileWand.AddVariations(62, (int)tileIdToPlace, new int[]
			{
				59,
				60,
				61
			});
			flexibleTileWand.AddVariations(169, (int)tileIdToPlace, new int[]
			{
				62,
				63,
				65,
				66,
				67
			});
			flexibleTileWand.AddVariations(276, (int)tileIdToPlace, new int[]
			{
				64
			});
			flexibleTileWand.AddVariations(1291, 702, new int[]
			{
				0,
				1,
				2
			});
			return flexibleTileWand;
		}

		// Token: 0x060022D4 RID: 8916 RVA: 0x0053A008 File Offset: 0x00538208
		public static FlexibleTileWand CreateRubblePlacerSmall()
		{
			FlexibleTileWand flexibleTileWand = new FlexibleTileWand();
			ushort tileIdToPlace = 650;
			flexibleTileWand.AddVariations(3, (int)tileIdToPlace, new int[]
			{
				0,
				1,
				2,
				3,
				4,
				5
			});
			flexibleTileWand.AddVariations(2, (int)tileIdToPlace, new int[]
			{
				6,
				7,
				8,
				9,
				10,
				11
			});
			flexibleTileWand.AddVariations(154, (int)tileIdToPlace, new int[]
			{
				12,
				13,
				14,
				15,
				16,
				17,
				18,
				19
			});
			flexibleTileWand.AddVariations(154, (int)tileIdToPlace, new int[]
			{
				20,
				21,
				22,
				23,
				24,
				25,
				26,
				27
			});
			flexibleTileWand.AddVariations(9, (int)tileIdToPlace, new int[]
			{
				28,
				29,
				30,
				31,
				32
			});
			flexibleTileWand.AddVariations(9, (int)tileIdToPlace, new int[]
			{
				33,
				34,
				35
			});
			flexibleTileWand.AddVariations(593, (int)tileIdToPlace, new int[]
			{
				36,
				37,
				38,
				39,
				40,
				41
			});
			flexibleTileWand.AddVariations(664, (int)tileIdToPlace, new int[]
			{
				42,
				43,
				44,
				45,
				46,
				47
			});
			flexibleTileWand.AddVariations(150, (int)tileIdToPlace, new int[]
			{
				48,
				49,
				50,
				51,
				52,
				53
			});
			flexibleTileWand.AddVariations(3271, (int)tileIdToPlace, new int[]
			{
				54,
				55,
				56,
				57,
				58,
				59
			});
			flexibleTileWand.AddVariations(3086, (int)tileIdToPlace, new int[]
			{
				60,
				61,
				62,
				63,
				64,
				65
			});
			flexibleTileWand.AddVariations(3081, (int)tileIdToPlace, new int[]
			{
				66,
				67,
				68,
				69,
				70,
				71
			});
			flexibleTileWand.AddVariations(62, (int)tileIdToPlace, new int[]
			{
				72
			});
			flexibleTileWand.AddVariations(169, (int)tileIdToPlace, new int[]
			{
				73,
				74,
				76,
				78,
				79,
				80,
				81
			});
			flexibleTileWand.AddVariations(276, (int)tileIdToPlace, new int[]
			{
				75,
				77
			});
			flexibleTileWand.AddVariation(5114, 700, 0);
			flexibleTileWand.AddVariation(5333, 701, 0);
			flexibleTileWand.AddVariations(208, 703, new int[]
			{
				6,
				7
			});
			flexibleTileWand.AddVariations(331, 703, new int[]
			{
				8
			});
			flexibleTileWand.AddVariations(223, 703, new int[]
			{
				9
			});
			flexibleTileWand.AddVariation(165, 707, 5);
			return flexibleTileWand;
		}

		// Token: 0x060022D5 RID: 8917 RVA: 0x0053A243 File Offset: 0x00538443
		public static FlexibleTileWand CreateSingleTileWand(int itemIdToConsume, int TileTypeToplace, params int[] stylesToPlace)
		{
			FlexibleTileWand flexibleTileWand = new FlexibleTileWand();
			flexibleTileWand.AddVariations(itemIdToConsume, TileTypeToplace, stylesToPlace);
			return flexibleTileWand;
		}

		// Token: 0x060022D6 RID: 8918 RVA: 0x0053A254 File Offset: 0x00538454
		public static FlexibleTileWand CreateMiteyTitey()
		{
			FlexibleTileWand flexibleTileWand = new FlexibleTileWand();
			ushort tileIdToPlace = 693;
			flexibleTileWand.AddVariations(664, (int)tileIdToPlace, new int[]
			{
				0,
				1,
				2
			});
			flexibleTileWand.AddVariations(3, (int)tileIdToPlace, new int[]
			{
				3,
				4,
				5
			});
			flexibleTileWand.AddVariations(1124, (int)tileIdToPlace, new int[]
			{
				9,
				10,
				11
			});
			flexibleTileWand.AddVariations(409, (int)tileIdToPlace, new int[]
			{
				12,
				13,
				14
			});
			flexibleTileWand.AddVariations(61, (int)tileIdToPlace, new int[]
			{
				15,
				16,
				17
			});
			flexibleTileWand.AddVariations(836, (int)tileIdToPlace, new int[]
			{
				18,
				19,
				20
			});
			flexibleTileWand.AddVariations(3271, (int)tileIdToPlace, new int[]
			{
				21,
				22,
				23
			});
			flexibleTileWand.AddVariations(3086, (int)tileIdToPlace, new int[]
			{
				24,
				25,
				26
			});
			flexibleTileWand.AddVariations(3081, (int)tileIdToPlace, new int[]
			{
				27,
				28,
				29
			});
			flexibleTileWand.AddVariations(834, (int)tileIdToPlace, new int[]
			{
				30,
				31,
				32
			});
			flexibleTileWand.AddVariations(833, (int)tileIdToPlace, new int[]
			{
				33,
				34,
				35
			});
			flexibleTileWand.AddVariations(835, (int)tileIdToPlace, new int[]
			{
				36,
				37,
				38
			});
			int offset = 39;
			flexibleTileWand.AddVariationsWithOffset(3, (int)tileIdToPlace, offset, new int[]
			{
				3,
				4,
				5
			});
			flexibleTileWand.AddVariationsWithOffset(1124, (int)tileIdToPlace, offset, new int[]
			{
				9,
				10,
				11
			});
			flexibleTileWand.AddVariationsWithOffset(409, (int)tileIdToPlace, offset, new int[]
			{
				12,
				13,
				14
			});
			flexibleTileWand.AddVariationsWithOffset(61, (int)tileIdToPlace, offset, new int[]
			{
				15,
				16,
				17
			});
			flexibleTileWand.AddVariationsWithOffset(836, (int)tileIdToPlace, offset, new int[]
			{
				18,
				19,
				20
			});
			flexibleTileWand.AddVariationsWithOffset(3271, (int)tileIdToPlace, offset, new int[]
			{
				21,
				22,
				23
			});
			flexibleTileWand.AddVariationsWithOffset(3086, (int)tileIdToPlace, offset, new int[]
			{
				24,
				25,
				26
			});
			flexibleTileWand.AddVariationsWithOffset(3081, (int)tileIdToPlace, offset, new int[]
			{
				27,
				28,
				29
			});
			tileIdToPlace = 694;
			flexibleTileWand.AddVariations(664, (int)tileIdToPlace, new int[]
			{
				0,
				1,
				2
			});
			flexibleTileWand.AddVariations(3, (int)tileIdToPlace, new int[]
			{
				3,
				4,
				5
			});
			flexibleTileWand.AddVariations(150, (int)tileIdToPlace, new int[]
			{
				6,
				7,
				8
			});
			flexibleTileWand.AddVariations(409, (int)tileIdToPlace, new int[]
			{
				12,
				13,
				14
			});
			flexibleTileWand.AddVariations(61, (int)tileIdToPlace, new int[]
			{
				15,
				16,
				17
			});
			flexibleTileWand.AddVariations(836, (int)tileIdToPlace, new int[]
			{
				18,
				19,
				20
			});
			flexibleTileWand.AddVariations(3271, (int)tileIdToPlace, new int[]
			{
				21,
				22,
				23
			});
			flexibleTileWand.AddVariations(3086, (int)tileIdToPlace, new int[]
			{
				24,
				25,
				26
			});
			flexibleTileWand.AddVariations(3081, (int)tileIdToPlace, new int[]
			{
				27,
				28,
				29
			});
			flexibleTileWand.AddVariations(834, (int)tileIdToPlace, new int[]
			{
				30,
				31,
				32
			});
			flexibleTileWand.AddVariations(833, (int)tileIdToPlace, new int[]
			{
				33,
				34,
				35
			});
			flexibleTileWand.AddVariations(835, (int)tileIdToPlace, new int[]
			{
				36,
				37,
				38
			});
			flexibleTileWand.AddVariationsWithOffset(3, (int)tileIdToPlace, offset, new int[]
			{
				3,
				4,
				5
			});
			flexibleTileWand.AddVariationsWithOffset(409, (int)tileIdToPlace, offset, new int[]
			{
				12,
				13,
				14
			});
			flexibleTileWand.AddVariationsWithOffset(61, (int)tileIdToPlace, offset, new int[]
			{
				15,
				16,
				17
			});
			flexibleTileWand.AddVariationsWithOffset(836, (int)tileIdToPlace, offset, new int[]
			{
				18,
				19,
				20
			});
			flexibleTileWand.AddVariationsWithOffset(3271, (int)tileIdToPlace, offset, new int[]
			{
				21,
				22,
				23
			});
			flexibleTileWand.AddVariationsWithOffset(3086, (int)tileIdToPlace, offset, new int[]
			{
				24,
				25,
				26
			});
			flexibleTileWand.AddVariationsWithOffset(3081, (int)tileIdToPlace, offset, new int[]
			{
				27,
				28,
				29
			});
			return flexibleTileWand;
		}

		// Token: 0x060022D7 RID: 8919 RVA: 0x0053A6D0 File Offset: 0x005388D0
		public static FlexibleTileWand CreatePortableKiln()
		{
			FlexibleTileWand flexibleTileWand = new FlexibleTileWand();
			int variationsPerRow = 3;
			int tileIdToPlace = 653;
			flexibleTileWand.AddVariations_ByRow(133, tileIdToPlace, variationsPerRow, new int[]
			{
				0,
				1,
				2,
				3
			});
			flexibleTileWand.AddVariations_ByRow(664, tileIdToPlace, variationsPerRow, new int[]
			{
				4,
				5,
				6
			});
			flexibleTileWand.AddVariations_ByRow(4564, tileIdToPlace, variationsPerRow, new int[]
			{
				7,
				8,
				9
			});
			flexibleTileWand.AddVariations_ByRow(154, tileIdToPlace, variationsPerRow, new int[]
			{
				10,
				11,
				12
			});
			flexibleTileWand.AddVariations_ByRow(173, tileIdToPlace, variationsPerRow, new int[]
			{
				13,
				14,
				15
			});
			flexibleTileWand.AddVariations_ByRow(61, tileIdToPlace, variationsPerRow, new int[]
			{
				16,
				17,
				18
			});
			flexibleTileWand.AddVariations_ByRow(150, tileIdToPlace, variationsPerRow, new int[]
			{
				19,
				20,
				21
			});
			flexibleTileWand.AddVariations_ByRow(836, tileIdToPlace, variationsPerRow, new int[]
			{
				22,
				23,
				24
			});
			flexibleTileWand.AddVariations_ByRow(3272, tileIdToPlace, variationsPerRow, new int[]
			{
				25,
				26,
				27
			});
			flexibleTileWand.AddVariations_ByRow(1101, tileIdToPlace, variationsPerRow, new int[]
			{
				28,
				29,
				30
			});
			flexibleTileWand.AddVariations_ByRow(3081, tileIdToPlace, variationsPerRow, new int[]
			{
				31,
				32,
				33
			});
			flexibleTileWand.AddVariations_ByRow(3271, tileIdToPlace, variationsPerRow, new int[]
			{
				34,
				35,
				36
			});
			return flexibleTileWand;
		}

		// Token: 0x060022D9 RID: 8921 RVA: 0x0053A884 File Offset: 0x00538A84
		// Note: this type is marked as 'beforefieldinit'.
		static FlexibleTileWand()
		{
			int itemIdToConsume = 5472;
			int tileTypeToplace = 698;
			int[] array = new int[3];
			array[0] = 1;
			array[1] = 2;
			FlexibleTileWand.DeadCellsDisplayJar = FlexibleTileWand.CreateSingleTileWand(itemIdToConsume, tileTypeToplace, array).WithoutAmmoIcon().WithoutAmmoConsumption();
		}

		// Token: 0x04004CFD RID: 19709
		public static FlexibleTileWand RubblePlacementSmall = FlexibleTileWand.CreateRubblePlacerSmall();

		// Token: 0x04004CFE RID: 19710
		public static FlexibleTileWand RubblePlacementMedium = FlexibleTileWand.CreateRubblePlacerMedium();

		// Token: 0x04004CFF RID: 19711
		public static FlexibleTileWand RubblePlacementLarge = FlexibleTileWand.CreateRubblePlacerLarge();

		// Token: 0x04004D00 RID: 19712
		public static FlexibleTileWand MiteyTitey = FlexibleTileWand.CreateMiteyTitey();

		// Token: 0x04004D01 RID: 19713
		public static FlexibleTileWand SandCastleBucket = FlexibleTileWand.CreateSingleTileWand(169, 552, new int[]
		{
			0,
			1,
			2,
			3
		}).WithoutAmmoIcon();

		// Token: 0x04004D02 RID: 19714
		public static FlexibleTileWand GardenGnome = FlexibleTileWand.CreateSingleTileWand(4609, 567, new int[]
		{
			0,
			1,
			2,
			3,
			4
		}).WithoutAmmoIcon().WithoutAmmoConsumption();

		// Token: 0x04004D03 RID: 19715
		public static FlexibleTileWand Coral = FlexibleTileWand.CreateSingleTileWand(275, 81, new int[]
		{
			0,
			1,
			2,
			3,
			4,
			5
		}).WithoutAmmoIcon().WithoutAmmoConsumption();

		// Token: 0x04004D04 RID: 19716
		public static FlexibleTileWand Seashell = FlexibleTileWand.CreateSingleTileWand(2625, 324, new int[]
		{
			0,
			1,
			2
		}).WithoutAmmoIcon().WithoutAmmoConsumption();

		// Token: 0x04004D05 RID: 19717
		public static FlexibleTileWand Starfish = FlexibleTileWand.CreateSingleTileWand(2626, 324, new int[]
		{
			3,
			4,
			5
		}).WithoutAmmoIcon().WithoutAmmoConsumption();

		// Token: 0x04004D06 RID: 19718
		public static FlexibleTileWand LightningWhelkShell = FlexibleTileWand.CreateSingleTileWand(4072, 324, new int[]
		{
			6,
			7,
			8
		}).WithoutAmmoIcon().WithoutAmmoConsumption();

		// Token: 0x04004D07 RID: 19719
		public static FlexibleTileWand TulipShell = FlexibleTileWand.CreateSingleTileWand(4073, 324, new int[]
		{
			9,
			10,
			11
		}).WithoutAmmoIcon().WithoutAmmoConsumption();

		// Token: 0x04004D08 RID: 19720
		public static FlexibleTileWand JunoniaShell = FlexibleTileWand.CreateSingleTileWand(4071, 324, new int[]
		{
			12,
			13,
			14
		}).WithoutAmmoIcon().WithoutAmmoConsumption();

		// Token: 0x04004D09 RID: 19721
		public static FlexibleTileWand JackoLantern = FlexibleTileWand.CreateSingleTileWand(1813, 35, new int[]
		{
			0,
			1,
			2,
			3,
			4,
			5,
			6,
			7,
			8
		}).WithoutAmmoIcon().WithoutAmmoConsumption();

		// Token: 0x04004D0A RID: 19722
		public static FlexibleTileWand Catacomb = FlexibleTileWand.CreateSingleTileWand(1417, 241, new int[]
		{
			0,
			1,
			2,
			3,
			4,
			5,
			6,
			7,
			8
		}).WithoutAmmoIcon().WithoutAmmoConsumption();

		// Token: 0x04004D0B RID: 19723
		public static FlexibleTileWand Present = FlexibleTileWand.CreateSingleTileWand(1869, 36, new int[]
		{
			0,
			1,
			2,
			3,
			4,
			5,
			6,
			7
		}).WithoutAmmoIcon().WithoutAmmoConsumption();

		// Token: 0x04004D0C RID: 19724
		public static FlexibleTileWand PartyPresent = FlexibleTileWand.CreateSingleTileWand(3749, 457, new int[]
		{
			0,
			1,
			2,
			3,
			4
		}).WithoutAmmoIcon().WithoutAmmoConsumption();

		// Token: 0x04004D0D RID: 19725
		public static FlexibleTileWand Book = FlexibleTileWand.CreateSingleTileWand(149, 50, new int[]
		{
			0,
			1,
			2,
			3,
			4
		}).WithoutAmmoIcon().WithoutAmmoConsumption();

		// Token: 0x04004D0E RID: 19726
		public static FlexibleTileWand LawnFlamingo = FlexibleTileWand.CreateSingleTileWand(4420, 545, new int[]
		{
			0,
			1
		}).WithoutAmmoIcon().WithoutAmmoConsumption();

		// Token: 0x04004D0F RID: 19727
		public static FlexibleTileWand PortableKiln = FlexibleTileWand.CreatePortableKiln();

		// Token: 0x04004D10 RID: 19728
		public static FlexibleTileWand DeadCellsDisplayJar;

		// Token: 0x04004D11 RID: 19729
		private UnifiedRandom _random = new UnifiedRandom();

		// Token: 0x04004D12 RID: 19730
		private Dictionary<int, FlexibleTileWand.OptionBucket> _options = new Dictionary<int, FlexibleTileWand.OptionBucket>();

		// Token: 0x04004D13 RID: 19731
		public bool ConsumesAmmoItem = true;

		// Token: 0x04004D14 RID: 19732
		public bool ShowsHoverAmmoIcon = true;

		// Token: 0x04004D15 RID: 19733
		public bool CanConsumeFavorites = true;

		// Token: 0x020007CE RID: 1998
		private class OptionBucket
		{
			// Token: 0x06004226 RID: 16934 RVA: 0x006BC99F File Offset: 0x006BAB9F
			public OptionBucket(int itemTypeToConsume)
			{
				this.ItemTypeToConsume = itemTypeToConsume;
				this.Options = new List<FlexibleTileWand.PlacementOption>();
			}

			// Token: 0x06004227 RID: 16935 RVA: 0x006BC9B9 File Offset: 0x006BABB9
			public FlexibleTileWand.PlacementOption GetRandomOption(UnifiedRandom random)
			{
				return this.Options[random.Next(this.Options.Count)];
			}

			// Token: 0x06004228 RID: 16936 RVA: 0x006BC9D8 File Offset: 0x006BABD8
			public FlexibleTileWand.PlacementOption GetOptionWithCycling(int cycleOffset)
			{
				int count = this.Options.Count;
				int index = (cycleOffset % count + count) % count;
				return this.Options[index];
			}

			// Token: 0x040070CA RID: 28874
			public int ItemTypeToConsume;

			// Token: 0x040070CB RID: 28875
			public List<FlexibleTileWand.PlacementOption> Options;
		}

		// Token: 0x020007CF RID: 1999
		public class PlacementOption
		{
			// Token: 0x040070CC RID: 28876
			public int TileIdToPlace;

			// Token: 0x040070CD RID: 28877
			public int TileStyleToPlace;
		}
	}
}
