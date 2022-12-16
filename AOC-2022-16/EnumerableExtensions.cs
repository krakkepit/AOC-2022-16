namespace AOC_2022_16;

internal static class EnumerableExtensions
{
    public static Dictionary<(string ValveFrom, string ValveTo), int> Distances(this IEnumerable<Valve> valves)
    {
        //Floyd Warshall algorithm
        var names = valves.Select(x => x.Name);
        var dict = valves.SelectMany(valve => valve.Links.Select(link => (valve.Name, link))).ToDictionary(keySelector: item => item, elementSelector: _ => 1);

        foreach (var k in names)
        {
            foreach (var i in names)
            {
                if (!dict.ContainsKey((i, k))) continue;

                foreach (var j in names)
                {
                    if (!dict.ContainsKey((k, j))) continue;

                    if (dict.ContainsKey((i, j)))
                    {
                        dict[(i, j)] = Math.Min(dict[(i, j)], dict[(i, k)] + dict[(k, j)]);
                    }
                    else
                    {
                        dict[(i, j)] = dict[(i, k)] + dict[(k, j)];
                    }
                }
            }
        }

        return dict;
    }

    public static List<T> WithUpdatedItem<T>(this List<T> list, T item, Func<T, T> action)
    {
        var l = new List<T>(list);
        l.Remove(item);
        var adjustedItem = action.Invoke(item);
        l.Add(adjustedItem);

        return l;
    }

    public static List<T> Concat<T>(this List<T> list, T item)
        => new(list)
        {
            item
        };

    public static List<T> Singleton<T>(this T obj)
        => new() { obj };
}
