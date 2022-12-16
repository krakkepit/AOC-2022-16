using System.Text.RegularExpressions;

namespace AOC_2022_16;

internal partial record Valve(string Name, int Outflow, IEnumerable<string> Links)
{
    [GeneratedRegex("Valve (?<Name>\\w+) has flow rate=(?<Outflow>\\d+); tunnel(s?) lead(s?) to valve(s?) (?<Links>(\\w+(, )?)+)")]
    private static partial Regex Regex();

    public static Valve Parse(string input)
    {
        var r = Regex();
        return new Valve(r.GetString(input, nameof(Name)), r.GetInt(input, nameof(Outflow)), r.GetStrings(input, nameof(Links)));
    }

    public override string ToString()
        => $"{{ {Name}: {Outflow} | [{string.Join(',', Links)}] }}";
}
