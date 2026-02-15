using System;
using System.Collections;
using System.Collections.Generic;
using ReLogic.Content.Sources;

namespace Terraria.Audio
{
	// Token: 0x020005CE RID: 1486
	public interface IAudioSystem : IDisposable
	{
		// Token: 0x06003A4E RID: 14926
		void LoadCue(int cueIndex, string cueName);

		// Token: 0x06003A4F RID: 14927
		void PauseAll();

		// Token: 0x06003A50 RID: 14928
		void ResumeAll();

		// Token: 0x06003A51 RID: 14929
		void UpdateMisc();

		// Token: 0x06003A52 RID: 14930
		void UpdateAudioEngine();

		// Token: 0x06003A53 RID: 14931
		void UpdateAmbientCueState(int i, bool gameIsActive, ref float trackVolume, float systemVolume);

		// Token: 0x06003A54 RID: 14932
		void UpdateAmbientCueTowardStopping(int i, float stoppingSpeed, ref float trackVolume, float systemVolume);

		// Token: 0x06003A55 RID: 14933
		void UpdateCommonTrack(bool active, int i, float totalVolume, ref float tempFade);

		// Token: 0x06003A56 RID: 14934
		void UpdateCommonTrackTowardStopping(int i, float totalVolume, ref float tempFade, bool isMainTrackAudible);

		// Token: 0x06003A57 RID: 14935
		bool IsTrackPlaying(int trackIndex);

		// Token: 0x06003A58 RID: 14936
		void UseSources(List<IContentSource> sources);

		// Token: 0x06003A59 RID: 14937
		IEnumerator PrepareWaveBank();

		// Token: 0x06003A5A RID: 14938
		void LoadFromSources();

		// Token: 0x06003A5B RID: 14939
		void Update();

		// Token: 0x06003A5C RID: 14940
		void SetPlayCallback(int trackIndex, AudioTrackPlayCallback callback);
	}
}
