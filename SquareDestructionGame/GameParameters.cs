public static class GameParameters
{
    public const string GAME_NAME = "SQUARE DESTRUCTION GAME";
    public const int MAX_GAME_SQUARES = 4;
    public const int AIRPLANE_SPEED = 2;
    public const int POWER_INCREASING_VALUE = 10;
    public const int SCORE_INCREASING_VALUE = 300;
    public const int SQUARE_WIDTH = 5;
    public const int SQUARE_HEIGHT = 3;
    public const double SQUARE_SPEED = 0.01;
    public const double PARTICULE_SPEED = 0.1;
    public static (int min, int max) SQUARE_CREATION_TIME_DOMAIN = (2, 5);
    public static (int width, int height) MIN_WINDOW_SIZE = (70, 20);
}
