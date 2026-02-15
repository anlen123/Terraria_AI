using System;
using Microsoft.Xna.Framework.Audio;

namespace Terraria.Audio
{
	// Token: 0x020005CA RID: 1482
	public abstract class ASoundEffectBasedAudioTrack : IAudioTrack, IDisposable
	{
		// Token: 0x06003A20 RID: 14880 RVA: 0x00653C20 File Offset: 0x00651E20
		public ASoundEffectBasedAudioTrack()
		{
		}

		// Token: 0x06003A21 RID: 14881 RVA: 0x00653C48 File Offset: 0x00651E48
		protected void CreateSoundEffect(int sampleRate, AudioChannels channels)
		{
			this._sampleRate = sampleRate;
			this._channels = channels;
			this._soundEffectInstance = new DynamicSoundEffectInstance(this._sampleRate, this._channels);
		}

		// Token: 0x06003A22 RID: 14882 RVA: 0x00653C6F File Offset: 0x00651E6F
		private void _soundEffectInstance_BufferNeeded(object sender, EventArgs e)
		{
			this.PrepareBuffer();
		}

		// Token: 0x06003A23 RID: 14883 RVA: 0x00653C77 File Offset: 0x00651E77
		public void Update()
		{
			if (!this.IsPlaying || this._soundEffectInstance.PendingBufferCount >= 8)
			{
				return;
			}
			this.PrepareBuffer();
		}

		// Token: 0x06003A24 RID: 14884 RVA: 0x00653C98 File Offset: 0x00651E98
		protected void ResetBuffer()
		{
			for (int i = 0; i < this._bufferToSubmit.Length; i++)
			{
				this._bufferToSubmit[i] = 0;
			}
		}

		// Token: 0x06003A25 RID: 14885 RVA: 0x00653CC4 File Offset: 0x00651EC4
		protected void PrepareBuffer()
		{
			for (int i = 0; i < 2; i++)
			{
				this.ReadAheadPutAChunkIntoTheBuffer();
			}
		}

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x06003A26 RID: 14886 RVA: 0x00653CE3 File Offset: 0x00651EE3
		public bool IsPlaying
		{
			get
			{
				return this._soundEffectInstance.State == SoundState.Playing;
			}
		}

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x06003A27 RID: 14887 RVA: 0x00653CF3 File Offset: 0x00651EF3
		public bool IsStopped
		{
			get
			{
				return this._soundEffectInstance.State == SoundState.Stopped;
			}
		}

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x06003A28 RID: 14888 RVA: 0x00653D03 File Offset: 0x00651F03
		public bool IsPaused
		{
			get
			{
				return this._soundEffectInstance.State == SoundState.Paused;
			}
		}

		// Token: 0x06003A29 RID: 14889 RVA: 0x00653D13 File Offset: 0x00651F13
		public void Stop(AudioStopOptions options)
		{
			this._soundEffectInstance.Stop(options == AudioStopOptions.Immediate);
		}

		// Token: 0x06003A2A RID: 14890 RVA: 0x00653D24 File Offset: 0x00651F24
		public void Play()
		{
			this.PrepareToPlay();
			this._soundEffectInstance.Play();
		}

		// Token: 0x06003A2B RID: 14891 RVA: 0x00653D37 File Offset: 0x00651F37
		public void Pause()
		{
			this._soundEffectInstance.Pause();
		}

		// Token: 0x06003A2C RID: 14892 RVA: 0x00653D44 File Offset: 0x00651F44
		public void SetVariable(string variableName, float value)
		{
			if (variableName == "Volume")
			{
				float volume = this.ReMapVolumeToMatchXact(value);
				this._soundEffectInstance.Volume = volume;
				return;
			}
			if (variableName == "Pitch")
			{
				this._soundEffectInstance.Pitch = value;
				return;
			}
			if (!(variableName == "Pan"))
			{
				return;
			}
			this._soundEffectInstance.Pan = value;
		}

		// Token: 0x06003A2D RID: 14893 RVA: 0x00653DA8 File Offset: 0x00651FA8
		private float ReMapVolumeToMatchXact(float musicVolume)
		{
			double num = 31.0 * (double)musicVolume - 25.0 - 11.94;
			return (float)Math.Pow(10.0, num / 20.0);
		}

		// Token: 0x06003A2E RID: 14894 RVA: 0x00653DF0 File Offset: 0x00651FF0
		public void Resume()
		{
			this._soundEffectInstance.Resume();
		}

		// Token: 0x06003A2F RID: 14895 RVA: 0x00653DFD File Offset: 0x00651FFD
		protected virtual void PrepareToPlay()
		{
			this.ResetBuffer();
		}

		// Token: 0x06003A30 RID: 14896
		public abstract void Reuse();

		// Token: 0x06003A31 RID: 14897 RVA: 0x00009E06 File Offset: 0x00008006
		protected virtual void ReadAheadPutAChunkIntoTheBuffer()
		{
		}

		// Token: 0x06003A32 RID: 14898
		public abstract void Dispose();

		// Token: 0x04005DA3 RID: 23971
		protected const int bufferLength = 4096;

		// Token: 0x04005DA4 RID: 23972
		protected const int bufferCountPerSubmit = 2;

		// Token: 0x04005DA5 RID: 23973
		protected const int buffersToCoverFor = 8;

		// Token: 0x04005DA6 RID: 23974
		protected byte[] _bufferToSubmit = new byte[4096];

		// Token: 0x04005DA7 RID: 23975
		protected float[] _temporaryBuffer = new float[2048];

		// Token: 0x04005DA8 RID: 23976
		private int _sampleRate;

		// Token: 0x04005DA9 RID: 23977
		private AudioChannels _channels;

		// Token: 0x04005DAA RID: 23978
		protected DynamicSoundEffectInstance _soundEffectInstance;
	}
}
