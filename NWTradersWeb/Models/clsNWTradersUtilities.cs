using System;
using System.Collections.Generic;
using System.Linq;

namespace NWTradersWeb.Models
{
    public class NWTradersUtilities
    {

        private static NorthwindEntities nwEntities = new NorthwindEntities();

        public static List<string> AllCountries()
        {
            List<string> allCountries = new List<string>();
            allCountries =
                nwEntities.
                Customers.
                Select(c => c.Country).
                Distinct().
                ToList();

            return allCountries;
        }

        public static List<string> AllCompanyNames()
        {
            return nwEntities.
                Customers.
                OrderBy(c => c.CompanyName).
                Select(c => c.CompanyName).
                Distinct().
                ToList();
        }

        public static List<String> AllTitles()
        {
            List<String> allTitles = new List<string>();

            // From the customers table, 
            // where the contact title is not null or empty, 
            // select every contact title 
            // keep only the distinct ones 
            // then convert it to a list.
            allTitles = nwEntities.Customers.
                Where(c => string.IsNullOrEmpty(c.ContactTitle) == false).
                Select(c => c.ContactTitle).
                Distinct().
                ToList();

            return allTitles;
        }

        /// <summary>
        /// Will generate 3 or specified number of random characters
        /// </summary>
        /// <param name="length"></param>
        public static string GenerateRandomUpperCaseCharacters( int length = 3)
        {
            const string possibleCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string randomString = "";
            int randomLocation;
            char randomCharacter;

            Random random = new Random();

            // run this "length" times.
            for (int i=0; i< length; i++)
            {
                // Get a random number no bigger than the number of possible characters A .. Z
                randomLocation = random.Next(possibleCharacters.Length);

                // Get the character at the random location in possible characters.
                randomCharacter = possibleCharacters.ToCharArray()[randomLocation];

                // Attach the random character to the randomString
                randomString += randomCharacter;

                // repeat "length" times.
            }

            // And return the random string.
            return randomString;
        }

        public static List<string> itemsPerPage
        {
            get { return (new List<string> { "10", "15", "25", "50", "All" }); }
        }

    }
}
