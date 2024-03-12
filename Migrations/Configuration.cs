namespace OOP.Migrations
{
    using Faker;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity;
    using OOP.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<OOP.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(OOP.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            //dajcie tu jakiś warunek ktory się nie spełni bo  po update całą baze bedzie wypełniac (dane mogą się powtarzać) - szkoda zasobów :D
            if (true)
            {
                return;
            }

            var adresy = new List<Adres>();
            var users = new List<ApplicationUser>();
            var ankiety = new List<Ankieta>();
            var dzialy = new List<Dzial>();
  
            var komisje = new List<Komisja>();
            var ocena = new List<Ocena>();
            var polaAnkiet = new List<PoleAnkiety>();
            var osiagniecie = new List<Osiagniecie>();
            var pracaDyplomowe = new List<PracaDyplomowa>();
            var pracownicy = new List<Pracownik>();
            var stronaAnkieties = new List<StronaAnkiety>();

            string[] nazwydzialow = {"DSP",
                                     "DN",
                                     "BRiPM",
                                     "OWI",
                                     "Bds.Wm",
                                     "CSSDIR",
                                     "DJK",
                                     "DP",};
            string password = "Haslo123";
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            //adresy i konta
            for (int i = 0; i < 40; i++)
            {


                var adres1 = new Adres
                {
                    Ulica = LocationFaker.StreetName(),
                    Numer = LocationFaker.StreetNumber().ToString(),
                    KodPocztowy = LocationFaker.PostCode(),
                    Miasto = LocationFaker.City()
                };
                adresy.Add(adres1);

                var user = new ApplicationUser
                {
                    UserName = InternetFaker.Email(),
                    Email = InternetFaker.Email(),
                    Adres = adres1
                };
                users.Add(user);
            }
            //rektorom dzikanom pracownikom działom przypsanie konta
            for (int i = 0; i < 40; i++)
            {
              
                if (1 < i && i < 10)
                {
                    //8 działów
                    for (int j = 0; j < 8; j++)
                    {
                        var dzial = new Dzial
                        {
                            ApplicationUser = users[i],
                            Nazwa = nazwydzialow[j]
                        };
                        dzialy.Add(dzial);
                        i++;
                    }
                    continue;
                }
                else
                {
                    //reszta pracownicy 
                    var pracownik = new Pracownik
                    {
                        Imie = NameFaker.FemaleName(),
                        Nazwisko = NameFaker.FemaleFirstName(),
                        Stopien = "Magiser",
                        Tytul = " magister inżynier",
                        ApplicationUser = users[i]
                    };
                    pracownicy.Add(pracownik);
                }
            }
            //pare komisji trzeba  ale komisja nie zawiera pracowników a pracownik nie zwiera komisji


            //ankiety
            for (int i = 0; i < 28; i++)
            {
                if (i % 8 == 0)
                {
                    var ankieta = new Ankieta
                    {
                        Data = DateTimeFaker.DateTime(),
                        Pracownik = pracownicy[4]
                    };
                    ankiety.Add(ankieta);
                }
                else
                {
                    var ankieta = new Ankieta
                    {
                        Data = DateTimeFaker.DateTime(),
                        Pracownik = pracownicy[i]
                    };
                    ankiety.Add(ankieta);

                }
            }
            Random random = new Random();
            //strony
            for (int i = 0; i < 100; i++)
            {

                var stronaAnkiety = new StronaAnkiety
                {
                    Dzial = dzialy[random.Next(8)],
                    Ankieta = ankiety[random.Next(28)]
                };
                stronaAnkieties.Add(stronaAnkiety);
            }
            //polaAnkiet 
            for (int i = 0; i < 800; i++)
            {

                var poleAnkiety = new PoleAnkiety
                {
                    StronaAnkiety = stronaAnkieties[random.Next(100)],
                    LiczbaPunktow = random.Next(120),
                    Tresc = StringFaker.SelectFrom(50, "ABCDEFGHIJKLMNOPRSTUWXYZabcdefghijklmnoprstuwyxz")
                };
                polaAnkiet.Add(poleAnkiety);
            }



            foreach (Adres a in adresy)
            {
                context.Adresy.Add(a);

            }
            try
            {
                context.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Console.WriteLine($"Encja: {validationError.PropertyName} Error: {validationError.ErrorMessage} Adres");
                    }
                }
            }

            foreach (ApplicationUser u in users)
            {
                userManager.Create(u, password);
            }
            try
            {
                context.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Console.WriteLine($"Encja: {validationError.PropertyName} Error: {validationError.ErrorMessage} Users");
                    }
                }
            }
            foreach (Pracownik p in pracownicy)
            {
                context.Pracownicy.Add(p);
            }
            try
            {
                context.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Console.WriteLine($"Encja: {validationError.PropertyName} Error: {validationError.ErrorMessage} Pracownicy");
                    }
                }
            }
            
           
            foreach (Dzial d in dzialy)
            {
                context.Dzials.Add(d);
            }
            try
            {
                context.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Console.WriteLine($"Encja: {validationError.PropertyName} Error: {validationError.ErrorMessage} dzial");
                    }
                }
            }
            foreach (Ankieta a in ankiety)
            {
                context.Ankiety.Add(a);
            }
            try
            {
                context.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Console.WriteLine($"Encja: {validationError.PropertyName} Error: {validationError.ErrorMessage} Ankiety");
                    }
                }
            }
            foreach (StronaAnkiety s in stronaAnkieties)
            {
                context.StronyAnkiet.Add(s);
            }
            try
            {
                context.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Console.WriteLine($"Encja: {validationError.PropertyName} Error: {validationError.ErrorMessage} Strony");
                    }
                }
            }
            foreach (PoleAnkiety p in polaAnkiet)
            {
                context.PolaAnkiet.Add(p);
            }

            try
            {
                context.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Console.WriteLine($"Encja: {validationError.PropertyName} Error: {validationError.ErrorMessage} Pola");
                    }
                }
            }
        }
        //sól działa xd
        //gdzieś wywaa błąd że nazwa użytkownika jest zajęta w USers można sprobować wypierdolic wszystko z bazy danychi sprawdzić w siądzie abo faker troluje nie 
        //godzina 3:30 musze spać
    }

}
