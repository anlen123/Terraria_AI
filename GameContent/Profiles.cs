using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Localization;

namespace Terraria.GameContent
{
	// Token: 0x0200026D RID: 621
	public class Profiles
	{
		// Token: 0x020007EC RID: 2028
		public class StackedNPCProfile : ITownNPCProfile
		{
			// Token: 0x0600426F RID: 17007 RVA: 0x006BD64C File Offset: 0x006BB84C
			public StackedNPCProfile(params ITownNPCProfile[] profilesInOrderOfVariants)
			{
				this._profiles = profilesInOrderOfVariants;
			}

			// Token: 0x06004270 RID: 17008 RVA: 0x001DA9FB File Offset: 0x001D8BFB
			public int RollVariation()
			{
				return 0;
			}

			// Token: 0x06004271 RID: 17009 RVA: 0x006BD65C File Offset: 0x006BB85C
			public string GetNameForVariant(NPC npc)
			{
				int num = 0;
				if (this._profiles.IndexInRange(npc.townNpcVariationIndex))
				{
					num = npc.townNpcVariationIndex;
				}
				return this._profiles[num].GetNameForVariant(npc);
			}

			// Token: 0x06004272 RID: 17010 RVA: 0x006BD694 File Offset: 0x006BB894
			public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
			{
				int num = 0;
				if (this._profiles.IndexInRange(npc.townNpcVariationIndex))
				{
					num = npc.townNpcVariationIndex;
				}
				return this._profiles[num].GetTextureNPCShouldUse(npc);
			}

			// Token: 0x06004273 RID: 17011 RVA: 0x006BD6CC File Offset: 0x006BB8CC
			public int GetHeadTextureIndex(NPC npc)
			{
				int num = 0;
				if (this._profiles.IndexInRange(npc.townNpcVariationIndex))
				{
					num = npc.townNpcVariationIndex;
				}
				return this._profiles[num].GetHeadTextureIndex(npc);
			}

			// Token: 0x0400711D RID: 28957
			internal ITownNPCProfile[] _profiles;
		}

		// Token: 0x020007ED RID: 2029
		public class LegacyNPCProfile : ITownNPCProfile
		{
			// Token: 0x06004274 RID: 17012 RVA: 0x006BD704 File Offset: 0x006BB904
			public LegacyNPCProfile(string npcFileTitleFilePath, int defaultHeadIndex, bool includeDefault = true, bool uniquePartyTexture = true)
			{
				this._rootFilePath = npcFileTitleFilePath;
				this._defaultVariationHeadIndex = defaultHeadIndex;
				if (Main.dedServ)
				{
					return;
				}
				this._defaultNoAlt = Main.Assets.Request<Texture2D>(npcFileTitleFilePath + (includeDefault ? "_Default" : ""), 0);
				if (uniquePartyTexture)
				{
					this._defaultParty = Main.Assets.Request<Texture2D>(npcFileTitleFilePath + (includeDefault ? "_Default_Party" : "_Party"), 0);
					return;
				}
				this._defaultParty = this._defaultNoAlt;
			}

			// Token: 0x06004275 RID: 17013 RVA: 0x001DA9FB File Offset: 0x001D8BFB
			public int RollVariation()
			{
				return 0;
			}

			// Token: 0x06004276 RID: 17014 RVA: 0x006BD78A File Offset: 0x006BB98A
			public string GetNameForVariant(NPC npc)
			{
				return NPC.getNewNPCName(npc.type);
			}

			// Token: 0x06004277 RID: 17015 RVA: 0x006BD797 File Offset: 0x006BB997
			public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
			{
				if (npc.IsABestiaryIconDummy && !npc.ForcePartyHatOn)
				{
					return this._defaultNoAlt;
				}
				if (npc.altTexture == 1)
				{
					return this._defaultParty;
				}
				return this._defaultNoAlt;
			}

			// Token: 0x06004278 RID: 17016 RVA: 0x006BD7C6 File Offset: 0x006BB9C6
			public int GetHeadTextureIndex(NPC npc)
			{
				return this._defaultVariationHeadIndex;
			}

			// Token: 0x0400711E RID: 28958
			private string _rootFilePath;

			// Token: 0x0400711F RID: 28959
			private int _defaultVariationHeadIndex;

			// Token: 0x04007120 RID: 28960
			internal Asset<Texture2D> _defaultNoAlt;

			// Token: 0x04007121 RID: 28961
			internal Asset<Texture2D> _defaultParty;
		}

		// Token: 0x020007EE RID: 2030
		public class TransformableNPCProfile : ITownNPCProfile
		{
			// Token: 0x06004279 RID: 17017 RVA: 0x006BD7D0 File Offset: 0x006BB9D0
			public TransformableNPCProfile(string npcFileTitleFilePath, int defaultHeadIndex, bool includeCredits = true)
			{
				this._rootFilePath = npcFileTitleFilePath;
				this._defaultVariationHeadIndex = defaultHeadIndex;
				if (Main.dedServ)
				{
					return;
				}
				this._defaultNoAlt = Main.Assets.Request<Texture2D>(npcFileTitleFilePath + "_Default", 0);
				this._defaultTransformed = Main.Assets.Request<Texture2D>(npcFileTitleFilePath + "_Default_Transformed", 0);
				if (includeCredits)
				{
					this._defaultCredits = Main.Assets.Request<Texture2D>(npcFileTitleFilePath + "_Default_Credits", 0);
				}
			}

