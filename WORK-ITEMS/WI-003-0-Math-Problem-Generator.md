# WI-003-0-Math-Problem-Generator

## Priority: 0 (Critical - MVP)
## Estimated Time: 1.5 hours
## Day: 2

## Description
Create math problem generation system for all four basic operations with age-appropriate difficulty levels.

## Acceptance Criteria
- [x] Generate addition problems (single/double digit)
- [x] Generate subtraction problems (no negative results)
- [x] Generate multiplication problems (times tables 1-12)
- [x] Generate division problems (with/without remainders)
- [x] Problem difficulty scales with age groups
- [x] Random problem generation within constraints

## Tasks
- [x] Create `MathProblem` data model
- [x] Implement `ProblemGenerator` class
- [x] Add difficulty level configurations
- [x] Create problem validation logic
- [x] Test problem generation for all operations

## Age Group Configurations
**Ages 5-7 (Rookie Rally):**
- Addition: 1+3, 5+2 (single digit)
- Subtraction: 7-4, 9-5 (single digit)

**Ages 7-9 (Junior Championship):**
- Addition: 23+14 (no carrying)
- Subtraction: 45-23 (no borrowing)
- Multiplication: times tables 1-5
- Division: 10÷2, 15÷3 (simple)

**Ages 9-12 (Pro Circuit):**
- Addition/Subtraction with carrying/borrowing
- Multiplication: times tables up to 12
- Division with remainders
- Simple fractions: 1/2, 1/4

## Dependencies
- WI-002-0-Core-Game-Structure

## Definition of Done
- Can generate 20+ problems of each type without errors
- Difficulty progression works correctly
- Problem validation prevents invalid scenarios
