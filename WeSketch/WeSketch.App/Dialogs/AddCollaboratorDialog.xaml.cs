﻿using System;
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

namespace WeSketch.App.Dialogs
{
    /// <summary>
    /// Interaction logic for AddCollaboratorDialog.xaml
    /// </summary>
    public partial class AddCollaboratorDialog : UserControl
    {
        public AddCollaboratorDialog()
        {
            InitializeComponent();
            tbxCollaboratorUsername.KeyUp += TbxCollaboratorUsername_KeyUp;
        }

        private void TbxCollaboratorUsername_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                btnAdd.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                e.Handled = true;
            }
        }

        public void ResetDialog()
        {
            tbxCollaboratorUsername.Clear();
            tbxCollaboratorUsername.Focus();
        }
        
    }
}
