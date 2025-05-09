using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class GameData 
{

    public Vector3 playerOnePosition;
    public Vector3 playerTwoPosition;

    public SerializableDictionary<int, bool> levels;
    public SerializableDictionary<int, bool> sheep;
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
        levels = new SerializableDictionary<int, bool>();
        sheep = new SerializableDictionary<int, bool>();
        //playerRotation = Vector3.zero;
       /// sourcesTime = new SerializableDictionary<string, float>();
       // itemsStack = new SerializableDictionary<uint, uint>();
        //charactersDialog = new SerializableDictionary<string, uint>();
    }

}
