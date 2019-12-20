using System;

namespace YTB.Intranet.Helper
{
    class WeatherHelper
    {
        public static String GetTurkishDay(String day)
        {
            switch (day)
            {
                case "Mon":
                    return "Pzt";
                case "Tue":
                    return "Sal";
                case "Wed":
                    return "Çarş";
                case "Thu":
                    return "Perş";
                case "Fri":
                    return "Cuma";
                case "Sat":
                    return "Cmt";
                case "Sun":
                    return "Pzr";
                case "Monday":
                    return "Pazartesi";
                case "Tuesday":
                    return "Salı";
                case "Wednesday":
                    return "Çarşamba";
                case "Thursday":
                    return "Perşembe";
                case "Friday":
                    return "Cuma";
                case "Saturday":
                    return "Cumartesi";
                case "Sunday":
                    return "Pazar";
            }
            return "";
        }

        public static String TemptoCelcius(String f)
        {
            int sonuc =  Convert.ToInt32(5.0 / 9.0 * (Convert.ToInt32(f) - 32));

            return sonuc.ToString();
        }

        public static String GetTextbyCode(String code)
        {
            String sonuc = "";
            int newcode = Convert.ToInt32(code);
            switch(newcode)
            {
                case 0:
                    sonuc = "Hortum";
                    break;
                case 1:
                    sonuc = "Tropik Fırtına";
                    break;
                case 2:
                    sonuc = "Fırtına";
                    break;
                case 3:
                    sonuc = "Seyrek Yıldırımlı Fırtına";
                    break;
                case 4:
                    sonuc = "Yıldırımlı Fırtına";
                    break;
                case 5:
                    sonuc = "Karla karışık Yağmur";
                    break;
                case 6:
                    sonuc = "Karla karışık Yağmur";
                    break;
                case 7:
                    sonuc = "Karla karışık Yağmur";
                    break;
                case 8:
                    sonuc = "Ciseli Yağmur";
                    break;
                case 9:
                    sonuc = "Ciseli Yağmur";
                    break;
                case 10:
                    sonuc = "Yağmurlu Soğuk";
                    break;
                case 11:
                    sonuc = "Sağanak Yağış";
                    break;
                case 12:
                    sonuc = "Sağanak Yağış";
                    break;
                case 13:
                    sonuc = "İnce Kar Yağışı";
                    break;
                case 14:
                    sonuc = "Hafif Karlı Yağmur";
                    break;
                case 15:
                    sonuc = "Rüzgarlı Kar Yağışı";
                    break;
                case 16:
                    sonuc = "Kar Yağışı";
                    break;
                case 17:
                    sonuc = "Dolu Yağışı";
                    break;
                case 18:
                    sonuc = "Sulu Kar Yağışı";
                    break;
                case 19:
                    sonuc = "Tozlu Rüzgar";
                    break;
                case 20:
                    sonuc = "Sisli";
                    break;
                case 21:
                    sonuc = "Puslu";
                    break;
                case 22:
                    sonuc = "Dumanlı";
                    break;
                case 23:
                    sonuc = "Sert Rüzgarlı";
                    break;
                case 24:
                    sonuc = "Rüzgarlı";
                    break;
                case 25:
                    sonuc = "Soğuk";
                    break;
                case 26:
                    sonuc = "Yağmurlu";
                    break;
                case 27:
                    sonuc = "Gece Yoğun Bulutlu";
                    break;
                case 28:
                    sonuc = "Gündüz Yoğun Bulutlu";
                    break;
                case 29:
                    sonuc = "Gece Parçalı Bulutlu";
                    break;
                case 30:
                    sonuc = "Gündüz Parçalı Bulutlu";
                    break;
                case 31:
                    sonuc = "Gece Açık Hava";
                    break;
                case 32:
                    sonuc = "Yağmurlu";
                    break;
                case 33:
                    sonuc = "Gece Açık Hava";
                    break;
                case 34:
                    sonuc = "Gündüz Açık Hava";
                    break;
                case 35:
                    sonuc = "Yağmur ve Dolu Karışık";
                    break;
                case 36:
                    sonuc = "Sıcak";
                    break;
                case 37:
                    sonuc = "Fırtınalı";
                    break;
                case 38:
                    sonuc = "Fırtınalı";
                    break;
                case 39:
                    sonuc = "Fırtınalı";
                    break;
                case 40:
                    sonuc = "Yağmurlu";
                    break;
                case 41:
                    sonuc = "Sağnak Kar Yağışı";
                    break;
                case 42:
                    sonuc = "Gök gürültülü Kar Yağışlı";
                    break;
                case 43:
                    sonuc = "Sağnak Kar Yağışı";
                    break;
                case 44:
                    sonuc = "Parçalı Bulutlu";
                    break;
                case 45:
                    sonuc = "Fırtınalı Yağışlı";
                    break;
                case 46:
                    sonuc = "Sağnak Kar Yağışı";
                    break;
                case 47:
                    sonuc = "Sağnak Kar Yağışı";
                    break;
            }
            return sonuc;
        }

