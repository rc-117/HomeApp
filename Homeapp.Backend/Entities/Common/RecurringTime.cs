namespace Homeapp.Backend.Entities
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The time of day that a recurring item will occur. Represented with integers. 
    /// </summary>
    public class RecurringTime
    {
        [Range(0, 24)]
        public int Hour { get; set; }

        [Range(0, 59)]
        public int Minutes { get; set; }
    }
}