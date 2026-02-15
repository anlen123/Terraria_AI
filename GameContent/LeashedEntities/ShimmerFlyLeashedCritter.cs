using System;
using System.IO;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent.LeashedEntities
{
	// Token: 0x0200045C RID: 1116
	public class ShimmerFlyLeashedCritter : FlyLeashedCritter
	{
		// Token: 0x06003278 RID: 12920 RVA: 0x005EF89E File Offset: 0x005EDA9E
		protected override void SetDefaults(Item sample)
		{
			base.SetDefaults(sample);
			if (Main.netMode == 0)
			{
				this.oldPositions = LeashedCritter._dummy.oldPos;
			}
			this.oldPositionsLength = (byte)LeashedCritter._dummy.oldPos.Length;
		}

		// Token: 0x06003279 RID: 12921 RVA: 0x005EF8D1 File Offset: 0x005EDAD1
		public override void NetSend(BinaryWriter writer, bool full)
		{
			base.NetSend(writer, full);
			if (full)
			{
				writer.Write(this.oldPositionsLength);
			}
		}

		// Token: 0x0600327A RID: 12922 RVA: 0x005EF8EA File Offset: 0x005EDAEA
		public override void NetReceive(BinaryReader reader, bool full)
		{
			base.NetReceive(reader, full);
			if (full)
			{
				this.oldPositionsLength = reader.ReadByte();
				this.oldPositions = new Vector2[(int)this.oldPositionsLength];
			}
		}

		// Token: 0x0600327B RID: 12923 RVA: 0x005EF914 File Offset: 0x005EDB14
		protected override void VisualEffects()
		{
			base.VisualEffects();
			if (this.oldPositions == null)
			{
				return;
			}
			for (int i = this.oldPositions.Length - 1; i > 0; i--)
			{
				this.oldPositions[i] = this.oldPositions[i - 1];
			}
			this.oldPositions[0] = this.position + this.netOffset;
		}

		// Token: 0x0600327C RID: 12924 RVA: 0x005EF97C File Offset: 0x005EDB7C
		public override void Draw()
		{
			Vector2[] oldPos = LeashedCritter._dummy.oldPos;
			LeashedCritter._dummy.oldPos = this.oldPositions;
			base.Draw();
			LeashedCritter._dummy.oldPos = oldPos;
		}

		// Token: 0x04005800 RID: 22528
		public new static ShimmerFlyLeashedCritter Prototype = new ShimmerFlyLeashedCritter();

		// Token: 0x04005801 RID: 22529
		private byte oldPositionsLength;

		// Token: 0x04005802 RID: 22530
		private Vector2[] oldPositions;
	}
}
