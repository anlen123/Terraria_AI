using System;
using Microsoft.Xna.Framework;
using Terraria.Enums;

namespace Terraria.GameContent
{
	// Token: 0x0200024D RID: 589
	public class DontStarveSeed
	{
		// Token: 0x06002303 RID: 8963 RVA: 0x0053B750 File Offset: 0x00539950
		public static void ModifyNightColor(ref Color bgColorToSet, ref Color moonColor)
		{
			if (Main.GetMoonPhase() == MoonPhase.Full)
			{
				return;
			}
			float fromValue = (float)(Main.time / 32400.0);
			Color value = bgColorToSet;
			Color black = Color.Black;
			Color value2 = bgColorToSet;
			float amount = Utils.Remap(fromValue, 0f, 0.5f, 0f, 1f, true);
			float amount2 = Utils.Remap(fromValue, 0.5f, 1f, 0f, 1f, true);
			Color color = Color.Lerp(Color.Lerp(value, black, amount), value2, amount2);
			bgColorToSet = color;
		}

		// Token: 0x06002304 RID: 8964 RVA: 0x0053B7DC File Offset: 0x005399DC
		public static void ModifyMinimumLightColorAtNight(ref byte minimalLight)
		{
			switch (Main.GetMoonPhase())
			{
			case MoonPhase.Full:
				minimalLight = 45;
				break;
			case MoonPhase.ThreeQuartersAtLeft:
			case MoonPhase.ThreeQuartersAtRight:
				minimalLight = 1;
				break;
			case MoonPhase.HalfAtLeft:
			case MoonPhase.HalfAtRight:
				minimalLight = 1;
				break;
			case MoonPhase.QuarterAtLeft:
			case MoonPhase.QuarterAtRight:
				minimalLight = 1;
				break;
			case MoonPhase.Empty:
				minimalLight = 1;
				break;
			}
			if (Main.bloodMoon)
			{
				minimalLight = Utils.Max<byte>(new byte[]
				{
					minimalLight,
					35
				});
			}
		}

		// Token: 0x06002305 RID: 8965 RVA: 0x0053B84D File Offset: 0x00539A4D
		public static void FixBiomeDarkness(ref Color bgColor, ref int R, ref int G, ref int B)
		{
			if (!Main.dontStarveWorld)
			{
				return;
			}
			R = (int)((byte)Math.Min((int)bgColor.R, R));
			G = (int)((byte)Math.Min((int)bgColor.G, G));
			B = (int)((byte)Math.Min((int)bgColor.B, B));
		}

		// Token: 0x06002306 RID: 8966 RVA: 0x0053B887 File Offset: 0x00539A87
		public static void Initialize()
		{
			Player.Hooks.OnEnterWorld += DontStarveSeed.Hook_OnEnterWorld;
		}

		// Token: 0x06002307 RID: 8967 RVA: 0x0053B89A File Offset: 0x00539A9A
		private static void Hook_OnEnterWorld(Player player)
		{
			player.UpdateStarvingState(false);
		}
	}
}
