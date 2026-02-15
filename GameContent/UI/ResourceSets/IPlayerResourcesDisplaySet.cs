using System;
using Terraria.DataStructures;

namespace Terraria.GameContent.UI.ResourceSets
{
	// Token: 0x020003C0 RID: 960
	public interface IPlayerResourcesDisplaySet : IConfigKeyHolder
	{
		// Token: 0x06002D0A RID: 11530
		void Draw();

		// Token: 0x06002D0B RID: 11531
		void TryToHover();
	}
}
