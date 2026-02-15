using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.UI;
using Terraria.ID;

namespace Terraria.Cinematics
{
	// Token: 0x020005AC RID: 1452
	public class DD2Film : Film
	{
		// Token: 0x0600395D RID: 14685 RVA: 0x00650C74 File Offset: 0x0064EE74
		public DD2Film()
		{
			base.AppendKeyFrames(new FrameEvent[]
			{
				new FrameEvent(this.CreateDryad),
				new FrameEvent(this.CreateCritters)
			});
			base.AppendSequences(120, new FrameEvent[]
			{
				new FrameEvent(this.DryadStand),
				new FrameEvent(this.DryadLookRight)
			});
			base.AppendSequences(100, new FrameEvent[]
			{
				new FrameEvent(this.DryadLookRight),
				new FrameEvent(this.DryadInteract)
			});
			base.AddKeyFrame(base.AppendPoint - 20, new FrameEvent(this.CreatePortal));
			base.AppendSequences(30, new FrameEvent[]
			{
				new FrameEvent(this.DryadLookLeft),
				new FrameEvent(this.DryadStand)
			});
			base.AppendSequences(40, new FrameEvent[]
			{
				new FrameEvent(this.DryadConfusedEmote),
				new FrameEvent(this.DryadStand),
				new FrameEvent(this.DryadLookLeft)
			});
			base.AppendKeyFrame(new FrameEvent(this.CreateOgre));
			base.AddKeyFrame(base.AppendPoint + 60, new FrameEvent(this.SpawnJavalinThrower));
			base.AddKeyFrame(base.AppendPoint + 120, new FrameEvent(this.SpawnGoblin));
			base.AddKeyFrame(base.AppendPoint + 180, new FrameEvent(this.SpawnGoblin));
			base.AddKeyFrame(base.AppendPoint + 240, new FrameEvent(this.SpawnWitherBeast));
			base.AppendSequences(30, new FrameEvent[]
			{
				new FrameEvent(this.DryadStand),
				new FrameEvent(this.DryadLookLeft)
			});
			base.AppendSequences(30, new FrameEvent[]
			{
				new FrameEvent(this.DryadLookRight),
				new FrameEvent(this.DryadWalk)
			});
			base.AppendSequences(300, new FrameEvent[]
			{
				new FrameEvent(this.DryadAttack),
				new FrameEvent(this.DryadLookLeft)
			});
			base.AppendKeyFrame(new FrameEvent(this.RemoveEnemyDamage));
			base.AppendSequences(60, new FrameEvent[]
			{
				new FrameEvent(this.DryadLookRight),
				new FrameEvent(this.DryadStand),
				new FrameEvent(this.DryadAlertEmote)
			});
			base.AddSequences(base.AppendPoint - 90, 60, new FrameEvent[]
			{
				new FrameEvent(this.OgreLookLeft),
				new FrameEvent(this.OgreStand)
			});
			base.AddKeyFrame(base.AppendPoint - 12, new FrameEvent(this.OgreSwingSound));
			base.AddSequences(base.AppendPoint - 30, 50, new FrameEvent[]
			{
				new FrameEvent(this.DryadPortalKnock),
				new FrameEvent(this.DryadStand)
			});
			base.AppendKeyFrame(new FrameEvent(this.RestoreEnemyDamage));
			base.AppendSequences(40, new FrameEvent[]
			{
				new FrameEvent(this.DryadPortalFade),
				new FrameEvent(this.DryadStand)
			});
			base.AppendSequence(180, new FrameEvent(this.DryadStand));
			base.AddSequence(0, base.AppendPoint, new FrameEvent(this.PerFrameSettings));
		}

		// Token: 0x0600395E RID: 14686 RVA: 0x00650FEC File Offset: 0x0064F1EC
		private void PerFrameSettings(FrameEventData evt)
		{
			CombatText.clearAll();
		}

		// Token: 0x0600395F RID: 14687 RVA: 0x00650FF4 File Offset: 0x0064F1F4
		private void CreateDryad(FrameEventData evt)
		{
			this._dryad = this.PlaceNPCOnGround(20, this._startPoint);
			this._dryad.knockBackResist = 0f;
			this._dryad.immortal = true;
			this._dryad.dontTakeDamage = true;
			this._dryad.takenDamageMultiplier = 0f;
			this._dryad.immune[255] = 100000;
		}

		// Token: 0x06003960 RID: 14688 RVA: 0x00651064 File Offset: 0x0064F264
		private void DryadInteract(FrameEventData evt)
		{
			if (this._dryad != null)
			{
				this._dryad.ai[0] = 9f;
				if (evt.IsFirstFrame)
				{
					this._dryad.ai[1] = (float)evt.Duration;
				}
				this._dryad.localAI[0] = 0f;
			}
		}

