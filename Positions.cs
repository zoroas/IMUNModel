using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalMUNManager.model
{
    public static class Positions
    {
        public static String[] GetAll()
        {
            return new String[] {
                "General Assembly Delegate",   // delegate
                "Special Conference Delegate", // delegate
                "Security Council Delegate",   // delegate
                "ICJ Advocate",                // delegate
                "ICJ Judge",                   // delegate
                "ICJ President",               // officer
                "Press",                       // press
                "Chaperone",                   // director
                "Director",                    // director
                "Secretary General",           // officer
                "Deputy Secretary General",    // officer
                "Special Conference President",// officer
                "Special Conference VP",       // officer
                "Security Council President",  // officer
                "Security Council VP",         // officer
                "Administrative Staff Head",   // officer
                "Editor in Chief",             // officer
                "Admin Staff",                 // admin
            };
        }

        // doesn't have a delegation
        public static bool IsSpecialPosition(string position)
        {
            string[] special = new String[] {
                "ICJ Advocate",     
                "ICJ Judge",        
                "ICJ President",  
                "Press",           
                "Chaperone",      
                "Director",       
                "Secretary General",                
                "Deputy Secretary General",         
                "Special Conference President",     
                "Special Conference VP",            
                "Security Council President",     
                "Security Council VP",            
                "Administrative Staff Head",      
                "Editor in Chief",                 
                "Admin Staff",     
            };
            foreach (String s in special) {
                if (position.Equals(s))
                    return true;
            }
            return false;
        }

    }
}
