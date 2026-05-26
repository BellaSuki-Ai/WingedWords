using System.Drawing.Printing;
using WingedWords.Models;

namespace WingedWords.Services
{
    public class PrintService
    {
        private List<Aphorism> _aphorismsToPrint = new();
        private int _currentPage = 0;
        private int _currentIndex = 0;

        // Викликається з UI  передаємо список що друкуємо
        public void Print(List<Aphorism> aphorisms)
        {
            if (aphorisms.Count == 0)
            {
                MessageBox.Show("Немає висловів для друку.", "Друк",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _aphorismsToPrint = aphorisms;
            _currentIndex = 0;
            _currentPage = 1;

            var printDoc = new PrintDocument();
            printDoc.PrintPage += OnPrintPage;

            var dialog = new PrintDialog();
            dialog.Document = printDoc;

            if (dialog.ShowDialog() == DialogResult.OK)
                printDoc.Print();
        }

        // Викликається для кожної сторінки
        private void OnPrintPage(object sender, PrintPageEventArgs e)
        {
            var g = e.Graphics!;
            float margin = 60f;
            float y = margin;
            float pageWidth = e.PageBounds.Width - margin * 2;
            float lineHeight = 22f;

            // Шрифти
            var titleFont = new Font("Georgia", 16, FontStyle.Bold);
            var textFont = new Font("Georgia", 11, FontStyle.Italic);
            var metaFont = new Font("Arial", 9, FontStyle.Regular);
            var pageFont = new Font("Arial", 9, FontStyle.Regular);
            var brush = Brushes.Black;
            var grayBrush = Brushes.Gray;

            // Заголовок тільки на першій сторінці
            if (_currentPage == 1)
            {
                g.DrawString("Крилаті вислови", titleFont, brush, margin, y);
                y += 40;
                g.DrawLine(Pens.Gray, margin, y, e.PageBounds.Width - margin, y);
                y += 20;
            }

            // Друк вислову поки є місце на сторінці
            while (_currentIndex < _aphorismsToPrint.Count)
            {
                var a = _aphorismsToPrint[_currentIndex];

                // Текст вислову
                var textRect = new RectangleF(margin, y, pageWidth, 200);
                var textSize = g.MeasureString($"«{a.Text}»", textFont, (int)pageWidth);

                // Перевіряємо чи влізе на поточну сторінку
                float blockHeight = textSize.Height + lineHeight * 2 + 15;
                if (y + blockHeight > e.PageBounds.Height - margin)
                {
                    e.HasMorePages = true;
                    _currentPage++;
                    return;
                }

                //  вислів
                g.DrawString($"«{a.Text}»", textFont, brush, textRect);
                y += textSize.Height + 5;

                // Автор і джерело
                string meta = $"— {a.Author}";
                if (!string.IsNullOrWhiteSpace(a.Source))
                    meta += $", {a.Source}";
                g.DrawString(meta, metaFont, grayBrush, margin + 20, y);
                y += lineHeight;

                // Тема і категорія
                string info = $"[{CategoryToUkrainian(a.Category)}]";
                if (!string.IsNullOrWhiteSpace(a.Theme))
                    info += $"  Тема: {a.Theme}";
                g.DrawString(info, metaFont, grayBrush, margin + 20, y);
                y += lineHeight + 15;

                // Розділювач між висловами
                g.DrawLine(Pens.LightGray, margin, y, e.PageBounds.Width - margin, y);
                y += 15;

                _currentIndex++;
            }

            // Номер сторінки внизу
            string pageNum = $"Сторінка {_currentPage}";
            g.DrawString(pageNum, pageFont, grayBrush,
                e.PageBounds.Width - margin - 80,
                e.PageBounds.Height - margin);

            e.HasMorePages = false;
        }

        private string CategoryToUkrainian(AphorismCategory category) => category switch
        {
            AphorismCategory.Proverb  => "Прислів'я",
            AphorismCategory.Saying   => "Приказка",
            AphorismCategory.Aphorism => "Афоризм",
            AphorismCategory.Pun      => "Каламбур",
            _                         => "Інше"
        };
    }
}