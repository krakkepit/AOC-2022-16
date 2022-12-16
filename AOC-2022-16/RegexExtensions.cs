using System.Text.RegularExpressions;

namespace AOC_2022_16;

internal static class RegexExtensions
{
    public static string GetString(this Regex regex, string input, string groupName)
        => regex.Match(input).Groups[groupName].Value;
    public static int GetInt(this Regex regex, string input, string groupName)
        => int.Parse(regex.Match(input).Groups[groupName].Value);
    public static IEnumerable<int> GetInts(this Regex regex, string input, string groupName)
        => regex.Match(input).Groups[groupName].Value.Split(", ").Select(int.Parse);
    public static IEnumerable<string> GetStrings(this Regex regex, string input, string groupName)
        => regex.Match(input).Groups[groupName].Value.Split(", ");
}
