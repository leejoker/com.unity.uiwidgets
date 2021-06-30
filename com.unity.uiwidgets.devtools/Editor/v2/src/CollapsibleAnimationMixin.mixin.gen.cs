using System;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.DevTools.inspector;
using Unity.UIWidgets.widgets;

namespace Unity.UIWidgets.DevTools
{
            public abstract class CollapsibleAnimationMixinTickerProviderStateMixin<T> : TickerProviderStateMixin<T> where T : StatefulWidget {
        
            AnimationController expandController;
                          
            Animation<float> expandArrowAnimation;
          
            Animation<float> expandCurve;

            bool shouldShow()
            {
              return isExpanded;
            }

            void onExpandChanged(bool expanded)
            {
              
            }

            public bool isExpanded
            {
              get;
            }
          
            public override void initState() {
              base.initState();
              expandArrowAnimation = new CurvedAnimation(curve: Curves.easeInOutCubic, parent: new AnimationController(
                duration: new TimeSpan(0,0,0,0,200),
                vsync: this,
                value: 0.0f
              ));
            }
            
          
            
            public override void dispose() {
              expandController.dispose();
              base.dispose();
            }
          
            public void setExpanded(bool expanded) {
              setState(() => {
                if (expanded) {
                  expandController.forward();
                } else {
                  expandController.reverse();
                }
                onExpandChanged(expanded);
              });
            }
            
            public override void didUpdateWidget(StatefulWidget oldWidget) {
              base.didUpdateWidget(oldWidget);
              if (isExpanded) {
                expandController.forward();
              } else {
                expandController.reverse();
              }
            }
            
        }

}