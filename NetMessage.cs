using System;
using System.IO;
using Ionic.Zlib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.GameContent.Items;
using Terraria.GameContent.Tile_Entities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Map;
using Terraria.Net.Sockets;
using Terraria.Social;
using Terraria.Testing;

namespace Terraria
{
	// Token: 0x02000030 RID: 48
	public class NetMessage
	{
		// Token: 0x060002A6 RID: 678 RVA: 0x0003C3CC File Offset: 0x0003A5CC
		public static bool TrySendData(int msgType, int remoteClient = -1, int ignoreClient = -1, NetworkText text = null, int number = 0, float number2 = 0f, float number3 = 0f, float number4 = 0f, int number5 = 0, int number6 = 0, int number7 = 0)
		{
			try
			{
				NetMessage.SendData(msgType, remoteClient, ignoreClient, text, number, number2, number3, number4, number5, number6, number7);
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0003C40C File Offset: 0x0003A60C
		public static void SendData(int msgType, int remoteClient = -1, int ignoreClient = -1, NetworkText text = null, int number = 0, float number2 = 0f, float number3 = 0f, float number4 = 0f, int number5 = 0, int number6 = 0, int number7 = 0)
		{
			if (Main.netMode == 0)
			{
				return;
			}
			if (msgType == 21 && (Main.item[number].shimmerTime > 0f || Main.item[number].shimmered))
			{
				msgType = 145;
			}
			if (msgType == 21 && Main.item[number].type == 0)
			{
				msgType = 151;
			}
			int num = 256;
			if (text == null)
			{
				text = NetworkText.Empty;
			}
			if (Main.netMode == 2 && remoteClient >= 0)
			{
				num = remoteClient;
			}
			MessageBuffer obj = NetMessage.buffer[num];
			lock (obj)
			{
				BinaryWriter writer = NetMessage.buffer[num].writer;
				if (writer == null)
				{
					NetMessage.buffer[num].ResetWriter();
					writer = NetMessage.buffer[num].writer;
				}
				writer.BaseStream.Position = 0L;
				long position = writer.BaseStream.Position;
				writer.BaseStream.Position += 2L;
				writer.Write((byte)msgType);
				switch (msgType)
				{
				case 1:
					writer.Write("Terraria" + 318);
					break;
				case 2:
					text.Serialize(writer);
					if (Main.dedServ)
					{
						Console.WriteLine(Language.GetTextValue("CLI.ClientWasBooted", Netplay.Clients[num].Socket.GetRemoteAddress().ToString(), text));
					}
					break;
				case 3:
					writer.Write((byte)remoteClient);
					writer.Write(false);
					break;
				case 4:
				{
					Player player = Main.player[number];
					writer.Write((byte)number);
					writer.Write((byte)player.skinVariant);
					writer.Write((byte)player.voiceVariant);
					writer.Write(player.voicePitchOffset);
					writer.Write((byte)player.hair);
					writer.Write(player.name);
					writer.Write(player.hairDye);
					NetMessage.WriteAccessoryVisibility(writer, player.hideVisibleAccessory);
					writer.Write(player.hideMisc);
					writer.WriteRGB(player.hairColor);
					writer.WriteRGB(player.skinColor);
					writer.WriteRGB(player.eyeColor);
					writer.WriteRGB(player.shirtColor);
					writer.WriteRGB(player.underShirtColor);
					writer.WriteRGB(player.pantsColor);
					writer.WriteRGB(player.shoeColor);
					BitsByte bb = 0;
					if (player.difficulty == 1)
					{
						bb[0] = true;
					}
					else if (player.difficulty == 2)
					{
						bb[1] = true;
					}
					else if (player.difficulty == 3)
					{
						bb[3] = true;
					}
					bb[2] = player.extraAccessory;
					writer.Write(bb);
					BitsByte bb2 = 0;
					bb2[0] = player.UsingBiomeTorches;
					bb2[1] = player.happyFunTorchTime;
					bb2[2] = player.unlockedBiomeTorches;
					bb2[3] = player.unlockedSuperCart;
					bb2[4] = player.enabledSuperCart;
					writer.Write(bb2);
					BitsByte bb3 = 0;
					bb3[0] = player.usedAegisCrystal;
					bb3[1] = player.usedAegisFruit;
					bb3[2] = player.usedArcaneCrystal;
					bb3[3] = player.usedGalaxyPearl;
					bb3[4] = player.usedGummyWorm;
					bb3[5] = player.usedAmbrosia;
					bb3[6] = player.ateArtisanBread;
					writer.Write(bb3);
					break;
				}
				case 5:
				{
					writer.Write((byte)number);
					writer.Write((short)number2);
					PlayerItemSlotID.SlotReference slotReference = new PlayerItemSlotID.SlotReference(Main.player[number], (int)number2);
					Item item = slotReference.Item;
					if (item.Name == "" || item.stack == 0 || item.type == 0)
					{
						item.SetDefaults(0, null);
					}
					int num2 = item.stack;
					int type = item.type;
					if (num2 < 0)
					{
						num2 = 0;
					}
					writer.Write((short)num2);
					writer.Write(item.prefix);
					writer.Write((short)type);
					BitsByte bb4 = default(BitsByte);
					bb4[0] = item.favorited;
					bb4[1] = (number3 != 0f);
					writer.Write(bb4);
					break;
				}
				case 7:
				{
					writer.Write((int)Main.time);
					BitsByte bb5 = 0;
					bb5[0] = Main.dayTime;
					bb5[1] = Main.bloodMoon;
					bb5[2] = Main.eclipse;
					writer.Write(bb5);
					writer.Write((byte)Main.moonPhase);
					writer.Write((short)Main.maxTilesX);
					writer.Write((short)Main.maxTilesY);
					writer.Write((short)Main.spawnTileX);
					writer.Write((short)Main.spawnTileY);
					writer.Write((short)Main.worldSurface);
					writer.Write((short)Main.rockLayer);
					writer.Write(Main.ActiveWorldFileData.WorldId);
					writer.Write(Main.worldName);
					writer.Write((byte)Main.GameMode);
					writer.Write(Main.ActiveWorldFileData.UniqueId.ToByteArray());
					writer.Write(Main.ActiveWorldFileData.WorldGeneratorVersion);
					writer.Write((byte)Main.moonType);
					writer.Write((byte)WorldGen.treeBG1);
					writer.Write((byte)WorldGen.treeBG2);
					writer.Write((byte)WorldGen.treeBG3);
					writer.Write((byte)WorldGen.treeBG4);
					writer.Write((byte)WorldGen.corruptBG);
					writer.Write((byte)WorldGen.jungleBG);
					writer.Write((byte)WorldGen.snowBG);
					writer.Write((byte)WorldGen.hallowBG);
					writer.Write((byte)WorldGen.crimsonBG);
					writer.Write((byte)WorldGen.desertBG);
					writer.Write((byte)WorldGen.oceanBG);
					writer.Write((byte)WorldGen.mushroomBG);
					writer.Write((byte)WorldGen.underworldBG);
					writer.Write((byte)Main.iceBackStyle);
					writer.Write((byte)Main.jungleBackStyle);
					writer.Write((byte)Main.hellBackStyle);
					writer.Write(Main.windSpeedTarget);
					writer.Write((byte)Main.numClouds);
					for (int i = 0; i < 3; i++)
					{
						writer.Write(Main.treeX[i]);
					}
					for (int j = 0; j < 4; j++)
					{
						writer.Write((byte)Main.treeStyle[j]);
					}
					for (int k = 0; k < 3; k++)
					{
						writer.Write(Main.caveBackX[k]);
					}
					for (int l = 0; l < 4; l++)
					{
						writer.Write((byte)Main.caveBackStyle[l]);
					}
					WorldGen.TreeTops.SyncSend(writer);
					if (!Main.raining)
					{
						Main.maxRaining = 0f;
					}
					writer.Write(Main.maxRaining);
					BitsByte bb6 = 0;
					bb6[0] = WorldGen.shadowOrbSmashed;
					bb6[1] = NPC.downedBoss1;
					bb6[2] = NPC.downedBoss2;
					bb6[3] = NPC.downedBoss3;
					bb6[4] = Main.hardMode;
					bb6[5] = NPC.downedClown;
					bb6[7] = NPC.downedPlantBoss;
					writer.Write(bb6);
					BitsByte bb7 = 0;
					bb7[0] = NPC.downedMechBoss1;
					bb7[1] = NPC.downedMechBoss2;
					bb7[2] = NPC.downedMechBoss3;
					bb7[3] = NPC.downedMechBossAny;
					bb7[4] = (Main.cloudBGActive >= 1f);
					bb7[5] = WorldGen.crimson;
					bb7[6] = Main.pumpkinMoon;
					bb7[7] = Main.snowMoon;
					writer.Write(bb7);
					BitsByte bb8 = 0;
					bb8[1] = Main.fastForwardTimeToDawn;
					bb8[2] = Main.slimeRain;
					bb8[3] = NPC.downedSlimeKing;
					bb8[4] = NPC.downedQueenBee;
					bb8[5] = NPC.downedFishron;
					bb8[6] = NPC.downedMartians;
					bb8[7] = NPC.downedAncientCultist;
					writer.Write(bb8);
					BitsByte bb9 = 0;
					bb9[0] = NPC.downedMoonlord;
					bb9[1] = NPC.downedHalloweenKing;
					bb9[2] = NPC.downedHalloweenTree;
					bb9[3] = NPC.downedChristmasIceQueen;
					bb9[4] = NPC.downedChristmasSantank;
					bb9[5] = NPC.downedChristmasTree;
					bb9[6] = NPC.downedGolemBoss;
					bb9[7] = BirthdayParty.PartyIsUp;
					writer.Write(bb9);
					BitsByte bb10 = 0;
					bb10[0] = NPC.downedPirates;
					bb10[1] = NPC.downedFrost;
					bb10[2] = NPC.downedGoblins;
					bb10[3] = Sandstorm.Happening;
					bb10[4] = DD2Event.Ongoing;
					bb10[5] = DD2Event.DownedInvasionT1;
					bb10[6] = DD2Event.DownedInvasionT2;
					bb10[7] = DD2Event.DownedInvasionT3;
					writer.Write(bb10);
					BitsByte bb11 = 0;
					bb11[0] = NPC.combatBookWasUsed;
					bb11[1] = LanternNight.LanternsUp;
					bb11[2] = NPC.downedTowerSolar;
					bb11[3] = NPC.downedTowerVortex;
					bb11[4] = NPC.downedTowerNebula;
					bb11[5] = NPC.downedTowerStardust;
					bb11[6] = Main.forceHalloweenForToday;
					bb11[7] = Main.forceXMasForToday;
					writer.Write(bb11);
					BitsByte bb12 = 0;
					bb12[0] = NPC.boughtCat;
					bb12[1] = NPC.boughtDog;
					bb12[2] = NPC.boughtBunny;
					bb12[3] = NPC.freeCake;
					bb12[4] = Main.drunkWorld;
					bb12[5] = NPC.downedEmpressOfLight;
					bb12[6] = NPC.downedQueenSlime;
					bb12[7] = Main.getGoodWorld;
					writer.Write(bb12);
					BitsByte bb13 = 0;
					bb13[0] = Main.tenthAnniversaryWorld;
					bb13[1] = Main.dontStarveWorld;
					bb13[2] = NPC.downedDeerclops;
					bb13[3] = Main.notTheBeesWorld;
					bb13[4] = Main.remixWorld;
					bb13[5] = NPC.unlockedSlimeBlueSpawn;
					bb13[6] = NPC.combatBookVolumeTwoWasUsed;
					bb13[7] = NPC.peddlersSatchelWasUsed;
					writer.Write(bb13);
					BitsByte bb14 = 0;
					bb14[0] = NPC.unlockedSlimeGreenSpawn;
					bb14[1] = NPC.unlockedSlimeOldSpawn;
					bb14[2] = NPC.unlockedSlimePurpleSpawn;
					bb14[3] = NPC.unlockedSlimeRainbowSpawn;
					bb14[4] = NPC.unlockedSlimeRedSpawn;
					bb14[5] = NPC.unlockedSlimeYellowSpawn;
					bb14[6] = NPC.unlockedSlimeCopperSpawn;
					bb14[7] = Main.fastForwardTimeToDusk;
					writer.Write(bb14);
					BitsByte bb15 = 0;
					bb15[0] = Main.noTrapsWorld;
					bb15[1] = Main.zenithWorld;
					bb15[2] = NPC.unlockedTruffleSpawn;
					bb15[3] = Main.vampireSeed;
					bb15[4] = Main.infectedSeed;
					bb15[5] = Main.teamBasedSpawnsSeed;
					bb15[6] = Main.skyblockWorld;
					bb15[7] = Main.dualDungeonsSeed;
					writer.Write(bb15);
					BitsByte bb16 = 0;
					bb16[0] = WorldGen.Skyblock.lowTiles;
					writer.Write(bb16);
					writer.Write((byte)Main.sundialCooldown);
					writer.Write((byte)Main.moondialCooldown);
					writer.Write((short)WorldGen.SavedOreTiers.Copper);
					writer.Write((short)WorldGen.SavedOreTiers.Iron);
					writer.Write((short)WorldGen.SavedOreTiers.Silver);
					writer.Write((short)WorldGen.SavedOreTiers.Gold);
					writer.Write((short)WorldGen.SavedOreTiers.Cobalt);
					writer.Write((short)WorldGen.SavedOreTiers.Mythril);
					writer.Write((short)WorldGen.SavedOreTiers.Adamantite);
					writer.Write((sbyte)Main.invasionType);
					if (SocialAPI.Network != null)
					{
						writer.Write(SocialAPI.Network.GetLobbyId());
					}
					else
					{
						writer.Write(0UL);
					}
					writer.Write(Sandstorm.IntendedSeverity);
					ExtraSpawnPointManager.Write(writer, true);
					break;
				}
				case 8:
					writer.Write(number);
					writer.Write((int)number2);
					writer.Write((byte)number3);
					break;
				case 9:
				{
					writer.Write(number);
					text.Serialize(writer);
					BitsByte bb17 = (byte)number2;
					writer.Write(bb17);
					break;
				}
				case 10:
					NetMessage.CompressTileBlock(number, (int)number2, (short)number3, (short)number4, writer.BaseStream);
					break;
				case 11:
					writer.Write((short)number);
					writer.Write((short)number2);
					writer.Write((short)number3);
					writer.Write((short)number4);
					break;
				case 12:
				{
					Player player2 = Main.player[number];
					writer.Write((byte)number);
					writer.Write((short)player2.SpawnX);
					writer.Write((short)player2.SpawnY);
					writer.Write(player2.respawnTimer);
					writer.Write((short)player2.numberOfDeathsPVE);
					writer.Write((short)player2.numberOfDeathsPVP);
					writer.Write((byte)player2.team);
					writer.Write((byte)number2);
					break;
				}
				case 13:
				{
					Player player3 = Main.player[number];
					writer.Write((byte)number);
					BitsByte bb18 = 0;
					bb18[0] = player3.controlUp;
					bb18[1] = player3.controlDown;
					bb18[2] = player3.controlLeft;
					bb18[3] = player3.controlRight;
					bb18[4] = player3.controlJump;
					bb18[5] = player3.controlUseItem;
					bb18[6] = (player3.direction == 1);
					writer.Write(bb18);
					BitsByte bb19 = 0;
					bb19[0] = player3.pulley;
					bb19[1] = (player3.pulley && player3.pulleyDir == 2);
					bb19[2] = (player3.velocity != Vector2.Zero);
					bb19[3] = player3.vortexStealthActive;
					bb19[4] = (player3.gravDir == 1f);
					bb19[5] = player3.shieldRaised;
					bb19[6] = player3.ghost;
					bb19[7] = player3.mount.Active;
					writer.Write(bb19);
					BitsByte bb20 = 0;
					bb20[0] = player3.tryKeepingHoveringUp;
					bb20[1] = player3.IsVoidVaultEnabled;
					bb20[2] = player3.sitting.isSitting;
					bb20[3] = player3.downedDD2EventAnyDifficulty;
					bb20[4] = player3.petting.isPetting;
					bb20[5] = player3.petting.isPetSmall;
					bb20[6] = (player3.PotionOfReturnOriginalUsePosition != null);
					bb20[7] = player3.tryKeepingHoveringDown;
					writer.Write(bb20);
					BitsByte bb21 = 0;
					bb21[0] = player3.sleeping.isSleeping;
					bb21[1] = player3.autoReuseAllWeapons;
					bb21[2] = player3.controlDownHold;
					bb21[3] = player3.isOperatingAnotherEntity;
					bb21[4] = player3.controlUseTile;
					bb21[5] = (player3.netCameraTarget != null);
					bb21[6] = player3.lastItemUseAttemptSuccess;
					writer.Write(bb21);
					writer.Write((byte)player3.selectedItem);
					writer.WriteVector2(player3.position);
					if (bb19[2])
					{
						writer.WriteVector2(player3.velocity);
					}
					if (bb19[7])
					{
						writer.Write((ushort)player3.mount.Type);
					}
					if (bb20[6])
					{
						writer.WriteVector2(player3.PotionOfReturnOriginalUsePosition.Value);
						writer.WriteVector2(player3.PotionOfReturnHomePosition.Value);
					}
					if (bb21[5])
					{
						writer.WriteVector2(player3.netCameraTarget.Value);
					}
					if (player3 == Main.LocalPlayer)
					{
						player3.lastSyncedNetCameraTarget = player3.netCameraTarget;
					}
					break;
				}
				case 14:
					writer.Write((byte)number);
					writer.Write((byte)number2);
					break;
				case 16:
					writer.Write((byte)number);
					writer.Write((short)Main.player[number].statLife);
					writer.Write((short)Main.player[number].statLifeMax);
					break;
				case 17:
					writer.Write((byte)number);
					writer.Write((short)number2);
					writer.Write((short)number3);
					writer.Write((short)number4);
					writer.Write((byte)number5);
					break;
				case 18:
					writer.Write(Main.dayTime ? 1 : 0);
					writer.Write((int)Main.time);
					writer.Write(Main.sunModY);
					writer.Write(Main.moonModY);
					break;
				case 19:
					writer.Write((byte)number);
					writer.Write((short)number2);
					writer.Write((short)number3);
					writer.Write((number4 == 1f) ? 1 : 0);
					break;
				case 20:
				{
					int num3 = number;
					int num4 = (int)number2;
					int num5 = (int)number3;
					if (num5 < 0)
					{
						num5 = 0;
					}
					int num6 = (int)number4;
					if (num6 < 0)
					{
						num6 = 0;
					}
					if (num3 < num5)
					{
						num3 = num5;
					}
					if (num3 >= Main.maxTilesX + num5)
					{
						num3 = Main.maxTilesX - num5 - 1;
					}
					if (num4 < num6)
					{
						num4 = num6;
					}
					if (num4 >= Main.maxTilesY + num6)
					{
						num4 = Main.maxTilesY - num6 - 1;
					}
					writer.Write((short)num3);
					writer.Write((short)num4);
					writer.Write((byte)num5);
					writer.Write((byte)num6);
					writer.Write((byte)number5);
					for (int m = num3; m < num3 + num5; m++)
					{
						for (int n = num4; n < num4 + num6; n++)
						{
							BitsByte bb22 = 0;
							BitsByte bb23 = 0;
							BitsByte bb24 = 0;
							byte b = 0;
							byte b2 = 0;
							Tile tile = Main.tile[m, n];
							bb22[0] = tile.active();
							bb22[2] = (tile.wall > 0);
							bb22[3] = (tile.liquid > 0 && Main.netMode == 2);
							bb22[4] = tile.wire();
							bb22[5] = tile.halfBrick();
							bb22[6] = tile.actuator();
							bb22[7] = tile.inActive();
							bb23[0] = tile.wire2();
							bb23[1] = tile.wire3();
							if (tile.active() && tile.color() > 0)
							{
								bb23[2] = true;
								b = tile.color();
							}
							if (tile.wall > 0 && tile.wallColor() > 0)
							{
								bb23[3] = true;
								b2 = tile.wallColor();
							}
							bb23 += (byte)(tile.slope() << 4);
							bb23[7] = tile.wire4();
							bb24[0] = tile.fullbrightBlock();
							bb24[1] = tile.fullbrightWall();
							bb24[2] = tile.invisibleBlock();
							bb24[3] = tile.invisibleWall();
							writer.Write(bb22);
							writer.Write(bb23);
							writer.Write(bb24);
							if (b > 0)
							{
								writer.Write(b);
							}
							if (b2 > 0)
							{
								writer.Write(b2);
							}
							if (tile.active())
							{
								writer.Write(tile.type);
								if (Main.tileFrameImportant[(int)tile.type])
								{
									writer.Write(tile.frameX);
									writer.Write(tile.frameY);
								}
							}
							if (tile.wall > 0)
							{
								writer.Write(tile.wall);
							}
							if (tile.liquid > 0 && Main.netMode == 2)
							{
								writer.Write(tile.liquid);
								writer.Write(tile.liquidType());
							}
						}
					}
					break;
				}
				case 21:
				case 90:
				case 145:
				case 148:
				{
					WorldItem worldItem = Main.item[number];
					Item inner = worldItem.inner;
					writer.Write((short)number);
					writer.WriteVector2(worldItem.position);
					writer.WriteVector2(worldItem.velocity);
					writer.Write((short)inner.stack);
					writer.Write(inner.prefix);
					writer.Write((byte)number2);
					short value = 0;
					if (worldItem.active && worldItem.stack > 0)
					{
						value = (short)worldItem.type;
					}
					writer.Write(value);
					if (msgType == 145)
					{
						writer.Write(worldItem.shimmered);
						writer.Write(worldItem.shimmerTime);
					}
					if (msgType == 148)
					{
						writer.Write((byte)MathHelper.Clamp((float)worldItem.timeLeftInWhichTheItemCannotBeTakenByEnemies, 0f, 255f));
					}
					break;
				}
				case 22:
				{
					WorldItem worldItem2 = Main.item[number];
					writer.Write((short)number);
					writer.Write((byte)worldItem2.playerIndexTheItemIsReservedFor);
					writer.WriteVector2(worldItem2.position);
					break;
				}
				case 23:
				{
					NPC npc = Main.npc[number];
					writer.Write((short)number);
					writer.WriteVector2(npc.position);
					writer.WriteVector2(npc.velocity);
					writer.Write((ushort)npc.target);
					int num7 = npc.life;
					if (!npc.active)
					{
						num7 = 0;
					}
					short value2 = (short)npc.netID;
					bool[] array = new bool[4];
					BitsByte bb25 = 0;
					bb25[0] = (npc.direction > 0);
					bb25[1] = (npc.directionY > 0);
					bb25[2] = (array[0] = (npc.ai[0] != 0f));
					bb25[3] = (array[1] = (npc.ai[1] != 0f));
					bb25[4] = (array[2] = (npc.ai[2] != 0f));
					bb25[5] = (array[3] = (npc.ai[3] != 0f));
					bb25[6] = (npc.spriteDirection > 0);
					bb25[7] = (num7 == npc.lifeMax);
					writer.Write(bb25);
					BitsByte bb26 = 0;
					bb26[0] = (npc.statsAreScaledForThisManyPlayers > 1);
					bb26[1] = npc.SpawnedFromStatue;
					bb26[2] = (npc.difficulty != 1f);
					bb26[3] = npc.spawnNeedsSyncing;
					bb26[4] = (npc.spawnNeedsSyncing && npc.shimmerTransparency > 0f);
					writer.Write(bb26);
					for (int num8 = 0; num8 < NPC.maxAI; num8++)
					{
						if (array[num8])
						{
							writer.Write(npc.ai[num8]);
						}
					}
					writer.Write(value2);
					if (bb26[0])
					{
						writer.Write((byte)npc.statsAreScaledForThisManyPlayers);
					}
					if (bb26[2])
					{
						writer.Write(npc.difficulty);
					}
					if (!bb25[7])
					{
						byte b3 = 1;
						if (npc.lifeMax > 32767)
						{
							b3 = 4;
						}
						else if (npc.lifeMax > 127)
						{
							b3 = 2;
						}
						writer.Write(b3);
						if (b3 == 2)
						{
							writer.Write((short)num7);
						}
						else if (b3 == 4)
						{
							writer.Write(num7);
						}
						else
						{
							writer.Write((sbyte)num7);
						}
					}
					if (npc.type >= 0 && npc.type < (int)NPCID.Count && Main.npcCatchable[npc.type])
					{
						writer.Write((byte)npc.releaseOwner);
					}
					break;
				}
				case 24:
					writer.Write((short)number);
					writer.Write((byte)number2);
					break;
				case 27:
				{
					Projectile projectile = Main.projectile[number];
					writer.Write((short)projectile.identity);
					writer.WriteVector2(projectile.position);
					writer.WriteVector2(projectile.velocity);
					writer.Write((byte)projectile.owner);
					writer.Write((short)projectile.type);
					BitsByte bb27 = 0;
					BitsByte bb28 = 0;
					bb27[0] = (projectile.ai[0] != 0f);
					bb27[1] = (projectile.ai[1] != 0f);
					bb28[0] = (projectile.ai[2] != 0f);
					if (projectile.bannerIdToRespondTo != 0)
					{
						bb27[3] = true;
					}
					if (projectile.damage != 0)
					{
						bb27[4] = true;
					}
					if (projectile.knockBack != 0f)
					{
						bb27[5] = true;
					}
					if (projectile.type > 0 && projectile.type < (int)ProjectileID.Count && ProjectileID.Sets.NeedsUUID[projectile.type])
					{
						bb27[7] = true;
					}
					if (projectile.originalDamage != 0)
					{
						bb27[6] = true;
					}
					if (bb28 != 0)
					{
						bb27[2] = true;
					}
					writer.Write(bb27);
					if (bb27[2])
					{
						writer.Write(bb28);
					}
					if (bb27[0])
					{
						writer.Write(projectile.ai[0]);
					}
					if (bb27[1])
					{
						writer.Write(projectile.ai[1]);
					}
					if (bb27[3])
					{
						writer.Write((ushort)projectile.bannerIdToRespondTo);
					}
					if (bb27[4])
					{
						writer.Write((short)projectile.damage);
					}
					if (bb27[5])
					{
						writer.Write(projectile.knockBack);
					}
					if (bb27[6])
					{
						writer.Write((short)projectile.originalDamage);
					}
					if (bb27[7])
					{
						writer.Write((short)projectile.projUUID);
					}
					if (bb28[0])
					{
						writer.Write(projectile.ai[2]);
					}
					break;
				}
				case 28:
					writer.Write((short)number);
					writer.Write((short)number2);
					writer.Write(number3);
					writer.Write((byte)(number4 + 1f));
					writer.Write((byte)number5);
					break;
				case 29:
					writer.Write((short)number);
					writer.Write((byte)number2);
					break;
				case 30:
					writer.Write((byte)number);
					writer.Write(Main.player[number].hostile);
					break;
				case 31:
					writer.Write((short)number);
					writer.Write((short)number2);
					break;
				case 32:
				{
					Item item2 = Main.chest[number].item[(int)((byte)number2)];
					writer.Write((short)number);
					writer.Write((byte)number2);
					short value3 = (short)item2.type;
					if (item2.Name == null)
					{
						value3 = 0;
					}
					writer.Write((short)item2.stack);
					writer.Write(item2.prefix);
					writer.Write(value3);
					break;
				}
				case 33:
				{
					int num9 = 0;
					int num10 = 0;
					int num11 = 0;
					string text2 = null;
					if (number > -1)
					{
						num9 = Main.chest[number].x;
						num10 = Main.chest[number].y;
					}
					if (number2 == 1f)
					{
						string text3 = text.ToString();
						num11 = (int)((byte)text3.Length);
						if (num11 == 0 || num11 > 20)
						{
							num11 = 255;
						}
						else
						{
							text2 = text3;
						}
					}
					writer.Write((short)number);
					writer.Write((short)num9);
					writer.Write((short)num10);
					writer.Write((byte)num11);
					if (text2 != null)
					{
						writer.Write(text2);
					}
					break;
				}
				case 34:
					writer.Write((byte)number);
					writer.Write((short)number2);
					writer.Write((short)number3);
					writer.Write((short)number4);
					if (Main.netMode == 2)
					{
						Netplay.GetSectionX((int)number2);
						Netplay.GetSectionY((int)number3);
						writer.Write((short)number5);
					}
					else
					{
						writer.Write(0);
					}
					break;
				case 35:
					writer.Write((byte)number);
					writer.Write((short)number2);
					break;
				case 36:
				{
					Player player4 = Main.player[number];
					writer.Write((byte)number);
					writer.Write(player4.zone1);
					writer.Write(player4.zone2);
					writer.Write(player4.zone3);
					writer.Write(player4.zone4);
					writer.Write(player4.zone5);
					writer.Write((byte)player4.townNPCs);
					break;
				}
				case 38:
					writer.Write(Netplay.ServerPassword);
					break;
				case 39:
					writer.Write((short)number);
					break;
				case 40:
					writer.Write((byte)number);
					writer.Write((short)Main.player[number].talkNPC);
					break;
				case 41:
					writer.Write((byte)number);
					writer.Write(Main.player[number].itemRotation);
					writer.Write((short)Main.player[number].itemAnimation);
					break;
				case 42:
					writer.Write((byte)number);
					writer.Write((short)Main.player[number].statMana);
					writer.Write((short)Main.player[number].statManaMax);
					break;
				case 43:
					writer.Write((byte)number);
					writer.Write((short)number2);
					break;
				case 45:
				case 157:
					writer.Write((byte)number);
					writer.Write((byte)Main.player[number].team);
					break;
				case 46:
					writer.Write((short)number);
					writer.Write((short)number2);
					break;
				case 47:
					writer.Write((short)number);
					writer.Write((short)Main.sign[number].x);
					writer.Write((short)Main.sign[number].y);
					writer.Write(Main.sign[number].text);
					writer.Write((byte)number2);
					writer.Write((byte)number3);
					break;
				case 48:
				{
					Tile tile2 = Main.tile[number, (int)number2];
					writer.Write((short)number);
					writer.Write((short)number2);
					writer.Write(tile2.liquid);
					writer.Write(tile2.liquidType());
					break;
				}
				case 50:
				{
					writer.Write((byte)number);
					Player player5 = Main.player[number];
					for (int num12 = 0; num12 < Player.maxBuffs; num12++)
					{
						if (player5.buffType[num12] > 0)
						{
							writer.Write((ushort)player5.buffType[num12]);
						}
					}
					writer.Write(0);
					break;
				}
				case 51:
					writer.Write((byte)number);
					writer.Write((byte)number2);
					break;
				case 52:
					writer.Write((byte)number2);
					writer.Write((short)number3);
					writer.Write((short)number4);
					break;
				case 53:
					writer.Write((short)number);
					writer.Write((ushort)number2);
					writer.Write((short)number3);
					break;
				case 54:
				{
					NPC npc2 = Main.npc[number];
					writer.Write((short)number);
					for (int num13 = 0; num13 < NPC.maxBuffs; num13++)
					{
						int num14 = npc2.buffType[num13];
						int num15 = npc2.buffTime[num13];
						if (num14 > 0 && num15 > 0)
						{
							writer.Write((ushort)num14);
							writer.Write((ushort)num15);
						}
					}
					writer.Write(0);
					break;
				}
				case 55:
					writer.Write((byte)number);
					writer.Write((ushort)number2);
					writer.Write((int)number3);
					break;
				case 56:
					writer.Write((short)number);
					if (Main.netMode == 2)
					{
						string givenName = Main.npc[number].GivenName;
						writer.Write(givenName);
						writer.Write(Main.npc[number].townNpcVariationIndex);
					}
					break;
				case 57:
					writer.Write(WorldGen.tGood);
					writer.Write(WorldGen.tEvil);
					writer.Write(WorldGen.tBlood);
					break;
				case 58:
					writer.Write((byte)number);
					writer.Write(number2);
					break;
				case 59:
					writer.Write((short)number);
					writer.Write((short)number2);
					break;
				case 60:
					writer.Write((short)number);
					writer.Write((short)number2);
					writer.Write((short)number3);
					writer.Write((byte)number4);
					break;
				case 61:
					writer.Write((short)number);
					writer.Write((short)number2);
					break;
				case 62:
					writer.Write((byte)number);
					writer.Write((byte)number2);
					break;
				case 63:
				case 64:
					writer.Write((short)number);
					writer.Write((short)number2);
					writer.Write((byte)number3);
					writer.Write((byte)number4);
					break;
				case 65:
				{
					BitsByte bb29 = 0;
					bb29[0] = ((number & 1) == 1);
					bb29[1] = ((number & 2) == 2);
					bb29[2] = (number6 == 1);
					bb29[3] = (number7 != 0);
					writer.Write(bb29);
					writer.Write((short)number2);
					writer.Write(number3);
					writer.Write(number4);
					writer.Write((byte)number5);
					if (bb29[3])
					{
						writer.Write(number7);
					}
					if (Main.netMode == 2 && number == 0 && number2 != (float)ignoreClient)
					{
						Main.player[(int)number2].unacknowledgedTeleports++;
					}
					break;
				}
				case 66:
					writer.Write((byte)number);
					writer.Write((short)number2);
					break;
				case 68:
					writer.Write(Main.clientUUID);
					break;
				case 69:
					Netplay.GetSectionX((int)number2);
					Netplay.GetSectionY((int)number3);
					writer.Write((short)number);
					writer.Write((short)number2);
					writer.Write((short)number3);
					writer.Write(Main.chest[(int)((short)number)].name);
					break;
				case 70:
					writer.Write((short)number);
					writer.Write((byte)number2);
					break;
				case 71:
					writer.Write(number);
					writer.Write((int)number2);
					writer.Write((short)number3);
					writer.Write((byte)number4);
					break;
				case 72:
					for (int num16 = 0; num16 < Main.TravelShopMaxSlots; num16++)
					{
						writer.Write((short)Main.travelShop[num16]);
					}
					break;
				case 73:
					writer.Write((byte)number);
					break;
				case 74:
				{
					writer.Write((byte)Main.anglerQuest);
					bool value4 = Main.anglerWhoFinishedToday.Contains(text.ToString());
					writer.Write(value4);
					break;
				}
				case 76:
					writer.Write((byte)number);
					writer.Write(Main.player[number].anglerQuestsFinished);
					writer.Write(Main.player[number].golferScoreAccumulated);
					break;
				case 77:
					writer.Write((short)number);
					writer.Write((ushort)number2);
					writer.Write((short)number3);
					writer.Write((short)number4);
					break;
				case 78:
					writer.Write(number);
					writer.Write((int)number2);
					writer.Write((sbyte)number3);
					writer.Write((sbyte)number4);
					break;
				case 79:
					writer.Write((short)number);
					writer.Write((short)number2);
					writer.Write((short)number3);
					writer.Write((short)number4);
					writer.Write((byte)number5);
					writer.Write((sbyte)number6);
					writer.Write(number7 == 1);
					break;
				case 80:
					writer.Write((byte)number);
					writer.Write((short)number2);
					break;
				case 81:
					writer.Write(number2);
					writer.Write(number3);
					writer.WriteRGB(new Color
					{
						PackedValue = (uint)number
					});
					writer.Write((int)number4);
					break;
				case 84:
				{
					byte b4 = (byte)number;
					float stealth = Main.player[(int)b4].stealth;
					writer.Write(b4);
					writer.Write(stealth);
					break;
				}
				case 85:
					if (Main.netMode == 1)
					{
						QuickStacking.WriteNetInventorySlots(writer);
						writer.Write((byte)number);
					}
					else
					{
						QuickStacking.WriteBlockedChestList(writer);
					}
					break;
				case 86:
				{
					writer.Write(number);
					TileEntity ent;
					bool flag2 = TileEntity.TryGet<TileEntity>(number, out ent);
					writer.Write(flag2);
					if (flag2)
					{
						TileEntity.Write(writer, ent, true);
					}
					break;
				}
				case 87:
					writer.Write((short)number);
					writer.Write((short)number2);
					writer.Write((byte)number3);
					break;
				case 88:
				{
					BitsByte bb30 = (byte)number2;
					BitsByte bb31 = (byte)number3;
					writer.Write((short)number);
					writer.Write(bb30);
					WorldItem worldItem3 = Main.item[number];
					if (bb30[0])
					{
						writer.Write(worldItem3.color.PackedValue);
					}
					if (bb30[1])
					{
						writer.Write((ushort)worldItem3.damage);
					}
					if (bb30[2])
					{
						writer.Write(worldItem3.knockBack);
					}
					if (bb30[3])
					{
						writer.Write((ushort)worldItem3.useAnimation);
					}
					if (bb30[4])
					{
						writer.Write((ushort)worldItem3.useTime);
					}
					if (bb30[5])
					{
						writer.Write((short)worldItem3.shoot);
					}
					if (bb30[6])
					{
						writer.Write(worldItem3.shootSpeed);
					}
					if (bb30[7])
					{
						writer.Write(bb31);
						if (bb31[0])
						{
							writer.Write((ushort)worldItem3.width);
						}
						if (bb31[1])
						{
							writer.Write((ushort)worldItem3.height);
						}
						if (bb31[2])
						{
							writer.Write(worldItem3.scale);
						}
						if (bb31[3])
						{
							writer.Write((short)worldItem3.ammo);
						}
						if (bb31[4])
						{
							writer.Write((short)worldItem3.useAmmo);
						}
						if (bb31[5])
						{
							writer.Write(worldItem3.notAmmo);
						}
					}
					break;
				}
				case 89:
				{
					writer.Write((short)number);
					writer.Write((short)number2);
					Item item3 = Main.player[(int)number4].inventory[(int)number3];
					writer.Write((short)item3.type);
					writer.Write(item3.prefix);
					writer.Write((short)number5);
					break;
				}
				case 91:
					writer.Write(number);
					writer.Write((byte)number2);
					if (number2 != 255f)
					{
						writer.Write((ushort)number3);
						writer.Write((ushort)number4);
						writer.Write((byte)number5);
						if (number5 < 0)
						{
							writer.Write((short)number6);
						}
					}
					break;
				case 92:
					writer.Write((short)number);
					writer.Write((int)number2);
					writer.Write(number3);
					writer.Write(number4);
					break;
				case 95:
					writer.Write((ushort)number);
					writer.Write((byte)number2);
					break;
				case 96:
				{
					writer.Write((byte)number);
					Player player6 = Main.player[number];
					writer.Write((short)number4);
					writer.Write(number2);
					writer.Write(number3);
					writer.WriteVector2(player6.velocity);
					break;
				}
				case 97:
					writer.Write((short)number);
					break;
				case 98:
					writer.Write((short)number);
					break;
				case 99:
					writer.Write((byte)number);
					writer.WriteVector2(Main.player[number].MinionRestTargetPoint);
					break;
				case 100:
				{
					writer.Write((ushort)number);
					NPC npc3 = Main.npc[number];
					writer.Write((short)number4);
					writer.Write(number2);
					writer.Write(number3);
					writer.WriteVector2(npc3.velocity);
					break;
				}
				case 101:
					writer.Write((ushort)NPC.ShieldStrengthTowerSolar);
					writer.Write((ushort)NPC.ShieldStrengthTowerVortex);
					writer.Write((ushort)NPC.ShieldStrengthTowerNebula);
					writer.Write((ushort)NPC.ShieldStrengthTowerStardust);
					break;
				case 102:
					writer.Write((byte)number);
					writer.Write((ushort)number2);
					writer.Write(number3);
					writer.Write(number4);
					break;
				case 103:
					writer.Write(NPC.MaxMoonLordCountdown);
					writer.Write(NPC.MoonLordCountdown);
					break;
				case 104:
					writer.Write((byte)number);
					writer.Write((short)number2);
					writer.Write(((short)number3 < 0) ? 0f : number3);
					writer.Write((byte)number4);
					writer.Write(number5);
					writer.Write((byte)number6);
					break;
				case 105:
					writer.Write((short)number);
					writer.Write((short)number2);
					writer.Write(number3 == 1f);
					break;
				case 106:
				{
					HalfVector2 halfVector = new HalfVector2((float)number, number2);
					writer.Write(halfVector.PackedValue);
					break;
				}
				case 107:
					writer.Write((byte)number2);
					writer.Write((byte)number3);
					writer.Write((byte)number4);
					text.Serialize(writer);
					writer.Write((short)number5);
					break;
				case 108:
					writer.Write((short)number);
					writer.Write(number2);
					writer.Write((short)number3);
					writer.Write((short)number4);
					writer.Write((short)number5);
					writer.Write((short)number6);
					writer.Write((byte)number7);
					break;
				case 109:
					writer.Write((short)number);
					writer.Write((short)number2);
					writer.Write((short)number3);
					writer.Write((short)number4);
					writer.Write((byte)number5);
					break;
				case 110:
					writer.Write((short)number);
					writer.Write((short)number2);
					writer.Write((byte)number3);
					break;
				case 112:
					writer.Write((byte)number);
					writer.Write((int)number2);
					writer.Write((int)number3);
					writer.Write((byte)number4);
					writer.Write((short)number5);
					writer.Write((byte)number6);
					break;
				case 113:
					writer.Write((short)number);
					writer.Write((short)number2);
					break;
				case 115:
					writer.Write((byte)number);
					writer.Write((short)Main.player[number].MinionAttackTargetNPC);
					break;
				case 116:
					writer.Write(number);
					break;
				case 117:
					writer.Write((byte)number);
					NetMessage._currentPlayerDeathReason.WriteSelfTo(writer);
					writer.Write((short)number2);
					writer.Write((byte)(number3 + 1f));
					writer.Write((byte)number4);
					writer.Write((sbyte)number5);
					break;
				case 118:
					writer.Write((byte)number);
					NetMessage._currentPlayerDeathReason.WriteSelfTo(writer);
					writer.Write((short)number2);
					writer.Write((byte)(number3 + 1f));
					writer.Write((byte)number4);
					break;
				case 119:
					writer.Write(number2);
					writer.Write(number3);
					writer.WriteRGB(new Color
					{
						PackedValue = (uint)number
					});
					text.Serialize(writer);
					break;
				case 120:
					writer.Write((byte)number);
					writer.Write((byte)number2);
					break;
				case 121:
				{
					int num17 = (int)number3;
					writer.Write((byte)number);
					writer.Write((int)number2);
					writer.Write((byte)num17);
					writer.Write((byte)number4);
					TEDisplayDoll tedisplayDoll;
					if (TileEntity.TryGet<TEDisplayDoll>((int)number2, out tedisplayDoll))
					{
						tedisplayDoll.WriteData((int)number3, (int)number4, writer);
					}
					else
					{
						TEDisplayDoll.WriteDummySync((int)number3, (int)number4, writer);
					}
					break;
				}
				case 122:
					writer.Write(number);
					writer.Write((byte)number2);
					break;
				case 123:
				{
					writer.Write((short)number);
					writer.Write((short)number2);
					Item item4 = Main.player[(int)number4].inventory[(int)number3];
					writer.Write((short)item4.type);
					writer.Write(item4.prefix);
					writer.Write((short)number5);
					break;
				}
				case 124:
				{
					int num18 = (int)number3;
					bool flag3 = number4 == 1f;
					if (flag3)
					{
						num18 += 2;
					}
					writer.Write((byte)number);
					writer.Write((int)number2);
					writer.Write((byte)num18);
					TEHatRack tehatRack;
					if (TileEntity.TryGet<TEHatRack>((int)number2, out tehatRack))
					{
						tehatRack.WriteItem((int)number3, writer, flag3);
					}
					else
					{
						writer.Write(0);
						writer.Write(0);
					}
					break;
				}
				case 125:
					writer.Write((byte)number);
					writer.Write((short)number2);
					writer.Write((short)number3);
					writer.Write((byte)number4);
					break;
				case 126:
					NetMessage._currentRevengeMarker.WriteSelfTo(writer);
					break;
				case 127:
					writer.Write(number);
					break;
				case 128:
					writer.Write((byte)number);
					writer.Write((ushort)number5);
					writer.Write((ushort)number6);
					writer.Write((ushort)number2);
					writer.Write((ushort)number3);
					break;
				case 130:
					writer.Write((ushort)number);
					writer.Write((ushort)number2);
					writer.Write((short)number3);
					break;
				case 131:
				{
					writer.Write((ushort)number);
					writer.Write((byte)number2);
					byte b5 = (byte)number2;
					if (b5 == 1)
					{
						writer.Write((int)number3);
						writer.Write((short)number4);
					}
					break;
				}
				case 132:
					NetMessage._currentNetSoundInfo.WriteSelfTo(writer);
					break;
				case 133:
				{
					writer.Write((short)number);
					writer.Write((short)number2);
					Item item5 = Main.player[(int)number4].inventory[(int)number3];
					writer.Write((short)item5.type);
					writer.Write(item5.prefix);
					writer.Write((short)number5);
					break;
				}
				case 134:
				{
					writer.Write((byte)number);
					Player player7 = Main.player[number];
					writer.Write(player7.ladyBugLuckTimeLeft);
					writer.Write(player7.torchLuck);
					writer.Write(player7.luckPotion);
					writer.Write(player7.HasGardenGnomeNearby);
					writer.Write(player7.brokenMirrorBadLuck);
					writer.Write(player7.equipmentBasedLuckBonus);
					writer.Write(player7.coinLuck);
					writer.Write(player7.kiteLuckLevel);
					break;
				}
				case 135:
					writer.Write((byte)number);
					break;
				case 136:
					for (int num19 = 0; num19 < 2; num19++)
					{
						for (int num20 = 0; num20 < 3; num20++)
						{
							writer.Write((ushort)NPC.cavernMonsterType[num19, num20]);
						}
					}
					break;
				case 137:
					writer.Write((short)number);
					writer.Write((ushort)number2);
					break;
				case 139:
				{
					writer.Write((byte)number);
					bool value5 = number2 == 1f;
					writer.Write(value5);
					break;
				}
				case 140:
					writer.Write((byte)number);
					writer.Write((int)number2);
					break;
				case 141:
					writer.Write((byte)number);
					writer.Write((byte)number2);
					writer.Write(number3);
					writer.Write(number4);
					writer.Write(number5);
					writer.Write(number6);
					break;
				case 142:
				{
					writer.Write((byte)number);
					Player player8 = Main.player[number];
					player8.piggyBankProjTracker.Write(writer);
					player8.voidLensChest.Write(writer);
					break;
				}
				case 146:
					writer.Write((byte)number);
					if (number == 0)
					{
						writer.WriteVector2(new Vector2((float)((int)number2), (float)((int)number3)));
					}
					else if (number == 1)
					{
						writer.WriteVector2(new Vector2((float)((int)number2), (float)((int)number3)));
						writer.Write((int)number4);
					}
					break;
				case 147:
					writer.Write((byte)number);
					writer.Write((byte)number2);
					NetMessage.WriteAccessoryVisibility(writer, Main.player[number].hideVisibleAccessory);
					break;
				case 149:
				{
					writer.Write((short)number);
					writer.Write((short)number2);
					Item item6 = Main.player[(int)number4].inventory[(int)number3];
					writer.Write((short)item6.type);
					writer.Write(item6.prefix);
					writer.Write((short)number5);
					break;
				}
				case 150:
					writer.Write((byte)number);
					writer.Write((short)number2);
					break;
				case 151:
					Main.item[number].playerIndexTheItemIsReservedFor = 255;
					writer.Write((short)number);
					break;
				case 152:
					writer.Write((byte)number);
					break;
				case 153:
					writer.Write((byte)number);
					writer.Write((short)number2);
					break;
				case 155:
					writer.Write((short)number);
					writer.Write((short)number2);
					break;
				case 156:
					writer.Write((short)number);
					writer.Write((short)number2);
					writer.Write((short)number3);
					break;
				case 158:
					writer.Write((byte)number);
					break;
				case 159:
					writer.Write((short)number);
					writer.Write((short)number2);
					break;
				case 160:
				{
					WorldItem worldItem4 = Main.item[number];
					writer.Write((short)number);
					writer.WriteVector2(worldItem4.position);
					break;
				}
				case 161:
					writer.Write(text.ToString());
					break;
				}
				int num21 = (int)writer.BaseStream.Position;
				if (num21 > 65535)
				{
					throw new Exception(string.Concat(new object[]
					{
						"Maximum packet length exceeded. id: ",
						msgType,
						" length: ",
						num21
					}));
				}
				writer.BaseStream.Position = position;
				writer.Write((ushort)num21);
				writer.BaseStream.Position = (long)num21;
				if (Main.netMode == 1)
				{
					if (Netplay.Connection.IsConnected())
					{
						NetMessage.SendPacketToServer(NetMessage.buffer[num].writeBuffer);
					}
				}
				else if (remoteClient == -1)
				{
					if (msgType == 34 || msgType == 69)
					{
						for (int num22 = 0; num22 < 256; num22++)
						{
							if (num22 != ignoreClient && NetMessage.buffer[num22].broadcast && Netplay.Clients[num22].IsConnected())
							{
								NetMessage.SendPacket(NetMessage.buffer[num].writeBuffer, num22);
							}
						}
					}
					else if (msgType == 20)
					{
						for (int num23 = 0; num23 < 256; num23++)
						{
							if (num23 != ignoreClient && NetMessage.buffer[num23].broadcast && Netplay.Clients[num23].IsConnected() && Netplay.Clients[num23].SectionRange((int)Math.Max(number3, number4), number, (int)number2))
							{
								NetMessage.SendPacket(NetMessage.buffer[num].writeBuffer, num23);
							}
						}
					}
					else if (msgType == 23)
					{
						NPC npc4 = Main.npc[number];
						bool flag4 = npc4.boss || npc4.netAlways || npc4.townNPC || !npc4.active || npc4.life <= 0 || npc4.spawnNeedsSyncing;
						if (flag4)
						{
							npc4.spawnNeedsSyncing = false;
							npc4.netStream = 0;
							npc4.netUpdate = false;
							npc4.netUpdatePendingSpamCooldown = false;
							npc4.netUpdatePendingFullSpamCooldown = false;
							Array.Clear(npc4.playerNetSyncState, 0, npc4.playerNetSyncState.Length);
						}
						for (int num24 = 0; num24 < 256; num24++)
						{
							if (num24 != ignoreClient && NetMessage.buffer[num24].broadcast && Netplay.Clients[num24].IsConnected())
							{
								if (!flag4)
								{
									if (npc4.playerNetSyncState[num24].skippedSyncs < 4 && !Netplay.Clients[num24].IsSectionActive(npc4.NetSectionCoordinates))
									{
										NPC.PlayerNetSyncState[] playerNetSyncState = npc4.playerNetSyncState;
										int num25 = num24;
										playerNetSyncState[num25].skippedSyncs = playerNetSyncState[num25].skippedSyncs + 1;
										goto IL_3526;
									}
									npc4.playerNetSyncState[num24] = default(NPC.PlayerNetSyncState);
								}
								NetMessage.SendPacket(NetMessage.buffer[num].writeBuffer, num24);
							}
							IL_3526:;
						}
					}
					else if (msgType == 28)
					{
						NPC npc5 = Main.npc[number];
						for (int num26 = 0; num26 < 256; num26++)
						{
							if (num26 != ignoreClient && NetMessage.buffer[num26].broadcast && Netplay.Clients[num26].IsConnected() && (npc5.life <= 0 || Netplay.Clients[num26].IsSectionActive(npc5.NetSectionCoordinates)))
							{
								NetMessage.SendPacket(NetMessage.buffer[num].writeBuffer, num26);
							}
						}
					}
					else if (msgType == 13)
					{
						for (int num27 = 0; num27 < 256; num27++)
						{
							if (num27 != ignoreClient && NetMessage.buffer[num27].broadcast && Netplay.Clients[num27].IsConnected())
							{
								NetMessage.SendPacket(NetMessage.buffer[num].writeBuffer, num27);
							}
						}
					}
					else if (msgType == 27)
					{
						Projectile projectile2 = Main.projectile[number];
						bool flag5 = projectile2.type == 12 || Main.projPet[projectile2.type] || projectile2.aiStyle == 11 || projectile2.netImportant;
						if (flag5)
						{
							Array.Clear(projectile2.netSyncSkippedForPlayer, 0, projectile2.netSyncSkippedForPlayer.Length);
						}
						for (int num28 = 0; num28 < 256; num28++)
						{
							if (num28 != ignoreClient && NetMessage.buffer[num28].broadcast && Netplay.Clients[num28].IsConnected())
							{
								if (!flag5)
								{
									if (!Netplay.Clients[num28].IsSectionActive(projectile2.NetSectionCoordinates))
									{
										projectile2.netSyncSkippedForPlayer[num28] = true;
										goto IL_36DC;
									}
									projectile2.netSyncSkippedForPlayer[num28] = false;
								}
								NetMessage.SendPacket(NetMessage.buffer[num].writeBuffer, num28);
							}
							IL_36DC:;
						}
					}
					else
					{
						for (int num29 = 0; num29 < 256; num29++)
						{
							if (num29 != ignoreClient && (NetMessage.buffer[num29].broadcast || (Netplay.Clients[num29].State >= 3 && msgType == 10)) && Netplay.Clients[num29].IsConnected())
							{
								NetMessage.SendPacket(NetMessage.buffer[num].writeBuffer, num29);
							}
						}
					}
				}
				else if (Netplay.Clients[remoteClient].IsConnected())
				{
					if (msgType == 23)
					{
						Main.npc[number].playerNetSyncState[remoteClient] = default(NPC.PlayerNetSyncState);
					}
					else if (msgType == 27)
					{
						Main.projectile[number].netSyncSkippedForPlayer[remoteClient] = false;
					}
					NetMessage.SendPacket(NetMessage.buffer[num].writeBuffer, remoteClient);
				}
				if (Main.verboseNetplay)
				{
					for (int num30 = 0; num30 < num21; num30++)
					{
					}
					for (int num31 = 0; num31 < num21; num31++)
					{
						byte b6 = NetMessage.buffer[num].writeBuffer[num31];
					}
				}
				NetMessage.buffer[num].writeLocked = false;
				if (msgType == 2 && Main.netMode == 2)
				{
					Netplay.Clients[num].PendingTermination = true;
				}
			}
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0003FC48 File Offset: 0x0003DE48
		private static void SendPacketToServer(byte[] data)
		{
			NetMessage.SendPacket(data, 256);
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0003FC58 File Offset: 0x0003DE58
		private static void SendPacket(byte[] data, int remoteClient)
		{
			try
			{
				ushort num = BitConverter.ToUInt16(data, 0);
				byte messageId = data[2];
				NetMessage.buffer[remoteClient].spamCount++;
				Main.ActiveNetDiagnosticsUI.CountSentMessage((int)messageId, (int)num);
				if (!Main.dedServ)
				{
					Netplay.Connection.Socket.AsyncSend(data, 0, (int)num, new SocketSendCallback(Netplay.Connection.ClientWriteCallBack), null);
				}
				else
				{
					Netplay.Clients[remoteClient].Socket.AsyncSend(data, 0, (int)num, new SocketSendCallback(Netplay.Clients[remoteClient].ServerWriteCallBack), null);
				}
			}
			catch
			{
				bool dedServ = Main.dedServ;
			}
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0003FD00 File Offset: 0x0003DF00
		public static void SendChestContentsTo(int chest, int targetPlayer)
		{
			NetMessage.TrySendData(155, targetPlayer, -1, null, chest, (float)Main.chest[chest].maxItems, 0f, 0f, 0, 0, 0);
			for (int i = 0; i < Main.chest[chest].maxItems; i++)
			{
				NetMessage.TrySendData(32, targetPlayer, -1, null, chest, (float)i, 0f, 0f, 0, 0, 0);
			}
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0003FD68 File Offset: 0x0003DF68
		private static void WriteAccessoryVisibility(BinaryWriter writer, bool[] hideVisibleAccessory)
		{
			ushort num = 0;
			for (int i = 0; i < hideVisibleAccessory.Length; i++)
			{
				if (hideVisibleAccessory[i])
				{
					num |= (ushort)(1 << i);
				}
			}
			writer.Write(num);
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0003FD9C File Offset: 0x0003DF9C
		public static void CompressTileBlock(int xStart, int yStart, short width, short height, Stream stream)
		{
			using (DeflateStream deflateStream = new DeflateStream(stream, 0, true))
			{
				BinaryWriter binaryWriter = new BinaryWriter(deflateStream);
				binaryWriter.Write(xStart);
				binaryWriter.Write(yStart);
				binaryWriter.Write(width);
				binaryWriter.Write(height);
				NetMessage.CompressTileBlock_Inner(binaryWriter, xStart, yStart, (int)width, (int)height);
			}
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0003FDFC File Offset: 0x0003DFFC
		public static void CompressTileBlock_Inner(BinaryWriter writer, int xStart, int yStart, int width, int height)
		{
			short num = 0;
			short num2 = 0;
			short num3 = 0;
			short num4 = 0;
			int num5 = 0;
			int num6 = 0;
			byte b = 0;
			byte[] array = new byte[16];
			Tile tile = null;
			for (int i = yStart; i < yStart + height; i++)
			{
				for (int j = xStart; j < xStart + width; j++)
				{
					Tile tile2 = Main.tile[j, i];
					if (tile2.isTheSameAs(tile) && TileID.Sets.AllowsSaveCompressionBatching[(int)tile2.type])
					{
						num4 += 1;
					}
					else
					{
						if (tile != null)
						{
							if (num4 > 0)
							{
								array[num5] = (byte)(num4 & 255);
								num5++;
								if (num4 > 255)
								{
									b |= 128;
									array[num5] = (byte)(((int)num4 & 65280) >> 8);
									num5++;
								}
								else
								{
									b |= 64;
								}
							}
							array[num6] = b;
							writer.Write(array, num6, num5 - num6);
							num4 = 0;
						}
						num5 = 4;
						byte b4;
						byte b3;
						byte b2 = b = (b3 = (b4 = 0));
						if (tile2.active())
						{
							b |= 2;
							array[num5] = (byte)tile2.type;
							num5++;
							if (tile2.type > 255)
							{
								array[num5] = (byte)(tile2.type >> 8);
								num5++;
								b |= 32;
							}
							if (TileID.Sets.BasicChest[(int)tile2.type] && tile2.frameX % 36 == 0 && tile2.frameY % 36 == 0)
							{
								short num7 = (short)Chest.FindChest(j, i);
								if (num7 != -1)
								{
									NetMessage._compressChestList[(int)num] = num7;
									num += 1;
								}
							}
							if (tile2.type == 88 && tile2.frameX % 54 == 0 && tile2.frameY % 36 == 0)
							{
								short num8 = (short)Chest.FindChest(j, i);
								if (num8 != -1)
								{
									NetMessage._compressChestList[(int)num] = num8;
									num += 1;
								}
							}
							if (tile2.type == 85 && tile2.frameX % 36 == 0 && tile2.frameY % 36 == 0)
							{
								short num9 = (short)Sign.ReadSign(j, i, true);
								if (num9 != -1)
								{
									short[] compressSignList = NetMessage._compressSignList;
									short num10 = num2;
									num2 = num10 + 1;
									compressSignList[(int)num10] = num9;
								}
							}
							if (tile2.type == 55 && tile2.frameX % 36 == 0 && tile2.frameY % 36 == 0)
							{
								short num11 = (short)Sign.ReadSign(j, i, true);
								if (num11 != -1)
								{
									short[] compressSignList2 = NetMessage._compressSignList;
									short num12 = num2;
									num2 = num12 + 1;
									compressSignList2[(int)num12] = num11;
								}
							}
							if (tile2.type == 425 && tile2.frameX % 36 == 0 && tile2.frameY % 36 == 0)
							{
								short num13 = (short)Sign.ReadSign(j, i, true);
								if (num13 != -1)
								{
									short[] compressSignList3 = NetMessage._compressSignList;
									short num14 = num2;
									num2 = num14 + 1;
									compressSignList3[(int)num14] = num13;
								}
							}
							if (tile2.type == 573 && tile2.frameX % 36 == 0 && tile2.frameY % 36 == 0)
							{
								short num15 = (short)Sign.ReadSign(j, i, true);
								if (num15 != -1)
								{
									short[] compressSignList4 = NetMessage._compressSignList;
									short num16 = num2;
									num2 = num16 + 1;
									compressSignList4[(int)num16] = num15;
								}
							}
							if (tile2.type == 378 && tile2.frameX % 36 == 0 && tile2.frameY == 0)
							{
								int num17 = TileEntityType<TETrainingDummy>.Find(j, i);
								if (num17 != -1)
								{
									short[] compressEntities = NetMessage._compressEntities;
									short num18 = num3;
									num3 = num18 + 1;
									compressEntities[(int)num18] = (short)num17;
								}
							}
							if (tile2.type == 395 && tile2.frameX % 36 == 0 && tile2.frameY == 0)
							{
								int num19 = TileEntityType<TEItemFrame>.Find(j, i);
								if (num19 != -1)
								{
									short[] compressEntities2 = NetMessage._compressEntities;
									short num20 = num3;
									num3 = num20 + 1;
									compressEntities2[(int)num20] = (short)num19;
								}
							}
							if (tile2.type == 698 && tile2.frameX % 18 == 0 && tile2.frameY == 0)
							{
								int num21 = TileEntityType<TEDeadCellsDisplayJar>.Find(j, i);
								if (num21 != -1)
								{
									short[] compressEntities3 = NetMessage._compressEntities;
									short num22 = num3;
									num3 = num22 + 1;
									compressEntities3[(int)num22] = (short)num21;
								}
							}
							if (tile2.type == 520 && tile2.frameX % 18 == 0 && tile2.frameY == 0)
							{
								int num23 = TileEntityType<TEFoodPlatter>.Find(j, i);
								if (num23 != -1)
								{
									short[] compressEntities4 = NetMessage._compressEntities;
									short num24 = num3;
									num3 = num24 + 1;
									compressEntities4[(int)num24] = (short)num23;
								}
							}
							if (tile2.type == 471 && tile2.frameX % 54 == 0 && tile2.frameY == 0)
							{
								int num25 = TileEntityType<TEWeaponsRack>.Find(j, i);
								if (num25 != -1)
								{
									short[] compressEntities5 = NetMessage._compressEntities;
									short num26 = num3;
									num3 = num26 + 1;
									compressEntities5[(int)num26] = (short)num25;
								}
							}
							if (tile2.type == 470 && tile2.frameX % 36 == 0 && tile2.frameY == 0)
							{
								int num27 = TileEntityType<TEDisplayDoll>.Find(j, i);
								if (num27 != -1)
								{
									short[] compressEntities6 = NetMessage._compressEntities;
									short num28 = num3;
									num3 = num28 + 1;
									compressEntities6[(int)num28] = (short)num27;
								}
							}
							if (tile2.type == 475 && tile2.frameX % 54 == 0 && tile2.frameY == 0)
							{
								int num29 = TileEntityType<TEHatRack>.Find(j, i);
								if (num29 != -1)
								{
									short[] compressEntities7 = NetMessage._compressEntities;
									short num30 = num3;
									num3 = num30 + 1;
									compressEntities7[(int)num30] = (short)num29;
								}
							}
							if (tile2.type == 597 && tile2.frameX % 54 == 0 && tile2.frameY % 72 == 0)
							{
								int num31 = TileEntityType<TETeleportationPylon>.Find(j, i);
								if (num31 != -1)
								{
									short[] compressEntities8 = NetMessage._compressEntities;
									short num32 = num3;
									num3 = num32 + 1;
									compressEntities8[(int)num32] = (short)num31;
								}
							}
							if (Main.tileFrameImportant[(int)tile2.type])
							{
								array[num5] = (byte)(tile2.frameX & 255);
								num5++;
								array[num5] = (byte)(((int)tile2.frameX & 65280) >> 8);
								num5++;
								array[num5] = (byte)(tile2.frameY & 255);
								num5++;
								array[num5] = (byte)(((int)tile2.frameY & 65280) >> 8);
								num5++;
							}
							if (tile2.color() != 0)
							{
								b3 |= 8;
								array[num5] = tile2.color();
								num5++;
							}
						}
						if (tile2.wall != 0)
						{
							b |= 4;
							array[num5] = (byte)tile2.wall;
							num5++;
							if (tile2.wallColor() != 0)
							{
								b3 |= 16;
								array[num5] = tile2.wallColor();
								num5++;
							}
						}
						if (tile2.liquid != 0)
						{
							if (tile2.shimmer())
							{
								b3 |= 128;
								b |= 8;
							}
							else if (tile2.lava())
							{
								b |= 16;
							}
							else if (tile2.honey())
							{
								b |= 24;
							}
							else
							{
								b |= 8;
							}
							array[num5] = tile2.liquid;
							num5++;
						}
						if (tile2.wire())
						{
							b2 |= 2;
						}
						if (tile2.wire2())
						{
							b2 |= 4;
						}
						if (tile2.wire3())
						{
							b2 |= 8;
						}
						int num33;
						if (tile2.halfBrick())
						{
							num33 = 16;
						}
						else if (tile2.slope() != 0)
						{
							num33 = (int)(tile2.slope() + 1) << 4;
						}
						else
						{
							num33 = 0;
						}
						b2 |= (byte)num33;
						if (tile2.actuator())
						{
							b3 |= 2;
						}
						if (tile2.inActive())
						{
							b3 |= 4;
						}
						if (tile2.wire4())
						{
							b3 |= 32;
						}
						if (tile2.wall > 255)
						{
							array[num5] = (byte)(tile2.wall >> 8);
							num5++;
							b3 |= 64;
						}
						if (tile2.invisibleBlock())
						{
							b4 |= 2;
						}
						if (tile2.invisibleWall())
						{
							b4 |= 4;
						}
						if (tile2.fullbrightBlock())
						{
							b4 |= 8;
						}
						if (tile2.fullbrightWall())
						{
							b4 |= 16;
						}
						num6 = 3;
						if (b4 != 0)
						{
							b3 |= 1;
							array[num6] = b4;
							num6--;
						}
						if (b3 != 0)
						{
							b2 |= 1;
							array[num6] = b3;
							num6--;
						}
						if (b2 != 0)
						{
							b |= 1;
							array[num6] = b2;
							num6--;
						}
						tile = tile2;
					}
				}
			}
			if (num4 > 0)
			{
				array[num5] = (byte)(num4 & 255);
				num5++;
				if (num4 > 255)
				{
					b |= 128;
					array[num5] = (byte)(((int)num4 & 65280) >> 8);
					num5++;
				}
				else
				{
					b |= 64;
				}
			}
			array[num6] = b;
			writer.Write(array, num6, num5 - num6);
			writer.Write(num);
			for (int k = 0; k < (int)num; k++)
			{
				Chest chest = Main.chest[(int)NetMessage._compressChestList[k]];
				writer.Write(NetMessage._compressChestList[k]);
				writer.Write((short)chest.x);
				writer.Write((short)chest.y);
				writer.Write(chest.name);
			}
			writer.Write(num2);
			for (int l = 0; l < (int)num2; l++)
			{
				Sign sign = Main.sign[(int)NetMessage._compressSignList[l]];
				writer.Write(NetMessage._compressSignList[l]);
				writer.Write((short)sign.x);
				writer.Write((short)sign.y);
				writer.Write(sign.text);
			}
			writer.Write(num3);
			for (int m = 0; m < (int)num3; m++)
			{
				TileEntity.Write(writer, TileEntity.ByID[(int)NetMessage._compressEntities[m]], false);
			}
		}

		// Token: 0x060002AE RID: 686 RVA: 0x00040698 File Offset: 0x0003E898
		public static void DecompressTileBlock(Stream stream)
		{
			using (DeflateStream deflateStream = new DeflateStream(stream, 1, true))
			{
				BinaryReader binaryReader = new BinaryReader(deflateStream);
				NetMessage.DecompressTileBlock_Inner(binaryReader, binaryReader.ReadInt32(), binaryReader.ReadInt32(), (int)binaryReader.ReadInt16(), (int)binaryReader.ReadInt16());
			}
		}

		// Token: 0x060002AF RID: 687 RVA: 0x000406F0 File Offset: 0x0003E8F0
		public static void DecompressTileBlock_Inner(BinaryReader reader, int xStart, int yStart, int width, int height)
		{
			Tile tile = null;
			int num = 0;
			for (int i = yStart; i < yStart + height; i++)
			{
				for (int j = xStart; j < xStart + width; j++)
				{
					if (num != 0)
					{
						num--;
						if (Main.tile[j, i] == null)
						{
							Main.tile[j, i] = new Tile(tile);
						}
						else
						{
							Main.tile[j, i].CopyFrom(tile);
						}
					}
					else
					{
						byte b3;
						byte b2;
						byte b = b2 = (b3 = 0);
						tile = Main.tile[j, i];
						if (tile == null)
						{
							tile = new Tile();
							Main.tile[j, i] = tile;
						}
						else
						{
							tile.ClearEverything();
						}
						byte b4 = reader.ReadByte();
						bool flag = false;
						if ((b4 & 1) == 1)
						{
							flag = true;
							b2 = reader.ReadByte();
						}
						bool flag2 = false;
						if (flag && (b2 & 1) == 1)
						{
							flag2 = true;
							b = reader.ReadByte();
						}
						if (flag2 && (b & 1) == 1)
						{
							b3 = reader.ReadByte();
						}
						bool flag3 = tile.active();
						byte b5;
						if ((b4 & 2) == 2)
						{
							tile.active(true);
							ushort type = tile.type;
							int num2;
							if ((b4 & 32) == 32)
							{
								b5 = reader.ReadByte();
								num2 = (int)reader.ReadByte();
								num2 = (num2 << 8 | (int)b5);
							}
							else
							{
								num2 = (int)reader.ReadByte();
							}
							tile.type = (ushort)num2;
							if (Main.tileFrameImportant[num2])
							{
								tile.frameX = reader.ReadInt16();
								tile.frameY = reader.ReadInt16();
							}
							else if (!flag3 || tile.type != type)
							{
								tile.frameX = -1;
								tile.frameY = -1;
							}
							if ((b & 8) == 8)
							{
								tile.color(reader.ReadByte());
							}
						}
						if ((b4 & 4) == 4)
						{
							tile.wall = (ushort)reader.ReadByte();
							if ((b & 16) == 16)
							{
								tile.wallColor(reader.ReadByte());
							}
						}
						b5 = (byte)((b4 & 24) >> 3);
						if (b5 != 0)
						{
							tile.liquid = reader.ReadByte();
							if ((b & 128) == 128)
							{
								tile.shimmer(true);
							}
							else if (b5 > 1)
							{
								if (b5 == 2)
								{
									tile.lava(true);
								}
								else
								{
									tile.honey(true);
								}
							}
						}
						if (b2 > 1)
						{
							if ((b2 & 2) == 2)
							{
								tile.wire(true);
							}
							if ((b2 & 4) == 4)
							{
								tile.wire2(true);
							}
							if ((b2 & 8) == 8)
							{
								tile.wire3(true);
							}
							b5 = (byte)((b2 & 112) >> 4);
							if (b5 != 0 && Main.tileSolid[(int)tile.type])
							{
								if (b5 == 1)
								{
									tile.halfBrick(true);
								}
								else
								{
									tile.slope(b5 - 1);
								}
							}
						}
						if (b > 1)
						{
							if ((b & 2) == 2)
							{
								tile.actuator(true);
							}
							if ((b & 4) == 4)
							{
								tile.inActive(true);
							}
							if ((b & 32) == 32)
							{
								tile.wire4(true);
							}
							if ((b & 64) == 64)
							{
								b5 = reader.ReadByte();
								tile.wall = (ushort)((int)b5 << 8 | (int)tile.wall);
							}
						}
						if (b3 > 1)
						{
							if ((b3 & 2) == 2)
							{
								tile.invisibleBlock(true);
							}
							if ((b3 & 4) == 4)
							{
								tile.invisibleWall(true);
							}
							if ((b3 & 8) == 8)
							{
								tile.fullbrightBlock(true);
							}
							if ((b3 & 16) == 16)
							{
								tile.fullbrightWall(true);
							}
						}
						b5 = (byte)((b4 & 192) >> 6);
						if (b5 == 0)
						{
							num = 0;
						}
						else if (b5 == 1)
						{
							num = (int)reader.ReadByte();
						}
						else
						{
							num = (int)reader.ReadInt16();
						}
					}
				}
			}
			short num3 = reader.ReadInt16();
			for (int k = 0; k < (int)num3; k++)
			{
				short num4 = reader.ReadInt16();
				short x = reader.ReadInt16();
				short y = reader.ReadInt16();
				string name = reader.ReadString();
				if (num4 >= 0 && num4 < 8000)
				{
					Chest.CreateWorldChest((int)num4, (int)x, (int)y).name = name;
				}
			}
			num3 = reader.ReadInt16();
			for (int l = 0; l < (int)num3; l++)
			{
				short num5 = reader.ReadInt16();
				short x2 = reader.ReadInt16();
				short y2 = reader.ReadInt16();
				string text = reader.ReadString();
				if (num5 >= 0 && num5 < 32000)
				{
					if (Main.sign[(int)num5] == null)
					{
						Main.sign[(int)num5] = new Sign();
					}
					Main.sign[(int)num5].text = text;
					Main.sign[(int)num5].x = (int)x2;
					Main.sign[(int)num5].y = (int)y2;
				}
			}
			num3 = reader.ReadInt16();
			for (int m = 0; m < (int)num3; m++)
			{
				TileEntity.Add(TileEntity.Read(reader, 318, false));
			}
			MapUpdateQueue.Add(new Rectangle(xStart, yStart, width, height));
			Main.sectionManager.SetTilesLoaded(xStart, yStart, xStart + width - 1, yStart + height - 1);
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x00040B5C File Offset: 0x0003ED5C
		public static void ReceiveBytes(byte[] bytes, int streamLength, int i = 256)
		{
			MessageBuffer obj = NetMessage.buffer[i];
			lock (obj)
			{
				try
				{
					Buffer.BlockCopy(bytes, 0, NetMessage.buffer[i].readBuffer, NetMessage.buffer[i].totalData, streamLength);
					NetMessage.buffer[i].totalData += streamLength;
					NetMessage.buffer[i].checkBytes = true;
				}
				catch
				{
					if (Main.netMode == 1)
					{
						Main.menuMode = 15;
						Main.statusText = Language.GetTextValue("Error.BadHeaderBufferOverflow");
						Netplay.Disconnect = true;
					}
					else
					{
						Netplay.Clients[i].PendingTermination = true;
					}
				}
			}
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x00040C1C File Offset: 0x0003EE1C
		public static void CheckBytes(int bufferIndex = 256)
		{
			if (Main.dedServ && Netplay.Clients[bufferIndex].PendingTermination)
			{
				Netplay.Clients[bufferIndex].PendingTerminationApproved = true;
				return;
			}
			if (!Main.dedServ && !Netplay.Connection.IsConnected() && !Netplay.Connection.IsReading && !NetMessage.buffer[bufferIndex].checkBytes)
			{
				Netplay.Disconnect = true;
				Main.statusText = Language.GetTextValue("Net.LostConnection");
			}
			if (!NetMessage.buffer[bufferIndex].checkBytes)
			{
				return;
			}
			MessageBuffer obj = NetMessage.buffer[bufferIndex];
			lock (obj)
			{
				NetMessage.buffer[bufferIndex].checkBytes = false;
				int num = 0;
				int i = NetMessage.buffer[bufferIndex].totalData;
				try
				{
					while (i >= 2)
					{
						int num2 = (int)BitConverter.ToUInt16(NetMessage.buffer[bufferIndex].readBuffer, num);
						if (num2 < 3)
						{
							throw new IndexOutOfRangeException("Invalid packet. Message size too small (" + num2 + ")");
						}
						if (i < num2)
						{
							break;
						}
						long position = NetMessage.buffer[bufferIndex].reader.BaseStream.Position;
						int num3;
						NetMessage.buffer[bufferIndex].GetData(num + 2, num2 - 2, out num3);
						NetMessage.buffer[bufferIndex].reader.BaseStream.Position = position + (long)num2;
						i -= num2;
						num += num2;
					}
				}
				catch (Exception)
				{
					if (Main.dedServ && num < NetMessage.buffer.Length - 100)
					{
						Console.WriteLine(Language.GetTextValue("Error.NetMessageError", NetMessage.buffer[num + 2]));
					}
					i = 0;
					num = 0;
				}
				if (i != NetMessage.buffer[bufferIndex].totalData)
				{
					for (int j = 0; j < i; j++)
					{
						NetMessage.buffer[bufferIndex].readBuffer[j] = NetMessage.buffer[bufferIndex].readBuffer[j + num];
					}
					NetMessage.buffer[bufferIndex].totalData = i;
				}
			}
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x00040E28 File Offset: 0x0003F028
		public static void BootPlayer(int plr, NetworkText msg)
		{
			NetMessage.SendData(2, plr, -1, msg, 0, 0f, 0f, 0f, 0, 0, 0);
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x00040E54 File Offset: 0x0003F054
		public static void SendObjectPlacement(int whoAmi, int x, int y, int type, int style, int alternative, int random, int direction)
		{
			int remoteClient;
			int ignoreClient;
			if (Main.netMode == 2)
			{
				remoteClient = -1;
				ignoreClient = whoAmi;
			}
			else
			{
				remoteClient = whoAmi;
				ignoreClient = -1;
			}
			NetMessage.SendData(79, remoteClient, ignoreClient, null, x, (float)y, (float)type, (float)style, alternative, random, direction);
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x00040E8C File Offset: 0x0003F08C
		public static void SendTemporaryAnimation(int whoAmi, int animationType, int tileType, int xCoord, int yCoord)
		{
			if (Main.netMode == 2)
			{
				NetMessage.SendData(77, whoAmi, -1, null, animationType, (float)tileType, (float)xCoord, (float)yCoord, 0, 0, 0);
			}
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x00040EB8 File Offset: 0x0003F0B8
		public static void SendPlayerHurt(int playerTargetIndex, PlayerDeathReason reason, int damage, int direction, bool critical, bool pvp, int hitContext, int remoteClient = -1, int ignoreClient = -1)
		{
			NetMessage._currentPlayerDeathReason = reason;
			BitsByte bb = 0;
			bb[0] = critical;
			bb[1] = pvp;
			NetMessage.SendData(117, remoteClient, ignoreClient, null, playerTargetIndex, (float)damage, (float)direction, (float)bb, hitContext, 0, 0);
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x00040F04 File Offset: 0x0003F104
		public static void SendPlayerDeath(int playerTargetIndex, PlayerDeathReason reason, int damage, int direction, bool pvp, int remoteClient = -1, int ignoreClient = -1)
		{
			NetMessage._currentPlayerDeathReason = reason;
			BitsByte bb = 0;
			bb[0] = pvp;
			NetMessage.SendData(118, remoteClient, ignoreClient, null, playerTargetIndex, (float)damage, (float)direction, (float)bb, 0, 0, 0);
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x00040F44 File Offset: 0x0003F144
		public static void PlayNetSound(NetMessage.NetSoundInfo info, int remoteClient = -1, int ignoreClient = -1)
		{
			NetMessage._currentNetSoundInfo = info;
			NetMessage.SendData(132, remoteClient, ignoreClient, null, 0, 0f, 0f, 0f, 0, 0, 0);
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x00040F78 File Offset: 0x0003F178
		public static void SendCoinLossRevengeMarker(CoinLossRevengeSystem.RevengeMarker marker, int remoteClient = -1, int ignoreClient = -1)
		{
			NetMessage._currentRevengeMarker = marker;
			NetMessage.SendData(126, remoteClient, ignoreClient, null, 0, 0f, 0f, 0f, 0, 0, 0);
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x00040FA8 File Offset: 0x0003F1A8
		public static void SendTileSquare(int whoAmi, int tileX, int tileY, int xSize, int ySize, TileChangeType changeType = TileChangeType.None)
		{
			NetMessage.SendData(20, whoAmi, -1, null, tileX, (float)tileY, (float)xSize, (float)ySize, (int)changeType, 0, 0);
		}

		// Token: 0x060002BA RID: 698 RVA: 0x00040FCC File Offset: 0x0003F1CC
		public static void SendTileSquare(int whoAmi, int tileX, int tileY, int centeredSquareSize, TileChangeType changeType = TileChangeType.None)
		{
			int num = (centeredSquareSize - 1) / 2;
			NetMessage.SendTileSquare(whoAmi, tileX - num, tileY - num, centeredSquareSize, centeredSquareSize, changeType);
		}

		// Token: 0x060002BB RID: 699 RVA: 0x00040FF0 File Offset: 0x0003F1F0
		public static void SendTileSquare(int whoAmi, int tileX, int tileY, TileChangeType changeType = TileChangeType.None)
		{
			int num = 1;
			int num2 = (num - 1) / 2;
			NetMessage.SendTileSquare(whoAmi, tileX - num2, tileY - num2, num, num, changeType);
		}

		// Token: 0x060002BC RID: 700 RVA: 0x00041014 File Offset: 0x0003F214
		public static void SendTravelShop(int remoteClient)
		{
			if (Main.netMode == 2)
			{
				NetMessage.SendData(72, remoteClient, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
			}
		}

		// Token: 0x060002BD RID: 701 RVA: 0x00041048 File Offset: 0x0003F248
		public static void SendAnglerQuest(int remoteClient)
		{
			if (Main.netMode != 2)
			{
				return;
			}
			if (remoteClient == -1)
			{
				for (int i = 0; i < 255; i++)
				{
					if (Netplay.Clients[i].State == 10)
					{
						NetMessage.SendData(74, i, -1, NetworkText.FromLiteral(Main.player[i].name), Main.anglerQuest, 0f, 0f, 0f, 0, 0, 0);
					}
				}
				return;
			}
			if (Netplay.Clients[remoteClient].State == 10)
			{
				NetMessage.SendData(74, remoteClient, -1, NetworkText.FromLiteral(Main.player[remoteClient].name), Main.anglerQuest, 0f, 0f, 0f, 0, 0, 0);
			}
		}

		// Token: 0x060002BE RID: 702 RVA: 0x000410F8 File Offset: 0x0003F2F8
		public static void ResyncTiles(Rectangle area)
		{
			for (int i = 0; i < Netplay.Clients.Length; i++)
			{
				if (Netplay.Clients[i].IsActive)
				{
					NetMessage.ResyncTiles(i, area);
				}
			}
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0004112C File Offset: 0x0003F32C
		private static void ResyncTiles(int clientId, Rectangle area)
		{
			for (int i = area.Left; i < area.Right; i += 200)
			{
				for (int j = area.Top; j < area.Bottom; j += 150)
				{
					NetMessage.SendData(10, clientId, -1, null, i, (float)j, (float)Math.Min(area.Right - i, 200), (float)Math.Min(area.Bottom - j, 150), 0, 0, 0);
				}
			}
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x000411AC File Offset: 0x0003F3AC
		public static void SendSection(int whoAmi, int sectionX, int sectionY)
		{
			if (Main.netMode != 2)
			{
				return;
			}
			try
			{
				if (sectionX >= 0 && sectionY >= 0 && sectionX < Main.maxSectionsX && sectionY < Main.maxSectionsY)
				{
					if (!Netplay.Clients[whoAmi].TileSections[sectionX, sectionY])
					{
						Netplay.Clients[whoAmi].TileSections[sectionX, sectionY] = true;
						int number = sectionX * 200;
						int num = sectionY * 150;
						int num2 = 150;
						for (int i = num; i < num + 150; i += num2)
						{
							NetMessage.SendData(10, whoAmi, -1, null, number, (float)i, 200f, (float)num2, 0, 0, 0);
						}
						NetMessage.SyncNPCsForSection(whoAmi, sectionX, sectionY);
						NetMessage.SyncChestContentsForSection(whoAmi, sectionX, sectionY);
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0004126C File Offset: 0x0003F46C
		private static void SyncChestContentsForSection(int whoAmi, int sectionX, int sectionY)
		{
			for (int i = 0; i < 8000; i++)
			{
				Chest chest = Main.chest[i];
				if (chest != null)
				{
					int sectionX2 = Netplay.GetSectionX(chest.x);
					int sectionY2 = Netplay.GetSectionY(chest.y);
					if (sectionX == sectionX2 && sectionY == sectionY2)
					{
						NetMessage.SendChestContentsTo(i, whoAmi);
					}
				}
			}
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x000412BC File Offset: 0x0003F4BC
		private static void SyncNPCsForSection(int whoAmi, int sectionX, int sectionY)
		{
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				if (Main.npc[i].active && Main.npc[i].townNPC)
				{
					int sectionX2 = Netplay.GetSectionX((int)(Main.npc[i].position.X / 16f));
					int sectionY2 = Netplay.GetSectionY((int)(Main.npc[i].position.Y / 16f));
					if (sectionX2 == sectionX && sectionY2 == sectionY)
					{
						NetMessage.SendData(23, whoAmi, -1, null, i, 0f, 0f, 0f, 0, 0, 0);
					}
				}
			}
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x00041358 File Offset: 0x0003F558
		public static void greetPlayer(int plr)
		{
			if (Main.motd == "")
			{
				ChatHelper.SendChatMessageToClient(NetworkText.FromFormattable("{0} {1}!", new object[]
				{
					Lang.mp[18].ToNetworkText(),
					Main.worldName
				}), new Color(255, 240, 20), plr);
			}
			else
			{
				ChatHelper.SendChatMessageToClient(NetworkText.FromLiteral(Main.motd), new Color(255, 240, 20), plr);
			}
			string text = "";
			for (int i = 0; i < 255; i++)
			{
				if (Main.player[i].active)
				{
					if (text == "")
					{
						text += Main.player[i].name;
					}
					else
					{
						text = text + ", " + Main.player[i].name;
					}
				}
			}
			ChatHelper.SendChatMessageToClient(NetworkText.FromKey("Game.JoinGreeting", new object[]
			{
				text
			}), new Color(255, 240, 20), plr);
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x00041464 File Offset: 0x0003F664
		public static void sendWater(int x, int y)
		{
			if (Main.netMode == 1)
			{
				NetMessage.SendData(48, -1, -1, null, x, (float)y, 0f, 0f, 0, 0, 0);
				return;
			}
			for (int i = 0; i < 256; i++)
			{
				if ((NetMessage.buffer[i].broadcast || Netplay.Clients[i].State >= 3) && Netplay.Clients[i].IsConnected())
				{
					int num = x / 200;
					int num2 = y / 150;
					if (Netplay.Clients[i].TileSections[num, num2])
					{
						NetMessage.SendData(48, i, -1, null, x, (float)y, 0f, 0f, 0, 0, 0);
					}
				}
			}
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0004150E File Offset: 0x0003F70E
		public static void SyncDisconnectedPlayer(int plr)
		{
			NetMessage.SyncOnePlayer(plr, -1, plr);
			NetMessage.EnsureLocalPlayerIsPresent();
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x00041520 File Offset: 0x0003F720
		public static void SyncConnectedPlayer(int plr)
		{
			NetMessage.SyncOnePlayer(plr, -1, plr);
			for (int i = 0; i < 255; i++)
			{
				if (plr != i && Main.player[i].active)
				{
					NetMessage.SyncOnePlayer(i, plr, -1);
				}
			}
			NetMessage.SendNPCHousesAndTravelShop(plr);
			NetMessage.SendAnglerQuest(plr);
			CreditsRollEvent.SendCreditsRollRemainingTimeToPlayer(plr);
			NPC.RevengeManager.SendAllMarkersToPlayer(plr);
			NetMessage.EnsureLocalPlayerIsPresent();
			DebugOptions.SyncToJoiningPlayer(plr);
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x00041588 File Offset: 0x0003F788
		private static void SendNPCHousesAndTravelShop(int plr)
		{
			bool flag = false;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC npc = Main.npc[i];
				if (npc.active)
				{
					bool flag2 = npc.townNPC && NPC.TypeToDefaultHeadIndex(npc.type) > 0;
					if (npc.aiStyle == 7)
					{
						flag2 = true;
					}
					if (flag2)
					{
						if (!flag && npc.type == 368)
						{
							flag = true;
						}
						byte householdStatus = WorldGen.TownManager.GetHouseholdStatus(npc);
						NetMessage.SendData(60, plr, -1, null, i, (float)npc.homeTileX, (float)npc.homeTileY, (float)householdStatus, 0, 0, 0);
					}
				}
			}
			if (flag)
			{
				NetMessage.SendTravelShop(plr);
			}
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0004162C File Offset: 0x0003F82C
		private static void EnsureLocalPlayerIsPresent()
		{
			if (!Main.autoShutdown)
			{
				return;
			}
			bool flag = false;
			for (int i = 0; i < 255; i++)
			{
				if (NetMessage.DoesPlayerSlotCountAsAHost(i))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				Console.WriteLine(Language.GetTextValue("Net.ServerAutoShutdown"));
				Netplay.Disconnect = true;
			}
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x00041677 File Offset: 0x0003F877
		public static bool DoesPlayerSlotCountAsAHost(int plr)
		{
			return Netplay.Clients[plr].State == 10 && Netplay.Clients[plr].Socket.GetRemoteAddress().IsLocalHost();
		}

		// Token: 0x060002CA RID: 714 RVA: 0x000416A4 File Offset: 0x0003F8A4
		private static void SyncOnePlayer(int plr, int toWho, int fromWho)
		{
			int num = 0;
			if (Main.player[plr].active)
			{
				num = 1;
			}
			if (Netplay.Clients[plr].State == 10)
			{
				NetMessage.SendData(14, toWho, fromWho, null, plr, (float)num, 0f, 0f, 0, 0, 0);
				NetMessage.SendData(4, toWho, fromWho, null, plr, 0f, 0f, 0f, 0, 0, 0);
				NetMessage.SendData(13, toWho, fromWho, null, plr, 0f, 0f, 0f, 0, 0, 0);
				if (Main.player[plr].statLife <= 0)
				{
					NetMessage.SendData(135, toWho, fromWho, null, plr, 0f, 0f, 0f, 0, 0, 0);
				}
				NetMessage.SendData(16, toWho, fromWho, null, plr, 0f, 0f, 0f, 0, 0, 0);
				NetMessage.SendData(30, toWho, fromWho, null, plr, 0f, 0f, 0f, 0, 0, 0);
				NetMessage.SendData(45, toWho, fromWho, null, plr, 0f, 0f, 0f, 0, 0, 0);
				NetMessage.SendData(42, toWho, fromWho, null, plr, 0f, 0f, 0f, 0, 0, 0);
				NetMessage.SendData(50, toWho, fromWho, null, plr, 0f, 0f, 0f, 0, 0, 0);
				NetMessage.SendData(80, toWho, fromWho, null, plr, (float)Main.player[plr].chest, 0f, 0f, 0, 0, 0);
				NetMessage.SendData(142, toWho, fromWho, null, plr, 0f, 0f, 0f, 0, 0, 0);
				NetMessage.SendData(147, toWho, fromWho, null, plr, (float)Main.player[plr].CurrentLoadoutIndex, 0f, 0f, 0, 0, 0);
				TagEffectState.NetModule.SyncStateIfNecessary(Main.player[plr].TagEffectState, toWho, fromWho);
				for (int i = 0; i < 59; i++)
				{
					NetMessage.SendData(5, toWho, fromWho, null, plr, (float)(PlayerItemSlotID.Inventory0 + i), 0f, 0f, 0, 0, 0);
				}
				for (int j = 0; j < Main.player[plr].armor.Length; j++)
				{
					NetMessage.SendData(5, toWho, fromWho, null, plr, (float)(PlayerItemSlotID.Armor0 + j), 0f, 0f, 0, 0, 0);
				}
				for (int k = 0; k < Main.player[plr].dye.Length; k++)
				{
					NetMessage.SendData(5, toWho, fromWho, null, plr, (float)(PlayerItemSlotID.Dye0 + k), 0f, 0f, 0, 0, 0);
				}
				NetMessage.SyncOnePlayer_ItemArray(plr, toWho, fromWho, Main.player[plr].miscEquips, PlayerItemSlotID.Misc0);
				NetMessage.SyncOnePlayer_ItemArray(plr, toWho, fromWho, Main.player[plr].miscDyes, PlayerItemSlotID.MiscDye0);
				NetMessage.SyncOnePlayer_ItemArray(plr, toWho, fromWho, Main.player[plr].Loadouts[0].Armor, PlayerItemSlotID.Loadout1_Armor_0);
				NetMessage.SyncOnePlayer_ItemArray(plr, toWho, fromWho, Main.player[plr].Loadouts[0].Dye, PlayerItemSlotID.Loadout1_Dye_0);
				NetMessage.SyncOnePlayer_ItemArray(plr, toWho, fromWho, Main.player[plr].Loadouts[1].Armor, PlayerItemSlotID.Loadout2_Armor_0);
				NetMessage.SyncOnePlayer_ItemArray(plr, toWho, fromWho, Main.player[plr].Loadouts[1].Dye, PlayerItemSlotID.Loadout2_Dye_0);
				NetMessage.SyncOnePlayer_ItemArray(plr, toWho, fromWho, Main.player[plr].Loadouts[2].Armor, PlayerItemSlotID.Loadout3_Armor_0);
				NetMessage.SyncOnePlayer_ItemArray(plr, toWho, fromWho, Main.player[plr].Loadouts[2].Dye, PlayerItemSlotID.Loadout3_Dye_0);
				if (!Netplay.Clients[plr].IsAnnouncementCompleted)
				{
					Netplay.Clients[plr].IsAnnouncementCompleted = true;
					ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.mp[19].Key, new object[]
					{
						Main.player[plr].name
					}), new Color(255, 240, 20), plr);
					if (Main.dedServ)
					{
						Console.WriteLine(Lang.mp[19].Format(Main.player[plr].name));
					}
				}
				for (int l = 0; l < 1000; l++)
				{
					Projectile projectile = Main.projectile[l];
					if (projectile.active && projectile.owner == plr)
					{
						NetMessage.SendData(27, toWho, -1, null, l, 0f, 0f, 0f, 0, 0, 0);
					}
				}
				return;
			}
			num = 0;
			NetMessage.SendData(14, -1, plr, null, plr, (float)num, 0f, 0f, 0, 0, 0);
			if (Netplay.Clients[plr].IsAnnouncementCompleted)
			{
				Netplay.Clients[plr].IsAnnouncementCompleted = false;
				ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.mp[20].Key, new object[]
				{
					Netplay.Clients[plr].Name
				}), new Color(255, 240, 20), plr);
				if (Main.dedServ)
				{
					Console.WriteLine(Lang.mp[20].Format(Netplay.Clients[plr].Name));
				}
				Netplay.Clients[plr].Name = "Anonymous";
			}
			Player.Hooks.PlayerDisconnect(plr);
		}

		// Token: 0x060002CB RID: 715 RVA: 0x00041B7C File Offset: 0x0003FD7C
		private static void SyncOnePlayer_ItemArray(int plr, int toWho, int fromWho, Item[] arr, int slot)
		{
			for (int i = 0; i < arr.Length; i++)
			{
				NetMessage.SendData(5, toWho, fromWho, null, plr, (float)(slot + i), 0f, 0f, 0, 0, 0);
			}
		}

		// Token: 0x040001F4 RID: 500
		public static MessageBuffer[] buffer = new MessageBuffer[257];

		// Token: 0x040001F5 RID: 501
		private static short[] _compressChestList = new short[8000];

		// Token: 0x040001F6 RID: 502
		private static short[] _compressSignList = new short[32000];

		// Token: 0x040001F7 RID: 503
		private static short[] _compressEntities = new short[1000];

		// Token: 0x040001F8 RID: 504
		private static PlayerDeathReason _currentPlayerDeathReason;

		// Token: 0x040001F9 RID: 505
		private static NetMessage.NetSoundInfo _currentNetSoundInfo;

		// Token: 0x040001FA RID: 506
		private static CoinLossRevengeSystem.RevengeMarker _currentRevengeMarker;

		// Token: 0x02000604 RID: 1540
		public struct NetSoundInfo
		{
			// Token: 0x06003BA2 RID: 15266 RVA: 0x00659F4F File Offset: 0x0065814F
			public NetSoundInfo(Vector2 position, ushort soundIndex, int style = -1, float volume = -1f, float pitchOffset = -1f)
			{
				this.position = position;
				this.soundIndex = soundIndex;
				this.style = style;
				this.volume = volume;
				this.pitchOffset = pitchOffset;
			}

			// Token: 0x06003BA3 RID: 15267 RVA: 0x00659F78 File Offset: 0x00658178
			public void WriteSelfTo(BinaryWriter writer)
			{
				writer.WriteVector2(this.position);
				writer.Write(this.soundIndex);
				BitsByte bb = new BitsByte(this.style != -1, this.volume != -1f, this.pitchOffset != -1f, false, false, false, false, false);
				writer.Write(bb);
				if (bb[0])
				{
					writer.Write(this.style);
				}
				if (bb[1])
				{
					writer.Write(this.volume);
				}
				if (bb[2])
				{
					writer.Write(this.pitchOffset);
				}
			}

			// Token: 0x040063FF RID: 25599
			public Vector2 position;

			// Token: 0x04006400 RID: 25600
			public ushort soundIndex;

			// Token: 0x04006401 RID: 25601
			public int style;

			// Token: 0x04006402 RID: 25602
			public float volume;

			// Token: 0x04006403 RID: 25603
			public float pitchOffset;
		}
	}
}
