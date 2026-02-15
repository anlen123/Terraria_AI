using System;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.GameContent.Metadata;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Physics;

namespace Terraria.GameContent.Golf
{
	// Token: 0x02000319 RID: 793
	public static class GolfHelper
	{
		// Token: 0x06002750 RID: 10064 RVA: 0x005665CE File Offset: 0x005647CE
		public static BallStepResult StepGolfBall(Entity entity, ref float angularVelocity)
		{
			return BallCollision.Step(GolfHelper.PhysicsProperties, entity, ref angularVelocity, GolfHelper.Listener);
		}

		// Token: 0x06002751 RID: 10065 RVA: 0x005665E1 File Offset: 0x005647E1
		public static Vector2 FindVectorOnOval(Vector2 vector, Vector2 radius)
		{
			if (Math.Abs(radius.X) < 0.0001f || Math.Abs(radius.Y) < 0.0001f)
			{
				return Vector2.Zero;
			}
			return Vector2.Normalize(vector / radius) * radius;
		}

		// Token: 0x06002752 RID: 10066 RVA: 0x00566620 File Offset: 0x00564820
		public static GolfHelper.ShotStrength CalculateShotStrength(Vector2 shotVector, GolfHelper.ClubProperties clubProperties)
		{
			Vector2.Normalize(shotVector);
			float value = shotVector.Length();
			float num = GolfHelper.FindVectorOnOval(shotVector, clubProperties.MaximumStrength).Length();
			float num2 = GolfHelper.FindVectorOnOval(shotVector, clubProperties.MinimumStrength).Length();
			float num3 = MathHelper.Clamp(value, num2, num);
			float relativeStrength = Math.Max((num3 - num2) / (num - num2), 0.001f);
			return new GolfHelper.ShotStrength(num3 * 32f, relativeStrength, clubProperties.RoughLandResistance);
		}

		// Token: 0x06002753 RID: 10067 RVA: 0x00566690 File Offset: 0x00564890
		public static bool IsPlayerHoldingClub(Player player)
		{
			if (player == null || player.HeldItem == null)
			{
				return false;
			}
			int type = player.HeldItem.type;
			return type == 4039 || type - 4092 <= 2 || type - 4587 <= 11;
		}

		// Token: 0x06002754 RID: 10068 RVA: 0x005666D8 File Offset: 0x005648D8
		public static GolfHelper.ShotStrength CalculateShotStrength(Projectile golfHelper, Entity golfBall)
		{
			int num = Main.screenWidth;
			if (num > Main.screenHeight)
			{
				num = Main.screenHeight;
			}
			int num2 = 150;
			num -= num2;
			num /= 2;
			if (num < 200)
			{
				num = 200;
			}
			float divider = (float)num;
			divider = 300f;
			if (golfHelper.ai[0] != 0f)
			{
				return default(GolfHelper.ShotStrength);
			}
			Vector2 shotVector = (golfHelper.Center - golfBall.Center) / divider;
			GolfHelper.ClubProperties clubPropertiesFromGolfHelper = GolfHelper.GetClubPropertiesFromGolfHelper(golfHelper);
			return GolfHelper.CalculateShotStrength(shotVector, clubPropertiesFromGolfHelper);
		}

		// Token: 0x06002755 RID: 10069 RVA: 0x0056675C File Offset: 0x0056495C
		public static GolfHelper.ClubProperties GetClubPropertiesFromGolfHelper(Projectile golfHelper)
		{
			return GolfHelper.GetClubProperties((short)Main.player[golfHelper.owner].HeldItem.type);
		}

