using UnityEngine;

public class ReadInput : MonoBehaviour
{
    [SerializeField]private Rigidbody2D ball;
    [SerializeField] LineRenderer ballArc;
    [SerializeField]private float forceFactor = 10.0f;
    [SerializeField] private float maxDragLength = 3.0f;

    private int arcPoints = 15;
    private float timeGap = 0.01f;
    private Vector2 startPoint;
    private Vector2 endPoint;

    private bool hasLaunched = false;

    private void Start() { 
        ballArc.positionCount = 0;
    }

    private void Update() {
        if (hasLaunched) return;

        if (Input.GetMouseButtonDown(0)) {
            startPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ballArc.positionCount = arcPoints;
        }

        if (Input.GetMouseButton(0)) {
            Vector2 currPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dragDir = (startPoint - currPoint).normalized;
            float rawDragLen = Vector2.Distance(startPoint, currPoint);
            float dragLen = Mathf.Min(rawDragLen, maxDragLength);

            DrawArc(dragDir, dragLen);
        }

        if (Input.GetMouseButtonUp(0)) { 
            endPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dragDir = (startPoint - endPoint).normalized;
            float rawDragLen = Vector2.Distance(startPoint, endPoint);
            float dragLen = Mathf.Min(rawDragLen, maxDragLength);

            ballArc.positionCount = 0;
            Launch(dragDir, dragLen);
        }
    }

    private void DrawArc(Vector2 dir, float len) {
        Vector2 appliedForce = forceFactor * len * dir;
        Vector2 initialVelocity = appliedForce / ball.mass;

        Vector2 startPosition = ball.position;
        Vector2 gravity = Physics2D.gravity * ball.gravityScale;

        //float predictionTimeStep = 0.03f;

        for (int i = 0; i < arcPoints; i++) {
            float t = i * timeGap;
            Vector2 pointPosition = startPosition + (initialVelocity * t) + (0.5f * gravity * (t * t));
            ballArc.SetPosition(i, pointPosition);
        }
    }

    void Launch(Vector2 dir, float len) {
        if (hasLaunched) return;
        hasLaunched= true;
        ball.AddForce(forceFactor * len * dir, ForceMode2D.Impulse);
    }

}
