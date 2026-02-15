using System;
using System.Collections;
using System.Collections.Generic;
using ReLogic.Content.Sources;

namespace Terraria.Audio
{
	// Token: 0x020005CB RID: 1483
	public class DisabledAudioSystem : IAudioSystem, IDisposable
	{
		// Token: 0x06003A33 RID: 14899 RVA: 0x00009E06 File Offset: 0x00008006
		public void LoadFromSources()
		{
		}

		// Token: 0x06003A34 RID: 14900 RVA: 0x00009E06 File Offset: 0x00008006
		public void UseSources(List<IContentSource> sources)
		{
		}

		// Token: 0x06003A35 RID: 14901 RVA: 0x00009E06 File Offset: 0x00008006
		public void Update()
		{
		}

		// Token: 0x06003A36 RID: 14902 RVA: 0x00009E06 File Offset: 0x00008006
		public void UpdateMisc()
		{
		}

		// Token: 0x06003A38 RID: 14904 RVA: 0x00653E05 File Offset: 0x00652005
		public IEnumerator PrepareWaveBank()
		{
			yield break;
		}

		// Token: 0x06003A39 RID: 14905 RVA: 0x00009E06 File Offset: 0x00008006
		public void LoadCue(int cueIndex, string cueName)
		{
		}

		// Token: 0x06003A3A RID: 14906 RVA: 0x00009E06 File Offset: 0x00008006
		public void PauseAll()
		{
		}

		// Token: 0x06003A3B RID: 14907 RVA: 0x00009E06 File Offset: 0x00008006
		public void ResumeAll()
		{
		}

		// Token: 0x06003A3C RID: 14908 RVA: 0x00009E06 File Offset: 0x00008006
		public void UpdateAmbientCueState(int i, bool gameIsActive, ref float trackVolume, float systemVolume)
		{
		}

		// Token: 0x06003A3D RID: 14909 RVA: 0x00009E06 File Offset: 0x00008006
		public void UpdateAmbientCueTowardStopping(int i, float stoppingSpeed, ref float trackVolume, float systemVolume)
		{
		}

		// Token: 0x06003A3E RID: 14910 RVA: 0x001DA9FB File Offset: 0x001D8BFB
		public bool IsTrackPlaying(int trackIndex)
		{
			return false;
		}

		// Token: 0x06003A3F RID: 14911 RVA: 0x00009E06 File Offset: 0x00008006
		public void UpdateCommonTrack(bool active, int i, float totalVolume, ref float tempFade)
		{
		}

		// Token: 0x06003A40 RID: 14912 RVA: 0x00009E06 File Offset: 0x00008006
		public void UpdateCommonTrackTowardStopping(int i, float totalVolume, ref float tempFade, bool isMainTrackAudible)
		{
		}

		// Token: 0x06003A41 RID: 14913 RVA: 0x00009E06 File Offset: 0x00008006
		public void UpdateAudioEngine()
		{
		}

		// Token: 0x06003A42 RID: 14914 RVA: 0x00009E06 File Offset: 0x00008006
		public void SetPlayCallback(int trackIndex, AudioTrackPlayCallback callback)
		{
		}

		// Token: 0x06003A43 RID: 14915 RVA: 0x00009E06 File Offset: 0x00008006
		public void Dispose()
		{
		}
	}
}
