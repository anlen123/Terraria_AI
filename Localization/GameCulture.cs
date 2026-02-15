using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Terraria.Localization
{
	// Token: 0x02000185 RID: 389
	public class GameCulture
	{
		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06001E72 RID: 7794 RVA: 0x0050F51F File Offset: 0x0050D71F
		// (set) Token: 0x06001E73 RID: 7795 RVA: 0x0050F526 File Offset: 0x0050D726
		public static GameCulture DefaultCulture { get; set; } = GameCulture._NamedCultures[GameCulture.CultureName.English];

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06001E74 RID: 7796 RVA: 0x0050F52E File Offset: 0x0050D72E
		public bool IsActive
		{
			get
			{
				return Language.ActiveCulture == this;
			}
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06001E75 RID: 7797 RVA: 0x0050F538 File Offset: 0x0050D738
		public string Name
		{
			get
			{
				return this.CultureInfo.Name;
			}
		}

		// Token: 0x06001E76 RID: 7798 RVA: 0x0050F545 File Offset: 0x0050D745
		public static GameCulture FromCultureName(GameCulture.CultureName name)
		{
			if (!GameCulture._NamedCultures.ContainsKey(name))
			{
				return GameCulture.DefaultCulture;
			}
			return GameCulture._NamedCultures[name];
		}

		// Token: 0x06001E77 RID: 7799 RVA: 0x0050F565 File Offset: 0x0050D765
		public static GameCulture FromLegacyId(int id)
		{
			if (id < 1)
			{
				id = 1;
			}
			if (!GameCulture._legacyCultures.ContainsKey(id))
			{
				return GameCulture.DefaultCulture;
			}
			return GameCulture._legacyCultures[id];
		}

		// Token: 0x06001E78 RID: 7800 RVA: 0x0050F58C File Offset: 0x0050D78C
		public static GameCulture FromName(string name)
		{
			return GameCulture._legacyCultures.Values.SingleOrDefault((GameCulture culture) => culture.Name == name) ?? GameCulture.DefaultCulture;
		}

		// Token: 0x06001E7A RID: 7802 RVA: 0x0050F6D3 File Offset: 0x0050D8D3
		public GameCulture(string name, int legacyId)
		{
			this.CultureInfo = new CultureInfo(name);
			this.LegacyId = legacyId;
			GameCulture.RegisterLegacyCulture(this, legacyId);
		}

		// Token: 0x06001E7B RID: 7803 RVA: 0x0050F6F5 File Offset: 0x0050D8F5
		private static void RegisterLegacyCulture(GameCulture culture, int legacyId)
		{
			if (GameCulture._legacyCultures == null)
			{
				GameCulture._legacyCultures = new Dictionary<int, GameCulture>();
			}
			GameCulture._legacyCultures.Add(legacyId, culture);
		}

		// Token: 0x040016C9 RID: 5833
		private static Dictionary<GameCulture.CultureName, GameCulture> _NamedCultures = new Dictionary<GameCulture.CultureName, GameCulture>
		{
			{
				GameCulture.CultureName.English,
				new GameCulture("en-US", 1)
			},
			{
				GameCulture.CultureName.German,
				new GameCulture("de-DE", 2)
			},
			{
				GameCulture.CultureName.Italian,
				new GameCulture("it-IT", 3)
			},
			{
				GameCulture.CultureName.French,
				new GameCulture("fr-FR", 4)
			},
			{
				GameCulture.CultureName.Spanish,
				new GameCulture("es-ES", 5)
			},
			{
				GameCulture.CultureName.Russian,
				new GameCulture("ru-RU", 6)
			},
			{
				GameCulture.CultureName.Chinese,
				new GameCulture("zh-Hans", 7)
			},
			{
				GameCulture.CultureName.Portuguese,
				new GameCulture("pt-BR", 8)
			},
			{
				GameCulture.CultureName.Polish,
				new GameCulture("pl-PL", 9)
			},
			{
				GameCulture.CultureName.Japanese,
				new GameCulture("ja-JP", 10)
			},
			{
				GameCulture.CultureName.Korean,
				new GameCulture("ko-KR", 11)
			},
			{
				GameCulture.CultureName.ChineseTraditional,
				new GameCulture("zh-Hant", 12)
			}
		};

		// Token: 0x040016CB RID: 5835
		private static Dictionary<int, GameCulture> _legacyCultures;

		// Token: 0x040016CC RID: 5836
		public readonly CultureInfo CultureInfo;

		// Token: 0x040016CD RID: 5837
		public readonly int LegacyId;

		// Token: 0x02000754 RID: 1876
		public enum CultureName
		{
			// Token: 0x040069AC RID: 27052
			English = 1,
			// Token: 0x040069AD RID: 27053
			German,
			// Token: 0x040069AE RID: 27054
			Italian,
			// Token: 0x040069AF RID: 27055
			French,
			// Token: 0x040069B0 RID: 27056
			Spanish,
			// Token: 0x040069B1 RID: 27057
			Russian,
			// Token: 0x040069B2 RID: 27058
			Chinese,
			// Token: 0x040069B3 RID: 27059
			Portuguese,
			// Token: 0x040069B4 RID: 27060
			Polish,
			// Token: 0x040069B5 RID: 27061
			Japanese,
			// Token: 0x040069B6 RID: 27062
			Korean,
			// Token: 0x040069B7 RID: 27063
			ChineseTraditional,
			// Token: 0x040069B8 RID: 27064
			Unknown = 9999
		}
	}
}
