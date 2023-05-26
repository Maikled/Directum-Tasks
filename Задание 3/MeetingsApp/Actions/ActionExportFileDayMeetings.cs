using MeetingsApp.Interfaces;
using System.Text;

namespace MeetingsApp.Actions
{
    internal class ActionExportFileDayMeetings : IAction
    {
        private List<IMeeting> _meetings;
        private const string _operationInputMessage = "\nОперация экспорта всех встреч на определённый день в текстовый файл:\n"
            + "Введите дату в формате \"DD/MM/YYYY\"";

        private string _message = "На данный день встреч нет";

        public ActionExportFileDayMeetings(List<IMeeting> meetings)
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
                var date = ParseInput(query);
                var dateTime = date.ToDateTime(new TimeOnly());
                var meetings = _meetings.Where(p => dateTime >= p.StartMeeting.Date && dateTime <= p.EndMeeting.Date);

                var builder = new StringBuilder();
                foreach (var elem in meetings)
                {
                    builder.Append(MeetingManager.ViewMeeting(elem));
                }

                var fileMessage = $"Выбранный день: {dateTime.ToShortDateString()}\n" + _message;
                if (builder.Length > 0)
                {
                    fileMessage = $"Выбранный день: {dateTime.ToShortDateString()}\nКоличество встреч в этот день {meetings.Count()}\n" + builder.ToString();
                }

                var fileName = "DayMeetings.txt";
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    var bytes = Encoding.UTF8.GetBytes(fileMessage);
                    stream.Write(bytes);
                }

                _message = $"Данные записаны в текстовый файл, путь к файлу {path}";
            }
        }

        public DateOnly ParseInput(string input)
        {
            if (DateOnly.TryParse(input, out DateOnly date))
            {
                return date;
            }
            throw new Exception("Введены некорректные данные");
        }
    }
}
