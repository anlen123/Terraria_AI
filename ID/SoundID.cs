using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.Audio;

namespace Terraria.ID
{
	// Token: 0x020001AF RID: 431
	public class SoundID
	{
		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06001F15 RID: 7957 RVA: 0x00512FD4 File Offset: 0x005111D4
		public static int TrackableLegacySoundCount
		{
			get
			{
				return SoundID._trackableLegacySoundPathList.Count;
			}
		}

		// Token: 0x06001F16 RID: 7958 RVA: 0x00512FE0 File Offset: 0x005111E0
		public static string GetTrackableLegacySoundPath(int id)
		{
			return SoundID._trackableLegacySoundPathList[id];
		}

		// Token: 0x06001F17 RID: 7959 RVA: 0x00512FED File Offset: 0x005111ED
		private static LegacySoundStyle CreateTrackable(string name, SoundID.SoundStyleDefaults defaults, int maxInstances = 0)
		{
			return SoundID.CreateTrackable(name, 1, defaults.Type, maxInstances).WithPitchVariance(defaults.PitchVariance).WithVolume(defaults.Volume);
		}

		// Token: 0x06001F18 RID: 7960 RVA: 0x00513013 File Offset: 0x00511213
		private static LegacySoundStyle CreateTrackable(string name, int variations, SoundID.SoundStyleDefaults defaults, int maxInstances = 0)
		{
			return SoundID.CreateTrackable(name, variations, defaults.Type, maxInstances).WithPitchVariance(defaults.PitchVariance).WithVolume(defaults.Volume);
		}

		// Token: 0x06001F19 RID: 7961 RVA: 0x00513039 File Offset: 0x00511239
		private static LegacySoundStyle CreateTrackable(string name, SoundType type = SoundType.Sound, int maxInstances = 0)
		{
			return SoundID.CreateTrackable(name, 1, type, maxInstances);
		}

		// Token: 0x06001F1A RID: 7962 RVA: 0x00513044 File Offset: 0x00511244
		private static LegacySoundStyle CreateTrackable(string name, int variations, SoundType type = SoundType.Sound, int maxInstances = 0)
		{
			if (SoundID._trackableLegacySoundPathList == null)
			{
				SoundID._trackableLegacySoundPathList = new List<string>();
			}
			int count = SoundID._trackableLegacySoundPathList.Count;
			if (variations == 1)
			{
				SoundID._trackableLegacySoundPathList.Add(name);
			}
			else
			{
				for (int i = 0; i < variations; i++)
				{
					SoundID._trackableLegacySoundPathList.Add(name + "_" + i);
				}
			}
			return new LegacySoundStyle(42, count, variations, type, maxInstances);
		}

		// Token: 0x06001F1B RID: 7963 RVA: 0x005130B0 File Offset: 0x005112B0
		public static void FillAccessMap()
		{
			Dictionary<string, LegacySoundStyle> ret = new Dictionary<string, LegacySoundStyle>();
			Dictionary<string, ushort> ret2 = new Dictionary<string, ushort>();
			Dictionary<ushort, LegacySoundStyle> ret3 = new Dictionary<ushort, LegacySoundStyle>();
			ushort nextIndex = 0;
			List<FieldInfo> list = (from f in typeof(SoundID).GetFields(BindingFlags.Static | BindingFlags.Public)
			where f.FieldType == typeof(LegacySoundStyle)
			select f).ToList<FieldInfo>();
			list.Sort((FieldInfo a, FieldInfo b) => string.Compare(a.Name, b.Name));
			list.ForEach(delegate(FieldInfo field)
			{
				ret[field.Name] = (LegacySoundStyle)field.GetValue(null);
				ushort nextIndex;
				ret2[field.Name] = nextIndex;
				ret3[nextIndex] = (LegacySoundStyle)field.GetValue(null);
				nextIndex = nextIndex;
				nextIndex += 1;
			});
			SoundID.SoundByName = ret;
			SoundID.IndexByName = ret2;
			SoundID.SoundByIndex = ret3;
		}

		// Token: 0x04001A2F RID: 6703
		private static readonly SoundID.SoundStyleDefaults ItemDefaults = new SoundID.SoundStyleDefaults(1f, 0.06f, SoundType.Sound);

		// Token: 0x04001A30 RID: 6704
		public const int Dig = 0;

		// Token: 0x04001A31 RID: 6705
		public const int PlayerHit = 1;

		// Token: 0x04001A32 RID: 6706
		public const int Item = 2;

		// Token: 0x04001A33 RID: 6707
		public const int NPCHit = 3;

		// Token: 0x04001A34 RID: 6708
		public const int NPCKilled = 4;

		// Token: 0x04001A35 RID: 6709
		public const int PlayerKilled = 5;

		// Token: 0x04001A36 RID: 6710
		public const int Grass = 6;

		// Token: 0x04001A37 RID: 6711
		public const int Grab = 7;

		// Token: 0x04001A38 RID: 6712
		public const int DoorOpen = 8;

		// Token: 0x04001A39 RID: 6713
		public const int DoorClosed = 9;

		// Token: 0x04001A3A RID: 6714
		public const int MenuOpen = 10;

		// Token: 0x04001A3B RID: 6715
		public const int MenuClose = 11;

		// Token: 0x04001A3C RID: 6716
		public const int MenuTick = 12;

		// Token: 0x04001A3D RID: 6717
		public const int Shatter = 13;

		// Token: 0x04001A3E RID: 6718
		public const int ZombieMoan = 14;

		// Token: 0x04001A3F RID: 6719
		public const int Roar = 15;

		// Token: 0x04001A40 RID: 6720
		public const int DoubleJump = 16;

		// Token: 0x04001A41 RID: 6721
		public const int Run = 17;

		// Token: 0x04001A42 RID: 6722
		public const int Coins = 18;

		// Token: 0x04001A43 RID: 6723
		public const int Splash = 19;

		// Token: 0x04001A44 RID: 6724
		public const int FemaleHit = 20;

		// Token: 0x04001A45 RID: 6725
		public const int Tink = 21;

		// Token: 0x04001A46 RID: 6726
		public const int Unlock = 22;

		// Token: 0x04001A47 RID: 6727
		public const int Drown = 23;

		// Token: 0x04001A48 RID: 6728
		public const int Chat = 24;

		// Token: 0x04001A49 RID: 6729
		public const int MaxMana = 25;

		// Token: 0x04001A4A RID: 6730
		public const int Mummy = 26;

		// Token: 0x04001A4B RID: 6731
		public const int Pixie = 27;

		// Token: 0x04001A4C RID: 6732
		public const int Mech = 28;

		// Token: 0x04001A4D RID: 6733
		public const int Zombie = 29;

		// Token: 0x04001A4E RID: 6734
		public const int Duck = 30;

		// Token: 0x04001A4F RID: 6735
		public const int Frog = 31;

		// Token: 0x04001A50 RID: 6736
		public const int Bird = 32;

		// Token: 0x04001A51 RID: 6737
		public const int Critter = 33;

		// Token: 0x04001A52 RID: 6738
		public const int Waterfall = 34;

		// Token: 0x04001A53 RID: 6739
		public const int Lavafall = 35;

		// Token: 0x04001A54 RID: 6740
		public const int ForceRoar = 36;

		// Token: 0x04001A55 RID: 6741
		public const int Meowmere = 37;

		// Token: 0x04001A56 RID: 6742
		public const int CoinPickup = 38;

		// Token: 0x04001A57 RID: 6743
		public const int Drip = 39;

		// Token: 0x04001A58 RID: 6744
		public const int Camera = 40;

		// Token: 0x04001A59 RID: 6745
		public const int MoonLord = 41;

		// Token: 0x04001A5A RID: 6746
		public const int Trackable = 42;

		// Token: 0x04001A5B RID: 6747
		public const int Thunder = 43;

		// Token: 0x04001A5C RID: 6748
		public const int Seagull = 44;

		// Token: 0x04001A5D RID: 6749
		public const int Dolphin = 45;

		// Token: 0x04001A5E RID: 6750
		public const int Owl = 46;

		// Token: 0x04001A5F RID: 6751
		public const int GuitarC = 47;

		// Token: 0x04001A60 RID: 6752
		public const int GuitarD = 48;

		// Token: 0x04001A61 RID: 6753
		public const int GuitarEm = 49;

		// Token: 0x04001A62 RID: 6754
		public const int GuitarG = 50;

		// Token: 0x04001A63 RID: 6755
		public const int GuitarBm = 51;

		// Token: 0x04001A64 RID: 6756
		public const int GuitarAm = 52;

		// Token: 0x04001A65 RID: 6757
		public const int DrumHiHat = 53;

		// Token: 0x04001A66 RID: 6758
		public const int DrumTomHigh = 54;

		// Token: 0x04001A67 RID: 6759
		public const int DrumTomLow = 55;

		// Token: 0x04001A68 RID: 6760
		public const int DrumTomMid = 56;

		// Token: 0x04001A69 RID: 6761
		public const int DrumClosedHiHat = 57;

		// Token: 0x04001A6A RID: 6762
		public const int DrumCymbal1 = 58;

		// Token: 0x04001A6B RID: 6763
		public const int DrumCymbal2 = 59;

		// Token: 0x04001A6C RID: 6764
		public const int DrumKick = 60;

		// Token: 0x04001A6D RID: 6765
		public const int DrumTamaSnare = 61;

		// Token: 0x04001A6E RID: 6766
		public const int DrumFloorTom = 62;

		// Token: 0x04001A6F RID: 6767
		public const int Research = 63;

		// Token: 0x04001A70 RID: 6768
		public const int ResearchComplete = 64;

		// Token: 0x04001A71 RID: 6769
		public const int QueenSlime = 65;

		// Token: 0x04001A72 RID: 6770
		public const int Clown = 66;

		// Token: 0x04001A73 RID: 6771
		public const int Cockatiel = 67;

		// Token: 0x04001A74 RID: 6772
		public const int Macaw = 68;

		// Token: 0x04001A75 RID: 6773
		public const int Toucan = 69;

		// Token: 0x04001A76 RID: 6774
		public static readonly LegacySoundStyle NPCHit1 = new LegacySoundStyle(3, 1, SoundType.Sound, 0);

		// Token: 0x04001A77 RID: 6775
		public static readonly LegacySoundStyle NPCHit2 = new LegacySoundStyle(3, 2, SoundType.Sound, 0);

		// Token: 0x04001A78 RID: 6776
		public static readonly LegacySoundStyle NPCHit3 = new LegacySoundStyle(3, 3, SoundType.Sound, 0);

		// Token: 0x04001A79 RID: 6777
		public static readonly LegacySoundStyle NPCHit4 = new LegacySoundStyle(3, 4, SoundType.Sound, 0);

		// Token: 0x04001A7A RID: 6778
		public static readonly LegacySoundStyle NPCHit5 = new LegacySoundStyle(3, 5, SoundType.Sound, 0);

		// Token: 0x04001A7B RID: 6779
		public static readonly LegacySoundStyle NPCHit6 = new LegacySoundStyle(3, 6, SoundType.Sound, 0);

		// Token: 0x04001A7C RID: 6780
		public static readonly LegacySoundStyle NPCHit7 = new LegacySoundStyle(3, 7, SoundType.Sound, 0);

		// Token: 0x04001A7D RID: 6781
		public static readonly LegacySoundStyle NPCHit8 = new LegacySoundStyle(3, 8, SoundType.Sound, 0);

		// Token: 0x04001A7E RID: 6782
		public static readonly LegacySoundStyle NPCHit9 = new LegacySoundStyle(3, 9, SoundType.Sound, 0);

		// Token: 0x04001A7F RID: 6783
		public static readonly LegacySoundStyle NPCHit10 = new LegacySoundStyle(3, 10, SoundType.Sound, 0);

		// Token: 0x04001A80 RID: 6784
		public static readonly LegacySoundStyle NPCHit11 = new LegacySoundStyle(3, 11, SoundType.Sound, 0);

		// Token: 0x04001A81 RID: 6785
		public static readonly LegacySoundStyle NPCHit12 = new LegacySoundStyle(3, 12, SoundType.Sound, 0);

		// Token: 0x04001A82 RID: 6786
		public static readonly LegacySoundStyle NPCHit13 = new LegacySoundStyle(3, 13, SoundType.Sound, 0);

		// Token: 0x04001A83 RID: 6787
		public static readonly LegacySoundStyle NPCHit14 = new LegacySoundStyle(3, 14, SoundType.Sound, 0);

		// Token: 0x04001A84 RID: 6788
		public static readonly LegacySoundStyle NPCHit15 = new LegacySoundStyle(3, 15, SoundType.Sound, 0);

		// Token: 0x04001A85 RID: 6789
		public static readonly LegacySoundStyle NPCHit16 = new LegacySoundStyle(3, 16, SoundType.Sound, 0);

		// Token: 0x04001A86 RID: 6790
		public static readonly LegacySoundStyle NPCHit17 = new LegacySoundStyle(3, 17, SoundType.Sound, 0);

		// Token: 0x04001A87 RID: 6791
		public static readonly LegacySoundStyle NPCHit18 = new LegacySoundStyle(3, 18, SoundType.Sound, 0);

		// Token: 0x04001A88 RID: 6792
		public static readonly LegacySoundStyle NPCHit19 = new LegacySoundStyle(3, 19, SoundType.Sound, 0);

