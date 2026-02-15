using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.Utilities;

namespace Terraria.Testing
{
	// Token: 0x0200010F RID: 271
	public static class DetailedFPS
	{
		// Token: 0x06001AA2 RID: 6818 RVA: 0x004F6C18 File Offset: 0x004F4E18
		static DetailedFPS()
		{
			for (int i = 0; i < DetailedFPS.Frames.Length; i++)
			{
				DetailedFPS.Frames[i].Init();
			}
		}

		// Token: 0x06001AA3 RID: 6819 RVA: 0x004F6C94 File Offset: 0x004F4E94
		public static void StartNextFrame()
		{
			TimeLogger.StartNextFrame();
			DetailedFPS.Frames[DetailedFPS.newest].Finish();
			DetailedFPS.newest++;
			if (DetailedFPS.newest == DetailedFPS.Frames.Length)
			{
				DetailedFPS.newest = 0;
			}
			if (DetailedFPS.newest == DetailedFPS.oldest)
			{
				DetailedFPS.oldest++;
			}
			if (DetailedFPS.oldest == DetailedFPS.Frames.Length)
			{
				DetailedFPS.oldest = 0;
			}
			DetailedFPS.Frames[DetailedFPS.newest].Start();
		}

		// Token: 0x06001AA4 RID: 6820 RVA: 0x004F6D1A File Offset: 0x004F4F1A
		public static void Begin(DetailedFPS.OperationCategory category)
		{
			DetailedFPS.Frames[DetailedFPS.newest].Begin(category);
		}

