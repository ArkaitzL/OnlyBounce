using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

namespace BaboOnLite 
{
    [DefaultExecutionOrder(0)]
    //[AddComponentMenu("BaboOnLite/Fps")]
    [DisallowMultipleComponent]
    [HelpURL("https://docs.google.com/document/d/1zPv7QP-ZyisadG5zREiMmzV7UWsYTUPZIPT0f_YlhSE/edit?usp=sharing")]

    public partial class Fps {
        [SerializeField] private TextMeshProUGUI fpsText;
    }
    public partial class Fps : MonoBehaviour
    {
        void Start()
        {
            //Contador FPS
            List<int> media = new List<int>();
            ControladorBG.Rutina(.01f, () => {
                float fpsCont = 1f / Time.deltaTime;
                media.Add(Mathf.RoundToInt(fpsCont));
            }, true);

            ControladorBG.Rutina(1f, () => {
                fpsText.text = $"{(int)media.Average()} FPS";
            }, true);
        }
    }
}