		// Token: 0x04001A89 RID: 6793
		public static readonly LegacySoundStyle NPCHit20 = new LegacySoundStyle(3, 20, SoundType.Sound, 0);

		// Token: 0x04001A8A RID: 6794
		public static readonly LegacySoundStyle NPCHit21 = new LegacySoundStyle(3, 21, SoundType.Sound, 0);

		// Token: 0x04001A8B RID: 6795
		public static readonly LegacySoundStyle NPCHit22 = new LegacySoundStyle(3, 22, SoundType.Sound, 0);

		// Token: 0x04001A8C RID: 6796
		public static readonly LegacySoundStyle NPCHit23 = new LegacySoundStyle(3, 23, SoundType.Sound, 0);

		// Token: 0x04001A8D RID: 6797
		public static readonly LegacySoundStyle NPCHit24 = new LegacySoundStyle(3, 24, SoundType.Sound, 0);

		// Token: 0x04001A8E RID: 6798
		public static readonly LegacySoundStyle NPCHit25 = new LegacySoundStyle(3, 25, SoundType.Sound, 0);

		// Token: 0x04001A8F RID: 6799
		public static readonly LegacySoundStyle NPCHit26 = new LegacySoundStyle(3, 26, SoundType.Sound, 0);

		// Token: 0x04001A90 RID: 6800
		public static readonly LegacySoundStyle NPCHit27 = new LegacySoundStyle(3, 27, SoundType.Sound, 0);

		// Token: 0x04001A91 RID: 6801
		public static readonly LegacySoundStyle NPCHit28 = new LegacySoundStyle(3, 28, SoundType.Sound, 0);

		// Token: 0x04001A92 RID: 6802
		public static readonly LegacySoundStyle NPCHit29 = new LegacySoundStyle(3, 29, SoundType.Sound, 0);

		// Token: 0x04001A93 RID: 6803
		public static readonly LegacySoundStyle NPCHit30 = new LegacySoundStyle(3, 30, SoundType.Sound, 0);

		// Token: 0x04001A94 RID: 6804
		public static readonly LegacySoundStyle NPCHit31 = new LegacySoundStyle(3, 31, SoundType.Sound, 0);

		// Token: 0x04001A95 RID: 6805
		public static readonly LegacySoundStyle NPCHit32 = new LegacySoundStyle(3, 32, SoundType.Sound, 0);

		// Token: 0x04001A96 RID: 6806
		public static readonly LegacySoundStyle NPCHit33 = new LegacySoundStyle(3, 33, SoundType.Sound, 0);

		// Token: 0x04001A97 RID: 6807
		public static readonly LegacySoundStyle NPCHit34 = new LegacySoundStyle(3, 34, SoundType.Sound, 0);

		// Token: 0x04001A98 RID: 6808
		public static readonly LegacySoundStyle NPCHit35 = new LegacySoundStyle(3, 35, SoundType.Sound, 0);

		// Token: 0x04001A99 RID: 6809
		public static readonly LegacySoundStyle NPCHit36 = new LegacySoundStyle(3, 36, SoundType.Sound, 0);

		// Token: 0x04001A9A RID: 6810
		public static readonly LegacySoundStyle NPCHit37 = new LegacySoundStyle(3, 37, SoundType.Sound, 0);

		// Token: 0x04001A9B RID: 6811
		public static readonly LegacySoundStyle NPCHit38 = new LegacySoundStyle(3, 38, SoundType.Sound, 0);

		// Token: 0x04001A9C RID: 6812
		public static readonly LegacySoundStyle NPCHit39 = new LegacySoundStyle(3, 39, SoundType.Sound, 0);

		// Token: 0x04001A9D RID: 6813
		public static readonly LegacySoundStyle NPCHit40 = new LegacySoundStyle(3, 40, SoundType.Sound, 0);

		// Token: 0x04001A9E RID: 6814
		public static readonly LegacySoundStyle NPCHit41 = new LegacySoundStyle(3, 41, SoundType.Sound, 0);

		// Token: 0x04001A9F RID: 6815
		public static readonly LegacySoundStyle NPCHit42 = new LegacySoundStyle(3, 42, SoundType.Sound, 0);

		// Token: 0x04001AA0 RID: 6816
		public static readonly LegacySoundStyle NPCHit43 = new LegacySoundStyle(3, 43, SoundType.Sound, 0);

		// Token: 0x04001AA1 RID: 6817
		public static readonly LegacySoundStyle NPCHit44 = new LegacySoundStyle(3, 44, SoundType.Sound, 0);

		// Token: 0x04001AA2 RID: 6818
		public static readonly LegacySoundStyle NPCHit45 = new LegacySoundStyle(3, 45, SoundType.Sound, 0);

		// Token: 0x04001AA3 RID: 6819
		public static readonly LegacySoundStyle NPCHit46 = new LegacySoundStyle(3, 46, SoundType.Sound, 0);

		// Token: 0x04001AA4 RID: 6820
		public static readonly LegacySoundStyle NPCHit47 = new LegacySoundStyle(3, 47, SoundType.Sound, 0);

		// Token: 0x04001AA5 RID: 6821
		public static readonly LegacySoundStyle NPCHit48 = new LegacySoundStyle(3, 48, SoundType.Sound, 0);

		// Token: 0x04001AA6 RID: 6822
		public static readonly LegacySoundStyle NPCHit49 = new LegacySoundStyle(3, 49, SoundType.Sound, 0);

		// Token: 0x04001AA7 RID: 6823
		public static readonly LegacySoundStyle NPCHit50 = new LegacySoundStyle(3, 50, SoundType.Sound, 0);

		// Token: 0x04001AA8 RID: 6824
		public static readonly LegacySoundStyle NPCHit51 = new LegacySoundStyle(3, 51, SoundType.Sound, 0);

		// Token: 0x04001AA9 RID: 6825
		public static readonly LegacySoundStyle NPCHit52 = new LegacySoundStyle(3, 52, SoundType.Sound, 0);

		// Token: 0x04001AAA RID: 6826
		public static readonly LegacySoundStyle NPCHit53 = new LegacySoundStyle(3, 53, SoundType.Sound, 0);

		// Token: 0x04001AAB RID: 6827
		public static readonly LegacySoundStyle NPCHit54 = new LegacySoundStyle(3, 54, SoundType.Sound, 0);

		// Token: 0x04001AAC RID: 6828
		public static readonly LegacySoundStyle NPCHit55 = new LegacySoundStyle(3, 55, SoundType.Sound, 0);

		// Token: 0x04001AAD RID: 6829
		public static readonly LegacySoundStyle NPCHit56 = new LegacySoundStyle(3, 56, SoundType.Sound, 0);

		// Token: 0x04001AAE RID: 6830
		public static readonly LegacySoundStyle NPCHit57 = new LegacySoundStyle(3, 57, SoundType.Sound, 0);

		// Token: 0x04001AAF RID: 6831
		public static readonly LegacySoundStyle NPCHit58 = new LegacySoundStyle(3, 58, SoundType.Sound, 0);

		// Token: 0x04001AB0 RID: 6832
		public static readonly LegacySoundStyle NPCDeath1 = new LegacySoundStyle(4, 1, SoundType.Sound, 0);

		// Token: 0x04001AB1 RID: 6833
		public static readonly LegacySoundStyle NPCDeath2 = new LegacySoundStyle(4, 2, SoundType.Sound, 0);

		// Token: 0x04001AB2 RID: 6834
		public static readonly LegacySoundStyle NPCDeath3 = new LegacySoundStyle(4, 3, SoundType.Sound, 0);

		// Token: 0x04001AB3 RID: 6835
		public static readonly LegacySoundStyle NPCDeath4 = new LegacySoundStyle(4, 4, SoundType.Sound, 0);

		// Token: 0x04001AB4 RID: 6836
		public static readonly LegacySoundStyle NPCDeath5 = new LegacySoundStyle(4, 5, SoundType.Sound, 0);

		// Token: 0x04001AB5 RID: 6837
		public static readonly LegacySoundStyle NPCDeath6 = new LegacySoundStyle(4, 6, SoundType.Sound, 0);

		// Token: 0x04001AB6 RID: 6838
		public static readonly LegacySoundStyle NPCDeath7 = new LegacySoundStyle(4, 7, SoundType.Sound, 0);

		// Token: 0x04001AB7 RID: 6839
		public static readonly LegacySoundStyle NPCDeath8 = new LegacySoundStyle(4, 8, SoundType.Sound, 0);

		// Token: 0x04001AB8 RID: 6840
		public static readonly LegacySoundStyle NPCDeath9 = new LegacySoundStyle(4, 9, SoundType.Sound, 0);

		// Token: 0x04001AB9 RID: 6841
		public static readonly LegacySoundStyle NPCDeath10 = new LegacySoundStyle(4, 10, SoundType.Sound, 0);

		// Token: 0x04001ABA RID: 6842
		public static readonly LegacySoundStyle NPCDeath11 = new LegacySoundStyle(4, 11, SoundType.Sound, 0);

		// Token: 0x04001ABB RID: 6843
		public static readonly LegacySoundStyle NPCDeath12 = new LegacySoundStyle(4, 12, SoundType.Sound, 0);

		// Token: 0x04001ABC RID: 6844
		public static readonly LegacySoundStyle NPCDeath13 = new LegacySoundStyle(4, 13, SoundType.Sound, 0);

		// Token: 0x04001ABD RID: 6845
		public static readonly LegacySoundStyle NPCDeath14 = new LegacySoundStyle(4, 14, SoundType.Sound, 0);

		// Token: 0x04001ABE RID: 6846
		public static readonly LegacySoundStyle NPCDeath15 = new LegacySoundStyle(4, 15, SoundType.Sound, 0);

		// Token: 0x04001ABF RID: 6847
		public static readonly LegacySoundStyle NPCDeath16 = new LegacySoundStyle(4, 16, SoundType.Sound, 0);

		// Token: 0x04001AC0 RID: 6848
		public static readonly LegacySoundStyle NPCDeath17 = new LegacySoundStyle(4, 17, SoundType.Sound, 0);

		// Token: 0x04001AC1 RID: 6849
		public static readonly LegacySoundStyle NPCDeath18 = new LegacySoundStyle(4, 18, SoundType.Sound, 0);

		// Token: 0x04001AC2 RID: 6850
		public static readonly LegacySoundStyle NPCDeath19 = new LegacySoundStyle(4, 19, SoundType.Sound, 0);

		// Token: 0x04001AC3 RID: 6851
		public static readonly LegacySoundStyle NPCDeath20 = new LegacySoundStyle(4, 20, SoundType.Sound, 0);

		// Token: 0x04001AC4 RID: 6852
		public static readonly LegacySoundStyle NPCDeath21 = new LegacySoundStyle(4, 21, SoundType.Sound, 0);

		// Token: 0x04001AC5 RID: 6853
		public static readonly LegacySoundStyle NPCDeath22 = new LegacySoundStyle(4, 22, SoundType.Sound, 0);

		// Token: 0x04001AC6 RID: 6854
		public static readonly LegacySoundStyle NPCDeath23 = new LegacySoundStyle(4, 23, SoundType.Sound, 0);

		// Token: 0x04001AC7 RID: 6855
		public static readonly LegacySoundStyle NPCDeath24 = new LegacySoundStyle(4, 24, SoundType.Sound, 0);

		// Token: 0x04001AC8 RID: 6856
		public static readonly LegacySoundStyle NPCDeath25 = new LegacySoundStyle(4, 25, SoundType.Sound, 0);

		// Token: 0x04001AC9 RID: 6857
		public static readonly LegacySoundStyle NPCDeath26 = new LegacySoundStyle(4, 26, SoundType.Sound, 0);

		// Token: 0x04001ACA RID: 6858
		public static readonly LegacySoundStyle NPCDeath27 = new LegacySoundStyle(4, 27, SoundType.Sound, 0);

		// Token: 0x04001ACB RID: 6859
		public static readonly LegacySoundStyle NPCDeath28 = new LegacySoundStyle(4, 28, SoundType.Sound, 0);

		// Token: 0x04001ACC RID: 6860
		public static readonly LegacySoundStyle NPCDeath29 = new LegacySoundStyle(4, 29, SoundType.Sound, 0);

		// Token: 0x04001ACD RID: 6861
		public static readonly LegacySoundStyle NPCDeath30 = new LegacySoundStyle(4, 30, SoundType.Sound, 0);

		// Token: 0x04001ACE RID: 6862
		public static readonly LegacySoundStyle NPCDeath31 = new LegacySoundStyle(4, 31, SoundType.Sound, 0);

		// Token: 0x04001ACF RID: 6863
		public static readonly LegacySoundStyle NPCDeath32 = new LegacySoundStyle(4, 32, SoundType.Sound, 0);

		// Token: 0x04001AD0 RID: 6864
		public static readonly LegacySoundStyle NPCDeath33 = new LegacySoundStyle(4, 33, SoundType.Sound, 0);

		// Token: 0x04001AD1 RID: 6865
		public static readonly LegacySoundStyle NPCDeath34 = new LegacySoundStyle(4, 34, SoundType.Sound, 0);

		// Token: 0x04001AD2 RID: 6866
		public static readonly LegacySoundStyle NPCDeath35 = new LegacySoundStyle(4, 35, SoundType.Sound, 0);