		// Token: 0x06002756 RID: 10070 RVA: 0x0056677C File Offset: 0x0056497C
		public static GolfHelper.ClubProperties GetClubProperties(short itemId)
		{
			Vector2 vector = new Vector2(0.25f, 0.25f);
			if (itemId == 4039)
			{
				return new GolfHelper.ClubProperties(vector, Vector2.One, 0f);
			}
			switch (itemId)
			{
			case 4092:
				return new GolfHelper.ClubProperties(Vector2.Zero, vector, 0f);
			case 4093:
				return new GolfHelper.ClubProperties(vector, new Vector2(0.65f, 1.5f), 1f);
			case 4094:
				return new GolfHelper.ClubProperties(vector, new Vector2(1.5f, 0.65f), 0f);
			default:
				switch (itemId)
				{
				case 4587:
					return new GolfHelper.ClubProperties(vector, Vector2.One, 0f);
				case 4588:
					return new GolfHelper.ClubProperties(Vector2.Zero, vector, 0f);
				case 4589:
					return new GolfHelper.ClubProperties(vector, new Vector2(0.65f, 1.5f), 1f);
				case 4590:
					return new GolfHelper.ClubProperties(vector, new Vector2(1.5f, 0.65f), 0f);
				case 4591:
					return new GolfHelper.ClubProperties(vector, Vector2.One, 0f);
				case 4592:
					return new GolfHelper.ClubProperties(Vector2.Zero, vector, 0f);
				case 4593:
					return new GolfHelper.ClubProperties(vector, new Vector2(0.65f, 1.5f), 1f);
				case 4594:
					return new GolfHelper.ClubProperties(vector, new Vector2(1.5f, 0.65f), 0f);
				case 4595:
					return new GolfHelper.ClubProperties(vector, Vector2.One, 0f);
				case 4596:
					return new GolfHelper.ClubProperties(Vector2.Zero, vector, 0f);
				case 4597:
					return new GolfHelper.ClubProperties(vector, new Vector2(0.65f, 1.5f), 1f);
				case 4598:
					return new GolfHelper.ClubProperties(vector, new Vector2(1.5f, 0.65f), 0f);
				default:
					return default(GolfHelper.ClubProperties);
				}
				break;
			}
		}

		// Token: 0x06002757 RID: 10071 RVA: 0x00566964 File Offset: 0x00564B64
		public static Projectile FindHelperFromGolfBall(Projectile golfBall)
		{
			for (int i = 0; i < 1000; i++)
			{
				Projectile projectile = Main.projectile[i];
				if (projectile.active && projectile.type == 722 && projectile.owner == golfBall.owner)
				{
					return Main.projectile[i];
				}
			}
			return null;
		}

		// Token: 0x06002758 RID: 10072 RVA: 0x005669B8 File Offset: 0x00564BB8
		public static Projectile FindGolfBallForHelper(Projectile golfHelper)
		{
			for (int i = 0; i < 1000; i++)
			{
				Projectile projectile = Main.projectile[i];
				Vector2 vector = golfHelper.Center - projectile.Center;
				if (projectile.active && ProjectileID.Sets.IsAGolfBall[projectile.type] && projectile.owner == golfHelper.owner && GolfHelper.ValidateShot(projectile, Main.player[golfHelper.owner], ref vector))
				{
					return Main.projectile[i];
				}
			}
			return null;
		}

		// Token: 0x06002759 RID: 10073 RVA: 0x00566A32 File Offset: 0x00564C32
		public static bool IsGolfBallResting(Projectile golfBall)
		{
			return (int)golfBall.localAI[1] == 0 || Vector2.Distance(golfBall.position, golfBall.oldPos[golfBall.oldPos.Length - 1]) < 1f;
		}

		// Token: 0x0600275A RID: 10074 RVA: 0x00566A68 File Offset: 0x00564C68
		public static bool IsGolfShotValid(Entity golfBall, Player player)
		{
			Vector2 vector = golfBall.Center - player.Bottom;
			if (player.direction == -1)
			{
				vector.X *= -1f;
			}
			return vector.X >= -16f && vector.X <= 32f && vector.Y <= 16f && vector.Y >= -16f;
		}

		// Token: 0x0600275B RID: 10075 RVA: 0x00566ADC File Offset: 0x00564CDC
		public static bool ValidateShot(Entity golfBall, Player player, ref Vector2 shotVector)
		{
			Vector2 vector = golfBall.Center - player.Bottom;
			if (player.direction == -1)
			{
				vector.X *= -1f;
				shotVector.X *= -1f;
			}
			float num = shotVector.ToRotation();
			if (num > 0f)
			{
				shotVector = shotVector.Length() * new Vector2((float)Math.Cos(0.0), (float)Math.Sin(0.0));
			}
			else if (num < -1.5207964f)
			{
				shotVector = shotVector.Length() * new Vector2((float)Math.Cos(-1.5207964181900024), (float)Math.Sin(-1.5207964181900024));
			}
			if (player.direction == -1)
			{
				shotVector.X *= -1f;
			}
			return vector.X >= -16f && vector.X <= 32f && vector.Y <= 16f && vector.Y >= -16f;
		}

