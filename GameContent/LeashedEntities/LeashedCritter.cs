using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Utilities;

namespace Terraria.GameContent.LeashedEntities
{
	// Token: 0x02000465 RID: 1125
	public abstract class LeashedCritter : LeashedEntity
	{
		// Token: 0x0600329F RID: 12959 RVA: 0x005F0560 File Offset: 0x005EE760
		public void SetDefaults(int itemType)
		{
			this.SetDefaults(ContentSamples.ItemsByType[itemType]);
		}

		// Token: 0x060032A0 RID: 12960 RVA: 0x005F0574 File Offset: 0x005EE774
		protected virtual void SetDefaults(Item sample)
		{
			this.npcType = (int)sample.makeNPC;
			LeashedCritter._dummy.SetDefaults(this.npcType, default(NPCSpawnParams));
			base.Size = LeashedCritter._dummy.Size;
		}

		// Token: 0x060032A1 RID: 12961 RVA: 0x005F05B8 File Offset: 0x005EE7B8
		public override void NetSend(BinaryWriter writer, bool full)
		{
			if (full)
			{
				writer.Write7BitEncodedInt(this.npcType);
				writer.WriteVector2(base.Size);
			}
			writer.WritePackedVector2(this.position - base.AnchorPosition.ToWorldCoordinates(8f, 8f));
			writer.Write(this.direction > 0);
			writer.Write(this.rand.state);
			writer.Write(this.WaitTime);
			writer.Write(this.State);
			writer.Write((sbyte)(this.TargetPosition.X - base.AnchorPosition.X));
			writer.Write((sbyte)(this.TargetPosition.Y - base.AnchorPosition.Y));
		}

