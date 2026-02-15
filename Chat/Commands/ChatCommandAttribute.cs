using System;

namespace Terraria.Chat.Commands
{
	// Token: 0x020005B6 RID: 1462
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
	public sealed class ChatCommandAttribute : Attribute
	{
		// Token: 0x060039D1 RID: 14801 RVA: 0x00652DC9 File Offset: 0x00650FC9
		public ChatCommandAttribute(string name)
		{
			this.Name = name;
		}

		// Token: 0x04005D8B RID: 23947
		public readonly string Name;
	}
}
