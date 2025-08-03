using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
 
public class MinimumAgeAttribute : ValidationAttribute, IClientModelValidator
{
    public int Years { get; }
    public int? MaxYears { get; }
 
    public MinimumAgeAttribute(int years, int maxYears = 80)
    {
        Years = years;
        MaxYears = maxYears;
    }
 
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
            return ValidationResult.Success;
           
        if (value is DateTime dob)
        {
            var today = DateTime.Today;
            var age = today.Year - dob.Year;
            if (dob > today.AddYears(-age)) age--;
 
            if (age < Years)
                return new ValidationResult($"User must be at least {Years} years old.");
            if (MaxYears.HasValue && age > MaxYears)
                return new ValidationResult($"User age must not exceed {MaxYears} years.");
        }
        return ValidationResult.Success;
    }
 
    public void AddValidation(ClientModelValidationContext context)
    {
        MergeAttribute(context.Attributes, "data-val", "true");
        MergeAttribute(context.Attributes, "data-val-minimumage", $"User must be at least {Years} years old.");
        MergeAttribute(context.Attributes, "data-val-minimumage-years", Years.ToString());
    }
 
    private bool MergeAttribute(System.Collections.Generic.IDictionary<string, string> attributes, string key, string value)
    {
        if (attributes.ContainsKey(key)) return false;
        attributes.Add(key, value);
        return true;
    }
}
 