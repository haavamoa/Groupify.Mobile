﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Groupify.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Overview
    {
        public Overview()
        {
            InitializeComponent();
        }

        private void BackdropMainView_BackClicked(object sender, EventArgs e)
        {

        }
    }
}