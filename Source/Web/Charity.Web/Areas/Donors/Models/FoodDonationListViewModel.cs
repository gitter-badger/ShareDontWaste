﻿namespace Charity.Web.Areas.Donors.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Charity.Data.Models;
    using Charity.Web.Infrastructure.Mapping;

    public class FoodDonationListViewModel : IMapFrom<FoodDonation>
    {
        public int Id { get; set; }

        public FoodCategory FoodCategory { get; set; }

        public string Quantity { get; set; }

        [Display(Name = "Expiration Date")]
        public DateTime ExpirationDate { get; set; }

        [Display(Name = "Available From")]
        public DateTime AvailableFrom { get; set; }

        [Display(Name = "Available To")]
        public DateTime AvailableTo { get; set; }

        [Display(Name = "Completed")]
        public bool IsCompleted { get; set; }
    }
}