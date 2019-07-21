using Microsoft.AspNetCore.Identity;

namespace Blog.MVC.Model
{
    public class Role:IdentityRole<int>
    {
        public string Description { get; set; }
    }
}
