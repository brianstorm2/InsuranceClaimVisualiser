using System;
using Microsoft.Data.SqlClient;

namespace LossRatioCalculator
{
    class Program
    {
        public static void Main(string[] args)
        {
            //use system environment variables for the program to retrieve username and password
            string username = Environment.GetEnvironmentVariable("SQL_SERVER_USERNAME");
            string password = Environment.GetEnvironmentVariable("SQL_SERVER_PASSWORD");

            //connect to local sql server db PolicyClaimsTrackerDB
            string connectToLocalSqlServer = $"Server=localhost,1433;Database=PolicyClaimsTrackerDB; User Id={username}; Password={password};TrustServerCertificate=True";
            using (SqlConnection connection = new SqlConnection(connectToLocalSqlServer))
            {
                connection.Open();
                Console.WriteLine("Connection Opened");

                //run ExtractClaimsByGeographyMethod
                ExtractClaimsByGeography(connection);

                connection.Close();
            }

        }

        public static void ExtractClaimsByGeography(SqlConnection connection)
        {
            //query to group number of claims by geography
            SqlCommand command = new SqlCommand("SELECT SUM(c.ClaimAmount) AS TotalClaims, r.RegionName FROM Claims c LEFT JOIN Policies p ON c.PolicyID = p.PolicyID LEFT JOIN Regions r ON r.RegionID = p.RegionID GROUP BY r.RegionName", connection);
            Dictionary<string, decimal> claimsByRegion = new Dictionary<string, decimal>();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                //iterates through columns (0) = Column1
                while (reader.Read())
                {
                    decimal totalClaims = reader.GetDecimal(0);
                    string regionName = reader.GetString(1);

                    //store results by geography claim in dictionary
                    //move to seperate function
                    claimsByRegion.Add(regionName, totalClaims);
                    /*DateTime ClaimDate = reader.GetDateTime(3);
                    Console.WriteLine($"ClaimID: {ClaimID}, PolicyID: {PolicyID}, Claim Amount: {ClaimAmount}, Claim Date: {ClaimDate.ToShortDateString()}");*/
                }
                foreach (KeyValuePair<string, decimal> kvp in claimsByRegion)
                {
                    Console.WriteLine($"Region {kvp.Key}, Total Claims: {kvp.Value}");
                }
                //Generate Bar Chart
                DataGraphProducer.DataVisualiser visualiseByGeography = new DataGraphProducer.DataVisualiser();
                visualiseByGeography.GenerateClaimsBarChartByGeography(claimsByRegion);
            }
        }
    }
}
