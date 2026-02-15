using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using ReLogic.Content.Sources;

namespace Terraria.Audio
{
	// Token: 0x020005D1 RID: 1489
	public class LegacyAudioSystem : IAudioSystem, IDisposable
	{
		// Token: 0x06003A73 RID: 14963 RVA: 0x00653F14 File Offset: 0x00652114
		public void LoadFromSources()
		{
			List<IContentSource> fileSources = this.FileSources;
			for (int i = 0; i < this.AudioTracks.Length; i++)
			{
				string str;
				if (this.TrackNamesByIndex.TryGetValue(i, out str))
				{
					string assetPath = "Music" + Path.DirectorySeparatorChar.ToString() + str;
					IAudioTrack audioTrack = this.DefaultTrackByIndex[i];
					IAudioTrack audioTrack2 = audioTrack;
					IAudioTrack audioTrack3 = this.FindReplacementTrack(fileSources, assetPath);
					if (audioTrack3 != null)
					{
						audioTrack2 = audioTrack3;
					}
					if (this.AudioTracks[i] != audioTrack2)
					{
						this.AudioTracks[i].Stop(AudioStopOptions.Immediate);
					}
					if (this.AudioTracks[i] != audioTrack)
					{
						this.AudioTracks[i].Dispose();
					}
					this.AudioTracks[i] = audioTrack2;
				}
			}
		}

		// Token: 0x06003A74 RID: 14964 RVA: 0x00653FCC File Offset: 0x006521CC
		public void UseSources(List<IContentSource> sourcesFromLowestToHighest)
		{
			this.FileSources = sourcesFromLowestToHighest;
			this.LoadFromSources();
		}

		// Token: 0x06003A75 RID: 14965 RVA: 0x00653FDC File Offset: 0x006521DC
		public void Update()
		{
			if (!this.WaveBank.IsPrepared)
			{
				return;
			}
			for (int i = 0; i < this.AudioTracks.Length; i++)
			{
				if (this.AudioTracks[i] != null)
				{
					this.AudioTracks[i].Update();
				}
			}
		}

		// Token: 0x06003A76 RID: 14966 RVA: 0x00654024 File Offset: 0x00652224
		private IAudioTrack FindReplacementTrack(List<IContentSource> sources, string assetPath)
		{
			IAudioTrack audioTrack = null;
			for (int i = 0; i < sources.Count; i++)
			{
				IContentSource contentSource = sources[i];
				if (contentSource.HasAsset(assetPath))
				{
					string extension = contentSource.GetExtension(assetPath);
					try
					{
						IAudioTrack audioTrack2 = null;
						if (!(extension == ".ogg"))
						{
							if (!(extension == ".wav"))
							{
								if (extension == ".mp3")
								{
									audioTrack2 = new MP3AudioTrack(contentSource.OpenStream(assetPath));
								}
							}
							else
							{
								audioTrack2 = new WAVAudioTrack(contentSource.OpenStream(assetPath));
							}
						}
						else
						{
							audioTrack2 = new OGGAudioTrack(contentSource.OpenStream(assetPath));
						}
						if (audioTrack2 != null)
						{
							if (audioTrack != null)
							{
								audioTrack.Dispose();
							}
							audioTrack = audioTrack2;
						}
					}
					catch
					{
						string textToShow = "A resource pack failed to load " + assetPath + "!";
						Main.IssueReporter.AddReport(textToShow);
						Main.IssueReporterIndicator.AttemptLettingPlayerKnow();
					}
				}
			}
			return audioTrack;
		}

		// Token: 0x06003A77 RID: 14967 RVA: 0x00654110 File Offset: 0x00652310
		public LegacyAudioSystem()
		{
			this.Engine = new AudioEngine("Content\\TerrariaMusic.xgs");
			this.SoundBank = new SoundBank(this.Engine, "Content\\Sound Bank.xsb");
			this.Engine.Update();
			this.WaveBank = new WaveBank(this.Engine, "Content\\Wave Bank.xwb", 0, 512);
			this.Engine.Update();
			this.AudioTracks = new IAudioTrack[Main.maxMusic];
			this.TrackNamesByIndex = new Dictionary<int, string>();
			this.DefaultTrackByIndex = new Dictionary<int, IAudioTrack>();
			this.TrackLoopCounts = new int[Main.maxMusic];
			this.PlayCallbacks = new AudioTrackPlayCallback[Main.maxMusic];
		}

