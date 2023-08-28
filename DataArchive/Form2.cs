using DataArchive.Driver;
using DataArchive.Model;
using DataArchive.Properties;
using DataArchive.Utility;

namespace DataArchive;

public partial class Form2 : Form {

    public EndPoint EndPoint { get; private set; }
    public List<string> DatabaseNames { get; private set; } = new List<string>();
    public List<IDriver> Drivers
        => DriverUtility.Drivers
            .Where(x => new[] { Direction.Input, Direction.InputOutput }.Contains(x.Direction))
            .OrderBy(x => x.Direction)
            .ThenBy(x => x.Name)
            .ToList();

    public Form2(EndPoint? source) {
        EndPoint = (source ?? new()).Clone();
        InitializeComponent();

        // drivers apply to combobox items
        comboBox1.DataSource = new BindingSource() { DataSource = Drivers };
        comboBox1.DisplayMember = "Name";
        comboBox1.ValueMember = "Name";

        // database names apply to combobox datasource
        comboBox2.DataSource = new BindingSource() { DataSource = DatabaseNames };

        // password apply
        textBox3.TextChanged += (sender, value) => {
            EndPoint.Password = textBox3.Text;
        };

        // data binding
        textBox1.DataBindings.Add(nameof(textBox1.Text), EndPoint, nameof(EndPoint.Host));
        textBox2.DataBindings.Add(nameof(textBox2.Text), EndPoint, nameof(EndPoint.UserName));
        textBox3.DataBindings.Add(nameof(textBox3.Text), EndPoint, nameof(EndPoint.Password));
        comboBox1.DataBindings.Add(nameof(comboBox1.SelectedValue), EndPoint, nameof(EndPoint.DriverName));
        comboBox2.DataBindings.Add(nameof(comboBox2.SelectedItem), EndPoint, nameof(EndPoint.DatabaseName));
    }

    private void toolStripButton1_Click(object sender, EventArgs e) {
        IDriver? driver = DriverUtility.Drivers.SingleOrDefault(x => x.Name == EndPoint.DriverName);
        if (driver != null && driver.Validate(EndPoint)) {
            DatabaseNames = driver.GetDatabases().ToList();
            if (DatabaseNames.Count() > 0) {
                toolStripStatusLabel5.Image = Resources.accept;
            }
            else {
                toolStripStatusLabel5.Image = Resources.block;
            }
        }
        else {
            toolStripStatusLabel5.Image = Resources.block;
        }
    }

    private void toolStripButton2_Click(object sender, EventArgs e) {
        DialogResult = DialogResult.OK;
        Close();
    }

}
