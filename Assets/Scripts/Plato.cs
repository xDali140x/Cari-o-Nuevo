using UnityEngine;

public class Plato : MonoBehaviour
{
    public Collider colliderActivador;

    public Collider[] collidersADesaparecer;

    public float tiempoNecesario = 3f;

    private float[] contadores;
    private bool[] yaDesaparecio;

    void Start()
    {
        contadores = new float[collidersADesaparecer.Length];
        yaDesaparecio = new bool[collidersADesaparecer.Length];
    }

    void Update()
    {
        if (colliderActivador == null) return;
        if (collidersADesaparecer == null || collidersADesaparecer.Length == 0) return;

        for (int i = 0; i < collidersADesaparecer.Length; i++)
        {
            Collider col = collidersADesaparecer[i];

            if (col == null) continue;
            if (yaDesaparecio[i]) continue;

            if (colliderActivador.bounds.Intersects(col.bounds))
            {
                contadores[i] += Time.deltaTime;

                if (contadores[i] >= tiempoNecesario)
                {
                    MeshRenderer mesh = col.GetComponent<MeshRenderer>();

                    if (mesh != null)
                    {
                        mesh.enabled = false;
                    }

                    yaDesaparecio[i] = true;
                }
            }
            else
            {
                contadores[i] = 0f;
            }
        }
    }
}