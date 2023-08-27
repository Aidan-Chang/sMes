using DataArchive.Model;

namespace DataArchive;

public partial class Form3 : Form {

    public Target Target { get; private set; }

    public Form3(Target? target) {
        Target = (target ?? new()).Clone();
        InitializeComponent();
    }

    private void toolStripButton1_Click(object sender, EventArgs e) {

    }

    private void toolStripButton2_Click(object sender, EventArgs e) {

    }

}
