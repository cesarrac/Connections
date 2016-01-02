using UnityEngine;
using System.Collections;

public enum ConnectionMode { INPUT, OUTPUT }
public enum ConnectionType { Health, Education, Technology, Defense, Spirituality, Economy, Entertainment}
public enum Connector { Pole, City }

public class Connection {

    // Is this connected to a POLE or a CITY? 

    public int end_x_position;

    public ConnectionType connectionType;

    public ConnectionMode connectionMode;

    public Connector connector;
    
    public Pole originPole;

    public int dataPacketSize;

    public float dataTransferSpeed; // < ----------- in seconds!

    public City sourceCity, outputCity;

    public Connection(int end_x, ConnectionType con_type, ConnectionMode con_mode, Connector _connector, Pole origin, int packetSize, float transferSpeed)
    {
        end_x_position = end_x;
        connectionType = con_type;
        originPole = origin;
        connectionMode = con_mode;
        connector = _connector;
        dataPacketSize = packetSize;
        dataTransferSpeed = transferSpeed;
    }

	
}
