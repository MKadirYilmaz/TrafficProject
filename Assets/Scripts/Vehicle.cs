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

    private TrafficLight prevLight;
    private bool isWaiting = false;
    private bool canPass = false;
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
        if (light != prevLight) // If vehicle reaches another light reset the canPass value
            canPass = false;

        if (light.currentState == TrafficLight.LightState.Green)
        {
            canPass = true;
        }
        else if (!isWaiting && !canPass)
        {
            currentSpeed = 0;
            TrafficLight.onGreenLid += OnGreenLid;
            isWaiting = true;
        }

        prevLight = light;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, currentRoad.GetPosition(currentRoadStep)) < 0.02)
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
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position + transform.up * 0.1f, new Vector2(0.3f, 0.3f), 0, transform.up, 0.1f, vehicleLayerMask);
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
