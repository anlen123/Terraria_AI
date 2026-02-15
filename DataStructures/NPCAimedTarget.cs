using System;
using Microsoft.Xna.Framework;
using Terraria.Enums;

namespace Terraria.DataStructures
{
	// Token: 0x02000597 RID: 1431
	public struct NPCAimedTarget
	{
		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x06003853 RID: 14419 RVA: 0x006313F8 File Offset: 0x0062F5F8
		public bool Invalid
		{
			get
			{
				return this.Type == NPCTargetType.None;
			}
		}

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x06003854 RID: 14420 RVA: 0x00631403 File Offset: 0x0062F603
		public Vector2 Center
		{
			get
			{
				return this.Position + this.Size / 2f;
			}
		}

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x06003855 RID: 14421 RVA: 0x00631420 File Offset: 0x0062F620
		public Vector2 Size
		{
			get
			{
				return new Vector2((float)this.Width, (float)this.Height);
			}
		}

		// Token: 0x06003856 RID: 14422 RVA: 0x00631438 File Offset: 0x0062F638
		public NPCAimedTarget(NPC npc)
		{
			this.Type = NPCTargetType.NPC;
			this.Hitbox = npc.Hitbox;
			this.Width = npc.width;
			this.Height = npc.height;
			this.Position = npc.position;
			this.Velocity = npc.velocity;
		}

		// Token: 0x06003857 RID: 14423 RVA: 0x00631488 File Offset: 0x0062F688
		public NPCAimedTarget(Player player, bool ignoreTank = true)
		{
			this.Type = NPCTargetType.Player;
			this.Hitbox = player.Hitbox;
			this.Width = player.width;
			this.Height = player.height;
			this.Position = player.position;
			this.Velocity = player.velocity;
			if (!ignoreTank && player.tankPet > -1)
			{
				Projectile projectile = Main.projectile[player.tankPet];
				this.Type = NPCTargetType.PlayerTankPet;
				this.Hitbox = projectile.Hitbox;
				this.Width = projectile.width;
				this.Height = projectile.height;
				this.Position = projectile.position;
				this.Velocity = projectile.velocity;
			}
		}

		// Token: 0x04005C63 RID: 23651
		public NPCTargetType Type;

		// Token: 0x04005C64 RID: 23652
		public Rectangle Hitbox;

		// Token: 0x04005C65 RID: 23653
		public int Width;

		// Token: 0x04005C66 RID: 23654
		public int Height;

		// Token: 0x04005C67 RID: 23655
		public Vector2 Position;

		// Token: 0x04005C68 RID: 23656
		public Vector2 Velocity;
	}
}
