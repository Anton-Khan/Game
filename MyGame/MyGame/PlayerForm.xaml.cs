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
using System.Windows.Shapes;

namespace MyGame
{
    /// <summary>
    /// Логика взаимодействия для PlayerForm.xaml
    /// </summary>
    public partial class PlayerForm : Window
    {
        public int Id { get; set; }

        public PlayerForm()
        {
            InitializeComponent();
        }

        private void Id_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Id = Convert.ToInt32((e.AddedItems[0] as ComboBoxItem).Content.ToString());
        }
    }
}
