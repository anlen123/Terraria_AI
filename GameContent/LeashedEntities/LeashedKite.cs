using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ID;

namespace Terraria.GameContent.LeashedEntities
{
	// Token: 0x02000466 RID: 1126
	public class LeashedKite : LeashedEntity
	{
		// Token: 0x060032AE RID: 12974 RVA: 0x005F0D60 File Offset: 0x005EEF60
		public void SetDefaults(int projType)
		{
			this.projType = projType;
			LeashedKite._dummy.SetDefaults(projType);
			base.Size = LeashedKite._dummy.Size;
		}

		// Token: 0x060032AF RID: 12975 RVA: 0x005F0D84 File Offset: 0x005EEF84
		public override void NetSend(BinaryWriter writer, bool full)
		{
			if (full)
			{
				writer.Write7BitEncodedInt(this.projType);
			}
			writer.WriteVector2(this.position);
			writer.WritePackedVector2(this.velocity);
			writer.Write((byte)((double)(this.rotation * 256f) / 6.283185307179586));
			writer.Write(this.windTarget);
			writer.Write(this.cloudAlpha);
			writer.Write(this.timeCounter);
		}

		// Token: 0x060032B0 RID: 12976 RVA: 0x005F0DFC File Offset: 0x005EEFFC
		public override void NetReceive(BinaryReader reader, bool full)
		{
			if (full)
			{
				this.SetDefaults(reader.Read7BitEncodedInt());
			}
			Vector2 position = this.position;
			this.position = reader.ReadVector2();
			this.velocity = reader.ReadPackedVector2();
			this.rotation = (float)((double)reader.ReadByte() * 3.141592653589793 * 2.0 / 256.0);
			this.windTarget = reader.ReadSingle();
			this.cloudAlpha = reader.ReadSingle();
			this.timeCounter = reader.ReadSingle();
			if (full)
			{
				this.netOffset = Vector2.Zero;
			}
			else
			{
				this.netOffset += position - this.position;
			}
			if (full)
			{
				this.Update();
			}
		}

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x060032B1 RID: 12977 RVA: 0x005F0EBD File Offset: 0x005EF0BD
		private Vector2 AnchorWorldPosition
		{
			get
			{
				return base.AnchorPosition.ToWorldCoordinates(8f, 8f);
			}
		}

		// Token: 0x060032B2 RID: 12978 RVA: 0x005F0ED4 File Offset: 0x005EF0D4
		public override void Draw()
		{
			Main.instance.LoadProjectile(this.projType);
			this.CopyToDummy();
			LeashedKite._dummy.position += this.netOffset;
			Main.DrawKite(LeashedKite._dummy, this.AnchorWorldPosition);
		}

		// Token: 0x060032B3 RID: 12979 RVA: 0x005F0F22 File Offset: 0x005EF122
		public override void Update()
		{
			this.Update(false);
		}

		// Token: 0x060032B4 RID: 12980 RVA: 0x005F0F2C File Offset: 0x005EF12C
		public void Update(bool fastForward)
		{
			if (this.oldPos == null)
			{
				int num = ProjectileID.Sets.TrailCacheLength[this.projType];
				this.oldPos = new Vector2[num];
				this.oldRot = new float[num];
				this.oldSpriteDirection = new int[num];
			}
			if (base.NearbySectionsMissing(3))
			{
				return;
			}
			if (fastForward || Vector2.DistanceSquared(this.position, this.oldPos[0]) > 256f)
			{
				for (int i = 0; i < this.oldPos.Length; i++)
				{
					this.oldPos[i] = this.position;
					this.oldRot[i] = this.rotation;
					this.oldSpriteDirection[i] = this.spriteDirection;
				}
			}
			if (Main.netMode != 1)
			{
				this.windTarget = Main.WindForVisuals;
				this.cloudAlpha = Main.cloudAlpha;
			}
			this.windCurrent = (fastForward ? this.windTarget : MathHelper.Lerp(this.windCurrent, this.windTarget, 0.05f));
			this.timeWithoutWind = ((Math.Abs(this.windCurrent) >= 0.2f) ? 0 : (fastForward ? 3600 : (this.timeWithoutWind + 1)));
			this.kiteDistance = Utils.Remap((float)this.timeWithoutWind, 120f, 420f, 250f, 48f, true);
			this.MoveKite(fastForward);
			this.netOffset = this.netOffset.MoveTowards(Vector2.Zero, 2f);
		}