		// Token: 0x0600275C RID: 10076 RVA: 0x00566BFC File Offset: 0x00564DFC
		public static void HitGolfBall(Entity entity, Vector2 velocity, float roughLandResistance)
		{
			Vector2 bottom = entity.Bottom;
			bottom.Y += 1f;
			Point point = bottom.ToTileCoordinates();
			Tile tile = Main.tile[point.X, point.Y];
			if (tile != null && tile.active())
			{
				TileMaterial byTileId = TileMaterials.GetByTileId(tile.type);
				velocity = Vector2.Lerp(velocity * byTileId.GolfPhysics.ClubImpactDampening, velocity, byTileId.GolfPhysics.ImpactDampeningResistanceEfficiency * roughLandResistance);
			}
			entity.velocity = velocity;
			Projectile projectile = entity as Projectile;
			if (projectile != null)
			{
				projectile.timeLeft = 18000;
				if (projectile.ai[1] < 0f)
				{
					projectile.ai[1] = 0f;
				}
				projectile.ai[1] += 1f;
				projectile.localAI[1] = 1f;
				Main.LocalGolfState.RecordSwing(projectile);
			}
		}

		// Token: 0x0600275D RID: 10077 RVA: 0x00566CE3 File Offset: 0x00564EE3
		public static void DrawPredictionLine(Entity golfBall, Vector2 impactVelocity, float chargeProgress, float roughLandResistance)
		{
			if (GolfHelper.PredictionLine == null)
			{
				GolfHelper.PredictionLine = new FancyGolfPredictionLine(20);
			}
			GolfHelper.PredictionLine.Update(golfBall, impactVelocity, roughLandResistance);
			GolfHelper.PredictionLine.Draw(Main.Camera, Main.spriteBatch, chargeProgress);
		}

		// Token: 0x040050B2 RID: 20658
		public const int PointsNeededForLevel1 = 500;

		// Token: 0x040050B3 RID: 20659
		public const int PointsNeededForLevel2 = 1000;

		// Token: 0x040050B4 RID: 20660
		public const int PointsNeededForLevel3 = 2000;

		// Token: 0x040050B5 RID: 20661
		public static readonly PhysicsProperties PhysicsProperties = new PhysicsProperties(0.3f, 0.99f);

		// Token: 0x040050B6 RID: 20662
		public static readonly GolfHelper.ContactListener Listener = new GolfHelper.ContactListener();

		// Token: 0x040050B7 RID: 20663
		public static FancyGolfPredictionLine PredictionLine;

		// Token: 0x02000878 RID: 2168
		public struct ClubProperties
		{
			// Token: 0x06004461 RID: 17505 RVA: 0x006C04C7 File Offset: 0x006BE6C7
			public ClubProperties(Vector2 minimumStrength, Vector2 maximumStrength, float roughLandResistance)
			{
				this.MinimumStrength = minimumStrength;
				this.MaximumStrength = maximumStrength;
				this.RoughLandResistance = roughLandResistance;
			}

			// Token: 0x0400724C RID: 29260
			public readonly Vector2 MinimumStrength;

			// Token: 0x0400724D RID: 29261
			public readonly Vector2 MaximumStrength;

			// Token: 0x0400724E RID: 29262
			public readonly float RoughLandResistance;
		}

		// Token: 0x02000879 RID: 2169
		public struct ShotStrength
		{
			// Token: 0x06004462 RID: 17506 RVA: 0x006C04DE File Offset: 0x006BE6DE
			public ShotStrength(float absoluteStrength, float relativeStrength, float roughLandResistance)
			{
				this.AbsoluteStrength = absoluteStrength;
				this.RelativeStrength = relativeStrength;
				this.RoughLandResistance = roughLandResistance;
			}

			// Token: 0x0400724F RID: 29263
			public readonly float AbsoluteStrength;

			// Token: 0x04007250 RID: 29264
			public readonly float RelativeStrength;

			// Token: 0x04007251 RID: 29265
			public readonly float RoughLandResistance;
		}

