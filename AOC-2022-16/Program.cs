using AOC_2022_16;

Console.WriteLine("Hello, World!");
var file = File.ReadAllLines("TextFile1.txt");

var valves = file.Select(Valve.Parse).ToList();

//We are setting up a network with only valves that can release pressure.
//This makes the network smaller, from 57 to 15 entries in the final test input.
var pressurizedValves = valves.Where(x => x.Outflow is > 0).ToList();

//We then calculate the distances from every valve to another, such that we know how much time it will cost to get from one point to another.
//This is also important because we can then also use non-pressure-releasing valves in our traversal from one pressurized valve to another.
var distances = valves.Distances();

Dictionary<string, long> memo = new();
var openedValves = new List<string>();

//Part 1
var me = new ValveWorker(CurrentRoom: "AA", Time: 0);
var path = MaximumReleasedPressure(minutes: 30L, me);

//Part 2
//var elephant = new ValveWorker(CurrentRoom: "AA", Time: 0);
//var path = FindPath(26L, me, elephant);
Console.WriteLine(path);
Console.ReadLine();

/**
 * =======================================================================================================
 * Implementation
 * Maximum Released Pressure
 * =======================================================================================================
 */

long MaximumReleasedPressure(
    long minutes,
    params ValveWorker[] workers)
    => ReleasedPressure(minutes, workers.ToList(), valvesOpened: new());

long ReleasedPressure(
    long minutes,
    List<ValveWorker> persons,
    List<string> valvesOpened)
{
    //Null check
    if (pressurizedValves is null) return 0;

    //If we have opened all valves, we cannot free up any more pressure.
    if (valvesOpened.Count == pressurizedValves.Count) return 0;

    //Make use of memoization to see if you have already ended up in this state to prevent a lot of computational complexity
    if (persons.Select(x => x.MemoKey(valvesOpened, persons)).Where(memo.ContainsKey).FirstOrDefault() is string key) return memo[key];

    //For all places we can go to from this spot, check if it's worth (if we can release more pressure than expected when having the current solution).
    long pressure = 0;

    foreach (var pressurizedValve in pressurizedValves.Where(p => !valvesOpened.Contains(p.Name)))
    {
        pressure = persons.Select(person => ReleasedPressureIfValveTurned(person, distances!, pressurizedValve, minutes, pressure, valvesOpened, persons)).Max();
    }

    //Save the pressure result to memoize later.
    persons.Select(x => x.MemoKey(valvesOpened, persons)).ToList().ForEach(memoKey => memo[memoKey] = pressure);
    return pressure;
}

long ReleasedPressureIfValveTurned(
    ValveWorker person,
    Dictionary<(string ValveFrom, string ValveTo), int> distances,
    Valve valveToBeTurned,
    long minutes,
    long currentPressure,
    List<string> valvesOpened,
    List<ValveWorker> persons)
{
    var releaseTime = person.ReleaseTime(distances, valveToBeTurned);

    //If the valve cannot be turned in time, do not consider turning it.
    if (releaseTime <= minutes)
    {
        //The amount of release that turning the knob gives is equal to Outflow per minute * amount of remaining minutes.
        long releasePressure =
            valveToBeTurned.Outflow * (minutes - releaseTime);

        //Calculate the amount of pressure that you can still release after opening this valve.
        var updatedPersons = persons.WithUpdatedItem(person, p => p with { CurrentRoom = valveToBeTurned.Name, Time = releaseTime });
        long maxPressureReleaseAfterThisRelease = ReleasedPressure(minutes, updatedPersons, valvesOpened.Concat(valveToBeTurned.Name));

        //Opt for the maximum pressure release.
        currentPressure = Math.Max(currentPressure, releasePressure + maxPressureReleaseAfterThisRelease);
    }

    return currentPressure;
}