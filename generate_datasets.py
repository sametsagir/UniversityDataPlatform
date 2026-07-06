import os
import csv
import random

# Target folder (sample_datasets folder in the script's directory)
script_dir = os.path.dirname(os.path.abspath(__file__))
output_dir = os.path.join(script_dir, "sample_datasets")
if not os.path.exists(output_dir):
    os.makedirs(output_dir)

def generate_engineering():
    # 1. Engineering - Sensor Fault Detection (Classification: 1200 rows, 9 columns)
    filename = os.path.join(output_dir, "Engineering_Sensor_Fault_Detection.csv")
    headers = ["SensorID", "Temperature_C", "Vibration_Hz", "Pressure_kPa", "Voltage_V", "Flow_Rate_m3s", "Operating_Hours", "Noise_dB", "Fault_Status"]
    with open(filename, "w", newline="", encoding="utf-8") as f:
        writer = csv.writer(f)
        writer.writerow(headers)
        for i in range(1, 1201):
            temp = round(random.uniform(20.0, 95.0), 2)
            vib = round(random.uniform(10.0, 180.0), 2)
            pres = round(random.uniform(80.0, 320.0), 2)
            volt = round(random.uniform(210.0, 240.0), 2)
            flow = round(random.uniform(0.1, 5.0), 2)
            hours = random.randint(100, 15000)
            noise = round(random.uniform(40.0, 95.0), 2)
            
            # Increase fault probability based on temperature, vibration, pressure, and operating hours
            prob = 0.01
            if temp > 80: prob += 0.3
            if vib > 140: prob += 0.3
            if pres > 280: prob += 0.2
            if hours > 10000: prob += 0.1
            
            fault = 1 if random.random() < prob else 0
            writer.writerow([f"SEN_{i:04d}", temp, vib, pres, volt, flow, hours, noise, fault])

def generate_medicine():
    # 2. Medicine - Diabetes Risk Prediction (Classification: 850 rows, 9 columns)
    filename = os.path.join(output_dir, "Medicine_Clinical_Diabetes_Prediction.csv")
    headers = ["PatientID", "Age", "Pregnancies", "Glucose_Level", "Blood_Pressure", "Skin_Thickness", "Insulin_Level", "BMI", "Diabetes_Status"]
    with open(filename, "w", newline="", encoding="utf-8") as f:
        writer = csv.writer(f)
        writer.writerow(headers)
        for i in range(1, 851):
            age = random.randint(18, 80)
            preg = random.randint(0, 12) if age < 45 else 0
            glucose = random.randint(70, 200)
            bp = random.randint(55, 110)
            skin = random.randint(10, 45)
            insulin = random.randint(15, 300)
            bmi = round(random.uniform(18.0, 42.0), 1)
            
            prob = 0.05
            if glucose > 130: prob += 0.45
            if bmi > 30.0: prob += 0.25
            if age > 45: prob += 0.15
            
            diabetes = 1 if random.random() < prob else 0
            writer.writerow([f"PAT_{i:04d}", age, preg, glucose, bp, skin, insulin, bmi, diabetes])

def generate_architecture():
    # 3. Architecture - Building Energy Efficiency (Regression: 650 rows, 8 columns)
    filename = os.path.join(output_dir, "Architecture_Building_Energy_Consumption.csv")
    headers = ["BuildingID", "Surface_Area", "Wall_Area", "Roof_Area", "Overall_Height", "Orientation", "Glass_Area_Ratio", "Heating_Load_kWh"]
    with open(filename, "w", newline="", encoding="utf-8") as f:
        writer = csv.writer(f)
        writer.writerow(headers)
        for i in range(1, 651):
            sa = random.randint(500, 800)
            wa = random.randint(240, 420)
            ca = sa - wa
            height = random.choice([3, 7])
            orientation = random.randint(2, 5) # 2: North, 3: East, 4: South, 5: West
            glass = random.choice([0.10, 0.25, 0.40])
            
            # Heating load depends on area, height, and glazing ratio (Regression)
            base_load = (sa * 0.05) + (wa * 0.08) + (ca * 0.04) + (height * 2.5) - (glass * 15)
            load = round(base_load + random.uniform(-5.0, 5.0), 2)
            writer.writerow([f"BLD_{i:04d}", sa, wa, ca, height, orientation, glass, load])

