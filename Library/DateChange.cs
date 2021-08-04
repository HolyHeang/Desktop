using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public static class DateChange
    {
        public static string Num(int num)
        {
            if (num >= 10 && num < 100)
            {
                int num1, num2;
                string total = "";
                num1 = (num / 10);
                num2 = (num % 10);

                switch (num1)
                {
                    case 1:
                        total = "១";
                        break;
                    case 2:
                        total = "២";
                        break;
                    case 3:
                        total = "៣";
                        break;
                    case 4:
                        total = "៤";
                        break;
                    case 5:
                        total = "៥";
                        break;
                    case 6:
                        total = "៦";
                        break;
                    case 7:
                        total = "៧";
                        break;
                    case 8:
                        total = "៨";
                        break;
                    case 9:
                        total = "៩";
                        break;
                    case 0:
                        total = "០";
                        break;
                }
                switch (num2)
                {
                    case 1:
                        total += "១";
                        break;
                    case 2:
                        total += "២";
                        break;
                    case 3:
                        total += "៣";
                        break;
                    case 4:
                        total += "៤";
                        break;
                    case 5:
                        total += "៥";
                        break;
                    case 6:
                        total += "៦";
                        break;
                    case 7:
                        total += "៧";
                        break;
                    case 8:
                        total += "៨";
                        break;
                    case 9:
                        total += "៩";
                        break;
                    case 0:
                        total += "០";
                        break;
                }
                return total;
            }
            else if (num >= 100 && num < 1000)
            {
                int num1, num2, num3;
                string total = "";
                num1 = (num / 100);
                num2 = (num / 10) % 10;
                num3 = (num % 100);
                switch (num1)
                {
                    case 1:
                        total = "១";
                        break;
                    case 2:
                        total = "២";
                        break;
                    case 3:
                        total = "៣";
                        break;
                    case 4:
                        total = "៤";
                        break;
                    case 5:
                        total = "៥";
                        break;
                    case 6:
                        total = "៦";
                        break;
                    case 7:
                        total = "៧";
                        break;
                    case 8:
                        total = "៨";
                        break;
                    case 9:
                        total = "៩";
                        break;
                    case 0:
                        total = "០";
                        break;
                }
                switch (num2)
                {
                    case 1:
                        total += "១";
                        break;
                    case 2:
                        total += "២";
                        break;
                    case 3:
                        total += "៣";
                        break;
                    case 4:
                        total += "៤";
                        break;
                    case 5:
                        total += "៥";
                        break;
                    case 6:
                        total += "៦";
                        break;
                    case 7:
                        total += "៧";
                        break;
                    case 8:
                        total += "៨";
                        break;
                    case 9:
                        total += "៩";
                        break;
                    case 0:
                        total += "០";
                        break;
                }
                switch (num3)
                {
                    case 1:
                        total += "១";
                        break;
                    case 2:
                        total += "២";
                        break;
                    case 3:
                        total += "៣";
                        break;
                    case 4:
                        total += "៤";
                        break;
                    case 5:
                        total += "៥";
                        break;
                    case 6:
                        total += "៦";
                        break;
                    case 7:
                        total += "៧";
                        break;
                    case 8:
                        total += "៨";
                        break;
                    case 9:
                        total += "៩";
                        break;
                    case 0:
                        total += "០";
                        break;
                }
                return total;
            }
            else if (num >= 1000 && num < 10000)
            {
                int num1, num2, num3, num4;
                string total = "";
                num1 = (num / 1000);
                num2 = (num / 100) % 10;
                num3 = (num / 10) % 10;
                num4 = num % 10;
                switch (num1)
                {
                    case 1:
                        total = "១";
                        break;
                    case 2:
                        total = "២";
                        break;
                    case 3:
                        total = "៣";
                        break;
                    case 4:
                        total = "៤";
                        break;
                    case 5:
                        total = "៥";
                        break;
                    case 6:
                        total = "៦";
                        break;
                    case 7:
                        total = "៧";
                        break;
                    case 8:
                        total = "៨";
                        break;
                    case 9:
                        total = "៩";
                        break;
                    case 0:
                        total = "០";
                        break;
                }
                switch (num2)
                {
                    case 1:
                        total += "១";
                        break;
                    case 2:
                        total += "២";
                        break;
                    case 3:
                        total += "៣";
                        break;
                    case 4:
                        total += "៤";
                        break;
                    case 5:
                        total += "៥";
                        break;
                    case 6:
                        total += "៦";
                        break;
                    case 7:
                        total += "៧";
                        break;
                    case 8:
                        total += "៨";
                        break;
                    case 9:
                        total += "៩";
                        break;
                    case 0:
                        total += "០";
                        break;
                }
                switch (num3)
                {
                    case 1:
                        total += "១";
                        break;
                    case 2:
                        total += "២";
                        break;
                    case 3:
                        total += "៣";
                        break;
                    case 4:
                        total += "៤";
                        break;
                    case 5:
                        total += "៥";
                        break;
                    case 6:
                        total += "៦";
                        break;
                    case 7:
                        total += "៧";
                        break;
                    case 8:
                        total += "៨";
                        break;
                    case 9:
                        total += "៩";
                        break;
                    case 0:
                        total += "០";
                        break;
                }
                switch (num4)
                {
                    case 1:
                        total += "១";
                        break;
                    case 2:
                        total += "២";
                        break;
                    case 3:
                        total += "៣";
                        break;
                    case 4:
                        total += "៤";
                        break;
                    case 5:
                        total += "៥";
                        break;
                    case 6:
                        total += "៦";
                        break;
                    case 7:
                        total += "៧";
                        break;
                    case 8:
                        total += "៨";
                        break;
                    case 9:
                        total += "៩";
                        break;
                    case 0:
                        total += "០";
                        break;
                }
                return total;
            }
            else
            {
                switch (num)
                {
                    case 1:
                        return "១";
                    case 2:
                        return "២";
                    case 3:
                        return "៣";
                    case 4:
                        return "៤";
                    case 5:
                        return "៥";
                    case 6:
                        return "៦";
                    case 7:
                        return "៧";
                    case 8:
                        return "៨";
                    case 9:
                        return "៩";
                    case 0:
                        return "០";
                }
            }
            return "";
        }

        public static string checkDay(string date)
        {
            if (date.Equals("Monday"))
            {
                return "ថ្ងៃច័ន្ទ";
            }
            else if (date.Equals("Tuesday"))
            {
                return "ថ្ងៃអង្គារ";
            }
            else if (date.Equals("Wednesday"))
            {
                return "ថ្ងៃពុធ";
            }
            else if (date.Equals("Thursday"))
            {
                return "ថ្ងៃព្រហស្បតិ៍";
            }
            else if (date.Equals("Friday"))
            {
                return "ថ្ងៃសុក្រ";
            }
            else if (date.Equals("Saturday"))
            {
                return "ថ្ងៃសៅរ៍";
            }
            else if (date.Equals("Sunday"))
            {
                return "ថ្ងៃអាទិត្យ";
            }
            return null;
        }
        public static string checkMonth(int month)
        {
            switch (month)
            {
                case 1:
                    return "ខែមករា";
                case 2:
                    return "ខែកុម្ភៈ";
                case 3:
                    return "ខែមីនា";
                case 4:
                    return "ខែមេសា";
                case 5:
                    return "ខែឧសភា";
                case 6:
                    return "ខែមិថុនា";
                case 7:
                    return "ខែកក្កដា";
                case 8:
                    return "ខែសីហា";
                case 9:
                    return "ខែកញ្ញា";
                case 10:
                    return "ខែតុលា";
                case 11:
                    return "ខែវិច្អិកា";
                case 12:
                    return "ខែធ្នូ";
            }

            return null;
        }
        public static int checkMonthString(string month)
        {
            switch (month)
            {
                case "មករា":
                    return 1;
                case "កុម្ភៈ":
                    return 2;
                case "មីនា":
                    return 3;
                case "មេសា":
                    return 4;
                case "ឧសភា":
                    return 5;
                case "មិថុនា":
                    return 6;
                case "កក្កដា":
                    return 7;
                case "សីហា":
                    return 8;
                case "កញ្ញា":
                    return 9;
                case "តុលា":
                    return 10;
                case "វិច្ឆិកា":
                    return 11;
                case "ធ្នូ":
                    return 12;
            }

            return 0;
        }
    }
}
