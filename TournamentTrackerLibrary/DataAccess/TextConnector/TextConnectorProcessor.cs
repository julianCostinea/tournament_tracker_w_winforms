using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using TournamentTrackerLibrary.Models;

namespace TournamentTrackerLibrary.DataAccess.TextConnector
{
    public static class TextConnectorProcessor
    {
        public static string FullFilePath(this string fileName) // PrizeModels.csv
        {
            // C:\Users\jason\source\repos\TournamentTracker\TournamentTracker\PrizeModels.csv
            return $"{ConfigurationManager.AppSettings["filePath"]}\\{fileName}";
        }
        
        public static List<string> LoadFile(this string file)
        {
            if (!File.Exists(file))
            {
                return new List<string>();
            }
            
            return File.ReadAllLines(file).ToList();
        }
        
        public static List<PrizeModel> ConvertToPrizeModels(this List<string> lines)
        {
            List<PrizeModel> output = new List<PrizeModel>();
            
            foreach (string line in lines)
            {
                string[] cols = line.Split(',');
                
                PrizeModel p = new PrizeModel();
                p.Id = int.Parse(cols[0]);
                p.PlaceNumber = int.Parse(cols[1]);
                p.PlaceName = cols[2];
                p.PrizeAmount = decimal.Parse(cols[3]);
                p.PrizePercentage = double.Parse(cols[4]);
                
                output.Add(p);
            }
            
            return output;
        }
        
        public static List<PersonModel> ConvertToPersonModels(this List<string> lines)
        {
            List<PersonModel> output = new List<PersonModel>();
            
            foreach (string line in lines)
            {
                string[] cols = line.Split(',');
                
                PersonModel p = new PersonModel();
                p.Id = int.Parse(cols[0]);
                p.FirstName = cols[1];
                p.LastName = cols[2];
                p.EmailAddress = cols[3];
                p.CellphoneNumber = cols[4];
                
                output.Add(p);
            }
            
            return output;
        }
        
        public static List<TeamModel> ConvertToTeamModels(this List<string> lines, string peopleFileName)
        {
            // id,team name,list of ids separated by the pipe
            // 3,Tim's Team,1|3|5
            List<TeamModel> output = new List<TeamModel>();
            List<PersonModel> people = peopleFileName.FullFilePath().LoadFile().ConvertToPersonModels();
            
            foreach (string line in lines)
            {
                string[] cols = line.Split(',');
                
                TeamModel t = new TeamModel();
                t.Id = int.Parse(cols[0]);
                t.TeamName = cols[1];
                
                string[] personIds = cols[2].Split('|');
                
                foreach (string id in personIds)
                {
                    t.TeamMembers.Add(people.Where(x => x.Id == int.Parse(id)).First());
                }
                
                output.Add(t);
            }
            
            return output;
        }
        
        public static List<TournamentModel> ConvertToTournamentModels(this List<string> lines, string teamFileName, string peopleFileName, string prizeFileName)
        {
            // id,tournament name,entry fee,(id|id|id - entered teams),(id|id|id - prizes),(id^id^id|id^id^id|id^id^id - rounds)
            // 4,Sample Tournament,5000,1|3|5,1|3,1^3^5|3^5^7|5^7^9
            List<TournamentModel> output = new List<TournamentModel>();
            List<TeamModel> teams = teamFileName.FullFilePath().LoadFile().ConvertToTeamModels(peopleFileName);
            List<PrizeModel> prizes = prizeFileName.FullFilePath().LoadFile().ConvertToPrizeModels();
            
            foreach (string line in lines)
            {
                string[] cols = line.Split(',');
                
                TournamentModel tm = new TournamentModel();
                tm.Id = int.Parse(cols[0]);
                tm.TournamentName = cols[1];
                tm.EntryFee = decimal.Parse(cols[2]);
                
                string[] teamIds = cols[3].Split('|');
                
                foreach (string id in teamIds)
                {
                    tm.EnteredTeams.Add(teams.Where(x => x.Id == int.Parse(id)).First());
                }
                
                if (cols[4].Length > 0)
                {
                    string[] prizeIds = cols[4].Split('|');
                    
                    foreach (string id in prizeIds)
                    {
                        tm.Prizes.Add(prizes.Where(x => x.Id == int.Parse(id)).First());
                    }
                }
                
                // Capture rounds information
                // string[] rounds = cols[5].Split('|');
                //
                // foreach (string round in rounds)
                // {
                //     string[] ms = round.Split('^');
                //     List<MatchupModel> mm = new List<MatchupModel>();
                //     
                //     foreach (string matchupModelTextId in ms)
                //     {
                //         mm.Add(GlobalConfig.MatchupFile.FullFilePath().LoadFile().Where(x => x.Id == int.Parse(matchupModelTextId)).First());
                //     }
                //     
                //     tm.Rounds.Add(mm);
                // }
                
                output.Add(tm);
            }
            
            return output;
        }

