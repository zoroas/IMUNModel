using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using System.IO;

namespace IMUNModel
{
        public class DelegationWithEmbassy : Delegation
        {
            public DelegationWithEmbassy(string name, bool isSecurityCouncil) : base(name, isSecurityCouncil)
            {
            }
        }

        public class DelegationWithoutEmbassy : Delegation
        {
            public DelegationWithoutEmbassy(string name, bool isSecurityCouncil) : base(name, isSecurityCouncil)
            {
            }
        }

        public class Delegation
        {
            public String Name { get; set; }
            public Boolean IsSecurityCouncil { get; set; }

            //private static String Root
            //{
            //    get { return ""; } //HttpContext.Current.Server.MapPath("~"); }
            //}

            public Delegation(string name) : this(name, false)
            {

            }

            public Delegation(string name, Boolean isSecurityCouncil)
            {
                this.Name = name;
                this.IsSecurityCouncil = isSecurityCouncil;
            }

            public override string ToString()
            {
                return this.Name;
            }

            public static Delegation[] GetAllObjDelegations(string root)
            {
                return GetObjDelegationsWithEmbassy(root).Union(GetObjDelegationsWithoutEmbassy(root)).OrderBy(x => x.Name).ToArray();
            }

            public static Delegation GetDelegation(string root, String name)
            {
                String[] securityCouncil = GetDelegationsSecurityCouncil(root);
                bool isSecurityCouncil = securityCouncil.Contains(name);
                if (GetDelegationNamesWithEmbassy(root).Contains(name))
                    return new DelegationWithEmbassy(name, isSecurityCouncil);
                if (GetDelegationNamesWithoutEmbassy(root).Contains(name))
                    return new DelegationWithoutEmbassy(name, isSecurityCouncil);
                return null;
            }

            public static String[] GetDelegationsSecurityCouncil(string root)
            {
                String[] result = null;
                try
                {
                    String pathWithout = root + @"\delegations\DelegationsSecurityCouncil.txt";
                    String[] delegations = new String[0];
                    using (StreamReader sr = new StreamReader(pathWithout))
                    {
                        String text = sr.ReadToEnd();
                        delegations = text.Split(';').Select(x => x.Trim()).ToArray();
                    }
                    return delegations;
                }
                catch (Exception)
                {
                    Console.WriteLine("Error reading Sec. Council delegations file. Please contact application administrator.");
                }
                return result;
            }

            public static Delegation[] GetObjDelegationsWithoutEmbassy(string root)
            {
                List<Delegation> a = new List<Delegation>();
                String[] securityCouncil = GetDelegationsSecurityCouncil(root);
                String[] delegationsWithoutEmbassy = GetDelegationNamesWithoutEmbassy(root);
                foreach (String s in delegationsWithoutEmbassy)
                {
                    bool isSecurityCouncil = securityCouncil.Contains(s);
                    a.Add(new DelegationWithoutEmbassy(s, isSecurityCouncil));
                }
                return a.ToArray();
            }

            public static Delegation[] GetObjDelegationsWithEmbassy(string root)
            {
                List<Delegation> a = new List<Delegation>();
                String[] securityCouncil = GetDelegationsSecurityCouncil(root);
                String[] delegationsWithEmbassy = GetDelegationNamesWithEmbassy(root);
                foreach (String s in delegationsWithEmbassy)
                {
                    bool isSecurityCouncil = securityCouncil.Contains(s);
                    a.Add(new DelegationWithEmbassy(s, isSecurityCouncil));
                }
                return a.ToArray();
            }

            public static String[] GetAllDelegationNames(string root)
            {
                return GetDelegationNamesWithEmbassy(root).Union(GetDelegationNamesWithoutEmbassy(root)).OrderBy(x => x).ToArray();
            }

            public static Delegation[] GetAvailableObjDelegations(string root)
            {
                List<Delegation> assignedDelegations = new List<Delegation>();
                foreach (School s in School.GetAllSchools(root))
                {
                    assignedDelegations.AddRange(s.GetDelegationsObj(root));
                }

                List<Delegation> allDelegations = GetAllObjDelegations(root).ToList();
                foreach (Delegation s in assignedDelegations)
                {
                    Delegation d = allDelegations.FirstOrDefault(x => x.Name.Equals(s.Name));
                    if (d != null)
                    {
                        allDelegations.Remove(d);
                    }
                }
                return allDelegations.ToArray();
            }

            private static String[] GetDelegationNamesWithoutEmbassy(string root)
            {
                String[] result = null;
                try
                {
                    String pathWithout = root + @"\delegations\DelegationsWithoutEmbassy.txt";
                    String[] delegationsWithout = new String[0];
                    using (StreamReader sr = new StreamReader(pathWithout))
                    {
                        String text = sr.ReadToEnd();
                        delegationsWithout = text.Split(';').Select(x => x.Trim()).ToArray();
                    }
                    return delegationsWithout;
                }
                catch (Exception)
                {
                    Console.WriteLine("Error reading delegations file. Please contact application administrator.");
                }
                return result;
            }

            private static String[] GetDelegationNamesWithEmbassy(string root)
            {
                String[] result = null;
                try
                {
                    String pathWith = root + @"\delegations\DelegationsWithEmbassy.txt";
                    String[] delegationsWith = new String[0];
                    using (StreamReader sr = new StreamReader(pathWith))
                    {
                        String text = sr.ReadToEnd();
                        delegationsWith = text.Split(';').Select(x => x.Trim()).ToArray();
                    }
                    return delegationsWith;
                }
                catch (Exception)
                {
                    Console.WriteLine("Error reading delegations file. Please contact application administrator.");
                }
                return result;
            }

            public static bool? HasEmbassy(string root, String delegation)
            {
                if (GetDelegationNamesWithEmbassy(root).Contains(delegation))
                    return true;
                if (GetDelegationNamesWithoutEmbassy(root).Contains(delegation))
                    return false;
                return null;
            }

        }

}
