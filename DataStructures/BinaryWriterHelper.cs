using System;
using System.IO;

namespace Terraria.DataStructures
{
	// Token: 0x02000547 RID: 1351
	public struct BinaryWriterHelper
	{
		// Token: 0x06003765 RID: 14181 RVA: 0x0062E3A9 File Offset: 0x0062C5A9
		public void ReservePointToFillLengthLaterByFilling6Bytes(BinaryWriter writer)
		{
			this._placeInWriter = writer.BaseStream.Position;
			writer.Write(0U);
			writer.Write(0);
		}

		// Token: 0x06003766 RID: 14182 RVA: 0x0062E3CC File Offset: 0x0062C5CC
		public void FillReservedPoint(BinaryWriter writer, ushort dataId)
		{
			long position = writer.BaseStream.Position;
			writer.BaseStream.Position = this._placeInWriter;
			long num = position - this._placeInWriter - 4L;
			writer.Write((int)num);
			writer.Write(dataId);
			writer.BaseStream.Position = position;
		}

		// Token: 0x06003767 RID: 14183 RVA: 0x0062E420 File Offset: 0x0062C620
		public void FillOnlyIfThereIsLengthOrRevertToSavedPosition(BinaryWriter writer, ushort dataId, out bool wroteSomething)
		{
			wroteSomething = false;
			long position = writer.BaseStream.Position;
			writer.BaseStream.Position = this._placeInWriter;
			long num = position - this._placeInWriter - 4L;
			if (num == 0L)
			{
				return;
			}
			writer.Write((int)num);
			writer.Write(dataId);
			writer.BaseStream.Position = position;
			wroteSomething = true;
		}

		// Token: 0x04005B80 RID: 23424
		private long _placeInWriter;
	}
}
