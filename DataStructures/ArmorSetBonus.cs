using System;
using System.Collections.Generic;
using Terraria.Localization;

namespace Terraria.DataStructures
{
	// Token: 0x02000534 RID: 1332
	public class ArmorSetBonus
	{
		// Token: 0x06003724 RID: 14116 RVA: 0x0062D378 File Offset: 0x0062B578
		public int GetPart(ArmorSetBonus.PartType part)
		{
			switch (part)
			{
			case ArmorSetBonus.PartType.Head:
				return this.Head;
			case ArmorSetBonus.PartType.Body:
				return this.Body;
			case ArmorSetBonus.PartType.Legs:
				return this.Legs;
			default:
				return 0;
			}
		}

		// Token: 0x06003725 RID: 14117 RVA: 0x0062D3A8 File Offset: 0x0062B5A8
		public ArmorSetBonus.QueryResult QueryCount(ArmorSetBonus.QueryContext context)
		{
			ArmorSetBonus.QueryResult result = default(ArmorSetBonus.QueryResult);
			this.TryCounting(context.HeadItem, this.Head, ref result.ItemsFound, ref result.ItemsNeeded);
			this.TryCounting(context.BodyItem, this.Body, ref result.ItemsFound, ref result.ItemsNeeded);
			this.TryCounting(context.LegItem, this.Legs, ref result.ItemsFound, ref result.ItemsNeeded);
			return result;
		}

		// Token: 0x06003726 RID: 14118 RVA: 0x0062D41E File Offset: 0x0062B61E
		private void TryCounting(int testedItem, int neededItem, ref int foundItemCount, ref int neededItemCount)
		{
			if (neededItem == 0)
			{
				return;
			}
			neededItemCount++;
			if (testedItem == neededItem)
			{
				foundItemCount++;
			}
		}

		// Token: 0x06003727 RID: 14119 RVA: 0x0062D438 File Offset: 0x0062B638
		public string GetTooltipForSinglePiece(int itemType)
		{
			LocalizedText description = this.Description;
			if (this.PrimaryPart != ArmorSetBonus.PartType.None && this.GetPart(this.PrimaryPart) != itemType)
			{
				description = ArmorSetBonus.ItemSetBonusDecidedBy[(int)this.PrimaryPart];
			}
			return ArmorSetBonus.ItemSetBonusGeneral.FormatWith(new ArmorSetBonus.SetBonusDisplayStringSubstitutes
			{
				Description = description
			});
		}

		// Token: 0x06003728 RID: 14120 RVA: 0x0062D488 File Offset: 0x0062B688
		public string GetTooltipForWornArmor(ArmorSetBonus.QueryContext context, ArmorSetBonus.QueryResult result)
		{
			LocalizedText description = this.Description;
			if (this.PrimaryPart != ArmorSetBonus.PartType.None && context.GetPart(this.PrimaryPart) != this.GetPart(this.PrimaryPart))
			{
				description = ArmorSetBonus.ItemSetBonusDecidedBy[(int)this.PrimaryPart];
			}
			return ArmorSetBonus.ItemSetBonusEquipped.FormatWith(new ArmorSetBonus.SetBonusDisplayStringSubstitutes
			{
				Description = description,
				Numerator = result.ItemsFound,
				Denominator = result.ItemsNeeded
			});
		}

		// Token: 0x06003729 RID: 14121 RVA: 0x0062D4FA File Offset: 0x0062B6FA
		public static ArmorSetBonus.Builder Create(ArmorSetBonus.ArmorSetEffect effect, string textKey, ArmorSetBonus.PartType primaryPart = ArmorSetBonus.PartType.None)
		{
			return new ArmorSetBonus.Builder(effect, textKey, primaryPart);
		}

		// Token: 0x04005B3D RID: 23357
		public ArmorSetBonus.ArmorSetEffect Effect;

		// Token: 0x04005B3E RID: 23358
		public LocalizedText Description;

		// Token: 0x04005B3F RID: 23359
		public int Head;

		// Token: 0x04005B40 RID: 23360
		public int Body;

		// Token: 0x04005B41 RID: 23361
		public int Legs;

		// Token: 0x04005B42 RID: 23362
		public ArmorSetBonus.PartType PrimaryPart;

		// Token: 0x04005B43 RID: 23363
		private static LocalizedText ItemSetBonusEquipped = Language.GetText("UI.ItemSetBonusEquipped");

		// Token: 0x04005B44 RID: 23364
		private static LocalizedText ItemSetBonusGeneral = Language.GetText("UI.ItemSetBonusGeneral");

		// Token: 0x04005B45 RID: 23365
		private static LocalizedText[] ItemSetBonusDecidedBy = new LocalizedText[]
		{
			null,
			Language.GetText("UI.ItemSetBonusDecidedByHead"),
			Language.GetText("UI.ItemSetBonusDecidedByBody"),
			Language.GetText("UI.ItemSetBonusDecidedByLegs")
		};

		// Token: 0x020009B0 RID: 2480
		// (Invoke) Token: 0x06004A10 RID: 18960
		public delegate void ArmorSetEffect(Player player);

		// Token: 0x020009B1 RID: 2481
		public enum PartType
		{
			// Token: 0x0400767C RID: 30332
			None,
			// Token: 0x0400767D RID: 30333
			Head,
			// Token: 0x0400767E RID: 30334
			Body,
			// Token: 0x0400767F RID: 30335
			Legs
		}

