using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System.Linq;


namespace Snake
{
    struct Location //static structure for locations allowing two locations for X and Y
    {
        public int X;
        public int Y;

        public Location(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    };

    enum Direction { Left, Right, Up, Down }; // sets a default condition for the directions
    enum DirectionP2 { Left, Right, Up, Down }; // sets a default condition for the directions

    class Score // public class for the high scores, allows to store two variables using a string (name) , and int for points
    {
        public string Name { get; set; }
        public int Points { get; set; }

        public Score(string name, int points)
        {
            this.Name = name;
            this.Points = points;
        }
    }

    class Program
    {
        static Direction direction;  //static direction variable
        static DirectionP2 directionP2; //static direction variable for player2
        static List<Location> snake = new List<Location>(); // list of locations for the snake
        static List<Location> snakeP2 = new List<Location>(); // list of locations for player 2
        static Location star = new Location(30, 10); // initial coords for the spawn point of the apple
        static List<Location> barrier = new List<Location>(); //list of locations for barrier or obstycles
        static List<Location> starTemp = new List<Location>(); // list of locations for the temp star check
        static List<Score> score = new List<Score>(); //creates a list of scores
        static List<int> scoreList = new List<int>(); // creates a list of scores for the high scores
        public bool collision = false; // bool to check for collesions
        public bool collisionP2 = false; //bool to check for collisions for player 2
        public static bool highScoreAchieved = false; //checks to see if an high score is achieved
        public static int starTempY; // temp Y cord for apple
        public static int starTempX; // temp X cord for apple
        public static int starIntialX; // inital x temp variable
        public static int starIntialY; // inital y temp variable
        public static int starStartX; // start variable for the apple x
        public static int starStartY; // start variable for the apple y
        public static bool active = false; // bool for active state
        public static int currentScore = 0;  //resets Player One Score
        public static int currentScoreP2 = 0; //Resets Player Two Score
        public static bool mapOne = false;                  //** 
        public static bool mapTwo = false;                  // These bools control the map selection and which grid is drawn
        public static bool mapCustom = false;               //**

        public static int startingCordX; // integer for the Starting Cord X Axis For Player One
        public static int startingCordY; //integer for the Starting Cord Y Axis For Player One

        public static string startingDirection; //Starting Direction Of Players ( Using string switch of Right, Left, Up, Down)
        public static string startingDirectonP1; //Starting Direction Of Players ( Using string switch of Right, Left, Up, Down)
        public static string startingDirectonP2; //Starting Direction Of Players ( Using string switch of Right, Left, Up, Down)

        public static int startingCordXP2; // integer for the Starting Cord X Axis For Player Two
        public static int startingCordYP2; //integer for the Starting Cord Y Axis For Player Two
      
        public static bool onePlayer = false;  //One Player Game Mode
        public static bool twoPlayer = false; // Two Player Game Mode

        public static bool easy = false;  //
        public static bool medium = false;// Public bools for difficulty
        public static bool hard = false; //


        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.GetEncoding(1252); //adds special character
            Console.Title = "Snake Game"; // console typing

            //Location head = new Location(40, 12); //starting position of head of snake
            //Location head = new Location(startingCordX, startingCordY);
            //snake.Add(head); // adds the coords for the head of the snake into the list of locations

            Console.CursorVisible = false; //turns off the cursor stopping the flicking
        

            Console.BufferWidth = Console.WindowWidth = 80; //sets console window width size
            Console.BufferHeight = Console.WindowHeight = 30; //sets console window height size


            //direction = Direction.Right; //inital direction
            initalStar(); // runs inital star variable
            intro(); // runs title screen - when breaked 
            menu(); // menu screen

            #region threading

            Thread thread = new Thread(Move); //adds a thread to move function
            Thread thread2 = new Thread(drawGrid); //creates a thread for the drawgrid function
            thread.IsBackground = false; // background thread
            thread.Start(); //starts the thread


            #endregion

            #region key inputs

