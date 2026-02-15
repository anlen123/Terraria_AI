using System;
using System.IO;
using Microsoft.Xna.Framework.Audio;

namespace Terraria.Audio
{
	// Token: 0x020005DB RID: 1499
	public class WAVAudioTrack : ASoundEffectBasedAudioTrack
	{
		// Token: 0x06003ACF RID: 15055 RVA: 0x006583C4 File Offset: 0x006565C4
		public WAVAudioTrack(Stream stream)
		{
			this._stream = stream;
			BinaryReader binaryReader = new BinaryReader(stream);
			binaryReader.ReadInt32();
			binaryReader.ReadInt32();
			binaryReader.ReadInt32();
			AudioChannels channels = AudioChannels.Mono;
			uint sampleRate = 0U;
			bool flag = false;
			int num = 0;
			while (!flag && num < 10)
			{
				uint num2 = binaryReader.ReadUInt32();
				int chunkSize = binaryReader.ReadInt32();
				if (num2 != 544501094U)
				{
					if (num2 == 1263424842U)
					{
						WAVAudioTrack.SkipJunk(binaryReader, chunkSize);
					}
				}
				else
				{
					binaryReader.ReadInt16();
					channels = (AudioChannels)binaryReader.ReadUInt16();
					sampleRate = binaryReader.ReadUInt32();
					binaryReader.ReadInt32();
					binaryReader.ReadInt16();
					binaryReader.ReadInt16();
					flag = true;
				}
				if (!flag)
				{
					num++;
				}
			}
			binaryReader.ReadInt32();
			binaryReader.ReadInt32();
			this._streamContentStartIndex = stream.Position;
			base.CreateSoundEffect((int)sampleRate, channels);
		}

		// Token: 0x06003AD0 RID: 15056 RVA: 0x0065849C File Offset: 0x0065669C
		private static void SkipJunk(BinaryReader reader, int chunkSize)
		{
			int num = chunkSize;
			if (num % 2 != 0)
			{
				num++;
			}
			reader.ReadBytes(num);
		}

		// Token: 0x06003AD1 RID: 15057 RVA: 0x006584BC File Offset: 0x006566BC
		protected override void ReadAheadPutAChunkIntoTheBuffer()
		{
			byte[] bufferToSubmit = this._bufferToSubmit;
			if (this._stream.Read(bufferToSubmit, 0, bufferToSubmit.Length) < 1)
			{
				base.Stop(AudioStopOptions.Immediate);
				return;
			}
			this._soundEffectInstance.SubmitBuffer(this._bufferToSubmit);
		}

		// Token: 0x06003AD2 RID: 15058 RVA: 0x006584FC File Offset: 0x006566FC
		public override void Reuse()
		{
			this._stream.Position = this._streamContentStartIndex;
		}

		// Token: 0x06003AD3 RID: 15059 RVA: 0x0065850F File Offset: 0x0065670F
		public override void Dispose()
		{
			this._soundEffectInstance.Dispose();
			this._stream.Dispose();
		}

		// Token: 0x04005E1C RID: 24092
		private long _streamContentStartIndex = -1L;

		// Token: 0x04005E1D RID: 24093
		private Stream _stream;

		// Token: 0x04005E1E RID: 24094
		private const uint JUNK = 1263424842U;

		// Token: 0x04005E1F RID: 24095
		private const uint FMT = 544501094U;
	}
}
