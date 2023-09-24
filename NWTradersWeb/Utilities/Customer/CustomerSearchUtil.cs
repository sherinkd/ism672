using NWTradersWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NWTradersWeb.Utilities
{
    public class CustomerSearchUtil
    {
        List<Customer> customers = new List<Customer>();
        public CustomerSearchUtil(
             List<Customer> customers
            ) {
            this.customers = customers;
        }

        public List<Customer> GetCustomers()
        {
            return this.customers;
        }

        public CustomerSearchUtil ByCompanyName(
            string companyName
            ) {
            if (string.IsNullOrEmpty(companyName))
            {
                return this;
            }

            customers = customers
                .Where(c => c.CompanyName.ToUpper().Contains(companyName.ToUpper()))
                .OrderBy(c => c.CompanyName)
                .Select(c => c)
                .ToList();

            return this;
        }

        public CustomerSearchUtil ByCountryName(
            string countryName)
        {
            if (string.IsNullOrEmpty(countryName)) { return this; }

            customers = customers
                        .Where(c => c.Country.Equals(countryName))
                        .OrderBy(c => c.CompanyName)
                        .Select(c => c)
                        .ToList();

            return this;
        }

        public CustomerSearchUtil ByTitle(
            string title)
        {
            if (string.IsNullOrEmpty(title)) {  return this; }

            customers = customers
                .Where(c => c.ContactTitle.ToUpper().Contains(title.ToUpper()))
                .OrderBy(c => c.CompanyName)
                .Select(c => c)
                .ToList();

            return this;
        }

        public CustomerSearchUtil ByContact(
            string contact)
        {
            if (string.IsNullOrEmpty(contact)) { return this; }

            customers = customers
                .Where(c => c.ContactName.ToUpper().Contains(contact.ToUpper()))
                .OrderBy(c => c.CompanyName)
                .Select(c => c)
                .ToList();

            return this;
        }

        public CustomerSearchUtil ByRegion(
            string region
            )
        {
            if (string.IsNullOrEmpty(region)) {  return this; }

            customers = customers
                .Where(c => !string.IsNullOrEmpty(c.Region) && c.Region.Equals(region))
                .OrderBy(c => c.CompanyName)
                .Select(c => c)
                .ToList();

            return this;
        }

        public CustomerSearchUtil ByCity(
            string city)
        {
            if (string.IsNullOrEmpty(city)) { return this; }

            customers = customers
                .Where(c => c.City.Equals(city))
                .OrderBy(c => c.CompanyName)
                .Select(c => c)
                .ToList();

            return this;
        }
    }
}