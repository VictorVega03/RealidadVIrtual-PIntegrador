using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;   // <-- IMPORTANTE

public class PreguntaOperacion : MonoBehaviour
{
    public TMP_Text preguntaTexto;   // <-- CAMBIO A TMP_Text
    public Button[] botones;
    public string siguienteEscena;

    private int respuestaCorrecta;

    void Start()
    {
        GenerarPregunta();
    }

    void GenerarPregunta()
    {
        int a = UnityEngine.Random.Range(1, 10);
        int b = UnityEngine.Random.Range(1, 10);

        string escena = SceneManager.GetActiveScene().name;

        // -------------------------------------------------
        // Operaciones según escena
        // -------------------------------------------------

        if (escena == "ExamSuma")
        {
            preguntaTexto.text = $"¿Cuánto es {a} + {b}?";
            respuestaCorrecta = a + b;
        }
        else if (escena == "ExamResta")
        {
            preguntaTexto.text = $"¿Cuánto es {a} - {b}?";
            respuestaCorrecta = a - b;
        }
        else if (escena == "ExamMulti")
        {
            preguntaTexto.text = $"¿Cuánto es {a} x {b}?";
            respuestaCorrecta = a * b;
        }
        else if (escena == "ExamDivision")
        {
            b = UnityEngine.Random.Range(1, 10);
            int resultado = a * b;

            preguntaTexto.text = $"¿Cuánto es {resultado} ÷ {a}?";
            respuestaCorrecta = b;
        }

        // -------------------------------------------------
        // Opciones de respuesta
        // -------------------------------------------------

        int posCorrecta = UnityEngine.Random.Range(0, botones.Length);

        for (int i = 0; i < botones.Length; i++)
        {
            int valor;

            if (i == posCorrecta)
                valor = respuestaCorrecta;
            else
                valor = respuestaCorrecta + UnityEngine.Random.Range(-4, 5);

            if (valor == respuestaCorrecta && i != posCorrecta)
                valor += UnityEngine.Random.Range(1, 5);

            botones[i].GetComponentInChildren<TMP_Text>().text = valor.ToString();

            int respuestaBoton = valor;
            botones[i].onClick.RemoveAllListeners();
            botones[i].onClick.AddListener(() => SeleccionarRespuesta(respuestaBoton));
        }
    }

    void SeleccionarRespuesta(int valor)
    {
        int aciertos = PlayerPrefs.GetInt("aciertosTotal", 0);

        if (valor == respuestaCorrecta)
            aciertos++;

        PlayerPrefs.SetInt("aciertosTotal", aciertos);

        SceneManager.LoadScene(siguienteEscena);
    }
}
