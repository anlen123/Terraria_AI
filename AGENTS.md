# Terraria 项目 AGENTS.md

## 项目概述

这是一个 **Terraria（泰拉瑞亚）** 的官方源代码项目，由 Re-Logic 开发的 2D 沙盒冒险游戏。这是一个基于 XNA Framework 构建的商业游戏项目，使用 C# 语言开发。

### 主要技术栈

- **语言**: C# (.NET Framework 4.0)
- **游戏框架**: Microsoft.Xna.Framework (XNA Game Studio 4.0)
- **构建系统**: MSBuild
- **平台**: Windows (x86)
- **第三方库**:
  - ReLogic (Re-Logic 自定义库)
  - Steamworks.NET (Steam 集成)
  - Newtonsoft.Json (JSON 处理)
  - Ionic.Zip.CF (ZIP 压缩)
  - CsvHelper (CSV 处理)
  - MP3Sharp, NVorbis (音频解码)
  - RailSDK.Net (Razer Chroma RGB 支持)
  - SteelSeriesEngineWrapper (SteelSeries 设备支持)

### 项目架构

项目采用经典的 XNA Game 架构，主要组件包括：

- **Main.cs** - 核心游戏类，继承自 `Game` 类，包含游戏主循环和全局状态管理
- **Program.cs** - 程序入口点，处理启动参数和初始化
- **WindowsLaunch.cs** - Windows 平台启动器，处理程序集解析和控制台事件

### 目录结构

```
Terraria/
├── Achievements/          # 成就系统
├── Audio/                 # 音频系统（音效、音乐、音轨）
├── Chat/                  # 聊天系统和命令
├── Cinematics/            # 过场动画
├── DataStructures/        # 数据结构和实体源
├── Enums/                 # 枚举定义
├── GameContent/           # 游戏内容（生物、物品、世界生成等）
├── GameInput/             # 游戏输入处理
├── Graphics/              # 图形渲染
├── ID/                    # 各种 ID 定义
├── Initializers/          # 初始化器
├── IO/                    # 输入输出操作
├── Libraries/             # 第三方库
├── Localization/          # 本地化
├── Map/                   # 地图系统
├── Modules/               # 游戏模块
├── Net/                   # 网络通信
├── ObjectData/            # 对象数据
├── Physics/               # 物理系统
├── Properties/            # 项目属性
├── Server/                # 服务器相关
├── Social/                # 社交平台集成
├── Testing/               # 测试代码
├── UI/                    # 用户界面
├── Utilities/             # 工具类
├── WorldBuilding/         # 世界生成
└── *.cs                   # 核心游戏类文件
```

## 构建和运行

### 构建命令

使用 MSBuild 构建项目：

```bash
# Debug 配置
msbuild Terraria.csproj /p:Configuration=Debug /p:Platform=x86

# Release 配置
msbuild Terraria.csproj /p:Configuration=Release /p:Platform=x86
```

或在 Visual Studio 中：
1. 打开 `Terraria.csproj`
2. 选择配置（Debug/Release）
3. 选择平台（x86）
4. 按 F6 或选择"生成" → "生成解决方案"

### 运行参数

程序支持以下启动参数：

| 参数 | 说明 |
|------|------|
| `-savedirectory` | 指定存档目录 |
| `-logfile` | 启用日志文件 |
| `-minidump` | 启用崩溃转储 |
| `-logerrors` | 记录所有异常 |
| `-fulldump` | 启用完整内存转储 |
| `-disableannouncementbox` | 禁用公告盒 |
| `-announcementboxrange` | 设置公告盒范围 |

### 专用服务器

要运行专用服务器：

```bash
Terraria.exe -config serverconfig.txt
```

## 开发约定

### 代码风格

- **命名约定**: 遵循 Microsoft C# 命名约定
  - 类名: PascalCase (如 `Main`, `Player`)
  - 方法: PascalCase (如 `Update`, `Draw`)
  - 局部变量: camelCase (如 `player`, `npc`)
  - 常量: PascalCase (如 `MaxPlayers`)

