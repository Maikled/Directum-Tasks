using MeetingsApp.Interfaces;

namespace MeetingsApp
{
    internal class MeetingManager
    {
        private System.Timers.Timer _timer = new System.Timers.Timer();
        private List<IMeeting> _meetings;
        public MeetingManager(List<IMeeting> meetings)
        {
            _meetings = meetings;
            InitTimer();
        }

        ~MeetingManager()
        {
            DisposeTimer();
        }

        private void InitTimer()
        {
            _timer.AutoReset = true;
            _timer.Elapsed += (s, e) => MeetingsCheckNotify();
            _timer.Elapsed += (s, e) => MeetingsCheckRunning();
            _timer.Elapsed += (s, e) => MeetingsCheckFinished();
            _timer.Elapsed += (s, e) => CorrectionTimeTimer();
            _timer.Start();
        }

        private void DisposeTimer()
        {
            _timer.Stop();
            _timer.Dispose();
        }

        private void CorrectionTimeTimer()
        {
            _timer.Interval = (60 - DateTime.Now.Second) * 1000;
        }

        private void MeetingsCheckNotify()
        {
            var meetings = _meetings.Where(p => p.Status == Enums.MeetingStatus.Planed && p.StatusNotify == Enums.MeetingStatusNotify.NotNotified && (p.StartMeeting.Subtract(DateTime.Now) <= p.TimeMinuteNotify.ToTimeSpan()));
            foreach (var meeting in meetings)
            {
                if (meeting.TimeMinuteNotify != new TimeOnly())
                {
                    Console.WriteLine($"{DateTime.Now}| Уведомление: встреча с ID \"{meeting.ID}\" начнётся в {meeting.StartMeeting.ToShortTimeString()}, напомнание за \"{meeting.TimeMinuteNotify.Minute}\" минут");
                    meeting.StatusNotify = Enums.MeetingStatusNotify.Notified;
                }
            }
        }

        private void MeetingsCheckRunning()
        {
            var meetings = _meetings.Where(p => p.Status == Enums.MeetingStatus.Planed && (p.StartMeeting <= DateTime.Now));
            foreach (var meeting in meetings)
            {
                meeting.Status = Enums.MeetingStatus.Running;
            }
        }

        private void MeetingsCheckFinished()
        {
            var meetings = _meetings.Where(p => p.Status == Enums.MeetingStatus.Running && (p.EndMeeting <= DateTime.Now));
            foreach (var meeting in meetings)
            {
                meeting.Status = Enums.MeetingStatus.Finished;
            }
        }

        public static bool CheckMeetingsIntersection(IMeeting meeting, IEnumerable<IMeeting> meetings)
        {
            if(meeting != null && meetings != null)
            {
                if (meetings.Count() == 0)
                    return true;

                bool result = false;
                foreach(var elem in meetings)
                {
                    if(meeting.StartMeeting > elem.EndMeeting || elem.StartMeeting > meeting.EndMeeting)
                    {
                        result = true;
                    }
                }

                return result;
            }
            else
                return false;
        }

        public static bool TryConvertInputData(string date, string time, out DateTime dateTime)
        {
            if (DateOnly.TryParse(date, out var resultDate) && TimeOnly.TryParse(time, out var resultTime))
            {
                dateTime = resultDate.ToDateTime(resultTime);
                return true;
            }
            else
            {
                dateTime = DateTime.MinValue;
                return false;
            }
        }

        public static string ViewMeeting(IMeeting meeting)
        {
            if(meeting != null)
            {
                var message = $"ID: \"{meeting.ID}\", Статус: \"{meeting.Status}\"  Дата начала: \"{meeting.StartMeeting.ToShortDateString()}\", Время начала: \"{meeting.StartMeeting.ToShortTimeString()}\", Дата окончания: \"{meeting.EndMeeting.ToShortDateString()}\", Время окончания: \"{meeting.EndMeeting.ToShortTimeString()}\"";
                if (meeting.TimeMinuteNotify != new TimeOnly())
                    message += $",  Напоминание за \"{meeting.TimeMinuteNotify.Minute}\" минут;\n";
                return message;
            }
            else
                return "";
        }
    }
}
