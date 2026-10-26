using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(-1, 2, true, Description = "Отрицательная длина целой части")]
    [TestCase(-1, 2, false, Description = "Отрицательная длина целой части без знака")]
    public void Constructor_WithInvalidIntegerLength_ShouldThrowArgumentException(
        int integerLength, int fractionLength, bool allowSign)
    {
        Assert.Throws<ArgumentException>(() =>
            new NumberValidator(integerLength, fractionLength, allowSign));
    }

    [TestCase(1, 0, true, Description = "Валидные параметры со знаком")]
    [TestCase(1, 0, false, Description = "Валидные параметры без знака")]
    [TestCase(17, 2, true, Description = "Большие значения параметров")]
    public void Constructor_WithValidParameters_ShouldNotThrow(
        int integerLength, int fractionLength, bool allowSign)
    {
        Assert.DoesNotThrow(() =>
            new NumberValidator(integerLength, fractionLength, allowSign));
    }


    [TestCase("0.0", 17, 2, true, true, Description = "Простой ноль с дробной частью")]
    [TestCase("0", 17, 2, true, true, Description = "Простой ноль без дробной части")]
    [TestCase("+1.23", 4, 2, true, true, Description = "Положительное число со знаком, достаточная длина")]

    [TestCase("00.00", 3, 2, true, false, Description = "Ведущий ноль в целой части")]
    [TestCase("-0.00", 3, 2, true, false, Description = "Отрицательный ноль")]
    [TestCase("+0.00", 3, 2, true, false, Description = "Положительный ноль со знаком")]
    [TestCase("+1.23", 3, 2, true, false, Description = "Число со знаком, но недостаточная длина целой части")]
    [TestCase("0.000", 17, 2, true, false, Description = "Слишком много знаков после запятой")]
    [TestCase("-1.23", 3, 2, true, false, Description = "Отрицательное число (знак не разрешен)")]
    [TestCase("a.sd", 3, 2, true, false, Description = "Некорректный формат")]
    public void IsValidNumber_ShouldReturnExpectedResult(
        string input,
        int integerLength,
        int fractionLength,
        bool allowSign,
        bool expectedResult)
    {
        var validator = new NumberValidator(integerLength, fractionLength, allowSign);

        Assert.That(validator.IsValidNumber(input), Is.EqualTo(expectedResult));
    }

}