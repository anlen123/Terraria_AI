using System;
using System.Collections.Generic;
using Terraria.ID;

namespace Terraria.GameContent.ItemDropRules
{
	// Token: 0x0200030A RID: 778
	public class SlimeBodyItemDropRule : IItemDropRule
	{
		// Token: 0x17000393 RID: 915
		// (get) Token: 0x060026CE RID: 9934 RVA: 0x0055F640 File Offset: 0x0055D840
		// (set) Token: 0x060026CF RID: 9935 RVA: 0x0055F648 File Offset: 0x0055D848
		public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

		// Token: 0x060026D0 RID: 9936 RVA: 0x0055F651 File Offset: 0x0055D851
		public SlimeBodyItemDropRule()
		{
			this.ChainedRules = new List<IItemDropRuleChainAttempt>();
		}

		// Token: 0x060026D1 RID: 9937 RVA: 0x0055F664 File Offset: 0x0055D864
		public bool CanDrop(DropAttemptInfo info)
		{
			return NPCID.Sets.SlimeCanContainItems[info.npc.type] && info.npc.ai[1] > 0f && info.npc.ai[1] < (float)ItemID.Count;
		}

		// Token: 0x060026D2 RID: 9938 RVA: 0x0055F6A4 File Offset: 0x0055D8A4
		public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			int itemId = (int)info.npc.ai[1];
			int minValue;
			int num;
			this.GetDropInfo(itemId, out minValue, out num);
			CommonCode.DropItemFromNPC(info.npc, itemId, info.rng.Next(minValue, num + 1), false);
			return new ItemDropAttemptResult
			{
				State = ItemDropAttemptResultState.Success
			};
		}

		// Token: 0x060026D3 RID: 9939 RVA: 0x0055F6F8 File Offset: 0x0055D8F8
		public void GetDropInfo(int itemId, out int amountDroppedMinimum, out int amountDroppedMaximum)
		{
			amountDroppedMinimum = 1;
			amountDroppedMaximum = 1;
			if (itemId <= 751)
			{
				if (itemId <= 166)
				{
					if (itemId <= 73)
					{
						switch (itemId)
						{
						case 2:
						case 3:
						case 9:
							goto IL_1B7;
						case 4:
						case 5:
						case 6:
						case 7:
						case 10:
							return;
						case 8:
							amountDroppedMinimum = 5;
							amountDroppedMaximum = 10;
							return;
						case 11:
						case 12:
						case 13:
						case 14:
							break;
						default:
							switch (itemId)
							{
							case 71:
								amountDroppedMinimum = 50;
								amountDroppedMaximum = 99;
								return;
							case 72:
								amountDroppedMinimum = 20;
								amountDroppedMaximum = 99;
								return;
							case 73:
								amountDroppedMinimum = 1;
								amountDroppedMaximum = 2;
								return;
							default:
								return;
							}
							break;
						}
					}
					else
					{
						if (itemId == 147)
						{
							goto IL_1C0;
						}
						if (itemId == 150)
						{
							goto IL_1B7;
						}
						if (itemId != 166)
						{
							return;
						}
						amountDroppedMinimum = 2;
						amountDroppedMaximum = 6;
						return;
					}
				}
				else if (itemId <= 366)
				{
					if (itemId != 174)
					{
						if (itemId == 314)
						{
							goto IL_1C0;
						}
						if (itemId - 364 > 2)
						{
							return;
						}
					}
				}
				else
				{
					if (itemId == 593)
					{
						goto IL_1B7;
					}
					if (itemId - 699 > 3)
					{
						if (itemId != 751)
						{
							return;
						}
						goto IL_1B7;
					}
				}
			}
			else if (itemId <= 3081)
			{
				if (itemId <= 1106)
				{
					if (itemId == 965)
					{
						amountDroppedMinimum = 20;
						amountDroppedMaximum = 45;
						return;
					}
					if (itemId == 1103)
					{
						goto IL_1B7;
					}
					if (itemId - 1104 > 2)
					{
						return;
					}
				}
				else
				{
					if (itemId - 1124 <= 1 || itemId == 1345)
					{
						goto IL_1C0;
					}
					if (itemId != 3081)
					{
						return;
					}
					goto IL_1B7;
				}
			}
			else if (itemId <= 3610)
			{
				if (itemId == 3086)
				{
					goto IL_1B7;
				}
				if (itemId != 3347)
				{
					if (itemId - 3609 > 1)
					{
						return;
					}
					goto IL_1B7;
				}
			}
			else
			{
				if (itemId - 3736 <= 2)
				{
					goto IL_1C0;
				}
				if (itemId - 4343 <= 1)
				{
					amountDroppedMinimum = 2;
					amountDroppedMaximum = 5;
					return;
				}
				if (itemId != 5395)
				{
					return;
				}
				goto IL_1B7;
			}
			amountDroppedMinimum = 3;
			amountDroppedMaximum = 13;
			return;
			IL_1B7:
			amountDroppedMinimum = 10;
			amountDroppedMaximum = 25;
			return;
			IL_1C0:
			amountDroppedMinimum = 2;
			amountDroppedMaximum = 5;
		}

		// Token: 0x060026D4 RID: 9940 RVA: 0x0055F8CB File Offset: 0x0055DACB
		public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
		{
			Chains.ReportDroprates(this.ChainedRules, 1f, drops, ratesInfo);
		}
	}
}