            while (true) // whilst the game is active
            {

                ConsoleKeyInfo keyInfo = Console.ReadKey(true); //reads the input key though console

                switch (keyInfo.Key) //switch statement based on key input for player one
                {
                    case ConsoleKey.Escape:
                        return;
                    case ConsoleKey.UpArrow:
                        direction = Direction.Up;
                        break;
                    case ConsoleKey.DownArrow:
                        direction = Direction.Down;
                        break;
                    case ConsoleKey.LeftArrow:
                        direction = Direction.Left;
                        break;
                    case ConsoleKey.RightArrow:
                        direction = Direction.Right;
                        break;
                    case ConsoleKey.W:
                        directionP2 = DirectionP2.Up;
                        break;
                    case ConsoleKey.S:
                        directionP2 = DirectionP2.Down;
                        break;
                    case ConsoleKey.A:
                        directionP2 = DirectionP2.Left;
                        break;
                    case ConsoleKey.D:
                        directionP2 = DirectionP2.Right;
                        break;
                }
            }
        }

            #endregion


        public static void Move() //movement function
        {
            bool quit = false; // sets variable
            while (active == true) // whilst this variable that was set is not true will run
            {
                var next = snake[0]; //stores the snake location as next for Player One
                var nextP2 = snakeP2[0]; //stores the snake location as next Player Two

                switch (direction) //switch statement based on direction
                {
                    case Direction.Left: //if the direction is left
                        if (next.X > 1) // move -- on the x axis as long as next.x is greater than one
                            next.X--;
                        break; // code break
                    case Direction.Right: //if the direction is right
                        if (next.X < 60) // move ++ on the x axis as long as the next.x is less than 60
                            next.X++;
                        break;
                    case Direction.Up: //if the direction is up
                        if (next.Y > 1) //move -- on the y axis as long as next.y is greater than one
                            next.Y--;
                        break;
                    case Direction.Down: //if the direction is down
                        if (next.Y < 24) // move ++ on the y axis as long as the next.y is less than 24
                            next.Y++;
                        break;
                }


                switch (directionP2) //switch statement based on direction
                {
                    case DirectionP2.Left: //if the direction is left
                        if (nextP2.X > 1) // move -- on the x axis as long as next.x is greater than one
                            nextP2.X--;
                        break; // code break
                    case DirectionP2.Right: //if the direction is right
                        if (nextP2.X < 60) // move ++ on the x axis as long as the next.x is less than 60
                            nextP2.X++;
                        break;
                    case DirectionP2.Up: //if the direction is up
                        if (nextP2.Y > 1) //move -- on the y axis as long as next.y is greater than one
                            nextP2.Y--;
                        break;
                    case DirectionP2.Down: //if the direction is down
                        if (nextP2.Y < 24) // move ++ on the y axis as long as the next.y is less than 24
                            nextP2.Y++;
                        break;
                    //           }
                }
                #region player one detection
                //collisions detection for player one
                foreach (Location location in snake) // for each locations in the list of locations called snake
                {

                    Console.SetCursorPosition(location.X, location.Y); // sets cursor positon based on x and y cords
                    Console.ForegroundColor = ConsoleColor.Yellow; //sets snake as yellow
                    Console.Write((char)176); // draws ansii code 176
                    Console.ResetColor(); //resets colour of next draw

                    if (next.X == location.X && next.Y == location.Y) // if the next position is hits the border
                    {
                        quit = true; // then quit is true
                        Console.ForegroundColor = ConsoleColor.Black; // sets color of snake to black on game end
                        gameOverScreen(); // shows game over screen
                        break; //breaks code

                    }

                    foreach (Location obst in barrier) // for each locations in the list barriers
                    {
                        if (next.X == obst.X && next.Y == obst.Y) // if the next move x and y is listed in as a obsticle
                        {
                            quit = true; // then quit is true
                            Console.ForegroundColor = ConsoleColor.Black; // sets color of snake to black on game end
                            gameOverScreen(); // shows game over screen
                            break; //breaks code
                        }
                    }

                    foreach (Location locations in snakeP2) // for each locations of player 2
                    {
                        if (next.X == locations.X && next.Y == locations.Y) // if the next move x and y is listed in as a location as player 2
                        {
                            quit = true; // then quit is true
                            Console.ForegroundColor = ConsoleColor.Black; // sets color of snake to black on game end
                            gameOverScreen(); // shows game over screen
                            break; //breaks code
                        }
                    }
                }
                #endregion

                #region player two detection
                //collisions detection for player one
                if (twoPlayer == true)
                {
                    foreach (Location location in snakeP2) // for each locations in the list of locations called snake
                    {

                        Console.SetCursorPosition(location.X, location.Y); // sets cursor positon based on x and y cords
                        Console.ForegroundColor = ConsoleColor.Blue; //sets snake as yellow
                        Console.Write((char)176); // draws ansii code 176
                        Console.ResetColor(); //resets colour of next draw

                        if (nextP2.X == location.X && nextP2.Y == location.Y) // if the next position is hits the border
                        {
                            quit = true; // then quit is true
                            Console.ForegroundColor = ConsoleColor.Black; // sets color of snake to black on game end
                            gameOverScreen(); // shows game over screen
                            break; //breaks code

                        }

                        foreach (Location obst in barrier) // for each locations in the list barriers
                        {
                            if (nextP2.X == obst.X && nextP2.Y == obst.Y) // if the next move x and y is listed in as a obsticle
                            {
                                quit = true; // then quit is true
                                Console.ForegroundColor = ConsoleColor.Black; // sets color of snake to black on game end
                                gameOverScreen(); // shows game over screen
                                break; //breaks code
                            }
                        }

                        foreach (Location locations in snakeP2) // for each locations of player 2
                        {
                            if (nextP2.X == locations.X && nextP2.Y == locations.Y) // if the next move x and y is listed in as a location as player 2
                            {
                                quit = true; // then quit is true
                                Console.ForegroundColor = ConsoleColor.Black; // sets color of snake to black on game end
                                gameOverScreen(); // shows game over screen
                                break; //breaks code
                            }
                        }
                    }
                }
                #endregion
                #region star
                // show star

                Console.SetCursorPosition(star.X, star.Y); // draws apple
                Console.Write('*'); // apple symbol

                snake.Insert(0, next); //insets a snake at the next location for player one
                snakeP2.Insert(0, nextP2); //inserts a snake at the next location for player two
                if (next.X == star.X && next.Y == star.Y) //if the next grid location for player one is a star location
                {
                    currentScore++; //add one to player one score
                    randomStar();// draw random star
                }
                else
                    snake.RemoveAt(snake.Count - 1); // if next move is not a star then remove a snake piece to stop it growing

                if (nextP2.X == star.X && nextP2.Y == star.Y) //if the next grid location for player two is a star location
                {
                    currentScoreP2++; //add one to player two score
                    randomStar(); //draw random star
                }
                else
                    snakeP2.RemoveAt(snakeP2.Count - 1); //if next move is not a star then remove a snake piece to stop it growing

                #endregion star

                if (easy == true) //if the difficulty is easy
                {
                    Thread.Sleep(500);// puts pause between moves
                }
                if(medium == true) // if the difficuty is medium
                {
                    Thread.Sleep(250); // puts pause between moves
                }
                if(hard == true) //if the difficulty is hard
                {
                    Thread.Sleep(100); //put pause between the moves
                }


                Console.Clear(); //clears console
                #region borderdraw
                drawGrid(); // runs the grid drawing function


                #endregion
                Console.CursorVisible = false; // stops blinking cursor
            }
        }

