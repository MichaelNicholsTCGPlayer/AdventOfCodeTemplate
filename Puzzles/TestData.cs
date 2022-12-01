namespace Puzzles
{
    public record TestData
    {
        public TestData(string expected1, string expected2, string[] data)
        {
            Expected1 = expected1;
            Expected2 = expected2;
            Data = data;
        }

        public string Expected1 { get; set; }

        public string Expected2 { get; set; }

        public string[] Data { get; set; }
    }
}
