using MeetingsApp.Interfaces;
using MeetingsApp.Model;

namespace MeetingsApp.Actions
{
    internal class ActionAddMeeting : IAction
    {
        private List<IMeeting> _meetings;
        private const string _operationInputMessage = "\nОперация добавления встречи:\n"
            + "Введите данные встречи:\n \tдата начала встречи,\n \tвремя начала встречи,\n \tдата окончания встречи,\n \tвремя окончания встречи,\n \tнапоминание за несколько минут до встречи (необязательно)\n"
            + "Формат ввода данных: \"DD/MM/YYYY MM:HH DD/MM/YYYY MM:HH MM\" или \"DD/MM/YYYY MM:HH DD/MM/YYYY MM\"\n";

        private string _message = "";

        public ActionAddMeeting(List<IMeeting> meetings)
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
            if(query != null)
            {
                ParseInput(query, out IMeeting? meeting);
                if(meeting != null)
                {
                    if (MeetingManager.CheckMeetingsIntersection(meeting, _meetings))
                    {
                        _meetings.Add(meeting);
                        _message = "Встреча успешно добавлена";
                    }
                    else
                        throw new Exception("Данная встреча пересекается с другими встречами");
                }
            }
        }

        private void ParseInput(string input, out IMeeting? meeting)
        {
            meeting = null;
            var query = input.Trim().Split(" ");
            
            if(query.Length >= 4)
            {
                if (MeetingManager.TryConvertInputData(query[0], query[1], out var dateTimeStart) && MeetingManager.TryConvertInputData(query[2], query[3], out var dateTimeEnd))
                {
                    if (dateTimeStart < dateTimeEnd)
                    {
                        meeting = new Meeting(_meetings.Count);
                        meeting.SetStartDateTime(dateTimeStart);
                        meeting.SetEndDateTime(dateTimeEnd);

                        if (query.Length == 5 && uint.TryParse(query[4], out uint notifyMinuts))
                        {
                            meeting.SetTimeMinuteNotify(new TimeOnly().AddMinutes(notifyMinuts));
                        }

                        return;
                    }
                    throw new Exception("Некорректный ввод данных. Начало встречи должно быть до окончания встречи");
                }
            }

            throw new Exception("Некорректный ввод данных");
        }
    }
}
