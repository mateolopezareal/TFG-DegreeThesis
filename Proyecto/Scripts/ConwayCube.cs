using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
using UnityScript.Steps;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
public class ConwayCube : Agent
{
    [Header("¿Que color es?")]
    public int tipo;
    [Header("Tiempo entre decisiones")]
    public float timeBetweenDecisionsAtInference;
    [Header("Materials")]
    public Material dead;
    public Material black;
    public Material green;
    public Material red;
    [HideInInspector]
    public Material actualmaterial;
    [HideInInspector]
    public bool contado;
    private float m_TimeSinceDecision;
    private Material cubeBack, cubeForward, cubeRight, cubeLeft, cubeForwardRight, cubeForwardLeft, cubeBackRight, cubeBackLeft;
    private Histograma histograma;
    private float entropiaC, entropiaN, entropiaV, entropiaR;
    private int colores0, coloresN, coloresV, coloresR;
    //Método de incialización de un Objeto en Unity
    public override void Initialize()
    {
        histograma = gameObject.GetComponentInParent<Histograma>();
        entropiaC = histograma.GetEntropyCompleta2();
        ChooseRandomColor();
        base.Initialize();
        int[] colores = CuentaColores();
        colores0 = colores[0];
        coloresN = colores[1];
        coloresV = colores[2];
        coloresR = colores[3];
    }
    //Métodod para el cambio de color del agente
    public void EstaMuerto()
    {
        gameObject.GetComponentInChildren<Renderer>().material = dead;
        actualmaterial = dead;
    }
    public void EstaRojo()
    {
        gameObject.GetComponentInChildren<Renderer>().material = red;
        actualmaterial = red;
    }
    public void EstaVerde()
    {
        gameObject.GetComponentInChildren<Renderer>().material = green;
        actualmaterial = green;
    }
    public void EstaNegro()
    {
        gameObject.GetComponentInChildren<Renderer>().material = black;
        actualmaterial = black;
    }

    //Método llamado antes de recibir la acción donde se pasa información del entorno a través de observaciones al agente
    public override void CollectObservations(VectorSensor sensor)
    {
        //Si obervaciones solo vecinos
        /*
        List<Material> lista = new List<Material>();
        lista.Add(actualmaterial);
        lista.Add(cubeForward);
        lista.Add(cubeForwardRight);
        lista.Add(cubeRight);
        lista.Add(cubeBackRight);
        lista.Add(cubeBack);
        lista.Add(cubeBackLeft);
        lista.Add(cubeLeft);
        lista.Add(cubeForwardLeft);
        List<float> listanum = new List<float>();
        foreach (Material material in lista)
        {
            if (material == dead)
            {
                //AddVectorObs(0);
                listanum.Add(0);
            }
            else if (material == black)
            {
                //AddVectorObs(1);
                listanum.Add(1);
            }
            else if (material == green)
            {
                //AddVectorObs(2);
                listanum.Add(2);
            }
            else if (material == red)
            {
                //AddVectorObs(3);
                listanum.Add(3);
            }
            else
            {
                //AddVectorObs(4);
                listanum.Add(4);
            }
        }
        foreach (int num in listanum)
        {
            sensor.AddOneHotObservation(num,4);
        }
        */

        //Si obervaciones malla entera
        foreach (int cubo in histograma.getListaCompleto())
        {
            sensor.AddOneHotObservation(cubo, 4);
        }
    }
    public override void OnActionReceived(float[] vectorAction)
    {
        CambiaColor(vectorAction);
        contado = true;
    }
    //Método llamado cada vez que llegue una acción de la política
    private void CambiaColor(float[] vectorAction)
    {
        var color = (int)vectorAction[0];
        if (actualmaterial == dead)
        {
            if (coloresV == 0 && coloresR == 0 && coloresN != 0)
                EstaNegro();
            else if (coloresN == 0 && coloresR == 0 && coloresV != 0)
                EstaVerde();
            else if (coloresV == 0 && coloresN == 0 && coloresR != 0)
                EstaRojo();
            else if (coloresV == 0 && coloresR == 0 && coloresN == 0)
                EstaMuerto();
            else
            {
                switch (color)
                {
                    case 0:
                        EstaMuerto();
                        break;
                    case 1:
                        EstaNegro();
                        break;
                    case 2:
                        EstaVerde();
                        break;
                    case 3:
                        EstaRojo();
                        break;
                }
            }
        }
        else
        {
            switch (color)
            {
                case 0:
                    EstaMuerto();
                    break;
                case 1:
                    EstaNegro();
                    break;
                case 2:
                    EstaVerde();
                    break;
                case 3:
                    EstaRojo();
                    break;
            }
        }
    }

