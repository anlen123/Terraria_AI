using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.Graphics;

namespace Terraria.GameContent.Golf
{
	// Token: 0x02000317 RID: 791
	public class FancyGolfPredictionLine
	{
		// Token: 0x06002744 RID: 10052 RVA: 0x00565FDC File Offset: 0x005641DC
		public FancyGolfPredictionLine(int iterations)
		{
			this._positions = new List<Vector2>(iterations * 2 + 1);
			this._iterations = iterations;
		}

		// Token: 0x06002745 RID: 10053 RVA: 0x0056604C File Offset: 0x0056424C
		public void Update(Entity golfBall, Vector2 impactVelocity, float roughLandResistance)
		{
			bool flag = Main.tileSolid[379];
			Main.tileSolid[379] = false;
			this._positions.Clear();
			this._time += 0.016666668f;
			this._entity.position = golfBall.position;
			this._entity.width = golfBall.width;
			this._entity.height = golfBall.height;
			GolfHelper.HitGolfBall(this._entity, impactVelocity, roughLandResistance);
			this._positions.Add(this._entity.position);
			float num = 0f;
			for (int i = 0; i < this._iterations; i++)
			{
				GolfHelper.StepGolfBall(this._entity, ref num);
				this._positions.Add(this._entity.position);
			}
			Main.tileSolid[379] = flag;
		}

		// Token: 0x06002746 RID: 10054 RVA: 0x0056612C File Offset: 0x0056432C
		public void Draw(Camera camera, SpriteBatch spriteBatch, float chargeProgress)
		{
			this._drawer.Begin(camera.GameViewMatrix.TransformationMatrix);
			int count = this._positions.Count;
			Texture2D value = TextureAssets.Extra[33].Value;
			Vector2 value2 = new Vector2(3.5f, 3.5f);
			Vector2 origin = value.Size() / 2f;
			Vector2 value3 = value2 - camera.UnscaledPosition;
			float num = 0f;
			float num2 = 0f;
			for (int i = 0; i < this._positions.Count - 1; i++)
			{
				float num3;
				float num4;
				this.GetSectionLength(i, out num3, out num4);
				if (num3 != 0f)
				{
					while (num < num2 + num3)
					{
						float num5 = (num - num2) / num3 + (float)i;
						Vector2 position = this.GetPosition((num - num2) / num3 + (float)i);
						Color color = this.GetColor2(num5);
						color *= MathHelper.Clamp(2f - 2f * num5 / (float)(this._positions.Count - 1), 0f, 1f);
						spriteBatch.Draw(value, position + value3, null, color, 0f, origin, this.GetScale(num), SpriteEffects.None, 0f);
						num += 4f;
					}
					num2 += num3;
				}
			}
			this._drawer.End();
		}

		// Token: 0x06002747 RID: 10055 RVA: 0x00566294 File Offset: 0x00564494
		private Color GetColor(float travelledLength)
		{
			float num = travelledLength % 200f / 200f;
			num *= (float)this._colors.Length;
			num -= this._time * 3.1415927f * 1.5f;
			num %= (float)this._colors.Length;
			if (num < 0f)
			{
				num += (float)this._colors.Length;
			}
			int num2 = (int)Math.Floor((double)num);
			int num3 = num2 + 1;
			num2 = Utils.Clamp<int>(num2 % this._colors.Length, 0, this._colors.Length - 1);
			num3 = Utils.Clamp<int>(num3 % this._colors.Length, 0, this._colors.Length - 1);
			float amount = num - (float)num2;
			Color value = Color.Lerp(this._colors[num2], this._colors[num3], amount);
			value.A = 64;
			return value * 0.6f;
		}

		// Token: 0x06002748 RID: 10056 RVA: 0x00566374 File Offset: 0x00564574
		private Color GetColor2(float index)
		{
			float num = index * 0.5f - this._time * 3.1415927f * 1.5f;
			int num2 = (int)Math.Floor((double)num) % this._colors.Length;
			if (num2 < 0)
			{
				num2 += this._colors.Length;
			}
			int num3 = (num2 + 1) % this._colors.Length;
			float amount = num - (float)Math.Floor((double)num);
			Color value = Color.Lerp(this._colors[num2], this._colors[num3], amount);
			value.A = 64;
			return value * 0.6f;
		}

		// Token: 0x06002749 RID: 10057 RVA: 0x00566408 File Offset: 0x00564608
		private float GetScale(float travelledLength)
		{
			return 0.2f + Utils.GetLerpValue(0.8f, 1f, (float)Math.Cos((double)(travelledLength / 50f + this._time * -3.1415927f)) * 0.5f + 0.5f, true) * 0.15f;
		}

		// Token: 0x0600274A RID: 10058 RVA: 0x00566458 File Offset: 0x00564658
		private void GetSectionLength(int startIndex, out float length, out float rotation)
		{
			int num = startIndex + 1;
			if (num >= this._positions.Count)
			{
				num = this._positions.Count - 1;
			}
			length = Vector2.Distance(this._positions[startIndex], this._positions[num]);
			rotation = (this._positions[num] - this._positions[startIndex]).ToRotation();
		}

		// Token: 0x0600274B RID: 10059 RVA: 0x005664C8 File Offset: 0x005646C8
		private Vector2 GetPosition(float indexProgress)
		{
			int num = (int)Math.Floor((double)indexProgress);
			int num2 = num + 1;
			if (num2 >= this._positions.Count)
			{
				num2 = this._positions.Count - 1;
			}
			float amount = indexProgress - (float)num;
			return Vector2.Lerp(this._positions[num], this._positions[num2], amount);
		}

		// Token: 0x040050AB RID: 20651
		private readonly List<Vector2> _positions;

		// Token: 0x040050AC RID: 20652
		private readonly Entity _entity = new FancyGolfPredictionLine.PredictionEntity();

		// Token: 0x040050AD RID: 20653
		private readonly int _iterations;

		// Token: 0x040050AE RID: 20654
		private readonly Color[] _colors = new Color[]
		{
			Color.White,
			Color.Gray
		};

		// Token: 0x040050AF RID: 20655
		private readonly BasicDebugDrawer _drawer = new BasicDebugDrawer(Main.instance.GraphicsDevice);

		// Token: 0x040050B0 RID: 20656
		private float _time;

		// Token: 0x02000877 RID: 2167
		private class PredictionEntity : Entity
		{
		}
	}
}
