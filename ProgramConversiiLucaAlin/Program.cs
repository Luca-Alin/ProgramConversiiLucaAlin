namespace ProgramConversiiLucaAlin
{
        public class HelloWorld
        {
            private static String _initialNumber = String.Empty;
            private static int _initialBase;
            private static int _finalBase;
            private static readonly char[] IndexTable =     //Index table este folosit pentru a transforma litere in numere
            {                                               //de exemplu litera 'D' == 13
                '0', '1', '2', '3',                         //din obtinem valoarea numerica a lui 'D' cu expresia de mai jos
                '4', '5', '6', '7',                         //Array.IndexOf(IndexTable, 'D') == 13
                '8', '9', 'A', 'B',
                'C', 'D', 'E', 'F'
            };
            private static long _wholeNumberInBase10 = 0;   
            private static double _decimalNumberInBase10 = 0;

            public static void Main(string[] args)
            {
                ReadUserInput();
                ConvertWholeNumberToBase10();
                ConvertDecimalNumberToBase10();
                ConvertNumberToFinalBase();
            }

            private static void ReadUserInput()
            {
                Console.WriteLine("Introduceti un numar (virgula se reprezinta cu \".\"): ");
                _initialNumber = Console.ReadLine();

                int countOfCommas = 0;
                for (int i = 0; i < _initialNumber.Length; i++)
                    if (_initialNumber[i] == '.')
                        countOfCommas++;
                if (countOfCommas >= 2) throw new Exception("Prea multe virgule");

                Console.WriteLine("Introduceti baza numarului: ");
                _initialBase = Convert.ToInt32(Console.ReadLine());
                if (_initialBase < 2 || _initialBase > 16) throw new Exception("Baza gresita");

                for (int i = 0; i < _initialNumber.Length; i++)
                    if (_initialNumber[i] != '.')
                        if (Array.IndexOf(IndexTable, _initialNumber[i]) == -1 ||
                            Array.IndexOf(IndexTable, _initialNumber[i]) >= _initialBase)
                            throw new Exception($"Numarul nu poate fi reprezentat in baza {_initialBase}");

                Console.WriteLine("Introduceti baza in care trebuie transformat: ");
                _finalBase = Convert.ToInt32(Console.ReadLine());
                if (_finalBase < 2 || _finalBase > 16) throw new Exception("Baza gresita");
            }

            ///<summary>
            ///     Transforma partea intreaga a numarului
            /// introdus de utilizator in baza 10, aceasta informatie fiind
            /// stocata in variabila "_wholeNumberInBase10"
            ///</summary>
            public static void ConvertWholeNumberToBase10()
            {

                
                String wholeNumber = _initialNumber.Split('.')[0];

                int power = wholeNumber.Length - 1;
                for (int i = 0; i < wholeNumber.Length; i++)
                    _wholeNumberInBase10 += (long)Math.Pow(_initialBase, power--) * Array.IndexOf(IndexTable, wholeNumber[i]);

            }


            ///<summary>
            ///     Transforma partea zecimala a numarului
            /// introdus de utilizator in baza 10, initial sub forma de fractie,
            /// apoi numarul este transformat in format zecimal cu ajutorul unui algoritm
            /// pentru fractii periodice.
            /// Variabila "_decimalNumberInBase10" retine partea zecimala
            ///</summary>
            private static void ConvertDecimalNumberToBase10()
            {
                
                String decimalNumber = String.Empty;
                try
                { 
                    decimalNumber = _initialNumber.Split('.')[1];
                }
                catch
                {
                    
                }

                /*reprezentam partea fractionara a numarului initial sub forma de fractie
                 *folosind doua liste, una pentru numarator, alta pentru numitor
                 */
                List<int> nominators = new List<int>();
                List<int> denominators = new List<int>();
                int power = 1;
                for (int i = 0; i < decimalNumber.Length; i++)
                {
                    nominators.Add(int.Parse(decimalNumber[i].ToString()));
                    denominators.Add((int)Math.Pow(_initialBase, power++));
                }

                int nominatorsSum = 0;
                int commonDenominator = (int)Math.Pow(_initialBase, --power);

                for (int i = 0; i < nominators.Count; i++)
                    nominatorsSum += nominators[i] * (commonDenominator / denominators[i]);

                //algoritm de impartire a fractiilor, ce detecteaza fractii periodice
                List<int> quotientsList = new List<int>();
                List<int> remaindersList = new List<int>();

                int remainder = nominatorsSum % commonDenominator;
                remaindersList.Add(remainder);
                bool periodic = false;
                do
                {
                    quotientsList.Add(remainder * 10 / commonDenominator);
                    remainder = remainder * 10 % commonDenominator;

                    if (!remaindersList.Contains(remainder)) remaindersList.Add(remainder);
                    else periodic = true;
                } while (remainder != 0 && periodic == false);

                String dnib10 = "0.";
                if (!periodic)
                    foreach (int i in quotientsList)
                        dnib10 += i;
                else
                    for (int i = 0; i < quotientsList.Count; i++)
                        dnib10 += quotientsList[i];
                
                
                _decimalNumberInBase10 = double.Parse(dnib10);
            }

            /// <summary>
            ///     Transforma variabilele "_wholeNumberInBase10" si "_decimalNumberInBase10"
            /// in baza introdusa de utilizator
            /// </summary>
            public static void ConvertNumberToFinalBase()
            {

                String finalWholeNumber = String.Empty;
                
                do
                {
                    finalWholeNumber = IndexTable[(int)_wholeNumberInBase10 % _finalBase].ToString() + finalWholeNumber;
                    _wholeNumberInBase10 /= _finalBase;
                } while (_wholeNumberInBase10 > 0);

                

                
                String finalDecimalNumber = String.Empty;
                int length = _decimalNumberInBase10.ToString().Length;
                double lastRemainder = 0;
                for (int i = 0; i < length; i++)
                {
                    _decimalNumberInBase10 *= _finalBase;
                    finalDecimalNumber += Math.Floor(_decimalNumberInBase10);
                    _decimalNumberInBase10 = _decimalNumberInBase10 - Math.Floor(_decimalNumberInBase10);

                    if (_decimalNumberInBase10 - 0.05 <= lastRemainder && _decimalNumberInBase10 + 0.05 >= lastRemainder) break;
                    lastRemainder = _decimalNumberInBase10;
                }

                String finalNumber = $"{finalWholeNumber}.{finalDecimalNumber}";
                while (finalNumber.Length > 2 && finalNumber[finalNumber.Length - 1] == '0')
                    finalNumber = finalNumber.Substring(0, finalNumber.Length - 2);

                if (finalNumber[finalNumber.Length - 1] == '.')
                    finalNumber = finalNumber.Substring(0, finalNumber.Length - 2);
                
                Console.WriteLine($"{_initialNumber} (baza {_initialBase}) => {finalNumber} (baza {_finalBase})");
            }
        }    
}    