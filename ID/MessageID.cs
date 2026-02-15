using System;
using Terraria.Utilities;

namespace Terraria.ID
{
	// Token: 0x020001C0 RID: 448
	public class MessageID
	{
		// Token: 0x04003D58 RID: 15704
		public const byte NeverCalled = 0;

		// Token: 0x04003D59 RID: 15705
		public const byte Hello = 1;

		// Token: 0x04003D5A RID: 15706
		public const byte Kick = 2;

		// Token: 0x04003D5B RID: 15707
		public const byte PlayerInfo = 3;

		// Token: 0x04003D5C RID: 15708
		public const byte SyncPlayer = 4;

		// Token: 0x04003D5D RID: 15709
		public const byte SyncEquipment = 5;

		// Token: 0x04003D5E RID: 15710
		public const byte RequestWorldData = 6;

		// Token: 0x04003D5F RID: 15711
		public const byte WorldData = 7;

		// Token: 0x04003D60 RID: 15712
		public const byte SpawnTileData = 8;

		// Token: 0x04003D61 RID: 15713
		public const byte StatusTextSize = 9;

		// Token: 0x04003D62 RID: 15714
		public const byte TileSection = 10;

		// Token: 0x04003D63 RID: 15715
		[Old("Deprecated. Framing happens as needed after TileSection is sent.")]
		public const byte TileFrameSection = 11;

		// Token: 0x04003D64 RID: 15716
		public const byte PlayerSpawn = 12;

		// Token: 0x04003D65 RID: 15717
		public const byte PlayerControls = 13;

		// Token: 0x04003D66 RID: 15718
		public const byte PlayerActive = 14;

		// Token: 0x04003D67 RID: 15719
		[Old("Deprecated.")]
		public const byte Unknown15 = 15;

		// Token: 0x04003D68 RID: 15720
		public const byte PlayerLifeMana = 16;

		// Token: 0x04003D69 RID: 15721
		public const byte TileManipulation = 17;

		// Token: 0x04003D6A RID: 15722
		public const byte SetTime = 18;

		// Token: 0x04003D6B RID: 15723
		public const byte ToggleDoorState = 19;

		// Token: 0x04003D6C RID: 15724
		public const byte AreaTileChange = 20;

		// Token: 0x04003D6D RID: 15725
		public const byte SyncItem = 21;

		// Token: 0x04003D6E RID: 15726
		public const byte ItemOwner = 22;

		// Token: 0x04003D6F RID: 15727
		public const byte SyncNPC = 23;

		// Token: 0x04003D70 RID: 15728
		public const byte UnusedMeleeStrike = 24;

		// Token: 0x04003D71 RID: 15729
		[Old("Deprecated. Use NetTextModule instead.")]
		public const byte Unused25 = 25;

		// Token: 0x04003D72 RID: 15730
		[Old("Deprecated.")]
		public const byte Unused26 = 26;

		// Token: 0x04003D73 RID: 15731
		public const byte SyncProjectile = 27;

		// Token: 0x04003D74 RID: 15732
		public const byte DamageNPC = 28;

		// Token: 0x04003D75 RID: 15733
		public const byte KillProjectile = 29;

		// Token: 0x04003D76 RID: 15734
		public const byte TogglePVP = 30;

		// Token: 0x04003D77 RID: 15735
		public const byte RequestChestOpen = 31;

		// Token: 0x04003D78 RID: 15736
		public const byte SyncChestItem = 32;

		// Token: 0x04003D79 RID: 15737
		public const byte SyncPlayerChest = 33;

		// Token: 0x04003D7A RID: 15738
		public const byte ChestUpdates = 34;

		// Token: 0x04003D7B RID: 15739
		public const byte PlayerHeal = 35;

		// Token: 0x04003D7C RID: 15740
		public const byte SyncPlayerZone = 36;

		// Token: 0x04003D7D RID: 15741
		public const byte RequestPassword = 37;

		// Token: 0x04003D7E RID: 15742
		public const byte SendPassword = 38;

		// Token: 0x04003D7F RID: 15743
		public const byte ReleaseItemOwnership = 39;

