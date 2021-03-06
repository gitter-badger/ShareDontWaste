﻿namespace Charity.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Charity.Data.Common.Repositories;
    using Charity.Data.Models;
    using Charity.Services.Common;

    public class RecipientTypeService : IRecipientTypeService
    {
        private readonly IRecipientTypeRepository recipientTypeRepository;

        public RecipientTypeService(IRecipientTypeRepository recipientTypeRepository)
        {
            this.recipientTypeRepository = recipientTypeRepository;
        }

        public IEnumerable<RecipientType> GetAll()
        {
            return this.recipientTypeRepository.All().OrderBy(r => r.Name).AsEnumerable();
        }
    }
}
