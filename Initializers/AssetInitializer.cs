using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using ReLogic.Content;
using ReLogic.Content.Readers;
using ReLogic.Graphics;
using ReLogic.Utilities;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.IO;
using Terraria.Testing;
using Terraria.Utilities;

namespace Terraria.Initializers
{
	// Token: 0x0200007F RID: 127
	public static class AssetInitializer
	{
		// Token: 0x06001560 RID: 5472 RVA: 0x004C3E90 File Offset: 0x004C2090
		public static void CreateAssetServices(GameServiceContainer services)
		{
			AssetReaderCollection assetReaderCollection = new AssetReaderCollection();
			assetReaderCollection.RegisterReader(new PngReader(XnaExtensions.Get<IGraphicsDeviceService>(services).GraphicsDevice), new string[]
			{
				".png"
			});
			assetReaderCollection.RegisterReader(new XnbReader(services), new string[]
			{
				".xnb"
			});
			assetReaderCollection.RegisterReader(new FxReader(XnaExtensions.Get<IGraphicsDeviceService>(services).GraphicsDevice), new string[]
			{
				".fx"
			});
			AsyncAssetLoader asyncAssetLoader = new AsyncAssetLoader(assetReaderCollection, 20);
			asyncAssetLoader.RequireTypeCreationOnTransfer(typeof(Texture2D));
			asyncAssetLoader.RequireTypeCreationOnTransfer(typeof(DynamicSpriteFont));
			asyncAssetLoader.RequireTypeCreationOnTransfer(typeof(SpriteFont));
			IAssetRepository assetRepository = new AssetRepository(new AssetLoader(assetReaderCollection), asyncAssetLoader);
			IAssetRepository assetRepository2 = assetRepository;
			assetRepository2.AssetValueUpdatedHandler = (AssetValueUpdated)Delegate.Combine(assetRepository2.AssetValueUpdatedHandler, new AssetValueUpdated(AssetInitializer.TagAsset));
			services.AddService(typeof(AssetReaderCollection), assetReaderCollection);
			services.AddService(typeof(IAssetRepository), assetRepository);
		}

		// Token: 0x06001561 RID: 5473 RVA: 0x004C3F90 File Offset: 0x004C2190
		private static void TagAsset(IAsset asset, object value)
		{
			GraphicsResource graphicsResource = value as GraphicsResource;
			if (graphicsResource != null)
			{
				graphicsResource.Name = asset.Name;
				graphicsResource.Tag = asset;
			}
		}

		// Token: 0x06001562 RID: 5474 RVA: 0x004C3FBC File Offset: 0x004C21BC
		public static ResourcePackList CreateResourcePackList(IServiceProvider services)
		{
			JArray serializedState;
			string searchPath;
			AssetInitializer.GetResourcePacksFolderPathAndConfirmItExists(out serializedState, out searchPath);
			return ResourcePackList.FromJson(serializedState, services, searchPath);
		}

		// Token: 0x06001563 RID: 5475 RVA: 0x004C3FDC File Offset: 0x004C21DC
		public static ResourcePackList CreatePublishableResourcePacksList(IServiceProvider services)
		{
			JArray serializedState;
			string searchPath;
			AssetInitializer.GetResourcePacksFolderPathAndConfirmItExists(out serializedState, out searchPath);
			return ResourcePackList.Publishable(serializedState, services, searchPath);
		}

		// Token: 0x06001564 RID: 5476 RVA: 0x004C3FFA File Offset: 0x004C21FA
		public static void GetResourcePacksFolderPathAndConfirmItExists(out JArray resourcePackJson, out string resourcePackFolder)
		{
			resourcePackJson = Main.Configuration.Get<JArray>("ResourcePacks", new JArray());
			resourcePackFolder = Path.Combine(Main.SavePath, "ResourcePacks");
			Utils.TryCreatingDirectory(resourcePackFolder);
		}

		// Token: 0x06001565 RID: 5477 RVA: 0x004C402C File Offset: 0x004C222C
		public static void LoadSplashAssets()
		{
			TextureAssets.SplashTexture16x9 = AssetInitializer.LoadAsset<Texture2D>("Images\\SplashScreens\\Splash_1", 1);
			TextureAssets.SplashTexture4x3 = AssetInitializer.LoadAsset<Texture2D>("Images\\logo_" + new UnifiedRandom().Next(1, 9), 1);
			TextureAssets.SplashTextureLegoResonanace = AssetInitializer.LoadAsset<Texture2D>("Images\\SplashScreens\\ResonanceArray", 1);
			int num = new UnifiedRandom().Next(1, 11);
			TextureAssets.SplashTextureLegoBack = AssetInitializer.LoadAsset<Texture2D>("Images\\SplashScreens\\Splash_" + num + "_0", 1);
			TextureAssets.SplashTextureLegoTree = AssetInitializer.LoadAsset<Texture2D>("Images\\SplashScreens\\Splash_" + num + "_1", 1);
			TextureAssets.SplashTextureLegoFront = AssetInitializer.LoadAsset<Texture2D>("Images\\SplashScreens\\Splash_" + num + "_2", 1);
			TextureAssets.Item[75] = AssetInitializer.LoadAsset<Texture2D>("Images\\Item_" + 75, 1);
			TextureAssets.LoadingSunflower = AssetInitializer.LoadAsset<Texture2D>("Images\\UI\\Sunflower_Loading", 1);
		}

		// Token: 0x06001566 RID: 5478 RVA: 0x004C411D File Offset: 0x004C231D
		public static void Load(bool asyncLoad)
		{
			int mode = asyncLoad ? 2 : 1;
			AssetInitializer.LoadTextures(mode);
			AssetInitializer.LoadRenderTargetAssets(mode);
			AssetInitializer.LoadSounds(mode);
			AssetInitializer.LoadFonts(mode);
		}

		// Token: 0x06001567 RID: 5479 RVA: 0x004C4140 File Offset: 0x004C2340
		private static void LoadFonts(AssetRequestMode mode)
		{
			FontAssets.ItemStack = AssetInitializer.LoadAsset<DynamicSpriteFont>("Fonts/Item_Stack", mode);
			FontAssets.MouseText = AssetInitializer.LoadAsset<DynamicSpriteFont>("Fonts/Mouse_Text", mode);
			FontAssets.DeathText = AssetInitializer.LoadAsset<DynamicSpriteFont>("Fonts/Death_Text", mode);
			FontAssets.CombatText[0] = AssetInitializer.LoadAsset<DynamicSpriteFont>("Fonts/Combat_Text", mode);
			FontAssets.CombatText[1] = AssetInitializer.LoadAsset<DynamicSpriteFont>("Fonts/Combat_Crit", mode);
		}

		// Token: 0x06001568 RID: 5480 RVA: 0x004C41A1 File Offset: 0x004C23A1
		private static void LoadSounds(AssetRequestMode mode)
		{
			SoundEngine.Load(Main.instance.Services);
		}