        public static void gameOverScreen() // runs when lose state has been sent, game over screen
        {
            Console.Clear();
            Console.ResetColor(); // resets any colour from the previous program
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Game Over"); // prints out game over
            highScoreTest();
        }

        public static void randomStar()  //creates a random x and y for the apple
        {
            Random random = new Random();

            starTempX = random.Next(1, 60);
            starTempY = random.Next(1, 22);

            foreach (Location obst in barrier)
            {
                if (starTempX != obst.X && starTempY != obst.Y)
                {
                    star.X = starTempX;
                    star.Y = starTempY;
                    break;
                }
                else
                {
                    randomStar();
                    break;
                }
            }
        }

        public static void initalStar()
        {
            Random initialRandom = new Random(); //calls a random number
            starIntialX = initialRandom.Next(1, 60); //selects a random number between 1 and 60
            starIntialY = initialRandom.Next(1, 22); // selects a random number between 1 and 22
            foreach (Location obst in barrier) // checks to see if random number is in the barrier
            {
                if (starIntialX != obst.X && starIntialY != obst.Y)
                {
                    // Random random = new Random();
                    starIntialX = starStartX;
                    starIntialY = starStartY;
                    // star = new Location(starStartX, starStartY);
                    break;
                }
                //star.X = random.Next(0, 60);
                //star.Y = random.Next(0, 22); 
                else
                {
                    initalStar(); //if it collides then it runs again
                    break;
                }
            }
        } //places the inital star

