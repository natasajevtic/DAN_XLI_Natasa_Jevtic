using System.ComponentModel;
using System.Windows.Input;
using Zadatak_1.Commands;

namespace Zadatak_1.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        MainWindow main;
        //instantiation of BackgroundWorker class and setting properties of this object to true
        BackgroundWorker backgroundWorker = new BackgroundWorker()
        {
            WorkerReportsProgress = true,
            WorkerSupportsCancellation = true
        };
        //private field and property which are bonded with user interface element for input number of copies of documents
        private string numberOfCopies;
        public string NumberOfCopies
        {
            get
            {
                return numberOfCopies;
            }
            set
            {
                numberOfCopies = value;
                OnPropertyChanged("NumberOfCopies");
            }
        }
        //private field and property which are bonded with user interface element for input text to be printed
        private string text;
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                OnPropertyChanged("Text");
            }
        }
        //private field and property which are bonded with value of user interface element for displaying percent of progress
        private int percent;
        public int Percent
        {
            get { return this.percent; }
            set
            {
                this.percent = value;
                OnPropertyChanged("Percent");
            }
        }
        //private field and property which are bounded with user interface element for feedback of progress
        private string message;
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
                OnPropertyChanged("Message");
            }
        }
        //private field and property which are bounded with user interface element for starting process of document printing
        private ICommand start;
        public ICommand Start
        {
            get
            {
                if (start == null)
                {
                    start = new RelayCommand(param => StartExecute(), param => CanStartExecute());
                }
                return start;
            }
        }
        //private field and property which are bounded with user interface element for canceling process of document printing
        private ICommand cancel;
        public ICommand Cancel
        {
            get
            {
                if (cancel == null)
                {
                    cancel = new RelayCommand(param => CancelExecute(), param => CanCancelExecute());
                }
                return cancel;
            }
        }
        /// <summary>
        /// Constructor with parameter.
        /// </summary>
        /// <param name="main"></param>
        public MainWindowViewModel(MainWindow main)
        {
            this.main = main;
            //adding method to DoWork event
            backgroundWorker.DoWork += DoWork;
            //adding method to ProgressChanged event
            backgroundWorker.ProgressChanged += ProgressChanged;
            //adding method to RunWorkerCompleted event
            backgroundWorker.RunWorkerCompleted += RunWorkerCompleted;
        }
        public void DoWork(object sender, DoWorkEventArgs e)
        {
            
        }
        public void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            
        }

        public void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
           
        }
        public bool CanStartExecute()
        {
            return false;
        }

        public void StartExecute()
        {
            
        }

        public void CancelExecute()
        {
            
        }

        public bool CanCancelExecute()
        {
            return false;
        }
    }
}
