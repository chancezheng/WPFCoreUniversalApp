using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection.Metadata;
using System.Text;
using System.Windows.Controls;

namespace DesktopUniversalFrame.Entity.CustomValidationRules
{
    /// <summary>
    /// 文本值验证
    /// </summary>
    public class TextValidationRule : ValidationRule
    {
        public TextValidationRule()
        {
           
        }


        private ValidationType _validationType = ValidationType.Int;
        private int _minLength;
        private int _maxLength;
        private int _minValue;
        private int _maxValue;
        private string _errorMessage;

        public ValidationType ValidationType
        {
            get { return _validationType; }
            set { _validationType = value; }
        }
     
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }

        public int MinLength
        {
            get { return _minLength; }
            set { _minLength = value; }
        }
        public int MaxLength
        {
            get { return _maxLength; }
            set { _maxLength = value; }
        }


        public int MinValue
        {
            get { return _minValue; }
            set { _minValue = value; }
        }
        public int MaxValue
        {
            get { return _maxValue; }
            set { _maxValue = value; }
        }


        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || string.IsNullOrEmpty(value as string))
                return new ValidationResult(false, "不能为空");
            try
            {
                if (ValidationType == ValidationType.Int)
                {
                    int inputValue = int.Parse((string)value);
                    if (!GetCheckResult(ValidationType, inputValue))
                        return new ValidationResult(false, string.Format("取值范围为{0}～{1}", MinValue, MaxValue));
                }
                else if (ValidationType == ValidationType.Double)
                {
                    double inputValue = double.Parse((string)value);
                    if (!GetCheckResult(ValidationType, inputValue))
                        return new ValidationResult(false, string.Format("取值范围为{0}～{1}", MinValue, MaxValue));
                }
                else if (ValidationType == ValidationType.String)
                {
                    string inputStr = (string)value;
                    if (!GetCheckResult(ValidationType, inputStr))
                        return new ValidationResult(false, string.Format("长度范围为{0}～{1}", MinLength, MaxLength));
                }
            }
            catch (Exception ex)
            {
                return new ValidationResult(false, ex.Message);
            }
            return new ValidationResult(true, null);
        }

        //检验数据
        private bool GetCheckResult(ValidationType validationType, object value)
        {
            bool isRight = false;

            switch (validationType)
            {
                case ValidationType.String:
                    if ((value as string).Length < MinLength || (value as string).Length > MaxLength)
                        isRight = false;
                    else
                        isRight = true;
                    break;
                case ValidationType.Int:
                    {
                        if ((int)value < MinValue || (int)value > MaxValue)
                            isRight = false;
                        else
                            isRight = true;                            
                    }
                    break;
                case ValidationType.Double:
                    {
                        if ((double)value <  MinValue|| (double)value > MaxValue)
                            isRight = false;
                        else
                            isRight = true;
                    }
                    break;
                default:
                    break;
            }

            return isRight;
        }
    }
}
