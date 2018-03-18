using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace GroceryShark.iOS
{
    public static class WalmartSearch
    {

        private class Attributes
        {
            public string paperWoodIndicator { get; set; }
            public string replenishmentEndDate { get; set; }
            public string size { get; set; }
        }

        private class ImageEntity
        {
            public string thumbnailImage { get; set; }
            public string mediumImage { get; set; }
            public string largeImage { get; set; }
            public string entityType { get; set; }
        }

        private class RootObject
        {
            public int itemId { get; set; }
            public int parentItemId { get; set; }
            public string name { get; set; }
            public string msrp { get; set; }
            public string salePrice { get; set; }
            public string upc { get; set; }
            public string categoryPath { get; set; }
            public string shortDescription { get; set; }
            public string longDescription { get; set; }
            public string brandName { get; set; }
            public string thumbnailImage { get; set; }
            public string mediumImage { get; set; }
            public string largeImage { get; set; }
            public string productTrackingUrl { get; set; }
            public string size { get; set; }
            public string color { get; set; }
            public string modelNumber { get; set; }
            public string productUrl { get; set; }
            public string customerRating { get; set; }
            public int numReviews { get; set; }
            public string customerRatingImage { get; set; }
            public string categoryNode { get; set; }
            public bool bundle { get; set; }
            public bool clearance { get; set; }
            public bool preOrder { get; set; }
            public string stock { get; set; }
            public Attributes attributes { get; set; }
            public string addToCartUrl { get; set; }
            public string affiliateAddToCartUrl { get; set; }
            public bool freeShippingOver50Dollars { get; set; }
            public List<ImageEntity> imageEntities { get; set; }
            public string offerType { get; set; }
            public bool isTwoDayShippingEligible { get; set; }
            public bool availableOnline { get; set; }

        }

        private static void finditem(string item, List<item> list)
        {

            var client = new WebClient();
            string apisite = "http://api.walmartlabs.com/v1/items/";
            string apikey = "sh8syn5xak8gafnvxvwzstyg";
            RootObject obj = null;


            try
            {

                var response = client.DownloadString(apisite + item + "?format=json&apiKey=" + apikey);
                obj = JsonConvert.DeserializeObject<RootObject>(response);

            }
            catch
            {
                Console.WriteLine("Catch");

            }

            string msrp;

            if (obj != null)
            {

                if (obj.msrp != "")
                {
                    msrp = Convert.ToString(obj.msrp);
                }
                else
                {

                    msrp = "";

                }

                list.Add(new item(Convert.ToString(obj.itemId),obj.name,obj.shortDescription,msrp,""));

            }

            //list.Add(new item(Convert.ToString(obj.itemId),obj.name,obj.shortDescription,msrp,""));

        }


        public static List<item> Search(string key = "huntz")
        {

            const string site = "https://www.walmart.com/search/?query=";
            List<item> searchresults = new List<item>();
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(site + key + "&stores=1935&cat_id=976759");
            var htmlNodes = doc.DocumentNode.SelectNodes(xpath: "//div[@class='display-inline-block pull-left prod-ProductCard--Image']/a");
            var othernodes = doc.DocumentNode.SelectNodes(xpath: "//div[@class='search-result-product-title gridview']/span/a[@class='product-title-link']/span");

            if (othernodes == null)
            {


                othernodes = doc.DocumentNode.SelectNodes(xpath: "//div[@class='search-result-product-title listview']/span/a[@class='product-title-link']/span");

            }

            try
            {

                for (int i = 0; i < othernodes.Count; i++)
                {
                    string[] split;
                    string idnum = "";
                    String output = htmlNodes[i].Attributes["href"].Value;
                    Console.WriteLine(output);
                    split = output.ToString().Split('/');

                    if (split[3].Contains("?"))
                    {

                        idnum = split[3].Split('?')[0];

                    }
                    else
                    {

                        idnum = split[3];

                    }

                    String output2 = othernodes[i].InnerText;


                    searchresults.Add(new item(id: idnum, name: output2));


                }

            }catch{}

            return searchresults;


        }

    }
}
