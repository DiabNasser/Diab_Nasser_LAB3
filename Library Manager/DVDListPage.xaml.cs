﻿using LibraryManager.BLL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using static System.Reflection.Metadata.BlobBuilder;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace LibraryManager
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DVDListPage : Page
    {
        private List<DVD> DVDs;

        //storing the member with whom the loan should be associated
        public Member selectedMember { get; set; }
        public DVDListPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is string && !string.IsNullOrWhiteSpace((string)e.Parameter))
            {
                string memberID = e.Parameter.ToString();

                selectedMember = MemberStore.Instance.GetMembersByID(memberID);

                // Filter DVDs based on availability status (IsAvailable property)
                DVDs = DVDStore.Instance.DVDs.Where(b => b.isAvailable == false).ToList();
            }
            base.OnNavigatedTo(e);
        }



        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DVD selectedDVD = DVDs[DVDsListView.SelectedIndex];
            DisplayLoanDVDDialog(selectedDVD);

        }

        private async void DisplayLoanDVDDialog(DVD dvd)
        {
            ContentDialog loanDVDDialog = new ContentDialog
            {
                Title = $"Loan DVD",
                Content = $"Wolud you like to loan the {dvd.title} dvd?",
                PrimaryButtonText = "OK",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await loanDVDDialog.ShowAsync();

            // Loan the book if the user clicked the primary button.
            /// Otherwise, do nothing.
            if (result == ContentDialogResult.Primary)
            {
                // TODO: Loan the book
                LoanDVD(dvd);
            }
            else
            {
                // The user clicked the CLoseButton, pressed ESC, Gamepad B, or the system back button.
                // Do nothing.
            }

        }

        private void LoanDVD(DVD dvd)
        {
            LoanStore.Instance.CreateNewLoan(selectedMember, dvd);
            Frame.Navigate(typeof(MemberLoanPage), selectedMember.id);
        }



        private void AddDVDButton_Click(object sender, RoutedEventArgs e)
        {
            //Frame.Navigate(typeof(DVDLoanPage));
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