		// Token: 0x0200087A RID: 2170
		public class ContactListener : IBallContactListener
		{
			// Token: 0x06004463 RID: 17507 RVA: 0x006C04F8 File Offset: 0x006BE6F8
			public void OnCollision(PhysicsProperties properties, ref Vector2 position, ref Vector2 velocity, ref BallCollisionEvent collision)
			{
				TileMaterial byTileId = TileMaterials.GetByTileId(collision.Tile.type);
				Vector2 value = velocity * byTileId.GolfPhysics.SideImpactDampening;
				Vector2 value2 = collision.Normal * Vector2.Dot(velocity, collision.Normal) * (byTileId.GolfPhysics.DirectImpactDampening - byTileId.GolfPhysics.SideImpactDampening);
				velocity = value + value2;
				Projectile projectile = collision.Entity as Projectile;
				ushort type = collision.Tile.type;
				if (type - 421 > 1)
				{
					if (type == 476)
					{
						float num = velocity.Length() / collision.TimeScale;
						if (collision.Normal.Y <= -0.01f && num <= 100f)
						{
							velocity *= 0f;
							if (projectile != null && projectile.active)
							{
								this.PutBallInCup(projectile, collision);
							}
						}
					}
				}
				else
				{
					float num2 = 2.5f * collision.TimeScale;
					Vector2 vector = new Vector2(-collision.Normal.Y, collision.Normal.X);
					if (collision.Tile.type == 422)
					{
						vector = -vector;
					}
					float num3 = Vector2.Dot(velocity, vector);
					if (num3 < num2)
					{
						velocity += vector * MathHelper.Clamp(num2 - num3, 0f, num2 * 0.5f);
					}
				}
				if (projectile != null && projectile.type >= 0 && ProjectileID.Sets.BreaksFromToyBreaker[projectile.type] && collision.Tile.type >= 0 && TileID.Sets.BreaksToys[(int)collision.Tile.type])
				{
					projectile.Kill();
				}
				if (projectile != null && projectile.active && velocity.Y < -0.3f && velocity.Y > -2f && velocity.Length() > 1f)
				{
					Dust dust = Dust.NewDustPerfect(collision.Entity.Center, 31, new Vector2?(collision.Normal), 127, default(Color), 1f);
					dust.scale = 0.7f;
					dust.fadeIn = 1f;
					dust.velocity = dust.velocity * 0.5f + Main.rand.NextVector2CircularEdge(0.5f, 0.4f);
				}
			}

			// Token: 0x06004464 RID: 17508 RVA: 0x006C0794 File Offset: 0x006BE994
			public void PutBallInCup(Projectile proj, BallCollisionEvent collision)
			{
				if (proj.owner == Main.myPlayer && Main.LocalGolfState.ShouldScoreHole)
				{
					Point point = (collision.ImpactPoint - collision.Normal * 0.5f).ToTileCoordinates();
					int owner = proj.owner;
					int num = (int)proj.ai[1];
					int type = proj.type;
					if (num > 1)
					{
						Main.LocalGolfState.SetScoreTime();
					}
					Main.LocalGolfState.RecordBallInfo(proj);
					Main.LocalGolfState.LandBall(proj);
					int golfBallScore = Main.LocalGolfState.GetGolfBallScore(proj);
					if (num > 0)
					{
						Main.player[owner].AccumulateGolfingScore(golfBallScore);
					}
					GolfHelper.ContactListener.PutBallInCup_TextAndEffects(point, owner, num, type);
					Main.LocalGolfState.ResetScoreTime();
					Wiring.HitSwitch(point.X, point.Y);
					NetMessage.SendData(59, -1, -1, null, point.X, (float)point.Y, 0f, 0f, 0, 0, 0);
					if (Main.netMode == 1)
					{
						NetMessage.SendData(128, -1, -1, null, owner, (float)num, (float)type, 0f, point.X, point.Y, 0);
					}
				}
				proj.Kill();
			}

