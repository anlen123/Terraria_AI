using System;

namespace Terraria.GameContent.UI.ResourceSets
{
	// Token: 0x020003B9 RID: 953
	public class CommonResourceBarMethods
	{
		// Token: 0x06002CD3 RID: 11475 RVA: 0x0059FB20 File Offset: 0x0059DD20
		public static void DrawLifeMouseOver()
		{
			if (!Main.mouseText)
			{
				Player localPlayer = Main.LocalPlayer;
				localPlayer.cursorItemIconEnabled = false;
				string text = localPlayer.statLife + "/" + localPlayer.statLifeMax2;
				Main.instance.MouseTextHackZoom(text, null);
				Main.mouseText = true;
			}
		}

		// Token: 0x06002CD4 RID: 11476 RVA: 0x0059FB74 File Offset: 0x0059DD74
		public static void DrawManaMouseOver()
		{
			if (!Main.mouseText)
			{
				Player localPlayer = Main.LocalPlayer;
				localPlayer.cursorItemIconEnabled = false;
				string text = localPlayer.statMana + "/" + localPlayer.statManaMax2;
				Main.instance.MouseTextHackZoom(text, null);
				Main.mouseText = true;
			}
		}
	}
}
