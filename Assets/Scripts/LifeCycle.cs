using UnityEngine;

public class Lifecycle : MonoBehaviour {
    [SerializeField] private GameObject ballPrefab;

    private Vector2 initialPosition;
    private bool isLaunched = false;
    private bool isCollided = false;
    private float timer = 0f;

    private ReadInput readInputComponent;

    void Start() {
        initialPosition = transform.position;
        readInputComponent = GetComponent<ReadInput>();
    }

    void Update() {
        if (isCollided) {
            timer += Time.deltaTime;

            if (timer >= 12f) {
                // show restart button to perform!!
            }
        }
    }

    public void MarkAsLaunched() {
        isLaunched = true;
        if (readInputComponent != null) {
            readInputComponent.enabled = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (isLaunched && !isCollided) {
            isCollided = true;
            timer = 0f;
        }
    }
}