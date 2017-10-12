using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EFCore2WebAPI.Models
{
    public class Post
    {
        //Initial
        //public int PostId { get; set; }
        //public string Title { get; set; }

        //public ICollection<Tag> Tags { get; } = new List<Tag>();

        //Step 1
        //public int PostId { get; set; }
        //public string Title { get; set; }
        //public ICollection<PostTag> PostTags { get; } = new List<PostTag>();

        //Step 2
        //public int PostId { get; set; }
        //public string Title { get; set; }
        //private ICollection<PostTag> PostTags { get; } = new List<PostTag>();

        //[NotMapped]
        //public IEnumerable<Tag> Tags => PostTags.Select(e => e.Tag);

        //Step 3
        //public Post()
        //=> Tags = new JoinCollectionFacade<Tag, PostTag>(
        //    PostTags,
        //    pt => pt.Tag,
        //    t => new PostTag { Post = this, Tag = t });

        //public int PostId { get; set; }
        //public string Title { get; set; }

        //private ICollection<PostTag> PostTags { get; } = new List<PostTag>();

        //[NotMapped]
        //public ICollection<Tag> Tags { get; }

        //Step 4
        public Post() => Tags = new JoinCollectionFacade<Tag, Post, PostTag>(this, PostTags);

        public int PostId { get; set; }
        public string Title { get; set; }

        private ICollection<PostTag> PostTags { get; } = new List<PostTag>();

        [NotMapped]
        public ICollection<Tag> Tags { get; }
    }
}
