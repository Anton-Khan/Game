using MyGame.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MyGame
{
    class Character
    {
        private int _health = 100;
        private Ellipse _person;
        private float radius;

        

        private Ellipse _direction;
        
        private int firstCooldown = 0;
        private int secondCooldown = 0;

        


        private Pos pos = new Pos();
        public Pos nextPos = new Pos();
        private Pos Way = new Pos();
        public Pos center;
        private float Speed;

        public Ellipse Person { get => _person; private set => _person = value; }
        public Ellipse Direction { get => _direction; private set => _direction = value; }
        
        public Pos Pos { get => pos; set => pos = value; }
        public int FirstCooldown { get => firstCooldown;  set => firstCooldown = value; }
        public int SecondCooldown { get => secondCooldown; set => secondCooldown = value; }
        public int Health { get => _health; set => _health = value; }
        public float Radius { get => radius; set => radius = value; }

        public void resetFirstCD() { FirstCooldown = 140; }
        public void resetSecondCD() { SecondCooldown = 280; }

        public Character( Pos pos)
        {
            CreatePerson(pos);
            
        }

        private void CreatePerson(Pos a)
        {
            _person = new Ellipse();
            Radius = 30;
            _person.Width = Radius;
            _person.Height = Radius;
            Radius /= 2;
            _person.Fill = Brushes.Black;
            pos.x = a.x ;
            pos.y = a.y;
            center = new Pos();
            center.x = pos.x + Radius;
            center.y = pos.y + Radius;
            Speed = 0.0f;
            Canvas.SetLeft(_person, pos.x);
            Canvas.SetTop(_person, pos.y);
        }

        public void CreateDirection(Pos dir)
        {
            _direction = new Ellipse();
            _direction.Width = 5;
            _direction.Height = 5;
            _direction.Fill = Brushes.Green;
            
            Canvas.SetLeft(_direction, dir.x - _direction.Width / 2);
            Canvas.SetTop(_direction, dir.y - _direction.Height / 2);
        }

        public void Blink(Pos dir)
        {

            int R = 150;
            Pos OA = new Pos();
            Pos AB = new Pos();
            Pos OB = new Pos();

            OB.x = dir.x;
            OB.y = dir.y;

            OA.x = pos.x;
            OA.y = pos.y;

            AB.x = OB.x - OA.x;
            AB.y = OB.y - OA.y;

            Pos OC = new Pos();
            float BlSpeed = (float)Math.Sqrt(AB.x * AB.x + AB.y * AB.y);
            if (BlSpeed > R) {
                OA.x *= (BlSpeed - R);
                OA.y *= (BlSpeed - R);
                OB.x *= R;
                OB.y *= R;
                OC.x = (OA.x + OB.x) * 1/BlSpeed;
                OC.y = (OA.y + OB.y) * 1/BlSpeed;
            }
            else
            {
                OC.x = OB.x;
                OC.y = OB.y;
            }
            
            pos.x = OC.x;
            pos.y = OC.y;
            Canvas.SetLeft(_person, pos.x - _person.Width / 2);
            Canvas.SetTop(_person, pos.y - _person.Height / 2);
            nextPos.x = dir.x;
            nextPos.y = dir.y;
            
            Normalize();

        }

        public void Normalize()
        {
            Way.x = nextPos.x - pos.x;
            Way.y = nextPos.y - pos.y;
            Speed = (float)Math.Sqrt(Way.x * Way.x + Way.y * Way.y);
        }

        public bool Move(int update)
        {
            if (FirstCooldown != 0)
                FirstCooldown--;
            if (SecondCooldown != 0)
                SecondCooldown--;
            if (!((pos.x < nextPos.x + 1 && pos.x > nextPos.x - 1) && (pos.y < nextPos.y + 1 && pos.y > nextPos.y - 1)))
            {
                if(Speed != 0) { 
                pos.x += Way.x * 1 / Speed * 2f;
                pos.y += Way.y * 1 / Speed * 2f;
                Radius += Way.y * 1 / Speed * 2f;
                center.x = pos.x + Radius;
                center.y = pos.y + Radius;
                Canvas.SetLeft(_person, pos.x - _person.Width / 2);
                Canvas.SetTop(_person, pos.y - _person.Height / 2);
                }
                return true;
            }
            else
            {
                return false;
            }

        }

    }
}