			// Token: 0x06004465 RID: 17509 RVA: 0x006C08B8 File Offset: 0x006BEAB8
			public static void PutBallInCup_TextAndEffects(Point hitLocation, int plr, int numberOfHits, int projid)
			{
				if (numberOfHits == 0)
				{
					return;
				}
				GolfHelper.ContactListener.EmitGolfballExplosion(hitLocation.ToWorldCoordinates(8f, 0f));
				string key = "Game.BallBounceResultGolf_Single";
				NetworkText networkText;
				if (numberOfHits != 1)
				{
					key = "Game.BallBounceResultGolf_Plural";
					networkText = NetworkText.FromKey(key, new object[]
					{
						Main.player[plr].name,
						NetworkText.FromKey(Lang.GetProjectileName(projid).Key, new object[0]),
						numberOfHits
					});
				}
				else
				{
					networkText = NetworkText.FromKey(key, new object[]
					{
						Main.player[plr].name,
						NetworkText.FromKey(Lang.GetProjectileName(projid).Key, new object[0])
					});
				}
				if (Main.netMode == 0 || Main.netMode == 1)
				{
					Main.NewText(networkText.ToString(), byte.MaxValue, 240, 20);
					return;
				}
				if (Main.netMode == 2)
				{
					ChatHelper.BroadcastChatMessage(networkText, new Color(255, 240, 20), -1);
				}
			}

			// Token: 0x06004466 RID: 17510 RVA: 0x006C09AC File Offset: 0x006BEBAC
			public void OnPassThrough(PhysicsProperties properties, ref Vector2 position, ref Vector2 velocity, ref float angularVelocity, ref BallPassThroughEvent collision)
			{
				switch (collision.Type)
				{
				case BallPassThroughType.Water:
					velocity *= 0.91f;
					angularVelocity *= 0.91f;
					return;
				case BallPassThroughType.Honey:
					velocity *= 0.8f;
					angularVelocity *= 0.8f;
					return;
				case BallPassThroughType.Lava:
					break;
				case BallPassThroughType.Tile:
				{
					TileMaterial byTileId = TileMaterials.GetByTileId(collision.Tile.type);
					velocity *= byTileId.GolfPhysics.PassThroughDampening;
					angularVelocity *= byTileId.GolfPhysics.PassThroughDampening;
					break;
				}
				default:
					return;
				}
			}

			// Token: 0x06004467 RID: 17511 RVA: 0x006C0A60 File Offset: 0x006BEC60
			public static void EmitGolfballExplosion_Old(Vector2 Center)
			{
				GolfHelper.ContactListener.EmitGolfballExplosion(Center);
			}

