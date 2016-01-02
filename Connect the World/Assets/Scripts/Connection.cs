using UnityEngine;
using System.Collections;

public enum ConnectionMode { INPUT, OUTPUT }
public enum ConnectionType { Health, Education, Technology, Defense, Spirituality, Economy, Entertainment}

public class Connection {

    // Is this connected to a POLE or a CITY? 

    public int end_x_position;

    public ConnectionType connectionType;

    public ConnectionMode connectionMode;
    
    public Pole originPole; 

    public Connection(int end_x, ConnectionType con_type, ConnectionMode con_mode, Pole origin)
    {
        end_x_position = end_x;
        connectionType = con_type;
        originPole = origin;
        connectionMode = con_mode;
    }

	
}
