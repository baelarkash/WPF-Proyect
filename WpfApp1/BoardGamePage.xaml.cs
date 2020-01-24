﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using WpfApp1.DDBB.Models;

namespace WpfApp1
{
    /// <summary>
    /// Lógica de interacción para TournamentPage.xaml
    /// </summary>
    public partial class BoardGamePage : Page
    {
        DDBB.DDBBContext db = new DDBB.DDBBContext();
        public BoardGamePage()
		{
			InitializeComponent();
            this.DataContext = new BoardGame();
            refreshTable();
            this.cmbPlayers.ItemsSource = db.Players.ToList();

        }
		public BoardGamePage(int id):this()
        {
            InitializeComponent();
			string parameter = string.Empty;
            this.cmbPlayers.ItemsSource = db.Players.ToList();
            loadBoardGame(id);
            refreshTable();

        }
        public void CreateOrUpdate(object sender, RoutedEventArgs e)
        {
            try
            {            
                BoardGame item =(BoardGame) this.DataContext;
                if(item.Id == 0)
                {
                    db.BoardGames.Add(item);
                    item.CreationDate = DateTime.Now;
                }
                item.OwnerId = ((Player)cmbPlayers.SelectedItem).Id;
                db.SaveChanges();
            }catch(Exception ex) { }
            var items = db.BoardGames.ToList();
            Table.ItemsSource = items;
            this.DataContext = new BoardGame();
        }
        private void Edit(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListViewItem);
            if (item != null)
            {
                var boardGame = item.DataContext as BoardGame;
                loadBoardGame(boardGame.Id);
            }
        }
        private void loadBoardGame(int id)
        {
            var dataItem = db.BoardGames.Find(id);
            this.DataContext = dataItem;
            cmbPlayers.SelectedValue = dataItem.Owner;
        }
        private void refreshTable()
        {
            var items = db.BoardGames.OrderBy(x=>x.Name).ToList();
            Table.ItemsSource = items;
        }

        private void AddButton(object sender, RoutedEventArgs e)
        {
            this.DataContext = new BoardGame();
        }
        private void DeleteButton(object sender, RoutedEventArgs e)
        {
            var item = (BoardGame)Table.DataContext;
            db.BoardGames.Remove(item);
            db.SaveChanges();
            refreshTable();
            this.DataContext = new BoardGame();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
