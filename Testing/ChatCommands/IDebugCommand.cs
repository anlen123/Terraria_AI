using System;

namespace Terraria.Testing.ChatCommands
{
	// Token: 0x02000120 RID: 288
	public interface IDebugCommand
	{
		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06001B59 RID: 7001
		string Name { get; }

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06001B5A RID: 7002
		string Description { get; }

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06001B5B RID: 7003
		string HelpText { get; }

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06001B5C RID: 7004
		CommandRequirement Requirements { get; }

		// Token: 0x06001B5D RID: 7005
		bool Process(DebugMessage message);
	}
}
