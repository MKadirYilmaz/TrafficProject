using System.Data.Common;
using UnityEngine;
using UnityEngine.UIElements;

public class Vehicle : MonoBehaviour
{

    public float speed = 10.0f;
    private float currentSpeed;
    public LineRenderer currentRoad;
    public LayerMask vehicleLayerMask;
    private Vector3 destination;
    private Rigidbody2D rb;

    private bool isWaiting = false;
    private int currentRoadStep = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = speed;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        TrafficLight light = collision.GetComponentInParent<TrafficLight>();
        if (light.currentState != TrafficLight.LightState.Green && !isWaiting)
        {
            currentSpeed = 0;
            TrafficLight.onGreenLid += OnGreenLid;
            isWaiting = true;
        }
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
        Vector3 start = transform.position;
        Vector3 end = start + transform.up;
        Debug.DrawLine(start, end, Color.red);
        //transform.position += destination * currentSpeed * Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (currentRoadStep >= currentRoad.positionCount)
            return;
        destination = (currentRoad.GetPosition(currentRoadStep) - transform.position).normalized;
        transform.up = destination;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.up, 0.2f, vehicleLayerMask);
        if (hits.Length != 0)
        {
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject != gameObject)
                {
                    Debug.Log($"{this.name} touched to {hit.collider.name}");
                    rb.linearVelocity = Vector2.zero;
                    return;
                }    
            }
        }
        rb.MovePosition(transform.position + destination * currentSpeed * Time.deltaTime);
    }

    private void OnGreenLid()
    {
        Debug.Log("Green is lid.");
        currentSpeed = speed;
        isWaiting = false;
        TrafficLight.onGreenLid -= OnGreenLid;
    }
}
