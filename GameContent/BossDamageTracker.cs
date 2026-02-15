using System;
using Terraria.Localization;

namespace Terraria.GameContent
{
	// Token: 0x0200022E RID: 558
	public class BossDamageTracker : NPCDamageTracker
	{
		// Token: 0x060021EC RID: 8684 RVA: 0x00532941 File Offset: 0x00530B41
		public BossDamageTracker(int type, NPCDamageTracker.CustomDefinition definition)
		{
			if (definition != null && definition.NPCTypes != null)
			{
				type = definition.NPCTypes[0];
			}
			this._type = type;
			this._overrides = definition;
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x060021ED RID: 8685 RVA: 0x00532970 File Offset: 0x00530B70
		public override LocalizedText Name
		{
			get
			{
				if (this._overrides == null || this._overrides.Name == null)
				{
					return Lang.GetNPCName(this._type);
				}
				return this._overrides.Name;
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x060021EE RID: 8686 RVA: 0x0053299E File Offset: 0x00530B9E
		public override LocalizedText KillTimeMessage
		{
			get
			{
				return Language.GetText(this._killed ? "BossDamageCommand.KillTime" : "BossDamageCommand.KillTimeEscaped");
			}
		}

		// Token: 0x060021EF RID: 8687 RVA: 0x005329BC File Offset: 0x00530BBC
		protected override bool IncludeDamageFor(NPC npc)
		{
			if (NPCDamageTracker.BossTypeForMob[npc.type] == this._type)
			{
				return true;
			}
			if (this._overrides == null || this._overrides.NPCTypes == null)
			{
				return npc.type == this._type;
			}
			return this._overrides.NPCTypes.Contains(npc.type);
		}

		// Token: 0x060021F0 RID: 8688 RVA: 0x00532A19 File Offset: 0x00530C19
		protected override void CheckActive()
		{
			if (!this.IsActive())
			{
				base.Stop();
			}
		}

		// Token: 0x060021F1 RID: 8689 RVA: 0x00532A2C File Offset: 0x00530C2C
		private bool IsActive()
		{
			if (this._overrides != null && this._overrides.NPCTypes != null)
			{
				foreach (int num in this._overrides.NPCTypes)
				{
					if (NPC.npcsFoundForCheckActive[num])
					{
						return true;
					}
				}
				return false;
			}
			return NPC.npcsFoundForCheckActive[this._type];
		}

		// Token: 0x060021F2 RID: 8690 RVA: 0x00532AB0 File Offset: 0x00530CB0
		protected override void OnBossKilled(NPC npc)
		{
			this._killed = true;
		}

		// Token: 0x04004C9F RID: 19615
		private readonly int _type;

		// Token: 0x04004CA0 RID: 19616
		private readonly NPCDamageTracker.CustomDefinition _overrides;

		// Token: 0x04004CA1 RID: 19617
		private bool _killed;
	}
}
