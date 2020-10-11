using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickedUpItemDisplaySpawner : MonoBehaviour
{
    [SerializeField]PickedUPItemDisplay pickedUpItemDisplay;

    public class PickupQueue
    {
        public Vector3 location;
        public string title="";
    }

    Queue<PickupQueue> pickupQueue = new Queue<PickupQueue>();

    void Start()
    {
        InvokeRepeating(nameof(TryToSpawnItem), .1f, .1f);
    }

    void TryToSpawnItem()
    {
        if (pickupQueue.Count==0) return;
        PickupQueue item = pickupQueue.Dequeue();
        SpawnDisplayedItem(item.location,item.title);
    }

    public void SpawnPickupDisplay(Vector3 location, string title)
    {
        if (pickedUpItemDisplay == null) return;
        PickupQueue item = new PickupQueue();
        item.location = location;
        item.title = title;
        pickupQueue.Enqueue(item);
    }

    void SpawnDisplayedItem(Vector3 location, string title)
    {
        var item = Instantiate(pickedUpItemDisplay, location, Quaternion.identity) as PickedUPItemDisplay;
        item.Initialize(title);
    }
}
