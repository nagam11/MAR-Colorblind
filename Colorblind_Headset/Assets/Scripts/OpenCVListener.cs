using UnityEngine;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class OpenCVListener : MonoBehaviour
{
    Thread receiveThread;
    UdpClient client;
    int port;

    string lastReceivedUDPPacket = "";
    string allReceivedUDPPackets = "";

    // Use this for initialization
    void Start()
    {
        InitUDP();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void InitUDP()
    {
        print("UPDSend.init()");
        port = 5065;

        print("Sending to 127.0.0.1 : " + port);

        receiveThread = new Thread(new ThreadStart(ReceiveData))
        {
            IsBackground = true
        };
        receiveThread.Start();

    }

    private void ReceiveData()
    {
        client = new UdpClient(port);
        while (true)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse("0.0.0.0"), port);
                byte[] data = client.Receive(ref anyIP);

                string text = Encoding.UTF8.GetString(data);
                print(">> " + text);
                lastReceivedUDPPacket = text;
                allReceivedUDPPackets = allReceivedUDPPackets + text;   

            }
            catch (Exception e)
            {
                print(e.ToString());
            }
        }
    }

    public string getLatestUDPPacket()
    {
        allReceivedUDPPackets = "";
        return lastReceivedUDPPacket;
    }

    void OnApplicationQuit()
    {
        if (receiveThread != null)
        {
            receiveThread.Abort();
        }
    }
}
