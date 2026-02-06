using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLogic : MonoBehaviour
{
    [Header("Paneles")]
    [SerializeField] private GameObject panelOpciones;

    [Header("Nombres de Escenas")]
    [SerializeField] private string EscenaHistoria;
    [SerializeField] private string EscenaPVP;
    [SerializeField] private string EscenaLevelEditor;

    void Start()
    {
       
    }

   

    public void AbrirModoHistoria()
    {
        ConfiHistoria();
        if (!string.IsNullOrEmpty(EscenaHistoria))
        {
            Debug.Log($"Cargando modo historia: {EscenaHistoria}");
            SceneManager.LoadScene(EscenaHistoria);
        }
       
    }

    public void AbrirModoPVP()
    {
        ConfiPVP();
        if (!string.IsNullOrEmpty(EscenaPVP))
        {
            Debug.Log($"Cargando modo PVP: {EscenaPVP}");
            SceneManager.LoadScene(EscenaPVP);
        }
       
    }

    public void AbrirLevelEditor()
    {
        ConfiLevelEditor();
        if (!string.IsNullOrEmpty(EscenaLevelEditor))
        {
            Debug.Log($"Cargando editor de niveles: {EscenaLevelEditor}");
            SceneManager.LoadScene(EscenaLevelEditor);
        }
      
    }

    public void MostrarOpciones()
    {
        if (panelOpciones != null)
        {
            panelOpciones.SetActive(true);
            Debug.Log("Panel de opciones activado");
        }
    }

    public void OcultarOpciones()
    {
        if (panelOpciones != null)
        {
            panelOpciones.SetActive(false);
            Debug.Log("Panel de opciones desactivado");
        }
    }

    /// <summary>
    /// Me he de llegir las merdes encara per saber com configurar
    /// </summary>

    public void ConfiHistoria() { }
   
    public void ConfiPVP() { }
    public void ConfiLevelEditor() { }
}