        public static void menu()
        {
            Console.Clear();  //clears the screen
            Console.SetCursorPosition(33, 3); //sets cursor position
            Console.ForegroundColor = ConsoleColor.Yellow; // draws yellow
            Console.WriteLine("Welcome To Snake"); // welcome to snake
            Console.ForegroundColor = ConsoleColor.White; //draws white
            Console.Write("\n\t1:One Player\n\t2:Two Player\n\t3:About\n\t4:Quit"); //menu options

            ConsoleKeyInfo keyInfo = Console.ReadKey(); //checks for key input

            switch (keyInfo.Key)
            {
                case (ConsoleKey.D1): //checks for console key 1
                    onePlayer = true;
                    mapSelection();
                    break;
                case (ConsoleKey.D2):  //checks for console key 2
                    twoPlayer = true;
                    mapSelection();
                    break;
                case (ConsoleKey.D3):  //checks for console key 2
                    about();
                    break;
                case (ConsoleKey.D4):  //checks for console key 2
                    quit();
                    break;
                default: // if anything else is selected, it clears screen and runs option again
                    Console.Clear();
                    menu();
                    break;
            }
        } //main menu screen

        public static void intro()  //intro screen
        {
            /*
             *  Draw the entry screen title logo
            */

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(12, 1);
            Console.Write(@"     _______..__   __.      ___       __  ___  _______ ");
            Console.SetCursorPosition(12, 2);
            Console.Write(@"    /       ||  \ |  |     /   \     |  |/  / |   ____|");
            Console.SetCursorPosition(12, 3);
            Console.Write(@"   |   (----`|   \|  |    /  ^  \    |  '  /  |  |__   ");
            Console.SetCursorPosition(12, 4);
            Console.Write(@"    \   \    |  . `  |   /  /_\  \   |    <   |   __|  ");
            Console.SetCursorPosition(12, 5);
            Console.Write(@".----)   |   |  |\   |  /  _____  \  |  .  \  |  |____ ");
            Console.SetCursorPosition(12, 6);
            Console.Write(@"|_______/    |__| \__| /__/     \__\ |__|\__\ |_______|");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.SetCursorPosition(25, 9);
            Console.WriteLine("Press Any Key To Continue...");
            Console.ReadKey();

        }

        public static void mapSelection() //gives options for map selection
        {
            Console.Clear();
            Console.SetCursorPosition(33, 3);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Select Map");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\n\t1:Map One\n\t2:Map Two\n\t3:Custom Map");

            ConsoleKeyInfo keyInfo = Console.ReadKey(); //checks for key input

            switch (keyInfo.Key)
            {
                case (ConsoleKey.D1): //checks for console key 1
                    mapOne = true;
                    difficulty();
                    break;
                case (ConsoleKey.D2):  //checks for console key 2
                    mapTwo = true;
                    difficulty();
                    break;
                case (ConsoleKey.D3):  //checks for console key 3
                    mapCustom = true;
                    difficulty();
                    break;
                default: // if anything else is selected, it clears screen and runs option again
                    Console.Clear();
                    menu();
                    break;
            }
        }

        public static void quit()  //closes application
        {
            Environment.Exit(0); //closes application

        }

