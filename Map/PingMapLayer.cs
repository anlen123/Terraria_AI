using System;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.UI;

namespace Terraria.Map
{
	// Token: 0x0200017D RID: 381
	public class PingMapLayer : IMapLayer
	{
		// Token: 0x06001E2C RID: 7724 RVA: 0x00503314 File Offset: 0x00501514
		public void Draw(ref MapOverlayDrawContext context, ref string text)
		{
			SpriteFrame spriteFrame = new SpriteFrame(1, 5);
			DateTime now = DateTime.Now;
			foreach (SlotVector<PingMapLayer.Ping>.ItemPair itemPair in this._pings)
			{
				PingMapLayer.Ping value = itemPair.Value;
				double totalSeconds = (now - value.Time).TotalSeconds;
				int num = (int)(totalSeconds * 10.0);
				spriteFrame.CurrentRow = (byte)(num % (int)spriteFrame.RowCount);
				context.Draw(TextureAssets.MapPing.Value, value.Position, spriteFrame, Alignment.Center);
				if (totalSeconds > 15.0)
				{
					this._pings.Remove(itemPair.Id);
				}
			}
		}

		// Token: 0x06001E2D RID: 7725 RVA: 0x005033E4 File Offset: 0x005015E4
		public void Add(Vector2 position)
		{
			if (this._pings.Count == this._pings.Capacity)
			{
				return;
			}
			this._pings.Add(new PingMapLayer.Ping(position));
		}

		// Token: 0x06001E2E RID: 7726 RVA: 0x00503411 File Offset: 0x00501611
		public void Clear()
		{
			this._pings.Clear();
		}

		// Token: 0x04001683 RID: 5763
		private const double PING_DURATION_IN_SECONDS = 15.0;

		// Token: 0x04001684 RID: 5764
		private const double PING_FRAME_RATE = 10.0;

		// Token: 0x04001685 RID: 5765
		private readonly SlotVector<PingMapLayer.Ping> _pings = new SlotVector<PingMapLayer.Ping>(100);

		// Token: 0x02000750 RID: 1872
		private struct Ping
		{
			// Token: 0x060040DC RID: 16604 RVA: 0x0069D76A File Offset: 0x0069B96A
			public Ping(Vector2 position)
			{
				this.Position = position;
				this.Time = DateTime.Now;
			}

			// Token: 0x040069A3 RID: 27043
			public readonly Vector2 Position;

			// Token: 0x040069A4 RID: 27044
			public readonly DateTime Time;
		}
	}
}
