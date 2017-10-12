using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using EFCore2WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore2WebAPIWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();

            SeedDatabase();

            host.Run();
        }

        public static void SeedDatabase()
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
                var posts = LoadPosts(context, "as added");

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
        }

        private static List<Post> LoadPosts(MyContext context, string message)
        {
            #region OldConsoleStuff
            //Console.WriteLine($"Dumping posts {message}:");
            #endregion

            var posts = context.Posts
                .Include("PostTags.Tag")
                .ToList();

            #region OldConsoleStuff
            //foreach (var post in posts)
            //{
            //    Console.WriteLine($"  Post {post.Title}");
            //    foreach (var tag in post.Tags)
            //    {
            //        Console.WriteLine($"    Tag {tag.Text}");
            //    }
            //}

            //Console.WriteLine();
            #endregion

            return posts;
        }
    }
}
