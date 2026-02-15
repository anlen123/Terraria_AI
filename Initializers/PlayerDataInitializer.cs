using System;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.ID;

namespace Terraria.Initializers
{
	// Token: 0x02000086 RID: 134
	public static class PlayerDataInitializer
	{
		// Token: 0x0600158A RID: 5514 RVA: 0x004CB63C File Offset: 0x004C983C
		public static void Load()
		{
			TextureAssets.Players = new Asset<Texture2D>[PlayerVariantID.Count, PlayerTextureID.Count];
			PlayerDataInitializer.LoadStarterMale();
			PlayerDataInitializer.LoadStarterFemale();
			PlayerDataInitializer.LoadStickerMale();
			PlayerDataInitializer.LoadStickerFemale();
			PlayerDataInitializer.LoadGangsterMale();
			PlayerDataInitializer.LoadGangsterFemale();
			PlayerDataInitializer.LoadCoatMale();
			PlayerDataInitializer.LoadDressFemale();
			PlayerDataInitializer.LoadDressMale();
			PlayerDataInitializer.LoadCoatFemale();
			PlayerDataInitializer.LoadDisplayDollMale();
			PlayerDataInitializer.LoadDisplayDollFemale();
		}

		// Token: 0x0600158B RID: 5515 RVA: 0x004CB69C File Offset: 0x004C989C
		private static void LoadVariant(int ID, int[] pieceIDs)
		{
			for (int i = 0; i < pieceIDs.Length; i++)
			{
				TextureAssets.Players[ID, pieceIDs[i]] = Main.Assets.Request<Texture2D>(string.Concat(new object[]
				{
					"Images/Player_",
					ID,
					"_",
					pieceIDs[i]
				}), 2);
			}
		}

		// Token: 0x0600158C RID: 5516 RVA: 0x004CB700 File Offset: 0x004C9900
		private static void CopyVariant(int to, int from)
		{
			for (int i = 0; i < PlayerTextureID.Count; i++)
			{
				TextureAssets.Players[to, i] = TextureAssets.Players[from, i];
			}
		}

		// Token: 0x0600158D RID: 5517 RVA: 0x004CB735 File Offset: 0x004C9935
		private static void LoadStarterMale()
		{
			PlayerDataInitializer.LoadVariant(0, new int[]
			{
				0,
				1,
				2,
				3,
				4,
				5,
				6,
				7,
				8,
				9,
				10,
				11,
				12,
				13,
				15
			});
			TextureAssets.Players[0, 14] = Asset<Texture2D>.Empty;
		}

		// Token: 0x0600158E RID: 5518 RVA: 0x004CB761 File Offset: 0x004C9961
		private static void LoadStickerMale()
		{
			PlayerDataInitializer.CopyVariant(1, 0);
			PlayerDataInitializer.LoadVariant(1, new int[]
			{
				4,
				6,
				8,
				11,
				12,
				13
			});
		}

		// Token: 0x0600158F RID: 5519 RVA: 0x004CB781 File Offset: 0x004C9981
		private static void LoadGangsterMale()
		{
			PlayerDataInitializer.CopyVariant(2, 0);
			PlayerDataInitializer.LoadVariant(2, new int[]
			{
				4,
				6,
				8,
				11,
				12,
				13
			});
		}

		// Token: 0x06001590 RID: 5520 RVA: 0x004CB7A1 File Offset: 0x004C99A1
		private static void LoadCoatMale()
		{
			PlayerDataInitializer.CopyVariant(3, 0);
			PlayerDataInitializer.LoadVariant(3, new int[]
			{
				4,
				6,
				8,
				11,
				12,
				13,
				14
			});
		}

		// Token: 0x06001591 RID: 5521 RVA: 0x004CB7C1 File Offset: 0x004C99C1
		private static void LoadDressMale()
		{
			PlayerDataInitializer.CopyVariant(8, 0);
			PlayerDataInitializer.LoadVariant(8, new int[]
			{
				4,
				6,
				8,
				11,
				12,
				13,
				14
			});
		}

