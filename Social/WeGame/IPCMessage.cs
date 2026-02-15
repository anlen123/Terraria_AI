using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Terraria.Social.WeGame
{
	// Token: 0x02000135 RID: 309
	public class IPCMessage
	{
		// Token: 0x06001C46 RID: 7238 RVA: 0x004FD1BA File Offset: 0x004FB3BA
		public void Build<T>(IPCMessageType cmd, T t)
		{
			this._jsonData = WeGameHelper.Serialize<T>(t);
			this._cmd = cmd;
		}

		// Token: 0x06001C47 RID: 7239 RVA: 0x004FD1D0 File Offset: 0x004FB3D0
		public void BuildFrom(byte[] data)
		{
			byte[] value = data.Take(4).ToArray<byte>();
			byte[] bytes = data.Skip(4).ToArray<byte>();
			this._cmd = (IPCMessageType)BitConverter.ToInt32(value, 0);
			this._jsonData = Encoding.UTF8.GetString(bytes);
		}

		// Token: 0x06001C48 RID: 7240 RVA: 0x004FD215 File Offset: 0x004FB415
		public void Parse<T>(out T value)
		{
			WeGameHelper.UnSerialize<T>(this._jsonData, out value);
		}

		// Token: 0x06001C49 RID: 7241 RVA: 0x004FD224 File Offset: 0x004FB424
		public byte[] GetBytes()
		{
			List<byte> list = new List<byte>();
			byte[] bytes = BitConverter.GetBytes((int)this._cmd);
			list.AddRange(bytes);
			list.AddRange(Encoding.UTF8.GetBytes(this._jsonData));
			return list.ToArray();
		}

		// Token: 0x06001C4A RID: 7242 RVA: 0x004FD264 File Offset: 0x004FB464
		public IPCMessageType GetCmd()
		{
			return this._cmd;
		}

		// Token: 0x040015A9 RID: 5545
		private IPCMessageType _cmd;

		// Token: 0x040015AA RID: 5546
		private string _jsonData;
	}
}
