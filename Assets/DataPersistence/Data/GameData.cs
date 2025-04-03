using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class GameData 
{

    public Vector3 playerPosition;
    public Vector3 playerRotation;
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
        playerPosition = Vector3.zero;
        playerRotation = Vector3.zero;
        sourcesTime = new SerializableDictionary<string, float>();
        itemsStack = new SerializableDictionary<uint, uint>();
        charactersDialog = new SerializableDictionary<string, uint>();
    }

}
