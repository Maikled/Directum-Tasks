using MeetingsApp.Enums;

namespace MeetingsApp.Interfaces
{
    internal interface IMeeting
    {
        public int ID { get; }
        public DateTime StartMeeting { get; }
        public DateTime EndMeeting { get; }
        public MeetingStatus Status { get; set; }
        public MeetingStatusNotify StatusNotify { get; set; }
        public TimeOnly TimeMinuteNotify { get; }
        public void SetStartDateTime(DateTime newStartDateTime);
        public void SetEndDateTime(DateTime newEndDateTime);
        public void SetTimeMinuteNotify(TimeOnly newTimeMinuteNotify);
    }
}