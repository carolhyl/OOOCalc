using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OOOCalc
{
	internal class Program
	{
		static void Main(string[] args)
		{

			Console.WriteLine("請輸入你請假的起始時間(格式為:yyyy/MM/dd HH:mm:ss)");
			string InputStartTime = Console.ReadLine();
			DateTime startTime = DateTime.Parse(InputStartTime);

			Console.WriteLine("請輸入你請假的結束時間(格式為:yyyy/MM/dd HH:mm:ss)");
			string InputEndTime = Console.ReadLine();
			DateTime endTime = DateTime.Parse(InputEndTime);

			OOO A = new OOO();

            Console.WriteLine($"你請假的時數是:{A.OOOCalc(startTime, endTime)}");
        }

		//先輸入你得起迄時間
	}

	public class WorkDay
	{
		/// <summary>
		/// 確認今天是不是工作日
		/// </summary>
		/// 

		public DateTime WorkBegin { get; private set; }
		public DateTime WorkEnd { get; private set; }

        public DateTime Date { get; set; }

        private DateTime _startTime;
		public DateTime StarTime
		{
			get { return _startTime; }
			set { if (StarTime > WorkBegin) { _startTime = value; } else { _startTime = WorkBegin; } }
		}

		private DateTime _endTime;
		public DateTime EndTime
		{
			get { return _endTime; }
			set { if (EndTime < WorkEnd) { _endTime = value; } else { _endTime = WorkEnd; } }
			//set { if (EndTime > WorkEnd) { return WorkEnd; } }
			//set { EndTime > WorkEnd ? WorkEnd : value } }
		}

		public int WorkingDay()
		{
			Date = DateTime.Today;
			WorkBegin = new DateTime(Date.Year, Date.Month, Date.Day, 9, 0, 0, 0);
			WorkEnd = new DateTime(Date.Year, Date.Month, Date.Day, 18, 0, 0, 0);
			TimeSpan Total = StarTime - EndTime;
			int totalHour = (int)Total.TotalHours;

		 
			if ((int)StarTime.DayOfWeek >= 1 && (int)StarTime.DayOfWeek <= 5 && (int)EndTime.DayOfWeek >= 1 && (int)EndTime.DayOfWeek <= 5) { return totalHour; }
			else { Console.WriteLine("錯誤,輸入的日期出現假日!"); }
			return totalHour; //回傳值要怎麼寫QAQ
		}
	}

	public class NapTime : WorkDay
	{
        public DateTime NapStart { get; set; }
		public DateTime NapEnd { get; set; }

        public void NappingTime()
		{
			Date = DateTime.Today;
			NapStart = new DateTime(Date.Year, Date.Month, Date.Day, 12, 0, 0, 0);
			NapEnd = new DateTime(Date.Year, Date.Month, Date.Day, 13, 0, 0, 0);
		}
	}
	public class OOO : NapTime
	{

		public TimeSpan TotalHour { get; set; }

		public int OOOCalc(DateTime StarTime, DateTime EndTime )
		{		 
			TotalHour = EndTime - StarTime;
			double totalHour = TotalHour.TotalHours;
			
			// 如果請假時間在午休前
			if (StarTime < NapStart) { return (int)(TotalHour - (NapEnd - NapStart)).TotalHours; }
			// 如果請假時間在午休中
			if (StarTime > NapStart && StarTime < NapEnd) { return (int)(TotalHour - (NapEnd - StarTime)).TotalHours; }
			// 如果請假時間在午休後
			return (int)totalHour;
		}	
	}
	}
	
	

