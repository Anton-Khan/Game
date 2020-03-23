using MyGame.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyGame
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 

        
        


    public partial class MainWindow : Window
    {
        Character you;
        Character enemy;
        

        List<Skill> skills;

        private Fps fps;
        private Thread thread;
        private Action action;
        private TextBlock FirstSkillCD;
        private TextBlock SecondSkillCD;
        Rectangle firstSkill;
        Rectangle secondSkill;


        public MainWindow()
        {
            InitializeComponent();
            


            you = new Character(new Pos(10, 300));
            field.Children.Add(you.Person);

            enemy = new Character(new Pos(560,300));
            enemy.nextPos.x = enemy.Pos.x;
            enemy.nextPos.y = enemy.Pos.y;
            Network.enemyPos.x = enemy.Pos.x;
            Network.enemyPos.y = enemy.Pos.y;
            field.Children.Add(enemy.Person);

            skills = new List<Skill>();
            CreateHUD();
            

            fps = new Fps();
            fps.ProcessChanged += Fps_ProcessChanged;
            fps.ProcessCompleted += Fps_ProcessCompleted;

            

            this.PreviewKeyUp += Field_KeyUp;
            this.Closed += MainWindow_Closed;
            this.SizeChanged += MainWindow_SizeChanged;
            this.Loaded += MainWindow_Loaded;      

            thread = new Thread(fps.Go);
            thread.Start();
            
            
            
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

            PlayerForm f = new PlayerForm();
            f.Show();
            f.Owner = this;

            f.Closed += F_Closed;
            Network.Start();
            Network.Connect();

        }

        private void F_Closed(object sender, EventArgs e)
        {
            Network.id = (sender as PlayerForm).Id;
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            field.Children.Remove(firstSkill);
            field.Children.Remove(FirstSkillCD);
            field.Children.Remove(secondSkill);
            field.Children.Remove(SecondSkillCD);
            
            CreateHUD();
        }

       

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            fps.Canel();
            thread.Abort();
        }

        private void CreateYourDirection(Pos dir)
        {
            if (field.Children.Contains(you.Direction))
            {
                field.Children.Remove(you.Direction);
            }
            you.CreateDirection(dir);
            field.Children.Add(you.Direction);
            Case();
        }

        private void Field_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            { 
                case Key.S:
                    you.nextPos = you.Pos;
                    if (field.Children.Contains(you.Direction))
                        field.Children.Remove(you.Direction);
                break;
                case Key.Q:
                    
                    if (you.FirstCooldown == 0)
                    {
                        skills.Add(new Bullet(you.Pos, true));
                        field.Children.Add(skills[skills.Count-1].Shape);
                        skills[skills.Count - 1].NextPos.x = (float)Mouse.GetPosition(field).X;
                        skills[skills.Count - 1].NextPos.y = (float)Mouse.GetPosition(field).Y;
                        skills[skills.Count - 1].Normalize();
                        
                        GenerateAction();

                        you.resetFirstCD();
                        
                    }
                break;
                case Key.E:
                    if (you.SecondCooldown == 0)
                    {
                        Skill t = new Blink(you);
                        t.NextPos.x = (float)Mouse.GetPosition(field).X;
                        t.NextPos.y = (float)Mouse.GetPosition(field).Y;
                        skills.Add(t);
                        field.Children.Add(skills[skills.Count - 1].Shape);
                        CreateYourDirection(t.NextPos);
                        GenerateAction();
                        you.resetSecondCD();
                    }
                    break;
                default:
                    break;
            }
        }

        public void Case()
        {
            Network.Send(you.nextPos);
        }

        public void GenerateAction()
        {
            
            
            action = () =>
            {
                Title = you.FirstCooldown.ToString() + " " + you.SecondCooldown.ToString() + " " + you.Health.ToString() + " " + enemy.Health.ToString();
                FirstSkillCD.Text = (you.FirstCooldown/20).ToString();
                SecondSkillCD.Text = (you.SecondCooldown/20).ToString();
                you.Move(1);
                
                if(enemy.nextPos.x != Network.enemyPos.x || enemy.nextPos.y != Network.enemyPos.y)
                {
                    enemy.nextPos.x = Network.enemyPos.x;
                    enemy.nextPos.y = Network.enemyPos.y;
                    enemy.Normalize();
                }
                enemy.Move(1);



                for (int i = 0; i < skills.Count; i++)
                {
                    if(skills[i].GetType() == typeof(Bullet))
                    {
                        if (skills[i].isYour)
                        {
                            if (Lenght(skills[i].center, enemy.center) <= RadSum(skills[i].Radius, enemy.Radius))
                            {
                                if (skills[i].CanDamage)
                                {
                                    enemy.Health -= skills[i].Damage;
                                    skills[i].CanDamage = false;
                                    Ellipse a = new Ellipse();
                                    a.Width = 2;
                                    a.Height = 2;
                                    a.Fill = Brushes.Gray;
                                    Canvas.SetLeft(a, skills[i].center.x);
                                    Canvas.SetTop(a, skills[i].center.y);
                                    field.Children.Add(a);
                                }
                            }  
                        }
                    }else if(skills[i].GetType() == typeof(Explosion))
                    {
                        if (skills[i].isYour)
                        {
                            if (Lenght(skills[i].center, enemy.center) <= RadSum(skills[i].Radius, enemy.Radius))
                            {
                                
                                enemy.Health -= skills[i].Damage;
                                skills[i].CanDamage = false;
                                Ellipse a = new Ellipse();
                                a.Width = 4;
                                a.Height = 4;
                                a.Fill = Brushes.DarkGray;
                                Canvas.SetLeft(a, skills[i].center.x);
                                Canvas.SetTop(a, skills[i].center.y);
                                field.Children.Add(a);
                                
                                
                            }
                        }
                    }


                    if (!skills[i].Move(1))
                    {
                        if (skills[i].GetType() == typeof(Bullet))
                        {
                            field.Children.Remove(skills[i].Shape);
                            Skill t = new Explosion(skills[i].NextPos, true);
                            skills.Add(t);
                            field.Children.Add(t.Shape);
                            skills[i] = null;
                            skills.Remove(skills[i]);
                        }
                        else if (skills[i].GetType() == typeof(Explosion))
                        {
                            field.Children.Remove(skills[i].Shape);
                            skills[i] = null;
                            skills.Remove(skills[i]);
                        }else if(skills[i].GetType() == typeof(Blink))
                        {
                            field.Children.Remove(skills[i].Shape);
                            skills[i] = null; 
                            skills.Remove(skills[i]);
                            
                        }
                        
                    }
                }
                
            };
        }
        
        public void CreateHUD()
        {
            firstSkill = new Rectangle();
            firstSkill.Width = 60;
            firstSkill.Height = 60;
            firstSkill.Stroke = Brushes.Black;
            firstSkill.StrokeThickness = 2;

            firstSkill.Opacity = 90;
            field.Children.Add(firstSkill);
            Canvas.SetLeft(firstSkill, ActualWidth / 2 - 60);
            Canvas.SetBottom(firstSkill, 0);

            FirstSkillCD = new TextBlock();
            FirstSkillCD.Text = you.FirstCooldown.ToString();
            FirstSkillCD.FontSize = 33;
            FirstSkillCD.Foreground = new SolidColorBrush(Colors.Black);
            Canvas.SetLeft(FirstSkillCD, ActualWidth / 2 - 50);
            Canvas.SetBottom(FirstSkillCD, 10);
            field.Children.Add(FirstSkillCD);

            secondSkill = new Rectangle();
            secondSkill.Width = 60;
            secondSkill.Height = 60;
            secondSkill.Stroke = Brushes.Black;
            secondSkill.StrokeThickness = 2;

            secondSkill.Opacity = 90;
            field.Children.Add(secondSkill);
            Canvas.SetLeft(secondSkill, ActualWidth / 2 -1);
            Canvas.SetBottom(secondSkill, 0);


            SecondSkillCD = new TextBlock();
            SecondSkillCD.Text = you.FirstCooldown.ToString();
            SecondSkillCD.FontSize = 33;
            SecondSkillCD.Foreground = new SolidColorBrush(Colors.Black);
            Canvas.SetLeft(SecondSkillCD, ActualWidth / 2 +10);
            Canvas.SetBottom(SecondSkillCD, 10);
            field.Children.Add(SecondSkillCD);

        }

        private void Fps_ProcessCompleted(bool Canceled)
        {
            Action action = () =>
            {
                string message = (!Canceled) ? "Процесс завершен" : "Процесс отменен";
               // MessageBox.Show(message);
            };
            Dispatcher.Invoke(action);
           
        }

        private void Fps_ProcessChanged(int update)
        {
            
            if (action == null)
            {
                GenerateAction();
                Dispatcher.Invoke(action);
                action = null;
            }
            else
            {
                
                Dispatcher.Invoke(action);
                
            }
            
           
        }

        
        
        private void Field_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            if (thread == null )
            {
                you.nextPos.x = (float)Mouse.GetPosition(field).X;
                you.nextPos.y = (float)Mouse.GetPosition(field).Y;
                you.Normalize();
                CreateYourDirection(you.nextPos);
                
            }
            else
            {
                if (!thread.IsAlive)
                {
                    you.nextPos.x = (float)Mouse.GetPosition(field).X;
                    you.nextPos.y = (float)Mouse.GetPosition(field).Y;
                    you.Normalize();
                    CreateYourDirection(you.nextPos);
                    fps.Continue();
                    
                }
                else
                {
                    you.nextPos.x = (float)Mouse.GetPosition(field).X;
                    you.nextPos.y = (float)Mouse.GetPosition(field).Y;
                    you.Normalize();
                    CreateYourDirection(you.nextPos);
                }
            }
        }

        private float Lenght(Pos f, Pos s)
        {
            float result = (float)Math.Sqrt((s.x - f.x) * (s.x - f.x) + (s.y - f.y) * (s.y - f.y));
            return result;
        }

        private float RadSum(float f, float s)
        {
            float result = f + s;
            return result;
        }
        
        
    }

    public class Pos
    {
        public float x;
        public float y;

        public Pos() { }

        public Pos(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return $"{x} {y}";
        }
    }

    public class HUD_EL
    {
        public Rectangle shape;
        public TextBlock text;
        public int CD;

        public HUD_EL()
        {
        }

        
    }
}
