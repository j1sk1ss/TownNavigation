using UnityEngine;


public class TextLookAt : MonoBehaviour {
    void Start() {
        transform = GetComponent<Transform>();
        camera = Camera.main;
        
        RotateText();
    }

    private new Transform transform { get; set; }
    [SerializeField] private new Camera camera { get; set; }

    private void RotateText() {
        transform.LookAt(camera.transform);
        transform.Rotate(0, 180, 0);
    }

    private void Update() => RotateText();
}
