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
using WpfApp1.DDBB;
using WpfApp1.DDBB.Models;

namespace WpfApp1
{
	/// <summary>
	/// Lógica de interacción para UsersPage.xaml
	/// </summary>
	public partial class UsersPage : Page
	{
        DDBBContext db = new DDBBContext();
        public UsersPage()
		{
			InitializeComponent();
            this.DataContext = new Player();
            var items = db.Players.ToList();
			Table.ItemsSource = items;
            cmbBoardGames.ItemsSource = db.BoardGames.ToList();
        }
        public UsersPage(int id)
        {
            InitializeComponent();
            this.DataContext = db.Players.Find(id);
            var items = db.Players.ToList();
            Table.ItemsSource = items;
            cmbBoardGames.ItemsSource = db.BoardGames.ToList();
        }
        private void Edit(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListViewItem);
            if (item != null)
            {
                var Player = item.DataContext as Player;
                var dataItem = db.Players.Find(Player.Id);
                this.DataContext = dataItem;
            }
        }
        public void CreateOrUpdate(object sender, RoutedEventArgs e)
        {
            Player item = (Player)this.DataContext;
            if (item.Id == 0)
            {
                db.Players.Add(item);
                item.CreationDate = DateTime.Now;
            }
            db.SaveChanges();
            RefreshTable();
        }
        private void RefreshTable()
        {
            var items = db.Players.ToList();
            Table.ItemsSource = items;
            this.DataContext = new Player();
        }
        private void AddButton(object sender, RoutedEventArgs e)
        {
            this.DataContext = new Player();
        }
        private void DeleteButton(object sender, RoutedEventArgs e)
        {
            var item = (Player)Table.DataContext;
            db.Players.Remove(item);
            db.SaveChanges();
            RefreshTable();
            this.DataContext = new BoardGame();
        }


        private void EditFavourite(object sender,RoutedEventArgs e)
        {
            var item = (sender as ListViewItem);
            if (item != null)
            {
                var PlayerF = item.DataContext as PlayerFavourite;
                var dataItem = db.playerFavourites.Find(PlayerF.Id);

                PlayerFavourite.DataContext = dataItem;
                cmbBoardGames.SelectedItem = dataItem.BoardGame;
            }
        }
        private void CreateOrUpdateFavourite(object sender,RoutedEventArgs e)
        {
            var item = (PlayerFavourite)PlayerFavourite.DataContext;
            if(item.Id != 0)
            {
                db.SaveChanges();
            }
            else
            {

                db.playerFavourites.Add(item);
            }
            PlayerFavourite.DataContext = new PlayerFavourite();
            cmbBoardGames.SelectedItem = null;
        }
        private void AddFavouriteButton(object sender,RoutedEventArgs e)
        {
            PlayerFavourite.DataContext = new PlayerFavourite();
            cmbBoardGames.SelectedItem = null;
        }
        private void DeleteFavouriteButton(object sender,RoutedEventArgs e)
        {
            var playerFavourite = (PlayerFavourite)Table2.DataContext;
            db.playerFavourites.Remove(playerFavourite);
            PlayerFavourite.DataContext = new PlayerFavourite();
            cmbBoardGames.SelectedItem = null;
        }
    }
}
