namespace TripEnjoy.Presentation.Razor.ViewModels
{
    public class Rate
    {
        public int RateId { get; set; }
        public int RateValue { get; set; }
        public DateTime RateDate { get; set; }
        public int RoomId { get; set; }
        public int AccountId { get; set; }
        public string FullName { get; set; }
        public string CommentContent { get; set; }
    }
}
