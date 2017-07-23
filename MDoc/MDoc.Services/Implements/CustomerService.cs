﻿using System;
using System.Linq;
using MDoc.Entities;
using MDoc.Repositories.Contract;
using MDoc.Services.Contract.DataContracts;
using MDoc.Services.Contract.Interfaces;

namespace MDoc.Services.Implements
{
    public class CustomerService : BaseService, ICustomerService
    {
        #region [Constructor]

        public CustomerService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        #endregion

        #region [Variable]

        #endregion

        #region [Implements]

        public IQueryable<CustomerModel> ListOfCustomers()
            => UnitOfWork.GetRepository<Customer>().Get(m => !m.IsDeleted)
                .Join(UnitOfWork.GetRepository<Gender>().Get(), customer => customer.GenderId, gender => gender.GenderId,
                    (customer, gender) => new {customer, gender})
                .Select(x => new CustomerModel
                {
                    CustomerId = x.customer.CustomerId,
                    FirstName = x.customer.FirstName,
                    LastName = x.customer.LastName,
                    Address = x.customer.Address,
                    DOB = x.customer.DOB,
                    Email = x.customer.Email,
                    Mobile = x.customer.Mobile,
                    GenderId = x.customer.GenderId,
                    Gender = x.gender.Label
                });

        public CustomerModel Detail(int customerId)
        {
            var customer = UnitOfWork.GetRepository<Customer>().GetByKeys(customerId);
            if (customer == null) return new CustomerModel {CustomerId = 0};
            var result = new CustomerModel
            {
                CustomerId = customer.CustomerId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Address = customer.Address,
                CountryId = customer.CountryId,
                DOB = customer.DOB,
                DistrictId = customer.DistrictId,
                Email = customer.Email,
                GenderId = customer.GenderId,
                IdentityCardDateExpired = customer.IdentityCardDareExpired,
                IdentityCardDateValid = customer.IdentityCardDateValid,
                IdentityCardNo = customer.IdentityCardNo,
                IdentityCardPlaceId = customer.IdentityCardPlaceId,
                Mobile = customer.Mobile,
                ProvinceId = customer.ProvinceId,
                WardId = customer.WardId
            };
            return result;
        }

        public bool Create(CustomerModel model)
        {
            var entity = new Customer
            {
                CountryId = model.CountryId,
                ProvinceId = model.ProvinceId,
                DistrictId = model.DistrictId,
                WardId = model.WardId,
                Address = model.Address,
                DOB = model.DOB.Value,
                GenderId = model.GenderId,
                IdentityCardDareExpired = model.IdentityCardDateExpired,
                IdentityCardDateValid = model.IdentityCardDateValid,
                IdentityCardNo = model.IdentityCardNo,
                IdentityCardPlaceId = model.IdentityCardPlaceId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                IsDeleted = false,
                Mobile = model.Mobile,
                CreatedById = model.LoggedUserId,
                CreatedDate = DateTime.Now
            };
            UnitOfWork.GetRepository<Customer>().Create(entity);
            UnitOfWork.SaveChanges();
            return entity.CustomerId > 0;
        }

        public bool Update(CustomerModel model)
        {
            var customer = UnitOfWork.GetRepository<Customer>().GetByKeys(model.CustomerId);
            if (customer == null) return false;
            customer.FirstName = model.FirstName;
            customer.LastName = model.LastName;
            customer.GenderId = model.GenderId;
            customer.DOB = model.DOB.Value;
            customer.Email = model.Email;
            customer.Mobile = model.Mobile;
            customer.GenderId = model.GenderId;
            customer.CountryId = model.CountryId;
            customer.ProvinceId = model.ProvinceId;
            customer.DistrictId = model.DistrictId;
            customer.WardId = model.WardId;
            customer.Address = model.Address;
            customer.UpdatedDate = DateTime.Now;
            customer.UpdatedById = model.LoggedUserId;
            UnitOfWork.SaveChanges();
            return true;
        }

        public bool Remove(CustomerModel model)
        {
            var customer = UnitOfWork.GetRepository<Customer>().GetByKeys(model.CustomerId);
            if(customer == null) return false;
            customer.IsDeleted = true;
            customer.UpdatedById = model.LoggedUserId;
            customer.UpdatedDate = DateTime.Now;
            UnitOfWork.SaveChanges();
            return true;
        }

        #endregion
    }
}