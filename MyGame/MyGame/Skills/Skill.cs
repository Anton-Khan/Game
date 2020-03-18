using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace MyGame.Skills
{
    abstract class Skill
    {
        private Ellipse _shape;
        public bool isYour; // чей скилл
        public float Radius;
        public Pos center;
        private int damage;
        private bool canDamage = true; 

        public Pos Pos = new Pos();
        public Pos NextPos = new Pos();
        protected Pos Way = new Pos();
        protected float Speed;

        public Ellipse Shape
        {
            get { return _shape; }
            set { _shape = value; }
        }

        public int Damage { get => damage;  set => damage = value; }
        public bool CanDamage { get => canDamage; set => canDamage = value; }

        public void Normalize()
        {
            Way.x = NextPos.x - Pos.x;
            Way.y = NextPos.y - Pos.y;
            Speed = (float)Math.Sqrt(Way.x * Way.x + Way.y * Way.y);
        }

        public abstract bool Move(int a);  
    }
}
