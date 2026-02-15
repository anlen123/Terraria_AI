using System;
using System.Collections.Generic;
using System.IO;
using Terraria.UI;

namespace Terraria.GameContent.Creative
{
	// Token: 0x02000326 RID: 806
	public interface ICreativePower
	{
		// Token: 0x1700039F RID: 927
		// (get) Token: 0x060027AF RID: 10159
		// (set) Token: 0x060027B0 RID: 10160
		ushort PowerId { get; set; }

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x060027B1 RID: 10161
		// (set) Token: 0x060027B2 RID: 10162
		string ServerConfigName { get; set; }

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x060027B3 RID: 10163
		// (set) Token: 0x060027B4 RID: 10164
		PowerPermissionLevel CurrentPermissionLevel { get; set; }

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x060027B5 RID: 10165
		// (set) Token: 0x060027B6 RID: 10166
		PowerPermissionLevel DefaultPermissionLevel { get; set; }

		// Token: 0x060027B7 RID: 10167
		void DeserializeNetMessage(BinaryReader reader, int userId);

		// Token: 0x060027B8 RID: 10168
		void ProvidePowerButtons(CreativePowerUIElementRequestInfo info, List<UIElement> elements);

		// Token: 0x060027B9 RID: 10169
		bool GetIsUnlocked();
	}
}
