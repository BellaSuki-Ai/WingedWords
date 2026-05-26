using WingedWords.Interfaces;
using WingedWords.Models;
using WingedWords.Repository;
using WingedWords.Services;

namespace WingedWords.Forms
{
    public class MainForm : Form
    {
        // --- Сервіси ---
        private readonly IAphorismService _service;
        private readonly PrintService _printService;
        private bool _isLoading = false; // Захист від рекурсії

        // --- UI елементи ---
        private DataGridView _grid = new();
        private TextBox _searchBox = new();
        private ComboBox _authorFilter = new();
        private ComboBox _themeFilter = new();
        private ComboBox _categoryFilter = new();
        private CheckBox _favoritesOnly = new();
        private Button _btnAdd = new();
        private Button _btnEdit = new();
        private Button _btnDelete = new();
        private Button _btnFavorite = new();
        private Button _btnPrint = new();
        private Button _btnClearFilter = new();
        private Label _countLabel = new();

        public MainForm()
        {
            _service = new AphorismService(new JsonAphorismRepository());
            _printService = new PrintService();

            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            Text = "Крилаті вислови — Довідник";
            Size = new Size(1100, 680);
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(900, 550);
            BackColor = Color.FromArgb(245, 245, 250);

            BuildTopPanel();
            BuildFilterPanel();
            BuildGrid();
            BuildBottomPanel();
        }

        // --- Верхня панель: заголовок + пошук ---
        private void BuildTopPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(45, 45, 70),
                Padding = new Padding(10, 10, 10, 10)
            };

            var title = new Label
            {
                Text = "Крилаті вислови",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(15, 15)
            };

            _searchBox = new TextBox
            {
                PlaceholderText = "Пошук по тексту, автору, ключових словах...",
                Size = new Size(380, 28),
                Location = new Point(550, 17),
                Font = new Font("Segoe UI", 10)
            };
            _searchBox.TextChanged += (s, e) => ApplyFilters();

