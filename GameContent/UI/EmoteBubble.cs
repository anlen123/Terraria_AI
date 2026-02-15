using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Events;
using Terraria.ID;

namespace Terraria.GameContent.UI
{
	// Token: 0x0200037C RID: 892
	public class EmoteBubble
	{
		// Token: 0x06002963 RID: 10595 RVA: 0x0057AF64 File Offset: 0x00579164
		public static void UpdateAll()
		{
			Dictionary<int, EmoteBubble> obj = EmoteBubble.byID;
			lock (obj)
			{
				EmoteBubble.toClean.Clear();
				foreach (KeyValuePair<int, EmoteBubble> keyValuePair in EmoteBubble.byID)
				{
					keyValuePair.Value.Update();
					if (keyValuePair.Value.lifeTime <= 0)
					{
						EmoteBubble.toClean.Add(keyValuePair.Key);
					}
				}
				foreach (int key in EmoteBubble.toClean)
				{
					EmoteBubble.byID.Remove(key);
				}
				EmoteBubble.toClean.Clear();
			}
		}

		// Token: 0x06002964 RID: 10596 RVA: 0x0057B060 File Offset: 0x00579260
		public static void DrawAll(SpriteBatch sb)
		{
			Dictionary<int, EmoteBubble> obj = EmoteBubble.byID;
			lock (obj)
			{
				foreach (KeyValuePair<int, EmoteBubble> keyValuePair in EmoteBubble.byID)
				{
					keyValuePair.Value.Draw(sb);
				}
			}
		}

		// Token: 0x06002965 RID: 10597 RVA: 0x0057B0E0 File Offset: 0x005792E0
		public static Tuple<int, int> SerializeNetAnchor(WorldUIAnchor anch)
		{
			if (anch.type == WorldUIAnchor.AnchorType.Entity)
			{
				int item = 0;
				if (anch.entity is NPC)
				{
					item = 0;
				}
				else if (anch.entity is Player)
				{
					item = 1;
				}
				else if (anch.entity is Projectile)
				{
					item = 2;
				}
				return Tuple.Create<int, int>(item, anch.entity.whoAmI);
			}
			return Tuple.Create<int, int>(0, 0);
		}

		// Token: 0x06002966 RID: 10598 RVA: 0x0057B143 File Offset: 0x00579343
		public static WorldUIAnchor DeserializeNetAnchor(int type, int meta)
		{
			if (type == 0)
			{
				return new WorldUIAnchor(Main.npc[meta]);
			}
			if (type == 1)
			{
				return new WorldUIAnchor(Main.player[meta]);
			}
			if (type == 2)
			{
				return new WorldUIAnchor(Main.projectile[meta]);
			}
			throw new Exception("How did you end up getting this?");
		}

		// Token: 0x06002967 RID: 10599 RVA: 0x0057B181 File Offset: 0x00579381
		public static int AssignNewID()
		{
			return EmoteBubble.NextID++;
		}

		// Token: 0x06002968 RID: 10600 RVA: 0x0057B190 File Offset: 0x00579390
		public static int NewBubble(int emoticon, WorldUIAnchor bubbleAnchor, int time)
		{
			if (Main.netMode == 1)
			{
				return -1;
			}
			EmoteBubble emoteBubble = new EmoteBubble(emoticon, bubbleAnchor, time);
			emoteBubble.ID = EmoteBubble.AssignNewID();
			EmoteBubble.byID[emoteBubble.ID] = emoteBubble;
			if (Main.netMode == 2)
			{
				Tuple<int, int> tuple = EmoteBubble.SerializeNetAnchor(bubbleAnchor);
				NetMessage.SendData(91, -1, -1, null, emoteBubble.ID, (float)tuple.Item1, (float)tuple.Item2, (float)time, emoticon, 0, 0);
			}
			EmoteBubble.OnBubbleChange(emoteBubble.ID);
			return emoteBubble.ID;
		}

