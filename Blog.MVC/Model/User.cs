using Microsoft.AspNetCore.Identity;

namespace Blog.MVC.Model
{
    public class User:IdentityUser<int>
    {
        public string Name { get; set; }
    }
}
