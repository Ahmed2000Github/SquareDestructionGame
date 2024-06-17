
public class Particule : Drawer
{
    public Particule(int x, int y)
    {
        X = x;
        Y = y;
    }
    private const double Speed = GameParameters.PARTICULE_SPEED;
    public static int WindowHeight = 26;
    public double SpeedAccumulator { get; set; } = 0;
    public bool IsDestructed { get; set; } = false;
    public event Action<Particule>? OnDamageRecieved;

    public override void Draw()
    {
        base.Draw();
        OnDamageRecieved.Invoke(this);
        if (Y == 0)
        {
            IsDestructed = true;
            ClearAllPrecedences();
        }
        if (!IsDestructed)
        {
            Console.WriteLine("$");
        }
        ClearAllPrecedences();

        if (Y >= 0)
        {
            SpeedAccumulator += Speed;
            Y -= (int)SpeedAccumulator;
            SpeedAccumulator = SpeedAccumulator < 1 ? SpeedAccumulator : 0;
        }
    }
    private void ClearAllPrecedences()
    {
        var counter = Y + 1;
        var height = WindowHeight;
        while (counter < height)
        {
            SetCursor(height - counter);
            Console.WriteLine(" ");
            counter++;
        }
    }

}
