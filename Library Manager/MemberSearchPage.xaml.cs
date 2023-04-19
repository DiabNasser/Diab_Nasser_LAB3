using LibraryManager.BLL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace LibraryManager
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MemberSearchPage : Page
    {
        //This will be presented in the list
        ObservableCollection<Member> members = new ObservableCollection<Member>();

        public MemberSearchPage()
        {
            this.InitializeComponent();

            LoadAllMembers();

            //We set the navigation cache mode -> the page will be chached while we are "away" from it.
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        //Loads all members
        private void LoadAllMembers(string nameSearch = null, string idSearch = null)
        {
            members.Clear();

            //Filter members by name and/or ID if search criteria are provided
            var filteredMembers = MemberStore.Instance.members;
            if (!string.IsNullOrEmpty(nameSearch))
            {
                filteredMembers = filteredMembers.Where(m => m.name.Contains(nameSearch)).ToList();
            }
            if (!string.IsNullOrEmpty(idSearch))
            {
                filteredMembers = filteredMembers.Where(m => m.id.Contains(idSearch)).ToList();
            }

            //Add filtered members to ObservableCollection
            foreach (var member in filteredMembers)
            {
                members.Add(member);
            }
        }


        //TODO: Load searched members

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MembersListView.SelectedIndex < 0) { return; }
            Member mb = members[MembersListView.SelectedIndex];

            this.Frame.Navigate(typeof(MemberLoanPage), mb.id);
            // Clear the selected index to allow re-selecting the same member again
            MembersListView.SelectedIndex = -1;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string name = MemberNameTextBox.Text;
            string id = MemberIdTextBox.Text;

            LoadAllMembers(name, id);
            if (members.Count == 0)
            {
                var dialog = new MessageDialog("No member found.");
                _ = dialog.ShowAsync();
            }
        }

    }
}
