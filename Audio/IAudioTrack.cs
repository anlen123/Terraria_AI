using System;
using Microsoft.Xna.Framework.Audio;

namespace Terraria.Audio
{
	// Token: 0x020005CF RID: 1487
	public interface IAudioTrack : IDisposable
	{
		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x06003A5D RID: 14941
		bool IsPlaying { get; }

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x06003A5E RID: 14942
		bool IsStopped { get; }

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x06003A5F RID: 14943
		bool IsPaused { get; }

		// Token: 0x06003A60 RID: 14944
		void Stop(AudioStopOptions options);

		// Token: 0x06003A61 RID: 14945
		void Play();

		// Token: 0x06003A62 RID: 14946
		void Pause();

		// Token: 0x06003A63 RID: 14947
		void SetVariable(string variableName, float value);

		// Token: 0x06003A64 RID: 14948
		void Resume();

		// Token: 0x06003A65 RID: 14949
		void Reuse();

		// Token: 0x06003A66 RID: 14950
		void Update();
	}
}
