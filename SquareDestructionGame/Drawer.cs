public class Drawer
{
    public int X { get; set; }
    public int Y { get; set; }
    
    public void SetCursor(int yOffset = 0)
    {
        Console.SetCursorPosition(X, Y + yOffset);
    }
    public virtual void Draw()
    {
            SetCursor();
    }
}
