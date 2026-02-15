using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Reflection;
using Terraria.DataStructures;

namespace Terraria.ID
{
	// Token: 0x020001C5 RID: 453
	public class NPCID
	{
		// Token: 0x06001F4C RID: 8012 RVA: 0x00518AD8 File Offset: 0x00516CD8
		public static int FromLegacyName(string name)
		{
			int result;
			if (NPCID.LegacyNameToIdMap.TryGetValue(name, out result))
			{
				return result;
			}
			return 0;
		}

		// Token: 0x06001F4D RID: 8013 RVA: 0x00518AF7 File Offset: 0x00516CF7
		public static int FromNetId(int id)
		{
			if (id < 0)
			{
				return NPCID.NetIdMap[-id - 1];
			}
			return id;
		}

		// Token: 0x040043D7 RID: 17367
		private static readonly int[] NetIdMap = new int[]
		{
			81,
			81,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			6,
			6,
			31,
			31,
			77,
			42,
			42,
			176,
			176,
			176,
			176,
			173,
			173,
			183,
			183,
			3,
			3,
			132,
			132,
			186,
			186,
			187,
			187,
			188,
			188,
			189,
			189,
			190,
			191,
			192,
			193,
			194,
			2,
			200,
			200,
			21,
			21,
			201,
			201,
			202,
			202,
			203,
			203,
			223,
			223,
			231,
			231,
			232,
			232,
			233,
			233,
			234,
			234,
			235,
			235
		};

		// Token: 0x040043D8 RID: 17368
		private static readonly Dictionary<string, int> LegacyNameToIdMap = new Dictionary<string, int>
		{
			{
				"Slimeling",
				-1
			},
			{
				"Slimer2",
				-2
			},
			{
				"Green Slime",
				-3
			},
			{
				"Pinky",
				-4
			},
			{
				"Baby Slime",
				-5
			},
			{
				"Black Slime",
				-6
			},
			{
				"Purple Slime",
				-7
			},
			{
				"Red Slime",
				-8
			},
			{
				"Yellow Slime",
				-9
			},
			{
				"Jungle Slime",
				-10
			},
			{
				"Little Eater",
				-11
			},
			{
				"Big Eater",
				-12
			},
			{
				"Short Bones",
				-13
			},
			{
				"Big Boned",
				-14
			},
			{
				"Heavy Skeleton",
				-15
			},
			{
				"Little Stinger",
				-16
			},
			{
				"Big Stinger",
				-17
			},
			{
				"Tiny Moss Hornet",
				-18
			},
			{
				"Little Moss Hornet",
				-19
			},
			{
				"Big Moss Hornet",
				-20
			},
			{
				"Giant Moss Hornet",
				-21
			},
			{
				"Little Crimera",
				-22
			},
			{
				"Big Crimera",
				-23
			},
			{
				"Little Crimslime",
				-24
			},
			{
				"Big Crimslime",
				-25
			},
			{
				"Small Zombie",
				-26
			},
			{
				"Big Zombie",
				-27
			},
			{
				"Small Bald Zombie",
				-28
			},
			{
				"Big Bald Zombie",
				-29
			},
			{
				"Small Pincushion Zombie",
				-30
			},
			{
				"Big Pincushion Zombie",
				-31
			},
			{
				"Small Slimed Zombie",
				-32
			},
			{
				"Big Slimed Zombie",
				-33
			},
			{
				"Small Swamp Zombie",
				-34
			},
			{
				"Big Swamp Zombie",
				-35
			},
			{
				"Small Twiggy Zombie",
				-36
			},
			{
				"Big Twiggy Zombie",
				-37
			},
			{
				"Cataract Eye 2",
				-38
			},
			{
				"Sleepy Eye 2",
				-39
			},
			{
				"Dialated Eye 2",
				-40
			},
			{
				"Green Eye 2",
				-41
			},
			{
				"Purple Eye 2",
				-42
			},
			{
				"Demon Eye 2",
				-43
			},
			{
				"Small Female Zombie",
				-44
			},
			{
				"Big Female Zombie",
				-45
			},
			{
				"Small Skeleton",
				-46
			},
			{
				"Big Skeleton",
				-47
			},
			{
				"Small Headache Skeleton",
				-48
			},
			{
				"Big Headache Skeleton",
				-49
			},
			{
				"Small Misassembled Skeleton",
				-50
			},
			{
				"Big Misassembled Skeleton",
				-51
			},
			{
				"Small Pantless Skeleton",
				-52
			},
			{
				"Big Pantless Skeleton",
				-53
			},
			{
				"Small Rain Zombie",
				-54
			},
			{
				"Big Rain Zombie",
				-55
			},
			{
				"Little Hornet Fatty",
				-56
			},
			{
				"Big Hornet Fatty",
				-57
			},
			{
				"Little Hornet Honey",
				-58
			},
			{
				"Big Hornet Honey",
				-59
			},
			{
				"Little Hornet Leafy",
				-60
			},
			{
				"Big Hornet Leafy",
				-61
			},
			{
				"Little Hornet Spikey",
				-62
			},
			{
				"Big Hornet Spikey",
				-63
			},
			{
				"Little Hornet Stingy",
				-64
			},
			{
				"Big Hornet Stingy",
				-65
			},
			{
				"Blue Slime",
				1
			},
			{
				"Demon Eye",
				2
			},
			{
				"Zombie",
				3
			},
			{
				"Eye of Cthulhu",
				4
			},
			{
				"Servant of Cthulhu",
				5
			},
			{
				"Eater of Souls",
				6
			},
			{
				"Devourer",
				7
			},
			{
				"Giant Worm",
				10
			},
			{
				"Eater of Worlds",
				13
			},
			{
				"Mother Slime",
				16
			},
			{
				"Merchant",
				17
			},
			{
				"Nurse",
				18
			},
			{
				"Arms Dealer",
				19
			},
			{
				"Dryad",
				20
			},
			{
				"Skeleton",
				21
			},
			{
				"Guide",
				22
			},
			{
				"Meteor Head",
				23
			},
			{
				"Fire Imp",
				24
			},
			{
				"Burning Sphere",
				25
			},
			{
				"Goblin Peon",
				26
			},
			{
				"Goblin Thief",
				27
			},
			{
				"Goblin Warrior",
				28
			},
			{
				"Goblin Sorcerer",
				29
			},
			{
				"Chaos Ball",
				30
			},
			{
				"Angry Bones",
				31
			},
			{
				"Dark Caster",
				32
			},
			{
				"Water Sphere",
				33
			},
			{
				"Cursed Skull",
				34
			},
			{
				"Skeletron",
				35
			},
			{
				"Old Man",
				37
			},
			{
				"Demolitionist",
				38
			},
			{
				"Bone Serpent",
				39
			},
			{
				"Hornet",
				42
			},
			{
				"Man Eater",
				43
			},
			{
				"Undead Miner",
				44
			},
			{
				"Tim",
				45
			},
			{
				"Bunny",
				46
			},
			{
				"Corrupt Bunny",
				47
			},
			{
				"Harpy",
				48
			},
			{
				"Cave Bat",
				49
			},
			{
				"King Slime",
				50
			},
			{
				"Jungle Bat",
				51
			},
			{
				"Doctor Bones",
				52
			},
			{
				"The Groom",
				53
			},
			{
				"Clothier",
				54
			},
			{
				"Goldfish",
				55
			},
			{
				"Snatcher",
				56
			},
			{
				"Corrupt Goldfish",
				57
			},
			{
				"Piranha",
				58
			},
			{
				"Lava Slime",
				59
			},
			{
				"Hellbat",
				60
			},
			{
				"Vulture",
				61
			},
			{
				"Demon",
				62
			},
			{
				"Blue Jellyfish",
				63
			},
			{
				"Pink Jellyfish",
				64
			},
			{
				"Shark",
				65
			},
			{
				"Voodoo Demon",
				66
			},
			{
				"Crab",
				67
			},
			{
				"Dungeon Guardian",
				68
			},
			{
				"Antlion",
				69
			},
			{
				"Spike Ball",
				70
			},
			{
				"Dungeon Slime",
				71
			},
			{
				"Blazing Wheel",
				72
			},
			{
				"Goblin Scout",
				73
			},
			{
				"Bird",
				74
			},
			{
				"Pixie",
				75
			},
			{
				"Armored Skeleton",
				77
			},
			{
				"Mummy",
				78
			},
			{
				"Dark Mummy",
				79
			},
			{
				"Light Mummy",
				80
			},
			{
				"Corrupt Slime",
				81
			},
			{
				"Wraith",
				82
			},
			{
				"Cursed Hammer",
				83
			},
			{
				"Enchanted Sword",
				84
			},
			{
				"Mimic",
				85
			},
			{
				"Unicorn",
				86
			},
			{
				"Wyvern",
				87
			},
			{
				"Giant Bat",
				93
			},
			{
				"Corruptor",
				94
			},
			{
				"Digger",
				95
			},
			{
				"World Feeder",
				98
			},
			{
				"Clinger",
				101
			},
			{
				"Angler Fish",
				102
			},
			{
				"Green Jellyfish",
				103
			},
			{
				"Werewolf",
				104
			},
			{
				"Bound Goblin",
				105
			},
			{
				"Bound Wizard",
				106
			},
			{
				"Goblin Tinkerer",
				107
			},
			{
				"Wizard",
				108
			},
			{
				"Clown",
				109
			},
			{
				"Skeleton Archer",
				110
			},
			{
				"Goblin Archer",
				111
			},
			{
				"Vile Spit",
				112
			},
			{
				"Wall of Flesh",
				113
			},
			{
				"The Hungry",
				115
			},
			{
				"Leech",
				117
			},
			{
				"Chaos Elemental",
				120
			},
			{
				"Slimer",
				121
			},
			{
				"Gastropod",
				122
			},
			{
				"Bound Mechanic",
				123
			},
			{
				"Mechanic",
				124
			},
			{
				"Retinazer",
				125
			},
			{
				"Spazmatism",
				126
			},
			{
				"Skeletron Prime",
				127
			},
			{
				"Prime Cannon",
				128
			},
			{
				"Prime Saw",
				129
			},
			{
				"Prime Vice",
				130
			},
			{
				"Prime Laser",
				131
			},
			{
				"Wandering Eye",
				133
			},
			{
				"The Destroyer",
				134
			},
			{
				"Illuminant Bat",
				137
			},
			{
				"Illuminant Slime",
				138
			},
			{
				"Probe",
				139
			},
			{
				"Possessed Armor",
				140
			},
			{
				"Toxic Sludge",
				141
			},
			{
				"Santa Claus",
				142
			},
			{
				"Snowman Gangsta",
				143
			},
			{
				"Mister Stabby",
				144
			},
			{
				"Snow Balla",
				145
			},
			{
				"Ice Slime",
				147
			},
			{
				"Penguin",
				148
			},
			{
				"Ice Bat",
				150
			},
			{
				"Lava Bat",
				151
			},
			{
				"Giant Flying Fox",
				152
			},
			{
				"Giant Tortoise",
				153
			},
			{
				"Ice Tortoise",
				154
			},
			{
				"Wolf",
				155
			},
			{
				"Red Devil",
				156
			},
			{
				"Arapaima",
				157
			},
			{
				"Vampire",
				158
			},
			{
				"Truffle",
				160
			},
			{
				"Zombie Eskimo",
				161
			},
			{
				"Frankenstein",
				162
			},
			{
				"Black Recluse",
				163
			},
			{
				"Wall Creeper",
				164
			},
			{
				"Swamp Thing",
				166
			},
			{
				"Undead Viking",
				167
			},
			{
				"Corrupt Penguin",
				168
			},
			{
				"Ice Elemental",
				169
			},
			{
				"Pigron",
				170
			},
			{
				"Rune Wizard",
				172
			},
			{
				"Crimera",
				173
			},
			{
				"Herpling",
				174
			},
			{
				"Angry Trapper",
				175
			},
			{
				"Moss Hornet",
				176
			},
			{
				"Derpling",
				177
			},
			{
				"Steampunker",
				178
			},
			{
				"Crimson Axe",
				179
			},
			{
				"Face Monster",
				181
			},
			{
				"Floaty Gross",
				182
			},
			{
				"Crimslime",
				183
			},
			{
				"Spiked Ice Slime",
				184
			},
			{
				"Snow Flinx",
				185
			},
			{
				"Lost Girl",
				195
			},
			{
				"Nymph",
				196
			},
			{
				"Armored Viking",
				197
			},
			{
				"Lihzahrd",
				198
			},
			{
				"Spiked Jungle Slime",
				204
			},
			{
				"Moth",
				205
			},
			{
				"Icy Merman",
				206
			},
			{
				"Dye Trader",
				207
			},
			{
				"Party Girl",
				208
			},
			{
				"Cyborg",
				209
			},
			{
				"Bee",
				210
			},
			{
				"Pirate Deckhand",
				212
			},
			{
				"Pirate Corsair",
				213
			},
			{
				"Pirate Deadeye",
				214
			},
			{
				"Pirate Crossbower",
				215
			},
			{
				"Pirate Captain",
				216
			},
			{
				"Cochineal Beetle",
				217
			},
			{
				"Cyan Beetle",
				218
			},
			{
				"Lac Beetle",
				219
			},
			{
				"Sea Snail",
				220
			},
			{
				"Squid",
				221
			},
			{
				"Queen Bee",
				222
			},
			{
				"Raincoat Zombie",
				223
			},
			{
				"Flying Fish",
				224
			},
			{
				"Umbrella Slime",
				225
			},
			{
				"Flying Snake",
				226
			},
			{
				"Painter",
				227
			},
			{
				"Witch Doctor",
				228
			},
			{
				"Pirate",
				229
			},
			{
				"Jungle Creeper",
				236
			},
			{
				"Blood Crawler",
				239
			},
			{
				"Blood Feeder",
				241
			},
			{
				"Blood Jelly",
				242
			},
			{
				"Ice Golem",
				243
			},
			{
				"Rainbow Slime",
				244
			},
			{
				"Golem",
				245
			},
			{
				"Golem Head",
				246
			},
			{
				"Golem Fist",
				247
			},
			{
				"Angry Nimbus",
				250
			},
			{
				"Eyezor",
				251
			},
			{
				"Parrot",
				252
			},
			{
				"Reaper",
				253
			},
			{
				"Spore Zombie",
				254
			},
			{
				"Fungo Fish",
				256
			},
			{
				"Anomura Fungus",
				257
			},
			{
				"Mushi Ladybug",
				258
			},
			{
				"Fungi Bulb",
				259
			},
			{
				"Giant Fungi Bulb",
				260
			},
			{
				"Fungi Spore",
				261
			},
			{
				"Plantera",
				262
			},
			{
				"Plantera's Hook",
				263
			},
			{
				"Plantera's Tentacle",
				264
			},
			{
				"Spore",
				265
			},
			{
				"Brain of Cthulhu",
				266
			},
			{
				"Creeper",
				267
			},
			{
				"Ichor Sticker",
				268
			},
			{
				"Rusty Armored Bones",
				269
			},
			{
				"Blue Armored Bones",
				273
			},
			{
				"Hell Armored Bones",
				277
			},
			{
				"Ragged Caster",
				281
			},
			{
				"Necromancer",
				283
			},
			{
				"Diabolist",
				285
			},
			{
				"Bone Lee",
				287
			},
			{
				"Dungeon Spirit",
				288
			},
			{
				"Giant Cursed Skull",
				289
			},
			{
				"Paladin",
				290
			},
			{
				"Skeleton Sniper",
				291
			},
			{
				"Tactical Skeleton",
				292
			},
			{
				"Skeleton Commando",
				293
			},
			{
				"Blue Jay",
				297
			},
			{
				"Cardinal",
				298
			},
			{
				"Squirrel",
				299
			},
			{
				"Mouse",
				300
			},
			{
				"Raven",
				301
			},
			{
				"Slime",
				302
			},
			{
				"Hoppin' Jack",
				304
			},
			{
				"Scarecrow",
				305
			},
			{
				"Headless Horseman",
				315
			},
			{
				"Ghost",
				316
			},
			{
				"Mourning Wood",
				325
			},
			{
				"Splinterling",
				326
			},
			{
				"Pumpking",
				327
			},
			{
				"Hellhound",
				329
			},
			{
				"Poltergeist",
				330
			},
			{
				"Zombie Elf",
				338
			},
			{
				"Present Mimic",
				341
			},
			{
				"Gingerbread Man",
				342
			},
			{
				"Yeti",
				343
			},
			{
				"Everscream",
				344
			},
			{
				"Ice Queen",
				345
			},
			{
				"Santa",
				346
			},
			{
				"Elf Copter",
				347
			},
			{
				"Nutcracker",
				348
			},
			{
				"Elf Archer",
				350
			},
			{
				"Krampus",
				351
			},
			{
				"Flocko",
				352
			},
			{
				"Stylist",
				353
			},
			{
				"Webbed Stylist",
				354
			},
			{
				"Firefly",
				355
			},
			{
				"Butterfly",
				356
			},
			{
				"Worm",
				357
			},
			{
				"Lightning Bug",
				358
			},
			{
				"Snail",
				359
			},
			{
				"Glowing Snail",
				360
			},
			{
				"Frog",
				361
			},
			{
				"Duck",
				362
			},
			{
				"Scorpion",
				366
			},
			{
				"Traveling Merchant",
				368
			},
			{
				"Angler",
				369
			},
			{
				"Duke Fishron",
				370
			},
			{
				"Detonating Bubble",
				371
			},
			{
				"Sharkron",
				372
			},
			{
				"Truffle Worm",
				374
			},
			{
				"Sleeping Angler",
				376
			},
			{
				"Grasshopper",
				377
			},
			{
				"Chattering Teeth Bomb",
				378
			},
			{
				"Blue Cultist Archer",
				379
			},
			{
				"White Cultist Archer",
				380
			},
			{
				"Brain Scrambler",
				381
			},
			{
				"Ray Gunner",
				382
			},
			{
				"Martian Officer",
				383
			},
			{
				"Bubble Shield",
				384
			},
			{
				"Gray Grunt",
				385
			},
			{
				"Martian Engineer",
				386
			},
			{
				"Tesla Turret",
				387
			},
			{
				"Martian Drone",
				388
			},
			{
				"Gigazapper",
				389
			},
			{
				"Scutlix Gunner",
				390
			},
			{
				"Scutlix",
				391
			},
			{
				"Martian Saucer",
				392
			},
			{
				"Martian Saucer Turret",
				393
			},
			{
				"Martian Saucer Cannon",
				394
			},
			{
				"Moon Lord",
				396
			},
			{
				"Moon Lord's Hand",
				397
			},
			{
				"Moon Lord's Core",
				398
			},
			{
				"Martian Probe",
				399
			},
			{
				"Milkyway Weaver",
				402
			},
			{
				"Star Cell",
				405
			},
			{
				"Flow Invader",
				407
			},
			{
				"Twinkle Popper",
				409
			},
			{
				"Twinkle",
				410
			},
			{
				"Stargazer",
				411
			},
			{
				"Crawltipede",
				412
			},
			{
				"Drakomire",
				415
			},
			{
				"Drakomire Rider",
				416
			},
			{
				"Sroller",
				417
			},
			{
				"Corite",
				418
			},
			{
				"Selenian",
				419
			},
			{
				"Nebula Floater",
				420
			},
			{
				"Brain Suckler",
				421
			},
			{
				"Vortex Pillar",
				422
			},
			{
				"Evolution Beast",
				423
			},
			{
				"Predictor",
				424
			},
			{
				"Storm Diver",
				425
			},
			{
				"Alien Queen",
				426
			},
			{
				"Alien Hornet",
				427
			},
			{
				"Alien Larva",
				428
			},
			{
				"Vortexian",
				429
			},
			{
				"Mysterious Tablet",
				437
			},
			{
				"Lunatic Devote",
				438
			},
			{
				"Lunatic Cultist",
				439
			},
			{
				"Tax Collector",
				441
			},
			{
				"Gold Bird",
				442
			},
			{
				"Gold Bunny",
				443
			},
			{
				"Gold Butterfly",
				444
			},
			{
				"Gold Frog",
				445
			},
			{
				"Gold Grasshopper",
				446
			},
			{
				"Gold Mouse",
				447
			},
			{
				"Gold Worm",
				448
			},
			{
				"Phantasm Dragon",
				454
			},
			{
				"Butcher",
				460
			},
			{
				"Creature from the Deep",
				461
			},
			{
				"Fritz",
				462
			},
			{
				"Nailhead",
				463
			},
			{
				"Crimtane Bunny",
				464
			},
			{
				"Crimtane Goldfish",
				465
			},
			{
				"Psycho",
				466
			},
			{
				"Deadly Sphere",
				467
			},
			{
				"Dr. Man Fly",
				468
			},
			{
				"The Possessed",
				469
			},
			{
				"Vicious Penguin",
				470
			},
			{
				"Goblin Summoner",
				471
			},
			{
				"Shadowflame Apparation",
				472
			},
			{
				"Corrupt Mimic",
				473
			},
			{
				"Crimson Mimic",
				474
			},
			{
				"Hallowed Mimic",
				475
			},
			{
				"Jungle Mimic",
				476
			},
			{
				"Mothron",
				477
			},
			{
				"Mothron Egg",
				478
			},
			{
				"Baby Mothron",
				479
			},
			{
				"Medusa",
				480
			},
			{
				"Hoplite",
				481
			},
			{
				"Granite Golem",
				482
			},
			{
				"Granite Elemental",
				483
			},
			{
				"Enchanted Nightcrawler",
				484
			},
			{
				"Grubby",
				485
			},
			{
				"Sluggy",
				486
			},
			{
				"Buggy",
				487
			},
			{
				"Target Dummy",
				488
			},
			{
				"Blood Zombie",
				489
			},
			{
				"Drippler",
				490
			},
			{
				"Stardust Pillar",
				493
			},
			{
				"Crawdad",
				494
			},
			{
				"Giant Shelly",
				496
			},
			{
				"Salamander",
				498
			},
			{
				"Nebula Pillar",
				507
			},
			{
				"Antlion Charger",
				508
			},
			{
				"Antlion Swarmer",
				509
			},
			{
				"Dune Splicer",
				510
			},
			{
				"Tomb Crawler",
				513
			},
			{
				"Solar Flare",
				516
			},
			{
				"Solar Pillar",
				517
			},
			{
				"Drakanian",
				518
			},
			{
				"Solar Fragment",
				519
			},
			{
				"Martian Walker",
				520
			},
			{
				"Ancient Vision",
				521
			},
			{
				"Ancient Light",
				522
			},
			{
				"Ancient Doom",
				523
			},
			{
				"Ghoul",
				524
			},
			{
				"Vile Ghoul",
				525
			},
			{
				"Tainted Ghoul",
				526
			},
			{
				"Dreamer Ghoul",
				527
			},
			{
				"Lamia",
				528
			},
			{
				"Sand Poacher",
				530
			},
			{
				"Basilisk",
				532
			},
			{
				"Desert Spirit",
				533
			},
			{
				"Tortured Soul",
				534
			},
			{
				"Spiked Slime",
				535
			},
			{
				"The Bride",
				536
			},
			{
				"Sand Slime",
				537
			},
			{
				"Red Squirrel",
				538
			},
			{
				"Gold Squirrel",
				539
			},
			{
				"Sand Elemental",
				541
			},
			{
				"Sand Shark",
				542
			},
			{
				"Bone Biter",
				543
			},
			{
				"Flesh Reaver",
				544
			},
			{
				"Crystal Thresher",
				545
			},
			{
				"Angry Tumbler",
				546
			},
			{
				"???",
				547
			},
			{
				"Eternia Crystal",
				548
			},
			{
				"Mysterious Portal",
				549
			},
			{
				"Tavernkeep",
				550
			},
			{
				"Betsy",
				551
			},
			{
				"Etherian Goblin",
				552
			},
			{
				"Etherian Goblin Bomber",
				555
			},
			{
				"Etherian Wyvern",
				558
			},
			{
				"Etherian Javelin Thrower",
				561
			},
			{
				"Dark Mage",
				564
			},
			{
				"Old One's Skeleton",
				566
			},
			{
				"Wither Beast",
				568
			},
			{
				"Drakin",
				570
			},
			{
				"Kobold",
				572
			},
			{
				"Kobold Glider",
				574
			},
			{
				"Ogre",
				576
			},
			{
				"Etherian Lightning Bug",
				578
			}
		};

		// Token: 0x040043D9 RID: 17369
		public const short NegativeIDCount = -66;

		// Token: 0x040043DA RID: 17370
		public const short BigHornetStingy = -65;

		// Token: 0x040043DB RID: 17371
		public const short LittleHornetStingy = -64;

		// Token: 0x040043DC RID: 17372
		public const short BigHornetSpikey = -63;

		// Token: 0x040043DD RID: 17373
		public const short LittleHornetSpikey = -62;

		// Token: 0x040043DE RID: 17374
		public const short BigHornetLeafy = -61;

		// Token: 0x040043DF RID: 17375
		public const short LittleHornetLeafy = -60;

		// Token: 0x040043E0 RID: 17376
		public const short BigHornetHoney = -59;

		// Token: 0x040043E1 RID: 17377
		public const short LittleHornetHoney = -58;

		// Token: 0x040043E2 RID: 17378
		public const short BigHornetFatty = -57;

		// Token: 0x040043E3 RID: 17379
		public const short LittleHornetFatty = -56;

		// Token: 0x040043E4 RID: 17380
		public const short BigRainZombie = -55;

		// Token: 0x040043E5 RID: 17381
		public const short SmallRainZombie = -54;

		// Token: 0x040043E6 RID: 17382
		public const short BigPantlessSkeleton = -53;

		// Token: 0x040043E7 RID: 17383
		public const short SmallPantlessSkeleton = -52;

		// Token: 0x040043E8 RID: 17384
		public const short BigMisassembledSkeleton = -51;

		// Token: 0x040043E9 RID: 17385
		public const short SmallMisassembledSkeleton = -50;

		// Token: 0x040043EA RID: 17386
		public const short BigHeadacheSkeleton = -49;

		// Token: 0x040043EB RID: 17387
		public const short SmallHeadacheSkeleton = -48;

		// Token: 0x040043EC RID: 17388
		public const short BigSkeleton = -47;

		// Token: 0x040043ED RID: 17389
		public const short SmallSkeleton = -46;

		// Token: 0x040043EE RID: 17390
		public const short BigFemaleZombie = -45;

		// Token: 0x040043EF RID: 17391
		public const short SmallFemaleZombie = -44;

		// Token: 0x040043F0 RID: 17392
		public const short DemonEye2 = -43;

		// Token: 0x040043F1 RID: 17393
		public const short PurpleEye2 = -42;

		// Token: 0x040043F2 RID: 17394
		public const short GreenEye2 = -41;

		// Token: 0x040043F3 RID: 17395
		public const short DialatedEye2 = -40;

		// Token: 0x040043F4 RID: 17396
		public const short SleepyEye2 = -39;

		// Token: 0x040043F5 RID: 17397
		public const short CataractEye2 = -38;

		// Token: 0x040043F6 RID: 17398
		public const short BigTwiggyZombie = -37;

		// Token: 0x040043F7 RID: 17399
		public const short SmallTwiggyZombie = -36;

		// Token: 0x040043F8 RID: 17400
		public const short BigSwampZombie = -35;

		// Token: 0x040043F9 RID: 17401
		public const short SmallSwampZombie = -34;

		// Token: 0x040043FA RID: 17402
		public const short BigSlimedZombie = -33;

		// Token: 0x040043FB RID: 17403
		public const short SmallSlimedZombie = -32;

		// Token: 0x040043FC RID: 17404
		public const short BigPincushionZombie = -31;

		// Token: 0x040043FD RID: 17405
		public const short SmallPincushionZombie = -30;

		// Token: 0x040043FE RID: 17406
		public const short BigBaldZombie = -29;

		// Token: 0x040043FF RID: 17407
		public const short SmallBaldZombie = -28;

		// Token: 0x04004400 RID: 17408
		public const short BigZombie = -27;

		// Token: 0x04004401 RID: 17409
		public const short SmallZombie = -26;

		// Token: 0x04004402 RID: 17410
		public const short BigCrimslime = -25;

		// Token: 0x04004403 RID: 17411
		public const short LittleCrimslime = -24;

		// Token: 0x04004404 RID: 17412
		public const short BigCrimera = -23;

		// Token: 0x04004405 RID: 17413
		public const short LittleCrimera = -22;

		// Token: 0x04004406 RID: 17414
		public const short GiantMossHornet = -21;

		// Token: 0x04004407 RID: 17415
		public const short BigMossHornet = -20;

		// Token: 0x04004408 RID: 17416
		public const short LittleMossHornet = -19;

		// Token: 0x04004409 RID: 17417
		public const short TinyMossHornet = -18;

		// Token: 0x0400440A RID: 17418
		public const short BigStinger = -17;

		// Token: 0x0400440B RID: 17419
		public const short LittleStinger = -16;

		// Token: 0x0400440C RID: 17420
		public const short HeavySkeleton = -15;

		// Token: 0x0400440D RID: 17421
		public const short BigBoned = -14;

		// Token: 0x0400440E RID: 17422
		public const short ShortBones = -13;

		// Token: 0x0400440F RID: 17423
		public const short BigEater = -12;

		// Token: 0x04004410 RID: 17424
		public const short LittleEater = -11;

		// Token: 0x04004411 RID: 17425
		public const short JungleSlime = -10;

		// Token: 0x04004412 RID: 17426
		public const short YellowSlime = -9;

		// Token: 0x04004413 RID: 17427
		public const short RedSlime = -8;

