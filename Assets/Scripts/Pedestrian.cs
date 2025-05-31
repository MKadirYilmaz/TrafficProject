using UnityEngine;

public class Pedestrian : MonoBehaviour
{
    public float speed = 0.5f;
    public LineRenderer currentRoad;

    private Vector3 destination;
    private Rigidbody2D rb;
    private float currentSpeed;
    private bool isWaiting = false;
    private int currentRoadStep = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = speed;
        speed *= Random.Range(0.6f, 1.4f);
        GetComponentInChildren<SpriteRenderer>().color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, currentRoad.GetPosition(currentRoadStep)) < 0.01)
        {
            currentRoadStep++;
            if (currentRoadStep >= currentRoad.positionCount)
            {
                Destroy(gameObject);
                return;
            }
        }
    }

    void FixedUpdate()
    {
        if (currentRoadStep >= currentRoad.positionCount)
            return;
        destination = (currentRoad.GetPosition(currentRoadStep) - transform.position).normalized;
        transform.up = destination;
        rb.MovePosition(transform.position + destination * currentSpeed * Time.deltaTime);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        TrafficLight light = collision.GetComponentInParent<TrafficLight>();
        if (light.currentState != TrafficLight.LightState.Red && !isWaiting)
        {
            rb.linearVelocity = Vector2.zero;
            currentSpeed = 0;
            TrafficLight.onRedLid += OnRedLid;
            isWaiting = true;
        }
    }

    private void OnRedLid()
    {
        currentSpeed = speed;
        isWaiting = false;
        TrafficLight.onRedLid -= OnRedLid;
    }
}
