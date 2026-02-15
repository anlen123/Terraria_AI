using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using NVorbis;

namespace Terraria.Audio
{
	// Token: 0x020005D6 RID: 1494
	public class OGGAudioTrack : ASoundEffectBasedAudioTrack
	{
		// Token: 0x06003AAE RID: 15022 RVA: 0x00657D56 File Offset: 0x00655F56
		public OGGAudioTrack(Stream streamToRead)
		{
			this._vorbisReader = new VorbisReader(streamToRead, true);
			this.FindLoops();
			base.CreateSoundEffect(this._vorbisReader.SampleRate, (AudioChannels)this._vorbisReader.Channels);
		}

		// Token: 0x06003AAF RID: 15023 RVA: 0x00657D8D File Offset: 0x00655F8D
		protected override void ReadAheadPutAChunkIntoTheBuffer()
		{
			this.PrepareBufferToSubmit();
			this._soundEffectInstance.SubmitBuffer(this._bufferToSubmit);
		}

		// Token: 0x06003AB0 RID: 15024 RVA: 0x00657DA8 File Offset: 0x00655FA8
		private void PrepareBufferToSubmit()
		{
			byte[] bufferToSubmit = this._bufferToSubmit;
			float[] temporaryBuffer = this._temporaryBuffer;
			VorbisReader vorbisReader = this._vorbisReader;
			int num = vorbisReader.ReadSamples(temporaryBuffer, 0, temporaryBuffer.Length);
			bool flag = this._loopEnd > 0 && vorbisReader.DecodedPosition >= (long)this._loopEnd;
			bool flag2 = num < temporaryBuffer.Length;
			if (flag || flag2)
			{
				vorbisReader.DecodedPosition = (long)this._loopStart;
				vorbisReader.ReadSamples(temporaryBuffer, num, temporaryBuffer.Length - num);
			}
			OGGAudioTrack.ApplyTemporaryBufferTo(temporaryBuffer, bufferToSubmit);
		}

		// Token: 0x06003AB1 RID: 15025 RVA: 0x00657E24 File Offset: 0x00656024
		private static void ApplyTemporaryBufferTo(float[] temporaryBuffer, byte[] samplesBuffer)
		{
			for (int i = 0; i < temporaryBuffer.Length; i++)
			{
				short num = (short)(temporaryBuffer[i] * 32767f);
				samplesBuffer[i * 2] = (byte)num;
				samplesBuffer[i * 2 + 1] = (byte)(num >> 8);
			}
		}

		// Token: 0x06003AB2 RID: 15026 RVA: 0x00657E5C File Offset: 0x0065605C
		public override void Reuse()
		{
			this._vorbisReader.SeekTo(0L, SeekOrigin.Begin);
		}

		// Token: 0x06003AB3 RID: 15027 RVA: 0x00657E6C File Offset: 0x0065606C
		private void FindLoops()
		{
			IDictionary<string, IList<string>> all = this._vorbisReader.Tags.All;
			this.TryReadingTag(all, "LOOPSTART", ref this._loopStart);
			this.TryReadingTag(all, "LOOPEND", ref this._loopEnd);
		}

		// Token: 0x06003AB4 RID: 15028 RVA: 0x00657EB0 File Offset: 0x006560B0
		private void TryReadingTag(IDictionary<string, IList<string>> tags, string entryName, ref int result)
		{
			IList<string> list;
			int num;
			if (tags.TryGetValue(entryName, out list) && list.Count > 0 && int.TryParse(list[0], out num))
			{
				result = num;
			}
		}

		// Token: 0x06003AB5 RID: 15029 RVA: 0x00657EE4 File Offset: 0x006560E4
		public override void Dispose()
		{
			this._soundEffectInstance.Dispose();
			this._vorbisReader.Dispose();
		}

		// Token: 0x04005E0F RID: 24079
		private VorbisReader _vorbisReader;

		// Token: 0x04005E10 RID: 24080
		private int _loopStart;

		// Token: 0x04005E11 RID: 24081
		private int _loopEnd;
	}
}
