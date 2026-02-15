using System;

namespace Terraria.Audio
{
	// Token: 0x020005C9 RID: 1481
	public class VampireSizzleTracker
	{
		// Token: 0x06003A1E RID: 14878 RVA: 0x00653BEB File Offset: 0x00651DEB
		public VampireSizzleTracker(int whoAmI)
		{
			this._playerIndex = whoAmI;
		}

		// Token: 0x06003A1F RID: 14879 RVA: 0x00653BFA File Offset: 0x00651DFA
		public bool IsActiveAndInGame()
		{
			return !Main.gameMenu && Main.vampireSeed && Main.player[this._playerIndex].sunScorchCounter > 0;
		}

		// Token: 0x04005DA2 RID: 23970
		private int _playerIndex;
	}
}
