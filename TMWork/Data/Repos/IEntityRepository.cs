﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMWork.Data.Models.Customer;
using TMWork.Data.Models.Invoice;
using TMWork.Data.Models.User;
using TMWork.Data.Models.Team;

namespace TMWork.Data.Repos
{
    public interface IEntityRepository<T>
    {
        IEnumerable<T> GetAll { get; }
        IEnumerable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProeprties);
        T Find(int id);
        void insertOrUpdate(T entity);
        void Delete(int id);
        void Save();
    }

    public interface IUserRepository<T> : IEntityRepository<AuthUser>
    {
    }
    public interface IInvoiceRepository
    {
        IEnumerable<Invoice> GetAll();
        Invoice FindById(int Id);
        Invoice FindByName(string Name);
        void Add(Invoice newCustomer);
        void Update(Invoice newCustomer);
        void Remove(Invoice newCustomer);
        bool SaveAll();
    }
    public interface IContactRepository
    {
        IEnumerable<Contact> GetAll();
        Contact FindById(int Id);
        Contact FindByName(string Name);
        void Add(Contact newContact);
        void Update(Contact newContact);
        void Remove(Contact newContact);
        bool SaveAll();
    }
    public interface IMemberRepository
    {
        IEnumerable<Member> GetAll();
        Member FindById(int Id);
        Member FindByName(string Name);
        void Add(Member newMember);
        void Update(Member newMember);
        void Remove(Member newMember);
        bool SaveAll();
    }
    public interface IMissionRepository
    {
        IEnumerable<Mission> GetAll();
        Mission FindById(int Id);
        Mission FindByName(string Name);
        void Add(Mission newMission);
        void Update(Mission newMisison);
        void Remove(Mission newMission);
        bool SaveAll();
    }

    public interface ICustomerRepository 
    {
        IEnumerable<Customer> GetAll();
        IEnumerable<Customer> GetAllWithApplianceProblems();
        Customer FindById(int Id);
        Customer FindByName(string Name);
        void Add(Customer newCustomer);
        void Update(Customer newCustomer);
        void Remove(Customer newCustomer);
        bool SaveAll();     
    }

    public interface ICustomerApplianceTypeRepository
    {
        IEnumerable<CustomerApplianceType> GetAll();
        IEnumerable<CustomerApplianceType> GetAllWithBrands();
        CustomerApplianceType FindById(int Id);
        CustomerApplianceType FindByName(string Name);
        void Add(CustomerApplianceType newCustomerApplianceType);
        void Update(CustomerApplianceType newCustomerApplianceType);
        void Remove(CustomerApplianceType newCustomerApplianceType);
        bool SaveAll();
    }

    public interface ICustomerApplianceBrandRepository
    {
        IEnumerable<CustomerApplianceBrand> GetAll();
        CustomerApplianceBrand FindById(int Id);
        CustomerApplianceBrand FindByName(string Name);
        void Add(CustomerApplianceBrand newCustomerApplianceBrand);
        void Update(CustomerApplianceBrand newCustomerApplianceBrand);
        void Remove(CustomerApplianceBrand newCustomerApplianceBrand);
        bool SaveAll();
    }

    public interface ICustomerCouponRepository
    {
        IEnumerable<CustomerCoupon> GetAll();
        IEnumerable<CustomerCoupon> GetAllNonExpired();
        CustomerCoupon FindById(int Id);
        CustomerCoupon FindByName(string Name);
        void Add(CustomerCoupon newCustomerCoupon);
        void Update(CustomerCoupon newCustomerCoupon);
        void Delete(CustomerCoupon newCustomerCoupon);
        bool SaveAll();
    }
}