- **格式**: 使用标准缩进（通常 4 空格）

### 架构模式

- **静态类**: 大量使用静态类和静态方法（如 `Main`, `Program`）
- **单例模式**: `Main.instance` 作为全局游戏实例
- **事件驱动**: 使用事件系统处理游戏生命周期
  - `OnEngineLoad`
  - `OnPreDraw`
  - `OnPostDraw`
  - `OnTickForThirdPartySoftwareOnly`

### 安全性

- **允许不安全代码**: 项目允许使用 `unsafe` 代码块
- **P/Invoke**: 使用平台调用与 Windows API 交互

### 扩展性

项目为模组开发提供扩展点：
- `IEntitySource` 接口用于跟踪实体创建来源
- `IChatCommand` 接口用于自定义聊天命令
- `ICreativePower` 接口用于创意模式权限

### 游戏难度等级

- **Classic** (0): 经典模式
- **Expert** (1): 专家模式
- **Master** (2): 大师模式
- **Journey** (3): 旅途模式
- **Legendary** (4): 传奇模式（仅用于"福星高照"种子）

### 特殊世界种子

项目支持多种特殊世界种子：
- `drunkWorld`: 醉酒世界
- `getGoodWorld`: 福星高照
- `tenthAnniversaryWorld`: 十周年世界
- `notTheBeesWorld`: 不是蜜蜂世界
- `dontStarveWorld`: 饥饿世界
- `remixWorld`: 混音世界
- `noTrapsWorld`: 无陷阱世界
- `zenithWorld`: 天顶世界
- `skyblockWorld`: 天空岛世界

## 关键文件说明

| 文件 | 说明 |
|------|------|
| `Main.cs` | 游戏核心类，包含游戏循环、渲染、更新逻辑 |
| `Program.cs` | 程序入口，处理启动参数和初始化 |
| `WindowsLaunch.cs` | Windows 平台启动器 |
| `Player.cs` | 玩家类 |
| `NPC.cs` | NPC 类 |
| `Item.cs` | 物品类 |
| `Projectile.cs` | 投射物类 |
| `Tile.cs` | 方块类 |
| `WorldGen.cs` | 世界生成器 |
| `Terraria.csproj` | 项目配置文件 |
| `app.manifest` | 应用程序清单 |

## 注意事项

1. **平台限制**: 项目仅支持 Windows x86 平台
2. **XNA 依赖**: 需要安装 XNA Framework 4.0
3. **不安全代码**: 项目大量使用不安全代码，需要 `AllowUnsafeBlocks`
4. **商业项目**: 这是官方商业游戏源码，需遵守相关许可协议
5. **Steam 集成**: 默认集成 Steamworks，需要 Steam 环境
6. **大型项目**: 超过 70,000 行代码的 Main.cs 反映了项目的复杂性

## 常见任务

### 添加新物品

1. 在 `GameContent/Items/` 中定义物品行为
2. 在 `ID/ItemID.cs` 中分配 ID
3. 在 `GameContent/ItemDropRules/` 中定义掉落规则
4. 添加本地化文本

### 添加新 NPC

1. 在 `GameContent/NPCs/` 中定义 NPC 类
2. 在 `ID/NPCID.cs` 中分配 ID
3. 配置生成规则和 AI
4. 添加纹理和动画

### 修改世界生成

1. 编辑 `WorldGen.cs` 中的生成逻辑
2. 使用 `GameContent/Generation/` 中的生成工具
3. 在 `WorldBuilding/` 中添加自定义生成器

### 添加聊天命令

1. 在 `Chat/Commands/` 中实现 `IChatCommand` 接口
2. 使用 `ChatCommandAttribute` 标记命令
3. 在 `ChatCommandProcessor.cs` 中注册命令