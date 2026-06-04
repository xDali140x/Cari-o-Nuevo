using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ContornoInteractuableAutomatico : MonoBehaviour
{
    [Header("Material")]
    [Tooltip("Usa MAT_ContornoInteractuable.")]
    public Material materialContorno;

    [Header("Manos o controles")]
    [Tooltip("Transform que sigue la mano o el control izquierdo.")]
    public Transform manoIzquierda;

    [Tooltip("Transform que sigue la mano o el control derecho.")]
    public Transform manoDerecha;

    [Header("Distancias")]
    [Tooltip("El contorno aparece cuando una mano entra en esta distancia.")]
    [Min(0.01f)]
    public float distanciaParaMostrar = 0.5f;

    [Tooltip("El contorno desaparece cuando la mano ya está prácticamente tocando el objeto.")]
    [Min(0f)]
    public float distanciaDeContacto = 0.025f;

    [Header("Opcional")]
    [Tooltip("Puedes dejarlo vacío. El script buscará los colliders automáticamente.")]
    public Collider[] collidersDelObjeto;

    private readonly List<Renderer> renderersDeContorno =
        new List<Renderer>();

    private readonly List<GameObject> objetosGenerados =
        new List<GameObject>();

    private bool contornoVisible;

    private void Awake()
    {
        if (materialContorno == null)
        {
            Debug.LogError(
                $"Falta asignar el material de contorno en {name}.",
                this
            );

            enabled = false;
            return;
        }

        if (collidersDelObjeto == null ||
            collidersDelObjeto.Length == 0)
        {
            collidersDelObjeto =
                GetComponentsInChildren<Collider>(true);
        }

        GenerarContornosAutomaticamente();
        CambiarVisibilidad(false, true);
    }

    private void Update()
    {
        float distanciaMasCercana = Mathf.Min(
            CalcularDistancia(manoIzquierda),
            CalcularDistancia(manoDerecha)
        );

        bool estaCerca =
            distanciaMasCercana <= distanciaParaMostrar;

        bool estaTocando =
            distanciaMasCercana <= distanciaDeContacto;

        CambiarVisibilidad(estaCerca && !estaTocando);
    }

    private float CalcularDistancia(Transform mano)
    {
        if (mano == null)
        {
            return Mathf.Infinity;
        }

        float distanciaMenor = Mathf.Infinity;

        foreach (Collider col in collidersDelObjeto)
        {
            if (col == null || !col.enabled)
            {
                continue;
            }

            Vector3 puntoCercano =
                col.ClosestPoint(mano.position);

            float distancia =
                Vector3.Distance(mano.position, puntoCercano);

            if (distancia < distanciaMenor)
            {
                distanciaMenor = distancia;
            }
        }

        return distanciaMenor;
    }

    private void GenerarContornosAutomaticamente()
    {
        MeshFilter[] meshFilters =
            GetComponentsInChildren<MeshFilter>(true);

        SkinnedMeshRenderer[] skinnedRenderers =
            GetComponentsInChildren<SkinnedMeshRenderer>(true);

        foreach (MeshFilter meshFilter in meshFilters)
        {
            MeshRenderer rendererOriginal =
                meshFilter.GetComponent<MeshRenderer>();

            if (rendererOriginal == null ||
                meshFilter.sharedMesh == null)
            {
                continue;
            }

            GameObject copia =
                new GameObject($"__Contorno_{rendererOriginal.name}");

            copia.transform.SetParent(
                rendererOriginal.transform,
                false
            );

            copia.layer = rendererOriginal.gameObject.layer;

            MeshFilter filtroCopia =
                copia.AddComponent<MeshFilter>();

            filtroCopia.sharedMesh =
                meshFilter.sharedMesh;

            MeshRenderer rendererCopia =
                copia.AddComponent<MeshRenderer>();

            rendererCopia.sharedMaterials =
                CrearListaDeMateriales(
                    meshFilter.sharedMesh.subMeshCount
                );

            rendererCopia.shadowCastingMode =
                ShadowCastingMode.Off;

            rendererCopia.receiveShadows = false;

            objetosGenerados.Add(copia);
            renderersDeContorno.Add(rendererCopia);
        }

        foreach (SkinnedMeshRenderer rendererOriginal
                 in skinnedRenderers)
        {
            if (rendererOriginal.sharedMesh == null)
            {
                continue;
            }

            GameObject copia =
                new GameObject($"__Contorno_{rendererOriginal.name}");

            copia.transform.SetParent(
                rendererOriginal.transform,
                false
            );

            copia.layer = rendererOriginal.gameObject.layer;

            SkinnedMeshRenderer rendererCopia =
                copia.AddComponent<SkinnedMeshRenderer>();

            rendererCopia.sharedMesh =
                rendererOriginal.sharedMesh;

            rendererCopia.bones =
                rendererOriginal.bones;

            rendererCopia.rootBone =
                rendererOriginal.rootBone;

            rendererCopia.localBounds =
                rendererOriginal.localBounds;

            rendererCopia.sharedMaterials =
                CrearListaDeMateriales(
                    rendererOriginal.sharedMesh.subMeshCount
                );

            rendererCopia.shadowCastingMode =
                ShadowCastingMode.Off;

            rendererCopia.receiveShadows = false;

            objetosGenerados.Add(copia);
            renderersDeContorno.Add(rendererCopia);
        }
    }

    private Material[] CrearListaDeMateriales(int cantidad)
    {
        int total = Mathf.Max(1, cantidad);

        Material[] materiales =
            new Material[total];

        for (int i = 0; i < total; i++)
        {
            materiales[i] = materialContorno;
        }

        return materiales;
    }

    private void CambiarVisibilidad(
        bool mostrar,
        bool forzar = false
    )
    {
        if (!forzar && contornoVisible == mostrar)
        {
            return;
        }

        contornoVisible = mostrar;

        foreach (Renderer rendererContorno
                 in renderersDeContorno)
        {
            if (rendererContorno != null)
            {
                rendererContorno.enabled = mostrar;
            }
        }
    }

    private void OnDestroy()
    {
        foreach (GameObject objetoGenerado
                 in objetosGenerados)
        {
            if (objetoGenerado != null)
            {
                Destroy(objetoGenerado);
            }
        }
    }

    private void OnValidate()
    {
        if (distanciaDeContacto > distanciaParaMostrar)
        {
            distanciaDeContacto = distanciaParaMostrar;
        }
    }
}