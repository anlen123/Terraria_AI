using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Terraria.Localization
{
	// Token: 0x0200018A RID: 394
	public class LocalizedText
	{
		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06001EBB RID: 7867 RVA: 0x00510730 File Offset: 0x0050E930
		public string Value
		{
			get
			{
				VariableText variableText = this._value as VariableText;
				string result;
				if (variableText != null && variableText.TryFormat(new Func<string, object>(Lang.GetGlobalSubstitution), out result))
				{
					return result;
				}
				return this._value.ToString();
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06001EBC RID: 7868 RVA: 0x0051076F File Offset: 0x0050E96F
		public string UnformattedValue
		{
			get
			{
				return this._value.ToString();
			}
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06001EBD RID: 7869 RVA: 0x0051077C File Offset: 0x0050E97C
		// (set) Token: 0x06001EBE RID: 7870 RVA: 0x00510784 File Offset: 0x0050E984
		public string EnglishValue { get; private set; }

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06001EBF RID: 7871 RVA: 0x0051078D File Offset: 0x0050E98D
		public bool HasValue
		{
			get
			{
				return this.EnglishValue != this.Key;
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06001EC0 RID: 7872 RVA: 0x005107A0 File Offset: 0x0050E9A0
		public bool ConditionsMet
		{
			get
			{
				VariableText variableText = this._value as VariableText;
				return variableText == null || variableText.ConditionsMet(new Func<string, object>(Lang.GetGlobalSubstitution));
			}
		}

		// Token: 0x06001EC1 RID: 7873 RVA: 0x005107D0 File Offset: 0x0050E9D0
		internal LocalizedText(string key, string text)
		{
			this.Key = key;
			this.SetValue(text);
		}

		// Token: 0x06001EC2 RID: 7874 RVA: 0x005107E8 File Offset: 0x0050E9E8
		internal void SetValue(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this._value = value;
			if (LanguageManager.Instance != null && LanguageManager.Instance.ActiveCulture == GameCulture.DefaultCulture)
			{
				this.EnglishValue = value;
			}
			VariableText value2;
			if (VariableText.TryCreate(value, out value2))
			{
				this._value = value2;
			}
		}

		// Token: 0x06001EC3 RID: 7875 RVA: 0x0051083C File Offset: 0x0050EA3C
		public bool GetValueIfConditionsMet(out string formatted)
		{
			VariableText variableText = this._value as VariableText;
			if (variableText != null)
			{
				return variableText.TryFormat(new Func<string, object>(Lang.GetGlobalSubstitution), out formatted);
			}
			formatted = this.Value;
			return true;
		}

		// Token: 0x06001EC4 RID: 7876 RVA: 0x00510878 File Offset: 0x0050EA78
		public bool TryFormatWith(object obj, out string formatted)
		{
			VariableText variableText = this._value as VariableText;
			if (variableText != null)
			{
				return variableText.TryFormat(LocalizedText.GetPropertyLookupFunc(obj), out formatted);
			}
			formatted = this.Value;
			return true;
		}

		// Token: 0x06001EC5 RID: 7877 RVA: 0x005108AC File Offset: 0x0050EAAC
		public bool TryFormatWith(Func<string, object> lookup, out string formatted)
		{
			VariableText variableText = this._value as VariableText;
			if (variableText != null)
			{
				return variableText.TryFormat(lookup, out formatted);
			}
			formatted = this.Value;
			return true;
		}

		// Token: 0x06001EC6 RID: 7878 RVA: 0x005108DC File Offset: 0x0050EADC
		public string FormatWith(object obj)
		{
			string result;
			if (!this.TryFormatWith(obj, out result))
			{
				return this.Value;
			}
			return result;
		}

		// Token: 0x06001EC7 RID: 7879 RVA: 0x005108FC File Offset: 0x0050EAFC
		public string FormatWith(Func<string, object> lookup)
		{
			string result;
			if (!this.TryFormatWith(lookup, out result))
			{
				return this.Value;
			}
			return result;
		}

		// Token: 0x06001EC8 RID: 7880 RVA: 0x0051091C File Offset: 0x0050EB1C
		public bool ConditionsMetWith(object obj)
		{
			VariableText variableText = this._value as VariableText;
			return variableText == null || variableText.ConditionsMet(LocalizedText.GetPropertyLookupFunc(obj));
		}

		// Token: 0x06001EC9 RID: 7881 RVA: 0x00510948 File Offset: 0x0050EB48
		public bool ConditionsMetWith(Func<string, object> lookup)
		{
			VariableText variableText = this._value as VariableText;
			return variableText == null || variableText.ConditionsMet(lookup);
		}

		// Token: 0x06001ECA RID: 7882 RVA: 0x0051096D File Offset: 0x0050EB6D
		public NetworkText ToNetworkText()
		{
			return NetworkText.FromKey(this.Key, new object[0]);
		}

		// Token: 0x06001ECB RID: 7883 RVA: 0x00510980 File Offset: 0x0050EB80
		public NetworkText ToNetworkText(params object[] substitutions)
		{
			return NetworkText.FromKey(this.Key, substitutions);
		}

		// Token: 0x06001ECC RID: 7884 RVA: 0x0051098E File Offset: 0x0050EB8E
		public static explicit operator string(LocalizedText text)
		{
			return text.Value;
		}

		// Token: 0x06001ECD RID: 7885 RVA: 0x00510996 File Offset: 0x0050EB96
		public string Format(object arg0)
		{
			return string.Format(this.Value, arg0);
		}

		// Token: 0x06001ECE RID: 7886 RVA: 0x005109A4 File Offset: 0x0050EBA4
		public string Format(object arg0, object arg1)
		{
			return string.Format(this.Value, arg0, arg1);
		}

		// Token: 0x06001ECF RID: 7887 RVA: 0x005109B3 File Offset: 0x0050EBB3
		public string Format(object arg0, object arg1, object arg2)
		{
			return string.Format(this.Value, arg0, arg1, arg2);
		}

		// Token: 0x06001ED0 RID: 7888 RVA: 0x005109C3 File Offset: 0x0050EBC3
		public string Format(params object[] args)
		{
			return string.Format(this.Value, args);
		}

		// Token: 0x06001ED1 RID: 7889 RVA: 0x005109D1 File Offset: 0x0050EBD1
		public override string ToString()
		{
			return this.Value;
		}

		// Token: 0x06001ED2 RID: 7890 RVA: 0x005109D9 File Offset: 0x0050EBD9
		public bool EqualsCommand(string text)
		{
			text = text.ToLower();
			return text == this.Value || text == this.EnglishValue;
		}

		// Token: 0x06001ED3 RID: 7891 RVA: 0x005109FF File Offset: 0x0050EBFF
		public bool ParseCommandPrefix(string text, out string remainder)
		{
			return Utils.ParseCommandPrefix(text, this.Value, out remainder) || Utils.ParseCommandPrefix(text, this.EnglishValue, out remainder);
		}

		// Token: 0x06001ED4 RID: 7892 RVA: 0x00510A20 File Offset: 0x0050EC20
		private static Func<string, object> GetPropertyLookupFunc(object inst)
		{
			Type type = inst.GetType();
			PropertyDescriptorCollection properties;
			if (!LocalizedText._propertyLookupCache.TryGetValue(type, out properties))
			{
				LocalizedText._propertyLookupCache[type] = (properties = TypeDescriptor.GetProperties(type));
			}
			return delegate(string name)
			{
				PropertyDescriptor propertyDescriptor = properties[name];
				if (propertyDescriptor != null)
				{
					return propertyDescriptor.GetValue(inst);
				}
				return Lang.GetGlobalSubstitution(name);
			};
		}

		// Token: 0x040016D7 RID: 5847
		public static readonly LocalizedText Empty = new LocalizedText("", "");

		// Token: 0x040016D8 RID: 5848
		private static Regex _substitutionRegex = new Regex("{(\\?(?:!)?)?([a-zA-Z][\\w\\.]*)}", RegexOptions.Compiled);

		// Token: 0x040016D9 RID: 5849
		public readonly string Key;

		// Token: 0x040016DA RID: 5850
		private object _value;

		// Token: 0x040016DC RID: 5852
		private static Dictionary<Type, PropertyDescriptorCollection> _propertyLookupCache = new Dictionary<Type, PropertyDescriptorCollection>();
	}
}
