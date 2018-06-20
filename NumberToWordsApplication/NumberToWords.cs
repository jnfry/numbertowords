using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NumberToWordsApplication
{
    /// <summary>
    /// A class containing tools to turn numbers into words.
    /// Interprets numbers as dollar amounts.
    /// </summary>
    public static class NumberToWords
    {
        // 15 digits max. in a "trillions" number.
        const int DOLLARS_MAX_LEN = 15;
        // Don't allow over 99 cents.
        const int CENTS_MAX_LEN = 2;

        // What might appear in the "ones" digit. Also used for the hundreds.
        static readonly string[] ones = { "", "ONE", "TWO", "THREE", "FOUR", "FIVE",
            "SIX", "SEVEN", "EIGHT", "NINE" };

        // What might appear when the "tens" digit is a 1.
        static readonly string[] teens = { "TEN", "ELEVEN", "TWELVE", "THIRTEEN",
            "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN" };

        // What might appear when the "tens" digit is not a 1.
        static readonly string[] tens = { "", "TEN", "TWENTY", "THIRTY", "FORTY",
            "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY" };

        // https://en.wikipedia.org/wiki/Names_of_large_numbers
        // Names of large powers of 10, or "suffixes" to each segment.
        // Leave hundred empty, as it is added at each segment independently.
        static readonly string[] suffixes = { "", " THOUSAND", " MILLION", " BILLION",
            " TRILLION" };


        /// <summary>
        /// Generates the word representation of a number.
        /// </summary>
        /// <param name="number">String of digits, separators, and decimals.</param>
        /// <returns>Word representation of number.</returns>
        public static string Wordify(string number)
        {
            if (number == null)
            {
                throw new ArgumentException("Invalid input: input cannot be null.");
            }

            // Split into dollars and cents.
            List<string> parts = number.Split('.').ToList();
            if (parts.Count > 2)
            {
                throw new ArgumentException("Invalid input: input must contain " +
                    "one or less decimals.");
            }

            // Remove separators that may appear in larger dollar numbers.
            string dollarPart = parts[0].Replace(" ", "").Replace(",", "");
            if (dollarPart.Length > DOLLARS_MAX_LEN)
            {
                throw new ArgumentException("Invalid input: dollar part must be less than " +
                    DOLLARS_MAX_LEN + " digits long.");

            }

            string words = WordifyDollars(dollarPart);

            // Check if there is a cent part.
            if (parts.Count > 1)
            {
                string centsPart = parts[1];
                if (centsPart.Length > CENTS_MAX_LEN)
                {
                    throw new ArgumentException("Invalid input: cents part must be " +
                        "less than " + CENTS_MAX_LEN + " digits long.");
                }
                words += WordifyCents(centsPart);
            }


            return words;
        }

        /// <summary>
        /// Generates the word representation of the dollar part of a dollar amount.
        /// </summary>
        /// <param name="dollars">A string of max. 15 digits.</param>
        /// <returns>String containing word representation of dollars.</returns>
        private static string WordifyDollars(string dollars)
        {
            // GenerateSegments returns segments of 3 digits, in reverse order.
            List<List<int>> dollarSegments = GenerateSegements(new List<char>(dollars));

            // Working on reversed list is more intuitive than decrementing.
            dollarSegments.Reverse();

            // Iterate over each segment, prepending its words to the existing words.
            string words = "";
            for (int i = 0; i < dollarSegments.Count(); i++)
            {
                List<int> segment = dollarSegments[i];
                string segmentWords = WordifySegment(segment);

                // No words when segment is 000, 00, or 0.
                if (segmentWords.Length == 0)
                {
                    continue;
                }

                // Ensure correct "DOLLAR" or "DOLLARS".
                // Added at first non-zero segment.
                if (words == "")
                {
                    // If this is the only segment, and it is "1".
                    if (i == 0 && i + 1 == dollarSegments.Count()
                        && segmentWords == "ONE")
                    {
                        words = "DOLLAR";
                    }
                    else
                    {
                        words = "DOLLARS";
                    }
                }

                // If no segments have been prepended yet.
                if (words == "DOLLAR" || words == "DOLLARS")
                {
                    // Exclude any commas.
                    words = String.Format("{0}{1} {2}",
                        segmentWords, suffixes[i], words);
                }
                else
                {
                    // Comma separate if some other words already added.
                    words = String.Format("{0}{1}, {2}",
                        segmentWords, suffixes[i], words);
                }
            }

            // Edge case where only segment was 0, 00, or 000.
            if (words == "")
            {
                words = "ZERO DOLLARS";
            }
            return words;

        }

        /// <summary>
        /// Generates the word representation of the cents part of a dollar amount.
        /// </summary>
        /// <param name="cents">A string of two digits.</param>
        /// <returns>String containing word representation of cents.</returns>
        private static string WordifyCents(string cents)
        {
            // e.g. ".1" should read as "TEN CENTS" and "." as "ZERO CENTS"
            if (cents.Length == 1 || cents.Length == 0) { cents += '0'; }
            // Length is checked above, so there should be only a single segment.
            List<int> centSegment = GenerateSegements(new List<char>(cents))[0];

            // Wordify the cent segment
            string words = WordifySegment(centSegment);

            // WordifySegment is used for dollars as well, which often
            // doesn't want to say ZERO when there are zeros. Handle that here.
            if (words.Length == 0) { words = "ZERO"; }

            // Ensure proper grammar.
            // String concatenation is easier to read than formatter here.
            if (words == "ONE") { words += " CENT"; }
            else { words += " CENTS"; }

            return " AND " + words;

        }

        /// <summary>
        /// Generates the words for a segment of 1 to 3 digits.
        /// </summary>
        /// <param name="segment">List of numbers to wordify.</param>
        /// <returns>Word representation of the segment.</returns>
        private static string WordifySegment(List<int> segment)
        {
            // Reverse the segment as we want to start with the "ones" digit.
            segment.Reverse();

            string words = "";

            int segLength = segment.Count();
            for (int i = 0; i < segLength; i++)
            {
                int num = segment[i];
                // No words to generate if number is 0.
                if (num == 0) { continue; }

                switch (i)
                {
                    // The "ones" digit
                    case 0:
                        // Check if current and next num classify as a "Teen".
                        if (i + 1 < segLength && segment[i + 1] == 1)
                        {
                            // Using continue for readability, the next iteration
                            // will handle the resulting "Teen".
                            continue;
                        }
                        else
                        {
                            words = ones[num];
                        }
                        break;

                    // The "tens" digit
                    case 1:
                        if (num == 1)
                        {
                            // This is a "teen", so no need to concatenate with 
                            // the remainder of the result, that would usually 
                            // be the "ones" digit.
                            words = teens[segment[i - 1]];
                        }
                        else if (segment[i - 1] == 0)
                        {
                            // Nothing will follow, don't include hyphen.
                            // e.g. FOURTY
                            words = tens[num];
                        }
                        else
                        {
                            // e.g. FOURTY-FIVE
                            words = String.Format("{0}-{1}",
                                tens[num], words);
                        }
                        break;

                    // The "hundreds" digit
                    case 2:
                        // Nothing will follow, so don't include "AND" or 
                        // the remainder of result.
                        if (segment[i - 1] == 0 && segment[i - 2] == 0)
                        {
                            words = String.Format("{0} HUNDRED",
                            ones[num]);
                        }
                        else
                        {
                            words = String.Format("{0} HUNDRED AND {1}",
                            ones[num], words);
                        }
                        break;
                }

            }
            // Incase caller wants to use segment again.
            segment.Reverse();
            return words;
        }

        /// <summary>
        /// Splits a list of digit characters into groups of 3 or less ints.
        /// </summary>
        /// <param name="input">List of number characters</param>
        /// <returns>List of segments contining ints of characters.</returns>
        private static List<List<int>> GenerateSegements(List<char> input)
        {
            // We will be doing a reverse traversal of lists, so reverse it 
            // instead, and then use a normal foreach loop.
            // Makes code more intuitive and readable.
            input.Reverse();

            List<List<int>> result = new List<List<int>>();

            // Iterate over each char of the input to create segments of 3.
            // Final segment will be shorter if not divisible by 3.
            int i = 0;
            List<int> segment = new List<int>();
            foreach (char c in input)
            {
                // Chars checked individually here for more detailed error messages.
                int digit = (int)Char.GetNumericValue(c);
                if (digit < 0)
                {
                    throw new ArgumentException("Invalid input: character \"" + c +
                        "\" is not valid. Use only numbers, decimals, spaces, and commas.");
                }

                segment.Add(digit);

                // Check if this is the last char of the input or if segment is full.
                if (++i == input.Count() || segment.Count() == 3)
                {
                    // Reverse the segment to correct the order.
                    segment.Reverse();
                    result.Add(segment);
                    segment = new List<int>();
                }
            }

            // Correct order of the input incase caller wishes to use again.
            input.Reverse();

            // Correct order of the result. Callers will reverse this again, 
            // however returning this result in the correct order means this method can mostly be reused.
            result.Reverse();
            return result;
        }
    }
}