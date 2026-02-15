using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Utilities;
using Terraria.Utilities.Terraria.Utilities;

namespace Terraria.GameContent
{
	// Token: 0x02000239 RID: 569
	public class LightningGenerator
	{
		// Token: 0x06002267 RID: 8807 RVA: 0x00537A60 File Offset: 0x00535C60
		public LightningGenerator.Bolt Generate(List<LightningGenerator.Bolt> bolts, uint seed, Vector2 sourcePosition, Vector2 targetPosition, bool calcPositions, bool calcRotations)
		{
			LightningGenerator.Bolt result = this.GenerateBolt(bolts, seed, 0, calcPositions, sourcePosition, targetPosition, this.RotationStrength, (float)this.StepSize, new FloatRange(0f, 1f));
			if (calcRotations)
			{
				foreach (LightningGenerator.Bolt bolt in bolts)
				{
					bolt.rotations = LightningGenerator.CalcRotations(bolt.positions);
					LightningGenerator.SmoothRotations(bolt.rotations);
				}
			}
			return result;
		}

		// Token: 0x06002268 RID: 8808 RVA: 0x00537AF4 File Offset: 0x00535CF4
		private LightningGenerator.Bolt GenerateBolt(List<LightningGenerator.Bolt> bolts, uint seed, int depth, bool calcPositions, Vector2 startPos, Vector2 targetPos, float rotationStrength, float stepSize, FloatRange progressRange)
		{
			LCG32Random lcg32Random = new LCG32Random(seed);
			float num = 0f;
			float[] array = new float[this.Layers];
			Point b = targetPos.ToTileCoordinates();
			Vector2 vector = startPos;
			Vector2 vector2 = targetPos - startPos;
			float num2 = vector2.Length();
			vector2 /= num2;
			Vector2 value = new Vector2(vector2.Y, -vector2.X);
			int num3 = (int)(num2 * 2f / stepSize);
			int num4 = 0;
			Vector2[] array2 = calcPositions ? new Vector2[num3] : null;
			LightningGenerator.Bolt bolt = new LightningGenerator.Bolt
			{
				positions = array2,
				forkDepth = depth,
				progressRange = progressRange
			};
			int i;
			for (i = 0; i < num3; i++)
			{
				if (calcPositions)
				{
					array2[i] = vector;
				}
				Vector2 vector3 = targetPos - vector;
				float num5 = Vector2.Dot(vector3, vector2);
				if (num5 < stepSize)
				{
					break;
				}
				float num6 = MathHelper.Clamp(1f - num5 / num2, 0f, 1f);
				if (this.SolidTileCollision && vector.ToTileCoordinates() != b && this.TileCollision(vector))
				{
					bolt.progressRange = new FloatRange(progressRange.Minimum, progressRange.Lerp(num6));
					bolt.collidedWithTile = true;
					break;
				}
				vector3 /= vector3.Length();
				float num7 = -Vector2.Dot(vector3, value);
				float num8 = Math.Max(0.01f, Math.Min(num6, 1f - num6) * this.PerpendicularDeviationFactor * 2f);
				float num9 = MathHelper.Clamp(num7 / num8, -1f, 1f);
				int num10;
				if (this.PickLayerToReroll(lcg32Random.NextDouble(), 0.5f, out num10))
				{
					float num11 = rotationStrength;
					for (int j = this.Layers - 1; j > num10; j--)
					{
						num11 /= this.LayerStrengthFactor;
					}
					float num12 = (float)lcg32Random.NextDouble() * 2f - 1f;
					num12 += (num9 - num12 * Math.Abs(num9)) / 2f;
					float num13 = num12 * num11;
					float num14 = array[num10];
					float num15 = num13 - num14;
					num += num15;
					array[num10] = num13;
					if (num10 == this.Layers - 1)
					{
						float num16 = lcg32Random.NextFloat();
						float num17 = Utils.Remap((float)num4, 0f, (float)this.MaxForksPerBolt, 1f, 0f, true);
						float num18 = num - num15 * (1f + this.ForkReflectAngleMultiplier);
						if (bolts != null && Math.Abs(num15) >= rotationStrength * this.ForkGenerationThresholdAngleFraction && this.ForkProgressRange.Contains(num6) && depth < this.MaxForkDepth && num16 < num17 && Math.Abs(num18) < 1.3962635f)
						{
							num4++;
							float num19 = (1f - num6) * this.ForkLengthMultiplier;
							Vector2 targetPos2 = vector + vector3.RotatedBy((double)num18, default(Vector2)) * num2 * num19;
							this.GenerateBolt(bolts, lcg32Random.state + 1U, depth + 1, calcPositions, vector, targetPos2, rotationStrength * this.ForkRotationStrengthMultiplier, stepSize * this.ForkStepSizeMultiplier, new FloatRange(progressRange.Lerp(num6), progressRange.Lerp(num6 + num19)));
						}
					}
				}
				float num20 = Utils.Remap(num6, this.ReduceRandomnessAfter, 1f, 0f, 1f, true);
				num20 += Utils.Remap(Math.Abs(num9), 0.5f, 1f, 0f, 1f, true);
				if (this.PickHighLayerToReroll(lcg32Random.NextDouble(), num20, out num10))
				{
					num -= array[num10];
					array[num10] = 0f;
				}
				vector += vector3.RotatedBy((double)num, default(Vector2)) * stepSize;
			}
			if (calcPositions && i < num3)
			{
				Array.Resize<Vector2>(ref array2, i + 1);
				bolt.positions = array2;
			}
			if (bolts != null && i > 2)
			{
				bolts.Add(bolt);
			}
			return bolt;
		}

