using System.Collections;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public enum LightState { Green, Yellow, Red }

    public float greenDuration = 3.0f;
    public float yellowDuration = 0.5f;
    public float redDuration = 4.0f;
    public LightState currentState = LightState.Green;

    public delegate void OnLightLid();
    public static event OnLightLid onGreenLid;
    public static event OnLightLid onRedLid;
    private SpriteRenderer lightMesh;

    void OnValidate()
    {
        lightMesh = GetComponentInChildren<SpriteRenderer>();
        switch (currentState)
        {
            case LightState.Green:
                lightMesh.color = Color.green;
                break;
            case LightState.Yellow:
                lightMesh.color = Color.yellow;
                break;
            case LightState.Red:
                lightMesh.color = Color.red;
                break;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lightMesh = GetComponentInChildren<SpriteRenderer>();
        StartCoroutine(LightController());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator LightController()
    {
        while (true)
        {
            switch (currentState)
            {
                case LightState.Green:
                    yield return new WaitForSeconds(greenDuration);
                    currentState = LightState.Yellow;
                    lightMesh.color = Color.yellow;
                    break;
                case LightState.Yellow:
                    yield return new WaitForSeconds(yellowDuration);
                    currentState = LightState.Red;
                    lightMesh.color = Color.red;
                    onRedLid?.Invoke();
                    break;
                case LightState.Red:
                    yield return new WaitForSeconds(redDuration);
                    currentState = LightState.Green;
                    lightMesh.color = Color.green;
                    onGreenLid?.Invoke();
                    break;
            }
        }
    }
}
