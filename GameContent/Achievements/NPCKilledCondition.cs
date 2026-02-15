using System;
using System.Collections.Generic;
using Terraria.Achievements;

namespace Terraria.GameContent.Achievements
{
	// Token: 0x02000289 RID: 649
	public class NPCKilledCondition : AchievementCondition
	{
		// Token: 0x060024F3 RID: 9459 RVA: 0x005524BC File Offset: 0x005506BC
		private NPCKilledCondition(short npcId) : base("NPC_KILLED_" + npcId)
		{
			this._npcIds = new short[]
			{
				npcId
			};
			NPCKilledCondition.ListenForPickup(this);
		}

		// Token: 0x060024F4 RID: 9460 RVA: 0x005524EA File Offset: 0x005506EA
		private NPCKilledCondition(short[] npcIds) : base("NPC_KILLED_" + npcIds[0])
		{
			this._npcIds = npcIds;
			NPCKilledCondition.ListenForPickup(this);
		}

		// Token: 0x060024F5 RID: 9461 RVA: 0x00552514 File Offset: 0x00550714
		private static void ListenForPickup(NPCKilledCondition condition)
		{
			if (!NPCKilledCondition._isListenerHooked)
			{
				AchievementsHelper.OnNPCKilled += NPCKilledCondition.NPCKilledListener;
				NPCKilledCondition._isListenerHooked = true;
			}
			for (int i = 0; i < condition._npcIds.Length; i++)
			{
				if (!NPCKilledCondition._listeners.ContainsKey(condition._npcIds[i]))
				{
					NPCKilledCondition._listeners[condition._npcIds[i]] = new List<NPCKilledCondition>();
				}
				NPCKilledCondition._listeners[condition._npcIds[i]].Add(condition);
			}
		}

		// Token: 0x060024F6 RID: 9462 RVA: 0x00552598 File Offset: 0x00550798
		private static void NPCKilledListener(Player player, short npcId)
		{
			if (player.whoAmI != Main.myPlayer)
			{
				return;
			}
			if (NPCKilledCondition._listeners.ContainsKey(npcId))
			{
				foreach (NPCKilledCondition npckilledCondition in NPCKilledCondition._listeners[npcId])
				{
					npckilledCondition.Complete();
				}
			}
		}

		// Token: 0x060024F7 RID: 9463 RVA: 0x00552608 File Offset: 0x00550808
		public static AchievementCondition Create(params short[] npcIds)
		{
			return new NPCKilledCondition(npcIds);
		}

		// Token: 0x060024F8 RID: 9464 RVA: 0x00552610 File Offset: 0x00550810
		public static AchievementCondition Create(short npcId)
		{
			return new NPCKilledCondition(npcId);
		}

		// Token: 0x060024F9 RID: 9465 RVA: 0x00552618 File Offset: 0x00550818
		public static AchievementCondition[] CreateMany(params short[] npcs)
		{
			AchievementCondition[] array = new AchievementCondition[npcs.Length];
			for (int i = 0; i < npcs.Length; i++)
			{
				array[i] = new NPCKilledCondition(npcs[i]);
			}
			return array;
		}

		// Token: 0x04004F53 RID: 20307
		private const string Identifier = "NPC_KILLED";

		// Token: 0x04004F54 RID: 20308
		private static Dictionary<short, List<NPCKilledCondition>> _listeners = new Dictionary<short, List<NPCKilledCondition>>();

		// Token: 0x04004F55 RID: 20309
		private static bool _isListenerHooked;

		// Token: 0x04004F56 RID: 20310
		private short[] _npcIds;
	}
}
