using System;
using Microsoft.Xna.Framework.Audio;
using Terraria.Utilities;

namespace Terraria.Audio
{
	// Token: 0x020005D3 RID: 1491
	public class LegacySoundStyle : SoundStyle
	{
		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x06003A90 RID: 14992 RVA: 0x006578D8 File Offset: 0x00655AD8
		public int Style
		{
			get
			{
				if (this.Variations != 1)
				{
					return LegacySoundStyle.Random.Next(this._style, this._style + this.Variations);
				}
				return this._style;
			}
		}

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x06003A91 RID: 14993 RVA: 0x00657907 File Offset: 0x00655B07
		public override bool IsTrackable
		{
			get
			{
				return this.SoundId == 42;
			}
		}

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x06003A92 RID: 14994 RVA: 0x00657913 File Offset: 0x00655B13
		public override int MaxTrackedInstances
		{
			get
			{
				return this._maxTrackedInstances;
			}
		}

		// Token: 0x06003A93 RID: 14995 RVA: 0x0065791B File Offset: 0x00655B1B
		public LegacySoundStyle(int soundId, int style, SoundType type = SoundType.Sound, int maxTrackedInstances = 0) : base(type)
		{
			this._style = style;
			this.Variations = 1;
			this.SoundId = soundId;
			this._maxTrackedInstances = maxTrackedInstances;
		}

		// Token: 0x06003A94 RID: 14996 RVA: 0x00657941 File Offset: 0x00655B41
		public LegacySoundStyle(int soundId, int style, int variations, SoundType type = SoundType.Sound, int maxTrackedInstances = 0) : base(type)
		{
			this._style = style;
			this.Variations = variations;
			this.SoundId = soundId;
			this._maxTrackedInstances = maxTrackedInstances;
		}

		// Token: 0x06003A95 RID: 14997 RVA: 0x00657968 File Offset: 0x00655B68
		private LegacySoundStyle(int soundId, int style, int variations, SoundType type, float volume, float pitchVariance, int maxTrackedInstances) : base(volume, pitchVariance, type)
		{
			this._style = style;
			this.Variations = variations;
			this.SoundId = soundId;
			this._maxTrackedInstances = maxTrackedInstances;
		}

		// Token: 0x06003A96 RID: 14998 RVA: 0x00657993 File Offset: 0x00655B93
		public LegacySoundStyle WithVolume(float volume)
		{
			return new LegacySoundStyle(this.SoundId, this._style, this.Variations, base.Type, volume, base.PitchVariance, this.MaxTrackedInstances);
		}

		// Token: 0x06003A97 RID: 14999 RVA: 0x006579BF File Offset: 0x00655BBF
		public LegacySoundStyle WithPitchVariance(float pitchVariance)
		{
			return new LegacySoundStyle(this.SoundId, this._style, this.Variations, base.Type, base.Volume, pitchVariance, this.MaxTrackedInstances);
		}

		// Token: 0x06003A98 RID: 15000 RVA: 0x006579EB File Offset: 0x00655BEB
		public LegacySoundStyle AsMusic()
		{
			return new LegacySoundStyle(this.SoundId, this._style, this.Variations, SoundType.Music, base.Volume, base.PitchVariance, this.MaxTrackedInstances);
		}

		// Token: 0x06003A99 RID: 15001 RVA: 0x00657A17 File Offset: 0x00655C17
		public LegacySoundStyle AsAmbient()
		{
			return new LegacySoundStyle(this.SoundId, this._style, this.Variations, SoundType.Ambient, base.Volume, base.PitchVariance, this.MaxTrackedInstances);
		}

		// Token: 0x06003A9A RID: 15002 RVA: 0x00657A43 File Offset: 0x00655C43
		public LegacySoundStyle AsSound()
		{
			return new LegacySoundStyle(this.SoundId, this._style, this.Variations, SoundType.Sound, base.Volume, base.PitchVariance, this.MaxTrackedInstances);
		}

		// Token: 0x06003A9B RID: 15003 RVA: 0x00657A6F File Offset: 0x00655C6F
		public bool Includes(int soundId, int style)
		{
			return this.SoundId == soundId && style >= this._style && style < this._style + this.Variations;
		}

		// Token: 0x06003A9C RID: 15004 RVA: 0x00657A95 File Offset: 0x00655C95
		public override SoundEffect GetRandomSound()
		{
			if (this.IsTrackable)
			{
				return SoundEngine.GetTrackableSoundByStyleId(this.Style);
			}
			return null;
		}

		// Token: 0x04005E04 RID: 24068
		private static readonly UnifiedRandom Random = new UnifiedRandom();

		// Token: 0x04005E05 RID: 24069
		private readonly int _style;

		// Token: 0x04005E06 RID: 24070
		public readonly int Variations;

		// Token: 0x04005E07 RID: 24071
		public readonly int SoundId;

		// Token: 0x04005E08 RID: 24072
		public readonly int _maxTrackedInstances;
	}
}
