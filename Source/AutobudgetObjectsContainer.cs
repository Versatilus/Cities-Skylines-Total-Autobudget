using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Autobudget   
{
    public class AutobudgetObjectsContainer
    {
        private const string optionsFileName = "TotalAutobudgetOptions.xml";

        public AutobudgetElectricity AutobudgetElectricity;
        public AutobudgetWater AutobudgetWater;
        public AutobudgetHeating AutobudgetHeating;
        public AutobudgetGarbage AutobudgetGarbage;
        public AutobudgetHealthcare AutobudgetHealthcare;
        public AutobudgetEducation AutobudgetEducation;
        public AutobudgetPolice AutobudgetPolice;
        public AutobudgetFire AutobudgetFire;
        public AutobudgetIndustry AutobudgetIndustry;
        public AutobudgetRoad AutobudgetRoad;
        public AutobudgetPost AutobudgetPost;
        public AutobudgetTaxi AutobudgetTaxi;
        public bool IsCreateControlsOnBudgetPanel = true;

        [XmlIgnore]
        public List<AutobudgetBase> AllAutobudgetObjects = new List<AutobudgetBase>();

        public void Save()
        {
            XmlSerializer ser = new XmlSerializer(typeof(AutobudgetObjectsContainer));
            TextWriter writer = new StreamWriter(getOptionsFilePath());
            ser.Serialize(writer, this);
            writer.Close();
        }

        public void InitObjects()
        {
            if (AutobudgetElectricity == null) AutobudgetElectricity = new AutobudgetElectricity();
            if (AutobudgetWater == null) AutobudgetWater = new AutobudgetWater();
            if (AutobudgetHeating == null) AutobudgetHeating = new AutobudgetHeating();
            if (AutobudgetGarbage == null) AutobudgetGarbage = new AutobudgetGarbage();
            if (AutobudgetHealthcare == null) AutobudgetHealthcare = new AutobudgetHealthcare();
            if (AutobudgetEducation == null) AutobudgetEducation = new AutobudgetEducation();
            if (AutobudgetPolice == null) AutobudgetPolice = new AutobudgetPolice();
            if (AutobudgetFire == null) AutobudgetFire = new AutobudgetFire();
            if (AutobudgetIndustry == null) AutobudgetIndustry = new AutobudgetIndustry();
            if (AutobudgetRoad == null) AutobudgetRoad = new AutobudgetRoad();
            if (AutobudgetPost == null) AutobudgetPost = new AutobudgetPost();
            if (AutobudgetTaxi == null) AutobudgetTaxi = new AutobudgetTaxi();

            AllAutobudgetObjects.Clear();
            AllAutobudgetObjects.Add(AutobudgetElectricity);
            AllAutobudgetObjects.Add(AutobudgetWater);
            AllAutobudgetObjects.Add(AutobudgetHeating);
            AllAutobudgetObjects.Add(AutobudgetGarbage);
            AllAutobudgetObjects.Add(AutobudgetHealthcare);
            AllAutobudgetObjects.Add(AutobudgetEducation);
            AllAutobudgetObjects.Add(AutobudgetPolice);
            AllAutobudgetObjects.Add(AutobudgetFire);
            AllAutobudgetObjects.Add(AutobudgetIndustry);
            AllAutobudgetObjects.Add(AutobudgetRoad);
            AllAutobudgetObjects.Add(AutobudgetPost);
            AllAutobudgetObjects.Add(AutobudgetTaxi);
        }

        public static AutobudgetObjectsContainer CreateFromFile()
        {
            string path = getOptionsFilePath();

            if (!File.Exists(path)) return null;

            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(AutobudgetObjectsContainer));
                TextReader reader = new StreamReader(path);
                AutobudgetObjectsContainer instance = (AutobudgetObjectsContainer)ser.Deserialize(reader);
                reader.Close();

                return instance;
            }
            catch
            {
                return null;
            }
        }

        private static string getOptionsFilePath()
        {
            //return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Colossal Order", "Cities_Skylines", optionsFileName);
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            path = Path.Combine(path, "Colossal Order");
            path = Path.Combine(path, "Cities_Skylines");
            path = Path.Combine(path, optionsFileName);
            return path;
        }
    }
}