using System;
using System.IO;
using Microsoft.Xna.Framework;

namespace Terraria.DataStructures
{
	// Token: 0x02000556 RID: 1366
	public class NPCFollowState
	{
		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06003780 RID: 14208 RVA: 0x0062E795 File Offset: 0x0062C995
		public Vector2 BreadcrumbPosition
		{
			get
			{
				return this._floorBreadcrumb;
			}
		}

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x06003781 RID: 14209 RVA: 0x0062E79D File Offset: 0x0062C99D
		public bool IsFollowingPlayer
		{
			get
			{
				return this._playerIndexBeingFollowed != null;
			}
		}

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06003782 RID: 14210 RVA: 0x0062E7AA File Offset: 0x0062C9AA
		public Player PlayerBeingFollowed
		{
			get
			{
				if (this._playerIndexBeingFollowed != null)
				{
					return Main.player[this._playerIndexBeingFollowed.Value];
				}
				return null;
			}
		}

		// Token: 0x06003783 RID: 14211 RVA: 0x0062E7CC File Offset: 0x0062C9CC
		public void FollowPlayer(int playerIndex)
		{
			this._playerIndexBeingFollowed = new int?(playerIndex);
			this._floorBreadcrumb = Main.player[playerIndex].Bottom;
			this._npc.netUpdate = true;
		}

		// Token: 0x06003784 RID: 14212 RVA: 0x0062E7F8 File Offset: 0x0062C9F8
		public void StopFollowing()
		{
			this._playerIndexBeingFollowed = null;
			this.MoveNPCBackHome();
			this._npc.netUpdate = true;
		}

		// Token: 0x06003785 RID: 14213 RVA: 0x0062E818 File Offset: 0x0062CA18
		public void Clear(NPC npcToBelongTo)
		{
			this._npc = npcToBelongTo;
			this._playerIndexBeingFollowed = null;
			this._floorBreadcrumb = default(Vector2);
		}

		// Token: 0x06003786 RID: 14214 RVA: 0x0062E839 File Offset: 0x0062CA39
		private bool ShouldSync()
		{
			return this._npc.isLikeATownNPC;
		}

		// Token: 0x06003787 RID: 14215 RVA: 0x0062E848 File Offset: 0x0062CA48
		public void WriteTo(BinaryWriter writer)
		{
			int num = (this._playerIndexBeingFollowed != null) ? this._playerIndexBeingFollowed.Value : -1;
			writer.Write((short)num);
		}

		// Token: 0x06003788 RID: 14216 RVA: 0x0062E87C File Offset: 0x0062CA7C
		public void ReadFrom(BinaryReader reader)
		{
			short num = reader.ReadInt16();
			if (Main.player.IndexInRange((int)num))
			{
				this._playerIndexBeingFollowed = new int?((int)num);
			}
		}

		// Token: 0x06003789 RID: 14217 RVA: 0x0062E8AC File Offset: 0x0062CAAC
		private void MoveNPCBackHome()
		{
			this._npc.ai[0] = 20f;
			this._npc.ai[1] = 0f;
			this._npc.ai[2] = 0f;
			this._npc.ai[3] = 0f;
			this._npc.netUpdate = true;
		}

		// Token: 0x0600378A RID: 14218 RVA: 0x0062E910 File Offset: 0x0062CB10
		public void Update()
		{
			if (!this.IsFollowingPlayer)
			{
				return;
			}
			Player playerBeingFollowed = this.PlayerBeingFollowed;
			if (!playerBeingFollowed.active || playerBeingFollowed.dead)
			{
				this.StopFollowing();
				return;
			}
			this.UpdateBreadcrumbs(playerBeingFollowed);
			Dust.QuickDust(this._floorBreadcrumb, Color.Red);
		}

		// Token: 0x0600378B RID: 14219 RVA: 0x0062E95C File Offset: 0x0062CB5C
		private void UpdateBreadcrumbs(Player player)
		{
			Vector2? vector = null;
			if (player.velocity.Y == 0f && player.gravDir == 1f)
			{
				vector = new Vector2?(player.Bottom);
			}
			int num = 8;
			if (vector != null && Vector2.Distance(vector.Value, this._floorBreadcrumb) >= (float)num)
			{
				this._floorBreadcrumb = vector.Value;
				this._npc.netUpdate = true;
			}
		}

		// Token: 0x04005B97 RID: 23447
		private NPC _npc;

		// Token: 0x04005B98 RID: 23448
		private int? _playerIndexBeingFollowed;

		// Token: 0x04005B99 RID: 23449
		private Vector2 _floorBreadcrumb;
	}
}