		// Token: 0x06002269 RID: 8809 RVA: 0x00537F0C File Offset: 0x0053610C
		private bool TileCollision(Vector2 pos)
		{
			Point point = pos.ToTileCoordinates();
			if (!WorldGen.InWorld(point, 0) || Main.tile[point.X, point.Y] == null)
			{
				return false;
			}
			if (WorldGen.SolidOrSlopedTile(point.X, point.Y))
			{
				return true;
			}
			int liquid = (int)Main.tile[point.X, point.Y].liquid;
			return liquid > 0 && (int)pos.Y % 16 > 16 * (255 - liquid) / 255;
		}

		// Token: 0x0600226A RID: 8810 RVA: 0x00537F97 File Offset: 0x00536197
		private bool PickLayerToReroll(double r, float chance, out int layer)
		{
			for (layer = 0; layer < this.Layers; layer++)
			{
				if (r >= (double)(1f - chance))
				{
					return true;
				}
				r /= (double)chance;
			}
			return false;
		}

		// Token: 0x0600226B RID: 8811 RVA: 0x00537FC2 File Offset: 0x005361C2
		private bool PickHighLayerToReroll(double r, float chance, out int layer)
		{
			if (!this.PickLayerToReroll(r, chance, out layer))
			{
				return false;
			}
			layer = this.Layers - 1 - layer;
			return true;
		}

		// Token: 0x0600226C RID: 8812 RVA: 0x00537FE0 File Offset: 0x005361E0
		private static float[] CalcRotations(Vector2[] positions)
		{
			float[] array = new float[positions.Length];
			if (array.Length < 2)
			{
				return array;
			}
			int i = 0;
			float num = (positions[0] - positions[1]).ToRotation();
			array[i++] = num;
			while (i < array.Length - 1)
			{
				float num2 = (positions[i] - positions[i + 1]).ToRotation();
				array[i++] = num + MathHelper.WrapAngle(num2 - num) / 2f;
				num = num2;
			}
			array[i] = num;
			return array;
		}

		// Token: 0x0600226D RID: 8813 RVA: 0x00538064 File Offset: 0x00536264
		private static void SmoothRotations(float[] rotations)
		{
			float num = rotations[0];
			for (int i = 1; i < rotations.Length - 1; i++)
			{
				float num2 = rotations[i];
				float num3 = rotations[i + 1];
				rotations[i] = num2 + (MathHelper.WrapAngle(num - num2) + MathHelper.WrapAngle(num3 - num2)) / 2f;
				num = num2;
			}
		}

		// Token: 0x04004CD2 RID: 19666
		public bool SolidTileCollision;

		// Token: 0x04004CD3 RID: 19667
		public float RotationStrength;

		// Token: 0x04004CD4 RID: 19668
		public int StepSize;

		// Token: 0x04004CD5 RID: 19669
		public int Layers;

		// Token: 0x04004CD6 RID: 19670
		public float LayerStrengthFactor;

