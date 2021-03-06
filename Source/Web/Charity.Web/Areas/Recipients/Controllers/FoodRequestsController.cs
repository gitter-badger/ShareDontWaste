﻿namespace Charity.Web.Areas.Recipients.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using PagedList;
    using Charity.Common;
    using Charity.Data.Models;
    using Charity.Services.Common;
    using Charity.Web.Areas.Recipients.Models;
    using Charity.Web.Infrastructure.Identity;    

    [Authorize(Roles = GlobalConstants.RecipientRoleName)]
    public class FoodRequestsController : Controller
    {
        private readonly IFoodRequestService foodRequestService;
        private readonly IFoodDonationService foodDonationService;
        private readonly IRecipientProfileService recipientProfileService;
        private readonly ICurrentUser currentUserProvider;
        private readonly IFoodRequestCommentService foodRequestCommentService;

        public FoodRequestsController(
            IFoodRequestService foodRequestService,
            IFoodDonationService foodDonationService,
            IRecipientProfileService recipientProfileService,
            ICurrentUser currentUserProvider,
            IFoodRequestCommentService foodRequestCommentService)
        {
            this.foodRequestService = foodRequestService;
            this.foodDonationService = foodDonationService;
            this.recipientProfileService = recipientProfileService;
            this.currentUserProvider = currentUserProvider;
            this.foodRequestCommentService = foodRequestCommentService;
        }

        public ActionResult MyRequests(int? page)
        {
            int pageSize = 10;
            int pageIndex = (page ?? 1);

            ApplicationUser user = this.currentUserProvider.Get();
            Recipient recipient = this.recipientProfileService.GetByApplicationUserId(user.Id);

            var foodRequests = this.foodRequestService.ListByRecipient(recipient.Id)
                .Project().To<FoodRequestListViewModel>();

            var foodRequestsPagedList = foodRequests.ToPagedList(pageIndex, pageSize);

            return View(foodRequestsPagedList);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var foodRequest = this.foodRequestService.GetById((int)id);

            if (foodRequest == null)
            {
                return HttpNotFound();
            }

            ApplicationUser user = this.currentUserProvider.Get();
            var recipient = this.recipientProfileService.GetByApplicationUserId(user.Id);

            if (foodRequest.RecipientId != recipient.Id)
            {
                throw new HttpException(400, "This is not your request");
            }

            var model = Mapper.Map<FoodRequest, FoodRequestViewModel>(foodRequest);

            // Mark all comments as read
            this.foodRequestCommentService.MarkCommentsAsReadFromRecipient(foodRequest.Id);

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FoodRequestRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var foodRequest = Mapper.Map<FoodRequestRegisterModel, FoodRequest>(model);

                ApplicationUser user = this.currentUserProvider.Get();
                var recipient = this.recipientProfileService.GetByApplicationUserId(user.Id);

                foodRequest.RecipientId = recipient.Id;

                var donation = this.foodDonationService.GetById(model.FoodDonationId);
                if (donation == null || donation.IsCompleted == true)
                {
                    throw new HttpException(404, "Donation not found");
                }

                var existingRequest = this.foodRequestService.GetByDonationIdAndRecipientId(model.FoodDonationId, recipient.Id);
                if (existingRequest != null)
                {
                    throw new HttpException(400, "The request already exists");
                }

                this.foodRequestService.Add(foodRequest);

                return Content("Your request is sent");
            }

            throw new HttpException(400, "Invalid request");
        }

        public ActionResult Edit(int? id)
        {
            ApplicationUser user = this.currentUserProvider.Get();
            Recipient recipient = this.recipientProfileService.GetByApplicationUserId(user.Id);
            FoodRequest foodRequest = this.foodRequestService.GetById((int)id);

            if (recipient.Id != foodRequest.RecipientId)
            {
                return RedirectToAction("Index");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (foodRequest == null)
            {
                return HttpNotFound();
            }

            FoodRequestEditModel model = Mapper.Map<FoodRequest, FoodRequestEditModel>(foodRequest);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FoodRequestEditModel model)
        {
            var foodRequest = this.foodRequestService.GetById(model.Id);

            Mapper.Map<FoodRequestEditModel, FoodRequest>(model, foodRequest);

            if (ModelState.IsValid)
            {
                this.foodRequestService.Update(foodRequest);
                return RedirectToAction("MyRequests");
            }

            return View(foodRequest);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var foodRequest = this.foodRequestService.GetById((int)id);

            if (foodRequest == null)
            {
                return HttpNotFound();
            }

            var model = Mapper.Map<FoodRequest, FoodRequestViewModel>(foodRequest);

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var foodRequest = this.foodRequestService.GetById(id);
            this.foodRequestService.Delete(foodRequest);

            return RedirectToAction("MyRequests");
        }
    }
}