			// Token: 0x06004468 RID: 17512 RVA: 0x006C0A74 File Offset: 0x006BEC74
			public static void EmitGolfballExplosion(Vector2 Center)
			{
				SoundEngine.PlaySound(SoundID.Item129, Center, 0f, 1f);
				for (float num = 0f; num < 1f; num += 0.085f)
				{
					Dust dust = Dust.NewDustPerfect(Center, 278, new Vector2?((num * 6.2831855f).ToRotationVector2() * new Vector2(2f, 0.5f)), 0, default(Color), 1f);
					dust.fadeIn = 1.2f;
					dust.noGravity = true;
					dust.velocity.X = dust.velocity.X * 0.7f;
					dust.velocity.Y = dust.velocity.Y - 1.5f;
					dust.position.Y = dust.position.Y + 8f;
					dust.velocity.X = dust.velocity.X * 2f;
					dust.color = Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.5f, byte.MaxValue);
				}
				float num2 = Main.rand.NextFloat();
				float num3 = (float)Main.rand.Next(5, 10);
				int num4 = 0;
				while ((float)num4 < num3)
				{
					int num5 = Main.rand.Next(5, 22);
					Vector2 value = (((float)num4 - num3 / 2f) * 6.2831855f / 256f - 1.5707964f).ToRotationVector2() * new Vector2(5f, 1f) * (0.25f + Main.rand.NextFloat() * 0.05f);
					Color color = Main.hslToRgb((num2 + (float)num4 / num3) % 1f, 0.7f, 0.7f, byte.MaxValue);
					color.A = 127;
					for (int i = 0; i < num5; i++)
					{
						Dust dust2 = Dust.NewDustPerfect(Center + new Vector2((float)num4 - num3 / 2f, 0f) * 2f, 278, new Vector2?(value), 0, default(Color), 1f);
						dust2.fadeIn = 0.7f;
						dust2.scale = 0.7f;
						dust2.noGravity = true;
						dust2.position.Y = dust2.position.Y + -1f;
						dust2.velocity *= (float)i;
						dust2.scale += 0.2f - (float)i * 0.03f;
						dust2.velocity += Main.rand.NextVector2Circular(0.05f, 0.05f);
						dust2.color = color;
					}
					num4++;
				}
				for (float num6 = 0f; num6 < 1f; num6 += 0.2f)
				{
					Dust dust3 = Dust.NewDustPerfect(Center, 278, new Vector2?((num6 * 6.2831855f).ToRotationVector2() * new Vector2(1f, 0.5f)), 0, default(Color), 1f);
					dust3.fadeIn = 1.2f;
					dust3.noGravity = true;
					dust3.velocity.X = dust3.velocity.X * 0.7f;
					dust3.velocity.Y = dust3.velocity.Y - 0.5f;
					dust3.position.Y = dust3.position.Y + 8f;
					dust3.velocity.X = dust3.velocity.X * 2f;
					dust3.color = Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.3f, byte.MaxValue);
				}
				float num7 = Main.rand.NextFloatDirection();
				for (float num8 = 0f; num8 < 1f; num8 += 0.15f)
				{
					Dust dust4 = Dust.NewDustPerfect(Center, 278, new Vector2?((num7 + num8 * 6.2831855f).ToRotationVector2() * 4f), 0, default(Color), 1f);
					dust4.fadeIn = 1.5f;
					dust4.velocity *= 0.5f + num8 * 0.8f;
					dust4.noGravity = true;
					Dust dust5 = dust4;
					dust5.velocity.X = dust5.velocity.X * 0.35f;
					Dust dust6 = dust4;
					dust6.velocity.Y = dust6.velocity.Y * 2f;
					Dust dust7 = dust4;
					dust7.velocity.Y = dust7.velocity.Y - 1f;
					dust4.velocity.Y = -Math.Abs(dust4.velocity.Y);
					dust4.position += dust4.velocity * 3f;
					dust4.color = Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.6f + Main.rand.NextFloat() * 0.2f, byte.MaxValue);
				}
			}

			// Token: 0x06004469 RID: 17513 RVA: 0x006C0F60 File Offset: 0x006BF160
			public static void EmitGolfballExplosion_v1(Vector2 Center)
			{
				for (float num = 0f; num < 1f; num += 0.085f)
				{
					Dust dust = Dust.NewDustPerfect(Center, 278, new Vector2?((num * 6.2831855f).ToRotationVector2() * new Vector2(2f, 0.5f)), 0, default(Color), 1f);
					dust.fadeIn = 1.2f;
					dust.noGravity = true;
					dust.velocity.X = dust.velocity.X * 0.7f;
					dust.velocity.Y = dust.velocity.Y - 1.5f;
					dust.position.Y = dust.position.Y + 8f;
					dust.color = Color.Lerp(Color.Silver, Color.White, 0.5f);
				}
				for (float num2 = 0f; num2 < 1f; num2 += 0.2f)
				{
					Dust dust2 = Dust.NewDustPerfect(Center, 278, new Vector2?((num2 * 6.2831855f).ToRotationVector2() * new Vector2(1f, 0.5f)), 0, default(Color), 1f);
					dust2.fadeIn = 1.2f;
					dust2.noGravity = true;
					dust2.velocity.X = dust2.velocity.X * 0.7f;
					dust2.velocity.Y = dust2.velocity.Y - 0.5f;
					dust2.position.Y = dust2.position.Y + 8f;
					dust2.color = Color.Lerp(Color.Silver, Color.White, 0.5f);
				}
				float num3 = Main.rand.NextFloatDirection();
				for (float num4 = 0f; num4 < 1f; num4 += 0.15f)
				{
					Dust dust3 = Dust.NewDustPerfect(Center, 278, new Vector2?((num3 + num4 * 6.2831855f).ToRotationVector2() * 4f), 0, default(Color), 1f);
					dust3.fadeIn = 1.5f;
					dust3.velocity *= 0.5f + num4 * 0.8f;
					dust3.noGravity = true;
					Dust dust4 = dust3;
					dust4.velocity.X = dust4.velocity.X * 0.35f;
					Dust dust5 = dust3;
					dust5.velocity.Y = dust5.velocity.Y * 2f;
					Dust dust6 = dust3;
					dust6.velocity.Y = dust6.velocity.Y - 1f;
					dust3.velocity.Y = -Math.Abs(dust3.velocity.Y);
					dust3.position += dust3.velocity * 3f;
					dust3.color = Color.Lerp(Color.Silver, Color.White, 0.5f);
				}
			}
		}
	}
}
