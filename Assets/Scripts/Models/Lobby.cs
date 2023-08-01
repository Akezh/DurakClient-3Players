using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DurakServer;

namespace Assets.Scripts.Models
{
    public class Lobby
    {
        public int Id { get; set; }
        public DurakNetPlayer iPlayer { get; set; }
        public List<DurakNetPlayer> enemyPlayers { get; set; }
        public List<Card> DeckList { get; set; }
        public Card TrumpBox { get; set; }
    }
}