		// Token: 0x06001569 RID: 5481 RVA: 0x004C41B2 File Offset: 0x004C23B2
		private static void LoadRenderTargetAssets(AssetRequestMode mode)
		{
			AssetInitializer.RegisterRenderTargetAsset(TextureAssets.RenderTargets.PlayerRainbowWings = new PlayerRainbowWingsTextureContent());
			AssetInitializer.RegisterRenderTargetAsset(TextureAssets.RenderTargets.PlayerTitaniumStormBuff = new PlayerTitaniumStormBuffTextureContent());
			AssetInitializer.RegisterRenderTargetAsset(TextureAssets.RenderTargets.QueenSlimeMount = new PlayerQueenSlimeMountTextureContent());
		}

		// Token: 0x0600156A RID: 5482 RVA: 0x004C41E4 File Offset: 0x004C23E4
		private static void RegisterRenderTargetAsset(INeedRenderTargetContent content)
		{
			Main.ContentThatNeedsRenderTargets.Add(content);
		}

		// Token: 0x0600156B RID: 5483 RVA: 0x004C41F4 File Offset: 0x004C23F4
		private static void LoadTextures(AssetRequestMode mode)
		{
			for (int i = 0; i < TextureAssets.Item.Length; i++)
			{
				int num = ItemID.Sets.TextureCopyLoad[i];
				if (num != -1)
				{
					TextureAssets.Item[i] = TextureAssets.Item[num];
				}
				else
				{
					TextureAssets.Item[i] = AssetInitializer.LoadAsset<Texture2D>("Images/Item_" + i, 0);
				}
			}
			for (int j = 0; j < TextureAssets.Npc.Length; j++)
			{
				TextureAssets.Npc[j] = AssetInitializer.LoadAsset<Texture2D>("Images/NPC_" + j, 0);
			}
			for (int k = 0; k < TextureAssets.Projectile.Length; k++)
			{
				TextureAssets.Projectile[k] = AssetInitializer.LoadAsset<Texture2D>("Images/Projectile_" + k, 0);
			}
			for (int l = 0; l < TextureAssets.Gore.Length; l++)
			{
				TextureAssets.Gore[l] = AssetInitializer.LoadAsset<Texture2D>("Images/Gore_" + l, 0);
			}
			for (int m = 0; m < TextureAssets.Wall.Length; m++)
			{
				TextureAssets.Wall[m] = AssetInitializer.LoadAsset<Texture2D>("Images/Wall_" + m, 0);
			}
			for (int n = 0; n < TextureAssets.Tile.Length; n++)
			{
				TextureAssets.Tile[n] = AssetInitializer.LoadAsset<Texture2D>("Images/Tiles_" + n, 0);
			}
			for (int num2 = 0; num2 < TextureAssets.ItemFlame.Length; num2++)
			{
				TextureAssets.ItemFlame[num2] = AssetInitializer.LoadAsset<Texture2D>("Images/ItemFlame_" + num2, 0);
			}
			for (int num3 = 0; num3 < TextureAssets.Wings.Length; num3++)
			{
				TextureAssets.Wings[num3] = AssetInitializer.LoadAsset<Texture2D>("Images/Wings_" + num3, 0);
			}
			for (int num4 = 0; num4 < TextureAssets.PlayerHair.Length; num4++)
			{
				TextureAssets.PlayerHair[num4] = AssetInitializer.LoadAsset<Texture2D>("Images/Player_Hair_" + (num4 + 1), 0);
			}
			for (int num5 = 0; num5 < TextureAssets.PlayerHairAlt.Length; num5++)
			{
				TextureAssets.PlayerHairAlt[num5] = AssetInitializer.LoadAsset<Texture2D>("Images/Player_HairAlt_" + (num5 + 1), 0);
			}
			for (int num6 = 0; num6 < TextureAssets.ArmorHead.Length; num6++)
			{
				TextureAssets.ArmorHead[num6] = AssetInitializer.LoadAsset<Texture2D>("Images/Armor_Head_" + num6, 0);
			}
			for (int num7 = 0; num7 < TextureAssets.FemaleBody.Length; num7++)
			{
				TextureAssets.FemaleBody[num7] = AssetInitializer.LoadAsset<Texture2D>("Images/Female_Body_" + num7, 0);
			}
			for (int num8 = 0; num8 < TextureAssets.ArmorBody.Length; num8++)
			{
				TextureAssets.ArmorBody[num8] = AssetInitializer.LoadAsset<Texture2D>("Images/Armor_Body_" + num8, 0);
			}
			for (int num9 = 0; num9 < TextureAssets.ArmorBodyComposite.Length; num9++)
			{
				TextureAssets.ArmorBodyComposite[num9] = AssetInitializer.LoadAsset<Texture2D>("Images/Armor/Armor_" + num9, 0);
			}
			for (int num10 = 0; num10 < TextureAssets.ArmorArm.Length; num10++)
			{
				TextureAssets.ArmorArm[num10] = AssetInitializer.LoadAsset<Texture2D>("Images/Armor_Arm_" + num10, 0);
			}
			for (int num11 = 0; num11 < TextureAssets.ArmorLeg.Length; num11++)
			{
				TextureAssets.ArmorLeg[num11] = AssetInitializer.LoadAsset<Texture2D>("Images/Armor_Legs_" + num11, 0);
			}
			for (int num12 = 0; num12 < TextureAssets.AccHandsOn.Length; num12++)
			{
				TextureAssets.AccHandsOn[num12] = AssetInitializer.LoadAsset<Texture2D>("Images/Acc_HandsOn_" + num12, 0);
			}
			for (int num13 = 0; num13 < TextureAssets.AccHandsOff.Length; num13++)
			{
				TextureAssets.AccHandsOff[num13] = AssetInitializer.LoadAsset<Texture2D>("Images/Acc_HandsOff_" + num13, 0);
			}
			for (int num14 = 0; num14 < TextureAssets.AccHandsOnComposite.Length; num14++)
			{
				TextureAssets.AccHandsOnComposite[num14] = AssetInitializer.LoadAsset<Texture2D>("Images/Accessories/Acc_HandsOn_" + num14, 0);
			}
			for (int num15 = 0; num15 < TextureAssets.AccHandsOffComposite.Length; num15++)
			{
				TextureAssets.AccHandsOffComposite[num15] = AssetInitializer.LoadAsset<Texture2D>("Images/Accessories/Acc_HandsOff_" + num15, 0);
			}
			for (int num16 = 0; num16 < TextureAssets.AccBack.Length; num16++)
			{
				TextureAssets.AccBack[num16] = AssetInitializer.LoadAsset<Texture2D>("Images/Acc_Back_" + num16, 0);
			}
			for (int num17 = 0; num17 < TextureAssets.AccFront.Length; num17++)
			{
				TextureAssets.AccFront[num17] = AssetInitializer.LoadAsset<Texture2D>("Images/Acc_Front_" + num17, 0);
			}
			for (int num18 = 0; num18 < TextureAssets.AccShoes.Length; num18++)
			{
				TextureAssets.AccShoes[num18] = AssetInitializer.LoadAsset<Texture2D>("Images/Acc_Shoes_" + num18, 0);
			}
			for (int num19 = 0; num19 < TextureAssets.AccWaist.Length; num19++)
			{
				TextureAssets.AccWaist[num19] = AssetInitializer.LoadAsset<Texture2D>("Images/Acc_Waist_" + num19, 0);
			}
			for (int num20 = 0; num20 < TextureAssets.AccShield.Length; num20++)
			{
				TextureAssets.AccShield[num20] = AssetInitializer.LoadAsset<Texture2D>("Images/Acc_Shield_" + num20, 0);
			}
			for (int num21 = 0; num21 < TextureAssets.AccNeck.Length; num21++)
			{
				TextureAssets.AccNeck[num21] = AssetInitializer.LoadAsset<Texture2D>("Images/Acc_Neck_" + num21, 0);
			}
			for (int num22 = 0; num22 < TextureAssets.AccFace.Length; num22++)
			{
				TextureAssets.AccFace[num22] = AssetInitializer.LoadAsset<Texture2D>("Images/Acc_Face_" + num22, 0);
			}
			for (int num23 = 0; num23 < TextureAssets.AccBalloon.Length; num23++)
			{
				TextureAssets.AccBalloon[num23] = AssetInitializer.LoadAsset<Texture2D>("Images/Acc_Balloon_" + num23, 0);
			}
			for (int num24 = 0; num24 < TextureAssets.AccBeard.Length; num24++)
			{
				TextureAssets.AccBeard[num24] = AssetInitializer.LoadAsset<Texture2D>("Images/Acc_Beard_" + num24, 0);
			}
			for (int num25 = 0; num25 < TextureAssets.Background.Length; num25++)
			{
				TextureAssets.Background[num25] = AssetInitializer.LoadAsset<Texture2D>("Images/Background_" + num25, 0);
			}
			TextureAssets.FlameRing = AssetInitializer.LoadAsset<Texture2D>("Images/FlameRing", 0);
			TextureAssets.TileCrack = AssetInitializer.LoadAsset<Texture2D>("Images\\TileCracks", mode);
			for (int num26 = 0; num26 < TextureAssets.ChestStack.Length; num26++)
			{
				TextureAssets.ChestStack[num26] = AssetInitializer.LoadAsset<Texture2D>("Images\\UI\\ChestStack_" + num26, mode);
			}
			for (int num27 = 0; num27 < TextureAssets.ChestCraft.Length; num27++)
			{
				TextureAssets.ChestCraft[num27] = AssetInitializer.LoadAsset<Texture2D>("Images\\UI\\ChestCraft_" + num27, mode);
			}
			TextureAssets.SmartDig = AssetInitializer.LoadAsset<Texture2D>("Images\\SmartDig", mode);
			TextureAssets.SmartCursorArrow = AssetInitializer.LoadAsset<Texture2D>("Images\\UI\\SmartCursorArrow", mode);
			TextureAssets.IceBarrier = AssetInitializer.LoadAsset<Texture2D>("Images\\IceBarrier", mode);
			TextureAssets.Frozen = AssetInitializer.LoadAsset<Texture2D>("Images\\Frozen", mode);
			for (int num28 = 0; num28 < TextureAssets.Pvp.Length; num28++)
			{
				TextureAssets.Pvp[num28] = AssetInitializer.LoadAsset<Texture2D>("Images\\UI\\PVP_" + num28, mode);
			}
			for (int num29 = 0; num29 < TextureAssets.EquipPage.Length; num29++)
			{
				TextureAssets.EquipPage[num29] = AssetInitializer.LoadAsset<Texture2D>("Images\\UI\\DisplaySlots_" + num29, mode);
			}
			TextureAssets.HouseBanner = AssetInitializer.LoadAsset<Texture2D>("Images\\UI\\House_Banner", mode);
			TextureAssets.NPCHappiness = AssetInitializer.LoadAsset<Texture2D>("Images\\UI\\NPCHappiness", mode);
			for (int num30 = 0; num30 < TextureAssets.CraftToggle.Length; num30++)
			{
				TextureAssets.CraftToggle[num30] = AssetInitializer.LoadAsset<Texture2D>("Images\\UI\\Craft_Toggle_" + num30, mode);
			}
			for (int num31 = 0; num31 < TextureAssets.BannerToggle.Length; num31++)
			{
				TextureAssets.BannerToggle[num31] = AssetInitializer.LoadAsset<Texture2D>("Images\\UI\\Banner_Toggle_" + num31, mode);
			}
			for (int num32 = 0; num32 < TextureAssets.InventorySort.Length; num32++)
			{
				TextureAssets.InventorySort[num32] = AssetInitializer.LoadAsset<Texture2D>("Images\\UI\\Sort_" + num32, mode);
			}
			for (int num33 = 0; num33 < TextureAssets.TextGlyph.Length; num33++)
			{
				TextureAssets.TextGlyph[num33] = AssetInitializer.LoadAsset<Texture2D>("Images\\UI\\Glyphs_" + num33, mode);
			}
			for (int num34 = 0; num34 < TextureAssets.HotbarRadial.Length; num34++)
			{
				TextureAssets.HotbarRadial[num34] = AssetInitializer.LoadAsset<Texture2D>("Images\\UI\\HotbarRadial_" + num34, mode);
			}
			for (int num35 = 0; num35 < TextureAssets.InfoIcon.Length; num35++)
			{
				TextureAssets.InfoIcon[num35] = AssetInitializer.LoadAsset<Texture2D>("Images\\UI\\InfoIcon_" + num35, mode);
			}
			for (int num36 = 0; num36 < TextureAssets.Reforge.Length; num36++)
			{
				TextureAssets.Reforge[num36] = AssetInitializer.LoadAsset<Texture2D>("Images\\UI\\Reforge_" + num36, mode);
			}
			for (int num37 = 0; num37 < TextureAssets.Camera.Length; num37++)
			{
				TextureAssets.Camera[num37] = AssetInitializer.LoadAsset<Texture2D>("Images\\UI\\Camera_" + num37, mode);
			}
			for (int num38 = 0; num38 < TextureAssets.WireUi.Length; num38++)
			{
				TextureAssets.WireUi[num38] = AssetInitializer.LoadAsset<Texture2D>("Images\\UI\\Wires_" + num38, mode);
			}
			TextureAssets.BuilderAcc = AssetInitializer.LoadAsset<Texture2D>("Images\\UI\\BuilderIcons", mode);
			TextureAssets.QuicksIcon = AssetInitializer.LoadAsset<Texture2D>("Images\\UI\\UI_quickicon1", mode);
			TextureAssets.TexturePackButtons = AssetInitializer.LoadAsset<Texture2D>("Images\\UI\\TexturePackButtons", mode);
			TextureAssets.CraftUpButton = AssetInitializer.LoadAsset<Texture2D>("Images\\RecUp", mode);
			TextureAssets.CraftDownButton = AssetInitializer.LoadAsset<Texture2D>("Images\\RecDown", mode);
			TextureAssets.ScrollLeftButton = AssetInitializer.LoadAsset<Texture2D>("Images\\RecLeft", mode);
			TextureAssets.ScrollRightButton = AssetInitializer.LoadAsset<Texture2D>("Images\\RecRight", mode);
			TextureAssets.OneDropLogo = AssetInitializer.LoadAsset<Texture2D>("Images\\OneDropLogo", mode);
			TextureAssets.Pulley = AssetInitializer.LoadAsset<Texture2D>("Images\\PlayerPulley", mode);
			TextureAssets.Timer = AssetInitializer.LoadAsset<Texture2D>("Images\\Timer", mode);
			TextureAssets.EmoteMenuButton = AssetInitializer.LoadAsset<Texture2D>("Images\\UI\\Emotes", mode);
			TextureAssets.BestiaryMenuButton = AssetInitializer.LoadAsset<Texture2D>("Images\\UI\\Bestiary", mode);
			TextureAssets.Wof = AssetInitializer.LoadAsset<Texture2D>("Images\\WallOfFlesh", mode);
			TextureAssets.WallOutline = AssetInitializer.LoadAsset<Texture2D>("Images\\Wall_Outline", mode);
			TextureAssets.Fade = AssetInitializer.LoadAsset<Texture2D>("Images\\fade-out", mode);
			TextureAssets.Ghost = AssetInitializer.LoadAsset<Texture2D>("Images\\Ghost", mode);
			TextureAssets.EvilCactus = AssetInitializer.LoadAsset<Texture2D>("Images\\Evil_Cactus", mode);
			TextureAssets.GoodCactus = AssetInitializer.LoadAsset<Texture2D>("Images\\Good_Cactus", mode);
			TextureAssets.CrimsonCactus = AssetInitializer.LoadAsset<Texture2D>("Images\\Crimson_Cactus", mode);
			TextureAssets.WraithEye = AssetInitializer.LoadAsset<Texture2D>("Images\\Wraith_Eyes", mode);
			TextureAssets.Firefly = AssetInitializer.LoadAsset<Texture2D>("Images\\Firefly", mode);
			TextureAssets.FireflyJar = AssetInitializer.LoadAsset<Texture2D>("Images\\FireflyJar", mode);
			TextureAssets.Lightningbug = AssetInitializer.LoadAsset<Texture2D>("Images\\LightningBug", mode);
			TextureAssets.LightningbugJar = AssetInitializer.LoadAsset<Texture2D>("Images\\LightningBugJar", mode);
			for (int num39 = 1; num39 <= 3; num39++)
			{
				TextureAssets.JellyfishBowl[num39 - 1] = AssetInitializer.LoadAsset<Texture2D>("Images\\jellyfishBowl" + num39, mode);
			}
			TextureAssets.GlowSnail = AssetInitializer.LoadAsset<Texture2D>("Images\\GlowSnail", mode);
			TextureAssets.IceQueen = AssetInitializer.LoadAsset<Texture2D>("Images\\IceQueen", mode);
			TextureAssets.SantaTank = AssetInitializer.LoadAsset<Texture2D>("Images\\SantaTank", mode);
			TextureAssets.JackHat = AssetInitializer.LoadAsset<Texture2D>("Images\\JackHat", mode);
			TextureAssets.TreeFace = AssetInitializer.LoadAsset<Texture2D>("Images\\TreeFace", mode);
			TextureAssets.PumpkingFace = AssetInitializer.LoadAsset<Texture2D>("Images\\PumpkingFace", mode);
			TextureAssets.ReaperEye = AssetInitializer.LoadAsset<Texture2D>("Images\\Reaper_Eyes", mode);
			TextureAssets.MapDeath = AssetInitializer.LoadAsset<Texture2D>("Images\\MapDeath", mode);
			TextureAssets.DukeFishron = AssetInitializer.LoadAsset<Texture2D>("Images\\DukeFishron", mode);
			TextureAssets.Map = AssetInitializer.LoadAsset<Texture2D>("Images\\Map", mode);
			for (int num40 = 0; num40 < TextureAssets.MapBGs.Length; num40++)
			{
				TextureAssets.MapBGs[num40] = AssetInitializer.LoadAsset<Texture2D>("Images\\MapBG" + (num40 + 1), mode);
			}
			TextureAssets.Hue = AssetInitializer.LoadAsset<Texture2D>("Images\\Hue", mode);
			TextureAssets.ColorSlider = AssetInitializer.LoadAsset<Texture2D>("Images\\ColorSlider", mode);
			TextureAssets.ColorBar = AssetInitializer.LoadAsset<Texture2D>("Images\\ColorBar", mode);
			TextureAssets.ColorBlip = AssetInitializer.LoadAsset<Texture2D>("Images\\ColorBlip", mode);
			TextureAssets.ColorHighlight = AssetInitializer.LoadAsset<Texture2D>("Images\\UI\\Slider_Highlight", mode);
			TextureAssets.LockOnCursor = AssetInitializer.LoadAsset<Texture2D>("Images\\UI\\LockOn_Cursor", mode);
			TextureAssets.Rain = AssetInitializer.LoadAsset<Texture2D>("Images\\Rain", mode);
			for (int num41 = 0; num41 < (int)GlowMaskID.Count; num41++)
			{
				TextureAssets.GlowMask[num41] = AssetInitializer.LoadAsset<Texture2D>("Images\\Glow_" + num41, mode);
			}
			for (int num42 = 0; num42 < TextureAssets.HighlightMask.Length; num42++)
			{
				if (TileID.Sets.HasOutlines[num42])
				{
					TextureAssets.HighlightMask[num42] = AssetInitializer.LoadAsset<Texture2D>("Images\\Misc\\TileOutlines\\Tiles_" + num42, mode);
				}
			}
			for (int num43 = 0; num43 < (int)ExtrasID.Count; num43++)
			{
				TextureAssets.Extra[num43] = AssetInitializer.LoadAsset<Texture2D>("Images\\Extra_" + num43, mode);
			}
			for (int num44 = 0; num44 < 4; num44++)
			{
				TextureAssets.Coin[num44] = AssetInitializer.LoadAsset<Texture2D>("Images\\Coin_" + num44, mode);
			}
			TextureAssets.MagicPixel = AssetInitializer.LoadAsset<Texture2D>("Images\\MagicPixel", mode);
			TextureAssets.SettingsPanel = AssetInitializer.LoadAsset<Texture2D>("Images\\UI\\Settings_Panel", mode);
			TextureAssets.SettingsPanel2 = AssetInitializer.LoadAsset<Texture2D>("Images\\UI\\Settings_Panel_2", mode);
			for (int num45 = 0; num45 < TextureAssets.XmasTree.Length; num45++)
			{
				TextureAssets.XmasTree[num45] = AssetInitializer.LoadAsset<Texture2D>("Images\\Xmas_" + num45, mode);
			}
			for (int num46 = 0; num46 < 6; num46++)
			{
				TextureAssets.Clothes[num46] = AssetInitializer.LoadAsset<Texture2D>("Images\\Clothes_" + num46, mode);
			}
			for (int num47 = 0; num47 < TextureAssets.Flames.Length; num47++)
			{
				TextureAssets.Flames[num47] = AssetInitializer.LoadAsset<Texture2D>("Images\\Flame_" + num47, mode);
			}
			for (int num48 = 0; num48 < 8; num48++)
			{
				TextureAssets.MapIcon[num48] = AssetInitializer.LoadAsset<Texture2D>("Images\\Map_" + num48, mode);
			}
			for (int num49 = 0; num49 < TextureAssets.Underworld.Length; num49++)
			{
				TextureAssets.Underworld[num49] = AssetInitializer.LoadAsset<Texture2D>("Images/Backgrounds/Underworld " + num49, 0);
			}
			TextureAssets.Dest[0] = AssetInitializer.LoadAsset<Texture2D>("Images\\Dest1", mode);
			TextureAssets.Dest[1] = AssetInitializer.LoadAsset<Texture2D>("Images\\Dest2", mode);
			TextureAssets.Dest[2] = AssetInitializer.LoadAsset<Texture2D>("Images\\Dest3", mode);
			TextureAssets.Actuator = AssetInitializer.LoadAsset<Texture2D>("Images\\Actuator", mode);
			TextureAssets.Wire = AssetInitializer.LoadAsset<Texture2D>("Images\\Wires", mode);
			TextureAssets.Wire2 = AssetInitializer.LoadAsset<Texture2D>("Images\\Wires2", mode);
			TextureAssets.Wire3 = AssetInitializer.LoadAsset<Texture2D>("Images\\Wires3", mode);
			TextureAssets.Wire4 = AssetInitializer.LoadAsset<Texture2D>("Images\\Wires4", mode);
			TextureAssets.WireNew = AssetInitializer.LoadAsset<Texture2D>("Images\\WiresNew", mode);
			TextureAssets.FlyingCarpet = AssetInitializer.LoadAsset<Texture2D>("Images\\FlyingCarpet", mode);
			TextureAssets.Hb1 = AssetInitializer.LoadAsset<Texture2D>("Images\\HealthBar1", mode);
			TextureAssets.Hb2 = AssetInitializer.LoadAsset<Texture2D>("Images\\HealthBar2", mode);
			for (int num50 = 0; num50 < TextureAssets.NpcHead.Length; num50++)
			{
				TextureAssets.NpcHead[num50] = AssetInitializer.LoadAsset<Texture2D>("Images\\NPC_Head_" + num50, mode);
			}
			for (int num51 = 0; num51 < TextureAssets.NpcHeadBoss.Length; num51++)
			{
				TextureAssets.NpcHeadBoss[num51] = AssetInitializer.LoadAsset<Texture2D>("Images\\NPC_Head_Boss_" + num51, mode);
			}
			for (int num52 = 1; num52 < TextureAssets.BackPack.Length; num52++)
			{
				TextureAssets.BackPack[num52] = AssetInitializer.LoadAsset<Texture2D>("Images\\BackPack_" + num52, mode);
			}
			for (int num53 = 1; num53 < BuffID.Count; num53++)
			{
				TextureAssets.Buff[num53] = AssetInitializer.LoadAsset<Texture2D>("Images\\Buff_" + num53, mode);
			}
			Main.instance.LoadBackground(0);
			Main.instance.LoadBackground(49);
			TextureAssets.MinecartMount = AssetInitializer.LoadAsset<Texture2D>("Images\\Mount_Minecart", mode);
			for (int num54 = 0; num54 < TextureAssets.RudolphMount.Length; num54++)
			{
				TextureAssets.RudolphMount[num54] = AssetInitializer.LoadAsset<Texture2D>("Images\\Rudolph_" + num54, mode);
			}
			TextureAssets.BunnyMount = AssetInitializer.LoadAsset<Texture2D>("Images\\Mount_Bunny", mode);
			TextureAssets.PigronMount = AssetInitializer.LoadAsset<Texture2D>("Images\\Mount_Pigron", mode);
			TextureAssets.SlimeMount = AssetInitializer.LoadAsset<Texture2D>("Images\\Mount_Slime", mode);
			TextureAssets.TurtleMount = AssetInitializer.LoadAsset<Texture2D>("Images\\Mount_Turtle", mode);
			TextureAssets.UnicornMount = AssetInitializer.LoadAsset<Texture2D>("Images\\Mount_Unicorn", mode);
			TextureAssets.BasiliskMount = AssetInitializer.LoadAsset<Texture2D>("Images\\Mount_Basilisk", mode);
			TextureAssets.MinecartMechMount[0] = AssetInitializer.LoadAsset<Texture2D>("Images\\Mount_MinecartMech", mode);
			TextureAssets.MinecartMechMount[1] = AssetInitializer.LoadAsset<Texture2D>("Images\\Mount_MinecartMechGlow", mode);
			TextureAssets.CuteFishronMount[0] = AssetInitializer.LoadAsset<Texture2D>("Images\\Mount_CuteFishron1", mode);
			TextureAssets.CuteFishronMount[1] = AssetInitializer.LoadAsset<Texture2D>("Images\\Mount_CuteFishron2", mode);
			TextureAssets.MinecartWoodMount = AssetInitializer.LoadAsset<Texture2D>("Images\\Mount_MinecartWood", mode);
			TextureAssets.DesertMinecartMount = AssetInitializer.LoadAsset<Texture2D>("Images\\Mount_MinecartDesert", mode);
			TextureAssets.FishMinecartMount = AssetInitializer.LoadAsset<Texture2D>("Images\\Mount_MinecartMineCarp", mode);
			TextureAssets.BeeMount[0] = AssetInitializer.LoadAsset<Texture2D>("Images\\Mount_Bee", mode);
			TextureAssets.BeeMount[1] = AssetInitializer.LoadAsset<Texture2D>("Images\\Mount_BeeWings", mode);
			TextureAssets.UfoMount[0] = AssetInitializer.LoadAsset<Texture2D>("Images\\Mount_UFO", mode);
			TextureAssets.UfoMount[1] = AssetInitializer.LoadAsset<Texture2D>("Images\\Mount_UFOGlow", mode);
			TextureAssets.DrillMount[0] = AssetInitializer.LoadAsset<Texture2D>("Images\\Mount_DrillRing", mode);
			TextureAssets.DrillMount[1] = AssetInitializer.LoadAsset<Texture2D>("Images\\Mount_DrillSeat", mode);
			TextureAssets.DrillMount[2] = AssetInitializer.LoadAsset<Texture2D>("Images\\Mount_DrillDiode", mode);
			TextureAssets.DrillMount[3] = AssetInitializer.LoadAsset<Texture2D>("Images\\Mount_Glow_DrillRing", mode);
			TextureAssets.DrillMount[4] = AssetInitializer.LoadAsset<Texture2D>("Images\\Mount_Glow_DrillSeat", mode);
			TextureAssets.DrillMount[5] = AssetInitializer.LoadAsset<Texture2D>("Images\\Mount_Glow_DrillDiode", mode);
			TextureAssets.ScutlixMount[0] = AssetInitializer.LoadAsset<Texture2D>("Images\\Mount_Scutlix", mode);
			TextureAssets.ScutlixMount[1] = AssetInitializer.LoadAsset<Texture2D>("Images\\Mount_ScutlixEyes", mode);
			TextureAssets.ScutlixMount[2] = AssetInitializer.LoadAsset<Texture2D>("Images\\Mount_ScutlixEyeGlow", mode);
			for (int num55 = 0; num55 < TextureAssets.Gem.Length; num55++)
			{
				TextureAssets.Gem[num55] = AssetInitializer.LoadAsset<Texture2D>("Images\\Gem_" + num55, mode);
			}
			for (int num56 = 0; num56 < CloudID.Count; num56++)
			{
				TextureAssets.Cloud[num56] = AssetInitializer.LoadAsset<Texture2D>("Images\\Cloud_" + num56, mode);
			}
			for (int num57 = 0; num57 < 4; num57++)
			{
				TextureAssets.Star[num57] = AssetInitializer.LoadAsset<Texture2D>("Images\\Star_" + num57, mode);
			}
			for (int num58 = 0; num58 < 15; num58++)
			{
				TextureAssets.Liquid[num58] = AssetInitializer.LoadAsset<Texture2D>("Images\\Liquid_" + num58, mode);
				TextureAssets.LiquidSlope[num58] = AssetInitializer.LoadAsset<Texture2D>("Images\\LiquidSlope_" + num58, mode);
			}
			Main.instance.waterfallManager.LoadContent();
			TextureAssets.NpcToggle[0] = AssetInitializer.LoadAsset<Texture2D>("Images\\House_1", mode);
			TextureAssets.NpcToggle[1] = AssetInitializer.LoadAsset<Texture2D>("Images\\House_2", mode);
			TextureAssets.HbLock[0] = AssetInitializer.LoadAsset<Texture2D>("Images\\Lock_0", mode);
			TextureAssets.HbLock[1] = AssetInitializer.LoadAsset<Texture2D>("Images\\Lock_1", mode);
			TextureAssets.blockReplaceIcon[0] = AssetInitializer.LoadAsset<Texture2D>("Images\\UI\\BlockReplace_0", mode);
			TextureAssets.blockReplaceIcon[1] = AssetInitializer.LoadAsset<Texture2D>("Images\\UI\\BlockReplace_1", mode);
			TextureAssets.Grid = AssetInitializer.LoadAsset<Texture2D>("Images\\Grid", mode);
			TextureAssets.Trash = AssetInitializer.LoadAsset<Texture2D>("Images\\Trash", mode);
			TextureAssets.Cd = AssetInitializer.LoadAsset<Texture2D>("Images\\CoolDown", mode);
			TextureAssets.Logo = AssetInitializer.LoadAsset<Texture2D>("Images\\Logo", mode);
			TextureAssets.Logo2 = AssetInitializer.LoadAsset<Texture2D>("Images\\Logo2", mode);
			TextureAssets.Logo3 = AssetInitializer.LoadAsset<Texture2D>("Images\\Logo3", mode);
			TextureAssets.Logo4 = AssetInitializer.LoadAsset<Texture2D>("Images\\Logo4", mode);
			TextureAssets.Logo5 = AssetInitializer.LoadAsset<Texture2D>("Images\\Logo5", mode);
			TextureAssets.Logo6 = AssetInitializer.LoadAsset<Texture2D>("Images\\Logo6", mode);
			TextureAssets.Dust = AssetInitializer.LoadAsset<Texture2D>("Images\\Dust", mode);
			TextureAssets.Sun = AssetInitializer.LoadAsset<Texture2D>("Images\\Sun", mode);
			TextureAssets.Sun2 = AssetInitializer.LoadAsset<Texture2D>("Images\\Sun2", mode);
			TextureAssets.Sun3 = AssetInitializer.LoadAsset<Texture2D>("Images\\Sun3", mode);
			TextureAssets.BlackTile = AssetInitializer.LoadAsset<Texture2D>("Images\\Black_Tile", mode);
			TextureAssets.Heart = AssetInitializer.LoadAsset<Texture2D>("Images\\Heart", mode);
			TextureAssets.Heart2 = AssetInitializer.LoadAsset<Texture2D>("Images\\Heart2", mode);
			TextureAssets.Bubble = AssetInitializer.LoadAsset<Texture2D>("Images\\Bubble", mode);
			TextureAssets.Flame = AssetInitializer.LoadAsset<Texture2D>("Images\\Flame", mode);
			TextureAssets.Mana = AssetInitializer.LoadAsset<Texture2D>("Images\\Mana", mode);
			for (int num59 = 0; num59 < TextureAssets.Cursors.Length; num59++)
			{
				TextureAssets.Cursors[num59] = AssetInitializer.LoadAsset<Texture2D>("Images\\UI\\Cursor_" + num59, mode);
			}
			TextureAssets.CursorRadial = AssetInitializer.LoadAsset<Texture2D>("Images\\UI\\Radial", mode);
			TextureAssets.Ninja = AssetInitializer.LoadAsset<Texture2D>("Images\\Ninja", mode);
			TextureAssets.AntLion = AssetInitializer.LoadAsset<Texture2D>("Images\\AntlionBody", mode);
			TextureAssets.SpikeBase = AssetInitializer.LoadAsset<Texture2D>("Images\\Spike_Base", mode);
			TextureAssets.Wood[0] = AssetInitializer.LoadAsset<Texture2D>("Images\\Tiles_5_0", mode);
			TextureAssets.Wood[1] = AssetInitializer.LoadAsset<Texture2D>("Images\\Tiles_5_1", mode);
			TextureAssets.Wood[2] = AssetInitializer.LoadAsset<Texture2D>("Images\\Tiles_5_2", mode);
			TextureAssets.Wood[3] = AssetInitializer.LoadAsset<Texture2D>("Images\\Tiles_5_3", mode);
			TextureAssets.Wood[4] = AssetInitializer.LoadAsset<Texture2D>("Images\\Tiles_5_4", mode);
			TextureAssets.Wood[5] = AssetInitializer.LoadAsset<Texture2D>("Images\\Tiles_5_5", mode);
			TextureAssets.Wood[6] = AssetInitializer.LoadAsset<Texture2D>("Images\\Tiles_5_6", mode);
			TextureAssets.SmileyMoon = AssetInitializer.LoadAsset<Texture2D>("Images\\Moon_Smiley", mode);
			TextureAssets.PumpkinMoon = AssetInitializer.LoadAsset<Texture2D>("Images\\Moon_Pumpkin", mode);
			TextureAssets.SnowMoon = AssetInitializer.LoadAsset<Texture2D>("Images\\Moon_Snow", mode);
			for (int num60 = 0; num60 < TextureAssets.CageTop.Length; num60++)
			{
				TextureAssets.CageTop[num60] = AssetInitializer.LoadAsset<Texture2D>("Images\\CageTop_" + num60, mode);
			}
			for (int num61 = 0; num61 < TextureAssets.Moon.Length; num61++)
			{
				TextureAssets.Moon[num61] = AssetInitializer.LoadAsset<Texture2D>("Images\\Moon_" + num61, mode);
			}
			for (int num62 = 0; num62 < TextureAssets.TreeTop.Length; num62++)
			{
				TextureAssets.TreeTop[num62] = AssetInitializer.LoadAsset<Texture2D>("Images\\Tree_Tops_" + num62, mode);
			}
			for (int num63 = 0; num63 < TextureAssets.TreeBranch.Length; num63++)
			{
				TextureAssets.TreeBranch[num63] = AssetInitializer.LoadAsset<Texture2D>("Images\\Tree_Branches_" + num63, mode);
			}
			TextureAssets.ShroomCap = AssetInitializer.LoadAsset<Texture2D>("Images\\Shroom_Tops", mode);
			TextureAssets.InventoryBack = AssetInitializer.LoadAsset<Texture2D>("Images\\Inventory_Back", mode);
			TextureAssets.InventoryBack2 = AssetInitializer.LoadAsset<Texture2D>("Images\\Inventory_Back2", mode);
			TextureAssets.InventoryBack3 = AssetInitializer.LoadAsset<Texture2D>("Images\\Inventory_Back3", mode);
			TextureAssets.InventoryBack4 = AssetInitializer.LoadAsset<Texture2D>("Images\\Inventory_Back4", mode);
			TextureAssets.InventoryBack5 = AssetInitializer.LoadAsset<Texture2D>("Images\\Inventory_Back5", mode);
			TextureAssets.InventoryBack6 = AssetInitializer.LoadAsset<Texture2D>("Images\\Inventory_Back6", mode);
			TextureAssets.InventoryBack7 = AssetInitializer.LoadAsset<Texture2D>("Images\\Inventory_Back7", mode);
			TextureAssets.InventoryBack8 = AssetInitializer.LoadAsset<Texture2D>("Images\\Inventory_Back8", mode);
			TextureAssets.InventoryBack9 = AssetInitializer.LoadAsset<Texture2D>("Images\\Inventory_Back9", mode);
			TextureAssets.InventoryBack10 = AssetInitializer.LoadAsset<Texture2D>("Images\\Inventory_Back10", mode);
			TextureAssets.InventoryBack11 = AssetInitializer.LoadAsset<Texture2D>("Images\\Inventory_Back11", mode);
			TextureAssets.InventoryBack12 = AssetInitializer.LoadAsset<Texture2D>("Images\\Inventory_Back12", mode);
			TextureAssets.InventoryBack13 = AssetInitializer.LoadAsset<Texture2D>("Images\\Inventory_Back13", mode);
			TextureAssets.InventoryBack14 = AssetInitializer.LoadAsset<Texture2D>("Images\\Inventory_Back14", mode);
			TextureAssets.InventoryBack15 = AssetInitializer.LoadAsset<Texture2D>("Images\\Inventory_Back15", mode);
			TextureAssets.InventoryBack16 = AssetInitializer.LoadAsset<Texture2D>("Images\\Inventory_Back16", mode);
			TextureAssets.InventoryBack17 = AssetInitializer.LoadAsset<Texture2D>("Images\\Inventory_Back17", mode);
			TextureAssets.InventoryBack18 = AssetInitializer.LoadAsset<Texture2D>("Images\\Inventory_Back18", mode);
			TextureAssets.InventoryBack19 = AssetInitializer.LoadAsset<Texture2D>("Images\\Inventory_Back19", mode);
			TextureAssets.InventoryBack20 = AssetInitializer.LoadAsset<Texture2D>("Images\\Inventory_Back20", mode);
			TextureAssets.InventoryBack21 = AssetInitializer.LoadAsset<Texture2D>("Images\\Inventory_Back21", mode);
			TextureAssets.InventoryBack22 = AssetInitializer.LoadAsset<Texture2D>("Images\\Inventory_Back22", mode);
			TextureAssets.InventoryBack23 = AssetInitializer.LoadAsset<Texture2D>("Images\\Inventory_Back23", mode);
			TextureAssets.InventoryBack24 = AssetInitializer.LoadAsset<Texture2D>("Images\\Inventory_Back24", mode);
			TextureAssets.HairStyleBack = AssetInitializer.LoadAsset<Texture2D>("Images\\HairStyleBack", mode);
			TextureAssets.ClothesStyleBack = AssetInitializer.LoadAsset<Texture2D>("Images\\ClothesStyleBack", mode);
			TextureAssets.InventoryTickOff = AssetInitializer.LoadAsset<Texture2D>("Images\\Inventory_Tick_Off", mode);
			TextureAssets.InventoryTickOn = AssetInitializer.LoadAsset<Texture2D>("Images\\Inventory_Tick_On", mode);
			TextureAssets.TextBack = AssetInitializer.LoadAsset<Texture2D>("Images\\Text_Back", mode);
			TextureAssets.Chat = AssetInitializer.LoadAsset<Texture2D>("Images\\Chat", mode);
			TextureAssets.Chat2 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chat2", mode);
			TextureAssets.ChatBack = AssetInitializer.LoadAsset<Texture2D>("Images\\Chat_Back", mode);
			TextureAssets.Team = AssetInitializer.LoadAsset<Texture2D>("Images\\Team", mode);
			PlayerDataInitializer.Load();
			TextureAssets.Chaos = AssetInitializer.LoadAsset<Texture2D>("Images\\Chaos", mode);
			TextureAssets.EyeLaser = AssetInitializer.LoadAsset<Texture2D>("Images\\Eye_Laser", mode);
			TextureAssets.BoneEyes = AssetInitializer.LoadAsset<Texture2D>("Images\\Bone_Eyes", mode);
			TextureAssets.BoneLaser = AssetInitializer.LoadAsset<Texture2D>("Images\\Bone_Laser", mode);
			TextureAssets.LightDisc = AssetInitializer.LoadAsset<Texture2D>("Images\\Light_Disc", mode);
			TextureAssets.Confuse = AssetInitializer.LoadAsset<Texture2D>("Images\\Confuse", mode);
			TextureAssets.Probe = AssetInitializer.LoadAsset<Texture2D>("Images\\Probe", mode);
			TextureAssets.SunOrb = AssetInitializer.LoadAsset<Texture2D>("Images\\SunOrb", mode);
			TextureAssets.SunAltar = AssetInitializer.LoadAsset<Texture2D>("Images\\SunAltar", mode);
			TextureAssets.XmasLight = AssetInitializer.LoadAsset<Texture2D>("Images\\XmasLight", mode);
			TextureAssets.Beetle = AssetInitializer.LoadAsset<Texture2D>("Images\\BeetleOrb", mode);
			for (int num64 = 0; num64 < (int)ChainID.Count; num64++)
			{
				TextureAssets.Chains[num64] = AssetInitializer.LoadAsset<Texture2D>("Images\\Chains_" + num64, mode);
			}
			TextureAssets.Chain20 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain20", mode);
			TextureAssets.FishingLine = AssetInitializer.LoadAsset<Texture2D>("Images\\FishingLine", mode);
			TextureAssets.Chain = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain", mode);
			TextureAssets.Chain2 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain2", mode);
			TextureAssets.Chain3 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain3", mode);
			TextureAssets.Chain4 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain4", mode);
			TextureAssets.Chain5 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain5", mode);
			TextureAssets.Chain6 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain6", mode);
			TextureAssets.Chain7 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain7", mode);
			TextureAssets.Chain8 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain8", mode);
			TextureAssets.Chain9 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain9", mode);
			TextureAssets.Chain10 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain10", mode);
			TextureAssets.Chain11 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain11", mode);
			TextureAssets.Chain12 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain12", mode);
			TextureAssets.Chain13 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain13", mode);
			TextureAssets.Chain14 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain14", mode);
			TextureAssets.Chain15 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain15", mode);
			TextureAssets.Chain16 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain16", mode);
			TextureAssets.Chain17 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain17", mode);
			TextureAssets.Chain18 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain18", mode);
			TextureAssets.Chain19 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain19", mode);
			TextureAssets.Chain20 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain20", mode);
			TextureAssets.Chain21 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain21", mode);
			TextureAssets.Chain22 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain22", mode);
			TextureAssets.Chain23 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain23", mode);
			TextureAssets.Chain24 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain24", mode);
			TextureAssets.Chain25 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain25", mode);
			TextureAssets.Chain26 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain26", mode);
			TextureAssets.Chain27 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain27", mode);
			TextureAssets.Chain28 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain28", mode);
			TextureAssets.Chain29 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain29", mode);
			TextureAssets.Chain30 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain30", mode);
			TextureAssets.Chain31 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain31", mode);
			TextureAssets.Chain32 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain32", mode);
			TextureAssets.Chain33 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain33", mode);
			TextureAssets.Chain34 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain34", mode);
			TextureAssets.Chain35 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain35", mode);
			TextureAssets.Chain36 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain36", mode);
			TextureAssets.Chain37 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain37", mode);
			TextureAssets.Chain38 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain38", mode);
			TextureAssets.Chain39 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain39", mode);
			TextureAssets.Chain40 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain40", mode);
			TextureAssets.Chain41 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain41", mode);
			TextureAssets.Chain42 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain42", mode);
			TextureAssets.Chain43 = AssetInitializer.LoadAsset<Texture2D>("Images\\Chain43", mode);
			TextureAssets.EyeLaserSmall = AssetInitializer.LoadAsset<Texture2D>("Images\\Eye_Laser_Small", mode);
			TextureAssets.BoneArm = AssetInitializer.LoadAsset<Texture2D>("Images\\Arm_Bone", mode);
			TextureAssets.PumpkingArm = AssetInitializer.LoadAsset<Texture2D>("Images\\PumpkingArm", mode);
			TextureAssets.PumpkingCloak = AssetInitializer.LoadAsset<Texture2D>("Images\\PumpkingCloak", mode);
			TextureAssets.BoneArm2 = AssetInitializer.LoadAsset<Texture2D>("Images\\Arm_Bone_2", mode);
			TextureAssets.BoneArm3 = AssetInitializer.LoadAsset<Texture2D>("Images\\Arm_Bone_3", mode);
			for (int num65 = 1; num65 < TextureAssets.GemChain.Length; num65++)
			{
				TextureAssets.GemChain[num65] = AssetInitializer.LoadAsset<Texture2D>("Images\\GemChain_" + num65, mode);
			}
			for (int num66 = 1; num66 < TextureAssets.Golem.Length; num66++)
			{
				TextureAssets.Golem[num66] = AssetInitializer.LoadAsset<Texture2D>("Images\\GolemLights" + num66, mode);
			}
			TextureAssets.GolfSwingBarFill = AssetInitializer.LoadAsset<Texture2D>("Images\\UI\\GolfSwingBarFill", mode);
			TextureAssets.GolfSwingBarPanel = AssetInitializer.LoadAsset<Texture2D>("Images\\UI\\GolfSwingBarPanel", mode);
			TextureAssets.SpawnPoint = AssetInitializer.LoadAsset<Texture2D>("Images\\UI\\SpawnPoint", mode);
			TextureAssets.SpawnBed = AssetInitializer.LoadAsset<Texture2D>("Images\\UI\\SpawnBed", mode);
			TextureAssets.MapPing = AssetInitializer.LoadAsset<Texture2D>("Images\\UI\\MapPing", mode);
			TextureAssets.GolfBallArrow = AssetInitializer.LoadAsset<Texture2D>("Images\\UI\\GolfBall_Arrow", mode);
			TextureAssets.GolfBallArrowShadow = AssetInitializer.LoadAsset<Texture2D>("Images\\UI\\GolfBall_Arrow_Shadow", mode);
			TextureAssets.GolfBallOutline = AssetInitializer.LoadAsset<Texture2D>("Images\\Misc\\GolfBallOutline", mode);
			TextureAssets.NpcPortraitBackground = AssetInitializer.LoadAsset<Texture2D>("Images\\TownNPCs\\Portraits\\Portrait_Window", mode);
			Main.ResourceSetsManager.LoadContent(mode);
			Main.MinimapFrameManagerInstance.LoadContent(mode);
			Main.AchievementAdvisor.LoadContent();
		}

		// Token: 0x0600156C RID: 5484 RVA: 0x004C5FFF File Offset: 0x004C41FF
		private static Asset<T> LoadAsset<T>(string assetName, AssetRequestMode mode) where T : class
		{
			return Main.Assets.Request<T>(assetName, mode);
		}
	}
}
