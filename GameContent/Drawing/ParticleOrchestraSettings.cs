using System;
using System.IO;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent.Drawing
{
	// Token: 0x02000442 RID: 1090
	public struct ParticleOrchestraSettings
	{
		// Token: 0x06003132 RID: 12594 RVA: 0x005C88CE File Offset: 0x005C6ACE
		public void Serialize(BinaryWriter writer)
		{
			writer.WriteVector2(this.PositionInWorld);
			writer.WriteVector2(this.MovementVector);
			writer.Write(this.UniqueInfoPiece);
			writer.Write(this.IndexOfPlayerWhoInvokedThis);
		}

		// Token: 0x06003133 RID: 12595 RVA: 0x005C8900 File Offset: 0x005C6B00
		public void DeserializeFrom(BinaryReader reader)
		{
			this.PositionInWorld = reader.ReadVector2();
			this.MovementVector = reader.ReadVector2();
			this.UniqueInfoPiece = reader.ReadInt32();
			this.IndexOfPlayerWhoInvokedThis = reader.ReadByte();
		}

		// Token: 0x0400574A RID: 22346
		public Vector2 PositionInWorld;

		// Token: 0x0400574B RID: 22347
		public Vector2 MovementVector;

		// Token: 0x0400574C RID: 22348
		public int UniqueInfoPiece;

		// Token: 0x0400574D RID: 22349
		public byte IndexOfPlayerWhoInvokedThis;
	}
}
