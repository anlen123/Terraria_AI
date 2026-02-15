using System;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ID;

namespace Terraria.GameContent
{
	// Token: 0x0200027E RID: 638
	public static class TextureAssets
	{
		// Token: 0x04004DEE RID: 19950
		public static Asset<Texture2D>[] InfoIcon = new Asset<Texture2D>[14];

		// Token: 0x04004DEF RID: 19951
		public static Asset<Texture2D>[] WireUi = new Asset<Texture2D>[12];

		// Token: 0x04004DF0 RID: 19952
		public static Asset<Texture2D> BuilderAcc;

		// Token: 0x04004DF1 RID: 19953
		public static Asset<Texture2D> QuicksIcon;

		// Token: 0x04004DF2 RID: 19954
		public static Asset<Texture2D>[] Clothes = new Asset<Texture2D>[6];

		// Token: 0x04004DF3 RID: 19955
		public static Asset<Texture2D>[] MapIcon = new Asset<Texture2D>[9];

		// Token: 0x04004DF4 RID: 19956
		public static Asset<Texture2D>[] Underworld = new Asset<Texture2D>[14];

		// Token: 0x04004DF5 RID: 19957
		public static Asset<Texture2D> MapPing;

		// Token: 0x04004DF6 RID: 19958
		public static Asset<Texture2D> Map;

		// Token: 0x04004DF7 RID: 19959
		public static Asset<Texture2D>[] MapBGs = new Asset<Texture2D>[42];

		// Token: 0x04004DF8 RID: 19960
		public static Asset<Texture2D> Hue;

		// Token: 0x04004DF9 RID: 19961
		public static Asset<Texture2D> FlameRing;

		// Token: 0x04004DFA RID: 19962
		public static Asset<Texture2D> MapDeath;

		// Token: 0x04004DFB RID: 19963
		public static Asset<Texture2D> ColorSlider;

		// Token: 0x04004DFC RID: 19964
		public static Asset<Texture2D> ColorBar;

		// Token: 0x04004DFD RID: 19965
		public static Asset<Texture2D> ColorBlip;

		// Token: 0x04004DFE RID: 19966
		public static Asset<Texture2D> SmartDig;

		// Token: 0x04004DFF RID: 19967
		public static Asset<Texture2D> SmartCursorArrow;

		// Token: 0x04004E00 RID: 19968
		public static Asset<Texture2D> ColorHighlight;

		// Token: 0x04004E01 RID: 19969
		public static Asset<Texture2D> TileCrack;

		// Token: 0x04004E02 RID: 19970
		public static Asset<Texture2D> LockOnCursor;

		// Token: 0x04004E03 RID: 19971
		public static Asset<Texture2D> IceBarrier;

		// Token: 0x04004E04 RID: 19972
		public static Asset<Texture2D>[] ChestStack = new Asset<Texture2D>[4];

		// Token: 0x04004E05 RID: 19973
		public static Asset<Texture2D>[] NpcHead = new Asset<Texture2D>[NPCHeadID.Count];

		// Token: 0x04004E06 RID: 19974
		public static Asset<Texture2D>[] NpcHeadBoss = new Asset<Texture2D>[40];

		// Token: 0x04004E07 RID: 19975
		public static Asset<Texture2D>[] CraftToggle = new Asset<Texture2D>[6];

		// Token: 0x04004E08 RID: 19976
		public static Asset<Texture2D>[] ChestCraft = new Asset<Texture2D>[4];

		// Token: 0x04004E09 RID: 19977
		public static Asset<Texture2D>[] BannerToggle = new Asset<Texture2D>[4];

		// Token: 0x04004E0A RID: 19978
		public static Asset<Texture2D>[] InventorySort = new Asset<Texture2D>[2];

		// Token: 0x04004E0B RID: 19979
		public static Asset<Texture2D>[] TextGlyph = new Asset<Texture2D>[1];

		// Token: 0x04004E0C RID: 19980
		public static Asset<Texture2D>[] HotbarRadial = new Asset<Texture2D>[3];

		// Token: 0x04004E0D RID: 19981
		public static Asset<Texture2D> CraftUpButton;

		// Token: 0x04004E0E RID: 19982
		public static Asset<Texture2D> CraftDownButton;

		// Token: 0x04004E0F RID: 19983
		public static Asset<Texture2D> ScrollLeftButton;

		// Token: 0x04004E10 RID: 19984
		public static Asset<Texture2D> ScrollRightButton;

