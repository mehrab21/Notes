using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Notes.Migrations;

namespace Notes.Models
{
    public class Note
    {
        [Key]
        public int Id { get; set; }

        public string title { get; set; }
        public string content { get; set; }
        public string tags { get; set; }
        public DateTime createdAt { get; set; } = DateTime.Now;

        [ForeignKey("User")]
        public int UserId { get; set; } // Foreign key to User
        public User User { get; set; }



    }
}
