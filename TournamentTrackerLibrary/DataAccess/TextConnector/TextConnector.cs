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
        private const string TournamentFile = "TournamentModels.csv";
        private const string MatchupFile = "MatchupModels.csv";
        private const string MatchupEntryFile = "MatchupEntryModels.csv";

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
            return TeamFile.FullFilePath().LoadFile().ConvertToTeamModels(PeopleFile);
        }

        public void CreateTournament(TournamentModel model)
        {
            List<TournamentModel> tournaments = TournamentFile.FullFilePath().LoadFile().ConvertToTournamentModels(TeamFile, PeopleFile, PrizesFile);
            
            int currentId = tournaments.Count == 0 ? 1 : tournaments.OrderByDescending(x => x.Id).First().Id + 1;
            
            model.Id = currentId;
            
            model.SaveRoundsToFile(MatchupFile, MatchupEntryFile);
            
            tournaments.Add(model);
            
            tournaments.SaveToTournamentFile(TournamentFile);
            
        }

        public void UpdateMatchup(MatchupModel model)
        {
            throw new System.NotImplementedException();
        }

        public List<TournamentModel> GetTournament_All()
        {
            return TournamentFile.FullFilePath().LoadFile().ConvertToTournamentModels(TeamFile, PeopleFile, PrizesFile);
        }
    }
}