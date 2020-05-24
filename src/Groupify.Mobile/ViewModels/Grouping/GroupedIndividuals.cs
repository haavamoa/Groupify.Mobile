using System;
using System.Collections.Generic;
using System.Text;
using Groupify.Mobile.Models;

namespace Groupify.Mobile.ViewModels.Grouping
{
    public class GroupedIndividuals : List<Individual>
    {
        public GroupedIndividuals(string name, List<Individual> individuals)
        {
            Name = name;
            AddRange(individuals);
        }

        public string Name { get; }
    }
}
