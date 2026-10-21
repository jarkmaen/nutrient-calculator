using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs;

public class NutrientResponseDto
{
    // Macros
    [Required]
    public decimal Carbohydrate { get; set; }

    [Required]
    public decimal Energy { get; set; }

    [Required]
    public decimal Fat { get; set; }

    [Required]
    public decimal Fiber { get; set; }

    [Required]
    public decimal Protein { get; set; }

    [Required]
    public decimal SaturatedFat { get; set; }

    [Required]
    public decimal Sugar { get; set; }

    [Required]
    public decimal? AlphaLinolenicAcid { get; set; }

    [Required]
    public decimal? LinoleicAcid { get; set; }

    // Minerals
    [Required]
    public decimal Chloride { get; set; }

    [Required]
    public decimal Sodium { get; set; }

    [Required]
    public decimal? Calcium { get; set; }

    [Required]
    public decimal? Chromium { get; set; }

    [Required]
    public decimal? Copper { get; set; }

    [Required]
    public decimal? Fluoride { get; set; }

    [Required]
    public decimal? Iodine { get; set; }

    [Required]
    public decimal? Iron { get; set; }

    [Required]
    public decimal? Magnesium { get; set; }

    [Required]
    public decimal? Manganese { get; set; }

    [Required]
    public decimal? Molybdenum { get; set; }

    [Required]
    public decimal? Phosphorus { get; set; }

    [Required]
    public decimal? Potassium { get; set; }

    [Required]
    public decimal? Selenium { get; set; }

    [Required]
    public decimal? Zinc { get; set; }

    // Vitamins
    [Required]
    public decimal? Biotin { get; set; }

    [Required]
    public decimal? Choline { get; set; }

    [Required]
    public decimal? Folate { get; set; }

    [Required]
    public decimal? Niacin { get; set; }

    [Required]
    public decimal? PantothenicAcid { get; set; }

    [Required]
    public decimal? Riboflavin { get; set; }

    [Required]
    public decimal? Thiamin { get; set; }

    [Required]
    public decimal? VitaminA { get; set; }

    [Required]
    public decimal? VitaminB12 { get; set; }

    [Required]
    public decimal? VitaminB6 { get; set; }

    [Required]
    public decimal? VitaminC { get; set; }

    [Required]
    public decimal? VitaminD { get; set; }

    [Required]
    public decimal? VitaminE { get; set; }

    [Required]
    public decimal? VitaminK { get; set; }
}
