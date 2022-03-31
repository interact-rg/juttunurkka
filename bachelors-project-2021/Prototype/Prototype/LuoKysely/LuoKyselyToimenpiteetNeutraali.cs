﻿
/*
Copyright 2021 Emma Kemppainen, Jesse Huttunen, Tanja Kultala, Niklas Arjasmaa

This file is part of "Mieliala kysely".

Mieliala kysely is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, version 3 of the License.

Mieliala kysely is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Mieliala kysely.  If not, see <https://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Prototype
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LuoKyselyToimenpiteetNeutraali : ContentPage
    {

        public IList<CollectionItem> Items { get; set; }
        public class CollectionItem

        {
            public Emoji Emoji { get; set; }
            public IList<string> ActivityChoises { get; set; }
            public ObservableCollection<object> Selected { get; set; }
            public CollectionItem(Emoji emoji, IList<string> activities)
            {
                Emoji = emoji;
                ActivityChoises = activities;
               
                Selected = new ObservableCollection<object>();
                foreach (var item in emoji.activities)
                {
                    Selected.Add(ActivityChoises[ActivityChoises.IndexOf(item)]);
                }
            }
        }

        public LuoKyselyToimenpiteetNeutraali()
        {
            InitializeComponent();

                  //alustus
                Items = new List<CollectionItem>();
                int numero = 0;
                foreach( var individual in SurveyManager.GetInstance().GetSurvey().emojis)
                {
                if(individual.Name== "Neutraali")
                {
                    Items.Add(new CollectionItem(SurveyManager.GetInstance().GetSurvey().emojis[numero], Const.activities[2]));
                    Console.WriteLine("Emojin nimi:" + new CollectionItem(SurveyManager.GetInstance().GetSurvey().emojis[numero], Const.activities[2]).Emoji.Name);
                    Console.WriteLine("Aktiviteetteja: " + new CollectionItem(SurveyManager.GetInstance().GetSurvey().emojis[numero], Const.activities[2]).ActivityChoises.Count);
                    Console.WriteLine("Emojin aktiviteetit: " + new CollectionItem(SurveyManager.GetInstance().GetSurvey().emojis[numero], Const.activities[2]).ActivityChoises[0]);


                }
                else
                {
                    numero++;
                }
            }
            BindingContext = this;
        }

 
        async void EdellinenButtonClicked(object sender, EventArgs e)
        {
            var res = await DisplayAlert("Tahdotko varmasti keskeyttää kyselyn tekemisen?", "", "Kyllä", "Ei");

            if (res == true)
            {
                //survey resetoidaan
                SurveyManager.GetInstance().ResetSurvey();

                //Jos ollaan edit tilassa, niin siirrytään takaisin kyselyntarkastelu sivulle, muutoin main menuun
                if (Main.GetInstance().GetMainState() == Main.MainState.Editing)
                {
                    Main.GetInstance().BrowseSurveys();
                    await Navigation.PopAsync();
                }
                else
                {
                    // siirrytään etusivulle
                    await Navigation.PopToRootAsync();
                }

            }
            else return;
        }


        void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is CollectionView cv && cv.SelectionChangedCommandParameter is CollectionItem item)
            {

            }
        }


        async void JatkaButtonClicked(object sender, EventArgs e)
        {
            
			//error if not all emojis have at least 1 selected activity
/*			if (!ActivitiesSet())
			{
                await DisplayAlert("Kaikkia valintoja ei ole tehty", "Sinun on valittava jokaiselle mielialalle vähintään yksi aktiviteetti", "OK");
                return;
            }
*/
            //asetetaan emojit survey olioon
            List<Emoji> tempEmojis = new List<Emoji>();
            List<string> tempActivities = new List<string>();

            foreach (var item in Items)
            {
                foreach (var selection in item.Selected)
                {
                    Console.WriteLine("Lisätään aktiviteetti:" +selection as string);
                    Console.WriteLine(item.Emoji.Name +"emojin yhteyteen");
                    tempActivities.Add(selection as string);
                }
                item.Emoji.activities = tempActivities;
                tempEmojis.Add(item.Emoji);
            }
            int numero = 0;
            foreach (var individual in SurveyManager.GetInstance().GetSurvey().emojis)
            {
                if (individual.Name == "Neutraali")
                {
                    SurveyManager.GetInstance().GetSurvey().emojis[numero].activities = tempActivities;
                    break;
                }
                else
                {
                    numero++;
                }
            }

            // siirrytään "Luo kysely -lopetus" sivulle 
//            if(SurveyManager.GetInstance().GetSurvey().emojis[numero +1].Name == "Hämmästy")
            await Navigation.PushAsync(new LuoKyselyLopetus()); ;
        }
/*
        //function which checks whether the user has selected at least 1 activity for each emoji.
        private bool ActivitiesSet()
        {
            foreach (var item in Items)
            {
                if (item.Selected.Count == 0)
                {
                    return false;
                }
            }

            return true;
        }*/
    }
}