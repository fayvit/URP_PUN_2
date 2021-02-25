using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FayvitMove;

[System.Serializable]
public class ControlledMoveForCharacter
{
    [SerializeField] private BasicMove mov;

    private GameObject oControlado;    
    private NavMeshPath path;

    private int indiceDaDirecao = 0;

    public BasicMove Mov
    {
        get { return mov; }
    }

    public ControlledMoveForCharacter(Transform T)
    {
        mov = new BasicMove();// (T);
        mov.StartFields(T);
        oControlado = T.gameObject;
    }

    public void StartFields(Transform T)
    {
        mov.StartFields(T);
        oControlado = T.gameObject;
    }


    public void ModificarOndeChegar(Vector3 ondeChegar,float velocidade=-1)
    {
        if(velocidade>=0)
            mov.ChangeWalkSpeed(velocidade);

       // if (mov.NoChao())
        {

            path = new NavMeshPath();
            NavMeshHit navHit = new NavMeshHit();
            NavMeshHit navHit2 = new NavMeshHit();

            if(oControlado!=null)
                if (NavMesh.SamplePosition(ondeChegar, out navHit, 10, 1)
                    && NavMesh.SamplePosition(oControlado.transform.position, out navHit2, 10, 1))
                {
                    //Debug.Log(
                    NavMesh.CalculatePath(navHit2.position, navHit.position, 1, path);//);
                }

            //Debug.Log("cantos da Path: " + path.corners.Length);

            indiceDaDirecao = 1;
        }
    }

    public void TestepuloBoxOverlap(Vector3 pos)
    {
        
        Vector3 centerBox = oControlado.transform.position + oControlado.transform.forward *1.1f* mov.Controller.radius+mov.Controller.center;
        Vector3 extendsBox = new Vector3(mov.Controller.radius, 0.5f*Mathf.Max(0.1f, mov.Controller.height - mov.Controller.radius, 0.2f));

        Collider[] C = Physics.OverlapBox(centerBox, extendsBox,Quaternion.LookRotation(oControlado.transform.forward));
        if (C.Length > 0)                                                     
        {
            for (int i = 0; i < C.Length; i++)
            {
//                Debug.Log(C[i].gameObject.name);

                //if(C[i].gameObject.layer==9 && mov.NoChao())
                  //  mov._Pulo.IniciaAplicaPulo(oControlado.transform.position.y);
            }
            
        }
    }

    /*
    public void TestePuloRaycastOriginal(Vector3 pos)
    {
        Transform T = oControlado.transform;
        RaycastHit hit = new RaycastHit();
        Vector3 proj = Vector3.ProjectOnPlane((path.corners[indiceDaDirecao] - pos), Vector3.up);
        if (Vector3.Distance(T.forward, proj.normalized) < 0.5f)
            if(  Physics.Raycast(pos,T.forward, out hit, 1))
                if (Vector3.Angle(hit.normal, Vector3.up) > 70
                     && Vector3.Angle(hit.normal, Vector3.up) < 110
                    && !mov._Pulo.EstouPulando)
                        mov._Pulo.IniciaAplicaPulo(T.position.y);
    }*/

    public bool UpdatePosition(float pathDistanceCheck=1)
    {
        bool retorno = false;
        if (path != null)
        {
            if (indiceDaDirecao < path.corners.Length)
            {
                Vector3 pos = oControlado.transform.position;
                mov.MoveApplicator(Vector3.ProjectOnPlane(path.corners[indiceDaDirecao] - pos, Vector3.up).normalized);

                TestepuloBoxOverlap(pos);

                if (Vector3.Distance(path.corners[indiceDaDirecao], pos) < pathDistanceCheck /*&& mov.NoChao()*/)
                    indiceDaDirecao++;

            }
            else
            {
                mov.MoveApplicator(Vector3.zero);
                //AnimacaoPadrao();
                retorno = true;
            }
        }
        else
        {
            Debug.Log("PathNUll");
            return true;            
        }
        return retorno;
    }


    public void AnimacaoPadrao()
    {
       // mov.Animador.SetFloat("velocidade", 0);
        //mov.MoveApplicator(Vector3.zero);
        //mov._Pulo.NaoEstouPulando();
        //mov.Animador.SetBool("pulo", !mov.NoChao());
    }

    public void Destruir()
    {
        MonoBehaviour.Destroy(oControlado.GetComponent<CharacterController>());
    }


}
