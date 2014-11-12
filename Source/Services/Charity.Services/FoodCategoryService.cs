﻿namespace Charity.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Charity.Data.Common.Repositories;
    using Charity.Data.Models;

    public class FoodCategoryService
    {
        private readonly IFoodCategoryRepository foodCategoryRepository;

        public FoodCategoryService(IFoodCategoryRepository foodCategoryRepository)
        {
            this.foodCategoryRepository = foodCategoryRepository;
        }

        public IEnumerable<FoodCategory> GetAll()
        {
            return this.foodCategoryRepository.All().OrderBy(c => c.Id).AsEnumerable();
        }
    }
}
