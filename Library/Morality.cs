using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class ListMorality
    {
        public List<Morality> data { get; set; }
        public string date { get; set; }
    }
    public class Morality: INotifyPropertyChanged
    {
        string Moralities="",Health="",Bangkeun_Phal="";
        public string number { get; set; }
        public string name { get; set; }
        public string gender { get; set; }
        public string avg_score { get; set; }
        public int rank { get; set; }
        public string grading { get; set; }
        public string id { get; set; }
        public string profile { get; set; }
        public string morality 
        {
            get
            {
                return Moralities;
            }
            set
            {
                Moralities = value;
                OnpropertyChange();
            }
        }
        public string bangkeun_phal 
        {
            get
            {
                return Bangkeun_Phal;
            }
            set
            {
                Bangkeun_Phal = value;
                OnpropertyChange();
            }
        }
        public string health
        {
            get
            {
                return Health;
            }
            set
            {
                Health = value;
                OnpropertyChange();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnpropertyChange([CallerMemberName] string caller = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }
    }
}
