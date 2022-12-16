using AOC_2022_16;

record ValveWorker(string CurrentRoom, long Time)
{
    public string MemoKey(List<string> valvesOpened, List<ValveWorker> persons)
    {
        List<string> memoKey1 = new();

        memoKey1.AddRange(valvesOpened.OrderBy(x => x));
        memoKey1.AddRange(new string[] { CurrentRoom, Time.ToString() });
        memoKey1.AddRange(persons.Except(Enumerable.Repeat(this, 1)).SelectMany(x => new string[] { x.CurrentRoom, x.Time.ToString() }));

        return string.Join(", ", memoKey1);
    }

    //The current time + the time to get to the otherRoom + 1 (to open the valve).
    public long ReleaseTime(Dictionary<(string ValveFrom, string ValveTo), int> distances, Valve otherRoom)
        => Time + distances![(CurrentRoom, otherRoom.Name)] + 1;
}