		// Token: 0x04004414 RID: 17428
		public const short PurpleSlime = -7;

		// Token: 0x04004415 RID: 17429
		public const short BlackSlime = -6;

		// Token: 0x04004416 RID: 17430
		public const short BabySlime = -5;

		// Token: 0x04004417 RID: 17431
		public const short Pinky = -4;

		// Token: 0x04004418 RID: 17432
		public const short GreenSlime = -3;

		// Token: 0x04004419 RID: 17433
		public const short Slimer2 = -2;

		// Token: 0x0400441A RID: 17434
		public const short Slimeling = -1;

		// Token: 0x0400441B RID: 17435
		public const short None = 0;

		// Token: 0x0400441C RID: 17436
		public const short BlueSlime = 1;

		// Token: 0x0400441D RID: 17437
		public const short DemonEye = 2;

		// Token: 0x0400441E RID: 17438
		public const short Zombie = 3;

		// Token: 0x0400441F RID: 17439
		public const short EyeofCthulhu = 4;

		// Token: 0x04004420 RID: 17440
		public const short ServantofCthulhu = 5;

		// Token: 0x04004421 RID: 17441
		public const short EaterofSouls = 6;

		// Token: 0x04004422 RID: 17442
		public const short DevourerHead = 7;

		// Token: 0x04004423 RID: 17443
		public const short DevourerBody = 8;

		// Token: 0x04004424 RID: 17444
		public const short DevourerTail = 9;

		// Token: 0x04004425 RID: 17445
		public const short GiantWormHead = 10;

		// Token: 0x04004426 RID: 17446
		public const short GiantWormBody = 11;

		// Token: 0x04004427 RID: 17447
		public const short GiantWormTail = 12;

		// Token: 0x04004428 RID: 17448
		public const short EaterofWorldsHead = 13;

		// Token: 0x04004429 RID: 17449
		public const short EaterofWorldsBody = 14;

		// Token: 0x0400442A RID: 17450
		public const short EaterofWorldsTail = 15;

		// Token: 0x0400442B RID: 17451
		public const short MotherSlime = 16;

		// Token: 0x0400442C RID: 17452
		public const short Merchant = 17;

		// Token: 0x0400442D RID: 17453
		public const short Nurse = 18;

		// Token: 0x0400442E RID: 17454
		public const short ArmsDealer = 19;

		// Token: 0x0400442F RID: 17455
		public const short Dryad = 20;

		// Token: 0x04004430 RID: 17456
		public const short Skeleton = 21;

		// Token: 0x04004431 RID: 17457
		public const short Guide = 22;

		// Token: 0x04004432 RID: 17458
		public const short MeteorHead = 23;

		// Token: 0x04004433 RID: 17459
		public const short FireImp = 24;

		// Token: 0x04004434 RID: 17460
		public const short BurningSphere = 25;

		// Token: 0x04004435 RID: 17461
		public const short GoblinPeon = 26;

		// Token: 0x04004436 RID: 17462
		public const short GoblinThief = 27;

		// Token: 0x04004437 RID: 17463
		public const short GoblinWarrior = 28;

		// Token: 0x04004438 RID: 17464
		public const short GoblinSorcerer = 29;

		// Token: 0x04004439 RID: 17465
		public const short ChaosBall = 30;

		// Token: 0x0400443A RID: 17466
		public const short AngryBones = 31;

		// Token: 0x0400443B RID: 17467
		public const short DarkCaster = 32;

		// Token: 0x0400443C RID: 17468
		public const short WaterSphere = 33;

		// Token: 0x0400443D RID: 17469
		public const short CursedSkull = 34;

		// Token: 0x0400443E RID: 17470
		public const short SkeletronHead = 35;

		// Token: 0x0400443F RID: 17471
		public const short SkeletronHand = 36;

		// Token: 0x04004440 RID: 17472
		public const short OldMan = 37;

		// Token: 0x04004441 RID: 17473
		public const short Demolitionist = 38;

		// Token: 0x04004442 RID: 17474
		public const short BoneSerpentHead = 39;

		// Token: 0x04004443 RID: 17475
		public const short BoneSerpentBody = 40;

		// Token: 0x04004444 RID: 17476
		public const short BoneSerpentTail = 41;

		// Token: 0x04004445 RID: 17477
		public const short Hornet = 42;

		// Token: 0x04004446 RID: 17478
		public const short ManEater = 43;

		// Token: 0x04004447 RID: 17479
		public const short UndeadMiner = 44;

		// Token: 0x04004448 RID: 17480
		public const short Tim = 45;

		// Token: 0x04004449 RID: 17481
		public const short Bunny = 46;

		// Token: 0x0400444A RID: 17482
		public const short CorruptBunny = 47;

		// Token: 0x0400444B RID: 17483
		public const short Harpy = 48;

		// Token: 0x0400444C RID: 17484
		public const short CaveBat = 49;

		// Token: 0x0400444D RID: 17485
		public const short KingSlime = 50;

		// Token: 0x0400444E RID: 17486
		public const short JungleBat = 51;

		// Token: 0x0400444F RID: 17487
		public const short DoctorBones = 52;

		// Token: 0x04004450 RID: 17488
		public const short TheGroom = 53;

		// Token: 0x04004451 RID: 17489
		public const short Clothier = 54;

		// Token: 0x04004452 RID: 17490
		public const short Goldfish = 55;

		// Token: 0x04004453 RID: 17491
		public const short Snatcher = 56;

		// Token: 0x04004454 RID: 17492
		public const short CorruptGoldfish = 57;

		// Token: 0x04004455 RID: 17493
		public const short Piranha = 58;

		// Token: 0x04004456 RID: 17494
		public const short LavaSlime = 59;

		// Token: 0x04004457 RID: 17495
		public const short Hellbat = 60;

		// Token: 0x04004458 RID: 17496
		public const short Vulture = 61;

		// Token: 0x04004459 RID: 17497
		public const short Demon = 62;

		// Token: 0x0400445A RID: 17498
		public const short BlueJellyfish = 63;

		// Token: 0x0400445B RID: 17499
		public const short PinkJellyfish = 64;

		// Token: 0x0400445C RID: 17500
		public const short Shark = 65;

		// Token: 0x0400445D RID: 17501
		public const short VoodooDemon = 66;

		// Token: 0x0400445E RID: 17502
		public const short Crab = 67;

		// Token: 0x0400445F RID: 17503
		public const short DungeonGuardian = 68;

		// Token: 0x04004460 RID: 17504
		public const short Antlion = 69;

		// Token: 0x04004461 RID: 17505
		public const short SpikeBall = 70;

		// Token: 0x04004462 RID: 17506
		public const short DungeonSlime = 71;

		// Token: 0x04004463 RID: 17507
		public const short BlazingWheel = 72;

		// Token: 0x04004464 RID: 17508
		public const short GoblinScout = 73;

		// Token: 0x04004465 RID: 17509
		public const short Bird = 74;

		// Token: 0x04004466 RID: 17510
		public const short Pixie = 75;

		// Token: 0x04004467 RID: 17511
		public const short None2 = 76;

		// Token: 0x04004468 RID: 17512
		public const short ArmoredSkeleton = 77;

		// Token: 0x04004469 RID: 17513
		public const short Mummy = 78;

		// Token: 0x0400446A RID: 17514
		public const short DarkMummy = 79;

		// Token: 0x0400446B RID: 17515
		public const short LightMummy = 80;

		// Token: 0x0400446C RID: 17516
		public const short CorruptSlime = 81;

		// Token: 0x0400446D RID: 17517
		public const short Wraith = 82;

		// Token: 0x0400446E RID: 17518
		public const short CursedHammer = 83;

		// Token: 0x0400446F RID: 17519
		public const short EnchantedSword = 84;

		// Token: 0x04004470 RID: 17520
		public const short Mimic = 85;

		// Token: 0x04004471 RID: 17521
		public const short Unicorn = 86;

		// Token: 0x04004472 RID: 17522
		public const short WyvernHead = 87;

		// Token: 0x04004473 RID: 17523
		public const short WyvernLegs = 88;

		// Token: 0x04004474 RID: 17524
		public const short WyvernBody = 89;

		// Token: 0x04004475 RID: 17525
		public const short WyvernBody2 = 90;

		// Token: 0x04004476 RID: 17526
		public const short WyvernBody3 = 91;

		// Token: 0x04004477 RID: 17527
		public const short WyvernTail = 92;

		// Token: 0x04004478 RID: 17528
		public const short GiantBat = 93;

		// Token: 0x04004479 RID: 17529
		public const short Corruptor = 94;

		// Token: 0x0400447A RID: 17530
		public const short DiggerHead = 95;

		// Token: 0x0400447B RID: 17531
		public const short DiggerBody = 96;

		// Token: 0x0400447C RID: 17532
		public const short DiggerTail = 97;

		// Token: 0x0400447D RID: 17533
		public const short SeekerHead = 98;

		// Token: 0x0400447E RID: 17534
		public const short SeekerBody = 99;

		// Token: 0x0400447F RID: 17535
		public const short SeekerTail = 100;

		// Token: 0x04004480 RID: 17536
		public const short Clinger = 101;

		// Token: 0x04004481 RID: 17537
		public const short AnglerFish = 102;

		// Token: 0x04004482 RID: 17538
		public const short GreenJellyfish = 103;

		// Token: 0x04004483 RID: 17539
		public const short Werewolf = 104;

		// Token: 0x04004484 RID: 17540
		public const short BoundGoblin = 105;

		// Token: 0x04004485 RID: 17541
		public const short BoundWizard = 106;

		// Token: 0x04004486 RID: 17542
		public const short GoblinTinkerer = 107;

		// Token: 0x04004487 RID: 17543
		public const short Wizard = 108;

		// Token: 0x04004488 RID: 17544
		public const short Clown = 109;

		// Token: 0x04004489 RID: 17545
		public const short SkeletonArcher = 110;

		// Token: 0x0400448A RID: 17546
		public const short GoblinArcher = 111;

		// Token: 0x0400448B RID: 17547
		public const short VileSpit = 112;

		// Token: 0x0400448C RID: 17548
		public const short WallofFlesh = 113;

		// Token: 0x0400448D RID: 17549
		public const short WallofFleshEye = 114;

		// Token: 0x0400448E RID: 17550
		public const short TheHungry = 115;

		// Token: 0x0400448F RID: 17551
		public const short TheHungryII = 116;

		// Token: 0x04004490 RID: 17552
		public const short LeechHead = 117;

		// Token: 0x04004491 RID: 17553
		public const short LeechBody = 118;

		// Token: 0x04004492 RID: 17554
		public const short LeechTail = 119;

		// Token: 0x04004493 RID: 17555
		public const short ChaosElemental = 120;

		// Token: 0x04004494 RID: 17556
		public const short Slimer = 121;

		// Token: 0x04004495 RID: 17557
		public const short Gastropod = 122;

		// Token: 0x04004496 RID: 17558
		public const short BoundMechanic = 123;

		// Token: 0x04004497 RID: 17559
		public const short Mechanic = 124;

		// Token: 0x04004498 RID: 17560
		public const short Retinazer = 125;

		// Token: 0x04004499 RID: 17561
		public const short Spazmatism = 126;

		// Token: 0x0400449A RID: 17562
		public const short SkeletronPrime = 127;

		// Token: 0x0400449B RID: 17563
		public const short PrimeCannon = 128;

		// Token: 0x0400449C RID: 17564
		public const short PrimeSaw = 129;

		// Token: 0x0400449D RID: 17565
		public const short PrimeVice = 130;

		// Token: 0x0400449E RID: 17566
		public const short PrimeLaser = 131;

		// Token: 0x0400449F RID: 17567
		public const short BaldZombie = 132;

		// Token: 0x040044A0 RID: 17568
		public const short WanderingEye = 133;

		// Token: 0x040044A1 RID: 17569
		public const short TheDestroyer = 134;

		// Token: 0x040044A2 RID: 17570
		public const short TheDestroyerBody = 135;

		// Token: 0x040044A3 RID: 17571
		public const short TheDestroyerTail = 136;

		// Token: 0x040044A4 RID: 17572
		public const short IlluminantBat = 137;

		// Token: 0x040044A5 RID: 17573
		public const short IlluminantSlime = 138;

		// Token: 0x040044A6 RID: 17574
		public const short Probe = 139;

		// Token: 0x040044A7 RID: 17575
		public const short PossessedArmor = 140;

		// Token: 0x040044A8 RID: 17576
		public const short ToxicSludge = 141;

		// Token: 0x040044A9 RID: 17577
		public const short SantaClaus = 142;

		// Token: 0x040044AA RID: 17578
		public const short SnowmanGangsta = 143;

		// Token: 0x040044AB RID: 17579
		public const short MisterStabby = 144;

		// Token: 0x040044AC RID: 17580
		public const short SnowBalla = 145;

		// Token: 0x040044AD RID: 17581
		public const short None3 = 146;

		// Token: 0x040044AE RID: 17582
		public const short IceSlime = 147;

		// Token: 0x040044AF RID: 17583
		public const short Penguin = 148;

		// Token: 0x040044B0 RID: 17584
		public const short PenguinBlack = 149;

		// Token: 0x040044B1 RID: 17585
		public const short IceBat = 150;

		// Token: 0x040044B2 RID: 17586
		public const short Lavabat = 151;

		// Token: 0x040044B3 RID: 17587
		public const short GiantFlyingFox = 152;

		// Token: 0x040044B4 RID: 17588
		public const short GiantTortoise = 153;

		// Token: 0x040044B5 RID: 17589
		public const short IceTortoise = 154;

		// Token: 0x040044B6 RID: 17590
		public const short Wolf = 155;

		// Token: 0x040044B7 RID: 17591
		public const short RedDevil = 156;

		// Token: 0x040044B8 RID: 17592
		public const short Arapaima = 157;

		// Token: 0x040044B9 RID: 17593
		public const short VampireBat = 158;

		// Token: 0x040044BA RID: 17594
		public const short Vampire = 159;

		// Token: 0x040044BB RID: 17595
		public const short Truffle = 160;

		// Token: 0x040044BC RID: 17596
		public const short ZombieEskimo = 161;

		// Token: 0x040044BD RID: 17597
		public const short Frankenstein = 162;

		// Token: 0x040044BE RID: 17598
		public const short BlackRecluse = 163;

		// Token: 0x040044BF RID: 17599
		public const short WallCreeper = 164;

		// Token: 0x040044C0 RID: 17600
		public const short WallCreeperWall = 165;

		// Token: 0x040044C1 RID: 17601
		public const short SwampThing = 166;

		// Token: 0x040044C2 RID: 17602
		public const short UndeadViking = 167;

		// Token: 0x040044C3 RID: 17603
		public const short CorruptPenguin = 168;

		// Token: 0x040044C4 RID: 17604
		public const short IceElemental = 169;

		// Token: 0x040044C5 RID: 17605
		public const short PigronCorruption = 170;

		// Token: 0x040044C6 RID: 17606
		public const short PigronHallow = 171;

		// Token: 0x040044C7 RID: 17607
		public const short RuneWizard = 172;

		// Token: 0x040044C8 RID: 17608
		public const short Crimera = 173;

		// Token: 0x040044C9 RID: 17609
		public const short Herpling = 174;

		// Token: 0x040044CA RID: 17610
		public const short AngryTrapper = 175;

		// Token: 0x040044CB RID: 17611
		public const short MossHornet = 176;

		// Token: 0x040044CC RID: 17612
		public const short Derpling = 177;

		// Token: 0x040044CD RID: 17613
		public const short Steampunker = 178;

		// Token: 0x040044CE RID: 17614
		public const short CrimsonAxe = 179;

		// Token: 0x040044CF RID: 17615
		public const short PigronCrimson = 180;

		// Token: 0x040044D0 RID: 17616
		public const short FaceMonster = 181;

		// Token: 0x040044D1 RID: 17617
		public const short FloatyGross = 182;

		// Token: 0x040044D2 RID: 17618
		public const short Crimslime = 183;

		// Token: 0x040044D3 RID: 17619
		public const short SpikedIceSlime = 184;

		// Token: 0x040044D4 RID: 17620
		public const short SnowFlinx = 185;

		// Token: 0x040044D5 RID: 17621
		public const short PincushionZombie = 186;

		// Token: 0x040044D6 RID: 17622
		public const short SlimedZombie = 187;

		// Token: 0x040044D7 RID: 17623
		public const short SwampZombie = 188;

		// Token: 0x040044D8 RID: 17624
		public const short TwiggyZombie = 189;

		// Token: 0x040044D9 RID: 17625
		public const short CataractEye = 190;

		// Token: 0x040044DA RID: 17626
		public const short SleepyEye = 191;

		// Token: 0x040044DB RID: 17627
		public const short DialatedEye = 192;

		// Token: 0x040044DC RID: 17628
		public const short GreenEye = 193;

		// Token: 0x040044DD RID: 17629
		public const short PurpleEye = 194;

		// Token: 0x040044DE RID: 17630
		public const short LostGirl = 195;

		// Token: 0x040044DF RID: 17631
		public const short Nymph = 196;

		// Token: 0x040044E0 RID: 17632
		public const short ArmoredViking = 197;

		// Token: 0x040044E1 RID: 17633
		public const short Lihzahrd = 198;

		// Token: 0x040044E2 RID: 17634
		public const short LihzahrdCrawler = 199;

		// Token: 0x040044E3 RID: 17635
		public const short FemaleZombie = 200;

		// Token: 0x040044E4 RID: 17636
		public const short HeadacheSkeleton = 201;

		// Token: 0x040044E5 RID: 17637
		public const short MisassembledSkeleton = 202;

		// Token: 0x040044E6 RID: 17638
		public const short PantlessSkeleton = 203;

		// Token: 0x040044E7 RID: 17639
		public const short SpikedJungleSlime = 204;

		// Token: 0x040044E8 RID: 17640
		public const short Moth = 205;

		// Token: 0x040044E9 RID: 17641
		public const short IcyMerman = 206;

		// Token: 0x040044EA RID: 17642
		public const short DyeTrader = 207;

		// Token: 0x040044EB RID: 17643
		public const short PartyGirl = 208;

		// Token: 0x040044EC RID: 17644
		public const short Cyborg = 209;

		// Token: 0x040044ED RID: 17645
		public const short Bee = 210;

		// Token: 0x040044EE RID: 17646
		public const short BeeSmall = 211;

		// Token: 0x040044EF RID: 17647
		public const short PirateDeckhand = 212;

		// Token: 0x040044F0 RID: 17648
		public const short PirateCorsair = 213;

		// Token: 0x040044F1 RID: 17649
		public const short PirateDeadeye = 214;

		// Token: 0x040044F2 RID: 17650
		public const short PirateCrossbower = 215;

		// Token: 0x040044F3 RID: 17651
		public const short PirateCaptain = 216;

		// Token: 0x040044F4 RID: 17652
		public const short CochinealBeetle = 217;

		// Token: 0x040044F5 RID: 17653
		public const short CyanBeetle = 218;

		// Token: 0x040044F6 RID: 17654
		public const short LacBeetle = 219;

		// Token: 0x040044F7 RID: 17655
		public const short SeaSnail = 220;

		// Token: 0x040044F8 RID: 17656
		public const short Squid = 221;

		// Token: 0x040044F9 RID: 17657
		public const short QueenBee = 222;

		// Token: 0x040044FA RID: 17658
		public const short ZombieRaincoat = 223;

		// Token: 0x040044FB RID: 17659
		public const short FlyingFish = 224;

		// Token: 0x040044FC RID: 17660
		public const short UmbrellaSlime = 225;

		// Token: 0x040044FD RID: 17661
		public const short FlyingSnake = 226;

		// Token: 0x040044FE RID: 17662
		public const short Painter = 227;

		// Token: 0x040044FF RID: 17663
		public const short WitchDoctor = 228;

		// Token: 0x04004500 RID: 17664
		public const short Pirate = 229;

		// Token: 0x04004501 RID: 17665
		public const short GoldfishWalker = 230;

		// Token: 0x04004502 RID: 17666
		public const short HornetFatty = 231;

		// Token: 0x04004503 RID: 17667
		public const short HornetHoney = 232;

		// Token: 0x04004504 RID: 17668
		public const short HornetLeafy = 233;

		// Token: 0x04004505 RID: 17669
		public const short HornetSpikey = 234;

		// Token: 0x04004506 RID: 17670
		public const short HornetStingy = 235;

		// Token: 0x04004507 RID: 17671
		public const short JungleCreeper = 236;

		// Token: 0x04004508 RID: 17672
		public const short JungleCreeperWall = 237;

		// Token: 0x04004509 RID: 17673
		public const short BlackRecluseWall = 238;

		// Token: 0x0400450A RID: 17674
		public const short BloodCrawler = 239;

		// Token: 0x0400450B RID: 17675
		public const short BloodCrawlerWall = 240;

		// Token: 0x0400450C RID: 17676
		public const short BloodFeeder = 241;

		// Token: 0x0400450D RID: 17677
		public const short BloodJelly = 242;

		// Token: 0x0400450E RID: 17678
		public const short IceGolem = 243;

		// Token: 0x0400450F RID: 17679
		public const short RainbowSlime = 244;

		// Token: 0x04004510 RID: 17680
		public const short Golem = 245;

		// Token: 0x04004511 RID: 17681
		public const short GolemHead = 246;

		// Token: 0x04004512 RID: 17682
		public const short GolemFistLeft = 247;

		// Token: 0x04004513 RID: 17683
		public const short GolemFistRight = 248;

		// Token: 0x04004514 RID: 17684
		public const short GolemHeadFree = 249;

		// Token: 0x04004515 RID: 17685
		public const short AngryNimbus = 250;

		// Token: 0x04004516 RID: 17686
		public const short Eyezor = 251;

		// Token: 0x04004517 RID: 17687
		public const short Parrot = 252;

		// Token: 0x04004518 RID: 17688
		public const short Reaper = 253;

		// Token: 0x04004519 RID: 17689
		public const short ZombieMushroom = 254;

		// Token: 0x0400451A RID: 17690
		public const short ZombieMushroomHat = 255;

		// Token: 0x0400451B RID: 17691
		public const short FungoFish = 256;

		// Token: 0x0400451C RID: 17692
		public const short AnomuraFungus = 257;

		// Token: 0x0400451D RID: 17693
		public const short MushiLadybug = 258;

		// Token: 0x0400451E RID: 17694
		public const short FungiBulb = 259;

		// Token: 0x0400451F RID: 17695
		public const short GiantFungiBulb = 260;

		// Token: 0x04004520 RID: 17696
		public const short FungiSpore = 261;

		// Token: 0x04004521 RID: 17697
		public const short Plantera = 262;

		// Token: 0x04004522 RID: 17698
		public const short PlanterasHook = 263;

		// Token: 0x04004523 RID: 17699
		public const short PlanterasTentacle = 264;

		// Token: 0x04004524 RID: 17700
		public const short Spore = 265;

		// Token: 0x04004525 RID: 17701
		public const short BrainofCthulhu = 266;

		// Token: 0x04004526 RID: 17702
		public const short Creeper = 267;

		// Token: 0x04004527 RID: 17703
		public const short IchorSticker = 268;

		// Token: 0x04004528 RID: 17704
		public const short RustyArmoredBonesAxe = 269;

		// Token: 0x04004529 RID: 17705
		public const short RustyArmoredBonesFlail = 270;

		// Token: 0x0400452A RID: 17706
		public const short RustyArmoredBonesSword = 271;

		// Token: 0x0400452B RID: 17707
		public const short RustyArmoredBonesSwordNoArmor = 272;

		// Token: 0x0400452C RID: 17708
		public const short BlueArmoredBones = 273;

		// Token: 0x0400452D RID: 17709
		public const short BlueArmoredBonesMace = 274;

		// Token: 0x0400452E RID: 17710
		public const short BlueArmoredBonesNoPants = 275;

		// Token: 0x0400452F RID: 17711
		public const short BlueArmoredBonesSword = 276;

		// Token: 0x04004530 RID: 17712
		public const short HellArmoredBones = 277;

		// Token: 0x04004531 RID: 17713
		public const short HellArmoredBonesSpikeShield = 278;

		// Token: 0x04004532 RID: 17714
		public const short HellArmoredBonesMace = 279;

		// Token: 0x04004533 RID: 17715
		public const short HellArmoredBonesSword = 280;

		// Token: 0x04004534 RID: 17716
		public const short RaggedCaster = 281;

		// Token: 0x04004535 RID: 17717
		public const short RaggedCasterOpenCoat = 282;

		// Token: 0x04004536 RID: 17718
		public const short Necromancer = 283;

		// Token: 0x04004537 RID: 17719
		public const short NecromancerArmored = 284;

		// Token: 0x04004538 RID: 17720
		public const short DiabolistRed = 285;

		// Token: 0x04004539 RID: 17721
		public const short DiabolistWhite = 286;

		// Token: 0x0400453A RID: 17722
		public const short BoneLee = 287;

		// Token: 0x0400453B RID: 17723
		public const short DungeonSpirit = 288;

		// Token: 0x0400453C RID: 17724
		public const short GiantCursedSkull = 289;

		// Token: 0x0400453D RID: 17725
		public const short Paladin = 290;

		// Token: 0x0400453E RID: 17726
		public const short SkeletonSniper = 291;

		// Token: 0x0400453F RID: 17727
		public const short TacticalSkeleton = 292;

		// Token: 0x04004540 RID: 17728
		public const short SkeletonCommando = 293;

		// Token: 0x04004541 RID: 17729
		public const short AngryBonesBig = 294;

		// Token: 0x04004542 RID: 17730
		public const short AngryBonesBigMuscle = 295;

		// Token: 0x04004543 RID: 17731
		public const short AngryBonesBigHelmet = 296;

		// Token: 0x04004544 RID: 17732
		public const short BirdBlue = 297;

		// Token: 0x04004545 RID: 17733
		public const short BirdRed = 298;

		// Token: 0x04004546 RID: 17734
		public const short Squirrel = 299;

		// Token: 0x04004547 RID: 17735
		public const short Mouse = 300;

		// Token: 0x04004548 RID: 17736
		public const short Raven = 301;

		// Token: 0x04004549 RID: 17737
		public const short SlimeMasked = 302;

		// Token: 0x0400454A RID: 17738
		public const short BunnySlimed = 303;

		// Token: 0x0400454B RID: 17739
		public const short HoppinJack = 304;

		// Token: 0x0400454C RID: 17740
		public const short Scarecrow1 = 305;

		// Token: 0x0400454D RID: 17741
		public const short Scarecrow2 = 306;

		// Token: 0x0400454E RID: 17742
		public const short Scarecrow3 = 307;

		// Token: 0x0400454F RID: 17743
		public const short Scarecrow4 = 308;

		// Token: 0x04004550 RID: 17744
		public const short Scarecrow5 = 309;

		// Token: 0x04004551 RID: 17745
		public const short Scarecrow6 = 310;

		// Token: 0x04004552 RID: 17746
		public const short Scarecrow7 = 311;

		// Token: 0x04004553 RID: 17747
		public const short Scarecrow8 = 312;

		// Token: 0x04004554 RID: 17748
		public const short Scarecrow9 = 313;

		// Token: 0x04004555 RID: 17749
		public const short Scarecrow10 = 314;

		// Token: 0x04004556 RID: 17750
		public const short HeadlessHorseman = 315;

		// Token: 0x04004557 RID: 17751
		public const short Ghost = 316;

		// Token: 0x04004558 RID: 17752
		public const short DemonEyeOwl = 317;

		// Token: 0x04004559 RID: 17753
		public const short DemonEyeSpaceship = 318;

		// Token: 0x0400455A RID: 17754
		public const short ZombieDoctor = 319;

		// Token: 0x0400455B RID: 17755
		public const short ZombieSuperman = 320;

		// Token: 0x0400455C RID: 17756
		public const short ZombiePixie = 321;

		// Token: 0x0400455D RID: 17757
		public const short SkeletonTopHat = 322;

		// Token: 0x0400455E RID: 17758
		public const short SkeletonAstonaut = 323;

		// Token: 0x0400455F RID: 17759
		public const short SkeletonAlien = 324;

		// Token: 0x04004560 RID: 17760
		public const short MourningWood = 325;

		// Token: 0x04004561 RID: 17761
		public const short Splinterling = 326;

		// Token: 0x04004562 RID: 17762
		public const short Pumpking = 327;

		// Token: 0x04004563 RID: 17763
		public const short PumpkingBlade = 328;

		// Token: 0x04004564 RID: 17764
		public const short Hellhound = 329;

		// Token: 0x04004565 RID: 17765
		public const short Poltergeist = 330;

		// Token: 0x04004566 RID: 17766
		public const short ZombieXmas = 331;

		// Token: 0x04004567 RID: 17767
		public const short ZombieSweater = 332;

		// Token: 0x04004568 RID: 17768
		public const short SlimeRibbonWhite = 333;

		// Token: 0x04004569 RID: 17769
		public const short SlimeRibbonYellow = 334;

		// Token: 0x0400456A RID: 17770
		public const short SlimeRibbonGreen = 335;

		// Token: 0x0400456B RID: 17771
		public const short SlimeRibbonRed = 336;

		// Token: 0x0400456C RID: 17772
		public const short BunnyXmas = 337;

		// Token: 0x0400456D RID: 17773
		public const short ZombieElf = 338;

		// Token: 0x0400456E RID: 17774
		public const short ZombieElfBeard = 339;

		// Token: 0x0400456F RID: 17775
		public const short ZombieElfGirl = 340;

		// Token: 0x04004570 RID: 17776
		public const short PresentMimic = 341;

