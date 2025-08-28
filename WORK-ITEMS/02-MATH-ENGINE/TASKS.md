# Day 2: Math Engine Development

## Objectives
- Create math problem generation system
- Implement difficulty scaling
- Build answer validation system
- Allow math type selection

## Tasks

### 1. Math Problem Generator
- [ ] Create `MathProblem` class to represent problems
- [ ] Implement problem generation for each operation:
  - [ ] Addition (single/double digit based on difficulty)
  - [ ] Subtraction (no negative results for young kids)
  - [ ] Multiplication (times tables 1-12)
  - [ ] Division (with and without remainders)
- [ ] Random problem selection within difficulty constraints

### 2. Difficulty System
**Rally Series 1: "Rookie Rally" (Ages 5-7)**
- Single digit addition (1+3, 5+2)
- Single digit subtraction (7-4, 9-5) 
- Simple counting challenges

**Rally Series 2: "Junior Championship" (Ages 7-9)**
- Double digit addition without carrying (23+14)
- Double digit subtraction without borrowing (45-23)
- Times tables 1-5
- Simple division (10÷2, 15÷3)

**Rally Series 3: "Pro Circuit" (Ages 9-12)**
- Addition/subtraction with carrying/borrowing
- Times tables up to 12
- Division with remainders
- Simple fractions (1/2, 1/4)

### 3. Problem Selection System
- [ ] Allow user to select math types at start:
  - [ ] Addition only
  - [ ] Subtraction only  
  - [ ] Multiplication only
  - [ ] Division only
  - [ ] Mixed problems (random selection)
- [ ] Gradual scaling within selected types

### 4. Answer Validation
- [ ] Check user input against correct answer
- [ ] Handle invalid input (non-numbers, empty input)
- [ ] Provide immediate feedback (correct/incorrect)
- [ ] Track accuracy statistics

### 5. Story Problems for Car Repairs
- [ ] Create car-themed word problems
- [ ] Examples:
  - "Your car needs 4 wheels, one fell off, how many do you need?"
  - "You have 8 spark plugs, used 3, how many are left?"
  - "Each wheel needs 5 bolts, you have 2 wheels, how many bolts total?"
- [ ] Make story problems slightly harder than current racing problems

## File Structure
```
src/Math/
├── MathProblem.cs         # Problem data model
├── ProblemGenerator.cs    # Generates problems by type/difficulty
├── DifficultyManager.cs   # Manages difficulty progression
├── AnswerValidator.cs     # Validates user answers
└── StoryProblemGenerator.cs # Car repair story problems
```

## Definition of Done
- [ ] Can generate problems for all 4 operations
- [ ] Difficulty scales appropriately for each rally series
- [ ] User can select which math types to practice
- [ ] Answer validation works correctly
- [ ] Story problems generate for car repairs
- [ ] Math statistics are tracked

## Testing Criteria
- [ ] Generate 20 problems of each type without errors
- [ ] Difficulty progression works correctly
- [ ] Invalid answers are handled gracefully
- [ ] Story problems are car-themed and appropriate difficulty

## Estimated Time: 3-4 hours

## Integration Points
- Integrates with racing mechanics (Day 3)
- Used by progress tracking system (Day 5)
