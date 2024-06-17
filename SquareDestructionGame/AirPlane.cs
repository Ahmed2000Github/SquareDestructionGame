using System.Security.Cryptography;

public class AirPlane : Drawer
{
    private int OldXposition ;
    public AirPlane(int x, int y)
    {
        X = x;
        OldXposition = x;
        Y = y;
    }
    public ConsoleColor ForegroundColor { get; set; } = ConsoleColor.Green;


    public override void Draw()
    {

        Console.ForegroundColor = ForegroundColor;
        Write("    @@@    ");
        Write("  @@@@@@@  ",1);
        Write("  @@   @@  ",2);
        ForegroundColor = ConsoleColor.Green;
        Console.ForegroundColor = ConsoleColor.Gray;

    }
    private void Write(string part,int line = 0)
    {
        if (OldXposition !=X)
        {
            Console.SetCursorPosition(0, Y + line);
            var iterator = 1;
            while (iterator < Console.WindowWidth)
            {
                Console.Write(" ");
                iterator++;
            }
            OldXposition = X;
        }
        base.Draw();
        SetCursor(line);
        Console.Write(part);
    }
}
