using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Scripts.Models
{
    public class Card
    {
         #region Constructors

        public Card()
            : this("N/A", "Username", new List<string> { "N/A" })
        {

        }

        public Card(string name,
            string season, List<string> types)
        {
            this.Name = name;

            this.Season = season;
            this.Types = types;
        }

        #endregion

        #region Properties

        [DataMember]
        public string Name { set; get; }

        [DataMember]
        public string Season { set; get; }

        [DataMember]
        public List<string> Types { set; get; }

        #endregion
    }
}