    //Método para que el agente se de cuenta a partir de RayCasting de los agentes a su alrededor
    public int[] CuentaColores()
    {
        int coloresN = 0, coloresR = 0, coloresV = 0, colores0 = 0;
        Ray cubeRayForward = new Ray(transform.position, Vector3.forward);
        Ray cubeRayBack = new Ray(transform.position, Vector3.back);
        Ray cubeRayRight = new Ray(transform.position, Vector3.right);
        Ray cubeRayLeft = new Ray(transform.position, Vector3.left);
        Ray cubeRayForwardRight = new Ray(transform.position, new Vector3(1, 0, 1));
        Ray cubeRayForwardLeft = new Ray(transform.position, new Vector3(-1, 0, 1));
        Ray cubeRayBackRight = new Ray(transform.position, new Vector3(1, 0, -1));
        Ray cubeRayBackLeft = new Ray(transform.position, new Vector3(-1, 0, -1));
        RaycastHit hit;
        if (Physics.Raycast(cubeRayForward, out hit, 1))
        {
            if (hit.collider.tag == "cube")
            {
                cubeForward = hit.collider.gameObject.GetComponentInChildren<ConwayCube>().actualmaterial;
                if (cubeForward == dead)
                {
                    colores0++;
                }
                else if (cubeForward == black)
                {
                    coloresN++;
                }
                else if (cubeForward == red)
                {
                    coloresR++;
                }
                else if (cubeForward == green)
                {
                    coloresV++;
                }
            }
        }
        if (Physics.Raycast(cubeRayBack, out hit, 1))
        {
            if (hit.collider.tag == "cube")
            {
                cubeBack = hit.collider.gameObject.GetComponentInChildren<ConwayCube>().actualmaterial;
                if (cubeBack == dead)
                {
                    colores0++;
                }
                else if (cubeBack == black)
                {
                    coloresN++;
                }
                else if (cubeBack == red)
                {
                    coloresR++;
                }
                else if (cubeBack == green)
                {
                    coloresV++;
                }
            }
        }
        if (Physics.Raycast(cubeRayRight, out hit, 1))
        {
            if (hit.collider.tag == "cube")
            {
                cubeRight = hit.collider.gameObject.GetComponentInChildren<ConwayCube>().actualmaterial;
                if (cubeRight == dead)
                {
                    colores0++;
                }
                else if (cubeRight == black)
                {
                    coloresN++;
                }
                else if (cubeRight == red)
                {
                    coloresR++;
                }
                else if (cubeRight == green)
                {
                    coloresV++;
                }
            }
        }
        if (Physics.Raycast(cubeRayLeft, out hit, 1))
        {
            if (hit.collider.tag == "cube")
            {
                cubeLeft = hit.collider.gameObject.GetComponentInChildren<ConwayCube>().actualmaterial;
                if (cubeLeft == dead)
                {
                    colores0++;
                }
                else if (cubeLeft == black)
                {
                    coloresN++;
                }
                else if (cubeLeft == red)
                {
                    coloresR++;
                }
                else if (cubeLeft == green)
                {
                    coloresV++;
                }
            }
        }
        if (Physics.Raycast(cubeRayForwardRight, out hit, 2))
        {

            if (hit.collider.tag == "cube")
            {
                cubeForwardRight = hit.collider.gameObject.GetComponentInChildren<ConwayCube>().actualmaterial;
                if (cubeForwardRight == dead)
                {
                    colores0++;
                }
                else if (cubeForwardRight == black)
                {
                    coloresN++;
                }
                else if (cubeForwardRight == red)
                {
                    coloresR++;
                }
                else if (cubeForwardRight == green)
                {
                    coloresV++;
                }
            }
        }
        if (Physics.Raycast(cubeRayForwardLeft, out hit, 2))
        {
            if (hit.collider.tag == "cube")
            {
                cubeForwardLeft = hit.collider.gameObject.GetComponentInChildren<ConwayCube>().actualmaterial;
                if (cubeForwardLeft == dead)
                {
                    colores0++;
                }
                else if (cubeForwardLeft == black)
                {
                    coloresN++;
                }
                else if (cubeForwardLeft == red)
                {
                    coloresR++;
                }
                else if (cubeForwardLeft == green)
                {
                    coloresV++;
                }
            }
        }
        if (Physics.Raycast(cubeRayBackRight, out hit, 2))
        {
            if (hit.collider.tag == "cube")
            {
                cubeBackRight = hit.collider.gameObject.GetComponentInChildren<ConwayCube>().actualmaterial;
                if (cubeBackRight == dead)
                {
                    colores0++;
                }
                else if (cubeBackRight == black)
                {
                    coloresN++;
                }
                else if (cubeBackRight == red)
                {
                    coloresR++;
                }
                else if (cubeBackRight == green)
                {
                    coloresV++;
                }
            }
        }
        if (Physics.Raycast(cubeRayBackLeft, out hit, 2))
        {
            if (hit.collider.tag == "cube")
            {
                cubeBackLeft = hit.collider.gameObject.GetComponentInChildren<ConwayCube>().actualmaterial;
                if (cubeBackLeft == dead)
                {
                    colores0++;
                }
                else if (cubeBackLeft == black)
                {
                    coloresN++;
                }
                else if (cubeBackLeft == red)
                {
                    coloresR++;
                }
                else if (cubeBackLeft == green)
                {
                    coloresV++;
                }
            }
        }
        int[] colores = new int[4];
        colores[0] = colores0;
        colores[1] = coloresN;
        colores[2] = coloresV;
        colores[3] = coloresR;
        return colores;
    }
    
