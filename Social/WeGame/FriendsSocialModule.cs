using System;
using rail;
using Terraria.Social.Base;

namespace Terraria.Social.WeGame
{
	// Token: 0x02000127 RID: 295
	public class FriendsSocialModule : FriendsSocialModule
	{
		// Token: 0x06001B94 RID: 7060 RVA: 0x00009E06 File Offset: 0x00008006
		public override void Initialize()
		{
		}

		// Token: 0x06001B95 RID: 7061 RVA: 0x00009E06 File Offset: 0x00008006
		public override void Shutdown()
		{
		}

		// Token: 0x06001B96 RID: 7062 RVA: 0x004FAAF0 File Offset: 0x004F8CF0
		public override string GetUsername()
		{
			string text;
			rail_api.RailFactory().RailPlayer().GetPlayerName(ref text);
			WeGameHelper.WriteDebugString("GetUsername by wegame" + text, new object[0]);
			return text;
		}

		// Token: 0x06001B97 RID: 7063 RVA: 0x004FAB26 File Offset: 0x004F8D26
		public override void OpenJoinInterface()
		{
			WeGameHelper.WriteDebugString("OpenJoinInterface by wegame", new object[0]);
			rail_api.RailFactory().RailFloatingWindow().AsyncShowRailFloatingWindow(10, "");
		}
	}
}
