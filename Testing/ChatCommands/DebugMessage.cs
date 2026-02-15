using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria.Chat;
using Terraria.Localization;

namespace Terraria.Testing.ChatCommands
{
	// Token: 0x0200011F RID: 287
	public class DebugMessage
	{
		// Token: 0x06001B51 RID: 6993 RVA: 0x004F9E9C File Offset: 0x004F809C
		public DebugMessage(byte author, string message) : this(author, message, new Vector2((float)Main.mouseX, (float)Main.mouseY) + Main.screenPosition)
		{
		}

		// Token: 0x06001B52 RID: 6994 RVA: 0x004F9EC4 File Offset: 0x004F80C4
		private DebugMessage(byte author, string message, Vector2 mousePosition)
		{
			this.CommandName = "";
			this.Arguments = "";
			base..ctor();
			this.MousePosition = mousePosition;
			this.Author = author;
			if (message[0] != '/')
			{
				return;
			}
			string text = message.ToLower();
			int num = text.Length;
			for (int i = 0; i < text.Length; i++)
			{
				if (text[i] == ' ')
				{
					num = i;
					break;
				}
			}
			string text2 = text.Substring(1, num - 1);
			this.CommandName = text2;
			if (text2.Length == 0)
			{
				return;
			}
			if (num < message.Length - 1)
			{
				this.Arguments = message.Substring(num + 1);
			}
		}

		// Token: 0x06001B53 RID: 6995 RVA: 0x004F9F6A File Offset: 0x004F816A
		private DebugMessage(byte author, string commandName, string arguments, Vector2 mousePosition)
		{
			this.CommandName = "";
			this.Arguments = "";
			base..ctor();
			this.Author = author;
			this.CommandName = commandName;
			this.Arguments = arguments;
			this.MousePosition = mousePosition;
		}

		// Token: 0x06001B54 RID: 6996 RVA: 0x004F9FA5 File Offset: 0x004F81A5
		public void Reply(string message)
		{
			ChatHelper.DisplayMessageOnClient(NetworkText.FromLiteral(message), new Color(250, 250, 0), (int)this.Author);
		}

		// Token: 0x06001B55 RID: 6997 RVA: 0x004F9FC8 File Offset: 0x004F81C8
		public void ReplyError(string message)
		{
			ChatHelper.DisplayMessageOnClient(NetworkText.FromLiteral(message), new Color(250, 0, 0), (int)this.Author);
		}

		// Token: 0x06001B56 RID: 6998 RVA: 0x004F9FE7 File Offset: 0x004F81E7
		public void Serialize(BinaryWriter writer)
		{
			writer.Write(this.CommandName);
			writer.Write(this.Arguments);
			writer.WriteVector2(this.MousePosition);
		}

		// Token: 0x06001B57 RID: 6999 RVA: 0x004FA010 File Offset: 0x004F8210
		public static DebugMessage Deserialize(byte author, BinaryReader reader)
		{
			string commandName = reader.ReadString();
			string arguments = reader.ReadString();
			Vector2 mousePosition = reader.ReadVector2();
			return new DebugMessage(author, commandName, arguments, mousePosition);
		}

		// Token: 0x06001B58 RID: 7000 RVA: 0x004FA03B File Offset: 0x004F823B
		public DebugMessage CreateSubMessage(string newMessage)
		{
			return new DebugMessage(this.Author, newMessage, this.MousePosition);
		}

		// Token: 0x04001556 RID: 5462
		private const char COMMAND_PREFIX = '/';

		// Token: 0x04001557 RID: 5463
		public readonly byte Author;

		// Token: 0x04001558 RID: 5464
		public readonly string CommandName;

		// Token: 0x04001559 RID: 5465
		public readonly string Arguments;

		// Token: 0x0400155A RID: 5466
		public readonly Vector2 MousePosition;
	}
}