		// Token: 0x04001AD3 RID: 6867
		public static readonly LegacySoundStyle NPCDeath36 = new LegacySoundStyle(4, 36, SoundType.Sound, 0);

		// Token: 0x04001AD4 RID: 6868
		public static readonly LegacySoundStyle NPCDeath37 = new LegacySoundStyle(4, 37, SoundType.Sound, 0);

		// Token: 0x04001AD5 RID: 6869
		public static readonly LegacySoundStyle NPCDeath38 = new LegacySoundStyle(4, 38, SoundType.Sound, 0);

		// Token: 0x04001AD6 RID: 6870
		public static readonly LegacySoundStyle NPCDeath39 = new LegacySoundStyle(4, 39, SoundType.Sound, 0);

		// Token: 0x04001AD7 RID: 6871
		public static readonly LegacySoundStyle NPCDeath40 = new LegacySoundStyle(4, 40, SoundType.Sound, 0);

		// Token: 0x04001AD8 RID: 6872
		public static readonly LegacySoundStyle NPCDeath41 = new LegacySoundStyle(4, 41, SoundType.Sound, 0);

		// Token: 0x04001AD9 RID: 6873
		public static readonly LegacySoundStyle NPCDeath42 = new LegacySoundStyle(4, 42, SoundType.Sound, 0);

		// Token: 0x04001ADA RID: 6874
		public static readonly LegacySoundStyle NPCDeath43 = new LegacySoundStyle(4, 43, SoundType.Sound, 0);

		// Token: 0x04001ADB RID: 6875
		public static readonly LegacySoundStyle NPCDeath44 = new LegacySoundStyle(4, 44, SoundType.Sound, 0);

		// Token: 0x04001ADC RID: 6876
		public static readonly LegacySoundStyle NPCDeath45 = new LegacySoundStyle(4, 45, SoundType.Sound, 0);

		// Token: 0x04001ADD RID: 6877
		public static readonly LegacySoundStyle NPCDeath46 = new LegacySoundStyle(4, 46, SoundType.Sound, 0);

		// Token: 0x04001ADE RID: 6878
		public static readonly LegacySoundStyle NPCDeath47 = new LegacySoundStyle(4, 47, SoundType.Sound, 0);

		// Token: 0x04001ADF RID: 6879
		public static readonly LegacySoundStyle NPCDeath48 = new LegacySoundStyle(4, 48, SoundType.Sound, 0);

		// Token: 0x04001AE0 RID: 6880
		public static readonly LegacySoundStyle NPCDeath49 = new LegacySoundStyle(4, 49, SoundType.Sound, 0);

		// Token: 0x04001AE1 RID: 6881
		public static readonly LegacySoundStyle NPCDeath50 = new LegacySoundStyle(4, 50, SoundType.Sound, 0);

		// Token: 0x04001AE2 RID: 6882
		public static readonly LegacySoundStyle NPCDeath51 = new LegacySoundStyle(4, 51, SoundType.Sound, 0);

		// Token: 0x04001AE3 RID: 6883
		public static readonly LegacySoundStyle NPCDeath52 = new LegacySoundStyle(4, 52, SoundType.Sound, 0);

		// Token: 0x04001AE4 RID: 6884
		public static readonly LegacySoundStyle NPCDeath53 = new LegacySoundStyle(4, 53, SoundType.Sound, 0);

		// Token: 0x04001AE5 RID: 6885
		public static readonly LegacySoundStyle NPCDeath54 = new LegacySoundStyle(4, 54, SoundType.Sound, 0);

		// Token: 0x04001AE6 RID: 6886
		public static readonly LegacySoundStyle NPCDeath55 = new LegacySoundStyle(4, 55, SoundType.Sound, 0);

		// Token: 0x04001AE7 RID: 6887
		public static readonly LegacySoundStyle NPCDeath56 = new LegacySoundStyle(4, 56, SoundType.Sound, 0);

		// Token: 0x04001AE8 RID: 6888
		public static readonly LegacySoundStyle NPCDeath57 = new LegacySoundStyle(4, 57, SoundType.Sound, 0);

		// Token: 0x04001AE9 RID: 6889
		public static readonly LegacySoundStyle NPCDeath58 = new LegacySoundStyle(4, 58, SoundType.Sound, 0);

		// Token: 0x04001AEA RID: 6890
		public static readonly LegacySoundStyle NPCDeath59 = new LegacySoundStyle(4, 59, SoundType.Sound, 0);

		// Token: 0x04001AEB RID: 6891
		public static readonly LegacySoundStyle NPCDeath60 = new LegacySoundStyle(4, 60, SoundType.Sound, 0);

		// Token: 0x04001AEC RID: 6892
		public static readonly LegacySoundStyle NPCDeath61 = new LegacySoundStyle(4, 61, SoundType.Sound, 0);

		// Token: 0x04001AED RID: 6893
		public static readonly LegacySoundStyle NPCDeath62 = new LegacySoundStyle(4, 62, SoundType.Sound, 0);

		// Token: 0x04001AEE RID: 6894
		public static readonly LegacySoundStyle NPCDeath63 = new LegacySoundStyle(4, 63, SoundType.Sound, 0);

		// Token: 0x04001AEF RID: 6895
		public static readonly LegacySoundStyle NPCDeath64 = new LegacySoundStyle(4, 64, SoundType.Sound, 0);

		// Token: 0x04001AF0 RID: 6896
		public static readonly LegacySoundStyle NPCDeath65 = new LegacySoundStyle(4, 65, SoundType.Sound, 0);

		// Token: 0x04001AF1 RID: 6897
		public static readonly LegacySoundStyle NPCDeath66 = new LegacySoundStyle(4, 66, SoundType.Sound, 0);

		// Token: 0x04001AF2 RID: 6898
		public static readonly LegacySoundStyle NPCDeath67 = new LegacySoundStyle(4, 67, SoundType.Sound, 0);

		// Token: 0x04001AF3 RID: 6899
		public static readonly LegacySoundStyle NPCDeath68 = new LegacySoundStyle(4, 68, SoundType.Sound, 0);

		// Token: 0x04001AF4 RID: 6900
		public static short NPCDeathCount = 69;

		// Token: 0x04001AF5 RID: 6901
		public static readonly LegacySoundStyle Item1 = new LegacySoundStyle(2, 1, SoundType.Sound, 0);

		// Token: 0x04001AF6 RID: 6902
		public static readonly LegacySoundStyle Item2 = new LegacySoundStyle(2, 2, SoundType.Sound, 0);

		// Token: 0x04001AF7 RID: 6903
		public static readonly LegacySoundStyle Item3 = new LegacySoundStyle(2, 3, SoundType.Sound, 0);

		// Token: 0x04001AF8 RID: 6904
		public static readonly LegacySoundStyle Item4 = new LegacySoundStyle(2, 4, SoundType.Sound, 0);

		// Token: 0x04001AF9 RID: 6905
		public static readonly LegacySoundStyle Item5 = new LegacySoundStyle(2, 5, SoundType.Sound, 0);

		// Token: 0x04001AFA RID: 6906
		public static readonly LegacySoundStyle Item6 = new LegacySoundStyle(2, 6, SoundType.Sound, 0);

		// Token: 0x04001AFB RID: 6907
		public static readonly LegacySoundStyle Item7 = new LegacySoundStyle(2, 7, SoundType.Sound, 0);

		// Token: 0x04001AFC RID: 6908
		public static readonly LegacySoundStyle Item8 = new LegacySoundStyle(2, 8, SoundType.Sound, 0);

		// Token: 0x04001AFD RID: 6909
		public static readonly LegacySoundStyle Item9 = new LegacySoundStyle(2, 9, SoundType.Sound, 0);

		// Token: 0x04001AFE RID: 6910
		public static readonly LegacySoundStyle Item10 = new LegacySoundStyle(2, 10, SoundType.Sound, 0);

		// Token: 0x04001AFF RID: 6911
		public static readonly LegacySoundStyle Item11 = new LegacySoundStyle(2, 11, SoundType.Sound, 0);

		// Token: 0x04001B00 RID: 6912
		public static readonly LegacySoundStyle Item12 = new LegacySoundStyle(2, 12, SoundType.Sound, 0);

		// Token: 0x04001B01 RID: 6913
		public static readonly LegacySoundStyle Item13 = new LegacySoundStyle(2, 13, SoundType.Sound, 0);

		// Token: 0x04001B02 RID: 6914
		public static readonly LegacySoundStyle Item14 = new LegacySoundStyle(2, 14, SoundType.Sound, 0);

		// Token: 0x04001B03 RID: 6915
		public static readonly LegacySoundStyle Item15 = new LegacySoundStyle(2, 15, SoundType.Sound, 0);

		// Token: 0x04001B04 RID: 6916
		public static readonly LegacySoundStyle Item16 = new LegacySoundStyle(2, 16, SoundType.Sound, 0);

		// Token: 0x04001B05 RID: 6917
		public static readonly LegacySoundStyle Item17 = new LegacySoundStyle(2, 17, SoundType.Sound, 0);

		// Token: 0x04001B06 RID: 6918
		public static readonly LegacySoundStyle Item18 = new LegacySoundStyle(2, 18, SoundType.Sound, 0);

		// Token: 0x04001B07 RID: 6919
		public static readonly LegacySoundStyle Item19 = new LegacySoundStyle(2, 19, SoundType.Sound, 0);

		// Token: 0x04001B08 RID: 6920
		public static readonly LegacySoundStyle Item20 = new LegacySoundStyle(2, 20, SoundType.Sound, 0);

		// Token: 0x04001B09 RID: 6921
		public static readonly LegacySoundStyle Item21 = new LegacySoundStyle(2, 21, SoundType.Sound, 0);

		// Token: 0x04001B0A RID: 6922
		public static readonly LegacySoundStyle Item22 = new LegacySoundStyle(2, 22, SoundType.Sound, 0);

		// Token: 0x04001B0B RID: 6923
		public static readonly LegacySoundStyle Item23 = new LegacySoundStyle(2, 23, SoundType.Sound, 0);

		// Token: 0x04001B0C RID: 6924
		public static readonly LegacySoundStyle Item24 = new LegacySoundStyle(2, 24, SoundType.Sound, 0);

		// Token: 0x04001B0D RID: 6925
		public static readonly LegacySoundStyle Item25 = new LegacySoundStyle(2, 25, SoundType.Sound, 0);

		// Token: 0x04001B0E RID: 6926
		public static readonly LegacySoundStyle Item26 = new LegacySoundStyle(2, 26, SoundType.Sound, 0);

		// Token: 0x04001B0F RID: 6927
		public static readonly LegacySoundStyle Item27 = new LegacySoundStyle(2, 27, SoundType.Sound, 0);

		// Token: 0x04001B10 RID: 6928
		public static readonly LegacySoundStyle Item28 = new LegacySoundStyle(2, 28, SoundType.Sound, 0);

		// Token: 0x04001B11 RID: 6929
		public static readonly LegacySoundStyle Item29 = new LegacySoundStyle(2, 29, SoundType.Sound, 0);

		// Token: 0x04001B12 RID: 6930
		public static readonly LegacySoundStyle Item30 = new LegacySoundStyle(2, 30, SoundType.Sound, 0);

		// Token: 0x04001B13 RID: 6931
		public static readonly LegacySoundStyle Item31 = new LegacySoundStyle(2, 31, SoundType.Sound, 0);

		// Token: 0x04001B14 RID: 6932
		public static readonly LegacySoundStyle Item32 = new LegacySoundStyle(2, 32, SoundType.Sound, 0);

		// Token: 0x04001B15 RID: 6933
		public static readonly LegacySoundStyle Item33 = new LegacySoundStyle(2, 33, SoundType.Sound, 0);

		// Token: 0x04001B16 RID: 6934
		public static readonly LegacySoundStyle Item34 = new LegacySoundStyle(2, 34, SoundType.Sound, 0);

		// Token: 0x04001B17 RID: 6935
		public static readonly LegacySoundStyle Item35 = new LegacySoundStyle(2, 35, SoundType.Sound, 0);

		// Token: 0x04001B18 RID: 6936
		public static readonly LegacySoundStyle Item36 = new LegacySoundStyle(2, 36, SoundType.Sound, 0);

		// Token: 0x04001B19 RID: 6937
		public static readonly LegacySoundStyle Item37 = new LegacySoundStyle(2, 37, SoundType.Sound, 0);

		// Token: 0x04001B1A RID: 6938
		public static readonly LegacySoundStyle Item38 = new LegacySoundStyle(2, 38, SoundType.Sound, 0);

		// Token: 0x04001B1B RID: 6939
		public static readonly LegacySoundStyle Item39 = new LegacySoundStyle(2, 39, SoundType.Sound, 0);

		// Token: 0x04001B1C RID: 6940
		public static readonly LegacySoundStyle Item40 = new LegacySoundStyle(2, 40, SoundType.Sound, 0);

		// Token: 0x04001B1D RID: 6941
		public static readonly LegacySoundStyle Item41 = new LegacySoundStyle(2, 41, SoundType.Sound, 0);

		// Token: 0x04001B1E RID: 6942
		public static readonly LegacySoundStyle Item42 = new LegacySoundStyle(2, 42, SoundType.Sound, 0);

