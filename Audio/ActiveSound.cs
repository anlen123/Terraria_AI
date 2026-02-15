using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Terraria.Audio
{
	// Token: 0x020005C7 RID: 1479
	public class ActiveSound
	{
		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x06003A0C RID: 14860 RVA: 0x006537DF File Offset: 0x006519DF
		// (set) Token: 0x06003A0D RID: 14861 RVA: 0x006537E7 File Offset: 0x006519E7
		public SoundEffectInstance Sound { get; private set; }

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x06003A0E RID: 14862 RVA: 0x006537F0 File Offset: 0x006519F0
		// (set) Token: 0x06003A0F RID: 14863 RVA: 0x006537F8 File Offset: 0x006519F8
		public SoundStyle Style { get; private set; }

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x06003A10 RID: 14864 RVA: 0x00653801 File Offset: 0x00651A01
		public bool IsPlaying
		{
			get
			{
				return this.Sound != null && this.Sound.State == SoundState.Playing;
			}
		}

		// Token: 0x06003A11 RID: 14865 RVA: 0x0065381B File Offset: 0x00651A1B
		private void UseOverrides(SoundPlayOverrides overrides)
		{
			if (overrides.Volume != null)
			{
				this.Volume = overrides.Volume.Value;
			}
		}

		// Token: 0x06003A12 RID: 14866 RVA: 0x00653840 File Offset: 0x00651A40
		public ActiveSound(SoundStyle style, Vector2 position, SoundPlayOverrides overrides)
		{
			this.Position = position;
			this.Volume = 1f;
			this.Pitch = style.PitchVariance;
			this.IsGlobal = false;
			this.Style = style;
			this.UseOverrides(overrides);
			this.Play();
		}

		// Token: 0x06003A13 RID: 14867 RVA: 0x0065388C File Offset: 0x00651A8C
		public ActiveSound(SoundStyle style)
		{
			this.Position = Vector2.Zero;
			this.Volume = 1f;
			this.Pitch = style.PitchVariance;
			this.IsGlobal = true;
			this.Style = style;
			this.Play();
		}

		// Token: 0x06003A14 RID: 14868 RVA: 0x006538CC File Offset: 0x00651ACC
		public ActiveSound(SoundStyle style, Vector2 position, ActiveSound.LoopedPlayCondition condition, SoundPlayOverrides overrides)
		{
			this.Position = position;
			this.Volume = 1f;
			this.Pitch = style.PitchVariance;
			this.IsGlobal = false;
			this.Style = style;
			this.UseOverrides(overrides);
			this.PlayLooped(condition);
		}

		// Token: 0x06003A15 RID: 14869 RVA: 0x0065391C File Offset: 0x00651B1C
		private void Play()
		{
			SoundEffectInstance soundEffectInstance = this.Style.GetRandomSound().CreateInstance();
			this.Sound = soundEffectInstance;
			soundEffectInstance.Pitch += this.Style.GetRandomPitch();
			this.Pitch = soundEffectInstance.Pitch;
			soundEffectInstance.Volume = this.DetermineIntendedVolume();
			soundEffectInstance.Play();
			SoundInstanceGarbageCollector.Track(soundEffectInstance);
			this.Update();
		}

		// Token: 0x06003A16 RID: 14870 RVA: 0x00653984 File Offset: 0x00651B84
		private void PlayLooped(ActiveSound.LoopedPlayCondition condition)
		{
			SoundEffectInstance soundEffectInstance = this.Style.GetRandomSound().CreateInstance();
			this.Sound = soundEffectInstance;
			soundEffectInstance.Pitch += this.Style.GetRandomPitch();
			this.Pitch = soundEffectInstance.Pitch;
			soundEffectInstance.IsLooped = true;
			this.Condition = condition;
			soundEffectInstance.Play();
			SoundInstanceGarbageCollector.Track(soundEffectInstance);
			this.Update();
		}

		// Token: 0x06003A17 RID: 14871 RVA: 0x006539ED File Offset: 0x00651BED
		public void Stop()
		{
			if (this.Sound != null)
			{
				this.Sound.Stop();
			}
		}

		// Token: 0x06003A18 RID: 14872 RVA: 0x00653A02 File Offset: 0x00651C02
		public void Pause()
		{
			if (this.Sound != null && this.Sound.State == SoundState.Playing)
			{
				this.Sound.Pause();
			}
		}

		// Token: 0x06003A19 RID: 14873 RVA: 0x00653A24 File Offset: 0x00651C24
		public void Resume()
		{
			if (this.Sound != null && this.Sound.State == SoundState.Paused)
			{
				this.Sound.Resume();
			}
		}

		// Token: 0x06003A1A RID: 14874 RVA: 0x00653A48 File Offset: 0x00651C48
		public void Update()
		{
			if (this.Sound == null)
			{
				return;
			}
			if (this.Condition != null && !this.Condition())
			{
				this.Sound.Stop(true);
				return;
			}
			float volume = this.DetermineIntendedVolume();
			this.Sound.Volume = volume;
			this.Sound.Pitch = this.Pitch;
		}

		// Token: 0x06003A1B RID: 14875 RVA: 0x00653AA4 File Offset: 0x00651CA4
		private float DetermineIntendedVolume()
		{
			float num = 1f;
			if (!this.IsGlobal)
			{
				Vector2 vector = this.Position - Main.Camera.Center;
				this.Sound.Pan = MathHelper.Clamp(vector.X / ((float)Main.MaxWorldViewSize.X * 0.5f), -1f, 1f);
				num = MathHelper.Clamp(1f - vector.Length() / LegacySoundPlayer.SoundAttenuationDistance, 0f, 1f);
			}
			num *= this.Style.Volume * this.Volume;
			switch (this.Style.Type)
			{
			case SoundType.Sound:
				num *= Main.soundVolume;
				break;
			case SoundType.Ambient:
				num *= Main.ambientVolume;
				break;
			case SoundType.Music:
				num *= Main.musicVolume;
				break;
			}
			return MathHelper.Clamp(num, 0f, 1f);
		}

		// Token: 0x04005D9A RID: 23962
		public readonly bool IsGlobal;

		// Token: 0x04005D9B RID: 23963
		public Vector2 Position;

		// Token: 0x04005D9C RID: 23964
		public float Volume;

		// Token: 0x04005D9D RID: 23965
		public float Pitch;

		// Token: 0x04005D9F RID: 23967
		public ActiveSound.LoopedPlayCondition Condition;

		// Token: 0x020009C7 RID: 2503
		// (Invoke) Token: 0x06004A46 RID: 19014
		public delegate bool LoopedPlayCondition();
	}
}