		// Token: 0x04004E11 RID: 19985
		public static Asset<Texture2D> Frozen;

		// Token: 0x04004E12 RID: 19986
		public static Asset<Texture2D> MagicPixel;

		// Token: 0x04004E13 RID: 19987
		public static Asset<Texture2D> SettingsPanel;

		// Token: 0x04004E14 RID: 19988
		public static Asset<Texture2D> SettingsPanel2;

		// Token: 0x04004E15 RID: 19989
		public static Asset<Texture2D>[] Dest = new Asset<Texture2D>[3];

		// Token: 0x04004E16 RID: 19990
		public static Asset<Texture2D>[] Gem = new Asset<Texture2D>[7];

		// Token: 0x04004E17 RID: 19991
		public static Asset<Texture2D>[] RudolphMount = new Asset<Texture2D>[3];

		// Token: 0x04004E18 RID: 19992
		public static Asset<Texture2D> BunnyMount;

		// Token: 0x04004E19 RID: 19993
		public static Asset<Texture2D> PigronMount;

		// Token: 0x04004E1A RID: 19994
		public static Asset<Texture2D> SlimeMount;

		// Token: 0x04004E1B RID: 19995
		public static Asset<Texture2D> MinecartMount;

		// Token: 0x04004E1C RID: 19996
		public static Asset<Texture2D> TurtleMount;

		// Token: 0x04004E1D RID: 19997
		public static Asset<Texture2D> DesertMinecartMount;

		// Token: 0x04004E1E RID: 19998
		public static Asset<Texture2D> FishMinecartMount;

		// Token: 0x04004E1F RID: 19999
		public static Asset<Texture2D>[] BeeMount = new Asset<Texture2D>[2];

		// Token: 0x04004E20 RID: 20000
		public static Asset<Texture2D>[] UfoMount = new Asset<Texture2D>[2];

		// Token: 0x04004E21 RID: 20001
		public static Asset<Texture2D>[] DrillMount = new Asset<Texture2D>[6];

		// Token: 0x04004E22 RID: 20002
		public static Asset<Texture2D>[] ScutlixMount = new Asset<Texture2D>[3];

		// Token: 0x04004E23 RID: 20003
		public static Asset<Texture2D> UnicornMount;

		// Token: 0x04004E24 RID: 20004
		public static Asset<Texture2D> BasiliskMount;

		// Token: 0x04004E25 RID: 20005
		public static Asset<Texture2D>[] MinecartMechMount = new Asset<Texture2D>[2];

		// Token: 0x04004E26 RID: 20006
		public static Asset<Texture2D>[] CuteFishronMount = new Asset<Texture2D>[2];

		// Token: 0x04004E27 RID: 20007
		public static Asset<Texture2D> MinecartWoodMount;

		// Token: 0x04004E28 RID: 20008
		public static Asset<Texture2D>[] Wings = new Asset<Texture2D>[ArmorIDs.Wing.Count];

		// Token: 0x04004E29 RID: 20009
		public static Asset<Texture2D>[] ArmorHead = new Asset<Texture2D>[ArmorIDs.Head.Count];

		// Token: 0x04004E2A RID: 20010
		public static Asset<Texture2D>[] ArmorBody = new Asset<Texture2D>[ArmorIDs.Body.Count];

		// Token: 0x04004E2B RID: 20011
		public static Asset<Texture2D>[] ArmorBodyComposite = new Asset<Texture2D>[ArmorIDs.Body.Count];

		// Token: 0x04004E2C RID: 20012
		public static Asset<Texture2D>[] FemaleBody = new Asset<Texture2D>[ArmorIDs.Body.Count];

		// Token: 0x04004E2D RID: 20013
		public static Asset<Texture2D>[] ArmorArm = new Asset<Texture2D>[ArmorIDs.Body.Count];

		// Token: 0x04004E2E RID: 20014
		public static Asset<Texture2D>[] ArmorLeg = new Asset<Texture2D>[ArmorIDs.Legs.Count];

		// Token: 0x04004E2F RID: 20015
		public static Asset<Texture2D>[] AccHandsOn = new Asset<Texture2D>[ArmorIDs.HandOn.Count];

		// Token: 0x04004E30 RID: 20016
		public static Asset<Texture2D>[] AccHandsOnComposite = new Asset<Texture2D>[ArmorIDs.HandOn.Count];

