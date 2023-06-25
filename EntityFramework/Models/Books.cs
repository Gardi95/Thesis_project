using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFramework.Models
{

    public class Books
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key()]
        //[ig]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public DateTime PublicationDate { get; set; }
        public string Publisher { get; set; }
        public int Pages { get; set; }
        public string ShelfLocation { get; set; }
    }
}
