using System.Collections;
using System.Collections.Generic;
using MLAgentsExamples;
using UnityEngine;
using System.Linq;
using TMPro;
using System;
using Unity.MLAgents;

public class Histograma : MonoBehaviour
{

    public Material dead;
    public Material black;
    public Material green;
    public Material red;
    [Tooltip("The TextMeshPro text that shows the cumulative reward of the agent")]
    public TextMeshPro cumulativeRewardText;

    public void Update()
    {
        float valor = 0;
        ConwayCube[] lista = gameObject.GetComponentsInChildren<ConwayCube>();
        foreach (ConwayCube cubo in lista)
        {
            valor += cubo.GetCumulativeReward();
        }
        valor /= 144;
        cumulativeRewardText.text = valor.ToString("0.00");
    }

    public List<int> getListaCompleto()
    {
        List<int> histograma = new List<int>();
        ConwayCube[] lista = gameObject.GetComponentsInChildren<ConwayCube>();
        foreach (ConwayCube cubo in lista)
        {
            if (cubo.actualmaterial == dead)
            {
                histograma.Add(0);
            }
            else if (cubo.actualmaterial == black)
            {
                histograma.Add(1);
            }
            else if (cubo.actualmaterial == green)
            {
                histograma.Add(2);
            }
            else if (cubo.actualmaterial == red)
            {
                histograma.Add(3);
            }
            else
            {
                histograma.Add(4);
            }
        }
        return histograma;
    }
    public int[] getHistogramaCompleto()
    {
        int[] histograma = getListaCompleto().ToArray();
        int colores0 = 0, coloresR = 0, coloresV = 0, coloresN = 0;
        foreach (int cubo in histograma)
        {
            switch (cubo)
            {
                case 0: colores0++; break;
                case 1: coloresN++; break;
                case 2: coloresV++; break;
                case 3: coloresR++; break;
                case 4: break;
            }
        }
        int[] mapa = new int[4];
        mapa[0] = colores0;
        mapa[1] = coloresN;
        mapa[2] = coloresV;
        mapa[3] = coloresR;
        return mapa;
    }
    public int[] getHistogramaColor(Material color)
    {
        int colores0 = 0, coloresR = 0, coloresV = 0, coloresN = 0;
        int[] histograma = new int[4];
        ConwayCube[] lista = gameObject.GetComponentsInChildren<ConwayCube>();
        foreach (ConwayCube cubo in lista)
        {
            if (cubo.actualmaterial == color)
            {
                int[] colores = cubo.CuentaColores();
                colores0 += colores[0];
                coloresN += colores[1];
                coloresV += colores[2];
                coloresR += colores[3];
            }
        }
        histograma[0] = colores0;
        histograma[1] = coloresN;
        histograma[2] = coloresV;
        histograma[3] = coloresR;
        return histograma;
    }

