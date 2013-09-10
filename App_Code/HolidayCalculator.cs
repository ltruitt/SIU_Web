using System;
using System.Collections;
using System.Linq;
using System.Xml;



	/// <summary>
	/// Summary description for HolidayCalculator.
	/// </summary>
	public class HolidayCalculator
	{
#region Constructor
        //////////////////////////////////////////////////////////////////////////////
		// Returns all of the holidays occuring in the year following the date      //
        // that is passed in the constructor.  Holidays are defined in an XML file. //
		////////////////////////////////////////////////////////////////////////////// 
		public HolidayCalculator(System.DateTime startDate, string xmlPath)
		{
			this._startingDate = startDate;
			_orderedHolidays = new ArrayList();
			_xHolidays = new XmlDocument();
			_xHolidays.Load(xmlPath);
			this.processXML();
		}
#endregion

		#region Private Properties
		private readonly ArrayList _orderedHolidays;
		private readonly XmlDocument _xHolidays;
		private DateTime _startingDate;
		#endregion

#region Public Properties

        ///////////////////////////////////////////////////////////
		// Return List of holidays listed in chronological order //
		///////////////////////////////////////////////////////////
		public ArrayList OrderedHolidays 
		{
			get { return this._orderedHolidays; }
		}

        public bool IsHoldayDate(string dateString)
        {
            return IsHoldayDate ( DateTime.Parse(dateString) );
        }

        public bool IsHoldayDate(DateTime date)
        {
            return this.OrderedHolidays.Cast<Holiday>().Any(h => h.Date.CompareTo(date) == 0);
        }

        public string ListHolidays()
        {
            string HolidayList = string.Empty;
            foreach (Holiday aHoliday in OrderedHolidays)
            {
                HolidayList += aHoliday.Date.ToShortDateString() + " : " + aHoliday.Name + "<br/>";
            }
            return HolidayList;
        }

#endregion



#region Private Methods

        ///////////////////////////////////////////////////////////////////////
		// Loops through the holidays defined in the XML configuration file, //
		// and adds the next occurance into the OrderHolidays collection if  //
        // it occurs within one year.                                        //
        ///////////////////////////////////////////////////////////////////////
		private void processXML()
		{
			foreach (XmlNode n in _xHolidays.SelectNodes("/Holidays/Holiday"))
			{
                ///////////////////////////////
                // Add Holiday For This Year //
                ///////////////////////////////
				Holiday h = this.processNode(n);
                Holiday h2 = this.processNode(n);
                if (h.Date.Year > 1)
                {
                    this._orderedHolidays.Add(h);

                    ///////////////////////////////
                    // Add Holiday For Last Year //
                    ///////////////////////////////
                    h2.Date =  h.Date.AddYears(-1);
                    this._orderedHolidays.Add(h2);
                }
			}
			_orderedHolidays.Sort();
		}

        ///////////////////////////////////////////////////////////////
		// Processes a Holiday node from the XML configuration file. //
        ///////////////////////////////////////////////////////////////
		private Holiday processNode(XmlNode n)
		{
			Holiday h = new Holiday();
			h.Name = n.Attributes["name"].Value;
			ArrayList childNodes = new ArrayList();
			foreach (XmlNode o in n.ChildNodes)
			{
				childNodes.Add(o.Name);
			}
			if (childNodes.Contains("WeekOfMonth"))
			{
				int m = Int32.Parse(n.SelectSingleNode("./Month").InnerXml);
				int w = Int32.Parse(n.SelectSingleNode("./WeekOfMonth").InnerXml);
				int wd = Int32.Parse(n.SelectSingleNode("./DayOfWeek").InnerXml);
				h.Date = this.getDateByMonthWeekWeekday(m,w,wd,this._startingDate);
			}
			else if (childNodes.Contains("DayOfWeekOnOrAfter"))
			{
				int dow = Int32.Parse(n.SelectSingleNode("./DayOfWeekOnOrAfter/DayOfWeek").InnerXml);
				if (dow > 6 || dow < 0)
					throw new Exception("DOW is greater than 6");
				int m = Int32.Parse(n.SelectSingleNode("./DayOfWeekOnOrAfter/Month").InnerXml);
				int d = Int32.Parse(n.SelectSingleNode("./DayOfWeekOnOrAfter/Day").InnerXml);
				h.Date = this.getDateByWeekdayOnOrAfter(dow,m,d, this._startingDate);
			}
			else if (childNodes.Contains("WeekdayOnOrAfter"))
			{
				int m = Int32.Parse(n.SelectSingleNode("./WeekdayOnOrAfter/Month").InnerXml);
				int d = Int32.Parse(n.SelectSingleNode("./WeekdayOnOrAfter/Day").InnerXml);
				DateTime dt = new DateTime(this._startingDate.Year, m, d);
				if (dt < this._startingDate)
					dt = dt.AddYears(1);
				while(dt.DayOfWeek.Equals(DayOfWeek.Saturday) || dt.DayOfWeek.Equals(DayOfWeek.Sunday))
				{
					dt = dt.AddDays(1);
				}
				h.Date =dt;
			}
			else if (childNodes.Contains("LastFullWeekOfMonth"))
			{
				int m = Int32.Parse(n.SelectSingleNode("./LastFullWeekOfMonth/Month").InnerXml);
				int weekday = Int32.Parse(n.SelectSingleNode("./LastFullWeekOfMonth/DayOfWeek").InnerXml);
				DateTime dt = this.getDateByMonthWeekWeekday(m,5,weekday, this._startingDate);

				if (dt.AddDays(6-weekday).Month == m)
					h.Date = dt;
				else
					h.Date = dt.AddDays(-7);
			}
			else if (childNodes.Contains("DaysAfterHoliday"))
			{
				XmlNode basis = _xHolidays.SelectSingleNode("/Holidays/Holiday[@name='" + n.SelectSingleNode("./DaysAfterHoliday").Attributes["Holiday"].Value + "']");
				Holiday bHoliday = this.processNode(basis);
				int days = Int32.Parse(n.SelectSingleNode("./DaysAfterHoliday/Days").InnerXml);
				h.Date = bHoliday.Date.AddDays(days);
			}
			else if (childNodes.Contains("Easter"))
			{
				h.Date = this.easter();
			}
			else
			{
				if (childNodes.Contains("Month") && childNodes.Contains("Day"))
				{
					int m = Int32.Parse(n.SelectSingleNode("./Month").InnerXml);
					int d = Int32.Parse(n.SelectSingleNode("./Day").InnerXml);
					DateTime dt = new DateTime(this._startingDate.Year, m, d);
					if (dt < this._startingDate)
					{
						dt = dt.AddYears(1);
					}
					if (childNodes.Contains("EveryXYears"))
					{
						int yearMult = Int32.Parse(n.SelectSingleNode("./EveryXYears").InnerXml);
						int startYear = Int32.Parse(n.SelectSingleNode("./StartYear").InnerXml);
						if (((dt.Year - startYear) % yearMult) == 0)
						{
							h.Date = dt;
						}
					}
					else
					{
						h.Date = dt;
					}
				}
			}
			return h;
		}

		//////////////////////////////////////////////////////////////////
		// Determines the next occurance of Easter (western Christian). //
        //////////////////////////////////////////////////////////////////
		private DateTime easter()
		{
			DateTime workDate = this.getFirstDayOfMonth(this._startingDate);
			int y = workDate.Year;
			if (workDate.Month > 4)
				y = y+1;
			return this.easter(y);
		}

        ////////////////////////////////////////////////////////////////////////////////
		// Determines the occurance of Easter in the given year.                      //
		// If the result comes before StartDate, recalculates for the following year. //
		////////////////////////////////////////////////////////////////////////////////
		private DateTime easter(int y)
		{
			int a=y%19;
			int b=y/100;
			int c=y%100;
			int d=b/4;
			int e=b%4;
			int f=(b+8)/25;
			int g=(b-f+1)/3;
			int h=(19*a+b-d-g+15)%30;
			int i=c/4;
			int k=c%4;
			int l=(32+2*e+2*i-h-k)%7;
			int m=(a+11*h+22*l)/451;
			int easterMonth =(h+l-7*m+114)/31;
			int  p=(h+l-7*m+114)%31;
			int easterDay=p+1;
			DateTime est = new DateTime(y,easterMonth,easterDay);
			if (est < this._startingDate)
				return this.easter(y+1);
			else
				return new DateTime(y,easterMonth,easterDay);
		}

        ///////////////////////////////////////////////////////////////////////////////////////////////////
		// Gets the next occurance of a weekday after a given month and day in the year after StartDate. //
		///////////////////////////////////////////////////////////////////////////////////////////////////
		private DateTime getDateByWeekdayOnOrAfter(int weekday, int m, int d, DateTime startDate)
		{
			DateTime workDate = this.getFirstDayOfMonth(startDate);
			while (workDate.Month != m)
			{
				workDate = workDate.AddMonths(1);
			}
			workDate = workDate.AddDays(d-1);

			while (weekday != (int)(workDate.DayOfWeek))
			{
				workDate = workDate.AddDays(1);
			}

            /////////////////////////////////////////////////////////////////////////////
			// It's possible the resulting date is before the specified starting date. // 
            // If so we'll calculate again for the next year.                          //
            /////////////////////////////////////////////////////////////////////////////
			if (workDate < this._startingDate)
				return this.getDateByWeekdayOnOrAfter(weekday,m,d,startDate.AddYears(1));
			else
				return workDate;				
		}

        ////////////////////////////////////////////////////////////////////////////////
		// Gets the n'th instance of a day-of-week in the given month after StartDate //
		////////////////////////////////////////////////////////////////////////////////
		private DateTime getDateByMonthWeekWeekday(int month, int week, int weekday, DateTime startDate)
		{
			DateTime workDate = this.getFirstDayOfMonth(startDate);
			while (workDate.Month != month)
			{
				workDate = workDate.AddMonths(1);
			}
			while ((int)workDate.DayOfWeek != weekday)
			{
				workDate = workDate.AddDays(1);
			}

			DateTime result;
			if (week == 1)
			{
				result =  workDate;
			}
			else
			{
				int addDays = (week*7)-7;
				int day = workDate.Day + addDays;
				if (day > DateTime.DaysInMonth(workDate.Year, workDate.Month))
				{
					day = day-7;
				}
				result = new  DateTime(workDate.Year,workDate.Month,day);
			}

            /////////////////////////////////////////////////////////////////////////////
			// It's possible the resulting date is before the specified starting date. // 
            // If so we'll calculate again for the next year.                          //
            /////////////////////////////////////////////////////////////////////////////
			if (result >= this._startingDate)
				return result;
			else
				return this.getDateByMonthWeekWeekday(month,week,weekday,startDate.AddYears(1));


		}

        ////////////////////////////////////////////////////////////////
		// Returns the first day of the month for the specified date. //
		////////////////////////////////////////////////////////////////
		private DateTime getFirstDayOfMonth(DateTime dt)
		{
			return new DateTime(dt.Year, dt.Month, 1);
		}
#endregion

#region Holiday Object
		public class Holiday : IComparable
		{
			public System.DateTime Date;
			public string Name;

			public int CompareTo(object obj)
			{
				if (obj is Holiday)
				{
					Holiday h = (Holiday)obj;
					return this.Date.CompareTo(h.Date);
				}
				throw new ArgumentException("Object is not a Holiday"); 
			}
		}
#endregion
	}

