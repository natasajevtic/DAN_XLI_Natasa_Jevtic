using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
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
            backgroundWorker.DoWork += BW_DoWork;
            //adding method to ProgressChanged event
            backgroundWorker.ProgressChanged += BW_ProgressChanged;
            //adding method to RunWorkerCompleted event
            backgroundWorker.RunWorkerCompleted += BW_RunWorkerCompleted;
        }
        /// <summary>
        /// This method performs documents printing.
        /// </summary>
        /// <param name="sender">Object.</param>
        /// <param name="e">DoWorkEventArgs object.</param>
        public void BW_DoWork(object sender, DoWorkEventArgs e)
        {
            double number = Double.Parse(NumberOfCopies);
            double result = 100 / number;
            //creating files - copies of document and writing text in them
            for (int i = 1; i <= number; i++)
            {
                Thread.Sleep(1000);
                string file = string.Format(@"../../{0}.{1}_{2}_{3}_{4}_{5}.txt", i, DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Hour, DateTime.Now.Minute);
                using (StreamWriter writer = new StreamWriter(file))
                {
                    writer.WriteLine(Text);
                }
                //invoking method that raises ProgressChanged event and passing the percentage of processing that is complete
                backgroundWorker.ReportProgress(Convert.ToInt32(result * i));
                //if cancelling requested
                if (backgroundWorker.CancellationPending)
                {
                    //setting property to true
                    e.Cancel = true;
                    //passing zero to reset progress percentage
                    backgroundWorker.ReportProgress(0);
                    return;
                }
            }
        }
        public void BW_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            
        }

        public void BW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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