		// Token: 0x06001AA5 RID: 6821 RVA: 0x004F6D31 File Offset: 0x004F4F31
		public static void End()
		{
			TimeLogger.EndDrawFrame();
			DetailedFPS.Begin(DetailedFPS.OperationCategory.Idle);
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06001AA6 RID: 6822 RVA: 0x004F6D40 File Offset: 0x004F4F40
		public static TimeSpan CurrentFrameTime
		{
			get
			{
				DetailedFPS.Frame frame = DetailedFPS.Frames[DetailedFPS.newest];
				return Utils.SWTicksToTimeSpan(frame.events.Last<DetailedFPS.Frame.Event>().timestamp - frame.events[0].timestamp);
			}
		}

		// Token: 0x06001AA7 RID: 6823 RVA: 0x004F6D84 File Offset: 0x004F4F84
		public static float GetCPUUtilization(int numFrames)
		{
			long[] array = new long[6];
			int num = 0;
			foreach (DetailedFPS.Frame frame in DetailedFPS.EnumerateFrames())
			{
				if (num++ == numFrames)
				{
					break;
				}
				if (frame.events.Count >= 2)
				{
					DetailedFPS.Frame.Event @event = frame.events[0];
					foreach (DetailedFPS.Frame.Event event2 in frame.events)
					{
						array[(int)@event.category] += event2.timestamp - @event.timestamp;
						@event = event2;
					}
				}
			}
			long num2 = array.Sum();
			return (float)((double)(array[2] + array[1]) / (double)num2);
		}

		// Token: 0x06001AA8 RID: 6824 RVA: 0x004F6E7C File Offset: 0x004F507C
		public static bool VsyncAppearsActive()
		{
			long num = 0L;
			int num2 = 60;
			int num3 = 0;
			foreach (DetailedFPS.Frame frame in DetailedFPS.EnumerateFrames())
			{
				if (num3++ == num2)
				{
					break;
				}
				if (frame.events.Count >= 2)
				{
					DetailedFPS.Frame.Event @event = frame.events[0];
					foreach (DetailedFPS.Frame.Event event2 in frame.events)
					{
						if (@event.category == DetailedFPS.OperationCategory.Present)
						{
							num += event2.timestamp - @event.timestamp;
						}
						@event = event2;
					}
				}
			}
			return Utils.SWTicksToTimeSpan(num / (long)num2).TotalSeconds >= Main.TARGET_FRAME_TIME * 0.1;
		}

		// Token: 0x06001AA9 RID: 6825 RVA: 0x004F6F80 File Offset: 0x004F5180
		private static TimeSpan GetGCPauseTime()
		{
			TimeSpan timeSpan = NewRuntimeMethods.GC_GetTotalPauseDuration();
			TimeSpan result = timeSpan - DetailedFPS.LastGCPauseTime;
			DetailedFPS.LastGCPauseTime = timeSpan;
			return result;
		}

		// Token: 0x06001AAA RID: 6826 RVA: 0x004F6FA4 File Offset: 0x004F51A4
		private static int GetCollectionCount(int gen)
		{
			int num = GC.CollectionCount(gen);
			int result = num - DetailedFPS.LastCollectionCount[gen];
			DetailedFPS.LastCollectionCount[gen] = num;
			return result;
		}

		// Token: 0x06001AAB RID: 6827 RVA: 0x004F6FCC File Offset: 0x004F51CC
		private static int GetAllocatedBytes()
		{
			long num = NewRuntimeMethods.GC_GetTotalAllocatedBytes();
			long num2 = num - DetailedFPS.LastAllocatedBytes;
			DetailedFPS.LastAllocatedBytes = num;
			return (int)num2;
		}

		// Token: 0x06001AAC RID: 6828 RVA: 0x004F6FED File Offset: 0x004F51ED
		private static IEnumerable<DetailedFPS.Frame> EnumerateFrames()
		{
			int i = DetailedFPS.newest;
			while (i != DetailedFPS.oldest)
			{
				int num = i - 1;
				i = num;
				if (num < 0)
				{
					i = DetailedFPS.Frames.Length - 1;
				}
				yield return DetailedFPS.Frames[i];
			}
			yield break;
		}

		// Token: 0x06001AAD RID: 6829 RVA: 0x004F6FF8 File Offset: 0x004F51F8
		public static void Draw()
		{
			Rectangle r = new Rectangle((Main.screenWidth - DetailedFPS.Frames.Length * 2) / 2, Main.screenHeight - 100, DetailedFPS.Frames.Length * 2, 100);
			DetailedFPS.DrawFPSBox(r);
			int num = 0;
			long num2 = 0L;
			foreach (DetailedFPS.Frame frame in DetailedFPS.EnumerateFrames())
			{
				num++;
				DetailedFPS.DrawFrame(r.Right - num * 2, frame);
				num2 += frame.Allocated;
			}
			if (num2 > 0L)
			{
				long num3 = num2 / (long)num;
				DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, FontAssets.MouseText.Value, string.Format("Avg Alloc: {0,5} bytes/frame", num3), new Vector2((float)((Main.screenWidth - DetailedFPS.Frames.Length * 2) / 2 - 240), (float)(Main.screenHeight - 24)), Color.White);
			}
			if (Main.keyState.PressingAlt())
			{
				DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, FontAssets.MouseText.Value, string.Format("Time Acc: {0,5:0.0} ms", Main.UpdateTimeAccumulator * 1000.0), new Vector2((float)(Main.screenWidth - 200), (float)(Main.screenHeight - 24)), Color.White);
			}
		}