            panel.Controls.AddRange(new Control[] { title, _searchBox });
            Controls.Add(panel);
        }

        // --- Панель фільтрів ---
        private void BuildFilterPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.FromArgb(230, 230, 240),
                Padding = new Padding(10, 8, 10, 8)
            };

            int x = 10;

            panel.Controls.Add(MakeLabel("Автор:", x, 14)); x += 55;

            _authorFilter = new ComboBox
            {
                Location = new Point(x, 12),
                Size = new Size(160, 26),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            _authorFilter.SelectedIndexChanged += (s, e) => ApplyFilters();
            panel.Controls.Add(_authorFilter); x += 175;

            panel.Controls.Add(MakeLabel("Тема:", x, 14)); x += 50;

            _themeFilter = new ComboBox
            {
                Location = new Point(x, 12),
                Size = new Size(150, 26),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            _themeFilter.SelectedIndexChanged += (s, e) => ApplyFilters();
            panel.Controls.Add(_themeFilter); x += 165;

            panel.Controls.Add(MakeLabel("Тип:", x, 14)); x += 45;

            _categoryFilter = new ComboBox
            {
                Location = new Point(x, 12),
                Size = new Size(130, 26),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            _categoryFilter.Items.Add("Усі типи");
            foreach (AphorismCategory cat in Enum.GetValues<AphorismCategory>())
                _categoryFilter.Items.Add(CategoryToUkrainian(cat));
            _categoryFilter.SelectedIndex = 0;
            _categoryFilter.SelectedIndexChanged += (s, e) => ApplyFilters();
            panel.Controls.Add(_categoryFilter); x += 145;

            _favoritesOnly = new CheckBox
            {
                Text = "⭐ Тільки обрані",
                Location = new Point(x, 14),
                AutoSize = true,
                Font = new Font("Segoe UI", 9)
            };
            _favoritesOnly.CheckedChanged += (s, e) => ApplyFilters();
            panel.Controls.Add(_favoritesOnly); x += 150;

            _btnClearFilter = MakeButton("✕ Скинути", x, 10, 90, 28, Color.FromArgb(180, 60, 60));
            _btnClearFilter.Click += (s, e) => ClearFilters();
            panel.Controls.Add(_btnClearFilter);

            Controls.Add(panel);
        }

        // --- Таблиця висловів ---
        private void BuildGrid()
        {
            _grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                Font = new Font("Segoe UI", 10),
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            };

            // Стиль заголовків
            _grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 70);
            _grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            _grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            _grid.ColumnHeadersHeight = 36;
            _grid.EnableHeadersVisualStyles = false;

            // Чергування рядків
            _grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 255);

            // Колонки
            _grid.Columns.Add(new DataGridViewTextBoxColumn
                { Name = "Text", HeaderText = "Вислів", Width = 420,
                  DefaultCellStyle = { WrapMode = DataGridViewTriState.True } });
            _grid.Columns.Add(new DataGridViewTextBoxColumn
                { Name = "Author", HeaderText = "Автор", Width = 160 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn
                { Name = "Source", HeaderText = "Джерело", Width = 140 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn
                { Name = "Category", HeaderText = "Тип", Width = 100 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn
                { Name = "Theme", HeaderText = "Тема", Width = 120 });
            _grid.Columns.Add(new DataGridViewTextBoxColumn
                { Name = "Favorite", HeaderText = "⭐", Width = 40 });

            _grid.CellDoubleClick += (s, e) => EditSelected();

            Controls.Add(_grid);
        }

        // --- Нижня панель: кнопки ---
        private void BuildBottomPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 55,
                BackColor = Color.FromArgb(230, 230, 240),
                Padding = new Padding(10, 10, 10, 10)
            };

            int x = 10;

            _btnAdd = MakeButton("+ Додати", x, 12, 100, 32, Color.FromArgb(50, 130, 80));
            _btnAdd.Click += (s, e) => AddNew();
            panel.Controls.Add(_btnAdd); x += 115;

            _btnEdit = MakeButton("Редагувати", x, 12, 120, 32, Color.FromArgb(60, 100, 170));
            _btnEdit.Click += (s, e) => EditSelected();
            panel.Controls.Add(_btnEdit); x += 135;

            _btnDelete = MakeButton("Видалити", x, 12, 110, 32, Color.FromArgb(180, 60, 60));
            _btnDelete.Click += (s, e) => DeleteSelected();
            panel.Controls.Add(_btnDelete); x += 125;

            _btnFavorite = MakeButton("Обране", x, 12, 110, 32, Color.FromArgb(180, 140, 30));
            _btnFavorite.Click += (s, e) => ToggleFavorite();
            panel.Controls.Add(_btnFavorite); x += 125;

            _btnPrint = MakeButton("Друкувати", x, 12, 120, 32, Color.FromArgb(80, 80, 80));
            _btnPrint.Click += (s, e) => PrintSelected();
            panel.Controls.Add(_btnPrint);

            _countLabel = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                Location = new Point(700, 18)
            };
            panel.Controls.Add(_countLabel);

            Controls.Add(panel);
        }

        // --- Завантаження даних ---
        private void LoadData(List<Aphorism>? list = null)
        {
            var items = list ?? _service.GetAll();
            _grid.Rows.Clear();

            foreach (var a in items)
            {
                _grid.Rows.Add(
                    a.Text,
                    a.Author,
                    a.Source,
                    CategoryToUkrainian(a.Category),
                    a.Theme,
                    a.IsFavorite ? "⭐" : ""
                );
                _grid.Rows[^1].Tag = a.Id; // Зберігаємо Id у тегу рядка
            }

            RefreshFilters();
            _countLabel.Text = $"Всього: {items.Count} висловів";
        }

        // --- Фільтрація ---
        private void ApplyFilters()
            {
                if (_isLoading) return; // Якщо йде завантаження — ігноруємо

                var filter = new SearchFilter
            {
                Keyword = _searchBox.Text,
                Author = _authorFilter.SelectedIndex > 0
                ? _authorFilter.SelectedItem!.ToString()! : "",
                Theme = _themeFilter.SelectedIndex > 0
                ? _themeFilter.SelectedItem!.ToString()! : "",
                Category = _categoryFilter.SelectedIndex > 0
                ? (AphorismCategory?)(_categoryFilter.SelectedIndex - 1) : null,
                OnlyFavorites = _favoritesOnly.Checked
            };

    LoadData(_service.Search(filter));
}

        private void ClearFilters()
        {
            _searchBox.Clear();
            _authorFilter.SelectedIndex = 0;
            _themeFilter.SelectedIndex = 0;
            _categoryFilter.SelectedIndex = 0;
            _favoritesOnly.Checked = false;
            LoadData();
        }

        private void RefreshFilters()
        {
            _isLoading = true; // Вмикаємо захист

            string? savedAuthor = _authorFilter.SelectedItem?.ToString();
            string? savedTheme = _themeFilter.SelectedItem?.ToString();

            _authorFilter.Items.Clear();
            _authorFilter.Items.Add("Усі автори");
            _service.GetAllAuthors().ForEach(a => _authorFilter.Items.Add(a));
            _authorFilter.SelectedIndex = Math.Max(0, _authorFilter.Items.IndexOf(savedAuthor ?? ""));

            _themeFilter.Items.Clear();
            _themeFilter.Items.Add("Усі теми");
            _service.GetAllThemes().ForEach(t => _themeFilter.Items.Add(t));
            _themeFilter.SelectedIndex = Math.Max(0, _themeFilter.Items.IndexOf(savedTheme ?? ""));

            _isLoading = false; // Вимикаємо захист
}

        // --- CRUD дії ---
        private void AddNew()
        {
            using var form = new EditForm(null, _service);
            if (form.ShowDialog() == DialogResult.OK)
                LoadData();
        }

        private void EditSelected()
        {
            string? id = GetSelectedId();
            if (id == null) return;

            var aphorism = _service.GetById(id);
            if (aphorism == null) return;

            using var form = new EditForm(aphorism, _service);
            if (form.ShowDialog() == DialogResult.OK)
                LoadData();
        }

        private void DeleteSelected()
        {
            string? id = GetSelectedId();
            if (id == null) return;

            var result = MessageBox.Show(
                "Ви впевнені, що хочете видалити цей вислів?",
                "Підтвердження видалення",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                _service.Delete(id);
                LoadData();
            }
        }

        private void ToggleFavorite()
        {
            string? id = GetSelectedId();
            if (id == null) return;

            _service.ToggleFavorite(id);
            LoadData();
        }

        private void PrintSelected()
        {
            var list = _service.GetAll().Where(a => a.IsFavorite).ToList();
            if (list.Count == 0)
                list = _service.GetAll(); // Якщо нема обраних — друкуємо всі

            _printService.Print(list);
        }

        // --- Допоміжні методи ---
        private string? GetSelectedId()
        {
            if (_grid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Оберіть вислів зі списку.", "Увага",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
            return _grid.SelectedRows[0].Tag?.ToString();
        }

        private static Label MakeLabel(string text, int x, int y) => new()
        {
            Text = text, Location = new Point(x, y),
            AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold)
        };

        private static Button MakeButton(string text, int x, int y, int w, int h, Color color)
        {
            return new Button
            {
                Text = text, Location = new Point(x, y), Size = new Size(w, h),
                BackColor = color, ForeColor = Color.White, FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
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