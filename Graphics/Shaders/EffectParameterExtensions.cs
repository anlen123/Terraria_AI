using System;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics.Shaders
{
	// Token: 0x020001E7 RID: 487
	public static class EffectParameterExtensions
	{
		// Token: 0x0600206F RID: 8303 RVA: 0x00521DAB File Offset: 0x0051FFAB
		public static ShaderData.EffectParameter<T> GetParameter<T>(this Effect effect, string name)
		{
			return ShaderData.EffectParameter<T>.Get(effect.Parameters[name]);
		}
	}
}
