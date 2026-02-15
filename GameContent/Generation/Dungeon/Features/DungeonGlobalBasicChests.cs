using System;
using Terraria.GameContent.Generation.Dungeon.Rooms;

namespace Terraria.GameContent.Generation.Dungeon.Features
{
	// Token: 0x020004D4 RID: 1236
	public class DungeonGlobalBasicChests : GlobalDungeonFeature
	{
		// Token: 0x060034D0 RID: 13520 RVA: 0x0060A332 File Offset: 0x00608532
		public DungeonGlobalBasicChests(DungeonFeatureSettings settings) : base(settings)
		{
			DungeonCrawler.CurrentDungeonData.dungeonFeatures.Add(this);
		}

		// Token: 0x060034D1 RID: 13521 RVA: 0x0060A34B File Offset: 0x0060854B
		public override bool GenerateFeature(DungeonData data)
		{
			this.generated = false;
			this.BasicChests(data);
			this.generated = true;
			return true;
		}

		// Token: 0x060034D2 RID: 13522 RVA: 0x0060A364 File Offset: 0x00608564
		private void BasicChests(DungeonData data)
		{
			for (int i = 0; i < data.dungeonRooms.Count; i++)
			{
				DungeonRoom dungeonRoom = data.dungeonRooms[i];
				int num = 0;
				while (num < 1000 && !dungeonRoom.TryGenerateChestInRoom(data, this))
				{
					num++;
				}
			}
		}
	}
}
