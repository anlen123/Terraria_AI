using System;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using XPT.Core.Audio.MP3Sharp;

namespace Terraria.Audio
{
	// Token: 0x020005D4 RID: 1492
	public class MP3AudioTrack : ASoundEffectBasedAudioTrack
	{
		// Token: 0x06003A9E RID: 15006 RVA: 0x00657AB8 File Offset: 0x00655CB8
		public MP3AudioTrack(Stream stream)
		{
			this._stream = stream;
			MP3Stream mp3Stream = new MP3Stream(stream);
			int frequency = mp3Stream.Frequency;
			this._mp3Stream = mp3Stream;
			base.CreateSoundEffect(frequency, AudioChannels.Stereo);
		}

		// Token: 0x06003A9F RID: 15007 RVA: 0x00657AEF File Offset: 0x00655CEF
		public override void Reuse()
		{
			this._mp3Stream.Position = 0L;
		}

		// Token: 0x06003AA0 RID: 15008 RVA: 0x00657AFE File Offset: 0x00655CFE
		public override void Dispose()
		{
			this._soundEffectInstance.Dispose();
			this._mp3Stream.Dispose();
			this._stream.Dispose();
		}

		// Token: 0x06003AA1 RID: 15009 RVA: 0x00657B24 File Offset: 0x00655D24
		protected override void ReadAheadPutAChunkIntoTheBuffer()
		{
			byte[] bufferToSubmit = this._bufferToSubmit;
			if (this._mp3Stream.Read(bufferToSubmit, 0, bufferToSubmit.Length) < 1)
			{
				base.Stop(AudioStopOptions.Immediate);
				return;
			}
			this._soundEffectInstance.SubmitBuffer(this._bufferToSubmit);
		}

		// Token: 0x04005E09 RID: 24073
		private Stream _stream;

		// Token: 0x04005E0A RID: 24074
		private MP3Stream _mp3Stream;
	}
}
