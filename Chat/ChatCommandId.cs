using System;
using System.IO;
using System.Text;
using ReLogic.Utilities;
using Terraria.Chat.Commands;

namespace Terraria.Chat
{
	// Token: 0x020005B1 RID: 1457
	public struct ChatCommandId
	{
		// Token: 0x060039A7 RID: 14759 RVA: 0x006526D5 File Offset: 0x006508D5
		private ChatCommandId(string name)
		{
			this._name = name;
		}

		// Token: 0x060039A8 RID: 14760 RVA: 0x006526E0 File Offset: 0x006508E0
		public static ChatCommandId FromType<T>() where T : IChatCommand
		{
			ChatCommandAttribute cacheableAttribute = AttributeUtilities.GetCacheableAttribute<T, ChatCommandAttribute>();
			if (cacheableAttribute != null)
			{
				return new ChatCommandId(cacheableAttribute.Name);
			}
			return new ChatCommandId(null);
		}

		// Token: 0x060039A9 RID: 14761 RVA: 0x00652708 File Offset: 0x00650908
		public void Serialize(BinaryWriter writer)
		{
			writer.Write(this._name ?? "");
		}

		// Token: 0x060039AA RID: 14762 RVA: 0x0065271F File Offset: 0x0065091F
		public static ChatCommandId Deserialize(BinaryReader reader)
		{
			return new ChatCommandId(reader.ReadString());
		}

		// Token: 0x060039AB RID: 14763 RVA: 0x0065272C File Offset: 0x0065092C
		public int GetMaxSerializedSize()
		{
			return 4 + Encoding.UTF8.GetByteCount(this._name ?? "");
		}

		// Token: 0x04005D82 RID: 23938
		private readonly string _name;
	}
}
