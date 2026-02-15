using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Testing
{
	// Token: 0x02000118 RID: 280
	public class DebugLineDraw
	{
		// Token: 0x06001AFE RID: 6910 RVA: 0x004F8B3D File Offset: 0x004F6D3D
		private DebugLineDraw(bool ui)
		{
			this._ui = ui;
		}

		// Token: 0x06001AFF RID: 6911 RVA: 0x004F8B57 File Offset: 0x004F6D57
		public void AddLine(Vector2 start, Vector2 end, Color colorStart, Color colorEnd = default(Color), int LifeTime = 1, float width = 1f)
		{
			this.lines.Add(new DebugLineDraw.LineDrawer(start, end, colorStart, colorEnd, LifeTime, width));
		}

		// Token: 0x06001B00 RID: 6912 RVA: 0x004F8B72 File Offset: 0x004F6D72
		public void AddLine(Point start, Point end, Color colorStart, Color colorEnd = default(Color), int LifeTime = 1, float width = 1f)
		{
			this.lines.Add(new DebugLineDraw.LineDrawer(start.ToVector2(), end.ToVector2(), colorStart, colorEnd, LifeTime, width));
		}

		// Token: 0x06001B01 RID: 6913 RVA: 0x004F8B98 File Offset: 0x004F6D98
		public void AddRectangle(Vector2 start, Vector2 end, Color colorStart, Color colorEnd = default(Color), int LifeTime = 1, float width = 1f)
		{
			this.lines.Add(new DebugLineDraw.LineDrawer(start, new Vector2(start.X, end.Y), colorStart, colorEnd, LifeTime, width));
			this.lines.Add(new DebugLineDraw.LineDrawer(start, new Vector2(end.X, start.Y), colorStart, colorEnd, LifeTime, width));
			this.lines.Add(new DebugLineDraw.LineDrawer(end, new Vector2(start.X, end.Y), colorStart, colorEnd, LifeTime, width));
			this.lines.Add(new DebugLineDraw.LineDrawer(end, new Vector2(end.X, start.Y), colorStart, colorEnd, LifeTime, width));
		}

		// Token: 0x06001B02 RID: 6914 RVA: 0x004F8C49 File Offset: 0x004F6E49
		public static void PreUpdate()
		{
			DebugLineDraw.SetPhase(DebugLineDraw.UpdatePhase.Update);
		}

		// Token: 0x06001B03 RID: 6915 RVA: 0x004F8C51 File Offset: 0x004F6E51
		public static void PreWorldUpdate()
		{
			DebugLineDraw.SetPhase(DebugLineDraw.UpdatePhase.UpdateInWorld);
		}

		// Token: 0x06001B04 RID: 6916 RVA: 0x004F8C59 File Offset: 0x004F6E59
		public static void PreDraw()
		{
			DebugLineDraw.SetPhase(DebugLineDraw.UpdatePhase.Draw);
		}

		// Token: 0x06001B05 RID: 6917 RVA: 0x004F8C61 File Offset: 0x004F6E61
		private static void SetPhase(DebugLineDraw.UpdatePhase phase)
		{
			DebugLineDraw.CurrentPhase = phase;
			DebugLineDraw.UI.TickLines();
			DebugLineDraw.World.TickLines();
		}

		// Token: 0x06001B06 RID: 6918 RVA: 0x004F8C80 File Offset: 0x004F6E80
		public void TickLines()
		{
			int num = 0;
			for (int i = 0; i < this.lines.Count; i++)
			{
				DebugLineDraw.LineDrawer lineDrawer = this.lines[i];
				if (lineDrawer.Phase == DebugLineDraw.CurrentPhase)
				{
					lineDrawer.TimeLeft--;
				}
				if (lineDrawer.TimeLeft >= 0)
				{
					this.lines[num++] = lineDrawer;
				}
			}
			this.lines.RemoveRange(num, this.lines.Count - num);
		}

		// Token: 0x06001B07 RID: 6919 RVA: 0x004F8D00 File Offset: 0x004F6F00
		public void Draw(SpriteBatch spriteBatch)
		{
			if (this.lines.Count == 0)
			{
				return;
			}
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, null, null, null, this._ui ? (Matrix.CreateTranslation(Main.screenPosition.X, Main.screenPosition.Y, 0f) * Main.UIScaleMatrix) : Main.GameViewMatrix.TransformationMatrix);
			for (int i = 0; i < this.lines.Count; i++)
			{
				this.lines[i].Draw(spriteBatch);
			}
			spriteBatch.End();
		}

		// Token: 0x0400153C RID: 5436
		public static readonly DebugLineDraw UI = new DebugLineDraw(true);

		// Token: 0x0400153D RID: 5437
		public static readonly DebugLineDraw World = new DebugLineDraw(false);

		// Token: 0x0400153E RID: 5438
		private static DebugLineDraw.UpdatePhase CurrentPhase;

		// Token: 0x0400153F RID: 5439
		private readonly List<DebugLineDraw.LineDrawer> lines = new List<DebugLineDraw.LineDrawer>();

		// Token: 0x04001540 RID: 5440
		private readonly bool _ui;

		// Token: 0x02000728 RID: 1832
		private enum UpdatePhase
		{
			// Token: 0x04006943 RID: 26947
			Update,
			// Token: 0x04006944 RID: 26948
			UpdateInWorld,
			// Token: 0x04006945 RID: 26949
			Draw
		}

		// Token: 0x02000729 RID: 1833
		private class LineDrawer
		{
			// Token: 0x06004073 RID: 16499 RVA: 0x0069D01C File Offset: 0x0069B21C
			public LineDrawer(Vector2 start, Vector2 end, Color colorStart, Color colorEnd = default(Color), int LifeTime = 1, float width = 1f)
			{
				this.vS = start;
				this.vE = end;
				this.cS = colorStart;
				this.cE = ((colorEnd == default(Color)) ? colorStart : colorEnd);
				this.TimeLeft = LifeTime;
				this.Width = width;
			}

			// Token: 0x06004074 RID: 16500 RVA: 0x0069D07C File Offset: 0x0069B27C
			public void Draw(SpriteBatch spriteBatch)
			{
				Utils.DrawLine(spriteBatch, this.vS, this.vE, this.cS, this.cE, this.Width);
			}

			// Token: 0x04006946 RID: 26950
			public Vector2 vS;

			// Token: 0x04006947 RID: 26951
			public Vector2 vE;

			// Token: 0x04006948 RID: 26952
			public Color cS;

			// Token: 0x04006949 RID: 26953
			public Color cE;

			// Token: 0x0400694A RID: 26954
			public int TimeLeft;

			// Token: 0x0400694B RID: 26955
			public float Width;

			// Token: 0x0400694C RID: 26956
			public DebugLineDraw.UpdatePhase Phase = DebugLineDraw.CurrentPhase;
		}
	}
}