def generate_literature():
    # 4. Literature - Linguistics and Author Analysis (Classification: 1500 rows, 11 columns)
    filename = os.path.join(output_dir, "Literature_Linguistics_Author_Analysis.csv")
    headers = ["TextID", "Avg_Word_Length", "Avg_Sentence_Length", "Punctuation_Ratio", "Richness_Index", "Repetitive_Word_Ratio", "Adverb_Ratio", "Adjective_Ratio", "Passive_Sentence_Ratio", "Conjunction_Ratio", "Author_Class"]
    with open(filename, "w", newline="", encoding="utf-8") as f:
        writer = csv.writer(f)
        writer.writerow(headers)
        for i in range(1, 1501):
            author = random.choice(["Ahmet_Hamdi", "Halide_Edip", "Peyami_Safa"])
            
            # Stylistic linguistic features based on author profile
            if author == "Ahmet_Hamdi":
                word_len = round(random.uniform(5.5, 6.5), 2)
                sentence_len = round(random.uniform(18.0, 28.0), 2)
                punc = round(random.uniform(0.12, 0.18), 3)
                richness = round(random.uniform(0.70, 0.85), 2)
            elif author == "Halide_Edip":
                word_len = round(random.uniform(4.8, 5.8), 2)
                sentence_len = round(random.uniform(14.0, 22.0), 2)
                punc = round(random.uniform(0.09, 0.14), 3)
                richness = round(random.uniform(0.60, 0.75), 2)
            else: # Peyami Safa
                word_len = round(random.uniform(5.2, 6.2), 2)
                sentence_len = round(random.uniform(16.0, 25.0), 2)
                punc = round(random.uniform(0.11, 0.16), 3)
                richness = round(random.uniform(0.65, 0.80), 2)
                
            repeat = round(random.uniform(0.10, 0.25), 3)
            adverb = round(random.uniform(0.05, 0.12), 3)
            adjective = round(random.uniform(0.08, 0.18), 3)
            passive = round(random.uniform(0.02, 0.10), 3)
            conjunction = round(random.uniform(0.04, 0.15), 3)
            
            writer.writerow([f"TXT_{i:04d}", word_len, sentence_len, punc, richness, repeat, adverb, adjective, passive, conjunction, author])

def generate_science():
    # 5. Science - Chemical Compound Stability (Classification: 1000 rows, 10 columns)
    filename = os.path.join(output_dir, "Science_Chemistry_Compound_Stability.csv")
    headers = ["CompoundID", "Molecular_Weight", "Melting_Point", "Density_gcm3", "Solubility_gL", "pH_Value", "Reactivity_Index", "Pressure_atm", "Temperature_K", "Is_Stable"]
    with open(filename, "w", newline="", encoding="utf-8") as f:
        writer = csv.writer(f)
        writer.writerow(headers)
        for i in range(1, 1001):
            mw = round(random.uniform(50.0, 450.0), 2)
            mp = round(random.uniform(-10.0, 350.0), 2)
            density = round(random.uniform(0.8, 3.2), 3)
            sol = round(random.uniform(0.01, 100.0), 2)
            ph = round(random.uniform(1.0, 14.0), 2)
            react = round(random.uniform(0.1, 10.0), 2)
            pres = round(random.uniform(0.5, 15.0), 2)
            temp = round(random.uniform(200.0, 600.0), 2)
            
            # Stability index formula
            score = (ph - 7.0)**2 + (react * 1.5) + (temp * 0.01) + (pres * 0.2)
            stable = 1 if score < 18.0 else 0
            writer.writerow([f"CMP_{i:04d}", mw, mp, density, sol, ph, react, pres, temp, stable])

