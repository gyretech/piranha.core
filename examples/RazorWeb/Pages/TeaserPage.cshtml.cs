using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Piranha;
using Piranha.AspNetCore.Models;
using Piranha.Models;
using RazorWeb.Models;

namespace RazorWeb.Pages
{
    public class TeaserPageModel : SinglePageModel<TeaserPage>
    {
        private readonly IDb _db;

        public TeaserPageModel(IApi api, IAuthorizationService auth, IDb db) : base(api, auth)
        {
            _db = db;
        }

        public override async Task<IActionResult> OnGet(Guid id, bool draft = false)
        {
            var result = await base.OnGet(id, draft);

            if (Data != null && Data.IsStartPage)
            {
                var latest = await _db.Posts
                    .Where(p => p.Published <= DateTime.Now)
                    .OrderByDescending(p => p.Published)
                    .Take(1)
                    .Select(p => p.Id)
                    .ToListAsync();

                if (latest.Count() > 0)
                {
                    Data.LatestPost = await _api.Posts
                        .GetByIdAsync<PostInfo>(latest.First());
                }
            }
            return result;
        }
    }
}