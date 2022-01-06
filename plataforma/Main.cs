using Godot;
using System;
using System.IO.Ports;


public class Main : Spatial
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    SerialPort port;
    int x;
    int y;
    int z;
    string dato;
    Label label;
    MeshInstance plat;
    MeshInstance punto;
    public override void _Ready()
    {
        try
        {
            port = new SerialPort("/dev/ttyUSB0", 115200);
            // Abro puerto y reseteo el arduino
            port.Open();
            port.DtrEnable = false;
            System.Threading.Thread.Sleep(1000);
            port.DiscardInBuffer();
            port.DtrEnable = true;
        }
        catch 
        {
            GD.Print("No se encuentra el Arduino");
        }

        label = GetNode<Label>("UI/Label");
        plat = GetNode<MeshInstance>("Plataforma");
        punto = GetNode<MeshInstance>("Punto");

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        port.Write(" "); // Para sincronizar
        if (port.BytesToRead > 0)
        {
            dato = port.ReadLine();
            label.Text = dato;
            x = int.Parse(dato.Split(",")[0]);
            y = int.Parse(dato.Split(",")[1]);
            z = int.Parse(dato.Split(",")[2]);
            plat.LookAt((new Vector3(x, -z, -y)).Normalized(), Vector3.Up);
            punto.Translation = (new Vector3(-x, -z, y)).Normalized() * 2;
        }
    }
        
    public override void _ExitTree()
    {
        base._ExitTree();
        port.Dispose();
    }
}