		// Token: 0x04004E31 RID: 20017
		public static Asset<Texture2D>[] AccHandsOff = new Asset<Texture2D>[ArmorIDs.HandOff.Count];

		// Token: 0x04004E32 RID: 20018
		public static Asset<Texture2D>[] AccHandsOffComposite = new Asset<Texture2D>[ArmorIDs.HandOff.Count];

		// Token: 0x04004E33 RID: 20019
		public static Asset<Texture2D>[] AccBack = new Asset<Texture2D>[ArmorIDs.Back.Count];

		// Token: 0x04004E34 RID: 20020
		public static Asset<Texture2D>[] AccFront = new Asset<Texture2D>[ArmorIDs.Front.Count];

		// Token: 0x04004E35 RID: 20021
		public static Asset<Texture2D>[] AccShoes = new Asset<Texture2D>[ArmorIDs.Shoe.Count];

		// Token: 0x04004E36 RID: 20022
		public static Asset<Texture2D>[] AccWaist = new Asset<Texture2D>[ArmorIDs.Waist.Count];

		// Token: 0x04004E37 RID: 20023
		public static Asset<Texture2D>[] AccShield = new Asset<Texture2D>[ArmorIDs.Shield.Count];

		// Token: 0x04004E38 RID: 20024
		public static Asset<Texture2D>[] AccNeck = new Asset<Texture2D>[ArmorIDs.Neck.Count];

		// Token: 0x04004E39 RID: 20025
		public static Asset<Texture2D>[] AccFace = new Asset<Texture2D>[(int)ArmorIDs.Face.Count];

		// Token: 0x04004E3A RID: 20026
		public static Asset<Texture2D>[] AccBalloon = new Asset<Texture2D>[ArmorIDs.Balloon.Count];

		// Token: 0x04004E3B RID: 20027
		public static Asset<Texture2D>[] AccBeard = new Asset<Texture2D>[(int)ArmorIDs.Beard.Count];

		// Token: 0x04004E3C RID: 20028
		public static Asset<Texture2D> Pulley;

		// Token: 0x04004E3D RID: 20029
		public static Asset<Texture2D>[] XmasTree = new Asset<Texture2D>[5];

		// Token: 0x04004E3E RID: 20030
		public static Asset<Texture2D>[] Flames = new Asset<Texture2D>[18];

		// Token: 0x04004E3F RID: 20031
		public static Asset<Texture2D> Timer;

		// Token: 0x04004E40 RID: 20032
		public static Asset<Texture2D>[] Reforge = new Asset<Texture2D>[2];

		// Token: 0x04004E41 RID: 20033
		public static Asset<Texture2D> EmoteMenuButton;

		// Token: 0x04004E42 RID: 20034
		public static Asset<Texture2D> BestiaryMenuButton;

		// Token: 0x04004E43 RID: 20035
		public static Asset<Texture2D> WallOutline;

		// Token: 0x04004E44 RID: 20036
		public static Asset<Texture2D> Actuator;

		// Token: 0x04004E45 RID: 20037
		public static Asset<Texture2D> Wire;

		// Token: 0x04004E46 RID: 20038
		public static Asset<Texture2D> Wire2;

		// Token: 0x04004E47 RID: 20039
		public static Asset<Texture2D> Wire3;

		// Token: 0x04004E48 RID: 20040
		public static Asset<Texture2D> Wire4;

		// Token: 0x04004E49 RID: 20041
		public static Asset<Texture2D> WireNew;

		// Token: 0x04004E4A RID: 20042
		public static Asset<Texture2D>[] Camera = new Asset<Texture2D>[8];

		// Token: 0x04004E4B RID: 20043
		public static Asset<Texture2D> FlyingCarpet;

		// Token: 0x04004E4C RID: 20044
		public static Asset<Texture2D> Grid;

		// Token: 0x04004E4D RID: 20045
		public static Asset<Texture2D> LightDisc;

		// Token: 0x04004E4E RID: 20046
		public static Asset<Texture2D> EyeLaser;

		// Token: 0x04004E4F RID: 20047
		public static Asset<Texture2D> BoneEyes;

		// Token: 0x04004E50 RID: 20048
		public static Asset<Texture2D> BoneLaser;

		// Token: 0x04004E51 RID: 20049
		public static Asset<Texture2D> Trash;

		// Token: 0x04004E52 RID: 20050
		public static Asset<Texture2D> FishingLine;

