using System;
using System.Windows;

namespace TripApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve form values
            var startDate = StartDatePicker.SelectedDate;
            var endDate = EndDatePicker.SelectedDate;
            var location = LocationTextBox.Text;
            var peopleCount = PeopleTextBox.Text;
            var isFlexDate = FlexDateCheckBox.IsChecked ?? false;

            // Display the values
            MessageBox.Show($"Start Date: {startDate}\n" +
                            $"End Date: {endDate}\n" +
                            $"Location: {location}\n" +
                            $"People: {peopleCount}\n" +
                            $"Flexible Dates: {isFlexDate}");
        
            //From here internet, local connection, cloud based or other database could be attached
            //and done other functionality.
        }
    }
}
