using System;
using System.Windows.Forms;

namespace olcEngineSpriteEditor
{
    public partial class NewSprite : Form
    {
        private readonly MainForm _parent;

        public NewSprite(MainForm parent)
        {
            _parent = parent;
            InitializeComponent();
        }

        private void NewSprite_Load(object sender, EventArgs e)
        {

        }

        private void okButton_Click(object sender, EventArgs e)
        {
            _parent.LoadedSprite = new Sprite((int)width.Value, (int)height.Value);
            DialogResult = DialogResult.OK;
        }
    }
}
