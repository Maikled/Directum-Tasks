using MeetingsApp.Interfaces;

namespace MeetingsApp.Actions
{
    internal class ActionRemoveMeeting : IAction
    {
        private List<IMeeting> _meetings;
        private const string _operationInputMessage = "\nОперация удаления встречи:\n"
            + "Введите ID встречи для удаления\n";

        private string _message = "";

        public ActionRemoveMeeting(List<IMeeting> meetings)
        {
            _meetings = meetings;
        }
        public string InputInfoOperation()
        {
            return _operationInputMessage;
        }

        public string OutputInfoOperation()
        {
            return _message;
        }

        public void StartAction()
        {
            var query = Console.ReadLine();
            if (query != null)
            {
                var index = ParseInput(query);
                var meeting = _meetings.FirstOrDefault(p => p.ID == index);
                if (meeting != null)
                {
                    _meetings.Remove(meeting);
                    _message = "Встреча удалена";
                }
                else
                    throw new Exception($"Встреча не найдена");
            }
        }

        public int ParseInput(string input)
        {
            var query = input.Trim();
            if(int.TryParse(query, out var result))
            {
                return result;
            }
            throw new Exception("Некорректный ввод данных");
        }
    }
}