		// Token: 0x060032B5 RID: 12981 RVA: 0x005F10A0 File Offset: 0x005EF2A0
		private void MoveKite(bool fastForward = false)
		{
			this.CopyToDummy();
			LeashedKite._dummy.owner = 255;
			Player player = Main.player[255];
			Vector2 anchorWorldPosition = this.AnchorWorldPosition;
			player.Center = anchorWorldPosition;
			if (this.timeWithoutWind == 0)
			{
				int direction = (LeashedKite._dummy.Center.X - anchorWorldPosition.X < 0f) ? -1 : 1;
				LeashedKite._dummy.spriteDirection = direction;
				player.direction = direction;
			}
			this.timeCounter += 0.016666668f;
			KiteFlyingInfo kiteFlyingInfo = new KiteFlyingInfo
			{
				BobOffset = (anchorWorldPosition.X + anchorWorldPosition.Y * 0.92f) * 0.0025f,
				WindInWorld = this.windCurrent,
				CloudAlpha = this.cloudAlpha,
				GlobalTime = this.timeCounter,
				CanReelThroughBlocks = false
			};
			if (fastForward)
			{
				LeashedKite._dummy.KiteLogic(anchorWorldPosition, kiteFlyingInfo);
				this.timeCounter = 6f;
				Vector2 vector = new Vector2(kiteFlyingInfo.WindInWorld, -2f).SafeNormalize(Vector2.Zero) * this.kiteDistance;
				Vector2 position = LeashedKite._dummy.position;
				LeashedKite._dummy.velocity = vector;
				LeashedKite._dummy.HandleMovement(vector);
				LeashedKite._dummy.position = LeashedKite._dummy.position.MoveTowards(position, 1f);
				if (LeashedKite._dummy.velocity.Length() > 4f)
				{
					LeashedKite._dummy.velocity = LeashedKite._dummy.velocity.SafeNormalize(Vector2.Zero) * 4f;
				}
				LeashedKite._dummy.KiteLogic(anchorWorldPosition, kiteFlyingInfo);
				for (int i = this.oldPos.Length - 1; i >= 0; i--)
				{
					this.oldPos[i] = LeashedKite._dummy.position;
					this.oldRot[i] = LeashedKite._dummy.rotation;
					this.oldSpriteDirection[i] = LeashedKite._dummy.spriteDirection;
				}
			}
			else
			{
				Utils.Shift<Vector2>(this.oldPos, 1);
				Utils.Shift<float>(this.oldRot, 1);
				Utils.Shift<int>(this.oldSpriteDirection, 1);
				this.oldPos[0] = this.position;
				this.oldRot[0] = this.rotation;
				this.oldSpriteDirection[0] = this.spriteDirection;
				LeashedKite._dummy.KiteLogic(anchorWorldPosition, kiteFlyingInfo);
				LeashedKite._dummy.HandleMovement(LeashedKite._dummy.velocity);
				Vector2 value;
				int num;
				int num2;
				LeashedKite._dummy.GetCollisionParams(out value, out num, out num2);
				if (Collision.SolidFullTiles(LeashedKite._dummy.position + LeashedKite._dummy.Size / 2f - new Vector2((float)num, (float)num2) * value, new Vector2((float)num, (float)num2)))
				{
					LeashedKite._dummy.Bottom = LeashedKite._dummy.Bottom.MoveTowards(anchorWorldPosition, 2f);
				}
			}
			this.CopyFromDummy();
		}

