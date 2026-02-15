using System;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent.UI
{
	// Token: 0x0200037E RID: 894
	public class WorldUIAnchor
	{
		// Token: 0x06002983 RID: 10627 RVA: 0x0057CC96 File Offset: 0x0057AE96
		public WorldUIAnchor()
		{
			this.type = WorldUIAnchor.AnchorType.None;
		}

		// Token: 0x06002984 RID: 10628 RVA: 0x0057CCBB File Offset: 0x0057AEBB
		public WorldUIAnchor(Entity anchor)
		{
			this.type = WorldUIAnchor.AnchorType.Entity;
			this.entity = anchor;
		}

		// Token: 0x06002985 RID: 10629 RVA: 0x0057CCE7 File Offset: 0x0057AEE7
		public WorldUIAnchor(Vector2 anchor)
		{
			this.type = WorldUIAnchor.AnchorType.Pos;
			this.pos = anchor;
		}

		// Token: 0x06002986 RID: 10630 RVA: 0x0057CD14 File Offset: 0x0057AF14
		public WorldUIAnchor(int topLeftX, int topLeftY, int width, int height)
		{
			this.type = WorldUIAnchor.AnchorType.Tile;
			this.pos = new Vector2((float)topLeftX + (float)width / 2f, (float)topLeftY + (float)height / 2f) * 16f;
			this.size = new Vector2((float)width, (float)height) * 16f;
		}

		// Token: 0x06002987 RID: 10631 RVA: 0x0057CD8C File Offset: 0x0057AF8C
		public bool InRange(Vector2 target, float tileRangeX, float tileRangeY)
		{
			switch (this.type)
			{
			case WorldUIAnchor.AnchorType.Entity:
				return Math.Abs(target.X - this.entity.Center.X) <= tileRangeX * 16f + (float)this.entity.width / 2f && Math.Abs(target.Y - this.entity.Center.Y) <= tileRangeY * 16f + (float)this.entity.height / 2f;
			case WorldUIAnchor.AnchorType.Tile:
				return Math.Abs(target.X - this.pos.X) <= tileRangeX * 16f + this.size.X / 2f && Math.Abs(target.Y - this.pos.Y) <= tileRangeY * 16f + this.size.Y / 2f;
			case WorldUIAnchor.AnchorType.Pos:
				return Math.Abs(target.X - this.pos.X) <= tileRangeX * 16f && Math.Abs(target.Y - this.pos.Y) <= tileRangeY * 16f;
			default:
				return true;
			}
		}

		// Token: 0x04005279 RID: 21113
		public WorldUIAnchor.AnchorType type;

		// Token: 0x0400527A RID: 21114
		public Entity entity;

		// Token: 0x0400527B RID: 21115
		public Vector2 pos = Vector2.Zero;

		// Token: 0x0400527C RID: 21116
		public Vector2 size = Vector2.Zero;

		// Token: 0x020008D7 RID: 2263
		public enum AnchorType
		{
			// Token: 0x04007350 RID: 29520
			Entity,
			// Token: 0x04007351 RID: 29521
			Tile,
			// Token: 0x04007352 RID: 29522
			Pos,
			// Token: 0x04007353 RID: 29523
			None
		}
	}
}
