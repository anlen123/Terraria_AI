using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.Graphics.Renderers;
using Terraria.ID;
using Terraria.ObjectData;

namespace Terraria.DataStructures
{
	// Token: 0x02000532 RID: 1330
	public class RichRoomCheckFeedback : IRoomCheckFeedback, IRoomCheckFeedback_Spread, IRoomCheckFeedback_Scoring
	{
		// Token: 0x06003705 RID: 14085 RVA: 0x00009A99 File Offset: 0x00007C99
		private static RoomCheckParticle GetNewParticle()
		{
			return new RoomCheckParticle();
		}

		// Token: 0x06003706 RID: 14086 RVA: 0x0062B63C File Offset: 0x0062983C
		private void Add(int x, int y, int iteration, RichRoomCheckFeedback.Reason type)
		{
			if (this._spaceCount >= this._space.Length)
			{
				Array.Resize<RichRoomCheckFeedback.ParticlePreparation>(ref this._space, this._space.Length * 2);
			}
			if (this._highestIteration < iteration)
			{
				this._highestIteration = iteration;
			}
			RichRoomCheckFeedback.ParticlePreparation[] space = this._space;
			int spaceCount = this._spaceCount;
			this._spaceCount = spaceCount + 1;
			space[spaceCount] = new RichRoomCheckFeedback.ParticlePreparation
			{
				type = type,
				x = x,
				y = y,
				iteration = iteration
			};
		}

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x06003707 RID: 14087 RVA: 0x001DA9FB File Offset: 0x001D8BFB
		public bool StopOnFail
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x06003708 RID: 14088 RVA: 0x001DA9FB File Offset: 0x001D8BFB
		public bool DisplayText
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003709 RID: 14089 RVA: 0x0062B6C4 File Offset: 0x006298C4
		public void BeginSpread(int x, int y)
		{
			this._spaceCount = 0;
			this._highestIteration = 0;
			this._originX = x;
			this._originY = y;
		}

		// Token: 0x0600370A RID: 14090 RVA: 0x0062B6E2 File Offset: 0x006298E2
		public void StartedInASolidTile(int x, int y)
		{
			this.Add(x, y, 0, RichRoomCheckFeedback.Reason.BlockedWall);
		}

		// Token: 0x0600370B RID: 14091 RVA: 0x0062B6EE File Offset: 0x006298EE
		public void TooCloseToWorldEdge(int x, int y, int iteration)
		{
			this.Add(x, y, iteration, RichRoomCheckFeedback.Reason.OpenAir);
		}

		// Token: 0x0600370C RID: 14092 RVA: 0x0062B6FA File Offset: 0x006298FA
		public void AnyBlockScannedHere(int x, int y, int iteration)
		{
			this.Add(x, y, iteration, RichRoomCheckFeedback.Reason.Good);
		}

		// Token: 0x0600370D RID: 14093 RVA: 0x0062B6EE File Offset: 0x006298EE
		public void RoomTooBig(int x, int y, int iteration)
		{
			this.Add(x, y, iteration, RichRoomCheckFeedback.Reason.OpenAir);
		}

		// Token: 0x0600370E RID: 14094 RVA: 0x0062B706 File Offset: 0x00629906
		public void BlockingWall(int x, int y, int iteration)
		{
			this.Add(x, y, iteration, RichRoomCheckFeedback.Reason.BlockedWall);
		}

		// Token: 0x0600370F RID: 14095 RVA: 0x0062B706 File Offset: 0x00629906
		public void BlockingOpenGate(int x, int y, int iteration)
		{
			this.Add(x, y, iteration, RichRoomCheckFeedback.Reason.BlockedWall);
		}

		// Token: 0x06003710 RID: 14096 RVA: 0x0062B712 File Offset: 0x00629912
		public void Stinkbug(int x, int y, int iteration)
		{
			this.Add(x, y, iteration, RichRoomCheckFeedback.Reason.Hazard);
		}

		// Token: 0x06003711 RID: 14097 RVA: 0x0062B712 File Offset: 0x00629912
		public void EchoStinkbug(int x, int y, int iteration)
		{
			this.Add(x, y, iteration, RichRoomCheckFeedback.Reason.Hazard);
		}

		// Token: 0x06003712 RID: 14098 RVA: 0x0062B6EE File Offset: 0x006298EE
		public void MissingAWall(int x, int y, int iteration)
		{
			this.Add(x, y, iteration, RichRoomCheckFeedback.Reason.OpenAir);
		}