		// Token: 0x06003A78 RID: 14968 RVA: 0x006541C1 File Offset: 0x006523C1
		public IEnumerator PrepareWaveBank()
		{
			while (!this.WaveBank.IsPrepared)
			{
				this.Engine.Update();
				yield return null;
			}
			yield break;
		}

		// Token: 0x06003A79 RID: 14969 RVA: 0x006541D0 File Offset: 0x006523D0
		public void LoadCue(int cueIndex, string cueName)
		{
			CueAudioTrack cueAudioTrack = new CueAudioTrack(this.SoundBank, cueName);
			this.TrackNamesByIndex[cueIndex] = cueName;
			this.DefaultTrackByIndex[cueIndex] = cueAudioTrack;
			this.AudioTracks[cueIndex] = cueAudioTrack;
		}

		// Token: 0x06003A7A RID: 14970 RVA: 0x00009E06 File Offset: 0x00008006
		public void UpdateMisc()
		{
		}

		// Token: 0x06003A7B RID: 14971 RVA: 0x00654210 File Offset: 0x00652410
		public void PauseAll()
		{
			if (!this.WaveBank.IsPrepared)
			{
				return;
			}
			float[] musicFade = Main.musicFade;
			for (int i = 0; i < this.AudioTracks.Length; i++)
			{
				if (this.AudioTracks[i] != null && !this.AudioTracks[i].IsPaused && this.AudioTracks[i].IsPlaying && musicFade[i] > 0f)
				{
					try
					{
						this.AudioTracks[i].Pause();
					}
					catch (Exception)
					{
					}
				}
			}
		}

		// Token: 0x06003A7C RID: 14972 RVA: 0x00654298 File Offset: 0x00652498
		public void ResumeAll()
		{
			if (!this.WaveBank.IsPrepared)
			{
				return;
			}
			float[] musicFade = Main.musicFade;
			for (int i = 0; i < this.AudioTracks.Length; i++)
			{
				if (this.AudioTracks[i] != null && this.AudioTracks[i].IsPaused && musicFade[i] > 0f)
				{
					try
					{
						this.AudioTracks[i].Resume();
					}
					catch (Exception)
					{
					}
				}
			}
		}

		// Token: 0x06003A7D RID: 14973 RVA: 0x00654314 File Offset: 0x00652514
		public void UpdateAmbientCueState(int i, bool gameIsActive, ref float trackVolume, float systemVolume)
		{
			if (!this.WaveBank.IsPrepared || this.AudioTracks[i] == null)
			{
				return;
			}
			if (systemVolume == 0f)
			{
				if (this.AudioTracks[i].IsPlaying)
				{
					this.AudioTracks[i].Stop(AudioStopOptions.Immediate);
					return;
				}
			}
			else
			{
				if (!this.AudioTracks[i].IsPlaying)
				{
					this.AudioTracks[i].Reuse();
					this.AudioTracks[i].Play();
					this.AudioTracks[i].SetVariable("Volume", trackVolume * systemVolume);
					return;
				}
				if (this.AudioTracks[i].IsPaused && gameIsActive)
				{
					this.AudioTracks[i].Resume();
					return;
				}
				trackVolume += 0.005f;
				if (trackVolume > 1f)
				{
					trackVolume = 1f;
				}
				this.AudioTracks[i].SetVariable("Volume", trackVolume * systemVolume);
			}
		}

		// Token: 0x06003A7E RID: 14974 RVA: 0x006543F4 File Offset: 0x006525F4
		public void UpdateAmbientCueTowardStopping(int i, float stoppingSpeed, ref float trackVolume, float systemVolume)
		{
			if (!this.WaveBank.IsPrepared || this.AudioTracks[i] == null)
			{
				return;
			}
			if (!this.AudioTracks[i].IsPlaying)
			{
				trackVolume = 0f;
				return;
			}
			if (trackVolume > 0f)
			{
				trackVolume -= stoppingSpeed;
				if (trackVolume < 0f)
				{
					trackVolume = 0f;
				}
			}
			if (trackVolume <= 0f)
			{
				this.AudioTracks[i].Stop(AudioStopOptions.Immediate);
				return;
			}
			this.AudioTracks[i].SetVariable("Volume", trackVolume * systemVolume);
		}