		// Token: 0x06002969 RID: 10601 RVA: 0x0057B210 File Offset: 0x00579410
		public static int NewBubbleNPC(WorldUIAnchor bubbleAnchor, int time, WorldUIAnchor other = null)
		{
			if (Main.netMode == 1)
			{
				return -1;
			}
			EmoteBubble emoteBubble = new EmoteBubble(0, bubbleAnchor, time);
			emoteBubble.ID = EmoteBubble.AssignNewID();
			EmoteBubble.byID[emoteBubble.ID] = emoteBubble;
			emoteBubble.PickNPCEmote(other);
			if (Main.netMode == 2)
			{
				Tuple<int, int> tuple = EmoteBubble.SerializeNetAnchor(bubbleAnchor);
				NetMessage.SendData(91, -1, -1, null, emoteBubble.ID, (float)tuple.Item1, (float)tuple.Item2, (float)time, emoteBubble.emote, emoteBubble.metadata, 0);
			}
			return emoteBubble.ID;
		}

		// Token: 0x0600296A RID: 10602 RVA: 0x0057B298 File Offset: 0x00579498
		public static void CheckForNPCsToReactToEmoteBubble(int emoteID, Player player)
		{
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC npc = Main.npc[i];
				if (npc != null && npc.active && npc.aiStyle == 7 && npc.townNPC && npc.ai[0] < 2f && ((player.CanBeTalkedTo && player.Distance(npc.Center) < 200f) || !Collision.CanHitLine(npc.Top, 0, 0, player.Top, 0, 0)))
				{
					int direction = (npc.position.X < player.position.X).ToDirectionInt();
					npc.ai[0] = 19f;
					npc.ai[1] = 220f;
					npc.ai[2] = (float)player.whoAmI;
					npc.direction = direction;
					npc.netUpdate = true;
				}
			}
		}

		// Token: 0x0600296B RID: 10603 RVA: 0x0057B384 File Offset: 0x00579584
		public EmoteBubble(int emotion, WorldUIAnchor bubbleAnchor, int time = 180)
		{
			this.anchor = bubbleAnchor;
			this.emote = emotion;
			this.lifeTime = time;
			this.lifeTimeStart = time;
		}

		// Token: 0x0600296C RID: 10604 RVA: 0x0057B3A8 File Offset: 0x005795A8
		private void Update()
		{
			int num = this.lifeTime - 1;
			this.lifeTime = num;
			if (num <= 0)
			{
				return;
			}
			num = this.frameCounter + 1;
			this.frameCounter = num;
			if (num >= 8)
			{
				this.frameCounter = 0;
				num = this.frame + 1;
				this.frame = num;
				if (num >= 2)
				{
					this.frame = 0;
				}
			}
		}

		// Token: 0x0600296D RID: 10605 RVA: 0x0057B400 File Offset: 0x00579600
		public static void DrawTemporaryBubble(SpriteBatch spritebatch, int emoteId, int maxLifetime, int currentLifetime, Entity anchor)
		{
			EmoteBubble._temporaryBubble.anchor.type = WorldUIAnchor.AnchorType.Entity;
			EmoteBubble._temporaryBubble.anchor.entity = anchor;
			EmoteBubble._temporaryBubble.emote = emoteId;
			EmoteBubble._temporaryBubble.lifeTimeStart = maxLifetime;
			EmoteBubble._temporaryBubble.lifeTime = currentLifetime;
			EmoteBubble._temporaryBubble.Draw(spritebatch);
		}