		// Token: 0x06003713 RID: 14099 RVA: 0x0062B71E File Offset: 0x0062991E
		public void UnsafeWall(int x, int y, int iteration)
		{
			this.Add(x, y, iteration, RichRoomCheckFeedback.Reason.UnsafeWall);
		}

		// Token: 0x06003714 RID: 14100 RVA: 0x0062B72C File Offset: 0x0062992C
		public void EndSpread()
		{
			Vector2 value = new Vector2((float)this._originX, (float)this._originY);
			for (int i = 0; i < this._spaceCount; i++)
			{
				RichRoomCheckFeedback.ParticlePreparation particlePreparation = this._space[i];
				for (int j = 0; j < this._spaceCount; j++)
				{
					RichRoomCheckFeedback.ParticlePreparation particlePreparation2 = this._space[j];
					if (particlePreparation.x == particlePreparation2.x && particlePreparation.y == particlePreparation2.y && particlePreparation.type == RichRoomCheckFeedback.Reason.Good && particlePreparation2.type != RichRoomCheckFeedback.Reason.Good)
					{
						particlePreparation.consumed = true;
					}
				}
				this._space[i] = particlePreparation;
			}
			float highestDistanceFromOrigin = this.GetHighestDistanceFromOrigin(ref value);
			float num = 3f * highestDistanceFromOrigin + 60f;
			for (int k = 0; k < this._spaceCount; k++)
			{
				RichRoomCheckFeedback.ParticlePreparation particlePreparation3 = this._space[k];
				if (!particlePreparation3.consumed)
				{
					ushort type = Main.tile[particlePreparation3.x, particlePreparation3.y].type;
					bool flag = TileID.Sets.RoomNeeds.CountsAsTable[(int)type] || TileID.Sets.RoomNeeds.CountsAsChair[(int)type] || TileID.Sets.RoomNeeds.CountsAsTorch[(int)type] || TileID.Sets.RoomNeeds.CountsAsDoor[(int)type];
					Asset<Texture2D> textureAsset = TextureAssets.Extra[293];
					Color colorTint = Color.Cyan * 0.7f;
					colorTint.A /= 2;
					Vector2 value2 = new Vector2((float)particlePreparation3.x, (float)particlePreparation3.y);
					float num2 = 1f;
					switch (particlePreparation3.type)
					{
					case RichRoomCheckFeedback.Reason.BlockedWall:
						textureAsset = TextureAssets.Extra[292];
						colorTint = new Color(80, 255, 255) * 0.7f;
						colorTint.A /= 2;
						goto IL_31D;
					case RichRoomCheckFeedback.Reason.UnsafeWall:
						textureAsset = TextureAssets.Extra[298];
						colorTint = new Color(255, 40, 40, 255);
						goto IL_23C;
					case RichRoomCheckFeedback.Reason.OpenAir:
					case RichRoomCheckFeedback.Reason.Hazard:
						textureAsset = TextureAssets.Extra[292];
						colorTint = new Color(255, 40, 40, 255);
						goto IL_23C;
					}
					num2 = 1.5f;
					if (flag)
					{
						goto IL_31D;
					}
					IL_23C:
					RoomCheckParticle roomCheckParticle = RichRoomCheckFeedback._particlePool.RequestParticle();
					roomCheckParticle.SetBasicInfo(textureAsset, null, Vector2.Zero, new Vector2((float)(particlePreparation3.x * 16 + 8), (float)(particlePreparation3.y * 16 + 8)));
					roomCheckParticle.Delay = (int)(3f * Vector2.Distance(value, value2));
					float num3 = num - (float)roomCheckParticle.Delay;
					roomCheckParticle.SetTypeInfo(num3 * num2, true);
					roomCheckParticle.FadeInNormalizedTime = Utils.Remap(num - 24f, (float)roomCheckParticle.Delay, num, 0f, 1f, true);
					roomCheckParticle.FadeOutNormalizedTime = Utils.Remap(num - 6f, (float)roomCheckParticle.Delay, num, 0f, 1f, true);
					roomCheckParticle.ColorTint = colorTint;
					roomCheckParticle.Scale = Vector2.One;
					Main.ParticleSystem_World_OverPlayers.Add(roomCheckParticle);
				}
				IL_31D:;
			}
			for (int l = 0; l < this._spaceCount; l++)
			{
				RichRoomCheckFeedback.ParticlePreparation particlePreparation4 = this._space[l];
				if (!particlePreparation4.consumed)
				{
					ushort type2 = Main.tile[particlePreparation4.x, particlePreparation4.y].type;
					if (TileID.Sets.RoomNeeds.CountsAsTable[(int)type2] || TileID.Sets.RoomNeeds.CountsAsChair[(int)type2] || TileID.Sets.RoomNeeds.CountsAsTorch[(int)type2] || TileID.Sets.RoomNeeds.CountsAsDoor[(int)type2])
					{
						Rectangle rectangle;
						TileObjectData.TryGetTileBounds(particlePreparation4.x, particlePreparation4.y, out rectangle);
						for (int m = 0; m < this._spaceCount; m++)
						{
							if (m != l)
							{
								RichRoomCheckFeedback.ParticlePreparation particlePreparation5 = this._space[m];
								if (particlePreparation5.x >= rectangle.Left && particlePreparation5.x < rectangle.Right && particlePreparation5.y >= rectangle.Top && particlePreparation5.y < rectangle.Bottom)
								{
									this._space[m].consumed = true;
								}
							}
						}
					}
				}
			}
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			for (int n = 0; n < this._spaceCount; n++)
			{
				RichRoomCheckFeedback.ParticlePreparation particlePreparation6 = this._space[n];
				if (!particlePreparation6.consumed)
				{
					ushort type3 = Main.tile[particlePreparation6.x, particlePreparation6.y].type;
					bool flag6 = TileID.Sets.RoomNeeds.CountsAsTable[(int)type3];
					bool flag7 = TileID.Sets.RoomNeeds.CountsAsChair[(int)type3];
					bool flag8 = TileID.Sets.RoomNeeds.CountsAsTorch[(int)type3];
					bool flag9 = TileID.Sets.RoomNeeds.CountsAsDoor[(int)type3];
					if (flag6 || flag7 || flag9 || flag8)
					{
						Asset<Texture2D> asset = TextureAssets.Extra[293];
						if (flag6)
						{
							if (flag3)
							{
								goto IL_711;
							}
							flag3 = true;
							asset = TextureAssets.Extra[297];
						}
						if (flag7)
						{
							if (flag2)
							{
								goto IL_711;
							}
							flag2 = true;
							asset = TextureAssets.Extra[295];
						}
						if (flag9)
						{
							if (flag5)
							{
								goto IL_711;
							}
							flag5 = true;
							asset = TextureAssets.Extra[296];
						}
						if (flag8)
						{
							if (flag4)
							{
								goto IL_711;
							}
							flag4 = true;
							asset = TextureAssets.Extra[294];
						}
						Rectangle rectangle2;
						TileObjectData.TryGetTileBounds(particlePreparation6.x, particlePreparation6.y, out rectangle2);
						Color color;
						(Color.LimeGreen * 0.8f).A = color.A / 2;
						Vector2 value3 = new Vector2((float)particlePreparation6.x, (float)particlePreparation6.y);
						RoomCheckParticle roomCheckParticle2 = RichRoomCheckFeedback._particlePool.RequestParticle();
						Vector2 value4 = new Vector2((float)(rectangle2.Left + rectangle2.Right) / 2f, MathHelper.Min((float)rectangle2.Top / 2f + (float)rectangle2.Bottom / 2f, (float)(rectangle2.Top + 1)));
						roomCheckParticle2.SetBasicInfo(asset, null, Vector2.Zero, value4 * 16f + new Vector2(0f, (float)(-(float)asset.Height() / 2)));
						roomCheckParticle2.Delay = (int)(3f * Vector2.Distance(value, value3));
						float num4 = num - (float)roomCheckParticle2.Delay;
						roomCheckParticle2.SetTypeInfo(num4, true);
						roomCheckParticle2.FadeInNormalizedTime = Utils.Remap(num - 24f, (float)roomCheckParticle2.Delay, num, 0f, 1f, true);
						roomCheckParticle2.FadeOutNormalizedTime = Utils.Remap(num - 6f, (float)roomCheckParticle2.Delay, num, 0f, 1f, true);
						roomCheckParticle2.Scale = Vector2.One;
						int num5 = 32;
						RoomCheckParticle roomCheckParticle3 = roomCheckParticle2;
						roomCheckParticle3.LocalPosition.Y = roomCheckParticle3.LocalPosition.Y - (float)num5;
						roomCheckParticle2.Velocity = new Vector2(0f, (float)num5 * 2.5f) / num4;
						roomCheckParticle2.AccelerationPerFrame = -roomCheckParticle2.Velocity * 1.25f / num4;
						Main.ParticleSystem_World_OverPlayers.Add(roomCheckParticle2);
					}
				}
				IL_711:;
			}
		}

