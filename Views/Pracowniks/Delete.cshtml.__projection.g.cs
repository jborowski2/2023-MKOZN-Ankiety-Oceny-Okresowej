//------------------------------------------------------------------------------
// <auto-generated>
//     Ten kod zosta� wygenerowany przez narz�dzie.
//     Wersja wykonawcza:4.0.30319.42000
//
//     Zmiany w tym pliku mog� spowodowa� nieprawid�owe zachowanie i zostan� utracone, je�li
//     kod zostanie ponownie wygenerowany.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASP {
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Security;
using System.Web.UI;
using System.Web.WebPages;
using System.Web.WebPages.Html;

public class _Page_Delete_cshtml : System.Web.WebPages.WebPage {
private static object @__o;
#line hidden
public _Page_Delete_cshtml() {
}
protected System.Web.HttpApplication ApplicationInstance {
get {
return ((System.Web.HttpApplication)(Context.ApplicationInstance));
}
}
public override void Execute() {

#line 1 "C:\Users\zymci\AppData\Local\Temp\041847F49CAF1E1D93321192B254BF2C84EE\3\ocenaokresowapracownikow\Views\Pracowniks\Delete.cshtml"
__o = model;


#line default
#line hidden

#line 2 "C:\Users\zymci\AppData\Local\Temp\041847F49CAF1E1D93321192B254BF2C84EE\3\ocenaokresowapracownikow\Views\Pracowniks\Delete.cshtml"
  
    ViewBag.Title = "Usu�";
   


#line default
#line hidden

#line 3 "C:\Users\zymci\AppData\Local\Temp\041847F49CAF1E1D93321192B254BF2C84EE\3\ocenaokresowapracownikow\Views\Pracowniks\Delete.cshtml"
       __o = Html.DisplayNameFor(model => model.Imie);


#line default
#line hidden

#line 4 "C:\Users\zymci\AppData\Local\Temp\041847F49CAF1E1D93321192B254BF2C84EE\3\ocenaokresowapracownikow\Views\Pracowniks\Delete.cshtml"
       __o = Html.DisplayFor(model => model.Imie);


#line default
#line hidden

#line 5 "C:\Users\zymci\AppData\Local\Temp\041847F49CAF1E1D93321192B254BF2C84EE\3\ocenaokresowapracownikow\Views\Pracowniks\Delete.cshtml"
       __o = Html.DisplayNameFor(model => model.Nazwisko);


#line default
#line hidden

#line 6 "C:\Users\zymci\AppData\Local\Temp\041847F49CAF1E1D93321192B254BF2C84EE\3\ocenaokresowapracownikow\Views\Pracowniks\Delete.cshtml"
       __o = Html.DisplayFor(model => model.Nazwisko);


#line default
#line hidden

#line 7 "C:\Users\zymci\AppData\Local\Temp\041847F49CAF1E1D93321192B254BF2C84EE\3\ocenaokresowapracownikow\Views\Pracowniks\Delete.cshtml"
       __o = Html.DisplayNameFor(model => model.Stopien);


#line default
#line hidden

#line 8 "C:\Users\zymci\AppData\Local\Temp\041847F49CAF1E1D93321192B254BF2C84EE\3\ocenaokresowapracownikow\Views\Pracowniks\Delete.cshtml"
       __o = Html.DisplayFor(model => model.Stopien);


#line default
#line hidden

#line 9 "C:\Users\zymci\AppData\Local\Temp\041847F49CAF1E1D93321192B254BF2C84EE\3\ocenaokresowapracownikow\Views\Pracowniks\Delete.cshtml"
       __o = Html.DisplayNameFor(model => model.Tytul);


#line default
#line hidden

#line 10 "C:\Users\zymci\AppData\Local\Temp\041847F49CAF1E1D93321192B254BF2C84EE\3\ocenaokresowapracownikow\Views\Pracowniks\Delete.cshtml"
       __o = Html.DisplayFor(model => model.Tytul);


#line default
#line hidden

#line 11 "C:\Users\zymci\AppData\Local\Temp\041847F49CAF1E1D93321192B254BF2C84EE\3\ocenaokresowapracownikow\Views\Pracowniks\Delete.cshtml"
       __o = Html.DisplayNameFor(model => model.NumerTelefonu);


#line default
#line hidden

#line 12 "C:\Users\zymci\AppData\Local\Temp\041847F49CAF1E1D93321192B254BF2C84EE\3\ocenaokresowapracownikow\Views\Pracowniks\Delete.cshtml"
       __o = Html.DisplayFor(model => model.NumerTelefonu);


#line default
#line hidden

#line 13 "C:\Users\zymci\AppData\Local\Temp\041847F49CAF1E1D93321192B254BF2C84EE\3\ocenaokresowapracownikow\Views\Pracowniks\Delete.cshtml"
       __o = Html.DisplayNameFor(model => model.PrzelozonyID);


#line default
#line hidden

#line 14 "C:\Users\zymci\AppData\Local\Temp\041847F49CAF1E1D93321192B254BF2C84EE\3\ocenaokresowapracownikow\Views\Pracowniks\Delete.cshtml"
       __o = Html.DisplayFor(model => model.PrzelozonyID);


#line default
#line hidden

#line 15 "C:\Users\zymci\AppData\Local\Temp\041847F49CAF1E1D93321192B254BF2C84EE\3\ocenaokresowapracownikow\Views\Pracowniks\Delete.cshtml"
       __o = Html.DisplayNameFor(model => model.ApplicationUserID);


#line default
#line hidden

#line 16 "C:\Users\zymci\AppData\Local\Temp\041847F49CAF1E1D93321192B254BF2C84EE\3\ocenaokresowapracownikow\Views\Pracowniks\Delete.cshtml"
       __o = Html.DisplayFor(model => model.ApplicationUserID);


#line default
#line hidden

#line 17 "C:\Users\zymci\AppData\Local\Temp\041847F49CAF1E1D93321192B254BF2C84EE\3\ocenaokresowapracownikow\Views\Pracowniks\Delete.cshtml"
    using (Html.BeginForm()) {
        

#line default
#line hidden

#line 18 "C:\Users\zymci\AppData\Local\Temp\041847F49CAF1E1D93321192B254BF2C84EE\3\ocenaokresowapracownikow\Views\Pracowniks\Delete.cshtml"
   __o = Html.AntiForgeryToken();


#line default
#line hidden

#line 19 "C:\Users\zymci\AppData\Local\Temp\041847F49CAF1E1D93321192B254BF2C84EE\3\ocenaokresowapracownikow\Views\Pracowniks\Delete.cshtml"
                                

        

#line default
#line hidden

#line 20 "C:\Users\zymci\AppData\Local\Temp\041847F49CAF1E1D93321192B254BF2C84EE\3\ocenaokresowapracownikow\Views\Pracowniks\Delete.cshtml"
       __o = Html.ActionLink("Back to List", "Index");


#line default
#line hidden

#line 21 "C:\Users\zymci\AppData\Local\Temp\041847F49CAF1E1D93321192B254BF2C84EE\3\ocenaokresowapracownikow\Views\Pracowniks\Delete.cshtml"
              
    }

#line default
#line hidden
}
}
}
