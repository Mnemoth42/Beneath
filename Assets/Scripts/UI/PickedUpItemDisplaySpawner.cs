using System;
using System.Collections.Generic;
using UnityEngine;

public class PickedUpItemDisplaySpawner : MonoBehaviour
{
    [SerializeField]PickedUPItemDisplay pickedUpItemDisplay;
    [SerializeField]float spawnrate=.1f;

    public class PickupQueue
    {
        public Vector3 location;
        public string title="";
    }

    //Queue<PickupQueue> pickupQueue = new Queue<PickupQueue>();
    Queue<Action> pickupQueue = new Queue<Action>();

    void Start()
    {
        InvokeRepeating(nameof(TryToSpawnItem), spawnrate, spawnrate);
    }

    void TryToSpawnItem()
    {
        if (pickupQueue.Count==0) return;
        //PickupQueue item = pickupQueue.Dequeue();
        //SpawnDisplayedItem(item.location,item.title);
        pickupQueue.Dequeue().Invoke();
    }

    public void SpawnPickupDisplay(Vector3 location, string title)
    {
        if (pickedUpItemDisplay == null) return;
        //PickupQueue item = new PickupQueue();
        //item.location = location;
        //item.title = title;
        //pickupQueue.Enqueue(item);
        pickupQueue.Enqueue( ()=>
        {
            SpawnDisplayedItem(location, title);
        });
    }

    void SpawnDisplayedItem(Vector3 location, string title)
    {
        var item = Instantiate(pickedUpItemDisplay, location, Quaternion.identity) as PickedUPItemDisplay;
        item.Initialize(title);
    }
}
