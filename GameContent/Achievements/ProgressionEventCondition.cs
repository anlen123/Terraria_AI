using System;
using System.Collections.Generic;
using Terraria.Achievements;

namespace Terraria.GameContent.Achievements
{
	// Token: 0x0200028A RID: 650
	public class ProgressionEventCondition : AchievementCondition
	{
		// Token: 0x060024FB RID: 9467 RVA: 0x00552654 File Offset: 0x00550854
		private ProgressionEventCondition(int eventID) : base("PROGRESSION_EVENT_" + eventID)
		{
			this._eventIDs = new int[]
			{
				eventID
			};
			ProgressionEventCondition.ListenForPickup(this);
		}

		// Token: 0x060024FC RID: 9468 RVA: 0x00552682 File Offset: 0x00550882
		private ProgressionEventCondition(int[] eventIDs) : base("PROGRESSION_EVENT_" + eventIDs[0])
		{
			this._eventIDs = eventIDs;
			ProgressionEventCondition.ListenForPickup(this);
		}

		// Token: 0x060024FD RID: 9469 RVA: 0x005526AC File Offset: 0x005508AC
		private static void ListenForPickup(ProgressionEventCondition condition)
		{
			if (!ProgressionEventCondition._isListenerHooked)
			{
				AchievementsHelper.OnProgressionEvent += ProgressionEventCondition.ProgressionEventListener;
				ProgressionEventCondition._isListenerHooked = true;
			}
			for (int i = 0; i < condition._eventIDs.Length; i++)
			{
				if (!ProgressionEventCondition._listeners.ContainsKey(condition._eventIDs[i]))
				{
					ProgressionEventCondition._listeners[condition._eventIDs[i]] = new List<ProgressionEventCondition>();
				}
				ProgressionEventCondition._listeners[condition._eventIDs[i]].Add(condition);
			}
		}

		// Token: 0x060024FE RID: 9470 RVA: 0x00552730 File Offset: 0x00550930
		private static void ProgressionEventListener(int eventID)
		{
			if (ProgressionEventCondition._listeners.ContainsKey(eventID))
			{
				foreach (ProgressionEventCondition progressionEventCondition in ProgressionEventCondition._listeners[eventID])
				{
					progressionEventCondition.Complete();
				}
			}
		}

		// Token: 0x060024FF RID: 9471 RVA: 0x00552794 File Offset: 0x00550994
		public static ProgressionEventCondition Create(params int[] eventIDs)
		{
			return new ProgressionEventCondition(eventIDs);
		}

		// Token: 0x06002500 RID: 9472 RVA: 0x0055279C File Offset: 0x0055099C
		public static ProgressionEventCondition Create(int eventID)
		{
			return new ProgressionEventCondition(eventID);
		}

		// Token: 0x06002501 RID: 9473 RVA: 0x005527A4 File Offset: 0x005509A4
		public static ProgressionEventCondition[] CreateMany(params int[] eventIDs)
		{
			ProgressionEventCondition[] array = new ProgressionEventCondition[eventIDs.Length];
			for (int i = 0; i < eventIDs.Length; i++)
			{
				array[i] = new ProgressionEventCondition(eventIDs[i]);
			}
			return array;
		}

		// Token: 0x04004F57 RID: 20311
		private const string Identifier = "PROGRESSION_EVENT";

		// Token: 0x04004F58 RID: 20312
		private static Dictionary<int, List<ProgressionEventCondition>> _listeners = new Dictionary<int, List<ProgressionEventCondition>>();

		// Token: 0x04004F59 RID: 20313
		private static bool _isListenerHooked;

		// Token: 0x04004F5A RID: 20314
		private int[] _eventIDs;
	}
}
