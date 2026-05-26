using WingedWords.Models;
using WingedWords.Services;

namespace WingedWords.Forms
{
    public class PrintPreviewForm : Form
    {
        private readonly List<Aphorism> _aphorisms;
        private readonly PrintService _printService;
        private RichTextBox _preview = new();

        public PrintPreviewForm(List<Aphorism> aphorisms, PrintService printService)
        {
            _aphorisms = aphorisms;
            _printService = printService;
            InitializeComponent();
            FillPreview();
        }

        private void InitializeComponent()
        {
            Text = "Попередній перегляд друку";
            Size = new Size(680, 600);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.FromArgb(245, 245, 250);

            var topPanel = new Panel
            {
                Dock = DockStyle.Top, Height = 50,
                BackColor = Color.FromArgb(45, 45, 70)
            };

            var lblTitle = new Label
            {
                Text = $"До друку: {_aphorisms.Count} висловів",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                AutoSize = true, Location = new Point(15, 14)
            };
            topPanel.Controls.Add(lblTitle);

            var btnPrint = new Button
            {
                Text = "🖨 Друкувати",
                Location = new Point(500, 10), Size = new Size(130, 32),
                BackColor = Color.FromArgb(50, 130, 80),
                ForeColor = Color.White, FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnPrint.Click += (s, e) => _printService.Print(_aphorisms);
            topPanel.Controls.Add(btnPrint);

            _preview = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                BackColor = Color.White,
                Font = new Font("Georgia", 11),
                BorderStyle = BorderStyle.None,
                Padding = new Padding(20)
            };

            Controls.Add(_preview);
            Controls.Add(topPanel);
        }

        private void FillPreview()
        {
            _preview.Clear();
            foreach (var a in _aphorisms)
            {
                _preview.SelectionFont = new Font("Georgia", 11, FontStyle.Italic);
                _preview.AppendText($"«{a.Text}»\n");

                _preview.SelectionFont = new Font("Arial", 9);
                _preview.SelectionColor = Color.Gray;
                _preview.AppendText($"  — {a.Author}");
                if (!string.IsNullOrWhiteSpace(a.Source))
                    _preview.AppendText($", {a.Source}");
                _preview.AppendText("\n");
                _preview.AppendText($"  [{CategoryToUkrainian(a.Category)}]");
                if (!string.IsNullOrWhiteSpace(a.Theme))
                    _preview.AppendText($"  Тема: {a.Theme}");
                _preview.AppendText("\n");

                _preview.SelectionColor = Color.LightGray;
                _preview.AppendText("─────────────────────────────────\n\n");
                _preview.SelectionColor = Color.Black;
            }
        }

        private static string CategoryToUkrainian(AphorismCategory cat) => cat switch
        {
            AphorismCategory.Proverb  => "Прислів'я",
            AphorismCategory.Saying   => "Приказка",
            AphorismCategory.Aphorism => "Афоризм",
            AphorismCategory.Pun      => "Каламбур",
            _                         => "Інше"
        };
    }
}