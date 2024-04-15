using System;
using System.Collections.Generic;
using System.Linq;

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
        static Pixel berry;

        static int screenWidth;
        static int screenHeight;

        static Random randomNumber;

        static int score;
        static int gameOver;

        static string movementDirection;

        static List<Pixel> snakeBodyPositionsList = new List<Pixel>();

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

            SetGameStatusAndScore();

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

            berry = new Pixel(0, 0, ConsoleColor.Cyan);

            randomNumber = new Random();
        }
        public static void SetGameStatusAndScore()
        {
            score = 0;
            gameOver = 0;
        }
        public static void CreateSnake()
        {
            snakeHead = new Pixel(screenWidth / 2, screenHeight / 2, ConsoleColor.Red);
            movementDirection = "RIGHT";
            snakeBodyPositionsList = new List<Pixel>();
            buttonPressed = false;
        }
        #endregion
        public static void GenerateBerry()
        {
            berry.PositionX = randomNumber.Next(1, screenWidth - 2);
            berry.PositionY = randomNumber.Next(1, screenHeight - 2);


            //BUG - BERRY COULD SPAWN INSIDE SNAKE'S BODY
            //FIXED - WE CHECK IF COORDINATES ARE DIFFERENT THAN ANY OF THE BODY COORDINATES, IF NOT, GENERATE NEW COORDINATES
            while (PixelListContainsPixel(snakeBodyPositionsList, berry))
            {
                berry.PositionX = randomNumber.Next(1, screenWidth - 2);
                berry.PositionY = randomNumber.Next(1, screenHeight - 2);
            }
        }
        public static bool PixelListContainsPixel(List<Pixel> pixelList, Pixel pixel)
        {
            foreach (Pixel listPixel in pixelList)
            {
                if (listPixel.PositionX == pixel.PositionX && listPixel.PositionY == pixel.PositionY)
                {
                    return true;
                }
            }
            return false;
        }


        public static bool PixelOverlapsPixel(Pixel pixel1, Pixel pixel2)
        {
            if (pixel1.PositionX == pixel2.PositionX && pixel1.PositionY == pixel2.PositionY)
            {
                return true;
            }
            return false;
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
            if (snakeHead.PositionX == screenWidth - 1 || snakeHead.PositionX == 0 || snakeHead.PositionY == screenHeight - 1 || snakeHead.PositionY == 0)
            {
                gameOver = 1;
            }
        }
        public static void CheckSnakeBerryCollision()
        {
            if (PixelOverlapsPixel(snakeHead, berry))
            {
                score++;
                GenerateBerry();
            }
        }
        public static void CheckSnakeBodyCollision()
        {
            if (PixelListContainsPixel(snakeBodyPositionsList, snakeHead))
            {
                gameOver = 1;
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
                    if (pressedKey.Key.Equals(ConsoleKey.UpArrow) && movementDirection != "DOWN" && !buttonPressed)
                    {
                        movementDirection = "UP";
                        buttonPressed = true;
                    }
                    if (pressedKey.Key.Equals(ConsoleKey.DownArrow) && movementDirection != "UP" && !buttonPressed)
                    {
                        movementDirection = "DOWN";
                        buttonPressed = true;
                    }
                    if (pressedKey.Key.Equals(ConsoleKey.LeftArrow) && movementDirection != "RIGHT" && !buttonPressed)
                    {
                        movementDirection = "LEFT";
                        buttonPressed = true;
                    }
                    if (pressedKey.Key.Equals(ConsoleKey.RightArrow) && movementDirection != "LEFT" && !buttonPressed)
                    {
                        movementDirection = "RIGHT";
                        buttonPressed = true;
                    }
                }
            }
        }
        public static void AddNewSnakeCoordinate()
        {
            snakeBodyPositionsList.Add(new Pixel(snakeHead.PositionX, snakeHead.PositionY, ConsoleColor.Green));
        }
        public static void DetermineDirection()
        {
            switch (movementDirection)
            {
                case "UP":
                    snakeHead.PositionY--;
                    break;
                case "DOWN":
                    snakeHead.PositionY++;
                    break;
                case "LEFT":
                    snakeHead.PositionX--;
                    break;
                case "RIGHT":
                    snakeHead.PositionX++;
                    break;
            }
        }
        public static void RemoveOldSnakeCoordinate()
        {

            if (snakeBodyPositionsList.Count() > score + 5)
            {
                snakeBodyPositionsList.RemoveAt(0);
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

            for (int i = 0; i < snakeBodyPositionsList.Count(); i++)
            {
                Console.ForegroundColor = snakeBodyPositionsList[i].Color;
                Console.SetCursorPosition(snakeBodyPositionsList[i].PositionX, snakeBodyPositionsList[i].PositionY);
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
                Console.SetCursorPosition(i, screenHeight - 1);
                Console.Write("■");
            }

            for (int i = 0; i < screenHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("■");
                Console.SetCursorPosition(screenWidth - 1, i);
                Console.Write("■");
            }

        }

        public static void DrawSnakeHead()
        {
            Console.SetCursorPosition(snakeHead.PositionX, snakeHead.PositionY);
            Console.ForegroundColor = snakeHead.Color;
            Console.Write("■");
        }

        public static void DrawBerry()
        {
            Console.SetCursorPosition(berry.PositionX, berry.PositionY);
            Console.ForegroundColor = berry.Color;
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
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public ConsoleColor Color { get; set; }

        public Pixel(int positionX, int positionY, ConsoleColor color)
        {
            PositionX = positionX;
            PositionY = positionY;
            Color = color;
        }
    }

}


