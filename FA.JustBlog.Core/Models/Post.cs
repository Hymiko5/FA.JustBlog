using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.JustBlog.Core.Models
{
    public class Post:IEntity
    {
        /// <summary>
        /// Unique post id.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Post's title.
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string? Title { get; set; }
        /// <summary>
        /// Short description.
        /// </summary>
        [Display(Name ="Short Description")]
        public string? ShortDescription { get; set; }
        /// <summary>
        /// Post's content.
        /// </summary>
        [Display(Name ="Post Content")]
        public string? PostContent { get; set; }
        /// <summary>
        /// Url Slug.
        /// </summary>
        [MaxLength(255)]
        public string? UrlSlug { get; set; }
        /// <summary>
        /// is published.
        /// </summary>
        public bool Published { get; set; } = false;
        /// <summary>
        /// Posted date.
        /// </summary>
        [Display(Name ="Posted On")]
        public DateTime PostedOn { get; set; }
        /// <summary>
        /// is Modified.
        /// </summary>
        public bool Modified { get; set; } = false;
        /// <summary>
        /// Category id.
        /// </summary>
        [ForeignKey("Category")]
        [Display(Name ="Category")]
        public int CategoryId { get; set; }
        /// <summary>
        /// Category instance.
        /// </summary>
        public Category? Category { get; set; }
        /// <summary>
        /// Post tag map List.
        /// </summary>
        public IList<PostTagMap>? PostTagMaps { get; set; }
        /// <summary>
        /// List comment.
        /// </summary>
        public IList<Comment>? Comments { get; set; }
        /// <summary>
        /// Count of the views.
        /// </summary>
        public int ViewCount { get; set; }
        /// <summary>
        /// Rate Count.
        /// </summary>
        public int RateCount { get; set; }
        /// <summary>
        /// Total rate.
        /// </summary>
        public int TotalRate { get; set; }
        /// <summary>
        /// Avg rate
        /// </summary>
        public decimal Rate => RateCount == 0 ? 0 : TotalRate / RateCount;

    }
}
