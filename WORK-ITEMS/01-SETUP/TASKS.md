# Day 1: Setup & Core Structure

## Objectives
- Initialize .NET C# project in VS Code
- Create basic project structure
- Implement core game loop
- Setup development environment

## Tasks

### 1. Project Initialization
- [x] Create project directory structure
- [ ] Initialize .NET console application (`dotnet new console`)
- [ ] Configure VS Code settings
- [ ] Setup C# extension configuration

### 2. Core Project Structure
```
src/
├── Program.cs              # Main entry point
├── Core/
│   ├── Game.cs            # Main game class
│   ├── GameState.cs       # Game state management
│   └── MenuSystem.cs      # Menu navigation
├── Models/
│   ├── Player.cs          # Player data model
│   ├── Car.cs             # Car state model
│   └── RallyStage.cs      # Stage data model
├── Utils/
│   ├── ConsoleHelper.cs   # Console utility functions
│   └── Constants.cs       # Game constants
└── Tests/
    └── BasicTests.cs      # Basic unit tests
```

### 3. Basic Game Loop Implementation
- [ ] Main menu system
- [ ] Game state management (Menu, Playing, GameOver)
- [ ] Basic console input/output handling
- [ ] Exit functionality

### 4. Console UI Framework
- [ ] Color-coded text output
- [ ] ASCII art car representation
- [ ] Clear screen functionality
- [ ] Input validation system

## Definition of Done
- [x] Project structure created
- [ ] .NET project compiles and runs
- [ ] Basic menu navigation works
- [ ] Console displays properly formatted text
- [ ] Can exit game cleanly

## Testing Criteria
- [ ] Application starts without errors
- [ ] Menu navigation responds to user input
- [ ] Console text is readable and properly formatted
- [ ] Application exits cleanly

## Estimated Time: 2-3 hours

## Notes
- Keep it simple for MVP - focus on functionality over visuals
- Ensure good code structure for easy feature additions
- Test frequently during development
