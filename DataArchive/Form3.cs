using DataArchive.Driver;
using DataArchive.Model;
using DataArchive.Utility;

namespace DataArchive;

public partial class Form3 : Form {

    public EndPoint EndPoint { get; private set; }

    public Form3(EndPoint? target) {
        EndPoint = (target ?? new()).Clone();
        InitializeComponent();

        // drivers apply to combobox items
        comboBox1.Items.Clear();
        comboBox1.DisplayMember = "Name";
        foreach (var item in DriverUtility.Drivers.Where(x => new[] { Direction.Output, Direction.InputOutput }.Contains(x.Direction)).OrderBy(x => x.Direction).ThenBy(x => x.Name)) {
            comboBox1.Items.Add(item);
        }

        // data binding
        textBox1.DataBindings.Add(nameof(textBox1.Text), EndPoint, nameof(EndPoint.Host));
        textBox2.DataBindings.Add(nameof(textBox1.Text), EndPoint, nameof(EndPoint.UserName));
        textBox3.DataBindings.Add(nameof(textBox1.Text), EndPoint, nameof(EndPoint.Password));
        comboBox1.DataBindings.Add(nameof(comboBox1.SelectedValue), EndPoint, nameof(EndPoint.Driver));
        comboBox2.DataBindings.Add(nameof(comboBox2.SelectedValue), EndPoint, nameof(EndPoint.DatabaseName));
    }

    private void toolStripButton1_Click(object sender, EventArgs e) {

    }

    private void toolStripButton2_Click(object sender, EventArgs e) {

    }

}
