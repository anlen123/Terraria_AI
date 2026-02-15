using System;
using System.Collections.Generic;
using ReLogic.Utilities;
using Terraria.Chat.Commands;
using Terraria.Localization;

namespace Terraria.Chat
{
	// Token: 0x020005B2 RID: 1458
	public class ChatCommandProcessor : IChatProcessor
	{
		// Token: 0x060039AC RID: 14764 RVA: 0x0065274C File Offset: 0x0065094C
		public ChatCommandProcessor AddCommand<T>() where T : IChatCommand, new()
		{
			ChatCommandAttribute cacheableAttribute = AttributeUtilities.GetCacheableAttribute<T, ChatCommandAttribute>();
			string commandKey = "ChatCommand." + cacheableAttribute.Name;
			ChatCommandId chatCommandId = ChatCommandId.FromType<T>();
			this._commands[chatCommandId] = Activator.CreateInstance<T>();
			if (Language.Exists(commandKey))
			{
				this._localizedCommands.Add(Language.GetText(commandKey), chatCommandId);
			}
			else
			{
				commandKey += "_";
				foreach (LocalizedText key2 in Language.FindAll((string key, LocalizedText text) => key.StartsWith(commandKey)))
				{
					this._localizedCommands.Add(key2, chatCommandId);
				}
			}
			return this;
		}

		// Token: 0x060039AD RID: 14765 RVA: 0x0065280C File Offset: 0x00650A0C
		public void AddAlias(LocalizedText alias, Func<string> result)
		{
			this._aliases[alias] = result;
		}

		// Token: 0x060039AE RID: 14766 RVA: 0x0065281C File Offset: 0x00650A1C
		public void PrepareAliases()
		{
			foreach (IChatCommand chatCommand in this._commands.Values)
			{
				if (chatCommand is ICommandAliasProvider)
				{
					((ICommandAliasProvider)chatCommand).PrepareAliases(this);
				}
			}
		}

		// Token: 0x060039AF RID: 14767 RVA: 0x00652884 File Offset: 0x00650A84
		public ChatCommandProcessor AddDefaultCommand<T>() where T : IChatCommand, new()
		{
			this.AddCommand<T>();
			ChatCommandId key = ChatCommandId.FromType<T>();
			this._defaultCommand = this._commands[key];
			return this;
		}

		// Token: 0x060039B0 RID: 14768 RVA: 0x006528B4 File Offset: 0x00650AB4
		private static bool ParseCommandPrefix<T>(string text, Dictionary<LocalizedText, T> commands, out string remainder, out T value)
		{
			foreach (KeyValuePair<LocalizedText, T> keyValuePair in commands)
			{
				if (keyValuePair.Key.ParseCommandPrefix(text, out remainder))
				{
					value = keyValuePair.Value;
					return true;
				}
			}
			remainder = "";
			value = default(T);
			return false;
		}

		// Token: 0x060039B1 RID: 14769 RVA: 0x00652930 File Offset: 0x00650B30
		public ChatMessage CreateOutgoingMessage(string text)
		{
			ChatMessage chatMessage = new ChatMessage(text);
			string text2;
			ChatCommandId chatCommandId;
			if (ChatCommandProcessor.ParseCommandPrefix<ChatCommandId>(chatMessage.Text, this._localizedCommands, out text2, out chatCommandId))
			{
				chatMessage.Text = text2;
				chatMessage.SetCommand(chatCommandId);
				this._commands[chatCommandId].ProcessOutgoingMessage(chatMessage);
				return chatMessage;
			}
			Func<string> func;
			if (ChatCommandProcessor.ParseCommandPrefix<Func<string>>(chatMessage.Text, this._aliases, out text2, out func))
			{
				return this.CreateOutgoingMessage(func());
			}
			return chatMessage;
		}

		// Token: 0x060039B2 RID: 14770 RVA: 0x006529A4 File Offset: 0x00650BA4
		public void ProcessIncomingMessage(ChatMessage message, int clientId)
		{
			IChatCommand chatCommand;
			if (this._commands.TryGetValue(message.CommandId, out chatCommand))
			{
				chatCommand.ProcessIncomingMessage(message.Text, (byte)clientId);
				message.Consume();
				return;
			}
			if (this._defaultCommand != null)
			{
				this._defaultCommand.ProcessIncomingMessage(message.Text, (byte)clientId);
				message.Consume();
			}
		}

		// Token: 0x04005D83 RID: 23939
		private readonly Dictionary<LocalizedText, ChatCommandId> _localizedCommands = new Dictionary<LocalizedText, ChatCommandId>();

		// Token: 0x04005D84 RID: 23940
		private readonly Dictionary<ChatCommandId, IChatCommand> _commands = new Dictionary<ChatCommandId, IChatCommand>();

		// Token: 0x04005D85 RID: 23941
		private Dictionary<LocalizedText, Func<string>> _aliases = new Dictionary<LocalizedText, Func<string>>();

		// Token: 0x04005D86 RID: 23942
		private IChatCommand _defaultCommand;
	}
}