		// Token: 0x04004571 RID: 17777
		public const short GingerbreadMan = 342;

		// Token: 0x04004572 RID: 17778
		public const short Yeti = 343;

		// Token: 0x04004573 RID: 17779
		public const short Everscream = 344;

		// Token: 0x04004574 RID: 17780
		public const short IceQueen = 345;

		// Token: 0x04004575 RID: 17781
		public const short SantaNK1 = 346;

		// Token: 0x04004576 RID: 17782
		public const short ElfCopter = 347;

		// Token: 0x04004577 RID: 17783
		public const short Nutcracker = 348;

		// Token: 0x04004578 RID: 17784
		public const short NutcrackerSpinning = 349;

		// Token: 0x04004579 RID: 17785
		public const short ElfArcher = 350;

		// Token: 0x0400457A RID: 17786
		public const short Krampus = 351;

		// Token: 0x0400457B RID: 17787
		public const short Flocko = 352;

		// Token: 0x0400457C RID: 17788
		public const short Stylist = 353;

		// Token: 0x0400457D RID: 17789
		public const short WebbedStylist = 354;

		// Token: 0x0400457E RID: 17790
		public const short Firefly = 355;

		// Token: 0x0400457F RID: 17791
		public const short Butterfly = 356;

		// Token: 0x04004580 RID: 17792
		public const short Worm = 357;

		// Token: 0x04004581 RID: 17793
		public const short LightningBug = 358;

		// Token: 0x04004582 RID: 17794
		public const short Snail = 359;

		// Token: 0x04004583 RID: 17795
		public const short GlowingSnail = 360;

		// Token: 0x04004584 RID: 17796
		public const short Frog = 361;

		// Token: 0x04004585 RID: 17797
		public const short Duck = 362;

		// Token: 0x04004586 RID: 17798
		public const short Duck2 = 363;

		// Token: 0x04004587 RID: 17799
		public const short DuckWhite = 364;

		// Token: 0x04004588 RID: 17800
		public const short DuckWhite2 = 365;

		// Token: 0x04004589 RID: 17801
		public const short ScorpionBlack = 366;

		// Token: 0x0400458A RID: 17802
		public const short Scorpion = 367;

		// Token: 0x0400458B RID: 17803
		public const short TravellingMerchant = 368;

		// Token: 0x0400458C RID: 17804
		public const short Angler = 369;

		// Token: 0x0400458D RID: 17805
		public const short DukeFishron = 370;

		// Token: 0x0400458E RID: 17806
		public const short DetonatingBubble = 371;

		// Token: 0x0400458F RID: 17807
		public const short Sharkron = 372;

		// Token: 0x04004590 RID: 17808
		public const short Sharkron2 = 373;

		// Token: 0x04004591 RID: 17809
		public const short TruffleWorm = 374;

		// Token: 0x04004592 RID: 17810
		public const short TruffleWormDigger = 375;

		// Token: 0x04004593 RID: 17811
		public const short SleepingAngler = 376;

		// Token: 0x04004594 RID: 17812
		public const short Grasshopper = 377;

		// Token: 0x04004595 RID: 17813
		public const short ChatteringTeethBomb = 378;

		// Token: 0x04004596 RID: 17814
		public const short CultistArcherBlue = 379;

		// Token: 0x04004597 RID: 17815
		public const short CultistArcherWhite = 380;

		// Token: 0x04004598 RID: 17816
		public const short BrainScrambler = 381;

		// Token: 0x04004599 RID: 17817
		public const short RayGunner = 382;

		// Token: 0x0400459A RID: 17818
		public const short MartianOfficer = 383;

		// Token: 0x0400459B RID: 17819
		public const short ForceBubble = 384;

		// Token: 0x0400459C RID: 17820
		public const short GrayGrunt = 385;

		// Token: 0x0400459D RID: 17821
		public const short MartianEngineer = 386;

		// Token: 0x0400459E RID: 17822
		public const short MartianTurret = 387;

		// Token: 0x0400459F RID: 17823
		public const short MartianDrone = 388;

		// Token: 0x040045A0 RID: 17824
		public const short GigaZapper = 389;

		// Token: 0x040045A1 RID: 17825
		public const short ScutlixRider = 390;

		// Token: 0x040045A2 RID: 17826
		public const short Scutlix = 391;

		// Token: 0x040045A3 RID: 17827
		public const short MartianSaucer = 392;

		// Token: 0x040045A4 RID: 17828
		public const short MartianSaucerTurret = 393;

		// Token: 0x040045A5 RID: 17829
		public const short MartianSaucerCannon = 394;

		// Token: 0x040045A6 RID: 17830
		public const short MartianSaucerCore = 395;

		// Token: 0x040045A7 RID: 17831
		public const short MoonLordHead = 396;

		// Token: 0x040045A8 RID: 17832
		public const short MoonLordHand = 397;

		// Token: 0x040045A9 RID: 17833
		public const short MoonLordCore = 398;

		// Token: 0x040045AA RID: 17834
		public const short MartianProbe = 399;

		// Token: 0x040045AB RID: 17835
		public const short MoonLordFreeEye = 400;

		// Token: 0x040045AC RID: 17836
		public const short MoonLordLeechBlob = 401;

		// Token: 0x040045AD RID: 17837
		public const short StardustWormHead = 402;

		// Token: 0x040045AE RID: 17838
		public const short StardustWormBody = 403;

		// Token: 0x040045AF RID: 17839
		public const short StardustWormTail = 404;

		// Token: 0x040045B0 RID: 17840
		public const short StardustCellBig = 405;

		// Token: 0x040045B1 RID: 17841
		public const short StardustCellSmall = 406;

		// Token: 0x040045B2 RID: 17842
		public const short StardustJellyfishBig = 407;

		// Token: 0x040045B3 RID: 17843
		public const short StardustJellyfishSmall = 408;

		// Token: 0x040045B4 RID: 17844
		public const short StardustSpiderBig = 409;

		// Token: 0x040045B5 RID: 17845
		public const short StardustSpiderSmall = 410;

		// Token: 0x040045B6 RID: 17846
		public const short StardustSoldier = 411;

		// Token: 0x040045B7 RID: 17847
		public const short SolarCrawltipedeHead = 412;

		// Token: 0x040045B8 RID: 17848
		public const short SolarCrawltipedeBody = 413;

		// Token: 0x040045B9 RID: 17849
		public const short SolarCrawltipedeTail = 414;

		// Token: 0x040045BA RID: 17850
		public const short SolarDrakomire = 415;

		// Token: 0x040045BB RID: 17851
		public const short SolarDrakomireRider = 416;

		// Token: 0x040045BC RID: 17852
		public const short SolarSroller = 417;

		// Token: 0x040045BD RID: 17853
		public const short SolarCorite = 418;

		// Token: 0x040045BE RID: 17854
		public const short SolarSolenian = 419;

		// Token: 0x040045BF RID: 17855
		public const short NebulaBrain = 420;

		// Token: 0x040045C0 RID: 17856
		public const short NebulaHeadcrab = 421;

		// Token: 0x040045C1 RID: 17857
		public const short NebulaBeast = 423;

		// Token: 0x040045C2 RID: 17858
		public const short NebulaSoldier = 424;

		// Token: 0x040045C3 RID: 17859
		public const short VortexRifleman = 425;

		// Token: 0x040045C4 RID: 17860
		public const short VortexHornetQueen = 426;

		// Token: 0x040045C5 RID: 17861
		public const short VortexHornet = 427;

		// Token: 0x040045C6 RID: 17862
		public const short VortexLarva = 428;

		// Token: 0x040045C7 RID: 17863
		public const short VortexSoldier = 429;

		// Token: 0x040045C8 RID: 17864
		public const short ArmedZombie = 430;

		// Token: 0x040045C9 RID: 17865
		public const short ArmedZombieEskimo = 431;

		// Token: 0x040045CA RID: 17866
		public const short ArmedZombiePincussion = 432;

		// Token: 0x040045CB RID: 17867
		public const short ArmedZombieSlimed = 433;

		// Token: 0x040045CC RID: 17868
		public const short ArmedZombieSwamp = 434;

		// Token: 0x040045CD RID: 17869
		public const short ArmedZombieTwiggy = 435;

		// Token: 0x040045CE RID: 17870
		public const short ArmedZombieCenx = 436;

		// Token: 0x040045CF RID: 17871
		public const short CultistTablet = 437;

		// Token: 0x040045D0 RID: 17872
		public const short CultistDevote = 438;

		// Token: 0x040045D1 RID: 17873
		public const short CultistBoss = 439;

		// Token: 0x040045D2 RID: 17874
		public const short CultistBossClone = 440;

		// Token: 0x040045D3 RID: 17875
		public const short GoldBird = 442;

		// Token: 0x040045D4 RID: 17876
		public const short GoldBunny = 443;

		// Token: 0x040045D5 RID: 17877
		public const short GoldButterfly = 444;

		// Token: 0x040045D6 RID: 17878
		public const short GoldFrog = 445;

		// Token: 0x040045D7 RID: 17879
		public const short GoldGrasshopper = 446;

		// Token: 0x040045D8 RID: 17880
		public const short GoldMouse = 447;

		// Token: 0x040045D9 RID: 17881
		public const short GoldWorm = 448;

		// Token: 0x040045DA RID: 17882
		public const short BoneThrowingSkeleton = 449;

		// Token: 0x040045DB RID: 17883
		public const short BoneThrowingSkeleton2 = 450;

		// Token: 0x040045DC RID: 17884
		public const short BoneThrowingSkeleton3 = 451;

		// Token: 0x040045DD RID: 17885
		public const short BoneThrowingSkeleton4 = 452;

		// Token: 0x040045DE RID: 17886
		public const short SkeletonMerchant = 453;

		// Token: 0x040045DF RID: 17887
		public const short CultistDragonHead = 454;

		// Token: 0x040045E0 RID: 17888
		public const short CultistDragonBody1 = 455;

		// Token: 0x040045E1 RID: 17889
		public const short CultistDragonBody2 = 456;

		// Token: 0x040045E2 RID: 17890
		public const short CultistDragonBody3 = 457;

		// Token: 0x040045E3 RID: 17891
		public const short CultistDragonBody4 = 458;

		// Token: 0x040045E4 RID: 17892
		public const short CultistDragonTail = 459;

		// Token: 0x040045E5 RID: 17893
		public const short Butcher = 460;

		// Token: 0x040045E6 RID: 17894
		public const short CreatureFromTheDeep = 461;

		// Token: 0x040045E7 RID: 17895
		public const short Fritz = 462;

		// Token: 0x040045E8 RID: 17896
		public const short Nailhead = 463;

		// Token: 0x040045E9 RID: 17897
		public const short CrimsonBunny = 464;

		// Token: 0x040045EA RID: 17898
		public const short CrimsonGoldfish = 465;

		// Token: 0x040045EB RID: 17899
		public const short Psycho = 466;

		// Token: 0x040045EC RID: 17900
		public const short DeadlySphere = 467;

		// Token: 0x040045ED RID: 17901
		public const short DrManFly = 468;

		// Token: 0x040045EE RID: 17902
		public const short ThePossessed = 469;

		// Token: 0x040045EF RID: 17903
		public const short CrimsonPenguin = 470;

		// Token: 0x040045F0 RID: 17904
		public const short GoblinSummoner = 471;

		// Token: 0x040045F1 RID: 17905
		public const short ShadowFlameApparition = 472;

		// Token: 0x040045F2 RID: 17906
		public const short BigMimicCorruption = 473;

		// Token: 0x040045F3 RID: 17907
		public const short BigMimicCrimson = 474;

		// Token: 0x040045F4 RID: 17908
		public const short BigMimicHallow = 475;

		// Token: 0x040045F5 RID: 17909
		public const short BigMimicJungle = 476;

		// Token: 0x040045F6 RID: 17910
		public const short Mothron = 477;

		// Token: 0x040045F7 RID: 17911
		public const short MothronEgg = 478;

		// Token: 0x040045F8 RID: 17912
		public const short MothronSpawn = 479;

		// Token: 0x040045F9 RID: 17913
		public const short Medusa = 480;

		// Token: 0x040045FA RID: 17914
		public const short GreekSkeleton = 481;

		// Token: 0x040045FB RID: 17915
		public const short GraniteGolem = 482;

		// Token: 0x040045FC RID: 17916
		public const short GraniteFlyer = 483;

		// Token: 0x040045FD RID: 17917
		public const short EnchantedNightcrawler = 484;

		// Token: 0x040045FE RID: 17918
		public const short Grubby = 485;

		// Token: 0x040045FF RID: 17919
		public const short Sluggy = 486;

		// Token: 0x04004600 RID: 17920
		public const short Buggy = 487;

		// Token: 0x04004601 RID: 17921
		public const short TargetDummy = 488;

		// Token: 0x04004602 RID: 17922
		public const short BloodZombie = 489;

		// Token: 0x04004603 RID: 17923
		public const short Drippler = 490;

		// Token: 0x04004604 RID: 17924
		public const short PirateShip = 491;

		// Token: 0x04004605 RID: 17925
		public const short PirateShipCannon = 492;

		// Token: 0x04004606 RID: 17926
		public const short LunarTowerStardust = 493;

		// Token: 0x04004607 RID: 17927
		public const short Crawdad = 494;

		// Token: 0x04004608 RID: 17928
		public const short Crawdad2 = 495;

		// Token: 0x04004609 RID: 17929
		public const short GiantShelly = 496;

		// Token: 0x0400460A RID: 17930
		public const short GiantShelly2 = 497;

		// Token: 0x0400460B RID: 17931
		public const short Salamander = 498;

		// Token: 0x0400460C RID: 17932
		public const short Salamander2 = 499;

		// Token: 0x0400460D RID: 17933
		public const short Salamander3 = 500;

		// Token: 0x0400460E RID: 17934
		public const short Salamander4 = 501;

		// Token: 0x0400460F RID: 17935
		public const short Salamander5 = 502;

		// Token: 0x04004610 RID: 17936
		public const short Salamander6 = 503;

		// Token: 0x04004611 RID: 17937
		public const short Salamander7 = 504;

		// Token: 0x04004612 RID: 17938
		public const short Salamander8 = 505;

		// Token: 0x04004613 RID: 17939
		public const short Salamander9 = 506;

		// Token: 0x04004614 RID: 17940
		public const short LunarTowerNebula = 507;

		// Token: 0x04004615 RID: 17941
		public const short LunarTowerVortex = 422;

		// Token: 0x04004616 RID: 17942
		public const short TaxCollector = 441;

		// Token: 0x04004617 RID: 17943
		public const short GiantWalkingAntlion = 508;

		// Token: 0x04004618 RID: 17944
		public const short GiantFlyingAntlion = 509;

		// Token: 0x04004619 RID: 17945
		public const short DuneSplicerHead = 510;

		// Token: 0x0400461A RID: 17946
		public const short DuneSplicerBody = 511;

		// Token: 0x0400461B RID: 17947
		public const short DuneSplicerTail = 512;

		// Token: 0x0400461C RID: 17948
		public const short TombCrawlerHead = 513;

		// Token: 0x0400461D RID: 17949
		public const short TombCrawlerBody = 514;

		// Token: 0x0400461E RID: 17950
		public const short TombCrawlerTail = 515;

		// Token: 0x0400461F RID: 17951
		public const short SolarFlare = 516;

		// Token: 0x04004620 RID: 17952
		public const short LunarTowerSolar = 517;

		// Token: 0x04004621 RID: 17953
		public const short SolarSpearman = 518;

		// Token: 0x04004622 RID: 17954
		public const short SolarGoop = 519;

		// Token: 0x04004623 RID: 17955
		public const short MartianWalker = 520;

		// Token: 0x04004624 RID: 17956
		public const short AncientCultistSquidhead = 521;

		// Token: 0x04004625 RID: 17957
		public const short AncientLight = 522;

		// Token: 0x04004626 RID: 17958
		public const short AncientDoom = 523;

		// Token: 0x04004627 RID: 17959
		public const short DesertGhoul = 524;

		// Token: 0x04004628 RID: 17960
		public const short DesertGhoulCorruption = 525;

		// Token: 0x04004629 RID: 17961
		public const short DesertGhoulCrimson = 526;

		// Token: 0x0400462A RID: 17962
		public const short DesertGhoulHallow = 527;

		// Token: 0x0400462B RID: 17963
		public const short DesertLamiaLight = 528;

		// Token: 0x0400462C RID: 17964
		public const short DesertLamiaDark = 529;

		// Token: 0x0400462D RID: 17965
		public const short DesertScorpionWalk = 530;

		// Token: 0x0400462E RID: 17966
		public const short DesertScorpionWall = 531;

		// Token: 0x0400462F RID: 17967
		public const short DesertBeast = 532;

		// Token: 0x04004630 RID: 17968
		public const short DesertDjinn = 533;

		// Token: 0x04004631 RID: 17969
		public const short DemonTaxCollector = 534;

		// Token: 0x04004632 RID: 17970
		public const short SlimeSpiked = 535;

		// Token: 0x04004633 RID: 17971
		public const short TheBride = 536;

		// Token: 0x04004634 RID: 17972
		public const short SandSlime = 537;

		// Token: 0x04004635 RID: 17973
		public const short SquirrelRed = 538;

		// Token: 0x04004636 RID: 17974
		public const short SquirrelGold = 539;

		// Token: 0x04004637 RID: 17975
		public const short PartyBunny = 540;

		// Token: 0x04004638 RID: 17976
		public const short SandElemental = 541;

		// Token: 0x04004639 RID: 17977
		public const short SandShark = 542;

		// Token: 0x0400463A RID: 17978
		public const short SandsharkCorrupt = 543;

		// Token: 0x0400463B RID: 17979
		public const short SandsharkCrimson = 544;

		// Token: 0x0400463C RID: 17980
		public const short SandsharkHallow = 545;

		// Token: 0x0400463D RID: 17981
		public const short Tumbleweed = 546;

		// Token: 0x0400463E RID: 17982
		public const short DD2AttackerTest = 547;

		// Token: 0x0400463F RID: 17983
		public const short DD2EterniaCrystal = 548;

		// Token: 0x04004640 RID: 17984
		public const short DD2LanePortal = 549;

		// Token: 0x04004641 RID: 17985
		public const short DD2Bartender = 550;

		// Token: 0x04004642 RID: 17986
		public const short DD2Betsy = 551;

		// Token: 0x04004643 RID: 17987
		public const short DD2GoblinT1 = 552;

		// Token: 0x04004644 RID: 17988
		public const short DD2GoblinT2 = 553;

		// Token: 0x04004645 RID: 17989
		public const short DD2GoblinT3 = 554;

		// Token: 0x04004646 RID: 17990
		public const short DD2GoblinBomberT1 = 555;

		// Token: 0x04004647 RID: 17991
		public const short DD2GoblinBomberT2 = 556;

		// Token: 0x04004648 RID: 17992
		public const short DD2GoblinBomberT3 = 557;

		// Token: 0x04004649 RID: 17993
		public const short DD2WyvernT1 = 558;

		// Token: 0x0400464A RID: 17994
		public const short DD2WyvernT2 = 559;

		// Token: 0x0400464B RID: 17995
		public const short DD2WyvernT3 = 560;

		// Token: 0x0400464C RID: 17996
		public const short DD2JavelinstT1 = 561;

		// Token: 0x0400464D RID: 17997
		public const short DD2JavelinstT2 = 562;

		// Token: 0x0400464E RID: 17998
		public const short DD2JavelinstT3 = 563;

		// Token: 0x0400464F RID: 17999
		public const short DD2DarkMageT1 = 564;

		// Token: 0x04004650 RID: 18000
		public const short DD2DarkMageT3 = 565;

		// Token: 0x04004651 RID: 18001
		public const short DD2SkeletonT1 = 566;

		// Token: 0x04004652 RID: 18002
		public const short DD2SkeletonT3 = 567;

		// Token: 0x04004653 RID: 18003
		public const short DD2WitherBeastT2 = 568;

		// Token: 0x04004654 RID: 18004
		public const short DD2WitherBeastT3 = 569;

		// Token: 0x04004655 RID: 18005
		public const short DD2DrakinT2 = 570;

		// Token: 0x04004656 RID: 18006
		public const short DD2DrakinT3 = 571;

		// Token: 0x04004657 RID: 18007
		public const short DD2KoboldWalkerT2 = 572;

		// Token: 0x04004658 RID: 18008
		public const short DD2KoboldWalkerT3 = 573;

		// Token: 0x04004659 RID: 18009
		public const short DD2KoboldFlyerT2 = 574;

		// Token: 0x0400465A RID: 18010
		public const short DD2KoboldFlyerT3 = 575;

		// Token: 0x0400465B RID: 18011
		public const short DD2OgreT2 = 576;

		// Token: 0x0400465C RID: 18012
		public const short DD2OgreT3 = 577;

		// Token: 0x0400465D RID: 18013
		public const short DD2LightningBugT3 = 578;

		// Token: 0x0400465E RID: 18014
		public const short BartenderUnconscious = 579;

		// Token: 0x0400465F RID: 18015
		public const short WalkingAntlion = 580;

		// Token: 0x04004660 RID: 18016
		public const short FlyingAntlion = 581;

		// Token: 0x04004661 RID: 18017
		public const short LarvaeAntlion = 582;

		// Token: 0x04004662 RID: 18018
		public const short FairyCritterPink = 583;

		// Token: 0x04004663 RID: 18019
		public const short FairyCritterGreen = 584;

		// Token: 0x04004664 RID: 18020
		public const short FairyCritterBlue = 585;

		// Token: 0x04004665 RID: 18021
		public const short ZombieMerman = 586;

		// Token: 0x04004666 RID: 18022
		public const short EyeballFlyingFish = 587;

		// Token: 0x04004667 RID: 18023
		public const short Golfer = 588;

		// Token: 0x04004668 RID: 18024
		public const short GolferRescue = 589;

		// Token: 0x04004669 RID: 18025
		public const short TorchZombie = 590;

		// Token: 0x0400466A RID: 18026
		public const short ArmedTorchZombie = 591;

		// Token: 0x0400466B RID: 18027
		public const short GoldGoldfish = 592;

		// Token: 0x0400466C RID: 18028
		public const short GoldGoldfishWalker = 593;

		// Token: 0x0400466D RID: 18029
		public const short WindyBalloon = 594;

		// Token: 0x0400466E RID: 18030
		public const short BlackDragonfly = 595;

		// Token: 0x0400466F RID: 18031
		public const short BlueDragonfly = 596;

		// Token: 0x04004670 RID: 18032
		public const short GreenDragonfly = 597;

		// Token: 0x04004671 RID: 18033
		public const short OrangeDragonfly = 598;

		// Token: 0x04004672 RID: 18034
		public const short RedDragonfly = 599;

		// Token: 0x04004673 RID: 18035
		public const short YellowDragonfly = 600;

		// Token: 0x04004674 RID: 18036
		public const short GoldDragonfly = 601;

		// Token: 0x04004675 RID: 18037
		public const short Seagull = 602;

		// Token: 0x04004676 RID: 18038
		public const short Seagull2 = 603;

		// Token: 0x04004677 RID: 18039
		public const short LadyBug = 604;

		// Token: 0x04004678 RID: 18040
		public const short GoldLadyBug = 605;

		// Token: 0x04004679 RID: 18041
		public const short Maggot = 606;

		// Token: 0x0400467A RID: 18042
		public const short Pupfish = 607;

		// Token: 0x0400467B RID: 18043
		public const short Grebe = 608;

		// Token: 0x0400467C RID: 18044
		public const short Grebe2 = 609;

		// Token: 0x0400467D RID: 18045
		public const short Rat = 610;

		// Token: 0x0400467E RID: 18046
		public const short Owl = 611;

		// Token: 0x0400467F RID: 18047
		public const short WaterStrider = 612;

		// Token: 0x04004680 RID: 18048
		public const short GoldWaterStrider = 613;

		// Token: 0x04004681 RID: 18049
		public const short ExplosiveBunny = 614;

		// Token: 0x04004682 RID: 18050
		public const short Dolphin = 615;

		// Token: 0x04004683 RID: 18051
		public const short Turtle = 616;

		// Token: 0x04004684 RID: 18052
		public const short TurtleJungle = 617;

		// Token: 0x04004685 RID: 18053
		public const short BloodNautilus = 618;

		// Token: 0x04004686 RID: 18054
		public const short BloodSquid = 619;

		// Token: 0x04004687 RID: 18055
		public const short GoblinShark = 620;

		// Token: 0x04004688 RID: 18056
		public const short BloodEelHead = 621;

		// Token: 0x04004689 RID: 18057
		public const short BloodEelBody = 622;

		// Token: 0x0400468A RID: 18058
		public const short BloodEelTail = 623;

		// Token: 0x0400468B RID: 18059
		public const short Gnome = 624;

		// Token: 0x0400468C RID: 18060
		public const short SeaTurtle = 625;

		// Token: 0x0400468D RID: 18061
		public const short Seahorse = 626;

		// Token: 0x0400468E RID: 18062
		public const short GoldSeahorse = 627;

		// Token: 0x0400468F RID: 18063
		public const short Dandelion = 628;

		// Token: 0x04004690 RID: 18064
		public const short IceMimic = 629;

		// Token: 0x04004691 RID: 18065
		public const short BloodMummy = 630;

		// Token: 0x04004692 RID: 18066
		public const short RockGolem = 631;

		// Token: 0x04004693 RID: 18067
		public const short MaggotZombie = 632;

		// Token: 0x04004694 RID: 18068
		public const short BestiaryGirl = 633;

		// Token: 0x04004695 RID: 18069
		public const short SporeBat = 634;

		// Token: 0x04004696 RID: 18070
		public const short SporeSkeleton = 635;

		// Token: 0x04004697 RID: 18071
		public const short HallowBoss = 636;

		// Token: 0x04004698 RID: 18072
		public const short TownCat = 637;

		// Token: 0x04004699 RID: 18073
		public const short TownDog = 638;

		// Token: 0x0400469A RID: 18074
		public const short GemSquirrelAmethyst = 639;

		// Token: 0x0400469B RID: 18075
		public const short GemSquirrelTopaz = 640;

		// Token: 0x0400469C RID: 18076
		public const short GemSquirrelSapphire = 641;

		// Token: 0x0400469D RID: 18077
		public const short GemSquirrelEmerald = 642;

		// Token: 0x0400469E RID: 18078
		public const short GemSquirrelRuby = 643;

		// Token: 0x0400469F RID: 18079
		public const short GemSquirrelDiamond = 644;

		// Token: 0x040046A0 RID: 18080
		public const short GemSquirrelAmber = 645;

		// Token: 0x040046A1 RID: 18081
		public const short GemBunnyAmethyst = 646;

		// Token: 0x040046A2 RID: 18082
		public const short GemBunnyTopaz = 647;

		// Token: 0x040046A3 RID: 18083
		public const short GemBunnySapphire = 648;

		// Token: 0x040046A4 RID: 18084
		public const short GemBunnyEmerald = 649;

		// Token: 0x040046A5 RID: 18085
		public const short GemBunnyRuby = 650;

		// Token: 0x040046A6 RID: 18086
		public const short GemBunnyDiamond = 651;

		// Token: 0x040046A7 RID: 18087
		public const short GemBunnyAmber = 652;

		// Token: 0x040046A8 RID: 18088
		public const short HellButterfly = 653;

		// Token: 0x040046A9 RID: 18089
		public const short Lavafly = 654;

		// Token: 0x040046AA RID: 18090
		public const short MagmaSnail = 655;

		// Token: 0x040046AB RID: 18091
		public const short TownBunny = 656;

		// Token: 0x040046AC RID: 18092
		public const short QueenSlimeBoss = 657;

		// Token: 0x040046AD RID: 18093
		public const short QueenSlimeMinionBlue = 658;

		// Token: 0x040046AE RID: 18094
		public const short QueenSlimeMinionPink = 659;

		// Token: 0x040046AF RID: 18095
		public const short QueenSlimeMinionPurple = 660;

		// Token: 0x040046B0 RID: 18096
		public const short EmpressButterfly = 661;

		// Token: 0x040046B1 RID: 18097
		public const short PirateGhost = 662;

		// Token: 0x040046B2 RID: 18098
		public const short Princess = 663;

		// Token: 0x040046B3 RID: 18099
		public const short TorchGod = 664;

		// Token: 0x040046B4 RID: 18100
		public const short ChaosBallTim = 665;

		// Token: 0x040046B5 RID: 18101
		public const short VileSpitEaterOfWorlds = 666;

		// Token: 0x040046B6 RID: 18102
		public const short GoldenSlime = 667;

		// Token: 0x040046B7 RID: 18103
		public const short Deerclops = 668;

		// Token: 0x040046B8 RID: 18104
		public const short Stinkbug = 669;

		// Token: 0x040046B9 RID: 18105
		public const short TownSlimeBlue = 670;

		// Token: 0x040046BA RID: 18106
		public const short ScarletMacaw = 671;

		// Token: 0x040046BB RID: 18107
		public const short BlueMacaw = 672;

		// Token: 0x040046BC RID: 18108
		public const short Toucan = 673;

		// Token: 0x040046BD RID: 18109
		public const short YellowCockatiel = 674;

		// Token: 0x040046BE RID: 18110
		public const short GrayCockatiel = 675;

		// Token: 0x040046BF RID: 18111
		public const short ShimmerSlime = 676;

		// Token: 0x040046C0 RID: 18112
		public const short Shimmerfly = 677;

		// Token: 0x040046C1 RID: 18113
		public const short TownSlimeGreen = 678;

		// Token: 0x040046C2 RID: 18114
		public const short TownSlimeOld = 679;

