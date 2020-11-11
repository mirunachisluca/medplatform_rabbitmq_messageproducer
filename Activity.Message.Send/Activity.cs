using System;

namespace Activity.Message.Send
{
    public class Activity
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ActivityName { get; set; }
        public int PatientId { get; set; }
    }
}
