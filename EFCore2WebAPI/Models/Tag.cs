using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EFCore2WebAPI.Models
{
    public class Tag
    {
        //Initial
        //public int TagId { get; set; }
        //public string Text { get; set; }

        //public ICollection<Post> Posts { get; } = new List<Post>();

        //Step 1
        //public int TagId { get; set; }
        //public string Text { get; set; }
        //public ICollection<PostTag> PostTags { get; } = new List<PostTag>();

        //Step 2
        //public int TagId { get; set; }
        //public string Text { get; set; }
        //private ICollection<PostTag> PostTags { get; } = new List<PostTag>();

        //[NotMapped]
        //public IEnumerable<Post> Posts => PostTags.Select(e => e.Post);

        //Step 3
        //public Tag()
        //=> Posts = new JoinCollectionFacade<Post, PostTag>(
        //    PostTags,
        //    pt => pt.Post,
        //    p => new PostTag { Post = p, Tag = this });

        //public int TagId { get; set; }
        //public string Text { get; set; }

        //private ICollection<PostTag> PostTags { get; } = new List<PostTag>();

        //[NotMapped]
        //public ICollection<Post> Posts { get; }

        //Step 4
        public Tag() => Posts = new JoinCollectionFacade<Post, Tag, PostTag>(this, PostTags);

        public int TagId { get; set; }
        public string Text { get; set; }

        private ICollection<PostTag> PostTags { get; } = new List<PostTag>();

        [NotMapped]
        public IEnumerable<Post> Posts { get; }
    }
}
