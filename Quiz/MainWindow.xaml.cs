using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Quiz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string abc = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
        string answer;
        string MyAnswer;
        Question question;
        TextBox[] tbArray;
        Button[] btnArray;

        static MongoClient client = new MongoClient();
        static IMongoDatabase database = client.GetDatabase("QuestionsDB");
        static IMongoCollection<Question> collection = database.GetCollection<Question>("Questions");

        public MainWindow()
        {
            InitializeComponent();
            LoadQuestion();
            LoadTextBox();
            LoadButtons();
            
        }

        void LoadQuestion()
        {
            MyAnswer = string.Empty;
            List<Question> list = collection.AsQueryable().ToList<Question>();
            if (question != null)
            {
                list.Remove(question);
            }
            Random rnd= new Random();
            int num = rnd.Next(list.Count);
            label_Question.Content = list[num].Question_Name;
            answer = list[num].Answer;
        }

        void LoadTextBox()
        {
            TB_Panel.Children.Clear();
            tbArray = new TextBox[answer.Length];
            char[] b = new char[answer.Length];
            for (int i = 0; i < answer.Length; i++)
                b[i] = answer[i];
            for (int i= 0; i < b.Length; i++)
            {
                TextBox tb = new TextBox();
                tb.Width= 50;
                tb.Height= 50;
                tb.Background = Brushes.Black;
                tb.FontSize = 18;
                tb.FontWeight = FontWeights.Bold;
                tb.Padding= new Thickness(14);
                tb.IsEnabled= false;
                TB_Panel.Children.Add(tb);
                tbArray[i] = tb;
            }
        }

        void LoadButtons()
        {
            Buttons_Panel.Children.Clear();
            btnArray = new Button[42];
            Random rnd = new Random();
            char[] b = new char[abc.Length];
            for (int i = 0; i < abc.Length; i++)
                b[i] = abc[i];
            char[] c = new char[answer.Length];
            for (int i = 0; i < answer.Length; i++)
                c[i] = answer[i];
            
            for (int i = 0; i < btnArray.Length; i++)
            {
                Button btn = new Button();
                btn.Width = 25;
                btn.Height = 25;
                btn.Click += Button_Click;
                btnArray[i] = btn;
            }
            for (int i = 0; i < c.Length; i++)
            {
                int num = rnd.Next(btnArray.Length);
                if (btnArray[num].Content == null)
                {
                    btnArray[num].Content = c[i];
                }
                else
                {
                    i--;
                }              
            }
            for (int i = 0; i < btnArray.Length; i++)
            {
                if (btnArray[i].Content == null)
                {
                    btnArray[i].Content = b[rnd.Next(b.Length)];
                }
            }
            for (int i = 0; i < btnArray.Length;++i)
            {
                Buttons_Panel.Children.Add(btnArray[i]);
            }
        }

        void EraseAll()
        {
            for (int i = 0; i < tbArray.Length; i++)
            {
                tbArray[i].Text = null;
                tbArray[i].Background = Brushes.Black;
            }
            for (int i = 0; i < btnArray.Length; i++)
            {
                if (btnArray[i].Visibility == Visibility.Hidden)
                {
                    btnArray[i].Visibility = Visibility.Visible;
                }
            }
            MyAnswer = string.Empty;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {         
            for (int i = 0; i < tbArray.Length; i++)
            {
                if (tbArray[i].Text == string.Empty)
                {
                    tbArray[i].Text = (sender as Button).Content.ToString();
                    tbArray[i].Background = Brushes.White;
                    MyAnswer += (sender as Button).Content.ToString();
                    (sender as Button).Visibility = Visibility.Hidden;
                    return;
                }
            }
        }

        private void btn_Answer_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < tbArray.Length; i++)
            {
                if (tbArray[i].Text == string.Empty)
                {
                    MessageBox.Show("Заполните все ячейки!");
                    return;
                }
            }

            if (MyAnswer == answer)
            {
                MessageBox.Show("Победа!");
                question = collection.Find(x => x.Question_Name == label_Question.Content.ToString()).FirstOrDefault();
                LoadQuestion();
                LoadTextBox();
                LoadButtons();
            }

            else
            {
                MessageBox.Show("Неправильный ответ!");
                EraseAll();
            }
        }

        private void btn_Erase_Click(object sender, RoutedEventArgs e)
        {
            EraseAll();
        }
    }
}
