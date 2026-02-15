using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;

namespace Terraria.Graphics.Renderers
{
	// Token: 0x0200020A RID: 522
	public class ItemTransferParticle : IPooledParticle, IParticle
	{
		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06002151 RID: 8529 RVA: 0x0052E1E4 File Offset: 0x0052C3E4
		// (set) Token: 0x06002152 RID: 8530 RVA: 0x0052E1EC File Offset: 0x0052C3EC
		public bool ShouldBeRemovedFromRenderer { get; private set; }

		// Token: 0x06002153 RID: 8531 RVA: 0x0052E1F5 File Offset: 0x0052C3F5
		public ItemTransferParticle()
		{
			this._itemInstance = new Item();
		}

		// Token: 0x06002154 RID: 8532 RVA: 0x0052E208 File Offset: 0x0052C408
		public void Update(ref ParticleRendererSettings settings)
		{
			int num = this._lifeTimeCounted + 1;
			this._lifeTimeCounted = num;
			if (num >= this._lifeTimeTotal)
			{
				this.ShouldBeRemovedFromRenderer = true;
			}
		}

		// Token: 0x06002155 RID: 8533 RVA: 0x0052E238 File Offset: 0x0052C438
		public void Prepare(int itemType, int lifeTimeTotal, Vector2 startPosition, Vector2 endPosition, Vector2 offsetStart, Vector2 offsetEnd, bool transitionIn, bool fullbright, bool inInventory, int stack = 1)
		{
			this._itemInstance.SetDefaults(itemType, null);
			this._itemInstance.stack = stack;
			this._lifeTimeTotal = lifeTimeTotal;
			this.StartPosition = startPosition;
			this.StartOffset = offsetStart;
			this.EndPosition = endPosition;
			this.EndOffset = offsetEnd;
			this.TransitionIn = transitionIn;
			this.Fullbright = fullbright;
			this.InInventory = inInventory;
			Vector2 vector = (this.EndPosition - this.StartPosition).SafeNormalize(Vector2.UnitY).RotatedBy(1.5707963705062866, default(Vector2));
			bool flag = vector.Y < 0f;
			bool flag2 = vector.Y == 0f;
			if (!flag || (flag2 && Main.rand.Next(2) == 0))
			{
				vector *= -1f;
			}
			vector = new Vector2(0f, -1f);
			float scaleFactor = Vector2.Distance(this.EndPosition, this.StartPosition);
			this.BezierHelper1 = vector * scaleFactor + Main.rand.NextVector2Circular(32f, 32f);
			this.BezierHelper2 = -vector * scaleFactor + Main.rand.NextVector2Circular(32f, 32f);
		}

		// Token: 0x06002156 RID: 8534 RVA: 0x0052E380 File Offset: 0x0052C580
		public void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
		{
			float num = (float)this._lifeTimeCounted / (float)this._lifeTimeTotal;
			float num2 = Utils.Remap(num, 0.1f, 0.5f, 0f, 0.85f, true);
			num2 = Utils.Remap(num, 0.5f, 0.9f, num2, 1f, true);
			Vector2 vector;
			Vector2.Hermite(ref this.StartPosition, ref this.BezierHelper1, ref this.EndPosition, ref this.BezierHelper2, num2, out vector);
			Vector2 value = Vector2.Zero;
			if (num <= 0.15f)
			{
				value = Vector2.Lerp(Vector2.Zero, this.StartOffset, num / 0.15f);
			}
			else if (num <= 0.5f)
			{
				value = Vector2.Lerp(this.StartOffset, this.EndOffset, (num - 0.15f) / 0.35f);
			}
			else if (num <= 0.85f)
			{
				value = this.EndOffset;
			}
			else
			{
				value = Vector2.Lerp(this.EndOffset, Vector2.Zero, Utils.Remap(num, 0.85f, 0.95f, 0f, 1f, true));
			}
			vector += value;
			float num3 = Utils.Remap(num, 0f, 0.15f, (float)(this.TransitionIn ? 0 : 1), 1f, true) * Utils.Remap(num, 0.85f, 0.95f, 1f, 0f, true);
			Color color = this.Fullbright ? Color.White : Lighting.GetColor(vector.ToTileCoordinates());
			int context = 31;
			int num4 = 32;
			if (this.InInventory)
			{
				num4 = 32;
				num3 = 1f;
				float num5 = num;
				num5 *= num5;
				vector = Vector2.Lerp(this.StartPosition - new Vector2(26f, 26f) * Main.inventoryScale, this.EndPosition - new Vector2(26f, 26f) * Main.inventoryScale, num5);
				context = 14;
			}
			if (this.InInventory)
			{
				ItemSlot.Draw(spritebatch, ref this._itemInstance, context, settings.AnchorPosition + vector, color);
				return;
			}
			ItemSlot.DrawItemIcon(this._itemInstance, context, Main.spriteBatch, settings.AnchorPosition + vector, this._itemInstance.scale * num3, (float)num4, color, 1f, false);
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06002157 RID: 8535 RVA: 0x0052E5BA File Offset: 0x0052C7BA
		// (set) Token: 0x06002158 RID: 8536 RVA: 0x0052E5C2 File Offset: 0x0052C7C2
		public bool IsRestingInPool { get; private set; }

		// Token: 0x06002159 RID: 8537 RVA: 0x0052E5CB File Offset: 0x0052C7CB
		public void RestInPool()
		{
			this.IsRestingInPool = true;
		}

		// Token: 0x0600215A RID: 8538 RVA: 0x0052E5D4 File Offset: 0x0052C7D4
		public virtual void FetchFromPool()
		{
			this._lifeTimeCounted = 0;
			this._lifeTimeTotal = 0;
			this.IsRestingInPool = false;
			this.ShouldBeRemovedFromRenderer = false;
			this.StartPosition = (this.EndPosition = (this.BezierHelper1 = (this.BezierHelper2 = Vector2.Zero)));
		}

		// Token: 0x04004BA5 RID: 19365
		private Vector2 StartPosition;

		// Token: 0x04004BA6 RID: 19366
		private Vector2 EndPosition;

		// Token: 0x04004BA7 RID: 19367
		private Vector2 StartOffset;

		// Token: 0x04004BA8 RID: 19368
		private Vector2 EndOffset;

		// Token: 0x04004BA9 RID: 19369
		private Vector2 BezierHelper1;

		// Token: 0x04004BAA RID: 19370
		private Vector2 BezierHelper2;

		// Token: 0x04004BAB RID: 19371
		private bool TransitionIn;

		// Token: 0x04004BAC RID: 19372
		private bool Fullbright;

		// Token: 0x04004BAD RID: 19373
		private bool InInventory;

		// Token: 0x04004BAE RID: 19374
		private Item _itemInstance;

		// Token: 0x04004BAF RID: 19375
		private int _lifeTimeCounted;

		// Token: 0x04004BB0 RID: 19376
		private int _lifeTimeTotal;
	}
}
