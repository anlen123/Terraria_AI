using System;

namespace Terraria.DataStructures
{
	// Token: 0x0200058F RID: 1423
	public class DoubleStack<T1>
	{
		// Token: 0x06003830 RID: 14384 RVA: 0x006306F4 File Offset: 0x0062E8F4
		public DoubleStack(int segmentSize = 1024, int initialSize = 0)
		{
			if (segmentSize < 16)
			{
				segmentSize = 16;
			}
			this._start = segmentSize / 2;
			this._end = this._start;
			this._size = 0;
			this._segmentShiftPosition = segmentSize + this._start;
			initialSize += this._start;
			int num = initialSize / segmentSize + 1;
			this._segmentList = new T1[num][];
			for (int i = 0; i < num; i++)
			{
				this._segmentList[i] = new T1[segmentSize];
			}
			this._segmentSize = segmentSize;
			this._segmentCount = num;
			this._last = this._segmentSize * this._segmentCount - 1;
		}

		// Token: 0x06003831 RID: 14385 RVA: 0x00630794 File Offset: 0x0062E994
		public void PushFront(T1 front)
		{
			if (this._start == 0)
			{
				T1[][] array = new T1[this._segmentCount + 1][];
				for (int i = 0; i < this._segmentCount; i++)
				{
					array[i + 1] = this._segmentList[i];
				}
				array[0] = new T1[this._segmentSize];
				this._segmentList = array;
				this._segmentCount++;
				this._start += this._segmentSize;
				this._end += this._segmentSize;
				this._last += this._segmentSize;
			}
			this._start--;
			T1[] array2 = this._segmentList[this._start / this._segmentSize];
			int num = this._start % this._segmentSize;
			array2[num] = front;
			this._size++;
		}

		// Token: 0x06003832 RID: 14386 RVA: 0x0063087C File Offset: 0x0062EA7C
		public T1 PopFront()
		{
			if (this._size == 0)
			{
				throw new InvalidOperationException("The DoubleStack is empty.");
			}
			T1[] array = this._segmentList[this._start / this._segmentSize];
			int num = this._start % this._segmentSize;
			T1 result = array[num];
			array[num] = default(T1);
			this._start++;
			this._size--;
			if (this._start >= this._segmentShiftPosition)
			{
				T1[] array2 = this._segmentList[0];
				for (int i = 0; i < this._segmentCount - 1; i++)
				{
					this._segmentList[i] = this._segmentList[i + 1];
				}
				this._segmentList[this._segmentCount - 1] = array2;
				this._start -= this._segmentSize;
				this._end -= this._segmentSize;
			}
			if (this._size == 0)
			{
				this._start = this._segmentSize / 2;
				this._end = this._start;
			}
			return result;
		}

		// Token: 0x06003833 RID: 14387 RVA: 0x0063098C File Offset: 0x0062EB8C
		public T1 PeekFront()
		{
			if (this._size == 0)
			{
				throw new InvalidOperationException("The DoubleStack is empty.");
			}
			T1[] array = this._segmentList[this._start / this._segmentSize];
			int num = this._start % this._segmentSize;
			return array[num];
		}

		// Token: 0x06003834 RID: 14388 RVA: 0x006309D4 File Offset: 0x0062EBD4
		public void PushBack(T1 back)
		{
			if (this._end == this._last)
			{
				T1[][] array = new T1[this._segmentCount + 1][];
				for (int i = 0; i < this._segmentCount; i++)
				{
					array[i] = this._segmentList[i];
				}
				array[this._segmentCount] = new T1[this._segmentSize];
				this._segmentCount++;
				this._segmentList = array;
				this._last += this._segmentSize;
			}
			T1[] array2 = this._segmentList[this._end / this._segmentSize];
			int num = this._end % this._segmentSize;
			array2[num] = back;
			this._end++;
			this._size++;
		}

		// Token: 0x06003835 RID: 14389 RVA: 0x00630A9C File Offset: 0x0062EC9C
		public T1 PopBack()
		{
			if (this._size == 0)
			{
				throw new InvalidOperationException("The DoubleStack is empty.");
			}
			T1[] array = this._segmentList[this._end / this._segmentSize];
			int num = this._end % this._segmentSize;
			T1 result = array[num];
			array[num] = default(T1);
			this._end--;
			this._size--;
			if (this._size == 0)
			{
				this._start = this._segmentSize / 2;
				this._end = this._start;
			}
			return result;
		}

		// Token: 0x06003836 RID: 14390 RVA: 0x00630B34 File Offset: 0x0062ED34
		public T1 PeekBack()
		{
			if (this._size == 0)
			{
				throw new InvalidOperationException("The DoubleStack is empty.");
			}
			T1[] array = this._segmentList[this._end / this._segmentSize];
			int num = this._end % this._segmentSize;
			return array[num];
		}

		// Token: 0x06003837 RID: 14391 RVA: 0x00630B7C File Offset: 0x0062ED7C
		public void Clear(bool quickClear = false)
		{
			if (!quickClear)
			{
				for (int i = 0; i < this._segmentCount; i++)
				{
					Array.Clear(this._segmentList[i], 0, this._segmentSize);
				}
			}
			this._start = this._segmentSize / 2;
			this._end = this._start;
			this._size = 0;
		}

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x06003838 RID: 14392 RVA: 0x00630BD2 File Offset: 0x0062EDD2
		public int Count
		{
			get
			{
				return this._size;
			}
		}

		// Token: 0x04005C3D RID: 23613
		private T1[][] _segmentList;

		// Token: 0x04005C3E RID: 23614
		private readonly int _segmentSize;

		// Token: 0x04005C3F RID: 23615
		private int _segmentCount;

		// Token: 0x04005C40 RID: 23616
		private readonly int _segmentShiftPosition;

		// Token: 0x04005C41 RID: 23617
		private int _start;

		// Token: 0x04005C42 RID: 23618
		private int _end;

		// Token: 0x04005C43 RID: 23619
		private int _size;

		// Token: 0x04005C44 RID: 23620
		private int _last;
	}
}
