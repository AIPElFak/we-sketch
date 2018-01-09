﻿using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using WeSketch.App.Controller;
using WeSketch.App.Data;
using WeSketch.App.Data.API;
using WeSketch.App.Dialogs;
using WeSketch.App.Model;
using WeSketch.App.View;

namespace WeSketch.App.Forms
{
    /// <summary>
    /// Interaction logic for FormDashboard.xaml
    /// </summary>
    public partial class FormDashboard : MetroWindow, IViewDashboard
    {
        ISketch sketch;
        IAPI api;
        CreateBoardDialog createBoardDialog;
        CustomDialog customDialog;

        public FormDashboard(ISketch sketch)
        {
            InitializeComponent();
            Init(sketch);
            api = new SketchService();
        }

        private async void CreateBoard()
        {
            customDialog = new CustomDialog();
            createBoardDialog = new CreateBoardDialog();
            createBoardDialog.btnCancel.Click += ButtonCancel_Click; ;
            createBoardDialog.btnCreate.Click += ButtonCreate_Click; ;
            customDialog.Content = createBoardDialog;
            await this.ShowMetroDialogAsync(customDialog);
        }

        private void ButtonCreate_Click(object sender, RoutedEventArgs e)
        {
            var title = createBoardDialog.tbxBoardTitle.Text;
            var isPublic = createBoardDialog.cbxIsPublic.IsChecked;

            
  
            this.HideMetroDialogAsync(customDialog);

            LoadMyBoards();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.HideMetroDialogAsync(customDialog);
        }

        private void LoadMyBoards()
        {
            var user = sketch.GetUser();
            var boards = api.GetMyBoards(user);
            //boards.Boards[0].Collaborators.Add(new User() { Username = "Siska" });
            //boards.Boards[0].Collaborators.Add(new User() { Username = "Miska" });
            dataMyBoards.ItemsSource = boards.Boards;
        }

        private void tbxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadMyBoards();
        }

        private void dataMyBoards_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (dataMyBoards.SelectedItem == null) return;
            var board = dataMyBoards.SelectedItem as Board;
            sketch.SetBoard(board);
            FormWorkspace form = new FormWorkspace(sketch);
            form.Show();
        }

        private void btnCreateBoard_Click(object sender, RoutedEventArgs e)
        {
            CreateBoard();
        }

        private void btnDeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            if (dataMyBoards.SelectedItem == null) return;
            var board = dataMyBoards.SelectedItem as Board;
            api.DeleteBoard(board, sketch.GetUser());
            LoadMyBoards();
        }

        public void UpdateMyBoards()
        {
            throw new System.NotImplementedException();
        }

        public void Init(ISketch model)
        {
            this.sketch = model;
        }

        public void MakeController()
        {
            
        }
    }
}