using System;

namespace Terraria.GameContent.Animations
{
	// Token: 0x02000529 RID: 1321
	public interface IAnimationSegment
	{
		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x060036D2 RID: 14034
		float DedicatedTimeNeeded { get; }

		// Token: 0x060036D3 RID: 14035
		void Draw(ref GameAnimationSegment info);
	}
}
