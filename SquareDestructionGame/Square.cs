public class Square : Drawer
{
    public Square(int x, int y)
    {
        X = x;
        Y = y;
    }
    private const double Speed = GameParameters.SQUARE_SPEED;
    public event Action? OnEdgeCollision;
    public double SpeedAccumulator { get; set; } = 1;
    public int Damage { get; set; } = 0;
    public const int SquarePartsCounter = GameParameters.SQUARE_WIDTH * GameParameters.SQUARE_HEIGHT;
    public int MaxYPosition { get; set; } = 0;

    public override void Draw()
    {
       
     
        var length = SquarePartsCounter - Damage;
        base.Draw();
        int counter = 1;

        while (counter <= length)
        {
            PrintCharacter('#', counter);
            counter++;
        }
        counter = length + 1;
        while (counter <= SquarePartsCounter)
        {
            PrintCharacter(' ', counter);
            counter++;
        }
        
        if (Y>1)
        {
            Y -=1;
            RemovePreviousLine();
            Y += 1;
        }
        if (MaxYPosition > Y)
        {
            SpeedAccumulator += Speed; 
        Y = (int)SpeedAccumulator;
        }else if (MaxYPosition == Y)
        {
            OnEdgeCollision?.Invoke();
        }
    }

    public void Destruct()
    {
        RemovePrevious();
    }
    private void RemovePrevious()
    {
        base.Draw();
        Console.Write("  ");
        int counter = 1;

        while (counter <= SquarePartsCounter)
        {
            PrintCharacter(' ', counter);
            counter++;
        }
    }
    private void RemovePreviousLine()
    {
        base.Draw();
        int counter = 1;

        while (counter <= GameParameters.SQUARE_WIDTH)
        {
            PrintCharacter(' ', counter);
            counter++;
        }
    }
    private void PrintCharacter(char character, int positioner)
    {
        Console.Write(character);
        if (positioner % GameParameters.SQUARE_WIDTH == 0)
        {
            SetCursor(positioner / GameParameters.SQUARE_WIDTH);
        }
    }
}