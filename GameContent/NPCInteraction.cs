using System;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent
{
	// Token: 0x0200023F RID: 575
	public abstract class NPCInteraction
	{
		// Token: 0x0600228A RID: 8842
		public abstract bool Condition();

		// Token: 0x0600228B RID: 8843
		public abstract string GetText();

		// Token: 0x0600228C RID: 8844
		public abstract void Interact();

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x0600228D RID: 8845 RVA: 0x001DA9FB File Offset: 0x001D8BFB
		public virtual bool ShowExcalmation
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600228E RID: 8846 RVA: 0x0053880D File Offset: 0x00536A0D
		public virtual bool TryAddCoins(ref Color chatColor, out int coinValue)
		{
			coinValue = 0;
			return false;
		}

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x0600228F RID: 8847 RVA: 0x00538813 File Offset: 0x00536A13
		public Player LocalPlayer
		{
			get
			{
				return Main.LocalPlayer;
			}
		}

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06002290 RID: 8848 RVA: 0x0053881A File Offset: 0x00536A1A
		public NPC TalkNPC
		{
			get
			{
				return Main.npc[this.LocalPlayer.talkNPC];
			}
		}

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06002291 RID: 8849 RVA: 0x0053882D File Offset: 0x00536A2D
		public int TalkNPCType
		{
			get
			{
				if (this.LocalPlayer.talkNPC == -1)
				{
					return 0;
				}
				return this.TalkNPC.type;
			}
		}
	}
}
