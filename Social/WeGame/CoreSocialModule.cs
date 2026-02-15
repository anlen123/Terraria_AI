using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using rail;
using ReLogic.OS;
using Terraria.Utilities;

namespace Terraria.Social.WeGame
{
	// Token: 0x02000126 RID: 294
	public class CoreSocialModule : ISocialModule
	{
		// Token: 0x06001B86 RID: 7046
		[DllImport("kernel32.dll", ExactSpelling = true)]
		private static extern uint GetCurrentThreadId();

		// Token: 0x14000031 RID: 49
		// (add) Token: 0x06001B87 RID: 7047 RVA: 0x004FA8BC File Offset: 0x004F8ABC
		// (remove) Token: 0x06001B88 RID: 7048 RVA: 0x004FA8F0 File Offset: 0x004F8AF0
		public static event Action OnTick;

		// Token: 0x06001B89 RID: 7049 RVA: 0x004FA924 File Offset: 0x004F8B24
		public void Initialize()
		{
			RailGameID railGameID = new RailGameID();
			railGameID.id_ = 2000328UL;
			string[] array;
			if (Main.dedServ)
			{
				array = Environment.GetCommandLineArgs();
			}
			else
			{
				array = new string[]
				{
					" "
				};
			}
			if (rail_api.RailNeedRestartAppForCheckingEnvironment(railGameID, array.Length, array))
			{
				Environment.Exit(1);
			}
			if (!rail_api.RailInitialize())
			{
				Environment.Exit(1);
			}
			this._callbackHelper.RegisterCallback(2, new RailEventCallBackHandler(CoreSocialModule.RailEventCallBack));
			this.isRailValid = true;
			ThreadPool.QueueUserWorkItem(new WaitCallback(this.TickThread), null);
			Main.OnTickForThirdPartySoftwareOnly += CoreSocialModule.RailEventTick;
		}

		// Token: 0x06001B8A RID: 7050 RVA: 0x004FA9C3 File Offset: 0x004F8BC3
		public static void RailEventTick()
		{
			rail_api.RailFireEvents();
			if (Monitor.TryEnter(CoreSocialModule._railTickLock))
			{
				Monitor.Pulse(CoreSocialModule._railTickLock);
				Monitor.Exit(CoreSocialModule._railTickLock);
			}
		}

		// Token: 0x06001B8B RID: 7051 RVA: 0x004FA9EA File Offset: 0x004F8BEA
		private void TickThread(object context)
		{
			Monitor.Enter(CoreSocialModule._railTickLock);
			while (this.isRailValid)
			{
				if (CoreSocialModule.OnTick != null)
				{
					CoreSocialModule.OnTick();
				}
				Monitor.Wait(CoreSocialModule._railTickLock);
			}
			Monitor.Exit(CoreSocialModule._railTickLock);
		}

		// Token: 0x06001B8C RID: 7052 RVA: 0x004FAA28 File Offset: 0x004F8C28
		public void Shutdown()
		{
			if (Platform.IsWindows)
			{
				Application.ApplicationExit += delegate(object obj, EventArgs evt)
				{
					this.isRailValid = false;
				};
			}
			else
			{
				this.isRailValid = false;
				AppDomain.CurrentDomain.ProcessExit += delegate(object obj, EventArgs evt)
				{
					this.isRailValid = false;
				};
			}
			this._callbackHelper.UnregisterAllCallback();
			rail_api.RailFinalize();
		}

		// Token: 0x06001B8D RID: 7053 RVA: 0x004FAA7C File Offset: 0x004F8C7C
		public static void RailEventCallBack(RAILEventID eventId, EventBase data)
		{
			if (eventId == 2)
			{
				CoreSocialModule.ProcessRailSystemStateChange(((RailSystemStateChanged)data).state);
			}
		}

		// Token: 0x06001B8E RID: 7054 RVA: 0x004FAA92 File Offset: 0x004F8C92
		public static void SaveAndQuitCallBack()
		{
			Main.WeGameRequireExitGame();
		}

		// Token: 0x06001B8F RID: 7055 RVA: 0x004FAA99 File Offset: 0x004F8C99
		private static void ProcessRailSystemStateChange(RailSystemState state)
		{
			if (state == 2 || state == 3)
			{
				Terraria.Utilities.MessageBox.Show("检测到WeGame异常，游戏将自动保存进度并退出游戏", "Terraria--WeGame Error", Terraria.Utilities.MessageBoxButtons.OK, Terraria.Utilities.MessageBoxIcon.Error);
				WorldGen.SaveAndQuit(new Action(CoreSocialModule.SaveAndQuitCallBack));
			}
		}

		// Token: 0x04001570 RID: 5488
		private RailCallBackHelper _callbackHelper = new RailCallBackHelper();

		// Token: 0x04001572 RID: 5490
		private static object _railTickLock = new object();

		// Token: 0x04001573 RID: 5491
		private bool isRailValid;
	}
}
