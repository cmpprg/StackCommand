# Stack Command - Development Roadmap
## Current Work: 
### Unit Selection System - This is fundamental for an RTS:

- ~~Single unit selection with mouse click~~
- ~~Multiple unit selection with drag box~~
- ~~Selection visual feedback~~


### Basic Movement System:

- ~~Click-to-move functionality~~
- ~~Pathfinding on terrain~~
- Movement animation/feedback


### Camera Control System:

- RTS-style camera movement with edge scrolling
- Zoom functionality
- Camera rotation (optional)

## Epic 1: Core Game Framework
### Stories
1. Set up basic Unity project with required packages[Complete]
2. Implement basic grid system for game world
3. Create basic unit movement system
4. Implement camera controls and user input handling
5. Set up basic UI framework

## Epic 2: Unit System
### Stories
1. Create basic unit prefab with properties[Complete]
2. Implement unit movement and pathfinding
3. Create unit selection system
4. Implement basic combat system
5. Add unit generation timing system
6. Create unit stacking mechanics
7. Implement stack-based combat modifications
8. Add visual feedback for unit states

## Epic 3: Building System
### Stories
1. Create home base implementation
   - Unit generation
   - Health system
   - Victory/defeat conditions
2. Implement Unit Generator
   - Production queue
   - Resource management
3. Create Unit Fuser
   - Formation patterns
   - Fusion mechanics
4. Implement Unit Stacker
   - Stacking logic
   - Upgrade system

## Epic 4: Territory System
### Stories
1. Implement territory grid system
2. Create territory capture mechanics
3. Add territory influence system
   - Healing mechanism
   - Damage mechanism
4. Implement territory visualization
5. Add territory control UI

## Epic 5: Combat System
### Stories
1. Implement range-based combat
2. Create line of sight system
3. Add stack-based attack limitations
4. Implement damage calculation
5. Add combat visual effects
6. Create unit death/destruction system

## Epic 6: UI/UX
### Stories
1. Create main menu
2. Implement in-game HUD
3. Add building placement interface
4. Create unit information display
5. Implement territory control visualization
6. Add victory/defeat screens

## Epic 7: Game Balance & Polish
### Stories
1. Balance unit generation times
2. Adjust combat values
3. Fine-tune territory effects
4. Balance building costs
5. Add sound effects and music
6. Implement particle effects
7. Add game options and settings

## Epic 8: Testing & Deployment
### Stories
1. Implement automated tests
2. Perform performance optimization
3. Add save/load system
4. Create build pipeline
5. Platform-specific testing
6. Create installation packages

## Suggested Development Order:
1. Start with Epic 1 to create the foundation
2. Move to basic unit mechanics from Epic 2
3. Implement home base from Epic 3
4. Add basic combat from Epic 5
5. Create essential UI elements from Epic 6
6. Continue with remaining features in order of dependency