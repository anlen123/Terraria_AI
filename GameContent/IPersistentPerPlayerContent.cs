using System;
using System.IO;

namespace Terraria.GameContent
{
	// Token: 0x0200025C RID: 604
	public interface IPersistentPerPlayerContent
	{
		// Token: 0x06002351 RID: 9041
		void Save(Player player, BinaryWriter writer);

		// Token: 0x06002352 RID: 9042
		void Load(Player player, BinaryReader reader, int gameVersionSaveWasMadeOn);

		// Token: 0x06002353 RID: 9043
		void ApplyLoadedDataToOutOfPlayerFields(Player player);

		// Token: 0x06002354 RID: 9044
		void ResetDataForNewPlayer(Player player);

		// Token: 0x06002355 RID: 9045
		void Reset();
	}
}
