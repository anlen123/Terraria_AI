using System;
using System.IO;
using System.Text;
using Terraria.Chat.Commands;

namespace Terraria.Chat
{
	// Token: 0x020005B4 RID: 1460
	public sealed class ChatMessage
	{
		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x060039C1 RID: 14785 RVA: 0x00652C79 File Offset: 0x00650E79
		// (set) Token: 0x060039C2 RID: 14786 RVA: 0x00652C81 File Offset: 0x00650E81
		public ChatCommandId CommandId { get; private set; }

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x060039C3 RID: 14787 RVA: 0x00652C8A File Offset: 0x00650E8A
		// (set) Token: 0x060039C4 RID: 14788 RVA: 0x00652C92 File Offset: 0x00650E92
		public string Text { get; set; }

		// Token: 0x170004A1 RID: 1185
		// (get) Token: 0x060039C5 RID: 14789 RVA: 0x00652C9B File Offset: 0x00650E9B
		// (set) Token: 0x060039C6 RID: 14790 RVA: 0x00652CA3 File Offset: 0x00650EA3
		public bool IsConsumed { get; private set; }

		// Token: 0x060039C7 RID: 14791 RVA: 0x00652CAC File Offset: 0x00650EAC
		public ChatMessage(string message)
		{
			this.CommandId = ChatCommandId.FromType<SayChatCommand>();
			this.Text = message;
			this.IsConsumed = false;
		}

		// Token: 0x060039C8 RID: 14792 RVA: 0x00652CCD File Offset: 0x00650ECD
		private ChatMessage(string message, ChatCommandId commandId)
		{
			this.CommandId = commandId;
			this.Text = message;
		}

		// Token: 0x060039C9 RID: 14793 RVA: 0x00652CE4 File Offset: 0x00650EE4
		public void Serialize(BinaryWriter writer)
		{
			if (this.IsConsumed)
			{
				throw new InvalidOperationException("Message has already been consumed.");
			}
			this.CommandId.Serialize(writer);
			writer.Write(this.Text);
		}

		// Token: 0x060039CA RID: 14794 RVA: 0x00652D20 File Offset: 0x00650F20
		public int GetMaxSerializedSize()
		{
			if (this.IsConsumed)
			{
				throw new InvalidOperationException("Message has already been consumed.");
			}
			return 0 + this.CommandId.GetMaxSerializedSize() + (4 + Encoding.UTF8.GetByteCount(this.Text));
		}

		// Token: 0x060039CB RID: 14795 RVA: 0x00652D64 File Offset: 0x00650F64
		public static ChatMessage Deserialize(BinaryReader reader)
		{
			ChatCommandId commandId = ChatCommandId.Deserialize(reader);
			return new ChatMessage(reader.ReadString(), commandId);
		}

		// Token: 0x060039CC RID: 14796 RVA: 0x00652D84 File Offset: 0x00650F84
		public void SetCommand(ChatCommandId commandId)
		{
			if (this.IsConsumed)
			{
				throw new InvalidOperationException("Message has already been consumed.");
			}
			this.CommandId = commandId;
		}

		// Token: 0x060039CD RID: 14797 RVA: 0x00652DA0 File Offset: 0x00650FA0
		public void SetCommand<T>() where T : IChatCommand
		{
			if (this.IsConsumed)
			{
				throw new InvalidOperationException("Message has already been consumed.");
			}
			this.CommandId = ChatCommandId.FromType<T>();
		}

		// Token: 0x060039CE RID: 14798 RVA: 0x00652DC0 File Offset: 0x00650FC0
		public void Consume()
		{
			this.IsConsumed = true;
		}
	}
}
