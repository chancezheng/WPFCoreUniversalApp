using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace DesktopUniversalFrame.Common
{
    /// <summary>
    /// 3D动画视图
    /// </summary>
    public class AnimationView 
    {
        /***
         * Anmimation3D动画说明
         * WPF中是通过逆时针环绕的，以正方体为例：正面对应角度=>0,左侧面=>90D,反面=>180D,右侧面=>270
         * 希望大家不要入坑了
         * ***/

        /// <summary>
        /// 开启Rotation3DAnimation
        /// </summary>
        /// <param name="control"></param>
        public static void JoinRotation3DAnimation(Control control, double toAngle)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.Duration = TimeSpan.FromSeconds(3);
            //doubleAnimation.From = fromAngle;
            doubleAnimation.To = toAngle;
            AxisAngleRotation3D axisAngleRotation3D = control.FindName("loginAxisAngleRotation3D") as AxisAngleRotation3D;
            axisAngleRotation3D.BeginAnimation(AxisAngleRotation3D.AngleProperty, doubleAnimation);
        }

        /// <summary>
        /// 开启DoubleAnimationClock
        /// </summary>
        /// <param name="control"></param>
        public static void JoinDoubleAnimation(Control control)
        {
            double roateAngle = 180D;
            DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames = new DoubleAnimationUsingKeyFrames();
            Duration duration = new Duration(TimeSpan.FromSeconds(3));
            DiscreteDoubleKeyFrame discreteDoubleKeyFrame = new DiscreteDoubleKeyFrame(roateAngle);
            doubleAnimationUsingKeyFrames.Duration = duration;
            doubleAnimationUsingKeyFrames.KeyFrames.Add(discreteDoubleKeyFrame);
            AnimationClock clock = doubleAnimationUsingKeyFrames.CreateClock();
            control.ApplyAnimationClock(Control.RenderTransformProperty, clock);
        }
    }
}
