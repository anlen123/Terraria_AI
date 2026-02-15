using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.GameContent.Events;
using Terraria.GameContent.Tile_Entities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Social;
using Terraria.UI;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.IO
{
	// Token: 0x02000075 RID: 117
	public class WorldFile
	{
		// Token: 0x0600150F RID: 5391 RVA: 0x004BCF25 File Offset: 0x004BB125
		public static void ReuseTempsForNextSave()
		{
			WorldFile._reuseTempsForNextSave = true;
		}

		// Token: 0x06001510 RID: 5392 RVA: 0x004BCF30 File Offset: 0x004BB130
		public static void SetOngoingToTemps()
		{
			Main.dayTime = WorldFile._tempDayTime;
			Main.time = WorldFile._tempTime;
			Main.moonPhase = WorldFile._tempMoonPhase;
			Main.bloodMoon = WorldFile._tempBloodMoon;
			Main.eclipse = WorldFile._tempEclipse;
			Main.raining = WorldFile._tempRaining;
			Main.rainTime = WorldFile._tempRainTime;
			Main.maxRaining = WorldFile._tempMaxRain;
			Main.cloudAlpha = WorldFile._tempMaxRain;
			CultistRitual.delay = WorldFile._tempCultistDelay;
			BirthdayParty.ManualParty = WorldFile._tempPartyManual;
			BirthdayParty.GenuineParty = WorldFile._tempPartyGenuine;
			BirthdayParty.PartyDaysOnCooldown = WorldFile._tempPartyCooldown;
			BirthdayParty.CelebratingNPCs.Clear();
			BirthdayParty.CelebratingNPCs.AddRange(WorldFile.TempPartyCelebratingNPCs);
			Sandstorm.Happening = WorldFile._tempSandstormHappening;
			Sandstorm.TimeLeft = WorldFile._tempSandstormTimeLeft;
			Sandstorm.Severity = WorldFile._tempSandstormSeverity;
			Sandstorm.IntendedSeverity = WorldFile._tempSandstormIntendedSeverity;
			LanternNight.GenuineLanterns = WorldFile._tempLanternNightGenuine;
			LanternNight.LanternNightsOnCooldown = WorldFile._tempLanternNightCooldown;
			LanternNight.ManualLanterns = WorldFile._tempLanternNightManual;
			LanternNight.NextNightIsLanternNight = WorldFile._tempLanternNightNextNightIsGenuine;
			WorldGen.meteorShowerCount = WorldFile._tempMeteorShowerCount;
			Main.coinRain = WorldFile._tempCoinRain;
		}

		// Token: 0x06001511 RID: 5393 RVA: 0x004BD03C File Offset: 0x004BB23C
		public static bool IsValidWorld(string file, bool cloudSave)
		{
			return WorldFile.GetFileMetadata(file, cloudSave) != null;
		}

		// Token: 0x06001512 RID: 5394 RVA: 0x004BD048 File Offset: 0x004BB248
		public static WorldFileData GetAllMetadata(string file, bool cloudSave)
		{
			WorldFileData result;
			try
			{
				if (!FileUtilities.Exists(file, cloudSave))
				{
					throw new FileNotFoundException(file);
				}
				using (Stream stream = cloudSave ? new MemoryStream(SocialAPI.Cloud.Read(file)) : new FileStream(file, FileMode.Open))
				{
					using (BinaryReader binaryReader = new BinaryReader(stream))
					{
						int num = binaryReader.ReadInt32();
						if (num > 318)
						{
							result = WorldFileData.FromInvalidWorld(file, cloudSave, StatusID.LaterVersion, new FileFormatException("File is from a later version of Terraria. '" + file + "'"));
						}
						else
						{
							WorldFileData worldFileData = new WorldFileData(file, cloudSave);
							if (num >= 135)
							{
								worldFileData.Metadata = FileMetadata.Read(binaryReader, FileType.World);
							}
							else
							{
								worldFileData.Metadata = FileMetadata.FromCurrentSettings(FileType.World);
							}
							if (num >= 88)
							{
								binaryReader.ReadInt16();
								stream.Position = (long)binaryReader.ReadInt32();
							}
							worldFileData.Name = binaryReader.ReadString();
							if (num >= 179)
							{
								string seed;
								if (num == 179)
								{
									seed = binaryReader.ReadInt32().ToString();
								}
								else
								{
									seed = binaryReader.ReadString();
								}
								worldFileData.SetSeed(seed);
								worldFileData.WorldGeneratorVersion = binaryReader.ReadUInt64();
							}
							else
							{
								worldFileData.SetSeedToEmpty();
								worldFileData.WorldGeneratorVersion = 0UL;
							}
							if (num >= 181)
							{
								worldFileData.UniqueId = new Guid(binaryReader.ReadBytes(16));
							}
							else
							{
								worldFileData.UniqueId = Guid.Empty;
							}
							worldFileData.WorldId = binaryReader.ReadInt32();
							binaryReader.ReadInt32();
							binaryReader.ReadInt32();
							binaryReader.ReadInt32();
							binaryReader.ReadInt32();
							int y = binaryReader.ReadInt32();
							int x = binaryReader.ReadInt32();
							worldFileData.SetWorldSize(x, y);
							if (num >= 209)
							{
								worldFileData.GameMode = binaryReader.ReadInt32();
								if (num >= 222)
								{
									worldFileData.DrunkWorld = binaryReader.ReadBoolean();
									if (num >= 227)
									{
										worldFileData.ForTheWorthy = binaryReader.ReadBoolean();
									}
									if (num >= 238)
									{
										worldFileData.Anniversary = binaryReader.ReadBoolean();
									}
									if (num >= 239)
									{
										worldFileData.DontStarve = binaryReader.ReadBoolean();
									}
									if (num >= 241)
									{
										worldFileData.NotTheBees = binaryReader.ReadBoolean();
									}
									if (num >= 249)
									{
										worldFileData.RemixWorld = binaryReader.ReadBoolean();
									}
									if (num >= 266)
									{
										worldFileData.NoTrapsWorld = binaryReader.ReadBoolean();
									}
									if (num >= 267)
									{
										worldFileData.ZenithWorld = binaryReader.ReadBoolean();
									}
									else
									{
										worldFileData.ZenithWorld = (worldFileData.DrunkWorld && worldFileData.RemixWorld);
										if (worldFileData.ZenithWorld)
										{
											worldFileData.NoTrapsWorld = true;
										}
									}
									if (num >= 302)
									{
										worldFileData.SkyblockWorld = binaryReader.ReadBoolean();
									}
								}
							}
							else if (num >= 112)
							{
								if (binaryReader.ReadBoolean())
								{
									worldFileData.GameMode = 1;
								}
								else
								{
									worldFileData.GameMode = 0;
								}
							}
							if (num >= 141)
							{
								worldFileData.CreationTime = DateTime.FromBinary(binaryReader.ReadInt64());
							}
							else if (!cloudSave)
							{
								worldFileData.CreationTime = File.GetCreationTime(file);
							}
							else
							{
								worldFileData.CreationTime = DateTime.Now;
							}
							if (num >= 284)
							{
								worldFileData.LastPlayed = DateTime.FromBinary(binaryReader.ReadInt64());
							}
							else if (!cloudSave)
							{
								worldFileData.LastPlayed = File.GetLastWriteTimeUtc(file);
							}
							else
							{
								worldFileData.LastPlayed = default(DateTime);
							}
							if (num >= 63)
							{
								binaryReader.ReadByte();
							}
							if (num >= 44)
							{
								binaryReader.ReadInt32();
								binaryReader.ReadInt32();
								binaryReader.ReadInt32();
								binaryReader.ReadInt32();
								binaryReader.ReadInt32();
								binaryReader.ReadInt32();
								binaryReader.ReadInt32();
							}
							if (num >= 60)
							{
								binaryReader.ReadInt32();
								binaryReader.ReadInt32();
								binaryReader.ReadInt32();
								binaryReader.ReadInt32();
								binaryReader.ReadInt32();
								binaryReader.ReadInt32();
								binaryReader.ReadInt32();
								binaryReader.ReadInt32();
							}
							if (num >= 61)
							{
								binaryReader.ReadInt32();
								binaryReader.ReadInt32();
							}
							binaryReader.ReadInt32();
							binaryReader.ReadInt32();
							binaryReader.ReadDouble();
							binaryReader.ReadDouble();
							binaryReader.ReadDouble();
							binaryReader.ReadBoolean();
							binaryReader.ReadInt32();
							binaryReader.ReadBoolean();
							if (num >= 70)
							{
								binaryReader.ReadBoolean();
							}
							binaryReader.ReadInt32();
							binaryReader.ReadInt32();
							worldFileData.HasCrimson = (num >= 56 && binaryReader.ReadBoolean());
							binaryReader.ReadBoolean();
							binaryReader.ReadBoolean();
							binaryReader.ReadBoolean();
							if (num >= 66)
							{
								binaryReader.ReadBoolean();
							}
							if (num >= 44)
							{
								binaryReader.ReadBoolean();
							}
							if (num >= 44)
							{
								binaryReader.ReadBoolean();
							}
							if (num >= 44)
							{
								binaryReader.ReadBoolean();
							}
							if (num >= 44)
							{
								binaryReader.ReadBoolean();
							}
							if (num >= 64)
							{
								binaryReader.ReadBoolean();
							}
							if (num >= 64)
							{
								binaryReader.ReadBoolean();
							}
							if (num >= 118)
							{
								binaryReader.ReadBoolean();
							}
							if (num >= 29)
							{
								binaryReader.ReadBoolean();
							}
							if (num >= 29)
							{
								binaryReader.ReadBoolean();
							}
							if (num >= 34)
							{
								binaryReader.ReadBoolean();
							}
							if (num >= 29)
							{
								binaryReader.ReadBoolean();
							}
							if (num >= 32)
							{
								binaryReader.ReadBoolean();
							}
							if (num >= 37)
							{
								binaryReader.ReadBoolean();
							}
							if (num >= 56)
							{
								binaryReader.ReadBoolean();
							}
							binaryReader.ReadBoolean();
							binaryReader.ReadBoolean();
							binaryReader.ReadByte();
							if (num >= 23)
							{
								binaryReader.ReadInt32();
							}
							worldFileData.IsHardMode = (num >= 23 && binaryReader.ReadBoolean());
							if (num >= 257)
							{
								binaryReader.ReadBoolean();
							}
							binaryReader.ReadInt32();
							binaryReader.ReadInt32();
							binaryReader.ReadInt32();
							binaryReader.ReadDouble();
							if (num >= 118)
							{
								binaryReader.ReadDouble();
							}
							if (num >= 113)
							{
								binaryReader.ReadByte();
							}
							if (num >= 53)
							{
								binaryReader.ReadBoolean();
								binaryReader.ReadInt32();
								binaryReader.ReadSingle();
							}
							if (num >= 54)
							{
								binaryReader.ReadInt32();
								binaryReader.ReadInt32();
								binaryReader.ReadInt32();
							}
							if (num >= 55)
							{
								binaryReader.ReadByte();
								binaryReader.ReadByte();
								binaryReader.ReadByte();
							}
							if (num >= 60)
							{
								binaryReader.ReadByte();
								binaryReader.ReadByte();
								binaryReader.ReadByte();
								binaryReader.ReadByte();
								binaryReader.ReadByte();
							}
							if (num >= 60)
							{
								binaryReader.ReadInt32();
							}
							if (num >= 62)
							{
								binaryReader.ReadInt16();
							}
							if (num >= 88)
							{
								binaryReader.ReadSingle();
							}
							if (num < 95)
							{
								result = worldFileData;
							}
							else
							{
								for (int i = binaryReader.ReadInt32(); i > 0; i--)
								{
									binaryReader.ReadString();
								}
								if (num < 99)
								{
									result = worldFileData;
								}
								else
								{
									binaryReader.ReadBoolean();
									if (num < 101)
									{
										result = worldFileData;
									}
									else
									{
										binaryReader.ReadInt32();
										if (num < 104)
										{
											result = worldFileData;
										}
										else
										{
											binaryReader.ReadBoolean();
											if (num >= 129)
											{
												binaryReader.ReadBoolean();
											}
											if (num >= 201)
											{
												binaryReader.ReadBoolean();
											}
											if (num >= 107)
											{
												binaryReader.ReadInt32();
											}
											if (num >= 108)
											{
												binaryReader.ReadInt32();
											}
											if (num < 109)
											{
												result = worldFileData;
											}
											else
											{
												BannerSystem.ValidateWorld(binaryReader, num);
												if (num < 128)
												{
													result = worldFileData;
												}
												else
												{
													binaryReader.ReadBoolean();
													if (num < 131)
													{
														result = worldFileData;
													}
													else
													{
														binaryReader.ReadBoolean();
														binaryReader.ReadBoolean();
														binaryReader.ReadBoolean();
														worldFileData.DefeatedMoonlord = binaryReader.ReadBoolean();
														result = worldFileData;
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			catch (Exception exception)
			{
				result = WorldFileData.FromInvalidWorld(file, cloudSave, StatusID.UnknownError, exception);
			}
			return result;
		}

		// Token: 0x06001513 RID: 5395 RVA: 0x004BD7BC File Offset: 0x004BB9BC
		public static WorldFileData CreateMetadata(string name, bool cloudSave, int GameMode)
		{
			WorldFileData worldFileData = new WorldFileData(Main.GetWorldPathFromName(name, cloudSave), cloudSave);
			if (Main.autoGenFileLocation != null && Main.autoGenFileLocation != "")
			{
				worldFileData = new WorldFileData(Main.autoGenFileLocation, cloudSave);
				Main.autoGenFileLocation = null;
			}
			worldFileData.Name = name;
			worldFileData.GameMode = GameMode;
			worldFileData.CreationTime = DateTime.Now;
			worldFileData.Metadata = FileMetadata.FromCurrentSettings(FileType.World);
			worldFileData.SetFavorite(false, true);
			worldFileData.WorldGeneratorVersion = 1365799600129UL;
			worldFileData.UniqueId = Guid.NewGuid();
			if (Main.DefaultSeed == "")
			{
				worldFileData.SetSeedToRandom();
			}
			else
			{
				worldFileData.SetSeed(Main.DefaultSeed);
			}
			return worldFileData;
		}

		// Token: 0x06001514 RID: 5396 RVA: 0x004BD86D File Offset: 0x004BBA6D
		public static void ResetTemps()
		{
			WorldFile._tempRaining = false;
			WorldFile._tempMaxRain = 0f;
			WorldFile._tempRainTime = 0;
			WorldFile._tempDayTime = true;
			WorldFile._tempBloodMoon = false;
			WorldFile._tempEclipse = false;
			WorldFile._tempMoonPhase = 0;
			Main.anglerWhoFinishedToday.Clear();
			Main.anglerQuestFinished = false;
		}

		// Token: 0x06001515 RID: 5397 RVA: 0x004BD8B0 File Offset: 0x004BBAB0
		public static void ClearTempTiles()
		{
			for (int i = 0; i < Main.maxTilesX; i++)
			{
				for (int j = 0; j < Main.maxTilesY; j++)
				{
					if (Main.tile[i, j].type == 127 || Main.tile[i, j].type == 504)
					{
						WorldGen.KillTile(i, j, false, false, false);
					}
				}
			}
		}

		// Token: 0x06001516 RID: 5398 RVA: 0x004BD914 File Offset: 0x004BBB14
		public static void LoadWorld()
		{
			if (Main.rand == null)
			{
				Main.rand = new UnifiedRandom((int)DateTime.Now.Ticks);
			}
			Main.lockMenuBGChange = true;
			WorldFile._isWorldOnCloud = Main.ActiveWorldFileData.IsCloudSave;
			Main.checkXMas();
			Main.checkHalloween();
			if (WorldFile._isWorldOnCloud && SocialAPI.Cloud == null)
			{
				WorldFile.LastThrownLoadException = new Exception("Cloud inaccessible");
				WorldGen.loadFailed = true;
				return;
			}
			if (!FileUtilities.Exists(Main.worldPathName, WorldFile._isWorldOnCloud) && Main.autoGen)
			{
				if (!WorldFile._isWorldOnCloud)
				{
					for (int i = Main.worldPathName.Length - 1; i >= 0; i--)
					{
						if (Main.worldPathName.Substring(i, 1) == (Path.DirectorySeparatorChar.ToString() ?? ""))
						{
							Utils.TryCreatingDirectory(Main.worldPathName.Substring(0, i));
							break;
						}
					}
				}
				Main.ActiveWorldFileData = WorldFile.CreateMetadata((Main.worldName == "") ? "World" : Main.worldName, WorldFile._isWorldOnCloud, Main.GameMode);
				string text = (Main.AutogenSeedName ?? "").Trim();
				string text2;
				string seed;
				List<string> list;
				if (WorldFileData.TryApplyingCopiedSeed(text, true, out text2, out seed, out list))
				{
					Main.ActiveWorldFileData.SetSeed(seed);
				}
				else
				{
					text = Utils.TrimUserString(text, WorldFileData.MAX_USER_SEED_TEXT_LENGTH);
					AWorldGenerationOption optionFromSeedText = WorldGenerationOptions.GetOptionFromSeedText(text);
					WorldGenerationOptions.SelectOption(optionFromSeedText);
					foreach (AWorldGenerationOption aworldGenerationOption in WorldGenerationOptions.Options)
					{
						if (aworldGenerationOption.AutoGenEnabled)
						{
							aworldGenerationOption.Enabled = true;
						}
					}
					if (optionFromSeedText != null || string.IsNullOrWhiteSpace(text))
					{
						Main.ActiveWorldFileData.SetSeedToRandomWithCurrentEvents();
					}
					else
					{
						Main.ActiveWorldFileData.SetSeed(text);
					}
				}
				if (WorldGen.GenerateWorld(Main.AutogenProgress, null))
				{
					WorldFile.SaveWorld(Main.ActiveWorldFileData.IsCloudSave, true);
				}
			}
			try
			{
				using (MemoryStream memoryStream = new MemoryStream(FileUtilities.ReadAllBytes(Main.worldPathName, WorldFile._isWorldOnCloud)))
				{
					using (BinaryReader binaryReader = new BinaryReader(memoryStream))
					{
						try
						{
							WorldGen.loadFailed = false;
							int num = binaryReader.ReadInt32();
							WorldFile._versionNumber = num;
							int num2;
							if (num <= 0)
							{
								num2 = StatusID.UnknownError;
							}
							else if (num > 318)
							{
								num2 = StatusID.LaterVersion;
							}
							else if (num <= 87)
							{
								num2 = WorldFile.LoadWorld_Version1_Old_BeforeRelease88(binaryReader);
							}
							else
							{
								num2 = WorldFile.LoadWorld_Version2(binaryReader);
							}
							if (num < 141)
							{
								if (!WorldFile._isWorldOnCloud)
								{
									Main.ActiveWorldFileData.CreationTime = File.GetCreationTime(Main.worldPathName);
								}
								else
								{
									Main.ActiveWorldFileData.CreationTime = DateTime.Now;
								}
							}
							binaryReader.Close();
							memoryStream.Close();
							if (num2 != StatusID.Ok)
							{
								throw new Exception("LoadWorld failed with status: " + StatusID.Search.GetName(num2));
							}
							WorldFile.CheckSavedOreTiers();
							WorldFile.ConvertOldTileEntities();
							WorldFile.ClearTempTiles();
							WorldGen.isGeneratingOrLoadingWorld = true;
							GenVars.waterLine = Main.maxTilesY;
							Liquid.QuickWater(2, -1, -1);
							WorldGen.WaterCheck();
							int num3 = 0;
							Liquid.quickSettle = true;
							int num4 = Liquid.numLiquid + LiquidBuffer.numLiquidBuffer;
							float num5 = 0f;
							while (Liquid.numLiquid > 0 && num3 < 100000)
							{
								num3++;
								float num6 = (float)(num4 - (Liquid.numLiquid + LiquidBuffer.numLiquidBuffer)) / (float)num4;
								if (Liquid.numLiquid + LiquidBuffer.numLiquidBuffer > num4)
								{
									num4 = Liquid.numLiquid + LiquidBuffer.numLiquidBuffer;
								}
								if (num6 > num5)
								{
									num5 = num6;
								}
								else
								{
									num6 = num5;
								}
								Main.statusText = string.Concat(new object[]
								{
									Lang.gen[27].Value,
									" ",
									(int)(num6 * 100f / 2f + 50f),
									"%"
								});
								Liquid.UpdateLiquid();
							}
							Liquid.quickSettle = false;
							Main.weatherCounter = WorldGen.genRand.Next(3600, 18000);
							Cloud.resetClouds();
							WorldGen.WaterCheck();
							WorldGen.isGeneratingOrLoadingWorld = false;
							NPC.setFireFlyChance();
							WorldGen.Skyblock.ScanTiles();
							if (Main.slimeRainTime > 0.0)
							{
								Main.StartSlimeRain(false);
							}
							NPC.SetWorldSpecificMonstersByWorldID();
						}
						catch (Exception lastThrownLoadException)
						{
							WorldFile.LastThrownLoadException = lastThrownLoadException;
							WorldGen.loadFailed = true;
							try
							{
								binaryReader.Close();
								memoryStream.Close();
							}
							catch
							{
							}
						}
					}
				}
			}
			catch (Exception lastThrownLoadException2)
			{
				WorldFile.LastThrownLoadException = lastThrownLoadException2;
				WorldGen.loadFailed = true;
			}
		}

		// Token: 0x06001517 RID: 5399 RVA: 0x004BDE0C File Offset: 0x004BC00C
		public static void CheckSavedOreTiers()
		{
			if (WorldGen.SavedOreTiers.Copper == -1 || WorldGen.SavedOreTiers.Iron == -1 || WorldGen.SavedOreTiers.Silver == -1 || WorldGen.SavedOreTiers.Gold == -1)
			{
				int[] array = WorldGen.CountTileTypesInWorld(new int[]
				{
					7,
					166,
					6,
					167,
					9,
					168,
					8,
					169
				});
				for (int i = 0; i < array.Length; i += 2)
				{
					int num = array[i];
					int num2 = array[i + 1];
					switch (i)
					{
					case 0:
						if (num > num2)
						{
							WorldGen.SavedOreTiers.Copper = 7;
						}
						else
						{
							WorldGen.SavedOreTiers.Copper = 166;
						}
						break;
					case 2:
						if (num > num2)
						{
							WorldGen.SavedOreTiers.Iron = 6;
						}
						else
						{
							WorldGen.SavedOreTiers.Iron = 167;
						}
						break;
					case 4:
						if (num > num2)
						{
							WorldGen.SavedOreTiers.Silver = 9;
						}
						else
						{
							WorldGen.SavedOreTiers.Silver = 168;
						}
						break;
					case 6:
						if (num > num2)
						{
							WorldGen.SavedOreTiers.Gold = 8;
						}
						else
						{
							WorldGen.SavedOreTiers.Gold = 169;
						}
						break;
					}
				}
			}
		}

		// Token: 0x06001518 RID: 5400 RVA: 0x004BDEF4 File Offset: 0x004BC0F4
		public static void SaveWorld()
		{
			try
			{
				WorldFile.SaveWorld(WorldFile._isWorldOnCloud, false);
			}
			catch (Exception exception)
			{
				FancyErrorPrinter.ShowFileSavingFailError(exception, Main.WorldPath);
				throw;
			}
		}

		// Token: 0x06001519 RID: 5401 RVA: 0x004BDF2C File Offset: 0x004BC12C
		public static void SaveWorld(bool useCloudSaving, bool resetTime = false)
		{
			if (useCloudSaving && SocialAPI.Cloud == null)
			{
				return;
			}
			if (Main.worldName == "")
			{
				Main.worldName = "World";
			}
			while (WorldGen.TransformingWorld)
			{
				Main.statusText = Lang.gen[48].Value;
			}
			if (Monitor.TryEnter(WorldFile.IOLock))
			{
				try
				{
					FileUtilities.ProtectedInvoke(delegate
					{
						WorldFile.InternalSaveWorld(useCloudSaving, resetTime);
					});
					return;
				}
				finally
				{
					Monitor.Exit(WorldFile.IOLock);
				}
				return;
			}
		}

		// Token: 0x0600151A RID: 5402 RVA: 0x004BDFD0 File Offset: 0x004BC1D0
		private static void InternalSaveWorld(bool useCloudSaving, bool resetTime)
		{
			Utils.TryCreatingDirectory(Main.WorldPath);
			if (WorldFile._reuseTempsForNextSave)
			{
				WorldFile._reuseTempsForNextSave = false;
			}
			else
			{
				WorldFile.SetTempToOngoing();
			}
			if (resetTime)
			{
				WorldFile.ResetTempsToDayTime();
			}
			if (Main.worldPathName == null)
			{
				return;
			}
			new Stopwatch().Start();
			byte[] array;
			int num;
			using (MemoryStream memoryStream = new MemoryStream(7000000))
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					WorldFile.SaveWorld_Version2(binaryWriter);
				}
				array = memoryStream.ToArray();
				num = array.Length;
			}
			byte[] array2 = null;
			if (FileUtilities.Exists(Main.worldPathName, useCloudSaving))
			{
				array2 = FileUtilities.ReadAllBytes(Main.worldPathName, useCloudSaving);
			}
			FileUtilities.Write(Main.worldPathName, array, num, useCloudSaving);
			array = FileUtilities.ReadAllBytes(Main.worldPathName, useCloudSaving);
			string text = null;
			using (MemoryStream memoryStream2 = new MemoryStream(array, 0, num, false))
			{
				using (BinaryReader binaryReader = new BinaryReader(memoryStream2))
				{
					if (!Main.validateSaves || WorldFile.ValidateWorld(binaryReader))
					{
						if (array2 != null)
						{
							text = Main.worldPathName + ".bak";
							Main.statusText = Lang.gen[50].Value;
						}
						WorldFile.DoRollingBackups(text);
					}
					else
					{
						text = Main.worldPathName;
					}
				}
			}
			if (text != null && array2 != null)
			{
				FileUtilities.WriteAllBytes(text, array2, useCloudSaving);
			}
		}

		// Token: 0x0600151B RID: 5403 RVA: 0x004BE148 File Offset: 0x004BC348
		private static void DoRollingBackups(string backupWorldWritePath)
		{
			if (Main.WorldRollingBackupsCountToKeep <= 1)
			{
				return;
			}
			int num = Main.WorldRollingBackupsCountToKeep;
			if (num > 9)
			{
				num = 9;
			}
			int num2 = 1;
			for (int i = 1; i < num; i++)
			{
				string path = backupWorldWritePath + i;
				if (i == 1)
				{
					path = backupWorldWritePath;
				}
				if (!FileUtilities.Exists(path, false))
				{
					break;
				}
				num2 = i + 1;
			}
			for (int j = num2 - 1; j > 0; j--)
			{
				string text = backupWorldWritePath + j;
				if (j == 1)
				{
					text = backupWorldWritePath;
				}
				string destination = backupWorldWritePath + (j + 1);
				if (FileUtilities.Exists(text, false))
				{
					FileUtilities.Move(text, destination, false);
				}
			}
		}

		// Token: 0x0600151C RID: 5404 RVA: 0x004BE1E8 File Offset: 0x004BC3E8
		private static void ResetTempsToDayTime()
		{
			WorldFile._tempDayTime = true;
			WorldFile._tempTime = 13500.0;
			WorldFile._tempMoonPhase = 0;
			WorldFile._tempBloodMoon = false;
			WorldFile._tempEclipse = false;
			WorldFile._tempCultistDelay = 86400;
			if (WorldGen.SecretSeed.graveyardBloodmoonStart.Enabled)
			{
				WorldFile._tempDayTime = false;
				WorldFile._tempBloodMoon = true;
				WorldFile._tempTime = 1.0;
			}
			WorldFile._tempPartyManual = false;
			WorldFile._tempPartyGenuine = false;
			if (Main.tenthAnniversaryWorld && !Main.skyblockWorld)
			{
				WorldFile._tempPartyGenuine = true;
			}
			WorldFile._tempPartyCooldown = 0;
			WorldFile.TempPartyCelebratingNPCs.Clear();
			WorldFile._tempSandstormHappening = false;
			WorldFile._tempSandstormTimeLeft = 0;
			WorldFile._tempSandstormSeverity = 0f;
			WorldFile._tempSandstormIntendedSeverity = 0f;
			WorldFile._tempLanternNightCooldown = 0;
			WorldFile._tempLanternNightGenuine = false;
			WorldFile._tempLanternNightManual = false;
			WorldFile._tempLanternNightNextNightIsGenuine = false;
			WorldFile._tempMeteorShowerCount = 0;
			WorldFile._tempCoinRain = 0;
		}

		// Token: 0x0600151D RID: 5405 RVA: 0x004BE2C0 File Offset: 0x004BC4C0
		public static void SetTempToOngoing()
		{
			WorldFile._tempDayTime = Main.dayTime;
			WorldFile._tempTime = Main.time;
			WorldFile._tempMoonPhase = Main.moonPhase;
			WorldFile._tempBloodMoon = Main.bloodMoon;
			WorldFile._tempEclipse = Main.eclipse;
			WorldFile._tempCultistDelay = CultistRitual.delay;
			WorldFile._tempPartyManual = BirthdayParty.ManualParty;
			WorldFile._tempPartyGenuine = BirthdayParty.GenuineParty;
			WorldFile._tempPartyCooldown = BirthdayParty.PartyDaysOnCooldown;
			WorldFile.TempPartyCelebratingNPCs.Clear();
			WorldFile.TempPartyCelebratingNPCs.AddRange(BirthdayParty.CelebratingNPCs);
			WorldFile._tempSandstormHappening = Sandstorm.Happening;
			WorldFile._tempSandstormTimeLeft = Sandstorm.TimeLeft;
			WorldFile._tempSandstormSeverity = Sandstorm.Severity;
			WorldFile._tempSandstormIntendedSeverity = Sandstorm.IntendedSeverity;
			WorldFile._tempRaining = Main.raining;
			WorldFile._tempRainTime = Main.rainTime;
			WorldFile._tempMaxRain = Main.maxRaining;
			WorldFile._tempLanternNightCooldown = LanternNight.LanternNightsOnCooldown;
			WorldFile._tempLanternNightGenuine = LanternNight.GenuineLanterns;
			WorldFile._tempLanternNightManual = LanternNight.ManualLanterns;
			WorldFile._tempLanternNightNextNightIsGenuine = LanternNight.NextNightIsLanternNight;
			WorldFile._tempMeteorShowerCount = WorldGen.meteorShowerCount;
			WorldFile._tempCoinRain = Main.coinRain;
		}

		// Token: 0x0600151E RID: 5406 RVA: 0x004BE3C4 File Offset: 0x004BC5C4
		private static void ConvertOldTileEntities()
		{
			List<Point> list = new List<Point>();
			List<Point> list2 = new List<Point>();
			for (int i = 0; i < Main.maxTilesX; i++)
			{
				for (int j = 0; j < Main.maxTilesY; j++)
				{
					Tile tile = Main.tile[i, j];
					if ((tile.type == 128 || tile.type == 269) && tile.frameY == 0 && (tile.frameX % 100 == 0 || tile.frameX % 100 == 36))
					{
						list.Add(new Point(i, j));
					}
					if (tile.type == 334 && tile.frameY == 0 && tile.frameX % 54 == 0)
					{
						list2.Add(new Point(i, j));
					}
					if (tile.type == 49 && (tile.frameX == -1 || tile.frameY == -1))
					{
						tile.frameX = 0;
						tile.frameY = 0;
					}
				}
			}
			foreach (Point point in list)
			{
				if (WorldGen.InWorld(point.X, point.Y, 5))
				{
					int frameX = (int)Main.tile[point.X, point.Y].frameX;
					int frameX2 = (int)Main.tile[point.X, point.Y + 1].frameX;
					int frameX3 = (int)Main.tile[point.X, point.Y + 2].frameX;
					for (int k = 0; k < 2; k++)
					{
						for (int l = 0; l < 3; l++)
						{
							Tile tile2 = Main.tile[point.X + k, point.Y + l];
							Tile tile3 = tile2;
							tile3.frameX %= 100;
							if (tile2.type == 269)
							{
								Tile tile4 = tile2;
								tile4.frameX += 72;
							}
							tile2.type = 470;
						}
					}
					int num = TileEntityType<TEDisplayDoll>.Place(point.X, point.Y);
					if (num != -1)
					{
						TEDisplayDoll tedisplayDoll;
						TileEntity.TryGet<TEDisplayDoll>(num, out tedisplayDoll);
						tedisplayDoll.SetInventoryFromMannequin(frameX, frameX2, frameX3);
					}
				}
			}
			foreach (Point point2 in list2)
			{
				if (WorldGen.InWorld(point2.X, point2.Y, 5))
				{
					bool flag = Main.tile[point2.X, point2.Y].frameX >= 54;
					short frameX4 = Main.tile[point2.X, point2.Y + 1].frameX;
					int frameX5 = (int)Main.tile[point2.X + 1, point2.Y + 1].frameX;
					bool flag2 = frameX4 >= 5000;
					int num2 = (int)(frameX4 % 5000);
					num2 -= 100;
					int prefix = frameX5 - ((frameX5 >= 25000) ? 25000 : 10000);
					for (int m = 0; m < 3; m++)
					{
						for (int n = 0; n < 3; n++)
						{
							Tile tile5 = Main.tile[point2.X + m, point2.Y + n];
							tile5.type = 471;
							tile5.frameX = (short)((flag ? 54 : 0) + m * 18);
							tile5.frameY = (short)(n * 18);
						}
					}
					if (TileEntityType<TEWeaponsRack>.Place(point2.X, point2.Y) != -1 && flag2)
					{
						TEWeaponsRack.TryPlacing(point2.X, point2.Y, num2, prefix, 1);
					}
				}
			}
		}

		// Token: 0x0600151F RID: 5407 RVA: 0x004BE7D8 File Offset: 0x004BC9D8
		public static void SaveWorld_Version2(BinaryWriter writer)
		{
			int[] pointers = new int[]
			{
				WorldFile.SaveFileFormatHeader(writer),
				WorldFile.SaveWorldHeader(writer),
				WorldFile.SaveWorldTiles(writer),
				WorldFile.SaveChests(writer),
				WorldFile.SaveSigns(writer),
				WorldFile.SaveNPCs(writer),
				WorldFile.SaveTileEntities(writer),
				WorldFile.SaveWeightedPressurePlates(writer),
				WorldFile.SaveTownManager(writer),
				WorldFile.SaveBestiary(writer),
				WorldFile.SaveCreativePowers(writer)
			};
			WorldFile.SaveFooter(writer);
			WorldFile.SaveHeaderPointers(writer, pointers);
		}

		// Token: 0x06001520 RID: 5408 RVA: 0x004BE864 File Offset: 0x004BCA64
		public static int SaveFileFormatHeader(BinaryWriter writer)
		{
			ushort count = TileID.Count;
			short num = 11;
			writer.Write(318);
			Main.WorldFileMetadata.IncrementAndWrite(writer);
			writer.Write(num);
			for (int i = 0; i < (int)num; i++)
			{
				writer.Write(0);
			}
			writer.Write(count);
			byte b = 0;
			byte b2 = 1;
			for (int i = 0; i < (int)count; i++)
			{
				if (Main.tileFrameImportant[i])
				{
					b |= b2;
				}
				if (b2 == 128)
				{
					writer.Write(b);
					b = 0;
					b2 = 1;
				}
				else
				{
					b2 = (byte)(b2 << 1);
				}
			}
			if (b2 != 1)
			{
				writer.Write(b);
			}
			return (int)writer.BaseStream.Position;
		}

		// Token: 0x06001521 RID: 5409 RVA: 0x004BE908 File Offset: 0x004BCB08
		public static int SaveHeaderPointers(BinaryWriter writer, int[] pointers)
		{
			writer.BaseStream.Position = 0L;
			writer.Write(318);
			writer.BaseStream.Position += 20L;
			writer.Write((short)pointers.Length);
			for (int i = 0; i < pointers.Length; i++)
			{
				writer.Write(pointers[i]);
			}
			return (int)writer.BaseStream.Position;
		}

		// Token: 0x06001522 RID: 5410 RVA: 0x004BE970 File Offset: 0x004BCB70
		public static int SaveWorldHeader(BinaryWriter writer)
		{
			writer.Write(Main.worldName);
			writer.Write(Main.ActiveWorldFileData.SeedText);
			writer.Write(Main.ActiveWorldFileData.WorldGeneratorVersion);
			writer.Write(Main.ActiveWorldFileData.UniqueId.ToByteArray());
			writer.Write(Main.ActiveWorldFileData.WorldId);
			writer.Write((int)Main.leftWorld);
			writer.Write((int)Main.rightWorld);
			writer.Write((int)Main.topWorld);
			writer.Write((int)Main.bottomWorld);
			writer.Write(Main.maxTilesY);
			writer.Write(Main.maxTilesX);
			writer.Write(Main.GameMode);
			writer.Write(Main.drunkWorld);
			writer.Write(Main.getGoodWorld);
			writer.Write(Main.tenthAnniversaryWorld);
			writer.Write(Main.dontStarveWorld);
			writer.Write(Main.notTheBeesWorld);
			writer.Write(Main.remixWorld);
			writer.Write(Main.noTrapsWorld);
			writer.Write(Main.zenithWorld);
			writer.Write(Main.skyblockWorld);
			writer.Write(Main.ActiveWorldFileData.CreationTime.ToBinary());
			writer.Write(DateTime.UtcNow.ToBinary());
			writer.Write((byte)Main.moonType);
			writer.Write(Main.treeX[0]);
			writer.Write(Main.treeX[1]);
			writer.Write(Main.treeX[2]);
			writer.Write(Main.treeStyle[0]);
			writer.Write(Main.treeStyle[1]);
			writer.Write(Main.treeStyle[2]);
			writer.Write(Main.treeStyle[3]);
			writer.Write(Main.caveBackX[0]);
			writer.Write(Main.caveBackX[1]);
			writer.Write(Main.caveBackX[2]);
			writer.Write(Main.caveBackStyle[0]);
			writer.Write(Main.caveBackStyle[1]);
			writer.Write(Main.caveBackStyle[2]);
			writer.Write(Main.caveBackStyle[3]);
			writer.Write(Main.iceBackStyle);
			writer.Write(Main.jungleBackStyle);
			writer.Write(Main.hellBackStyle);
			writer.Write(Main.spawnTileX);
			writer.Write(Main.spawnTileY);
			writer.Write(Main.worldSurface);
			writer.Write(Main.rockLayer);
			writer.Write(WorldFile._tempTime);
			writer.Write(WorldFile._tempDayTime);
			writer.Write(WorldFile._tempMoonPhase);
			writer.Write(WorldFile._tempBloodMoon);
			writer.Write(WorldFile._tempEclipse);
			writer.Write(Main.dungeonX);
			writer.Write(Main.dungeonY);
			writer.Write(WorldGen.crimson);
			writer.Write(NPC.downedBoss1);
			writer.Write(NPC.downedBoss2);
			writer.Write(NPC.downedBoss3);
			writer.Write(NPC.downedQueenBee);
			writer.Write(NPC.downedMechBoss1);
			writer.Write(NPC.downedMechBoss2);
			writer.Write(NPC.downedMechBoss3);
			writer.Write(NPC.downedMechBossAny);
			writer.Write(NPC.downedPlantBoss);
			writer.Write(NPC.downedGolemBoss);
			writer.Write(NPC.downedSlimeKing);
			writer.Write(NPC.savedGoblin);
			writer.Write(NPC.savedWizard);
			writer.Write(NPC.savedMech);
			writer.Write(NPC.downedGoblins);
			writer.Write(NPC.downedClown);
			writer.Write(NPC.downedFrost);
			writer.Write(NPC.downedPirates);
			writer.Write(WorldGen.shadowOrbSmashed);
			writer.Write(WorldGen.spawnMeteor);
			writer.Write((byte)WorldGen.shadowOrbCount);
			writer.Write(WorldGen.altarCount);
			writer.Write(Main.hardMode);
			writer.Write(Main.afterPartyOfDoom);
			writer.Write(Main.invasionDelay);
			writer.Write(Main.invasionSize);
			writer.Write(Main.invasionType);
			writer.Write(Main.invasionX);
			writer.Write(Main.slimeRainTime);
			writer.Write((byte)Main.sundialCooldown);
			writer.Write(WorldFile._tempRaining);
			writer.Write(WorldFile._tempRainTime);
			writer.Write(WorldFile._tempMaxRain);
			writer.Write(WorldGen.SavedOreTiers.Cobalt);
			writer.Write(WorldGen.SavedOreTiers.Mythril);
			writer.Write(WorldGen.SavedOreTiers.Adamantite);
			writer.Write((byte)WorldGen.treeBG1);
			writer.Write((byte)WorldGen.corruptBG);
			writer.Write((byte)WorldGen.jungleBG);
			writer.Write((byte)WorldGen.snowBG);
			writer.Write((byte)WorldGen.hallowBG);
			writer.Write((byte)WorldGen.crimsonBG);
			writer.Write((byte)WorldGen.desertBG);
			writer.Write((byte)WorldGen.oceanBG);
			writer.Write((int)Main.cloudBGActive);
			writer.Write((short)Main.numClouds);
			writer.Write(Main.windSpeedTarget);
			writer.Write(Main.anglerWhoFinishedToday.Count);
			for (int i = 0; i < Main.anglerWhoFinishedToday.Count; i++)
			{
				writer.Write(Main.anglerWhoFinishedToday[i]);
			}
			writer.Write(NPC.savedAngler);
			writer.Write(Main.anglerQuest);
			writer.Write(NPC.savedStylist);
			writer.Write(NPC.savedTaxCollector);
			writer.Write(NPC.savedGolfer);
			writer.Write(Main.invasionSizeStart);
			writer.Write(WorldFile._tempCultistDelay);
			BannerSystem.Save(writer);
			writer.Write(Main.fastForwardTimeToDawn);
			writer.Write(NPC.downedFishron);
			writer.Write(NPC.downedMartians);
			writer.Write(NPC.downedAncientCultist);
			writer.Write(NPC.downedMoonlord);
			writer.Write(NPC.downedHalloweenKing);
			writer.Write(NPC.downedHalloweenTree);
			writer.Write(NPC.downedChristmasIceQueen);
			writer.Write(NPC.downedChristmasSantank);
			writer.Write(NPC.downedChristmasTree);
			writer.Write(NPC.downedTowerSolar);
			writer.Write(NPC.downedTowerVortex);
			writer.Write(NPC.downedTowerNebula);
			writer.Write(NPC.downedTowerStardust);
			writer.Write(NPC.TowerActiveSolar);
			writer.Write(NPC.TowerActiveVortex);
			writer.Write(NPC.TowerActiveNebula);
			writer.Write(NPC.TowerActiveStardust);
			writer.Write(NPC.LunarApocalypseIsUp);
			writer.Write(WorldFile._tempPartyManual);
			writer.Write(WorldFile._tempPartyGenuine);
			writer.Write(WorldFile._tempPartyCooldown);
			writer.Write(WorldFile.TempPartyCelebratingNPCs.Count);
			for (int j = 0; j < WorldFile.TempPartyCelebratingNPCs.Count; j++)
			{
				writer.Write(WorldFile.TempPartyCelebratingNPCs[j]);
			}
			writer.Write(WorldFile._tempSandstormHappening);
			writer.Write(WorldFile._tempSandstormTimeLeft);
			writer.Write(WorldFile._tempSandstormSeverity);
			writer.Write(WorldFile._tempSandstormIntendedSeverity);
			writer.Write(NPC.savedBartender);
			DD2Event.Save(writer);
			writer.Write((byte)WorldGen.mushroomBG);
			writer.Write((byte)WorldGen.underworldBG);
			writer.Write((byte)WorldGen.treeBG2);
			writer.Write((byte)WorldGen.treeBG3);
			writer.Write((byte)WorldGen.treeBG4);
			writer.Write(NPC.combatBookWasUsed);
			writer.Write(WorldFile._tempLanternNightCooldown);
			writer.Write(WorldFile._tempLanternNightGenuine);
			writer.Write(WorldFile._tempLanternNightManual);
			writer.Write(WorldFile._tempLanternNightNextNightIsGenuine);
			WorldGen.TreeTops.Save(writer);
			writer.Write(Main.forceHalloweenForToday);
			writer.Write(Main.forceXMasForToday);
			writer.Write(WorldGen.SavedOreTiers.Copper);
			writer.Write(WorldGen.SavedOreTiers.Iron);
			writer.Write(WorldGen.SavedOreTiers.Silver);
			writer.Write(WorldGen.SavedOreTiers.Gold);
			writer.Write(NPC.boughtCat);
			writer.Write(NPC.boughtDog);
			writer.Write(NPC.boughtBunny);
			writer.Write(NPC.downedEmpressOfLight);
			writer.Write(NPC.downedQueenSlime);
			writer.Write(NPC.downedDeerclops);
			writer.Write(NPC.unlockedSlimeBlueSpawn);
			writer.Write(NPC.unlockedMerchantSpawn);
			writer.Write(NPC.unlockedDemolitionistSpawn);
			writer.Write(NPC.unlockedPartyGirlSpawn);
			writer.Write(NPC.unlockedDyeTraderSpawn);
			writer.Write(NPC.unlockedTruffleSpawn);
			writer.Write(NPC.unlockedArmsDealerSpawn);
			writer.Write(NPC.unlockedNurseSpawn);
			writer.Write(NPC.unlockedPrincessSpawn);
			writer.Write(NPC.combatBookVolumeTwoWasUsed);
			writer.Write(NPC.peddlersSatchelWasUsed);
			writer.Write(NPC.unlockedSlimeGreenSpawn);
			writer.Write(NPC.unlockedSlimeOldSpawn);
			writer.Write(NPC.unlockedSlimePurpleSpawn);
			writer.Write(NPC.unlockedSlimeRainbowSpawn);
			writer.Write(NPC.unlockedSlimeRedSpawn);
			writer.Write(NPC.unlockedSlimeYellowSpawn);
			writer.Write(NPC.unlockedSlimeCopperSpawn);
			writer.Write(Main.fastForwardTimeToDusk);
			writer.Write((byte)Main.moondialCooldown);
			writer.Write(Main.forceHalloweenForever);
			writer.Write(Main.forceXMasForever);
			writer.Write(Main.vampireSeed);
			writer.Write(Main.infectedSeed);
			writer.Write(WorldFile._tempMeteorShowerCount);
			writer.Write(WorldFile._tempCoinRain);
			writer.Write(Main.teamBasedSpawnsSeed);
			ExtraSpawnPointManager.Write(writer, false);
			writer.Write(Main.dualDungeonsSeed);
			writer.Write(WorldGen.Manifest.Serialize());
			return (int)writer.BaseStream.Position;
		}

		// Token: 0x06001523 RID: 5411 RVA: 0x004BF26C File Offset: 0x004BD46C
		public static int SaveWorldTiles(BinaryWriter writer)
		{
			byte[] array = new byte[16];
			for (int i = 0; i < Main.maxTilesX; i++)
			{
				float num = (float)i / (float)Main.maxTilesX;
				Main.statusText = string.Concat(new object[]
				{
					Lang.gen[49].Value,
					" ",
					(int)(num * 100f + 1f),
					"%"
				});
				for (int j = 0; j < Main.maxTilesY; j++)
				{
					Tile tile = Main.tile[i, j];
					int num2 = 4;
					byte b4;
					byte b3;
					byte b2;
					byte b = b2 = (b3 = (b4 = 0));
					bool flag = false;
					if (tile.active())
					{
						flag = true;
					}
					if (flag)
					{
						b2 |= 2;
						array[num2] = (byte)tile.type;
						num2++;
						if (tile.type > 255)
						{
							array[num2] = (byte)(tile.type >> 8);
							num2++;
							b2 |= 32;
						}
						if (Main.tileFrameImportant[(int)tile.type])
						{
							array[num2] = (byte)(tile.frameX & 255);
							num2++;
							array[num2] = (byte)(((int)tile.frameX & 65280) >> 8);
							num2++;
							array[num2] = (byte)(tile.frameY & 255);
							num2++;
							array[num2] = (byte)(((int)tile.frameY & 65280) >> 8);
							num2++;
						}
						if (tile.color() != 0)
						{
							b3 |= 8;
							array[num2] = tile.color();
							num2++;
						}
					}
					if (tile.wall != 0)
					{
						b2 |= 4;
						array[num2] = (byte)tile.wall;
						num2++;
						if (tile.wallColor() != 0)
						{
							b3 |= 16;
							array[num2] = tile.wallColor();
							num2++;
						}
					}
					if (tile.liquid != 0)
					{
						if (tile.shimmer())
						{
							b3 |= 128;
							b2 |= 8;
						}
						else if (tile.lava())
						{
							b2 |= 16;
						}
						else if (tile.honey())
						{
							b2 |= 24;
						}
						else
						{
							b2 |= 8;
						}
						array[num2] = tile.liquid;
						num2++;
					}
					if (tile.wire())
					{
						b |= 2;
					}
					if (tile.wire2())
					{
						b |= 4;
					}
					if (tile.wire3())
					{
						b |= 8;
					}
					int num3;
					if (tile.halfBrick())
					{
						num3 = 16;
					}
					else if (tile.slope() != 0)
					{
						num3 = (int)(tile.slope() + 1) << 4;
					}
					else
					{
						num3 = 0;
					}
					b |= (byte)num3;
					if (tile.actuator())
					{
						b3 |= 2;
					}
					if (tile.inActive())
					{
						b3 |= 4;
					}
					if (tile.wire4())
					{
						b3 |= 32;
					}
					if (tile.wall > 255)
					{
						array[num2] = (byte)(tile.wall >> 8);
						num2++;
						b3 |= 64;
					}
					if (tile.invisibleBlock())
					{
						b4 |= 2;
					}
					if (tile.invisibleWall())
					{
						b4 |= 4;
					}
					if (tile.fullbrightBlock())
					{
						b4 |= 8;
					}
					if (tile.fullbrightWall())
					{
						b4 |= 16;
					}
					int num4 = 3;
					if (b4 != 0)
					{
						b3 |= 1;
						array[num4] = b4;
						num4--;
					}
					if (b3 != 0)
					{
						b |= 1;
						array[num4] = b3;
						num4--;
					}
					if (b != 0)
					{
						b2 |= 1;
						array[num4] = b;
						num4--;
					}
					short num5 = 0;
					int num6 = j + 1;
					int num7 = Main.maxTilesY - j - 1;
					while (num7 > 0 && tile.isTheSameAs(Main.tile[i, num6]) && TileID.Sets.AllowsSaveCompressionBatching[(int)tile.type])
					{
						num5 += 1;
						num7--;
						num6++;
					}
					j += (int)num5;
					if (num5 > 0)
					{
						array[num2] = (byte)(num5 & 255);
						num2++;
						if (num5 > 255)
						{
							b2 |= 128;
							array[num2] = (byte)(((int)num5 & 65280) >> 8);
							num2++;
						}
						else
						{
							b2 |= 64;
						}
					}
					array[num4] = b2;
					writer.Write(array, num4, num2 - num4);
				}
			}
			return (int)writer.BaseStream.Position;
		}

		// Token: 0x06001524 RID: 5412 RVA: 0x004BF6A4 File Offset: 0x004BD8A4
		public static int SaveChests(BinaryWriter writer)
		{
			short num = 0;
			for (int i = 0; i < 8000; i++)
			{
				Chest chest = Main.chest[i];
				if (chest != null)
				{
					bool flag = false;
					for (int j = chest.x; j <= chest.x + 1; j++)
					{
						for (int k = chest.y; k <= chest.y + 1; k++)
						{
							if (j < 0 || k < 0 || j >= Main.maxTilesX || k >= Main.maxTilesY)
							{
								flag = true;
								break;
							}
							Tile tile = Main.tile[j, k];
							if (!tile.active() || !Main.tileContainer[(int)tile.type])
							{
								flag = true;
								break;
							}
						}
					}
					if (flag)
					{
						Chest.RemoveChest(i);
					}
					else
					{
						num += 1;
					}
				}
			}
			writer.Write(num);
			for (int i = 0; i < 8000; i++)
			{
				Chest chest = Main.chest[i];
				if (chest != null)
				{
					writer.Write(chest.x);
					writer.Write(chest.y);
					writer.Write(chest.name);
					writer.Write(chest.maxItems);
					for (int l = 0; l < chest.maxItems; l++)
					{
						Item item = chest.item[l];
						if (item == null || ItemID.Sets.ItemsThatShouldNotBeInInventory[item.type])
						{
							writer.Write(0);
						}
						else
						{
							if (item.stack < 0)
							{
								item.stack = 1;
							}
							writer.Write((short)item.stack);
							if (item.stack > 0)
							{
								writer.Write(item.type);
								writer.Write(item.prefix);
							}
						}
					}
				}
			}
			return (int)writer.BaseStream.Position;
		}

		// Token: 0x06001525 RID: 5413 RVA: 0x004BF850 File Offset: 0x004BDA50
		public static int SaveSigns(BinaryWriter writer)
		{
			short num = 0;
			for (int i = 0; i < 32000; i++)
			{
				Sign sign = Main.sign[i];
				if (sign != null && sign.text != null)
				{
					num += 1;
				}
			}
			writer.Write(num);
			for (int j = 0; j < 32000; j++)
			{
				Sign sign = Main.sign[j];
				if (sign != null && sign.text != null)
				{
					writer.Write(sign.text);
					writer.Write(sign.x);
					writer.Write(sign.y);
				}
			}
			return (int)writer.BaseStream.Position;
		}

		// Token: 0x06001526 RID: 5414 RVA: 0x004BF8E4 File Offset: 0x004BDAE4
		public static int SaveNPCs(BinaryWriter writer)
		{
			bool[] array = (bool[])NPC.ShimmeredTownNPCs.Clone();
			writer.Write(array.Count(true));
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i])
				{
					writer.Write(i);
				}
			}
			for (int j = 0; j < Main.npc.Length; j++)
			{
				NPC npc = Main.npc[j];
				if (npc.active && npc.townNPC && npc.type != 368)
				{
					writer.Write(npc.active);
					writer.Write(npc.netID);
					writer.Write(npc.GivenName);
					writer.Write(npc.position.X);
					writer.Write(npc.position.Y);
					writer.Write(npc.homeless);
					writer.Write(npc.homeTileX);
					writer.Write(npc.homeTileY);
					BitsByte bb = 0;
					bb[0] = npc.townNPC;
					writer.Write(bb);
					if (bb[0])
					{
						writer.Write(npc.townNpcVariationIndex);
					}
					writer.Write(npc.homelessDespawn);
				}
			}
			writer.Write(false);
			for (int k = 0; k < Main.npc.Length; k++)
			{
				NPC npc2 = Main.npc[k];
				if (npc2.active && NPCID.Sets.SavesAndLoads[npc2.type])
				{
					writer.Write(npc2.active);
					writer.Write(npc2.netID);
					writer.WriteVector2(npc2.position);
				}
			}
			writer.Write(false);
			return (int)writer.BaseStream.Position;
		}

		// Token: 0x06001527 RID: 5415 RVA: 0x004BFA96 File Offset: 0x004BDC96
		public static int SaveFooter(BinaryWriter writer)
		{
			writer.Write(true);
			writer.Write(Main.worldName);
			writer.Write(Main.ActiveWorldFileData.WorldId);
			return (int)writer.BaseStream.Position;
		}

		// Token: 0x06001528 RID: 5416 RVA: 0x004BFAC8 File Offset: 0x004BDCC8
		public static int LoadWorld_Version2(BinaryReader reader)
		{
			reader.BaseStream.Position = 0L;
			bool[] importance;
			int[] array;
			if (!WorldFile.LoadFileFormatHeader(reader, out importance, out array))
			{
				return StatusID.BadSectionPointer;
			}
			if (reader.BaseStream.Position != (long)array[0])
			{
				return StatusID.BadSectionPointer;
			}
			WorldFile.LoadHeader(reader);
			if (reader.BaseStream.Position != (long)array[1])
			{
				return StatusID.BadSectionPointer;
			}
			WorldFile.LoadWorldTiles(reader, importance);
			if (reader.BaseStream.Position != (long)array[2])
			{
				return StatusID.BadSectionPointer;
			}
			WorldFile.LoadChests(reader);
			if (reader.BaseStream.Position != (long)array[3])
			{
				return StatusID.BadSectionPointer;
			}
			WorldFile.LoadSigns(reader);
			if (reader.BaseStream.Position != (long)array[4])
			{
				return StatusID.BadSectionPointer;
			}
			WorldFile.LoadNPCs(reader);
			if (reader.BaseStream.Position != (long)array[5])
			{
				return StatusID.BadSectionPointer;
			}
			if (WorldFile._versionNumber >= 116)
			{
				if (WorldFile._versionNumber < 122)
				{
					WorldFile.LoadDummies(reader);
					if (reader.BaseStream.Position != (long)array[6])
					{
						return StatusID.BadSectionPointer;
					}
				}
				else
				{
					WorldFile.LoadTileEntities(reader);
					if (reader.BaseStream.Position != (long)array[6])
					{
						return StatusID.BadSectionPointer;
					}
				}
			}
			if (WorldFile._versionNumber >= 170)
			{
				WorldFile.LoadWeightedPressurePlates(reader);
				if (reader.BaseStream.Position != (long)array[7])
				{
					return StatusID.BadSectionPointer;
				}
			}
			if (WorldFile._versionNumber >= 189)
			{
				WorldFile.LoadTownManager(reader);
				if (reader.BaseStream.Position != (long)array[8])
				{
					return StatusID.BadSectionPointer;
				}
			}
			if (WorldFile._versionNumber >= 210)
			{
				WorldFile.LoadBestiary(reader, WorldFile._versionNumber);
				if (reader.BaseStream.Position != (long)array[9])
				{
					return StatusID.BadSectionPointer;
				}
			}
			else
			{
				WorldFile.LoadBestiaryForVersionsBefore210();
			}
			if (WorldFile._versionNumber >= 220)
			{
				WorldFile.LoadCreativePowers(reader, WorldFile._versionNumber);
				if (reader.BaseStream.Position != (long)array[10])
				{
					return StatusID.BadSectionPointer;
				}
			}
			WorldFile.LoadWorld_LastMinuteFixes();
			return WorldFile.LoadFooter(reader);
		}

		// Token: 0x06001529 RID: 5417 RVA: 0x004BFCA9 File Offset: 0x004BDEA9
		private static void LoadWorld_LastMinuteFixes()
		{
			if (WorldFile._versionNumber < 258)
			{
				WorldFile.ConvertIlluminantPaintToNewField();
			}
			WorldFile.FixAgainstExploits();
		}

		// Token: 0x0600152A RID: 5418 RVA: 0x004BFCC4 File Offset: 0x004BDEC4
		private static void FixAgainstExploits()
		{
			for (int i = 0; i < 8000; i++)
			{
				Chest chest = Main.chest[i];
				if (chest != null)
				{
					chest.FixLoadedData();
				}
			}
			foreach (TileEntity tileEntity in TileEntity.ByID.Values)
			{
				IFixLoadedData fixLoadedData = tileEntity as IFixLoadedData;
				if (fixLoadedData != null)
				{
					fixLoadedData.FixLoadedData();
				}
			}
			for (int j = 0; j < Main.npc.Length; j++)
			{
				NPC npc = Main.npc[j];
				if (npc.active && npc.townNPC && npc.type != 368 && npc.type != 37 && !npc.homeless)
				{
					bool flag = WorldGen.StartRoomCheck(npc.homeTileX, npc.homeTileY - 1, NoRoomCheckFeedback.WithoutText);
					if (!flag)
					{
						Point point;
						if (WorldGen.TownManager.HasRoom(npc.type, out point))
						{
							npc.homeTileX = point.X;
							npc.homeTileY = point.Y;
							flag = WorldGen.StartRoomCheck(npc.homeTileX, npc.homeTileY - 1, NoRoomCheckFeedback.WithoutText);
						}
						if (!flag)
						{
							WorldGen.TownManager.KickOut(npc);
							npc.homeless = true;
						}
					}
				}
			}
		}

		// Token: 0x0600152B RID: 5419 RVA: 0x004BFE34 File Offset: 0x004BE034
		public static bool LoadFileFormatHeader(BinaryReader reader, out bool[] importance, out int[] positions)
		{
			importance = null;
			positions = null;
			if ((WorldFile._versionNumber = reader.ReadInt32()) >= 135)
			{
				try
				{
					Main.WorldFileMetadata = FileMetadata.Read(reader, FileType.World);
					goto IL_4B;
				}
				catch (FormatException value)
				{
					Console.WriteLine(Language.GetTextValue("Error.UnableToLoadWorld"));
					Console.WriteLine(value);
					return false;
				}
			}
			Main.WorldFileMetadata = FileMetadata.FromCurrentSettings(FileType.World);
			IL_4B:
			short num = reader.ReadInt16();
			positions = new int[(int)num];
			for (int i = 0; i < (int)num; i++)
			{
				positions[i] = reader.ReadInt32();
			}
			ushort num2 = reader.ReadUInt16();
			importance = new bool[(int)num2];
			byte b = 0;
			byte b2 = 128;
			for (int i = 0; i < (int)num2; i++)
			{
				if (b2 == 128)
				{
					b = reader.ReadByte();
					b2 = 1;
				}
				else
				{
					b2 = (byte)(b2 << 1);
				}
				if ((b & b2) == b2)
				{
					importance[i] = true;
				}
			}
			return true;
		}

		// Token: 0x0600152C RID: 5420 RVA: 0x004BFF14 File Offset: 0x004BE114
		public static void LoadHeader(BinaryReader reader)
		{
			int versionNumber = WorldFile._versionNumber;
			Main.worldName = reader.ReadString();
			if (versionNumber >= 179)
			{
				string seed;
				if (versionNumber == 179)
				{
					seed = reader.ReadInt32().ToString();
				}
				else
				{
					seed = reader.ReadString();
				}
				Main.ActiveWorldFileData.SetSeed(seed);
				Main.ActiveWorldFileData.WorldGeneratorVersion = reader.ReadUInt64();
			}
			if (versionNumber >= 181)
			{
				Main.ActiveWorldFileData.UniqueId = new Guid(reader.ReadBytes(16));
			}
			else
			{
				Main.ActiveWorldFileData.UniqueId = Guid.NewGuid();
			}
			Main.ActiveWorldFileData.WorldId = reader.ReadInt32();
			Main.leftWorld = (float)reader.ReadInt32();
			Main.rightWorld = (float)reader.ReadInt32();
			Main.topWorld = (float)reader.ReadInt32();
			Main.bottomWorld = (float)reader.ReadInt32();
			Main.maxTilesY = reader.ReadInt32();
			Main.maxTilesX = reader.ReadInt32();
			WorldGen.clearWorld();
			if (versionNumber >= 209)
			{
				Main.GameMode = reader.ReadInt32();
				if (versionNumber >= 222)
				{
					Main.drunkWorld = reader.ReadBoolean();
				}
				if (versionNumber >= 227)
				{
					Main.getGoodWorld = reader.ReadBoolean();
				}
				if (versionNumber >= 238)
				{
					Main.tenthAnniversaryWorld = reader.ReadBoolean();
				}
				if (versionNumber >= 239)
				{
					Main.dontStarveWorld = reader.ReadBoolean();
				}
				if (versionNumber >= 241)
				{
					Main.notTheBeesWorld = reader.ReadBoolean();
				}
				if (versionNumber >= 249)
				{
					Main.remixWorld = reader.ReadBoolean();
				}
				if (versionNumber >= 266)
				{
					Main.noTrapsWorld = reader.ReadBoolean();
				}
				if (versionNumber >= 267)
				{
					Main.zenithWorld = reader.ReadBoolean();
				}
				else
				{
					Main.zenithWorld = (Main.remixWorld && Main.drunkWorld);
				}
				if (versionNumber >= 302)
				{
					Main.skyblockWorld = reader.ReadBoolean();
				}
			}
			else
			{
				if (versionNumber >= 112)
				{
					Main.GameMode = (reader.ReadBoolean() ? 1 : 0);
				}
				else
				{
					Main.GameMode = 0;
				}
				if (versionNumber == 208 && reader.ReadBoolean())
				{
					Main.GameMode = 2;
				}
			}
			if (versionNumber >= 141)
			{
				Main.ActiveWorldFileData.CreationTime = DateTime.FromBinary(reader.ReadInt64());
			}
			if (versionNumber >= 284)
			{
				Main.ActiveWorldFileData.LastPlayed = DateTime.FromBinary(reader.ReadInt64());
			}
			Main.moonType = (int)reader.ReadByte();
			Main.treeX[0] = reader.ReadInt32();
			Main.treeX[1] = reader.ReadInt32();
			Main.treeX[2] = reader.ReadInt32();
			Main.treeStyle[0] = reader.ReadInt32();
			Main.treeStyle[1] = reader.ReadInt32();
			Main.treeStyle[2] = reader.ReadInt32();
			Main.treeStyle[3] = reader.ReadInt32();
			Main.caveBackX[0] = reader.ReadInt32();
			Main.caveBackX[1] = reader.ReadInt32();
			Main.caveBackX[2] = reader.ReadInt32();
			Main.caveBackStyle[0] = reader.ReadInt32();
			Main.caveBackStyle[1] = reader.ReadInt32();
			Main.caveBackStyle[2] = reader.ReadInt32();
			Main.caveBackStyle[3] = reader.ReadInt32();
			Main.iceBackStyle = reader.ReadInt32();
			Main.jungleBackStyle = reader.ReadInt32();
			Main.hellBackStyle = reader.ReadInt32();
			Main.spawnTileX = reader.ReadInt32();
			Main.spawnTileY = reader.ReadInt32();
			Main.worldSurface = reader.ReadDouble();
			Main.rockLayer = reader.ReadDouble();
			WorldFile._tempTime = reader.ReadDouble();
			WorldFile._tempDayTime = reader.ReadBoolean();
			WorldFile._tempMoonPhase = reader.ReadInt32();
			WorldFile._tempBloodMoon = reader.ReadBoolean();
			WorldFile._tempEclipse = reader.ReadBoolean();
			Main.eclipse = WorldFile._tempEclipse;
			Main.dungeonX = reader.ReadInt32();
			Main.dungeonY = reader.ReadInt32();
			WorldGen.crimson = reader.ReadBoolean();
			NPC.downedBoss1 = reader.ReadBoolean();
			NPC.downedBoss2 = reader.ReadBoolean();
			NPC.downedBoss3 = reader.ReadBoolean();
			NPC.downedQueenBee = reader.ReadBoolean();
			NPC.downedMechBoss1 = reader.ReadBoolean();
			NPC.downedMechBoss2 = reader.ReadBoolean();
			NPC.downedMechBoss3 = reader.ReadBoolean();
			NPC.downedMechBossAny = reader.ReadBoolean();
			NPC.downedPlantBoss = reader.ReadBoolean();
			NPC.downedGolemBoss = reader.ReadBoolean();
			if (versionNumber >= 118)
			{
				NPC.downedSlimeKing = reader.ReadBoolean();
			}
			NPC.savedGoblin = reader.ReadBoolean();
			NPC.savedWizard = reader.ReadBoolean();
			NPC.savedMech = reader.ReadBoolean();
			NPC.downedGoblins = reader.ReadBoolean();
			NPC.downedClown = reader.ReadBoolean();
			NPC.downedFrost = reader.ReadBoolean();
			NPC.downedPirates = reader.ReadBoolean();
			WorldGen.shadowOrbSmashed = reader.ReadBoolean();
			WorldGen.spawnMeteor = reader.ReadBoolean();
			WorldGen.shadowOrbCount = (int)reader.ReadByte();
			WorldGen.altarCount = reader.ReadInt32();
			Main.hardMode = reader.ReadBoolean();
			if (versionNumber >= 257)
			{
				Main.afterPartyOfDoom = reader.ReadBoolean();
			}
			else
			{
				Main.afterPartyOfDoom = false;
			}
			Main.invasionDelay = reader.ReadInt32();
			Main.invasionSize = reader.ReadInt32();
			Main.invasionType = reader.ReadInt32();
			Main.invasionX = reader.ReadDouble();
			if (versionNumber >= 118)
			{
				Main.slimeRainTime = reader.ReadDouble();
			}
			if (versionNumber >= 113)
			{
				Main.sundialCooldown = (int)reader.ReadByte();
			}
			WorldFile._tempRaining = reader.ReadBoolean();
			WorldFile._tempRainTime = reader.ReadInt32();
			WorldFile._tempMaxRain = reader.ReadSingle();
			WorldFile.FixEndlessRainWorlds();
			WorldGen.SavedOreTiers.Cobalt = reader.ReadInt32();
			WorldGen.SavedOreTiers.Mythril = reader.ReadInt32();
			WorldGen.SavedOreTiers.Adamantite = reader.ReadInt32();
			WorldGen.setBG(0, (int)reader.ReadByte());
			WorldGen.setBG(1, (int)reader.ReadByte());
			WorldGen.setBG(2, (int)reader.ReadByte());
			WorldGen.setBG(3, (int)reader.ReadByte());
			WorldGen.setBG(4, (int)reader.ReadByte());
			WorldGen.setBG(5, (int)reader.ReadByte());
			WorldGen.setBG(6, (int)reader.ReadByte());
			WorldGen.setBG(7, (int)reader.ReadByte());
			Main.cloudBGActive = (float)reader.ReadInt32();
			Main.cloudBGAlpha = (((double)Main.cloudBGActive < 1.0) ? 0f : 1f);
			Main.cloudBGActive = (float)(-(float)WorldGen.genRand.Next(8640, 86400));
			Main.numClouds = (int)reader.ReadInt16();
			Main.windSpeedTarget = reader.ReadSingle();
			Main.windSpeedCurrent = Main.windSpeedTarget;
			if (versionNumber < 95)
			{
				return;
			}
			Main.anglerWhoFinishedToday.Clear();
			for (int i = reader.ReadInt32(); i > 0; i--)
			{
				Main.anglerWhoFinishedToday.Add(reader.ReadString());
			}
			if (versionNumber < 99)
			{
				return;
			}
			NPC.savedAngler = reader.ReadBoolean();
			if (versionNumber < 101)
			{
				return;
			}
			Main.anglerQuest = reader.ReadInt32();
			if (versionNumber < 104)
			{
				return;
			}
			NPC.savedStylist = reader.ReadBoolean();
			if (versionNumber >= 129)
			{
				NPC.savedTaxCollector = reader.ReadBoolean();
			}
			if (versionNumber >= 201)
			{
				NPC.savedGolfer = reader.ReadBoolean();
			}
			if (versionNumber < 107)
			{
				if (Main.invasionType > 0 && Main.invasionSize > 0)
				{
					Main.FakeLoadInvasionStart();
				}
			}
			else
			{
				Main.invasionSizeStart = reader.ReadInt32();
			}
			if (versionNumber < 108)
			{
				WorldFile._tempCultistDelay = 86400;
			}
			else
			{
				WorldFile._tempCultistDelay = reader.ReadInt32();
			}
			if (versionNumber < 109)
			{
				return;
			}
			BannerSystem.Load(reader, versionNumber);
			if (versionNumber < 128)
			{
				return;
			}
			Main.fastForwardTimeToDawn = reader.ReadBoolean();
			if (versionNumber < 131)
			{
				return;
			}
			NPC.downedFishron = reader.ReadBoolean();
			NPC.downedMartians = reader.ReadBoolean();
			NPC.downedAncientCultist = reader.ReadBoolean();
			NPC.downedMoonlord = reader.ReadBoolean();
			NPC.downedHalloweenKing = reader.ReadBoolean();
			NPC.downedHalloweenTree = reader.ReadBoolean();
			NPC.downedChristmasIceQueen = reader.ReadBoolean();
			NPC.downedChristmasSantank = reader.ReadBoolean();
			NPC.downedChristmasTree = reader.ReadBoolean();
			if (versionNumber < 140)
			{
				return;
			}
			NPC.downedTowerSolar = reader.ReadBoolean();
			NPC.downedTowerVortex = reader.ReadBoolean();
			NPC.downedTowerNebula = reader.ReadBoolean();
			NPC.downedTowerStardust = reader.ReadBoolean();
			NPC.TowerActiveSolar = reader.ReadBoolean();
			NPC.TowerActiveVortex = reader.ReadBoolean();
			NPC.TowerActiveNebula = reader.ReadBoolean();
			NPC.TowerActiveStardust = reader.ReadBoolean();
			NPC.LunarApocalypseIsUp = reader.ReadBoolean();
			if (NPC.TowerActiveSolar)
			{
				NPC.ShieldStrengthTowerSolar = NPC.ShieldStrengthTowerMax;
			}
			if (NPC.TowerActiveVortex)
			{
				NPC.ShieldStrengthTowerVortex = NPC.ShieldStrengthTowerMax;
			}
			if (NPC.TowerActiveNebula)
			{
				NPC.ShieldStrengthTowerNebula = NPC.ShieldStrengthTowerMax;
			}
			if (NPC.TowerActiveStardust)
			{
				NPC.ShieldStrengthTowerStardust = NPC.ShieldStrengthTowerMax;
			}
			if (versionNumber < 170)
			{
				WorldFile._tempPartyManual = false;
				WorldFile._tempPartyGenuine = false;
				WorldFile._tempPartyCooldown = 0;
				WorldFile.TempPartyCelebratingNPCs.Clear();
			}
			else
			{
				WorldFile._tempPartyManual = reader.ReadBoolean();
				WorldFile._tempPartyGenuine = reader.ReadBoolean();
				WorldFile._tempPartyCooldown = reader.ReadInt32();
				int num = reader.ReadInt32();
				WorldFile.TempPartyCelebratingNPCs.Clear();
				for (int j = 0; j < num; j++)
				{
					WorldFile.TempPartyCelebratingNPCs.Add(reader.ReadInt32());
				}
			}
			if (versionNumber < 174)
			{
				WorldFile._tempSandstormHappening = false;
				WorldFile._tempSandstormTimeLeft = 0;
				WorldFile._tempSandstormSeverity = 0f;
				WorldFile._tempSandstormIntendedSeverity = 0f;
			}
			else
			{
				WorldFile._tempSandstormHappening = reader.ReadBoolean();
				WorldFile._tempSandstormTimeLeft = reader.ReadInt32();
				WorldFile._tempSandstormSeverity = reader.ReadSingle();
				WorldFile._tempSandstormIntendedSeverity = reader.ReadSingle();
			}
			DD2Event.Load(reader, versionNumber);
			if (versionNumber > 194)
			{
				WorldGen.setBG(8, (int)reader.ReadByte());
			}
			else
			{
				WorldGen.setBG(8, 0);
			}
			if (versionNumber >= 215)
			{
				WorldGen.setBG(9, (int)reader.ReadByte());
			}
			else
			{
				WorldGen.setBG(9, 0);
			}
			if (versionNumber > 195)
			{
				WorldGen.setBG(10, (int)reader.ReadByte());
				WorldGen.setBG(11, (int)reader.ReadByte());
				WorldGen.setBG(12, (int)reader.ReadByte());
			}
			else
			{
				WorldGen.setBG(10, WorldGen.treeBG1);
				WorldGen.setBG(11, WorldGen.treeBG1);
				WorldGen.setBG(12, WorldGen.treeBG1);
			}
			if (versionNumber >= 204)
			{
				NPC.combatBookWasUsed = reader.ReadBoolean();
			}
			if (versionNumber < 207)
			{
				WorldFile._tempLanternNightCooldown = 0;
				WorldFile._tempLanternNightGenuine = false;
				WorldFile._tempLanternNightManual = false;
				WorldFile._tempLanternNightNextNightIsGenuine = false;
			}
			else
			{
				WorldFile._tempLanternNightCooldown = reader.ReadInt32();
				WorldFile._tempLanternNightGenuine = reader.ReadBoolean();
				WorldFile._tempLanternNightManual = reader.ReadBoolean();
				WorldFile._tempLanternNightNextNightIsGenuine = reader.ReadBoolean();
			}
			WorldGen.TreeTops.Load(reader, versionNumber);
			if (versionNumber >= 212)
			{
				Main.forceHalloweenForToday = reader.ReadBoolean();
				Main.forceXMasForToday = reader.ReadBoolean();
			}
			else
			{
				Main.forceHalloweenForToday = false;
				Main.forceXMasForToday = false;
			}
			if (versionNumber >= 216)
			{
				WorldGen.SavedOreTiers.Copper = reader.ReadInt32();
				WorldGen.SavedOreTiers.Iron = reader.ReadInt32();
				WorldGen.SavedOreTiers.Silver = reader.ReadInt32();
				WorldGen.SavedOreTiers.Gold = reader.ReadInt32();
			}
			else
			{
				WorldGen.SavedOreTiers.Copper = -1;
				WorldGen.SavedOreTiers.Iron = -1;
				WorldGen.SavedOreTiers.Silver = -1;
				WorldGen.SavedOreTiers.Gold = -1;
			}
			if (versionNumber >= 217)
			{
				NPC.boughtCat = reader.ReadBoolean();
				NPC.boughtDog = reader.ReadBoolean();
				NPC.boughtBunny = reader.ReadBoolean();
			}
			else
			{
				NPC.boughtCat = false;
				NPC.boughtDog = false;
				NPC.boughtBunny = false;
			}
			if (versionNumber >= 223)
			{
				NPC.downedEmpressOfLight = reader.ReadBoolean();
				NPC.downedQueenSlime = reader.ReadBoolean();
			}
			else
			{
				NPC.downedEmpressOfLight = false;
				NPC.downedQueenSlime = false;
			}
			if (versionNumber >= 240)
			{
				NPC.downedDeerclops = reader.ReadBoolean();
			}
			else
			{
				NPC.downedDeerclops = false;
			}
			if (versionNumber >= 250)
			{
				NPC.unlockedSlimeBlueSpawn = reader.ReadBoolean();
			}
			else
			{
				NPC.unlockedSlimeBlueSpawn = false;
			}
			if (versionNumber >= 251)
			{
				NPC.unlockedMerchantSpawn = reader.ReadBoolean();
				NPC.unlockedDemolitionistSpawn = reader.ReadBoolean();
				NPC.unlockedPartyGirlSpawn = reader.ReadBoolean();
				NPC.unlockedDyeTraderSpawn = reader.ReadBoolean();
				NPC.unlockedTruffleSpawn = reader.ReadBoolean();
				NPC.unlockedArmsDealerSpawn = reader.ReadBoolean();
				NPC.unlockedNurseSpawn = reader.ReadBoolean();
				NPC.unlockedPrincessSpawn = reader.ReadBoolean();
			}
			else
			{
				NPC.unlockedMerchantSpawn = false;
				NPC.unlockedDemolitionistSpawn = false;
				NPC.unlockedPartyGirlSpawn = false;
				NPC.unlockedDyeTraderSpawn = false;
				NPC.unlockedTruffleSpawn = false;
				NPC.unlockedArmsDealerSpawn = false;
				NPC.unlockedNurseSpawn = false;
				NPC.unlockedPrincessSpawn = false;
			}
			if (versionNumber >= 259)
			{
				NPC.combatBookVolumeTwoWasUsed = reader.ReadBoolean();
			}
			else
			{
				NPC.combatBookVolumeTwoWasUsed = false;
			}
			if (versionNumber >= 260)
			{
				NPC.peddlersSatchelWasUsed = reader.ReadBoolean();
			}
			else
			{
				NPC.peddlersSatchelWasUsed = false;
			}
			if (versionNumber >= 261)
			{
				NPC.unlockedSlimeGreenSpawn = reader.ReadBoolean();
				NPC.unlockedSlimeOldSpawn = reader.ReadBoolean();
				NPC.unlockedSlimePurpleSpawn = reader.ReadBoolean();
				NPC.unlockedSlimeRainbowSpawn = reader.ReadBoolean();
				NPC.unlockedSlimeRedSpawn = reader.ReadBoolean();
				NPC.unlockedSlimeYellowSpawn = reader.ReadBoolean();
				NPC.unlockedSlimeCopperSpawn = reader.ReadBoolean();
			}
			else
			{
				NPC.unlockedSlimeGreenSpawn = false;
				NPC.unlockedSlimeOldSpawn = false;
				NPC.unlockedSlimePurpleSpawn = false;
				NPC.unlockedSlimeRainbowSpawn = false;
				NPC.unlockedSlimeRedSpawn = false;
				NPC.unlockedSlimeYellowSpawn = false;
				NPC.unlockedSlimeCopperSpawn = false;
			}
			if (versionNumber >= 264)
			{
				Main.fastForwardTimeToDusk = reader.ReadBoolean();
				Main.moondialCooldown = (int)reader.ReadByte();
			}
			else
			{
				Main.fastForwardTimeToDusk = false;
				Main.moondialCooldown = 0;
			}
			if (versionNumber >= 287)
			{
				Main.forceHalloweenForever = reader.ReadBoolean();
				Main.forceXMasForever = reader.ReadBoolean();
			}
			else
			{
				Main.forceHalloweenForever = false;
				Main.forceXMasForever = false;
			}
			if (versionNumber >= 288)
			{
				Main.vampireSeed = reader.ReadBoolean();
			}
			else
			{
				Main.vampireSeed = false;
			}
			if (versionNumber >= 296)
			{
				Main.infectedSeed = reader.ReadBoolean();
			}
			else
			{
				Main.infectedSeed = false;
			}
			if (versionNumber >= 291)
			{
				WorldFile._tempMeteorShowerCount = reader.ReadInt32();
				WorldFile._tempCoinRain = reader.ReadInt32();
			}
			else
			{
				WorldFile._tempMeteorShowerCount = 0;
				WorldFile._tempCoinRain = 0;
			}
			if (versionNumber >= 297)
			{
				Main.teamBasedSpawnsSeed = reader.ReadBoolean();
				ExtraSpawnPointManager.Read(reader, false);
			}
			else
			{
				Main.teamBasedSpawnsSeed = false;
				ExtraSpawnPointManager.Clear();
			}
			Main.dualDungeonsSeed = (versionNumber >= 304 && reader.ReadBoolean());
			Main.UpdateTimeRate();
			if (versionNumber >= 299 && versionNumber < 313)
			{
				reader.ReadUInt32();
			}
			WorldGen.Manifest = WorldManifest.Deserialize((versionNumber < 299) ? "" : reader.ReadString());
		}

		// Token: 0x0600152D RID: 5421 RVA: 0x004C0CA4 File Offset: 0x004BEEA4
		public static void LoadWorldTiles(BinaryReader reader, bool[] importance)
		{
			for (int i = 0; i < Main.maxTilesX; i++)
			{
				float num = (float)i / (float)Main.maxTilesX;
				Main.statusText = string.Concat(new object[]
				{
					Lang.gen[51].Value,
					" ",
					(int)((double)num * 100.0 + 1.0),
					"%"
				});
				for (int j = 0; j < Main.maxTilesY; j++)
				{
					int num2 = -1;
					byte b3;
					byte b2;
					byte b = b2 = (b3 = 0);
					Tile tile = Main.tile[i, j];
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
					byte b5;
					if ((b4 & 2) == 2)
					{
						tile.active(true);
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
						if (importance[num2])
						{
							tile.frameX = reader.ReadInt16();
							tile.frameY = reader.ReadInt16();
							if (tile.type == 144)
							{
								tile.frameY = 0;
							}
						}
						else
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
						if (tile.wall >= WallID.Count)
						{
							tile.wall = 0;
						}
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
						if (b5 != 0 && TileID.Sets.SaveSlopes[(int)tile.type])
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
							if (tile.wall >= WallID.Count)
							{
								tile.wall = 0;
							}
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
					int k;
					if (b5 == 0)
					{
						k = 0;
					}
					else if (b5 == 1)
					{
						k = (int)reader.ReadByte();
					}
					else
					{
						k = (int)reader.ReadInt16();
					}
					if (num2 != -1)
					{
						if ((double)j <= Main.worldSurface)
						{
							if ((double)(j + k) <= Main.worldSurface)
							{
								WorldGen.tileCounts[num2] += (k + 1) * 5;
							}
							else
							{
								int num3 = (int)(Main.worldSurface - (double)j + 1.0);
								int num4 = k + 1 - num3;
								WorldGen.tileCounts[num2] += num3 * 5 + num4;
							}
						}
						else
						{
							WorldGen.tileCounts[num2] += k + 1;
						}
					}
					while (k > 0)
					{
						j++;
						Main.tile[i, j].CopyFrom(tile);
						k--;
					}
				}
			}
			WorldGen.AddUpAlignmentCounts(true);
			if (WorldFile._versionNumber < 105)
			{
				WorldGen.FixHearts();
			}
		}

		// Token: 0x0600152E RID: 5422 RVA: 0x004C10A4 File Offset: 0x004BF2A4
		public static void LoadChests(BinaryReader reader)
		{
			int num = (int)reader.ReadInt16();
			int num2 = 0;
			if (WorldFile._versionNumber < 294)
			{
				num2 = (int)reader.ReadInt16();
			}
			int i;
			for (i = 0; i < num; i++)
			{
				int x = reader.ReadInt32();
				int y = reader.ReadInt32();
				Chest chest = Chest.CreateWorldChest(i, x, y);
				chest.name = reader.ReadString();
				if (WorldFile._versionNumber >= 294)
				{
					int num3 = reader.ReadInt32();
					chest.Resize(num3);
					num2 = num3;
				}
				int num4;
				int num5;
				if (num2 < chest.maxItems)
				{
					num4 = num2;
					num5 = 0;
				}
				else
				{
					num4 = chest.maxItems;
					num5 = num2 - chest.maxItems;
				}
				for (int j = 0; j < num4; j++)
				{
					short num6 = reader.ReadInt16();
					Item item = new Item();
					if (num6 > 0)
					{
						item.netDefaults(reader.ReadInt32());
						item.stack = (int)num6;
						item.Prefix((int)reader.ReadByte());
					}
					else if (num6 < 0)
					{
						item.netDefaults(reader.ReadInt32());
						item.Prefix((int)reader.ReadByte());
						item.stack = 1;
					}
					chest.item[j] = item;
				}
				for (int k = 0; k < num5; k++)
				{
					short num6 = reader.ReadInt16();
					if (num6 > 0)
					{
						reader.ReadInt32();
						reader.ReadByte();
					}
				}
			}
			List<Point16> list = new List<Point16>();
			for (int l = 0; l < i; l++)
			{
				if (Main.chest[l] != null)
				{
					Point16 item2 = new Point16(Main.chest[l].x, Main.chest[l].y);
					if (list.Contains(item2))
					{
						Chest.RemoveChest(l);
					}
					else
					{
						list.Add(item2);
					}
				}
			}
			while (i < 8000)
			{
				Chest.RemoveChest(i);
				i++;
			}
			if (WorldFile._versionNumber < 115)
			{
				WorldFile.FixDresserChests();
			}
		}

		// Token: 0x0600152F RID: 5423 RVA: 0x004C1278 File Offset: 0x004BF478
		private static void ConvertIlluminantPaintToNewField()
		{
			for (int i = 0; i < Main.maxTilesX; i++)
			{
				for (int j = 0; j < Main.maxTilesY; j++)
				{
					Tile tile = Main.tile[i, j];
					if (tile.active() && tile.color() == 31)
					{
						tile.color(0);
						tile.fullbrightBlock(true);
					}
					if (tile.wallColor() == 31)
					{
						tile.wallColor(0);
						tile.fullbrightWall(true);
					}
				}
			}
		}

		// Token: 0x06001530 RID: 5424 RVA: 0x004C12EC File Offset: 0x004BF4EC
		public static void LoadSigns(BinaryReader reader)
		{
			short num = reader.ReadInt16();
			int i;
			for (i = 0; i < (int)num; i++)
			{
				string text = reader.ReadString();
				int num2 = reader.ReadInt32();
				int num3 = reader.ReadInt32();
				Tile tile = Main.tile[num2, num3];
				Sign sign;
				if (tile.active() && Main.tileSign[(int)tile.type])
				{
					sign = new Sign();
					sign.text = text;
					sign.x = num2;
					sign.y = num3;
				}
				else
				{
					sign = null;
				}
				Main.sign[i] = sign;
			}
			List<Point16> list = new List<Point16>();
			for (int j = 0; j < 32000; j++)
			{
				if (Main.sign[j] != null)
				{
					Point16 item = new Point16(Main.sign[j].x, Main.sign[j].y);
					if (list.Contains(item))
					{
						Main.sign[j] = null;
					}
					else
					{
						list.Add(item);
					}
				}
			}
			while (i < 32000)
			{
				Main.sign[i] = null;
				i++;
			}
		}

		// Token: 0x06001531 RID: 5425 RVA: 0x004C13F0 File Offset: 0x004BF5F0
		public static void LoadDummies(BinaryReader reader)
		{
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				reader.ReadInt16();
				reader.ReadInt16();
			}
		}

		// Token: 0x06001532 RID: 5426 RVA: 0x004C1420 File Offset: 0x004BF620
		public static void LoadNPCs(BinaryReader reader)
		{
			if (WorldFile._versionNumber >= 268)
			{
				int num = reader.ReadInt32();
				while (num-- > 0)
				{
					NPC.ShimmeredTownNPCs[reader.ReadInt32()] = true;
				}
			}
			int num2 = 0;
			bool flag = reader.ReadBoolean();
			while (flag)
			{
				NPC npc = Main.npc[num2];
				if (WorldFile._versionNumber >= 190)
				{
					npc.SetDefaults(reader.ReadInt32(), default(NPCSpawnParams));
				}
				else
				{
					npc.SetDefaults(NPCID.FromLegacyName(reader.ReadString()), default(NPCSpawnParams));
				}
				npc.GivenName = reader.ReadString();
				npc.position.X = reader.ReadSingle();
				npc.position.Y = reader.ReadSingle();
				npc.homeless = reader.ReadBoolean();
				npc.homeTileX = reader.ReadInt32();
				npc.homeTileY = reader.ReadInt32();
				if (WorldFile._versionNumber >= 213 && reader.ReadByte()[0])
				{
					npc.townNpcVariationIndex = reader.ReadInt32();
				}
				if (WorldFile._versionNumber >= 315)
				{
					npc.homelessDespawn = reader.ReadBoolean();
				}
				num2++;
				flag = reader.ReadBoolean();
			}
			if (WorldFile._versionNumber >= 140)
			{
				flag = reader.ReadBoolean();
				while (flag)
				{
					NPC npc = Main.npc[num2];
					if (WorldFile._versionNumber >= 190)
					{
						npc.SetDefaults(reader.ReadInt32(), default(NPCSpawnParams));
					}
					else
					{
						npc.SetDefaults(NPCID.FromLegacyName(reader.ReadString()), default(NPCSpawnParams));
					}
					npc.position = reader.ReadVector2();
					num2++;
					flag = reader.ReadBoolean();
				}
			}
			if (WorldFile._versionNumber < 251)
			{
				NPC.unlockedMerchantSpawn = NPC.AnyNPCs(17);
				NPC.unlockedDemolitionistSpawn = NPC.AnyNPCs(38);
				NPC.unlockedPartyGirlSpawn = NPC.AnyNPCs(208);
				NPC.unlockedDyeTraderSpawn = NPC.AnyNPCs(207);
				NPC.unlockedTruffleSpawn = NPC.AnyNPCs(160);
				NPC.unlockedArmsDealerSpawn = NPC.AnyNPCs(19);
				NPC.unlockedNurseSpawn = NPC.AnyNPCs(18);
				NPC.unlockedPrincessSpawn = NPC.AnyNPCs(663);
			}
		}

		// Token: 0x06001533 RID: 5427 RVA: 0x004C1644 File Offset: 0x004BF844
		public static void ValidateLoadNPCs(BinaryReader fileIO)
		{
			int num = fileIO.ReadInt32();
			while (num-- > 0)
			{
				fileIO.ReadInt32();
			}
			bool flag = fileIO.ReadBoolean();
			while (flag)
			{
				fileIO.ReadInt32();
				fileIO.ReadString();
				fileIO.ReadSingle();
				fileIO.ReadSingle();
				fileIO.ReadBoolean();
				fileIO.ReadInt32();
				fileIO.ReadInt32();
				if (fileIO.ReadByte()[0])
				{
					fileIO.ReadInt32();
				}
				fileIO.ReadBoolean();
				flag = fileIO.ReadBoolean();
			}
			flag = fileIO.ReadBoolean();
			while (flag)
			{
				fileIO.ReadInt32();
				fileIO.ReadSingle();
				fileIO.ReadSingle();
				flag = fileIO.ReadBoolean();
			}
		}

		// Token: 0x06001534 RID: 5428 RVA: 0x004C16FC File Offset: 0x004BF8FC
		public static int LoadFooter(BinaryReader reader)
		{
			if (!reader.ReadBoolean())
			{
				return StatusID.BadFooter;
			}
			if (reader.ReadString() != Main.worldName)
			{
				return StatusID.BadFooter;
			}
			if (reader.ReadInt32() != Main.ActiveWorldFileData.WorldId)
			{
				return StatusID.BadFooter;
			}
			return StatusID.Ok;
		}

		// Token: 0x06001535 RID: 5429 RVA: 0x004C174C File Offset: 0x004BF94C
		public static bool ValidateWorld(BinaryReader fileIO)
		{
			new Stopwatch().Start();
			bool result;
			try
			{
				Stream baseStream = fileIO.BaseStream;
				int num = fileIO.ReadInt32();
				if (num == 0 || num > 318)
				{
					result = false;
				}
				else
				{
					baseStream.Position = 0L;
					bool[] array;
					int[] array2;
					if (!WorldFile.LoadFileFormatHeader(fileIO, out array, out array2))
					{
						result = false;
					}
					else
					{
						string b = fileIO.ReadString();
						if (num >= 179)
						{
							if (num == 179)
							{
								fileIO.ReadInt32();
							}
							else
							{
								fileIO.ReadString();
							}
							fileIO.ReadUInt64();
						}
						if (num >= 181)
						{
							fileIO.ReadBytes(16);
						}
						int num2 = fileIO.ReadInt32();
						fileIO.ReadInt32();
						fileIO.ReadInt32();
						fileIO.ReadInt32();
						fileIO.ReadInt32();
						int num3 = fileIO.ReadInt32();
						int num4 = fileIO.ReadInt32();
						baseStream.Position = (long)array2[1];
						for (int i = 0; i < num4; i++)
						{
							float num5 = (float)i / (float)Main.maxTilesX;
							Main.statusText = string.Concat(new object[]
							{
								Lang.gen[73].Value,
								" ",
								(int)(num5 * 100f + 1f),
								"%"
							});
							for (int j = 0; j < num3; j++)
							{
								byte b3;
								byte b2 = b3 = 0;
								byte b4 = fileIO.ReadByte();
								bool flag = false;
								if ((b4 & 1) == 1)
								{
									flag = true;
									b3 = fileIO.ReadByte();
								}
								bool flag2 = false;
								if (flag && (b3 & 1) == 1)
								{
									flag2 = true;
									b2 = fileIO.ReadByte();
								}
								if (flag2 && (b2 & 1) == 1)
								{
									fileIO.ReadByte();
								}
								byte b5;
								if ((b4 & 2) == 2)
								{
									int num6;
									if ((b4 & 32) == 32)
									{
										b5 = fileIO.ReadByte();
										num6 = (int)fileIO.ReadByte();
										num6 = (num6 << 8 | (int)b5);
									}
									else
									{
										num6 = (int)fileIO.ReadByte();
									}
									if (array[num6])
									{
										fileIO.ReadInt16();
										fileIO.ReadInt16();
									}
									if ((b2 & 8) == 8)
									{
										fileIO.ReadByte();
									}
								}
								if ((b4 & 4) == 4)
								{
									fileIO.ReadByte();
									if ((b2 & 16) == 16)
									{
										fileIO.ReadByte();
									}
								}
								if ((b4 & 24) >> 3 != 0)
								{
									fileIO.ReadByte();
								}
								if ((b2 & 64) == 64)
								{
									fileIO.ReadByte();
								}
								b5 = (byte)((b4 & 192) >> 6);
								int num7;
								if (b5 == 0)
								{
									num7 = 0;
								}
								else if (b5 == 1)
								{
									num7 = (int)fileIO.ReadByte();
								}
								else
								{
									num7 = (int)fileIO.ReadInt16();
								}
								j += num7;
							}
						}
						if (baseStream.Position != (long)array2[2])
						{
							result = false;
						}
						else
						{
							int num8 = (int)fileIO.ReadInt16();
							int num9 = 0;
							if (num < 294)
							{
								num9 = (int)fileIO.ReadInt16();
							}
							for (int k = 0; k < num8; k++)
							{
								fileIO.ReadInt32();
								fileIO.ReadInt32();
								fileIO.ReadString();
								if (num >= 294)
								{
									num9 = fileIO.ReadInt32();
								}
								for (int l = 0; l < num9; l++)
								{
									if (fileIO.ReadInt16() > 0)
									{
										fileIO.ReadInt32();
										fileIO.ReadByte();
									}
								}
							}
							if (baseStream.Position != (long)array2[3])
							{
								result = false;
							}
							else
							{
								int num10 = (int)fileIO.ReadInt16();
								for (int m = 0; m < num10; m++)
								{
									fileIO.ReadString();
									fileIO.ReadInt32();
									fileIO.ReadInt32();
								}
								if (baseStream.Position != (long)array2[4])
								{
									result = false;
								}
								else
								{
									WorldFile.ValidateLoadNPCs(fileIO);
									if (baseStream.Position != (long)array2[5])
									{
										result = false;
									}
									else
									{
										if (WorldFile._versionNumber >= 116 && WorldFile._versionNumber <= 121)
										{
											int num11 = fileIO.ReadInt32();
											for (int n = 0; n < num11; n++)
											{
												fileIO.ReadInt16();
												fileIO.ReadInt16();
											}
											if (baseStream.Position != (long)array2[6])
											{
												return false;
											}
										}
										if (WorldFile._versionNumber >= 122)
										{
											int num12 = fileIO.ReadInt32();
											for (int num13 = 0; num13 < num12; num13++)
											{
												TileEntity.Read(fileIO, WorldFile._versionNumber, false);
											}
										}
										if (WorldFile._versionNumber >= 170)
										{
											int num14 = fileIO.ReadInt32();
											for (int num15 = 0; num15 < num14; num15++)
											{
												fileIO.ReadInt64();
											}
										}
										if (WorldFile._versionNumber >= 189)
										{
											int num16 = fileIO.ReadInt32();
											fileIO.ReadBytes(12 * num16);
										}
										if (WorldFile._versionNumber >= 210)
										{
											Main.BestiaryTracker.ValidateWorld(fileIO, WorldFile._versionNumber);
										}
										if (WorldFile._versionNumber >= 220)
										{
											CreativePowerManager.Instance.ValidateWorld(fileIO, WorldFile._versionNumber);
										}
										bool flag3 = fileIO.ReadBoolean();
										string a = fileIO.ReadString();
										int num17 = fileIO.ReadInt32();
										bool flag4 = false;
										if (flag3 && (a == b || num17 == num2))
										{
											flag4 = true;
										}
										result = flag4;
									}
								}
							}
						}
					}
				}
			}
			catch (Exception value)
			{
				using (StreamWriter streamWriter = new StreamWriter("client-crashlog.txt", true))
				{
					streamWriter.WriteLine(DateTime.Now);
					streamWriter.WriteLine(value);
					streamWriter.WriteLine("");
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06001536 RID: 5430 RVA: 0x004C1C64 File Offset: 0x004BFE64
		public static FileMetadata GetFileMetadata(string file, bool cloudSave)
		{
			if (file == null)
			{
				return null;
			}
			try
			{
				byte[] buffer = null;
				bool flag = cloudSave && SocialAPI.Cloud != null;
				if (flag)
				{
					int num = 24;
					buffer = new byte[num];
					SocialAPI.Cloud.Read(file, buffer, num);
				}
				using (Stream stream = flag ? new MemoryStream(buffer) : new FileStream(file, FileMode.Open))
				{
					using (BinaryReader binaryReader = new BinaryReader(stream))
					{
						if (binaryReader.ReadInt32() >= 135)
						{
							return FileMetadata.Read(binaryReader, FileType.World);
						}
						return FileMetadata.FromCurrentSettings(FileType.World);
					}
				}
			}
			catch
			{
			}
			return null;
		}

		// Token: 0x06001537 RID: 5431 RVA: 0x004C1D20 File Offset: 0x004BFF20
		private static void FixDresserChests()
		{
			for (int i = 0; i < Main.maxTilesX; i++)
			{
				for (int j = 0; j < Main.maxTilesY; j++)
				{
					Tile tile = Main.tile[i, j];
					if (tile.active() && tile.type == 88 && tile.frameX % 54 == 0 && tile.frameY % 36 == 0)
					{
						Chest.CreateChest(i, j, -1);
					}
				}
			}
		}

		// Token: 0x06001538 RID: 5432 RVA: 0x004C1D8C File Offset: 0x004BFF8C
		private static void FixEndlessRainWorlds()
		{
			if (WorldFile._versionNumber > 317)
			{
				return;
			}
			if (WorldFile._tempRainTime < 5184000)
			{
				return;
			}
			foreach (string code in Main.ActiveWorldFileData.GetSecretSeedCodes())
			{
				if (WorldGen.SecretSeed.rainsForAYear.Check(code))
				{
					return;
				}
			}
			WorldFile._tempRaining = false;
			WorldFile._tempRainTime = 0;
			WorldFile._tempMaxRain = 0f;
		}

		// Token: 0x06001539 RID: 5433 RVA: 0x004C1E20 File Offset: 0x004C0020
		public static int SaveTileEntities(BinaryWriter writer)
		{
			object entityCreationLock = TileEntity.EntityCreationLock;
			lock (entityCreationLock)
			{
				writer.Write(TileEntity.ByID.Count);
				foreach (KeyValuePair<int, TileEntity> keyValuePair in TileEntity.ByID)
				{
					TileEntity.Write(writer, keyValuePair.Value, false);
				}
			}
			return (int)writer.BaseStream.Position;
		}

		// Token: 0x0600153A RID: 5434 RVA: 0x004C1EBC File Offset: 0x004C00BC
		public static void LoadTileEntities(BinaryReader reader)
		{
			TileEntity.Clear();
			int num = reader.ReadInt32();
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				TileEntity tileEntity = TileEntity.Read(reader, WorldFile._versionNumber, false);
				tileEntity.ID = num2++;
				TileEntity entity;
				if (TileEntity.TryGetAt<TileEntity>((int)tileEntity.Position.X, (int)tileEntity.Position.Y, out entity))
				{
					TileEntity.Remove(entity, false);
				}
				TileEntity.Add(tileEntity);
			}
			TileEntity.TileEntitiesNextID = num;
			List<TileEntity> list = new List<TileEntity>();
			foreach (KeyValuePair<Point16, TileEntity> keyValuePair in TileEntity.ByPosition)
			{
				if (!WorldGen.InWorld((int)keyValuePair.Value.Position.X, (int)keyValuePair.Value.Position.Y, 1))
				{
					list.Add(keyValuePair.Value);
				}
				else if (!TileEntity.manager.CheckValidTile((int)keyValuePair.Value.type, (int)keyValuePair.Value.Position.X, (int)keyValuePair.Value.Position.Y))
				{
					list.Add(keyValuePair.Value);
				}
			}
			try
			{
				foreach (TileEntity entity2 in list)
				{
					TileEntity.Remove(entity2, false);
				}
			}
			catch
			{
			}
			foreach (TileEntity tileEntity2 in TileEntity.ByID.Values)
			{
				tileEntity2.OnWorldLoaded();
			}
		}

		// Token: 0x0600153B RID: 5435 RVA: 0x004C2094 File Offset: 0x004C0294
		public static int SaveWeightedPressurePlates(BinaryWriter writer)
		{
			object entityCreationLock = PressurePlateHelper.EntityCreationLock;
			lock (entityCreationLock)
			{
				writer.Write(PressurePlateHelper.PressurePlatesPressed.Count);
				foreach (KeyValuePair<Point, bool[]> keyValuePair in PressurePlateHelper.PressurePlatesPressed)
				{
					writer.Write(keyValuePair.Key.X);
					writer.Write(keyValuePair.Key.Y);
				}
			}
			return (int)writer.BaseStream.Position;
		}

		// Token: 0x0600153C RID: 5436 RVA: 0x004C2148 File Offset: 0x004C0348
		public static void LoadWeightedPressurePlates(BinaryReader reader)
		{
			PressurePlateHelper.Reset();
			PressurePlateHelper.NeedsFirstUpdate = true;
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				Point key = new Point(reader.ReadInt32(), reader.ReadInt32());
				PressurePlateHelper.PressurePlatesPressed.Add(key, new bool[255]);
			}
		}

		// Token: 0x0600153D RID: 5437 RVA: 0x004C219B File Offset: 0x004C039B
		public static int SaveTownManager(BinaryWriter writer)
		{
			WorldGen.TownManager.Save(writer);
			return (int)writer.BaseStream.Position;
		}

		// Token: 0x0600153E RID: 5438 RVA: 0x004C21B4 File Offset: 0x004C03B4
		public static void LoadTownManager(BinaryReader reader)
		{
			WorldGen.TownManager.Load(reader);
		}

		// Token: 0x0600153F RID: 5439 RVA: 0x004C21C1 File Offset: 0x004C03C1
		public static int SaveBestiary(BinaryWriter writer)
		{
			Main.BestiaryTracker.Save(writer);
			return (int)writer.BaseStream.Position;
		}

		// Token: 0x06001540 RID: 5440 RVA: 0x004C21DA File Offset: 0x004C03DA
		public static void LoadBestiary(BinaryReader reader, int loadVersionNumber)
		{
			Main.BestiaryTracker.Load(reader, loadVersionNumber);
		}

		// Token: 0x06001541 RID: 5441 RVA: 0x004C21E8 File Offset: 0x004C03E8
		private static void LoadBestiaryForVersionsBefore210()
		{
			Main.BestiaryTracker.FillBasedOnVersionBefore210();
		}

		// Token: 0x06001542 RID: 5442 RVA: 0x004C21F4 File Offset: 0x004C03F4
		public static int SaveCreativePowers(BinaryWriter writer)
		{
			CreativePowerManager.Instance.SaveToWorld(writer);
			return (int)writer.BaseStream.Position;
		}

		// Token: 0x06001543 RID: 5443 RVA: 0x004C220D File Offset: 0x004C040D
		public static void LoadCreativePowers(BinaryReader reader, int loadVersionNumber)
		{
			CreativePowerManager.Instance.LoadFromWorld(reader, loadVersionNumber);
		}

		// Token: 0x06001544 RID: 5444 RVA: 0x004C221C File Offset: 0x004C041C
		private static int LoadWorld_Version1_Old_BeforeRelease88(BinaryReader fileIO)
		{
			Main.WorldFileMetadata = FileMetadata.FromCurrentSettings(FileType.World);
			int versionNumber = WorldFile._versionNumber;
			if (versionNumber > 318)
			{
				return StatusID.LaterVersion;
			}
			Main.worldName = fileIO.ReadString();
			Main.ActiveWorldFileData.WorldId = fileIO.ReadInt32();
			Main.leftWorld = (float)fileIO.ReadInt32();
			Main.rightWorld = (float)fileIO.ReadInt32();
			Main.topWorld = (float)fileIO.ReadInt32();
			Main.bottomWorld = (float)fileIO.ReadInt32();
			Main.maxTilesY = fileIO.ReadInt32();
			Main.maxTilesX = fileIO.ReadInt32();
			if (versionNumber >= 112)
			{
				Main.GameMode = (fileIO.ReadBoolean() ? 1 : 0);
			}
			else
			{
				Main.GameMode = 0;
			}
			if (versionNumber >= 63)
			{
				Main.moonType = (int)fileIO.ReadByte();
			}
			else
			{
				WorldGen.RandomizeMoonState(WorldGen.genRand, false);
			}
			WorldGen.clearWorld();
			if (versionNumber >= 44)
			{
				Main.treeX[0] = fileIO.ReadInt32();
				Main.treeX[1] = fileIO.ReadInt32();
				Main.treeX[2] = fileIO.ReadInt32();
				Main.treeStyle[0] = fileIO.ReadInt32();
				Main.treeStyle[1] = fileIO.ReadInt32();
				Main.treeStyle[2] = fileIO.ReadInt32();
				Main.treeStyle[3] = fileIO.ReadInt32();
			}
			if (versionNumber >= 60)
			{
				Main.caveBackX[0] = fileIO.ReadInt32();
				Main.caveBackX[1] = fileIO.ReadInt32();
				Main.caveBackX[2] = fileIO.ReadInt32();
				Main.caveBackStyle[0] = fileIO.ReadInt32();
				Main.caveBackStyle[1] = fileIO.ReadInt32();
				Main.caveBackStyle[2] = fileIO.ReadInt32();
				Main.caveBackStyle[3] = fileIO.ReadInt32();
				Main.iceBackStyle = fileIO.ReadInt32();
				if (versionNumber >= 61)
				{
					Main.jungleBackStyle = fileIO.ReadInt32();
					Main.hellBackStyle = fileIO.ReadInt32();
				}
			}
			else
			{
				WorldGen.RandomizeCaveBackgrounds();
			}
			Main.spawnTileX = fileIO.ReadInt32();
			Main.spawnTileY = fileIO.ReadInt32();
			Main.worldSurface = fileIO.ReadDouble();
			Main.rockLayer = fileIO.ReadDouble();
			WorldFile._tempTime = fileIO.ReadDouble();
			WorldFile._tempDayTime = fileIO.ReadBoolean();
			WorldFile._tempMoonPhase = fileIO.ReadInt32();
			WorldFile._tempBloodMoon = fileIO.ReadBoolean();
			if (versionNumber >= 70)
			{
				WorldFile._tempEclipse = fileIO.ReadBoolean();
				Main.eclipse = WorldFile._tempEclipse;
			}
			Main.dungeonX = fileIO.ReadInt32();
			Main.dungeonY = fileIO.ReadInt32();
			if (versionNumber >= 56)
			{
				WorldGen.crimson = fileIO.ReadBoolean();
			}
			else
			{
				WorldGen.crimson = false;
			}
			NPC.downedBoss1 = fileIO.ReadBoolean();
			NPC.downedBoss2 = fileIO.ReadBoolean();
			NPC.downedBoss3 = fileIO.ReadBoolean();
			if (versionNumber >= 66)
			{
				NPC.downedQueenBee = fileIO.ReadBoolean();
			}
			if (versionNumber >= 44)
			{
				NPC.downedMechBoss1 = fileIO.ReadBoolean();
				NPC.downedMechBoss2 = fileIO.ReadBoolean();
				NPC.downedMechBoss3 = fileIO.ReadBoolean();
				NPC.downedMechBossAny = fileIO.ReadBoolean();
			}
			if (versionNumber >= 64)
			{
				NPC.downedPlantBoss = fileIO.ReadBoolean();
				NPC.downedGolemBoss = fileIO.ReadBoolean();
			}
			if (versionNumber >= 29)
			{
				NPC.savedGoblin = fileIO.ReadBoolean();
				NPC.savedWizard = fileIO.ReadBoolean();
				if (versionNumber >= 34)
				{
					NPC.savedMech = fileIO.ReadBoolean();
					if (versionNumber >= 80)
					{
						NPC.savedStylist = fileIO.ReadBoolean();
					}
				}
				if (versionNumber >= 129)
				{
					NPC.savedTaxCollector = fileIO.ReadBoolean();
				}
				if (versionNumber >= 201)
				{
					NPC.savedGolfer = fileIO.ReadBoolean();
				}
				NPC.downedGoblins = fileIO.ReadBoolean();
			}
			if (versionNumber >= 32)
			{
				NPC.downedClown = fileIO.ReadBoolean();
			}
			if (versionNumber >= 37)
			{
				NPC.downedFrost = fileIO.ReadBoolean();
			}
			if (versionNumber >= 56)
			{
				NPC.downedPirates = fileIO.ReadBoolean();
			}
			WorldGen.shadowOrbSmashed = fileIO.ReadBoolean();
			WorldGen.spawnMeteor = fileIO.ReadBoolean();
			WorldGen.shadowOrbCount = (int)fileIO.ReadByte();
			if (versionNumber >= 23)
			{
				WorldGen.altarCount = fileIO.ReadInt32();
				Main.hardMode = fileIO.ReadBoolean();
			}
			Main.invasionDelay = fileIO.ReadInt32();
			Main.invasionSize = fileIO.ReadInt32();
			Main.invasionType = fileIO.ReadInt32();
			Main.invasionX = fileIO.ReadDouble();
			if (versionNumber >= 113)
			{
				Main.sundialCooldown = (int)fileIO.ReadByte();
			}
			if (versionNumber >= 53)
			{
				WorldFile._tempRaining = fileIO.ReadBoolean();
				WorldFile._tempRainTime = fileIO.ReadInt32();
				WorldFile._tempMaxRain = fileIO.ReadSingle();
			}
			if (versionNumber >= 54)
			{
				WorldGen.SavedOreTiers.Cobalt = fileIO.ReadInt32();
				WorldGen.SavedOreTiers.Mythril = fileIO.ReadInt32();
				WorldGen.SavedOreTiers.Adamantite = fileIO.ReadInt32();
			}
			else if (versionNumber >= 23 && WorldGen.altarCount == 0)
			{
				WorldGen.SavedOreTiers.Cobalt = -1;
				WorldGen.SavedOreTiers.Mythril = -1;
				WorldGen.SavedOreTiers.Adamantite = -1;
			}
			else
			{
				WorldGen.SavedOreTiers.Cobalt = 107;
				WorldGen.SavedOreTiers.Mythril = 108;
				WorldGen.SavedOreTiers.Adamantite = 111;
			}
			int style = 0;
			int style2 = 0;
			int style3 = 0;
			int style4 = 0;
			int style5 = 0;
			int style6 = 0;
			int style7 = 0;
			int style8 = 0;
			int style9 = 0;
			int style10 = 0;
			if (versionNumber >= 55)
			{
				style = (int)fileIO.ReadByte();
				style2 = (int)fileIO.ReadByte();
				style3 = (int)fileIO.ReadByte();
			}
			if (versionNumber >= 60)
			{
				style4 = (int)fileIO.ReadByte();
				style5 = (int)fileIO.ReadByte();
				style6 = (int)fileIO.ReadByte();
				style7 = (int)fileIO.ReadByte();
				style8 = (int)fileIO.ReadByte();
			}
			WorldGen.setBG(0, style);
			WorldGen.setBG(1, style2);
			WorldGen.setBG(2, style3);
			WorldGen.setBG(3, style4);
			WorldGen.setBG(4, style5);
			WorldGen.setBG(5, style6);
			WorldGen.setBG(6, style7);
			WorldGen.setBG(7, style8);
			WorldGen.setBG(8, style9);
			WorldGen.setBG(9, style10);
			WorldGen.setBG(10, style);
			WorldGen.setBG(11, style);
			WorldGen.setBG(12, style);
			if (versionNumber >= 60)
			{
				Main.cloudBGActive = (float)fileIO.ReadInt32();
				if (Main.cloudBGActive >= 1f)
				{
					Main.cloudBGAlpha = 1f;
				}
				else
				{
					Main.cloudBGAlpha = 0f;
				}
			}
			else
			{
				Main.cloudBGActive = (float)(-(float)WorldGen.genRand.Next(8640, 86400));
			}
			if (versionNumber >= 62)
			{
				Main.numClouds = (int)fileIO.ReadInt16();
				Main.windSpeedTarget = fileIO.ReadSingle();
				Main.windSpeedCurrent = Main.windSpeedTarget;
			}
			else
			{
				WorldGen.RandomizeWeather();
			}
			for (int i = 0; i < Main.maxTilesX; i++)
			{
				float num = (float)i / (float)Main.maxTilesX;
				Main.statusText = string.Concat(new object[]
				{
					Lang.gen[51].Value,
					" ",
					(int)(num * 100f + 1f),
					"%"
				});
				for (int j = 0; j < Main.maxTilesY; j++)
				{
					Tile tile = Main.tile[i, j];
					int num2 = -1;
					tile.active(fileIO.ReadBoolean());
					if (tile.active())
					{
						if (versionNumber > 77)
						{
							num2 = (int)fileIO.ReadUInt16();
						}
						else
						{
							num2 = (int)fileIO.ReadByte();
						}
						tile.type = (ushort)num2;
						if (tile.type == 127 || tile.type == 504)
						{
							tile.active(false);
						}
						if (versionNumber < 72 && (tile.type == 35 || tile.type == 36 || tile.type == 170 || tile.type == 171 || tile.type == 172))
						{
							tile.frameX = fileIO.ReadInt16();
							tile.frameY = fileIO.ReadInt16();
						}
						else if (Main.tileFrameImportant[num2])
						{
							if (versionNumber < 28 && num2 == 4)
							{
								tile.frameX = 0;
								tile.frameY = 0;
							}
							else if (versionNumber < 40 && tile.type == 19)
							{
								tile.frameX = 0;
								tile.frameY = 0;
							}
							else if (versionNumber < 195 && tile.type == 49)
							{
								tile.frameX = 0;
								tile.frameY = 0;
							}
							else
							{
								tile.frameX = fileIO.ReadInt16();
								tile.frameY = fileIO.ReadInt16();
								if (tile.type == 144)
								{
									tile.frameY = 0;
								}
							}
						}
						else
						{
							tile.frameX = -1;
							tile.frameY = -1;
						}
						if (versionNumber >= 48 && fileIO.ReadBoolean())
						{
							tile.color(fileIO.ReadByte());
						}
					}
					if (versionNumber <= 25)
					{
						fileIO.ReadBoolean();
					}
					if (fileIO.ReadBoolean())
					{
						tile.wall = (ushort)fileIO.ReadByte();
						if (tile.wall >= WallID.Count)
						{
							tile.wall = 0;
						}
						if (versionNumber >= 48 && fileIO.ReadBoolean())
						{
							tile.wallColor(fileIO.ReadByte());
						}
					}
					if (fileIO.ReadBoolean())
					{
						tile.liquid = fileIO.ReadByte();
						tile.lava(fileIO.ReadBoolean());
						if (versionNumber >= 51)
						{
							tile.honey(fileIO.ReadBoolean());
						}
					}
					if (versionNumber >= 33)
					{
						tile.wire(fileIO.ReadBoolean());
					}
					if (versionNumber >= 43)
					{
						tile.wire2(fileIO.ReadBoolean());
						tile.wire3(fileIO.ReadBoolean());
					}
					if (versionNumber >= 41)
					{
						tile.halfBrick(fileIO.ReadBoolean());
						if (!TileID.Sets.SaveSlopes[(int)tile.type])
						{
							tile.halfBrick(false);
						}
						if (versionNumber >= 49)
						{
							tile.slope(fileIO.ReadByte());
							if (!TileID.Sets.SaveSlopes[(int)tile.type])
							{
								tile.slope(0);
							}
						}
					}
					if (versionNumber >= 42)
					{
						tile.actuator(fileIO.ReadBoolean());
						tile.inActive(fileIO.ReadBoolean());
					}
					int num3 = 0;
					if (versionNumber >= 25)
					{
						num3 = (int)fileIO.ReadInt16();
					}
					if (num2 != -1)
					{
						if ((double)j <= Main.worldSurface)
						{
							if ((double)(j + num3) <= Main.worldSurface)
							{
								WorldGen.tileCounts[num2] += (num3 + 1) * 5;
							}
							else
							{
								int num4 = (int)(Main.worldSurface - (double)j + 1.0);
								int num5 = num3 + 1 - num4;
								WorldGen.tileCounts[num2] += num4 * 5 + num5;
							}
						}
						else
						{
							WorldGen.tileCounts[num2] += num3 + 1;
						}
					}
					if (num3 > 0)
					{
						for (int k = j + 1; k < j + num3 + 1; k++)
						{
							Main.tile[i, k].CopyFrom(Main.tile[i, j]);
						}
						j += num3;
					}
				}
			}
			WorldGen.AddUpAlignmentCounts(true);
			if (versionNumber < 67)
			{
				WorldGen.FixSunflowers();
			}
			if (versionNumber < 72)
			{
				WorldGen.FixChands();
			}
			int num6 = 40;
			if (versionNumber < 58)
			{
				num6 = 20;
			}
			int num7 = 1000;
			for (int l = 0; l < num7; l++)
			{
				if (fileIO.ReadBoolean())
				{
					int x = fileIO.ReadInt32();
					int y = fileIO.ReadInt32();
					Chest chest = Chest.CreateWorldChest(l, x, y);
					if (versionNumber >= 85)
					{
						chest.name = Utils.TrimUserString(fileIO.ReadString(), 20);
					}
					for (int m = 0; m < 40; m++)
					{
						if (m < num6)
						{
							int num8;
							if (versionNumber >= 59)
							{
								num8 = (int)fileIO.ReadInt16();
							}
							else
							{
								num8 = (int)fileIO.ReadByte();
							}
							if (num8 > 0)
							{
								if (versionNumber >= 38)
								{
									chest.item[m].netDefaults(fileIO.ReadInt32());
								}
								else
								{
									short type = ItemID.FromLegacyName(fileIO.ReadString(), versionNumber);
									chest.item[m].SetDefaults((int)type, null);
								}
								chest.item[m].stack = num8;
								if (versionNumber >= 36)
								{
									chest.item[m].Prefix((int)fileIO.ReadByte());
								}
							}
						}
					}
				}
			}
			for (int n = 0; n < 1000; n++)
			{
				if (fileIO.ReadBoolean())
				{
					string text = fileIO.ReadString();
					int num9 = fileIO.ReadInt32();
					int num10 = fileIO.ReadInt32();
					if (Main.tile[num9, num10].active() && (Main.tile[num9, num10].type == 55 || Main.tile[num9, num10].type == 85))
					{
						Main.sign[n] = new Sign();
						Main.sign[n].x = num9;
						Main.sign[n].y = num10;
						Main.sign[n].text = text;
					}
				}
			}
			bool flag = fileIO.ReadBoolean();
			int num11 = 0;
			while (flag)
			{
				if (versionNumber >= 190)
				{
					Main.npc[num11].SetDefaults(fileIO.ReadInt32(), default(NPCSpawnParams));
				}
				else
				{
					Main.npc[num11].SetDefaults(NPCID.FromLegacyName(fileIO.ReadString()), default(NPCSpawnParams));
				}
				if (versionNumber >= 83)
				{
					Main.npc[num11].GivenName = fileIO.ReadString();
				}
				Main.npc[num11].position.X = fileIO.ReadSingle();
				Main.npc[num11].position.Y = fileIO.ReadSingle();
				Main.npc[num11].homeless = fileIO.ReadBoolean();
				Main.npc[num11].homeTileX = fileIO.ReadInt32();
				Main.npc[num11].homeTileY = fileIO.ReadInt32();
				flag = fileIO.ReadBoolean();
				num11++;
			}
			if (versionNumber >= 31 && versionNumber <= 83)
			{
				NPC.setNPCName(fileIO.ReadString(), 17, true);
				NPC.setNPCName(fileIO.ReadString(), 18, true);
				NPC.setNPCName(fileIO.ReadString(), 19, true);
				NPC.setNPCName(fileIO.ReadString(), 20, true);
				NPC.setNPCName(fileIO.ReadString(), 22, true);
				NPC.setNPCName(fileIO.ReadString(), 54, true);
				NPC.setNPCName(fileIO.ReadString(), 38, true);
				NPC.setNPCName(fileIO.ReadString(), 107, true);
				NPC.setNPCName(fileIO.ReadString(), 108, true);
				if (versionNumber >= 35)
				{
					NPC.setNPCName(fileIO.ReadString(), 124, true);
					if (versionNumber >= 65)
					{
						NPC.setNPCName(fileIO.ReadString(), 160, true);
						NPC.setNPCName(fileIO.ReadString(), 178, true);
						NPC.setNPCName(fileIO.ReadString(), 207, true);
						NPC.setNPCName(fileIO.ReadString(), 208, true);
						NPC.setNPCName(fileIO.ReadString(), 209, true);
						NPC.setNPCName(fileIO.ReadString(), 227, true);
						NPC.setNPCName(fileIO.ReadString(), 228, true);
						NPC.setNPCName(fileIO.ReadString(), 229, true);
						if (versionNumber >= 79)
						{
							NPC.setNPCName(fileIO.ReadString(), 353, true);
						}
					}
				}
			}
			if (Main.invasionType > 0 && Main.invasionSize > 0)
			{
				Main.FakeLoadInvasionStart();
			}
			if (versionNumber < 7)
			{
				return StatusID.Ok;
			}
			bool flag2 = fileIO.ReadBoolean();
			string a = fileIO.ReadString();
			int num12 = fileIO.ReadInt32();
			if (flag2 && (a == Main.worldName || num12 == Main.worldID))
			{
				return StatusID.Ok;
			}
			return StatusID.UnknownError;
		}

		// Token: 0x040010B6 RID: 4278
		internal static readonly object IOLock = new object();

		// Token: 0x040010B7 RID: 4279
		private static bool _reuseTempsForNextSave;

		// Token: 0x040010B8 RID: 4280
		private static double _tempTime = Main.time;

		// Token: 0x040010B9 RID: 4281
		private static bool _tempRaining;

		// Token: 0x040010BA RID: 4282
		private static float _tempMaxRain;

		// Token: 0x040010BB RID: 4283
		private static int _tempRainTime;

		// Token: 0x040010BC RID: 4284
		private static bool _tempDayTime = Main.dayTime;

		// Token: 0x040010BD RID: 4285
		private static bool _tempBloodMoon = Main.bloodMoon;

		// Token: 0x040010BE RID: 4286
		private static bool _tempEclipse = Main.eclipse;

		// Token: 0x040010BF RID: 4287
		private static int _tempMoonPhase = Main.moonPhase;

		// Token: 0x040010C0 RID: 4288
		private static int _tempCultistDelay = CultistRitual.delay;

		// Token: 0x040010C1 RID: 4289
		private static int _versionNumber;

		// Token: 0x040010C2 RID: 4290
		private static bool _isWorldOnCloud;

		// Token: 0x040010C3 RID: 4291
		private static bool _tempPartyGenuine;

		// Token: 0x040010C4 RID: 4292
		private static bool _tempPartyManual;

		// Token: 0x040010C5 RID: 4293
		private static int _tempPartyCooldown;

		// Token: 0x040010C6 RID: 4294
		private static readonly List<int> TempPartyCelebratingNPCs = new List<int>();

		// Token: 0x040010C7 RID: 4295
		private static bool _tempSandstormHappening;

		// Token: 0x040010C8 RID: 4296
		private static int _tempSandstormTimeLeft;

		// Token: 0x040010C9 RID: 4297
		private static float _tempSandstormSeverity;

		// Token: 0x040010CA RID: 4298
		private static float _tempSandstormIntendedSeverity;

		// Token: 0x040010CB RID: 4299
		private static bool _tempLanternNightGenuine;

		// Token: 0x040010CC RID: 4300
		private static bool _tempLanternNightManual;

		// Token: 0x040010CD RID: 4301
		private static bool _tempLanternNightNextNightIsGenuine;

		// Token: 0x040010CE RID: 4302
		private static int _tempLanternNightCooldown;

		// Token: 0x040010CF RID: 4303
		private static int _tempCoinRain;

		// Token: 0x040010D0 RID: 4304
		private static int _tempMeteorShowerCount;

		// Token: 0x040010D1 RID: 4305
		public static Exception LastThrownLoadException;

		// Token: 0x040010D2 RID: 4306
		private const int VersionNumberForChestRework = 294;

		// Token: 0x02000668 RID: 1640
		public static class TilePacker
		{
			// Token: 0x04006646 RID: 26182
			public const int Header1_1 = 1;

			// Token: 0x04006647 RID: 26183
			public const int Header1_2 = 2;

			// Token: 0x04006648 RID: 26184
			public const int Header1_4 = 4;

			// Token: 0x04006649 RID: 26185
			public const int Header1_8 = 8;

			// Token: 0x0400664A RID: 26186
			public const int Header1_10 = 16;

			// Token: 0x0400664B RID: 26187
			public const int Header1_18 = 24;

			// Token: 0x0400664C RID: 26188
			public const int Header1_20 = 32;

			// Token: 0x0400664D RID: 26189
			public const int Header1_40 = 64;

			// Token: 0x0400664E RID: 26190
			public const int Header1_80 = 128;

			// Token: 0x0400664F RID: 26191
			public const int Header1_C0 = 192;

			// Token: 0x04006650 RID: 26192
			public const int Header2_1 = 1;

			// Token: 0x04006651 RID: 26193
			public const int Header2_2 = 2;

			// Token: 0x04006652 RID: 26194
			public const int Header2_4 = 4;

			// Token: 0x04006653 RID: 26195
			public const int Header2_8 = 8;

			// Token: 0x04006654 RID: 26196
			public const int Header2_10 = 16;

			// Token: 0x04006655 RID: 26197
			public const int Header2_20 = 32;

			// Token: 0x04006656 RID: 26198
			public const int Header2_40 = 64;

			// Token: 0x04006657 RID: 26199
			public const int Header2_70 = 112;

			// Token: 0x04006658 RID: 26200
			public const int Header2_80 = 128;

			// Token: 0x04006659 RID: 26201
			public const int Header3_1 = 1;

			// Token: 0x0400665A RID: 26202
			public const int Header3_2 = 2;

			// Token: 0x0400665B RID: 26203
			public const int Header3_4 = 4;

			// Token: 0x0400665C RID: 26204
			public const int Header3_8 = 8;

			// Token: 0x0400665D RID: 26205
			public const int Header3_10 = 16;

			// Token: 0x0400665E RID: 26206
			public const int Header3_20 = 32;

			// Token: 0x0400665F RID: 26207
			public const int Header3_40 = 64;

			// Token: 0x04006660 RID: 26208
			public const int Header3_80 = 128;

			// Token: 0x04006661 RID: 26209
			public const int Header4_1 = 1;

			// Token: 0x04006662 RID: 26210
			public const int Header4_2 = 2;

			// Token: 0x04006663 RID: 26211
			public const int Header4_4 = 4;

			// Token: 0x04006664 RID: 26212
			public const int Header4_8 = 8;

			// Token: 0x04006665 RID: 26213
			public const int Header4_10 = 16;

			// Token: 0x04006666 RID: 26214
			public const int Header4_20 = 32;

			// Token: 0x04006667 RID: 26215
			public const int Header4_40 = 64;

			// Token: 0x04006668 RID: 26216
			public const int Header4_80 = 128;
		}
	}
}
