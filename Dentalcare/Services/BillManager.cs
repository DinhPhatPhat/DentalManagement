using Dentalcare.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dentalcare.Services
{

    public class BillManager
    {
        private readonly clinicEntities db;
        public BillManager()
        {
            db = new clinicEntities();
        }
        public List<Bill> getBills() { 
            return db.Bills
                .Where(f => f.hide == false)
                     .OrderBy(f => f.new_order)
                     .ToList();
        }
        public List<Bill> getBillsIsPayed()
        {
            return db.Bills
                .Where(f => f.hide == false && f.isPayed)
                     .OrderBy(f => f.new_order)
                     .ToList();
        }
        public int getAverageRevenueMonths()
        {
            // Get the clinic start date (assuming the first clinic in the db is used)
            var dayStart = db.Clinics.FirstOrDefault()?.dateStartClinic;

            // If the clinic start date is null, return 0
            if (dayStart == null)
            {
                return 0;
            }

            // Calculate the total revenue and the number of months since the clinic started
            decimal totalRevenue = 0;
            int monthsPassed = 0;

            // Get the current date and calculate the number of months
            DateTime currentDate = DateTime.Now;
            monthsPassed = (currentDate.Year - dayStart.Value.Year) * 12 + currentDate.Month - dayStart.Value.Month;

            // If the clinic started in the future (shouldn't happen), return 0
            if (monthsPassed <= 0)
            {
                return 0;
            }

            // Iterate over the bills and add the totalPrice of paid bills
            foreach (Bill bill in getBillsIsPayed())
            {
                totalRevenue += bill.totalPrice;
            }

            // Calculate and return the average revenue per month
            return (int)(totalRevenue / monthsPassed);
        }

        public int getAverageRevenueYears()
        {
            // Get the clinic start date (assuming the first clinic in the db is used)
            var dayStart = db.Clinics.FirstOrDefault()?.dateStartClinic;

            // If the clinic start date is null, return 0
            if (dayStart == null)
            {
                return 0;
            }

            // Calculate the total revenue and the number of years since the clinic started
            decimal totalRevenue = 0;
            int yearsPassed = 0;

            // Get the current date and calculate the number of years
            DateTime currentDate = DateTime.Now;
            yearsPassed = currentDate.Year - dayStart.Value.Year;

            // If the clinic started in the future (shouldn't happen), return 0
            if (yearsPassed <= 0)
            {
                return 0;
            }

            // Iterate over the bills and add the totalPrice of paid bills
            foreach (Bill bill in getBillsIsPayed())
            {
                totalRevenue += bill.totalPrice;
            }

            // Calculate and return the average revenue per year
            return (int)(totalRevenue / yearsPassed);
        }

        // Example method to get the list of available years based on your business logic
        public List<string> GetAvailableYears()
        {
            int currentYear = DateTime.Now.Year;
            List<string> years = new List<string>();

            // Generate the last 4 years
            for (int i = 0; i < 4; i++)
            {
                years.Add((currentYear - i).ToString());
            }

            return years;
        }

        // Method to get revenue data for the year
        public ChartData GetYearlyRevenueData()
        {
            // Logic to retrieve yearly revenue data
            var data = new ChartData
            {
                Labels = GetAvailableYears(),
                Data = new List<int> { 20000, 22000, 25000, 30000 }
            };
            return data;
        }

        // Method to get revenue data for specific quarters of a year
        public ChartData GetQuarterlyRevenueData(int year)
        {
            var data = new ChartData
            {
                Labels = new List<string> { "Q1", "Q2", "Q3", "Q4" },
                Data = new List<int> { 5000, 6000, 7000, 8000 }
            };
            return data;
        }

        // Method to get revenue data for specific months of a year
        public ChartData GetMonthlyRevenueData(int year)
        {
            // Example data for monthly revenue
            var data = new ChartData
            {
                Labels = new List<string> { "T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8", "T9", "T10", "T11", "T12" },
                Data = new List<int> { 1500, 1600, 1700, 1800, 1900, 2000, 2100, 2200, 2300, 2400, 2500, 2600 }
            };
            return data;
        }

        public class ChartData
        {
            public List<string> Labels { get; set; }
            public List<int> Data { get; set; }
        }
    }
}