		// Token: 0x04003D80 RID: 15744
		public const byte SyncTalkNPC = 40;

		// Token: 0x04003D81 RID: 15745
		public const byte ItemRotationAndAnimation = 41;

		// Token: 0x04003D82 RID: 15746
		public const byte Unknown42 = 42;

		// Token: 0x04003D83 RID: 15747
		public const byte ManaEffect = 43;

		// Token: 0x04003D84 RID: 15748
		[Old("Deprecated.")]
		public const byte Unknown44 = 44;

		// Token: 0x04003D85 RID: 15749
		public const byte TeamChange = 45;

		// Token: 0x04003D86 RID: 15750
		public const byte OpenSignRequest = 46;

		// Token: 0x04003D87 RID: 15751
		public const byte OpenSignResponse = 47;

		// Token: 0x04003D88 RID: 15752
		[Old("Deprecated. Use NetLiquidModule instead.")]
		public const byte LiquidUpdate = 48;

		// Token: 0x04003D89 RID: 15753
		public const byte InitialSpawn = 49;

		// Token: 0x04003D8A RID: 15754
		public const byte PlayerBuffs = 50;

		// Token: 0x04003D8B RID: 15755
		public const byte MiscDataSync = 51;

		// Token: 0x04003D8C RID: 15756
		public const byte LockAndUnlock = 52;

		// Token: 0x04003D8D RID: 15757
		public const byte AddNPCBuff = 53;

		// Token: 0x04003D8E RID: 15758
		public const byte NPCBuffs = 54;

		// Token: 0x04003D8F RID: 15759
		public const byte AddPlayerBuffPvP = 55;

		// Token: 0x04003D90 RID: 15760
		public const byte UniqueTownNPCInfoSyncRequest = 56;

		// Token: 0x04003D91 RID: 15761
		public const byte Unknown57 = 57;

		// Token: 0x04003D92 RID: 15762
		public const byte InstrumentSound = 58;

		// Token: 0x04003D93 RID: 15763
		public const byte HitSwitch = 59;

		// Token: 0x04003D94 RID: 15764
		public const byte Unknown60 = 60;

		// Token: 0x04003D95 RID: 15765
		public const byte SpawnBossUseLicenseStartEvent = 61;

		// Token: 0x04003D96 RID: 15766
		public const byte Unknown62 = 62;

		// Token: 0x04003D97 RID: 15767
		public const byte SyncTilePaintOrCoating = 63;

		// Token: 0x04003D98 RID: 15768
		public const byte SyncWallPaintOrCoating = 64;

		// Token: 0x04003D99 RID: 15769
		public const byte TeleportEntity = 65;

		// Token: 0x04003D9A RID: 15770
		public const byte Unknown66 = 66;

		// Token: 0x04003D9B RID: 15771
		public const byte Unknown67 = 67;

		// Token: 0x04003D9C RID: 15772
		public const byte Unknown68 = 68;

		// Token: 0x04003D9D RID: 15773
		public const byte ChestName = 69;

		// Token: 0x04003D9E RID: 15774
		public const byte BugCatching = 70;

		// Token: 0x04003D9F RID: 15775
		public const byte BugReleasing = 71;

		// Token: 0x04003DA0 RID: 15776
		public const byte TravelMerchantItems = 72;

		// Token: 0x04003DA1 RID: 15777
		public const byte RequestTeleportationByServer = 73;

		// Token: 0x04003DA2 RID: 15778
		public const byte AnglerQuest = 74;

		// Token: 0x04003DA3 RID: 15779
		public const byte AnglerQuestFinished = 75;

		// Token: 0x04003DA4 RID: 15780
		public const byte QuestsCountSync = 76;

		// Token: 0x04003DA5 RID: 15781
		public const byte TemporaryAnimation = 77;

		// Token: 0x04003DA6 RID: 15782
		public const byte InvasionProgressReport = 78;

		// Token: 0x04003DA7 RID: 15783
		public const byte PlaceObject = 79;

		// Token: 0x04003DA8 RID: 15784
		public const byte SyncPlayerChestIndex = 80;

		// Token: 0x04003DA9 RID: 15785
		public const byte CombatTextInt = 81;

