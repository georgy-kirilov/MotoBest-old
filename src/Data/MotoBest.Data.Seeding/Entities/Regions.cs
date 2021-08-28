namespace MotoBest.Data.Seeding.Entities
{
    using System.Collections.Generic;

    public static class Regions
    {
        public const string Blagoevgrad = "Благоевград";
        public const string Burgas = "Бургас";
        public const string Varna = "Варна";
        public const string VelikoTarnovo = "Велико Търново";
        public const string Vidin = "Видин";
        public const string Vratsa = "Враца";
        public const string Gabrovo = "Габрово";
        public const string Dobrich = "Добрич";
        public const string Kardzhali = "Кърджали";
        public const string Kyustendil = "Кюстендил";
        public const string Lovech = "Ловеч";
        public const string Montana = "Монтана";
        public const string Pazardzhik = "Пазарджик";
        public const string Pernik = "Перник";
        public const string Pleven = "Плевен";
        public const string Plovdiv = "Пловдив";
        public const string Razgrad = "Разград";
        public const string Ruse = "Русе";
        public const string Silistra = "Силистра";
        public const string Sliven = "Сливен";
        public const string Smolyan = "Смолян";
        public const string Sofia = "София";
        public const string StaraZagora = "Стара Загора";
        public const string Targovishte = "Търговище";
        public const string Haskovo = "Хасково";
        public const string Shumen = "Шумен";
        public const string Yambol = "Ямбол";
        public const string Abroad = "Извън страната";

        public static IEnumerable<string> All()
        {
            return new[]
            {
                Blagoevgrad,
                Burgas,
                Varna,
                VelikoTarnovo,
                Vidin,
                Vratsa,
                Gabrovo,
                Dobrich,
                Kardzhali,
                Kyustendil,
                Lovech,
                Montana,
                Pazardzhik,
                Pernik,
                Pleven,
                Plovdiv,
                Razgrad,
                Ruse,
                Silistra,
                Sliven,
                Smolyan,
                Sofia,
                StaraZagora,
                Targovishte,
                Haskovo,
                Shumen,
                Yambol,
                Abroad,
            };
        }
    }
}
