using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace Terraria.Audio
{
	// Token: 0x020005D7 RID: 1495
	public static class SoundInstanceGarbageCollector
	{
		// Token: 0x06003AB6 RID: 15030 RVA: 0x00657EFC File Offset: 0x006560FC
		public static void Track(SoundEffectInstance sound)
		{
			if (Program.IsFna)
			{
				SoundInstanceGarbageCollector._activeSounds.Add(sound);
			}
		}

		// Token: 0x06003AB7 RID: 15031 RVA: 0x00657F10 File Offset: 0x00656110
		public static void Update()
		{
			for (int i = 0; i < SoundInstanceGarbageCollector._activeSounds.Count; i++)
			{
				if (SoundInstanceGarbageCollector._activeSounds[i] == null)
				{
					SoundInstanceGarbageCollector._activeSounds.RemoveAt(i);
					i--;
				}
				else if (SoundInstanceGarbageCollector._activeSounds[i].State == SoundState.Stopped)
				{
					SoundInstanceGarbageCollector._activeSounds[i].Dispose();
					SoundInstanceGarbageCollector._activeSounds.RemoveAt(i);
					i--;
				}
			}
		}

		// Token: 0x04005E12 RID: 24082
		private static readonly List<SoundEffectInstance> _activeSounds = new List<SoundEffectInstance>(128);
	}
}
