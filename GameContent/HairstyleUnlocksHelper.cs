using System;
using System.Collections.Generic;

namespace Terraria.GameContent
{
	// Token: 0x0200024F RID: 591
	public class HairstyleUnlocksHelper
	{
		// Token: 0x06002315 RID: 8981 RVA: 0x0053BC5C File Offset: 0x00539E5C
		public void UpdateUnlocks()
		{
			if (!this.ListWarrantsRemake())
			{
				return;
			}
			this.RebuildList();
		}

		// Token: 0x06002316 RID: 8982 RVA: 0x0053BC70 File Offset: 0x00539E70
		private bool ListWarrantsRemake()
		{
			bool flag = NPC.downedMartians && !Main.gameMenu;
			bool flag2 = NPC.downedMoonlord && !Main.gameMenu;
			bool flag3 = NPC.downedPlantBoss && !Main.gameMenu;
			bool flag4 = Main.hairWindow && !Main.gameMenu;
			bool gameMenu = Main.gameMenu;
			bool result = false;
			if (this._defeatedMartians != flag || this._defeatedMoonlord != flag2 || this._defeatedPlantera != flag3 || this._isAtStylist != flag4 || this._isAtCharacterCreation != gameMenu)
			{
				result = true;
			}
			this._defeatedMartians = flag;
			this._defeatedMoonlord = flag2;
			this._defeatedPlantera = flag3;
			this._isAtStylist = flag4;
			this._isAtCharacterCreation = gameMenu;
			return result;
		}

		// Token: 0x06002317 RID: 8983 RVA: 0x0053BD2C File Offset: 0x00539F2C
		private void RebuildList()
		{
			List<int> availableHairstyles = this.AvailableHairstyles;
			availableHairstyles.Clear();
			if (this._isAtCharacterCreation || this._isAtStylist)
			{
				for (int i = 0; i < 51; i++)
				{
					availableHairstyles.Add(i);
				}
				availableHairstyles.Add(136);
				availableHairstyles.Add(137);
				availableHairstyles.Add(138);
				availableHairstyles.Add(139);
				availableHairstyles.Add(140);
				availableHairstyles.Add(141);
				availableHairstyles.Add(142);
				availableHairstyles.Add(143);
				availableHairstyles.Add(144);
				availableHairstyles.Add(147);
				availableHairstyles.Add(148);
				availableHairstyles.Add(149);
				availableHairstyles.Add(150);
				availableHairstyles.Add(151);
				availableHairstyles.Add(154);
				availableHairstyles.Add(155);
				availableHairstyles.Add(157);
				availableHairstyles.Add(158);
				availableHairstyles.Add(161);
				for (int j = 51; j < 123; j++)
				{
					availableHairstyles.Add(j);
				}
				availableHairstyles.Add(134);
				availableHairstyles.Add(135);
				availableHairstyles.Add(146);
				availableHairstyles.Add(152);
				availableHairstyles.Add(153);
				availableHairstyles.Add(156);
				availableHairstyles.Add(159);
				availableHairstyles.Add(165);
				availableHairstyles.Add(160);
				for (int k = 167; k < 228; k++)
				{
					availableHairstyles.Add(k);
				}
			}
			if (this._isAtStylist)
			{
				if (this._defeatedPlantera)
				{
					availableHairstyles.Add(162);
					availableHairstyles.Add(164);
					availableHairstyles.Add(163);
					availableHairstyles.Add(145);
				}
				if (this._defeatedMartians)
				{
					availableHairstyles.AddRange(new int[]
					{
						132,
						131,
						130,
						129,
						128,
						127,
						126,
						125,
						124,
						123
					});
				}
				if (this._defeatedMartians && this._defeatedMoonlord)
				{
					availableHairstyles.Add(133);
				}
			}
		}

		// Token: 0x04004D27 RID: 19751
		public List<int> AvailableHairstyles = new List<int>();

		// Token: 0x04004D28 RID: 19752
		private bool _defeatedMartians;

		// Token: 0x04004D29 RID: 19753
		private bool _defeatedMoonlord;

		// Token: 0x04004D2A RID: 19754
		private bool _defeatedPlantera;

		// Token: 0x04004D2B RID: 19755
		private bool _isAtStylist;

		// Token: 0x04004D2C RID: 19756
		private bool _isAtCharacterCreation;
	}
}