		// Token: 0x04003DAA RID: 15786
		public const byte NetModules = 82;

		// Token: 0x04003DAB RID: 15787
		[Old("Deprecated.")]
		public const byte Unused83 = 83;

		// Token: 0x04003DAC RID: 15788
		public const byte PlayerStealth = 84;

		// Token: 0x04003DAD RID: 15789
		public const byte QuickStackChests = 85;

		// Token: 0x04003DAE RID: 15790
		public const byte TileEntitySharing = 86;

		// Token: 0x04003DAF RID: 15791
		public const byte TileEntityPlacement = 87;

		// Token: 0x04003DB0 RID: 15792
		public const byte ItemTweaker = 88;

		// Token: 0x04003DB1 RID: 15793
		public const byte ItemFrameTryPlacing = 89;

		// Token: 0x04003DB2 RID: 15794
		public const byte InstancedItem = 90;

		// Token: 0x04003DB3 RID: 15795
		public const byte SyncEmoteBubble = 91;

		// Token: 0x04003DB4 RID: 15796
		public const byte SyncExtraValue = 92;

		// Token: 0x04003DB5 RID: 15797
		public const byte SocialHandshake = 93;

		// Token: 0x04003DB6 RID: 15798
		public const byte DevCommands = 94;

		// Token: 0x04003DB7 RID: 15799
		public const byte MurderSomeoneElsesPortal = 95;

		// Token: 0x04003DB8 RID: 15800
		public const byte TeleportPlayerThroughPortal = 96;

		// Token: 0x04003DB9 RID: 15801
		public const byte AchievementMessageNPCKilled = 97;

		// Token: 0x04003DBA RID: 15802
		public const byte AchievementMessageEventHappened = 98;

		// Token: 0x04003DBB RID: 15803
		public const byte MinionRestTargetUpdate = 99;

		// Token: 0x04003DBC RID: 15804
		public const byte TeleportNPCThroughPortal = 100;

		// Token: 0x04003DBD RID: 15805
		public const byte UpdateTowerShieldStrengths = 101;

		// Token: 0x04003DBE RID: 15806
		public const byte NebulaLevelupRequest = 102;

		// Token: 0x04003DBF RID: 15807
		public const byte MoonlordHorror = 103;

		// Token: 0x04003DC0 RID: 15808
		public const byte ShopOverride = 104;

		// Token: 0x04003DC1 RID: 15809
		public const byte GemLockToggle = 105;

		// Token: 0x04003DC2 RID: 15810
		public const byte PoofOfSmoke = 106;

		// Token: 0x04003DC3 RID: 15811
		public const byte SmartTextMessage = 107;

		// Token: 0x04003DC4 RID: 15812
		public const byte WiredCannonShot = 108;

		// Token: 0x04003DC5 RID: 15813
		public const byte MassWireOperation = 109;

		// Token: 0x04003DC6 RID: 15814
		public const byte MassWireOperationPay = 110;

		// Token: 0x04003DC7 RID: 15815
		public const byte ToggleParty = 111;

		// Token: 0x04003DC8 RID: 15816
		public const byte SpecialFX = 112;

		// Token: 0x04003DC9 RID: 15817
		public const byte CrystalInvasionStart = 113;

		// Token: 0x04003DCA RID: 15818
		public const byte CrystalInvasionWipeAllTheThingsss = 114;

		// Token: 0x04003DCB RID: 15819
		public const byte MinionAttackTargetUpdate = 115;

		// Token: 0x04003DCC RID: 15820
		public const byte CrystalInvasionSendWaitTime = 116;

		// Token: 0x04003DCD RID: 15821
		public const byte PlayerHurtV2 = 117;

		// Token: 0x04003DCE RID: 15822
		public const byte PlayerDeathV2 = 118;

		// Token: 0x04003DCF RID: 15823
		public const byte CombatTextString = 119;

		// Token: 0x04003DD0 RID: 15824
		public const byte Emoji = 120;

		// Token: 0x04003DD1 RID: 15825
		public const byte TEDisplayDollDataSync = 121;

		// Token: 0x04003DD2 RID: 15826
		public const byte RequestTileEntityInteraction = 122;

