namespace QA.AutomatedMagic.Framework.TestContextItems.Dynamic.ValueGeneration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using MetaMagic;

    [MetaType("String generator")]
    public class StringGenerator : BaseValueGenerator
    {
        [MetaTypeValue("String generator type", IsRequired = false)]
        public StringGeneratorType Type { get; set; } = StringGeneratorType.Mixed;

        [MetaTypeValue("Generated string length", IsRequired = false)]
        public int Length { get; set; } = 5;

        public override object GenerateValue()
        {
            var sb = new StringBuilder();
            switch (Type)
            {
                case StringGeneratorType.Chars:

                    for (int i = 0; i < Length; i++)
                    {
                        if (_random.Next(0, 100) % 2 == 0)
                            sb.Append((char)_random.Next(65, 91));
                        else
                            sb.Append((char)_random.Next(97, 123));
                    }

                    break;
                case StringGeneratorType.Digits:

                    for (int i = 0; i < Length; i++)
                    {
                        sb.Append(_random.Next(0, 10));
                    }

                    break;
                case StringGeneratorType.Mixed:

                    while (sb.Length < Length)
                    {
                        var guid = Guid.NewGuid().ToString().Replace("-", "");
                        var neededCharsCount = Length - sb.Length;

                        if (neededCharsCount >= guid.Length)
                            sb.Append(guid);
                        else
                            sb.Append(guid.Substring(0, neededCharsCount));
                    }

                    break;
                default:
                    break;
            }

            return sb.ToString();
        }

        public enum StringGeneratorType
        {
            Chars, Digits, Mixed
        }
    }
}
