
namespace Puzzles.Solutions
{
    public interface IPuzzle
    {
        int Day { get; }

        string Puzzle1(string[] input);

        string Puzzle2(string[] input);
    }
}