def generate_business():
    # 6. Business - Customer Churn Analysis (Classification: 1600 rows, 11 columns)
    filename = os.path.join(output_dir, "Business_Customer_Churn_Analysis.csv")
    headers = ["CustomerID", "Tenure_Months", "Monthly_Charges_USD", "Total_Charges_USD", "Support_Calls", "Age", "Payment_Method", "Extra_Packages", "Satisfaction_Score", "Complaints", "Churn_Status"]
    with open(filename, "w", newline="", encoding="utf-8") as f:
        writer = csv.writer(f)
        writer.writerow(headers)
        for i in range(1, 1601):
            tenure = random.randint(1, 72)
            monthly = random.randint(50, 450)
            total = round(tenure * monthly * random.uniform(0.9, 1.1), 2)
            calls = random.randint(0, 10)
            age = random.randint(18, 75)
            pay = random.choice(["Credit_Card", "Bank_Transfer", "Auto_Debit"])
            extras = random.randint(0, 5)
            satisfaction = random.randint(1, 5)
            complaints = random.randint(0, 4)
            
            # Churn probability based on complaints, support calls, tenure, and satisfaction
            prob = 0.05
            if satisfaction <= 2: prob += 0.4
            if complaints >= 2: prob += 0.3
            if calls > 5: prob += 0.15
            if tenure < 6: prob += 0.1
            
            churn = 1 if random.random() < prob else 0
            writer.writerow([f"CUST_{i:04d}", tenure, monthly, total, calls, age, pay, extras, satisfaction, complaints, churn])

def generate_forestry():
    # 7. Forestry - Tree Type Classification (Classification: 1100 rows, 10 columns)
    filename = os.path.join(output_dir, "Forestry_Tree_Type_Classification.csv")
    headers = ["TreeID", "Elevation_m", "Aspect_Degrees", "Slope_Degrees", "Soil_Moisture_Percent", "Trunk_Diameter_cm", "Height_m", "Chlorophyll_Index", "Annual_Rainfall_mm", "Tree_Type"]
    with open(filename, "w", newline="", encoding="utf-8") as f:
        writer = csv.writer(f)
        writer.writerow(headers)
        for i in range(1, 1101):
            elevation = random.randint(200, 2200)
            aspect = random.randint(0, 360)
            slope = random.randint(0, 45)
            moist = round(random.uniform(5.0, 75.0), 2)
            dbh = round(random.uniform(5.0, 95.0), 2)
            height = round(dbh * random.uniform(0.3, 0.6) + random.uniform(2.0, 10.0), 2)
            chlorophyll = round(random.uniform(15.0, 85.0), 2)
            rain = random.randint(400, 1800)
            
            # Tree type distribution based on elevation and soil moisture
            if elevation < 800:
                tree_type = "Turkish_Pine" if moist < 35 else "Oak"
            elif elevation < 1600:
                tree_type = "Black_Pine" if moist < 45 else "Beech"
            else:
                tree_type = "Scotch_Pine" if rain < 1000 else "Fir"
                
            writer.writerow([f"TRE_{i:04d}", elevation, aspect, slope, moist, dbh, height, chlorophyll, rain, tree_type])

def generate_tourism():
    # 8. Tourism - Hotel Booking Cancellation (Classification: 1800 rows, 11 columns)
    filename = os.path.join(output_dir, "Tourism_Hotel_Cancellation_Analysis.csv")
    headers = ["BookingID", "Lead_Time_Days", "Nights_Stayed", "Adults_Count", "Children_Count", "Room_Type", "Meal_Plan", "Parking_Request", "Repeated_Guest", "Previous_Cancellations", "Cancellation_Status"]
    with open(filename, "w", newline="", encoding="utf-8") as f:
        writer = csv.writer(f)
        writer.writerow(headers)
        for i in range(1, 1801):
            lead = random.randint(0, 365)
            nights = random.randint(1, 14)
            adults = random.randint(1, 4)
            kids = random.randint(0, 3)
            room = random.choice(["Standard", "Family", "Suite", "King"])
            meal = random.choice(["Breakfast_Included", "Half_Board", "All_Inclusive"])
            parking = random.choice([0, 1])
            repeat = random.choice([0, 1])
            prev_cancel = random.choice([0, 1]) if repeat else 0
            
            # Cancellation probability
            prob = 0.08
            if lead > 90: prob += 0.25
            if prev_cancel: prob += 0.4
            if parking == 1: prob -= 0.1
            if repeat == 1: prob -= 0.05
            
            cancel = 1 if random.random() < prob else 0
            writer.writerow([f"RES_{i:04d}", lead, nights, adults, kids, room, meal, parking, repeat, prev_cancel, cancel])

