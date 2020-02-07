using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace IMUNModel
{
    public class School
    {
        public String Name { get; set; }
        public String Password { get; set; }
        public String Address { get; set; }
        public String MUNDirector { get; set; }
        public String SchoolPhone { get; set; }
        public String SchoolFiscalNumber { get; set; }
        public String DirectorPhone { get; set; }
        public String Email { get; set; }
        public int NrOfStudentsGAAndSpecConf { get; set; }
        public int NrOfSecCouncilRequested { get; set; }
        public int NrDirectorsRequested { get; set; }
        public int NrTotalRequests { get; set; }
        public int NrPressRequests { get; set; }
        //public int NrChaperoneRequests { get; set; }
        public int NrICJJudgesRequested { get; set; }
        public int NrICJAdvocatesRequested { get; set; }
        public Boolean IsICJPresident { get; set; }
        public Boolean IsICJRegistrar { get; set; }
        public SchoolStatus Status { get; set; }
        public String LastUserChangingStatus { get; set; }
        
        public School(string root, String name)
        {
            try
            {
                this.Name = name;
                this.Delegations = new List<string>();
                this.LastUserChangingStatus = "";
                this.Status = SchoolStatus.SUBMITTED;
                Directory.CreateDirectory(root + @"\schools\" + name);
            }
            catch (Exception x)
            {
                throw x;
            }
        }

        [JsonProperty]
        private readonly List<String> Delegations;

        public Delegation[] GetDelegationsObj(string root)
        {
            List<Delegation> a = new List<Delegation>();
            foreach (String s in this.Delegations)
            {
                bool? hasEmbassy = Delegation.HasEmbassy(root, s);
                bool isSecurityCouncil = Delegation.GetDelegationsSecurityCouncil(root).Contains(s);
                if (hasEmbassy == true)
                    a.Add(new DelegationWithEmbassy(s, isSecurityCouncil));
                else if (hasEmbassy == false)
                    a.Add(new DelegationWithoutEmbassy(s, isSecurityCouncil));
                else
                    throw new Exception("Delegation not known: " + s);
            }
            return a.ToArray();
        }

        public Delegation[] GetAvailableDelegationsObj(string root)
        {
            List<Delegation> delegations = GetDelegationsObj(root).ToList();
            //foreach (Card c in this.GetCardsList())
            //{
            //    Delegation d = delegations.FirstOrDefault(x => x.Name.Equals(c.Country));
            //    delegations.Remove(d);
            //}
            return delegations.ToArray();
        }

        public Delegation[] GetAvailableDelegationsForCard(string root, Card card)
        {
            List<Delegation> delegations = this.GetAvailableDelegationsObj(root).ToList();
            delegations.Add(Delegation.GetDelegation(root, card.Country));
            return delegations.ToArray();
        }

        public override string ToString()
        {
            return this.Name;
        }

        public void AddDelegation(Delegation delegation)
        {
            this.Delegations.Add(delegation.Name);
        }

        public void RemoveDelegation(Delegation delegation)
        {
            this.Delegations.Remove(delegation.Name);
        }

        public void Save(String root)
        {
            String schoolsPath = root + @"\schools\";

            using (StreamWriter file = File.CreateText(schoolsPath + this.Name + ".json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, this);
            }

            try
            {
                DirectoryInfo di = new DirectoryInfo(schoolsPath + this.Name);
                if (!di.Exists)
                {
                    Directory.CreateDirectory(schoolsPath + this.Name);
                }
            }
            catch(Exception)
            {

            }
        }

        public static void SaveAll(String root, List<School> schools)
        {
            foreach (School s in schools)
                s.Save(root);
        }

        public Card[] GetCardsList(String root)
        {
            try
            {
                List<Card> cards = new List<Card>();
                DirectoryInfo dir = new DirectoryInfo(root + @"\schools\" + this.Name);
                foreach (FileInfo finfo in dir.GetFiles())
                {
                    if (finfo.Extension.Equals(".json"))
                    {
                        //Console.WriteLine(schoolPath + "/" + finfo.Name);
                        using (StreamReader reader = new StreamReader(finfo.FullName))
                        {
                            string json = reader.ReadToEnd();
                            Card c = JsonConvert.DeserializeObject<Card>(json);
                            cards.Add(c);
                        }
                    }
                }
                return cards.OrderBy(x => x.FirstName).ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static School[] GetAllSchools(String root)
        {
            try
            {

                DirectoryInfo dir = new DirectoryInfo(root + @"\schools\");
                FileInfo[] files = dir.GetFiles();
                List<School> list = new List<School>();
                foreach (FileInfo info in files)
                    using (StreamReader reader = new StreamReader(info.FullName))
                    {
                        string json = reader.ReadToEnd();
                        School school = null;
                        if (json.Length > 0)
                        {
                            school = JsonConvert.DeserializeObject<School>(json);
                            list.Add(school);
                        }
                    }
                return list.ToArray();
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException();
            }
        }

        public static School GetSchool(String root, String name)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(root + @"\\schools");
                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo info in files)
                    using (StreamReader reader = new StreamReader(info.FullName))
                    {
                        string json = reader.ReadToEnd();
                        School school = null;
                        if (json.Length > 0)
                        {
                            school = JsonConvert.DeserializeObject<School>(json);
                            if (school.Name.Equals(name))
                                return school;
                        }
                    }
                return null;
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException();
            }
        }

        public void Delete(String root)
        {
            DirectoryInfo dir = new DirectoryInfo(root + @"\schools\" + this.Name);
            FileInfo[] files = dir.GetFiles();
            while (files.Length > 0)
            {
                File.Delete(files[0].FullName);
                files = dir.GetFiles();
            }
            Directory.Delete(dir.FullName);
            File.Delete(root + @"\schools\" + this.Name + ".json");
        }
    }
}