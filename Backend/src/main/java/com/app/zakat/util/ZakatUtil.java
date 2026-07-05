package com.app.zakat.util;

import java.time.LocalDate;
import java.time.chrono.ChronoLocalDate;
import java.time.chrono.HijrahChronology;
import java.time.chrono.HijrahDate;

public class ZakatUtil {

	public static String usingHijrahChronology(LocalDate gregorianDate) {
	    HijrahChronology hijrahChronology = HijrahChronology.INSTANCE;
	    ChronoLocalDate hijriChronoLocalDate = hijrahChronology.date(gregorianDate);
	    return HijrahDate.from(hijriChronoLocalDate).toString();
	}
}
