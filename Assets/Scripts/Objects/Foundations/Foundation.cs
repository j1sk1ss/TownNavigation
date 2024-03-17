using System.Collections;
using TMPro;
using UnityEngine;


public class Foundation : MonoBehaviour {
    
   private void Awake() {
        storage       = new System.Random().Next(2);
        creationSpeed = new System.Random().Next(5) - 3;
        reserved      = 0;
        transform     = GetComponent<Transform>();

        // Hard coded getting label object
        label = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        
        StartCoroutine(LifeCycle());
    }
   
    public delegate void AddFoundation(Foundation foundation);
    public delegate void DelFoundation(Foundation foundation);
    public static event AddFoundation OnFull;
    public static event DelFoundation OnEmpty;    
    public new Transform transform { get; private set; }
    
    [SerializeField] private int storage { get; set; }
    [SerializeField] private int reserved { get; set; }
    [SerializeField] private int creationSpeed { get; set; }
    [SerializeField] private TMP_Text label { get; set; }
    
    /// <summary>
    /// Method of Foundation Life Cycle. Production / Using storage
    /// </summary>
    /// <returns> Coroutine </returns>
    private IEnumerator LifeCycle() {
        storage += creationSpeed; // Produce goods
        if (storage < 0) storage = 0;
        if (reserved < 0) reserved = 0;        
        
        if (storage > 0) {
            label.text = $"Storage: {storage}\nReserved: {reserved}\nReady";
            MarkFull();
        }
        else {
            label.text = $"Storage: {storage}\nReserved: {reserved}\nNot ready";
            MarkEmpty();
        }
        
        yield return new WaitForSeconds(1);
        StartCoroutine(LifeCycle());
    }

    /// <summary>
    /// Create event that mark this foundation as full (non empty)
    /// </summary>
    private void MarkFull() => OnFull?.Invoke(this);
    
    /// <summary>
    /// Create event that mark this foundation as empty
    /// </summary>
    private void MarkEmpty() => OnEmpty?.Invoke(this);

    public void ChangeReserved(int value) => reserved += value;
    public void ChangeStorage(int value) => storage += value;

    public int GetReserved() => reserved;
    public int GetStorage() => storage;
}