		// Token: 0x0600296E RID: 10606 RVA: 0x0057B45C File Offset: 0x0057965C
		public void Draw(SpriteBatch sb)
		{
			Texture2D value = TextureAssets.Extra[48].Value;
			SpriteEffects spriteEffects = SpriteEffects.None;
			Vector2 vector = this.GetPosition(out spriteEffects);
			vector = vector.Floor();
			bool flag = this.lifeTime < 6 || this.lifeTimeStart - this.lifeTime < 6;
			Rectangle rectangle = value.Frame(8, EmoteBubble.EMOTE_SHEET_VERTICAL_FRAMES, flag ? 0 : 1, 0, 0, 0);
			Vector2 origin = new Vector2((float)(rectangle.Width / 2), (float)rectangle.Height);
			if (Main.player[Main.myPlayer].gravDir == -1f)
			{
				origin.Y = 0f;
				spriteEffects |= SpriteEffects.FlipVertically;
				vector = Main.ReverseGravitySupport(vector, 0f);
			}
			sb.Draw(value, vector, new Rectangle?(rectangle), Color.White, 0f, origin, 1f, spriteEffects, 0f);
			if (!flag)
			{
				if (this.emote >= 0)
				{
					if ((this.emote == 87 || this.emote == 89) && (spriteEffects & SpriteEffects.FlipHorizontally) != SpriteEffects.None)
					{
						spriteEffects &= ~SpriteEffects.FlipHorizontally;
						vector.X += 4f;
					}
					sb.Draw(value, vector, new Rectangle?(value.Frame(8, EmoteBubble.EMOTE_SHEET_VERTICAL_FRAMES, this.emote * 2 % 8 + this.frame, 1 + this.emote / 4, 0, 0)), Color.White, 0f, origin, 1f, spriteEffects, 0f);
					return;
				}
				if (this.emote == -1)
				{
					value = TextureAssets.NpcHead[this.metadata].Value;
					float num = 1f;
					if ((float)value.Width / 22f > 1f)
					{
						num = 22f / (float)value.Width;
					}
					if ((float)value.Height / 16f > 1f / num)
					{
						num = 16f / (float)value.Height;
					}
					sb.Draw(value, vector + new Vector2((float)(((spriteEffects & SpriteEffects.FlipHorizontally) != SpriteEffects.None) ? 1 : -1), (float)(-(float)rectangle.Height + 3)), null, Color.White, 0f, new Vector2((float)(value.Width / 2), 0f), num, spriteEffects, 0f);
				}
			}
		}

		// Token: 0x0600296F RID: 10607 RVA: 0x0057B680 File Offset: 0x00579880
		private Vector2 GetPosition(out SpriteEffects effect)
		{
			switch (this.anchor.type)
			{
			case WorldUIAnchor.AnchorType.Entity:
				effect = ((this.anchor.entity.direction == -1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally);
				return new Vector2(this.anchor.entity.Top.X, this.anchor.entity.VisualPosition.Y) + new Vector2((float)(-(float)this.anchor.entity.direction * this.anchor.entity.width) * 0.75f, 2f) - Main.screenPosition;
			case WorldUIAnchor.AnchorType.Tile:
				effect = SpriteEffects.None;
				return this.anchor.pos - Main.screenPosition + new Vector2(0f, -this.anchor.size.Y / 2f);
			case WorldUIAnchor.AnchorType.Pos:
				effect = SpriteEffects.None;
				return this.anchor.pos - Main.screenPosition;
			default:
				effect = SpriteEffects.None;
				return new Vector2((float)Main.screenWidth, (float)Main.screenHeight) / 2f;
			}
		}

		// Token: 0x06002970 RID: 10608 RVA: 0x0057B7B4 File Offset: 0x005799B4
		public static void OnBubbleChange(int bubbleID)
		{
			EmoteBubble emoteBubble = EmoteBubble.byID[bubbleID];
			if (emoteBubble.anchor.type == WorldUIAnchor.AnchorType.Entity)
			{
				Player player = emoteBubble.anchor.entity as Player;
				if (player != null)
				{
					foreach (EmoteBubble emoteBubble2 in EmoteBubble.byID.Values)
					{
						if (emoteBubble2.anchor.type == WorldUIAnchor.AnchorType.Entity && emoteBubble2.anchor.entity == player && emoteBubble2.ID != bubbleID)
						{
							emoteBubble2.lifeTime = 6;
						}
					}
				}
			}
		}

		// Token: 0x06002971 RID: 10609 RVA: 0x0057B85C File Offset: 0x00579A5C
		public static void MakeLocalPlayerEmote(int emoteId)
		{
			if (Main.netMode != 2 && Main.LocalPlayer.dead)
			{
				return;
			}
			if (Main.netMode == 0)
			{
				EmoteBubble.NewBubble(emoteId, new WorldUIAnchor(Main.LocalPlayer), 360);
				EmoteBubble.CheckForNPCsToReactToEmoteBubble(emoteId, Main.LocalPlayer);
				return;
			}
			NetMessage.SendData(120, -1, -1, null, Main.myPlayer, (float)emoteId, 0f, 0f, 0, 0, 0);
		}

		// Token: 0x06002972 RID: 10610 RVA: 0x0057B8C8 File Offset: 0x00579AC8
		public void PickNPCEmote(WorldUIAnchor other = null)
		{
			Player plr = Main.player[(int)Player.FindClosest(((NPC)this.anchor.entity).Center, 0, 0)];
			List<int> list = new List<int>();
			bool flag = false;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				if (Main.npc[i].active && Main.npc[i].boss)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				if (Main.rand.Next(3) == 0)
				{
					this.ProbeTownNPCs(list);
				}
				if (Main.rand.Next(3) == 0)
				{
					this.ProbeEmotions(list);
				}
				if (Main.rand.Next(3) == 0)
				{
					this.ProbeBiomes(list, plr);
				}
				if (Main.rand.Next(2) == 0)
				{
					this.ProbeCritters(list);
				}
				if (Main.rand.Next(2) == 0)
				{
					this.ProbeItems(list, plr);
				}
				if (Main.rand.Next(5) == 0)
				{
					this.ProbeBosses(list);
				}
				if (Main.rand.Next(2) == 0)
				{
					this.ProbeDebuffs(list, plr);
				}
				if (Main.rand.Next(2) == 0)
				{
					this.ProbeEvents(list);
				}
				if (Main.rand.Next(2) == 0)
				{
					this.ProbeWeather(list, plr);
				}
				this.ProbeExceptions(list, plr, other);
			}
			else
			{
				this.ProbeCombat(list);
			}
			if (list.Count > 0)
			{
				this.emote = list[Main.rand.Next(list.Count)];
			}
		}

