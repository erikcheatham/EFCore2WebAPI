using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EFCore2WebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Text;

namespace EFCore2WebAPIWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public async Task<Post> Get()
        {
            //return new string[] { "value1", "value2" };
            Post value = new Post { Title = "New HTTP POST" };

            using (var context = new MyContext())
            {
                //return LoadPosts(context);
                //await AddPost(context);

                //Add Post To Context
                await AddPostFromValueAsync(context, value);

                //Update Post In Context
                //await UpdatePostFromValue(context, value.PostId);
            }

            List<Post> ret = new List<Post>();

            using (var context = new MyContext())
            {
                ret = await LoadPostsAsync(context);
                //return await Json(ret);

                Post newValue = ret.Where(x => x.Title == value.Title).FirstOrDefault();

                //Update Post Out Of Context With Post Object
                await UpdatePostFromValueAsync(context, newValue);
                
                //Post Updating Tags
                ret = await LoadPostsAsync(context);

                value = ret.Where(x => x.Title == value.Title).FirstOrDefault();
            }

            return ret.Last();

            //return new HttpResponseMessage()
            //{
            //    Content = new StringContent(JArray.FromObject(ret).ToString(), Encoding.UTF8, "application/json")
            //};
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<Post> Get(int id)
        {
            using (var context = new MyContext())
            {
                return await LoadPostAsync(context, id);
            }
        }

        // POST api/values
        [HttpPost]
        public async Task Post([FromBody]Post value)
        {
            using (var context = new MyContext())
            {
                await AddPostFromValueAsync(context, value);
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody]Post value)
        {
            await Task.FromResult(false);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await Task.FromResult(false);
        }

        private async Task<List<Post>> LoadPostsAsync(MyContext context)
        {
            var posts = await context.Posts
                .Include("PostTags.Tag")
                .ToListAsync();

            return posts;
        }

        private async Task<Post> LoadPostAsync(MyContext context, int id)
        {
            var posts = await context.Posts
                .Include("PostTags.Tag")
                .ToListAsync();

            Post ret = posts.Where(x => x.PostId == id).FirstOrDefault();

            return ret;
        }

        private async Task AddPostAsync(MyContext context)
        {
            var posts = await LoadPostsAsync(context);

            posts.Add(context.Add(new Post { Title = "New HTTP POST" }).Entity);

            await context.SaveChangesAsync();
        }

        private async Task AddPostFromValueAsync(MyContext context, Post value)
        {
            var posts = await LoadPostsAsync(context);

            var tags = new[]
                {
                new Tag { Text = "Golden" },
                new Tag { Text = "Pineapple" },
                new Tag { Text = "Girlscout" },
                new Tag { Text = "Cookies" }
            };

            value.Tags.Add(tags[0]);
            value.Tags.Add(tags[1]);
            value.Tags.Add(tags[2]);
            value.Tags.Add(tags[3]);

            posts.Add(context.Add(value).Entity);

            await context.SaveChangesAsync();
        }

        private async Task UpdatePostFromValueAsync(MyContext context, Post value)
        {
            List<Post> posts = await LoadPostsAsync(context);

            //posts.Add(context.Add(value).Entity);

            var newTag1 = new Tag { Text = "Sweet" };
            var newTag2 = new Tag { Text = "Buzz" };

            foreach (var post in posts)
            {
                //if (post == value)
                //{
                    var oldTag = post.Tags.FirstOrDefault(e => e.Text == "Pineapple");
                    if (oldTag != null)
                    {
                        post.Tags.Remove(oldTag);
                        post.Tags.Add(newTag1);
                    }
                    post.Tags.Add(newTag2);
                //}
            }

            posts.ToList().ForEach(c => c.Tags.Where(x => x.Text == "Sweet").Select(x => x.Text = "Pineapple"));

            //posts.ToList().ForEach(c => c == value).Where(x => x == value).Select(x => x.Text = "Pineapple"));

            //posts.Where(x => x == value).FirstOrDefault().Tags.Select(e => { e.Text = "Pineapple"; }).ToList();
            //posts.Where(x => x == value).FirstOrDefault().Tags.Select(e => { e.Text = value; return "Pineapple";  }).ToList();

            #region OldManyUpdate
            //foreach (var post in posts)
            //{
            //    var oldTag = post.Tags.FirstOrDefault(e => e.Text == "Pineapple");
            //    if (oldTag != null)
            //    {
            //        post.Tags.Remove(oldTag);
            //        post.Tags.Add(newTag1);
            //    }
            //    post.Tags.Add(newTag2);
            //}
            #endregion

            await context.SaveChangesAsync();
        }
    }
}