		// Token: 0x020009B2 RID: 2482
		public struct QueryContext
		{
			// Token: 0x06004A13 RID: 18963 RVA: 0x006D24BF File Offset: 0x006D06BF
			public QueryContext(Player player)
			{
				this.HeadItem = ArmorSetBonus.QueryContext.TryGetType(player.armor[0]);
				this.BodyItem = ArmorSetBonus.QueryContext.TryGetType(player.armor[1]);
				this.LegItem = ArmorSetBonus.QueryContext.TryGetType(player.armor[2]);
			}

			// Token: 0x06004A14 RID: 18964 RVA: 0x006D24FA File Offset: 0x006D06FA
			private static int TryGetType(Item item)
			{
				if (item != null)
				{
					return item.type;
				}
				return 0;
			}

			// Token: 0x06004A15 RID: 18965 RVA: 0x006D2507 File Offset: 0x006D0707
			public int GetPart(ArmorSetBonus.PartType part)
			{
				switch (part)
				{
				case ArmorSetBonus.PartType.Head:
					return this.HeadItem;
				case ArmorSetBonus.PartType.Body:
					return this.BodyItem;
				case ArmorSetBonus.PartType.Legs:
					return this.LegItem;
				default:
					return 0;
				}
			}

			// Token: 0x04007680 RID: 30336
			public int HeadItem;

			// Token: 0x04007681 RID: 30337
			public int BodyItem;

			// Token: 0x04007682 RID: 30338
			public int LegItem;
		}

		// Token: 0x020009B3 RID: 2483
		public struct QueryResult
		{
			// Token: 0x1700059D RID: 1437
			// (get) Token: 0x06004A16 RID: 18966 RVA: 0x006D2535 File Offset: 0x006D0735
			public bool Complete
			{
				get
				{
					return this.ItemsNeeded == this.ItemsFound;
				}
			}

			// Token: 0x04007683 RID: 30339
			public int ItemsNeeded;

			// Token: 0x04007684 RID: 30340
			public int ItemsFound;
		}

		// Token: 0x020009B4 RID: 2484
		private class SetBonusDisplayStringSubstitutes
		{
			// Token: 0x1700059E RID: 1438
			// (get) Token: 0x06004A17 RID: 18967 RVA: 0x006D2545 File Offset: 0x006D0745
			// (set) Token: 0x06004A18 RID: 18968 RVA: 0x006D254D File Offset: 0x006D074D
			public int Numerator { get; set; }

			// Token: 0x1700059F RID: 1439
			// (get) Token: 0x06004A19 RID: 18969 RVA: 0x006D2556 File Offset: 0x006D0756
			// (set) Token: 0x06004A1A RID: 18970 RVA: 0x006D255E File Offset: 0x006D075E
			public int Denominator { get; set; }

			// Token: 0x170005A0 RID: 1440
			// (get) Token: 0x06004A1B RID: 18971 RVA: 0x006D2567 File Offset: 0x006D0767
			// (set) Token: 0x06004A1C RID: 18972 RVA: 0x006D256F File Offset: 0x006D076F
			public LocalizedText Description { get; set; }
		}

		// Token: 0x020009B5 RID: 2485
		public class Builder
		{
			// Token: 0x06004A1E RID: 18974 RVA: 0x006D2578 File Offset: 0x006D0778
			public Builder(ArmorSetBonus.ArmorSetEffect effect, string textKey, ArmorSetBonus.PartType primaryPart)
			{
				this.Effect = effect;
				this.TextKey = textKey;
				this.PrimaryPart = primaryPart;
			}

			// Token: 0x06004A1F RID: 18975 RVA: 0x006D25A0 File Offset: 0x006D07A0
			public ArmorSetBonus.Builder Set(int head, int body, int legs)
			{
				this._sets.Add(new ArmorSetBonus.Builder.Parts
				{
					Head = head,
					Body = body,
					Legs = legs
				});
				return this;
			}

			// Token: 0x06004A20 RID: 18976 RVA: 0x006D25DC File Offset: 0x006D07DC
			public ArmorSetBonus.Builder Set(int[] headOptions, int[] bodyOptions, int[] legsOptions)
			{
				if (headOptions == null)
				{
					headOptions = new int[1];
				}
				if (bodyOptions == null)
				{
					bodyOptions = new int[1];
				}
				if (legsOptions == null)
				{
					legsOptions = new int[1];
				}
				foreach (int head in headOptions)
				{
					foreach (int body in bodyOptions)
					{
						foreach (int legs in legsOptions)
						{
							this.Set(head, body, legs);
						}
					}
				}
				return this;
			}

			// Token: 0x06004A21 RID: 18977 RVA: 0x006D2664 File Offset: 0x006D0864
			public void Add()
			{
				foreach (ArmorSetBonus.Builder.Parts parts in this._sets)
				{
					ArmorSetBonuses.All.Add(new ArmorSetBonus
					{
						Effect = this.Effect,
						Description = Language.GetText(this.TextKey),
						Head = parts.Head,
						Body = parts.Body,
						Legs = parts.Legs,
						PrimaryPart = this.PrimaryPart
					});
				}
			}

			// Token: 0x04007688 RID: 30344
			private ArmorSetBonus.ArmorSetEffect Effect;

			// Token: 0x04007689 RID: 30345
			private string TextKey;

			// Token: 0x0400768A RID: 30346
			private ArmorSetBonus.PartType PrimaryPart;

			// Token: 0x0400768B RID: 30347
			private List<ArmorSetBonus.Builder.Parts> _sets = new List<ArmorSetBonus.Builder.Parts>();

			// Token: 0x02000B0E RID: 2830
			private struct Parts
			{
				// Token: 0x040078D7 RID: 30935
				public int Head;

				// Token: 0x040078D8 RID: 30936
				public int Body;

				// Token: 0x040078D9 RID: 30937
				public int Legs;
			}
		}
	}
}
