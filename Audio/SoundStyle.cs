using System;
using Microsoft.Xna.Framework.Audio;
using Terraria.Utilities;

namespace Terraria.Audio
{
	// Token: 0x020005DA RID: 1498
	public abstract class SoundStyle
	{
		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x06003AC5 RID: 15045 RVA: 0x0065833C File Offset: 0x0065653C
		public float Volume
		{
			get
			{
				return this._volume;
			}
		}

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x06003AC6 RID: 15046 RVA: 0x00658344 File Offset: 0x00656544
		public float PitchVariance
		{
			get
			{
				return this._pitchVariance;
			}
		}

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06003AC7 RID: 15047 RVA: 0x0065834C File Offset: 0x0065654C
		public SoundType Type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x06003AC8 RID: 15048
		public abstract bool IsTrackable { get; }

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x06003AC9 RID: 15049
		public abstract int MaxTrackedInstances { get; }

		// Token: 0x06003ACA RID: 15050 RVA: 0x00658354 File Offset: 0x00656554
		public SoundStyle(float volume, float pitchVariance, SoundType type = SoundType.Sound)
		{
			this._volume = volume;
			this._pitchVariance = pitchVariance;
			this._type = type;
		}

		// Token: 0x06003ACB RID: 15051 RVA: 0x00658371 File Offset: 0x00656571
		public SoundStyle(SoundType type = SoundType.Sound)
		{
			this._volume = 1f;
			this._pitchVariance = 0f;
			this._type = type;
		}

		// Token: 0x06003ACC RID: 15052 RVA: 0x00658396 File Offset: 0x00656596
		public float GetRandomPitch()
		{
			return SoundStyle._random.NextFloat() * this.PitchVariance - this.PitchVariance * 0.5f;
		}

		// Token: 0x06003ACD RID: 15053
		public abstract SoundEffect GetRandomSound();

		// Token: 0x04005E18 RID: 24088
		private static UnifiedRandom _random = new UnifiedRandom();

		// Token: 0x04005E19 RID: 24089
		private float _volume;

		// Token: 0x04005E1A RID: 24090
		private float _pitchVariance;

		// Token: 0x04005E1B RID: 24091
		private SoundType _type;
	}
}
