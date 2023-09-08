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

        public bool IsAnimationReset { get; private set; } = true;

        public ButtonWidthAnimation(Action<double> callback, double start, double end)
        {
            _startAnimation = new Animation(callback, start, end);
            _resetAnimation = new Animation(callback, end, start);
        }

        public void Start(IAnimatable owner, string name, uint rate = 16U, uint length = 250U, Easing easing = null) 
        {
            _startAnimation.Commit(owner, name, rate, length, easing);
            IsAnimationReset = false;
        }

        public void Reset(IAnimatable owner, string name, uint rate = 16U, uint length = 250U, Easing easing = null,
            Action<double, bool> finished = null)
        {
            IsAnimationReset = true;
            _resetAnimation.Commit(owner, name, rate, length, easing, finished);
        }
    }
}
