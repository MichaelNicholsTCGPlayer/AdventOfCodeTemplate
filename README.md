# AdventOfCodeTemplate
A starter template for an AdvantOfCode C# Repo.

This solution is setup to allow the dev to jump strait into the fun part of Advent of Code (solving the puzzle). For each day, simple copy your Puzzle Input into the text file: Puzzles/Input.<day>.txt, and this will allow the system to load up your input for solving the puzzle.

The solution also has support for multiple test inputs (as well as expected outputs for both puzzles). The test input is located at: Puzzles/TestInput/Day<day>/*.txt.
Any txt file contained within this folder will be loaded as test input (it doesn't matter what the name is, as long as it is a txt file). You can have multiple test files. Test cases for each file will be automatically generated. I have added a blank test file for each day to get started since it is expected that atleast 1 test case for each day will be given.

The Test input file format looks like the following:
First Line: Expected Puzzle 1 Output
Second Line: Expected Puzzle 2 Output
Third Line: Separator Bar
Remaining Lines: The Test Input

ex:
```
<Expected Puzzle 1 Output>
<Expected Puzzle 2 Output>
-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-#-
<Test Input>
```

Once your Input and Test Input has been set. You can work on the solution. The solution files can be found in Puzzles/Solutions. One class file for each day. Each class has two methods stubbed out Puzzle1() and Puzzle2().


# Running the Solution
This solution uses the Visual Studio Test Runner to manage running your puzzle solutions. Each day has its own file in the PuzzleRunner which has two Properties. Once you have a solution to the problem, you can enter the value in the correct property so your PuzzleRunner will report success. I have put a default value of "TODO" for all solutions as a place holder.

It is expected that all tests will fail initially until the test is implemented.
