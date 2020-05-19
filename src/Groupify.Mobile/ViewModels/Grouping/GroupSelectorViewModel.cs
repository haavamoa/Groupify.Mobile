using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Groupify.Mobile.Abstractions;
using Groupify.Mobile.Models;
using Groupify.Mobile.ViewModels.Grouping.Abstractions;

namespace Groupify.Mobile.ViewModels.Grouping
{
    public class GroupSelectorViewModel : IGroupState
    {

        public event PropertyChangedEventHandler PropertyChanged;

        internal void RandomizeGroup(int numberOfIndividualsInGroup, Group group)
        {
           
        }
    }
}
