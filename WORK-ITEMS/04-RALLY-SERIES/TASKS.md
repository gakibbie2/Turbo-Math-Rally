# Day 4: Rally Series & Progression System

## Objectives
- Implement multiple rally series with increasing difficulty
- Create stage progression logic
- Build championship/series completion system
- Handle win/lose conditions across series

## Tasks

### 1. Rally Series Structure

**Rally Series 1: "Rookie Rally" (Ages 5-7)**
- **Stages**: Forest Rally, Park Rally, Beach Rally (3 stages)
- **Problems per stage**: 8-12 problems
- **Math focus**: Single digit addition/subtraction
- **Success criteria**: 70% accuracy minimum

**Rally Series 2: "Junior Championship" (Ages 7-9)** 
- **Stages**: Mountain Rally, Desert Rally, City Rally, Snow Rally (4 stages)
- **Problems per stage**: 12-15 problems  
- **Math focus**: Double digit operations, simple multiplication/division
- **Success criteria**: 75% accuracy minimum

**Rally Series 3: "Pro Circuit" (Ages 9-12)**
- **Stages**: Extreme Forest, Canyon Rally, Night Rally, Championship Final (4 stages)
- **Problems per stage**: 15-20 problems
- **Math focus**: Complex operations, fractions
- **Success criteria**: 80% accuracy minimum

### 2. Series Progression Logic
- [ ] Track current series and stage
- [ ] Unlock next stage after completing current stage
- [ ] Unlock next series after completing all stages in current series
- [ ] Handle series completion celebration
- [ ] Reset progress option

### 3. Stage Configuration System
- [ ] Create `RallyStage` data structure with:
  - [ ] Stage name and description
  - [ ] Problem count requirements
  - [ ] Accuracy thresholds
  - [ ] Time bonus targets
  - [ ] Math difficulty level
  - [ ] Theme/environment description

### 4. Difficulty Scaling Between Stages
- [ ] Gradual increase in problem difficulty within series
- [ ] Faster time requirements for later stages
- [ ] Higher accuracy requirements for advanced stages
- [ ] More complex story problems in later series

### 5. Championship System
- [ ] Track performance across entire series:
  - [ ] Total time for series completion
  - [ ] Overall accuracy percentage
  - [ ] Number of breakdowns
  - [ ] Stages completed without breakdown
- [ ] Series completion rewards/celebrations
- [ ] Leaderboard for family members (local only for MVP)

### 6. Win/Lose Conditions

**Stage Win Conditions:**
- Complete required number of problems
- Achieve minimum accuracy threshold
- Successfully repair car if breakdowns occur

**Stage Lose Conditions:**
- Fail car repair story problem
- Give up/quit stage

**Series Win Conditions:**
- Complete all stages in series
- Maintain minimum series accuracy

## File Structure
```
src/Rally/
├── RallySeries.cs         # Series data and configuration
├── RallyStage.cs          # Individual stage data
├── SeriesManager.cs       # Series progression logic
├── ChampionshipTracker.cs # Cross-stage performance tracking
└── StageConfiguration.cs  # Stage setup and difficulty
```

## Stage Data Example
```csharp
public class RallyStage 
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int ProblemCount { get; set; }
    public double MinAccuracy { get; set; }
    public int TimeBonus { get; set; }
    public DifficultyLevel Difficulty { get; set; }
    public MathOperationType[] AllowedOperations { get; set; }
}
```

## User Experience Flow
1. **Series Selection**: Choose or view current series
2. **Stage Selection**: View available stages (locked/unlocked)
3. **Pre-Race**: Stage briefing and objectives
4. **Racing**: Complete stage using racing mechanics from Day 3
5. **Post-Race**: Results, unlock next stage, series progress
6. **Series Complete**: Celebration, unlock next series

## Definition of Done
- [ ] All 3 rally series properly configured
- [ ] Stage progression works correctly
- [ ] Difficulty scales appropriately between series
- [ ] Series completion detection works
- [ ] Win/lose conditions function properly
- [ ] Performance tracking across stages

## Testing Criteria
- [ ] Complete Rookie Rally series successfully
- [ ] Fail a stage and retry mechanism works
- [ ] Progression to Junior Championship after Rookie completion
- [ ] Difficulty increase is noticeable between series
- [ ] Performance statistics track correctly

## Estimated Time: 3-4 hours

## Integration Points
- Uses Racing Mechanics from Day 3
- Uses Math Engine from Day 2
- Provides data for Parent/Kid modes (Day 5)
