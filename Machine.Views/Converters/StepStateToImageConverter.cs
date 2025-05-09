﻿using Machine.Steps.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Machine.Views.Converters
{
    [ContentProperty("Values")]
    class StepStateToImageConverter : IValueConverter
    {
        public List<StepStateToImageConverterItem> Values { get; set; } = new List<StepStateToImageConverterItem>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object result = null;

            if (value is StepState pt)
            {
                foreach (var item in Values)
                {
                    if (item.When == pt)
                    {
                        result = item.Then;
                        break;
                    }
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [ContentProperty("Then")]
    class StepStateToImageConverterItem
    {
        public StepState When { get; set; }
        public object Then { get; set; }
    }
}
