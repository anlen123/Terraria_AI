using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.IO;

namespace Terraria.GameContent.UI.BigProgressBar
{
	// Token: 0x02000388 RID: 904
	public class BigProgressBarSystem
	{
		// Token: 0x060029B7 RID: 10679 RVA: 0x0057DE9B File Offset: 0x0057C09B
		public void BindTo(Preferences preferences)
		{
			preferences.OnLoad += this.Configuration_OnLoad;
			preferences.OnSave += this.Configuration_Save;
		}

		// Token: 0x060029B8 RID: 10680 RVA: 0x0057DEC1 File Offset: 0x0057C0C1
		public void Update()
		{
			if (this._currentBar == null)
			{
				this.TryFindingNPCToTrack();
			}
			if (this._currentBar == null)
			{
				return;
			}
			if (!this._currentBar.ValidateAndCollectNecessaryInfo(ref this._info))
			{
				this._currentBar = null;
			}
		}

		// Token: 0x060029B9 RID: 10681 RVA: 0x0057DEF4 File Offset: 0x0057C0F4
		public void Draw(SpriteBatch spriteBatch)
		{
			if (this._currentBar == null)
			{
				return;
			}
			this._currentBar.Draw(ref this._info, spriteBatch);
		}

		// Token: 0x060029BA RID: 10682 RVA: 0x0057DF14 File Offset: 0x0057C114
		private void TryFindingNPCToTrack()
		{
			Rectangle value = new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight);
			value.Inflate(5000, 5000);
			float num = float.PositiveInfinity;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC npc = Main.npc[i];
				if (npc.active && npc.Hitbox.Intersects(value))
				{
					float num2 = npc.Distance(Main.LocalPlayer.Center);
					if (num > num2 && this.TryTracking(i))
					{
						num = num2;
					}
				}
			}
		}

		// Token: 0x060029BB RID: 10683 RVA: 0x0057DFB4 File Offset: 0x0057C1B4
		public bool TryTracking(int npcIndex)
		{
			if (npcIndex < 0 || npcIndex > Main.maxNPCs)
			{
				return false;
			}
			NPC npc = Main.npc[npcIndex];
			if (!npc.active)
			{
				return false;
			}
			BigProgressBarInfo info = new BigProgressBarInfo
			{
				npcIndexToAimAt = npcIndex
			};
			IBigProgressBar bigProgressBar = this._bossBar;
			IBigProgressBar bigProgressBar2;
			if (this._bossBarsByNpcNetId.TryGetValue(npc.netID, out bigProgressBar2))
			{
				bigProgressBar = bigProgressBar2;
			}
			if (!bigProgressBar.ValidateAndCollectNecessaryInfo(ref info))
			{
				return false;
			}
			this._currentBar = bigProgressBar;
			info.showText = true;
			this._info = info;
			return true;
		}

		// Token: 0x060029BC RID: 10684 RVA: 0x0057E035 File Offset: 0x0057C235
		private void Configuration_Save(Preferences obj)
		{
			obj.Put("ShowBossBarHealthText", BigProgressBarSystem.ShowText);
		}

		// Token: 0x060029BD RID: 10685 RVA: 0x0057E04C File Offset: 0x0057C24C
		private void Configuration_OnLoad(Preferences obj)
		{
			BigProgressBarSystem.ShowText = obj.Get<bool>("ShowBossBarHealthText", BigProgressBarSystem.ShowText);
		}

		// Token: 0x060029BE RID: 10686 RVA: 0x0057E063 File Offset: 0x0057C263
		public static void ToggleShowText()
		{
			BigProgressBarSystem.ShowText = !BigProgressBarSystem.ShowText;
		}

		// Token: 0x0400528D RID: 21133
		private IBigProgressBar _currentBar;

		// Token: 0x0400528E RID: 21134
		private CommonBossBigProgressBar _bossBar = new CommonBossBigProgressBar();

		// Token: 0x0400528F RID: 21135
		private BigProgressBarInfo _info;

		// Token: 0x04005290 RID: 21136
		private static TwinsBigProgressBar _twinsBar = new TwinsBigProgressBar();

		// Token: 0x04005291 RID: 21137
		private static EaterOfWorldsProgressBar _eaterOfWorldsBar = new EaterOfWorldsProgressBar();

		// Token: 0x04005292 RID: 21138
		private static BrainOfCthuluBigProgressBar _brainOfCthuluBar = new BrainOfCthuluBigProgressBar();

		// Token: 0x04005293 RID: 21139
		private static GolemHeadProgressBar _golemBar = new GolemHeadProgressBar();

		// Token: 0x04005294 RID: 21140
		private static MoonLordProgressBar _moonlordBar = new MoonLordProgressBar();

		// Token: 0x04005295 RID: 21141
		private static SolarFlarePillarBigProgressBar _solarPillarBar = new SolarFlarePillarBigProgressBar();

		// Token: 0x04005296 RID: 21142
		private static VortexPillarBigProgressBar _vortexPillarBar = new VortexPillarBigProgressBar();

		// Token: 0x04005297 RID: 21143
		private static NebulaPillarBigProgressBar _nebulaPillarBar = new NebulaPillarBigProgressBar();

		// Token: 0x04005298 RID: 21144
		private static StardustPillarBigProgressBar _stardustPillarBar = new StardustPillarBigProgressBar();

		// Token: 0x04005299 RID: 21145
		private static NeverValidProgressBar _neverValid = new NeverValidProgressBar();

		// Token: 0x0400529A RID: 21146
		private static PirateShipBigProgressBar _pirateShipBar = new PirateShipBigProgressBar();

		// Token: 0x0400529B RID: 21147
		private static MartianSaucerBigProgressBar _martianSaucerBar = new MartianSaucerBigProgressBar();

		// Token: 0x0400529C RID: 21148
		private static DeerclopsBigProgressBar _deerclopsBar = new DeerclopsBigProgressBar();

		// Token: 0x0400529D RID: 21149
		public static bool ShowText = true;

		// Token: 0x0400529E RID: 21150
		private Dictionary<int, IBigProgressBar> _bossBarsByNpcNetId = new Dictionary<int, IBigProgressBar>
		{
			{
				125,
				BigProgressBarSystem._twinsBar
			},
			{
				126,
				BigProgressBarSystem._twinsBar
			},
			{
				13,
				BigProgressBarSystem._eaterOfWorldsBar
			},
			{
				14,
				BigProgressBarSystem._eaterOfWorldsBar
			},
			{
				15,
				BigProgressBarSystem._eaterOfWorldsBar
			},
			{
				266,
				BigProgressBarSystem._brainOfCthuluBar
			},
			{
				245,
				BigProgressBarSystem._golemBar
			},
			{
				246,
				BigProgressBarSystem._golemBar
			},
			{
				249,
				BigProgressBarSystem._neverValid
			},
			{
				517,
				BigProgressBarSystem._solarPillarBar
			},
			{
				422,
				BigProgressBarSystem._vortexPillarBar
			},
			{
				507,
				BigProgressBarSystem._nebulaPillarBar
			},
			{
				493,
				BigProgressBarSystem._stardustPillarBar
			},
			{
				398,
				BigProgressBarSystem._moonlordBar
			},
			{
				396,
				BigProgressBarSystem._moonlordBar
			},
			{
				397,
				BigProgressBarSystem._moonlordBar
			},
			{
				548,
				BigProgressBarSystem._neverValid
			},
			{
				549,
				BigProgressBarSystem._neverValid
			},
			{
				491,
				BigProgressBarSystem._pirateShipBar
			},
			{
				492,
				BigProgressBarSystem._pirateShipBar
			},
			{
				440,
				BigProgressBarSystem._neverValid
			},
			{
				395,
				BigProgressBarSystem._martianSaucerBar
			},
			{
				393,
				BigProgressBarSystem._martianSaucerBar
			},
			{
				394,
				BigProgressBarSystem._martianSaucerBar
			},
			{
				68,
				BigProgressBarSystem._neverValid
			},
			{
				668,
				BigProgressBarSystem._deerclopsBar
			}
		};

		// Token: 0x0400529F RID: 21151
		private const string _preferencesKey = "ShowBossBarHealthText";
	}
}
