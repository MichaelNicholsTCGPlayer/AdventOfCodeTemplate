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
                if (_puzzle == null)
                {
                    lock (_locker)
                    {
                        if (_puzzle == null)
                        {
                            _puzzle = Activator.CreateInstance<T>();
                        }

                    }
                }

                return _puzzle;
            }
        }

        [Theory]
        [MemberData(nameof(GetTestDataForPuzzle), 1)]
        public void Puzzle1TestsPass(string expected, string[] input)
        {
            RunPuzzleTest(input, 1, expected);
        }

        [Fact]
        public void Puzzle1Passes()
        {
            var input = ReadRealInput();
            var answer = RunPuzzle(input, 1);

            Assert.Equal(Puzzle1Solution, answer);
        }

        [Theory]
        [MemberData(nameof(GetTestDataForPuzzle), 2)]
        public void Puzzle2TestsPass(string expected, string[] input)
        {
            RunPuzzleTest(input, 2, expected);
        }

        [Fact]
        public void Puzzle2Passes()
        {
            var input = ReadRealInput();
            var answer = RunPuzzle(input, 2);

            Assert.Equal(Puzzle2Solution, answer);
        }

        public static IEnumerable<object[]> GetTestDataForPuzzle(int puzzleNumber)
        {
            foreach(var testData in ReadTestInput())
            {
                yield return new object[]
                {
                    puzzleNumber == 1 ? testData.Expected1 : testData.Expected2,
                    testData.Data,
                };
            }
        }

        protected abstract string Puzzle1Solution { get; }

        protected abstract string Puzzle2Solution { get; }


        protected string[] ReadRealInput()
        {
            var fileName = Path.Combine(DataFolder, $"Input/{Puzzle.Day:D2}.txt");
            return File.ReadAllLines(fileName);
        }

        private static TestData[] ReadTestInput()
        {
            //var folderName = Path.Combine(dataFolder, $"TestInput/Day{Puzzle.Day:D2}");
            var folderName = Path.Combine(DataFolder, $"TestInput/Day{Puzzle.Day:D2}");

            var files = Directory.GetFiles(folderName, "*.txt");


            List<TestData> tests = new List<TestData>();
            foreach (var file in files)
            {
                var data = File.ReadAllLines(file).ToList();
                var expectedResult1 = data[0];
                data.RemoveAt(0);

                var expectedResult2 = data[0];
                data.RemoveAt(0);


                if (!data[0].StartsWith("-#-#-#-#-#-"))
                {
                    throw new Exception("File does not confirm to Test input format. First 2 lines are expected outputs. 3rd line is a separator");
                }
                data.RemoveAt(0); // Remove Line

                tests.Add(new TestData(expectedResult1, expectedResult2, data.ToArray()));
            }

            return tests.ToArray();
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
