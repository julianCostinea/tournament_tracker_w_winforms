using System.Collections.Generic;
using System.Linq;
using TournamentTrackerLibrary.Models;

namespace TournamentTrackerLibrary.DataAccess.TextConnector
{
    public class TextConnector : IDataConnection
    {
        private const string PrizesFile = "PrizeModels.csv";
        private const string PeopleFile = "PersonModels.csv";
        private const string TeamFile = "TeamModels.csv";

        public PrizeModel CreatePrize(PrizeModel model)
        {
            List<PrizeModel> prizes = PrizesFile.FullFilePath().LoadFile().ConvertToPrizeModels();

            int currentId = prizes.Count == 0 ? 1 : prizes.OrderByDescending(x => x.Id).First().Id + 1;
            model.Id = currentId;

            prizes.Add(model);

            prizes.SaveToPrizeFile(PrizesFile);

            return model;
        }

        public PersonModel CreatePerson(PersonModel model)
        {
            List<PersonModel> people = PeopleFile.FullFilePath().LoadFile().ConvertToPersonModels();

            int currentId = people.Count == 0 ? 1 : people.OrderByDescending(x => x.Id).First().Id + 1;
            model.Id = currentId;

            people.Add(model);

            people.SaveToPersonFile(PeopleFile);

            return model;
        }

        public List<PersonModel> GetPerson_All()
        {
            return PeopleFile.FullFilePath().LoadFile().ConvertToPersonModels();
        }

        public TeamModel CreateTeam(TeamModel model)
        {
            List<TeamModel> teams = TeamFile.FullFilePath().LoadFile().ConvertToTeamModels(PeopleFile);

            int currentId = teams.Count == 0 ? 1 : teams.OrderByDescending(x => x.Id).First().Id + 1;
            
            model.Id = currentId;
            
            teams.Add(model);
            
            teams.SaveToTeamFile(TeamFile);
            
            return model;
        }

        public List<TeamModel> GetTeam_All()
        {
            throw new System.NotImplementedException();
        }
    }
}