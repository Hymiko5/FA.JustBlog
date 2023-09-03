using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.JustBlog.Core.Models
{
    public class Tag:IEntity
    {
        /// <summary>
        /// Unique tag id.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Tag's name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Url Slug.
        /// </summary>
        public string UrlSlug { get; set; }
        /// <summary>
        /// Tag's description.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Count.
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// Post tag map List.
        /// </summary>
        public IList<PostTagMap> PostTagMaps { get; set; }
    }
}
