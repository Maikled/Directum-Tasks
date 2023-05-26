using MeetingsApp.Interfaces;
using System.Text;

namespace MeetingsApp.Actions
{
    internal class ActionShowDayMeetings : IAction
    {
        private List<IMeeting> _meetings;
        private const string _operationInputMessage = "\nОперация просмотра встреч на определённый день:\n"
            + "Введите дату в формате \"DD/MM/YYYY\"";

        private string _message = "На данный день встреч нет";

        public ActionShowDayMeetings(List<IMeeting> meetings)
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
                foreach(var elem in meetings)
                {
                    builder.Append(MeetingManager.ViewMeeting(elem));
                }

                if(builder.Length > 0)
                {
                    _message = $"Количество встреч в этот день {meetings.Count()}\n" + builder.ToString();
                }
            }
        }

        public DateOnly ParseInput(string input)
        {
            if(DateOnly.TryParse(input, out DateOnly date))
            {
                return date;
            }
            throw new Exception("Введены некорректные данные");
        }
    }
}
