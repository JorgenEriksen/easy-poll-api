using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyPollAPI.Models
{
    public class Question
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public int QuestionOrder { get; set; }
        public PollGame PollGame { get; set; }
        public virtual ICollection<QuestionAlternative> QuestionAlternatives { get; set; }

    }
}
