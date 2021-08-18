using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NToastNotify;
using OpenHackathonWeb.API;
using OpenHackathonWeb.Data;
using OpenHackathonWeb.Extensions;
using OpenHackathonWeb.Helpers;
using OpenHackathonWeb.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OpenHackathonWeb.Controllers
{
    [Authorize]
    public class HackathonController : Controller
    {
        private readonly HackathonDbContext _context;
        private readonly IApiService _apiService;
        private readonly IOptions<AppSettings> _appSettings;
        private readonly IToastNotification _toastNotification;

        private const long OneBitCointInSatoshi = 100_000_000;

        public HackathonController(HackathonDbContext context,
            IApiService apiService,
            IOptions<AppSettings> appSettings,
            IToastNotification toastNotification)
        {
            _context = context;
            _apiService = apiService;
            _appSettings = appSettings;
            _toastNotification = toastNotification;
        }

        private void RegisteredUsersDropDown(int hackathonId)
        {
            var hackathonRegistrations = _context.HackathonRegistrations.Where(x => x.Id == hackathonId).ToList();

            var registeredUsers = (from hr in _context.HackathonRegistrations
                                   join u in _context.Users on hr.UserId equals u.Id
                                   where hr.HackathonId == hackathonId
                                   select new SelectListItem
                                   {
                                       Text = $"{u.FirstName} {u.LastName}",
                                       Value = u.WalletAddress
                                   }).ToList();

            ViewBag.RegisteredUsers = registeredUsers;
        }

        private void ManagersDropDown()
        {
            var managers = _context.Users.Where(x => x.UserRole == (int)UserRoles.HackathonManager)
                .Select(x => new SelectListItem
                {
                    Text = $"{x.FirstName} {x.LastName}",
                    Value = x.WalletAddress
                }).ToList();

            ViewBag.Managers = managers;
        }

        //TODO: remove method
        public async Task<IActionResult> Index()
        {
            return View(await _context.Hackathons.ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            ManagersDropDown();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(HackathonViewModel model)
        {
            if (ModelState.IsValid)
            {
                var manager = _context.Users.FirstOrDefault(x => x.WalletAddress == model.ManagerWalletAddress);
                if (manager == null)
                {
                    _toastNotification.AddErrorToastMessage("Hackathon Manager must be a member");
                    return View(model);
                }

                var createHackathonTransaction = await _apiService.CreateHackathon(model.Title, model.Duration, manager.WalletAddress, User.GetWalletAddress(), model.PrizeAmount);
                if (!createHackathonTransaction.Success)
                {

                    _toastNotification.AddErrorToastMessage("An error occurred while creating entry into Blockchain!");
                    return View(model);
                }

                await Task.Delay(_appSettings.Value.AverageBlockTime);

                var receipt = await _apiService.GetReceipt(createHackathonTransaction.TransactionId);
                if (receipt.Success)
                {
                    var hackathon = new Hackathons
                    {
                        Id = Convert.ToInt32(receipt.ReturnValue),
                        Title = model.Title,
                        Description = model.Description,
                        PrizeAmount = model.PrizeAmount,
                        Manager = manager.Id,
                        Duration = model.Duration
                    };

                    await _context.Hackathons.AddAsync(hackathon);
                    await _context.SaveChangesAsync();

                    _toastNotification.AddSuccessToastMessage("Hackathon successfully created");

                    return RedirectToAction("Index", "Dashboard");
                }

                _toastNotification.AddErrorToastMessage("An error occurred while fetching receipt!");
            }

            ManagersDropDown();
            return View(model);
        }

        //[HttpGet]
        //public IActionResult Deposit()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Deposit(DepositFundViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var depositFundTransaction = await _apiService.DepositFund(User.GetWalletAddress(), model.Amount);
        //        if (!depositFundTransaction.Success)
        //        {
        //            _toastNotification.AddErrorToastMessage("An error occurred while deposit fund!");
        //            return View(model);
        //        }

        //        await Task.Delay(_appSettings.Value.AverageBlockTime);
        //        var receipt = await _apiService.GetReceipt(depositFundTransaction.TransactionId);
        //        if (!receipt.Success)
        //            _toastNotification.AddErrorToastMessage("An error occurred while fetching receipt!");


        //        _toastNotification.AddSuccessToastMessage("Fund successfully deposited");
        //        return RedirectToAction("Index", "Dashboard");
        //    }
        //    return View(model);
        //}

        [HttpGet]
        public IActionResult SetPrize(int hackathonId)
        {
            var model = new SetPrizeViewModel()
            {
                HackathonId = hackathonId
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SetPrize(SetPrizeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var hackathon = _context.Hackathons.FirstOrDefault(x => x.Id == model.HackathonId);
                if(hackathon == null)
                {
                    _toastNotification.AddErrorToastMessage("Hackathon not found.");
                    return View(model);
                }

                if(model.Amount > hackathon.PrizeAmount)
                {
                    _toastNotification.AddErrorToastMessage("You cannot set prize amount than total prize.");
                    return View(model);
                }

                model.SatoshiAmount = model.Amount * OneBitCointInSatoshi;
                var setPrizeTransaction = await _apiService.SetPrize(model.HackathonId, model.Rank, model.SatoshiAmount, User.GetWalletAddress());
                if (!setPrizeTransaction.Success)
                {
                    _toastNotification.AddErrorToastMessage("An error occurred while set prize!");
                    return View(model);
                }

                await Task.Delay(_appSettings.Value.AverageBlockTime);

                var receipt = await _apiService.GetReceipt(setPrizeTransaction.TransactionId);
                if (!receipt.Success)
                    _toastNotification.AddErrorToastMessage("An error occurred while fetching receipt!");

                _toastNotification.AddSuccessToastMessage("Prize successfully set");
                return RedirectToAction("Index", "Dashboard");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult AnnounceWinner(int hackathonId)
        {
            RegisteredUsersDropDown(hackathonId);
            var model = new AnnounceWinnerViewModel()
            {
                HackathonId = hackathonId
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AnnounceWinner(AnnounceWinnerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var announceWinnerTransaction = await _apiService.AnnounceWinner(model.HackathonId, model.Rank, model.WinnerWalletAddress, User.GetWalletAddress());
                if (!announceWinnerTransaction.Success)
                {
                    _toastNotification.AddErrorToastMessage("An error occurred in announce winner");
                    return View(model);
                }

                await Task.Delay(_appSettings.Value.AverageBlockTime);

                var receipt = await _apiService.GetReceipt(announceWinnerTransaction.TransactionId);
                if (!receipt.Success)
                    _toastNotification.AddErrorToastMessage("An error occurred while fetching receipt!");

                _toastNotification.AddSuccessToastMessage("Winner Announced");

                return RedirectToAction("Index", "Dashboard");
            }

            RegisteredUsersDropDown(model.HackathonId);
            return View(model);
        }

        [HttpGet]
        public IActionResult Register(int hackathonId)
        {
            var model = new HackathonRegistrationViewModel() { HackathonId = hackathonId };
            return PartialView("_Register", model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(HackathonRegistrationViewModel model)
        {
            var registerHackathonTransaction = await _apiService.Register(model.HackathonId, User.GetWalletAddress());
            if (!registerHackathonTransaction.Success)
            {
                _toastNotification.AddErrorToastMessage("An error occurred");
                return Json(new { Success = false });
            }

            await Task.Delay(_appSettings.Value.AverageBlockTime);

            var receipt = await _apiService.GetReceipt(registerHackathonTransaction.TransactionId);
            if (receipt.Success)
            {
                await _context.HackathonRegistrations.AddAsync(new HackathonRegistrations()
                {
                    HackathonId = model.HackathonId,
                    UserId = User.GetUserId()
                });

                await _context.SaveChangesAsync();

                _toastNotification.AddErrorToastMessage("Registered Successfully");
                return Json(new { success = true });
            }

            _toastNotification.AddErrorToastMessage("An error occurred");
            return Json(new { Success = false });
        }
    }
}