using System;
using System.Collections.Generic;
using Terraria.Achievements;

namespace Terraria.GameContent.Achievements
{
	// Token: 0x0200028B RID: 651
	public class TileDestroyedCondition : AchievementCondition
	{
		// Token: 0x06002503 RID: 9475 RVA: 0x005527E0 File Offset: 0x005509E0
		private TileDestroyedCondition(ushort[] tileIds) : base("TILE_DESTROYED_" + tileIds[0])
		{
			this._tileIds = tileIds;
			TileDestroyedCondition.ListenForDestruction(this);
		}

		// Token: 0x06002504 RID: 9476 RVA: 0x00552808 File Offset: 0x00550A08
		private static void ListenForDestruction(TileDestroyedCondition condition)
		{
			if (!TileDestroyedCondition._isListenerHooked)
			{
				AchievementsHelper.OnTileDestroyed += TileDestroyedCondition.TileDestroyedListener;
				TileDestroyedCondition._isListenerHooked = true;
			}
			for (int i = 0; i < condition._tileIds.Length; i++)
			{
				if (!TileDestroyedCondition._listeners.ContainsKey(condition._tileIds[i]))
				{
					TileDestroyedCondition._listeners[condition._tileIds[i]] = new List<TileDestroyedCondition>();
				}
				TileDestroyedCondition._listeners[condition._tileIds[i]].Add(condition);
			}
		}

		// Token: 0x06002505 RID: 9477 RVA: 0x0055288C File Offset: 0x00550A8C
		private static void TileDestroyedListener(Player player, ushort tileId)
		{
			if (player.whoAmI != Main.myPlayer)
			{
				return;
			}
			if (TileDestroyedCondition._listeners.ContainsKey(tileId))
			{
				foreach (TileDestroyedCondition tileDestroyedCondition in TileDestroyedCondition._listeners[tileId])
				{
					tileDestroyedCondition.Complete();
				}
			}
		}

		// Token: 0x06002506 RID: 9478 RVA: 0x005528FC File Offset: 0x00550AFC
		public static AchievementCondition Create(params ushort[] tileIds)
		{
			return new TileDestroyedCondition(tileIds);
		}

		// Token: 0x04004F5B RID: 20315
		private const string Identifier = "TILE_DESTROYED";

		// Token: 0x04004F5C RID: 20316
		private static Dictionary<ushort, List<TileDestroyedCondition>> _listeners = new Dictionary<ushort, List<TileDestroyedCondition>>();

		// Token: 0x04004F5D RID: 20317
		private static bool _isListenerHooked;

		// Token: 0x04004F5E RID: 20318
		private ushort[] _tileIds;
	}
}
