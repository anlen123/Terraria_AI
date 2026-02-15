using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Terraria.IO;

namespace Terraria.WorldBuilding
{
	// Token: 0x02000097 RID: 151
	public class WorldGenerationOptions
	{
		// Token: 0x1700025F RID: 607
		// (get) Token: 0x060016E2 RID: 5858 RVA: 0x004DC882 File Offset: 0x004DAA82
		public static IEnumerable<AWorldGenerationOption> Options
		{
			get
			{
				return WorldGenerationOptions._options;
			}
		}

		// Token: 0x060016E3 RID: 5859 RVA: 0x004DC889 File Offset: 0x004DAA89
		public static T Get<T>() where T : AWorldGenerationOption
		{
			return WorldGenerationOptions.OptionStorage<T>.Instance;
		}

		// Token: 0x060016E4 RID: 5860 RVA: 0x004DC890 File Offset: 0x004DAA90
		static WorldGenerationOptions()
		{
			WorldGenerationOptions.Register<WorldSeedOption_Normal>();
			WorldGenerationOptions.Register<WorldSeedOption_NotTheBees>();
			WorldGenerationOptions.Register<WorldSeedOption_Drunk>();
			WorldGenerationOptions.Register<WorldSeedOption_Anniversary>();
			WorldGenerationOptions.Register<WorldSeedOption_DontStarve>();
			WorldGenerationOptions.Register<WorldSeedOption_ForTheWorthy>();
			WorldGenerationOptions.Register<WorldSeedOption_NoTraps>();
			WorldGenerationOptions.Register<WorldSeedOption_Remix>();
			WorldGenerationOptions.Register<WorldSeedOption_Everything>();
			WorldGenerationOptions.Register<WorldSeedOption_Skyblock>();
		}

		// Token: 0x060016E5 RID: 5861 RVA: 0x004DC8D0 File Offset: 0x004DAAD0
		public static void Register<T>() where T : AWorldGenerationOption, new()
		{
			if (WorldGenerationOptions.OptionStorage<T>.Instance != null)
			{
				throw new ArgumentException(typeof(T) + " has already been registered");
			}
			T t = Activator.CreateInstance<T>();
			WorldGenerationOptions.OptionStorage<T>.Instance = t;
			WorldGenerationOptions._options.Add(t);
		}

		// Token: 0x060016E6 RID: 5862 RVA: 0x004DC91F File Offset: 0x004DAB1F
		public static void Reset()
		{
			WorldGenerationOptions.Get<WorldSeedOption_Normal>().Enabled = true;
		}

		// Token: 0x060016E7 RID: 5863 RVA: 0x004DC92C File Offset: 0x004DAB2C
		public static void SelectOption(AWorldGenerationOption option)
		{
			WorldGenerationOptions.Reset();
			if (option != null)
			{
				option.Enabled = true;
			}
		}

		// Token: 0x060016E8 RID: 5864 RVA: 0x004DC940 File Offset: 0x004DAB40
		public static AWorldGenerationOption GetOptionFromSeedText(string processedSeed)
		{
			int num = WorldFileData.TranslateSeed(processedSeed);
			string a = Regex.Replace(processedSeed.ToLower(), "[^a-z0-9]+", "");
			foreach (AWorldGenerationOption aworldGenerationOption in WorldGenerationOptions.Options)
			{
				foreach (int num2 in aworldGenerationOption.SpecialSeedValues)
				{
					if (num == num2)
					{
						return aworldGenerationOption;
					}
				}
				foreach (string b in aworldGenerationOption.SpecialSeedNames)
				{
					if (a == b)
					{
						return aworldGenerationOption;
					}
				}
			}
			return null;
		}

		// Token: 0x060016E9 RID: 5865 RVA: 0x004DCA04 File Offset: 0x004DAC04
		public static void TryEnablingFlagFrom(string line)
		{
			int length = "seed_".Length;
			if (line.Length < length)
			{
				return;
			}
			if (!line.ToLower().StartsWith("seed_"))
			{
				return;
			}
			string[] array = line.Substring(length).Split(new char[]
			{
				'='
			});
			if (array.Length != 2)
			{
				return;
			}
			int value;
			if (!int.TryParse(array[1].Trim(), out value))
			{
				return;
			}
			bool autoGenEnabled = Utils.Clamp<int>(value, 0, 1) == 1;
			string namePiece = array[0].Trim().ToLower();
			AWorldGenerationOption aworldGenerationOption = WorldGenerationOptions._options.FirstOrDefault((AWorldGenerationOption x) => x.ServerConfigName != null && x.ServerConfigName == namePiece);
			if (aworldGenerationOption == null)
			{
				return;
			}
			aworldGenerationOption.AutoGenEnabled = autoGenEnabled;
		}

		// Token: 0x040011AF RID: 4527
		private static List<AWorldGenerationOption> _options = new List<AWorldGenerationOption>();

		// Token: 0x040011B0 RID: 4528
		private const string _powerPermissionsLineHeader = "seed_";

		// Token: 0x0200068B RID: 1675
		private class OptionStorage<T> where T : AWorldGenerationOption
		{
			// Token: 0x0400672D RID: 26413
			public static T Instance;
		}
	}
}