		// Token: 0x04001B1F RID: 6943
		public static readonly LegacySoundStyle Item43 = new LegacySoundStyle(2, 43, SoundType.Sound, 0);

		// Token: 0x04001B20 RID: 6944
		public static readonly LegacySoundStyle Item44 = new LegacySoundStyle(2, 44, SoundType.Sound, 0);

		// Token: 0x04001B21 RID: 6945
		public static readonly LegacySoundStyle Item45 = new LegacySoundStyle(2, 45, SoundType.Sound, 0);

		// Token: 0x04001B22 RID: 6946
		public static readonly LegacySoundStyle Item46 = new LegacySoundStyle(2, 46, SoundType.Sound, 0);

		// Token: 0x04001B23 RID: 6947
		public static readonly LegacySoundStyle Item47 = new LegacySoundStyle(2, 47, SoundType.Sound, 0);

		// Token: 0x04001B24 RID: 6948
		public static readonly LegacySoundStyle Item48 = new LegacySoundStyle(2, 48, SoundType.Sound, 0);

		// Token: 0x04001B25 RID: 6949
		public static readonly LegacySoundStyle Item49 = new LegacySoundStyle(2, 49, SoundType.Sound, 0);

		// Token: 0x04001B26 RID: 6950
		public static readonly LegacySoundStyle Item50 = new LegacySoundStyle(2, 50, SoundType.Sound, 0);

		// Token: 0x04001B27 RID: 6951
		public static readonly LegacySoundStyle Item51 = new LegacySoundStyle(2, 51, SoundType.Sound, 0);

		// Token: 0x04001B28 RID: 6952
		public static readonly LegacySoundStyle Item52 = new LegacySoundStyle(2, 52, SoundType.Sound, 0);

		// Token: 0x04001B29 RID: 6953
		public static readonly LegacySoundStyle Item53 = new LegacySoundStyle(2, 53, SoundType.Sound, 0);

		// Token: 0x04001B2A RID: 6954
		public static readonly LegacySoundStyle Item54 = new LegacySoundStyle(2, 54, SoundType.Sound, 0);

		// Token: 0x04001B2B RID: 6955
		public static readonly LegacySoundStyle Item55 = new LegacySoundStyle(2, 55, SoundType.Sound, 0);

		// Token: 0x04001B2C RID: 6956
		public static readonly LegacySoundStyle Item56 = new LegacySoundStyle(2, 56, SoundType.Sound, 0);

		// Token: 0x04001B2D RID: 6957
		public static readonly LegacySoundStyle Item57 = new LegacySoundStyle(2, 57, SoundType.Sound, 0);

		// Token: 0x04001B2E RID: 6958
		public static readonly LegacySoundStyle Item58 = new LegacySoundStyle(2, 58, SoundType.Sound, 0);

		// Token: 0x04001B2F RID: 6959
		public static readonly LegacySoundStyle Item59 = new LegacySoundStyle(2, 59, SoundType.Sound, 0);

		// Token: 0x04001B30 RID: 6960
		public static readonly LegacySoundStyle Item60 = new LegacySoundStyle(2, 60, SoundType.Sound, 0);

		// Token: 0x04001B31 RID: 6961
		public static readonly LegacySoundStyle Item61 = new LegacySoundStyle(2, 61, SoundType.Sound, 0);

		// Token: 0x04001B32 RID: 6962
		public static readonly LegacySoundStyle Item62 = new LegacySoundStyle(2, 62, SoundType.Sound, 0);

		// Token: 0x04001B33 RID: 6963
		public static readonly LegacySoundStyle Item63 = new LegacySoundStyle(2, 63, SoundType.Sound, 0);

		// Token: 0x04001B34 RID: 6964
		public static readonly LegacySoundStyle Item64 = new LegacySoundStyle(2, 64, SoundType.Sound, 0);

		// Token: 0x04001B35 RID: 6965
		public static readonly LegacySoundStyle Item65 = new LegacySoundStyle(2, 65, SoundType.Sound, 0);

		// Token: 0x04001B36 RID: 6966
		public static readonly LegacySoundStyle Item66 = new LegacySoundStyle(2, 66, SoundType.Sound, 0);

		// Token: 0x04001B37 RID: 6967
		public static readonly LegacySoundStyle Item67 = new LegacySoundStyle(2, 67, SoundType.Sound, 0);

		// Token: 0x04001B38 RID: 6968
		public static readonly LegacySoundStyle Item68 = new LegacySoundStyle(2, 68, SoundType.Sound, 0);

		// Token: 0x04001B39 RID: 6969
		public static readonly LegacySoundStyle Item69 = new LegacySoundStyle(2, 69, SoundType.Sound, 0);

		// Token: 0x04001B3A RID: 6970
		public static readonly LegacySoundStyle Item70 = new LegacySoundStyle(2, 70, SoundType.Sound, 0);

		// Token: 0x04001B3B RID: 6971
		public static readonly LegacySoundStyle Item71 = new LegacySoundStyle(2, 71, SoundType.Sound, 0);

		// Token: 0x04001B3C RID: 6972
		public static readonly LegacySoundStyle Item72 = new LegacySoundStyle(2, 72, SoundType.Sound, 0);

		// Token: 0x04001B3D RID: 6973
		public static readonly LegacySoundStyle Item73 = new LegacySoundStyle(2, 73, SoundType.Sound, 0);

		// Token: 0x04001B3E RID: 6974
		public static readonly LegacySoundStyle Item74 = new LegacySoundStyle(2, 74, SoundType.Sound, 0);

		// Token: 0x04001B3F RID: 6975
		public static readonly LegacySoundStyle Item75 = new LegacySoundStyle(2, 75, SoundType.Sound, 0);

		// Token: 0x04001B40 RID: 6976
		public static readonly LegacySoundStyle Item76 = new LegacySoundStyle(2, 76, SoundType.Sound, 0);

		// Token: 0x04001B41 RID: 6977
		public static readonly LegacySoundStyle Item77 = new LegacySoundStyle(2, 77, SoundType.Sound, 0);

		// Token: 0x04001B42 RID: 6978
		public static readonly LegacySoundStyle Item78 = new LegacySoundStyle(2, 78, SoundType.Sound, 0);

		// Token: 0x04001B43 RID: 6979
		public static readonly LegacySoundStyle Item79 = new LegacySoundStyle(2, 79, SoundType.Sound, 0);

		// Token: 0x04001B44 RID: 6980
		public static readonly LegacySoundStyle Item80 = new LegacySoundStyle(2, 80, SoundType.Sound, 0);

		// Token: 0x04001B45 RID: 6981
		public static readonly LegacySoundStyle Item81 = new LegacySoundStyle(2, 81, SoundType.Sound, 0);

		// Token: 0x04001B46 RID: 6982
		public static readonly LegacySoundStyle Item82 = new LegacySoundStyle(2, 82, SoundType.Sound, 0);

		// Token: 0x04001B47 RID: 6983
		public static readonly LegacySoundStyle Item83 = new LegacySoundStyle(2, 83, SoundType.Sound, 0);

		// Token: 0x04001B48 RID: 6984
		public static readonly LegacySoundStyle Item84 = new LegacySoundStyle(2, 84, SoundType.Sound, 0);

		// Token: 0x04001B49 RID: 6985
		public static readonly LegacySoundStyle Item85 = new LegacySoundStyle(2, 85, SoundType.Sound, 0);

		// Token: 0x04001B4A RID: 6986
		public static readonly LegacySoundStyle Item86 = new LegacySoundStyle(2, 86, SoundType.Sound, 0);

		// Token: 0x04001B4B RID: 6987
		public static readonly LegacySoundStyle Item87 = new LegacySoundStyle(2, 87, SoundType.Sound, 0);

		// Token: 0x04001B4C RID: 6988
		public static readonly LegacySoundStyle Item88 = new LegacySoundStyle(2, 88, SoundType.Sound, 0);

		// Token: 0x04001B4D RID: 6989
		public static readonly LegacySoundStyle Item89 = new LegacySoundStyle(2, 89, SoundType.Sound, 0);

		// Token: 0x04001B4E RID: 6990
		public static readonly LegacySoundStyle Item90 = new LegacySoundStyle(2, 90, SoundType.Sound, 0);

		// Token: 0x04001B4F RID: 6991
		public static readonly LegacySoundStyle Item91 = new LegacySoundStyle(2, 91, SoundType.Sound, 0);

		// Token: 0x04001B50 RID: 6992
		public static readonly LegacySoundStyle Item92 = new LegacySoundStyle(2, 92, SoundType.Sound, 0);

		// Token: 0x04001B51 RID: 6993
		public static readonly LegacySoundStyle Item93 = new LegacySoundStyle(2, 93, SoundType.Sound, 0);

		// Token: 0x04001B52 RID: 6994
		public static readonly LegacySoundStyle Item94 = new LegacySoundStyle(2, 94, SoundType.Sound, 0);

		// Token: 0x04001B53 RID: 6995
		public static readonly LegacySoundStyle Item95 = new LegacySoundStyle(2, 95, SoundType.Sound, 0);

		// Token: 0x04001B54 RID: 6996
		public static readonly LegacySoundStyle Item96 = new LegacySoundStyle(2, 96, SoundType.Sound, 0);

		// Token: 0x04001B55 RID: 6997
		public static readonly LegacySoundStyle Item97 = new LegacySoundStyle(2, 97, SoundType.Sound, 0);

		// Token: 0x04001B56 RID: 6998
		public static readonly LegacySoundStyle Item98 = new LegacySoundStyle(2, 98, SoundType.Sound, 0);

		// Token: 0x04001B57 RID: 6999
		public static readonly LegacySoundStyle Item99 = new LegacySoundStyle(2, 99, SoundType.Sound, 0);

		// Token: 0x04001B58 RID: 7000
		public static readonly LegacySoundStyle Item100 = new LegacySoundStyle(2, 100, SoundType.Sound, 0);

		// Token: 0x04001B59 RID: 7001
		public static readonly LegacySoundStyle Item101 = new LegacySoundStyle(2, 101, SoundType.Sound, 0);

		// Token: 0x04001B5A RID: 7002
		public static readonly LegacySoundStyle Item102 = new LegacySoundStyle(2, 102, SoundType.Sound, 0);

		// Token: 0x04001B5B RID: 7003
		public static readonly LegacySoundStyle Item103 = new LegacySoundStyle(2, 103, SoundType.Sound, 0);

		// Token: 0x04001B5C RID: 7004
		public static readonly LegacySoundStyle Item104 = new LegacySoundStyle(2, 104, SoundType.Sound, 0);

		// Token: 0x04001B5D RID: 7005
		public static readonly LegacySoundStyle Item105 = new LegacySoundStyle(2, 105, SoundType.Sound, 0);

		// Token: 0x04001B5E RID: 7006
		public static readonly LegacySoundStyle Item106 = new LegacySoundStyle(2, 106, SoundType.Sound, 0);

		// Token: 0x04001B5F RID: 7007
		public static readonly LegacySoundStyle Item107 = new LegacySoundStyle(2, 107, SoundType.Sound, 0);

		// Token: 0x04001B60 RID: 7008
		public static readonly LegacySoundStyle Item108 = new LegacySoundStyle(2, 108, SoundType.Sound, 0);

		// Token: 0x04001B61 RID: 7009
		public static readonly LegacySoundStyle Item109 = new LegacySoundStyle(2, 109, SoundType.Sound, 0);

		// Token: 0x04001B62 RID: 7010
		public static readonly LegacySoundStyle Item110 = new LegacySoundStyle(2, 110, SoundType.Sound, 0);

		// Token: 0x04001B63 RID: 7011
		public static readonly LegacySoundStyle Item111 = new LegacySoundStyle(2, 111, SoundType.Sound, 0);

		// Token: 0x04001B64 RID: 7012
		public static readonly LegacySoundStyle Item112 = new LegacySoundStyle(2, 112, SoundType.Sound, 0);

		// Token: 0x04001B65 RID: 7013
		public static readonly LegacySoundStyle Item113 = new LegacySoundStyle(2, 113, SoundType.Sound, 0);

		// Token: 0x04001B66 RID: 7014
		public static readonly LegacySoundStyle Item114 = new LegacySoundStyle(2, 114, SoundType.Sound, 0);

		// Token: 0x04001B67 RID: 7015
		public static readonly LegacySoundStyle Item115 = new LegacySoundStyle(2, 115, SoundType.Sound, 0);

		// Token: 0x04001B68 RID: 7016
		public static readonly LegacySoundStyle Item116 = new LegacySoundStyle(2, 116, SoundType.Sound, 0);

		// Token: 0x04001B69 RID: 7017
		public static readonly LegacySoundStyle Item117 = new LegacySoundStyle(2, 117, SoundType.Sound, 0);

		// Token: 0x04001B6A RID: 7018
		public static readonly LegacySoundStyle Item118 = new LegacySoundStyle(2, 118, SoundType.Sound, 0);

		// Token: 0x04001B6B RID: 7019
		public static readonly LegacySoundStyle Item119 = new LegacySoundStyle(2, 119, SoundType.Sound, 0);

		// Token: 0x04001B6C RID: 7020
		public static readonly LegacySoundStyle Item120 = new LegacySoundStyle(2, 120, SoundType.Sound, 0);

