using System;

namespace Terraria.Testing.ChatCommands
{
	// Token: 0x0200011C RID: 284
	[Flags]
	public enum CommandRequirement
	{
		// Token: 0x04001548 RID: 5448
		SinglePlayer = 1,
		// Token: 0x04001549 RID: 5449
		MultiplayerClient = 2,
		// Token: 0x0400154A RID: 5450
		MultiplayerRPC = 4,
		// Token: 0x0400154B RID: 5451
		LocalServer = 8,
		// Token: 0x0400154C RID: 5452
		ClientAuthority = 5,
		// Token: 0x0400154D RID: 5453
		AnyAuthority = 13,
		// Token: 0x0400154E RID: 5454
		Client = 3,
		// Token: 0x0400154F RID: 5455
		All = 15
	}
}
