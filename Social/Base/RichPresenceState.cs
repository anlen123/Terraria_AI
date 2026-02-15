using System;
using Terraria.GameContent.UI.States;

namespace Terraria.Social.Base
{
	// Token: 0x02000150 RID: 336
	public class RichPresenceState : IEquatable<RichPresenceState>
	{
		// Token: 0x06001D0F RID: 7439 RVA: 0x0050037C File Offset: 0x004FE57C
		public bool Equals(RichPresenceState other)
		{
			return this.GameMode == other.GameMode;
		}

		// Token: 0x06001D10 RID: 7440 RVA: 0x00500390 File Offset: 0x004FE590
		public static RichPresenceState GetCurrentState()
		{
			RichPresenceState richPresenceState = new RichPresenceState();
			if (Main.gameMenu)
			{
				bool flag = Main.MenuUI.CurrentState is UICharacterCreation;
				bool flag2 = Main.MenuUI.CurrentState is UIWorldCreation;
				if (flag)
				{
					richPresenceState.GameMode = RichPresenceState.GameModeState.CreatingPlayer;
				}
				else if (flag2)
				{
					richPresenceState.GameMode = RichPresenceState.GameModeState.CreatingWorld;
				}
				else
				{
					richPresenceState.GameMode = RichPresenceState.GameModeState.InMainMenu;
				}
			}
			else if (Main.netMode == 0)
			{
				richPresenceState.GameMode = RichPresenceState.GameModeState.PlayingSingle;
			}
			else
			{
				richPresenceState.GameMode = RichPresenceState.GameModeState.PlayingMulti;
			}
			return richPresenceState;
		}

		// Token: 0x0400160F RID: 5647
		public RichPresenceState.GameModeState GameMode;

		// Token: 0x02000742 RID: 1858
		public enum GameModeState
		{
			// Token: 0x0400698B RID: 27019
			InMainMenu,
			// Token: 0x0400698C RID: 27020
			CreatingPlayer,
			// Token: 0x0400698D RID: 27021
			CreatingWorld,
			// Token: 0x0400698E RID: 27022
			PlayingSingle,
			// Token: 0x0400698F RID: 27023
			PlayingMulti
		}
	}
}
