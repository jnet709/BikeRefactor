using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BikeDistributor
{
    public class Order
    {
        private const double TaxRate = .0725d;
        private readonly IList<Line> _lines = new List<Line>();

        public Order(string company)
        {
            Company = company;
        }

        public string Company { get; private set; }

        public void AddLine(Line line)
        {
            _lines.Add(line);
        }

        /// <summary>
        /// Kevin Nguyen's notes: the codes for this method is modified and shortened
        /// </summary>
        /// <returns></returns>
        public string Receipt()
        {
            var result = getLineInfo(false);
            return result;
        }

        /// <summary>
        /// Kevin Nguyen's notes: the codes for this method is modified and shortened
        /// </summary>
        /// <returns></returns>
        public string HtmlReceipt()
        {
            var result = getLineInfo(true);
            return result;
        }


        #region help methods

        /// <summary>
        /// Kevin Nguyen's notes: This is a new common method for the "Receipt()" and "HtmlReceipt()" methods.
        /// 
        /// Logic of the 2 methods are moved here with midofcations.
        /// 
        /// 1. Refactor the "foreach" logic and "switch" logic that is repeated in the "Receipt()" and "HtmlReceipt()" methods.
        /// 2. Refactor concatinating sub-string that is repeated in the 2 methods for a lineright after the "switch" statement. 
        /// 3. Reduce if/else in the switch statement by using the “?:”  operator
        /// 
        /// I assume that the receipt is generated if there exists a line
        /// </summary>
        /// <param name="isHtmlFormat"> true if the returned string is expected to have HTML tags in it</param>
        /// <returns>returned string</returns>
        private string getLineInfo(bool isHtmlFormat)
        {
            // I assume that the receipt is generated if there is at least one line
            if (!_lines.Any())
            {
                return string.Empty;
            }
            
            var totalAmount = 0d;
            string lineInfoStr;
            // main task begins
            var result = isHtmlFormat ? new StringBuilder(string.Format("<html><body><h1>Order Receipt for {0}</h1>", Company))
                                      : new StringBuilder(string.Format("Order Receipt for {0}{1}", Company, Environment.NewLine));

            if (isHtmlFormat)
            {
                result.Append("<ul>");
            }

            foreach (var line in _lines)
            {
                var thisAmount = 0d;
                switch (line.Bike.Price)
                {
                    case Bike.OneThousand:
                        thisAmount += (line.Quantity >= 20) ? line.Quantity*line.Bike.Price*.9d : line.Quantity*line.Bike.Price;
                        break;
                    case Bike.TwoThousand:
                        thisAmount += (line.Quantity >= 10) ? line.Quantity*line.Bike.Price*.8d : line.Quantity*line.Bike.Price;
                        break;
                    case Bike.FiveThousand:
                        thisAmount += (line.Quantity >= 5) ? line.Quantity * line.Bike.Price * .8d : line.Quantity*line.Bike.Price;
                        break;
                }

                lineInfoStr = string.Format("{0} x {1} {2} = {3}", line.Quantity, line.Bike.Brand, line.Bike.Model, thisAmount.ToString("C"));
                if (isHtmlFormat)
                {
                    result.Append(string.Format("<li>{0}</li>", lineInfoStr));
                }
                else
                {
                    result.AppendLine(string.Format("\t{0}", lineInfoStr));
                }

                totalAmount += thisAmount;
            }

            var tax = totalAmount * TaxRate;
            var subTotalStr = string.Format("Sub-Total: {0}", totalAmount.ToString("C"));
            var taxStr = string.Format("Tax: {0}", tax.ToString("C"));
            var totalStr = string.Format("Total: {0}", (totalAmount + tax).ToString("C"));

            if (isHtmlFormat)
            {
                result.Append("</ul>");
                result.Append(string.Format("<h3>{0}</h3>", subTotalStr));
                result.Append(string.Format("<h3>{0}</h3>", taxStr));
                result.Append(string.Format("<h2>{0}</h2>", totalStr));
                result.Append("</body></html>");
            }
            else
            {
                result.AppendLine(string.Format("{0}", subTotalStr));
                result.AppendLine(string.Format("{0}", taxStr));
                result.Append(string.Format("{0}", totalStr));
            }  
            
            return result.ToString();
        }

        #endregion

    }
}