		// Token: 0x04001B6D RID: 7021
		public static readonly LegacySoundStyle Item121 = new LegacySoundStyle(2, 121, SoundType.Sound, 0);

		// Token: 0x04001B6E RID: 7022
		public static readonly LegacySoundStyle Item122 = new LegacySoundStyle(2, 122, SoundType.Sound, 0);

		// Token: 0x04001B6F RID: 7023
		public static readonly LegacySoundStyle Item123 = new LegacySoundStyle(2, 123, SoundType.Sound, 0);

		// Token: 0x04001B70 RID: 7024
		public static readonly LegacySoundStyle Item124 = new LegacySoundStyle(2, 124, SoundType.Sound, 0);

		// Token: 0x04001B71 RID: 7025
		public static readonly LegacySoundStyle Item125 = new LegacySoundStyle(2, 125, SoundType.Sound, 0);

		// Token: 0x04001B72 RID: 7026
		public static readonly LegacySoundStyle Item126 = new LegacySoundStyle(2, 126, SoundType.Sound, 0);

		// Token: 0x04001B73 RID: 7027
		public static readonly LegacySoundStyle Item127 = new LegacySoundStyle(2, 127, SoundType.Sound, 0);

		// Token: 0x04001B74 RID: 7028
		public static readonly LegacySoundStyle Item128 = new LegacySoundStyle(2, 128, SoundType.Sound, 0);

		// Token: 0x04001B75 RID: 7029
		public static readonly LegacySoundStyle Item129 = new LegacySoundStyle(2, 129, SoundType.Sound, 0);

		// Token: 0x04001B76 RID: 7030
		public static readonly LegacySoundStyle Item130 = new LegacySoundStyle(2, 130, SoundType.Sound, 0);

		// Token: 0x04001B77 RID: 7031
		public static readonly LegacySoundStyle Item131 = new LegacySoundStyle(2, 131, SoundType.Sound, 0);

		// Token: 0x04001B78 RID: 7032
		public static readonly LegacySoundStyle Item132 = new LegacySoundStyle(2, 132, SoundType.Sound, 0);

		// Token: 0x04001B79 RID: 7033
		public static readonly LegacySoundStyle Item133 = new LegacySoundStyle(2, 133, SoundType.Sound, 0);

		// Token: 0x04001B7A RID: 7034
		public static readonly LegacySoundStyle Item134 = new LegacySoundStyle(2, 134, SoundType.Sound, 0);

		// Token: 0x04001B7B RID: 7035
		public static readonly LegacySoundStyle Item135 = new LegacySoundStyle(2, 135, SoundType.Sound, 0);

		// Token: 0x04001B7C RID: 7036
		public static readonly LegacySoundStyle Item136 = new LegacySoundStyle(2, 136, SoundType.Sound, 0);

		// Token: 0x04001B7D RID: 7037
		public static readonly LegacySoundStyle Item137 = new LegacySoundStyle(2, 137, SoundType.Sound, 0);

		// Token: 0x04001B7E RID: 7038
		public static readonly LegacySoundStyle Item138 = new LegacySoundStyle(2, 138, SoundType.Sound, 0);

		// Token: 0x04001B7F RID: 7039
		public static readonly LegacySoundStyle Item139 = new LegacySoundStyle(2, 139, SoundType.Sound, 0);

		// Token: 0x04001B80 RID: 7040
		public static readonly LegacySoundStyle Item140 = new LegacySoundStyle(2, 140, SoundType.Sound, 0);

		// Token: 0x04001B81 RID: 7041
		public static readonly LegacySoundStyle Item141 = new LegacySoundStyle(2, 141, SoundType.Sound, 0);

		// Token: 0x04001B82 RID: 7042
		public static readonly LegacySoundStyle Item142 = new LegacySoundStyle(2, 142, SoundType.Sound, 0);

		// Token: 0x04001B83 RID: 7043
		public static readonly LegacySoundStyle Item143 = new LegacySoundStyle(2, 143, SoundType.Sound, 0);

		// Token: 0x04001B84 RID: 7044
		public static readonly LegacySoundStyle Item144 = new LegacySoundStyle(2, 144, SoundType.Sound, 0);

		// Token: 0x04001B85 RID: 7045
		public static readonly LegacySoundStyle Item145 = new LegacySoundStyle(2, 145, SoundType.Sound, 0);

		// Token: 0x04001B86 RID: 7046
		public static readonly LegacySoundStyle Item146 = new LegacySoundStyle(2, 146, SoundType.Sound, 0);

		// Token: 0x04001B87 RID: 7047
		public static readonly LegacySoundStyle Item147 = new LegacySoundStyle(2, 147, SoundType.Sound, 0);

		// Token: 0x04001B88 RID: 7048
		public static readonly LegacySoundStyle Item148 = new LegacySoundStyle(2, 148, SoundType.Sound, 0);

		// Token: 0x04001B89 RID: 7049
		public static readonly LegacySoundStyle Item149 = new LegacySoundStyle(2, 149, SoundType.Sound, 0);

		// Token: 0x04001B8A RID: 7050
		public static readonly LegacySoundStyle Item150 = new LegacySoundStyle(2, 150, SoundType.Sound, 0);

		// Token: 0x04001B8B RID: 7051
		public static readonly LegacySoundStyle Item151 = new LegacySoundStyle(2, 151, SoundType.Sound, 0);

		// Token: 0x04001B8C RID: 7052
		public static readonly LegacySoundStyle Item152 = new LegacySoundStyle(2, 152, SoundType.Sound, 0);

		// Token: 0x04001B8D RID: 7053
		public static readonly LegacySoundStyle Item153 = new LegacySoundStyle(2, 153, SoundType.Sound, 0);

		// Token: 0x04001B8E RID: 7054
		public static readonly LegacySoundStyle Item154 = new LegacySoundStyle(2, 154, SoundType.Sound, 0);

		// Token: 0x04001B8F RID: 7055
		public static readonly LegacySoundStyle Item155 = new LegacySoundStyle(2, 155, SoundType.Sound, 0);

		// Token: 0x04001B90 RID: 7056
		public static readonly LegacySoundStyle Item156 = new LegacySoundStyle(2, 156, SoundType.Sound, 0);

		// Token: 0x04001B91 RID: 7057
		public static readonly LegacySoundStyle Item157 = new LegacySoundStyle(2, 157, SoundType.Sound, 0);

		// Token: 0x04001B92 RID: 7058
		public static readonly LegacySoundStyle Item158 = new LegacySoundStyle(2, 158, SoundType.Sound, 0);

		// Token: 0x04001B93 RID: 7059
		public static readonly LegacySoundStyle Item159 = new LegacySoundStyle(2, 159, SoundType.Sound, 0);

		// Token: 0x04001B94 RID: 7060
		public static readonly LegacySoundStyle Item160 = new LegacySoundStyle(2, 160, SoundType.Sound, 0);

		// Token: 0x04001B95 RID: 7061
		public static readonly LegacySoundStyle Item161 = new LegacySoundStyle(2, 161, SoundType.Sound, 0);

		// Token: 0x04001B96 RID: 7062
		public static readonly LegacySoundStyle Item162 = new LegacySoundStyle(2, 162, SoundType.Sound, 0);

		// Token: 0x04001B97 RID: 7063
		public static readonly LegacySoundStyle Item163 = new LegacySoundStyle(2, 163, SoundType.Sound, 0);

		// Token: 0x04001B98 RID: 7064
		public static readonly LegacySoundStyle Item164 = new LegacySoundStyle(2, 164, SoundType.Sound, 0);

		// Token: 0x04001B99 RID: 7065
		public static readonly LegacySoundStyle Item165 = new LegacySoundStyle(2, 165, SoundType.Sound, 0);

		// Token: 0x04001B9A RID: 7066
		public static readonly LegacySoundStyle Item166 = new LegacySoundStyle(2, 166, SoundType.Sound, 0);

		// Token: 0x04001B9B RID: 7067
		public static readonly LegacySoundStyle Item167 = new LegacySoundStyle(2, 167, SoundType.Sound, 0);

		// Token: 0x04001B9C RID: 7068
		public static readonly LegacySoundStyle Item168 = new LegacySoundStyle(2, 168, SoundType.Sound, 0);

		// Token: 0x04001B9D RID: 7069
		public static readonly LegacySoundStyle Item169 = new LegacySoundStyle(2, 169, SoundType.Sound, 0);

		// Token: 0x04001B9E RID: 7070
		public static readonly LegacySoundStyle Item170 = new LegacySoundStyle(2, 170, SoundType.Sound, 0);

		// Token: 0x04001B9F RID: 7071
		public static readonly LegacySoundStyle Item171 = new LegacySoundStyle(2, 171, SoundType.Sound, 0);

		// Token: 0x04001BA0 RID: 7072
		public static readonly LegacySoundStyle Item172 = new LegacySoundStyle(2, 172, SoundType.Sound, 0);

		// Token: 0x04001BA1 RID: 7073
		public static readonly LegacySoundStyle Item173 = new LegacySoundStyle(2, 173, SoundType.Sound, 0);

		// Token: 0x04001BA2 RID: 7074
		public static readonly LegacySoundStyle Item174 = new LegacySoundStyle(2, 174, SoundType.Sound, 0);

		// Token: 0x04001BA3 RID: 7075
		public static readonly LegacySoundStyle Item175 = new LegacySoundStyle(2, 175, SoundType.Sound, 0);

		// Token: 0x04001BA4 RID: 7076
		public static readonly LegacySoundStyle Item176 = new LegacySoundStyle(2, 176, SoundType.Sound, 0);

		// Token: 0x04001BA5 RID: 7077
		public static readonly LegacySoundStyle Item177 = new LegacySoundStyle(2, 177, SoundType.Sound, 0);

		// Token: 0x04001BA6 RID: 7078
		public static readonly LegacySoundStyle Item178 = new LegacySoundStyle(2, 178, SoundType.Sound, 0);

		// Token: 0x04001BA7 RID: 7079
		public static readonly LegacySoundStyle Item179 = new LegacySoundStyle(2, 179, SoundType.Sound, 0).WithVolume(0.6f);

		// Token: 0x04001BA8 RID: 7080
		public static readonly LegacySoundStyle Item180 = new LegacySoundStyle(2, 180, SoundType.Sound, 0);

		// Token: 0x04001BA9 RID: 7081
		public static readonly LegacySoundStyle Item181 = new LegacySoundStyle(2, 181, SoundType.Sound, 0);

		// Token: 0x04001BAA RID: 7082
		public static readonly LegacySoundStyle Item182 = new LegacySoundStyle(2, 182, SoundType.Sound, 0);

		// Token: 0x04001BAB RID: 7083
		public static readonly LegacySoundStyle Item183 = new LegacySoundStyle(2, 183, SoundType.Sound, 0);

		// Token: 0x04001BAC RID: 7084
		public static readonly LegacySoundStyle Item184 = new LegacySoundStyle(2, 184, SoundType.Sound, 0);

		// Token: 0x04001BAD RID: 7085
		public static readonly LegacySoundStyle Item185 = new LegacySoundStyle(2, 185, SoundType.Sound, 0);

		// Token: 0x04001BAE RID: 7086
		public static readonly LegacySoundStyle Item186 = new LegacySoundStyle(2, 186, SoundType.Sound, 0);

		// Token: 0x04001BAF RID: 7087
		public static readonly LegacySoundStyle Item187 = new LegacySoundStyle(2, 187, SoundType.Sound, 0);

		// Token: 0x04001BB0 RID: 7088
		public static readonly LegacySoundStyle Item188 = new LegacySoundStyle(2, 188, SoundType.Sound, 0);

		// Token: 0x04001BB1 RID: 7089
		public static readonly LegacySoundStyle Item189 = new LegacySoundStyle(2, 189, SoundType.Sound, 0);

		// Token: 0x04001BB2 RID: 7090
		public static readonly LegacySoundStyle Item190 = new LegacySoundStyle(2, 190, SoundType.Sound, 0);

		// Token: 0x04001BB3 RID: 7091
		public static readonly LegacySoundStyle Item191 = new LegacySoundStyle(2, 191, SoundType.Sound, 0);

		// Token: 0x04001BB4 RID: 7092
		public static readonly LegacySoundStyle Item192 = new LegacySoundStyle(2, 192, SoundType.Sound, 0);

		// Token: 0x04001BB5 RID: 7093
		public static readonly LegacySoundStyle Item193 = new LegacySoundStyle(2, 193, SoundType.Sound, 0);

		// Token: 0x04001BB6 RID: 7094
		public static readonly LegacySoundStyle Item194 = new LegacySoundStyle(2, 194, SoundType.Sound, 0);

		// Token: 0x04001BB7 RID: 7095
		public static readonly LegacySoundStyle Item195 = new LegacySoundStyle(2, 195, SoundType.Sound, 0);

		// Token: 0x04001BB8 RID: 7096
		public static readonly LegacySoundStyle Item196 = new LegacySoundStyle(2, 196, SoundType.Sound, 0);

		// Token: 0x04001BB9 RID: 7097
		public static readonly LegacySoundStyle Item197 = new LegacySoundStyle(2, 197, SoundType.Sound, 0);

		// Token: 0x04001BBA RID: 7098
		public static readonly LegacySoundStyle Item198 = new LegacySoundStyle(2, 198, SoundType.Sound, 0);

