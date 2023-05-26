using MeetingsApp.Interfaces;
using System.Text;

namespace MeetingsApp.Actions
{
    internal class ActionShowMeetings : IAction
    {
        private List<IMeeting> _meetings;
        private const string _operationInputMessage = "\nОперация просмотра всех встреч:\n";

        private string _message = "Встречи отсутствуют";

        public ActionShowMeetings(List<IMeeting> meetings)
        {
            _meetings = meetings;
        }

        public string InputInfoOperation()
        {
            return _operationInputMessage;
        }

        public string OutputInfoOperation()
        {
            return $"Количество встреч: {_meetings.Count}\n" + _message;
        }

        public void StartAction()
        {
            var builder = new StringBuilder();
            foreach (var elem in _meetings)
            {
                builder.Append(MeetingManager.ViewMeeting(elem));
            }

            if (builder.Length > 0)
            {
                _message = builder.ToString();
            }
        }
    }
}