		// Token: 0x06002973 RID: 10611 RVA: 0x0057BA23 File Offset: 0x00579C23
		private void ProbeCombat(List<int> list)
		{
			list.Add(16);
			list.Add(1);
			list.Add(2);
			list.Add(91);
			list.Add(93);
			list.Add(84);
			list.Add(84);
		}

		// Token: 0x06002974 RID: 10612 RVA: 0x0057BA5C File Offset: 0x00579C5C
		private void ProbeWeather(List<int> list, Player plr)
		{
			if (Main.cloudBGActive > 0f)
			{
				list.Add(96);
			}
			if (Main.cloudAlpha > 0f)
			{
				if (!Main.dayTime)
				{
					list.Add(5);
				}
				list.Add(4);
				if (plr.ZoneSnow)
				{
					list.Add(98);
				}
				if (plr.position.X < 4000f || (plr.position.X > (float)(Main.maxTilesX * 16 - 4000) && (double)plr.position.Y < Main.worldSurface / 16.0))
				{
					list.Add(97);
				}
			}
			else
			{
				list.Add(95);
			}
			if (plr.ZoneHallow)
			{
				list.Add(6);
			}
		}

		// Token: 0x06002975 RID: 10613 RVA: 0x0057BB1C File Offset: 0x00579D1C
		private void ProbeEvents(List<int> list)
		{
			if (BirthdayParty.PartyIsUp && Main.rand.Next(3) == 0)
			{
				list.Add(Utils.SelectRandom<int>(Main.rand, new int[]
				{
					127,
					128,
					129,
					126
				}));
			}
			if (Main.bloodMoon || (!Main.dayTime && Main.rand.Next(4) == 0))
			{
				list.Add(18);
			}
			if (Main.eclipse || (Main.hardMode && Main.rand.Next(4) == 0))
			{
				list.Add(19);
			}
			if ((!Main.dayTime || WorldGen.spawnMeteor) && NPC.downedBoss2)
			{
				list.Add(99);
			}
			if (Main.pumpkinMoon || ((NPC.downedHalloweenKing || NPC.downedHalloweenTree) && !Main.dayTime))
			{
				list.Add(20);
			}
			if (Main.snowMoon || ((NPC.downedChristmasIceQueen || NPC.downedChristmasSantank || NPC.downedChristmasTree) && !Main.dayTime))
			{
				list.Add(21);
			}
			if (DD2Event.Ongoing || DD2Event.DownedInvasionAnyDifficulty)
			{
				list.Add(133);
			}
		}

