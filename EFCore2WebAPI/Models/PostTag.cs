using EFCore2WebAPI.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore2WebAPI.Models
{
    //Step 3
    //public class PostTag
    //{
    //    public int PostId { get; set; }
    //    public Post Post { get; set; }

    //    public int TagId { get; set; }
    //    public Tag Tag { get; set; }
    //}

    //Step 4
    public class PostTag : IJoinEntity<Post>, IJoinEntity<Tag>
    {
        public int PostId { get; set; }
        public Post Post { get; set; }
        Post IJoinEntity<Post>.Navigation
        {
            get => Post;
            set => Post = value;
        }

        public int TagId { get; set; }
        public Tag Tag { get; set; }
        Tag IJoinEntity<Tag>.Navigation
        {
            get => Tag;
            set => Tag = value;
        }
    }
}
