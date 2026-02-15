using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.Chat;
using Terraria.GameContent.NetModules;
using Terraria.Localization;
using Terraria.Net;

namespace Terraria.Testing.ChatCommands
{
	// Token: 0x0200011E RID: 286
	public class DebugCommandProcessor
	{
		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06001B46 RID: 6982 RVA: 0x004F9B3B File Offset: 0x004F7D3B
		public IEnumerable<IDebugCommand> Commands
		{
			get
			{
				return this._commands.Values;
			}
		}

		// Token: 0x06001B47 RID: 6983 RVA: 0x004F9B48 File Offset: 0x004F7D48
		public DebugCommandProcessor()
		{
			if (DebugOptions.enableDebugCommands)
			{
				this.AddAttributeCommandsFromType(typeof(ToolkitDebugCommands));
			}
		}

		// Token: 0x06001B48 RID: 6984 RVA: 0x004F9B74 File Offset: 0x004F7D74
		public void AddAttributeCommandsFromType(Type type)
		{
			foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
			{
				DebugCommandAttribute attribute = AttributeUtilities.GetAttribute<DebugCommandAttribute>(methodInfo);
				if (attribute != null)
				{
					IDebugCommand debugCommand = attribute.ToDebugCommand(methodInfo);
					this.AddCommand(debugCommand);
				}
			}
		}

		// Token: 0x06001B49 RID: 6985 RVA: 0x004F9BB7 File Offset: 0x004F7DB7
		public void AddCommand(IDebugCommand debugCommand)
		{
			this._commands[debugCommand.Name.ToLower()] = debugCommand;
		}

		// Token: 0x06001B4A RID: 6986 RVA: 0x004F9BD0 File Offset: 0x004F7DD0
		public bool Process(byte playerId, string message)
		{
			return this.Process(new DebugMessage(playerId, message));
		}

		// Token: 0x06001B4B RID: 6987 RVA: 0x004F9BE0 File Offset: 0x004F7DE0
		public bool Process(DebugMessage message)
		{
			if (!DebugOptions.enableDebugCommands && !message.CommandName.Equals("toggledebugcommands"))
			{
				return false;
			}
			IDebugCommand debugCommand;
			if (!this._commands.TryGetValue(message.CommandName, out debugCommand))
			{
				return (int)message.Author == Main.myPlayer && this.TryProcessMemo(message);
			}
			if ((debugCommand.Requirements & CommandRequirement.MultiplayerRPC) != (CommandRequirement)0 && Main.netMode == 1)
			{
				NetPacket packet = NetDebugModule.Serialize(message);
				NetManager.Instance.SendToServer(packet);
				return true;
			}
			if (!DebugCommandProcessor.CanRunCommandLocally((int)message.Author, debugCommand.Requirements))
			{
				return false;
			}
			bool flag = debugCommand.Process(message);
			if (!flag && debugCommand.HelpText != null)
			{
				message.Reply(debugCommand.HelpText);
			}
			if ((DebugOptions.Shared_ReportCommandUsage || debugCommand.Name == "showdebug") && flag && Main.netMode != 0)
			{
				string arg = (message.Author == byte.MaxValue) ? "server" : Main.player[(int)message.Author].name;
				string text = string.Format("{0} debugged: /{1} {2}", arg, message.CommandName, message.Arguments);
				if (Main.netMode != 1)
				{
					ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text), new Color(250, 250, 0), -1);
				}
				else
				{
					ChatHelper.SendChatMessageFromClient(new ChatMessage(text));
				}
			}
			return true;
		}

		// Token: 0x06001B4C RID: 6988 RVA: 0x004F9D26 File Offset: 0x004F7F26
		public bool ExecuteSubMessage(DebugMessage baseMessage, string newMessage)
		{
			return this.Process(baseMessage.CreateSubMessage(newMessage));
		}

		// Token: 0x06001B4D RID: 6989 RVA: 0x004F9D38 File Offset: 0x004F7F38
		private static bool CanRunCommandLocally(int playerId, CommandRequirement requirements)
		{
			return (Main.netMode == 0 && (requirements & CommandRequirement.SinglePlayer) != (CommandRequirement)0) || (Main.netMode == 1 && (requirements & CommandRequirement.MultiplayerClient) != (CommandRequirement)0) || (Main.netMode == 2 && (requirements & CommandRequirement.LocalServer) != (CommandRequirement)0 && playerId == 255) || (Main.netMode == 2 && (requirements & CommandRequirement.MultiplayerRPC) != (CommandRequirement)0 && playerId < 255);
		}

		// Token: 0x06001B4E RID: 6990 RVA: 0x004F9D8C File Offset: 0x004F7F8C
		private bool TryProcessMemo(DebugMessage message)
		{
			string path = Path.Combine(DebugCommandProcessor.MemoCommandsPath, message.CommandName.ToLower() + ".txt");
			if (!File.Exists(path))
			{
				return false;
			}
			try
			{
				string[] array = message.Arguments.Split(new char[]
				{
					' '
				});
				foreach (string text in File.ReadAllLines(path))
				{
					string format = text;
					object[] args = array;
					this.ExecuteSubMessage(message, string.Format(format, args));
				}
			}
			catch (FormatException)
			{
				message.ReplyError("Memo formatting error. Perhaps you forgot to pass arguments?");
			}
			return true;
		}

		// Token: 0x06001B4F RID: 6991 RVA: 0x004F9E2C File Offset: 0x004F802C
		public static void OpenMemo(string name)
		{
			Utils.TryCreatingDirectory(DebugCommandProcessor.MemoCommandsPath);
			string text = Path.Combine(DebugCommandProcessor.MemoCommandsPath, name.ToLower() + ".txt");
			if (!File.Exists(text))
			{
				File.WriteAllBytes(text, new byte[0]);
			}
			System.Diagnostics.Process.Start(new ProcessStartInfo(text)
			{
				UseShellExecute = true
			});
		}

		// Token: 0x04001554 RID: 5460
		private readonly Dictionary<string, IDebugCommand> _commands = new Dictionary<string, IDebugCommand>();

		// Token: 0x04001555 RID: 5461
		private static string MemoCommandsPath = Path.Combine(Main.SavePath, "MemoCommands");
	}
}
