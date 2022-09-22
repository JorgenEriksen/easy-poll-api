using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyPollAPI.Models
{
    public class PollGame
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int StatusId { get; set; }
        public PollGameStatusType Status { get; set; }
        public string InviteCode { get; set; }
        public int CurrentQuestionOrder { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<TempUser> TempUsers { get; set; }
    }
}
