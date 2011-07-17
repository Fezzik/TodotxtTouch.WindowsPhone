﻿using System;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Shell;
using TodotxtTouch.WindowsPhone.Messages;
using TodotxtTouch.WindowsPhone.ViewModel;

namespace TodotxtTouch.WindowsPhone
{
	public partial class MainPivot : TaskFilterPage
	{
		public MainPivot()
		{
			InitializeComponent();

			Messenger.Default.Register<NeedCredentialsMessage>(
				this, (message) => ShowLogin());

			Messenger.Default.Register<ViewTaskMessage>(
				this, ViewSelectedTask);

			Messenger.Default.Register<DrillDownMessage>(this, DrillDown);
				
			((ApplicationBarIconButton)ApplicationBar.Buttons[0]).Click += AddButton_Click;
			((ApplicationBarIconButton)ApplicationBar.Buttons[1]).Click += MultiSelect_Click;
			((ApplicationBarIconButton)ApplicationBar.Buttons[2]).Click += SyncButton_Click;

			Loaded += MainPage_Loaded;
		}

		private void SyncButton_Click(object sender, EventArgs e)
		{
			((MainViewModel)DataContext).SyncCommand.Execute(null);
		}

		private void MultiSelect_Click(object sender, EventArgs e)
		{
			string filter;
			if (NavigationContext.QueryString.TryGetValue("filter", out filter))
			{
				NavigationService.Navigate(
			   new Uri("/MultiSelectPage.xaml?filter=" + filter, UriKind.Relative));
			}
			else
			{
				NavigationService.Navigate(
			   new Uri("/MultiSelectPage.xaml", UriKind.Relative));
			}
		}

		private void DrillDown(DrillDownMessage message)
		{
			 NavigationService.Navigate(
			   new Uri("/MainPivot.xaml?filter=" + message.Filter, UriKind.Relative));
		}

		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			BroadCastFilter();
		}

		private void MainPage_Loaded(object sender, RoutedEventArgs e)
		{
			LittleWatson.CheckForPreviousException();	

			Messenger.Default.Send(new ApplicationReadyMessage());
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
			((MainViewModel) DataContext).AddTaskCommand.Execute(null);
		}

		private void ViewSelectedTask(ViewTaskMessage obj)
		{
			NavigationService.Navigate(new Uri("/TaskDetail.xaml", UriKind.Relative));
		}

		private void ShowLogin()
		{
			NavigationService.Navigate(new Uri("/DropboxLogin.xaml", UriKind.Relative));
		}

		private void ArchiveClick(object sender, EventArgs e)
		{
			((MainViewModel)DataContext).ArchiveTasksCommand.Execute(null);
		}

		private void SettingsClick(object sender, EventArgs e)
		{
			NavigationService.Navigate(new Uri("/ApplicationSettingsPage.xaml", UriKind.Relative));
		}
	}
}

