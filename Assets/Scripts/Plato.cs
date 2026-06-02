using UnityEngine;

public class Plato : MonoBehaviour
{
    public Collider colliderActivador;

    public Collider[] collidersADesaparecer;

    public float tiempoNecesario = 3f;
    public bool soloUnaVez = true;

    private float contadorTiempo = 0f;
    private bool yaSeActivo = false;

    void Update()
    {
        if (colliderActivador == null) return;
        if (collidersADesaparecer == null || collidersADesaparecer.Length == 0) return;
        if (soloUnaVez && yaSeActivo) return;

        bool estaTocando = false;

        foreach (Collider col in collidersADesaparecer)
        {
            if (col == null) continue;

            if (colliderActivador.bounds.Intersects(col.bounds))
            {
                estaTocando = true;
                break;
            }
        }

        if (estaTocando)
        {
            contadorTiempo += Time.deltaTime;

            if (contadorTiempo >= tiempoNecesario)
            {
                foreach (Collider col in collidersADesaparecer)
                {
                    if (col == null) continue;

                    MeshRenderer mesh = col.GetComponent<MeshRenderer>();

                    if (mesh != null)
                    {
                        mesh.enabled = false;
                    }
                }

                yaSeActivo = true;
            }
        }
        else
        {
            contadorTiempo = 0f;
        }
    }
}