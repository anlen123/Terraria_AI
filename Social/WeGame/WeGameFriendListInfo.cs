using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using rail;

namespace Terraria.Social.WeGame
{
	// Token: 0x0200013D RID: 317
	[DataContract]
	public class WeGameFriendListInfo
	{
		// Token: 0x040015C4 RID: 5572
		[DataMember]
		public List<RailFriendInfo> _friendList;
	}
}
