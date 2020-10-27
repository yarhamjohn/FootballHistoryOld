using System;

namespace football.history.api.Calculators
{
    public static class SeasonDatesCalculator
    {
        public static DateTime GetSeasonEndDate(int seasonStartYear)
        {
            /*
             * Originally the season end date was set to be June 30th as this was roughly half-way between seasons.
             * Due to COVID-19, there was a delay in the fixtures meaning 2019-2020 actually finished on August 4th
             * with the Championship play-off final. Although the 2020-2021 league season did not commence until
             * September, some cup games were held from 29th August so the middle of August is set here.
             */
            if (seasonStartYear == 2019)
            {
                return new DateTime(seasonStartYear + 1, 08, 20);
            }

            return new DateTime(seasonStartYear + 1, 06, 30);
        }

        public static int GetSeasonStartYear(DateTime date)
        {
            /*
             * Originally the season end date was set to be June 30th as this was roughly half-way between seasons.
             * Due to COVID-19, there was a delay in the fixtures meaning 2019-2020 actually finished on August 4th
             * with the Championship play-off final. Although the 2020-2021 league season did not commence until
             * September, some cup games were held from 29th August so the middle of August is set here.
             */
            if (date.Year == 2020)
            {
                return date.Month >= 8 && date.Day > 20 ? date.Year : date.Year - 1;
            }

            return date.Month > 6 ? date.Year : date.Year - 1;
        }
    }
}
