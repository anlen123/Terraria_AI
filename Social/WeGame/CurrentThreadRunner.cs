using System;
using System.Windows.Threading;

namespace Terraria.Social.WeGame
{
	// Token: 0x0200012F RID: 303
	public class CurrentThreadRunner
	{
		// Token: 0x06001C10 RID: 7184 RVA: 0x004FC7EC File Offset: 0x004FA9EC
		public CurrentThreadRunner()
		{
			this._dsipatcher = Dispatcher.CurrentDispatcher;
		}

		// Token: 0x06001C11 RID: 7185 RVA: 0x004FC7FF File Offset: 0x004FA9FF
		public void Run(Action f)
		{
			this._dsipatcher.BeginInvoke(f, new object[0]);
		}

		// Token: 0x04001596 RID: 5526
		private Dispatcher _dsipatcher;
	}
}
