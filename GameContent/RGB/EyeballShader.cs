using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;
using Terraria.Utilities;

namespace Terraria.GameContent.RGB
{
	// Token: 0x020002B9 RID: 697
	public class EyeballShader : ChromaShader
	{
		// Token: 0x0600259A RID: 9626 RVA: 0x00557C1C File Offset: 0x00555E1C
		public EyeballShader(bool isSpawning)
		{
			this._isSpawning = isSpawning;
		}

		// Token: 0x0600259B RID: 9627 RVA: 0x00557C70 File Offset: 0x00555E70
		public override void Update(float elapsedTime)
		{
			this.UpdateEyelid(elapsedTime);
			bool flag = this._timeUntilPupilMove <= 0f;
			this._pupilOffset = (this._targetOffset + this._pupilOffset) * 0.5f;
			this._timeUntilPupilMove -= elapsedTime;
			if (flag)
			{
				float num = (float)this._random.NextDouble() * 6.2831855f;
				float scaleFactor;
				if (this._isSpawning)
				{
					this._timeUntilPupilMove = (float)this._random.NextDouble() * 0.4f + 0.3f;
					scaleFactor = (float)this._random.NextDouble() * 0.7f;
				}
				else
				{
					this._timeUntilPupilMove = (float)this._random.NextDouble() * 0.4f + 0.6f;
					scaleFactor = (float)this._random.NextDouble() * 0.3f;
				}
				this._targetOffset = new Vector2((float)Math.Cos((double)num), (float)Math.Sin((double)num)) * scaleFactor;
			}
		}

		// Token: 0x0600259C RID: 9628 RVA: 0x00557D68 File Offset: 0x00555F68
		private void UpdateEyelid(float elapsedTime)
		{
			float num = 0.5f;
			float num2 = 6f;
			if (this._isSpawning)
			{
				if (NPC.MoonLordCountdown >= NPC.MaxMoonLordCountdown - 10)
				{
					this._eyelidStateTime = 0f;
					this._eyelidState = EyeballShader.EyelidState.Closed;
				}
				num = (float)NPC.MoonLordCountdown / (float)NPC.MaxMoonLordCountdown * 10f + 0.5f;
				num2 = 2f;
			}
			this._eyelidStateTime += elapsedTime;
			switch (this._eyelidState)
			{
			case EyeballShader.EyelidState.Closed:
				this._eyelidProgress = 0f;
				if (this._eyelidStateTime > num)
				{
					this._eyelidStateTime = 0f;
					this._eyelidState = EyeballShader.EyelidState.Opening;
					return;
				}
				break;
			case EyeballShader.EyelidState.Opening:
				this._eyelidProgress = this._eyelidStateTime / 0.4f;
				if (this._eyelidStateTime > 0.4f)
				{
					this._eyelidStateTime = 0f;
					this._eyelidState = EyeballShader.EyelidState.Open;
					return;
				}
				break;
			case EyeballShader.EyelidState.Open:
				this._eyelidProgress = 1f;
				if (this._eyelidStateTime > num2)
				{
					this._eyelidStateTime = 0f;
					this._eyelidState = EyeballShader.EyelidState.Closing;
					return;
				}
				break;
			case EyeballShader.EyelidState.Closing:
				this._eyelidProgress = 1f - this._eyelidStateTime / 0.4f;
				if (this._eyelidStateTime > 0.4f)
				{
					this._eyelidStateTime = 0f;
					this._eyelidState = EyeballShader.EyelidState.Closed;
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x0600259D RID: 9629 RVA: 0x00557EB0 File Offset: 0x005560B0
		[RgbProcessor(new EffectDetailLevel[]
		{
			1
		})]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			Vector2 vector = new Vector2(1.5f, 0.5f);
			Vector2 value = vector + this._pupilOffset;
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Vector2 vector2 = canvasPositionOfIndex - vector;
				Vector4 vector3 = Vector4.One;
				float num = (value - canvasPositionOfIndex).Length();
				for (int j = 1; j < EyeballShader.Rings.Length; j++)
				{
					EyeballShader.Ring ring = EyeballShader.Rings[j];
					EyeballShader.Ring ring2 = EyeballShader.Rings[j - 1];
					if (num < ring.Distance)
					{
						vector3 = Vector4.Lerp(ring2.Color, ring.Color, (num - ring2.Distance) / (ring.Distance - ring2.Distance));
						break;
					}
				}
				float num2 = (float)Math.Sqrt((double)(1f - 0.4f * vector2.Y * vector2.Y)) * 5f;
				float num3 = Math.Abs(vector2.X) - num2 * (1.1f * this._eyelidProgress - 0.1f);
				if (num3 > 0f)
				{
					vector3 = Vector4.Lerp(vector3, this._eyelidColor, Math.Min(1f, num3 * 10f));
				}
				fragment.SetColor(i, vector3);
			}
		}

		// Token: 0x04004FE9 RID: 20457
		private static readonly EyeballShader.Ring[] Rings = new EyeballShader.Ring[]
		{
			new EyeballShader.Ring(Color.Black.ToVector4(), 0f),
			new EyeballShader.Ring(Color.Black.ToVector4(), 0.4f),
			new EyeballShader.Ring(new Color(17, 220, 237).ToVector4(), 0.5f),
			new EyeballShader.Ring(new Color(17, 120, 237).ToVector4(), 0.6f),
			new EyeballShader.Ring(Vector4.One, 0.65f)
		};

		// Token: 0x04004FEA RID: 20458
		private readonly Vector4 _eyelidColor = new Color(108, 110, 75).ToVector4();

		// Token: 0x04004FEB RID: 20459
		private float _eyelidProgress;

		// Token: 0x04004FEC RID: 20460
		private Vector2 _pupilOffset = Vector2.Zero;

		// Token: 0x04004FED RID: 20461
		private Vector2 _targetOffset = Vector2.Zero;

		// Token: 0x04004FEE RID: 20462
		private readonly UnifiedRandom _random = new UnifiedRandom();

		// Token: 0x04004FEF RID: 20463
		private float _timeUntilPupilMove;

		// Token: 0x04004FF0 RID: 20464
		private float _eyelidStateTime;

		// Token: 0x04004FF1 RID: 20465
		private readonly bool _isSpawning;

		// Token: 0x04004FF2 RID: 20466
		private EyeballShader.EyelidState _eyelidState;

		// Token: 0x02000810 RID: 2064
		private struct Ring
		{
			// Token: 0x060042E4 RID: 17124 RVA: 0x006BE8C5 File Offset: 0x006BCAC5
			public Ring(Vector4 color, float distance)
			{
				this.Color = color;
				this.Distance = distance;
			}

			// Token: 0x040071B3 RID: 29107
			public readonly Vector4 Color;

			// Token: 0x040071B4 RID: 29108
			public readonly float Distance;
		}

		// Token: 0x02000811 RID: 2065
		private enum EyelidState
		{
			// Token: 0x040071B6 RID: 29110
			Closed,
			// Token: 0x040071B7 RID: 29111
			Opening,
			// Token: 0x040071B8 RID: 29112
			Open,
			// Token: 0x040071B9 RID: 29113
			Closing
		}
	}
}
