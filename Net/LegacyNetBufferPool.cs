using System;
using System.Collections.Generic;

namespace Terraria.Net
{
	// Token: 0x0200016D RID: 365
	public class LegacyNetBufferPool
	{
		// Token: 0x06001DB8 RID: 7608 RVA: 0x005011E4 File Offset: 0x004FF3E4
		public static byte[] RequestBuffer(int size)
		{
			object obj = LegacyNetBufferPool.bufferLock;
			byte[] result;
			lock (obj)
			{
				if (size <= 256)
				{
					if (LegacyNetBufferPool._smallBufferQueue.Count == 0)
					{
						LegacyNetBufferPool._smallBufferCount++;
						result = new byte[256];
					}
					else
					{
						result = LegacyNetBufferPool._smallBufferQueue.Dequeue();
					}
				}
				else if (size <= 1024)
				{
					if (LegacyNetBufferPool._mediumBufferQueue.Count == 0)
					{
						LegacyNetBufferPool._mediumBufferCount++;
						result = new byte[1024];
					}
					else
					{
						result = LegacyNetBufferPool._mediumBufferQueue.Dequeue();
					}
				}
				else if (size <= 16384)
				{
					if (LegacyNetBufferPool._largeBufferQueue.Count == 0)
					{
						LegacyNetBufferPool._largeBufferCount++;
						result = new byte[16384];
					}
					else
					{
						result = LegacyNetBufferPool._largeBufferQueue.Dequeue();
					}
				}
				else
				{
					LegacyNetBufferPool._customBufferCount++;
					result = new byte[size];
				}
			}
			return result;
		}

		// Token: 0x06001DB9 RID: 7609 RVA: 0x005012E8 File Offset: 0x004FF4E8
		public static byte[] RequestBuffer(byte[] data, int offset, int size)
		{
			byte[] array = LegacyNetBufferPool.RequestBuffer(size);
			Buffer.BlockCopy(data, offset, array, 0, size);
			return array;
		}

		// Token: 0x06001DBA RID: 7610 RVA: 0x00501308 File Offset: 0x004FF508
		public static void ReturnBuffer(byte[] buffer)
		{
			int num = buffer.Length;
			object obj = LegacyNetBufferPool.bufferLock;
			lock (obj)
			{
				if (num <= 256)
				{
					LegacyNetBufferPool._smallBufferQueue.Enqueue(buffer);
				}
				else if (num <= 1024)
				{
					LegacyNetBufferPool._mediumBufferQueue.Enqueue(buffer);
				}
				else if (num <= 16384)
				{
					LegacyNetBufferPool._largeBufferQueue.Enqueue(buffer);
				}
			}
		}

		// Token: 0x06001DBB RID: 7611 RVA: 0x00501384 File Offset: 0x004FF584
		public static void DisplayBufferSizes()
		{
			object obj = LegacyNetBufferPool.bufferLock;
			lock (obj)
			{
				Main.NewText(string.Concat(new object[]
				{
					"Small Buffers:  ",
					LegacyNetBufferPool._smallBufferQueue.Count,
					" queued of ",
					LegacyNetBufferPool._smallBufferCount
				}), byte.MaxValue, byte.MaxValue, byte.MaxValue);
				Main.NewText(string.Concat(new object[]
				{
					"Medium Buffers: ",
					LegacyNetBufferPool._mediumBufferQueue.Count,
					" queued of ",
					LegacyNetBufferPool._mediumBufferCount
				}), byte.MaxValue, byte.MaxValue, byte.MaxValue);
				Main.NewText(string.Concat(new object[]
				{
					"Large Buffers:  ",
					LegacyNetBufferPool._largeBufferQueue.Count,
					" queued of ",
					LegacyNetBufferPool._largeBufferCount
				}), byte.MaxValue, byte.MaxValue, byte.MaxValue);
				Main.NewText("Custom Buffers: 0 queued of " + LegacyNetBufferPool._customBufferCount, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			}
		}

		// Token: 0x06001DBC RID: 7612 RVA: 0x005014DC File Offset: 0x004FF6DC
		public static void PrintBufferSizes()
		{
			object obj = LegacyNetBufferPool.bufferLock;
			lock (obj)
			{
				Console.WriteLine(string.Concat(new object[]
				{
					"Small Buffers:  ",
					LegacyNetBufferPool._smallBufferQueue.Count,
					" queued of ",
					LegacyNetBufferPool._smallBufferCount
				}));
				Console.WriteLine(string.Concat(new object[]
				{
					"Medium Buffers: ",
					LegacyNetBufferPool._mediumBufferQueue.Count,
					" queued of ",
					LegacyNetBufferPool._mediumBufferCount
				}));
				Console.WriteLine(string.Concat(new object[]
				{
					"Large Buffers:  ",
					LegacyNetBufferPool._largeBufferQueue.Count,
					" queued of ",
					LegacyNetBufferPool._largeBufferCount
				}));
				Console.WriteLine("Custom Buffers: 0 queued of " + LegacyNetBufferPool._customBufferCount);
				Console.WriteLine("");
			}
		}

		// Token: 0x04001653 RID: 5715
		private const int SMALL_BUFFER_SIZE = 256;

		// Token: 0x04001654 RID: 5716
		private const int MEDIUM_BUFFER_SIZE = 1024;

		// Token: 0x04001655 RID: 5717
		private const int LARGE_BUFFER_SIZE = 16384;

		// Token: 0x04001656 RID: 5718
		private static object bufferLock = new object();

		// Token: 0x04001657 RID: 5719
		private static Queue<byte[]> _smallBufferQueue = new Queue<byte[]>();

		// Token: 0x04001658 RID: 5720
		private static Queue<byte[]> _mediumBufferQueue = new Queue<byte[]>();

		// Token: 0x04001659 RID: 5721
		private static Queue<byte[]> _largeBufferQueue = new Queue<byte[]>();

		// Token: 0x0400165A RID: 5722
		private static int _smallBufferCount;

		// Token: 0x0400165B RID: 5723
		private static int _mediumBufferCount;

		// Token: 0x0400165C RID: 5724
		private static int _largeBufferCount;

		// Token: 0x0400165D RID: 5725
		private static int _customBufferCount;
	}
}