        public static void drawGrid() //function that draws the grid
        {
            Console.SetCursorPosition(star.X, star.Y); // draws apple
            Console.Write('*'); // apple symbol


            Console.SetCursorPosition(0, 0); //sets inital condition of the cursor to the top left corner
            Console.ForegroundColor = ConsoleColor.Red; // draws grid as red
            Console.Write(@"#============================================================# *-------------*
|                                                            | 
|                                                            |
|                                                            |
|                                                            |"); Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("Player One : " + currentScore); Console.ForegroundColor = ConsoleColor.Red; Console.Write(@"
|                                                            |"); if (twoPlayer == true) { Console.ForegroundColor = ConsoleColor.Blue; Console.Write("Player Two : " + currentScoreP2); } Console.ForegroundColor = ConsoleColor.Red; Console.Write(@"
|                                                            |
|                                                            |
|                                                            |
|                                                            |
|                                                            |  -------------
|                                                            |
|                                                            |
|                                                            |
|                                                            |
|                                                            |  Difficulty: "); Console.Write(@"
|                                                            |  
|                                                            |
|                                                            |   
|                                                            |
|                                                            |
|                                                            |
|                                                            |
|                                                            |
#============================================================# *-------------*"); //draws grid


            foreach (Location obst in barrier) // for ever location listed in barrier
            {
                Console.SetCursorPosition(obst.X, obst.Y); // sets the x and y cord
                Console.Write("/"); //draws a forwards slash as a wall
            }

        } // draws the grid, depending on which file it brings in

        public static void about()
        {
            Console.Clear();
            Console.SetCursorPosition(30, 2);
            Console.Write("Loading Custom Maps\n This game supports custom maps but requires input by various files to make it \n work correctly \n \n");
            Console.Write(" File List\n \n grid.txt\n This is the file that holds the coord system for the map, each line is one \n block on the map\n");
            Console.Write(" p1direction.txt\n This holds the initial direction the Player One snake travels -\n Right, Left, Up, Down \n");
            Console.Write(" p2direction.txt\n This holds the initial direction the Player Two snake travels -\n Right, Left, Up, Down \n");
            Console.Write(" startingLocation.txt\n This holds the initial starting point the Player One snake starts \n");
            Console.Write(" startingLocationP2.txt\n This holds the initial starting point the Player Two snake starts \n");
            Console.Write("\n The Playable grid is 24, 60 however the outside grid is already drawn by the \n game automatically");


        }

        public static void firstRunSetup()
        {

            string line;  // Line (n) from file
            string[] cordinates; //string cordinates in array
            string[] startingCord; //string for storing the starting coordinates in an array
            int barrierX; //intger for barrier X axis
            int barrierY; //integer for the barrier Y Axis


            #region Map One
            if (mapOne == true)
            {
                try
                {
                    using (StreamReader reader = new StreamReader("MapOne/grid.txt"))
                    {
                        while ((line = reader.ReadLine()) != null) // as long as there is another line
                        {
                            cordinates = line.Split(','); //split the cordinates in the file 
                            barrierX = Int32.Parse(cordinates[0]); // store first string as an int in the barrierx variable
                            barrierY = Int32.Parse(cordinates[1]); // store the second string as an int in the barrierY variable
                            Location barriertemp = new Location(barrierX, barrierY); //store both variables in one location , x then y
                            barrier.Add(barriertemp); //add the location into the list of the locations of barrier
                            if (onePlayer == true)
                            {
                                startingCordX = 40;
                                startingCordY = 12;
                                startingDirectonP1 = "RIGHT";
                            }
                            if (twoPlayer == true)
                            {
                                startingCordX = 20;
                                startingCordY = 12;
                                startingDirectonP1 = "LEFT";
                                startingCordXP2 = 40;
                                startingCordYP2 = 12;
                                startingDirectonP2 = "RIGHT";

                            }

                        }
                    }
                }
                catch
                {
                    Console.Clear();
                    Console.Write("Map Files Not Found");
                    Console.ReadLine();
                }
            }
            #endregion

            #region Map Two
            if (mapTwo == true)
            {
                try
                {
                    using (StreamReader reader = new StreamReader("MapTwo/grid.txt"))
                    {
                        while ((line = reader.ReadLine()) != null) // as long as there is another line
                        {
                            cordinates = line.Split(','); //split the cordinates in the file 
                            barrierX = Int32.Parse(cordinates[0]); // store first string as an int in the barrierx variable
                            barrierY = Int32.Parse(cordinates[1]); // store the second string as an int in the barrierY variable
                            Location barriertemp = new Location(barrierX, barrierY); //store both variables in one location , x then y
                            barrier.Add(barriertemp); //add the location into the list of the locations of barrier
                            startingCordX = 40;
                            startingCordY = 12;
                            startingDirectonP1 = "LEFT";
                            startingDirectonP2 = "LEFT";
                        }
                    }
                }
                catch
                {
                    Console.Clear();
                    Console.Write("Map Files Not Found");
                    Console.ReadLine();
                }
            }
            #endregion

            #region Custom Map
            if (mapCustom == true)
            {
                try
                {
                    using (StreamReader reader = new StreamReader("Custom/grid.txt"))
                    {
                        while ((line = reader.ReadLine()) != null) // as long as there is another line
                        {
                            cordinates = line.Split(','); //split the cordinates in the file 
                            barrierX = Int32.Parse(cordinates[0]); // store first string as an int in the barrierx variable
                            barrierY = Int32.Parse(cordinates[1]); // store the second string as an int in the barrierY variable
                            Location barriertemp = new Location(barrierX, barrierY); //store both variables in one location , x then y
                            barrier.Add(barriertemp); //add the location into the list of the locations of barrier
                        }
                    }
                }
                catch
                {
                    Console.Clear();
                    Console.Write("Map Files Not Found");
                    Console.ReadLine();
                }

                try
                {
                    using (StreamReader reader = new StreamReader("Custom/startingLocation.txt"))
                    {
                        while ((line = reader.ReadLine()) != null) // as long as there is another line
                        {
                            startingCord = line.Split(','); //split the cordinates in the file 
                            startingCordX = Int32.Parse(startingCord[0]); // store first string as an int in the barrierx variable
                            startingCordY = Int32.Parse(startingCord[1]); // store the second string as an int in the barrierY variable
                            //          reader.Close(); //closes the reader
                        }
                    }
                }
                catch
                {
                    Console.Clear();
                    Console.Write("Starting Location Not Found");
                    Console.ReadLine();
                }
                try
                {
                    using (StreamReader reader = new StreamReader("Custom/startingLocationP2.txt"))
                    {
                        while ((line = reader.ReadLine()) != null) // as long as there is another line
                        {
                            startingCord = line.Split(','); //split the cordinates in the file 
                            startingCordXP2 = Int32.Parse(startingCord[0]); // store first string as an int in the barrierx variable
                            startingCordYP2 = Int32.Parse(startingCord[1]); // store the second string as an int in the barrierY variable
                            //          reader.Close(); //closes the reader
                        }
                    }
                }
                catch
                {
                    Console.Clear();
                    Console.Write("Starting Location For Player 2 Not Found");
                    Console.ReadLine();
                }
                try
                {
                    using (StreamReader reader = new StreamReader("Custom/p1direction.txt"))
                    {
                        string startingDirectionIn;
                        startingDirectionIn = reader.ReadLine();
                        startingDirectonP1 = startingDirectionIn.ToUpper();
                    }
                }
                catch
                {
                    Console.Clear();
                    Console.Write("Starting Direction Not Found");
                    Console.ReadLine();
                }
                try
                {
                    using (StreamReader reader = new StreamReader("Custom/p2direction.txt"))
                    {
                        string startingDirectionIn;
                        startingDirectionIn = reader.ReadLine();
                        startingDirectonP2 = startingDirectionIn.ToUpper();
                    }
                }
                catch
                {
                    Console.Clear();
                    Console.Write("Starting Direction Not Found");
                    Console.ReadLine();
                }
            }


            #endregion

            try
            {
                using (StreamReader reader = new StreamReader("highScore.txt"))
                {

                    while ((line = reader.ReadLine()) != null) // as long as there is another line
                    {
                        // var scores = File.ReadLines("scores.txt");
                        string[] parts = line.Split(',');
                        string name = parts[0];
                        int points = int.Parse(parts[1]);
                        scoreList.Add(points);
                        score.Add(new Score(name, points));
                    }
                }
            }
            catch
            {
                Console.Clear();
                Console.Write("High Scores Not Found");
                Console.ReadLine();
            }

            Location head = new Location(startingCordX, startingCordY);
            snake.Add(head); // adds the coords for the head of the snake into the list of locations
            Location headP2 = new Location(startingCordXP2, startingCordYP2);
            snakeP2.Add(headP2); // adds the coords for the head of the snake into the list of locations
            switch (startingDirectonP1)
            {
                case "RIGHT":
                    direction = Direction.Right;
                    break;
                case "UP":
                    direction = Direction.Up;
                    break;
                case "DOWN":
                    direction = Direction.Down;
                    break;
                case "LEFT":
                    direction = Direction.Left;
                    break;
            }
            switch (startingDirectonP2)
            {
                case "RIGHT":
                    directionP2 = DirectionP2.Right;
                    break;
                case "UP":
                    directionP2 = DirectionP2.Up;
                    break;
                case "DOWN":
                    directionP2 = DirectionP2.Down;
                    break;
                case "LEFT":
                    directionP2 = DirectionP2.Left;
                    break;
            }




            active = true;
        } // inital setup

        public static void highScoreTest()
        {
            Console.Clear();
            // bool highScoreAchieved = false;
            Console.WriteLine("Game Over");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Blue;
            if (currentScore > currentScoreP2)
            {
                Console.WriteLine("Player One Wins With A Score Of : " + currentScore);

                foreach (int points in scoreList)
                {
                    if (currentScore > points)
                    {


                        Console.WriteLine("You have a High Score Player One");
                        Console.WriteLine("Please Enter Your Name Player One :");
                        string playerName = Console.ReadLine();

                        score.Add(new Score(playerName, currentScore));
                        score.Sort(delegate(Score p2, Score p1) { return p1.Points.CompareTo(p2.Points); });
                        int remove = Math.Min(score.Count, 2);
                        score.RemoveAt(3);
                        break;
                        //Console.Clear();
                        //Console.WriteLine("High Score List");
                        //score.ForEach(delegate(Score p) { Console.WriteLine(String.Format("{0} {1}", p.Points, p.Name)); });
                    }
                }
                foreach (int points in scoreList)
                {
                    if (currentScoreP2 > points)
                    {


                        Console.WriteLine("You have a High Score Player Two");
                        Console.WriteLine("Please Enter Your Name Player Two :");
                        string playerName = Console.ReadLine();

                        score.Add(new Score(playerName, currentScoreP2));
                        score.Sort(delegate(Score p2, Score p1) { return p1.Points.CompareTo(p2.Points); });
                        int remove = Math.Min(score.Count, 2);
                        score.RemoveAt(3);
                        break;
                        //Console.Clear();
                        //Console.WriteLine("High Score List");
                        //score.ForEach(delegate(Score p) { Console.WriteLine(String.Format("{0} {1}", p.Points, p.Name)); });
                    }
                }

                Console.Clear();
                Console.WriteLine("High Score List/n");
                score.ForEach(delegate(Score p) { Console.WriteLine(String.Format("{0} {1}", p.Points, p.Name)); });
            }
            if (currentScore < currentScoreP2)
            {
                Console.WriteLine("Player Two Wins With A Score Of : " + currentScoreP2);

                foreach (int points in scoreList)
                {
                    if (currentScoreP2 > points)
                    {


                        Console.WriteLine("You have a High Score Player Two:");
                        Console.WriteLine("Please Enter Your Name Player Two:");
                        string playerName = Console.ReadLine();

                        score.Add(new Score(playerName, currentScoreP2));
                        score.Sort(delegate(Score p2, Score p1) { return p1.Points.CompareTo(p2.Points); });
                        int remove = Math.Min(score.Count, 2);
                        score.RemoveAt(3);
                        break;
                        Console.Clear();
                        Console.WriteLine("High Score List/n");
                        score.ForEach(delegate(Score p) { Console.WriteLine(String.Format("{0} {1}", p.Points, p.Name)); });
                    }
                }
            }
            if (currentScore == currentScoreP2)
            {
                Console.WriteLine("It Is A Draw With Both Players Having A Score Of : " + currentScore);
                Console.ReadKey();
                Console.WriteLine("Press Any Key To Continue");
            }
           
            Console.Clear();
            Console.WriteLine("Thanks For Playing - Game Over\n");
            Console.WriteLine("High Score List");
            score.ForEach(delegate(Score p) { Console.WriteLine(String.Format("{0} {1}", p.Points, p.Name)); });



            using (StreamWriter writer = new StreamWriter("highscore.txt"))
            {
                foreach (Score pair in score)
                {
                    writer.WriteLine("{0},{1}", pair.Name, pair.Points);
                }
            }
            highScoreAchieved = true;
            Console.ReadLine();

        }

        public static void difficulty()
        {
            Console.Clear();
            Console.SetCursorPosition(33, 3);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Select Map");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\n\t1:Easy\n\t2:Medium\n\t3:Hard");

            ConsoleKeyInfo keyInfo = Console.ReadKey(); //checks for key input

            switch (keyInfo.Key)
            {
                case (ConsoleKey.D1): //checks for console key 1
                    easy = true;
                    firstRunSetup();
                    break;
                case (ConsoleKey.D2):  //checks for console key 2
                    medium = true;
                    firstRunSetup();
                    break;
                case (ConsoleKey.D3):  //checks for console key 3
                    hard = true;
                    firstRunSetup();
                    break;
                default: // if anything else is selected, it clears screen and runs option again
                    Console.Clear();
                    menu();
                    break;
            }
        }
    }
}
     
    