		// Token: 0x04001BBB RID: 7099
		public static readonly LegacySoundStyle Item199 = new LegacySoundStyle(2, 199, SoundType.Sound, 0);

		// Token: 0x04001BBC RID: 7100
		public static short ItemSoundCount = 200;

		// Token: 0x04001BBD RID: 7101
		public static readonly LegacySoundStyle DD2_GoblinBomb = new LegacySoundStyle(2, 14, SoundType.Sound, 0).WithVolume(0.5f);

		// Token: 0x04001BBE RID: 7102
		public static readonly LegacySoundStyle AchievementComplete = SoundID.CreateTrackable("achievement_complete", SoundType.Sound, 0);

		// Token: 0x04001BBF RID: 7103
		public static readonly LegacySoundStyle BlizzardInsideBuildingLoop = SoundID.CreateTrackable("blizzard_inside_building_loop", SoundType.Ambient, 0);

		// Token: 0x04001BC0 RID: 7104
		public static readonly LegacySoundStyle BlizzardStrongLoop = SoundID.CreateTrackable("blizzard_strong_loop", SoundType.Ambient, 0).WithVolume(0.5f);

		// Token: 0x04001BC1 RID: 7105
		public static readonly LegacySoundStyle LiquidsHoneyWater = SoundID.CreateTrackable("liquids_honey_water", 3, SoundType.Ambient, 0);

		// Token: 0x04001BC2 RID: 7106
		public static readonly LegacySoundStyle LiquidsHoneyLava = SoundID.CreateTrackable("liquids_honey_lava", 3, SoundType.Ambient, 0);

		// Token: 0x04001BC3 RID: 7107
		public static readonly LegacySoundStyle LiquidsWaterLava = SoundID.CreateTrackable("liquids_water_lava", 3, SoundType.Ambient, 0);

		// Token: 0x04001BC4 RID: 7108
		public static readonly LegacySoundStyle DD2_BallistaTowerShot = SoundID.CreateTrackable("dd2_ballista_tower_shot", 3, SoundType.Sound, 0);

		// Token: 0x04001BC5 RID: 7109
		public static readonly LegacySoundStyle DD2_ExplosiveTrapExplode = SoundID.CreateTrackable("dd2_explosive_trap_explode", 3, SoundType.Sound, 0);

		// Token: 0x04001BC6 RID: 7110
		public static readonly LegacySoundStyle DD2_FlameburstTowerShot = SoundID.CreateTrackable("dd2_flameburst_tower_shot", 3, SoundType.Sound, 6);

		// Token: 0x04001BC7 RID: 7111
		public static readonly LegacySoundStyle DD2_LightningAuraZap = SoundID.CreateTrackable("dd2_lightning_aura_zap", 4, SoundType.Sound, 0);

		// Token: 0x04001BC8 RID: 7112
		public static readonly LegacySoundStyle DD2_DefenseTowerSpawn = SoundID.CreateTrackable("dd2_defense_tower_spawn", SoundType.Sound, 0);

		// Token: 0x04001BC9 RID: 7113
		public static readonly LegacySoundStyle DD2_BetsyDeath = SoundID.CreateTrackable("dd2_betsy_death", 3, SoundType.Sound, 0);

		// Token: 0x04001BCA RID: 7114
		public static readonly LegacySoundStyle DD2_BetsyFireballShot = SoundID.CreateTrackable("dd2_betsy_fireball_shot", 3, SoundType.Sound, 0);

		// Token: 0x04001BCB RID: 7115
		public static readonly LegacySoundStyle DD2_BetsyFireballImpact = SoundID.CreateTrackable("dd2_betsy_fireball_impact", 3, SoundType.Sound, 0);

		// Token: 0x04001BCC RID: 7116
		public static readonly LegacySoundStyle DD2_BetsyFlameBreath = SoundID.CreateTrackable("dd2_betsy_flame_breath", SoundType.Sound, 0);

		// Token: 0x04001BCD RID: 7117
		public static readonly LegacySoundStyle DD2_BetsyFlyingCircleAttack = SoundID.CreateTrackable("dd2_betsy_flying_circle_attack", SoundType.Sound, 0);

		// Token: 0x04001BCE RID: 7118
		public static readonly LegacySoundStyle DD2_BetsyHurt = SoundID.CreateTrackable("dd2_betsy_hurt", 3, SoundType.Sound, 0);

		// Token: 0x04001BCF RID: 7119
		public static readonly LegacySoundStyle DD2_BetsyScream = SoundID.CreateTrackable("dd2_betsy_scream", SoundType.Sound, 0);

		// Token: 0x04001BD0 RID: 7120
		public static readonly LegacySoundStyle DD2_BetsySummon = SoundID.CreateTrackable("dd2_betsy_summon", 3, SoundType.Sound, 0);

		// Token: 0x04001BD1 RID: 7121
		public static readonly LegacySoundStyle DD2_BetsyWindAttack = SoundID.CreateTrackable("dd2_betsy_wind_attack", 3, SoundType.Sound, 0);

		// Token: 0x04001BD2 RID: 7122
		public static readonly LegacySoundStyle DD2_DarkMageAttack = SoundID.CreateTrackable("dd2_dark_mage_attack", 3, SoundType.Sound, 0);

		// Token: 0x04001BD3 RID: 7123
		public static readonly LegacySoundStyle DD2_DarkMageCastHeal = SoundID.CreateTrackable("dd2_dark_mage_cast_heal", 3, SoundType.Sound, 0);

		// Token: 0x04001BD4 RID: 7124
		public static readonly LegacySoundStyle DD2_DarkMageDeath = SoundID.CreateTrackable("dd2_dark_mage_death", 3, SoundType.Sound, 0);

		// Token: 0x04001BD5 RID: 7125
		public static readonly LegacySoundStyle DD2_DarkMageHealImpact = SoundID.CreateTrackable("dd2_dark_mage_heal_impact", 3, SoundType.Sound, 0);

		// Token: 0x04001BD6 RID: 7126
		public static readonly LegacySoundStyle DD2_DarkMageHurt = SoundID.CreateTrackable("dd2_dark_mage_hurt", 3, SoundType.Sound, 0);

		// Token: 0x04001BD7 RID: 7127
		public static readonly LegacySoundStyle DD2_DarkMageSummonSkeleton = SoundID.CreateTrackable("dd2_dark_mage_summon_skeleton", 3, SoundType.Sound, 0);

		// Token: 0x04001BD8 RID: 7128
		public static readonly LegacySoundStyle DD2_DrakinBreathIn = SoundID.CreateTrackable("dd2_drakin_breath_in", 3, SoundType.Sound, 0);

		// Token: 0x04001BD9 RID: 7129
		public static readonly LegacySoundStyle DD2_DrakinDeath = SoundID.CreateTrackable("dd2_drakin_death", 3, SoundType.Sound, 0);

		// Token: 0x04001BDA RID: 7130
		public static readonly LegacySoundStyle DD2_DrakinHurt = SoundID.CreateTrackable("dd2_drakin_hurt", 3, SoundType.Sound, 0);

		// Token: 0x04001BDB RID: 7131
		public static readonly LegacySoundStyle DD2_DrakinShot = SoundID.CreateTrackable("dd2_drakin_shot", 3, SoundType.Sound, 0);

		// Token: 0x04001BDC RID: 7132
		public static readonly LegacySoundStyle DD2_GoblinDeath = SoundID.CreateTrackable("dd2_goblin_death", 3, SoundType.Sound, 0);

		// Token: 0x04001BDD RID: 7133
		public static readonly LegacySoundStyle DD2_GoblinHurt = SoundID.CreateTrackable("dd2_goblin_hurt", 6, SoundType.Sound, 0);

		// Token: 0x04001BDE RID: 7134
		public static readonly LegacySoundStyle DD2_GoblinScream = SoundID.CreateTrackable("dd2_goblin_scream", 3, SoundType.Sound, 0);

		// Token: 0x04001BDF RID: 7135
		public static readonly LegacySoundStyle DD2_GoblinBomberDeath = SoundID.CreateTrackable("dd2_goblin_bomber_death", 3, SoundType.Sound, 0);

		// Token: 0x04001BE0 RID: 7136
		public static readonly LegacySoundStyle DD2_GoblinBomberHurt = SoundID.CreateTrackable("dd2_goblin_bomber_hurt", 3, SoundType.Sound, 0);

		// Token: 0x04001BE1 RID: 7137
		public static readonly LegacySoundStyle DD2_GoblinBomberScream = SoundID.CreateTrackable("dd2_goblin_bomber_scream", 3, SoundType.Sound, 0);

		// Token: 0x04001BE2 RID: 7138
		public static readonly LegacySoundStyle DD2_GoblinBomberThrow = SoundID.CreateTrackable("dd2_goblin_bomber_throw", 3, SoundType.Sound, 0);

		// Token: 0x04001BE3 RID: 7139
		public static readonly LegacySoundStyle DD2_JavelinThrowersAttack = SoundID.CreateTrackable("dd2_javelin_throwers_attack", 3, SoundType.Sound, 0);

		// Token: 0x04001BE4 RID: 7140
		public static readonly LegacySoundStyle DD2_JavelinThrowersDeath = SoundID.CreateTrackable("dd2_javelin_throwers_death", 3, SoundType.Sound, 0);

		// Token: 0x04001BE5 RID: 7141
		public static readonly LegacySoundStyle DD2_JavelinThrowersHurt = SoundID.CreateTrackable("dd2_javelin_throwers_hurt", 3, SoundType.Sound, 0);

		// Token: 0x04001BE6 RID: 7142
		public static readonly LegacySoundStyle DD2_JavelinThrowersTaunt = SoundID.CreateTrackable("dd2_javelin_throwers_taunt", 3, SoundType.Sound, 0);

		// Token: 0x04001BE7 RID: 7143
		public static readonly LegacySoundStyle DD2_KoboldDeath = SoundID.CreateTrackable("dd2_kobold_death", 3, SoundType.Sound, 0);

		// Token: 0x04001BE8 RID: 7144
		public static readonly LegacySoundStyle DD2_KoboldExplosion = SoundID.CreateTrackable("dd2_kobold_explosion", 3, SoundType.Sound, 0);

		// Token: 0x04001BE9 RID: 7145
		public static readonly LegacySoundStyle DD2_KoboldHurt = SoundID.CreateTrackable("dd2_kobold_hurt", 3, SoundType.Sound, 0);

		// Token: 0x04001BEA RID: 7146
		public static readonly LegacySoundStyle DD2_KoboldIgnite = SoundID.CreateTrackable("dd2_kobold_ignite", SoundType.Sound, 0);

		// Token: 0x04001BEB RID: 7147
		public static readonly LegacySoundStyle DD2_KoboldIgniteLoop = SoundID.CreateTrackable("dd2_kobold_ignite_loop", SoundType.Sound, 0);

		// Token: 0x04001BEC RID: 7148
		public static readonly LegacySoundStyle DD2_KoboldScreamChargeLoop = SoundID.CreateTrackable("dd2_kobold_scream_charge_loop", SoundType.Sound, 0);

		// Token: 0x04001BED RID: 7149
		public static readonly LegacySoundStyle DD2_KoboldFlyerChargeScream = SoundID.CreateTrackable("dd2_kobold_flyer_charge_scream", 3, SoundType.Sound, 0);

		// Token: 0x04001BEE RID: 7150
		public static readonly LegacySoundStyle DD2_KoboldFlyerDeath = SoundID.CreateTrackable("dd2_kobold_flyer_death", 3, SoundType.Sound, 0);

		// Token: 0x04001BEF RID: 7151
		public static readonly LegacySoundStyle DD2_KoboldFlyerHurt = SoundID.CreateTrackable("dd2_kobold_flyer_hurt", 3, SoundType.Sound, 0);

		// Token: 0x04001BF0 RID: 7152
		public static readonly LegacySoundStyle DD2_LightningBugDeath = SoundID.CreateTrackable("dd2_lightning_bug_death", 3, SoundType.Sound, 0);

		// Token: 0x04001BF1 RID: 7153
		public static readonly LegacySoundStyle DD2_LightningBugHurt = SoundID.CreateTrackable("dd2_lightning_bug_hurt", 3, SoundType.Sound, 0);

		// Token: 0x04001BF2 RID: 7154
		public static readonly LegacySoundStyle DD2_LightningBugZap = SoundID.CreateTrackable("dd2_lightning_bug_zap", 3, SoundType.Sound, 0);

		// Token: 0x04001BF3 RID: 7155
		public static readonly LegacySoundStyle DD2_OgreAttack = SoundID.CreateTrackable("dd2_ogre_attack", 3, SoundType.Sound, 0);

		// Token: 0x04001BF4 RID: 7156
		public static readonly LegacySoundStyle DD2_OgreDeath = SoundID.CreateTrackable("dd2_ogre_death", 3, SoundType.Sound, 0);

		// Token: 0x04001BF5 RID: 7157
		public static readonly LegacySoundStyle DD2_OgreGroundPound = SoundID.CreateTrackable("dd2_ogre_ground_pound", SoundType.Sound, 0);

		// Token: 0x04001BF6 RID: 7158
		public static readonly LegacySoundStyle DD2_OgreHurt = SoundID.CreateTrackable("dd2_ogre_hurt", 3, SoundType.Sound, 0);

