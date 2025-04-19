using System.Collections.Generic;

/// <summary>
/// The DayData class is used to store the data for a day in the game.
/// </summary>
public class DayData
{
    /// <summary>
    /// The customersSpawnProbs property stores the probabilities of spawning a customer in the market
    /// based on its stereotype.
    /// </summary>
    public Dictionary<string, float> customersSpawnProbs = new()
    {
      { "Karen", 0 },
      { "AnnoyingKid", 0 }
    };

    /// <summary>
    /// The managerTimes property stores the time the manager spends doing
    /// its activities in the market.
    /// </summary>
    public Dictionary<string, float> managerTimes = new ()
    {
      { "OfficeTime", 0 },
      { "PatrolTime", 0 }
    };

    /// <summary>
    /// The numberOfTasks property stores the number of tasks that can be assigned to the player
    /// in one day.
    /// </summary>
    public int numberOfTasks;

    /// <summary>
    /// The tasksProbs property stores the probabilities of a task being assigned to the player
    /// based on its type.
    /// </summary>
    public Dictionary<string, float> tasksProbs = new()
    {
        { "CleanFloor", 0 },
        { "FixFuseBox", 0 },
        { "FixToilet", 0 }
    };
}