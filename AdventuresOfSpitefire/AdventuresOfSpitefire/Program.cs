using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

namespace AdventuresOfSpitefire
{
    class Program
    {
        static List<int> wallPos = new List<int>();
        //random keyword
        static Random rnd = new Random();
        //players skin
        static public string skin = "1";
        //key pressed variable
        static public ConsoleKeyInfo ck = new ConsoleKeyInfo();
        //board
        static public string[] board = new string[640];
        //how much space there has been before an x
        static public int spaceCounter = 0;
        //space between X's
        static public int spaceX = 5;
        static void Main(string[] args)
        {
            string border = "--------------------------------------------------------------------------------"; //80 dashes
            //how many milliseconds to wait until the player moves forwards but it does not matter if speed does not have value because of accelerator method
            int speed;
            //player moves when counter reaches score
            int speedCounter = 0;
            //while the game is running
            bool exit = false;
            //the player
            Player user = new Player(4, true);
            //if the second just passed
            bool secondPassed = false;
            //the score
            int score = 0;
            //high score
            int highScore = 0;
            // while the game is running
            while (exit != true)
            {
                speed = 200;
                //how long the game is going
                int seconds = 0;
                //is the game paused
                bool paused = true;
                //if the game was just unpaused
                bool unpaused = false;
                //the score
                score = 0;
                //space between X's
                spaceX = 5;
                //loop to fill the board
                for (int i = 0; i < board.Length; i++)
                {
                    if (i == 4)
                    {
                        board[4] = skin;
                    }
                    else
                    {
                        board[i] = " ";
                    }
                }
                /*
                for (int i = 0; i < board.Length; i++)
                {
                    Console.Write(board[i]);
                }
                Console.ReadLine();
                */
                //the loop will be used to start all of the walls for the game
                for (int i = 0; i < 60; i++)
                {
                    moveChar(0);
                    spaceCounter++;
                }
                //there is no spaceCounter = 0 because it already is after loop
                //spaceX = something
                user.isAlive = true;
                //load menu
                startScreen();
                //while the player hasn't  died
                while (user.isAlive)
                {
                    Console.Clear();
                    if (paused == true)
                    {
                        //do not touch
                        space(false, @"|--\     ^      |     |   /----  |----  |---\");
                        space(false, @" |  |    / \     |     |   |      |      |    \");
                        space(false, @"  |--/   /---\    |     |   \---\  |----  |     |");
                        space(false, @" |     /     \   |     |       |  |      |    /");
                        space(false, @"|    /       \  \_____/  _____/  |____  |___/");
                    }
                    //if the game was just unpaused
                    if (unpaused == true)
                    {
                        //loop countdown
                        for (int wait = 3; wait > 0; wait--)
                        {
                            //print the countdown in the middle
                            space(false, Convert.ToString(wait));
                            //print space
                            for (int printSpace = 0; printSpace < 5; printSpace++)
                            {
                                Console.WriteLine();
                            }
                            //print the default
                            Console.WriteLine("Use the up and down keys to move around or press p to pause");
                            Console.WriteLine("To quit press pause and then press q");
                            Console.WriteLine("Your player is {0}", skin);
                            Console.WriteLine("try getting the highest score by not smashing into the X walls");
                            space(false, $"Score:{score} High Score:{highScore}");
                            //print border
                            Console.Write(border);
                            //print board
                            for (int i = 0; i < board.Length; i++)
                            {
                                Console.Write(board[i]);
                            }
                            Console.Write(border);
                            //wait one second
                            Thread.Sleep(1000);
                            //clear screen
                            Console.Clear();
                        }
                        unpaused = false;
                    }
                    Console.WriteLine("Use the up and down keys to move around or press p to pause");
                    Console.WriteLine("To quit press pause and then press q");
                    Console.WriteLine("Your player is {0}", skin);
                    Console.WriteLine("try getting the highest score by not smashing into the X walls");
                    space(false, $"Score:{score} High Score:{highScore}");
                    //print border
                    Console.Write(border);
                    //print board
                    for (int i = 0; i < board.Length; i++)
                    {
                        Console.Write(board[i]);
                    }
                    Console.Write(border);

                    //loop until input is pressed
                    while (Console.KeyAvailable == false && secondPassed == false && paused == false)
                    {
                        //wait for 1 millisec
                        Thread.Sleep(1);
                        //add to counter
                        speedCounter++;
                        //if counter is as big as the time we want to wait
                        if (speedCounter == speed)
                        {
                            secondPassed = true;
                            speedCounter = 0;
                            moveChar(seconds);
                            //the score gets higher
                            score += scoreAdder(seconds);
                            speed = accelerator(seconds);
                            //a second has passed (more like a turn than a second)
                            seconds++;
                            spaceCounter++;
                        }
                    }
                    //if second has passed we need to get out of the loop and make changes
                    //but not pause the game to wait for input
                    if (secondPassed == false)
                    {
                        //place the input into the variable
                        ck = Console.ReadKey(true);
                    }
                    //if game is paused
                    if (ck.Key == ConsoleKey.P && secondPassed == false)
                    {
                        //if not already paused
                        if (paused == false)
                        {
                            //pause game
                            paused = true;
                        }
                        else
                        {
                            //unpause game
                            paused = false;
                            unpaused = true;
                        }
                    }
                    if (ck.Key == ConsoleKey.Q && secondPassed == false)
                    {
                        if (paused == true)
                        {
                            concentScr();
                        }
                    }
                    if (ck.Key == ConsoleKey.UpArrow && secondPassed == false)
                    {
                        board[user.pos] = " ";
                        user.pos = user.moveUp();
                        board[user.pos] = skin;
                    }
                    if (ck.Key == ConsoleKey.DownArrow && secondPassed == false)
                    {
                        board[user.pos] = " ";
                        user.moveDown();
                        board[user.pos] = skin;
                    }
                    if (wallPos.Contains(user.pos))
                    {
                        Console.Clear();
                        user.isAlive = false;
                        wallPos.Clear();
                        space(false, "you lost");
                        if (score > highScore)
                        {
                            space(false, "you hit a new high score!");
                        }
                        space(false, $"your score was {score}");
                        space(false, "Press enter to continue");
                        highScore = score;
                        //Console.ReadKey(true);
                        Console.ReadLine();
                        Console.Clear();
                    }
                    secondPassed = false;
                }
            }
            Console.WriteLine("Score:{0} High score{1}", score, highScore);
        }
        static public void startScreen()
        {
            //length of window: 70
            //cont is the variable that's used to loop the method
            bool cont = false;
            //at what option is the cursor
            int cursorPos = 0;
            //the choices that the user can make
            string[] choices = { "play", "quit" }; //can add new stuff in future updates
            //loop the method
            while (cont == false) {
                Console.Clear();
                space(false, "The adventures of spitefire");
                Console.WriteLine();
                for (int i = 0; i < choices.Length; i++)
                { 
                    if (cursorPos == i)
                    {
                        space(true, choices[i]);
                    }
                    if (cursorPos != i)
                    {
                        space(false, choices[i]);
                    }
                }
                while (Console.KeyAvailable == false)
                {
                    Thread.Sleep(50);
                }
                ck = Console.ReadKey(true);
                if (ck.Key == ConsoleKey.UpArrow)
                {
                    cursorPos = MoveCursor(cursorPos, choices.Length, true);
                }
                if (ck.Key == ConsoleKey.DownArrow)
                {
                    cursorPos = MoveCursor(cursorPos, choices.Length, false);
                }
                if (ck.Key == ConsoleKey.Enter)
                {
                    if (cursorPos == 0)
                    {
                        cont = true;
                    }
                    if (cursorPos == 1)
                    {
                        concentScr();
                    }
                }
            }
        }
        static public void space(bool cursor, string word)
        {
            double wordLen = word.Length;
            int spaceLen = Convert.ToInt32(Math.Ceiling((80 - wordLen) / 2));
            for (int i = 0; i < spaceLen - 2; i++)
            {
                Console.Write(" ");
            }
            if (cursor)
            {
                Console.WriteLine("->{0}", word);
            }
            else
            {
                Console.WriteLine("  {0}", word);
            }
        }
        static public int MoveCursor(int pos, int max, bool isUp) 
        {
            //true up
            //false down
            if (isUp == true)
            {
                if (pos == 0)
                {
                    return max - 1;
                }
                else
                {
                    pos--;
                    return pos;
                }
            }
            if (isUp == false)
            {
                if (pos == max - 1)
                {
                    return pos = 0;
                }
                else
                {
                    pos++;
                    return pos;
                }
            }
            else
            {
                return pos;
            }
        }
        public static void concentScr()
        {
            string[] choices2 = { "yes", "no" };
            bool conti = false;
            int cursorPos2 = 0;
            while (conti == false)
            {
                Console.Clear();
                space(false, "are you sure?");
                Console.WriteLine();
                for (int i = 0; i < 2; i++)
                {
                    if (i == cursorPos2)
                    {
                        space(true, choices2[i]);
                    }
                    else
                    {
                        space(false, choices2[i]);
                    }
                }
                while (Console.KeyAvailable == false)
                {
                    Thread.Sleep(50);
                }
                ck = Console.ReadKey(true);
                if (ck.Key == ConsoleKey.UpArrow)
                {
                    cursorPos2 = MoveCursor(cursorPos2, 2, true);
                }
                if (ck.Key == ConsoleKey.DownArrow)
                {
                    cursorPos2 = MoveCursor(cursorPos2, 2, false);
                }
                if (ck.Key == ConsoleKey.Enter)
                {
                    if (cursorPos2 == 0)
                    {
                        Environment.Exit(0);
                    }
                    if (cursorPos2 == 1)
                    {
                        conti = true;
                    }
                }
            }
        }
        public static int scoreAdder(double sec)
        {
            //the first second returns 0
            if (sec == 0)
            {
                return 0;
            }
            double mult = sec / 2;
            return Convert.ToInt32(Math.Ceiling(mult));
        }
        public static void moveChar(int sec)
        {
            //TO DO: create class for the whole X line
            //the right edge
            int[] edge = { 79, 159, 239, 319, 399, 479, 559, 639 };
            //max number of walls
            int numWallsMax = 5;
            //minimum number of walls
            int numWallsMin = 2;
            //number of walls
            int numWalls;
            //picking the wall position before appending to wallPos
            int wall;
            if (sec == 15)
            {
                if (spaceCounter == spaceX)
                {
                    spaceCounter = 4;
                }
                spaceX = 4;
                numWallsMin = 3;
                numWallsMax = 6;
            }
            if (sec == 35)
            {
                numWallsMin = 5;
                numWallsMax = 7;
            }
            //other time options
            //loop through the whole array and through all of it's positions
            for (int i = 0; i < wallPos.Count; i++)
            {
                //if wallpos is the position of the the left edge
                while (wallPos[i] == 0||wallPos[i] == 80||wallPos[i] == 160||wallPos[i] == 240||wallPos[i] == 320||wallPos[i] == 400||wallPos[i] == 480||wallPos[i] == 560)
                {
                    //remove X from board
                    board[wallPos[i]] = " ";
                    //remove X from position array
                    wallPos.RemoveAt(i);
                }
                //else
                //remove X from board
                board[wallPos[i]] = " ";
                //decrease the wall position by 1
                wallPos[i]--;
                //have the new position contain X on the board
                board[wallPos[i]] = "X";
            }
            //if it's time to create new X line
            if (spaceCounter == spaceX)
            {
                //have num walls be a random number between the min and max number of walls
                numWalls = rnd.Next(numWallsMin, numWallsMax + 1);
                //loop the number of walls
                for (int i = 0; i < numWalls; i++)
                {
                    //wall variable chooses from the 10 choices in edge array for it's position
                    wall = rnd.Next(0, 8);
                    //while wall position is already in wallPos array
                    while (wallPos.Contains(edge[wall]))
                    {
                        //try the next position if it's empty
                        //if wall was at the bottom try the top position
                        if (wall == 7)
                        {
                            wall = 0;
                        }
                        else
                        {
                            wall++;
                        }
                    }
                    //add to positions one of the edge positions chosen by the wall variable as the index
                    wallPos.Add(edge[wall]);
                    //add X to the right side of the game
                    board[edge[wall]] = "X";
                }
                spaceCounter = 0;
            }
        }
        public static int accelerator(double sec)
        {
            if (sec <= 10)
            {
                return 200;
            }
            if (sec >= 150)
            {
                return 50;
            }
            //double multi = sec / 10;
            int multi = Convert.ToInt32(Math.Round(sec / 10, 0, MidpointRounding.AwayFromZero));
            int speedSub = multi * 10;
            return 200 - speedSub;
        }
    }
}