			// Token: 0x0600427A RID: 17018 RVA: 0x001DA9FB File Offset: 0x001D8BFB
			public int RollVariation()
			{
				return 0;
			}

			// Token: 0x0600427B RID: 17019 RVA: 0x006BD78A File Offset: 0x006BB98A
			public string GetNameForVariant(NPC npc)
			{
				return NPC.getNewNPCName(npc.type);
			}

			// Token: 0x0600427C RID: 17020 RVA: 0x006BD850 File Offset: 0x006BBA50
			public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
			{
				if (npc.altTexture == 3 && this._defaultCredits != null)
				{
					return this._defaultCredits;
				}
				if (npc.IsABestiaryIconDummy)
				{
					return this._defaultNoAlt;
				}
				if (npc.altTexture == 2)
				{
					return this._defaultTransformed;
				}
				return this._defaultNoAlt;
			}

			// Token: 0x0600427D RID: 17021 RVA: 0x006BD88F File Offset: 0x006BBA8F
			public int GetHeadTextureIndex(NPC npc)
			{
				return this._defaultVariationHeadIndex;
			}

			// Token: 0x04007122 RID: 28962
			private string _rootFilePath;

			// Token: 0x04007123 RID: 28963
			private int _defaultVariationHeadIndex;

			// Token: 0x04007124 RID: 28964
			internal Asset<Texture2D> _defaultNoAlt;

			// Token: 0x04007125 RID: 28965
			internal Asset<Texture2D> _defaultTransformed;

			// Token: 0x04007126 RID: 28966
			internal Asset<Texture2D> _defaultCredits;
		}

		// Token: 0x020007EF RID: 2031
		public class VariantNPCProfile : ITownNPCProfile
		{
			// Token: 0x0600427E RID: 17022 RVA: 0x006BD898 File Offset: 0x006BBA98
			public VariantNPCProfile(string npcFileTitleFilePath, string npcBaseName, int[] variantHeadIds, params string[] variantTextureNames)
			{
				this._rootFilePath = npcFileTitleFilePath;
				this._npcBaseName = npcBaseName;
				this._variantHeadIDs = variantHeadIds;
				this._variants = variantTextureNames;
				foreach (string str in this._variants)
				{
					string text = this._rootFilePath + "_" + str;
					if (!Main.dedServ)
					{
						this._variantTextures[text] = Main.Assets.Request<Texture2D>(text, 0);
					}
				}
			}

			// Token: 0x0600427F RID: 17023 RVA: 0x006BD920 File Offset: 0x006BBB20
			public Profiles.VariantNPCProfile SetPartyTextures(params string[] variantTextureNames)
			{
				foreach (string str in variantTextureNames)
				{
					string text = this._rootFilePath + "_" + str + "_Party";
					if (!Main.dedServ)
					{
						this._variantTextures[text] = Main.Assets.Request<Texture2D>(text, 0);
					}
				}
				return this;
			}

			// Token: 0x06004280 RID: 17024 RVA: 0x006BD978 File Offset: 0x006BBB78
			public int RollVariation()
			{
				return Main.rand.Next(this._variants.Length);
			}

			// Token: 0x06004281 RID: 17025 RVA: 0x006BD98C File Offset: 0x006BBB8C
			public string GetNameForVariant(NPC npc)
			{
				return Language.RandomFromCategory(this._npcBaseName + "Names_" + this._variants[npc.townNpcVariationIndex], WorldGen.genRand).Value;
			}

			// Token: 0x06004282 RID: 17026 RVA: 0x006BD9BC File Offset: 0x006BBBBC
			public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
			{
				string text = this._rootFilePath + "_" + this._variants[npc.townNpcVariationIndex];
				if (npc.IsABestiaryIconDummy)
				{
					return this._variantTextures[text];
				}
				if (npc.altTexture == 1 && this._variantTextures.ContainsKey(text + "_Party"))
				{
					return this._variantTextures[text + "_Party"];
				}
				return this._variantTextures[text];
			}

			// Token: 0x06004283 RID: 17027 RVA: 0x006BDA40 File Offset: 0x006BBC40
			public int GetHeadTextureIndex(NPC npc)
			{
				return this._variantHeadIDs[npc.townNpcVariationIndex];
			}

			// Token: 0x04007127 RID: 28967
			private string _rootFilePath;

			// Token: 0x04007128 RID: 28968
			private string _npcBaseName;

			// Token: 0x04007129 RID: 28969
			private int[] _variantHeadIDs;

			// Token: 0x0400712A RID: 28970
			private string[] _variants;

			// Token: 0x0400712B RID: 28971
			internal Dictionary<string, Asset<Texture2D>> _variantTextures = new Dictionary<string, Asset<Texture2D>>();
		}
	}
}
