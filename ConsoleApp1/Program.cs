using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace ConsoleAppTestValidation
{

    public class MyExampleClass
    {
        [MaxLength(10), MinLength(1)]
        public string Name { get; set; } = null!;

        [Required]
        public string StringName2 { get; set; } = null!;

        [Required]
        public List<ModelTest> ThingsList { get; set; } = null!;
    }

    public class ModelTest
    {
        [Required]
        public string Things = null!;

        [Required]
        public List<ModelTest2> ThingsList2 { get; set; } = null!;

    }

    public class ModelTest2
    {
        [MaxLength(10), MinLength(1)]
        public string Things2 { get; set; } = null!;

    }


    class Program
    {
        static void Main(string[] args)
        {
            List<ModelTest2> itemsList2 = new List<ModelTest2> { new ModelTest2 { Things2 = "01234fefefefefefef5678999" } };

            List<MyExampleClass> myClasses = new List<MyExampleClass>
            {
                new MyExampleClass
                {
                    Name="test1",
                    StringName2 = "A",
                    ThingsList = new List<ModelTest>
                    {
                        new ModelTest { Things = "555" ,ThingsList2 = itemsList2 } ,
                    },
                },
                 new MyExampleClass
                {
                    Name="test2",
                    StringName2 = "A",
                },

            };

            Console.WriteLine("******* Validation *******");
            var errorListStr = validateWithValue(myClasses);

            //error string
            var errorFinalStr = convertArrayToString(errorListStr);
            Console.WriteLine(errorFinalStr);

            Console.ReadKey();
           
        }

        //logic validate
        static List<string> validateWithValue(List<MyExampleClass> data)
        {
            var errorListStr = new ArrayList();
            foreach (var item in data)
            {
                var msg = Validate(item);
                if (msg != "")
                {
                    errorListStr.Add(msg);
                }

                if (item.ThingsList is null) continue;
                foreach (var item2 in item.ThingsList)
                {
                    var msg2 = Validate(item2);
                    if (msg2 != "")
                    {
                        errorListStr.Add(msg2);
                    }
                    if (item2.ThingsList2 is null) continue;
                    foreach (var item3 in item2.ThingsList2)
                    {
                        var msg3 = Validate(item3);
                        if (msg3 != "")
                        {
                            errorListStr.Add(msg3);
                        }
                    }

                }
            }

            List<string> lst = errorListStr.OfType<string>().ToList();
            return lst;
        }

        //convert list to string
        static string convertArrayToString(List<String> data)
        {
            var stringStr = "";
            foreach (var item in data)
            {
                stringStr += item + " | ";
            }
            return stringStr;
        }

        //method for validate
        static string Validate<T>(T item)
        {
            if (item ==null) return "";
            var context = new ValidationContext(item, serviceProvider: null, items: null);
            var errorResults = new List<ValidationResult>();

            //Perform validation
            var isValid = Validator.TryValidateObject(item, context, errorResults, true);
            var isValidStr = isValid ? "Valid" : "Not-Valid";

            IEnumerable<string>? errorMessages = errorResults?.Select(x => x.ToString());
            if (errorMessages != null && !isValid)
            {
                var errorListStr = string.Join("; ", errorMessages);
                //Console.WriteLine(isValidStr + "(" + errorListStr + ")");
                return isValidStr + "(" + errorListStr + ")";
            }
            return "";
        }
    }
}