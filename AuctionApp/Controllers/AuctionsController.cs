using System.Data;
using AuctionApp.Core;
using AuctionApp.Core.Interfaces;
using AuctionApp.Models.Auction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionApp.Controllers
{
    [Authorize]
    public class AuctionsController : Controller
    {
        private IAuctionService _auctionService;

        public AuctionsController(IAuctionService auctionService)
        {
            _auctionService = auctionService;
        }

        [AllowAnonymous]
        public ActionResult Index(string filter = "active")
        {
            try
            {
                List<Auction> auctions = new List<Auction>();
                string userName = User.Identity.Name;

                if (filter == "active")
                {
                    auctions = _auctionService.GetActiveAuctions();
                }
                else if (filter == "userBids")
                {
                    auctions = _auctionService.GetAuctionByUserBids(userName);
                }

                List<AuctionVm> auctionVms = new List<AuctionVm>();
                foreach (var auction in auctions)
                {
                    auctionVms.Add(AuctionVm.FromAuction(auction));
                }

                ViewData["Filter"] = filter;
                return View(auctionVms);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        public ActionResult Details(Guid id)
        {
            try
            {
                Auction auction = _auctionService.GetAuctionDetails(id);
                AuctionDetailsVm detailsVm = AuctionDetailsVm.FromAuction(auction);
                
                // this to toggle redirection based on previous page url
                string referrerUrl = Request.Headers["Referer"].ToString();
                ViewData["ReferrerUrl"] = referrerUrl;
                
                // add the auction owner's username to ViewBag
                // to disable PlaceBid link in the list own auctions 
                ViewBag.AuctionOwner = auction.User;
                
                return View(detailsVm);
            }
            catch (DataException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateAuctionVm createAuctionVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string userName = User.Identity.Name;
                    string name = createAuctionVm.AuctionName;
                    string description = createAuctionVm.AuctionDescription;
                    decimal openingBid = createAuctionVm.OpeningBid;
                    DateTime expirationDate = createAuctionVm.ExpirationDate;

                    _auctionService.CreateAuction(userName, name, description, openingBid, expirationDate);
                    return RedirectToAction("MyAccount");
                }

                return View(createAuctionVm);
            }
            catch (DataException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(createAuctionVm);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An unexpected error occurred.");
                return View(createAuctionVm);
            }
        }

        public ActionResult MyAccount(string filter = "myAuctions")
        {
            try
            {
                List<Auction> auctions = new List<Auction>();
                string userName = User.Identity.Name;

                if (filter == "myAuctions")
                {
                    auctions = _auctionService.GetAuctionByUserName(userName);
                } 
                else if (filter == "wonAuctions")
                {
                    auctions = _auctionService.GetAuctionUserHasWon(userName);
                }

                // fetch and sort by expiration date descending (newest first)
                var auctionVms = auctions.Select(a => AuctionVm.FromAuction(a))
                    .OrderByDescending(a => a.ExpirationDate)
                    .ToList();

                ViewData["Filter"] = filter;
                return View(auctionVms);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        public ActionResult PlaceBid(Guid auctionId)
        {
            if (IsUserOwnerOfAuction(auctionId)) return BadRequest();

            ViewBag.AuctionId = auctionId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PlaceBid(Guid auctionId, PlaceBidVm placeBidVm)
        {
            if (IsUserOwnerOfAuction(auctionId)) return BadRequest();

            try
            {
                if (ModelState.IsValid)
                {
                    string userName = User.Identity.Name;
                    decimal bidPrice = placeBidVm.BidAmount;
                    _auctionService.PlaceBid(userName, bidPrice, auctionId);
                    return RedirectToAction("Details", new { id = auctionId });
                }
                ViewBag.AuctionId = auctionId;
                return View(placeBidVm);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewBag.AuctionId = auctionId;
                return View(placeBidVm);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An unexpected error occurred.");
                ViewBag.AuctionId = auctionId;
                return View(placeBidVm);
            }
        }
        
        public ActionResult Edit(Guid auctionId)
        {
            if (!IsUserOwnerOfAuction(auctionId)) return BadRequest();

            try
            {
                Auction auction = _auctionService.GetAuctionDetails(auctionId);
                if (auction == null)
                    return NotFound();

                var editVm = new EditAuctionDescriptionVm
                {
                    Description = auction.Description
                };

                ViewBag.AuctionId = auctionId;
                return View(editVm);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid auctionId, EditAuctionDescriptionVm editAuctionDescriptionVm)
        {
            if (!IsUserOwnerOfAuction(auctionId)) return BadRequest();

            try
            {
                if (ModelState.IsValid)
                {
                    string userName = User.Identity.Name;
                    string description = editAuctionDescriptionVm.Description;
                    _auctionService.EditDescription(userName, description, auctionId);
                    return RedirectToAction("Details", new { id = auctionId });
                }
                ViewBag.AuctionId = auctionId; 
                return View(editAuctionDescriptionVm);
            }
            catch (DataException ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewBag.AuctionId = auctionId;
                return View(editAuctionDescriptionVm);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An unexpected error occurred.");
                ViewBag.AuctionId = auctionId;
                return View(editAuctionDescriptionVm);
            }
        }
        private bool IsUserOwnerOfAuction(Guid auctionId)
        {
            try
            {
                Auction auction = _auctionService.GetAuctionDetails(auctionId);
                if (auction == null)
                {
                    return false;
                }
                return auction.User.Equals(User.Identity.Name, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        } 
    }
}   