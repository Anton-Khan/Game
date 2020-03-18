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
    class Explosion : Skill
    {
        private int life;

        public Explosion(Pos pos, bool IsYour)
        {
            CreateExplosion(pos);
            life = 20;
            isYour = IsYour;
        }

        private void CreateExplosion(Pos pos)
        {
            Shape = new Ellipse();
            Radius = 10;
            Shape.Width = Radius;
            Shape.Height = Radius;
            Radius /= 2;
            Shape.Fill = Brushes.Red;
            this.Pos.x = pos.x;
            this.Pos.y = pos.y;
            center = new Pos((float)(pos.x - Shape.Width / 2), (float)(Pos.y - Shape.Height / 2));
            Speed = 0.01f;
            Damage = 1;
            Canvas.SetLeft(Shape, pos.x);
            Canvas.SetTop(Shape, pos.y);
        }

        public override bool Move(int a)
        {
            if (life > 0)
            {
                Shape.Width += 2;
                Shape.Height += 2;
                Radius += 2;
                Canvas.SetLeft(Shape, Pos.x - Shape.Width / 2);
                Canvas.SetTop(Shape, Pos.y - Shape.Height / 2);
                life--;
                if (life < 5)
                    Damage = 2;
                return true;
            }
            else
            {
                return false;
            }
           
        }
    }
}
