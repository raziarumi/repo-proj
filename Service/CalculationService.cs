using Core.Models;
using Core.ViewModels;
using DataAccess;
using System;

namespace Service
{
    public class CalculationService
    {
        //private readonly ApplicationDataContext _dataContext;
        public string GetSummation(DataInputViewModel model)
        {
            string Summation = "";
            if (model.Number1.StartsWith("-") && model.Number2.StartsWith("-"))
            {
                Summation = GetSummationOfNumbers(model.Number1.Replace("-", ""), model.Number2.Replace("-", ""));
                Summation = "-" + Summation;
            }
            else if ((model.Number1.StartsWith("-") && !model.Number2.StartsWith("-")) || (!model.Number1.StartsWith("-") && model.Number2.StartsWith("-")))
            {
                Summation = GetDifference(model.Number1.Replace("-", ""), model.Number2.Replace("-", ""));
                if (Summation != "0" && isSmaller(model.Number1.Replace("-", ""), model.Number2.Replace("-", "")) && model.Number2.StartsWith("-"))
                {
                    Summation = "-" + Summation;
                }
                else if (Summation != "0" && !isSmaller(model.Number1.Replace("-", ""), model.Number2.Replace("-", "")) && model.Number1.StartsWith("-"))
                {
                    Summation = "-" + Summation;
                }
            }
            else
            {
                Summation = GetSummationOfNumbers(model.Number1, model.Number2);
            }

            return Summation;
        }


        private string GetDifference(string number1, string number2)
        {

            string num1Scale = number1;
            string num2Scale = number2;

            string num1precision = "";
            string num2precision = "";
            int carryOut = 0;
            string result1 = "";
            if (number1.Contains(".") || number2.Contains("."))
            {
                (num1Scale, num2Scale, num1precision, num2precision) = GetScaleAndRecision(number1, number2);

                if (isSmaller(num1Scale, num2Scale))
                    (result1, carryOut) = Difference(num2precision, num1precision, 0, false);

                else
                    (result1, carryOut) = Difference(num1precision, num2precision, 0, false);

                //result1 = ReveserseString(result1);
            }
            (string result, int carry) = Difference(num1Scale, num2Scale, carryOut);
            result = string.Format("{0}.{1}", result, result1);
            return result;
        }

        private bool isSmaller(string number1, string number2)
        {
            // Calculate lengths of both string 
            int n1 = number1.Length, n2 = number2.Length;
            if (n1 < n2)
            {
                number1 = number1.PadLeft(n2, '0');
            };
            if (n2 < n1)
            {

                number2 = number2.PadLeft(n1, '0');
            }

            for (int i = 0; i < number1.Length; i++)
                if (number1[i] < number2[i])
                    return true;
                else if (number1[i] > number2[i])
                    return false;

            return false;
        }
        private (string, int) Difference(string number1, string number2, int carry = 0, bool doReverse = true)
        {
            if (doReverse && isSmaller(number1, number2))
            {
                string temp = number1;
                number1 = number2;
                number2 = temp;
            }

            string str = "";

            int n1 = number1.Length, n2 = number2.Length;

            // Reverse both of strings 
            number1 = ReveserseString(number1);
            number2 = ReveserseString(number2);



            for (int i = 0; i < n2; i++)
            {
                int sub = ((int)(number1[i] - '0') -
                        (int)(number2[i] - '0') - carry);
                if (sub < 0)
                {
                    sub = sub + 10;
                    carry = 1;
                }
                else
                    carry = 0;

                str += (char)(sub + '0');
            }

            for (int i = n2; i < n1; i++)
            {
                int sub = ((int)(number1[i] - '0') - carry);

                if (sub < 0)
                {
                    sub = sub + 10;
                    carry = 1;
                }
                else
                    carry = 0;

                str += (char)(sub + '0');
            }
            str = ReveserseString(str);
            return (str, carry);
        }
        private string GetSummationOfNumbers(string number1, string number2)
        {
            string num1Scale = number1;
            string num2Scale = number2;

            string num1precision = "";
            string num2precision = "";
            int carryOut = 0;
            string result1 = "";
            if (number1.Contains(".") || number2.Contains("."))
            {
                (num1Scale, num2Scale, num1precision, num2precision) = GetScaleAndRecision(number1, number2);

                (result1, carryOut) = Sum(num1precision, num2precision);

                result1 = ReveserseString(result1);
            }
            (string result, int carry) = Sum(num1Scale, num2Scale, carryOut);

            if (carry > 0)
                result += (char)(carry + '0');


            result = ReveserseString(result);
            if (result1 != "")
            {
                result = string.Format("{0}.{1}", result, result1);

            }
            return result;
        }

        private static (string, string, string, string) GetScaleAndRecision(string number1, string number2)
        {
            string num1Scale = number1; string num2Scale = number2; string num1precision = ""; string num2precision = "";
            var arr = number1.Split('.');
            num1Scale = arr[0];
            if (arr.Length > 1)
                num1precision = arr[1];

            var arr2 = number2.Split('.');
            num2Scale = arr2[0];
            if (arr2.Length > 1)
                num2precision = arr2[1];

            if (num1precision.Length < num2precision.Length)
            {
                num1precision = num1precision.PadRight(num2precision.Length, '0');
            };
            if (num2precision.Length < num1precision.Length)
            {
                num2precision = num2precision.PadRight(num1precision.Length, '0');
            }
            return (num1Scale, num2Scale, num1precision, num2precision);
        }

        private (string, int) Sum(string number1, string number2, int carry = 0)
        {
            string result = "";
            if (isSmaller(number2, number1))
            {
                string temp = number1;
                number1 = number2;
                number2 = temp;
            }
            number1 = ReveserseString(number1);
            number2 = ReveserseString(number2);

            for (int i = 0; i < number1.Length; i++)
            {
                int sum = ((int)(number1[i] - '0') +
                        (int)(number2[i] - '0') + carry);
                result += (char)(sum % 10 + '0');

                carry = sum / 10;
            }
            for (int i = number1.Length; i < number2.Length; i++)
            {
                int sum = ((int)(number2[i] - '0') + carry);
                result += (char)(sum % 10 + '0');
                carry = sum / 10;
            }
            //if (carry > 0)
            //    result += (char)(carry + '0');
            //result = ReveserseString(result);
            return (result, carry);
        }
        private string ReveserseString(string str)
        {
            char[] ch = str.ToCharArray();
            Array.Reverse(ch);
            str = new string(ch);
            return str;
        }
    }
}
