using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GuessingNumber
{
    class Program
    {
        public static int NumberParticipantsInt;
        public static int NumberGuessInt;
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
                        Thread FirstThread = new Thread(new ThreadStart(InitialSettings));
                        FirstThread.Start();
                        FirstThread.Join();

                        foreach (var item in ListParticipants)
                        {
                            item.Start();
                        }
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
                    Console.WriteLine("You have successfully returned to the Main menu\n Press enter.");
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
                    Console.WriteLine("You have successfully returned to the Main menu\n Press enter.");
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

            Thread_Generator.Start();
            Console.Clear();
            Console.WriteLine("The number of participants has just been entered");
            Console.WriteLine("Participants number: " + NumberParticipantsInt);
            Console.WriteLine("The number to be guessed is determined");

            Thread_Generator.Join();

        }

        public static void CreateThreads()
        {
            for (int i = 1; i <= NumberParticipantsInt; i++)
            {
                ListParticipants.Add(new Thread(new ThreadStart(GuessNumbers)));
                string nameThread = "Participant_";
                ListParticipants.LastOrDefault().Name = string.Format(nameThread + i);
            }
        }

        private static readonly object locker = new object();
        static bool lockTaken = false;
        public static void GuessNumbers()
        {
            bool exitWhile = true;
            while (exitWhile == true)
            {

                int randomNum = RandomNumber();
                Thread thread = Thread.CurrentThread;
                if (ListParticipants.Count == 1)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("\n" + new string('=', 50));

                    bool evenBool;
                    bool oddBool;

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
                        Console.ReadLine();
                        System.Environment.Exit(0);
                    }
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

                        if (randomNum % 2 == 0 && evenBool)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine(thread.Name.ToString() + " hit the parity of the number!");
                            Console.ForegroundColor = ConsoleColor.Red;

                        }
                        else if (randomNum % 2 == 1 && oddBool)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine(thread.Name.ToString() + " hit the parity of the number!");
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        Console.Write(new string('=', 50));
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                }
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

                        if (NumberGuessInt == randomNum)
                        {
                            lock (locker)
                            {
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
                                    System.Environment.Exit(0);
                                }
                                finally
                                {
                                    //if (lockTaken) Monitor.Exit(locker);
                                }

                            }

                        }
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

                            if (randomNum % 2 == 0 && evenBool)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine(thread.Name.ToString() + " hit the parity of the number!");
                                Console.ForegroundColor = ConsoleColor.Red;

                            }
                            else if (randomNum % 2 == 1 && oddBool)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine(thread.Name.ToString() + " hit the parity of the number!");
                                Console.ForegroundColor = ConsoleColor.Red;
                            }
                            Console.Write(new string('=', 50));
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                    }
                }

            }
        }

        static int RandomNumber()
        {
            Thread.Sleep(100);
            Random random = new Random();
            return random.Next(1, 101);
        }

    }
}
