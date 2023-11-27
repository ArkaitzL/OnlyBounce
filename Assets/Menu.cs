using UnityEngine.UI;
using TMPro;
using UnityEngine;
using BaboOnLite;

public class Menu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mejor_txt;

    private void Start()
    {
        mejor_txt.text = Save.Data.mejor_puntuacion.ToString();
    }
}