		// Token: 0x060032A2 RID: 12962 RVA: 0x005F067C File Offset: 0x005EE87C
		public override void NetReceive(BinaryReader reader, bool full)
		{
			if (full)
			{
				this.npcType = reader.Read7BitEncodedInt();
				base.Size = reader.ReadVector2();
			}
			Vector2 position = this.position;
			this.position = reader.ReadPackedVector2() + base.AnchorPosition.ToWorldCoordinates(8f, 8f);
			this.direction = (reader.ReadBoolean() ? 1 : -1);
			this.rand.state = reader.ReadUInt32();
			this.WaitTime = reader.ReadInt16();
			this.State = reader.ReadByte();
			this.TargetPosition = new Point16((int)(base.AnchorPosition.X + (short)reader.ReadSByte()), (int)(base.AnchorPosition.Y + (short)reader.ReadSByte()));
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

		// Token: 0x060032A3 RID: 12963 RVA: 0x005F0771 File Offset: 0x005EE971
		public override void Spawn(bool newlyAdded)
		{
			base.Center = base.AnchorPosition.ToWorldCoordinates(8f, 8f);
			this.TargetPosition = base.AnchorPosition;
			this.rand = new LCG32Random((uint)Main.rand.Next());
		}

		// Token: 0x060032A4 RID: 12964 RVA: 0x005F07AF File Offset: 0x005EE9AF
		public override void Update()
		{
			this.netOffset = this.netOffset.MoveTowards(Vector2.Zero, 2f);
		}

		// Token: 0x060032A5 RID: 12965 RVA: 0x005F07CC File Offset: 0x005EE9CC
		protected void Recall()
		{
			bool flag = Main.netMode != 2;
			if (flag)
			{
				for (int i = 0; i < 10; i++)
				{
					Dust.NewDustDirect(this.position, this.width, this.height, 15, 0f, 0f, 150, default(Color), 1.1f);
				}
			}
			base.Center = base.AnchorPosition.ToWorldCoordinates(8f, 8f) - new Vector2(0f, 16f);
			this.velocity = Vector2.Zero;
			if (flag)
			{
				for (int j = 0; j < 10; j++)
				{
					Dust.NewDustDirect(this.position, this.width, this.height, 15, 0f, 0f, 150, default(Color), 1.1f);
				}
			}
		}

		// Token: 0x060032A6 RID: 12966 RVA: 0x005F08B0 File Offset: 0x005EEAB0
		protected virtual void VisualEffects()
		{
			if (this.npcType >= 0 && NPCID.Sets.IsGoldCritter[this.npcType])
			{
				this.position += this.netOffset;
				Color color = Lighting.GetColor((int)base.Center.X / 16, (int)base.Center.Y / 16);
				if (color.R > 20 || color.B > 20 || color.G > 20)
				{
					int num = (int)color.R;
					if ((int)color.G > num)
					{
						num = (int)color.G;
					}
					if ((int)color.B > num)
					{
						num = (int)color.B;
					}
					num /= 30;
					if (Main.rand.Next(300) < num)
					{
						int num2 = Dust.NewDust(this.position, this.width, this.height, 43, 0f, 0f, 254, new Color(255, 255, 0), 0.5f);
						Main.dust[num2].velocity *= 0f;
					}
				}
				this.position -= this.netOffset;
			}
		}

		// Token: 0x060032A7 RID: 12967 RVA: 0x005F09F0 File Offset: 0x005EEBF0
		protected virtual void CopyToDummy()
		{
			LeashedCritter._dummy.type = this.npcType;
			LeashedCritter._dummy.Size = base.Size;
			LeashedCritter._dummy.frame = this.frame;
			LeashedCritter._dummy.frameCounter = this.frameCounter;
			LeashedCritter._dummy.position = base.Center + new Vector2(0f, 8f) - new Vector2(base.Size.X / 2f, base.Size.Y);
			LeashedCritter._dummy.velocity = this.velocity;
			LeashedCritter._dummy.direction = this.direction;
			LeashedCritter._dummy.spriteDirection = this.spriteDirection;
			LeashedCritter._dummy.scale = this.scale;
			LeashedCritter._dummy.rotation = 0f;
			LeashedCritter._dummy.alpha = 0;
			LeashedCritter._dummy.wet = false;
			Array.Clear(LeashedCritter._dummy.ai, 0, LeashedCritter._dummy.ai.Length);
			Array.Clear(LeashedCritter._dummy.localAI, 0, LeashedCritter._dummy.localAI.Length);
		}

		// Token: 0x060032A8 RID: 12968 RVA: 0x005F0B24 File Offset: 0x005EED24
		protected void CopyFromDummy()
		{
			this.frame = LeashedCritter._dummy.frame;
			this.frameCounter = LeashedCritter._dummy.frameCounter;
			this.spriteDirection = LeashedCritter._dummy.spriteDirection;
		}

		// Token: 0x060032A9 RID: 12969 RVA: 0x005F0B58 File Offset: 0x005EED58
		public override void Draw()
		{
			Main.instance.LoadNPC(this.npcType);
			if (this.frame.Width == 0 || this.frame.Height == 0)
			{
				this.frame = new Rectangle(0, 0, TextureAssets.Npc[this.npcType].Width(), TextureAssets.Npc[this.npcType].Height() / Main.npcFrameCount[this.npcType]);
			}
			this.CopyToDummy();
			LeashedCritter._dummy.position += this.netOffset + this.GetDrawOffset();
			Main.instance.DrawNPCDirect(Main.spriteBatch, LeashedCritter._dummy, true, Main.screenPosition);
			Point point = LeashedCritter._dummy.Center.ToTileCoordinates();
			byte liquid = Framing.GetTileSafely(point.X, point.Y).liquid;
			if ((this.isAquatic && liquid < 255) || (!this.isAquatic && liquid > 0))
			{
				this.DrawBubble();
			}
		}

		// Token: 0x060032AA RID: 12970 RVA: 0x005F0C5A File Offset: 0x005EEE5A
		public virtual Vector2 GetDrawOffset()
		{
			return Vector2.Zero;
		}

		// Token: 0x060032AB RID: 12971 RVA: 0x005F0C64 File Offset: 0x005EEE64
		protected void DrawBubble()
		{
			Main.instance.LoadGore(413);
			Texture2D value = TextureAssets.Gore[413].Value;
			Rectangle rectangle = value.Frame(1, 1, 0, 0, 0, 0);
			Vector2 origin = rectangle.Size() / 2f;
			Vector2 vector = this.position;
			vector += this.netOffset + this.GetDrawOffset() + LeashedCritter._dummy.Size * new Vector2(0.5f, 0.5f);
			Point tileCoords = vector.ToTileCoordinates();
			Main.spriteBatch.Draw(value, vector - Main.screenPosition, new Rectangle?(rectangle), Lighting.GetColor(tileCoords), 0f, origin, 1f, SpriteEffects.None, 0f);
		}

		// Token: 0x04005815 RID: 22549
		protected static NPC _dummy = new NPC();

		// Token: 0x04005816 RID: 22550
		public int anchorStyle;

		// Token: 0x04005817 RID: 22551
		protected int npcType;

		// Token: 0x04005818 RID: 22552
		protected int spriteDirection;

		// Token: 0x04005819 RID: 22553
		protected Rectangle frame;

		// Token: 0x0400581A RID: 22554
		protected double frameCounter;

		// Token: 0x0400581B RID: 22555
		protected LCG32Random rand;

		// Token: 0x0400581C RID: 22556
		protected short WaitTime;

		// Token: 0x0400581D RID: 22557
		protected byte State;

		// Token: 0x0400581E RID: 22558
		protected Point16 TargetPosition;

		// Token: 0x0400581F RID: 22559
		protected Vector2 netOffset;

		// Token: 0x04005820 RID: 22560
		protected float scale = 1f;

		// Token: 0x04005821 RID: 22561
		protected int strayingRangeInBlocks;

		// Token: 0x04005822 RID: 22562
		protected bool isAquatic;

		// Token: 0x04005823 RID: 22563
		protected static readonly float gravity = 0.3f;

		// Token: 0x04005824 RID: 22564
		protected static readonly float maxFallSpeed = 10f;

		// Token: 0x04005825 RID: 22565
		protected const int RecallDuration = 20;
	}
}
