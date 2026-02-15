using System;
using Microsoft.Xna.Framework.Audio;

namespace Terraria.Audio
{
	// Token: 0x020005D0 RID: 1488
	public class CueAudioTrack : IAudioTrack, IDisposable
	{
		// Token: 0x06003A67 RID: 14951 RVA: 0x00653E63 File Offset: 0x00652063
		public CueAudioTrack(SoundBank bank, string cueName)
		{
			this._soundBank = bank;
			this._cueName = cueName;
			this.Reuse();
		}

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x06003A68 RID: 14952 RVA: 0x00653E7F File Offset: 0x0065207F
		public bool IsPlaying
		{
			get
			{
				return this._cue.IsPlaying;
			}
		}

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x06003A69 RID: 14953 RVA: 0x00653E8C File Offset: 0x0065208C
		public bool IsStopped
		{
			get
			{
				return this._cue.IsStopped;
			}
		}

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x06003A6A RID: 14954 RVA: 0x00653E99 File Offset: 0x00652099
		public bool IsPaused
		{
			get
			{
				return this._cue.IsPaused;
			}
		}

		// Token: 0x06003A6B RID: 14955 RVA: 0x00653EA6 File Offset: 0x006520A6
		public void Stop(AudioStopOptions options)
		{
			this._cue.Stop(options);
		}

		// Token: 0x06003A6C RID: 14956 RVA: 0x00653EB4 File Offset: 0x006520B4
		public void Play()
		{
			this._cue.Play();
		}

		// Token: 0x06003A6D RID: 14957 RVA: 0x00653EC1 File Offset: 0x006520C1
		public void SetVariable(string variableName, float value)
		{
			this._cue.SetVariable(variableName, value);
		}

		// Token: 0x06003A6E RID: 14958 RVA: 0x00653ED0 File Offset: 0x006520D0
		public void Resume()
		{
			this._cue.Resume();
		}

		// Token: 0x06003A6F RID: 14959 RVA: 0x00653EDD File Offset: 0x006520DD
		public void Reuse()
		{
			if (this._cue != null)
			{
				this.Stop(AudioStopOptions.Immediate);
			}
			this._cue = this._soundBank.GetCue(this._cueName);
		}

		// Token: 0x06003A70 RID: 14960 RVA: 0x00653F05 File Offset: 0x00652105
		public void Pause()
		{
			this._cue.Pause();
		}

		// Token: 0x06003A71 RID: 14961 RVA: 0x00009E06 File Offset: 0x00008006
		public void Dispose()
		{
		}

		// Token: 0x06003A72 RID: 14962 RVA: 0x00009E06 File Offset: 0x00008006
		public void Update()
		{
		}

		// Token: 0x04005DAD RID: 23981
		private Cue _cue;

		// Token: 0x04005DAE RID: 23982
		private string _cueName;

		// Token: 0x04005DAF RID: 23983
		private SoundBank _soundBank;
	}
}
