using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MyGame.Skills
{
    class Blink : Skill
    {
        private Character character;
        private int life = 30;
        private int delay;
        private bool jump = true;

        public Blink(Character c)
        {
            CreateBlink(c.Pos);
            this.character = c;
        }

        private void CreateBlink(Pos pos)
        {
            Shape = new Ellipse();
            Radius = 10;
            Shape.Width = Radius;
            Shape.Height = Radius;
            Radius /= 2;
            Shape.Fill = Brushes.WhiteSmoke;
            this.Pos.x = pos.x;
            this.Pos.y = pos.y;
            center = new Pos((float)(pos.x - Shape.Width / 2), (float)(Pos.y - Shape.Height / 2));
            Speed = 0.01f;
            //delay = life / 5;
            Canvas.SetLeft(Shape, Pos.x);
            Canvas.SetTop(Shape, Pos.y);
        }

        public override bool Move(int a)
        {
            if (jump )//&& life < delay)
            {
                character.Blink(NextPos);
                jump = false;
            }
            
            if (life > 0)
            {
                Shape.Width += 2;
                Shape.Height += 2;
                Radius += 2;
                Canvas.SetLeft(Shape, Pos.x - Shape.Width / 2);
                Canvas.SetTop(Shape, Pos.y - Shape.Height / 2);
                life--;
                return true;
            }else
                return false;
        }
    }
}
