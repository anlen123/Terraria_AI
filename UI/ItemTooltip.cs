using System;
using Terraria.Localization;

namespace Terraria.UI
{
	// Token: 0x020000EC RID: 236
	public class ItemTooltip
	{
		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x060018F5 RID: 6389 RVA: 0x004E66F7 File Offset: 0x004E48F7
		public int Lines
		{
			get
			{
				this.ValidateTooltip();
				if (this._tooltipLines == null)
				{
					return 0;
				}
				return this._tooltipLines.Length;
			}
		}

		// Token: 0x060018F6 RID: 6390 RVA: 0x0000357B File Offset: 0x0000177B
		private ItemTooltip()
		{
		}

		// Token: 0x060018F7 RID: 6391 RVA: 0x004E6711 File Offset: 0x004E4911
		private ItemTooltip(string key)
		{
			this._text = Language.GetText(key);
		}

		// Token: 0x060018F8 RID: 6392 RVA: 0x004E6725 File Offset: 0x004E4925
		public static ItemTooltip FromLanguageKey(string key)
		{
			return new ItemTooltip(key);
		}

		// Token: 0x060018F9 RID: 6393 RVA: 0x004E672D File Offset: 0x004E492D
		public string GetLine(int line)
		{
			this.ValidateTooltip();
			return this._tooltipLines[line];
		}

		// Token: 0x060018FA RID: 6394 RVA: 0x004E673D File Offset: 0x004E493D
		private ItemTooltip(string[] hardcodedLines)
		{
			this._validatorKey = ItemTooltip._neverUpdateHack;
			this._tooltipLines = hardcodedLines;
			this._processedText = string.Join("\n", hardcodedLines);
		}

		// Token: 0x060018FB RID: 6395 RVA: 0x004E6768 File Offset: 0x004E4968
		public static ItemTooltip FromHardcodedText(params string[] text)
		{
			return new ItemTooltip(text);
		}

		// Token: 0x060018FC RID: 6396 RVA: 0x004E6770 File Offset: 0x004E4970
		private void ValidateTooltip()
		{
			if (this._validatorKey == ItemTooltip._neverUpdateHack)
			{
				return;
			}
			if (this._validatorKey != ItemTooltip._globalValidatorKey)
			{
				this._validatorKey = ItemTooltip._globalValidatorKey;
				if (this._text == null || !this._text.HasValue)
				{
					this._tooltipLines = null;
					this._processedText = string.Empty;
					return;
				}
				string value = this._text.Value;
				this._tooltipLines = value.Split(new char[]
				{
					'\n'
				});
				this._processedText = value;
			}
		}

		// Token: 0x060018FD RID: 6397 RVA: 0x004E67F5 File Offset: 0x004E49F5
		public static void InvalidateTooltips()
		{
			ItemTooltip._globalValidatorKey += 1UL;
			if (ItemTooltip._globalValidatorKey == 18446744073709551615UL)
			{
				ItemTooltip._globalValidatorKey = 0UL;
			}
		}

		// Token: 0x0400130E RID: 4878
		public static readonly ItemTooltip None = new ItemTooltip();

		// Token: 0x0400130F RID: 4879
		private static ulong _globalValidatorKey = 1UL;

		// Token: 0x04001310 RID: 4880
		private static readonly ulong _neverUpdateHack = ulong.MaxValue;

		// Token: 0x04001311 RID: 4881
		private string[] _tooltipLines;

		// Token: 0x04001312 RID: 4882
		private ulong _validatorKey;

		// Token: 0x04001313 RID: 4883
		private readonly LocalizedText _text;

		// Token: 0x04001314 RID: 4884
		private string _processedText;
	}
}
