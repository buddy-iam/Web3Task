using UnityEngine;

public class CollideDecision : MonoBehaviour {

    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject retry;

    private void Start() {
        if (victoryPanel != null) {
            victoryPanel.SetActive(false);
        }
        if (retry != null) {
            retry.SetActive(true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.name == "Slope") {
            Debug.Log("Slope hit recorded! Head for the bucket sensor.");
        } else {
            TriggerFail();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.name == "Sensor") {
            TriggerSuccess();;
        } else {
            TriggerFail();
        }
    }

    void TriggerFail() {
        Debug.Log("Try Again!");
    }

    void TriggerSuccess() {
        if (victoryPanel != null) {
            victoryPanel.SetActive(true);
        }
        if (retry != null) {
            retry.SetActive(false);
        }
        Debug.Log("Great Job!");
    }
}