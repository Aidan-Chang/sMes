using DataArchive.Model;
using DataArchive.Provider;
using DataArchive.Utility;

namespace DataArchive;

public partial class Form3 : Form {

    public Respository Respository { get; private set; }

    public List<string> DatabaseNames { get; private set; } = new List<string>();
    public List<IProvider> Drivers
        => ProviderUtility.Drivers
            .Where(x => new[] { Direction.Output, Direction.InputOutput }.Contains(x.Direction))
            .OrderBy(x => x.Direction)
            .ThenBy(x => x.Name)
            .ToList();

    public Form3(Respository? target) {
        Respository = (target ?? new()).Clone();
        InitializeComponent();

        // drivers apply to combobox items
        comboBox1.DataSource = new BindingSource() { DataSource = Drivers };
        comboBox1.DisplayMember = "Name";
        comboBox1.ValueMember = "Name";

        // database names apply to combobox datasource
        comboBox2.DataSource = new BindingSource() { DataSource = DatabaseNames };

        // password apply
        textBox3.TextChanged += (sender, value) => {
            Respository.Password = textBox3.Text;
        };

        // secure apply
        checkBox1.CheckedChanged += (sender, value) => {
            Respository.Trust = checkBox1.Checked;
        };

        // data binding
        textBox1.DataBindings.Add(nameof(textBox1.Text), Respository, nameof(Respository.Host));
        textBox2.DataBindings.Add(nameof(textBox2.Text), Respository, nameof(Respository.UserName));
        textBox3.DataBindings.Add(nameof(textBox3.Text), Respository, nameof(Respository.Password));
        comboBox1.DataBindings.Add(nameof(comboBox1.SelectedValue), Respository, nameof(Respository.Provider));
        comboBox2.DataBindings.Add(nameof(comboBox2.SelectedItem), Respository, nameof(Respository.DatabaseName));
        checkBox1.DataBindings.Add(nameof(checkBox1.Checked), Respository, nameof(Respository.Trust));
    }

    private void toolStripButton1_Click(object sender, EventArgs e) {
        IProvider? driver = ProviderUtility.Drivers.SingleOrDefault(x => x.Name == Respository.Provider);
    }

    private void toolStripButton2_Click(object sender, EventArgs e) {
        DialogResult = DialogResult.OK;
        Close();
    }

}
