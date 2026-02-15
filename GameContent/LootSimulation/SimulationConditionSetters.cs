using System;
using Terraria.GameContent.LootSimulation.LootSimulatorConditionSetterTypes;

namespace Terraria.GameContent.LootSimulation
{
	// Token: 0x020002EC RID: 748
	public class SimulationConditionSetters
	{
		// Token: 0x0400504F RID: 20559
		public static FastConditionSetter HardMode = new FastConditionSetter(delegate(SimulatorInfo info)
		{
			Main.hardMode = true;
		}, delegate(SimulatorInfo info)
		{
			Main.hardMode = false;
		});

		// Token: 0x04005050 RID: 20560
		public static FastConditionSetter ExpertMode = new FastConditionSetter(delegate(SimulatorInfo info)
		{
			Main.GameMode = 1;
			info.runningExpertMode = true;
		}, delegate(SimulatorInfo info)
		{
			Main.GameMode = 0;
			info.runningExpertMode = false;
		});

		// Token: 0x04005051 RID: 20561
		public static FastConditionSetter Eclipse = new FastConditionSetter(delegate(SimulatorInfo info)
		{
			Main.eclipse = true;
		}, delegate(SimulatorInfo info)
		{
			Main.eclipse = false;
		});

		// Token: 0x04005052 RID: 20562
		public static FastConditionSetter BloodMoon = new FastConditionSetter(delegate(SimulatorInfo info)
		{
			Main.bloodMoon = true;
		}, delegate(SimulatorInfo info)
		{
			Main.bloodMoon = false;
		});

		// Token: 0x04005053 RID: 20563
		public static FastConditionSetter SlainMechBosses = new FastConditionSetter(delegate(SimulatorInfo info)
		{
			NPC.downedMechBoss1 = (NPC.downedMechBoss2 = (NPC.downedMechBoss3 = (NPC.downedMechBossAny = true)));
		}, delegate(SimulatorInfo info)
		{
			NPC.downedMechBoss1 = (NPC.downedMechBoss2 = (NPC.downedMechBoss3 = (NPC.downedMechBossAny = false)));
		});

		// Token: 0x04005054 RID: 20564
		public static FastConditionSetter SlainPlantera = new FastConditionSetter(delegate(SimulatorInfo info)
		{
			NPC.downedPlantBoss = true;
		}, delegate(SimulatorInfo info)
		{
			NPC.downedPlantBoss = false;
		});

		// Token: 0x04005055 RID: 20565
		public static StackedConditionSetter ExpertAndHardMode = new StackedConditionSetter(new ISimulationConditionSetter[]
		{
			SimulationConditionSetters.ExpertMode,
			SimulationConditionSetters.HardMode
		});

		// Token: 0x04005056 RID: 20566
		public static FastConditionSetter WindyWeather = new FastConditionSetter(delegate(SimulatorInfo info)
		{
			Main._shouldUseWindyDayMusic = true;
		}, delegate(SimulatorInfo info)
		{
			Main._shouldUseWindyDayMusic = false;
		});

		// Token: 0x04005057 RID: 20567
		public static FastConditionSetter MidDay = new FastConditionSetter(delegate(SimulatorInfo info)
		{
			Main.dayTime = true;
			Main.time = 27000.0;
		}, delegate(SimulatorInfo info)
		{
			info.ReturnToOriginalDaytime();
		});

		// Token: 0x04005058 RID: 20568
		public static FastConditionSetter MidNight = new FastConditionSetter(delegate(SimulatorInfo info)
		{
			Main.dayTime = false;
			Main.time = 16200.0;
		}, delegate(SimulatorInfo info)
		{
			info.ReturnToOriginalDaytime();
		});

		// Token: 0x04005059 RID: 20569
		public static FastConditionSetter SlimeRain = new FastConditionSetter(delegate(SimulatorInfo info)
		{
			Main.slimeRain = true;
		}, delegate(SimulatorInfo info)
		{
			Main.slimeRain = false;
		});

		// Token: 0x0400505A RID: 20570
		public static StackedConditionSetter WindyExpertHardmodeEndgameEclipseMorning = new StackedConditionSetter(new ISimulationConditionSetter[]
		{
			SimulationConditionSetters.WindyWeather,
			SimulationConditionSetters.ExpertMode,
			SimulationConditionSetters.HardMode,
			SimulationConditionSetters.SlainMechBosses,
			SimulationConditionSetters.SlainPlantera,
			SimulationConditionSetters.Eclipse,
			SimulationConditionSetters.MidDay
		});

		// Token: 0x0400505B RID: 20571
		public static StackedConditionSetter WindyExpertHardmodeEndgameBloodMoonNight = new StackedConditionSetter(new ISimulationConditionSetter[]
		{
			SimulationConditionSetters.WindyWeather,
			SimulationConditionSetters.ExpertMode,
			SimulationConditionSetters.HardMode,
			SimulationConditionSetters.SlainMechBosses,
			SimulationConditionSetters.SlainPlantera,
			SimulationConditionSetters.BloodMoon,
			SimulationConditionSetters.MidNight
		});

		// Token: 0x0400505C RID: 20572
		public static SlimeStaffConditionSetter SlimeStaffTest = new SlimeStaffConditionSetter(100);

		// Token: 0x0400505D RID: 20573
		public static LuckyCoinConditionSetter LuckyCoinTest = new LuckyCoinConditionSetter(100);
	}
}