		// Token: 0x04001BF7 RID: 7159
		public static readonly LegacySoundStyle DD2_OgreRoar = SoundID.CreateTrackable("dd2_ogre_roar", 3, SoundType.Sound, 0);

		// Token: 0x04001BF8 RID: 7160
		public static readonly LegacySoundStyle DD2_OgreSpit = SoundID.CreateTrackable("dd2_ogre_spit", SoundType.Sound, 0);

		// Token: 0x04001BF9 RID: 7161
		public static readonly LegacySoundStyle DD2_SkeletonDeath = SoundID.CreateTrackable("dd2_skeleton_death", 3, SoundType.Sound, 0);

		// Token: 0x04001BFA RID: 7162
		public static readonly LegacySoundStyle DD2_SkeletonHurt = SoundID.CreateTrackable("dd2_skeleton_hurt", 3, SoundType.Sound, 0);

		// Token: 0x04001BFB RID: 7163
		public static readonly LegacySoundStyle DD2_SkeletonSummoned = SoundID.CreateTrackable("dd2_skeleton_summoned", SoundType.Sound, 0);

		// Token: 0x04001BFC RID: 7164
		public static readonly LegacySoundStyle DD2_WitherBeastAuraPulse = SoundID.CreateTrackable("dd2_wither_beast_aura_pulse", 2, SoundType.Sound, 0);

		// Token: 0x04001BFD RID: 7165
		public static readonly LegacySoundStyle DD2_WitherBeastCrystalImpact = SoundID.CreateTrackable("dd2_wither_beast_crystal_impact", 3, SoundType.Sound, 0);

		// Token: 0x04001BFE RID: 7166
		public static readonly LegacySoundStyle DD2_WitherBeastDeath = SoundID.CreateTrackable("dd2_wither_beast_death", 3, SoundType.Sound, 0);

		// Token: 0x04001BFF RID: 7167
		public static readonly LegacySoundStyle DD2_WitherBeastHurt = SoundID.CreateTrackable("dd2_wither_beast_hurt", 3, SoundType.Sound, 0);

		// Token: 0x04001C00 RID: 7168
		public static readonly LegacySoundStyle DD2_WyvernDeath = SoundID.CreateTrackable("dd2_wyvern_death", 3, SoundType.Sound, 0);

		// Token: 0x04001C01 RID: 7169
		public static readonly LegacySoundStyle DD2_WyvernHurt = SoundID.CreateTrackable("dd2_wyvern_hurt", 3, SoundType.Sound, 0);

		// Token: 0x04001C02 RID: 7170
		public static readonly LegacySoundStyle DD2_WyvernScream = SoundID.CreateTrackable("dd2_wyvern_scream", 3, SoundType.Sound, 0);

		// Token: 0x04001C03 RID: 7171
		public static readonly LegacySoundStyle DD2_WyvernDiveDown = SoundID.CreateTrackable("dd2_wyvern_dive_down", 3, SoundType.Sound, 0);

		// Token: 0x04001C04 RID: 7172
		public static readonly LegacySoundStyle DD2_EtherianPortalDryadTouch = SoundID.CreateTrackable("dd2_etherian_portal_dryad_touch", SoundType.Sound, 0);

		// Token: 0x04001C05 RID: 7173
		public static readonly LegacySoundStyle DD2_EtherianPortalIdleLoop = SoundID.CreateTrackable("dd2_etherian_portal_idle_loop", SoundType.Sound, 0);

		// Token: 0x04001C06 RID: 7174
		public static readonly LegacySoundStyle DD2_EtherianPortalOpen = SoundID.CreateTrackable("dd2_etherian_portal_open", SoundType.Sound, 0);

		// Token: 0x04001C07 RID: 7175
		public static readonly LegacySoundStyle DD2_EtherianPortalSpawnEnemy = SoundID.CreateTrackable("dd2_etherian_portal_spawn_enemy", 3, SoundType.Sound, 0);

		// Token: 0x04001C08 RID: 7176
		public static readonly LegacySoundStyle DD2_CrystalCartImpact = SoundID.CreateTrackable("dd2_crystal_cart_impact", 3, SoundType.Sound, 0);

		// Token: 0x04001C09 RID: 7177
		public static readonly LegacySoundStyle DD2_DefeatScene = SoundID.CreateTrackable("dd2_defeat_scene", SoundType.Sound, 0);

		// Token: 0x04001C0A RID: 7178
		public static readonly LegacySoundStyle DD2_WinScene = SoundID.CreateTrackable("dd2_win_scene", SoundType.Sound, 0);

		// Token: 0x04001C0B RID: 7179
		public static readonly LegacySoundStyle DD2_BetsysWrathShot = SoundID.DD2_BetsyFireballShot.WithVolume(0.4f);

		// Token: 0x04001C0C RID: 7180
		public static readonly LegacySoundStyle DD2_BetsysWrathImpact = SoundID.DD2_BetsyFireballImpact.WithVolume(0.4f);

		// Token: 0x04001C0D RID: 7181
		public static readonly LegacySoundStyle DD2_BookStaffCast = SoundID.CreateTrackable("dd2_book_staff_cast", 3, SoundType.Sound, 0);

		// Token: 0x04001C0E RID: 7182
		public static readonly LegacySoundStyle DD2_BookStaffTwisterLoop = SoundID.CreateTrackable("dd2_book_staff_twister_loop", SoundType.Sound, 0);

		// Token: 0x04001C0F RID: 7183
		public static readonly LegacySoundStyle DD2_GhastlyGlaiveImpactGhost = SoundID.CreateTrackable("dd2_ghastly_glaive_impact_ghost", 3, SoundType.Sound, 0);

		// Token: 0x04001C10 RID: 7184
		public static readonly LegacySoundStyle DD2_GhastlyGlaivePierce = SoundID.CreateTrackable("dd2_ghastly_glaive_pierce", 3, SoundType.Sound, 0);

		// Token: 0x04001C11 RID: 7185
		public static readonly LegacySoundStyle DD2_MonkStaffGroundImpact = SoundID.CreateTrackable("dd2_monk_staff_ground_impact", 3, SoundType.Sound, 0);

		// Token: 0x04001C12 RID: 7186
		public static readonly LegacySoundStyle DD2_MonkStaffGroundMiss = SoundID.CreateTrackable("dd2_monk_staff_ground_miss", 3, SoundType.Sound, 0);

		// Token: 0x04001C13 RID: 7187
		public static readonly LegacySoundStyle DD2_MonkStaffSwing = SoundID.CreateTrackable("dd2_monk_staff_swing", 4, SoundType.Sound, 0);

		// Token: 0x04001C14 RID: 7188
		public static readonly LegacySoundStyle DD2_PhantomPhoenixShot = SoundID.CreateTrackable("dd2_phantom_phoenix_shot", 3, SoundType.Sound, 0);

		// Token: 0x04001C15 RID: 7189
		public static readonly LegacySoundStyle DD2_SonicBoomBladeSlash = SoundID.CreateTrackable("dd2_sonic_boom_blade_slash", 3, SoundID.ItemDefaults, 0).WithVolume(0.5f);

		// Token: 0x04001C16 RID: 7190
		public static readonly LegacySoundStyle DD2_SkyDragonsFuryCircle = SoundID.CreateTrackable("dd2_sky_dragons_fury_circle", 3, SoundType.Sound, 0);

		// Token: 0x04001C17 RID: 7191
		public static readonly LegacySoundStyle DD2_SkyDragonsFuryShot = SoundID.CreateTrackable("dd2_sky_dragons_fury_shot", 3, SoundType.Sound, 0);

		// Token: 0x04001C18 RID: 7192
		public static readonly LegacySoundStyle DD2_SkyDragonsFurySwing = SoundID.CreateTrackable("dd2_sky_dragons_fury_swing", 4, SoundType.Sound, 0);

		// Token: 0x04001C19 RID: 7193
		public static readonly LegacySoundStyle PlayerHurt1 = new LegacySoundStyle(1, 1, SoundType.Sound, 0);

		// Token: 0x04001C1A RID: 7194
		public static readonly LegacySoundStyle PlayerHurt2 = new LegacySoundStyle(20, 1, SoundType.Sound, 0);

		// Token: 0x04001C1B RID: 7195
		public static readonly LegacySoundStyle LucyTheAxeTalk = SoundID.CreateTrackable("lucyaxe_talk", 5, SoundType.Sound, 0).WithVolume(0.4f).WithPitchVariance(0.1f);

		// Token: 0x04001C1C RID: 7196
		public static readonly LegacySoundStyle DeerclopsHit = SoundID.CreateTrackable("deerclops_hit", 3, SoundType.Sound, 0).WithVolume(0.3f);

		// Token: 0x04001C1D RID: 7197
		public static readonly LegacySoundStyle DeerclopsDeath = SoundID.CreateTrackable("deerclops_death", SoundType.Sound, 0);

		// Token: 0x04001C1E RID: 7198
		public static readonly LegacySoundStyle DeerclopsScream = SoundID.CreateTrackable("deerclops_scream", 3, SoundType.Sound, 0);

		// Token: 0x04001C1F RID: 7199
		public static readonly LegacySoundStyle DeerclopsIceAttack = SoundID.CreateTrackable("deerclops_ice_attack", 3, SoundType.Sound, 0).WithVolume(0.1f);

		// Token: 0x04001C20 RID: 7200
		public static readonly LegacySoundStyle DeerclopsRubbleAttack = SoundID.CreateTrackable("deerclops_rubble_attack", SoundType.Sound, 0).WithVolume(0.5f);

		// Token: 0x04001C21 RID: 7201
		public static readonly LegacySoundStyle DeerclopsStep = SoundID.CreateTrackable("deerclops_step", SoundType.Sound, 0).WithVolume(0.2f);

		// Token: 0x04001C22 RID: 7202
		public static readonly LegacySoundStyle ChesterOpen = SoundID.CreateTrackable("chester_open", 2, SoundType.Sound, 0);

		// Token: 0x04001C23 RID: 7203
		public static readonly LegacySoundStyle ChesterClose = SoundID.CreateTrackable("chester_close", 2, SoundType.Sound, 0);

		// Token: 0x04001C24 RID: 7204
		public static readonly LegacySoundStyle AbigailSummon = SoundID.CreateTrackable("abigail_summon", SoundType.Sound, 0);

		// Token: 0x04001C25 RID: 7205
		public static readonly LegacySoundStyle AbigailCry = SoundID.CreateTrackable("abigail_cry", 3, SoundType.Sound, 0).WithVolume(0.4f);

		// Token: 0x04001C26 RID: 7206
		public static readonly LegacySoundStyle AbigailAttack = SoundID.CreateTrackable("abigail_attack", SoundType.Sound, 0).WithVolume(0.35f);

		// Token: 0x04001C27 RID: 7207
		public static readonly LegacySoundStyle AbigailUpgrade = SoundID.CreateTrackable("abigail_upgrade", 3, SoundType.Sound, 0).WithVolume(0.5f);

		// Token: 0x04001C28 RID: 7208
		public static readonly LegacySoundStyle GlommerBounce = SoundID.CreateTrackable("glommer_bounce", 2, SoundType.Sound, 0).WithVolume(0.5f);

		// Token: 0x04001C29 RID: 7209
		public static readonly LegacySoundStyle DSTMaleHurt = SoundID.CreateTrackable("dst_male_hit", 3, SoundType.Sound, 0).WithVolume(0.1f);

		// Token: 0x04001C2A RID: 7210
		public static readonly LegacySoundStyle DSTFemaleHurt = SoundID.CreateTrackable("dst_female_hit", 3, SoundType.Sound, 0).WithVolume(0.1f);

		// Token: 0x04001C2B RID: 7211
		public static readonly LegacySoundStyle JimsDrone = SoundID.CreateTrackable("Drone", SoundType.Sound, 0).WithVolume(0.1f);

		// Token: 0x04001C2C RID: 7212
		public static readonly LegacySoundStyle RCCar = SoundID.CreateTrackable("rccar", SoundType.Sound, 0).WithVolume(0.015f);

		// Token: 0x04001C2D RID: 7213
		public static readonly LegacySoundStyle VampireSizzle = SoundID.CreateTrackable("sizzle", SoundType.Sound, 0).WithVolume(1f);

		// Token: 0x04001C2E RID: 7214
		public static readonly LegacySoundStyle RainbowBoulder = SoundID.CreateTrackable("rainbow_boulder", SoundType.Sound, 0);

		// Token: 0x04001C2F RID: 7215
		public static readonly LegacySoundStyle MenuAccept = SoundID.CreateTrackable("menu_accept", SoundType.Sound, 0);

		// Token: 0x04001C30 RID: 7216
		public static readonly LegacySoundStyle Hungry = SoundID.CreateTrackable("hungry", SoundType.Sound, 0);

		// Token: 0x04001C31 RID: 7217
		public static readonly LegacySoundStyle PalSummon = SoundID.CreateTrackable("pal_summon", SoundType.Sound, 0).WithVolume(0.15f);

		// Token: 0x04001C32 RID: 7218
		public static readonly LegacySoundStyle PalCattiva = SoundID.CreateTrackable("pal_cattiva", SoundType.Sound, 0).WithVolume(0.35f);

		// Token: 0x04001C33 RID: 7219
		public static readonly LegacySoundStyle PalCattivaPain = SoundID.CreateTrackable("pal_cattiva_pain", SoundType.Sound, 0).WithVolume(0.35f);