		// Token: 0x04004E53 RID: 20051
		public static Asset<Texture2D> Beetle;

		// Token: 0x04004E54 RID: 20052
		public static Asset<Texture2D> Probe;

		// Token: 0x04004E55 RID: 20053
		public static Asset<Texture2D> EyeLaserSmall;

		// Token: 0x04004E56 RID: 20054
		public static Asset<Texture2D> XmasLight;

		// Token: 0x04004E57 RID: 20055
		public static Asset<Texture2D>[] Golem = new Asset<Texture2D>[4];

		// Token: 0x04004E58 RID: 20056
		public static Asset<Texture2D> Confuse;

		// Token: 0x04004E59 RID: 20057
		public static Asset<Texture2D> SunOrb;

		// Token: 0x04004E5A RID: 20058
		public static Asset<Texture2D> SunAltar;

		// Token: 0x04004E5B RID: 20059
		public static Asset<Texture2D>[] Chains = new Asset<Texture2D>[(int)ChainID.Count];

		// Token: 0x04004E5C RID: 20060
		public static Asset<Texture2D> Chain;

		// Token: 0x04004E5D RID: 20061
		public static Asset<Texture2D>[] GemChain = new Asset<Texture2D>[7];

		// Token: 0x04004E5E RID: 20062
		public static Asset<Texture2D> Chain2;

		// Token: 0x04004E5F RID: 20063
		public static Asset<Texture2D> Chain3;

		// Token: 0x04004E60 RID: 20064
		public static Asset<Texture2D> Chain4;

		// Token: 0x04004E61 RID: 20065
		public static Asset<Texture2D> Chain5;

		// Token: 0x04004E62 RID: 20066
		public static Asset<Texture2D> Chain6;

		// Token: 0x04004E63 RID: 20067
		public static Asset<Texture2D> Chain7;

		// Token: 0x04004E64 RID: 20068
		public static Asset<Texture2D> Chain8;

		// Token: 0x04004E65 RID: 20069
		public static Asset<Texture2D> Chain9;

		// Token: 0x04004E66 RID: 20070
		public static Asset<Texture2D> Chain10;

		// Token: 0x04004E67 RID: 20071
		public static Asset<Texture2D> Chain11;

		// Token: 0x04004E68 RID: 20072
		public static Asset<Texture2D> Chain12;

		// Token: 0x04004E69 RID: 20073
		public static Asset<Texture2D> Chain13;

		// Token: 0x04004E6A RID: 20074
		public static Asset<Texture2D> Chain14;

		// Token: 0x04004E6B RID: 20075
		public static Asset<Texture2D> Chain15;

		// Token: 0x04004E6C RID: 20076
		public static Asset<Texture2D> Chain16;

		// Token: 0x04004E6D RID: 20077
		public static Asset<Texture2D> Chain17;

		// Token: 0x04004E6E RID: 20078
		public static Asset<Texture2D> Chain18;

		// Token: 0x04004E6F RID: 20079
		public static Asset<Texture2D> Chain19;

		// Token: 0x04004E70 RID: 20080
		public static Asset<Texture2D> Chain20;

		// Token: 0x04004E71 RID: 20081
		public static Asset<Texture2D> Chain21;

		// Token: 0x04004E72 RID: 20082
		public static Asset<Texture2D> Chain22;

		// Token: 0x04004E73 RID: 20083
		public static Asset<Texture2D> Chain23;

		// Token: 0x04004E74 RID: 20084
		public static Asset<Texture2D> Chain24;

		// Token: 0x04004E75 RID: 20085
		public static Asset<Texture2D> Chain25;

		// Token: 0x04004E76 RID: 20086
		public static Asset<Texture2D> Chain26;

		// Token: 0x04004E77 RID: 20087
		public static Asset<Texture2D> Chain27;

		// Token: 0x04004E78 RID: 20088
		public static Asset<Texture2D> Chain28;

		// Token: 0x04004E79 RID: 20089
		public static Asset<Texture2D> Chain29;

		// Token: 0x04004E7A RID: 20090
		public static Asset<Texture2D> Chain30;

		// Token: 0x04004E7B RID: 20091
		public static Asset<Texture2D> Chain31;

		// Token: 0x04004E7C RID: 20092
		public static Asset<Texture2D> Chain32;

		// Token: 0x04004E7D RID: 20093
		public static Asset<Texture2D> Chain33;

