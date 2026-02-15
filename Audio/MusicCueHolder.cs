using System;
using Microsoft.Xna.Framework.Audio;

namespace Terraria.Audio
{
	// Token: 0x020005D5 RID: 1493
	public class MusicCueHolder
	{
		// Token: 0x06003AA2 RID: 15010 RVA: 0x00657B64 File Offset: 0x00655D64
		public MusicCueHolder(SoundBank soundBank, string cueName)
		{
			this._soundBank = soundBank;
			this._cueName = cueName;
			this._loadedCue = null;
		}

		// Token: 0x06003AA3 RID: 15011 RVA: 0x00657B84 File Offset: 0x00655D84
		public void Pause()
		{
			if (this._loadedCue == null)
			{
				return;
			}
			if (this._loadedCue.IsPaused)
			{
				return;
			}
			if (!this._loadedCue.IsPlaying)
			{
				return;
			}
			try
			{
				this._loadedCue.Pause();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06003AA4 RID: 15012 RVA: 0x00657BD8 File Offset: 0x00655DD8
		public void Resume()
		{
			if (this._loadedCue == null)
			{
				return;
			}
			if (!this._loadedCue.IsPaused)
			{
				return;
			}
			try
			{
				this._loadedCue.Resume();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06003AA5 RID: 15013 RVA: 0x00657C20 File Offset: 0x00655E20
		public void Stop()
		{
			if (this._loadedCue == null)
			{
				return;
			}
			this.SetVolume(0f);
			this._loadedCue.Stop(AudioStopOptions.Immediate);
		}

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06003AA6 RID: 15014 RVA: 0x00657C42 File Offset: 0x00655E42
		public bool IsPlaying
		{
			get
			{
				return this._loadedCue != null && this._loadedCue.IsPlaying;
			}
		}

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x06003AA7 RID: 15015 RVA: 0x00657C59 File Offset: 0x00655E59
		public bool IsOngoing
		{
			get
			{
				return this._loadedCue != null && (this._loadedCue.IsPlaying || !this._loadedCue.IsStopped);
			}
		}

		// Token: 0x06003AA8 RID: 15016 RVA: 0x00657C82 File Offset: 0x00655E82
		public void RestartAndTryPlaying(float volume)
		{
			this.PurgeCue();
			this.TryPlaying(volume);
		}

		// Token: 0x06003AA9 RID: 15017 RVA: 0x00657C91 File Offset: 0x00655E91
		private void PurgeCue()
		{
			if (this._loadedCue == null)
			{
				return;
			}
			this._loadedCue.Stop(AudioStopOptions.Immediate);
			this._loadedCue.Dispose();
			this._loadedCue = null;
		}

		// Token: 0x06003AAA RID: 15018 RVA: 0x00657CBA File Offset: 0x00655EBA
		public void Play(float volume)
		{
			this.LoadTrack(false);
			this.SetVolume(volume);
			this._loadedCue.Play();
		}

		// Token: 0x06003AAB RID: 15019 RVA: 0x00657CD5 File Offset: 0x00655ED5
		public void TryPlaying(float volume)
		{
			this.LoadTrack(false);
			if (!this._loadedCue.IsPrepared)
			{
				return;
			}
			this.SetVolume(volume);
			if (!this._loadedCue.IsPlaying)
			{
				this._loadedCue.Play();
			}
		}

		// Token: 0x06003AAC RID: 15020 RVA: 0x00657D0B File Offset: 0x00655F0B
		public void SetVolume(float volume)
		{
			this._lastSetVolume = volume;
			if (this._loadedCue != null)
			{
				this._loadedCue.SetVariable("Volume", this._lastSetVolume);
			}
		}

		// Token: 0x06003AAD RID: 15021 RVA: 0x00657D32 File Offset: 0x00655F32
		private void LoadTrack(bool forceReload)
		{
			if (forceReload || this._loadedCue == null)
			{
				this._loadedCue = this._soundBank.GetCue(this._cueName);
			}
		}

		// Token: 0x04005E0B RID: 24075
		private SoundBank _soundBank;

		// Token: 0x04005E0C RID: 24076
		private string _cueName;

		// Token: 0x04005E0D RID: 24077
		private Cue _loadedCue;

		// Token: 0x04005E0E RID: 24078
		private float _lastSetVolume;
	}
}
