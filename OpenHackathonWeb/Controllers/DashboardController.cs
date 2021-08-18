using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenHackathonWeb.API;
using OpenHackathonWeb.Data;
using OpenHackathonWeb.Extensions;
using OpenHackathonWeb.Helpers;
using OpenHackathonWeb.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OpenHackathonWeb.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly HackathonDbContext _context;
        private readonly IApiService _apiService;

        public DashboardController(HackathonDbContext context,
            IApiService apiService)
        {
            _context = context;
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var hackathonList = await _context.Hackathons.ToListAsync();

            var hackathonViewModel = from h in hackathonList
                                     join u in _context.Users on h.Manager equals u.Id
                                     select new HackathonViewModel
                                     {
                                         Id = h.Id,
                                         Title = h.Title,
                                         Duration = h.Duration,
                                         ManagerId = u.Id,
                                         ManagerName = u.FirstName,
                                         PrizeAmount = h.PrizeAmount
                                     };

            var hackathonViewModelList = hackathonViewModel.ToList();

            if (User.GetRole() == (int)UserRoles.HackathonManager)
                hackathonViewModelList = hackathonViewModelList.Where(x => x.ManagerId == User.GetUserId()).ToList();

            foreach (var hackathon in hackathonViewModelList)
            {
                hackathon.IsUserRegistered = _context.HackathonRegistrations.Any(x => x.UserId == User.GetUserId() && x.HackathonId == hackathon.Id);
            }

            return View(hackathonViewModelList);
        }

        [HttpGet]
        public async Task<IActionResult> GetAddressBalance()
        {
            return Json(await _apiService.AddressBalance(User.GetWalletAddress()));
        }
    }
}