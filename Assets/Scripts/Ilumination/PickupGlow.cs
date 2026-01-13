using UnityEngine;

public class PickupGlow : MonoBehaviour
{
    public Color glowColor = Color.yellow;
    public float minIntensity = 0.5f;
    public float maxIntensity = 2.5f;
    public float pulseSpeed = 2f;

    private Material material;
    private float t;
    private bool collected = false;

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        material = renderer.material;
        material.EnableKeyword("_EMISSION");
    }

    void Update()
    {
        if (collected) return;

        t += Time.deltaTime * pulseSpeed;

        float intensity = Mathf.Lerp(
            minIntensity,
            maxIntensity,
            (Mathf.Sin(t) + 1f) / 2f
        );

        material.SetColor("_EmissionColor", glowColor * intensity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    void Collect()
    {
        collected = true;

        // Apaga el brillo
        material.SetColor("_EmissionColor", Color.black);
        // - enviar evento

        Destroy(gameObject);
    }
}
