using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Terraria.Cinematics
{
	// Token: 0x020005AD RID: 1453
	public class DSTFilm : Film
	{
		// Token: 0x06003979 RID: 14713 RVA: 0x00651D80 File Offset: 0x0064FF80
		public DSTFilm()
		{
			this.BuildSequence();
		}

		// Token: 0x0600397A RID: 14714 RVA: 0x00651D8E File Offset: 0x0064FF8E
		public override void OnBegin()
		{
			this.PrepareScene();
			Main.hideUI = true;
			base.OnBegin();
		}

		// Token: 0x0600397B RID: 14715 RVA: 0x00651DA2 File Offset: 0x0064FFA2
		public override void OnEnd()
		{
			this.ClearScene();
			Main.hideUI = false;
			base.OnEnd();
		}

		// Token: 0x0600397C RID: 14716 RVA: 0x00651DB8 File Offset: 0x0064FFB8
		private void BuildSequence()
		{
			base.AppendKeyFrames(new FrameEvent[]
			{
				new FrameEvent(this.EquipDSTShaderItem)
			});
			base.AppendEmptySequence(60);
			base.AppendKeyFrames(new FrameEvent[]
			{
				new FrameEvent(this.CreateDeerclops),
				new FrameEvent(this.CreateChester),
				new FrameEvent(this.ControlPlayer)
			});
			base.AppendEmptySequence(60);
			base.AppendEmptySequence(187);
			base.AppendKeyFrames(new FrameEvent[]
			{
				new FrameEvent(this.StopBeforeCliff)
			});
			base.AppendEmptySequence(20);
			base.AppendKeyFrames(new FrameEvent[]
			{
				new FrameEvent(this.TurnPlayerToTheLeft)
			});
			base.AppendEmptySequence(20);
			base.AppendKeyFrames(new FrameEvent[]
			{
				new FrameEvent(this.DeerclopsAttack)
			});
			base.AppendEmptySequence(60);
			base.AppendKeyFrames(new FrameEvent[]
			{
				new FrameEvent(this.RemoveDSTShaderItem)
			});
		}

		// Token: 0x0600397D RID: 14717 RVA: 0x00651EB8 File Offset: 0x006500B8
		private void PrepareScene()
		{
			Main.dayTime = true;
			Main.time = 13500.0;
			Main.time = 43638.0;
			Main.windSpeedCurrent = (Main.windSpeedTarget = 0.36799997f);
			Main.windCounter = 2011;
			Main.cloudAlpha = 0f;
			Main.raining = true;
			Main.rainTime = 3600;
			Main.maxRaining = (Main.oldMaxRaining = (Main.cloudAlpha = 0.9f));
			Main.raining = true;
			Main.maxRaining = (Main.oldMaxRaining = (Main.cloudAlpha = 0.6f));
			Main.raining = true;
			Main.maxRaining = (Main.oldMaxRaining = (Main.cloudAlpha = 0.6f));
			this._startPoint = new Point(4050, 488).ToWorldCoordinates(8f, 8f);
			this._startPoint -= new Vector2(1280f, 0f);
		}

		// Token: 0x0600397E RID: 14718 RVA: 0x00651FAD File Offset: 0x006501AD
		private void ClearScene()
		{
			if (this._deerclops != null)
			{
				this._deerclops.active = false;
			}
			if (this._chester != null)
			{
				this._chester.active = false;
			}
			Main.LocalPlayer.isControlledByFilm = false;
		}

		// Token: 0x0600397F RID: 14719 RVA: 0x00651FE4 File Offset: 0x006501E4
		private void EquipDSTShaderItem(FrameEventData evt)
		{
			this._oldItem = Main.LocalPlayer.armor[3];
			Item item = new Item();
			item.SetDefaults(5113, null);
			Main.LocalPlayer.armor[3] = item;
		}

		// Token: 0x06003980 RID: 14720 RVA: 0x00652022 File Offset: 0x00650222
		private void RemoveDSTShaderItem(FrameEventData evt)
		{
			Main.LocalPlayer.armor[3] = this._oldItem;
		}

		// Token: 0x06003981 RID: 14721 RVA: 0x00652038 File Offset: 0x00650238
		private void CreateDeerclops(FrameEventData evt)
		{
			this._deerclops = this.PlaceNPCOnGround(668, this._startPoint);
			this._deerclops.immortal = true;
			this._deerclops.dontTakeDamage = true;
			this._deerclops.takenDamageMultiplier = 0f;
			this._deerclops.immune[255] = 100000;
			this._deerclops.immune[Main.myPlayer] = 100000;
			this._deerclops.ai[0] = -1f;
			this._deerclops.velocity.Y = 4f;
			this._deerclops.velocity.X = 6f;
			NPC deerclops = this._deerclops;
			deerclops.position.X = deerclops.position.X - 24f;
			this._deerclops.direction = (this._deerclops.spriteDirection = 1);
		}

		// Token: 0x06003982 RID: 14722 RVA: 0x00652120 File Offset: 0x00650320
		private NPC PlaceNPCOnGround(int type, Vector2 position)
		{
			int x;
			int num;
			DSTFilm.FindFloorAt(position, out x, out num);
			if (type == 668)
			{
				num -= 240;
			}
			int start = 100;
			int num2 = NPC.NewNPC(new EntitySource_Film(), x, num, type, start, 0f, 0f, 0f, 0f, 255);
			return Main.npc[num2];
		}

		// Token: 0x06003983 RID: 14723 RVA: 0x0065217C File Offset: 0x0065037C
		private void CreateChester(FrameEventData evt)
		{
			int num;
			int num2;
			DSTFilm.FindFloorAt(this._startPoint + new Vector2(110f, 0f), out num, out num2);
			num2 -= 240;
			int num3 = Projectile.NewProjectile(null, (float)num, (float)num2, 0f, 0f, 960, 0, 0f, Main.myPlayer, -1f, 0f, 0f, null);
			this._chester = Main.projectile[num3];
			this._chester.velocity.Y = 4f;
			this._chester.velocity.X = 6f;
		}

		// Token: 0x06003984 RID: 14724 RVA: 0x00652220 File Offset: 0x00650420
		private void ControlPlayer(FrameEventData evt)
		{
			Player localPlayer = Main.LocalPlayer;
			localPlayer.isControlledByFilm = true;
			localPlayer.controlRight = true;
			int num;
			int num2;
			DSTFilm.FindFloorAt(this._startPoint + new Vector2(150f, 0f), out num, out num2);
			localPlayer.BottomLeft = new Vector2((float)num, (float)num2);
			localPlayer.velocity.X = 6f;
		}

		// Token: 0x06003985 RID: 14725 RVA: 0x00652281 File Offset: 0x00650481
		private void StopBeforeCliff(FrameEventData evt)
		{
			Main.LocalPlayer.controlRight = false;
			this._chester.ai[0] = -2f;
		}

		// Token: 0x06003986 RID: 14726 RVA: 0x006522A0 File Offset: 0x006504A0
		private void TurnPlayerToTheLeft(FrameEventData evt)
		{
			Main.LocalPlayer.ChangeDir(-1);
			this._chester.velocity = new Vector2(-0.1f, 0f);
			this._chester.spriteDirection = (this._chester.direction = -1);
			this._deerclops.ai[0] = 1f;
			this._deerclops.ai[1] = 0f;
			this._deerclops.TargetClosest(true);
		}

		// Token: 0x06003987 RID: 14727 RVA: 0x0065231C File Offset: 0x0065051C
		private void DeerclopsAttack(FrameEventData evt)
		{
			Main.LocalPlayer.controlJump = true;
			this._chester.velocity.Y = -11.4f;
			this._deerclops.ai[0] = 1f;
			this._deerclops.ai[1] = 0f;
			this._deerclops.TargetClosest(true);
		}

		// Token: 0x06003988 RID: 14728 RVA: 0x0065237C File Offset: 0x0065057C
		private static void FindFloorAt(Vector2 position, out int x, out int y)
		{
			x = (int)position.X;
			y = (int)position.Y;
			int i = x / 16;
			int num = y / 16;
			while (!WorldGen.SolidTile(i, num, false))
			{
				num++;
			}
			y = num * 16;
		}

		// Token: 0x04005D76 RID: 23926
		private NPC _deerclops;

		// Token: 0x04005D77 RID: 23927
		private Projectile _chester;

		// Token: 0x04005D78 RID: 23928
		private Vector2 _startPoint;

		// Token: 0x04005D79 RID: 23929
		private Item _oldItem;
	}
}
