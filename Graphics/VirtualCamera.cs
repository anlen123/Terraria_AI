using System;
using Microsoft.Xna.Framework;

namespace Terraria.Graphics
{
	// Token: 0x020001D4 RID: 468
	public struct VirtualCamera
	{
		// Token: 0x06001F89 RID: 8073 RVA: 0x0051C341 File Offset: 0x0051A541
		public VirtualCamera(Player player)
		{
			this.Player = player;
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06001F8A RID: 8074 RVA: 0x0051C34A File Offset: 0x0051A54A
		public Vector2 Position
		{
			get
			{
				return this.Center - this.Size * 0.5f;
			}
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06001F8B RID: 8075 RVA: 0x0051C367 File Offset: 0x0051A567
		public Vector2 Size
		{
			get
			{
				return new Vector2((float)Main.maxScreenW, (float)Main.maxScreenH);
			}
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06001F8C RID: 8076 RVA: 0x0051C37A File Offset: 0x0051A57A
		public Vector2 Center
		{
			get
			{
				return this.Player.Center;
			}
		}

		// Token: 0x04004A0D RID: 18957
		public readonly Player Player;
	}
}
