using UnityEngine;

[RequireComponent(typeof(Light))]
public class ChristmasLight : MonoBehaviour
{
    public Color[] colors =
    {
        Color.red,
        Color.green,
        Color.blue
    };

    [Tooltip("Tiempo que tarda en pasar de un color a otro")]
    public float transitionDuration = 1.5f;

    private Light bulbLight;
    private int currentIndex = 0;
    private int nextIndex = 1;
    private float t = 0f;

    void Start()
    {
        bulbLight = GetComponent<Light>();
        bulbLight.type = LightType.Point;
        bulbLight.color = colors[currentIndex];
    }

    void Update()
    {
        t += Time.deltaTime / transitionDuration;

        bulbLight.color = Color.Lerp(
            colors[currentIndex],
            colors[nextIndex],
            t
        );

        if (t >= 1f)
        {
            t = 0f;
            currentIndex = nextIndex;
            nextIndex = (nextIndex + 1) % colors.Length;
        }
    }
}
