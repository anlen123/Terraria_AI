using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics.Effects
{
	// Token: 0x020001EA RID: 490
	public class SkyManager : EffectManager<CustomSky>
	{
		// Token: 0x06002078 RID: 8312 RVA: 0x00521DC8 File Offset: 0x0051FFC8
		public void Reset()
		{
			foreach (CustomSky customSky in this._effects.Values)
			{
				customSky.Reset();
			}
			this._activeSkies.Clear();
		}

		// Token: 0x06002079 RID: 8313 RVA: 0x00521E28 File Offset: 0x00520028
		public void Update(GameTime gameTime)
		{
			int num = Main.dayRate;
			if (num < 1)
			{
				num = 1;
			}
			for (int i = 0; i < num; i++)
			{
				LinkedListNode<CustomSky> next;
				for (LinkedListNode<CustomSky> linkedListNode = this._activeSkies.First; linkedListNode != null; linkedListNode = next)
				{
					CustomSky value = linkedListNode.Value;
					next = linkedListNode.Next;
					value.Update(gameTime);
					if (!value.IsActive())
					{
						this._activeSkies.Remove(linkedListNode);
					}
				}
			}
		}

		// Token: 0x0600207A RID: 8314 RVA: 0x00521E87 File Offset: 0x00520087
		public void Draw(SpriteBatch spriteBatch)
		{
			this.DrawDepthRange(spriteBatch, float.MinValue, float.MaxValue);
		}

		// Token: 0x0600207B RID: 8315 RVA: 0x00521E9A File Offset: 0x0052009A
		public void DrawToDepth(SpriteBatch spriteBatch, float minDepth)
		{
			if (this._lastDepth <= minDepth)
			{
				return;
			}
			this.DrawDepthRange(spriteBatch, minDepth, this._lastDepth);
			this._lastDepth = minDepth;
		}

		// Token: 0x0600207C RID: 8316 RVA: 0x00521EBC File Offset: 0x005200BC
		public void DrawDepthRange(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			foreach (CustomSky customSky in this._activeSkies)
			{
				customSky.Draw(spriteBatch, minDepth, maxDepth);
			}
		}

		// Token: 0x0600207D RID: 8317 RVA: 0x00521F10 File Offset: 0x00520110
		public void DrawRemainingDepth(SpriteBatch spriteBatch)
		{
			this.DrawDepthRange(spriteBatch, float.MinValue, this._lastDepth);
			this._lastDepth = float.MinValue;
		}

		// Token: 0x0600207E RID: 8318 RVA: 0x00521F2F File Offset: 0x0052012F
		public void ResetDepthTracker()
		{
			this._lastDepth = float.MaxValue;
		}

		// Token: 0x0600207F RID: 8319 RVA: 0x00521F3C File Offset: 0x0052013C
		public void SetStartingDepth(float depth)
		{
			this._lastDepth = depth;
		}

		// Token: 0x06002080 RID: 8320 RVA: 0x00521F45 File Offset: 0x00520145
		public override void OnActivate(CustomSky effect, Vector2 position)
		{
			this._activeSkies.Remove(effect);
			this._activeSkies.AddLast(effect);
		}

		// Token: 0x06002081 RID: 8321 RVA: 0x00521F64 File Offset: 0x00520164
		public Color ProcessTileColor(Color color)
		{
			foreach (CustomSky customSky in this._activeSkies)
			{
				color = customSky.OnTileColor(color);
			}
			return color;
		}

		// Token: 0x06002082 RID: 8322 RVA: 0x00521FB8 File Offset: 0x005201B8
		public float ProcessCloudAlpha()
		{
			float num = 1f;
			foreach (CustomSky customSky in this._activeSkies)
			{
				num *= customSky.GetCloudAlpha();
			}
			return MathHelper.Clamp(num, 0f, 1f);
		}

		// Token: 0x04004AF1 RID: 19185
		public static SkyManager Instance = new SkyManager();

		// Token: 0x04004AF2 RID: 19186
		private float _lastDepth;

		// Token: 0x04004AF3 RID: 19187
		private LinkedList<CustomSky> _activeSkies = new LinkedList<CustomSky>();
	}
}
