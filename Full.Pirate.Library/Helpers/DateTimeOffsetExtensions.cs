using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Full.Pirate.Library
{
    public static class DateTimeOffsetExtensions
    {
        public static int GetAge(this DateTimeOffset dateOfBirth)
        {
            DateTimeOffset currentDate = DateTimeOffset.UtcNow;
            int age = currentDate.Year - dateOfBirth.Year;
            if (currentDate.DayOfYear < dateOfBirth.DayOfYear)
            {
                age -= 1;
            }
            return age;
        }
    }
}
