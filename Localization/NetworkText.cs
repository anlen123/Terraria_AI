using System;
using System.IO;

namespace Terraria.Localization
{
	// Token: 0x0200018B RID: 395
	public class NetworkText
	{
		// Token: 0x06001ED6 RID: 7894 RVA: 0x00510AAF File Offset: 0x0050ECAF
		private NetworkText(string text, NetworkText.Mode mode)
		{
			this._text = text;
			this._mode = mode;
		}

		// Token: 0x06001ED7 RID: 7895 RVA: 0x00510AC8 File Offset: 0x0050ECC8
		private static NetworkText[] ConvertSubstitutionsToNetworkText(object[] substitutions)
		{
			NetworkText[] array = new NetworkText[substitutions.Length];
			for (int i = 0; i < substitutions.Length; i++)
			{
				NetworkText networkText = substitutions[i] as NetworkText;
				if (networkText == null)
				{
					networkText = NetworkText.FromLiteral(substitutions[i].ToString());
				}
				array[i] = networkText;
			}
			return array;
		}

		// Token: 0x06001ED8 RID: 7896 RVA: 0x00510B0B File Offset: 0x0050ED0B
		public static NetworkText FromFormattable(string text, params object[] substitutions)
		{
			return new NetworkText(text, NetworkText.Mode.Formattable)
			{
				_substitutions = NetworkText.ConvertSubstitutionsToNetworkText(substitutions)
			};
		}

		// Token: 0x06001ED9 RID: 7897 RVA: 0x00510B20 File Offset: 0x0050ED20
		public static NetworkText FromLiteral(string text)
		{
			return new NetworkText(text, NetworkText.Mode.Literal);
		}

		// Token: 0x06001EDA RID: 7898 RVA: 0x00510B29 File Offset: 0x0050ED29
		public static NetworkText FromKey(string key, params object[] substitutions)
		{
			return new NetworkText(key, NetworkText.Mode.LocalizationKey)
			{
				_substitutions = NetworkText.ConvertSubstitutionsToNetworkText(substitutions)
			};
		}

		// Token: 0x06001EDB RID: 7899 RVA: 0x00510B3E File Offset: 0x0050ED3E
		public void Serialize(BinaryWriter writer)
		{
			writer.Write((byte)this._mode);
			writer.Write(this._text);
			this.SerializeSubstitutionList(writer);
		}

		// Token: 0x06001EDC RID: 7900 RVA: 0x00510B60 File Offset: 0x0050ED60
		private void SerializeSubstitutionList(BinaryWriter writer)
		{
			if (this._mode == NetworkText.Mode.Literal)
			{
				return;
			}
			writer.Write((byte)this._substitutions.Length);
			for (int i = 0; i < (this._substitutions.Length & 255); i++)
			{
				this._substitutions[i].Serialize(writer);
			}
		}

		// Token: 0x06001EDD RID: 7901 RVA: 0x00510BAC File Offset: 0x0050EDAC
		public static NetworkText Deserialize(BinaryReader reader)
		{
			NetworkText.Mode mode = (NetworkText.Mode)reader.ReadByte();
			NetworkText networkText = new NetworkText(reader.ReadString(), mode);
			networkText.DeserializeSubstitutionList(reader);
			return networkText;
		}

		// Token: 0x06001EDE RID: 7902 RVA: 0x00510BD4 File Offset: 0x0050EDD4
		public static NetworkText DeserializeLiteral(BinaryReader reader)
		{
			NetworkText.Mode mode = (NetworkText.Mode)reader.ReadByte();
			NetworkText networkText = new NetworkText(reader.ReadString(), mode);
			networkText.DeserializeSubstitutionList(reader);
			if (mode != NetworkText.Mode.Literal)
			{
				networkText.SetToEmptyLiteral();
			}
			return networkText;
		}

		// Token: 0x06001EDF RID: 7903 RVA: 0x00510C08 File Offset: 0x0050EE08
		private void DeserializeSubstitutionList(BinaryReader reader)
		{
			if (this._mode == NetworkText.Mode.Literal)
			{
				return;
			}
			this._substitutions = new NetworkText[(int)reader.ReadByte()];
			for (int i = 0; i < this._substitutions.Length; i++)
			{
				this._substitutions[i] = NetworkText.Deserialize(reader);
			}
		}

		// Token: 0x06001EE0 RID: 7904 RVA: 0x00510C50 File Offset: 0x0050EE50
		private void SetToEmptyLiteral()
		{
			this._mode = NetworkText.Mode.Literal;
			this._text = string.Empty;
			this._substitutions = null;
		}

		// Token: 0x06001EE1 RID: 7905 RVA: 0x00510C6C File Offset: 0x0050EE6C
		public override string ToString()
		{
			try
			{
				switch (this._mode)
				{
				case NetworkText.Mode.Literal:
					return this._text;
				case NetworkText.Mode.Formattable:
				{
					string text = this._text;
					object[] substitutions = this._substitutions;
					return string.Format(text, substitutions);
				}
				case NetworkText.Mode.LocalizationKey:
				{
					string text2 = this._text;
					object[] substitutions = this._substitutions;
					return Language.GetTextValue(text2, substitutions);
				}
				default:
					return this._text;
				}
			}
			catch (Exception arg)
			{
				"NetworkText.ToString() threw an exception.\n" + this.ToDebugInfoString("") + "\n" + "Exception: " + arg;
				this.SetToEmptyLiteral();
			}
			return this._text;
		}

		// Token: 0x06001EE2 RID: 7906 RVA: 0x00510D20 File Offset: 0x0050EF20
		private string ToDebugInfoString(string linePrefix = "")
		{
			string text = string.Format("{0}Mode: {1}\n{0}Text: {2}\n", linePrefix, this._mode, this._text);
			if (this._mode == NetworkText.Mode.LocalizationKey)
			{
				text += string.Format("{0}Localized Text: {1}\n", linePrefix, Language.GetTextValue(this._text));
			}
			if (this._mode != NetworkText.Mode.Literal)
			{
				for (int i = 0; i < this._substitutions.Length; i++)
				{
					text += string.Format("{0}Substitution {1}:\n", linePrefix, i);
					text += this._substitutions[i].ToDebugInfoString(linePrefix + "\t");
				}
			}
			return text;
		}

		// Token: 0x040016DD RID: 5853
		public static readonly NetworkText Empty = NetworkText.FromLiteral("");

		// Token: 0x040016DE RID: 5854
		private NetworkText[] _substitutions;

		// Token: 0x040016DF RID: 5855
		private string _text;

		// Token: 0x040016E0 RID: 5856
		private NetworkText.Mode _mode;

		// Token: 0x02000759 RID: 1881
		private enum Mode : byte
		{
			// Token: 0x040069C4 RID: 27076
			Literal,
			// Token: 0x040069C5 RID: 27077
			Formattable,
			// Token: 0x040069C6 RID: 27078
			LocalizationKey
		}
	}
}
