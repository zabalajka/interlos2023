# Interlos 2023 - INTERnetová LOgická Soutěž

Official website is [https://interlos.fi.muni.cz/](https://interlos.fi.muni.cz/) with [tasks](https://interlos.fi.muni.cz/game/).

These are the source codes that were used during the competition. Minor clean-up was made and some comments were added, but the overall logic/functionality hasn't been changed.


## P1 Losí plotr (Plotter / Printing instructions)

### Task

Input contains set of printing instructions.

### Solution

Solution is straightforward - process each line/instruction by recording whether current position is "coloured" and moving printing head to new position.

First pass will output an image with new set of printing instructions ([output](./outputs/P1%20-%20output.txt)). Therefore, second pass is needed to get the final image ([output2](./outputs/P1%20-%20output2.txt)). The second image looks a bit distorted (each line seems to be shifted one character to the right), so I assume there is a bug when interpreting the input. However, resulting passphrase `KROKODYL` can still be read.


## P3 Logo lOS s.l.o. (Screensaver for moose-shaped monitor)

_Task abandoned in favour of other tasks._


## P6 Visutý Lost (Crossing the bridge)

### Task

Input contains _n_ rows and _c_ columns which represents a bridge. Each tile is numbered and the goal is jump from tile to tile across the bridge without jumping on a tile with same number more than once (more tiles share the same number).

### Solution

For each tile in starting row, new attempt is made. Then, for each attempt, all possible movements (L,R,U,D + diagonally) are evaluated. If tile with a given number was already visited, such attempt is discarded, otherwise such movement/attempt is put to the backlog of all possibilities. Basically, it is brute force approach with tree growing into width, but the leaves are discarded relatively quickly, so memory is not an issue.

The algorithm was taking too long without producing a result, so I started making various optimizations:
* record only bitmap of visited tiles instead of ordered list of the tiles (which is the expected answer for this task), upon finding the correct starting tile, another pass would be made with also recording the ordered list of visited tiles
* change order in which new attempts are put into the backlog favouring attempts that are further ahead on the bridge (higher row); reasoning is that attempts that are further ahead are more likely to reach the end sooner

This still didn't produce result in meaningful amount of time. At some point I realized that there are exactly 24 different numbers on tiles and exactly 24 rows, so if a tile's number can be visited only once, that also means that each row can be visited only once. Therefore, the movement needs to be limited only forward (directly or diagonally). After this change, the result was produced pretty much immediately.


## P8 Popisné čísla (House numbers)

### Task

Find all numbers that in binary have twice as many ones than zeros.

### Solution

This looked to be just math task (without programming). In the end, scripting simplifies how the result is obtained, but probably this task could be solved in Excel just with built-in formulas.

House number (in binary) fulfils these conditions:
* starts with 1 (no leading zeros)
* there is _k_ zeros and _2k_ ones or _2k-1_ ones excluding the leading one which has fixed position.
* number isn't higher than _max = 12345678987654321<sub>10</sub>_ or `10_1011_1101_1100_0101_0100_0110_0010_1001_0001_1111_0100_1011_0001`<sub>2</sub>

Maximum number is 54 digits long which corresponds to _k = 18_. For _k = 1, 2, ..., 17_ all numbers are valid, for _k = 18_ limitations apply.

#### _k = 1, 2, ..., 17_

We are looking for all (different) combinations of _k_ zeros and _2k-1_ ones which corresponds to combination number _Comb(k + 2k - 1; k)_.

#### _k = 18_

The approach with combination number cannot be used directly, because it would produce results higher than the maximum allowed number. To obtain number of possible combinations while fulfilling the maximum condition, following logic is used:
* first _i = 1, 2, ..., 54_ (most significant, from left) digits are taken from the _max_, these digits are fixed
* if the following digit is `0` then no combinations are calculated, because it would produce also combinations that are higher than _max_
* otherwise, if the following digit is `1`, then _Comb(zeros + ones, ones)_ is calculated where
  * `0` is fixed as following digit ensuring the number will always be smaller than _max_
  * _zeros = k - zeros_in_left_part - 1_ represents number of zeros in the remaining portion of the string already excluding the new fixed `0`
  * _onces = 2k - ones_in_left_part_ represents number of ones in the remaining potion of the string

Result is then sum of all combinations numbers mentioned above.
