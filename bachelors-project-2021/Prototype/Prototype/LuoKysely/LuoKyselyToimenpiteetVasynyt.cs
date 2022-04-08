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
    public partial class LuoKyselyToimenpiteetVasynyt : ContentPage
    {

        public IList<CollectionItem> Items { get; set; }
        private string myStringProperty;

        public string MyStringProperty
        {
            get { return myStringProperty; }
            set
            {
                myStringProperty = value;
                OnPropertyChanged(nameof(MyStringProperty)); // Notify that there was a change on this property
            }
        }

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

        public LuoKyselyToimenpiteetVasynyt()
        {
            InitializeComponent();

                  //alustus
                Items = new List<CollectionItem>();
                int numero = 0;
                foreach( var individual in SurveyManager.GetInstance().GetSurvey().emojis)
                {
                if(individual.Name== "Väsynyt")
                {
                    Items.Add(new CollectionItem(SurveyManager.GetInstance().GetSurvey().emojis[numero], Const.activities[4]));
                    Console.WriteLine("Emojin nimi:" + new CollectionItem(SurveyManager.GetInstance().GetSurvey().emojis[numero], Const.activities[1]).Emoji.Name);
                    Console.WriteLine("Aktiviteetteja: " + new CollectionItem(SurveyManager.GetInstance().GetSurvey().emojis[numero], Const.activities[1]).ActivityChoises.Count);
                    Console.WriteLine("Emojin aktiviteetit: " + new CollectionItem(SurveyManager.GetInstance().GetSurvey().emojis[numero], Const.activities[1]).ActivityChoises[0]);
                    break;

                }
                else
                {
                    numero++;
                }
            }
            BindingContext = this;

            int selectedEmojis = SurveyManager.GetInstance().GetSurvey().emojis.Count;
            int emojiNumber = numero + 1;
            String title = "Aktiviteetti " +emojiNumber+ "/"+selectedEmojis ;
            MyStringProperty = title;

        }

        async void EdellinenButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();

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
                if (individual.Name == "Väsynyt")
                {
                    SurveyManager.GetInstance().GetSurvey().emojis[numero].activities = tempActivities;
                    break;
                }
                else
                {
                    numero++;
                }
            }
            int nextEmojiNumber = numero + 1;
            int luku = SurveyManager.GetInstance().GetSurvey().emojis.Count;

            if (nextEmojiNumber < luku)
            {
                String name = SurveyManager.GetInstance().GetSurvey().emojis[nextEmojiNumber].Name;

                if (name == "Iloinen")
                {
                    await Navigation.PushAsync(new LuoKyselyToimenpiteetIloinen());
                }
                else if (name == "Hämmästynyt")
                {
                    await Navigation.PushAsync(new LuoKyselyToimenpiteetHammastynyt());
                }
                else if (name == "Neutraali")
                {
                    await Navigation.PushAsync(new LuoKyselyToimenpiteetNeutraali());

                }
                else if (name == "Vihainen")
                {
                    await Navigation.PushAsync(new LuoKyselyToimenpiteetVihainen());

                }
                else if (name == "Väsynyt")
                {
                    await Navigation.PushAsync(new LuoKyselyToimenpiteetVasynyt());

                }
                else if (name == "Miettivä")
                {
                    await Navigation.PushAsync(new LuoKyselyToimenpiteetMiettiva());

                }
                else if (name == "Itkunauru")
                {
                    await Navigation.PushAsync(new LuoKyselyToimenpiteetItkunauru());

                }
                else { }
            }else
                {
                await Navigation.PushAsync(new LuoKyselyLopetus());

            }



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