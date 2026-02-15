using System;
using System.IO;

namespace Terraria.IO
{
	// Token: 0x02000072 RID: 114
	public class FileMetadata
	{
		// Token: 0x060014F8 RID: 5368 RVA: 0x0000357B File Offset: 0x0000177B
		private FileMetadata()
		{
		}

		// Token: 0x060014F9 RID: 5369 RVA: 0x004BC725 File Offset: 0x004BA925
		public void Write(BinaryWriter writer)
		{
			writer.Write(27981915666277746UL | (ulong)this.Type << 56);
			writer.Write(this.Revision);
			writer.Write((ulong)((long)((this.IsFavorite.ToInt() & 1) | 0)));
		}

		// Token: 0x060014FA RID: 5370 RVA: 0x004BC763 File Offset: 0x004BA963
		public void IncrementAndWrite(BinaryWriter writer)
		{
			this.Revision += 1U;
			this.Write(writer);
		}

		// Token: 0x060014FB RID: 5371 RVA: 0x004BC77A File Offset: 0x004BA97A
		public static FileMetadata FromCurrentSettings(FileType type)
		{
			return new FileMetadata
			{
				Type = type,
				Revision = 0U,
				IsFavorite = false
			};
		}

		// Token: 0x060014FC RID: 5372 RVA: 0x004BC798 File Offset: 0x004BA998
		public static FileMetadata Read(BinaryReader reader, FileType expectedType)
		{
			FileMetadata fileMetadata = new FileMetadata();
			fileMetadata.Read(reader);
			if (fileMetadata.Type != expectedType)
			{
				throw new FormatException(string.Concat(new string[]
				{
					"Expected type \"",
					Enum.GetName(typeof(FileType), expectedType),
					"\" but found \"",
					Enum.GetName(typeof(FileType), fileMetadata.Type),
					"\"."
				}));
			}
			return fileMetadata;
		}

		// Token: 0x060014FD RID: 5373 RVA: 0x004BC81C File Offset: 0x004BAA1C
		private void Read(BinaryReader reader)
		{
			ulong num = reader.ReadUInt64();
			if ((num & 72057594037927935UL) != 27981915666277746UL)
			{
				throw new FormatException("Expected Re-Logic file format.");
			}
			byte b = (byte)(num >> 56 & 255UL);
			FileType fileType = FileType.None;
			FileType[] array = (FileType[])Enum.GetValues(typeof(FileType));
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == (FileType)b)
				{
					fileType = array[i];
					break;
				}
			}
			if (fileType == FileType.None)
			{
				throw new FormatException("Found invalid file type.");
			}
			this.Type = fileType;
			this.Revision = reader.ReadUInt32();
			ulong num2 = reader.ReadUInt64();
			this.IsFavorite = ((num2 & 1UL) == 1UL);
		}

		// Token: 0x040010A7 RID: 4263
		public const ulong MAGIC_NUMBER = 27981915666277746UL;

		// Token: 0x040010A8 RID: 4264
		public const int SIZE = 20;

		// Token: 0x040010A9 RID: 4265
		public FileType Type;

		// Token: 0x040010AA RID: 4266
		public uint Revision;

		// Token: 0x040010AB RID: 4267
		public bool IsFavorite;
	}
}