    public ConwayCube[] getLista()
    {
        ConwayCube[] lista = gameObject.GetComponentsInChildren<ConwayCube>();
        return lista;
    }
    public int getNumeroVecinos(Material color)
    {
        int[] histograma = getListaCompleto().ToArray();
        int max = histograma.Length, num, valor = 0;
        if (color == dead)
        {
            num = 0;
        }
        else if (color == black)
        {
            num = 1;
        }
        else if (color == green)
        {
            num = 2;
        }
        else if (color == red)
        {
            num = 3;
        }
        else
        {
            num = 4;
        }
        for (int i = 0; i < max; i++)
        {
            if (histograma[i] == num)
            {
                valor++;
            }
        }
        return valor;

    }
    public float[] getHistogramaColorTrampas(Material color)
    {
        int colores0 = 0, coloresR = 0, coloresV = 0, coloresN = 0, num;
        int[] histograma = getListaCompleto().ToArray();
        int max = histograma.Length;
        int[] contado = new int[max];
        for (int i = 0; i < max; i++)
        {
            contado[i] = 0;
        }
        if (color == dead)
        {
            num = 0;
        }
        else if (color == black)
        {
            num = 1;
        }
        else if (color == green)
        {
            num = 2;
        }
        else if (color == red)
        {
            num = 3;
        }
        else
        {
            num = 4;
        }
        for (int i = 0; i < max; i++)
        {
            if (histograma[i] == num)
            {
                if (i + 1 < max)
                {
                    if (contado[i + 1] == 0)
                    {
                        contado[i + 1] = 1;
                        switch (histograma[i + 1])
                        {
                            case 0: colores0++; break;
                            case 1: coloresN++; break;
                            case 2: coloresV++; break;
                            case 3: coloresR++; break;
                            case 4: break;
                        }
                    }
                }
                if (i + 11 < max)
                {
                    if (i != 0 || i % 12 != 0)
                    {
                        if (contado[i + 11] == 0)
                        {
                            contado[i + 11] = 1;
                            switch (histograma[i + 11])
                            {
                                case 0: colores0++; break;
                                case 1: coloresN++; break;
                                case 2: coloresV++; break;
                                case 3: coloresR++; break;
                                case 4: break;
                            }
                        }
                    }
                }
                if (i + 12 < max)
                {
                    if (contado[i + 12] == 0)
                    {
                        contado[i + 12] = 1;
                        switch (histograma[i + 12])
                        {
                            case 0: colores0++; break;
                            case 1: coloresN++; break;
                            case 2: coloresV++; break;
                            case 3: coloresR++; break;
                            case 4: break;
                        }
                    }
                }
                if (i + 13 < max)
                {
                    if (i != 11 || (i + 1) % 12 != 0)
                    {
                        if (contado[i + 13] == 0)
                        {
                            contado[i + 13] = 1;
                            switch (histograma[i + 13])
                            {
                                case 0: colores0++; break;
                                case 1: coloresN++; break;
                                case 2: coloresV++; break;
                                case 3: coloresR++; break;
                                case 4: break;
                            }
                        }
                    }
                }
                if (i - 1 >= 0)
                {
                    if (contado[i - 1] == 0)
                    {
                        contado[i - 1] = 1;
                        switch (histograma[i - 1])
                        {
                            case 0: colores0++; break;
                            case 1: coloresN++; break;
                            case 2: coloresV++; break;
                            case 3: coloresR++; break;
                            case 4: break;
                        }
                    }
                }
                if (i - 11 >= 0)
                {
                    if (i != 11 || ((i + 1) % 12 != 0))
                    {
                        if (contado[i - 11] == 0)
                        {
                            contado[i - 11] = 1;
                            switch (histograma[i - 11])
                            {
                                case 0: colores0++; break;
                                case 1: coloresN++; break;
                                case 2: coloresV++; break;
                                case 3: coloresR++; break;
                                case 4: break;
                            }
                        }
                    }
                }
                if (i - 12 >= 0)
                {
                    if (contado[i - 12] == 0)
                    {
                        contado[i - 12] = 1;
                        switch (histograma[i - 12])
                        {
                            case 0: colores0++; break;
                            case 1: coloresN++; break;
                            case 2: coloresV++; break;
                            case 3: coloresR++; break;
                            case 4: break;
                        }
                    }
                }
                if (i - 13 >= 0)
                {
                    if (i != 0 || (i % 12 != 0))
                    {

                        if (contado[i - 13] == 0)
                        {
                            contado[i - 13] = 1;
                            switch (histograma[i - 13])
                            {
                                case 0: colores0++; break;
                                case 1: coloresN++; break;
                                case 2: coloresV++; break;
                                case 3: coloresR++; break;
                                case 4: break;
                            }
                        }
                    }
                }
            }
        }
        float[] mapa = new float[4];
        mapa[0] = colores0;
        mapa[1] = coloresN;
        mapa[2] = coloresV;
        mapa[3] = coloresR;
        return mapa;
    }
    public float GetEntropyCompleta()
    {
        int[] histogram = getHistogramaCompleto();
        double[] lista = new double[histogram.Length];
        double entropia = 0;
        for (int i = 0; i < histogram.Length; i++)
        {
            lista[i] = histogram[i];
            lista[i] = lista[i] / (144);
            if (lista[i] != 0)
            {
                entropia += (lista[i] * Math.Log(lista[i], 4));
            }
        }
        return -(float)entropia;
    }
    public float GetEntropyCompleta2()
    {
        int[] histogram = getHistogramaCompleto();
        double[] lista = new double[histogram.Length];
        double entropia = 0;
        int muertos = histogram[0];

        for (int i = 1; i < histogram.Length; i++)
        {
            lista[i] = histogram[i];
            if (lista[i] != 0)
            {
                if (muertos != 144)
                {
                    lista[i] = lista[i] / (144 - muertos);
                }
                else
                {
                    lista[i] = 0;
                }

                entropia += (lista[i] * Math.Log(lista[i], 3));
            }
        }
        return -(float)entropia;
    }
    public float GetMediaDistancias(Material color, String nombre)
    {
        ConwayCube[] lista = gameObject.GetComponentsInChildren<ConwayCube>();
        float suma = 0, num = getNumeroVecinos(color);
        int pFrom = nombre.IndexOf("BasicAgent 0.") + "BasicAgent 0.".Length;
        int pTo = nombre.LastIndexOf(")");
        String aux = nombre.Substring(pFrom, pTo - pFrom);
        pTo = aux.LastIndexOf("(");
        int col = int.Parse(aux.Substring(0, pTo));
        int fila = int.Parse(aux.Substring(pTo + 1, aux.Length - pTo - 1));
        for (int i = 0; i < lista.Length; i += 12)
        {
            for (int j = 0; j < 11; j++)
            {
                if (lista[i + j].actualmaterial == color)
                {
                    suma += Math.Abs(fila - (i) / 12) + Math.Abs(col - j);
                }
            }
        }
        return suma / num;
    }
}