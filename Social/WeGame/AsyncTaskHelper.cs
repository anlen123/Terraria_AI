using System;
using System.Threading.Tasks;

namespace Terraria.Social.WeGame
{
	// Token: 0x02000130 RID: 304
	public class AsyncTaskHelper
	{
		// Token: 0x06001C12 RID: 7186 RVA: 0x004FC814 File Offset: 0x004FAA14
		private AsyncTaskHelper()
		{
			this._currentThreadRunner = new CurrentThreadRunner();
		}

		// Token: 0x06001C13 RID: 7187 RVA: 0x004FC828 File Offset: 0x004FAA28
		public void RunAsyncTaskAndReply(Action task, Action replay)
		{
			Task.Factory.StartNew(delegate()
			{
				task();
				this._currentThreadRunner.Run(replay);
			});
		}

		// Token: 0x06001C14 RID: 7188 RVA: 0x004FC867 File Offset: 0x004FAA67
		public void RunAsyncTask(Action task)
		{
			Task.Factory.StartNew(task);
		}

		// Token: 0x04001597 RID: 5527
		private CurrentThreadRunner _currentThreadRunner;
	}
}
