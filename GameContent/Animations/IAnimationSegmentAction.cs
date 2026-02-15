using System;

namespace Terraria.GameContent.Animations
{
	// Token: 0x0200052C RID: 1324
	public interface IAnimationSegmentAction<T>
	{
		// Token: 0x060036D9 RID: 14041
		void BindTo(T obj);

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x060036DA RID: 14042
		int ExpectedLengthOfActionInFrames { get; }

		// Token: 0x060036DB RID: 14043
		void ApplyTo(T obj, float localTimeForObj);

		// Token: 0x060036DC RID: 14044
		void SetDelay(float delay);
	}
}
