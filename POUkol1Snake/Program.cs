using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

/*
 Vezměte tento lagacy kód známé hry snake. 

Pokud máte problém kód pochopit, pročtěte si diskuzi pod příspěvkem. 

Refaktorujte kód pomocí pravidel čistého kódu, jak nejlépe umíte. K orientaci v těch pravidlech můžete využít knihu Clean Code. 

Dejte důraz na smysluplná jména, malé metody, malé konzistentní třídy, nemíchejte do hromady kompetence na různých úrovních abstrakce, atd... 

Ukažte na kódu, jak pěkně dokážete kód refaktorovat a vyčistit, ale nepřekračujte hranice common sence a nezacházejte do absurdit. 

Velký důraz položte na decoupling logiky kódu od jeho GUI. 

Výsledný kód odevzdejte jako odkaz na GitHub, nebo nějakou ekvivalentní službu. Máte potřebu ke kódu něco sdělit, učiňte tak v readme.md v kořeni Vašeho projektu. Nezapomeňte včas přidat správný ignore file. 
 */

namespace Snake
{

    class Program
    {
        static Pixel snakeHead;
        static int screenWidth;
        static int screenHeight;
        static Random randomNumber;
        static int score;
        static int gameOver;
        static string movement;
        static List<int> snakeBodyPositionXList = new List<int>();
        static List<int> snakeBodyPositionYList = new List<int>();
        static int berryPositionX;
        static int berryPositionY;
        static bool buttonPressed;
        static DateTime startTime;
        static DateTime passedTime;

        static void Main(string[] args)
        {
            Initialize();
            #region GameLoop
            while (true)
            {               
                UpdateState();
                if (gameOver == 1)
                {
                    break;
                }
                Render();
                ProcessUserInput();
            }
            #endregion
            ShowGameOverScreen();
        }
        public static void Initialize()
        {
            PreparePlayground();

            InitializeGameStatusAndScore();

            CreateSnake();

            GenerateBerry();
        }
        #region Initialize functions
        public static void PreparePlayground()
        {
            Console.WindowHeight = 16;
            Console.WindowWidth = 32;

            screenWidth = Console.WindowWidth;
            screenHeight = Console.WindowHeight;

            randomNumber = new Random();
        }
        public static void InitializeGameStatusAndScore()
        {
            score = 0;
            gameOver = 0;
        }
        public static void CreateSnake()
        {
            snakeHead = new Pixel();
            snakeHead.xPosition = screenWidth / 2;
            snakeHead.yPosition = screenHeight / 2;

            snakeHead.color = ConsoleColor.Red;

            movement = "RIGHT";

            snakeBodyPositionXList = new List<int>();
            snakeBodyPositionYList = new List<int>();
            buttonPressed = false;
        }
        #endregion
        public static void GenerateBerry()
        {
            berryPositionX = randomNumber.Next(1, screenWidth - 2);
            berryPositionY = randomNumber.Next(1, screenHeight - 2);

            
            //BUG - BERRY COULD SPAWN INSIDE SNAKE'S BODY
            //FIXED - WE CHECK IF COORDINATES ARE DIFFERENT THAN ANY OF THE BODY COORDINATES, IF NOT, GENERATE NEW COORDINATES
            while (snakeBodyPositionXList.Contains(berryPositionX) && snakeBodyPositionYList.Contains(berryPositionY))
            {
                    berryPositionX = randomNumber.Next(1, screenWidth - 2);
                    berryPositionY = randomNumber.Next(1, screenHeight - 2);
            }
        }      
        

