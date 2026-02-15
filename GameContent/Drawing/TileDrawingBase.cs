using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Graphics;

namespace Terraria.GameContent.Drawing
{
	// Token: 0x02000443 RID: 1091
	public class TileDrawingBase
	{
		// Token: 0x06003134 RID: 12596 RVA: 0x005C8932 File Offset: 0x005C6B32
		public void Begin(RasterizerState rasterizer, Matrix transformation)
		{
			this.batchBeginner = new SpriteBatchBeginner(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, rasterizer, null, transformation);
			this.batchBeginner.Begin(Main.tileBatch);
			this.batchBeginner.Begin(Main.spriteBatch);
		}

		// Token: 0x06003135 RID: 12597 RVA: 0x005C8974 File Offset: 0x005C6B74
		public void End()
		{
			TimeLogger.StartTimestamp fromTimestamp = TimeLogger.Start();
			int num = Main.tileBatch.End();
			num += Main.spriteBatch.PendingDrawCallCount();
			Main.spriteBatch.End();
			this.DrawCallLogData.Add(num);
			this.FlushLogData.AddTime(fromTimestamp);
		}

		// Token: 0x06003136 RID: 12598 RVA: 0x005C89C4 File Offset: 0x005C6BC4
		public void RestartLayeredBatch()
		{
			TimeLogger.StartTimestamp fromTimestamp = TimeLogger.Start();
			int value = Main.tileBatch.Restart();
			this.DrawCallLogData.Add(value);
			this.FlushLogData.AddTime(fromTimestamp);
		}

		// Token: 0x06003137 RID: 12599 RVA: 0x005C89FC File Offset: 0x005C6BFC
		public void RestartSpriteBatch()
		{
			TimeLogger.StartTimestamp fromTimestamp = TimeLogger.Start();
			int value = Main.spriteBatch.PendingDrawCallCount();
			Main.spriteBatch.End();
			this.batchBeginner.Begin(Main.spriteBatch);
			this.DrawCallLogData.Add(value);
			this.FlushLogData.AddTime(fromTimestamp);
		}

		// Token: 0x0400574E RID: 22350
		protected TimeLogger.TimeLogData FlushLogData;

		// Token: 0x0400574F RID: 22351
		protected TimeLogger.TimeLogData DrawCallLogData;

		// Token: 0x04005750 RID: 22352
		private SpriteBatchBeginner batchBeginner;
	}
}
