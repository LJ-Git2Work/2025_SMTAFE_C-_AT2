using Drone_Service_Application;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace Drone_Service_Application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// Drone Service Application for Icarus Pty Ltd
    /// CITE Managed Services - Senior Programmer
    /// Compliant with CITEMS coding standards (http://www.citems.com.au/)
    /// </summary>
    public partial class MainWindow : Window
    {
        // 6.2 Create a global List<T> of type Drone called "FinishedList"
        private List<Drone> FinishedList;

        // 6.3 Create a global Queue<T> of type Drone called "RegularService"
        private Queue<Drone> RegularService;

        // 6.4 Create a global Queue<T> of type Drone called "ExpressService"
        private Queue<Drone> ExpressService;

        /// <summary>
        /// Constructor - Initialize MainWindow and data structures
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            InitializeDataStructures();

            // Attach event handler for service cost validation
            ServiceCostTextBox.PreviewTextInput += ServiceCostTextBox_PreviewTextInput;
            ServiceCostTextBox.LostFocus += ServiceCostTextBox_LostFocus;
        }

        /// <summary>
        /// Initialize all data structures (Queues and List)
        /// </summary>
        private void InitializeDataStructures()
        {
            FinishedList = new List<Drone>();
            RegularService = new Queue<Drone>();
            ExpressService = new Queue<Drone>();

            UpdateStatus("Application initialized - Ready");
        }

        // 6.5 Create a button method called "AddNewItem" that will add a new service item to a Queue based on the priority
        /// <summary>
        /// AddNewItem button click event
        /// Validates input, creates Drone object, and adds to appropriate queue based on priority
        /// </summary>
        private void AddNewItem(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate all required inputs
                if (string.IsNullOrWhiteSpace(ClientNameTextBox.Text))
                {
                    UpdateStatus("Error: Client Name is required");
                    return;
                }

                if (string.IsNullOrWhiteSpace(DroneModelTextBox.Text))
                {
                    UpdateStatus("Error: Drone Model is required");
                    return;
                }

                if (string.IsNullOrWhiteSpace(ServiceProblemTextBox.Text))
                {
                    UpdateStatus("Error: Service Problem is required");
                    return;
                }

                if (string.IsNullOrWhiteSpace(ServiceCostTextBox.Text))
                {
                    UpdateStatus("Error: Service Cost is required");
                    return;
                }

                // Parse and validate service cost
                if (!double.TryParse(ServiceCostTextBox.Text, out double serviceCost) || serviceCost <= 0)
                {
                    UpdateStatus("Error: Service Cost must be a valid positive number");
                    return;
                }

                // Parse and validate service tag
                if (!int.TryParse(ServiceTagTextBox.Text, out int serviceTag))
                {
                    UpdateStatus("Error: Service Tag must be a valid number");
                    return;
                }

                // Validate service tag range (100-900)
                if (serviceTag < 100 || serviceTag > 900)
                {
                    UpdateStatus("Error: Service Tag must be between 100 and 900");
                    return;
                }

                // 6.7 Call GetServicePriority method before adding to queue
                string priority = GetServicePriority();

                // 6.6 Before adding to Express Queue, increase service cost by 15%
                if (priority == "Express")
                {
                    serviceCost = serviceCost * 1.15;
                }

                // Create new Drone object using setter methods
                Drone newDrone = new Drone();
                newDrone.SetClientName(ClientNameTextBox.Text);
                newDrone.SetDroneModel(DroneModelTextBox.Text);
                newDrone.SetServiceProblem(ServiceProblemTextBox.Text);
                newDrone.SetServiceCost(serviceCost);
                newDrone.SetServiceTag(serviceTag);

                // Add to appropriate queue based on priority
                if (priority == "Regular")
                {
                    RegularService.Enqueue(newDrone);
                    DisplayRegularService();
                    UpdateStatus($"Added {newDrone.GetClientName()} to Regular Service Queue - Tag #{serviceTag}");
                }
                else // Express
                {
                    ExpressService.Enqueue(newDrone);
                    DisplayExpressService();
                    UpdateStatus($"Added {newDrone.GetClientName()} to Express Service Queue - Tag #{serviceTag} (15% surcharge applied)");
                }

                // 6.11 Increment the service tag control (called before adding, as per requirement)
                IncrementServiceTag();

                // 6.17 Clear all textboxes after service item has been added
                ClearTextBoxes();
            }
            catch (Exception ex)
            {
                UpdateStatus($"Critical Error: {ex.Message}");
            }
        }

        // 6.7 Create a custom method called "GetServicePriority" which returns the value of the priority radio group
        /// <summary>
        /// GetServicePriority - Returns the selected priority from radio buttons
        /// Must be called inside AddNewItem method before adding to queue
        /// </summary>
        private string GetServicePriority()
        {
            if (RegularRadioButton.IsChecked == true)
            {
                return "Regular";
            }
            else if (ExpressRadioButton.IsChecked == true)
            {
                return "Express";
            }
            return "Regular"; // Default fallback
        }

        // 6.8 Create a custom method that will display all elements in the RegularService queue
        /// <summary>
        /// Display all elements in RegularService queue using ListView with appropriate column headers
        /// Manually populates ListView items by calling Drone getter methods
        /// </summary>
        private void DisplayRegularService()
        {
            // Clear the ListView before populating
            RegularServiceListView.Items.Clear();

            // Iterate through all items in the RegularService queue
            foreach (Drone drone in RegularService)
            {
                // Create a ListViewItem with the drone information
                ListViewItem item = new ListViewItem();
                item.Content = new
                {
                    ClientName = drone.GetClientName(),
                    DroneModel = drone.GetDroneModel(),
                    ServiceProblem = drone.GetServiceProblem(),
                    ServiceCost = drone.GetServiceCost().ToString("C"),
                    ServiceTag = drone.GetServiceTag()
                };
                RegularServiceListView.Items.Add(item);
            }
        }

        // 6.9 Create a custom method that will display all elements in the ExpressService queue
        /// <summary>
        /// Display all elements in ExpressService queue using ListView with appropriate column headers
        /// Manually populates ListView items by calling Drone getter methods
        /// </summary>
        private void DisplayExpressService()
        {
            // Clear the ListView before populating
            ExpressServiceListView.Items.Clear();

            // Iterate through all items in the ExpressService queue
            foreach (Drone drone in ExpressService)
            {
                // Create a ListViewItem with the drone information
                ListViewItem item = new ListViewItem();
                item.Content = new
                {
                    ClientName = drone.GetClientName(),
                    DroneModel = drone.GetDroneModel(),
                    ServiceProblem = drone.GetServiceProblem(),
                    ServiceCost = drone.GetServiceCost().ToString("C"),
                    ServiceTag = drone.GetServiceTag()
                };
                ExpressServiceListView.Items.Add(item);
            }
        }

        // 6.10 Create a custom method to ensure the Service Cost textbox can only accept a double value with two decimal points
        /// <summary>
        /// Validate Service Cost input - only allows double values with up to two decimal places
        /// </summary>
        private void ServiceCostTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string currentText = textBox.Text;
            string newText = currentText.Insert(textBox.SelectionStart, e.Text);

            // Empty string is valid (allows clearing the textbox)
            if (string.IsNullOrEmpty(newText))
            {
                e.Handled = false; 
                return;
            }

            // Check if it can be parsed as a decimal/double
            if (!double.TryParse(newText, out double result))
            {
                e.Handled = true; 
                return;
            }

            // Check decimal places
            int decimalIndex = newText.IndexOf('.');
            if (decimalIndex >= 0)
            {
                // Count digits after decimal point
                int decimalPlaces = newText.Length - decimalIndex - 1;
                if (decimalPlaces > 2)
                {
                    e.Handled = true;
                    return;
                }
            }

            e.Handled = false; 
        }

        /// <summary>
        /// Validate Service Cost on lost focus - ensure proper decimal format
        /// </summary>
        private void ServiceCostTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(ServiceCostTextBox.Text, out double value))
            {
                ServiceCostTextBox.Text = value.ToString("F2");
            }
        }

        // 6.11 Create a custom method that will automatically increment the service tag when a service item is added
        /// <summary>
        /// Increment the Service Tag by 10
        /// Wraps from 900 back to 100
        /// </summary>
        private void IncrementServiceTag()
        {
            if (int.TryParse(ServiceTagTextBox.Text, out int currentTag))
            {
                int newTag = currentTag + 10;

                // Wrap around to 100 if exceeds 900
                if (newTag > 900)
                {
                    newTag = 100;
                }

                ServiceTagTextBox.Text = newTag.ToString();
            }
        }

        /// <summary>
        /// Service Tag Up Button Click - Increment by 10 with wrapping
        /// </summary>
        private void ServiceTagUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(ServiceTagTextBox.Text, out int currentTag))
            {
                int newTag = currentTag + 10;

                // Wrap around to 100 if exceeds 900
                if (newTag > 900)
                {
                    newTag = 100;
                }

                ServiceTagTextBox.Text = newTag.ToString();
            }
            else
            {
                // If invalid, reset to minimum
                ServiceTagTextBox.Text = "100";
            }
        }

        /// <summary>
        /// Service Tag Down Button Click - Decrement by 10 with wrapping
        /// </summary>
        private void ServiceTagDownButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(ServiceTagTextBox.Text, out int currentTag))
            {
                int newTag = currentTag - 10;

                // Wrap around to 900 if goes below 100
                if (newTag < 100)
                {
                    newTag = 900;
                }

                ServiceTagTextBox.Text = newTag.ToString();
            }
            else
            {
                // If invalid, reset to minimum
                ServiceTagTextBox.Text = "100";
            }
        }

        // 6.12 Create a mouse click method for the regular service ListView
        /// <summary>
        /// Regular Service ListView click event
        /// Displays Client Name and Service Problem in related textboxes
        /// </summary>
        private void RegularServiceListView_MouseClick(object sender, MouseButtonEventArgs e)
        {
            if (RegularServiceListView.SelectedItem != null)
            {
                ListViewItem item = (ListViewItem)RegularServiceListView.SelectedItem;
                dynamic content = item.Content;

                ClientNameTextBox.Text = content.ClientName;
                ServiceProblemTextBox.Text = content.ServiceProblem;
                UpdateStatus($"Selected: {content.ClientName} from Regular Service Queue");
            }
        }

        // 6.13 Create a mouse click method for the express service ListView
        /// <summary>
        /// Express Service ListView click event
        /// Displays Client Name and Service Problem in related textboxes
        /// </summary>
        private void ExpressServiceListView_MouseClick(object sender, MouseButtonEventArgs e)
        {
            if (ExpressServiceListView.SelectedItem != null)
            {
                ListViewItem item = (ListViewItem)ExpressServiceListView.SelectedItem;
                dynamic content = item.Content;

                ClientNameTextBox.Text = content.ClientName;
                ServiceProblemTextBox.Text = content.ServiceProblem;
                UpdateStatus($"Selected: {content.ClientName} from Express Service Queue");
            }
        }

        // 6.14 Create a button click method to remove from regular ListView and dequeue regular service Queue
        /// <summary>
        /// Complete Regular Service button click event
        /// Dequeues from RegularService, adds to FinishedList, updates displays
        /// </summary>
        private void CompleteRegularService(object sender, RoutedEventArgs e)
        {
            try
            {
                if (RegularService.Count > 0)
                {
                    // Dequeue from regular service queue
                    Drone completedDrone = RegularService.Dequeue();

                    // Add to finished list
                    FinishedList.Add(completedDrone);

                    // Update displays
                    DisplayRegularService();
                    DisplayFinishedServices();

                    UpdateStatus($"Service completed for {completedDrone.GetClientName()} - Moved to Finished Services");
                }
                else
                {
                    UpdateStatus("Error: No items in Regular Service Queue to complete");
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error completing regular service: {ex.Message}");
            }
        }

        // 6.15 Create a button click method to remove from express ListView and dequeue express service Queue
        /// <summary>
        /// Complete Express Service button click event
        /// Dequeues from ExpressService, adds to FinishedList, updates displays
        /// </summary>
        private void CompleteExpressService(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ExpressService.Count > 0)
                {
                    // Dequeue from express service queue
                    Drone completedDrone = ExpressService.Dequeue();

                    // Add to finished list
                    FinishedList.Add(completedDrone);

                    // Update displays
                    DisplayExpressService();
                    DisplayFinishedServices();

                    UpdateStatus($"Express service completed for {completedDrone.GetClientName()} - Moved to Finished Services");
                }
                else
                {
                    UpdateStatus("Error: No items in Express Service Queue to complete");
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error completing express service: {ex.Message}");
            }
        }

        /// <summary>
        /// Display all finished services in ListBox
        /// Shows Client Name and Service Cost using GetDisplay() method
        /// </summary>
        private void DisplayFinishedServices()
        {
            FinishedListBox.Items.Clear();
            foreach (Drone drone in FinishedList)
            {
                FinishedListBox.Items.Add(drone.Display());
            }
        }

        // 6.16 Create a double mouse click method to delete from finished listbox and remove from List
        /// <summary>
        /// Finished ListBox double-click event
        /// Removes service item from finished listbox and FinishedList (client payment/collection)
        /// </summary>
        private void FinishedListBox_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (FinishedListBox.SelectedIndex >= 0 && FinishedListBox.SelectedIndex < FinishedList.Count)
                {
                    int selectedIndex = FinishedListBox.SelectedIndex;
                    string clientName = FinishedList[selectedIndex].GetClientName();

                    // Remove from List<T>
                    FinishedList.RemoveAt(selectedIndex);

                    // Update display
                    DisplayFinishedServices();

                    UpdateStatus($"Removed {clientName} from Finished Services - Payment collected, drone released");
                }
                else
                {
                    UpdateStatus("Error: Please select a valid item from Finished Services");
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error removing finished service: {ex.Message}");
            }
        }

        // 6.17 Create a custom method that will clear all textboxes after each service item has been added
        /// <summary>
        /// Clear all input textboxes and reset to default state
        /// Called after service item has been successfully added
        /// </summary>
        private void ClearTextBoxes()
        {
            ClientNameTextBox.Clear();
            DroneModelTextBox.Clear();
            ServiceProblemTextBox.Clear();
            ServiceCostTextBox.Clear();

            // Reset priority to Regular (default)
            RegularRadioButton.IsChecked = true;

            // Set focus back to first input field
            ClientNameTextBox.Focus();
        }

        /// <summary>
        /// Update status strip with feedback message
        /// All user interactions use status strip for feedback (no message boxes unless critical)
        /// </summary>
        private void UpdateStatus(string message)
        {
            StatusTextBlock.Text = $"{DateTime.Now:HH:mm:ss} - {message}";
        }

        /// <summary>
        /// Validate Service Tag input - numeric only
        /// Ensures only numeric values can be entered
        /// </summary>
        private void ServiceTagTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Check if all characters in the input are digits
            e.Handled = !e.Text.All(char.IsDigit);
        }
    }
}