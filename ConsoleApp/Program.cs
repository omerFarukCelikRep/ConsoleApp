using ConsoleApp.Exceptions;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {

        async static Task Main(string[] args)
        {
            MyClass myClass = new MyClass();

            ConsoleKeyInfo input = new ConsoleKeyInfo();

            Console.WriteLine(@"Start with 'P' key to write a command
T:Print Text (Example : 'PT:Text:E')
S:Sound Beep (Example : 'PS:Frequency,Duration:E')");
            string command = "";
            do
            {
                try
                {
                    input = Console.ReadKey();

                    if (IsSpecialCharacter(input))
                    {
                        command += input.KeyChar;


                        Console.Clear();
                        Console.WriteLine(command);

                        string[] commandLine = command.Split(':'); ;

                        if (commandLine.Length == 3 && commandLine[2] != string.Empty)
                        {
                            if (command.EndsWith(":E"))
                            {
                                switch (commandLine[0])
                                {
                                    case "PT":
                                        PrintText(commandLine[1]);

                                        var pub = Task.Run(() =>
                                        {
                                            Task.Delay(1000).Wait();
                                            myClass.PushAck(command, "Valid command").Wait();

                                        });

                                        break;
                                    case "PS":
                                        MakeSound(commandLine[1]);
                                        break;
                                    default:
                                        Console.WriteLine("Please enter valid command");
                                        break;
                                }



                                break;


                            }
                            else
                            {
                                throw new CommanLineMustEndWithEException();

                            }


                        }
                    }
                    else
                    {
                        throw new Exception("Please enter valid character");
                    }
                }
                catch (CommanLineMustEndWithEException ex)
                {
                    Console.Clear();
                    Console.WriteLine(ex.Message);
                    command = "";
                    continue;
                }
                catch (Exception ex)
                {
                    Console.Clear();
                    Console.WriteLine(ex.Message);
                    break;
                }


            } while (true);


            Stopwatch timer = new Stopwatch();
            timer.Start();

            var mymessage = await myClass.WaitForAck("1");
            timer.Stop();

            Console.WriteLine($"message \"{mymessage}\" recieved in {timer.ElapsedMilliseconds} ms");

        }

        private static bool IsSpecialCharacter(ConsoleKeyInfo input)
        {
            return input.KeyChar < 128 && input.KeyChar > 31;
        }

        private static void PrintText(string text)
        {
            Console.WriteLine(text);
        }

        private static void MakeSound(string command)
        {
            try
            {
                string[] parameters = command.Split(',');

                int frequency = int.Parse(parameters[0]);
                int duration = int.Parse(parameters[1]);

                Console.Beep(frequency, duration);

            }
            catch (Exception ex)
            {

                throw new Exception("Please enter a valid command" + ex.Message);
            }
        }
    }
}
