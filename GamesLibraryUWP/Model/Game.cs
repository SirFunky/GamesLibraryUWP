using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesLibraryUWP.Model
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gener { get; set; }
        public int NumberOfPlayers { get; set; }
        public Publisher Publisher { get; set; }
        public List<Studio> Studio { get; set; }
        public List<Developer> Developer { get; set; }
        public virtual ICollection<GameDeveloper> GameDevelopers { get; set; }
    }
}
