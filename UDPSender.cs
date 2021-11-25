using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPSender : MonoBehaviour
{

  [Header("UDP settings")]
  [Tooltip("Localhost")]
  [SerializeField] public string IP = "127.0.0.1";
  [Tooltip("Localport")]
  [SerializeField] public int portLocal = 8000;
  [Tooltip("Remote port")]
  [SerializeField] public int portRemote = 8001;

  [Header("GameObjects to track")]
  [SerializeField] public GameObject[] trackedObjects;

  // Floats to be sent
  private double[] SendFloat = new double[13];
  private string UDP_message;
  private bool keepSending = true;

  // Create necessary UdpClient objects
  UdpClient client;
  IPEndPoint remoteEndPoint;

  // Receiving Thread
  Thread receiveThread;

  // info strings
  private string lastReceivedUDPPacket = "";
  private string allReceivedUDPPackets = "";
  // clear this from time to time!

  public void Start ()
  {
    // Initialize UDP
    init();
  }

  public void Update ()
  {
    if (Input.GetKeyDown(KeyCode.Q)) { keepSending = false; } // If the 'Q' key on the keyboard is pressed, the string 'Quit' will be sent over UDP.

    if (keepSending)
    {
      // Obviously, not necessary to put this in a double[] first and then in a string, but this keeps the code more readable.
      // Could also be done without and then using a for loop containing the size of the trackedObject list. This would, however, slow down the program a little.
      
      // Set all the correct floats to the SendFloat variable
      SendFloat[0] = trackedObjects[0].transform.position.x;
      SendFloat[1] = trackedObjects[0].transform.position.y;
      SendFloat[2] = trackedObjects[0].transform.position.z;
      SendFloat[3] = trackedObjects[0].transform.rotation.eulerAngles.x;
      SendFloat[4] = trackedObjects[0].transform.rotation.eulerAngles.y;
      SendFloat[5] = trackedObjects[0].transform.rotation.eulerAngles.z;

      SendFloat[6] = trackedObjects[1].transform.position.x;
      SendFloat[7] = trackedObjects[1].transform.position.y;
      SendFloat[8] = trackedObjects[1].transform.position.z;
      SendFloat[9] = trackedObjects[1].transform.rotation.eulerAngles.x;
      SendFloat[10] = trackedObjects[1].transform.rotation.eulerAngles.y;
      SendFloat[11] = trackedObjects[1].transform.rotation.eulerAngles.z;

      SendFloat[12] = Time.time; // Add the timestamp

      // Combine into a string
      UDP_message = SendFloat[0] + ","
                  + SendFloat[1] + ","
                  + SendFloat[2] + ","
                  + SendFloat[3] + ","
                  + SendFloat[4] + ","
                  + SendFloat[5] + ","
                  + SendFloat[6] + ","
                  + SendFloat[7] + ","
                  + SendFloat[8] + ","
                  + SendFloat[9] + ","
                  + SendFloat[10] + ","
                  + SendFloat[11] + ","
                  + SendFloat[12];

      // Send the string over UDP
      sendData(UDP_message);
    }
    else
    {
      sendData("Quit"); // Sending 'Quit' over UDP, so Matlab knows to stop saving the data
    }
  }

  // Initialization code
  private void init()
  {
    // Initialize (seen in comments window)
    print("UDP Object init()");

    // Create remote endpoint (to Matlab) 
    remoteEndPoint = new IPEndPoint (IPAddress.Parse (IP), portRemote);

    // Create local client
    client = new UdpClient (portLocal);

    // local endpoint define (where messages are received)
    // Create a new thread for reception of incoming messages
    receiveThread = new Thread (
      new ThreadStart (ReceiveData));
    receiveThread.IsBackground = true;
    receiveThread.Start ();
  }

  // Receive data, update packets received
  private  void ReceiveData()
  {
    while (true) {

      try {
        IPEndPoint anyIP = new IPEndPoint (IPAddress.Any, 0);
        byte[] data = client.Receive (ref anyIP);
        string text = Encoding.UTF8.GetString (data);
        print (">> " + text);
        lastReceivedUDPPacket = text;
        allReceivedUDPPackets = allReceivedUDPPackets + text;

      } catch (Exception err) {
        print (err.ToString ());
      }
    }
  }

  // Send data
  private void sendData (string UDP_senddata)
  {
    try {
      byte[] data = Encoding.UTF8.GetBytes (UDP_senddata);
      client.Send (data, data.Length, remoteEndPoint);
      
    } catch (Exception err) {
      print (err.ToString ());
    }
  }

  // getLatestUDPPacket, clears all previous packets
  public string getLatestUDPPacket ()
  {
    allReceivedUDPPackets = "";
    return lastReceivedUDPPacket;
  }

  //Prevent crashes - close clients and threads properly!
  void OnDisable ()
  { 
    if (receiveThread != null)
      receiveThread.Abort (); 

    client.Close ();
  }

}