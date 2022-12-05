using Puzzles;
using Puzzles.Solutions;

namespace PuzzleRunners
{
    public abstract class BaseRunner<T>
        where T : IPuzzle
    {
        private static object _locker = new();

        private static string? _dataFolder;
        protected static string DataFolder
        {
            get
            {
                // Tests can run in parallel, use locking to optimistically access/initialize the DataFolder property
                if (_dataFolder == null)
                {
                    lock (_locker)
                    {
                        if (_dataFolder == null)
                        {
                            var assemblyPath = System.Reflection.Assembly.GetAssembly(typeof(BaseRunner<T>))!.Location;
                            var index = assemblyPath.IndexOf("PuzzleRunner");
                            var rootPath = assemblyPath[..index];
                            _dataFolder = Path.Combine(rootPath, "Puzzles");
                        }
                    }
                }

                return _dataFolder;
            }
        }

        private static T? _puzzle;
        public static T Puzzle
        {
            get
            {
                // Tests can run in parallel, use locking to optimistically access/initialize the Puzzle property
                if (_puzzle == null)
                {
                    lock (_locker)
                    {
                        if (_puzzle == null)
                        {
                            // Use the default constructor to create an instance of the Puzzle  (which should implement the IPuzzle interface)
                            _puzzle = Activator.CreateInstance<T>();
                        }

                    }
                }

                return _puzzle;
            }
        }

        [Theory]
        [MemberData(nameof(GetTestDataFilePaths))]
        public void Puzzle1TestsPass(string fileName)
        {
            var testData = ReadTestInput(fileName);
            RunPuzzleTest(testData.Data, 1, testData.Expected1);
        }

        [Fact]
        public void Puzzle1Passes()
        {
            var input = ReadRealInput();
            var answer = RunPuzzle(input, 1);

            Assert.Equal(Puzzle1Solution, answer);
        }

        [Theory]
        [MemberData(nameof(GetTestDataFilePaths))]
        public void Puzzle2TestsPass(string fileName)
        {
            var testData = ReadTestInput(fileName);
            RunPuzzleTest(testData.Data, 2, testData.Expected2);
        }

        [Fact]
        public void Puzzle2Passes()
        {
            var input = ReadRealInput();
            var answer = RunPuzzle(input, 2);

            Assert.Equal(Puzzle2Solution, answer);
        }

        public static IEnumerable<object[]> GetTestDataFilePaths()
        {
            // Get all txt files in the TestInput folder for the day
            return Directory.GetFiles(TestDataFolderPath, "*.txt").Select(r => new object[] { r.Replace(TestDataFolderPath, "") });
        }

        private static string TestDataFolderPath => Path.Combine(DataFolder, $"TestInput\\Day{Puzzle.Day:D2}\\");

        /// <summary>
        /// This is the real solution to Puzzle 2  (Once you submit your answer to AoC and comes back success, you should enter it here, so that your tests pass)
        /// </summary>
        protected abstract string Puzzle1Solution { get; }

        /// <summary>
        /// This is the real solution to Puzzle 2  (Once you submit your answer to AoC and comes back success, you should enter it here, so that your tests pass)
        /// </summary>
        protected abstract string Puzzle2Solution { get; }

        protected string[] ReadRealInput()
        {
            // Read all the lines for the real puzzle input
            var fileName = Path.Combine(DataFolder, $"Input\\{Puzzle.Day:D2}.txt");
            return File.ReadAllLines(fileName);
        }

        private TestData ReadTestInput(string filePath)
        {
            var data = File.ReadAllLines(Path.Combine(TestDataFolderPath, filePath)).ToList();

            // First line is Puzzle1 Expected Result
            var expectedResult1 = data[0];
            data.RemoveAt(0);

            // Second line is Puzzle1 Expected Result
            var expectedResult2 = data[0];
            data.RemoveAt(0);

            // Third line is separator (check it, just in case they didnt use the first 2 lines as expected results by accident)
            if (!data[0].StartsWith("-#-#-#-#-#-"))
            {
                throw new Exception("File does not confirm to Test input format. First 2 lines are expected outputs. 3rd line is a separator");
            }
            data.RemoveAt(0); // Remove Line

            return new TestData(expectedResult1, expectedResult2, data.ToArray());
        }

        protected string RunPuzzle(string[] input, int puzzleNumber)
        {
            if (puzzleNumber == 1)
            {
                return Puzzle.Puzzle1(input);
            }
            else
            {
                return Puzzle.Puzzle2(input);
            }
        }

        protected void RunPuzzleTest(string[] input, int puzzleNumber, string expected)
        {
            string answer;
            if (puzzleNumber == 1)
            {
                answer = Puzzle.Puzzle1(input);
            }
            else
            {
                answer = Puzzle.Puzzle2(input);
            }

            Assert.Equal(expected, answer);
        }
    }
}
