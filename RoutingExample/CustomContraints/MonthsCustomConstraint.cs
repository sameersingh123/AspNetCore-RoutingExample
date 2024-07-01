
using System.Text.RegularExpressions;

namespace RoutingExample.CustomContraints
{

    //Eg: sales-report/2020/apr is an incoming request
    public class MonthsCustomConstraint : IRouteConstraint
    {
        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            //check whether value exists
            if (!values.ContainsKey(routeKey))      //(routeKey= month)
            {
                return false;  //not a match
            }
            Regex regex = new Regex("^(apr|jul|oct|jan)$");
            string? monthValue = Convert.ToString(values[routeKey]);

            if(regex.IsMatch(monthValue))
            {
                return true;  //its a match
            }
            return false;
        }
    }
}
