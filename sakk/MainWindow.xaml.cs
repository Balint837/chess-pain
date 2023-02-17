using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace sakk
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BitmapImage
            var c = new List<gyumolcs>() { new alma(), new banan() };
            c.ForEach(v => {
                v.erik();
                v.Rohad();
            });
        }
    }


    public abstract class gyumolcs
    {
        public abstract void erik();

        public virtual void Rohad()
        {
            MessageBox.Show("Rohad");
        }


    }

    class alma : gyumolcs
    {
        public override void erik()
        {
            MessageBox.Show("Piros alma");
        }

        public override void Rohad()
        {
            MessageBox.Show("Barna lett az alma...");
        }

    }

    class banan : gyumolcs
    {
        public override void erik()
        {
            MessageBox.Show("Sárga banán");
        }
    }
}