    public void ChooseRandomColor()
    {
        if (UnityEngine.Random.Range(0, 23) == 3)
        {
            switch (UnityEngine.Random.Range(0, 3))
            {
                case 0:
                    EstaNegro();
                    break;
                case 1:
                    EstaVerde();
                    break;
                case 2:
                    EstaRojo();
                    break;
            }
        }
        else
            EstaMuerto();

    }
    //Método que se llama al comienzo de cada episodio, es llamado cada resteo del agente
    public override void OnEpisodeBegin()
    {
        ChooseRandomColor();
        entropiaC = histograma.GetEntropyCompleta2();
        int[] colores = CuentaColores();
        colores0 = colores[0];
        coloresN = colores[1];
        coloresV = colores[2];
        coloresR = colores[3];
    }
    //Método para que los agentes se actualicen a la vez de forma fija
    public void FixedUpdate()
    {
        WaitTimeInference();
        int[] colores = CuentaColores();
        colores0 = colores[0];
        coloresN = colores[1];
        coloresV = colores[2];
        coloresR = colores[3];
        entropiaC = histograma.GetEntropyCompleta2();
        float vecinos = histograma.GetMediaDistancias(actualmaterial, name);
        if (actualmaterial != dead) {
            if (actualmaterial == black)
            {
                AddReward(0.01f * coloresN);
            }
            else if (actualmaterial == green)
            {
                AddReward(0.01f * coloresV);
            }
            else if (actualmaterial == red)
            {
                AddReward(0.01f * coloresR);
            }
            //El valor que multiplica la entropiaC cambiará dependiendo del objetivo y politicas añadidas
            AddReward(0.4f*entropiaC);
            if (vecinos != 0)
            {
                AddReward(1 / vecinos);
            }
        }
        else
            AddReward(-1f);
    }
    
    void WaitTimeInference()
    {
        if (!Academy.Instance.IsCommunicatorOn)
        {
            RequestDecision();
        }
        else
        {
            if (m_TimeSinceDecision >= timeBetweenDecisionsAtInference)
            {
                m_TimeSinceDecision = 0f;
                RequestDecision();
            }
            else
            {
                m_TimeSinceDecision += Time.fixedDeltaTime;
            }
        }
    }

}
