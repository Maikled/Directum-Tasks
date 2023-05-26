using MeetingsApp.Enums;
using MeetingsApp.Interfaces;

namespace MeetingsApp.Model
{
    internal class Meeting : IMeeting
    {
        public int ID { get; private set; }
        public DateTime StartMeeting { get; private set; }
        public DateTime EndMeeting { get; private set; }
        public MeetingStatus Status { get; set; } = MeetingStatus.Planed;
        public MeetingStatusNotify StatusNotify { get; set; } = MeetingStatusNotify.NotNotified;
        public TimeOnly TimeMinuteNotify { get; private set; } = new TimeOnly();

        public Meeting(int id)
        {
            ID = id;
        }

        public void SetStartDateTime(DateTime newStartDateTime)
        {
            if (newStartDateTime > DateTime.Now)
                StartMeeting = newStartDateTime;
            else
                throw new Exception("Некорректный ввод данных. Некорректное значение даты или времени начала встречи");
        }

        public void SetEndDateTime(DateTime newEndDateTime)
        {
            if (newEndDateTime > DateTime.Now)
                EndMeeting = newEndDateTime;
            else
                throw new Exception("Некорректный ввод данных. Некорректное значение даты или времени окончания встречи");
        }

        public void SetTimeMinuteNotify(TimeOnly newTimeMinuteNotify)
        {
            if (StartMeeting.Subtract(DateTime.Now) <= newTimeMinuteNotify.ToTimeSpan())
                throw new Exception("Некорректный ввод данных. Некорректное время напоминания");
            else
            {
                TimeMinuteNotify = newTimeMinuteNotify;
            }
        }
    }
}