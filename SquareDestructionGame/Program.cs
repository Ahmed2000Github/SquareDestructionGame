

// initial game parameters
bool gameOver = false;
bool isWindowsSizeSupported = true;
int windowWidth = Console.WindowWidth - 1;
int windowHeight = Console.WindowHeight - 4;
List<int> occupedPositions = new();
List<Particule> particules = new List<Particule>();
List<Square> squares = new List<Square>();
AirPlane airPlane = new AirPlane(Console.WindowWidth / 2, windowHeight);
StatusBar statusBar = new StatusBar();

Console.Title = GameParameters.GAME_NAME;

var controlListenerThread = new Thread(controlListener);
controlListenerThread.Start();
var randomSquareCreatorThread = new Thread(randomSquareCreator);
randomSquareCreatorThread.Start();


// main program
while (!gameOver)
{
    try
    {
        Console.CursorVisible = false;
        if (isWindowsSizeSupported)
        {
            statusBar.Draw();
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (var square in squares)
            {
                square.Draw();
            }
            Console.ForegroundColor = ConsoleColor.Magenta;

            foreach (var particule in particules)
            {
                particule.Draw();
            }


            particules = particules.Where(p => !p.IsDestructed).ToList();
            Console.ForegroundColor = ConsoleColor.Green;
            airPlane.Draw();
           
        }
        else
        {
            PrintLabel("Window Size Not Supported");
        }
        if (Console.WindowHeight - 4 != windowHeight || Console.WindowWidth - 1 != windowWidth)
        {
            Console.Clear();
            Console.CursorVisible = false;
            windowWidth = Console.WindowWidth - 1;
            windowHeight = Console.WindowHeight - 4;
            Particule.WindowHeight = windowHeight;
            NotifyWindowChanged();
            isWindowsSizeSupported = GameParameters.MIN_WINDOW_SIZE.width < windowWidth && GameParameters.MIN_WINDOW_SIZE.height < windowHeight;
        }

    }
    catch { }

}
Console.Clear();
PrintLabel("Game Finished");
PrintLabel("(press any key to exit)");
controlListenerThread?.Interrupt();
randomSquareCreatorThread?.Interrupt();


// Utils functions

//listen to the keyborad input keys
void controlListener()
{
    while (!gameOver)
    {
        var key = Console.ReadKey(true).Key;
        switch (key)
        {
            case ConsoleKey.Escape:
                gameOver = true;
                Console.Clear();
                break;
            case ConsoleKey.X:
                var particule = new Particule(airPlane.X + 5, airPlane.Y-1);
                particule.OnDamageRecieved += OnDamageRecieved;
                particules.Add(particule);
                break;
            case ConsoleKey.LeftArrow:
                var newXPositionLeft = airPlane.X - GameParameters.AIRPLANE_SPEED;
                if (newXPositionLeft >= 0) airPlane.X = newXPositionLeft;
                break;
            case ConsoleKey.RightArrow:
                var newXPositionRight = airPlane.X + GameParameters.AIRPLANE_SPEED;
                if (newXPositionRight < windowWidth - 9) airPlane.X = newXPositionRight;
                break;
        }
    }

}

// generate random squares during the game
void randomSquareCreator()
{
    while (!gameOver)
    {
        if (squares.Count() < GameParameters.MAX_GAME_SQUARES)
    {
        var time = new Random().Next(GameParameters.SQUARE_CREATION_TIME_DOMAIN.min, GameParameters.SQUARE_CREATION_TIME_DOMAIN.max);
        var getXPosition = () => new Random().Next(2, windowWidth - GameParameters.SQUARE_WIDTH);
        var xPosition = getXPosition();
            var isValidPosition = checkPosition(xPosition);
            while (!isValidPosition)
            {
                xPosition = getXPosition();
                isValidPosition = checkPosition(xPosition);
            }
            Thread.Sleep(TimeSpan.FromSeconds(time));
        var square = new Square(xPosition, 1)
        {
            MaxYPosition = airPlane.Y 
        };
        square.OnEdgeCollision += OnEdgeCollision;
            occupedPositions.Add(square.X);
        squares.Add(square);
    }
    }
}

//validate if the position is valid for square creation
bool checkPosition(int xPosition)
{
    foreach (var position in occupedPositions)
    {
        if (xPosition >= position && xPosition <= position + 4)
        {
            return false;
        }
    }
    return true;
}

// handling game case when square tach the bottom edge
void OnEdgeCollision()
{
    airPlane.ForegroundColor = ConsoleColor.Red;
    statusBar.Power -= GameParameters.POWER_INCREASING_VALUE;
    if (statusBar.Power <= 0)
    {
        new Thread(
            () =>
            {
                Thread.Sleep(100);
                gameOver = true;
            }
            ).Start();
    }
    RemoveDeathSquares(s => s.Y == s.MaxYPosition);
}

//handing squares finished
void RemoveDeathSquares(Func<Square,bool> condition)
{
    foreach (var square in squares.Where(condition))
    {
        square.Destruct();
    }
    squares = squares.Where(s => !condition(s)).ToList();
}

//handle case when particule tach square
void OnDamageRecieved(Particule particule)
{
    bool isThereAnyFinishedSquare = false;
    foreach (var square in squares)
    {
        var numberOfLines = (int)(square.Damage / 5);
        var isParticuleInXDomain = square.X <= particule.X && square.X + 4 >= particule.X;
        var isParticuleInYDomain = square.Y <= particule.Y && square.Y + numberOfLines >= particule.Y;

        if (isParticuleInXDomain && isParticuleInYDomain)
        {
            particule.IsDestructed = true;
            square.Damage++;
            isThereAnyFinishedSquare = square.Damage == Square.SquarePartsCounter;
            if (isThereAnyFinishedSquare)
            {
                statusBar.Score += GameParameters.SCORE_INCREASING_VALUE;
            }
        }
    }
    if (isThereAnyFinishedSquare)
    {
        RemoveDeathSquares(s => s.Damage == Square.SquarePartsCounter);
    }

}

// print label
void PrintLabel(string label)
{
    Console.Clear();
    Console.SetCursorPosition(windowWidth/2 - label.Length/2,windowHeight/2);
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(label);
    Thread.Sleep(1000);
}

// set element place when window changes
void NotifyWindowChanged()
{
    airPlane.Y = windowHeight;
    squares = new();
    
}