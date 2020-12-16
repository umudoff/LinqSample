// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)
//
//Copyright (C) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using SampleSupport;
using Task.Data;

// Version Mad01

namespace SampleQueries
{
    [Title("LINQ Module")]
    [Prefix("Linq")]
    public class LinqSamples : SampleHarness
    {

        private DataSource dataSource = new DataSource();

        [Category("Restriction Operators")]
        [Title("Where - Task 1")]
        [Description("This sample uses the where clause to find all elements of an array with a value less than 5.")]
        public void Linq1()
        {
            int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

            var lowNums =
                from num in numbers
                where num < 5
                select num;

            Console.WriteLine("Numbers < 5:");
            foreach (var x in lowNums)
            {
                Console.WriteLine(x);
            }
        }

        [Category("Restriction Operators")]
        [Title("Where - Task 2")]
        [Description("This sample return return all presented in market products")]

        public void Linq2()
        {
            var products =
                from p in dataSource.Products
                where p.UnitsInStock > 0
                select p;

            foreach (var p in products)
            {
                ObjectDumper.Write(p);
            }
        }


        [Category("LINQ Samples")]
        [Title("Where - Task 001")]
        [Description("This sample return return all customers whose order sum exceed X value ")]
        public void Linq001()
        {
            var x = 100000;
            var customers =
            (from c in dataSource.Customers
             where c.Orders.Sum(o => o.Total) > x
             select new { c.CustomerID, o = c.Orders.Sum(o => o.Total) });


            foreach (var p in customers)
            {
                ObjectDumper.Write(p);
            }
        }


        [Category("LINQ Samples")]
        [Title("Where - Task 001_MethodBased")]
        [Description("This sample return return all customers whose order sum exceed X value ")]
        public void Linq_MethodBased_001()
        {
            var x = 5000;
            var res =
                  dataSource.Customers.Where(c => c.Orders.Sum(o => o.Total) > x).
                  Select(c => new { CustId = c.CustomerID, SumCust = c.Orders.Sum(o => o.Total) });


            foreach (var p in res)
            {
                ObjectDumper.Write(p);
            }


        }



        [Category("LINQ Samples")]
        [Title("Where - Task 002")]
        [Description("This sample return all customers and suppliers in same country and city ")]
        public void Linq002()
        {
            var res =
           (from c in dataSource.Customers
            join s in dataSource.Suppliers
            on new { c.Country, c.City } equals new { s.Country, s.City }
            select new
            {
                c.CustomerID,
                c.Country,
                c.City,
                s.SupplierName,
                SupplierCounty = s.Country,
                SupplierCity = s.City
            });





            foreach (var p in res)
            {
                ObjectDumper.Write(p);
            }

            ObjectDumper.Write("Grouped Version\n");


            var result2 =
          (from c in dataSource.Customers
           join s in dataSource.Suppliers
           on new { c.Country, c.City } equals new { s.Country, s.City }
           group c by new { c.Country, c.City } into grouping
           select new
           {
               grouping.Key.Country,
               grouping.Key.City,
               gr = grouping
           });

            foreach (var p in result2)
            {
                ObjectDumper.Write(p + "  " + p.gr.Select(g => g.CompanyName));
            }




        }



        [Category("LINQ Samples")]
        [Title("Where - Task 002_MethodBased")]
        [Description("This sample return all customers and suppliers in same country and city ")]
        public void Linq_MethodBased_002()
        {
            var result = dataSource.Customers.Join(dataSource.Suppliers,
                                                 c => new { c.Country, c.City },
                                                 s => new { s.Country, s.City },
                                                 (c, s) => new { Customer = c, Supplier = s });

            foreach (var p in result)
            {
                ObjectDumper.Write(p);
            }

            ObjectDumper.Write("Grouped Version\n");

            var result2 = dataSource.Customers
               .GroupJoin(dataSource.Suppliers,
               c => new { c.City, c.Country },
               s => new { s.City, s.Country },
               (c, s) => new { Customer = c, Suppliers = s });
            foreach (var p in result2)
            {
                ObjectDumper.Write(p);
            }

        }
        [Category("LINQ Samples")]
        [Title("Where - Task 003")]
        [Description("This sample return all customers who have  order exceed X value ")]
        public void Linq003()
        {
            var x = 1000;


            var res =
          (from c in dataSource.Customers
           where c.Orders.Any(o => o.Total > x)
           select new
           {
               c.CustomerID,
               c.City,
               c.Country
           });





            foreach (var p in res)
            {
                ObjectDumper.Write(p);
            }
        }