		// Token: 0x06003715 RID: 14101 RVA: 0x0062BE60 File Offset: 0x0062A060
		private float GetHighestDistanceFromOrigin(ref Vector2 origin)
		{
			float num = 0f;
			for (int i = 0; i < this._spaceCount; i++)
			{
				RichRoomCheckFeedback.ParticlePreparation particlePreparation = this._space[i];
				Vector2 value = new Vector2((float)particlePreparation.x, (float)particlePreparation.y);
				float num2 = Vector2.Distance(origin, value);
				if (num < num2)
				{
					num = num2;
				}
			}
			return num;
		}

		// Token: 0x06003716 RID: 14102 RVA: 0x0062BEBD File Offset: 0x0062A0BD
		public void BeginScoring()
		{
			this._bestScore = default(RichRoomCheckFeedback.ScorePreparation);
			this._scoreCount = 0;
		}

		// Token: 0x06003717 RID: 14103 RVA: 0x0062BED4 File Offset: 0x0062A0D4
		public void ReportScore(int x, int y, int score)
		{
			if (this._scoreCount >= this._score.Length)
			{
				Array.Resize<RichRoomCheckFeedback.ScorePreparation>(ref this._score, this._score.Length * 2);
			}
			RichRoomCheckFeedback.ScorePreparation[] score2 = this._score;
			int scoreCount = this._scoreCount;
			this._scoreCount = scoreCount + 1;
			score2[scoreCount] = new RichRoomCheckFeedback.ScorePreparation
			{
				x = x,
				y = y,
				score = score
			};
		}

