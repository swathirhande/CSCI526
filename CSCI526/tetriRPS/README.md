# TetriRPS

TetriRPS is a 2D puzzle game which innovatively combines classic Tetris mechanics with rock-paper-scissors elements, creating a dynamic interaction system between falling tetrominos. Players receive tetrominos classified as rock, paper, or scissors, and blocks are cleared not by completing lines but through strategic placements that allow them to "defeat" adjacent blocks. In this game, players place falling tetromino blocks, each representing one of three elements: rock, paper, or scissors.

The key mechanic revolves around the Rock-Paper-Scissors hierarchy for clearing blocks:

1)Paper clears Rock: When a new Paper block touches an already existing Rock block below or beside it, both the Paper and Rock cells that are in contact are cleared.

2)Rock clears Scissors: When a new Rock block touches an already existing Scissors block below or beside it, both the Scissors and Rock cells that are in contact are cleared.

3)Scissors clear Paper: When a new Scissors block touches an already existing Paper block below or beside it, both the Paper and Scissors cells that are in contact are cleared.

The reverse (e.g., Rock touching Paper) does not clear the blocks but simply stacks them.


