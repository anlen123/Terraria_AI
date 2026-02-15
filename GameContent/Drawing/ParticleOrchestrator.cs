using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Utilities;
using Terraria.Audio;
using Terraria.GameContent.NetModules;
using Terraria.Graphics.CameraModifiers;
using Terraria.Graphics.Renderers;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Net;

namespace Terraria.GameContent.Drawing
{
	// Token: 0x02000440 RID: 1088
	public class ParticleOrchestrator
	{
		// Token: 0x060030D2 RID: 12498 RVA: 0x005BD674 File Offset: 0x005BB874
		public static void RequestParticleSpawn(bool clientOnly, ParticleOrchestraType type, ParticleOrchestraSettings settings, int? overrideInvokingPlayerIndex = null)
		{
			settings.IndexOfPlayerWhoInvokedThis = (byte)Main.myPlayer;
			if (overrideInvokingPlayerIndex != null)
			{
				settings.IndexOfPlayerWhoInvokedThis = (byte)overrideInvokingPlayerIndex.Value;
			}
			ParticleOrchestrator.SpawnParticlesDirect(type, settings);
			if (!clientOnly && Main.netMode == 1)
			{
				NetManager.Instance.SendToServer(NetParticlesModule.Serialize(type, settings));
			}
		}

		// Token: 0x060030D3 RID: 12499 RVA: 0x005BD6C9 File Offset: 0x005BB8C9
		public static void BroadcastParticleSpawn(ParticleOrchestraType type, ParticleOrchestraSettings settings)
		{
			settings.IndexOfPlayerWhoInvokedThis = (byte)Main.myPlayer;
			if (!Main.dedServ)
			{
				ParticleOrchestrator.SpawnParticlesDirect(type, settings);
				return;
			}
			NetManager.Instance.Broadcast(NetParticlesModule.Serialize(type, settings), -1);
		}

		// Token: 0x060030D4 RID: 12500 RVA: 0x005BD6F9 File Offset: 0x005BB8F9
		public static void BroadcastOrRequestParticleSpawn(ParticleOrchestraType type, ParticleOrchestraSettings settings)
		{
			settings.IndexOfPlayerWhoInvokedThis = (byte)Main.myPlayer;
			if (!Main.dedServ)
			{
				ParticleOrchestrator.SpawnParticlesDirect(type, settings);
			}
			if (Main.netMode != 0)
			{
				NetManager.Instance.SendToServerOrBroadcast(NetParticlesModule.Serialize(type, settings));
			}
		}

		// Token: 0x060030D5 RID: 12501 RVA: 0x005BD72E File Offset: 0x005BB92E
		private static FadingParticle GetNewFadingParticle()
		{
			return new FadingParticle();
		}

		// Token: 0x060030D6 RID: 12502 RVA: 0x005BD735 File Offset: 0x005BB935
		private static FadingPlayerShaderParticle GetNewFadingPlayerShaderParticle()
		{
			return new FadingPlayerShaderParticle();
		}

		// Token: 0x060030D7 RID: 12503 RVA: 0x005BD73C File Offset: 0x005BB93C
		private static LittleFlyingCritterParticle GetNewPooFlyParticle()
		{
			return new LittleFlyingCritterParticle();
		}

		// Token: 0x060030D8 RID: 12504 RVA: 0x005BD73C File Offset: 0x005BB93C
		private static LittleFlyingCritterParticle GetNewNatureFlyParticle()
		{
			return new LittleFlyingCritterParticle();
		}

		// Token: 0x060030D9 RID: 12505 RVA: 0x005BD743 File Offset: 0x005BB943
		private static ItemTransferParticle GetNewItemTransferParticle()
		{
			return new ItemTransferParticle();
		}

		// Token: 0x060030DA RID: 12506 RVA: 0x005BD74A File Offset: 0x005BB94A
		private static FakeFishParticle GetNewFakeFishParticle()
		{
			return new FakeFishParticle();
		}

		// Token: 0x060030DB RID: 12507 RVA: 0x005BD751 File Offset: 0x005BB951
		private static FlameParticle GetNewFlameParticle()
		{
			return new FlameParticle();
		}

		// Token: 0x060030DC RID: 12508 RVA: 0x005BD758 File Offset: 0x005BB958
		private static RandomizedFrameParticle GetNewRandomizedFrameParticle()
		{
			return new RandomizedFrameParticle();
		}

		// Token: 0x060030DD RID: 12509 RVA: 0x005BD75F File Offset: 0x005BB95F
		private static PrettySparkleParticle GetNewPrettySparkleParticle()
		{
			return new PrettySparkleParticle();
		}

		// Token: 0x060030DE RID: 12510 RVA: 0x005BD766 File Offset: 0x005BB966
		private static GasParticle GetNewGasParticle()
		{
			return new GasParticle();
		}

		// Token: 0x060030DF RID: 12511 RVA: 0x005BD76D File Offset: 0x005BB96D
		private static BloodyExplosionParticle GetNewBloodyExplosionParticle()
		{
			return new BloodyExplosionParticle();
		}

		// Token: 0x060030E0 RID: 12512 RVA: 0x005BD774 File Offset: 0x005BB974
		private static ShockIconParticle GetNewShockIconParticle()
		{
			return new ShockIconParticle();
		}

		// Token: 0x060030E1 RID: 12513 RVA: 0x005BD743 File Offset: 0x005BB943
		private static ItemTransferParticle GetNewItemTransferParticle_ScreenSpace()
		{
			return new ItemTransferParticle();
		}

		// Token: 0x060030E2 RID: 12514 RVA: 0x005BD77B File Offset: 0x005BB97B
		private static StormLightningParticle GetNewStormLightningParticle()
		{
			return new StormLightningParticle();
		}

		// Token: 0x060030E3 RID: 12515 RVA: 0x005BD784 File Offset: 0x005BB984
		public static void SpawnParticlesDirect(ParticleOrchestraType type, ParticleOrchestraSettings settings)
		{
			if (Main.netMode == 2)
			{
				return;
			}
			switch (type)
			{
			case ParticleOrchestraType.Keybrand:
				ParticleOrchestrator.Spawn_Keybrand(settings);
				return;
			case ParticleOrchestraType.FlameWaders:
				ParticleOrchestrator.Spawn_FlameWaders(settings);
				return;
			case ParticleOrchestraType.StellarTune:
				ParticleOrchestrator.Spawn_StellarTune(settings);
				return;
			case ParticleOrchestraType.WallOfFleshGoatMountFlames:
				ParticleOrchestrator.Spawn_WallOfFleshGoatMountFlames(settings);
				return;
			case ParticleOrchestraType.BlackLightningHit:
				ParticleOrchestrator.Spawn_BlackLightningHit(settings);
				return;
			case ParticleOrchestraType.RainbowRodHit:
				ParticleOrchestrator.Spawn_RainbowRodHit(settings);
				return;
			case ParticleOrchestraType.BlackLightningSmall:
				ParticleOrchestrator.Spawn_BlackLightningSmall(settings);
				return;
			case ParticleOrchestraType.StardustPunch:
				ParticleOrchestrator.Spawn_StardustPunch(settings);
				return;
			case ParticleOrchestraType.PrincessWeapon:
				ParticleOrchestrator.Spawn_PrincessWeapon(settings);
				return;
			case ParticleOrchestraType.PaladinsHammer:
				ParticleOrchestrator.Spawn_PaladinsHammer(settings);
				return;
			case ParticleOrchestraType.NightsEdge:
				ParticleOrchestrator.Spawn_NightsEdge(settings);
				return;
			case ParticleOrchestraType.SilverBulletSparkle:
				ParticleOrchestrator.Spawn_SilverBulletSparkle(settings);
				return;
			case ParticleOrchestraType.TrueNightsEdge:
				ParticleOrchestrator.Spawn_TrueNightsEdge(settings);
				return;
			case ParticleOrchestraType.Excalibur:
				ParticleOrchestrator.Spawn_Excalibur(settings);
				return;
			case ParticleOrchestraType.TrueExcalibur:
				ParticleOrchestrator.Spawn_TrueExcalibur(settings);
				return;
			case ParticleOrchestraType.TerraBlade:
				ParticleOrchestrator.Spawn_TerraBlade(settings);
				return;
			case ParticleOrchestraType.ChlorophyteLeafCrystalPassive:
				ParticleOrchestrator.Spawn_LeafCrystalPassive(settings);
				return;
			case ParticleOrchestraType.ChlorophyteLeafCrystalShot:
				ParticleOrchestrator.Spawn_LeafCrystalShot(settings);
				return;
			case ParticleOrchestraType.BestReforge:
				ParticleOrchestrator.Spawn_BestReforge(settings);
				return;
			case ParticleOrchestraType.PetExchange:
				ParticleOrchestrator.Spawn_PetExchange(settings);
				return;
			case ParticleOrchestraType.SlapHand:
				ParticleOrchestrator.Spawn_SlapHand(settings);
				return;
			case ParticleOrchestraType.FlyMeal:
				ParticleOrchestrator.Spawn_FlyMeal(settings);
				return;
			case ParticleOrchestraType.VampireOnFire:
				ParticleOrchestrator.Spawn_VampireOnFire(settings);
				return;
			case ParticleOrchestraType.GasTrap:
				ParticleOrchestrator.Spawn_GasTrap(settings);
				return;
			case ParticleOrchestraType.ItemTransfer:
				ParticleOrchestrator.Spawn_ItemTransfer(settings);
				return;
			case ParticleOrchestraType.ShimmerArrow:
				ParticleOrchestrator.Spawn_ShimmerArrow(settings);
				return;
			case ParticleOrchestraType.TownSlimeTransform:
				ParticleOrchestrator.Spawn_TownSlimeTransform(settings);
				return;
			case ParticleOrchestraType.LoadoutChange:
				ParticleOrchestrator.Spawn_LoadOutChange(settings);
				return;
			case ParticleOrchestraType.ShimmerBlock:
				ParticleOrchestrator.Spawn_ShimmerBlock(settings);
				return;
			case ParticleOrchestraType.Digestion:
				ParticleOrchestrator.Spawn_Digestion(settings);
				return;
			case ParticleOrchestraType.WaffleIron:
				ParticleOrchestrator.Spawn_WaffleIron(settings);
				return;
			case ParticleOrchestraType.PooFly:
				ParticleOrchestrator.Spawn_PooFly(settings);
				return;
			case ParticleOrchestraType.ShimmerTownNPC:
				ParticleOrchestrator.Spawn_ShimmerTownNPC(settings);
				return;
			case ParticleOrchestraType.ShimmerTownNPCSend:
				ParticleOrchestrator.Spawn_ShimmerTownNPCSend(settings);
				return;
			case ParticleOrchestraType.DeadCellsMushroomBoiExplosion:
				ParticleOrchestrator.Spawn_DeadCellsMushroomBoiExplosion(settings);
				return;
			case ParticleOrchestraType.DeadCellsDownDashExplosion:
				ParticleOrchestrator.Spawn_DeadCellsDownDashExplosion(settings);
				return;
			case ParticleOrchestraType.DeadCellsBarnacleShotFiring:
				ParticleOrchestrator.Spawn_DeadCellsBarnacleShotFiring(settings);
				return;
			case ParticleOrchestraType.BlueLightningSmall:
				ParticleOrchestrator.Spawn_BlueLightningSmall(settings);
				return;
			case ParticleOrchestraType.ShadowOrbExplosion:
				ParticleOrchestrator.Spawn_ShadowOrbExplosion(settings);
				return;
			case ParticleOrchestraType.UFOLaser:
				ParticleOrchestrator.Spawn_UFOLaser(settings);
				return;
			case ParticleOrchestraType.DeadCellsBeheadedEffect:
				ParticleOrchestrator.Spawn_DeadCellsHeadEffect(settings);
				return;
			case ParticleOrchestraType.DeadCellsFlint:
				ParticleOrchestrator.Spawn_DeadCellsFlint(settings);
				return;
			case ParticleOrchestraType.DeadCellsBarrelExplosion:
				ParticleOrchestrator.Spawn_DeadCellsBarrelExplosion(settings);
				return;
			case ParticleOrchestraType.DeadCellsMushroomBoiTargetFound:
				ParticleOrchestrator.Spawn_DeadCellsMushroomBoiTargetFound(settings);
				return;
			case ParticleOrchestraType.MoonLordWhipHit:
				ParticleOrchestrator.Spawn_MoonLordWhip(settings);
				return;
			case ParticleOrchestraType.MoonLordWhipEye:
				ParticleOrchestrator.Spawn_MoonLordWhipEye(settings);
				return;
			case ParticleOrchestraType.PlayerVoiceOverrideSound:
				ParticleOrchestrator.Spawn_PlayerVoiceOverrideSound(settings);
				return;
			case ParticleOrchestraType.RainbowBoulder1:
				ParticleOrchestrator.Spawn_RainbowBoulder1(settings);
				return;
			case ParticleOrchestraType.RainbowBoulder2:
				ParticleOrchestrator.Spawn_RainbowBoulder2(settings);
				return;
			case ParticleOrchestraType.RainbowBoulder3:
				ParticleOrchestrator.Spawn_RainbowBoulder3(settings);
				return;
			case ParticleOrchestraType.RainbowBoulder4:
				ParticleOrchestrator.Spawn_RainbowBoulder4(settings, false);
				return;
			case ParticleOrchestraType.FakeFish:
				ParticleOrchestrator.Spawn_FakeFish(settings, false);
				return;
			case ParticleOrchestraType.LakeSparkle:
				ParticleOrchestrator.Spawn_LakeSparkle(settings);
				return;
			case ParticleOrchestraType.NatureFly:
				ParticleOrchestrator.Spawn_NatureFly(settings);
				return;
			case ParticleOrchestraType.FakeFishJump:
				ParticleOrchestrator.Spawn_FakeFish(settings, true);
				return;
			case ParticleOrchestraType.ClassyCane:
				ParticleOrchestrator.Spawn_ClassyCane(settings);
				return;
			case ParticleOrchestraType.MagnetSphereBolt:
				ParticleOrchestrator.Spawn_MagnetSphereBolt(settings);
				return;
			case ParticleOrchestraType.HeatRay:
				ParticleOrchestrator.Spawn_HeatRay(settings);
				return;
			case ParticleOrchestraType.ShadowbeamHostile:
				ParticleOrchestrator.Spawn_Shadowbeam(settings, true);
				return;
			case ParticleOrchestraType.ShadowbeamFriendly:
				ParticleOrchestrator.Spawn_Shadowbeam(settings, false);
				return;
			case ParticleOrchestraType.RainbowBoulderPetBounce:
				ParticleOrchestrator.Spawn_RainbowBoulder4(settings, true);
				return;
			case ParticleOrchestraType.StormLightning:
				ParticleOrchestrator.Spawn_StormLightning(settings);
				return;
			case ParticleOrchestraType.StormlightningWindup:
				ParticleOrchestrator.Spawn_StormLightningWindup(settings);
				return;
			case ParticleOrchestraType.PaladinsShieldHit:
				ParticleOrchestrator.Spawn_PaladinsShieldHit(settings);
				return;
			case ParticleOrchestraType.HeroicisSetSpawnSound:
				ParticleOrchestrator.Spawn_HeroicisSetSpawnSound(settings);
				return;
			case ParticleOrchestraType.BlueLightningSmallLong:
				ParticleOrchestrator.Spawn_BlueLightningSmallLong(settings);
				return;
			case ParticleOrchestraType.InScreenDungeonSpawn:
				ParticleOrchestrator.Spawn_InScreenDungeonSpawn(settings);
				return;
			case ParticleOrchestraType.CattivaHit:
				ParticleOrchestrator.Spawn_CattivaHit(settings);
				return;
			case ParticleOrchestraType.PaladinsHammerShockwave:
				ParticleOrchestrator.Spawn_PaladinsHammerShockwave(settings);
				return;
			default:
				return;
			}
		}

		// Token: 0x060030E4 RID: 12516 RVA: 0x005BDAA0 File Offset: 0x005BBCA0
		private static void Spawn_PaladinsHammerShockwave(ParticleOrchestraSettings settings)
		{
			Vector2 positionInWorld = settings.PositionInWorld;
			int num = (int)settings.MovementVector.X;
			int num2 = (int)settings.MovementVector.Y;
			Vector2 vector = positionInWorld + new Vector2((float)num * 0.5f, (float)num2 * 0.5f);
			SoundEngine.PlaySound(SoundID.Item180, vector, 0f, 1f);
			for (int i = 0; i < 200; i++)
			{
				if (Main.rand.Next(2) == 0)
				{
					int num3 = Dust.NewDust(positionInWorld, num, num2, 57, 0f, 0f, 200, default(Color), 1.2f);
					Main.dust[num3].velocity = Main.dust[num3].position - vector;
					Main.dust[num3].velocity.Normalize();
					Main.dust[num3].velocity *= 9.5f;
					Main.dust[num3].noGravity = true;
				}
				if (Main.rand.Next(3) == 0)
				{
					int num4 = Dust.NewDust(positionInWorld, num, num2, 43, 0f, 0f, 254, default(Color), 0.3f);
					Main.dust[num4].velocity = Main.dust[num4].position - vector;
					Main.dust[num4].velocity.Normalize();
					Main.dust[num4].velocity *= 9.5f;
					Main.dust[num4].noGravity = true;
				}
			}
		}

		// Token: 0x060030E5 RID: 12517 RVA: 0x005BDC48 File Offset: 0x005BBE48
		private static void Spawn_InScreenDungeonSpawn(ParticleOrchestraSettings settings)
		{
			Vector2 movementVector = settings.MovementVector;
			Vector2 positionInWorld = settings.PositionInWorld;
			int uniqueInfoPiece = settings.UniqueInfoPiece;
			Vector2 value = positionInWorld + new Vector2(-0.5f, -1f) * movementVector;
			int num = Main.rand.Next(2, 5);
			for (int i = 0; i < num; i++)
			{
				Gore gore = Gore.NewGorePerfect(value + new Vector2(-16f, -16f) + movementVector * new Vector2(Main.rand.NextFloat(), 0.5f + Main.rand.NextFloat() * 0.5f), Main.rand.NextVector2Circular(2f, 2f), Main.rand.Next(61, 64), 1f);
				gore.scale *= 1f;
				gore.position -= gore.velocity * 5f;
			}
			SoundEngine.PlaySound(ContentSamples.NpcsByNetId[uniqueInfoPiece].HitSound, (int)settings.PositionInWorld.X, (int)settings.PositionInWorld.Y, 0f, 1f);
		}

		// Token: 0x060030E6 RID: 12518 RVA: 0x005BDD88 File Offset: 0x005BBF88
		private static void Spawn_HeroicisSetSpawnSound(ParticleOrchestraSettings settings)
		{
			SoundEngine.PlaySound(SoundID.RainbowBoulder, (int)settings.PositionInWorld.X, (int)settings.PositionInWorld.Y, 0f, 1f);
		}

		// Token: 0x060030E7 RID: 12519 RVA: 0x005BDDB8 File Offset: 0x005BBFB8
		private static void Spawn_PaladinsShieldHit(ParticleOrchestraSettings settings)
		{
			Main.instance.LoadItem(938);
			Asset<Texture2D> asset = TextureAssets.Item[938];
			if (!asset.IsLoaded)
			{
				return;
			}
			Vector2 positionInWorld = settings.PositionInWorld;
			int num = (int)positionInWorld.X;
			int num2 = (int)positionInWorld.Y;
			Vector2 value = Vector2.Zero;
			if (Main.player.IndexInRange(num))
			{
				Player player = Main.player[num];
				value = player.MountedCenter;
				FadingParticle fadingParticle = ParticleOrchestrator._poolFading.RequestParticle();
				int num3 = 30;
				fadingParticle.followPlayerIndex = num;
				fadingParticle.ColorTint = new Color(255, 255, 255, 220) * 0.85f;
				fadingParticle.SetBasicInfo(asset, null, Vector2.Zero, Vector2.Zero);
				fadingParticle.SetTypeInfo((float)num3, true);
				fadingParticle.FadeInNormalizedTime = 0.1f;
				fadingParticle.FadeOutNormalizedTime = 0.1f;
				fadingParticle.Scale = new Vector2(1f, 1f) * 0.65f;
				fadingParticle.ScaleVelocity = new Vector2(2f, 2f) / (float)num3;
				fadingParticle.ScaleAcceleration = -fadingParticle.ScaleVelocity / (float)num3;
				Main.ParticleSystem_World_OverPlayers.Add(fadingParticle);
				for (int i = 0; i < 4; i++)
				{
					Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, 57, 0f, 0f, 0, default(Color), 1f);
					dust.velocity *= 3f;
					dust.noGravity = true;
					dust.scale = 1f;
					dust.fadeIn = 1.5f;
					dust.noLight = true;
				}
			}
			if (Main.player.IndexInRange(num2))
			{
				Player player2 = Main.player[num2];
				FadingParticle fadingParticle2 = ParticleOrchestrator._poolFading.RequestParticle();
				float num4 = 15f;
				fadingParticle2.followPlayerIndex = num;
				fadingParticle2.ColorTint = new Color(255, 255, 255, 127) * 0.75f;
				fadingParticle2.SetBasicInfo(asset, null, Vector2.Zero, Vector2.Zero);
				fadingParticle2.SetTypeInfo(num4, true);
				fadingParticle2.FadeInNormalizedTime = 0.1f;
				fadingParticle2.FadeOutNormalizedTime = 0.1f;
				fadingParticle2.Scale = new Vector2(1f, 1f) * 0.65f;
				fadingParticle2.ScaleVelocity = new Vector2(2f, 2f) / num4;
				fadingParticle2.ScaleAcceleration = -fadingParticle2.ScaleVelocity / num4;
				Vector2 value2 = value - player2.MountedCenter;
				fadingParticle2.LocalPosition = -value2;
				if (num == -1)
				{
					fadingParticle2.LocalPosition = player2.MountedCenter;
				}
				fadingParticle2.Velocity = value2 * 0.5f / num4;
				Main.ParticleSystem_World_OverPlayers.Add(fadingParticle2);
				for (int j = 0; j < 4; j++)
				{
					Dust dust2 = Dust.NewDustDirect(player2.position, player2.width, player2.height, 57, 0f, 0f, 0, default(Color), 1f);
					dust2.velocity *= 3f;
					dust2.noGravity = true;
					dust2.scale = 1f;
					dust2.fadeIn = 1.5f;
					dust2.noLight = true;
				}
			}
		}

		// Token: 0x060030E8 RID: 12520 RVA: 0x005BE150 File Offset: 0x005BC350
		private static void Spawn_StormLightningWindup(ParticleOrchestraSettings settings)
		{
			float num = Main.rand.NextFloat() * 6.2831855f;
			float num2 = (float)Main.rand.Next(1, 3);
			float scaleFactor = 0.7f;
			short num3 = 916;
			Main.instance.LoadProjectile((int)num3);
			Color white = Color.White;
			if (settings.UniqueInfoPiece != 0)
			{
				white.PackedValue = (uint)settings.UniqueInfoPiece;
			}
			white.A = 0;
			for (float num4 = 0f; num4 < 1f; num4 += 1f / num2)
			{
				int num5 = Main.rand.Next(7, 11);
				float f = 6.2831855f * num4 + num + Main.rand.NextFloatDirection() * 0.25f;
				float scaleFactor2 = Main.rand.NextFloat() * 3f + 0.1f;
				Vector2 vector = Main.rand.NextVector2Circular(6f, 6f) * scaleFactor;
				Color colorTint = white;
				RandomizedFrameParticle randomizedFrameParticle = ParticleOrchestrator._poolRandomizedFrame.RequestParticle();
				randomizedFrameParticle.SetBasicInfo(TextureAssets.Projectile[(int)num3], null, Vector2.Zero, vector);
				randomizedFrameParticle.SetTypeInfo(Main.projFrames[(int)num3], 2, (float)num5);
				randomizedFrameParticle.Velocity = f.ToRotationVector2() * scaleFactor2 * new Vector2(1f, 0.5f) + settings.MovementVector * 0.5f;
				randomizedFrameParticle.ColorTint = colorTint;
				randomizedFrameParticle.LocalPosition = settings.PositionInWorld + vector;
				randomizedFrameParticle.Rotation = randomizedFrameParticle.Velocity.ToRotation();
				randomizedFrameParticle.Scale = new Vector2(1.5f, 0.75f) * 1f;
				randomizedFrameParticle.FadeInNormalizedTime = 1f;
				randomizedFrameParticle.FadeOutNormalizedTime = 0.9f;
				randomizedFrameParticle.ScaleVelocity = new Vector2(0.025f);
				if (Main.rand.Next(3) == 0)
				{
					randomizedFrameParticle.LocalPosition += randomizedFrameParticle.Velocity * (float)(num5 - 1);
					randomizedFrameParticle.Velocity *= -0.5f;
				}
				Main.ParticleSystem_World_OverPlayers.Add(randomizedFrameParticle);
				RandomizedFrameParticle randomizedFrameParticle2 = ParticleOrchestrator._poolRandomizedFrame.RequestParticle();
				randomizedFrameParticle2.SetBasicInfo(TextureAssets.Projectile[(int)num3], null, Vector2.Zero, vector);
				randomizedFrameParticle2.SetTypeInfo(Main.projFrames[(int)num3], 2, (float)num5);
				randomizedFrameParticle2.Velocity = randomizedFrameParticle.Velocity;
				randomizedFrameParticle2.ColorTint = new Color(255, 255, 255, 0);
				randomizedFrameParticle2.LocalPosition = randomizedFrameParticle.LocalPosition;
				randomizedFrameParticle2.Rotation = randomizedFrameParticle.Rotation;
				randomizedFrameParticle2.Scale = randomizedFrameParticle.Scale * 0.5f;
				randomizedFrameParticle2.FadeInNormalizedTime = randomizedFrameParticle.FadeInNormalizedTime;
				randomizedFrameParticle2.FadeOutNormalizedTime = randomizedFrameParticle.FadeOutNormalizedTime;
				randomizedFrameParticle2.ScaleVelocity = randomizedFrameParticle.ScaleVelocity * 0.5f;
				Main.ParticleSystem_World_OverPlayers.Add(randomizedFrameParticle2);
			}
		}

