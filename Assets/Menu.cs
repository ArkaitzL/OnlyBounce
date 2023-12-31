using UnityEngine.UI;
using TMPro;
using UnityEngine;
using BaboOnLite;

public class Menu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mejor_txt;

    [SerializeField] private Image imagen;
    [SerializeField] private Sprite desmuteado, muteado;

    private void Start()
    {
        mejor_txt.text = Save.Data.mejor_puntuacion.ToString();
        Aplicar();
    }

    public void Mutear()
    {
        Save.Data.muteado = !Save.Data.muteado;
        Aplicar();
    }

    private void Aplicar()
    {
        Instanciar<Musica>.Coger().Cambiar();
        imagen.sprite = (Save.Data.muteado) ? muteado : desmuteado;
    }
}