		// Token: 0x06002976 RID: 10614 RVA: 0x0057BC2C File Offset: 0x00579E2C
		private void ProbeDebuffs(List<int> list, Player plr)
		{
			if (plr.Center.Y > (float)(Main.maxTilesY * 16 - 3200) || plr.onFire || ((NPC)this.anchor.entity).onFire || plr.onFire2)
			{
				list.Add(9);
			}
			if (Main.rand.Next(2) == 0)
			{
				list.Add(11);
			}
			if (plr.poisoned || ((NPC)this.anchor.entity).poisoned || plr.ZoneJungle)
			{
				list.Add(8);
			}
			if (plr.inventory[plr.selectedItem].type == 215 || Main.rand.Next(3) == 0)
			{
				list.Add(10);
			}
		}

		// Token: 0x06002977 RID: 10615 RVA: 0x0057BCF8 File Offset: 0x00579EF8
		private void ProbeItems(List<int> list, Player plr)
		{
			list.Add(7);
			list.Add(73);
			list.Add(74);
			list.Add(75);
			list.Add(78);
			list.Add(90);
			if (plr.statLife < plr.statLifeMax2 / 2)
			{
				list.Add(84);
			}
		}

		// Token: 0x06002978 RID: 10616 RVA: 0x0057BD4C File Offset: 0x00579F4C
		private void ProbeTownNPCs(List<int> list)
		{
			for (int i = 0; i < (int)NPCID.Count; i++)
			{
				EmoteBubble.CountNPCs[i] = 0;
			}
			for (int j = 0; j < Main.maxNPCs; j++)
			{
				if (Main.npc[j].active)
				{
					EmoteBubble.CountNPCs[Main.npc[j].type]++;
				}
			}
			int type = ((NPC)this.anchor.entity).type;
			for (int k = 0; k < (int)NPCID.Count; k++)
			{
				if (NPCID.Sets.FaceEmote[k] > 0 && EmoteBubble.CountNPCs[k] > 0 && k != type)
				{
					list.Add(NPCID.Sets.FaceEmote[k]);
				}
			}
		}

		// Token: 0x06002979 RID: 10617 RVA: 0x0057BDF8 File Offset: 0x00579FF8
		private void ProbeBiomes(List<int> list, Player plr)
		{
			if ((double)(plr.position.Y / 16f) < Main.worldSurface * 0.45)
			{
				list.Add(22);
				return;
			}
			if ((double)(plr.position.Y / 16f) > Main.rockLayer + (double)(Main.maxTilesY / 2) - 100.0)
			{
				list.Add(31);
				return;
			}
			if ((double)(plr.position.Y / 16f) > Main.rockLayer)
			{
				list.Add(30);
				return;
			}
			if (plr.ZoneHallow)
			{
				list.Add(27);
				return;
			}
			if (plr.ZoneCorrupt)
			{
				list.Add(26);
				return;
			}
			if (plr.ZoneCrimson)
			{
				list.Add(25);
				return;
			}
			if (plr.ZoneJungle)
			{
				list.Add(24);
				return;
			}
			if (plr.ZoneSnow)
			{
				list.Add(32);
				return;
			}
			if ((double)(plr.position.Y / 16f) < Main.worldSurface && (plr.position.X < 4000f || plr.position.X > (float)(16 * (Main.maxTilesX - 250))))
			{
				list.Add(29);
				return;
			}
			if (plr.ZoneDesert)
			{
				list.Add(28);
				return;
			}
			list.Add(23);
		}

		// Token: 0x0600297A RID: 10618 RVA: 0x0057BF48 File Offset: 0x0057A148
		private void ProbeCritters(List<int> list)
		{
			ref Vector2 center = this.anchor.entity.Center;
			float num = 1f;
			float num2 = 1f;
			if ((double)center.Y < Main.rockLayer * 16.0)
			{
				num2 = 0.2f;
			}
			else
			{
				num = 0.2f;
			}
			if (Main.rand.NextFloat() <= num)
			{
				if (Main.dayTime)
				{
					list.Add(13);
					list.Add(12);
					list.Add(68);
					list.Add(62);
					list.Add(63);
					list.Add(69);
					list.Add(70);
				}
				if (!Main.dayTime || (Main.dayTime && (Main.time < 5400.0 || Main.time > 48600.0)))
				{
					list.Add(61);
				}
				if (NPC.downedGoblins)
				{
					list.Add(64);
				}
				if (NPC.downedFrost)
				{
					list.Add(66);
				}
				if (NPC.downedPirates)
				{
					list.Add(65);
				}
				if (NPC.downedMartians)
				{
					list.Add(71);
				}
				if (WorldGen.crimson)
				{
					list.Add(67);
				}
			}
			if (Main.rand.NextFloat() <= num2)
			{
				list.Add(72);
				list.Add(69);
			}
		}

