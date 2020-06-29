using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows;
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
                //if cancelling requested
                if (backgroundWorker.CancellationPending)
                {
                    //setting property to true
                    e.Cancel = true;
                    //passing zero to reset progress percentage
                    backgroundWorker.ReportProgress(0);
                    return;
                }
                using (StreamWriter writer = new StreamWriter(file))
                {
                    writer.WriteLine(Text);
                }
                //invoking method that raises ProgressChanged event and passing the percentage of processing that is complete
                backgroundWorker.ReportProgress(Convert.ToInt32(result * i));
            }
        }
        /// <summary>
        /// This method updates user interface element with the progress made so far.
        /// </summary>
        /// <param name="sender">Object.</param>
        /// <param name="e">ProgressChangedEventArgs object.</param>
        public void BW_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //setting value of user interface elements
            Percent = e.ProgressPercentage;
            Message = e.ProgressPercentage.ToString() + "%";
        }
        /// <summary>
        /// This method update user interface element to show state of document printing.
        /// </summary>
        /// <param name="sender">Object.</param>
        /// <param name="e">RunWorkerCompletedEventArgs object.</param>
        public void BW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //if printing cancelled
            if (e.Cancelled)
            {
                Message = "Printing cancelled.";
            }
            //if some error occurred during document printing
            else if (e.Error != null)
            {
                Message = e.Error.Message.ToString();
            }
            //if printing successfully finished
            else
            {
                Message = "Printing completed.";
            }
        }
        /// <summary>
        /// This method checks if user input data is valid.
        /// </summary>
        /// <returns>True if data is valid, false if not.</returns>
        public bool CanStartExecute()
        {
            try
            {
                //checking if input for text to print and number of documents copies is empty, and if input for number of copies is not positive number
                //if condition is true, returns false and printing is disabled
                if (String.IsNullOrEmpty(Text) || String.IsNullOrEmpty(NumberOfCopies) || !Int32.TryParse(NumberOfCopies, out int copies) || copies <= 0)
                {
                    return false;
                }
                //if condition is false, returns true and printing is enabled
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }
        /// <summary>
        /// This method checks if printing is already started. If not started, runs documents printing.
        /// </summary>
        public void StartExecute()
        {
            try
            {
                //checking if printing is already running
                if (backgroundWorker.IsBusy)
                {
                    //id condition is true, display notification to user
                    MessageBox.Show("Printing is in progress, please wait.");
                }
                //if condition is not true, runs printing
                else
                {
                    backgroundWorker.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        /// <summary>
        /// This method canceling background operation of documents printing.
        /// </summary>
        public void CancelExecute()
        {
            //cancelling documents printing
            backgroundWorker.CancelAsync();
        }
        /// <summary>
        /// This method checks if document printing can be canceled.
        /// </summary>
        /// <returns>True if canceling is possible, false if not.</returns>
        public bool CanCancelExecute()
        {
            //if printing is running returns true and canceling is enabled
            if (backgroundWorker.IsBusy)
            {
                return true;
            }
            //if condition is not true, returns false and calceling is disabled
            else
            {
                return false;
            }
        }
    }
}
