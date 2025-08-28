# WI-002-0-Core-Game-Structure

## Priority: 0 (Critical - MVP)
## Estimated Time: 1 hour
## Day: 1

## Description
Create the foundational game architecture with main game loop, state management, and basic menu system.

## Acceptance Criteria
- [ ] Main game class with game loop
- [ ] Game state enumeration (Menu, Playing, GameOver)
- [ ] Basic menu navigation system
- [ ] Clean exit functionality
- [ ] Console clear and display management

## Tasks
- [ ] Create `Game.cs` main game class
- [ ] Implement `GameState` enumeration
- [ ] Create `MenuSystem.cs` for navigation
- [ ] Implement main game loop
- [ ] Add console utilities for display management

## File Structure
```
src/
├── Core/
│   ├── Game.cs
│   ├── GameState.cs
│   └── MenuSystem.cs
└── Utils/
    └── ConsoleHelper.cs
```

## Dependencies
- WI-001-0-Project-Setup

## Definition of Done
- Can navigate main menu
- Can exit game cleanly
- Game loop manages state transitions properly
