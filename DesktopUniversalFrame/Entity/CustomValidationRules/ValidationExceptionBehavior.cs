using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace DesktopUniversalFrame.Entity.CustomValidationRules
{
    /// <summary>
    /// 验证行为类
    /// </summary>
    public class ValidationExceptionBehavior : Behavior<FrameworkElement>
    {
        //错误计数器
        private int validationExceptionCount = 0;

        protected override void OnAttached()
        {
            this.AssociatedObject.AddHandler(Validation.ErrorEvent, new EventHandler<ValidationErrorEventArgs>(this.OnOccuredValidationError));
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        private IValidationExceptionHandle GetValidationHandle()
        {
            if (this.AssociatedObject.DataContext is IValidationExceptionHandle)
                return AssociatedObject.DataContext as IValidationExceptionHandle;

            return null;
        }

        /// <summary>
        /// 验证ValidationError事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOccuredValidationError(object sender, ValidationErrorEventArgs e)
        {
            try
            {
                var handle = GetValidationHandle();
                var element = e.OriginalSource as FrameworkElement;
                if (handle == null || element == null)
                    return;

                if (e.Action == ValidationErrorEventAction.Added)
                    validationExceptionCount++;
                else if (e.Action == ValidationErrorEventAction.Removed)
                    validationExceptionCount--;

                handle.HasValidationError = validationExceptionCount == 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
