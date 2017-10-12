using EFCore2.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EFCore2
{
    class Program
    {
        //Step 2 Final
        //    public static void Main(string[] args)
        //    {
        //        using (var context = new MyContext())
        //        {
        //            context.Database.EnsureDeleted();
        //            context.Database.EnsureCreated();

        //            var tags = new[]
        //            {
        //            new Tag { Text = "Golden" },
        //            new Tag { Text = "Pineapple" },
        //            new Tag { Text = "Girlscout" },
        //            new Tag { Text = "Cookies" }
        //        };

        //            var posts = new[]
        //            {
        //            new Post { Title = "Best Boutiques on the Eastside" },
        //            new Post { Title = "Avoiding over-priced Hipster joints" },
        //            new Post { Title = "Where to buy Mars Bars" }
        //        };

        //            context.AddRange(
        //                new PostTag { Post = posts[0], Tag = tags[0] },
        //                new PostTag { Post = posts[0], Tag = tags[1] },
        //                new PostTag { Post = posts[1], Tag = tags[2] },
        //                new PostTag { Post = posts[1], Tag = tags[3] },
        //                new PostTag { Post = posts[2], Tag = tags[0] },
        //                new PostTag { Post = posts[2], Tag = tags[1] },
        //                new PostTag { Post = posts[2], Tag = tags[2] },
        //                new PostTag { Post = posts[2], Tag = tags[3] });

        //            context.SaveChanges();
        //        }

        //        using (var context = new MyContext())
        //        {
        //            var posts = LoadAndDisplayPosts(context, "as added");

        //            posts.Add(context.Add(new Post { Title = "Going to Red Robin" }).Entity);

        //            var newTag1 = new Tag { Text = "Sweet" };
        //            var newTag2 = new Tag { Text = "Buzz" };

        //            //Step 1
        //            //foreach (var post in posts)
        //            //{
        //            //    var oldPostTag = post.PostTags.FirstOrDefault(e => e.Tag.Text == "Pineapple");
        //            //    if (oldPostTag != null)
        //            //    {
        //            //        post.PostTags.Remove(oldPostTag);
        //            //        post.PostTags.Add(new PostTag { Post = post, Tag = newTag1 });
        //            //    }
        //            //    post.PostTags.Add(new PostTag { Post = post, Tag = newTag2 });
        //            //}

        //            //Step 2
        //            foreach (var post in posts)
        //            {
        //                var oldPostTag = GetPostTags(post).FirstOrDefault(e => e.Tag.Text == "Pineapple");
        //                if (oldPostTag != null)
        //                {
        //                    GetPostTags(post).Remove(oldPostTag);
        //                    GetPostTags(post).Add(new PostTag { Post = post, Tag = newTag1 });
        //                }
        //                GetPostTags(post).Add(new PostTag { Post = post, Tag = newTag2 });
        //            }

        //            context.SaveChanges();
        //        }

        //        using (var context = new MyContext())
        //        {
        //            LoadAndDisplayPosts(context, "after manipulation");
        //        }

        //        Console.WriteLine("Press any key to continue ...");
        //        Console.ReadLine();
        //    }

        //    private static List<Post> LoadAndDisplayPosts(MyContext context, string message)
        //    {
        //        Console.WriteLine($"Dumping posts {message}:");

        //        //Step 1
        //        //var posts = context.Posts
        //        //    .Include(e => e.PostTags)
        //        //    .ThenInclude(e => e.Tag)
        //        //    .ToList();

        //        //Step 2
        //        var posts = context.Posts
        //        .Include("PostTags.Tag")
        //        .ToList();

        //        foreach (var post in posts)
        //        {
        //            Console.WriteLine($"  Post {post.Title}");
        //            //Step 1
        //            //foreach (var tag in post.PostTags.Select(e => e.Tag))
        //            //{
        //            //    Console.WriteLine($"    Tag {tag.Text}");
        //            //}

        //            //Step 2
        //            foreach (var tag in post.Tags)
        //            {
        //                Console.WriteLine($"Tag {tag.Text}");
        //            }
        //        }

        //        Console.WriteLine();

        //        return posts;
        //    }

        //    private static ICollection<PostTag> GetPostTags(object entity)
        //    => (ICollection<PostTag>)entity
        //        .GetType()
        //        .GetRuntimeProperties()
        //        .Single(e => e.Name == "PostTags")
        //        .GetValue(entity);
        //}

        //Step 3
        public static void Main()
        {
            using (var context = new MyContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var tags = new[]
                {
                new Tag { Text = "Golden" },
                new Tag { Text = "Pineapple" },
                new Tag { Text = "Girlscout" },
                new Tag { Text = "Cookies" }
            };

                var posts = new[]
                {
                new Post { Title = "Best Boutiques on the Eastside" },
                new Post { Title = "Avoiding over-priced Hipster joints" },
                new Post { Title = "Where to buy Mars Bars" }
            };

                posts[0].Tags.Add(tags[0]);
                posts[0].Tags.Add(tags[1]);
                posts[1].Tags.Add(tags[2]);
                posts[1].Tags.Add(tags[3]);
                posts[2].Tags.Add(tags[0]);
                posts[2].Tags.Add(tags[1]);
                posts[2].Tags.Add(tags[2]);
                posts[2].Tags.Add(tags[3]);

                context.AddRange(tags);
                context.AddRange(posts);

                context.SaveChanges();
            }

            using (var context = new MyContext())
            {
                var posts = LoadAndDisplayPosts(context, "as added");

                posts.Add(context.Add(new Post { Title = "Going to Red Robin" }).Entity);

                var newTag1 = new Tag { Text = "Sweet" };
                var newTag2 = new Tag { Text = "Buzz" };

                foreach (var post in posts)
                {
                    var oldTag = post.Tags.FirstOrDefault(e => e.Text == "Pineapple");
                    if (oldTag != null)
                    {
                        post.Tags.Remove(oldTag);
                        post.Tags.Add(newTag1);
                    }
                    post.Tags.Add(newTag2);
                }

                context.SaveChanges();
            }

            using (var context = new MyContext())
            {
                LoadAndDisplayPosts(context, "after manipulation");
            }

            Console.WriteLine("Press any key to continue ...");
            Console.ReadLine();
        }

        private static List<Post> LoadAndDisplayPosts(MyContext context, string message)
        {
            Console.WriteLine($"Dumping posts {message}:");

            var posts = context.Posts
                .Include("PostTags.Tag")
                .ToList();

            foreach (var post in posts)
            {
                Console.WriteLine($"  Post {post.Title}");
                foreach (var tag in post.Tags)
                {
                    Console.WriteLine($"    Tag {tag.Text}");
                }
            }

            Console.WriteLine();

            return posts;
        }
    }
}