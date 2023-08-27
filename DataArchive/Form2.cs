using DataArchive.Model;

namespace DataArchive;

public partial class Form2 : Form {

    public Source Source { get; private set; }

    public Form2(Source? source) {
        Source = (source ?? new()).Clone();
        InitializeComponent();
    }

    private void toolStripButton1_Click(object sender, EventArgs e) {

    }

    private void toolStripButton2_Click(object sender, EventArgs e) {

    }

}
