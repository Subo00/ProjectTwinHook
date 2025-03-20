using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMyUpdate{
    void MyUpdate();
}



public class UpdateManager : MonoBehaviour{

    public static UpdateManager Instance;

    private readonly HashSet<IMyUpdate> myUpdates = new();
    private readonly List<IMyUpdate> toAdd = new();
    private readonly List<IMyUpdate> toRemove = new();

    public void AddUpdatable(IMyUpdate myUpdate) => toAdd.Add(myUpdate);
    public void RemoveUpdatable(IMyUpdate myUpdate) => toRemove.Add(myUpdate);

    private void RunUpdates(){
        // Process additions and removals before running updates
        foreach (var myUpdate in toAdd){
            myUpdates.Add(myUpdate);
        }
        toAdd.Clear();

        foreach (var myUpdate in toRemove){
            myUpdates.Remove(myUpdate);
        }
        toRemove.Clear();

        // Now run the updates
        foreach (var myUpdate in myUpdates){
            myUpdate?.MyUpdate();
        }
    }

    private void Start(){

        if (UpdateManager.Instance != null){
            Destroy(gameObject);
        }
        UpdateManager.Instance = this;
    }

    void Update(){
        RunUpdates();
    }
}