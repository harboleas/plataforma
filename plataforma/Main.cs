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
    int x_sen;
    int y_sen;
    int z_sen;
    string dato;
    Vector3 grav;
    Label label;
    StaticBody plat;
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
        plat = GetNode<StaticBody>("Plataforma");
        punto = GetNode<MeshInstance>("Punto");

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        port.Write(" "); // Para sincronizar
        if (port.BytesToRead > 0)
        {
            dato = port.ReadLine();
            x_sen = int.Parse(dato.Split(",")[0]);
            y_sen = int.Parse(dato.Split(",")[1]);
            z_sen = int.Parse(dato.Split(",")[2]);
            grav = new Vector3(-x_sen, -z_sen, y_sen);
            var n = (new Vector3(grav.x, -grav.y, grav.z)).Normalized();
            var nz = (new Vector3(n.x, n.y, 0)).Normalized();
            var nx = (new Vector3(0, n.y, n.z)).Normalized();

            var ang_x = Vector3.Up.SignedAngleTo(nx, Vector3.Right);
            var ang_z = Vector3.Up.SignedAngleTo(nz, Vector3.Back);

            plat.Rotation = (new Vector3(ang_x, 0, ang_z));

            label.Text = grav.ToString();
            punto.Translation = plat.Transform.origin + grav.Normalized() * 2;

        }
    }
        
    public void _on_exiting()
    {
        port.Dispose();
    }
}
