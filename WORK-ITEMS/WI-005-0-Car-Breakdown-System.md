# WI-005-0-Car-Breakdown-System

## Priority: 0 (Critical - MVP)
## Estimated Time: 1 hour
## Day: 3

## Description
Implement the 3-strike car breakdown system where wrong answers lead to car repairs.

## Acceptance Criteria
- [x] Track wrong answers as "strikes"
- [x] Strike 1: Warning message, slight slowdown
- [x] Strike 2: More severe warning, bigger slowdown
- [x] Strike 3: Car "breaks down" - must repair
- [x] Reset strikes after successful repair
- [x] Game over if repair fails

## Tasks
- [x] Create `CarBreakdownSystem` class
- [x] Implement strike tracking
- [x] Add progressive warning system
- [x] Create breakdown trigger logic
- [x] Add strike reset functionality

## Dependencies
- WI-004-0-Answer-Validation-System ✅

## Definition of Done
- Wrong answers increment strike counter ✅
- Appropriate warnings show at each strike level ✅
- Car breakdown triggers at 3 strikes ✅
- Strike counter resets after successful repair ✅

## Status: ✅ COMPLETE
