# 🎰 Slot Machine Game

A 2D Slot Machine game developed using **Unity 6.3 LTS and C#**. The project recreates a classic casino-style slot machine with animated spinning reels, symbol matching, gold betting, and reward calculations.

## 🎮 Game Overview

The player starts with 500 Gold (G) and places bets to spin three reels. Each reel randomly selects one of four symbols. Players earn rewards by matching symbols or landing two Cherries.

## ✨ Features

- **Three Spinning Reels** – Independent reels with staggered stopping times.
- **Four Slot Symbols** – Seven, Cherry, Bell, and BAR.
- **Smooth Reel Animation** – Continuous scrolling with looping and bounce effects.
- **Betting System** – Multiple bet options.
- **Gold Management** – Gold is deducted when placing bets and rewarded based on results.
- **Winning Conditions** – Three matching symbols or two Cherries.
- **Lever Animation** – Lever pull and return animation.
- **UI Feedback** – Displays player gold and game results.

## 🎰 Slot Symbols

| Symbol ID | Symbol | Description |
|---|---|---|
| 0 | Seven (7) | Jackpot |
| 1 | Cherry | Cherry reward |
| 2 | Bell | Bell reward |
| 3 | BAR | BAR reward |

## 🏆 Winning Rules

The game uses the following payout multipliers:

| Winning Combination | Payout |
|---|---:|
| 3 Sevens | 50x Bet |
| 3 BARs | 10x Bet |
| 3 Bells | 5x Bet |
| 3 Cherries | 3x Bet |
| 2 Cherries | 1x Bet (Bet Refund) |

**Example:**

If the player places a 10G bet:

- 3 Sevens → 500G reward.
- 3 BARs → 100G reward.
- 3 Bells → 50G reward.
- 3 Cherries → 30G reward.
- 2 Cherries → 10G refund.

## 🛠️ Technologies Used

- Unity 6.3 LTS
- C#
- Unity UI (Canvas)
- TextMeshPro
- Unity Coroutines
- Sprite Editor
- RectTransform
- Vertical Layout Group

## 📂 Project Structure

```text
Assets/
│
├── Art/
│   ├── SlotMachine/
│   └── Symbols/
│
├── Prefabs/
│   ├── Reels/
│   └── UI/
│
├── Scenes/
│   └── SampleScene.unity
│
├── Scripts/
│   ├── SlotMachineManager.cs
│   ├── ReelController.cs
│   └── LeverController.cs
│
└── UI/
    ├── Canvas
    ├── GoldText
    ├── StatusText
    └── BetButtons
```

*The folder structure above is a suggested organization.*

## 🧠 Core Systems

### 1. SlotMachineManager

Responsible for managing the overall game logic.

Responsibilities:
- Manages player gold.
- Handles betting.
- Starts the spinning round.
- Controls staggered reel stopping.
- Checks winning combinations.
- Calculates rewards.
- Updates UI elements.

### 2. ReelController

Responsible for individual reel animations.

Responsibilities:
- Controls reel spinning.
- Scrolls symbols vertically.
- Handles continuous looping.
- Stops at the selected symbol.
- Adds a bounce effect when stopping.
- Tracks the current symbol index.

### 3. LeverController

Responsible for the slot machine lever interaction and animation.

Responsibilities:
- Plays the lever pull animation.
- Handles lever return animation.

## ⚙️ Setup Instructions

1. Clone or download this repository.
2. Open the project using Unity Hub.
3. Open the project in Unity 6.3 LTS.
4. Open `SampleScene`.
5. Assign the three ReelController references to SlotMachineManager.
6. Assign the LeverController reference.
7. Connect Gold Text and Status Text using TextMeshPro.
8. Assign the betting buttons.
9. Configure each reel's ReelStrip reference.
10. Configure the lever animation.

### Reel Configuration

Each reel uses a `RectTransform` containing four symbol images.

The symbol order must be:

1. Seven
2. Cherry
3. Bell
4. BAR

The symbol order is important because the game uses symbol IDs to determine winning combinations.

## 🎨 Animation Settings

The ReelController provides the following configurable parameters:

| Parameter | Description |
|---|---|
| Symbol Height | Height of each symbol |
| Total Unique Symbols | Number of unique symbols |
| Vertical Offset | Fine-tunes symbol alignment |
| Spin Speed | Controls reel scrolling speed |
| Bounce Offset | Controls stopping overshoot |
| Bounce Duration | Controls bounce animation duration |

## 🎯 Future Improvements

- Sound effects for spinning and winning.
- Background music.
- Improved jackpot animation.
- More slot symbols.
- Persistent player gold using save data.
- Improved UI and game effects.
- Mobile platform support.

## 👨‍💻 Development

This project was created as a Unity game development practice project to explore:

- Object-oriented programming in C#.
- Unity UI systems.
- Coroutine-based animations.
- Game logic and reward calculations.
- Sprite management.
- Component-based architecture.

---

**Engine:** Unity 6.3 LTS  
**Language:** C#  
**Project Type:** 2D Slot Machine Game  
**Status:** In Development