        [Category("LINQ Samples")]
        [Title("Where - Task 003_MethodBased")]
        [Description("This sample return all customers whose have orders exceed X value ")]
        public void Linq_MethodBased_003()
        {
            var x = 1000;

            var res = dataSource.Customers.Where(c => c.Orders.Any(o => o.Total > x))
                                          .Select(c => new
                                          {
                                              c.CustomerID,
                                              c.City,
                                              c.Country
                                          });


            foreach (var p in res)
            {
                ObjectDumper.Write(p);
            }
        }



        [Category("LINQ Samples")]
        [Title("Where - Task 004")]
        [Description("This sample return all customers with their first order date (month and date) ")]
        public void Linq004()
        {
            var res = (from c in dataSource.Customers

                       select new
                       {
                           cust = c.CustomerID,
                           FirstOrder = (c.Orders.Length == 0) ? DateTime.MinValue : c.Orders.Min(o => o.OrderDate)
                       }
                        );

            foreach (var p in res)
            {
                ObjectDumper.Write(p);
            }
        }


        [Category("LINQ Samples")]
        [Title("Where - Task 004_MethodBased")]
        [Description("This sample return all customers with their first order date (month and date) ")]
        public void Linq_MethodBased_004()
        {
            var res = dataSource.Customers.Select(c => new
            {
                cust = c.CustomerID,
                FirstOrder = (c.Orders.Length == 0) ? DateTime.MinValue : c.Orders.Min(o => o.OrderDate)
            });
            foreach (var p in res)
            {
                ObjectDumper.Write(p);
            }
        }


        [Category("LINQ Samples")]
        [Title("Where - Task 005")]
        [Description("This sample return all customers with their first order date sort by year month and total spending max to min ")]
        public void Linq_005()
        {
            var res = from s in (from c in dataSource.Customers
                       select new
                       {
                           cust = c.CustomerID,
                           compName=c.CompanyName,
                           FirstOrder = (c.Orders.Length == 0) ? DateTime.MinValue : c.Orders.Min(o => o.OrderDate),
                           SumOrder =c.Orders.Sum(o=> o.Total)
                            
                       }
                        )
                       orderby s.SumOrder descending ,s.compName, s.FirstOrder.Year, s.FirstOrder.Month 
                      select new { name= s.compName, 
                                   year=s.FirstOrder.Year,
                                   month=s.FirstOrder.Month, 
                                   totalOrders= s.SumOrder};

            foreach (var p in res)
            {
                ObjectDumper.Write(p);

            }
        }



        [Category("LINQ Samples")]
        [Title("Where - Task 005_MethodBase")]
        [Description("This sample return all customers with their first order date sort by year month and total spending max to min ")]
        public void Linq_005_MethodBased()
        {
            var res = dataSource.Customers.Select(c => new
            {
                compName=c.CompanyName,
                FirstOrder = (c.Orders.Length == 0) ? DateTime.MinValue : c.Orders.Min(o => o.OrderDate),
                SumOrder =c.Orders.Sum(o=> o.Total)
            }).OrderBy(c=> c.compName).ThenBy(c=> c.FirstOrder.Year)
              .ThenBy(c=> c.FirstOrder.Month).ThenByDescending(c=>c.SumOrder);
            foreach (var p in res)
            {
                ObjectDumper.Write(p);
            }
        }



    }



}
