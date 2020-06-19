using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GuessingNumber
{
    class Program
    {
        //Number participants in game
        public static int NumberParticipantsInt;
        //number guess
        public static int NumberGuessInt;
        //List all participants
        public static List<Thread> ListParticipants = new List<Thread>();
        public static Thread Thread_Generator = new Thread(new ThreadStart(CreateThreads));
        public static bool exitApp = true;
        static void Main(string[] args)
        {
            bool mainMenu = true;
            do
            {
                NumberParticipantsInt = 0;
                NumberGuessInt = 0;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Welcome to Application for Guessing Number\n\n");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("1. Start game");
                Console.WriteLine("\n0. Leave Application");
                Console.Write("\n" + new string('=', 50) + "\n");
                Console.Write("Choose: ");
                string choseMainMenu = Console.ReadLine();
                switch (choseMainMenu)
                {
                    case "1":
                        NumberGuessInt = 0;
                        NumberParticipantsInt = 0;
                        //Crate and start first thread
                        Thread FirstThread = new Thread(new ThreadStart(InitialSettings));
                        FirstThread.Start();
                        FirstThread.Join();

                        //when the first thread is completed, the second thread is also completed
                        //all threads are started after the second thread has generated all new threads
                        foreach (var item in ListParticipants)
                        {
                            item.Start();
                        }

                        //Logic to print the main menu
                        if (exitApp == false)
                        {
                            break;
                        }
                        Console.ReadKey();
                        if (exitApp == false)
                        {
                            mainMenu = false;
                        }
                        else
                        {
                            mainMenu = true;
                        }

                        break;
                    case "0":
                        //Exit app
                        bool yesNo = true;
                        do
                        {
                            Console.WriteLine("\n*  *  *  *  *  *  *  *  *  *  *");
                            Console.WriteLine("* Are you sure want to leave? *");
                            Console.WriteLine("*          Yes   /   No        *");
                            Console.WriteLine("*  *  *  *  *  *  *  *  *  *  *");
                            Console.Write("Choose: ");
                            string odgovor = Console.ReadLine();
                            if (odgovor.ToLower() == "yes")
                            {
                                Console.Clear();
                                yesNo = false;
                                mainMenu = false;
                                exitApp = false;
                                break;
                            }
                            else if (odgovor.ToLower() == "no")
                            {
                                Console.Clear();
                                yesNo = false;
                                mainMenu = true;
                                exitApp = false;
                                break;
                            }
                            else if (odgovor.ToLower() != "no" || odgovor.ToLower() != "yes")
                            {
                                Console.Clear();
                                Console.WriteLine("\n\tYou can only enter \"Yes\" or \"No\"\n\n");

                            }
                        } while (yesNo);
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Wrong input, please try again...");
                        break;
                }
            } while (mainMenu);
        }
        /// <summary>
        /// Method for entering initial settings
        /// Entry of participants - participantNumberInt
        /// Enter the number to be guessed - guessNumberInt
        /// </summary>
        public static void InitialSettings()
        {
            bool participantBool = false;
            int participantNumberInt;
            bool guessBool = false;
            int guessNumberInt;

            //Insert number participants
            Console.WriteLine("Press \"0\" go to main menu.");
            do
            {
                Console.Write("Enter your number of participants (1 to 100): ");
                string participantsNumber = Console.ReadLine();
                if (participantsNumber == "0")
                {
                    Console.Clear();
                    Console.WriteLine("Press enter to returned Main menu.");
                    return;
                }
                participantBool = int.TryParse(participantsNumber, out participantNumberInt);
                if (!participantBool || participantNumberInt < 1 || participantNumberInt > 100)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Enter the number of participants between 1 and 100: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    participantBool = false;
                }
            } while (!participantBool);
            NumberParticipantsInt = participantNumberInt;

            //Insert number needed to guess
            do
            {
                Console.Write("Enter the number needed to guess (1 to 100): ");
                string guessNumber = Console.ReadLine();
                if (guessNumber == "0")
                {
                    Console.Clear();
                    Console.WriteLine("Press enter to returned Main menu..");
                    return;
                }
                guessBool = int.TryParse(guessNumber, out guessNumberInt);
                if (guessNumberInt < 1 || guessNumberInt > 100 || !guessBool)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please enter a number between 0 and 100.");
                    Console.ForegroundColor = ConsoleColor.Green;
                    guessBool = false;
                }
            } while (!guessBool);
            NumberGuessInt = guessNumberInt;

            //Starting Thread_Generator thread, that creates new threads for all participants
            Thread_Generator.Start();
            Console.Clear();
            //When the creation of threads begins, it is displayed how 
            //many participants there are and that the number of selected ones
            Console.WriteLine("The number of participants has just been entered");
            Console.WriteLine("Participants number: " + NumberParticipantsInt);
            Console.WriteLine("The number to be guessed is determined");

            //Waiting for all threads to be created
            Thread_Generator.Join();
        }

        /// <summary>
        /// Method for creating threds
        /// </summary>
        public static void CreateThreads()
        {
            for (int i = 1; i <= NumberParticipantsInt; i++)
            {
                ListParticipants.Add(new Thread(new ThreadStart(GuessNumbers)));
                string nameThread = "Participant_";
                ListParticipants.LastOrDefault().Name = string.Format(nameThread + i);
            }
        }

        /// <summary>
        /// Locker
        /// </summary>
        private static readonly object locker = new object();

        /// <summary>
        /// An indicator of whether a resource is locked
        /// </summary>
        static bool lockTaken = false;

        /// <summary>
        /// Method for guessing numbers
        /// </summary>
        public static void GuessNumbers()
        {
            bool exitWhile = true;
            //A loop in which all threads rotate until the given number is hit
            while (exitWhile == true)
            {
                //Generate random number
                int randomNum = RandomNumber();
                Thread thread = Thread.CurrentThread;
                //If there is only one thread, no lock is required
                if (ListParticipants.Count == 1)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("\n" + new string('=', 50));

                    bool evenBool;
                    bool oddBool;
                    //If the specified number is hit
                    if (NumberGuessInt == randomNum)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("\n" + new string('*', 50));
                        Console.WriteLine($"\n{thread.Name.ToString()} won, and the required number was {NumberGuessInt}");
                        Console.Write("\n" + new string('*', 50));
                        exitWhile = false;
                        exitApp = false;
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("Press 2X any key to exit app");
                        Console.ReadKey();
                        System.Environment.Exit(0);
                    }
                    //If the specified number is not hit
                    else
                    {
                        Console.WriteLine($"\n\nParticipiant: {thread.Name.ToString()}\nParticipant number: {randomNum}");
                        if (NumberGuessInt % 2 == 0)
                        {
                            evenBool = true;
                            oddBool = false;
                        }
                        else
                        {
                            evenBool = false;
                            oddBool = true;
                        }
                        //Print if the number being guessed is even
                        if (randomNum % 2 == 0 && evenBool)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine(thread.Name.ToString() + " hit the parity of the number! \n(EVEN NUMBERs)");
                            Console.ForegroundColor = ConsoleColor.Red;

                        }
                        //Print if the number being guessed is odd
                        else if (randomNum % 2 == 1 && oddBool)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine(thread.Name.ToString() + " hit the parity of the number! \n(ODD NUMBERs)");
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        Console.Write(new string('=', 50));
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                }
                //If there are more participants, it is necessary to lock the printout
                else
                {
                    lock (locker)
                    {
                        Monitor.Pulse(locker);
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("\n" + new string('=', 50));

                        bool evenBool;
                        bool oddBool;
                        //If the specified number is hit
                        if (NumberGuessInt == randomNum)
                        {
                            lock (locker)
                            {
                                //When it finds the correct number, it locks the printout 
                                //and does not allow further movement through the loop, other threads
                                lockTaken = false;
                                try
                                {

                                    Monitor.Enter(locker, ref lockTaken);
                                    Monitor.Wait(locker);
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.Write("\n" + new string('*', 50));
                                    Console.WriteLine($"\n{thread.Name.ToString()} won, and the required number was {NumberGuessInt}");
                                    Console.Write("\n" + new string('*', 50));
                                    exitWhile = false;
                                    exitApp = false;
                                    Console.ForegroundColor = ConsoleColor.Blue;
                                    Console.WriteLine("Press 2X any key to exit app");
                                    Console.ReadLine();
                                    //exit application
                                    System.Environment.Exit(0);
                                }
                                finally
                                {
                                }
                            }
                        }
                        //If the specified number is not hit
                        else
                        {
                            Console.WriteLine($"\n\nParticipiant: {thread.Name.ToString()}\nParticipant number: {randomNum}");
                            if (NumberGuessInt % 2 == 0)
                            {
                                evenBool = true;
                                oddBool = false;
                            }
                            else
                            {
                                evenBool = false;
                                oddBool = true;
                            }
                            //Print if the number being guessed is even
                            if (randomNum % 2 == 0 && evenBool)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine(thread.Name.ToString() + " hit the parity of the number! \n(EVEN NUMBERs)");
                                Console.ForegroundColor = ConsoleColor.Red;

                            }
                            //Print if the number being guessed is odd
                            else if (randomNum % 2 == 1 && oddBool)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine(thread.Name.ToString() + " hit the parity of the number! \n(ODD NUMBERs)");
                                Console.ForegroundColor = ConsoleColor.Red;
                            }
                            Console.Write(new string('=', 50));
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                    }
                }

            }
        }

        /// <summary>
        /// Method for random number generation
        /// </summary>
        /// <returns></returns>
        static int RandomNumber()
        {
            Thread.Sleep(100);
            Random random = new Random();
            return random.Next(1, 101);
        }
    }
}
