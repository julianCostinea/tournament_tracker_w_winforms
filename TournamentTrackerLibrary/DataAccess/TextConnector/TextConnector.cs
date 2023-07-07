﻿using System.Collections.Generic;
using System.Linq;
using TournamentTrackerLibrary.Models;

namespace TournamentTrackerLibrary.DataAccess.TextConnector
{
    public class TextConnector : IDataConnection
    {
        private const string PrizesFile = "PrizeModels.csv";
        private const string PeopleFile = "PersonModels.csv";
        
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
    }
}