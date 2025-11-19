using System;using System.Collections.Generic;using System.Globalization;using System.Linq;using System.Text;using System.Threading.Tasks;namespace Drone_Service_Application{
    public class Drone
    {
        private string ClientName = string.Empty;
        private string DroneModel = string.Empty;
        private string ServiceProblem = string.Empty;
        private double ServiceCost = 0.0;
        private int ServiceTag = 0;

        // Public getter/setter methods
        public string GetClientName() { return ClientName; }
        public void SetClientName(string value) { ClientName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower()); ; }

        public string GetDroneModel() { return DroneModel; }
        public void SetDroneModel(string value) { DroneModel = value; }

        public string GetServiceProblem() { return ServiceProblem; }
        public void SetServiceProblem(string value) { ServiceProblem = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower()); ; }
        
        public double GetServiceCost() { return ServiceCost; }
        public void SetServiceCost(double value) { ServiceCost = value; }
        
        public int GetServiceTag() { return ServiceTag; }
        public void SetServiceTag(int value) { ServiceTag = value; }
        public string Display() { return $"Client Name: {ClientName}\n Service Cost: {ServiceCost:C}"; }


    }}