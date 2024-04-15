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
        static Random randomnummer;
        static int score;
        static int gameOver;
        static string movement;

        static List<int> snakeBodyPositionXList = new List<int>();
        static List<int> snakeBodyPositionYList = new List<int>();

        static int berryPositionX;
        static int berryPositionY;

        static bool buttonPressed;

        static DateTime startTime;
        static DateTime nextTime;

        static void Main(string[] args)
        {      
            Initialize();
 
            //GAME LOOP
            while (true)
            {
                Console.Clear();
                UpdateState();
                if (gameOver == 1)
                {
                    break;
                }
                Render();          
                CheckUserInput();                   
            }

            Console.SetCursorPosition(screenWidth / 5, screenHeight / 2);
            Console.WriteLine("Game over, Score: " + score);
            Console.SetCursorPosition(screenWidth / 5, screenHeight / 2 + 1);
        }
        
        public static void Initialize() 
        {

            Console.WindowHeight = 16;
            Console.WindowWidth = 32;
            snakeHead = new Pixel();
            screenWidth = Console.WindowWidth;
            screenHeight = Console.WindowHeight;

            randomnummer = new Random();
            score = 0;
            gameOver = 0;

            snakeHead.xPosition = screenWidth / 2;
            snakeHead.yPosition = screenHeight / 2;

            snakeHead.color = ConsoleColor.Red;

            movement = "RIGHT";

            snakeBodyPositionXList = new List<int>();
            snakeBodyPositionYList = new List<int>();

            berryPositionX = randomnummer.Next(1, screenWidth - 2);
            berryPositionY = randomnummer.Next(1, screenHeight - 2);

            buttonPressed = false;

        }
        public static void UpdateState()
        {
            
            if (snakeHead.xPosition == screenWidth - 1 || snakeHead.xPosition == 0 || snakeHead.yPosition == screenHeight - 1 || snakeHead.yPosition == 0)
            {
                gameOver = 1;
            }
            
            if (berryPositionX == snakeHead.xPosition && berryPositionY == snakeHead.yPosition)
            {
                score++;
                berryPositionX = randomnummer.Next(1, screenWidth - 2);
                berryPositionY = randomnummer.Next(1, screenHeight - 2);
            }

            for (int i = 0; i < snakeBodyPositionXList.Count(); i++)
            {
                if (snakeBodyPositionXList[i] == snakeHead.xPosition && snakeBodyPositionYList[i] == snakeHead.yPosition)
                {
                    gameOver = 1;
                }
            }
        }
        public static void CheckUserInput()
        {
            startTime = DateTime.Now;
            buttonPressed = false;
            while (true)
            {
                nextTime = DateTime.Now;
                if (nextTime.Subtract(startTime).TotalMilliseconds > 500) { break; }
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo toets = Console.ReadKey(true);
                    //Console.WriteLine(toets.Key.ToString());
                    if (toets.Key.Equals(ConsoleKey.UpArrow) && movement != "DOWN" && !buttonPressed)
                    {
                        movement = "UP";
                        buttonPressed = true;
                    }
                    if (toets.Key.Equals(ConsoleKey.DownArrow) && movement != "UP" && !buttonPressed)
                    {
                        movement = "DOWN";
                        buttonPressed = true;
                    }
                    if (toets.Key.Equals(ConsoleKey.LeftArrow) && movement != "RIGHT" && !buttonPressed)
                    {
                        movement = "LEFT";
                        buttonPressed = true;
                    }
                    if (toets.Key.Equals(ConsoleKey.RightArrow) && movement != "LEFT" && !buttonPressed)
                    {
                        movement = "RIGHT";
                        buttonPressed = true;
                    }
                }
            }
            snakeBodyPositionXList.Add(snakeHead.xPosition);
            snakeBodyPositionYList.Add(snakeHead.yPosition);
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


            if (snakeBodyPositionXList.Count() > score+5)
            {
                snakeBodyPositionXList.RemoveAt(0);
                snakeBodyPositionYList.RemoveAt(0);
            }
        }
        public static void Render()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            for (int i = 0; i < snakeBodyPositionXList.Count(); i++)
            {
                Console.SetCursorPosition(snakeBodyPositionXList[i], snakeBodyPositionYList[i]);
                Console.Write("■");
            }

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

            Console.SetCursorPosition(snakeHead.xPosition, snakeHead.yPosition);
            Console.ForegroundColor = snakeHead.color;
            Console.Write("■");

            Console.SetCursorPosition(berryPositionX, berryPositionY);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("■");        
        }
    }
    class Pixel
    {
        public int xPosition { get; set; }
        public int yPosition { get; set; }
        public ConsoleColor color { get; set; }
    }

}
    

