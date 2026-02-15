using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ID;

namespace Terraria.GameContent.Tile_Entities
{
	// Token: 0x0200041C RID: 1052
	public class TETrainingDummy : TileEntityType<TETrainingDummy>
	{
		// Token: 0x06003044 RID: 12356 RVA: 0x005B8C17 File Offset: 0x005B6E17
		public override void RegisterTileEntityID(int assignedID)
		{
			base.RegisterTileEntityID(assignedID);
			TileEntity._UpdateStart += TETrainingDummy.ClearBoxes;
		}

		// Token: 0x06003045 RID: 12357 RVA: 0x005B8C31 File Offset: 0x005B6E31
		public override void NetPlaceEntityAttempt(int x, int y)
		{
			TileEntityType<TETrainingDummy>.Place(x, y);
		}

		// Token: 0x06003046 RID: 12358 RVA: 0x005B8C3B File Offset: 0x005B6E3B
		public override bool IsTileValidForEntity(int x, int y)
		{
			return TETrainingDummy.ValidTile(x, y);
		}

		// Token: 0x06003047 RID: 12359 RVA: 0x005B8C44 File Offset: 0x005B6E44
		public static void ClearBoxes()
		{
			TETrainingDummy.playerBoxes.Clear();
			TETrainingDummy.playerBoxFilled = false;
			TETrainingDummy.npcSlotsFull = false;
		}

		// Token: 0x06003048 RID: 12360 RVA: 0x005B8C5C File Offset: 0x005B6E5C
		public override void Update()
		{
			if (this.npc != -1)
			{
				if (!Main.npc[this.npc].active || Main.npc[this.npc].type != 488 || Main.npc[this.npc].ai[0] != (float)this.Position.X || Main.npc[this.npc].ai[1] != (float)this.Position.Y)
				{
					this.Deactivate();
					return;
				}
			}
			else if (!TETrainingDummy.npcSlotsFull)
			{
				TETrainingDummy.FillPlayerHitboxes();
				Rectangle value = new Rectangle((int)(this.Position.X * 16), (int)(this.Position.Y * 16), 32, 48);
				value.Inflate(1600, 1600);
				bool flag = false;
				foreach (Rectangle rectangle in TETrainingDummy.playerBoxes)
				{
					if (rectangle.Intersects(value))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					this.Activate();
				}
			}
		}

		// Token: 0x06003049 RID: 12361 RVA: 0x005B8D88 File Offset: 0x005B6F88
		private static void FillPlayerHitboxes()
		{
			if (!TETrainingDummy.playerBoxFilled)
			{
				for (int i = 0; i < 255; i++)
				{
					if (Main.player[i].active)
					{
						TETrainingDummy.playerBoxes.Add(Main.player[i].getRect());
					}
				}
				TETrainingDummy.playerBoxFilled = true;
			}
		}

		// Token: 0x0600304A RID: 12362 RVA: 0x005B8DD8 File Offset: 0x005B6FD8
		public static bool ValidTile(int x, int y)
		{
			return Main.tile[x, y].active() && Main.tile[x, y].type == 378 && Main.tile[x, y].frameY == 0 && Main.tile[x, y].frameX % 36 == 0;
		}

		// Token: 0x0600304B RID: 12363 RVA: 0x005B8E3C File Offset: 0x005B703C
		public TETrainingDummy()
		{
			this.npc = -1;
			this.RequiresUpdates = true;
		}

		// Token: 0x0600304C RID: 12364 RVA: 0x005B8E54 File Offset: 0x005B7054
		public static int Hook_AfterPlacement(int x, int y, int type = 378, int style = 0, int direction = 1, int alternate = 0)
		{
			if (Main.netMode == 1)
			{
				NetMessage.SendTileSquare(Main.myPlayer, x - 1, y - 2, 2, 3, TileChangeType.None);
				NetMessage.SendData(87, -1, -1, null, x - 1, (float)(y - 2), (float)TileEntityType<TETrainingDummy>.EntityTypeID, 0f, 0, 0, 0);
				return -1;
			}
			return TileEntityType<TETrainingDummy>.Place(x - 1, y - 2);
		}

		// Token: 0x0600304D RID: 12365 RVA: 0x005B8EA8 File Offset: 0x005B70A8
		public override void WriteExtraData(BinaryWriter writer, bool networkSend)
		{
			writer.Write((short)this.npc);
		}

		// Token: 0x0600304E RID: 12366 RVA: 0x005B8EB7 File Offset: 0x005B70B7
		public override void ReadExtraData(BinaryReader reader, int gameVersion, bool networkSend)
		{
			this.npc = (int)reader.ReadInt16();
		}

		// Token: 0x0600304F RID: 12367 RVA: 0x005B8EC8 File Offset: 0x005B70C8
		private void Activate()
		{
			int num = NPC.NewNPC(new EntitySource_TileEntity(this), (int)(this.Position.X * 16 + 16), (int)(this.Position.Y * 16 + 48), 488, 100, 0f, 0f, 0f, 0f, 255);
			if (num == Main.maxNPCs)
			{
				TETrainingDummy.npcSlotsFull = true;
				return;
			}
			Main.npc[num].ai[0] = (float)this.Position.X;
			Main.npc[num].ai[1] = (float)this.Position.Y;
			Main.npc[num].netUpdate = true;
			this.npc = num;
			if (Main.netMode != 1)
			{
				NetMessage.SendData(86, -1, -1, null, this.ID, (float)this.Position.X, (float)this.Position.Y, 0f, 0, 0, 0);
			}
		}

		// Token: 0x06003050 RID: 12368 RVA: 0x005B8FB4 File Offset: 0x005B71B4
		public void Deactivate()
		{
			if (this.npc != -1)
			{
				Main.npc[this.npc].active = false;
			}
			this.npc = -1;
			if (Main.netMode != 1)
			{
				NetMessage.SendData(86, -1, -1, null, this.ID, (float)this.Position.X, (float)this.Position.Y, 0f, 0, 0, 0);
			}
		}

		// Token: 0x06003051 RID: 12369 RVA: 0x005B901C File Offset: 0x005B721C
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				this.Position.X,
				"x  ",
				this.Position.Y,
				"y npc: ",
				this.npc
			});
		}

		// Token: 0x040056AB RID: 22187
		private static List<Rectangle> playerBoxes = new List<Rectangle>();

		// Token: 0x040056AC RID: 22188
		private static bool playerBoxFilled;

		// Token: 0x040056AD RID: 22189
		private static bool npcSlotsFull;

		// Token: 0x040056AE RID: 22190
		public int npc;

		// Token: 0x040056AF RID: 22191
		public int activationRetryCooldown;
	}
}
