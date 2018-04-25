using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using WebApplication2.Data;

namespace WebApplication2.Controllers
{
    [Route("Photo")]
    [Authorize]
    public class PhotoController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private IMemoryCache _cache;

        public PhotoController(UserManager<ApplicationUser> userManager, IMemoryCache cache)
        {
            _userManager = userManager;
            _cache = cache;
        }

        public async Task<IActionResult> Get()
        {
            var userid = User.Identity.Name;
            var photo = await _cache.GetOrCreateAsync(userid, async foo => {
                foo.SetSlidingExpiration(TimeSpan.FromMinutes(5));
                var user = await _userManager.GetUserAsync(User);
                return user.Photo;
            });
            return File(photo, "image/png");
        }

    }
}