		// Token: 0x06003A7F RID: 14975 RVA: 0x0065447E File Offset: 0x0065267E
		public bool IsTrackPlaying(int trackIndex)
		{
			return this.WaveBank.IsPrepared && this.AudioTracks[trackIndex] != null && this.AudioTracks[trackIndex].IsPlaying;
		}

		// Token: 0x06003A80 RID: 14976 RVA: 0x006544A8 File Offset: 0x006526A8
		public void UpdateCommonTrack(bool active, int i, float totalVolume, ref float tempFade)
		{
			if (!this.WaveBank.IsPrepared || this.AudioTracks[i] == null)
			{
				return;
			}
			tempFade += 0.005f;
			if (tempFade > 1f)
			{
				tempFade = 1f;
			}
			if (!this.AudioTracks[i].IsPlaying && active)
			{
				this.AudioTracks[i].Reuse();
				this.AudioTracks[i].SetVariable("Volume", totalVolume);
				this.AudioTracks[i].Play();
				if (this.PlayCallbacks[i] != null)
				{
					this.PlayCallbacks[i](i, this.TrackLoopCounts[i]);
				}
				this.TrackLoopCounts[i]++;
				return;
			}
			this.AudioTracks[i].SetVariable("Volume", totalVolume);
		}

		// Token: 0x06003A81 RID: 14977 RVA: 0x00654574 File Offset: 0x00652774
		public void UpdateCommonTrackTowardStopping(int i, float totalVolume, ref float tempFade, bool isMainTrackAudible)
		{
			if (!this.WaveBank.IsPrepared || this.AudioTracks[i] == null)
			{
				return;
			}
			if (!this.AudioTracks[i].IsPlaying && this.AudioTracks[i].IsStopped)
			{
				tempFade = 0f;
				return;
			}
			if (isMainTrackAudible)
			{
				tempFade -= 0.005f;
			}
			else if (Main.curMusic == 0)
			{
				tempFade = 0f;
			}
			if (tempFade <= 0f)
			{
				tempFade = 0f;
				this.AudioTracks[i].SetVariable("Volume", 0f);
				this.AudioTracks[i].Stop(AudioStopOptions.Immediate);
				this.TrackLoopCounts[i] = 0;
				return;
			}
			this.AudioTracks[i].SetVariable("Volume", totalVolume);
		}

		// Token: 0x06003A82 RID: 14978 RVA: 0x0065462F File Offset: 0x0065282F
		public void UpdateAudioEngine()
		{
			this.Engine.Update();
		}

		// Token: 0x06003A83 RID: 14979 RVA: 0x0065463C File Offset: 0x0065283C
		public void SetPlayCallback(int trackIndex, AudioTrackPlayCallback callback)
		{
			this.PlayCallbacks[trackIndex] = callback;
		}

		// Token: 0x06003A84 RID: 14980 RVA: 0x00654647 File Offset: 0x00652847
		public void Dispose()
		{
			this.SoundBank.Dispose();
			this.WaveBank.Dispose();
			this.Engine.Dispose();
		}

		// Token: 0x04005DB0 RID: 23984
		public IAudioTrack[] AudioTracks;

		// Token: 0x04005DB1 RID: 23985
		public int MusicReplayDelay;

		// Token: 0x04005DB2 RID: 23986
		public AudioEngine Engine;

		// Token: 0x04005DB3 RID: 23987
		public SoundBank SoundBank;

		// Token: 0x04005DB4 RID: 23988
		public WaveBank WaveBank;

		// Token: 0x04005DB5 RID: 23989
		public Dictionary<int, string> TrackNamesByIndex;

		// Token: 0x04005DB6 RID: 23990
		public Dictionary<int, IAudioTrack> DefaultTrackByIndex;

		// Token: 0x04005DB7 RID: 23991
		public List<IContentSource> FileSources;

		// Token: 0x04005DB8 RID: 23992
		public int[] TrackLoopCounts;

		// Token: 0x04005DB9 RID: 23993
		public AudioTrackPlayCallback[] PlayCallbacks;
	}
}
