using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMUNModel
{
    public class Card
    {
        public String Filename { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String School { get; set; }
        public String Forum { get; set; }
        public String Country { get; set; }
        public String PictureUserName { get; set; }
        public String TimeStamp { get; set; }
        public bool IsChairman { get; set; }
        public bool HasOpeningSpeach { get; set; }

        [JsonIgnore]
        public String IsChairmanString { 
            get
            {
                return this.IsChairman ? "Yes" : "No";
            }
        }

        [JsonIgnore]
        public String HasOpeningSpeachString
        {
            get
            {
                return this.HasOpeningSpeach ? "Yes" : "No";
            }
        }


        [JsonIgnore]
        public bool IsPress
        {
            get
            {
                return this.Forum.Equals("Press");
            }
        }

        [JsonIgnore]
        public String CountryToString
        {
            get
            {
                return this.Country + (this.IsPress ? " (Press)" : "") + (this.IsChairman ? " (Chair)" : "");
            }
        }


        [JsonIgnore]
        public String PictureInternalName
        {
            get
            {
                return this.TimeStamp + this.PictureUserName;
            }
        }

        public String PicturePath(String home)
        {
                return home + @"schools\" + this.School + @"\" + this.PictureInternalName;
        }

        [JsonIgnore]
        public String Fullname
        {
            get
            {
                return this.FirstName + " " + this.LastName;
            }
        }



        public static Card GetCard(string schoolPath, string firstName, string lastName)
        {
            List<Card> cards = GetCardsListFrom(schoolPath);
            return cards.Where(x => x.FirstName.ToUpper().Equals(firstName.ToUpper()) &&
                                    x.LastName.ToUpper().Equals(lastName.ToUpper())).FirstOrDefault();
        }

        public void Delete(String root)
        {
            try
            {
                List<Card> cards = new List<Card>();
                DirectoryInfo schoolDir = new DirectoryInfo(root + @"\schools\" + this.School);

                string jsonFile = this.Filename;
                string picturefile = this.PictureInternalName;

                FileInfo info = new FileInfo(schoolDir + @"\" + jsonFile);
                File.Delete(info.FullName);
                info = new FileInfo(schoolDir + @"\" + picturefile);
                File.Delete(info.FullName);
            }
            catch(Exception)
            {

            }
        }

        public static List<Card> GetCardsListFrom(String schoolPath)
        {
            List<Card> cards = new List<Card>();
            DirectoryInfo dir = new DirectoryInfo(schoolPath);
            foreach (FileInfo finfo in dir.GetFiles())
            {
                if (finfo.Extension.Equals(".json"))
                {
                    using (StreamReader reader = new StreamReader(finfo.FullName))
                    {
                        string json = reader.ReadToEnd();
                        Card c = JsonConvert.DeserializeObject<Card>(json);
                        cards.Add(c);
                    }
                }
            }
            return cards.OrderBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
        }
        
        /// <summary>
        /// returns the initial of last name or an empty string if there is no name
        /// </summary>
        [JsonIgnore]
        public String LastNameInitial
        {
            get
            {
                return this.LastName.Length > 0 ? "" + this.LastName[0] : "";
            }
        }

        [JsonIgnore]
        public bool IsGeneralAssemblyDelegate
        {
            get
            {
                return this.Forum.Equals("General Assembly Delegate");
            }
        }

        [JsonIgnore]
        public bool IsSpecialConferenceDelegate
        {
            get
            {
                return this.Forum.Equals("Special Conference Delegate");
            }
        }

        [JsonIgnore]
        public bool IsSecurityCouncilDelegate
        {
            get
            {
                return this.Forum.Equals("Security Council Delegate");
            }
        }

        [JsonIgnore]
        public bool IsDirector
        {
            get
            {
                return this.Forum.Equals("Director");
            }
        }

        //[JsonIgnore]
        //public bool IsChaperone
        //{
        //    get
        //    {
        //        return this.Forum.Equals("Chaperone");
        //    }
        //}

        [JsonIgnore]
        public bool IsICJAdvocate
        {
            get
            {
                return (this.Forum.Equals("ICJ Advocate"));
            }
        }

        [JsonIgnore]
        public bool IsICJJudge
        {
            get
            {
                return (this.Forum.Equals("ICJ Judge"));
            }
        }

        [JsonIgnore]
        public bool IsICJPresident
        {
            get
            {
                return (this.Forum.Equals("ICJ President"));
            }
        }

        [JsonIgnore]
        public bool IsAdmin
        {
            get
            {
                return this.Forum.Equals("Admin Staff");
            }
        }

        [JsonIgnore]
        public bool IsOfficer // just for badges
        {
            get
            {
                return this.Forum.Equals("ICJ President") ||
                    this.Forum.Equals("Secretary General") ||
                    this.Forum.Equals("Deputy Secretary General") ||
                    this.Forum.Equals("Special Conference President") ||
                    this.Forum.Equals("Special Conference VP") ||
                    this.Forum.Equals("Security Council President") ||
                    this.Forum.Equals("Security Council VP") ||
                    this.Forum.Equals("Administrative Staff Head") ||
                    this.Forum.Equals("Editor in Chief")
                    ;
            }
        }

        [JsonIgnore]
        public bool IsDelegate // just for badges
        {
            get
            {
                return this.Forum.Equals("General Assembly Delegate") ||
                    this.Forum.Equals("Special Conference Delegate") ||
                    this.Forum.Equals("Security Council Delegate") ||
                    this.Forum.Equals("ICJ Advocate") ||
                    this.Forum.Equals("ICJ Judge")
                    ;
            }
        }

        [JsonIgnore]
        public bool IsCertificateDelegate // just for badges
        {
            get
            {
                return this.Forum.Equals("General Assembly Delegate") ||
                    this.Forum.Equals("Special Conference Delegate") ||
                    this.Forum.Equals("Security Council Delegate");
            }
        }
        
        [JsonIgnore]
        public String ServerPicturePath
        {
            get
            {
                return @"~\data\schools\" + this.School + @"\" + this.PictureInternalName;
            }
        }

        [JsonIgnore]
        public String LocalPicturePath
        {
            get
            {
                return ApplicationSettings.LocalRoot + @"\schools\" + this.School + @"\" + this.PictureInternalName;
            }
        }

        public Card() { }

        public Card(School school, string first, string last)
        {
            this.TimeStamp = DateTime.Now.ToString("yyyyMMddhhmmss");
            this.FirstName = first;
            this.LastName = last;
            this.School = school.Name;
            String serverRoot = ApplicationSettings.WebServerRoot;
            Directory.CreateDirectory(serverRoot + @"schools/" + school.Name);
        }

        public void Save(String schoolsFolder)
        {
            this.Filename = this.TimeStamp + this.FirstName + this.LastName + ".json";
            using (StreamWriter file = File.CreateText(schoolsFolder + @"/" +  this.Filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, this);
            }
        }

        public static Card[] GetAllCards(String root)
        {
            List<Card> cards = new List<Card>();
            foreach (School school in IMUNModel.School.GetAllSchools(root))
            {
                cards.AddRange(school.GetCardsList(root));
            }
            return cards.ToArray();
        }
    }
}
