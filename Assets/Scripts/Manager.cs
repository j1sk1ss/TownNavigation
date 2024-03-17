using System.Collections.Generic;
using UnityEngine;


public class Manager : MonoBehaviour {
    [SerializeField] private List<Foundation> freeFoundations { get; set; }
    [SerializeField] private List<Foundation> fullFoundations { get; set; }

    private void Start() {
        fullFoundations = new List<Foundation>();
        freeFoundations = new List<Foundation>();
    }

    private void OnEnable() {
        Foundation.OnEmpty  += AddEmptyFoundation;
        Foundation.OnFull   += AddFullFoundation;
        Transport.TakeEvent += GiveFoundation;
    }

    private void OnDisable() {
        Foundation.OnEmpty  -= AddEmptyFoundation;
        Foundation.OnFull   -= AddFullFoundation;
        Transport.TakeEvent -= GiveFoundation;
    }
    
    /// <summary>
    /// Add empty foundation to global stack of foundations
    /// </summary>
    /// <param name="foundation"> Foundation that now is empty </param>
    private void AddEmptyFoundation(Foundation foundation) {
        if (!freeFoundations.Contains(foundation)) freeFoundations.Add(foundation);
        if (fullFoundations.Contains(foundation)) fullFoundations.Remove(foundation);
    }
    
    /// <summary>
    /// Add full foundation to global stack of foundations
    /// </summary>
    /// <param name="foundation"> Foundation that now is full </param>
    private void AddFullFoundation(Foundation foundation) {
        if (!fullFoundations.Contains(foundation)) fullFoundations.Add(foundation);
        if (freeFoundations.Contains(foundation)) freeFoundations.Remove(foundation);
    }

    /// <summary>
    /// Give nearest to transport foundation 
    /// </summary>
    /// <param name="transport"> Transport that want to find next foundation </param>
    /// <param name="isEmpty"> Is empty foundation that want to find transport? </param>
    /// <param name="ignore"> Ignore some foundation </param>
    /// <returns> Nearest to transport foundation </returns>
    private Foundation GiveFoundation(Transport transport, bool isEmpty, Foundation ignore = null) {
        var list = isEmpty ? freeFoundations : fullFoundations;

        Foundation nearest = null;
        var lowestDistance = double.MaxValue;
        foreach (var foundation in list) {
            var enableGoods = foundation.GetStorage() - foundation.GetReserved();
            var distance = Vector3.Distance(foundation.transform.localPosition, transport.transform.localPosition);
            if (!isEmpty && enableGoods < 2) continue; // Is has not reserved goods
            if (distance >= lowestDistance || ignore == foundation) continue; // Is nearest and is not ignored
            
            lowestDistance = distance;
            nearest = foundation;
        }
        
        return nearest;
    }
}