		// Token: 0x06001592 RID: 5522 RVA: 0x004CB7E1 File Offset: 0x004C99E1
		private static void LoadStarterFemale()
		{
			PlayerDataInitializer.CopyVariant(4, 0);
			PlayerDataInitializer.LoadVariant(4, new int[]
			{
				3,
				4,
				5,
				6,
				7,
				8,
				9,
				10,
				11,
				12,
				13
			});
		}

		// Token: 0x06001593 RID: 5523 RVA: 0x004CB802 File Offset: 0x004C9A02
		private static void LoadStickerFemale()
		{
			PlayerDataInitializer.CopyVariant(5, 4);
			PlayerDataInitializer.LoadVariant(5, new int[]
			{
				4,
				6,
				8,
				11,
				12,
				13
			});
		}

		// Token: 0x06001594 RID: 5524 RVA: 0x004CB822 File Offset: 0x004C9A22
		private static void LoadGangsterFemale()
		{
			PlayerDataInitializer.CopyVariant(6, 4);
			PlayerDataInitializer.LoadVariant(6, new int[]
			{
				4,
				6,
				8,
				11,
				12,
				13
			});
		}

		// Token: 0x06001595 RID: 5525 RVA: 0x004CB842 File Offset: 0x004C9A42
		private static void LoadCoatFemale()
		{
			PlayerDataInitializer.CopyVariant(7, 4);
			PlayerDataInitializer.LoadVariant(7, new int[]
			{
				4,
				6,
				8,
				11,
				12,
				13,
				14
			});
		}

		// Token: 0x06001596 RID: 5526 RVA: 0x004CB862 File Offset: 0x004C9A62
		private static void LoadDressFemale()
		{
			PlayerDataInitializer.CopyVariant(9, 4);
			PlayerDataInitializer.LoadVariant(9, new int[]
			{
				4,
				6,
				8,
				11,
				12,
				13
			});
		}

		// Token: 0x06001597 RID: 5527 RVA: 0x004CB884 File Offset: 0x004C9A84
		private static void LoadDisplayDollMale()
		{
			PlayerDataInitializer.CopyVariant(10, 0);
			PlayerDataInitializer.LoadVariant(10, new int[]
			{
				0,
				2,
				3,
				5,
				7,
				9,
				10
			});
			Asset<Texture2D> asset = TextureAssets.Players[10, 2];
			TextureAssets.Players[10, 2] = asset;
			TextureAssets.Players[10, 1] = asset;
			TextureAssets.Players[10, 4] = asset;
			TextureAssets.Players[10, 6] = asset;
			TextureAssets.Players[10, 11] = asset;
			TextureAssets.Players[10, 12] = asset;
			TextureAssets.Players[10, 13] = asset;
			TextureAssets.Players[10, 8] = asset;
			TextureAssets.Players[10, 15] = asset;
		}

		// Token: 0x06001598 RID: 5528 RVA: 0x004CB944 File Offset: 0x004C9B44
		private static void LoadDisplayDollFemale()
		{
			PlayerDataInitializer.CopyVariant(11, 10);
			PlayerDataInitializer.LoadVariant(11, new int[]
			{
				3,
				5,
				7,
				9,
				10
			});
			Asset<Texture2D> asset = TextureAssets.Players[10, 2];
			TextureAssets.Players[11, 2] = asset;
			TextureAssets.Players[11, 1] = asset;
			TextureAssets.Players[11, 4] = asset;
			TextureAssets.Players[11, 6] = asset;
			TextureAssets.Players[11, 11] = asset;
			TextureAssets.Players[11, 12] = asset;
			TextureAssets.Players[11, 13] = asset;
			TextureAssets.Players[11, 8] = asset;
			TextureAssets.Players[11, 15] = asset;
		}
	}
}