		// Token: 0x06003718 RID: 14104 RVA: 0x0062BF44 File Offset: 0x0062A144
		public void SetAsHighScore(int x, int y, int score)
		{
			this._bestScore = new RichRoomCheckFeedback.ScorePreparation
			{
				x = x,
				y = y,
				score = score
			};
		}

		// Token: 0x06003719 RID: 14105 RVA: 0x0062BF78 File Offset: 0x0062A178
		public void EndScoring()
		{
			Vector2 value = new Vector2((float)this._originX, (float)this._originY);
			float num = 0f;
			for (int i = 0; i < this._spaceCount; i++)
			{
				RichRoomCheckFeedback.ParticlePreparation particlePreparation = this._space[i];
				Vector2 value2 = new Vector2((float)particlePreparation.x, (float)particlePreparation.y);
				float num2 = Vector2.Distance(value, value2);
				if (num < num2)
				{
					num = num2;
				}
			}
			float num3 = 3f * num + 90f;
			int score = this._bestScore.score;
			if (score == 0)
			{
				return;
			}
			for (int j = 0; j < this._scoreCount; j++)
			{
				RichRoomCheckFeedback.ScorePreparation scorePreparation = this._score[j];
				if (scorePreparation.score != 0 && (scorePreparation.x != this._bestScore.x || scorePreparation.y != this._bestScore.y))
				{
					Asset<Texture2D> textureAsset = TextureAssets.Extra[293];
					RoomCheckParticle roomCheckParticle = RichRoomCheckFeedback._particlePool.RequestParticle();
					roomCheckParticle.SetBasicInfo(textureAsset, null, Vector2.Zero, new Vector2((float)(scorePreparation.x * 16 + 8), (float)(scorePreparation.y * 16 + 8 - 16)));
					Vector2 value3 = new Vector2((float)scorePreparation.x, (float)scorePreparation.y);
					roomCheckParticle.Delay = (int)(3f * Vector2.Distance(value, value3));
					float timeToLive = num3 - (float)roomCheckParticle.Delay;
					roomCheckParticle.SetTypeInfo(timeToLive, true);
					roomCheckParticle.FadeInNormalizedTime = Utils.Remap(num3 - 24f, (float)roomCheckParticle.Delay, num3, 0f, 1f, true);
					roomCheckParticle.FadeOutNormalizedTime = Utils.Remap(num3 - 6f, (float)roomCheckParticle.Delay, num3, 0f, 1f, true);
					if (scorePreparation.score > 0)
					{
						roomCheckParticle.ColorTint = Color.LimeGreen * (0.5f + 0.5f * (float)scorePreparation.score / (float)score);
					}
					else
					{
						roomCheckParticle.ColorTint = Color.Red * ((float)scorePreparation.score / (float)(-(float)score));
					}
					roomCheckParticle.Scale = Vector2.One * 2f;
					Main.ParticleSystem_World_OverPlayers.Add(roomCheckParticle);
				}
			}
			for (int k = 0; k < 1; k++)
			{
				RichRoomCheckFeedback.ScorePreparation bestScore = this._bestScore;
				if (bestScore.score != 0)
				{
					Asset<Texture2D> textureAsset2 = TextureAssets.Extra[293];
					RoomCheckParticle roomCheckParticle2 = RichRoomCheckFeedback._particlePool.RequestParticle();
					roomCheckParticle2.SetBasicInfo(textureAsset2, null, Vector2.Zero, new Vector2((float)(bestScore.x * 16 + 8), (float)(bestScore.y * 16 + 8 - 16)));
					Vector2 value4 = new Vector2((float)bestScore.x, (float)bestScore.y);
					roomCheckParticle2.Delay = (int)(3f * Vector2.Distance(value, value4));
					float timeToLive2 = num3 - (float)roomCheckParticle2.Delay;
					roomCheckParticle2.SetTypeInfo(timeToLive2, true);
					roomCheckParticle2.FadeInNormalizedTime = Utils.Remap(num3 - 24f, (float)roomCheckParticle2.Delay, num3, 0f, 1f, true);
					roomCheckParticle2.FadeOutNormalizedTime = Utils.Remap(num3 - 6f, (float)roomCheckParticle2.Delay, num3, 0f, 1f, true);
					roomCheckParticle2.ColorTint = Main.OurFavoriteColor;
					roomCheckParticle2.Scale = Vector2.One * 3f;
					Main.ParticleSystem_World_OverPlayers.Add(roomCheckParticle2);
				}
			}
		}

