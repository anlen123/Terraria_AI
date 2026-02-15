using System;
using System.IO;

namespace Terraria.Net
{
	// Token: 0x0200016E RID: 366
	public abstract class NetModule
	{
		// Token: 0x06001DBF RID: 7615
		public abstract bool Deserialize(BinaryReader reader, int userId);

		// Token: 0x06001DC0 RID: 7616 RVA: 0x00501624 File Offset: 0x004FF824
		protected static NetPacket CreatePacket<T>(int maxSize = 65530) where T : NetModule
		{
			ushort id = NetManager.Instance.GetId<T>();
			return new NetPacket(id, maxSize);
		}
	}
}
