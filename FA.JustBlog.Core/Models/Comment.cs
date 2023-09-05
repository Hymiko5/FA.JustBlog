using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.JustBlog.Core.Models
{
    public class Comment:IEntity
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string? Name { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string? Email { get; set; }
        public int PostId { get; set; }
        [ForeignKey("PostId")]
        public Post? Post { get; set; }
        [MaxLength(255)]
        public string? CommentHeader { get; set; }
        public string? CommentText { get; set; }
        public DateTime CommentTime { get; set; }
    }
}
