using System;
using System.Collections.Generic;
using Terraria.GameContent.UI;
using Terraria.Localization;

namespace Terraria.Chat.Commands
{
	// Token: 0x020005BE RID: 1470
	[ChatCommand("Emoji")]
	public class EmojiCommand : IChatCommand, ICommandAliasProvider
	{
		// Token: 0x060039E9 RID: 14825 RVA: 0x006531D1 File Offset: 0x006513D1
		public EmojiCommand()
		{
			this.Initialize();
		}

		// Token: 0x060039EA RID: 14826 RVA: 0x006531EC File Offset: 0x006513EC
		public void Initialize()
		{
			this._byName.Clear();
			for (int i = 0; i < EmoteID.Count; i++)
			{
				LocalizedText emojiName = Lang.GetEmojiName(i);
				if (emojiName != LocalizedText.Empty)
				{
					this._byName[emojiName] = i;
				}
			}
		}

		// Token: 0x060039EB RID: 14827 RVA: 0x00653230 File Offset: 0x00651430
		public void PrepareAliases(ChatCommandProcessor commandProcessor)
		{
			for (int i = 0; i < EmoteID.Count; i++)
			{
				string name = EmoteID.Search.GetName(i);
				commandProcessor.AddAlias(Language.GetText("EmojiCommand." + name), () => string.Format("{0} {1}", Language.GetTextValue("ChatCommand.Emoji_1"), Language.GetTextValue("EmojiName." + name)));
			}
		}

		// Token: 0x060039EC RID: 14828 RVA: 0x00009E06 File Offset: 0x00008006
		public void ProcessIncomingMessage(string text, byte clientId)
		{
		}

		// Token: 0x060039ED RID: 14829 RVA: 0x0065328C File Offset: 0x0065148C
		public void ProcessOutgoingMessage(ChatMessage message)
		{
			if (Main.netMode != 2 && Main.LocalPlayer.dead)
			{
				message.Consume();
				return;
			}
			int num = -1;
			if (int.TryParse(message.Text, out num))
			{
				if (num < 0 || num >= EmoteID.Count)
				{
					return;
				}
			}
			else
			{
				num = -1;
			}
			if (num == -1)
			{
				foreach (LocalizedText localizedText in this._byName.Keys)
				{
					if (localizedText.EqualsCommand(message.Text))
					{
						num = this._byName[localizedText];
						break;
					}
				}
			}
			if (num != -1)
			{
				if (Main.netMode == 0)
				{
					EmoteBubble.NewBubble(num, new WorldUIAnchor(Main.LocalPlayer), 360);
					EmoteBubble.CheckForNPCsToReactToEmoteBubble(num, Main.LocalPlayer);
				}
				else
				{
					NetMessage.SendData(120, -1, -1, null, Main.myPlayer, (float)num, 0f, 0f, 0, 0, 0);
				}
			}
			message.Consume();
		}

		// Token: 0x060039EE RID: 14830 RVA: 0x0065338C File Offset: 0x0065158C
		public void PrintWarning(string text)
		{
			throw new Exception("This needs localized text!");
		}

		// Token: 0x04005D91 RID: 23953
		public const int PlayerEmojiDuration = 360;

		// Token: 0x04005D92 RID: 23954
		private readonly Dictionary<LocalizedText, int> _byName = new Dictionary<LocalizedText, int>();
	}
}
