using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GabeUtilityLibrary
{
    /// <summary>
    /// Code by Gabriel Atienza-Norris, Last Edited 12/04/2022*******
    /// Glossary ****************************************************
    /// Naming Conventions ******************************************
    /// Prefix - Input Type
    /// Suffix Modifiers (When Always Written in the Following Order)
    /// R - Required
    /// C - Non-Numerics (char values only)
    /// N - Numeric (Numeric Values Only)
    /// E - Exact Length
    /// I - Integer Parse
    /// L - Long Integer (Int64)
    /// Do - Double
    /// De - Decimal
    /// Method Overloads ********************************************
    /// First Argument - Input
    /// Second Argument - Data Field
    /// Third Argument (Overload) - (string) Min Length / (numeric) Min Value / ** Used with E Suffix ** (string) Exact Length / (numeric) Exact Value 
    /// Fourth Argument (Overload) - (string) Max Length / (numeric) Max Value
    /// Special Cases Overloads *************************************
    /// First Argument - Return Type
    /// Second Argument - Min Length
    /// Third Argument - Max Length
    /// NOTE ********************************************************
    /// Please always use library methods in Try/Catch blocks
    /// </summary>
    public static class InputValidation 
    {
        //Generic String Validation Methods ***********************************************************************************
        public static string Required(string input, string field) 
            => input.Length != 0 ? input : throw new Exception($"{field} is required.");

        public static string MinLength(string input, string field, int minLength) =>
            input.Length >= minLength ? input : throw new Exception($"{field} must have a minimum of {minLength} characters.");

        public static string MaxLength(string input, string field, int maxLength) =>
            input.Length <= maxLength ? input : throw new Exception($"{field} must have a minimum of {maxLength} characters.");

        public static string ExactLength(string input, string field, int exactLength) =>
            input.Length == exactLength ? input : throw new Exception($"{field} must have exactly {exactLength} characters.");

        public static string AlphabeticalOnly(string input, string field) =>
            input.All(Char.IsLetter) ? input : throw new Exception($"{field} must contain only letters.");

        public static string AlphaNumericOnly(string input, string field) =>
            input.All(Char.IsLetterOrDigit) ? input : throw new Exception($"{field} must only contain letters and numbers.");

        //Generic Numeric Validation Methods *********************************************************************************
        public static int TryInteger(string input, string field) => input == "" ? 0 :
            int.TryParse(input, out int parsedInput) ? parsedInput :
            throw new Exception($"{field} must only contain numbers with no decimal places or special characters.");

        public static Int64 TryInteger64(string input, string field) => input == "" ? 0 :
            Int64.TryParse(input, out Int64 parsedInput) ? parsedInput :
            throw new Exception($"{field} must only contain numbers with no decimal places or special characters.");

        public static double TryDouble(string input, string field) => input == "" ? 0 :
            double.TryParse(input, out double parsedInput) ? parsedInput :
            throw new Exception($"{field} must only contain numbers (may include decimal points).");

        public static decimal TryDecimal(string input, string field) => input == "" ? 0 :
            decimal.TryParse(input, out decimal parsedInput) ? parsedInput :
            throw new Exception($"{field} must only contain numbers (may include decimal points).");

        //Special Cases ******************************************************************************************************

        //Canadian Postal Code
        public static string CanadianPostalCode(string input) =>  new Regex("^[A-CEGHJ-NPR-TVXY][0-9][A-CEGHJ-NPR-TV-Z][ -][0-9][A-CEGHJ-NPR-TV-Z][0-9]$").IsMatch(input.ToUpper())
                ? input.ToUpper() : String.IsNullOrEmpty(input) ? "" : throw new Exception("Invalid postal code. Use format 'A1B-2C3' or 'A1B 2C3'");

        public static string CanadianPostalCodeNoGap(string input) => new Regex("^[A-CEGHJ-NPR-TVXY][0-9][A-CEGHJ-NPR-TV-Z][0-9][A-CEGHJ-NPR-TV-Z][0-9]$").IsMatch(input.ToUpper())
        ? input.ToUpper() : String.IsNullOrEmpty(input) ? "" : throw new Exception("Invalid postal code. Use format 'A1B2C3'.");

        public static string CanadianPostalCodeR(string input) => Required(CanadianPostalCode(input), "Postal code");

        public static string CanadianPostalCodeNoGapR(string input) => Required(CanadianPostalCodeNoGap(input), "Postal code");

        //Canadian Street Address (Basic)
        public static string CanadianStreetAddress(string input) => input == "" ? input :
            !new Regex("[!@#$%^&*()_]").IsMatch(input) ? input : throw new Exception("Street address must not include special characters other than '-'.");

        public static string CanadianStreetAddressR(string input) => Required(CanadianStreetAddress(input), "Street address");

        //Canadian City (Basic)
        public static string CanadianCity(string input) => input == "" ? input :
            !new Regex("[!@#$%^&*()_0-9]").IsMatch(input) ? input : throw new Exception("City name must not include special characters other than '-'.");

        public static string CanadianCityR(string input) => Required(CanadianCity(input), "City name");

        public static string CanadianCityR(string input, int minLength) => 
            MinLength(Required(CanadianCity(input), "City name"), "City name", minLength);

        public static string CanadianCityR(string input, int minLength, int maxLength) =>
            MaxLength(MinLength(Required(CanadianCity(input), "City name"), "City name", minLength), "City name", maxLength);

        //Email Validation
        public static string Email(string input) => new Regex("^[^@]{1,64}@.{1,250}[.][a-zA-Z]{1,4}$").IsMatch(input)
                ? input : String.IsNullOrEmpty(input) ? "" : throw new Exception("Invalid email entry.");

        public static string EmailR(string input) => Required(Email(input), "Email");

        public static string EmailR(string input, int minLength) => MinLength(Required(EmailR(input), "Email"), "Email", minLength);

        public static string EmailR(string input, int minLength, int maxLength) => 
            MaxLength(MinLength(Required(EmailR(input), "Email"), "Email", minLength), "Email", maxLength);

        //Compound Validation Methods ****************************************************************************************
        //Method Set For String Input / Required 
        public static string StringR(string input, string field) => Required(input, field);

        public static string StringRE(string input, string field, int exactLength) =>
            ExactLength(Required(input, field), field, exactLength);

        public static string StringRNIE(string input, string field, int exactLength) =>
            TryInteger(ExactLength(Required(input, field), field, exactLength), field).ToString();

        public static string StringRNLE(string input, string field, int exactLength) =>
            TryInteger64(ExactLength(Required(input, field), field, exactLength), field).ToString();

        public static string StringR(string input, string field, int minLength) => MinLength(Required(input, field), field, minLength);

        public static string StringR(string input, string field, int minLength, int maxLength) => 
            MaxLength(MinLength(Required(input, field), field, minLength), field, maxLength);

        //Method Set For String Input / Required / Characters Only
        public static string StringRC(string input, string field) => AlphabeticalOnly(Required(input, field), field);

        public static string StringRCE(string input, string field, int exactLength) =>
            AlphabeticalOnly(ExactLength(Required(input, field), field, exactLength), field);

        public static string StringRC(string input, string field, int minLength) => 
            AlphabeticalOnly(MinLength(Required(input, field), field, minLength), field);

        public static string StringRC(string input, string field, int minLength, int maxLength) =>
            AlphabeticalOnly(MaxLength(MinLength(Required(input, field), field, minLength), field, maxLength), field);

        //Method Set For Integer Input / Required
        public static int IntR(string input, string field) => TryInteger(Required(input, field), field);

        //Method Set For Double Input / Required
        public static double DoubleR(string input, string field) => TryDouble(Required(input, field), field);

        //Method Set For Decimal Input / Required
        public static decimal DecimalR(string input, string field) => TryDecimal(Required(input, field), field);
    }
}