		// Token: 0x04004CD7 RID: 19671
		public float PerpendicularDeviationFactor;

		// Token: 0x04004CD8 RID: 19672
		public float ReduceRandomnessAfter;

		// Token: 0x04004CD9 RID: 19673
		public float ForkGenerationThresholdAngleFraction;

		// Token: 0x04004CDA RID: 19674
		public float ForkReflectAngleMultiplier;

		// Token: 0x04004CDB RID: 19675
		public float ForkRotationStrengthMultiplier;

		// Token: 0x04004CDC RID: 19676
		public float ForkStepSizeMultiplier;

		// Token: 0x04004CDD RID: 19677
		public float ForkLengthMultiplier;

		// Token: 0x04004CDE RID: 19678
		public int MaxForksPerBolt;

		// Token: 0x04004CDF RID: 19679
		public int MaxForkDepth;

		// Token: 0x04004CE0 RID: 19680
		public FloatRange ForkProgressRange;

		// Token: 0x020007C2 RID: 1986
		public class Bolt
		{
			// Token: 0x17000533 RID: 1331
			// (get) Token: 0x06004200 RID: 16896 RVA: 0x006BC261 File Offset: 0x006BA461
			public bool IsMainBolt
			{
				get
				{
					return this.forkDepth == 0;
				}
			}

			// Token: 0x040070A4 RID: 28836
			public Vector2[] positions;

			// Token: 0x040070A5 RID: 28837
			public float[] rotations;

			// Token: 0x040070A6 RID: 28838
			public FloatRange progressRange;

			// Token: 0x040070A7 RID: 28839
			public int forkDepth;

			// Token: 0x040070A8 RID: 28840
			public bool collidedWithTile;
		}

		// Token: 0x020007C3 RID: 1987
		public static class StormLightning
		{
			// Token: 0x06004202 RID: 16898 RVA: 0x006BC26C File Offset: 0x006BA46C
			public static bool CanHitTarget(uint seed, Vector2 targetPosition)
			{
				return !LightningGenerator.StormLightning.Generate(null, seed, targetPosition, false, false).collidedWithTile;
			}

			// Token: 0x06004203 RID: 16899 RVA: 0x006BC280 File Offset: 0x006BA480
			public static LightningGenerator.Bolt GenerateMainBoltPath(uint seed, Vector2 targetPosition)
			{
				return LightningGenerator.StormLightning.Generate(null, seed, targetPosition, true, false);
			}

			// Token: 0x06004204 RID: 16900 RVA: 0x006BC28C File Offset: 0x006BA48C
			public static LightningGenerator.Bolt Generate(List<LightningGenerator.Bolt> bolts, uint seed, Vector2 targetPosition, bool calcPositions = true, bool calcRotations = true)
			{
				LCG32Random lcg32Random = new LCG32Random(seed);
				Vector2 value = -Vector2.UnitY.RotatedBy((lcg32Random.NextDouble() * 2.0 - 1.0) * (double)LightningGenerator.StormLightning.SourceRotationLimit, default(Vector2));
				return LightningGenerator.StormLightning.Generator.Generate(bolts, seed, targetPosition + value * LightningGenerator.StormLightning.Length, targetPosition, calcPositions, calcRotations);
			}

			// Token: 0x040070A9 RID: 28841
			public static LightningGenerator Generator = new LightningGenerator
			{
				RotationStrength = 0.9f,
				StepSize = 8,
				Layers = 4,
				LayerStrengthFactor = 1.5f,
				PerpendicularDeviationFactor = 5f,
				ReduceRandomnessAfter = 0.8f,
				ForkGenerationThresholdAngleFraction = 0.65f,
				ForkReflectAngleMultiplier = 0.4f,
				ForkRotationStrengthMultiplier = 0.9f,
				ForkStepSizeMultiplier = 0.8f,
				ForkLengthMultiplier = 0.8f,
				MaxForksPerBolt = 2,
				MaxForkDepth = 2,
				ForkProgressRange = new FloatRange(0.3f, 0.8f),
				SolidTileCollision = true
			};

			// Token: 0x040070AA RID: 28842
			private static float SourceRotationLimit = 0.34906587f;

			// Token: 0x040070AB RID: 28843
			private static float Length = 1000f;
		}
	}
}
