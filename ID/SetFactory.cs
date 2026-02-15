using System;
using System.Collections.Generic;

namespace Terraria.ID
{
	// Token: 0x020001B8 RID: 440
	public class SetFactory
	{
		// Token: 0x06001F29 RID: 7977 RVA: 0x00515AA4 File Offset: 0x00513CA4
		public SetFactory(int size)
		{
			if (size == 0)
			{
				throw new ArgumentOutOfRangeException("size cannot be 0, the intializer for Count must run first");
			}
			this._size = size;
		}

		// Token: 0x06001F2A RID: 7978 RVA: 0x00515B04 File Offset: 0x00513D04
		protected bool[] GetBoolBuffer()
		{
			object queueLock = this._queueLock;
			bool[] result;
			lock (queueLock)
			{
				if (this._boolBufferCache.Count == 0)
				{
					result = new bool[this._size];
				}
				else
				{
					result = this._boolBufferCache.Dequeue();
				}
			}
			return result;
		}

		// Token: 0x06001F2B RID: 7979 RVA: 0x00515B68 File Offset: 0x00513D68
		protected int[] GetIntBuffer()
		{
			object queueLock = this._queueLock;
			int[] result;
			lock (queueLock)
			{
				if (this._intBufferCache.Count == 0)
				{
					result = new int[this._size];
				}
				else
				{
					result = this._intBufferCache.Dequeue();
				}
			}
			return result;
		}

		// Token: 0x06001F2C RID: 7980 RVA: 0x00515BCC File Offset: 0x00513DCC
		protected ushort[] GetUshortBuffer()
		{
			object queueLock = this._queueLock;
			ushort[] result;
			lock (queueLock)
			{
				if (this._ushortBufferCache.Count == 0)
				{
					result = new ushort[this._size];
				}
				else
				{
					result = this._ushortBufferCache.Dequeue();
				}
			}
			return result;
		}

		// Token: 0x06001F2D RID: 7981 RVA: 0x00515C30 File Offset: 0x00513E30
		protected float[] GetFloatBuffer()
		{
			object queueLock = this._queueLock;
			float[] result;
			lock (queueLock)
			{
				if (this._floatBufferCache.Count == 0)
				{
					result = new float[this._size];
				}
				else
				{
					result = this._floatBufferCache.Dequeue();
				}
			}
			return result;
		}

		// Token: 0x06001F2E RID: 7982 RVA: 0x00515C94 File Offset: 0x00513E94
		public void Recycle<T>(T[] buffer)
		{
			object queueLock = this._queueLock;
			lock (queueLock)
			{
				if (typeof(T).Equals(typeof(bool)))
				{
					this._boolBufferCache.Enqueue((bool[])buffer);
				}
				else if (typeof(T).Equals(typeof(int)))
				{
					this._intBufferCache.Enqueue((int[])buffer);
				}
			}
		}

		// Token: 0x06001F2F RID: 7983 RVA: 0x00515D28 File Offset: 0x00513F28
		public bool[] CreateBoolSet(params int[] types)
		{
			return this.CreateBoolSet(false, types);
		}

		// Token: 0x06001F30 RID: 7984 RVA: 0x00515D34 File Offset: 0x00513F34
		public bool[] CreateBoolSet(bool defaultState, params int[] types)
		{
			bool[] boolBuffer = this.GetBoolBuffer();
			for (int i = 0; i < boolBuffer.Length; i++)
			{
				boolBuffer[i] = defaultState;
			}
			for (int j = 0; j < types.Length; j++)
			{
				boolBuffer[types[j]] = !defaultState;
			}
			return boolBuffer;
		}

		// Token: 0x06001F31 RID: 7985 RVA: 0x00515D72 File Offset: 0x00513F72
		public int[] CreateIntSet(params int[] types)
		{
			return this.CreateIntSet(-1, types);
		}