		// Token: 0x060032B6 RID: 12982 RVA: 0x005F13A4 File Offset: 0x005EF5A4
		public override void Spawn(bool newlyAdded)
		{
			base.Center = this.AnchorWorldPosition;
			this.velocity = new Vector2(0f, -5f);
			this.Update(!newlyAdded);
			this.windCurrent = (this.windTarget = Main.WindForVisuals);
			this.cloudAlpha = Main.cloudAlpha;
		}

		// Token: 0x060032B7 RID: 12983 RVA: 0x005F13FC File Offset: 0x005EF5FC
		private void CopyToDummy()
		{
			LeashedKite._dummy.type = this.projType;
			LeashedKite._dummy.Size = base.Size;
			LeashedKite._dummy.frame = this.frame;
			LeashedKite._dummy.frameCounter = this.frameCounter;
			LeashedKite._dummy.position = this.position;
			LeashedKite._dummy.velocity = this.velocity;
			LeashedKite._dummy.rotation = this.rotation;
			LeashedKite._dummy.spriteDirection = this.spriteDirection;
			LeashedKite._dummy.oldPos = this.oldPos;
			LeashedKite._dummy.oldRot = this.oldRot;
			LeashedKite._dummy.oldSpriteDirection = this.oldSpriteDirection;
			LeashedKite._dummy.scale = 1f;
			LeashedKite._dummy.ai[0] = this.kiteDistance;
			LeashedKite._dummy.localAI[0] = this.projectileLocalAI0;
			LeashedKite._dummy.localAI[1] = this.projectileLocalAI1;
			LeashedKite._dummy.extraUpdates = 0;
		}

		// Token: 0x060032B8 RID: 12984 RVA: 0x005F150C File Offset: 0x005EF70C
		private void CopyFromDummy()
		{
			this.frame = LeashedKite._dummy.frame;
			this.frameCounter = LeashedKite._dummy.frameCounter;
			this.position = LeashedKite._dummy.position;
			this.velocity = LeashedKite._dummy.velocity;
			this.rotation = LeashedKite._dummy.rotation;
			this.spriteDirection = LeashedKite._dummy.spriteDirection;
			this.projectileLocalAI0 = LeashedKite._dummy.localAI[0];
			this.projectileLocalAI1 = LeashedKite._dummy.localAI[1];
		}

		// Token: 0x04005826 RID: 22566
		public static LeashedKite Prototype;

		// Token: 0x04005827 RID: 22567
		private static Projectile _dummy = new Projectile();

		// Token: 0x04005828 RID: 22568
		public int projType;

		// Token: 0x04005829 RID: 22569
		public int frame;

		// Token: 0x0400582A RID: 22570
		public int frameCounter;

		// Token: 0x0400582B RID: 22571
		public float rotation;

		// Token: 0x0400582C RID: 22572
		public int spriteDirection = 1;

		// Token: 0x0400582D RID: 22573
		public float kiteDistance = 250f;

		// Token: 0x0400582E RID: 22574
		public float windTarget;

		// Token: 0x0400582F RID: 22575
		public float windCurrent;

		// Token: 0x04005830 RID: 22576
		public float timeCounter;

		// Token: 0x04005831 RID: 22577
		public float cloudAlpha;

		// Token: 0x04005832 RID: 22578
		public int timeWithoutWind;

		// Token: 0x04005833 RID: 22579
		public float projectileLocalAI0;

		// Token: 0x04005834 RID: 22580
		public float projectileLocalAI1;

		// Token: 0x04005835 RID: 22581
		public Vector2[] oldPos;

		// Token: 0x04005836 RID: 22582
		public float[] oldRot;

		// Token: 0x04005837 RID: 22583
		public int[] oldSpriteDirection;

		// Token: 0x04005838 RID: 22584
		public Vector2 netOffset;
	}
}
