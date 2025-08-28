# Day 3: Racing Mechanics Implementation

## Objectives
- Create race progression system tied to math performance
- Implement car breakdown mechanics (3 strikes rule)
- Integrate story problems for car repairs
- Build rally stage structure

## Tasks

### 1. Race Progression System
- [ ] Create race progress tracking (0-100% completion)
- [ ] Link correct answers to forward progress
- [ ] Wrong answers slow progress or cause setbacks
- [ ] Visual representation of race progress (ASCII progress bar)
- [ ] Time tracking for each stage

### 2. Car Breakdown Mechanics
- [ ] Track wrong answers (strike system)
- [ ] **Strike 1**: Warning message, slight slowdown
- [ ] **Strike 2**: More severe warning, bigger slowdown  
- [ ] **Strike 3**: Car "breaks down" - must repair
- [ ] Car repair requires solving story problem
- [ ] Reset strikes after successful repair

### 3. Story Problem Integration
- [ ] Trigger story problems on car breakdown
- [ ] Story problems must be solved to continue racing
- [ ] Story problem difficulty matches or slightly exceeds current level
- [ ] Successful repair restores car and resets strikes
- [ ] Failed story problem = race over (game over condition)

### 4. Rally Stage Structure
- [ ] Each stage has:
  - [ ] Name and theme (Forest Rally, Desert Rally, etc.)
  - [ ] Target number of problems to complete
  - [ ] Minimum accuracy requirement
  - [ ] Time bonus objectives
- [ ] Stage completion criteria
- [ ] Transition between stages

### 5. Racing Feedback System
- [ ] Real-time feedback during race:
  - [ ] Speed indicators based on answer speed
  - [ ] Progress visualization
  - [ ] Strike warnings
  - [ ] Encouragement messages
- [ ] ASCII art car representation showing damage/health

### 6. Game States During Racing
```
Racing States:
- RACING: Normal problem-solving
- BREAKDOWN: Car repair story problem
- STAGE_COMPLETE: Stage finished successfully  
- GAME_OVER: Too many breakdowns or failed repair
```

## File Structure
```
src/Racing/
├── RaceManager.cs         # Main racing logic coordinator
├── ProgressTracker.cs     # Tracks race completion %
├── CarBreakdownSystem.cs  # Manages strikes and breakdowns
├── StageManager.cs        # Handles stage progression
└── RacingDisplay.cs       # Console output for racing
```

## Race Flow
1. **Start Stage**: Display stage info, initialize progress
2. **Racing Loop**:
   - Present math problem
   - Get user answer
   - Update progress based on correctness/speed
   - Check for strikes/breakdown
   - Display current state
   - Repeat until stage complete or game over
3. **Breakdown Handling**:
   - Stop normal racing
   - Present story problem
   - Handle repair attempt
   - Continue or end game
4. **Stage Completion**: Show results, advance to next stage

## Definition of Done
- [ ] Race progress advances with correct answers
- [ ] Car breakdown system works (3 strikes)
- [ ] Story problems trigger on breakdown
- [ ] Can complete or fail a rally stage
- [ ] Proper transitions between game states
- [ ] Clear visual feedback throughout race

## Testing Criteria
- [ ] Complete a stage with all correct answers
- [ ] Trigger breakdown with 3 wrong answers
- [ ] Successfully repair car with story problem
- [ ] Fail story problem and get game over
- [ ] Progress bar updates correctly

## Estimated Time: 4-5 hours

## Integration Points
- Uses Math Engine from Day 2
- Provides data for Rally Series system (Day 4)
- Feeds into Parent/Kid mode analytics (Day 5)
