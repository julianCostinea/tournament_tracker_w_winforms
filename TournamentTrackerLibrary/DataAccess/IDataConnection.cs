using System.Collections.Generic;
using TournamentTrackerLibrary.Models;

namespace TournamentTrackerLibrary.DataAccess
{
    public interface IDataConnection
    {
        void CreatePrize(PrizeModel model);
        void CreatePerson(PersonModel model);
        List<PersonModel> GetPerson_All();
        void CreateTeam(TeamModel model);
        List<TeamModel> GetTeam_All();
        void CreateTournament(TournamentModel model);
        void UpdateMatchup(MatchupModel model);
        List<TournamentModel> GetTournament_All();
    }
}