        public static void SaveToPrizeFile(this List<PrizeModel> models, string filename)
        {
            List<string> lines = new List<string>();
            
            foreach (PrizeModel p in models)
            {
                lines.Add($"{p.Id},{p.PlaceNumber},{p.PlaceName},{p.PrizeAmount},{p.PrizePercentage}");
            }
            
            File.WriteAllLines(filename.FullFilePath(), lines);
        }
        
        public static void SaveToPersonFile(this List<PersonModel> models, string filename)
        {
            List<string> lines = new List<string>();
            
            foreach (PersonModel p in models)
            {
                lines.Add($"{p.Id},{p.FirstName},{p.LastName},{p.EmailAddress},{p.CellphoneNumber}");
            }
            
            File.WriteAllLines(filename.FullFilePath(), lines);
        }
        
        public static void SaveToTeamFile(this List<TeamModel> models, string filename)
        {
            List<string> lines = new List<string>();
            
            foreach (TeamModel t in models)
            {
                lines.Add($"{t.Id},{t.TeamName},{ConvertPeopleListToString(t.TeamMembers)}");
            }
            
            File.WriteAllLines(filename.FullFilePath(), lines);
        }
        
        public static void SaveToTournamentFile(this List<TournamentModel> models, string filename)
        {
            List<string> lines = new List<string>();
            
            foreach (TournamentModel tm in models)
            {
                lines.Add($"{tm.Id},{tm.TournamentName},{tm.EntryFee},{ConvertTeamListToString(tm.EnteredTeams)},{ConvertPrizeListToString(tm.Prizes)},{ConvertRoundListToString(tm.Rounds)}");
            }
            
            File.WriteAllLines(filename.FullFilePath(), lines);
        }

        private static string ConvertRoundListToString(List<List<MatchupModel>> tmRounds)
        {
            string output = "";
            
            if (tmRounds.Count == 0)
            {
                return "";
            }
            
            foreach (List<MatchupModel> r in tmRounds)
            {
                output += $"{ConvertMatchupListToString(r)}|";
            }
            
            output = output.Substring(0, output.Length - 1);
            
            return output;
        }

        private static string ConvertMatchupListToString(List<MatchupModel> matchupModels)
        {
            string output = "";
            
            if (matchupModels.Count == 0)
            {
                return "";
            }
            
            foreach (MatchupModel m in matchupModels)
            {
                output += $"{m.Id}^";
            }
            
            output = output.Substring(0, output.Length - 1);
            
            return output;
        }

        private static string ConvertPrizeListToString(List<PrizeModel> tmPrizes)
        {
            string output = "";
            
            if (tmPrizes.Count == 0)
            {
                return "";
            }
            
            foreach (PrizeModel p in tmPrizes)
            {
                output += $"{p.Id}|";
            }
            
            output = output.Substring(0, output.Length - 1);
            
            return output;
        }

        private static string ConvertTeamListToString(List<TeamModel> teams)
        {
            string output = "";
            
            if (teams.Count == 0)
            {
                return "";
            }
            
            foreach (TeamModel t in teams)
            {
                output += $"{t.Id}|";
            }
            
            output = output.Substring(0, output.Length - 1);
            
            return output;
        }
        
        private static string ConvertPeopleListToString(List<PersonModel> people)
        {
            string output = "";
            
            if (people.Count == 0)
            {
                return "";
            }
            
            foreach (PersonModel p in people)
            {
                output += $"{p.Id}|";
            }
            
            output = output.Substring(0, output.Length - 1);
            
            return output;
        }
    }
}