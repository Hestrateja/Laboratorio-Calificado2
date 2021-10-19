using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Another_Test : MonoBehaviour
{
    public Text txt_msg;

    void Start()
    {
        new Thread(Run).Start();
        InvokeRepeating("LoopSec", 0, 1);
    }

    int secCount = 0;

    void LoopSec()
    {
        print("Un segundo para recibir:" + secCount + "Tiempos");
        secCount = 0;
    }

    float AccX = 0, AccY = 0, AccZ = 0, Temp = 0, GyroX = 0, GyroY = 0, GyroZ = 0;

    void Update()
    {
        txt_msg.text = "";
        txt_msg.text += "AccX : " + AccX.ToString("0.00");
        txt_msg.text += "    ";
        txt_msg.text += "AccY : " + AccY.ToString("0.00");
        txt_msg.text += "    ";
        txt_msg.text += "AccZ : " + AccZ.ToString("0.00");
        txt_msg.text += "\n";

        txt_msg.text += "Temp : " + Temp.ToString("0.00");
        txt_msg.text += "    ";
        txt_msg.text += "\n";

        txt_msg.text += "GyroX : " + GyroX.ToString("0.00");
        txt_msg.text += "    ";
        txt_msg.text += "GyroY : " + GyroY.ToString("0.00");
        txt_msg.text += "    ";
        txt_msg.text += "GyroZ : " + GyroZ.ToString("0.00");
    }

    SerialPort sp = null;
    bool isClose = false;


    void Run()
    {

        Debug.Log("Run");

        string data = "No Data";

        try
        {

            sp = new SerialPort("COM3", 115200, Parity.None, 8, StopBits.One); // El puerto de comunicación es COM5, velocidad en baudios 115200

            sp.Open(); // Abra el puerto de comunicación COM5



            while (isClose == false)
            {
                if (sp.ReadByte() == 0xAA && sp.ReadByte() == 0xAA)
                {
                    List<int> d = new List<int>();


                    while (true)
                    {
                        int dd = sp.ReadByte();
                        int dd2 = sp.ReadByte();
                        if (dd == 0xAA && dd2 == 0xAA)
                        {
                            break;
                        }
                        d.Add(dd);
                        d.Add(dd2);
                    }

                    if (d.Count != 14)
                    {
                        continue;
                    }

                    unchecked
                    {
                        // acelerómetro
                        short accX = (short)(d[0] << 8 | d[1]);
                        short accY = (short)(d[2] << 8 | d[3]);
                        short accZ = (short)(d[4] << 8 | d[5]);
                        AccX = accX / 32767.0f;
                        AccY = accY / 32767.0f;
                        AccZ = accZ / 32767.0f;

                        // temperatura
                        short temp = (short)(d[6] << 8 | d[7]);
                        Temp = 36.53f + temp / 340.0f;

                        // giroscopio
                        short gyroX = (short)(d[8] << 8 | d[9]);
                        short gyroY = (short)(d[10] << 8 | d[11]);
                        short gyroZ = (short)(d[12] << 8 | d[13]);
                        GyroX = gyroX / 32767.0f;
                        GyroY = gyroY / 32767.0f;
                        GyroZ = gyroZ / 32767.0f;
                    }

                    secCount++;
                    for(int i = 0; i<16; i++)
                    {
                        Debug.Log(d[i]);
                    }
                }
            }


        }
        catch (Exception ex)
        {
            Debug.Log(ex);
            Debug.Log(data);
            isClose = true;
            sp.Close(); // Cerrar el puerto de comunicación COM5
        }
    }

    private void OnDestroy()
    {
        isClose = true;
        sp.Close();
        print("Close");
    }
}