		// Token: 0x060030E9 RID: 12521 RVA: 0x005BE464 File Offset: 0x005BC664
		private static void Spawn_StormLightning(ParticleOrchestraSettings settings)
		{
			StormLightningParticle stormLightningParticle = ParticleOrchestrator.StormLightningParticles.RequestParticle();
			Color white = Color.White;
			if (settings.UniqueInfoPiece != 0)
			{
				white.PackedValue = (uint)settings.UniqueInfoPiece;
			}
			int num = 45;
			int seed = (int)settings.MovementVector.X;
			stormLightningParticle.Prepare((uint)seed, settings.PositionInWorld, num, white);
			Main.ParticleSystem_World_OverPlayers.Add(stormLightningParticle);
			settings.PositionInWorld = stormLightningParticle.EndPosition;
			PunchCameraModifier modifier = new PunchCameraModifier(settings.PositionInWorld, (Main.rand.NextFloat() * 6.2831855f).ToRotationVector2() * new Vector2(0.5f, 1f), MathHelper.Lerp(2f, 10f, Main.SceneState.outsideWeatherEffectIntensity), 3f, 45, 1500f, "StormLightning");
			Main.instance.CameraModifiers.Add(modifier);
			Main.NewLightning(true, true);
			SoundEngine.PlaySound(SoundID.InstantThunder, settings.PositionInWorld, 0f, MathHelper.Lerp(0.4f, 1f, Main.SceneState.outsideWeatherEffectIntensity));
			Lighting.AddLight(settings.PositionInWorld, (float)white.R / 255f, (float)white.G / 255f, (float)white.B / 255f);
			int num2 = 3;
			Point point = settings.PositionInWorld.ToTileCoordinates();
			for (int i = point.X - 1; i <= point.X + 1; i++)
			{
				for (int j = point.Y - 1; j <= point.Y + 1; j++)
				{
					Tile tileSafely = Framing.GetTileSafely(i, j);
					if (tileSafely.active())
					{
						int num3 = WorldGen.KillTile_GetTileDustAmount(true, tileSafely);
						for (int k = 0; k < num3; k++)
						{
							Dust dust = Main.dust[WorldGen.KillTile_MakeTileDust(i, j, tileSafely)];
							dust.velocity.Y = dust.velocity.Y - (3f + (float)num2 * 1.5f);
							dust.velocity.Y = dust.velocity.Y * Main.rand.NextFloat();
							dust.velocity.Y = dust.velocity.Y * 0.75f;
							dust.scale += (float)num2 * 0.03f;
						}
						for (int l = 0; l < num3 - 1; l++)
						{
							Dust dust2 = Main.dust[WorldGen.KillTile_MakeTileDust(i, j, tileSafely)];
							dust2.velocity.Y = dust2.velocity.Y - (1f + (float)num2);
							dust2.velocity.Y = dust2.velocity.Y * Main.rand.NextFloat();
							dust2.velocity.Y = dust2.velocity.Y * 0.75f;
						}
						if (tileSafely.type == 5)
						{
							for (int m = 0; m < 6; m++)
							{
								Dust dust3 = Dust.NewDustDirect(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 6, 0f, 0f, 0, default(Color), 1f);
								dust3.velocity *= 2f;
								dust3.fadeIn += Main.rand.NextFloat();
								dust3.noLightEmittence = true;
							}
							for (int n = 0; n < 3; n++)
							{
								Dust dust4 = Dust.NewDustDirect(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 31, 0f, 0f, 0, default(Color), 1f);
								dust4.velocity *= 3f;
								dust4.noGravity = true;
								dust4.fadeIn += 1f + Main.rand.NextFloat();
							}
						}
					}
				}
			}
			ParticleOrchestrator.SpawnLightningExplosionDust(settings.PositionInWorld, white);
			for (int num4 = 0; num4 < 4; num4++)
			{
				Gore.NewGoreDirect(settings.PositionInWorld + Utils.RandomVector2(Main.rand, -15f, 15f) * new Vector2(0.5f, 1f) + new Vector2(-20f, 0f), Vector2.Zero, 61 + Main.rand.Next(3), 1f).velocity *= 0.5f;
			}
			Vector2 vector = new Vector2(1.1f);
			Vector2 vector2 = new Vector2(-0.9f);
			short num5 = 1091;
			Main.instance.LoadProjectile((int)num5);
			FadingParticle fadingParticle = ParticleOrchestrator._poolFading.RequestParticle();
			fadingParticle.SetBasicInfo(TextureAssets.Projectile[(int)num5], null, Vector2.Zero, settings.PositionInWorld);
			fadingParticle.SetTypeInfo((float)num, true);
			fadingParticle.ColorTint = white;
			fadingParticle.ColorTint.A = 0;
			fadingParticle.FadeInNormalizedTime = 0.01f;
			fadingParticle.FadeOutNormalizedTime = 0.6f;
			fadingParticle.Scale = vector;
			fadingParticle.ScaleVelocity = vector2 / (float)num;
			fadingParticle.ScaleAcceleration = fadingParticle.ScaleVelocity * (-1f / (float)num);
			Main.ParticleSystem_World_OverPlayers.Add(fadingParticle);
			FadingParticle fadingParticle2 = ParticleOrchestrator._poolFading.RequestParticle();
			fadingParticle2.SetBasicInfo(TextureAssets.Projectile[(int)num5], null, Vector2.Zero, settings.PositionInWorld);
			fadingParticle2.SetTypeInfo((float)num, true);
			fadingParticle2.ColorTint = new Color(255, 255, 255);
			fadingParticle2.FadeInNormalizedTime = 0.01f;
			fadingParticle2.FadeOutNormalizedTime = 0.6f;
			fadingParticle2.Scale = vector * 0.7f;
			fadingParticle2.ScaleVelocity = vector2 * 0.7f / (float)num;
			fadingParticle2.ScaleAcceleration = fadingParticle2.ScaleVelocity * (-1f / (float)num);
			Main.ParticleSystem_World_OverPlayers.Add(fadingParticle2);
			float num6 = 12f;
			num5 = 916;
			Main.instance.LoadProjectile((int)num5);
			for (float num7 = 0f; num7 < 1f; num7 += 1f / num6)
			{
				int num8 = Main.rand.Next(14, 22);
				Vector2 vector3 = Main.rand.NextVector2Circular(6f, 6f) * 0.7f;
				Color colorTint = white;
				RandomizedFrameParticle randomizedFrameParticle = ParticleOrchestrator._poolRandomizedFrame.RequestParticle();
				randomizedFrameParticle.SetBasicInfo(TextureAssets.Projectile[(int)num5], null, Vector2.Zero, vector3);
				randomizedFrameParticle.SetTypeInfo(Main.projFrames[(int)num5], 3, (float)num8);
				randomizedFrameParticle.Velocity = Main.rand.NextVector2Circular(6f, 3f);
				randomizedFrameParticle.ColorTint = colorTint;
				randomizedFrameParticle.LocalPosition = settings.PositionInWorld + vector3;
				randomizedFrameParticle.Rotation = randomizedFrameParticle.Velocity.ToRotation();
				randomizedFrameParticle.Scale = new Vector2(1.5f, 0.75f) * 0.85f;
				randomizedFrameParticle.FadeInNormalizedTime = 0.01f;
				randomizedFrameParticle.FadeOutNormalizedTime = 0f;
				randomizedFrameParticle.ScaleVelocity = new Vector2(0.025f);
				Main.ParticleSystem_World_OverPlayers.Add(randomizedFrameParticle);
				RandomizedFrameParticle randomizedFrameParticle2 = ParticleOrchestrator._poolRandomizedFrame.RequestParticle();
				randomizedFrameParticle2.SetBasicInfo(TextureAssets.Projectile[(int)num5], null, Vector2.Zero, vector3);
				randomizedFrameParticle2.SetTypeInfo(Main.projFrames[(int)num5], 3, (float)num8);
				randomizedFrameParticle2.Velocity = randomizedFrameParticle.Velocity;
				randomizedFrameParticle2.ColorTint = new Color(255, 255, 255, 0);
				randomizedFrameParticle2.LocalPosition = randomizedFrameParticle.LocalPosition;
				randomizedFrameParticle2.Rotation = randomizedFrameParticle.Rotation;
				randomizedFrameParticle2.Scale = randomizedFrameParticle.Scale * 0.5f;
				randomizedFrameParticle2.FadeInNormalizedTime = randomizedFrameParticle.FadeInNormalizedTime;
				randomizedFrameParticle2.FadeOutNormalizedTime = randomizedFrameParticle.FadeOutNormalizedTime;
				randomizedFrameParticle2.ScaleVelocity = randomizedFrameParticle.ScaleVelocity * 0.5f;
				Main.ParticleSystem_World_OverPlayers.Add(randomizedFrameParticle2);
			}
		}

		// Token: 0x060030EA RID: 12522 RVA: 0x005BEC70 File Offset: 0x005BCE70
		public static void SpawnLightningExplosionDust(Vector2 position, Color customColor)
		{
			for (int i = 0; i < 12; i++)
			{
				Dust dust = Dust.NewDustPerfect(position, 226, null, 0, default(Color), 1f);
				dust.HackFrame(278);
				dust.color = customColor;
				dust.customData = dust.color;
				dust.velocity *= 1f + Main.rand.NextFloat() * 2.5f;
				dust.velocity += new Vector2(0f, -2f);
				dust.fadeIn = 0f;
				dust.scale = 0.4f + Main.rand.NextFloat() * 0.5f;
				Dust dust2 = dust;
				dust2.velocity.X = dust2.velocity.X * 2f;
				dust.position -= dust.velocity * 3f;
			}
			for (int j = 0; j < 6; j++)
			{
				Dust dust3 = Dust.NewDustPerfect(position, 226, null, 0, default(Color), 1f);
				dust3.HackFrame(278);
				dust3.color = customColor;
				dust3.customData = dust3.color;
				dust3.velocity *= 1f + Main.rand.NextFloat() * 2.5f;
				dust3.velocity += new Vector2(0f, -2f);
				dust3.fadeIn = 1.7f + (float)Main.rand.Next() * 1f;
				dust3.noGravity = true;
				dust3.scale = 0.4f + Main.rand.NextFloat() * 0.5f;
				Dust dust4 = dust3;
				dust4.velocity.X = dust4.velocity.X * 2f;
				dust3.velocity *= 0.6f;
				dust3.position -= dust3.velocity * 3f;
			}
			for (int k = 0; k < 6; k++)
			{
				Dust dust5 = Dust.NewDustPerfect(position + Main.rand.NextVector2Circular(20f, 20f), 226, null, 0, default(Color), 1f);
				dust5.HackFrame(278);
				dust5.color = customColor;
				dust5.customData = dust5.color;
				dust5.velocity = new Vector2(0f, -1f).RotatedByRandom(0.7853981852531433);
				dust5.velocity *= 3f + Main.rand.NextFloat() * 6.5f;
				dust5.fadeIn = 0f;
				dust5.scale = 0.4f + Main.rand.NextFloat() * 0.5f;
				dust5.noGravity = true;
				dust5.position -= dust5.velocity * 2f;
			}
			for (int l = 0; l < 6; l++)
			{
				Dust dust6 = Dust.NewDustPerfect(position, 306, new Vector2?(new Vector2(0f, -4f).RotatedByRandom(1.5707963705062866)), 0, default(Color), 1f);
				dust6.color = new Color((int)customColor.R, (int)customColor.G, (int)customColor.B, 0);
				dust6.scale = 2.8f;
				dust6.fadeIn = 0f;
				dust6.noGravity = (Main.rand.Next(3) != 0);
				Dust dust7 = Dust.CloneDust(dust6);
				dust7.color = new Color(255, 255, 255, 0);
				dust7.scale = 1.9f;
			}
			for (int m = 0; m < 6; m++)
			{
				Dust dust8 = Dust.NewDustPerfect(position, 306, new Vector2?(new Vector2(0f, -2f).RotatedByRandom(1.5707963705062866)), 0, default(Color), 1f);
				dust8.color = new Color((int)customColor.R, (int)customColor.G, (int)customColor.B, 0);
				dust8.scale = 2.8f;
				dust8.fadeIn = 0f;
				dust8.noGravity = (Main.rand.Next(3) != 0);
				Dust dust9 = Dust.CloneDust(dust8);
				dust9.color = new Color(255, 255, 255, 0);
				dust9.scale = 1.9f;
			}
			for (int n = 0; n < 3; n++)
			{
				Dust dust10 = Dust.NewDustPerfect(position + Main.rand.NextVector2Circular(20f, 20f), 43, new Vector2?(Main.rand.NextVector2Circular(6f, 6f) * Main.rand.NextFloat()), 26, Color.Lerp(customColor, Color.White, Main.rand.NextFloat()), 1f + Main.rand.NextFloat() * 1.4f);
				dust10.fadeIn = 1.5f;
				if (dust10.velocity.Y > 0f && Main.rand.Next(2) == 0)
				{
					Dust dust11 = dust10;
					dust11.velocity.Y = dust11.velocity.Y * -1f;
				}
				dust10.noGravity = true;
			}
		}

		// Token: 0x060030EB RID: 12523 RVA: 0x005BF238 File Offset: 0x005BD438
		private static void Spawn_NatureFly(ParticleOrchestraSettings settings)
		{
			int num = ParticleOrchestrator._natureFlies.CountParticlesInUse();
			if (num > 50 && Main.rand.NextFloat() >= Utils.Remap((float)num, 50f, 400f, 0.5f, 0f, true))
			{
				return;
			}
			LittleFlyingCritterParticle littleFlyingCritterParticle = ParticleOrchestrator._natureFlies.RequestParticle();
			Color overrideColor = Main.hslToRgb(Main.rand.NextFloat() * 0.14f, 0.1f + 0.6f * Main.rand.NextFloat(), 0.3f + 0.3f * Main.rand.NextFloat(), byte.MaxValue);
			overrideColor.A = 170;
			littleFlyingCritterParticle.Prepare(LittleFlyingCritterParticle.FlyType.RegularFly, settings.PositionInWorld, Main.rand.Next(180, 301), overrideColor, 1);
			Main.ParticleSystem_World_OverPlayers.Add(littleFlyingCritterParticle);
		}

		// Token: 0x060030EC RID: 12524 RVA: 0x005BF30C File Offset: 0x005BD50C
		private static void Spawn_LakeSparkle(ParticleOrchestraSettings settings)
		{
			FadingParticle fadingParticle = ParticleOrchestrator._poolFading.RequestParticle();
			Vector2 value = Vector2.UnitY * (-0.1f - 0.4f * Main.rand.NextFloat());
			value = settings.MovementVector / 60f;
			fadingParticle.SetBasicInfo(TextureAssets.Star[0], null, Vector2.Zero, settings.PositionInWorld);
			float num = 50f + 90f * Main.rand.NextFloat();
			fadingParticle.SetTypeInfo(num, true);
			fadingParticle.AccelerationPerFrame = value / num;
			fadingParticle.ColorTint = new Color(255, 255, 255, 0) * Main.rand.NextFloat();
			fadingParticle.FadeInNormalizedTime = 0.45f;
			fadingParticle.FadeOutNormalizedTime = 0.45f;
			fadingParticle.Rotation = Main.rand.NextFloat() * 6.2831855f;
			fadingParticle.RotationVelocity = Main.rand.NextFloatDirection() * 6.2831855f / 32f;
			fadingParticle.RotationVelocity *= Utils.Clamp<float>(settings.MovementVector.Length() / 32f, 0f, 1f);
			fadingParticle.Scale = Vector2.One * (0.5f + 0.5f * Main.rand.NextFloat());
			Main.ParticleSystem_World_BehindPlayers.Add(fadingParticle);
		}

		// Token: 0x060030ED RID: 12525 RVA: 0x005BF474 File Offset: 0x005BD674
		private static void Spawn_FakeFish(ParticleOrchestraSettings settings, bool jumping = false)
		{
			FakeFishParticle fakeFishParticle = ParticleOrchestrator._fakeFish.RequestParticle();
			Vector2 movementVector = settings.MovementVector;
			int uniqueInfoPiece = settings.UniqueInfoPiece;
			int lifeTimeTotal = Main.rand.Next(240, 421);
			fakeFishParticle.Position = settings.PositionInWorld;
			fakeFishParticle.Velocity = Vector2.Zero;
			fakeFishParticle.Prepare(uniqueInfoPiece, lifeTimeTotal, movementVector);
			if (jumping)
			{
				fakeFishParticle.Velocity = settings.MovementVector;
				fakeFishParticle.TryJumping();
			}
			Main.ParticleSystem_World_BehindPlayers.Add(fakeFishParticle);
		}

