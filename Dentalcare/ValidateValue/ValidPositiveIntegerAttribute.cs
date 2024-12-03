using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
public class ValidPositiveIntAttribute : ValidationAttribute
{
    public ValidPositiveIntAttribute() : base("Giá trị phải là số nguyên và không nhỏ hơn 0.")
    {
    }

    public override bool IsValid(object value)
    {
        if (value == null)
        {
            return true; // Allow null value (for required check)
        }

        var stringValue = value.ToString();

        // Check if the price contains a dot
        if (stringValue.Contains("."))
        {
            return false; // Invalid if it contains a dot
        }

        // Check if the price is a valid number and greater than or equal to 0
        if (decimal.TryParse(stringValue, out decimal result))
        {
            return result >= 0; // Invalid if less than 0
        }

        return false; // Invalid if the value cannot be parsed as a number
    }
}
