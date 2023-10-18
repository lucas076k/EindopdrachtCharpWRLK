namespace fujas
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Game game = new Game(false, textBox1.Text);
            Visible = false;
            if (!game.IsDisposed)
                game.ShowDialog();
            Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Game game = new Game(true);
            Visible = false;
            if (!game.IsDisposed)
                game.ShowDialog();
            Visible = true;
        }

    }
}