		// Token: 0x06003961 RID: 14689 RVA: 0x006510BC File Offset: 0x0064F2BC
		private void SpawnWitherBeast(FrameEventData evt)
		{
			int num = NPC.NewNPC(new EntitySource_Film(), (int)this._portal.Center.X, (int)this._portal.Bottom.Y, 568, 0, 0f, 0f, 0f, 0f, 255);
			NPC npc = Main.npc[num];
			npc.knockBackResist = 0f;
			npc.immortal = true;
			npc.dontTakeDamage = true;
			npc.takenDamageMultiplier = 0f;
			npc.immune[255] = 100000;
			npc.friendly = this._ogre.friendly;
			this._army.Add(npc);
		}

		// Token: 0x06003962 RID: 14690 RVA: 0x00651170 File Offset: 0x0064F370
		private void SpawnJavalinThrower(FrameEventData evt)
		{
			int num = NPC.NewNPC(new EntitySource_Film(), (int)this._portal.Center.X, (int)this._portal.Bottom.Y, 561, 0, 0f, 0f, 0f, 0f, 255);
			NPC npc = Main.npc[num];
			npc.knockBackResist = 0f;
			npc.immortal = true;
			npc.dontTakeDamage = true;
			npc.takenDamageMultiplier = 0f;
			npc.immune[255] = 100000;
			npc.friendly = this._ogre.friendly;
			this._army.Add(npc);
		}

		// Token: 0x06003963 RID: 14691 RVA: 0x00651224 File Offset: 0x0064F424
		private void SpawnGoblin(FrameEventData evt)
		{
			int num = NPC.NewNPC(new EntitySource_Film(), (int)this._portal.Center.X, (int)this._portal.Bottom.Y, 552, 0, 0f, 0f, 0f, 0f, 255);
			NPC npc = Main.npc[num];
			npc.knockBackResist = 0f;
			npc.immortal = true;
			npc.dontTakeDamage = true;
			npc.takenDamageMultiplier = 0f;
			npc.immune[255] = 100000;
			npc.friendly = this._ogre.friendly;
			this._army.Add(npc);
		}

		// Token: 0x06003964 RID: 14692 RVA: 0x006512D8 File Offset: 0x0064F4D8
		private void CreateCritters(FrameEventData evt)
		{
			for (int i = 0; i < 5; i++)
			{
				float num = (float)i / 5f;
				NPC npc = this.PlaceNPCOnGround((int)Utils.SelectRandom<short>(Main.rand, new short[]
				{
					46,
					46,
					299,
					538
				}), this._startPoint + new Vector2((num - 0.25f) * 400f + Main.rand.NextFloat() * 50f - 25f, 0f));
				npc.ai[0] = 0f;
				npc.ai[1] = 600f;
				this._critters.Add(npc);
			}
			if (this._dryad == null)
			{
				return;
			}
			for (int j = 0; j < 10; j++)
			{
				float num2 = (float)j / 10f;
				int num3 = NPC.NewNPC(new EntitySource_Film(), (int)this._dryad.position.X + Main.rand.Next(-1000, 800), (int)this._dryad.position.Y - Main.rand.Next(-50, 300), 356, 0, 0f, 0f, 0f, 0f, 255);
				NPC npc2 = Main.npc[num3];
				npc2.ai[0] = Main.rand.NextFloat() * 4f - 2f;
				npc2.ai[1] = Main.rand.NextFloat() * 4f - 2f;
				npc2.velocity.X = Main.rand.NextFloat() * 4f - 2f;
				this._critters.Add(npc2);
			}
		}

		// Token: 0x06003965 RID: 14693 RVA: 0x00651491 File Offset: 0x0064F691
		private void OgreSwingSound(FrameEventData evt)
		{
			SoundEngine.PlaySound(SoundID.DD2_OgreAttack, this._ogre.Center, 0f, 1f);
		}

		// Token: 0x06003966 RID: 14694 RVA: 0x006514B4 File Offset: 0x0064F6B4
		private void DryadPortalKnock(FrameEventData evt)
		{
			if (this._dryad != null)
			{
				if (evt.Frame == 20)
				{
					NPC dryad = this._dryad;
					dryad.velocity.Y = dryad.velocity.Y - 7f;
					NPC dryad2 = this._dryad;
					dryad2.velocity.X = dryad2.velocity.X - 8f;
					SoundEngine.PlaySound(3, (int)this._dryad.Center.X, (int)this._dryad.Center.Y, 1, 1f, 0f);
				}
				if (evt.Frame >= 20)
				{
					this._dryad.ai[0] = 1f;
					this._dryad.ai[1] = (float)evt.Remaining;
					this._dryad.rotation += 0.05f;
				}
			}
			if (this._ogre != null)
			{
				if (evt.Frame > 40)
				{
					this._ogre.target = Main.myPlayer;
					this._ogre.direction = 1;
					return;
				}
				this._ogre.direction = -1;
				this._ogre.ai[1] = 0f;
				this._ogre.ai[0] = Math.Min(40f, this._ogre.ai[0]);
				this._ogre.target = 300 + this._dryad.whoAmI;
			}
		}

