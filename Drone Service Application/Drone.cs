using System;using System.Collections.Generic;using System.Globalization;using System.Linq;using System.Text;using System.Threading.Tasks;namespace Drone_Service_Application{
    public class Drone
    {
        // Private attributes
        private string ClientName;
        private string DroneModel;
        private string ServiceProblem;
        private double ServiceCost;
        private int ServiceTag;

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
        public string Display() { return $"Client Name: {ClientName} - Service Cost: {ServiceCost:C}"; }
    }}