		// Token: 0x040046C3 RID: 18115
		public const short TownSlimePurple = 680;

		// Token: 0x040046C4 RID: 18116
		public const short TownSlimeRainbow = 681;

		// Token: 0x040046C5 RID: 18117
		public const short TownSlimeRed = 682;

		// Token: 0x040046C6 RID: 18118
		public const short TownSlimeYellow = 683;

		// Token: 0x040046C7 RID: 18119
		public const short TownSlimeCopper = 684;

		// Token: 0x040046C8 RID: 18120
		public const short BoundTownSlimeOld = 685;

		// Token: 0x040046C9 RID: 18121
		public const short BoundTownSlimePurple = 686;

		// Token: 0x040046CA RID: 18122
		public const short BoundTownSlimeYellow = 687;

		// Token: 0x040046CB RID: 18123
		public const short Pufferfish = 688;

		// Token: 0x040046CC RID: 18124
		public const short OwlMimic = 689;

		// Token: 0x040046CD RID: 18125
		public const short StatueMimic = 690;

		// Token: 0x040046CE RID: 18126
		public const short MossZombie = 691;

		// Token: 0x040046CF RID: 18127
		public const short Orca = 692;

		// Token: 0x040046D0 RID: 18128
		public const short LibrarianSkeleton = 693;

		// Token: 0x040046D1 RID: 18129
		public const short WaterBoltMimic = 694;

		// Token: 0x040046D2 RID: 18130
		public const short PalworldCattivaDistressed = 695;

		// Token: 0x040046D3 RID: 18131
		public const short PalworldFoxsparksDistressed = 696;

		// Token: 0x040046D4 RID: 18132
		public static readonly short Count = 697;

		// Token: 0x040046D5 RID: 18133
		public static readonly IdDictionary Search = IdDictionary.Create<NPCID, short>();

		// Token: 0x02000788 RID: 1928
		public static class Sets
		{
			// Token: 0x0600414E RID: 16718 RVA: 0x006A8A03 File Offset: 0x006A6C03
			public static NPCID.Sets.NPCPortraitSelector PrioritizedPortrait()
			{
				return new NPCID.Sets.NPCPortraitSelector();
			}

			// Token: 0x0600414F RID: 16719 RVA: 0x006A8A0A File Offset: 0x006A6C0A
			public static NPCID.Sets.BasicNPCPortrait BasicPortrait(string texturePath)
			{
				return new NPCID.Sets.BasicNPCPortrait(texturePath);
			}

			// Token: 0x06004150 RID: 16720 RVA: 0x006A8A12 File Offset: 0x006A6C12
			public static NPCID.Sets.NPCPortraitSelector.SelectionCondition VariantPortraitCondition(int variantIndex)
			{
				return new NPCID.Sets.NPCPortraitSelector.SelectionCondition(new NPCID.Sets.NPCVariantChecker(variantIndex).Fits);
			}

			// Token: 0x06004151 RID: 16721 RVA: 0x006A8A28 File Offset: 0x006A6C28
			public static bool ShimmeredPortraitCondition()
			{
				int talkNPC = Main.LocalPlayer.talkNPC;
				return talkNPC >= 0 && talkNPC < Main.maxNPCs && Main.npc[talkNPC].IsShimmerVariant;
			}

			// Token: 0x06004152 RID: 16722 RVA: 0x006A8A5C File Offset: 0x006A6C5C
			public static Dictionary<int, NPCID.Sets.NPCBestiaryDrawModifiers> NPCBestiaryDrawOffsetCreation()
			{
				Dictionary<int, NPCID.Sets.NPCBestiaryDrawModifiers> redigitEntries = NPCID.Sets.GetRedigitEntries();
				Dictionary<int, NPCID.Sets.NPCBestiaryDrawModifiers> leinforsEntries = NPCID.Sets.GetLeinforsEntries();
				Dictionary<int, NPCID.Sets.NPCBestiaryDrawModifiers> groxEntries = NPCID.Sets.GetGroxEntries();
				Dictionary<int, NPCID.Sets.NPCBestiaryDrawModifiers> dictionary = new Dictionary<int, NPCID.Sets.NPCBestiaryDrawModifiers>();
				foreach (KeyValuePair<int, NPCID.Sets.NPCBestiaryDrawModifiers> keyValuePair in groxEntries)
				{
					dictionary[keyValuePair.Key] = keyValuePair.Value;
				}
				foreach (KeyValuePair<int, NPCID.Sets.NPCBestiaryDrawModifiers> keyValuePair2 in leinforsEntries)
				{
					dictionary[keyValuePair2.Key] = keyValuePair2.Value;
				}
				foreach (KeyValuePair<int, NPCID.Sets.NPCBestiaryDrawModifiers> keyValuePair3 in redigitEntries)
				{
					dictionary[keyValuePair3.Key] = keyValuePair3.Value;
				}
				return dictionary;
			}

