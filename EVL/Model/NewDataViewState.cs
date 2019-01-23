using System.Collections.ObjectModel;
using System.ComponentModel;
using EVL.Utils;

namespace EVL.Model
{

    public interface IReadOnlyNewDataViewState : INotifyPropertyChanged
    {
        ReadOnlyObservableCollection<MetricQuestionAnswer> MetricQA { get; }
        ReadOnlyObservableCollection<CharacteristicQuestionAnswer> CharacteristicQA { get; }
        ReadOnlyObservableCollection<ClientRatingQuestionAnswer> ClientRatingQA { get; }

        double ClientLoyalty { get; set; }
        double PriorClientLoyalty { get; set; }
        int SegmentID { get; set; }
    }

    public class NewDataViewState : IReadOnlyNewDataViewState
    {
        public readonly ObservableCollection<MetricQuestionAnswer> MetricQA;
        public readonly ObservableCollection<CharacteristicQuestionAnswer> CharacteristicQA;
        public readonly ObservableCollection<ClientRatingQuestionAnswer> RatingQA;

        private double clientLoyalty;
        public double ClientLoyalty
        {
            get { return clientLoyalty; }
            set
            {
                clientLoyalty = value;
                OnPropertyChanged(nameof(ClientLoyalty));
            }
        }

        private double priorClientLoyalty;
        public double PriorClientLoyalty
        {
            get { return priorClientLoyalty; }
            set
            {
                priorClientLoyalty = value;
                OnPropertyChanged(nameof(ClientLoyalty));
            }
        }

        private int segmentID;
        public int SegmentID
        {
            get { return segmentID; }
            set
            {
                segmentID = value;
                OnPropertyChanged(nameof(SegmentID));
            }
        }

        public int ProjectID { get; set; }
        
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        
        ReadOnlyObservableCollection<MetricQuestionAnswer> IReadOnlyNewDataViewState.MetricQA => MetricQA.AsReadOnly();
        ReadOnlyObservableCollection<CharacteristicQuestionAnswer> IReadOnlyNewDataViewState.CharacteristicQA => CharacteristicQA.AsReadOnly();
        ReadOnlyObservableCollection<ClientRatingQuestionAnswer> IReadOnlyNewDataViewState.ClientRatingQA => RatingQA.AsReadOnly();

        public NewDataViewState(int projectID)
        {
            this.MetricQA = new ObservableCollection<MetricQuestionAnswer>();
            this.CharacteristicQA = new ObservableCollection<CharacteristicQuestionAnswer>();
            this.RatingQA = new ObservableCollection<ClientRatingQuestionAnswer>();
            this.ProjectID = projectID;
            this.clientLoyalty = -1;
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
