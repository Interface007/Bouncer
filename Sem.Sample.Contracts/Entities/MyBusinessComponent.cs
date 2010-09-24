// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MyBusinessComponent.cs" company="Sven Erik Matzen">
//   Copyright (c) Sven Erik Matzen. GNU Library General Public License (LGPL) Version 2.1.
// </copyright>
// <summary>
//   Defines the MyBusinessComponent type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Sample.Contracts.Entities
{
    using System;

    /// <summary>
    /// This business component does not check for any valid data,
    /// so it will fail in some cases and throw exceptions.
    /// </summary>
    internal class MyBusinessComponent
    {
        /// <summary>
        /// This method does already know, that calling GetTheName will fail,
        /// but it does not check for customer to have a name set.
        /// </summary>
        /// <param name="customer"></param>
        internal void WriteCustomerProperties(MyCustomer customer)
        {
            Console.WriteLine(
                @"calling customer {0} with Id {1}", 
                GetTheName(customer), 
                FormatTheId(customer));
        }

        /// <summary>
        /// This is called from CallCustomer - both business components "MyBusinessComponent" 
        /// and "MySaveBusinessComponent" do call this method.
        /// </summary>
        /// <param name="customer">the customer to get the id from</param>
        /// <returns>a formatted id</returns>
        protected static string FormatTheId(MyCustomer customer)
        {
            return ">>" + customer.InternalId.ToString().Replace("-", string.Empty) + "<<";
        }

        /// <summary>
        /// This is called from CallCustomer - both business components "MyBusinessComponent" 
        /// and "MySaveBusinessComponent" do call this method.
        /// </summary>
        /// <param name="customer">the customer to get the name from</param>
        /// <returns>the string-processed name property</returns>
        protected static string GetTheName(MyCustomer customer)
        {
            return customer.FullName.Replace("-", " ");
        }
    }
}