		// Token: 0x04003DD3 RID: 15827
		public const byte WeaponsRackTryPlacing = 123;

		// Token: 0x04003DD4 RID: 15828
		public const byte TEHatRackItemSync = 124;

		// Token: 0x04003DD5 RID: 15829
		public const byte SyncTilePicking = 125;

		// Token: 0x04003DD6 RID: 15830
		public const byte SyncRevengeMarker = 126;

		// Token: 0x04003DD7 RID: 15831
		public const byte RemoveRevengeMarker = 127;

		// Token: 0x04003DD8 RID: 15832
		public const byte LandGolfBallInCup = 128;

		// Token: 0x04003DD9 RID: 15833
		public const byte FinishedConnectingToServer = 129;

		// Token: 0x04003DDA RID: 15834
		public const byte FishOutNPC = 130;

		// Token: 0x04003DDB RID: 15835
		public const byte TamperWithNPC = 131;

		// Token: 0x04003DDC RID: 15836
		public const byte PlayLegacySound = 132;

		// Token: 0x04003DDD RID: 15837
		public const byte FoodPlatterTryPlacing = 133;

		// Token: 0x04003DDE RID: 15838
		public const byte UpdatePlayerLuckFactors = 134;

		// Token: 0x04003DDF RID: 15839
		public const byte DeadPlayer = 135;

		// Token: 0x04003DE0 RID: 15840
		public const byte SyncCavernMonsterType = 136;

		// Token: 0x04003DE1 RID: 15841
		public const byte RequestNPCBuffRemoval = 137;

		// Token: 0x04003DE2 RID: 15842
		public const byte ClientSyncedInventory = 138;

		// Token: 0x04003DE3 RID: 15843
		public const byte SetCountsAsHostForGameplay = 139;

		// Token: 0x04003DE4 RID: 15844
		public const byte SetMiscEventValues = 140;

		// Token: 0x04003DE5 RID: 15845
		public const byte RequestLucyPopup = 141;

		// Token: 0x04003DE6 RID: 15846
		public const byte SyncProjectileTrackers = 142;

		// Token: 0x04003DE7 RID: 15847
		public const byte CrystalInvasionRequestedToSkipWaitTime = 143;

		// Token: 0x04003DE8 RID: 15848
		public const byte RequestQuestEffect = 144;

		// Token: 0x04003DE9 RID: 15849
		public const byte SyncItemsWithShimmer = 145;

		// Token: 0x04003DEA RID: 15850
		public const byte ShimmerActions = 146;

		// Token: 0x04003DEB RID: 15851
		public const byte SyncLoadout = 147;

		// Token: 0x04003DEC RID: 15852
		public const byte SyncItemCannotBeTakenByEnemies = 148;

		// Token: 0x04003DED RID: 15853
		public const byte DeadCellsDisplayJarTryPlacing = 149;

		// Token: 0x04003DEE RID: 15854
		public const byte SpectatePlayer = 150;

		// Token: 0x04003DEF RID: 15855
		public const byte SyncItemDespawn = 151;

		// Token: 0x04003DF0 RID: 15856
		public const byte ItemUseSound = 152;

		// Token: 0x04003DF1 RID: 15857
		public const byte NPCDebuffDamage = 153;

		// Token: 0x04003DF2 RID: 15858
		public const byte Ping = 154;

		// Token: 0x04003DF3 RID: 15859
		public const byte SyncChestSize = 155;

		// Token: 0x04003DF4 RID: 15860
		public const byte TELeashedEntityAnchorPlaceItem = 156;

		// Token: 0x04003DF5 RID: 15861
		public const byte TeamChangeFromUI = 157;

		// Token: 0x04003DF6 RID: 15862
		public const byte ExtraSpawnSectionLoaded = 158;

		// Token: 0x04003DF7 RID: 15863
		public const byte RequestSection = 159;

		// Token: 0x04003DF8 RID: 15864
		public const byte ItemPosition = 160;

		// Token: 0x04003DF9 RID: 15865
		public const byte HostToken = 161;

		// Token: 0x04003DFA RID: 15866
		public static readonly byte Count = 162;
	}
}