		// Token: 0x0600297B RID: 10619 RVA: 0x0057C084 File Offset: 0x0057A284
		private void ProbeEmotions(List<int> list)
		{
			list.Add(0);
			list.Add(1);
			list.Add(2);
			list.Add(3);
			list.Add(15);
			list.Add(16);
			list.Add(17);
			list.Add(87);
			list.Add(91);
			list.Add(136);
			list.Add(134);
			list.Add(135);
			list.Add(137);
			list.Add(138);
			list.Add(139);
			if (Main.bloodMoon && !Main.dayTime)
			{
				int item = Utils.SelectRandom<int>(Main.rand, new int[]
				{
					16,
					1,
					138
				});
				list.Add(item);
				list.Add(item);
				list.Add(item);
			}
		}

		// Token: 0x0600297C RID: 10620 RVA: 0x0057C158 File Offset: 0x0057A358
		private void ProbeBosses(List<int> list)
		{
			int num = 0;
			if ((!NPC.downedBoss1 && !Main.dayTime) || NPC.downedBoss1)
			{
				num = 1;
			}
			if (NPC.downedBoss2)
			{
				num = 2;
			}
			if (NPC.downedQueenBee || NPC.downedBoss3)
			{
				num = 3;
			}
			if (Main.hardMode)
			{
				num = 4;
			}
			if (NPC.downedMechBossAny)
			{
				num = 5;
			}
			if (NPC.downedPlantBoss)
			{
				num = 6;
			}
			if (NPC.downedGolemBoss)
			{
				num = 7;
			}
			if (NPC.downedAncientCultist)
			{
				num = 8;
			}
			int maxValue = 10;
			if (NPC.downedMoonlord)
			{
				maxValue = 1;
			}
			if ((num >= 1 && num <= 2) || (num >= 1 && Main.rand.Next(maxValue) == 0))
			{
				list.Add(39);
				if (WorldGen.crimson)
				{
					list.Add(41);
				}
				else
				{
					list.Add(40);
				}
				list.Add(51);
			}
			if ((num >= 2 && num <= 3) || (num >= 2 && Main.rand.Next(maxValue) == 0))
			{
				list.Add(43);
				list.Add(42);
			}
			if ((num >= 4 && num <= 5) || (num >= 4 && Main.rand.Next(maxValue) == 0))
			{
				list.Add(44);
				list.Add(47);
				list.Add(45);
				list.Add(46);
			}
			if ((num >= 5 && num <= 6) || (num >= 5 && Main.rand.Next(maxValue) == 0))
			{
				if (!NPC.downedMechBoss1)
				{
					list.Add(47);
				}
				if (!NPC.downedMechBoss2)
				{
					list.Add(45);
				}
				if (!NPC.downedMechBoss3)
				{
					list.Add(46);
				}
				list.Add(48);
			}
			if (num == 6 || (num >= 6 && Main.rand.Next(maxValue) == 0))
			{
				list.Add(48);
				list.Add(49);
				list.Add(50);
			}
			if (num == 7 || (num >= 7 && Main.rand.Next(maxValue) == 0))
			{
				list.Add(49);
				list.Add(50);
				list.Add(52);
			}
			if (num == 8 || (num >= 8 && Main.rand.Next(maxValue) == 0))
			{
				list.Add(52);
				list.Add(53);
			}
			if (NPC.downedPirates && Main.expertMode)
			{
				list.Add(59);
			}
			if (NPC.downedMartians)
			{
				list.Add(60);
			}
			if (NPC.downedChristmasIceQueen)
			{
				list.Add(57);
			}
			if (NPC.downedChristmasSantank)
			{
				list.Add(58);
			}
			if (NPC.downedChristmasTree)
			{
				list.Add(56);
			}
			if (NPC.downedHalloweenKing)
			{
				list.Add(55);
			}
			if (NPC.downedHalloweenTree)
			{
				list.Add(54);
			}
			if (NPC.downedEmpressOfLight)
			{
				list.Add(143);
			}
			if (NPC.downedQueenSlime)
			{
				list.Add(144);
			}
			if (NPC.downedDeerclops)
			{
				list.Add(150);
			}
		}