		// Token: 0x06001F32 RID: 7986 RVA: 0x00515D7C File Offset: 0x00513F7C
		public int[] CreateIntSet(int defaultState, params int[] inputs)
		{
			if (inputs.Length % 2 != 0)
			{
				throw new Exception("You have a bad length for inputs on CreateArraySet");
			}
			int[] intBuffer = this.GetIntBuffer();
			for (int i = 0; i < intBuffer.Length; i++)
			{
				intBuffer[i] = defaultState;
			}
			for (int j = 0; j < inputs.Length; j += 2)
			{
				intBuffer[inputs[j]] = inputs[j + 1];
			}
			return intBuffer;
		}

		// Token: 0x06001F33 RID: 7987 RVA: 0x00515DD0 File Offset: 0x00513FD0
		public ushort[] CreateUshortSet(ushort defaultState, params ushort[] inputs)
		{
			if (inputs.Length % 2 != 0)
			{
				throw new Exception("You have a bad length for inputs on CreateArraySet");
			}
			ushort[] ushortBuffer = this.GetUshortBuffer();
			for (int i = 0; i < ushortBuffer.Length; i++)
			{
				ushortBuffer[i] = defaultState;
			}
			for (int j = 0; j < inputs.Length; j += 2)
			{
				ushortBuffer[(int)inputs[j]] = inputs[j + 1];
			}
			return ushortBuffer;
		}

		// Token: 0x06001F34 RID: 7988 RVA: 0x00515E24 File Offset: 0x00514024
		public float[] CreateFloatSet(float defaultState, params float[] inputs)
		{
			if (inputs.Length % 2 != 0)
			{
				throw new Exception("You have a bad length for inputs on CreateArraySet");
			}
			float[] floatBuffer = this.GetFloatBuffer();
			for (int i = 0; i < floatBuffer.Length; i++)
			{
				floatBuffer[i] = defaultState;
			}
			for (int j = 0; j < inputs.Length; j += 2)
			{
				floatBuffer[(int)inputs[j]] = inputs[j + 1];
			}
			return floatBuffer;
		}

		// Token: 0x06001F35 RID: 7989 RVA: 0x00515E78 File Offset: 0x00514078
		public T[] CreateCustomSet<T>(T defaultState, params object[] inputs)
		{
			if (inputs.Length % 2 != 0)
			{
				throw new Exception("You have a bad length for inputs on CreateCustomSet");
			}
			T[] array = new T[this._size];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = defaultState;
			}
			if (inputs != null)
			{
				for (int j = 0; j < inputs.Length; j += 2)
				{
					T t;
					if (typeof(T).IsPrimitive)
					{
						t = (T)((object)inputs[j + 1]);
					}
					else if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(Nullable<>))
					{
						t = (T)((object)inputs[j + 1]);
					}
					else if (typeof(T).IsClass)
					{
						t = (T)((object)inputs[j + 1]);
					}
					else
					{
						t = (T)((object)Convert.ChangeType(inputs[j + 1], typeof(T)));
					}
					if (inputs[j] is ushort)
					{
						array[(int)((ushort)inputs[j])] = t;
					}
					else if (inputs[j] is int)
					{
						array[(int)inputs[j]] = t;
					}
					else
					{
						array[(int)((short)inputs[j])] = t;
					}
				}
			}
			return array;
		}

		// Token: 0x040020DC RID: 8412
		protected int _size;

		// Token: 0x040020DD RID: 8413
		private readonly Queue<int[]> _intBufferCache = new Queue<int[]>();

		// Token: 0x040020DE RID: 8414
		private readonly Queue<ushort[]> _ushortBufferCache = new Queue<ushort[]>();

		// Token: 0x040020DF RID: 8415
		private readonly Queue<bool[]> _boolBufferCache = new Queue<bool[]>();

		// Token: 0x040020E0 RID: 8416
		private readonly Queue<float[]> _floatBufferCache = new Queue<float[]>();

		// Token: 0x040020E1 RID: 8417
		private object _queueLock = new object();
	}
}
