using UnityEngine;

public class CollideDecision : MonoBehaviour
{
    [SerializeField]private float thVel = 1.0f;
    [SerializeField] private float thAngle = 1.0f;

    //[SerializeField]private GameObject slope;
    //private SpriteRenderer spriteRenderer;

    //void Start() { 
        //spriteRenderer = GetComponent<SpriteRenderer>();
    //}

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.name == "Slope") { 
            float collideSpeed = collision.relativeVelocity.magnitude;

            Vector2 incomingAngle = -collision.relativeVelocity;
            Vector2 surfaceAngle = collision.GetContact(0).normal;

            float collideAngle = Mathf.Abs(90f - Vector2.Angle(incomingAngle, surfaceAngle));

            if (collideSpeed > thVel || collideAngle > thAngle) {
                TriggerFail();
            } else { 
                TriggerSuccess();
            }
        }
    }

    void TriggerFail() {
        //spriteRenderer.color = Color.black;
        Debug.Log("Wrong direction / force / surface");
    }

    void TriggerSuccess() { 
        //spriteRenderer.color = Color.green;
        Debug.Log("Great Job!");
    }

}