		// Token: 0x06003967 RID: 14695 RVA: 0x00651618 File Offset: 0x0064F818
		private void RemoveEnemyDamage(FrameEventData evt)
		{
			this._ogre.friendly = true;
			foreach (NPC npc in this._army)
			{
				npc.friendly = true;
			}
		}

		// Token: 0x06003968 RID: 14696 RVA: 0x00651678 File Offset: 0x0064F878
		private void RestoreEnemyDamage(FrameEventData evt)
		{
			this._ogre.friendly = false;
			foreach (NPC npc in this._army)
			{
				npc.friendly = false;
			}
		}

		// Token: 0x06003969 RID: 14697 RVA: 0x006516D8 File Offset: 0x0064F8D8
		private void DryadPortalFade(FrameEventData evt)
		{
			if (this._dryad != null && this._portal != null)
			{
				if (evt.IsFirstFrame)
				{
					SoundEngine.PlaySound(SoundID.DD2_EtherianPortalDryadTouch, this._dryad.Center, 0f, 1f);
				}
				float num = (float)(evt.Frame - 7) / (float)(evt.Duration - 7);
				num = Math.Max(0f, num);
				this._dryad.color = new Color(Vector3.Lerp(Vector3.One, new Vector3(0.5f, 0f, 0.8f), num));
				this._dryad.Opacity = 1f - num;
				this._dryad.rotation += 0.05f * (num * 4f + 1f);
				this._dryad.scale = 1f - num;
				if (this._dryad.position.X < this._portal.Right.X)
				{
					NPC dryad = this._dryad;
					dryad.velocity.X = dryad.velocity.X * 0.95f;
					NPC dryad2 = this._dryad;
					dryad2.velocity.Y = dryad2.velocity.Y * 0.55f;
				}
				int num2 = (int)(6f * num);
				float num3 = this._dryad.Size.Length() / 2f;
				num3 /= 20f;
				for (int i = 0; i < num2; i++)
				{
					if (Main.rand.Next(5) == 0)
					{
						Dust dust = Dust.NewDustDirect(this._dryad.position, this._dryad.width, this._dryad.height, 27, this._dryad.velocity.X * 1f, 0f, 100, default(Color), 1f);
						dust.scale = 0.55f;
						dust.fadeIn = 0.7f;
						dust.velocity *= 0.1f * num3;
						dust.velocity += this._dryad.velocity;
					}
				}
			}
		}

		// Token: 0x0600396A RID: 14698 RVA: 0x00651900 File Offset: 0x0064FB00
		private void CreatePortal(FrameEventData evt)
		{
			this._portal = this.PlaceNPCOnGround(549, this._startPoint + new Vector2(-240f, 0f));
			this._portal.immortal = true;
		}

		// Token: 0x0600396B RID: 14699 RVA: 0x00651939 File Offset: 0x0064FB39
		private void DryadStand(FrameEventData evt)
		{
			if (this._dryad != null)
			{
				this._dryad.ai[0] = 0f;
				this._dryad.ai[1] = (float)evt.Remaining;
			}
		}

		// Token: 0x0600396C RID: 14700 RVA: 0x0065196A File Offset: 0x0064FB6A
		private void DryadLookRight(FrameEventData evt)
		{
			if (this._dryad != null)
			{
				this._dryad.direction = 1;
				this._dryad.spriteDirection = 1;
			}
		}

		// Token: 0x0600396D RID: 14701 RVA: 0x0065198C File Offset: 0x0064FB8C
		private void DryadLookLeft(FrameEventData evt)
		{
			if (this._dryad != null)
			{
				this._dryad.direction = -1;
				this._dryad.spriteDirection = -1;
			}
		}

		// Token: 0x0600396E RID: 14702 RVA: 0x006519AE File Offset: 0x0064FBAE
		private void DryadWalk(FrameEventData evt)
		{
			this._dryad.ai[0] = 1f;
			this._dryad.ai[1] = 2f;
		}

		// Token: 0x0600396F RID: 14703 RVA: 0x006519D4 File Offset: 0x0064FBD4
		private void DryadConfusedEmote(FrameEventData evt)
		{
			if (this._dryad != null && evt.IsFirstFrame)
			{
				EmoteBubble.NewBubble(87, new WorldUIAnchor(this._dryad), evt.Duration);
			}
		}

