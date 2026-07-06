using System;
using System.Globalization;

namespace Application.Utils;

public class ZakatUtil
{
	public static string UsingHijrahChronology(DateTime gregorianDate)
	{
		CultureInfo cultureInfo = new CultureInfo("ar-SA");
		return gregorianDate.ToString("yyyy/MM/dd", cultureInfo);
	}
}
