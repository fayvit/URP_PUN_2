using UnityEngine;
using System.Collections;

namespace FayvitCommandReader
{
    public class ChangeController
    {

        public static Controlador ControllerInUse(Controlador atual)
        {
            Controlador entrada = atual;

            if (Input.touchCount > 0)
                atual = Controlador.Android;
            else if (Input.GetKeyDown(KeyCode.BackQuote) || Input.GetKeyDown(KeyCode.Space))
                atual = Controlador.teclado;
            else for (int i = 1; i <= 4; i++)
                    if (CommandReader.VerifyThisControlUse(i))
                        atual = (Controlador)i;

            if (entrada != atual)
                Debug.Log("Controlador modificado para " + atual);

            return atual;
        }

        
    }
}
