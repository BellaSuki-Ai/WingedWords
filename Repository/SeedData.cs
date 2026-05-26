using WingedWords.Models;

namespace WingedWords.Repository
{
    public static class SeedData
    {
        public static List<Aphorism> GetSeedAphorisms()
        {
            return new List<Aphorism>
            {
                new Aphorism
                {
                    Text = "Не відкладай на завтра те, що можна зробити сьогодні.",
                    Author = "Народна мудрість",
                    Source = "Українські прислів'я",
                    Category = AphorismCategory.Proverb,
                    Theme = "Час",
                    Keywords = new List<string> { "час", "відповідальність", "праця" },
                    IsFavorite = true
                },
                new Aphorism
                {
                    Text = "Знання — сила.",
                    Author = "Френсіс Бекон",
                    Source = "Meditationes Sacrae",
                    Category = AphorismCategory.Aphorism,
                    Theme = "Знання",
                    Keywords = new List<string> { "знання", "сила", "освіта" },
                    IsFavorite = true
                },
                new Aphorism
                {
                    Text = "Я знаю лише те, що нічого не знаю.",
                    Author = "Сократ",
                    Source = "Діалоги Платона",
                    Category = AphorismCategory.Aphorism,
                    Theme = "Мудрість",
                    Keywords = new List<string> { "мудрість", "знання", "філософія" },
                    IsFavorite = false
                },
                new Aphorism
                {
                    Text = "Без труда не виловиш і рибку зі ставка.",
                    Author = "Народна мудрість",
                    Source = "Українські прислів'я",
                    Category = AphorismCategory.Proverb,
                    Theme = "Праця",
                    Keywords = new List<string> { "праця", "зусилля", "результат" },
                    IsFavorite = false
                },
                new Aphorism
                {
                    Text = "Краще пізно, ніж ніколи.",
                    Author = "Народна мудрість",
                    Source = "Латинські приказки",
                    Category = AphorismCategory.Saying,
                    Theme = "Час",
                    Keywords = new List<string> { "час", "початок", "рішення" },
                    IsFavorite = false
                },
                new Aphorism
                {
                    Text = "Життя — це те, що трапляється з тобою, поки ти будуєш інші плани.",
                    Author = "Джон Леннон",
                    Source = "Beautiful Boy",
                    Category = AphorismCategory.Aphorism,
                    Theme = "Життя",
                    Keywords = new List<string> { "життя", "плани", "момент" },
                    IsFavorite = true
                },
                new Aphorism
                {
                    Text = "Той, хто не ризикує, не п'є шампанського.",
                    Author = "Народна мудрість",
                    Source = "Російські приказки",
                    Category = AphorismCategory.Saying,
                    Theme = "Ризик",
                    Keywords = new List<string> { "ризик", "успіх", "сміливість" },
                    IsFavorite = false
                },
                new Aphorism
                {
                    Text = "Будь змінами, які хочеш бачити у світі.",
                    Author = "Махатма Ганді",
                    Source = "Промови та статті",
                    Category = AphorismCategory.Aphorism,
                    Theme = "Зміни",
                    Keywords = new List<string> { "зміни", "світ", "дія" },
                    IsFavorite = true
                },
                new Aphorism
                {
                    Text = "Оптиміст — людина, яка знає, наскільки все погано. Песиміст — той, хто щойно це зрозумів.",
                    Author = "Оскар Уайльд",
                    Source = "Дотепні вислови",
                    Category = AphorismCategory.Pun,
                    Theme = "Гумор",
                    Keywords = new List<string> { "гумор", "оптимізм", "песимізм" },
                    IsFavorite = false
                },
                new Aphorism
                {
                    Text = "Слово — не горобець: вилетить — не спіймаєш.",
                    Author = "Народна мудрість",
                    Source = "Українські прислів'я",
                    Category = AphorismCategory.Proverb,
                    Theme = "Мова",
                    Keywords = new List<string> { "слово", "мова", "обережність" },
                    IsFavorite = false
                },
                new Aphorism
                {
                    Text = "Геній — це один відсоток натхнення і дев'яносто дев'ять відсотків поту.",
                    Author = "Томас Едісон",
                    Source = "Harper's Monthly",
                    Category = AphorismCategory.Aphorism,
                    Theme = "Праця",
                    Keywords = new List<string> { "геній", "праця", "натхнення" },
                    IsFavorite = true
                },
                new Aphorism
                {
                    Text = "Друзі пізнаються в біді.",
                    Author = "Народна мудрість",
                    Source = "Українські прислів'я",
                    Category = AphorismCategory.Proverb,
                    Theme = "Дружба",
                    Keywords = new List<string> { "дружба", "підтримка", "довіра" },
                    IsFavorite = false
                }
            };
        }
    }
}