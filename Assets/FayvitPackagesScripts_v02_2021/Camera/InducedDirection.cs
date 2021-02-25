using UnityEngine;
using System.Collections;

public class InducedDirection
{
    private bool aplicandoLerpDirNoFoco = false;
    private bool estavaAplicando = false;
    private bool estaZerado = false;
    private bool estavaZerado = false;
    private float guardeH;
    private float guardeV;
    private Vector3 direcaoGuardada;

    public void OnStartFocus()
    {
        aplicandoLerpDirNoFoco = false;
        estavaAplicando = false;
    }

    void TrocarGuardado(float h,float v,Transform cameraTransform)
    {
        direcaoGuardada = cameraTransform.TransformDirection(Vector3.forward);
        guardeH = h;
        guardeV = v;
    }

    public Vector3 Direction(bool focando, Transform cameraTransform, float h, float v)
    {
        //return cameraTransform.TransformDirection(Vector3.forward);
        Vector3 retorno;


        if (aplicandoLerpDirNoFoco)
        {
            if ((h == 0 && v == 0) || (Mathf.Abs(guardeH - h) < 0.1f && Mathf.Abs(guardeV - v) < 0.1f))
            {
                estaZerado = true;
                aplicandoLerpDirNoFoco = false;
            }
            else
                estaZerado = false;
        }

        if (!estaZerado && estavaZerado && focando)
        {
            TrocarGuardado(h, v, cameraTransform);
        }

        if (aplicandoLerpDirNoFoco)
        {
            direcaoGuardada = Vector3.Lerp(direcaoGuardada, cameraTransform.TransformDirection(Vector3.forward), 0.25f * Time.deltaTime);
            retorno = direcaoGuardada;
        }
        else
        {
            aplicandoLerpDirNoFoco = focando;
            if (estavaAplicando != aplicandoLerpDirNoFoco)
            {

                TrocarGuardado(h,v,cameraTransform);

            }else if(!focando)
                direcaoGuardada = cameraTransform.TransformDirection(Vector3.forward);

            retorno = direcaoGuardada;
        }

        estavaAplicando = aplicandoLerpDirNoFoco;
        estavaZerado = estaZerado;


        return retorno;
    }
}