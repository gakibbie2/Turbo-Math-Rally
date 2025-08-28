# WI-005-0-Car-Breakdown-System

## Priority: 0 (Critical - MVP)
## Estimated Time: 1 hour
## Day: 3

## Description
Implement the 3-strike car breakdown system where wrong answers lead to car repairs.

## Acceptance Criteria
- [ ] Track wrong answers as "strikes"
- [ ] Strike 1: Warning message, slight slowdown
- [ ] Strike 2: More severe warning, bigger slowdown
- [ ] Strike 3: Car "breaks down" - must repair
- [ ] Reset strikes after successful repair
- [ ] Game over if repair fails

## Tasks
- [ ] Create `CarBreakdownSystem` class
- [ ] Implement strike tracking
- [ ] Add progressive warning system
- [ ] Create breakdown trigger logic
- [ ] Add strike reset functionality

## Dependencies
- WI-004-0-Answer-Validation-System

## Definition of Done
- Wrong answers increment strike counter
- Appropriate warnings show at each strike level
- Car breakdown triggers at 3 strikes
- Strike counter resets after successful repair
