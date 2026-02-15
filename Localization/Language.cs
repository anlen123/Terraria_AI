using System;
using System.Text.RegularExpressions;
using Terraria.Utilities;

namespace Terraria.Localization
{
	// Token: 0x02000188 RID: 392
	public static class Language
	{
		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06001E84 RID: 7812 RVA: 0x0050F714 File Offset: 0x0050D914
		public static GameCulture ActiveCulture
		{
			get
			{
				return LanguageManager.Instance.ActiveCulture;
			}
		}

		// Token: 0x06001E85 RID: 7813 RVA: 0x0050F720 File Offset: 0x0050D920
		public static LocalizedText GetText(string key)
		{
			return LanguageManager.Instance.GetText(key);
		}

		// Token: 0x06001E86 RID: 7814 RVA: 0x0050F72D File Offset: 0x0050D92D
		public static string GetTextValue(string key)
		{
			return LanguageManager.Instance.GetTextValue(key);
		}

		// Token: 0x06001E87 RID: 7815 RVA: 0x0050F73A File Offset: 0x0050D93A
		public static string GetTextValue(string key, object arg0)
		{
			return LanguageManager.Instance.GetTextValue(key, arg0);
		}

		// Token: 0x06001E88 RID: 7816 RVA: 0x0050F748 File Offset: 0x0050D948
		public static string GetTextValue(string key, object arg0, object arg1)
		{
			return LanguageManager.Instance.GetTextValue(key, arg0, arg1);
		}

		// Token: 0x06001E89 RID: 7817 RVA: 0x0050F757 File Offset: 0x0050D957
		public static string GetTextValue(string key, object arg0, object arg1, object arg2)
		{
			return LanguageManager.Instance.GetTextValue(key, arg0, arg1, arg2);
		}

		// Token: 0x06001E8A RID: 7818 RVA: 0x0050F767 File Offset: 0x0050D967
		public static string GetTextValue(string key, params object[] args)
		{
			return LanguageManager.Instance.GetTextValue(key, args);
		}

		// Token: 0x06001E8B RID: 7819 RVA: 0x0050F775 File Offset: 0x0050D975
		public static string GetTextValueWith(string key, object obj)
		{
			return LanguageManager.Instance.GetText(key).FormatWith(obj);
		}

		// Token: 0x06001E8C RID: 7820 RVA: 0x0050F788 File Offset: 0x0050D988
		public static bool Exists(string key)
		{
			return LanguageManager.Instance.Exists(key);
		}

		// Token: 0x06001E8D RID: 7821 RVA: 0x0050F795 File Offset: 0x0050D995
		public static int GetCategorySize(string key)
		{
			return LanguageManager.Instance.GetCategorySize(key);
		}

		// Token: 0x06001E8E RID: 7822 RVA: 0x0050F7A2 File Offset: 0x0050D9A2
		public static LocalizedText[] FindAll(Regex regex)
		{
			return LanguageManager.Instance.FindAll(regex);
		}

		// Token: 0x06001E8F RID: 7823 RVA: 0x0050F7AF File Offset: 0x0050D9AF
		public static LocalizedText[] FindAll(LanguageSearchFilter filter)
		{
			return LanguageManager.Instance.FindAll(filter);
		}

		// Token: 0x06001E90 RID: 7824 RVA: 0x0050F7BC File Offset: 0x0050D9BC
		public static LocalizedText SelectRandom(LanguageSearchFilter filter, UnifiedRandom random = null)
		{
			return LanguageManager.Instance.SelectRandom(filter, random);
		}

		// Token: 0x06001E91 RID: 7825 RVA: 0x0050F7CA File Offset: 0x0050D9CA
		public static LocalizedText RandomFromCategory(string categoryName, UnifiedRandom random = null)
		{
			return LanguageManager.Instance.RandomFromCategory(categoryName, random);
		}

		// Token: 0x06001E92 RID: 7826 RVA: 0x0050F7D8 File Offset: 0x0050D9D8
		public static bool TryGetVariation(string key, string variant, out string value)
		{
			return LanguageManager.Instance.TryGetVariation(key, variant, out value);
		}
	}
}
