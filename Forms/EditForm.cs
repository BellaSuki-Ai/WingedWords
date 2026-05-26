using WingedWords.Interfaces;
using WingedWords.Models;

namespace WingedWords.Forms
{
    public class EditForm : Form
    {
        private readonly IAphorismService _service;
        private readonly Aphorism _aphorism;
        private readonly bool _isNew;

        // Поля вводу
        private TextBox _txtText = new();
        private TextBox _txtAuthor = new();
        private TextBox _txtSource = new();
        private TextBox _txtTheme = new();
        private TextBox _txtKeywords = new();
        private ComboBox _cmbCategory = new();
        private CheckBox _chkFavorite = new();
        private Button _btnSave = new();
        private Button _btnCancel = new();

        public EditForm(Aphorism? aphorism, IAphorismService service)
        {
            _service = service;
            _isNew = aphorism == null;
            _aphorism = aphorism ?? new Aphorism();

            InitializeComponent();
            FillFields();
        }

        private void InitializeComponent()
        {
            Text = _isNew ? "Додати вислів" : "Редагувати вислів";
            Size = new Size(560, 480);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            BackColor = Color.FromArgb(245, 245, 250);

            int labelX = 20, fieldX = 160, y = 20, fieldW = 350;

            AddLabel("Текст вислову:*", labelX, y);
            _txtText = AddTextBox(fieldX, y, fieldW, 80, true);
            y += 100;

            AddLabel("Автор:*", labelX, y);
            _txtAuthor = AddTextBox(fieldX, y, fieldW);
            y += 40;

            AddLabel("Джерело:", labelX, y);
            _txtSource = AddTextBox(fieldX, y, fieldW);
            y += 40;

            AddLabel("Тема:", labelX, y);
            _txtTheme = AddTextBox(fieldX, y, fieldW);
            y += 40;

            AddLabel("Ключові слова:", labelX, y);
            _txtKeywords = AddTextBox(fieldX, y, fieldW);
            AddLabel("(через кому)", fieldX, y + 22, Color.Gray, 8);
            y += 50;

            AddLabel("Тип:", labelX, y);
            _cmbCategory = new ComboBox
            {
                Location = new Point(fieldX, y),
                Size = new Size(200, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            foreach (AphorismCategory cat in Enum.GetValues<AphorismCategory>())
                _cmbCategory.Items.Add(CategoryToUkrainian(cat));
            _cmbCategory.SelectedIndex = 0;
            Controls.Add(_cmbCategory);
            y += 40;

            _chkFavorite = new CheckBox
            {
                Text = "⭐ Додати до обраних",
                Location = new Point(fieldX, y),
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };
            Controls.Add(_chkFavorite);
            y += 45;

            // Кнопки
            _btnSave = new Button
            {
                Text = "Зберегти",
                Location = new Point(fieldX, y),
                Size = new Size(130, 36),
                BackColor = Color.FromArgb(50, 130, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            _btnSave.Click += BtnSave_Click;
            Controls.Add(_btnSave);

            _btnCancel = new Button
            {
                Text = "Скасувати",
                Location = new Point(fieldX + 145, y),
                Size = new Size(110, 36),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10),
                Cursor = Cursors.Hand
            };
            _btnCancel.Click += (s, e) => DialogResult = DialogResult.Cancel;
            Controls.Add(_btnCancel);
        }

        private void FillFields()
        {
            _txtText.Text = _aphorism.Text;
            _txtAuthor.Text = _aphorism.Author;
            _txtSource.Text = _aphorism.Source;
            _txtTheme.Text = _aphorism.Theme;
            _txtKeywords.Text = string.Join(", ", _aphorism.Keywords);
            _cmbCategory.SelectedIndex = (int)_aphorism.Category;
            _chkFavorite.Checked = _aphorism.IsFavorite;
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            // Валідація
            if (string.IsNullOrWhiteSpace(_txtText.Text))
            {
                MessageBox.Show("Введіть текст вислову.", "Помилка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtText.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(_txtAuthor.Text))
            {
                MessageBox.Show("Введіть автора.", "Помилка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtAuthor.Focus();
                return;
            }

            // Зберігаємо дані
            _aphorism.Text = _txtText.Text.Trim();
            _aphorism.Author = _txtAuthor.Text.Trim();
            _aphorism.Source = _txtSource.Text.Trim();
            _aphorism.Theme = _txtTheme.Text.Trim();
            _aphorism.Category = (AphorismCategory)_cmbCategory.SelectedIndex;
            _aphorism.IsFavorite = _chkFavorite.Checked;
            _aphorism.Keywords = _txtKeywords.Text
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(k => k.Trim())
                .Where(k => !string.IsNullOrEmpty(k))
                .ToList();

            if (_isNew)
                _service.Add(_aphorism);
            else
                _service.Update(_aphorism);

            DialogResult = DialogResult.OK;
        }

        // --- Допоміжні методи ---
        private TextBox AddTextBox(int x, int y, int w, int h = 28, bool multiline = false)
        {
            var tb = new TextBox
            {
                Location = new Point(x, y),
                Size = new Size(w, h),
                Font = new Font("Segoe UI", 10),
                Multiline = multiline
            };
            if (multiline) tb.ScrollBars = ScrollBars.Vertical;
            Controls.Add(tb);
            return tb;
        }

        private void AddLabel(string text, int x, int y,
            Color? color = null, float size = 9.5f)
        {
            Controls.Add(new Label
            {
                Text = text,
                Location = new Point(x, y + 4),
                AutoSize = true,
                Font = new Font("Segoe UI", size),
                ForeColor = color ?? Color.FromArgb(45, 45, 70)
            });
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