		// Token: 0x0600297D RID: 10621 RVA: 0x0057C3E8 File Offset: 0x0057A5E8
		private void ProbeExceptions(List<int> list, Player plr, WorldUIAnchor other)
		{
			NPC npc = (NPC)this.anchor.entity;
			if (npc.type == 17)
			{
				list.Add(80);
				list.Add(85);
				list.Add(85);
				list.Add(85);
				list.Add(85);
			}
			else if (npc.type == 18)
			{
				list.Add(73);
				list.Add(73);
				list.Add(84);
				list.Add(75);
			}
			else if (npc.type == 19)
			{
				if (other != null && ((NPC)other.entity).type == 22)
				{
					list.Add(1);
					list.Add(1);
					list.Add(93);
					list.Add(92);
				}
				else if (other != null && ((NPC)other.entity).type == 22)
				{
					list.Add(1);
					list.Add(1);
					list.Add(93);
					list.Add(92);
				}
				else
				{
					list.Add(82);
					list.Add(82);
					list.Add(85);
					list.Add(85);
					list.Add(77);
					list.Add(93);
				}
			}
			else if (npc.type == 20)
			{
				if (list.Contains(121))
				{
					list.Add(121);
					list.Add(121);
				}
				list.Add(14);
				list.Add(14);
			}
			else if (npc.type == 22)
			{
				if (!Main.bloodMoon)
				{
					if (other != null && ((NPC)other.entity).type == 19)
					{
						list.Add(1);
						list.Add(1);
						list.Add(93);
						list.Add(92);
					}
					else
					{
						list.Add(79);
					}
				}
				if (!Main.dayTime)
				{
					list.Add(16);
					list.Add(16);
					list.Add(16);
				}
			}
			else if (npc.type == 37)
			{
				list.Add(43);
				list.Add(43);
				list.Add(43);
				list.Add(72);
				list.Add(72);
			}
			else if (npc.type == 38)
			{
				if (Main.bloodMoon)
				{
					list.Add(77);
					list.Add(77);
					list.Add(77);
					list.Add(81);
				}
				else
				{
					list.Add(77);
					list.Add(77);
					list.Add(81);
					list.Add(81);
					list.Add(81);
					list.Add(90);
					list.Add(90);
				}
			}
			else if (npc.type == 54)
			{
				if (Main.bloodMoon)
				{
					list.Add(43);
					list.Add(72);
					list.Add(1);
				}
				else
				{
					if (list.Contains(111))
					{
						list.Add(111);
					}
					list.Add(17);
				}
			}
			else if (npc.type == 107)
			{
				if (other != null && ((NPC)other.entity).type == 124)
				{
					list.Remove(111);
					list.Add(0);
					list.Add(0);
					list.Add(0);
					list.Add(17);
					list.Add(17);
					list.Add(86);
					list.Add(88);
					list.Add(88);
				}
				else
				{
					if (list.Contains(111))
					{
						list.Add(111);
						list.Add(111);
						list.Add(111);
					}
					list.Add(91);
					list.Add(92);
					list.Add(91);
					list.Add(92);
				}
			}
			else if (npc.type == 108)
			{
				list.Add(100);
				list.Add(89);
				list.Add(11);
			}
			if (npc.type == 124)
			{
				if (other != null && ((NPC)other.entity).type == 107)
				{
					list.Remove(111);
					list.Add(0);
					list.Add(0);
					list.Add(0);
					list.Add(17);
					list.Add(17);
					list.Add(88);
					list.Add(88);
					return;
				}
				if (list.Contains(109))
				{
					list.Add(109);
					list.Add(109);
					list.Add(109);
				}
				if (list.Contains(108))
				{
					list.Remove(108);
					if (Main.hardMode)
					{
						list.Add(108);
						list.Add(108);
					}
					else
					{
						list.Add(106);
						list.Add(106);
					}
				}
				list.Add(43);
				list.Add(2);
				return;
			}
			else
			{
				if (npc.type == 142)
				{
					list.Add(32);
					list.Add(66);
					list.Add(17);
					list.Add(15);
					list.Add(15);
					return;
				}
				if (npc.type == 160)
				{
					list.Add(10);
					list.Add(89);
					list.Add(94);
					list.Add(8);
					return;
				}
				if (npc.type == 178)
				{
					list.Add(83);
					list.Add(83);
					return;
				}
				if (npc.type == 207)
				{
					list.Add(28);
					list.Add(95);
					list.Add(93);
					return;
				}
				if (npc.type == 208)
				{
					list.Add(94);
					list.Add(17);
					list.Add(3);
					list.Add(77);
					return;
				}
				if (npc.type == 209)
				{
					list.Add(48);
					list.Add(83);
					list.Add(5);
					list.Add(5);
					return;
				}
				if (npc.type == 227)
				{
					list.Add(63);
					list.Add(68);
					return;
				}
				if (npc.type == 228)
				{
					list.Add(24);
					list.Add(24);
					list.Add(95);
					list.Add(8);
					return;
				}
				if (npc.type == 229)
				{
					list.Add(93);
					list.Add(9);
					list.Add(65);
					list.Add(120);
					list.Add(59);
					return;
				}
				if (npc.type == 353)
				{
					if (list.Contains(104))
					{
						list.Add(104);
						list.Add(104);
					}
					if (list.Contains(111))
					{
						list.Add(111);
						list.Add(111);
					}
					list.Add(67);
					return;
				}
				if (npc.type == 368)
				{
					list.Add(85);
					list.Add(7);
					list.Add(79);
					return;
				}
				if (npc.type == 369)
				{
					if (!Main.bloodMoon)
					{
						list.Add(70);
						list.Add(70);
						list.Add(76);
						list.Add(76);
						list.Add(79);
						list.Add(79);
						if ((double)npc.position.Y < Main.worldSurface)
						{
							list.Add(29);
							return;
						}
					}
				}
				else
				{
					if (npc.type == 453)
					{
						list.Add(72);
						list.Add(69);
						list.Add(87);
						list.Add(3);
						return;
					}
					if (npc.type == 441)
					{
						list.Add(100);
						list.Add(100);
						list.Add(1);
						list.Add(1);
						list.Add(1);
						list.Add(87);
					}
				}
				return;
			}
		}

