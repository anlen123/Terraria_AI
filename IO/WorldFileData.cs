using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ReLogic.Utilities;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.IO
{
	// Token: 0x02000070 RID: 112
	public class WorldFileData : FileData
	{
		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x060014D7 RID: 5335 RVA: 0x004BBEE4 File Offset: 0x004BA0E4
		public string SeedText
		{
			get
			{
				return this._seedText;
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x060014D8 RID: 5336 RVA: 0x004BBEEC File Offset: 0x004BA0EC
		public int Seed
		{
			get
			{
				return this._seed;
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x060014D9 RID: 5337 RVA: 0x004BBEF4 File Offset: 0x004BA0F4
		public bool IsValid
		{
			get
			{
				return this.LoadStatus == StatusID.Ok;
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x060014DA RID: 5338 RVA: 0x004BBF03 File Offset: 0x004BA103
		public string WorldSizeName
		{
			get
			{
				return this._worldSizeName.Value;
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x060014DB RID: 5339 RVA: 0x004BBF10 File Offset: 0x004BA110
		// (set) Token: 0x060014DC RID: 5340 RVA: 0x004BBF1B File Offset: 0x004BA11B
		public bool HasCrimson
		{
			get
			{
				return !this.HasCorruption;
			}
			set
			{
				this.HasCorruption = !value;
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x060014DD RID: 5341 RVA: 0x004BBF27 File Offset: 0x004BA127
		public bool HasValidSeed
		{
			get
			{
				return this.WorldGeneratorVersion > 0UL;
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x060014DE RID: 5342 RVA: 0x004BBF33 File Offset: 0x004BA133
		public bool UseGuidAsMapName
		{
			get
			{
				return this.WorldGeneratorVersion >= 777389080577UL;
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x060014DF RID: 5343 RVA: 0x004BBF49 File Offset: 0x004BA149
		public string MapFileName
		{
			get
			{
				if (!this.UseGuidAsMapName)
				{
					return this.WorldId.ToString();
				}
				return this.UniqueId.ToString();
			}
		}

		// Token: 0x060014E0 RID: 5344 RVA: 0x004BBF70 File Offset: 0x004BA170
		public string GetWorldName(bool allowCropping = false)
		{
			string text = this.Name;
			if (text == null)
			{
				return text;
			}
			if (allowCropping)
			{
				int num = 494;
				text = FontAssets.MouseText.Value.CreateCroppedText(text, (float)num);
			}
			return text;
		}

		// Token: 0x060014E1 RID: 5345 RVA: 0x004BBFA8 File Offset: 0x004BA1A8
		public string GetFullSeedText(bool allowCropping = false)
		{
			int num = 0;
			if (this.WorldSizeX == 4200 && this.WorldSizeY == 1200)
			{
				num = 1;
			}
			if (this.WorldSizeX == 6400 && this.WorldSizeY == 1800)
			{
				num = 2;
			}
			if (this.WorldSizeX == 8400 && this.WorldSizeY == 2400)
			{
				num = 3;
			}
			int num2 = 0;
			if (this.HasCorruption)
			{
				num2 = 1;
			}
			if (this.HasCrimson)
			{
				num2 = 2;
			}
			int num3 = this.GameMode + 1;
			string text = this._seedText;
			if (allowCropping)
			{
				int num4 = 340;
				text = FontAssets.MouseText.Value.CreateCroppedText(text, (float)num4);
			}
			int serializedSeedsSum = this.GetSerializedSeedsSum();
			return string.Format("{0}.{1}.{2}.{3}.{4}", new object[]
			{
				num,
				num3,
				num2,
				serializedSeedsSum,
				text
			});
		}

		// Token: 0x060014E2 RID: 5346 RVA: 0x004BC090 File Offset: 0x004BA290
		public int GetSerializedSeedsSum()
		{
			int num = 0;
			if (this.DrunkWorld)
			{
				num++;
			}
			if (this.NotTheBees)
			{
				num += 2;
			}
			if (this.ForTheWorthy)
			{
				num += 4;
			}
			if (this.Anniversary)
			{
				num += 8;
			}
			if (this.DontStarve)
			{
				num += 16;
			}
			if (this.RemixWorld)
			{
				num += 32;
			}
			if (this.NoTrapsWorld)
			{
				num += 64;
			}
			if (this.ZenithWorld)
			{
				num += 128;
			}
			if (this.SkyblockWorld)
			{
				num += 256;
			}
			return num;
		}

		// Token: 0x060014E3 RID: 5347 RVA: 0x004BC117 File Offset: 0x004BA317
		public List<string> GetSecretSeedCodes()
		{
			if (string.IsNullOrWhiteSpace(this._seedText))
			{
				return new List<string>();
			}
			return this._seedText.Split(new char[]
			{
				'|'
			}).ToList<string>();
		}

		// Token: 0x060014E4 RID: 5348 RVA: 0x004BC148 File Offset: 0x004BA348
		private static void EnableSeedOptions(int serializedSeedSum)
		{
			for (int i = 0; i < WorldFileData.seedOptionsInOrder.Count; i++)
			{
				if ((serializedSeedSum >> i & 1) == 1)
				{
					WorldFileData.seedOptionsInOrder[i].Enabled = true;
				}
			}
		}

		// Token: 0x060014E5 RID: 5349 RVA: 0x004BC188 File Offset: 0x004BA388
		public static bool TryApplyingCopiedSeed(string input, bool playSound, out string processedSeed, out string seedTextIncludingSecrets, out List<string> secretSeedTexts)
		{
			processedSeed = input;
			seedTextIncludingSecrets = input;
			secretSeedTexts = null;
			if (string.IsNullOrWhiteSpace(input))
			{
				return false;
			}
			int num;
			int num2;
			int num3;
			if (!WorldFileData.TryParseSeedOptionValue(ref processedSeed, out num) || !WorldFileData.TryParseSeedOptionValue(ref processedSeed, out num2) || !WorldFileData.TryParseSeedOptionValue(ref processedSeed, out num3))
			{
				return false;
			}
			if (num <= 0 || num > 3)
			{
				return false;
			}
			if (num2 <= 0 || num2 > 4)
			{
				return false;
			}
			if (num3 <= 0 || num3 > 2)
			{
				return false;
			}
			int serializedSeedSum;
			if (!WorldFileData.TryParseSeedOptionValue(ref processedSeed, out serializedSeedSum))
			{
				serializedSeedSum = 0;
			}
			seedTextIncludingSecrets = processedSeed;
			secretSeedTexts = new List<string>();
			List<WorldGen.SecretSeed> list = new List<WorldGen.SecretSeed>();
			string item;
			WorldGen.SecretSeed item2;
			while (WorldFileData.TryParseSecretSeed(ref processedSeed, out item, out item2))
			{
				secretSeedTexts.Add(item);
				list.Add(item2);
			}
			if (processedSeed.Length > WorldFileData.MAX_USER_SEED_TEXT_LENGTH)
			{
				return false;
			}
			WorldGen.SetWorldSize(num - 1);
			Main.GameMode = num2 - 1;
			WorldGen.WorldGenParam_Evil = num3 - 1;
			WorldGenerationOptions.Reset();
			WorldFileData.EnableSeedOptions(serializedSeedSum);
			WorldGen.SecretSeed.ClearAllSeeds();
			foreach (WorldGen.SecretSeed seed in list)
			{
				WorldGen.SecretSeed.Enable(seed, playSound);
				playSound = false;
			}
			return true;
		}

		// Token: 0x060014E6 RID: 5350 RVA: 0x004BC2A4 File Offset: 0x004BA4A4
		private static bool TryParseSeedOptionValue(ref string processedSeed, out int value)
		{
			int num = processedSeed.IndexOf('.');
			if (num < 0)
			{
				value = 0;
				return false;
			}
			if (!int.TryParse(processedSeed.Substring(0, num), out value))
			{
				return false;
			}
			processedSeed = processedSeed.Substring(num + 1);
			return true;
		}

		// Token: 0x060014E7 RID: 5351 RVA: 0x004BC2E4 File Offset: 0x004BA4E4
		private static bool TryParseSecretSeed(ref string processedSeed, out string secretSeedText, out WorldGen.SecretSeed secretSeed)
		{
			int num = processedSeed.IndexOf('|');
			if (num < 0)
			{
				secretSeedText = null;
				secretSeed = null;
				return false;
			}
			secretSeedText = processedSeed.Substring(0, num);
			if (!WorldGen.SecretSeed.CheckInputForSecretSeed(secretSeedText, out secretSeed))
			{
				return false;
			}
			processedSeed = processedSeed.Substring(num + 1);
			return true;
		}

		// Token: 0x060014E8 RID: 5352 RVA: 0x004BC32B File Offset: 0x004BA52B
		public WorldFileData() : base("World")
		{
		}

		// Token: 0x060014E9 RID: 5353 RVA: 0x004BC355 File Offset: 0x004BA555
		public WorldFileData(string path, bool cloudSave) : base("World", path, cloudSave)
		{
		}

		// Token: 0x060014EA RID: 5354 RVA: 0x004BC381 File Offset: 0x004BA581
		public override void SetAsActive()
		{
			if (this.LoadException != null)
			{
				throw this.LoadException;
			}
			Main.ActiveWorldFileData = this;
		}

		// Token: 0x060014EB RID: 5355 RVA: 0x004BC398 File Offset: 0x004BA598
		public void SetWorldSize(int x, int y)
		{
			this.WorldSizeX = x;
			this.WorldSizeY = y;
			if (x == 4200)
			{
				this._worldSizeName = Language.GetText("UI.WorldSizeSmall");
				return;
			}
			if (x == 6400)
			{
				this._worldSizeName = Language.GetText("UI.WorldSizeMedium");
				return;
			}
			if (x != 8400)
			{
				this._worldSizeName = Language.GetText("UI.WorldSizeUnknown");
				return;
			}
			this._worldSizeName = Language.GetText("UI.WorldSizeLarge");
		}

		// Token: 0x060014EC RID: 5356 RVA: 0x004BC410 File Offset: 0x004BA610
		public static WorldFileData FromInvalidWorld(string path, bool cloudSave, int statusCode, Exception exception)
		{
			WorldFileData worldFileData = new WorldFileData(path, cloudSave);
			worldFileData.GameMode = 0;
			worldFileData.SetSeedToEmpty();
			worldFileData.WorldGeneratorVersion = 0UL;
			worldFileData.Metadata = FileMetadata.FromCurrentSettings(FileType.World);
			worldFileData.SetWorldSize(1, 1);
			worldFileData.HasCorruption = true;
			worldFileData.IsHardMode = false;
			worldFileData.LoadStatus = statusCode;
			worldFileData.LoadException = exception;
			worldFileData.Name = FileUtilities.GetFileName(path, false);
			worldFileData.UniqueId = Guid.Empty;
			if (!cloudSave)
			{
				worldFileData.CreationTime = File.GetCreationTime(path);
			}
			else
			{
				worldFileData.CreationTime = DateTime.Now;
			}
			return worldFileData;
		}

		// Token: 0x060014ED RID: 5357 RVA: 0x004BC49F File Offset: 0x004BA69F
		public void SetSeedToEmpty()
		{
			this.SetSeed("");
		}

		// Token: 0x060014EE RID: 5358 RVA: 0x004BC4AC File Offset: 0x004BA6AC
		public void SetSeed(string seedText)
		{
			this._seedText = seedText;
			this._seed = WorldFileData.TranslateSeed(seedText);
		}

		// Token: 0x060014EF RID: 5359 RVA: 0x004BC4C4 File Offset: 0x004BA6C4
		public static int TranslateSeed(string seedText)
		{
			int num;
			if (!int.TryParse(seedText, out num))
			{
				return Crc32.Calculate(seedText);
			}
			if (num != -2147483648)
			{
				return Math.Abs(num);
			}
			return int.MaxValue;
		}

		// Token: 0x060014F0 RID: 5360 RVA: 0x004BC4F8 File Offset: 0x004BA6F8
		public void SetSeedToRandom()
		{
			this.SetSeed(new UnifiedRandom().Next().ToString());
		}

		// Token: 0x060014F1 RID: 5361 RVA: 0x004BC51D File Offset: 0x004BA71D
		public void SetSeedToRandomWithCurrentEvents()
		{
			this.SetSeedToRandom();
			if (Main.isHalloweenDateNow())
			{
				WorldGen.SecretSeed.Enable(WorldGen.SecretSeed.halloweenGen, false);
				this.SetSeed("pumpkinseason|" + this.SeedText);
			}
		}

		// Token: 0x060014F2 RID: 5362 RVA: 0x004BC550 File Offset: 0x004BA750
		public override void MoveToCloud()
		{
			if (base.IsCloudSave)
			{
				return;
			}
			string worldPathFromName = Main.GetWorldPathFromName(this.Name, true);
			if (FileUtilities.MoveToCloud(base.Path, worldPathFromName))
			{
				Main.LocalFavoriteData.ClearEntry(this);
				this._isCloudSave = true;
				this._path = worldPathFromName;
				Main.CloudFavoritesData.SaveFavorite(this);
			}
		}

		// Token: 0x060014F3 RID: 5363 RVA: 0x004BC5A8 File Offset: 0x004BA7A8
		public override void MoveToLocal()
		{
			if (!base.IsCloudSave)
			{
				return;
			}
			string worldPathFromName = Main.GetWorldPathFromName(this.Name, false);
			if (FileUtilities.MoveToLocal(base.Path, worldPathFromName))
			{
				Main.CloudFavoritesData.ClearEntry(this);
				this._isCloudSave = false;
				this._path = worldPathFromName;
				Main.LocalFavoriteData.SaveFavorite(this);
			}
		}

		// Token: 0x060014F4 RID: 5364 RVA: 0x004BC5FD File Offset: 0x004BA7FD
		public void Rename(string newDisplayName)
		{
			if (newDisplayName == null)
			{
				return;
			}
			WorldGen.RenameWorld(this, newDisplayName, this.GetRenameCallback(delegate
			{
				Main.menuMode = 6;
			}));
		}

		// Token: 0x060014F5 RID: 5365 RVA: 0x004BC630 File Offset: 0x004BA830
		public void CopyToLocal(string newDisplayName, Action onCompleted)
		{
			if (base.IsCloudSave)
			{
				return;
			}
			string worldPathFromName = Main.GetWorldPathFromName(Guid.NewGuid().ToString(), false);
			FileUtilities.Copy(base.Path, worldPathFromName, false);
			this._path = worldPathFromName;
			WorldGen.RenameWorld(this, newDisplayName, this.GetRenameCallback(onCompleted));
		}

		// Token: 0x060014F6 RID: 5366 RVA: 0x004BC682 File Offset: 0x004BA882
		private Action<string> GetRenameCallback(Action returnToMenu)
		{
			Action <>9__1;
			return delegate(string newWorldName)
			{
				this.Name = newWorldName;
				Action action;
				if ((action = <>9__1) == null)
				{
					action = (<>9__1 = delegate()
					{
						SoundEngine.PlaySound(10, -1, -1, 1, 1f, 0f);
						returnToMenu();
					});
				}
				Main.QueueMainThreadAction(action);
			};
		}

		// Token: 0x04001086 RID: 4230
		private const ulong GUID_IN_WORLD_FILE_VERSION = 777389080577UL;

		// Token: 0x04001087 RID: 4231
		public static readonly int MAX_USER_SEED_TEXT_LENGTH = 40;

		// Token: 0x04001088 RID: 4232
		public DateTime CreationTime;

		// Token: 0x04001089 RID: 4233
		public DateTime LastPlayed;

		// Token: 0x0400108A RID: 4234
		public int WorldSizeX;

		// Token: 0x0400108B RID: 4235
		public int WorldSizeY;

		// Token: 0x0400108C RID: 4236
		public ulong WorldGeneratorVersion;

		// Token: 0x0400108D RID: 4237
		private string _seedText = "";

		// Token: 0x0400108E RID: 4238
		private int _seed;

		// Token: 0x0400108F RID: 4239
		public int LoadStatus = StatusID.Ok;

		// Token: 0x04001090 RID: 4240
		public Exception LoadException;

		// Token: 0x04001091 RID: 4241
		public Guid UniqueId;

		// Token: 0x04001092 RID: 4242
		public int WorldId;

		// Token: 0x04001093 RID: 4243
		public LocalizedText _worldSizeName;

		// Token: 0x04001094 RID: 4244
		public int GameMode;

		// Token: 0x04001095 RID: 4245
		public bool DrunkWorld;

		// Token: 0x04001096 RID: 4246
		public bool NotTheBees;

		// Token: 0x04001097 RID: 4247
		public bool ForTheWorthy;

		// Token: 0x04001098 RID: 4248
		public bool Anniversary;

		// Token: 0x04001099 RID: 4249
		public bool DontStarve;

		// Token: 0x0400109A RID: 4250
		public bool RemixWorld;

		// Token: 0x0400109B RID: 4251
		public bool NoTrapsWorld;

		// Token: 0x0400109C RID: 4252
		public bool ZenithWorld;

		// Token: 0x0400109D RID: 4253
		public bool SkyblockWorld;

		// Token: 0x0400109E RID: 4254
		public bool HasCorruption = true;

		// Token: 0x0400109F RID: 4255
		public bool IsHardMode;

		// Token: 0x040010A0 RID: 4256
		public bool DefeatedMoonlord;

		// Token: 0x040010A1 RID: 4257
		private static List<AWorldGenerationOption> seedOptionsInOrder = new List<AWorldGenerationOption>
		{
			WorldGenerationOptions.Get<WorldSeedOption_Drunk>(),
			WorldGenerationOptions.Get<WorldSeedOption_NotTheBees>(),
			WorldGenerationOptions.Get<WorldSeedOption_ForTheWorthy>(),
			WorldGenerationOptions.Get<WorldSeedOption_Anniversary>(),
			WorldGenerationOptions.Get<WorldSeedOption_DontStarve>(),
			WorldGenerationOptions.Get<WorldSeedOption_Remix>(),
			WorldGenerationOptions.Get<WorldSeedOption_NoTraps>(),
			WorldGenerationOptions.Get<WorldSeedOption_Everything>(),
			WorldGenerationOptions.Get<WorldSeedOption_Skyblock>()
		};
	}
}
