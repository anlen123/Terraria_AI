using System;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace Terraria.Testing
{
	// Token: 0x02000115 RID: 277
	public static class DebugOptions
	{
		// Token: 0x06001AF2 RID: 6898 RVA: 0x004F831C File Offset: 0x004F651C
		public static void SyncToJoiningPlayer(int playerIndex)
		{
			if (DebugOptions.enableDebugCommands)
			{
				NetMessage.SendData(94, playerIndex, -1, NetworkText.FromLiteral("/showdebug"), 0, (float)(DebugOptions.Shared_ReportCommandUsage ? 1 : 0), 0f, 0f, 0, 0, 0);
				NetMessage.SendData(94, playerIndex, -1, NetworkText.FromLiteral("/setserverping"), 0, (float)DebugOptions.Shared_ServerPing, 0f, 0f, 0, 0, 0);
			}
		}

		// Token: 0x04001522 RID: 5410
		public static bool enableDebugCommands = false;

		// Token: 0x04001523 RID: 5411
		public static bool Shared_ReportCommandUsage = true;

		// Token: 0x04001524 RID: 5412
		public static int Shared_ServerPing = 0;

		// Token: 0x04001525 RID: 5413
		public static double UpdateWaitInMs = 0.0;

		// Token: 0x04001526 RID: 5414
		public static double DrawWaitInMs = 0.0;

		// Token: 0x04001527 RID: 5415
		public static bool devLightTilesCheat;

		// Token: 0x04001528 RID: 5416
		public static bool noLimits;

		// Token: 0x04001529 RID: 5417
		public static bool noPause;

		// Token: 0x0400152A RID: 5418
		public static int unlockMap;

		// Token: 0x0400152B RID: 5419
		public static bool ShowSections;

		// Token: 0x0400152C RID: 5420
		public static bool ShowUnbreakableWall;

		// Token: 0x0400152D RID: 5421
		public static bool DrawLinkPoints;

		// Token: 0x0400152E RID: 5422
		public static bool ShowNetOffsetDust;

		// Token: 0x0400152F RID: 5423
		public static Vector2 FakeNetOffset;

		// Token: 0x04001530 RID: 5424
		public static bool hideTiles = false;

		// Token: 0x04001531 RID: 5425
		public static bool hideTiles2 = false;

		// Token: 0x04001532 RID: 5426
		public static bool hideWalls = false;

		// Token: 0x04001533 RID: 5427
		public static bool hideWater = false;

		// Token: 0x04001534 RID: 5428
		public static bool NoDamageVar;

		// Token: 0x04001535 RID: 5429
		public static bool LetProjectilesAimAtTargetDummies;

		// Token: 0x04001536 RID: 5430
		public static bool PracticeMode;
	}
}