		// Token: 0x06003970 RID: 14704 RVA: 0x00651A01 File Offset: 0x0064FC01
		private void DryadAlertEmote(FrameEventData evt)
		{
			if (this._dryad != null && evt.IsFirstFrame)
			{
				EmoteBubble.NewBubble(3, new WorldUIAnchor(this._dryad), evt.Duration);
			}
		}

		// Token: 0x06003971 RID: 14705 RVA: 0x00651A30 File Offset: 0x0064FC30
		private void CreateOgre(FrameEventData evt)
		{
			int num = NPC.NewNPC(new EntitySource_Film(), (int)this._portal.Center.X, (int)this._portal.Bottom.Y, 576, 0, 0f, 0f, 0f, 0f, 255);
			this._ogre = Main.npc[num];
			this._ogre.knockBackResist = 0f;
			this._ogre.immortal = true;
			this._ogre.dontTakeDamage = true;
			this._ogre.takenDamageMultiplier = 0f;
			this._ogre.immune[255] = 100000;
		}

		// Token: 0x06003972 RID: 14706 RVA: 0x00651AE4 File Offset: 0x0064FCE4
		private void OgreStand(FrameEventData evt)
		{
			if (this._ogre != null)
			{
				this._ogre.ai[0] = 0f;
				this._ogre.ai[1] = 0f;
				this._ogre.velocity = Vector2.Zero;
			}
		}

		// Token: 0x06003973 RID: 14707 RVA: 0x00651B22 File Offset: 0x0064FD22
		private void DryadAttack(FrameEventData evt)
		{
			if (this._dryad != null)
			{
				this._dryad.ai[0] = 14f;
				this._dryad.ai[1] = (float)evt.Remaining;
				this._dryad.dryadWard = false;
			}
		}

		// Token: 0x06003974 RID: 14708 RVA: 0x00651B5F File Offset: 0x0064FD5F
		private void OgreLookRight(FrameEventData evt)
		{
			if (this._ogre != null)
			{
				this._ogre.direction = 1;
				this._ogre.spriteDirection = 1;
			}
		}

		// Token: 0x06003975 RID: 14709 RVA: 0x00651B81 File Offset: 0x0064FD81
		private void OgreLookLeft(FrameEventData evt)
		{
			if (this._ogre != null)
			{
				this._ogre.direction = -1;
				this._ogre.spriteDirection = -1;
			}
		}

		// Token: 0x06003976 RID: 14710 RVA: 0x00651BA4 File Offset: 0x0064FDA4
		public override void OnBegin()
		{
			Main.NewText("DD2Film: Begin", byte.MaxValue, byte.MaxValue, byte.MaxValue);
			Main.dayTime = true;
			Main.time = 27000.0;
			this._startPoint = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY - 32f);
			base.OnBegin();
		}

		// Token: 0x06003977 RID: 14711 RVA: 0x00651C0C File Offset: 0x0064FE0C
		private NPC PlaceNPCOnGround(int type, Vector2 position)
		{
			int num = (int)position.X;
			int num2 = (int)position.Y;
			int i = num / 16;
			int num3 = num2 / 16;
			while (!WorldGen.SolidTile(i, num3, false))
			{
				num3++;
			}
			num2 = num3 * 16;
			int start = 100;
			if (type == 20)
			{
				start = 1;
			}
			else if (type == 576)
			{
				start = 50;
			}
			int num4 = NPC.NewNPC(new EntitySource_Film(), num, num2, type, start, 0f, 0f, 0f, 0f, 255);
			return Main.npc[num4];
		}

		// Token: 0x06003978 RID: 14712 RVA: 0x00651C94 File Offset: 0x0064FE94
		public override void OnEnd()
		{
			if (this._dryad != null)
			{
				this._dryad.active = false;
			}
			if (this._portal != null)
			{
				this._portal.active = false;
			}
			if (this._ogre != null)
			{
				this._ogre.active = false;
			}
			foreach (NPC npc in this._critters)
			{
				npc.active = false;
			}
			foreach (NPC npc2 in this._army)
			{
				npc2.active = false;
			}
			Main.NewText("DD2Film: End", byte.MaxValue, byte.MaxValue, byte.MaxValue);
			base.OnEnd();
		}

		// Token: 0x04005D70 RID: 23920
		private NPC _dryad;

		// Token: 0x04005D71 RID: 23921
		private NPC _ogre;

		// Token: 0x04005D72 RID: 23922
		private NPC _portal;

		// Token: 0x04005D73 RID: 23923
		private List<NPC> _army = new List<NPC>();

		// Token: 0x04005D74 RID: 23924
		private List<NPC> _critters = new List<NPC>();

		// Token: 0x04005D75 RID: 23925
		private Vector2 _startPoint;
	}
}
