using System.Collections.Generic;

/// <summary>
/// The DayData class is used to store the data for a day in the game.
/// Each property of this class represents a key of the JSON file that contains part of the data for a day.
/// </summary>
public class DayData
{
    /// <summary>
    /// The customersSpawnProbs property stores the probabilities of spawning a customer in the market
    /// based on its stereotype.
    /// </summary>
    public Dictionary<string, float> customersSpawnProbs;

    /// <summary>
    /// The managerTimes property stores the time the manager spends doing
    /// its activities in the market.
    /// </summary>
    public Dictionary<string, float> managerTimes;

    /// <summary>
    /// The numberOfTasks property stores the number of tasks that can be assigned to the player and
    /// the customersToSend property stores the number of customers that the player must send(by attacking them) to his uncle
    /// market in one day.
    /// The minProductsInShelfs and maxProductsInShelfs properties define the minimum and maximum number of products in the shelves, respectively.
    /// </summary>
    public int numberOfTasks, customersToSend, minProductsInShelfs, maxProductsInShelfs;

    /// <summary>
    /// The tasksProbs property stores the probabilities of a task being assigned to the player
    /// based on its type.
    /// </summary>
    public Dictionary<string, float> tasksProbs;
}