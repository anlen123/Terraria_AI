using System;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace Terraria.Graphics.Shaders
{
	// Token: 0x020001DF RID: 479
	public class ArmorShaderDataSet
	{
		// Token: 0x0600200B RID: 8203 RVA: 0x00520314 File Offset: 0x0051E514
		public T BindShader<T>(int itemId, T shaderData) where T : ArmorShaderData
		{
			Dictionary<int, int> shaderLookupDictionary = this._shaderLookupDictionary;
			int num = this._shaderDataCount + 1;
			this._shaderDataCount = num;
			shaderLookupDictionary[itemId] = num;
			this._shaderData.Add(shaderData);
			return shaderData;
		}

		// Token: 0x0600200C RID: 8204 RVA: 0x00520350 File Offset: 0x0051E550
		public void Apply(int shaderId, Entity entity, DrawData? drawData = null)
		{
			if (shaderId >= 1 && shaderId <= this._shaderDataCount)
			{
				this._shaderData[shaderId - 1].Apply(entity, drawData);
				return;
			}
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();
		}

		// Token: 0x0600200D RID: 8205 RVA: 0x00520390 File Offset: 0x0051E590
		public void ApplySecondary(int shaderId, Entity entity, DrawData? drawData = null)
		{
			if (shaderId >= 1 && shaderId <= this._shaderDataCount)
			{
				this._shaderData[shaderId - 1].GetSecondaryShader(entity).Apply(entity, drawData);
				return;
			}
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();
		}

		// Token: 0x0600200E RID: 8206 RVA: 0x005203E0 File Offset: 0x0051E5E0
		public ArmorShaderData GetShaderFromItemId(int type)
		{
			if (this._shaderLookupDictionary.ContainsKey(type))
			{
				return this._shaderData[this._shaderLookupDictionary[type] - 1];
			}
			return null;
		}

		// Token: 0x0600200F RID: 8207 RVA: 0x0052040B File Offset: 0x0051E60B
		public int GetShaderIdFromItemId(int type)
		{
			if (this._shaderLookupDictionary.ContainsKey(type))
			{
				return this._shaderLookupDictionary[type];
			}
			return 0;
		}

		// Token: 0x06002010 RID: 8208 RVA: 0x00520429 File Offset: 0x0051E629
		public ArmorShaderData GetSecondaryShader(int id, Player player)
		{
			if (id != 0 && id <= this._shaderDataCount && this._shaderData[id - 1] != null)
			{
				return this._shaderData[id - 1].GetSecondaryShader(player);
			}
			return null;
		}

		// Token: 0x04004A74 RID: 19060
		protected List<ArmorShaderData> _shaderData = new List<ArmorShaderData>();

		// Token: 0x04004A75 RID: 19061
		protected Dictionary<int, int> _shaderLookupDictionary = new Dictionary<int, int>();

		// Token: 0x04004A76 RID: 19062
		protected int _shaderDataCount;
	}
}
