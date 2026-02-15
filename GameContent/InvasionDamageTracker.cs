using System;
using System.Collections.Generic;
using Terraria.Localization;

namespace Terraria.GameContent
{
	// Token: 0x0200022F RID: 559
	public class InvasionDamageTracker : NPCDamageTracker
	{
		// Token: 0x060021F3 RID: 8691 RVA: 0x00532AB9 File Offset: 0x00530CB9
		public InvasionDamageTracker(int invasionGroup, LocalizedText name = null)
		{
			this._invasionGroup = invasionGroup;
			this._name = ((name != null) ? name : Language.GetText(InvasionDamageTracker.VanillaInvasionNameKeys[invasionGroup]));
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x060021F4 RID: 8692 RVA: 0x00532AE4 File Offset: 0x00530CE4
		public override LocalizedText Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x060021F5 RID: 8693 RVA: 0x000762F3 File Offset: 0x000744F3
		public override LocalizedText KillTimeMessage
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060021F6 RID: 8694 RVA: 0x00532AEC File Offset: 0x00530CEC
		protected override bool IncludeDamageFor(NPC npc)
		{
			return NPC.GetNPCInvasionGroup(npc.type) == this._invasionGroup;
		}

		// Token: 0x060021F7 RID: 8695 RVA: 0x00532B01 File Offset: 0x00530D01
		protected override void CheckActive()
		{
			if (!this.IsActive())
			{
				base.Stop();
			}
		}

		// Token: 0x060021F8 RID: 8696 RVA: 0x00532B11 File Offset: 0x00530D11
		private bool IsActive()
		{
			if (this._invasionGroup == -2)
			{
				return Main.pumpkinMoon;
			}
			if (this._invasionGroup == -1)
			{
				return Main.snowMoon;
			}
			return Main.invasionType == this._invasionGroup;
		}

		// Token: 0x04004CA2 RID: 19618
		private static Dictionary<int, string> VanillaInvasionNameKeys = new Dictionary<int, string>
		{
			{
				1,
				"Bestiary_Invasions.Goblins"
			},
			{
				2,
				"Bestiary_Invasions.FrostLegion"
			},
			{
				3,
				"Bestiary_Invasions.Pirates"
			},
			{
				4,
				"Bestiary_Invasions.Martian"
			},
			{
				-2,
				"Bestiary_Invasions.PumpkinMoon"
			},
			{
				-1,
				"Bestiary_Invasions.FrostMoon"
			}
		};

		// Token: 0x04004CA3 RID: 19619
		private readonly int _invasionGroup;

		// Token: 0x04004CA4 RID: 19620
		private readonly LocalizedText _name;
	}
}
