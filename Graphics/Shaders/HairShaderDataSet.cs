using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Terraria.Graphics.Shaders
{
	// Token: 0x020001E0 RID: 480
	public class HairShaderDataSet
	{
		// Token: 0x06002012 RID: 8210 RVA: 0x0052047C File Offset: 0x0051E67C
		public T BindShader<T>(int itemId, T shaderData) where T : HairShaderData
		{
			if (this._shaderDataCount == 255)
			{
				throw new Exception("Too many shaders bound.");
			}
			Dictionary<int, short> shaderLookupDictionary = this._shaderLookupDictionary;
			byte b = this._shaderDataCount + 1;
			this._shaderDataCount = b;
			shaderLookupDictionary[itemId] = (short)b;
			this._shaderData.Add(shaderData);
			return shaderData;
		}

		// Token: 0x06002013 RID: 8211 RVA: 0x005204D1 File Offset: 0x0051E6D1
		public void Apply(short shaderId, Player player, DrawData? drawData = null)
		{
			if (shaderId != 0 && shaderId <= (short)this._shaderDataCount)
			{
				this._shaderData[(int)(shaderId - 1)].Apply(player, drawData);
				return;
			}
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();
		}

		// Token: 0x06002014 RID: 8212 RVA: 0x0052050F File Offset: 0x0051E70F
		public Color GetColor(short shaderId, Player player, Color lightColor)
		{
			if (shaderId != 0 && shaderId <= (short)this._shaderDataCount)
			{
				return this._shaderData[(int)(shaderId - 1)].GetColor(player, lightColor);
			}
			return new Color(lightColor.ToVector4() * player.hairColor.ToVector4());
		}

		// Token: 0x06002015 RID: 8213 RVA: 0x0052054F File Offset: 0x0051E74F
		public HairShaderData GetShaderFromItemId(int type)
		{
			if (this._shaderLookupDictionary.ContainsKey(type))
			{
				return this._shaderData[(int)(this._shaderLookupDictionary[type] - 1)];
			}
			return null;
		}

		// Token: 0x06002016 RID: 8214 RVA: 0x0052057A File Offset: 0x0051E77A
		public short GetShaderIdFromItemId(int type)
		{
			if (this._shaderLookupDictionary.ContainsKey(type))
			{
				return this._shaderLookupDictionary[type];
			}
			return -1;
		}

		// Token: 0x04004A77 RID: 19063
		protected List<HairShaderData> _shaderData = new List<HairShaderData>();

		// Token: 0x04004A78 RID: 19064
		protected Dictionary<int, short> _shaderLookupDictionary = new Dictionary<int, short>();

		// Token: 0x04004A79 RID: 19065
		protected byte _shaderDataCount;
	}
}
