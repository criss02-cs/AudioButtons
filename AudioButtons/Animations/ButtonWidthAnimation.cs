using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioButtons.Animations
{
    public class ButtonWidthAnimation
    {
        private Animation _startAnimation;
        private Animation _resetAnimation;

        public ButtonWidthAnimation(Action<double> callback, double start, double end)
        {
            _startAnimation = new Animation(callback, start, end);
            _resetAnimation = new Animation(callback, end, start);
        }

        public void Start(IAnimatable owner, string name, uint rate = 16U, uint length = 250U, Easing easing = null) 
        {
            _startAnimation.Commit(owner, name, rate, length, easing);
        }

        public void Reset(IAnimatable owner, string name, uint rate = 16U, uint length = 250U, Easing easing = null)
        {
            _resetAnimation.Commit(owner, name, rate, length, easing);
        }
    }
}