		// Token: 0x04004E7E RID: 20094
		public static Asset<Texture2D> Chain34;

		// Token: 0x04004E7F RID: 20095
		public static Asset<Texture2D> Chain35;

		// Token: 0x04004E80 RID: 20096
		public static Asset<Texture2D> Chain36;

		// Token: 0x04004E81 RID: 20097
		public static Asset<Texture2D> Chain37;

		// Token: 0x04004E82 RID: 20098
		public static Asset<Texture2D> Chain38;

		// Token: 0x04004E83 RID: 20099
		public static Asset<Texture2D> Chain39;

		// Token: 0x04004E84 RID: 20100
		public static Asset<Texture2D> Chain40;

		// Token: 0x04004E85 RID: 20101
		public static Asset<Texture2D> Chain41;

		// Token: 0x04004E86 RID: 20102
		public static Asset<Texture2D> Chain42;

		// Token: 0x04004E87 RID: 20103
		public static Asset<Texture2D> Chain43;

		// Token: 0x04004E88 RID: 20104
		public static Asset<Texture2D> Hb1;

		// Token: 0x04004E89 RID: 20105
		public static Asset<Texture2D> Hb2;

		// Token: 0x04004E8A RID: 20106
		public static Asset<Texture2D> Chaos;

		// Token: 0x04004E8B RID: 20107
		public static Asset<Texture2D> Cd;

		// Token: 0x04004E8C RID: 20108
		public static Asset<Texture2D> Wof;

		// Token: 0x04004E8D RID: 20109
		public static Asset<Texture2D> BoneArm;

		// Token: 0x04004E8E RID: 20110
		public static Asset<Texture2D> BoneArm2;

		// Token: 0x04004E8F RID: 20111
		public static Asset<Texture2D> BoneArm3;

		// Token: 0x04004E90 RID: 20112
		public static Asset<Texture2D> PumpkingArm;

		// Token: 0x04004E91 RID: 20113
		public static Asset<Texture2D> PumpkingCloak;

		// Token: 0x04004E92 RID: 20114
		public static Asset<Texture2D>[] EquipPage = new Asset<Texture2D>[11];

		// Token: 0x04004E93 RID: 20115
		public static Asset<Texture2D> HouseBanner;

		// Token: 0x04004E94 RID: 20116
		public static Asset<Texture2D> NPCHappiness;

		// Token: 0x04004E95 RID: 20117
		public static Asset<Texture2D>[] Pvp = new Asset<Texture2D>[3];

		// Token: 0x04004E96 RID: 20118
		public static Asset<Texture2D>[] NpcToggle = new Asset<Texture2D>[2];

		// Token: 0x04004E97 RID: 20119
		public static Asset<Texture2D>[] HbLock = new Asset<Texture2D>[2];

		// Token: 0x04004E98 RID: 20120
		public static Asset<Texture2D>[] blockReplaceIcon = new Asset<Texture2D>[2];

		// Token: 0x04004E99 RID: 20121
		public static Asset<Texture2D>[] Buff = new Asset<Texture2D>[BuffID.Count];

		// Token: 0x04004E9A RID: 20122
		public static Asset<Texture2D>[] Item = new Asset<Texture2D>[(int)ItemID.Count];

		// Token: 0x04004E9B RID: 20123
		public static Asset<Texture2D>[] ItemFlame = new Asset<Texture2D>[(int)ItemID.Count];

		// Token: 0x04004E9C RID: 20124
		public static Asset<Texture2D>[] Npc = new Asset<Texture2D>[(int)NPCID.Count];

		// Token: 0x04004E9D RID: 20125
		public static Asset<Texture2D>[] Projectile = new Asset<Texture2D>[(int)ProjectileID.Count];

		// Token: 0x04004E9E RID: 20126
		public static Asset<Texture2D>[] Gore = new Asset<Texture2D>[GoreID.Count];

		// Token: 0x04004E9F RID: 20127
		public static Asset<Texture2D>[] BackPack = new Asset<Texture2D>[10];

		// Token: 0x04004EA0 RID: 20128
		public static Asset<Texture2D> Rain;

		// Token: 0x04004EA1 RID: 20129
		public static Asset<Texture2D>[] GlowMask = new Asset<Texture2D>[(int)GlowMaskID.Count];

		// Token: 0x04004EA2 RID: 20130
		public static Asset<Texture2D>[] Extra = new Asset<Texture2D>[(int)ExtrasID.Count];

