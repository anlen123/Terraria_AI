using System;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation.Dungeon.Halls;
using Terraria.GameContent.Generation.Dungeon.Rooms;
using Terraria.Localization;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation.Dungeon.LayoutProviders
{
	// Token: 0x020004BA RID: 1210
	public class LegacyDungeonLayoutProvider : DungeonLayoutProvider
	{
		// Token: 0x06003485 RID: 13445 RVA: 0x00603FE9 File Offset: 0x006021E9
		public LegacyDungeonLayoutProvider(DungeonLayoutProviderSettings settings) : base(settings)
		{
		}

		// Token: 0x06003486 RID: 13446 RVA: 0x0060547C File Offset: 0x0060367C
		public override void ProvideLayout(DungeonData data, GenerationProgress progress, UnifiedRandom genRand, ref int roomDelay)
		{
			LegacyDungeonLayoutProviderSettings legacyDungeonLayoutProviderSettings = (LegacyDungeonLayoutProviderSettings)this.settings;
			int steps = legacyDungeonLayoutProviderSettings.Steps;
			int maxSteps = legacyDungeonLayoutProviderSettings.MaxSteps;
			this.LegacyDungeonLayout(data, progress, genRand, this.settings.StyleData.BrickTileType, this.settings.StyleData.BrickCrackedTileType, this.settings.StyleData.BrickWallType, steps, maxSteps, ref roomDelay);
		}

		// Token: 0x06003487 RID: 13447 RVA: 0x006054E0 File Offset: 0x006036E0
		public void LegacyDungeonLayout(DungeonData data, GenerationProgress progress, UnifiedRandom genRand, ushort tileType, ushort crackedTileType, ushort wallType, int steps, int maxSteps, ref int roomDelay)
		{
			if (data.genVars.preGenDungeonEntranceSettings.PrecalculateEntrancePosition)
			{
				data.genVars.generatingDungeonPositionX = -10 + (int)data.genVars.dungeonEntrancePosition.X + genRand.Next(20);
				data.genVars.generatingDungeonPositionY = (int)data.genVars.dungeonEntrancePosition.Y + 30;
			}
			data.outerProgressionBounds = new DungeonBounds[1];
			data.outerProgressionBounds[0] = data.genVars.outerPotentialDungeonBounds;
			LegacyDungeonHallSettings legacyDungeonHallSettings = new LegacyDungeonHallSettings
			{
				StyleData = data.genVars.dungeonStyle,
				RandomSeed = genRand.Next()
			};
			LegacyDungeonRoomSettings legacyDungeonRoomSettings = new LegacyDungeonRoomSettings
			{
				StyleData = data.genVars.dungeonStyle,
				RandomSeed = genRand.Next()
			};
			DungeonCrawler.MakeDungeon_GetRoom(new LegacyDungeonRoomSettings
			{
				StyleData = data.genVars.dungeonStyle,
				StartingRoom = true,
				RandomSeed = genRand.Next(),
				RoomPosition = new Point(data.genVars.generatingDungeonPositionX, data.genVars.generatingDungeonPositionY)
			}, true).GenerateRoom(data);
			while (steps > 0)
			{
				data.dungeonBounds.UpdateBounds(data.genVars.generatingDungeonPositionX, data.genVars.generatingDungeonPositionY);
				steps--;
				int num = (maxSteps - steps) / maxSteps * 60;
				DungeonUtils.UpdateDungeonProgress(progress, (float)num / 100f, Language.GetTextValue("WorldGeneration.DungeonRoomsAndHalls"), false);
				if (roomDelay > 0)
				{
					roomDelay--;
				}
				if (roomDelay == 0 & genRand.Next(3) == 0)
				{
					roomDelay = 5;
					if (genRand.Next(2) == 0)
					{
						int generatingDungeonPositionX = data.genVars.generatingDungeonPositionX;
						int generatingDungeonPositionY = data.genVars.generatingDungeonPositionY;
						legacyDungeonHallSettings.RandomSeed = genRand.Next();
						DungeonCrawler.MakeDungeon_GetHall_Legacy(legacyDungeonHallSettings).GenerateHall(data, data.genVars.generatingDungeonPositionX, data.genVars.generatingDungeonPositionY);
						if (genRand.Next(2) == 0)
						{
							legacyDungeonHallSettings.RandomSeed = genRand.Next();
							DungeonCrawler.MakeDungeon_GetHall_Legacy(legacyDungeonHallSettings).GenerateHall(data, data.genVars.generatingDungeonPositionX, data.genVars.generatingDungeonPositionY);
						}
						legacyDungeonRoomSettings.RandomSeed = genRand.Next();
						legacyDungeonRoomSettings.RoomPosition = new Point(data.genVars.generatingDungeonPositionX, data.genVars.generatingDungeonPositionY);
						DungeonCrawler.MakeDungeon_GetRoom(legacyDungeonRoomSettings, true).GenerateRoom(data);
						data.genVars.generatingDungeonPositionX = generatingDungeonPositionX;
						data.genVars.generatingDungeonPositionY = generatingDungeonPositionY;
					}
					else
					{
						legacyDungeonRoomSettings.RandomSeed = genRand.Next();
						legacyDungeonRoomSettings.RoomPosition = new Point(data.genVars.generatingDungeonPositionX, data.genVars.generatingDungeonPositionY);
						DungeonCrawler.MakeDungeon_GetRoom(legacyDungeonRoomSettings, true).GenerateRoom(data);
					}
				}
				else
				{
					legacyDungeonHallSettings.RandomSeed = genRand.Next();
					DungeonCrawler.MakeDungeon_GetHall_Legacy(legacyDungeonHallSettings).GenerateHall(data, data.genVars.generatingDungeonPositionX, data.genVars.generatingDungeonPositionY);
				}
			}
			legacyDungeonRoomSettings.RandomSeed = genRand.Next();
			legacyDungeonRoomSettings.RoomPosition = new Point(data.genVars.generatingDungeonPositionX, data.genVars.generatingDungeonPositionY);
			DungeonCrawler.MakeDungeon_GetRoom(legacyDungeonRoomSettings, true).GenerateRoom(data);
			data.outerProgressionBounds[0] = data.dungeonBounds;
		}
	}
}