		// Token: 0x04001C34 RID: 7220
		public static readonly LegacySoundStyle PalCattivaJoy = SoundID.CreateTrackable("pal_cattiva_joy", SoundType.Sound, 0).WithVolume(0.35f);

		// Token: 0x04001C35 RID: 7221
		public static readonly LegacySoundStyle PalChillet = SoundID.CreateTrackable("pal_chillet", SoundType.Sound, 0).WithVolume(0.35f);

		// Token: 0x04001C36 RID: 7222
		public static readonly LegacySoundStyle PalChilletJoy = SoundID.CreateTrackable("pal_chillet_joy", SoundType.Sound, 0).WithVolume(0.35f);

		// Token: 0x04001C37 RID: 7223
		public static readonly LegacySoundStyle PalChilletAttack = SoundID.CreateTrackable("pal_chillet_attack", SoundType.Sound, 0).WithVolume(0.35f);

		// Token: 0x04001C38 RID: 7224
		public static readonly LegacySoundStyle PalFoxparks = SoundID.CreateTrackable("pal_foxparks", SoundType.Sound, 0).WithVolume(0.35f);

		// Token: 0x04001C39 RID: 7225
		public static readonly LegacySoundStyle PalFoxparksPain = SoundID.CreateTrackable("pal_foxparks_pain", SoundType.Sound, 0).WithVolume(0.35f);

		// Token: 0x04001C3A RID: 7226
		public static readonly LegacySoundStyle PalFoxparksJoy = SoundID.CreateTrackable("pal_foxparks_joy", SoundType.Sound, 0).WithVolume(0.35f);

		// Token: 0x04001C3B RID: 7227
		public static readonly LegacySoundStyle PalFoxparksAttack = SoundID.CreateTrackable("pal_foxparks_attack", SoundType.Sound, 0).WithVolume(0.18f);

		// Token: 0x04001C3C RID: 7228
		public static readonly LegacySoundStyle PalDigtoise = SoundID.CreateTrackable("pal_digtoise", SoundType.Sound, 0).WithVolume(0.35f);

		// Token: 0x04001C3D RID: 7229
		public static readonly LegacySoundStyle PalDigtoiseJoy = SoundID.CreateTrackable("pal_digtoise_joy", SoundType.Sound, 0).WithVolume(0.35f);

		// Token: 0x04001C3E RID: 7230
		public static readonly LegacySoundStyle FoxparksFlame = new LegacySoundStyle(2, 34, SoundType.Sound, 0).WithVolume(0.15f);

		// Token: 0x04001C3F RID: 7231
		public static readonly LegacySoundStyle LeafBlower = new LegacySoundStyle(2, 34, SoundType.Sound, 0).WithVolume(0.13f);

		// Token: 0x04001C40 RID: 7232
		public static readonly LegacySoundStyle DeadCellsBarrelLauncherFire = SoundID.CreateTrackable("deadcells_barrel_launcher_fire", SoundType.Sound, 0).WithVolume(0.5f);

		// Token: 0x04001C41 RID: 7233
		public static readonly LegacySoundStyle DeadCellsBarrelLauncherExplode = SoundID.CreateTrackable("deadcells_barrel_launcher_explode", SoundType.Sound, 0).WithVolume(0.6f);

		// Token: 0x04001C42 RID: 7234
		public static readonly LegacySoundStyle DeadCellsMushroomSummon = SoundID.CreateTrackable("deadcells_mushroom_summon", SoundType.Sound, 0).WithVolume(0.35f);

		// Token: 0x04001C43 RID: 7235
		public static readonly LegacySoundStyle DeadCellsMushroomLand = SoundID.CreateTrackable("deadcells_mushroom_land", SoundType.Sound, 0).WithVolume(0.35f);

		// Token: 0x04001C44 RID: 7236
		public static readonly LegacySoundStyle DeadCellsMushroomJump = SoundID.CreateTrackable("deadcells_mushroom_jump", SoundType.Sound, 0).WithVolume(0.35f);

		// Token: 0x04001C45 RID: 7237
		public static readonly LegacySoundStyle DeadCellsMushroomExplode = SoundID.CreateTrackable("deadcells_mushroom_explode", SoundType.Sound, 0).WithVolume(0.15f);

		// Token: 0x04001C46 RID: 7238
		public static readonly LegacySoundStyle DeadCellsFlintCharge = SoundID.CreateTrackable("deadcells_flint_charge", SoundType.Sound, 0).WithVolume(0.5f);

		// Token: 0x04001C47 RID: 7239
		public static readonly LegacySoundStyle DeadCellsFlintRelease = SoundID.CreateTrackable("deadcells_flint_release", SoundType.Sound, 0).WithVolume(0.5f);

		// Token: 0x04001C48 RID: 7240
		public static readonly LegacySoundStyle DeadCellsFlintWave = SoundID.CreateTrackable("deadcells_flint_wave", SoundType.Sound, 0).WithVolume(0.1f);

		// Token: 0x04001C49 RID: 7241
		public static readonly LegacySoundStyle MeteorShower = SoundID.CreateTrackable("meteor_shower", 6, SoundType.Sound, 0).WithVolume(0.8f).WithPitchVariance(0.3f);

		// Token: 0x04001C4A RID: 7242
		public static readonly LegacySoundStyle BestReforge = SoundID.CreateTrackable("best_reforge", SoundType.Sound, 0).WithPitchVariance(0.1f);

		// Token: 0x04001C4B RID: 7243
		public static readonly LegacySoundStyle TrashItem = SoundID.CreateTrackable("trash_item", 2, SoundType.Sound, 0).WithVolume(0.55f).WithPitchVariance(0.25f);

		// Token: 0x04001C4C RID: 7244
		public static readonly LegacySoundStyle InstantThunder = SoundID.CreateTrackable("instant_thunder", 3, SoundType.Sound, 0);

		// Token: 0x04001C4D RID: 7245
		public static readonly LegacySoundStyle SonarPotion = SoundID.CreateTrackable("sonar_potion", SoundType.Sound, 0).WithVolume(0.65f).WithPitchVariance(0.03f);

		// Token: 0x04001C4E RID: 7246
		public static readonly LegacySoundStyle StatueMimicScare = SoundID.CreateTrackable("statuemimic_scare", SoundType.Sound, 0).WithVolume(0.8f).WithPitchVariance(0.1f);

		// Token: 0x04001C4F RID: 7247
		public static readonly LegacySoundStyle StatueMimicJump = SoundID.CreateTrackable("statuemimic_jump", SoundType.Sound, 0).WithVolume(0.3f).WithPitchVariance(0.1f);

		// Token: 0x04001C50 RID: 7248
		public static readonly LegacySoundStyle StatueMimicLaugh = SoundID.CreateTrackable("statuemimic_laugh", 3, SoundType.Sound, 0).WithVolume(0.8f).WithPitchVariance(0.1f);

		// Token: 0x04001C51 RID: 7249
		public const float playerVoicePitchVariance = 0.4f;

		// Token: 0x04001C52 RID: 7250
		public static readonly LegacySoundStyle DefaultPlayerHurt = SoundID.CreateTrackable("player_hit_default", SoundType.Sound, 0).WithVolume(0.9f).WithPitchVariance(0.2f);

		// Token: 0x04001C53 RID: 7251
		public static readonly LegacySoundStyle BellHurt = new LegacySoundStyle(2, 35, SoundType.Sound, 0).WithPitchVariance(0.4f);

		// Token: 0x04001C54 RID: 7252
		public static readonly LegacySoundStyle ChickenHurt = SoundID.CreateTrackable("player_hit_chicken", SoundType.Sound, 0).WithVolume(0.8f).WithPitchVariance(0.4f);

		// Token: 0x04001C55 RID: 7253
		public static readonly LegacySoundStyle ChickenHurtRare = SoundID.CreateTrackable("player_hit_chicken_rare", SoundType.Sound, 0).WithVolume(0.5f).WithPitchVariance(0.4f);

		// Token: 0x04001C56 RID: 7254
		public static readonly LegacySoundStyle FrogHurt = SoundID.CreateTrackable("player_hit_frog", SoundType.Sound, 0).WithVolume(0.5f).WithPitchVariance(0.4f);

		// Token: 0x04001C57 RID: 7255
		public static readonly LegacySoundStyle GoatHurt = SoundID.CreateTrackable("player_hit_goat", SoundType.Sound, 0).WithVolume(0.5f).WithPitchVariance(0.4f);

		// Token: 0x04001C58 RID: 7256
		public static readonly LegacySoundStyle RetroHurt = SoundID.CreateTrackable("player_hit_retro", SoundType.Sound, 0).WithVolume(0.5f).WithPitchVariance(0.4f);

		// Token: 0x04001C59 RID: 7257
		public static readonly LegacySoundStyle RetroDeath = SoundID.CreateTrackable("player_death_retro", SoundType.Sound, 0).WithVolume(0.5f).WithPitchVariance(0.4f);

		// Token: 0x04001C5A RID: 7258
		public static readonly LegacySoundStyle CatHurt = SoundID.CreateTrackable("player_hit_cat", SoundType.Sound, 0).WithVolume(0.5f).WithPitchVariance(0.4f);

		// Token: 0x04001C5B RID: 7259
		public static readonly LegacySoundStyle DogHurt = SoundID.CreateTrackable("player_hit_dog", SoundType.Sound, 0).WithVolume(0.5f).WithPitchVariance(0.4f);

		// Token: 0x04001C5C RID: 7260
		public static readonly LegacySoundStyle TurkeyHurt = SoundID.CreateTrackable("player_hit_turkey", SoundType.Sound, 0).WithVolume(0.5f).WithPitchVariance(0.4f);

		// Token: 0x04001C5D RID: 7261
		public static readonly LegacySoundStyle GoblinHurt = SoundID.CreateTrackable("player_hit_goblin", SoundType.Sound, 0).WithVolume(0.5f).WithPitchVariance(0.4f);

		// Token: 0x04001C5E RID: 7262
		public static readonly LegacySoundStyle CrowHurt = SoundID.CreateTrackable("player_hit_crow", SoundType.Sound, 0).WithVolume(0.5f).WithPitchVariance(0.4f);

		// Token: 0x04001C5F RID: 7263
		public static readonly LegacySoundStyle BalloonHurt = SoundID.CreateTrackable("player_hit_balloon", SoundType.Sound, 0).WithVolume(0.5f).WithPitchVariance(0.4f);

		// Token: 0x04001C60 RID: 7264
		public static readonly LegacySoundStyle BalloonDeath = SoundID.CreateTrackable("player_hit_balloon", SoundType.Sound, 0).WithVolume(0.8f).WithPitchVariance(0.4f);

		// Token: 0x04001C61 RID: 7265
		public static readonly LegacySoundStyle UndeadHurt = SoundID.CreateTrackable("player_hit_undead", SoundType.Sound, 0).WithVolume(0.5f).WithPitchVariance(0.4f);

		// Token: 0x04001C62 RID: 7266
		public static readonly LegacySoundStyle VampireHurt = SoundID.CreateTrackable("player_hit_vampire", SoundType.Sound, 0).WithVolume(0.5f).WithPitchVariance(0.4f);

		// Token: 0x04001C63 RID: 7267
		public static readonly LegacySoundStyle FairyHurt = SoundID.CreateTrackable("player_hit_fairy", SoundType.Sound, 0).WithVolume(0.5f).WithPitchVariance(0.4f);

		// Token: 0x04001C64 RID: 7268
		public static readonly LegacySoundStyle FishSplash = SoundID.CreateTrackable("fish_splash", SoundType.Sound, 0).WithVolume(1f).WithPitchVariance(0.02f);

		// Token: 0x04001C65 RID: 7269
		public static readonly LegacySoundStyle EOWDiggin = SoundID.CreateTrackable("eow_dig", SoundType.Sound, 0).WithVolume(1f).WithPitchVariance(0.02f);

		// Token: 0x04001C66 RID: 7270
		public static readonly LegacySoundStyle BombFuse = SoundID.CreateTrackable("fuse", SoundType.Sound, 0).WithVolume(0.2f);

		// Token: 0x04001C67 RID: 7271
		private static List<string> _trackableLegacySoundPathList;

		// Token: 0x04001C68 RID: 7272
		public static Dictionary<string, LegacySoundStyle> SoundByName = null;

		// Token: 0x04001C69 RID: 7273
		public static Dictionary<string, ushort> IndexByName = null;

		// Token: 0x04001C6A RID: 7274
		public static Dictionary<ushort, LegacySoundStyle> SoundByIndex = null;

		// Token: 0x0200077B RID: 1915
		private struct SoundStyleDefaults
		{
			// Token: 0x06004136 RID: 16694 RVA: 0x0069FE44 File Offset: 0x0069E044
			public SoundStyleDefaults(float volume, float pitchVariance, SoundType type = SoundType.Sound)
			{
				this.PitchVariance = pitchVariance;
				this.Volume = volume;
				this.Type = type;
			}

			// Token: 0x04006DE0 RID: 28128
			public readonly float PitchVariance;

			// Token: 0x04006DE1 RID: 28129
			public readonly float Volume;

			// Token: 0x04006DE2 RID: 28130
			public readonly SoundType Type;
		}
	}
}