		// Token: 0x04004EA3 RID: 20131
		public static Asset<Texture2D>[] HighlightMask = new Asset<Texture2D>[(int)TileID.Count];

		// Token: 0x04004EA4 RID: 20132
		public static Asset<Texture2D>[] Coin = new Asset<Texture2D>[4];

		// Token: 0x04004EA5 RID: 20133
		public static Asset<Texture2D>[] Cursors = new Asset<Texture2D>[18];

		// Token: 0x04004EA6 RID: 20134
		public static Asset<Texture2D> CursorRadial;

		// Token: 0x04004EA7 RID: 20135
		public static Asset<Texture2D> Dust;

		// Token: 0x04004EA8 RID: 20136
		public static Asset<Texture2D> Sun;

		// Token: 0x04004EA9 RID: 20137
		public static Asset<Texture2D> Sun2;

		// Token: 0x04004EAA RID: 20138
		public static Asset<Texture2D> Sun3;

		// Token: 0x04004EAB RID: 20139
		public static Asset<Texture2D>[] Moon = new Asset<Texture2D>[9];

		// Token: 0x04004EAC RID: 20140
		public static Asset<Texture2D> SmileyMoon;

		// Token: 0x04004EAD RID: 20141
		public static Asset<Texture2D> PumpkinMoon;

		// Token: 0x04004EAE RID: 20142
		public static Asset<Texture2D> SnowMoon;

		// Token: 0x04004EAF RID: 20143
		public static Asset<Texture2D> OneDropLogo;

		// Token: 0x04004EB0 RID: 20144
		public static Asset<Texture2D>[] Tile = new Asset<Texture2D>[(int)TileID.Count];

		// Token: 0x04004EB1 RID: 20145
		public static Asset<Texture2D> BlackTile;

		// Token: 0x04004EB2 RID: 20146
		public static Asset<Texture2D>[] Wall = new Asset<Texture2D>[(int)WallID.Count];

		// Token: 0x04004EB3 RID: 20147
		public static Asset<Texture2D>[] Background = new Asset<Texture2D>[Main.maxBackgrounds];

		// Token: 0x04004EB4 RID: 20148
		public static Asset<Texture2D>[] Cloud = new Asset<Texture2D>[CloudID.Count];

		// Token: 0x04004EB5 RID: 20149
		public static Asset<Texture2D>[] Star = new Asset<Texture2D>[4];

		// Token: 0x04004EB6 RID: 20150
		public static Asset<Texture2D>[] Liquid = new Asset<Texture2D>[15];

		// Token: 0x04004EB7 RID: 20151
		public static Asset<Texture2D>[] LiquidSlope = new Asset<Texture2D>[15];

		// Token: 0x04004EB8 RID: 20152
		public static Asset<Texture2D> Heart;

		// Token: 0x04004EB9 RID: 20153
		public static Asset<Texture2D> Heart2;

		// Token: 0x04004EBA RID: 20154
		public static Asset<Texture2D> Mana;

		// Token: 0x04004EBB RID: 20155
		public static Asset<Texture2D> Bubble;

		// Token: 0x04004EBC RID: 20156
		public static Asset<Texture2D> Flame;

		// Token: 0x04004EBD RID: 20157
		public static Asset<Texture2D>[] CageTop = new Asset<Texture2D>[5];

		// Token: 0x04004EBE RID: 20158
		public static Asset<Texture2D>[] TreeTop = new Asset<Texture2D>[32];

		// Token: 0x04004EBF RID: 20159
		public static Asset<Texture2D>[] TreeBranch = new Asset<Texture2D>[32];

		// Token: 0x04004EC0 RID: 20160
		public static Asset<Texture2D>[] Wood = new Asset<Texture2D>[7];

		// Token: 0x04004EC1 RID: 20161
		public static Asset<Texture2D> ShroomCap;

		// Token: 0x04004EC2 RID: 20162
		public static Asset<Texture2D> InventoryBack;

		// Token: 0x04004EC3 RID: 20163
		public static Asset<Texture2D> InventoryBack2;

		// Token: 0x04004EC4 RID: 20164
		public static Asset<Texture2D> InventoryBack3;

		// Token: 0x04004EC5 RID: 20165
		public static Asset<Texture2D> InventoryBack4;

		// Token: 0x04004EC6 RID: 20166
		public static Asset<Texture2D> InventoryBack5;