			// Token: 0x06004153 RID: 16723 RVA: 0x006A8B68 File Offset: 0x006A6D68
			private static Dictionary<int, NPCID.Sets.NPCBestiaryDrawModifiers> GetRedigitEntries()
			{
				Dictionary<int, NPCID.Sets.NPCBestiaryDrawModifiers> dictionary = new Dictionary<int, NPCID.Sets.NPCBestiaryDrawModifiers>();
				int key = 691;
				NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					PortraitPositionXOverride = new float?((float)2)
				};
				dictionary.Add(key, value);
				int key2 = 430;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key2, value);
				int key3 = 431;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key3, value);
				int key4 = 432;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key4, value);
				int key5 = 433;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key5, value);
				int key6 = 434;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key6, value);
				int key7 = 435;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key7, value);
				int key8 = 436;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key8, value);
				int key9 = 591;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key9, value);
				int key10 = 449;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key10, value);
				int key11 = 450;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key11, value);
				int key12 = 451;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key12, value);
				int key13 = 452;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key13, value);
				int key14 = 595;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key14, value);
				int key15 = 596;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key15, value);
				int key16 = 597;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key16, value);
				int key17 = 598;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key17, value);
				int key18 = 600;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key18, value);
				int key19 = 495;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key19, value);
				int key20 = 497;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key20, value);
				int key21 = 498;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key21, value);
				int key22 = 500;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key22, value);
				int key23 = 501;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key23, value);
				int key24 = 502;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key24, value);
				int key25 = 503;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key25, value);
				int key26 = 504;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key26, value);
				int key27 = 505;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key27, value);
				int key28 = 506;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key28, value);
				int key29 = 230;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key29, value);
				int key30 = 593;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key30, value);
				int key31 = 158;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key31, value);
				int key32 = -2;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key32, value);
				int key33 = 440;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key33, value);
				int key34 = 568;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key34, value);
				int key35 = 566;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key35, value);
				int key36 = 576;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key36, value);
				int key37 = 558;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key37, value);
				int key38 = 559;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key38, value);
				int key39 = 552;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key39, value);
				int key40 = 553;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key40, value);
				int key41 = 564;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key41, value);
				int key42 = 570;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key42, value);
				int key43 = 555;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key43, value);
				int key44 = 556;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key44, value);
				int key45 = 574;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key45, value);
				int key46 = 561;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key46, value);
				int key47 = 562;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key47, value);
				int key48 = 572;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key48, value);
				int key49 = 689;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key49, value);
				int key50 = 535;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key50, value);
				return dictionary;
			}

			// Token: 0x06004154 RID: 16724 RVA: 0x006A9104 File Offset: 0x006A7304
			private static Dictionary<int, NPCID.Sets.NPCBestiaryDrawModifiers> GetGroxEntries()
			{
				return new Dictionary<int, NPCID.Sets.NPCBestiaryDrawModifiers>
				{
					{
						692,
						new NPCID.Sets.NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(35f, 4f),
							Velocity = 1f,
							PortraitPositionXOverride = new float?((float)5),
							IsWet = true,
							SpriteDirection = new int?(1)
						}
					}
				};
			}

			// Token: 0x06004155 RID: 16725 RVA: 0x006A9170 File Offset: 0x006A7370
			private static Dictionary<int, NPCID.Sets.NPCBestiaryDrawModifiers> GetLeinforsEntries()
			{
				Dictionary<int, NPCID.Sets.NPCBestiaryDrawModifiers> dictionary = new Dictionary<int, NPCID.Sets.NPCBestiaryDrawModifiers>();
				int key = -65;
				NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key, value);
				int key2 = -64;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key2, value);
				int key3 = -63;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key3, value);
				int key4 = -62;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key4, value);
				int key5 = -61;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(2f, 4f),
					Scale = 1.1f,
					Hide = true
				};
				dictionary.Add(key5, value);
				int key6 = -60;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(1f, 3f),
					Scale = 0.9f,
					Hide = true
				};
				dictionary.Add(key6, value);
				int key7 = -59;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key7, value);
				int key8 = -58;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key8, value);
				int key9 = -57;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key9, value);
				int key10 = -56;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key10, value);
				int key11 = -55;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true,
					Velocity = 1f,
					Scale = 1.1f
				};
				dictionary.Add(key11, value);
				int key12 = -54;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true,
					Velocity = 1f,
					Scale = 0.9f
				};
				dictionary.Add(key12, value);
				int key13 = -53;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					Scale = 1.1f,
					Hide = true
				};
				dictionary.Add(key13, value);
				int key14 = -52;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					Scale = 0.9f,
					Hide = true
				};
				dictionary.Add(key14, value);
				int key15 = -51;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					Scale = 1.1f,
					Hide = true
				};
				dictionary.Add(key15, value);
				int key16 = -50;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					Scale = 0.9f,
					Hide = true
				};
				dictionary.Add(key16, value);
				int key17 = -49;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					Scale = 1.1f,
					Hide = true
				};
				dictionary.Add(key17, value);
				int key18 = -48;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					Scale = 0.9f,
					Hide = true
				};
				dictionary.Add(key18, value);
				int key19 = -47;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					Scale = 1.1f,
					Hide = true
				};
				dictionary.Add(key19, value);
				int key20 = -46;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					Scale = 0.9f,
					Hide = true
				};
				dictionary.Add(key20, value);
				int key21 = -45;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					Scale = 1.1f,
					Hide = true
				};
				dictionary.Add(key21, value);
				int key22 = -44;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					Scale = 0.9f,
					Hide = true
				};
				dictionary.Add(key22, value);
				int key23 = -43;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(5f, -15f),
					Hide = true
				};
				dictionary.Add(key23, value);
				int key24 = -42;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(5f, -15f),
					Hide = true
				};
				dictionary.Add(key24, value);
				int key25 = -41;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(5f, -15f),
					Hide = true
				};
				dictionary.Add(key25, value);
				int key26 = -40;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(5f, -15f),
					Hide = true
				};
				dictionary.Add(key26, value);
				int key27 = -39;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(5f, -15f),
					Hide = true
				};
				dictionary.Add(key27, value);
				int key28 = -38;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(5f, -15f),
					Hide = true
				};
				dictionary.Add(key28, value);
				int key29 = -37;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					Scale = 1.1f,
					Hide = true
				};
				dictionary.Add(key29, value);
				int key30 = -36;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					Scale = 0.9f,
					Hide = true
				};
				dictionary.Add(key30, value);
				int key31 = -35;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					Scale = 1.1f,
					Hide = true
				};
				dictionary.Add(key31, value);
				int key32 = -34;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					Scale = 0.9f,
					Hide = true
				};
				dictionary.Add(key32, value);
				int key33 = -33;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					Scale = 1.1f,
					Hide = true
				};
				dictionary.Add(key33, value);
				int key34 = -32;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					Scale = 0.9f,
					Hide = true
				};
				dictionary.Add(key34, value);
				int key35 = -31;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					Scale = 1.1f,
					Hide = true
				};
				dictionary.Add(key35, value);
				int key36 = -30;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					Scale = 0.9f,
					Hide = true
				};
				dictionary.Add(key36, value);
				int key37 = -29;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					Scale = 1.1f,
					Hide = true
				};
				dictionary.Add(key37, value);
				int key38 = -28;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					Scale = 0.9f,
					Hide = true
				};
				dictionary.Add(key38, value);
				int key39 = -27;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					Scale = 1.1f,
					Hide = true
				};
				dictionary.Add(key39, value);
				int key40 = -26;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					Scale = 0.9f,
					Hide = true
				};
				dictionary.Add(key40, value);
				int key41 = -23;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(5f, -9f),
					Rotation = 0.75f,
					Scale = 1.2f,
					Hide = true
				};
				dictionary.Add(key41, value);
				int key42 = -22;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(5f, -9f),
					Rotation = 0.75f,
					Scale = 0.8f,
					Hide = true
				};
				dictionary.Add(key42, value);
				int key43 = -25;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key43, value);
				int key44 = -24;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key44, value);
				int key45 = -21;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(2f, 5f),
					Scale = 1.2f,
					Hide = true
				};
				dictionary.Add(key45, value);
				int key46 = -20;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(2f, 4f),
					Scale = 1.1f,
					Hide = true
				};
				dictionary.Add(key46, value);
				int key47 = -19;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(1f, 3f),
					Scale = 0.9f,
					Hide = true
				};
				dictionary.Add(key47, value);
				int key48 = -18;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 2f),
					Scale = 0.8f,
					Hide = true
				};
				dictionary.Add(key48, value);
				int key49 = -17;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 3f),
					Scale = 1.2f,
					Hide = true
				};
				dictionary.Add(key49, value);
				int key50 = -16;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(2f, 3f),
					Scale = 0.8f,
					Hide = true
				};
				dictionary.Add(key50, value);
				int key51 = -15;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					Scale = 1.2f,
					Hide = true
				};
				dictionary.Add(key51, value);
				int key52 = -14;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					Scale = 1.1f,
					Hide = true
				};
				dictionary.Add(key52, value);
				int key53 = -13;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					Scale = 0.9f,
					Hide = true
				};
				dictionary.Add(key53, value);
				int key54 = -12;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(5f, -9f),
					Rotation = 0.75f,
					Scale = 1.2f,
					Hide = true
				};
				dictionary.Add(key54, value);
				int key55 = -11;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(5f, -9f),
					Rotation = 0.75f,
					Scale = 0.8f,
					Hide = true
				};
				dictionary.Add(key55, value);
				int key56 = 2;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(6f, -15f),
					PortraitPositionYOverride = new float?((float)-35)
				};
				dictionary.Add(key56, value);
				int key57 = 3;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					PortraitPositionXOverride = new float?((float)2)
				};
				dictionary.Add(key57, value);
				int key58 = 4;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(25f, -30f),
					Rotation = 0.7f,
					Frame = new int?(4)
				};
				dictionary.Add(key58, value);
				int key59 = 5;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(3f, 4f),
					Rotation = 1.5f
				};
				dictionary.Add(key59, value);
				int key60 = 6;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(5f, -9f),
					Rotation = 0.75f
				};
				dictionary.Add(key60, value);
				int key61 = 7;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_7",
					Position = new Vector2(20f, 29f),
					PortraitPositionXOverride = new float?(0f),
					PortraitPositionYOverride = new float?((float)10)
				};
				dictionary.Add(key61, value);
				int key62 = 8;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key62, value);
				int key63 = 9;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key63, value);
				int key64 = 10;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_10",
					Position = new Vector2(2f, 24f),
					PortraitPositionXOverride = new float?(0f),
					PortraitPositionYOverride = new float?((float)10)
				};
				dictionary.Add(key64, value);
				int key65 = 11;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key65, value);
				int key66 = 12;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key66, value);
				int key67 = 13;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_13",
					Position = new Vector2(40f, 22f),
					PortraitPositionXOverride = new float?(0f),
					PortraitPositionYOverride = new float?((float)10)
				};
				dictionary.Add(key67, value);
				int key68 = 14;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key68, value);
				int key69 = 15;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key69, value);
				int key70 = 17;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key70, value);
				int key71 = 18;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					PortraitPositionXOverride = new float?((float)2)
				};
				dictionary.Add(key71, value);
				int key72 = 19;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key72, value);
				int key73 = 20;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					PortraitPositionXOverride = new float?((float)2)
				};
				dictionary.Add(key73, value);
				int key74 = 22;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key74, value);
				int key75 = 25;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key75, value);
				int key76 = 26;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key76, value);
				int key77 = 27;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key77, value);
				int key78 = 28;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key78, value);
				int key79 = 30;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key79, value);
				int key80 = 665;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key80, value);
				int key81 = 31;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key81, value);
				int key82 = 33;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key82, value);
				int key83 = 34;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Direction = new int?(1)
				};
				dictionary.Add(key83, value);
				int key84 = 21;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key84, value);
				int key85 = 35;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, -12f),
					Scale = 0.9f,
					PortraitPositionXOverride = new float?((float)-1),
					PortraitPositionYOverride = new float?((float)-3)
				};
				dictionary.Add(key85, value);
				int key86 = 36;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key86, value);
				int key87 = 38;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key87, value);
				int key88 = 37;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key88, value);
				int key89 = 39;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_39",
					Position = new Vector2(40f, 23f),
					PortraitPositionXOverride = new float?(0f),
					PortraitPositionYOverride = new float?((float)10)
				};
				dictionary.Add(key89, value);
				int key90 = 40;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key90, value);
				int key91 = 41;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key91, value);
				int key92 = 43;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(4f, -6f),
					Rotation = 2.3561945f
				};
				dictionary.Add(key92, value);
				int key93 = 44;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key93, value);
				int key94 = 46;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key94, value);
				int key95 = 47;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key95, value);
				int key96 = 48;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(2f, -14f),
					PortraitPositionYOverride = new float?((float)-30)
				};
				dictionary.Add(key96, value);
				int key97 = 49;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, -13f),
					PortraitPositionYOverride = new float?((float)-30)
				};
				dictionary.Add(key97, value);
				int key98 = 50;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 90f),
					PortraitScale = new float?(1.1f),
					PortraitPositionYOverride = new float?((float)70)
				};
				dictionary.Add(key98, value);
				int key99 = 51;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, -13f),
					PortraitPositionYOverride = new float?((float)-30)
				};
				dictionary.Add(key99, value);
				int key100 = 52;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key100, value);
				int key101 = 53;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key101, value);
				int key102 = 54;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key102, value);
				int key103 = 55;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 6f),
					PortraitPositionYOverride = new float?((float)7),
					IsWet = true
				};
				dictionary.Add(key103, value);
				int key104 = 56;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(4f, -6f),
					Rotation = 2.3561945f
				};
				dictionary.Add(key104, value);
				int key105 = 57;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 6f),
					PortraitPositionYOverride = new float?((float)6),
					IsWet = true
				};
				dictionary.Add(key105, value);
				int key106 = 58;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 6f),
					PortraitPositionYOverride = new float?((float)6),
					IsWet = true
				};
				dictionary.Add(key106, value);
				int key107 = 60;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, -19f),
					PortraitPositionYOverride = new float?((float)-36)
				};
				dictionary.Add(key107, value);
				int key108 = 61;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 0.05f,
					PortraitPositionYOverride = new float?((float)-15)
				};
				dictionary.Add(key108, value);
				int key109 = 62;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, -10f),
					PortraitPositionYOverride = new float?((float)-25)
				};
				dictionary.Add(key109, value);
				int key110 = 65;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(35f, 4f),
					Velocity = 1f,
					PortraitPositionXOverride = new float?((float)5),
					IsWet = true
				};
				dictionary.Add(key110, value);
				int key111 = 66;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, -6f),
					Scale = 0.9f,
					PortraitPositionYOverride = new float?((float)-15)
				};
				dictionary.Add(key111, value);
				int key112 = 68;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(-1f, -12f),
					Scale = 0.9f,
					PortraitPositionYOverride = new float?((float)-3)
				};
				dictionary.Add(key112, value);
				int key113 = 70;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key113, value);
				int key114 = 72;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key114, value);
				int key115 = 73;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key115, value);
				int key116 = 74;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(1f, -14f),
					Velocity = 0.05f,
					PortraitPositionYOverride = new float?((float)-30)
				};
				dictionary.Add(key116, value);
				int key117 = 75;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 6f)
				};
				dictionary.Add(key117, value);
				int key118 = 76;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key118, value);
				int key119 = 77;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key119, value);
				int key120 = 78;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 0.5f
				};
				dictionary.Add(key120, value);
				int key121 = 79;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 0.5f
				};
				dictionary.Add(key121, value);
				int key122 = 80;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 0.5f
				};
				dictionary.Add(key122, value);
				int key123 = 83;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(-4f, -4f),
					Scale = 0.9f,
					PortraitPositionYOverride = new float?((float)-25)
				};
				dictionary.Add(key123, value);
				int key124 = 84;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(6f, -11f),
					PortraitPositionXOverride = new float?(0f),
					PortraitPositionYOverride = new float?((float)-28)
				};
				dictionary.Add(key124, value);
				int key125 = 86;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(20f, 6f),
					Velocity = 1f,
					PortraitPositionXOverride = new float?(0f),
					PortraitPositionYOverride = new float?((float)2)
				};
				dictionary.Add(key125, value);
				int key126 = 87;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_87",
					Position = new Vector2(55f, 15f),
					PortraitPositionXOverride = new float?((float)4),
					PortraitPositionYOverride = new float?((float)10)
				};
				dictionary.Add(key126, value);
				int key127 = 88;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key127, value);
				int key128 = 89;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key128, value);
				int key129 = 90;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key129, value);
				int key130 = 91;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key130, value);
				int key131 = 92;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key131, value);
				int key132 = 93;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(4f, -11f),
					PortraitPositionYOverride = new float?((float)-30)
				};
				dictionary.Add(key132, value);
				int key133 = 94;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(8f, 0f),
					Rotation = 0.75f
				};
				dictionary.Add(key133, value);
				int key134 = 95;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_95",
					Position = new Vector2(20f, 28f),
					PortraitPositionXOverride = new float?(0f),
					PortraitPositionYOverride = new float?((float)10)
				};
				dictionary.Add(key134, value);
				int key135 = 96;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key135, value);
				int key136 = 97;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key136, value);
				int key137 = 98;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_98",
					Position = new Vector2(40f, 24f),
					PortraitPositionXOverride = new float?(0f),
					PortraitPositionYOverride = new float?((float)12)
				};
				dictionary.Add(key137, value);
				int key138 = 99;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key138, value);
				int key139 = 100;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key139, value);
				int key140 = 101;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(-2f, 6f),
					Rotation = 2.3561945f
				};
				dictionary.Add(key140, value);
				int key141 = 102;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 6f),
					PortraitPositionYOverride = new float?((float)6),
					IsWet = true
				};
				dictionary.Add(key141, value);
				int key142 = 104;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key142, value);
				int key143 = 105;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key143, value);
				int key144 = 106;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key144, value);
				int key145 = 107;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key145, value);
				int key146 = 108;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key146, value);
				int key147 = 109;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 35f),
					Velocity = 1f,
					PortraitPositionYOverride = new float?(0f)
				};
				dictionary.Add(key147, value);
				int key148 = 110;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key148, value);
				int key149 = 111;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key149, value);
				int key150 = 112;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key150, value);
				int key151 = 666;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key151, value);
				int key152 = 113;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_113",
					Position = new Vector2(56f, 5f),
					PortraitPositionXOverride = new float?((float)10),
					PortraitPositionYOverride = new float?(0f)
				};
				dictionary.Add(key152, value);
				int key153 = 114;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key153, value);
				int key154 = 115;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_115",
					Position = new Vector2(56f, 3f),
					PortraitPositionXOverride = new float?((float)55),
					PortraitPositionYOverride = new float?(0f)
				};
				dictionary.Add(key154, value);
				int key155 = 116;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(3f, -5f),
					PortraitPositionXOverride = new float?((float)4),
					PortraitPositionYOverride = new float?((float)-26)
				};
				dictionary.Add(key155, value);
				int key156 = 117;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_117",
					Position = new Vector2(10f, 20f),
					PortraitPositionXOverride = new float?(0f),
					PortraitPositionYOverride = new float?(0f)
				};
				dictionary.Add(key156, value);
				int key157 = 118;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key157, value);
				int key158 = 119;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key158, value);
				int key159 = 120;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key159, value);
				int key160 = 123;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key160, value);
				int key161 = 124;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					PortraitPositionXOverride = new float?((float)2)
				};
				dictionary.Add(key161, value);
				int key162 = 121;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(-2f, -4f),
					PortraitPositionYOverride = new float?((float)-20)
				};
				dictionary.Add(key162, value);
				int key163 = 122;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 4f)
				};
				dictionary.Add(key163, value);
				int key164 = 125;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(-28f, -23f),
					Rotation = -0.75f
				};
				dictionary.Add(key164, value);
				int key165 = 126;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(28f, 30f),
					Rotation = 2.25f
				};
				dictionary.Add(key165, value);
				int key166 = 127;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_127",
					Position = new Vector2(0f, 0f),
					PortraitPositionXOverride = new float?(0f),
					PortraitPositionYOverride = new float?((float)1)
				};
				dictionary.Add(key166, value);
				int key167 = 128;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(-6f, -2f),
					Rotation = -0.75f,
					Hide = true
				};
				dictionary.Add(key167, value);
				int key168 = 129;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(4f, 4f),
					Rotation = 0.75f,
					Hide = true
				};
				dictionary.Add(key168, value);
				int key169 = 130;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(6f, 8f),
					Rotation = 2.25f,
					Hide = true
				};
				dictionary.Add(key169, value);
				int key170 = 131;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(-8f, 8f),
					Rotation = -2.25f,
					Hide = true
				};
				dictionary.Add(key170, value);
				int key171 = 132;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key171, value);
				int key172 = 133;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, -5f),
					PortraitPositionYOverride = new float?((float)-25)
				};
				dictionary.Add(key172, value);
				int key173 = 134;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_134",
					Position = new Vector2(60f, 8f),
					PortraitPositionXOverride = new float?((float)3),
					PortraitPositionYOverride = new float?(0f)
				};
				dictionary.Add(key173, value);
				int key174 = 135;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key174, value);
				int key175 = 136;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key175, value);
				int key176 = 137;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(4f, -11f),
					PortraitPositionYOverride = new float?((float)-30)
				};
				dictionary.Add(key176, value);
				int key177 = 140;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key177, value);
				int key178 = 142;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					PortraitPositionXOverride = new float?((float)1)
				};
				dictionary.Add(key178, value);
				int key179 = 146;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key179, value);
				int key180 = 148;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key180, value);
				int key181 = 149;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key181, value);
				int key182 = 150;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, -11f),
					PortraitPositionYOverride = new float?((float)-30)
				};
				dictionary.Add(key182, value);
				int key183 = 151;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, -11f),
					PortraitPositionYOverride = new float?((float)-30)
				};
				dictionary.Add(key183, value);
				int key184 = 152;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(2f, -11f),
					PortraitPositionYOverride = new float?((float)-30)
				};
				dictionary.Add(key184, value);
				int key185 = 153;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(20f, 0f),
					Velocity = 1f,
					PortraitPositionXOverride = new float?(0f)
				};
				dictionary.Add(key185, value);
				int key186 = 154;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(20f, 0f),
					Velocity = 1f,
					PortraitPositionXOverride = new float?(0f)
				};
				dictionary.Add(key186, value);
				int key187 = 155;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(15f, 0f),
					Velocity = 3f,
					PortraitPositionXOverride = new float?(0f)
				};
				dictionary.Add(key187, value);
				int key188 = 156;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					PortraitPositionYOverride = new float?((float)-15)
				};
				dictionary.Add(key188, value);
				int key189 = 157;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(20f, 5f),
					PortraitPositionXOverride = new float?((float)5),
					PortraitPositionYOverride = new float?((float)10),
					IsWet = true
				};
				dictionary.Add(key189, value);
				int key190 = 160;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key190, value);
				int key191 = 158;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, -11f),
					PortraitPositionYOverride = new float?((float)-30)
				};
				dictionary.Add(key191, value);
				int key192 = 159;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key192, value);
				int key193 = 161;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key193, value);
				int key194 = 162;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key194, value);
				int key195 = 163;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key195, value);
				int key196 = 164;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key196, value);
				int key197 = 165;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Rotation = -1.6f,
					Velocity = 2f
				};
				dictionary.Add(key197, value);
				int key198 = 167;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key198, value);
				int key199 = 168;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key199, value);
				int key200 = 170;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(10f, 5f),
					PortraitPositionXOverride = new float?(0f),
					PortraitPositionYOverride = new float?((float)-12)
				};
				dictionary.Add(key200, value);
				int key201 = 171;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(10f, 5f),
					PortraitPositionXOverride = new float?(0f),
					PortraitPositionYOverride = new float?((float)-12)
				};
				dictionary.Add(key201, value);
				int key202 = 173;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Rotation = 0.75f,
					Position = new Vector2(0f, -5f)
				};
				dictionary.Add(key202, value);
				int key203 = 174;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, -5f),
					Velocity = 1f
				};
				dictionary.Add(key203, value);
				int key204 = 175;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(4f, -2f),
					Rotation = 2.3561945f
				};
				dictionary.Add(key204, value);
				int key205 = 176;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 5f)
				};
				dictionary.Add(key205, value);
				int key206 = 177;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(10f, 15f),
					PortraitPositionXOverride = new float?((float)-4),
					PortraitPositionYOverride = new float?((float)1),
					Frame = new int?(0)
				};
				dictionary.Add(key206, value);
				int key207 = 178;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					PortraitPositionXOverride = new float?((float)2)
				};
				dictionary.Add(key207, value);
				int key208 = 179;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(-5f, 12f),
					PortraitPositionYOverride = new float?((float)-7)
				};
				dictionary.Add(key208, value);
				int key209 = 180;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(10f, 5f),
					PortraitPositionXOverride = new float?(0f),
					PortraitPositionYOverride = new float?((float)-12)
				};
				dictionary.Add(key209, value);
				int key210 = 181;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key210, value);
				int key211 = 185;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key211, value);
				int key212 = 186;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key212, value);
				int key213 = 187;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key213, value);
				int key214 = 188;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key214, value);
				int key215 = 189;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key215, value);
				int key216 = 190;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(6f, -15f),
					PortraitPositionYOverride = new float?((float)-35)
				};
				dictionary.Add(key216, value);
				int key217 = 191;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(6f, -15f),
					PortraitPositionYOverride = new float?((float)-35)
				};
				dictionary.Add(key217, value);
				int key218 = 192;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(6f, -15f),
					PortraitPositionYOverride = new float?((float)-35)
				};
				dictionary.Add(key218, value);
				int key219 = 193;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(5f, -15f),
					PortraitPositionYOverride = new float?((float)-35)
				};
				dictionary.Add(key219, value);
				int key220 = 194;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(5f, -15f),
					PortraitPositionYOverride = new float?((float)-35)
				};
				dictionary.Add(key220, value);
				int key221 = 196;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key221, value);
				int key222 = 197;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key222, value);
				int key223 = 198;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key223, value);
				int key224 = 199;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key224, value);
				int key225 = 200;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					PortraitPositionYOverride = new float?((float)2)
				};
				dictionary.Add(key225, value);
				int key226 = 201;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key226, value);
				int key227 = 202;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key227, value);
				int key228 = 203;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key228, value);
				int key229 = 206;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key229, value);
				int key230 = 207;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key230, value);
				int key231 = 208;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					PortraitPositionXOverride = new float?((float)2)
				};
				dictionary.Add(key231, value);
				int key232 = 209;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key232, value);
				int key233 = 212;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key233, value);
				int key234 = 213;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key234, value);
				int key235 = 214;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key235, value);
				int key236 = 215;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key236, value);
				int key237 = 216;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key237, value);
				int key238 = 221;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 5f),
					Velocity = 1f
				};
				dictionary.Add(key238, value);
				int key239 = 222;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(10f, 55f),
					PortraitPositionXOverride = new float?(0f),
					PortraitPositionYOverride = new float?((float)40)
				};
				dictionary.Add(key239, value);
				int key240 = 223;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key240, value);
				int key241 = 224;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(5f, -10f),
					PortraitPositionYOverride = new float?((float)-30)
				};
				dictionary.Add(key241, value);
				int key242 = 226;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(5f, 3f),
					PortraitPositionYOverride = new float?((float)-15)
				};
				dictionary.Add(key242, value);
				int key243 = 227;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					PortraitPositionXOverride = new float?((float)-3)
				};
				dictionary.Add(key243, value);
				int key244 = 228;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					PortraitPositionXOverride = new float?((float)-5)
				};
				dictionary.Add(key244, value);
				int key245 = 229;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key245, value);
				int key246 = 225;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(2f, 0f)
				};
				dictionary.Add(key246, value);
				int key247 = 230;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key247, value);
				int key248 = 231;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 5f)
				};
				dictionary.Add(key248, value);
				int key249 = 232;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 5f)
				};
				dictionary.Add(key249, value);
				int key250 = 233;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 5f)
				};
				dictionary.Add(key250, value);
				int key251 = 234;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 5f)
				};
				dictionary.Add(key251, value);
				int key252 = 235;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 5f)
				};
				dictionary.Add(key252, value);
				int key253 = 236;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key253, value);
				int key254 = 237;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Rotation = -1.6f,
					Velocity = 2f
				};
				dictionary.Add(key254, value);
				int key255 = 238;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Rotation = -1.6f,
					Velocity = 2f
				};
				dictionary.Add(key255, value);
				int key256 = 239;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key256, value);
				int key257 = 240;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Rotation = -1.6f,
					Velocity = 2f
				};
				dictionary.Add(key257, value);
				int key258 = 241;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 6f),
					PortraitPositionYOverride = new float?((float)6),
					IsWet = true
				};
				dictionary.Add(key258, value);
				int key259 = 242;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 10f)
				};
				dictionary.Add(key259, value);
				int key260 = 243;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 60f),
					Velocity = 1f,
					PortraitPositionYOverride = new float?((float)15)
				};
				dictionary.Add(key260, value);
				int key261 = 245;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_245",
					Position = new Vector2(2f, 48f),
					PortraitPositionXOverride = new float?(0f),
					PortraitPositionYOverride = new float?((float)24)
				};
				dictionary.Add(key261, value);
				int key262 = 246;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key262, value);
				int key263 = 247;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key263, value);
				int key264 = 248;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key264, value);
				int key265 = 249;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key265, value);
				int key266 = 250;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, -6f),
					PortraitPositionYOverride = new float?((float)-26)
				};
				dictionary.Add(key266, value);
				int key267 = 251;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key267, value);
				int key268 = 252;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(3f, 3f),
					Velocity = 0.05f
				};
				dictionary.Add(key268, value);
				int key269 = 253;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 5f)
				};
				dictionary.Add(key269, value);
				int key270 = 254;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key270, value);
				int key271 = 255;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					PortraitPositionXOverride = new float?((float)-2)
				};
				dictionary.Add(key271, value);
				int key272 = 256;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 5f)
				};
				dictionary.Add(key272, value);
				int key273 = 257;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key273, value);
				int key274 = 258;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key274, value);
				int key275 = 259;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_259",
					Position = new Vector2(0f, 25f),
					PortraitPositionXOverride = new float?(0f),
					PortraitPositionYOverride = new float?((float)8)
				};
				dictionary.Add(key275, value);
				int key276 = 260;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_260",
					Position = new Vector2(0f, 25f),
					PortraitPositionXOverride = new float?((float)1),
					PortraitPositionYOverride = new float?((float)4)
				};
				dictionary.Add(key276, value);
				int key277 = 261;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key277, value);
				int key278 = 262;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 20f),
					Scale = 0.8f
				};
				dictionary.Add(key278, value);
				int key279 = 264;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key279, value);
				int key280 = 263;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key280, value);
				int key281 = 265;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key281, value);
				int key282 = 266;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(5f, 5f),
					Frame = new int?(4)
				};
				dictionary.Add(key282, value);
				int key283 = 268;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(2f, -5f)
				};
				dictionary.Add(key283, value);
				int key284 = 269;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(-5f, 0f),
					Velocity = 1f
				};
				dictionary.Add(key284, value);
				int key285 = 270;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(-5f, 0f),
					Velocity = 1f
				};
				dictionary.Add(key285, value);
				int key286 = 271;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(-2f, 0f),
					Velocity = 1f
				};
				dictionary.Add(key286, value);
				int key287 = 272;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(-5f, 0f),
					Velocity = 1f
				};
				dictionary.Add(key287, value);
				int key288 = 273;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(-5f, 0f),
					Velocity = 1f
				};
				dictionary.Add(key288, value);
				int key289 = 274;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(-3f, 0f),
					Velocity = 1f
				};
				dictionary.Add(key289, value);
				int key290 = 275;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(-3f, 2f),
					PortraitPositionYOverride = new float?((float)3),
					Velocity = 1f
				};
				dictionary.Add(key290, value);
				int key291 = 276;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(-5f, 0f),
					Velocity = 1f
				};
				dictionary.Add(key291, value);
				int key292 = 277;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(-5f, 0f),
					Velocity = 1f
				};
				dictionary.Add(key292, value);
				int key293 = 278;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(-2f, 0f),
					Velocity = 1f
				};
				dictionary.Add(key293, value);
				int key294 = 279;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(-5f, 0f),
					Velocity = 1f
				};
				dictionary.Add(key294, value);
				int key295 = 280;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(-3f, 0f),
					Velocity = 1f
				};
				dictionary.Add(key295, value);
				int key296 = 287;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key296, value);
				int key297 = 289;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(2f, 10f),
					Direction = new int?(1)
				};
				dictionary.Add(key297, value);
				int key298 = 290;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(4f, 6f),
					Velocity = 1f
				};
				dictionary.Add(key298, value);
				int key299 = 291;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key299, value);
				int key300 = 292;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key300, value);
				int key301 = 293;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key301, value);
				int key302 = 294;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key302, value);
				int key303 = 295;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key303, value);
				int key304 = 296;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key304, value);
				int key305 = 297;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(1f, -14f),
					Velocity = 0.05f,
					PortraitPositionYOverride = new float?((float)-30)
				};
				dictionary.Add(key305, value);
				int key306 = 298;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(1f, -14f),
					Velocity = 0.05f,
					PortraitPositionYOverride = new float?((float)-30)
				};
				dictionary.Add(key306, value);
				int key307 = 299;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key307, value);
				int key308 = 301;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					PortraitPositionYOverride = new float?((float)-20),
					Direction = new int?(-1),
					SpriteDirection = new int?(1),
					Velocity = 0.05f
				};
				dictionary.Add(key308, value);
				int key309 = 303;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key309, value);
				int key310 = 305;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 0.05f
				};
				dictionary.Add(key310, value);
				int key311 = 306;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 0.05f
				};
				dictionary.Add(key311, value);
				int key312 = 307;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 0.05f
				};
				dictionary.Add(key312, value);
				int key313 = 308;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 0.05f
				};
				dictionary.Add(key313, value);
				int key314 = 309;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 0.05f
				};
				dictionary.Add(key314, value);
				int key315 = 310;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key315, value);
				int key316 = 311;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key316, value);
				int key317 = 312;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key317, value);
				int key318 = 313;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key318, value);
				int key319 = 314;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key319, value);
				int key320 = 315;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(14f, 26f),
					Velocity = 2f,
					PortraitPositionXOverride = new float?(0f),
					PortraitPositionYOverride = new float?(0f)
				};
				dictionary.Add(key320, value);
				int key321 = 316;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 4f)
				};
				dictionary.Add(key321, value);
				int key322 = 317;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(5f, -15f),
					PortraitPositionYOverride = new float?((float)-35)
				};
				dictionary.Add(key322, value);
				int key323 = 318;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(5f, -13f),
					PortraitPositionYOverride = new float?((float)-31)
				};
				dictionary.Add(key323, value);
				int key324 = 319;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key324, value);
				int key325 = 320;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key325, value);
				int key326 = 321;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					PortraitPositionXOverride = new float?((float)2)
				};
				dictionary.Add(key326, value);
				int key327 = 322;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key327, value);
				int key328 = 323;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key328, value);
				int key329 = 324;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key329, value);
				int key330 = 325;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 36f)
				};
				dictionary.Add(key330, value);
				int key331 = 326;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key331, value);
				int key332 = 327;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, -8f)
				};
				dictionary.Add(key332, value);
				int key333 = 328;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key333, value);
				int key334 = 329;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 2f
				};
				dictionary.Add(key334, value);
				int key335 = 330;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(2f, 14f)
				};
				dictionary.Add(key335, value);
				int key336 = 331;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key336, value);
				int key337 = 332;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key337, value);
				int key338 = 337;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key338, value);
				int key339 = 338;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key339, value);
				int key340 = 339;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key340, value);
				int key341 = 340;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key341, value);
				int key342 = 342;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key342, value);
				int key343 = 343;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(5f, 25f),
					Velocity = 1f,
					PortraitPositionYOverride = new float?(0f)
				};
				dictionary.Add(key343, value);
				int key344 = 344;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 90f)
				};
				dictionary.Add(key344, value);
				int key345 = 345;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(-1f, 90f)
				};
				dictionary.Add(key345, value);
				int key346 = 346;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(30f, 80f),
					PortraitPositionXOverride = new float?(0f),
					PortraitPositionYOverride = new float?((float)60)
				};
				dictionary.Add(key346, value);
				int key347 = 347;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 4f)
				};
				dictionary.Add(key347, value);
				int key348 = 348;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					Hide = true
				};
				dictionary.Add(key348, value);
				int key349 = 349;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(-3f, 18f),
					PortraitPositionYOverride = new float?(0f)
				};
				dictionary.Add(key349, value);
				int key350 = 350;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(3f, 0f),
					Velocity = 1f
				};
				dictionary.Add(key350, value);
				int key351 = 351;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(6f, 60f),
					Velocity = 1f,
					PortraitPositionYOverride = new float?((float)30)
				};
				dictionary.Add(key351, value);
				int key352 = 353;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					PortraitPositionXOverride = new float?((float)2)
				};
				dictionary.Add(key352, value);
				int key353 = 633;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					PortraitPositionXOverride = new float?((float)2)
				};
				dictionary.Add(key353, value);
				int key354 = 354;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key354, value);
				int key355 = 355;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(1f, 2f)
				};
				dictionary.Add(key355, value);
				int key356 = 356;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(2f, 3f),
					PortraitPositionYOverride = new float?((float)1)
				};
				dictionary.Add(key356, value);
				int key357 = 357;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 2f),
					Velocity = 1f
				};
				dictionary.Add(key357, value);
				int key358 = 358;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(1f, 2f)
				};
				dictionary.Add(key358, value);
				int key359 = 359;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 18f),
					PortraitPositionYOverride = new float?((float)40)
				};
				dictionary.Add(key359, value);
				int key360 = 360;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(1f, 17f),
					PortraitPositionYOverride = new float?((float)39)
				};
				dictionary.Add(key360, value);
				int key361 = 362;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(3f, 0f),
					Velocity = 1f
				};
				dictionary.Add(key361, value);
				int key362 = 363;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					Hide = true
				};
				dictionary.Add(key362, value);
				int key363 = 364;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(3f, 0f),
					Velocity = 1f
				};
				dictionary.Add(key363, value);
				int key364 = 365;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					Hide = true
				};
				dictionary.Add(key364, value);
				int key365 = 366;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(1f, 0f),
					Velocity = 1f
				};
				dictionary.Add(key365, value);
				int key366 = 367;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(2f, 0f),
					Velocity = 1f
				};
				dictionary.Add(key366, value);
				int key367 = 368;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key367, value);
				int key368 = 369;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					PortraitPositionXOverride = new float?((float)2)
				};
				dictionary.Add(key368, value);
				int key369 = 370;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(56f, -4f),
					Direction = new int?(1),
					PortraitPositionXOverride = new float?(0f),
					PortraitPositionYOverride = new float?(0f)
				};
				dictionary.Add(key369, value);
				int key370 = 371;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key370, value);
				int key371 = 372;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(35f, 4f),
					Velocity = 1f,
					PortraitPositionXOverride = new float?((float)10),
					PortraitPositionYOverride = new float?((float)-3)
				};
				dictionary.Add(key371, value);
				int key372 = 373;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key372, value);
				int key373 = 374;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key373, value);
				int key374 = 375;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key374, value);
				int key375 = 376;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key375, value);
				int key376 = 379;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key376, value);
				int key377 = 380;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key377, value);
				int key378 = 381;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(-2f, 0f),
					Velocity = 1f
				};
				dictionary.Add(key378, value);
				int key379 = 382;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(-2f, 0f),
					Velocity = 1f
				};
				dictionary.Add(key379, value);
				int key380 = 383;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(-2f, 0f),
					Velocity = 1f
				};
				dictionary.Add(key380, value);
				int key381 = 384;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key381, value);
				int key382 = 385;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(-2f, 0f),
					Velocity = 1f
				};
				dictionary.Add(key382, value);
				int key383 = 386;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(-2f, 0f),
					Velocity = 1f
				};
				dictionary.Add(key383, value);
				int key384 = 387;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(2f, 0f),
					Velocity = 3f
				};
				dictionary.Add(key384, value);
				int key385 = 388;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Direction = new int?(1)
				};
				dictionary.Add(key385, value);
				int key386 = 389;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(-6f, 0f),
					Velocity = 1f
				};
				dictionary.Add(key386, value);
				int key387 = 390;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(12f, 0f),
					Direction = new int?(-1),
					SpriteDirection = new int?(1),
					Velocity = 2f,
					PortraitPositionXOverride = new float?(0f),
					PortraitPositionYOverride = new float?((float)-12)
				};
				dictionary.Add(key387, value);
				int key388 = 391;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(16f, 12f),
					Direction = new int?(-1),
					SpriteDirection = new int?(1),
					Velocity = 2f,
					PortraitPositionXOverride = new float?((float)3),
					PortraitPositionYOverride = new float?(0f)
				};
				dictionary.Add(key388, value);
				int key389 = 392;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key389, value);
				int key390 = 395;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_395",
					Position = new Vector2(-1f, 18f),
					PortraitPositionXOverride = new float?((float)1),
					PortraitPositionYOverride = new float?(0f)
				};
				dictionary.Add(key390, value);
				int key391 = 393;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key391, value);
				int key392 = 394;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key392, value);
				int key393 = 396;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key393, value);
				int key394 = 397;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key394, value);
				int key395 = 398;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_398",
					Position = new Vector2(0f, 5f),
					Scale = 0.4f,
					PortraitScale = new float?(0.7f),
					PortraitPositionYOverride = new float?(0f)
				};
				dictionary.Add(key395, value);
				int key396 = 400;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key396, value);
				int key397 = 401;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key397, value);
				int key398 = 402;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_402",
					Position = new Vector2(42f, 15f),
					PortraitPositionXOverride = new float?(0f),
					PortraitPositionYOverride = new float?(0f)
				};
				dictionary.Add(key398, value);
				int key399 = 403;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key399, value);
				int key400 = 404;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key400, value);
				int key401 = 408;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key401, value);
				int key402 = 410;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key402, value);
				int key403 = 411;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Direction = new int?(-1),
					SpriteDirection = new int?(1),
					Velocity = 1f
				};
				dictionary.Add(key403, value);
				int key404 = 412;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_412",
					Position = new Vector2(50f, 28f),
					PortraitPositionXOverride = new float?(0f),
					PortraitPositionYOverride = new float?((float)4)
				};
				dictionary.Add(key404, value);
				int key405 = 413;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key405, value);
				int key406 = 414;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key406, value);
				int key407 = 415;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(26f, 0f),
					Velocity = 3f,
					PortraitPositionXOverride = new float?((float)5)
				};
				dictionary.Add(key407, value);
				int key408 = 416;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(6f, 20f),
					Velocity = 1f,
					PortraitPositionXOverride = new float?(0f),
					PortraitPositionYOverride = new float?(0f)
				};
				dictionary.Add(key408, value);
				int key409 = 417;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(-2f, 8f),
					Velocity = 1f,
					PortraitPositionYOverride = new float?(0f)
				};
				dictionary.Add(key409, value);
				int key410 = 418;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(2f, 4f)
				};
				dictionary.Add(key410, value);
				int key411 = 419;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(-2f, 0f),
					Velocity = 1f
				};
				dictionary.Add(key411, value);
				int key412 = 420;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 4f),
					Direction = new int?(1)
				};
				dictionary.Add(key412, value);
				int key413 = 421;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(1f, -1f)
				};
				dictionary.Add(key413, value);
				int key414 = 422;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(1f, 44f),
					Scale = 0.4f,
					PortraitPositionXOverride = new float?((float)2),
					PortraitPositionYOverride = new float?((float)134)
				};
				dictionary.Add(key414, value);
				int key415 = 423;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Direction = new int?(-1),
					SpriteDirection = new int?(1),
					Velocity = 1f
				};
				dictionary.Add(key415, value);
				int key416 = 424;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(4f, 0f),
					Direction = new int?(-1),
					SpriteDirection = new int?(1),
					Velocity = 2f
				};
				dictionary.Add(key416, value);
				int key417 = 425;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(4f, 0f),
					Direction = new int?(-1),
					SpriteDirection = new int?(1),
					Velocity = 2f
				};
				dictionary.Add(key417, value);
				int key418 = 426;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(2f, 8f),
					Velocity = 2f,
					PortraitPositionYOverride = new float?(0f)
				};
				dictionary.Add(key418, value);
				int key419 = 427;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					PortraitPositionYOverride = new float?((float)-4)
				};
				dictionary.Add(key419, value);
				int key420 = 428;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key420, value);
				int key421 = 429;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(2f, 0f),
					Velocity = 1f
				};
				dictionary.Add(key421, value);
				int key422 = 430;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key422, value);
				int key423 = 431;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key423, value);
				int key424 = 432;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key424, value);
				int key425 = 433;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key425, value);
				int key426 = 434;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key426, value);
				int key427 = 435;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key427, value);
				int key428 = 436;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key428, value);
				int key429 = 437;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key429, value);
				int key430 = 439;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key430, value);
				int key431 = 440;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key431, value);
				int key432 = 441;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key432, value);
				int key433 = 442;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(1f, -14f),
					Velocity = 0.05f,
					PortraitPositionYOverride = new float?((float)-30)
				};
				dictionary.Add(key433, value);
				int key434 = 443;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key434, value);
				int key435 = 444;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(2f, 2f),
					PortraitPositionYOverride = new float?(0f)
				};
				dictionary.Add(key435, value);
				int key436 = 448;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key436, value);
				int key437 = 449;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key437, value);
				int key438 = 450;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key438, value);
				int key439 = 451;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key439, value);
				int key440 = 452;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key440, value);
				int key441 = 453;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key441, value);
				int key442 = 454;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_454",
					Position = new Vector2(57f, 10f),
					PortraitPositionXOverride = new float?((float)5),
					PortraitPositionYOverride = new float?(0f)
				};
				dictionary.Add(key442, value);
				int key443 = 455;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key443, value);
				int key444 = 456;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key444, value);
				int key445 = 457;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key445, value);
				int key446 = 458;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key446, value);
				int key447 = 459;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key447, value);
				int key448 = 460;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key448, value);
				int key449 = 461;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(-2f, 0f),
					Velocity = 1f
				};
				dictionary.Add(key449, value);
				int key450 = 462;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key450, value);
				int key451 = 463;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key451, value);
				int key452 = 464;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key452, value);
				int key453 = 465;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 6f),
					PortraitPositionYOverride = new float?((float)6),
					IsWet = true
				};
				dictionary.Add(key453, value);
				int key454 = 466;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key454, value);
				int key455 = 467;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key455, value);
				int key456 = 468;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key456, value);
				int key457 = 469;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key457, value);
				int key458 = 470;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key458, value);
				int key459 = 471;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key459, value);
				int key460 = 472;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 0f),
					PortraitPositionYOverride = new float?((float)-30),
					SpriteDirection = new int?(-1),
					Velocity = 1f
				};
				dictionary.Add(key460, value);
				int key461 = 476;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key461, value);
				int key462 = 477;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(25f, 6f),
					PortraitPositionXOverride = new float?((float)10)
				};
				dictionary.Add(key462, value);
				int key463 = 478;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key463, value);
				int key464 = 479;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(5f, 4f),
					PortraitPositionYOverride = new float?((float)-15)
				};
				dictionary.Add(key464, value);
				int key465 = 481;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key465, value);
				int key466 = 482;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key466, value);
				int key467 = 483;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, -10f),
					PortraitPositionYOverride = new float?((float)-30)
				};
				dictionary.Add(key467, value);
				int key468 = 484;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key468, value);
				int key469 = 487;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 0.5f
				};
				dictionary.Add(key469, value);
				int key470 = 486;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 0.5f
				};
				dictionary.Add(key470, value);
				int key471 = 485;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 0.5f
				};
				dictionary.Add(key471, value);
				int key472 = 489;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key472, value);
				int key473 = 491;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_491",
					Position = new Vector2(30f, -5f),
					Scale = 0.8f,
					PortraitPositionXOverride = new float?((float)1),
					PortraitPositionYOverride = new float?((float)-1)
				};
				dictionary.Add(key473, value);
				int key474 = 492;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key474, value);
				int key475 = 493;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(1f, 44f),
					Scale = 0.4f,
					PortraitPositionXOverride = new float?((float)2),
					PortraitPositionYOverride = new float?((float)134)
				};
				dictionary.Add(key475, value);
				int key476 = 494;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key476, value);
				int key477 = 495;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(-4f, 0f),
					Velocity = 1f
				};
				dictionary.Add(key477, value);
				int key478 = 496;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 0.5f
				};
				dictionary.Add(key478, value);
				int key479 = 497;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 0.5f
				};
				dictionary.Add(key479, value);
				int key480 = 498;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key480, value);
				int key481 = 499;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key481, value);
				int key482 = 500;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key482, value);
				int key483 = 501;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key483, value);
				int key484 = 502;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key484, value);
				int key485 = 503;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key485, value);
				int key486 = 504;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key486, value);
				int key487 = 505;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key487, value);
				int key488 = 506;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key488, value);
				int key489 = 507;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(1f, 44f),
					Scale = 0.4f,
					PortraitPositionXOverride = new float?((float)2),
					PortraitPositionYOverride = new float?((float)134)
				};
				dictionary.Add(key489, value);
				int key490 = 508;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					Position = new Vector2(10f, 0f),
					PortraitPositionXOverride = new float?(0f)
				};
				dictionary.Add(key490, value);
				int key491 = 509;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(6f, 0f),
					PortraitPositionXOverride = new float?((float)-10),
					PortraitPositionYOverride = new float?((float)-20)
				};
				dictionary.Add(key491, value);
				int key492 = 510;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_510",
					Position = new Vector2(55f, 18f),
					PortraitPositionXOverride = new float?((float)10),
					PortraitPositionYOverride = new float?((float)12)
				};
				dictionary.Add(key492, value);
				int key493 = 512;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key493, value);
				int key494 = 511;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key494, value);
				int key495 = 513;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_513",
					Position = new Vector2(37f, 24f),
					PortraitPositionXOverride = new float?((float)10),
					PortraitPositionYOverride = new float?((float)17)
				};
				dictionary.Add(key495, value);
				int key496 = 514;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key496, value);
				int key497 = 515;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key497, value);
				int key498 = 516;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key498, value);
				int key499 = 517;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(1f, 44f),
					Scale = 0.4f,
					PortraitPositionXOverride = new float?((float)2),
					PortraitPositionYOverride = new float?((float)135)
				};
				dictionary.Add(key499, value);
				int key500 = 518;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(-17f, 0f),
					Velocity = 1f
				};
				dictionary.Add(key500, value);
				int key501 = 519;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key501, value);
				int key502 = 520;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 56f),
					Velocity = 1f,
					PortraitPositionYOverride = new float?((float)10)
				};
				dictionary.Add(key502, value);
				int key503 = 521;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(5f, 5f),
					PortraitPositionYOverride = new float?((float)-10),
					SpriteDirection = new int?(-1),
					Velocity = 0.05f
				};
				dictionary.Add(key503, value);
				int key504 = 522;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key504, value);
				int key505 = 523;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key505, value);
				int key506 = 524;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key506, value);
				int key507 = 525;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key507, value);
				int key508 = 526;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key508, value);
				int key509 = 527;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key509, value);
				int key510 = 528;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key510, value);
				int key511 = 529;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key511, value);
				int key512 = 530;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key512, value);
				int key513 = 531;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 4f),
					Velocity = 2f,
					PortraitPositionYOverride = new float?((float)10)
				};
				dictionary.Add(key513, value);
				int key514 = 532;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(6f, 0f),
					Velocity = 1f
				};
				dictionary.Add(key514, value);
				int key515 = 533;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(3f, 5f)
				};
				dictionary.Add(key515, value);
				int key516 = 534;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key516, value);
				int key517 = 536;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(-2f, 0f),
					Velocity = 1f
				};
				dictionary.Add(key517, value);
				int key518 = 538;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key518, value);
				int key519 = 539;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key519, value);
				int key520 = 540;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key520, value);
				int key521 = 541;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 30f),
					PortraitPositionYOverride = new float?(0f)
				};
				dictionary.Add(key521, value);
				int key522 = 542;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(35f, -3f),
					PortraitPositionXOverride = new float?(0f)
				};
				dictionary.Add(key522, value);
				int key523 = 543;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(35f, -3f),
					PortraitPositionXOverride = new float?(0f)
				};
				dictionary.Add(key523, value);
				int key524 = 544;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(35f, -3f),
					PortraitPositionXOverride = new float?(0f)
				};
				dictionary.Add(key524, value);
				int key525 = 545;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(35f, -3f),
					PortraitPositionXOverride = new float?(0f)
				};
				dictionary.Add(key525, value);
				int key526 = 546;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, -3f),
					Direction = new int?(1)
				};
				dictionary.Add(key526, value);
				int key527 = 547;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key527, value);
				int key528 = 548;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key528, value);
				int key529 = 549;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key529, value);
				int key530 = 550;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					PortraitPositionXOverride = new float?((float)-2)
				};
				dictionary.Add(key530, value);
				int key531 = 551;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(95f, -4f)
				};
				dictionary.Add(key531, value);
				int key532 = 552;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key532, value);
				int key533 = 553;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key533, value);
				int key534 = 554;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key534, value);
				int key535 = 555;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key535, value);
				int key536 = 556;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key536, value);
				int key537 = 557;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key537, value);
				int key538 = 558;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(3f, -2f)
				};
				dictionary.Add(key538, value);
				int key539 = 559;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(3f, -2f)
				};
				dictionary.Add(key539, value);
				int key540 = 560;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(3f, -2f)
				};
				dictionary.Add(key540, value);
				int key541 = 561;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key541, value);
				int key542 = 562;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key542, value);
				int key543 = 563;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key543, value);
				int key544 = 566;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(-3f, 0f),
					Velocity = 1f
				};
				dictionary.Add(key544, value);
				int key545 = 567;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(-3f, 0f),
					Velocity = 1f
				};
				dictionary.Add(key545, value);
				int key546 = 568;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key546, value);
				int key547 = 569;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key547, value);
				int key548 = 570;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(10f, 5f),
					Velocity = 1f,
					PortraitPositionXOverride = new float?(0f),
					PortraitPositionYOverride = new float?((float)2)
				};
				dictionary.Add(key548, value);
				int key549 = 571;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(10f, 5f),
					Velocity = 1f,
					PortraitPositionXOverride = new float?(0f),
					PortraitPositionYOverride = new float?((float)2)
				};
				dictionary.Add(key549, value);
				int key550 = 572;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key550, value);
				int key551 = 573;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key551, value);
				int key552 = 578;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 4f)
				};
				dictionary.Add(key552, value);
				int key553 = 574;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(16f, 6f),
					Velocity = 1f
				};
				dictionary.Add(key553, value);
				int key554 = 575;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(16f, 6f),
					Velocity = 1f
				};
				dictionary.Add(key554, value);
				int key555 = 576;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(20f, 70f),
					Velocity = 1f,
					PortraitPositionXOverride = new float?((float)10),
					PortraitPositionYOverride = new float?(0f),
					PortraitScale = new float?(0.75f)
				};
				dictionary.Add(key555, value);
				int key556 = 577;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(20f, 70f),
					Velocity = 1f,
					PortraitPositionXOverride = new float?((float)10),
					PortraitPositionYOverride = new float?(0f),
					PortraitScale = new float?(0.75f)
				};
				dictionary.Add(key556, value);
				int key557 = 580;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 0f),
					Scale = 0.9f,
					Velocity = 1f
				};
				dictionary.Add(key557, value);
				int key558 = 581;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, -8f),
					PortraitPositionYOverride = new float?((float)-30)
				};
				dictionary.Add(key558, value);
				int key559 = 582;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key559, value);
				int key560 = 585;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 1f),
					Direction = new int?(1)
				};
				dictionary.Add(key560, value);
				int key561 = 584;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 1f),
					Direction = new int?(1)
				};
				dictionary.Add(key561, value);
				int key562 = 583;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 1f),
					Direction = new int?(1)
				};
				dictionary.Add(key562, value);
				int key563 = 586;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key563, value);
				int key564 = 579;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key564, value);
				int key565 = 588;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					PortraitPositionXOverride = new float?((float)1)
				};
				dictionary.Add(key565, value);
				int key566 = 587;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(2f, -14f),
					PortraitPositionYOverride = new float?((float)-30)
				};
				dictionary.Add(key566, value);
				int key567 = 591;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(9f, 0f)
				};
				dictionary.Add(key567, value);
				int key568 = 590;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					PortraitPositionXOverride = new float?((float)2)
				};
				dictionary.Add(key568, value);
				int key569 = 592;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 6f),
					PortraitPositionYOverride = new float?((float)7),
					IsWet = true
				};
				dictionary.Add(key569, value);
				int key570 = 593;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key570, value);
				int key571 = 594;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_594",
					Scale = 0.8f
				};
				dictionary.Add(key571, value);
				int key572 = 589;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key572, value);
				int key573 = 602;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key573, value);
				int key574 = 603;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key574, value);
				int key575 = 604;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(2f, 22f),
					PortraitPositionYOverride = new float?((float)41)
				};
				dictionary.Add(key575, value);
				int key576 = 605;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(2f, 22f),
					PortraitPositionYOverride = new float?((float)41)
				};
				dictionary.Add(key576, value);
				int key577 = 606;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key577, value);
				int key578 = 607;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 6f),
					PortraitPositionYOverride = new float?((float)7),
					IsWet = true
				};
				dictionary.Add(key578, value);
				int key579 = 608;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key579, value);
				int key580 = 609;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key580, value);
				int key581 = 611;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 0f),
					Direction = new int?(-1),
					SpriteDirection = new int?(1)
				};
				dictionary.Add(key581, value);
				int key582 = 612;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key582, value);
				int key583 = 613;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key583, value);
				int key584 = 614;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key584, value);
				int key585 = 615;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 10f),
					Scale = 0.88f,
					PortraitPositionYOverride = new float?((float)20),
					IsWet = true
				};
				dictionary.Add(key585, value);
				int key586 = 616;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key586, value);
				int key587 = 617;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key587, value);
				int key588 = 618;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(12f, -5f),
					Scale = 0.9f,
					PortraitPositionYOverride = new float?(0f)
				};
				dictionary.Add(key588, value);
				int key589 = 619;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(1f, 7f),
					Scale = 0.85f,
					PortraitPositionYOverride = new float?((float)10)
				};
				dictionary.Add(key589, value);
				int key590 = 620;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(6f, 5f),
					Scale = 0.78f,
					Velocity = 1f
				};
				dictionary.Add(key590, value);
				int key591 = 621;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_621",
					Position = new Vector2(46f, 20f),
					PortraitPositionXOverride = new float?((float)10),
					PortraitPositionYOverride = new float?((float)17)
				};
				dictionary.Add(key591, value);
				int key592 = 622;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key592, value);
				int key593 = 623;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key593, value);
				int key594 = 624;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 2f
				};
				dictionary.Add(key594, value);
				int key595 = 625;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, -12f),
					Velocity = 1f
				};
				dictionary.Add(key595, value);
				int key596 = 626;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, -16f)
				};
				dictionary.Add(key596, value);
				int key597 = 627;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, -16f)
				};
				dictionary.Add(key597, value);
				int key598 = 628;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Direction = new int?(1)
				};
				dictionary.Add(key598, value);
				int key599 = 630;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 0.5f
				};
				dictionary.Add(key599, value);
				int key600 = 632;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key600, value);
				int key601 = 631;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 0.75f
				};
				dictionary.Add(key601, value);
				int key602 = 634;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					Position = new Vector2(0f, -13f),
					PortraitPositionYOverride = new float?((float)-30)
				};
				dictionary.Add(key602, value);
				int key603 = 635;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key603, value);
				int key604 = 636;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 50f),
					PortraitPositionYOverride = new float?((float)30)
				};
				dictionary.Add(key604, value);
				int key605 = 639;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key605, value);
				int key606 = 640;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key606, value);
				int key607 = 641;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key607, value);
				int key608 = 642;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key608, value);
				int key609 = 643;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key609, value);
				int key610 = 644;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key610, value);
				int key611 = 645;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key611, value);
				int key612 = 646;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key612, value);
				int key613 = 647;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key613, value);
				int key614 = 648;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key614, value);
				int key615 = 649;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key615, value);
				int key616 = 650;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key616, value);
				int key617 = 651;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key617, value);
				int key618 = 652;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key618, value);
				int key619 = 637;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 0.25f
				};
				dictionary.Add(key619, value);
				int key620 = 638;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key620, value);
				int key621 = 653;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(2f, 3f),
					PortraitPositionYOverride = new float?((float)1)
				};
				dictionary.Add(key621, value);
				int key622 = 654;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(1f, 2f)
				};
				dictionary.Add(key622, value);
				int key623 = 655;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(1f, 17f),
					PortraitPositionYOverride = new float?((float)39)
				};
				dictionary.Add(key623, value);
				int key624 = 656;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f
				};
				dictionary.Add(key624, value);
				int key625 = 657;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 60f),
					PortraitPositionYOverride = new float?((float)40)
				};
				dictionary.Add(key625, value);
				int key626 = 660;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(-2f, -4f),
					PortraitPositionYOverride = new float?((float)-20)
				};
				dictionary.Add(key626, value);
				int key627 = 661;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(2f, 3f),
					PortraitPositionYOverride = new float?((float)1)
				};
				dictionary.Add(key627, value);
				int key628 = 662;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key628, value);
				int key629 = 663;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 1f,
					PortraitPositionXOverride = new float?((float)1)
				};
				dictionary.Add(key629, value);
				dictionary.Add(664, new NPCID.Sets.NPCBestiaryDrawModifiers(0));
				int key630 = 667;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key630, value);
				int key631 = 668;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 2.5f,
					Position = new Vector2(36f, 40f),
					Scale = 0.9f,
					PortraitPositionXOverride = new float?((float)6),
					PortraitPositionYOverride = new float?((float)50)
				};
				dictionary.Add(key631, value);
				int key632 = 669;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(2f, 22f),
					PortraitPositionYOverride = new float?((float)41)
				};
				dictionary.Add(key632, value);
				int key633 = 670;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					SpriteDirection = new int?(1),
					Velocity = 0.7f
				};
				dictionary.Add(key633, value);
				int key634 = 678;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					SpriteDirection = new int?(1),
					Velocity = 0.7f
				};
				dictionary.Add(key634, value);
				int key635 = 679;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					SpriteDirection = new int?(1),
					Velocity = 0.7f
				};
				dictionary.Add(key635, value);
				int key636 = 680;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					SpriteDirection = new int?(1),
					Velocity = 0.7f
				};
				dictionary.Add(key636, value);
				int key637 = 681;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					SpriteDirection = new int?(1),
					Velocity = 0.7f
				};
				dictionary.Add(key637, value);
				int key638 = 682;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					SpriteDirection = new int?(1),
					Velocity = 0.7f
				};
				dictionary.Add(key638, value);
				int key639 = 683;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					SpriteDirection = new int?(1),
					Velocity = 0.7f
				};
				dictionary.Add(key639, value);
				int key640 = 684;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					SpriteDirection = new int?(1),
					Velocity = 0.7f
				};
				dictionary.Add(key640, value);
				int key641 = 671;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(1f, -18f),
					Velocity = 0.05f,
					PortraitPositionYOverride = new float?((float)-35)
				};
				dictionary.Add(key641, value);
				int key642 = 672;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(1f, -18f),
					Velocity = 0.05f,
					PortraitPositionYOverride = new float?((float)-35)
				};
				dictionary.Add(key642, value);
				int key643 = 673;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(1f, -16f),
					Velocity = 0.05f,
					PortraitPositionYOverride = new float?((float)-35)
				};
				dictionary.Add(key643, value);
				int key644 = 674;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(1f, -16f),
					Velocity = 0.05f,
					PortraitPositionYOverride = new float?((float)-35)
				};
				dictionary.Add(key644, value);
				int key645 = 675;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(1f, -16f),
					Velocity = 0.05f,
					PortraitPositionYOverride = new float?((float)-35)
				};
				dictionary.Add(key645, value);
				int key646 = 677;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(1f, 2f)
				};
				dictionary.Add(key646, value);
				int key647 = 685;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key647, value);
				int key648 = 686;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key648, value);
				int key649 = 687;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Velocity = 0f
				};
				dictionary.Add(key649, value);
				int key650 = 688;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Position = new Vector2(0f, 6f),
					PortraitPositionYOverride = new float?((float)7),
					IsWet = true
				};
				dictionary.Add(key650, value);
				int key651 = 695;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key651, value);
				int key652 = 696;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key652, value);
				int key653 = 0;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key653, value);
				int key654 = 488;
				value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
				{
					Hide = true
				};
				dictionary.Add(key654, value);
				return dictionary;
			}

			// Token: 0x04006F1A RID: 28442
			public static SetFactory Factory = new SetFactory((int)NPCID.Count);

			// Token: 0x04006F1B RID: 28443
			public static Dictionary<int, int> SpecialSpawningRules = new Dictionary<int, int>
			{
				{
					259,
					0
				},
				{
					260,
					0
				},
				{
					175,
					0
				},
				{
					43,
					0
				},
				{
					56,
					0
				},
				{
					101,
					0
				}
			};

			// Token: 0x04006F1C RID: 28444
			public static Dictionary<int, NPCID.Sets.NPCPortraitProvider> NPCPortraits = new Dictionary<int, NPCID.Sets.NPCPortraitProvider>
			{
				{
					369,
					NPCID.Sets.PrioritizedPortrait().With(new NPCID.Sets.NPCPortraitSelector.SelectionCondition(NPCID.Sets.ShimmeredPortraitCondition), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Angler_shimmer")).Default(NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Angler"))
				},
				{
					19,
					NPCID.Sets.PrioritizedPortrait().With(new NPCID.Sets.NPCPortraitSelector.SelectionCondition(NPCID.Sets.ShimmeredPortraitCondition), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_ArmsDealer_shimmer")).Default(NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_ArmsDealer"))
				},
				{
					54,
					NPCID.Sets.PrioritizedPortrait().With(new NPCID.Sets.NPCPortraitSelector.SelectionCondition(NPCID.Sets.ShimmeredPortraitCondition), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Clothier_shimmer")).Default(NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Clothier"))
				},
				{
					209,
					NPCID.Sets.PrioritizedPortrait().With(new NPCID.Sets.NPCPortraitSelector.SelectionCondition(NPCID.Sets.ShimmeredPortraitCondition), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Cyborg_shimmer")).Default(NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Cyborg"))
				},
				{
					38,
					NPCID.Sets.PrioritizedPortrait().With(new NPCID.Sets.NPCPortraitSelector.SelectionCondition(NPCID.Sets.ShimmeredPortraitCondition), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Demolitionist_shimmer")).Default(NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Demolitionist"))
				},
				{
					20,
					NPCID.Sets.PrioritizedPortrait().With(new NPCID.Sets.NPCPortraitSelector.SelectionCondition(NPCID.Sets.ShimmeredPortraitCondition), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Dryad_shimmer")).Default(NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Dryad"))
				},
				{
					207,
					NPCID.Sets.PrioritizedPortrait().With(new NPCID.Sets.NPCPortraitSelector.SelectionCondition(NPCID.Sets.ShimmeredPortraitCondition), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_DyeTrader_shimmer")).Default(NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_DyeTrader"))
				},
				{
					107,
					NPCID.Sets.PrioritizedPortrait().With(new NPCID.Sets.NPCPortraitSelector.SelectionCondition(NPCID.Sets.ShimmeredPortraitCondition), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_GoblinTinkerer_shimmer")).Default(NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_GoblinTinkerer"))
				},
				{
					588,
					NPCID.Sets.PrioritizedPortrait().With(new NPCID.Sets.NPCPortraitSelector.SelectionCondition(NPCID.Sets.ShimmeredPortraitCondition), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Golfer_shimmer")).Default(NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Golfer"))
				},
				{
					22,
					NPCID.Sets.PrioritizedPortrait().With(new NPCID.Sets.NPCPortraitSelector.SelectionCondition(NPCID.Sets.ShimmeredPortraitCondition), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Guide_shimmer")).Default(NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Guide"))
				},
				{
					124,
					NPCID.Sets.PrioritizedPortrait().With(new NPCID.Sets.NPCPortraitSelector.SelectionCondition(NPCID.Sets.ShimmeredPortraitCondition), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Mechanic_shimmer")).Default(NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Mechanic"))
				},
				{
					17,
					NPCID.Sets.PrioritizedPortrait().With(new NPCID.Sets.NPCPortraitSelector.SelectionCondition(NPCID.Sets.ShimmeredPortraitCondition), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Merchant_shimmer")).Default(NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Merchant"))
				},
				{
					18,
					NPCID.Sets.PrioritizedPortrait().With(new NPCID.Sets.NPCPortraitSelector.SelectionCondition(NPCID.Sets.ShimmeredPortraitCondition), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Nurse_shimmer")).Default(NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Nurse"))
				},
				{
					37,
					NPCID.Sets.PrioritizedPortrait().With(new NPCID.Sets.NPCPortraitSelector.SelectionCondition(NPCID.Sets.ShimmeredPortraitCondition), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_OldMan_shimmer")).Default(NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_OldMan"))
				},
				{
					227,
					NPCID.Sets.PrioritizedPortrait().With(new NPCID.Sets.NPCPortraitSelector.SelectionCondition(NPCID.Sets.ShimmeredPortraitCondition), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Painter_shimmer")).Default(NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Painter"))
				},
				{
					208,
					NPCID.Sets.PrioritizedPortrait().With(new NPCID.Sets.NPCPortraitSelector.SelectionCondition(NPCID.Sets.ShimmeredPortraitCondition), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_PartyGirl_shimmer")).Default(NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_PartyGirl"))
				},
				{
					229,
					NPCID.Sets.PrioritizedPortrait().With(new NPCID.Sets.NPCPortraitSelector.SelectionCondition(NPCID.Sets.ShimmeredPortraitCondition), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Pirate_shimmer")).Default(NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Pirate"))
				},
				{
					663,
					NPCID.Sets.PrioritizedPortrait().With(new NPCID.Sets.NPCPortraitSelector.SelectionCondition(NPCID.Sets.ShimmeredPortraitCondition), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Princess_shimmer")).Default(NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Princess"))
				},
				{
					142,
					NPCID.Sets.PrioritizedPortrait().With(new NPCID.Sets.NPCPortraitSelector.SelectionCondition(NPCID.Sets.ShimmeredPortraitCondition), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Santa_shimmer")).Default(NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Santa"))
				},
				{
					453,
					NPCID.Sets.PrioritizedPortrait().With(new NPCID.Sets.NPCPortraitSelector.SelectionCondition(NPCID.Sets.ShimmeredPortraitCondition), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_SkeletonMerchant_shimmer")).Default(NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_SkeletonMerchant"))
				},
				{
					178,
					NPCID.Sets.PrioritizedPortrait().With(new NPCID.Sets.NPCPortraitSelector.SelectionCondition(NPCID.Sets.ShimmeredPortraitCondition), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Steampunker_shimmer")).Default(NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Steampunker"))
				},
				{
					353,
					NPCID.Sets.PrioritizedPortrait().With(new NPCID.Sets.NPCPortraitSelector.SelectionCondition(NPCID.Sets.ShimmeredPortraitCondition), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Stylist_shimmer")).Default(NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Stylist"))
				},
				{
					550,
					NPCID.Sets.PrioritizedPortrait().With(new NPCID.Sets.NPCPortraitSelector.SelectionCondition(NPCID.Sets.ShimmeredPortraitCondition), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Tavernkeep_shimmer")).Default(NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Tavernkeep"))
				},
				{
					441,
					NPCID.Sets.PrioritizedPortrait().With(new NPCID.Sets.NPCPortraitSelector.SelectionCondition(NPCID.Sets.ShimmeredPortraitCondition), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_TaxCollector_shimmer")).Default(NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_TaxCollector"))
				},
				{
					368,
					NPCID.Sets.PrioritizedPortrait().With(new NPCID.Sets.NPCPortraitSelector.SelectionCondition(NPCID.Sets.ShimmeredPortraitCondition), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_TravellingMerchant_shimmer")).Default(NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_TravellingMerchant"))
				},
				{
					160,
					NPCID.Sets.PrioritizedPortrait().With(new NPCID.Sets.NPCPortraitSelector.SelectionCondition(NPCID.Sets.ShimmeredPortraitCondition), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Truffle_shimmer")).Default(NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Truffle"))
				},
				{
					228,
					NPCID.Sets.PrioritizedPortrait().With(new NPCID.Sets.NPCPortraitSelector.SelectionCondition(NPCID.Sets.ShimmeredPortraitCondition), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_WitchDoctor_shimmer")).Default(NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_WitchDoctor"))
				},
				{
					108,
					NPCID.Sets.PrioritizedPortrait().With(new NPCID.Sets.NPCPortraitSelector.SelectionCondition(NPCID.Sets.ShimmeredPortraitCondition), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Wizard_shimmer")).Default(NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Wizard"))
				},
				{
					633,
					NPCID.Sets.PrioritizedPortrait().With(() => NPCID.Sets.ShimmeredPortraitCondition() && !NPC.ShouldBestiaryGirlBeLycantrope(), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Zoologista_shimmer")).With(() => NPCID.Sets.ShimmeredPortraitCondition() && NPC.ShouldBestiaryGirlBeLycantrope(), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Zoologistb_shimmer")).With(() => !NPCID.Sets.ShimmeredPortraitCondition() && NPC.ShouldBestiaryGirlBeLycantrope(), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Zoologistb")).Default(NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Zoologista"))
				},
				{
					680,
					NPCID.Sets.PrioritizedPortrait().Default(NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_SlimeClumsy"))
				},
				{
					678,
					NPCID.Sets.PrioritizedPortrait().Default(NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_SlimeCool"))
				},
				{
					681,
					NPCID.Sets.PrioritizedPortrait().Default(NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_SlimeDiva"))
				},
				{
					679,
					NPCID.Sets.PrioritizedPortrait().Default(NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_SlimeElder"))
				},
				{
					683,
					NPCID.Sets.PrioritizedPortrait().Default(NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_SlimeMystic"))
				},
				{
					670,
					NPCID.Sets.PrioritizedPortrait().Default(NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_SlimeNerdy"))
				},
				{
					684,
					NPCID.Sets.PrioritizedPortrait().Default(NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_SlimeSquire"))
				},
				{
					682,
					NPCID.Sets.PrioritizedPortrait().Default(NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_SlimeSurly"))
				},
				{
					638,
					NPCID.Sets.PrioritizedPortrait().With(NPCID.Sets.VariantPortraitCondition(0), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Dog_Labrador")).With(NPCID.Sets.VariantPortraitCondition(1), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Dog_PitBull")).With(NPCID.Sets.VariantPortraitCondition(2), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Dog_Beagle")).With(NPCID.Sets.VariantPortraitCondition(3), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Dog_Corgi")).With(NPCID.Sets.VariantPortraitCondition(4), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Dog_Dalmatian")).With(NPCID.Sets.VariantPortraitCondition(5), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Dog_Husky")).Default(NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Dog_Labrador"))
				},
				{
					637,
					NPCID.Sets.PrioritizedPortrait().With(NPCID.Sets.VariantPortraitCondition(0), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Cat_Siamese")).With(NPCID.Sets.VariantPortraitCondition(1), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Cat_Black")).With(NPCID.Sets.VariantPortraitCondition(2), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Cat_Orangetabby")).With(NPCID.Sets.VariantPortraitCondition(3), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Cat_RussianBlue")).With(NPCID.Sets.VariantPortraitCondition(4), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Cat_Silver")).With(NPCID.Sets.VariantPortraitCondition(5), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Cat_White")).Default(NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Cat_Black"))
				},
				{
					656,
					NPCID.Sets.PrioritizedPortrait().With(NPCID.Sets.VariantPortraitCondition(0), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Bunny_White")).With(NPCID.Sets.VariantPortraitCondition(1), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Bunny_Angora")).With(NPCID.Sets.VariantPortraitCondition(2), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Bunny_Dutch")).With(NPCID.Sets.VariantPortraitCondition(3), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Bunny_Flemish")).With(NPCID.Sets.VariantPortraitCondition(4), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Bunny_Lop")).With(NPCID.Sets.VariantPortraitCondition(5), NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Bunny_Silver")).Default(NPCID.Sets.BasicPortrait("Images/TownNPCs/Portraits/Portrait_Bunny_Angora"))
				}
			};

			// Token: 0x04006F1D RID: 28445
			public static Dictionary<int, NPCID.Sets.NPCBestiaryDrawModifiers> NPCBestiaryDrawOffset = NPCID.Sets.NPCBestiaryDrawOffsetCreation();

			// Token: 0x04006F1E RID: 28446
			public static Dictionary<int, NPCDebuffImmunityData> DebuffImmunitySets = new Dictionary<int, NPCDebuffImmunityData>
			{
				{
					0,
					null
				},
				{
					1,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					2,
					null
				},
				{
					3,
					null
				},
				{
					4,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					5,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					6,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					7,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					8,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					9,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					10,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					11,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					12,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					13,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31,
							30,
							375
						}
					}
				},
				{
					14,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31,
							30,
							375
						}
					}
				},
				{
					15,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31,
							30,
							375
						}
					}
				},
				{
					16,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					17,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					18,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					19,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					20,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					21,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					22,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					23,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							24,
							31,
							323
						}
					}
				},
				{
					24,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							24,
							31,
							323
						}
					}
				},
				{
					25,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					26,
					null
				},
				{
					27,
					null
				},
				{
					28,
					null
				},
				{
					29,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					30,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					665,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					31,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					32,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					33,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					34,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					35,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31,
							169,
							337,
							344
						}
					}
				},
				{
					36,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					37,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					38,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					39,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							24,
							31,
							323
						}
					}
				},
				{
					40,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							24,
							31,
							323
						}
					}
				},
				{
					41,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							24,
							31,
							323
						}
					}
				},
				{
					42,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					43,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					44,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					45,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					46,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					47,
					null
				},
				{
					48,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					49,
					null
				},
				{
					50,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					51,
					null
				},
				{
					52,
					null
				},
				{
					53,
					null
				},
				{
					54,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					55,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					56,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					57,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					58,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					59,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							24,
							323
						}
					}
				},
				{
					60,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							24,
							323
						}
					}
				},
				{
					61,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					62,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							24,
							31,
							153,
							323
						}
					}
				},
				{
					63,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					64,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					65,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					66,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							24,
							31,
							153,
							323
						}
					}
				},
				{
					67,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					68,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true,
						ImmuneToWhips = true
					}
				},
				{
					69,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					70,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							24,
							31,
							39,
							70,
							323
						}
					}
				},
				{
					71,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					72,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true,
						ImmuneToWhips = true
					}
				},
				{
					73,
					null
				},
				{
					74,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					75,
					null
				},
				{
					76,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					77,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					78,
					null
				},
				{
					79,
					null
				},
				{
					80,
					null
				},
				{
					81,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					82,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					83,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							24,
							31,
							323
						}
					}
				},
				{
					84,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							24,
							31,
							323
						}
					}
				},
				{
					85,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					690,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					86,
					null
				},
				{
					87,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					88,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					89,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					90,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					91,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					92,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					93,
					null
				},
				{
					94,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					95,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					96,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					97,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					98,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					99,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					100,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					101,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							39,
							31
						}
					}
				},
				{
					102,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					103,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					104,
					null
				},
				{
					105,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					106,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					107,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					108,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					109,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					110,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					111,
					null
				},
				{
					112,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					666,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					113,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							24,
							31,
							323
						}
					}
				},
				{
					114,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							24,
							31,
							323
						}
					}
				},
				{
					115,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					116,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					117,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					118,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					119,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					120,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					121,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					122,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					123,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					124,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					125,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					126,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					127,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31,
							169,
							337,
							344
						}
					}
				},
				{
					128,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					129,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					130,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					131,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					132,
					null
				},
				{
					133,
					null
				},
				{
					134,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					135,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					136,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					137,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					138,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					139,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					140,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							24,
							323
						}
					}
				},
				{
					141,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							70
						}
					}
				},
				{
					142,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					143,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							44,
							324
						}
					}
				},
				{
					144,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							44,
							324
						}
					}
				},
				{
					145,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							44,
							324
						}
					}
				},
				{
					146,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					147,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							44,
							324
						}
					}
				},
				{
					148,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					149,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					150,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							44,
							324
						}
					}
				},
				{
					151,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							24,
							323
						}
					}
				},
				{
					152,
					null
				},
				{
					153,
					null
				},
				{
					154,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							44,
							324
						}
					}
				},
				{
					155,
					null
				},
				{
					156,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							24,
							31,
							153,
							323
						}
					}
				},
				{
					157,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					158,
					null
				},
				{
					159,
					null
				},
				{
					160,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					161,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							44,
							324
						}
					}
				},
				{
					162,
					null
				},
				{
					163,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					164,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					165,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					166,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					167,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							44,
							324
						}
					}
				},
				{
					168,
					null
				},
				{
					169,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31,
							44,
							324
						}
					}
				},
				{
					170,
					null
				},
				{
					171,
					null
				},
				{
					172,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					173,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					174,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					175,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					176,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					177,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					178,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					179,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							24,
							31,
							323
						}
					}
				},
				{
					180,
					null
				},
				{
					181,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					182,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					183,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					184,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							44,
							324
						}
					}
				},
				{
					185,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							44,
							324
						}
					}
				},
				{
					186,
					null
				},
				{
					187,
					null
				},
				{
					188,
					null
				},
				{
					189,
					null
				},
				{
					190,
					null
				},
				{
					191,
					null
				},
				{
					192,
					null
				},
				{
					193,
					null
				},
				{
					194,
					null
				},
				{
					195,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					196,
					null
				},
				{
					197,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							44,
							324
						}
					}
				},
				{
					198,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					199,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					200,
					null
				},
				{
					201,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					202,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					203,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					204,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					205,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					206,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							44,
							324
						}
					}
				},
				{
					207,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					208,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					209,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					210,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					211,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					212,
					null
				},
				{
					213,
					null
				},
				{
					214,
					null
				},
				{
					215,
					null
				},
				{
					216,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					217,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					218,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					219,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					220,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					221,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					222,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					223,
					null
				},
				{
					224,
					null
				},
				{
					225,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					226,
					null
				},
				{
					227,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					228,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					229,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					230,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					231,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					232,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					233,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					234,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					235,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					236,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					237,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					238,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					239,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					240,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					241,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					242,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					243,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31,
							44,
							324
						}
					}
				},
				{
					244,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					245,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							24,
							31,
							323
						}
					}
				},
				{
					246,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							24,
							31,
							323
						}
					}
				},
				{
					247,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							24,
							31,
							323
						}
					}
				},
				{
					248,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							24,
							31,
							323
						}
					}
				},
				{
					249,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							24,
							31,
							323
						}
					}
				},
				{
					250,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					251,
					null
				},
				{
					252,
					null
				},
				{
					253,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					254,
					null
				},
				{
					255,
					null
				},
				{
					256,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					257,
					null
				},
				{
					258,
					null
				},
				{
					259,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					260,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					261,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					262,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					263,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					264,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					265,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					266,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					267,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					268,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							69
						}
					}
				},
				{
					269,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					270,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					271,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					272,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					273,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					274,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					275,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					276,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					277,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							24,
							323
						}
					}
				},
				{
					278,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							24,
							323
						}
					}
				},
				{
					279,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							24,
							323
						}
					}
				},
				{
					280,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							24,
							323
						}
					}
				},
				{
					281,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					282,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					283,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					284,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					285,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					286,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					287,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					288,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					289,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					290,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31,
							69
						}
					}
				},
				{
					291,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					292,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					293,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					294,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					295,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					296,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					297,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					298,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					671,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					672,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					673,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					674,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					675,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					299,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					300,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					301,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					302,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					303,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					304,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					305,
					null
				},
				{
					306,
					null
				},
				{
					307,
					null
				},
				{
					308,
					null
				},
				{
					309,
					null
				},
				{
					310,
					null
				},
				{
					311,
					null
				},
				{
					312,
					null
				},
				{
					313,
					null
				},
				{
					314,
					null
				},
				{
					315,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							24,
							31,
							323
						}
					}
				},
				{
					316,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							24,
							31,
							39,
							44,
							69,
							70,
							153,
							189,
							203,
							204,
							323,
							324
						}
					}
				},
				{
					317,
					null
				},
				{
					318,
					null
				},
				{
					319,
					null
				},
				{
					320,
					null
				},
				{
					321,
					null
				},
				{
					322,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					323,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					324,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					325,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					326,
					null
				},
				{
					327,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							24,
							31,
							323
						}
					}
				},
				{
					328,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							24,
							31,
							323
						}
					}
				},
				{
					329,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							24,
							323
						}
					}
				},
				{
					330,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					331,
					null
				},
				{
					332,
					null
				},
				{
					333,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					334,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					335,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					336,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					337,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					338,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							44,
							324
						}
					}
				},
				{
					339,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							44,
							324
						}
					}
				},
				{
					340,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							44,
							324
						}
					}
				},
				{
					341,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							24,
							31,
							323
						}
					}
				},
				{
					342,
					null
				},
				{
					343,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							44,
							324
						}
					}
				},
				{
					344,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31,
							44,
							324
						}
					}
				},
				{
					345,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31,
							44,
							324
						}
					}
				},
				{
					346,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31,
							44,
							324
						}
					}
				},
				{
					347,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					348,
					null
				},
				{
					349,
					null
				},
				{
					350,
					null
				},
				{
					351,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							44,
							324
						}
					}
				},
				{
					352,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31,
							44,
							324
						}
					}
				},
				{
					353,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					354,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					355,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					356,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					357,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					358,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					359,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					360,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					361,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					362,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					363,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					364,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					365,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					366,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					367,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					368,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					369,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					370,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					371,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					372,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					373,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					374,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					375,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					376,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					377,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					378,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					379,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					380,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					381,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					382,
					null
				},
				{
					383,
					null
				},
				{
					384,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					385,
					null
				},
				{
					386,
					null
				},
				{
					387,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					388,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					389,
					null
				},
				{
					390,
					null
				},
				{
					391,
					null
				},
				{
					392,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true,
						ImmuneToWhips = true
					}
				},
				{
					393,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					394,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					395,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					396,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					397,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					398,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					399,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					400,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					401,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					402,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					403,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					404,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					405,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					406,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					407,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					408,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					409,
					null
				},
				{
					410,
					null
				},
				{
					411,
					null
				},
				{
					412,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					413,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					414,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					415,
					null
				},
				{
					416,
					null
				},
				{
					417,
					null
				},
				{
					418,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							24,
							31,
							323
						}
					}
				},
				{
					419,
					null
				},
				{
					420,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					421,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					422,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					423,
					null
				},
				{
					424,
					null
				},
				{
					425,
					null
				},
				{
					426,
					null
				},
				{
					427,
					null
				},
				{
					428,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					429,
					null
				},
				{
					430,
					null
				},
				{
					431,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							44,
							324
						}
					}
				},
				{
					432,
					null
				},
				{
					433,
					null
				},
				{
					434,
					null
				},
				{
					435,
					null
				},
				{
					436,
					null
				},
				{
					437,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					438,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					439,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					440,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true,
						ImmuneToWhips = true
					}
				},
				{
					441,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					442,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					443,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					444,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					445,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					446,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					447,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					448,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					449,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					450,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					451,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					452,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					453,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					454,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					455,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					456,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					457,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					458,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					459,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					460,
					null
				},
				{
					461,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					462,
					null
				},
				{
					463,
					null
				},
				{
					464,
					null
				},
				{
					465,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					466,
					null
				},
				{
					467,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							24,
							31,
							323
						}
					}
				},
				{
					468,
					null
				},
				{
					469,
					null
				},
				{
					470,
					null
				},
				{
					471,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31,
							153
						}
					}
				},
				{
					472,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31,
							153
						}
					}
				},
				{
					473,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							24,
							31,
							323
						}
					}
				},
				{
					474,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							24,
							31,
							323
						}
					}
				},
				{
					475,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							24,
							31,
							323
						}
					}
				},
				{
					476,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							24,
							31,
							323
						}
					}
				},
				{
					477,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					478,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					479,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					480,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					481,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					482,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							24,
							31,
							323
						}
					}
				},
				{
					483,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							24,
							31,
							323
						}
					}
				},
				{
					484,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					485,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					486,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					487,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					488,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					489,
					null
				},
				{
					490,
					null
				},
				{
					491,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true,
						ImmuneToWhips = true
					}
				},
				{
					492,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					493,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					494,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					495,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					496,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					497,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					498,
					null
				},
				{
					499,
					null
				},
				{
					500,
					null
				},
				{
					501,
					null
				},
				{
					502,
					null
				},
				{
					503,
					null
				},
				{
					504,
					null
				},
				{
					505,
					null
				},
				{
					506,
					null
				},
				{
					507,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					508,
					null
				},
				{
					509,
					null
				},
				{
					510,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					511,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					512,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					513,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					514,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					515,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					516,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					517,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					518,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					519,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					520,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							24,
							31,
							323
						}
					}
				},
				{
					521,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							24,
							31,
							39,
							44,
							69,
							70,
							153,
							189,
							203,
							204,
							323,
							324
						}
					}
				},
				{
					522,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					523,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					524,
					null
				},
				{
					525,
					null
				},
				{
					526,
					null
				},
				{
					527,
					null
				},
				{
					528,
					null
				},
				{
					529,
					null
				},
				{
					530,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					531,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					532,
					null
				},
				{
					533,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							24,
							31,
							39,
							44,
							69,
							70,
							153,
							189,
							203,
							204,
							323,
							324
						}
					}
				},
				{
					534,
					null
				},
				{
					535,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					536,
					null
				},
				{
					537,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					538,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					539,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					540,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					541,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					542,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					543,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					544,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					545,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					546,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					547,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					548,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					549,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true,
						ImmuneToWhips = true
					}
				},
				{
					550,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					551,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							24,
							31,
							323
						}
					}
				},
				{
					552,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					553,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					554,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					555,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					556,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					557,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					558,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					559,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					560,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					561,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					562,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					563,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					564,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					565,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					566,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					567,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					568,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					569,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					570,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					571,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					572,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					573,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					574,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					575,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					576,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					577,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					578,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					579,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					580,
					null
				},
				{
					581,
					null
				},
				{
					582,
					null
				},
				{
					583,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true,
						ImmuneToWhips = true
					}
				},
				{
					584,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true,
						ImmuneToWhips = true
					}
				},
				{
					585,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true,
						ImmuneToWhips = true
					}
				},
				{
					586,
					null
				},
				{
					587,
					null
				},
				{
					588,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					589,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					590,
					null
				},
				{
					591,
					null
				},
				{
					592,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					593,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					594,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					595,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					596,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					597,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					598,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					599,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					600,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					601,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					602,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					603,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					604,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					605,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					606,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					607,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					608,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					609,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					610,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					611,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					689,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					612,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					613,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					614,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					615,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					616,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					617,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					618,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					619,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					620,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					621,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					622,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					623,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					624,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					625,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					626,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					627,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					628,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					629,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							24,
							31,
							44,
							323,
							324
						}
					}
				},
				{
					630,
					null
				},
				{
					631,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							24,
							31,
							323
						}
					}
				},
				{
					632,
					null
				},
				{
					633,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					634,
					null
				},
				{
					635,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20
						}
					}
				},
				{
					636,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					637,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					638,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					639,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					640,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					641,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					642,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					643,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					644,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					645,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					646,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					647,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					648,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					649,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					650,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					651,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					652,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					653,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					654,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					655,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					656,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					657,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					658,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					659,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					660,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					661,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					662,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					663,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					668,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					669,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					670,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					678,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					679,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					680,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					681,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					682,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					683,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					684,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					677,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					685,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					686,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true,
						ImmuneToWhips = true
					}
				},
				{
					687,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true,
						ImmuneToWhips = true
					}
				},
				{
					688,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					692,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					693,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					694,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							20,
							31
						}
					}
				},
				{
					695,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				},
				{
					696,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[]
						{
							31
						}
					}
				}
			};

			// Token: 0x04006F1F RID: 28447
			public static List<int> NormalGoldCritterBestiaryPriority = new List<int>
			{
				46,
				540,
				614,
				303,
				337,
				443,
				74,
				297,
				298,
				671,
				672,
				673,
				674,
				675,
				442,
				55,
				230,
				592,
				593,
				299,
				538,
				539,
				300,
				447,
				361,
				445,
				377,
				446,
				356,
				444,
				357,
				448,
				595,
				596,
				597,
				598,
				599,
				600,
				601,
				626,
				627,
				612,
				613,
				604,
				605,
				669,
				677
			};

			// Token: 0x04006F20 RID: 28448
			public static List<int> BossBestiaryPriority = new List<int>
			{
				664,
				4,
				5,
				50,
				535,
				13,
				14,
				15,
				266,
				267,
				668,
				35,
				36,
				222,
				113,
				114,
				117,
				115,
				116,
				657,
				658,
				659,
				660,
				125,
				126,
				134,
				135,
				136,
				139,
				127,
				128,
				131,
				129,
				130,
				262,
				263,
				264,
				636,
				245,
				246,
				249,
				247,
				248,
				370,
				372,
				373,
				439,
				438,
				379,
				380,
				440,
				521,
				454,
				507,
				517,
				422,
				493,
				398,
				396,
				397,
				400,
				401
			};

			// Token: 0x04006F21 RID: 28449
			public static List<int> TownNPCBestiaryPriority = new List<int>
			{
				22,
				17,
				18,
				38,
				369,
				20,
				19,
				207,
				227,
				353,
				633,
				550,
				588,
				107,
				228,
				124,
				54,
				108,
				178,
				229,
				160,
				441,
				209,
				208,
				663,
				142,
				637,
				638,
				656,
				670,
				678,
				679,
				680,
				681,
				682,
				683,
				684,
				368,
				453,
				37,
				687
			};

			// Token: 0x04006F22 RID: 28450
			public static bool[] SpawnOnPlayerCanSpawnInMidairOnSkyblock = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				4,
				266,
				222,
				125,
				126,
				127,
				262,
				551
			});

			// Token: 0x04006F23 RID: 28451
			public static bool[] DontDropDungeonKeysOrSouls = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				23
			});

			// Token: 0x04006F24 RID: 28452
			public static bool[] DontDoHardmodeScaling = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				5,
				13,
				14,
				15,
				267,
				113,
				114,
				115,
				116,
				117,
				118,
				119,
				658,
				659,
				660,
				400,
				522
			});

			// Token: 0x04006F25 RID: 28453
			public static bool[] ReflectStarShotsInForTheWorthy = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				4,
				5,
				13,
				14,
				15,
				266,
				267,
				35,
				36,
				113,
				114,
				115,
				116,
				117,
				118,
				119,
				125,
				126,
				134,
				135,
				136,
				139,
				127,
				128,
				131,
				129,
				130,
				262,
				263,
				264,
				245,
				247,
				248,
				246,
				249,
				398,
				400,
				397,
				396,
				401
			});

			// Token: 0x04006F26 RID: 28454
			public static bool[] IsTownPet = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				637,
				638,
				656,
				670,
				678,
				679,
				680,
				681,
				682,
				683,
				684
			});

			// Token: 0x04006F27 RID: 28455
			public static bool[] IsTownSlime = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				670,
				678,
				679,
				680,
				681,
				682,
				683,
				684
			});

			// Token: 0x04006F28 RID: 28456
			public static bool[] CanConvertIntoCopperSlimeTownNPC = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				1,
				302,
				335,
				336,
				333,
				334
			});

			// Token: 0x04006F29 RID: 28457
			public static List<int> GoldCrittersCollection = new List<int>
			{
				443,
				442,
				592,
				593,
				444,
				601,
				445,
				446,
				605,
				447,
				627,
				613,
				448,
				539
			};

			// Token: 0x04006F2A RID: 28458
			public static bool[] IsGoldCritter = NPCID.Sets.Factory.CreateBoolSet(false, new int[]
			{
				442,
				443,
				444,
				445,
				446,
				447,
				448,
				539,
				592,
				593,
				601,
				605,
				613,
				627
			});

			// Token: 0x04006F2B RID: 28459
			public static bool[] ZappingJellyfish = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				63,
				64,
				103,
				242
			});

			// Token: 0x04006F2C RID: 28460
			public static bool?[] HunterPotionFriendlyOverride = NPCID.Sets.Factory.CreateCustomSet<bool?>(null, new object[]
			{
				689,
				false
			});

			// Token: 0x04006F2D RID: 28461
			public static bool[] CantTakeLunchMoney = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				394,
				393,
				392,
				690,
				492,
				491,
				662,
				384,
				478,
				535,
				658,
				659,
				660,
				128,
				131,
				129,
				130,
				139,
				267,
				247,
				248,
				246,
				249,
				245,
				409,
				410,
				397,
				396,
				401,
				400,
				440,
				68,
				534,
				316
			});

			// Token: 0x04006F2E RID: 28462
			public static Dictionary<int, int> RespawnEnemyID = new Dictionary<int, int>
			{
				{
					492,
					0
				},
				{
					491,
					0
				},
				{
					394,
					0
				},
				{
					393,
					0
				},
				{
					392,
					0
				},
				{
					13,
					0
				},
				{
					14,
					0
				},
				{
					15,
					0
				},
				{
					412,
					0
				},
				{
					413,
					0
				},
				{
					414,
					0
				},
				{
					134,
					0
				},
				{
					135,
					0
				},
				{
					136,
					0
				},
				{
					454,
					0
				},
				{
					455,
					0
				},
				{
					456,
					0
				},
				{
					457,
					0
				},
				{
					458,
					0
				},
				{
					459,
					0
				},
				{
					8,
					7
				},
				{
					9,
					7
				},
				{
					11,
					10
				},
				{
					12,
					10
				},
				{
					40,
					39
				},
				{
					41,
					39
				},
				{
					96,
					95
				},
				{
					97,
					95
				},
				{
					99,
					98
				},
				{
					100,
					98
				},
				{
					88,
					87
				},
				{
					89,
					87
				},
				{
					90,
					87
				},
				{
					91,
					87
				},
				{
					92,
					87
				},
				{
					118,
					117
				},
				{
					119,
					117
				},
				{
					514,
					513
				},
				{
					515,
					513
				},
				{
					511,
					510
				},
				{
					512,
					510
				},
				{
					622,
					621
				},
				{
					623,
					621
				}
			};

			// Token: 0x04006F2F RID: 28463
			public static Dictionary<int, Vector2> NPCPortraitsCloseUpOffsets = new Dictionary<int, Vector2>
			{
				{
					17,
					new Vector2(6f, 0f)
				},
				{
					18,
					new Vector2(-6f, 0f)
				},
				{
					19,
					new Vector2(0f, 0f)
				},
				{
					20,
					new Vector2(0f, 0f)
				},
				{
					22,
					new Vector2(0f, 0f)
				},
				{
					38,
					new Vector2(0f, -6f)
				},
				{
					54,
					new Vector2(6f, 0f)
				},
				{
					37,
					new Vector2(0f, 0f)
				},
				{
					107,
					new Vector2(0f, 0f)
				},
				{
					108,
					new Vector2(6f, 4f)
				},
				{
					124,
					new Vector2(-6f, 4f)
				},
				{
					142,
					new Vector2(3f, 6f)
				},
				{
					160,
					new Vector2(-6f, 18f)
				},
				{
					178,
					new Vector2(-6f, 12f)
				},
				{
					207,
					new Vector2(0f, 0f)
				},
				{
					208,
					new Vector2(-12f, 0f)
				},
				{
					209,
					new Vector2(0f, 6f)
				},
				{
					227,
					new Vector2(-3f, 0f)
				},
				{
					228,
					new Vector2(9f, 6f)
				},
				{
					229,
					new Vector2(0f, 0f)
				},
				{
					353,
					new Vector2(-6f, 6f)
				},
				{
					368,
					new Vector2(0f, 6f)
				},
				{
					369,
					new Vector2(-6f, -6f)
				},
				{
					441,
					new Vector2(-9f, 0f)
				},
				{
					453,
					new Vector2(-12f, 0f)
				},
				{
					550,
					new Vector2(15f, 12f)
				},
				{
					588,
					new Vector2(3f, 0f)
				},
				{
					633,
					new Vector2(-3f, 0f)
				},
				{
					663,
					new Vector2(0f, -6f)
				},
				{
					637,
					new Vector2(-15f, 8f)
				},
				{
					638,
					new Vector2(-24f, 12f)
				},
				{
					656,
					new Vector2(0f, 0f)
				},
				{
					684,
					new Vector2(-3f, 2f)
				},
				{
					670,
					new Vector2(0f, 2f)
				},
				{
					678,
					new Vector2(-3f, 2f)
				},
				{
					679,
					new Vector2(0f, 2f)
				},
				{
					680,
					new Vector2(-6f, 2f)
				},
				{
					681,
					new Vector2(0f, 2f)
				},
				{
					683,
					new Vector2(-6f, 2f)
				},
				{
					682,
					new Vector2(-3f, 2f)
				}
			};

			// Token: 0x04006F30 RID: 28464
			public static Dictionary<int, Vector2> NPCPortraitsFullBodyRetroOffsets = new Dictionary<int, Vector2>
			{
				{
					18,
					new Vector2(-3f, 0f)
				},
				{
					20,
					new Vector2(-4f, 0f)
				},
				{
					124,
					new Vector2(-6f, 2f)
				},
				{
					178,
					new Vector2(-4f, 0f)
				},
				{
					208,
					new Vector2(-4f, 0f)
				},
				{
					227,
					new Vector2(6f, 0f)
				},
				{
					353,
					new Vector2(-4f, 0f)
				},
				{
					369,
					new Vector2(-4f, 0f)
				},
				{
					441,
					new Vector2(-10f, 0f)
				},
				{
					588,
					new Vector2(-2f, 0f)
				},
				{
					633,
					new Vector2(-6f, 0f)
				},
				{
					637,
					new Vector2(-8f, 0f)
				},
				{
					638,
					new Vector2(-8f, 0f)
				},
				{
					684,
					new Vector2(-12f, 0f)
				},
				{
					670,
					new Vector2(-6f, 0f)
				},
				{
					678,
					new Vector2(-8f, 0f)
				},
				{
					679,
					new Vector2(-6f, 0f)
				},
				{
					680,
					new Vector2(-10f, 0f)
				},
				{
					681,
					new Vector2(-6f, 0f)
				},
				{
					683,
					new Vector2(-6f, 0f)
				},
				{
					682,
					new Vector2(-6f, 0f)
				}
			};

			// Token: 0x04006F31 RID: 28465
			public static int[] TrailingMode = NPCID.Sets.Factory.CreateIntSet(-1, new int[]
			{
				439,
				0,
				440,
				0,
				370,
				1,
				372,
				1,
				373,
				1,
				396,
				1,
				400,
				1,
				401,
				1,
				473,
				2,
				474,
				2,
				475,
				2,
				476,
				2,
				4,
				3,
				471,
				3,
				477,
				3,
				479,
				3,
				120,
				4,
				137,
				4,
				138,
				4,
				94,
				5,
				125,
				6,
				126,
				6,
				127,
				6,
				128,
				6,
				129,
				6,
				130,
				6,
				131,
				6,
				139,
				6,
				140,
				6,
				407,
				6,
				420,
				6,
				425,
				6,
				427,
				6,
				426,
				6,
				581,
				6,
				516,
				6,
				542,
				6,
				543,
				6,
				544,
				6,
				545,
				6,
				402,
				7,
				417,
				7,
				419,
				7,
				418,
				7,
				574,
				7,
				575,
				7,
				519,
				7,
				521,
				7,
				522,
				7,
				546,
				7,
				558,
				7,
				559,
				7,
				560,
				7,
				551,
				7,
				620,
				7,
				657,
				6,
				636,
				7,
				677,
				7,
				685,
				7
			});

			// Token: 0x04006F32 RID: 28466
			public static bool[] IsDragonfly = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				595,
				596,
				597,
				598,
				599,
				600,
				601
			});

			// Token: 0x04006F33 RID: 28467
			public static bool[] BelongsToInvasionOldOnesArmy = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				552,
				553,
				554,
				561,
				562,
				563,
				555,
				556,
				557,
				558,
				559,
				560,
				576,
				577,
				568,
				569,
				566,
				567,
				570,
				571,
				572,
				573,
				548,
				549,
				564,
				565,
				574,
				575,
				551,
				578
			});

			// Token: 0x04006F34 RID: 28468
			public static bool[] TeleportationImmune = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				552,
				553,
				554,
				561,
				562,
				563,
				555,
				556,
				557,
				558,
				559,
				560,
				576,
				577,
				568,
				569,
				566,
				567,
				570,
				571,
				572,
				573,
				548,
				549,
				564,
				565,
				574,
				575,
				551,
				578
			});

			// Token: 0x04006F35 RID: 28469
			public static bool[] UsesNewTargeting = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				547,
				552,
				553,
				554,
				561,
				562,
				563,
				555,
				556,
				557,
				558,
				559,
				560,
				576,
				577,
				568,
				569,
				566,
				567,
				570,
				571,
				572,
				573,
				564,
				565,
				574,
				575,
				551,
				578,
				210,
				211,
				620,
				668
			});

			// Token: 0x04006F36 RID: 28470
			public static bool[] BirdThatCanPoop = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				74,
				297,
				298,
				442,
				363,
				365,
				603,
				609,
				671,
				672,
				673,
				674,
				675
			});

			// Token: 0x04006F37 RID: 28471
			public static bool[] CritterThatCanTurnOnPlayers = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				645,
				639,
				644,
				642,
				643,
				641,
				640,
				299,
				539,
				538
			});

			// Token: 0x04006F38 RID: 28472
			public static bool[] TakesDamageFromHostilesWithoutBeingFriendly = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				46,
				55,
				74,
				148,
				149,
				230,
				297,
				298,
				299,
				303,
				355,
				356,
				358,
				359,
				360,
				361,
				362,
				363,
				364,
				365,
				366,
				367,
				377,
				357,
				374,
				442,
				443,
				444,
				445,
				446,
				448,
				538,
				539,
				337,
				540,
				484,
				485,
				486,
				487,
				592,
				593,
				595,
				596,
				597,
				598,
				599,
				600,
				601,
				602,
				603,
				604,
				605,
				606,
				607,
				608,
				609,
				611,
				689,
				612,
				613,
				614,
				615,
				616,
				617,
				625,
				626,
				627,
				639,
				640,
				641,
				642,
				643,
				644,
				645,
				646,
				647,
				648,
				649,
				650,
				651,
				652,
				653,
				654,
				655,
				583,
				584,
				585,
				669,
				671,
				672,
				673,
				674,
				675,
				677,
				687,
				688
			});

			// Token: 0x04006F39 RID: 28473
			public static bool[] AllNPCs = NPCID.Sets.Factory.CreateBoolSet(true, new int[0]);

			// Token: 0x04006F3A RID: 28474
			public static bool[] HurtingBees = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				210,
				211
			});

			// Token: 0x04006F3B RID: 28475
			public static bool[] CanBeHurtByBees = NPCID.Sets.Factory.CreateBoolSet(true, new int[]
			{
				210,
				211,
				222
			});

			// Token: 0x04006F3C RID: 28476
			public static bool[] ConveyorBeltCollision = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				624,
				85,
				629,
				195,
				1,
				147,
				184,
				537,
				204,
				16,
				59,
				71,
				535,
				225,
				676,
				303,
				335,
				336,
				333,
				334,
				667,
				141,
				81,
				121,
				183,
				138,
				244,
				304,
				105,
				123,
				685,
				686,
				687,
				106,
				354,
				376,
				579,
				589,
				37,
				695,
				696
			});

			// Token: 0x04006F3D RID: 28477
			public static bool[] SlimeCanContainItems = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				1,
				59,
				147,
				184,
				537
			});

			// Token: 0x04006F3E RID: 28478
			public static bool[] FighterUsesDD2PortalAppearEffect = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				552,
				553,
				554,
				561,
				562,
				563,
				555,
				556,
				557,
				576,
				577,
				568,
				569,
				570,
				571,
				572,
				573,
				564,
				565
			});

			// Token: 0x04006F3F RID: 28479
			public static float[] StatueSpawnedDropRarity = NPCID.Sets.Factory.CreateCustomSet<float>(-1f, new object[]
			{
				480,
				0.05f,
				82,
				0.05f,
				86,
				0.05f,
				48,
				0.05f,
				490,
				0.05f,
				489,
				0.05f,
				170,
				0.05f,
				180,
				0.05f,
				171,
				0.05f,
				167,
				0.25f,
				73,
				0.01f,
				24,
				0.05f,
				481,
				0.05f,
				42,
				0.05f,
				6,
				0.05f,
				2,
				0.05f,
				49,
				0.2f,
				3,
				0.2f,
				58,
				0.2f,
				21,
				0.2f,
				65,
				0.2f,
				449,
				0.2f,
				482,
				0.2f,
				103,
				0.2f,
				64,
				0.2f,
				63,
				0.2f,
				85,
				0f
			});

			// Token: 0x04006F40 RID: 28480
			public static bool[] NoEarlymodeLootWhenSpawnedFromStatue = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				480,
				82,
				86,
				170,
				180,
				171
			});

			// Token: 0x04006F41 RID: 28481
			public static bool[] NeedsExpertScaling = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				25,
				30,
				665,
				33,
				112,
				666,
				261,
				265,
				371,
				516,
				519,
				397,
				396,
				398,
				491
			});

			// Token: 0x04006F42 RID: 28482
			public static bool[] ProjectileNPC = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				25,
				30,
				665,
				33,
				112,
				666,
				261,
				265,
				371,
				516,
				519
			});

			// Token: 0x04006F43 RID: 28483
			public static bool[] SavesAndLoads = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				422,
				507,
				517,
				493
			});

			// Token: 0x04006F44 RID: 28484
			public static int[] TrailCacheLength = NPCID.Sets.Factory.CreateIntSet(10, new int[]
			{
				402,
				36,
				519,
				20,
				522,
				20,
				620,
				20,
				677,
				60,
				685,
				10
			});

			// Token: 0x04006F45 RID: 28485
			public static bool[] SearchSpawnSlotsInReverse = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				222,
				245
			});

			// Token: 0x04006F46 RID: 28486
			public static bool[] CannotSpawnInSlot0 = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				134,
				135,
				136,
				7,
				8,
				9,
				10,
				11,
				12,
				13,
				14,
				15,
				39,
				40,
				41,
				87,
				88,
				89,
				90,
				91,
				92,
				95,
				96,
				97,
				98,
				99,
				100,
				117,
				118,
				119,
				375,
				402,
				412,
				413,
				414,
				454,
				455,
				456,
				457,
				458,
				459,
				510,
				511,
				512,
				513,
				514,
				515,
				621,
				622,
				623
			});

			// Token: 0x04006F47 RID: 28487
			public static bool[] SkipUpdateInUnsyncedTiles = NPCID.Sets.Factory.CreateBoolSet(true, new int[]
			{
				135,
				136,
				14,
				15,
				8,
				9,
				11,
				12,
				40,
				41,
				96,
				97,
				99,
				100,
				118,
				119,
				622,
				623,
				511,
				512,
				514,
				515,
				455,
				456,
				457,
				458,
				459,
				89,
				90,
				91,
				92,
				413,
				414
			});

			// Token: 0x04006F48 RID: 28488
			public static bool[] UsesMultiplayerProximitySyncing = NPCID.Sets.Factory.CreateBoolSet(true, new int[]
			{
				398,
				397,
				396
			});

			// Token: 0x04006F49 RID: 28489
			public static bool[] NoMultiplayerSmoothingByType = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				50,
				657,
				120,
				245,
				247,
				248,
				246,
				370,
				222,
				398,
				397,
				396,
				400,
				401,
				668,
				70,
				690
			});

			// Token: 0x04006F4A RID: 28490
			public static bool[] NoMultiplayerSmoothingByAI = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				6,
				8,
				37
			});

			// Token: 0x04006F4B RID: 28491
			public static bool[] MPAllowedEnemies = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				4,
				13,
				50,
				126,
				125,
				134,
				127,
				128,
				131,
				129,
				130,
				222,
				245,
				266,
				370,
				657,
				668
			});

			// Token: 0x04006F4C RID: 28492
			public static bool[] TownCritter = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				46,
				148,
				149,
				230,
				299,
				300,
				303,
				337,
				361,
				362,
				364,
				366,
				367,
				443,
				445,
				447,
				538,
				539,
				540,
				583,
				584,
				585,
				592,
				593,
				602,
				607,
				608,
				610,
				616,
				617,
				625,
				626,
				627,
				639,
				640,
				641,
				642,
				643,
				644,
				645,
				646,
				647,
				648,
				649,
				650,
				651,
				652,
				687,
				688
			});

			// Token: 0x04006F4D RID: 28493
			public static bool[] CountsAsCritter = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				46,
				303,
				337,
				540,
				443,
				74,
				297,
				298,
				442,
				611,
				689,
				377,
				446,
				612,
				613,
				356,
				444,
				595,
				596,
				597,
				598,
				599,
				600,
				601,
				604,
				605,
				357,
				448,
				374,
				484,
				355,
				358,
				606,
				359,
				360,
				485,
				486,
				487,
				148,
				149,
				55,
				230,
				592,
				593,
				299,
				538,
				539,
				300,
				447,
				361,
				445,
				362,
				363,
				364,
				365,
				367,
				366,
				583,
				584,
				585,
				602,
				603,
				607,
				608,
				609,
				610,
				616,
				617,
				625,
				626,
				627,
				615,
				639,
				640,
				641,
				642,
				643,
				644,
				645,
				646,
				647,
				648,
				649,
				650,
				651,
				652,
				653,
				654,
				655,
				661,
				669,
				671,
				672,
				673,
				674,
				675,
				677,
				687,
				688
			});

			// Token: 0x04006F4E RID: 28494
			public static bool[] HasNoPartyText = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				441,
				453
			});

			// Token: 0x04006F4F RID: 28495
			public static int[] HatOffsetY = NPCID.Sets.Factory.CreateIntSet(0, new int[]
			{
				227,
				4,
				107,
				2,
				108,
				2,
				229,
				4,
				17,
				2,
				38,
				8,
				160,
				-10,
				208,
				2,
				142,
				2,
				124,
				2,
				453,
				2,
				37,
				4,
				54,
				4,
				209,
				4,
				369,
				6,
				441,
				6,
				353,
				-2,
				633,
				-2,
				550,
				-2,
				588,
				2,
				663,
				2,
				637,
				0,
				638,
				0,
				656,
				4,
				670,
				0,
				678,
				0,
				679,
				0,
				680,
				0,
				681,
				0,
				682,
				0,
				683,
				0,
				684,
				0
			});

			// Token: 0x04006F50 RID: 28496
			public static int[] FaceEmote = NPCID.Sets.Factory.CreateIntSet(0, new int[]
			{
				17,
				101,
				18,
				102,
				19,
				103,
				20,
				104,
				22,
				105,
				37,
				106,
				38,
				107,
				54,
				108,
				107,
				109,
				108,
				110,
				124,
				111,
				142,
				112,
				160,
				113,
				178,
				114,
				207,
				115,
				208,
				116,
				209,
				117,
				227,
				118,
				228,
				119,
				229,
				120,
				353,
				121,
				368,
				122,
				369,
				123,
				453,
				124,
				441,
				125,
				588,
				140,
				633,
				141,
				663,
				145
			});

			// Token: 0x04006F51 RID: 28497
			public static int[] ExtraFramesCount = NPCID.Sets.Factory.CreateIntSet(0, new int[]
			{
				17,
				9,
				18,
				9,
				19,
				9,
				20,
				7,
				22,
				10,
				37,
				5,
				38,
				9,
				54,
				7,
				107,
				9,
				108,
				7,
				124,
				9,
				142,
				9,
				160,
				7,
				178,
				9,
				207,
				9,
				208,
				9,
				209,
				10,
				227,
				9,
				228,
				10,
				229,
				10,
				353,
				9,
				633,
				9,
				368,
				10,
				369,
				9,
				453,
				9,
				441,
				9,
				550,
				9,
				588,
				9,
				663,
				7,
				637,
				18,
				638,
				11,
				656,
				20,
				670,
				6,
				678,
				6,
				679,
				6,
				680,
				6,
				681,
				6,
				682,
				6,
				683,
				6,
				684,
				6
			});

			// Token: 0x04006F52 RID: 28498
			public static int[] AttackFrameCount = NPCID.Sets.Factory.CreateIntSet(0, new int[]
			{
				17,
				4,
				18,
				4,
				19,
				4,
				20,
				2,
				22,
				5,
				37,
				0,
				38,
				4,
				54,
				2,
				107,
				4,
				108,
				2,
				124,
				4,
				142,
				4,
				160,
				2,
				178,
				4,
				207,
				4,
				208,
				4,
				209,
				5,
				227,
				4,
				228,
				5,
				229,
				5,
				353,
				4,
				633,
				4,
				368,
				5,
				369,
				4,
				453,
				4,
				441,
				4,
				550,
				4,
				588,
				4,
				663,
				2,
				637,
				0,
				638,
				0,
				656,
				0,
				670,
				0,
				678,
				0,
				679,
				0,
				680,
				0,
				681,
				0,
				682,
				0,
				683,
				0,
				684,
				0
			});

			// Token: 0x04006F53 RID: 28499
			public static int[] DangerDetectRange = NPCID.Sets.Factory.CreateIntSet(-1, new int[]
			{
				38,
				300,
				17,
				320,
				107,
				300,
				19,
				900,
				22,
				700,
				124,
				800,
				228,
				800,
				178,
				900,
				18,
				300,
				229,
				1000,
				209,
				1000,
				54,
				700,
				108,
				700,
				160,
				700,
				20,
				1200,
				369,
				300,
				453,
				300,
				368,
				900,
				207,
				60,
				227,
				800,
				208,
				400,
				142,
				500,
				441,
				50,
				353,
				60,
				633,
				100,
				550,
				120,
				588,
				120,
				663,
				700,
				638,
				250,
				637,
				250,
				656,
				250,
				670,
				250,
				678,
				250,
				679,
				250,
				680,
				250,
				681,
				250,
				682,
				250,
				683,
				250,
				684,
				250
			});

			// Token: 0x04006F54 RID: 28500
			public static bool[] ShimmerImmunity = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				637,
				638,
				656,
				670,
				684,
				678,
				679,
				680,
				681,
				682,
				683,
				356,
				669,
				676,
				244,
				677,
				594,
				667,
				662,
				5,
				115,
				116,
				139,
				245,
				247,
				248,
				246,
				249,
				344,
				325,
				50,
				535,
				657,
				658,
				659,
				660,
				668,
				25,
				30,
				33,
				70,
				72,
				665,
				666,
				112,
				516,
				517,
				518,
				519,
				520,
				521,
				522,
				523,
				381,
				382,
				383,
				384,
				385,
				386,
				387,
				388,
				389,
				390,
				391,
				392,
				393,
				394,
				395,
				396,
				397,
				398,
				399,
				400,
				401,
				402,
				403,
				404,
				405,
				406,
				407,
				408,
				409,
				410,
				411,
				412,
				413,
				414,
				415,
				416,
				417,
				418,
				419,
				420,
				421,
				423,
				424,
				425,
				426,
				427,
				428,
				429,
				548,
				549,
				551,
				552,
				553,
				554,
				555,
				556,
				557,
				558,
				559,
				560,
				561,
				562,
				563,
				564,
				565,
				566,
				567,
				568,
				569,
				570,
				571,
				572,
				573,
				574,
				575,
				576,
				577,
				578
			});

			// Token: 0x04006F55 RID: 28501
			public static int[] ShimmerTransformToItem = NPCID.Sets.Factory.CreateIntSet(-1, new int[]
			{
				651,
				182,
				644,
				182,
				650,
				178,
				643,
				178,
				649,
				179,
				642,
				179,
				648,
				177,
				641,
				177,
				640,
				180,
				647,
				180,
				646,
				181,
				639,
				181,
				652,
				999,
				645,
				999,
				448,
				5341
			});

			// Token: 0x04006F56 RID: 28502
			public static bool[] ShimmerTownTransform = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				22,
				17,
				18,
				227,
				207,
				633,
				588,
				208,
				369,
				353,
				38,
				20,
				550,
				19,
				107,
				228,
				54,
				124,
				441,
				229,
				160,
				108,
				178,
				209,
				142,
				663,
				37,
				453,
				368
			});

			// Token: 0x04006F57 RID: 28503
			public static int[] ShimmerTransformToNPC = NPCID.Sets.Factory.CreateIntSet(-1, new int[]
			{
				3,
				21,
				132,
				202,
				186,
				201,
				187,
				21,
				188,
				21,
				189,
				202,
				200,
				203,
				590,
				21,
				1,
				676,
				302,
				676,
				335,
				676,
				336,
				676,
				334,
				676,
				333,
				676,
				225,
				676,
				141,
				676,
				16,
				676,
				147,
				676,
				184,
				676,
				537,
				676,
				204,
				676,
				81,
				676,
				183,
				676,
				138,
				676,
				121,
				676,
				591,
				449,
				430,
				449,
				436,
				452,
				432,
				450,
				433,
				449,
				434,
				449,
				435,
				451,
				614,
				677,
				74,
				677,
				297,
				677,
				298,
				677,
				673,
				677,
				672,
				677,
				671,
				677,
				675,
				677,
				674,
				677,
				362,
				677,
				363,
				677,
				364,
				677,
				365,
				677,
				608,
				677,
				609,
				677,
				602,
				677,
				603,
				677,
				611,
				677,
				689,
				677,
				148,
				677,
				149,
				677,
				46,
				677,
				303,
				677,
				337,
				677,
				540,
				677,
				299,
				677,
				538,
				677,
				55,
				677,
				607,
				677,
				615,
				677,
				625,
				677,
				626,
				677,
				688,
				677,
				361,
				677,
				687,
				677,
				484,
				677,
				604,
				677,
				358,
				677,
				355,
				677,
				616,
				677,
				617,
				677,
				654,
				677,
				653,
				677,
				655,
				677,
				585,
				677,
				584,
				677,
				583,
				677,
				595,
				677,
				596,
				677,
				600,
				677,
				597,
				677,
				598,
				677,
				599,
				677,
				357,
				677,
				377,
				677,
				606,
				677,
				359,
				677,
				360,
				677,
				367,
				677,
				366,
				677,
				300,
				677,
				610,
				677,
				612,
				677,
				487,
				677,
				486,
				677,
				485,
				677,
				669,
				677,
				356,
				677,
				661,
				677,
				374,
				677,
				442,
				677,
				443,
				677,
				444,
				677,
				601,
				677,
				445,
				677,
				592,
				677,
				446,
				677,
				605,
				677,
				447,
				677,
				627,
				677,
				539,
				677,
				613,
				677
			});

			// Token: 0x04006F58 RID: 28504
			public static int[] AttackTime = NPCID.Sets.Factory.CreateIntSet(-1, new int[]
			{
				38,
				34,
				17,
				34,
				107,
				60,
				19,
				40,
				22,
				30,
				124,
				34,
				228,
				40,
				178,
				24,
				18,
				34,
				229,
				60,
				209,
				60,
				54,
				60,
				108,
				30,
				160,
				60,
				20,
				600,
				369,
				34,
				453,
				34,
				368,
				60,
				207,
				15,
				227,
				60,
				208,
				34,
				142,
				34,
				441,
				15,
				353,
				12,
				633,
				12,
				550,
				34,
				588,
				20,
				663,
				60,
				638,
				-1,
				637,
				-1,
				656,
				-1,
				670,
				-1,
				678,
				-1,
				679,
				-1,
				680,
				-1,
				681,
				-1,
				682,
				-1,
				683,
				-1,
				684,
				-1
			});

			// Token: 0x04006F59 RID: 28505
			public static int[] AttackAverageChance = NPCID.Sets.Factory.CreateIntSet(1, new int[]
			{
				38,
				40,
				17,
				30,
				107,
				60,
				19,
				30,
				22,
				30,
				124,
				30,
				228,
				50,
				178,
				50,
				18,
				60,
				229,
				40,
				209,
				30,
				54,
				30,
				108,
				30,
				160,
				60,
				20,
				60,
				369,
				50,
				453,
				30,
				368,
				40,
				207,
				1,
				227,
				30,
				208,
				50,
				142,
				50,
				441,
				1,
				353,
				1,
				633,
				1,
				550,
				40,
				588,
				20,
				663,
				1,
				638,
				1,
				637,
				1,
				656,
				1,
				670,
				1,
				678,
				1,
				679,
				1,
				680,
				1,
				681,
				1,
				682,
				1,
				683,
				1,
				684,
				1
			});

			// Token: 0x04006F5A RID: 28506
			public static int[] AttackType = NPCID.Sets.Factory.CreateIntSet(-1, new int[]
			{
				38,
				0,
				17,
				0,
				107,
				0,
				19,
				1,
				22,
				1,
				124,
				0,
				228,
				1,
				178,
				1,
				18,
				0,
				229,
				1,
				209,
				1,
				54,
				2,
				108,
				2,
				160,
				2,
				20,
				2,
				369,
				0,
				453,
				0,
				368,
				1,
				207,
				3,
				227,
				1,
				208,
				0,
				142,
				0,
				441,
				3,
				353,
				3,
				633,
				0,
				550,
				0,
				588,
				0,
				663,
				2,
				638,
				-1,
				637,
				-1,
				656,
				-1,
				670,
				-1,
				678,
				-1,
				679,
				-1,
				680,
				-1,
				681,
				-1,
				682,
				-1,
				683,
				-1,
				684,
				-1
			});

			// Token: 0x04006F5B RID: 28507
			public static int[] PrettySafe = NPCID.Sets.Factory.CreateIntSet(-1, new int[]
			{
				19,
				300,
				22,
				200,
				124,
				200,
				228,
				300,
				178,
				300,
				229,
				300,
				209,
				300,
				54,
				100,
				108,
				100,
				160,
				100,
				20,
				200,
				368,
				200,
				227,
				200
			});

			// Token: 0x04006F5C RID: 28508
			public static Color[] MagicAuraColor = NPCID.Sets.Factory.CreateCustomSet<Color>(Color.White, new object[]
			{
				54,
				new Color(100, 4, 227, 127),
				108,
				new Color(255, 80, 60, 127),
				160,
				new Color(40, 80, 255, 127),
				20,
				new Color(40, 255, 80, 127),
				663,
				Main.hslToRgb(0.92f, 1f, 0.78f, 127)
			});

			// Token: 0x04006F5D RID: 28509
			public static bool[] DemonEyes = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				2,
				190,
				192,
				193,
				191,
				194,
				317,
				318
			});

			// Token: 0x04006F5E RID: 28510
			public static bool[] Zombies = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				3,
				132,
				186,
				187,
				188,
				189,
				200,
				223,
				161,
				254,
				255,
				52,
				53,
				536,
				319,
				320,
				321,
				332,
				436,
				431,
				432,
				433,
				434,
				435,
				331,
				430,
				590
			});

			// Token: 0x04006F5F RID: 28511
			public static bool[] Skeletons = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				77,
				449,
				450,
				451,
				452,
				481,
				201,
				202,
				203,
				21,
				324,
				110,
				323,
				293,
				291,
				322,
				292,
				197,
				167,
				44,
				635
			});

			// Token: 0x04006F60 RID: 28512
			public static int[] BossHeadTextures = NPCID.Sets.Factory.CreateIntSet(-1, new int[]
			{
				4,
				0,
				13,
				2,
				344,
				3,
				370,
				4,
				246,
				5,
				249,
				5,
				345,
				6,
				50,
				7,
				396,
				8,
				395,
				9,
				325,
				10,
				262,
				11,
				327,
				13,
				222,
				14,
				125,
				15,
				126,
				20,
				346,
				17,
				127,
				18,
				35,
				19,
				68,
				19,
				113,
				22,
				266,
				23,
				439,
				24,
				440,
				24,
				134,
				25,
				491,
				26,
				517,
				27,
				422,
				28,
				507,
				29,
				493,
				30,
				549,
				35,
				564,
				32,
				565,
				32,
				576,
				33,
				577,
				33,
				551,
				34,
				548,
				36,
				636,
				37,
				657,
				38,
				668,
				39
			});

			// Token: 0x04006F61 RID: 28513
			public static bool[] PositiveNPCTypesExcludedFromDeathTally = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				121,
				384,
				478,
				479,
				410,
				472,
				378
			});

			// Token: 0x04006F62 RID: 28514
			public static bool[] ShouldBeCountedAsBossForBestiary = NPCID.Sets.Factory.CreateBoolSet(false, new int[]
			{
				517,
				422,
				507,
				493,
				13,
				664
			});

			// Token: 0x04006F63 RID: 28515
			public static bool[] ShouldBeCountedAsBossForRainbowBoulders = NPCID.Sets.Factory.CreateBoolSet(false, new int[]
			{
				517,
				422,
				507,
				493,
				13,
				14,
				15,
				267,
				36,
				114,
				664,
				134,
				135,
				136,
				128,
				129,
				130,
				131,
				245,
				247,
				248,
				246,
				491,
				492,
				392,
				395,
				394,
				393,
				564,
				565,
				576,
				577,
				551,
				325,
				327,
				344,
				345,
				346
			});

			// Token: 0x04006F64 RID: 28516
			public static bool[] DangerThatPreventsOtherDangers = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				517,
				422,
				507,
				493,
				399,
				13,
				14,
				15
			});

			// Token: 0x04006F65 RID: 28517
			public static bool[] MustAlwaysDraw = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				113,
				114,
				115,
				116,
				126,
				125
			});

			// Token: 0x04006F66 RID: 28518
			public static int[] ExtraTextureCount = NPCID.Sets.Factory.CreateIntSet(0, new int[]
			{
				38,
				1,
				17,
				1,
				107,
				0,
				19,
				0,
				22,
				0,
				124,
				1,
				228,
				0,
				178,
				1,
				18,
				1,
				229,
				1,
				209,
				1,
				54,
				1,
				108,
				1,
				160,
				0,
				20,
				0,
				369,
				1,
				453,
				1,
				368,
				1,
				207,
				1,
				227,
				1,
				208,
				0,
				142,
				1,
				441,
				1,
				353,
				1,
				633,
				1,
				550,
				0,
				588,
				1,
				633,
				2,
				663,
				1,
				638,
				0,
				637,
				0,
				656,
				0,
				670,
				0,
				678,
				0,
				679,
				0,
				680,
				0,
				681,
				0,
				682,
				0,
				683,
				0,
				684,
				0
			});

			// Token: 0x04006F67 RID: 28519
			public static int[] NPCFramingGroup = NPCID.Sets.Factory.CreateIntSet(0, new int[]
			{
				18,
				1,
				20,
				1,
				208,
				1,
				178,
				1,
				124,
				1,
				353,
				1,
				633,
				1,
				369,
				2,
				160,
				3,
				637,
				4,
				638,
				5,
				656,
				6,
				670,
				7,
				678,
				7,
				679,
				7,
				680,
				7,
				681,
				7,
				682,
				7,
				683,
				7,
				684,
				7
			});

			// Token: 0x04006F68 RID: 28520
			public static bool[] CanHitPastShimmer = NPCID.Sets.Factory.CreateBoolSet(new int[]
			{
				535,
				5,
				13,
				14,
				15,
				666,
				267,
				36,
				210,
				211,
				115,
				116,
				117,
				118,
				119,
				658,
				659,
				660,
				134,
				135,
				136,
				139,
				128,
				131,
				129,
				130,
				263,
				264,
				246,
				249,
				247,
				248,
				371,
				372,
				373,
				566,
				567,
				440,
				522,
				523,
				521,
				454,
				455,
				456,
				457,
				458,
				459,
				397,
				396,
				400
			});

			// Token: 0x04006F69 RID: 28521
			public static int[][] TownNPCsFramingGroups = new int[][]
			{
				new int[]
				{
					0,
					0,
					0,
					-2,
					-2,
					-2,
					0,
					0,
					0,
					0,
					-2,
					-2,
					-2,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0
				},
				new int[]
				{
					0,
					0,
					0,
					-2,
					-2,
					-2,
					0,
					0,
					0,
					-2,
					-2,
					-2,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0
				},
				new int[]
				{
					0,
					0,
					0,
					-2,
					-2,
					-2,
					0,
					0,
					-2,
					-2,
					-2,
					-2,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0
				},
				new int[]
				{
					0,
					0,
					-2,
					0,
					0,
					0,
					0,
					-2,
					-2,
					-2,
					0,
					0,
					0,
					0,
					-2,
					-2,
					0,
					0,
					0,
					0,
					0,
					0
				},
				new int[]
				{
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					2,
					2,
					4,
					6,
					4,
					2,
					2,
					-2,
					-4,
					-6,
					-4,
					-2,
					-4,
					-4,
					-6,
					-6,
					-6,
					-4
				},
				new int[]
				{
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					-2,
					-2,
					-2,
					0,
					0,
					-2,
					-2,
					0,
					0,
					4,
					6,
					6,
					6,
					6,
					4,
					4,
					4,
					4,
					4,
					4
				},
				new int[]
				{
					0,
					0,
					-2,
					-4,
					-4,
					-2,
					0,
					-2,
					0,
					0,
					2,
					4,
					6,
					4,
					2,
					0,
					-2,
					-4,
					-6,
					-6,
					-6,
					-6,
					-6,
					-6,
					-4,
					-2
				},
				new int[]
				{
					0,
					-2,
					0,
					-2,
					-4,
					-6,
					-4,
					-2,
					0,
					0,
					2,
					2,
					4,
					2
				}
			};

			// Token: 0x02000AA1 RID: 2721
			public struct NPCBestiaryDrawModifiers
			{
				// Token: 0x06004BF2 RID: 19442 RVA: 0x006D8938 File Offset: 0x006D6B38
				public NPCBestiaryDrawModifiers(int seriouslyWhyCantStructsHaveParameterlessConstructors)
				{
					this.Position = default(Vector2);
					this.Rotation = 0f;
					this.Scale = 1f;
					this.PortraitScale = new float?((float)1);
					this.Hide = false;
					this.IsWet = false;
					this.Frame = null;
					this.Direction = null;
					this.SpriteDirection = null;
					this.Velocity = 0f;
					this.PortraitPositionXOverride = null;
					this.PortraitPositionYOverride = null;
					this.CustomTexturePath = null;
				}

				// Token: 0x040077E0 RID: 30688
				public Vector2 Position;

				// Token: 0x040077E1 RID: 30689
				public float? PortraitPositionXOverride;

				// Token: 0x040077E2 RID: 30690
				public float? PortraitPositionYOverride;

				// Token: 0x040077E3 RID: 30691
				public float Rotation;

				// Token: 0x040077E4 RID: 30692
				public float Scale;

				// Token: 0x040077E5 RID: 30693
				public float? PortraitScale;

				// Token: 0x040077E6 RID: 30694
				public bool Hide;

				// Token: 0x040077E7 RID: 30695
				public bool IsWet;

				// Token: 0x040077E8 RID: 30696
				public int? Frame;

				// Token: 0x040077E9 RID: 30697
				public int? Direction;

				// Token: 0x040077EA RID: 30698
				public int? SpriteDirection;

				// Token: 0x040077EB RID: 30699
				public float Velocity;

				// Token: 0x040077EC RID: 30700
				public string CustomTexturePath;
			}

			// Token: 0x02000AA2 RID: 2722
			public interface NPCPortraitProvider
			{
				// Token: 0x06004BF3 RID: 19443
				void GetDrawData(out Texture2D texture, out Rectangle drawFrame);
			}

			// Token: 0x02000AA3 RID: 2723
			public class BasicNPCPortrait : NPCID.Sets.NPCPortraitProvider
			{
				// Token: 0x06004BF4 RID: 19444 RVA: 0x006D89D0 File Offset: 0x006D6BD0
				public BasicNPCPortrait(string texturePath)
				{
					this.TexturePath = texturePath;
					this.HorizontalFrames = (this.VerticalFrames = 1);
					this.PaddingX = (this.PaddingY = 0);
					this.FrameX = (this.FrameY = 0);
				}

				// Token: 0x06004BF5 RID: 19445 RVA: 0x006D8A1C File Offset: 0x006D6C1C
				public virtual void GetDrawData(out Texture2D texture, out Rectangle drawFrame)
				{
					if (this._image == null)
					{
						this._image = Main.Assets.Request<Texture2D>(this.TexturePath, 1);
					}
					texture = null;
					if (this._image.IsLoaded)
					{
						texture = this._image.Value;
					}
					drawFrame = texture.Frame(this.HorizontalFrames, this.VerticalFrames, this.FrameX, this.FrameY, this.PaddingX, this.PaddingY);
				}

				// Token: 0x040077ED RID: 30701
				public string TexturePath;

				// Token: 0x040077EE RID: 30702
				public int HorizontalFrames;

				// Token: 0x040077EF RID: 30703
				public int VerticalFrames;

				// Token: 0x040077F0 RID: 30704
				public int PaddingX;

				// Token: 0x040077F1 RID: 30705
				public int PaddingY;

				// Token: 0x040077F2 RID: 30706
				public int FrameX;

				// Token: 0x040077F3 RID: 30707
				public int FrameY;

				// Token: 0x040077F4 RID: 30708
				private Asset<Texture2D> _image;
			}

			// Token: 0x02000AA4 RID: 2724
			public class NPCPortraitSelector : NPCID.Sets.NPCPortraitProvider
			{
				// Token: 0x06004BF6 RID: 19446 RVA: 0x006D8A98 File Offset: 0x006D6C98
				public NPCID.Sets.NPCPortraitSelector With(NPCID.Sets.NPCPortraitSelector.SelectionCondition condition, NPCID.Sets.NPCPortraitProvider portrait)
				{
					this._entries.Add(new NPCID.Sets.NPCPortraitSelector.Entry
					{
						Condition = condition,
						Portrait = portrait
					});
					return this;
				}

				// Token: 0x06004BF7 RID: 19447 RVA: 0x006D8ACC File Offset: 0x006D6CCC
				public NPCID.Sets.NPCPortraitSelector Default(NPCID.Sets.NPCPortraitProvider portrait)
				{
					List<NPCID.Sets.NPCPortraitSelector.Entry> entries = this._entries;
					NPCID.Sets.NPCPortraitSelector.Entry item = default(NPCID.Sets.NPCPortraitSelector.Entry);
					item.Condition = (() => true);
					item.Portrait = portrait;
					entries.Add(item);
					return this;
				}

				// Token: 0x06004BF8 RID: 19448 RVA: 0x006D8B1C File Offset: 0x006D6D1C
				public void GetDrawData(out Texture2D texture, out Rectangle drawFrame)
				{
					texture = null;
					drawFrame = default(Rectangle);
					NPCID.Sets.NPCPortraitProvider firstValidatedEntry = this.GetFirstValidatedEntry();
					if (firstValidatedEntry != null)
					{
						firstValidatedEntry.GetDrawData(out texture, out drawFrame);
					}
				}

				// Token: 0x06004BF9 RID: 19449 RVA: 0x006D8B48 File Offset: 0x006D6D48
				private NPCID.Sets.NPCPortraitProvider GetFirstValidatedEntry()
				{
					foreach (NPCID.Sets.NPCPortraitSelector.Entry entry in this._entries)
					{
						if (entry.Condition())
						{
							return entry.Portrait;
						}
					}
					return null;
				}

				// Token: 0x040077F5 RID: 30709
				private List<NPCID.Sets.NPCPortraitSelector.Entry> _entries = new List<NPCID.Sets.NPCPortraitSelector.Entry>();

				// Token: 0x02000B12 RID: 2834
				// (Invoke) Token: 0x06004DA9 RID: 19881
				public delegate bool SelectionCondition();

				// Token: 0x02000B13 RID: 2835
				public struct Entry
				{
					// Token: 0x040078DE RID: 30942
					public NPCID.Sets.NPCPortraitSelector.SelectionCondition Condition;

					// Token: 0x040078DF RID: 30943
					public NPCID.Sets.NPCPortraitProvider Portrait;
				}
			}

			// Token: 0x02000AA5 RID: 2725
			public class NPCVariantChecker
			{
				// Token: 0x06004BFB RID: 19451 RVA: 0x006D8BC3 File Offset: 0x006D6DC3
				public NPCVariantChecker(int variantToCheck)
				{
					this.VariantToCheck = variantToCheck;
				}

				// Token: 0x06004BFC RID: 19452 RVA: 0x006D8BD4 File Offset: 0x006D6DD4
				public bool Fits()
				{
					int talkNPC = Main.LocalPlayer.talkNPC;
					return talkNPC >= 0 && talkNPC < Main.maxNPCs && Main.npc[talkNPC].townNpcVariationIndex == this.VariantToCheck;
				}

				// Token: 0x040077F6 RID: 30710
				public int VariantToCheck;
			}

			// Token: 0x02000AA6 RID: 2726
			private static class LocalBuffID
			{
				// Token: 0x040077F7 RID: 30711
				public const int Confused = 31;

				// Token: 0x040077F8 RID: 30712
				public const int Poisoned = 20;

				// Token: 0x040077F9 RID: 30713
				public const int OnFire = 24;

				// Token: 0x040077FA RID: 30714
				public const int OnFire3 = 323;

				// Token: 0x040077FB RID: 30715
				public const int ShadowFlame = 153;

				// Token: 0x040077FC RID: 30716
				public const int Daybreak = 189;

				// Token: 0x040077FD RID: 30717
				public const int Frostburn = 44;

				// Token: 0x040077FE RID: 30718
				public const int Frostburn2 = 324;

				// Token: 0x040077FF RID: 30719
				public const int CursedInferno = 39;

				// Token: 0x04007800 RID: 30720
				public const int BetsysCurse = 203;

				// Token: 0x04007801 RID: 30721
				public const int Ichor = 69;

				// Token: 0x04007802 RID: 30722
				public const int Venom = 70;

				// Token: 0x04007803 RID: 30723
				public const int Oiled = 204;

				// Token: 0x04007804 RID: 30724
				public const int BoneJavelin = 169;

				// Token: 0x04007805 RID: 30725
				public const int TentacleSpike = 337;

				// Token: 0x04007806 RID: 30726
				public const int BloodButcherer = 344;

				// Token: 0x04007807 RID: 30727
				public const int Bleeding = 30;

				// Token: 0x04007808 RID: 30728
				public const int Hemorrhage = 375;
			}
		}
	}
}
