﻿using System.Collections.Generic;

namespace TournamentTrackerLibrary.Models
{
    public class TeamModel
    {
        public int Id { get; set; }
        public string TeamName { get; set; }
        public List<PersonModel> TeamMembers { get; set; } = new List<PersonModel>();
    }
}