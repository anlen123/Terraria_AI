using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Creative;
using Terraria.GameContent.Events;
using Terraria.GameContent.Golf;
using Terraria.GameContent.Tile_Entities;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Map;
using Terraria.Net;
using Terraria.Net.Sockets;
using Terraria.Testing;
using Terraria.UI;

namespace Terraria
{
	// Token: 0x0200002D RID: 45
	public class MessageBuffer
	{
		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000215 RID: 533 RVA: 0x00021EF9 File Offset: 0x000200F9
		public int RemainingReadBufferLength
		{
			get
			{
				return this.readBuffer.Length - this.totalData;
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000216 RID: 534 RVA: 0x00021F0C File Offset: 0x0002010C
		// (remove) Token: 0x06000217 RID: 535 RVA: 0x00021F40 File Offset: 0x00020140
		public static event TileChangeReceivedEvent OnTileChangeReceived;

		// Token: 0x06000218 RID: 536 RVA: 0x00021F74 File Offset: 0x00020174
		public void Reset()
		{
			Array.Clear(this.readBuffer, 0, this.readBuffer.Length);
			Array.Clear(this.writeBuffer, 0, this.writeBuffer.Length);
			this.writeLocked = false;
			this.messageLength = 0;
			this.totalData = 0;
			this.spamCount = 0;
			this.broadcast = false;
			this.checkBytes = false;
			this.ResetReader();
			this.ResetWriter();
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00021FDF File Offset: 0x000201DF
		public void ResetReader()
		{
			if (this.readerStream != null)
			{
				this.readerStream.Close();
			}
			this.readerStream = new MemoryStream(this.readBuffer);
			this.reader = new BinaryReader(this.readerStream);
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00022016 File Offset: 0x00020216
		public void ResetWriter()
		{
			if (this.writerStream != null)
			{
				this.writerStream.Close();
			}
			this.writerStream = new MemoryStream(this.writeBuffer);
			this.writer = new BinaryWriter(this.writerStream);
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00022050 File Offset: 0x00020250
		private float[] ReUseTemporaryProjectileAI()
		{
			for (int i = 0; i < this._temporaryProjectileAI.Length; i++)
			{
				this._temporaryProjectileAI[i] = 0f;
			}
			return this._temporaryProjectileAI;
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00022084 File Offset: 0x00020284
		private float[] ReUseTemporaryNPCAI()
		{
			for (int i = 0; i < this._temporaryNPCAI.Length; i++)
			{
				this._temporaryNPCAI[i] = 0f;
			}
			return this._temporaryNPCAI;
		}

		// Token: 0x0600021D RID: 541 RVA: 0x000220B8 File Offset: 0x000202B8
		public void GetData(int start, int length, out int messageType)
		{
			if (this.whoAmI < 256)
			{
				Netplay.Clients[this.whoAmI].TimeOutTimer = 0;
			}
			else
			{
				Netplay.Connection.TimeOutTimer = 0;
			}
			int num = start + 1;
			byte b = this.readBuffer[start];
			messageType = (int)b;
			if (b >= MessageID.Count)
			{
				return;
			}
			Main.ActiveNetDiagnosticsUI.CountReadMessage((int)b, length);
			if (Main.netMode == 1 && Netplay.Connection.StatusMax > 0)
			{
				Netplay.Connection.StatusCount++;
			}
			if (Main.verboseNetplay)
			{
				for (int i = start; i < start + length; i++)
				{
				}
				for (int j = start; j < start + length; j++)
				{
					byte b2 = this.readBuffer[j];
				}
			}
			if (Main.netMode == 2 && b != 38 && Netplay.Clients[this.whoAmI].State == -1)
			{
				NetMessage.TrySendData(2, this.whoAmI, -1, Lang.mp[1].ToNetworkText(), 0, 0f, 0f, 0f, 0, 0, 0);
				return;
			}
			if (Main.netMode == 2)
			{
				if (Netplay.Clients[this.whoAmI].State < 10 && b > 12 && b != 93 && b != 16 && b != 42 && b != 50 && b != 38 && b != 68 && b != 147 && b != 161)
				{
					NetMessage.BootPlayer(this.whoAmI, Lang.mp[2].ToNetworkText());
				}
				if (Netplay.Clients[this.whoAmI].State == 0 && b != 1)
				{
					NetMessage.BootPlayer(this.whoAmI, Lang.mp[2].ToNetworkText());
				}
			}
			if (this.reader == null)
			{
				this.ResetReader();
			}
			this.reader.BaseStream.Position = (long)num;
			switch (b)
			{
			case 1:
				if (Main.netMode != 2)
				{
					return;
				}
				if (Main.dedServ && Netplay.IsBanned(Netplay.Clients[this.whoAmI].Socket.GetRemoteAddress()))
				{
					NetMessage.TrySendData(2, this.whoAmI, -1, Lang.mp[3].ToNetworkText(), 0, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				if (Netplay.Clients[this.whoAmI].State != 0)
				{
					return;
				}
				if (!(this.reader.ReadString() == "Terraria" + 318))
				{
					NetMessage.TrySendData(2, this.whoAmI, -1, Lang.mp[4].ToNetworkText(), 0, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				if (string.IsNullOrEmpty(Netplay.ServerPassword))
				{
					Netplay.Clients[this.whoAmI].State = 1;
					NetMessage.TrySendData(3, this.whoAmI, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				Netplay.Clients[this.whoAmI].State = -1;
				NetMessage.TrySendData(37, this.whoAmI, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
				return;
			case 2:
				if (Main.netMode != 1)
				{
					return;
				}
				Netplay.Disconnect = true;
				Main.statusText = NetworkText.Deserialize(this.reader).ToString();
				return;
			case 3:
			{
				if (Main.netMode != 1)
				{
					return;
				}
				if (Netplay.Connection.State == 1)
				{
					Netplay.Connection.State = 2;
				}
				int num2 = (int)this.reader.ReadByte();
				bool value = this.reader.ReadBoolean();
				Netplay.Connection.ServerSpecialFlags[2] = value;
				if (num2 != Main.myPlayer)
				{
					Main.player[num2] = Main.ActivePlayerFileData.Player;
					Main.player[Main.myPlayer] = new Player();
				}
				Main.player[num2].whoAmI = num2;
				Main.myPlayer = num2;
				Player player = Main.player[num2];
				NetMessage.TrySendData(4, -1, -1, null, num2, 0f, 0f, 0f, 0, 0, 0);
				NetMessage.TrySendData(68, -1, -1, null, num2, 0f, 0f, 0f, 0, 0, 0);
				NetMessage.TrySendData(16, -1, -1, null, num2, 0f, 0f, 0f, 0, 0, 0);
				NetMessage.TrySendData(42, -1, -1, null, num2, 0f, 0f, 0f, 0, 0, 0);
				NetMessage.TrySendData(50, -1, -1, null, num2, 0f, 0f, 0f, 0, 0, 0);
				NetMessage.TrySendData(147, -1, -1, null, num2, (float)player.CurrentLoadoutIndex, 0f, 0f, 0, 0, 0);
				for (int k = 0; k < 59; k++)
				{
					NetMessage.TrySendData(5, -1, -1, null, num2, (float)(PlayerItemSlotID.Inventory0 + k), 0f, 0f, 0, 0, 0);
				}
				MessageBuffer.TrySendingItemArray(num2, player.armor, PlayerItemSlotID.Armor0);
				MessageBuffer.TrySendingItemArray(num2, player.dye, PlayerItemSlotID.Dye0);
				MessageBuffer.TrySendingItemArray(num2, player.miscEquips, PlayerItemSlotID.Misc0);
				MessageBuffer.TrySendingItemArray(num2, player.miscDyes, PlayerItemSlotID.MiscDye0);
				MessageBuffer.TrySendingItemArray(num2, player.bank.item, PlayerItemSlotID.Bank1_0);
				MessageBuffer.TrySendingItemArray(num2, player.bank2.item, PlayerItemSlotID.Bank2_0);
				NetMessage.TrySendData(5, -1, -1, null, num2, (float)PlayerItemSlotID.TrashItem, 0f, 0f, 0, 0, 0);
				MessageBuffer.TrySendingItemArray(num2, player.bank3.item, PlayerItemSlotID.Bank3_0);
				MessageBuffer.TrySendingItemArray(num2, player.bank4.item, PlayerItemSlotID.Bank4_0);
				MessageBuffer.TrySendingItemArray(num2, player.Loadouts[0].Armor, PlayerItemSlotID.Loadout1_Armor_0);
				MessageBuffer.TrySendingItemArray(num2, player.Loadouts[0].Dye, PlayerItemSlotID.Loadout1_Dye_0);
				MessageBuffer.TrySendingItemArray(num2, player.Loadouts[1].Armor, PlayerItemSlotID.Loadout2_Armor_0);
				MessageBuffer.TrySendingItemArray(num2, player.Loadouts[1].Dye, PlayerItemSlotID.Loadout2_Dye_0);
				MessageBuffer.TrySendingItemArray(num2, player.Loadouts[2].Armor, PlayerItemSlotID.Loadout3_Armor_0);
				MessageBuffer.TrySendingItemArray(num2, player.Loadouts[2].Dye, PlayerItemSlotID.Loadout3_Dye_0);
				if (!string.IsNullOrWhiteSpace(Netplay.HostToken))
				{
					NetMessage.TrySendData(161, -1, -1, NetworkText.FromLiteral(Netplay.HostToken), 0, 0f, 0f, 0f, 0, 0, 0);
				}
				NetMessage.TrySendData(6, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
				if (Netplay.Connection.State == 2)
				{
					Netplay.Connection.State = 3;
					return;
				}
				return;
			}
			case 4:
			{
				int num3 = (int)this.reader.ReadByte();
				if (Main.netMode == 2)
				{
					num3 = this.whoAmI;
				}
				if (num3 == Main.myPlayer && !Main.ServerSideCharacter)
				{
					return;
				}
				Player player2 = Main.player[num3];
				player2.whoAmI = num3;
				player2.skinVariant = (int)this.reader.ReadByte();
				player2.skinVariant = (int)MathHelper.Clamp((float)player2.skinVariant, 0f, (float)(PlayerVariantID.Count - 1));
				player2.voiceVariant = (int)this.reader.ReadByte();
				player2.voiceVariant = Utils.Clamp<int>(player2.voiceVariant, 1, 4);
				player2.voicePitchOffset = this.reader.ReadSingle();
				if (float.IsNaN(player2.voicePitchOffset))
				{
					player2.voicePitchOffset = 0f;
				}
				player2.voicePitchOffset = Utils.Clamp<float>(player2.voicePitchOffset, -1f, 1f);
				player2.hair = (int)this.reader.ReadByte();
				if (player2.hair >= 228)
				{
					player2.hair = 0;
				}
				player2.name = this.reader.ReadString().Trim().Trim();
				player2.hairDye = this.reader.ReadByte();
				MessageBuffer.ReadAccessoryVisibility(this.reader, player2.hideVisibleAccessory);
				player2.hideMisc = this.reader.ReadByte();
				player2.hairColor = this.reader.ReadRGB();
				player2.skinColor = this.reader.ReadRGB();
				player2.eyeColor = this.reader.ReadRGB();
				player2.shirtColor = this.reader.ReadRGB();
				player2.underShirtColor = this.reader.ReadRGB();
				player2.pantsColor = this.reader.ReadRGB();
				player2.shoeColor = this.reader.ReadRGB();
				BitsByte bitsByte = this.reader.ReadByte();
				player2.difficulty = 0;
				if (bitsByte[0])
				{
					player2.difficulty = 1;
				}
				if (bitsByte[1])
				{
					player2.difficulty = 2;
				}
				if (bitsByte[3])
				{
					player2.difficulty = 3;
				}
				if (player2.difficulty > 3)
				{
					player2.difficulty = 3;
				}
				player2.extraAccessory = bitsByte[2];
				BitsByte bitsByte2 = this.reader.ReadByte();
				player2.UsingBiomeTorches = bitsByte2[0];
				player2.happyFunTorchTime = bitsByte2[1];
				player2.unlockedBiomeTorches = bitsByte2[2];
				player2.unlockedSuperCart = bitsByte2[3];
				player2.enabledSuperCart = bitsByte2[4];
				BitsByte bitsByte3 = this.reader.ReadByte();
				player2.usedAegisCrystal = bitsByte3[0];
				player2.usedAegisFruit = bitsByte3[1];
				player2.usedArcaneCrystal = bitsByte3[2];
				player2.usedGalaxyPearl = bitsByte3[3];
				player2.usedGummyWorm = bitsByte3[4];
				player2.usedAmbrosia = bitsByte3[5];
				player2.ateArtisanBread = bitsByte3[6];
				if (Main.netMode != 2)
				{
					return;
				}
				bool flag = false;
				if (Netplay.Clients[this.whoAmI].State < 10)
				{
					for (int l = 0; l < 255; l++)
					{
						if (l != num3 && player2.name == Main.player[l].name && Netplay.Clients[l].IsActive)
						{
							flag = true;
						}
					}
				}
				if (flag)
				{
					NetMessage.TrySendData(2, this.whoAmI, -1, NetworkText.FromKey(Lang.mp[5].Key, new object[]
					{
						player2.name
					}), 0, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				if (player2.name.Length > Player.nameLen)
				{
					NetMessage.TrySendData(2, this.whoAmI, -1, NetworkText.FromKey("Net.NameTooLong", new object[0]), 0, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				if (player2.name == "")
				{
					NetMessage.TrySendData(2, this.whoAmI, -1, NetworkText.FromKey("Net.EmptyName", new object[0]), 0, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				if (player2.difficulty == 3 && !Main.IsJourneyMode)
				{
					NetMessage.TrySendData(2, this.whoAmI, -1, NetworkText.FromKey("Net.PlayerIsCreativeAndWorldIsNotCreative", new object[0]), 0, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				if (player2.difficulty != 3 && Main.IsJourneyMode)
				{
					NetMessage.TrySendData(2, this.whoAmI, -1, NetworkText.FromKey("Net.PlayerIsNotCreativeAndWorldIsCreative", new object[0]), 0, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				Netplay.Clients[this.whoAmI].Name = player2.name;
				Netplay.Clients[this.whoAmI].Name = player2.name;
				NetMessage.TrySendData(4, -1, this.whoAmI, null, num3, 0f, 0f, 0f, 0, 0, 0);
				return;
			}
			case 5:
			{
				int num4 = (int)this.reader.ReadByte();
				int num5 = (int)this.reader.ReadInt16();
				int stack = (int)this.reader.ReadInt16();
				int prefixWeWant = (int)this.reader.ReadByte();
				int type = (int)this.reader.ReadInt16();
				BitsByte bitsByte4 = this.reader.ReadByte();
				bool favorited = bitsByte4[0];
				bool flag2 = bitsByte4[1];
				if (Main.netMode == 2)
				{
					num4 = this.whoAmI;
				}
				if (num4 == Main.myPlayer && !Main.ServerSideCharacter && !Main.player[num4].HasLockedInventory())
				{
					return;
				}
				Player player3 = Main.player[num4];
				Player obj = player3;
				lock (obj)
				{
					PlayerItemSlotID.SlotReference slot = new PlayerItemSlotID.SlotReference(player3, num5);
					PlayerItemSlotID.SlotReference slotReference = new PlayerItemSlotID.SlotReference(Main.clientPlayer, num5);
					Item item = new Item();
					item.SetDefaults(type, null);
					item.stack = stack;
					item.Prefix(prefixWeWant);
					item.favorited = favorited;
					slot.Item = item;
					if (num4 == Main.myPlayer && !Main.ServerSideCharacter)
					{
						slotReference.Item = item.Clone();
					}
					if (num5 >= PlayerItemSlotID.Bank4_0 && num5 < PlayerItemSlotID.Loadout1_Armor_0)
					{
						if (Main.netMode == 1 && player3.disableVoidBag == num5 - PlayerItemSlotID.Bank4_0)
						{
							player3.disableVoidBag = -1;
						}
					}
					else if (num5 <= 58)
					{
						if (num4 == Main.myPlayer && num5 == 58)
						{
							Main.mouseItem = item.Clone();
						}
						if (num4 == Main.myPlayer && Main.netMode == 1)
						{
							Main.player[num4].inventoryChestStack[num5] = false;
						}
					}
					if (Main.netMode == 1 && num4 == Main.myPlayer && flag2)
					{
						ItemSlot.IndicateBlockedSlot(slot);
					}
					bool[] canRelay = PlayerItemSlotID.CanRelay;
					if (Main.netMode == 2 && num4 == this.whoAmI && canRelay.IndexInRange(num5) && canRelay[num5])
					{
						NetMessage.TrySendData(5, -1, this.whoAmI, null, num4, (float)num5, 0f, 0f, 0, 0, 0);
					}
					return;
				}
				break;
			}
			case 6:
				break;
			case 7:
			{
				if (Main.netMode != 1)
				{
					return;
				}
				Main.time = (double)this.reader.ReadInt32();
				BitsByte bitsByte5 = this.reader.ReadByte();
				Main.dayTime = bitsByte5[0];
				Main.bloodMoon = bitsByte5[1];
				Main.eclipse = bitsByte5[2];
				Main.moonPhase = (int)this.reader.ReadByte();
				Main.maxTilesX = (int)this.reader.ReadInt16();
				Main.maxTilesY = (int)this.reader.ReadInt16();
				Main.spawnTileX = (int)this.reader.ReadInt16();
				Main.spawnTileY = (int)this.reader.ReadInt16();
				Main.worldSurface = (double)this.reader.ReadInt16();
				Main.rockLayer = (double)this.reader.ReadInt16();
				Main.ActiveWorldFileData.WorldId = this.reader.ReadInt32();
				Main.worldName = this.reader.ReadString();
				Main.GameMode = (int)this.reader.ReadByte();
				Main.ActiveWorldFileData.UniqueId = new Guid(this.reader.ReadBytes(16));
				Main.ActiveWorldFileData.WorldGeneratorVersion = this.reader.ReadUInt64();
				Main.moonType = (int)this.reader.ReadByte();
				WorldGen.setBG(0, (int)this.reader.ReadByte());
				WorldGen.setBG(10, (int)this.reader.ReadByte());
				WorldGen.setBG(11, (int)this.reader.ReadByte());
				WorldGen.setBG(12, (int)this.reader.ReadByte());
				WorldGen.setBG(1, (int)this.reader.ReadByte());
				WorldGen.setBG(2, (int)this.reader.ReadByte());
				WorldGen.setBG(3, (int)this.reader.ReadByte());
				WorldGen.setBG(4, (int)this.reader.ReadByte());
				WorldGen.setBG(5, (int)this.reader.ReadByte());
				WorldGen.setBG(6, (int)this.reader.ReadByte());
				WorldGen.setBG(7, (int)this.reader.ReadByte());
				WorldGen.setBG(8, (int)this.reader.ReadByte());
				WorldGen.setBG(9, (int)this.reader.ReadByte());
				Main.iceBackStyle = (int)this.reader.ReadByte();
				Main.jungleBackStyle = (int)this.reader.ReadByte();
				Main.hellBackStyle = (int)this.reader.ReadByte();
				Main.windSpeedTarget = this.reader.ReadSingle();
				Main.numClouds = (int)this.reader.ReadByte();
				for (int m = 0; m < 3; m++)
				{
					Main.treeX[m] = this.reader.ReadInt32();
				}
				for (int n = 0; n < 4; n++)
				{
					Main.treeStyle[n] = (int)this.reader.ReadByte();
				}
				for (int num6 = 0; num6 < 3; num6++)
				{
					Main.caveBackX[num6] = this.reader.ReadInt32();
				}
				for (int num7 = 0; num7 < 4; num7++)
				{
					Main.caveBackStyle[num7] = (int)this.reader.ReadByte();
				}
				WorldGen.TreeTops.SyncReceive(this.reader);
				WorldGen.BackgroundsCache.UpdateCache();
				Main.maxRaining = this.reader.ReadSingle();
				Main.raining = (Main.maxRaining > 0f);
				BitsByte bitsByte6 = this.reader.ReadByte();
				WorldGen.shadowOrbSmashed = bitsByte6[0];
				NPC.downedBoss1 = bitsByte6[1];
				NPC.downedBoss2 = bitsByte6[2];
				NPC.downedBoss3 = bitsByte6[3];
				Main.hardMode = bitsByte6[4];
				NPC.downedClown = bitsByte6[5];
				Main.ServerSideCharacter = bitsByte6[6];
				NPC.downedPlantBoss = bitsByte6[7];
				if (Main.ServerSideCharacter)
				{
					Main.ActivePlayerFileData.MarkAsServerSide();
				}
				BitsByte bitsByte7 = this.reader.ReadByte();
				NPC.downedMechBoss1 = bitsByte7[0];
				NPC.downedMechBoss2 = bitsByte7[1];
				NPC.downedMechBoss3 = bitsByte7[2];
				NPC.downedMechBossAny = bitsByte7[3];
				Main.cloudBGActive = (float)(bitsByte7[4] ? 1 : 0);
				WorldGen.crimson = bitsByte7[5];
				Main.pumpkinMoon = bitsByte7[6];
				Main.snowMoon = bitsByte7[7];
				BitsByte bitsByte8 = this.reader.ReadByte();
				Main.fastForwardTimeToDawn = bitsByte8[1];
				Main.UpdateTimeRate();
				bool flag4 = bitsByte8[2];
				NPC.downedSlimeKing = bitsByte8[3];
				NPC.downedQueenBee = bitsByte8[4];
				NPC.downedFishron = bitsByte8[5];
				NPC.downedMartians = bitsByte8[6];
				NPC.downedAncientCultist = bitsByte8[7];
				BitsByte bitsByte9 = this.reader.ReadByte();
				NPC.downedMoonlord = bitsByte9[0];
				NPC.downedHalloweenKing = bitsByte9[1];
				NPC.downedHalloweenTree = bitsByte9[2];
				NPC.downedChristmasIceQueen = bitsByte9[3];
				NPC.downedChristmasSantank = bitsByte9[4];
				NPC.downedChristmasTree = bitsByte9[5];
				NPC.downedGolemBoss = bitsByte9[6];
				BirthdayParty.ManualParty = bitsByte9[7];
				BitsByte bitsByte10 = this.reader.ReadByte();
				NPC.downedPirates = bitsByte10[0];
				NPC.downedFrost = bitsByte10[1];
				NPC.downedGoblins = bitsByte10[2];
				Sandstorm.Happening = bitsByte10[3];
				DD2Event.Ongoing = bitsByte10[4];
				DD2Event.DownedInvasionT1 = bitsByte10[5];
				DD2Event.DownedInvasionT2 = bitsByte10[6];
				DD2Event.DownedInvasionT3 = bitsByte10[7];
				BitsByte bitsByte11 = this.reader.ReadByte();
				NPC.combatBookWasUsed = bitsByte11[0];
				LanternNight.ManualLanterns = bitsByte11[1];
				NPC.downedTowerSolar = bitsByte11[2];
				NPC.downedTowerVortex = bitsByte11[3];
				NPC.downedTowerNebula = bitsByte11[4];
				NPC.downedTowerStardust = bitsByte11[5];
				Main.forceHalloweenForToday = bitsByte11[6];
				Main.forceXMasForToday = bitsByte11[7];
				BitsByte bitsByte12 = this.reader.ReadByte();
				NPC.boughtCat = bitsByte12[0];
				NPC.boughtDog = bitsByte12[1];
				NPC.boughtBunny = bitsByte12[2];
				NPC.freeCake = bitsByte12[3];
				Main.drunkWorld = bitsByte12[4];
				NPC.downedEmpressOfLight = bitsByte12[5];
				NPC.downedQueenSlime = bitsByte12[6];
				Main.getGoodWorld = bitsByte12[7];
				BitsByte bitsByte13 = this.reader.ReadByte();
				Main.tenthAnniversaryWorld = bitsByte13[0];
				Main.dontStarveWorld = bitsByte13[1];
				NPC.downedDeerclops = bitsByte13[2];
				Main.notTheBeesWorld = bitsByte13[3];
				Main.remixWorld = bitsByte13[4];
				NPC.unlockedSlimeBlueSpawn = bitsByte13[5];
				NPC.combatBookVolumeTwoWasUsed = bitsByte13[6];
				NPC.peddlersSatchelWasUsed = bitsByte13[7];
				BitsByte bitsByte14 = this.reader.ReadByte();
				NPC.unlockedSlimeGreenSpawn = bitsByte14[0];
				NPC.unlockedSlimeOldSpawn = bitsByte14[1];
				NPC.unlockedSlimePurpleSpawn = bitsByte14[2];
				NPC.unlockedSlimeRainbowSpawn = bitsByte14[3];
				NPC.unlockedSlimeRedSpawn = bitsByte14[4];
				NPC.unlockedSlimeYellowSpawn = bitsByte14[5];
				NPC.unlockedSlimeCopperSpawn = bitsByte14[6];
				Main.fastForwardTimeToDusk = bitsByte14[7];
				BitsByte bitsByte15 = this.reader.ReadByte();
				Main.noTrapsWorld = bitsByte15[0];
				Main.zenithWorld = bitsByte15[1];
				NPC.unlockedTruffleSpawn = bitsByte15[2];
				Main.vampireSeed = bitsByte15[3];
				Main.infectedSeed = bitsByte15[4];
				Main.teamBasedSpawnsSeed = bitsByte15[5];
				Main.skyblockWorld = bitsByte15[6];
				Main.dualDungeonsSeed = bitsByte15[7];
				WorldGen.Skyblock.lowTiles = this.reader.ReadByte()[0];
				Main.sundialCooldown = (int)this.reader.ReadByte();
				Main.moondialCooldown = (int)this.reader.ReadByte();
				WorldGen.SavedOreTiers.Copper = (int)this.reader.ReadInt16();
				WorldGen.SavedOreTiers.Iron = (int)this.reader.ReadInt16();
				WorldGen.SavedOreTiers.Silver = (int)this.reader.ReadInt16();
				WorldGen.SavedOreTiers.Gold = (int)this.reader.ReadInt16();
				WorldGen.SavedOreTiers.Cobalt = (int)this.reader.ReadInt16();
				WorldGen.SavedOreTiers.Mythril = (int)this.reader.ReadInt16();
				WorldGen.SavedOreTiers.Adamantite = (int)this.reader.ReadInt16();
				if (flag4)
				{
					Main.StartSlimeRain(false);
				}
				else
				{
					Main.StopSlimeRain(true);
				}
				Main.invasionType = (int)this.reader.ReadSByte();
				Main.LobbyId = this.reader.ReadUInt64();
				Sandstorm.IntendedSeverity = this.reader.ReadSingle();
				ExtraSpawnPointManager.Read(this.reader, true);
				if (Netplay.Connection.State == 3)
				{
					Main.windSpeedCurrent = Main.windSpeedTarget;
					Netplay.Connection.State = 4;
				}
				Main.checkHalloween();
				Main.checkXMas();
				return;
			}
			case 8:
			{
				if (Main.netMode != 2)
				{
					return;
				}
				NetMessage.TrySendData(7, this.whoAmI, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
				int num8 = this.reader.ReadInt32();
				int num9 = this.reader.ReadInt32();
				int num10 = (int)this.reader.ReadByte();
				bool flag5 = true;
				if (num8 == -1 || num9 == -1)
				{
					flag5 = false;
				}
				else if (num8 < 10 || num8 > Main.maxTilesX - 10)
				{
					flag5 = false;
				}
				else if (num9 < 10 || num9 > Main.maxTilesY - 10)
				{
					flag5 = false;
				}
				bool flag6 = false;
				if (Main.teamBasedSpawnsSeed && num10 != 0)
				{
					flag6 = true;
				}
				int num11 = Netplay.GetSectionX(Main.spawnTileX) - 2;
				int num12 = Netplay.GetSectionY(Main.spawnTileY) - 1;
				int num13 = num11 + 5;
				int num14 = num12 + 3;
				if (num11 < 0)
				{
					num11 = 0;
				}
				if (num13 >= Main.maxSectionsX)
				{
					num13 = Main.maxSectionsX;
				}
				if (num12 < 0)
				{
					num12 = 0;
				}
				if (num14 >= Main.maxSectionsY)
				{
					num14 = Main.maxSectionsY;
				}
				int num15 = (num13 - num11) * (num14 - num12);
				List<Point> list = new List<Point>();
				for (int num16 = num11; num16 < num13; num16++)
				{
					for (int num17 = num12; num17 < num14; num17++)
					{
						list.Add(new Point(num16, num17));
					}
				}
				int num18 = -1;
				int num19 = -1;
				if (flag5)
				{
					num8 = Netplay.GetSectionX(num8) - 2;
					num9 = Netplay.GetSectionY(num9) - 1;
					num18 = num8 + 5;
					num19 = num9 + 3;
					if (num8 < 0)
					{
						num8 = 0;
					}
					if (num18 >= Main.maxSectionsX)
					{
						num18 = Main.maxSectionsX - 1;
					}
					if (num9 < 0)
					{
						num9 = 0;
					}
					if (num19 >= Main.maxSectionsY)
					{
						num19 = Main.maxSectionsY - 1;
					}
					for (int num20 = num8; num20 <= num18; num20++)
					{
						for (int num21 = num9; num21 <= num19; num21++)
						{
							if (num20 < num11 || num20 >= num13 || num21 < num12 || num21 >= num14)
							{
								list.Add(new Point(num20, num21));
								num15++;
							}
						}
					}
				}
				int num22 = -1;
				int num23 = -1;
				int num24 = -1;
				int num25 = -1;
				if (flag6)
				{
					Point zero = Point.Zero;
					if (ExtraSpawnPointManager.TryGetExtraSpawnPointForTeam(num10, out zero))
					{
						num22 = zero.X;
						num23 = zero.Y;
						num22 = Netplay.GetSectionX(num22) - 2;
						num23 = Netplay.GetSectionY(num23) - 1;
						num24 = num22 + 5;
						num25 = num23 + 3;
						if (num22 < 0)
						{
							num22 = 0;
						}
						if (num24 >= Main.maxSectionsX)
						{
							num24 = Main.maxSectionsX - 1;
						}
						if (num23 < 0)
						{
							num23 = 0;
						}
						if (num25 >= Main.maxSectionsY)
						{
							num25 = Main.maxSectionsY - 1;
						}
						for (int num26 = num22; num26 <= num24; num26++)
						{
							for (int num27 = num23; num27 <= num25; num27++)
							{
								if ((num26 < num11 || num26 >= num13 || num27 < num12 || num27 >= num14) && (num26 < num8 || num26 >= num18 || num27 < num9 || num27 >= num19))
								{
									list.Add(new Point(num26, num27));
									num15++;
								}
							}
						}
					}
					else
					{
						flag6 = false;
					}
				}
				List<Point> list2;
				PortalHelper.SyncPortalsOnPlayerJoin(this.whoAmI, 1, list, out list2);
				num15 += list2.Count;
				if (Netplay.Clients[this.whoAmI].State == 2)
				{
					Netplay.Clients[this.whoAmI].State = 3;
				}
				NetMessage.TrySendData(9, this.whoAmI, -1, Lang.inter[44].ToNetworkText(), num15, 0f, 0f, 0f, 0, 0, 0);
				Netplay.Clients[this.whoAmI].StatusText2 = Language.GetTextValue("Net.IsReceivingTileData");
				Netplay.Clients[this.whoAmI].StatusMax += num15;
				for (int num28 = num11; num28 < num13; num28++)
				{
					for (int num29 = num12; num29 < num14; num29++)
					{
						NetMessage.SendSection(this.whoAmI, num28, num29);
					}
				}
				if (flag5)
				{
					for (int num30 = num8; num30 <= num18; num30++)
					{
						for (int num31 = num9; num31 <= num19; num31++)
						{
							NetMessage.SendSection(this.whoAmI, num30, num31);
						}
					}
				}
				if (flag6)
				{
					for (int num32 = num22; num32 <= num24; num32++)
					{
						for (int num33 = num23; num33 <= num25; num33++)
						{
							NetMessage.SendSection(this.whoAmI, num32, num33);
						}
					}
				}
				for (int num34 = 0; num34 < list2.Count; num34++)
				{
					NetMessage.SendSection(this.whoAmI, list2[num34].X, list2[num34].Y);
				}
				for (int num35 = 0; num35 < 400; num35++)
				{
					if (Main.item[num35].active)
					{
						NetMessage.TrySendData(21, this.whoAmI, -1, null, num35, 0f, 0f, 0f, 0, 0, 0);
						NetMessage.TrySendData(22, this.whoAmI, -1, null, num35, 0f, 0f, 0f, 0, 0, 0);
					}
				}
				for (int num36 = 0; num36 < Main.maxNPCs; num36++)
				{
					if (Main.npc[num36].active)
					{
						NetMessage.TrySendData(23, this.whoAmI, -1, null, num36, 0f, 0f, 0f, 0, 0, 0);
						NetMessage.TrySendData(54, this.whoAmI, -1, null, num36, 0f, 0f, 0f, 0, 0, 0);
					}
				}
				for (int num37 = 0; num37 < 1000; num37++)
				{
					if (Main.projectile[num37].active && (Main.projPet[Main.projectile[num37].type] || Main.projectile[num37].netImportant))
					{
						NetMessage.TrySendData(27, this.whoAmI, -1, null, num37, 0f, 0f, 0f, 0, 0, 0);
					}
				}
				NetManager.Instance.SendToClient(BannerSystem.NetBannersModule.WriteFullState(), this.whoAmI);
				NetMessage.TrySendData(57, this.whoAmI, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
				NetMessage.TrySendData(103, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
				NetMessage.TrySendData(101, this.whoAmI, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
				NetMessage.TrySendData(136, this.whoAmI, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
				Main.BestiaryTracker.OnPlayerJoining(this.whoAmI);
				CreativePowerManager.Instance.SyncThingsToJoiningPlayer(this.whoAmI);
				Main.PylonSystem.OnPlayerJoining(this.whoAmI);
				NetMessage.TrySendData(49, this.whoAmI, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
				return;
			}
			case 9:
			{
				if (Main.netMode != 1)
				{
					return;
				}
				Netplay.Connection.StatusMax += this.reader.ReadInt32();
				Netplay.Connection.StatusText = NetworkText.Deserialize(this.reader).ToString();
				BitsByte bitsByte16 = this.reader.ReadByte();
				BitsByte serverSpecialFlags = Netplay.Connection.ServerSpecialFlags;
				serverSpecialFlags[0] = bitsByte16[0];
				serverSpecialFlags[1] = bitsByte16[1];
				Netplay.Connection.ServerSpecialFlags = serverSpecialFlags;
				return;
			}
			case 10:
				if (Main.netMode != 1)
				{
					return;
				}
				NetMessage.DecompressTileBlock(this.reader.BaseStream);
				return;
			case 11:
				if (Main.netMode != 1)
				{
					return;
				}
				WorldGen.SectionTileFrame((int)this.reader.ReadInt16(), (int)this.reader.ReadInt16(), (int)this.reader.ReadInt16(), (int)this.reader.ReadInt16());
				return;
			case 12:
			{
				int num38 = (int)this.reader.ReadByte();
				if (Main.netMode == 2)
				{
					num38 = this.whoAmI;
				}
				Player player4 = Main.player[num38];
				player4.SpawnX = (int)this.reader.ReadInt16();
				player4.SpawnY = (int)this.reader.ReadInt16();
				player4.respawnTimer = this.reader.ReadInt32();
				player4.numberOfDeathsPVE = (int)this.reader.ReadInt16();
				player4.numberOfDeathsPVP = (int)this.reader.ReadInt16();
				player4.team = (int)this.reader.ReadByte();
				if (player4.respawnTimer > 0)
				{
					player4.dead = true;
				}
				PlayerSpawnContext playerSpawnContext = (PlayerSpawnContext)this.reader.ReadByte();
				player4.Spawn(playerSpawnContext);
				if (Main.netMode != 2 || Netplay.Clients[this.whoAmI].State < 3)
				{
					return;
				}
				if (Netplay.Clients[this.whoAmI].State != 3)
				{
					NetMessage.TrySendData(12, -1, this.whoAmI, null, this.whoAmI, (float)((byte)playerSpawnContext), 0f, 0f, 0, 0, 0);
					return;
				}
				Netplay.Clients[this.whoAmI].State = 10;
				NetMessage.buffer[this.whoAmI].broadcast = true;
				NetMessage.SyncConnectedPlayer(this.whoAmI);
				bool flag7 = NetMessage.DoesPlayerSlotCountAsAHost(this.whoAmI);
				Main.countsAsHostForGameplay[this.whoAmI] = flag7;
				if (NetMessage.DoesPlayerSlotCountAsAHost(this.whoAmI))
				{
					NetMessage.TrySendData(139, this.whoAmI, -1, null, this.whoAmI, (float)flag7.ToInt(), 0f, 0f, 0, 0, 0);
				}
				NetMessage.TrySendData(12, -1, this.whoAmI, null, this.whoAmI, (float)((byte)playerSpawnContext), 0f, 0f, 0, 0, 0);
				NetMessage.TrySendData(129, this.whoAmI, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
				NetMessage.greetPlayer(this.whoAmI);
				if (Main.player[num38].unlockedBiomeTorches)
				{
					NPC npc = new NPC();
					npc.SetDefaults(664, default(NPCSpawnParams));
					Main.BestiaryTracker.Kills.RegisterKill(npc);
					return;
				}
				return;
			}
			case 13:
			{
				int num39 = (int)this.reader.ReadByte();
				if (num39 == Main.myPlayer && !Main.ServerSideCharacter)
				{
					return;
				}
				if (Main.netMode == 2)
				{
					num39 = this.whoAmI;
				}
				Player player5 = Main.player[num39];
				BitsByte bitsByte17 = this.reader.ReadByte();
				BitsByte bitsByte18 = this.reader.ReadByte();
				BitsByte bitsByte19 = this.reader.ReadByte();
				BitsByte bitsByte20 = this.reader.ReadByte();
				player5.controlUp = bitsByte17[0];
				player5.controlDown = bitsByte17[1];
				player5.controlLeft = bitsByte17[2];
				player5.controlRight = bitsByte17[3];
				player5.controlJump = bitsByte17[4];
				player5.controlUseItem = bitsByte17[5];
				player5.direction = (bitsByte17[6] ? 1 : -1);
				if (bitsByte18[0])
				{
					player5.pulley = true;
					player5.pulleyDir = (bitsByte18[1] ? 2 : 1);
				}
				else
				{
					player5.pulley = false;
				}
				player5.vortexStealthActive = bitsByte18[3];
				player5.gravDir = (float)(bitsByte18[4] ? 1 : -1);
				player5.TryTogglingShield(bitsByte18[5]);
				player5.ghost = bitsByte18[6];
				player5.selectedItemState.Select((int)this.reader.ReadByte());
				Vector2 vector = this.reader.ReadVector2();
				Vector2 velocity = Vector2.Zero;
				if (bitsByte18[2])
				{
					velocity = this.reader.ReadVector2();
				}
				if (player5.unacknowledgedTeleports > 0)
				{
					vector = player5.position;
					velocity = player5.velocity;
				}
				if (Main.netMode == 1 && player5.position != Vector2.Zero)
				{
					player5.netOffset += player5.position - vector;
					if (player5.netOffset.Length() > (float)Main.multiplayerNPCSmoothingRange)
					{
						player5.netOffset = Vector2.Zero;
					}
					if (player5.netOffset != Vector2.Zero && DebugOptions.ShowNetOffsetDust && Vector2.Distance(vector, player5.position) > 4f)
					{
						Dust.QuickDustLine(vector, player5.position, 20f, Color.Red);
					}
				}
				player5.position = vector;
				player5.velocity = velocity;
				Vector2 position = player5.position;
				if (bitsByte18[7])
				{
					player5.mount.SetMount((int)this.reader.ReadUInt16(), player5);
				}
				else
				{
					player5.mount.Dismount(player5, false);
				}
				if (bitsByte19[6])
				{
					player5.PotionOfReturnOriginalUsePosition = new Vector2?(this.reader.ReadVector2());
					player5.PotionOfReturnHomePosition = new Vector2?(this.reader.ReadVector2());
				}
				else
				{
					player5.PotionOfReturnOriginalUsePosition = null;
					player5.PotionOfReturnHomePosition = null;
				}
				player5.tryKeepingHoveringUp = bitsByte19[0];
				player5.IsVoidVaultEnabled = bitsByte19[1];
				player5.sitting.isSitting = bitsByte19[2];
				player5.downedDD2EventAnyDifficulty = bitsByte19[3];
				player5.petting.isPetting = bitsByte19[4];
				player5.petting.isPetSmall = bitsByte19[5];
				player5.tryKeepingHoveringDown = bitsByte19[7];
				player5.sleeping.SetIsSleepingAndAdjustPlayerRotation(player5, bitsByte20[0]);
				player5.autoReuseAllWeapons = bitsByte20[1];
				player5.controlDownHold = bitsByte20[2];
				player5.isOperatingAnotherEntity = bitsByte20[3];
				player5.controlUseTile = bitsByte20[4];
				player5.netCameraTarget = (bitsByte20[5] ? new Vector2?(this.reader.ReadVector2()) : null);
				player5.lastItemUseAttemptSuccess = bitsByte20[6];
				Utils.Swap<Vector2>(ref position, ref player5.position);
				if (Main.netMode == 2 && Netplay.Clients[this.whoAmI].State == 10)
				{
					NetMessage.TrySendData(13, -1, this.whoAmI, null, num39, 0f, 0f, 0f, 0, 0, 0);
				}
				Utils.Swap<Vector2>(ref position, ref player5.position);
				return;
			}
			case 14:
			{
				int num40 = (int)this.reader.ReadByte();
				int num41 = (int)this.reader.ReadByte();
				if (Main.netMode != 1)
				{
					return;
				}
				bool active = Main.player[num40].active;
				if (num41 == 1)
				{
					if (!Main.player[num40].active)
					{
						Main.player[num40] = new Player();
					}
					Main.player[num40].active = true;
				}
				else
				{
					Main.player[num40].active = false;
				}
				if (active == Main.player[num40].active)
				{
					return;
				}
				if (Main.player[num40].active)
				{
					Player.Hooks.PlayerConnect(num40);
					return;
				}
				Player.Hooks.PlayerDisconnect(num40);
				return;
			}
			case 15:
			case 25:
			case 26:
			case 44:
			case 67:
			case 83:
			case 93:
				return;
			case 16:
			{
				int num42 = (int)this.reader.ReadByte();
				if (num42 == Main.myPlayer && !Main.ServerSideCharacter)
				{
					return;
				}
				if (Main.netMode == 2)
				{
					num42 = this.whoAmI;
				}
				Player player6 = Main.player[num42];
				player6.statLife = (int)this.reader.ReadInt16();
				player6.statLifeMax = (int)this.reader.ReadInt16();
				if (player6.statLifeMax < 20)
				{
					player6.statLifeMax = 20;
				}
				player6.dead = (player6.statLife <= 0);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(16, -1, this.whoAmI, null, num42, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				return;
			}
			case 17:
			{
				byte b3 = this.reader.ReadByte();
				int num43 = (int)this.reader.ReadInt16();
				int num44 = (int)this.reader.ReadInt16();
				short num45 = this.reader.ReadInt16();
				int num46 = (int)this.reader.ReadByte();
				bool flag8 = num45 == 1;
				if (!WorldGen.InWorld(num43, num44, 3))
				{
					return;
				}
				if (Main.tile[num43, num44] == null)
				{
					Main.tile[num43, num44] = new Tile();
				}
				if (Main.netMode == 2)
				{
					if (!flag8)
					{
						if (b3 == 0 || b3 == 2 || b3 == 4)
						{
							Netplay.Clients[this.whoAmI].SpamDeleteBlock += 1f;
						}
						if (b3 == 1 || b3 == 3)
						{
							Netplay.Clients[this.whoAmI].SpamAddBlock += 1f;
						}
					}
					if (!Netplay.Clients[this.whoAmI].TileSections[Netplay.GetSectionX(num43), Netplay.GetSectionY(num44)])
					{
						flag8 = true;
					}
				}
				MapUpdateQueue.Add(num43, num44);
				if (b3 == 0)
				{
					WorldGen.KillTile(num43, num44, flag8, false, false);
					if (Main.netMode == 1 && !flag8)
					{
						HitTile.ClearAllTilesAtThisLocation(num43, num44);
					}
				}
				bool flag9 = false;
				if (b3 == 1)
				{
					bool forced = true;
					if (WorldGen.CheckTileBreakability2_ShouldTileSurvive(num43, num44))
					{
						flag9 = true;
						forced = false;
					}
					WorldGen.PlaceTile(num43, num44, (int)num45, false, forced, -1, num46);
				}
				if (b3 == 2)
				{
					WorldGen.KillWall(num43, num44, flag8);
				}
				if (b3 == 3)
				{
					WorldGen.PlaceWall(num43, num44, (int)num45, false);
				}
				if (b3 == 4)
				{
					WorldGen.KillTile(num43, num44, flag8, false, true);
				}
				if (b3 == 5)
				{
					WorldGen.PlaceWire(num43, num44);
				}
				if (b3 == 6)
				{
					WorldGen.KillWire(num43, num44);
				}
				if (b3 == 7)
				{
					WorldGen.PoundTile(num43, num44);
				}
				if (b3 == 8)
				{
					WorldGen.PlaceActuator(num43, num44);
				}
				if (b3 == 9)
				{
					WorldGen.KillActuator(num43, num44);
				}
				if (b3 == 10)
				{
					WorldGen.PlaceWire2(num43, num44);
				}
				if (b3 == 11)
				{
					WorldGen.KillWire2(num43, num44);
				}
				if (b3 == 12)
				{
					WorldGen.PlaceWire3(num43, num44);
				}
				if (b3 == 13)
				{
					WorldGen.KillWire3(num43, num44);
				}
				if (b3 == 14)
				{
					WorldGen.SlopeTile(num43, num44, (int)num45, false, true);
				}
				if (b3 == 15)
				{
					Minecart.FrameTrack(num43, num44, true, false);
				}
				if (b3 == 16)
				{
					WorldGen.PlaceWire4(num43, num44);
				}
				if (b3 == 17)
				{
					WorldGen.KillWire4(num43, num44);
				}
				if (b3 == 18)
				{
					Wiring.SetCurrentUser(this.whoAmI);
					Wiring.PokeLogicGate(num43, num44);
					Wiring.SetCurrentUser(-1);
					return;
				}
				if (b3 == 19)
				{
					Wiring.SetCurrentUser(this.whoAmI);
					Wiring.Actuate(num43, num44);
					Wiring.SetCurrentUser(-1);
					return;
				}
				if (b3 == 20)
				{
					if (!WorldGen.InWorld(num43, num44, 2))
					{
						return;
					}
					int type2 = (int)Main.tile[num43, num44].type;
					WorldGen.KillTile(num43, num44, flag8, false, false);
					num45 = ((Main.tile[num43, num44].active() && (int)Main.tile[num43, num44].type == type2) ? 1 : 0);
					if (Main.netMode == 2)
					{
						NetMessage.TrySendData(17, -1, -1, null, (int)b3, (float)num43, (float)num44, (float)num45, num46, 0, 0);
						return;
					}
					return;
				}
				else
				{
					if (b3 == 21)
					{
						WorldGen.ReplaceTile(num43, num44, (int)((ushort)num45), num46);
					}
					if (b3 == 22)
					{
						WorldGen.ReplaceWall(num43, num44, (ushort)num45);
					}
					if (b3 == 23 && WorldGen.CanPoundTile(num43, num44))
					{
						Main.tile[num43, num44].slope((byte)num45);
						WorldGen.PoundTile(num43, num44);
					}
					if (Main.netMode != 2)
					{
						return;
					}
					if (flag9)
					{
						NetMessage.SendTileSquare(-1, num43, num44, 5, TileChangeType.None);
						return;
					}
					if ((b3 != 1 && b3 != 21) || !TileID.Sets.Falling[(int)num45] || Main.tile[num43, num44].active())
					{
						NetMessage.TrySendData(17, -1, this.whoAmI, null, (int)b3, (float)num43, (float)num44, (float)num45, num46, 0, 0);
						return;
					}
					return;
				}
				break;
			}
			case 18:
				if (Main.netMode != 1)
				{
					return;
				}
				Main.dayTime = (this.reader.ReadByte() == 1);
				Main.time = (double)this.reader.ReadInt32();
				Main.sunModY = this.reader.ReadInt16();
				Main.moonModY = this.reader.ReadInt16();
				return;
			case 19:
			{
				byte b4 = this.reader.ReadByte();
				int num47 = (int)this.reader.ReadInt16();
				int num48 = (int)this.reader.ReadInt16();
				if (!WorldGen.InWorld(num47, num48, 3))
				{
					return;
				}
				int num49 = (this.reader.ReadByte() == 0) ? -1 : 1;
				if (b4 == 0)
				{
					WorldGen.OpenDoor(num47, num48, num49);
				}
				else if (b4 == 1)
				{
					WorldGen.CloseDoor(num47, num48, true);
				}
				else if (b4 == 2)
				{
					WorldGen.ShiftTrapdoor(num47, num48, num49 == 1, 1);
				}
				else if (b4 == 3)
				{
					WorldGen.ShiftTrapdoor(num47, num48, num49 == 1, 0);
				}
				else if (b4 == 4)
				{
					WorldGen.ShiftTallGate(num47, num48, false, true);
				}
				else if (b4 == 5)
				{
					WorldGen.ShiftTallGate(num47, num48, true, true);
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(19, -1, this.whoAmI, null, (int)b4, (float)num47, (float)num48, (float)((num49 == 1) ? 1 : 0), 0, 0, 0);
					return;
				}
				return;
			}
			case 20:
			{
				int num50 = (int)this.reader.ReadInt16();
				int num51 = (int)this.reader.ReadInt16();
				ushort num52 = (ushort)this.reader.ReadByte();
				ushort num53 = (ushort)this.reader.ReadByte();
				byte b5 = this.reader.ReadByte();
				if (!WorldGen.InWorld(num50, num51, 3))
				{
					return;
				}
				TileChangeType type3 = TileChangeType.None;
				if (Enum.IsDefined(typeof(TileChangeType), b5))
				{
					type3 = (TileChangeType)b5;
				}
				if (MessageBuffer.OnTileChangeReceived != null)
				{
					MessageBuffer.OnTileChangeReceived(num50, num51, (int)Math.Max(num52, num53), type3);
				}
				BitsByte bitsByte21 = 0;
				BitsByte bitsByte22 = 0;
				BitsByte bitsByte23 = 0;
				for (int num54 = num50; num54 < num50 + (int)num52; num54++)
				{
					for (int num55 = num51; num55 < num51 + (int)num53; num55++)
					{
						if (Main.tile[num54, num55] == null)
						{
							Main.tile[num54, num55] = new Tile();
						}
						Tile tile = Main.tile[num54, num55];
						bool flag10 = tile.active();
						bitsByte21 = this.reader.ReadByte();
						bitsByte22 = this.reader.ReadByte();
						bitsByte23 = this.reader.ReadByte();
						tile.active(bitsByte21[0]);
						tile.wall = (ushort)(bitsByte21[2] ? 1 : 0);
						bool flag11 = bitsByte21[3];
						if (Main.netMode != 2)
						{
							tile.liquid = (flag11 ? 1 : 0);
						}
						tile.wire(bitsByte21[4]);
						tile.halfBrick(bitsByte21[5]);
						tile.actuator(bitsByte21[6]);
						tile.inActive(bitsByte21[7]);
						tile.wire2(bitsByte22[0]);
						tile.wire3(bitsByte22[1]);
						if (bitsByte22[2])
						{
							tile.color(this.reader.ReadByte());
						}
						if (bitsByte22[3])
						{
							tile.wallColor(this.reader.ReadByte());
						}
						if (tile.active())
						{
							int type4 = (int)tile.type;
							tile.type = this.reader.ReadUInt16();
							if (Main.tileFrameImportant[(int)tile.type])
							{
								tile.frameX = this.reader.ReadInt16();
								tile.frameY = this.reader.ReadInt16();
							}
							else if (!flag10 || (int)tile.type != type4)
							{
								tile.frameX = -1;
								tile.frameY = -1;
							}
							byte b6 = 0;
							if (bitsByte22[4])
							{
								b6 += 1;
							}
							if (bitsByte22[5])
							{
								b6 += 2;
							}
							if (bitsByte22[6])
							{
								b6 += 4;
							}
							tile.slope(b6);
						}
						tile.wire4(bitsByte22[7]);
						tile.fullbrightBlock(bitsByte23[0]);
						tile.fullbrightWall(bitsByte23[1]);
						tile.invisibleBlock(bitsByte23[2]);
						tile.invisibleWall(bitsByte23[3]);
						if (tile.wall > 0)
						{
							tile.wall = this.reader.ReadUInt16();
						}
						if (flag11)
						{
							tile.liquid = this.reader.ReadByte();
							tile.liquidType((int)this.reader.ReadByte());
						}
					}
				}
				WorldGen.RangeFrame(num50, num51, num50 + (int)num52, num51 + (int)num53);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData((int)b, -1, this.whoAmI, null, num50, (float)num51, (float)num52, (float)num53, (int)b5, 0, 0);
					return;
				}
				return;
			}
			case 21:
			case 90:
			case 145:
			case 148:
			{
				int num56 = (int)this.reader.ReadInt16();
				Vector2 vector2 = this.reader.ReadVector2();
				Vector2 velocity2 = this.reader.ReadVector2();
				int stack2 = (int)this.reader.ReadInt16();
				int prefix = (int)this.reader.ReadByte();
				BitsByte bb = this.reader.ReadByte();
				bool flag12 = bb[0];
				bool flag13 = bb[1];
				int num57 = (int)this.reader.ReadInt16();
				bool shimmered = false;
				float shimmerTime = 0f;
				int timeLeftInWhichTheItemCannotBeTakenByEnemies = 0;
				if (b == 145)
				{
					shimmered = this.reader.ReadBoolean();
					shimmerTime = this.reader.ReadSingle();
				}
				if (b == 148)
				{
					timeLeftInWhichTheItemCannotBeTakenByEnemies = (int)this.reader.ReadByte();
				}
				WorldItem worldItem = Main.item[num56];
				if (Main.netMode == 1)
				{
					ItemSyncPersistentStats itemSyncPersistentStats = default(ItemSyncPersistentStats);
					itemSyncPersistentStats.CopyFrom(worldItem);
					bool newAndShiny = (worldItem.newAndShiny || worldItem.type != num57) && ItemSlot.Options.HighlightNewItems && (num57 < 0 || num57 >= (int)ItemID.Count || !ItemID.Sets.NeverAppearsAsNewInInventory[num57]);
					worldItem.SetDefaults(num57);
					worldItem.newAndShiny = newAndShiny;
					worldItem.Prefix(prefix);
					worldItem.stack = stack2;
					worldItem.position = vector2;
					worldItem.velocity = velocity2;
					worldItem.shimmered = shimmered;
					worldItem.shimmerTime = shimmerTime;
					if (b == 90)
					{
						worldItem.instanced = true;
						worldItem.playerIndexTheItemIsReservedFor = Main.myPlayer;
						worldItem.keepTime = 600;
					}
					else if (flag13)
					{
						worldItem.keepTime = 100;
					}
					worldItem.timeLeftInWhichTheItemCannotBeTakenByEnemies = timeLeftInWhichTheItemCannotBeTakenByEnemies;
					worldItem.wet = Collision.WetCollision(worldItem.position, worldItem.width, worldItem.height);
					itemSyncPersistentStats.PasteInto(worldItem);
					return;
				}
				if (Main.timeItemSlotCannotBeReusedFor[num56] > 0)
				{
					return;
				}
				bool flag14 = num56 == 400;
				if (flag14)
				{
					Item item2 = new Item();
					item2.SetDefaults(num57, null);
					num56 = Item.NewItem(new EntitySource_Sync(), (int)vector2.X, (int)vector2.Y, item2.width, item2.height, item2.type, stack2, true, 0, false);
					worldItem = Main.item[num56];
					flag13 = (bb[1] = !flag12);
				}
				else
				{
					int timeSinceTheItemHasBeenReservedForSomeone = worldItem.timeSinceTheItemHasBeenReservedForSomeone;
					if (worldItem.playerIndexTheItemIsReservedFor != this.whoAmI)
					{
						timeSinceTheItemHasBeenReservedForSomeone = 0;
					}
					worldItem.playerIndexTheItemIsReservedFor = 255;
					worldItem.SetDefaults(num57);
					worldItem.playerIndexTheItemIsReservedFor = this.whoAmI;
					worldItem.timeSinceTheItemHasBeenReservedForSomeone = timeSinceTheItemHasBeenReservedForSomeone;
				}
				worldItem.Prefix(prefix);
				worldItem.stack = stack2;
				worldItem.position = vector2;
				worldItem.velocity = velocity2;
				worldItem.timeLeftInWhichTheItemCannotBeTakenByEnemies = timeLeftInWhichTheItemCannotBeTakenByEnemies;
				if (b == 145)
				{
					worldItem.shimmered = shimmered;
					worldItem.shimmerTime = shimmerTime;
				}
				if (flag13)
				{
					worldItem.ownIgnore = this.whoAmI;
					worldItem.ownTime = 100;
				}
				if (flag14)
				{
					NetMessage.TrySendData((int)b, -1, -1, null, num56, (float)bb, 0f, 0f, 0, 0, 0);
					Main.item[num56].FindOwner();
					return;
				}
				NetMessage.TrySendData((int)b, -1, this.whoAmI, null, num56, 0f, 0f, 0f, 0, 0, 0);
				return;
			}
			case 22:
			{
				int num58 = (int)this.reader.ReadInt16();
				int num59 = (int)this.reader.ReadByte();
				Vector2 position2 = this.reader.ReadVector2();
				WorldItem worldItem2 = Main.item[num58];
				if (Main.netMode == 2)
				{
					return;
				}
				worldItem2.playerIndexTheItemIsReservedFor = num59;
				worldItem2.position = position2;
				if (num59 == Main.myPlayer)
				{
					worldItem2.keepTime = Math.Max(worldItem2.keepTime, 15);
					return;
				}
				worldItem2.keepTime = 0;
				return;
			}
			case 23:
			{
				if (Main.netMode != 1)
				{
					return;
				}
				int num60 = (int)this.reader.ReadInt16();
				Vector2 vector3 = this.reader.ReadVector2();
				Vector2 velocity3 = this.reader.ReadVector2();
				int num61 = (int)this.reader.ReadUInt16();
				if (num61 == 65535)
				{
					num61 = 0;
				}
				BitsByte bitsByte24 = this.reader.ReadByte();
				BitsByte bitsByte25 = this.reader.ReadByte();
				float[] array = this.ReUseTemporaryNPCAI();
				for (int num62 = 0; num62 < NPC.maxAI; num62++)
				{
					if (bitsByte24[num62 + 2])
					{
						array[num62] = this.reader.ReadSingle();
					}
					else
					{
						array[num62] = 0f;
					}
				}
				int num63 = (int)this.reader.ReadInt16();
				int? playerCountForMultiplayerDifficultyOverride = new int?(1);
				if (bitsByte25[0])
				{
					playerCountForMultiplayerDifficultyOverride = new int?((int)this.reader.ReadByte());
				}
				float value2 = 1f;
				if (bitsByte25[2])
				{
					value2 = this.reader.ReadSingle();
				}
				int num64 = 0;
				if (!bitsByte24[7])
				{
					byte b7 = this.reader.ReadByte();
					if (b7 == 2)
					{
						num64 = (int)this.reader.ReadInt16();
					}
					else if (b7 == 4)
					{
						num64 = this.reader.ReadInt32();
					}
					else
					{
						num64 = (int)this.reader.ReadSByte();
					}
				}
				NPC npc2 = Main.npc[num60];
				bool flag15 = bitsByte25[3] || !npc2.active;
				int num65 = -1;
				if (flag15 || npc2.netID != num63)
				{
					if (flag15)
					{
						npc2.ResetForNewNPC();
					}
					else
					{
						num65 = npc2.type;
					}
					npc2.active = true;
					npc2.SetDefaults(num63, new NPCSpawnParams
					{
						playerCountForMultiplayerDifficultyOverride = playerCountForMultiplayerDifficultyOverride,
						difficultyOverride = new float?(value2)
					});
				}
				if (!flag15 && Vector2.DistanceSquared(npc2.position, vector3) <= (float)(Main.multiplayerNPCSmoothingRange * Main.multiplayerNPCSmoothingRange))
				{
					npc2.netOffset += npc2.position - vector3;
					if (npc2.netOffset != Vector2.Zero && DebugOptions.ShowNetOffsetDust && Vector2.Distance(vector3, npc2.position) > 4f)
					{
						Dust.QuickDustLine(vector3, npc2.position, 20f, Color.Red);
					}
				}
				npc2.position = vector3;
				npc2.velocity = velocity3;
				npc2.target = num61;
				npc2.direction = (bitsByte24[0] ? 1 : -1);
				npc2.directionY = (bitsByte24[1] ? 1 : -1);
				npc2.spriteDirection = (bitsByte24[6] ? 1 : -1);
				if (bitsByte24[7])
				{
					num64 = (npc2.life = npc2.lifeMax);
				}
				else
				{
					npc2.life = num64;
				}
				if (num64 <= 0)
				{
					npc2.active = false;
				}
				npc2.SpawnedFromStatue = bitsByte25[1];
				if (npc2.SpawnedFromStatue)
				{
					npc2.value = 0f;
				}
				if (bitsByte25[4])
				{
					npc2.shimmerTransparency = 1f;
				}
				for (int num66 = 0; num66 < NPC.maxAI; num66++)
				{
					npc2.ai[num66] = array[num66];
				}
				if (num65 > -1)
				{
					npc2.TransformVisuals(num65, npc2.type);
				}
				if (num63 == 262)
				{
					NPC.plantBoss = num60;
				}
				if (num63 == 245)
				{
					NPC.golemBoss = num60;
				}
				if (num63 == 668)
				{
					NPC.deerclopsBoss = num60;
				}
				if (npc2.type >= 0 && npc2.type < (int)NPCID.Count && Main.npcCatchable[npc2.type])
				{
					npc2.releaseOwner = (short)this.reader.ReadByte();
					return;
				}
				return;
			}
			case 24:
			{
				int num67 = (int)this.reader.ReadInt16();
				int num68 = (int)this.reader.ReadByte();
				if (Main.netMode == 2)
				{
					num68 = this.whoAmI;
				}
				Player player7 = Main.player[num68];
				Main.npc[num67].StrikeNPC(player7.inventory[player7.selectedItem].damage, player7.inventory[player7.selectedItem].knockBack, player7.direction, false, false, false, -1);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(24, -1, this.whoAmI, null, num67, (float)num68, 0f, 0f, 0, 0, 0);
					NetMessage.TrySendData(23, -1, -1, null, num67, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				return;
			}
			case 27:
			{
				int num69 = (int)this.reader.ReadInt16();
				Vector2 position3 = this.reader.ReadVector2();
				Vector2 velocity4 = this.reader.ReadVector2();
				int num70 = (int)this.reader.ReadByte();
				int num71 = (int)this.reader.ReadInt16();
				BitsByte bitsByte26 = this.reader.ReadByte();
				BitsByte bitsByte27 = bitsByte26[2] ? this.reader.ReadByte() : 0;
				float[] array2 = this.ReUseTemporaryProjectileAI();
				array2[0] = (bitsByte26[0] ? this.reader.ReadSingle() : 0f);
				array2[1] = (bitsByte26[1] ? this.reader.ReadSingle() : 0f);
				int bannerIdToRespondTo = (int)(bitsByte26[3] ? this.reader.ReadUInt16() : 0);
				int damage = (int)(bitsByte26[4] ? this.reader.ReadInt16() : 0);
				float knockBack = bitsByte26[5] ? this.reader.ReadSingle() : 0f;
				int originalDamage = (int)(bitsByte26[6] ? this.reader.ReadInt16() : 0);
				int num72 = (int)(bitsByte26[7] ? this.reader.ReadInt16() : -1);
				if (num72 >= 1000)
				{
					num72 = -1;
				}
				array2[2] = (bitsByte27[0] ? this.reader.ReadSingle() : 0f);
				if (Main.netMode == 2)
				{
					if (num71 == 949)
					{
						num70 = 255;
					}
					else
					{
						num70 = this.whoAmI;
						if (Main.projHostile[num71])
						{
							return;
						}
					}
				}
				int num73 = 1000;
				for (int num74 = 0; num74 < 1000; num74++)
				{
					if (Main.projectile[num74].owner == num70 && Main.projectile[num74].identity == num69 && Main.projectile[num74].active)
					{
						num73 = num74;
						break;
					}
				}
				if (num73 == 1000)
				{
					for (int num75 = 0; num75 < 1000; num75++)
					{
						if (!Main.projectile[num75].active)
						{
							num73 = num75;
							break;
						}
					}
				}
				if (num73 == 1000)
				{
					num73 = Projectile.FindOldestProjectile();
				}
				Projectile projectile = Main.projectile[num73];
				if (!projectile.active || projectile.type != num71)
				{
					projectile.SetDefaults(num71);
					if (Main.netMode == 2)
					{
						Netplay.Clients[this.whoAmI].SpamProjectile += 1f;
					}
				}
				projectile.identity = num69;
				projectile.position = position3;
				projectile.velocity = velocity4;
				projectile.type = num71;
				projectile.damage = damage;
				projectile.bannerIdToRespondTo = bannerIdToRespondTo;
				projectile.originalDamage = originalDamage;
				projectile.knockBack = knockBack;
				projectile.owner = num70;
				for (int num76 = 0; num76 < Projectile.maxAI; num76++)
				{
					projectile.ai[num76] = array2[num76];
				}
				if (num72 >= 0)
				{
					projectile.projUUID = num72;
					Main.projectileIdentity[num70, num72] = num73;
				}
				projectile.ProjectileFixDesperation();
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(27, -1, this.whoAmI, null, num73, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				return;
			}
			case 28:
			{
				int num77 = (int)this.reader.ReadInt16();
				int num78 = (int)this.reader.ReadInt16();
				float num79 = this.reader.ReadSingle();
				int num80 = (int)(this.reader.ReadByte() - 1);
				byte b8 = this.reader.ReadByte();
				if (Main.netMode == 2)
				{
					if (num78 < 0)
					{
						num78 = 0;
					}
					Main.npc[num77].PlayerInteraction(this.whoAmI);
				}
				if (num78 >= 0)
				{
					Main.npc[num77].StrikeNPC(num78, num79, num80, b8 == 1, false, true, (Main.netMode == 2) ? this.whoAmI : 255);
				}
				else
				{
					Main.npc[num77].life = 0;
					Main.npc[num77].HitEffect(0, 10.0);
					Main.npc[num77].active = false;
				}
				if (Main.netMode != 2)
				{
					return;
				}
				NetMessage.TrySendData(28, -1, this.whoAmI, null, num77, (float)num78, num79, (float)num80, (int)b8, 0, 0);
				if (Main.npc[num77].life <= 0)
				{
					NetMessage.TrySendData(23, -1, -1, null, num77, 0f, 0f, 0f, 0, 0, 0);
				}
				if (Main.npc[num77].realLife >= 0 && Main.npc[Main.npc[num77].realLife].life <= 0)
				{
					NetMessage.TrySendData(23, -1, -1, null, Main.npc[num77].realLife, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				return;
			}
			case 29:
			{
				int num81 = (int)this.reader.ReadInt16();
				int num82 = (int)this.reader.ReadByte();
				if (Main.netMode == 2)
				{
					num82 = this.whoAmI;
				}
				for (int num83 = 0; num83 < 1000; num83++)
				{
					if (Main.projectile[num83].owner == num82 && Main.projectile[num83].identity == num81 && Main.projectile[num83].active)
					{
						Main.projectile[num83].Kill();
						break;
					}
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(29, -1, this.whoAmI, null, num81, (float)num82, 0f, 0f, 0, 0, 0);
					return;
				}
				return;
			}
			case 30:
			{
				int num84 = (int)this.reader.ReadByte();
				if (Main.netMode == 2)
				{
					num84 = this.whoAmI;
				}
				bool flag16 = this.reader.ReadBoolean();
				Main.player[num84].hostile = flag16;
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(30, -1, this.whoAmI, null, num84, 0f, 0f, 0f, 0, 0, 0);
					LocalizedText localizedText = flag16 ? Lang.mp[11] : Lang.mp[12];
					Color color = Main.teamColor[Main.player[num84].team];
					ChatHelper.BroadcastChatMessage(NetworkText.FromKey(localizedText.Key, new object[]
					{
						Main.player[num84].name
					}), color, -1);
					return;
				}
				return;
			}
			case 31:
			{
				if (Main.netMode != 2)
				{
					return;
				}
				int num85 = (int)this.reader.ReadInt16();
				int num86 = (int)this.reader.ReadInt16();
				int num87 = Chest.FindChest(num85, num86);
				if (num87 <= -1 || Chest.UsingChest(num87) != -1)
				{
					return;
				}
				NetMessage.SendChestContentsTo(num87, this.whoAmI);
				NetMessage.TrySendData(33, this.whoAmI, -1, null, num87, 0f, 0f, 0f, 0, 0, 0);
				Main.player[this.whoAmI].chest = num87;
				if (Main.myPlayer == this.whoAmI)
				{
					Main.PipsUseGrid = false;
				}
				NetMessage.TrySendData(80, -1, this.whoAmI, null, this.whoAmI, (float)num87, 0f, 0f, 0, 0, 0);
				if (Main.netMode == 2 && WorldGen.IsChestRigged(num85, num86))
				{
					Wiring.SetCurrentUser(this.whoAmI);
					Wiring.HitSwitch(num85, num86);
					Wiring.SetCurrentUser(-1);
					NetMessage.TrySendData(59, -1, this.whoAmI, null, num85, (float)num86, 0f, 0f, 0, 0, 0);
					return;
				}
				return;
			}
			case 32:
			{
				int num88 = (int)this.reader.ReadInt16();
				int num89 = (int)this.reader.ReadByte();
				int stack3 = (int)this.reader.ReadInt16();
				int prefixWeWant2 = (int)this.reader.ReadByte();
				int type5 = (int)this.reader.ReadInt16();
				if (num88 < 0 || num88 >= 8000 || Main.chest[num88] == null)
				{
					return;
				}
				if (Main.chest[num88].item[num89] == null)
				{
					Main.chest[num88].item[num89] = new Item();
				}
				Main.chest[num88].item[num89].SetDefaults(type5, null);
				Main.chest[num88].item[num89].Prefix(prefixWeWant2);
				Main.chest[num88].item[num89].stack = stack3;
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(32, -1, this.whoAmI, null, num88, (float)num89, 0f, 0f, 0, 0, 0);
					return;
				}
				return;
			}
			case 33:
			{
				int num90 = (int)this.reader.ReadInt16();
				int num91 = (int)this.reader.ReadInt16();
				int num92 = (int)this.reader.ReadInt16();
				int num93 = (int)this.reader.ReadByte();
				string name = string.Empty;
				if (num93 != 0)
				{
					if (num93 <= 20)
					{
						name = this.reader.ReadString();
					}
					else if (num93 != 255)
					{
						num93 = 0;
					}
				}
				if (Main.netMode != 1)
				{
					if (num93 != 0)
					{
						int chest = Main.player[this.whoAmI].chest;
						Chest chest2 = Main.chest[chest];
						chest2.name = name;
						NetMessage.TrySendData(69, -1, this.whoAmI, null, chest, (float)chest2.x, (float)chest2.y, 0f, 0, 0, 0);
					}
					Main.player[this.whoAmI].chest = num90;
					NetMessage.TrySendData(80, -1, this.whoAmI, null, this.whoAmI, (float)num90, 0f, 0f, 0, 0, 0);
					return;
				}
				Player player8 = Main.player[Main.myPlayer];
				if (player8.chest == -1)
				{
					Main.playerInventory = true;
					SoundEngine.PlaySound(10, -1, -1, 1, 1f, 0f);
					if (num90 != -1)
					{
						ItemSlot.SetGlowForChest(Main.chest[num90]);
					}
				}
				else if (player8.chest != num90 && num90 != -1)
				{
					Main.playerInventory = true;
					SoundEngine.PlaySound(12, -1, -1, 1, 1f, 0f);
					Main.PipsUseGrid = false;
					ItemSlot.SetGlowForChest(Main.chest[num90]);
				}
				else if (player8.chest != -1 && num90 == -1)
				{
					SoundEngine.PlaySound(11, -1, -1, 1, 1f, 0f);
					Main.PipsUseGrid = false;
				}
				player8.chest = num90;
				player8.chestX = num91;
				player8.chestY = num92;
				if (Main.tile[num91, num92].frameX >= 36 && Main.tile[num91, num92].frameX < 72)
				{
					AchievementsHelper.HandleSpecialEvent(Main.player[Main.myPlayer], 16);
					return;
				}
				return;
			}
			case 34:
			{
				byte b9 = this.reader.ReadByte();
				int num94 = (int)this.reader.ReadInt16();
				int num95 = (int)this.reader.ReadInt16();
				int num96 = (int)this.reader.ReadInt16();
				int num97 = (int)this.reader.ReadInt16();
				if (Main.netMode == 2)
				{
					num97 = 0;
				}
				if (Main.netMode == 2)
				{
					if (b9 == 0)
					{
						int num98 = WorldGen.PlaceChest(num94, num95, 21, false, num96);
						if (num98 != -1)
						{
							NetMessage.TrySendData(34, -1, -1, null, (int)b9, (float)num94, (float)num95, (float)num96, num98, 0, 0);
							return;
						}
						NetMessage.TrySendData(34, this.whoAmI, -1, null, (int)b9, (float)num94, (float)num95, (float)num96, num98, 0, 0);
						int itemDrop_Chests = WorldGen.GetItemDrop_Chests(num96, false);
						if (itemDrop_Chests > 0)
						{
							Item.NewItem(new EntitySource_TileBreak(num94, num95), num94 * 16, num95 * 16, 32, 32, itemDrop_Chests, 1, true, 0, false);
							return;
						}
						return;
					}
					else if (b9 == 1 && Main.tile[num94, num95].type == 21)
					{
						Tile tile2 = Main.tile[num94, num95];
						if (tile2.frameX % 36 != 0)
						{
							num94--;
						}
						if (tile2.frameY % 36 != 0)
						{
							num95--;
						}
						int number = Chest.FindChest(num94, num95);
						WorldGen.KillTile(num94, num95, false, false, false);
						if (!tile2.active())
						{
							NetMessage.TrySendData(34, -1, -1, null, (int)b9, (float)num94, (float)num95, 0f, number, 0, 0);
							return;
						}
						return;
					}
					else if (b9 == 2)
					{
						int num99 = WorldGen.PlaceChest(num94, num95, 88, false, num96);
						if (num99 == -1)
						{
							NetMessage.TrySendData(34, this.whoAmI, -1, null, (int)b9, (float)num94, (float)num95, (float)num96, num99, 0, 0);
							Item.NewItem(new EntitySource_TileBreak(num94, num95), num94 * 16, num95 * 16, 32, 32, WorldGen.GetItemDrop_Dressers(num96), 1, true, 0, false);
							return;
						}
						NetMessage.TrySendData(34, -1, -1, null, (int)b9, (float)num94, (float)num95, (float)num96, num99, 0, 0);
						return;
					}
					else if (b9 == 3 && Main.tile[num94, num95].type == 88)
					{
						Tile tile3 = Main.tile[num94, num95];
						num94 -= (int)(tile3.frameX % 54 / 18);
						if (tile3.frameY % 36 != 0)
						{
							num95--;
						}
						int number2 = Chest.FindChest(num94, num95);
						WorldGen.KillTile(num94, num95, false, false, false);
						if (!tile3.active())
						{
							NetMessage.TrySendData(34, -1, -1, null, (int)b9, (float)num94, (float)num95, 0f, number2, 0, 0);
							return;
						}
						return;
					}
					else if (b9 == 4)
					{
						int num100 = WorldGen.PlaceChest(num94, num95, 467, false, num96);
						if (num100 != -1)
						{
							NetMessage.TrySendData(34, -1, -1, null, (int)b9, (float)num94, (float)num95, (float)num96, num100, 0, 0);
							return;
						}
						NetMessage.TrySendData(34, this.whoAmI, -1, null, (int)b9, (float)num94, (float)num95, (float)num96, num100, 0, 0);
						int itemDrop_Chests2 = WorldGen.GetItemDrop_Chests(num96, true);
						if (itemDrop_Chests2 > 0)
						{
							Item.NewItem(new EntitySource_TileBreak(num94, num95), num94 * 16, num95 * 16, 32, 32, itemDrop_Chests2, 1, true, 0, false);
							return;
						}
						return;
					}
					else
					{
						if (b9 != 5 || Main.tile[num94, num95].type != 467)
						{
							return;
						}
						Tile tile4 = Main.tile[num94, num95];
						if (tile4.frameX % 36 != 0)
						{
							num94--;
						}
						if (tile4.frameY % 36 != 0)
						{
							num95--;
						}
						int number3 = Chest.FindChest(num94, num95);
						WorldGen.KillTile(num94, num95, false, false, false);
						if (!tile4.active())
						{
							NetMessage.TrySendData(34, -1, -1, null, (int)b9, (float)num94, (float)num95, 0f, number3, 0, 0);
							return;
						}
						return;
					}
				}
				else if (b9 == 0)
				{
					if (num97 == -1)
					{
						WorldGen.KillTile(num94, num95, false, false, false);
						return;
					}
					SoundEngine.PlaySound(0, num94 * 16, num95 * 16, 1, 1f, 0f);
					WorldGen.PlaceChestDirect(num94, num95, 21, num96, num97);
					return;
				}
				else if (b9 == 2)
				{
					if (num97 == -1)
					{
						WorldGen.KillTile(num94, num95, false, false, false);
						return;
					}
					SoundEngine.PlaySound(0, num94 * 16, num95 * 16, 1, 1f, 0f);
					WorldGen.PlaceDresserDirect(num94, num95, 88, num96, num97);
					return;
				}
				else
				{
					if (b9 != 4)
					{
						Chest.DestroyChestDirect(num94, num95, num97);
						WorldGen.KillTile(num94, num95, false, false, false);
						return;
					}
					if (num97 == -1)
					{
						WorldGen.KillTile(num94, num95, false, false, false);
						return;
					}
					SoundEngine.PlaySound(0, num94 * 16, num95 * 16, 1, 1f, 0f);
					WorldGen.PlaceChestDirect(num94, num95, 467, num96, num97);
					return;
				}
				break;
			}
			case 35:
			{
				int num101 = (int)this.reader.ReadByte();
				if (Main.netMode == 2)
				{
					num101 = this.whoAmI;
				}
				int num102 = (int)this.reader.ReadInt16();
				if (num101 != Main.myPlayer || Main.ServerSideCharacter)
				{
					Main.player[num101].HealEffect(num102, true);
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(35, -1, this.whoAmI, null, num101, (float)num102, 0f, 0f, 0, 0, 0);
					return;
				}
				return;
			}
			case 36:
			{
				int num103 = (int)this.reader.ReadByte();
				if (Main.netMode == 2)
				{
					num103 = this.whoAmI;
				}
				Player player9 = Main.player[num103];
				bool flag17 = player9.zone5[0];
				player9.zone1 = this.reader.ReadByte();
				player9.zone2 = this.reader.ReadByte();
				player9.zone3 = this.reader.ReadByte();
				player9.zone4 = this.reader.ReadByte();
				player9.zone5 = this.reader.ReadByte();
				player9.townNPCs = (int)this.reader.ReadByte();
				if (Main.netMode == 2)
				{
					if (!flag17 && player9.zone5[0])
					{
						NPC.Spawner.SpawnFaelings(player9);
					}
					NetMessage.TrySendData(36, -1, this.whoAmI, null, num103, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				return;
			}
			case 37:
				if (Main.netMode != 1)
				{
					return;
				}
				if (Main.autoPass)
				{
					NetMessage.TrySendData(38, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
					Main.autoPass = false;
					return;
				}
				Netplay.ServerPassword = "";
				Main.menuMode = 31;
				return;
			case 38:
				if (Main.netMode != 2)
				{
					return;
				}
				if (this.reader.ReadString() == Netplay.ServerPassword)
				{
					Netplay.Clients[this.whoAmI].State = 1;
					NetMessage.TrySendData(3, this.whoAmI, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				NetMessage.TrySendData(2, this.whoAmI, -1, Lang.mp[1].ToNetworkText(), 0, 0f, 0f, 0f, 0, 0, 0);
				return;
			case 39:
			{
				int num104 = (int)this.reader.ReadInt16();
				WorldItem worldItem3 = Main.item[num104];
				if (Main.netMode == 1)
				{
					if (worldItem3.playerIndexTheItemIsReservedFor != Main.myPlayer)
					{
						return;
					}
					worldItem3.playerIndexTheItemIsReservedFor = 255;
					NetMessage.TrySendData(39, -1, -1, null, num104, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				else
				{
					if (worldItem3.playerIndexTheItemIsReservedFor != this.whoAmI)
					{
						return;
					}
					worldItem3.playerIndexTheItemIsReservedFor = 255;
					worldItem3.FindOwner();
					if (worldItem3.playerIndexTheItemIsReservedFor == 255)
					{
						NetMessage.TrySendData(22, -1, this.whoAmI, null, num104, 0f, 0f, 0f, 0, 0, 0);
						return;
					}
					return;
				}
				break;
			}
			case 40:
			{
				int num105 = (int)this.reader.ReadByte();
				if (Main.netMode == 2)
				{
					num105 = this.whoAmI;
				}
				int talkNPC = (int)this.reader.ReadInt16();
				Main.player[num105].SetTalkNPC(talkNPC);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(40, -1, this.whoAmI, null, num105, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				return;
			}
			case 41:
			{
				int num106 = (int)this.reader.ReadByte();
				if (Main.netMode == 2)
				{
					num106 = this.whoAmI;
				}
				Player player10 = Main.player[num106];
				float itemRotation = this.reader.ReadSingle();
				int itemAnimation = (int)this.reader.ReadInt16();
				player10.itemRotation = itemRotation;
				player10.itemAnimation = itemAnimation;
				player10.channel = player10.inventory[player10.selectedItem].channel;
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(41, -1, this.whoAmI, null, num106, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				return;
			}
			case 42:
			{
				int num107 = (int)this.reader.ReadByte();
				if (Main.netMode == 2)
				{
					num107 = this.whoAmI;
				}
				else if (Main.myPlayer == num107 && !Main.ServerSideCharacter)
				{
					return;
				}
				int statMana = (int)this.reader.ReadInt16();
				int statManaMax = (int)this.reader.ReadInt16();
				Main.player[num107].statMana = statMana;
				Main.player[num107].statManaMax = statManaMax;
				return;
			}
			case 43:
			{
				int num108 = (int)this.reader.ReadByte();
				if (Main.netMode == 2)
				{
					num108 = this.whoAmI;
				}
				int num109 = (int)this.reader.ReadInt16();
				if (num108 != Main.myPlayer)
				{
					Main.player[num108].ManaEffect(num109);
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(43, -1, this.whoAmI, null, num108, (float)num109, 0f, 0f, 0, 0, 0);
					return;
				}
				return;
			}
			case 45:
			case 157:
			{
				int num110 = (int)this.reader.ReadByte();
				if (Main.netMode == 2)
				{
					num110 = this.whoAmI;
				}
				int num111 = (int)this.reader.ReadByte();
				Player player11 = Main.player[num110];
				int team = player11.team;
				player11.team = num111;
				Color color2 = Main.teamColor[num111];
				if (Main.netMode != 2)
				{
					return;
				}
				NetMessage.TrySendData(45, -1, this.whoAmI, null, num110, 0f, 0f, 0f, 0, 0, 0);
				LocalizedText localizedText2 = Lang.mp[13 + num111];
				if (num111 == 5)
				{
					localizedText2 = Lang.mp[22];
				}
				for (int num112 = 0; num112 < 255; num112++)
				{
					if (num112 == this.whoAmI || (team > 0 && Main.player[num112].team == team) || (num111 > 0 && Main.player[num112].team == num111))
					{
						ChatHelper.SendChatMessageToClient(NetworkText.FromKey(localizedText2.Key, new object[]
						{
							player11.name
						}), color2, num112);
					}
				}
				if (b != 157 || !Main.teamBasedSpawnsSeed)
				{
					return;
				}
				Point zero2 = Point.Zero;
				if (ExtraSpawnPointManager.TryGetExtraSpawnPointForTeam(num111, out zero2))
				{
					RemoteClient.CheckSection(this.whoAmI, zero2.ToWorldCoordinates(8f, 8f), 1);
					NetMessage.SendData(158, num110, -1, null, num110, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				return;
			}
			case 46:
			{
				if (Main.netMode != 2)
				{
					return;
				}
				int i2 = (int)this.reader.ReadInt16();
				int j2 = (int)this.reader.ReadInt16();
				int num113 = Sign.ReadSign(i2, j2, true);
				if (num113 >= 0)
				{
					NetMessage.TrySendData(47, this.whoAmI, -1, null, num113, (float)this.whoAmI, 0f, 0f, 0, 0, 0);
					return;
				}
				return;
			}
			case 47:
			{
				int num114 = (int)this.reader.ReadInt16();
				int x = (int)this.reader.ReadInt16();
				int y = (int)this.reader.ReadInt16();
				string text = this.reader.ReadString();
				int num115 = (int)this.reader.ReadByte();
				BitsByte bitsByte28 = this.reader.ReadByte();
				if (num114 < 0 || num114 >= 32000)
				{
					return;
				}
				string a = null;
				if (Main.sign[num114] != null)
				{
					a = Main.sign[num114].text;
				}
				Main.sign[num114] = new Sign();
				Main.sign[num114].x = x;
				Main.sign[num114].y = y;
				Sign.TextSign(num114, text);
				if (Main.netMode == 2 && a != text)
				{
					num115 = this.whoAmI;
					NetMessage.TrySendData(47, -1, this.whoAmI, null, num114, (float)num115, 0f, 0f, 0, 0, 0);
				}
				if (Main.netMode == 1 && num115 == Main.myPlayer && Main.sign[num114] != null && !bitsByte28[0])
				{
					Main.LocalPlayer.OpenSign(num114);
					return;
				}
				return;
			}
			case 48:
			{
				int num116 = (int)this.reader.ReadInt16();
				int num117 = (int)this.reader.ReadInt16();
				byte b10 = this.reader.ReadByte();
				byte liquidType = this.reader.ReadByte();
				if (Main.netMode == 2 && Netplay.SpamCheck)
				{
					int num118 = this.whoAmI;
					int num119 = (int)(Main.player[num118].position.X + (float)(Main.player[num118].width / 2));
					int num120 = (int)(Main.player[num118].position.Y + (float)(Main.player[num118].height / 2));
					int num121 = 10;
					int num122 = num119 - num121;
					int num123 = num119 + num121;
					int num124 = num120 - num121;
					int num125 = num120 + num121;
					if (num116 < num122 || num116 > num123 || num117 < num124 || num117 > num125)
					{
						Netplay.Clients[this.whoAmI].SpamWater += 1f;
					}
				}
				if (Main.tile[num116, num117] == null)
				{
					Main.tile[num116, num117] = new Tile();
				}
				Tile obj2 = Main.tile[num116, num117];
				lock (obj2)
				{
					Main.tile[num116, num117].liquid = b10;
					Main.tile[num116, num117].liquidType((int)liquidType);
					if (Main.netMode == 2)
					{
						WorldGen.SquareTileFrame(num116, num117, true);
						if (b10 == 0)
						{
							NetMessage.SendData(48, -1, this.whoAmI, null, num116, (float)num117, 0f, 0f, 0, 0, 0);
						}
					}
					return;
				}
				goto IL_550B;
			}
			case 49:
				goto IL_550B;
			case 50:
			{
				int num126 = (int)this.reader.ReadByte();
				if (Main.netMode == 2)
				{
					num126 = this.whoAmI;
				}
				else if (num126 == Main.myPlayer && !Main.ServerSideCharacter)
				{
					return;
				}
				Player player12 = Main.player[num126];
				int num127 = 0;
				int num128;
				while ((num128 = (int)this.reader.ReadUInt16()) > 0)
				{
					player12.buffType[num127] = num128;
					player12.buffTime[num127] = 60;
					num127++;
				}
				Array.Clear(player12.buffType, num127, player12.buffType.Length - num127);
				Array.Clear(player12.buffTime, num127, player12.buffTime.Length - num127);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(50, -1, this.whoAmI, null, num126, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				return;
			}
			case 51:
			{
				byte b11 = this.reader.ReadByte();
				if (Main.netMode == 2)
				{
					b11 = (byte)this.whoAmI;
				}
				byte b12 = this.reader.ReadByte();
				if (b12 == 1)
				{
					NPC.SpawnSkeletron((int)b11, false);
					return;
				}
				if (b12 == 2)
				{
					if (Main.netMode == 2)
					{
						NetMessage.TrySendData(51, -1, this.whoAmI, null, (int)b11, (float)b12, 0f, 0f, 0, 0, 0);
						return;
					}
					SoundEngine.PlaySound(SoundID.Item1, (int)Main.player[(int)b11].position.X, (int)Main.player[(int)b11].position.Y, 0f, 1f);
					return;
				}
				else if (b12 == 3)
				{
					if (Main.netMode == 2)
					{
						Main.Sundialing();
						return;
					}
					return;
				}
				else
				{
					if (b12 == 4)
					{
						Main.npc[(int)b11].BigMimicSpawnSmoke();
						return;
					}
					if (b12 == 5)
					{
						if (Main.netMode == 2)
						{
							NPC npc3 = new NPC();
							npc3.SetDefaults(664, default(NPCSpawnParams));
							Main.BestiaryTracker.Kills.RegisterKill(npc3);
							return;
						}
						return;
					}
					else
					{
						if (b12 == 6 && Main.netMode == 2)
						{
							Main.Moondialing();
							return;
						}
						return;
					}
				}
				break;
			}
			case 52:
			{
				int num129 = (int)this.reader.ReadByte();
				int num130 = (int)this.reader.ReadInt16();
				int num131 = (int)this.reader.ReadInt16();
				if (num129 == 1)
				{
					Chest.Unlock(num130, num131);
					if (Main.netMode == 2)
					{
						NetMessage.TrySendData(52, -1, this.whoAmI, null, 0, (float)num129, (float)num130, (float)num131, 0, 0, 0);
						NetMessage.SendTileSquare(-1, num130, num131, 2, TileChangeType.None);
					}
				}
				if (num129 == 2)
				{
					WorldGen.UnlockDoor(num130, num131);
					if (Main.netMode == 2)
					{
						NetMessage.TrySendData(52, -1, this.whoAmI, null, 0, (float)num129, (float)num130, (float)num131, 0, 0, 0);
						NetMessage.SendTileSquare(-1, num130, num131, 2, TileChangeType.None);
					}
				}
				if (num129 != 3)
				{
					return;
				}
				Chest.Lock(num130, num131);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(52, -1, this.whoAmI, null, 0, (float)num129, (float)num130, (float)num131, 0, 0, 0);
					NetMessage.SendTileSquare(-1, num130, num131, 2, TileChangeType.None);
					return;
				}
				return;
			}
			case 53:
			{
				int num132 = (int)this.reader.ReadInt16();
				int type6 = (int)this.reader.ReadUInt16();
				int time = (int)this.reader.ReadInt16();
				Main.npc[num132].AddBuff(type6, time, true);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(54, -1, -1, null, num132, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				return;
			}
			case 54:
			{
				if (Main.netMode != 1)
				{
					return;
				}
				int num133 = (int)this.reader.ReadInt16();
				NPC npc4 = Main.npc[num133];
				int num134 = 0;
				int num135;
				while ((num135 = (int)this.reader.ReadUInt16()) > 0)
				{
					npc4.buffType[num134] = num135;
					npc4.buffTime[num134] = (int)this.reader.ReadUInt16();
					num134++;
				}
				Array.Clear(npc4.buffType, num134, npc4.buffType.Length - num134);
				Array.Clear(npc4.buffTime, num134, npc4.buffTime.Length - num134);
				return;
			}
			case 55:
			{
				int num136 = (int)this.reader.ReadByte();
				int num137 = (int)this.reader.ReadUInt16();
				int num138 = this.reader.ReadInt32();
				if (Main.netMode == 2 && !Main.pvpBuff[num137])
				{
					return;
				}
				if (Main.netMode == 1 && num136 != Main.myPlayer)
				{
					return;
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(55, num136, -1, null, num136, (float)num137, (float)num138, 0f, 0, 0, 0);
					return;
				}
				Main.player[num136].AddBuff(num137, num138, true);
				return;
			}
			case 56:
			{
				int num139 = (int)this.reader.ReadInt16();
				if (num139 < 0 || num139 >= Main.maxNPCs)
				{
					return;
				}
				if (Main.netMode == 1)
				{
					string givenName = this.reader.ReadString();
					Main.npc[num139].GivenName = givenName;
					int townNpcVariationIndex = this.reader.ReadInt32();
					Main.npc[num139].townNpcVariationIndex = townNpcVariationIndex;
					return;
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(56, this.whoAmI, -1, null, num139, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				return;
			}
			case 57:
				if (Main.netMode != 1)
				{
					return;
				}
				WorldGen.tGood = this.reader.ReadByte();
				WorldGen.tEvil = this.reader.ReadByte();
				WorldGen.tBlood = this.reader.ReadByte();
				return;
			case 58:
			{
				int num140 = (int)this.reader.ReadByte();
				if (Main.netMode == 2)
				{
					num140 = this.whoAmI;
				}
				float num141 = this.reader.ReadSingle();
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(58, -1, this.whoAmI, null, this.whoAmI, num141, 0f, 0f, 0, 0, 0);
					return;
				}
				Player player13 = Main.player[num140];
				int type7 = player13.inventory[player13.selectedItem].type;
				if (type7 == 4372 || type7 == 4057 || type7 == 4715)
				{
					player13.PlayGuitarChord(num141);
					return;
				}
				if (type7 == 4673)
				{
					player13.PlayDrums(num141);
					return;
				}
				Main.musicPitch = num141;
				LegacySoundStyle type8 = SoundID.Item26;
				if (type7 == 507)
				{
					type8 = SoundID.Item35;
				}
				if (type7 == 1305)
				{
					type8 = SoundID.Item47;
				}
				SoundEngine.PlaySound(type8, player13.position, 0f, 1f);
				return;
			}
			case 59:
			{
				int num142 = (int)this.reader.ReadInt16();
				int num143 = (int)this.reader.ReadInt16();
				Wiring.SetCurrentUser(this.whoAmI);
				Wiring.HitSwitch(num142, num143);
				Wiring.SetCurrentUser(-1);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(59, -1, this.whoAmI, null, num142, (float)num143, 0f, 0f, 0, 0, 0);
					return;
				}
				return;
			}
			case 60:
			{
				int num144 = (int)this.reader.ReadInt16();
				int num145 = (int)this.reader.ReadInt16();
				int num146 = (int)this.reader.ReadInt16();
				byte b13 = this.reader.ReadByte();
				if (num144 >= Main.maxNPCs)
				{
					NetMessage.BootPlayer(this.whoAmI, NetworkText.FromKey("Net.CheatingInvalid", new object[0]));
					return;
				}
				NPC npc5 = Main.npc[num144];
				bool isLikeATownNPC = npc5.isLikeATownNPC;
				if (Main.netMode == 1)
				{
					npc5.homeless = (b13 == 1);
					npc5.homeTileX = num145;
					npc5.homeTileY = num146;
				}
				if (!isLikeATownNPC)
				{
					return;
				}
				if (Main.netMode == 1)
				{
					if (b13 == 1)
					{
						WorldGen.TownManager.KickOut(npc5.type);
						return;
					}
					if (b13 == 2)
					{
						WorldGen.TownManager.SetRoom(npc5.type, num145, num146);
						return;
					}
					return;
				}
				else
				{
					if (b13 == 1)
					{
						WorldGen.kickOut(num144);
						return;
					}
					WorldGen.moveRoom(num145, num146, num144);
					return;
				}
				break;
			}
			case 61:
			{
				int num147 = (int)this.reader.ReadInt16();
				int num148 = (int)this.reader.ReadInt16();
				if (Main.netMode != 2)
				{
					return;
				}
				if (num148 >= 0 && num148 < (int)NPCID.Count && NPCID.Sets.MPAllowedEnemies[num148])
				{
					if (!NPC.AnyNPCs(num148))
					{
						NPC.SpawnOnPlayer(num147, num148, 0f, 0f, 0f, 0f);
						return;
					}
					return;
				}
				else if (num148 == -4)
				{
					if (!Main.dayTime && !DD2Event.Ongoing)
					{
						ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[31].Key, new object[0]), new Color(50, 255, 130), -1);
						Main.startPumpkinMoon();
						NetMessage.TrySendData(7, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
						NetMessage.TrySendData(78, -1, -1, null, 0, 1f, 2f, 1f, 0, 0, 0);
						return;
					}
					return;
				}
				else if (num148 == -5)
				{
					if (!Main.dayTime && !DD2Event.Ongoing)
					{
						ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[34].Key, new object[0]), new Color(50, 255, 130), -1);
						Main.startSnowMoon();
						NetMessage.TrySendData(7, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
						NetMessage.TrySendData(78, -1, -1, null, 0, 1f, 1f, 1f, 0, 0, 0);
						return;
					}
					return;
				}
				else if (num148 == -6)
				{
					if (Main.dayTime && !Main.eclipse)
					{
						if (Main.remixWorld)
						{
							ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[106].Key, new object[0]), new Color(50, 255, 130), -1);
						}
						else
						{
							ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[20].Key, new object[0]), new Color(50, 255, 130), -1);
						}
						Main.eclipse = true;
						NetMessage.TrySendData(7, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
						return;
					}
					return;
				}
				else
				{
					if (num148 == -7)
					{
						Main.invasionDelay = 0;
						Main.StartInvasion(4);
						NetMessage.TrySendData(7, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
						NetMessage.TrySendData(78, -1, -1, null, 0, 1f, (float)(Main.invasionType + 3), 0f, 0, 0, 0);
						return;
					}
					if (num148 == -8)
					{
						if (NPC.downedGolemBoss && Main.hardMode && !NPC.AnyDanger(false, false) && !NPC.AnyoneNearCultists())
						{
							WorldGen.StartImpendingDoom(720);
							NetMessage.TrySendData(7, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
							return;
						}
						return;
					}
					else if (num148 == -10)
					{
						if (!Main.dayTime && !Main.bloodMoon)
						{
							ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[8].Key, new object[0]), new Color(50, 255, 130), -1);
							Main.bloodMoon = true;
							if (Main.GetMoonPhase() == MoonPhase.Empty)
							{
								Main.moonPhase = 5;
							}
							AchievementsHelper.NotifyProgressionEvent(4);
							NetMessage.TrySendData(7, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
							return;
						}
						return;
					}
					else
					{
						if (num148 == -11)
						{
							ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Misc.CombatBookUsed", new object[0]), new Color(50, 255, 130), -1);
							NPC.combatBookWasUsed = true;
							NetMessage.TrySendData(7, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
							return;
						}
						if (num148 == -12)
						{
							NPC.UnlockOrExchangePet(ref NPC.boughtCat, 637, "Misc.LicenseCatUsed", num148);
							return;
						}
						if (num148 == -13)
						{
							NPC.UnlockOrExchangePet(ref NPC.boughtDog, 638, "Misc.LicenseDogUsed", num148);
							return;
						}
						if (num148 == -14)
						{
							NPC.UnlockOrExchangePet(ref NPC.boughtBunny, 656, "Misc.LicenseBunnyUsed", num148);
							return;
						}
						if (num148 == -15)
						{
							NPC.UnlockOrExchangePet(ref NPC.unlockedSlimeBlueSpawn, 670, "Misc.LicenseSlimeUsed", num148);
							return;
						}
						if (num148 == -16)
						{
							NPC.SpawnMechQueen(num147);
							return;
						}
						if (num148 == -17)
						{
							ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Misc.CombatBookVolumeTwoUsed", new object[0]), new Color(50, 255, 130), -1);
							NPC.combatBookVolumeTwoWasUsed = true;
							NetMessage.TrySendData(7, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
							return;
						}
						if (num148 == -18)
						{
							ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Misc.PeddlersSatchelUsed", new object[0]), new Color(50, 255, 130), -1);
							NPC.peddlersSatchelWasUsed = true;
							NetMessage.TrySendData(7, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
							return;
						}
						if (num148 == -19)
						{
							Main.StartSlimeRain(true);
							return;
						}
						if (num148 < 0)
						{
							int num149 = 1;
							if (num148 > (int)(-(int)InvasionID.Count))
							{
								num149 = -num148;
							}
							if (num149 > 0 && Main.invasionType == 0)
							{
								Main.invasionDelay = 0;
								Main.StartInvasion(num149);
							}
							NetMessage.TrySendData(7, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
							NetMessage.TrySendData(78, -1, -1, null, 0, 1f, (float)(Main.invasionType + 3), 0f, 0, 0, 0);
							return;
						}
						return;
					}
				}
				break;
			}
			case 62:
			{
				int num150 = (int)this.reader.ReadByte();
				int num151 = (int)this.reader.ReadByte();
				if (Main.netMode == 2)
				{
					num150 = this.whoAmI;
				}
				if (num151 == 1)
				{
					Main.player[num150].NinjaDodge();
				}
				if (num151 == 2)
				{
					Main.player[num150].ShadowDodge();
				}
				if (num151 == 4)
				{
					Main.player[num150].BrainOfConfusionDodge();
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(62, -1, this.whoAmI, null, num150, (float)num151, 0f, 0f, 0, 0, 0);
					return;
				}
				return;
			}
			case 63:
			{
				int num152 = (int)this.reader.ReadInt16();
				int num153 = (int)this.reader.ReadInt16();
				byte b14 = this.reader.ReadByte();
				byte b15 = this.reader.ReadByte();
				if (b15 == 0)
				{
					WorldGen.paintTile(num152, num153, b14, false, true);
				}
				else
				{
					WorldGen.paintCoatTile(num152, num153, b14, false, true);
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(63, -1, this.whoAmI, null, num152, (float)num153, (float)b14, (float)b15, 0, 0, 0);
					return;
				}
				return;
			}
			case 64:
			{
				int num154 = (int)this.reader.ReadInt16();
				int num155 = (int)this.reader.ReadInt16();
				byte b16 = this.reader.ReadByte();
				byte b17 = this.reader.ReadByte();
				if (b17 == 0)
				{
					WorldGen.paintWall(num154, num155, b16, false, true);
				}
				else
				{
					WorldGen.paintCoatWall(num154, num155, b16, false, true);
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(64, -1, this.whoAmI, null, num154, (float)num155, (float)b16, (float)b17, 0, 0, 0);
					return;
				}
				return;
			}
			case 65:
			{
				BitsByte bitsByte29 = this.reader.ReadByte();
				int num156 = (int)this.reader.ReadInt16();
				if (Main.netMode == 2)
				{
					num156 = this.whoAmI;
				}
				Vector2 vector4 = this.reader.ReadVector2();
				int num157 = (int)this.reader.ReadByte();
				int num158 = 0;
				if (bitsByte29[0])
				{
					num158++;
				}
				if (bitsByte29[1])
				{
					num158 += 2;
				}
				bool flag18 = false;
				if (bitsByte29[2])
				{
					flag18 = true;
				}
				int num159 = 0;
				if (bitsByte29[3])
				{
					num159 = this.reader.ReadInt32();
				}
				if (flag18)
				{
					vector4 = Main.player[num156].position;
				}
				if (num158 == 0)
				{
					Main.player[num156].Teleport(vector4, num157, num159);
					if (Main.netMode == 2)
					{
						NetMessage.TrySendData(65, -1, this.whoAmI, null, 0, (float)num156, vector4.X, vector4.Y, num157, flag18.ToInt(), num159);
					}
					if (Main.netMode == 1 && num156 == Main.myPlayer)
					{
						NetMessage.TrySendData(65, -1, -1, null, 3, (float)num156, 0f, 0f, 0, 0, 0);
						return;
					}
					return;
				}
				else
				{
					if (num158 == 1)
					{
						Main.npc[num156].Teleport(vector4, num157, num159);
						Main.npc[num156].netOffset *= 0f;
						return;
					}
					if (num158 == 2)
					{
						Main.player[num156].Teleport(vector4, num157, num159);
						if (Main.netMode != 2)
						{
							return;
						}
						RemoteClient.CheckSection(this.whoAmI, vector4, 1);
						NetMessage.TrySendData(65, -1, -1, null, 0, (float)num156, vector4.X, vector4.Y, num157, flag18.ToInt(), num159);
						int num160 = -1;
						float num161 = 9999f;
						for (int num162 = 0; num162 < 255; num162++)
						{
							if (Main.player[num162].active && num162 != this.whoAmI)
							{
								Vector2 vector5 = Main.player[num162].position - Main.player[this.whoAmI].position;
								if (vector5.Length() < num161)
								{
									num161 = vector5.Length();
									num160 = num162;
								}
							}
						}
						if (num160 >= 0)
						{
							ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Game.HasTeleportedTo", new object[]
							{
								Main.player[this.whoAmI].name,
								Main.player[num160].name
							}), new Color(250, 250, 0), -1);
							return;
						}
						return;
					}
					else
					{
						if (num158 == 3)
						{
							Player player14 = Main.player[num156];
							int unacknowledgedTeleports = player14.unacknowledgedTeleports;
							player14.unacknowledgedTeleports = unacknowledgedTeleports - 1;
							return;
						}
						return;
					}
				}
				break;
			}
			case 66:
			{
				int num163 = (int)this.reader.ReadByte();
				int num164 = (int)this.reader.ReadInt16();
				if (num164 <= 0)
				{
					return;
				}
				Player player15 = Main.player[num163];
				player15.statLife += num164;
				if (player15.statLife > player15.statLifeMax2)
				{
					player15.statLife = player15.statLifeMax2;
				}
				player15.HealEffect(num164, false);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(66, -1, this.whoAmI, null, num163, (float)num164, 0f, 0f, 0, 0, 0);
					return;
				}
				return;
			}
			case 68:
				this.reader.ReadString();
				return;
			case 69:
			{
				int num165 = (int)this.reader.ReadInt16();
				int num166 = (int)this.reader.ReadInt16();
				int num167 = (int)this.reader.ReadInt16();
				if (Main.netMode == 1)
				{
					if (num165 < 0 || num165 >= 8000)
					{
						return;
					}
					Chest chest3 = Main.chest[num165];
					if (chest3 == null)
					{
						chest3 = Chest.CreateWorldChest(num165, num166, num167);
					}
					else if (chest3.x != num166 || chest3.y != num167)
					{
						return;
					}
					chest3.name = this.reader.ReadString();
					return;
				}
				else
				{
					if (num165 < -1 || num165 >= 8000)
					{
						return;
					}
					if (num165 == -1)
					{
						num165 = Chest.FindChest(num166, num167);
						if (num165 == -1)
						{
							return;
						}
					}
					Chest chest4 = Main.chest[num165];
					if (chest4.x != num166 || chest4.y != num167)
					{
						return;
					}
					NetMessage.TrySendData(69, this.whoAmI, -1, null, num165, (float)num166, (float)num167, 0f, 0, 0, 0);
					return;
				}
				break;
			}
			case 70:
			{
				if (Main.netMode != 2)
				{
					return;
				}
				int num168 = (int)this.reader.ReadInt16();
				int who = (int)this.reader.ReadByte();
				if (Main.netMode == 2)
				{
					who = this.whoAmI;
				}
				if (num168 < Main.maxNPCs && num168 >= 0)
				{
					NPC.CatchNPC(num168, who);
					return;
				}
				return;
			}
			case 71:
			{
				if (Main.netMode != 2)
				{
					return;
				}
				int x2 = this.reader.ReadInt32();
				int y2 = this.reader.ReadInt32();
				int type9 = (int)this.reader.ReadInt16();
				byte style = this.reader.ReadByte();
				NPC.ReleaseNPC(x2, y2, type9, (int)style, this.whoAmI);
				return;
			}
			case 72:
				if (Main.netMode != 1)
				{
					return;
				}
				for (int num169 = 0; num169 < Main.TravelShopMaxSlots; num169++)
				{
					Main.travelShop[num169] = (int)this.reader.ReadInt16();
				}
				return;
			case 73:
				switch (this.reader.ReadByte())
				{
				case 0:
					Main.player[this.whoAmI].TeleportationPotion();
					return;
				case 1:
					Main.player[this.whoAmI].MagicConch();
					return;
				case 2:
					Main.player[this.whoAmI].DemonConch();
					return;
				case 3:
					Main.player[this.whoAmI].Shellphone_Spawn();
					return;
				case 4:
					Main.player[this.whoAmI].PlayerNoSpaceTeleport();
					return;
				default:
					return;
				}
				break;
			case 74:
				if (Main.netMode != 1)
				{
					return;
				}
				Main.anglerQuest = (int)this.reader.ReadByte();
				Main.anglerQuestFinished = this.reader.ReadBoolean();
				return;
			case 75:
			{
				if (Main.netMode != 2)
				{
					return;
				}
				string name2 = Main.player[this.whoAmI].name;
				if (!Main.anglerWhoFinishedToday.Contains(name2))
				{
					Main.anglerWhoFinishedToday.Add(name2);
					return;
				}
				return;
			}
			case 76:
			{
				int num170 = (int)this.reader.ReadByte();
				if (num170 == Main.myPlayer && !Main.ServerSideCharacter)
				{
					return;
				}
				if (Main.netMode == 2)
				{
					num170 = this.whoAmI;
				}
				Player player16 = Main.player[num170];
				player16.anglerQuestsFinished = this.reader.ReadInt32();
				player16.golferScoreAccumulated = this.reader.ReadInt32();
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(76, -1, this.whoAmI, null, num170, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				return;
			}
			case 77:
			{
				int type10 = (int)this.reader.ReadInt16();
				ushort tileType = this.reader.ReadUInt16();
				short x3 = this.reader.ReadInt16();
				short y3 = this.reader.ReadInt16();
				Animation.NewTemporaryAnimation(type10, tileType, (int)x3, (int)y3);
				return;
			}
			case 78:
				if (Main.netMode != 1)
				{
					return;
				}
				Main.ReportInvasionProgress(this.reader.ReadInt32(), this.reader.ReadInt32(), (int)this.reader.ReadSByte(), (int)this.reader.ReadSByte());
				return;
			case 79:
			{
				int x4 = (int)this.reader.ReadInt16();
				int y4 = (int)this.reader.ReadInt16();
				short type11 = this.reader.ReadInt16();
				int style2 = (int)this.reader.ReadInt16();
				int num171 = (int)this.reader.ReadByte();
				int random = (int)this.reader.ReadSByte();
				int direction;
				if (this.reader.ReadBoolean())
				{
					direction = 1;
				}
				else
				{
					direction = -1;
				}
				if (Main.netMode == 2)
				{
					Netplay.Clients[this.whoAmI].SpamAddBlock += 1f;
					if (!WorldGen.InWorld(x4, y4, 10) || !Netplay.Clients[this.whoAmI].TileSections[Netplay.GetSectionX(x4), Netplay.GetSectionY(y4)])
					{
						return;
					}
				}
				WorldGen.PlaceObject(x4, y4, (int)type11, false, style2, num171, random, direction);
				if (Main.netMode == 2)
				{
					NetMessage.SendObjectPlacement(this.whoAmI, x4, y4, (int)type11, style2, num171, random, direction);
					return;
				}
				return;
			}
			case 80:
			{
				if (Main.netMode != 1)
				{
					return;
				}
				int num172 = (int)this.reader.ReadByte();
				int num173 = (int)this.reader.ReadInt16();
				if (num173 >= -3 && num173 < 8000)
				{
					Main.player[num172].chest = num173;
					return;
				}
				return;
			}
			case 81:
			{
				if (Main.netMode != 1)
				{
					return;
				}
				int x5 = (int)this.reader.ReadSingle();
				int y5 = (int)this.reader.ReadSingle();
				Color color3 = this.reader.ReadRGB();
				int amount = this.reader.ReadInt32();
				CombatText.NewText(new Rectangle(x5, y5, 0, 0), color3, amount, false, false);
				return;
			}
			case 82:
				NetManager.Instance.Read(this.reader, this.whoAmI, length);
				return;
			case 84:
			{
				int num174 = (int)this.reader.ReadByte();
				if (Main.netMode == 2)
				{
					num174 = this.whoAmI;
				}
				float stealth = this.reader.ReadSingle();
				Main.player[num174].stealth = stealth;
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(84, -1, this.whoAmI, null, num174, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				return;
			}
			case 85:
				if (Main.netMode == 2 && this.whoAmI < 255)
				{
					Player player17 = Main.player[this.whoAmI];
					QuickStacking.SourceInventory inventory = QuickStacking.ReadNetInventory(player17, this.reader);
					bool smartStack = this.reader.ReadBoolean();
					QuickStacking.QuickStackToNearbyChests(player17, inventory, smartStack);
					return;
				}
				if (Main.netMode == 1)
				{
					QuickStacking.IndicateBlockedChests(Main.LocalPlayer, QuickStacking.ReadBlockedChestList(this.reader));
					return;
				}
				return;
			case 86:
			{
				if (Main.netMode != 1)
				{
					return;
				}
				int id = this.reader.ReadInt32();
				if (this.reader.ReadBoolean())
				{
					TileEntity tileEntity = TileEntity.Read(this.reader, 318, true);
					tileEntity.ID = id;
					TileEntity.Add(tileEntity);
					return;
				}
				TileEntity entity;
				if (TileEntity.TryGet<TileEntity>(id, out entity))
				{
					TileEntity.Remove(entity, false);
					return;
				}
				return;
			}
			case 87:
			{
				if (Main.netMode != 2)
				{
					return;
				}
				int x6 = (int)this.reader.ReadInt16();
				int y6 = (int)this.reader.ReadInt16();
				int type12 = (int)this.reader.ReadByte();
				if (!WorldGen.InWorld(x6, y6, 0))
				{
					return;
				}
				TileEntity tileEntity2;
				if (TileEntity.TryGetAt<TileEntity>(x6, y6, out tileEntity2))
				{
					return;
				}
				TileEntity.PlaceEntityNet(x6, y6, type12);
				return;
			}
			case 88:
			{
				if (Main.netMode != 1)
				{
					return;
				}
				int num175 = (int)this.reader.ReadInt16();
				if (num175 < 0 || num175 > 400)
				{
					return;
				}
				Item inner = Main.item[num175].inner;
				BitsByte bitsByte30 = this.reader.ReadByte();
				if (bitsByte30[0])
				{
					inner.color.PackedValue = this.reader.ReadUInt32();
				}
				if (bitsByte30[1])
				{
					inner.damage = (int)this.reader.ReadUInt16();
				}
				if (bitsByte30[2])
				{
					inner.knockBack = this.reader.ReadSingle();
				}
				if (bitsByte30[3])
				{
					inner.useAnimation = (int)this.reader.ReadUInt16();
				}
				if (bitsByte30[4])
				{
					inner.useTime = (int)this.reader.ReadUInt16();
				}
				if (bitsByte30[5])
				{
					inner.shoot = (int)this.reader.ReadInt16();
				}
				if (bitsByte30[6])
				{
					inner.shootSpeed = this.reader.ReadSingle();
				}
				if (!bitsByte30[7])
				{
					return;
				}
				bitsByte30 = this.reader.ReadByte();
				if (bitsByte30[0])
				{
					inner.width = (int)this.reader.ReadInt16();
				}
				if (bitsByte30[1])
				{
					inner.height = (int)this.reader.ReadInt16();
				}
				if (bitsByte30[2])
				{
					inner.scale = this.reader.ReadSingle();
				}
				if (bitsByte30[3])
				{
					inner.ammo = (int)this.reader.ReadInt16();
				}
				if (bitsByte30[4])
				{
					inner.useAmmo = (int)this.reader.ReadInt16();
				}
				if (bitsByte30[5])
				{
					inner.notAmmo = this.reader.ReadBoolean();
					return;
				}
				return;
			}
			case 89:
			{
				if (Main.netMode != 2)
				{
					return;
				}
				int x7 = (int)this.reader.ReadInt16();
				int y7 = (int)this.reader.ReadInt16();
				int type13 = (int)this.reader.ReadInt16();
				int prefix2 = (int)this.reader.ReadByte();
				int stack4 = (int)this.reader.ReadInt16();
				TEItemFrame.TryPlacing(x7, y7, type13, prefix2, stack4);
				return;
			}
			case 91:
			{
				if (Main.netMode != 1)
				{
					return;
				}
				int num176 = this.reader.ReadInt32();
				int num177 = (int)this.reader.ReadByte();
				if (num177 != 255)
				{
					int num178 = (int)this.reader.ReadUInt16();
					int num179 = (int)this.reader.ReadUInt16();
					int num180 = (int)this.reader.ReadByte();
					int metadata = 0;
					if (num180 < 0)
					{
						metadata = (int)this.reader.ReadInt16();
					}
					WorldUIAnchor worldUIAnchor = EmoteBubble.DeserializeNetAnchor(num177, num178);
					if (num177 == 1)
					{
						Main.player[num178].emoteTime = 360;
					}
					Dictionary<int, EmoteBubble> byID = EmoteBubble.byID;
					lock (byID)
					{
						if (!EmoteBubble.byID.ContainsKey(num176))
						{
							EmoteBubble.byID[num176] = new EmoteBubble(num180, worldUIAnchor, num179);
						}
						else
						{
							EmoteBubble.byID[num176].lifeTime = num179;
							EmoteBubble.byID[num176].lifeTimeStart = num179;
							EmoteBubble.byID[num176].emote = num180;
							EmoteBubble.byID[num176].anchor = worldUIAnchor;
						}
						EmoteBubble.byID[num176].ID = num176;
						EmoteBubble.byID[num176].metadata = metadata;
						EmoteBubble.OnBubbleChange(num176);
						return;
					}
					goto IL_7A37;
				}
				if (EmoteBubble.byID.ContainsKey(num176))
				{
					EmoteBubble.byID.Remove(num176);
					return;
				}
				return;
			}
			case 92:
				goto IL_7A37;
			case 94:
			{
				string a2 = this.reader.ReadString();
				this.reader.ReadInt32();
				int num181 = (int)this.reader.ReadSingle();
				this.reader.ReadSingle();
				if (!DebugOptions.enableDebugCommands)
				{
					return;
				}
				if (a2 == "/showdebug")
				{
					DebugOptions.Shared_ReportCommandUsage = (num181 == 1);
					return;
				}
				if (a2 == "/setserverping")
				{
					DebugOptions.Shared_ServerPing = num181;
					DebugNetworkStream.Latency = (uint)(num181 / 2);
					return;
				}
				return;
			}
			case 95:
			{
				ushort num182 = this.reader.ReadUInt16();
				int num183 = (int)this.reader.ReadByte();
				if (Main.netMode != 2)
				{
					return;
				}
				for (int num184 = 0; num184 < 1000; num184++)
				{
					if (Main.projectile[num184].owner == (int)num182 && Main.projectile[num184].active && Main.projectile[num184].type == 602 && Main.projectile[num184].ai[1] == (float)num183)
					{
						Main.projectile[num184].Kill();
						NetMessage.TrySendData(29, -1, -1, null, Main.projectile[num184].identity, (float)num182, 0f, 0f, 0, 0, 0);
						return;
					}
				}
				return;
			}
			case 96:
			{
				int num185 = (int)this.reader.ReadByte();
				if (Main.netMode == 2)
				{
					num185 = this.whoAmI;
				}
				Player player18 = Main.player[num185];
				int num186 = (int)this.reader.ReadInt16();
				Vector2 vector6 = this.reader.ReadVector2();
				Vector2 velocity5 = this.reader.ReadVector2();
				int lastPortalColorIndex = num186 + ((num186 % 2 == 0) ? 1 : -1);
				player18.lastPortalColorIndex = lastPortalColorIndex;
				player18.Teleport(vector6, 4, num186);
				player18.velocity = velocity5;
				if (Main.netMode == 2)
				{
					NetMessage.SendData(96, -1, num185, null, num185, vector6.X, vector6.Y, (float)num186, 0, 0, 0);
					return;
				}
				return;
			}
			case 97:
				if (Main.netMode != 1)
				{
					return;
				}
				AchievementsHelper.NotifyNPCKilledDirect(Main.player[Main.myPlayer], (int)this.reader.ReadInt16());
				return;
			case 98:
				if (Main.netMode != 1)
				{
					return;
				}
				AchievementsHelper.NotifyProgressionEvent((int)this.reader.ReadInt16());
				return;
			case 99:
			{
				int num187 = (int)this.reader.ReadByte();
				if (Main.netMode == 2)
				{
					num187 = this.whoAmI;
				}
				Main.player[num187].MinionRestTargetPoint = this.reader.ReadVector2();
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(99, -1, this.whoAmI, null, num187, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				return;
			}
			case 100:
			{
				int num188 = (int)this.reader.ReadUInt16();
				NPC npc6 = Main.npc[num188];
				int num189 = (int)this.reader.ReadInt16();
				Vector2 newPos = this.reader.ReadVector2();
				Vector2 velocity6 = this.reader.ReadVector2();
				int lastPortalColorIndex2 = num189 + ((num189 % 2 == 0) ? 1 : -1);
				npc6.lastPortalColorIndex = lastPortalColorIndex2;
				npc6.Teleport(newPos, 4, num189);
				npc6.velocity = velocity6;
				npc6.netOffset *= 0f;
				return;
			}
			case 101:
				if (Main.netMode == 2)
				{
					return;
				}
				NPC.ShieldStrengthTowerSolar = (int)this.reader.ReadUInt16();
				NPC.ShieldStrengthTowerVortex = (int)this.reader.ReadUInt16();
				NPC.ShieldStrengthTowerNebula = (int)this.reader.ReadUInt16();
				NPC.ShieldStrengthTowerStardust = (int)this.reader.ReadUInt16();
				if (NPC.ShieldStrengthTowerSolar < 0)
				{
					NPC.ShieldStrengthTowerSolar = 0;
				}
				if (NPC.ShieldStrengthTowerVortex < 0)
				{
					NPC.ShieldStrengthTowerVortex = 0;
				}
				if (NPC.ShieldStrengthTowerNebula < 0)
				{
					NPC.ShieldStrengthTowerNebula = 0;
				}
				if (NPC.ShieldStrengthTowerStardust < 0)
				{
					NPC.ShieldStrengthTowerStardust = 0;
				}
				if (NPC.ShieldStrengthTowerSolar > NPC.LunarShieldPowerMax)
				{
					NPC.ShieldStrengthTowerSolar = NPC.LunarShieldPowerMax;
				}
				if (NPC.ShieldStrengthTowerVortex > NPC.LunarShieldPowerMax)
				{
					NPC.ShieldStrengthTowerVortex = NPC.LunarShieldPowerMax;
				}
				if (NPC.ShieldStrengthTowerNebula > NPC.LunarShieldPowerMax)
				{
					NPC.ShieldStrengthTowerNebula = NPC.LunarShieldPowerMax;
				}
				if (NPC.ShieldStrengthTowerStardust > NPC.LunarShieldPowerMax)
				{
					NPC.ShieldStrengthTowerStardust = NPC.LunarShieldPowerMax;
					return;
				}
				return;
			case 102:
			{
				int num190 = (int)this.reader.ReadByte();
				ushort num191 = this.reader.ReadUInt16();
				Vector2 vector7 = this.reader.ReadVector2();
				if (Main.netMode == 2)
				{
					num190 = this.whoAmI;
					NetMessage.TrySendData(102, -1, -1, null, num190, (float)num191, vector7.X, vector7.Y, 0, 0, 0);
					return;
				}
				Player player19 = Main.player[num190];
				for (int num192 = 0; num192 < 255; num192++)
				{
					Player player20 = Main.player[num192];
					if (player20.active && !player20.dead && (player19.team == 0 || player19.team == player20.team) && player20.Distance(vector7) < 700f)
					{
						Vector2 value3 = player19.Center - player20.Center;
						Vector2 vector8 = Vector2.Normalize(value3);
						if (!vector8.HasNaNs())
						{
							int type14 = 90;
							float num193 = 0f;
							float num194 = 0.20943952f;
							Vector2 spinningpoint = new Vector2(0f, -8f);
							Vector2 value4 = new Vector2(-3f);
							float num195 = 0f;
							float num196 = 0.005f;
							if (num191 != 173)
							{
								if (num191 != 176)
								{
									if (num191 == 179)
									{
										type14 = 86;
									}
								}
								else
								{
									type14 = 88;
								}
							}
							else
							{
								type14 = 90;
							}
							int num197 = 0;
							while ((float)num197 < value3.Length() / 6f)
							{
								Vector2 position4 = player20.Center + 6f * (float)num197 * vector8 + spinningpoint.RotatedBy((double)num193, default(Vector2)) + value4;
								num193 += num194;
								int num198 = Dust.NewDust(position4, 6, 6, type14, 0f, 0f, 100, default(Color), 1.5f);
								Main.dust[num198].noGravity = true;
								Main.dust[num198].velocity = Vector2.Zero;
								num195 = (Main.dust[num198].fadeIn = num195 + num196);
								Main.dust[num198].velocity += vector8 * 1.5f;
								num197++;
							}
						}
						player20.NebulaLevelup((int)num191);
					}
				}
				return;
			}
			case 103:
				if (Main.netMode == 1)
				{
					NPC.MaxMoonLordCountdown = this.reader.ReadInt32();
					NPC.MoonLordCountdown = this.reader.ReadInt32();
					return;
				}
				return;
			case 104:
			{
				if (Main.netMode != 1 || Main.npcShop <= 0)
				{
					return;
				}
				Item[] item3 = Main.instance.shop[Main.npcShop].item;
				int num199 = (int)this.reader.ReadByte();
				int type15 = (int)this.reader.ReadInt16();
				int stack5 = (int)this.reader.ReadInt16();
				int prefixWeWant3 = (int)this.reader.ReadByte();
				int value5 = this.reader.ReadInt32();
				BitsByte bitsByte31 = this.reader.ReadByte();
				if (num199 < item3.Length)
				{
					item3[num199] = new Item();
					item3[num199].SetDefaults(type15, null);
					item3[num199].stack = stack5;
					item3[num199].Prefix(prefixWeWant3);
					item3[num199].value = value5;
					item3[num199].buyOnce = bitsByte31[0];
					return;
				}
				return;
			}
			case 105:
			{
				if (Main.netMode == 1)
				{
					return;
				}
				int i3 = (int)this.reader.ReadInt16();
				int j3 = (int)this.reader.ReadInt16();
				bool on = this.reader.ReadBoolean();
				WorldGen.ToggleGemLock(i3, j3, on);
				return;
			}
			case 106:
			{
				if (Main.netMode != 1)
				{
					return;
				}
				HalfVector2 halfVector = default(HalfVector2);
				halfVector.PackedValue = this.reader.ReadUInt32();
				Utils.PoofOfSmoke(halfVector.ToVector2());
				return;
			}
			case 107:
			{
				if (Main.netMode != 1)
				{
					return;
				}
				Color c = this.reader.ReadRGB();
				string text2 = NetworkText.Deserialize(this.reader).ToString();
				int widthLimit = (int)this.reader.ReadInt16();
				Main.NewTextMultiline(text2, false, c, widthLimit);
				return;
			}
			case 108:
			{
				if (Main.netMode != 1)
				{
					return;
				}
				int damage2 = (int)this.reader.ReadInt16();
				float knockBack2 = this.reader.ReadSingle();
				int x8 = (int)this.reader.ReadInt16();
				int y8 = (int)this.reader.ReadInt16();
				int angle = (int)this.reader.ReadInt16();
				int ammo = (int)this.reader.ReadInt16();
				int num200 = (int)this.reader.ReadByte();
				if (num200 != Main.myPlayer)
				{
					return;
				}
				WorldGen.ShootFromCannon(x8, y8, angle, ammo, damage2, knockBack2, num200, true);
				return;
			}
			case 109:
			{
				if (Main.netMode != 2)
				{
					return;
				}
				int x9 = (int)this.reader.ReadInt16();
				int y9 = (int)this.reader.ReadInt16();
				int x10 = (int)this.reader.ReadInt16();
				int y10 = (int)this.reader.ReadInt16();
				WiresUI.Settings.MultiToolMode toolMode = (WiresUI.Settings.MultiToolMode)this.reader.ReadByte();
				int num201 = this.whoAmI;
				WiresUI.Settings.MultiToolMode toolMode2 = WiresUI.Settings.ToolMode;
				WiresUI.Settings.ToolMode = toolMode;
				Wiring.MassWireOperation(new Point(x9, y9), new Point(x10, y10), Main.player[num201]);
				WiresUI.Settings.ToolMode = toolMode2;
				return;
			}
			case 110:
			{
				if (Main.netMode != 1)
				{
					return;
				}
				int type16 = (int)this.reader.ReadInt16();
				int num202 = (int)this.reader.ReadInt16();
				int num203 = (int)this.reader.ReadByte();
				if (num203 != Main.myPlayer)
				{
					return;
				}
				Player player21 = Main.player[num203];
				for (int num204 = 0; num204 < num202; num204++)
				{
					player21.ConsumeItem(type16, false, false);
				}
				player21.wireOperationsCooldown = 0;
				return;
			}
			case 111:
				if (Main.netMode != 2)
				{
					return;
				}
				BirthdayParty.ToggleManualParty();
				return;
			case 112:
			{
				int num205 = (int)this.reader.ReadByte();
				int num206 = this.reader.ReadInt32();
				int num207 = this.reader.ReadInt32();
				int num208 = (int)this.reader.ReadByte();
				int num209 = (int)this.reader.ReadInt16();
				bool flag19 = this.reader.ReadByte() == 1;
				if (num205 == 1)
				{
					if (Main.netMode == 1)
					{
						WorldGen.TreeGrowFX(num206, num207, num208, num209, flag19);
					}
					if (Main.netMode == 2)
					{
						NetMessage.TrySendData((int)b, -1, -1, null, num205, (float)num206, (float)num207, (float)num208, num209, flag19 ? 1 : 0, 0);
						return;
					}
					return;
				}
				else
				{
					if (num205 == 2)
					{
						NPC.FairyEffects(new Vector2((float)num206, (float)num207), num209);
						return;
					}
					return;
				}
				break;
			}
			case 113:
			{
				int x11 = (int)this.reader.ReadInt16();
				int y11 = (int)this.reader.ReadInt16();
				if (Main.netMode == 2 && !Main.snowMoon && !Main.pumpkinMoon)
				{
					if (DD2Event.WouldFailSpawningHere(x11, y11))
					{
						DD2Event.FailureMessage(this.whoAmI);
					}
					DD2Event.SummonCrystal(x11, y11, this.whoAmI);
					return;
				}
				return;
			}
			case 114:
				if (Main.netMode != 1)
				{
					return;
				}
				DD2Event.WipeEntities();
				return;
			case 115:
			{
				int num210 = (int)this.reader.ReadByte();
				if (Main.netMode == 2)
				{
					num210 = this.whoAmI;
				}
				Main.player[num210].MinionAttackTargetNPC = (int)this.reader.ReadInt16();
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(115, -1, this.whoAmI, null, num210, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				return;
			}
			case 116:
				if (Main.netMode != 1)
				{
					return;
				}
				DD2Event.TimeLeftBetweenWaves = this.reader.ReadInt32();
				return;
			case 117:
			{
				int num211 = (int)this.reader.ReadByte();
				if (Main.netMode == 2 && this.whoAmI != num211 && (!Main.player[num211].hostile || !Main.player[this.whoAmI].hostile))
				{
					return;
				}
				PlayerDeathReason playerDeathReason = PlayerDeathReason.FromReader(this.reader);
				int damage3 = (int)this.reader.ReadInt16();
				int num212 = (int)(this.reader.ReadByte() - 1);
				BitsByte bitsByte32 = this.reader.ReadByte();
				bool flag20 = bitsByte32[0];
				bool pvp = bitsByte32[1];
				int num213 = (int)this.reader.ReadSByte();
				Main.player[num211].Hurt(playerDeathReason, damage3, num212, pvp, true, flag20, num213, true);
				if (Main.netMode == 2)
				{
					NetMessage.SendPlayerHurt(num211, playerDeathReason, damage3, num212, flag20, pvp, num213, -1, this.whoAmI);
					return;
				}
				return;
			}
			case 118:
			{
				int num214 = (int)this.reader.ReadByte();
				if (Main.netMode == 2)
				{
					num214 = this.whoAmI;
				}
				PlayerDeathReason playerDeathReason2 = PlayerDeathReason.FromReader(this.reader);
				int num215 = (int)this.reader.ReadInt16();
				int num216 = (int)(this.reader.ReadByte() - 1);
				bool pvp2 = this.reader.ReadByte()[0];
				Main.player[num214].KillMe(playerDeathReason2, (double)num215, num216, pvp2);
				if (Main.netMode == 2)
				{
					NetMessage.SendPlayerDeath(num214, playerDeathReason2, num215, num216, pvp2, -1, this.whoAmI);
					return;
				}
				return;
			}
			case 119:
			{
				if (Main.netMode != 1)
				{
					return;
				}
				int x12 = (int)this.reader.ReadSingle();
				int y12 = (int)this.reader.ReadSingle();
				Color color4 = this.reader.ReadRGB();
				NetworkText networkText = NetworkText.Deserialize(this.reader);
				CombatText.NewText(new Rectangle(x12, y12, 0, 0), color4, networkText.ToString(), false, false);
				return;
			}
			case 120:
			{
				int num217 = (int)this.reader.ReadByte();
				if (Main.netMode == 2)
				{
					num217 = this.whoAmI;
				}
				int num218 = (int)this.reader.ReadByte();
				if (num218 >= 0 && num218 < EmoteID.Count && Main.netMode == 2)
				{
					EmoteBubble.NewBubble(num218, new WorldUIAnchor(Main.player[num217]), 360);
					EmoteBubble.CheckForNPCsToReactToEmoteBubble(num218, Main.player[num217]);
					return;
				}
				return;
			}
			case 121:
			{
				int num219 = (int)this.reader.ReadByte();
				if (Main.netMode == 2)
				{
					num219 = this.whoAmI;
				}
				int num220 = this.reader.ReadInt32();
				int num221 = (int)this.reader.ReadByte();
				int num222 = (int)this.reader.ReadByte();
				TEDisplayDoll tedisplayDoll;
				if (!TileEntity.TryGet<TEDisplayDoll>(num220, out tedisplayDoll))
				{
					TEDisplayDoll.ReadDummySync(num221, num222, this.reader);
					return;
				}
				tedisplayDoll.ReadData(num221, num222, this.reader);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData((int)b, -1, num219, null, num219, (float)num220, (float)num221, (float)num222, 0, 0, 0);
					return;
				}
				return;
			}
			case 122:
			{
				int num223 = this.reader.ReadInt32();
				int num224 = (int)this.reader.ReadByte();
				if (Main.netMode == 2)
				{
					num224 = this.whoAmI;
				}
				if (Main.netMode == 2)
				{
					if (num223 == -1)
					{
						Main.player[num224].tileEntityAnchor.Clear();
						NetMessage.TrySendData((int)b, -1, -1, null, num223, (float)num224, 0f, 0f, 0, 0, 0);
						return;
					}
					int num225;
					TileEntity tileEntity3;
					if (!TileEntity.IsOccupied(num223, out num225) && TileEntity.TryGet<TileEntity>(num223, out tileEntity3))
					{
						Main.player[num224].tileEntityAnchor.Set(num223, (int)tileEntity3.Position.X, (int)tileEntity3.Position.Y);
						NetMessage.TrySendData((int)b, -1, -1, null, num223, (float)num224, 0f, 0f, 0, 0, 0);
					}
				}
				if (Main.netMode != 1)
				{
					return;
				}
				if (num223 == -1)
				{
					Main.player[num224].tileEntityAnchor.Clear();
					return;
				}
				TileEntity tileEntity4;
				if (TileEntity.TryGet<TileEntity>(num223, out tileEntity4))
				{
					TileEntity.SetInteractionAnchor(Main.player[num224], (int)tileEntity4.Position.X, (int)tileEntity4.Position.Y, num223);
					return;
				}
				return;
			}
			case 123:
			{
				if (Main.netMode != 2)
				{
					return;
				}
				int x13 = (int)this.reader.ReadInt16();
				int y13 = (int)this.reader.ReadInt16();
				int type17 = (int)this.reader.ReadInt16();
				int prefix3 = (int)this.reader.ReadByte();
				int stack6 = (int)this.reader.ReadInt16();
				TEWeaponsRack.TryPlacing(x13, y13, type17, prefix3, stack6);
				return;
			}
			case 124:
			{
				int num226 = (int)this.reader.ReadByte();
				if (Main.netMode == 2)
				{
					num226 = this.whoAmI;
				}
				int num227 = this.reader.ReadInt32();
				int num228 = (int)this.reader.ReadByte();
				bool flag21 = false;
				if (num228 >= 2)
				{
					flag21 = true;
					num228 -= 2;
				}
				TEHatRack tehatRack;
				if (!TileEntity.TryGet<TEHatRack>(num227, out tehatRack) || num228 >= 2)
				{
					this.reader.ReadInt32();
					this.reader.ReadByte();
					return;
				}
				tehatRack.ReadItem(num228, this.reader, flag21);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData((int)b, -1, num226, null, num226, (float)num227, (float)num228, (float)flag21.ToInt(), 0, 0, 0);
					return;
				}
				return;
			}
			case 125:
			{
				int num229 = (int)this.reader.ReadByte();
				int num230 = (int)this.reader.ReadInt16();
				int num231 = (int)this.reader.ReadInt16();
				int num232 = (int)this.reader.ReadByte();
				if (Main.netMode == 2)
				{
					num229 = this.whoAmI;
				}
				if (Main.netMode == 1)
				{
					Main.player[Main.myPlayer].GetOtherPlayersPickTile(num230, num231, num232);
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(125, -1, num229, null, num229, (float)num230, (float)num231, (float)num232, 0, 0, 0);
					return;
				}
				return;
			}
			case 126:
				if (Main.netMode != 1)
				{
					return;
				}
				NPC.RevengeManager.AddMarkerFromReader(this.reader);
				return;
			case 127:
			{
				int markerUniqueID = this.reader.ReadInt32();
				if (Main.netMode != 1)
				{
					return;
				}
				NPC.RevengeManager.DestroyMarker(markerUniqueID);
				return;
			}
			case 128:
			{
				int num233 = (int)this.reader.ReadByte();
				int num234 = (int)this.reader.ReadUInt16();
				int num235 = (int)this.reader.ReadUInt16();
				int num236 = (int)this.reader.ReadUInt16();
				int num237 = (int)this.reader.ReadUInt16();
				if (Main.netMode == 2)
				{
					NetMessage.SendData(128, -1, num233, null, num233, (float)num236, (float)num237, 0f, num234, num235, 0);
					return;
				}
				GolfHelper.ContactListener.PutBallInCup_TextAndEffects(new Point(num234, num235), num233, num236, num237);
				return;
			}
			case 129:
				if (Main.netMode != 1)
				{
					return;
				}
				if (Main.LocalPlayer.team > 0)
				{
					NetMessage.SendData(45, -1, -1, null, Main.myPlayer, 0f, 0f, 0f, 0, 0, 0);
				}
				Main.FixUIScale();
				Main.TrySetPreparationState(Main.WorldPreparationState.ProcessingData);
				return;
			case 130:
			{
				if (Main.netMode != 2)
				{
					return;
				}
				int num238 = (int)this.reader.ReadUInt16();
				int num239 = (int)this.reader.ReadUInt16();
				int num240 = (int)this.reader.ReadInt16();
				if (num240 == 682)
				{
					if (NPC.unlockedSlimeRedSpawn)
					{
						return;
					}
					NPC.unlockedSlimeRedSpawn = true;
					NetMessage.TrySendData(7, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
				}
				num238 *= 16;
				num239 *= 16;
				NPC npc7 = new NPC();
				npc7.SetDefaults(num240, default(NPCSpawnParams));
				int type18 = npc7.type;
				int netID = npc7.netID;
				int num241 = NPC.NewNPC(new EntitySource_FishedOut(Main.player[this.whoAmI]), num238, num239, num240, 0, 0f, 0f, 0f, 0f, 255);
				if (netID != type18)
				{
					Main.npc[num241].SetDefaults(netID, default(NPCSpawnParams));
					NetMessage.TrySendData(23, -1, -1, null, num241, 0f, 0f, 0f, 0, 0, 0);
				}
				if (num240 == 682)
				{
					WorldGen.CheckAchievement_RealEstateAndTownSlimes();
					return;
				}
				return;
			}
			case 131:
			{
				if (Main.netMode != 1)
				{
					return;
				}
				int num242 = (int)this.reader.ReadUInt16();
				NPC npc8;
				if (num242 < Main.maxNPCs)
				{
					npc8 = Main.npc[num242];
				}
				else
				{
					npc8 = new NPC();
				}
				int num243 = (int)this.reader.ReadByte();
				if (num243 == 1)
				{
					int time2 = this.reader.ReadInt32();
					int fromWho = (int)this.reader.ReadInt16();
					npc8.GetImmuneTime(fromWho, time2);
					return;
				}
				return;
			}
			case 132:
			{
				if (Main.netMode != 1)
				{
					return;
				}
				Point point = this.reader.ReadVector2().ToPoint();
				ushort key = this.reader.ReadUInt16();
				LegacySoundStyle legacySoundStyle = SoundID.SoundByIndex[key];
				BitsByte bitsByte33 = this.reader.ReadByte();
				int style3;
				if (bitsByte33[0])
				{
					style3 = this.reader.ReadInt32();
				}
				else
				{
					style3 = legacySoundStyle.Style;
				}
				float volumeScale;
				if (bitsByte33[1])
				{
					volumeScale = MathHelper.Clamp(this.reader.ReadSingle(), 0f, 1f);
				}
				else
				{
					volumeScale = legacySoundStyle.Volume;
				}
				float pitchOffset;
				if (bitsByte33[2])
				{
					pitchOffset = MathHelper.Clamp(this.reader.ReadSingle(), -1f, 1f);
				}
				else
				{
					pitchOffset = legacySoundStyle.GetRandomPitch();
				}
				SoundEngine.PlaySound(legacySoundStyle.SoundId, point.X, point.Y, style3, volumeScale, pitchOffset);
				return;
			}
			case 133:
			{
				if (Main.netMode != 2)
				{
					return;
				}
				int x14 = (int)this.reader.ReadInt16();
				int y14 = (int)this.reader.ReadInt16();
				int type19 = (int)this.reader.ReadInt16();
				int prefix4 = (int)this.reader.ReadByte();
				int stack7 = (int)this.reader.ReadInt16();
				TEFoodPlatter.TryPlacing(x14, y14, type19, prefix4, stack7);
				return;
			}
			case 134:
			{
				int num244 = (int)this.reader.ReadByte();
				int ladyBugLuckTimeLeft = this.reader.ReadInt32();
				float torchLuck = this.reader.ReadSingle();
				byte luckPotion = this.reader.ReadByte();
				bool hasGardenGnomeNearby = this.reader.ReadBoolean();
				bool brokenMirrorBadLuck = this.reader.ReadBoolean();
				float equipmentBasedLuckBonus = this.reader.ReadSingle();
				float coinLuck = this.reader.ReadSingle();
				byte kiteLuckLevel = this.reader.ReadByte();
				if (Main.netMode == 2)
				{
					num244 = this.whoAmI;
				}
				Player player22 = Main.player[num244];
				player22.ladyBugLuckTimeLeft = ladyBugLuckTimeLeft;
				player22.torchLuck = torchLuck;
				player22.luckPotion = luckPotion;
				player22.HasGardenGnomeNearby = hasGardenGnomeNearby;
				player22.brokenMirrorBadLuck = brokenMirrorBadLuck;
				player22.equipmentBasedLuckBonus = equipmentBasedLuckBonus;
				player22.coinLuck = coinLuck;
				player22.kiteLuckLevel = kiteLuckLevel;
				player22.RecalculateLuck();
				if (Main.netMode == 2)
				{
					NetMessage.SendData(134, -1, num244, null, num244, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				return;
			}
			case 135:
			{
				int num245 = (int)this.reader.ReadByte();
				if (Main.netMode == 1)
				{
					Main.player[num245].immuneAlpha = 255;
					return;
				}
				return;
			}
			case 136:
				for (int num246 = 0; num246 < 2; num246++)
				{
					for (int num247 = 0; num247 < 3; num247++)
					{
						NPC.cavernMonsterType[num246, num247] = (int)this.reader.ReadUInt16();
					}
				}
				return;
			case 137:
			{
				if (Main.netMode != 2)
				{
					return;
				}
				int num248 = (int)this.reader.ReadInt16();
				int buffTypeToRemove = (int)this.reader.ReadUInt16();
				if (num248 >= 0 && num248 < Main.maxNPCs)
				{
					Main.npc[num248].RequestBuffRemoval(buffTypeToRemove);
					return;
				}
				return;
			}
			case 138:
				goto IL_A04E;
			case 139:
				if (Main.netMode != 2)
				{
					int num249 = (int)this.reader.ReadByte();
					bool flag22 = this.reader.ReadBoolean();
					Main.countsAsHostForGameplay[num249] = flag22;
					return;
				}
				return;
			case 140:
			{
				int num250 = (int)this.reader.ReadByte();
				int num251 = this.reader.ReadInt32();
				switch (num250)
				{
				case 0:
					if (Main.netMode != 1)
					{
						return;
					}
					CreditsRollEvent.SetRemainingTimeDirect(num251);
					return;
				case 1:
					if (Main.netMode != 2)
					{
						return;
					}
					NPC.TransformCopperSlime(num251);
					return;
				case 2:
					if (Main.netMode != 2)
					{
						return;
					}
					NPC.TransformElderSlime(num251);
					return;
				default:
					return;
				}
				break;
			}
			case 141:
			{
				LucyAxeMessage.MessageSource messageSource = (LucyAxeMessage.MessageSource)this.reader.ReadByte();
				byte b18 = this.reader.ReadByte();
				Vector2 vector9 = this.reader.ReadVector2();
				int num252 = this.reader.ReadInt32();
				int num253 = this.reader.ReadInt32();
				if (Main.netMode == 2)
				{
					NetMessage.SendData(141, -1, this.whoAmI, null, (int)messageSource, (float)b18, vector9.X, vector9.Y, num252, num253, 0);
					return;
				}
				LucyAxeMessage.CreateFromNet(messageSource, b18, new Vector2((float)num252, (float)num253), vector9);
				return;
			}
			case 142:
			{
				int num254 = (int)this.reader.ReadByte();
				if (Main.netMode == 2)
				{
					num254 = this.whoAmI;
				}
				Player player23 = Main.player[num254];
				player23.piggyBankProjTracker.TryReading(this.reader);
				player23.voidLensChest.TryReading(this.reader);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(142, -1, this.whoAmI, null, num254, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				return;
			}
			case 143:
				if (Main.netMode != 2)
				{
					return;
				}
				DD2Event.AttemptToSkipWaitTime();
				return;
			case 144:
				if (Main.netMode != 2)
				{
					return;
				}
				NPC.HaveDryadDoStardewAnimation();
				return;
			case 146:
			{
				int num255 = (int)this.reader.ReadByte();
				if (num255 == 0)
				{
					WorldItem.ShimmerEffect(this.reader.ReadVector2());
					return;
				}
				if (num255 == 1)
				{
					Vector2 coinPosition = this.reader.ReadVector2();
					int coinAmount = this.reader.ReadInt32();
					Main.player[Main.myPlayer].AddCoinLuck(coinPosition, coinAmount);
					return;
				}
				return;
			}
			case 147:
			{
				int num256 = (int)this.reader.ReadByte();
				if (Main.netMode == 2)
				{
					num256 = this.whoAmI;
				}
				int num257 = (int)this.reader.ReadByte();
				Main.player[num256].TrySwitchingLoadout(num257);
				MessageBuffer.ReadAccessoryVisibility(this.reader, Main.player[num256].hideVisibleAccessory);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData((int)b, -1, num256, null, num256, (float)num257, 0f, 0f, 0, 0, 0);
					return;
				}
				return;
			}
			case 149:
			{
				if (Main.netMode != 2)
				{
					return;
				}
				int x15 = (int)this.reader.ReadInt16();
				int y15 = (int)this.reader.ReadInt16();
				int type20 = (int)this.reader.ReadInt16();
				int prefix5 = (int)this.reader.ReadByte();
				int stack8 = (int)this.reader.ReadInt16();
				TEDeadCellsDisplayJar.TryPlacing(x15, y15, type20, prefix5, stack8);
				return;
			}
			case 150:
			{
				int num258 = (int)this.reader.ReadByte();
				if (Main.netMode == 2)
				{
					num258 = this.whoAmI;
				}
				int num259 = (int)this.reader.ReadInt16();
				Player player24 = Main.player[num258];
				if (Main.netMode == 2)
				{
					if (num259 >= 0)
					{
						player24.SetOrRequestSpectating(num259);
						return;
					}
					player24.spectating = -1;
					NetMessage.SendData(150, -1, this.whoAmI, null, this.whoAmI, (float)num259, 0f, 0f, 0, 0, 0);
					return;
				}
				else
				{
					if (player24 != Main.LocalPlayer || player24.spectating >= 0)
					{
						player24.spectating = num259;
						return;
					}
					return;
				}
				break;
			}
			case 151:
			{
				int num260 = (int)this.reader.ReadInt16();
				WorldItem worldItem4 = Main.item[num260];
				if (Main.netMode == 2 && Main.timeItemSlotCannotBeReusedFor[num260] > 0)
				{
					return;
				}
				if (Main.netMode == 2 && worldItem4.playerIndexTheItemIsReservedFor != this.whoAmI)
				{
					return;
				}
				worldItem4.playerIndexTheItemIsReservedFor = 255;
				worldItem4.TurnToAir(false);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(151, -1, this.whoAmI, null, num260, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				return;
			}
			case 152:
			{
				int num261 = (int)this.reader.ReadByte();
				if (Main.netMode == 2)
				{
					num261 = this.whoAmI;
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(152, -1, this.whoAmI, null, num261, 0f, 0f, 0f, 0, 0, 0);
				}
				if (Main.netMode != 1)
				{
					return;
				}
				Player player25 = Main.player[num261];
				Item item4 = player25.inventory[player25.selectedItem];
				if (item4.UseSound != null)
				{
					SoundEngine.PlaySound(item4.UseSound, player25.Center, item4.useSoundPitch, 1f);
					return;
				}
				return;
			}
			case 153:
			{
				int num262 = (int)this.reader.ReadByte();
				int num263 = (int)this.reader.ReadInt16();
				Main.npc[num262].GetHurtByDebuff(num263);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(153, -1, this.whoAmI, null, num262, (float)num263, 0f, 0f, 0, 0, 0);
					return;
				}
				return;
			}
			case 154:
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(154, this.whoAmI, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
					return;
				}
				Ping.PingRecieved();
				return;
			case 155:
			{
				short num264 = this.reader.ReadInt16();
				short newSize = this.reader.ReadInt16();
				if (num264 < 0 || num264 >= 8000)
				{
					return;
				}
				Main.chest[(int)num264].Resize((int)newSize);
				return;
			}
			case 156:
			{
				if (Main.netMode != 2)
				{
					return;
				}
				Point16 point2 = new Point16(this.reader.ReadInt16(), this.reader.ReadInt16());
				int itemType = (int)this.reader.ReadInt16();
				TELeashedEntityAnchorWithItem teleashedEntityAnchorWithItem;
				if (TileEntity.TryGetAt<TELeashedEntityAnchorWithItem>((int)point2.X, (int)point2.Y, out teleashedEntityAnchorWithItem))
				{
					teleashedEntityAnchorWithItem.InsertItem(itemType);
					return;
				}
				return;
			}
			case 158:
			{
				if (Main.netMode == 2)
				{
					return;
				}
				byte b19 = this.reader.ReadByte();
				Main.player[(int)b19].Spawn(PlayerSpawnContext.TeamSwap);
				return;
			}
			case 159:
			{
				if (Main.netMode != 2)
				{
					return;
				}
				int sectionX = (int)this.reader.ReadUInt16();
				int sectionY = (int)this.reader.ReadUInt16();
				NetMessage.SendSection(this.whoAmI, sectionX, sectionY);
				return;
			}
			case 160:
			{
				if (Main.netMode == 2)
				{
					return;
				}
				int num265 = (int)this.reader.ReadInt16();
				Vector2 position5 = this.reader.ReadVector2();
				Main.item[num265].position = position5;
				return;
			}
			case 161:
			{
				string b20 = this.reader.ReadString();
				Main.player[this.whoAmI].host = (!string.IsNullOrWhiteSpace(Netplay.HostToken) && Netplay.HostToken == b20);
				return;
			}
			default:
				goto IL_A04E;
			}
			if (Main.netMode != 2)
			{
				return;
			}
			if (Netplay.Clients[this.whoAmI].State == 1)
			{
				Netplay.Clients[this.whoAmI].State = 2;
			}
			NetMessage.TrySendData(7, this.whoAmI, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
			Main.SyncAnInvasion(this.whoAmI);
			return;
			IL_550B:
			if (Netplay.Connection.State == 6)
			{
				Netplay.Connection.State = 10;
				Main.player[Main.myPlayer].Spawn(PlayerSpawnContext.SpawningIntoWorld);
				return;
			}
			return;
			IL_7A37:
			int num266 = (int)this.reader.ReadInt16();
			int num267 = this.reader.ReadInt32();
			float num268 = this.reader.ReadSingle();
			float num269 = this.reader.ReadSingle();
			if (num266 < 0 || num266 > Main.maxNPCs)
			{
				return;
			}
			if (Main.netMode == 1)
			{
				Main.npc[num266].moneyPing(new Vector2(num268, num269));
				Main.npc[num266].extraValue = num267;
				return;
			}
			Main.npc[num266].extraValue += num267;
			NetMessage.TrySendData(92, -1, -1, null, num266, (float)Main.npc[num266].extraValue, num268, num269, 0, 0, 0);
			return;
			IL_A04E:
			if (Main.netMode == 2 && Netplay.Clients[this.whoAmI].State == 0)
			{
				NetMessage.BootPlayer(this.whoAmI, Lang.mp[2].ToNetworkText());
			}
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0002C194 File Offset: 0x0002A394
		private static void ReadAccessoryVisibility(BinaryReader reader, bool[] hideVisibleAccessory)
		{
			ushort num = reader.ReadUInt16();
			for (int i = 0; i < hideVisibleAccessory.Length; i++)
			{
				hideVisibleAccessory[i] = (((int)num & 1 << i) != 0);
			}
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0002C1C4 File Offset: 0x0002A3C4
		private static void TrySendingItemArray(int plr, Item[] array, int slotStartIndex)
		{
			for (int i = 0; i < array.Length; i++)
			{
				NetMessage.TrySendData(5, -1, -1, null, plr, (float)(slotStartIndex + i), 0f, 0f, 0, 0, 0);
			}
		}

		// Token: 0x04000187 RID: 391
		public const int readBufferMax = 131070;

		// Token: 0x04000188 RID: 392
		public const int writeBufferMax = 131070;

		// Token: 0x04000189 RID: 393
		public bool broadcast;

		// Token: 0x0400018A RID: 394
		public byte[] readBuffer = new byte[131070];

		// Token: 0x0400018B RID: 395
		public byte[] writeBuffer = new byte[131070];

		// Token: 0x0400018C RID: 396
		public bool writeLocked;

		// Token: 0x0400018D RID: 397
		public int messageLength;

		// Token: 0x0400018E RID: 398
		public int totalData;

		// Token: 0x0400018F RID: 399
		public int whoAmI;

		// Token: 0x04000190 RID: 400
		public int spamCount;

		// Token: 0x04000191 RID: 401
		public int maxSpam;

		// Token: 0x04000192 RID: 402
		public bool checkBytes;

		// Token: 0x04000193 RID: 403
		public MemoryStream readerStream;

		// Token: 0x04000194 RID: 404
		public MemoryStream writerStream;

		// Token: 0x04000195 RID: 405
		public BinaryReader reader;

		// Token: 0x04000196 RID: 406
		public BinaryWriter writer;

		// Token: 0x04000197 RID: 407
		public PacketHistory History = new PacketHistory(100, 65535);

		// Token: 0x04000199 RID: 409
		private float[] _temporaryProjectileAI = new float[Projectile.maxAI];

		// Token: 0x0400019A RID: 410
		private float[] _temporaryNPCAI = new float[NPC.maxAI];
	}
}
