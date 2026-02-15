using System;
using System.IO;

namespace Terraria.GameContent.Bestiary
{
	// Token: 0x02000333 RID: 819
	public class BestiaryUnlocksTracker : IPersistentPerWorldContent, IOnPlayerJoining
	{
		// Token: 0x060027FB RID: 10235 RVA: 0x00570E2A File Offset: 0x0056F02A
		public void Save(BinaryWriter writer)
		{
			this.Kills.Save(writer);
			this.Sights.Save(writer);
			this.Chats.Save(writer);
		}

		// Token: 0x060027FC RID: 10236 RVA: 0x00570E50 File Offset: 0x0056F050
		public void Load(BinaryReader reader, int gameVersionSaveWasMadeOn)
		{
			this.Kills.Load(reader, gameVersionSaveWasMadeOn);
			this.Sights.Load(reader, gameVersionSaveWasMadeOn);
			this.Chats.Load(reader, gameVersionSaveWasMadeOn);
		}

		// Token: 0x060027FD RID: 10237 RVA: 0x00570E79 File Offset: 0x0056F079
		public void ValidateWorld(BinaryReader reader, int gameVersionSaveWasMadeOn)
		{
			this.Kills.ValidateWorld(reader, gameVersionSaveWasMadeOn);
			this.Sights.ValidateWorld(reader, gameVersionSaveWasMadeOn);
			this.Chats.ValidateWorld(reader, gameVersionSaveWasMadeOn);
		}

		// Token: 0x060027FE RID: 10238 RVA: 0x00570EA2 File Offset: 0x0056F0A2
		public void Reset()
		{
			this.Kills.Reset();
			this.Sights.Reset();
			this.Chats.Reset();
		}

		// Token: 0x060027FF RID: 10239 RVA: 0x00570EC5 File Offset: 0x0056F0C5
		public void OnPlayerJoining(int playerIndex)
		{
			this.Kills.OnPlayerJoining(playerIndex);
			this.Sights.OnPlayerJoining(playerIndex);
			this.Chats.OnPlayerJoining(playerIndex);
		}

		// Token: 0x06002800 RID: 10240 RVA: 0x00009E06 File Offset: 0x00008006
		public void FillBasedOnVersionBefore210()
		{
		}

		// Token: 0x040050F3 RID: 20723
		public NPCKillsTracker Kills = new NPCKillsTracker();

		// Token: 0x040050F4 RID: 20724
		public NPCWasNearPlayerTracker Sights = new NPCWasNearPlayerTracker();

		// Token: 0x040050F5 RID: 20725
		public NPCWasChatWithTracker Chats = new NPCWasChatWithTracker();
	}
}