		// Token: 0x04004EC7 RID: 20167
		public static Asset<Texture2D> InventoryBack6;

		// Token: 0x04004EC8 RID: 20168
		public static Asset<Texture2D> InventoryBack7;

		// Token: 0x04004EC9 RID: 20169
		public static Asset<Texture2D> InventoryBack8;

		// Token: 0x04004ECA RID: 20170
		public static Asset<Texture2D> InventoryBack9;

		// Token: 0x04004ECB RID: 20171
		public static Asset<Texture2D> InventoryBack10;

		// Token: 0x04004ECC RID: 20172
		public static Asset<Texture2D> InventoryBack11;

		// Token: 0x04004ECD RID: 20173
		public static Asset<Texture2D> InventoryBack12;

		// Token: 0x04004ECE RID: 20174
		public static Asset<Texture2D> InventoryBack13;

		// Token: 0x04004ECF RID: 20175
		public static Asset<Texture2D> InventoryBack14;

		// Token: 0x04004ED0 RID: 20176
		public static Asset<Texture2D> InventoryBack15;

		// Token: 0x04004ED1 RID: 20177
		public static Asset<Texture2D> InventoryBack16;

		// Token: 0x04004ED2 RID: 20178
		public static Asset<Texture2D> InventoryBack17;

		// Token: 0x04004ED3 RID: 20179
		public static Asset<Texture2D> InventoryBack18;

		// Token: 0x04004ED4 RID: 20180
		public static Asset<Texture2D> InventoryBack19;

		// Token: 0x04004ED5 RID: 20181
		public static Asset<Texture2D> InventoryBack20;

		// Token: 0x04004ED6 RID: 20182
		public static Asset<Texture2D> InventoryBack21;

		// Token: 0x04004ED7 RID: 20183
		public static Asset<Texture2D> InventoryBack22;

		// Token: 0x04004ED8 RID: 20184
		public static Asset<Texture2D> InventoryBack23;

		// Token: 0x04004ED9 RID: 20185
		public static Asset<Texture2D> InventoryBack24;

		// Token: 0x04004EDA RID: 20186
		public static Asset<Texture2D> HairStyleBack;

		// Token: 0x04004EDB RID: 20187
		public static Asset<Texture2D> ClothesStyleBack;

		// Token: 0x04004EDC RID: 20188
		public static Asset<Texture2D> InventoryTickOn;

		// Token: 0x04004EDD RID: 20189
		public static Asset<Texture2D> InventoryTickOff;

		// Token: 0x04004EDE RID: 20190
		public static Asset<Texture2D> SplashTexture16x9;

		// Token: 0x04004EDF RID: 20191
		public static Asset<Texture2D> SplashTexture4x3;

		// Token: 0x04004EE0 RID: 20192
		public static Asset<Texture2D> SplashTextureLegoBack;

		// Token: 0x04004EE1 RID: 20193
		public static Asset<Texture2D> SplashTextureLegoResonanace;

		// Token: 0x04004EE2 RID: 20194
		public static Asset<Texture2D> SplashTextureLegoTree;

		// Token: 0x04004EE3 RID: 20195
		public static Asset<Texture2D> SplashTextureLegoFront;

		// Token: 0x04004EE4 RID: 20196
		public static Asset<Texture2D> Logo;

		// Token: 0x04004EE5 RID: 20197
		public static Asset<Texture2D> Logo2;

		// Token: 0x04004EE6 RID: 20198
		public static Asset<Texture2D> Logo3;

		// Token: 0x04004EE7 RID: 20199
		public static Asset<Texture2D> Logo4;

		// Token: 0x04004EE8 RID: 20200
		public static Asset<Texture2D> Logo5;

		// Token: 0x04004EE9 RID: 20201
		public static Asset<Texture2D> Logo6;

		// Token: 0x04004EEA RID: 20202
		public static Asset<Texture2D> TextBack;

		// Token: 0x04004EEB RID: 20203
		public static Asset<Texture2D> Chat;

		// Token: 0x04004EEC RID: 20204
		public static Asset<Texture2D> Chat2;

		// Token: 0x04004EED RID: 20205
		public static Asset<Texture2D> ChatBack;

		// Token: 0x04004EEE RID: 20206
		public static Asset<Texture2D> Team;

		// Token: 0x04004EEF RID: 20207
		public static Asset<Texture2D> Re;

		// Token: 0x04004EF0 RID: 20208
		public static Asset<Texture2D> Ra;