def generate_health_sciences():
    # 9. Health Sciences - Physical Recovery Time (Regression: 900 rows, 10 columns)
    filename = os.path.join(output_dir, "Health_Sciences_Patient_Recovery_Time.csv")
    headers = ["PatientID", "Patient_Age", "Body_Fat_Ratio", "Daily_Calories", "Weekly_Sport_Hours", "Sleep_Hours", "Smoking_Status", "Chronic_Disease", "Physical_Activity_Score", "Recovery_Time_Days"]
    with open(filename, "w", newline="", encoding="utf-8") as f:
        writer = csv.writer(f)
        writer.writerow(headers)
        for i in range(1, 901):
            age = random.randint(18, 70)
            fat = round(random.uniform(10.0, 38.0), 2)
            cal = random.randint(1500, 3500)
            sport = random.randint(0, 15)
            sleep = round(random.uniform(5.0, 9.5), 1)
            smoke = random.choice([0, 1])
            chronic = random.choice([0, 1])
            act_score = random.randint(20, 100)
            
            # Recovery time formula (days) - Regression
            base_days = 5.0 + (age * 0.12) + (fat * 0.15) - (sport * 0.4) - (sleep * 0.3) + (smoke * 4.5) + (chronic * 3.5) - (act_score * 0.05)
            days = max(2, round(base_days + random.uniform(-2.0, 2.0), 1))
            writer.writerow([f"HLT_{i:04d}", age, fat, cal, sport, sleep, smoke, chronic, act_score, days])

def generate_sports_science():
    # 10. Sports Science - Athlete Performance Metrics (Regression: 800 rows, 10 columns)
    filename = os.path.join(output_dir, "Sports_Science_Athlete_Performance_Metrics.csv")
    headers = ["AthleteID", "Age", "Weight_kg", "Height_cm", "VO2_Max_ml_kg_min", "Max_Heart_Rate", "Resting_Heart_Rate", "Weekly_Distance_km", "Weekly_Training_Hours", "Performance_Score"]
    with open(filename, "w", newline="", encoding="utf-8") as f:
        writer = csv.writer(f)
        writer.writerow(headers)
        for i in range(1, 801):
            age = random.randint(16, 40)
            weight = round(random.uniform(55.0, 95.0), 1)
            height = random.randint(160, 202)
            vo2 = round(random.uniform(35.0, 75.0), 2)
            mhr = random.randint(165, 205)
            rhr = random.randint(40, 75)
            dist = round(random.uniform(10.0, 120.0), 2)
            hours = random.randint(3, 25)
            
            # Performance score formula (out of 100)
            base_perf = (vo2 * 0.7) + (hours * 1.2) + (dist * 0.1) - (rhr * 0.2) + ((height/weight)*10)
            perf = min(100.0, max(10.0, round(base_perf + random.uniform(-4.0, 4.0), 2)))
            writer.writerow([f"ATH_{i:04d}", age, weight, height, vo2, mhr, rhr, dist, hours, perf])

if __name__ == "__main__":
    generate_engineering()
    generate_medicine()
    generate_architecture()
    generate_literature()
    generate_science()
    generate_business()
    generate_forestry()
    generate_tourism()
    generate_health_sciences()
    generate_sports_science()
    print("10 domain-specific CSV files successfully generated under sample_datasets/")
