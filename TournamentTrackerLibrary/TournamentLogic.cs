using System;
using System.Collections.Generic;
using System.Linq;
using TournamentTrackerLibrary.Models;

namespace TournamentTrackerLibrary
{
    public static class TournamentLogic
    {
        public static void CreateRounds(TournamentModel model)
        {
            List<TeamModel> randomizedTeams = RandomizeTeamOrder(model.EnteredTeams);
            int rounds = FindNumberOfRounds(randomizedTeams.Count);
            int byes = NumberOfByes(rounds, randomizedTeams.Count);

            model.Rounds.Add(CreateFirstRound(byes, randomizedTeams));

            CreateOtherRounds(model, rounds);

            UpdateTournamentResults(model);
        }

        private static void CreateOtherRounds(TournamentModel model, int rounds)
        {
            int round = 2;
            
            List<MatchupModel> previousRound = model.Rounds[0];
            
            while (round <= rounds)
            {
                List<MatchupModel> currentRound = new List<MatchupModel>();
                
                foreach (MatchupModel matchup in previousRound)
                {
                    MatchupModel newMatchup = new MatchupModel();
                    
                    foreach (MatchupEntryModel entry in matchup.Entries)
                    {
                        newMatchup.Entries.Add(new MatchupEntryModel {ParentMatchup = entry});
                    }
                    
                    currentRound.Add(newMatchup);
                }
                
                model.Rounds.Add(currentRound);
                round += 1;
                previousRound = currentRound;
            }
        }

        private static List<MatchupModel> CreateFirstRound(int byes, List<TeamModel> teams)
        {
            List<MatchupModel> output = new List<MatchupModel>();
            MatchupModel currentMatchup = new MatchupModel();

            foreach (TeamModel team in teams)
            {
                currentMatchup.Entries.Add(new MatchupEntryModel { TeamCompeting = team });
                //byes get assigned first so we can get rid of them 
                // byes > 0 runs first, and a new entry with only on team is created, then byes is reduced by 1
                if (byes > 0 || currentMatchup.Entries.Count > 1)
                {
                    currentMatchup.MatchupRound = 1;
                    output.Add(currentMatchup);
                    currentMatchup = new MatchupModel();

                    if (byes > 0)
                    {
                        byes -= 1;
                    }
                }
            }

            return output;
        }

        private static int NumberOfByes(int rounds, int numberOfTeams)
        {
            int output = 0;
            int totalTeams = 1;

            for (int i = 1; i <= rounds; i++)
            {
                totalTeams *= 2;
            }

            output = totalTeams - numberOfTeams;

            return output;
        }

        private static int FindNumberOfRounds(int randomizedTeamsCount)
        {
            int output = 1;
            int val = 2;

            while (val < randomizedTeamsCount)
            {
                output += 1;
                val *= 2;
            }

            return output;
        }

        private static List<TeamModel> RandomizeTeamOrder(List<TeamModel> teams)
        {
            return teams.OrderBy(x=> Guid.NewGuid()).ToList();
        }
    }
}