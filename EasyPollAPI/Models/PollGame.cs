using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyPollAPI.Models
{
    public class PollGame
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public bool HasStarted { get; set; }
        public bool AdminIsParticipating { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<TempUser> TempUsers { get; set; }
    }
}
