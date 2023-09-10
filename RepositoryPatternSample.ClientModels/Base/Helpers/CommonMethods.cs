using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternSample.ClientModels.Base.Helpers
{
	public static class CommonMethods
	{
		public static string GetRandomString(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"; // Characters to choose from
			var random = new Random();
			string randomString = new string(Enumerable.Repeat(chars, length)
				.Select(s => s[random.Next(s.Length)]).ToArray()); // Generate the random string

			return randomString;
		}
		public static DateTime GetBDCurrentTime()
		{
			////Option 1
			//var Bangladesh_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
			//return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, Bangladesh_Standard_Time);

			//Option 2
			return DateTime.UtcNow.Add(TimeSpan.FromHours(6));
		}
	}
}