		// Token: 0x06001AAE RID: 6830 RVA: 0x004F714C File Offset: 0x004F534C
		private static void DrawFPSBox(Rectangle r)
		{
			Color white = Color.White;
			DetailedFPS.DrawRect(new Rectangle(r.Left, r.Y, 2, r.Height), white);
			DetailedFPS.DrawRect(new Rectangle(r.Right, r.Y, 2, r.Height), white);
			DetailedFPS.DrawRect(new Rectangle(r.Left, r.Y, r.Width, 1), white);
			int num = 24;
			DetailedFPS.OperationCategory operationCategory = DetailedFPS.OperationCategory.Idle;
			while (operationCategory <= DetailedFPS.OperationCategory.GC)
			{
				if (operationCategory != DetailedFPS.OperationCategory.GC || !(DetailedFPS.LastGCPauseTime == TimeSpan.Zero))
				{
					DetailedFPS.DrawRect(new Rectangle(r.Right + 8, r.Bottom - num + 8, 8, 8), DetailedFPS.GetColor(operationCategory));
					DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, FontAssets.MouseText.Value, operationCategory.ToString(), new Vector2((float)(r.Right + 20), (float)(r.Bottom - num)), Color.White);
				}
				operationCategory++;
				num += 24;
			}
		}

		// Token: 0x06001AAF RID: 6831 RVA: 0x004F7254 File Offset: 0x004F5454
		private static void DrawFrame(int x, DetailedFPS.Frame frame)
		{
			if (frame.events.Count < 2)
			{
				return;
			}
			int num = 0;
			DetailedFPS.Frame.Event @event = frame.events[0];
			long timestamp = @event.timestamp;
			for (int i = 1; i < frame.events.Count; i++)
			{
				DetailedFPS.Frame.Event event2 = frame.events[i];
				int num2 = (int)(Utils.SWTicksToTimeSpan(@event.timestamp - timestamp).TotalMilliseconds * 6.0);
				int num3 = (int)(Utils.SWTicksToTimeSpan(event2.timestamp - timestamp).TotalMilliseconds * 6.0);
				DetailedFPS.DrawRect(new Rectangle(x, Main.screenHeight - num3, 2, num3 - num2), DetailedFPS.GetColor(@event.category));
				@event = event2;
				num = num3;
			}
			num = Math.Max(num, 100);
			for (int j = 0; j <= GC.MaxGeneration; j++)
			{
				for (int k = 0; k < frame.CollectionCount[j]; k++)
				{
					DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, FontAssets.MouseText.Value, DetailedFPS._gcGenText[j], new Vector2((float)(x - 10), (float)(Main.screenHeight - num - 15)), Color.White, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f, null, null);
					num += 10;
				}
			}
		}

		// Token: 0x06001AB0 RID: 6832 RVA: 0x004F73A0 File Offset: 0x004F55A0
		private static Color GetColor(DetailedFPS.OperationCategory category)
		{
			switch (category)
			{
			case DetailedFPS.OperationCategory.Idle:
				return Color.Gray;
			case DetailedFPS.OperationCategory.Update:
				return Color.Orange;
			case DetailedFPS.OperationCategory.Draw:
				return Color.Green;
			case DetailedFPS.OperationCategory.Present:
				return Color.Magenta;
			case DetailedFPS.OperationCategory.GC:
				return Color.Blue;
			default:
				return Color.Black;
			}
		}

		// Token: 0x06001AB1 RID: 6833 RVA: 0x004F73EC File Offset: 0x004F55EC
		private static void DrawRect(Rectangle r, Color c)
		{
			Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, r, c);
		}

		// Token: 0x040014F4 RID: 5364
		public static readonly int FrameCount = 300;

		// Token: 0x040014F5 RID: 5365
		private static DetailedFPS.Frame[] Frames = new DetailedFPS.Frame[DetailedFPS.FrameCount];

		// Token: 0x040014F6 RID: 5366
		private static int oldest;

		// Token: 0x040014F7 RID: 5367
		private static int newest;

		// Token: 0x040014F8 RID: 5368
		private static TimeSpan LastGCPauseTime;

		// Token: 0x040014F9 RID: 5369
		private static int[] LastCollectionCount = new int[GC.MaxGeneration + 1];

		// Token: 0x040014FA RID: 5370
		private static long LastAllocatedBytes;

		// Token: 0x040014FB RID: 5371
		private const int PixelsPerMs = 6;

		// Token: 0x040014FC RID: 5372
		private const int FrameWidth = 2;

		// Token: 0x040014FD RID: 5373
		private const int BoxHeight = 100;

		// Token: 0x040014FE RID: 5374
		private static string[] _gcGenText = new string[]
		{
			"G0",
			"G1",
			"G2"
		};

		// Token: 0x0200071C RID: 1820
		public enum OperationCategory
		{
			// Token: 0x04006912 RID: 26898
			Idle,
			// Token: 0x04006913 RID: 26899
			Update,
			// Token: 0x04006914 RID: 26900
			Draw,
			// Token: 0x04006915 RID: 26901
			Present,
			// Token: 0x04006916 RID: 26902
			GC,
			// Token: 0x04006917 RID: 26903
			End,
			// Token: 0x04006918 RID: 26904
			Count
		}

		// Token: 0x0200071D RID: 1821
		private struct Frame
		{
			// Token: 0x0600403D RID: 16445 RVA: 0x0069C6F4 File Offset: 0x0069A8F4
			public void Init()
			{
				this.events = new List<DetailedFPS.Frame.Event>(16);
				this.CollectionCount = new int[GC.MaxGeneration + 1];
			}

			// Token: 0x0600403E RID: 16446 RVA: 0x0069C715 File Offset: 0x0069A915
			public void Start()
			{
				this.events.Clear();
				this.Begin(DetailedFPS.OperationCategory.Idle);
			}

			// Token: 0x0600403F RID: 16447 RVA: 0x0069C72C File Offset: 0x0069A92C
			public void Begin(DetailedFPS.OperationCategory category)
			{
				if (this.events.Count >= 1000)
				{
					return;
				}
				if (this.events.Count > 0 && this.events.Last<DetailedFPS.Frame.Event>().category == category)
				{
					return;
				}
				long timestamp = Stopwatch.GetTimestamp();
				if (this.events.Count > 0)
				{
					DetailedFPS.Frame.Event @event = this.events.Last<DetailedFPS.Frame.Event>();
					if (@event.category == DetailedFPS.OperationCategory.Draw || @event.category == DetailedFPS.OperationCategory.Update)
					{
						TimeLogger.TotalDrawAndUpdate.Add((int)(timestamp - @event.timestamp));
					}
				}
				this.events.Add(new DetailedFPS.Frame.Event(category, timestamp));
				TimeSpan gcpauseTime = DetailedFPS.GetGCPauseTime();
				if (gcpauseTime > TimeSpan.Zero)
				{
					long num = Utils.TimeSpanToSWTicks(gcpauseTime);
					this.events.Insert(this.events.Count - 1, new DetailedFPS.Frame.Event(DetailedFPS.OperationCategory.GC, timestamp - num));
					TimeLogger.GCPause.Add((int)num);
				}
			}

			// Token: 0x06004040 RID: 16448 RVA: 0x0069C80C File Offset: 0x0069AA0C
			public void Finish()
			{
				this.Begin(DetailedFPS.OperationCategory.End);
				for (int i = 0; i <= GC.MaxGeneration; i++)
				{
					this.CollectionCount[i] = DetailedFPS.GetCollectionCount(i);
				}
				if (Main.CollectGen0EveryFrame)
				{
					this.CollectionCount[0]--;
				}
				this.Allocated = (long)DetailedFPS.GetAllocatedBytes();
			}

			// Token: 0x04006919 RID: 26905
			public List<DetailedFPS.Frame.Event> events;

			// Token: 0x0400691A RID: 26906
			public int[] CollectionCount;

			// Token: 0x0400691B RID: 26907
			public long Allocated;

			// Token: 0x02000A84 RID: 2692
			public struct Event
			{
				// Token: 0x06004B96 RID: 19350 RVA: 0x006D7564 File Offset: 0x006D5764
				public Event(DetailedFPS.OperationCategory category, long timestamp)
				{
					this.category = category;
					this.timestamp = timestamp;
				}

				// Token: 0x0400772C RID: 30508
				public DetailedFPS.OperationCategory category;

				// Token: 0x0400772D RID: 30509
				public long timestamp;
			}
		}
	}
}
