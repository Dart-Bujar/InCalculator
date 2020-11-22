using System;
using System.Runtime.InteropServices;

namespace ApplCEEHACk
{
    public class InfusionSpeedCalculation
    {
        public double patientWeight;
        
        public double lastAPTT;
        public DateTime lastAPTTCheck;
        private bool _wasInfusionStopped;
        
        public double actualPumpSpeedMlPerHour; // in ml per hour
        public double actualPumpSpeedIUPerKgPerHour; // in IU/kg/hour

        public double actualAPTT; 
        public DateTime nextAPTTCheck;
        public int timePeriod; // in minutes
        
        public double heparinDilution = 50; // 25000 IU Heparinum natricum/500 ml


        public InfusionSpeedCalculation(double patientWeight)
        {
            this.patientWeight = patientWeight;
            this.lastAPTT = 0;
            this.lastAPTTCheck = DateTime.Now;
            this.actualPumpSpeedIUPerKgPerHour = 0;
            _wasInfusionStopped = false;

            var doseAdj = getAdjustmentByAPTT(actualAPTT);
            this.actualPumpSpeedMlPerHour = calculateRecommendedSpeed(this.actualPumpSpeedIUPerKgPerHour, doseAdj, this.patientWeight);

            this.actualAPTT = 0;
            this.nextAPTTCheck = DateTime.Now;

            this.timePeriod = 240; // 4 hours

            this.actualPumpSpeedIUPerKgPerHour += doseAdj;
        }

        public double getActualPumpSpeedMlPerHour()
        {
            return this.actualPumpSpeedMlPerHour;
        }
        
        public double getActualPumpSpeedIUPerKgPerHour()
        {
            return this.actualPumpSpeedIUPerKgPerHour;
        }
        
        public DateTime getTime()
        {
            return this.nextAPTTCheck;
        }
        
        public void setActualPumpSpeedMlPerHour(double actualValue)
        {
            this.actualPumpSpeedMlPerHour = actualValue;
            this.actualPumpSpeedIUPerKgPerHour = (this.actualPumpSpeedMlPerHour * heparinDilution) / this.patientWeight;
        }

        public double getSpeedIUPerKgPerHour(double actualPumpSpeedMlPerHour, double patientWeight)
        {
            return (actualPumpSpeedMlPerHour * heparinDilution) / patientWeight;
        }

        public double GetRecomended(double actualAptt, double actualPumpSpeedMlPerHour, double patientWeight)
        {
            int doseAdj;

            doseAdj = getAdjustmentByAPTT(actualAptt);
            // stop and wait for 60 min, then reduce 3 IU/kg/h
            if (doseAdj == -3)
            {
                this.timePeriod = 60;
                this.nextAPTTCheck = DateTime.Now.AddMinutes(this.timePeriod);
                _wasInfusionStopped = true;
                return -100;
            }
            

            this.timePeriod = 240; // 4 hours
            actualPumpSpeedIUPerKgPerHour = getSpeedIUPerKgPerHour(actualPumpSpeedMlPerHour, patientWeight);
            var recommended = calculateRecommendedSpeed(actualPumpSpeedIUPerKgPerHour, doseAdj, patientWeight);

            this.nextAPTTCheck = DateTime.Now.AddMinutes(this.timePeriod);
            this.actualPumpSpeedIUPerKgPerHour += doseAdj;
            return recommended;
        }


        public double updateAll(double actualAptt)
        {
            this.lastAPTT = this.actualAPTT;
            this.lastAPTTCheck = this.nextAPTTCheck; //.AddMinutes(this.timePeriod);
            
            this.actualAPTT = actualAptt;

            int doseAdj;
            
            if (_wasInfusionStopped)
            {
                doseAdj = -3;
                _wasInfusionStopped = false;
            } else {
                doseAdj = getAdjustmentByAPTT(actualAptt);
                // stop and wait for 60 min, then reduce 3 IU/kg/h
                if (doseAdj == -3)
                {
                    this.timePeriod = 60;
                    this.nextAPTTCheck = DateTime.Now.AddMinutes(this.timePeriod);
                    _wasInfusionStopped = true;
                    return -100;
                }
            }

            this.timePeriod = 240; // 4 hours
            var recommended = calculateRecommendedSpeed(this.actualPumpSpeedIUPerKgPerHour, doseAdj, this.patientWeight);

            this.nextAPTTCheck = DateTime.Now.AddMinutes(this.timePeriod);
            this.actualPumpSpeedIUPerKgPerHour += doseAdj;
            return recommended;
        }

        // returns dose adjustment
        public int getAdjustmentByAPTT(double aptt)
        {
            // init
            if (aptt == 0)
                return 18;
            
            // increase by 4 IU/kg/h
            if (aptt < 1.2)
                return 4;
            
            // increase by 2 IU/kg/h
            if (aptt >= 1.2 && aptt < 1.5)
                return 2;
            
            // +- 1-2 IU/kg/h
            if (aptt >= 1.5 && aptt <= 2.3)
                return 0;
            
            // reduce by 2 IU/kg/h
            if (aptt > 2.3 && aptt <= 3.0)
                return -2;
            
            // stop for an hour, and then reduce by 3 IU/kg/h
            if (aptt > 3)
                return -3;
            
            Console.WriteLine("Error number");
            return -100;
        }

        // actualSpeed is in IU/kg/h
        public double calculateRecommendedSpeed(double actualSpeed, double doseAdjustment, double patientWeight)
        {
            // init speed for patients with weight >= 100
            if (actualSpeed == 0 && patientWeight >= 100)
                return 36; // ml/h
            
            // init speed for patients with weight <= 50
            if (actualSpeed == 0 && patientWeight <= 50)
                return 18; // ml/h
            
            // calculate recommended speed (ml/h) 
            var recommendedSpeed = Math.Round((actualSpeed + doseAdjustment) * patientWeight / heparinDilution, MidpointRounding.ToEven);
            return recommendedSpeed;
        }

    }
}