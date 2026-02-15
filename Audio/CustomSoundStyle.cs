using System;
using Microsoft.Xna.Framework.Audio;
using Terraria.Utilities;

namespace Terraria.Audio
{
	// Token: 0x020005CC RID: 1484
	public class CustomSoundStyle : SoundStyle
	{
		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x06003A44 RID: 14916 RVA: 0x000379F1 File Offset: 0x00035BF1
		public override bool IsTrackable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x06003A45 RID: 14917 RVA: 0x001DA9FB File Offset: 0x001D8BFB
		public override int MaxTrackedInstances
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06003A46 RID: 14918 RVA: 0x00653E0D File Offset: 0x0065200D
		public CustomSoundStyle(SoundEffect soundEffect, SoundType type = SoundType.Sound, float volume = 1f, float pitchVariance = 0f) : base(volume, pitchVariance, type)
		{
			this._soundEffects = new SoundEffect[]
			{
				soundEffect
			};
		}

		// Token: 0x06003A47 RID: 14919 RVA: 0x00653E29 File Offset: 0x00652029
		public CustomSoundStyle(SoundEffect[] soundEffects, SoundType type = SoundType.Sound, float volume = 1f, float pitchVariance = 0f) : base(volume, pitchVariance, type)
		{
			this._soundEffects = soundEffects;
		}

		// Token: 0x06003A48 RID: 14920 RVA: 0x00653E3C File Offset: 0x0065203C
		public override SoundEffect GetRandomSound()
		{
			return this._soundEffects[CustomSoundStyle.Random.Next(this._soundEffects.Length)];
		}

		// Token: 0x04005DAB RID: 23979
		private static readonly UnifiedRandom Random = new UnifiedRandom();

		// Token: 0x04005DAC RID: 23980
		private readonly SoundEffect[] _soundEffects;
	}
}
