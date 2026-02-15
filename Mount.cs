using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.UI;
using Terraria.GameInput;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace Terraria
{
	// Token: 0x0200002F RID: 47
	public class Mount
	{
		// Token: 0x0600023E RID: 574 RVA: 0x0002E9B4 File Offset: 0x0002CBB4
		public void ApplyDummyFrameCounters()
		{
			this._frameCounter = 0f;
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0002E9C1 File Offset: 0x0002CBC1
		private static void MeowcartLandingSound(Player Player, Vector2 Position, int Width, int Height)
		{
			SoundEngine.PlaySound(37, (int)Position.X + Width / 2, (int)Position.Y + Height / 2, 5, 1f, 0f);
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0002E9EC File Offset: 0x0002CBEC
		private static void MeowcartBumperSound(Player Player, Vector2 Position, int Width, int Height)
		{
			SoundEngine.PlaySound(37, (int)Position.X + Width / 2, (int)Position.Y + Height / 2, 3, 1f, 0f);
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0002EA17 File Offset: 0x0002CC17
		public Mount()
		{
			this._debugDraw = new List<DrillDebugDraw>();
			this.Reset();
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0002EA3C File Offset: 0x0002CC3C
		public void Reset()
		{
			this._active = false;
			this._type = -1;
			this._flipDraw = false;
			this._frame = 0;
			this._frameCounter = 0f;
			this._frameExtra = 0;
			this._frameExtraCounter = 0f;
			this._frameState = 0;
			this._flyTime = 0;
			this._idleTime = 0;
			this._idleTimeNext = -1;
			this._walkingGraceTimeLeft = 0;
			this._fatigueMax = 0f;
			this._abilityCharging = false;
			this._abilityCharge = 0;
			this._aiming = false;
			this._shouldSuperCart = false;
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0002EACC File Offset: 0x0002CCCC
		public static void Initialize()
		{
			Mount.mounts = new Mount.MountData[MountID.Count];
			Mount.MountData mountData = new Mount.MountData();
			Mount.mounts[0] = mountData;
			mountData.spawnDust = 57;
			mountData.spawnDustNoGravity = false;
			mountData.buff = 90;
			mountData.heightBoost = 20;
			mountData.flightTimeMax = 160;
			mountData.runSpeed = 5.5f;
			mountData.dashSpeed = 12f;
			mountData.acceleration = 0.09f;
			mountData.jumpHeight = 17;
			mountData.jumpSpeed = 5.31f;
			mountData.totalFrames = 12;
			int[] array = new int[mountData.totalFrames];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = 30;
			}
			array[1] += 2;
			array[11] += 2;
			mountData.playerYOffsets = array;
			mountData.xOffset = 13;
			mountData.bodyFrame = 3;
			mountData.yOffset = -7;
			mountData.playerHeadOffset = 22;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 6;
			mountData.runningFrameDelay = 12;
			mountData.runningFrameStart = 6;
			mountData.flyingFrameCount = 6;
			mountData.flyingFrameDelay = 6;
			mountData.flyingFrameStart = 6;
			mountData.inAirFrameCount = 1;
			mountData.inAirFrameDelay = 12;
			mountData.inAirFrameStart = 1;
			mountData.idleFrameCount = 4;
			mountData.idleFrameDelay = 30;
			mountData.idleFrameStart = 2;
			mountData.idleFrameLoop = true;
			mountData.swimFrameCount = mountData.inAirFrameCount;
			mountData.swimFrameDelay = mountData.inAirFrameDelay;
			mountData.swimFrameStart = mountData.inAirFrameStart;
			if (Main.netMode != 2)
			{
				mountData.backTexture = TextureAssets.RudolphMount[0];
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = TextureAssets.RudolphMount[1];
				mountData.frontTextureExtra = TextureAssets.RudolphMount[2];
				mountData.textureWidth = mountData.backTexture.Width();
				mountData.textureHeight = mountData.backTexture.Height();
			}
			mountData = new Mount.MountData();
			Mount.mounts[2] = mountData;
			mountData.spawnDust = 58;
			mountData.buff = 129;
			mountData.heightBoost = 20;
			mountData.flightTimeMax = 160;
			mountData.runSpeed = 5f;
			mountData.dashSpeed = 9f;
			mountData.acceleration = 0.08f;
			mountData.jumpHeight = 10;
			mountData.jumpSpeed = 6.01f;
			mountData.totalFrames = 16;
			array = new int[mountData.totalFrames];
			for (int j = 0; j < array.Length; j++)
			{
				array[j] = 22;
			}
			array[12] += 2;
			array[13] += 4;
			array[14] += 2;
			mountData.playerYOffsets = array;
			mountData.xOffset = 1;
			mountData.bodyFrame = 3;
			mountData.yOffset = 8;
			mountData.playerHeadOffset = 22;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 7;
			mountData.runningFrameCount = 5;
			mountData.runningFrameDelay = 12;
			mountData.runningFrameStart = 11;
			mountData.flyingFrameCount = 6;
			mountData.flyingFrameDelay = 6;
			mountData.flyingFrameStart = 1;
			mountData.inAirFrameCount = 1;
			mountData.inAirFrameDelay = 12;
			mountData.inAirFrameStart = 0;
			mountData.idleFrameCount = 3;
			mountData.idleFrameDelay = 20;
			mountData.idleFrameStart = 8;
			mountData.idleFrameLoop = false;
			mountData.swimFrameCount = mountData.inAirFrameCount;
			mountData.swimFrameDelay = mountData.inAirFrameDelay;
			mountData.swimFrameStart = mountData.inAirFrameStart;
			if (Main.netMode != 2)
			{
				mountData.backTexture = TextureAssets.PigronMount;
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = Asset<Texture2D>.Empty;
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.backTexture.Width();
				mountData.textureHeight = mountData.backTexture.Height();
			}
			mountData = new Mount.MountData();
			Mount.mounts[1] = mountData;
			mountData.spawnDust = 15;
			mountData.buff = 128;
			mountData.heightBoost = 20;
			mountData.flightTimeMax = 0;
			mountData.fallDamage = 0.8f;
			mountData.runSpeed = 4f;
			mountData.dashSpeed = 7.8f;
			mountData.acceleration = 0.13f;
			mountData.jumpHeight = 15;
			mountData.jumpSpeed = 5.01f;
			mountData.totalFrames = 7;
			array = new int[mountData.totalFrames];
			for (int k = 0; k < array.Length; k++)
			{
				array[k] = 14;
			}
			array[2] += 2;
			array[3] += 4;
			array[4] += 8;
			array[5] += 8;
			mountData.playerYOffsets = array;
			mountData.xOffset = 1;
			mountData.bodyFrame = 3;
			mountData.yOffset = 4;
			mountData.playerHeadOffset = 22;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 7;
			mountData.runningFrameDelay = 12;
			mountData.runningFrameStart = 0;
			mountData.flyingFrameCount = 6;
			mountData.flyingFrameDelay = 6;
			mountData.flyingFrameStart = 1;
			mountData.inAirFrameCount = 1;
			mountData.inAirFrameDelay = 12;
			mountData.inAirFrameStart = 5;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			mountData.swimFrameCount = mountData.inAirFrameCount;
			mountData.swimFrameDelay = mountData.inAirFrameDelay;
			mountData.swimFrameStart = mountData.inAirFrameStart;
			if (Main.netMode != 2)
			{
				mountData.backTexture = TextureAssets.BunnyMount;
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = Asset<Texture2D>.Empty;
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.backTexture.Width();
				mountData.textureHeight = mountData.backTexture.Height();
			}
			mountData = new Mount.MountData();
			Mount.mounts[3] = mountData;
			mountData.spawnDust = 56;
			mountData.buff = 130;
			mountData.heightBoost = 20;
			mountData.flightTimeMax = 0;
			mountData.fallDamage = 0.5f;
			mountData.extraFall = 10;
			mountData.runSpeed = 4f;
			mountData.dashSpeed = 4f;
			mountData.acceleration = 0.18f;
			mountData.jumpHeight = 12;
			mountData.jumpSpeed = 8.25f;
			mountData.constantJump = true;
			mountData.totalFrames = 4;
			array = new int[mountData.totalFrames];
			for (int l = 0; l < array.Length; l++)
			{
				array[l] = 20;
			}
			array[1] += 2;
			array[3] -= 2;
			mountData.playerYOffsets = array;
			mountData.xOffset = 1;
			mountData.bodyFrame = 3;
			mountData.yOffset = 11;
			mountData.playerHeadOffset = 22;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 4;
			mountData.runningFrameDelay = 12;
			mountData.runningFrameStart = 0;
			mountData.flyingFrameCount = 0;
			mountData.flyingFrameDelay = 0;
			mountData.flyingFrameStart = 0;
			mountData.inAirFrameCount = 1;
			mountData.inAirFrameDelay = 12;
			mountData.inAirFrameStart = 1;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			if (Main.netMode != 2)
			{
				mountData.backTexture = TextureAssets.SlimeMount;
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = Asset<Texture2D>.Empty;
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.backTexture.Width();
				mountData.textureHeight = mountData.backTexture.Height();
			}
			mountData = new Mount.MountData();
			Mount.mounts[6] = mountData;
			mountData.Minecart = true;
			mountData.delegations = new Mount.MountDelegatesData();
			mountData.delegations.MinecartDust = new Action<Vector2>(DelegateMethods.Minecart.Sparks);
			mountData.spawnDust = 213;
			mountData.buff = 118;
			mountData.heightBoost = 10;
			mountData.flightTimeMax = 0;
			mountData.fallDamage = 1f;
			mountData.runSpeed = 13f;
			mountData.dashSpeed = 13f;
			mountData.acceleration = 0.04f;
			mountData.jumpHeight = 15;
			mountData.jumpSpeed = 5.15f;
			mountData.blockExtraJumps = true;
			mountData.totalFrames = 3;
			array = new int[mountData.totalFrames];
			for (int m = 0; m < array.Length; m++)
			{
				array[m] = 8;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = 1;
			mountData.bodyFrame = 3;
			mountData.yOffset = 13;
			mountData.playerHeadOffset = 14;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 3;
			mountData.runningFrameDelay = 12;
			mountData.runningFrameStart = 0;
			mountData.flyingFrameCount = 0;
			mountData.flyingFrameDelay = 0;
			mountData.flyingFrameStart = 0;
			mountData.inAirFrameCount = 0;
			mountData.inAirFrameDelay = 0;
			mountData.inAirFrameStart = 0;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			if (Main.netMode != 2)
			{
				mountData.backTexture = Asset<Texture2D>.Empty;
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = TextureAssets.MinecartMount;
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.frontTexture.Width();
				mountData.textureHeight = mountData.frontTexture.Height();
			}
			mountData = new Mount.MountData();
			Mount.mounts[15] = mountData;
			Mount.SetAsMinecart(mountData, 208, TextureAssets.DesertMinecartMount, 0, 0);
			mountData = new Mount.MountData();
			Mount.mounts[18] = mountData;
			Mount.SetAsMinecart(mountData, 220, TextureAssets.Extra[108], 0, 0);
			mountData = new Mount.MountData();
			Mount.mounts[19] = mountData;
			Mount.SetAsMinecart(mountData, 222, TextureAssets.Extra[109], 0, 0);
			mountData = new Mount.MountData();
			Mount.mounts[20] = mountData;
			Mount.SetAsMinecart(mountData, 224, TextureAssets.Extra[110], 0, 0);
			mountData = new Mount.MountData();
			Mount.mounts[21] = mountData;
			Mount.SetAsMinecart(mountData, 226, TextureAssets.Extra[111], 0, 0);
			mountData = new Mount.MountData();
			Mount.mounts[22] = mountData;
			Mount.SetAsMinecart(mountData, 228, TextureAssets.Extra[112], 0, 0);
			mountData = new Mount.MountData();
			Mount.mounts[24] = mountData;
			Mount.SetAsMinecart(mountData, 231, TextureAssets.Extra[115], 0, 0);
			mountData.frontTextureGlow = TextureAssets.Extra[116];
			mountData = new Mount.MountData();
			Mount.mounts[25] = mountData;
			Mount.SetAsMinecart(mountData, 233, TextureAssets.Extra[117], 0, 0);
			mountData = new Mount.MountData();
			Mount.mounts[26] = mountData;
			Mount.SetAsMinecart(mountData, 235, TextureAssets.Extra[118], 0, 0);
			mountData = new Mount.MountData();
			Mount.mounts[27] = mountData;
			Mount.SetAsMinecart(mountData, 237, TextureAssets.Extra[119], 0, 0);
			mountData = new Mount.MountData();
			Mount.mounts[28] = mountData;
			Mount.SetAsMinecart(mountData, 239, TextureAssets.Extra[120], 0, 0);
			mountData = new Mount.MountData();
			Mount.mounts[29] = mountData;
			Mount.SetAsMinecart(mountData, 241, TextureAssets.Extra[121], 0, 0);
			mountData = new Mount.MountData();
			Mount.mounts[30] = mountData;
			Mount.SetAsMinecart(mountData, 243, TextureAssets.Extra[122], 0, 0);
			mountData = new Mount.MountData();
			Mount.mounts[31] = mountData;
			Mount.SetAsMinecart(mountData, 245, TextureAssets.Extra[123], 0, 0);
			mountData = new Mount.MountData();
			Mount.mounts[32] = mountData;
			Mount.SetAsMinecart(mountData, 247, TextureAssets.Extra[124], 0, 0);
			mountData = new Mount.MountData();
			Mount.mounts[33] = mountData;
			Mount.SetAsMinecart(mountData, 249, TextureAssets.Extra[125], 0, 0);
			mountData.delegations.MinecartDust = new Action<Vector2>(DelegateMethods.Minecart.SparksMeow);
			mountData.delegations.MinecartLandingSound = new Action<Player, Vector2, int, int>(Mount.MeowcartLandingSound);
			mountData.delegations.MinecartBumperSound = new Action<Player, Vector2, int, int>(Mount.MeowcartBumperSound);
			mountData = new Mount.MountData();
			Mount.mounts[34] = mountData;
			Mount.SetAsMinecart(mountData, 251, TextureAssets.Extra[126], 0, 0);
			mountData = new Mount.MountData();
			Mount.mounts[35] = mountData;
			Mount.SetAsMinecart(mountData, 253, TextureAssets.Extra[127], 0, 0);
			mountData = new Mount.MountData();
			Mount.mounts[36] = mountData;
			Mount.SetAsMinecart(mountData, 255, TextureAssets.Extra[128], 0, 0);
			mountData = new Mount.MountData();
			Mount.mounts[38] = mountData;
			Mount.SetAsMinecart(mountData, 269, TextureAssets.Extra[150], 0, 0);
			if (Main.netMode != 2)
			{
				mountData.backTexture = mountData.frontTexture;
			}
			mountData = new Mount.MountData();
			Mount.mounts[39] = mountData;
			Mount.SetAsMinecart(mountData, 272, TextureAssets.Extra[155], 0, 0);
			mountData.yOffset -= 2;
			if (Main.netMode != 2)
			{
				mountData.frontTextureExtra = TextureAssets.Extra[165];
			}
			mountData.runSpeed = 6f;
			mountData.dashSpeed = 6f;
			mountData.acceleration = 0.02f;
			mountData = new Mount.MountData();
			Mount.mounts[16] = mountData;
			mountData.Minecart = true;
			mountData.delegations = new Mount.MountDelegatesData();
			mountData.delegations.MinecartDust = new Action<Vector2>(DelegateMethods.Minecart.Sparks);
			mountData.spawnDust = 213;
			mountData.buff = 210;
			mountData.heightBoost = 10;
			mountData.flightTimeMax = 0;
			mountData.fallDamage = 1f;
			mountData.runSpeed = 13f;
			mountData.dashSpeed = 13f;
			mountData.acceleration = 0.04f;
			mountData.jumpHeight = 15;
			mountData.jumpSpeed = 5.15f;
			mountData.blockExtraJumps = true;
			mountData.totalFrames = 3;
			array = new int[mountData.totalFrames];
			for (int n = 0; n < array.Length; n++)
			{
				array[n] = 8;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = 1;
			mountData.bodyFrame = 3;
			mountData.yOffset = 13;
			mountData.playerHeadOffset = 14;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 3;
			mountData.runningFrameDelay = 12;
			mountData.runningFrameStart = 0;
			mountData.flyingFrameCount = 0;
			mountData.flyingFrameDelay = 0;
			mountData.flyingFrameStart = 0;
			mountData.inAirFrameCount = 0;
			mountData.inAirFrameDelay = 0;
			mountData.inAirFrameStart = 0;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			if (Main.netMode != 2)
			{
				mountData.backTexture = Asset<Texture2D>.Empty;
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = TextureAssets.FishMinecartMount;
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.frontTexture.Width();
				mountData.textureHeight = mountData.frontTexture.Height();
			}
			mountData = new Mount.MountData();
			Mount.mounts[51] = mountData;
			Mount.SetAsMinecart(mountData, 338, TextureAssets.Extra[246], -10, -8);
			mountData.spawnDust = 211;
			mountData.delegations.MinecartDust = new Action<Vector2>(DelegateMethods.Minecart.SparksFart);
			mountData.delegations.MinecartBumperSound = new Action<Player, Vector2, int, int>(DelegateMethods.Minecart.BumperSoundFart);
			mountData.delegations.MinecartLandingSound = new Action<Player, Vector2, int, int>(DelegateMethods.Minecart.LandingSoundFart);
			mountData.delegations.MinecartJumpingSound = new Action<Player, Vector2, int, int>(DelegateMethods.Minecart.JumpingSoundFart);
			mountData = new Mount.MountData();
			Mount.mounts[53] = mountData;
			Mount.SetAsMinecart(mountData, 346, TextureAssets.Extra[251], -10, -8);
			mountData.spawnDust = 211;
			mountData.delegations.MinecartDust = new Action<Vector2>(DelegateMethods.Minecart.SparksTerraFart);
			mountData.delegations.MinecartBumperSound = new Action<Player, Vector2, int, int>(DelegateMethods.Minecart.BumperSoundFart);
			mountData.delegations.MinecartLandingSound = new Action<Player, Vector2, int, int>(DelegateMethods.Minecart.LandingSoundFart);
			mountData.delegations.MinecartJumpingSound = new Action<Player, Vector2, int, int>(DelegateMethods.Minecart.JumpingSoundFart);
			mountData = new Mount.MountData();
			Mount.mounts[4] = mountData;
			mountData.spawnDust = 56;
			mountData.buff = 131;
			mountData.heightBoost = 26;
			mountData.flightTimeMax = 0;
			mountData.fallDamage = 1f;
			mountData.runSpeed = 2f;
			mountData.dashSpeed = 5f;
			mountData.swimSpeed = 10f;
			mountData.acceleration = 0.08f;
			mountData.jumpHeight = 12;
			mountData.jumpSpeed = 3.7f;
			mountData.totalFrames = 12;
			array = new int[mountData.totalFrames];
			for (int num = 0; num < array.Length; num++)
			{
				array[num] = 26;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = 1;
			mountData.bodyFrame = 3;
			mountData.yOffset = 13;
			mountData.playerHeadOffset = 28;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 6;
			mountData.runningFrameDelay = 12;
			mountData.runningFrameStart = 0;
			mountData.flyingFrameCount = 0;
			mountData.flyingFrameDelay = 0;
			mountData.flyingFrameStart = 0;
			mountData.inAirFrameCount = 1;
			mountData.inAirFrameDelay = 12;
			mountData.inAirFrameStart = 3;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			mountData.swimFrameCount = 6;
			mountData.swimFrameDelay = 12;
			mountData.swimFrameStart = 6;
			if (Main.netMode != 2)
			{
				mountData.backTexture = TextureAssets.TurtleMount;
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = Asset<Texture2D>.Empty;
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.backTexture.Width();
				mountData.textureHeight = mountData.backTexture.Height();
			}
			mountData = new Mount.MountData();
			Mount.mounts[5] = mountData;
			mountData.spawnDust = 152;
			mountData.buff = 132;
			mountData.heightBoost = 16;
			mountData.flightTimeMax = 320;
			mountData.fatigueMax = 320;
			mountData.fallDamage = 0f;
			mountData.usesHover = true;
			mountData.runSpeed = 2f;
			mountData.dashSpeed = 2f;
			mountData.acceleration = 0.16f;
			mountData.jumpHeight = 10;
			mountData.jumpSpeed = 4f;
			mountData.blockExtraJumps = true;
			mountData.totalFrames = 12;
			array = new int[mountData.totalFrames];
			for (int num2 = 0; num2 < array.Length; num2++)
			{
				array[num2] = 16;
			}
			array[8] = 18;
			mountData.playerYOffsets = array;
			mountData.xOffset = 1;
			mountData.bodyFrame = 3;
			mountData.yOffset = 4;
			mountData.playerHeadOffset = 18;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 5;
			mountData.runningFrameDelay = 12;
			mountData.runningFrameStart = 0;
			mountData.flyingFrameCount = 3;
			mountData.flyingFrameDelay = 12;
			mountData.flyingFrameStart = 5;
			mountData.inAirFrameCount = 3;
			mountData.inAirFrameDelay = 12;
			mountData.inAirFrameStart = 5;
			mountData.idleFrameCount = 4;
			mountData.idleFrameDelay = 12;
			mountData.idleFrameStart = 8;
			mountData.idleFrameLoop = true;
			mountData.swimFrameCount = 0;
			mountData.swimFrameDelay = 12;
			mountData.swimFrameStart = 0;
			if (Main.netMode != 2)
			{
				mountData.backTexture = TextureAssets.BeeMount[0];
				mountData.backTextureExtra = TextureAssets.BeeMount[1];
				mountData.frontTexture = Asset<Texture2D>.Empty;
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.backTexture.Width();
				mountData.textureHeight = mountData.backTexture.Height();
			}
			mountData = new Mount.MountData();
			Mount.mounts[7] = mountData;
			mountData.spawnDust = 226;
			mountData.spawnDustNoGravity = true;
			mountData.buff = 141;
			mountData.heightBoost = 16;
			mountData.flightTimeMax = 320;
			mountData.fatigueMax = 320;
			mountData.fallDamage = 0f;
			mountData.usesHover = true;
			mountData.runSpeed = 8f;
			mountData.dashSpeed = 8f;
			mountData.acceleration = 0.16f;
			mountData.jumpHeight = 10;
			mountData.jumpSpeed = 4f;
			mountData.blockExtraJumps = true;
			mountData.totalFrames = 8;
			array = new int[mountData.totalFrames];
			for (int num3 = 0; num3 < array.Length; num3++)
			{
				array[num3] = 16;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = 1;
			mountData.bodyFrame = 3;
			mountData.yOffset = 4;
			mountData.playerHeadOffset = 18;
			mountData.standingFrameCount = 8;
			mountData.standingFrameDelay = 4;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 8;
			mountData.runningFrameDelay = 4;
			mountData.runningFrameStart = 0;
			mountData.flyingFrameCount = 8;
			mountData.flyingFrameDelay = 4;
			mountData.flyingFrameStart = 0;
			mountData.inAirFrameCount = 8;
			mountData.inAirFrameDelay = 4;
			mountData.inAirFrameStart = 0;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 12;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = true;
			mountData.swimFrameCount = 0;
			mountData.swimFrameDelay = 12;
			mountData.swimFrameStart = 0;
			if (Main.netMode != 2)
			{
				mountData.backTexture = Asset<Texture2D>.Empty;
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = TextureAssets.UfoMount[0];
				mountData.frontTextureExtra = TextureAssets.UfoMount[1];
				mountData.textureWidth = mountData.frontTexture.Width();
				mountData.textureHeight = mountData.frontTexture.Height();
			}
			mountData = new Mount.MountData();
			Mount.mounts[8] = mountData;
			mountData.spawnDust = 226;
			mountData.buff = 142;
			mountData.heightBoost = 16;
			mountData.flightTimeMax = 320;
			mountData.fatigueMax = 320;
			mountData.fallDamage = 1f;
			mountData.usesHover = true;
			mountData.swimSpeed = 4f;
			mountData.runSpeed = 6f;
			mountData.dashSpeed = 4f;
			mountData.acceleration = 0.16f;
			mountData.jumpHeight = 10;
			mountData.jumpSpeed = 4f;
			mountData.blockExtraJumps = true;
			mountData.emitsLight = true;
			mountData.lightColor = new Vector3(0.3f, 0.3f, 0.4f);
			mountData.totalFrames = 1;
			array = new int[mountData.totalFrames];
			for (int num4 = 0; num4 < array.Length; num4++)
			{
				array[num4] = 4;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = 1;
			mountData.bodyFrame = 3;
			mountData.yOffset = 4;
			mountData.playerHeadOffset = 18;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 1;
			mountData.runningFrameDelay = 12;
			mountData.runningFrameStart = 0;
			mountData.flyingFrameCount = 1;
			mountData.flyingFrameDelay = 12;
			mountData.flyingFrameStart = 0;
			mountData.inAirFrameCount = 1;
			mountData.inAirFrameDelay = 12;
			mountData.inAirFrameStart = 0;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 12;
			mountData.idleFrameStart = 8;
			mountData.swimFrameCount = 0;
			mountData.swimFrameDelay = 12;
			mountData.swimFrameStart = 0;
			if (Main.netMode != 2)
			{
				mountData.backTexture = TextureAssets.DrillMount[0];
				mountData.backTextureGlow = TextureAssets.DrillMount[3];
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.backTextureExtraGlow = Asset<Texture2D>.Empty;
				mountData.frontTexture = TextureAssets.DrillMount[1];
				mountData.frontTextureGlow = TextureAssets.DrillMount[4];
				mountData.frontTextureExtra = TextureAssets.DrillMount[2];
				mountData.frontTextureExtraGlow = TextureAssets.DrillMount[5];
				mountData.textureWidth = mountData.frontTexture.Width();
				mountData.textureHeight = mountData.frontTexture.Height();
			}
			Mount.drillTextureSize = new Vector2(80f, 80f);
			if (!Main.dedServ)
			{
				Vector2 value = new Vector2((float)mountData.textureWidth, (float)(mountData.textureHeight / mountData.totalFrames));
				if (Mount.drillTextureSize != value)
				{
					throw new Exception(string.Concat(new object[]
					{
						"Be sure to update the Drill texture origin to match the actual texture size of ",
						mountData.textureWidth,
						", ",
						mountData.textureHeight,
						"."
					}));
				}
			}
			mountData = new Mount.MountData();
			Mount.mounts[9] = mountData;
			mountData.spawnDust = 15;
			mountData.buff = 143;
			mountData.heightBoost = 16;
			mountData.flightTimeMax = 0;
			mountData.fatigueMax = 0;
			mountData.fallDamage = 0f;
			mountData.abilityChargeMax = 40;
			mountData.abilityCooldown = 20;
			mountData.abilityDuration = 0;
			mountData.runSpeed = 8f;
			mountData.dashSpeed = 8f;
			mountData.acceleration = 0.4f;
			mountData.jumpHeight = 22;
			mountData.jumpSpeed = 10.01f;
			mountData.blockExtraJumps = false;
			mountData.totalFrames = 12;
			array = new int[mountData.totalFrames];
			for (int num5 = 0; num5 < array.Length; num5++)
			{
				array[num5] = 16;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = 1;
			mountData.bodyFrame = 3;
			mountData.yOffset = 6;
			mountData.playerHeadOffset = 18;
			mountData.standingFrameCount = 6;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 6;
			mountData.runningFrameCount = 6;
			mountData.runningFrameDelay = 12;
			mountData.runningFrameStart = 0;
			mountData.flyingFrameCount = 0;
			mountData.flyingFrameDelay = 12;
			mountData.flyingFrameStart = 0;
			mountData.inAirFrameCount = 1;
			mountData.inAirFrameDelay = 12;
			mountData.inAirFrameStart = 1;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 12;
			mountData.idleFrameStart = 6;
			mountData.idleFrameLoop = true;
			mountData.swimFrameCount = 0;
			mountData.swimFrameDelay = 12;
			mountData.swimFrameStart = 0;
			if (Main.netMode != 2)
			{
				mountData.backTexture = TextureAssets.ScutlixMount[0];
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = TextureAssets.ScutlixMount[1];
				mountData.frontTextureExtra = TextureAssets.ScutlixMount[2];
				mountData.textureWidth = mountData.backTexture.Width();
				mountData.textureHeight = mountData.backTexture.Height();
			}
			Mount.scutlixEyePositions = new Vector2[10];
			Mount.scutlixEyePositions[0] = new Vector2(60f, 2f);
			Mount.scutlixEyePositions[1] = new Vector2(70f, 6f);
			Mount.scutlixEyePositions[2] = new Vector2(68f, 6f);
			Mount.scutlixEyePositions[3] = new Vector2(76f, 12f);
			Mount.scutlixEyePositions[4] = new Vector2(80f, 10f);
			Mount.scutlixEyePositions[5] = new Vector2(84f, 18f);
			Mount.scutlixEyePositions[6] = new Vector2(74f, 20f);
			Mount.scutlixEyePositions[7] = new Vector2(76f, 24f);
			Mount.scutlixEyePositions[8] = new Vector2(70f, 34f);
			Mount.scutlixEyePositions[9] = new Vector2(76f, 34f);
			Mount.scutlixTextureSize = new Vector2(45f, 54f);
			if (!Main.dedServ)
			{
				Vector2 value2 = new Vector2((float)(mountData.textureWidth / 2), (float)(mountData.textureHeight / mountData.totalFrames));
				if (Mount.scutlixTextureSize != value2)
				{
					throw new Exception(string.Concat(new object[]
					{
						"Be sure to update the Scutlix texture origin to match the actual texture size of ",
						mountData.textureWidth,
						", ",
						mountData.textureHeight,
						"."
					}));
				}
			}
			for (int num6 = 0; num6 < Mount.scutlixEyePositions.Length; num6++)
			{
				Mount.scutlixEyePositions[num6] -= Mount.scutlixTextureSize;
			}
			mountData = new Mount.MountData();
			Mount.mounts[10] = mountData;
			mountData.spawnDust = 15;
			mountData.buff = 162;
			mountData.heightBoost = 34;
			mountData.flightTimeMax = 0;
			mountData.fallDamage = 0.2f;
			mountData.runSpeed = 4f;
			mountData.dashSpeed = 12f;
			mountData.acceleration = 0.3f;
			mountData.jumpHeight = 10;
			mountData.jumpSpeed = 8.01f;
			mountData.totalFrames = 16;
			array = new int[mountData.totalFrames];
			for (int num7 = 0; num7 < array.Length; num7++)
			{
				array[num7] = 28;
			}
			array[3] += 2;
			array[4] += 2;
			array[7] += 2;
			array[8] += 2;
			array[12] += 2;
			array[13] += 2;
			array[15] += 4;
			mountData.playerYOffsets = array;
			mountData.xOffset = 5;
			mountData.bodyFrame = 3;
			mountData.yOffset = 1;
			mountData.playerHeadOffset = 34;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 7;
			mountData.runningFrameDelay = 15;
			mountData.runningFrameStart = 1;
			mountData.dashingFrameCount = 6;
			mountData.dashingFrameDelay = 40;
			mountData.dashingFrameStart = 9;
			mountData.flyingFrameCount = 6;
			mountData.flyingFrameDelay = 6;
			mountData.flyingFrameStart = 1;
			mountData.inAirFrameCount = 1;
			mountData.inAirFrameDelay = 12;
			mountData.inAirFrameStart = 15;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			mountData.swimFrameCount = mountData.inAirFrameCount;
			mountData.swimFrameDelay = mountData.inAirFrameDelay;
			mountData.swimFrameStart = mountData.inAirFrameStart;
			if (Main.netMode != 2)
			{
				mountData.backTexture = TextureAssets.UnicornMount;
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = Asset<Texture2D>.Empty;
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.backTexture.Width();
				mountData.textureHeight = mountData.backTexture.Height();
			}
			mountData = new Mount.MountData();
			Mount.mounts[11] = mountData;
			mountData.Minecart = true;
			mountData.delegations = new Mount.MountDelegatesData();
			mountData.delegations.MinecartDust = new Action<Vector2>(DelegateMethods.Minecart.SparksMech);
			mountData.spawnDust = 213;
			mountData.buff = 166;
			mountData.heightBoost = 12;
			mountData.flightTimeMax = 0;
			mountData.fallDamage = 1f;
			mountData.runSpeed = 13f;
			mountData.dashSpeed = 13f;
			mountData.acceleration = 0.04f;
			mountData.jumpHeight = 15;
			mountData.jumpSpeed = 5.15f;
			mountData.blockExtraJumps = true;
			mountData.totalFrames = 3;
			array = new int[mountData.totalFrames];
			for (int num8 = 0; num8 < array.Length; num8++)
			{
				array[num8] = 9;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = -1;
			mountData.bodyFrame = 3;
			mountData.yOffset = 11;
			mountData.playerHeadOffset = 14;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 3;
			mountData.runningFrameDelay = 12;
			mountData.runningFrameStart = 0;
			mountData.flyingFrameCount = 0;
			mountData.flyingFrameDelay = 0;
			mountData.flyingFrameStart = 0;
			mountData.inAirFrameCount = 0;
			mountData.inAirFrameDelay = 0;
			mountData.inAirFrameStart = 0;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			if (Main.netMode != 2)
			{
				mountData.backTexture = Asset<Texture2D>.Empty;
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = TextureAssets.MinecartMechMount[0];
				mountData.frontTextureGlow = TextureAssets.MinecartMechMount[1];
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.frontTexture.Width();
				mountData.textureHeight = mountData.frontTexture.Height();
			}
			mountData = new Mount.MountData();
			Mount.mounts[12] = mountData;
			mountData.spawnDust = 15;
			mountData.buff = 168;
			mountData.heightBoost = 14;
			mountData.flightTimeMax = 320;
			mountData.fatigueMax = 320;
			mountData.fallDamage = 0f;
			mountData.usesHover = true;
			mountData.runSpeed = 2f;
			mountData.dashSpeed = 1f;
			mountData.acceleration = 0.2f;
			mountData.jumpHeight = 4;
			mountData.jumpSpeed = 3f;
			mountData.swimSpeed = 16f;
			mountData.blockExtraJumps = true;
			mountData.totalFrames = 23;
			array = new int[mountData.totalFrames];
			for (int num9 = 0; num9 < array.Length; num9++)
			{
				array[num9] = 12;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = 2;
			mountData.bodyFrame = 3;
			mountData.yOffset = 16;
			mountData.playerHeadOffset = 16;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 8;
			mountData.runningFrameCount = 7;
			mountData.runningFrameDelay = 14;
			mountData.runningFrameStart = 8;
			mountData.flyingFrameCount = 8;
			mountData.flyingFrameDelay = 16;
			mountData.flyingFrameStart = 0;
			mountData.inAirFrameCount = 8;
			mountData.inAirFrameDelay = 6;
			mountData.inAirFrameStart = 0;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			mountData.swimFrameCount = 8;
			mountData.swimFrameDelay = 4;
			mountData.swimFrameStart = 15;
			if (Main.netMode != 2)
			{
				mountData.backTexture = TextureAssets.CuteFishronMount[0];
				mountData.backTextureGlow = TextureAssets.CuteFishronMount[1];
				mountData.frontTexture = Asset<Texture2D>.Empty;
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.backTexture.Width();
				mountData.textureHeight = mountData.backTexture.Height();
			}
			mountData = new Mount.MountData();
			Mount.mounts[13] = mountData;
			mountData.Minecart = true;
			mountData.delegations = new Mount.MountDelegatesData();
			mountData.delegations.MinecartDust = new Action<Vector2>(DelegateMethods.Minecart.Sparks);
			mountData.spawnDust = 213;
			mountData.buff = 184;
			mountData.heightBoost = 10;
			mountData.flightTimeMax = 0;
			mountData.fallDamage = 1f;
			mountData.runSpeed = 10f;
			mountData.dashSpeed = 10f;
			mountData.acceleration = 0.03f;
			mountData.jumpHeight = 12;
			mountData.jumpSpeed = 5.15f;
			mountData.blockExtraJumps = true;
			mountData.totalFrames = 3;
			array = new int[mountData.totalFrames];
			for (int num10 = 0; num10 < array.Length; num10++)
			{
				array[num10] = 8;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = 1;
			mountData.bodyFrame = 3;
			mountData.yOffset = 13;
			mountData.playerHeadOffset = 14;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 3;
			mountData.runningFrameDelay = 12;
			mountData.runningFrameStart = 0;
			mountData.flyingFrameCount = 0;
			mountData.flyingFrameDelay = 0;
			mountData.flyingFrameStart = 0;
			mountData.inAirFrameCount = 0;
			mountData.inAirFrameDelay = 0;
			mountData.inAirFrameStart = 0;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			if (Main.netMode != 2)
			{
				mountData.backTexture = Asset<Texture2D>.Empty;
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = TextureAssets.MinecartWoodMount;
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.frontTexture.Width();
				mountData.textureHeight = mountData.frontTexture.Height();
			}
			mountData = new Mount.MountData();
			Mount.mounts[14] = mountData;
			mountData.spawnDust = 15;
			mountData.buff = 193;
			mountData.heightBoost = 8;
			mountData.flightTimeMax = 0;
			mountData.fallDamage = 0.2f;
			mountData.runSpeed = 8f;
			mountData.acceleration = 0.25f;
			mountData.jumpHeight = 20;
			mountData.jumpSpeed = 8.01f;
			mountData.totalFrames = 8;
			array = new int[mountData.totalFrames];
			for (int num11 = 0; num11 < array.Length; num11++)
			{
				array[num11] = 8;
			}
			array[1] += 2;
			array[3] += 2;
			array[6] += 2;
			mountData.playerYOffsets = array;
			mountData.xOffset = 4;
			mountData.bodyFrame = 3;
			mountData.yOffset = 9;
			mountData.playerHeadOffset = 10;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 6;
			mountData.runningFrameDelay = 30;
			mountData.runningFrameStart = 2;
			mountData.inAirFrameCount = 1;
			mountData.inAirFrameDelay = 12;
			mountData.inAirFrameStart = 1;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			mountData.swimFrameCount = mountData.inAirFrameCount;
			mountData.swimFrameDelay = mountData.inAirFrameDelay;
			mountData.swimFrameStart = mountData.inAirFrameStart;
			if (Main.netMode != 2)
			{
				mountData.backTexture = TextureAssets.BasiliskMount;
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = Asset<Texture2D>.Empty;
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.backTexture.Width();
				mountData.textureHeight = mountData.backTexture.Height();
			}
			mountData = new Mount.MountData();
			Mount.mounts[17] = mountData;
			mountData.spawnDust = 15;
			mountData.buff = 212;
			mountData.heightBoost = 16;
			mountData.flightTimeMax = 0;
			mountData.fallDamage = 0.2f;
			mountData.runSpeed = 8f;
			mountData.acceleration = 0.25f;
			mountData.jumpHeight = 20;
			mountData.jumpSpeed = 8.01f;
			mountData.totalFrames = 4;
			array = new int[mountData.totalFrames];
			for (int num12 = 0; num12 < array.Length; num12++)
			{
				array[num12] = 8;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = 2;
			mountData.bodyFrame = 3;
			mountData.yOffset = 17 - mountData.heightBoost;
			mountData.playerHeadOffset = 18;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 4;
			mountData.runningFrameDelay = 12;
			mountData.runningFrameStart = 0;
			mountData.inAirFrameCount = 1;
			mountData.inAirFrameDelay = 12;
			mountData.inAirFrameStart = 1;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			mountData.swimFrameCount = mountData.inAirFrameCount;
			mountData.swimFrameDelay = mountData.inAirFrameDelay;
			mountData.swimFrameStart = mountData.inAirFrameStart;
			if (Main.netMode != 2)
			{
				mountData.backTexture = TextureAssets.Extra[97];
				mountData.backTextureExtra = TextureAssets.Extra[96];
				mountData.frontTexture = Asset<Texture2D>.Empty;
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.backTextureExtra.Width();
				mountData.textureHeight = mountData.backTextureExtra.Height();
			}
			mountData = new Mount.MountData();
			Mount.mounts[23] = mountData;
			mountData.spawnDust = 43;
			mountData.spawnDustNoGravity = true;
			mountData.buff = 230;
			mountData.heightBoost = 0;
			mountData.flightTimeMax = 320;
			mountData.fatigueMax = 320;
			mountData.fallDamage = 0f;
			mountData.usesHover = true;
			mountData.runSpeed = 9f;
			mountData.dashSpeed = 9f;
			mountData.acceleration = 0.16f;
			mountData.jumpHeight = 10;
			mountData.jumpSpeed = 4f;
			mountData.blockExtraJumps = true;
			mountData.totalFrames = 6;
			array = new int[mountData.totalFrames];
			for (int num13 = 0; num13 < array.Length; num13++)
			{
				array[num13] = 6;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = -2;
			mountData.bodyFrame = 0;
			mountData.yOffset = 8;
			mountData.playerHeadOffset = 0;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 0;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 1;
			mountData.runningFrameDelay = 0;
			mountData.runningFrameStart = 0;
			mountData.flyingFrameCount = 1;
			mountData.flyingFrameDelay = 0;
			mountData.flyingFrameStart = 0;
			mountData.inAirFrameCount = 6;
			mountData.inAirFrameDelay = 8;
			mountData.inAirFrameStart = 0;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = true;
			mountData.swimFrameCount = 0;
			mountData.swimFrameDelay = 0;
			mountData.swimFrameStart = 0;
			if (Main.netMode != 2)
			{
				mountData.backTexture = TextureAssets.Extra[113];
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = Asset<Texture2D>.Empty;
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.backTexture.Width();
				mountData.textureHeight = mountData.backTexture.Height();
			}
			mountData = new Mount.MountData();
			Mount.mounts[37] = mountData;
			mountData.spawnDust = 282;
			mountData.buff = 265;
			mountData.heightBoost = 12;
			mountData.flightTimeMax = 0;
			mountData.fallDamage = 0.2f;
			mountData.runSpeed = 6f;
			mountData.acceleration = 0.15f;
			mountData.jumpHeight = 14;
			mountData.jumpSpeed = 6.01f;
			mountData.totalFrames = 10;
			array = new int[mountData.totalFrames];
			for (int num14 = 0; num14 < array.Length; num14++)
			{
				array[num14] = 20;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = 5;
			mountData.bodyFrame = 4;
			mountData.yOffset = 1;
			mountData.playerHeadOffset = 20;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 7;
			mountData.runningFrameDelay = 20;
			mountData.runningFrameStart = 2;
			mountData.inAirFrameCount = 1;
			mountData.inAirFrameDelay = 12;
			mountData.inAirFrameStart = 1;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			mountData.swimFrameCount = mountData.runningFrameCount;
			mountData.swimFrameDelay = 10;
			mountData.swimFrameStart = mountData.runningFrameStart;
			if (Main.netMode != 2)
			{
				mountData.backTexture = TextureAssets.Extra[149];
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = Asset<Texture2D>.Empty;
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.backTexture.Width();
				mountData.textureHeight = mountData.backTexture.Height();
			}
			mountData = new Mount.MountData();
			Mount.mounts[40] = mountData;
			Mount.SetAsHorse(mountData, 275, TextureAssets.Extra[161]);
			mountData = new Mount.MountData();
			Mount.mounts[41] = mountData;
			Mount.SetAsHorse(mountData, 276, TextureAssets.Extra[162]);
			mountData = new Mount.MountData();
			Mount.mounts[42] = mountData;
			Mount.SetAsHorse(mountData, 277, TextureAssets.Extra[163]);
			mountData = new Mount.MountData();
			Mount.mounts[43] = mountData;
			mountData.spawnDust = 15;
			mountData.buff = 278;
			mountData.heightBoost = 12;
			mountData.flightTimeMax = 0;
			mountData.fallDamage = 0.25f;
			mountData.extraFall = 20;
			mountData.runSpeed = 5f;
			mountData.acceleration = 0.1f;
			mountData.jumpHeight = 8;
			mountData.jumpSpeed = 8f;
			mountData.constantJump = true;
			mountData.totalFrames = 4;
			array = new int[mountData.totalFrames];
			for (int num15 = 0; num15 < array.Length; num15++)
			{
				array[num15] = 14;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = 5;
			mountData.bodyFrame = 4;
			mountData.yOffset = 10;
			mountData.playerHeadOffset = 10;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 5;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 4;
			mountData.runningFrameDelay = 5;
			mountData.runningFrameStart = 0;
			mountData.inAirFrameCount = 1;
			mountData.inAirFrameDelay = 5;
			mountData.inAirFrameStart = 0;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			mountData.swimFrameCount = 1;
			mountData.swimFrameDelay = 5;
			mountData.swimFrameStart = 0;
			if (Main.netMode != 2)
			{
				mountData.backTexture = Asset<Texture2D>.Empty;
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = TextureAssets.Extra[164];
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.frontTexture.Width();
				mountData.textureHeight = mountData.frontTexture.Height();
			}
			mountData = new Mount.MountData();
			Mount.mounts[44] = mountData;
			mountData.spawnDust = 228;
			mountData.buff = 279;
			mountData.heightBoost = 24;
			mountData.flightTimeMax = 320;
			mountData.fatigueMax = 320;
			mountData.fallDamage = 0f;
			mountData.usesHover = true;
			mountData.runSpeed = 3f;
			mountData.dashSpeed = 6f;
			mountData.acceleration = 0.12f;
			mountData.jumpHeight = 3;
			mountData.jumpSpeed = 1f;
			mountData.swimSpeed = mountData.runSpeed;
			mountData.blockExtraJumps = true;
			mountData.totalFrames = 10;
			array = new int[mountData.totalFrames];
			for (int num16 = 0; num16 < array.Length; num16++)
			{
				array[num16] = 9;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = 0;
			mountData.bodyFrame = 3;
			mountData.yOffset = 8;
			mountData.playerHeadOffset = 16;
			mountData.runningFrameCount = 10;
			mountData.runningFrameDelay = 8;
			mountData.runningFrameStart = 0;
			if (Main.netMode != 2)
			{
				mountData.backTexture = Asset<Texture2D>.Empty;
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = TextureAssets.Extra[166];
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.frontTexture.Width();
				mountData.textureHeight = mountData.frontTexture.Height();
			}
			mountData = new Mount.MountData();
			Mount.mounts[45] = mountData;
			mountData.spawnDust = 6;
			mountData.buff = 280;
			mountData.heightBoost = 25;
			mountData.flightTimeMax = 0;
			mountData.fallDamage = 0.1f;
			mountData.runSpeed = 12f;
			mountData.dashSpeed = 16f;
			mountData.acceleration = 0.5f;
			mountData.jumpHeight = 14;
			mountData.jumpSpeed = 7f;
			mountData.emitsLight = true;
			mountData.lightColor = new Vector3(0.6f, 0.4f, 0.35f);
			mountData.totalFrames = 8;
			array = new int[mountData.totalFrames];
			for (int num17 = 0; num17 < array.Length; num17++)
			{
				array[num17] = 30;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = 0;
			mountData.bodyFrame = 0;
			mountData.xOffset = 2;
			mountData.yOffset = 1;
			mountData.playerHeadOffset = 20;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 20;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 6;
			mountData.runningFrameDelay = 20;
			mountData.runningFrameStart = 2;
			mountData.inAirFrameCount = 1;
			mountData.inAirFrameDelay = 20;
			mountData.inAirFrameStart = 1;
			mountData.swimFrameCount = mountData.runningFrameCount;
			mountData.swimFrameDelay = 20;
			mountData.swimFrameStart = mountData.runningFrameStart;
			if (Main.netMode != 2)
			{
				mountData.backTexture = TextureAssets.Extra[167];
				mountData.backTextureGlow = TextureAssets.GlowMask[283];
				mountData.frontTexture = Asset<Texture2D>.Empty;
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.backTexture.Width();
				mountData.textureHeight = mountData.backTexture.Height();
			}
			mountData = new Mount.MountData();
			Mount.mounts[46] = mountData;
			mountData.spawnDust = 15;
			mountData.buff = 281;
			mountData.heightBoost = 0;
			mountData.flightTimeMax = 0;
			mountData.fatigueMax = 0;
			mountData.fallDamage = 0f;
			mountData.abilityChargeMax = 40;
			mountData.abilityCooldown = 40;
			mountData.abilityDuration = 0;
			mountData.runSpeed = 8f;
			mountData.dashSpeed = 8f;
			mountData.acceleration = 0.4f;
			mountData.jumpHeight = 8;
			mountData.jumpSpeed = 9.01f;
			mountData.blockExtraJumps = false;
			mountData.totalFrames = 27;
			array = new int[mountData.totalFrames];
			for (int num18 = 0; num18 < array.Length; num18++)
			{
				array[num18] = 4;
				if (num18 == 1 || num18 == 2 || num18 == 7 || num18 == 8)
				{
					array[num18] += 2;
				}
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = 1;
			mountData.bodyFrame = 3;
			mountData.yOffset = 1;
			mountData.playerHeadOffset = 2;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 11;
			mountData.runningFrameDelay = 12;
			mountData.runningFrameStart = 0;
			mountData.inAirFrameCount = 11;
			mountData.inAirFrameDelay = 12;
			mountData.inAirFrameStart = 1;
			mountData.swimFrameCount = mountData.runningFrameCount;
			mountData.swimFrameDelay = mountData.runningFrameDelay;
			mountData.swimFrameStart = mountData.runningFrameStart;
			Mount.santankTextureSize = new Vector2(23f, 2f);
			if (Main.netMode != 2)
			{
				mountData.backTexture = Asset<Texture2D>.Empty;
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = TextureAssets.Extra[168];
				mountData.frontTextureExtra = TextureAssets.Extra[168];
				mountData.textureWidth = mountData.frontTexture.Width();
				mountData.textureHeight = mountData.frontTexture.Height();
			}
			mountData = new Mount.MountData();
			Mount.mounts[47] = mountData;
			mountData.spawnDust = 5;
			mountData.buff = 282;
			mountData.heightBoost = 34;
			mountData.flightTimeMax = 0;
			mountData.fallDamage = 0.2f;
			mountData.runSpeed = 4f;
			mountData.dashSpeed = 12f;
			mountData.acceleration = 0.3f;
			mountData.jumpHeight = 10;
			mountData.jumpSpeed = 8.01f;
			mountData.totalFrames = 16;
			array = new int[mountData.totalFrames];
			for (int num19 = 0; num19 < array.Length; num19++)
			{
				array[num19] = 30;
			}
			array[3] += 2;
			array[4] += 2;
			array[7] += 2;
			array[8] += 2;
			array[12] += 2;
			array[13] += 2;
			array[15] += 4;
			mountData.playerYOffsets = array;
			mountData.xOffset = 5;
			mountData.bodyFrame = 3;
			mountData.yOffset = -1;
			mountData.playerHeadOffset = 34;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 7;
			mountData.runningFrameDelay = 15;
			mountData.runningFrameStart = 1;
			mountData.dashingFrameCount = 6;
			mountData.dashingFrameDelay = 40;
			mountData.dashingFrameStart = 9;
			mountData.flyingFrameCount = 6;
			mountData.flyingFrameDelay = 6;
			mountData.flyingFrameStart = 1;
			mountData.inAirFrameCount = 1;
			mountData.inAirFrameDelay = 12;
			mountData.inAirFrameStart = 15;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			mountData.swimFrameCount = mountData.inAirFrameCount;
			mountData.swimFrameDelay = mountData.inAirFrameDelay;
			mountData.swimFrameStart = mountData.inAirFrameStart;
			if (Main.netMode != 2)
			{
				mountData.backTexture = TextureAssets.Extra[169];
				mountData.backTextureGlow = TextureAssets.GlowMask[284];
				mountData.frontTexture = Asset<Texture2D>.Empty;
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.backTexture.Width();
				mountData.textureHeight = mountData.backTexture.Height();
			}
			mountData = new Mount.MountData();
			Mount.mounts[48] = mountData;
			mountData.spawnDust = 62;
			mountData.buff = 283;
			mountData.heightBoost = 14;
			mountData.flightTimeMax = 320;
			mountData.fallDamage = 0f;
			mountData.usesHover = true;
			mountData.runSpeed = 8f;
			mountData.dashSpeed = 8f;
			mountData.acceleration = 0.2f;
			mountData.jumpHeight = 5;
			mountData.jumpSpeed = 6f;
			mountData.swimSpeed = mountData.runSpeed;
			mountData.totalFrames = 6;
			array = new int[mountData.totalFrames];
			for (int num20 = 0; num20 < array.Length; num20++)
			{
				array[num20] = 9;
			}
			array[0] += 6;
			array[1] += 6;
			array[2] += 4;
			array[3] += 4;
			array[4] += 4;
			array[5] += 6;
			mountData.playerYOffsets = array;
			mountData.xOffset = 1;
			mountData.bodyFrame = 0;
			mountData.yOffset = 16;
			mountData.playerHeadOffset = 16;
			mountData.runningFrameCount = 6;
			mountData.runningFrameDelay = 8;
			mountData.runningFrameStart = 0;
			if (Main.netMode != 2)
			{
				mountData.backTexture = TextureAssets.Extra[170];
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = Asset<Texture2D>.Empty;
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.backTexture.Width();
				mountData.textureHeight = mountData.backTexture.Height();
			}
			mountData = new Mount.MountData();
			Mount.mounts[49] = mountData;
			mountData.spawnDust = 35;
			mountData.buff = 305;
			mountData.heightBoost = 8;
			mountData.runSpeed = 2f;
			mountData.dashSpeed = 1f;
			mountData.acceleration = 0.4f;
			mountData.jumpHeight = 4;
			mountData.jumpSpeed = 3f;
			mountData.swimSpeed = 14f;
			mountData.blockExtraJumps = true;
			mountData.flightTimeMax = 0;
			mountData.fatigueMax = 320;
			mountData.usesHover = true;
			mountData.emitsLight = true;
			mountData.lightColor = new Vector3(0.3f, 0.15f, 0.1f);
			mountData.totalFrames = 8;
			array = new int[mountData.totalFrames];
			for (int num21 = 0; num21 < array.Length; num21++)
			{
				array[num21] = 10;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = 2;
			mountData.bodyFrame = 3;
			mountData.yOffset = 1;
			mountData.playerHeadOffset = 16;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 4;
			mountData.runningFrameCount = 4;
			mountData.runningFrameDelay = 14;
			mountData.runningFrameStart = 4;
			mountData.inAirFrameCount = 1;
			mountData.inAirFrameDelay = 6;
			mountData.inAirFrameStart = 4;
			mountData.swimFrameCount = 4;
			mountData.swimFrameDelay = 16;
			mountData.swimFrameStart = 0;
			if (Main.netMode != 2)
			{
				mountData.backTexture = TextureAssets.Extra[172];
				mountData.backTextureGlow = TextureAssets.GlowMask[285];
				mountData.frontTexture = Asset<Texture2D>.Empty;
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.backTexture.Width();
				mountData.textureHeight = mountData.backTexture.Height();
			}
			mountData = new Mount.MountData();
			Mount.mounts[50] = mountData;
			mountData.spawnDust = 243;
			mountData.buff = 318;
			mountData.heightBoost = 20;
			mountData.flightTimeMax = 80;
			mountData.fallDamage = 0.5f;
			mountData.runSpeed = 5.5f;
			mountData.dashSpeed = 5.5f;
			mountData.acceleration = 0.2f;
			mountData.jumpHeight = 10;
			mountData.jumpSpeed = 7.25f;
			mountData.constantJump = true;
			mountData.totalFrames = 8;
			array = new int[mountData.totalFrames];
			for (int num22 = 0; num22 < array.Length; num22++)
			{
				array[num22] = 20;
			}
			array[1] += 2;
			array[4] += 2;
			array[5] += 2;
			mountData.playerYOffsets = array;
			mountData.xOffset = 1;
			mountData.bodyFrame = 3;
			mountData.yOffset = -1;
			mountData.playerHeadOffset = 22;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 5;
			mountData.runningFrameDelay = 16;
			mountData.runningFrameStart = 0;
			mountData.flyingFrameCount = 0;
			mountData.flyingFrameDelay = 0;
			mountData.flyingFrameStart = 0;
			mountData.inAirFrameCount = 1;
			mountData.inAirFrameDelay = 12;
			mountData.inAirFrameStart = 5;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			if (Main.netMode != 2)
			{
				mountData.backTexture = TextureAssets.Extra[204];
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = Asset<Texture2D>.Empty;
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.backTexture.Width();
				mountData.textureHeight = mountData.backTexture.Height();
			}
			mountData = new Mount.MountData();
			Mount.mounts[52] = mountData;
			mountData.delegations = new Mount.MountDelegatesData();
			mountData.delegations.MouthPosition = new Mount.MountDelegatesData.OverridePositionMethod(DelegateMethods.Mount.WolfMouthPosition);
			mountData.delegations.HandPosition = new Mount.MountDelegatesData.OverridePositionMethod(DelegateMethods.Mount.NoPosition);
			mountData.spawnDust = 31;
			mountData.buff = 342;
			mountData.flightTimeMax = 0;
			mountData.fallDamage = 0.1f;
			mountData.runSpeed = 9.5f;
			mountData.acceleration = 0.18f;
			mountData.jumpHeight = 18;
			mountData.jumpSpeed = 9.01f;
			mountData.totalFrames = 15;
			array = new int[mountData.totalFrames];
			for (int num23 = 0; num23 < array.Length; num23++)
			{
				array[num23] = 0;
			}
			array[1] += -14;
			array[2] += -10;
			array[3] += -8;
			array[4] += 18;
			array[5] += -2;
			int[] array2 = array;
			int num24 = 6;
			array2[num24] = array2[num24];
			array[7] += 2;
			array[8] += 4;
			array[9] += 4;
			array[10] += 2;
			int[] array3 = array;
			int num25 = 11;
			array3[num25] = array3[num25];
			array[12] += 4;
			array[13] += 2;
			array[14] += -4;
			mountData.playerYOffsets = array;
			mountData.xOffset = 4;
			mountData.bodyFrame = 3;
			mountData.yOffset = -1;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 6;
			mountData.runningFrameDelay = 20;
			mountData.runningFrameStart = 5;
			mountData.inAirFrameCount = 3;
			mountData.inAirFrameDelay = 12;
			mountData.inAirFrameStart = 12;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			mountData.swimFrameCount = mountData.inAirFrameCount;
			mountData.swimFrameDelay = mountData.inAirFrameDelay;
			mountData.swimFrameStart = mountData.inAirFrameStart;
			if (Main.netMode != 2)
			{
				mountData.backTexture = Asset<Texture2D>.Empty;
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = TextureAssets.Extra[253];
				mountData.frontTextureExtra = TextureAssets.Extra[254];
				mountData.textureWidth = mountData.frontTexture.Width();
				mountData.textureHeight = mountData.frontTexture.Height();
			}
			mountData = new Mount.MountData();
			Mount.mounts[54] = mountData;
			mountData.delegations = new Mount.MountDelegatesData();
			mountData.delegations.MouthPosition = new Mount.MountDelegatesData.OverridePositionMethod(DelegateMethods.Mount.VelociraptorMouthPosition);
			mountData.delegations.HandPosition = new Mount.MountDelegatesData.OverridePositionMethod(DelegateMethods.Mount.NoPosition);
			mountData.spawnDust = 31;
			mountData.buff = 370;
			mountData.flightTimeMax = 0;
			mountData.fallDamage = 0.1f;
			mountData.runSpeed = 4.5f;
			mountData.dashSpeed = 7.5f;
			mountData.acceleration = 0.15f;
			mountData.jumpHeight = 15;
			mountData.jumpSpeed = 6.01f;
			mountData.totalFrames = 21;
			array = new int[mountData.totalFrames];
			for (int num26 = 0; num26 < array.Length; num26++)
			{
				array[num26] = 0;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = 4;
			mountData.bodyFrame = 0;
			mountData.yOffset = -4;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 7;
			mountData.runningFrameDelay = 20;
			mountData.runningFrameStart = 8;
			mountData.inAirFrameCount = 2;
			mountData.inAirFrameDelay = 6;
			mountData.inAirFrameStart = 5;
			mountData.flyingFrameCount = 5;
			mountData.flyingFrameDelay = 6;
			mountData.flyingFrameStart = 16;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			mountData.swimFrameCount = mountData.inAirFrameCount;
			mountData.swimFrameDelay = mountData.inAirFrameDelay;
			mountData.swimFrameStart = mountData.inAirFrameStart;
			if (Main.netMode != 2)
			{
				mountData.backTexture = Asset<Texture2D>.Empty;
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = TextureAssets.Extra[271];
				mountData.frontTextureExtra = TextureAssets.Extra[272];
				mountData.textureWidth = mountData.frontTexture.Width();
				mountData.textureHeight = mountData.frontTexture.Height();
			}
			mountData = new Mount.MountData();
			Mount.mounts[55] = mountData;
			mountData.delegations = new Mount.MountDelegatesData();
			mountData.delegations.MouthPosition = new Mount.MountDelegatesData.OverridePositionMethod(DelegateMethods.Mount.NoPosition);
			mountData.delegations.HandPosition = new Mount.MountDelegatesData.OverridePositionMethod(DelegateMethods.Mount.NoPosition);
			mountData.delegations.PlayerSize = new Mount.MountDelegatesData.OverrideSizeMethod(DelegateMethods.Mount.RatPlayerSize);
			mountData.spawnDust = 31;
			mountData.buff = 374;
			mountData.flightTimeMax = 0;
			mountData.fallDamage = 0.1f;
			mountData.dismountsOnItemUse = true;
			mountData.runSpeed = 4.5f;
			mountData.dashSpeed = 7.5f;
			mountData.acceleration = 0.15f;
			mountData.jumpHeight = 15;
			mountData.jumpSpeed = 6.01f;
			mountData.totalFrames = 16;
			array = new int[mountData.totalFrames];
			for (int num27 = 0; num27 < array.Length; num27++)
			{
				array[num27] = 0;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = 0;
			mountData.bodyFrame = 3;
			mountData.yOffset = -2;
			mountData.playerHeadOffset = -30;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 7;
			mountData.runningFrameDelay = 12;
			mountData.runningFrameStart = 4;
			mountData.dashingFrameCount = 5;
			mountData.dashingFrameDelay = 16;
			mountData.dashingFrameStart = 11;
			mountData.inAirFrameCount = 3;
			mountData.inAirFrameDelay = 6;
			mountData.inAirFrameStart = 12;
			mountData.idleFrameCount = 4;
			mountData.idleFrameDelay = 16;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			mountData.swimFrameCount = mountData.inAirFrameCount;
			mountData.swimFrameDelay = mountData.inAirFrameDelay;
			mountData.swimFrameStart = mountData.inAirFrameStart;
			if (Main.netMode != 2)
			{
				mountData.backTexture = Asset<Texture2D>.Empty;
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = TextureAssets.Extra[273];
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.frontTexture.Width();
				mountData.textureHeight = mountData.frontTexture.Height();
			}
			mountData = new Mount.MountData();
			Mount.mounts[56] = mountData;
			mountData.delegations = new Mount.MountDelegatesData();
			mountData.delegations.MouthPosition = new Mount.MountDelegatesData.OverridePositionMethod(DelegateMethods.Mount.NoPosition);
			mountData.delegations.HandPosition = new Mount.MountDelegatesData.OverridePositionMethod(DelegateMethods.Mount.NoPosition);
			mountData.delegations.PlayerSize = new Mount.MountDelegatesData.OverrideSizeMethod(DelegateMethods.Mount.BatPlayerSize);
			mountData.delegations.DashDust = new Mount.MountDelegatesData.AdjustDashDustMethod(DelegateMethods.Mount.BatDashDust);
			mountData.spawnDust = 31;
			mountData.buff = 377;
			mountData.flightTimeMax = 320;
			mountData.fatigueMax = 320;
			mountData.fallDamage = 0f;
			mountData.usesHover = true;
			mountData.dismountsOnItemUse = true;
			mountData.blockExtraJumps = true;
			mountData.dashSpeed = 4.5f;
			mountData.runSpeed = mountData.dashSpeed;
			mountData.acceleration = 0.2f;
			mountData.jumpHeight = 8;
			mountData.jumpSpeed = 5f;
			mountData.totalFrames = 16;
			array = new int[mountData.totalFrames];
			for (int num28 = 0; num28 < array.Length; num28++)
			{
				array[num28] = 0;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = -4;
			mountData.bodyFrame = 3;
			mountData.yOffset = -1;
			mountData.playerHeadOffset = -24;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 6;
			mountData.standingFrameStart = 0;
			mountData.dashingFrameCount = 6;
			mountData.dashingFrameDelay = 40;
			mountData.dashingFrameStart = 9;
			mountData.flyingFrameCount = 8;
			mountData.flyingFrameDelay = 5;
			mountData.flyingFrameStart = 1;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;
			mountData.runningFrameCount = mountData.flyingFrameCount;
			mountData.runningFrameDelay = mountData.flyingFrameDelay;
			mountData.runningFrameStart = mountData.flyingFrameStart;
			mountData.inAirFrameCount = mountData.flyingFrameCount;
			mountData.inAirFrameDelay = mountData.flyingFrameDelay;
			mountData.inAirFrameStart = mountData.flyingFrameStart;
			mountData.swimFrameCount = mountData.flyingFrameCount;
			mountData.swimFrameDelay = mountData.flyingFrameDelay;
			mountData.swimFrameStart = mountData.flyingFrameStart;
			if (Main.netMode != 2)
			{
				mountData.backTexture = Asset<Texture2D>.Empty;
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = TextureAssets.Extra[274];
				mountData.frontTextureGlow = TextureAssets.GlowMask[370];
				mountData.frontTextureExtra = Asset<Texture2D>.Empty;
				mountData.textureWidth = mountData.frontTexture.Width();
				mountData.textureHeight = mountData.frontTexture.Height();
			}
			mountData = new Mount.MountData();
			Mount.mounts[61] = mountData;
			mountData.delegations = new Mount.MountDelegatesData();
			mountData.delegations.MouthPosition = new Mount.MountDelegatesData.OverridePositionMethod(DelegateMethods.Mount.NoPosition);
			mountData.delegations.HandPosition = new Mount.MountDelegatesData.OverridePositionMethod(DelegateMethods.Mount.NoPosition);
			mountData.delegations.PlayerSize = new Mount.MountDelegatesData.OverrideSizeMethod(DelegateMethods.Mount.PixiePlayerSize);
			mountData.delegations.DashDust = new Mount.MountDelegatesData.AdjustDashDustMethod(DelegateMethods.Mount.PixieDashDust);
			mountData.spawnDust = 43;
			mountData.spawnDustNoGravity = true;
			mountData.buff = 384;
			mountData.flightTimeMax = 320;
			mountData.fatigueMax = 320;
			mountData.fallDamage = 0f;
			mountData.usesHover = true;
			mountData.dismountsOnItemUse = true;
			mountData.blockExtraJumps = true;
			mountData.dashSpeed = 4.5f;
			mountData.runSpeed = mountData.dashSpeed;
			mountData.acceleration = 0.2f;
			mountData.jumpHeight = 8;
			mountData.jumpSpeed = 5f;
			mountData.totalFrames = 4;
			array = new int[mountData.totalFrames];
			for (int num29 = 0; num29 < array.Length; num29++)
			{
				array[num29] = 0;
			}
			mountData.playerYOffsets = array;
			mountData.xOffset = -2;
			mountData.yOffset = -1;
			mountData.bodyFrame = 0;
			mountData.playerHeadOffset = -24;
			mountData.standingFrameCount = 4;
			mountData.standingFrameDelay = 8;
			mountData.standingFrameStart = 0;
			mountData.dashingFrameCount = 4;
			mountData.dashingFrameDelay = 2;
			mountData.dashingFrameStart = 0;
			mountData.flyingFrameCount = 4;
			mountData.flyingFrameDelay = 6;
			mountData.flyingFrameStart = 0;
			mountData.idleFrameCount = 4;
			mountData.idleFrameDelay = 8;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = true;
			mountData.runningFrameCount = mountData.flyingFrameCount;
			mountData.runningFrameDelay = mountData.flyingFrameDelay;
			mountData.runningFrameStart = mountData.flyingFrameStart;
			mountData.inAirFrameCount = mountData.flyingFrameCount;
			mountData.inAirFrameDelay = mountData.flyingFrameDelay;
			mountData.inAirFrameStart = mountData.flyingFrameStart;
			mountData.swimFrameCount = mountData.flyingFrameCount;
			mountData.swimFrameDelay = mountData.flyingFrameDelay;
			mountData.swimFrameStart = mountData.flyingFrameStart;
			if (Main.netMode != 2)
			{
				mountData.backTexture = Asset<Texture2D>.Empty;
				mountData.backTextureExtra = Asset<Texture2D>.Empty;
				mountData.frontTexture = TextureAssets.Extra[290];
				mountData.frontTextureGlow = Asset<Texture2D>.Empty;
				mountData.frontTextureExtra = TextureAssets.Extra[291];
				mountData.textureWidth = mountData.frontTexture.Width();
				mountData.textureHeight = mountData.frontTexture.Height();
			}
			mountData = new Mount.MountData();
			Mount.mounts[57] = mountData;
			Mount.SetAsRollerSkate(mountData, 378);
			mountData = new Mount.MountData();
			Mount.mounts[58] = mountData;
			Mount.SetAsRollerSkate(mountData, 379);
			mountData = new Mount.MountData();
			Mount.mounts[59] = mountData;
			Mount.SetAsRollerSkate(mountData, 380);
			mountData = new Mount.MountData();
			Mount.mounts[60] = mountData;
			Mount.SetAsRollerSkate(mountData, 381);
			mountData = new Mount.MountData();
			Mount.mounts[62] = mountData;
			Mount.SetAsChillet(mountData, 387, TextureAssets.Extra[301]);
			mountData = new Mount.MountData();
			Mount.mounts[63] = mountData;
			Mount.SetAsChillet(mountData, 388, TextureAssets.Extra[302]);
		}

		// Token: 0x06000244 RID: 580 RVA: 0x00032DC8 File Offset: 0x00030FC8
		public static void SetAsRollerSkate(Mount.MountData newMount, int buff)
		{
			newMount.spawnDust = 31;
			newMount.buff = buff;
			newMount.CanRideMinecartTracks = true;
			newMount.CanUseWings = true;
			newMount.dashSpeed = 7.5f;
			newMount.runSpeed = newMount.dashSpeed;
			newMount.acceleration = 0.3f;
			newMount.jumpHeight = 14;
			newMount.jumpSpeed = 7f;
			newMount.fallDamage = 1f;
			newMount.blockExtraJumps = false;
			newMount.totalFrames = 1;
			int[] array = new int[newMount.totalFrames];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = 2;
			}
			newMount.playerYOffsets = array;
			newMount.xOffset = 0;
			newMount.bodyFrame = 3;
			newMount.yOffset = 0;
			newMount.standingFrameCount = 1;
			newMount.standingFrameDelay = 6;
			newMount.standingFrameStart = 0;
			if (Main.netMode != 2)
			{
				newMount.backTexture = Asset<Texture2D>.Empty;
				newMount.backTextureExtra = Asset<Texture2D>.Empty;
				newMount.frontTexture = Asset<Texture2D>.Empty;
				newMount.frontTextureExtra = Asset<Texture2D>.Empty;
				newMount.textureWidth = 1;
				newMount.textureHeight = 1;
			}
		}

		// Token: 0x06000245 RID: 581 RVA: 0x00032ED4 File Offset: 0x000310D4
		public static void SetAsHorse(Mount.MountData newMount, int buff, Asset<Texture2D> texture)
		{
			newMount.spawnDust = 3;
			newMount.buff = buff;
			newMount.heightBoost = 34;
			newMount.flightTimeMax = 0;
			newMount.fallDamage = 0.5f;
			newMount.runSpeed = 3f;
			newMount.dashSpeed = 9f;
			newMount.acceleration = 0.25f;
			newMount.jumpHeight = 6;
			newMount.jumpSpeed = 7.01f;
			newMount.totalFrames = 16;
			int[] array = new int[newMount.totalFrames];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = 28;
			}
			array[3] += 2;
			array[4] += 2;
			array[7] += 2;
			array[8] += 2;
			array[12] += 2;
			array[13] += 2;
			array[15] += 4;
			newMount.playerYOffsets = array;
			newMount.xOffset = 5;
			newMount.bodyFrame = 3;
			newMount.yOffset = 1;
			newMount.playerHeadOffset = 34;
			newMount.standingFrameCount = 1;
			newMount.standingFrameDelay = 12;
			newMount.standingFrameStart = 0;
			newMount.runningFrameCount = 7;
			newMount.runningFrameDelay = 15;
			newMount.runningFrameStart = 1;
			newMount.dashingFrameCount = 6;
			newMount.dashingFrameDelay = 40;
			newMount.dashingFrameStart = 9;
			newMount.flyingFrameCount = 6;
			newMount.flyingFrameDelay = 6;
			newMount.flyingFrameStart = 1;
			newMount.inAirFrameCount = 1;
			newMount.inAirFrameDelay = 12;
			newMount.inAirFrameStart = 15;
			newMount.idleFrameCount = 0;
			newMount.idleFrameDelay = 0;
			newMount.idleFrameStart = 0;
			newMount.idleFrameLoop = false;
			newMount.swimFrameCount = newMount.inAirFrameCount;
			newMount.swimFrameDelay = newMount.inAirFrameDelay;
			newMount.swimFrameStart = newMount.inAirFrameStart;
			if (Main.netMode != 2)
			{
				newMount.backTexture = texture;
				newMount.backTextureExtra = Asset<Texture2D>.Empty;
				newMount.frontTexture = Asset<Texture2D>.Empty;
				newMount.frontTextureExtra = Asset<Texture2D>.Empty;
				newMount.textureWidth = newMount.backTexture.Width();
				newMount.textureHeight = newMount.backTexture.Height();
			}
		}

		// Token: 0x06000246 RID: 582 RVA: 0x000330E0 File Offset: 0x000312E0
		public static void SetAsChillet(Mount.MountData newMount, int buff, Asset<Texture2D> texture)
		{
			newMount.spawnDust = 15;
			newMount.buff = buff;
			newMount.heightBoost = 4;
			newMount.flightTimeMax = 0;
			newMount.fallDamage = 0.5f;
			newMount.runSpeed = 3f;
			newMount.dashSpeed = 9f;
			newMount.acceleration = 0.32f;
			newMount.jumpHeight = 8;
			newMount.jumpSpeed = 8.01f;
			newMount.walkingGraceTimeMax = 10;
			newMount.totalFrames = 13;
			int[] array = new int[newMount.totalFrames];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = 10;
			}
			array[4] += 2;
			array[5] += 6;
			array[6] += 2;
			int[] array2 = array;
			int num = 7;
			array2[num] = array2[num];
			array[8] += 2;
			array[9] += 6;
			array[10] += 6;
			array[11] += 4;
			newMount.playerYOffsets = array;
			newMount.xOffset = 7;
			newMount.bodyFrame = 3;
			newMount.yOffset = -10;
			newMount.playerHeadOffset = 10;
			newMount.standingFrameCount = 4;
			newMount.standingFrameDelay = 12;
			newMount.standingFrameStart = 0;
			newMount.runningFrameCount = 6;
			newMount.runningFrameDelay = 30;
			newMount.runningFrameStart = 6;
			newMount.dashingFrameCount = 7;
			newMount.dashingFrameDelay = 130;
			newMount.dashingFrameStart = 5;
			newMount.flyingFrameCount = 6;
			newMount.flyingFrameDelay = 6;
			newMount.flyingFrameStart = 1;
			newMount.inAirFrameCount = 1;
			newMount.inAirFrameDelay = 12;
			newMount.inAirFrameStart = 4;
			newMount.idleFrameCount = 4;
			newMount.idleFrameDelay = 4;
			newMount.idleFrameStart = 0;
			newMount.idleFrameLoop = true;
			newMount.swimFrameCount = newMount.inAirFrameCount;
			newMount.swimFrameDelay = newMount.inAirFrameDelay;
			newMount.swimFrameStart = newMount.inAirFrameStart;
			if (Main.netMode != 2)
			{
				newMount.backTexture = texture;
				newMount.backTextureExtra = Asset<Texture2D>.Empty;
				newMount.frontTexture = Asset<Texture2D>.Empty;
				newMount.frontTextureExtra = Asset<Texture2D>.Empty;
				newMount.textureWidth = newMount.backTexture.Width();
				newMount.textureHeight = newMount.backTexture.Height();
			}
		}

		// Token: 0x06000247 RID: 583 RVA: 0x00033300 File Offset: 0x00031500
		public static void SetAsMinecart(Mount.MountData newMount, int buff, Asset<Texture2D> texture, int verticalOffset = 0, int playerVerticalOffset = 0)
		{
			newMount.Minecart = true;
			newMount.delegations = new Mount.MountDelegatesData();
			newMount.delegations.MinecartDust = new Action<Vector2>(DelegateMethods.Minecart.Sparks);
			newMount.spawnDust = 213;
			newMount.buff = buff;
			newMount.heightBoost = 10;
			newMount.flightTimeMax = 0;
			newMount.fallDamage = 1f;
			newMount.runSpeed = 13f;
			newMount.dashSpeed = 13f;
			newMount.acceleration = 0.04f;
			newMount.jumpHeight = 15;
			newMount.jumpSpeed = 5.15f;
			newMount.blockExtraJumps = true;
			newMount.totalFrames = 3;
			int[] array = new int[newMount.totalFrames];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = 8 - verticalOffset + playerVerticalOffset;
			}
			newMount.playerYOffsets = array;
			newMount.xOffset = 1;
			newMount.bodyFrame = 3;
			newMount.yOffset = 13 + verticalOffset;
			newMount.playerHeadOffset = 14;
			newMount.standingFrameCount = 1;
			newMount.standingFrameDelay = 12;
			newMount.standingFrameStart = 0;
			newMount.runningFrameCount = 3;
			newMount.runningFrameDelay = 12;
			newMount.runningFrameStart = 0;
			newMount.flyingFrameCount = 0;
			newMount.flyingFrameDelay = 0;
			newMount.flyingFrameStart = 0;
			newMount.inAirFrameCount = 0;
			newMount.inAirFrameDelay = 0;
			newMount.inAirFrameStart = 0;
			newMount.idleFrameCount = 0;
			newMount.idleFrameDelay = 0;
			newMount.idleFrameStart = 0;
			newMount.idleFrameLoop = false;
			if (Main.netMode != 2)
			{
				newMount.backTexture = Asset<Texture2D>.Empty;
				newMount.backTextureExtra = Asset<Texture2D>.Empty;
				newMount.frontTexture = texture;
				newMount.frontTextureExtra = Asset<Texture2D>.Empty;
				newMount.textureWidth = newMount.frontTexture.Width();
				newMount.textureHeight = newMount.frontTexture.Height();
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000248 RID: 584 RVA: 0x000334B2 File Offset: 0x000316B2
		public bool Active
		{
			get
			{
				return this._active;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000249 RID: 585 RVA: 0x000334BA File Offset: 0x000316BA
		public int Type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600024A RID: 586 RVA: 0x000334C2 File Offset: 0x000316C2
		public int Frame
		{
			get
			{
				return this._frame;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600024B RID: 587 RVA: 0x000334CA File Offset: 0x000316CA
		public int FlyTime
		{
			get
			{
				return this._flyTime;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600024C RID: 588 RVA: 0x000334D2 File Offset: 0x000316D2
		public int BuffType
		{
			get
			{
				return this._data.buff;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600024D RID: 589 RVA: 0x000334DF File Offset: 0x000316DF
		public int BodyFrame
		{
			get
			{
				return this._data.bodyFrame;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600024E RID: 590 RVA: 0x000334EC File Offset: 0x000316EC
		public int XOffset
		{
			get
			{
				return this._data.xOffset;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600024F RID: 591 RVA: 0x000334F9 File Offset: 0x000316F9
		public int YOffset
		{
			get
			{
				return this._data.yOffset;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000250 RID: 592 RVA: 0x00033506 File Offset: 0x00031706
		public int RunningGraceTime
		{
			get
			{
				return this._walkingGraceTimeLeft;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000251 RID: 593 RVA: 0x0003350E File Offset: 0x0003170E
		public int PlayerXOFfset
		{
			get
			{
				return this._data.playerXOffset;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000252 RID: 594 RVA: 0x0003351B File Offset: 0x0003171B
		public int PlayerOffset
		{
			get
			{
				if (!this._active)
				{
					return 0;
				}
				if (this._frame >= this._data.totalFrames)
				{
					return 0;
				}
				return this._data.playerYOffsets[this._frame];
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000253 RID: 595 RVA: 0x0003354E File Offset: 0x0003174E
		public int PlayerOffsetHitbox
		{
			get
			{
				if (!this._active)
				{
					return 0;
				}
				return -this.PlayerOffset + this._data.heightBoost;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000254 RID: 596 RVA: 0x0003356D File Offset: 0x0003176D
		public int PlayerHeadOffset
		{
			get
			{
				if (!this._active)
				{
					return 0;
				}
				return this._data.playerHeadOffset;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000255 RID: 597 RVA: 0x00033584 File Offset: 0x00031784
		public int HeightBoost
		{
			get
			{
				return this._data.heightBoost;
			}
		}

		// Token: 0x06000256 RID: 598 RVA: 0x00033591 File Offset: 0x00031791
		public static int GetHeightBoost(int MountType)
		{
			if (MountType <= -1 || MountType >= MountID.Count)
			{
				return 0;
			}
			return Mount.mounts[MountType].heightBoost;
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000257 RID: 599 RVA: 0x000335B0 File Offset: 0x000317B0
		public float RunSpeed
		{
			get
			{
				if (this._type == 4 && this._frameState == 4)
				{
					return this._data.swimSpeed;
				}
				if ((this._type == 12 || this._type == 44 || this._type == 49) && this._frameState == 4)
				{
					return this._data.swimSpeed;
				}
				if (this._type == 12 && this._frameState == 2)
				{
					return this._data.runSpeed + 13.5f;
				}
				if (this._type == 44 && this._frameState == 2)
				{
					return this._data.runSpeed + 4f;
				}
				if (this._type == 5 && this._frameState == 2)
				{
					float num = this._fatigue / this._fatigueMax;
					return this._data.runSpeed + 4f * (1f - num);
				}
				if (this._type == 50 && this._frameState == 2)
				{
					return this._data.runSpeed + 2f;
				}
				if (this._shouldSuperCart)
				{
					return Mount.SuperCartRunSpeed;
				}
				return this._data.runSpeed;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000258 RID: 600 RVA: 0x000336D0 File Offset: 0x000318D0
		public float DashSpeed
		{
			get
			{
				if (this._shouldSuperCart)
				{
					return Mount.SuperCartDashSpeed;
				}
				return this._data.dashSpeed;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000259 RID: 601 RVA: 0x000336EB File Offset: 0x000318EB
		public float Acceleration
		{
			get
			{
				if (this._shouldSuperCart)
				{
					return Mount.SuperCartAcceleration;
				}
				return this._data.acceleration;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600025A RID: 602 RVA: 0x00033706 File Offset: 0x00031906
		public int ExtraFall
		{
			get
			{
				return this._data.extraFall;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600025B RID: 603 RVA: 0x00033713 File Offset: 0x00031913
		public float FallDamage
		{
			get
			{
				return this._data.fallDamage;
			}
		}

		// Token: 0x0600025C RID: 604 RVA: 0x00033720 File Offset: 0x00031920
		public int JumpHeight(float xVelocity)
		{
			int num = this._data.jumpHeight;
			int type = this._type;
			switch (type)
			{
			case 0:
				num += (int)(Math.Abs(xVelocity) / 4f);
				goto IL_65;
			case 1:
				num += (int)(Math.Abs(xVelocity) / 2.5f);
				goto IL_65;
			case 2:
			case 3:
				goto IL_65;
			case 4:
				break;
			default:
				if (type != 49)
				{
					goto IL_65;
				}
				break;
			}
			if (this._frameState == 4)
			{
				num += 5;
			}
			IL_65:
			if (this._shouldSuperCart)
			{
				num = Mount.SuperCartJumpHeight;
			}
			return num;
		}

		// Token: 0x0600025D RID: 605 RVA: 0x000337A4 File Offset: 0x000319A4
		public float JumpSpeed(float xVelocity)
		{
			float num = this._data.jumpSpeed;
			int type = this._type;
			if (type > 1)
			{
				if (type == 4 || type == 49)
				{
					if (this._frameState == 4)
					{
						num += 2.5f;
					}
				}
			}
			else
			{
				num += Math.Abs(xVelocity) / 7f;
			}
			if (this._shouldSuperCart)
			{
				num = Mount.SuperCartJumpSpeed;
			}
			return num;
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600025E RID: 606 RVA: 0x00033804 File Offset: 0x00031A04
		public bool AutoJump
		{
			get
			{
				return this._data.constantJump;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600025F RID: 607 RVA: 0x00033811 File Offset: 0x00031A11
		public bool BlockExtraJumps
		{
			get
			{
				return this._data.blockExtraJumps;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000260 RID: 608 RVA: 0x0003381E File Offset: 0x00031A1E
		public bool IsConsideredASlimeMount
		{
			get
			{
				return this._type == 3 || this._type == 50;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000261 RID: 609 RVA: 0x00033835 File Offset: 0x00031A35
		public bool Cart
		{
			get
			{
				return this._data != null && this._active && this._data.Minecart;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000262 RID: 610 RVA: 0x00033854 File Offset: 0x00031A54
		public bool CanGrindRails
		{
			get
			{
				return this._data != null && this._active && this._data.CanRideMinecartTracks;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000263 RID: 611 RVA: 0x00033873 File Offset: 0x00031A73
		public bool AnyTrackRider
		{
			get
			{
				return this._data != null && this._active && (this._data.Minecart || this._data.CanRideMinecartTracks);
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000264 RID: 612 RVA: 0x000338A1 File Offset: 0x00031AA1
		public bool CanUseWings
		{
			get
			{
				return this._data == null || !this._active || this._data.CanUseWings;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000265 RID: 613 RVA: 0x000338C0 File Offset: 0x00031AC0
		public Mount.MountDelegatesData Delegations
		{
			get
			{
				if (this._data == null)
				{
					return this._defaultDelegatesData;
				}
				return this._data.delegations;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000266 RID: 614 RVA: 0x000338DC File Offset: 0x00031ADC
		public Vector2 Origin
		{
			get
			{
				return new Vector2((float)this._data.textureWidth / 2f, (float)this._data.textureHeight / (2f * (float)this._data.totalFrames));
			}
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00033914 File Offset: 0x00031B14
		public bool CanFly(Player mountedPlayer)
		{
			return this._active && ((this._type == 54 && ((Mount.SelectiveFlyingMountData)this._mountSpecificData).allowedToFly) || (this._data.flightTimeMax != 0 && this._type != 48));
		}

		// Token: 0x06000268 RID: 616 RVA: 0x00033965 File Offset: 0x00031B65
		public bool CanHover()
		{
			if (!this._active || !this._data.usesHover)
			{
				return false;
			}
			if (this._type == 49)
			{
				return this._frameState == 4;
			}
			return this._data.usesHover;
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000269 RID: 617 RVA: 0x000339A2 File Offset: 0x00031BA2
		public bool AbilityCharging
		{
			get
			{
				return this._abilityCharging;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600026A RID: 618 RVA: 0x000339AA File Offset: 0x00031BAA
		public bool AbilityActive
		{
			get
			{
				return this._abilityActive;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600026B RID: 619 RVA: 0x000339B2 File Offset: 0x00031BB2
		public float AbilityCharge
		{
			get
			{
				return (float)this._abilityCharge / (float)this._data.abilityChargeMax;
			}
		}

		// Token: 0x0600026C RID: 620 RVA: 0x000339C8 File Offset: 0x00031BC8
		public IEntitySource GetProjectileSpawnSource(Player mountedPlayer)
		{
			return new EntitySource_Mount(mountedPlayer, this.Type);
		}

		// Token: 0x0600026D RID: 621 RVA: 0x000339D8 File Offset: 0x00031BD8
		public void StartAbilityCharge(Player mountedPlayer)
		{
			if (Main.myPlayer == mountedPlayer.whoAmI)
			{
				int type = this._type;
				if (type == 9)
				{
					int type2 = 441;
					float num = Main.screenPosition.X + (float)Main.mouseX;
					float num2 = Main.screenPosition.Y + (float)Main.mouseY;
					float ai = num - mountedPlayer.position.X;
					float ai2 = num2 - mountedPlayer.position.Y;
					Projectile.NewProjectile(this.GetProjectileSpawnSource(mountedPlayer), num, num2, 0f, 0f, type2, 0, 0f, mountedPlayer.whoAmI, ai, ai2, 0f, null);
					this._abilityCharging = true;
					return;
				}
			}
			else
			{
				int type = this._type;
				if (type == 9)
				{
					this._abilityCharging = true;
				}
			}
		}

		// Token: 0x0600026E RID: 622 RVA: 0x00033A98 File Offset: 0x00031C98
		public void StopAbilityCharge()
		{
			int type = this._type;
			if (type == 9 || type == 46)
			{
				this._abilityCharging = false;
				this._abilityCooldown = this._data.abilityCooldown;
				this._abilityDuration = this._data.abilityDuration;
			}
		}

		// Token: 0x0600026F RID: 623 RVA: 0x00033ADF File Offset: 0x00031CDF
		public bool CheckBuff(int buffID)
		{
			return this._data.buff == buffID;
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00033AF0 File Offset: 0x00031CF0
		public void AbilityRecovery()
		{
			if (this._abilityCharging)
			{
				if (this._abilityCharge < this._data.abilityChargeMax)
				{
					this._abilityCharge++;
				}
			}
			else if (this._abilityCharge > 0)
			{
				this._abilityCharge--;
			}
			if (this._abilityCooldown > 0)
			{
				this._abilityCooldown--;
			}
			if (this._abilityDuration > 0)
			{
				this._abilityDuration--;
			}
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00033B6D File Offset: 0x00031D6D
		public void FatigueRecovery()
		{
			if (this._fatigue > 2f)
			{
				this._fatigue -= 2f;
				return;
			}
			this._fatigue = 0f;
		}

		// Token: 0x06000272 RID: 626 RVA: 0x00033B9A File Offset: 0x00031D9A
		public bool Flight()
		{
			if (this._flyTime <= 0)
			{
				return false;
			}
			this._flyTime--;
			return true;
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000273 RID: 627 RVA: 0x00033BB8 File Offset: 0x00031DB8
		public bool AllowDirectionChange
		{
			get
			{
				int type = this._type;
				return type != 9 || this._abilityCooldown < this._data.abilityCooldown / 2;
			}
		}

		// Token: 0x06000274 RID: 628 RVA: 0x00033BE8 File Offset: 0x00031DE8
		public void UpdateAfterEquips(Player mountedPlayer)
		{
			if (!this._active)
			{
				return;
			}
			if (this._type == 54)
			{
				((Mount.SelectiveFlyingMountData)this._mountSpecificData).allowedToFly = (mountedPlayer.wingsLogic > 0);
			}
			if (this._type == 55)
			{
				mountedPlayer.spikedBoots++;
				mountedPlayer.fullRotationOrigin = mountedPlayer.Size / 2f;
				if (mountedPlayer.slideDir >= 1)
				{
					if (1.5707964f - Math.Abs(mountedPlayer.fullRotation) <= 0.1f)
					{
						mountedPlayer.fullRotation = -1.5707964f;
						return;
					}
					mountedPlayer.fullRotation = mountedPlayer.fullRotation.AngleLerp(-1.5707964f, 0.5f);
					return;
				}
				else if (mountedPlayer.slideDir <= -1)
				{
					if (1.5707964f - Math.Abs(mountedPlayer.fullRotation) <= 0.1f)
					{
						mountedPlayer.fullRotation = 1.5707964f;
						return;
					}
					mountedPlayer.fullRotation = mountedPlayer.fullRotation.AngleLerp(1.5707964f, 0.5f);
					return;
				}
				else
				{
					if (Math.Abs(mountedPlayer.fullRotation) <= 0.1f)
					{
						mountedPlayer.fullRotation = 0f;
						return;
					}
					mountedPlayer.fullRotation = mountedPlayer.fullRotation.AngleLerp(0f, 0.5f);
				}
			}
		}

		// Token: 0x06000275 RID: 629 RVA: 0x00033D20 File Offset: 0x00031F20
		public void UpdateDrill(Player mountedPlayer, bool controlUp, bool controlDown)
		{
			Mount.DrillMountData drillMountData = (Mount.DrillMountData)this._mountSpecificData;
			for (int i = 0; i < drillMountData.beams.Length; i++)
			{
				Mount.DrillBeam drillBeam = drillMountData.beams[i];
				if (drillBeam.cooldown > 1)
				{
					drillBeam.cooldown--;
				}
				else if (drillBeam.cooldown == 1)
				{
					drillBeam.cooldown = 0;
					drillBeam.curTileTarget = Point16.NegativeOne;
				}
			}
			drillMountData.diodeRotation = drillMountData.diodeRotation * 0.85f + 0.15f * drillMountData.diodeRotationTarget;
			if (drillMountData.beamCooldown > 0)
			{
				drillMountData.beamCooldown--;
			}
		}

		// Token: 0x06000276 RID: 630 RVA: 0x00033DC0 File Offset: 0x00031FC0
		public void UseDrill(Player mountedPlayer)
		{
			if (this._type != 8 || !this._abilityActive)
			{
				return;
			}
			Mount.DrillMountData drillMountData = (Mount.DrillMountData)this._mountSpecificData;
			bool flag = mountedPlayer.whoAmI == Main.myPlayer;
			if (mountedPlayer.controlUseItem)
			{
				int num = 0;
				while (num < Mount.amountOfBeamsAtOnce && drillMountData.beamCooldown == 0)
				{
					for (int i = 0; i < drillMountData.beams.Length; i++)
					{
						Mount.DrillBeam drillBeam = drillMountData.beams[i];
						if (drillBeam.cooldown == 0)
						{
							Point16 point = this.DrillSmartCursor_Blocks(mountedPlayer, drillMountData);
							if (!(point == Point16.NegativeOne))
							{
								drillBeam.curTileTarget = point;
								int pickPower = Mount.drillPickPower;
								if (flag)
								{
									bool flag2 = true;
									if (WorldGen.InWorld((int)point.X, (int)point.Y, 0) && Main.tile[(int)point.X, (int)point.Y] != null && Main.tile[(int)point.X, (int)point.Y].type == 26 && !Main.hardMode)
									{
										flag2 = false;
										mountedPlayer.Hurt(PlayerDeathReason.ByOther(4), mountedPlayer.statLife / 2, -mountedPlayer.direction, false, false, false, -1, true);
									}
									if (mountedPlayer.noBuilding)
									{
										flag2 = false;
									}
									if (flag2)
									{
										mountedPlayer.PickTile((int)point.X, (int)point.Y, pickPower);
									}
								}
								Vector2 vector = new Vector2((float)(point.X << 4) + 8f, (float)(point.Y << 4) + 8f);
								float num2 = (vector - mountedPlayer.Center).ToRotation();
								for (int j = 0; j < 2; j++)
								{
									float num3 = num2 + ((Main.rand.Next(2) == 1) ? -1f : 1f) * 1.5707964f;
									float num4 = (float)Main.rand.NextDouble() * 2f + 2f;
									Vector2 vector2 = new Vector2((float)Math.Cos((double)num3) * num4, (float)Math.Sin((double)num3) * num4);
									int num5 = Dust.NewDust(vector, 0, 0, 230, vector2.X, vector2.Y, 0, default(Color), 1f);
									Main.dust[num5].noGravity = true;
									Main.dust[num5].customData = mountedPlayer;
								}
								if (flag)
								{
									Tile.SmoothSlope((int)point.X, (int)point.Y, true, true);
								}
								drillBeam.cooldown = Mount.drillPickTime;
								drillBeam.lastPurpose = 0;
								break;
							}
						}
					}
					num++;
				}
			}
			if (mountedPlayer.controlUseTile)
			{
				int num6 = 0;
				while (num6 < Mount.amountOfBeamsAtOnce && drillMountData.beamCooldown == 0)
				{
					for (int k = 0; k < drillMountData.beams.Length; k++)
					{
						Mount.DrillBeam drillBeam2 = drillMountData.beams[k];
						if (drillBeam2.cooldown == 0)
						{
							Point16 point2 = this.DrillSmartCursor_Walls(mountedPlayer, drillMountData);
							if (!(point2 == Point16.NegativeOne))
							{
								drillBeam2.curTileTarget = point2;
								int damage = Mount.drillPickPower;
								if (flag)
								{
									bool flag3 = true;
									if (mountedPlayer.noBuilding)
									{
										flag3 = false;
									}
									if (flag3)
									{
										mountedPlayer.PickWall((int)point2.X, (int)point2.Y, damage);
									}
								}
								Vector2 vector3 = new Vector2((float)(point2.X << 4) + 8f, (float)(point2.Y << 4) + 8f);
								float num7 = (vector3 - mountedPlayer.Center).ToRotation();
								for (int l = 0; l < 2; l++)
								{
									float num8 = num7 + ((Main.rand.Next(2) == 1) ? -1f : 1f) * 1.5707964f;
									float num9 = (float)Main.rand.NextDouble() * 2f + 2f;
									Vector2 vector4 = new Vector2((float)Math.Cos((double)num8) * num9, (float)Math.Sin((double)num8) * num9);
									int num10 = Dust.NewDust(vector3, 0, 0, 230, vector4.X, vector4.Y, 0, default(Color), 1f);
									Main.dust[num10].noGravity = true;
									Main.dust[num10].customData = mountedPlayer;
								}
								drillBeam2.cooldown = Mount.drillPickTime;
								drillBeam2.lastPurpose = 1;
								break;
							}
						}
					}
					num6++;
				}
			}
		}

		// Token: 0x06000277 RID: 631 RVA: 0x00034218 File Offset: 0x00032418
		private Point16 DrillSmartCursor_Blocks(Player mountedPlayer, Mount.DrillMountData data)
		{
			Vector2 value;
			if (mountedPlayer.whoAmI == Main.myPlayer)
			{
				value = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
			}
			else
			{
				value = data.crosshairPosition;
			}
			Vector2 center = mountedPlayer.Center;
			Vector2 value2 = value - center;
			float num = value2.Length();
			if (num > 224f)
			{
				num = 224f;
			}
			num += 32f;
			value2.Normalize();
			Vector2 start = center;
			Vector2 end = center + value2 * num;
			Point16 tilePoint = new Point16(-1, -1);
			if (!Utils.PlotTileLine(start, end, 65.6f, delegate(int x, int y)
			{
				tilePoint = new Point16(x, y);
				for (int i = 0; i < data.beams.Length; i++)
				{
					if (data.beams[i].curTileTarget == tilePoint && data.beams[i].lastPurpose == 0)
					{
						return true;
					}
				}
				return !WorldGen.CanKillTile(x, y) || Main.tile[x, y] == null || Main.tile[x, y].inActive() || !Main.tile[x, y].active();
			}))
			{
				return tilePoint;
			}
			return new Point16(-1, -1);
		}

		// Token: 0x06000278 RID: 632 RVA: 0x000342EC File Offset: 0x000324EC
		private Point16 DrillSmartCursor_Walls(Player mountedPlayer, Mount.DrillMountData data)
		{
			Vector2 value;
			if (mountedPlayer.whoAmI == Main.myPlayer)
			{
				value = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
			}
			else
			{
				value = data.crosshairPosition;
			}
			Vector2 center = mountedPlayer.Center;
			Vector2 value2 = value - center;
			float num = value2.Length();
			if (num > 224f)
			{
				num = 224f;
			}
			num += 32f;
			num += 16f;
			value2.Normalize();
			Vector2 start = center;
			Vector2 end = center + value2 * num;
			Point16 tilePoint = new Point16(-1, -1);
			if (!Utils.PlotTileLine(start, end, 97.6f, delegate(int x, int y)
			{
				tilePoint = new Point16(x, y);
				for (int i = 0; i < data.beams.Length; i++)
				{
					if (data.beams[i].curTileTarget == tilePoint && data.beams[i].lastPurpose == 1)
					{
						return true;
					}
				}
				Tile tile = Main.tile[x, y];
				return tile != null && (tile.wall <= 0 || !Player.CanPlayerSmashWall(x, y));
			}))
			{
				return tilePoint;
			}
			return new Point16(-1, -1);
		}

		// Token: 0x06000279 RID: 633 RVA: 0x000343CC File Offset: 0x000325CC
		public void UseAbility(Player mountedPlayer, Vector2 mousePosition, bool toggleOn)
		{
			int type = this._type;
			if (type != 8)
			{
				if (type != 9)
				{
					if (type != 46)
					{
						return;
					}
					if (Main.myPlayer == mountedPlayer.whoAmI)
					{
						if (this._abilityCooldown <= 10)
						{
							int damage = 120;
							Vector2 vector = mountedPlayer.Center + new Vector2((float)(mountedPlayer.width * -(float)mountedPlayer.direction), 26f);
							Vector2 vector2 = new Vector2(0f, -4f).RotatedByRandom(0.10000000149011612);
							Projectile.NewProjectile(this.GetProjectileSpawnSource(mountedPlayer), vector.X, vector.Y, vector2.X, vector2.Y, 930, damage, 0f, Main.myPlayer, 0f, 0f, 0f, null);
							SoundEngine.PlaySound(SoundID.Item89.SoundId, (int)vector.X, (int)vector.Y, SoundID.Item89.Style, 0.2f, 0f);
						}
						int type2 = 14;
						int damage2 = 100;
						mousePosition = this.ClampToDeadZone(mountedPlayer, mousePosition);
						Vector2 vector3;
						vector3.X = mountedPlayer.position.X + (float)(mountedPlayer.width / 2);
						vector3.Y = mountedPlayer.position.Y + (float)mountedPlayer.height;
						Vector2 vector4 = new Vector2(vector3.X + (float)(mountedPlayer.width * mountedPlayer.direction), vector3.Y - 12f);
						Vector2 vector5 = mousePosition - vector4;
						vector5 = vector5.SafeNormalize(Vector2.Zero);
						vector5 *= 12f;
						vector5 = vector5.RotatedByRandom(0.20000000298023224);
						Projectile.NewProjectile(this.GetProjectileSpawnSource(mountedPlayer), vector4.X, vector4.Y, vector5.X, vector5.Y, type2, damage2, 0f, Main.myPlayer, 0f, 0f, 0f, null);
						SoundEngine.PlaySound(SoundID.Item11.SoundId, (int)vector4.X, (int)vector4.Y, SoundID.Item11.Style, 0.2f, 0f);
						return;
					}
				}
				else if (Main.myPlayer == mountedPlayer.whoAmI)
				{
					int type3 = 606;
					mousePosition = this.ClampToDeadZone(mountedPlayer, mousePosition);
					Vector2 vector6;
					vector6.X = mountedPlayer.position.X + (float)(mountedPlayer.width / 2);
					vector6.Y = mountedPlayer.position.Y + (float)mountedPlayer.height;
					int num = (this._frameExtra - 6) * 2;
					for (int i = 0; i < 2; i++)
					{
						Vector2 vector7;
						vector7.Y = vector6.Y + Mount.scutlixEyePositions[num + i].Y + (float)this._data.yOffset;
						if (mountedPlayer.direction == -1)
						{
							vector7.X = vector6.X - Mount.scutlixEyePositions[num + i].X - (float)this._data.xOffset;
						}
						else
						{
							vector7.X = vector6.X + Mount.scutlixEyePositions[num + i].X + (float)this._data.xOffset;
						}
						Vector2 vector8 = mousePosition - vector7;
						vector8.Normalize();
						vector8 *= 14f;
						int damage3 = 150;
						vector7 += vector8;
						Projectile.NewProjectile(this.GetProjectileSpawnSource(mountedPlayer), vector7.X, vector7.Y, vector8.X, vector8.Y, type3, damage3, 0f, Main.myPlayer, 0f, 0f, 0f, null);
					}
					return;
				}
			}
			else
			{
				if (Main.myPlayer != mountedPlayer.whoAmI)
				{
					this._abilityActive = toggleOn;
					return;
				}
				if (!toggleOn)
				{
					this._abilityActive = false;
					return;
				}
				if (!this._abilityActive)
				{
					if (mountedPlayer.whoAmI == Main.myPlayer)
					{
						float num2 = Main.screenPosition.X + (float)Main.mouseX;
						float num3 = Main.screenPosition.Y + (float)Main.mouseY;
						float ai = num2 - mountedPlayer.position.X;
						float ai2 = num3 - mountedPlayer.position.Y;
						Projectile.NewProjectile(this.GetProjectileSpawnSource(mountedPlayer), num2, num3, 0f, 0f, 453, 0, 0f, mountedPlayer.whoAmI, ai, ai2, 0f, null);
					}
					this._abilityActive = true;
					return;
				}
			}
		}

		// Token: 0x0600027A RID: 634 RVA: 0x00034844 File Offset: 0x00032A44
		public bool Hover(Player mountedPlayer)
		{
			bool flag = this.DoesHoverIgnoresFatigue();
			bool flag2 = this._frameState == 2 || this._frameState == 4;
			if (this._type == 49)
			{
				flag2 = (this._frameState == 4);
			}
			if (this._type == 56)
			{
				flag2 = (this._frameState == 2 || this._frameState == 3);
			}
			if (this._type == 61)
			{
				flag2 = (this._frameState == 2 || this._frameState == 3 || this._frameState == 4);
			}
			if (flag2)
			{
				bool flag3 = true;
				float num = 1f;
				float num2 = mountedPlayer.gravity / Player.defaultGravity;
				if (mountedPlayer.slowFall)
				{
					num2 /= 3f;
				}
				if (num2 < 0.25f)
				{
					num2 = 0.25f;
				}
				if (!flag)
				{
					if (this._flyTime > 0)
					{
						this._flyTime--;
					}
					else if (this._fatigue < this._fatigueMax)
					{
						this._fatigue += num2;
					}
					else
					{
						flag3 = false;
					}
				}
				if (this._type == 12 && !mountedPlayer.MountFishronSpecial)
				{
					num = 0.5f;
				}
				float num3 = this._fatigue / this._fatigueMax;
				if (flag)
				{
					num3 = 0f;
				}
				bool flag4 = true;
				if (this._type == 48)
				{
					flag4 = false;
				}
				float num4 = 4f * num3;
				float num5 = 4f * num3;
				bool flag5 = false;
				if (this._type == 48)
				{
					num4 = 0f;
					num5 = 0f;
					if (!flag3)
					{
						flag5 = true;
					}
					if (mountedPlayer.controlDown)
					{
						num5 = 8f;
					}
				}
				if (num4 == 0f)
				{
					num4 = -0.001f;
				}
				if (num5 == 0f)
				{
					num5 = -0.001f;
				}
				float num6 = mountedPlayer.velocity.Y;
				if (flag4 && (mountedPlayer.controlUp || mountedPlayer.controlJump) && flag3)
				{
					num4 = -2f - 6f * (1f - num3);
					if (this._type == 48)
					{
						num4 /= 3f;
					}
					if (this._type == 56 || this._type == 61)
					{
						num4 = -this._data.dashSpeed;
					}
					num6 -= this._data.acceleration * num;
				}
				else if (mountedPlayer.controlDown)
				{
					num5 = 8f;
					if (this._type == 56 || this._type == 61)
					{
						num5 = this._data.dashSpeed;
					}
					num6 += this._data.acceleration * num;
				}
				else if (flag5)
				{
					float num7 = mountedPlayer.gravity * mountedPlayer.gravDir;
					num6 += num7;
					num5 = 4f;
				}
				else
				{
					int jump = mountedPlayer.jump;
				}
				if (num6 < num4)
				{
					if (num4 - num6 < this._data.acceleration)
					{
						num6 = num4;
					}
					else
					{
						num6 += this._data.acceleration * num;
					}
				}
				else if (num6 > num5)
				{
					if (num6 - num5 < this._data.acceleration)
					{
						num6 = num5;
					}
					else
					{
						num6 -= this._data.acceleration * num;
					}
				}
				if (this._type == 56 || this._type == 61)
				{
					if (num4 != -0.001f)
					{
						num6 = MathHelper.Max(num6, num4);
					}
					if (num5 != -0.001f)
					{
						num6 = MathHelper.Min(num6, num5);
					}
				}
				mountedPlayer.velocity.Y = num6;
				if (num4 == -0.001f && num5 == -0.001f && num6 == -0.001f)
				{
					mountedPlayer.position.Y = mountedPlayer.position.Y - -0.001f;
					Mount.TryStabilizingSmallMountPositionBetweenSlopes(mountedPlayer);
				}
				mountedPlayer.fallStart = (int)(mountedPlayer.position.Y / 16f);
			}
			else if (!flag)
			{
				mountedPlayer.velocity.Y = mountedPlayer.velocity.Y + mountedPlayer.gravity * mountedPlayer.gravDir;
			}
			else if (mountedPlayer.velocity.Y == 0f)
			{
				Vector2 velocity = Vector2.UnitY * mountedPlayer.gravDir * 1f;
				if (Collision.TileCollision(mountedPlayer.position, velocity, mountedPlayer.width, mountedPlayer.height, false, false, (int)mountedPlayer.gravDir, false, false, true).Y != 0f || mountedPlayer.controlDown)
				{
					mountedPlayer.velocity.Y = 0.001f;
				}
			}
			else if (mountedPlayer.velocity.Y == -0.001f)
			{
				mountedPlayer.velocity.Y = mountedPlayer.velocity.Y - -0.001f;
			}
			if (this._type == 7)
			{
				float num8 = mountedPlayer.velocity.X / this._data.dashSpeed;
				if ((double)num8 > 0.95)
				{
					num8 = 0.95f;
				}
				if ((double)num8 < -0.95)
				{
					num8 = -0.95f;
				}
				float fullRotation = 0.7853982f * num8 / 2f;
				float num9 = Math.Abs(2f - (float)this._frame / 2f) / 2f;
				Lighting.AddLight((int)(mountedPlayer.position.X + (float)(mountedPlayer.width / 2)) / 16, (int)(mountedPlayer.position.Y + (float)(mountedPlayer.height / 2)) / 16, 0.4f, 0.2f * num9, 0f);
				mountedPlayer.fullRotation = fullRotation;
			}
			else if (this._type == 8)
			{
				float num10 = mountedPlayer.velocity.X / this._data.dashSpeed;
				if ((double)num10 > 0.95)
				{
					num10 = 0.95f;
				}
				if ((double)num10 < -0.95)
				{
					num10 = -0.95f;
				}
				float fullRotation2 = 0.7853982f * num10 / 2f;
				mountedPlayer.fullRotation = fullRotation2;
				Mount.DrillMountData drillMountData = (Mount.DrillMountData)this._mountSpecificData;
				float num11 = drillMountData.outerRingRotation;
				num11 += mountedPlayer.velocity.X / 80f;
				if (num11 > 3.1415927f)
				{
					num11 -= 6.2831855f;
				}
				else if (num11 < -3.1415927f)
				{
					num11 += 6.2831855f;
				}
				drillMountData.outerRingRotation = num11;
			}
			else if (this._type == 23)
			{
				float num12 = -mountedPlayer.velocity.Y / this._data.dashSpeed;
				num12 = MathHelper.Clamp(num12, -1f, 1f);
				float num13 = mountedPlayer.velocity.X / this._data.dashSpeed;
				num13 = MathHelper.Clamp(num13, -1f, 1f);
				float num14 = -0.19634955f * num12 * (float)mountedPlayer.direction;
				float num15 = 0.19634955f * num13;
				float fullRotation3 = num14 + num15;
				mountedPlayer.fullRotation = fullRotation3;
				mountedPlayer.fullRotationOrigin = new Vector2((float)(mountedPlayer.width / 2), (float)mountedPlayer.height);
			}
			return true;
		}

		// Token: 0x0600027B RID: 635 RVA: 0x00034EE4 File Offset: 0x000330E4
		private static void TryStabilizingSmallMountPositionBetweenSlopes(Player mountedPlayer)
		{
			if (mountedPlayer.height >= 42)
			{
				return;
			}
			Vector4 vec = Collision.SlopeCollision(mountedPlayer.position, mountedPlayer.velocity, mountedPlayer.width, mountedPlayer.height, 0f, false, false);
			mountedPlayer.position = vec.XY();
		}

		// Token: 0x0600027C RID: 636 RVA: 0x00034F30 File Offset: 0x00033130
		private bool DoesHoverIgnoresFatigue()
		{
			return this._type == 7 || this._type == 8 || this._type == 12 || this._type == 23 || this._type == 44 || this._type == 49 || this._type == 56 || this._type == 61;
		}

		// Token: 0x0600027D RID: 637 RVA: 0x00034F90 File Offset: 0x00033190
		private float GetWitchBroomTrinketRotation(Player player)
		{
			float num = Utils.Clamp<float>(player.velocity.X / 10f, -1f, 1f);
			Point point = player.Center.ToTileCoordinates();
			float num2 = 0.5f;
			if (WorldGen.InAPlaceWithWind(point.X, point.Y, 1, 1))
			{
				num2 = 1f;
			}
			float num3 = (float)Math.Sin((double)((float)player.miscCounter / 300f * 6.2831855f * 3f)) * 0.7853982f * Math.Abs(Main.WindForVisuals) * 0.5f + 0.7853982f * -Main.WindForVisuals * 0.5f;
			num3 *= num2;
			return num * (float)Math.Sin((double)((float)player.miscCounter / 150f * 6.2831855f * 3f)) * 0.7853982f * 0.5f + num * 0.7853982f * 0.5f + num3;
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0003507E File Offset: 0x0003327E
		private Vector2 GetWitchBroomTrinketOriginOffset(Player player)
		{
			return new Vector2((float)(29 * player.direction), -4f);
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00035094 File Offset: 0x00033294
		public void UpdateFrame(Player mountedPlayer, int state, Vector2 velocity)
		{
			if (this._frameState != state)
			{
				if (this._type == 56 && ((this._frameState == 5 && (mountedPlayer.dash <= 0 || mountedPlayer.dashDelay >= 0)) || (this._frameState != 5 && mountedPlayer.dash > 0 && mountedPlayer.dashDelay < 0)))
				{
					this._frameCounter = 0f;
				}
				this._frameState = state;
				bool flag = true;
				if (this._type == 48 && (state == 1 || state == 2))
				{
					flag = false;
				}
				if (this._type == 56 && (state == 1 || state == 4 || state == 2))
				{
					flag = false;
				}
				if (this._type == 61 && (state == 1 || state == 4 || state == 2))
				{
					flag = false;
				}
				if (this._type == 55 && (state == 1 || state == 5 || state == 4 || (state == 2 && mountedPlayer.sliding)))
				{
					flag = false;
				}
				if (flag)
				{
					this._frameCounter = 0f;
				}
			}
			if (state != 0)
			{
				this._idleTime = 0;
			}
			if (mountedPlayer.isDisplayDollOrInanimate)
			{
				this._idleTime = 0;
			}
			if (velocity.Y == 0f)
			{
				this._walkingGraceTimeLeft = this._data.walkingGraceTimeMax;
			}
			else if (this._walkingGraceTimeLeft > 0)
			{
				this._walkingGraceTimeLeft--;
			}
			if (mountedPlayer.justJumped || (mountedPlayer.controlDown && mountedPlayer.velocity.Y > 0f))
			{
				this._walkingGraceTimeLeft = 0;
			}
			if (this._data.emitsLight)
			{
				Point point = mountedPlayer.Center.ToTileCoordinates();
				Lighting.AddLight(point.X, point.Y, this._data.lightColor.X, this._data.lightColor.Y, this._data.lightColor.Z);
			}
			int type = this._type;
			switch (type)
			{
			case 5:
				if (state != 2)
				{
					this._frameExtra = 0;
					this._frameExtraCounter = 0f;
					goto IL_1665;
				}
				goto IL_1665;
			case 6:
			case 11:
			case 12:
			case 13:
			case 15:
			case 16:
				goto IL_1665;
			case 7:
				state = 2;
				goto IL_1665;
			case 8:
			{
				if (state != 0 && state != 1)
				{
					goto IL_1665;
				}
				Vector2 vector;
				vector.X = mountedPlayer.position.X;
				vector.Y = mountedPlayer.position.Y + (float)mountedPlayer.height;
				int num = (int)(vector.X / 16f);
				float num2 = vector.Y / 16f;
				float num3 = 0f;
				float num4 = (float)mountedPlayer.width;
				while (num4 > 0f)
				{
					float num5 = (float)((num + 1) * 16) - vector.X;
					if (num5 > num4)
					{
						num5 = num4;
					}
					num3 += Collision.GetTileRotation(vector) * num5;
					num4 -= num5;
					vector.X += num5;
					num++;
				}
				float num6 = num3 / (float)mountedPlayer.width - mountedPlayer.fullRotation;
				float num7 = 0f;
				float num8 = 0.15707964f;
				if (num6 < 0f)
				{
					if (num6 > -num8)
					{
						num7 = num6;
					}
					else
					{
						num7 = -num8;
					}
				}
				else if (num6 > 0f)
				{
					if (num6 < num8)
					{
						num7 = num6;
					}
					else
					{
						num7 = num8;
					}
				}
				if (num7 == 0f)
				{
					goto IL_1665;
				}
				mountedPlayer.fullRotation += num7;
				if (mountedPlayer.fullRotation > 0.7853982f)
				{
					mountedPlayer.fullRotation = 0.7853982f;
				}
				if (mountedPlayer.fullRotation < -0.7853982f)
				{
					mountedPlayer.fullRotation = -0.7853982f;
					goto IL_1665;
				}
				goto IL_1665;
			}
			case 9:
				if (this._aiming)
				{
					goto IL_1665;
				}
				this._frameExtraCounter += 1f;
				if (this._frameExtraCounter < 12f)
				{
					goto IL_1665;
				}
				this._frameExtraCounter = 0f;
				this._frameExtra++;
				if (this._frameExtra >= 6)
				{
					this._frameExtra = 0;
					goto IL_1665;
				}
				goto IL_1665;
			case 10:
				break;
			case 14:
			{
				bool flag2 = Math.Abs(velocity.X) > this.RunSpeed / 2f;
				float num9 = (float)Math.Sign(mountedPlayer.velocity.X);
				float num10 = 12f;
				float num11 = 40f;
				if (!flag2)
				{
					mountedPlayer.basiliskCharge = 0f;
				}
				else
				{
					mountedPlayer.basiliskCharge = Utils.Clamp<float>(mountedPlayer.basiliskCharge + 0.0055555557f, 0f, 1f);
				}
				if ((double)mountedPlayer.position.Y > Main.worldSurface * 16.0 + 160.0)
				{
					Lighting.AddLight(mountedPlayer.Center, 0.5f, 0.1f, 0.1f);
				}
				if (flag2 && velocity.Y == 0f)
				{
					for (int i = 0; i < 2; i++)
					{
						Dust dust = Main.dust[Dust.NewDust(mountedPlayer.BottomLeft, mountedPlayer.width, 6, 31, 0f, 0f, 0, default(Color), 1f)];
						dust.velocity = new Vector2(velocity.X * 0.15f, Main.rand.NextFloat() * -2f);
						dust.noLight = true;
						dust.scale = 0.5f + Main.rand.NextFloat() * 0.8f;
						dust.fadeIn = 0.5f + Main.rand.NextFloat() * 1f;
						dust.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMount, mountedPlayer);
					}
					if (mountedPlayer.cMount == 0)
					{
						mountedPlayer.position += new Vector2(num9 * 24f, 0f);
						mountedPlayer.FloorVisuals(true);
						mountedPlayer.position -= new Vector2(num9 * 24f, 0f);
					}
				}
				if (num9 == (float)mountedPlayer.direction)
				{
					for (int j = 0; j < (int)(3f * mountedPlayer.basiliskCharge); j++)
					{
						Dust dust2 = Main.dust[Dust.NewDust(mountedPlayer.BottomLeft, mountedPlayer.width, 6, 6, 0f, 0f, 0, default(Color), 1f)];
						Vector2 value = mountedPlayer.Center + new Vector2(num9 * num11, num10);
						dust2.position = mountedPlayer.Center + new Vector2(num9 * (num11 - 2f), num10 - 6f + Main.rand.NextFloat() * 12f);
						dust2.velocity = (dust2.position - value).SafeNormalize(Vector2.Zero) * (3.5f + Main.rand.NextFloat() * 0.5f);
						if (dust2.velocity.Y < 0f)
						{
							Dust dust3 = dust2;
							dust3.velocity.Y = dust3.velocity.Y * (1f + 2f * Main.rand.NextFloat());
						}
						dust2.velocity += mountedPlayer.velocity * 0.55f;
						dust2.velocity *= mountedPlayer.velocity.Length() / this.RunSpeed;
						dust2.velocity *= mountedPlayer.basiliskCharge;
						dust2.noGravity = true;
						dust2.noLight = true;
						dust2.scale = 0.5f + Main.rand.NextFloat() * 0.8f;
						dust2.fadeIn = 0.5f + Main.rand.NextFloat() * 1f;
						dust2.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMount, mountedPlayer);
					}
					goto IL_1665;
				}
				goto IL_1665;
			}
			case 17:
				this.UpdateFrame_GolfCart(mountedPlayer, state, velocity);
				goto IL_1665;
			default:
				switch (type)
				{
				case 39:
					this._frameExtraCounter += 1f;
					if (this._frameExtraCounter <= 6f)
					{
						goto IL_1665;
					}
					this._frameExtraCounter = 0f;
					this._frameExtra++;
					if (this._frameExtra > 5)
					{
						this._frameExtra = 0;
						goto IL_1665;
					}
					goto IL_1665;
				case 40:
				case 41:
				case 42:
				case 47:
					break;
				case 43:
					if (mountedPlayer.velocity.Y == 0f)
					{
						mountedPlayer.isPerformingPogostickTricks = false;
					}
					if (mountedPlayer.isPerformingPogostickTricks)
					{
						mountedPlayer.fullRotation += (float)mountedPlayer.direction * 6.2831855f / 30f;
					}
					else
					{
						mountedPlayer.fullRotation = (float)Math.Sign(mountedPlayer.velocity.X) * Utils.GetLerpValue(0f, this.RunSpeed - 0.2f, Math.Abs(mountedPlayer.velocity.X), true) * 0.4f;
					}
					mountedPlayer.fullRotationOrigin = new Vector2((float)(mountedPlayer.width / 2), (float)mountedPlayer.height * 0.8f);
					goto IL_1665;
				case 44:
				{
					state = 1;
					bool flag3 = Math.Abs(velocity.X) > this.DashSpeed - this.RunSpeed / 4f;
					if (this._mountSpecificData == null)
					{
						this._mountSpecificData = false;
					}
					bool flag4 = (bool)this._mountSpecificData;
					if (flag4 && !flag3)
					{
						this._mountSpecificData = false;
					}
					else if (!flag4 && flag3)
					{
						this._mountSpecificData = true;
						Vector2 vector2 = mountedPlayer.Center + new Vector2((float)(mountedPlayer.width * mountedPlayer.direction), 0f);
						Vector2 value2 = new Vector2(40f, 30f);
						float num12 = 6.2831855f * Main.rand.NextFloat();
						for (float num13 = 0f; num13 < 20f; num13 += 1f)
						{
							Dust dust4 = Main.dust[Dust.NewDust(vector2, 0, 0, 228, 0f, 0f, 0, default(Color), 1f)];
							Vector2 vector3 = Vector2.UnitY.RotatedBy((double)(num13 * 6.2831855f / 20f + num12), default(Vector2));
							vector3 *= 0.8f;
							dust4.position = vector2 + vector3 * value2;
							dust4.velocity = vector3 + new Vector2(this.RunSpeed - (float)Math.Sign(velocity.Length()), 0f);
							if (velocity.X > 0f)
							{
								Dust dust5 = dust4;
								dust5.velocity.X = dust5.velocity.X * -1f;
							}
							if (Main.rand.Next(2) == 0)
							{
								dust4.velocity *= 0.5f;
							}
							dust4.noGravity = true;
							dust4.scale = 1.5f + Main.rand.NextFloat() * 0.8f;
							dust4.fadeIn = 0f;
							dust4.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMount, mountedPlayer);
						}
					}
					int num14 = (int)this.RunSpeed - (int)Math.Abs(velocity.X);
					if (num14 <= 0)
					{
						num14 = 1;
					}
					if (Main.rand.Next(num14) == 0)
					{
						int num15 = 22;
						int num16 = mountedPlayer.width / 2 + 10;
						Vector2 bottom = mountedPlayer.Bottom;
						bottom.X -= (float)num16;
						bottom.Y -= (float)(num15 - 6);
						Dust dust6 = Main.dust[Dust.NewDust(bottom, num16 * 2, num15, 228, 0f, 0f, 0, default(Color), 1f)];
						dust6.velocity = Vector2.Zero;
						dust6.noGravity = true;
						dust6.noLight = true;
						dust6.scale = 0.25f + Main.rand.NextFloat() * 0.8f;
						dust6.fadeIn = 0.5f + Main.rand.NextFloat() * 2f;
						dust6.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMount, mountedPlayer);
						goto IL_1665;
					}
					goto IL_1665;
				}
				case 45:
				{
					bool flag5 = Math.Abs(velocity.X) > this.DashSpeed * 0.9f;
					if (this._mountSpecificData == null)
					{
						this._mountSpecificData = false;
					}
					bool flag6 = (bool)this._mountSpecificData;
					if (flag6 && !flag5)
					{
						this._mountSpecificData = false;
					}
					else if (!flag6 && flag5)
					{
						this._mountSpecificData = true;
						Vector2 vector4 = mountedPlayer.Center + new Vector2((float)(mountedPlayer.width * mountedPlayer.direction), 0f);
						Vector2 value3 = new Vector2(40f, 30f);
						float num17 = 6.2831855f * Main.rand.NextFloat();
						for (float num18 = 0f; num18 < 20f; num18 += 1f)
						{
							Dust dust7 = Main.dust[Dust.NewDust(vector4, 0, 0, 6, 0f, 0f, 0, default(Color), 1f)];
							Vector2 vector5 = Vector2.UnitY.RotatedBy((double)(num18 * 6.2831855f / 20f + num17), default(Vector2));
							vector5 *= 0.8f;
							dust7.position = vector4 + vector5 * value3;
							dust7.velocity = vector5 + new Vector2(this.RunSpeed - (float)Math.Sign(velocity.Length()), 0f);
							if (velocity.X > 0f)
							{
								Dust dust8 = dust7;
								dust8.velocity.X = dust8.velocity.X * -1f;
							}
							if (Main.rand.Next(2) == 0)
							{
								dust7.velocity *= 0.5f;
							}
							dust7.noGravity = true;
							dust7.scale = 1.5f + Main.rand.NextFloat() * 0.8f;
							dust7.fadeIn = 0f;
							dust7.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMount, mountedPlayer);
						}
					}
					if (flag5)
					{
						int num19 = (int)Utils.SelectRandom<short>(Main.rand, new short[]
						{
							6,
							6,
							31
						});
						int num20 = 6;
						Dust dust9 = Main.dust[Dust.NewDust(mountedPlayer.Center - new Vector2((float)num20, (float)(num20 - 12)), num20 * 2, num20 * 2, num19, 0f, 0f, 0, default(Color), 1f)];
						dust9.velocity = mountedPlayer.velocity * 0.1f;
						if (Main.rand.Next(2) == 0)
						{
							dust9.noGravity = true;
						}
						dust9.scale = 0.7f + Main.rand.NextFloat() * 0.8f;
						if (Main.rand.Next(3) == 0)
						{
							dust9.fadeIn = 0.1f;
						}
						if (num19 == 31)
						{
							dust9.noGravity = true;
							dust9.scale *= 1.5f;
							dust9.fadeIn = 0.2f;
						}
						dust9.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMount, mountedPlayer);
						goto IL_1665;
					}
					goto IL_1665;
				}
				case 46:
					if (state != 0)
					{
						state = 1;
					}
					if (!this._aiming)
					{
						if (state == 0)
						{
							this._frameExtra = 12;
							this._frameExtraCounter = 0f;
							goto IL_1665;
						}
						if (this._frameExtra < 12)
						{
							this._frameExtra = 12;
						}
						this._frameExtraCounter += Math.Abs(velocity.X);
						if (this._frameExtraCounter < 8f)
						{
							goto IL_1665;
						}
						this._frameExtraCounter = 0f;
						this._frameExtra++;
						if (this._frameExtra >= 24)
						{
							this._frameExtra = 12;
							goto IL_1665;
						}
						goto IL_1665;
					}
					else
					{
						if (this._frameExtra < 24)
						{
							this._frameExtra = 24;
						}
						this._frameExtraCounter += 1f;
						if (this._frameExtraCounter < 3f)
						{
							goto IL_1665;
						}
						this._frameExtraCounter = 0f;
						this._frameExtra++;
						if (this._frameExtra >= 27)
						{
							this._frameExtra = 24;
							goto IL_1665;
						}
						goto IL_1665;
					}
					break;
				case 48:
					state = 1;
					goto IL_1665;
				case 49:
				{
					if (state != 4 && mountedPlayer.wet)
					{
						state = (this._frameState = 4);
					}
					velocity.Length();
					float num21 = mountedPlayer.velocity.Y * 0.05f;
					if (mountedPlayer.direction < 0)
					{
						num21 *= -1f;
					}
					mountedPlayer.fullRotation = num21;
					mountedPlayer.fullRotationOrigin = new Vector2((float)(mountedPlayer.width / 2), (float)(mountedPlayer.height / 2));
					goto IL_1665;
				}
				case 50:
					if (mountedPlayer.velocity.Y == 0f)
					{
						this._frameExtraCounter = 0f;
						this._frameExtra = 3;
						goto IL_1665;
					}
					this._frameExtraCounter += 1f;
					if (this._flyTime > 0)
					{
						this._frameExtraCounter += 1f;
					}
					if (this._frameExtraCounter <= 7f)
					{
						goto IL_1665;
					}
					this._frameExtraCounter = 0f;
					this._frameExtra++;
					if (this._frameExtra > 3)
					{
						this._frameExtra = 0;
						goto IL_1665;
					}
					goto IL_1665;
				case 51:
				case 53:
				case 57:
				case 58:
				case 59:
				case 60:
					goto IL_1665;
				case 52:
					if (state == 4)
					{
						state = 2;
						goto IL_1665;
					}
					goto IL_1665;
				case 54:
					this.UpdateFrame_Velociraptor(mountedPlayer, ref state);
					goto IL_1665;
				case 55:
					if (mountedPlayer.sliding)
					{
						state = (this._frameState = 1);
						goto IL_1665;
					}
					if (state == 4)
					{
						state = (this._frameState = 2);
						goto IL_1665;
					}
					if ((state == 1 || state == 5) && Math.Abs(mountedPlayer.velocity.X) > 4f)
					{
						state = (this._frameState = 5);
						goto IL_1665;
					}
					goto IL_1665;
				case 56:
					if (mountedPlayer.dash > 0 && mountedPlayer.dashDelay < 0)
					{
						state = (this._frameState = 5);
						goto IL_1665;
					}
					if (state != 0 && state != 1)
					{
						state = (this._frameState = 2);
						goto IL_1665;
					}
					goto IL_1665;
				case 61:
				{
					Point point2 = mountedPlayer.Center.ToTileCoordinates();
					Vector3 vector6 = Projectile.GetFairyQueenWeaponsColorFull(mountedPlayer.whoAmI, mountedPlayer.Center, 0.41f, 1f, 0.1f, 1f, 0.5f).ToVector3() * 0.55f;
					Lighting.AddLight(point2.X, point2.Y, vector6.X, vector6.Y, vector6.Z);
					if (this._frameState == 4)
					{
						state = (this._frameState = 2);
						goto IL_1665;
					}
					goto IL_1665;
				}
				default:
					goto IL_1665;
				}
				break;
			}
			bool flag7 = Math.Abs(velocity.X) > this.DashSpeed - this.RunSpeed / 2f;
			if (state == 1)
			{
				bool flag8 = false;
				if (flag7)
				{
					state = 5;
					if (this._frameExtra < 6)
					{
						flag8 = true;
					}
					this._frameExtra++;
				}
				else
				{
					this._frameExtra = 0;
				}
				if ((this._type == 10 || this._type == 47) && flag8)
				{
					int type2 = 6;
					if (this._type == 10)
					{
						type2 = Utils.SelectRandom<int>(Main.rand, new int[]
						{
							176,
							177,
							179
						});
					}
					Vector2 vector7 = mountedPlayer.Center + new Vector2((float)(mountedPlayer.width * mountedPlayer.direction), 0f);
					Vector2 value4 = new Vector2(40f, 30f);
					float num22 = 6.2831855f * Main.rand.NextFloat();
					for (float num23 = 0f; num23 < 14f; num23 += 1f)
					{
						Dust dust10 = Main.dust[Dust.NewDust(vector7, 0, 0, type2, 0f, 0f, 0, default(Color), 1f)];
						Vector2 vector8 = Vector2.UnitY.RotatedBy((double)(num23 * 6.2831855f / 14f + num22), default(Vector2));
						vector8 *= 0.2f * (float)this._frameExtra;
						dust10.position = vector7 + vector8 * value4;
						dust10.velocity = vector8 + new Vector2(this.RunSpeed - (float)(Math.Sign(velocity.X) * this._frameExtra * 2), 0f);
						dust10.noGravity = true;
						if (this._type == 47)
						{
							dust10.noLightEmittence = true;
						}
						dust10.scale = 1f + Main.rand.NextFloat() * 0.8f;
						dust10.fadeIn = Main.rand.NextFloat() * 2f;
						dust10.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMount, mountedPlayer);
					}
				}
			}
			if (this._type == 10 && flag7)
			{
				Dust dust11 = Main.dust[Dust.NewDust(mountedPlayer.position, mountedPlayer.width, mountedPlayer.height, Utils.SelectRandom<int>(Main.rand, new int[]
				{
					176,
					177,
					179
				}), 0f, 0f, 0, default(Color), 1f)];
				dust11.velocity = Vector2.Zero;
				dust11.noGravity = true;
				dust11.scale = 0.5f + Main.rand.NextFloat() * 0.8f;
				dust11.fadeIn = 1f + Main.rand.NextFloat() * 2f;
				dust11.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMount, mountedPlayer);
			}
			if (this._type == 47 && flag7 && velocity.Y == 0f)
			{
				int num24 = (int)mountedPlayer.Center.X / 16;
				int num25 = (int)(mountedPlayer.position.Y + (float)mountedPlayer.height - 1f) / 16;
				Tile tile = Main.tile[num24, num25 + 1];
				if (tile != null && tile.active() && tile.liquid == 0 && WorldGen.SolidTileAllowBottomSlope(num24, num25 + 1))
				{
					ParticleOrchestrator.RequestParticleSpawn(true, ParticleOrchestraType.WallOfFleshGoatMountFlames, new ParticleOrchestraSettings
					{
						PositionInWorld = new Vector2((float)(num24 * 16 + 8), (float)(num25 * 16 + 16))
					}, new int?(mountedPlayer.whoAmI));
				}
			}
			IL_1665:
			switch (state)
			{
			case 0:
				if (this._data.idleFrameCount != 0)
				{
					if (this._type == 5)
					{
						if (this._fatigue != 0f)
						{
							if (this._idleTime == 0)
							{
								this._idleTimeNext = this._idleTime + 1;
							}
						}
						else
						{
							this._idleTime = 0;
							this._idleTimeNext = 2;
						}
					}
					else if (this._idleTime == 0)
					{
						this._idleTimeNext = Main.rand.Next(900, 1500);
						if (this._type == 17)
						{
							this._idleTimeNext = Main.rand.Next(120, 300);
						}
						if (this._type == 2)
						{
							this._idleTimeNext = Main.rand.Next(600, 900);
						}
						if (this._type == 55)
						{
							this._idleTimeNext = Main.rand.Next(120, 420);
						}
					}
					this._idleTime++;
				}
				this._frameCounter += 1f;
				if (this._data.idleFrameCount != 0 && this._idleTime >= this._idleTimeNext)
				{
					float num26 = (float)this._data.idleFrameDelay;
					if (this._type == 5)
					{
						num26 *= 2f - 1f * this._fatigue / this._fatigueMax;
					}
					int idleFrameCount = this._data.idleFrameCount;
					if (this._type == 55)
					{
						int num27 = (int)(((float)this._idleTime - (float)this._idleTimeNext) / num26 / (float)idleFrameCount * (float)Mount.idleFrames_Rat.Length);
						if (num27 >= Mount.idleFrames_Rat.Length)
						{
							this._frameCounter = 0f;
							this._frame = this._data.standingFrameStart;
							this._idleTime = 0;
						}
						else
						{
							this._frame = this._data.idleFrameStart + Mount.idleFrames_Rat[num27];
						}
					}
					else
					{
						int num28 = (int)((float)(this._idleTime - this._idleTimeNext) / num26);
						if (num28 >= idleFrameCount)
						{
							if (this._data.idleFrameLoop)
							{
								this._idleTime = this._idleTimeNext;
								this._frame = this._data.idleFrameStart;
							}
							else
							{
								this._frameCounter = 0f;
								this._frame = this._data.standingFrameStart;
								this._idleTime = 0;
							}
						}
						else
						{
							this._frame = this._data.idleFrameStart + num28;
							if (this._data.idleFrameLoop)
							{
								if (this._frame < this._data.idleFrameStart || this._frame >= this._data.idleFrameStart + this._data.idleFrameCount)
								{
									this._frame = this._data.idleFrameStart;
								}
							}
							else if (this._frame < this._data.idleFrameStart || this._frame >= this._data.idleFrameStart + this._data.idleFrameCount)
							{
								this._frame = this._data.standingFrameStart;
							}
						}
						if (this._type == 5)
						{
							this._frameExtra = this._frame;
						}
						if (this._type == 17)
						{
							this._frameExtra = this._frame;
							this._frame = 0;
						}
					}
				}
				else
				{
					if (this._frameCounter > (float)this._data.standingFrameDelay)
					{
						this._frameCounter -= (float)this._data.standingFrameDelay;
						this._frame++;
					}
					if (this._frame < this._data.standingFrameStart || this._frame >= this._data.standingFrameStart + this._data.standingFrameCount)
					{
						this._frame = this._data.standingFrameStart;
					}
				}
				break;
			case 1:
			{
				type = this._type;
				float num29;
				if (type <= 50)
				{
					if (type != 9)
					{
						switch (type)
						{
						case 44:
							num29 = Math.Max(1f, Math.Abs(velocity.X) * 0.25f);
							goto IL_1B5D;
						case 45:
						case 47:
						case 49:
							goto IL_1B50;
						case 46:
							break;
						case 48:
							num29 = Math.Max(0.5f, velocity.Length() * 0.125f);
							goto IL_1B5D;
						case 50:
							num29 = Math.Abs(velocity.X) * 0.5f;
							goto IL_1B5D;
						default:
							goto IL_1B50;
						}
					}
					if (this._flipDraw)
					{
						num29 = -Math.Abs(velocity.X);
						goto IL_1B5D;
					}
					num29 = Math.Abs(velocity.X);
					goto IL_1B5D;
				}
				else if (type != 55)
				{
					if (type == 56)
					{
						num29 = MathHelper.Clamp(velocity.Length() * 0.5f, 1f, 2f);
						goto IL_1B5D;
					}
				}
				else
				{
					if (mountedPlayer.sliding)
					{
						num29 = velocity.Length();
						goto IL_1B5D;
					}
					num29 = Math.Abs(velocity.X);
					goto IL_1B5D;
				}
				IL_1B50:
				num29 = Math.Abs(velocity.X);
				IL_1B5D:
				this._frameCounter += num29;
				if (num29 >= 0f)
				{
					if (this._frameCounter > (float)this._data.runningFrameDelay)
					{
						this._frameCounter -= (float)this._data.runningFrameDelay;
						this._frame++;
					}
					if (this._frame < this._data.runningFrameStart || this._frame >= this._data.runningFrameStart + this._data.runningFrameCount)
					{
						this._frame = this._data.runningFrameStart;
					}
				}
				else
				{
					if (this._frameCounter < 0f)
					{
						this._frameCounter += (float)this._data.runningFrameDelay;
						this._frame--;
					}
					if (this._frame < this._data.runningFrameStart || this._frame >= this._data.runningFrameStart + this._data.runningFrameCount)
					{
						this._frame = this._data.runningFrameStart + this._data.runningFrameCount - 1;
					}
				}
				break;
			}
			case 2:
			{
				float num30 = 1f;
				if (this._type == 56 || this._type == 61)
				{
					num30 = MathHelper.Clamp(velocity.Length() * 0.5f, 1f, 2f);
				}
				int frame = this._frame;
				this._frameCounter += num30;
				if (this._frameCounter > (float)this._data.inAirFrameDelay)
				{
					this._frameCounter -= (float)this._data.inAirFrameDelay;
					this._frame++;
				}
				if (this._frame < this._data.inAirFrameStart || this._frame >= this._data.inAirFrameStart + this._data.inAirFrameCount)
				{
					this._frame = this._data.inAirFrameStart;
				}
				if (this._type == 62 || this._type == 63)
				{
					int num31 = this._data.inAirFrameDelay - 1;
					if (frame < 4)
					{
						this._frameCounter = (float)num31;
					}
					num31 = 6;
					if (this._frameCounter < (float)num31)
					{
						this._frame = 5;
					}
					else
					{
						this._frameCounter = (float)num31;
					}
				}
				if (this._type == 4)
				{
					if (velocity.Y < 0f)
					{
						this._frame = 3;
					}
					else
					{
						this._frame = 6;
					}
				}
				else if (this._type == 52 || this._type == 55)
				{
					if (velocity.Y < 0f)
					{
						this._frame = this._data.inAirFrameStart;
					}
					if (this._frame == this._data.inAirFrameStart + this._data.inAirFrameCount - 1)
					{
						this._frameCounter = 0f;
					}
				}
				else if (this._type == 54)
				{
					if (mountedPlayer.grappling[0] >= 0)
					{
						if (velocity.Length() > 0.01f)
						{
							if (this._frame == this._data.inAirFrameStart + this._data.inAirFrameCount - 1)
							{
								this._frameCounter = 0f;
							}
						}
						else
						{
							this._frame = 7;
						}
					}
					else if (velocity.Y < 0f)
					{
						if (this._frame == this._data.inAirFrameStart + this._data.inAirFrameCount - 1)
						{
							this._frameCounter = 0f;
						}
					}
					else
					{
						this._frame = 7;
					}
				}
				else if (this._type == 5)
				{
					float num32 = this._fatigue / this._fatigueMax;
					this._frameExtraCounter += 6f - 4f * num32;
					if (this._frameExtraCounter > (float)this._data.flyingFrameDelay)
					{
						this._frameExtra++;
						this._frameExtraCounter -= (float)this._data.flyingFrameDelay;
					}
					if (this._frameExtra < this._data.flyingFrameStart || this._frameExtra >= this._data.flyingFrameStart + this._data.flyingFrameCount)
					{
						this._frameExtra = this._data.flyingFrameStart;
					}
				}
				else if (this._type == 23)
				{
					float num33 = mountedPlayer.velocity.Length();
					if (num33 < 1f)
					{
						this._frame = 0;
						this._frameCounter = 0f;
					}
					else if (num33 > 5f)
					{
						this._frameCounter += 1f;
					}
				}
				break;
			}
			case 3:
			{
				float num34 = 1f;
				if (this._type == 56 || this._type == 61)
				{
					num34 = MathHelper.Clamp(velocity.Length() * 0.5f, 1f, 2f);
				}
				this._frameCounter += num34;
				int num35 = this._data.flyingFrameDelay;
				if (this.Type == 54 && this._flyTime > 0)
				{
					num35 -= 2;
				}
				if (this._frameCounter > (float)num35)
				{
					this._frameCounter -= (float)num35;
					this._frame++;
				}
				if (this._frame < this._data.flyingFrameStart || this._frame >= this._data.flyingFrameStart + this._data.flyingFrameCount)
				{
					this._frame = this._data.flyingFrameStart;
				}
				break;
			}
			case 4:
			{
				int num36 = (int)((Math.Abs(velocity.X) + Math.Abs(velocity.Y)) / 2f);
				this._frameCounter += (float)num36;
				if (this._frameCounter > (float)this._data.swimFrameDelay)
				{
					this._frameCounter -= (float)this._data.swimFrameDelay;
					this._frame++;
				}
				if (this._frame < this._data.swimFrameStart || this._frame >= this._data.swimFrameStart + this._data.swimFrameCount)
				{
					this._frame = this._data.swimFrameStart;
				}
				if (this.Type == 37 && velocity.X == 0f)
				{
					this._frame = 4;
				}
				break;
			}
			case 5:
			{
				type = this._type;
				float num29;
				if (type == 9)
				{
					if (this._flipDraw)
					{
						num29 = -Math.Abs(velocity.X);
					}
					else
					{
						num29 = Math.Abs(velocity.X);
					}
				}
				else
				{
					num29 = Math.Abs(velocity.X);
				}
				this._frameCounter += num29;
				if (num29 >= 0f)
				{
					if (this._frameCounter > (float)this._data.dashingFrameDelay)
					{
						this._frameCounter -= (float)this._data.dashingFrameDelay;
						this._frame++;
					}
					if (this._frame < this._data.dashingFrameStart || this._frame >= this._data.dashingFrameStart + this._data.dashingFrameCount)
					{
						this._frame = this._data.dashingFrameStart;
					}
				}
				else
				{
					if (this._frameCounter < 0f)
					{
						this._frameCounter += (float)this._data.dashingFrameDelay;
						this._frame--;
					}
					if (this._frame < this._data.dashingFrameStart || this._frame >= this._data.dashingFrameStart + this._data.dashingFrameCount)
					{
						this._frame = this._data.dashingFrameStart + this._data.dashingFrameCount - 1;
					}
				}
				break;
			}
			}
			if ((this._type == 62 || this._type == 63) && mountedPlayer.dashDelay < 0 && mountedPlayer.dashDelay >= -5)
			{
				this._frame = 5;
			}
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00037400 File Offset: 0x00035600
		public void UpdateFrame_Velociraptor(Player mountedPlayer, ref int state)
		{
			Mount.SelectiveFlyingMountData selectiveFlyingMountData = (Mount.SelectiveFlyingMountData)this._mountSpecificData;
			if (state == 4)
			{
				state = (selectiveFlyingMountData.allowedToFly ? 3 : 2);
			}
			bool flag = state == 2 || state == 3;
			int num = (mountedPlayer.wings > 0) ? mountedPlayer.wings : mountedPlayer.wingsLogic;
			if (flag && selectiveFlyingMountData.allowedToFly && ((num > 0 && !ArmorIDs.Wing.Sets.AlwaysAnimated[num]) || mountedPlayer.ShouldDrawWingsThatAreAlwaysAnimated(true)))
			{
				state = 3;
				return;
			}
			if (state == 3)
			{
				state = 2;
			}
		}

		// Token: 0x06000281 RID: 641 RVA: 0x00037484 File Offset: 0x00035684
		public void TryBeginningFlight(Player mountedPlayer, int state)
		{
			if (this._frameState == state)
			{
				return;
			}
			if (state != 2 && state != 3)
			{
				return;
			}
			if (!this.CanHover())
			{
				return;
			}
			if (mountedPlayer.controlUp || mountedPlayer.controlDown || mountedPlayer.controlJump)
			{
				return;
			}
			Vector2 velocity = Vector2.UnitY * mountedPlayer.gravDir;
			if (Collision.TileCollision(mountedPlayer.position + new Vector2(0f, -0.001f), velocity, mountedPlayer.width, mountedPlayer.height, false, false, (int)mountedPlayer.gravDir, false, false, true).Y == 0f)
			{
				return;
			}
			if (this.DoesHoverIgnoresFatigue())
			{
				mountedPlayer.position.Y = mountedPlayer.position.Y + -0.001f;
				return;
			}
			float num = mountedPlayer.gravity * mountedPlayer.gravDir;
			mountedPlayer.position.Y = mountedPlayer.position.Y - mountedPlayer.velocity.Y;
			mountedPlayer.velocity.Y = mountedPlayer.velocity.Y - num;
		}

		// Token: 0x06000282 RID: 642 RVA: 0x00037578 File Offset: 0x00035778
		public int GetIntendedGroundedFrame(Player mountedPlayer)
		{
			bool flag = MountID.Sets.CanUseHooks[this.Type] && mountedPlayer.grappling[0] >= 0;
			bool flag2 = mountedPlayer.velocity.X == 0f || ((mountedPlayer.slippy || mountedPlayer.slippy2 || mountedPlayer.windPushed) && !mountedPlayer.controlLeft && !mountedPlayer.controlRight);
			if (flag)
			{
				return 2;
			}
			if (flag2)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x06000283 RID: 643 RVA: 0x000375F0 File Offset: 0x000357F0
		public void TryLanding(Player mountedPlayer)
		{
			if (this._frameState != 3 && this._frameState != 2)
			{
				return;
			}
			if (mountedPlayer.controlUp || mountedPlayer.controlDown || mountedPlayer.controlJump)
			{
				return;
			}
			Vector2 velocity = Vector2.UnitY * mountedPlayer.gravDir * 4f;
			if (Collision.TileCollision(mountedPlayer.position, velocity, mountedPlayer.width, mountedPlayer.height, false, false, (int)mountedPlayer.gravDir, false, false, true).Y != 0f)
			{
				return;
			}
			this.UpdateFrame(mountedPlayer, this.GetIntendedGroundedFrame(mountedPlayer), mountedPlayer.velocity);
		}

		// Token: 0x06000284 RID: 644 RVA: 0x00037690 File Offset: 0x00035890
		private void UpdateFrame_GolfCart(Player mountedPlayer, int state, Vector2 velocity)
		{
			if (state != 2)
			{
				if (this._frameExtraCounter != 0f || this._frameExtra != 0)
				{
					if (this._frameExtraCounter == -1f)
					{
						this._frameExtraCounter = 0f;
						this._frameExtra = 1;
					}
					float num = this._frameExtraCounter + 1f;
					this._frameExtraCounter = num;
					if (num >= 6f)
					{
						this._frameExtraCounter = 0f;
						if (this._frameExtra > 0)
						{
							this._frameExtra--;
						}
					}
				}
				else
				{
					this._frameExtra = 0;
					this._frameExtraCounter = 0f;
				}
			}
			else if (velocity.Y >= 0f)
			{
				if (this._frameExtra < 1)
				{
					this._frameExtra = 1;
				}
				if (this._frameExtra == 2)
				{
					this._frameExtraCounter = -1f;
				}
				else
				{
					float num = this._frameExtraCounter + 1f;
					this._frameExtraCounter = num;
					if (num >= 6f)
					{
						this._frameExtraCounter = 0f;
						if (this._frameExtra < 2)
						{
							this._frameExtra++;
						}
					}
				}
			}
			if (state != 2 && state != 0 && state != 3 && state != 4)
			{
				Mount.EmitGolfCartWheelDust(mountedPlayer, mountedPlayer.Bottom + new Vector2((float)(mountedPlayer.direction * -20), 0f));
				Mount.EmitGolfCartWheelDust(mountedPlayer, mountedPlayer.Bottom + new Vector2((float)(mountedPlayer.direction * 20), 0f));
			}
			Mount.EmitGolfCartlight(mountedPlayer.Bottom + new Vector2((float)(mountedPlayer.direction * 40), -20f), mountedPlayer.direction);
		}

		// Token: 0x06000285 RID: 645 RVA: 0x00037834 File Offset: 0x00035A34
		private static void EmitGolfCartSmoke(Player mountedPlayer, bool rushing)
		{
			Vector2 position = mountedPlayer.Bottom + new Vector2((float)(-(float)mountedPlayer.direction * 34), -mountedPlayer.gravDir * 12f);
			Dust dust = Dust.NewDustDirect(position, 0, 0, 31, (float)(-(float)mountedPlayer.direction), -mountedPlayer.gravDir * 0.24f, 100, default(Color), 1f);
			dust.position = position;
			dust.velocity *= 0.1f;
			dust.velocity += new Vector2((float)(-(float)mountedPlayer.direction), -mountedPlayer.gravDir * 0.25f);
			dust.scale = 0.5f;
			if (mountedPlayer.velocity.X != 0f)
			{
				dust.velocity += new Vector2((float)Math.Sign(mountedPlayer.velocity.X) * 1.3f, 0f);
			}
			if (rushing)
			{
				dust.fadeIn = 0.8f;
			}
		}

		// Token: 0x06000286 RID: 646 RVA: 0x00037940 File Offset: 0x00035B40
		private static void EmitGolfCartlight(Vector2 worldLocation, int playerDirection)
		{
			float num = 0f;
			if (playerDirection == -1)
			{
				num = 3.1415927f;
			}
			float num2 = 0.09817477f;
			int num3 = 5;
			float num4 = 200f;
			DelegateMethods.v2_1 = worldLocation.ToTileCoordinates().ToVector2();
			DelegateMethods.f_1 = num4 / 16f;
			DelegateMethods.v3_1 = new Vector3(0.7f, 0.7f, 0.7f);
			for (float num5 = 0f; num5 < (float)num3; num5 += 1f)
			{
				Vector2 value = (num + num2 * (num5 - (float)(num3 / 2))).ToRotationVector2();
				Utils.PlotTileLine(worldLocation, worldLocation + value * num4, 8f, new Utils.TileActionAttempt(DelegateMethods.CastLightOpen_StopForSolids_ScaleWithDistance));
			}
		}

		// Token: 0x06000287 RID: 647 RVA: 0x000379F1 File Offset: 0x00035BF1
		private static bool ShouldGolfCartEmitLight()
		{
			return true;
		}

		// Token: 0x06000288 RID: 648 RVA: 0x000379F4 File Offset: 0x00035BF4
		private static void EmitGolfCartWheelDust(Player mountedPlayer, Vector2 legSpot)
		{
			if (Main.rand.Next(5) != 0)
			{
				return;
			}
			Point point = (legSpot + new Vector2(0f, mountedPlayer.gravDir * 2f)).ToTileCoordinates();
			if (!WorldGen.InWorld(point.X, point.Y, 10))
			{
				return;
			}
			Tile tileSafely = Framing.GetTileSafely(point.X, point.Y);
			if (!WorldGen.SolidTile(point))
			{
				return;
			}
			int num = WorldGen.KillTile_GetTileDustAmount(true, tileSafely);
			if (num > 1)
			{
				num = 1;
			}
			Vector2 value = new Vector2((float)(-(float)mountedPlayer.direction), -mountedPlayer.gravDir * 1f);
			for (int i = 0; i < num; i++)
			{
				Dust dust = Main.dust[WorldGen.KillTile_MakeTileDust(point.X, point.Y, tileSafely)];
				dust.velocity *= 0.2f;
				dust.velocity += value;
				dust.position = legSpot;
				dust.scale *= 0.8f;
				dust.fadeIn *= 0.8f;
			}
		}

		// Token: 0x06000289 RID: 649 RVA: 0x00037B08 File Offset: 0x00035D08
		private void DoGemMinecartEffect(Player mountedPlayer, int dustType)
		{
			if (Main.rand.Next(10) != 0)
			{
				return;
			}
			Vector2 value = Main.rand.NextVector2Square(-1f, 1f) * new Vector2(22f, 10f);
			Vector2 value2 = new Vector2(0f, 10f) * mountedPlayer.Directions;
			Vector2 vector = mountedPlayer.Center + value2 + value;
			vector = mountedPlayer.RotatedRelativePoint(vector, false, true);
			Dust dust = Dust.NewDustPerfect(vector, dustType, null, 0, default(Color), 1f);
			dust.noGravity = true;
			dust.fadeIn = 0.6f;
			dust.scale = 0.4f;
			dust.velocity *= 0.25f;
			dust.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMinecart, mountedPlayer);
		}

		// Token: 0x0600028A RID: 650 RVA: 0x00037BF0 File Offset: 0x00035DF0
		private void DoSteamMinecartEffect(Player mountedPlayer, int dustType)
		{
			float num = Math.Abs(mountedPlayer.velocity.X);
			if (num < 1f || (num < 6f && this._frame != 0))
			{
				return;
			}
			Vector2 value = Main.rand.NextVector2Square(-1f, 1f) * new Vector2(3f, 3f);
			Vector2 value2 = new Vector2(-10f, -4f) * mountedPlayer.Directions;
			Vector2 vector = mountedPlayer.Center + value2 + value;
			vector = mountedPlayer.RotatedRelativePoint(vector, false, true);
			Dust dust = Dust.NewDustPerfect(vector, dustType, null, 0, default(Color), 1f);
			dust.noGravity = true;
			dust.fadeIn = 0.6f;
			dust.scale = 1.8f;
			dust.velocity *= 0.25f;
			dust.velocity.Y = dust.velocity.Y - 2f;
			dust.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMinecart, mountedPlayer);
		}

		// Token: 0x0600028B RID: 651 RVA: 0x00037D08 File Offset: 0x00035F08
		private void DoExhaustMinecartEffect(Player mountedPlayer, int dustType)
		{
			float num = mountedPlayer.velocity.Length();
			if (num < 1f && Main.rand.Next(4) != 0)
			{
				return;
			}
			int i = 1 + (int)num / 6;
			while (i > 0)
			{
				i--;
				Vector2 value = Main.rand.NextVector2Square(-1f, 1f) * new Vector2(3f, 3f);
				Vector2 vector = new Vector2(-18f, 20f) * mountedPlayer.Directions;
				if (num > 6f)
				{
					vector.X += (float)(4 * mountedPlayer.direction);
				}
				if (i > 0)
				{
					vector += mountedPlayer.velocity * (float)(i / 3);
				}
				Vector2 vector2 = mountedPlayer.Center + vector + value;
				vector2 = mountedPlayer.RotatedRelativePoint(vector2, false, true);
				Dust dust = Dust.NewDustPerfect(vector2, dustType, null, 0, default(Color), 1f);
				dust.noGravity = true;
				dust.fadeIn = 0.6f;
				dust.scale = 1.2f;
				dust.velocity *= 0.2f;
				if (num < 1f)
				{
					Dust dust2 = dust;
					dust2.velocity.X = dust2.velocity.X - 0.5f * (float)mountedPlayer.direction;
				}
				dust.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMinecart, mountedPlayer);
			}
		}

		// Token: 0x0600028C RID: 652 RVA: 0x00037E80 File Offset: 0x00036080
		private void DoConfettiMinecartEffect(Player mountedPlayer)
		{
			float num = mountedPlayer.velocity.Length();
			if ((num < 1f && Main.rand.Next(6) != 0) || (num < 3f && Main.rand.Next(3) != 0))
			{
				return;
			}
			int i = 1 + (int)num / 6;
			while (i > 0)
			{
				i--;
				float num2 = Main.rand.NextFloat() * 2f;
				Vector2 value = Main.rand.NextVector2Square(-1f, 1f) * new Vector2(3f, 8f);
				Vector2 vector = new Vector2(-18f, 4f) * mountedPlayer.Directions;
				vector.X += num * (float)mountedPlayer.direction * 0.5f + (float)(mountedPlayer.direction * i) * num2;
				if (i > 0)
				{
					vector += mountedPlayer.velocity * (float)(i / 3);
				}
				Vector2 vector2 = mountedPlayer.Center + vector + value;
				vector2 = mountedPlayer.RotatedRelativePoint(vector2, false, true);
				Dust dust = Dust.NewDustPerfect(vector2, 139 + Main.rand.Next(4), null, 0, default(Color), 1f);
				dust.noGravity = true;
				dust.fadeIn = 0.6f;
				dust.scale = 0.5f + num2 / 2f;
				dust.velocity *= 0.2f;
				if (num < 1f)
				{
					Dust dust2 = dust;
					dust2.velocity.X = dust2.velocity.X - 0.5f * (float)mountedPlayer.direction;
				}
				dust.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMinecart, mountedPlayer);
			}
		}

		// Token: 0x0600028D RID: 653 RVA: 0x00038044 File Offset: 0x00036244
		public void UpdateEffects(Player mountedPlayer)
		{
			mountedPlayer.autoJump = this.AutoJump;
			this._shouldSuperCart = (MountID.Sets.Cart[this._type] && mountedPlayer.UsingSuperCart);
			if (this._shouldSuperCart)
			{
				this.CastSuperCartLaser(mountedPlayer);
				float num = 1f + Math.Abs(mountedPlayer.velocity.X) / this.RunSpeed * 2.5f;
				mountedPlayer.statDefense += (int)(2f * num);
			}
			switch (this._type)
			{
			case 8:
				if (mountedPlayer.ownedProjectileCounts[453] < 1)
				{
					this._abilityActive = false;
					return;
				}
				break;
			case 9:
			case 46:
			{
				if (this._type == 46)
				{
					mountedPlayer.hasJumpOption_Santank = true;
				}
				Vector2 center = mountedPlayer.Center;
				Vector2 vector = center;
				bool flag = false;
				float num2 = 1500f;
				float num3 = 850f;
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					NPC npc = Main.npc[i];
					if (npc.CanBeChasedBy(this, false))
					{
						Vector2 v = npc.Center - center;
						float num4 = v.Length();
						if (num4 < num3 && ((Vector2.Distance(vector, center) > num4 && num4 < num2) || !flag))
						{
							bool flag2 = true;
							float num5 = Math.Abs(v.ToRotation());
							if (mountedPlayer.direction == 1 && (double)num5 > 1.047197594907988)
							{
								flag2 = false;
							}
							else if (mountedPlayer.direction == -1 && (double)num5 < 2.0943951461045853)
							{
								flag2 = false;
							}
							if (Collision.CanHitLine(center, 0, 0, npc.position, npc.width, npc.height) && flag2)
							{
								num2 = num4;
								vector = npc.Center;
								flag = true;
							}
						}
					}
				}
				if (!flag)
				{
					this._abilityCharging = false;
					this.ResetHeadPosition();
					return;
				}
				bool flag3 = this._abilityCooldown == 0;
				if (this._type == 46)
				{
					flag3 = (this._abilityCooldown % 10 == 0);
				}
				if (flag3 && mountedPlayer.whoAmI == Main.myPlayer)
				{
					this.AimAbility(mountedPlayer, vector);
					if (this._abilityCooldown == 0)
					{
						this.StopAbilityCharge();
					}
					this.UseAbility(mountedPlayer, vector, false);
					return;
				}
				this.AimAbility(mountedPlayer, vector);
				this._abilityCharging = true;
				return;
			}
			case 10:
				mountedPlayer.hasJumpOption_Unicorn = true;
				if (Math.Abs(mountedPlayer.velocity.X) > mountedPlayer.mount.DashSpeed - mountedPlayer.mount.RunSpeed / 2f)
				{
					mountedPlayer.noKnockback = true;
				}
				if (mountedPlayer.itemAnimation > 0 && mountedPlayer.inventory[mountedPlayer.selectedItem].type == 1260)
				{
					AchievementsHelper.HandleSpecialEvent(mountedPlayer, 5);
					return;
				}
				break;
			case 11:
			{
				Vector3 vector2 = new Vector3(0.4f, 0.12f, 0.15f);
				float scaleFactor = 1f + Math.Abs(mountedPlayer.velocity.X) / this.RunSpeed * 2.5f;
				int num6 = Math.Sign(mountedPlayer.velocity.X);
				if (num6 == 0)
				{
					num6 = mountedPlayer.direction;
				}
				if (Main.netMode != 2)
				{
					vector2 *= scaleFactor;
					Lighting.AddLight(mountedPlayer.Center, vector2.X, vector2.Y, vector2.Z);
					Lighting.AddLight(mountedPlayer.Top, vector2.X, vector2.Y, vector2.Z);
					Lighting.AddLight(mountedPlayer.Bottom, vector2.X, vector2.Y, vector2.Z);
					Lighting.AddLight(mountedPlayer.Left, vector2.X, vector2.Y, vector2.Z);
					Lighting.AddLight(mountedPlayer.Right, vector2.X, vector2.Y, vector2.Z);
					float num7 = -24f;
					if (mountedPlayer.direction != num6)
					{
						num7 = -22f;
					}
					if (num6 == -1)
					{
						num7 += 1f;
					}
					Vector2 value = new Vector2(num7 * (float)num6, -19f).RotatedBy((double)mountedPlayer.fullRotation, default(Vector2));
					Vector2 vector3 = new Vector2(MathHelper.Lerp(0f, -8f, mountedPlayer.fullRotation / 0.7853982f), MathHelper.Lerp(0f, 2f, Math.Abs(mountedPlayer.fullRotation / 0.7853982f))).RotatedBy((double)mountedPlayer.fullRotation, default(Vector2));
					if (num6 == Math.Sign(mountedPlayer.fullRotation))
					{
						vector3 *= MathHelper.Lerp(1f, 0.6f, Math.Abs(mountedPlayer.fullRotation / 0.7853982f));
					}
					Vector2 vector4 = mountedPlayer.Bottom + value + vector3;
					Vector2 vector5 = mountedPlayer.oldPosition + mountedPlayer.Size * new Vector2(0.5f, 1f) + value + vector3;
					if (Vector2.Distance(vector4, vector5) > 3f)
					{
						int num8 = (int)Vector2.Distance(vector4, vector5) / 3;
						if (Vector2.Distance(vector4, vector5) % 3f != 0f)
						{
							num8++;
						}
						for (float num9 = 1f; num9 <= (float)num8; num9 += 1f)
						{
							Dust dust = Main.dust[Dust.NewDust(mountedPlayer.Center, 0, 0, 182, 0f, 0f, 0, default(Color), 1f)];
							dust.position = Vector2.Lerp(vector5, vector4, num9 / (float)num8);
							dust.noGravity = true;
							dust.velocity = Vector2.Zero;
							dust.customData = mountedPlayer;
							dust.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMinecart, mountedPlayer);
						}
						return;
					}
					Dust dust2 = Main.dust[Dust.NewDust(mountedPlayer.Center, 0, 0, 182, 0f, 0f, 0, default(Color), 1f)];
					dust2.position = vector4;
					dust2.noGravity = true;
					dust2.velocity = Vector2.Zero;
					dust2.customData = mountedPlayer;
					dust2.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMinecart, mountedPlayer);
					return;
				}
				break;
			}
			case 12:
				if (mountedPlayer.MountFishronSpecial)
				{
					Vector3 vector6 = Colors.CurrentLiquidColor.ToVector3();
					vector6 *= 0.4f;
					Point point = (mountedPlayer.Center + Vector2.UnitX * (float)mountedPlayer.direction * 20f + mountedPlayer.velocity * 10f).ToTileCoordinates();
					if (!WorldGen.SolidTile(point.X, point.Y, false))
					{
						Lighting.AddLight(point.X, point.Y, vector6.X, vector6.Y, vector6.Z);
					}
					else
					{
						Lighting.AddLight(mountedPlayer.Center + Vector2.UnitX * (float)mountedPlayer.direction * 20f, vector6.X, vector6.Y, vector6.Z);
					}
					mountedPlayer.meleeDamage += 0.15f;
					mountedPlayer.rangedDamage += 0.15f;
					mountedPlayer.magicDamage += 0.15f;
					mountedPlayer.minionDamage += 0.15f;
				}
				if (mountedPlayer.statLife <= mountedPlayer.statLifeMax2 / 2)
				{
					mountedPlayer.MountFishronSpecialCounter = 60f;
				}
				if (mountedPlayer.wet || (Main.raining && WorldGen.InAPlaceWithWind(mountedPlayer.position, mountedPlayer.width, mountedPlayer.height)))
				{
					mountedPlayer.MountFishronSpecialCounter = 420f;
					return;
				}
				break;
			case 13:
			case 15:
			case 17:
			case 18:
			case 19:
			case 20:
			case 21:
			case 33:
			case 35:
			case 38:
			case 39:
			case 43:
			case 44:
			case 45:
			case 48:
			case 49:
			case 50:
			case 51:
			case 52:
			case 53:
			case 54:
				break;
			case 14:
				mountedPlayer.hasJumpOption_Basilisk = true;
				if (Math.Abs(mountedPlayer.velocity.X) > mountedPlayer.mount.DashSpeed - mountedPlayer.mount.RunSpeed / 2f)
				{
					mountedPlayer.noKnockback = true;
					return;
				}
				break;
			case 16:
				mountedPlayer.ignoreWater = true;
				return;
			case 22:
			{
				mountedPlayer.lavaMax += 420;
				Vector2 vector7 = mountedPlayer.Center + new Vector2(20f, 10f) * mountedPlayer.Directions;
				Vector2 vector8 = vector7 + mountedPlayer.velocity;
				Vector2 vector9 = vector7 + new Vector2(-1f, -0.5f) * mountedPlayer.Directions;
				vector7 = mountedPlayer.RotatedRelativePoint(vector7, false, true);
				vector8 = mountedPlayer.RotatedRelativePoint(vector8, false, true);
				vector9 = mountedPlayer.RotatedRelativePoint(vector9, false, true);
				Vector2 value2 = mountedPlayer.shadowPos[2] - mountedPlayer.position + vector7;
				Vector2 value3 = vector8 - vector7;
				vector7 += value3;
				value2 += value3;
				Vector2 value4 = vector8 - vector9;
				float num10 = MathHelper.Clamp(mountedPlayer.velocity.Length() / 5f, 0f, 1f);
				for (float num11 = 0f; num11 <= 1f; num11 += 0.1f)
				{
					if (Main.rand.NextFloat() >= num10)
					{
						Dust dust3 = Dust.NewDustPerfect(Vector2.Lerp(value2, vector7, num11), 65, new Vector2?(Main.rand.NextVector2Circular(0.5f, 0.5f) * num10), 0, default(Color), 1f);
						dust3.scale = 0.6f;
						dust3.fadeIn = 0f;
						dust3.customData = mountedPlayer;
						dust3.velocity *= -1f;
						dust3.noGravity = true;
						dust3.velocity -= value4;
						dust3.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMinecart, mountedPlayer);
						if (Main.rand.Next(10) == 0)
						{
							dust3.fadeIn = 1.3f;
							dust3.velocity = Main.rand.NextVector2Circular(3f, 3f) * num10;
						}
					}
				}
				return;
			}
			case 23:
			{
				Vector2 pos = mountedPlayer.Center + this.GetWitchBroomTrinketOriginOffset(mountedPlayer) + (this.GetWitchBroomTrinketRotation(mountedPlayer) + 1.5707964f).ToRotationVector2() * 11f;
				Vector3 rgb = new Vector3(1f, 0.75f, 0.5f) * 0.85f;
				Vector2 vector10 = mountedPlayer.RotatedRelativePoint(pos, false, true);
				Lighting.AddLight(vector10, rgb);
				if (Main.rand.Next(45) == 0)
				{
					Vector2 vector11 = Main.rand.NextVector2Circular(4f, 4f);
					Dust dust4 = Dust.NewDustPerfect(vector10 + vector11, 43, new Vector2?(Vector2.Zero), 254, new Color(255, 255, 0, 255), 0.3f);
					if (vector11 != Vector2.Zero)
					{
						dust4.velocity = vector10.DirectionTo(dust4.position) * 0.2f;
					}
					dust4.fadeIn = 0.3f;
					dust4.noLightEmittence = true;
					dust4.customData = mountedPlayer;
					dust4.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMount, mountedPlayer);
				}
				float num12 = 0.1f;
				num12 += mountedPlayer.velocity.Length() / 30f;
				Vector2 vector12 = mountedPlayer.Center + new Vector2(18f - 20f * Main.rand.NextFloat() * (float)mountedPlayer.direction, 12f);
				Vector2 vector13 = mountedPlayer.Center + new Vector2((float)(52 * mountedPlayer.direction), -6f);
				vector12 = mountedPlayer.RotatedRelativePoint(vector12, false, true);
				vector13 = mountedPlayer.RotatedRelativePoint(vector13, false, true);
				if (Main.rand.NextFloat() <= num12)
				{
					float num13 = Main.rand.NextFloat();
					for (float num14 = 0f; num14 < 1f; num14 += 0.125f)
					{
						if (Main.rand.Next(15) == 0)
						{
							Vector2 vector14 = (6.2831855f * num14 + num13).ToRotationVector2() * new Vector2(0.5f, 1f) * 4f;
							vector14 = vector14.RotatedBy((double)mountedPlayer.fullRotation, default(Vector2));
							Dust dust5 = Dust.NewDustPerfect(vector12 + vector14, 43, new Vector2?(Vector2.Zero), 254, new Color(255, 255, 0, 255), 0.3f);
							dust5.velocity = vector14 * 0.025f + vector13.DirectionTo(dust5.position) * 0.5f;
							dust5.fadeIn = 0.3f;
							dust5.noLightEmittence = true;
							dust5.shader = GameShaders.Armor.GetSecondaryShader(mountedPlayer.cMount, mountedPlayer);
						}
					}
					return;
				}
				break;
			}
			case 24:
				DelegateMethods.v3_1 = new Vector3(0.1f, 0.3f, 1f) * 0.4f;
				Utils.PlotTileLine(mountedPlayer.MountedCenter, mountedPlayer.MountedCenter + mountedPlayer.velocity * 6f, 40f, new Utils.TileActionAttempt(DelegateMethods.CastLightOpen));
				Utils.PlotTileLine(mountedPlayer.Left, mountedPlayer.Right, 40f, new Utils.TileActionAttempt(DelegateMethods.CastLightOpen));
				return;
			case 25:
				this.DoGemMinecartEffect(mountedPlayer, 86);
				return;
			case 26:
				this.DoGemMinecartEffect(mountedPlayer, 87);
				return;
			case 27:
				this.DoGemMinecartEffect(mountedPlayer, 88);
				return;
			case 28:
				this.DoGemMinecartEffect(mountedPlayer, 89);
				return;
			case 29:
				this.DoGemMinecartEffect(mountedPlayer, 90);
				return;
			case 30:
				this.DoGemMinecartEffect(mountedPlayer, 91);
				return;
			case 31:
				this.DoGemMinecartEffect(mountedPlayer, 262);
				return;
			case 32:
				this.DoExhaustMinecartEffect(mountedPlayer, 31);
				return;
			case 34:
				this.DoConfettiMinecartEffect(mountedPlayer);
				return;
			case 36:
				this.DoSteamMinecartEffect(mountedPlayer, 303);
				return;
			case 37:
				mountedPlayer.canFloatInWater = true;
				mountedPlayer.accFlipper = true;
				return;
			case 40:
			case 41:
			case 42:
				if (Math.Abs(mountedPlayer.velocity.X) > mountedPlayer.mount.DashSpeed - mountedPlayer.mount.RunSpeed / 2f)
				{
					mountedPlayer.noKnockback = true;
					return;
				}
				break;
			case 47:
				mountedPlayer.hasJumpOption_WallOfFleshGoat = true;
				if (Math.Abs(mountedPlayer.velocity.X) > mountedPlayer.mount.DashSpeed - mountedPlayer.mount.RunSpeed / 2f)
				{
					mountedPlayer.noKnockback = true;
					return;
				}
				break;
			case 55:
			case 56:
				mountedPlayer.IsAllowedToHoldItems = false;
				mountedPlayer.noItems = true;
				return;
			case 57:
			case 58:
			case 59:
			case 60:
				mountedPlayer.MinecartSettings.MagnetOffset.Y = mountedPlayer.MinecartSettings.MagnetOffset.Y - 5f;
				mountedPlayer.MinecartSettings.MinecartTextureWidth = 4f;
				mountedPlayer.MinecartSettings.MagnetOffset.X = 2f;
				mountedPlayer.MinecartSettings.WheelOffset.X = 4f;
				mountedPlayer.doorHelper.AllowOpeningDoorsByVelocityAloneForATime(60);
				break;
			case 61:
			{
				mountedPlayer.IsAllowedToHoldItems = false;
				mountedPlayer.noItems = true;
				bool flag4 = Main.rand.Next(15) == 0;
				if ((int)Main.timeForVisualEffects % 2 == 0 && (flag4 || (float)(Main.rand.Next(6) + 1) < mountedPlayer.velocity.Length()))
				{
					Color fairyQueenWeaponsColorFull = Projectile.GetFairyQueenWeaponsColorFull(mountedPlayer.whoAmI, mountedPlayer.Center, 0.41f, 1f, 0.45f, 1f, 0.7f);
					Color fairyQueenWeaponsColorFull2 = Projectile.GetFairyQueenWeaponsColorFull(mountedPlayer.whoAmI, mountedPlayer.Center, 0.41f, 1f, 0f, 1f, 0.7f);
					Dust dust6 = Dust.NewDustDirect(mountedPlayer.Center, 0, 0, 278, 0f, 0f, 200, Color.Lerp(fairyQueenWeaponsColorFull, fairyQueenWeaponsColorFull2, Main.rand.NextFloat()), 0.65f);
					dust6.position = mountedPlayer.Center + new Vector2(0f, -2f);
					if (flag4)
					{
						dust6.velocity *= 0.4f;
					}
					else
					{
						dust6.velocity *= 0.04f * mountedPlayer.velocity.Length();
					}
					dust6.velocity += mountedPlayer.velocity * 0.3f;
					dust6.position += mountedPlayer.velocity * 0.7f;
					dust6.position += (Main.rand.NextFloat() * 6.2831855f).ToRotationVector2() * Main.rand.NextFloat() * 2f;
					dust6.noGravity = true;
					dust6.noLight = true;
					return;
				}
				break;
			}
			case 62:
			case 63:
				mountedPlayer.meleeDamage += 0.1f;
				mountedPlayer.rangedDamage += 0.1f;
				mountedPlayer.magicDamage += 0.1f;
				mountedPlayer.minionDamage += 0.1f;
				return;
			default:
				return;
			}
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0003920C File Offset: 0x0003740C
		private void CastSuperCartLaser(Player mountedPlayer)
		{
			int num = Math.Sign(mountedPlayer.velocity.X);
			if (num == 0)
			{
				num = mountedPlayer.direction;
			}
			if (mountedPlayer.whoAmI == Main.myPlayer && mountedPlayer.velocity.X != 0f)
			{
				Vector2 minecartMechPoint = Mount.GetMinecartMechPoint(mountedPlayer, 20, -19);
				int damage = 60;
				int num2 = 0;
				float num3 = 0f;
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					NPC npc = Main.npc[i];
					if (npc.active && npc.immune[mountedPlayer.whoAmI] <= 0 && !npc.dontTakeDamage && npc.Distance(minecartMechPoint) < 300f && npc.CanBeChasedBy(mountedPlayer, false) && Collision.CanHitLine(npc.position, npc.width, npc.height, minecartMechPoint, 0, 0) && Math.Abs(MathHelper.WrapAngle(MathHelper.WrapAngle(npc.AngleFrom(minecartMechPoint)) - MathHelper.WrapAngle((mountedPlayer.fullRotation + (float)num == -1f) ? 3.1415927f : 0f))) < 0.7853982f)
					{
						minecartMechPoint = Mount.GetMinecartMechPoint(mountedPlayer, -20, -39);
						Vector2 vector = npc.position + npc.Size * Utils.RandomVector2(Main.rand, 0f, 1f) - minecartMechPoint;
						num3 += vector.ToRotation();
						num2++;
						int num4 = Projectile.NewProjectile(this.GetProjectileSpawnSource(mountedPlayer), minecartMechPoint.X, minecartMechPoint.Y, vector.X, vector.Y, 591, 0, 0f, mountedPlayer.whoAmI, (float)mountedPlayer.whoAmI, 0f, 0f, null);
						Main.projectile[num4].Center = npc.Center;
						Main.projectile[num4].damage = damage;
						Main.projectile[num4].Damage();
						Main.projectile[num4].damage = 0;
						Main.projectile[num4].Center = minecartMechPoint;
					}
				}
			}
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0003942C File Offset: 0x0003762C
		public static Vector2 GetMinecartMechPoint(Player mountedPlayer, int offX, int offY)
		{
			int num = Math.Sign(mountedPlayer.velocity.X);
			if (num == 0)
			{
				num = mountedPlayer.direction;
			}
			float num2 = (float)offX;
			int num3 = Math.Sign(offX);
			if (mountedPlayer.direction != num)
			{
				num2 -= (float)num3;
			}
			if (num == -1)
			{
				num2 -= (float)num3;
			}
			Vector2 value = new Vector2(num2 * (float)num, (float)offY).RotatedBy((double)mountedPlayer.fullRotation, default(Vector2));
			Vector2 vector = new Vector2(MathHelper.Lerp(0f, -8f, mountedPlayer.fullRotation / 0.7853982f), MathHelper.Lerp(0f, 2f, Math.Abs(mountedPlayer.fullRotation / 0.7853982f))).RotatedBy((double)mountedPlayer.fullRotation, default(Vector2));
			if (num == Math.Sign(mountedPlayer.fullRotation))
			{
				vector *= MathHelper.Lerp(1f, 0.6f, Math.Abs(mountedPlayer.fullRotation / 0.7853982f));
			}
			return mountedPlayer.Bottom + value + vector;
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0003953C File Offset: 0x0003773C
		public void ResetFlightTime(Player mountedPlayer)
		{
			this._flyTime = (this._active ? this._data.flightTimeMax : 0);
			if (this._type == 0)
			{
				this._flyTime += (int)(Math.Abs(mountedPlayer.velocity.X) * 20f);
			}
			if (this._type == 54)
			{
				this._flyTime = mountedPlayer.wingTimeMax;
			}
		}

		// Token: 0x06000291 RID: 657 RVA: 0x000395A7 File Offset: 0x000377A7
		public void CheckMountBuff(Player mountedPlayer)
		{
			if (this._type == -1 || mountedPlayer.FindBuffIndex(this._data.buff) != -1)
			{
				return;
			}
			this.TryDismount(mountedPlayer);
		}

		// Token: 0x06000292 RID: 658 RVA: 0x000395CF File Offset: 0x000377CF
		public void ResetHeadPosition()
		{
			if (this._aiming)
			{
				this._aiming = false;
				if (this._type != 46)
				{
					this._frameExtra = 0;
				}
				this._flipDraw = false;
			}
		}

		// Token: 0x06000293 RID: 659 RVA: 0x000395F8 File Offset: 0x000377F8
		private Vector2 ClampToDeadZone(Player mountedPlayer, Vector2 position)
		{
			int type = this._type;
			int num;
			int num2;
			if (type != 8)
			{
				if (type != 9)
				{
					if (type != 46)
					{
						return position;
					}
					num = (int)Mount.santankTextureSize.Y;
					num2 = (int)Mount.santankTextureSize.X;
				}
				else
				{
					num = (int)Mount.scutlixTextureSize.Y;
					num2 = (int)Mount.scutlixTextureSize.X;
				}
			}
			else
			{
				num = (int)Mount.drillTextureSize.Y;
				num2 = (int)Mount.drillTextureSize.X;
			}
			Vector2 center = mountedPlayer.Center;
			position -= center;
			if (position.X > (float)(-(float)num2) && position.X < (float)num2 && position.Y > (float)(-(float)num) && position.Y < (float)num)
			{
				float num3 = (float)num2 / Math.Abs(position.X);
				float num4 = (float)num / Math.Abs(position.Y);
				if (num3 > num4)
				{
					position *= num4;
				}
				else
				{
					position *= num3;
				}
			}
			return position + center;
		}

		// Token: 0x06000294 RID: 660 RVA: 0x000396EC File Offset: 0x000378EC
		public bool AimAbility(Player mountedPlayer, Vector2 mousePosition)
		{
			this._aiming = true;
			int type = this._type;
			if (type == 8)
			{
				Vector2 v = this.ClampToDeadZone(mountedPlayer, mousePosition) - mountedPlayer.Center;
				Mount.DrillMountData drillMountData = (Mount.DrillMountData)this._mountSpecificData;
				float num = v.ToRotation();
				if (num < 0f)
				{
					num += 6.2831855f;
				}
				drillMountData.diodeRotationTarget = num;
				float num2 = drillMountData.diodeRotation % 6.2831855f;
				if (num2 < 0f)
				{
					num2 += 6.2831855f;
				}
				if (num2 < num)
				{
					if (num - num2 > 3.1415927f)
					{
						num2 += 6.2831855f;
					}
				}
				else if (num2 - num > 3.1415927f)
				{
					num2 -= 6.2831855f;
				}
				drillMountData.diodeRotation = num2;
				drillMountData.crosshairPosition = mousePosition;
				return true;
			}
			int frameExtra;
			int direction;
			float num3;
			float abilityCharge;
			if (type == 9)
			{
				frameExtra = this._frameExtra;
				direction = mountedPlayer.direction;
				num3 = MathHelper.ToDegrees((this.ClampToDeadZone(mountedPlayer, mousePosition) - mountedPlayer.Center).ToRotation());
				if (num3 > 90f)
				{
					mountedPlayer.direction = -1;
					num3 = 180f - num3;
				}
				else if (num3 < -90f)
				{
					mountedPlayer.direction = -1;
					num3 = -180f - num3;
				}
				else
				{
					mountedPlayer.direction = 1;
				}
				if ((mountedPlayer.direction > 0 && mountedPlayer.velocity.X < 0f) || (mountedPlayer.direction < 0 && mountedPlayer.velocity.X > 0f))
				{
					this._flipDraw = true;
				}
				else
				{
					this._flipDraw = false;
				}
				if (num3 >= 0f)
				{
					if ((double)num3 < 22.5)
					{
						this._frameExtra = 8;
					}
					else if ((double)num3 < 67.5)
					{
						this._frameExtra = 9;
					}
					else if ((double)num3 < 112.5)
					{
						this._frameExtra = 10;
					}
				}
				else if ((double)num3 > -22.5)
				{
					this._frameExtra = 8;
				}
				else if ((double)num3 > -67.5)
				{
					this._frameExtra = 7;
				}
				else if ((double)num3 > -112.5)
				{
					this._frameExtra = 6;
				}
				abilityCharge = this.AbilityCharge;
				if (abilityCharge > 0f)
				{
					Vector2 vector;
					vector.X = mountedPlayer.position.X + (float)(mountedPlayer.width / 2);
					vector.Y = mountedPlayer.position.Y + (float)mountedPlayer.height;
					int num4 = (this._frameExtra - 6) * 2;
					for (int i = 0; i < 2; i++)
					{
						Vector2 vector2;
						vector2.Y = vector.Y + Mount.scutlixEyePositions[num4 + i].Y;
						if (mountedPlayer.direction == -1)
						{
							vector2.X = vector.X - Mount.scutlixEyePositions[num4 + i].X - (float)this._data.xOffset;
						}
						else
						{
							vector2.X = vector.X + Mount.scutlixEyePositions[num4 + i].X + (float)this._data.xOffset;
						}
						Lighting.AddLight((int)(vector2.X / 16f), (int)(vector2.Y / 16f), 1f * abilityCharge, 0f, 0f);
					}
				}
				return this._frameExtra != frameExtra || mountedPlayer.direction != direction;
			}
			if (type != 46)
			{
				return false;
			}
			frameExtra = this._frameExtra;
			direction = mountedPlayer.direction;
			num3 = MathHelper.ToDegrees((this.ClampToDeadZone(mountedPlayer, mousePosition) - mountedPlayer.Center).ToRotation());
			if (num3 > 90f)
			{
				mountedPlayer.direction = -1;
				num3 = 180f - num3;
			}
			else if (num3 < -90f)
			{
				mountedPlayer.direction = -1;
				num3 = -180f - num3;
			}
			else
			{
				mountedPlayer.direction = 1;
			}
			if ((mountedPlayer.direction > 0 && mountedPlayer.velocity.X < 0f) || (mountedPlayer.direction < 0 && mountedPlayer.velocity.X > 0f))
			{
				this._flipDraw = true;
			}
			else
			{
				this._flipDraw = false;
			}
			abilityCharge = this.AbilityCharge;
			if (abilityCharge > 0f)
			{
				Vector2 vector3;
				vector3.X = mountedPlayer.position.X + (float)(mountedPlayer.width / 2);
				vector3.Y = mountedPlayer.position.Y + (float)mountedPlayer.height;
				for (int j = 0; j < 2; j++)
				{
					Vector2 vector4 = new Vector2(vector3.X + (float)(mountedPlayer.width * mountedPlayer.direction), vector3.Y - 12f);
					Lighting.AddLight((int)(vector4.X / 16f), (int)(vector4.Y / 16f), 0.7f, 0.4f, 0.4f);
				}
			}
			return this._frameExtra != frameExtra || mountedPlayer.direction != direction;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x00039BC0 File Offset: 0x00037DC0
		public void Draw(List<DrawData> playerDrawData, int drawType, Player drawPlayer, Vector2 Position, Color drawColor, SpriteEffects playerEffect, float shadow)
		{
			if (playerDrawData == null)
			{
				return;
			}
			Texture2D texture2D;
			Texture2D texture2D2;
			switch (drawType)
			{
			case 0:
				texture2D = this._data.backTexture.Value;
				texture2D2 = this._data.backTextureGlow.Value;
				break;
			case 1:
				texture2D = this._data.backTextureExtra.Value;
				texture2D2 = this._data.backTextureExtraGlow.Value;
				break;
			case 2:
				if (this._type == 0 && this._idleTime >= this._idleTimeNext)
				{
					return;
				}
				texture2D = this._data.frontTexture.Value;
				texture2D2 = this._data.frontTextureGlow.Value;
				break;
			case 3:
				texture2D = this._data.frontTextureExtra.Value;
				texture2D2 = this._data.frontTextureExtraGlow.Value;
				break;
			default:
				texture2D = null;
				texture2D2 = null;
				break;
			}
			int type = this._type;
			if (type == 50 && texture2D != null)
			{
				PlayerQueenSlimeMountTextureContent queenSlimeMount = TextureAssets.RenderTargets.QueenSlimeMount;
				queenSlimeMount.Request();
				if (queenSlimeMount.IsReady)
				{
					texture2D = queenSlimeMount.GetTarget();
				}
			}
			if (texture2D == null)
			{
				return;
			}
			type = this._type;
			if ((type == 0 || type == 9) && drawType == 3 && shadow != 0f)
			{
				return;
			}
			int num = this.XOffset;
			int num2 = this.YOffset + this.PlayerOffset;
			if (drawPlayer.direction <= 0)
			{
				num *= -1;
			}
			Position.X = (float)((int)(Position.X - Main.screenPosition.X + (float)(drawPlayer.width / 2) + (float)num));
			Position.Y = (float)((int)(Position.Y - Main.screenPosition.Y + (float)(drawPlayer.height / 2) + (float)num2));
			bool flag = true;
			int num3 = this._data.totalFrames;
			int num4 = this._data.textureHeight;
			type = this._type;
			int num5;
			if (type <= 23)
			{
				if (type <= 9)
				{
					if (type != 5)
					{
						if (type == 9)
						{
							switch (drawType)
							{
							case 0:
								num5 = this._frame;
								goto IL_4DA;
							case 2:
								num5 = this._frameExtra;
								goto IL_4DA;
							case 3:
								num5 = this._frameExtra;
								goto IL_4DA;
							}
							num5 = 0;
							goto IL_4DA;
						}
					}
					else
					{
						if (drawType == 0)
						{
							num5 = this._frame;
							goto IL_4DA;
						}
						if (drawType != 1)
						{
							num5 = 0;
							goto IL_4DA;
						}
						num5 = this._frameExtra;
						goto IL_4DA;
					}
				}
				else if (type != 17)
				{
					if (type == 23)
					{
						num5 = this._frame;
						goto IL_4DA;
					}
				}
				else
				{
					num4 = texture2D.Height;
					if (drawType == 0)
					{
						num5 = this._frame;
						num3 = 4;
						goto IL_4DA;
					}
					if (drawType != 1)
					{
						num5 = 0;
						goto IL_4DA;
					}
					num5 = this._frameExtra;
					num3 = 4;
					goto IL_4DA;
				}
			}
			else if (type <= 46)
			{
				if (type != 39)
				{
					if (type == 46)
					{
						if (drawType == 2)
						{
							num5 = this._frame;
							goto IL_4DA;
						}
						if (drawType != 3)
						{
							num5 = 0;
							goto IL_4DA;
						}
						num5 = this._frameExtra;
						goto IL_4DA;
					}
				}
				else
				{
					num4 = texture2D.Height;
					if (drawType == 2)
					{
						num5 = this._frame;
						num3 = 3;
						goto IL_4DA;
					}
					if (drawType != 3)
					{
						num5 = 0;
						goto IL_4DA;
					}
					num5 = this._frameExtra;
					num3 = 6;
					goto IL_4DA;
				}
			}
			else if (type != 52)
			{
				if (type != 54)
				{
					if (type - 62 <= 1)
					{
						num5 = this._frame;
						if (num5 < 4 && drawPlayer.petting.isPetting && drawPlayer.petting.mount)
						{
							num5 = 12;
							goto IL_4DA;
						}
						goto IL_4DA;
					}
				}
				else
				{
					if (drawType != 3)
					{
						num5 = this._frame;
						goto IL_4DA;
					}
					if (drawPlayer.itemAnimation > 0)
					{
						Rectangle bodyFrame = drawPlayer.bodyFrame;
						int value = bodyFrame.Y / bodyFrame.Height;
						int useStyle = drawPlayer.lastVisualizedSelectedItem.useStyle;
						num5 = Utils.Clamp<int>(value, 1, 4);
						if (useStyle == 12 && drawPlayer.itemAnimation > drawPlayer.itemAnimationMax / 2)
						{
							num5 = 3;
						}
						if (useStyle == 2 || useStyle == 9 || useStyle == 4 || useStyle == 14)
						{
							num5 = 2;
						}
						if (useStyle == 8 || useStyle == 11)
						{
							num5 = 3;
							goto IL_4DA;
						}
						goto IL_4DA;
					}
					else
					{
						int holdStyle = drawPlayer.lastVisualizedSelectedItem.holdStyle;
						if (holdStyle == 1 || holdStyle == 6)
						{
							num5 = 3;
							goto IL_4DA;
						}
						if (holdStyle == 2)
						{
							num5 = 2;
							goto IL_4DA;
						}
						num5 = this._frame;
						goto IL_4DA;
					}
				}
			}
			else
			{
				if (drawType != 3)
				{
					num5 = this._frame;
					goto IL_4DA;
				}
				if (drawPlayer.itemAnimation <= 0)
				{
					int holdStyle2 = drawPlayer.lastVisualizedSelectedItem.holdStyle;
					num5 = this._frame;
					goto IL_4DA;
				}
				Rectangle bodyFrame2 = drawPlayer.bodyFrame;
				int num6 = bodyFrame2.Y / bodyFrame2.Height;
				int useStyle2 = drawPlayer.lastVisualizedSelectedItem.useStyle;
				num5 = Utils.Clamp<int>(num6, 1, 4);
				if (num5 == 3 || num6 == 0 || useStyle2 == 13)
				{
					num5 = this._frame;
				}
				if (useStyle2 == 12 && drawPlayer.itemAnimation > drawPlayer.itemAnimationMax / 2)
				{
					num5 = 3;
					goto IL_4DA;
				}
				goto IL_4DA;
			}
			num5 = this._frame;
			IL_4DA:
			int num7 = num4 / num3;
			Rectangle rectangle = new Rectangle(0, num7 * num5, this._data.textureWidth, num7);
			if (flag)
			{
				rectangle.Height -= 2;
			}
			type = this._type;
			if (type <= 7)
			{
				if (type != 0)
				{
					if (type == 7)
					{
						if (drawType == 3)
						{
							drawColor = new Color(250, 250, 250, 255) * drawPlayer.stealth * (1f - shadow);
						}
					}
				}
				else if (drawType == 3)
				{
					drawColor = Color.White;
				}
			}
			else if (type != 9)
			{
				if (type == 61)
				{
					drawColor = new Color(drawColor.ToVector4() * 0.5f + new Vector4(0.5f));
					if (drawType == 3)
					{
						drawColor = Projectile.GetFairyQueenWeaponsColorFull(drawPlayer.whoAmI, drawPlayer.Center, 0.41f, 1f, 0.15f, 1f, 0.7f);
						drawColor.A = (byte)((float)drawColor.A * 0.65f);
					}
					drawColor *= drawPlayer.stealth * (1f - shadow);
				}
			}
			else if (drawType == 3)
			{
				if (this._abilityCharge == 0)
				{
					return;
				}
				drawColor = Color.Multiply(Color.White, (float)this._abilityCharge / (float)this._data.abilityChargeMax);
				drawColor.A = 0;
			}
			Color color = new Color(drawColor.ToVector4() * 0.25f + new Vector4(0.75f));
			type = this._type;
			if (type <= 12)
			{
				if (type != 11)
				{
					if (type == 12)
					{
						if (drawType == 0)
						{
							float scale = MathHelper.Clamp(drawPlayer.MountFishronSpecialCounter / 60f, 0f, 1f);
							color = Colors.CurrentLiquidColor;
							if (color == Color.Transparent)
							{
								color = Color.White;
							}
							color.A = 127;
							color *= scale;
						}
					}
				}
				else if (drawType == 2)
				{
					color = Color.White;
					color.A = 127;
				}
			}
			else if (type != 24)
			{
				if (type != 45)
				{
					if (type == 56 && drawType == 2)
					{
						color = Color.White;
						color.A = 0;
					}
				}
				else if (drawType == 2)
				{
					color = new Color(150, 110, 110, 100);
				}
			}
			else if (drawType == 2)
			{
				color = Color.SkyBlue * 0.5f;
				color.A = 20;
			}
			float num8 = 0f;
			type = this._type;
			if (type != 7)
			{
				if (type == 8)
				{
					Mount.DrillMountData drillMountData = (Mount.DrillMountData)this._mountSpecificData;
					if (drawType == 0)
					{
						num8 = drillMountData.outerRingRotation - num8;
					}
					else if (drawType == 3)
					{
						num8 = drillMountData.diodeRotation - num8 - drawPlayer.fullRotation;
					}
				}
			}
			else
			{
				num8 = drawPlayer.fullRotation;
			}
			Vector2 origin = this.Origin;
			type = this._type;
			float scale2 = 1f;
			type = this._type;
			SpriteEffects spriteEffects;
			if (type != 7)
			{
				if (type != 8)
				{
					spriteEffects = playerEffect;
				}
				else
				{
					spriteEffects = ((drawPlayer.direction == 1 && drawType == 2) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
				}
			}
			else
			{
				spriteEffects = SpriteEffects.None;
			}
			if (this.Cart)
			{
				spriteEffects = ((Math.Sign(drawPlayer.velocity.X) == -drawPlayer.direction) ? (playerEffect ^ SpriteEffects.FlipHorizontally) : playerEffect);
			}
			bool flag2 = false;
			type = this._type;
			if (type <= 35)
			{
				if (type != 8)
				{
					if (type == 35)
					{
						if (drawType == 2)
						{
							Mount.ExtraFrameMountData extraFrameMountData = (Mount.ExtraFrameMountData)this._mountSpecificData;
							int num9 = -36;
							if ((spriteEffects & SpriteEffects.FlipHorizontally) != SpriteEffects.None)
							{
								num9 *= -1;
							}
							Vector2 value2 = new Vector2((float)num9, -26f);
							if (shadow == 0f)
							{
								if (Math.Abs(drawPlayer.velocity.X) > 1f)
								{
									extraFrameMountData.frameCounter += Math.Min(2f, Math.Abs(drawPlayer.velocity.X * 0.4f));
									while (extraFrameMountData.frameCounter > 6f)
									{
										extraFrameMountData.frameCounter -= 6f;
										extraFrameMountData.frame++;
										if ((extraFrameMountData.frame > 2 && extraFrameMountData.frame < 5) || extraFrameMountData.frame > 7)
										{
											extraFrameMountData.frame = 0;
										}
									}
								}
								else
								{
									extraFrameMountData.frameCounter += 1f;
									while (extraFrameMountData.frameCounter > 6f)
									{
										extraFrameMountData.frameCounter -= 6f;
										extraFrameMountData.frame++;
										if (extraFrameMountData.frame > 5)
										{
											extraFrameMountData.frame = 5;
										}
									}
								}
							}
							Texture2D value3 = TextureAssets.Extra[142].Value;
							Rectangle value4 = value3.Frame(1, 8, 0, extraFrameMountData.frame, 0, 0);
							if (flag)
							{
								value4.Height -= 2;
							}
							DrawData item = new DrawData(value3, Position + value2, new Rectangle?(value4), drawColor, num8, origin, scale2, spriteEffects, 0f)
							{
								shader = Mount.currentShader
							};
							playerDrawData.Add(item);
						}
					}
				}
			}
			else if (type != 38)
			{
				if (type == 50 && drawType == 0)
				{
					Vector2 position = Position + new Vector2(0f, (float)(8 - this.PlayerOffset + 20));
					Rectangle value5 = new Rectangle(0, num7 * this._frameExtra, this._data.textureWidth, num7);
					if (flag)
					{
						value5.Height -= 2;
					}
					DrawData item = new DrawData(TextureAssets.Extra[207].Value, position, new Rectangle?(value5), drawColor, num8, origin, scale2, spriteEffects, 0f)
					{
						shader = Mount.currentShader
					};
					playerDrawData.Add(item);
				}
			}
			else if (drawType == 0)
			{
				int num10 = 0;
				if ((spriteEffects & SpriteEffects.FlipHorizontally) != SpriteEffects.None)
				{
					num10 = 22;
				}
				Vector2 value6 = new Vector2((float)num10, -10f);
				Texture2D value7 = TextureAssets.Extra[151].Value;
				Rectangle value8 = value7.Frame(1, 1, 0, 0, 0, 0);
				DrawData item = new DrawData(value7, Position + value6, new Rectangle?(value8), drawColor, num8, origin, scale2, spriteEffects, 0f)
				{
					shader = Mount.currentShader
				};
				playerDrawData.Add(item);
				flag2 = true;
			}
			if (!flag2)
			{
				DrawData item = new DrawData(texture2D, Position, new Rectangle?(rectangle), drawColor, num8, origin, scale2, spriteEffects, 0f)
				{
					shader = Mount.currentShader
				};
				playerDrawData.Add(item);
				if (texture2D2 != null)
				{
					item = new DrawData(texture2D2, Position, new Rectangle?(rectangle), color * ((float)drawColor.A / 255f), num8, origin, scale2, spriteEffects, 0f);
					item.shader = Mount.currentShader;
				}
				playerDrawData.Add(item);
			}
			type = this._type;
			if (type <= 17)
			{
				if (type != 8)
				{
					if (type == 17)
					{
						if (drawType == 1 && Mount.ShouldGolfCartEmitLight())
						{
							rectangle = new Rectangle(0, num7 * 3, this._data.textureWidth, num7);
							if (flag)
							{
								rectangle.Height -= 2;
							}
							drawColor = Color.White * 1f;
							drawColor.A = 0;
							DrawData item = new DrawData(texture2D, Position, new Rectangle?(rectangle), drawColor, num8, origin, scale2, spriteEffects, 0f)
							{
								shader = Mount.currentShader
							};
							playerDrawData.Add(item);
						}
					}
				}
				else if (drawType == 3)
				{
					Mount.DrillMountData drillMountData2 = (Mount.DrillMountData)this._mountSpecificData;
					Rectangle value9 = new Rectangle(0, 0, 1, 1);
					Vector2 vector = Mount.drillDiodePoint1.RotatedBy((double)drillMountData2.diodeRotation, default(Vector2));
					Vector2 vector2 = Mount.drillDiodePoint2.RotatedBy((double)drillMountData2.diodeRotation, default(Vector2));
					for (int i = 0; i < drillMountData2.beams.Length; i++)
					{
						Mount.DrillBeam drillBeam = drillMountData2.beams[i];
						if (!(drillBeam.curTileTarget == Point16.NegativeOne))
						{
							for (int j = 0; j < 2; j++)
							{
								Vector2 value10 = new Vector2((float)(drillBeam.curTileTarget.X * 16 + 8), (float)(drillBeam.curTileTarget.Y * 16 + 8)) - Main.screenPosition - Position;
								Vector2 vector3;
								Color color2;
								if (j == 0)
								{
									vector3 = vector;
									color2 = Color.CornflowerBlue;
								}
								else
								{
									vector3 = vector2;
									color2 = Color.LightGreen;
								}
								color2.A = 128;
								color2 *= 0.5f;
								Vector2 v = value10 - vector3;
								float num11 = v.ToRotation();
								float y = v.Length();
								Vector2 scale3 = new Vector2(2f, y);
								DrawData item = new DrawData(TextureAssets.MagicPixel.Value, vector3 + Position, new Rectangle?(value9), color2, num11 - 1.5707964f, Vector2.Zero, scale3, SpriteEffects.None, 0f)
								{
									ignorePlayerRotation = true,
									shader = Mount.currentShader
								};
								playerDrawData.Add(item);
							}
						}
					}
				}
			}
			else if (type != 23)
			{
				if (type != 45)
				{
					if (type == 50 && drawType == 0)
					{
						texture2D = TextureAssets.Extra[205].Value;
						DrawData item = new DrawData(texture2D, Position, new Rectangle?(rectangle), drawColor, num8, origin, scale2, spriteEffects, 0f)
						{
							shader = Mount.currentShader
						};
						playerDrawData.Add(item);
						Vector2 position2 = Position + new Vector2(0f, (float)(8 - this.PlayerOffset + 20));
						Rectangle value11 = new Rectangle(0, num7 * this._frameExtra, this._data.textureWidth, num7);
						if (flag)
						{
							value11.Height -= 2;
						}
						texture2D = TextureAssets.Extra[206].Value;
						item = new DrawData(texture2D, position2, new Rectangle?(value11), drawColor, num8, origin, scale2, spriteEffects, 0f)
						{
							shader = Mount.currentShader
						};
						playerDrawData.Add(item);
					}
				}
				else if (drawType == 0 && shadow == 0f)
				{
					if (Math.Abs(drawPlayer.velocity.X) > this.DashSpeed * 0.9f)
					{
						color = new Color(255, 220, 220, 200);
						scale2 = 1.1f;
					}
					for (int k = 0; k < 2; k++)
					{
						Vector2 position3 = Position + new Vector2((float)Main.rand.Next(-10, 11) * 0.1f, (float)Main.rand.Next(-10, 11) * 0.1f);
						rectangle = new Rectangle(0, num7 * 3, this._data.textureWidth, num7);
						if (flag)
						{
							rectangle.Height -= 2;
						}
						DrawData item = new DrawData(texture2D2, position3, new Rectangle?(rectangle), color, num8, origin, scale2, spriteEffects, 0f)
						{
							shader = Mount.currentShader
						};
						playerDrawData.Add(item);
					}
				}
			}
			else if (drawType == 0)
			{
				texture2D = TextureAssets.Extra[114].Value;
				rectangle = texture2D.Frame(2, 1, 0, 0, 0, 0);
				int width = rectangle.Width;
				rectangle.Width -= 2;
				float witchBroomTrinketRotation = this.GetWitchBroomTrinketRotation(drawPlayer);
				Vector2 vector4 = Position + this.GetWitchBroomTrinketOriginOffset(drawPlayer);
				num8 = witchBroomTrinketRotation;
				origin = new Vector2((float)(rectangle.Width / 2), 0f);
				DrawData item = new DrawData(texture2D, vector4.Floor(), new Rectangle?(rectangle), drawColor, num8, origin, scale2, spriteEffects, 0f)
				{
					shader = Mount.currentShader
				};
				playerDrawData.Add(item);
				Color color3 = new Color(new Vector3(0.9f, 0.85f, 0f));
				color3.A /= 2;
				float num12 = ((float)drawPlayer.miscCounter / 75f * 6.2831855f).ToRotationVector2().X * 1f;
				Color color4 = new Color(80, 70, 40, 0) * (num12 / 8f + 0.5f) * 0.8f;
				rectangle.X += width;
				for (int l = 0; l < 4; l++)
				{
					item = new DrawData(texture2D, (vector4 + ((float)l * 1.5707964f).ToRotationVector2() * num12).Floor(), new Rectangle?(rectangle), color4, num8, origin, scale2, spriteEffects, 0f)
					{
						shader = Mount.currentShader
					};
					playerDrawData.Add(item);
				}
			}
			if (this._type == 62 || this._type == 63)
			{
				this.TryPettingMount(drawPlayer);
			}
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0003AD7C File Offset: 0x00038F7C
		private void TryPettingMount(Player player)
		{
			if (Main.gameMenu || Main.gamePaused)
			{
				return;
			}
			if (Math.Abs(player.velocity.X) >= 1f)
			{
				return;
			}
			Vector2 v = Main.ReverseGravitySupport(Main.MouseScreen, 0f) + Main.screenPosition;
			bool flag = Utils.CenteredRectangle(player.Bottom + new Vector2((float)(player.direction * 26), -54f), new Vector2(24f, 24f)).Contains(v.ToPoint());
			bool flag2 = flag & !player.lastMouseInterface;
			if (!Main.SmartCursorIsUsed)
			{
				bool flag3 = !PlayerInput.UsingGamepad;
			}
			if (!flag2)
			{
				return;
			}
			if (flag)
			{
				player.noThrow = 4;
				player.cursorItemIconEnabled = true;
				player.cursorItemIconID = ((this._type == 63) ? 5666 : 5665);
			}
			if (PlayerInput.UsingGamepad)
			{
				player.GamepadEnableGrappleCooldown();
			}
			if (Main.mouseRight && Main.mouseRightRelease && Player.BlockInteractionWithProjectiles == 0)
			{
				Main.mouseRightRelease = false;
				player.tileInteractAttempted = true;
				player.tileInteractionHappened = true;
				player.releaseUseTile = false;
				player.PetMount(new PlayerPettingInfo(this._type, false));
				EmoteBubble.NewBubble(0, new WorldUIAnchor(Main.LocalPlayer), 60);
			}
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0003AEBC File Offset: 0x000390BC
		public Mount.DismountCheckResult TryDismountWithResult(Player mountedPlayer)
		{
			Mount.DismountCheckResult dismountCheckResult = this.CanDismountWithResult(mountedPlayer);
			if (dismountCheckResult == Mount.DismountCheckResult.Succeeded)
			{
				this.Dismount(mountedPlayer, false);
			}
			return dismountCheckResult;
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0003AED0 File Offset: 0x000390D0
		public bool TryDismount(Player mountedPlayer)
		{
			return this.TryDismountWithResult(mountedPlayer) == Mount.DismountCheckResult.Succeeded;
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0003AEDC File Offset: 0x000390DC
		public void Dismount(Player mountedPlayer, bool ignoreEffect = false)
		{
			if (!this._active)
			{
				return;
			}
			bool cart = this.Cart;
			this._active = false;
			mountedPlayer.ClearBuff(this._data.buff);
			this._mountSpecificData = null;
			int type = this._type;
			if (type - 55 <= 1 && mountedPlayer.noItems && !mountedPlayer.cursed)
			{
				mountedPlayer.noItems = false;
			}
			if (cart)
			{
				mountedPlayer.cartFlip = false;
				mountedPlayer.lastBoost = Vector2.Zero;
			}
			mountedPlayer.fullRotation = 0f;
			mountedPlayer.fullRotationOrigin = Vector2.Zero;
			if (mountedPlayer.petting.isPetting && mountedPlayer.petting.mount)
			{
				mountedPlayer.StopPettingAnimal();
			}
			if (!ignoreEffect)
			{
				this.DoSpawnDust(mountedPlayer, true);
			}
			this.Reset();
			if (!mountedPlayer.isDisplayDollOrInanimate && mountedPlayer.whoAmI == Main.myPlayer)
			{
				NetMessage.SendData(13, -1, -1, null, mountedPlayer.whoAmI, 0f, 0f, 0f, 0, 0, 0);
			}
			Rectangle r;
			if (Collision.TryChangingSizeFromBottomCenter(mountedPlayer.Hitbox, 20, 42, out r))
			{
				Vector2 value = r.TopLeft() - mountedPlayer.position;
				mountedPlayer.position += value;
				mountedPlayer.width = 20;
				mountedPlayer.height = 42;
				for (int i = 0; i < mountedPlayer.shadowPos.Length; i++)
				{
					mountedPlayer.shadowPos[i] += value;
				}
				return;
			}
			mountedPlayer.position.Y = mountedPlayer.position.Y + (float)mountedPlayer.height;
			mountedPlayer.width = 20;
			mountedPlayer.height = 42;
			mountedPlayer.position.Y = mountedPlayer.position.Y - (float)mountedPlayer.height;
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0003B08C File Offset: 0x0003928C
		public void SetMount(int m, Player mountedPlayer)
		{
			if (this._type == m || m <= -1 || m >= MountID.Count)
			{
				return;
			}
			if (m == 5 && mountedPlayer.wet)
			{
				return;
			}
			if (this._active)
			{
				mountedPlayer.ClearBuff(this._data.buff);
				if (this.AnyTrackRider)
				{
					mountedPlayer.cartFlip = false;
					mountedPlayer.lastBoost = Vector2.Zero;
				}
				mountedPlayer.fullRotation = 0f;
				mountedPlayer.fullRotationOrigin = Vector2.Zero;
				this._mountSpecificData = null;
			}
			else
			{
				this._active = true;
			}
			this._flyTime = 0;
			this._type = m;
			this._data = Mount.mounts[m];
			this._fatigueMax = (float)this._data.fatigueMax;
			if (!mountedPlayer.isDisplayDollOrInanimate && mountedPlayer.whoAmI == Main.myPlayer)
			{
				NetMessage.SendData(13, -1, -1, null, mountedPlayer.whoAmI, 0f, 0f, 0f, 0, 0, 0);
			}
			mountedPlayer.AddBuff(this._data.buff, 3600, false);
			this._flipDraw = false;
			if (this._type == 44)
			{
				mountedPlayer.velocity *= 0.2f;
				mountedPlayer.dash = 0;
				mountedPlayer.dashType = 0;
				mountedPlayer.dashDelay = 0;
				mountedPlayer.dashTime = 0;
			}
			if (this._type == 9 && this._abilityCooldown < 20)
			{
				this._abilityCooldown = 20;
			}
			if (this._type == 46 && this._abilityCooldown < 40)
			{
				this._abilityCooldown = 40;
			}
			Mount.MountDelegatesData.OverrideSizeMethod playerSize = this._data.delegations.PlayerSize;
			Vector2? vector;
			if (playerSize != null && playerSize(mountedPlayer, out vector) && vector != null)
			{
				Vector2 value = vector.Value;
				Vector2 bottom = mountedPlayer.Bottom;
				mountedPlayer.position = mountedPlayer.Bottom;
				for (int i = 0; i < mountedPlayer.shadowPos.Length; i++)
				{
					Vector2[] shadowPos = mountedPlayer.shadowPos;
					int num = i;
					shadowPos[num].X = shadowPos[num].X + (float)(mountedPlayer.width / 2);
					Vector2[] shadowPos2 = mountedPlayer.shadowPos;
					int num2 = i;
					shadowPos2[num2].Y = shadowPos2[num2].Y + (float)mountedPlayer.height;
				}
				mountedPlayer.width = (int)value.X;
				mountedPlayer.height = (int)value.Y;
				mountedPlayer.position = new Vector2(bottom.X - value.X / 2f, bottom.Y - (float)mountedPlayer.height);
				for (int j = 0; j < mountedPlayer.shadowPos.Length; j++)
				{
					Vector2[] shadowPos3 = mountedPlayer.shadowPos;
					int num3 = j;
					shadowPos3[num3].X = shadowPos3[num3].X - (float)(mountedPlayer.width / 2);
					Vector2[] shadowPos4 = mountedPlayer.shadowPos;
					int num4 = j;
					shadowPos4[num4].Y = shadowPos4[num4].Y - (float)mountedPlayer.height;
				}
			}
			else
			{
				mountedPlayer.position.Y = mountedPlayer.position.Y + (float)mountedPlayer.height;
				for (int k = 0; k < mountedPlayer.shadowPos.Length; k++)
				{
					Vector2[] shadowPos5 = mountedPlayer.shadowPos;
					int num5 = k;
					shadowPos5[num5].Y = shadowPos5[num5].Y + (float)mountedPlayer.height;
				}
				mountedPlayer.height = 42 + this._data.heightBoost;
				mountedPlayer.position.Y = mountedPlayer.position.Y - (float)mountedPlayer.height;
				for (int l = 0; l < mountedPlayer.shadowPos.Length; l++)
				{
					Vector2[] shadowPos6 = mountedPlayer.shadowPos;
					int num6 = l;
					shadowPos6[num6].Y = shadowPos6[num6].Y - (float)mountedPlayer.height;
				}
			}
			mountedPlayer.ResetAdvancedShadows();
			if (this._type == 7 || this._type == 8)
			{
				mountedPlayer.fullRotationOrigin = new Vector2((float)(mountedPlayer.width / 2), (float)(mountedPlayer.height / 2));
			}
			int type = this._type;
			if (type - 62 <= 1)
			{
				SoundEngine.PlaySound(SoundID.PalChillet, mountedPlayer.Center, 0f, 1f);
			}
			if (this._type == 8)
			{
				this._mountSpecificData = new Mount.DrillMountData();
			}
			if (this._type == 35)
			{
				this._mountSpecificData = new Mount.ExtraFrameMountData();
			}
			if (this._type == 54)
			{
				this._mountSpecificData = new Mount.SelectiveFlyingMountData();
			}
			this.DoSpawnDust(mountedPlayer, false);
			if (this._type == 38 && mountedPlayer.whoAmI == Main.myPlayer)
			{
				AchievementsHelper.NotifyProgressionEvent(32);
			}
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0003B4B4 File Offset: 0x000396B4
		public void DoFailedDismountDust(Player mountedPlayer, int dustAmount)
		{
			if (Main.netMode == 2)
			{
				return;
			}
			Color newColor = Color.Transparent;
			if (this._type == 56)
			{
				newColor = Color.Black;
			}
			if (this._type == 61)
			{
				newColor = Projectile.GetFairyQueenWeaponsColorFull(mountedPlayer.whoAmI, mountedPlayer.Center, 0.27f, 1f, 0.15f, 1f, 0.7f);
			}
			Rectangle hitbox = mountedPlayer.Hitbox;
			hitbox.Inflate(10, 10);
			for (int i = 0; i < dustAmount; i++)
			{
				int spawnDust = this._data.spawnDust;
				float scale = 1f;
				int alpha = 0;
				int num = Dust.NewDust(hitbox.TopLeft(), hitbox.Width, hitbox.Height, spawnDust, 0f, 0f, alpha, newColor, scale);
				Main.dust[num].scale += (float)Main.rand.Next(-10, 21) * 0.01f;
				if (this._type == 56)
				{
					Dust dust = Main.dust[num];
					dust.scale = 0.7f;
					dust.velocity *= 0.4f;
					dust.velocity.Y = dust.velocity.Y + mountedPlayer.gravDir * 0.5f;
				}
				if (this._type == 61)
				{
					Dust dust2 = Main.dust[num];
					dust2.scale *= 1f + 0.3f * Main.rand.NextFloat();
					dust2.velocity += mountedPlayer.velocity * Main.rand.NextFloat();
				}
				if (this._data.spawnDustNoGravity)
				{
					Main.dust[num].noGravity = true;
				}
				else if (Main.rand.Next(2) == 0)
				{
					Main.dust[num].scale *= 1.3f;
					Main.dust[num].noGravity = true;
				}
				else
				{
					Main.dust[num].velocity *= 0.5f;
				}
				Main.dust[num].velocity += mountedPlayer.velocity * 0.8f;
			}
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0003B6E0 File Offset: 0x000398E0
		private void DoSpawnDust(Player mountedPlayer, bool isDismounting)
		{
			if (Main.netMode == 2)
			{
				return;
			}
			if (this._type == 52)
			{
				for (int i = 0; i < 100; i++)
				{
					int spawnDust = this._data.spawnDust;
					Dust dust = Dust.NewDustDirect(new Vector2(mountedPlayer.position.X - 20f, mountedPlayer.position.Y), mountedPlayer.width + 40, mountedPlayer.height, 267, 0f, 0f, 60, new Color(130, 60, 255, 70), 1f);
					dust.scale += (float)Main.rand.Next(-10, 21) * 0.01f;
					dust.noGravity = true;
					dust.velocity += mountedPlayer.velocity * 0.8f;
					dust.velocity *= Main.rand.NextFloat();
					Dust dust2 = dust;
					dust2.velocity.Y = dust2.velocity.Y + 2f * Main.rand.NextFloatDirection();
					dust.noLight = true;
					if (Main.rand.Next(3) == 0)
					{
						Dust dust3 = Dust.CloneDust(dust);
						dust3.color = Color.White;
						dust3.scale *= 0.5f;
						dust3.alpha = 0;
					}
				}
				return;
			}
			if (this._type == 62 || this._type == 63)
			{
				for (int j = 0; j < 100; j++)
				{
					int spawnDust2 = this._data.spawnDust;
					Dust dust4 = Dust.NewDustDirect(new Vector2(mountedPlayer.position.X - 20f, mountedPlayer.position.Y), mountedPlayer.width + 40, mountedPlayer.height, 306, 0f, 0f, 60, new Color(100, 227, 255, 127), 2f);
					dust4.scale += (float)Main.rand.Next(-10, 21) * 0.01f;
					dust4.noGravity = true;
					dust4.velocity += mountedPlayer.velocity * 0.8f;
					dust4.velocity *= Main.rand.NextFloat();
					Dust dust5 = dust4;
					dust5.velocity.Y = dust5.velocity.Y + 4f * Main.rand.NextFloatDirection();
					dust4.noLight = true;
					if (Main.rand.Next(3) == 0)
					{
						Dust dust6 = Dust.CloneDust(dust4);
						dust6.color = Color.White;
						dust6.scale *= 0.5f;
						dust6.alpha = 0;
					}
				}
				return;
			}
			Color newColor = Color.Transparent;
			if (this._type == 23)
			{
				newColor = new Color(255, 255, 0, 255);
			}
			if (this._type == 56)
			{
				newColor = Color.Black;
			}
			if (this._type == 61)
			{
				newColor = Projectile.GetFairyQueenWeaponsColorFull(mountedPlayer.whoAmI, mountedPlayer.Center, 0.27f, 1f, 0.15f, 1f, 0.7f);
			}
			int num = 100;
			if (this._type > 0 && MountID.Sets.IsRollerSkates[this._type])
			{
				num = 40;
			}
			Rectangle hitbox = new Rectangle((int)mountedPlayer.position.X - 20, (int)mountedPlayer.position.Y, mountedPlayer.width + 40, mountedPlayer.height);
			if (this._type == 56 || this._type == 61)
			{
				if (isDismounting)
				{
					Vector2 bottom = mountedPlayer.Bottom;
					hitbox = new Rectangle((int)bottom.X - (int)mountedPlayer.DefaultSize.X / 2, (int)bottom.Y - (int)mountedPlayer.DefaultSize.Y, (int)mountedPlayer.DefaultSize.X, (int)mountedPlayer.DefaultSize.Y);
				}
				else
				{
					hitbox = mountedPlayer.Hitbox;
				}
				hitbox.Inflate(10, 10);
			}
			if (this._type > 0 && MountID.Sets.IsRollerSkates[this._type])
			{
				int num2 = 10;
				int num3 = mountedPlayer.width / 2;
				int num4 = 10;
				hitbox = new Rectangle((int)mountedPlayer.Center.X - num3 - num2, (int)mountedPlayer.Bottom.Y - num4, Math.Max(1, num3 + num2 * 2), num4);
			}
			for (int k = 0; k < num; k++)
			{
				if (MountID.Sets.Cart[this._type])
				{
					if (k % 10 == 0)
					{
						int type = Main.rand.Next(61, 64);
						int num5 = Gore.NewGore(new Vector2(mountedPlayer.position.X - 20f, mountedPlayer.position.Y), Vector2.Zero, type, 1f);
						Main.gore[num5].alpha = 100;
						Main.gore[num5].velocity = Vector2.Transform(new Vector2(1f, 0f), Matrix.CreateRotationZ((float)(Main.rand.NextDouble() * 6.2831854820251465)));
					}
				}
				else
				{
					int type2 = this._data.spawnDust;
					float scale = 1f;
					int alpha = 0;
					if (this._type == 40 || this._type == 41 || this._type == 42)
					{
						if (Main.rand.Next(2) == 0)
						{
							type2 = 31;
						}
						else
						{
							type2 = 16;
						}
						scale = 0.9f;
						alpha = 50;
						if (this._type == 42)
						{
							type2 = 31;
						}
						if (this._type == 41)
						{
							type2 = 16;
						}
					}
					if (this._type != 61 || Main.rand.Next(isDismounting ? 2 : 5) == 0)
					{
						int num6 = Dust.NewDust(hitbox.TopLeft(), hitbox.Width, hitbox.Height, type2, 0f, 0f, alpha, newColor, scale);
						Main.dust[num6].scale += (float)Main.rand.Next(-10, 21) * 0.01f;
						if (this._type > 0 && MountID.Sets.IsRollerSkates[this._type])
						{
							Dust dust7 = Main.dust[num6];
							dust7.velocity *= 0.8f;
							dust7.velocity.Y = dust7.velocity.Y * 0.75f;
							dust7.velocity.Y = dust7.velocity.Y + mountedPlayer.gravDir * 0.25f;
							dust7.noLightEmittence = true;
							dust7.noGravity = true;
						}
						if (this._type == 56)
						{
							Dust dust8 = Main.dust[num6];
							dust8.scale = 0.7f;
							dust8.velocity *= 0.4f;
							dust8.velocity.Y = dust8.velocity.Y + mountedPlayer.gravDir * 0.5f;
						}
						if (this._type == 61)
						{
							Dust dust9 = Main.dust[num6];
							dust9.scale *= 1f + 0.3f * Main.rand.NextFloat();
							if (!isDismounting)
							{
								dust9.position = Vector2.Lerp(dust9.position, hitbox.Center.ToVector2(), 0.7f);
								dust9.velocity *= -2f;
								Dust dust10 = dust9;
								dust10.velocity.Y = dust10.velocity.Y - 1f;
							}
							dust9.velocity += mountedPlayer.velocity * Main.rand.NextFloat();
						}
						if (this._data.spawnDustNoGravity)
						{
							Main.dust[num6].noGravity = true;
						}
						else if (Main.rand.Next(2) == 0)
						{
							Main.dust[num6].scale *= 1.3f;
							Main.dust[num6].noGravity = true;
						}
						else
						{
							Main.dust[num6].velocity *= 0.5f;
						}
						Main.dust[num6].velocity += mountedPlayer.velocity * 0.8f;
						if (this._type == 40 || this._type == 41 || this._type == 42)
						{
							Main.dust[num6].velocity *= Main.rand.NextFloat();
						}
					}
				}
			}
			if (this._type == 40 || this._type == 41 || this._type == 42)
			{
				for (int l = 0; l < 5; l++)
				{
					int type3 = Main.rand.Next(61, 64);
					if (this._type == 41 || (this._type == 40 && Main.rand.Next(2) == 0))
					{
						type3 = Main.rand.Next(11, 14);
					}
					int num7 = Gore.NewGore(new Vector2(mountedPlayer.position.X + (float)(mountedPlayer.direction * 8), mountedPlayer.position.Y + 20f), Vector2.Zero, type3, 1f);
					Main.gore[num7].alpha = 100;
					Main.gore[num7].velocity = Vector2.Transform(new Vector2(1f, 0f), Matrix.CreateRotationZ((float)(Main.rand.NextDouble() * 6.2831854820251465))) * 1.4f;
				}
			}
			if (this._type == 23)
			{
				for (int m = 0; m < 4; m++)
				{
					int type4 = Main.rand.Next(61, 64);
					int num8 = Gore.NewGore(new Vector2(mountedPlayer.position.X - 20f, mountedPlayer.position.Y), Vector2.Zero, type4, 1f);
					Main.gore[num8].alpha = 100;
					Main.gore[num8].velocity = Vector2.Transform(new Vector2(1f, 0f), Matrix.CreateRotationZ((float)(Main.rand.NextDouble() * 6.2831854820251465)));
				}
			}
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0003C0F6 File Offset: 0x0003A2F6
		public static bool DismountsOnItemUse(int mountType)
		{
			return Mount.mounts[mountType].dismountsOnItemUse;
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0003C104 File Offset: 0x0003A304
		public bool CanVisuallyHoldItem(Player mountedPlayer, Item item)
		{
			return this.Type < 0 || !MountID.Sets.DontHoldItems[this.Type];
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0003C120 File Offset: 0x0003A320
		public bool CanMount(int m, Player mountingPlayer)
		{
			Mount.MountData mountData = Mount.mounts[m];
			Vector2 vector = mountingPlayer.Size + new Vector2(0f, (float)Mount.mounts[m].heightBoost);
			Mount.MountDelegatesData.OverrideSizeMethod playerSize = mountData.delegations.PlayerSize;
			Vector2? vector2;
			if (playerSize != null && playerSize(mountingPlayer, out vector2) && vector2 != null)
			{
				vector.X = (float)((int)vector2.Value.X);
				vector.Y = (float)((int)vector2.Value.Y);
			}
			return mountingPlayer.CanFitInSpaceWithSize(vector, default(Vector2)) && (m != 56 || (!mountingPlayer.wet && !mountingPlayer.dripping && !Collision.WetCollision(mountingPlayer.Bottom - vector * 0.5f, (int)vector.X, (int)vector.Y))) && (MountID.Sets.Cart[m] || MountID.Sets.CanUseHooks[m] || mountingPlayer.grappling[0] < 0);
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0003C221 File Offset: 0x0003A421
		public bool CanDismount(Player mountingPlayer)
		{
			return this.CanDismountWithResult(mountingPlayer) == Mount.DismountCheckResult.Succeeded;
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0003C230 File Offset: 0x0003A430
		public Mount.DismountCheckResult CanDismountWithResult(Player mountingPlayer)
		{
			Mount.DismountCheckResult result = Mount.DismountCheckResult.Succeeded;
			Rectangle rectangle;
			if (MountID.Sets.DontDismountWhenCCed[this.Type] && mountingPlayer.CCed)
			{
				result = Mount.DismountCheckResult.FailedCCed;
			}
			else if (!Collision.TryChangingSizeFromBottomCenter(mountingPlayer.Hitbox, 20, 42, out rectangle))
			{
				result = Mount.DismountCheckResult.FailedNoSpace;
			}
			return result;
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060002A2 RID: 674 RVA: 0x0003C26F File Offset: 0x0003A46F
		public bool DismountOnItemUse
		{
			get
			{
				return this.Active && this._data.dismountsOnItemUse;
			}
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0003C286 File Offset: 0x0003A486
		public void TryEarlyDismount(Player player)
		{
			if (!this.Active)
			{
				return;
			}
			if (!this._data.dismountsOnItemUse)
			{
				return;
			}
			if (!this.CanDismount(player))
			{
				return;
			}
			this.Dismount(player, false);
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0003C2B4 File Offset: 0x0003A4B4
		public bool FindTileHeight(Vector2 position, int maxTilesDown, out float tileHeight)
		{
			int num = (int)(position.X / 16f);
			int num2 = (int)(position.Y / 16f);
			for (int i = 0; i <= maxTilesDown; i++)
			{
				Tile tile = Main.tile[num, num2];
				bool flag = Main.tileSolid[(int)tile.type];
				bool flag2 = Main.tileSolidTop[(int)tile.type];
				if (tile.active())
				{
					if (flag)
					{
						if (flag2)
						{
						}
					}
				}
				num2++;
			}
			tileHeight = 0f;
			return true;
		}

		// Token: 0x040001BC RID: 444
		public static int currentShader = 0;

		// Token: 0x040001BD RID: 445
		public const int FrameStanding = 0;

		// Token: 0x040001BE RID: 446
		public const int FrameRunning = 1;

		// Token: 0x040001BF RID: 447
		public const int FrameInAir = 2;

		// Token: 0x040001C0 RID: 448
		public const int FrameFlying = 3;

		// Token: 0x040001C1 RID: 449
		public const int FrameSwimming = 4;

		// Token: 0x040001C2 RID: 450
		public const int FrameDashing = 5;

		// Token: 0x040001C3 RID: 451
		public const int DrawBack = 0;

		// Token: 0x040001C4 RID: 452
		public const int DrawBackExtra = 1;

		// Token: 0x040001C5 RID: 453
		public const int DrawFront = 2;

		// Token: 0x040001C6 RID: 454
		public const int DrawFrontExtra = 3;

		// Token: 0x040001C7 RID: 455
		private static Mount.MountData[] mounts;

		// Token: 0x040001C8 RID: 456
		private static Vector2[] scutlixEyePositions;

		// Token: 0x040001C9 RID: 457
		private static Vector2 scutlixTextureSize;

		// Token: 0x040001CA RID: 458
		public const int scutlixBaseDamage = 50;

		// Token: 0x040001CB RID: 459
		public static Vector2 drillDiodePoint1 = new Vector2(36f, -6f);

		// Token: 0x040001CC RID: 460
		public static Vector2 drillDiodePoint2 = new Vector2(36f, 8f);

		// Token: 0x040001CD RID: 461
		public static Vector2 drillTextureSize;

		// Token: 0x040001CE RID: 462
		public const int drillTextureWidth = 80;

		// Token: 0x040001CF RID: 463
		public const float drillRotationChange = 0.05235988f;

		// Token: 0x040001D0 RID: 464
		public static int drillPickPower = 210;

		// Token: 0x040001D1 RID: 465
		public static int drillPickTime = 1;

		// Token: 0x040001D2 RID: 466
		public static int amountOfBeamsAtOnce = 2;

		// Token: 0x040001D3 RID: 467
		public const float maxDrillLength = 48f;

		// Token: 0x040001D4 RID: 468
		private static Vector2 santankTextureSize;

		// Token: 0x040001D5 RID: 469
		private Mount.MountData _data;

		// Token: 0x040001D6 RID: 470
		private int _type;

		// Token: 0x040001D7 RID: 471
		private bool _flipDraw;

		// Token: 0x040001D8 RID: 472
		private int _frame;

		// Token: 0x040001D9 RID: 473
		private float _frameCounter;

		// Token: 0x040001DA RID: 474
		private int _frameExtra;

		// Token: 0x040001DB RID: 475
		private float _frameExtraCounter;

		// Token: 0x040001DC RID: 476
		private int _frameState;

		// Token: 0x040001DD RID: 477
		private int _flyTime;

		// Token: 0x040001DE RID: 478
		private int _idleTime;

		// Token: 0x040001DF RID: 479
		private int _idleTimeNext;

		// Token: 0x040001E0 RID: 480
		private float _fatigue;

		// Token: 0x040001E1 RID: 481
		private float _fatigueMax;

		// Token: 0x040001E2 RID: 482
		private bool _abilityCharging;

		// Token: 0x040001E3 RID: 483
		private int _abilityCharge;

		// Token: 0x040001E4 RID: 484
		private int _abilityCooldown;

		// Token: 0x040001E5 RID: 485
		private int _abilityDuration;

		// Token: 0x040001E6 RID: 486
		private bool _abilityActive;

		// Token: 0x040001E7 RID: 487
		private bool _aiming;

		// Token: 0x040001E8 RID: 488
		private bool _shouldSuperCart;

		// Token: 0x040001E9 RID: 489
		private int _walkingGraceTimeLeft;

		// Token: 0x040001EA RID: 490
		public List<DrillDebugDraw> _debugDraw;

		// Token: 0x040001EB RID: 491
		private object _mountSpecificData;

		// Token: 0x040001EC RID: 492
		private bool _active;

		// Token: 0x040001ED RID: 493
		public static float SuperCartRunSpeed = 20f;

		// Token: 0x040001EE RID: 494
		public static float SuperCartDashSpeed = 20f;

		// Token: 0x040001EF RID: 495
		public static float SuperCartAcceleration = 0.1f;

		// Token: 0x040001F0 RID: 496
		public static int SuperCartJumpHeight = 15;

		// Token: 0x040001F1 RID: 497
		public static float SuperCartJumpSpeed = 5.15f;

		// Token: 0x040001F2 RID: 498
		private Mount.MountDelegatesData _defaultDelegatesData = new Mount.MountDelegatesData();

		// Token: 0x040001F3 RID: 499
		public static int[] idleFrames_Rat = new int[]
		{
			0,
			1,
			3,
			2,
			3,
			2,
			3,
			2,
			1,
			0,
			0
		};

		// Token: 0x020005FA RID: 1530
		private class DrillBeam
		{
			// Token: 0x06003B97 RID: 15255 RVA: 0x00659C92 File Offset: 0x00657E92
			public DrillBeam()
			{
				this.curTileTarget = Point16.NegativeOne;
				this.cooldown = 0;
				this.lastPurpose = 0;
			}

			// Token: 0x0400639E RID: 25502
			public Point16 curTileTarget;

			// Token: 0x0400639F RID: 25503
			public int cooldown;

			// Token: 0x040063A0 RID: 25504
			public int lastPurpose;
		}

		// Token: 0x020005FB RID: 1531
		private class DrillMountData
		{
			// Token: 0x06003B98 RID: 15256 RVA: 0x00659CB4 File Offset: 0x00657EB4
			public DrillMountData()
			{
				this.beams = new Mount.DrillBeam[8];
				for (int i = 0; i < this.beams.Length; i++)
				{
					this.beams[i] = new Mount.DrillBeam();
				}
			}

			// Token: 0x040063A1 RID: 25505
			public float diodeRotationTarget;

			// Token: 0x040063A2 RID: 25506
			public float diodeRotation;

			// Token: 0x040063A3 RID: 25507
			public float outerRingRotation;

			// Token: 0x040063A4 RID: 25508
			public Mount.DrillBeam[] beams;

			// Token: 0x040063A5 RID: 25509
			public int beamCooldown;

			// Token: 0x040063A6 RID: 25510
			public Vector2 crosshairPosition;
		}

		// Token: 0x020005FC RID: 1532
		private class BooleanMountData
		{
			// Token: 0x06003B99 RID: 15257 RVA: 0x00659CF3 File Offset: 0x00657EF3
			public BooleanMountData()
			{
				this.boolean = false;
			}

			// Token: 0x040063A7 RID: 25511
			public bool boolean;
		}

		// Token: 0x020005FD RID: 1533
		private class SelectiveFlyingMountData
		{
			// Token: 0x06003B9A RID: 15258 RVA: 0x00659D02 File Offset: 0x00657F02
			public SelectiveFlyingMountData()
			{
				this.showFlyingFrames = false;
				this.allowedToFly = false;
			}

			// Token: 0x040063A8 RID: 25512
			public bool showFlyingFrames;

			// Token: 0x040063A9 RID: 25513
			public bool allowedToFly;
		}

		// Token: 0x020005FE RID: 1534
		private class ExtraFrameMountData
		{
			// Token: 0x06003B9B RID: 15259 RVA: 0x00659D18 File Offset: 0x00657F18
			public ExtraFrameMountData()
			{
				this.frame = 0;
				this.frameCounter = 0f;
			}

			// Token: 0x040063AA RID: 25514
			public int frame;

			// Token: 0x040063AB RID: 25515
			public float frameCounter;
		}

		// Token: 0x020005FF RID: 1535
		public class MountDelegatesData
		{
			// Token: 0x06003B9C RID: 15260 RVA: 0x00659D34 File Offset: 0x00657F34
			public MountDelegatesData()
			{
				this.MinecartDust = new Action<Vector2>(DelegateMethods.Minecart.Sparks);
				this.MinecartJumpingSound = new Action<Player, Vector2, int, int>(DelegateMethods.Minecart.JumpingSound);
				this.MinecartLandingSound = new Action<Player, Vector2, int, int>(DelegateMethods.Minecart.LandingSound);
				this.MinecartBumperSound = new Action<Player, Vector2, int, int>(DelegateMethods.Minecart.BumperSound);
			}

			// Token: 0x040063AC RID: 25516
			public Action<Vector2> MinecartDust;

			// Token: 0x040063AD RID: 25517
			public Action<Player, Vector2, int, int> MinecartJumpingSound;

			// Token: 0x040063AE RID: 25518
			public Action<Player, Vector2, int, int> MinecartLandingSound;

			// Token: 0x040063AF RID: 25519
			public Action<Player, Vector2, int, int> MinecartBumperSound;

			// Token: 0x040063B0 RID: 25520
			public Mount.MountDelegatesData.OverridePositionMethod MouthPosition;

			// Token: 0x040063B1 RID: 25521
			public Mount.MountDelegatesData.OverridePositionMethod HandPosition;

			// Token: 0x040063B2 RID: 25522
			public Mount.MountDelegatesData.OverrideSizeMethod PlayerSize;

			// Token: 0x040063B3 RID: 25523
			public Mount.MountDelegatesData.AdjustDashDustMethod DashDust;

			// Token: 0x02000A30 RID: 2608
			// (Invoke) Token: 0x06004A5F RID: 19039
			public delegate bool OverridePositionMethod(Player player, out Vector2? position);

			// Token: 0x02000A31 RID: 2609
			// (Invoke) Token: 0x06004A63 RID: 19043
			public delegate bool OverrideSizeMethod(Player player, out Vector2? size);

			// Token: 0x02000A32 RID: 2610
			// (Invoke) Token: 0x06004A67 RID: 19047
			public delegate Dust AdjustDashDustMethod(Player player, int currentDustCount, Dust dust);
		}

		// Token: 0x02000600 RID: 1536
		public class MountData
		{
			// Token: 0x040063B4 RID: 25524
			public Asset<Texture2D> backTexture = Asset<Texture2D>.Empty;

			// Token: 0x040063B5 RID: 25525
			public Asset<Texture2D> backTextureGlow = Asset<Texture2D>.Empty;

			// Token: 0x040063B6 RID: 25526
			public Asset<Texture2D> backTextureExtra = Asset<Texture2D>.Empty;

			// Token: 0x040063B7 RID: 25527
			public Asset<Texture2D> backTextureExtraGlow = Asset<Texture2D>.Empty;

			// Token: 0x040063B8 RID: 25528
			public Asset<Texture2D> frontTexture = Asset<Texture2D>.Empty;

			// Token: 0x040063B9 RID: 25529
			public Asset<Texture2D> frontTextureGlow = Asset<Texture2D>.Empty;

			// Token: 0x040063BA RID: 25530
			public Asset<Texture2D> frontTextureExtra = Asset<Texture2D>.Empty;

			// Token: 0x040063BB RID: 25531
			public Asset<Texture2D> frontTextureExtraGlow = Asset<Texture2D>.Empty;

			// Token: 0x040063BC RID: 25532
			public int textureWidth;

			// Token: 0x040063BD RID: 25533
			public int textureHeight;

			// Token: 0x040063BE RID: 25534
			public int xOffset;

			// Token: 0x040063BF RID: 25535
			public int yOffset;

			// Token: 0x040063C0 RID: 25536
			public int[] playerYOffsets;

			// Token: 0x040063C1 RID: 25537
			public int bodyFrame;

			// Token: 0x040063C2 RID: 25538
			public int playerHeadOffset;

			// Token: 0x040063C3 RID: 25539
			public int heightBoost;

			// Token: 0x040063C4 RID: 25540
			public int buff;

			// Token: 0x040063C5 RID: 25541
			public int flightTimeMax;

			// Token: 0x040063C6 RID: 25542
			public bool usesHover;

			// Token: 0x040063C7 RID: 25543
			public float runSpeed;

			// Token: 0x040063C8 RID: 25544
			public float dashSpeed;

			// Token: 0x040063C9 RID: 25545
			public float swimSpeed;

			// Token: 0x040063CA RID: 25546
			public float acceleration;

			// Token: 0x040063CB RID: 25547
			public float jumpSpeed;

			// Token: 0x040063CC RID: 25548
			public int jumpHeight;

			// Token: 0x040063CD RID: 25549
			public float fallDamage;

			// Token: 0x040063CE RID: 25550
			public int extraFall;

			// Token: 0x040063CF RID: 25551
			public int fatigueMax;

			// Token: 0x040063D0 RID: 25552
			public bool constantJump;

			// Token: 0x040063D1 RID: 25553
			public bool blockExtraJumps;

			// Token: 0x040063D2 RID: 25554
			public int abilityChargeMax;

			// Token: 0x040063D3 RID: 25555
			public int abilityDuration;

			// Token: 0x040063D4 RID: 25556
			public int abilityCooldown;

			// Token: 0x040063D5 RID: 25557
			public int walkingGraceTimeMax;

			// Token: 0x040063D6 RID: 25558
			public bool dismountsOnItemUse;

			// Token: 0x040063D7 RID: 25559
			public int spawnDust;

			// Token: 0x040063D8 RID: 25560
			public bool spawnDustNoGravity;

			// Token: 0x040063D9 RID: 25561
			public int totalFrames;

			// Token: 0x040063DA RID: 25562
			public int standingFrameStart;

			// Token: 0x040063DB RID: 25563
			public int standingFrameCount;

			// Token: 0x040063DC RID: 25564
			public int standingFrameDelay;

			// Token: 0x040063DD RID: 25565
			public int runningFrameStart;

			// Token: 0x040063DE RID: 25566
			public int runningFrameCount;

			// Token: 0x040063DF RID: 25567
			public int runningFrameDelay;

			// Token: 0x040063E0 RID: 25568
			public int flyingFrameStart;

			// Token: 0x040063E1 RID: 25569
			public int flyingFrameCount;

			// Token: 0x040063E2 RID: 25570
			public int flyingFrameDelay;

			// Token: 0x040063E3 RID: 25571
			public int inAirFrameStart;

			// Token: 0x040063E4 RID: 25572
			public int inAirFrameCount;

			// Token: 0x040063E5 RID: 25573
			public int inAirFrameDelay;

			// Token: 0x040063E6 RID: 25574
			public int idleFrameStart;

			// Token: 0x040063E7 RID: 25575
			public int idleFrameCount;

			// Token: 0x040063E8 RID: 25576
			public int idleFrameDelay;

			// Token: 0x040063E9 RID: 25577
			public bool idleFrameLoop;

			// Token: 0x040063EA RID: 25578
			public int swimFrameStart;

			// Token: 0x040063EB RID: 25579
			public int swimFrameCount;

			// Token: 0x040063EC RID: 25580
			public int swimFrameDelay;

			// Token: 0x040063ED RID: 25581
			public int dashingFrameStart;

			// Token: 0x040063EE RID: 25582
			public int dashingFrameCount;

			// Token: 0x040063EF RID: 25583
			public int dashingFrameDelay;

			// Token: 0x040063F0 RID: 25584
			public bool Minecart;

			// Token: 0x040063F1 RID: 25585
			public bool CanRideMinecartTracks;

			// Token: 0x040063F2 RID: 25586
			public bool CanUseWings;

			// Token: 0x040063F3 RID: 25587
			public Vector3 lightColor = Vector3.One;

			// Token: 0x040063F4 RID: 25588
			public bool emitsLight;

			// Token: 0x040063F5 RID: 25589
			public Mount.MountDelegatesData delegations = new Mount.MountDelegatesData();

			// Token: 0x040063F6 RID: 25590
			public int playerXOffset;
		}

		// Token: 0x02000601 RID: 1537
		public enum DismountCheckResult
		{
			// Token: 0x040063F8 RID: 25592
			Succeeded,
			// Token: 0x040063F9 RID: 25593
			FailedCCed,
			// Token: 0x040063FA RID: 25594
			FailedNoSpace
		}
	}
}
