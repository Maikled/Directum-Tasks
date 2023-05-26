using MeetingsApp.Output;
using MeetingsApp.Interfaces;
using MeetingsApp.Actions;
using MeetingsApp.Input;

namespace MeetingsApp
{
    internal class Program
    {
        static List<IMeeting> Meetings = new List<IMeeting>();
        static void Main(string[] args)
        {
            new MeetingManager(Meetings);

            while(true)
            {
                Console.WriteLine(ConsoleInput.CommandInformation());
                var inputKey = Console.ReadKey();

                IAction? action = null;
                if(inputKey.Key == ConsoleKey.F1)
                {
                    action = new ActionAddMeeting(Meetings);
                }
                if(inputKey.Key == ConsoleKey.F2)
                {
                    action = new ActionEditMeeting(Meetings);
                }
                if (inputKey.Key == ConsoleKey.F3)
                {
                    action = new ActionRemoveMeeting(Meetings);
                }
                if (inputKey.Key == ConsoleKey.F4)
                {
                    action = new ActionShowMeetings(Meetings);
                }
                if (inputKey.Key == ConsoleKey.F5)
                {
                    action = new ActionShowDayMeetings(Meetings);
                }
                if (inputKey.Key == ConsoleKey.F6)
                {
                    action = new ActionExportFileDayMeetings(Meetings);
                }
                if (inputKey.Key == ConsoleKey.Escape)
                {
                    return;
                }

                if (action != null)
                {
                    try
                    {
                        ConsoleOutput.Print(action.InputInfoOperation());
                        action.StartAction();
                        ConsoleOutput.Print(action.OutputInfoOperation());
                    }
                    catch(Exception ex)
                    {
                        ConsoleOutput.Print(ex.Message);
                    }
                }
                else
                {
                    ConsoleOutput.Print("Некорректный ввод");
                }
            }
        }
    }
}