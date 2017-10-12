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
        public Post Get()
        {
            //return new string[] { "value1", "value2" };
            Post value = new Post { Title = "New HTTP POST" };

            using (var context = new MyContext())
            {
                //return LoadPosts(context);
                //AddPost(context);

                //Add Post To Context
                AddPostFromValue(context, value);

                //Update Post In Context
                //UpdatePostFromValue(context, value.PostId);
            }

            List<Post> ret = new List<Post>();

            using (var context = new MyContext())
            {
                ret = LoadPosts(context);
                //return Json(ret);

                Post newValue = ret.Where(x => x.Title == value.Title).FirstOrDefault();

                //Update Post Out Of Context With Post Object
                UpdatePostFromValue(context, newValue);
                
                //Post Updating Tags
                ret = LoadPosts(context);

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
        public Post Get(int id)
        {
            using (var context = new MyContext())
            {
                return LoadPost(context, id);
            }
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]Post value)
        {
            using (var context = new MyContext())
            {
                AddPostFromValue(context, value);
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Post value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        private List<Post> LoadPosts(MyContext context)
        {
            var posts = context.Posts
                .Include("PostTags.Tag")
                .ToList();

            return posts;
        }

        private Post LoadPost(MyContext context, int id)
        {
            var posts = context.Posts
                .Include("PostTags.Tag")
                .ToList();

            Post ret = posts.Where(x => x.PostId == id).FirstOrDefault();

            return ret;
        }

        private void AddPost(MyContext context)
        {
            var posts = LoadPosts(context);

            posts.Add(context.Add(new Post { Title = "New HTTP POST" }).Entity);

            context.SaveChanges();
        }

        private void AddPostFromValue(MyContext context, Post value)
        {
            var posts = LoadPosts(context);

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

            context.SaveChanges();
        }

        private void UpdatePostFromValue(MyContext context, Post value)
        {
            var posts = LoadPosts(context);

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

            context.SaveChanges();
        }
    }
}
