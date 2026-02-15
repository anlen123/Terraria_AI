using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace Terraria.Chat.Commands
{
	// Token: 0x020005C1 RID: 1473
	[ChatCommand("Help")]
	public class HelpCommand : IChatCommand
	{
		// Token: 0x060039F6 RID: 14838 RVA: 0x00653477 File Offset: 0x00651677
		public void ProcessIncomingMessage(string text, byte clientId)
		{
			ChatHelper.SendChatMessageToClient(HelpCommand.ComposeMessage(HelpCommand.GetCommandAliasesByID()), HelpCommand.RESPONSE_COLOR, (int)clientId);
		}

		// Token: 0x060039F7 RID: 14839 RVA: 0x00653490 File Offset: 0x00651690
		private static Dictionary<string, List<LocalizedText>> GetCommandAliasesByID()
		{
			LocalizedText[] array = Language.FindAll(Lang.CreateDialogFilter("ChatCommandDescription.", true));
			Dictionary<string, List<LocalizedText>> dictionary = new Dictionary<string, List<LocalizedText>>();
			foreach (LocalizedText localizedText in array)
			{
				string text = localizedText.Key;
				text = text.Replace("ChatCommandDescription.", "");
				int num = text.IndexOf('_');
				if (num != -1)
				{
					text = text.Substring(0, num);
				}
				List<LocalizedText> list;
				if (!dictionary.TryGetValue(text, out list))
				{
					list = new List<LocalizedText>();
					dictionary[text] = list;
				}
				list.Add(localizedText);
			}
			return dictionary;
		}

		// Token: 0x060039F8 RID: 14840 RVA: 0x00653524 File Offset: 0x00651724
		private static NetworkText ComposeMessage(Dictionary<string, List<LocalizedText>> aliases)
		{
			string text = "";
			for (int i = 0; i < aliases.Count; i++)
			{
				text = string.Concat(new object[]
				{
					text,
					"{",
					i,
					"}\n"
				});
			}
			List<NetworkText> list = new List<NetworkText>();
			foreach (KeyValuePair<string, List<LocalizedText>> keyValuePair in aliases)
			{
				list.Add(Language.GetText("ChatCommandDescription." + keyValuePair.Key).ToNetworkText());
			}
			string text2 = text;
			object[] substitutions = list.ToArray();
			return NetworkText.FromFormattable(text2, substitutions);
		}

		// Token: 0x060039F9 RID: 14841 RVA: 0x00009E06 File Offset: 0x00008006
		public void ProcessOutgoingMessage(ChatMessage message)
		{
		}

		// Token: 0x04005D94 RID: 23956
		private static readonly Color RESPONSE_COLOR = new Color(255, 240, 20);
	}
}
