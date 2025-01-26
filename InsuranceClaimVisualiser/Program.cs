using System;
using System.Drawing;
using System.Security.Claims;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace LossRatioCalculator
{
    class Program
    {
        public static void Main(string[] args)
        {
            //use system environment variables for the program to retrieve username and password
            string username = Environment.GetEnvironmentVariable("SQL_SERVER_USERNAME");
            string password = Environment.GetEnvironmentVariable("SQL_SERVER_PASSWORD");

            string serverAndPort = "localhost,1433";

            //connect to local sql server db PolicyClaimsTrackerDB
            string connectToLocalSqlServer = $"Server={serverAndPort};Database=PolicyClaimsTrackerDB; User Id={username}; Password={password};TrustServerCertificate=True";
            using (SqlConnection connection = new SqlConnection(connectToLocalSqlServer))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Connection Opened");

                    //run ExtractClaimsByGeographyMethod
                    ExtractClaimsByGeography(connection);
                    CalcLossRatioByGeography(connection);
                    CalcClaimsByMonth(connection);

                    //close connection after program is finished
                    connection.Close();
                }
                //SQL exception errors
                catch (SqlException sqlEx)
                {
                    Console.WriteLine($"SQL Connection Error. {sqlEx.Message} Please check your username and password. Closing Program.");
                }
                //Any other errors
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected Error. {ex.Message} An unexpected error occured");
                }
            }

        }

        public static void ExtractClaimsByGeography(SqlConnection connection)
        {
            //query to group number of claims by geography
            SqlCommand command = new SqlCommand(@"SELECT SUM(c.ClaimAmount) AS TotalClaims, r.RegionName 
                                                FROM Claims c 
                                                LEFT JOIN Policies p 
                                                ON c.PolicyID = p.PolicyID 
                                                LEFT JOIN Regions r ON r.RegionID = p.RegionID GROUP BY r.RegionName", connection);

            Dictionary<string, decimal> claimsByRegion = new Dictionary<string, decimal>();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                //iterates through columns (0) = Column1
                while (reader.Read())
                {
                    decimal totalClaims = reader.GetDecimal(0);
                    string regionName = reader.GetString(1);

                    //store results by geography claim in dictionary
                    claimsByRegion.Add(regionName, totalClaims);
                    /*DateTime ClaimDate = reader.GetDateTime(3);
                    Console.WriteLine($"ClaimID: {ClaimID}, PolicyID: {PolicyID}, Claim Amount: {ClaimAmount}, Claim Date: {ClaimDate.ToShortDateString()}");*/
                }

                //Generate Bar Chart
                DataGraphProducer.DataVisualiser visualiseByGeography = new DataGraphProducer.DataVisualiser();
                visualiseByGeography.GenerateClaimsBarChartByGeography(claimsByRegion);
            }
        }

        //query calculating loss ratio by region
        public static void CalcLossRatioByGeography(SqlConnection connection)
        {
            //query to calculate loss ratio by region
            SqlCommand command = new SqlCommand(@"SELECT (SUM(c.ClaimAmount)/SUM(p.PremiumCollected))*100 AS LossRatio, r.RegionName 
                                                FROM Claims c 
                                                LEFT JOIN Policies p 
                                                ON c.PolicyID = p.PolicyID 
                                                LEFT JOIN Regions r 
                                                ON r.RegionID = p.RegionID GROUP BY r.RegionName", connection);

            Dictionary<string, decimal> lossRatioByRegion = new Dictionary<string, decimal>();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    decimal lossRatio = reader.GetDecimal(0);
                    string regionName = reader.GetString(1);

                    //store lossratio and region in dictionary
                    lossRatioByRegion.Add(regionName, lossRatio);
                }

                //Generate Bar Chart
                DataGraphProducer.DataVisualiser lossRatioByGeography = new DataGraphProducer.DataVisualiser();
                lossRatioByGeography.GenerateLossRatioBarChartByGeography(lossRatioByRegion);
            }
        }

        public static void CalcClaimsByMonth(SqlConnection connection)
        {

            //query to calculate money spent on claims per month
            SqlCommand command = new SqlCommand(@"SELECT FORMAT(ClaimDate, 'yyyy-MM') AS ClaimMonth,
                                                SUM(ClaimAmount) AS MonthlyClaims
                                                FROM CLAIMS
                                                GROUP BY FORMAT(ClaimDate, 'yyyy-MM')
                                                ORDER BY ClaimMonth", connection);

            Dictionary<string, decimal> claimsPerMonth = new Dictionary<string, decimal>();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string claimMonth = reader.GetString(0);
                    decimal monthlyClaims = reader.GetDecimal(1);
                    

                    //store monthly claim sum and month
                    claimsPerMonth.Add(claimMonth, monthlyClaims);
                }

                //Generate Line Graph
                DataGraphProducer.DataVisualiser claimsMonthly = new DataGraphProducer.DataVisualiser();
                claimsMonthly.GenerateMonthlyClaimsLineGraph(claimsPerMonth);
            }
        }
    }
}