		// Token: 0x04005267 RID: 21095
		private static int[] CountNPCs = new int[(int)NPCID.Count];

		// Token: 0x04005268 RID: 21096
		public static Dictionary<int, EmoteBubble> byID = new Dictionary<int, EmoteBubble>();

		// Token: 0x04005269 RID: 21097
		private static List<int> toClean = new List<int>();

		// Token: 0x0400526A RID: 21098
		public static int NextID;

		// Token: 0x0400526B RID: 21099
		public int ID;

		// Token: 0x0400526C RID: 21100
		public WorldUIAnchor anchor;

		// Token: 0x0400526D RID: 21101
		public int lifeTime;

		// Token: 0x0400526E RID: 21102
		public int lifeTimeStart;

		// Token: 0x0400526F RID: 21103
		public int emote;

		// Token: 0x04005270 RID: 21104
		public int metadata;

		// Token: 0x04005271 RID: 21105
		private const int frameSpeed = 8;

		// Token: 0x04005272 RID: 21106
		public int frameCounter;

		// Token: 0x04005273 RID: 21107
		public int frame;

		// Token: 0x04005274 RID: 21108
		public const int EMOTE_SHEET_HORIZONTAL_FRAMES = 8;

		// Token: 0x04005275 RID: 21109
		public const int EMOTE_SHEET_EMOTES_PER_ROW = 4;

		// Token: 0x04005276 RID: 21110
		public static readonly int EMOTE_SHEET_VERTICAL_FRAMES = 2 + (EmoteID.Count - 1) / 4;

		// Token: 0x04005277 RID: 21111
		private static EmoteBubble _temporaryBubble = new EmoteBubble(0, new WorldUIAnchor(), 0);
	}
}