		// Token: 0x04004EF1 RID: 20209
		public static Asset<Texture2D> Splash;

		// Token: 0x04004EF2 RID: 20210
		public static Asset<Texture2D> Fade;

		// Token: 0x04004EF3 RID: 20211
		public static Asset<Texture2D> Ninja;

		// Token: 0x04004EF4 RID: 20212
		public static Asset<Texture2D> AntLion;

		// Token: 0x04004EF5 RID: 20213
		public static Asset<Texture2D> SpikeBase;

		// Token: 0x04004EF6 RID: 20214
		public static Asset<Texture2D> Ghost;

		// Token: 0x04004EF7 RID: 20215
		public static Asset<Texture2D> EvilCactus;

		// Token: 0x04004EF8 RID: 20216
		public static Asset<Texture2D> GoodCactus;

		// Token: 0x04004EF9 RID: 20217
		public static Asset<Texture2D> CrimsonCactus;

		// Token: 0x04004EFA RID: 20218
		public static Asset<Texture2D> WraithEye;

		// Token: 0x04004EFB RID: 20219
		public static Asset<Texture2D> Firefly;

		// Token: 0x04004EFC RID: 20220
		public static Asset<Texture2D> FireflyJar;

		// Token: 0x04004EFD RID: 20221
		public static Asset<Texture2D> Lightningbug;

		// Token: 0x04004EFE RID: 20222
		public static Asset<Texture2D> LightningbugJar;

		// Token: 0x04004EFF RID: 20223
		public static Asset<Texture2D>[] JellyfishBowl = new Asset<Texture2D>[3];

		// Token: 0x04004F00 RID: 20224
		public static Asset<Texture2D> GlowSnail;

		// Token: 0x04004F01 RID: 20225
		public static Asset<Texture2D> IceQueen;

		// Token: 0x04004F02 RID: 20226
		public static Asset<Texture2D> SantaTank;

		// Token: 0x04004F03 RID: 20227
		public static Asset<Texture2D> ReaperEye;

		// Token: 0x04004F04 RID: 20228
		public static Asset<Texture2D> JackHat;

		// Token: 0x04004F05 RID: 20229
		public static Asset<Texture2D> TreeFace;

		// Token: 0x04004F06 RID: 20230
		public static Asset<Texture2D> PumpkingFace;

		// Token: 0x04004F07 RID: 20231
		public static Asset<Texture2D> DukeFishron;

		// Token: 0x04004F08 RID: 20232
		public static Asset<Texture2D>[,] Players;

		// Token: 0x04004F09 RID: 20233
		public static Asset<Texture2D>[] PlayerHair = new Asset<Texture2D>[228];

		// Token: 0x04004F0A RID: 20234
		public static Asset<Texture2D>[] PlayerHairAlt = new Asset<Texture2D>[228];

		// Token: 0x04004F0B RID: 20235
		public static Asset<Texture2D> LoadingSunflower;

		// Token: 0x04004F0C RID: 20236
		public static Asset<Texture2D> GolfSwingBarPanel;

		// Token: 0x04004F0D RID: 20237
		public static Asset<Texture2D> GolfSwingBarFill;

		// Token: 0x04004F0E RID: 20238
		public static Asset<Texture2D> SpawnPoint;

		// Token: 0x04004F0F RID: 20239
		public static Asset<Texture2D> SpawnBed;

		// Token: 0x04004F10 RID: 20240
		public static Asset<Texture2D> GolfBallArrow;

		// Token: 0x04004F11 RID: 20241
		public static Asset<Texture2D> GolfBallArrowShadow;

		// Token: 0x04004F12 RID: 20242
		public static Asset<Texture2D> GolfBallOutline;

		// Token: 0x04004F13 RID: 20243
		public static Asset<Texture2D> TexturePackButtons;

		// Token: 0x04004F14 RID: 20244
		public static Asset<Texture2D> NpcPortraitBackground;

		// Token: 0x02000804 RID: 2052
		public static class RenderTargets
		{
			// Token: 0x04007181 RID: 29057
			public static PlayerRainbowWingsTextureContent PlayerRainbowWings;

			// Token: 0x04007182 RID: 29058
			public static PlayerTitaniumStormBuffTextureContent PlayerTitaniumStormBuff;

			// Token: 0x04007183 RID: 29059
			public static PlayerQueenSlimeMountTextureContent QueenSlimeMount;
		}
	}
}
