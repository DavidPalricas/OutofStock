using UnityEngine;

/// <summary>
/// The DayConfig class is used to configure the parameters for a day in the game, in
/// the unity editor.
/// </summary>
[System.Serializable]
public class DayConfig
{
    /// <summary>
    /// The karenSpawnProb and annoyingKidSpawnProb properties define the probabilities of
    /// spawning a customer stereotype in the game, these stereotypes are respetively
    /// a Karen and an annoying kid.
    /// </summary>
    [Range(0, 1)]
    public float karenSpawnProb, annoyingKidSpawnProb;

    /// <summary>
    /// The managerOfficeTime and managerPatrolTime properties define the time the manager
    /// spends in the office and the time he spends patrolling the market, respectively.
    /// </summary>
    public float managerOfficeTime, managerPatrolTime;

    /// <summary>
    /// The numberOfTasks property defines the number of tasks that can be assigned to the player
    /// in one day.
    /// The customersToSend property defines the number of customers that the player must send (by attacking them)
    /// to his uncle's market in one day.
    /// </summary>
    public int numberOfTasks, customersToSend;

    /// <summary>
    /// The cleanFloorProb, fixFuseBoxProb and fixToiletProb properties define the probabilities of
    /// a task being assigned to the player, these tasks are respectively cleaning the floor,
    /// fixing a fuse box and fixing a toilet.
    /// </summary>
    [Range(0, 1)]
    public float cleanFloorProb, fixFuseBoxProb, fixToiletProb;
}

