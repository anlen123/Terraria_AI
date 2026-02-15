using System;
using System.IO;

namespace Terraria.GameContent
{
	// Token: 0x0200025B RID: 603
	public interface IPersistentPerWorldContent
	{
		// Token: 0x0600234D RID: 9037
		void Save(BinaryWriter writer);

		// Token: 0x0600234E RID: 9038
		void Load(BinaryReader reader, int gameVersionSaveWasMadeOn);

		// Token: 0x0600234F RID: 9039
		void ValidateWorld(BinaryReader reader, int gameVersionSaveWasMadeOn);

		// Token: 0x06002350 RID: 9040
		void Reset();
	}
}
