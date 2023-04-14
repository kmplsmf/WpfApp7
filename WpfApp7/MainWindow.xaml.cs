using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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

namespace WpfApp7
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public static class NoteFunc
    {
        public static int GetID()
        {
            int id = -1;
            List<Note> notes = GetNotes();
            foreach (Note note in notes)
            {
                if (note.id >= id) id = note.id + 1;
            }
            return id;
        }
        public static List<Note> GetNotes()
        {
            if (!File.Exists("notes.json"))
            {
                JSON.serialize("notes.json", new List<Note>());
            }
            List<Note> notes = JSON.deserialize<List<Note>>("notes.json");
            return notes;
        }
        public static void NewNote(Note note)
        {
            List<Note> notes = GetNotes();
            notes.Add(note);
            JSON.serialize("notes.json", notes);
        }
        public static void DeleteNote(Note note)
        {
            List<Note> notes = GetNotes();
            int id = 0;
            foreach (Note note1 in notes)
            {
                if (note.id == note1.id)
                {
                    notes.Remove(note1);
                    break;
                }

            }
            JSON.serialize("notes.json", notes);
        }
        public static void EditNote(Note note1, Note note2)
        {
            List<Note> notes = GetNotes();
            int id = 0;
            foreach (Note note in notes)
            {
                if (note.id == note1.id) break;
                id++;
            }
            notes[id] = note2;
            JSON.serialize("notes.json", notes);
        }
        public static List<Note> GetTodayNotes(DateTime? date)
        {
            List<Note> notes = GetNotes();
            List<Note> rr = new List<Note>();
            for (int i = 0; i < notes.Count; i++)
            {
                if (notes[i].GetDate().Date == date.Value)
                {
                    rr.Add(notes[i]);
                }
            }
            return rr;
        }
    }
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            kalendar.SelectedDate = DateTime.Now.Date;
            Update();
        }
        private void Update(object sender = null, RoutedEventArgs e = null)
        {
            noteGrid.ItemsSource = NoteFunc.GetTodayNotes(kalendar.SelectedDate);
            typeinput.ItemsSource = JSON.deserialize<List<string>>("types.json");
            double itog = 0;
            foreach (Note note in NoteFunc.GetNotes())
            {
                if (note.incoming)
                    itog += note.price;
                else
                    itog -= note.price;
            }
            summa.Text = "Сумма: " + itog;
        }

        private void createNote(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = nameinput.Text;
                double count = Convert.ToInt32(countinput.Text);
                if (typeinput.SelectedIndex == -1) return;
                string type = typeinput.SelectedItem.ToString();
                if (name != null && name != "" && count != null && type != null && type != "")
                {
                    Note note = new Note(NoteFunc.GetID(), name, type, count, kalendar.SelectedDate.Value);
                    NoteFunc.NewNote(note);
                }
                Update();
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void editNote(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = nameinput.Text;
                double count = Convert.ToInt32(countinput.Text);
                if (typeinput.SelectedIndex == -1) return;
                string type = typeinput.SelectedItem.ToString();
                if (name != null && name != "" && count != null && type != null && type != "")
                {
                    Note note = new Note(NoteFunc.GetID(), name, type, count, kalendar.SelectedDate.Value);
                    NoteFunc.EditNote((Note)noteGrid.SelectedItem, note);
                }
                Update();
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void deleteNote(object sender, RoutedEventArgs e)
        {
            if (typeinput.SelectedIndex == -1) return;
            NoteFunc.DeleteNote((Note)noteGrid.SelectedItem);
            Update();
        }

        private void createType(object sender, RoutedEventArgs e)
        {
            Window1 sozdanieTypaWindow = new Window1();
            sozdanieTypaWindow.ShowDialog();
            if (sozdanieTypaWindow.typename != "")
            {
                if (!File.Exists("types.json"))
                {
                    JSON.serialize("types.json", new List<string>());
                }
                List<string> types = JSON.deserialize<List<string>>("types.json");
                if (types == null)
                    types = new List<string>();
                types.Add(sozdanieTypaWindow.typename);
                JSON.serialize("types.json", types);
                typeinput.ItemsSource = types;
            }
        }

        private void noteGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (noteGrid.SelectedIndex >= 0)
            {
                Note note = noteGrid.SelectedItem as Note;
                nameinput.Text = note.name;
                if (!note.incoming) note.price = -note.price;
                countinput.Text = note.price.ToString();
                typeinput.SelectedItem = note.type;
            }
        }
    }
}
