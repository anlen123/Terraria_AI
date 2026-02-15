using System;
using System.Collections.Generic;
using System.Linq;

namespace Terraria.GameContent
{
	// Token: 0x02000243 RID: 579
	public static class SecretSeedsTracker
	{
		// Token: 0x060022B5 RID: 8885 RVA: 0x005395E2 File Offset: 0x005377E2
		public static void SetstringsFromConfig(ICollection<string> seedStrings)
		{
			SecretSeedsTracker._seedsForConfig.AddRange(seedStrings);
		}

		// Token: 0x060022B6 RID: 8886 RVA: 0x005395F0 File Offset: 0x005377F0
		public static void PrepareInterface()
		{
			if (!SecretSeedsTracker._processedConfig)
			{
				SecretSeedsTracker._processedConfig = true;
				SecretSeedsTracker._seedsForInterface.Clear();
				using (List<string>.Enumerator enumerator = SecretSeedsTracker._seedsForConfig.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						WorldGen.SecretSeed item;
						if (SecretSeedsTracker.SeedHasSecret(enumerator.Current, out item))
						{
							SecretSeedsTracker._seedsForInterface.Add(item);
						}
					}
				}
				SecretSeedsTracker._seedsForInterface = SecretSeedsTracker._seedsForInterface.Distinct<WorldGen.SecretSeed>().ToList<WorldGen.SecretSeed>();
				SecretSeedsTracker._seedsForConfig.Clear();
				SecretSeedsTracker._seedsForConfig.AddRange(from x in SecretSeedsTracker._seedsForInterface
				select x.TextThatWasUsedToUnlock);
			}
			SecretSeedsTracker._seedsForConfig.Sort();
			SecretSeedsTracker._seedsForInterface.Sort((WorldGen.SecretSeed a, WorldGen.SecretSeed b) => a.TextThatWasUsedToUnlock.CompareTo(b.TextThatWasUsedToUnlock));
		}

		// Token: 0x060022B7 RID: 8887 RVA: 0x005396EC File Offset: 0x005378EC
		private static bool SeedHasSecret(string seedString, out WorldGen.SecretSeed seed)
		{
			return WorldGen.SecretSeed.CheckInputForSecretSeed(seedString, out seed);
		}

		// Token: 0x060022B8 RID: 8888 RVA: 0x005396F8 File Offset: 0x005378F8
		public static void AddSeedToTrack(string seedString)
		{
			WorldGen.SecretSeed secretSeed;
			if (!SecretSeedsTracker.SeedHasSecret(seedString, out secretSeed))
			{
				return;
			}
			if (SecretSeedsTracker._seedsForInterface.Contains(secretSeed))
			{
				return;
			}
			SecretSeedsTracker._seedsForConfig.Add(secretSeed.TextThatWasUsedToUnlock);
			SecretSeedsTracker._seedsForInterface.Add(secretSeed);
			Main.SaveSettings();
		}

		// Token: 0x060022B9 RID: 8889 RVA: 0x0053973F File Offset: 0x0053793F
		public static List<string> GetStringsToSave()
		{
			return SecretSeedsTracker._seedsForConfig;
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x060022BA RID: 8890 RVA: 0x00539746 File Offset: 0x00537946
		public static List<WorldGen.SecretSeed> SeedsForInterface
		{
			get
			{
				return SecretSeedsTracker._seedsForInterface;
			}
		}

		// Token: 0x04004CFA RID: 19706
		private static List<string> _seedsForConfig = new List<string>();

		// Token: 0x04004CFB RID: 19707
		private static List<WorldGen.SecretSeed> _seedsForInterface = new List<WorldGen.SecretSeed>();

		// Token: 0x04004CFC RID: 19708
		private static bool _processedConfig = false;
	}
}
