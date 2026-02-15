using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Golf;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace Terraria.DataStructures
{
	// Token: 0x0200059B RID: 1435
	public struct PlayerDrawSet
	{
		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x06003869 RID: 14441 RVA: 0x00631ADC File Offset: 0x0062FCDC
		public Vector2 Center
		{
			get
			{
				return new Vector2(this.Position.X + (float)(this.drawPlayer.width / 2), this.Position.Y + (float)(this.drawPlayer.height / 2));
			}
		}

		// Token: 0x0600386A RID: 14442 RVA: 0x00631B18 File Offset: 0x0062FD18
		public void BoringSetup(Player player, List<DrawData> drawData, List<int> dust, List<int> gore, Vector2 drawPosition, float shadowOpacity, float rotation, Vector2 rotationOrigin, Projectile overrideHeldProjectile)
		{
			this.DrawDataCache = drawData;
			this.SelectedDrawnProjectile = null;
			if (player.heldProj != -1)
			{
				this.SelectedDrawnProjectile = Main.projectile[player.heldProj];
			}
			if (overrideHeldProjectile != null)
			{
				this.SelectedDrawnProjectile = overrideHeldProjectile;
			}
			this.DustCache = dust;
			this.GoreCache = gore;
			this.drawPlayer = player;
			this.shadow = shadowOpacity;
			this.rotation = rotation;
			this.rotationOrigin = rotationOrigin;
			this.heldItem = player.lastVisualizedSelectedItem;
			this.cHead = this.drawPlayer.cHead;
			this.cBody = this.drawPlayer.cBody;
			this.cLegs = this.drawPlayer.cLegs;
			if (this.drawPlayer.wearsRobe)
			{
				this.cLegs = this.cBody;
			}
			this.cHandOn = this.drawPlayer.cHandOn;
			this.cHandOff = this.drawPlayer.cHandOff;
			this.cBack = this.drawPlayer.cBack;
			this.cFront = this.drawPlayer.cFront;
			this.cShoe = this.drawPlayer.cShoe;
			this.cFlameWaker = this.drawPlayer.cFlameWaker;
			this.cWaist = this.drawPlayer.cWaist;
			this.cShield = this.drawPlayer.cShield;
			this.cNeck = this.drawPlayer.cNeck;
			this.cFace = this.drawPlayer.cFace;
			this.cBalloon = this.drawPlayer.cBalloon;
			this.cWings = this.drawPlayer.cWings;
			this.cCarpet = this.drawPlayer.cCarpet;
			this.cPortableStool = this.drawPlayer.cPortableStool;
			this.cFloatingTube = this.drawPlayer.cFloatingTube;
			this.cUnicornHorn = this.drawPlayer.cUnicornHorn;
			this.cAngelHalo = this.drawPlayer.cAngelHalo;
			this.cLeinShampoo = this.drawPlayer.cLeinShampoo;
			this.cBackpack = this.drawPlayer.cBackpack;
			this.cTail = this.drawPlayer.cTail;
			this.cFaceHead = this.drawPlayer.cFaceHead;
			this.cFaceFlower = this.drawPlayer.cFaceFlower;
			this.cFaceMask = this.drawPlayer.cFaceMask;
			this.cBalloonFront = this.drawPlayer.cBalloonFront;
			this.cBeard = this.drawPlayer.cBeard;
			this.cCoat = this.drawPlayer.cCoat;
			this.isSitting = this.drawPlayer.sitting.isSitting;
			this.seatYOffset = 0f;
			this.sittingIndex = 0;
			Vector2 zero = Vector2.Zero;
			this.drawPlayer.sitting.GetSittingOffsetInfo(this.drawPlayer, out zero, out this.seatYOffset);
			if (this.isSitting)
			{
				this.sittingIndex = this.drawPlayer.sitting.sittingIndex;
			}
			if (this.drawPlayer.mount.Active && this.drawPlayer.mount.Type == 17)
			{
				this.isSitting = true;
			}
			if (this.drawPlayer.mount.Active && this.drawPlayer.mount.Type == 23)
			{
				this.isSitting = true;
			}
			if (this.drawPlayer.mount.Active && this.drawPlayer.mount.Type == 45)
			{
				this.isSitting = true;
			}
			this.isSleeping = this.drawPlayer.sleeping.isSleeping;
			this.Position = drawPosition;
			this.Position += new Vector2(this.drawPlayer.MountXOffset * (float)this.drawPlayer.direction, 0f);
			if (this.isSitting)
			{
				this.torsoOffset = this.seatYOffset;
				this.Position += zero;
			}
			else
			{
				this.sittingIndex = -1;
			}
			if (this.isSleeping)
			{
				this.rotationOrigin = player.Size / 2f;
				Vector2 value;
				this.drawPlayer.sleeping.GetSleepingOffsetInfo(this.drawPlayer, out value);
				this.Position += value;
			}
			this.weaponDrawOrder = WeaponDrawOrder.BehindFrontArm;
			if (this.heldItem.type == 4952)
			{
				this.weaponDrawOrder = WeaponDrawOrder.BehindBackArm;
			}
			if (GolfHelper.IsPlayerHoldingClub(player) && player.itemAnimation > player.itemAnimationMax)
			{
				this.weaponDrawOrder = WeaponDrawOrder.OverFrontArm;
			}
			this.projectileDrawPosition = -1;
			this.ItemLocation = this.Position + (this.drawPlayer.itemLocation - this.drawPlayer.position);
			this.armorAdjust = 0;
			this.missingHand = false;
			this.missingArm = false;
			this.skinVar = this.drawPlayer.skinVariant;
			if (this.drawPlayer.body == 77 || this.drawPlayer.body == 103 || this.drawPlayer.body == 41 || this.drawPlayer.body == 100 || this.drawPlayer.body == 10 || this.drawPlayer.body == 11 || this.drawPlayer.body == 12 || this.drawPlayer.body == 13 || this.drawPlayer.body == 14 || this.drawPlayer.body == 43 || this.drawPlayer.body == 15 || this.drawPlayer.body == 16 || this.drawPlayer.body == 20 || this.drawPlayer.body == 39 || this.drawPlayer.body == 50 || this.drawPlayer.body == 38 || this.drawPlayer.body == 40 || this.drawPlayer.body == 57 || this.drawPlayer.body == 44 || this.drawPlayer.body == 52 || this.drawPlayer.body == 53 || this.drawPlayer.body == 68 || this.drawPlayer.body == 81 || this.drawPlayer.body == 85 || this.drawPlayer.body == 88 || this.drawPlayer.body == 98 || this.drawPlayer.body == 86 || this.drawPlayer.body == 87 || this.drawPlayer.body == 99 || this.drawPlayer.body == 165 || this.drawPlayer.body == 166 || this.drawPlayer.body == 167 || this.drawPlayer.body == 171 || this.drawPlayer.body == 45 || this.drawPlayer.body == 168 || this.drawPlayer.body == 169 || this.drawPlayer.body == 42 || this.drawPlayer.body == 180 || this.drawPlayer.body == 181 || this.drawPlayer.body == 183 || this.drawPlayer.body == 186 || this.drawPlayer.body == 187 || this.drawPlayer.body == 188 || this.drawPlayer.body == 64 || this.drawPlayer.body == 189 || this.drawPlayer.body == 191 || this.drawPlayer.body == 192 || this.drawPlayer.body == 198 || this.drawPlayer.body == 199 || this.drawPlayer.body == 202 || this.drawPlayer.body == 203 || this.drawPlayer.body == 58 || this.drawPlayer.body == 59 || this.drawPlayer.body == 60 || this.drawPlayer.body == 61 || this.drawPlayer.body == 62 || this.drawPlayer.body == 63 || this.drawPlayer.body == 36 || this.drawPlayer.body == 104 || this.drawPlayer.body == 184 || this.drawPlayer.body == 74 || this.drawPlayer.body == 78 || this.drawPlayer.body == 185 || this.drawPlayer.body == 196 || this.drawPlayer.body == 197 || this.drawPlayer.body == 182 || this.drawPlayer.body == 87 || this.drawPlayer.body == 76 || this.drawPlayer.body == 209 || this.drawPlayer.body == 168 || this.drawPlayer.body == 210 || this.drawPlayer.body == 211 || this.drawPlayer.body == 213)
			{
				this.missingHand = true;
			}
			int num = this.drawPlayer.body;
			if (num == 83)
			{
				this.missingArm = false;
			}
			else
			{
				this.missingArm = true;
			}
			this.drawPlayer.GetHairSettings(out this.fullHair, out this.hatHair, out this.hideHair, out this.backHairDraw, out this.drawsBackHairWithoutHeadgear);
			this.hairDyePacked = PlayerDrawHelper.PackShader((int)this.drawPlayer.hairDye, PlayerDrawHelper.ShaderConfiguration.HairShader);
			if (this.drawPlayer.head == 0 && this.drawPlayer.hairDye == 0)
			{
				this.hairDyePacked = PlayerDrawHelper.PackShader(1, PlayerDrawHelper.ShaderConfiguration.HairShader);
			}
			this.skinDyePacked = player.skinDyePacked;
			if (this.drawPlayer.mount.Active)
			{
				if (this.drawPlayer.mount.Type == 52)
				{
					this.AdjustmentsForWolfMount();
				}
				if (this.drawPlayer.mount.Type == 54)
				{
					this.AdjustmentsForVelociraptorMount();
				}
				if (this.drawPlayer.mount.Type == 55)
				{
					this.AdjustmentsForRatMount();
				}
				if (this.drawPlayer.mount.Type == 56)
				{
					this.AdjustmentsForBatMount();
				}
				if (this.drawPlayer.mount.Type == 61)
				{
					this.AdjustmentsForPixieMount();
				}
			}
			if (this.drawPlayer.isDisplayDollOrInanimate)
			{
				Point point = this.Center.ToTileCoordinates();
				bool isTileSelected;
				if (Main.InSmartCursorHighlightArea(point.X, point.Y, out isTileSelected))
				{
					Color color = Lighting.GetColor(point.X, point.Y);
					int num2 = (int)((color.R + color.G + color.B) / 3);
					if (num2 > 10)
					{
						this.selectionGlowColor = Colors.GetSelectionGlowColor(isTileSelected, num2);
					}
				}
			}
			this.mountOffSet = this.drawPlayer.HeightOffsetVisual;
			this.Position.Y = this.Position.Y - this.mountOffSet;
			if (this.drawPlayer.mount.Active)
			{
				Mount.currentShader = (this.drawPlayer.mount.Cart ? this.drawPlayer.cMinecart : this.drawPlayer.cMount);
			}
			else
			{
				Mount.currentShader = 0;
			}
			this.playerEffect = SpriteEffects.None;
			this.itemEffect = SpriteEffects.FlipHorizontally;
			this.colorHair = this.drawPlayer.GetImmuneAlpha(this.drawPlayer.GetHairColor(true), this.shadow);
			this.colorEyeWhites = this.drawPlayer.GetImmuneAlpha(Lighting.GetColorClamped((int)((double)this.Position.X + (double)this.drawPlayer.width * 0.5) / 16, (int)(((double)this.Position.Y + (double)this.drawPlayer.height * 0.25) / 16.0), Color.White), this.shadow);
			this.colorEyes = this.drawPlayer.GetImmuneAlpha(Lighting.GetColorClamped((int)((double)this.Position.X + (double)this.drawPlayer.width * 0.5) / 16, (int)(((double)this.Position.Y + (double)this.drawPlayer.height * 0.25) / 16.0), this.drawPlayer.eyeColor), this.shadow);
			this.colorHead = this.drawPlayer.GetImmuneAlpha(Lighting.GetColorClamped((int)((double)this.Position.X + (double)this.drawPlayer.width * 0.5) / 16, (int)(((double)this.Position.Y + (double)this.drawPlayer.height * 0.25) / 16.0), this.drawPlayer.skinColor), this.shadow);
			this.colorBodySkin = this.drawPlayer.GetImmuneAlpha(Lighting.GetColorClamped((int)((double)this.Position.X + (double)this.drawPlayer.width * 0.5) / 16, (int)(((double)this.Position.Y + (double)this.drawPlayer.height * 0.5) / 16.0), this.drawPlayer.skinColor), this.shadow);
			this.colorLegs = this.drawPlayer.GetImmuneAlpha(Lighting.GetColorClamped((int)((double)this.Position.X + (double)this.drawPlayer.width * 0.5) / 16, (int)(((double)this.Position.Y + (double)this.drawPlayer.height * 0.75) / 16.0), this.drawPlayer.skinColor), this.shadow);
			this.colorShirt = this.drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)((double)this.Position.X + (double)this.drawPlayer.width * 0.5) / 16, (int)(((double)this.Position.Y + (double)this.drawPlayer.height * 0.5) / 16.0), this.drawPlayer.shirtColor), this.shadow);
			this.colorUnderShirt = this.drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)((double)this.Position.X + (double)this.drawPlayer.width * 0.5) / 16, (int)(((double)this.Position.Y + (double)this.drawPlayer.height * 0.5) / 16.0), this.drawPlayer.underShirtColor), this.shadow);
			this.colorPants = this.drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)((double)this.Position.X + (double)this.drawPlayer.width * 0.5) / 16, (int)(((double)this.Position.Y + (double)this.drawPlayer.height * 0.75) / 16.0), this.drawPlayer.pantsColor), this.shadow);
			this.colorShoes = this.drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)((double)this.Position.X + (double)this.drawPlayer.width * 0.5) / 16, (int)(((double)this.Position.Y + (double)this.drawPlayer.height * 0.75) / 16.0), this.drawPlayer.shoeColor), this.shadow);
			this.colorArmorHead = this.drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)((double)this.Position.X + (double)this.drawPlayer.width * 0.5) / 16, (int)((double)this.Position.Y + (double)this.drawPlayer.height * 0.25) / 16, Color.White), this.shadow);
			this.colorArmorBody = this.drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)((double)this.Position.X + (double)this.drawPlayer.width * 0.5) / 16, (int)((double)this.Position.Y + (double)this.drawPlayer.height * 0.5) / 16, Color.White), this.shadow);
			this.colorMount = this.colorArmorBody;
			this.colorArmorLegs = this.drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)((double)this.Position.X + (double)this.drawPlayer.width * 0.5) / 16, (int)((double)this.Position.Y + (double)this.drawPlayer.height * 0.75) / 16, Color.White), this.shadow);
			this.floatingTubeColor = this.drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)((double)this.Position.X + (double)this.drawPlayer.width * 0.5) / 16, (int)((double)this.Position.Y + (double)this.drawPlayer.height * 0.75) / 16, Color.White), this.shadow);
			this.colorElectricity = new Color(255, 255, 255, 100);
			this.colorDisplayDollSkin = this.colorBodySkin;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			this.headGlowMask = -1;
			this.bodyGlowMask = -1;
			this.armGlowMask = -1;
			this.legsGlowMask = -1;
			this.headGlowColor = Color.Transparent;
			this.bodyGlowColor = Color.Transparent;
			this.armGlowColor = Color.Transparent;
			this.legsGlowColor = Color.Transparent;
			num = this.drawPlayer.head;
			switch (num)
			{
			case 169:
				num3++;
				break;
			case 170:
				num4++;
				break;
			case 171:
				num5++;
				break;
			default:
				if (num == 189)
				{
					num6++;
				}
				break;
			}
			num = this.drawPlayer.body;
			switch (num)
			{
			case 175:
				num3++;
				break;
			case 176:
				num4++;
				break;
			case 177:
				num5++;
				break;
			default:
				if (num == 190)
				{
					num6++;
				}
				break;
			}
			num = this.drawPlayer.legs;
			switch (num)
			{
			case 110:
				num3++;
				break;
			case 111:
				num4++;
				break;
			case 112:
				num5++;
				break;
			default:
				if (num == 130)
				{
					num6++;
				}
				break;
			}
			num3 = 3;
			num4 = 3;
			num5 = 3;
			num6 = 3;
			this.ArkhalisColor = this.drawPlayer.underShirtColor;
			this.ArkhalisColor.A = 180;
			if (this.drawPlayer.head == 169)
			{
				this.headGlowMask = 15;
				byte b = (byte)(62.5f * (float)(1 + num3));
				this.headGlowColor = new Color((int)b, (int)b, (int)b, 0);
			}
			else if (this.drawPlayer.head == 216)
			{
				this.headGlowMask = 256;
				byte b2 = 127;
				this.headGlowColor = new Color((int)b2, (int)b2, (int)b2, 0);
			}
			else if (this.drawPlayer.head == 210)
			{
				this.headGlowMask = 242;
				byte b3 = 127;
				this.headGlowColor = new Color((int)b3, (int)b3, (int)b3, 0);
			}
			else if (this.drawPlayer.head == 214)
			{
				this.headGlowMask = 245;
				this.headGlowColor = this.ArkhalisColor;
			}
			else if (this.drawPlayer.head == 240)
			{
				this.headGlowMask = 273;
				this.headGlowColor = new Color(230, 230, 230, 60);
			}
			else if (this.drawPlayer.head == 267)
			{
				this.headGlowMask = 301;
				this.headGlowColor = new Color(230, 230, 230, 60);
			}
			else if (this.drawPlayer.head == 268)
			{
				this.headGlowMask = 302;
				float num7 = (float)Main.mouseTextColor / 255f;
				num7 *= num7;
				this.headGlowColor = new Color(255, 255, 255) * num7;
			}
			else if (this.drawPlayer.head == 269)
			{
				this.headGlowMask = 304;
				this.headGlowColor = new Color(200, 200, 200);
			}
			else if (this.drawPlayer.head == 270)
			{
				this.headGlowMask = 305;
				this.headGlowColor = new Color(200, 200, 200, 150);
			}
			else if (this.drawPlayer.head == 271)
			{
				this.headGlowMask = 309;
				this.headGlowColor = Color.White;
			}
			else if (this.drawPlayer.head == 170)
			{
				this.headGlowMask = 16;
				byte b4 = (byte)(62.5f * (float)(1 + num4));
				this.headGlowColor = new Color((int)b4, (int)b4, (int)b4, 0);
			}
			else if (this.drawPlayer.head == 189)
			{
				this.headGlowMask = 184;
				byte b5 = (byte)(62.5f * (float)(1 + num6));
				this.headGlowColor = new Color((int)b5, (int)b5, (int)b5, 0);
				this.colorArmorHead = this.drawPlayer.GetImmuneAlphaPure(new Color((int)b5, (int)b5, (int)b5, 255), this.shadow);
			}
			else if (this.drawPlayer.head == 171)
			{
				byte b6 = (byte)(62.5f * (float)(1 + num5));
				this.colorArmorHead = this.drawPlayer.GetImmuneAlphaPure(new Color((int)b6, (int)b6, (int)b6, 255), this.shadow);
			}
			else if (this.drawPlayer.head == 175)
			{
				this.headGlowMask = 41;
				this.headGlowColor = new Color(255, 255, 255, 0);
			}
			else if (this.drawPlayer.head == 193)
			{
				this.headGlowMask = 209;
				this.headGlowColor = new Color(255, 255, 255, 127);
			}
			else if (this.drawPlayer.head == 109)
			{
				this.headGlowMask = 208;
				this.headGlowColor = new Color(255, 255, 255, 0);
			}
			else if (this.drawPlayer.head == 178)
			{
				this.headGlowMask = 96;
				this.headGlowColor = new Color(255, 255, 255, 0);
			}
			else if (this.drawPlayer.head == 282)
			{
				this.headGlowMask = 357;
				float num8 = (float)Main.mouseTextColor / 255f;
				num8 *= num8;
				this.headGlowColor = new Color(255, 255, 255, 0) * num8;
			}
			else if (this.drawPlayer.head == 284)
			{
				this.headGlowMask = 365;
				this.headGlowColor = PlayerDrawLayers.GetChickenBonesGlowColor(ref this, false, false);
			}
			else if (this.drawPlayer.head == 285)
			{
				this.headGlowMask = 367;
				this.headGlowColor = new Color(255, 255, 255, 0);
			}
			else if (this.drawPlayer.head == 291)
			{
				this.headGlowMask = 375;
				this.headGlowColor = new Color(255, 255, 255, 255);
			}
			else if (this.drawPlayer.head == 292)
			{
				this.headGlowMask = 378;
				this.headGlowColor = PlayerDrawLayers.GetLunaGlowColor(ref this, false);
			}
			if (this.drawPlayer.body == 175)
			{
				if (this.drawPlayer.Male)
				{
					this.bodyGlowMask = 13;
				}
				else
				{
					this.bodyGlowMask = 18;
				}
				byte b7 = (byte)(62.5f * (float)(1 + num3));
				this.bodyGlowColor = new Color((int)b7, (int)b7, (int)b7, 0);
			}
			else if (this.drawPlayer.body == 208)
			{
				if (this.drawPlayer.Male)
				{
					this.bodyGlowMask = 246;
				}
				else
				{
					this.bodyGlowMask = 247;
				}
				this.armGlowMask = 248;
				this.bodyGlowColor = this.ArkhalisColor;
				this.armGlowColor = this.ArkhalisColor;
			}
			else if (this.drawPlayer.body == 227)
			{
				this.bodyGlowColor = new Color(230, 230, 230, 60);
				this.armGlowColor = new Color(230, 230, 230, 60);
			}
			else if (this.drawPlayer.body == 237)
			{
				float num9 = (float)Main.mouseTextColor / 255f;
				num9 *= num9;
				this.bodyGlowColor = new Color(255, 255, 255) * num9;
			}
			else if (this.drawPlayer.body == 238 || this.drawPlayer.body == 260)
			{
				this.bodyGlowColor = new Color(255, 255, 255);
				this.armGlowColor = new Color(255, 255, 255);
			}
			else if (this.drawPlayer.body == 239)
			{
				this.bodyGlowColor = new Color(200, 200, 200, 150);
				this.armGlowColor = new Color(200, 200, 200, 150);
			}
			else if (this.drawPlayer.body == 190)
			{
				if (this.drawPlayer.Male)
				{
					this.bodyGlowMask = 185;
				}
				else
				{
					this.bodyGlowMask = 186;
				}
				this.armGlowMask = 188;
				byte b8 = (byte)(62.5f * (float)(1 + num6));
				this.bodyGlowColor = new Color((int)b8, (int)b8, (int)b8, 0);
				this.armGlowColor = new Color((int)b8, (int)b8, (int)b8, 0);
				this.colorArmorBody = this.drawPlayer.GetImmuneAlphaPure(new Color((int)b8, (int)b8, (int)b8, 255), this.shadow);
			}
			else if (this.drawPlayer.body == 176)
			{
				if (this.drawPlayer.Male)
				{
					this.bodyGlowMask = 14;
				}
				else
				{
					this.bodyGlowMask = 19;
				}
				this.armGlowMask = 12;
				byte b9 = (byte)(62.5f * (float)(1 + num4));
				this.bodyGlowColor = new Color((int)b9, (int)b9, (int)b9, 0);
				this.armGlowColor = new Color((int)b9, (int)b9, (int)b9, 0);
			}
			else if (this.drawPlayer.body == 194)
			{
				this.bodyGlowMask = 210;
				this.armGlowMask = 211;
				this.bodyGlowColor = new Color(255, 255, 255, 127);
				this.armGlowColor = new Color(255, 255, 255, 127);
			}
			else if (this.drawPlayer.body == 177)
			{
				byte b10 = (byte)(62.5f * (float)(1 + num5));
				this.colorArmorBody = this.drawPlayer.GetImmuneAlphaPure(new Color((int)b10, (int)b10, (int)b10, 255), this.shadow);
			}
			else if (this.drawPlayer.body == 179)
			{
				if (this.drawPlayer.Male)
				{
					this.bodyGlowMask = 42;
				}
				else
				{
					this.bodyGlowMask = 43;
				}
				this.armGlowMask = 44;
				this.bodyGlowColor = new Color(255, 255, 255, 0);
				this.armGlowColor = new Color(255, 255, 255, 0);
			}
			if (this.drawPlayer.legs == 111)
			{
				this.legsGlowMask = 17;
				byte b11 = (byte)(62.5f * (float)(1 + num4));
				this.legsGlowColor = new Color((int)b11, (int)b11, (int)b11, 0);
			}
			else if (this.drawPlayer.legs == 157)
			{
				this.legsGlowMask = 249;
				this.legsGlowColor = this.ArkhalisColor;
			}
			else if (this.drawPlayer.legs == 158)
			{
				this.legsGlowMask = 250;
				this.legsGlowColor = this.ArkhalisColor;
			}
			else if (this.drawPlayer.legs == 210)
			{
				this.legsGlowMask = 274;
				this.legsGlowColor = new Color(230, 230, 230, 60);
			}
			else if (this.drawPlayer.legs == 222)
			{
				this.legsGlowMask = 303;
				float num10 = (float)Main.mouseTextColor / 255f;
				num10 *= num10;
				this.legsGlowColor = new Color(255, 255, 255) * num10;
			}
			else if (this.drawPlayer.legs == 225)
			{
				this.legsGlowMask = 306;
				this.legsGlowColor = new Color(200, 200, 200, 150);
			}
			else if (this.drawPlayer.legs == 226)
			{
				this.legsGlowMask = 307;
				this.legsGlowColor = new Color(200, 200, 200, 150);
			}
			else if (this.drawPlayer.legs == 110)
			{
				this.legsGlowMask = 199;
				byte b12 = (byte)(62.5f * (float)(1 + num3));
				this.legsGlowColor = new Color((int)b12, (int)b12, (int)b12, 0);
			}
			else if (this.drawPlayer.legs == 112)
			{
				byte b13 = (byte)(62.5f * (float)(1 + num5));
				this.colorArmorLegs = this.drawPlayer.GetImmuneAlphaPure(new Color((int)b13, (int)b13, (int)b13, 255), this.shadow);
			}
			else if (this.drawPlayer.legs == 134)
			{
				this.legsGlowMask = 212;
				this.legsGlowColor = new Color(255, 255, 255, 127);
			}
			else if (this.drawPlayer.legs == 130)
			{
				byte b14 = (byte)(127 * (1 + num6));
				this.legsGlowMask = 187;
				this.legsGlowColor = new Color((int)b14, (int)b14, (int)b14, 0);
				this.colorArmorLegs = this.drawPlayer.GetImmuneAlphaPure(new Color((int)b14, (int)b14, (int)b14, 255), this.shadow);
			}
			float alphaReduction = this.shadow;
			this.headGlowColor = this.drawPlayer.GetImmuneAlphaPure(this.headGlowColor, alphaReduction);
			this.bodyGlowColor = this.drawPlayer.GetImmuneAlphaPure(this.bodyGlowColor, alphaReduction);
			this.armGlowColor = this.drawPlayer.GetImmuneAlphaPure(this.armGlowColor, alphaReduction);
			this.legsGlowColor = this.drawPlayer.GetImmuneAlphaPure(this.legsGlowColor, alphaReduction);
			if (this.drawPlayer.head > 0 && this.drawPlayer.head < ArmorIDs.Head.Count)
			{
				Main.instance.LoadArmorHead(this.drawPlayer.head);
				int num11 = ArmorIDs.Head.Sets.FrontToBackID[this.drawPlayer.head];
				if (num11 >= 0)
				{
					Main.instance.LoadArmorHead(num11);
				}
			}
			if (this.drawPlayer.body > 0 && this.drawPlayer.body < ArmorIDs.Body.Count)
			{
				Main.instance.LoadArmorBody(this.drawPlayer.body);
			}
			if (this.drawPlayer.legs > 0 && this.drawPlayer.legs < ArmorIDs.Legs.Count)
			{
				Main.instance.LoadArmorLegs(this.drawPlayer.legs);
			}
			if (this.drawPlayer.handon > 0 && (int)this.drawPlayer.handon < ArmorIDs.HandOn.Count)
			{
				Main.instance.LoadAccHandsOn((int)this.drawPlayer.handon);
			}
			if (this.drawPlayer.handoff > 0 && (int)this.drawPlayer.handoff < ArmorIDs.HandOff.Count)
			{
				Main.instance.LoadAccHandsOff((int)this.drawPlayer.handoff);
			}
			if (this.drawPlayer.back > 0 && (int)this.drawPlayer.back < ArmorIDs.Back.Count)
			{
				Main.instance.LoadAccBack((int)this.drawPlayer.back);
			}
			if (this.drawPlayer.front > 0 && (int)this.drawPlayer.front < ArmorIDs.Front.Count)
			{
				Main.instance.LoadAccFront((int)this.drawPlayer.front);
			}
			if (this.drawPlayer.shoe > 0 && (int)this.drawPlayer.shoe < ArmorIDs.Shoe.Count)
			{
				Main.instance.LoadAccShoes((int)this.drawPlayer.shoe);
			}
			if (this.drawPlayer.waist > 0 && (int)this.drawPlayer.waist < ArmorIDs.Waist.Count)
			{
				Main.instance.LoadAccWaist((int)this.drawPlayer.waist);
			}
			if (this.drawPlayer.shield > 0 && (int)this.drawPlayer.shield < ArmorIDs.Shield.Count)
			{
				Main.instance.LoadAccShield((int)this.drawPlayer.shield);
			}
			if (this.drawPlayer.neck > 0 && (int)this.drawPlayer.neck < ArmorIDs.Neck.Count)
			{
				Main.instance.LoadAccNeck((int)this.drawPlayer.neck);
			}
			if (this.drawPlayer.face > 0 && this.drawPlayer.face < ArmorIDs.Face.Count)
			{
				Main.instance.LoadAccFace((int)this.drawPlayer.face);
			}
			if (this.drawPlayer.balloon > 0 && (int)this.drawPlayer.balloon < ArmorIDs.Balloon.Count)
			{
				Main.instance.LoadAccBalloon((int)this.drawPlayer.balloon);
			}
			if (this.drawPlayer.backpack > 0 && (int)this.drawPlayer.backpack < ArmorIDs.Back.Count)
			{
				Main.instance.LoadAccBack((int)this.drawPlayer.backpack);
			}
			if (this.drawPlayer.tail > 0 && (int)this.drawPlayer.tail < ArmorIDs.Back.Count)
			{
				Main.instance.LoadAccBack((int)this.drawPlayer.tail);
			}
			if (this.drawPlayer.faceHead > 0 && this.drawPlayer.faceHead < ArmorIDs.Face.Count)
			{
				Main.instance.LoadAccFace((int)this.drawPlayer.faceHead);
			}
			if (this.drawPlayer.faceFlower > 0 && this.drawPlayer.faceFlower < ArmorIDs.Face.Count)
			{
				Main.instance.LoadAccFace((int)this.drawPlayer.faceFlower);
			}
			if (this.drawPlayer.faceMask > 0 && this.drawPlayer.faceMask < ArmorIDs.Face.Count)
			{
				Main.instance.LoadAccFace((int)this.drawPlayer.faceMask);
			}
			if (this.drawPlayer.balloonFront > 0 && (int)this.drawPlayer.balloonFront < ArmorIDs.Balloon.Count)
			{
				Main.instance.LoadAccBalloon((int)this.drawPlayer.balloonFront);
			}
			if (this.drawPlayer.beard > 0 && this.drawPlayer.beard < ArmorIDs.Beard.Count)
			{
				Main.instance.LoadAccBeard((int)this.drawPlayer.beard);
			}
			if (this.drawPlayer.coat > 0 && this.drawPlayer.coat < ArmorIDs.Body.Count)
			{
				Main.instance.LoadArmorBody(this.drawPlayer.coat);
			}
			Main.instance.LoadHair(this.drawPlayer.hair);
			if (this.drawPlayer.eyebrellaCloud)
			{
				Main.instance.LoadProjectile(238);
			}
			if (this.drawPlayer.isHatRackDoll)
			{
				this.colorLegs = Color.Transparent;
				this.colorBodySkin = Color.Transparent;
				this.colorHead = Color.Transparent;
				this.colorHair = Color.Transparent;
				this.colorEyes = Color.Transparent;
				this.colorEyeWhites = Color.Transparent;
			}
			if (this.drawPlayer.isDisplayDollOrInanimate)
			{
				if (this.drawPlayer.isFullbright)
				{
					this.colorHead = Color.White;
					this.colorBodySkin = Color.White;
					this.colorLegs = Color.White;
					this.colorEyes = Color.White;
					this.colorEyeWhites = Color.White;
					this.colorArmorHead = Color.White;
					this.colorArmorBody = Color.White;
					this.colorArmorLegs = Color.White;
					this.colorDisplayDollSkin = PlayerDrawHelper.DISPLAY_DOLL_DEFAULT_SKIN_COLOR;
				}
				else
				{
					this.colorDisplayDollSkin = this.drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)((double)this.Position.X + (double)this.drawPlayer.width * 0.5) / 16, (int)((double)this.Position.Y + (double)this.drawPlayer.height * 0.5) / 16, PlayerDrawHelper.DISPLAY_DOLL_DEFAULT_SKIN_COLOR), this.shadow);
				}
			}
			if (!this.drawPlayer.isDisplayDollOrInanimate)
			{
				if ((this.drawPlayer.head == 80 || this.drawPlayer.head == 79 || this.drawPlayer.head == 78 || this.drawPlayer.head == 283) && this.drawPlayer.body == 51 && this.drawPlayer.legs == 47)
				{
					float num12 = (float)Main.mouseTextColor / 200f - 0.3f;
					if (this.shadow != 0f)
					{
						num12 = 0f;
					}
					this.colorArmorHead.R = (byte)((float)this.colorArmorHead.R * num12);
					this.colorArmorHead.G = (byte)((float)this.colorArmorHead.G * num12);
					this.colorArmorHead.B = (byte)((float)this.colorArmorHead.B * num12);
					this.colorArmorBody.R = (byte)((float)this.colorArmorBody.R * num12);
					this.colorArmorBody.G = (byte)((float)this.colorArmorBody.G * num12);
					this.colorArmorBody.B = (byte)((float)this.colorArmorBody.B * num12);
					this.colorArmorLegs.R = (byte)((float)this.colorArmorLegs.R * num12);
					this.colorArmorLegs.G = (byte)((float)this.colorArmorLegs.G * num12);
					this.colorArmorLegs.B = (byte)((float)this.colorArmorLegs.B * num12);
				}
				if (this.drawPlayer.head == 193 && this.drawPlayer.body == 194 && this.drawPlayer.legs == 134)
				{
					float num13 = 0.6f - this.drawPlayer.ghostFade * 0.3f;
					if (this.shadow != 0f)
					{
						num13 = 0f;
					}
					this.colorArmorHead.R = (byte)((float)this.colorArmorHead.R * num13);
					this.colorArmorHead.G = (byte)((float)this.colorArmorHead.G * num13);
					this.colorArmorHead.B = (byte)((float)this.colorArmorHead.B * num13);
					this.colorArmorBody.R = (byte)((float)this.colorArmorBody.R * num13);
					this.colorArmorBody.G = (byte)((float)this.colorArmorBody.G * num13);
					this.colorArmorBody.B = (byte)((float)this.colorArmorBody.B * num13);
					this.colorArmorLegs.R = (byte)((float)this.colorArmorLegs.R * num13);
					this.colorArmorLegs.G = (byte)((float)this.colorArmorLegs.G * num13);
					this.colorArmorLegs.B = (byte)((float)this.colorArmorLegs.B * num13);
				}
				if (this.shadow > 0f)
				{
					this.colorLegs = Color.Transparent;
					this.colorBodySkin = Color.Transparent;
					this.colorHead = Color.Transparent;
					this.colorHair = Color.Transparent;
					this.colorEyes = Color.Transparent;
					this.colorEyeWhites = Color.Transparent;
				}
			}
			float num14 = 1f;
			float num15 = 1f;
			float num16 = 1f;
			float num17 = 1f;
			if (this.drawPlayer.honey && Main.rand.Next(30) == 0 && this.shadow == 0f)
			{
				Dust dust2 = Dust.NewDustDirect(this.Position, this.drawPlayer.width, this.drawPlayer.height, 152, 0f, 0f, 150, default(Color), 1f);
				dust2.velocity.Y = 0.3f;
				Dust dust3 = dust2;
				dust3.velocity.X = dust3.velocity.X * 0.1f;
				dust2.scale += (float)Main.rand.Next(3, 4) * 0.1f;
				dust2.alpha = 100;
				dust2.noGravity = true;
				dust2.velocity += this.drawPlayer.velocity * 0.1f;
				this.DustCache.Add(dust2.dustIndex);
			}
			if (this.drawPlayer.dryadWard && this.drawPlayer.velocity.X != 0f && Main.rand.Next(4) == 0)
			{
				Dust dust4 = Dust.NewDustDirect(new Vector2(this.drawPlayer.position.X - 2f, this.drawPlayer.position.Y + (float)this.drawPlayer.height - 2f), this.drawPlayer.width + 4, 4, 163, 0f, 0f, 100, default(Color), 1.5f);
				dust4.noGravity = true;
				dust4.noLight = true;
				dust4.velocity *= 0f;
				this.DustCache.Add(dust4.dustIndex);
			}
			if (this.drawPlayer.poisoned)
			{
				if (Main.rand.Next(50) == 0 && this.shadow == 0f)
				{
					Dust dust5 = Dust.NewDustDirect(this.Position, this.drawPlayer.width, this.drawPlayer.height, 46, 0f, 0f, 150, default(Color), 0.2f);
					dust5.noGravity = true;
					dust5.fadeIn = 1.9f;
					this.DustCache.Add(dust5.dustIndex);
				}
				num14 *= 0.65f;
				num16 *= 0.75f;
			}
			if (this.drawPlayer.venom)
			{
				if (Main.rand.Next(10) == 0 && this.shadow == 0f)
				{
					Dust dust6 = Dust.NewDustDirect(this.Position, this.drawPlayer.width, this.drawPlayer.height, 171, 0f, 0f, 100, default(Color), 0.5f);
					dust6.noGravity = true;
					dust6.fadeIn = 1.5f;
					this.DustCache.Add(dust6.dustIndex);
				}
				num15 *= 0.45f;
				num14 *= 0.75f;
			}
			if (this.drawPlayer.onFire)
			{
				if (Main.vampireSeed)
				{
					if (this.shadow == 0f)
					{
						for (int i = 0; i < 5; i++)
						{
							Dust dust7 = Dust.NewDustDirect(new Vector2(this.Position.X - 2f, this.Position.Y - 2f), this.drawPlayer.width + 10, this.drawPlayer.height + 10, 6, this.drawPlayer.velocity.X * 0.4f, this.drawPlayer.velocity.Y * 0.4f, 100, default(Color), 3f);
							dust7.noGravity = true;
							dust7.velocity *= 2.3f;
							Dust dust8 = dust7;
							dust8.velocity.Y = dust8.velocity.Y - 0.8f;
							if (i == 0)
							{
								Dust dust9 = dust7;
								dust9.velocity.X = dust9.velocity.X * 0.5f;
								Dust dust10 = dust7;
								dust10.velocity.Y = dust10.velocity.Y - 1.5f;
								dust7.noGravity = false;
								dust7.scale *= 0.4f;
							}
							this.DustCache.Add(dust7.dustIndex);
						}
					}
					num16 *= 0.6f;
					num15 *= 0.7f;
				}
				else
				{
					if (Main.rand.Next(4) == 0 && this.shadow == 0f)
					{
						Dust dust11 = Dust.NewDustDirect(new Vector2(this.Position.X - 2f, this.Position.Y - 2f), this.drawPlayer.width + 4, this.drawPlayer.height + 4, 6, this.drawPlayer.velocity.X * 0.4f, this.drawPlayer.velocity.Y * 0.4f, 100, default(Color), 3f);
						dust11.noGravity = true;
						dust11.velocity *= 1.8f;
						Dust dust12 = dust11;
						dust12.velocity.Y = dust12.velocity.Y - 0.5f;
						this.DustCache.Add(dust11.dustIndex);
					}
					num16 *= 0.6f;
					num15 *= 0.7f;
				}
			}
			if (this.drawPlayer.onFire3)
			{
				if (Main.rand.Next(4) == 0 && this.shadow == 0f)
				{
					Dust dust13 = Dust.NewDustDirect(new Vector2(this.Position.X - 2f, this.Position.Y - 2f), this.drawPlayer.width + 4, this.drawPlayer.height + 4, 6, this.drawPlayer.velocity.X * 0.4f, this.drawPlayer.velocity.Y * 0.4f, 100, default(Color), 3f);
					dust13.noGravity = true;
					dust13.velocity *= 1.8f;
					Dust dust14 = dust13;
					dust14.velocity.Y = dust14.velocity.Y - 0.5f;
					this.DustCache.Add(dust13.dustIndex);
				}
				num16 *= 0.6f;
				num15 *= 0.7f;
			}
			if (this.drawPlayer.dripping && this.shadow == 0f && Main.rand.Next(4) != 0)
			{
				Vector2 position = this.Position;
				position.X -= 2f;
				position.Y -= 2f;
				if (Main.rand.Next(2) == 0)
				{
					Dust dust15 = Dust.NewDustDirect(position, this.drawPlayer.width + 4, this.drawPlayer.height + 2, 211, 0f, 0f, 50, default(Color), 0.8f);
					if (Main.rand.Next(2) == 0)
					{
						dust15.alpha += 25;
					}
					if (Main.rand.Next(2) == 0)
					{
						dust15.alpha += 25;
					}
					dust15.noLight = true;
					dust15.velocity *= 0.2f;
					Dust dust16 = dust15;
					dust16.velocity.Y = dust16.velocity.Y + 0.2f;
					dust15.velocity += this.drawPlayer.velocity;
					this.DustCache.Add(dust15.dustIndex);
				}
				else
				{
					Dust dust17 = Dust.NewDustDirect(position, this.drawPlayer.width + 8, this.drawPlayer.height + 8, 211, 0f, 0f, 50, default(Color), 1.1f);
					if (Main.rand.Next(2) == 0)
					{
						dust17.alpha += 25;
					}
					if (Main.rand.Next(2) == 0)
					{
						dust17.alpha += 25;
					}
					dust17.noLight = true;
					dust17.noGravity = true;
					dust17.velocity *= 0.2f;
					Dust dust18 = dust17;
					dust18.velocity.Y = dust18.velocity.Y + 1f;
					dust17.velocity += this.drawPlayer.velocity;
					this.DustCache.Add(dust17.dustIndex);
				}
			}
			if (this.drawPlayer.drippingSlime)
			{
				int alpha = 175;
				Color newColor = new Color(0, 80, 255, 100);
				if (Main.rand.Next(4) != 0 && this.shadow == 0f)
				{
					Vector2 position2 = this.Position;
					position2.X -= 2f;
					position2.Y -= 2f;
					if (Main.rand.Next(2) == 0)
					{
						Dust dust19 = Dust.NewDustDirect(position2, this.drawPlayer.width + 4, this.drawPlayer.height + 2, 4, 0f, 0f, alpha, newColor, 1.4f);
						if (Main.rand.Next(2) == 0)
						{
							dust19.alpha += 25;
						}
						if (Main.rand.Next(2) == 0)
						{
							dust19.alpha += 25;
						}
						dust19.noLight = true;
						dust19.velocity *= 0.2f;
						Dust dust20 = dust19;
						dust20.velocity.Y = dust20.velocity.Y + 0.2f;
						dust19.velocity += this.drawPlayer.velocity;
						this.DustCache.Add(dust19.dustIndex);
					}
				}
				num14 *= 0.8f;
				num15 *= 0.8f;
			}
			if (this.drawPlayer.drippingSparkleSlime)
			{
				int alpha2 = 100;
				if (Main.rand.Next(4) != 0 && this.shadow == 0f)
				{
					Vector2 position3 = this.Position;
					position3.X -= 2f;
					position3.Y -= 2f;
					if (Main.rand.Next(4) == 0)
					{
						Color newColor2 = Main.hslToRgb(0.7f + 0.2f * Main.rand.NextFloat(), 1f, 0.5f, byte.MaxValue);
						newColor2.A /= 2;
						Dust dust21 = Dust.NewDustDirect(position3, this.drawPlayer.width + 4, this.drawPlayer.height + 2, 4, 0f, 0f, alpha2, newColor2, 0.65f);
						if (Main.rand.Next(2) == 0)
						{
							dust21.alpha += 25;
						}
						if (Main.rand.Next(2) == 0)
						{
							dust21.alpha += 25;
						}
						dust21.noLight = true;
						dust21.velocity *= 0.2f;
						dust21.velocity += this.drawPlayer.velocity * 0.7f;
						dust21.fadeIn = 0.8f;
						this.DustCache.Add(dust21.dustIndex);
					}
					if (Main.rand.Next(30) == 0)
					{
						Color color2;
						Main.hslToRgb(0.7f + 0.2f * Main.rand.NextFloat(), 1f, 0.5f, byte.MaxValue).A = color2.A / 2;
						Dust dust22 = Dust.NewDustDirect(position3, this.drawPlayer.width + 4, this.drawPlayer.height + 2, 43, 0f, 0f, 254, new Color(127, 127, 127, 0), 0.45f);
						dust22.noLight = true;
						Dust dust23 = dust22;
						dust23.velocity.X = dust23.velocity.X * 0f;
						dust22.velocity *= 0.03f;
						dust22.fadeIn = 0.6f;
						this.DustCache.Add(dust22.dustIndex);
					}
				}
				num14 *= 0.94f;
				num15 *= 0.82f;
			}
			if (this.drawPlayer.ichor)
			{
				num16 = 0f;
			}
			if (this.drawPlayer.electrified && this.shadow == 0f && Main.rand.Next(3) == 0)
			{
				Dust dust24 = Dust.NewDustDirect(new Vector2(this.Position.X - 2f, this.Position.Y - 2f), this.drawPlayer.width + 4, this.drawPlayer.height + 4, 226, 0f, 0f, 100, default(Color), 0.5f);
				dust24.velocity *= 1.6f;
				Dust dust25 = dust24;
				dust25.velocity.Y = dust25.velocity.Y - 1f;
				dust24.position = Vector2.Lerp(dust24.position, this.drawPlayer.Center, 0.5f);
				this.DustCache.Add(dust24.dustIndex);
			}
			if (this.drawPlayer.burned)
			{
				if (this.shadow == 0f)
				{
					Dust dust26 = Dust.NewDustDirect(new Vector2(this.Position.X - 2f, this.Position.Y - 2f), this.drawPlayer.width + 4, this.drawPlayer.height + 4, 6, this.drawPlayer.velocity.X * 0.4f, this.drawPlayer.velocity.Y * 0.4f, 100, default(Color), 2f);
					dust26.noGravity = true;
					dust26.velocity *= 1.8f;
					Dust dust27 = dust26;
					dust27.velocity.Y = dust27.velocity.Y - 0.75f;
					this.DustCache.Add(dust26.dustIndex);
				}
				num14 = 1f;
				num16 *= 0.6f;
				num15 *= 0.7f;
			}
			if (this.drawPlayer.onFrostBurn)
			{
				if (Main.rand.Next(4) == 0 && this.shadow == 0f)
				{
					Dust dust28 = Dust.NewDustDirect(new Vector2(this.Position.X - 2f, this.Position.Y - 2f), this.drawPlayer.width + 4, this.drawPlayer.height + 4, 135, this.drawPlayer.velocity.X * 0.4f, this.drawPlayer.velocity.Y * 0.4f, 100, default(Color), 3f);
					dust28.noGravity = true;
					dust28.velocity *= 1.8f;
					Dust dust29 = dust28;
					dust29.velocity.Y = dust29.velocity.Y - 0.5f;
					this.DustCache.Add(dust28.dustIndex);
				}
				num14 *= 0.5f;
				num15 *= 0.7f;
			}
			if (this.drawPlayer.onFrostBurn2)
			{
				if (Main.rand.Next(4) == 0 && this.shadow == 0f)
				{
					Dust dust30 = Dust.NewDustDirect(new Vector2(this.Position.X - 2f, this.Position.Y - 2f), this.drawPlayer.width + 4, this.drawPlayer.height + 4, 135, this.drawPlayer.velocity.X * 0.4f, this.drawPlayer.velocity.Y * 0.4f, 100, default(Color), 3f);
					dust30.noGravity = true;
					dust30.velocity *= 1.8f;
					Dust dust31 = dust30;
					dust31.velocity.Y = dust31.velocity.Y - 0.5f;
					this.DustCache.Add(dust30.dustIndex);
				}
				num14 *= 0.5f;
				num15 *= 0.7f;
			}
			if (this.drawPlayer.onFire2)
			{
				if (Main.rand.Next(4) == 0 && this.shadow == 0f)
				{
					Dust dust32 = Dust.NewDustDirect(new Vector2(this.Position.X - 2f, this.Position.Y - 2f), this.drawPlayer.width + 4, this.drawPlayer.height + 4, 75, this.drawPlayer.velocity.X * 0.4f, this.drawPlayer.velocity.Y * 0.4f, 100, default(Color), 3f);
					dust32.noGravity = true;
					dust32.velocity *= 1.8f;
					Dust dust33 = dust32;
					dust33.velocity.Y = dust33.velocity.Y - 0.5f;
					this.DustCache.Add(dust32.dustIndex);
				}
				num16 *= 0.6f;
				num15 *= 0.7f;
			}
			if (this.drawPlayer.noItems)
			{
				num15 *= 0.8f;
				num14 *= 0.65f;
			}
			if (this.drawPlayer.blind)
			{
				num15 *= 0.65f;
				num14 *= 0.7f;
			}
			if (this.drawPlayer.bleed)
			{
				num15 *= 0.9f;
				num16 *= 0.9f;
				if (!this.drawPlayer.dead && Main.rand.Next(20) == 0 && this.shadow == 0f)
				{
					Dust dust34 = Dust.NewDustDirect(this.Position, this.drawPlayer.width, this.drawPlayer.height, 5, 0f, 0f, 0, default(Color), 1f);
					Dust dust35 = dust34;
					dust35.velocity.Y = dust35.velocity.Y + 0.5f;
					dust34.velocity *= 0.25f;
					this.DustCache.Add(dust34.dustIndex);
				}
			}
			if (this.shadow == 0f && this.drawPlayer.palladiumRegen && this.drawPlayer.statLife < this.drawPlayer.statLifeMax2 && FocusHelper.AllowPlayerToEmitEffects && this.drawPlayer.miscCounter % 10 == 0 && this.shadow == 0f)
			{
				Vector2 position4;
				position4.X = this.Position.X + (float)Main.rand.Next(this.drawPlayer.width);
				position4.Y = this.Position.Y + (float)Main.rand.Next(this.drawPlayer.height);
				position4.X = this.Position.X + (float)(this.drawPlayer.width / 2) - 6f;
				position4.Y = this.Position.Y + (float)(this.drawPlayer.height / 2) - 6f;
				position4.X -= (float)Main.rand.Next(-10, 11);
				position4.Y -= (float)Main.rand.Next(-20, 21);
				int item = Gore.NewGore(position4, new Vector2((float)Main.rand.Next(-10, 11) * 0.1f, (float)Main.rand.Next(-20, -10) * 0.1f), 331, (float)Main.rand.Next(80, 120) * 0.01f);
				this.GoreCache.Add(item);
			}
			if (this.shadow == 0f && this.drawPlayer.loveStruck && FocusHelper.AllowPlayerToEmitEffects && Main.rand.Next(5) == 0)
			{
				Vector2 value2 = new Vector2((float)Main.rand.Next(-10, 11), (float)Main.rand.Next(-10, 11));
				value2.Normalize();
				value2.X *= 0.66f;
				int num18 = Gore.NewGore(this.Position + new Vector2((float)Main.rand.Next(this.drawPlayer.width + 1), (float)Main.rand.Next(this.drawPlayer.height + 1)), value2 * (float)Main.rand.Next(3, 6) * 0.33f, 331, (float)Main.rand.Next(40, 121) * 0.01f);
				Main.gore[num18].sticky = false;
				Main.gore[num18].velocity *= 0.4f;
				Gore gore2 = Main.gore[num18];
				gore2.velocity.Y = gore2.velocity.Y - 0.6f;
				this.GoreCache.Add(num18);
			}
			if (this.drawPlayer.stinky && FocusHelper.AllowPlayerToEmitEffects)
			{
				num14 *= 0.7f;
				num16 *= 0.55f;
				if (Main.rand.Next(5) == 0 && this.shadow == 0f)
				{
					Vector2 vector = new Vector2((float)Main.rand.Next(-10, 11), (float)Main.rand.Next(-10, 11));
					vector.Normalize();
					vector.X *= 0.66f;
					vector.Y = Math.Abs(vector.Y);
					Vector2 vector2 = vector * (float)Main.rand.Next(3, 5) * 0.25f;
					int num19 = Dust.NewDust(this.Position, this.drawPlayer.width, this.drawPlayer.height, 188, vector2.X, vector2.Y * 0.5f, 100, default(Color), 1.5f);
					Main.dust[num19].velocity *= 0.1f;
					Dust dust36 = Main.dust[num19];
					dust36.velocity.Y = dust36.velocity.Y - 0.5f;
					this.DustCache.Add(num19);
				}
			}
			if (this.drawPlayer.slowOgreSpit && FocusHelper.AllowPlayerToEmitEffects)
			{
				num14 *= 0.6f;
				num16 *= 0.45f;
				if (Main.rand.Next(5) == 0 && this.shadow == 0f)
				{
					int type = Utils.SelectRandom<int>(Main.rand, new int[]
					{
						4,
						256
					});
					Dust dust37 = Main.dust[Dust.NewDust(this.Position, this.drawPlayer.width, this.drawPlayer.height, type, 0f, 0f, 100, default(Color), 1f)];
					dust37.scale = 0.8f + Main.rand.NextFloat() * 0.6f;
					dust37.fadeIn = 0.5f;
					dust37.velocity *= 0.05f;
					dust37.noLight = true;
					if (dust37.type == 4)
					{
						dust37.color = new Color(80, 170, 40, 120);
					}
					this.DustCache.Add(dust37.dustIndex);
				}
				if (Main.rand.Next(5) == 0 && this.shadow == 0f)
				{
					int num20 = Gore.NewGore(this.Position + new Vector2(Main.rand.NextFloat(), Main.rand.NextFloat()) * this.drawPlayer.Size, Vector2.Zero, Utils.SelectRandom<int>(Main.rand, new int[]
					{
						1024,
						1025,
						1026
					}), 0.65f);
					Main.gore[num20].velocity *= 0.05f;
					this.GoreCache.Add(num20);
				}
			}
			if (FocusHelper.AllowPlayerToEmitEffects && this.shadow == 0f)
			{
				float num21 = (float)this.drawPlayer.miscCounter / 180f;
				float num22 = 0f;
				float scaleFactor = 10f;
				int type2 = 90;
				int num23 = 0;
				int j = 0;
				while (j < 3)
				{
					switch (j)
					{
					case 0:
						if (this.drawPlayer.nebulaLevelLife >= 1)
						{
							num22 = 6.2831855f / (float)this.drawPlayer.nebulaLevelLife;
							num23 = this.drawPlayer.nebulaLevelLife;
							goto IL_4496;
						}
						break;
					case 1:
						if (this.drawPlayer.nebulaLevelMana >= 1)
						{
							num22 = -6.2831855f / (float)this.drawPlayer.nebulaLevelMana;
							num23 = this.drawPlayer.nebulaLevelMana;
							num21 = (float)(-(float)this.drawPlayer.miscCounter) / 180f;
							scaleFactor = 20f;
							type2 = 88;
							goto IL_4496;
						}
						break;
					case 2:
						if (this.drawPlayer.nebulaLevelDamage >= 1)
						{
							num22 = 6.2831855f / (float)this.drawPlayer.nebulaLevelDamage;
							num23 = this.drawPlayer.nebulaLevelDamage;
							num21 = (float)this.drawPlayer.miscCounter / 180f;
							scaleFactor = 30f;
							type2 = 86;
							goto IL_4496;
						}
						break;
					default:
						goto IL_4496;
					}
					IL_456C:
					j++;
					continue;
					IL_4496:
					for (int k = 0; k < num23; k++)
					{
						Dust dust38 = Dust.NewDustDirect(this.Position, this.drawPlayer.width, this.drawPlayer.height, type2, 0f, 0f, 100, default(Color), 1.5f);
						dust38.noGravity = true;
						dust38.velocity = Vector2.Zero;
						dust38.position = this.drawPlayer.Center + Vector2.UnitY * this.drawPlayer.gfxOffY + (num21 * 6.2831855f + num22 * (float)k).ToRotationVector2() * scaleFactor;
						dust38.customData = this.drawPlayer;
						this.DustCache.Add(dust38.dustIndex);
					}
					goto IL_456C;
				}
			}
			if (this.drawPlayer.witheredArmor && FocusHelper.AllowPlayerToEmitEffects)
			{
				num15 *= 0.5f;
				num14 *= 0.75f;
			}
			if (this.drawPlayer.witheredWeapon && this.drawPlayer.itemAnimation > 0 && this.heldItem.damage > 0 && FocusHelper.AllowPlayerToEmitEffects && Main.rand.Next(3) == 0)
			{
				Dust dust39 = Dust.NewDustDirect(new Vector2(this.Position.X - 2f, this.Position.Y - 2f), this.drawPlayer.width + 4, this.drawPlayer.height + 4, 272, 0f, 0f, 50, default(Color), 0.5f);
				dust39.velocity *= 1.6f;
				Dust dust40 = dust39;
				dust40.velocity.Y = dust40.velocity.Y - 1f;
				dust39.position = Vector2.Lerp(dust39.position, this.drawPlayer.Center, 0.5f);
				this.DustCache.Add(dust39.dustIndex);
			}
			bool shimmering = this.drawPlayer.shimmering;
			if (num14 != 1f || num15 != 1f || num16 != 1f || num17 != 1f)
			{
				if (this.drawPlayer.onFire || this.drawPlayer.onFire2 || this.drawPlayer.onFrostBurn || this.drawPlayer.onFire3 || this.drawPlayer.onFrostBurn2)
				{
					this.colorEyeWhites = this.drawPlayer.GetImmuneAlpha(Color.White, this.shadow);
					this.colorEyes = this.drawPlayer.GetImmuneAlpha(this.drawPlayer.eyeColor, this.shadow);
					this.colorHair = this.drawPlayer.GetImmuneAlpha(this.drawPlayer.GetHairColor(false), this.shadow);
					this.colorHead = this.drawPlayer.GetImmuneAlpha(this.drawPlayer.skinColor, this.shadow);
					this.colorBodySkin = this.drawPlayer.GetImmuneAlpha(this.drawPlayer.skinColor, this.shadow);
					this.colorShirt = this.drawPlayer.GetImmuneAlpha(this.drawPlayer.shirtColor, this.shadow);
					this.colorUnderShirt = this.drawPlayer.GetImmuneAlpha(this.drawPlayer.underShirtColor, this.shadow);
					this.colorPants = this.drawPlayer.GetImmuneAlpha(this.drawPlayer.pantsColor, this.shadow);
					this.colorLegs = this.drawPlayer.GetImmuneAlpha(this.drawPlayer.skinColor, this.shadow);
					this.colorShoes = this.drawPlayer.GetImmuneAlpha(this.drawPlayer.shoeColor, this.shadow);
					this.colorArmorHead = this.drawPlayer.GetImmuneAlpha(Color.White, this.shadow);
					this.colorArmorBody = this.drawPlayer.GetImmuneAlpha(Color.White, this.shadow);
					this.colorArmorLegs = this.drawPlayer.GetImmuneAlpha(Color.White, this.shadow);
					if (this.drawPlayer.isDisplayDollOrInanimate)
					{
						this.colorDisplayDollSkin = this.drawPlayer.GetImmuneAlpha(PlayerDrawHelper.DISPLAY_DOLL_DEFAULT_SKIN_COLOR, this.shadow);
					}
				}
				else
				{
					this.colorEyeWhites = Main.buffColor(this.colorEyeWhites, num14, num15, num16, num17);
					this.colorEyes = Main.buffColor(this.colorEyes, num14, num15, num16, num17);
					this.colorHair = Main.buffColor(this.colorHair, num14, num15, num16, num17);
					this.colorHead = Main.buffColor(this.colorHead, num14, num15, num16, num17);
					this.colorBodySkin = Main.buffColor(this.colorBodySkin, num14, num15, num16, num17);
					this.colorShirt = Main.buffColor(this.colorShirt, num14, num15, num16, num17);
					this.colorUnderShirt = Main.buffColor(this.colorUnderShirt, num14, num15, num16, num17);
					this.colorPants = Main.buffColor(this.colorPants, num14, num15, num16, num17);
					this.colorLegs = Main.buffColor(this.colorLegs, num14, num15, num16, num17);
					this.colorShoes = Main.buffColor(this.colorShoes, num14, num15, num16, num17);
					this.colorArmorHead = Main.buffColor(this.colorArmorHead, num14, num15, num16, num17);
					this.colorArmorBody = Main.buffColor(this.colorArmorBody, num14, num15, num16, num17);
					this.colorArmorLegs = Main.buffColor(this.colorArmorLegs, num14, num15, num16, num17);
					if (this.drawPlayer.isDisplayDollOrInanimate)
					{
						this.colorDisplayDollSkin = Main.buffColor(PlayerDrawHelper.DISPLAY_DOLL_DEFAULT_SKIN_COLOR, num14, num15, num16, num17);
					}
				}
			}
			if (this.drawPlayer.socialGhost)
			{
				this.colorEyeWhites = Color.Transparent;
				this.colorEyes = Color.Transparent;
				this.colorHair = Color.Transparent;
				this.colorHead = Color.Transparent;
				this.colorBodySkin = Color.Transparent;
				this.colorShirt = Color.Transparent;
				this.colorUnderShirt = Color.Transparent;
				this.colorPants = Color.Transparent;
				this.colorShoes = Color.Transparent;
				this.colorLegs = Color.Transparent;
				if (this.colorArmorHead.A > Main.gFade)
				{
					this.colorArmorHead.A = Main.gFade;
				}
				if (this.colorArmorBody.A > Main.gFade)
				{
					this.colorArmorBody.A = Main.gFade;
				}
				if (this.colorArmorLegs.A > Main.gFade)
				{
					this.colorArmorLegs.A = Main.gFade;
				}
				if (this.drawPlayer.isDisplayDollOrInanimate)
				{
					this.colorDisplayDollSkin = Color.Transparent;
				}
			}
			if (this.drawPlayer.socialIgnoreLight)
			{
				float scale = 1f;
				this.colorEyeWhites = Color.White * scale;
				this.colorEyes = this.drawPlayer.eyeColor * scale;
				this.colorHair = GameShaders.Hair.GetColor((short)this.drawPlayer.hairDye, this.drawPlayer, Color.White);
				this.colorHead = this.drawPlayer.skinColor * scale;
				this.colorBodySkin = this.drawPlayer.skinColor * scale;
				this.colorShirt = this.drawPlayer.shirtColor * scale;
				this.colorUnderShirt = this.drawPlayer.underShirtColor * scale;
				this.colorPants = this.drawPlayer.pantsColor * scale;
				this.colorShoes = this.drawPlayer.shoeColor * scale;
				this.colorLegs = this.drawPlayer.skinColor * scale;
				this.colorArmorHead = Color.White;
				this.colorArmorBody = Color.White;
				this.colorArmorLegs = Color.White;
				if (this.drawPlayer.isDisplayDollOrInanimate)
				{
					this.colorDisplayDollSkin = PlayerDrawHelper.DISPLAY_DOLL_DEFAULT_SKIN_COLOR * scale;
				}
			}
			if (this.drawPlayer.opacityForAnimation != 1f)
			{
				this.shadow = 1f - this.drawPlayer.opacityForAnimation;
				float num24 = this.drawPlayer.opacityForAnimation;
				num24 *= num24;
				this.colorEyeWhites = Color.White * num24;
				this.colorEyes = this.drawPlayer.eyeColor * num24;
				this.colorHair = GameShaders.Hair.GetColor((short)this.drawPlayer.hairDye, this.drawPlayer, Color.White) * num24;
				this.colorHead = this.drawPlayer.skinColor * num24;
				this.colorBodySkin = this.drawPlayer.skinColor * num24;
				this.colorShirt = this.drawPlayer.shirtColor * num24;
				this.colorUnderShirt = this.drawPlayer.underShirtColor * num24;
				this.colorPants = this.drawPlayer.pantsColor * num24;
				this.colorShoes = this.drawPlayer.shoeColor * num24;
				this.colorLegs = this.drawPlayer.skinColor * num24;
				this.colorArmorHead = this.drawPlayer.GetImmuneAlpha(Color.White, this.shadow);
				this.colorArmorBody = this.drawPlayer.GetImmuneAlpha(Color.White, this.shadow);
				this.colorArmorLegs = this.drawPlayer.GetImmuneAlpha(Color.White, this.shadow);
				if (this.drawPlayer.isDisplayDollOrInanimate)
				{
					this.colorDisplayDollSkin = PlayerDrawHelper.DISPLAY_DOLL_DEFAULT_SKIN_COLOR * num24;
				}
			}
			this.stealth = 1f;
			if (this.heldItem.type == 3106)
			{
				float num25 = this.drawPlayer.stealth;
				if ((double)num25 < 0.03)
				{
					num25 = 0.03f;
				}
				float num26 = (1f + num25 * 10f) / 11f;
				if (num25 < 0f)
				{
					num25 = 0f;
				}
				if (num25 >= 1f - this.shadow && this.shadow > 0f)
				{
					num25 = this.shadow * 0.5f;
				}
				this.stealth = num26;
				this.colorArmorHead = new Color((int)((byte)((float)this.colorArmorHead.R * num25)), (int)((byte)((float)this.colorArmorHead.G * num25)), (int)((byte)((float)this.colorArmorHead.B * num26)), (int)((byte)((float)this.colorArmorHead.A * num25)));
				this.colorArmorBody = new Color((int)((byte)((float)this.colorArmorBody.R * num25)), (int)((byte)((float)this.colorArmorBody.G * num25)), (int)((byte)((float)this.colorArmorBody.B * num26)), (int)((byte)((float)this.colorArmorBody.A * num25)));
				this.colorArmorLegs = new Color((int)((byte)((float)this.colorArmorLegs.R * num25)), (int)((byte)((float)this.colorArmorLegs.G * num25)), (int)((byte)((float)this.colorArmorLegs.B * num26)), (int)((byte)((float)this.colorArmorLegs.A * num25)));
				num25 *= num25;
				this.colorEyeWhites = Color.Multiply(this.colorEyeWhites, num25);
				this.colorEyes = Color.Multiply(this.colorEyes, num25);
				this.colorHair = Color.Multiply(this.colorHair, num25);
				this.colorHead = Color.Multiply(this.colorHead, num25);
				this.colorBodySkin = Color.Multiply(this.colorBodySkin, num25);
				this.colorShirt = Color.Multiply(this.colorShirt, num25);
				this.colorUnderShirt = Color.Multiply(this.colorUnderShirt, num25);
				this.colorPants = Color.Multiply(this.colorPants, num25);
				this.colorShoes = Color.Multiply(this.colorShoes, num25);
				this.colorLegs = Color.Multiply(this.colorLegs, num25);
				this.colorMount = Color.Multiply(this.colorMount, num25);
				this.floatingTubeColor = Color.Multiply(this.floatingTubeColor, num25);
				this.headGlowColor = Color.Multiply(this.headGlowColor, num25);
				this.bodyGlowColor = Color.Multiply(this.bodyGlowColor, num25);
				this.armGlowColor = Color.Multiply(this.armGlowColor, num25);
				this.legsGlowColor = Color.Multiply(this.legsGlowColor, num25);
				if (this.drawPlayer.isDisplayDollOrInanimate)
				{
					this.colorDisplayDollSkin = Color.Multiply(this.colorDisplayDollSkin, num25);
				}
			}
			else if (this.drawPlayer.shroomiteStealth)
			{
				float num27 = this.drawPlayer.stealth;
				if ((double)num27 < 0.03)
				{
					num27 = 0.03f;
				}
				float num28 = (1f + num27 * 10f) / 11f;
				if (num27 < 0f)
				{
					num27 = 0f;
				}
				if (num27 >= 1f - this.shadow && this.shadow > 0f)
				{
					num27 = this.shadow * 0.5f;
				}
				this.stealth = num28;
				this.colorArmorHead = new Color((int)((byte)((float)this.colorArmorHead.R * num27)), (int)((byte)((float)this.colorArmorHead.G * num27)), (int)((byte)((float)this.colorArmorHead.B * num28)), (int)((byte)((float)this.colorArmorHead.A * num27)));
				this.colorArmorBody = new Color((int)((byte)((float)this.colorArmorBody.R * num27)), (int)((byte)((float)this.colorArmorBody.G * num27)), (int)((byte)((float)this.colorArmorBody.B * num28)), (int)((byte)((float)this.colorArmorBody.A * num27)));
				this.colorArmorLegs = new Color((int)((byte)((float)this.colorArmorLegs.R * num27)), (int)((byte)((float)this.colorArmorLegs.G * num27)), (int)((byte)((float)this.colorArmorLegs.B * num28)), (int)((byte)((float)this.colorArmorLegs.A * num27)));
				num27 *= num27;
				this.colorEyeWhites = Color.Multiply(this.colorEyeWhites, num27);
				this.colorEyes = Color.Multiply(this.colorEyes, num27);
				this.colorHair = Color.Multiply(this.colorHair, num27);
				this.colorHead = Color.Multiply(this.colorHead, num27);
				this.colorBodySkin = Color.Multiply(this.colorBodySkin, num27);
				this.colorShirt = Color.Multiply(this.colorShirt, num27);
				this.colorUnderShirt = Color.Multiply(this.colorUnderShirt, num27);
				this.colorPants = Color.Multiply(this.colorPants, num27);
				this.colorShoes = Color.Multiply(this.colorShoes, num27);
				this.colorLegs = Color.Multiply(this.colorLegs, num27);
				this.colorMount = Color.Multiply(this.colorMount, num27);
				this.floatingTubeColor = Color.Multiply(this.floatingTubeColor, num27);
				this.headGlowColor = Color.Multiply(this.headGlowColor, num27);
				this.bodyGlowColor = Color.Multiply(this.bodyGlowColor, num27);
				this.armGlowColor = Color.Multiply(this.armGlowColor, num27);
				this.legsGlowColor = Color.Multiply(this.legsGlowColor, num27);
				if (this.drawPlayer.isDisplayDollOrInanimate)
				{
					this.colorDisplayDollSkin = Color.Multiply(this.colorDisplayDollSkin, num27);
				}
			}
			else if (this.drawPlayer.setVortex)
			{
				float num29 = this.drawPlayer.stealth;
				if ((double)num29 < 0.03)
				{
					num29 = 0.03f;
				}
				if (num29 < 0f)
				{
					num29 = 0f;
				}
				if (num29 >= 1f - this.shadow && this.shadow > 0f)
				{
					num29 = this.shadow * 0.5f;
				}
				this.stealth = num29;
				Color secondColor = new Color(Vector4.Lerp(Vector4.One, new Vector4(0f, 0.12f, 0.16f, 0f), 1f - num29));
				this.colorArmorHead = this.colorArmorHead.MultiplyRGBA(secondColor);
				this.colorArmorBody = this.colorArmorBody.MultiplyRGBA(secondColor);
				this.colorArmorLegs = this.colorArmorLegs.MultiplyRGBA(secondColor);
				num29 *= num29;
				this.colorEyeWhites = Color.Multiply(this.colorEyeWhites, num29);
				this.colorEyes = Color.Multiply(this.colorEyes, num29);
				this.colorHair = Color.Multiply(this.colorHair, num29);
				this.colorHead = Color.Multiply(this.colorHead, num29);
				this.colorBodySkin = Color.Multiply(this.colorBodySkin, num29);
				this.colorShirt = Color.Multiply(this.colorShirt, num29);
				this.colorUnderShirt = Color.Multiply(this.colorUnderShirt, num29);
				this.colorPants = Color.Multiply(this.colorPants, num29);
				this.colorShoes = Color.Multiply(this.colorShoes, num29);
				this.colorLegs = Color.Multiply(this.colorLegs, num29);
				this.colorMount = Color.Multiply(this.colorMount, num29);
				this.floatingTubeColor = Color.Multiply(this.floatingTubeColor, num29);
				this.headGlowColor = Color.Multiply(this.headGlowColor, num29);
				this.bodyGlowColor = Color.Multiply(this.bodyGlowColor, num29);
				this.armGlowColor = Color.Multiply(this.armGlowColor, num29);
				this.legsGlowColor = Color.Multiply(this.legsGlowColor, num29);
				if (this.drawPlayer.isDisplayDollOrInanimate)
				{
					this.colorDisplayDollSkin = Color.Multiply(this.colorDisplayDollSkin, num29);
				}
			}
			if (this.hideEntirePlayerExceptHelmetsAndFaceAccessories)
			{
				this.hideHair = true;
				Color transparent = Color.Transparent;
				this.colorArmorBody = transparent;
				this.colorArmorLegs = transparent;
				this.colorEyeWhites = transparent;
				this.colorEyes = transparent;
				this.colorBodySkin = transparent;
				this.colorShirt = transparent;
				this.colorUnderShirt = transparent;
				this.colorPants = transparent;
				this.colorShoes = transparent;
				this.colorLegs = transparent;
				this.bodyGlowColor = transparent;
				this.armGlowColor = transparent;
				this.legsGlowColor = transparent;
				this.colorDisplayDollSkin = transparent;
			}
			if (this.hideEntirePlayer)
			{
				this.stealth = 1f;
				Color transparent2 = Color.Transparent;
				this.colorArmorHead = transparent2;
				this.colorArmorBody = transparent2;
				this.colorArmorLegs = transparent2;
				this.colorEyeWhites = transparent2;
				this.colorEyes = transparent2;
				this.colorHair = transparent2;
				this.colorHead = transparent2;
				this.colorBodySkin = transparent2;
				this.colorShirt = transparent2;
				this.colorUnderShirt = transparent2;
				this.colorPants = transparent2;
				this.colorShoes = transparent2;
				this.colorLegs = transparent2;
				this.headGlowColor = transparent2;
				this.bodyGlowColor = transparent2;
				this.armGlowColor = transparent2;
				this.legsGlowColor = transparent2;
				this.colorDisplayDollSkin = transparent2;
			}
			if (this.drawPlayer.gravDir == 1f)
			{
				if (this.drawPlayer.direction == 1)
				{
					this.playerEffect = SpriteEffects.None;
					this.itemEffect = SpriteEffects.None;
				}
				else
				{
					this.playerEffect = SpriteEffects.FlipHorizontally;
					this.itemEffect = SpriteEffects.FlipHorizontally;
				}
				if (!this.drawPlayer.dead)
				{
					this.drawPlayer.legPosition.Y = 0f;
					this.drawPlayer.headPosition.Y = 0f;
					this.drawPlayer.bodyPosition.Y = 0f;
				}
			}
			else
			{
				if (this.drawPlayer.direction == 1)
				{
					this.playerEffect = SpriteEffects.FlipVertically;
					this.itemEffect = SpriteEffects.FlipVertically;
				}
				else
				{
					this.playerEffect = (SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically);
					this.itemEffect = (SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically);
				}
				if (!this.drawPlayer.dead)
				{
					this.drawPlayer.legPosition.Y = 6f;
					this.drawPlayer.headPosition.Y = 6f;
					this.drawPlayer.bodyPosition.Y = 6f;
				}
			}
			num = this.heldItem.type;
			if (num <= 3185)
			{
				if (num != 3182 && num - 3184 > 1)
				{
					goto IL_58EB;
				}
			}
			else if (num != 3782)
			{
				if (num != 5118)
				{
					goto IL_58EB;
				}
				if (player.gravDir < 0f)
				{
					this.itemEffect ^= (SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically);
					goto IL_58EB;
				}
				goto IL_58EB;
			}
			this.itemEffect ^= (SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically);
			IL_58EB:
			this.legVect = new Vector2((float)this.drawPlayer.legFrame.Width * 0.5f, (float)this.drawPlayer.legFrame.Height * 0.75f);
			this.bodyVect = new Vector2((float)this.drawPlayer.legFrame.Width * 0.5f, (float)this.drawPlayer.legFrame.Height * 0.5f);
			this.headVect = new Vector2((float)this.drawPlayer.legFrame.Width * 0.5f, (float)this.drawPlayer.legFrame.Height * 0.4f);
			if ((this.drawPlayer.merman || this.drawPlayer.forceMerman) && !this.drawPlayer.hideMerman)
			{
				this.drawPlayer.headRotation = this.drawPlayer.velocity.Y * (float)this.drawPlayer.direction * 0.1f;
				if ((double)this.drawPlayer.headRotation < -0.3)
				{
					this.drawPlayer.headRotation = -0.3f;
				}
				if ((double)this.drawPlayer.headRotation > 0.3)
				{
					this.drawPlayer.headRotation = 0.3f;
				}
			}
			else if (!this.drawPlayer.dead)
			{
				this.drawPlayer.headRotation = 0f;
			}
			Rectangle bodyFrame = this.drawPlayer.bodyFrame;
			bodyFrame = this.drawPlayer.bodyFrame;
			bodyFrame.Y -= 336;
			if (bodyFrame.Y < 0)
			{
				bodyFrame.Y = 0;
			}
			this.hairFrontFrame = bodyFrame;
			this.hairBackFrame = bodyFrame;
			if (this.hideHair)
			{
				this.hairFrontFrame.Height = 0;
				this.hairBackFrame.Height = 0;
			}
			else if (this.backHairDraw)
			{
				int height = 26;
				this.hairFrontFrame.Height = height;
			}
			this.hidesTopSkin = (this.drawPlayer.body == 82 || this.drawPlayer.body == 83 || this.drawPlayer.body == 93 || this.drawPlayer.body == 21 || this.drawPlayer.body == 22);
			this.hidesBottomSkin = (this.drawPlayer.body == 93 || this.drawPlayer.legs == 20 || this.drawPlayer.legs == 21 || this.drawPlayer.legs == 216 || this.drawPlayer.legs == 214 || this.drawPlayer.legs == 215);
			this.drawFloatingTube = (this.drawPlayer.hasFloatingTube && !this.hideEntirePlayer && !this.hideEntirePlayerExceptHelmetsAndFaceAccessories);
			this.drawUnicornHorn = this.drawPlayer.hasUnicornHorn;
			this.drawAngelHalo = this.drawPlayer.hasAngelHalo;
			this.drawFrontAccInNeckAccLayer = false;
			if (this.drawPlayer.front > 0 && (int)this.drawPlayer.front < ArmorIDs.Front.Count)
			{
				if (ArmorIDs.Front.Sets.DrawsInNeckLayerRegardlessOfPlayerFrame[(int)this.drawPlayer.front])
				{
					this.drawFrontAccInNeckAccLayer = true;
				}
				else if (this.drawPlayer.bodyFrame.Y / this.drawPlayer.bodyFrame.Height == 5 && ArmorIDs.Front.Sets.DrawsInNeckLayer[(int)this.drawPlayer.front])
				{
					this.drawFrontAccInNeckAccLayer = true;
				}
			}
			this.mountHandlesHeadDraw = false;
			this.mountDrawsEyelid = false;
			if (this.drawPlayer.mount.Active && this.drawPlayer.mount.Type == 54)
			{
				this.mountHandlesHeadDraw = true;
				this.mountDrawsEyelid = true;
			}
			this.hairOffset = this.drawPlayer.GetHairDrawOffset(this.drawPlayer.hair, this.hatHair);
			this.helmetOffset = this.drawPlayer.GetHelmetDrawOffset(false);
			this.legsOffset = this.drawPlayer.GetLegsDrawOffset();
			this.CreateCompositeData();
		}

		// Token: 0x0600386B RID: 14443 RVA: 0x00637820 File Offset: 0x00635A20
		private void AdjustmentsForWolfMount()
		{
			this.hideEntirePlayer = true;
			this.weaponDrawOrder = WeaponDrawOrder.BehindBackArm;
			Vector2 value = new Vector2((float)(10 + this.drawPlayer.direction * 14), 12f);
			Vector2 vector = this.Position + value;
			this.Position.X = this.Position.X - (float)(this.drawPlayer.direction * 10);
			bool flag = this.heldItem.useStyle == 5 || this.SelectedDrawnProjectile != null;
			bool flag2 = this.heldItem.useStyle == 2;
			bool flag3 = this.heldItem.useStyle == 9;
			bool flag4 = this.drawPlayer.itemAnimation > 0;
			bool flag5 = this.heldItem.fishingPole != 0;
			bool flag6 = this.heldItem.useStyle == 14;
			bool flag7 = this.heldItem.useStyle == 8;
			bool flag8 = this.heldItem.holdStyle == 1;
			bool flag9 = this.heldItem.holdStyle == 2;
			bool flag10 = this.heldItem.holdStyle == 5;
			if (flag2)
			{
				this.ItemLocation += new Vector2((float)(this.drawPlayer.direction * 14), -4f);
				return;
			}
			if (!flag5)
			{
				if (flag3)
				{
					this.ItemLocation += (flag4 ? new Vector2((float)(this.drawPlayer.direction * 18), -4f) : new Vector2((float)(this.drawPlayer.direction * 14), -18f));
					return;
				}
				if (flag10)
				{
					this.ItemLocation += new Vector2((float)(this.drawPlayer.direction * 17), -8f);
					return;
				}
				if (flag8 && this.drawPlayer.itemAnimation == 0)
				{
					this.ItemLocation += new Vector2((float)(this.drawPlayer.direction * 14), -6f);
					return;
				}
				if (flag9 && this.drawPlayer.itemAnimation == 0)
				{
					this.ItemLocation += new Vector2((float)(this.drawPlayer.direction * 17), 4f);
					return;
				}
				if (flag7)
				{
					this.ItemLocation = vector + new Vector2((float)(this.drawPlayer.direction * 12), 2f);
					return;
				}
				if (flag6)
				{
					this.ItemLocation += new Vector2((float)(this.drawPlayer.direction * 5), -2f);
					return;
				}
				if (flag)
				{
					this.ItemLocation += new Vector2((float)(this.drawPlayer.direction * 4), -4f);
					return;
				}
				this.ItemLocation = vector;
			}
		}

		// Token: 0x0600386C RID: 14444 RVA: 0x00637AE4 File Offset: 0x00635CE4
		private void AdjustmentsForVelociraptorMount()
		{
			this.hideEntirePlayerExceptHelmetsAndFaceAccessories = true;
			this.weaponDrawOrder = WeaponDrawOrder.BehindFrontArm;
			this.Position.X = this.Position.X - (float)(this.drawPlayer.direction * 14);
			bool flag = this.drawPlayer.itemAnimation > 0;
			if (this.heldItem.useStyle == 8 && flag)
			{
				this.weaponDrawOrder = WeaponDrawOrder.OverFrontArm;
			}
			this.drawPlayer.ApplyItemPositionOffsetFromMount(ref this.ItemLocation);
		}

		// Token: 0x0600386D RID: 14445 RVA: 0x00637B56 File Offset: 0x00635D56
		private void AdjustmentsForRatMount()
		{
			this.hideEntirePlayer = true;
			this.weaponDrawOrder = WeaponDrawOrder.BehindBackArm;
		}

		// Token: 0x0600386E RID: 14446 RVA: 0x00637B56 File Offset: 0x00635D56
		private void AdjustmentsForBatMount()
		{
			this.hideEntirePlayer = true;
			this.weaponDrawOrder = WeaponDrawOrder.BehindBackArm;
		}

		// Token: 0x0600386F RID: 14447 RVA: 0x00637B56 File Offset: 0x00635D56
		private void AdjustmentsForPixieMount()
		{
			this.hideEntirePlayer = true;
			this.weaponDrawOrder = WeaponDrawOrder.BehindBackArm;
		}

		// Token: 0x06003870 RID: 14448 RVA: 0x00637B68 File Offset: 0x00635D68
		private void CreateCompositeData()
		{
			this.frontShoulderOffset = Vector2.Zero;
			this.backShoulderOffset = Vector2.Zero;
			this.usesCompositeTorso = (this.drawPlayer.body > 0 && this.drawPlayer.body < ArmorIDs.Body.Count && ArmorIDs.Body.Sets.UsesNewFramingCode[this.drawPlayer.body]);
			this.usesCompositeFrontHandAcc = (this.drawPlayer.handon > 0 && (int)this.drawPlayer.handon < ArmorIDs.HandOn.Count && ArmorIDs.HandOn.Sets.UsesNewFramingCode[(int)this.drawPlayer.handon]);
			this.usesCompositeBackHandAcc = (this.drawPlayer.handoff > 0 && (int)this.drawPlayer.handoff < ArmorIDs.HandOff.Count && ArmorIDs.HandOff.Sets.UsesNewFramingCode[(int)this.drawPlayer.handoff]);
			if (this.drawPlayer.body < 1)
			{
				this.usesCompositeTorso = true;
			}
			if (!this.usesCompositeTorso)
			{
				return;
			}
			Point pt = new Point(1, 1);
			Point pt2 = new Point(0, 1);
			Point pt3 = default(Point);
			Point pt4 = default(Point);
			Point point = default(Point);
			int targetFrameNumber = this.drawPlayer.bodyFrame.Y / this.drawPlayer.bodyFrame.Height;
			this.compShoulderOverFrontArm = true;
			this.hideCompositeShoulders = false;
			bool flag = true;
			if (this.drawPlayer.body > 0)
			{
				flag = ArmorIDs.Body.Sets.showsShouldersWhileJumping[this.drawPlayer.body];
			}
			if (this.drawPlayer.coat > 0)
			{
				this.hideCompositeShoulders = true;
			}
			if (this.drawPlayer.front > 0 && ArmorIDs.Front.Sets.HidesCompositeShoulders[(int)this.drawPlayer.front])
			{
				this.hideCompositeShoulders = true;
			}
			bool flag2 = false;
			if (this.drawPlayer.handon > 0)
			{
				flag2 = ArmorIDs.HandOn.Sets.UsesOldFramingTexturesForWalking[(int)this.drawPlayer.handon];
			}
			bool flag3 = !flag2;
			switch (targetFrameNumber)
			{
			case 0:
				point.X = 2;
				flag3 = true;
				break;
			case 1:
				point.X = 3;
				this.compShoulderOverFrontArm = false;
				flag3 = true;
				break;
			case 2:
				point.X = 4;
				this.compShoulderOverFrontArm = false;
				flag3 = true;
				break;
			case 3:
				point.X = 5;
				this.compShoulderOverFrontArm = true;
				flag3 = true;
				break;
			case 4:
				point.X = 6;
				this.compShoulderOverFrontArm = true;
				flag3 = true;
				break;
			case 5:
				point.X = 2;
				point.Y = 1;
				pt3.X = 1;
				this.compShoulderOverFrontArm = false;
				flag3 = true;
				if (!flag)
				{
					this.hideCompositeShoulders = true;
				}
				break;
			case 6:
				point.X = 3;
				point.Y = 1;
				break;
			case 7:
			case 8:
			case 9:
			case 10:
				point.X = 4;
				point.Y = 1;
				break;
			case 11:
			case 12:
			case 13:
				point.X = 3;
				point.Y = 1;
				break;
			case 14:
				point.X = 5;
				point.Y = 1;
				break;
			case 15:
			case 16:
				point.X = 6;
				point.Y = 1;
				break;
			case 17:
				point.X = 5;
				point.Y = 1;
				break;
			case 18:
			case 19:
				point.X = 3;
				point.Y = 1;
				break;
			}
			this.CreateCompositeData_DetermineShoulderOffsets(this.drawPlayer.body, targetFrameNumber);
			this.backShoulderOffset *= new Vector2((float)this.drawPlayer.direction, this.drawPlayer.gravDir);
			this.frontShoulderOffset *= new Vector2((float)this.drawPlayer.direction, this.drawPlayer.gravDir);
			if (this.drawPlayer.body > 0 && ArmorIDs.Body.Sets.shouldersAreAlwaysInTheBack[this.drawPlayer.body])
			{
				this.compShoulderOverFrontArm = false;
			}
			this.usesCompositeFrontHandAcc = flag3;
			pt4.X = point.X;
			pt4.Y = point.Y + 2;
			this.UpdateCompositeArm(this.drawPlayer.compositeFrontArm, ref this.compositeFrontArmRotation, ref point, 7);
			this.UpdateCompositeArm(this.drawPlayer.compositeBackArm, ref this.compositeBackArmRotation, ref pt4, 8);
			if (!this.drawPlayer.Male)
			{
				pt.Y += 2;
				pt2.Y += 2;
				pt3.Y += 2;
			}
			this.compBackShoulderFrame = this.CreateCompositeFrameRect(pt);
			this.compFrontShoulderFrame = this.CreateCompositeFrameRect(pt2);
			this.compBackArmFrame = this.CreateCompositeFrameRect(pt4);
			this.compFrontArmFrame = this.CreateCompositeFrameRect(point);
			this.compTorsoFrame = this.CreateCompositeFrameRect(pt3);
		}

		// Token: 0x06003871 RID: 14449 RVA: 0x00638018 File Offset: 0x00636218
		private void CreateCompositeData_DetermineShoulderOffsets(int armor, int targetFrameNumber)
		{
			int num = 0;
			if (armor <= 101)
			{
				if (armor != 55)
				{
					if (armor != 71)
					{
						if (armor == 101)
						{
							num = 6;
						}
					}
					else
					{
						num = 2;
					}
				}
				else
				{
					num = 1;
				}
			}
			else if (armor <= 201)
			{
				if (armor != 183)
				{
					if (armor == 201)
					{
						num = 5;
					}
				}
				else
				{
					num = 4;
				}
			}
			else if (armor != 204)
			{
				if (armor == 207)
				{
					num = 7;
				}
			}
			else
			{
				num = 3;
			}
			if (num == 0)
			{
				return;
			}
			switch (num)
			{
			case 1:
				switch (targetFrameNumber)
				{
				case 6:
					this.frontShoulderOffset.X = -2f;
					return;
				case 7:
				case 8:
				case 9:
				case 10:
					this.frontShoulderOffset.X = -4f;
					return;
				case 11:
				case 12:
				case 13:
				case 14:
					this.frontShoulderOffset.X = -2f;
					return;
				case 15:
				case 16:
				case 17:
					break;
				case 18:
				case 19:
					this.frontShoulderOffset.X = -2f;
					return;
				default:
					return;
				}
				break;
			case 2:
				switch (targetFrameNumber)
				{
				case 6:
					this.frontShoulderOffset.X = -2f;
					return;
				case 7:
				case 8:
				case 9:
				case 10:
					this.frontShoulderOffset.X = -4f;
					return;
				case 11:
				case 12:
				case 13:
				case 14:
					this.frontShoulderOffset.X = -2f;
					return;
				case 15:
				case 16:
				case 17:
					break;
				case 18:
				case 19:
					this.frontShoulderOffset.X = -2f;
					return;
				default:
					return;
				}
				break;
			case 3:
				if (targetFrameNumber - 7 <= 2)
				{
					this.frontShoulderOffset.X = -2f;
					return;
				}
				if (targetFrameNumber - 15 > 2)
				{
					return;
				}
				this.frontShoulderOffset.X = 2f;
				return;
			case 4:
				switch (targetFrameNumber)
				{
				case 6:
					this.frontShoulderOffset.X = -2f;
					return;
				case 7:
				case 8:
				case 9:
				case 10:
					this.frontShoulderOffset.X = -4f;
					return;
				case 11:
				case 12:
				case 13:
					this.frontShoulderOffset.X = -2f;
					return;
				case 14:
				case 17:
					break;
				case 15:
				case 16:
					this.frontShoulderOffset.X = 2f;
					return;
				case 18:
				case 19:
					this.frontShoulderOffset.X = -2f;
					return;
				default:
					return;
				}
				break;
			case 5:
				if (targetFrameNumber - 7 <= 3)
				{
					this.frontShoulderOffset.X = -2f;
					return;
				}
				if (targetFrameNumber - 15 > 1)
				{
					return;
				}
				this.frontShoulderOffset.X = 2f;
				return;
			case 6:
				if (targetFrameNumber - 7 <= 3)
				{
					this.frontShoulderOffset.X = -2f;
					return;
				}
				if (targetFrameNumber - 14 > 3)
				{
					return;
				}
				this.frontShoulderOffset.X = 2f;
				return;
			case 7:
				switch (targetFrameNumber)
				{
				case 6:
				case 7:
				case 8:
				case 9:
				case 10:
					this.frontShoulderOffset.X = -2f;
					return;
				case 11:
				case 12:
				case 13:
				case 14:
					this.frontShoulderOffset.X = -2f;
					return;
				case 15:
				case 16:
				case 17:
					break;
				case 18:
				case 19:
					this.frontShoulderOffset.X = -2f;
					break;
				default:
					return;
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06003872 RID: 14450 RVA: 0x0063834F File Offset: 0x0063654F
		private Rectangle CreateCompositeFrameRect(Point pt)
		{
			return new Rectangle(pt.X * 40, pt.Y * 56, 40, 56);
		}

		// Token: 0x06003873 RID: 14451 RVA: 0x0063836C File Offset: 0x0063656C
		private void UpdateCompositeArm(Player.CompositeArmData data, ref float rotation, ref Point frameIndex, int targetX)
		{
			if (!data.enabled)
			{
				rotation = 0f;
				return;
			}
			rotation = data.rotation;
			switch (data.stretch)
			{
			case Player.CompositeArmStretchAmount.Full:
				frameIndex.X = targetX;
				frameIndex.Y = 0;
				return;
			case Player.CompositeArmStretchAmount.None:
				frameIndex.X = targetX;
				frameIndex.Y = 3;
				return;
			case Player.CompositeArmStretchAmount.Quarter:
				frameIndex.X = targetX;
				frameIndex.Y = 2;
				return;
			case Player.CompositeArmStretchAmount.ThreeQuarters:
				frameIndex.X = targetX;
				frameIndex.Y = 1;
				return;
			default:
				return;
			}
		}

		// Token: 0x04005C76 RID: 23670
		public List<DrawData> DrawDataCache;

		// Token: 0x04005C77 RID: 23671
		public List<int> DustCache;

		// Token: 0x04005C78 RID: 23672
		public List<int> GoreCache;

		// Token: 0x04005C79 RID: 23673
		public Player drawPlayer;

		// Token: 0x04005C7A RID: 23674
		public float shadow;

		// Token: 0x04005C7B RID: 23675
		public Vector2 Position;

		// Token: 0x04005C7C RID: 23676
		public int projectileDrawPosition;

		// Token: 0x04005C7D RID: 23677
		public Vector2 ItemLocation;

		// Token: 0x04005C7E RID: 23678
		public int armorAdjust;

		// Token: 0x04005C7F RID: 23679
		public bool missingHand;

		// Token: 0x04005C80 RID: 23680
		public bool missingArm;

		// Token: 0x04005C81 RID: 23681
		public int skinVar;

		// Token: 0x04005C82 RID: 23682
		public bool fullHair;

		// Token: 0x04005C83 RID: 23683
		public bool drawsBackHairWithoutHeadgear;

		// Token: 0x04005C84 RID: 23684
		public bool hatHair;

		// Token: 0x04005C85 RID: 23685
		public bool hideHair;

		// Token: 0x04005C86 RID: 23686
		public int hairDyePacked;

		// Token: 0x04005C87 RID: 23687
		public int skinDyePacked;

		// Token: 0x04005C88 RID: 23688
		public float mountOffSet;

		// Token: 0x04005C89 RID: 23689
		public int cHead;

		// Token: 0x04005C8A RID: 23690
		public int cBody;

		// Token: 0x04005C8B RID: 23691
		public int cLegs;

		// Token: 0x04005C8C RID: 23692
		public int cHandOn;

		// Token: 0x04005C8D RID: 23693
		public int cHandOff;

		// Token: 0x04005C8E RID: 23694
		public int cBack;

		// Token: 0x04005C8F RID: 23695
		public int cFront;

		// Token: 0x04005C90 RID: 23696
		public int cShoe;

		// Token: 0x04005C91 RID: 23697
		public int cFlameWaker;

		// Token: 0x04005C92 RID: 23698
		public int cWaist;

		// Token: 0x04005C93 RID: 23699
		public int cShield;

		// Token: 0x04005C94 RID: 23700
		public int cNeck;

		// Token: 0x04005C95 RID: 23701
		public int cFace;

		// Token: 0x04005C96 RID: 23702
		public int cBalloon;

		// Token: 0x04005C97 RID: 23703
		public int cWings;

		// Token: 0x04005C98 RID: 23704
		public int cCarpet;

		// Token: 0x04005C99 RID: 23705
		public int cPortableStool;

		// Token: 0x04005C9A RID: 23706
		public int cFloatingTube;

		// Token: 0x04005C9B RID: 23707
		public int cUnicornHorn;

		// Token: 0x04005C9C RID: 23708
		public int cAngelHalo;

		// Token: 0x04005C9D RID: 23709
		public int cBeard;

		// Token: 0x04005C9E RID: 23710
		public int cLeinShampoo;

		// Token: 0x04005C9F RID: 23711
		public int cBackpack;

		// Token: 0x04005CA0 RID: 23712
		public int cTail;

		// Token: 0x04005CA1 RID: 23713
		public int cFaceHead;

		// Token: 0x04005CA2 RID: 23714
		public int cFaceFlower;

		// Token: 0x04005CA3 RID: 23715
		public int cFaceMask;

		// Token: 0x04005CA4 RID: 23716
		public int cBalloonFront;

		// Token: 0x04005CA5 RID: 23717
		public int cCoat;

		// Token: 0x04005CA6 RID: 23718
		public SpriteEffects playerEffect;

		// Token: 0x04005CA7 RID: 23719
		public SpriteEffects itemEffect;

		// Token: 0x04005CA8 RID: 23720
		public Color colorHair;

		// Token: 0x04005CA9 RID: 23721
		public Color colorEyeWhites;

		// Token: 0x04005CAA RID: 23722
		public Color colorEyes;

		// Token: 0x04005CAB RID: 23723
		public Color colorHead;

		// Token: 0x04005CAC RID: 23724
		public Color colorBodySkin;

		// Token: 0x04005CAD RID: 23725
		public Color colorLegs;

		// Token: 0x04005CAE RID: 23726
		public Color colorShirt;

		// Token: 0x04005CAF RID: 23727
		public Color colorUnderShirt;

		// Token: 0x04005CB0 RID: 23728
		public Color colorPants;

		// Token: 0x04005CB1 RID: 23729
		public Color colorShoes;

		// Token: 0x04005CB2 RID: 23730
		public Color colorArmorHead;

		// Token: 0x04005CB3 RID: 23731
		public Color colorArmorBody;

		// Token: 0x04005CB4 RID: 23732
		public Color colorMount;

		// Token: 0x04005CB5 RID: 23733
		public Color colorArmorLegs;

		// Token: 0x04005CB6 RID: 23734
		public Color colorElectricity;

		// Token: 0x04005CB7 RID: 23735
		public Color colorDisplayDollSkin;

		// Token: 0x04005CB8 RID: 23736
		public int headGlowMask;

		// Token: 0x04005CB9 RID: 23737
		public int bodyGlowMask;

		// Token: 0x04005CBA RID: 23738
		public int armGlowMask;

		// Token: 0x04005CBB RID: 23739
		public int legsGlowMask;

		// Token: 0x04005CBC RID: 23740
		public Color headGlowColor;

		// Token: 0x04005CBD RID: 23741
		public Color bodyGlowColor;

		// Token: 0x04005CBE RID: 23742
		public Color armGlowColor;

		// Token: 0x04005CBF RID: 23743
		public Color legsGlowColor;

		// Token: 0x04005CC0 RID: 23744
		public Color ArkhalisColor;

		// Token: 0x04005CC1 RID: 23745
		public float stealth;

		// Token: 0x04005CC2 RID: 23746
		public Vector2 legVect;

		// Token: 0x04005CC3 RID: 23747
		public Vector2 bodyVect;

		// Token: 0x04005CC4 RID: 23748
		public Vector2 headVect;

		// Token: 0x04005CC5 RID: 23749
		public Color selectionGlowColor;

		// Token: 0x04005CC6 RID: 23750
		public float torsoOffset;

		// Token: 0x04005CC7 RID: 23751
		public bool hidesTopSkin;

		// Token: 0x04005CC8 RID: 23752
		public bool hidesBottomSkin;

		// Token: 0x04005CC9 RID: 23753
		public float rotation;

		// Token: 0x04005CCA RID: 23754
		public Vector2 rotationOrigin;

		// Token: 0x04005CCB RID: 23755
		public Rectangle hairFrontFrame;

		// Token: 0x04005CCC RID: 23756
		public Rectangle hairBackFrame;

		// Token: 0x04005CCD RID: 23757
		public bool backHairDraw;

		// Token: 0x04005CCE RID: 23758
		public Color itemColor;

		// Token: 0x04005CCF RID: 23759
		public bool usesCompositeTorso;

		// Token: 0x04005CD0 RID: 23760
		public bool usesCompositeFrontHandAcc;

		// Token: 0x04005CD1 RID: 23761
		public bool usesCompositeBackHandAcc;

		// Token: 0x04005CD2 RID: 23762
		public bool compShoulderOverFrontArm;

		// Token: 0x04005CD3 RID: 23763
		public Rectangle compBackShoulderFrame;

		// Token: 0x04005CD4 RID: 23764
		public Rectangle compFrontShoulderFrame;

		// Token: 0x04005CD5 RID: 23765
		public Rectangle compBackArmFrame;

		// Token: 0x04005CD6 RID: 23766
		public Rectangle compFrontArmFrame;

		// Token: 0x04005CD7 RID: 23767
		public Rectangle compTorsoFrame;

		// Token: 0x04005CD8 RID: 23768
		public float compositeBackArmRotation;

		// Token: 0x04005CD9 RID: 23769
		public float compositeFrontArmRotation;

		// Token: 0x04005CDA RID: 23770
		public bool hideCompositeShoulders;

		// Token: 0x04005CDB RID: 23771
		public Vector2 frontShoulderOffset;

		// Token: 0x04005CDC RID: 23772
		public Vector2 backShoulderOffset;

		// Token: 0x04005CDD RID: 23773
		public WeaponDrawOrder weaponDrawOrder;

		// Token: 0x04005CDE RID: 23774
		public bool weaponOverFrontArm;

		// Token: 0x04005CDF RID: 23775
		public bool isSitting;

		// Token: 0x04005CE0 RID: 23776
		public bool isSleeping;

		// Token: 0x04005CE1 RID: 23777
		public float seatYOffset;

		// Token: 0x04005CE2 RID: 23778
		public int sittingIndex;

		// Token: 0x04005CE3 RID: 23779
		public bool drawFrontAccInNeckAccLayer;

		// Token: 0x04005CE4 RID: 23780
		public bool drawFrontAccInNeckAccLayerAlways;

		// Token: 0x04005CE5 RID: 23781
		public bool mountHandlesHeadDraw;

		// Token: 0x04005CE6 RID: 23782
		public bool mountDrawsEyelid;

		// Token: 0x04005CE7 RID: 23783
		public Item heldItem;

		// Token: 0x04005CE8 RID: 23784
		public bool drawFloatingTube;

		// Token: 0x04005CE9 RID: 23785
		public bool drawUnicornHorn;

		// Token: 0x04005CEA RID: 23786
		public bool drawAngelHalo;

		// Token: 0x04005CEB RID: 23787
		public Color floatingTubeColor;

		// Token: 0x04005CEC RID: 23788
		public Vector2 hairOffset;

		// Token: 0x04005CED RID: 23789
		public Vector2 helmetOffset;

		// Token: 0x04005CEE RID: 23790
		public Vector2 legsOffset;

		// Token: 0x04005CEF RID: 23791
		public bool hideEntirePlayer;

		// Token: 0x04005CF0 RID: 23792
		public bool hideEntirePlayerExceptHelmetsAndFaceAccessories;

		// Token: 0x04005CF1 RID: 23793
		public Projectile SelectedDrawnProjectile;
	}
}
