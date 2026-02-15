using System;
using System.Reflection;

namespace Terraria.Testing.ChatCommands
{
	// Token: 0x0200011D RID: 285
	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	public sealed class DebugCommandAttribute : Attribute
	{
		// Token: 0x06001B44 RID: 6980 RVA: 0x004F9B15 File Offset: 0x004F7D15
		public DebugCommandAttribute(string name, string description, CommandRequirement requirements)
		{
			this.Name = name;
			this.Description = description;
			this.Requirements = requirements;
		}

		// Token: 0x06001B45 RID: 6981 RVA: 0x004F9B32 File Offset: 0x004F7D32
		public IDebugCommand ToDebugCommand(MethodInfo method)
		{
			return new DebugCommandAttribute.InternalDebugCommand(this, method);
		}

		// Token: 0x04001550 RID: 5456
		public readonly string Name;

		// Token: 0x04001551 RID: 5457
		public readonly string Description;

		// Token: 0x04001552 RID: 5458
		public readonly CommandRequirement Requirements;

		// Token: 0x04001553 RID: 5459
		public string HelpText;

		// Token: 0x0200072E RID: 1838
		private class InternalDebugCommand : IDebugCommand
		{
			// Token: 0x1700051A RID: 1306
			// (get) Token: 0x06004085 RID: 16517 RVA: 0x0069D2F3 File Offset: 0x0069B4F3
			// (set) Token: 0x06004086 RID: 16518 RVA: 0x0069D2FB File Offset: 0x0069B4FB
			public string Name { get; private set; }

			// Token: 0x1700051B RID: 1307
			// (get) Token: 0x06004087 RID: 16519 RVA: 0x0069D304 File Offset: 0x0069B504
			// (set) Token: 0x06004088 RID: 16520 RVA: 0x0069D30C File Offset: 0x0069B50C
			public string Description { get; private set; }

			// Token: 0x1700051C RID: 1308
			// (get) Token: 0x06004089 RID: 16521 RVA: 0x0069D315 File Offset: 0x0069B515
			// (set) Token: 0x0600408A RID: 16522 RVA: 0x0069D31D File Offset: 0x0069B51D
			public string HelpText { get; private set; }

			// Token: 0x1700051D RID: 1309
			// (get) Token: 0x0600408B RID: 16523 RVA: 0x0069D326 File Offset: 0x0069B526
			// (set) Token: 0x0600408C RID: 16524 RVA: 0x0069D32E File Offset: 0x0069B52E
			public CommandRequirement Requirements { get; private set; }

			// Token: 0x0600408D RID: 16525 RVA: 0x0069D338 File Offset: 0x0069B538
			public InternalDebugCommand(DebugCommandAttribute attribute, MethodInfo method)
			{
				this.Name = attribute.Name;
				this.Description = attribute.Description;
				this.HelpText = attribute.HelpText;
				this.Requirements = attribute.Requirements;
				this._processMethod = (DebugCommandAttribute.InternalDebugCommand.ProcessMethod)Delegate.CreateDelegate(typeof(DebugCommandAttribute.InternalDebugCommand.ProcessMethod), method);
			}

			// Token: 0x0600408E RID: 16526 RVA: 0x0069D396 File Offset: 0x0069B596
			public bool Process(DebugMessage message)
			{
				return this._processMethod(message);
			}

			// Token: 0x04006964 RID: 26980
			private readonly DebugCommandAttribute.InternalDebugCommand.ProcessMethod _processMethod;

			// Token: 0x02000A85 RID: 2693
			// (Invoke) Token: 0x06004B98 RID: 19352
			private delegate bool ProcessMethod(DebugMessage message);
		}
	}
}
