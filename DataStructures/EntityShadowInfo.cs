using System;
using Microsoft.Xna.Framework;

namespace Terraria.DataStructures
{
	// Token: 0x0200053F RID: 1343
	public struct EntityShadowInfo
	{
		// Token: 0x0600374F RID: 14159 RVA: 0x0062DDCC File Offset: 0x0062BFCC
		public void CopyPlayer(Player player)
		{
			this.Position = player.position;
			this.Rotation = player.fullRotation;
			this.Origin = player.fullRotationOrigin;
			this.Direction = player.direction;
			this.GravityDirection = (int)player.gravDir;
			this.BodyFrameIndex = player.bodyFrame.Y / player.bodyFrame.Height;
		}

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x06003750 RID: 14160 RVA: 0x0062DE33 File Offset: 0x0062C033
		public Vector2 HeadgearOffset
		{
			get
			{
				return Main.OffsetsPlayerHeadgear[this.BodyFrameIndex];
			}
		}

		// Token: 0x04005B67 RID: 23399
		public Vector2 Position;

		// Token: 0x04005B68 RID: 23400
		public float Rotation;

		// Token: 0x04005B69 RID: 23401
		public Vector2 Origin;

		// Token: 0x04005B6A RID: 23402
		public int Direction;

		// Token: 0x04005B6B RID: 23403
		public int GravityDirection;

		// Token: 0x04005B6C RID: 23404
		public int BodyFrameIndex;
	}
}
