using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;

namespace Terraria.DataStructures
{
	// Token: 0x0200059E RID: 1438
	public struct PlayerDrawHeadSet
	{
		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x060038CF RID: 14543 RVA: 0x0064CD88 File Offset: 0x0064AF88
		public Rectangle HairFrame
		{
			get
			{
				Rectangle result = this.bodyFrameMemory;
				result.Height--;
				return result;
			}
		}

		// Token: 0x060038D0 RID: 14544 RVA: 0x0064CDAC File Offset: 0x0064AFAC
		public void BoringSetup(Player drawPlayer2, List<DrawData> drawData, List<int> dust, List<int> gore, float X, float Y, float Alpha, float Scale)
		{
			this.DrawData = drawData;
			this.Dust = dust;
			this.Gore = gore;
			this.drawPlayer = drawPlayer2;
			this.Position = this.drawPlayer.position;
			this.cHead = 0;
			this.cFace = 0;
			this.cUnicornHorn = 0;
			this.cAngelHalo = 0;
			this.cBeard = 0;
			this.drawUnicornHorn = false;
			this.drawAngelHalo = false;
			this.skinVar = this.drawPlayer.skinVariant;
			this.hairShaderPacked = PlayerDrawHelper.PackShader((int)this.drawPlayer.hairDye, PlayerDrawHelper.ShaderConfiguration.HairShader);
			if (this.drawPlayer.head == 0 && this.drawPlayer.hairDye == 0)
			{
				this.hairShaderPacked = PlayerDrawHelper.PackShader(1, PlayerDrawHelper.ShaderConfiguration.HairShader);
			}
			this.skinDyePacked = this.drawPlayer.skinDyePacked;
			if (this.drawPlayer.face > 0 && this.drawPlayer.face < ArmorIDs.Face.Count)
			{
				Main.instance.LoadAccFace((int)this.drawPlayer.face);
			}
			this.cHead = this.drawPlayer.cHead;
			this.cFace = this.drawPlayer.cFace;
			this.cFaceHead = this.drawPlayer.cFaceHead;
			this.cFaceFlower = this.drawPlayer.cFaceFlower;
			this.cFaceMask = this.drawPlayer.cFaceMask;
			this.cUnicornHorn = this.drawPlayer.cUnicornHorn;
			this.cAngelHalo = this.drawPlayer.cAngelHalo;
			this.cBeard = this.drawPlayer.cBeard;
			this.drawUnicornHorn = this.drawPlayer.hasUnicornHorn;
			this.drawAngelHalo = this.drawPlayer.hasAngelHalo;
			Main.instance.LoadHair(this.drawPlayer.hair);
			this.scale = Scale;
			this.colorEyeWhites = Main.quickAlpha(Color.White, Alpha);
			this.colorEyes = Main.quickAlpha(this.drawPlayer.eyeColor, Alpha);
			this.colorHair = Main.quickAlpha(this.drawPlayer.GetHairColor(false), Alpha);
			this.colorHead = Main.quickAlpha(this.drawPlayer.skinColor, Alpha);
			this.colorArmorHead = Main.quickAlpha(Color.White, Alpha);
			if (this.drawPlayer.isDisplayDollOrInanimate)
			{
				this.colorDisplayDollSkin = Main.quickAlpha(PlayerDrawHelper.DISPLAY_DOLL_DEFAULT_SKIN_COLOR, Alpha);
			}
			else
			{
				this.colorDisplayDollSkin = this.colorHead;
			}
			this.playerEffect = SpriteEffects.None;
			if (this.drawPlayer.direction < 0)
			{
				this.playerEffect = SpriteEffects.FlipHorizontally;
			}
			this.headVect = new Vector2((float)this.drawPlayer.legFrame.Width * 0.5f, (float)this.drawPlayer.legFrame.Height * 0.4f);
			this.bodyFrameMemory = this.drawPlayer.bodyFrame;
			this.bodyFrameMemory.Y = 0;
			this.Position = Main.screenPosition;
			this.Position.X = this.Position.X + X;
			this.Position.Y = this.Position.Y + Y;
			this.Position.X = this.Position.X - 6f;
			this.Position.Y = this.Position.Y - 4f;
			this.Position.Y = this.Position.Y - (float)this.drawPlayer.HeightMapOffset;
			if (this.drawPlayer.head > 0 && this.drawPlayer.head < ArmorIDs.Head.Count)
			{
				Main.instance.LoadArmorHead(this.drawPlayer.head);
				int num = ArmorIDs.Head.Sets.FrontToBackID[this.drawPlayer.head];
				if (num >= 0)
				{
					Main.instance.LoadArmorHead(num);
				}
			}
			if (this.drawPlayer.face > 0 && this.drawPlayer.face < ArmorIDs.Face.Count)
			{
				Main.instance.LoadAccFace((int)this.drawPlayer.face);
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
			if (this.drawPlayer.beard > 0 && this.drawPlayer.beard < ArmorIDs.Beard.Count)
			{
				Main.instance.LoadAccBeard((int)this.drawPlayer.beard);
			}
			bool flag;
			this.drawPlayer.GetHairSettings(out this.fullHair, out this.hatHair, out this.hideHair, out flag, out this.helmetIsOverFullHair);
			this.hairOffset = this.drawPlayer.GetHairDrawOffset(this.drawPlayer.hair, this.hatHair);
			this.hairOffset.Y = this.hairOffset.Y * this.drawPlayer.Directions.Y;
			this.helmetOffset = this.drawPlayer.GetHelmetDrawOffset(true);
			this.helmetOffset.Y = this.helmetOffset.Y * this.drawPlayer.Directions.Y;
			this.helmetIsTall = (this.drawPlayer.head == 14 || this.drawPlayer.head == 56 || this.drawPlayer.head == 158);
			this.helmetIsNormal = (!this.helmetIsTall && !this.helmetIsOverFullHair && this.drawPlayer.head > 0 && this.drawPlayer.head < ArmorIDs.Head.Count && this.drawPlayer.head != 28);
		}

		// Token: 0x04005CFA RID: 23802
		public List<DrawData> DrawData;

		// Token: 0x04005CFB RID: 23803
		public List<int> Dust;

		// Token: 0x04005CFC RID: 23804
		public List<int> Gore;

		// Token: 0x04005CFD RID: 23805
		public Player drawPlayer;

		// Token: 0x04005CFE RID: 23806
		public int cHead;

		// Token: 0x04005CFF RID: 23807
		public int cFace;

		// Token: 0x04005D00 RID: 23808
		public int cFaceHead;

		// Token: 0x04005D01 RID: 23809
		public int cFaceFlower;

		// Token: 0x04005D02 RID: 23810
		public int cFaceMask;

		// Token: 0x04005D03 RID: 23811
		public int cUnicornHorn;

		// Token: 0x04005D04 RID: 23812
		public int cAngelHalo;

		// Token: 0x04005D05 RID: 23813
		public int cBeard;

		// Token: 0x04005D06 RID: 23814
		public int skinVar;

		// Token: 0x04005D07 RID: 23815
		public int hairShaderPacked;

		// Token: 0x04005D08 RID: 23816
		public int skinDyePacked;

		// Token: 0x04005D09 RID: 23817
		public float scale;

		// Token: 0x04005D0A RID: 23818
		public Color colorEyeWhites;

		// Token: 0x04005D0B RID: 23819
		public Color colorEyes;

		// Token: 0x04005D0C RID: 23820
		public Color colorHair;

		// Token: 0x04005D0D RID: 23821
		public Color colorHead;

		// Token: 0x04005D0E RID: 23822
		public Color colorArmorHead;

		// Token: 0x04005D0F RID: 23823
		public Color colorDisplayDollSkin;

		// Token: 0x04005D10 RID: 23824
		public SpriteEffects playerEffect;

		// Token: 0x04005D11 RID: 23825
		public Vector2 headVect;

		// Token: 0x04005D12 RID: 23826
		public Rectangle bodyFrameMemory;

		// Token: 0x04005D13 RID: 23827
		public bool fullHair;

		// Token: 0x04005D14 RID: 23828
		public bool hatHair;

		// Token: 0x04005D15 RID: 23829
		public bool hideHair;

		// Token: 0x04005D16 RID: 23830
		public bool helmetIsTall;

		// Token: 0x04005D17 RID: 23831
		public bool helmetIsOverFullHair;

		// Token: 0x04005D18 RID: 23832
		public bool helmetIsNormal;

		// Token: 0x04005D19 RID: 23833
		public bool drawUnicornHorn;

		// Token: 0x04005D1A RID: 23834
		public bool drawAngelHalo;

		// Token: 0x04005D1B RID: 23835
		public Vector2 Position;

		// Token: 0x04005D1C RID: 23836
		public Vector2 hairOffset;

		// Token: 0x04005D1D RID: 23837
		public Vector2 helmetOffset;
	}
}
