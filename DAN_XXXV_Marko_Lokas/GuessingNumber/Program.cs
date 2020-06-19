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
                Console.Write("\n"+ new string('=', 50) + "\n");
                Console.Write("Choose: ");
                string choseMainMenu = Console.ReadLine();
                switch (choseMainMenu)
                {
                    case "1":
                        Thread FirstThread = new Thread(new ThreadStart(InitialSettings));
                        FirstThread.Start();
                        FirstThread.Join();

                        foreach (var item in ListParticipants)
                        {
                            Console.WriteLine(item.Name.ToString());
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
                                break;
                            }
                            else if (odgovor.ToLower() == "no")
                            {
                                Console.Clear();
                                yesNo = false;
                                mainMenu = true;
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

            Thread Thread_Generator = new Thread(new ThreadStart(CreateThreads));

            //Insert number participants
            Console.WriteLine("Press \"0\" go to main menu.");
            do
            {
                Console.Write("Enter your number of participants: ");
                string participantsNumber = Console.ReadLine();
                if (participantsNumber == "0")
                {
                    return;
                }
                participantBool = int.TryParse(participantsNumber, out participantNumberInt);
                if (!participantBool || participantNumberInt < 1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please enter a number greater than zero...");
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
                    return;
                }
                guessBool = int.TryParse(guessNumber, out guessNumberInt);
                if(guessNumberInt < 1 || guessNumberInt > 100 || !guessBool)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please enter a number between 0 and 100.");
                    Console.ForegroundColor = ConsoleColor.Green;
                    guessBool = false;
                }
            } while (!guessBool);

            NumberGuessInt = guessNumberInt;
            
            Thread_Generator.Start();
            Console.WriteLine("The number of participants has just been entered");
            Console.WriteLine("Participants number: " + NumberParticipantsInt);
            Console.WriteLine("The number to be guessed is determined");
            //Console.WriteLine("Game loading...");
            //Console.WriteLine("Everything is ready for the game to begin...");

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

        public static void GuessNumbers()
        {

        }

    }
}
