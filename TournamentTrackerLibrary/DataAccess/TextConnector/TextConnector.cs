using System.Collections.Generic;
using TournamentTrackerLibrary.Models;

namespace TournamentTrackerLibrary.DataAccess.TextConnector
{
    public class TextConnector : IDataConnection
    {
        private const string PrizesFile = "PrizeModels.csv";
        
        public PrizeModel CreatePrize(PrizeModel model)
        {
            List<PrizeModel> prizes = PrizesFile.FullFilePath().LoadFile().ConvertToPrizeModels();
            model.Id = 1;
            return model;
        }
    }
}