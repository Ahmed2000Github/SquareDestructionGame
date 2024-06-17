public class StatusBar : Drawer
{
    public StatusBar()
    {
        X = 0;
        Y = 0;
    }
    public double Power { get; set; } = 100;
    public int Score { get; set; } = 0;

    public override void Draw()
    {

        base.Draw();
        DrawPowerStatus();
        DrawScoreStatus();
    }
    private void DrawPowerStatus()
    {
        Console.BackgroundColor = ConsoleColor.Green;
        for (int i = 0; i < (int)Power / 10; i++)
        {
            Console.Write(" ");
        }
        Console.BackgroundColor = ConsoleColor.Red;
        for (int i = 0; i < 10 - (int)Power / 10; i++)
        {
            Console.Write(" ");
        }
        Console.BackgroundColor = ConsoleColor.Black;
        Console.Write($"{Power}% ");
    }
    private void DrawScoreStatus()
    {
        var score = $"Score: {Score}";
        var xPosition = Console.WindowWidth - (score.Length+5);
        Console.SetCursorPosition(xPosition, 0);
        Console.Write(score);
    }


}
