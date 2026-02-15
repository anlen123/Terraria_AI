using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Terraria.Localization
{
	// Token: 0x02000184 RID: 388
	internal class VariableText
	{
		// Token: 0x06001E6B RID: 7787 RVA: 0x0050F361 File Offset: 0x0050D561
		private VariableText(string original, string format, VariableText.Condition[] conditions, string[] variables)
		{
			this._original = original;
			this._format = format;
			this._conditions = conditions;
			this._variables = variables;
			this._formatArgBuffer = new object[variables.Length];
		}

		// Token: 0x06001E6C RID: 7788 RVA: 0x0050F3A0 File Offset: 0x0050D5A0
		public static bool TryCreate(string s, out VariableText text)
		{
			if (!VariableText._substitutionRegex.IsMatch(s))
			{
				text = null;
				return false;
			}
			List<string> variables = new List<string>();
			List<VariableText.Condition> conditions = new List<VariableText.Condition>();
			string format = VariableText._substitutionRegex.Replace(s, delegate(Match match)
			{
				string text2 = match.Groups[2].ToString();
				string a = match.Groups[1].ToString();
				if (a != "")
				{
					conditions.Add(new VariableText.Condition
					{
						Name = text2,
						RequiredValue = (a == "?")
					});
					return "";
				}
				int num = variables.IndexOf(text2);
				if (num < 0)
				{
					num = variables.Count;
					variables.Add(text2);
				}
				return "{" + num + "}";
			});
			text = new VariableText(s, format, conditions.ToArray(), variables.ToArray());
			return true;
		}

		// Token: 0x06001E6D RID: 7789 RVA: 0x0050F414 File Offset: 0x0050D614
		private bool CheckConditionsAndLoadArgs(Func<string, object> lookup)
		{
			foreach (VariableText.Condition condition in this._conditions)
			{
				if (((lookup(condition.Name) as bool?) ?? false) != condition.RequiredValue)
				{
					return false;
				}
			}
			for (int j = 0; j < this._variables.Length; j++)
			{
				object obj = lookup(this._variables[j]);
				if (obj == null)
				{
					return false;
				}
				this._formatArgBuffer[j] = obj;
			}
			return true;
		}

		// Token: 0x06001E6E RID: 7790 RVA: 0x0050F4AC File Offset: 0x0050D6AC
		public bool ConditionsMet(Func<string, object> lookup)
		{
			return this.CheckConditionsAndLoadArgs(lookup);
		}

		// Token: 0x06001E6F RID: 7791 RVA: 0x0050F4B8 File Offset: 0x0050D6B8
		public bool TryFormat(Func<string, object> lookup, out string formatted)
		{
			if (!this.CheckConditionsAndLoadArgs(lookup))
			{
				formatted = null;
				return false;
			}
			this._formatBuffer.AppendFormat(this._format, this._formatArgBuffer);
			formatted = this._formatBuffer.ToString();
			this._formatBuffer.Clear();
			return true;
		}

		// Token: 0x06001E70 RID: 7792 RVA: 0x0050F505 File Offset: 0x0050D705
		public override string ToString()
		{
			return this._original;
		}

		// Token: 0x040016C2 RID: 5826
		private readonly string _original;

		// Token: 0x040016C3 RID: 5827
		private readonly string _format;

		// Token: 0x040016C4 RID: 5828
		private readonly VariableText.Condition[] _conditions;

		// Token: 0x040016C5 RID: 5829
		private readonly string[] _variables;

		// Token: 0x040016C6 RID: 5830
		private readonly object[] _formatArgBuffer;

		// Token: 0x040016C7 RID: 5831
		private readonly StringBuilder _formatBuffer = new StringBuilder();

		// Token: 0x040016C8 RID: 5832
		private static readonly Regex _substitutionRegex = new Regex("{(\\?!?)?([a-zA-Z][\\w\\.]*)}", RegexOptions.Compiled);

		// Token: 0x02000752 RID: 1874
		private struct Condition
		{
			// Token: 0x040069A7 RID: 27047
			public bool RequiredValue;

			// Token: 0x040069A8 RID: 27048
			public string Name;
		}
	}
}
