namespace MVCSite.Models
{
    public class TimelineModel
	{
        public string day { get; set; }
        public string hour { get; set; }
        public List<string> characters { get; set; }
        public string location { get; set; }
        public string description { get; set; }
        public int pageNumber { get; set; }
    }
}
