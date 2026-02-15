using System;
using System.IO;

namespace Terraria.GameContent.LeashedEntities
{
	// Token: 0x02000458 RID: 1112
	public class NormalButterflyLeashedCritter : FlyLeashedCritter
	{
		// Token: 0x06003264 RID: 12900 RVA: 0x005EF2D6 File Offset: 0x005ED4D6
		protected override void SetDefaults(Item sample)
		{
			base.SetDefaults(sample);
			this.variant = (byte)sample.placeStyle;
		}

		// Token: 0x06003265 RID: 12901 RVA: 0x005EF2EC File Offset: 0x005ED4EC
		protected override void CopyToDummy()
		{
			base.CopyToDummy();
			LeashedCritter._dummy.ai[2] = (float)this.variant;
		}

		// Token: 0x06003266 RID: 12902 RVA: 0x005EF307 File Offset: 0x005ED507
		public override void NetSend(BinaryWriter writer, bool full)
		{
			base.NetSend(writer, full);
			if (full)
			{
				writer.Write(this.variant);
			}
		}

		// Token: 0x06003267 RID: 12903 RVA: 0x005EF320 File Offset: 0x005ED520
		public override void NetReceive(BinaryReader reader, bool full)
		{
			base.NetReceive(reader, full);
			if (full)
			{
				this.variant = reader.ReadByte();
			}
		}

		// Token: 0x040057F7 RID: 22519
		public new static NormalButterflyLeashedCritter Prototype = new NormalButterflyLeashedCritter();

		// Token: 0x040057F8 RID: 22520
		protected byte variant;
	}
}
