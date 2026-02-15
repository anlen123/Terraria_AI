using System;
using System.Linq;

namespace Terraria.Testing.ChatCommands
{
	// Token: 0x0200011A RID: 282
	public static class ArgumentHelper
	{
		// Token: 0x06001B3C RID: 6972 RVA: 0x004F9A28 File Offset: 0x004F7C28
		public static ArgumentListResult ParseList(string arguments)
		{
			return new ArgumentListResult(from arg in arguments.Split(new char[]
			{
				' '
			})
			select arg.Trim() into arg
			where arg.Length != 0
			select arg);
		}
	}
}
