using System;
using System.IO;

namespace Terraria.GameContent.Creative
{
	// Token: 0x0200032A RID: 810
	public class CreativeUnlocksTracker : IPersistentPerWorldContent, IOnPlayerJoining
	{
		// Token: 0x060027BD RID: 10173 RVA: 0x00568714 File Offset: 0x00566914
		public void Save(BinaryWriter writer)
		{
			this.ItemSacrifices.Save(writer);
		}

		// Token: 0x060027BE RID: 10174 RVA: 0x00568722 File Offset: 0x00566922
		public void Load(BinaryReader reader, int gameVersionSaveWasMadeOn)
		{
			this.ItemSacrifices.Load(reader, gameVersionSaveWasMadeOn);
		}

		// Token: 0x060027BF RID: 10175 RVA: 0x00568731 File Offset: 0x00566931
		public void ValidateWorld(BinaryReader reader, int gameVersionSaveWasMadeOn)
		{
			this.ValidateWorld(reader, gameVersionSaveWasMadeOn);
		}

		// Token: 0x060027C0 RID: 10176 RVA: 0x0056873B File Offset: 0x0056693B
		public void Reset()
		{
			this.ItemSacrifices.Reset();
		}

		// Token: 0x060027C1 RID: 10177 RVA: 0x00568748 File Offset: 0x00566948
		public void OnPlayerJoining(int playerIndex)
		{
			this.ItemSacrifices.OnPlayerJoining(playerIndex);
		}

		// Token: 0x040050E3 RID: 20707
		public ItemsSacrificedUnlocksTracker ItemSacrifices = new ItemsSacrificedUnlocksTracker();
	}
}
