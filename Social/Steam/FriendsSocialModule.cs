using System;
using Steamworks;
using Terraria.Social.Base;

namespace Terraria.Social.Steam
{
	// Token: 0x02000147 RID: 327
	public class FriendsSocialModule : FriendsSocialModule
	{
		// Token: 0x06001CBE RID: 7358 RVA: 0x00009E06 File Offset: 0x00008006
		public override void Initialize()
		{
		}

		// Token: 0x06001CBF RID: 7359 RVA: 0x00009E06 File Offset: 0x00008006
		public override void Shutdown()
		{
		}

		// Token: 0x06001CC0 RID: 7360 RVA: 0x004FECA5 File Offset: 0x004FCEA5
		public override string GetUsername()
		{
			return SteamFriends.GetPersonaName();
		}

		// Token: 0x06001CC1 RID: 7361 RVA: 0x004FECAC File Offset: 0x004FCEAC
		public override void OpenJoinInterface()
		{
			SteamFriends.ActivateGameOverlay("Friends");
		}
	}
}
