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
    class Bullet : Skill
    {
        
        
        public Bullet(Pos pos, bool IsYour)
        {
            CreateBullet(pos);
            isYour = IsYour;
        }

        private void CreateBullet(Pos pos)
        {
            Shape = new Ellipse();
            Radius = 10;
            Shape.Width = Radius;
            Shape.Height = Radius;
            Radius /= 2;
            Shape.Fill = Brushes.OrangeRed;
            this.Pos.x = pos.x;
            this.Pos.y = pos.y;
            center = new Pos();
            center.x = Pos.x ;
            center.y = Pos.y ;
            Speed = 0.01f;
            Damage = 10;
            Canvas.SetLeft(Shape, pos.x);
            Canvas.SetTop(Shape, pos.y);
        }

        

        public override bool Move(int update)
        {

            if (!((Pos.x < NextPos.x + 5 && Pos.x > NextPos.x - 5) && (Pos.y < NextPos.y + 5 && Pos.y > NextPos.y - 5)))
            {
                Pos.x += Way.x * 1 / Speed * 5f;
                Pos.y += Way.y * 1 / Speed * 5f;
                center.x = Pos.x ;
                center.y = Pos.y ;
                Canvas.SetLeft(Shape, Pos.x - Shape.Width / 2);
                Canvas.SetTop(Shape, Pos.y - Shape.Height / 2);
                return true;
            }
            else
            {
                return false;
            }

        }

    }
}
