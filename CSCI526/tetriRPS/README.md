# TetriRPS

## Play the Game

You can play the game here: [TetriRPS Game](https://swathirhande.github.io/TetriRPS/)

## Game Overview

**TetriRPS** is a thrilling and innovative 2D puzzle game that combines classic Tetris mechanics with the strategy of Rock, Paper, Scissors. In this game, players control falling tetromino blocks, each representing one of three elements: Rock, Paper, or Scissors. The challenge lies in placing these blocks strategically to clear the board and score points by exploiting the interactions between these elements.

### Key Mechanics

- **Rock, Paper, Scissors Interaction**: Each tetromino falls with a different element—Rock, Paper, or Scissors. The key mechanic is based on the Rock-Paper-Scissors hierarchy to clear blocks:
  - **Paper clears Rock**: When Paper touches Rock, both blocks are cleared.
  - **Rock clears Scissors**: When Rock touches Scissors, both blocks are cleared.
  - **Scissors clears Paper**: When Scissors touches Paper, both blocks are cleared.
- **Strategic Placement**: The blocks are cleared only when the "winning" element touches the "losing" element. If the combination is incorrect, no clearing occurs, and the blocks stack.
  
- **Block Representation**:
  - **Rock**: Represented by a black circle inside a white square tile.
  - **Paper**: Represented by a plain white square tile.
  - **Scissors**: Represented by a black triangle inside a white square tile.

### Gameplay Flow

1. **Falling Tetromino**: A random tetromino with one of the three elements (Rock, Paper, or Scissors) begins falling from the top of the screen.
2. **Placement**: When the tetromino lands on the board, the game checks for any Rock-Paper-Scissors interactions with adjacent blocks.
   - If the interaction is successful (e.g., Paper touches Rock), both blocks are cleared, and points are awarded.
   - If no interaction occurs, the block remains in place.
3. **Falling Mechanics**: If the tetromino has no blocks beneath it, it will continue falling to the next available space.
4. **New Block**: A new block begins falling once the current block reaches the bottom of the board or when it can no longer fall.
5. **Game Over**: The game continues until the stack of blocks reaches the second row from the top, at which point the game ends, and the player is awarded their total score.

### Scoring

- Points are awarded for each successful interaction between blocks, where the "winning" element touches the "losing" element.
- The higher the number of successful interactions, the higher the score.

### Additional Notes

- **Falling Behavior**: If a group of tiles doesn't touch the ground or another tile, they won’t fall to the ground.
- The game ends when the tetromino stack reaches the second row of the board, at which point the player’s final score is displayed.

