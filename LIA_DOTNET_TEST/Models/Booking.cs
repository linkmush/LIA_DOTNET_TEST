namespace LIA_DOTNET_TEST.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int Day { get; set; }
        public virtual TimeSlot? TimeSlot { get; set; }
        public virtual User? User { get; set; }
    }
}
