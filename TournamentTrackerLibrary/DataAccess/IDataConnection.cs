using System.Collections.Generic;
using TournamentTrackerLibrary.Models;

namespace TournamentTrackerLibrary.DataAccess
{
    public interface IDataConnection
    {
        PrizeModel CreatePrize(PrizeModel model);
        PersonModel CreatePerson(PersonModel model);
        List<PersonModel> GetPerson_All();
        TeamModel CreateTeam(TeamModel model);
        List<TeamModel> GetTeam_All();
        void CreateTournament(TournamentModel model);
    }
}