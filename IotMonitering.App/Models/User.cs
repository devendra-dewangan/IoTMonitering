using System.ComponentModel.DataAnnotations;

namespace IoTMonitoring.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string UserID { get; set; } = string.Empty;
        public IEnumerable<Device> Devices { get; set; } = [];
    }
}
