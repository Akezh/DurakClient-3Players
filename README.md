# Multiplayer Online Card Game "Durak"

![Durak](https://github.com/user-attachments/assets/bd56973c-4fc5-4603-ae0a-21ea2ddc2638)

## Overview

This repository contains the core game logic for a **multiplayer online card game** developed using **Unity**. The game is designed to support 3 players in real-time matches, featuring automated card distribution, advanced role management, and sophisticated game logic. With over 4,000 monthly players, the game was ranked among the top 3 in "Editors' Choice" on Google Play, and over a million game lobbies were created within the first year of its release.

## Server project
- Visit https://github.com/Akezh/DurakServer-3Players/

## Installation

1. Clone the repository:
    ```bash
    git clone https://github.com/username/multiplayer-card-game.git
    ```
2. Open the project in Unity.

## Technology Stack

- **Unity**: For rendering the game and managing game logic.
- **C#**: Core programming language used for game logic.
- **ASP.NET Core**: Backend technology handling user authentication and matchmaking.
- **gRPC**: For handling real-time, low-latency communication between players.

## Game Logic

The game follows traditional rules for card-based strategy games, where players take turns playing cards in a set order. The game handles automated decision-making for situations where human input is not required, ensuring a smooth and balanced experience.

### Core Game Logic Includes:

- **Random Card Distribution**: Cards are randomly distributed to players while ensuring fairness.
- **Turn-Based Mechanism**: A robust system for handling player turns and moves.
- **Card Validations**: Real-time validation of player actions according to game rules.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