        public static String GetIconbyCode(String code)
        {
            String sonuc = "";
            int newcode = Convert.ToInt32(code);
            switch (newcode)
            {
                case 0:
                    sonuc = "wind";
                    break;
                case 1:
                    sonuc = "sleet";
                    break;
                case 2:
                    sonuc = "rain";
                    break;
                case 3:
                    sonuc = "sleet";
                    break;
                case 4:
                    sonuc = "rain";
                    break;
                case 5:
                    sonuc = "sleet";
                    break;
                case 6:
                    sonuc = "sleet";
                    break;
                case 7:
                    sonuc = "sleet";
                    break;
                case 8:
                    sonuc = "rain";
                    break;
                case 9:
                    sonuc = "rain";
                    break;
                case 10:
                    sonuc = "rain";
                    break;
                case 11:
                    sonuc = "rain";
                    break;
                case 12:
                    sonuc = "rain";
                    break;
                case 13:
                    sonuc = "sleet";
                    break;
                case 14:
                    sonuc = "sleet";
                    break;
                case 15:
                    sonuc = "snow";
                    break;
                case 16:
                    sonuc = "snow";
                    break;
                case 17:
                    sonuc = "sleet";
                    break;
                case 18:
                    sonuc = "sleet";
                    break;
                case 19:
                    sonuc = "wind";
                    break;
                case 20:
                    sonuc = "fog";
                    break;
                case 21:
                    sonuc = "fog";
                    break;
                case 22:
                    sonuc = "fog";
                    break;
                case 23:
                    sonuc = "wind";
                    break;
                case 24:
                    sonuc = "wind";
                    break;
                case 25:
                    sonuc = "wind";
                    break;
                case 26:
                    sonuc = "rain";
                    break;
                case 27:
                    sonuc = "partly-cloudy-night";
                    break;
                case 28:
                    sonuc = "cloudy";
                    break;
                case 29:
                    sonuc = "partly-cloudy-night";
                    break;
                case 30:
                    sonuc = "partly-cloudy-day";
                    break;
                case 31:
                    sonuc = "clear-night";
                    break;
                case 32:
                    sonuc = "rain";
                    break;
                case 33:
                    sonuc = "clear-night";
                    break;
                case 34:
                    sonuc = "clear-day";
                    break;
                case 35:
                    sonuc = "sleet";
                    break;
                case 36:
                    sonuc = "hot";
                    break;
                case 37:
                    sonuc = "wind";
                    break;
                case 38:
                    sonuc = "wind";
                    break;
                case 39:
                    sonuc = "wind";
                    break;
                case 40:
                    sonuc = "rain";
                    break;
                case 41:
                    sonuc = "snow";
                    break;
                case 42:
                    sonuc = "snow";
                    break;
                case 43:
                    sonuc = "snow";
                    break;
                case 44:
                    sonuc = "partly-cloudy-day";
                    break;
                case 45:
                    sonuc = "rain";
                    break;
                case 46:
                    sonuc = "snow";
                    break;
                case 47:
                    sonuc = "snow";
                    break;
            }
            return sonuc;
        }
    }
}
