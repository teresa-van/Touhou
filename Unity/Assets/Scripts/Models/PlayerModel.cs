using System.Runtime.Serialization;

namespace Scripts.Models
{
    public class PlayerModel
    {

        #region Constructors

        public PlayerModel()
            : this(0, "Nickname", "Character", "Role", 4, 1, 1, 4)
        {

        }

        public PlayerModel(int id, string nickname, string character, string role, int health, int range, int distance, int maxHandSize)
        {
            this.ID = id;

            this.Nickname = nickname;
            this.Character = character;
            this.Role = role;
            this.Health = health;
            this.Range = range;
            this.Distance = distance;
            this.MaxHandSize = maxHandSize;
        }

        #endregion

        #region Properties

        [DataMember]
        public int ID { set; get; }

        [DataMember]
        public string Nickname { set; get; }

        [DataMember]
        public string Character { set; get; }

        [DataMember]
        public string Role { set; get; }

        [DataMember]
        public int Health { set; get; }

        [DataMember]
        public int Range { set; get; }

        [DataMember]
        public int Distance { set; get; }

        [DataMember]
        public int MaxHandSize { set; get; }

        #endregion

    }
}
