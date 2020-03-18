using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyGame
{
    class Fps
    {
        private bool _cancelled = false;

        public void Canel()
        {
            _cancelled = true;
        }
        public void Continue()
        {
            _cancelled = false;
        }

        public void Go()
        {
            while(true)
            {
                if (_cancelled == true)
                    break;
                Thread.Sleep(6);
                ProcessChanged(1);

            }

            ProcessCompleted(_cancelled);
        }

        public event Action<int> ProcessChanged;
        public event Action<bool> ProcessCompleted;

    }
}
