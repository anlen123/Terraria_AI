using System;
using Microsoft.Xna.Framework;
using Terraria.Utilities;

namespace Terraria.GameContent.RGB
{
	// Token: 0x020002C7 RID: 711
	public static class NoiseHelper
	{
		// Token: 0x060025C8 RID: 9672 RVA: 0x0055922C File Offset: 0x0055742C
		private static float[] CreateStaticNoise(int length)
		{
			UnifiedRandom r = new UnifiedRandom(1);
			float[] array = new float[length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = r.NextFloat();
			}
			return array;
		}

		// Token: 0x060025C9 RID: 9673 RVA: 0x00559260 File Offset: 0x00557460
		public static float GetDynamicNoise(int index, float currentTime)
		{
			float num = NoiseHelper.StaticNoise[index & 1023];
			float num2 = currentTime % 1f;
			return Math.Abs(Math.Abs(num - num2) - 0.5f) * 2f;
		}

		// Token: 0x060025CA RID: 9674 RVA: 0x0055929A File Offset: 0x0055749A
		public static float GetStaticNoise(int index)
		{
			return NoiseHelper.StaticNoise[index & 1023];
		}

		// Token: 0x060025CB RID: 9675 RVA: 0x005592A9 File Offset: 0x005574A9
		public static float GetDynamicNoise(int x, int y, float currentTime)
		{
			return NoiseHelper.GetDynamicNoiseInternal(x, y, currentTime % 1f);
		}

		// Token: 0x060025CC RID: 9676 RVA: 0x005592B9 File Offset: 0x005574B9
		private static float GetDynamicNoiseInternal(int x, int y, float wrappedTime)
		{
			x &= 31;
			y &= 31;
			return Math.Abs(Math.Abs(NoiseHelper.StaticNoise[y * 32 + x] - wrappedTime) - 0.5f) * 2f;
		}

		// Token: 0x060025CD RID: 9677 RVA: 0x005592EB File Offset: 0x005574EB
		public static float GetStaticNoise(int x, int y)
		{
			x &= 31;
			y &= 31;
			return NoiseHelper.StaticNoise[y * 32 + x];
		}

		// Token: 0x060025CE RID: 9678 RVA: 0x00559308 File Offset: 0x00557508
		public static float GetDynamicNoise(Vector2 position, float currentTime)
		{
			position *= 10f;
			currentTime %= 1f;
			Vector2 vector = new Vector2((float)Math.Floor((double)position.X), (float)Math.Floor((double)position.Y));
			Point point = new Point((int)vector.X, (int)vector.Y);
			Vector2 vector2 = new Vector2(position.X - vector.X, position.Y - vector.Y);
			float value = MathHelper.Lerp(NoiseHelper.GetDynamicNoiseInternal(point.X, point.Y, currentTime), NoiseHelper.GetDynamicNoiseInternal(point.X, point.Y + 1, currentTime), vector2.Y);
			float value2 = MathHelper.Lerp(NoiseHelper.GetDynamicNoiseInternal(point.X + 1, point.Y, currentTime), NoiseHelper.GetDynamicNoiseInternal(point.X + 1, point.Y + 1, currentTime), vector2.Y);
			return MathHelper.Lerp(value, value2, vector2.X);
		}

		// Token: 0x060025CF RID: 9679 RVA: 0x005593F8 File Offset: 0x005575F8
		public static float GetStaticNoise(Vector2 position)
		{
			position *= 10f;
			Vector2 vector = new Vector2((float)Math.Floor((double)position.X), (float)Math.Floor((double)position.Y));
			Point point = new Point((int)vector.X, (int)vector.Y);
			Vector2 vector2 = new Vector2(position.X - vector.X, position.Y - vector.Y);
			float value = MathHelper.Lerp(NoiseHelper.GetStaticNoise(point.X, point.Y), NoiseHelper.GetStaticNoise(point.X, point.Y + 1), vector2.Y);
			float value2 = MathHelper.Lerp(NoiseHelper.GetStaticNoise(point.X + 1, point.Y), NoiseHelper.GetStaticNoise(point.X + 1, point.Y + 1), vector2.Y);
			return MathHelper.Lerp(value, value2, vector2.X);
		}

		// Token: 0x04005013 RID: 20499
		private const int RANDOM_SEED = 1;

		// Token: 0x04005014 RID: 20500
		private const int NOISE_2D_SIZE = 32;

		// Token: 0x04005015 RID: 20501
		private const int NOISE_2D_SIZE_MASK = 31;

		// Token: 0x04005016 RID: 20502
		private const int NOISE_SIZE_MASK = 1023;

		// Token: 0x04005017 RID: 20503
		private static readonly float[] StaticNoise = NoiseHelper.CreateStaticNoise(1024);
	}
}