		// Token: 0x04005B31 RID: 23345
		public static RichRoomCheckFeedback Instance = new RichRoomCheckFeedback();

		// Token: 0x04005B32 RID: 23346
		private static ParticlePool<RoomCheckParticle> _particlePool = new ParticlePool<RoomCheckParticle>(100, new ParticlePool<RoomCheckParticle>.ParticleInstantiator(RichRoomCheckFeedback.GetNewParticle));

		// Token: 0x04005B33 RID: 23347
		private RichRoomCheckFeedback.ParticlePreparation[] _space = new RichRoomCheckFeedback.ParticlePreparation[128];

		// Token: 0x04005B34 RID: 23348
		private int _spaceCount;

		// Token: 0x04005B35 RID: 23349
		private int _highestIteration;

		// Token: 0x04005B36 RID: 23350
		private RichRoomCheckFeedback.ScorePreparation[] _score = new RichRoomCheckFeedback.ScorePreparation[128];

		// Token: 0x04005B37 RID: 23351
		private int _scoreCount;

		// Token: 0x04005B38 RID: 23352
		private RichRoomCheckFeedback.ScorePreparation _bestScore;

		// Token: 0x04005B39 RID: 23353
		private int _originX;

		// Token: 0x04005B3A RID: 23354
		private int _originY;

		// Token: 0x020009AB RID: 2475
		private enum Reason
		{
			// Token: 0x0400766A RID: 30314
			BlockedWall,
			// Token: 0x0400766B RID: 30315
			UnsafeWall,
			// Token: 0x0400766C RID: 30316
			OpenAir,
			// Token: 0x0400766D RID: 30317
			Good,
			// Token: 0x0400766E RID: 30318
			Hazard
		}

		// Token: 0x020009AC RID: 2476
		private struct ParticlePreparation
		{
			// Token: 0x0400766F RID: 30319
			public RichRoomCheckFeedback.Reason type;

			// Token: 0x04007670 RID: 30320
			public int x;

			// Token: 0x04007671 RID: 30321
			public int y;

			// Token: 0x04007672 RID: 30322
			public int iteration;

			// Token: 0x04007673 RID: 30323
			public bool consumed;
		}

		// Token: 0x020009AD RID: 2477
		private struct ScorePreparation
		{
			// Token: 0x04007674 RID: 30324
			public int x;

			// Token: 0x04007675 RID: 30325
			public int y;

			// Token: 0x04007676 RID: 30326
			public int score;
		}
	}
}
