using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    const int NUM_OF_LEVELS = 4;
    public Vector3 playerOnePosition;
    public Vector3 playerTwoPosition;

    //public bool[] levels;
    public bool[] sheep;

    public uint numOfCompletedLevels;
   // public SerializableDictionary<int, bool> levels;
    //public SerializableDictionary<int, bool> sheep;
                                 //id     time
    public SerializableDictionary<string, float> sourcesTime;
    //time
    //list of items               id    currentStack
    public SerializableDictionary<uint, uint> itemsStack;
    //list of recipes
    //public 
    //list of characters           name   questIndex
    public SerializableDictionary<string, uint> charactersDialog;
    //list of quests 

    public GameData()
    {
        playerOnePosition = Vector3.zero;
        playerTwoPosition = Vector3.zero;
        //levels = new bool[NUM_OF_LEVELS];
        sheep = new bool[NUM_OF_LEVELS];
        for(int i = 0; i < NUM_OF_LEVELS; i++)
        {
            //levels[i] = false;
            sheep[i] = false;
        }
        numOfCompletedLevels = 0;
        //playerRotation = Vector3.zero;
        /// sourcesTime = new SerializableDictionary<string, float>();
        // itemsStack = new SerializableDictionary<uint, uint>();
        //charactersDialog = new SerializableDictionary<string, uint>();
    }

}
