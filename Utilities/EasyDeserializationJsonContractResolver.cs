using System;
using System.ComponentModel;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Terraria.Utilities
{
	// Token: 0x020000C7 RID: 199
	public class EasyDeserializationJsonContractResolver : DefaultContractResolver
	{
		// Token: 0x060017DB RID: 6107 RVA: 0x004DFDF4 File Offset: 0x004DDFF4
		protected override JsonContract CreateContract(Type objectType)
		{
			JsonContract jsonContract = base.CreateContract(objectType);
			if (jsonContract is JsonStringContract && objectType != typeof(string))
			{
				TypeConverter converter = TypeDescriptor.GetConverter(objectType);
				if (converter != null && converter.CanConvertTo(typeof(string)) && !converter.CanConvertFrom(typeof(string)))
				{
					jsonContract = base.CreateObjectContract(objectType);
				}
			}
			if (objectType.IsArray || objectType.IsValueType)
			{
				jsonContract.IsReference = new bool?(false);
			}
			return jsonContract;
		}

		// Token: 0x060017DC RID: 6108 RVA: 0x004DFE78 File Offset: 0x004DE078
		protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
		{
			JsonProperty jsonProperty = base.CreateProperty(member, memberSerialization);
			if (!jsonProperty.Writable)
			{
				jsonProperty.Ignored = true;
			}
			return jsonProperty;
		}
	}
}
