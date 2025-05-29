using System.ComponentModel.DataAnnotations.Schema;
using Notes.Migrations;

namespace Notes.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string adress { get; set; }
        
        public List<Note> Notes { get; set; } = new List<Note>();

    }
}