        public static void UpdateState()
        {
            Console.Clear();
            CheckSnakeBorderCollision();
            CheckSnakeBerryCollision();
            CheckSnakeBodyCollision();
        }
        #region UpdateState functions
        public static void CheckSnakeBorderCollision()
        {
            if (snakeHead.xPosition == screenWidth - 1 || snakeHead.xPosition == 0 || snakeHead.yPosition == screenHeight - 1 || snakeHead.yPosition == 0)
            {
                gameOver = 1;
            }            
        }
        public static void CheckSnakeBerryCollision()
        {
            if (berryPositionX == snakeHead.xPosition && berryPositionY == snakeHead.yPosition)
            {
                score++;
                GenerateBerry();
            }
        }
        public static void CheckSnakeBodyCollision()
        {
            for (int i = 0; i < snakeBodyPositionXList.Count(); i++)
            {
                if (snakeBodyPositionXList[i] == snakeHead.xPosition && snakeBodyPositionYList[i] == snakeHead.yPosition)
                {
                    gameOver = 1;
                }
            }
        }
        #endregion
        public static void ProcessUserInput()
        {
            CheckForNewUserInput();
            AddNewSnakeCoordinate();
            DetermineDirection();
            RemoveOldSnakeCoordinate();
        }
        #region Process User Input Functions
        public static void CheckForNewUserInput()
        {
            startTime = DateTime.Now;
            buttonPressed = false;
            while (true)
            {
                passedTime = DateTime.Now;
                if (passedTime.Subtract(startTime).TotalMilliseconds > 500) { break; }
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                    if (pressedKey.Key.Equals(ConsoleKey.UpArrow) && movement != "DOWN" && !buttonPressed)
                    {
                        movement = "UP";
                        buttonPressed = true;
                    }
                    if (pressedKey.Key.Equals(ConsoleKey.DownArrow) && movement != "UP" && !buttonPressed)
                    {
                        movement = "DOWN";
                        buttonPressed = true;
                    }
                    if (pressedKey.Key.Equals(ConsoleKey.LeftArrow) && movement != "RIGHT" && !buttonPressed)
                    {
                        movement = "LEFT";
                        buttonPressed = true;
                    }
                    if (pressedKey.Key.Equals(ConsoleKey.RightArrow) && movement != "LEFT" && !buttonPressed)
                    {
                        movement = "RIGHT";
                        buttonPressed = true;
                    }
                }
            }
        }
        public static void AddNewSnakeCoordinate()
        {
            snakeBodyPositionXList.Add(snakeHead.xPosition);
            snakeBodyPositionYList.Add(snakeHead.yPosition);
        }
        public static void DetermineDirection()
        {
            switch (movement)
            {
                case "UP":
                    snakeHead.yPosition--;
                    break;
                case "DOWN":
                    snakeHead.yPosition++;
                    break;
                case "LEFT":
                    snakeHead.xPosition--;
                    break;
                case "RIGHT":
                    snakeHead.xPosition++;
                    break;
            }
        }
        public static void RemoveOldSnakeCoordinate()
        {

            if (snakeBodyPositionXList.Count() > score + 5)
            {
                snakeBodyPositionXList.RemoveAt(0);
                snakeBodyPositionYList.RemoveAt(0);
            }
        }
        #endregion
        public static void Render()
        {
            DrawSnakeBody();
            DrawPlaygroundBorders();
            DrawSnakeHead();
            DrawBerry();
        }
        #region Render functions
        public static void DrawSnakeBody()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            for (int i = 0; i < snakeBodyPositionXList.Count(); i++)
            {
                Console.SetCursorPosition(snakeBodyPositionXList[i], snakeBodyPositionYList[i]);
                Console.Write("■");
            }
        }
        public static void DrawPlaygroundBorders()
        {
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < screenWidth; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("■");
            }
            for (int i = 0; i < screenWidth; i++)
            {
                Console.SetCursorPosition(i, screenHeight - 1);
                Console.Write("■");
            }
            for (int i = 0; i < screenHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("■");
            }
            for (int i = 0; i < screenHeight; i++)
            {
                Console.SetCursorPosition(screenWidth - 1, i);
                Console.Write("■");
            }
        }

        public static void DrawSnakeHead()
        {
            Console.SetCursorPosition(snakeHead.xPosition, snakeHead.yPosition);
            Console.ForegroundColor = snakeHead.color;
            Console.Write("■");
        }

        public static void DrawBerry()
        {
            Console.SetCursorPosition(berryPositionX, berryPositionY);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("■");
        }
        #endregion
        public static void ShowGameOverScreen()
        {
            Console.SetCursorPosition(screenWidth / 5, screenHeight / 2);
            Console.WriteLine("Game over, Score: " + score);
            Console.SetCursorPosition(screenWidth / 5, screenHeight / 2 + 1);
        }
    }
    class Pixel
    {
        public int xPosition { get; set; }
        public int yPosition { get; set; }
        public ConsoleColor color { get; set; }
    }

}
    