		// Token: 0x060030EE RID: 12526 RVA: 0x005BF4F0 File Offset: 0x005BD6F0
		public static void MagnetFakeFish(Projectile proj, int searchedItemType)
		{
			using (IEnumerator<FakeFishParticle> enumerator = (from x in Main.ParticleSystem_World_BehindPlayers.Particles.OfType<FakeFishParticle>()
			orderby x.Position.Distance(proj.Center)
			select x).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.TryToMagnetizeTo(proj, searchedItemType))
					{
						return;
					}
				}
			}
			using (IEnumerator<FakeFishParticle> enumerator = (from x in Main.ParticleSystem_World_BehindPlayers.Particles.OfType<FakeFishParticle>()
			orderby x.Position.Distance(proj.Center)
			select x).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.TryToMagnetizeTo(proj, -1))
					{
						break;
					}
				}
			}
		}

		// Token: 0x060030EF RID: 12527 RVA: 0x005BF5CC File Offset: 0x005BD7CC
		public static void PingFakeFish(Projectile proj, int searchedItemType)
		{
			foreach (FakeFishParticle fakeFishParticle in from x in Main.ParticleSystem_World_BehindPlayers.Particles.OfType<FakeFishParticle>()
			orderby x.Position.Distance(proj.Center)
			select x)
			{
				fakeFishParticle.TryToBePinged(proj, searchedItemType);
			}
		}

		// Token: 0x060030F0 RID: 12528 RVA: 0x005BF648 File Offset: 0x005BD848
		public static void PushAwayFakeFish(Projectile proj, int searchedItemType)
		{
			foreach (FakeFishParticle fakeFishParticle in from x in Main.ParticleSystem_World_BehindPlayers.Particles.OfType<FakeFishParticle>()
			orderby x.Position.Distance(proj.Center)
			select x)
			{
				fakeFishParticle.TryToPushAway(proj, searchedItemType);
			}
		}

		// Token: 0x060030F1 RID: 12529 RVA: 0x005BF6C4 File Offset: 0x005BD8C4
		public static void RepelAt(Vector2 position, float radius, bool wet)
		{
			ParticleRepelDetails particleRepelDetails = new ParticleRepelDetails
			{
				SourcePosition = position,
				Radius = radius,
				IsInWater = wet
			};
			foreach (IParticle particle in Main.ParticleSystem_World_BehindPlayers.Particles)
			{
				IParticleRepel particleRepel = particle as IParticleRepel;
				if (particleRepel != null)
				{
					particleRepel.BeRepelled(ref particleRepelDetails);
				}
			}
			foreach (IParticle particle2 in Main.ParticleSystem_World_OverPlayers.Particles)
			{
				IParticleRepel particleRepel2 = particle2 as IParticleRepel;
				if (particleRepel2 != null)
				{
					particleRepel2.BeRepelled(ref particleRepelDetails);
				}
			}
		}

		// Token: 0x060030F2 RID: 12530 RVA: 0x005BF798 File Offset: 0x005BD998
		private static void Spawn_RainbowBoulder4(ParticleOrchestraSettings settings, bool pet = false)
		{
			if (!pet)
			{
				SoundEngine.PlaySound(SoundID.RainbowBoulder, (int)settings.PositionInWorld.X, (int)settings.PositionInWorld.Y, 0f, 1f);
			}
			int num = 14;
			int num2 = 20;
			if (pet)
			{
				num = 8;
				num2 = 10;
			}
			for (int i = 0; i < num; i++)
			{
				int num3 = Dust.NewDust(settings.PositionInWorld, 0, 0, 66, 0f, 0f, 100, Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.5f, byte.MaxValue), pet ? 1.2f : 1.7f);
				Main.dust[num3].noGravity = true;
				if (pet)
				{
					Main.dust[num3].velocity *= 2f;
					Main.dust[num3].position = settings.PositionInWorld + Main.rand.NextVector2Circular(30f, 30f);
				}
				else
				{
					Main.dust[num3].velocity *= 3f;
					Main.dust[num3].position = settings.PositionInWorld + Main.rand.NextVector2Circular(30f, 30f);
				}
			}
			int num4 = 20;
			if (pet)
			{
				num4 = 12;
			}
			Rectangle rect = Utils.CenteredRectangle(settings.PositionInWorld, new Vector2((float)num4, (float)num4));
			float num5 = settings.MovementVector.ToRotation() + 1.5707964f;
			for (float num6 = 0f; num6 < (float)num2; num6 += 1f)
			{
				PrettySparkleParticle prettySparkleParticle = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
				int num7 = Main.rand.Next(20, 40);
				prettySparkleParticle.ColorTint = Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.5f, 0);
				prettySparkleParticle.LocalPosition = Main.rand.NextVector2FromRectangle(rect);
				prettySparkleParticle.Rotation = 1.5707964f + num5;
				prettySparkleParticle.Scale = new Vector2(1f + Main.rand.NextFloat() * 1f, 0.7f + Main.rand.NextFloat() * 0.7f);
				prettySparkleParticle.Velocity = new Vector2(0f, -1f).RotatedBy((double)num5, default(Vector2));
				prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
				prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
				prettySparkleParticle.TimeToLive = (float)num7;
				prettySparkleParticle.FadeOutEnd = (float)num7;
				prettySparkleParticle.FadeInEnd = (float)(num7 / 2);
				prettySparkleParticle.FadeOutStart = (float)(num7 / 2);
				prettySparkleParticle.AdditiveAmount = 0.35f;
				prettySparkleParticle.DrawVerticalAxis = false;
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
				PrettySparkleParticle prettySparkleParticle2 = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
				prettySparkleParticle2.ColorTint = new Color(255, 255, 255, 0);
				prettySparkleParticle2.LocalPosition = Main.rand.NextVector2FromRectangle(rect);
				prettySparkleParticle2.Rotation = 1.5707964f + num5;
				prettySparkleParticle2.Scale = prettySparkleParticle.Scale * 0.5f;
				prettySparkleParticle2.Velocity = new Vector2(0f, 1f).RotatedBy((double)num5, default(Vector2));
				prettySparkleParticle2.FadeInNormalizedTime = 5E-06f;
				prettySparkleParticle2.FadeOutNormalizedTime = 0.95f;
				prettySparkleParticle2.TimeToLive = (float)num7;
				prettySparkleParticle2.FadeOutEnd = (float)num7;
				prettySparkleParticle2.FadeInEnd = (float)(num7 / 2);
				prettySparkleParticle2.FadeOutStart = (float)(num7 / 2);
				prettySparkleParticle2.AdditiveAmount = 1f;
				prettySparkleParticle2.DrawVerticalAxis = false;
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle2);
			}
		}

		// Token: 0x060030F3 RID: 12531 RVA: 0x005BFB48 File Offset: 0x005BDD48
		private static void Spawn_RainbowBoulder1(ParticleOrchestraSettings settings)
		{
			float num = Main.rand.NextFloat() * 6.2831855f;
			float num2 = (float)(6 + Main.rand.Next(7));
			float num3 = Main.rand.NextFloat();
			Main.rand.Next(2);
			for (float num4 = 0f; num4 < 1f; num4 += 1f / num2)
			{
				Vector2 zero = Vector2.Zero;
				Vector2 vector = new Vector2(Main.rand.NextFloat() * 0.3f + 1f);
				float f = num + Main.rand.NextFloat() * 6.2831855f;
				f = num + 6.2831855f * num4;
				float rotation = 1.5707964f;
				Vector2 vector2 = 0.8f * vector * (0.9f + 0.1f * Main.rand.NextFloat());
				float divider = (float)(Main.rand.Next(20) + 180);
				Vector2 value = Main.rand.NextVector2Circular(16f, 16f) * vector;
				vector2 = Main.rand.NextVector2CircularEdge(1.5f, 1.5f);
				vector2.X = 0f;
				if (vector2.Y > 0f)
				{
					vector2.Y *= -1f;
				}
				PrettySparkleParticle prettySparkleParticle = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
				prettySparkleParticle.Velocity = vector2 + zero;
				prettySparkleParticle.AccelerationPerFrame = f.ToRotationVector2() * -(vector2 / divider) - zero * 1f / divider;
				prettySparkleParticle.ColorTint = Main.hslToRgb((num3 + Main.rand.NextFloat() * 0.33f) % 1f, 1f, 0.4f + Main.rand.NextFloat() * 0.25f, byte.MaxValue);
				prettySparkleParticle.ColorTint = Main.hslToRgb((num4 + num3) % 1f, 1f, 0.5f, byte.MaxValue);
				prettySparkleParticle.ColorTint.A = byte.MaxValue;
				prettySparkleParticle.LocalPosition = settings.PositionInWorld + value;
				prettySparkleParticle.Rotation = rotation;
				prettySparkleParticle.Scale = vector;
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
				prettySparkleParticle = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
				prettySparkleParticle.Velocity = vector2 + zero;
				prettySparkleParticle.AccelerationPerFrame = f.ToRotationVector2() * -(vector2 / divider) - zero * 1f / divider;
				prettySparkleParticle.ColorTint = new Color(255, 255, 255, 0);
				prettySparkleParticle.LocalPosition = settings.PositionInWorld + value;
				prettySparkleParticle.Rotation = rotation;
				prettySparkleParticle.Scale = vector * 0.3f;
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
			}
			for (int i = 0; i < 21; i++)
			{
				Dust dust = Dust.NewDustDirect(settings.PositionInWorld, 16, 16, 66, 0f, 0f, 100, Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.5f, byte.MaxValue), 1.7f);
				dust.noGravity = true;
				dust.velocity = Main.rand.NextVector2CircularEdge(1f, 1f) * (8f + 2f * Main.rand.NextFloat());
				dust.fadeIn = 1f + Main.rand.NextFloat();
				dust.velocity.X = 0f;
				if (dust.velocity.Y > 0f)
				{
					Dust dust2 = dust;
					dust2.velocity.Y = dust2.velocity.Y * -1f;
				}
				dust.position -= dust.velocity * 5f;
			}
		}

		// Token: 0x060030F4 RID: 12532 RVA: 0x005BFF48 File Offset: 0x005BE148
		private static void Spawn_RainbowBoulder2(ParticleOrchestraSettings settings)
		{
			FadingParticle fadingParticle = ParticleOrchestrator._poolFading.RequestParticle();
			fadingParticle.SetBasicInfo(TextureAssets.Star[0], null, settings.MovementVector, settings.PositionInWorld);
			float num = 25f;
			fadingParticle.SetTypeInfo(num, true);
			fadingParticle.AccelerationPerFrame = settings.MovementVector / num;
			fadingParticle.ColorTint = Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.5f, byte.MaxValue);
			fadingParticle.FadeInNormalizedTime = 0.5f;
			fadingParticle.FadeOutNormalizedTime = 0.5f;
			fadingParticle.Rotation = Main.rand.NextFloat() * 6.2831855f;
			fadingParticle.Scale = Vector2.One * (0.5f + 0.5f * Main.rand.NextFloat());
			Main.ParticleSystem_World_OverPlayers.Add(fadingParticle);
			FadingParticle fadingParticle2 = fadingParticle;
			fadingParticle = ParticleOrchestrator._poolFading.RequestParticle();
			fadingParticle.SetBasicInfo(TextureAssets.Star[0], null, settings.MovementVector, settings.PositionInWorld);
			fadingParticle.SetTypeInfo(num, true);
			fadingParticle.AccelerationPerFrame = settings.MovementVector / num;
			fadingParticle.ColorTint = Main.hslToRgb(Main.rand.NextFloat(), 1f, 1f, byte.MaxValue);
			fadingParticle.ColorTint.A = 30;
			fadingParticle.FadeInNormalizedTime = 0.5f;
			fadingParticle.FadeOutNormalizedTime = 0.5f;
			fadingParticle.Rotation = fadingParticle2.Rotation;
			fadingParticle.Scale = fadingParticle2.Scale * 0.5f;
			Main.ParticleSystem_World_OverPlayers.Add(fadingParticle);
		}

		// Token: 0x060030F5 RID: 12533 RVA: 0x005C00E0 File Offset: 0x005BE2E0
		private static void Spawn_RainbowBoulder3(ParticleOrchestraSettings settings)
		{
			float num = Main.rand.NextFloat() * 6.2831855f;
			float num2 = (float)(3 + Main.rand.Next(7));
			float num3 = Main.rand.NextFloat();
			float num4 = 1.5707964f * (float)(Main.rand.Next(2) * 2 - 1);
			for (float num5 = 0f; num5 < 1f; num5 += 1f / num2)
			{
				Vector2 vector = settings.MovementVector * Main.rand.NextFloatDirection() * 0.15f;
				Vector2 vector2 = new Vector2(Main.rand.NextFloat() * 0.2f + 0.3f);
				float f = num + Main.rand.NextFloat() * 6.2831855f;
				f = num + 6.2831855f * num5;
				float rotation = 1.5707964f;
				Vector2 vector3 = 0.8f * vector2 * (0.3f + 1.5f * Main.rand.NextFloat());
				float divider = (float)(Main.rand.Next(20) + 70);
				Vector2 value = (6.2831855f * num5 + num).ToRotationVector2() * (14f - num2 + Main.rand.NextFloat() * 3f);
				PrettySparkleParticle prettySparkleParticle = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
				prettySparkleParticle.Velocity = f.ToRotationVector2() * vector3 + vector;
				prettySparkleParticle.AccelerationPerFrame = f.ToRotationVector2() * -(vector3 / divider) - vector * 1f / divider;
				prettySparkleParticle.ColorTint = Main.hslToRgb((num3 + Main.rand.NextFloat() * 0.33f) % 1f, 1f, 0.4f + Main.rand.NextFloat() * 0.25f, byte.MaxValue);
				prettySparkleParticle.ColorTint = Main.hslToRgb((num5 + num3) % 1f, 1f, 0.5f, byte.MaxValue);
				prettySparkleParticle.ColorTint.A = byte.MaxValue;
				prettySparkleParticle.LocalPosition = settings.PositionInWorld + value;
				prettySparkleParticle.Rotation = rotation;
				prettySparkleParticle.Scale = vector2;
				prettySparkleParticle.LocalPosition += prettySparkleParticle.Velocity * 4f;
				prettySparkleParticle.Velocity = prettySparkleParticle.Velocity.RotatedBy((double)num4, default(Vector2));
				prettySparkleParticle.AccelerationPerFrame = prettySparkleParticle.AccelerationPerFrame.RotatedBy((double)num4, default(Vector2));
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
				prettySparkleParticle = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
				prettySparkleParticle.Velocity = f.ToRotationVector2() * vector3 + vector;
				prettySparkleParticle.AccelerationPerFrame = f.ToRotationVector2() * -(vector3 / divider) - vector * 1f / divider;
				prettySparkleParticle.ColorTint = new Color(255, 255, 255, 0);
				prettySparkleParticle.LocalPosition = settings.PositionInWorld + value;
				prettySparkleParticle.Rotation = rotation;
				prettySparkleParticle.Scale = vector2 * 0.3f;
				prettySparkleParticle.LocalPosition += prettySparkleParticle.Velocity * 4f;
				prettySparkleParticle.Velocity = prettySparkleParticle.Velocity.RotatedBy((double)num4, default(Vector2));
				prettySparkleParticle.AccelerationPerFrame = prettySparkleParticle.AccelerationPerFrame.RotatedBy((double)num4, default(Vector2));
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
			}
			for (int i = 0; i < 4; i++)
			{
				Color newColor = Main.hslToRgb((num3 + Main.rand.NextFloat() * 0.12f) % 1f, 1f, 0.4f + Main.rand.NextFloat() * 0.25f, byte.MaxValue);
				int num6 = Dust.NewDust(settings.PositionInWorld, 0, 0, 267, 0f, 0f, 0, newColor, 1f);
				Main.dust[num6].velocity = Main.rand.NextVector2Circular(1f, 1f);
				Main.dust[num6].velocity += settings.MovementVector * Main.rand.NextFloatDirection() * 0.5f;
				Main.dust[num6].noGravity = true;
				Main.dust[num6].scale = 0.6f + Main.rand.NextFloat() * 0.9f;
				Main.dust[num6].fadeIn = 0.7f + Main.rand.NextFloat() * 0.8f;
				Main.dust[num6].noLight = true;
				Main.dust[num6].noLightEmittence = true;
				if (num6 != 6000)
				{
					Dust dust = Dust.CloneDust(num6);
					dust.scale /= 2f;
					dust.fadeIn *= 0.75f;
					dust.color = new Color(255, 255, 255, 255);
					dust.noLight = true;
					dust.noLightEmittence = true;
				}
			}
		}

		// Token: 0x060030F6 RID: 12534 RVA: 0x005C064C File Offset: 0x005BE84C
		private static void Spawn_MoonLordWhip(ParticleOrchestraSettings settings)
		{
			float num = 30f;
			float f = 6.2831855f * Main.rand.NextFloat();
			f = settings.MovementVector.SafeNormalize(Vector2.Zero).ToRotation();
			for (float num2 = 0f; num2 < 1f; num2 += 1f)
			{
				PrettySparkleParticle prettySparkleParticle = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
				Vector2 vector = f.ToRotationVector2() * 4f;
				prettySparkleParticle.ColorTint = new Color(0.3f, 0.6f, 0.7f, 0.5f);
				prettySparkleParticle.LocalPosition = settings.PositionInWorld;
				prettySparkleParticle.Rotation = vector.ToRotation();
				prettySparkleParticle.Scale = new Vector2(6f, 0.75f) * 1.1f;
				prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
				prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
				prettySparkleParticle.TimeToLive = num;
				prettySparkleParticle.FadeOutEnd = num;
				prettySparkleParticle.FadeInEnd = num / 2f;
				prettySparkleParticle.FadeOutStart = num / 2f;
				prettySparkleParticle.AdditiveAmount = 0.35f;
				prettySparkleParticle.LocalPosition -= vector * num * 0.25f;
				prettySparkleParticle.Velocity = vector;
				prettySparkleParticle.DrawVerticalAxis = false;
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
			}
			for (float num3 = 0f; num3 < 1f; num3 += 1f)
			{
				PrettySparkleParticle prettySparkleParticle2 = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
				Vector2 vector2 = f.ToRotationVector2() * 4f;
				prettySparkleParticle2.ColorTint = new Color(0.2f, 1f, 0.4f, 1f);
				prettySparkleParticle2.LocalPosition = settings.PositionInWorld;
				prettySparkleParticle2.Rotation = vector2.ToRotation();
				prettySparkleParticle2.Scale = new Vector2(6f, 0.75f) * 0.7f;
				prettySparkleParticle2.FadeInNormalizedTime = 5E-06f;
				prettySparkleParticle2.FadeOutNormalizedTime = 0.95f;
				prettySparkleParticle2.TimeToLive = num;
				prettySparkleParticle2.FadeOutEnd = num;
				prettySparkleParticle2.FadeInEnd = num / 2f;
				prettySparkleParticle2.FadeOutStart = num / 2f;
				prettySparkleParticle2.LocalPosition -= vector2 * num * 0.25f;
				prettySparkleParticle2.Velocity = vector2;
				prettySparkleParticle2.DrawVerticalAxis = false;
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle2);
				for (int i = 0; i < 2; i++)
				{
					Dust dust = Dust.NewDustPerfect(settings.PositionInWorld, 229, new Vector2?(vector2.RotatedBy((double)(Main.rand.NextFloatDirection() * 6.2831855f * 0.025f), default(Vector2)) * Main.rand.NextFloat()), 0, default(Color), 1f);
					dust.noGravity = true;
					dust.scale = 1.2f;
					dust.position += dust.velocity * 4f;
					dust = Dust.NewDustPerfect(settings.PositionInWorld, 229, new Vector2?(-vector2.RotatedBy((double)(Main.rand.NextFloatDirection() * 6.2831855f * 0.025f), default(Vector2)) * Main.rand.NextFloat()), 0, default(Color), 1f);
					dust.noGravity = true;
					dust.scale = 1.2f;
					dust.position += dust.velocity * 4f;
				}
			}
		}

		// Token: 0x060030F7 RID: 12535 RVA: 0x005C0A04 File Offset: 0x005BEC04
		private static void Spawn_DeadCellsBarrelExplosion(ParticleOrchestraSettings settings)
		{
			Asset<Texture2D> textureAsset = TextureAssets.Extra[269];
			Rectangle value = new Rectangle(0, 0, 100, 100);
			Rectangle value2 = new Rectangle(0, 101, 100, 100);
			Rectangle value3 = new Rectangle(0, 202, 100, 100);
			Color colorTint = new Color(52, 208, 254, 20);
			Color color = new Color(52, 208, 254, 20);
			float num = 25f;
			FadingParticle fadingParticle = ParticleOrchestrator._poolFading.RequestParticle();
			fadingParticle.SetBasicInfo(textureAsset, new Rectangle?(value), Vector2.Zero, settings.PositionInWorld);
			fadingParticle.SetTypeInfo(num, true);
			fadingParticle.ColorTint = color;
			fadingParticle.Rotation = Main.rand.NextFloat() * 6.2831855f;
			fadingParticle.RotationVelocity = 6.2831855f * (1f / num) * 1f * Main.rand.NextFloatDirection();
			fadingParticle.RotationAcceleration = -fadingParticle.RotationVelocity * (1f / num);
			fadingParticle.FadeInNormalizedTime = 0.01f;
			fadingParticle.FadeOutNormalizedTime = 0.1f;
			fadingParticle.Scale = Vector2.One * 1f;
			fadingParticle.ScaleVelocity = Vector2.One * (2f / num);
			fadingParticle.ScaleAcceleration = fadingParticle.ScaleVelocity * (-1f / num);
			Main.ParticleSystem_World_OverPlayers.Add(fadingParticle);
			FadingParticle fadingParticle2 = ParticleOrchestrator._poolFading.RequestParticle();
			fadingParticle2.SetBasicInfo(textureAsset, new Rectangle?(value2), Vector2.Zero, settings.PositionInWorld);
			fadingParticle2.SetTypeInfo(num * 0.8f, true);
			fadingParticle2.ColorTint = color * 0.25f;
			fadingParticle2.FadeInNormalizedTime = 0.1f;
			fadingParticle2.FadeOutNormalizedTime = 0.1f;
			fadingParticle2.Scale = Vector2.One * 0.5f;
			fadingParticle2.ScaleVelocity = Vector2.One * 8.5f * (1f / num);
			fadingParticle2.ScaleAcceleration = fadingParticle2.ScaleVelocity * (-1f / num);
			Main.ParticleSystem_World_OverPlayers.Add(fadingParticle2);
			FadingParticle fadingParticle3 = ParticleOrchestrator._poolFading.RequestParticle();
			fadingParticle3.SetBasicInfo(textureAsset, new Rectangle?(value2), Vector2.Zero, settings.PositionInWorld);
			fadingParticle3.SetTypeInfo(num, true);
			fadingParticle3.ColorTint = color * 0.3f;
			fadingParticle3.FadeInNormalizedTime = 0.4f;
			fadingParticle3.FadeOutNormalizedTime = 0.5f;
			fadingParticle3.Scale = fadingParticle.Scale;
			fadingParticle3.ScaleVelocity = fadingParticle.ScaleVelocity;
			fadingParticle3.ScaleAcceleration = fadingParticle.ScaleAcceleration;
			Main.ParticleSystem_World_OverPlayers.Add(fadingParticle3);
			FadingParticle fadingParticle4 = ParticleOrchestrator._poolFading.RequestParticle();
			fadingParticle4.SetBasicInfo(textureAsset, new Rectangle?(value3), Vector2.Zero, settings.PositionInWorld);
			fadingParticle4.SetTypeInfo(num, true);
			fadingParticle4.ColorTint = color;
			fadingParticle4.FadeInNormalizedTime = 0.01f;
			fadingParticle4.FadeOutNormalizedTime = 0.7f;
			fadingParticle4.Scale = fadingParticle.Scale * 0.9f;
			fadingParticle4.ScaleVelocity = fadingParticle.ScaleVelocity * 1.5f;
			fadingParticle4.ScaleAcceleration = fadingParticle.ScaleAcceleration * 1.5f;
			Main.ParticleSystem_World_OverPlayers.Add(fadingParticle4);
			for (float num2 = 0f; num2 < 1f; num2 += 0.04f)
			{
				float num3 = 25f;
				float num4 = 6.2831855f * num2;
				FadingParticle fadingParticle5 = ParticleOrchestrator._poolFading.RequestParticle();
				fadingParticle5.SetBasicInfo(TextureAssets.Extra[89], null, num4.ToRotationVector2() * (4f + 7f * Main.rand.NextFloat()), settings.PositionInWorld + num4.ToRotationVector2() * (10f + 90f * Main.rand.NextFloat()));
				fadingParticle5.SetTypeInfo(num3, true);
				fadingParticle5.AccelerationPerFrame = fadingParticle5.Velocity * (-1f / num3);
				fadingParticle5.LocalPosition -= fadingParticle5.Velocity * 4f;
				fadingParticle5.ColorTint = colorTint;
				fadingParticle5.Rotation = num4 + 1.5707964f;
				fadingParticle5.FadeInNormalizedTime = 0.2f;
				fadingParticle5.FadeOutNormalizedTime = 0.3f;
				fadingParticle5.Scale = new Vector2(0.4f, 0.8f) * (0.6f + 0.6f * Main.rand.NextFloat());
				Main.ParticleSystem_World_BehindPlayers.Add(fadingParticle5);
			}
			for (float num5 = 0f; num5 < 1f; num5 += 0.14285715f)
			{
				Dust dust = Dust.NewDustPerfect(settings.PositionInWorld, 267, new Vector2?(Vector2.Zero), 0, new Color(100, 200, 255, 127), 2f);
				dust.noGravity = true;
				dust.velocity = new Vector2(0f, 12f).RotatedBy((double)(6.2831855f * num5), default(Vector2)) * Main.rand.NextFloat();
			}
		}

		// Token: 0x060030F8 RID: 12536 RVA: 0x005C0F5C File Offset: 0x005BF15C
		private static void Spawn_DeadCellsFlint(ParticleOrchestraSettings settings)
		{
			Vector2 positionInWorld = settings.PositionInWorld;
			float num = settings.MovementVector.Y;
			if (num == 0f)
			{
				num = 1f;
			}
			float x = settings.MovementVector.X;
			bool flag = false;
			Point point = (positionInWorld + new Vector2(-x, -160f)).ToTileCoordinates();
			Point point2 = (positionInWorld + new Vector2(x, 160f)).ToTileCoordinates();
			int num2 = 3;
			point.X -= num2;
			point.Y -= num2;
			point2.X += num2;
			point2.Y += num2;
			int num3 = point.X / 2 + point2.X / 2;
			int num4 = (int)x / 2 + 8 * num2;
			float hue = 0.12f;
			float num5 = 2f;
			float num6 = 3f;
			float num7 = 40f;
			int num8 = 3;
			for (int i = point.X; i <= point2.X; i++)
			{
				for (int j = point.Y; j <= point2.Y; j++)
				{
					if (Vector2.Distance(positionInWorld, new Vector2((float)(i * 16), (float)(j * 16))) <= (float)num4)
					{
						Tile tileSafely = Framing.GetTileSafely(i, j);
						if (tileSafely.active() && Main.tileSolid[(int)tileSafely.type] && !Main.tileSolidTop[(int)tileSafely.type] && !Main.tileFrameImportant[(int)tileSafely.type])
						{
							Tile tileSafely2 = Framing.GetTileSafely(i, j - 1);
							if (!tileSafely2.active() || !Main.tileSolid[(int)tileSafely2.type] || Main.tileSolidTop[(int)tileSafely2.type])
							{
								flag = true;
								if (i > point.X + num2 || i < point.X - num2 || j > point.Y + num2 || j < point2.Y - num2)
								{
									int num9 = WorldGen.KillTile_GetTileDustAmount(true, tileSafely);
									for (int k = 0; k < num9; k++)
									{
										Dust dust = Main.dust[WorldGen.KillTile_MakeTileDust(i, j, tileSafely)];
										dust.velocity.Y = dust.velocity.Y - (3f + (float)num8 * 1.5f);
										dust.velocity.Y = dust.velocity.Y * Main.rand.NextFloat();
										dust.velocity.Y = dust.velocity.Y * 0.75f;
										dust.scale += (float)num8 * 0.03f;
									}
									if (num8 >= 2)
									{
										for (int l = 0; l < num9 - 1; l++)
										{
											Dust dust2 = Main.dust[WorldGen.KillTile_MakeTileDust(i, j, tileSafely)];
											dust2.velocity.Y = dust2.velocity.Y - (1f + (float)num8);
											dust2.velocity.Y = dust2.velocity.Y * Main.rand.NextFloat();
											dust2.velocity.Y = dust2.velocity.Y * 0.75f;
										}
									}
									if (num9 > 0 && Main.rand.Next(2) != 0)
									{
										float num10 = (float)Math.Abs(num3 - i) / (num7 / 2f);
										Gore gore = Gore.NewGoreDirect(positionInWorld, Vector2.Zero, 61 + Main.rand.Next(3), 1f - (float)num8 * 0.15f + num10 * 0.5f);
										Gore gore2 = gore;
										gore2.velocity.Y = gore2.velocity.Y - (0.1f + (float)num8 * 0.5f + num10 * (float)num8 * 1f);
										Gore gore3 = gore;
										gore3.velocity.Y = gore3.velocity.Y * Main.rand.NextFloat();
										Gore gore4 = gore;
										gore4.velocity.Y = gore4.velocity.Y * num;
										gore.position = new Vector2((float)(i * 16) + 20f * num, (float)(j * 16));
										PrettySparkleParticle prettySparkleParticle = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
										int num11 = Main.rand.Next(20, 40);
										prettySparkleParticle.ColorTint = Main.hslToRgb(hue, 1f, 0.5f, 0);
										prettySparkleParticle.LocalPosition = gore.position;
										prettySparkleParticle.Rotation = 1.5707964f;
										prettySparkleParticle.Scale = new Vector2(num5 + Main.rand.NextFloat() * num6, 0.7f + Main.rand.NextFloat() * 0.7f);
										prettySparkleParticle.Velocity = new Vector2(0f, -2f * num);
										prettySparkleParticle.FadeInNormalizedTime = 0.1f;
										prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
										prettySparkleParticle.TimeToLive = (float)num11;
										prettySparkleParticle.FadeOutEnd = (float)num11;
										prettySparkleParticle.FadeInEnd = (float)(num11 / 2);
										prettySparkleParticle.FadeOutStart = (float)(num11 / 2);
										prettySparkleParticle.AdditiveAmount = 0.35f;
										prettySparkleParticle.DrawVerticalAxis = false;
										Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
									}
								}
							}
						}
					}
				}
			}
			if (flag)
			{
				SoundEngine.PlaySound(SoundID.DeadCellsFlintRelease, positionInWorld, 0f, 1f);
				for (int m = 0; m < 30; m++)
				{
					Vector2 vector = Main.rand.NextVector2Circular(5f, 3f);
					if (vector.Y > 0f)
					{
						vector.Y *= -1f;
					}
					Dust.NewDustPerfect(positionInWorld + Main.rand.NextVector2Circular(7f, 0f), 228, new Vector2?(vector), 0, default(Color), 0.7f + 0.8f * Main.rand.NextFloat());
				}
				SoundEngine.PlayTrackedSound(SoundID.DD2_MonkStaffGroundImpact, positionInWorld, default(SoundPlayOverrides));
			}
		}

		// Token: 0x060030F9 RID: 12537 RVA: 0x005C14FC File Offset: 0x005BF6FC
		private static void Spawn_MoonLordWhipEye(ParticleOrchestraSettings settings)
		{
			Asset<Texture2D> asset = TextureAssets.Extra[270];
			if (!asset.IsLoaded)
			{
				return;
			}
			float scaleFactor = 1f;
			float num = 45f;
			float f = 6.2831855f * Main.rand.NextFloat();
			FadingParticle fadingParticle = ParticleOrchestrator._poolFading.RequestParticle();
			fadingParticle.SetBasicInfo(asset, null, Vector2.Zero, Vector2.Zero);
			fadingParticle.SetTypeInfo(num, true);
			fadingParticle.ColorTint = new Color(255, 255, 255, 0);
			fadingParticle.LocalPosition = settings.PositionInWorld + f.ToRotationVector2() * scaleFactor * num * 0.5f;
			fadingParticle.Rotation = 0f;
			fadingParticle.FadeInNormalizedTime = 0.01f;
			fadingParticle.FadeOutNormalizedTime = 0.1f;
			fadingParticle.Velocity = settings.MovementVector * 0.05f;
			fadingParticle.AccelerationPerFrame = -(settings.MovementVector * 0.25f) / num;
			fadingParticle.Scale = new Vector2(1f, 1f) * (0.7f + 0.45f * Main.rand.NextFloat());
			Main.ParticleSystem_World_BehindPlayers.Add(fadingParticle);
		}

		// Token: 0x060030FA RID: 12538 RVA: 0x005C1650 File Offset: 0x005BF850
		private static void Spawn_DeadCellsHeadEffect(ParticleOrchestraSettings settings)
		{
			Asset<Texture2D> asset = TextureAssets.Extra[266];
			if (!asset.IsLoaded)
			{
				return;
			}
			Player player = Main.player[(int)settings.IndexOfPlayerWhoInvokedThis];
			if (!player.active)
			{
				return;
			}
			Vector2 vector = settings.MovementVector;
			vector += new Vector2(0f, 0f);
			for (float num = 0f; num < 1f; num += 0.5f)
			{
				float scaleFactor = 0.5f * Utils.Remap(settings.MovementVector.Length(), 0f, 10f, 0.5f, 1f, true);
				float scaleFactor2 = 1f;
				float num2 = 15f;
				float f = 6.2831855f * Main.rand.NextFloat();
				Vector2 value = Main.rand.NextVector2Circular(4f, 4f) * scaleFactor;
				FadingPlayerShaderParticle fadingPlayerShaderParticle = ParticleOrchestrator._poolFadingPlayerShader.RequestParticle();
				fadingPlayerShaderParticle.SetBasicInfo(asset, null, Vector2.Zero, Vector2.Zero);
				fadingPlayerShaderParticle.SetTypeInfo(num2, player, player.cHead, false);
				fadingPlayerShaderParticle.Velocity = vector * 0.5f;
				fadingPlayerShaderParticle.AccelerationPerFrame = fadingPlayerShaderParticle.Velocity * -(-1f / num2) * 0.25f;
				fadingPlayerShaderParticle.LocalPosition = settings.PositionInWorld + f.ToRotationVector2() * scaleFactor2 * scaleFactor * num2 * 0.5f + value + vector * num;
				fadingPlayerShaderParticle.LocalPosition += fadingPlayerShaderParticle.Velocity * -0.5f;
				fadingPlayerShaderParticle.Rotation = vector.ToRotation() + 1.5707964f;
				fadingPlayerShaderParticle.FadeInNormalizedTime = 0.01f;
				fadingPlayerShaderParticle.FadeOutNormalizedTime = 0.1f;
				fadingPlayerShaderParticle.Scale = new Vector2(0.6f, 1.2f) * scaleFactor;
				fadingPlayerShaderParticle.ScaleVelocity = fadingPlayerShaderParticle.Scale * (-1f / num2);
				Main.ParticleSystem_World_BehindPlayers.Add(fadingPlayerShaderParticle);
			}
		}

		// Token: 0x060030FB RID: 12539 RVA: 0x005C187C File Offset: 0x005BFA7C
		private static void Spawn_UFOLaser(ParticleOrchestraSettings settings)
		{
			ParticleOrchestrator.SpawnHelper_SpawnInLine(settings, delegate(Vector2 dustPos, Vector2 velocity)
			{
				ParticleOrchestrator.SpawnHelper_SpawnSingleLineDust(160, dustPos, (Main.rand.Next(2) == 0) ? Color.LimeGreen : Color.CornflowerBlue);
				ParticleOrchestrator.SpawnHelper_SpawnSingleLineDust(160, dustPos - velocity * 0.25f, (Main.rand.Next(2) == 0) ? Color.LimeGreen : Color.CornflowerBlue);
			});
		}

		// Token: 0x060030FC RID: 12540 RVA: 0x005C18A3 File Offset: 0x005BFAA3
		private static void Spawn_MagnetSphereBolt(ParticleOrchestraSettings settings)
		{
			ParticleOrchestrator.SpawnHelper_SpawnInLine(settings, delegate(Vector2 dustPos, Vector2 velocity)
			{
				ParticleOrchestrator.SpawnHelper_SpawnSingleLineDust(160, dustPos, default(Color));
				ParticleOrchestrator.SpawnHelper_SpawnSingleLineDust(160, dustPos - velocity * 0.25f, default(Color));
				ParticleOrchestrator.SpawnHelper_SpawnSingleLineDust(160, dustPos - velocity * 0.5f, default(Color));
				ParticleOrchestrator.SpawnHelper_SpawnSingleLineDust(160, dustPos - velocity * 0.75f, default(Color));
			});
		}

		// Token: 0x060030FD RID: 12541 RVA: 0x005C18CA File Offset: 0x005BFACA
		private static void Spawn_HeatRay(ParticleOrchestraSettings settings)
		{
			ParticleOrchestrator.SpawnHelper_SpawnInLine(settings, delegate(Vector2 dustPos, Vector2 velocity)
			{
				ParticleOrchestrator.SpawnHelper_SpawnSingleLineDust(162, dustPos, default(Color));
				ParticleOrchestrator.SpawnHelper_SpawnSingleLineDust(162, dustPos - velocity * 0.25f, default(Color));
				ParticleOrchestrator.SpawnHelper_SpawnSingleLineDust(162, dustPos - velocity * 0.5f, default(Color));
				ParticleOrchestrator.SpawnHelper_SpawnSingleLineDust(162, dustPos - velocity * 0.75f, default(Color));
			});
		}

		// Token: 0x060030FE RID: 12542 RVA: 0x005C18F4 File Offset: 0x005BFAF4
		private static void Spawn_Shadowbeam(ParticleOrchestraSettings settings, bool hostile = true)
		{
			float velocityScalar = 0.25f;
			if (hostile)
			{
				velocityScalar = 0.03334f;
			}
			ParticleOrchestrator.SpawnHelper_SpawnInLine(settings, delegate(Vector2 dustPos, Vector2 velocity)
			{
				ParticleOrchestrator.SpawnHelper_SpawnSingleLineDust(173, dustPos, default(Color));
				ParticleOrchestrator.SpawnHelper_SpawnSingleLineDust(173, dustPos - velocity * velocityScalar, default(Color));
				ParticleOrchestrator.SpawnHelper_SpawnSingleLineDust(173, dustPos - velocity * velocityScalar * 2f, default(Color));
			});
		}

		// Token: 0x060030FF RID: 12543 RVA: 0x005C1934 File Offset: 0x005BFB34
		private static void SpawnHelper_SpawnInLine(ParticleOrchestraSettings settings, Action<Vector2, Vector2> effectsPerStep = null)
		{
			if (effectsPerStep == null)
			{
				return;
			}
			Vector2 positionInWorld = settings.PositionInWorld;
			Vector2 movementVector = settings.MovementVector;
			Vector2 arg = (positionInWorld - movementVector).SafeNormalize(Vector2.Zero) * ((float)settings.UniqueInfoPiece / 1000f);
			float num = (positionInWorld - movementVector).Length();
			float num2 = arg.Length();
			if (num2 < 4f)
			{
				num2 = 4f;
			}
			for (float num3 = 0f; num3 < num; num3 += num2)
			{
				float amount = num3 / num;
				Vector2 arg2 = Vector2.Lerp(movementVector, positionInWorld, amount);
				effectsPerStep(arg2, arg);
			}
		}

		// Token: 0x06003100 RID: 12544 RVA: 0x005C19D4 File Offset: 0x005BFBD4
		private static void SpawnHelper_SpawnSingleLineDust(int dustId, Vector2 dustPos, Color color = default(Color))
		{
			Dust dust = Main.dust[Dust.NewDust(dustPos, 1, 1, dustId, 0f, 0f, 0, default(Color), 1f)];
			dust.position = dustPos;
			dust.scale = (float)Main.rand.Next(70, 110) * 0.013f;
			dust.velocity *= 0.2f;
			if (color != default(Color))
			{
				dust.color = color;
			}
		}

		// Token: 0x06003101 RID: 12545 RVA: 0x005C1A5C File Offset: 0x005BFC5C
		private static void Spawn_ShadowOrbExplosion(ParticleOrchestraSettings settings)
		{
			float num = 20f + 10f * Main.rand.NextFloat();
			float num2 = -0.7853982f;
			float scaleFactor = 0.2f + 0.4f * Main.rand.NextFloat();
			Color color = Main.hslToRgb(Main.rand.NextFloat() * 0.05f + 0.75f, 1f, 0.5f, byte.MaxValue);
			color.A /= 2;
			color *= Main.rand.NextFloat() * 0.3f + 0.7f;
			for (float num3 = 0f; num3 < 2f; num3 += 1f)
			{
				PrettySparkleParticle prettySparkleParticle = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
				Vector2 vector = (0.7853982f + 3.1415927f * num3 + num2).ToRotationVector2() * 4f;
				prettySparkleParticle.ColorTint = color;
				prettySparkleParticle.LocalPosition = settings.PositionInWorld + Main.rand.NextVector2Circular(16f, 16f);
				prettySparkleParticle.Rotation = vector.ToRotation();
				prettySparkleParticle.Scale = new Vector2(4f, 1f) * 1.1f * scaleFactor;
				prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
				prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
				prettySparkleParticle.TimeToLive = num;
				prettySparkleParticle.FadeOutEnd = num;
				prettySparkleParticle.FadeInEnd = num / 2f;
				prettySparkleParticle.FadeOutStart = num / 2f;
				prettySparkleParticle.AdditiveAmount = 0.35f;
				prettySparkleParticle.LocalPosition -= vector * num * 0.25f;
				prettySparkleParticle.Velocity = vector * 0.05f;
				prettySparkleParticle.DrawVerticalAxis = false;
				if (num3 == 1f)
				{
					prettySparkleParticle.Scale *= 1.5f;
					prettySparkleParticle.Velocity *= 1.5f;
					prettySparkleParticle.LocalPosition -= prettySparkleParticle.Velocity * 4f;
				}
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
			}
			for (float num4 = 0f; num4 < 2f; num4 += 1f)
			{
				PrettySparkleParticle prettySparkleParticle2 = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
				Vector2 vector2 = (0.7853982f + 3.1415927f * num4 + num2).ToRotationVector2() * 4f;
				prettySparkleParticle2.ColorTint = new Color(0.7583333f, 0.4f, 0.2f, 1f);
				prettySparkleParticle2.LocalPosition = settings.PositionInWorld + Main.rand.NextVector2Circular(16f, 16f);
				prettySparkleParticle2.Rotation = vector2.ToRotation();
				prettySparkleParticle2.Scale = new Vector2(4f, 1f) * 0.7f * scaleFactor;
				prettySparkleParticle2.FadeInNormalizedTime = 5E-06f;
				prettySparkleParticle2.FadeOutNormalizedTime = 0.95f;
				prettySparkleParticle2.TimeToLive = num;
				prettySparkleParticle2.FadeOutEnd = num;
				prettySparkleParticle2.FadeInEnd = num / 2f;
				prettySparkleParticle2.FadeOutStart = num / 2f;
				prettySparkleParticle2.LocalPosition -= vector2 * num * 0.25f;
				prettySparkleParticle2.Velocity = vector2 * 0.05f;
				prettySparkleParticle2.DrawVerticalAxis = false;
				if (num4 == 1f)
				{
					prettySparkleParticle2.Scale *= 1.5f;
					prettySparkleParticle2.Velocity *= 1.5f;
					prettySparkleParticle2.LocalPosition -= prettySparkleParticle2.Velocity * 4f;
				}
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle2);
				for (int i = 0; i < 1; i++)
				{
					Dust dust = Dust.NewDustPerfect(settings.PositionInWorld, 6, new Vector2?(vector2.RotatedBy((double)(Main.rand.NextFloatDirection() * 6.2831855f * 0.025f), default(Vector2)) * Main.rand.NextFloat()), 0, default(Color), 1f);
					dust.noGravity = true;
					dust.scale = 1.4f;
					Dust dust2 = Dust.NewDustPerfect(settings.PositionInWorld, 6, new Vector2?(-vector2.RotatedBy((double)(Main.rand.NextFloatDirection() * 6.2831855f * 0.025f), default(Vector2)) * Main.rand.NextFloat()), 0, default(Color), 1f);
					dust2.noGravity = true;
					dust2.scale = 1.4f;
				}
			}
			Vector2 position = settings.PositionInWorld + new Vector2(-18f, -16f);
			float circleHalfWidth = 4f;
			float circleHalfHeight = 4f;
			for (int j = 0; j < 0; j++)
			{
				int num5 = j % 2;
				Gore gore = Gore.NewGoreDirect(position, Vector2.Zero, 99, 1f + Main.rand.NextFloat() * 0.3f);
				gore.velocity = Main.rand.NextVector2Circular(circleHalfWidth, circleHalfHeight);
				gore.alpha = 127;
				gore.rotation = 6.2831855f * Main.rand.NextFloat();
				gore.position += gore.velocity * 4f;
				gore = Gore.NewGoreDirect(position, Vector2.Zero, 99, 0.8f + Main.rand.NextFloat() * 0.3f);
				gore.velocity = Main.rand.NextVector2Circular(circleHalfWidth, circleHalfHeight);
				gore.alpha = 127;
				gore.rotation = 6.2831855f * Main.rand.NextFloat();
				gore.position += gore.velocity * 4f;
				gore = Gore.NewGoreDirect(position, Vector2.Zero, 99, 1.2f + Main.rand.NextFloat() * 0.3f);
				gore.velocity = Main.rand.NextVector2Circular(circleHalfWidth, circleHalfHeight);
				gore.alpha = 127;
				gore.rotation = 6.2831855f * Main.rand.NextFloat();
				gore.position += gore.velocity * 4f;
			}
		}

		// Token: 0x06003102 RID: 12546 RVA: 0x005C20FC File Offset: 0x005C02FC
		private static void Spawn_DeadCellsDownDashExplosion(ParticleOrchestraSettings settings)
		{
			Vector2 positionInWorld = settings.PositionInWorld;
			SoundEngine.PlayTrackedSound(SoundID.DD2_MonkStaffGroundImpact, positionInWorld, default(SoundPlayOverrides));
			float x = settings.MovementVector.X;
			Point point = (positionInWorld + new Vector2(-x, -160f)).ToTileCoordinates();
			Point point2 = (positionInWorld + new Vector2(x, 160f)).ToTileCoordinates();
			int num = point.X / 2 + point2.X / 2;
			int num2 = (int)x / 2;
			bool flag = settings.UniqueInfoPiece == 1;
			float num3 = flag ? 0.01f : 0.55f;
			float num4 = flag ? 2f : 1f;
			float num5 = flag ? 3f : 2f;
			for (int i = 0; i < 40; i++)
			{
				Vector2 vector = Main.rand.NextVector2Circular(5f, 3f);
				if (vector.Y > 0f)
				{
					vector.Y *= -1f;
				}
				if (flag)
				{
					vector *= 1.5f;
				}
				Dust dust = Dust.CloneDust(Dust.NewDustPerfect(positionInWorld + Main.rand.NextVector2Circular(10f, 0f), 306, new Vector2?(vector), 0, Main.hslToRgb(num3, 1f, 0.5f, byte.MaxValue), 1.9f + 0.8f * Main.rand.NextFloat()));
				dust.scale -= 0.6f;
				dust.color = Color.White;
			}
			float num6 = 40f;
			int num7 = 3;
			for (int j = point.X; j <= point2.X; j++)
			{
				for (int k = point.Y; k <= point2.Y; k++)
				{
					if (Vector2.Distance(positionInWorld, new Vector2((float)(j * 16), (float)(k * 16))) <= (float)num2)
					{
						Tile tileSafely = Framing.GetTileSafely(j, k);
						if (tileSafely.active() && Main.tileSolid[(int)tileSafely.type] && (!Main.tileSolidTop[(int)tileSafely.type] || tileSafely.frameY == 0) && (!Main.tileFrameImportant[(int)tileSafely.type] || Main.tileSolidTop[(int)tileSafely.type]))
						{
							Tile tileSafely2 = Framing.GetTileSafely(j, k - 1);
							if (!tileSafely2.active() || !Main.tileSolid[(int)tileSafely2.type] || Main.tileSolidTop[(int)tileSafely2.type])
							{
								int num8 = WorldGen.KillTile_GetTileDustAmount(true, tileSafely);
								for (int l = 0; l < num8; l++)
								{
									Dust dust2 = Main.dust[WorldGen.KillTile_MakeTileDust(j, k, tileSafely)];
									dust2.velocity.Y = dust2.velocity.Y - (3f + (float)num7 * 1.5f);
									dust2.velocity.Y = dust2.velocity.Y * Main.rand.NextFloat();
									dust2.velocity.Y = dust2.velocity.Y * 0.75f;
									dust2.scale += (float)num7 * 0.03f;
								}
								if (num7 >= 2)
								{
									for (int m = 0; m < num8 - 1; m++)
									{
										Dust dust3 = Main.dust[WorldGen.KillTile_MakeTileDust(j, k, tileSafely)];
										dust3.velocity.Y = dust3.velocity.Y - (1f + (float)num7);
										dust3.velocity.Y = dust3.velocity.Y * Main.rand.NextFloat();
										dust3.velocity.Y = dust3.velocity.Y * 0.75f;
									}
								}
								if (num8 > 0 && Main.rand.Next(3) != 0)
								{
									float num9 = (float)Math.Abs(num - j) / (num6 / 2f);
									Gore gore = Gore.NewGoreDirect(positionInWorld, Vector2.Zero, 61 + Main.rand.Next(3), 1f - (float)num7 * 0.15f + num9 * 0.5f);
									Gore gore2 = gore;
									gore2.velocity.Y = gore2.velocity.Y - (0.1f + (float)num7 * 0.5f + num9 * (float)num7 * 1f);
									Gore gore3 = gore;
									gore3.velocity.Y = gore3.velocity.Y * Main.rand.NextFloat();
									gore.position = new Vector2((float)(j * 16 + 20), (float)(k * 16));
									PrettySparkleParticle prettySparkleParticle = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
									int num10 = Main.rand.Next(20, 40);
									prettySparkleParticle.ColorTint = Main.hslToRgb(num3 + 0.1f * Main.rand.NextFloat(), 1f, 0.5f, 0);
									prettySparkleParticle.LocalPosition = gore.position;
									prettySparkleParticle.Rotation = 1.5707964f;
									prettySparkleParticle.Scale = new Vector2(num4 + Main.rand.NextFloat() * num5, 0.7f + Main.rand.NextFloat() * 0.7f);
									prettySparkleParticle.Velocity = new Vector2(0f, -2f);
									prettySparkleParticle.FadeInNormalizedTime = 0.1f;
									prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
									prettySparkleParticle.TimeToLive = (float)num10;
									prettySparkleParticle.FadeOutEnd = (float)num10;
									prettySparkleParticle.FadeInEnd = (float)(num10 / 2);
									prettySparkleParticle.FadeOutStart = (float)(num10 / 2);
									prettySparkleParticle.AdditiveAmount = 0.35f;
									prettySparkleParticle.DrawVerticalAxis = false;
									Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06003103 RID: 12547 RVA: 0x005C2660 File Offset: 0x005C0860
		private static void Spawn_ShimmerTownNPCSend(ParticleOrchestraSettings settings)
		{
			Rectangle rect = Utils.CenteredRectangle(settings.PositionInWorld, new Vector2(30f, 60f));
			for (float num = 0f; num < 20f; num += 1f)
			{
				PrettySparkleParticle prettySparkleParticle = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
				int num2 = Main.rand.Next(20, 40);
				prettySparkleParticle.ColorTint = Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.5f, 0);
				prettySparkleParticle.LocalPosition = Main.rand.NextVector2FromRectangle(rect);
				prettySparkleParticle.Rotation = 1.5707964f;
				prettySparkleParticle.Scale = new Vector2(1f + Main.rand.NextFloat() * 2f, 0.7f + Main.rand.NextFloat() * 0.7f);
				prettySparkleParticle.Velocity = new Vector2(0f, -1f);
				prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
				prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
				prettySparkleParticle.TimeToLive = (float)num2;
				prettySparkleParticle.FadeOutEnd = (float)num2;
				prettySparkleParticle.FadeInEnd = (float)(num2 / 2);
				prettySparkleParticle.FadeOutStart = (float)(num2 / 2);
				prettySparkleParticle.AdditiveAmount = 0.35f;
				prettySparkleParticle.DrawVerticalAxis = false;
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
				PrettySparkleParticle prettySparkleParticle2 = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
				prettySparkleParticle2.ColorTint = new Color(255, 255, 255, 0);
				prettySparkleParticle2.LocalPosition = Main.rand.NextVector2FromRectangle(rect);
				prettySparkleParticle2.Rotation = 1.5707964f;
				prettySparkleParticle2.Scale = prettySparkleParticle.Scale * 0.5f;
				prettySparkleParticle2.Velocity = new Vector2(0f, -1f);
				prettySparkleParticle2.FadeInNormalizedTime = 5E-06f;
				prettySparkleParticle2.FadeOutNormalizedTime = 0.95f;
				prettySparkleParticle2.TimeToLive = (float)num2;
				prettySparkleParticle2.FadeOutEnd = (float)num2;
				prettySparkleParticle2.FadeInEnd = (float)(num2 / 2);
				prettySparkleParticle2.FadeOutStart = (float)(num2 / 2);
				prettySparkleParticle2.AdditiveAmount = 1f;
				prettySparkleParticle2.DrawVerticalAxis = false;
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle2);
			}
		}

		// Token: 0x06003104 RID: 12548 RVA: 0x005C2874 File Offset: 0x005C0A74
		private static void Spawn_ShimmerTownNPC(ParticleOrchestraSettings settings)
		{
			Rectangle rectangle = Utils.CenteredRectangle(settings.PositionInWorld, new Vector2(30f, 60f));
			for (float num = 0f; num < 20f; num += 1f)
			{
				PrettySparkleParticle prettySparkleParticle = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
				int num2 = Main.rand.Next(20, 40);
				prettySparkleParticle.ColorTint = Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.5f, 0);
				prettySparkleParticle.LocalPosition = Main.rand.NextVector2FromRectangle(rectangle);
				prettySparkleParticle.Rotation = 1.5707964f;
				prettySparkleParticle.Scale = new Vector2(1f + Main.rand.NextFloat() * 2f, 0.7f + Main.rand.NextFloat() * 0.7f);
				prettySparkleParticle.Velocity = new Vector2(0f, -1f);
				prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
				prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
				prettySparkleParticle.TimeToLive = (float)num2;
				prettySparkleParticle.FadeOutEnd = (float)num2;
				prettySparkleParticle.FadeInEnd = (float)(num2 / 2);
				prettySparkleParticle.FadeOutStart = (float)(num2 / 2);
				prettySparkleParticle.AdditiveAmount = 0.35f;
				prettySparkleParticle.DrawVerticalAxis = false;
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
				PrettySparkleParticle prettySparkleParticle2 = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
				prettySparkleParticle2.ColorTint = new Color(255, 255, 255, 0);
				prettySparkleParticle2.LocalPosition = Main.rand.NextVector2FromRectangle(rectangle);
				prettySparkleParticle2.Rotation = 1.5707964f;
				prettySparkleParticle2.Scale = prettySparkleParticle.Scale * 0.5f;
				prettySparkleParticle2.Velocity = new Vector2(0f, -1f);
				prettySparkleParticle2.FadeInNormalizedTime = 5E-06f;
				prettySparkleParticle2.FadeOutNormalizedTime = 0.95f;
				prettySparkleParticle2.TimeToLive = (float)num2;
				prettySparkleParticle2.FadeOutEnd = (float)num2;
				prettySparkleParticle2.FadeInEnd = (float)(num2 / 2);
				prettySparkleParticle2.FadeOutStart = (float)(num2 / 2);
				prettySparkleParticle2.AdditiveAmount = 1f;
				prettySparkleParticle2.DrawVerticalAxis = false;
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle2);
			}
			for (int i = 0; i < 20; i++)
			{
				int num3 = Dust.NewDust(rectangle.TopLeft(), rectangle.Width, rectangle.Height, 308, 0f, 0f, 0, default(Color), 1f);
				Dust dust = Main.dust[num3];
				dust.velocity.Y = dust.velocity.Y - 8f;
				Dust dust2 = Main.dust[num3];
				dust2.velocity.X = dust2.velocity.X * 0.5f;
				Main.dust[num3].scale = 0.8f;
				Main.dust[num3].noGravity = true;
				int num4 = Main.rand.Next(6);
				if (num4 == 0)
				{
					Main.dust[num3].color = new Color(255, 255, 210);
				}
				else if (num4 == 1)
				{
					Main.dust[num3].color = new Color(190, 245, 255);
				}
				else if (num4 == 2)
				{
					Main.dust[num3].color = new Color(255, 150, 255);
				}
				else
				{
					Main.dust[num3].color = new Color(190, 175, 255);
				}
			}
			SoundEngine.PlaySound(SoundID.Item29, settings.PositionInWorld, 0f, 1f);
		}

		// Token: 0x06003105 RID: 12549 RVA: 0x005C2BEC File Offset: 0x005C0DEC
		private static void Spawn_PooFly(ParticleOrchestraSettings settings)
		{
			int num = ParticleOrchestrator._poolFlies.CountParticlesInUse();
			if (num > 50 && Main.rand.NextFloat() >= Utils.Remap((float)num, 50f, 400f, 0.5f, 0f, true))
			{
				return;
			}
			LittleFlyingCritterParticle littleFlyingCritterParticle = ParticleOrchestrator._poolFlies.RequestParticle();
			littleFlyingCritterParticle.Prepare(LittleFlyingCritterParticle.FlyType.RegularFly, settings.PositionInWorld, 300, default(Color), 0);
			Main.ParticleSystem_World_OverPlayers.Add(littleFlyingCritterParticle);
		}

		// Token: 0x06003106 RID: 12550 RVA: 0x005C2C64 File Offset: 0x005C0E64
		private static void Spawn_Digestion(ParticleOrchestraSettings settings)
		{
			Vector2 positionInWorld = settings.PositionInWorld;
			int num = (settings.MovementVector.X < 0f) ? 1 : -1;
			int num2 = Main.rand.Next(4);
			for (int i = 0; i < 3 + num2; i++)
			{
				int num3 = Dust.NewDust(positionInWorld + Vector2.UnitX * (float)(-(float)num) * 8f - Vector2.One * 5f + Vector2.UnitY * 8f, 3, 6, 216, (float)(-(float)num), 1f, 0, default(Color), 1f);
				Main.dust[num3].velocity /= 2f;
				Main.dust[num3].scale = 0.8f;
			}
			if (Main.rand.Next(30) == 0)
			{
				int num4 = Gore.NewGore(positionInWorld + Vector2.UnitX * (float)(-(float)num) * 8f, Vector2.Zero, Main.rand.Next(580, 583), 1f);
				Main.gore[num4].velocity /= 2f;
				Main.gore[num4].velocity.Y = Math.Abs(Main.gore[num4].velocity.Y);
				Main.gore[num4].velocity.X = -Math.Abs(Main.gore[num4].velocity.X) * (float)num;
			}
			SoundEngine.PlaySound(SoundID.Item16, settings.PositionInWorld, 0f, 1f);
		}

		// Token: 0x06003107 RID: 12551 RVA: 0x005C2E2C File Offset: 0x005C102C
		private static void Spawn_ShimmerBlock(ParticleOrchestraSettings settings)
		{
			float num = (float)settings.UniqueInfoPiece / 1000f;
			if (num <= 0f)
			{
				num = 1f;
			}
			FadingParticle fadingParticle = ParticleOrchestrator._poolFading.RequestParticle();
			fadingParticle.SetBasicInfo(TextureAssets.Star[0], null, settings.MovementVector, settings.PositionInWorld);
			float num2 = 45f;
			fadingParticle.SetTypeInfo(num2, true);
			fadingParticle.AccelerationPerFrame = settings.MovementVector / num2;
			fadingParticle.ColorTint = Main.hslToRgb(Main.rand.NextFloat(), 0.75f, 0.8f, byte.MaxValue);
			fadingParticle.ColorTint.A = 30;
			fadingParticle.FadeInNormalizedTime = 0.5f;
			fadingParticle.FadeOutNormalizedTime = 0.5f;
			fadingParticle.Rotation = Main.rand.NextFloat() * 6.2831855f;
			fadingParticle.Scale = Vector2.One * (0.5f + 0.5f * Main.rand.NextFloat()) * num;
			Main.ParticleSystem_World_OverPlayers.Add(fadingParticle);
		}

		// Token: 0x06003108 RID: 12552 RVA: 0x005C2F38 File Offset: 0x005C1138
		private static void Spawn_LoadOutChange(ParticleOrchestraSettings settings)
		{
			Player player = Main.player[(int)settings.IndexOfPlayerWhoInvokedThis];
			if (!player.active)
			{
				return;
			}
			Rectangle hitbox = player.Hitbox;
			int num = 6;
			hitbox.Height -= num;
			if (player.gravDir == 1f)
			{
				hitbox.Y += num;
			}
			for (int i = 0; i < 40; i++)
			{
				Dust dust = Dust.NewDustPerfect(Main.rand.NextVector2FromRectangle(hitbox), 16, null, 120, default(Color), Main.rand.NextFloat() * 0.8f + 0.8f);
				dust.velocity = new Vector2(0f, (float)(-(float)hitbox.Height) * Main.rand.NextFloat() * 0.04f).RotatedBy((double)(Main.rand.NextFloatDirection() * 6.2831855f * 0.1f), default(Vector2));
				dust.velocity += player.velocity * 2f * Main.rand.NextFloat();
				dust.noGravity = true;
				dust.noLight = (dust.noLightEmittence = true);
			}
			for (int j = 0; j < 5; j++)
			{
				Dust dust2 = Dust.NewDustPerfect(Main.rand.NextVector2FromRectangle(hitbox), 43, null, 254, Main.hslToRgb(Main.rand.NextFloat(), 0.3f, 0.8f, byte.MaxValue), Main.rand.NextFloat() * 0.8f + 0.8f);
				dust2.velocity = new Vector2(0f, (float)(-(float)hitbox.Height) * Main.rand.NextFloat() * 0.04f).RotatedBy((double)(Main.rand.NextFloatDirection() * 6.2831855f * 0.1f), default(Vector2));
				dust2.velocity += player.velocity * 2f * Main.rand.NextFloat();
				dust2.noGravity = true;
				dust2.noLight = (dust2.noLightEmittence = true);
			}
		}

		// Token: 0x06003109 RID: 12553 RVA: 0x005C3174 File Offset: 0x005C1374
		private static void Spawn_TownSlimeTransform(ParticleOrchestraSettings settings)
		{
			switch (settings.UniqueInfoPiece)
			{
			case 0:
				ParticleOrchestrator.NerdySlimeEffect(settings);
				return;
			case 1:
				ParticleOrchestrator.CopperSlimeEffect(settings);
				return;
			case 2:
				ParticleOrchestrator.ElderSlimeEffect(settings);
				return;
			default:
				return;
			}
		}

		// Token: 0x0600310A RID: 12554 RVA: 0x005C31B0 File Offset: 0x005C13B0
		private static void Spawn_DeadCellsMushroomBoiExplosion(ParticleOrchestraSettings settings)
		{
			if (Main.rand.Next(2) == 0)
			{
				int num = Main.rand.Next(ParticleOrchestrator._mushBoiExplosionSounds.Length);
				ActiveSound activeSound = SoundEngine.GetActiveSound(ParticleOrchestrator._mushBoiExplosionSounds[num]);
				if (activeSound != null && activeSound.IsPlaying)
				{
					activeSound.Stop();
				}
				ParticleOrchestrator._mushBoiExplosionSounds[num] = SoundEngine.PlayTrackedSound(SoundID.DeadCellsMushroomExplode, settings.PositionInWorld, default(SoundPlayOverrides));
			}
			BloodyExplosionParticle bloodyExplosionParticle = ParticleOrchestrator._poolBloodyExplosion.RequestParticle();
			Color lightColorTint = new Color(255, 10, 10, 50) * 0.35f;
			bloodyExplosionParticle.ColorTint = (bloodyExplosionParticle.LightColorTint = lightColorTint);
			bloodyExplosionParticle.LocalPosition = settings.PositionInWorld;
			bloodyExplosionParticle.TimeToLive = 10f;
			bloodyExplosionParticle.FadeInNormalizedTime = 0.3f;
			bloodyExplosionParticle.FadeOutNormalizedTime = 0.6f;
			bloodyExplosionParticle.Velocity = Vector2.Zero;
			bloodyExplosionParticle.InitialScale = 1f;
			Main.ParticleSystem_World_BehindPlayers.Add(bloodyExplosionParticle);
			Asset<Texture2D> textureAsset = TextureAssets.Extra[269];
			Rectangle value = new Rectangle(0, 0, 100, 100);
			Rectangle value2 = new Rectangle(0, 101, 100, 100);
			Rectangle value3 = new Rectangle(0, 202, 100, 100);
			Color colorTint = new Color(255, 10, 10, 127) * 0.45f;
			Color color = new Color(255, 10, 10, 127) * 0.45f;
			float num2 = 15f;
			FadingParticle fadingParticle = ParticleOrchestrator._poolFading.RequestParticle();
			fadingParticle.SetBasicInfo(textureAsset, new Rectangle?(value), Vector2.Zero, settings.PositionInWorld);
			fadingParticle.SetTypeInfo(num2, true);
			fadingParticle.ColorTint = color;
			fadingParticle.Rotation = Main.rand.NextFloat() * 6.2831855f;
			fadingParticle.RotationVelocity = 6.2831855f * (1f / num2) * 1f * Main.rand.NextFloatDirection();
			fadingParticle.RotationAcceleration = -fadingParticle.RotationVelocity * (1f / num2);
			fadingParticle.FadeInNormalizedTime = 0.01f;
			fadingParticle.FadeOutNormalizedTime = 0.1f;
			fadingParticle.Scale = Vector2.One * 0.6f;
			fadingParticle.ScaleVelocity = Vector2.One * (1.2f / num2);
			fadingParticle.ScaleAcceleration = fadingParticle.ScaleVelocity * (-1f / num2);
			Main.ParticleSystem_World_OverPlayers.Add(fadingParticle);
			FadingParticle fadingParticle2 = ParticleOrchestrator._poolFading.RequestParticle();
			fadingParticle2.SetBasicInfo(textureAsset, new Rectangle?(value2), Vector2.Zero, settings.PositionInWorld);
			fadingParticle2.SetTypeInfo(num2 * 0.8f, true);
			fadingParticle2.ColorTint = color * 0.25f;
			fadingParticle2.FadeInNormalizedTime = 0.1f;
			fadingParticle2.FadeOutNormalizedTime = 0.1f;
			fadingParticle2.Scale = Vector2.One * 0.3f;
			fadingParticle2.ScaleVelocity = Vector2.One * 6.5f * (1f / num2);
			fadingParticle2.ScaleAcceleration = fadingParticle2.ScaleVelocity * (-1f / num2);
			Main.ParticleSystem_World_OverPlayers.Add(fadingParticle2);
			FadingParticle fadingParticle3 = ParticleOrchestrator._poolFading.RequestParticle();
			fadingParticle3.SetBasicInfo(textureAsset, new Rectangle?(value2), Vector2.Zero, settings.PositionInWorld);
			fadingParticle3.SetTypeInfo(num2, true);
			fadingParticle3.ColorTint = color * 0.3f;
			fadingParticle3.FadeInNormalizedTime = 0.4f;
			fadingParticle3.FadeOutNormalizedTime = 0.5f;
			fadingParticle3.Scale = fadingParticle.Scale;
			fadingParticle3.ScaleVelocity = fadingParticle.ScaleVelocity;
			fadingParticle3.ScaleAcceleration = fadingParticle.ScaleAcceleration;
			Main.ParticleSystem_World_OverPlayers.Add(fadingParticle3);
			FadingParticle fadingParticle4 = ParticleOrchestrator._poolFading.RequestParticle();
			fadingParticle4.SetBasicInfo(textureAsset, new Rectangle?(value3), Vector2.Zero, settings.PositionInWorld);
			fadingParticle4.SetTypeInfo(num2, true);
			fadingParticle4.ColorTint = color;
			fadingParticle4.FadeInNormalizedTime = 0.01f;
			fadingParticle4.FadeOutNormalizedTime = 0.7f;
			fadingParticle4.Scale = fadingParticle.Scale * 0.9f;
			fadingParticle4.ScaleVelocity = fadingParticle.ScaleVelocity * 1.5f;
			fadingParticle4.ScaleAcceleration = fadingParticle.ScaleAcceleration * 1.5f;
			Main.ParticleSystem_World_OverPlayers.Add(fadingParticle4);
			for (float num3 = 0f; num3 < 1f; num3 += 0.16666667f)
			{
				float num4 = 15f;
				float num5 = 6.2831855f * num3;
				FadingParticle fadingParticle5 = ParticleOrchestrator._poolFading.RequestParticle();
				fadingParticle5.SetBasicInfo(TextureAssets.Extra[89], null, num5.ToRotationVector2() * (4f + 7f * Main.rand.NextFloat()), settings.PositionInWorld + num5.ToRotationVector2() * (10f + 90f * Main.rand.NextFloat()));
				fadingParticle5.SetTypeInfo(num4, true);
				fadingParticle5.AccelerationPerFrame = fadingParticle5.Velocity * (-1f / num4);
				fadingParticle5.LocalPosition -= fadingParticle5.Velocity * 4f;
				fadingParticle5.ColorTint = colorTint;
				fadingParticle5.Rotation = num5 + 1.5707964f;
				fadingParticle5.FadeInNormalizedTime = 0.2f;
				fadingParticle5.FadeOutNormalizedTime = 0.3f;
				fadingParticle5.Scale = new Vector2(0.4f, 0.8f) * (0.6f + 0.6f * Main.rand.NextFloat());
				Main.ParticleSystem_World_BehindPlayers.Add(fadingParticle5);
			}
		}

		// Token: 0x0600310B RID: 12555 RVA: 0x005C3778 File Offset: 0x005C1978
		private static void Spawn_DeadCellsMushroomBoiTargetFound(ParticleOrchestraSettings settings)
		{
			ShockIconParticle shockIconParticle = ParticleOrchestrator._poolShockIcon.RequestParticle();
			shockIconParticle.ColorTint = new Color(255, 255, 10, 150);
			shockIconParticle.LocalPosition = settings.PositionInWorld;
			shockIconParticle.TimeToLive = 15f;
			shockIconParticle.FadeInNormalizedTime = 0.2f;
			shockIconParticle.FadeOutNormalizedTime = 0.7f;
			shockIconParticle.Velocity = Vector2.Zero;
			shockIconParticle.InitialScale = 1f;
			shockIconParticle.ParentProjectileID = settings.UniqueInfoPiece;
			shockIconParticle.OffsetFromParent = new Vector2(0f, -40f);
			Main.ParticleSystem_World_BehindPlayers.Add(shockIconParticle);
		}

		// Token: 0x0600310C RID: 12556 RVA: 0x005C381C File Offset: 0x005C1A1C
		private static void Spawn_DeadCellsBarnacleShotFiring(ParticleOrchestraSettings settings)
		{
			Vector2 positionInWorld = settings.PositionInWorld;
			Vector2 movementVector = settings.MovementVector;
			SoundEngine.PlaySound(SoundID.Item95, positionInWorld, 0f, 1f);
			for (int i = 0; i < 15; i++)
			{
				Vector2 position = positionInWorld + new Vector2((float)Main.rand.Next(-4, 4), (float)Main.rand.Next(-4, 4));
				Vector2 vector = movementVector.SafeNormalize(Vector2.Zero).RotatedByRandom(0.19634954631328583) * ((i >= 8) ? (3f + Main.rand.NextFloat() * 2f) : (7f + Main.rand.NextFloat() * 2f));
				Dust dust = Dust.NewDustPerfect(position, 2, new Vector2?(vector), 0, default(Color), 1f);
				dust.alpha = 50;
				dust.scale = 1.2f + (float)Main.rand.Next(-5, 5) * 0.01f;
				dust.fadeIn = 0.5f;
				dust.noGravity = true;
				dust.velocity = vector;
			}
			for (int j = 0; j < 5; j++)
			{
				Vector2 position2 = positionInWorld + new Vector2((float)Main.rand.Next(-4, 4), (float)Main.rand.Next(-4, 4));
				Vector2 vector2 = movementVector.SafeNormalize(Vector2.Zero).RotatedByRandom(0.09817477315664291) * (0f + Main.rand.NextFloat() * 8f);
				Dust dust2 = Dust.NewDustPerfect(position2, 267, new Vector2?(vector2), 0, default(Color), 1f);
				dust2.alpha = 127;
				dust2.color = Color.Lerp(Color.DarkOliveGreen, Color.White, Main.rand.NextFloat() * 0.25f);
				dust2.fadeIn = 0.6f + Main.rand.NextFloat() * 0.6f;
				dust2.scale = 0.5f;
				dust2.noGravity = true;
				dust2.velocity = vector2;
			}
		}

		// Token: 0x0600310D RID: 12557 RVA: 0x005C3A2C File Offset: 0x005C1C2C
		private static void ElderSlimeEffect(ParticleOrchestraSettings settings)
		{
			for (int i = 0; i < 30; i++)
			{
				Dust dust = Dust.NewDustPerfect(settings.PositionInWorld + Main.rand.NextVector2Circular(20f, 20f), 43, new Vector2?((settings.MovementVector * 0.75f + Main.rand.NextVector2Circular(6f, 6f)) * Main.rand.NextFloat()), 26, Color.Lerp(Main.OurFavoriteColor, Color.White, Main.rand.NextFloat()), 1f + Main.rand.NextFloat() * 1.4f);
				dust.fadeIn = 1.5f;
				if (dust.velocity.Y > 0f && Main.rand.Next(2) == 0)
				{
					Dust dust2 = dust;
					dust2.velocity.Y = dust2.velocity.Y * -1f;
				}
				dust.noGravity = true;
			}
			for (int j = 0; j < 8; j++)
			{
				Gore.NewGoreDirect(settings.PositionInWorld + Utils.RandomVector2(Main.rand, -30f, 30f) * new Vector2(0.5f, 1f), Vector2.Zero, 61 + Main.rand.Next(3), 1f).velocity *= 0.5f;
			}
		}

		// Token: 0x0600310E RID: 12558 RVA: 0x005C3B98 File Offset: 0x005C1D98
		private static void NerdySlimeEffect(ParticleOrchestraSettings settings)
		{
			Color newColor = new Color(0, 80, 255, 100);
			for (int i = 0; i < 60; i++)
			{
				Dust.NewDustPerfect(settings.PositionInWorld, 4, new Vector2?((settings.MovementVector * 0.75f + Main.rand.NextVector2Circular(6f, 6f)) * Main.rand.NextFloat()), 175, newColor, 0.6f + Main.rand.NextFloat() * 1.4f);
			}
		}

		// Token: 0x0600310F RID: 12559 RVA: 0x005C3C2C File Offset: 0x005C1E2C
		private static void CopperSlimeEffect(ParticleOrchestraSettings settings)
		{
			for (int i = 0; i < 40; i++)
			{
				Dust dust = Dust.NewDustPerfect(settings.PositionInWorld + Main.rand.NextVector2Circular(20f, 20f), 43, new Vector2?((settings.MovementVector * 0.75f + Main.rand.NextVector2Circular(6f, 6f)) * Main.rand.NextFloat()), 26, Color.Lerp(new Color(183, 88, 25), Color.White, Main.rand.NextFloat() * 0.5f), 1f + Main.rand.NextFloat() * 1.4f);
				dust.fadeIn = 1.5f;
				if (dust.velocity.Y > 0f && Main.rand.Next(2) == 0)
				{
					Dust dust2 = dust;
					dust2.velocity.Y = dust2.velocity.Y * -1f;
				}
				dust.noGravity = true;
			}
		}

		// Token: 0x06003110 RID: 12560 RVA: 0x005C3D38 File Offset: 0x005C1F38
		private static void Spawn_ShimmerArrow(ParticleOrchestraSettings settings)
		{
			float num = 20f;
			for (int i = 0; i < 2; i++)
			{
				float num2 = 6.2831855f * Main.rand.NextFloatDirection() * 0.05f;
				Color color = Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.5f, byte.MaxValue);
				color.A /= 2;
				Color color2 = color;
				color2.A = byte.MaxValue;
				color2 = Color.Lerp(color2, Color.White, 0.5f);
				for (float num3 = 0f; num3 < 4f; num3 += 1f)
				{
					PrettySparkleParticle prettySparkleParticle = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
					Vector2 vector = (1.5707964f * num3 + num2).ToRotationVector2() * 4f;
					prettySparkleParticle.ColorTint = color;
					prettySparkleParticle.LocalPosition = settings.PositionInWorld;
					prettySparkleParticle.Rotation = vector.ToRotation();
					prettySparkleParticle.Scale = new Vector2((num3 % 2f == 0f) ? 2f : 4f, 0.5f) * 1.1f;
					prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
					prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
					prettySparkleParticle.TimeToLive = num;
					prettySparkleParticle.FadeOutEnd = num;
					prettySparkleParticle.FadeInEnd = num / 2f;
					prettySparkleParticle.FadeOutStart = num / 2f;
					prettySparkleParticle.AdditiveAmount = 0.35f;
					prettySparkleParticle.Velocity = -vector * 0.2f;
					prettySparkleParticle.DrawVerticalAxis = false;
					if (num3 % 2f == 1f)
					{
						prettySparkleParticle.Scale *= 0.9f;
						prettySparkleParticle.Velocity *= 0.9f;
					}
					Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
				}
				for (float num4 = 0f; num4 < 4f; num4 += 1f)
				{
					PrettySparkleParticle prettySparkleParticle2 = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
					Vector2 vector2 = (1.5707964f * num4 + num2).ToRotationVector2() * 4f;
					prettySparkleParticle2.ColorTint = color2;
					prettySparkleParticle2.LocalPosition = settings.PositionInWorld;
					prettySparkleParticle2.Rotation = vector2.ToRotation();
					prettySparkleParticle2.Scale = new Vector2((num4 % 2f == 0f) ? 2f : 4f, 0.5f) * 0.7f;
					prettySparkleParticle2.FadeInNormalizedTime = 5E-06f;
					prettySparkleParticle2.FadeOutNormalizedTime = 0.95f;
					prettySparkleParticle2.TimeToLive = num;
					prettySparkleParticle2.FadeOutEnd = num;
					prettySparkleParticle2.FadeInEnd = num / 2f;
					prettySparkleParticle2.FadeOutStart = num / 2f;
					prettySparkleParticle2.Velocity = vector2 * 0.2f;
					prettySparkleParticle2.DrawVerticalAxis = false;
					if (num4 % 2f == 1f)
					{
						prettySparkleParticle2.Scale *= 1.2f;
						prettySparkleParticle2.Velocity *= 1.2f;
					}
					Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle2);
					if (i == 0)
					{
						for (int j = 0; j < 1; j++)
						{
							Dust dust = Dust.NewDustPerfect(settings.PositionInWorld, 306, new Vector2?(vector2.RotatedBy((double)(Main.rand.NextFloatDirection() * 6.2831855f * 0.025f), default(Vector2)) * Main.rand.NextFloat()), 0, default(Color), 1f);
							dust.noGravity = true;
							dust.scale = 1.4f;
							dust.fadeIn = 1.2f;
							dust.color = color;
							Dust dust2 = Dust.NewDustPerfect(settings.PositionInWorld, 306, new Vector2?(-vector2.RotatedBy((double)(Main.rand.NextFloatDirection() * 6.2831855f * 0.025f), default(Vector2)) * Main.rand.NextFloat()), 0, default(Color), 1f);
							dust2.noGravity = true;
							dust2.scale = 1.4f;
							dust2.fadeIn = 1.2f;
							dust2.color = color;
						}
					}
				}
			}
		}

		// Token: 0x06003111 RID: 12561 RVA: 0x005C418C File Offset: 0x005C238C
		private static void Spawn_ItemTransfer(ParticleOrchestraSettings settings)
		{
			Vector2 positionInWorld = settings.PositionInWorld;
			Vector2 vector = settings.PositionInWorld + settings.MovementVector;
			int num = settings.UniqueInfoPiece & 16777215;
			BitsByte bitsByte = (byte)(settings.UniqueInfoPiece >> 24);
			bool flag = bitsByte[0];
			bool flag2 = bitsByte[1];
			bool transitionIn = bitsByte[2];
			bool fullbright = bitsByte[3];
			Item item;
			if (!ContentSamples.ItemsByType.TryGetValue(num, out item))
			{
				return;
			}
			if (item.IsAir)
			{
				return;
			}
			num = item.type;
			int num2 = Main.rand.Next(60, 80);
			Chest.AskForChestToEatItem(vector + new Vector2(-8f, -8f), num2 + 10);
			Vector2 value = Main.rand.NextVector2Square(-1f, 1f);
			ItemTransferParticle itemTransferParticle = ParticleOrchestrator._poolItemTransfer.RequestParticle();
			itemTransferParticle.Prepare(num, num2, positionInWorld, vector, flag ? (value * 24f) : Vector2.Zero, flag2 ? (value * 8f) : Vector2.Zero, transitionIn, fullbright, false, 1);
			Main.ParticleSystem_World_OverPlayers.Add(itemTransferParticle);
		}

		// Token: 0x06003112 RID: 12562 RVA: 0x005C42B8 File Offset: 0x005C24B8
		private static void Spawn_PetExchange(ParticleOrchestraSettings settings)
		{
			Vector2 positionInWorld = settings.PositionInWorld;
			for (int i = 0; i < 13; i++)
			{
				Gore gore = Gore.NewGoreDirect(positionInWorld + new Vector2(-20f, -20f) + Main.rand.NextVector2Circular(20f, 20f), Vector2.Zero, Main.rand.Next(61, 64), 1f + Main.rand.NextFloat() * 0.3f);
				gore.alpha = 100;
				gore.velocity = (6.2831855f * (float)Main.rand.Next()).ToRotationVector2() * Main.rand.NextFloat() + settings.MovementVector * 0.5f;
			}
		}

		// Token: 0x06003113 RID: 12563 RVA: 0x005C4384 File Offset: 0x005C2584
		private static void Spawn_TerraBlade(ParticleOrchestraSettings settings)
		{
			float num = 30f;
			float num2 = settings.MovementVector.ToRotation() + 1.5707964f;
			float x = 3f;
			for (float num3 = 0f; num3 < 4f; num3 += 1f)
			{
				PrettySparkleParticle prettySparkleParticle = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
				Vector2 vector = (1.5707964f * num3 + num2).ToRotationVector2() * 4f;
				prettySparkleParticle.ColorTint = new Color(0.2f, 0.85f, 0.4f, 0.5f);
				prettySparkleParticle.LocalPosition = settings.PositionInWorld;
				prettySparkleParticle.Rotation = vector.ToRotation();
				prettySparkleParticle.Scale = new Vector2(x, 0.5f) * 1.1f;
				prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
				prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
				prettySparkleParticle.TimeToLive = num;
				prettySparkleParticle.FadeOutEnd = num;
				prettySparkleParticle.FadeInEnd = num / 2f;
				prettySparkleParticle.FadeOutStart = num / 2f;
				prettySparkleParticle.AdditiveAmount = 0.35f;
				prettySparkleParticle.Velocity = -vector * 0.2f;
				prettySparkleParticle.DrawVerticalAxis = false;
				if (num3 % 2f == 1f)
				{
					prettySparkleParticle.Scale *= 1.5f;
					prettySparkleParticle.Velocity *= 2f;
				}
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
			}
			for (float num4 = -1f; num4 <= 1f; num4 += 2f)
			{
				PrettySparkleParticle prettySparkleParticle2 = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
				num2.ToRotationVector2() * 4f;
				Vector2 vector2 = (1.5707964f * num4 + num2).ToRotationVector2() * 2f;
				prettySparkleParticle2.ColorTint = new Color(0.4f, 1f, 0.4f, 0.5f);
				prettySparkleParticle2.LocalPosition = settings.PositionInWorld;
				prettySparkleParticle2.Rotation = vector2.ToRotation();
				prettySparkleParticle2.Scale = new Vector2(x, 0.5f) * 1.1f;
				prettySparkleParticle2.FadeInNormalizedTime = 5E-06f;
				prettySparkleParticle2.FadeOutNormalizedTime = 0.95f;
				prettySparkleParticle2.TimeToLive = num;
				prettySparkleParticle2.FadeOutEnd = num;
				prettySparkleParticle2.FadeInEnd = num / 2f;
				prettySparkleParticle2.FadeOutStart = num / 2f;
				prettySparkleParticle2.AdditiveAmount = 0.35f;
				prettySparkleParticle2.Velocity = vector2.RotatedBy(1.5707963705062866, default(Vector2)) * 0.5f;
				prettySparkleParticle2.DrawVerticalAxis = false;
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle2);
			}
			for (float num5 = 0f; num5 < 4f; num5 += 1f)
			{
				PrettySparkleParticle prettySparkleParticle3 = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
				Vector2 vector3 = (1.5707964f * num5 + num2).ToRotationVector2() * 4f;
				prettySparkleParticle3.ColorTint = new Color(0.2f, 1f, 0.2f, 1f);
				prettySparkleParticle3.LocalPosition = settings.PositionInWorld;
				prettySparkleParticle3.Rotation = vector3.ToRotation();
				prettySparkleParticle3.Scale = new Vector2(x, 0.5f) * 0.7f;
				prettySparkleParticle3.FadeInNormalizedTime = 5E-06f;
				prettySparkleParticle3.FadeOutNormalizedTime = 0.95f;
				prettySparkleParticle3.TimeToLive = num;
				prettySparkleParticle3.FadeOutEnd = num;
				prettySparkleParticle3.FadeInEnd = num / 2f;
				prettySparkleParticle3.FadeOutStart = num / 2f;
				prettySparkleParticle3.Velocity = vector3 * 0.2f;
				prettySparkleParticle3.DrawVerticalAxis = false;
				if (num5 % 2f == 1f)
				{
					prettySparkleParticle3.Scale *= 1.5f;
					prettySparkleParticle3.Velocity *= 2f;
				}
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle3);
				for (int i = 0; i < 1; i++)
				{
					Dust dust = Dust.NewDustPerfect(settings.PositionInWorld, 107, new Vector2?(vector3.RotatedBy((double)(Main.rand.NextFloatDirection() * 6.2831855f * 0.025f), default(Vector2)) * Main.rand.NextFloat()), 0, default(Color), 1f);
					dust.noGravity = true;
					dust.scale = 0.8f;
					Dust dust2 = Dust.NewDustPerfect(settings.PositionInWorld, 107, new Vector2?(-vector3.RotatedBy((double)(Main.rand.NextFloatDirection() * 6.2831855f * 0.025f), default(Vector2)) * Main.rand.NextFloat()), 0, default(Color), 1f);
					dust2.noGravity = true;
					dust2.scale = 1.4f;
				}
			}
		}

		// Token: 0x06003114 RID: 12564 RVA: 0x005C487C File Offset: 0x005C2A7C
		private static void Spawn_Excalibur(ParticleOrchestraSettings settings)
		{
			float num = 30f;
			float num2 = 0f;
			for (float num3 = 0f; num3 < 4f; num3 += 1f)
			{
				PrettySparkleParticle prettySparkleParticle = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
				Vector2 vector = (1.5707964f * num3 + num2).ToRotationVector2() * 4f;
				prettySparkleParticle.ColorTint = new Color(0.9f, 0.85f, 0.4f, 0.5f);
				prettySparkleParticle.LocalPosition = settings.PositionInWorld;
				prettySparkleParticle.Rotation = vector.ToRotation();
				prettySparkleParticle.Scale = new Vector2((num3 % 2f == 0f) ? 2f : 4f, 0.5f) * 1.1f;
				prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
				prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
				prettySparkleParticle.TimeToLive = num;
				prettySparkleParticle.FadeOutEnd = num;
				prettySparkleParticle.FadeInEnd = num / 2f;
				prettySparkleParticle.FadeOutStart = num / 2f;
				prettySparkleParticle.AdditiveAmount = 0.35f;
				prettySparkleParticle.Velocity = -vector * 0.2f;
				prettySparkleParticle.DrawVerticalAxis = false;
				if (num3 % 2f == 1f)
				{
					prettySparkleParticle.Scale *= 1.5f;
					prettySparkleParticle.Velocity *= 1.5f;
				}
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
			}
			for (float num4 = 0f; num4 < 4f; num4 += 1f)
			{
				PrettySparkleParticle prettySparkleParticle2 = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
				Vector2 vector2 = (1.5707964f * num4 + num2).ToRotationVector2() * 4f;
				prettySparkleParticle2.ColorTint = new Color(1f, 1f, 0.2f, 1f);
				prettySparkleParticle2.LocalPosition = settings.PositionInWorld;
				prettySparkleParticle2.Rotation = vector2.ToRotation();
				prettySparkleParticle2.Scale = new Vector2((num4 % 2f == 0f) ? 2f : 4f, 0.5f) * 0.7f;
				prettySparkleParticle2.FadeInNormalizedTime = 5E-06f;
				prettySparkleParticle2.FadeOutNormalizedTime = 0.95f;
				prettySparkleParticle2.TimeToLive = num;
				prettySparkleParticle2.FadeOutEnd = num;
				prettySparkleParticle2.FadeInEnd = num / 2f;
				prettySparkleParticle2.FadeOutStart = num / 2f;
				prettySparkleParticle2.Velocity = vector2 * 0.2f;
				prettySparkleParticle2.DrawVerticalAxis = false;
				if (num4 % 2f == 1f)
				{
					prettySparkleParticle2.Scale *= 1.5f;
					prettySparkleParticle2.Velocity *= 1.5f;
				}
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle2);
				for (int i = 0; i < 1; i++)
				{
					Dust dust = Dust.NewDustPerfect(settings.PositionInWorld, 169, new Vector2?(vector2.RotatedBy((double)(Main.rand.NextFloatDirection() * 6.2831855f * 0.025f), default(Vector2)) * Main.rand.NextFloat()), 0, default(Color), 1f);
					dust.noGravity = true;
					dust.scale = 1.4f;
					Dust dust2 = Dust.NewDustPerfect(settings.PositionInWorld, 169, new Vector2?(-vector2.RotatedBy((double)(Main.rand.NextFloatDirection() * 6.2831855f * 0.025f), default(Vector2)) * Main.rand.NextFloat()), 0, default(Color), 1f);
					dust2.noGravity = true;
					dust2.scale = 1.4f;
				}
			}
		}

		// Token: 0x06003115 RID: 12565 RVA: 0x005C4C47 File Offset: 0x005C2E47
		private static void Spawn_SlapHand(ParticleOrchestraSettings settings)
		{
			SoundEngine.PlaySound(SoundID.Item175, settings.PositionInWorld, 0f, 1f);
		}

		// Token: 0x06003116 RID: 12566 RVA: 0x005C4C64 File Offset: 0x005C2E64
		private static void Spawn_WaffleIron(ParticleOrchestraSettings settings)
		{
			SoundEngine.PlaySound(SoundID.Item178, settings.PositionInWorld, 0f, 1f);
		}

		// Token: 0x06003117 RID: 12567 RVA: 0x005C4C84 File Offset: 0x005C2E84
		private static void Spawn_PlayerVoiceOverrideSound(ParticleOrchestraSettings settings)
		{
			byte indexOfPlayerWhoInvokedThis = settings.IndexOfPlayerWhoInvokedThis;
			if (indexOfPlayerWhoInvokedThis < 0 || indexOfPlayerWhoInvokedThis >= 255)
			{
				return;
			}
			Player player = Main.player[(int)settings.IndexOfPlayerWhoInvokedThis];
			if (!player.active || player.dead)
			{
				return;
			}
			sbyte voiceOverride = (sbyte)settings.UniqueInfoPiece;
			sbyte voiceOverride2 = player.voiceOverride;
			player.voiceOverride = voiceOverride;
			player.PlayHurtSound();
			player.voiceOverride = voiceOverride2;
		}

		// Token: 0x06003118 RID: 12568 RVA: 0x005C4CE8 File Offset: 0x005C2EE8
		private static void Spawn_ClassyCane(ParticleOrchestraSettings settings)
		{
			int num = 7;
			float scale = 1.1f;
			int type = 10;
			Color newColor = default(Color);
			Vector2 positionInWorld = settings.PositionInWorld;
			int num2 = 20;
			int num3 = 20;
			positionInWorld.X -= (float)(num2 / 2);
			positionInWorld.Y -= (float)(num3 / 2);
			int num4 = Main.rand.Next(0, 2);
			for (int i = 0; i < num4; i++)
			{
				int num5 = Gore.NewGore(new Vector2(positionInWorld.X + (float)Main.rand.Next(num2), positionInWorld.Y + (float)Main.rand.Next(num3)), Vector2.Zero, 1218, 1f);
				Main.gore[num5].velocity = new Vector2((float)Main.rand.Next(-10, 11) * 0.4f, -(3f + (float)Main.rand.Next(6) * 0.3f));
			}
			for (int j = 0; j < num; j++)
			{
				int num6 = Dust.NewDust(positionInWorld, num2, num3, type, 0f, -1f, 80, newColor, scale);
				if (Main.rand.Next(3) != 0)
				{
					Main.dust[num6].noGravity = true;
				}
			}
		}

		// Token: 0x06003119 RID: 12569 RVA: 0x005C4E29 File Offset: 0x005C3029
		private static void Spawn_FlyMeal(ParticleOrchestraSettings settings)
		{
			SoundEngine.PlaySound(SoundID.Item16, settings.PositionInWorld, 0f, 1f);
		}

		// Token: 0x0600311A RID: 12570 RVA: 0x005C4E46 File Offset: 0x005C3046
		private static void Spawn_VampireOnFire(ParticleOrchestraSettings settings)
		{
			SoundEngine.PlaySound(SoundID.Item20, settings.PositionInWorld, 0f, 1f);
		}

		// Token: 0x0600311B RID: 12571 RVA: 0x005C4E64 File Offset: 0x005C3064
		private static void Spawn_GasTrap(ParticleOrchestraSettings settings)
		{
			SoundEngine.PlaySound(SoundID.Item16, settings.PositionInWorld, 0f, 1f);
			Vector2 movementVector = settings.MovementVector;
			int num = 12;
			int num2 = 10;
			float num3 = 5f;
			float num4 = 2.5f;
			Color lightColorTint = new Color(0.2f, 0.4f, 0.15f);
			Vector2 positionInWorld = settings.PositionInWorld;
			float num5 = 0.15707964f;
			float num6 = 0.20943952f;
			for (int i = 0; i < num; i++)
			{
				Vector2 vector = movementVector + new Vector2(num3 + Main.rand.NextFloat() * 1f, 0f).RotatedBy((double)((float)i / (float)num * 6.2831855f), Vector2.Zero);
				vector = vector.RotatedByRandom((double)num5);
				GasParticle gasParticle = ParticleOrchestrator._poolGas.RequestParticle();
				gasParticle.AccelerationPerFrame = Vector2.Zero;
				gasParticle.Velocity = vector;
				gasParticle.ColorTint = Color.White;
				gasParticle.LightColorTint = lightColorTint;
				gasParticle.LocalPosition = positionInWorld + vector;
				gasParticle.TimeToLive = (float)(50 + Main.rand.Next(20));
				gasParticle.InitialScale = 1f + Main.rand.NextFloat() * 0.35f;
				Main.ParticleSystem_World_BehindPlayers.Add(gasParticle);
			}
			for (int j = 0; j < num2; j++)
			{
				Vector2 vector2 = new Vector2(num4 + Main.rand.NextFloat() * 1.45f, 0f).RotatedBy((double)((float)j / (float)num2 * 6.2831855f), Vector2.Zero);
				vector2 = vector2.RotatedByRandom((double)num6);
				if (j % 2 == 0)
				{
					vector2 *= 0.5f;
				}
				GasParticle gasParticle2 = ParticleOrchestrator._poolGas.RequestParticle();
				gasParticle2.AccelerationPerFrame = Vector2.Zero;
				gasParticle2.Velocity = vector2;
				gasParticle2.ColorTint = Color.White;
				gasParticle2.LightColorTint = lightColorTint;
				gasParticle2.LocalPosition = positionInWorld;
				gasParticle2.TimeToLive = (float)(80 + Main.rand.Next(30));
				gasParticle2.InitialScale = 1f + Main.rand.NextFloat() * 0.5f;
				Main.ParticleSystem_World_BehindPlayers.Add(gasParticle2);
			}
		}

		// Token: 0x0600311C RID: 12572 RVA: 0x005C50A0 File Offset: 0x005C32A0
		private static void Spawn_TrueExcalibur(ParticleOrchestraSettings settings)
		{
			float num = 36f;
			float num2 = 0.7853982f;
			for (float num3 = 0f; num3 < 2f; num3 += 1f)
			{
				PrettySparkleParticle prettySparkleParticle = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
				Vector2 v = (1.5707964f * num3 + num2).ToRotationVector2() * 4f;
				prettySparkleParticle.ColorTint = new Color(1f, 0f, 0.3f, 1f);
				prettySparkleParticle.LocalPosition = settings.PositionInWorld;
				prettySparkleParticle.Rotation = v.ToRotation();
				prettySparkleParticle.Scale = new Vector2(5f, 0.5f) * 1.1f;
				prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
				prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
				prettySparkleParticle.TimeToLive = num;
				prettySparkleParticle.FadeOutEnd = num;
				prettySparkleParticle.FadeInEnd = num / 2f;
				prettySparkleParticle.FadeOutStart = num / 2f;
				prettySparkleParticle.AdditiveAmount = 0.35f;
				prettySparkleParticle.DrawVerticalAxis = false;
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
			}
			for (float num4 = 0f; num4 < 2f; num4 += 1f)
			{
				PrettySparkleParticle prettySparkleParticle2 = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
				Vector2 vector = (1.5707964f * num4 + num2).ToRotationVector2() * 4f;
				prettySparkleParticle2.ColorTint = new Color(1f, 0.5f, 0.8f, 1f);
				prettySparkleParticle2.LocalPosition = settings.PositionInWorld;
				prettySparkleParticle2.Rotation = vector.ToRotation();
				prettySparkleParticle2.Scale = new Vector2(3f, 0.5f) * 0.7f;
				prettySparkleParticle2.FadeInNormalizedTime = 5E-06f;
				prettySparkleParticle2.FadeOutNormalizedTime = 0.95f;
				prettySparkleParticle2.TimeToLive = num;
				prettySparkleParticle2.FadeOutEnd = num;
				prettySparkleParticle2.FadeInEnd = num / 2f;
				prettySparkleParticle2.FadeOutStart = num / 2f;
				prettySparkleParticle2.DrawVerticalAxis = false;
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle2);
				for (int i = 0; i < 1; i++)
				{
					if (Main.rand.Next(2) != 0)
					{
						Dust dust = Dust.NewDustPerfect(settings.PositionInWorld, 242, new Vector2?(vector.RotatedBy((double)(Main.rand.NextFloatDirection() * 6.2831855f * 0.025f), default(Vector2)) * Main.rand.NextFloat()), 0, default(Color), 1f);
						dust.noGravity = true;
						dust.scale = 1.4f;
						Dust dust2 = Dust.NewDustPerfect(settings.PositionInWorld, 242, new Vector2?(-vector.RotatedBy((double)(Main.rand.NextFloatDirection() * 6.2831855f * 0.025f), default(Vector2)) * Main.rand.NextFloat()), 0, default(Color), 1f);
						dust2.noGravity = true;
						dust2.scale = 1.4f;
					}
				}
			}
			num = 30f;
			num2 = 0f;
			for (float num5 = 0f; num5 < 4f; num5 += 1f)
			{
				PrettySparkleParticle prettySparkleParticle3 = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
				Vector2 vector2 = (1.5707964f * num5 + num2).ToRotationVector2() * 4f;
				prettySparkleParticle3.ColorTint = new Color(0.9f, 0.85f, 0.4f, 0.5f);
				prettySparkleParticle3.LocalPosition = settings.PositionInWorld;
				prettySparkleParticle3.Rotation = vector2.ToRotation();
				prettySparkleParticle3.Scale = new Vector2((num5 % 2f == 0f) ? 2f : 4f, 0.5f) * 1.1f;
				prettySparkleParticle3.FadeInNormalizedTime = 5E-06f;
				prettySparkleParticle3.FadeOutNormalizedTime = 0.95f;
				prettySparkleParticle3.TimeToLive = num;
				prettySparkleParticle3.FadeOutEnd = num;
				prettySparkleParticle3.FadeInEnd = num / 2f;
				prettySparkleParticle3.FadeOutStart = num / 2f;
				prettySparkleParticle3.AdditiveAmount = 0.35f;
				prettySparkleParticle3.Velocity = -vector2 * 0.2f;
				prettySparkleParticle3.DrawVerticalAxis = false;
				if (num5 % 2f == 1f)
				{
					prettySparkleParticle3.Scale *= 1.5f;
					prettySparkleParticle3.Velocity *= 1.5f;
				}
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle3);
			}
			for (float num6 = 0f; num6 < 4f; num6 += 1f)
			{
				PrettySparkleParticle prettySparkleParticle4 = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
				Vector2 vector3 = (1.5707964f * num6 + num2).ToRotationVector2() * 4f;
				prettySparkleParticle4.ColorTint = new Color(1f, 1f, 0.2f, 1f);
				prettySparkleParticle4.LocalPosition = settings.PositionInWorld;
				prettySparkleParticle4.Rotation = vector3.ToRotation();
				prettySparkleParticle4.Scale = new Vector2((num6 % 2f == 0f) ? 2f : 4f, 0.5f) * 0.7f;
				prettySparkleParticle4.FadeInNormalizedTime = 5E-06f;
				prettySparkleParticle4.FadeOutNormalizedTime = 0.95f;
				prettySparkleParticle4.TimeToLive = num;
				prettySparkleParticle4.FadeOutEnd = num;
				prettySparkleParticle4.FadeInEnd = num / 2f;
				prettySparkleParticle4.FadeOutStart = num / 2f;
				prettySparkleParticle4.Velocity = vector3 * 0.2f;
				prettySparkleParticle4.DrawVerticalAxis = false;
				if (num6 % 2f == 1f)
				{
					prettySparkleParticle4.Scale *= 1.5f;
					prettySparkleParticle4.Velocity *= 1.5f;
				}
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle4);
				for (int j = 0; j < 1; j++)
				{
					if (Main.rand.Next(2) != 0)
					{
						Dust dust3 = Dust.NewDustPerfect(settings.PositionInWorld, 169, new Vector2?(vector3.RotatedBy((double)(Main.rand.NextFloatDirection() * 6.2831855f * 0.025f), default(Vector2)) * Main.rand.NextFloat()), 0, default(Color), 1f);
						dust3.noGravity = true;
						dust3.scale = 1.4f;
						Dust dust4 = Dust.NewDustPerfect(settings.PositionInWorld, 169, new Vector2?(-vector3.RotatedBy((double)(Main.rand.NextFloatDirection() * 6.2831855f * 0.025f), default(Vector2)) * Main.rand.NextFloat()), 0, default(Color), 1f);
						dust4.noGravity = true;
						dust4.scale = 1.4f;
					}
				}
			}
		}

		// Token: 0x0600311D RID: 12573 RVA: 0x005C5798 File Offset: 0x005C3998
		private static void Spawn_BestReforge(ParticleOrchestraSettings settings)
		{
			Vector2 accelerationPerFrame = new Vector2(0f, 0.16350001f);
			Asset<Texture2D> textureAsset = Main.Assets.Request<Texture2D>("Images/UI/Creative/Research_Spark", 1);
			for (int i = 0; i < 8; i++)
			{
				Vector2 vector = Main.rand.NextVector2Circular(3f, 4f);
				if (vector.Y > 0f)
				{
					vector.Y = -vector.Y;
				}
				vector.Y -= 2f;
				Main.ParticleSystem_World_OverPlayers.Add(new CreativeSacrificeParticle(textureAsset, null, settings.MovementVector + vector, settings.PositionInWorld)
				{
					AccelerationPerFrame = accelerationPerFrame,
					ScaleOffsetPerFrame = -0.016666668f
				});
			}
		}

		// Token: 0x0600311E RID: 12574 RVA: 0x005C5860 File Offset: 0x005C3A60
		private static void Spawn_LeafCrystalPassive(ParticleOrchestraSettings settings)
		{
			float num = 90f;
			float num2 = 6.2831855f * Main.rand.NextFloat();
			float num3 = 3f;
			for (float num4 = 0f; num4 < num3; num4 += 1f)
			{
				PrettySparkleParticle prettySparkleParticle = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
				Vector2 v = (6.2831855f / num3 * num4 + num2).ToRotationVector2() * 4f;
				prettySparkleParticle.ColorTint = new Color(0.3f, 0.6f, 0.3f, 0.5f);
				prettySparkleParticle.LocalPosition = settings.PositionInWorld;
				prettySparkleParticle.Rotation = v.ToRotation();
				prettySparkleParticle.Scale = new Vector2(4f, 1f) * 0.4f;
				prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
				prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
				prettySparkleParticle.TimeToLive = num;
				prettySparkleParticle.FadeOutEnd = num;
				prettySparkleParticle.FadeInEnd = 10f;
				prettySparkleParticle.FadeOutStart = 10f;
				prettySparkleParticle.AdditiveAmount = 0.5f;
				prettySparkleParticle.Velocity = Vector2.Zero;
				prettySparkleParticle.DrawVerticalAxis = false;
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
			}
		}

		// Token: 0x0600311F RID: 12575 RVA: 0x005C5994 File Offset: 0x005C3B94
		private static void Spawn_LeafCrystalShot(ParticleOrchestraSettings settings)
		{
			int num = 30;
			PrettySparkleParticle prettySparkleParticle = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
			Vector2 movementVector = settings.MovementVector;
			Color color = Main.hslToRgb((float)settings.UniqueInfoPiece / 255f, 1f, 0.5f, byte.MaxValue);
			color = Color.Lerp(color, Color.Gold, (float)color.R / 255f * 0.5f);
			prettySparkleParticle.ColorTint = color;
			prettySparkleParticle.LocalPosition = settings.PositionInWorld;
			prettySparkleParticle.Rotation = movementVector.ToRotation();
			prettySparkleParticle.Scale = new Vector2(4f, 1f) * 1f;
			prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle.FadeOutNormalizedTime = 1f;
			prettySparkleParticle.TimeToLive = (float)num;
			prettySparkleParticle.FadeOutEnd = (float)num;
			prettySparkleParticle.FadeInEnd = (float)(num / 2);
			prettySparkleParticle.FadeOutStart = (float)(num / 2);
			prettySparkleParticle.AdditiveAmount = 0.5f;
			prettySparkleParticle.Velocity = settings.MovementVector;
			prettySparkleParticle.LocalPosition -= prettySparkleParticle.Velocity * 4f;
			prettySparkleParticle.DrawVerticalAxis = false;
			Lighting.AddLight(settings.PositionInWorld, new Vector3(0.05f, 0.2f, 0.1f) * 1.5f);
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
		}

		// Token: 0x06003120 RID: 12576 RVA: 0x005C5AE4 File Offset: 0x005C3CE4
		private static void Spawn_TrueNightsEdge(ParticleOrchestraSettings settings)
		{
			float num = 30f;
			float num2 = 0f;
			for (float num3 = 0f; num3 < 3f; num3 += 2f)
			{
				PrettySparkleParticle prettySparkleParticle = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
				Vector2 vector = (0.7853982f + 0.7853982f * num3 + num2).ToRotationVector2() * 4f;
				prettySparkleParticle.ColorTint = new Color(0.3f, 0.6f, 0.3f, 0.5f);
				prettySparkleParticle.LocalPosition = settings.PositionInWorld;
				prettySparkleParticle.Rotation = vector.ToRotation();
				prettySparkleParticle.Scale = new Vector2(4f, 1f) * 1.1f;
				prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
				prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
				prettySparkleParticle.TimeToLive = num;
				prettySparkleParticle.FadeOutEnd = num;
				prettySparkleParticle.FadeInEnd = num / 2f;
				prettySparkleParticle.FadeOutStart = num / 2f;
				prettySparkleParticle.AdditiveAmount = 0.35f;
				prettySparkleParticle.LocalPosition -= vector * num * 0.25f;
				prettySparkleParticle.Velocity = vector;
				prettySparkleParticle.DrawVerticalAxis = false;
				if (num3 == 1f)
				{
					prettySparkleParticle.Scale *= 1.5f;
					prettySparkleParticle.Velocity *= 1.5f;
					prettySparkleParticle.LocalPosition -= prettySparkleParticle.Velocity * 4f;
				}
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
			}
			for (float num4 = 0f; num4 < 3f; num4 += 2f)
			{
				PrettySparkleParticle prettySparkleParticle2 = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
				Vector2 vector2 = (0.7853982f + 0.7853982f * num4 + num2).ToRotationVector2() * 4f;
				prettySparkleParticle2.ColorTint = new Color(0.6f, 1f, 0.2f, 1f);
				prettySparkleParticle2.LocalPosition = settings.PositionInWorld;
				prettySparkleParticle2.Rotation = vector2.ToRotation();
				prettySparkleParticle2.Scale = new Vector2(4f, 1f) * 0.7f;
				prettySparkleParticle2.FadeInNormalizedTime = 5E-06f;
				prettySparkleParticle2.FadeOutNormalizedTime = 0.95f;
				prettySparkleParticle2.TimeToLive = num;
				prettySparkleParticle2.FadeOutEnd = num;
				prettySparkleParticle2.FadeInEnd = num / 2f;
				prettySparkleParticle2.FadeOutStart = num / 2f;
				prettySparkleParticle2.LocalPosition -= vector2 * num * 0.25f;
				prettySparkleParticle2.Velocity = vector2;
				prettySparkleParticle2.DrawVerticalAxis = false;
				if (num4 == 1f)
				{
					prettySparkleParticle2.Scale *= 1.5f;
					prettySparkleParticle2.Velocity *= 1.5f;
					prettySparkleParticle2.LocalPosition -= prettySparkleParticle2.Velocity * 4f;
				}
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle2);
				for (int i = 0; i < 2; i++)
				{
					Dust dust = Dust.NewDustPerfect(settings.PositionInWorld, 75, new Vector2?(vector2.RotatedBy((double)(Main.rand.NextFloatDirection() * 6.2831855f * 0.025f), default(Vector2)) * Main.rand.NextFloat()), 0, default(Color), 1f);
					dust.noGravity = true;
					dust.scale = 1.4f;
					Dust dust2 = Dust.NewDustPerfect(settings.PositionInWorld, 75, new Vector2?(-vector2.RotatedBy((double)(Main.rand.NextFloatDirection() * 6.2831855f * 0.025f), default(Vector2)) * Main.rand.NextFloat()), 0, default(Color), 1f);
					dust2.noGravity = true;
					dust2.scale = 1.4f;
				}
			}
		}

		// Token: 0x06003121 RID: 12577 RVA: 0x005C5EF0 File Offset: 0x005C40F0
		private static void Spawn_NightsEdge(ParticleOrchestraSettings settings)
		{
			float num = 30f;
			float num2 = 0f;
			for (float num3 = 0f; num3 < 3f; num3 += 1f)
			{
				PrettySparkleParticle prettySparkleParticle = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
				Vector2 vector = (0.7853982f + 0.7853982f * num3 + num2).ToRotationVector2() * 4f;
				prettySparkleParticle.ColorTint = new Color(0.25f, 0.1f, 0.5f, 0.5f);
				prettySparkleParticle.LocalPosition = settings.PositionInWorld;
				prettySparkleParticle.Rotation = vector.ToRotation();
				prettySparkleParticle.Scale = new Vector2(2f, 1f) * 1.1f;
				prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
				prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
				prettySparkleParticle.TimeToLive = num;
				prettySparkleParticle.FadeOutEnd = num;
				prettySparkleParticle.FadeInEnd = num / 2f;
				prettySparkleParticle.FadeOutStart = num / 2f;
				prettySparkleParticle.AdditiveAmount = 0.35f;
				prettySparkleParticle.LocalPosition -= vector * num * 0.25f;
				prettySparkleParticle.Velocity = vector;
				prettySparkleParticle.DrawVerticalAxis = false;
				if (num3 == 1f)
				{
					prettySparkleParticle.Scale *= 1.5f;
					prettySparkleParticle.Velocity *= 1.5f;
					prettySparkleParticle.LocalPosition -= prettySparkleParticle.Velocity * 4f;
				}
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
			}
			for (float num4 = 0f; num4 < 3f; num4 += 1f)
			{
				PrettySparkleParticle prettySparkleParticle2 = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
				Vector2 vector2 = (0.7853982f + 0.7853982f * num4 + num2).ToRotationVector2() * 4f;
				prettySparkleParticle2.ColorTint = new Color(0.5f, 0.25f, 1f, 1f);
				prettySparkleParticle2.LocalPosition = settings.PositionInWorld;
				prettySparkleParticle2.Rotation = vector2.ToRotation();
				prettySparkleParticle2.Scale = new Vector2(2f, 1f) * 0.7f;
				prettySparkleParticle2.FadeInNormalizedTime = 5E-06f;
				prettySparkleParticle2.FadeOutNormalizedTime = 0.95f;
				prettySparkleParticle2.TimeToLive = num;
				prettySparkleParticle2.FadeOutEnd = num;
				prettySparkleParticle2.FadeInEnd = num / 2f;
				prettySparkleParticle2.FadeOutStart = num / 2f;
				prettySparkleParticle2.LocalPosition -= vector2 * num * 0.25f;
				prettySparkleParticle2.Velocity = vector2;
				prettySparkleParticle2.DrawVerticalAxis = false;
				if (num4 == 1f)
				{
					prettySparkleParticle2.Scale *= 1.5f;
					prettySparkleParticle2.Velocity *= 1.5f;
					prettySparkleParticle2.LocalPosition -= prettySparkleParticle2.Velocity * 4f;
				}
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle2);
			}
		}

		// Token: 0x06003122 RID: 12578 RVA: 0x005C6210 File Offset: 0x005C4410
		private static void Spawn_SilverBulletSparkle(ParticleOrchestraSettings settings)
		{
			float num = Main.rand.NextFloat() * 6.2831855f;
			Vector2 movementVector = settings.MovementVector;
			Vector2 vector = new Vector2(Main.rand.NextFloat() * 0.2f + 0.4f);
			Main.rand.NextFloat();
			float rotation = 1.5707964f;
			Vector2 value = Main.rand.NextVector2Circular(4f, 4f) * vector;
			PrettySparkleParticle prettySparkleParticle = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
			prettySparkleParticle.AccelerationPerFrame = -movementVector * 1f / 30f;
			prettySparkleParticle.Velocity = movementVector;
			prettySparkleParticle.ColorTint = Color.White;
			prettySparkleParticle.LocalPosition = settings.PositionInWorld + value;
			prettySparkleParticle.Rotation = rotation;
			prettySparkleParticle.Scale = vector;
			prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
			prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
			prettySparkleParticle.FadeInEnd = 10f;
			prettySparkleParticle.FadeOutStart = 20f;
			prettySparkleParticle.FadeOutEnd = 30f;
			prettySparkleParticle.TimeToLive = 30f;
			Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
		}

		// Token: 0x06003123 RID: 12579 RVA: 0x005C6338 File Offset: 0x005C4538
		private static void Spawn_PaladinsHammer(ParticleOrchestraSettings settings)
		{
			float num = Main.rand.NextFloat() * 6.2831855f;
			float num2 = 1f;
			for (float num3 = 0f; num3 < 1f; num3 += 1f / num2)
			{
				float scaleFactor = 0.6f + Main.rand.NextFloat() * 0.35f;
				Vector2 vector = settings.MovementVector * scaleFactor;
				Vector2 vector2 = new Vector2(Main.rand.NextFloat() * 0.4f + 0.2f);
				float f = num + Main.rand.NextFloat() * 6.2831855f;
				float rotation = 1.5707964f;
				0.1f * vector2;
				Vector2 value = Main.rand.NextVector2Circular(12f, 12f) * vector2;
				PrettySparkleParticle prettySparkleParticle = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
				prettySparkleParticle.AccelerationPerFrame = -vector * 1f / 30f;
				prettySparkleParticle.Velocity = vector + f.ToRotationVector2() * 2f * scaleFactor;
				prettySparkleParticle.ColorTint = new Color(1f, 0.8f, 0.4f, 0f);
				prettySparkleParticle.LocalPosition = settings.PositionInWorld + value;
				prettySparkleParticle.Rotation = rotation;
				prettySparkleParticle.Scale = vector2;
				prettySparkleParticle.FadeInNormalizedTime = 5E-06f;
				prettySparkleParticle.FadeOutNormalizedTime = 0.95f;
				prettySparkleParticle.TimeToLive = 40f;
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
				prettySparkleParticle = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
				prettySparkleParticle.AccelerationPerFrame = -vector * 1f / 30f;
				prettySparkleParticle.Velocity = vector * 0.8f + f.ToRotationVector2() * 2f;
				prettySparkleParticle.ColorTint = new Color(255, 255, 255, 0);
				prettySparkleParticle.LocalPosition = settings.PositionInWorld + value;
				prettySparkleParticle.Rotation = rotation;
				prettySparkleParticle.Scale = vector2 * 0.6f;
				prettySparkleParticle.FadeInNormalizedTime = 0.1f;
				prettySparkleParticle.FadeOutNormalizedTime = 0.9f;
				prettySparkleParticle.TimeToLive = 60f;
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
			}
			for (int i = 0; i < 2; i++)
			{
				Color newColor = new Color(1f, 0.7f, 0.3f, 0f);
				int num4 = Dust.NewDust(settings.PositionInWorld, 0, 0, 267, 0f, 0f, 0, newColor, 1f);
				Main.dust[num4].velocity = Main.rand.NextVector2Circular(2f, 2f);
				Main.dust[num4].velocity += settings.MovementVector * (0.5f + 0.5f * Main.rand.NextFloat()) * 1.4f;
				Main.dust[num4].noGravity = true;
				Main.dust[num4].scale = 0.1f;
				Main.dust[num4].position += Main.rand.NextVector2Circular(16f, 16f);
				Main.dust[num4].velocity = settings.MovementVector;
				if (num4 != 6000)
				{
					Dust dust = Dust.CloneDust(num4);
					dust.scale /= 2f;
					dust.fadeIn *= 0.75f;
					dust.color = new Color(255, 255, 255, 255);
				}
			}
		}

		// Token: 0x06003124 RID: 12580 RVA: 0x005C670C File Offset: 0x005C490C
		private static void Spawn_PrincessWeapon(ParticleOrchestraSettings settings)
		{
			float num = Main.rand.NextFloat() * 6.2831855f;
			float num2 = 1f;
			for (float num3 = 0f; num3 < 1f; num3 += 1f / num2)
			{
				Vector2 vector = settings.MovementVector * (0.6f + Main.rand.NextFloat() * 0.35f);
				Vector2 vector2 = new Vector2(Main.rand.NextFloat() * 0.4f + 0.2f);
				float f = num + Main.rand.NextFloat() * 6.2831855f;
				float rotation = 1.5707964f;
				Vector2 vector3 = 0.1f * vector2;
				float divider = 60f;
				Vector2 value = Main.rand.NextVector2Circular(8f, 8f) * vector2;
				PrettySparkleParticle prettySparkleParticle = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
				prettySparkleParticle.Velocity = f.ToRotationVector2() * vector3 + vector;
				prettySparkleParticle.AccelerationPerFrame = f.ToRotationVector2() * -(vector3 / divider) - vector * 1f / 30f;
				prettySparkleParticle.AccelerationPerFrame = -vector * 1f / 60f;
				prettySparkleParticle.Velocity = vector * 0.66f;
				prettySparkleParticle.ColorTint = Main.hslToRgb((0.92f + Main.rand.NextFloat() * 0.02f) % 1f, 1f, 0.4f + Main.rand.NextFloat() * 0.25f, byte.MaxValue);
				prettySparkleParticle.ColorTint.A = 0;
				prettySparkleParticle.LocalPosition = settings.PositionInWorld + value;
				prettySparkleParticle.Rotation = rotation;
				prettySparkleParticle.Scale = vector2;
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
				prettySparkleParticle = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
				prettySparkleParticle.Velocity = f.ToRotationVector2() * vector3 + vector;
				prettySparkleParticle.AccelerationPerFrame = f.ToRotationVector2() * -(vector3 / divider) - vector * 1f / 15f;
				prettySparkleParticle.AccelerationPerFrame = -vector * 1f / 60f;
				prettySparkleParticle.Velocity = vector * 0.66f;
				prettySparkleParticle.ColorTint = new Color(255, 255, 255, 0);
				prettySparkleParticle.LocalPosition = settings.PositionInWorld + value;
				prettySparkleParticle.Rotation = rotation;
				prettySparkleParticle.Scale = vector2 * 0.6f;
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
			}
			for (int i = 0; i < 2; i++)
			{
				Color newColor = Main.hslToRgb((0.92f + Main.rand.NextFloat() * 0.02f) % 1f, 1f, 0.4f + Main.rand.NextFloat() * 0.25f, byte.MaxValue);
				int num4 = Dust.NewDust(settings.PositionInWorld, 0, 0, 267, 0f, 0f, 0, newColor, 1f);
				Main.dust[num4].velocity = Main.rand.NextVector2Circular(2f, 2f);
				Main.dust[num4].velocity += settings.MovementVector * (0.5f + 0.5f * Main.rand.NextFloat()) * 1.4f;
				Main.dust[num4].noGravity = true;
				Main.dust[num4].scale = 0.1f;
				Main.dust[num4].position += Main.rand.NextVector2Circular(16f, 16f);
				Main.dust[num4].velocity = settings.MovementVector;
				if (num4 != 6000)
				{
					Dust dust = Dust.CloneDust(num4);
					dust.scale /= 2f;
					dust.fadeIn *= 0.75f;
					dust.color = new Color(255, 255, 255, 255);
				}
			}
		}

		// Token: 0x06003125 RID: 12581 RVA: 0x005C6B7C File Offset: 0x005C4D7C
		private static void Spawn_StardustPunch(ParticleOrchestraSettings settings)
		{
			float num = Main.rand.NextFloat() * 6.2831855f;
			float num2 = 1f;
			for (float num3 = 0f; num3 < 1f; num3 += 1f / num2)
			{
				Vector2 vector = settings.MovementVector * (0.3f + Main.rand.NextFloat() * 0.35f);
				Vector2 vector2 = new Vector2(Main.rand.NextFloat() * 0.4f + 0.4f);
				float f = num + Main.rand.NextFloat() * 6.2831855f;
				float rotation = 1.5707964f;
				Vector2 vector3 = 0.1f * vector2;
				float divider = 60f;
				Vector2 value = Main.rand.NextVector2Circular(8f, 8f) * vector2;
				PrettySparkleParticle prettySparkleParticle = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
				prettySparkleParticle.Velocity = f.ToRotationVector2() * vector3 + vector;
				prettySparkleParticle.AccelerationPerFrame = f.ToRotationVector2() * -(vector3 / divider) - vector * 1f / 60f;
				prettySparkleParticle.ColorTint = Main.hslToRgb((0.6f + Main.rand.NextFloat() * 0.05f) % 1f, 1f, 0.4f + Main.rand.NextFloat() * 0.25f, byte.MaxValue);
				prettySparkleParticle.ColorTint.A = 0;
				prettySparkleParticle.LocalPosition = settings.PositionInWorld + value;
				prettySparkleParticle.Rotation = rotation;
				prettySparkleParticle.Scale = vector2;
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
				prettySparkleParticle = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
				prettySparkleParticle.Velocity = f.ToRotationVector2() * vector3 + vector;
				prettySparkleParticle.AccelerationPerFrame = f.ToRotationVector2() * -(vector3 / divider) - vector * 1f / 30f;
				prettySparkleParticle.ColorTint = new Color(255, 255, 255, 0);
				prettySparkleParticle.LocalPosition = settings.PositionInWorld + value;
				prettySparkleParticle.Rotation = rotation;
				prettySparkleParticle.Scale = vector2 * 0.6f;
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
			}
			for (int i = 0; i < 2; i++)
			{
				Color newColor = Main.hslToRgb((0.59f + Main.rand.NextFloat() * 0.05f) % 1f, 1f, 0.4f + Main.rand.NextFloat() * 0.25f, byte.MaxValue);
				int num4 = Dust.NewDust(settings.PositionInWorld, 0, 0, 267, 0f, 0f, 0, newColor, 1f);
				Main.dust[num4].velocity = Main.rand.NextVector2Circular(2f, 2f);
				Main.dust[num4].velocity += settings.MovementVector * (0.5f + 0.5f * Main.rand.NextFloat()) * 1.4f;
				Main.dust[num4].noGravity = true;
				Main.dust[num4].scale = 0.6f + Main.rand.NextFloat() * 2f;
				Main.dust[num4].position += Main.rand.NextVector2Circular(16f, 16f);
				if (num4 != 6000)
				{
					Dust dust = Dust.CloneDust(num4);
					dust.scale /= 2f;
					dust.fadeIn *= 0.75f;
					dust.color = new Color(255, 255, 255, 255);
				}
			}
		}

		// Token: 0x06003126 RID: 12582 RVA: 0x005C6F84 File Offset: 0x005C5184
		private static void Spawn_RainbowRodHit(ParticleOrchestraSettings settings)
		{
			float num = Main.rand.NextFloat() * 6.2831855f;
			float num2 = 6f;
			float num3 = Main.rand.NextFloat();
			for (float num4 = 0f; num4 < 1f; num4 += 1f / num2)
			{
				Vector2 vector = settings.MovementVector * Main.rand.NextFloatDirection() * 0.15f;
				Vector2 vector2 = new Vector2(Main.rand.NextFloat() * 0.4f + 0.4f);
				float f = num + Main.rand.NextFloat() * 6.2831855f;
				float rotation = 1.5707964f;
				Vector2 vector3 = 1.5f * vector2;
				float divider = 60f;
				Vector2 value = Main.rand.NextVector2Circular(8f, 8f) * vector2;
				PrettySparkleParticle prettySparkleParticle = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
				prettySparkleParticle.Velocity = f.ToRotationVector2() * vector3 + vector;
				prettySparkleParticle.AccelerationPerFrame = f.ToRotationVector2() * -(vector3 / divider) - vector * 1f / 60f;
				prettySparkleParticle.ColorTint = Main.hslToRgb((num3 + Main.rand.NextFloat() * 0.33f) % 1f, 1f, 0.4f + Main.rand.NextFloat() * 0.25f, byte.MaxValue);
				prettySparkleParticle.ColorTint.A = 0;
				prettySparkleParticle.LocalPosition = settings.PositionInWorld + value;
				prettySparkleParticle.Rotation = rotation;
				prettySparkleParticle.Scale = vector2;
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
				prettySparkleParticle = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
				prettySparkleParticle.Velocity = f.ToRotationVector2() * vector3 + vector;
				prettySparkleParticle.AccelerationPerFrame = f.ToRotationVector2() * -(vector3 / divider) - vector * 1f / 60f;
				prettySparkleParticle.ColorTint = new Color(255, 255, 255, 0);
				prettySparkleParticle.LocalPosition = settings.PositionInWorld + value;
				prettySparkleParticle.Rotation = rotation;
				prettySparkleParticle.Scale = vector2 * 0.6f;
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
			}
			for (int i = 0; i < 12; i++)
			{
				Color newColor = Main.hslToRgb((num3 + Main.rand.NextFloat() * 0.12f) % 1f, 1f, 0.4f + Main.rand.NextFloat() * 0.25f, byte.MaxValue);
				int num5 = Dust.NewDust(settings.PositionInWorld, 0, 0, 267, 0f, 0f, 0, newColor, 1f);
				Main.dust[num5].velocity = Main.rand.NextVector2Circular(1f, 1f);
				Main.dust[num5].velocity += settings.MovementVector * Main.rand.NextFloatDirection() * 0.5f;
				Main.dust[num5].noGravity = true;
				Main.dust[num5].scale = 0.6f + Main.rand.NextFloat() * 0.9f;
				Main.dust[num5].fadeIn = 0.7f + Main.rand.NextFloat() * 0.8f;
				if (num5 != 6000)
				{
					Dust dust = Dust.CloneDust(num5);
					dust.scale /= 2f;
					dust.fadeIn *= 0.75f;
					dust.color = new Color(255, 255, 255, 255);
				}
			}
		}

		// Token: 0x06003127 RID: 12583 RVA: 0x005C737C File Offset: 0x005C557C
		private static void Spawn_BlackLightningSmall(ParticleOrchestraSettings settings)
		{
			float num = Main.rand.NextFloat() * 6.2831855f;
			float num2 = (float)Main.rand.Next(1, 3);
			float scaleFactor = 0.7f;
			int num3 = 916;
			Main.instance.LoadProjectile(num3);
			Color value = new Color(255, 255, 255, 255);
			Color indigo = Color.Indigo;
			indigo.A = 0;
			for (float num4 = 0f; num4 < 1f; num4 += 1f / num2)
			{
				float f = 6.2831855f * num4 + num + Main.rand.NextFloatDirection() * 0.25f;
				float scaleFactor2 = Main.rand.NextFloat() * 4f + 0.1f;
				Vector2 vector = Main.rand.NextVector2Circular(12f, 12f) * scaleFactor;
				Color.Lerp(Color.Lerp(Color.Black, indigo, Main.rand.NextFloat() * 0.5f), value, Main.rand.NextFloat() * 0.6f);
				Color colorTint = new Color(0, 0, 0, 255);
				int num5 = Main.rand.Next(4);
				if (num5 == 1)
				{
					colorTint = Color.Lerp(new Color(106, 90, 205, 127), Color.Black, 0.1f + 0.7f * Main.rand.NextFloat());
				}
				if (num5 == 2)
				{
					colorTint = Color.Lerp(new Color(106, 90, 205, 60), Color.Black, 0.1f + 0.8f * Main.rand.NextFloat());
				}
				RandomizedFrameParticle randomizedFrameParticle = ParticleOrchestrator._poolRandomizedFrame.RequestParticle();
				randomizedFrameParticle.SetBasicInfo(TextureAssets.Projectile[num3], null, Vector2.Zero, vector);
				randomizedFrameParticle.SetTypeInfo(Main.projFrames[num3], 2, 24f);
				randomizedFrameParticle.Velocity = f.ToRotationVector2() * scaleFactor2 * new Vector2(1f, 0.5f) * 0.2f + settings.MovementVector;
				randomizedFrameParticle.ColorTint = colorTint;
				randomizedFrameParticle.LocalPosition = settings.PositionInWorld + vector;
				randomizedFrameParticle.Rotation = randomizedFrameParticle.Velocity.ToRotation();
				randomizedFrameParticle.Scale = Vector2.One * 0.5f;
				randomizedFrameParticle.FadeInNormalizedTime = 0.01f;
				randomizedFrameParticle.FadeOutNormalizedTime = 0.5f;
				randomizedFrameParticle.ScaleVelocity = new Vector2(0.025f);
				Main.ParticleSystem_World_OverPlayers.Add(randomizedFrameParticle);
			}
		}

		// Token: 0x06003128 RID: 12584 RVA: 0x005C7610 File Offset: 0x005C5810
		private static void Spawn_BlackLightningHit(ParticleOrchestraSettings settings)
		{
			float num = Main.rand.NextFloat() * 6.2831855f;
			float num2 = 7f;
			float scaleFactor = 0.7f;
			int num3 = 916;
			Main.instance.LoadProjectile(num3);
			Color value = new Color(255, 255, 255, 255);
			Color indigo = Color.Indigo;
			indigo.A = 0;
			for (float num4 = 0f; num4 < 1f; num4 += 1f / num2)
			{
				float num5 = 6.2831855f * num4 + num + Main.rand.NextFloatDirection() * 0.25f;
				float scaleFactor2 = Main.rand.NextFloat() * 4f + 0.1f;
				Vector2 vector = Main.rand.NextVector2Circular(12f, 12f) * scaleFactor;
				Color.Lerp(Color.Lerp(Color.Black, indigo, Main.rand.NextFloat() * 0.5f), value, Main.rand.NextFloat() * 0.6f);
				Color colorTint = new Color(0, 0, 0, 255);
				int num6 = Main.rand.Next(4);
				if (num6 == 1)
				{
					colorTint = Color.Lerp(new Color(106, 90, 205, 127), Color.Black, 0.1f + 0.7f * Main.rand.NextFloat());
				}
				if (num6 == 2)
				{
					colorTint = Color.Lerp(new Color(106, 90, 205, 60), Color.Black, 0.1f + 0.8f * Main.rand.NextFloat());
				}
				RandomizedFrameParticle randomizedFrameParticle = ParticleOrchestrator._poolRandomizedFrame.RequestParticle();
				randomizedFrameParticle.SetBasicInfo(TextureAssets.Projectile[num3], null, Vector2.Zero, vector);
				randomizedFrameParticle.SetTypeInfo(Main.projFrames[num3], 2, 24f);
				randomizedFrameParticle.Velocity = num5.ToRotationVector2() * scaleFactor2 * new Vector2(1f, 0.5f);
				randomizedFrameParticle.ColorTint = colorTint;
				randomizedFrameParticle.LocalPosition = settings.PositionInWorld + vector;
				randomizedFrameParticle.Rotation = num5;
				randomizedFrameParticle.Scale = Vector2.One;
				randomizedFrameParticle.FadeInNormalizedTime = 0.01f;
				randomizedFrameParticle.FadeOutNormalizedTime = 0.5f;
				randomizedFrameParticle.ScaleVelocity = new Vector2(0.05f);
				Main.ParticleSystem_World_OverPlayers.Add(randomizedFrameParticle);
			}
		}

		// Token: 0x06003129 RID: 12585 RVA: 0x005C7874 File Offset: 0x005C5A74
		private static void Spawn_BlueLightningSmallLong(ParticleOrchestraSettings settings)
		{
			Vector2 vector = settings.PositionInWorld;
			Vector2 value = settings.MovementVector.SafeNormalize(Vector2.Zero);
			float num = settings.MovementVector.Length();
			int num2 = 25;
			int num3 = 40;
			if (num < (float)num3)
			{
				return;
			}
			float num4 = 0f;
			while (num4 < num)
			{
				ParticleOrchestrator.Spawn_BlueLightningSmall(new ParticleOrchestraSettings
				{
					PositionInWorld = vector,
					MovementVector = value * 2f
				});
				num4 += (float)num2;
				vector += value * (float)num2;
			}
		}

		// Token: 0x0600312A RID: 12586 RVA: 0x005C7904 File Offset: 0x005C5B04
		private static void Spawn_BlueLightningSmall(ParticleOrchestraSettings settings)
		{
			float num = Main.rand.NextFloat() * 6.2831855f;
			float num2 = (float)Main.rand.Next(1, 3);
			Main.rand.Next(1, 3);
			float scaleFactor = 0.7f;
			short num3 = 916;
			Main.instance.LoadProjectile((int)num3);
			Color value = new Color(133, 255, 255, 0);
			Color value2 = new Color(15, 100, 155, 0);
			Lighting.AddLight(settings.PositionInWorld, new Vector3(0.4f, 0.8f, 1f) * 0.7f);
			for (float num4 = 0f; num4 < 1f; num4 += 1f / num2)
			{
				float f = 6.2831855f * num4 + num + Main.rand.NextFloatDirection() * 0.25f;
				float scaleFactor2 = Main.rand.NextFloat() * 4f + 0.1f;
				Vector2 vector = Main.rand.NextVector2Circular(6f, 6f) * scaleFactor;
				Color colorTint = Color.Lerp(value, value2, Main.rand.NextFloat());
				RandomizedFrameParticle randomizedFrameParticle = ParticleOrchestrator._poolRandomizedFrame.RequestParticle();
				randomizedFrameParticle.SetBasicInfo(TextureAssets.Projectile[(int)num3], null, Vector2.Zero, vector);
				randomizedFrameParticle.SetTypeInfo(Main.projFrames[(int)num3], 2, 10f);
				randomizedFrameParticle.Velocity = f.ToRotationVector2() * scaleFactor2 * new Vector2(1f, 0.5f) * 0.2f + settings.MovementVector;
				randomizedFrameParticle.ColorTint = colorTint;
				randomizedFrameParticle.LocalPosition = settings.PositionInWorld + vector;
				randomizedFrameParticle.Rotation = randomizedFrameParticle.Velocity.ToRotation();
				randomizedFrameParticle.Scale = Vector2.One * 1f;
				randomizedFrameParticle.FadeInNormalizedTime = 0.01f;
				randomizedFrameParticle.FadeOutNormalizedTime = 0.5f;
				randomizedFrameParticle.ScaleVelocity = new Vector2(0.025f);
				Main.ParticleSystem_World_OverPlayers.Add(randomizedFrameParticle);
				if (Main.rand.Next(2) == 0)
				{
					Dust dust = Dust.NewDustPerfect(randomizedFrameParticle.LocalPosition, 226, new Vector2?(randomizedFrameParticle.Velocity), 0, default(Color), 1f);
					dust.scale = 0.8f;
					dust.noGravity = true;
					dust.velocity = Main.rand.NextVector2Circular(4f, 4f);
					dust.scale *= 1.25f;
				}
			}
		}

		// Token: 0x0600312B RID: 12587 RVA: 0x005C7BA0 File Offset: 0x005C5DA0
		private static void Spawn_StellarTune(ParticleOrchestraSettings settings)
		{
			float num = Main.rand.NextFloat() * 6.2831855f;
			float num2 = 5f;
			Vector2 vector = new Vector2(0.7f);
			for (float num3 = 0f; num3 < 1f; num3 += 1f / num2)
			{
				float num4 = 6.2831855f * num3 + num + Main.rand.NextFloatDirection() * 0.25f;
				Vector2 vector2 = 1.5f * vector;
				float divider = 60f;
				Vector2 value = Main.rand.NextVector2Circular(12f, 12f) * vector;
				Color colorTint = Color.Lerp(Color.Gold, Color.HotPink, Main.rand.NextFloat());
				if (Main.rand.Next(2) == 0)
				{
					colorTint = Color.Lerp(Color.Violet, Color.HotPink, Main.rand.NextFloat());
				}
				PrettySparkleParticle prettySparkleParticle = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
				prettySparkleParticle.Velocity = num4.ToRotationVector2() * vector2;
				prettySparkleParticle.AccelerationPerFrame = num4.ToRotationVector2() * -(vector2 / divider);
				prettySparkleParticle.ColorTint = colorTint;
				prettySparkleParticle.LocalPosition = settings.PositionInWorld + value;
				prettySparkleParticle.Rotation = num4;
				prettySparkleParticle.Scale = vector * (Main.rand.NextFloat() * 0.8f + 0.2f);
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
			}
		}

		// Token: 0x0600312C RID: 12588 RVA: 0x005C7D1C File Offset: 0x005C5F1C
		private static void Spawn_CattivaHit(ParticleOrchestraSettings settings)
		{
			float num = Main.rand.NextFloat() * 6.2831855f;
			float num2 = 6f;
			Vector2 vector = new Vector2(0.7f);
			for (float num3 = 0f; num3 < 1f; num3 += 1f / num2)
			{
				float num4 = 6.2831855f * num3 + num + Main.rand.NextFloatDirection() * 0.25f;
				Vector2 vector2 = 4.5f * vector;
				float num5 = 16f;
				Vector2 value = Main.rand.NextVector2Circular(6f, 6f) * vector;
				Color colorTint = Color.Lerp(Color.Gold, Color.Orange, Main.rand.NextFloat());
				if (Main.rand.Next(2) == 0)
				{
					colorTint = Color.Lerp(Color.White, Color.Orange, Main.rand.NextFloat());
				}
				PrettySparkleParticle prettySparkleParticle = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
				prettySparkleParticle.Velocity = num4.ToRotationVector2() * vector2;
				prettySparkleParticle.AccelerationPerFrame = num4.ToRotationVector2() * -(vector2 / num5) * 0.5f;
				prettySparkleParticle.ColorTint = colorTint;
				prettySparkleParticle.LocalPosition = settings.PositionInWorld + value;
				prettySparkleParticle.Rotation = num4;
				prettySparkleParticle.Scale = vector * (Main.rand.NextFloat() * 0.7f + 0.1f);
				prettySparkleParticle.DrawVerticalAxis = false;
				prettySparkleParticle.FadeInEnd = 0.1f;
				prettySparkleParticle.FadeOutNormalizedTime = 0.5f;
				prettySparkleParticle.TimeToLive = num5;
				PrettySparkleParticle prettySparkleParticle2 = prettySparkleParticle;
				prettySparkleParticle2.Scale.X = prettySparkleParticle2.Scale.X * 4f;
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
			}
		}

		// Token: 0x0600312D RID: 12589 RVA: 0x005C7EE0 File Offset: 0x005C60E0
		private static void Spawn_Keybrand(ParticleOrchestraSettings settings)
		{
			float num = Main.rand.NextFloat() * 6.2831855f;
			float num2 = 3f;
			Vector2 vector = new Vector2(0.7f);
			for (float num3 = 0f; num3 < 1f; num3 += 1f / num2)
			{
				float num4 = 6.2831855f * num3 + num + Main.rand.NextFloatDirection() * 0.1f;
				Vector2 vector2 = 1.5f * vector;
				float divider = 60f;
				Vector2 value = Main.rand.NextVector2Circular(4f, 4f) * vector;
				PrettySparkleParticle prettySparkleParticle = ParticleOrchestrator._poolPrettySparkle.RequestParticle();
				prettySparkleParticle.Velocity = num4.ToRotationVector2() * vector2;
				prettySparkleParticle.AccelerationPerFrame = num4.ToRotationVector2() * -(vector2 / divider);
				prettySparkleParticle.ColorTint = Color.Lerp(Color.Gold, Color.OrangeRed, Main.rand.NextFloat());
				prettySparkleParticle.LocalPosition = settings.PositionInWorld + value;
				prettySparkleParticle.Rotation = num4;
				prettySparkleParticle.Scale = vector * 0.8f;
				Main.ParticleSystem_World_OverPlayers.Add(prettySparkleParticle);
			}
			num += 1f / num2 / 2f * 6.2831855f;
			num = Main.rand.NextFloat() * 6.2831855f;
			for (float num5 = 0f; num5 < 1f; num5 += 1f / num2)
			{
				float num6 = 6.2831855f * num5 + num + Main.rand.NextFloatDirection() * 0.1f;
				Vector2 vector3 = 1f * vector;
				float num7 = 30f;
				Color color = Color.Lerp(Color.Gold, Color.OrangeRed, Main.rand.NextFloat());
				color = Color.Lerp(Color.White, color, 0.5f);
				color.A = 0;
				Vector2 value2 = Main.rand.NextVector2Circular(4f, 4f) * vector;
				FadingParticle fadingParticle = ParticleOrchestrator._poolFading.RequestParticle();
				fadingParticle.SetBasicInfo(TextureAssets.Extra[98], null, Vector2.Zero, Vector2.Zero);
				fadingParticle.SetTypeInfo(num7, true);
				fadingParticle.Velocity = num6.ToRotationVector2() * vector3;
				fadingParticle.AccelerationPerFrame = num6.ToRotationVector2() * -(vector3 / num7);
				fadingParticle.ColorTint = color;
				fadingParticle.LocalPosition = settings.PositionInWorld + num6.ToRotationVector2() * vector3 * vector * num7 * 0.2f + value2;
				fadingParticle.Rotation = num6 + 1.5707964f;
				fadingParticle.FadeInNormalizedTime = 0.3f;
				fadingParticle.FadeOutNormalizedTime = 0.4f;
				fadingParticle.Scale = new Vector2(0.5f, 1.2f) * 0.8f * vector;
				Main.ParticleSystem_World_OverPlayers.Add(fadingParticle);
			}
			num2 = 1f;
			num = Main.rand.NextFloat() * 6.2831855f;
			for (float num8 = 0f; num8 < 1f; num8 += 1f / num2)
			{
				float num9 = 6.2831855f * num8 + num;
				float timeToLive = 30f;
				Color colorTint = Color.Lerp(Color.CornflowerBlue, Color.White, Main.rand.NextFloat());
				colorTint.A = 127;
				Vector2 value3 = Main.rand.NextVector2Circular(4f, 4f) * vector;
				Vector2 value4 = Main.rand.NextVector2Square(0.7f, 1.3f);
				FadingParticle fadingParticle2 = ParticleOrchestrator._poolFading.RequestParticle();
				fadingParticle2.SetBasicInfo(TextureAssets.Extra[174], null, Vector2.Zero, Vector2.Zero);
				fadingParticle2.SetTypeInfo(timeToLive, true);
				fadingParticle2.ColorTint = colorTint;
				fadingParticle2.LocalPosition = settings.PositionInWorld + value3;
				fadingParticle2.Rotation = num9 + 1.5707964f;
				fadingParticle2.FadeInNormalizedTime = 0.1f;
				fadingParticle2.FadeOutNormalizedTime = 0.4f;
				fadingParticle2.Scale = new Vector2(0.1f, 0.1f) * vector;
				fadingParticle2.ScaleVelocity = value4 * 1f / 60f;
				fadingParticle2.ScaleAcceleration = value4 * -0.016666668f / 60f;
				Main.ParticleSystem_World_OverPlayers.Add(fadingParticle2);
			}
		}

		// Token: 0x0600312E RID: 12590 RVA: 0x005C837C File Offset: 0x005C657C
		private static void Spawn_FlameWaders(ParticleOrchestraSettings settings)
		{
			float num = 60f;
			for (int i = -1; i <= 1; i++)
			{
				int num2 = (int)Main.rand.NextFromList(new short[]
				{
					326,
					327,
					328
				});
				Main.instance.LoadProjectile(num2);
				Player player = Main.player[(int)settings.IndexOfPlayerWhoInvokedThis];
				float scaleFactor = Main.rand.NextFloat() * 0.9f + 0.1f;
				Vector2 vector = settings.PositionInWorld + new Vector2((float)i * 5.3333335f, 0f);
				FlameParticle flameParticle = ParticleOrchestrator._poolFlame.RequestParticle();
				flameParticle.SetBasicInfo(TextureAssets.Projectile[num2], null, Vector2.Zero, vector);
				flameParticle.SetTypeInfo(num, (int)settings.IndexOfPlayerWhoInvokedThis, player.cFlameWaker);
				flameParticle.FadeOutNormalizedTime = 0.4f;
				flameParticle.ScaleAcceleration = Vector2.One * scaleFactor * -0.016666668f / num;
				flameParticle.Scale = Vector2.One * scaleFactor;
				Main.ParticleSystem_World_BehindPlayers.Add(flameParticle);
				if (Main.rand.Next(16) == 0)
				{
					Dust dust = Dust.NewDustDirect(vector, 4, 4, 6, 0f, 0f, 100, default(Color), 1f);
					if (Main.rand.Next(2) == 0)
					{
						dust.noGravity = true;
						dust.fadeIn = 1.15f;
					}
					else
					{
						dust.scale = 0.6f;
					}
					dust.velocity *= 0.6f;
					Dust dust2 = dust;
					dust2.velocity.Y = dust2.velocity.Y - 1.2f;
					dust.noLight = true;
					Dust dust3 = dust;
					dust3.position.Y = dust3.position.Y - 4f;
					dust.shader = GameShaders.Armor.GetSecondaryShader(player.cFlameWaker, player);
				}
			}
		}

		// Token: 0x0600312F RID: 12591 RVA: 0x005C8564 File Offset: 0x005C6764
		private static void Spawn_WallOfFleshGoatMountFlames(ParticleOrchestraSettings settings)
		{
			float num = 50f;
			for (int i = -1; i <= 1; i++)
			{
				int num2 = (int)Main.rand.NextFromList(new short[]
				{
					326,
					327,
					328
				});
				Main.instance.LoadProjectile(num2);
				Player player = Main.player[(int)settings.IndexOfPlayerWhoInvokedThis];
				float scaleFactor = Main.rand.NextFloat() * 0.9f + 0.1f;
				Vector2 vector = settings.PositionInWorld + new Vector2((float)i * 5.3333335f, 0f);
				FlameParticle flameParticle = ParticleOrchestrator._poolFlame.RequestParticle();
				flameParticle.SetBasicInfo(TextureAssets.Projectile[num2], null, Vector2.Zero, vector);
				flameParticle.SetTypeInfo(num, (int)settings.IndexOfPlayerWhoInvokedThis, player.cMount);
				flameParticle.FadeOutNormalizedTime = 0.3f;
				flameParticle.ScaleAcceleration = Vector2.One * scaleFactor * -0.016666668f / num;
				flameParticle.Scale = Vector2.One * scaleFactor;
				Main.ParticleSystem_World_BehindPlayers.Add(flameParticle);
				if (Main.rand.Next(8) == 0)
				{
					Dust dust = Dust.NewDustDirect(vector, 4, 4, 6, 0f, 0f, 100, default(Color), 1f);
					if (Main.rand.Next(2) == 0)
					{
						dust.noGravity = true;
						dust.fadeIn = 1.15f;
					}
					else
					{
						dust.scale = 0.6f;
					}
					dust.velocity *= 0.6f;
					Dust dust2 = dust;
					dust2.velocity.Y = dust2.velocity.Y - 1.2f;
					dust.noLight = true;
					Dust dust3 = dust;
					dust3.position.Y = dust3.position.Y - 4f;
					dust.shader = GameShaders.Armor.GetSecondaryShader(player.cMount, player);
				}
			}
		}

		// Token: 0x040056F4 RID: 22260
		internal static ParticlePool<FadingParticle> _poolFading = new ParticlePool<FadingParticle>(200, new ParticlePool<FadingParticle>.ParticleInstantiator(ParticleOrchestrator.GetNewFadingParticle));

		// Token: 0x040056F5 RID: 22261
		private static ParticlePool<FadingPlayerShaderParticle> _poolFadingPlayerShader = new ParticlePool<FadingPlayerShaderParticle>(200, new ParticlePool<FadingPlayerShaderParticle>.ParticleInstantiator(ParticleOrchestrator.GetNewFadingPlayerShaderParticle));

		// Token: 0x040056F6 RID: 22262
		private static ParticlePool<LittleFlyingCritterParticle> _poolFlies = new ParticlePool<LittleFlyingCritterParticle>(200, new ParticlePool<LittleFlyingCritterParticle>.ParticleInstantiator(ParticleOrchestrator.GetNewPooFlyParticle));

		// Token: 0x040056F7 RID: 22263
		private static ParticlePool<LittleFlyingCritterParticle> _natureFlies = new ParticlePool<LittleFlyingCritterParticle>(200, new ParticlePool<LittleFlyingCritterParticle>.ParticleInstantiator(ParticleOrchestrator.GetNewNatureFlyParticle));

		// Token: 0x040056F8 RID: 22264
		private static ParticlePool<ItemTransferParticle> _poolItemTransfer = new ParticlePool<ItemTransferParticle>(100, new ParticlePool<ItemTransferParticle>.ParticleInstantiator(ParticleOrchestrator.GetNewItemTransferParticle));

		// Token: 0x040056F9 RID: 22265
		private static ParticlePool<FakeFishParticle> _fakeFish = new ParticlePool<FakeFishParticle>(100, new ParticlePool<FakeFishParticle>.ParticleInstantiator(ParticleOrchestrator.GetNewFakeFishParticle));

		// Token: 0x040056FA RID: 22266
		private static ParticlePool<FlameParticle> _poolFlame = new ParticlePool<FlameParticle>(200, new ParticlePool<FlameParticle>.ParticleInstantiator(ParticleOrchestrator.GetNewFlameParticle));

		// Token: 0x040056FB RID: 22267
		private static ParticlePool<RandomizedFrameParticle> _poolRandomizedFrame = new ParticlePool<RandomizedFrameParticle>(200, new ParticlePool<RandomizedFrameParticle>.ParticleInstantiator(ParticleOrchestrator.GetNewRandomizedFrameParticle));

		// Token: 0x040056FC RID: 22268
		private static ParticlePool<PrettySparkleParticle> _poolPrettySparkle = new ParticlePool<PrettySparkleParticle>(200, new ParticlePool<PrettySparkleParticle>.ParticleInstantiator(ParticleOrchestrator.GetNewPrettySparkleParticle));

		// Token: 0x040056FD RID: 22269
		private static ParticlePool<GasParticle> _poolGas = new ParticlePool<GasParticle>(200, new ParticlePool<GasParticle>.ParticleInstantiator(ParticleOrchestrator.GetNewGasParticle));

		// Token: 0x040056FE RID: 22270
		private static ParticlePool<BloodyExplosionParticle> _poolBloodyExplosion = new ParticlePool<BloodyExplosionParticle>(200, new ParticlePool<BloodyExplosionParticle>.ParticleInstantiator(ParticleOrchestrator.GetNewBloodyExplosionParticle));

		// Token: 0x040056FF RID: 22271
		private static ParticlePool<ShockIconParticle> _poolShockIcon = new ParticlePool<ShockIconParticle>(200, new ParticlePool<ShockIconParticle>.ParticleInstantiator(ParticleOrchestrator.GetNewShockIconParticle));

		// Token: 0x04005700 RID: 22272
		public static ParticlePool<ItemTransferParticle> ScreenItemParticles = new ParticlePool<ItemTransferParticle>(100, new ParticlePool<ItemTransferParticle>.ParticleInstantiator(ParticleOrchestrator.GetNewItemTransferParticle_ScreenSpace));

		// Token: 0x04005701 RID: 22273
		public static ParticlePool<StormLightningParticle> StormLightningParticles = new ParticlePool<StormLightningParticle>(20, new ParticlePool<StormLightningParticle>.ParticleInstantiator(ParticleOrchestrator.GetNewStormLightningParticle));

		// Token: 0x04005702 RID: 22274
		private static SlotId[] _mushBoiExplosionSounds = new SlotId[3];
	}
}
