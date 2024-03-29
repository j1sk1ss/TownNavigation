using System;
using TMPro;
using UnityEngine;
using UnityEngine.AI;


public class Transport : MonoBehaviour {
    private void Awake() {
        storage     = 0;
        reserved    = 0;
        destination = null;

        // Cash transform
        transform = GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        
        // Hard coded getting label object
        label = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
    }
    
    public delegate Foundation TakeFoundation(Transport transport, bool isFree, Foundation ignore, int reserved);
    public static event TakeFoundation TakeEvent;    
    [SerializeField] private int storage { get; set; }
    [SerializeField] private int reserved { get; set; }
    [SerializeField] private Foundation destination { get; set; }
    [SerializeField] private Foundation prevDestination { get; set; }
    [SerializeField] private TMP_Text label { get; set; }
    private new Transform transform { get; set; }
    private NavMeshAgent agent { get; set; }
    
    /// <summary>
    /// We check every frame for new destination (if it null), and check
    /// are we reach current destination
    /// </summary>
    private void Update() {
        if (destination == null) SetDestination(storage > 0);

        if (agent.pathPending) return;
        if (!agent.hasPath) ReachDestination();
    }

    /// <summary>
    /// Set destination of transport
    /// </summary>
    /// <param name="isEmpty"> Is empty foundation? </param>
    private void SetDestination(bool isEmpty) {
        var foundation = TakeEvent?.Invoke(this, isEmpty, prevDestination, reserved);
        if (foundation == null) return;

        reserved = new System.Random().Next(Math.Max(0, foundation.GetStorage() - foundation.GetReserved()));
        if (!isEmpty) foundation.ChangeReserved(reserved);
        destination = foundation;

        agent.SetDestination(destination.transform.localPosition);
        label.text = $"Storage: {storage}\nDestination: {destination.name}";
    }

    /// <summary>
    /// Method that invokes when transport reach his destination
    /// </summary>
    private void ReachDestination() {
        if (destination == null) return;
        if (storage > 0) {
            // If transport give goods to foundation
            destination.ChangeStorage(storage);
            destination = null;
            
            storage = 0;
            label.text = $"Storage: {storage}\nDestination reached";
            return;
        }
        
        // If transport takes goods from foundation
        destination.ChangeStorage(-reserved);
        destination.ChangeReserved(-reserved);
        storage = reserved;

        prevDestination = destination;
        destination = null;
        label.text  = $"Storage: {storage}\nDestination reached";
    }
}
