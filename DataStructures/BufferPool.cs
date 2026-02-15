using System;
using System.Collections.Generic;

namespace Terraria.DataStructures
{
	// Token: 0x0200058C RID: 1420
	public static class BufferPool
	{
		// Token: 0x06003820 RID: 14368 RVA: 0x00630294 File Offset: 0x0062E494
		public static CachedBuffer Request(int size)
		{
			object obj = BufferPool.bufferLock;
			CachedBuffer result;
			lock (obj)
			{
				if (size <= 32)
				{
					if (BufferPool.SmallBufferQueue.Count == 0)
					{
						result = new CachedBuffer(new byte[32]);
					}
					else
					{
						result = BufferPool.SmallBufferQueue.Dequeue().Activate();
					}
				}
				else if (size <= 256)
				{
					if (BufferPool.MediumBufferQueue.Count == 0)
					{
						result = new CachedBuffer(new byte[256]);
					}
					else
					{
						result = BufferPool.MediumBufferQueue.Dequeue().Activate();
					}
				}
				else if (size <= 16384)
				{
					if (BufferPool.LargeBufferQueue.Count == 0)
					{
						result = new CachedBuffer(new byte[16384]);
					}
					else
					{
						result = BufferPool.LargeBufferQueue.Dequeue().Activate();
					}
				}
				else if (size <= 65536)
				{
					if (BufferPool.HugeBufferQueue.Count == 0)
					{
						result = new CachedBuffer(new byte[65536]);
					}
					else
					{
						result = BufferPool.HugeBufferQueue.Dequeue().Activate();
					}
				}
				else
				{
					result = new CachedBuffer(new byte[size]);
				}
			}
			return result;
		}

		// Token: 0x06003821 RID: 14369 RVA: 0x006303C0 File Offset: 0x0062E5C0
		public static CachedBuffer Request(byte[] data, int offset, int size)
		{
			CachedBuffer cachedBuffer = BufferPool.Request(size);
			Buffer.BlockCopy(data, offset, cachedBuffer.Data, 0, size);
			return cachedBuffer;
		}

		// Token: 0x06003822 RID: 14370 RVA: 0x006303E4 File Offset: 0x0062E5E4
		public static void Recycle(CachedBuffer buffer)
		{
			int length = buffer.Length;
			object obj = BufferPool.bufferLock;
			lock (obj)
			{
				if (length <= 32)
				{
					BufferPool.SmallBufferQueue.Enqueue(buffer);
				}
				else if (length <= 256)
				{
					BufferPool.MediumBufferQueue.Enqueue(buffer);
				}
				else if (length <= 16384)
				{
					BufferPool.LargeBufferQueue.Enqueue(buffer);
				}
				else if (length <= 65536)
				{
					BufferPool.HugeBufferQueue.Enqueue(buffer);
				}
			}
		}

		// Token: 0x06003823 RID: 14371 RVA: 0x00630474 File Offset: 0x0062E674
		public static void PrintBufferSizes()
		{
			object obj = BufferPool.bufferLock;
			lock (obj)
			{
				Console.WriteLine("SmallBufferQueue.Count: " + BufferPool.SmallBufferQueue.Count);
				Console.WriteLine("MediumBufferQueue.Count: " + BufferPool.MediumBufferQueue.Count);
				Console.WriteLine("LargeBufferQueue.Count: " + BufferPool.LargeBufferQueue.Count);
				Console.WriteLine("HugeBufferQueue.Count: " + BufferPool.HugeBufferQueue.Count);
				Console.WriteLine("");
			}
		}

		// Token: 0x04005C2B RID: 23595
		private const int SMALL_BUFFER_SIZE = 32;

		// Token: 0x04005C2C RID: 23596
		private const int MEDIUM_BUFFER_SIZE = 256;

		// Token: 0x04005C2D RID: 23597
		private const int LARGE_BUFFER_SIZE = 16384;

		// Token: 0x04005C2E RID: 23598
		private const int HUGE_BUFFER_SIZE = 65536;

		// Token: 0x04005C2F RID: 23599
		private static object bufferLock = new object();

		// Token: 0x04005C30 RID: 23600
		private static Queue<CachedBuffer> SmallBufferQueue = new Queue<CachedBuffer>();

		// Token: 0x04005C31 RID: 23601
		private static Queue<CachedBuffer> MediumBufferQueue = new Queue<CachedBuffer>();

		// Token: 0x04005C32 RID: 23602
		private static Queue<CachedBuffer> LargeBufferQueue = new Queue<CachedBuffer>();

		// Token: 0x04005C33 RID: 23603
		private static Queue<CachedBuffer> HugeBufferQueue = new Queue<CachedBuffer>();
	}
}
