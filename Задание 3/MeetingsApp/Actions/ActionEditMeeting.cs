using MeetingsApp.Interfaces;
using MeetingsApp.Model;

namespace MeetingsApp.Actions
{
    internal class ActionEditMeeting : IAction
    {
        private List<IMeeting> _meetings;
        private const string _operationInputMessage = "\nОперация изменения встречи:\n"
            + "Введите данные встречи:\n \tID встречи,\n \tдата начала встречи,\n \tвремя начала встречи,\n \tдата окончания встречи,\n \tвремя окончания встречи,\n \tнапоминание за несколько минут до встречи (необязательно)\n"
            + "Формат ввода данных: \"DD/MM/YYYY MM:HH DD/MM/YYYY MM:HH MM\" или \"DD/MM/YYYY MM:HH DD/MM/YYYY MM\"\n";

        private string _message = "";

        public ActionEditMeeting(List<IMeeting> meetings)
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
                ParseInput(query, out IMeeting? meeting);
                if (meeting != null)
                {
                    var oldMeeting = _meetings.FirstOrDefault(p => p.ID == meeting.ID);
                    if(oldMeeting != null)
                    {
                        if (oldMeeting.Status == Enums.MeetingStatus.Planed)
                        {
                            var meetings = _meetings.Where(p => p.ID != oldMeeting.ID);
                            if (MeetingManager.CheckMeetingsIntersection(meeting, meetings))
                            {
                                oldMeeting.SetStartDateTime(meeting.StartMeeting);
                                oldMeeting.SetEndDateTime(meeting.EndMeeting);
                                oldMeeting.SetTimeMinuteNotify(meeting.TimeMinuteNotify);
                                _message = $"Встреча с ID {meeting.ID} изменена";
                            }
                            else
                            {
                                throw new Exception("Данная встреча пересекается с другими встречами");
                            }
                        }
                        else
                            throw new Exception("Нельзя изменить встречу, которая уже идёт либо завершена");
                    }
                    else
                        throw new Exception($"Встреча с ID \"{meeting.ID}\" не найдена");
                }
            }
            
        }

        private void ParseInput(string input, out IMeeting? meeting)
        {
            meeting = null;
            var query = input.Trim().Split(" ");

            if (query.Length >= 5)
            {
                if (MeetingManager.TryConvertInputData(query[1], query[2], out var dateTimeStart) && MeetingManager.TryConvertInputData(query[3], query[4], out var dateTimeEnd))
                {
                    if (dateTimeStart < dateTimeEnd && int.TryParse(query[0], out var index))
                    {
                        meeting = new Meeting(index);
                        meeting.SetStartDateTime(dateTimeStart);
                        meeting.SetEndDateTime(dateTimeEnd);
                        if (query.Length == 6 && uint.TryParse(query[5], out var notifyMinuts))
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
