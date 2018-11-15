﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using HtmlAgilityPack;
using System.Text;
using System.Threading.Tasks;

namespace KrakScaper
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Indtast: Navn, Nummer eller adresse");
            GetHTMLAsync();
            Console.ReadKey();
        }

        private static async void GetHTMLAsync()
        {
            //Search Input Using krak.dk
            string q = Console.ReadLine();
            var url = "https://www.krak.dk/" + q +"/personer";
            Console.Clear();

           
            // Using the http client with Async
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

            // Load the html document
            var HtmlDocument = new HtmlDocument();
            HtmlDocument.LoadHtml(html);

            // a list of every result
            var NameList = HtmlDocument.DocumentNode.Descendants("ul")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("ResultList")).ToList();

            // Single person added to list
            var NameListPeople = NameList[0].Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("PersonResultListItem")).ToList();

            
            // for each person in our NameListPeople
            foreach (var person in NameListPeople)
            {
                // Name
                Console.WriteLine(person.Descendants("div")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("personName")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t'));


                // Address
                Console.WriteLine(person.Descendants("div")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("address")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t'));

                // Phone Number.. using a try catch 
                try
                {    
                    Console.WriteLine(person.Descendants("div")
                    .Where(node => node.GetAttributeValue("role", "")
                    .Equals("button")).FirstOrDefault().InnerText.Trim('\r', '\n', '\t').Substring(0,11));
                }
                catch (Exception)
                {
                    Console.WriteLine();
                    continue;
                }

                

                Console.WriteLine();
            }



        }
    }
}
